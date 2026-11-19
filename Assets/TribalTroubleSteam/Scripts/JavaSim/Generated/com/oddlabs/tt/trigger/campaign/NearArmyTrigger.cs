// Translated from NearArmyTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.trigger.campaign
{
    // Java: 9
    public class NearArmyTrigger : com.oddlabs.tt.trigger.IntervalTrigger
    {
        public com.oddlabs.tt.model.Unit[] src;
        public float r;
        public com.oddlabs.tt.player.Player player;
        public java.lang.Runnable runnable;
        // Java: 23
        public override void check()
        {
            foreach (com.oddlabs.tt.model.Unit unit in src)
            {
                if (unit.isDead())
                {
                    continue;
                }
                com.oddlabs.tt.pathfinder.FindOccupantFilter filter = new com.oddlabs.tt.pathfinder.FindOccupantFilter(unit.getPositionX(), unit.getPositionY(), r, unit, typeof(com.oddlabs.tt.model.Unit));
                player.getWorld().getUnitGrid().scan(filter, unit.getGridX(), unit.getGridY());
                foreach (com.oddlabs.tt.model.Unit filtered in java.lang.JIter.of(filter.getResult()))
                {
                    if (!filtered.isDead() && ((object)filtered.getOwner() == (object)player))
                    {
                        triggered();
                        return;
                    }
                }
            }
        }
        // Java: 41
        public override void done()
        {
            runnable.run();
        }
        public NearArmyTrigger(com.oddlabs.tt.model.Unit[] src, float r, com.oddlabs.tt.player.Player player, java.lang.Runnable runnable) : base(player.getWorld(), 0.25f, 0.0f)
        {
            this.src = src;
            this.r = r;
            this.player = player;
            this.runnable = runnable;
        }
    }
}
