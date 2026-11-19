// Translated from WorldConfig.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchmaking
{
    // Java: 15
    public class WorldConfig : java.lang.Object
    {
        public const long serialVersionUID = 1L;
        public int gamespeed;
        public int island_size;
        public int terrain_type;
        public int hills;
        public int vegetation;
        public int supplies;
        public int max_units;
        public int starting_units;
        public int max_buildings;
        public bool ships;
        public bool night;
        // Java: 46
        public static com.oddlabs.matchmaking.WorldConfig.Builder builder()
        {
            return new com.oddlabs.matchmaking.WorldConfig.Builder();
        }
        // Java: 50
        public static com.oddlabs.matchmaking.WorldConfig defaults()
        {
            return new com.oddlabs.matchmaking.WorldConfig.Builder().build();
        }
        // Java: 54
        public int getGamespeed()
        {
            return gamespeed;
        }
        // Java: 58
        public int getIslandSize()
        {
            return island_size;
        }
        // Java: 62
        public int getTerrainType()
        {
            return terrain_type;
        }
        // Java: 66
        public int getHills()
        {
            return hills;
        }
        // Java: 70
        public int getVegetation()
        {
            return vegetation;
        }
        // Java: 74
        public int getSupplies()
        {
            return supplies;
        }
        // Java: 78
        public int getMaxUnits()
        {
            return max_units;
        }
        // Java: 82
        public int getStartingUnits()
        {
            return starting_units;
        }
        // Java: 86
        public int getMaxBuildings()
        {
            return max_buildings;
        }
        // Java: 90
        public bool isShips()
        {
            return ships;
        }
        // Java: 94
        public bool isNight()
        {
            return night;
        }
        // Java: 98
        public class Builder : java.lang.Object
        {
            public int gamespeed_f;
            public int island_size;
            public int terrain_type;
            public int hills_f;
            public int vegetation_f;
            public int supplies_f;
            public int max_units;
            public int starting_units;
            public int max_buildings;
            public bool ships_f;
            public bool night_f;
            // Java: 115
            public com.oddlabs.matchmaking.WorldConfig.Builder gamespeed(int gamespeed_v)
            {
                this.gamespeed_f = gamespeed_v;
                return this;
            }
            // Java: 120
            public com.oddlabs.matchmaking.WorldConfig.Builder islandSize(int island_size)
            {
                this.island_size = island_size;
                return this;
            }
            // Java: 125
            public com.oddlabs.matchmaking.WorldConfig.Builder terrainType(int terrain_type)
            {
                this.terrain_type = terrain_type;
                return this;
            }
            // Java: 130
            public com.oddlabs.matchmaking.WorldConfig.Builder hills(int hills_v)
            {
                this.hills_f = hills_v;
                return this;
            }
            // Java: 135
            public com.oddlabs.matchmaking.WorldConfig.Builder vegetation(int vegetation_v)
            {
                this.vegetation_f = vegetation_v;
                return this;
            }
            // Java: 140
            public com.oddlabs.matchmaking.WorldConfig.Builder supplies(int supplies_v)
            {
                this.supplies_f = supplies_v;
                return this;
            }
            // Java: 145
            public com.oddlabs.matchmaking.WorldConfig.Builder maxUnits(int max_units)
            {
                this.max_units = max_units;
                return this;
            }
            // Java: 150
            public com.oddlabs.matchmaking.WorldConfig.Builder startingUnits(int starting_units)
            {
                this.starting_units = starting_units;
                return this;
            }
            // Java: 155
            public com.oddlabs.matchmaking.WorldConfig.Builder maxBuildings(int max_buildings)
            {
                this.max_buildings = max_buildings;
                return this;
            }
            // Java: 160
            public com.oddlabs.matchmaking.WorldConfig.Builder ships(bool ships_v)
            {
                this.ships_f = ships_v;
                return this;
            }
            // Java: 165
            public com.oddlabs.matchmaking.WorldConfig.Builder night(bool night_v)
            {
                this.night_f = night_v;
                return this;
            }
            // Java: 170
            public com.oddlabs.matchmaking.WorldConfig build()
            {
                return new com.oddlabs.matchmaking.WorldConfig(this);
            }
            private void init_Builder()
            {
                this.max_units = com.oddlabs.matchmaking.Game.DEFAULT_MAX_UNIT_COUNT;
                this.starting_units = com.oddlabs.matchmaking.Game.DEFAULT_INITIAL_UNIT_COUNT;
                this.max_buildings = com.oddlabs.matchmaking.Game.DEFAULT_MAX_BUILDING_COUNT;
            }
            public Builder() : base()
            {
                init_Builder();
            }
        }
        public WorldConfig(com.oddlabs.matchmaking.WorldConfig.Builder b) : base()
        {
            this.gamespeed = b.gamespeed_f;
            this.island_size = b.island_size;
            this.terrain_type = b.terrain_type;
            this.hills = b.hills_f;
            this.vegetation = b.vegetation_f;
            this.supplies = b.supplies_f;
            this.max_units = b.max_units;
            this.starting_units = b.starting_units;
            this.max_buildings = b.max_buildings;
            this.ships = b.ships_f;
            this.night = b.night_f;
        }
    }
}
