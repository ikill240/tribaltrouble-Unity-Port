// Translated from ReproduceUnitContainer.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 6
    public class ReproduceUnitContainer : com.oddlabs.tt.model.UnitContainer
    {
        public com.oddlabs.tt.model.Building building;
        public float unit_reproduction;
        // Java: 17
        public float getBuildProgress()
        {
            com.oddlabs.tt.model.ChieftainContainer chieftain_container = building.getChieftainContainer();
            if (chieftain_container.isTraining())
            {
                return 0;
            }
            else
            {
                return unit_reproduction;
            }
        }
        // Java: 26
        public void resetProgress()
        {
            unit_reproduction = 0.0f;
        }
        // Java: 30
        public override void enter(com.oddlabs.tt.model.Unit unit)
        {
            unit.removeNow();
            increaseSupply(1);
        }
        // Java: 37
        public override bool canEnter(com.oddlabs.tt.model.Unit unit)
        {
            return !unit.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.THROW) && getTotalSupplies() != getMaxSupplyCount();
        }
        // Java: 42
        public int getTotalSupplies()
        {
            return getNumSupplies() + getNumPreparing();
        }
        // Java: 47
        public override com.oddlabs.tt.model.Unit exit()
        {
            increaseSupply((-1));
            return null;
        }
        // Java: 54
        public override int increaseSupply(int amount)
        {
            int result = building.getOwner().getUnitCountContainer().increaseSupply(amount);
            return base.increaseSupply(amount);
        }
        // Java: 61
        public override void animate(float t)
        {
            com.oddlabs.tt.model.ChieftainContainer chieftain_container = building.getChieftainContainer();
            if ((building.getOwner().getUnitCountContainer().getNumSupplies() < getMaxSupplyCount() && getTotalSupplies() != getMaxSupplyCount()) || chieftain_container.isTraining())
            {
                float units = java.lang.Math.max(building.getUnitContainer().getNumSupplies(), 0.5f);
                unit_reproduction = (float)(unit_reproduction + (((float)(1.0f / 11.0f)) * java.lang.StrictMath.pow(units, (float)(1.0f / 3.0f))) * t);
                while (unit_reproduction >= 1.0f)
                {
                    unit_reproduction -= 1.0f;
                    if (chieftain_container.isTraining())
                    {
                        chieftain_container.progress();
                    }
                    else
                    {
                        increaseSupply(1);
                    }
                }
            }
            else
            {
                unit_reproduction = 0;
            }
        }
        private void init_ReproduceUnitContainer()
        {
            this.unit_reproduction = 0.0f;
        }
        public ReproduceUnitContainer(com.oddlabs.tt.model.Building building) : base(building.getOwner().getWorld().getMaxUnitCount())
        {
            init_ReproduceUnitContainer();
            this.building = building;
        }
    }
}
