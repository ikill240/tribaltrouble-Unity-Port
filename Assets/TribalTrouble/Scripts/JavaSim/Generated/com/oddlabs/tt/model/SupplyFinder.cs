// Translated from SupplyFinder.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 15
    public class SupplyFinder : java.lang.Object, com.oddlabs.tt.pathfinder.FinderFilter
    {
        public com.oddlabs.tt.model.Unit unit;
        public System.Type supply_class;
        public java.util.List region_list;
        public int max_region_dist_sqr;
        // Java: 26
        public com.oddlabs.tt.pathfinder.Occupant getOccupantFromRegion(com.oddlabs.tt.pathfinder.Region region, bool one_region)
        {
            java.util.List supplies = region.getObjects(supply_class);
            if (one_region)
            {
                if (!supplies.isEmpty())
                {
                    com.oddlabs.tt.model.Supply supply = findClosest(supplies);
                    return supply;
                }
            }
            else
            {
                int dx = region.getGridX() - unit.getGridX();
                int dy = region.getGridY() - unit.getGridY();
                int region_dist_sqr = dx * dx + dy * dy;
                if (!supplies.isEmpty())
                {
                    if (region_list.isEmpty())
                    {
                        int region_dist = java.lang.JC.d2i(java.lang.Math.sqrt(region_dist_sqr));
                        int max_region_dist = region_dist + com.oddlabs.tt.pathfinder.RegionBuilder.REGION_PATH_MAX_COST / 2;
                        max_region_dist_sqr = max_region_dist * max_region_dist;
                    }
                    region_list.add(supplies);
                }
                if (!region_list.isEmpty() && region_dist_sqr > max_region_dist_sqr)
                {
                    com.oddlabs.tt.model.Supply supply_2 = findClosest();
                    return supply_2;
                }
            }
            return null;
        }
        // Java: 56
        public com.oddlabs.tt.pathfinder.Occupant getBest()
        {
            return findClosest();
        }
        // Java: 61
        public com.oddlabs.tt.model.Supply findClosest(java.util.List supplies)
        {
            return ((com.oddlabs.tt.model.Supply)supplies.stream().min(java.util.Comparator_.comparingInt(new java.util.function.ToIntFunction_L((object a0) => { return this.distanceSquared(((com.oddlabs.tt.model.Supply)a0)); }))).orElse(null));
        }
        // Java: 65
        public com.oddlabs.tt.model.Supply findClosest()
        {
            com.oddlabs.tt.model.Supply closest = ((com.oddlabs.tt.model.Supply)region_list.stream().flatMap(new java.util.function.Function_L((object a0) => { return ((java.util.List)a0).stream(); })).min(java.util.Comparator_.comparingInt(new java.util.function.ToIntFunction_L((object a0) => { return this.distanceSquared(((com.oddlabs.tt.model.Supply)a0)); }))).orElse(null));
            region_list.clear();
            return closest;
        }
        // Java: 72
        public int distanceSquared(com.oddlabs.tt.model.Supply supply)
        {
            int dx = supply.getGridX() - unit.getGridX();
            int dy = supply.getGridY() - unit.getGridY();
            return dx * dx + dy * dy;
        }
        public const int MAX_GATHERERS_PER_SUPPLY = 7;
        // Java: 103
        public static bool isCrowded(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.model.Supply supply)
        {
            if (unit.isCloseEnough(0.0f, supply))
            {
                return false;
            }
            int capacity = java.lang.Math.min(com.oddlabs.tt.model.SupplyFinder.MAX_GATHERERS_PER_SUPPLY, com.oddlabs.tt.model.SupplyFinder.harvestSpots(unit.getUnitGrid(), supply));
            if (capacity == 0)
            {
                return true;
            }
            float supply_x = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(supply.getGridX());
            float supply_y = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(supply.getGridY());
            com.oddlabs.tt.pathfinder.FindOccupantFilter nearby = new com.oddlabs.tt.pathfinder.FindOccupantFilter(supply_x, supply_y, 4.0f, null, typeof(com.oddlabs.tt.model.Unit));
            unit.getUnitGrid().scan(nearby, supply.getGridX(), supply.getGridY());
            int at_supply = 0;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(nearby.getResult()))
            {
                if (!u.isDead() && ((object)u != (object)unit) && u.isCloseEnough(0.0f, supply) && (u.getCurrentController() is com.oddlabs.tt.model.behaviour.HarvestController hc) && ((object)hc.getSupply() == (object)supply) && ++at_supply >= capacity)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 126
        public static int harvestSpots(com.oddlabs.tt.pathfinder.UnitGrid grid, com.oddlabs.tt.model.Supply supply)
        {
            int gx = supply.getGridX();
            int gy = supply.getGridY();
            if (((object)grid.getOccupant(gx, gy) != (object)supply))
            {
                return com.oddlabs.tt.model.SupplyFinder.MAX_GATHERERS_PER_SUPPLY;
            }
            int size = grid.getGridSize();
            int spots = 0;
            for (int dy = (-1); dy <= 1; dy++)
            {
                for (int dx = (-1); dx <= 1; dx++)
                {
                    int x = gx + dx;
                    int y = gy + dy;
                    if ((dx == 0 && dy == 0) || x < 0 || y < 0 || x >= size || y >= size)
                    {
                        continue;
                    }
                    com.oddlabs.tt.pathfinder.Occupant occ = grid.getOccupant(x, y);
                    if (occ == null || (occ is com.oddlabs.tt.pathfinder.Movable))
                    {
                        spots++;
                    }
                }
            }
            return spots;
        }
        // Java: 146
        public bool acceptOccupant(com.oddlabs.tt.pathfinder.Occupant occ)
        {
            if (supply_class.isInstance(occ))
            {
                com.oddlabs.tt.model.Supply supply = ((com.oddlabs.tt.model.Supply)occ);
                if (((object)supply == (object)unit.getAvoidedSupply()))
                {
                    return false;
                }
                return !com.oddlabs.tt.model.SupplyFinder.isCrowded(unit, supply);
            }
            else
            {
                return false;
            }
        }
        private void init_SupplyFinder()
        {
            this.region_list = new java.util.ArrayList();
        }
        public SupplyFinder(com.oddlabs.tt.model.Unit unit, System.Type supply_class) : base()
        {
            init_SupplyFinder();
            this.unit = unit;
            this.supply_class = supply_class;
        }
    }
}
