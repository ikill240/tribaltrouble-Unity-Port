// Translated from GameOverTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.trigger
{
    // Java: 20
    public class GameOverTrigger : java.lang.Object, com.oddlabs.tt.animation.Animated
    {
        public int[] teams;
        public bool[] dead_tribes;
        public static java.util.ResourceBundle bundle;
        // Java: 26
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.trigger.GameOverTrigger.bundle, key, args);
        }
        public com.oddlabs.tt.viewer.WorldViewer viewer;
        // Java: 40
        public void animate(float t)
        {
            com.oddlabs.tt.player.Player[] players = viewer.getWorld().getPlayers();
            com.oddlabs.tt.player.Player local_player = viewer.getLocalPlayer();
            bool enemy_alive = false;
            for (int i = 0; i < players.Length; i++)
            {
                com.oddlabs.tt.player.Player current = players[i];
                if (!dead_tribes[i])
                {
                    if (!viewer.getPeerHub().isAlive(current))
                    {
                        if (((object)current == (object)local_player))
                        {
                            doGameOver(countTeams(players));
                            return;
                        }
                        else
                        {
                            dead_tribes[i] = true;
                            string defeat_message = i18n("defeat_message", current.getPlayerInfo().getName());
                            viewer.getPeerHub().receiveChat(com.oddlabs.tt.net.PeerHub.SYSTEM_NAME, defeat_message, false);
                        }
                    }
                    else if (local_player.isEnemy(current))
                    {
                        enemy_alive = true;
                    }
                }
            }
            if (!enemy_alive)
            {
                tryUnlockAchievements(local_player, players);
                doGameWon();
                return;
            }
            if (countTeams(players) < 2)
            {
                stop();
            }
        }
        // Java: 73
        public void tryUnlockAchievements(com.oddlabs.tt.player.Player local_player, com.oddlabs.tt.player.Player[] players)
        {
            if (com.oddlabs.tt.steam.SteamManager.getInstance() == null)
            {
                return;
            }
            if (viewer.getWorld().getGamespeed() != 4)
            {
                return;
            }
            int ai_team = (-1);
            bool is_player_alone = true;
            bool all_hards_same_team = true;
            int hard_ais_on_same_team = 0;
            int current_player_team = local_player.getPlayerInfo().getTeam();
            foreach (com.oddlabs.tt.player.Player current in players)
            {
                if (((object)current != (object)local_player) && current.getPlayerInfo().getTeam() == current_player_team)
                {
                    is_player_alone = false;
                    break;
                }
                if (((object)current != (object)local_player) && (current.getAI() is com.oddlabs.tt.player.AdvancedAI ai) && ai.getDifficulty() == com.oddlabs.tt.player.AdvancedAI.DIFFICULTY_HARD)
                {
                    if (ai_team == (-1))
                    {
                        ai_team = current.getPlayerInfo().getTeam();
                        hard_ais_on_same_team++;
                    }
                    else if (current.getPlayerInfo().getTeam() == ai_team)
                    {
                        hard_ais_on_same_team++;
                    }
                    else
                    {
                        all_hards_same_team = false;
                    }
                }
            }
            if (!is_player_alone || !all_hards_same_team)
            {
                return;
            }
            int map_size = viewer.getWorld().getMapSize();
            if (hard_ais_on_same_team >= 3 && map_size == com.oddlabs.matchmaking.Game.SIZE_SMALL)
            {
                com.oddlabs.tt.steam.SteamManager.unlockAchievement(com.oddlabs.tt.steam.SteamAchievementNames.BEAT_3_HARDS_ON_SMALL);
            }
            else if (hard_ais_on_same_team >= 5 && map_size == com.oddlabs.matchmaking.Game.SIZE_MEDIUM)
            {
                com.oddlabs.tt.steam.SteamManager.unlockAchievement(com.oddlabs.tt.steam.SteamAchievementNames.BEAT_5_HARDS_ON_MEDIUM);
            }
        }
        // Java: 114
        public int countTeams(com.oddlabs.tt.player.Player[] players)
        {
            for (int i = 0; i < players.Length; i++)
            {
                teams[i] = 0;
            }
            foreach (com.oddlabs.tt.player.Player current in players)
            {
                if (viewer.getPeerHub().isAlive(current) && current.getPlayerInfo().getTeam() != com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL)
                {
                    teams[current.getPlayerInfo().getTeam()]++;
                }
            }
            int team_count = 0;
            foreach (int team in teams)
            {
                if (team > 0)
                {
                    team_count++;
                }
            }
            return team_count;
        }
        // Java: 132
        public void disable()
        {
            viewer.getWorld().getAnimationManagerRealTime().removeAnimation(this);
        }
        // Java: 136
        public void createDelayTrigger(string text)
        {
            com.oddlabs.tt.gui.GUIRoot gui_root = viewer.getGUIRoot();
            new com.oddlabs.tt.trigger.GameOverDelayTrigger(viewer, gui_root.getDelegate().getCamera(), text);
        }
        // Java: 141
        public void doGameOver(int team_count)
        {
            viewer.getPeerHub().leaveGame();
            if (team_count < 2)
            {
                createDelayTrigger(i18n("you_defeated_game_over"));
            }
            else
            {
                createDelayTrigger(i18n("you_defeated"));
            }
            disable();
        }
        // Java: 151
        public void doGameWon()
        {
            viewer.getPeerHub().gameWon();
            createDelayTrigger(i18n("you_victorious"));
            disable();
        }
        // Java: 157
        public void stop()
        {
            createDelayTrigger(i18n("game_over"));
            disable();
        }
        static GameOverTrigger()
        {
            GameOverTrigger.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.trigger.GameOverTrigger).getName());
        }
        public GameOverTrigger(com.oddlabs.tt.viewer.WorldViewer viewer) : base()
        {
            this.viewer = viewer;
            viewer.getWorld().getAnimationManagerRealTime().registerAnimation(this);
            teams = new int[com.oddlabs.matchmaking.MatchmakingServerInterface.MAX_PLAYERS];
            dead_tribes = new bool[viewer.getWorld().getPlayers().Length];
            java.util.Arrays.fill(dead_tribes, false);
        }
        // mixin of default method Animated.updateChecksum
        public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
        }
    }
}
