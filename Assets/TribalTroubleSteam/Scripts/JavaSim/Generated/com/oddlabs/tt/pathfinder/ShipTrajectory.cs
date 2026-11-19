// Translated from ShipTrajectory.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.pathfinder
{
    // Java: 15
    public class ShipTrajectory : java.lang.Object
    {
        public com.oddlabs.tt.model.Ship ship;
        public com.oddlabs.tt.pathfinder.UnitGrid grid;
        public java.util.List trajectory;
        public bool isComplete_f;
        public int currentSegmentIndex;
        public float segmentProgress;
        public float totalProgress;
        // Java: 57
        public java.util.List findRegionPath(com.oddlabs.tt.pathfinder.ShipTrajectoryPoint from, com.oddlabs.tt.pathfinder.ShipTrajectoryPoint to)
        {
            java.util.List points = new java.util.ArrayList();
            if (from == null || to == null)
            {
                return null;
            }
            com.oddlabs.tt.pathfinder.Region src_region = findRegion(from.gridX, from.gridY);
            com.oddlabs.tt.pathfinder.Region dst_region = findRegion(to.gridX, to.gridY);
            if (src_region == null || dst_region == null)
            {
                return null;
            }
            com.oddlabs.tt.pathfinder.Region path_end = com.oddlabs.tt.pathfinder.PathFinder.findPathRegion(grid, src_region, dst_region);
            if (path_end == null)
            {
                return null;
            }
            com.oddlabs.tt.pathfinder.RegionNode node = ((com.oddlabs.tt.pathfinder.RegionNode)path_end.newPath());
            points.add(from);
            while (node != null)
            {
                com.oddlabs.tt.pathfinder.ShipTrajectoryPoint pt = new com.oddlabs.tt.pathfinder.ShipTrajectoryPoint(node.getRegion().getGridX(), node.getRegion().getGridY());
                points.add(pt);
                node = ((com.oddlabs.tt.pathfinder.RegionNode)node.getParent());
            }
            points.add(to);
            return points;
        }
        // Java: 88
        public com.oddlabs.tt.pathfinder.Region findRegion(int grid_x, int grid_y)
        {
            int size = grid.getGridSize();
            if (grid_x < 0 || grid_x >= size || grid_y < 0 || grid_y >= size)
            {
                return null;
            }
            return grid.getRegion(grid_x, grid_y, com.oddlabs.tt.pathfinder.UnitGrid.SEA);
        }
        // Java: 96
        public void debugRender(com.oddlabs.tt.landscape.HeightMap heightmap)
        {
            if (trajectory == null)
            {
                return;
            }
            float OFFSET = 0.1f;
            float z = (float)(heightmap.getSeaLevelMeters() + OFFSET);
            foreach (com.oddlabs.tt.pathfinder.ShipTrajectorySegment segment in java.lang.JIter.of(trajectory))
            {
                if (segment.isStraight)
                {
                    com.oddlabs.tt.util.DebugRender.drawLine(segment.p0.positionX, segment.p0.positionY, z, segment.p1.positionX, segment.p1.positionY, z, 0.0f, 1.0f, 0.0f);
                }
                else if (!segment.isPivot)
                {
                    drawArc(segment, z);
                }
            }
        }
        // Java: 114
        public void drawArc(com.oddlabs.tt.pathfinder.ShipTrajectorySegment segment, float z)
        {
            float prevX = segment.p0.positionX;
            float prevY = segment.p0.positionY;
            for (int i = 1; i <= 16; i++)
            {
                float percent = (float)(((float)(i)) / 16);
                com.oddlabs.tt.pathfinder.ShipTrajectoryPoint pt = ((com.oddlabs.tt.pathfinder.ShipTrajectoryPoint)segment.center.clone());
                pt.setDirectionTo(segment.p0);
                float deltaAngle = ((float)(java.lang.StrictMath.toDegrees((float)((float)(segment.length * percent) / segment.radius))));
                pt.rotate((float)(deltaAngle * segment.angle_sign));
                pt.move(segment.radius);
                com.oddlabs.tt.util.DebugRender.drawLine(prevX, prevY, z, pt.positionX, pt.positionY, z, 0.0f, 1.0f, 0.0f);
                prevX = pt.positionX;
                prevY = pt.positionY;
            }
        }
        // Java: 130
        public bool exists()
        {
            return trajectory != null && trajectory.size() > 0;
        }
        // Java: 134
        public bool isComplete()
        {
            return isComplete_f;
        }
        // Java: 138
        public com.oddlabs.tt.pathfinder.ShipTrajectorySegment get(int index)
        {
            return ((com.oddlabs.tt.pathfinder.ShipTrajectorySegment)trajectory.get(index));
        }
        // Java: 142
        public com.oddlabs.tt.pathfinder.ShipTrajectoryPoint advance(float distance)
        {
            if (distance <= 0.001f)
            {
                return new com.oddlabs.tt.pathfinder.ShipTrajectoryPoint(ship);
            }
            if (currentSegmentIndex >= trajectory.size())
            {
                return null;
            }
            com.oddlabs.tt.pathfinder.ShipTrajectoryPoint pt = new com.oddlabs.tt.pathfinder.ShipTrajectoryPoint();
            while (distance > 0.0f && currentSegmentIndex < trajectory.size())
            {
                distance = ((com.oddlabs.tt.pathfinder.ShipTrajectorySegment)trajectory.get(currentSegmentIndex)).advance(distance, pt);
                if (distance > 0.0f)
                {
                    currentSegmentIndex++;
                }
            }
            return pt;
        }
        // Java: 161
        public bool reachedGoal()
        {
            if (currentSegmentIndex >= trajectory.size())
            {
                return true;
            }
            return false;
        }
        // Java: 168
        public bool almostReachedGoal()
        {
            if (currentSegmentIndex >= trajectory.size())
            {
                return true;
            }
            float d = ((com.oddlabs.tt.pathfinder.ShipTrajectorySegment)trajectory.get(trajectory.size() - 1)).p1.gridDistanceTo(new com.oddlabs.tt.pathfinder.ShipTrajectoryPoint(ship));
            if (d <= 4)
            {
                return true;
            }
            return false;
        }
        // Java: 179
        public java.util.List createTrajectory(java.util.List path)
        {
            java.util.List result = new java.util.ArrayList();
            if (path == null || path.size() < 2)
            {
                return result;
            }
            int n = path.size();
            com.oddlabs.tt.pathfinder.ShipTrajectoryPoint prev = ((com.oddlabs.tt.pathfinder.ShipTrajectoryPoint)path.get(0));
            com.oddlabs.tt.pathfinder.ShipTrajectoryPoint pivotPt = ((com.oddlabs.tt.pathfinder.ShipTrajectoryPoint)prev.clone());
            pivotPt.setDirectionTo(((com.oddlabs.tt.pathfinder.ShipTrajectoryPoint)path.get(1)));
            result.add(makeStraightSegment(prev, pivotPt));
            for (int i = 1; i < n - 1; i++)
            {
                com.oddlabs.tt.pathfinder.ShipTrajectoryPoint a = ((com.oddlabs.tt.pathfinder.ShipTrajectoryPoint)path.get(i - 1));
                com.oddlabs.tt.pathfinder.ShipTrajectoryPoint b = ((com.oddlabs.tt.pathfinder.ShipTrajectoryPoint)path.get(i));
                com.oddlabs.tt.pathfinder.ShipTrajectoryPoint c = ((com.oddlabs.tt.pathfinder.ShipTrajectoryPoint)path.get(i + 1));
                float clip0 = (float)(a.distanceTo(b) * 0.5f);
                float clip1 = (float)(b.distanceTo(c) * 0.5f);
                float clip = (java.lang.StrictMath.min(clip0, clip1));
                clip = (java.lang.StrictMath.min(clip, 20.0f));
                com.oddlabs.tt.pathfinder.ShipTrajectoryPoint b_a = ((com.oddlabs.tt.pathfinder.ShipTrajectoryPoint)b.clone());
                b_a.setDirectionTo(a);
                b_a.move(clip);
                com.oddlabs.tt.pathfinder.ShipTrajectoryPoint b_c = ((com.oddlabs.tt.pathfinder.ShipTrajectoryPoint)b.clone());
                b_c.setDirectionTo(c);
                b_c.move(clip);
                com.oddlabs.tt.pathfinder.ShipTrajectoryPoint p0 = ((com.oddlabs.tt.pathfinder.ShipTrajectoryPoint)prev.clone());
                p0.setDirectionTo(b);
                com.oddlabs.tt.pathfinder.ShipTrajectoryPoint p1 = ((com.oddlabs.tt.pathfinder.ShipTrajectoryPoint)b_a.clone());
                p1.copyDirection(p0);
                result.add(makeStraightSegment(p0, p1));
                com.oddlabs.tt.pathfinder.ShipTrajectoryPoint center = b_a.rotated(90).intersection(b_c.rotated(90));
                if (center != null)
                {
                    float radius = center.distanceTo(b_a);
                    center.setDirectionTo(b_c);
                    center.rotate((float)(-90.0f));
                    b_c.copyDirection(center);
                    b_a.setDirectionTo(b);
                    result.add(makeArcSegment(b_a, b_c, radius, center));
                    prev = b_c;
                }
                else
                {
                    isComplete_f = false;
                    return result;
                }
            }
            com.oddlabs.tt.pathfinder.ShipTrajectoryPoint p1_2 = ((com.oddlabs.tt.pathfinder.ShipTrajectoryPoint)path.get(n - 1));
            prev.setDirectionTo(p1_2);
            p1_2.copyDirection(prev);
            result.add(makeStraightSegment(prev, p1_2));
            return result;
        }
        // Java: 242
        public com.oddlabs.tt.pathfinder.ShipTrajectorySegment makeStraightSegment(com.oddlabs.tt.pathfinder.ShipTrajectoryPoint p0, com.oddlabs.tt.pathfinder.ShipTrajectoryPoint p1)
        {
            return new com.oddlabs.tt.pathfinder.ShipTrajectorySegment(p0, p1);
        }
        // Java: 246
        public com.oddlabs.tt.pathfinder.ShipTrajectorySegment makeArcSegment(com.oddlabs.tt.pathfinder.ShipTrajectoryPoint p0, com.oddlabs.tt.pathfinder.ShipTrajectoryPoint p1, float radius, com.oddlabs.tt.pathfinder.ShipTrajectoryPoint center)
        {
            return new com.oddlabs.tt.pathfinder.ShipTrajectorySegment(p0, p1, radius, center);
        }
        // Java: 254
        public void optimizePath(java.util.List path)
        {
            if (path == null || path.size() < 3)
            {
                return;
            }
            bool changed = true;
            while (changed && path.size() >= 3)
            {
                changed = false;
                int i = 1;
                while (i < path.size() - 1)
                {
                    com.oddlabs.tt.pathfinder.ShipTrajectoryPoint prev = ((com.oddlabs.tt.pathfinder.ShipTrajectoryPoint)path.get(i - 1));
                    com.oddlabs.tt.pathfinder.ShipTrajectoryPoint next = ((com.oddlabs.tt.pathfinder.ShipTrajectoryPoint)path.get(i + 1));
                    if (!com.oddlabs.tt.pathfinder.ShipTrajectory.checkLandCollision(grid, prev, next) && com.oddlabs.tt.pathfinder.ShipTrajectory.checkShipsCollision(grid, ship, prev.moved((-7)), next.moved(7)) == null)
                    {
                        path.remove(i);
                        changed = true;
                    }
                    else
                    {
                        i++;
                    }
                }
            }
        }
        // Java: 278
        public class DeepWaterFinder : java.lang.Object, com.oddlabs.tt.pathfinder.ScanFilter
        {
            public com.oddlabs.tt.pathfinder.UnitGrid grid;
            public com.oddlabs.tt.pathfinder.ShipTrajectoryPoint pt;
            // Java: 286
            public virtual int getMinRadius()
            {
                return 0;
            }
            // Java: 290
            public virtual int getMaxRadius()
            {
                return grid.getGridSize();
            }
            // Java: 294
            public virtual bool filter(int grid_x, int grid_y, com.oddlabs.tt.pathfinder.Occupant occ)
            {
                int size = grid.getGridSize();
                if (grid.isDeepWater(grid_x, grid_y) && grid.getRegion(grid_x, grid_y, com.oddlabs.tt.pathfinder.UnitGrid.SEA) != null)
                {
                    pt = new com.oddlabs.tt.pathfinder.ShipTrajectoryPoint(grid_x, grid_y);
                    return true;
                }
                return false;
            }
            // Java: 303
            public virtual com.oddlabs.tt.pathfinder.ShipTrajectoryPoint result()
            {
                return pt;
            }
            private void init_DeepWaterFinder()
            {
                this.pt = null;
            }
            public DeepWaterFinder(com.oddlabs.tt.pathfinder.UnitGrid grid) : base()
            {
                init_DeepWaterFinder();
                this.grid = grid;
            }
        }
        // Java: 308
        public static com.oddlabs.tt.pathfinder.ShipTrajectoryPoint pickTargetPosition(com.oddlabs.tt.pathfinder.UnitGrid grid, com.oddlabs.tt.model.Ship self, com.oddlabs.tt.util.Target target)
        {
            int targetIsland = grid.getIslandId(target.getGridX(), target.getGridY());
            if (targetIsland != 0)
            {
                com.oddlabs.tt.landscape.IslandInfo island = grid.getIslandInfo(targetIsland);
                java.util.List pts = island.contourPoints();
                int best_d2 = (-1);
                int[] best_pt = ((int[])pts.get(0));
                for (int i = 0; i < pts.size(); i++)
                {
                    int[] pt = ((int[])pts.get(i));
                    bool obstacle = false;
                    foreach (com.oddlabs.tt.player.Player player in self.getOwner().getWorld().getPlayers())
                    {
                        foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(player.getUnits().getSet()))
                        {
                            if ((s is com.oddlabs.tt.model.Ship otherShip) && ((object)otherShip != (object)self))
                            {
                                int odx = s.getGridX() - pt[0];
                                int ody = s.getGridY() - pt[1];
                                int od2 = odx * odx + ody * ody;
                                if (od2 < 400)
                                {
                                    obstacle = true;
                                    break;
                                }
                            }
                        }
                        if (obstacle)
                        {
                            break;
                        }
                    }
                    if (!obstacle)
                    {
                        int dx = pt[0] - target.getGridX();
                        int dy = pt[1] - target.getGridY();
                        int d2 = dx * dx + dy * dy;
                        if (best_d2 == (-1) || d2 < best_d2)
                        {
                            best_d2 = d2;
                            best_pt = pt;
                        }
                    }
                }
                if (best_d2 != (-1))
                {
                    target = new com.oddlabs.tt.landscape.LandscapeTarget(best_pt[0], best_pt[1]);
                }
            }
            com.oddlabs.tt.pathfinder.ShipTrajectory.DeepWaterFinder finder = new com.oddlabs.tt.pathfinder.ShipTrajectory.DeepWaterFinder(grid);
            grid.scan(finder, target.getGridX(), target.getGridY(), com.oddlabs.tt.pathfinder.UnitGrid.SEA);
            return finder.result();
        }
        // Java: 356
        public static bool checkLandCollision(com.oddlabs.tt.pathfinder.UnitGrid grid, com.oddlabs.tt.pathfinder.ShipTrajectoryPoint p0, com.oddlabs.tt.pathfinder.ShipTrajectoryPoint p1)
        {
            if (p0.gridX == p1.gridX && p0.gridY == p1.gridY)
            {
                return !grid.isDeepWater(p0.gridX, p0.gridY);
            }
            int dx = java.lang.Math.abs(p1.gridX - p0.gridX);
            int dy = java.lang.Math.abs(p1.gridY - p0.gridY);
            float total = p0.distanceTo(p1);
            float step = (float)(total / java.lang.Math.max(dx, dy));
            int s = grid.getGridSize();
            int n = (java.lang.StrictMath.round((float)(total / step)));
            com.oddlabs.tt.pathfinder.ShipTrajectoryPoint origin = ((com.oddlabs.tt.pathfinder.ShipTrajectoryPoint)p0.clone());
            origin.setDirectionTo(p1);
            for (int i = 0; i <= n; i++)
            {
                com.oddlabs.tt.pathfinder.ShipTrajectoryPoint sample = origin.moved((float)(i * step));
                if (sample.gridX < 0 || sample.gridX >= s || sample.gridY < 0 || sample.gridY >= s)
                {
                    return true;
                }
                if (!grid.isDeepWater(sample.gridX, sample.gridY))
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 380
        public static bool polygonCollision(com.oddlabs.tt.pathfinder.ShipTrajectoryPoint[] poly0, com.oddlabs.tt.pathfinder.ShipTrajectoryPoint[] poly1)
        {
            for (int i = 0; i < poly0.Length; i++)
            {
                com.oddlabs.tt.pathfinder.ShipTrajectoryPoint p0_a = poly0[i];
                com.oddlabs.tt.pathfinder.ShipTrajectoryPoint p0_b = poly0[(i + 1) % poly0.Length];
                float min_x0 = java.lang.Math.min(p0_a.positionX, p0_b.positionX);
                float max_x0 = java.lang.Math.max(p0_a.positionX, p0_b.positionX);
                float min_y0 = java.lang.Math.min(p0_a.positionY, p0_b.positionY);
                float max_y0 = java.lang.Math.max(p0_a.positionY, p0_b.positionY);
                for (int j = 0; j < poly1.Length; j++)
                {
                    com.oddlabs.tt.pathfinder.ShipTrajectoryPoint p1_a = poly1[j];
                    com.oddlabs.tt.pathfinder.ShipTrajectoryPoint p1_b = poly1[(j + 1) % poly1.Length];
                    float min_x1 = java.lang.Math.min(p1_a.positionX, p1_b.positionX);
                    float max_x1 = java.lang.Math.max(p1_a.positionX, p1_b.positionX);
                    float min_y1 = java.lang.Math.min(p1_a.positionY, p1_b.positionY);
                    float max_y1 = java.lang.Math.max(p1_a.positionY, p1_b.positionY);
                    com.oddlabs.tt.pathfinder.ShipTrajectoryPoint inter = p0_a.intersection(p1_a);
                    if (inter != null)
                    {
                        if (inter.positionX >= min_x0 && inter.positionX <= max_x0 && inter.positionY >= min_y0 && inter.positionY <= max_y0 && inter.positionX >= min_x1 && inter.positionX <= max_x1 && inter.positionY >= min_y1 && inter.positionY <= max_y1)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        // Java: 409
        public static com.oddlabs.tt.pathfinder.ShipTrajectoryPoint[] shipToPolygon(com.oddlabs.tt.model.Ship s)
        {
            com.oddlabs.tt.pathfinder.ShipTrajectoryPoint[] poly = new com.oddlabs.tt.pathfinder.ShipTrajectoryPoint[4];
            com.oddlabs.tt.pathfinder.ShipTrajectoryPoint center = new com.oddlabs.tt.pathfinder.ShipTrajectoryPoint(s);
            poly[0] = center.moved((-6)).rotated(90).moved(2);
            poly[1] = poly[0].moved((-4));
            poly[2] = center.moved(6).rotated((-90)).moved(2);
            poly[3] = poly[2].moved((-4));
            poly[0].setDirectionTo(poly[1]);
            poly[1].setDirectionTo(poly[2]);
            poly[2].setDirectionTo(poly[3]);
            poly[3].setDirectionTo(poly[0]);
            return poly;
        }
        // Java: 426
        public class ShipFinder : java.lang.Object, com.oddlabs.tt.pathfinder.ScanFilter
        {
            public int radius;
            public java.util.Set ships;
            public com.oddlabs.tt.model.Ship self;
            // Java: 436
            public virtual int getMinRadius()
            {
                return 0;
            }
            // Java: 440
            public virtual int getMaxRadius()
            {
                return radius;
            }
            // Java: 444
            public virtual bool filter(int grid_x, int grid_y, com.oddlabs.tt.pathfinder.Occupant occ)
            {
                if ((occ is com.oddlabs.tt.model.Ship s) && ((object)s != (object)self) && !s.isDead())
                {
                    ships.add(s);
                }
                return false;
            }
            // Java: 451
            public virtual java.util.Set results()
            {
                return ships;
            }
            private void init_ShipFinder()
            {
                this.ships = new java.util.LinkedHashSet();
            }
            public ShipFinder(int radius, com.oddlabs.tt.model.Ship self) : base()
            {
                init_ShipFinder();
                this.radius = radius;
                this.self = self;
            }
        }
        // Java: 456
        public static com.oddlabs.tt.model.Ship checkShipsCollision(com.oddlabs.tt.pathfinder.UnitGrid grid, com.oddlabs.tt.model.Ship ship, com.oddlabs.tt.pathfinder.ShipTrajectoryPoint p0, com.oddlabs.tt.pathfinder.ShipTrajectoryPoint p1)
        {
            com.oddlabs.tt.pathfinder.ShipTrajectoryPoint[] poly = new com.oddlabs.tt.pathfinder.ShipTrajectoryPoint[4];
            com.oddlabs.tt.pathfinder.ShipTrajectoryPoint center0 = ((com.oddlabs.tt.pathfinder.ShipTrajectoryPoint)p0.clone());
            center0.setDirectionTo(p1);
            poly[0] = center0.rotated(90).moved(3);
            poly[1] = poly[0].moved((-6));
            com.oddlabs.tt.pathfinder.ShipTrajectoryPoint center1 = ((com.oddlabs.tt.pathfinder.ShipTrajectoryPoint)p1.clone());
            center1.copyDirection(center0);
            poly[2] = center1.rotated((-90)).moved(3);
            poly[3] = poly[2].moved((-6));
            poly[0].setDirectionTo(poly[1]);
            poly[1].setDirectionTo(poly[2]);
            poly[2].setDirectionTo(poly[3]);
            poly[3].setDirectionTo(poly[0]);
            int center_x = (p0.gridX + p1.gridX) / 2;
            int center_y = (p0.gridY + p1.gridY) / 2;
            int radius = (java.lang.StrictMath.round(poly[0].gridDistanceTo(poly[2]))) + 10;
            int r2 = radius * radius;
            foreach (com.oddlabs.tt.player.Player player in ship.getOwner().getWorld().getPlayers())
            {
                foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(player.getUnits().getSet()))
                {
                    if ((s is com.oddlabs.tt.model.Ship otherShip) && ((object)otherShip != (object)ship))
                    {
                        int dist_dx = center_x - otherShip.getGridX();
                        int dist_dy = center_y - otherShip.getGridY();
                        if (dist_dx * dist_dx + dist_dy * dist_dy < r2)
                        {
                            com.oddlabs.tt.pathfinder.ShipTrajectoryPoint[] otherPoly = com.oddlabs.tt.pathfinder.ShipTrajectory.shipToPolygon(otherShip);
                            if (com.oddlabs.tt.pathfinder.ShipTrajectory.polygonCollision(poly, otherPoly))
                            {
                                return otherShip;
                            }
                        }
                    }
                }
            }
            return null;
        }
        private void init_ShipTrajectory()
        {
            this.isComplete_f = true;
            this.currentSegmentIndex = 0;
            this.segmentProgress = 0.0f;
            this.totalProgress = 0.0f;
        }
        public ShipTrajectory(com.oddlabs.tt.model.Ship ship, com.oddlabs.tt.util.Target t) : base()
        {
            init_ShipTrajectory();
            this.ship = ship;
            grid = ship.getUnitGrid();
            com.oddlabs.tt.pathfinder.ShipTrajectoryPoint p0 = new com.oddlabs.tt.pathfinder.ShipTrajectoryPoint(ship);
            com.oddlabs.tt.pathfinder.ShipTrajectoryPoint p1 = com.oddlabs.tt.pathfinder.ShipTrajectory.pickTargetPosition(grid, ship, t);
            bool simple = !com.oddlabs.tt.pathfinder.ShipTrajectory.checkLandCollision(grid, p0, p1);
            if (simple)
            {
                java.util.ArrayList path = new java.util.ArrayList();
                if (p0.gridX == p1.gridX && p0.gridY == p1.gridY)
                {
                    trajectory = null;
                }
                else
                {
                    path.add(p0);
                    path.add(p1);
                    trajectory = createTrajectory(path);
                }
            }
            else
            {
                java.util.List regionPath = findRegionPath(p0, p1);
                if (regionPath != null)
                {
                    optimizePath(regionPath);
                    trajectory = createTrajectory(regionPath);
                }
                else
                {
                    trajectory = null;
                }
            }
        }
    }
}
