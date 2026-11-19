// Translated from PoisonFogFactory.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.weapon
{
    // Java: 6
    public class PoisonFogFactory : java.lang.Object, com.oddlabs.tt.model.weapon.MagicFactory
    {
        public float offset_x;
        public float offset_y;
        public float offset_z;
        public float hit_radius;
        public float hit_chance;
        public float interval;
        public float time;
        public int damage;
        public float seconds_per_anim;
        public float init_ratio;
        public float release_ratio;
        // Java: 34
        public float getHitRadius()
        {
            return hit_radius;
        }
        // Java: 39
        public float getSecondsPerAnim()
        {
            return seconds_per_anim;
        }
        // Java: 44
        public float getSecondsPerInit()
        {
            return (float)(init_ratio * seconds_per_anim);
        }
        // Java: 49
        public float getSecondsPerRelease()
        {
            return (float)(release_ratio * seconds_per_anim);
        }
        // Java: 54
        public com.oddlabs.tt.model.weapon.Magic execute(com.oddlabs.tt.model.Unit src)
        {
            return new com.oddlabs.tt.model.weapon.PoisonFog(offset_x, offset_y, offset_z, hit_radius, hit_chance, interval, time, damage, src);
        }
        public PoisonFogFactory(float offset_x, float offset_y, float offset_z, float hit_radius, float hit_chance, float interval, float time, int damage, float seconds_per_anim, float init_ratio, float release_ratio) : base()
        {
            this.offset_x = offset_x;
            this.offset_y = offset_y;
            this.offset_z = offset_z;
            this.hit_radius = hit_radius;
            this.hit_chance = hit_chance;
            this.interval = interval;
            this.time = time;
            this.damage = damage;
            this.seconds_per_anim = seconds_per_anim;
            this.init_ratio = init_ratio;
            this.release_ratio = release_ratio;
        }
    }
}
