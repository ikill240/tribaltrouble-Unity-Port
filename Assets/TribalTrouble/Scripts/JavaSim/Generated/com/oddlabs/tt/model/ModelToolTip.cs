// Translated from ModelToolTip.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 3
    public interface ModelToolTip
    {
        // Java: 4
        void visit(com.oddlabs.tt.model.ToolTipVisitor visitor);
    }
    public sealed class ModelToolTip_L : java.lang.Object, ModelToolTip
    {
        readonly System.Action<com.oddlabs.tt.model.ToolTipVisitor> f;
        public ModelToolTip_L(System.Action<com.oddlabs.tt.model.ToolTipVisitor> f) { this.f = f; }
        public void visit(com.oddlabs.tt.model.ToolTipVisitor p0) { f(p0); }
    }
}
