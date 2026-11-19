// Translated from NodeResult.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.pathfinder
{
    // Java: 3
    public class NodeResult : java.lang.Object
    {
        public com.oddlabs.tt.pathfinder.Node result;
        // Java: 10
        public com.oddlabs.tt.pathfinder.Node get()
        {
            return result;
        }
        public NodeResult(com.oddlabs.tt.pathfinder.Node node) : base()
        {
            this.result = node;
        }
    }
}
