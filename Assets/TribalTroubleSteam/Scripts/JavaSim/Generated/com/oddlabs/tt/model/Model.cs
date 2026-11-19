// Translated from Model.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 11
    public abstract class Model : com.oddlabs.tt.model.Element
    {
        public com.oddlabs.tt.landscape.World world;
        // Java: 19
        public override com.oddlabs.tt.model.Element self()
        {
            return this;
        }
        // Java: 24
        public virtual float getShadowDiameter()
        {
            return 0.0f;
        }
        // Java: 28
        public virtual float getShadowFade()
        {
            return 0.0f;
        }
        // Java: 32
        public virtual float getOffsetZ()
        {
            return 0.0f;
        }
        // Java: 36
        public virtual int getAnimation()
        {
            return 0;
        }
        // Java: 40
        public virtual float getAnimationTicks()
        {
            return 0.0f;
        }
        // Java: 44
        public virtual float getNoDetailSize()
        {
            return 0.0f;
        }
        // Java: 48
        public abstract com.oddlabs.tt.render.SpriteKey getSpriteRenderer();
        // Java: 50
        public void updateBounds()
        {
            float x = getPositionX();
            float y = getPositionY();
            float z = getPositionZ();
            com.oddlabs.tt.util.BoundingBox unit_bounds = getSpriteRenderer().getBounds(getAnimation());
            float error = getZError();
            setBounds((float)(unit_bounds.bmin_x + x), (float)(unit_bounds.bmax_x + x), (float)(unit_bounds.bmin_y + y), (float)(unit_bounds.bmax_y + y), (float)((float)(unit_bounds.bmin_z + z) - error), (float)((float)(unit_bounds.bmax_z + z) + error));
        }
        // Java: 60
        public virtual float getZError()
        {
            return 0.0f;
        }
        // Java: 64
        public float getLandscapeError()
        {
            return world.getHeightMap().getLeafFromCoordinates(getPositionX(), getPositionY()).getMaxError();
        }
        // Java: 68
        public com.oddlabs.tt.landscape.World getWorld()
        {
            return world;
        }
        // Java: 72
        public override void setPosition(float x, float y)
        {
            base.setPosition(x, y);
            reinsert();
        }
        // Java: 81
        public virtual void onReinsert()
        {
        }
        // Java: 85
        public void reinsert()
        {
            if (isRegistered())
            {
                com.oddlabs.tt.model.Element @ref = getReference();
                if (@ref == null)
                {
                    setPositionZ((float)(java.lang.Math.max(world.getHeightMap().getSeaLevelMeters(), world.getHeightMap().getNearestHeight(getPositionX(), getPositionY())) + getOffsetZ()));
                }
                else
                {
                    setPositionZ((float)(@ref.getPositionZ() + getOffsetZ()));
                }
                updateBounds();
                onReinsert();
                reregister();
            }
        }
        public Model(com.oddlabs.tt.landscape.World world) : base(((com.oddlabs.tt.landscape.World)java.util.Objects.requireNonNull(world, "world")).getElementRoot())
        {
            this.world = world;
        }
    }
}
