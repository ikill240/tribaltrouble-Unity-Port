// Translated from KingOfTheIslandModeRules.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gamemode.koth
{
    // Java: 51
    public class KingOfTheIslandModeRules : java.lang.Object, com.oddlabs.tt.gamemode.GameModeRules
    {
        public const float CAPTURE_RADIUS = 15.0f;
        public const float HOLD_SECONDS_TO_WIN = 60.0f;
        public const float WIN_FRACTION = 0.8f;
        public const int TREASURE_INDEX = 0;
        public const float RING_RADIUS_FRACTION = 0.35f;
        public const string OPTION_CAPTURE_RADIUS = "koth_capture_radius";
        public const string OPTION_HOLD_SECONDS = "koth_hold_seconds";
        public static java.util.List OPTIONS;
        public int winning_team;
        public int leading_team;
        public float hold_time;
        public float[] statue_x;
        public float[] statue_y;
        public int[] statue_controlling_team;
        // Java: 91
        public int getStatueCount()
        {
            return statue_x.Length;
        }
        // Java: 95
        public float getStatueX(int index)
        {
            return statue_x[index];
        }
        // Java: 99
        public float getStatueY(int index)
        {
            return statue_y[index];
        }
        // Java: 105
        public int getStatueControllingTeam(int index)
        {
            return statue_controlling_team[index];
        }
        // Java: 109
        public int getLeadingTeam()
        {
            return leading_team;
        }
        // Java: 113
        public int getWinningTeam()
        {
            return winning_team;
        }
        // Java: 117
        public static float getCaptureRadius()
        {
            return com.oddlabs.tt.gamemode.koth.KingOfTheIslandModeRules.CAPTURE_RADIUS;
        }
        // Java: 121
        public java.util.List getOptions()
        {
            return com.oddlabs.tt.gamemode.koth.KingOfTheIslandModeRules.OPTIONS;
        }
        // Java: 126
        public bool isPlayerAlive(com.oddlabs.tt.player.Player player)
        {
            if (winning_team != com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL)
            {
                return player.getPlayerInfo().getTeam() == winning_team;
            }
            int units = player.getUnitCountContainer().getNumSupplies();
            return units > 0 || player.hasActiveChieftain() || player.getQuarters() != null;
        }
        // Java: 141
        public void onGameStart(com.oddlabs.tt.viewer.WorldViewer viewer)
        {
            com.oddlabs.tt.gui.Label koth_status_label = new com.oddlabs.tt.gui.Label("", com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont(), 500, com.oddlabs.tt.gui.Origin.AT_MIDDLE);
            koth_status_label.setPos((viewer.getGUIRoot().getWidth() - koth_status_label.getWidth()) / 2, 10);
            viewer.getGUIRoot().addChild(koth_status_label);
            onWorldStart(viewer.getWorld(), new java.util.function.Consumer_L((object a0) => { koth_status_label.setText(((string)a0)); }));
        }
        // Java: 149
        public void onWorldStart(com.oddlabs.tt.landscape.World world, java.util.function.Consumer status)
        {
            winning_team = com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL;
            leading_team = com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL;
            hold_time = 0.0f;
            int statue_count = java.lang.Math.clamp(world.getKothStatueCount(), com.oddlabs.tt.landscape.WorldParameters.MIN_KOTH_STATUE_COUNT, com.oddlabs.tt.landscape.WorldParameters.MAX_KOTH_STATUE_COUNT);
            int center_grid = world.getHeightMap().getGridUnitsPerWorld() / 2;
            float center_x = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(center_grid);
            float center_y = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(center_grid);
            float ring_radius = (float)(com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(center_grid) * com.oddlabs.tt.gamemode.koth.KingOfTheIslandModeRules.RING_RADIUS_FRACTION);
            statue_x = new float[statue_count];
            statue_y = new float[statue_count];
            statue_controlling_team = new int[statue_count];
            for (int i = 0; i < statue_count; i++)
            {
                double angle = 2 * java.lang.Math.PI * i / statue_count;
                statue_x[i] = (float)(center_x + (float)(ring_radius * ((float)(java.lang.StrictMath.cos(angle)))));
                statue_y[i] = (float)(center_y + (float)(ring_radius * ((float)(java.lang.StrictMath.sin(angle)))));
                statue_controlling_team[i] = com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL;
                new com.oddlabs.tt.model.SceneryModel(world, statue_x[i], statue_y[i], 0.0f, 1.0f, world.getRacesResources().getTreasures()[com.oddlabs.tt.gamemode.koth.KingOfTheIslandModeRules.TREASURE_INDEX], 2.6f, false, "King of the Island");
            }
            int win_threshold = java.lang.JC.d2i(java.lang.Math.ceil((float)(statue_count * com.oddlabs.tt.gamemode.koth.KingOfTheIslandModeRules.WIN_FRACTION)));
            world.getAnimationManagerGameTime().registerAnimation(new KingOfTheIslandModeRules.Anon1_Animated(this, world, win_threshold, status));
            com.oddlabs.tt.gamemode.ObjectiveDirector.hold(world, statue_x, statue_y, com.oddlabs.tt.gamemode.koth.KingOfTheIslandModeRules.CAPTURE_RADIUS, win_threshold, new java.util.function.IntUnaryOperator_L((int i_2) =>
            {
                return statue_controlling_team[i_2];
            }), new java.util.function.IntSupplier_L(() =>
            {
                return leading_team;
            }));
        }
        public string last_status;
        // Java: 313
        public void show(java.util.function.Consumer status, string text)
        {
            if (status != null && !java.lang.JO.equals(text, last_status))
            {
                status.accept(text);
            }
            last_status = text;
        }
        // Java: 319
        public string describe()
        {
            return last_status;
        }
        static KingOfTheIslandModeRules()
        {
            KingOfTheIslandModeRules.OPTIONS = java.util.List_.of(new com.oddlabs.matchmaking.GameModeOption(com.oddlabs.tt.gamemode.koth.KingOfTheIslandModeRules.OPTION_CAPTURE_RADIUS, com.oddlabs.matchmaking.GameModeOption.Type.FLOAT, com.oddlabs.tt.gamemode.koth.KingOfTheIslandModeRules.CAPTURE_RADIUS, "koth_capture_radius", 5.0f, 40.0f, null), new com.oddlabs.matchmaking.GameModeOption(com.oddlabs.tt.gamemode.koth.KingOfTheIslandModeRules.OPTION_HOLD_SECONDS, com.oddlabs.matchmaking.GameModeOption.Type.FLOAT, com.oddlabs.tt.gamemode.koth.KingOfTheIslandModeRules.HOLD_SECONDS_TO_WIN, "koth_hold_seconds", 15.0f, 300.0f, null));
        }
        private void init_KingOfTheIslandModeRules()
        {
            this.winning_team = com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL;
            this.leading_team = com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL;
            this.hold_time = 0.0f;
            this.statue_x = new float[0];
            this.statue_y = new float[0];
            this.statue_controlling_team = new int[0];
            this.last_status = "";
        }
        public KingOfTheIslandModeRules() : base()
        {
            init_KingOfTheIslandModeRules();
        }
        // Java: anonymous class at line 215
        public sealed class Anon1_Animated : java.lang.Object, com.oddlabs.tt.animation.Animated
        {
            public com.oddlabs.tt.gamemode.koth.KingOfTheIslandModeRules this_0;
            public com.oddlabs.tt.landscape.World world;
            public int win_threshold;
            public java.util.function.Consumer status;
            public Anon1_Animated(com.oddlabs.tt.gamemode.koth.KingOfTheIslandModeRules this_0_, com.oddlabs.tt.landscape.World world_c, int win_threshold_c, java.util.function.Consumer status_c)
            {
                this.this_0 = this_0_;
                this.world = world_c;
                this.win_threshold = win_threshold_c;
                this.status = status_c;
                init_Anon1_Animated();
            }
            public const float CHECK_INTERVAL = 0.5f;
            public float t_since_last_check;
            // Java: 219
            public void animate(float t)
            {
                if (this_0.winning_team != com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL)
                {
                    return;
                }
                t_since_last_check += t;
                if (t_since_last_check < KingOfTheIslandModeRules.Anon1_Animated.CHECK_INTERVAL)
                {
                    return;
                }
                float elapsed = t_since_last_check;
                t_since_last_check = 0.0f;
                java.util.Map statues_per_team = new java.util.HashMap();
                for (int i_2 = 0; i_2 < this_0.statue_x.Length; i_2++)
                {
                    com.oddlabs.tt.pathfinder.FindOccupantFilter filter = new com.oddlabs.tt.pathfinder.FindOccupantFilter(this_0.statue_x[i_2], this_0.statue_y[i_2], com.oddlabs.tt.gamemode.koth.KingOfTheIslandModeRules.CAPTURE_RADIUS, null, typeof(com.oddlabs.tt.model.Unit));
                    world.getUnitGrid().scan(filter, com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(this_0.statue_x[i_2]), com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(this_0.statue_y[i_2]));
                    java.util.Map team_presence = new java.util.HashMap();
                    foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(filter.getResult()))
                    {
                        if (u.isDead())
                        {
                            continue;
                        }
                        int team = u.getOwner().getPlayerInfo().getTeam();
                        if (team == com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL)
                        {
                            continue;
                        }
                        team_presence.merge(team, 1, new java.util.function.BiFunction_L((object a0, object a1) => { return java.lang.Integer.sum(((int)a0), ((int)a1)); }));
                    }
                    int controller = (team_presence.size() == 1 ? ((int)((int?)team_presence.keySet().iterator().next())) : com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL);
                    this_0.statue_controlling_team[i_2] = controller;
                    if (controller != com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL)
                    {
                        statues_per_team.merge(controller, 1, new java.util.function.BiFunction_L((object a0, object a1) => { return java.lang.Integer.sum(((int)a0), ((int)a1)); }));
                    }
                }
                int best_team = com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL;
                int best_count = 0;
                bool tied = false;
                foreach (java.util.Map.Entry entry in java.lang.JIter.of(statues_per_team.entrySet()))
                {
                    if (((int)((int?)entry.getValue())) > best_count)
                    {
                        best_team = ((int)((int?)entry.getKey()));
                        best_count = ((int)((int?)entry.getValue()));
                        tied = false;
                    }
                    else if (((int?)entry.getValue()).intValue() == best_count)
                    {
                        tied = true;
                    }
                }
                int qualifying_team = (!tied && best_count >= win_threshold ? best_team : com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL);
                if (qualifying_team != com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL && qualifying_team == this_0.leading_team)
                {
                    this_0.hold_time += elapsed;
                    if (this_0.hold_time >= com.oddlabs.tt.gamemode.koth.KingOfTheIslandModeRules.HOLD_SECONDS_TO_WIN)
                    {
                        this_0.winning_team = this_0.leading_team;
                    }
                }
                else
                {
                    this_0.leading_team = qualifying_team;
                    this_0.hold_time = 0.0f;
                }
                if (this_0.winning_team != com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL)
                {
                    this_0.show(status, "Team " + ((this_0.winning_team + 1)) + " has won the Island!");
                }
                else if (this_0.leading_team != com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL)
                {
                    int seconds_left = java.lang.JC.d2i(java.lang.Math.ceil((float)(com.oddlabs.tt.gamemode.koth.KingOfTheIslandModeRules.HOLD_SECONDS_TO_WIN - this_0.hold_time)));
                    this_0.show(status, "Team " + ((this_0.leading_team + 1)) + " controls " + (best_count) + "/" + (this_0.statue_x.Length) + " statues - " + (seconds_left) + "s to win");
                }
                else if (best_count > 0)
                {
                    this_0.show(status, "Best control: " + (best_count) + "/" + (this_0.statue_x.Length) + " statues (need " + (win_threshold) + " to lead)");
                }
                else
                {
                    this_0.show(status, "The Island's statues are uncontrolled");
                }
            }
            private void init_Anon1_Animated()
            {
                this.t_since_last_check = 0.0f;
            }
            // mixin of default method Animated.updateChecksum
            public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
            {
            }
        }
    }
}
