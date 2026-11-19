// Translated from UnitInTowerTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.tutorial
{
    // Java: 6
    public class UnitInTowerTrigger : com.oddlabs.tt.tutorial.TutorialTrigger
    {
        public com.oddlabs.tt.model.Building tower;
        // Java: 14
        public override void run(com.oddlabs.tt.tutorial.Tutorial tutorial)
        {
            if (tower.getUnitContainer().getNumSupplies() > 0)
            {
                tutorial.next(new com.oddlabs.tt.tutorial.AttackTowerTrigger(tower));
            }
        }
        public UnitInTowerTrigger(com.oddlabs.tt.model.Building tower) : base(0.1f, 0.0f, "unit_in_tower")
        {
            this.tower = tower;
        }
    }
}
