// Translated from SupplyGatheredTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.trigger.campaign
{
    // Java: 12
    public class SupplyGatheredTrigger : com.oddlabs.tt.trigger.IntervalTrigger
    {
        public java.lang.Runnable runnable;
        public System.Type type;
        public int goal;
        public com.oddlabs.tt.player.Player local_player;
        // Java: 27
        public override void check()
        {
            int count = 0;
            com.oddlabs.tt.model.Selectable[][] selectables = local_player.classifyUnits();
            foreach (com.oddlabs.tt.model.Selectable[] selectable in selectables)
            {
                com.oddlabs.tt.model.Selectable s = selectable[0];
                if ((s.getPrimaryController() is com.oddlabs.tt.model.behaviour.NullController))
                {
                    foreach (com.oddlabs.tt.model.Selectable building in selectable)
                    {
                        if (building.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD_ARMIES))
                        {
                            count += (((com.oddlabs.tt.model.Building)building)).getSupplyContainer(type).getNumSupplies();
                        }
                    }
                }
            }
            if (count >= goal)
            {
                triggered();
            }
        }
        // Java: 47
        public override void done()
        {
            runnable.run();
        }
        public SupplyGatheredTrigger(com.oddlabs.tt.player.Player local_player, java.lang.Runnable runnable, System.Type type, int goal) : base(local_player.getWorld(), 0.5f, 0.0f)
        {
            this.local_player = local_player;
            this.runnable = runnable;
            this.type = type;
            this.goal = goal;
        }
    }
}
