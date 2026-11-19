// Translated from InstantHitFactory.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.weapon
{
    // Java: 15
    public class InstantHitFactory : com.oddlabs.tt.model.weapon.WeaponFactory
    {
        public com.oddlabs.tt.audio.Audio[] sounds;
        // Java: 23
        public override void doAttack(bool hit, com.oddlabs.tt.model.Unit src, com.oddlabs.tt.model.Selectable target)
        {
            int damage = 1;
            if ((target is com.oddlabs.tt.model.Building) && target.getTemplate().getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.ATTACK))
            {
                damage = 6;
            }
            else if (!hit)
            {
                return;
            }
            float dx = (float)(target.getPositionX() - src.getPositionX());
            float dy = (float)(target.getPositionY() - src.getPositionY());
            float dir_len_inv = (float)(1.0f / ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy))))));
            if ((target is com.oddlabs.tt.model.Unit))
            {
                com.oddlabs.tt.landscape.World world = src.getOwner().getWorld();
                world.getAudio().newAudio(new com.oddlabs.tt.audio.AudioParameters(sounds[world.getRandom().nextInt(sounds.Length)], target.getPositionX(), target.getPositionY(), target.getPositionZ(), com.oddlabs.tt.audio.AudioPlayer.AUDIO_RANK_DEATH, com.oddlabs.tt.audio.AudioPlayer.AUDIO_DISTANCE_DEATH, com.oddlabs.tt.audio.AudioPlayer.AUDIO_GAIN_DEATH, com.oddlabs.tt.audio.AudioPlayer.AUDIO_RADIUS_DEATH, (float)(1.0f + (float)(((float)(world.getRandom().nextFloat() - 0.5f)) * (((com.oddlabs.tt.model.UnitTemplate)target.getTemplate())).getDeathPitch()))));
            }
            target.hit(damage, (float)(dx * dir_len_inv), (float)(dy * dir_len_inv), src.getOwner());
        }
        // Java: 46
        public override System.Type getType()
        {
            return null;
        }
        public InstantHitFactory(float hit_chance, float range, float release_ratio, com.oddlabs.tt.audio.Audio[] sounds) : base(hit_chance, range, release_ratio)
        {
            this.sounds = sounds;
        }
    }
}
