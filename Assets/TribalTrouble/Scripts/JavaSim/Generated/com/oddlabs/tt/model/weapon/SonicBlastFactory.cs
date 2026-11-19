// Translated from SonicBlastFactory.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.weapon
{
    // Java: 6
    public class SonicBlastFactory : java.lang.Object, com.oddlabs.tt.model.weapon.MagicFactory
    {
        public float offset_x;
        public float offset_y;
        public float offset_z;
        public float hit_radius;
        public float hit_chance_closest;
        public float hit_chance_farthest;
        public int damage_closest;
        public int damage_farthest;
        public float seconds;
        public float seconds_per_anim;
        public float init_ratio;
        public float release_ratio;
        // Java: 37
        public float getHitRadius()
        {
            return hit_radius;
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
            return new com.oddlabs.tt.model.weapon.SonicBlast(offset_x, offset_y, offset_z, hit_radius, hit_chance_closest, hit_chance_farthest, damage_closest, damage_farthest, seconds, src);
        }
        public SonicBlastFactory(float offset_x, float offset_y, float offset_z, float hit_radius, float hit_chance_closest, float hit_chance_farthest, int damage_closest, int damage_farthest, float seconds, float seconds_per_anim, float init_ratio, float release_ratio) : base()
        {
            this.offset_x = offset_x;
            this.offset_y = offset_y;
            this.offset_z = offset_z;
            this.hit_radius = hit_radius;
            this.hit_chance_closest = hit_chance_closest;
            this.hit_chance_farthest = hit_chance_farthest;
            this.damage_closest = damage_closest;
            this.damage_farthest = damage_farthest;
            this.seconds = seconds;
            this.seconds_per_anim = seconds_per_anim;
            this.init_ratio = init_ratio;
            this.release_ratio = release_ratio;
        }
    }
}
