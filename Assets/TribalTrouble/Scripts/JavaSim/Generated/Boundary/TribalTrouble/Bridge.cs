// Hand-written: the seam between the translated game code (campaign islands, triggers, the Java GUI calls they make)
// and the Unity game. The Unity side sets Bridge.Host; the boundary classes below forward to it.
#pragma warning disable
namespace TribalTrouble.Bridge
{
    /// <summary>One game the Java code asked for (Menu.startNewGame + the player slots and unit infos it then set up).</summary>
    public sealed class GameRequest
    {
        public com.oddlabs.tt.landscape.WorldParameters worldParams;
        public com.oddlabs.tt.viewer.InGameInfo ingameInfo;
        public com.oddlabs.tt.net.WorldInitAction initAction;
        public int meters;
        public com.oddlabs.tt.procedural.Landscape.TerrainType terrain;
        public float hills, vegetation, supplies;
        public int seed;
        public bool archipelago;
        public string[] aiNames;
        public readonly System.Collections.Generic.SortedDictionary<int, Slot> slots = new System.Collections.Generic.SortedDictionary<int, Slot>();
        public readonly System.Collections.Generic.Dictionary<int, com.oddlabs.tt.player.UnitInfo> unitInfos = new System.Collections.Generic.Dictionary<int, com.oddlabs.tt.player.UnitInfo>();

        public sealed class Slot { public int type, race, team, aiDifficulty; public bool ready; }
    }

    public interface IHost
    {
        /// <summary>GameServerInterface.startServer: build the world for this request, then run its WorldInitAction.</summary>
        void StartGame(GameRequest request);
        void ShowDialog(string header, string text, int face, bool inGame, bool cancel, java.lang.Runnable onOk);
        void ShowMessage(string text);
        void CameraJump(float x, float y, float metersPerSecond, float maxSeconds, java.lang.Runnable done);
        void CameraReset(float x, float y);
        void CameraSetPos(float x, float y);
        /// <summary>GameStatsDelegate: the game is over (label is the result text).</summary>
        void GameOver(com.oddlabs.tt.viewer.WorldViewer viewer, string label);
        void ShowCampaignMap(com.oddlabs.tt.player.campaign.Campaign campaign);
        void AddCounter(com.oddlabs.tt.gui.CounterLabel counter);
        void RemoveCounter(com.oddlabs.tt.gui.CounterLabel counter);
        com.oddlabs.tt.player.campaign.CampaignState[] LoadSavegames();
        void SaveSavegames(com.oddlabs.tt.player.campaign.CampaignState[] states);
        void Chat(string from, string text);
        void Respond(object target);
    }

    public static class Hub
    {
        public static IHost Host;
    }
}
