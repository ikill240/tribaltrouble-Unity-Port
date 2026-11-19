// Translated from RubberSpearWeapon.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.weapon
{
    // Java: 11
    public class RubberSpearWeapon : com.oddlabs.tt.model.weapon.DirectedThrowingWeapon
    {
        public const float METERS_PER_SECOND = 28.0f;
        public int MAX_BOUNDS_LENGTH;
        // Java: 20
        public override void hitTarget(bool hit, com.oddlabs.tt.player.Player owner, com.oddlabs.tt.model.Selectable target)
        {
            if (hit)
            {
                damageTarget(target);
            }
            int bounce = (owner.getWorld().isExtended() ? owner.getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.CHICKEN_BOUNCE) : 0);
            com.oddlabs.tt.model.AttackScanFilter filter = new com.oddlabs.tt.model.AttackScanFilter(owner, MAX_BOUNDS_LENGTH + bounce);
            owner.getWorld().getUnitGrid().scan(filter, target.getGridX(), target.getGridY());
            com.oddlabs.tt.model.Selectable s = filter.removeTarget();
            if (s != null && owner.getWorld().getRandom().nextFloat() > (float)(0.5f - (float)(0.1f * bounce)))
            {
                setTarget(s);
            }
            else
            {
                base.hitTarget(hit, owner, target);
            }
        }
        // Java: 35
        public override float getMetersPerSecond()
        {
            return com.oddlabs.tt.model.weapon.RubberSpearWeapon.METERS_PER_SECOND;
        }
        // Java: 40
        public override int getDamage()
        {
            return 2;
        }
        private void init_RubberSpearWeapon()
        {
            this.MAX_BOUNDS_LENGTH = 5;
        }
        public RubberSpearWeapon(bool hit, com.oddlabs.tt.model.Unit src, com.oddlabs.tt.model.Selectable target, com.oddlabs.tt.render.SpriteKey sprite_renderer, com.oddlabs.tt.audio.Audio throw_sound, com.oddlabs.tt.audio.Audio[] hit_sounds) : base(hit, src, target, sprite_renderer, throw_sound, hit_sounds)
        {
            init_RubberSpearWeapon();
        }
    }
}
