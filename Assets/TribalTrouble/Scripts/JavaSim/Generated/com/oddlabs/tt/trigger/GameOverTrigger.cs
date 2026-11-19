// Translated from GameOverTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.trigger
{
    // Java: 21
    public class GameOverTrigger : java.lang.Object, com.oddlabs.tt.animation.Animated
    {
        public int[] teams;
        public bool[] dead_tribes;
        public static java.util.ResourceBundle bundle;
        // Java: 27
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.trigger.GameOverTrigger.bundle, key, args);
        }
        public com.oddlabs.tt.viewer.WorldViewer viewer;
        public static bool isHomeStay(com.oddlabs.tt.viewer.WorldViewer viewer)
        {
            if ((object)viewer.getWorld().getGameMode() != (object)com.oddlabs.matchmaking.GameMode.ISLAND_WARS)
            {
                return false;
            }
            com.oddlabs.tt.islandwars.IslandWarsSetup s = com.oddlabs.tt.islandwars.IslandWarsSetup.decode(viewer.getWorld().getIslandWars());
            return s != null && s.kind == com.oddlabs.tt.islandwars.IslandWarsSetup.HOME;
        }
        // Java: 41
        public void animate(float t)
        {
            com.oddlabs.tt.player.Player[] players = viewer.getWorld().getPlayers();
            com.oddlabs.tt.player.Player local_player = viewer.getLocalPlayer();
            bool enemy_alive = false;
            bool team_alive = false;
            for (int i = 0; i < players.Length; i++)
            {
                com.oddlabs.tt.player.Player current = players[i];
                if (!dead_tribes[i])
                {
                    if (!current.isAlive())
                    {
                        dead_tribes[i] = true;
                        if (((object)current != (object)local_player))
                        {
                            string defeat_message = i18n("defeat_message", current.getPlayerInfo().getName());
                            viewer.getPeerHub().receiveChat(com.oddlabs.tt.net.PeerHub.SYSTEM_NAME, defeat_message, false);
                        }
                    }
                    else if (local_player.isEnemy(current))
                    {
                        enemy_alive = true;
                    }
                    else
                    {
                        team_alive = true;
                    }
                }
            }
            if (!team_alive)
            {
                doGameOver(countTeams(players));
                return;
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
        // Java: 106
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
                if (((object)current != (object)local_player) && (current.getAI() is com.oddlabs.tt.player.AdvancedAI ai) && !ai.isAdaptive() && ai.getDifficulty() == com.oddlabs.tt.player.AdvancedAI.DIFFICULTY_HARD)
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
        // Java: 148
        public int countTeams(com.oddlabs.tt.player.Player[] players)
        {
            for (int i = 0; i < players.Length; i++)
            {
                teams[i] = 0;
            }
            foreach (com.oddlabs.tt.player.Player current in players)
            {
                if (current.isAlive() && current.getPlayerInfo().getTeam() != com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL)
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
        // Java: 169
        public void disable()
        {
            viewer.getWorld().getAnimationManagerRealTime().removeAnimation(this);
        }
        // Java: 173
        public void createDelayTrigger(string text)
        {
            com.oddlabs.tt.gui.GUIRoot gui_root = viewer.getGUIRoot();
            new com.oddlabs.tt.trigger.GameOverDelayTrigger(viewer, gui_root.getDelegate().getCamera(), text);
        }
        // Java: 178
        public void doGameOver(int team_count)
        {
            recordAdaptiveAiResult(false);
            com.oddlabs.tt.islandwars.IslandWarsClient.gameEnded(viewer, false);
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
        // Java: 190
        public void doGameWon()
        {
            recordAdaptiveAiResult(true);
            com.oddlabs.tt.islandwars.IslandWarsClient.gameEnded(viewer, true);
            viewer.getPeerHub().gameWon();
            createDelayTrigger(i18n("you_victorious"));
            disable();
        }
        // Java: 205
        public void recordAdaptiveAiResult(bool player_won)
        {
            bool skill_rating_recorded = false;
            foreach (com.oddlabs.tt.player.Player current in viewer.getWorld().getPlayers())
            {
                if ((current.getAI() is com.oddlabs.tt.player.AdvancedAI ai) && ai.isAdaptive())
                {
                    if (!skill_rating_recorded)
                    {
                        com.oddlabs.tt.player.AdaptiveAIProfile.get().recordMatchResult(player_won);
                        skill_rating_recorded = true;
                    }
                    ai.recordBanditOutcome(!player_won);
                    com.oddlabs.tt.player.AdaptiveAIProfile.get().recordTierNeeded(ai.getStartDifficulty(), ai.getTopTier(), !player_won);
                    ai.finalizeAdaptiveLearning();
                }
            }
        }
        // Java: 223
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
            // an Island Wars stay at home has nobody to beat: it ends when the player leaves (GameOverTrigger.java)
            if (isHomeStay(viewer))
            {
                teams = new int[com.oddlabs.matchmaking.MatchmakingServerInterface.MAX_PLAYERS];
                dead_tribes = new bool[viewer.getWorld().getPlayers().Length];
                return;
            }
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
