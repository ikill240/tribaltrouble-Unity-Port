// Translated from ConvertFactory.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.weapon
{
    // Java: 6
    public class ConvertFactory : java.lang.Object, com.oddlabs.tt.model.weapon.MagicFactory
    {
        public float offset_x;
        public float offset_y;
        public float hit_radius;
        public float seconds_per_anim;
        public float init_ratio;
        public float release_ratio;
        public bool affects_chieftains;
        // Java: 28
        public com.oddlabs.tt.model.weapon.Magic execute(com.oddlabs.tt.model.Unit src)
        {
            return new com.oddlabs.tt.model.weapon.Convert(offset_x, offset_y, hit_radius, src, affects_chieftains);
        }
        // Java: 33
        public float getHitRadius()
        {
            return hit_radius;
        }
        // Java: 38
        public float getSecondsPerAnim()
        {
            return seconds_per_anim;
        }
        // Java: 43
        public float getSecondsPerInit()
        {
            return (float)(init_ratio * seconds_per_anim);
        }
        // Java: 48
        public float getSecondsPerRelease()
        {
            return (float)(release_ratio * seconds_per_anim);
        }
        public ConvertFactory(float offset_x, float offset_y, float hit_radius, float seconds_per_anim, float init_ratio, float release_ratio, bool affects_chieftains) : base()
        {
            this.offset_x = offset_x;
            this.offset_y = offset_y;
            this.hit_radius = hit_radius;
            this.seconds_per_anim = seconds_per_anim;
            this.init_ratio = init_ratio;
            this.release_ratio = release_ratio;
            this.affects_chieftains = affects_chieftains;
        }
    }
}
