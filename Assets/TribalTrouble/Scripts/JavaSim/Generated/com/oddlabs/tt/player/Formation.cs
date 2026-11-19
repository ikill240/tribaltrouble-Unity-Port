// Translated from Formation.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player
{
    // Java: 9
    public sealed class Formation : java.lang.Enum
    {
        public static Formation[] values() { return (Formation[])VALUES_.Clone(); }
        public static Formation valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
        public static readonly Formation DEFAULT = new Formation("DEFAULT", 0);
        public static readonly Formation SQUARE = new Formation("SQUARE", 1);
        public static readonly Formation DIAMOND = new Formation("DIAMOND", 2);
        public static readonly Formation CIRCLE = new Formation("CIRCLE", 3);
        public static readonly Formation STAR = new Formation("STAR", 4);
        public static readonly Formation TIGHT = new Formation("TIGHT", 5);
        public static readonly Formation LOOSE = new Formation("LOOSE", 6);
        public static readonly Formation BY_TYPE = new Formation("BY_TYPE", 7);
        public static readonly Formation[] VALUES_ = new Formation[] { DEFAULT, SQUARE, DIAMOND, CIRCLE, STAR, TIGHT, LOOSE, BY_TYPE };
        public Formation(string name_, int ordinal_) : base(name_, ordinal_)
        {
        }
    }
}
