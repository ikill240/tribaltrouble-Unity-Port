// Translated from CursorType.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 6
    public sealed class CursorType : java.lang.Enum
    {
        public static CursorType[] values() { return (CursorType[])VALUES_.Clone(); }
        public static CursorType valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
        public static readonly CursorType NORMAL = new CursorType("NORMAL", 0);
        public static readonly CursorType TARGET = new CursorType("TARGET", 1);
        public static readonly CursorType TEXT = new CursorType("TEXT", 2);
        public static readonly CursorType DEBUG = new CursorType("DEBUG", 3);
        public static readonly CursorType HIDDEN = new CursorType("HIDDEN", 4);
        public static readonly CursorType NULL = new CursorType("NULL", 5);
        public static readonly CursorType[] VALUES_ = new CursorType[] { NORMAL, TARGET, TEXT, DEBUG, HIDDEN, NULL };
        public CursorType(string name_, int ordinal_) : base(name_, ordinal_)
        {
        }
    }
}
