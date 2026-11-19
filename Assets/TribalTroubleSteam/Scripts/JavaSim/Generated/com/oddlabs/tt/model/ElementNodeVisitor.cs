// Translated from ElementNodeVisitor.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 3
    public interface ElementNodeVisitor
    {
        // Java: 4
        void visitNode(com.oddlabs.tt.model.ElementNode node);
        // Java: 6
        void visitLeaf(com.oddlabs.tt.model.ElementLeaf leaf);
        // Java: 8
        void visit(com.oddlabs.tt.model.Element element);
    }
}
