// Translated from RallyMagicFactory.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.weapon
{
    // Java: 11
    public class RallyMagicFactory : java.lang.Object, com.oddlabs.tt.model.weapon.MagicFactory
    {
        public float hit_radius;
        public float duration;
        public float speed_mult;
        public float extra_range;
        public float attack_speed_mult;
        public float combat_mult;
        public int heal;
        public bool vikings;
        public float seconds_per_anim;
        public float init_ratio;
        public float release_ratio;
        public org.joml.Vector4f color;
        // Java: 60
        public float getHitRadius()
        {
            return hit_radius;
        }
        // Java: 65
        public float getSecondsPerAnim()
        {
            return seconds_per_anim;
        }
        // Java: 70
        public float getSecondsPerRelease()
        {
            return (float)(release_ratio * seconds_per_anim);
        }
        // Java: 75
        public float getSecondsPerInit()
        {
            return (float)(init_ratio * seconds_per_anim);
        }
        // Java: 80
        public com.oddlabs.tt.model.weapon.Magic execute(com.oddlabs.tt.model.Unit src)
        {
            return new com.oddlabs.tt.model.weapon.RallyMagic(src, hit_radius, duration, speed_mult, extra_range, attack_speed_mult, combat_mult, heal, vikings, color);
        }
        public RallyMagicFactory(float hit_radius, float duration, float speed_mult, float extra_range, float attack_speed_mult, float combat_mult, int heal, bool vikings, float seconds_per_anim, float init_ratio, float release_ratio) : base()
        {
            this.hit_radius = hit_radius;
            this.duration = duration;
            this.speed_mult = speed_mult;
            this.extra_range = extra_range;
            this.attack_speed_mult = attack_speed_mult;
            this.combat_mult = combat_mult;
            this.heal = heal;
            this.vikings = vikings;
            this.seconds_per_anim = seconds_per_anim;
            this.init_ratio = init_ratio;
            this.release_ratio = release_ratio;
            this.color = null;
        }
        public RallyMagicFactory(float hit_radius, float duration, float speed_mult, float extra_range, float attack_speed_mult, float combat_mult, int heal, bool vikings, float seconds_per_anim, float init_ratio, float release_ratio, org.joml.Vector4f color) : base()
        {
            this.hit_radius = hit_radius;
            this.duration = duration;
            this.speed_mult = speed_mult;
            this.extra_range = extra_range;
            this.attack_speed_mult = attack_speed_mult;
            this.combat_mult = combat_mult;
            this.heal = heal;
            this.vikings = vikings;
            this.seconds_per_anim = seconds_per_anim;
            this.init_ratio = init_ratio;
            this.release_ratio = release_ratio;
            this.color = color;
        }
    }
}
