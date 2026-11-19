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
            return java.lang.JStr.valueOf(base.getKey()) + (building.getGridX()) + "," + (building.getGridY());
        }
        // Java: 37
        public float reach()
        {
            return (building.getOwner().getWorld().isFreeForm() || building.getOwner().getWorld().isExtended() ? (float)(building.getSize() + 1.0f) : 0.0f);
        }
        // Java: 41
        public bool inReach()
        {
            return (building.getOwner().getWorld().isFreeForm() || building.getOwner().getWorld().isExtended()) && unit.isCloseEnough(reach(), building);
        }
        public const float NEXT_SITE_RANGE = 40.0f;
        // Java: 52
        public bool nextSite()
        {
            if (!unit.getOwner().getWorld().isExtended() || unit.getQueuedOrderCount() > 0 || ((object)unit.getPrimaryController() != (object)this))
            {
                return false;
            }
            com.oddlabs.tt.model.Building best = null;
            float best_d = (float)(com.oddlabs.tt.model.behaviour.RepairController.NEXT_SITE_RANGE * com.oddlabs.tt.model.behaviour.RepairController.NEXT_SITE_RANGE);
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(unit.getOwner().getUnits().getSet()))
            {
                if (!((s is com.oddlabs.tt.model.LandBuilding b)) || ((object)b == (object)building) || b.isDead())
                {
                    continue;
                }
                if (b.isComplete() && !b.isDamaged())
                {
                    continue;
                }
                float dx = (float)(b.getPositionX() - unit.getPositionX());
                float dy = (float)(b.getPositionY() - unit.getPositionY());
                float d = (float)((float)(dx * dx) + (float)(dy * dy));
                if (d < best_d || d == best_d && best != null && (b.getGridY() < best.getGridY() || b.getGridY() == best.getGridY() && b.getGridX() < best.getGridX()))
                {
                    best_d = d;
                    best = b;
                }
            }
            if (best == null)
            {
                return false;
            }
            unit.swapController(new com.oddlabs.tt.model.behaviour.RepairController(unit, best));
            return true;
        }
        // Java: 76
        public override void decide()
        {
            if (building.isDead())
            {
                unit.popController();
            }
            else if (((object)unit.getSupplyContainer().getSupplyType() == (object)typeof(com.oddlabs.tt.landscape.TreeSupply)) && unit.getSupplyContainer().getNumSupplies() > 0)
            {
                resetGiveUpCounter(com.oddlabs.tt.model.behaviour.RepairController.State.HARVEST.ordinal());
                if (unit.isCloseEnough(0.0f, building) || inReach())
                {
                    if (building.isDamaged())
                    {
                        unit.setBehaviour(new com.oddlabs.tt.model.behaviour.RepairBehaviour(unit, building));
                    }
                    else if (building.isComplete() && nextSite())
                    {
                        return;
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
                        if (com.oddlabs.tt.model.behaviour.WaitRetryController.applies(unit))
                        {
                            resetGiveUpCounter(com.oddlabs.tt.model.behaviour.RepairController.State.REPAIR.ordinal());
                            unit.pushController(new com.oddlabs.tt.model.behaviour.WaitRetryController(unit, com.oddlabs.tt.model.behaviour.WaitRetryController.delay(unit)));
                        }
                        else
                        {
                            unit.popController();
                        }
                    }
                    else
                    {
                        unit.setBehaviour(new com.oddlabs.tt.model.behaviour.WalkBehaviour(unit, building, reach(), false));
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
