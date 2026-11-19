// Translated from WeaponTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.tutorial
{
    // Java: 8
    public class WeaponTrigger : com.oddlabs.tt.tutorial.TutorialTrigger
    {
        public const int WEAPONS = 10;
        // Java: 16
        public override void run(com.oddlabs.tt.tutorial.Tutorial tutorial)
        {
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(tutorial.getViewer().getSelection().getCurrentSelection().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Building armory) && s.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD_ARMIES))
                {
                    if (armory.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)).getNumSupplies() >= com.oddlabs.tt.tutorial.WeaponTrigger.WEAPONS)
                    {
                        tutorial.next(new com.oddlabs.tt.tutorial.ArmyMenuTrigger(tutorial.getViewer().getLocalPlayer()));
                    }
                }
            }
        }
        public WeaponTrigger(com.oddlabs.tt.player.Player local_player) : base(0.5f, 0.0f, "weapon", new object[] { com.oddlabs.tt.tutorial.WeaponTrigger.WEAPONS })
        {
            local_player.enableHarvesting(true);
        }
    }
}
