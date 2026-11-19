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
        public int initial_unit_count;
        public int max_unit_count;
        public int max_chieftains;
        public int max_chieftains_per_quarters;
        public int max_building_count;
        public float chieftain_heal_idle_seconds;
        public int chieftain_heal_amount;
        public int target_num_quarters;
        public int target_num_armories;
        public int max_concurrent_quarters;
        public int max_concurrent_armories;
        public int num_resource_towers;
        public int max_concurrent_towers;
        public int koth_statue_count;
        public int starting_rock_warriors;
        public int starting_iron_warriors;
        public int starting_rubber_warriors;
        public bool magic1_enabled;
        public bool magic2_enabled;
        public bool magic3_enabled;
        public bool chiefs_courage_enabled;
        public bool adaptive_ai_enabled;
        public bool team_together;
        public bool free_form;
        public bool extended;
        public bool weather;
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
        // Java: 126
        public static com.oddlabs.matchmaking.WorldConfig.Builder builder()
        {
            return new com.oddlabs.matchmaking.WorldConfig.Builder();
        }
        // Java: 130
        public static com.oddlabs.matchmaking.WorldConfig defaults()
        {
            return new com.oddlabs.matchmaking.WorldConfig.Builder().build();
        }
        // Java: 134
        public int getGamespeed()
        {
            return gamespeed;
        }
        // Java: 138
        public int getIslandSize()
        {
            return island_size;
        }
        // Java: 142
        public int getTerrainType()
        {
            return terrain_type;
        }
        // Java: 146
        public int getHills()
        {
            return hills;
        }
        // Java: 150
        public int getVegetation()
        {
            return vegetation;
        }
        // Java: 154
        public int getSupplies()
        {
            return supplies;
        }
        // Java: 158
        public int getInitialUnitCount()
        {
            return initial_unit_count;
        }
        // Java: 162
        public int getMaxUnitCount()
        {
            return max_unit_count;
        }
        // Java: 166
        public int getMaxChieftains()
        {
            return max_chieftains;
        }
        // Java: 170
        public int getMaxChieftainsPerQuarters()
        {
            return max_chieftains_per_quarters;
        }
        // Java: 174
        public int getMaxBuildingCount()
        {
            return max_building_count;
        }
        // Java: 178
        public float getChieftainHealIdleSeconds()
        {
            return chieftain_heal_idle_seconds;
        }
        // Java: 182
        public int getChieftainHealAmount()
        {
            return chieftain_heal_amount;
        }
        // Java: 186
        public int getTargetNumQuarters()
        {
            return target_num_quarters;
        }
        // Java: 190
        public int getTargetNumArmories()
        {
            return target_num_armories;
        }
        // Java: 194
        public int getMaxConcurrentQuarters()
        {
            return max_concurrent_quarters;
        }
        // Java: 198
        public int getMaxConcurrentArmories()
        {
            return max_concurrent_armories;
        }
        // Java: 202
        public int getNumResourceTowers()
        {
            return num_resource_towers;
        }
        // Java: 206
        public int getMaxConcurrentTowers()
        {
            return max_concurrent_towers;
        }
        // Java: 210
        public int getKothStatueCount()
        {
            return koth_statue_count;
        }
        // Java: 214
        public int getStartingRockWarriors()
        {
            return starting_rock_warriors;
        }
        // Java: 218
        public int getStartingIronWarriors()
        {
            return starting_iron_warriors;
        }
        // Java: 222
        public int getStartingRubberWarriors()
        {
            return starting_rubber_warriors;
        }
        // Java: 226
        public bool isMagic1Enabled()
        {
            return magic1_enabled;
        }
        // Java: 230
        public bool isMagic2Enabled()
        {
            return magic2_enabled;
        }
        // Java: 234
        public bool isMagic3Enabled()
        {
            return magic3_enabled;
        }
        // Java: 238
        public bool isChiefsCourageEnabled()
        {
            return chiefs_courage_enabled;
        }
        // Java: 242
        public bool isAdaptiveAiEnabled()
        {
            return adaptive_ai_enabled;
        }
        // Java: 246
        public bool isTeamTogether()
        {
            return team_together;
        }
        // Java: 250
        public bool isFreeForm()
        {
            return free_form;
        }
        // Java: 254
        public bool isWeather()
        {
            return weather;
        }
        // Java: 258
        public bool isExtended()
        {
            return extended;
        }
        // Java: 262
        public float getMagic1Cost()
        {
            return magic1_cost;
        }
        // Java: 266
        public float getMagic2Cost()
        {
            return magic2_cost;
        }
        // Java: 270
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
        // Java: 274
        public float getBuildingHealthMultiplier()
        {
            return building_health_multiplier;
        }
        // Java: 278
        public float getVikingChiefHealthMultiplier()
        {
            return viking_chief_health_multiplier;
        }
        // Java: 282
        public float getNativeChiefHealthMultiplier()
        {
            return native_chief_health_multiplier;
        }
        // Java: 286
        public float getUnitRangeMultiplier()
        {
            return unit_range_multiplier;
        }
        // Java: 290
        public int getArmoryResourceCap()
        {
            return armory_resource_cap;
        }
        // Java: 294
        public int getRockResourceCap()
        {
            return rock_resource_cap;
        }
        // Java: 298
        public int getIronResourceCap()
        {
            return iron_resource_cap;
        }
        // Java: 302
        public int getRubberResourceCap()
        {
            return rubber_resource_cap;
        }
        // Java: 306
        public class Builder : java.lang.Object
        {
            public int gamespeed_f;
            public int island_size;
            public int terrain_type;
            public int hills_f;
            public int vegetation_f;
            public int supplies_f;
            public int initial_unit_count;
            public int max_unit_count;
            public int max_chieftains;
            public int max_chieftains_per_quarters;
            public int max_building_count;
            public float chieftain_heal_idle_seconds;
            public int chieftain_heal_amount;
            public int target_num_quarters;
            public int target_num_armories;
            public int max_concurrent_quarters;
            public int max_concurrent_armories;
            public int num_resource_towers;
            public int max_concurrent_towers;
            public int koth_statue_count;
            public int starting_rock_warriors;
            public int starting_iron_warriors;
            public int starting_rubber_warriors;
            public bool magic1_enabled;
            public bool magic2_enabled;
            public bool magic3_enabled;
            public bool chiefs_courage_enabled;
            public bool adaptive_ai_enabled;
            public bool team_together;
            public bool free_form;
            public bool extended_f;
            public bool weather_f;
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
            // Java: 359
            public com.oddlabs.matchmaking.WorldConfig.Builder gamespeed(int gamespeed_v)
            {
                this.gamespeed_f = gamespeed_v;
                return this;
            }
            // Java: 364
            public com.oddlabs.matchmaking.WorldConfig.Builder islandSize(int island_size)
            {
                this.island_size = island_size;
                return this;
            }
            // Java: 369
            public com.oddlabs.matchmaking.WorldConfig.Builder terrainType(int terrain_type)
            {
                this.terrain_type = terrain_type;
                return this;
            }
            // Java: 374
            public com.oddlabs.matchmaking.WorldConfig.Builder hills(int hills_v)
            {
                this.hills_f = hills_v;
                return this;
            }
            // Java: 379
            public com.oddlabs.matchmaking.WorldConfig.Builder vegetation(int vegetation_v)
            {
                this.vegetation_f = vegetation_v;
                return this;
            }
            // Java: 384
            public com.oddlabs.matchmaking.WorldConfig.Builder supplies(int supplies_v)
            {
                this.supplies_f = supplies_v;
                return this;
            }
            // Java: 389
            public com.oddlabs.matchmaking.WorldConfig.Builder initialUnitCount(int v)
            {
                this.initial_unit_count = v;
                return this;
            }
            // Java: 394
            public com.oddlabs.matchmaking.WorldConfig.Builder maxUnitCount(int v)
            {
                this.max_unit_count = v;
                return this;
            }
            // Java: 399
            public com.oddlabs.matchmaking.WorldConfig.Builder maxChieftains(int v)
            {
                this.max_chieftains = v;
                return this;
            }
            // Java: 404
            public com.oddlabs.matchmaking.WorldConfig.Builder maxChieftainsPerQuarters(int v)
            {
                this.max_chieftains_per_quarters = v;
                return this;
            }
            // Java: 409
            public com.oddlabs.matchmaking.WorldConfig.Builder maxBuildingCount(int v)
            {
                this.max_building_count = v;
                return this;
            }
            // Java: 414
            public com.oddlabs.matchmaking.WorldConfig.Builder chieftainHealIdleSeconds(float v)
            {
                this.chieftain_heal_idle_seconds = v;
                return this;
            }
            // Java: 419
            public com.oddlabs.matchmaking.WorldConfig.Builder chieftainHealAmount(int v)
            {
                this.chieftain_heal_amount = v;
                return this;
            }
            // Java: 424
            public com.oddlabs.matchmaking.WorldConfig.Builder targetNumQuarters(int v)
            {
                this.target_num_quarters = v;
                return this;
            }
            // Java: 429
            public com.oddlabs.matchmaking.WorldConfig.Builder targetNumArmories(int v)
            {
                this.target_num_armories = v;
                return this;
            }
            // Java: 434
            public com.oddlabs.matchmaking.WorldConfig.Builder maxConcurrentQuarters(int v)
            {
                this.max_concurrent_quarters = v;
                return this;
            }
            // Java: 439
            public com.oddlabs.matchmaking.WorldConfig.Builder maxConcurrentArmories(int v)
            {
                this.max_concurrent_armories = v;
                return this;
            }
            // Java: 444
            public com.oddlabs.matchmaking.WorldConfig.Builder numResourceTowers(int v)
            {
                this.num_resource_towers = v;
                return this;
            }
            // Java: 449
            public com.oddlabs.matchmaking.WorldConfig.Builder maxConcurrentTowers(int v)
            {
                this.max_concurrent_towers = v;
                return this;
            }
            // Java: 454
            public com.oddlabs.matchmaking.WorldConfig.Builder kothStatueCount(int v)
            {
                this.koth_statue_count = v;
                return this;
            }
            // Java: 459
            public com.oddlabs.matchmaking.WorldConfig.Builder startingRockWarriors(int v)
            {
                this.starting_rock_warriors = v;
                return this;
            }
            // Java: 464
            public com.oddlabs.matchmaking.WorldConfig.Builder startingIronWarriors(int v)
            {
                this.starting_iron_warriors = v;
                return this;
            }
            // Java: 469
            public com.oddlabs.matchmaking.WorldConfig.Builder startingRubberWarriors(int v)
            {
                this.starting_rubber_warriors = v;
                return this;
            }
            // Java: 474
            public com.oddlabs.matchmaking.WorldConfig.Builder magic1Enabled(bool v)
            {
                this.magic1_enabled = v;
                return this;
            }
            // Java: 479
            public com.oddlabs.matchmaking.WorldConfig.Builder magic2Enabled(bool v)
            {
                this.magic2_enabled = v;
                return this;
            }
            // Java: 484
            public com.oddlabs.matchmaking.WorldConfig.Builder magic3Enabled(bool v)
            {
                this.magic3_enabled = v;
                return this;
            }
            // Java: 489
            public com.oddlabs.matchmaking.WorldConfig.Builder chiefsCourageEnabled(bool v)
            {
                this.chiefs_courage_enabled = v;
                return this;
            }
            // Java: 494
            public com.oddlabs.matchmaking.WorldConfig.Builder adaptiveAiEnabled(bool v)
            {
                this.adaptive_ai_enabled = v;
                return this;
            }
            // Java: 499
            public com.oddlabs.matchmaking.WorldConfig.Builder freeForm(bool v)
            {
                this.free_form = v;
                return this;
            }
            // Java: 504
            public com.oddlabs.matchmaking.WorldConfig.Builder weather(bool v)
            {
                this.weather_f = v;
                return this;
            }
            // Java: 509
            public com.oddlabs.matchmaking.WorldConfig.Builder extended(bool v)
            {
                this.extended_f = v;
                return this;
            }
            // Java: 514
            public com.oddlabs.matchmaking.WorldConfig.Builder teamTogether(bool v)
            {
                this.team_together = v;
                return this;
            }
            // Java: 519
            public com.oddlabs.matchmaking.WorldConfig.Builder magic1Cost(float v)
            {
                this.magic1_cost = v;
                return this;
            }
            // Java: 524
            public com.oddlabs.matchmaking.WorldConfig.Builder magic2Cost(float v)
            {
                this.magic2_cost = v;
                return this;
            }
            // Java: 529
            public com.oddlabs.matchmaking.WorldConfig.Builder magic3Cost(float v)
            {
                this.magic3_cost = v;
                return this;
            }
            public com.oddlabs.matchmaking.WorldConfig.Builder extraMagicCost(int index, float v)
            {
                if (index == 3) this.magic4_cost = v; else if (index == 4) this.magic5_cost = v; else if (index == 5) this.magic6_cost = v;
                return this;
            }
            public com.oddlabs.matchmaking.WorldConfig.Builder extraMagicEnabled(int index, bool on)
            {
                if (index == 3) this.magic4_disabled = !on; else if (index == 4) this.magic5_disabled = !on; else if (index == 5) this.magic6_disabled = !on;
                return this;
            }
            // Java: 534
            public com.oddlabs.matchmaking.WorldConfig.Builder buildingHealthMultiplier(float v)
            {
                this.building_health_multiplier = v;
                return this;
            }
            // Java: 539
            public com.oddlabs.matchmaking.WorldConfig.Builder vikingChiefHealthMultiplier(float v)
            {
                this.viking_chief_health_multiplier = v;
                return this;
            }
            // Java: 544
            public com.oddlabs.matchmaking.WorldConfig.Builder nativeChiefHealthMultiplier(float v)
            {
                this.native_chief_health_multiplier = v;
                return this;
            }
            // Java: 549
            public com.oddlabs.matchmaking.WorldConfig.Builder unitRangeMultiplier(float v)
            {
                this.unit_range_multiplier = v;
                return this;
            }
            // Java: 554
            public com.oddlabs.matchmaking.WorldConfig.Builder armoryResourceCap(int v)
            {
                this.armory_resource_cap = v;
                return this;
            }
            // Java: 559
            public com.oddlabs.matchmaking.WorldConfig.Builder rockResourceCap(int v)
            {
                this.rock_resource_cap = v;
                return this;
            }
            // Java: 564
            public com.oddlabs.matchmaking.WorldConfig.Builder ironResourceCap(int v)
            {
                this.iron_resource_cap = v;
                return this;
            }
            // Java: 569
            public com.oddlabs.matchmaking.WorldConfig.Builder rubberResourceCap(int v)
            {
                this.rubber_resource_cap = v;
                return this;
            }
            // Java: 574
            public com.oddlabs.matchmaking.WorldConfig build()
            {
                return new com.oddlabs.matchmaking.WorldConfig(this);
            }
            private void init_Builder()
            {
                this.max_chieftains = 3;
                this.max_chieftains_per_quarters = 1;
                this.max_building_count = 20;
                this.chieftain_heal_idle_seconds = 10.0f;
                this.chieftain_heal_amount = 50;
                this.target_num_quarters = 3;
                this.target_num_armories = 1;
                this.max_concurrent_quarters = 2;
                this.max_concurrent_armories = 1;
                this.num_resource_towers = 6;
                this.max_concurrent_towers = 6;
                this.koth_statue_count = 5;
                this.starting_rock_warriors = 0;
                this.starting_iron_warriors = 0;
                this.starting_rubber_warriors = 0;
                this.magic1_enabled = true;
                this.magic2_enabled = true;
                this.magic3_enabled = true;
                this.chiefs_courage_enabled = true;
                this.adaptive_ai_enabled = false;
                this.team_together = false;
                this.free_form = false;
                this.extended_f = false;
                this.weather_f = false;
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
        public WorldConfig(com.oddlabs.matchmaking.WorldConfig.Builder b) : base()
        {
            this.gamespeed = b.gamespeed_f;
            this.island_size = b.island_size;
            this.terrain_type = b.terrain_type;
            this.hills = b.hills_f;
            this.vegetation = b.vegetation_f;
            this.supplies = b.supplies_f;
            this.initial_unit_count = b.initial_unit_count;
            this.max_unit_count = b.max_unit_count;
            this.max_chieftains = b.max_chieftains;
            this.max_chieftains_per_quarters = b.max_chieftains_per_quarters;
            this.max_building_count = b.max_building_count;
            this.chieftain_heal_idle_seconds = b.chieftain_heal_idle_seconds;
            this.chieftain_heal_amount = b.chieftain_heal_amount;
            this.target_num_quarters = b.target_num_quarters;
            this.target_num_armories = b.target_num_armories;
            this.max_concurrent_quarters = b.max_concurrent_quarters;
            this.max_concurrent_armories = b.max_concurrent_armories;
            this.num_resource_towers = b.num_resource_towers;
            this.max_concurrent_towers = b.max_concurrent_towers;
            this.koth_statue_count = b.koth_statue_count;
            this.starting_rock_warriors = b.starting_rock_warriors;
            this.starting_iron_warriors = b.starting_iron_warriors;
            this.starting_rubber_warriors = b.starting_rubber_warriors;
            this.magic1_enabled = b.magic1_enabled;
            this.magic2_enabled = b.magic2_enabled;
            this.magic3_enabled = b.magic3_enabled;
            this.chiefs_courage_enabled = b.chiefs_courage_enabled;
            this.adaptive_ai_enabled = b.adaptive_ai_enabled;
            this.team_together = b.team_together;
            this.free_form = b.free_form;
            this.extended = b.extended_f;
            this.weather = b.weather_f;
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
        }
    }
}
