// Translated from Strategy.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.ai
{
    // Java: 10
    public class Strategy : java.lang.Object
    {
        public int initial_quarters;
        public int max_quarters;
        public float expand_time;
        public int max_armory_distance;
        public float armory_distance_weight;
        public float armory_delay_weight;
        public float armory_threat_weight;
        public int scouts;
        public int armory_builders;
        public int quarters_builders;
        public int tower_builders;
        public int quarters_before_armory;
        public bool opening_near_start;
        public bool rush_response;
        public int rush_quarters;
        public float rush_seconds;
        public bool pressure_response;
        public float pressure_time;
        public int hold_early;
        public int hold_mid;
        public int hold_late;
        public float hold_mid_time;
        public int hold_chieftain;
        public bool stun_patience;
        public bool exploit_stun;
        public bool charge_mixed;
        public bool dodge_stun;
        public bool blast;
        public float blast_ratio;
        public float blast_min;
        public bool blast_defense;
        public float blast_play_time;
        public bool blast_save;
        public bool hunt_caster;
        public int hunt_caster_cells;
        public bool defend_exploit_stun;
        public bool dodge_fog;
        public bool native_lightning;
        public bool chief_tower_standoff;
        public bool stun_learn;
        public float dodge_catch;
        public int dodge_core;
        public bool tower_stun_follow_up;
        public int chieftain_min_quarters;
        public float chieftain_time;
        public bool multi_front_towers;
        public int towers_early;
        public int towers_mid;
        public int towers_late;
        public float towers_early_time;
        public float towers_mid_time;
        public float towers_late_time;
        public float attack_min_strength;
        public float attack_ratio;
        public bool reinforce;
        public float reinforce_ratio;
        public bool reinforce_multi;
        public float tower_weight;
        public float attack_max_strength;
        public float capped_ratio;
        public float retreat_ratio;
        public int raid_size;
        public float raid_time;
        public bool engage_spread;
        public bool project_defense;
        public float precontact_ratio;
        public bool strikes;
        public int forward_towers;
        public float forward_tower_time;
        public float forward_ratio;
        public float defend_hysteresis;
        public float response_ratio;
        public bool gather_under_threat;
        public bool gather_threat_1v1;
        public float stuck_trip_factor;
        public bool expansion;
        public bool far_expansion;
        public bool recall_old_gatherers;
        public float desperate_expansion;
        public float desperate_iron_cycle;
        public float armory_far_cost;
        public bool peon_militia;
        public float militia_time;
        public bool peon_rush;
        public bool micro_targets;
        public bool restore_dodge;
        public bool sappers;
        public bool pillage;
        public bool siege;
        public float siege_patience;
        public bool adaptive_caution;
        public float caution_decay;
        public bool creep_towers;
        public bool hidden_info;
        public int chicken_hunters;
        public float chicken_time;
        public int chicken_pool_div;
        public bool tower_fire;
        public bool unstick;
        public int chief_keep_out;
        public int chief_safe;
        public int chief_flee_hp;
        public int threat_look;
        public float hold_ratio;
        public bool chief_per_hit;
        public int base_radius;
        // Java: 339
        public virtual void @override(string assignments)
        {
            foreach (string pair in assignments.split(","))
            {
                if (pair.isBlank())
                {
                    continue;
                }
                string[] kv = pair.split("=");
                try
                {
                    java.lang.reflect.Field f = typeof(com.oddlabs.tt.player.ai.Strategy).getDeclaredField(kv[0].trim());
                    string v = kv[1].trim();
                    if (((object)f.getType() == (object)typeof(int)))
                    {
                        f.setInt(this, java.lang.Integer.parseInt(v));
                    }
                    else if (((object)f.getType() == (object)typeof(float)))
                    {
                        f.setFloat(this, java.lang.Float.parseFloat(v));
                    }
                    else if (((object)f.getType() == (object)typeof(bool)))
                    {
                        f.setBoolean(this, java.lang.Boolean.parseBoolean(v));
                    }
                }
                catch (System.Exception e)
                {
                    throw new System.ArgumentException("bad strategy override " + java.lang.JStr.valueOf(pair), (System.Exception)e);
                }
            }
        }
        // Java: 364
        public static com.oddlabs.tt.player.ai.Strategy forWorld(com.oddlabs.tt.landscape.World world, int enemies)
        {
            com.oddlabs.tt.player.ai.Strategy strategy = com.oddlabs.tt.player.ai.Strategy.forGame(world.getMapSize(), enemies);
            if (world.isExtended())
            {
                strategy.towers_early = java.lang.Math.max(strategy.towers_early, 2);
                strategy.towers_early_time = java.lang.Math.min(strategy.towers_early_time, 120.0f);
            }
            return strategy;
        }
        // Java: 374
        public static com.oddlabs.tt.player.ai.Strategy forGame(int map_size, int enemies)
        {
            com.oddlabs.tt.player.ai.Strategy strategy = com.oddlabs.tt.player.ai.Strategy.forMapSize(map_size);
            if (enemies > 1)
            {
                strategy.towers_early = 3;
                strategy.towers_early_time = java.lang.Math.min(strategy.towers_early_time, 200.0f);
                strategy.towers_mid = 6;
                strategy.towers_mid_time = java.lang.Math.min(strategy.towers_mid_time, 330.0f);
                strategy.towers_late = 14;
                strategy.towers_late_time = 600.0f;
                strategy.chieftain_time = java.lang.Math.min(strategy.chieftain_time, 240.0f);
            }
            return strategy;
        }
        // Java: 390
        public static com.oddlabs.tt.player.ai.Strategy forMapSize(int map_size)
        {
            com.oddlabs.tt.player.ai.Strategy strategy = new com.oddlabs.tt.player.ai.Strategy();
            switch ((map_size))
            {
                case 0:
                case 1:
                {
                    strategy.initial_quarters = 2;
                    strategy.quarters_before_armory = 2;
                    strategy.hold_mid = 7;
                    strategy.max_armory_distance = 230;
                    strategy.armory_distance_weight = 0.06f;
                    strategy.armory_delay_weight = 0.6f;
                    strategy.armory_threat_weight = 90.0f;
                    strategy.hold_early = 2;
                    strategy.expand_time = 240.0f;
                    strategy.towers_early_time = 150.0f;
                    strategy.attack_min_strength = 12.0f;
                    strategy.chieftain_time = 300.0f;
                    strategy.expansion = false;
                    strategy.towers_mid = 4;
                    strategy.towers_late = 8;
                    break;
                }
                case 3:
                {
                    strategy.max_armory_distance = 700;
                    strategy.expand_time = 360.0f;
                    break;
                }
                default:
                {
                    break;
                }
            }
            return strategy;
        }
        private void init_Strategy()
        {
            this.initial_quarters = 4;
            this.max_quarters = 4;
            this.expand_time = 300.0f;
            this.max_armory_distance = 260;
            this.armory_distance_weight = 0.03f;
            this.armory_delay_weight = 0.4f;
            this.armory_threat_weight = 60.0f;
            this.scouts = 1;
            this.armory_builders = 16;
            this.quarters_builders = 12;
            this.tower_builders = 8;
            this.quarters_before_armory = 4;
            this.opening_near_start = false;
            this.rush_response = true;
            this.rush_quarters = 2;
            this.rush_seconds = 240.0f;
            this.pressure_response = true;
            this.pressure_time = 720.0f;
            this.hold_early = 4;
            this.hold_mid = 14;
            this.hold_late = 8;
            this.hold_mid_time = 240.0f;
            this.hold_chieftain = 14;
            this.stun_patience = true;
            this.exploit_stun = true;
            this.charge_mixed = false;
            this.dodge_stun = true;
            this.blast = false;
            this.blast_ratio = 6.0f;
            this.blast_min = 12.0f;
            this.blast_defense = false;
            this.blast_play_time = 14.0f;
            this.blast_save = false;
            this.hunt_caster = true;
            this.hunt_caster_cells = 11;
            this.defend_exploit_stun = false;
            this.dodge_fog = true;
            this.native_lightning = false;
            this.chief_tower_standoff = true;
            this.stun_learn = false;
            this.dodge_catch = 0.6f;
            this.dodge_core = 10;
            this.tower_stun_follow_up = true;
            this.chieftain_min_quarters = 3;
            this.chieftain_time = 330.0f;
            this.multi_front_towers = true;
            this.towers_early = 1;
            this.towers_mid = 3;
            this.towers_late = 6;
            this.towers_early_time = 420.0f;
            this.towers_mid_time = 420.0f;
            this.towers_late_time = 720.0f;
            this.attack_min_strength = 18.0f;
            this.attack_ratio = 1.35f;
            this.reinforce = true;
            this.reinforce_ratio = 0.5f;
            this.reinforce_multi = false;
            this.tower_weight = 1.0f;
            this.attack_max_strength = 70.0f;
            this.capped_ratio = 0.6f;
            this.retreat_ratio = 1.45f;
            this.raid_size = 5;
            this.raid_time = 360.0f;
            this.engage_spread = true;
            this.project_defense = true;
            this.precontact_ratio = 1.1f;
            this.strikes = true;
            this.forward_towers = 0;
            this.forward_tower_time = 420.0f;
            this.forward_ratio = 1.4f;
            this.defend_hysteresis = 0.15f;
            this.response_ratio = 2.0f;
            this.gather_under_threat = true;
            this.gather_threat_1v1 = false;
            this.stuck_trip_factor = 0.0f;
            this.expansion = true;
            this.far_expansion = false;
            this.recall_old_gatherers = true;
            this.desperate_expansion = 0.9f;
            this.desperate_iron_cycle = 150.0f;
            this.armory_far_cost = 170.0f;
            this.peon_militia = true;
            this.militia_time = 600.0f;
            this.peon_rush = false;
            this.micro_targets = true;
            this.restore_dodge = false;
            this.sappers = true;
            this.pillage = false;
            this.siege = false;
            this.siege_patience = 100.0f;
            this.adaptive_caution = true;
            this.caution_decay = 1.1f;
            this.creep_towers = false;
            this.hidden_info = false;
            this.chicken_hunters = 7;
            this.chicken_time = 150.0f;
            this.chicken_pool_div = 18;
            this.tower_fire = true;
            this.unstick = true;
            this.chief_keep_out = 11;
            this.chief_safe = 0;
            this.chief_flee_hp = 24;
            this.threat_look = 30;
            this.hold_ratio = 0.0f;
            this.chief_per_hit = true;
            this.base_radius = 28;
        }
        public Strategy() : base()
        {
            init_Strategy();
        }
    }
}
