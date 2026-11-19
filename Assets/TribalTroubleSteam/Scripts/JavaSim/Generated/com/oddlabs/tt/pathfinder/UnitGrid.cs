// Translated from UnitGrid.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.pathfinder
{
    // Java: 12
    public class UnitGrid : java.lang.Object
    {
        public com.oddlabs.tt.landscape.HeightMap heightmap;
        public const int LAND = 0;
        public const int SEA = 1;
        public const int NUM_LAYERS = 2;
        // Java: 19
        public class Layer : java.lang.Object
        {
            public com.oddlabs.tt.pathfinder.Region[][] regions;
            public com.oddlabs.tt.pathfinder.Occupant[][] occupants;
            public Layer(int size) : base()
            {
                occupants = java.lang.JA.newArray2<com.oddlabs.tt.pathfinder.Occupant>(size, size);
                regions = java.lang.JA.newArray2<com.oddlabs.tt.pathfinder.Region>(size, size);
            }
        }
        public com.oddlabs.tt.pathfinder.UnitGrid.Layer[] layers;
        // Java: 39
        public bool filter(com.oddlabs.tt.pathfinder.ScanFilter filter_v, int x, int y)
        {
            return filter(filter_v, x, y, com.oddlabs.tt.pathfinder.UnitGrid.LAND);
        }
        // Java: 43
        public bool filter(com.oddlabs.tt.pathfinder.ScanFilter filter_v, int x, int y, int layer)
        {
            com.oddlabs.tt.pathfinder.Occupant[][] occupants = layers[layer].occupants;
            if (x < 0 || y < 0 || x >= occupants.Length || y >= occupants.Length)
            {
                return false;
            }
            return filter_v.filter(x, y, occupants[y][x]);
        }
        // Java: 50
        public com.oddlabs.tt.util.Target[] findGridTargets(int center_grid_x, int center_grid_y, int num_targets, bool grid_targets_only)
        {
            return findGridTargets(center_grid_x, center_grid_y, num_targets, grid_targets_only, (-1));
        }
        // Java: 55
        public com.oddlabs.tt.util.Target[] findGridTargets(int center_grid_x, int center_grid_y, int num_targets, bool grid_targets_only, int island)
        {
            com.oddlabs.tt.pathfinder.Occupant[][] occupants = layers[com.oddlabs.tt.pathfinder.UnitGrid.LAND].occupants;
            com.oddlabs.tt.pathfinder.FindTargetsFilter filter_v = new com.oddlabs.tt.pathfinder.FindTargetsFilter(this, num_targets, occupants.Length, grid_targets_only, island);
            scan(filter_v, center_grid_x, center_grid_y, com.oddlabs.tt.pathfinder.UnitGrid.LAND);
            return filter_v.getTargets();
        }
        // Java: 64
        public void scan(com.oddlabs.tt.pathfinder.ScanFilter filter_v, int center_grid_x, int center_grid_y)
        {
            scan(filter_v, center_grid_x, center_grid_y, com.oddlabs.tt.pathfinder.UnitGrid.LAND);
        }
        // Java: 68
        public void scan(com.oddlabs.tt.pathfinder.ScanFilter filter_v, int center_grid_x, int center_grid_y, int layer)
        {
            int radius = filter_v.getMinRadius();
            if (radius == 0)
            {
                if (filter(filter_v, center_grid_x, center_grid_y, layer))
                {
                    return;
                }
                radius++;
            }
            while (radius <= filter_v.getMaxRadius())
            {
                int x = center_grid_x - radius;
                int x2 = center_grid_x + radius;
                for (int i = 0; i < 2 * radius - 1; i++)
                {
                    int y_i = center_grid_y - radius + 1 + i;
                    if (filter(filter_v, x, y_i, layer))
                    {
                        return;
                    }
                    if (filter(filter_v, x2, y_i, layer))
                    {
                        return;
                    }
                }
                int y = center_grid_y - radius;
                int y2 = center_grid_y + radius;
                for (int i_2 = 0; i_2 < 2 * radius + 1; i_2++)
                {
                    int x_i = center_grid_x - radius + i_2;
                    if (filter(filter_v, x_i, y, layer))
                    {
                        return;
                    }
                    if (filter(filter_v, x_i, y2, layer))
                    {
                        return;
                    }
                }
                radius++;
            }
        }
        // Java: 98
        public static float coordinateFromGrid(int g)
        {
            return (float)(((float)(g + 0.5f)) * com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID);
        }
        // Java: 102
        public static int toGridCoordinate(float c)
        {
            return java.lang.JC.f2i(((float)(c / com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID)));
        }
        // Java: 106
        public int getGridSize()
        {
            return layers[com.oddlabs.tt.pathfinder.UnitGrid.LAND].occupants.Length;
        }
        // Java: 110
        public com.oddlabs.tt.pathfinder.Region getRegion(int grid_x, int grid_y)
        {
            return getRegion(grid_x, grid_y, com.oddlabs.tt.pathfinder.UnitGrid.LAND);
        }
        // Java: 114
        public com.oddlabs.tt.pathfinder.Region getRegion(int grid_x, int grid_y, int layer)
        {
            com.oddlabs.tt.pathfinder.Region region = layers[layer].regions[grid_y][grid_x];
            return region;
        }
        // Java: 119
        public void setRegion(int grid_x, int grid_y, com.oddlabs.tt.pathfinder.Region r)
        {
            setRegion(grid_x, grid_y, r, com.oddlabs.tt.pathfinder.UnitGrid.LAND);
        }
        // Java: 123
        public void setRegion(int grid_x, int grid_y, com.oddlabs.tt.pathfinder.Region r, int layer)
        {
            layers[layer].regions[grid_y][grid_x] = r;
        }
        // Java: 128
        public bool isGridOccupied(int grid_x, int grid_y)
        {
            return isGridOccupied(grid_x, grid_y, com.oddlabs.tt.pathfinder.UnitGrid.LAND);
        }
        // Java: 132
        public bool isGridOccupied(int grid_x, int grid_y, int layer)
        {
            return layers[layer].occupants[grid_y][grid_x] != null;
        }
        // Java: 136
        public com.oddlabs.tt.pathfinder.Occupant getOccupant(int grid_x, int grid_y)
        {
            return getOccupant(grid_x, grid_y, com.oddlabs.tt.pathfinder.UnitGrid.LAND);
        }
        // Java: 140
        public com.oddlabs.tt.pathfinder.Occupant getOccupant(int grid_x, int grid_y, int layer)
        {
            return layers[layer].occupants[grid_y][grid_x];
        }
        // Java: 144
        public void occupyGrid(int grid_x, int grid_y, com.oddlabs.tt.pathfinder.Occupant occupant)
        {
            occupyGrid(grid_x, grid_y, occupant, com.oddlabs.tt.pathfinder.UnitGrid.LAND);
        }
        // Java: 148
        public void occupyGrid(int grid_x, int grid_y, com.oddlabs.tt.pathfinder.Occupant occupant, int layer)
        {
            layers[layer].occupants[grid_y][grid_x] = occupant;
        }
        // Java: 153
        public bool isWater(int grid_x, int grid_y)
        {
            return heightmap.getWaterGrid()[grid_y][grid_x] != 0;
        }
        // Java: 157
        public bool isDeepWater(int grid_x, int grid_y)
        {
            return heightmap.getWaterGrid()[grid_y][grid_x] == 2;
        }
        // Java: 161
        public bool isDockable(int grid_x, int grid_y)
        {
            return heightmap.canDock(grid_x, grid_y);
        }
        // Java: 165
        public int getIslandId(int grid_x, int grid_y)
        {
            return heightmap.getIslandId(grid_x, grid_y);
        }
        // Java: 169
        public java.util.List getIslandIds()
        {
            return heightmap.getIslandIds();
        }
        // Java: 173
        public com.oddlabs.tt.landscape.IslandInfo getIslandInfo(int island_id)
        {
            return heightmap.getIslandInfo(island_id);
        }
        // Java: 178
        public int getIslandTreeCount(int island_id)
        {
            return heightmap.getIslandTreeCount(island_id);
        }
        // Java: 183
        public int getIslandRockCount(int island_id)
        {
            return heightmap.getIslandRockCount(island_id);
        }
        // Java: 187
        public int getIslandIronCount(int island_id)
        {
            return heightmap.getIslandIronCount(island_id);
        }
        // Java: 191
        public void freeGrid(int grid_x, int grid_y, com.oddlabs.tt.pathfinder.Occupant occupant)
        {
            freeGrid(grid_x, grid_y, occupant, com.oddlabs.tt.pathfinder.UnitGrid.LAND);
        }
        // Java: 195
        public void freeGrid(int grid_x, int grid_y, com.oddlabs.tt.pathfinder.Occupant occupant, int layer)
        {
            layers[layer].occupants[grid_y][grid_x] = null;
        }
        // Java: 200
        public void debugRenderRegions(float landscape_x, float landscape_y)
        {
            int RADIUS = 30;
            int center_x = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(landscape_x);
            int center_y = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(landscape_y);
            int start_x = java.lang.Math.max(0, center_x - RADIUS);
            int end_x = java.lang.Math.min(layers[com.oddlabs.tt.pathfinder.UnitGrid.LAND].occupants.Length - 0, center_x + RADIUS);
            int start_y = java.lang.Math.max(0, center_y - RADIUS);
            int end_y = java.lang.Math.min(layers[com.oddlabs.tt.pathfinder.UnitGrid.LAND].occupants.Length - 0, center_y + RADIUS);
            com.oddlabs.tt.pathfinder.Region last_region = null;
            for (int y = start_y; y < end_y; y++)
            {
                for (int x = start_x; x < end_x; x++)
                {
                    float xf = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(x);
                    float yf = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(y);
                    float zf = (float)(heightmap.getNearestHeight(xf, yf) + 2.0f);
                    com.oddlabs.tt.pathfinder.Region region = getRegion(x, y);
                    if (region == null)
                    {
                        com.oddlabs.tt.util.DebugRender.drawPoint(xf, yf, zf, 3.0f, 1.0f, 0.0f, 0.0f);
                    }
                    else
                    {
                        last_region = region;
                        org.joml.Vector4fc color = com.oddlabs.tt.util.DebugRender.debug_colors[java.lang.JO.hashCode(region) % com.oddlabs.tt.util.DebugRender.debug_colors.Length];
                        com.oddlabs.tt.util.DebugRender.drawPoint(xf, yf, zf, 3.0f, color.x(), color.y(), color.z());
                    }
                }
            }
            if (last_region != null)
            {
                last_region.debugRenderConnections(heightmap);
                last_region.debugRenderConnectionsReset();
            }
        }
        // Java: 230
        public void debugRenderQuad(int x, int y)
        {
            float OFFSET = 2.0f;
            float RADIUS = 0.5f;
            int s = com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID;
            float xf = (float)(((float)(x + 0.5f)) * s);
            float yf = (float)(((float)(y + 0.5f)) * s);
            float z = (float)(heightmap.getNearestHeight(xf, yf) + OFFSET);
            com.oddlabs.tt.util.DebugRender.drawLine((float)(xf - RADIUS), (float)(yf - RADIUS), z, (float)(xf + RADIUS), (float)(yf + RADIUS), z, 1.0f, 1.0f, 0.0f);
            com.oddlabs.tt.util.DebugRender.drawLine((float)(xf + RADIUS), (float)(yf - RADIUS), z, (float)(xf - RADIUS), (float)(yf + RADIUS), z, 1.0f, 1.0f, 0.0f);
        }
        // Java: 241
        public com.oddlabs.tt.landscape.HeightMap getHeightMap()
        {
            return heightmap;
        }
        // Java: 245
        public void debugRender(float landscape_x, float landscape_y)
        {
            int RADIUS = 30;
            int center_x = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(landscape_x);
            int center_y = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(landscape_y);
            int start_x = java.lang.Math.max(0, center_x - RADIUS);
            int end_x = java.lang.Math.min(layers[com.oddlabs.tt.pathfinder.UnitGrid.LAND].occupants.Length - 0, center_x + RADIUS);
            int start_y = java.lang.Math.max(0, center_y - RADIUS);
            int end_y = java.lang.Math.min(layers[com.oddlabs.tt.pathfinder.UnitGrid.LAND].occupants.Length - 0, center_y + RADIUS);
            for (int y = start_y; y < end_y; y++)
            {
                for (int x = start_x; x < end_x; x++)
                {
                    if (isGridOccupied(x, y))
                    {
                        debugRenderQuad(x, y);
                    }
                }
            }
        }
        public UnitGrid(com.oddlabs.tt.landscape.HeightMap heightmap) : base()
        {
            this.heightmap = heightmap;
            int unit_grid_size = heightmap.getAccessGrid().Length;
            layers = new com.oddlabs.tt.pathfinder.UnitGrid.Layer[2];
            layers[com.oddlabs.tt.pathfinder.UnitGrid.LAND] = new com.oddlabs.tt.pathfinder.UnitGrid.Layer(unit_grid_size);
            layers[com.oddlabs.tt.pathfinder.UnitGrid.SEA] = new com.oddlabs.tt.pathfinder.UnitGrid.Layer(unit_grid_size);
        }
    }
}
