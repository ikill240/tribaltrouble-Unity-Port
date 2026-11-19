// Translated from SupplyTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.tutorial
{
    // Java: 8
    public class SupplyTrigger : com.oddlabs.tt.tutorial.TutorialTrigger
    {
        public const int TREE = 20;
        public const int ROCK = 10;
        // Java: 17
        public override void run(com.oddlabs.tt.tutorial.Tutorial tutorial)
        {
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(tutorial.getViewer().getSelection().getCurrentSelection().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Building armory) && s.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD_ARMIES))
                {
                    if (armory.getSupplyContainer(typeof(com.oddlabs.tt.model.RockSupply)).getNumSupplies() >= com.oddlabs.tt.tutorial.SupplyTrigger.ROCK && armory.getSupplyContainer(typeof(com.oddlabs.tt.landscape.TreeSupply)).getNumSupplies() >= com.oddlabs.tt.tutorial.SupplyTrigger.TREE)
                    {
                        tutorial.next(new com.oddlabs.tt.tutorial.BuildMenuTrigger(tutorial.getViewer().getLocalPlayer()));
                    }
                }
            }
        }
        public SupplyTrigger(com.oddlabs.tt.player.Player player) : base(0.5f, 0.0f, "supply", new object[] { com.oddlabs.tt.tutorial.SupplyTrigger.TREE, com.oddlabs.tt.tutorial.SupplyTrigger.ROCK })
        {
            player.enableHarvesting(true);
        }
    }
}
