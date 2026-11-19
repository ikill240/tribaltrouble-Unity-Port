// GUI port: render states are the Unity host's business; the GUI only names them.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.render.state
{
    public class CullMode : java.lang.Enum, com.oddlabs.tt.render.state.Mode
    {
        public static readonly CullMode NONE = new CullMode("NONE", 0);
        public static readonly CullMode BACK = new CullMode("BACK", 1);
        public static readonly CullMode FRONT = new CullMode("FRONT", 2);
        public static readonly CullMode[] VALUES_ = { NONE, BACK, FRONT };
        public static CullMode[] values() => (CullMode[])VALUES_.Clone();
        CullMode(string name, int ordinal) : base(name, ordinal) { }
        public void apply() { }
    }
}
