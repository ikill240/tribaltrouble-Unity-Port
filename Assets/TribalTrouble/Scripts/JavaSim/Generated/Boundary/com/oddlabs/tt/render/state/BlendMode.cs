// GUI port: render states are the Unity host's business; the GUI only names them.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.render.state
{
    public class BlendMode : java.lang.Enum, com.oddlabs.tt.render.state.Mode
    {
        public static readonly BlendMode NONE = new BlendMode("NONE", 0);
        public static readonly BlendMode ALPHA = new BlendMode("ALPHA", 1);
        public static readonly BlendMode ADDITIVE = new BlendMode("ADDITIVE", 2);
        public static readonly BlendMode PREMULTIPLIED = new BlendMode("PREMULTIPLIED", 3);
        public static readonly BlendMode CUSTOM = new BlendMode("CUSTOM", 4);
        public static readonly BlendMode[] VALUES_ = { NONE, ALPHA, ADDITIVE, PREMULTIPLIED, CUSTOM };
        public static BlendMode[] values() => (BlendMode[])VALUES_.Clone();
        BlendMode(string name, int ordinal) : base(name, ordinal) { }
        public void apply() { }
    }
}
