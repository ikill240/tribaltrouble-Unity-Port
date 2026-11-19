// Translated from LightningCloud.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.weapon
{
    // Java: 25
    public class LightningCloud : java.lang.Object, com.oddlabs.tt.model.weapon.Magic
    {
        public const int NUM_STRIKES = 6;
        public const float SECONDS_BETWEEN_STRIKES = 0.125f;
        public const float BRIGHTNESS = 0.2f;
        public const float LIGHTNING_TIME = 0.1f;
        public static org.joml.Vector4fc DELTA_COLOR;
        public com.oddlabs.tt.player.Player owner;
        public float seconds_per_hit;
        public float meters_per_second;
        public float hit_chance;
        public int damage;
        public float height;
        public com.oddlabs.tt.particle.Emitter cloud;
        public org.joml.Vector3f position;
        public com.oddlabs.tt.audio.AbstractAudioPlayer bubbling_sound;
        public com.oddlabs.tt.audio.AbstractAudioPlayer cloud_sound;
        public float seconds_to_live;
        public com.oddlabs.tt.model.Selectable target;
        public com.oddlabs.tt.model.Selectable prev_target;
        public float hit_timer;
        public int strike_counter;
        public float lightning_timer;
        public bool lighted;
        public bool first_run;
        // Java: 85
        public void animate(float t)
        {
            if (first_run)
            {
                cloud_sound = owner.getWorld().getAudio().newAudio(new com.oddlabs.tt.audio.AudioParameters(owner.getWorld().getRacesResources().getCloudSound(), position.x(), position.y(), position.z(), com.oddlabs.tt.audio.AudioPlayer.AUDIO_RANK_MAGIC, com.oddlabs.tt.audio.AudioPlayer.AUDIO_DISTANCE_MAGIC, com.oddlabs.tt.audio.AudioPlayer.AUDIO_GAIN_CLOUD, com.oddlabs.tt.audio.AudioPlayer.AUDIO_RADIUS_CLOUD, 1.0f, true, false));
                first_run = false;
                bubbling_sound.stop(0.2f, com.oddlabs.tt.global.Settings.getSettings().sound_gain);
            }
            cloud_sound.setPos(position.x(), position.y(), position.z());
            seconds_to_live -= t;
            if (seconds_to_live <= 0.0f)
            {
                owner.getWorld().getAnimationManagerGameTime().removeAnimation(this);
                cloud_sound.stop(0.2f, com.oddlabs.tt.global.Settings.getSettings().sound_gain);
            }
            lightning_timer -= t;
            if (lightning_timer <= 0 && lighted)
            {
                cloud.forceColorChange((float)(-com.oddlabs.tt.model.weapon.LightningCloud.BRIGHTNESS), (float)(-com.oddlabs.tt.model.weapon.LightningCloud.BRIGHTNESS), (float)(-com.oddlabs.tt.model.weapon.LightningCloud.BRIGHTNESS), 0.0f);
                lighted = false;
            }
            hit_timer += t;
            if (hit_timer > seconds_per_hit)
            {
                if (target == null)
                {
                    target = owner.findNearestEnemy(com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(position.x()), com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(position.y()), prev_target);
                    if (target == null)
                    {
                        target = owner.findNearestEnemy(com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(position.x()), com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(position.y()));
                        if (target == null)
                        {
                            return;
                        }
                    }
                }
                float dx = (float)(target.getPositionX() - position.x());
                float dy = (float)(target.getPositionY() - position.y());
                float dist = ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy)))));
                dx = (float)(dx / dist);
                dy = (float)(dy / dist);
                if (dist < (float)(meters_per_second * t))
                {
                    if (!target.isDead() && owner.getWorld().getRandom().nextFloat() < (float)(hit_chance * ((float)(1 - target.getDefenseChance()))))
                    {
                        target.hit(damage, dx, dy, owner);
                    }
                    float x = target.getPositionX();
                    float y = target.getPositionY();
                    float z = owner.getWorld().getHeightMap().getNearestHeight(x, y);
                    owner.getWorld().getAudio().newAudio(new com.oddlabs.tt.audio.AudioParameters(owner.getWorld().getRacesResources().getLightningSound(), x, y, z, com.oddlabs.tt.audio.AudioPlayer.AUDIO_RANK_MAGIC, com.oddlabs.tt.audio.AudioPlayer.AUDIO_DISTANCE_MAGIC, com.oddlabs.tt.audio.AudioPlayer.AUDIO_GAIN_LIGHTNING, com.oddlabs.tt.audio.AudioPlayer.AUDIO_RADIUS_LIGHTNING));
                    strike(target);
                    strike(target);
                    prev_target = target;
                    target = null;
                    hit_timer = 0.0f;
                    strike_counter = 0;
                }
                else
                {
                    float x_2 = (float)(position.x() + (float)(dx * ((float)(meters_per_second * t))));
                    float y_2 = (float)(position.y() + (float)(dy * ((float)(meters_per_second * t))));
                    float z_2 = (float)(owner.getWorld().getHeightMap().getNearestHeight(x_2, y_2) + height);
                    position.set(x_2, y_2, z_2);
                }
            }
            else if (prev_target != null && strike_counter < com.oddlabs.tt.model.weapon.LightningCloud.NUM_STRIKES - 1 && hit_timer > (float)((strike_counter + 1) * com.oddlabs.tt.model.weapon.LightningCloud.SECONDS_BETWEEN_STRIKES))
            {
                strike(prev_target);
                strike(prev_target);
                strike_counter++;
            }
        }
        // Java: 164
        public void strike(com.oddlabs.tt.util.Target target)
        {
            if (lightning_timer <= 0.0f)
            {
                cloud.forceColorChange(com.oddlabs.tt.model.weapon.LightningCloud.BRIGHTNESS, com.oddlabs.tt.model.weapon.LightningCloud.BRIGHTNESS, com.oddlabs.tt.model.weapon.LightningCloud.BRIGHTNESS, 0.0f);
                lightning_timer = com.oddlabs.tt.model.weapon.LightningCloud.LIGHTNING_TIME;
                lighted = true;
            }
            float x = target.getPositionX();
            float y = target.getPositionY();
            float z = owner.getWorld().getHeightMap().getNearestHeight(x, y);
            new com.oddlabs.tt.particle.Lightning(owner.getWorld(), position, new org.joml.Vector3f(x, y, z), 0.5f, 15, com.oddlabs.util.Color.WHITE, com.oddlabs.tt.model.weapon.LightningCloud.DELTA_COLOR, owner.getWorld().getRacesResources().getLightningTexture(), com.oddlabs.tt.model.weapon.LightningCloud.LIGHTNING_TIME, owner.getWorld().getAnimationManagerGameTime());
        }
        // Java: 179
        public void interrupt()
        {
            if (bubbling_sound != null)
            {
                bubbling_sound.stop(0.2f, com.oddlabs.tt.global.Settings.getSettings().sound_gain);
            }
        }
        static LightningCloud()
        {
            LightningCloud.DELTA_COLOR = new org.joml.Vector4f(0.0f, 0.0f, 0.0f, (float)((float)(-1.0f) / com.oddlabs.tt.model.weapon.LightningCloud.LIGHTNING_TIME));
        }
        private void init_LightningCloud()
        {
            this.position = new org.joml.Vector3f();
            this.target = null;
            this.prev_target = null;
            this.hit_timer = 0.0f;
            this.strike_counter = 0;
            this.lightning_timer = 0.0f;
            this.lighted = false;
            this.first_run = true;
        }
        public LightningCloud(com.oddlabs.tt.landscape.World world, float offset_x, float offset_y, float offset_z, float seconds_to_live, float seconds_per_hit, float seconds_to_init, float meters_per_second, float hit_chance, int damage, float height, com.oddlabs.tt.model.Unit src) : base()
        {
            init_LightningCloud();
            this.seconds_to_live = seconds_to_live;
            this.seconds_per_hit = seconds_per_hit;
            this.meters_per_second = meters_per_second;
            this.hit_chance = hit_chance;
            this.damage = damage;
            this.height = height;
            owner = src.getOwner();
            float start_x = (float)((float)(src.getPositionX() + (float)(offset_x * src.getDirectionX())) - (float)(offset_y * ((float)(-src.getDirectionY()))));
            float start_y = (float)((float)(src.getPositionY() + (float)(offset_x * src.getDirectionY())) + (float)(offset_y * src.getDirectionX()));
            position.set(start_x, start_y, (float)(world.getHeightMap().getNearestHeight(start_x, start_y) + height));
            cloud = new com.oddlabs.tt.particle.ParametricEmitter(world, new com.oddlabs.tt.particle.CloudFunction(2.5f, 0.7f), position, 0.0f, 0.0f, 0.5f, 0.5f, 0.2f, 25, 100.0f, new org.joml.Vector4f(0.4f, 0.4f, 0.4f, 0.6f), new org.joml.Vector4f(0.0f, 0.0f, 0.0f, 0.0f), new org.joml.Vector3f(3.0f, 3.0f, 1.0f), new org.joml.Vector3f(0.0f, 0.0f, 0.0f), (float)(seconds_to_live + seconds_to_init), org.lwjgl.opengl.GL11.GL_SRC_ALPHA, org.lwjgl.opengl.GL11.GL_ONE_MINUS_SRC_ALPHA, world.getRacesResources().getSmokeTextures(), world.getAnimationManagerGameTime());
            bubbling_sound = world.getAudio().newAudio(new com.oddlabs.tt.audio.AudioParameters(world.getRacesResources().getBubblingSound(), position.x(), position.y(), world.getHeightMap().getNearestHeight(position.x(), position.y()), com.oddlabs.tt.audio.AudioPlayer.AUDIO_RANK_MAGIC, com.oddlabs.tt.audio.AudioPlayer.AUDIO_DISTANCE_MAGIC, com.oddlabs.tt.audio.AudioPlayer.AUDIO_GAIN_BUBBLING, com.oddlabs.tt.audio.AudioPlayer.AUDIO_RADIUS_BUBBLING, 1.0f, true, false));
        }
        // mixin of default method Animated.updateChecksum
        public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
        }
    }
}
