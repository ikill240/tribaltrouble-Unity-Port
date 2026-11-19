// Translated from Resource.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.mapeditor
{
    // Java: 8
    public sealed class Resource : java.lang.Enum
    {
        public static Resource[] values() { return (Resource[])VALUES_.Clone(); }
        public static Resource valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
        public static readonly Resource TREE = new Resource("TREE", 0);
        public static readonly Resource PALM = new Resource("PALM", 1);
        public static readonly Resource ROCK = new Resource("ROCK", 2);
        public static readonly Resource IRON = new Resource("IRON", 3);
        // Java: 16
        public bool isTree()
        {
            return ((object)this == (object)com.oddlabs.tt.mapeditor.Resource.TREE) || ((object)this == (object)com.oddlabs.tt.mapeditor.Resource.PALM);
        }
        // Java: 21
        public com.oddlabs.tt.landscape.AbstractTreeGroup.TreeType treeType(com.oddlabs.tt.procedural.Landscape.TerrainType terrain)
        {
            return ((this).ordinal() switch { 0 /*TREE*/ => (com.oddlabs.tt.landscape.AbstractTreeGroup.TreeType)((((object)terrain.@base() == (object)com.oddlabs.tt.procedural.Landscape.TerrainType.NATIVE) ? com.oddlabs.tt.landscape.AbstractTreeGroup.TreeType.JUNGLE : com.oddlabs.tt.landscape.AbstractTreeGroup.TreeType.OAK)), 1 /*PALM*/ => (com.oddlabs.tt.landscape.AbstractTreeGroup.TreeType)((((object)terrain.@base() == (object)com.oddlabs.tt.procedural.Landscape.TerrainType.NATIVE) ? com.oddlabs.tt.landscape.AbstractTreeGroup.TreeType.PALM : com.oddlabs.tt.landscape.AbstractTreeGroup.TreeType.PINE)), _ => throw new System.InvalidOperationException(java.lang.JStr.valueOf(this) + " is not a tree") });
        }
        // Java: 29
        public static com.oddlabs.tt.mapeditor.Resource ofTree(com.oddlabs.tt.landscape.AbstractTreeGroup.TreeType type)
        {
            return (((object)type == (object)com.oddlabs.tt.landscape.AbstractTreeGroup.TreeType.JUNGLE) || ((object)type == (object)com.oddlabs.tt.landscape.AbstractTreeGroup.TreeType.OAK) ? com.oddlabs.tt.mapeditor.Resource.TREE : com.oddlabs.tt.mapeditor.Resource.PALM);
        }
        public static readonly Resource[] VALUES_ = new Resource[] { TREE, PALM, ROCK, IRON };
        public Resource(string name_, int ordinal_) : base(name_, ordinal_)
        {
        }
    }
}
