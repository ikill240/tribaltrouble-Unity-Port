// Translated from StunFactory.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.weapon
{
    // Java: 6
    public class StunFactory : java.lang.Object, com.oddlabs.tt.model.weapon.MagicFactory
    {
        public float offset_x;
        public float offset_y;
        public float offset_z;
        public float hit_radius;
        public float stun_time_closest;
        public float stun_time_farthest;
        public float seconds_per_anim;
        public float init_ratio;
        public float release_ratio;
        // Java: 30
        public float getHitRadius()
        {
            return hit_radius;
        }
        // Java: 35
        public float getSecondsPerAnim()
        {
            return seconds_per_anim;
        }
        // Java: 40
        public float getSecondsPerRelease()
        {
            return (float)(release_ratio * seconds_per_anim);
        }
        // Java: 45
        public float getSecondsPerInit()
        {
            return (float)(init_ratio * seconds_per_anim);
        }
        // Java: 50
        public com.oddlabs.tt.model.weapon.Magic execute(com.oddlabs.tt.model.Unit src)
        {
            return new com.oddlabs.tt.model.weapon.Stun(offset_x, offset_y, offset_z, hit_radius, stun_time_closest, stun_time_farthest, src);
        }
        public StunFactory(float offset_x, float offset_y, float offset_z, float hit_radius, float stun_time_closest, float stun_time_farthest, float seconds_per_anim, float init_ratio, float release_ratio) : base()
        {
            this.offset_x = offset_x;
            this.offset_y = offset_y;
            this.offset_z = offset_z;
            this.hit_radius = hit_radius;
            this.stun_time_closest = stun_time_closest;
            this.stun_time_farthest = stun_time_farthest;
            this.seconds_per_anim = seconds_per_anim;
            this.init_ratio = init_ratio;
            this.release_ratio = release_ratio;
        }
    }
}
