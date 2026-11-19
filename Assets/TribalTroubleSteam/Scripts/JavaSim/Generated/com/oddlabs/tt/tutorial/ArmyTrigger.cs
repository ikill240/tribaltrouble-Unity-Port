// Translated from ArmyTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.tutorial
{
    // Java: 9
    public class ArmyTrigger : com.oddlabs.tt.tutorial.TutorialTrigger
    {
        public const int ARMY_SIZE = 10;
        // Java: 17
        public override void run(com.oddlabs.tt.tutorial.Tutorial tutorial)
        {
            long count = tutorial.getViewer().getLocalPlayer().getUnits().getSet().stream().filter(new java.util.function.Predicate_L((object s_l) =>
            {
                com.oddlabs.tt.model.Selectable s = (com.oddlabs.tt.model.Selectable)s_l;
                return (s is com.oddlabs.tt.model.Unit) && s.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.THROW);
            })).limit(com.oddlabs.tt.tutorial.ArmyTrigger.ARMY_SIZE).count();
            if (count >= com.oddlabs.tt.tutorial.ArmyTrigger.ARMY_SIZE)
            {
                tutorial.done(com.oddlabs.tt.form.TutorialForm.TUTORIAL_ARMORY);
            }
        }
        public ArmyTrigger(com.oddlabs.tt.player.Player local_player) : base(1.0f, 0.0f, "army", new object[] { com.oddlabs.tt.tutorial.ArmyTrigger.ARMY_SIZE })
        {
            local_player.enableMoving(true);
        }
    }
}
