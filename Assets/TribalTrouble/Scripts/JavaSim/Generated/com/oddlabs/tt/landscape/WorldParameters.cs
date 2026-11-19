// Translated from WorldParameters.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.landscape
{
    // Java: 11
    public class WorldParameters : java.lang.Object
    {
        public const long serialVersionUID = 1L;
        public const int DEFAULT_NUM_RESOURCE_TOWERS = 7;
        public const int NUM_RESOURCE_TOWERS_RANDOM = (-1);
        public const int MIN_RANDOM_RESOURCE_TOWERS = 1;
        public const int MAX_RANDOM_RESOURCE_TOWERS = 50;
        public const int MIN_KOTH_STATUE_COUNT = 3;
        public const int MAX_KOTH_STATUE_COUNT = 10;
        public const int DEFAULT_KOTH_STATUE_COUNT = 5;
        public string map_code;
        public int initial_unit_count;
        public int max_unit_count;
        public int initial_game_speed;
        public int map_size;
        public com.oddlabs.matchmaking.GameMode mode;
        public int max_chieftains;
        public int max_chieftains_per_quarters;
        public int max_building_count;
        public int koth_statue_count;
        public float chieftain_heal_idle_seconds;
        public int chieftain_heal_amount;
        public int target_num_quarters;
        public int target_num_armories;
        public int max_concurrent_quarters;
        public int max_concurrent_armories;
        public int num_resource_towers;
        public int max_concurrent_towers;
        public int starting_rock_warriors;
        public int starting_iron_warriors;
        public int starting_rubber_warriors;
        public bool magic1_enabled;
        public bool magic2_enabled;
        public bool magic3_enabled;
        public bool chiefs_courage_enabled;
        public bool adaptive_ai_enabled;
        public int adaptive_ai_seed_difficulty;
        public bool team_together;
        public float magic1_cost;
        public float magic2_cost;
        public float magic3_cost;
        public float magic4_cost;
        public bool magic4_disabled;
        public float magic5_cost;
        public bool magic5_disabled;
        public float magic6_cost;
        public bool magic6_disabled;
        public float building_health_multiplier;
        public float viking_chief_health_multiplier;
        public float native_chief_health_multiplier;
        public float unit_range_multiplier;
        public int armory_resource_cap;
        public int rock_resource_cap;
        public int iron_resource_cap;
        public int rubber_resource_cap;
        public bool free_form;
        public bool weather;
        public bool extended;
        public string island_wars;
        // Java: 187
        public static com.oddlabs.tt.landscape.WorldParameters.Builder builder()
        {
            return new com.oddlabs.tt.landscape.WorldParameters.Builder();
        }
        // Java: 191
        public string getMapcode()
        {
            return map_code;
        }
        // Java: 195
        public int getInitialUnitCount()
        {
            return initial_unit_count;
        }
        // Java: 199
        public int getMaxUnitCount()
        {
            return max_unit_count;
        }
        // Java: 203
        public int getInitialGameSpeed()
        {
            return initial_game_speed;
        }
        // Java: 207
        public int getMapSize()
        {
            return map_size;
        }
        // Java: 211
        public com.oddlabs.matchmaking.GameMode getGameMode()
        {
            return mode;
        }
        // Java: 216
        public int getMaxChieftains()
        {
            return max_chieftains;
        }
        // Java: 221
        public int getMaxChieftainsPerQuarters()
        {
            return max_chieftains_per_quarters;
        }
        // Java: 226
        public int getMaxBuildingCount()
        {
            return max_building_count;
        }
        // Java: 231
        public int getKothStatueCount()
        {
            return koth_statue_count;
        }
        // Java: 236
        public float getChieftainHealIdleSeconds()
        {
            return chieftain_heal_idle_seconds;
        }
        // Java: 241
        public int getChieftainHealAmount()
        {
            return chieftain_heal_amount;
        }
        // Java: 246
        public int getTargetNumQuarters()
        {
            return target_num_quarters;
        }
        // Java: 251
        public int getTargetNumArmories()
        {
            return target_num_armories;
        }
        // Java: 256
        public int getMaxConcurrentQuarters()
        {
            return max_concurrent_quarters;
        }
        // Java: 261
        public int getMaxConcurrentArmories()
        {
            return max_concurrent_armories;
        }
        // Java: 266
        public int getNumResourceTowers()
        {
            return num_resource_towers;
        }
        // Java: 271
        public int getMaxConcurrentTowers()
        {
            return max_concurrent_towers;
        }
        // Java: 275
        public int getStartingRockWarriors()
        {
            return starting_rock_warriors;
        }
        // Java: 279
        public int getStartingIronWarriors()
        {
            return starting_iron_warriors;
        }
        // Java: 283
        public int getStartingRubberWarriors()
        {
            return starting_rubber_warriors;
        }
        // Java: 287
        public bool isMagic1Enabled()
        {
            return magic1_enabled;
        }
        // Java: 291
        public bool isMagic2Enabled()
        {
            return magic2_enabled;
        }
        // Java: 295
        public bool isMagic3Enabled()
        {
            return magic3_enabled;
        }
        // Java: 299
        public bool isChiefsCourageEnabled()
        {
            return chiefs_courage_enabled;
        }
        // Java: 303
        public bool isAdaptiveAiEnabled()
        {
            return adaptive_ai_enabled;
        }
        // Java: 307
        public int getAdaptiveAiSeedDifficulty()
        {
            return adaptive_ai_seed_difficulty;
        }
        // Java: 311
        public bool isTeamTogether()
        {
            return team_together;
        }
        // Java: 315
        public bool isWeather()
        {
            return weather;
        }
        // Java: 319
        public bool isFreeForm()
        {
            return free_form;
        }
        // Java: 323
        public string getIslandWars()
        {
            return (island_wars == null ? "" : island_wars);
        }
        // Java: 327
        public void setIslandWars(string setup)
        {
            this.island_wars = setup;
        }
        // Java: 331
        public bool isExtended()
        {
            return extended;
        }
        // Java: 335
        public float getMagic1Cost()
        {
            return magic1_cost;
        }
        // Java: 339
        public float getMagic2Cost()
        {
            return magic2_cost;
        }
        // Java: 343
        public float getMagic3Cost()
        {
            return magic3_cost;
        }
        // the extended chieftain magics (index 3..5: Spirit Dance / War Cry, Ancestor Shield / Berserk, Healing Rain /
        // Fire Storm): their cost (0: the game's own) and whether they are off (absent in older presets: on)
        public float getExtraMagicCost(int index)
        {
            return (index) switch { 3 => magic4_cost, 4 => magic5_cost, 5 => magic6_cost, _ => 0.0f };
        }
        public bool isExtraMagicEnabled(int index)
        {
            return !((index) switch { 3 => magic4_disabled, 4 => magic5_disabled, 5 => magic6_disabled, _ => false });
        }
        // Java: 347
        public float getBuildingHealthMultiplier()
        {
            return building_health_multiplier;
        }
        // Java: 355
        public float getChiefHealthMultiplier(int race_index)
        {
            if (race_index == com.oddlabs.tt.model.RacesResources.RACE_VIKINGS)
            {
                return viking_chief_health_multiplier;
            }
            if (race_index == com.oddlabs.tt.model.RacesResources.RACE_NATIVES)
            {
                return native_chief_health_multiplier;
            }
            return 1.0f;
        }
        // Java: 363
        public float getUnitRangeMultiplier()
        {
            return unit_range_multiplier;
        }
        // Java: 367
        public int getArmoryResourceCap()
        {
            return armory_resource_cap;
        }
        // Java: 371
        public int getRockResourceCap()
        {
            return rock_resource_cap;
        }
        // Java: 375
        public int getIronResourceCap()
        {
            return iron_resource_cap;
        }
        // Java: 379
        public int getRubberResourceCap()
        {
            return rubber_resource_cap;
        }
        // Java: 383
        public class Builder : java.lang.Object
        {
            public string mapcode_f;
            public int initial_unit_count;
            public int max_unit_count;
            public int initial_game_speed;
            public int map_size;
            public com.oddlabs.matchmaking.GameMode mode_f;
            public int max_chieftains;
            public int max_chieftains_per_quarters;
            public int max_building_count;
            public int koth_statue_count;
            public float chieftain_heal_idle_seconds;
            public int chieftain_heal_amount;
            public int target_num_quarters;
            public int target_num_armories;
            public int max_concurrent_quarters;
            public int max_concurrent_armories;
            public int num_resource_towers;
            public int max_concurrent_towers;
            public int starting_rock_warriors;
            public int starting_iron_warriors;
            public int starting_rubber_warriors;
            public bool magic1_enabled;
            public bool magic2_enabled;
            public bool magic3_enabled;
            public bool chiefs_courage_enabled;
            public bool adaptive_ai_enabled;
            public int adaptive_ai_seed_difficulty;
            public bool team_together;
            public bool free_form;
            public bool weather_f;
            public bool extended_f;
            public string island_wars;
            public float magic1_cost;
            public float magic2_cost;
            public float magic3_cost;
            public float magic4_cost;
            public bool magic4_disabled;
            public float magic5_cost;
            public bool magic5_disabled;
            public float magic6_cost;
            public bool magic6_disabled;
            public float building_health_multiplier;
            public float viking_chief_health_multiplier;
            public float native_chief_health_multiplier;
            public float unit_range_multiplier;
            public int armory_resource_cap;
            public int rock_resource_cap;
            public int iron_resource_cap;
            public int rubber_resource_cap;
            // Java: 441
            public com.oddlabs.tt.landscape.WorldParameters.Builder mapcode(string mapcode_v)
            {
                this.mapcode_f = mapcode_v;
                return this;
            }
            // Java: 446
            public com.oddlabs.tt.landscape.WorldParameters.Builder initialUnitCount(int v)
            {
                this.initial_unit_count = v;
                return this;
            }
            // Java: 453
            public com.oddlabs.tt.landscape.WorldParameters.Builder initialUnitCountSetting(int v)
            {
                this.initial_unit_count = v;
                return this;
            }
            // Java: 458
            public com.oddlabs.tt.landscape.WorldParameters.Builder maxUnitCount(int v)
            {
                this.max_unit_count = v;
                return this;
            }
            // Java: 463
            public com.oddlabs.tt.landscape.WorldParameters.Builder initialGameSpeed(int v)
            {
                this.initial_game_speed = v;
                return this;
            }
            // Java: 468
            public com.oddlabs.tt.landscape.WorldParameters.Builder mapSize(int v)
            {
                this.map_size = v;
                return this;
            }
            // Java: 473
            public com.oddlabs.tt.landscape.WorldParameters.Builder mode(com.oddlabs.matchmaking.GameMode mode_v)
            {
                this.mode_f = mode_v;
                return this;
            }
            // Java: 479
            public com.oddlabs.tt.landscape.WorldParameters.Builder maxChieftains(int v)
            {
                this.max_chieftains = v;
                return this;
            }
            // Java: 485
            public com.oddlabs.tt.landscape.WorldParameters.Builder maxChieftainsPerQuarters(int v)
            {
                this.max_chieftains_per_quarters = v;
                return this;
            }
            // Java: 491
            public com.oddlabs.tt.landscape.WorldParameters.Builder maxBuildingCount(int v)
            {
                this.max_building_count = v;
                return this;
            }
            // Java: 501
            public com.oddlabs.tt.landscape.WorldParameters.Builder kothStatueCount(int v)
            {
                this.koth_statue_count = java.lang.Math.clamp(v, com.oddlabs.tt.landscape.WorldParameters.MIN_KOTH_STATUE_COUNT, com.oddlabs.tt.landscape.WorldParameters.MAX_KOTH_STATUE_COUNT);
                return this;
            }
            // Java: 507
            public com.oddlabs.tt.landscape.WorldParameters.Builder chieftainHealIdleSeconds(float v)
            {
                this.chieftain_heal_idle_seconds = v;
                return this;
            }
            // Java: 513
            public com.oddlabs.tt.landscape.WorldParameters.Builder chieftainHealAmount(int v)
            {
                this.chieftain_heal_amount = v;
                return this;
            }
            // Java: 519
            public com.oddlabs.tt.landscape.WorldParameters.Builder targetNumQuarters(int v)
            {
                this.target_num_quarters = v;
                return this;
            }
            // Java: 525
            public com.oddlabs.tt.landscape.WorldParameters.Builder targetNumArmories(int v)
            {
                this.target_num_armories = v;
                return this;
            }
            // Java: 531
            public com.oddlabs.tt.landscape.WorldParameters.Builder maxConcurrentQuarters(int v)
            {
                this.max_concurrent_quarters = v;
                return this;
            }
            // Java: 537
            public com.oddlabs.tt.landscape.WorldParameters.Builder maxConcurrentArmories(int v)
            {
                this.max_concurrent_armories = v;
                return this;
            }
            // Java: 543
            public com.oddlabs.tt.landscape.WorldParameters.Builder numResourceTowers(int v)
            {
                this.num_resource_towers = v;
                return this;
            }
            // Java: 549
            public com.oddlabs.tt.landscape.WorldParameters.Builder maxConcurrentTowers(int v)
            {
                this.max_concurrent_towers = v;
                return this;
            }
            // Java: 554
            public com.oddlabs.tt.landscape.WorldParameters.Builder startingRockWarriors(int v)
            {
                this.starting_rock_warriors = v;
                return this;
            }
            // Java: 559
            public com.oddlabs.tt.landscape.WorldParameters.Builder startingIronWarriors(int v)
            {
                this.starting_iron_warriors = v;
                return this;
            }
            // Java: 564
            public com.oddlabs.tt.landscape.WorldParameters.Builder startingRubberWarriors(int v)
            {
                this.starting_rubber_warriors = v;
                return this;
            }
            // Java: 569
            public com.oddlabs.tt.landscape.WorldParameters.Builder magic1Enabled(bool v)
            {
                this.magic1_enabled = v;
                return this;
            }
            // Java: 574
            public com.oddlabs.tt.landscape.WorldParameters.Builder magic2Enabled(bool v)
            {
                this.magic2_enabled = v;
                return this;
            }
            // Java: 579
            public com.oddlabs.tt.landscape.WorldParameters.Builder magic3Enabled(bool v)
            {
                this.magic3_enabled = v;
                return this;
            }
            // Java: 584
            public com.oddlabs.tt.landscape.WorldParameters.Builder chiefsCourageEnabled(bool v)
            {
                this.chiefs_courage_enabled = v;
                return this;
            }
            // Java: 589
            public com.oddlabs.tt.landscape.WorldParameters.Builder adaptiveAiEnabled(bool v)
            {
                this.adaptive_ai_enabled = v;
                return this;
            }
            // Java: 594
            public com.oddlabs.tt.landscape.WorldParameters.Builder adaptiveAiSeedDifficulty(int v)
            {
                this.adaptive_ai_seed_difficulty = v;
                return this;
            }
            // Java: 599
            public com.oddlabs.tt.landscape.WorldParameters.Builder weather(bool v)
            {
                this.weather_f = v;
                return this;
            }
            // Java: 604
            public com.oddlabs.tt.landscape.WorldParameters.Builder freeForm(bool v)
            {
                this.free_form = v;
                return this;
            }
            // Java: 609
            public com.oddlabs.tt.landscape.WorldParameters.Builder islandWars(string v)
            {
                this.island_wars = v;
                return this;
            }
            // Java: 614
            public com.oddlabs.tt.landscape.WorldParameters.Builder extended(bool v)
            {
                this.extended_f = v;
                return this;
            }
            // Java: 619
            public com.oddlabs.tt.landscape.WorldParameters.Builder teamTogether(bool v)
            {
                this.team_together = v;
                return this;
            }
            // Java: 624
            public com.oddlabs.tt.landscape.WorldParameters.Builder magic1Cost(float v)
            {
                this.magic1_cost = v;
                return this;
            }
            // Java: 629
            public com.oddlabs.tt.landscape.WorldParameters.Builder magic2Cost(float v)
            {
                this.magic2_cost = v;
                return this;
            }
            // Java: 634
            public com.oddlabs.tt.landscape.WorldParameters.Builder magic3Cost(float v)
            {
                this.magic3_cost = v;
                return this;
            }
            public com.oddlabs.tt.landscape.WorldParameters.Builder extraMagicCost(int index, float v)
            {
                if (index == 3) this.magic4_cost = v; else if (index == 4) this.magic5_cost = v; else if (index == 5) this.magic6_cost = v;
                return this;
            }
            public com.oddlabs.tt.landscape.WorldParameters.Builder extraMagicEnabled(int index, bool on)
            {
                if (index == 3) this.magic4_disabled = !on; else if (index == 4) this.magic5_disabled = !on; else if (index == 5) this.magic6_disabled = !on;
                return this;
            }
            // Java: 639
            public com.oddlabs.tt.landscape.WorldParameters.Builder buildingHealthMultiplier(float v)
            {
                this.building_health_multiplier = v;
                return this;
            }
            // Java: 644
            public com.oddlabs.tt.landscape.WorldParameters.Builder vikingChiefHealthMultiplier(float v)
            {
                this.viking_chief_health_multiplier = v;
                return this;
            }
            // Java: 649
            public com.oddlabs.tt.landscape.WorldParameters.Builder nativeChiefHealthMultiplier(float v)
            {
                this.native_chief_health_multiplier = v;
                return this;
            }
            // Java: 654
            public com.oddlabs.tt.landscape.WorldParameters.Builder unitRangeMultiplier(float v)
            {
                this.unit_range_multiplier = v;
                return this;
            }
            // Java: 659
            public com.oddlabs.tt.landscape.WorldParameters.Builder armoryResourceCap(int v)
            {
                this.armory_resource_cap = v;
                return this;
            }
            // Java: 664
            public com.oddlabs.tt.landscape.WorldParameters.Builder rockResourceCap(int v)
            {
                this.rock_resource_cap = v;
                return this;
            }
            // Java: 669
            public com.oddlabs.tt.landscape.WorldParameters.Builder ironResourceCap(int v)
            {
                this.iron_resource_cap = v;
                return this;
            }
            // Java: 674
            public com.oddlabs.tt.landscape.WorldParameters.Builder rubberResourceCap(int v)
            {
                this.rubber_resource_cap = v;
                return this;
            }
            // Java: 679
            public com.oddlabs.tt.landscape.WorldParameters build()
            {
                return new com.oddlabs.tt.landscape.WorldParameters(this);
            }
            private void init_Builder()
            {
                this.mapcode_f = "";
                this.map_size = com.oddlabs.matchmaking.Game.SIZE_NONE;
                this.mode_f = com.oddlabs.matchmaking.GameMode.STANDARD;
                this.max_chieftains = 3;
                this.max_chieftains_per_quarters = 1;
                this.max_building_count = 20;
                this.koth_statue_count = com.oddlabs.tt.landscape.WorldParameters.DEFAULT_KOTH_STATUE_COUNT;
                this.chieftain_heal_idle_seconds = 10.0f;
                this.chieftain_heal_amount = 50;
                this.target_num_quarters = 3;
                this.target_num_armories = 1;
                this.max_concurrent_quarters = 2;
                this.max_concurrent_armories = 1;
                this.num_resource_towers = com.oddlabs.tt.landscape.WorldParameters.DEFAULT_NUM_RESOURCE_TOWERS;
                this.max_concurrent_towers = 6;
                this.starting_rock_warriors = 0;
                this.starting_iron_warriors = 0;
                this.starting_rubber_warriors = 0;
                this.magic1_enabled = true;
                this.magic2_enabled = true;
                this.magic3_enabled = true;
                this.chiefs_courage_enabled = true;
                this.adaptive_ai_enabled = false;
                this.adaptive_ai_seed_difficulty = (-1);
                this.team_together = false;
                this.free_form = false;
                this.weather_f = false;
                this.extended_f = false;
                this.island_wars = "";
                this.magic1_cost = 40.0f;
                this.magic2_cost = 70.0f;
                this.magic3_cost = 120.0f;
                this.building_health_multiplier = 1.0f;
                this.viking_chief_health_multiplier = 1.0f;
                this.native_chief_health_multiplier = 1.0f;
                this.unit_range_multiplier = 1.0f;
                this.armory_resource_cap = 50000;
                this.rock_resource_cap = 50000;
                this.iron_resource_cap = 50000;
                this.rubber_resource_cap = 50000;
            }
            public Builder() : base()
            {
                init_Builder();
            }
        }
        public WorldParameters(int initial_game_speed, string map_code, int initial_unit_count, int max_unit_count) : this(initial_game_speed, map_code, initial_unit_count, max_unit_count, com.oddlabs.matchmaking.Game.SIZE_NONE, com.oddlabs.matchmaking.GameMode.STANDARD)
        {
        }
        public WorldParameters(int initial_game_speed, string map_code, int initial_unit_count, int max_unit_count, int map_size) : this(initial_game_speed, map_code, initial_unit_count, max_unit_count, map_size, com.oddlabs.matchmaking.GameMode.STANDARD)
        {
        }
        public WorldParameters(int initial_game_speed, string map_code, int initial_unit_count, int max_unit_count, int map_size, com.oddlabs.matchmaking.GameMode mode) : this(com.oddlabs.tt.landscape.WorldParameters.builder().initialGameSpeed(initial_game_speed).mapcode(map_code).initialUnitCount(initial_unit_count).maxUnitCount(max_unit_count).mapSize(map_size).mode(mode))
        {
        }
        public WorldParameters(com.oddlabs.tt.landscape.WorldParameters.Builder b) : base()
        {
            this.map_code = b.mapcode_f;
            this.initial_unit_count = b.initial_unit_count;
            this.max_unit_count = b.max_unit_count;
            this.initial_game_speed = b.initial_game_speed;
            this.map_size = b.map_size;
            this.mode = b.mode_f;
            this.max_chieftains = b.max_chieftains;
            this.max_chieftains_per_quarters = b.max_chieftains_per_quarters;
            this.max_building_count = b.max_building_count;
            this.koth_statue_count = b.koth_statue_count;
            this.chieftain_heal_idle_seconds = b.chieftain_heal_idle_seconds;
            this.chieftain_heal_amount = b.chieftain_heal_amount;
            this.target_num_quarters = b.target_num_quarters;
            this.target_num_armories = b.target_num_armories;
            this.max_concurrent_quarters = b.max_concurrent_quarters;
            this.max_concurrent_armories = b.max_concurrent_armories;
            this.num_resource_towers = b.num_resource_towers;
            this.max_concurrent_towers = b.max_concurrent_towers;
            this.starting_rock_warriors = b.starting_rock_warriors;
            this.starting_iron_warriors = b.starting_iron_warriors;
            this.starting_rubber_warriors = b.starting_rubber_warriors;
            this.magic1_enabled = b.magic1_enabled;
            this.magic2_enabled = b.magic2_enabled;
            this.magic3_enabled = b.magic3_enabled;
            this.chiefs_courage_enabled = b.chiefs_courage_enabled;
            this.adaptive_ai_enabled = b.adaptive_ai_enabled;
            this.adaptive_ai_seed_difficulty = b.adaptive_ai_seed_difficulty;
            this.team_together = b.team_together;
            this.magic1_cost = b.magic1_cost;
            this.magic2_cost = b.magic2_cost;
            this.magic3_cost = b.magic3_cost;
            this.magic4_cost = b.magic4_cost;
            this.magic4_disabled = b.magic4_disabled;
            this.magic5_cost = b.magic5_cost;
            this.magic5_disabled = b.magic5_disabled;
            this.magic6_cost = b.magic6_cost;
            this.magic6_disabled = b.magic6_disabled;
            this.building_health_multiplier = b.building_health_multiplier;
            this.viking_chief_health_multiplier = b.viking_chief_health_multiplier;
            this.native_chief_health_multiplier = b.native_chief_health_multiplier;
            this.unit_range_multiplier = b.unit_range_multiplier;
            this.armory_resource_cap = b.armory_resource_cap;
            this.rock_resource_cap = b.rock_resource_cap;
            this.iron_resource_cap = b.iron_resource_cap;
            this.rubber_resource_cap = b.rubber_resource_cap;
            this.free_form = b.free_form;
            this.weather = b.weather_f;
            this.extended = b.extended_f;
            this.island_wars = b.island_wars;
        }
    }
}
