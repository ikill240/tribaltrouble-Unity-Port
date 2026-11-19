// Translated from PathNode.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.pathfinder
{
    // Java: 3
    public interface PathNode
    {
        // Java: 4
        com.oddlabs.tt.pathfinder.PathNode getParent();
    }
    public sealed class PathNode_L : java.lang.Object, PathNode
    {
        readonly System.Func<com.oddlabs.tt.pathfinder.PathNode> f;
        public PathNode_L(System.Func<com.oddlabs.tt.pathfinder.PathNode> f) { this.f = f; }
        public com.oddlabs.tt.pathfinder.PathNode getParent() { return f(); }
    }
}
