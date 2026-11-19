// Translated from Stun.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.weapon
{
    // Java: 21
    public class Stun : java.lang.Object, com.oddlabs.tt.model.weapon.Magic
    {
        public float hit_radius;
        public float stun_time_closest;
        public float stun_time_farthest;
        public com.oddlabs.tt.player.Player owner;
        public float start_x;
        public float start_y;
        public com.oddlabs.tt.particle.RandomVelocityEmitter emitter;
        public com.oddlabs.tt.audio.AbstractAudioPlayer sound;
        public java.lang.Iterable target_list;
        // Java: 72
        public void animate(float t)
        {
            foreach (com.oddlabs.tt.model.Selectable selectable in java.lang.JIter.of(target_list))
            {
                com.oddlabs.tt.model.Unit unit = null;
                if ((selectable is com.oddlabs.tt.model.Unit unit1))
                {
                    unit = unit1;
                }
                else if ((selectable is com.oddlabs.tt.model.Building building))
                {
                    if (!building.isDead() && building.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.ATTACK))
                    {
                        com.oddlabs.tt.model.MountUnitContainer muc = ((com.oddlabs.tt.model.MountUnitContainer)building.getUnitContainer());
                        if (muc.getNumSupplies() > 0)
                        {
                            unit = muc.getUnit();
                        }
                    }
                }
                if (unit == null || unit.isDead())
                {
                    continue;
                }
                float dx = (float)(unit.getPositionX() - start_x);
                float dy = (float)(unit.getPositionY() - start_y);
                float squared_dist = (float)((float)(dx * dx) + (float)(dy * dy));
                if (owner.isEnemy(unit.getOwner()) && squared_dist < (float)(hit_radius * hit_radius))
                {
                    float dist = ((float)(java.lang.Math.sqrt(squared_dist)));
                    float time = calculateValueFromCurrentRadius(dist, stun_time_closest, stun_time_farthest);
                    unit.stun(time);
                }
            }
            interrupt();
        }
        // Java: 102
        public float calculateValueFromCurrentRadius(float current_radius, float max, float min)
        {
            float base_factor = (float)(6.0f / 7.0f);
            float error = ((float)(java.lang.StrictMath.pow(base_factor, hit_radius)));
            float factor = ((float)(java.lang.StrictMath.pow(base_factor, current_radius)));
            float result = (float)((float)((float)(((float)((float)(max - min) + error)) * factor) + min) - error);
            return result;
        }
        // Java: 110
        public void interrupt()
        {
            emitter.done();
            sound.stop(0.3f, com.oddlabs.tt.global.Settings.getSettings().sound_gain);
            owner.getWorld().getAnimationManagerGameTime().removeAnimation(this);
        }
        public Stun(float offset_x, float offset_y, float offset_z, float hit_radius, float stun_time_closest, float stun_time_farthest, com.oddlabs.tt.model.Unit src) : base()
        {
            this.hit_radius = hit_radius;
            this.stun_time_closest = stun_time_closest;
            this.stun_time_farthest = stun_time_farthest;
            this.owner = src.getOwner();
            start_x = (float)((float)(src.getPositionX() + (float)(offset_x * src.getDirectionX())) - (float)(offset_y * ((float)(-src.getDirectionY()))));
            start_y = (float)((float)(src.getPositionY() + (float)(offset_x * src.getDirectionY())) + (float)(offset_y * src.getDirectionX()));
            float z = (float)(src.getPositionZ() + offset_z);
            float alpha = 12.0f;
            float energy = 4.0f;
            emitter = new com.oddlabs.tt.particle.RandomVelocityEmitter(owner.getWorld(), new org.joml.Vector3f(start_x, start_y, z), 0.0f, 0.0f, 0.001f, 0.001f, 0.5f, ((float)(java.lang.Math.PI)), (-1), 35.0f, new org.joml.Vector3f(0.0f, 0.0f, 6.0f), new org.joml.Vector3f(0.0f, 0.0f, (float)(-2.0f)), new org.joml.Vector4f(1.0f, 1.0f, 1.0f, alpha), new org.joml.Vector4f(0.0f, 0.0f, 0.0f, (float)((float)(-alpha) / energy)), new org.joml.Vector3f(0.3f, 0.3f, 0.3f), new org.joml.Vector3f(0.025f, 0.025f, 0.025f), energy, 1.0f, org.lwjgl.opengl.GL11.GL_SRC_ALPHA, org.lwjgl.opengl.GL11.GL_ONE_MINUS_SRC_ALPHA, owner.getWorld().getRacesResources().getNoteTextures(), owner.getWorld().getAnimationManagerGameTime());
            com.oddlabs.tt.pathfinder.FindOccupantFilter filter = new com.oddlabs.tt.pathfinder.FindOccupantFilter(src.getPositionX(), src.getPositionY(), hit_radius, src, com.oddlabs.tt.model.Selectable.genericClass());
            com.oddlabs.tt.pathfinder.UnitGrid unit_grid = owner.getWorld().getUnitGrid();
            unit_grid.scan(filter, com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(src.getPositionX()), com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(src.getPositionY()));
            target_list = filter.getResult();
            sound = owner.getWorld().getAudio().newAudio(new com.oddlabs.tt.audio.AudioParameters(owner.getWorld().getRacesResources().getStunSound(owner.getWorld().getRandom()), start_x, start_y, z, com.oddlabs.tt.audio.AudioPlayer.AUDIO_RANK_MAGIC, com.oddlabs.tt.audio.AudioPlayer.AUDIO_DISTANCE_MAGIC, com.oddlabs.tt.audio.AudioPlayer.AUDIO_GAIN_STUN_LUR, com.oddlabs.tt.audio.AudioPlayer.AUDIO_RADIUS_STUN_LUR, 1.0f));
        }
        // mixin of default method Animated.updateChecksum
        public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
        }
    }
}
