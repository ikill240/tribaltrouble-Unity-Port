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
        public const int AI_EXPERT = 9;
        public const int AI_ULTRA = 10;
        public const int AI_FABLE = 11;
        public const int AI_GAUNTLET = 12;
        public static int[] MENU_DIFFICULTIES;
        // Java: 31
        public static int difficultyOfMenuEntry(int entry)
        {
            return com.oddlabs.tt.net.PlayerSlot.MENU_DIFFICULTIES[java.lang.Math.clamp(entry, 0, com.oddlabs.tt.net.PlayerSlot.MENU_DIFFICULTIES.Length - 1)];
        }
        // Java: 36
        public static int menuEntryOfDifficulty(int difficulty)
        {
            if (difficulty == com.oddlabs.tt.net.PlayerSlot.AI_ULTRA || difficulty == com.oddlabs.tt.net.PlayerSlot.AI_FABLE)
            {
                difficulty = com.oddlabs.tt.net.PlayerSlot.AI_GAUNTLET;
            }
            for (int i = 0; i < com.oddlabs.tt.net.PlayerSlot.MENU_DIFFICULTIES.Length; i++)
            {
                if (com.oddlabs.tt.net.PlayerSlot.MENU_DIFFICULTIES[i] == difficulty)
                {
                    return i;
                }
            }
            return 0;
        }
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
        // Java: 63
        public static bool isValidType(int type)
        {
            return type == com.oddlabs.tt.net.PlayerSlot.HUMAN || type == com.oddlabs.tt.net.PlayerSlot.AI;
        }
        // Java: 67
        public void setRating(int rating)
        {
            this.rating = rating;
        }
        // Java: 71
        public void setType(int type)
        {
            this.type = type;
        }
        // Java: 75
        public void setAIDifficulty(int ai_difficulty)
        {
            this.ai_difficulty = ai_difficulty;
        }
        // Java: 79
        public void setAddress(com.oddlabs.matchmaking.TunnelAddress address)
        {
            this.address = address;
        }
        // Java: 83
        public void setReady(bool ready)
        {
            this.ready = ready;
        }
        // Java: 92
        public int getSlot()
        {
            return slot;
        }
        // Java: 96
        public void setInfo(com.oddlabs.tt.player.PlayerInfo player_info)
        {
            this.player_info = player_info;
        }
        // Java: 100
        public com.oddlabs.tt.player.PlayerInfo getInfo()
        {
            return player_info;
        }
        // Java: 104
        public bool isReady()
        {
            return ready;
        }
        // Java: 108
        public com.oddlabs.matchmaking.TunnelAddress getAddress()
        {
            return address;
        }
        // Java: 112
        public int getAIDifficulty()
        {
            return ai_difficulty;
        }
        // Java: 116
        public int getType()
        {
            return type;
        }
        // Java: 120
        public int getRating()
        {
            return rating;
        }
        static PlayerSlot()
        {
            PlayerSlot.MENU_DIFFICULTIES = new int[] { com.oddlabs.tt.net.PlayerSlot.AI_EASY, com.oddlabs.tt.net.PlayerSlot.AI_NORMAL, com.oddlabs.tt.net.PlayerSlot.AI_HARD, com.oddlabs.tt.net.PlayerSlot.AI_EXPERT, com.oddlabs.tt.net.PlayerSlot.AI_GAUNTLET };
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
