// Translated from SurvivalModeRules.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gamemode.survival
{
    // Java: 31
    public class SurvivalModeRules : java.lang.Object, com.oddlabs.tt.gamemode.GameModeRules
    {
        public const float WAVE_INTERVAL_SECONDS = 60.0f;
        public const int GROWTH_PER_WAVE = 6;
        public const int BASE_WAVE_SIZE = 6;
        public const float SPAWN_SCATTER_RADIUS = 25.0f;
        public const string OPTION_WAVE_INTERVAL = "survival_wave_interval";
        public const string OPTION_GROWTH_PER_WAVE = "survival_growth_per_wave";
        public static java.util.List OPTIONS;
        public int wave_number;
        public float time_since_last_wave;
        public float total_elapsed;
        // Java: 65
        public int getWaveNumber()
        {
            return wave_number;
        }
        // Java: 69
        public java.util.List getOptions()
        {
            return com.oddlabs.tt.gamemode.survival.SurvivalModeRules.OPTIONS;
        }
        // Java: 74
        public bool isPlayerAlive(com.oddlabs.tt.player.Player player)
        {
            int units = player.getUnitCountContainer().getNumSupplies();
            return units > 0 || player.hasActiveChieftain() || player.getQuarters() != null;
        }
        // Java: 85
        public void onGameStart(com.oddlabs.tt.viewer.WorldViewer viewer)
        {
            com.oddlabs.tt.gui.Label survival_status_label = new com.oddlabs.tt.gui.Label("", com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont(), 400, com.oddlabs.tt.gui.Origin.AT_MIDDLE);
            survival_status_label.setPos((viewer.getGUIRoot().getWidth() - survival_status_label.getWidth()) / 2, 10);
            viewer.getGUIRoot().addChild(survival_status_label);
            onWorldStart(viewer.getWorld(), new java.util.function.Consumer_L((object a0) => { survival_status_label.setText(((string)a0)); }));
        }
        public com.oddlabs.tt.gamemode.ObjectiveDirector director;
        public string last_status;
        // Java: 98
        public string describe()
        {
            return last_status;
        }
        // Java: 103
        public void onWorldStart(com.oddlabs.tt.landscape.World world, java.util.function.Consumer status)
        {
            wave_number = 0;
            time_since_last_wave = 0.0f;
            total_elapsed = 0.0f;
            last_status = "";
            director = com.oddlabs.tt.gamemode.ObjectiveDirector.survival(world, new java.util.function.Predicate_L((object p_l) =>
            {
                com.oddlabs.tt.player.Player p = (com.oddlabs.tt.player.Player)p_l;
                return com.oddlabs.tt.gamemode.survival.SurvivalModeRules.isDefender(world, p);
            }));
            world.getAnimationManagerGameTime().registerAnimation(new SurvivalModeRules.Anon1_Animated(this, world, status));
        }
        // Java: 143
        public void spawnWave(com.oddlabs.tt.landscape.World world, int wave)
        {
            int wave_size = com.oddlabs.tt.gamemode.survival.SurvivalModeRules.BASE_WAVE_SIZE + (wave - 1) * com.oddlabs.tt.gamemode.survival.SurvivalModeRules.GROWTH_PER_WAVE;
            int[] warrior_templates = new int[] { com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER };
            foreach (com.oddlabs.tt.player.Player p in world.getPlayers())
            {
                if (p.getAI() == null || !com.oddlabs.tt.gamemode.survival.SurvivalModeRules.isEnemyOfADefender(world, p))
                {
                    continue;
                }
                com.oddlabs.tt.model.Race race = p.getRace();
                float base_x = p.getStartX();
                float base_y = p.getStartY();
                for (int i = 0; i < wave_size; i++)
                {
                    double angle = 2 * java.lang.Math.PI * i / wave_size;
                    float spawn_x = (float)(base_x + ((float)((java.lang.StrictMath.cos(angle) * com.oddlabs.tt.gamemode.survival.SurvivalModeRules.SPAWN_SCATTER_RADIUS))));
                    float spawn_y = (float)(base_y + ((float)((java.lang.StrictMath.sin(angle) * com.oddlabs.tt.gamemode.survival.SurvivalModeRules.SPAWN_SCATTER_RADIUS))));
                    int template = warrior_templates[i % warrior_templates.Length];
                    com.oddlabs.tt.model.Unit u = new com.oddlabs.tt.model.Unit(p, spawn_x, spawn_y, null, race.getUnitTemplate(template));
                    if (director != null)
                    {
                        director.march(u);
                    }
                }
            }
        }
        // Java: 169
        public static bool isDefender(com.oddlabs.tt.landscape.World world, com.oddlabs.tt.player.Player p)
        {
            bool humans = false;
            foreach (com.oddlabs.tt.player.Player h in world.getPlayers())
            {
                humans |= h.getAI() == null;
            }
            if (humans)
            {
                return p.getAI() == null;
            }
            com.oddlabs.tt.player.Player first = world.getPlayers()[0];
            return ((object)p == (object)first) || !p.isEnemy(first);
        }
        // Java: 179
        public static bool isEnemyOfADefender(com.oddlabs.tt.landscape.World world, com.oddlabs.tt.player.Player ai_player)
        {
            foreach (com.oddlabs.tt.player.Player d in world.getPlayers())
            {
                if (com.oddlabs.tt.gamemode.survival.SurvivalModeRules.isDefender(world, d) && d.isEnemy(ai_player))
                {
                    return true;
                }
            }
            return false;
        }
        static SurvivalModeRules()
        {
            SurvivalModeRules.OPTIONS = java.util.List_.of(new com.oddlabs.matchmaking.GameModeOption(com.oddlabs.tt.gamemode.survival.SurvivalModeRules.OPTION_WAVE_INTERVAL, com.oddlabs.matchmaking.GameModeOption.Type.FLOAT, com.oddlabs.tt.gamemode.survival.SurvivalModeRules.WAVE_INTERVAL_SECONDS, "survival_wave_interval", 15.0f, 180.0f, null), new com.oddlabs.matchmaking.GameModeOption(com.oddlabs.tt.gamemode.survival.SurvivalModeRules.OPTION_GROWTH_PER_WAVE, com.oddlabs.matchmaking.GameModeOption.Type.FLOAT, ((float)(com.oddlabs.tt.gamemode.survival.SurvivalModeRules.GROWTH_PER_WAVE)), "survival_growth_per_wave", 1.0f, 10.0f, null));
        }
        private void init_SurvivalModeRules()
        {
            this.wave_number = 0;
            this.time_since_last_wave = 0.0f;
            this.total_elapsed = 0.0f;
            this.last_status = "";
        }
        public SurvivalModeRules() : base()
        {
            init_SurvivalModeRules();
        }
        // Java: anonymous class at line 112
        public sealed class Anon1_Animated : java.lang.Object, com.oddlabs.tt.animation.Animated
        {
            public com.oddlabs.tt.gamemode.survival.SurvivalModeRules this_0;
            public com.oddlabs.tt.landscape.World world;
            public java.util.function.Consumer status;
            public Anon1_Animated(com.oddlabs.tt.gamemode.survival.SurvivalModeRules this_0_, com.oddlabs.tt.landscape.World world_c, java.util.function.Consumer status_c)
            {
                this.this_0 = this_0_;
                this.world = world_c;
                this.status = status_c;
            }
            // Java: 113
            public void animate(float t)
            {
                this_0.total_elapsed += t;
                this_0.time_since_last_wave += t;
                if (this_0.time_since_last_wave >= com.oddlabs.tt.gamemode.survival.SurvivalModeRules.WAVE_INTERVAL_SECONDS)
                {
                    this_0.time_since_last_wave = 0.0f;
                    this_0.wave_number++;
                    this_0.spawnWave(world, this_0.wave_number);
                }
                int minutes = java.lang.JC.f2i(((float)(this_0.total_elapsed / 60.0f)));
                int seconds = java.lang.JC.f2i(((float)(this_0.total_elapsed % 60.0f)));
                string text = "Wave " + (this_0.wave_number) + " - Survived " + (minutes) + ":" + java.lang.JStr.valueOf(((seconds < 10 ? "0" : ""))) + (seconds);
                if (status != null && !java.lang.JO.equals(text, this_0.last_status))
                {
                    status.accept(text);
                }
                this_0.last_status = text;
            }
            // mixin of default method Animated.updateChecksum
            public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
            {
            }
        }
    }
}
