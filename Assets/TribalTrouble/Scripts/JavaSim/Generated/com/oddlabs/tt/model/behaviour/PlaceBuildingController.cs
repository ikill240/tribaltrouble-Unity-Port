// Translated from PlaceBuildingController.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.behaviour
{
    // Java: 6
    public class PlaceBuildingController : com.oddlabs.tt.model.behaviour.Controller
    {
        public com.oddlabs.tt.model.Building building;
        public com.oddlabs.tt.model.Unit unit;
        // Java: 16
        public com.oddlabs.tt.model.Building getBuilding()
        {
            return building;
        }
        // Java: 20
        public bool canPlaceBuilding()
        {
            return unit.isCloseEnough(building.getSize(), building);
        }
        // Java: 24
        public override void decide()
        {
            if (building.isDead())
            {
                unit.popController();
            }
            else if (building.isPlaced())
            {
                if (building.isDamaged())
                {
                    unit.swapController(new com.oddlabs.tt.model.behaviour.RepairController(unit, building));
                }
                else
                {
                    unit.popController();
                }
            }
            else if (canPlaceBuilding())
            {
                if (building.isPlacingLegal())
                {
                    building.place();
                    unit.swapController(new com.oddlabs.tt.model.behaviour.RepairController(unit, building));
                }
                else
                {
                    unit.popController();
                }
            }
            else if (!shouldGiveUp(0))
            {
                unit.setBehaviour(new com.oddlabs.tt.model.behaviour.WalkBehaviour(unit, building, building.getSize(), false));
            }
            else if (com.oddlabs.tt.model.behaviour.WaitRetryController.applies(unit))
            {
                resetGiveUpCounters();
                unit.pushController(new com.oddlabs.tt.model.behaviour.WaitRetryController(unit, com.oddlabs.tt.model.behaviour.WaitRetryController.delay(unit)));
            }
            else
            {
                unit.popController();
            }
        }
        public PlaceBuildingController(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.model.Building building) : base(1)
        {
            this.unit = unit;
            this.building = building;
        }
    }
}
