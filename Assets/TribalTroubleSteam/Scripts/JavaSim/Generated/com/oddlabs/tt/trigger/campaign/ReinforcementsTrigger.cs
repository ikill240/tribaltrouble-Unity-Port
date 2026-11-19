// Translated from ReinforcementsTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.trigger.campaign
{
    // Java: 8
    public class ReinforcementsTrigger : com.oddlabs.tt.trigger.IntervalTrigger
    {
        public com.oddlabs.tt.player.Player player;
        public com.oddlabs.tt.model.DeployType type;
        public int units_deployed;
        // Java: 20
        public override void check()
        {
            if (player.getArmory() == null)
            {
                triggered();
            }
            else if (units_deployed < player.getUnitsLost())
            {
                int reinforcements = player.getUnitsLost() - units_deployed;
                if (reinforcements > player.getArmory().getUnitContainer().getNumSupplies())
                {
                    reinforcements = player.getArmory().getUnitContainer().getNumSupplies();
                }
                if (reinforcements > 0)
                {
                    player.deployUnits(player.getArmory(), type, reinforcements);
                    units_deployed += reinforcements;
                }
            }
        }
        // Java: 36
        public override void done()
        {
        }
        private void init_ReinforcementsTrigger()
        {
            this.units_deployed = 0;
        }
        public ReinforcementsTrigger(com.oddlabs.tt.player.Player player, com.oddlabs.tt.model.DeployType type) : base(player.getWorld(), 0.5f, 0.0f)
        {
            init_ReinforcementsTrigger();
            this.player = player;
            this.type = type;
        }
    }
}
