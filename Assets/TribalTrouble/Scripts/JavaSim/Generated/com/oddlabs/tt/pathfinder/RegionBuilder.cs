// Translated from RegionBuilder.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.pathfinder
{
    // Java: 11
    public class RegionBuilder : java.lang.Object
    {
        public const int MAX_EXAMINED_NODES_PER_PATH = 600;
        public const int REGION_PATH_MAX_COST = 70;
        public const int MAX_PATH_COST = 2048;
        public const int GRID_SIZE = 128;
        public const int DIAGONAL = 3;
        public const int STRAIGHT = 2;
        public static com.oddlabs.tt.pathfinder.Occupant unreachable_obj;
        // Java: 23
        public static com.oddlabs.tt.pathfinder.Occupant unreachable()
        {
            return com.oddlabs.tt.pathfinder.RegionBuilder.unreachable_obj;
        }
        // Java: 27
        public static void buildRegions(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, float start_x_f, float start_y_f)
        {
            bool[][] access_grid = unit_grid.getHeightMap().getAccessGrid();
            int grid_size = access_grid.Length;
            int start_x = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(start_x_f);
            int start_y = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(start_y_f);
            com.oddlabs.tt.pathfinder.RegionBuilderNode[][] dir_finder_grid = java.lang.JA.newArray2<com.oddlabs.tt.pathfinder.RegionBuilderNode>(grid_size, grid_size);
            int num_occupied = 0;
            for (int y = 0; y < grid_size; y++)
            {
                for (int x = 0; x < grid_size; x++)
                {
                    com.oddlabs.tt.pathfinder.RegionBuilderNode finder_node = new com.oddlabs.tt.pathfinder.RegionBuilderNode(x, y);
                    dir_finder_grid[y][x] = finder_node;
                    if (!access_grid[y][x])
                    {
                        unit_grid.occupyGrid(finder_node.getGridX(), finder_node.getGridY(), com.oddlabs.tt.pathfinder.RegionBuilder.unreachable_obj);
                        num_occupied++;
                    }
                }
            }
            com.oddlabs.tt.pathfinder.RegionBuilderNode start_node = dir_finder_grid[start_y][start_x];
            com.oddlabs.tt.pathfinder.QueueArray start_nodes = new com.oddlabs.tt.pathfinder.QueueArray(grid_size * grid_size);
            com.oddlabs.tt.util.PocketList region_nodes = new com.oddlabs.tt.util.PocketList(grid_size);
            start_nodes.addLast(start_node);
            int actual_num_regions = 0;
            while ((start_node = com.oddlabs.tt.pathfinder.RegionBuilder.findStartNode(unit_grid, region_nodes, start_nodes)) != null)
            {
                com.oddlabs.tt.pathfinder.Region region = new com.oddlabs.tt.pathfinder.Region();
                com.oddlabs.tt.pathfinder.RegionBuilder.addRegionNodes(unit_grid, dir_finder_grid, start_nodes, region, start_node.getGridX(), start_node.getGridY(), region_nodes);
                actual_num_regions++;
            }
            for (int y_2 = 0; y_2 < grid_size; y_2++)
            {
                for (int x_2 = 0; x_2 < grid_size; x_2++)
                {
                    com.oddlabs.tt.pathfinder.Region region_2 = unit_grid.getRegion(x_2, y_2);
                    if (region_2 != null)
                    {
                        com.oddlabs.tt.pathfinder.RegionBuilder.updateRegionNeighbours(unit_grid, x_2, y_2, region_2);
                    }
                }
            }
            com.oddlabs.tt.pathfinder.RegionBuilder.buildWaterRegions(unit_grid);
            com.oddlabs.tt.form.ProgressForm.progress(1.0f);
            java.lang.IO.println("actual_num_regions = " + (actual_num_regions));
        }
        // Java: 71
        public static void buildRegions(com.oddlabs.tt.pathfinder.UnitGrid unit_grid)
        {
            bool[][] access_grid = unit_grid.getHeightMap().getAccessGrid();
            java.util.List island_infos = unit_grid.getHeightMap().getIslandInfos();
            int grid_size = access_grid.Length;
            com.oddlabs.tt.pathfinder.RegionBuilderNode[][] dir_finder_grid = java.lang.JA.newArray2<com.oddlabs.tt.pathfinder.RegionBuilderNode>(grid_size, grid_size);
            int num_occupied = 0;
            for (int y = 0; y < grid_size; y++)
            {
                for (int x = 0; x < grid_size; x++)
                {
                    com.oddlabs.tt.pathfinder.RegionBuilderNode finder_node = new com.oddlabs.tt.pathfinder.RegionBuilderNode(x, y);
                    dir_finder_grid[y][x] = finder_node;
                    if (!access_grid[y][x])
                    {
                        unit_grid.occupyGrid(finder_node.getGridX(), finder_node.getGridY(), com.oddlabs.tt.pathfinder.RegionBuilder.unreachable_obj);
                        num_occupied++;
                    }
                }
            }
            com.oddlabs.tt.form.ProgressForm.progress(0.5f);
            int actual_num_regions = 0;
            foreach (com.oddlabs.tt.landscape.IslandInfo island_info in java.lang.JIter.of(island_infos))
            {
                com.oddlabs.tt.pathfinder.RegionBuilderNode start_node = dir_finder_grid[island_info.startY()][island_info.startX()];
                com.oddlabs.tt.pathfinder.QueueArray start_nodes = new com.oddlabs.tt.pathfinder.QueueArray(grid_size * grid_size);
                com.oddlabs.tt.util.PocketList region_nodes = new com.oddlabs.tt.util.PocketList(grid_size);
                start_nodes.addLast(start_node);
                while ((start_node = com.oddlabs.tt.pathfinder.RegionBuilder.findStartNode(unit_grid, region_nodes, start_nodes)) != null)
                {
                    com.oddlabs.tt.pathfinder.Region region = new com.oddlabs.tt.pathfinder.Region();
                    com.oddlabs.tt.pathfinder.RegionBuilder.addRegionNodes(unit_grid, dir_finder_grid, start_nodes, region, start_node.getGridX(), start_node.getGridY(), region_nodes);
                    actual_num_regions++;
                }
            }
            for (int y_2 = 0; y_2 < grid_size; y_2++)
            {
                for (int x_2 = 0; x_2 < grid_size; x_2++)
                {
                    com.oddlabs.tt.pathfinder.Region region_2 = unit_grid.getRegion(x_2, y_2);
                    if (region_2 != null)
                    {
                        com.oddlabs.tt.pathfinder.RegionBuilder.updateRegionNeighbours(unit_grid, x_2, y_2, region_2);
                    }
                }
            }
            com.oddlabs.tt.pathfinder.RegionBuilder.buildWaterRegions(unit_grid);
            com.oddlabs.tt.form.ProgressForm.progress(0.5f);
            java.lang.IO.println("actual_num_regions = " + (actual_num_regions));
        }
        // Java: 126
        public static void buildWaterRegions(com.oddlabs.tt.pathfinder.UnitGrid unit_grid)
        {
            sbyte[][] water_grid = unit_grid.getHeightMap().getWaterGrid();
            int grid_size = water_grid.Length;
            com.oddlabs.tt.pathfinder.RegionBuilderNode[][] dir_finder_grid = java.lang.JA.newArray2<com.oddlabs.tt.pathfinder.RegionBuilderNode>(grid_size, grid_size);
            for (int y = 0; y < grid_size; y++)
            {
                for (int x = 0; x < grid_size; x++)
                {
                    com.oddlabs.tt.pathfinder.RegionBuilderNode finder_node = new com.oddlabs.tt.pathfinder.RegionBuilderNode(x, y);
                    dir_finder_grid[y][x] = finder_node;
                    if (water_grid[y][x] != 2)
                    {
                        unit_grid.occupyGrid(finder_node.getGridX(), finder_node.getGridY(), com.oddlabs.tt.pathfinder.RegionBuilder.unreachable_obj, com.oddlabs.tt.pathfinder.UnitGrid.SEA);
                    }
                }
            }
            com.oddlabs.tt.pathfinder.RegionBuilderNode start_node = dir_finder_grid[0][0];
            com.oddlabs.tt.pathfinder.QueueArray start_nodes = new com.oddlabs.tt.pathfinder.QueueArray(grid_size * grid_size);
            com.oddlabs.tt.util.PocketList region_nodes = new com.oddlabs.tt.util.PocketList(grid_size);
            start_nodes.addLast(start_node);
            int actual_num_regions = 0;
            while ((start_node = com.oddlabs.tt.pathfinder.RegionBuilder.findStartNodeWater(unit_grid, region_nodes, start_nodes)) != null)
            {
                com.oddlabs.tt.pathfinder.Region region = new com.oddlabs.tt.pathfinder.Region();
                com.oddlabs.tt.pathfinder.RegionBuilder.addRegionNodesWater(unit_grid, dir_finder_grid, start_nodes, region, start_node.getGridX(), start_node.getGridY(), region_nodes);
                actual_num_regions++;
            }
            for (int y_2 = 0; y_2 < grid_size; y_2++)
            {
                for (int x_2 = 0; x_2 < grid_size; x_2++)
                {
                    com.oddlabs.tt.pathfinder.Region region_2 = unit_grid.getRegion(x_2, y_2, com.oddlabs.tt.pathfinder.UnitGrid.SEA);
                    if (region_2 != null)
                    {
                        com.oddlabs.tt.pathfinder.RegionBuilder.updateRegionNeighboursWater(unit_grid, x_2, y_2, region_2);
                    }
                }
            }
            java.lang.IO.println("actual_num_water_regions = " + (actual_num_regions));
        }
        // Java: 162
        public static void testNeighbourWater(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, int grid_x, int grid_y, com.oddlabs.tt.pathfinder.Region region)
        {
            int grid_size = unit_grid.getGridSize();
            if (grid_x < 0 || grid_y < 0 || grid_x >= grid_size || grid_y >= grid_size)
            {
                return;
            }
            com.oddlabs.tt.pathfinder.Region neighbour_region = unit_grid.getRegion(grid_x, grid_y, com.oddlabs.tt.pathfinder.UnitGrid.SEA);
            // (Steam 2.2.207 fix: boats got stuck where the network linked two sea regions with land between them)
            if (neighbour_region == null || ((object)neighbour_region == (object)region) || neighbour_region.isNeighbour(region))
            {
                return;
            }
            com.oddlabs.tt.pathfinder.ShipTrajectoryPoint p0 = new com.oddlabs.tt.pathfinder.ShipTrajectoryPoint(region.getGridX(), region.getGridY());
            com.oddlabs.tt.pathfinder.ShipTrajectoryPoint p1 = new com.oddlabs.tt.pathfinder.ShipTrajectoryPoint(neighbour_region.getGridX(), neighbour_region.getGridY());
            if (!com.oddlabs.tt.pathfinder.ShipTrajectory.checkLandCollision(unit_grid, p0, p1))
            {
                com.oddlabs.tt.pathfinder.Region.link(neighbour_region, region);
            }
        }
        // Java: 170
        public static void updateRegionNeighboursWater(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, int grid_x, int grid_y, com.oddlabs.tt.pathfinder.Region region)
        {
            com.oddlabs.tt.pathfinder.RegionBuilder.testNeighbourWater(unit_grid, grid_x + 1, grid_y, region);
            com.oddlabs.tt.pathfinder.RegionBuilder.testNeighbourWater(unit_grid, grid_x + 1, grid_y + 1, region);
            com.oddlabs.tt.pathfinder.RegionBuilder.testNeighbourWater(unit_grid, grid_x, grid_y + 1, region);
            com.oddlabs.tt.pathfinder.RegionBuilder.testNeighbourWater(unit_grid, grid_x - 1, grid_y + 1, region);
            com.oddlabs.tt.pathfinder.RegionBuilder.testNeighbourWater(unit_grid, grid_x - 1, grid_y, region);
            com.oddlabs.tt.pathfinder.RegionBuilder.testNeighbourWater(unit_grid, grid_x - 1, grid_y - 1, region);
            com.oddlabs.tt.pathfinder.RegionBuilder.testNeighbourWater(unit_grid, grid_x, grid_y - 1, region);
            com.oddlabs.tt.pathfinder.RegionBuilder.testNeighbourWater(unit_grid, grid_x + 1, grid_y - 1, region);
        }
        // Java: 182
        public static void addRegionNodesWater(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, com.oddlabs.tt.pathfinder.RegionBuilderNode[][] dir_finder_grid, com.oddlabs.tt.pathfinder.QueueArray start_nodes, com.oddlabs.tt.pathfinder.Region region, int start_x, int start_y, com.oddlabs.tt.util.PocketList region_nodes)
        {
            int min_x = start_x;
            int max_x = start_x;
            int min_y = start_y;
            int max_y = start_y;
            while (!region_nodes.isEmpty())
            {
                com.oddlabs.tt.pathfinder.RegionBuilderNode node = ((com.oddlabs.tt.pathfinder.RegionBuilderNode)region_nodes.removeBest());
                if (unit_grid.getRegion(node.getGridX(), node.getGridY(), com.oddlabs.tt.pathfinder.UnitGrid.SEA) != null)
                {
                    continue;
                }
                if (node.getTotalCost() > com.oddlabs.tt.pathfinder.RegionBuilder.REGION_PATH_MAX_COST)
                {
                    start_nodes.addLast(node);
                    continue;
                }
                int nx = node.getGridX();
                int ny = node.getGridY();
                if (max_x < nx)
                {
                    max_x = nx;
                }
                if (min_x > nx)
                {
                    min_x = nx;
                }
                if (max_y < ny)
                {
                    max_y = ny;
                }
                if (min_y > ny)
                {
                    min_y = ny;
                }
                unit_grid.setRegion(node.getGridX(), node.getGridY(), region, com.oddlabs.tt.pathfinder.UnitGrid.SEA);
                com.oddlabs.tt.pathfinder.RegionBuilder.addNeighboursWater(unit_grid, dir_finder_grid, region_nodes, node);
            }
            region.setPosition((max_x + min_x) / 2, (max_y + min_y) / 2);
        }
        // Java: 215
        public static void addNeighbourWater(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, com.oddlabs.tt.pathfinder.RegionBuilderNode[][] dir_finder_grid, com.oddlabs.tt.util.PocketList region_nodes, int x, int y, int cost)
        {
            if (x < 0 || y < 0 || y >= dir_finder_grid.Length || x >= dir_finder_grid[y].Length)
            {
                return;
            }
            com.oddlabs.tt.pathfinder.RegionBuilderNode node = dir_finder_grid[y][x];
            if (unit_grid.getRegion(node.getGridX(), node.getGridY(), com.oddlabs.tt.pathfinder.UnitGrid.SEA) != null)
            {
                return;
            }
            node.setTotalCost(cost);
            if (!unit_grid.isGridOccupied(node.getGridX(), node.getGridY(), com.oddlabs.tt.pathfinder.UnitGrid.SEA))
            {
                region_nodes.add(node.getTotalCost(), node);
            }
        }
        // Java: 228
        public static void addNeighboursWater(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, com.oddlabs.tt.pathfinder.RegionBuilderNode[][] dir_finder_grid, com.oddlabs.tt.util.PocketList region_nodes, com.oddlabs.tt.pathfinder.RegionBuilderNode node)
        {
            int x = node.getGridX();
            int y = node.getGridY();
            int cost = node.getTotalCost();
            com.oddlabs.tt.pathfinder.RegionBuilder.addNeighbourWater(unit_grid, dir_finder_grid, region_nodes, x - 1, y - 1, cost + com.oddlabs.tt.pathfinder.RegionBuilder.DIAGONAL);
            com.oddlabs.tt.pathfinder.RegionBuilder.addNeighbourWater(unit_grid, dir_finder_grid, region_nodes, x - 1, y, cost + com.oddlabs.tt.pathfinder.RegionBuilder.STRAIGHT);
            com.oddlabs.tt.pathfinder.RegionBuilder.addNeighbourWater(unit_grid, dir_finder_grid, region_nodes, x - 1, y + 1, cost + com.oddlabs.tt.pathfinder.RegionBuilder.DIAGONAL);
            com.oddlabs.tt.pathfinder.RegionBuilder.addNeighbourWater(unit_grid, dir_finder_grid, region_nodes, x, y - 1, cost + com.oddlabs.tt.pathfinder.RegionBuilder.STRAIGHT);
            com.oddlabs.tt.pathfinder.RegionBuilder.addNeighbourWater(unit_grid, dir_finder_grid, region_nodes, x, y + 1, cost + com.oddlabs.tt.pathfinder.RegionBuilder.STRAIGHT);
            com.oddlabs.tt.pathfinder.RegionBuilder.addNeighbourWater(unit_grid, dir_finder_grid, region_nodes, x + 1, y - 1, cost + com.oddlabs.tt.pathfinder.RegionBuilder.DIAGONAL);
            com.oddlabs.tt.pathfinder.RegionBuilder.addNeighbourWater(unit_grid, dir_finder_grid, region_nodes, x + 1, y, cost + com.oddlabs.tt.pathfinder.RegionBuilder.STRAIGHT);
            com.oddlabs.tt.pathfinder.RegionBuilder.addNeighbourWater(unit_grid, dir_finder_grid, region_nodes, x + 1, y + 1, cost + com.oddlabs.tt.pathfinder.RegionBuilder.DIAGONAL);
        }
        // Java: 244
        public static com.oddlabs.tt.pathfinder.RegionBuilderNode findStartNodeWater(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, com.oddlabs.tt.util.PocketList region_nodes, com.oddlabs.tt.pathfinder.QueueArray start_nodes)
        {
            region_nodes.clear();
            while (!start_nodes.isEmpty())
            {
                com.oddlabs.tt.pathfinder.RegionBuilderNode node = ((com.oddlabs.tt.pathfinder.RegionBuilderNode)start_nodes.removeFirst());
                if (unit_grid.getRegion(node.getGridX(), node.getGridY(), com.oddlabs.tt.pathfinder.UnitGrid.SEA) == null)
                {
                    node.setTotalCost(0);
                    region_nodes.add(node.getTotalCost(), node);
                    return node;
                }
            }
            return null;
        }
        // Java: 258
        public static void testNeighbour(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, int grid_x, int grid_y, com.oddlabs.tt.pathfinder.Region region)
        {
            com.oddlabs.tt.pathfinder.Region neighbour_region = unit_grid.getRegion(grid_x, grid_y);
            com.oddlabs.tt.pathfinder.Region.link(neighbour_region, region);
        }
        // Java: 263
        public static void updateRegionNeighbours(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, int grid_x, int grid_y, com.oddlabs.tt.pathfinder.Region region)
        {
            com.oddlabs.tt.pathfinder.RegionBuilder.testNeighbour(unit_grid, grid_x + 1, grid_y, region);
            com.oddlabs.tt.pathfinder.RegionBuilder.testNeighbour(unit_grid, grid_x + 1, grid_y + 1, region);
            com.oddlabs.tt.pathfinder.RegionBuilder.testNeighbour(unit_grid, grid_x, grid_y + 1, region);
            com.oddlabs.tt.pathfinder.RegionBuilder.testNeighbour(unit_grid, grid_x - 1, grid_y + 1, region);
            com.oddlabs.tt.pathfinder.RegionBuilder.testNeighbour(unit_grid, grid_x - 1, grid_y, region);
            com.oddlabs.tt.pathfinder.RegionBuilder.testNeighbour(unit_grid, grid_x - 1, grid_y - 1, region);
            com.oddlabs.tt.pathfinder.RegionBuilder.testNeighbour(unit_grid, grid_x, grid_y - 1, region);
            com.oddlabs.tt.pathfinder.RegionBuilder.testNeighbour(unit_grid, grid_x + 1, grid_y - 1, region);
        }
        // Java: 274
        public static void addRegionNodes(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, com.oddlabs.tt.pathfinder.RegionBuilderNode[][] dir_finder_grid, com.oddlabs.tt.pathfinder.QueueArray start_nodes, com.oddlabs.tt.pathfinder.Region region, int start_x, int start_y, com.oddlabs.tt.util.PocketList region_nodes)
        {
            int min_x = start_x;
            int max_x = start_x;
            int min_y = start_y;
            int max_y = start_y;
            while (!region_nodes.isEmpty())
            {
                com.oddlabs.tt.pathfinder.RegionBuilderNode node = ((com.oddlabs.tt.pathfinder.RegionBuilderNode)region_nodes.removeBest());
                if (unit_grid.getRegion(node.getGridX(), node.getGridY()) != null)
                {
                    continue;
                }
                if (node.getTotalCost() > com.oddlabs.tt.pathfinder.RegionBuilder.REGION_PATH_MAX_COST)
                {
                    start_nodes.addLast(node);
                    continue;
                }
                int nx = node.getGridX();
                int ny = node.getGridY();
                if (max_x < nx)
                {
                    max_x = nx;
                }
                if (min_x > nx)
                {
                    min_x = nx;
                }
                if (max_y < ny)
                {
                    max_y = ny;
                }
                if (min_y > ny)
                {
                    min_y = ny;
                }
                unit_grid.setRegion(node.getGridX(), node.getGridY(), region);
                com.oddlabs.tt.pathfinder.RegionBuilder.addNeighbours(unit_grid, dir_finder_grid, region_nodes, node);
            }
            region.setPosition((max_x + min_x) / 2, (max_y + min_y) / 2);
        }
        // Java: 307
        public static void addNeighbour(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, com.oddlabs.tt.pathfinder.RegionBuilderNode[][] dir_finder_grid, com.oddlabs.tt.util.PocketList region_nodes, int x, int y, int cost)
        {
            com.oddlabs.tt.pathfinder.RegionBuilderNode node = dir_finder_grid[y][x];
            if (unit_grid.getRegion(node.getGridX(), node.getGridY()) != null)
            {
                return;
            }
            node.setTotalCost(cost);
            if (!unit_grid.isGridOccupied(node.getGridX(), node.getGridY()))
            {
                region_nodes.add(node.getTotalCost(), node);
            }
        }
        // Java: 318
        public static void addNeighbours(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, com.oddlabs.tt.pathfinder.RegionBuilderNode[][] dir_finder_grid, com.oddlabs.tt.util.PocketList region_nodes, com.oddlabs.tt.pathfinder.RegionBuilderNode node)
        {
            int x = node.getGridX();
            int y = node.getGridY();
            int cost = node.getTotalCost();
            com.oddlabs.tt.pathfinder.RegionBuilder.addNeighbour(unit_grid, dir_finder_grid, region_nodes, x - 1, y - 1, cost + com.oddlabs.tt.pathfinder.RegionBuilder.DIAGONAL);
            com.oddlabs.tt.pathfinder.RegionBuilder.addNeighbour(unit_grid, dir_finder_grid, region_nodes, x - 1, y, cost + com.oddlabs.tt.pathfinder.RegionBuilder.STRAIGHT);
            com.oddlabs.tt.pathfinder.RegionBuilder.addNeighbour(unit_grid, dir_finder_grid, region_nodes, x - 1, y + 1, cost + com.oddlabs.tt.pathfinder.RegionBuilder.DIAGONAL);
            com.oddlabs.tt.pathfinder.RegionBuilder.addNeighbour(unit_grid, dir_finder_grid, region_nodes, x, y - 1, cost + com.oddlabs.tt.pathfinder.RegionBuilder.STRAIGHT);
            com.oddlabs.tt.pathfinder.RegionBuilder.addNeighbour(unit_grid, dir_finder_grid, region_nodes, x, y + 1, cost + com.oddlabs.tt.pathfinder.RegionBuilder.STRAIGHT);
            com.oddlabs.tt.pathfinder.RegionBuilder.addNeighbour(unit_grid, dir_finder_grid, region_nodes, x + 1, y - 1, cost + com.oddlabs.tt.pathfinder.RegionBuilder.DIAGONAL);
            com.oddlabs.tt.pathfinder.RegionBuilder.addNeighbour(unit_grid, dir_finder_grid, region_nodes, x + 1, y, cost + com.oddlabs.tt.pathfinder.RegionBuilder.STRAIGHT);
            com.oddlabs.tt.pathfinder.RegionBuilder.addNeighbour(unit_grid, dir_finder_grid, region_nodes, x + 1, y + 1, cost + com.oddlabs.tt.pathfinder.RegionBuilder.DIAGONAL);
        }
        // Java: 334
        public static com.oddlabs.tt.pathfinder.RegionBuilderNode findStartNode(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, com.oddlabs.tt.util.PocketList region_nodes, com.oddlabs.tt.pathfinder.QueueArray start_nodes)
        {
            region_nodes.clear();
            while (!start_nodes.isEmpty())
            {
                com.oddlabs.tt.pathfinder.RegionBuilderNode node = ((com.oddlabs.tt.pathfinder.RegionBuilderNode)start_nodes.removeFirst());
                if (unit_grid.getRegion(node.getGridX(), node.getGridY()) == null)
                {
                    node.setTotalCost(0);
                    region_nodes.add(node.getTotalCost(), node);
                    return node;
                }
            }
            return null;
        }
        static RegionBuilder()
        {
            RegionBuilder.unreachable_obj = new com.oddlabs.tt.pathfinder.StaticOccupant();
        }
        public RegionBuilder() : base()
        {
        }
    }
}
