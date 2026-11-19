// Translated from InputListener.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.guievent
{
    // Java: 6
    public interface InputListener : com.oddlabs.tt.guievent.EventListener
    {
        // Java: 8
        void handleInput(com.oddlabs.tt.input.InputEvent @event);
    }
    public sealed class InputListener_L : java.lang.Object, InputListener
    {
        readonly System.Action<com.oddlabs.tt.input.InputEvent> f;
        public InputListener_L(System.Action<com.oddlabs.tt.input.InputEvent> f) { this.f = f; }
        public void handleInput(com.oddlabs.tt.input.InputEvent p0) { f(p0); }
    }
}
