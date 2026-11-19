// Translated from Context.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.ultra
{
    // Java: 16
    public class Context : java.lang.Object
    {
        // Java: 18
        public sealed class Role : java.lang.Enum
        {
            public static Role[] values() { return (Role[])VALUES_.Clone(); }
            public static Role valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly Role BUILDER = new Role("BUILDER", 0);
            public static readonly Role HUNTER = new Role("HUNTER", 1);
            public static readonly Role CORPS = new Role("CORPS", 2);
            public static readonly Role EVACUEE = new Role("EVACUEE", 3);
            public static readonly Role[] VALUES_ = new Role[] { BUILDER, HUNTER, CORPS, EVACUEE };
            public Role(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        public com.oddlabs.tt.player.Player owner;
        public com.oddlabs.tt.landscape.World world;
        public com.oddlabs.tt.pathfinder.UnitGrid grid;
        public com.oddlabs.tt.model.Race race;
        public com.oddlabs.tt.player.ultra.Params @params;
        public com.oddlabs.tt.player.ultra.Strategy strategy;
        public java.util.Random rng;
        public com.oddlabs.tt.player.ultra.MapAnalysis map;
        public com.oddlabs.tt.player.ultra.WorldModel model;
        public com.oddlabs.tt.player.ultra.SiteFinder sites;
        public com.oddlabs.tt.player.ultra.Orders orders;
        public java.util.Map roles;
        public com.oddlabs.tt.player.ultra.ResourceTracker resources;
        public com.oddlabs.tt.player.ultra.BuildManager build;
        public com.oddlabs.tt.player.ultra.EconomyManager economy;
        public com.oddlabs.tt.player.ultra.MilitaryManager military;
        public com.oddlabs.tt.player.ultra.TowerManager towers;
        public com.oddlabs.tt.player.ultra.ChieftainManager chief;
        public com.oddlabs.tt.player.ultra.CorpsManager corps;
        public com.oddlabs.tt.player.ultra.CreepManager creep;
        public com.oddlabs.tt.player.ultra.ProxyManager proxy;
        public float now;
        public int error_count;
        // Java: 73
        public com.oddlabs.tt.player.ultra.Context.Role roleOf(com.oddlabs.tt.model.Unit u)
        {
            return ((com.oddlabs.tt.player.ultra.Context.Role)roles.get(u));
        }
        // Java: 77
        public bool hasRole(com.oddlabs.tt.model.Unit u)
        {
            return roles.containsKey(u);
        }
        // Java: 81
        public void setRole(com.oddlabs.tt.model.Unit u, com.oddlabs.tt.player.ultra.Context.Role role)
        {
            if (role == null)
            {
                roles.remove(u);
            }
            else
            {
                roles.put(u, role);
            }
        }
        // Java: 88
        public void clearRole(com.oddlabs.tt.model.Unit u)
        {
            roles.remove(u);
        }
        // Java: 93
        public void pruneRoles()
        {
            roles.keySet().removeIf(new java.util.function.Predicate_L((object a0) => { return ((com.oddlabs.tt.model.Unit)a0).isDead(); }));
        }
        // Java: 97
        public void log(java.util.function.Supplier message)
        {
            if (com.oddlabs.tt.player.ultra.UltraLog.enabled())
            {
                com.oddlabs.tt.player.ultra.UltraLog.log(owner, new java.util.function.Supplier_L(() =>
                {
                    return java.lang.JStr.valueOf(java.lang.JString.format("%7.1f ", now)) + java.lang.JStr.valueOf(((string)message.get()));
                }));
            }
        }
        private void init_Context()
        {
            this.roles = new java.util.LinkedHashMap();
        }
        public Context(com.oddlabs.tt.player.Player owner, com.oddlabs.tt.player.ultra.Params @params, com.oddlabs.tt.player.ultra.Strategy strategy) : base()
        {
            init_Context();
            this.owner = owner;
            this.world = owner.getWorld();
            this.grid = world.getUnitGrid();
            this.race = owner.getRace();
            this.@params = @params;
            this.strategy = strategy;
            int slot = 0;
            com.oddlabs.tt.player.Player[] players = world.getPlayers();
            for (int i = 0; i < players.Length; i++)
            {
                if (((object)players[i] == (object)owner))
                {
                    slot = i;
                }
            }
            this.rng = new java.util.Random(30583L ^ slot);
            this.map = new com.oddlabs.tt.player.ultra.MapAnalysis(owner);
            this.model = new com.oddlabs.tt.player.ultra.WorldModel(owner);
            this.sites = new com.oddlabs.tt.player.ultra.SiteFinder(map);
            this.orders = new com.oddlabs.tt.player.ultra.Orders(this);
        }
    }
}
