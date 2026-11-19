// Translated from GatherController.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.behaviour
{
    // Java: 14
    public class GatherController : com.oddlabs.tt.model.behaviour.Controller
    {
        // Java: 15
        public sealed class State : java.lang.Enum
        {
            public static State[] values() { return (State[])VALUES_.Clone(); }
            public static State valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly State HARVEST = new State("HARVEST", 0);
            public static readonly State DROPOFF = new State("DROPOFF", 1);
            public static readonly State[] VALUES_ = new State[] { HARVEST, DROPOFF };
            public State(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        public com.oddlabs.tt.model.Unit unit;
        public System.Type supply_type;
        public com.oddlabs.tt.model.Supply supply;
        public com.oddlabs.tt.pathfinder.FinderTrackerAlgorithm unknown_building_tracker;
        public com.oddlabs.tt.pathfinder.TargetTrackerAlgorithm assigned_building_tracker;
        public com.oddlabs.tt.model.Building assigned_building;
        // Java: 43
        public System.Type getSupplyType()
        {
            return supply_type;
        }
        // Java: 47
        public com.oddlabs.tt.model.Building getAssignedBuilding()
        {
            return assigned_building;
        }
        // Java: 53
        public com.oddlabs.tt.model.Supply getSupply()
        {
            return supply;
        }
        // Java: 57
        public override string getKey()
        {
            return java.lang.JStr.valueOf(base.getKey()) + java.lang.JStr.valueOf(supply_type);
        }
        // Java: 62
        public override bool onStuck()
        {
            if ((unit.getCurrentController() is com.oddlabs.tt.model.behaviour.HarvestController hc) && hc.getSupply() != null)
            {
                unit.setAvoidedSupply(hc.getSupply());
            }
            resetGiveUpCounters();
            supply = null;
            unit.swapController(new com.oddlabs.tt.model.behaviour.GatherController(unit, null, supply_type, assigned_building));
            return true;
        }
        // Java: 75
        public void gather()
        {
            resetGiveUpCounter(com.oddlabs.tt.model.behaviour.GatherController.State.DROPOFF.ordinal());
            if (supply != null && supply.isDead())
            {
                supply = null;
                resetGiveUpCounter(com.oddlabs.tt.model.behaviour.GatherController.State.HARVEST.ordinal());
            }
            if (supply != null && unit.isCloseEnough(unit.getRange(supply), supply))
            {
                unit.pushController(new com.oddlabs.tt.model.behaviour.HarvestController(unit, supply, supply_type));
            }
            else if (!shouldGiveUp(com.oddlabs.tt.model.behaviour.GatherController.State.HARVEST.ordinal()))
            {
                if (supply == null)
                {
                    unit.pushController(new com.oddlabs.tt.model.behaviour.HarvestController(unit, supply, supply_type));
                }
                else
                {
                    com.oddlabs.tt.pathfinder.TargetTrackerAlgorithm supply_tracker = new com.oddlabs.tt.pathfinder.TargetTrackerAlgorithm(unit.getUnitGrid(), 0.0f, supply);
                    unit.setBehaviour(new com.oddlabs.tt.model.behaviour.WalkBehaviour(unit, supply_tracker, false));
                }
            }
            else
            {
                unit.swapController(new com.oddlabs.tt.model.behaviour.TransferUnitController(unit));
            }
        }
        // Java: 96
        public com.oddlabs.tt.model.Building getBuilding()
        {
            if (assigned_building != null)
            {
                return assigned_building.getEntrance();
            }
            else if (unknown_building_tracker != null)
            {
                com.oddlabs.tt.model.Building building = ((com.oddlabs.tt.model.Building)unknown_building_tracker.getOccupant());
                if (building != null)
                {
                    building = building.getEntrance();
                }
                return building;
            }
            return null;
        }
        // Java: 109
        public void dropoff()
        {
            resetGiveUpCounter(com.oddlabs.tt.model.behaviour.GatherController.State.HARVEST.ordinal());
            com.oddlabs.tt.model.Building building = getBuilding();
            if (building != null && unit.getOwner().getWorld().isExtended() && (building.isDead() || building.getSupplyContainer(unit.getSupplyContainer().getSupplyType()) == null))
            {
                if (assigned_building != null && ((object)assigned_building.getEntrance() == (object)building))
                {
                    assigned_building = null;
                }
                unknown_building_tracker = null;
                building = null;
            }
            if (building != null && (unit.isCloseEnough(0.0f, building) || (building.getInteractDistance() > 0.0f && unit.isCloseEnough(building.getInteractDistance(), building))))
            {
                System.Type unit_supply_type = unit.getSupplyContainer().getSupplyType();
                int num_supplies = building.getSupplyContainer(unit_supply_type).increaseSupply(unit.getSupplyContainer().getNumSupplies());
                unit.getSupplyContainer().increaseSupply(-num_supplies, unit_supply_type);
                if (unit.getSupplyContainer().getNumSupplies() > 0)
                {
                    unit.swapController(new com.oddlabs.tt.model.behaviour.EnterController(unit, building));
                }
                else
                {
                    gather();
                }
            }
            else if (!shouldGiveUp(com.oddlabs.tt.model.behaviour.GatherController.State.DROPOFF.ordinal()))
            {
                if (assigned_building == null || assigned_building.isDead())
                {
                    unknown_building_tracker = new com.oddlabs.tt.pathfinder.FinderTrackerAlgorithm(unit.getUnitGrid(), new com.oddlabs.tt.model.BuildingFinder(unit.getOwner(), com.oddlabs.tt.model.Abilities.SUPPLY_CONTAINER));
                    assigned_building_tracker = null;
                }
                else
                {
                    assigned_building_tracker = new com.oddlabs.tt.pathfinder.TargetTrackerAlgorithm(unit.getUnitGrid(), assigned_building.getEntrance().getInteractDistance(), assigned_building.getEntrance());
                    unknown_building_tracker = null;
                }
                unit.setBehaviour(new com.oddlabs.tt.model.behaviour.WalkBehaviour(unit, getTracker(), false));
            }
            else if (com.oddlabs.tt.model.behaviour.WaitRetryController.applies(unit))
            {
                resetGiveUpCounter(com.oddlabs.tt.model.behaviour.GatherController.State.DROPOFF.ordinal());
                if (assigned_building != null && !assigned_building.isDead())
                {
                    assigned_building = null;
                }
                unit.pushController(new com.oddlabs.tt.model.behaviour.WaitRetryController(unit, com.oddlabs.tt.model.behaviour.WaitRetryController.delay(unit)));
            }
            else
            {
                unit.popController();
            }
        }
        // Java: 166
        public com.oddlabs.tt.pathfinder.TrackerAlgorithm getTracker()
        {
            return (assigned_building_tracker != null ? (com.oddlabs.tt.pathfinder.TrackerAlgorithm)(assigned_building_tracker) : (com.oddlabs.tt.pathfinder.TrackerAlgorithm)(unknown_building_tracker));
        }
        // Java: 170
        public override void decide()
        {
            if (unit.getSupplyContainer().getNumSupplies() > 0 && ((object)unit.getSupplyContainer().getSupplyType() == (object)supply_type))
            {
                dropoff();
            }
            else
            {
                gather();
            }
        }
        public GatherController(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.model.Supply supply, System.Type supply_type) : base(com.oddlabs.tt.model.behaviour.GatherController.State.values().Length)
        {
            this.unit = unit;
            this.supply = supply;
            this.supply_type = supply_type;
            this.assigned_building = null;
        }
        public GatherController(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.model.Supply supply, System.Type supply_type, com.oddlabs.tt.model.Building building) : base(com.oddlabs.tt.model.behaviour.GatherController.State.values().Length)
        {
            this.unit = unit;
            this.supply = supply;
            this.supply_type = supply_type;
            this.assigned_building = building;
        }
    }
}
