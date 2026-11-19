// Translated from SonicBlast.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.weapon
{
    // Java: 16
    public class SonicBlast : java.lang.Object, com.oddlabs.tt.model.weapon.Magic
    {
        public float hit_radius;
        public float hit_chance_closest;
        public float hit_chance_farthest;
        public int damage_closest;
        public int damage_farthest;
        public float seconds;
        public com.oddlabs.tt.player.Player owner;
        public float start_x;
        public float start_y;
        public float start_z;
        public com.oddlabs.tt.audio.AbstractAudioPlayer lur;
        public com.oddlabs.tt.audio.AbstractAudioPlayer rumble;
        public float time;
        public java.lang.Iterable blast_targets;
        public com.oddlabs.tt.particle.SonicBlastEffect sonicBlastEffect;
        public bool first_ring_sent;
        // Java: 77
        public void animate(float t)
        {
            time = java.lang.Math.min((float)(time + t), seconds);
            if (time >= seconds)
            {
                owner.getWorld().getAnimationManagerGameTime().removeAnimation(this);
            }
            if (!first_ring_sent)
            {
                first_ring_sent = true;
                owner.getWorld().getAudio().newAudio(new com.oddlabs.tt.audio.AudioParameters(owner.getWorld().getRacesResources().getBlastBlastSound(), start_x, start_y, start_z, com.oddlabs.tt.audio.AudioPlayer.AUDIO_RANK_MAGIC, com.oddlabs.tt.audio.AudioPlayer.AUDIO_DISTANCE_MAGIC, com.oddlabs.tt.audio.AudioPlayer.AUDIO_GAIN_BLAST_BLAST, com.oddlabs.tt.audio.AudioPlayer.AUDIO_RADIUS_BLAST_BLAST, 1.0f));
                lur.stop(0.3f, com.oddlabs.tt.global.Settings.getSettings().sound_gain);
                rumble.stop(0.2f, com.oddlabs.tt.global.Settings.getSettings().sound_gain);
            }
            sonicBlastEffect.update(t);
            float current_radius = (float)((float)(hit_radius * time) / seconds);
            float squared_radius = (float)(current_radius * current_radius);
            java.util.Iterator targets = blast_targets.iterator();
            while (targets.hasNext())
            {
                com.oddlabs.tt.model.Selectable s = ((com.oddlabs.tt.model.Selectable)targets.next());
                float dx = (float)(s.getPositionX() - start_x);
                float dy = (float)(s.getPositionY() - start_y);
                float squared_dist = (float)((float)(dx * dx) + (float)(dy * dy));
                if (squared_dist < squared_radius)
                {
                    if (!s.isDead())
                    {
                        float hit_chance = calculateValueFromCurrentRadius(current_radius, hit_chance_closest, hit_chance_farthest);
                        if (owner.getWorld().getRandom().nextFloat() < (float)(hit_chance * ((float)(1 - s.getDefenseChance()))))
                        {
                            int damage = java.lang.JC.f2i(calculateValueFromCurrentRadius(current_radius, damage_closest, damage_farthest));
                            float inv_dist = (float)(1.0f / (((float)(java.lang.Math.sqrt(squared_dist)))));
                            s.hit(damage, (float)(dx * inv_dist), (float)(dy * inv_dist), owner);
                        }
                    }
                    targets.remove();
                }
            }
        }
        // Java: 125
        public float calculateValueFromCurrentRadius(float current_radius, float max, float min)
        {
            float base_factor = (float)(4.0f / 7.0f);
            float error = ((float)(java.lang.StrictMath.pow(base_factor, hit_radius)));
            float factor = ((float)(java.lang.StrictMath.pow(base_factor, current_radius)));
            float result = (float)((float)((float)(((float)((float)(max - min) + error)) * factor) + min) - error);
            return result;
        }
        // Java: 133
        public void interrupt()
        {
            lur.stop(0.2f, com.oddlabs.tt.global.Settings.getSettings().sound_gain);
            rumble.stop(0.2f, com.oddlabs.tt.global.Settings.getSettings().sound_gain);
            sonicBlastEffect.abort();
        }
        private void init_SonicBlast()
        {
            this.time = 0.0f;
            this.first_ring_sent = false;
        }
        public SonicBlast(float offset_x, float offset_y, float offset_z, float hit_radius, float hit_chance_closest, float hit_chance_farthest, int damage_closest, int damage_farthest, float seconds, com.oddlabs.tt.model.Unit src) : base()
        {
            init_SonicBlast();
            this.hit_radius = hit_radius;
            this.hit_chance_closest = hit_chance_closest;
            this.hit_chance_farthest = hit_chance_farthest;
            this.damage_closest = damage_closest;
            this.damage_farthest = damage_farthest;
            this.seconds = seconds;
            owner = src.getOwner();
            start_x = (float)((float)(src.getPositionX() + (float)(offset_x * src.getDirectionX())) - (float)(offset_y * ((float)(-src.getDirectionY()))));
            start_y = (float)((float)(src.getPositionY() + (float)(offset_x * src.getDirectionY())) + (float)(offset_y * src.getDirectionX()));
            start_z = (float)(src.getPositionZ() + offset_z);
            com.oddlabs.tt.pathfinder.FindOccupantFilter filter = new com.oddlabs.tt.pathfinder.FindOccupantFilter(src.getPositionX(), src.getPositionY(), hit_radius, src, com.oddlabs.tt.model.Selectable.genericClass());
            com.oddlabs.tt.pathfinder.UnitGrid unit_grid = owner.getWorld().getUnitGrid();
            unit_grid.scan(filter, com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(src.getPositionX()), com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(src.getPositionY()));
            blast_targets = filter.getResult();
            sonicBlastEffect = new com.oddlabs.tt.particle.SonicBlastEffect(owner.getWorld(), new org.joml.Vector3f(start_x, start_y, start_z), hit_radius, seconds);
            lur = owner.getWorld().getAudio().newAudio(new com.oddlabs.tt.audio.AudioParameters(owner.getWorld().getRacesResources().getBlastLurSound(owner.getWorld().getRandom()), start_x, start_y, start_z, com.oddlabs.tt.audio.AudioPlayer.AUDIO_RANK_MAGIC, com.oddlabs.tt.audio.AudioPlayer.AUDIO_DISTANCE_MAGIC, com.oddlabs.tt.audio.AudioPlayer.AUDIO_GAIN_BLAST_LUR, com.oddlabs.tt.audio.AudioPlayer.AUDIO_RADIUS_BLAST_LUR, 1.0f));
            rumble = owner.getWorld().getAudio().newAudio(new com.oddlabs.tt.audio.AudioParameters(owner.getWorld().getRacesResources().getBlastRumbleSound(), start_x, start_y, start_z, com.oddlabs.tt.audio.AudioPlayer.AUDIO_RANK_MAGIC, com.oddlabs.tt.audio.AudioPlayer.AUDIO_DISTANCE_MAGIC, com.oddlabs.tt.audio.AudioPlayer.AUDIO_GAIN_BLAST_RUMBLE, com.oddlabs.tt.audio.AudioPlayer.AUDIO_RADIUS_BLAST_RUMBLE, 1.0f));
        }
        // mixin of default method Animated.updateChecksum
        public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
        }
    }
}
