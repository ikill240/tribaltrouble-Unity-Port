// Translated from LightningCloudFactory.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.weapon
{
    // Java: 6
    public class LightningCloudFactory : java.lang.Object, com.oddlabs.tt.model.weapon.MagicFactory
    {
        public float offset_x;
        public float offset_y;
        public float offset_z;
        public float seconds_to_live;
        public float seconds_per_hit;
        public float meters_per_second;
        public float hit_chance;
        public int damage;
        public float height;
        public float seconds_per_anim;
        public float init_ratio;
        public float release_ratio;
        // Java: 37
        public float getHitRadius()
        {
            return 0;
        }
        // Java: 42
        public float getSecondsPerAnim()
        {
            return seconds_per_anim;
        }
        // Java: 47
        public float getSecondsPerInit()
        {
            return (float)(init_ratio * seconds_per_anim);
        }
        // Java: 52
        public float getSecondsPerRelease()
        {
            return (float)(release_ratio * seconds_per_anim);
        }
        // Java: 57
        public com.oddlabs.tt.model.weapon.Magic execute(com.oddlabs.tt.model.Unit src)
        {
            float seconds_to_init = (float)(getSecondsPerRelease() - getSecondsPerInit());
            return new com.oddlabs.tt.model.weapon.LightningCloud(src.getOwner().getWorld(), offset_x, offset_y, offset_z, seconds_to_live, seconds_per_hit, seconds_to_init, meters_per_second, hit_chance, damage, height, src);
        }
        public LightningCloudFactory(float offset_x, float offset_y, float offset_z, float seconds_to_live, float seconds_per_hit, float meters_per_second, float hit_chance, int damage, float height, float seconds_per_anim, float init_ratio, float release_ratio) : base()
        {
            this.offset_x = offset_x;
            this.offset_y = offset_y;
            this.offset_z = offset_z;
            this.seconds_to_live = seconds_to_live;
            this.seconds_per_hit = seconds_per_hit;
            this.meters_per_second = meters_per_second;
            this.hit_chance = hit_chance;
            this.damage = damage;
            this.height = height;
            this.seconds_per_anim = seconds_per_anim;
            this.init_ratio = init_ratio;
            this.release_ratio = release_ratio;
        }
    }
}
