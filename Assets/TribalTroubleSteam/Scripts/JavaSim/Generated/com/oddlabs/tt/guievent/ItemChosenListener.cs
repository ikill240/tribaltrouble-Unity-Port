// Translated from ItemChosenListener.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.guievent
{
    // Java: 6
    public interface ItemChosenListener : com.oddlabs.tt.guievent.EventListener
    {
        // Java: 8
        void itemChosen(com.oddlabs.tt.gui.PulldownMenu menu, int item_index);
    }
    public sealed class ItemChosenListener_L : java.lang.Object, ItemChosenListener
    {
        readonly System.Action<com.oddlabs.tt.gui.PulldownMenu, int> f;
        public ItemChosenListener_L(System.Action<com.oddlabs.tt.gui.PulldownMenu, int> f) { this.f = f; }
        public void itemChosen(com.oddlabs.tt.gui.PulldownMenu p0, int p1) { f(p0, p1); }
    }
}
