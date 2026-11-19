// Translated from Params.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.fable
{
    // Java: 16
    public class Params : java.lang.Object
    {
        public float fast_period;
        public float slow_period;
        public float econ_period;
        public int armory_max_dist;
        public int armory_min_dist;
        public float armory_sector_deg;
        public float site_w_wood;
        public float site_w_iron;
        public float site_w_start;
        public float site_w_detour;
        public int armory_max_walk;
        public float site_iron_radius;
        public int site_min_iron_nodes;
        public float site_penalty_few_iron;
        public float site_enemy_iron_factor;
        public int quarters_min_dist;
        public int quarters_max_dist;
        public int building_spacing;
        public int tower_ring_radius;
        public int tower_spacing_min;
        public int quarters_min;
        public int quarters_max;
        public int towers_max;
        public int quarters_first_crew;
        public int opening_quarters;
        public int opening_parallel;
        public int breeders_early;
        public int breeders_boom;
        public int breeders_late;
        public int breeders_rear;
        public int boom_units;
        public int breeders_training;
        public int workers_min;
        public int workers_surplus_max;
        public float stock_target_wood_per_rate;
        public float stock_target_iron_per_rate;
        public int stock_wood_min;
        public int stock_wood_max;
        public int stock_iron_min;
        public int stock_iron_max;
        public int stock_rock_target;
        public int rock_reserve;
        public int max_peons_per_node;
        public int gather_radius;
        public int conversions_per_tick;
        public float conversion_cooldown;
        public int max_supply_orders_per_tick;
        public int cap_margin;
        public int cap_workers;
        public int cap_wood;
        public int cap_iron;
        public int cap_weapon_buffer;
        public int hunt_max_dist;
        public int hunt_max_peons;
        public int hunt_min_time;
        public float far_iron_m;
        public int hot_node_radius;
        public int crew_quarters;
        public int crew_tower;
        public int crew_armory_rebuild;
        public int crew_armory_opening;
        public bool expansion_armory;
        public float expand_min_time;
        public float expand_iron_m;
        public int expand_max_dist;
        public int crew_armory_expand;
        public float expand_retry_after;
        public int expand_workers;
        public float site_fail_timeout;
        public float site_stall_timeout;
        public int site_retries;
        public int helper_radius;
        public int chieftain_min_peons;
        public float chieftain_min_time;
        public int chieftain_min_quarters;
        public float stun_radius_cells;
        public int stun_cast_dist;
        public float counter_toot_progress;
        public int chief_escape_hp;
        public int chief_fragile_hp;
        public int chief_follow_dist;
        public int blast_min_peons;
        public int blast_max_own;
        public float value_rock;
        public float value_iron;
        public float value_rubber;
        public float value_chief;
        public float value_tower_rock;
        public float value_tower_iron;
        public float value_tower_rubber;
        public float engage_ratio;
        public float hold_ratio;
        public float retreat_ratio;
        public float attack_ratio;
        public float attack_ratio_no_chief;
        public float attack_ratio_stun;
        public float attack_min_time;
        public int attack_min_army;
        public float attack_max_army;
        public float capped_ratio;
        public float capped_total_ratio;
        public float capped_total_ratio_relaxed;
        public float cap_relax_time;
        public float attack_margin_per_march_s;
        public float retreat_cooldown;
        public float base_stun_value;
        public bool stun_recovery;
        public float attack_margin;
        public int home_guard_early;
        public int home_guard_mid;
        public int home_guard_late;
        public int home_guard_defend_bonus;
        public float guard_mid_time;
        public float guard_late_time;
        public int reinforce_batch;
        public int reinforce_batch_near;
        public int leg_length;
        public float leg_arrival_fraction;
        public int leg_arrival_radius;
        public float leg_timeout;
        public int staging_dist;
        public int threat_radius;
        public int alert_eta;
        public float engage_reorder_period;
        public int leash_dist;
        public int all_in_units;
        public float posture_dwell;
        public int tower_reach2;
        public int stand_min2;
        public int stand_max2;
        public int siege_min_army;
        public float siege_camp_ratio;
        public float siege_camp_patience;
        public bool field_sweep;
        public int sweep_radius;
        public float defend_sweep_after;
        public float stun_window;
        public float defend_pursuit;
        public bool corps;
        public int corps_size;
        public int corps_min_army;
        public int corps_min_demolish;
        public int corps_follow_dist;
        // Java: 206
        public void @override(string spec)
        {
            if (spec.isBlank())
            {
                return;
            }
            foreach (string entry in spec.split(","))
            {
                int eq = entry.indexOf('=');
                if (eq <= 0)
                {
                    continue;
                }
                string name = entry.substring(0, eq).trim();
                string @value = entry.substring(eq + 1).trim();
                try
                {
                    java.lang.reflect.Field f = typeof(com.oddlabs.tt.player.fable.Params).getField(name);
                    if (java.lang.reflect.Modifier.isStatic(f.getModifiers()))
                    {
                        continue;
                    }
                    System.Type t = f.getType();
                    if (((object)t == (object)typeof(int)))
                    {
                        f.setInt(this, java.lang.Integer.parseInt(@value));
                    }
                    else if (((object)t == (object)typeof(float)))
                    {
                        f.setFloat(this, java.lang.Float.parseFloat(@value));
                    }
                    else if (((object)t == (object)typeof(bool)))
                    {
                        f.setBoolean(this, java.lang.Boolean.parseBoolean(@value) || java.lang.JO.equals(@value, "1"));
                    }
                }
                catch (System.Exception e) when (e is System.Exception || e is System.FormatException)
                {
                    java.lang.IO.println("Params: cannot apply override '" + java.lang.JStr.valueOf(entry) + "': " + java.lang.JStr.valueOf(e));
                }
            }
        }
        // Java: 232
        public override string toString()
        {
            java.lang.StringBuilder sb = new java.lang.StringBuilder();
            foreach (java.lang.reflect.Field f in typeof(com.oddlabs.tt.player.fable.Params).getFields())
            {
                if (java.lang.reflect.Modifier.isStatic(f.getModifiers()))
                {
                    continue;
                }
                try
                {
                    sb.append(f.getName()).append('=').append(f.get(this)).append(' ');
                }
                catch (System.Exception unused)
                {
                }
            }
            return java.lang.JO.toString(sb).trim().toLowerCase(java.util.Locale.ROOT);
        }
        private void init_Params()
        {
            this.fast_period = 0.15f;
            this.slow_period = 0.5f;
            this.econ_period = 1.0f;
            this.armory_max_dist = 110;
            this.armory_min_dist = 12;
            this.armory_sector_deg = 75.0f;
            this.site_w_wood = 0.9f;
            this.site_w_iron = 0.45f;
            this.site_w_start = 0.12f;
            this.site_w_detour = 0.3f;
            this.armory_max_walk = 150;
            this.site_iron_radius = 60;
            this.site_min_iron_nodes = 8;
            this.site_penalty_few_iron = 40.0f;
            this.site_enemy_iron_factor = 2.0f;
            this.quarters_min_dist = 11;
            this.quarters_max_dist = 26;
            this.building_spacing = 11;
            this.tower_ring_radius = 12;
            this.tower_spacing_min = 7;
            this.quarters_min = 4;
            this.quarters_max = 5;
            this.towers_max = 12;
            this.quarters_first_crew = 17;
            this.opening_quarters = 4;
            this.opening_parallel = 2;
            this.breeders_early = 8;
            this.breeders_boom = 12;
            this.breeders_late = 6;
            this.breeders_rear = 1;
            this.boom_units = 190;
            this.breeders_training = 14;
            this.workers_min = 2;
            this.workers_surplus_max = 20;
            this.stock_target_wood_per_rate = 80.0f;
            this.stock_target_iron_per_rate = 40.0f;
            this.stock_wood_min = 6;
            this.stock_wood_max = 30;
            this.stock_iron_min = 3;
            this.stock_iron_max = 15;
            this.stock_rock_target = 6;
            this.rock_reserve = 8;
            this.max_peons_per_node = 4;
            this.gather_radius = 70;
            this.conversions_per_tick = 2;
            this.conversion_cooldown = 6.0f;
            this.max_supply_orders_per_tick = 4;
            this.cap_margin = 2;
            this.cap_workers = 16;
            this.cap_wood = 6;
            this.cap_iron = 4;
            this.cap_weapon_buffer = 20;
            this.hunt_max_dist = 125;
            this.hunt_max_peons = 5;
            this.hunt_min_time = 200;
            this.far_iron_m = 50.0f;
            this.hot_node_radius = 20;
            this.crew_quarters = 10;
            this.crew_tower = 6;
            this.crew_armory_rebuild = 25;
            this.crew_armory_opening = 14;
            this.expansion_armory = true;
            this.expand_min_time = 540.0f;
            this.expand_iron_m = 110.0f;
            this.expand_max_dist = 140;
            this.crew_armory_expand = 14;
            this.expand_retry_after = 400.0f;
            this.expand_workers = 16;
            this.site_fail_timeout = 120.0f;
            this.site_stall_timeout = 60.0f;
            this.site_retries = 3;
            this.helper_radius = 45;
            this.chieftain_min_peons = 40;
            this.chieftain_min_time = 300.0f;
            this.chieftain_min_quarters = 3;
            this.stun_radius_cells = 18.0f;
            this.stun_cast_dist = 15;
            this.counter_toot_progress = 0.85f;
            this.chief_escape_hp = 30;
            this.chief_fragile_hp = 24;
            this.chief_follow_dist = 6;
            this.blast_min_peons = 10;
            this.blast_max_own = 2;
            this.value_rock = 0.6f;
            this.value_iron = 1.0f;
            this.value_rubber = 1.6f;
            this.value_chief = 1.5f;
            this.value_tower_rock = 2.5f;
            this.value_tower_iron = 4.0f;
            this.value_tower_rubber = 6.0f;
            this.engage_ratio = 1.3f;
            this.hold_ratio = 0.8f;
            this.retreat_ratio = 0.55f;
            this.attack_ratio = 1.35f;
            this.attack_ratio_no_chief = 2.0f;
            this.attack_ratio_stun = 1.15f;
            this.attack_min_time = 420.0f;
            this.attack_min_army = 18;
            this.attack_max_army = 90.0f;
            this.capped_ratio = 1.0f;
            this.capped_total_ratio = 1.1f;
            this.capped_total_ratio_relaxed = 1.15f;
            this.cap_relax_time = 240.0f;
            this.attack_margin_per_march_s = 0.25f;
            this.retreat_cooldown = 60.0f;
            this.base_stun_value = 6.0f;
            this.stun_recovery = true;
            this.attack_margin = 6.0f;
            this.home_guard_early = 3;
            this.home_guard_mid = 5;
            this.home_guard_late = 7;
            this.home_guard_defend_bonus = 3;
            this.guard_mid_time = 480.0f;
            this.guard_late_time = 840.0f;
            this.reinforce_batch = 6;
            this.reinforce_batch_near = 3;
            this.leg_length = 30;
            this.leg_arrival_fraction = 0.85f;
            this.leg_arrival_radius = 10;
            this.leg_timeout = 25.0f;
            this.staging_dist = 18;
            this.threat_radius = 45;
            this.alert_eta = 45;
            this.engage_reorder_period = 4.0f;
            this.leash_dist = 15;
            this.all_in_units = 235;
            this.posture_dwell = 10.0f;
            this.tower_reach2 = 253;
            this.stand_min2 = 250;
            this.stand_max2 = 275;
            this.siege_min_army = 17;
            this.siege_camp_ratio = 0.4f;
            this.siege_camp_patience = 40.0f;
            this.field_sweep = true;
            this.sweep_radius = 130;
            this.defend_sweep_after = 60.0f;
            this.stun_window = 10.0f;
            this.defend_pursuit = 22.0f;
            this.corps = true;
            this.corps_size = 10;
            this.corps_min_army = 20;
            this.corps_min_demolish = 4;
            this.corps_follow_dist = 7;
        }
        public Params() : base()
        {
            init_Params();
        }
    }
}
