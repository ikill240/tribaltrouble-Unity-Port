// Translated from IronSpearWeapon.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.weapon
{
    // Java: 9
    public class IronSpearWeapon : com.oddlabs.tt.model.weapon.DirectedThrowingWeapon
    {
        public const float METERS_PER_SECOND = 25.0f;
        // Java: 17
        public override float getMetersPerSecond()
        {
            return com.oddlabs.tt.model.weapon.IronSpearWeapon.METERS_PER_SECOND;
        }
        // Java: 22
        public override int getDamage()
        {
            return 2;
        }
        public IronSpearWeapon(bool hit, com.oddlabs.tt.model.Unit src, com.oddlabs.tt.model.Selectable target, com.oddlabs.tt.render.SpriteKey sprite_renderer, com.oddlabs.tt.audio.Audio throw_sound, com.oddlabs.tt.audio.Audio[] hit_sounds) : base(hit, src, target, sprite_renderer, throw_sound, hit_sounds)
        {
        }
    }
}
