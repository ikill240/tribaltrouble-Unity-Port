// Translated from RepairTowerTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.tutorial
{
    // Java: 6
    public class RepairTowerTrigger : com.oddlabs.tt.tutorial.TutorialTrigger
    {
        public com.oddlabs.tt.model.Building tower;
        // Java: 15
        public override void run(com.oddlabs.tt.tutorial.Tutorial tutorial)
        {
            if (!tower.isDamaged())
            {
                tutorial.next(new com.oddlabs.tt.tutorial.EmptyTowerTrigger(tower));
            }
        }
        public RepairTowerTrigger(com.oddlabs.tt.model.Building tower) : base(0.1f, 0.0f, "repair")
        {
            this.tower = tower;
            tower.getOwner().enableRepairing(true);
        }
    }
}
