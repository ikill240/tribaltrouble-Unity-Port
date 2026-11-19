// Translated from RockAxeWeapon.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.weapon
{
    // Java: 9
    public class RockAxeWeapon : com.oddlabs.tt.model.weapon.RotatingThrowingWeapon
    {
        public float ROTS_PER_SECOND;
        public float ANGLE_DELTA;
        public const float METERS_PER_SECOND = 20.0f;
        // Java: 19
        public override float getAngleVelocity()
        {
            return ANGLE_DELTA;
        }
        // Java: 24
        public override float getMetersPerSecond()
        {
            return com.oddlabs.tt.model.weapon.RockAxeWeapon.METERS_PER_SECOND;
        }
        // Java: 29
        public override int getDamage()
        {
            return 1;
        }
        private void init_RockAxeWeapon()
        {
            this.ROTS_PER_SECOND = 3;
            this.ANGLE_DELTA = (float)(ROTS_PER_SECOND * 360.0f);
        }
        public RockAxeWeapon(bool hit, com.oddlabs.tt.model.Unit src, com.oddlabs.tt.model.Selectable target, com.oddlabs.tt.render.SpriteKey sprite_renderer, com.oddlabs.tt.audio.Audio throw_sound, com.oddlabs.tt.audio.Audio[] hit_sounds) : base(hit, src, target, sprite_renderer, throw_sound, hit_sounds)
        {
            init_RockAxeWeapon();
        }
    }
}
