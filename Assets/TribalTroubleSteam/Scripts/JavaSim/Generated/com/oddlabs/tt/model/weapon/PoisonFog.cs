// Translated from PoisonFog.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.weapon
{
    // Java: 17
    public class PoisonFog : java.lang.Object, com.oddlabs.tt.model.weapon.Magic
    {
        public const float OFFSET_Z = 1.1f;
        public const int PARTICLES_PER_BURST = 4;
        public const float SECONDS_BETWEEN_BURSTS = 0.15f;
        public const float BURST_RADIUS = 2.0f;
        public const float GAUSSIAN_LIMIT = 2.5f;
        public const int MIN_BURSTS_PER_SOUND = 2;
        public float hit_radius;
        public float hit_chance;
        public float interval;
        public int damage;
        public com.oddlabs.tt.model.Unit src;
        public com.oddlabs.tt.player.Player owner;
        public float start_x;
        public float start_y;
        public float total_time;
        public com.oddlabs.tt.audio.AbstractAudioPlayer bubbling_sound;
        public int next_sound;
        public float time;
        public int bursts;
        public int num_hits;
        public bool first_run;
        // Java: 66
        public void animate(float t)
        {
            time += t;
            if (time >= total_time)
            {
                owner.getWorld().getAnimationManagerGameTime().removeAnimation(this);
            }
            if (first_run)
            {
                bubbling_sound.stop(0.2f, com.oddlabs.tt.global.Settings.getSettings().sound_gain);
                first_run = false;
            }
            if ((float)(bursts * com.oddlabs.tt.model.weapon.PoisonFog.SECONDS_BETWEEN_BURSTS) < time)
            {
                float gaussian = (float)(((float)((com.oddlabs.tt.model.weapon.PoisonFog.GAUSSIAN_LIMIT - java.lang.Math.abs(java.lang.Math.max((float)(-com.oddlabs.tt.model.weapon.PoisonFog.GAUSSIAN_LIMIT), java.lang.Math.min(com.oddlabs.tt.model.weapon.PoisonFog.GAUSSIAN_LIMIT, owner.getWorld().getRandom().nextGaussian())))))) / com.oddlabs.tt.model.weapon.PoisonFog.GAUSSIAN_LIMIT);
                float r = (float)(gaussian * ((float)((float)(hit_radius - com.oddlabs.tt.model.weapon.PoisonFog.BURST_RADIUS) - 5.0f)));
                float a = (float)((float)(owner.getWorld().getRandom().nextFloat() * ((float)(java.lang.Math.PI))) * 2);
                float x = (float)(start_x + (float)(((float)(java.lang.StrictMath.cos(a))) * r));
                float y = (float)(start_y + (float)(((float)(java.lang.StrictMath.sin(a))) * r));
                float z = owner.getWorld().getHeightMap().getNearestHeight(x, y);
                float alpha = 8.0f;
                float energy = 2.0f;
                new com.oddlabs.tt.particle.RandomVelocityEmitter(owner.getWorld(), new org.joml.Vector3f(x, y, z), com.oddlabs.tt.model.weapon.PoisonFog.OFFSET_Z, (float)((float)(owner.getWorld().getRandom().nextFloat() * ((float)(java.lang.Math.PI))) * 2), com.oddlabs.tt.model.weapon.PoisonFog.BURST_RADIUS, 0.0f, 0.0f, 0.0f, com.oddlabs.tt.model.weapon.PoisonFog.PARTICLES_PER_BURST, com.oddlabs.tt.model.weapon.PoisonFog.PARTICLES_PER_BURST, new org.joml.Vector3f(0.0f, 0.0f, 0.0f), new org.joml.Vector3f(0.0f, 0.0f, 0.0f), new org.joml.Vector4f(1.0f, 1.0f, 1.0f, alpha), new org.joml.Vector4f(0.0f, 0.0f, 0.0f, (float)((float)(-alpha) / energy)), new org.joml.Vector3f(0.0f, 0.0f, 0.25f), new org.joml.Vector3f(3.5f, 3.5f, 0.0f), energy, 1.0f, org.lwjgl.opengl.GL11.GL_SRC_ALPHA, org.lwjgl.opengl.GL11.GL_ONE_MINUS_SRC_ALPHA, owner.getWorld().getRacesResources().getPoisonTextures(), owner.getWorld().getAnimationManagerGameTime());
                if (bursts % next_sound == 0)
                {
                    next_sound = com.oddlabs.tt.model.weapon.PoisonFog.MIN_BURSTS_PER_SOUND + owner.getWorld().getRandom().nextInt(5);
                    owner.getWorld().getAudio().newAudio(new com.oddlabs.tt.audio.AudioParameters(owner.getWorld().getRacesResources().getGasSound(), x, y, z, com.oddlabs.tt.audio.AudioPlayer.AUDIO_RANK_GAS, com.oddlabs.tt.audio.AudioPlayer.AUDIO_DISTANCE_MAGIC, com.oddlabs.tt.audio.AudioPlayer.AUDIO_GAIN_GAS, com.oddlabs.tt.audio.AudioPlayer.AUDIO_RADIUS_GAS, 1.0f));
                }
                bursts++;
            }
            if ((float)((num_hits + 1) * interval) < time)
            {
                hitUnits(hit_radius);
                num_hits++;
            }
        }
        // Java: 117
        public void hitUnits(float radius)
        {
            com.oddlabs.tt.pathfinder.FindOccupantFilter filter = new com.oddlabs.tt.pathfinder.FindOccupantFilter(start_x, start_y, radius, src, typeof(com.oddlabs.tt.model.Unit));
            com.oddlabs.tt.pathfinder.UnitGrid unit_grid = owner.getWorld().getUnitGrid();
            unit_grid.scan(filter, com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(start_x), com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(start_y));
            foreach (com.oddlabs.tt.model.Unit s in java.lang.JIter.of(filter.getResult()))
            {
                float dx = (float)(s.getPositionX() - start_x);
                float dy = (float)(s.getPositionY() - start_y);
                float squared_dist = (float)((float)(dx * dx) + (float)(dy * dy));
                if (!s.isDead() && ((owner.isEnemy(s.getOwner()) && owner.getWorld().getRandom().nextFloat() < (float)(hit_chance * ((float)(1 - s.getDefenseChance())))) || (!owner.isEnemy(s.getOwner()) && owner.getWorld().getRandom().nextFloat() < (float)(((float)(hit_chance / 4.0f)) * ((float)(1 - s.getDefenseChance()))) && ((object)s != (object)owner.getChieftain()))))
                {
                    float inv_dist = (float)(1.0f / (((float)(java.lang.Math.sqrt(squared_dist)))));
                    s.hit(damage, (float)(dx * inv_dist), (float)(dy * inv_dist), owner);
                }
            }
        }
        // Java: 136
        public void interrupt()
        {
            if (bubbling_sound != null)
            {
                bubbling_sound.stop(0.2f, com.oddlabs.tt.global.Settings.getSettings().sound_gain);
            }
        }
        private void init_PoisonFog()
        {
            this.next_sound = 1;
            this.time = 0.0f;
            this.bursts = 0;
            this.num_hits = 0;
            this.first_run = true;
        }
        public PoisonFog(float offset_x, float offset_y, float offset_z, float hit_radius, float hit_chance, float interval, float time, int damage, com.oddlabs.tt.model.Unit src) : base()
        {
            init_PoisonFog();
            this.hit_radius = hit_radius;
            this.hit_chance = hit_chance;
            this.interval = interval;
            total_time = time;
            this.damage = damage;
            this.src = src;
            owner = src.getOwner();
            start_x = (float)((float)(src.getPositionX() + (float)(offset_x * src.getDirectionX())) - (float)(offset_y * ((float)(-src.getDirectionY()))));
            start_y = (float)((float)(src.getPositionY() + (float)(offset_x * src.getDirectionY())) + (float)(offset_y * src.getDirectionX()));
            bubbling_sound = owner.getWorld().getAudio().newAudio(new com.oddlabs.tt.audio.AudioParameters(owner.getWorld().getRacesResources().getBubblingSound(), start_x, start_y, owner.getWorld().getHeightMap().getNearestHeight(start_x, start_y), com.oddlabs.tt.audio.AudioPlayer.AUDIO_RANK_MAGIC, com.oddlabs.tt.audio.AudioPlayer.AUDIO_DISTANCE_MAGIC, com.oddlabs.tt.audio.AudioPlayer.AUDIO_GAIN_BUBBLING, com.oddlabs.tt.audio.AudioPlayer.AUDIO_RADIUS_BUBBLING, 1.0f, true, false));
        }
        // mixin of default method Animated.updateChecksum
        public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
        }
    }
}
