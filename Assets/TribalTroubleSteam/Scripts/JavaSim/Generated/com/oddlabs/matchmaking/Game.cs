// Translated from Game.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchmaking
{
    // Java: 8
    public class Game : java.lang.Object
    {
        public const long serialVersionUID = 3L;
        public const int SIZE_NONE = (-1);
        public const int SIZE_SMALL = 0;
        public const int SIZE_MEDIUM = 1;
        public const int SIZE_LARGE = 2;
        public const int SIZE_ENORMOUS = 3;
        public const int SIZE_ARCHIPELAGO = 4;
        public const int TERRAIN_TYPE_NATIVE = 0;
        public const int TERRAIN_TYPE_VIKING = 1;
        public const int GAMESPEED_PAUSE = 0;
        public const int GAMESPEED_SLOW = 1;
        public const int GAMESPEED_NORMAL = 2;
        public const int GAMESPEED_FAST = 3;
        public const int GAMESPEED_LUDICROUS = 4;
        public const int MIN_LENGTH = 2;
        public const int MAX_LENGTH = 30;
        public const int DEFAULT_INITIAL_UNIT_COUNT = 20;
        public const int DEFAULT_MAX_UNIT_COUNT = 250;
        public const int DEFAULT_MAX_BUILDING_COUNT = 20;
        public string game_name;
        public sbyte size;
        public sbyte terrain;
        public sbyte hills;
        public sbyte trees;
        public sbyte supplies;
        public bool rated;
        public sbyte gamespeed;
        public string mapcode;
        public float random_start_pos;
        public int max_unit_count;
        public int initial_unit_count;
        public int max_building_count;
        public bool ships;
        public bool night;
        public int database_id;
        // Java: 73
        public static com.oddlabs.matchmaking.Game.Builder builder()
        {
            return new com.oddlabs.matchmaking.Game.Builder();
        }
        // Java: 77
        public bool isValid()
        {
            return game_name != null && game_name.length() >= com.oddlabs.matchmaking.Game.MIN_LENGTH && game_name.length() <= com.oddlabs.matchmaking.Game.MAX_LENGTH;
        }
        // Java: 81
        public bool isValidGuestGame()
        {
            return true;
        }
        // Java: 85
        public string getName()
        {
            return game_name;
        }
        // Java: 89
        public sbyte getSize()
        {
            return size;
        }
        // Java: 93
        public sbyte getTerrainType()
        {
            return terrain;
        }
        // Java: 97
        public sbyte getHills()
        {
            return hills;
        }
        // Java: 101
        public sbyte getTrees()
        {
            return trees;
        }
        // Java: 105
        public sbyte getSupplies()
        {
            return supplies;
        }
        // Java: 109
        public bool isRated()
        {
            return rated;
        }
        // Java: 113
        public sbyte getGamespeed()
        {
            return gamespeed;
        }
        // Java: 117
        public string getMapcode()
        {
            return mapcode;
        }
        // Java: 121
        public float getRandomStartPos()
        {
            return random_start_pos;
        }
        // Java: 125
        public int getMaxUnitCount()
        {
            return max_unit_count;
        }
        // Java: 129
        public int getInitialUnitCount()
        {
            return initial_unit_count;
        }
        // Java: 133
        public int getMaxBuildingCount()
        {
            return max_building_count;
        }
        // Java: 137
        public bool isShips()
        {
            return ships;
        }
        // Java: 141
        public bool isNight()
        {
            return night;
        }
        // Java: 145
        public void setDatabaseID(int database_id)
        {
            this.database_id = database_id;
        }
        // Java: 149
        public int getDatabaseID()
        {
            return database_id;
        }
        // Java: 153
        public class Builder : java.lang.Object
        {
            public string game_name;
            public sbyte size_f;
            public sbyte terrain_f;
            public sbyte hills_f;
            public sbyte trees_f;
            public sbyte supplies_f;
            public bool rated_f;
            public sbyte gamespeed_f;
            public string mapcode_f;
            public float random_start_pos;
            public int max_unit_count;
            public int initial_unit_count;
            public int max_building_count;
            public bool ships_f;
            public bool night_f;
            // Java: 173
            public com.oddlabs.matchmaking.Game.Builder name(string name_v)
            {
                this.game_name = name_v;
                return this;
            }
            // Java: 178
            public com.oddlabs.matchmaking.Game.Builder size(sbyte size_v)
            {
                this.size_f = size_v;
                return this;
            }
            // Java: 183
            public com.oddlabs.matchmaking.Game.Builder terrain(sbyte terrain_v)
            {
                this.terrain_f = terrain_v;
                return this;
            }
            // Java: 188
            public com.oddlabs.matchmaking.Game.Builder hills(sbyte hills_v)
            {
                this.hills_f = hills_v;
                return this;
            }
            // Java: 193
            public com.oddlabs.matchmaking.Game.Builder trees(sbyte trees_v)
            {
                this.trees_f = trees_v;
                return this;
            }
            // Java: 198
            public com.oddlabs.matchmaking.Game.Builder supplies(sbyte supplies_v)
            {
                this.supplies_f = supplies_v;
                return this;
            }
            // Java: 203
            public com.oddlabs.matchmaking.Game.Builder rated(bool rated_v)
            {
                this.rated_f = rated_v;
                return this;
            }
            // Java: 208
            public com.oddlabs.matchmaking.Game.Builder gamespeed(sbyte gamespeed_v)
            {
                this.gamespeed_f = gamespeed_v;
                return this;
            }
            // Java: 213
            public com.oddlabs.matchmaking.Game.Builder mapcode(string mapcode_v)
            {
                this.mapcode_f = mapcode_v;
                return this;
            }
            // Java: 218
            public com.oddlabs.matchmaking.Game.Builder randomStartPos(float random_start_pos)
            {
                this.random_start_pos = random_start_pos;
                return this;
            }
            // Java: 223
            public com.oddlabs.matchmaking.Game.Builder maxUnitCount(int max_unit_count)
            {
                this.max_unit_count = max_unit_count;
                return this;
            }
            // Java: 228
            public com.oddlabs.matchmaking.Game.Builder initialUnitCount(int initial_unit_count)
            {
                this.initial_unit_count = initial_unit_count;
                return this;
            }
            // Java: 233
            public com.oddlabs.matchmaking.Game.Builder maxBuildingCount(int max_building_count)
            {
                this.max_building_count = max_building_count;
                return this;
            }
            // Java: 238
            public com.oddlabs.matchmaking.Game.Builder ships(bool ships_v)
            {
                this.ships_f = ships_v;
                return this;
            }
            // Java: 243
            public com.oddlabs.matchmaking.Game.Builder night(bool night_v)
            {
                this.night_f = night_v;
                return this;
            }
            // Java: 248
            public com.oddlabs.matchmaking.Game build()
            {
                return new com.oddlabs.matchmaking.Game(this);
            }
            private void init_Builder()
            {
                this.game_name = "";
                this.max_unit_count = com.oddlabs.matchmaking.Game.DEFAULT_MAX_UNIT_COUNT;
                this.initial_unit_count = com.oddlabs.matchmaking.Game.DEFAULT_INITIAL_UNIT_COUNT;
                this.max_building_count = com.oddlabs.matchmaking.Game.DEFAULT_MAX_BUILDING_COUNT;
            }
            public Builder() : base()
            {
                init_Builder();
            }
        }
        public Game(com.oddlabs.matchmaking.Game.Builder b) : base()
        {
            this.game_name = b.game_name;
            this.size = b.size_f;
            this.terrain = b.terrain_f;
            this.hills = b.hills_f;
            this.trees = b.trees_f;
            this.supplies = b.supplies_f;
            this.rated = b.rated_f;
            this.gamespeed = b.gamespeed_f;
            this.mapcode = b.mapcode_f;
            this.random_start_pos = b.random_start_pos;
            this.max_unit_count = b.max_unit_count;
            this.initial_unit_count = b.initial_unit_count;
            this.max_building_count = b.max_building_count;
            this.ships = b.ships_f;
            this.night = b.night_f;
        }
    }
}
