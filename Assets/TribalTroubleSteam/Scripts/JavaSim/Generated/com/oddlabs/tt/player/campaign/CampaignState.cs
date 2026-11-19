// Translated from CampaignState.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.campaign
{
    // Java: 8
    public class CampaignState : java.lang.Object
    {
        public const long serialVersionUID = 1L;
        public const int RACE_VIKINGS = 0;
        public const int RACE_NATIVES = 1;
        public const int DIFFICULTY_EASY = 1;
        public const int DIFFICULTY_NORMAL = 0;
        public const int DIFFICULTY_HARD = 2;
        public const int ISLAND_AVAILABLE = 1;
        public const int ISLAND_UNAVAILABLE = 2;
        public const int ISLAND_COMPLETED = 3;
        public const int ISLAND_SEMI_AVAILABLE = 4;
        public const int ISLAND_HIDDEN = 5;
        public int[] island_states;
        public int prev_island;
        public int current_island;
        public int num_peons;
        public int num_rock_warriors;
        public int num_iron_warriors;
        public int num_rubber_warriors;
        public bool has_rubber_weapons;
        public bool has_magic0;
        public bool has_magic1;
        public string name;
        public long date;
        public int race;
        public int difficulty;
        // Java: 48
        public void setIslandState(int index, int state)
        {
            if (island_states[index] == com.oddlabs.tt.player.campaign.CampaignState.ISLAND_SEMI_AVAILABLE && state == com.oddlabs.tt.player.campaign.CampaignState.ISLAND_SEMI_AVAILABLE)
            {
                island_states[index] = com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE;
            }
            else if (island_states[index] != com.oddlabs.tt.player.campaign.CampaignState.ISLAND_COMPLETED)
            {
                island_states[index] = state;
            }
        }
        // Java: 55
        public int getIslandState(int index)
        {
            return island_states[index];
        }
        // Java: 62
        public void setCurrentIsland(int current_island)
        {
            setPrevIsland(this.current_island);
            this.current_island = current_island;
        }
        // Java: 67
        public int getCurrentIsland()
        {
            return current_island;
        }
        // Java: 71
        public void setPrevIsland(int prev_island)
        {
            this.prev_island = prev_island;
        }
        // Java: 75
        public int getPrevIsland()
        {
            return prev_island;
        }
        // Java: 79
        public void setNumPeons(int num_peons)
        {
            this.num_peons = num_peons;
        }
        // Java: 83
        public int getNumPeons()
        {
            return num_peons;
        }
        // Java: 87
        public void setNumRockWarriors(int num_rock_warriors)
        {
            this.num_rock_warriors = num_rock_warriors;
        }
        // Java: 91
        public int getNumRockWarriors()
        {
            return num_rock_warriors;
        }
        // Java: 95
        public void setNumIronWarriors(int num_iron_warriors)
        {
            this.num_iron_warriors = num_iron_warriors;
        }
        // Java: 99
        public int getNumIronWarriors()
        {
            return num_iron_warriors;
        }
        // Java: 103
        public void setNumRubberWarriors(int num_rubber_warriors)
        {
            this.num_rubber_warriors = num_rubber_warriors;
        }
        // Java: 107
        public int getNumRubberWarriors()
        {
            return num_rubber_warriors;
        }
        // Java: 111
        public void setHasRubberWeapons(bool has_rubber_weapons)
        {
            this.has_rubber_weapons = has_rubber_weapons;
        }
        // Java: 115
        public bool hasRubberWeapons()
        {
            return has_rubber_weapons;
        }
        // Java: 119
        public void setHasMagic0(bool has_magic0)
        {
            this.has_magic0 = has_magic0;
        }
        // Java: 123
        public bool hasMagic0()
        {
            return has_magic0;
        }
        // Java: 127
        public void setHasMagic1(bool has_magic1)
        {
            this.has_magic1 = has_magic1;
        }
        // Java: 131
        public bool hasMagic1()
        {
            return has_magic1;
        }
        // Java: 135
        public string getName()
        {
            return name;
        }
        // Java: 139
        public void setName(string name)
        {
            this.name = name;
        }
        // Java: 143
        public long getDate()
        {
            return date;
        }
        // Java: 147
        public void setDate(long date)
        {
            this.date = date;
        }
        // Java: 151
        public void setRace(int race)
        {
            this.race = race;
        }
        // Java: 155
        public int getRace()
        {
            return race;
        }
        // Java: 159
        public void setDifficulty(int difficulty)
        {
            this.difficulty = difficulty;
        }
        // Java: 163
        public int getDifficulty()
        {
            return difficulty;
        }
        private void init_CampaignState()
        {
            this.current_island = (-1);
            this.num_peons = 10;
            this.num_rock_warriors = 0;
            this.num_iron_warriors = 0;
            this.num_rubber_warriors = 0;
            this.has_rubber_weapons = false;
            this.has_magic0 = false;
            this.has_magic1 = false;
        }
        public CampaignState(int[] initial_states) : base()
        {
            init_CampaignState();
            island_states = new int[initial_states.Length];
            java.lang.JSystem.arraycopy(initial_states, 0, island_states, 0, island_states.Length);
            name = "";
        }
    }
}
