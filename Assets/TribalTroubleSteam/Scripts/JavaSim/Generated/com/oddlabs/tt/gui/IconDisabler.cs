// Translated from IconDisabler.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 3
    public interface IconDisabler
    {
        // Java: 5
        bool isDisabled();
    }
    public sealed class IconDisabler_L : java.lang.Object, IconDisabler
    {
        readonly System.Func<bool> f;
        public IconDisabler_L(System.Func<bool> f) { this.f = f; }
        public bool isDisabled() { return f(); }
    }
}
