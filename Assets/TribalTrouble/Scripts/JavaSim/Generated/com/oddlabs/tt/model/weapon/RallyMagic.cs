// Translated from RallyMagic.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.weapon
{
    // Java: 17
    public class RallyMagic : java.lang.Object, com.oddlabs.tt.model.weapon.Magic
    {
        public com.oddlabs.tt.player.Player owner;
        public com.oddlabs.tt.particle.RandomVelocityEmitter emitter;
        public float life;
        public bool done;
        // Java: 59
        public void animate(float t)
        {
            life -= t;
            if (life <= 0.0f)
            {
                interrupt();
            }
        }
        // Java: 66
        public void interrupt()
        {
            if (done)
            {
                return;
            }
            done = true;
            emitter.done();
            owner.getWorld().getAnimationManagerGameTime().removeAnimation(this);
        }
        private void init_RallyMagic()
        {
            this.life = 1.5f;
        }
        public RallyMagic(com.oddlabs.tt.model.Unit src, float hit_radius, float duration, float speed_mult, float extra_range, float attack_speed_mult, float combat_mult, int heal, bool vikings, org.joml.Vector4f color) : base()
        {
            init_RallyMagic();
            owner = src.getOwner();
            float x = src.getPositionX();
            float y = src.getPositionY();
            float z = (float)(src.getPositionZ() + 2.0f);
            float alpha = 10.0f;
            float energy = 2.5f;
            emitter = new com.oddlabs.tt.particle.RandomVelocityEmitter(owner.getWorld(), new org.joml.Vector3f(x, y, z), 0.0f, 0.0f, 0.001f, 0.001f, 0.5f, ((float)(java.lang.Math.PI)), (-1), 60.0f, new org.joml.Vector3f(0.0f, 0.0f, 7.0f), new org.joml.Vector3f(0.0f, 0.0f, (float)(-3.0f)), (color != null ? (org.joml.Vector4fc)(new org.joml.Vector4f(color.x_f, color.y_f, color.z_f, alpha)) : (vikings ? (org.joml.Vector4fc)(new org.joml.Vector4f(1.0f, 0.55f, 0.3f, alpha)) : (org.joml.Vector4fc)(new org.joml.Vector4f(0.5f, 1.0f, 0.6f, alpha)))), new org.joml.Vector4f(0.0f, 0.0f, 0.0f, (float)((float)(-alpha) / energy)), new org.joml.Vector3f(0.35f, 0.35f, 0.35f), new org.joml.Vector3f(0.03f, 0.03f, 0.03f), energy, 1.0f, org.lwjgl.opengl.GL11.GL_SRC_ALPHA, org.lwjgl.opengl.GL11.GL_ONE, owner.getWorld().getRacesResources().getNoteTextures(), owner.getWorld().getAnimationManagerGameTime());
            com.oddlabs.tt.pathfinder.FindOccupantFilter filter = new com.oddlabs.tt.pathfinder.FindOccupantFilter(x, y, hit_radius, src, com.oddlabs.tt.model.Selectable.genericClass());
            com.oddlabs.tt.pathfinder.UnitGrid unit_grid = owner.getWorld().getUnitGrid();
            unit_grid.scan(filter, com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(x), com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(y));
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(filter.getResult()))
            {
                if ((s is com.oddlabs.tt.model.Unit unit) && !unit.isDead() && !owner.isEnemy(unit.getOwner()))
                {
                    if (heal > 0)
                    {
                        unit.heal(heal);
                    }
                    unit.applyCourageBuff(duration, speed_mult, extra_range, attack_speed_mult, combat_mult, 0.0f);
                }
            }
            if (!src.isDead())
            {
                src.applyCourageBuff(duration, speed_mult, extra_range, attack_speed_mult, combat_mult, 0.0f);
            }
            if (vikings)
            {
                owner.getWorld().getAudio().newAudio(new com.oddlabs.tt.audio.AudioParameters(owner.getWorld().getRacesResources().getStunSound(owner.getWorld().getRandom()), x, y, z, com.oddlabs.tt.audio.AudioPlayer.AUDIO_RANK_MAGIC, com.oddlabs.tt.audio.AudioPlayer.AUDIO_DISTANCE_MAGIC, com.oddlabs.tt.audio.AudioPlayer.AUDIO_GAIN_STUN_LUR, com.oddlabs.tt.audio.AudioPlayer.AUDIO_RADIUS_STUN_LUR, 1.2f));
            }
        }
        // mixin of default method Animated.updateChecksum
        public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
        }
    }
}
