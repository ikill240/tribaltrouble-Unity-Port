// Translated from RegionNode.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.pathfinder
{
    // Java: 3
    public class RegionNode : java.lang.Object, com.oddlabs.tt.pathfinder.PathNode
    {
        public com.oddlabs.tt.pathfinder.RegionNode parent;
        public com.oddlabs.tt.pathfinder.Region region;
        // Java: 12
        public com.oddlabs.tt.pathfinder.Region getRegion()
        {
            return region;
        }
        // Java: 16
        public com.oddlabs.tt.pathfinder.PathNode getParent()
        {
            return parent;
        }
        public RegionNode(com.oddlabs.tt.pathfinder.RegionNode parent, com.oddlabs.tt.pathfinder.Region region) : base()
        {
            this.parent = parent;
            this.region = region;
        }
    }
}
