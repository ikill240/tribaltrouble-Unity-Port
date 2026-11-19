// Translated from SelectQuartersTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.tutorial
{
    // Java: 7
    public class SelectQuartersTrigger : com.oddlabs.tt.tutorial.TutorialTrigger
    {
        // Java: 12
        public override void run(com.oddlabs.tt.tutorial.Tutorial tutorial)
        {
            com.oddlabs.tt.model.Building building = tutorial.getViewer().getSelection().getCurrentSelection().getBuilding();
            if (building != null && building.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.REPRODUCE))
            {
                tutorial.next(new com.oddlabs.tt.tutorial.UnitsInQuartersTrigger());
            }
        }
        public SelectQuartersTrigger() : base(0.1f, 0.0f, "select_quarters")
        {
        }
    }
}
