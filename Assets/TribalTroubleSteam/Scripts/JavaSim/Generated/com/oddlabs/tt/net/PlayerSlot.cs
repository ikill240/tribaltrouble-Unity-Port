// Translated from PlayerSlot.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.net
{
    // Java: 9
    public class PlayerSlot : java.lang.Object
    {
        public const long serialVersionUID = 1L;
        public const int AI_NONE = 0;
        public const int AI_EASY = 1;
        public const int AI_NORMAL = 2;
        public const int AI_HARD = 3;
        public const int AI_TOWER_TUTORIAL = 4;
        public const int AI_CHIEFTAIN_TUTORIAL = 5;
        public const int AI_BATTLE_TUTORIAL = 6;
        public const int AI_PASSIVE_CAMPAIGN = 7;
        public const int AI_NEUTRAL_CAMPAIGN = 8;
        public const int OPEN = 1;
        public const int CLOSED = 2;
        public const int HUMAN = 3;
        public const int AI = 4;
        public int slot;
        public int type;
        public int rating;
        public bool ready;
        public com.oddlabs.tt.player.PlayerInfo player_info;
        public com.oddlabs.matchmaking.TunnelAddress address;
        public int ai_difficulty;
        // Java: 41
        public static bool isValidType(int type)
        {
            return type == com.oddlabs.tt.net.PlayerSlot.HUMAN || type == com.oddlabs.tt.net.PlayerSlot.AI;
        }
        // Java: 45
        public void setRating(int rating)
        {
            this.rating = rating;
        }
        // Java: 49
        public void setType(int type)
        {
            this.type = type;
        }
        // Java: 53
        public void setAIDifficulty(int ai_difficulty)
        {
            this.ai_difficulty = ai_difficulty;
        }
        // Java: 57
        public void setAddress(com.oddlabs.matchmaking.TunnelAddress address)
        {
            this.address = address;
        }
        // Java: 61
        public void setReady(bool ready)
        {
            this.ready = ready;
        }
        // Java: 65
        public int getSlot()
        {
            return slot;
        }
        // Java: 69
        public void setInfo(com.oddlabs.tt.player.PlayerInfo player_info)
        {
            this.player_info = player_info;
        }
        // Java: 73
        public com.oddlabs.tt.player.PlayerInfo getInfo()
        {
            return player_info;
        }
        // Java: 77
        public bool isReady()
        {
            return ready;
        }
        // Java: 81
        public com.oddlabs.matchmaking.TunnelAddress getAddress()
        {
            return address;
        }
        // Java: 85
        public int getAIDifficulty()
        {
            return ai_difficulty;
        }
        // Java: 89
        public int getType()
        {
            return type;
        }
        // Java: 93
        public int getRating()
        {
            return rating;
        }
        private void init_PlayerSlot()
        {
            this.type = com.oddlabs.tt.net.PlayerSlot.OPEN;
            this.ai_difficulty = com.oddlabs.tt.net.PlayerSlot.AI_NONE;
        }
        public PlayerSlot(int slot) : base()
        {
            init_PlayerSlot();
            this.slot = slot;
        }
    }
}
