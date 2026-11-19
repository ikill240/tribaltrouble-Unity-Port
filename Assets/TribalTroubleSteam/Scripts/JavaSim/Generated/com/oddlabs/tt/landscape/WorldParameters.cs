// Translated from WorldParameters.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.landscape
{
    // Java: 9
    public class WorldParameters : java.lang.Object
    {
        public const long serialVersionUID = 1L;
        public string map_code;
        public int initial_unit_count;
        public int max_unit_count;
        public int initial_game_speed;
        public int map_size;
        public int max_building_count;
        public bool ships;
        public bool night;
        // Java: 43
        public static com.oddlabs.tt.landscape.WorldParameters.Builder builder()
        {
            return new com.oddlabs.tt.landscape.WorldParameters.Builder();
        }
        // Java: 47
        public string getMapcode()
        {
            return map_code;
        }
        // Java: 51
        public int getInitialUnitCount()
        {
            return initial_unit_count;
        }
        // Java: 55
        public int getMaxUnitCount()
        {
            return max_unit_count;
        }
        // Java: 59
        public int getInitialGameSpeed()
        {
            return initial_game_speed;
        }
        // Java: 63
        public int getMapSize()
        {
            return map_size;
        }
        // Java: 67
        public int getMaxBuildingCount()
        {
            return max_building_count;
        }
        // Java: 71
        public bool isShipsEnabled()
        {
            return ships;
        }
        // Java: 75
        public bool isNight()
        {
            return night;
        }
        // Java: 79
        public class Builder : java.lang.Object
        {
            public string mapcode_f;
            public int initial_unit_count;
            public int max_unit_count;
            public int initial_game_speed;
            public int map_size;
            public int max_building_count;
            public bool ships_f;
            public bool night_f;
            // Java: 92
            public com.oddlabs.tt.landscape.WorldParameters.Builder mapcode(string mapcode_v)
            {
                this.mapcode_f = mapcode_v;
                return this;
            }
            // Java: 97
            public com.oddlabs.tt.landscape.WorldParameters.Builder initialUnitCount(int v)
            {
                this.initial_unit_count = v;
                return this;
            }
            // Java: 102
            public com.oddlabs.tt.landscape.WorldParameters.Builder maxUnitCount(int v)
            {
                this.max_unit_count = v;
                return this;
            }
            // Java: 107
            public com.oddlabs.tt.landscape.WorldParameters.Builder initialGameSpeed(int v)
            {
                this.initial_game_speed = v;
                return this;
            }
            // Java: 112
            public com.oddlabs.tt.landscape.WorldParameters.Builder mapSize(int v)
            {
                this.map_size = v;
                return this;
            }
            // Java: 117
            public com.oddlabs.tt.landscape.WorldParameters.Builder maxBuildingCount(int v)
            {
                this.max_building_count = v;
                return this;
            }
            // Java: 122
            public com.oddlabs.tt.landscape.WorldParameters.Builder ships(bool v)
            {
                this.ships_f = v;
                return this;
            }
            // Java: 127
            public com.oddlabs.tt.landscape.WorldParameters.Builder night(bool v)
            {
                this.night_f = v;
                return this;
            }
            // Java: 132
            public com.oddlabs.tt.landscape.WorldParameters build()
            {
                return new com.oddlabs.tt.landscape.WorldParameters(this);
            }
            private void init_Builder()
            {
                this.mapcode_f = "";
                this.map_size = com.oddlabs.matchmaking.Game.SIZE_NONE;
                this.max_building_count = com.oddlabs.matchmaking.Game.DEFAULT_MAX_BUILDING_COUNT;
            }
            public Builder() : base()
            {
                init_Builder();
            }
        }
        public WorldParameters(int initial_game_speed, string map_code, int initial_unit_count, int max_unit_count) : this(initial_game_speed, map_code, initial_unit_count, max_unit_count, com.oddlabs.matchmaking.Game.SIZE_NONE)
        {
        }
        public WorldParameters(int initial_game_speed, string map_code, int initial_unit_count, int max_unit_count, int map_size) : this(com.oddlabs.tt.landscape.WorldParameters.builder().initialGameSpeed(initial_game_speed).mapcode(map_code).initialUnitCount(initial_unit_count).maxUnitCount(max_unit_count).mapSize(map_size))
        {
        }
        public WorldParameters(com.oddlabs.tt.landscape.WorldParameters.Builder b) : base()
        {
            this.map_code = b.mapcode_f;
            this.initial_unit_count = b.initial_unit_count;
            this.max_unit_count = b.max_unit_count;
            this.initial_game_speed = b.initial_game_speed;
            this.map_size = b.map_size;
            this.max_building_count = b.max_building_count;
            this.ships = b.ships_f;
            this.night = b.night_f;
        }
    }
}
