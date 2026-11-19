// Translated from PassiveAI.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player
{
    // Java: 10
    public class PassiveAI : com.oddlabs.tt.player.AI
    {
        public bool walk_around;
        // Java: 18
        public override void animate(float time)
        {
            if (walk_around)
            {
                if (!shouldDoAction(time))
                {
                    return;
                }
                com.oddlabs.tt.model.Selectable[][] lists = getOwner().classifyUnits();
                foreach (com.oddlabs.tt.model.Selectable[] list in lists)
                {
                    com.oddlabs.tt.model.Selectable s = list[0];
                    if ((s.getPrimaryController() is com.oddlabs.tt.model.behaviour.IdleController))
                    {
                        foreach (com.oddlabs.tt.model.Selectable thrower in list)
                        {
                            float r = getOwner().getWorld().getRandom().nextFloat();
                            if (r < 0.2d)
                            {
                                com.oddlabs.tt.util.Target walkable_target = getTarget(getOwner().getWorld().getRandom());
                                getOwner().setTarget(com.oddlabs.tt.model.Selectable.newArray(thrower), walkable_target, com.oddlabs.tt.model.Action.ATTACK, true);
                            }
                        }
                    }
                }
                com.oddlabs.tt.model.Unit primary_chieftain = getOwner().getChieftain();
                if (primary_chieftain != null && !primary_chieftain.isDead())
                {
                    getOwner().getRace().getChieftainAI().decide(primary_chieftain);
                }
            }
        }
        public PassiveAI(com.oddlabs.tt.player.Player owner, com.oddlabs.tt.player.UnitInfo unit_info, bool walk_around) : base(owner, unit_info)
        {
            this.walk_around = walk_around;
        }
    }
}
