// Translated from FogInfo.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.resource
{
    // Java: 6
    public class FogInfo : java.lang.Object
    {
        // Java: 8
        public sealed class Mode : java.lang.Enum
        {
            public static Mode[] values() { return (Mode[])VALUES_.Clone(); }
            public static Mode valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly Mode NONE = new Mode("NONE", 0);
            public static readonly Mode LINEAR = new Mode("LINEAR", 1);
            public static readonly Mode EXP = new Mode("EXP", 2);
            public static readonly Mode EXP2 = new Mode("EXP2", 3);
            public static readonly Mode RADIAL = new Mode("RADIAL", 4);
            public static readonly Mode[] VALUES_ = new Mode[] { NONE, LINEAR, EXP, EXP2, RADIAL };
            public Mode(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        public com.oddlabs.tt.resource.FogInfo.Mode mode;
        public org.joml.Vector4fc color;
        public float density;
        public bool enabled;
        // Java: 36
        public virtual void setEnabled(bool enabled)
        {
            this.enabled = enabled;
        }
        // Java: 40
        public virtual bool isEnabled()
        {
            return enabled;
        }
        // Java: 44
        public virtual org.joml.Vector4fc getColor()
        {
            return color;
        }
        // Java: 48
        public virtual float getDensity()
        {
            return density;
        }
        // Java: 52
        public virtual com.oddlabs.tt.resource.FogInfo.Mode getMode()
        {
            return mode;
        }
        private void init_FogInfo()
        {
            this.enabled = true;
        }
        public FogInfo(com.oddlabs.tt.resource.FogInfo.Mode mode, org.joml.Vector4fc color, float density) : base()
        {
            init_FogInfo();
            this.mode = mode;
            this.color = color;
            this.density = density;
        }
    }
}
