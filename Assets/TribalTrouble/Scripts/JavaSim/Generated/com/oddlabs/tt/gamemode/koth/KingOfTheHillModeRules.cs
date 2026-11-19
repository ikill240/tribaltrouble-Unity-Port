// Translated from KingOfTheHillModeRules.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gamemode.koth
{
    // Java: 52
    public class KingOfTheHillModeRules : java.lang.Object, com.oddlabs.tt.gamemode.GameModeRules
    {
        public const float CAPTURE_RADIUS = 15.0f;
        public const float HOLD_SECONDS_TO_WIN = 120.0f;
        public const int TREASURE_INDEX = 0;
        public const string OPTION_CAPTURE_RADIUS = "kingofthehill_capture_radius";
        public const string OPTION_HOLD_SECONDS = "kingofthehill_hold_seconds";
        public static java.util.List OPTIONS;
        public int winning_team;
        public int holding_team;
        public float hold_time;
        public float center_x;
        public float center_y;
        // Java: 88
        public float getCenterX()
        {
            return center_x;
        }
        // Java: 92
        public float getCenterY()
        {
            return center_y;
        }
        // Java: 96
        public int getHoldingTeam()
        {
            return holding_team;
        }
        // Java: 100
        public int getWinningTeam()
        {
            return winning_team;
        }
        // Java: 104
        public static float getCaptureRadius()
        {
            return com.oddlabs.tt.gamemode.koth.KingOfTheHillModeRules.CAPTURE_RADIUS;
        }
        // Java: 108
        public java.util.List getOptions()
        {
            return com.oddlabs.tt.gamemode.koth.KingOfTheHillModeRules.OPTIONS;
        }
        // Java: 113
        public bool isPlayerAlive(com.oddlabs.tt.player.Player player)
        {
            if (winning_team != com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL)
            {
                return player.getPlayerInfo().getTeam() == winning_team;
            }
            int units = player.getUnitCountContainer().getNumSupplies();
            return units > 0 || player.hasActiveChieftain() || player.getQuarters() != null;
        }
        // Java: 128
        public void onGameStart(com.oddlabs.tt.viewer.WorldViewer viewer)
        {
            com.oddlabs.tt.gui.Label koth_status_label = new com.oddlabs.tt.gui.Label("", com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont(), 400, com.oddlabs.tt.gui.Origin.AT_MIDDLE);
            koth_status_label.setPos((viewer.getGUIRoot().getWidth() - koth_status_label.getWidth()) / 2, 10);
            viewer.getGUIRoot().addChild(koth_status_label);
            onWorldStart(viewer.getWorld(), new java.util.function.Consumer_L((object a0) => { koth_status_label.setText(((string)a0)); }));
        }
        // Java: 136
        public void onWorldStart(com.oddlabs.tt.landscape.World world, java.util.function.Consumer status)
        {
            winning_team = com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL;
            holding_team = com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL;
            hold_time = 0.0f;
            int center = world.getHeightMap().getGridUnitsPerWorld() / 2;
            center_x = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(center);
            center_y = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(center);
            new com.oddlabs.tt.model.SceneryModel(world, center_x, center_y, 0.0f, 1.0f, world.getRacesResources().getTreasures()[com.oddlabs.tt.gamemode.koth.KingOfTheHillModeRules.TREASURE_INDEX], 2.6f, false, "King of the Hill");
            world.getAnimationManagerGameTime().registerAnimation(new KingOfTheHillModeRules.Anon1_Animated(this, world, center, status));
            com.oddlabs.tt.gamemode.ObjectiveDirector.hold(world, new float[] { center_x }, new float[] { center_y }, com.oddlabs.tt.gamemode.koth.KingOfTheHillModeRules.CAPTURE_RADIUS, 1, new java.util.function.IntUnaryOperator_L((int i) =>
            {
                return holding_team;
            }), new java.util.function.IntSupplier_L(() =>
            {
                return holding_team;
            }));
        }
        public string last_status;
        // Java: 225
        public void show(java.util.function.Consumer status, string text)
        {
            if (status != null && !java.lang.JO.equals(text, last_status))
            {
                status.accept(text);
            }
            last_status = text;
        }
        // Java: 231
        public string describe()
        {
            return last_status;
        }
        static KingOfTheHillModeRules()
        {
            KingOfTheHillModeRules.OPTIONS = java.util.List_.of(new com.oddlabs.matchmaking.GameModeOption(com.oddlabs.tt.gamemode.koth.KingOfTheHillModeRules.OPTION_CAPTURE_RADIUS, com.oddlabs.matchmaking.GameModeOption.Type.FLOAT, com.oddlabs.tt.gamemode.koth.KingOfTheHillModeRules.CAPTURE_RADIUS, "kingofthehill_capture_radius", 5.0f, 40.0f, null), new com.oddlabs.matchmaking.GameModeOption(com.oddlabs.tt.gamemode.koth.KingOfTheHillModeRules.OPTION_HOLD_SECONDS, com.oddlabs.matchmaking.GameModeOption.Type.FLOAT, com.oddlabs.tt.gamemode.koth.KingOfTheHillModeRules.HOLD_SECONDS_TO_WIN, "kingofthehill_hold_seconds", 30.0f, 600.0f, null));
        }
        private void init_KingOfTheHillModeRules()
        {
            this.winning_team = com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL;
            this.holding_team = com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL;
            this.hold_time = 0.0f;
            this.center_x = 0.0f;
            this.center_y = 0.0f;
            this.last_status = "";
        }
        public KingOfTheHillModeRules() : base()
        {
            init_KingOfTheHillModeRules();
        }
        // Java: anonymous class at line 165
        public sealed class Anon1_Animated : java.lang.Object, com.oddlabs.tt.animation.Animated
        {
            public com.oddlabs.tt.gamemode.koth.KingOfTheHillModeRules this_0;
            public com.oddlabs.tt.landscape.World world;
            public int center;
            public java.util.function.Consumer status;
            public Anon1_Animated(com.oddlabs.tt.gamemode.koth.KingOfTheHillModeRules this_0_, com.oddlabs.tt.landscape.World world_c, int center_c, java.util.function.Consumer status_c)
            {
                this.this_0 = this_0_;
                this.world = world_c;
                this.center = center_c;
                this.status = status_c;
            }
            // Java: 166
            public void animate(float t)
            {
                if (this_0.winning_team != com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL)
                {
                    return;
                }
                com.oddlabs.tt.pathfinder.FindOccupantFilter filter = new com.oddlabs.tt.pathfinder.FindOccupantFilter(this_0.center_x, this_0.center_y, com.oddlabs.tt.gamemode.koth.KingOfTheHillModeRules.CAPTURE_RADIUS, null, typeof(com.oddlabs.tt.model.Unit));
                world.getUnitGrid().scan(filter, center, center);
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
                int contesting_team = (team_presence.size() == 1 ? ((int)((int?)team_presence.keySet().iterator().next())) : com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL);
                if (contesting_team != com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL && contesting_team == this_0.holding_team)
                {
                    this_0.hold_time += t;
                    if (this_0.hold_time >= com.oddlabs.tt.gamemode.koth.KingOfTheHillModeRules.HOLD_SECONDS_TO_WIN)
                    {
                        this_0.winning_team = this_0.holding_team;
                    }
                }
                else
                {
                    this_0.holding_team = contesting_team;
                    this_0.hold_time = 0.0f;
                }
                if (this_0.winning_team != com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL)
                {
                    this_0.show(status, "Team " + ((this_0.winning_team + 1)) + " has won the Hill!");
                }
                else if (this_0.holding_team != com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL)
                {
                    int seconds_left = java.lang.JC.d2i(java.lang.Math.ceil((float)(com.oddlabs.tt.gamemode.koth.KingOfTheHillModeRules.HOLD_SECONDS_TO_WIN - this_0.hold_time)));
                    this_0.show(status, "Team " + ((this_0.holding_team + 1)) + " leads the Hill - " + (seconds_left) + "s to win");
                }
                else if (team_presence.isEmpty())
                {
                    this_0.show(status, "The Hill is uncontrolled");
                }
                else
                {
                    this_0.show(status, "The Hill is contested!");
                }
            }
            // mixin of default method Animated.updateChecksum
            public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
            {
            }
        }
    }
}
