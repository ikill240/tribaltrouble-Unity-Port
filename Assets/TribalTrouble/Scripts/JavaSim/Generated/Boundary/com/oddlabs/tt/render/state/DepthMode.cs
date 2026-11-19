// GUI port: render states are the Unity host's business; the GUI only names them.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.render.state
{
    public class DepthMode : java.lang.Enum, com.oddlabs.tt.render.state.Mode
    {
        public static readonly DepthMode NONE = new DepthMode("NONE", 0);
        public static readonly DepthMode READ_ONLY = new DepthMode("READ_ONLY", 1);
        public static readonly DepthMode READ_WRITE = new DepthMode("READ_WRITE", 2);
        public static readonly DepthMode[] VALUES_ = { NONE, READ_ONLY, READ_WRITE };
        public static DepthMode[] values() => (DepthMode[])VALUES_.Clone();
        DepthMode(string name, int ordinal) : base(name, ordinal) { }
        public void apply() { }
    }
}
