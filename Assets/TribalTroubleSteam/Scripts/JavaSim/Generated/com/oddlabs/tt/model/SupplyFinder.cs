// Translated from SupplyFinder.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 14
    public class SupplyFinder : java.lang.Object, com.oddlabs.tt.pathfinder.FinderFilter
    {
        public com.oddlabs.tt.model.Unit unit;
        public System.Type supply_class;
        public java.util.List region_list;
        public int max_region_dist_sqr;
        // Java: 25
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
        // Java: 55
        public com.oddlabs.tt.pathfinder.Occupant getBest()
        {
            return findClosest();
        }
        // Java: 60
        public com.oddlabs.tt.model.Supply findClosest(java.util.List supplies)
        {
            return ((com.oddlabs.tt.model.Supply)supplies.stream().min(java.util.Comparator_.comparingInt(new java.util.function.ToIntFunction_L((object a0) => { return this.distanceSquared(((com.oddlabs.tt.model.Supply)a0)); }))).orElse(null));
        }
        // Java: 64
        public com.oddlabs.tt.model.Supply findClosest()
        {
            com.oddlabs.tt.model.Supply closest = ((com.oddlabs.tt.model.Supply)region_list.stream().flatMap(new java.util.function.Function_L((object a0) => { return ((java.util.List)a0).stream(); })).min(java.util.Comparator_.comparingInt(new java.util.function.ToIntFunction_L((object a0) => { return this.distanceSquared(((com.oddlabs.tt.model.Supply)a0)); }))).orElse(null));
            region_list.clear();
            return closest;
        }
        // Java: 71
        public int distanceSquared(com.oddlabs.tt.model.Supply supply)
        {
            int dx = supply.getGridX() - unit.getGridX();
            int dy = supply.getGridY() - unit.getGridY();
            return dx * dx + dy * dy;
        }
        // Java: 77
        public bool acceptOccupant(com.oddlabs.tt.pathfinder.Occupant occ)
        {
            if (supply_class.isInstance(occ))
            {
                com.oddlabs.tt.model.Supply supply = ((com.oddlabs.tt.model.Supply)occ);
                return true;
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
