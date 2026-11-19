// Translated from ThrowingFactory.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.weapon
{
    // Java: 9
    public class ThrowingFactory : com.oddlabs.tt.model.weapon.WeaponFactory
    {
        // Java: 10
        public interface WeaponConstructor
        {
            // Java: 12
            com.oddlabs.tt.model.weapon.ThrowingWeapon create(bool hit, com.oddlabs.tt.model.Unit src, com.oddlabs.tt.model.Selectable target, com.oddlabs.tt.render.SpriteKey sprite_renderer, com.oddlabs.tt.audio.Audio throw_sound, com.oddlabs.tt.audio.Audio[] hit_sounds);
        }
        public sealed class WeaponConstructor_L : java.lang.Object, WeaponConstructor
        {
            readonly System.Func<bool, com.oddlabs.tt.model.Unit, com.oddlabs.tt.model.Selectable, com.oddlabs.tt.render.SpriteKey, com.oddlabs.tt.audio.Audio, com.oddlabs.tt.audio.Audio[], com.oddlabs.tt.model.weapon.ThrowingWeapon> f;
            public WeaponConstructor_L(System.Func<bool, com.oddlabs.tt.model.Unit, com.oddlabs.tt.model.Selectable, com.oddlabs.tt.render.SpriteKey, com.oddlabs.tt.audio.Audio, com.oddlabs.tt.audio.Audio[], com.oddlabs.tt.model.weapon.ThrowingWeapon> f) { this.f = f; }
            public com.oddlabs.tt.model.weapon.ThrowingWeapon create(bool p0, com.oddlabs.tt.model.Unit p1, com.oddlabs.tt.model.Selectable p2, com.oddlabs.tt.render.SpriteKey p3, com.oddlabs.tt.audio.Audio p4, com.oddlabs.tt.audio.Audio[] p5) { return f(p0, p1, p2, p3, p4, p5); }
        }
        public System.Type weapon_type;
        public com.oddlabs.tt.model.weapon.ThrowingFactory.WeaponConstructor weapon_constructor;
        public com.oddlabs.tt.render.SpriteKey weapon_sprite;
        public com.oddlabs.tt.audio.Audio throw_sound;
        public com.oddlabs.tt.audio.Audio[] hit_sounds;
        // Java: 33
        public override void doAttack(bool hit, com.oddlabs.tt.model.Unit src, com.oddlabs.tt.model.Selectable target)
        {
            weapon_constructor.create(hit, src, target, weapon_sprite, throw_sound, hit_sounds);
        }
        // Java: 38
        public override System.Type getType()
        {
            return weapon_type;
        }
        public ThrowingFactory(System.Type weapon_type, com.oddlabs.tt.model.weapon.ThrowingFactory.WeaponConstructor weapon_constructor, float hit_chance, float range, float release_ratio, com.oddlabs.tt.render.SpriteKey weapon_sprite, com.oddlabs.tt.audio.Audio throw_sound, com.oddlabs.tt.audio.Audio[] hit_sounds) : base(hit_chance, range, release_ratio)
        {
            this.weapon_type = weapon_type;
            this.weapon_constructor = weapon_constructor;
            this.weapon_sprite = weapon_sprite;
            this.throw_sound = throw_sound;
            this.hit_sounds = hit_sounds;
        }
    }
}
