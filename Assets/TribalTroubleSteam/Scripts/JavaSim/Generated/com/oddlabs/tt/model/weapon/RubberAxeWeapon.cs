// Translated from RubberAxeWeapon.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.weapon
{
    // Java: 11
    public class RubberAxeWeapon : com.oddlabs.tt.model.weapon.RotatingThrowingWeapon
    {
        public float ROTS_PER_SECOND;
        public float ANGLE_DELTA;
        public int MAX_BOUNDS_LENGTH;
        public const float METERS_PER_SECOND = 30.0f;
        public const float BOUNCING_METERS_PER_SECOND = 10.0f;
        public bool bouncing;
        // Java: 25
        public override float getAngleVelocity()
        {
            return ANGLE_DELTA;
        }
        // Java: 30
        public override void hitTarget(bool hit, com.oddlabs.tt.player.Player owner, com.oddlabs.tt.model.Selectable target)
        {
            if (hit)
            {
                damageTarget(target);
            }
            com.oddlabs.tt.model.AttackScanFilter filter = new com.oddlabs.tt.model.AttackScanFilter(owner, MAX_BOUNDS_LENGTH);
            owner.getWorld().getUnitGrid().scan(filter, target.getGridX(), target.getGridY());
            com.oddlabs.tt.model.Selectable s = filter.removeTarget();
            if (s != null && owner.getWorld().getRandom().nextFloat() > 0.5f)
            {
                bouncing = true;
                setTarget(s);
            }
            else
            {
                base.hitTarget(hit, owner, target);
            }
        }
        // Java: 44
        public override float getMetersPerSecond()
        {
            return (bouncing ? com.oddlabs.tt.model.weapon.RubberAxeWeapon.BOUNCING_METERS_PER_SECOND : com.oddlabs.tt.model.weapon.RubberAxeWeapon.METERS_PER_SECOND);
        }
        // Java: 49
        public override int getDamage()
        {
            return 2;
        }
        private void init_RubberAxeWeapon()
        {
            this.ROTS_PER_SECOND = 9;
            this.ANGLE_DELTA = (float)(ROTS_PER_SECOND * 360.0f);
            this.MAX_BOUNDS_LENGTH = 3;
            this.bouncing = false;
        }
        public RubberAxeWeapon(bool hit, com.oddlabs.tt.model.Unit src, com.oddlabs.tt.model.Selectable target, com.oddlabs.tt.render.SpriteKey sprite_renderer, com.oddlabs.tt.audio.Audio throw_sound, com.oddlabs.tt.audio.Audio[] hit_sounds) : base(hit, src, target, sprite_renderer, throw_sound, hit_sounds)
        {
            init_RubberAxeWeapon();
        }
    }
}
