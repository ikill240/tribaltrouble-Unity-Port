// Translated from PlayerStyleProfile.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player
{
    // Java: 38
    public class PlayerStyleProfile : java.lang.Object
    {
        public static java.util.logging.Logger logger;
        public const float DEFAULT_AXIS_VALUE = 0.5f;
        public const float SAMPLE_WEIGHT = 0.08f;
        public const float SAMPLE_INTERVAL_SECONDS = 20.0f;
        public static com.oddlabs.tt.player.PlayerStyleProfile instance;
        public float aggression;
        public float economy_focus;
        public int prev_units_killed;
        public int prev_units_lost;
        public int prev_buildings_destroyed;
        public int prev_buildings_lost;
        public int prev_harvested;
        public float time_since_sample;
        // Java: 73
        public static com.oddlabs.tt.player.PlayerStyleProfile fresh()
        {
            return new com.oddlabs.tt.player.PlayerStyleProfile();
        }
        // Java: 78
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public static com.oddlabs.tt.player.PlayerStyleProfile get()
        {
            if (com.oddlabs.tt.player.PlayerStyleProfile.instance == null)
            {
                com.oddlabs.tt.player.PlayerStyleProfile.instance = new com.oddlabs.tt.player.PlayerStyleProfile();
                com.oddlabs.tt.player.PlayerStyleProfile.instance.load();
            }
            return com.oddlabs.tt.player.PlayerStyleProfile.instance;
        }
        // Java: 88
        public float getAggression()
        {
            return aggression;
        }
        // Java: 92
        public float getEconomyFocus()
        {
            return economy_focus;
        }
        // Java: 100
        public void update(com.oddlabs.tt.player.Player subject, float t)
        {
            time_since_sample += t;
            if (time_since_sample < com.oddlabs.tt.player.PlayerStyleProfile.SAMPLE_INTERVAL_SECONDS)
            {
                return;
            }
            float elapsed = time_since_sample;
            time_since_sample = 0.0f;
            int units_killed = subject.getUnitsKilled();
            int units_lost = subject.getUnitsLost();
            int buildings_destroyed = subject.getBuildingsDestroyed();
            int buildings_lost = subject.getBuildingsLost();
            int harvested = subject.getTreeHarvested() + subject.getRockHarvested() + subject.getIronHarvested() + subject.getRubberHarvested();
            if (prev_units_killed < 0)
            {
                prev_units_killed = units_killed;
                prev_units_lost = units_lost;
                prev_buildings_destroyed = buildings_destroyed;
                prev_buildings_lost = buildings_lost;
                prev_harvested = harvested;
                return;
            }
            int combat_delta = java.lang.Math.max(0, units_killed - prev_units_killed) + java.lang.Math.max(0, units_lost - prev_units_lost) + java.lang.Math.max(0, buildings_destroyed - prev_buildings_destroyed) + java.lang.Math.max(0, buildings_lost - prev_buildings_lost);
            int harvest_delta = java.lang.Math.max(0, harvested - prev_harvested);
            prev_units_killed = units_killed;
            prev_units_lost = units_lost;
            prev_buildings_destroyed = buildings_destroyed;
            prev_buildings_lost = buildings_lost;
            prev_harvested = harvested;
            if (combat_delta == 0 && harvest_delta == 0)
            {
                return;
            }
            float combat_rate = (float)(combat_delta / elapsed);
            float harvest_rate = (float)(harvest_delta / elapsed);
            float aggression_sample = (float)(combat_rate / ((float)(combat_rate + harvest_rate)));
            aggression = (float)(aggression + (float)(com.oddlabs.tt.player.PlayerStyleProfile.SAMPLE_WEIGHT * ((float)(aggression_sample - aggression))));
            economy_focus = (float)(economy_focus + (float)(com.oddlabs.tt.player.PlayerStyleProfile.SAMPLE_WEIGHT * ((float)(((float)(1.0f - aggression_sample)) - economy_focus))));
            aggression = java.lang.Math.clamp(aggression, 0.0f, 1.0f);
            economy_focus = java.lang.Math.clamp(economy_focus, 0.0f, 1.0f);
            save();
        }
        // Java: 158
        public java.nio.file.Path file()
        {
            java.nio.file.Path dir = (com.oddlabs.tt.render.Renderer.getLocalInput() == null ? (java.nio.file.Path)(null) : com.oddlabs.tt.render.Renderer.getLocalInput().getGameDir());
            return (dir == null ? (java.nio.file.Path)(null) : dir.resolve(com.oddlabs.tt.global.Globals.getPlayerStyleProfileFileName()));
        }
        // Java: 163
        public void load()
        {
            java.nio.file.Path file_v = file();
            if (file_v == null || !java.nio.file.Files.exists(file_v))
            {
                return;
            }
            java.util.Properties props = new java.util.Properties();
            {
                java.io.InputStream @in = java.nio.file.Files.newInputStream(file_v);
                try
                {
                    props.load(@in);
                }
                catch (System.IO.IOException e)
                {
                    com.oddlabs.tt.player.PlayerStyleProfile.logger.log(java.util.logging.Level.WARNING, "Failed to read player style profile from " + java.lang.JStr.valueOf(file_v) + "; using default.", e);
                    return;
                }
                finally
                {
                    if (@in != null) @in.close();
                }
            }
            try
            {
                aggression = java.lang.Math.clamp(java.lang.Float.parseFloat(props.getProperty("aggression", java.lang.Float.toString(com.oddlabs.tt.player.PlayerStyleProfile.DEFAULT_AXIS_VALUE))), 0.0f, 1.0f);
                economy_focus = java.lang.Math.clamp(java.lang.Float.parseFloat(props.getProperty("economy_focus", java.lang.Float.toString(com.oddlabs.tt.player.PlayerStyleProfile.DEFAULT_AXIS_VALUE))), 0.0f, 1.0f);
            }
            catch (System.FormatException e_2)
            {
                com.oddlabs.tt.player.PlayerStyleProfile.logger.log(java.util.logging.Level.WARNING, "Invalid value in " + java.lang.JStr.valueOf(file_v) + "; using defaults.", e_2);
                aggression = com.oddlabs.tt.player.PlayerStyleProfile.DEFAULT_AXIS_VALUE;
                economy_focus = com.oddlabs.tt.player.PlayerStyleProfile.DEFAULT_AXIS_VALUE;
            }
        }
        // Java: 187
        public void save()
        {
            java.util.Properties props = new java.util.Properties();
            props.setProperty("aggression", java.lang.Float.toString(aggression));
            props.setProperty("economy_focus", java.lang.Float.toString(economy_focus));
            java.nio.file.Path file_v = file();
            if (file_v == null)
            {
                return;
            }
            {
                java.io.OutputStream @out = java.nio.file.Files.newOutputStream(file_v);
                try
                {
                    props.store(@out, java.lang.JO.toString(java.time.Instant.now()));
                }
                catch (System.IO.IOException e)
                {
                    com.oddlabs.tt.player.PlayerStyleProfile.logger.log(java.util.logging.Level.WARNING, "Failed to write player style profile to " + java.lang.JStr.valueOf(file_v), e);
                }
                finally
                {
                    if (@out != null) @out.close();
                }
            }
        }
        static PlayerStyleProfile()
        {
            PlayerStyleProfile.logger = java.util.logging.Logger.getLogger(typeof(com.oddlabs.tt.player.PlayerStyleProfile).getName());
        }
        private void init_PlayerStyleProfile()
        {
            this.aggression = com.oddlabs.tt.player.PlayerStyleProfile.DEFAULT_AXIS_VALUE;
            this.economy_focus = com.oddlabs.tt.player.PlayerStyleProfile.DEFAULT_AXIS_VALUE;
            this.prev_units_killed = (-1);
            this.prev_units_lost = (-1);
            this.prev_buildings_destroyed = (-1);
            this.prev_buildings_lost = (-1);
            this.prev_harvested = (-1);
            this.time_since_sample = 0.0f;
        }
        public PlayerStyleProfile() : base()
        {
            init_PlayerStyleProfile();
        }
    }
}
