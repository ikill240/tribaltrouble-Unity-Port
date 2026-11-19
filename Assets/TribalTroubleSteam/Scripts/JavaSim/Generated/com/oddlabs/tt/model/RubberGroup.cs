// Translated from RubberGroup.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 13
    public class RubberGroup : java.lang.Object
    {
        public const int MIN_CHICKENS_PER_GROUP = 3;
        public const int MAX_CHICKENS_PER_GROUP = 7;
        public const float SPAWN_TIME = 2.0f;
        public com.oddlabs.tt.landscape.World world;
        public java.util.List supplies;
        // Java: 46
        public int[] getGroupPosition()
        {
            java.util.List tree_positions = world.getHeightMap().getTrees();
            int start_index = world.getRandom().nextInt(tree_positions.size());
            int index = (start_index + 1) % tree_positions.size();
            while (index != start_index)
            {
                int[] coords = ((int[])tree_positions.get(index));
                com.oddlabs.tt.pathfinder.Occupant occ = world.getUnitGrid().getOccupant(coords[0], coords[1]);
                if ((occ is com.oddlabs.tt.landscape.TreeSupply))
                {
                    return coords;
                }
                index = (index + 1) % tree_positions.size();
            }
            return null;
        }
        // Java: 60
        public void remove(com.oddlabs.tt.model.RubberSupply supply)
        {
            bool in_list = supplies.remove(supply);
            if (supplies.isEmpty())
            {
                (((com.oddlabs.tt.model.RubberSupplyManager)world.getSupplyManager(typeof(com.oddlabs.tt.model.RubberSupply)))).emptyGroup();
            }
        }
        private void init_RubberGroup()
        {
            this.supplies = new java.util.ArrayList();
        }
        public RubberGroup(com.oddlabs.tt.landscape.World world) : base()
        {
            init_RubberGroup();
            this.world = world;
            int[] group_position = getGroupPosition();
            if (group_position != null)
            {
                int num_supplies = com.oddlabs.tt.model.RubberGroup.MIN_CHICKENS_PER_GROUP + world.getRandom().nextInt(com.oddlabs.tt.model.RubberGroup.MAX_CHICKENS_PER_GROUP - com.oddlabs.tt.model.RubberGroup.MIN_CHICKENS_PER_GROUP + 1);
                com.oddlabs.tt.util.Target[] supply_positions = world.getUnitGrid().findGridTargets(group_position[0], group_position[1], num_supplies, true);
                float spawn_x = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(group_position[0]);
                float spawn_y = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(group_position[1]);
                for (int i = 0; i < num_supplies; i++)
                {
                    int grid_x = supply_positions[i].getGridX();
                    int grid_y = supply_positions[i].getGridY();
                    float x = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(grid_x);
                    float y = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(grid_y);
                    com.oddlabs.tt.model.RubberSupply supply = new com.oddlabs.tt.model.RubberSupply(world, world.getLandscapeResources().getChicken(), 2.0f, grid_x, grid_y, x, y, 0.0f, this, spawn_x, spawn_y);
                    supplies.add(supply);
                    new com.oddlabs.tt.model.SupplySpawnAnimation(supply, com.oddlabs.tt.model.RubberGroup.SPAWN_TIME);
                }
                (((com.oddlabs.tt.model.RubberSupplyManager)world.getSupplyManager(typeof(com.oddlabs.tt.model.RubberSupply)))).newGroup();
            }
        }
    }
}
