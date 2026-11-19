// Translated from LandscapeTargetRespond.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.landscape
{
    // Java: 9
    public class LandscapeTargetRespond : com.oddlabs.tt.model.Element, com.oddlabs.tt.animation.Animated
    {
        public const int SIZE = 128;
        public const float SECOND_PER_PICK_RESPOND = 0.33333334f;
        public float time;
        // Java: 24
        public override com.oddlabs.tt.model.Element self()
        {
            return this;
        }
        // Java: 29
        public void animate(float t)
        {
            if (time > 0)
            {
                time = java.lang.Math.max(0, (float)(time - t));
            }
            else
            {
                remove();
            }
        }
        // Java: 38
        public float getProgress()
        {
            return (float)(time / com.oddlabs.tt.landscape.LandscapeTargetRespond.SECOND_PER_PICK_RESPOND);
        }
        // Java: 42
        public override void register()
        {
            base.register();
            time = com.oddlabs.tt.landscape.LandscapeTargetRespond.SECOND_PER_PICK_RESPOND;
            com.oddlabs.tt.@event.LocalEventQueue.getQueue().getManager().registerAnimation(this);
        }
        // Java: 49
        public override void visit(com.oddlabs.tt.model.ElementVisitor visitor)
        {
            visitor.visitRespond(this);
        }
        // Java: 54
        public override void remove()
        {
            base.remove();
            com.oddlabs.tt.@event.LocalEventQueue.getQueue().getManager().removeAnimation(this);
        }
        public LandscapeTargetRespond(com.oddlabs.tt.landscape.World world, float x, float y) : base(world.getElementRoot())
        {
            setPosition(x, y);
            setPositionZ(world.getHeightMap().getNearestHeight(x, y));
            setBounds((float)(x - com.oddlabs.tt.landscape.LandscapeTargetRespond.SIZE / 2), (float)(x + com.oddlabs.tt.landscape.LandscapeTargetRespond.SIZE / 2), (float)(y - com.oddlabs.tt.landscape.LandscapeTargetRespond.SIZE / 2), (float)(y + com.oddlabs.tt.landscape.LandscapeTargetRespond.SIZE / 2), java.lang.Float.NEGATIVE_INFINITY, java.lang.Float.POSITIVE_INFINITY);
            register();
        }
        // mixin of default method Animated.updateChecksum
        public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
        }
    }
}
