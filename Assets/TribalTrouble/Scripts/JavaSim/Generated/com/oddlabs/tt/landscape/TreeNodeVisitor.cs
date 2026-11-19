// Translated from TreeNodeVisitor.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.landscape
{
    // Java: 3
    public interface TreeNodeVisitor
    {
        // Java: 4
        void visitLeaf(com.oddlabs.tt.landscape.TreeLeaf tree_leaf);
        // Java: 6
        void visitNode(com.oddlabs.tt.landscape.TreeGroup tree_group);
        // Java: 8
        void visitTree(com.oddlabs.tt.landscape.TreeSupply tree_supply);
    }
}
