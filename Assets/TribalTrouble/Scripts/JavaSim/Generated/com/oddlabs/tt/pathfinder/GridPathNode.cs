// Translated from GridPathNode.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.pathfinder
{
    // Java: 3
    public class GridPathNode : java.lang.Object, com.oddlabs.tt.pathfinder.PathNode
    {
        public com.oddlabs.tt.pathfinder.GridPathNode parent;
        public com.oddlabs.tt.pathfinder.DirectionNode dir;
        // Java: 12
        public com.oddlabs.tt.pathfinder.DirectionNode getDirection()
        {
            return dir;
        }
        // Java: 16
        public com.oddlabs.tt.pathfinder.PathNode getParent()
        {
            return parent;
        }
        public GridPathNode(com.oddlabs.tt.pathfinder.GridPathNode parent, com.oddlabs.tt.pathfinder.DirectionNode dir) : base()
        {
            this.parent = parent;
            this.dir = dir;
        }
    }
}
