// Translated from SupplySpawnAnimation.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 7
    public class SupplySpawnAnimation : java.lang.Object, com.oddlabs.tt.animation.Animated
    {
        public com.oddlabs.tt.model.Supply supply;
        public float limit;
        public float time;
        // Java: 20
        public void animate(float t)
        {
            time = java.lang.Math.min((float)(time + t), limit);
            supply.animateSpawn(t, (float)(time / limit));
            if (time >= limit)
            {
                supply.getWorld().getAnimationManagerGameTime().removeAnimation(this);
                supply.spawnComplete();
            }
        }
        private void init_SupplySpawnAnimation()
        {
            this.time = 0;
        }
        public SupplySpawnAnimation(com.oddlabs.tt.model.Supply supply, float limit) : base()
        {
            init_SupplySpawnAnimation();
            this.supply = supply;
            this.limit = limit;
            supply.getWorld().getAnimationManagerGameTime().registerAnimation(this);
            supply.animateSpawn(0, 0);
        }
        // mixin of default method Animated.updateChecksum
        public virtual void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
        }
    }
}
