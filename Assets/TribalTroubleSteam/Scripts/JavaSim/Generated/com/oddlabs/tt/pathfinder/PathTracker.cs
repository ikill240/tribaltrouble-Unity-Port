// Translated from PathTracker.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.pathfinder
{
    // Java: 18
    public class PathTracker : java.lang.Object
    {
        // Java: 19
        public sealed class State : java.lang.Enum
        {
            public static State[] values() { return (State[])VALUES_.Clone(); }
            public static State valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly State OK = new State("OK", 0);
            public static readonly State OK_INTERRUPTIBLE = new State("OK_INTERRUPTIBLE", 1);
            public static readonly State DONE = new State("DONE", 2);
            public static readonly State SOFTBLOCKED = new State("SOFTBLOCKED", 3);
            public static readonly State BLOCKED = new State("BLOCKED", 4);
            public static readonly State[] VALUES_ = new State[] { OK, OK_INTERRUPTIBLE, DONE, SOFTBLOCKED, BLOCKED };
            public State(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        public const int REGION_SEARCH_TRIES = 4;
        public com.oddlabs.tt.util.BezierPath bezier_path;
        public com.oddlabs.tt.pathfinder.UnitGrid unit_grid;
        public com.oddlabs.tt.pathfinder.Movable unit;
        public com.oddlabs.tt.pathfinder.RegionNode region_path;
        public com.oddlabs.tt.pathfinder.Region target_region;
        public com.oddlabs.tt.pathfinder.GridPathNode grid_path;
        public com.oddlabs.tt.pathfinder.Occupant current_blocker;
        public bool deadlock_mark;
        public int next_unit_grid_x;
        public int next_unit_grid_y;
        public com.oddlabs.tt.pathfinder.TrackerAlgorithm tracker_algorithm;
        public bool initial_path;
        public com.oddlabs.tt.pathfinder.PathTracker.State state;
        // Java: 52
        public void appendToolTip(com.oddlabs.tt.gui.ToolTipBox tool_tip_box)
        {
            tool_tip_box.append(" next_x=");
            tool_tip_box.append(next_unit_grid_x);
            tool_tip_box.append(" next_y=");
            tool_tip_box.append(next_unit_grid_y);
        }
        // Java: 59
        public com.oddlabs.tt.pathfinder.PathTracker.State animate(float speed)
        {
            doAnimate(speed);
            if (((object)state != (object)com.oddlabs.tt.pathfinder.PathTracker.State.SOFTBLOCKED) && ((object)state != (object)com.oddlabs.tt.pathfinder.PathTracker.State.BLOCKED))
            {
                current_blocker = null;
            }
            return state;
        }
        // Java: 67
        public void doAnimate(float speed)
        {
            if (bezier_path.isDone())
            {
                if (initial_path)
                {
                    state = initPath();
                    if (((object)state != (object)com.oddlabs.tt.pathfinder.PathTracker.State.OK))
                    {
                        return;
                    }
                    initial_path = false;
                }
                if (done(unit.getGridX(), unit.getGridY()))
                {
                    state = com.oddlabs.tt.pathfinder.PathTracker.State.DONE;
                    return;
                }
                state = lookAhead();
                if (((object)state != (object)com.oddlabs.tt.pathfinder.PathTracker.State.OK))
                {
                    if (((object)state == (object)com.oddlabs.tt.pathfinder.PathTracker.State.SOFTBLOCKED) || ((object)state == (object)com.oddlabs.tt.pathfinder.PathTracker.State.BLOCKED))
                    {
                        if (checkDeadlock())
                        {
                            state = com.oddlabs.tt.pathfinder.PathTracker.State.OK;
                            return;
                        }
                    }
                    return;
                }
                else
                {
                    unit.free();
                    advance();
                }
            }
            bezier_path.computeCurvePoint(speed);
            update();
            state = (bezier_path.isDone() ? com.oddlabs.tt.pathfinder.PathTracker.State.OK_INTERRUPTIBLE : com.oddlabs.tt.pathfinder.PathTracker.State.OK);
        }
        // Java: 99
        public com.oddlabs.tt.pathfinder.Occupant getBlocker()
        {
            return current_blocker;
        }
        // Java: 103
        public bool checkDeadlock()
        {
            com.oddlabs.tt.pathfinder.PathTracker start = findDeadlock();
            if (start != null)
            {
                start.solveDeadlock();
                return true;
            }
            else
            {
                return false;
            }
        }
        // Java: 112
        public com.oddlabs.tt.pathfinder.PathTracker getNextDeadlocked()
        {
            com.oddlabs.tt.pathfinder.Occupant occupant = getNextOccupantUnchecked();
            if (((object)occupant != (object)unit) && (occupant is com.oddlabs.tt.pathfinder.Movable next) && next.getTracker() != null)
            {
                if (next.isMoving() && (((object)next.getTracker().state == (object)com.oddlabs.tt.pathfinder.PathTracker.State.SOFTBLOCKED) || ((object)next.getTracker().state == (object)com.oddlabs.tt.pathfinder.PathTracker.State.BLOCKED)))
                {
                    return next.getTracker();
                }
            }
            return null;
        }
        // Java: 124
        public com.oddlabs.tt.pathfinder.PathTracker findDeadlock()
        {
            com.oddlabs.tt.pathfinder.PathTracker current = this;
            com.oddlabs.tt.pathfinder.PathTracker result = null;
            while (current != null)
            {
                if (current.deadlock_mark)
                {
                    result = current;
                    break;
                }
                current.deadlock_mark = true;
                current = current.getNextDeadlocked();
            }
            current = this;
            while (current != null && current.deadlock_mark)
            {
                current.deadlock_mark = false;
                current = current.getNextDeadlocked();
            }
            return result;
        }
        // Java: 145
        public void solveDeadlock()
        {
            com.oddlabs.tt.pathfinder.Movable current = unit;
            current.free();
            while (current != null)
            {
                com.oddlabs.tt.pathfinder.Movable next = ((com.oddlabs.tt.pathfinder.Movable)current.getTracker().getNextOccupant());
                if (next != null)
                {
                    next.free();
                }
                current.getTracker().advance();
                current = next;
            }
        }
        // Java: 158
        public void advance()
        {
            unit.setGridPosition(next_unit_grid_x, next_unit_grid_y);
            unit.occupy();
            findNextDirection();
        }
        // Java: 164
        public com.oddlabs.tt.pathfinder.Occupant getNextOccupantUnchecked()
        {
            return unit_grid.getOccupant(next_unit_grid_x, next_unit_grid_y);
        }
        // Java: 168
        public com.oddlabs.tt.pathfinder.Occupant getNextOccupant()
        {
            com.oddlabs.tt.pathfinder.Occupant occ = getNextOccupantUnchecked();
            return occ;
        }
        // Java: 174
        public void update()
        {
            unit.setPosition(bezier_path.getCurrentX(), bezier_path.getCurrentY());
            unit.setDirection(bezier_path.getCurrentDirectionX(), bezier_path.getCurrentDirectionY());
        }
        // Java: 179
        public bool done(int x, int y)
        {
            return tracker_algorithm.isDone(x, y);
        }
        // Java: 183
        public void findNextDirection()
        {
            if (grid_path == null || done(next_unit_grid_x, next_unit_grid_y))
            {
                bezier_path.endPath();
                initial_path = true;
                return;
            }
            com.oddlabs.tt.pathfinder.DirectionNode dir_node = grid_path.getDirection();
            grid_path = ((com.oddlabs.tt.pathfinder.GridPathNode)grid_path.getParent());
            next_unit_grid_x += dir_node.getDirectionX();
            next_unit_grid_y += dir_node.getDirectionY();
            float next_node_x = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(next_unit_grid_x);
            float next_node_y = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(next_unit_grid_y);
            bezier_path.nextPoint(dir_node.getInvLength(), next_node_x, next_node_y);
        }
        // Java: 198
        public void checkRegionPath(int src_x, int src_y)
        {
            com.oddlabs.tt.pathfinder.Region current_region = unit_grid.getRegion(src_x, src_y);
            if (target_region != null && tracker_algorithm.acceptRegion(target_region))
            {
                while (region_path != null)
                {
                    com.oddlabs.tt.pathfinder.Region region_path_region = region_path.getRegion();
                    if (((object)current_region == (object)region_path_region))
                    {
                        return;
                    }
                    region_path = ((com.oddlabs.tt.pathfinder.RegionNode)region_path.getParent());
                }
            }
            target_region = tracker_algorithm.findPathRegion(src_x, src_y);
            if (target_region != null)
            {
                region_path = ((com.oddlabs.tt.pathfinder.RegionNode)target_region.newPath());
            }
            else
            {
                region_path = null;
            }
        }
        // Java: 215
        public com.oddlabs.tt.pathfinder.GridPathNode findPathToNextRegion(int src_x, int src_y, com.oddlabs.tt.pathfinder.RegionNode next_region_node, bool allow_secondary_targets)
        {
            com.oddlabs.tt.pathfinder.Region next_region = null;
            com.oddlabs.tt.pathfinder.Region next_next_region = default;
            if (next_region_node != null)
            {
                next_region = next_region_node.getRegion();
                com.oddlabs.tt.pathfinder.RegionNode next_next_region_node = ((com.oddlabs.tt.pathfinder.RegionNode)next_region_node.getParent());
                if (next_next_region_node != null)
                {
                    next_next_region = next_next_region_node.getRegion();
                    int region_x = next_next_region.getGridX();
                    int region_y = next_next_region.getGridY();
                    return com.oddlabs.tt.pathfinder.PathFinder.findPathGrid(unit_grid, next_region, next_next_region, src_x, src_y, region_x, region_y, null, 0, allow_secondary_targets);
                }
            }
            return tracker_algorithm.findPathGrid(target_region, next_region, src_x, src_y, allow_secondary_targets);
        }
        // Java: 233
        public com.oddlabs.tt.pathfinder.PathTracker.State lookAhead()
        {
            checkRegionPath(next_unit_grid_x, next_unit_grid_y);
            if (region_path == null)
            {
                return com.oddlabs.tt.pathfinder.PathTracker.State.DONE;
            }
            com.oddlabs.tt.pathfinder.RegionNode next_region_node = ((com.oddlabs.tt.pathfinder.RegionNode)region_path.getParent());
            com.oddlabs.tt.pathfinder.Occupant occupant = getNextOccupant();
            if (occupant != null)
            {
                com.oddlabs.tt.pathfinder.GridPathNode patch_path = null;
                com.oddlabs.tt.pathfinder.RegionNode search_next_region_node = next_region_node;
                for (int i = 0; i < com.oddlabs.tt.pathfinder.PathTracker.REGION_SEARCH_TRIES; i++)
                {
                    patch_path = findPathToNextRegion(unit.getGridX(), unit.getGridY(), search_next_region_node, false);
                    if (done(unit.getGridX(), unit.getGridY()))
                    {
                        return com.oddlabs.tt.pathfinder.PathTracker.State.DONE;
                    }
                    if (patch_path != null || search_next_region_node == null)
                    {
                        break;
                    }
                    search_next_region_node = ((com.oddlabs.tt.pathfinder.RegionNode)search_next_region_node.getParent());
                }
                if (patch_path != null)
                {
                    initBezierPath(patch_path.getDirection());
                    grid_path = ((com.oddlabs.tt.pathfinder.GridPathNode)patch_path.getParent());
                    occupant = getNextOccupant();
                }
                if (occupant != null)
                {
                    current_blocker = occupant;
                    if (occupant.getPenalty() < com.oddlabs.tt.pathfinder.Occupant.STATIC)
                    {
                        return com.oddlabs.tt.pathfinder.PathTracker.State.SOFTBLOCKED;
                    }
                    else
                    {
                        return com.oddlabs.tt.pathfinder.PathTracker.State.BLOCKED;
                    }
                }
            }
            else if (grid_path == null && !done(next_unit_grid_x, next_unit_grid_y))
            {
                grid_path = findPathToNextRegion(next_unit_grid_x, next_unit_grid_y, next_region_node, true);
            }
            return com.oddlabs.tt.pathfinder.PathTracker.State.OK;
        }
        // Java: 271
        public void initBezierPath(com.oddlabs.tt.pathfinder.DirectionNode dir_node)
        {
            next_unit_grid_x = unit.getGridX() + dir_node.getDirectionX();
            next_unit_grid_y = unit.getGridY() + dir_node.getDirectionY();
            float next_node_x = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(next_unit_grid_x);
            float next_node_y = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(next_unit_grid_y);
            bezier_path.init(dir_node.getInvLength(), unit.getPositionX(), unit.getPositionY(), next_node_x, next_node_y);
        }
        // Java: 279
        public void setTarget(com.oddlabs.tt.pathfinder.TrackerAlgorithm tracker_algorithm)
        {
            this.tracker_algorithm = tracker_algorithm;
            initial_path = true;
            region_path = null;
            grid_path = null;
            target_region = null;
        }
        // Java: 287
        public com.oddlabs.tt.pathfinder.PathTracker.State initPath()
        {
            checkRegionPath(unit.getGridX(), unit.getGridY());
            if (region_path == null)
            {
                return com.oddlabs.tt.pathfinder.PathTracker.State.DONE;
            }
            com.oddlabs.tt.pathfinder.RegionNode next_region_node = ((com.oddlabs.tt.pathfinder.RegionNode)region_path.getParent());
            com.oddlabs.tt.pathfinder.GridPathNode init_path = findPathToNextRegion(unit.getGridX(), unit.getGridY(), next_region_node, true);
            if (done(unit.getGridX(), unit.getGridY()))
            {
                return com.oddlabs.tt.pathfinder.PathTracker.State.DONE;
            }
            if (init_path == null)
            {
                return com.oddlabs.tt.pathfinder.PathTracker.State.BLOCKED;
            }
            initBezierPath(init_path.getDirection());
            grid_path = ((com.oddlabs.tt.pathfinder.GridPathNode)init_path.getParent());
            return com.oddlabs.tt.pathfinder.PathTracker.State.OK;
        }
        // Java: 304
        public void debugRender()
        {
            com.oddlabs.tt.landscape.HeightMap heightmap = unit_grid.getHeightMap();
            bezier_path.debugRender(heightmap);
            float OFFSET = 2.0f;
            float next_node_x = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(next_unit_grid_x);
            float next_node_y = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(next_unit_grid_y);
            float prev_x = next_node_x;
            float prev_y = next_node_y;
            float prev_z = (float)(heightmap.getNearestHeight(prev_x, prev_y) + OFFSET);
            com.oddlabs.tt.pathfinder.GridPathNode node = grid_path;
            while (node != null)
            {
                float next_x = (float)(prev_x + com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID * node.getDirection().getDirectionX());
                float next_y = (float)(prev_y + com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID * node.getDirection().getDirectionY());
                float z = (float)(heightmap.getNearestHeight(next_x, next_y) + OFFSET);
                com.oddlabs.tt.util.DebugRender.drawLine(prev_x, prev_y, prev_z, next_x, next_y, z, 1.0f, 0.0f, 0.0f);
                prev_x = next_x;
                prev_y = next_y;
                prev_z = z;
                node = ((com.oddlabs.tt.pathfinder.GridPathNode)node.getParent());
            }
            com.oddlabs.tt.pathfinder.RegionNode region_node = region_path;
            bool first = true;
            while (region_node != null)
            {
                float x = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(region_node.getRegion().getGridX());
                float y = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(region_node.getRegion().getGridY());
                float z_2 = (float)(heightmap.getNearestHeight(x, y) + OFFSET);
                if (!first)
                {
                    com.oddlabs.tt.util.DebugRender.drawLine(prev_x, prev_y, prev_z, x, y, z_2, 0.0f, 0.0f, 1.0f);
                }
                prev_x = x;
                prev_y = y;
                prev_z = z_2;
                first = false;
                region_node = ((com.oddlabs.tt.pathfinder.RegionNode)region_node.getParent());
            }
        }
        private void init_PathTracker()
        {
            this.bezier_path = new com.oddlabs.tt.util.BezierPath();
            this.deadlock_mark = false;
            this.state = com.oddlabs.tt.pathfinder.PathTracker.State.DONE;
        }
        public PathTracker(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, com.oddlabs.tt.pathfinder.Movable unit) : base()
        {
            init_PathTracker();
            this.unit_grid = unit_grid;
            this.unit = unit;
        }
    }
}
