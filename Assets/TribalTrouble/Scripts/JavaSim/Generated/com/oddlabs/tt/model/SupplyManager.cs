// Translated from SupplyManager.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 10
    public class SupplyManager : java.lang.Object, com.oddlabs.tt.animation.Animated
    {
        public const float SLEEP_TIME = 5.0f;
        public const float SPAWN_TIME = 3.0f;
        public const float MAX_EMPTY_SUPPLIES = 0.75f;
        public java.util.List empty_supplies;
        public com.oddlabs.tt.landscape.World world;
        public int total_num_supplies;
        public float time;
        // Java: 28
        public com.oddlabs.tt.landscape.World getWorld()
        {
            return world;
        }
        // Java: 32
        public void resetCounter()
        {
            time = getSleepTime();
        }
        // Java: 36
        public virtual float getSleepTime()
        {
            return com.oddlabs.tt.model.SupplyManager.SLEEP_TIME;
        }
        // Java: 40
        public void debugSpawnSupply()
        {
            if (!empty_supplies.isEmpty())
            {
                insertSupply();
            }
        }
        // Java: 45
        public void newSupply()
        {
            total_num_supplies++;
        }
        // Java: 49
        public void emptySupply(com.oddlabs.tt.model.Supply supply)
        {
            empty_supplies.add(supply);
        }
        // Java: 53
        public void animate(float t)
        {
            if (time < 0)
            {
                resetCounter();
                if (shouldSpawn())
                {
                    insertSupply();
                }
            }
            time -= t;
        }
        // Java: 63
        public virtual bool shouldSpawn()
        {
            return java.lang.JC.f2i(((float)(total_num_supplies * com.oddlabs.tt.model.SupplyManager.MAX_EMPTY_SUPPLIES))) < empty_supplies.size();
        }
        // Java: 67
        public virtual void insertSupply()
        {
            int index = world.getRandom().nextInt(empty_supplies.size());
            com.oddlabs.tt.model.Supply supply = ((com.oddlabs.tt.model.Supply)empty_supplies.get(index));
            bool occupied = world.getUnitGrid().isGridOccupied(supply.getGridX(), supply.getGridY());
            if (!occupied)
            {
                empty_supplies.remove(supply);
                com.oddlabs.tt.model.Supply new_supply = supply.respawn();
                new com.oddlabs.tt.model.SupplySpawnAnimation(new_supply, com.oddlabs.tt.model.SupplyManager.SPAWN_TIME);
            }
        }
        private void init_SupplyManager()
        {
            this.empty_supplies = new java.util.ArrayList();
            this.total_num_supplies = 0;
        }
        public SupplyManager(com.oddlabs.tt.landscape.World world) : base()
        {
            init_SupplyManager();
            this.world = world;
            world.getAnimationManagerGameTime().registerAnimation(this);
            resetCounter();
        }
        // mixin of default method Animated.updateChecksum
        public virtual void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
        }
    }
}
