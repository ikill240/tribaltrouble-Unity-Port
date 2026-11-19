// Translated from TransferUnitController.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.behaviour
{
    // Java: 9
    public class TransferUnitController : com.oddlabs.tt.model.behaviour.Controller
    {
        public com.oddlabs.tt.model.Unit unit;
        public com.oddlabs.tt.pathfinder.FinderTrackerAlgorithm building_tracker;
        // Java: 18
        public override void decide()
        {
            if (building_tracker != null && ((com.oddlabs.tt.model.Building)building_tracker.getOccupant()) != null && unit.isCloseEnough(0.0f, ((com.oddlabs.tt.model.Building)building_tracker.getOccupant())))
            {
                com.oddlabs.tt.model.Building building = ((com.oddlabs.tt.model.Building)building_tracker.getOccupant());
                if (building.getUnitContainer().canEnter(unit))
                {
                    building.getUnitContainer().enter(unit);
                }
                else
                {
                    unit.popController();
                }
            }
            else if (!shouldGiveUp(0))
            {
                building_tracker = new com.oddlabs.tt.pathfinder.FinderTrackerAlgorithm(unit.getUnitGrid(), new com.oddlabs.tt.model.BuildingFinder(unit.getOwner(), com.oddlabs.tt.model.Abilities.SUPPLY_CONTAINER));
                unit.setBehaviour(new com.oddlabs.tt.model.behaviour.WalkBehaviour(unit, building_tracker, false));
            }
            else
            {
                unit.popController();
            }
        }
        public TransferUnitController(com.oddlabs.tt.model.Unit unit) : base(1)
        {
            this.unit = unit;
        }
    }
}
