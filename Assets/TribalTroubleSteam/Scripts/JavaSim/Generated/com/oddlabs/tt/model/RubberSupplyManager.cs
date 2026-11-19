// Translated from RubberSupplyManager.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 6
    public class RubberSupplyManager : com.oddlabs.tt.model.SupplyManager
    {
        public const float SLEEP_TICKS = 60.0f;
        public const int MAX_NUM_GROUPS = 3;
        public int current_groups;
        // Java: 16
        public override float getSleepTime()
        {
            return com.oddlabs.tt.model.RubberSupplyManager.SLEEP_TICKS;
        }
        // Java: 21
        public override bool shouldSpawn()
        {
            return current_groups < com.oddlabs.tt.model.RubberSupplyManager.MAX_NUM_GROUPS;
        }
        // Java: 26
        public override void insertSupply()
        {
            new com.oddlabs.tt.model.RubberGroup(getWorld());
        }
        // Java: 31
        public void newGroup()
        {
            current_groups++;
        }
        // Java: 35
        public void emptyGroup()
        {
            current_groups--;
        }
        private void init_RubberSupplyManager()
        {
            this.current_groups = 0;
        }
        public RubberSupplyManager(com.oddlabs.tt.landscape.World world) : base(world)
        {
            init_RubberSupplyManager();
        }
    }
}
