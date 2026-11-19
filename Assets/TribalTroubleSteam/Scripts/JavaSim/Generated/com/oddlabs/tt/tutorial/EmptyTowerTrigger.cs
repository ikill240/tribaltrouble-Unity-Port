// Translated from EmptyTowerTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.tutorial
{
    // Java: 7
    public class EmptyTowerTrigger : com.oddlabs.tt.tutorial.TutorialTrigger
    {
        public com.oddlabs.tt.model.Building tower;
        // Java: 16
        public override void run(com.oddlabs.tt.tutorial.Tutorial tutorial)
        {
            if (tower.getUnitContainer().getNumSupplies() == 0)
            {
                tutorial.done(com.oddlabs.tt.form.TutorialForm.TUTORIAL_TOWER);
            }
        }
        public EmptyTowerTrigger(com.oddlabs.tt.model.Building tower) : base(0.1f, 0.0f, "empty_tower")
        {
            this.tower = tower;
            tower.getOwner().enableTowerExits(true);
        }
    }
}
