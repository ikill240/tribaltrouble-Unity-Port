// Translated from FormFactory.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.@delegate
{
    // Java: 8
    public interface FormFactory : java.util.function.Supplier
    {
        // Java: 10
        com.oddlabs.tt.gui.Form create();
        // Java: 13
        object get();
    }
    public sealed class FormFactory_L : java.lang.Object, FormFactory
    {
        readonly System.Func<com.oddlabs.tt.gui.Form> f;
        public FormFactory_L(System.Func<com.oddlabs.tt.gui.Form> f) { this.f = f; }
        public com.oddlabs.tt.gui.Form create() { return f(); }
        public object get()
        {
            return create();
        }
    }
}
