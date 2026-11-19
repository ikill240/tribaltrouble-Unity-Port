// Bridge from the translated game/network code to the Unity host for the real Client/Server path.
#pragma warning disable
namespace TribalTroubleSteam.Bridge
{
    public interface INetHost
    {
        /// <summary>Menu.completeGameSetupHack: the world exists, its players are set up, the game view takes over.</summary>
        void WorldReady(com.oddlabs.tt.viewer.WorldViewer viewer);
        /// <summary>The connection to the game server (or the local loopback server) was lost.</summary>
        void ConnectionLost();
        /// <summary>ProgressForm.setProgressForm: run 'load' (the world generation) when ready - Java runs it right after the progress screen fades in.</summary>
        void Load(java.lang.Runnable load);
        /// <summary>ProgressForm.progress during a load (the network was just ticked).</summary>
        void LoadProgress();
        /// <summary>Multiplayer: connected to the game host (ConnectingForm.connected -> the lobby, GameMenu).</summary>
        void LobbyConnected(com.oddlabs.tt.net.Client client, com.oddlabs.matchmaking.Game game, com.oddlabs.tt.resource.WorldGenerator generator, int player_slot, int player_count);
        /// <summary>The lobby's slot list changed (GameMenu.setPlayers).</summary>
        void LobbyPlayers(com.oddlabs.tt.net.PlayerSlot[] players);
        /// <summary>The host started the game: world loading follows.</summary>
        void LobbyGameStarted();
        /// <summary>Menu.startNewGame / joinGame created a game (also when Java code starts one, e.g. the next tutorial).</summary>
        void GameCreated(com.oddlabs.tt.net.GameNetwork game);
    }

    public static class NetHub
    {
        public static INetHost Host;
    }

    /// <summary>Stands in for ConnectingForm (and then GameMenu, the lobby) as the Client's ConfigurationListener.</summary>
    public sealed class StartingListener : java.lang.Object, com.oddlabs.tt.net.ConfigurationListener
    {
        readonly bool multiplayer;
        public StartingListener(bool multiplayer) { this.multiplayer = multiplayer; }

        public void connected(com.oddlabs.tt.net.Client client, com.oddlabs.matchmaking.Game game, com.oddlabs.tt.resource.WorldGenerator generator, int player_slot, int player_count)
        {
            if (!multiplayer) return;
            // ConnectingForm.connected: random race, team = slot (rated: slot % 2), not ready yet
            var random = new java.util.Random(com.oddlabs.tt.@event.LocalEventQueue.getQueue().getHighPrecisionManager().getTick());
            random.nextFloat();
            int race = (int)(random.nextFloat() * (com.oddlabs.tt.model.RacesResources.getNumRaces() - 1) + .5f);
            int team = player_slot;
            if (game.isRated()) team = player_slot % 2;
            client.getServerInterface().setPlayerSlot(player_slot, com.oddlabs.tt.net.PlayerSlot.HUMAN, race, team, false, com.oddlabs.tt.net.PlayerSlot.AI_NONE);
            NetHub.Host?.LobbyConnected(client, game, generator, player_slot, player_count);
        }
        public void setPlayers(com.oddlabs.tt.net.PlayerSlot[] players) => NetHub.Host?.LobbyPlayers(players);
        public void gameStarted() => NetHub.Host?.LobbyGameStarted();
        public void connectionLost() => NetHub.Host?.ConnectionLost();
    }
}

namespace TribalTroubleSteam.Bridge
{
    /// <summary>What the game would play / show (AudioManager, NotificationManager), for the Unity presentation. Any thread.</summary>
    public static class SimHooks
    {
        public static System.Action<com.oddlabs.tt.audio.AudioParameters> Sound;
        public static System.Action<com.oddlabs.tt.model.Selectable> Attacked;
        public static System.Action<com.oddlabs.tt.model.Selectable> NewSelectable;
        public static System.Action<com.oddlabs.tt.player.Player, float, float> Beacon;
    }
}

namespace TribalTroubleSteam.Bridge
{
    /// <summary>
    /// The state of the Unity GUI and camera that the Java code inspects (the tutorial triggers look at the camera,
    /// the action panel's menus, which delegate is active, ...). The Unity controller writes it every frame.
    /// </summary>
    public static class UiState
    {
        public static float scrollX, scrollY, lastZoomFactor;
        public static bool rotateLeft, rotateRight, pitchUp, pitchDown;
        public static bool mapMode, firstPerson, placing;
        /// <summary>Spectating (SelectionDelegate.setObserverMode): watch only, no orders.</summary>
        public static bool observer;
        public static bool inArmyMenu, inBuildMenu, inHarvestMenu;

        /// <summary>The tutorial's instruction box (Tutorial.next1 / removeInfo); null = hidden.</summary>
        public static string tutorialText;
        /// <summary>Status labels the game code added to the screen (game-mode HUDs), drawn top-centre by Unity.</summary>
        public static readonly System.Collections.Generic.List<com.oddlabs.tt.gui.Label> hudLabels = new System.Collections.Generic.List<com.oddlabs.tt.gui.Label>();
        /// <summary>Tutorial finished (TutorialOverDelegate): number, and the choice (next / restart number, or -1 for the main menu).</summary>
        public static System.Action<int, System.Action<int>> TutorialDone;
        /// <summary>Renderer.startMenu: the game ended and Java returns to the main menu.</summary>
        public static System.Action ReturnToMenu;

        public static void Reset()
        {
            scrollX = scrollY = lastZoomFactor = 0;
            rotateLeft = rotateRight = pitchUp = pitchDown = mapMode = firstPerson = placing = observer = false;
            inArmyMenu = inBuildMenu = inHarvestMenu = false;
            tutorialText = null;
            hudLabels.Clear();
        }
    }
}

namespace TribalTroubleSteam.Bridge
{
    /// <summary>Steam for the translated SteamManager (implemented by the Unity host with Steamworks.NET).</summary>
    public interface ISteam
    {
        long AccountId { get; }
        string PersonaName { get; }
        int AppId { get; }
        void RequestWebApiTicket(string identity);
        /// <summary>The ticket when ready, else null (pumps Steam callbacks).</summary>
        sbyte[] WebApiTicket(int waitMs);
        void CancelTicket();
        void SetRichPresence(string key, string value);
        void ClearRichPresence();
        void UnlockAchievement(string id);
        void RunCallbacks();
    }

    /// <summary>Online hooks: guest login, Steam.</summary>
    public static class OnlineHooks
    {
        /// <summary>The next matchmaking login is a guest login (Renderer.isRegistered() == false).</summary>
        public static volatile bool Guest;
        /// <summary>Steam (Steamworks.NET, app 3945720) when the Steam client runs and the game is owned, else null.</summary>
        public static volatile ISteam Steam;
        /// <summary>Starts Steam (set by the TribalTroubleSteam.Steamworks assembly; SteamGame calls it when the game starts).</summary>
        public static System.Action StartSteam;
    }
}
