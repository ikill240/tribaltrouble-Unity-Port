// Translated from RepairController.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.behaviour
{
    // Java: 9
    public class RepairController : com.oddlabs.tt.model.behaviour.Controller
    {
        // Java: 10
        public sealed class State : java.lang.Enum
        {
            public static State[] values() { return (State[])VALUES_.Clone(); }
            public static State valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly State HARVEST = new State("HARVEST", 0);
            public static readonly State REPAIR = new State("REPAIR", 1);
            public static readonly State[] VALUES_ = new State[] { HARVEST, REPAIR };
            public State(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        public com.oddlabs.tt.model.Building building;
        public com.oddlabs.tt.model.Unit unit;
        // Java: 24
        public com.oddlabs.tt.model.Building getBuilding()
        {
            return building;
        }
        // Java: 28
        public override string getKey()
        {
            return java.lang.JStr.valueOf(base.getKey()) + "@" + (building.getGridX()) + "," + (building.getGridY());
        }
        // Java: 33
        public override void decide()
        {
            if (building.isDead())
            {
                unit.popController();
            }
            else if (((object)unit.getSupplyContainer().getSupplyType() == (object)typeof(com.oddlabs.tt.landscape.TreeSupply)) && unit.getSupplyContainer().getNumSupplies() > 0)
            {
                resetGiveUpCounter(com.oddlabs.tt.model.behaviour.RepairController.State.HARVEST.ordinal());
                if (unit.isCloseEnough(0.0f, building))
                {
                    if (building.isDamaged())
                    {
                        unit.setBehaviour(new com.oddlabs.tt.model.behaviour.RepairBehaviour(unit, building));
                    }
                    else if (building.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.SUPPLY_CONTAINER) && ((object)unit.getOwner() == (object)building.getOwner()))
                    {
                        unit.swapController(new com.oddlabs.tt.model.behaviour.EnterController(unit, building));
                    }
                    else
                    {
                        unit.popController();
                    }
                }
                else
                {
                    if (shouldGiveUp(com.oddlabs.tt.model.behaviour.RepairController.State.REPAIR.ordinal()))
                    {
                        unit.popController();
                    }
                    else
                    {
                        unit.setBehaviour(new com.oddlabs.tt.model.behaviour.WalkBehaviour(unit, building, 0, false));
                    }
                }
            }
            else
            {
                resetGiveUpCounter(com.oddlabs.tt.model.behaviour.RepairController.State.REPAIR.ordinal());
                if (!shouldGiveUp(com.oddlabs.tt.model.behaviour.RepairController.State.HARVEST.ordinal()))
                {
                    unit.pushController(new com.oddlabs.tt.model.behaviour.HarvestController(unit, null, typeof(com.oddlabs.tt.landscape.TreeSupply)));
                }
                else
                {
                    unit.popController();
                }
            }
        }
        public RepairController(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.model.Building building) : base(com.oddlabs.tt.model.behaviour.RepairController.State.values().Length)
        {
            this.unit = unit;
            this.building = building;
        }
    }
}
