// Translated from SonicBlastEffect.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.particle
{
    // Java: 9
    public class SonicBlastEffect : com.oddlabs.tt.model.Element
    {
        public org.joml.Vector3f position;
        public float maxRadius;
        public float duration;
        public float time;
        public bool dead;
        // Java: 31
        public void update(float dt)
        {
            if (dead)
            {
                return;
            }
            time += dt;
            if (time >= duration)
            {
                dead = true;
                remove();
            }
        }
        // Java: 40
        public void abort()
        {
            if (!dead)
            {
                dead = true;
                remove();
            }
        }
        // Java: 47
        public void updateBounds()
        {
            setBounds((float)(position.x_f - maxRadius), (float)(position.x_f + maxRadius), (float)(position.y_f - maxRadius), (float)(position.y_f + maxRadius), (float)(position.z_f - 1), (float)(position.z_f + 1));
        }
        // Java: 53
        public org.joml.Vector3f getPosition()
        {
            return position;
        }
        // Java: 57
        public float getTime()
        {
            return time;
        }
        // Java: 61
        public float getMaxRadius()
        {
            return maxRadius;
        }
        // Java: 65
        public bool isDead()
        {
            return dead;
        }
        // Java: 69
        public float getRadius()
        {
            return maxRadius;
        }
        // Java: 73
        public float getDuration()
        {
            return duration;
        }
        // Java: 77
        public override void visit(com.oddlabs.tt.model.ElementVisitor visitor)
        {
            visitor.visitSonicBlastEffect(this);
        }
        // Java: 82
        public override com.oddlabs.tt.model.Element self()
        {
            return this;
        }
        // Java: 87
        public override void remove()
        {
            base.remove();
        }
        public SonicBlastEffect(com.oddlabs.tt.landscape.World world, org.joml.Vector3f position, float maxRadius, float duration) : base(world.getElementRoot())
        {
            this.position = position;
            this.maxRadius = maxRadius;
            this.duration = duration;
            this.time = 0;
            this.dead = false;
            setPosition(position.x_f, position.y_f);
            setPositionZ(position.z_f);
            updateBounds();
            register();
        }
    }
}
