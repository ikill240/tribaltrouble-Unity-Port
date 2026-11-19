// Translated from Strategy.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.fable
{
    // Java: 12
    public interface Strategy
    {
        // Java: 13
        string name();
        // Java: 16
        com.oddlabs.tt.player.fable.Params @params();
        // Java: 19
        public static com.oddlabs.tt.player.fable.Strategy forWorld(com.oddlabs.tt.landscape.World world, com.oddlabs.tt.player.Player owner)
        {
            int size = world.getMapSize();
            int players = world.getPlayers().Length;
            int enemies = 0;
            foreach (com.oddlabs.tt.player.Player p in world.getPlayers())
            {
                if (owner.isEnemy(p))
                {
                    enemies++;
                }
            }
            return ((size) switch { 0 => (com.oddlabs.tt.player.fable.Strategy)(new com.oddlabs.tt.player.fable.Strategy.Scaled("small-island", 0.5f, enemies)), 1 => (com.oddlabs.tt.player.fable.Strategy)(new com.oddlabs.tt.player.fable.Strategy.Scaled("medium-island", 0.75f, enemies)), 3 => (com.oddlabs.tt.player.fable.Strategy)(new com.oddlabs.tt.player.fable.Strategy.Scaled("enormous-island", 1.4f, enemies)), _ => (com.oddlabs.tt.player.fable.Strategy)((players >= 6 ? (com.oddlabs.tt.player.fable.Strategy)(new com.oddlabs.tt.player.fable.Strategy.Scaled("large-island-crowded", 0.6f, enemies, true)) : (players > 2 ? (com.oddlabs.tt.player.fable.Strategy)(new com.oddlabs.tt.player.fable.Strategy.Scaled("large-island-ffa", 1.0f, enemies)) : (com.oddlabs.tt.player.fable.Strategy)(new com.oddlabs.tt.player.fable.Strategy.LargeIsland1v1())))) });
        }
        // Java: 38
        public class LargeIsland1v1 : java.lang.Object, com.oddlabs.tt.player.fable.Strategy
        {
            public com.oddlabs.tt.player.fable.Params params_f;
            // Java: 41
            public string name()
            {
                return "large-island-1v1";
            }
            // Java: 46
            public com.oddlabs.tt.player.fable.Params @params()
            {
                return params_f;
            }
            private void init_LargeIsland1v1()
            {
                this.params_f = new com.oddlabs.tt.player.fable.Params();
            }
            public LargeIsland1v1() : base()
            {
                init_LargeIsland1v1();
            }
        }
        // Java: 53
        public class Scaled : java.lang.Object, com.oddlabs.tt.player.fable.Strategy
        {
            public string name_f;
            public com.oddlabs.tt.player.fable.Params params_f;
            // Java: 86
            public string name()
            {
                return name_f;
            }
            // Java: 91
            public com.oddlabs.tt.player.fable.Params @params()
            {
                return params_f;
            }
            private void init_Scaled()
            {
                this.params_f = new com.oddlabs.tt.player.fable.Params();
            }
            public Scaled(string name_v, float scale, int enemies) : this(name_v, scale, enemies, false)
            {
            }
            public Scaled(string name_v, float scale, int enemies, bool crowded) : base()
            {
                init_Scaled();
                this.name_f = name_v;
                params_f.armory_max_dist = java.lang.Math.round((float)(params_f.armory_max_dist * scale));
                if (crowded)
                {
                    params_f.armory_sector_deg = 180.0f;
                    params_f.quarters_max_dist = java.lang.Math.min(params_f.quarters_max_dist, 20);
                    params_f.expansion_armory = false;
                }
                params_f.staging_dist = java.lang.Math.max(10, java.lang.Math.round((float)(params_f.staging_dist * scale)));
                params_f.leg_length = java.lang.Math.max(15, java.lang.Math.round((float)(params_f.leg_length * scale)));
                params_f.hunt_max_dist = java.lang.Math.round((float)(params_f.hunt_max_dist * scale));
                if (scale < 1.0f)
                {
                    params_f.attack_min_time = java.lang.Math.round((float)(params_f.attack_min_time * scale));
                    params_f.chieftain_min_time = java.lang.Math.round((float)(params_f.chieftain_min_time * scale));
                    params_f.quarters_first_crew = 14;
                }
                if (enemies > 1)
                {
                    params_f.home_guard_early += 2;
                    params_f.home_guard_mid += 2;
                    params_f.home_guard_late += 2;
                    params_f.attack_ratio += 0.3f;
                }
            }
        }
    }
}
