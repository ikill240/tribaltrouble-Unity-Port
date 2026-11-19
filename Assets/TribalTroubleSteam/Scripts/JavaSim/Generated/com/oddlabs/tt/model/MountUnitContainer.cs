// Translated from MountUnitContainer.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 6
    public class MountUnitContainer : com.oddlabs.tt.model.UnitContainer
    {
        public const float ATTACK_RANGE_INCREASE = 8.0f;
        public com.oddlabs.tt.model.Building building;
        public com.oddlabs.tt.model.Unit unit;
        // Java: 17
        public override void enter(com.oddlabs.tt.model.Unit unit)
        {
            this.unit = unit;
            unit.mount(building);
            unit.increaseRange(com.oddlabs.tt.model.MountUnitContainer.ATTACK_RANGE_INCREASE);
            increaseSupply(1);
            building.getAbilities().addAbilities(com.oddlabs.tt.model.Abilities.TARGET);
        }
        // Java: 26
        public override com.oddlabs.tt.model.Unit exit()
        {
            unit.unmount();
            unit.increaseRange((float)(-com.oddlabs.tt.model.MountUnitContainer.ATTACK_RANGE_INCREASE));
            com.oddlabs.tt.model.Unit result = unit;
            unit = null;
            increaseSupply((-1));
            building.getAbilities().removeAbilities(com.oddlabs.tt.model.Abilities.TARGET);
            return result;
        }
        // Java: 38
        public override bool canEnter(com.oddlabs.tt.model.Unit unit)
        {
            return !isSupplyFull() && unit.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.THROW);
        }
        // Java: 43
        public com.oddlabs.tt.model.Unit getUnit()
        {
            return unit;
        }
        public MountUnitContainer(com.oddlabs.tt.model.Building building) : base(1)
        {
            this.building = building;
        }
    }
}
