// Translated from DirectedThrowingWeapon.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.weapon
{
    // Java: 10
    public abstract class DirectedThrowingWeapon : com.oddlabs.tt.model.weapon.ThrowingWeapon
    {
        // Java: 16
        public virtual float getAngle()
        {
            return ((float)(java.lang.Math.toDegrees(java.lang.StrictMath.atan2(getZSpeed(), getMetersPerSecond()))));
        }
        // Java: 20
        public override void visit(com.oddlabs.tt.model.ElementVisitor visitor)
        {
            visitor.visitDirectedThrowingWeapon(this);
        }
        public DirectedThrowingWeapon(bool hit, com.oddlabs.tt.model.Unit src, com.oddlabs.tt.model.Selectable target, com.oddlabs.tt.render.SpriteKey sprite_renderer, com.oddlabs.tt.audio.Audio throw_sound, com.oddlabs.tt.audio.Audio[] hit_sounds) : base(hit, src, target, sprite_renderer, throw_sound, hit_sounds)
        {
        }
    }
}
