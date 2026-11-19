// Translated from Region.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.pathfinder
{
    // Java: 15
    public class Region : com.oddlabs.tt.pathfinder.Node
    {
        public java.util.Map object_lists;
        public java.util.Set neighbours;
        public java.util.ArrayList neighbour_list;
        public int center_x;
        public int center_y;
        // Java: 32
        public override int getGridX()
        {
            return center_x;
        }
        // Java: 37
        public override int getGridY()
        {
            return center_y;
        }
        // Java: 42
        public void setPosition(int center_x, int center_y)
        {
            this.center_x = center_x;
            this.center_y = center_y;
            com.oddlabs.tt.pathfinder.Region.graph_version++;
        }
        // Java: 48
        public override string toString()
        {
            return "Region: " + (center_x) + " " + (center_y);
        }
        // Java: 53
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
        // Java: 65
        public bool isNeighbour(com.oddlabs.tt.pathfinder.Region r)
        {
            return neighbours.contains(r);
        }
        public static void link(com.oddlabs.tt.pathfinder.Region r1, com.oddlabs.tt.pathfinder.Region r2)
        {
            if (r1 == null || r2 == null || ((object)r1 == (object)r2) || r1.neighbours.contains(r2))
            {
                return;
            }
            r1.addNeighbour(r2);
            r2.addNeighbour(r1);
            com.oddlabs.tt.pathfinder.Region.graph_version++;
        }
        public static int graph_version;
        public static com.oddlabs.tt.pathfinder.Region[] NO_PATH;
        public const int MAX_CACHED_PATHS = 48;
        public java.util.HashMap path_cache;
        public int path_cache_version;
        // Java: 81
        public com.oddlabs.tt.pathfinder.Region[] cachedPath(com.oddlabs.tt.pathfinder.Region dst)
        {
            if (path_cache == null || path_cache_version != com.oddlabs.tt.pathfinder.Region.graph_version)
            {
                return null;
            }
            return ((com.oddlabs.tt.pathfinder.Region[])path_cache.get(dst));
        }
        // Java: 87
        public void cachePath(com.oddlabs.tt.pathfinder.Region dst, com.oddlabs.tt.pathfinder.Region end)
        {
            if (path_cache == null)
            {
                path_cache = new java.util.HashMap();
            }
            if (path_cache_version != com.oddlabs.tt.pathfinder.Region.graph_version || path_cache.size() >= com.oddlabs.tt.pathfinder.Region.MAX_CACHED_PATHS)
            {
                path_cache.clear();
                path_cache_version = com.oddlabs.tt.pathfinder.Region.graph_version;
            }
            if (end == null)
            {
                path_cache.put(dst, com.oddlabs.tt.pathfinder.Region.NO_PATH);
                return;
            }
            int n = 0;
            for (com.oddlabs.tt.pathfinder.Node r = end; r != null; r = r.getParent())
            {
                n++;
            }
            com.oddlabs.tt.pathfinder.Region[] chain = new com.oddlabs.tt.pathfinder.Region[n];
            int i = 0;
            for (com.oddlabs.tt.pathfinder.Node r_2 = end; r_2 != null; r_2 = r_2.getParent())
            {
                chain[i++] = ((com.oddlabs.tt.pathfinder.Region)r_2);
            }
            path_cache.put(dst, chain);
        }
        // Java: 109
        public static com.oddlabs.tt.pathfinder.Region restorePath(com.oddlabs.tt.pathfinder.Region[] chain)
        {
            if (chain.Length == 0)
            {
                return null;
            }
            for (int i = 0; i < chain.Length; i++)
            {
                chain[i].restoreParent((i + 1 < chain.Length ? (com.oddlabs.tt.pathfinder.Node)(chain[i + 1]) : (com.oddlabs.tt.pathfinder.Node)(null)));
            }
            return chain[0];
        }
        // Java: 117
        public java.util.List getObjects(System.Type key)
        {
            java.util.List list = ((java.util.List)((java.util.List)object_lists.computeIfAbsent(key, new java.util.function.Function_L((object k_l) =>
            {
                System.Type k = (System.Type)k_l;
                return new java.util.ArrayList();
            }))));
            return list;
        }
        // Java: 123
        public void registerObject(System.Type key, object @object)
        {
            getObjects(key).add(@object);
        }
        // Java: 127
        public void unregisterObject(System.Type key, object @object)
        {
            java.util.List list = ((java.util.List)((java.util.List)object_lists.get(key)));
            list.remove(@object);
        }
        // Java: 134
        public void unlinkAll()
        {
            com.oddlabs.tt.pathfinder.Region.graph_version++;
            foreach (com.oddlabs.tt.pathfinder.Region n in java.lang.JIter.of(neighbours))
            {
                n.neighbours.remove(this);
                n.neighbour_list.remove(this);
            }
            neighbours.clear();
            neighbour_list.clear();
        }
        // Java: 144
        public void addNeighbour(com.oddlabs.tt.pathfinder.Region n)
        {
            if (neighbours.add(n))
            {
                neighbour_list.add(n);
            }
        }
        // Java: 149
        public override bool addNeighbours(com.oddlabs.tt.pathfinder.PathFinderAlgorithm finder, com.oddlabs.tt.pathfinder.UnitGrid unit_grid)
        {
            for (int i = 0, n = neighbour_list.size(); i < n; i++)
            {
                com.oddlabs.tt.pathfinder.Region neighbour = ((com.oddlabs.tt.pathfinder.Region)neighbour_list.get(i));
                if (!neighbour.isVisited())
                {
                    com.oddlabs.tt.pathfinder.PathFinder.addToOpenList(finder, neighbour, this, estimateCost(neighbour.getGridX(), neighbour.getGridY()));
                }
            }
            return false;
        }
        // Java: 160
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
        // Java: 169
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
        static Region()
        {
            Region.NO_PATH = new com.oddlabs.tt.pathfinder.Region[0];
        }
        private void init_Region()
        {
            this.object_lists = new java.util.HashMap();
            this.neighbours = new java.util.LinkedHashSet();
            this.neighbour_list = new java.util.ArrayList();
        }
        public Region() : base()
        {
            init_Region();
        }
    }
}
