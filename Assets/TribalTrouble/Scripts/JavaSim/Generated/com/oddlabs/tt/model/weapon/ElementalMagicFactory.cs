// Translated from ElementalMagicFactory.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.weapon
{
    // Java: 11
    public class ElementalMagicFactory : java.lang.Object, com.oddlabs.tt.model.weapon.MagicFactory
    {
        public const int CRYSTAL_COST = 2;
        public float hit_radius;
        public float duration;
        public bool vikings;
        public float seconds_per_anim;
        public float init_ratio;
        public float release_ratio;
        // Java: 31
        public float getHitRadius()
        {
            return hit_radius;
        }
        // Java: 36
        public float getSecondsPerAnim()
        {
            return seconds_per_anim;
        }
        // Java: 41
        public float getSecondsPerRelease()
        {
            return (float)(release_ratio * seconds_per_anim);
        }
        // Java: 46
        public float getSecondsPerInit()
        {
            return (float)(init_ratio * seconds_per_anim);
        }
        // Java: 51
        public bool isVikings()
        {
            return vikings;
        }
        // Java: 55
        public com.oddlabs.tt.model.weapon.Magic execute(com.oddlabs.tt.model.Unit src)
        {
            int focus = src.getOwner().getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.FOCUS);
            return new com.oddlabs.tt.model.weapon.ElementalMagic(src, hit_radius, (float)(duration * ((float)(1.0f + (float)(0.3f * focus)))), vikings);
        }
        public ElementalMagicFactory(float hit_radius, float duration, bool vikings, float seconds_per_anim, float init_ratio, float release_ratio) : base()
        {
            this.hit_radius = hit_radius;
            this.duration = duration;
            this.vikings = vikings;
            this.seconds_per_anim = seconds_per_anim;
            this.init_ratio = init_ratio;
            this.release_ratio = release_ratio;
        }
    }
}
