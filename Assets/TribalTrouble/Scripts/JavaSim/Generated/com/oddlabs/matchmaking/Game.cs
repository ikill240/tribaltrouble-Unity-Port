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
        public const int SIZE_UNREAL = 4;
        public const int SIZE_ARCHIPELAGO = 5;
        public const int TERRAIN_TYPE_NATIVE = 0;
        public const int TERRAIN_TYPE_VIKING = 1;
        public const int TERRAIN_TYPE_ELEMENTAL = 2;
        public const int GAMESPEED_PAUSE = 0;
        public const int GAMESPEED_SLOW = 1;
        public const int GAMESPEED_NORMAL = 2;
        public const int GAMESPEED_FAST = 3;
        public const int GAMESPEED_LUDICROUS = 4;
        public const int MIN_LENGTH = 2;
        public const int MAX_LENGTH = 30;
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
        public int database_id;
        // Java: 62
        public static com.oddlabs.matchmaking.Game.Builder builder()
        {
            return new com.oddlabs.matchmaking.Game.Builder();
        }
        // Java: 66
        public bool isValid()
        {
            return game_name != null && game_name.length() >= com.oddlabs.matchmaking.Game.MIN_LENGTH && game_name.length() <= com.oddlabs.matchmaking.Game.MAX_LENGTH;
        }
        // Java: 70
        public bool isValidGuestGame()
        {
            return true;
        }
        // Java: 74
        public string getName()
        {
            return game_name;
        }
        // Java: 78
        public sbyte getSize()
        {
            return size;
        }
        // Java: 82
        public sbyte getTerrainType()
        {
            return terrain;
        }
        // Java: 86
        public sbyte getHills()
        {
            return hills;
        }
        // Java: 90
        public sbyte getTrees()
        {
            return trees;
        }
        // Java: 94
        public sbyte getSupplies()
        {
            return supplies;
        }
        // Java: 98
        public bool isRated()
        {
            return rated;
        }
        // Java: 102
        public sbyte getGamespeed()
        {
            return gamespeed;
        }
        // Java: 106
        public string getMapcode()
        {
            return mapcode;
        }
        // Java: 110
        public float getRandomStartPos()
        {
            return random_start_pos;
        }
        // Java: 114
        public int getMaxUnitCount()
        {
            return max_unit_count;
        }
        // Java: 118
        public void setDatabaseID(int database_id)
        {
            this.database_id = database_id;
        }
        // Java: 122
        public int getDatabaseID()
        {
            return database_id;
        }
        // Java: 126
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
            // Java: 142
            public com.oddlabs.matchmaking.Game.Builder name(string name_v)
            {
                this.game_name = name_v;
                return this;
            }
            // Java: 147
            public com.oddlabs.matchmaking.Game.Builder size(sbyte size_v)
            {
                this.size_f = size_v;
                return this;
            }
            // Java: 152
            public com.oddlabs.matchmaking.Game.Builder terrain(sbyte terrain_v)
            {
                this.terrain_f = terrain_v;
                return this;
            }
            // Java: 157
            public com.oddlabs.matchmaking.Game.Builder hills(sbyte hills_v)
            {
                this.hills_f = hills_v;
                return this;
            }
            // Java: 162
            public com.oddlabs.matchmaking.Game.Builder trees(sbyte trees_v)
            {
                this.trees_f = trees_v;
                return this;
            }
            // Java: 167
            public com.oddlabs.matchmaking.Game.Builder supplies(sbyte supplies_v)
            {
                this.supplies_f = supplies_v;
                return this;
            }
            // Java: 172
            public com.oddlabs.matchmaking.Game.Builder rated(bool rated_v)
            {
                this.rated_f = rated_v;
                return this;
            }
            // Java: 177
            public com.oddlabs.matchmaking.Game.Builder gamespeed(sbyte gamespeed_v)
            {
                this.gamespeed_f = gamespeed_v;
                return this;
            }
            // Java: 182
            public com.oddlabs.matchmaking.Game.Builder mapcode(string mapcode_v)
            {
                this.mapcode_f = mapcode_v;
                return this;
            }
            // Java: 187
            public com.oddlabs.matchmaking.Game.Builder randomStartPos(float random_start_pos)
            {
                this.random_start_pos = random_start_pos;
                return this;
            }
            // Java: 192
            public com.oddlabs.matchmaking.Game.Builder maxUnitCount(int max_unit_count)
            {
                this.max_unit_count = max_unit_count;
                return this;
            }
            // Java: 197
            public com.oddlabs.matchmaking.Game build()
            {
                return new com.oddlabs.matchmaking.Game(this);
            }
            private void init_Builder()
            {
                this.game_name = "";
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
        }
    }
}
