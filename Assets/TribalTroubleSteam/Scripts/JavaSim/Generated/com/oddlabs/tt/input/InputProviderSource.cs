// Translated from InputProviderSource.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.input
{
    // Java: 9
    public interface InputProviderSource
    {
        // Java: 10
        com.oddlabs.tt.input.InputProvider createInputProvider();
    }
    public sealed class InputProviderSource_L : java.lang.Object, InputProviderSource
    {
        readonly System.Func<com.oddlabs.tt.input.InputProvider> f;
        public InputProviderSource_L(System.Func<com.oddlabs.tt.input.InputProvider> f) { this.f = f; }
        public com.oddlabs.tt.input.InputProvider createInputProvider() { return f(); }
    }
}
