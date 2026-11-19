// Translated from TreeLeaf.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.landscape
{
    // Java: 5
    public class TreeLeaf : com.oddlabs.tt.landscape.AbstractTreeGroup
    {
        public com.oddlabs.tt.landscape.TreeSupply[] infos;
        // Java: 12
        public void insertTree(com.oddlabs.tt.landscape.TreeSupply tree)
        {
            com.oddlabs.tt.landscape.TreeSupply[] new_infos = new com.oddlabs.tt.landscape.TreeSupply[infos.Length + 1];
            java.lang.JSystem.arraycopy(infos, 0, new_infos, 0, infos.Length);
            new_infos[new_infos.Length - 1] = tree;
            infos = new_infos;
        }
        // Java: 19
        public override bool initBounds()
        {
            if (infos.Length != 0)
            {
                com.oddlabs.tt.landscape.TreeSupply info = infos[0];
                info.initBounds();
                setBounds(info);
                for (int i = 1; i < infos.Length; i++)
                {
                    info = infos[i];
                    info.initBounds();
                    checkBounds(info);
                }
                base.initBounds();
                return true;
            }
            return false;
        }
        // Java: 36
        public override void visit(com.oddlabs.tt.landscape.TreeNodeVisitor visitor)
        {
            visitor.visitLeaf(this);
        }
        // Java: 41
        public void visitTrees(com.oddlabs.tt.landscape.TreeNodeVisitor visitor)
        {
            foreach (com.oddlabs.tt.landscape.TreeSupply info in infos)
            {
                visitor.visitTree(info);
            }
        }
        private void init_TreeLeaf()
        {
            this.infos = new com.oddlabs.tt.landscape.TreeSupply[0];
        }
        public TreeLeaf(com.oddlabs.tt.landscape.AbstractTreeGroup parent) : base(parent)
        {
            init_TreeLeaf();
        }
    }
}
