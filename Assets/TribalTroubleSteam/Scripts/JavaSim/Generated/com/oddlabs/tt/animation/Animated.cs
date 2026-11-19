// Translated from Animated.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.animation
{
    // Java: 9
    public interface Animated
    {
        // Java: 11
        void animate(float t);
        // Java: 13
        void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum);
    }
    public sealed class Animated_L : java.lang.Object, Animated
    {
        readonly System.Action<float> f;
        public Animated_L(System.Action<float> f) { this.f = f; }
        public void animate(float p0) { f(p0); }
        public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
        }
    }
}
