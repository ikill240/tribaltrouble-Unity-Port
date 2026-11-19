// Translated from AnimationInfo.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.geometry
{
    // Java: 8
    public class AnimationInfo : java.lang.Object
    {
        public const long serialVersionUID = 1L;
        // Java: 12
        public sealed class AnimationType : java.lang.Enum
        {
            public static AnimationType[] values() { return (AnimationType[])VALUES_.Clone(); }
            public static AnimationType valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly AnimationType LOOP = new AnimationType("LOOP", 0);
            public static readonly AnimationType PLAIN = new AnimationType("PLAIN", 1);
            public static readonly AnimationType[] VALUES_ = new AnimationType[] { LOOP, PLAIN };
            public AnimationType(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        public float[][] frames;
        public com.oddlabs.geometry.AnimationInfo.AnimationType type;
        public float wpc;
        public string name;
        // Java: 30
        public float[][] getFrames()
        {
            return frames;
        }
        // Java: 34
        public com.oddlabs.geometry.AnimationInfo.AnimationType getType()
        {
            return type;
        }
        // Java: 38
        public float getWPC()
        {
            return wpc;
        }
        // Java: 42
        public string getName()
        {
            return name;
        }
        public AnimationInfo(float[][] frames, com.oddlabs.geometry.AnimationInfo.AnimationType type, float wpc, string name) : base()
        {
            this.frames = frames;
            this.type = type;
            this.wpc = wpc;
            this.name = name;
        }
    }
}
