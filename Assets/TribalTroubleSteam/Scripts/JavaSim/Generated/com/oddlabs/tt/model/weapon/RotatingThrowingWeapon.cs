// Translated from RotatingThrowingWeapon.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.weapon
{
    // Java: 10
    public abstract class RotatingThrowingWeapon : com.oddlabs.tt.model.weapon.ThrowingWeapon
    {
        public float angle;
        // Java: 18
        public void setAngle(float angle)
        {
            this.angle = angle;
        }
        // Java: 22
        public float getAngle()
        {
            return angle;
        }
        // Java: 26
        public override void animate(float t)
        {
            base.animate(t);
            setAngle((float)(getAngle() + (float)(getAngleVelocity() * t)));
        }
        // Java: 32
        public abstract float getAngleVelocity();
        // Java: 34
        public override void visit(com.oddlabs.tt.model.ElementVisitor visitor)
        {
            visitor.visitRotatingThrowingWeapon(this);
        }
        private void init_RotatingThrowingWeapon()
        {
            this.angle = 0;
        }
        public RotatingThrowingWeapon(bool hit, com.oddlabs.tt.model.Unit src, com.oddlabs.tt.model.Selectable target, com.oddlabs.tt.render.SpriteKey sprite_renderer, com.oddlabs.tt.audio.Audio throw_sound, com.oddlabs.tt.audio.Audio[] hit_sounds) : base(hit, src, target, sprite_renderer, throw_sound, hit_sounds)
        {
            init_RotatingThrowingWeapon();
        }
    }
}
