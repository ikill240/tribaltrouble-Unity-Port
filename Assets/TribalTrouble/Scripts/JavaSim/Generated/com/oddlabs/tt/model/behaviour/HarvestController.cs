// Translated from HarvestController.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.behaviour
{
    // Java: 9
    public class HarvestController : com.oddlabs.tt.model.behaviour.Controller
    {
        public com.oddlabs.tt.model.Unit unit;
        public System.Type supply_class;
        public com.oddlabs.tt.pathfinder.FinderTrackerAlgorithm tracker;
        public com.oddlabs.tt.model.Supply supply;
        // Java: 23
        public com.oddlabs.tt.model.Supply getSupply()
        {
            return supply;
        }
        // Java: 27
        public void gather()
        {
            if (supply != null && com.oddlabs.tt.model.SupplyFinder.isCrowded(unit, supply))
            {
                supply = null;
            }
            if (supply != null && !supply.isEmpty() && unit.isCloseEnough(0.0f, supply))
            {
                resetGiveUpCounter(0);
                unit.setAvoidedSupply(null);
                unit.setBehaviour(new com.oddlabs.tt.model.behaviour.HarvestBehaviour(unit, supply));
            }
            else if (!shouldGiveUp(0))
            {
                tracker = new com.oddlabs.tt.pathfinder.FinderTrackerAlgorithm(unit.getUnitGrid(), new com.oddlabs.tt.model.SupplyFinder(unit, supply_class));
                unit.setBehaviour(new com.oddlabs.tt.model.behaviour.WalkBehaviour(unit, tracker, false));
            }
            else
            {
                unit.popController();
            }
        }
        // Java: 52
        public override void decide()
        {
            if (((object)unit.getSupplyContainer().getSupplyType() == (object)supply_class) && unit.getSupplyContainer().isSupplyFull())
            {
                unit.popController();
            }
            else
            {
                if (tracker != null)
                {
                    supply = ((com.oddlabs.tt.model.Supply)tracker.getOccupant());
                }
                gather();
            }
        }
        public HarvestController(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.model.Supply supply, System.Type supply_class) : base(1)
        {
            this.unit = unit;
            this.supply = supply;
            this.supply_class = supply_class;
        }
    }
}
