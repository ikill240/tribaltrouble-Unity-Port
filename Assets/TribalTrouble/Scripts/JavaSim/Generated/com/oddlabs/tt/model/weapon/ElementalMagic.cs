// Translated from ElementalMagic.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.weapon
{
    // Java: 16
    public class ElementalMagic : java.lang.Object, com.oddlabs.tt.model.weapon.Magic
    {
        public const float PULSE = 1.0f;
        public com.oddlabs.tt.player.Player owner;
        public com.oddlabs.tt.model.Unit src;
        public float x;
        public float y;
        public float radius;
        public bool vikings;
        public com.oddlabs.tt.particle.RandomVelocityEmitter emitter;
        public float life;
        public float pulse_time;
        public bool done;
        // Java: 62
        public void animate(float t)
        {
            life -= t;
            pulse_time += t;
            if (pulse_time >= com.oddlabs.tt.model.weapon.ElementalMagic.PULSE)
            {
                pulse_time -= com.oddlabs.tt.model.weapon.ElementalMagic.PULSE;
                pulse();
            }
            if (life <= 0.0f)
            {
                interrupt();
            }
        }
        // Java: 74
        public void pulse()
        {
            com.oddlabs.tt.pathfinder.FindOccupantFilter filter = new com.oddlabs.tt.pathfinder.FindOccupantFilter(x, y, radius, src, com.oddlabs.tt.model.Selectable.genericClass());
            com.oddlabs.tt.pathfinder.UnitGrid grid = owner.getWorld().getUnitGrid();
            grid.scan(filter, com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(x), com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(y));
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(filter.getResult()))
            {
                if (s.isDead())
                {
                    continue;
                }
                bool enemy = owner.isEnemy(s.getOwner());
                if (vikings)
                {
                    if (!enemy)
                    {
                        continue;
                    }
                    if ((s is com.oddlabs.tt.model.Unit u))
                    {
                        u.afflictions().burn(3.0f, 1, owner);
                    }
                    else if ((s is com.oddlabs.tt.model.LandBuilding b))
                    {
                        b.afflictions().burn(3.0f, 3, owner);
                    }
                }
                else if (!enemy && (s is com.oddlabs.tt.model.Unit u_2))
                {
                    u_2.heal(1);
                    if (u_2.hasAfflictions())
                    {
                        u_2.afflictions().cure();
                    }
                }
            }
            if (!vikings && !src.isDead())
            {
                src.heal(1);
            }
        }
        // Java: 99
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
        public ElementalMagic(com.oddlabs.tt.model.Unit src, float radius, float duration, bool vikings) : base()
        {
            this.src = src;
            this.owner = src.getOwner();
            this.x = src.getPositionX();
            this.y = src.getPositionY();
            this.radius = radius;
            this.vikings = vikings;
            this.life = duration;
            float z = src.getPositionZ();
            com.oddlabs.tt.model.RacesResources rr = owner.getWorld().getRacesResources();
            float alpha = (vikings ? 6.0f : 4.0f);
            float energy = (vikings ? 1.6f : 1.4f);
            if (vikings)
            {
                emitter = new com.oddlabs.tt.particle.RandomVelocityEmitter(owner.getWorld(), new org.joml.Vector3f(x, y, (float)(z + 9.0f)), 0.0f, 0.0f, (float)(radius * 0.8f), 1.0f, 0.4f, ((float)(java.lang.Math.PI)), (-1), 90.0f, new org.joml.Vector3f(0.0f, 0.0f, (float)(-6.0f)), new org.joml.Vector3f(0.0f, 0.0f, (float)(-4.0f)), new org.joml.Vector4f(1.0f, 0.45f, 0.1f, alpha), new org.joml.Vector4f(0.0f, (float)(-0.2f), 0.0f, (float)((float)(-alpha) / energy)), new org.joml.Vector3f(0.3f, 0.3f, 0.3f), new org.joml.Vector3f(0.15f, 0.15f, 0.15f), energy, 1.0f, org.lwjgl.opengl.GL11.GL_SRC_ALPHA, org.lwjgl.opengl.GL11.GL_ONE, rr.getSmokeTextures(), owner.getWorld().getAnimationManagerGameTime());
            }
            else
            {
                emitter = new com.oddlabs.tt.particle.RandomVelocityEmitter(owner.getWorld(), new org.joml.Vector3f(x, y, (float)(z + 9.0f)), 0.0f, 0.0f, (float)(radius * 0.8f), 1.0f, 0.2f, ((float)(java.lang.Math.PI)), (-1), 110.0f, new org.joml.Vector3f(0.0f, 0.0f, (float)(-10.0f)), new org.joml.Vector3f(0.0f, 0.0f, (float)(-6.0f)), new org.joml.Vector4f(0.45f, 0.9f, 1.0f, alpha), new org.joml.Vector4f(0.0f, 0.0f, 0.0f, (float)((float)(-alpha) / energy)), new org.joml.Vector3f(0.12f, 0.12f, 0.25f), new org.joml.Vector3f(0.0f, 0.0f, 0.05f), energy, 1.0f, org.lwjgl.opengl.GL11.GL_SRC_ALPHA, org.lwjgl.opengl.GL11.GL_ONE, rr.getPoisonTextures(), owner.getWorld().getAnimationManagerGameTime());
            }
            emitter.setFxTag((vikings ? com.oddlabs.tt.particle.Emitter.FX_EMBER_STORM : com.oddlabs.tt.particle.Emitter.FX_HEALING_RAIN), radius);
        }
        // mixin of default method Animated.updateChecksum
        public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
        }
    }
}
