// Translated from TreeLeaf.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.landscape
{
    // Java: 5
    public class TreeLeaf : com.oddlabs.tt.landscape.AbstractTreeGroup
    {
        public com.oddlabs.tt.landscape.TreeSupply[] infos;
        public int state_version;
        public object render_cache;
        // Java: 15
        public void insertTree(com.oddlabs.tt.landscape.TreeSupply tree)
        {
            com.oddlabs.tt.landscape.TreeSupply[] new_infos = new com.oddlabs.tt.landscape.TreeSupply[infos.Length + 1];
            java.lang.JSystem.arraycopy(infos, 0, new_infos, 0, infos.Length);
            new_infos[new_infos.Length - 1] = tree;
            infos = new_infos;
        }
        // Java: 22
        public void takeOut(com.oddlabs.tt.landscape.TreeSupply tree)
        {
            com.oddlabs.tt.landscape.TreeSupply[] new_infos = new com.oddlabs.tt.landscape.TreeSupply[infos.Length - 1];
            int j = 0;
            foreach (com.oddlabs.tt.landscape.TreeSupply info in infos)
            {
                if (((object)info != (object)tree))
                {
                    new_infos[j++] = info;
                }
            }
            infos = new_infos;
        }
        // Java: 32
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
        // Java: 49
        public override void visit(com.oddlabs.tt.landscape.TreeNodeVisitor visitor)
        {
            visitor.visitLeaf(this);
        }
        // Java: 55
        public int getStateVersion()
        {
            return state_version;
        }
        // Java: 59
        public void treeStateChanged()
        {
            state_version++;
        }
        // Java: 64
        public com.oddlabs.tt.landscape.TreeSupply[] getTrees()
        {
            return infos;
        }
        // Java: 68
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
