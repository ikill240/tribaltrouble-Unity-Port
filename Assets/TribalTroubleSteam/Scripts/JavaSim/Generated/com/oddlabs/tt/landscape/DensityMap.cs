// Translated from DensityMap.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.landscape
{
    // Java: 18
    public class DensityMap : java.lang.Object
    {
        public com.oddlabs.tt.landscape.World world;
        public com.oddlabs.tt.landscape.HeightMap map;
        public int last_tick;
        public int CHUNK_SIZE;
        public int[][] data;
        public int size;
        public int team;
        public int TOWER_WEIGHT;
        public int PEON_WEIGHT;
        public int ROCK_WEIGHT;
        public int IRON_WEIGHT;
        public int RUBBER_WEIGHT;
        public int CHIEFTAIN_WEIGHT;
        public int OUTSIDE_SCALE;
        // Java: 43
        public int calcWeight(com.oddlabs.tt.model.Selectable s)
        {
            if ((s is com.oddlabs.tt.model.Unit unit))
            {
                System.Type type = unit.getWeaponFactory().getType();
                if (((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)) || ((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RockSpearWeapon)))
                {
                    return ROCK_WEIGHT * OUTSIDE_SCALE;
                }
                else if (((object)type == (object)typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)) || ((object)type == (object)typeof(com.oddlabs.tt.model.weapon.IronSpearWeapon)))
                {
                    return IRON_WEIGHT * OUTSIDE_SCALE;
                }
                else if (((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)) || ((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RubberSpearWeapon)))
                {
                    return RUBBER_WEIGHT * OUTSIDE_SCALE;
                }
                else
                {
                    return PEON_WEIGHT * OUTSIDE_SCALE;
                }
            }
            else if ((s is com.oddlabs.tt.model.LandBuilding building))
            {
                if (!building.isComplete())
                {
                    return 0;
                }
                float health = (float)(building.getHitPoints() / ((float)(((com.oddlabs.tt.model.BuildingTemplate)building.getTemplate()).getMaxHitPoints())));
                int weight = 0;
                if (building.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.SUPPLY_CONTAINER))
                {
                    weight += building.getDeployContainer(com.oddlabs.tt.model.DeployType.ROCK_WARRIOR).getNumSupplies() * ROCK_WEIGHT;
                    weight += building.getDeployContainer(com.oddlabs.tt.model.DeployType.IRON_WARRIOR).getNumSupplies() * IRON_WEIGHT;
                    weight += building.getDeployContainer(com.oddlabs.tt.model.DeployType.RUBBER_WARRIOR).getNumSupplies() * RUBBER_WEIGHT;
                    weight += building.getDeployContainer(com.oddlabs.tt.model.DeployType.PEON).getNumSupplies() * PEON_WEIGHT;
                    int total = building.getUnitContainer().getNumSupplies();
                    int rubber = building.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)).getNumSupplies();
                    rubber += building.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RubberSpearWeapon)).getNumSupplies();
                    rubber = java.lang.Math.min(rubber, total);
                    weight += rubber * RUBBER_WEIGHT;
                    total -= rubber;
                    int iron = building.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)).getNumSupplies();
                    iron += building.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.IronSpearWeapon)).getNumSupplies();
                    iron = java.lang.Math.min(iron, total);
                    weight += iron * IRON_WEIGHT;
                    total -= iron;
                    int rock = building.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)).getNumSupplies();
                    rock += building.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RockSpearWeapon)).getNumSupplies();
                    rock = java.lang.Math.min(rock, total);
                    weight += rock * ROCK_WEIGHT;
                    total -= rock;
                    weight += total * PEON_WEIGHT;
                    return java.lang.StrictMath.round((float)(weight * health));
                }
                else if (building.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.ATTACK))
                {
                    weight += building.getUnitContainer().getNumSupplies() * TOWER_WEIGHT;
                }
                else
                {
                    weight += building.getUnitContainer().getNumSupplies() * PEON_WEIGHT;
                }
                return java.lang.StrictMath.round((float)(weight * health));
            }
            else if ((s is com.oddlabs.tt.model.Ship ship))
            {
                if (!ship.isComplete())
                {
                    return 0;
                }
                com.oddlabs.tt.model.ShipHR hr = ship.getShipHR();
                int weight_2 = 0;
                int peons = hr.countPeons();
                weight_2 += peons * PEON_WEIGHT;
                int rubber_2 = hr.countUnitsOfType(typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon));
                rubber_2 += hr.countUnitsOfType(typeof(com.oddlabs.tt.model.weapon.RubberSpearWeapon));
                int iron_2 = hr.countUnitsOfType(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon));
                iron_2 += hr.countUnitsOfType(typeof(com.oddlabs.tt.model.weapon.IronSpearWeapon));
                int rock_2 = hr.countUnitsOfType(typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon));
                iron_2 += hr.countUnitsOfType(typeof(com.oddlabs.tt.model.weapon.RockSpearWeapon));
                weight_2 += rubber_2 * RUBBER_WEIGHT * OUTSIDE_SCALE;
                weight_2 += iron_2 * IRON_WEIGHT * OUTSIDE_SCALE;
                weight_2 += rock_2 * ROCK_WEIGHT * OUTSIDE_SCALE;
                float health_2 = (float)(ship.getHitPoints() / ((float)(((com.oddlabs.tt.model.BuildingTemplate)ship.getTemplate()).getMaxHitPoints())));
                return java.lang.StrictMath.round((float)(weight_2 * health_2));
            }
            else
            {
                return 0;
            }
        }
        // Java: 116
        public void update()
        {
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    data[y][x] = 0;
                }
            }
            foreach (com.oddlabs.tt.player.Player player in world.getPlayers())
            {
                if (player.getPlayerInfo().getTeam() != team)
                {
                    foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(player.getUnits().getSet()))
                    {
                        int weight = calcWeight(s);
                        int x_2 = java.lang.StrictMath.round((float)(s.getGridX() / ((float)(CHUNK_SIZE))));
                        int y_2 = java.lang.StrictMath.round((float)(s.getGridY() / ((float)(CHUNK_SIZE))));
                        if (x_2 >= 0 && x_2 < size && y_2 >= 0 && y_2 < size)
                        {
                            data[y_2][x_2] += weight;
                        }
                    }
                }
            }
        }
        // Java: 136
        public int sum(int gx0, int gy0, int gx1, int gy1)
        {
            int x0 = java.lang.StrictMath.round((float)(gx0 / ((float)(CHUNK_SIZE))));
            int x1 = java.lang.StrictMath.round((float)(gx1 / ((float)(CHUNK_SIZE))));
            int y0 = java.lang.StrictMath.round((float)(gy0 / ((float)(CHUNK_SIZE))));
            int y1 = java.lang.StrictMath.round((float)(gy1 / ((float)(CHUNK_SIZE))));
            int total = 0;
            for (int y = y0; y <= y1; y++)
            {
                for (int x = x0; x <= x1; x++)
                {
                    total += data[y][x];
                }
            }
            return total;
        }
        // Java: 150
        public int getWeight(int gx, int gy, int r)
        {
            int cx = java.lang.StrictMath.round((float)(gx / ((float)(CHUNK_SIZE))));
            int cy = java.lang.StrictMath.round((float)(gy / ((float)(CHUNK_SIZE))));
            int total = 0;
            for (int y = java.lang.Math.max(cy - r, 0); y <= java.lang.Math.min(cy + r, size - 1); y++)
            {
                for (int x = java.lang.Math.max(cx - r, 0); x <= java.lang.Math.min(cx + r, size - 1); x++)
                {
                    total += data[y][x];
                }
            }
            return total;
        }
        private void init_DensityMap()
        {
            this.last_tick = 0;
            this.CHUNK_SIZE = 32;
            this.TOWER_WEIGHT = 10;
            this.PEON_WEIGHT = 1;
            this.ROCK_WEIGHT = 3;
            this.IRON_WEIGHT = 5;
            this.RUBBER_WEIGHT = 8;
            this.CHIEFTAIN_WEIGHT = 11;
            this.OUTSIDE_SCALE = 2;
        }
        public DensityMap(com.oddlabs.tt.landscape.World world, int team) : base()
        {
            init_DensityMap();
            this.world = world;
            this.map = world.getHeightMap();
            this.team = team;
            this.size = map.getGridUnitsPerWorld() >> 5;
            this.data = java.lang.JA.newArray2<int>(size, size);
        }
    }
}
