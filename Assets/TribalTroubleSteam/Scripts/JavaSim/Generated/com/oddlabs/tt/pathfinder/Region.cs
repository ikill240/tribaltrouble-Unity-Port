// Translated from Region.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.pathfinder
{
    // Java: 13
    public class Region : com.oddlabs.tt.pathfinder.Node
    {
        public java.util.Map object_lists;
        public java.util.List neighbours;
        public int center_x;
        public int center_y;
        // Java: 20
        public override int getGridX()
        {
            return center_x;
        }
        // Java: 25
        public override int getGridY()
        {
            return center_y;
        }
        // Java: 30
        public void setPosition(int center_x, int center_y)
        {
            this.center_x = center_x;
            this.center_y = center_y;
        }
        // Java: 35
        public override string toString()
        {
            return "Region: " + (center_x) + " " + (center_y);
        }
        // Java: 40
        public override com.oddlabs.tt.pathfinder.PathNode newPath()
        {
            com.oddlabs.tt.pathfinder.Node graph_node = this;
            com.oddlabs.tt.pathfinder.RegionNode current_node = null;
            while (graph_node != null)
            {
                current_node = new com.oddlabs.tt.pathfinder.RegionNode(current_node, ((com.oddlabs.tt.pathfinder.Region)graph_node));
                graph_node = graph_node.getParent();
            }
            return current_node;
        }
        // Java: 52
        public bool isNeighbour(com.oddlabs.tt.pathfinder.Region r)
        {
            return neighbours.contains(r);
        }
        // Java: 56
        public static void link(com.oddlabs.tt.pathfinder.Region r1, com.oddlabs.tt.pathfinder.Region r2)
        {
            if (r1 == null || r2 == null || ((object)r1 == (object)r2) || r1.neighbours.contains(r2))
            {
                return;
            }
            r1.addNeighbour(r2);
            r2.addNeighbour(r1);
        }
        // Java: 63
        public java.util.List getObjects(System.Type key)
        {
            java.util.List list = ((java.util.List)((java.util.List)object_lists.computeIfAbsent(key, new java.util.function.Function_L((object k_l) =>
            {
                System.Type k = (System.Type)k_l;
                return new java.util.ArrayList();
            }))));
            return list;
        }
        // Java: 69
        public void registerObject(System.Type key, object @object)
        {
            getObjects(key).add(@object);
        }
        // Java: 73
        public void unregisterObject(System.Type key, object @object)
        {
            java.util.List list = ((java.util.List)((java.util.List)object_lists.get(key)));
            list.remove(@object);
        }
        // Java: 79
        public void addNeighbour(com.oddlabs.tt.pathfinder.Region n)
        {
            neighbours.add(n);
        }
        // Java: 83
        public override bool addNeighbours(com.oddlabs.tt.pathfinder.PathFinderAlgorithm finder, com.oddlabs.tt.pathfinder.UnitGrid unit_grid)
        {
            foreach (com.oddlabs.tt.pathfinder.Region neighbour in java.lang.JIter.of(neighbours))
            {
                if (!neighbour.isVisited())
                {
                    com.oddlabs.tt.pathfinder.PathFinder.addToOpenList(finder, neighbour, this, estimateCost(neighbour.getGridX(), neighbour.getGridY()));
                }
            }
            return false;
        }
        // Java: 93
        public void debugRenderConnectionsReset()
        {
            if (!isVisited())
            {
                return;
            }
            setVisited(false);
            foreach (com.oddlabs.tt.pathfinder.Region neighbour in java.lang.JIter.of(neighbours))
            {
                neighbour.debugRenderConnectionsReset();
            }
        }
        // Java: 102
        public void debugRenderConnections(com.oddlabs.tt.landscape.HeightMap heightmap)
        {
            if (isVisited())
            {
                return;
            }
            setVisited(true);
            foreach (com.oddlabs.tt.pathfinder.Region neighbour in java.lang.JIter.of(neighbours))
            {
                float x1 = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(getGridX());
                float y1 = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(getGridY());
                float z1 = (float)(heightmap.getNearestHeight(x1, y1) + 2.0f);
                float x2 = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(neighbour.getGridX());
                float y2 = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(neighbour.getGridY());
                float z2 = (float)(heightmap.getNearestHeight(x2, y2) + 2.0f);
                com.oddlabs.tt.util.DebugRender.drawLine(x1, y1, z1, x2, y2, z2, 1.0f, 1.0f, 0.0f);
                neighbour.debugRenderConnections(heightmap);
            }
        }
        private void init_Region()
        {
            this.object_lists = new java.util.LinkedHashMap();
            this.neighbours = new java.util.ArrayList();
        }
        public Region() : base()
        {
            init_Region();
        }
    }
}
