// Translated from DeployType.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 3
    public sealed class DeployType : java.lang.Enum
    {
        public static DeployType[] values() { return (DeployType[])VALUES_.Clone(); }
        public static DeployType valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
        public static readonly DeployType ROCK_WARRIOR = new DeployType("ROCK_WARRIOR", 0);
        public static readonly DeployType IRON_WARRIOR = new DeployType("IRON_WARRIOR", 1);
        public static readonly DeployType RUBBER_WARRIOR = new DeployType("RUBBER_WARRIOR", 2);
        public static readonly DeployType PEON = new DeployType("PEON", 3);
        public static readonly DeployType PEON_HARVEST_TREE = new DeployType("PEON_HARVEST_TREE", 4);
        public static readonly DeployType PEON_TRANSPORT_TREE = new DeployType("PEON_TRANSPORT_TREE", 5);
        public static readonly DeployType PEON_HARVEST_ROCK = new DeployType("PEON_HARVEST_ROCK", 6);
        public static readonly DeployType PEON_TRANSPORT_ROCK = new DeployType("PEON_TRANSPORT_ROCK", 7);
        public static readonly DeployType PEON_HARVEST_IRON = new DeployType("PEON_HARVEST_IRON", 8);
        public static readonly DeployType PEON_TRANSPORT_IRON = new DeployType("PEON_TRANSPORT_IRON", 9);
        public static readonly DeployType PEON_HARVEST_RUBBER = new DeployType("PEON_HARVEST_RUBBER", 10);
        public static readonly DeployType PEON_TRANSPORT_RUBBER = new DeployType("PEON_TRANSPORT_RUBBER", 11);
        public static readonly DeployType[] VALUES_ = new DeployType[] { ROCK_WARRIOR, IRON_WARRIOR, RUBBER_WARRIOR, PEON, PEON_HARVEST_TREE, PEON_TRANSPORT_TREE, PEON_HARVEST_ROCK, PEON_TRANSPORT_ROCK, PEON_HARVEST_IRON, PEON_TRANSPORT_IRON, PEON_HARVEST_RUBBER, PEON_TRANSPORT_RUBBER };
        public DeployType(string name_, int ordinal_) : base(name_, ordinal_)
        {
        }
    }
}
