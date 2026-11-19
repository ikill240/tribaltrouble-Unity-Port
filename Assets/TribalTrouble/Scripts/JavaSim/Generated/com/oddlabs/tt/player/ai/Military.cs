// Translated from Military.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.ai
{
    // Java: 28
    public class Military : java.lang.Object
    {
        // Java: 29
        public sealed class Mode : java.lang.Enum
        {
            public static Mode[] values() { return (Mode[])VALUES_.Clone(); }
            public static Mode valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly Mode HOME = new Mode("HOME", 0);
            public static readonly Mode MUSTER = new Mode("MUSTER", 1);
            public static readonly Mode ATTACK = new Mode("ATTACK", 2);
            public static readonly Mode RETREAT = new Mode("RETREAT", 3);
            public static readonly Mode[] VALUES_ = new Mode[] { HOME, MUSTER, ATTACK, RETREAT };
            public Mode(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        // Java: 36
        public sealed class Role : java.lang.Enum
        {
            public static Role[] values() { return (Role[])VALUES_.Clone(); }
            public static Role valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly Role ARMY = new Role("ARMY", 0);
            public static readonly Role TOWER = new Role("TOWER", 1);
            public static readonly Role ATTACK = new Role("ATTACK", 2);
            public static readonly Role REINFORCE = new Role("REINFORCE", 3);
            public static readonly Role RAID = new Role("RAID", 4);
            public static readonly Role[] VALUES_ = new Role[] { ARMY, TOWER, ATTACK, REINFORCE, RAID };
            public Role(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        public const int ENGAGE_RADIUS = 22;
        public const float REORDER_PERIOD = 2.5f;
        public com.oddlabs.tt.player.ai.ExpertAI ai;
        public java.util.Map roles;
        public java.util.Map tower_assignments;
        public java.util.Map last_order;
        public java.util.Map chief_orders;
        public java.util.Map last_spots;
        public java.util.Map pending_orders;
        public java.util.ArrayDeque enemy_history;
        public com.oddlabs.tt.player.ai.Military.Mode mode_f;
        public int staging_x;
        public int staging_y;
        public float staging_time;
        public java.util.List threats;
        public java.util.List raiders;
        public float threat_strength;
        public float base_threat_strength;
        public int threat_x;
        public int threat_y;
        public int threat_level;
        public float last_threat_time;
        public int last_logged_threat;
        public bool last_logged_engage;
        public int engage_state;
        public com.oddlabs.tt.model.Selectable target;
        public int target_x;
        public int target_y;
        public com.oddlabs.tt.player.ai.DistanceField target_field;
        public float attack_start;
        public float attack_initial_strength;
        public float muster_start;
        public float next_wave_time;
        public float last_trace;
        public float hold_until;
        public float last_progress_time;
        public int best_target_dist;
        public int[] hold_spot;
        public int last_enemy_d2;
        public float last_charge_log;
        public java.util.Map hunt_targets;
        public java.util.Map dodge_orders;
        public java.util.Map tower_targets;
        public java.util.Map sapper_orders;
        public float last_pillage_log;
        public java.util.Map enemy_casts;
        public int own_stunned_before;
        public const float SPELL_RECHARGE = 40.0f;
        public const float STUN_WINDUP = 3.7f;
        public const float STUN_RADIUS = 36.0f;
        public const float STUN_OFFSET = 2.57f;
        // Java: 111
        public class Dodge : java.lang.Object
        {
            public int x() { return x_f; }
            public int y() { return y_f; }
            public float until() { return until_f; }
            public Dodge(int x, int y, float until)
            {
                this.x_f = x;
                this.y_f = y;
                this.until_f = until;
            }
            public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is Dodge r)) return false; return true && java.lang.JO.equals(x_f, r.x_f) && java.lang.JO.equals(y_f, r.y_f) && java.lang.JO.equals(until_f, r.until_f); }
            public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(x_f); h = 31 * h + java.lang.JO.hashCode(y_f); h = 31 * h + java.lang.JO.hashCode(until_f); } return h; }
            public override string toString() { return "Dodge[" + "x=" + java.lang.JO.toString(x_f) + ", y=" + java.lang.JO.toString(y_f) + ", until=" + java.lang.JO.toString(until_f) + "]"; }
            public int x_f;
            public int y_f;
            public float until_f;
        }
        public java.util.Map dodges;
        public java.util.Map caster_hunters;
        public float siege_start;
        public float siege_cooldown;
        public float blast_play_until;
        public float last_blast_play;
        public float siege_progress;
        public float last_pull_log;
        public java.util.Map siege_assign;
        public const int SIEGE_HOLD = 17;
        public const int TOWER_REACH = 16;
        public java.util.Map caster_targets;
        public java.util.Map windups;
        // Java: 136
        public class Fog : java.lang.Object
        {
            public com.oddlabs.tt.model.Unit caster;
            public float x;
            public float y;
            public float cast;
            public bool confirmed;
            public Fog(com.oddlabs.tt.model.Unit caster, float x, float y, float cast) : base()
            {
                this.caster = caster;
                this.x = x;
                this.y = y;
                this.cast = cast;
            }
        }
        public java.util.List fogs;
        public const float FOG_RELEASE = 3.64f;
        public const float FOG_TIME = 20.0f;
        public const float FOG_RADIUS = 26.0f;
        public const float FOG_OFFSET = 0.9f;
        public com.oddlabs.tt.model.Building creep_site;
        public java.util.List creep_towers;
        public float last_creep_try;
        public float attack_caution;
        public float last_caution_ease;
        public int attack_kills_start;
        public int attack_losses_start;
        public bool attack_running;
        public java.util.Map militia_orders;
        public float last_militia_log;
        public float last_peon_rush;
        public bool strike;
        public int escort_x;
        public int escort_y;
        public float escort_time;
        public int raid_x;
        public int raid_y;
        public float raid_start;
        public float last_raid_end;
        // Java: 197
        public int baseThreatLevel()
        {
            return threat_level;
        }
        // Java: 201
        public bool threatNear(int x, int y, int radius)
        {
            int r2 = radius * radius;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(threats))
            {
                if (!u.isDead() && com.oddlabs.tt.player.ai.MapAnalysis.dist2(x, y, u.getGridX(), u.getGridY()) <= r2)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 209
        public float enemyStrengthNear(int x, int y, int radius)
        {
            return (float)(com.oddlabs.tt.player.ai.Combat.strengthNear(ai.intel().enemy_warriors, x, y, radius) + com.oddlabs.tt.player.ai.Combat.strengthNear(ai.intel().enemy_chieftains, x, y, radius));
        }
        // Java: 215
        public float enemyFightersNear(int x, int y, int radius)
        {
            return (float)(enemyStrengthNear(x, y, radius) + com.oddlabs.tt.player.ai.Combat.strengthNear(ai.intel().enemy_peons, x, y, radius));
        }
        // Java: 220
        public bool wantsEverything()
        {
            return threat_level >= 2 || ((object)mode_f == (object)com.oddlabs.tt.player.ai.Military.Mode.MUSTER);
        }
        // Java: 224
        public int towerSeatsFree()
        {
            int n = 0;
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(ai.intel().towers))
            {
                if (!com.oddlabs.tt.player.ai.Intel.isTowerManned(t) && !tower_assignments.containsValue(t))
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 232
        public float armyStrength()
        {
            float s = 0.0f;
            foreach (java.util.Map.Entry e in java.lang.JIter.of(roles.entrySet()))
            {
                if (((object)((com.oddlabs.tt.player.ai.Military.Role)e.getValue()) == (object)com.oddlabs.tt.player.ai.Military.Role.ARMY) || ((object)((com.oddlabs.tt.player.ai.Military.Role)e.getValue()) == (object)com.oddlabs.tt.player.ai.Military.Role.RAID))
                {
                    s = (float)(s + com.oddlabs.tt.player.ai.Combat.@value(((com.oddlabs.tt.model.Unit)e.getKey())));
                }
            }
            return s;
        }
        // Java: 241
        public float armyStrengthWanted()
        {
            float enemy = enemyFieldStrength();
            float wanted = java.lang.Math.max((float)(5.0f + (float)(ai.time() / 90.0f)), (float)(0.75f * enemy));
            return java.lang.Math.min(wanted, 45.0f);
        }
        // Java: 248
        public float enemyFieldStrengthNear(int x, int y, int radius)
        {
            int r2 = radius * radius;
            float s = 0.0f;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(ai.intel().enemy_warriors))
            {
                s = (float)(s + (float)(com.oddlabs.tt.player.ai.Combat.@value(u) * ((com.oddlabs.tt.player.ai.MapAnalysis.dist2(x, y, u.getGridX(), u.getGridY()) <= r2 ? 1.0f : 0.3f))));
            }
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(ai.intel().enemy_chieftains))
            {
                s = (float)(s + (float)(com.oddlabs.tt.player.ai.Combat.@value(u_2) * ((com.oddlabs.tt.player.ai.MapAnalysis.dist2(x, y, u_2.getGridX(), u_2.getGridY()) <= r2 ? 1.0f : 0.3f))));
            }
            return s;
        }
        // Java: 258
        public float enemyFieldStrength()
        {
            float s = 0.0f;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(ai.intel().enemy_warriors))
            {
                s = (float)(s + com.oddlabs.tt.player.ai.Combat.@value(u));
            }
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(ai.intel().enemy_chieftains))
            {
                s = (float)(s + com.oddlabs.tt.player.ai.Combat.@value(u_2));
            }
            return s;
        }
        // Java: 270
        public void tick()
        {
            watchEnemyCasts();
            updateRoles();
            updateStaging();
            updateThreat();
            manTowers();
            if (threat_level > 0)
            {
                defend();
            }
            if (!raiders.isEmpty())
            {
                answerRaiders();
            }
            if (((object)mode_f != (object)com.oddlabs.tt.player.ai.Military.Mode.ATTACK) && siege_start >= 0.0f)
            {
                endSiege();
            }
            switch ((mode_f).ordinal())
            {
                case 0 /*HOME*/:
                {
                    holdStaging();
                    break;
                }
                case 1 /*MUSTER*/:
                {
                    muster();
                    break;
                }
                case 2 /*ATTACK*/:
                {
                    attack();
                    break;
                }
                case 3 /*RETREAT*/:
                {
                    retreat();
                    break;
                }
            }
            reinforce();
            raid();
            peonRush();
            restoreDodge();
            towerFire();
            sapperTick();
            flushOrders();
        }
        // Java: 297
        public void plan()
        {
            updateRoles();
            recordEnemyStrength();
            easeCaution();
            if (((object)mode_f == (object)com.oddlabs.tt.player.ai.Military.Mode.HOME) && threat_level < 2 && ai.strategy().strikes)
            {
                considerStrike();
            }
            if (((object)mode_f == (object)com.oddlabs.tt.player.ai.Military.Mode.HOME) && threat_level < 2)
            {
                considerAttack();
            }
            if (((object)mode_f == (object)com.oddlabs.tt.player.ai.Military.Mode.ATTACK) && threat_level < 2 && ai.strategy().reinforce)
            {
                considerReinforcing();
            }
            if (((object)mode_f == (object)com.oddlabs.tt.player.ai.Military.Mode.HOME) && threat_level == 0)
            {
                considerRaid();
            }
        }
        // Java: 311
        public void updateRoles()
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            for (java.util.Iterator it = roles.entrySet().iterator(); it.hasNext(); )
            {
                java.util.Map.Entry e = ((java.util.Map.Entry)it.next());
                com.oddlabs.tt.model.Unit u = ((com.oddlabs.tt.model.Unit)e.getKey());
                if (u.isDead() || u.isMounted())
                {
                    it.remove();
                    tower_assignments.remove(u);
                    last_order.remove(u);
                    last_spots.remove(u);
                    chief_orders.remove(u);
                }
            }
            for (java.util.Iterator it_2 = tower_assignments.entrySet().iterator(); it_2.hasNext(); )
            {
                java.util.Map.Entry e_2 = ((java.util.Map.Entry)it_2.next());
                com.oddlabs.tt.model.Unit u_2 = ((com.oddlabs.tt.model.Unit)e_2.getKey());
                com.oddlabs.tt.model.Building t = ((com.oddlabs.tt.model.Building)e_2.getValue());
                if (u_2.isDead() || u_2.isMounted() || t.isDead() || com.oddlabs.tt.player.ai.Intel.isTowerManned(t) || ((object)((com.oddlabs.tt.player.ai.Intel.WarriorState)intel.warrior_states.get(u_2)) != (object)com.oddlabs.tt.player.ai.Intel.WarriorState.ENTER))
                {
                    if (!u_2.isDead() && !u_2.isMounted() && ((object)((com.oddlabs.tt.player.ai.Military.Role)roles.get(u_2)) == (object)com.oddlabs.tt.player.ai.Military.Role.TOWER))
                    {
                        roles.put(u_2, com.oddlabs.tt.player.ai.Military.Role.ARMY);
                    }
                    it_2.remove();
                }
            }
            foreach (com.oddlabs.tt.model.Unit w in java.lang.JIter.of(intel.warriors))
            {
                if (!roles.containsKey(w))
                {
                    roles.put(w, com.oddlabs.tt.player.ai.Military.Role.ARMY);
                }
            }
        }
        // Java: 341
        public void updateStaging()
        {
            com.oddlabs.tt.model.Building armory = ai.intel().armory();
            if (armory == null)
            {
                if (!ai.intel().quarters.isEmpty())
                {
                    com.oddlabs.tt.model.Building q = ((com.oddlabs.tt.model.Building)ai.intel().quarters.getFirst());
                    staging_x = q.getGridX();
                    staging_y = q.getGridY();
                }
                return;
            }
            if ((float)(ai.time() - staging_time) < 30.0f)
            {
                return;
            }
            staging_time = ai.time();
            int[] p = ai.planner().getEnemyField().stepTowardsSource(armory.getGridX(), armory.getGridY(), 26);
            staging_x = p[0];
            staging_y = p[1];
        }
        // Java: 362
        public void updateThreat()
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            threats.clear();
            raiders.clear();
            int radius = ai.strategy().base_radius;
            int r2 = radius * radius;
            java.util.List own = new java.util.ArrayList();
            own.addAll(intel.armories);
            own.addAll(intel.quarters);
            own.addAll(intel.towers);
            own.addAll(intel.armory_sites);
            own.addAll(intel.quarters_sites);
            own.addAll(intel.tower_sites);
            java.util.List enemies = new java.util.ArrayList(intel.enemy_warriors);
            enemies.addAll(intel.enemy_chieftains);
            java.util.List at_base = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(enemies))
            {
                bool near_base = false;
                foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(own))
                {
                    if (com.oddlabs.tt.player.ai.MapAnalysis.dist2(e.getGridX(), e.getGridY(), b.getGridX(), b.getGridY()) <= r2)
                    {
                        near_base = true;
                        break;
                    }
                }
                bool near_peons = false;
                if (!near_base)
                {
                    foreach (com.oddlabs.tt.model.Unit p in java.lang.JIter.of(intel.peons))
                    {
                        com.oddlabs.tt.player.ai.Intel.PeonState s = ((com.oddlabs.tt.player.ai.Intel.PeonState)intel.peon_states.get(p));
                        if (((object)s == (object)com.oddlabs.tt.player.ai.Intel.PeonState.GATHER_CHICKEN))
                        {
                            continue;
                        }
                        if (com.oddlabs.tt.player.ai.MapAnalysis.dist2(e.getGridX(), e.getGridY(), p.getGridX(), p.getGridY()) <= 12 * 12)
                        {
                            near_peons = true;
                            break;
                        }
                    }
                }
                if (near_base)
                {
                    threats.add(e);
                    at_base.add(e);
                }
                else if (near_peons)
                {
                    threats.add(e);
                }
            }
            foreach (com.oddlabs.tt.model.Unit e_2 in java.lang.JIter.of(intel.enemy_peons))
            {
                bool hostile = false;
                foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(intel.towers))
                {
                    if (com.oddlabs.tt.player.ai.MapAnalysis.dist2(e_2.getGridX(), e_2.getGridY(), t.getGridX(), t.getGridY()) <= 5 * 5)
                    {
                        hostile = true;
                        break;
                    }
                }
                if (hostile)
                {
                    threats.add(e_2);
                    at_base.add(e_2);
                }
                else if (ai.strategy().peon_militia && ai.time() < ai.strategy().militia_time && raiding(e_2, own, r2))
                {
                    raiders.add(e_2);
                }
            }
            if (threats.isEmpty())
            {
                if (threat_level != 0)
                {
                    ai.log("threat over");
                }
                threat_level = 0;
                engage_state = 0;
                threat_strength = 0.0f;
                base_threat_strength = 0.0f;
                last_logged_threat = 0;
                return;
            }
            last_threat_time = ai.time();
            base_threat_strength = 0.0f;
            foreach (com.oddlabs.tt.model.Unit e_3 in java.lang.JIter.of(at_base))
            {
                base_threat_strength += com.oddlabs.tt.player.ai.Combat.@value(e_3);
            }
            com.oddlabs.tt.model.Building armory = intel.armory();
            float best = (float)(-1.0f);
            foreach (com.oddlabs.tt.model.Unit seed in java.lang.JIter.of(threats))
            {
                long sx = 0;
                long sy = 0;
                int n = 0;
                float strength = 0.0f;
                bool @base = false;
                foreach (com.oddlabs.tt.model.Unit e_4 in java.lang.JIter.of(threats))
                {
                    if (com.oddlabs.tt.player.ai.MapAnalysis.dist2(seed.getGridX(), seed.getGridY(), e_4.getGridX(), e_4.getGridY()) <= 15 * 15)
                    {
                        sx += e_4.getGridX();
                        sy += e_4.getGridY();
                        n++;
                        strength = (float)(strength + java.lang.Math.max(0.2f, com.oddlabs.tt.player.ai.Combat.@value(e_4)));
                        @base |= at_base.contains(e_4);
                    }
                }
                int cx = unchecked((int)((sx / n)));
                int cy = unchecked((int)((sy / n)));
                float weight = (float)(strength * ((@base ? 2.0f : 1.0f)));
                if (armory != null && com.oddlabs.tt.player.ai.MapAnalysis.dist2(cx, cy, armory.getGridX(), armory.getGridY()) <= 25 * 25)
                {
                    weight = (float)(weight * 1.5f);
                }
                if (weight > best)
                {
                    best = weight;
                    threat_x = cx;
                    threat_y = cy;
                    threat_strength = strength;
                }
            }
            bool at_armory = armory != null && com.oddlabs.tt.player.ai.MapAnalysis.dist2(threat_x, threat_y, armory.getGridX(), armory.getGridY()) <= 25 * 25;
            threat_level = (base_threat_strength >= 3.0f || at_armory ? 2 : 1);
        }
        // Java: 473
        public void defend()
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            com.oddlabs.tt.model.Building armory = intel.armory();
            float home = (float)(armyStrength() + towersStrength());
            if ((((object)mode_f == (object)com.oddlabs.tt.player.ai.Military.Mode.ATTACK) || ((object)mode_f == (object)com.oddlabs.tt.player.ai.Military.Mode.MUSTER)) && base_threat_strength > home && base_threat_strength > (float)(0.35f * attackStrength()))
            {
                ai.log(java.lang.JString.format("calling the army home: %.1f in the base against %.1f", base_threat_strength, home));
                endAttack();
            }
            java.util.List defenders = new java.util.ArrayList();
            foreach (java.util.Map.Entry e in java.lang.JIter.of(roles.entrySet()))
            {
                com.oddlabs.tt.player.ai.Military.Role r = ((com.oddlabs.tt.player.ai.Military.Role)e.getValue());
                com.oddlabs.tt.model.Unit u = ((com.oddlabs.tt.model.Unit)e.getKey());
                if (((object)r == (object)com.oddlabs.tt.player.ai.Military.Role.ARMY) || (((object)r == (object)com.oddlabs.tt.player.ai.Military.Role.RAID) && threat_level >= 2))
                {
                    defenders.add(u);
                }
            }
            float response = ai.strategy().response_ratio;
            if (threat_level < 2 && response > 0.0f)
            {
                defenders.sort(new java.util.Comparator_L((object a_l, object b_l) =>
                {
                    com.oddlabs.tt.model.Unit a = (com.oddlabs.tt.model.Unit)a_l;
                    com.oddlabs.tt.model.Unit b = (com.oddlabs.tt.model.Unit)b_l;
                    return java.lang.Integer.compare(com.oddlabs.tt.player.ai.MapAnalysis.dist2(a.getGridX(), a.getGridY(), threat_x, threat_y), com.oddlabs.tt.player.ai.MapAnalysis.dist2(b.getGridX(), b.getGridY(), threat_x, threat_y));
                }));
                float needed = (float)((float)(response * threat_strength) + 2.0f);
                float picked = 0.0f;
                int n = 0;
                while (n < defenders.size() && picked < needed)
                {
                    picked = (float)(picked + com.oddlabs.tt.player.ai.Combat.@value(((com.oddlabs.tt.model.Unit)defenders.get(n++))));
                }
                defenders = new java.util.ArrayList(defenders.subList(0, n));
            }
            float ours = 0.0f;
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(defenders))
            {
                ours = (float)(ours + com.oddlabs.tt.player.ai.Combat.@value(u_2));
            }
            float towers = 0.0f;
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(intel.towers))
            {
                if (com.oddlabs.tt.player.ai.MapAnalysis.dist2(t.getGridX(), t.getGridY(), threat_x, threat_y) <= 16 * 16)
                {
                    towers = (float)(towers + com.oddlabs.tt.player.ai.Combat.towerValue(t));
                }
            }
            com.oddlabs.tt.model.Unit chief = intel.chieftain;
            bool toot_ready = chief != null && ai.chieftain().stunReady();
            float effective = (float)((float)(ours + towers) + ((toot_ready ? (float)(0.6f * threat_strength) : 0.0f)));
            float behind = (ai.strategy().threat_look > 0 && ai.enemiesAlive() == 1 ? enemyStrengthNear(threat_x, threat_y, ai.strategy().threat_look) : 0.0f);
            float enemy = (float)(java.lang.Math.max(threat_strength, behind) * ((enemyStunReadyNear(threat_x, threat_y, 25) ? 1.5f : 1.0f)));
            float engage_ratio = (float)(0.8f - (float)(engage_state * ai.strategy().defend_hysteresis));
            bool engage = effective >= (float)(engage_ratio * enemy) || (armory != null && com.oddlabs.tt.player.ai.MapAnalysis.dist2(threat_x, threat_y, armory.getGridX(), armory.getGridY()) <= 14 * 14);
            if (threat_level != last_logged_threat || engage != last_logged_engage)
            {
                last_logged_threat = threat_level;
                last_logged_engage = engage;
                ai.log(java.lang.JString.format("threat %d: %.1f at %d,%d; defenders %.1f towers %.1f -> %s", threat_level, threat_strength, threat_x, threat_y, ours, towers, (engage ? (object)("engage") : (object)("fall back"))));
            }
            engage_state = (engage ? 1 : (-1));
            if (threat_level >= 2)
            {
                com.oddlabs.tt.player.ai.ExpertAI.debug_battle = new int[] { threat_x, threat_y };
            }
            if (ai.strategy().defend_exploit_stun && engage && chargeStunned(defenders, threat_x, threat_y))
            {
                evacuatePeons();
                return;
            }
            if (blastDefense(defenders, enemy))
            {
                evacuatePeons();
                return;
            }
            float hold = ai.strategy().hold_ratio;
            if (engage && hold > 0.0f && ai.enemiesAlive() == 1 && enemy >= (float)(hold * java.lang.Math.max(1.0f, ours)) && holdAtPost(defenders))
            {
                evacuatePeons();
                return;
            }
            if (engage)
            {
                foreach (com.oddlabs.tt.model.Unit u_3 in java.lang.JIter.of(defenders))
                {
                    if (((object)((com.oddlabs.tt.player.ai.Military.Role)roles.get(u_3)) == (object)com.oddlabs.tt.player.ai.Military.Role.RAID))
                    {
                        roles.put(u_3, com.oddlabs.tt.player.ai.Military.Role.ARMY);
                    }
                }
                engageSpread(defenders, threats, threat_x, threat_y, false);
                huntChieftains(defenders);
            }
            else if (armory != null)
            {
                foreach (com.oddlabs.tt.model.Unit u_4 in java.lang.JIter.of(defenders))
                {
                    attackGround(u_4, armory.getGridX(), armory.getGridY(), false);
                }
            }
            evacuatePeons();
        }
        // Java: 566
        public bool holdAtPost(java.util.List defenders)
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            com.oddlabs.tt.model.Building post = null;
            int best = java.lang.Integer.MAX_VALUE;
            java.util.List own = new java.util.ArrayList(intel.towers);
            own.addAll(intel.armories);
            own.addAll(intel.quarters);
            foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(own))
            {
                int d = com.oddlabs.tt.player.ai.MapAnalysis.dist2(b.getGridX(), b.getGridY(), threat_x, threat_y);
                if (((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID() == com.oddlabs.tt.model.Race.BUILDING_TOWER && com.oddlabs.tt.player.ai.Intel.isTowerActive(b))
                {
                    d -= 10 * 10;
                }
                if (d < best)
                {
                    best = d;
                    post = b;
                }
            }
            if (post == null)
            {
                return false;
            }
            int px = post.getGridX();
            int py = post.getGridY();
            java.util.List close = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(threats))
            {
                if (!e.isDead() && com.oddlabs.tt.player.ai.MapAnalysis.dist2(e.getGridX(), e.getGridY(), px, py) <= 14 * 14)
                {
                    close.add(e);
                }
            }
            java.util.List at_post = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(defenders))
            {
                if (((object)((com.oddlabs.tt.player.ai.Military.Role)roles.get(u)) == (object)com.oddlabs.tt.player.ai.Military.Role.RAID))
                {
                    roles.put(u, com.oddlabs.tt.player.ai.Military.Role.ARMY);
                }
                if (com.oddlabs.tt.player.ai.MapAnalysis.dist2(u.getGridX(), u.getGridY(), px, py) <= 12 * 12)
                {
                    at_post.add(u);
                }
                else
                {
                    attackGround(u, px, py, false);
                }
            }
            if (!close.isEmpty())
            {
                engageSpread(at_post, close, px, py, false);
            }
            huntChieftains(at_post);
            return true;
        }
        // Java: 606
        public float towersStrength()
        {
            float s = 0.0f;
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(ai.intel().towers))
            {
                s = (float)(s + com.oddlabs.tt.player.ai.Combat.towerValue(t));
            }
            return s;
        }
        // Java: 614
        public bool enemyStunReadyNear(int x, int y, int radius)
        {
            foreach (com.oddlabs.tt.model.Unit c in java.lang.JIter.of(ai.intel().enemy_chieftains))
            {
                if (c.isDead() || com.oddlabs.tt.player.ai.Intel.isStunned(c))
                {
                    continue;
                }
                if (com.oddlabs.tt.player.ai.MapAnalysis.dist2(x, y, c.getGridX(), c.getGridY()) > radius * radius)
                {
                    continue;
                }
                if (enemySpellReady(c))
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 630
        public bool enemySpellReady(com.oddlabs.tt.model.Unit chief)
        {
            if (ai.strategy().hidden_info)
            {
                return chief.getMagicProgress(0) >= 1.0f || chief.getMagicProgress(1) >= 1.0f;
            }
            float? cast = ((float?)enemy_casts.get(chief));
            return cast == null || (float)(ai.time() - ((float)cast)) >= com.oddlabs.tt.player.ai.Military.SPELL_RECHARGE;
        }
        // Java: 641
        public void watchEnemyCasts()
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            enemy_casts.keySet().removeIf(new java.util.function.Predicate_L((object a0) => { return ((com.oddlabs.tt.model.Unit)a0).isDead(); }));
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(intel.enemy_chieftains))
            {
                if (!e.isDead() && (e.getCurrentController() is com.oddlabs.tt.model.behaviour.MagicController))
                {
                    enemy_casts.put(e, ai.time());
                }
            }
            java.util.List stunned = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(intel.warriors))
            {
                if (com.oddlabs.tt.player.ai.Intel.isStunned(u))
                {
                    stunned.add(u);
                }
            }
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(intel.peons))
            {
                if (com.oddlabs.tt.player.ai.Intel.isStunned(u_2))
                {
                    stunned.add(u_2);
                }
            }
            if (stunned.size() >= own_stunned_before + 2)
            {
                int[] c = com.oddlabs.tt.player.ai.Military.centroid(stunned);
                com.oddlabs.tt.model.Unit caster = null;
                int best = 25 * 25;
                foreach (com.oddlabs.tt.model.Unit e_2 in java.lang.JIter.of(intel.enemy_chieftains))
                {
                    int d = com.oddlabs.tt.player.ai.MapAnalysis.dist2(e_2.getGridX(), e_2.getGridY(), c[0], c[1]);
                    if (!e_2.isDead() && d <= best)
                    {
                        best = d;
                        caster = e_2;
                    }
                }
                if (caster != null)
                {
                    enemy_casts.put(caster, ai.time());
                    ai.log("enemy stun caught " + ((stunned.size() - own_stunned_before)) + " of our units");
                }
            }
            own_stunned_before = stunned.size();
        }
        // Java: 675
        public bool isDodging(com.oddlabs.tt.model.Unit u)
        {
            return dodges.containsKey(u);
        }
        // Java: 683
        public void dodgeSpells()
        {
            float now = ai.time();
            dodges.values().removeIf(new java.util.function.Predicate_L((object d_l) =>
            {
                com.oddlabs.tt.player.ai.Military.Dodge d = (com.oddlabs.tt.player.ai.Military.Dodge)d_l;
                return d.until() < now;
            }));
            windups.entrySet().removeIf(new java.util.function.Predicate_L((object w_l) =>
            {
                java.util.Map.Entry w = (java.util.Map.Entry)w_l;
                return ((float)((float?)w.getValue())) < now && (((com.oddlabs.tt.model.Unit)w.getKey()).isDead() || !((((com.oddlabs.tt.model.Unit)w.getKey()).getCurrentController() is com.oddlabs.tt.model.behaviour.MagicController)));
            }));
            updateFogs(now);
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            com.oddlabs.tt.model.Race vikings = ai.owner().getWorld().getRacesResources().getRace(com.oddlabs.tt.model.RacesResources.RACE_VIKINGS);
            java.util.List casters = new java.util.ArrayList(intel.enemy_chieftains);
            if (intel.chieftain != null)
            {
                casters.add(intel.chieftain);
            }
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(casters))
            {
                if (e.isDead() || windups.containsKey(e) || !((e.getCurrentController() is com.oddlabs.tt.model.behaviour.MagicController)))
                {
                    continue;
                }
                windups.put(e, (float)(now + com.oddlabs.tt.player.ai.Military.STUN_WINDUP));
                if (((object)e.getOwner().getRace() != (object)vikings))
                {
                    if (ai.strategy().dodge_fog)
                    {
                        fogs.add(new com.oddlabs.tt.player.ai.Military.Fog(e, (float)(e.getPositionX() + (float)(com.oddlabs.tt.player.ai.Military.FOG_OFFSET * e.getDirectionX())), (float)(e.getPositionY() + (float)(com.oddlabs.tt.player.ai.Military.FOG_OFFSET * e.getDirectionY())), now));
                    }
                }
                else if (((object)e != (object)intel.chieftain) && ai.strategy().dodge_stun)
                {
                    dodgeStun(e, now);
                }
            }
            foreach (com.oddlabs.tt.player.ai.Military.Fog f in java.lang.JIter.of(fogs))
            {
                keepOutOf(f, now);
            }
            keepHuntingCasters(now);
            foreach (java.util.Map.Entry en in java.lang.JIter.of(dodges.entrySet()))
            {
                com.oddlabs.tt.model.Unit u = ((com.oddlabs.tt.model.Unit)en.getKey());
                com.oddlabs.tt.player.ai.Military.Dodge d_2 = ((com.oddlabs.tt.player.ai.Military.Dodge)en.getValue());
                if (u.isDead() || u.isMounted() || com.oddlabs.tt.player.ai.Intel.isStunned(u) || com.oddlabs.tt.player.ai.MapAnalysis.dist2(u.getGridX(), u.getGridY(), d_2.x(), d_2.y()) <= 2 * 2)
                {
                    continue;
                }
                if ((u.getCurrentController() is com.oddlabs.tt.model.behaviour.WalkController w_2) && com.oddlabs.tt.player.ai.MapAnalysis.dist2(w_2.getTarget().getGridX(), w_2.getTarget().getGridY(), d_2.x(), d_2.y()) <= 3 * 3)
                {
                    continue;
                }
                ai.owner().setLandscapeTarget(com.oddlabs.tt.model.Selectable.newArray(u), d_2.x(), d_2.y(), com.oddlabs.tt.model.Action.MOVE, false);
            }
        }
        // Java: 728
        public void updateFogs(float now)
        {
            for (java.util.Iterator it = fogs.iterator(); it.hasNext(); )
            {
                com.oddlabs.tt.player.ai.Military.Fog f = ((com.oddlabs.tt.player.ai.Military.Fog)it.next());
                if (now > (float)((float)((float)(f.cast + com.oddlabs.tt.player.ai.Military.FOG_RELEASE) + com.oddlabs.tt.player.ai.Military.FOG_TIME) + 0.5f))
                {
                    it.remove();
                }
                else if (!f.confirmed && now >= (float)((float)(f.cast + com.oddlabs.tt.player.ai.Military.FOG_RELEASE) + 0.1f))
                {
                    if (!f.caster.isDead() && f.caster.getLastMagicIndex() == com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_POISON)
                    {
                        f.confirmed = true;
                        ai.log("poison fog down at " + (com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(f.x)) + "," + (com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(f.y)) + java.lang.JStr.valueOf(((((object)f.caster.getOwner() == (object)ai.owner()) ? " (ours)" : ""))));
                    }
                    else
                    {
                        it.remove();
                    }
                }
                else if (!f.confirmed && f.caster.isDead())
                {
                    it.remove();
                }
            }
        }
        // Java: 751
        public void keepOutOf(com.oddlabs.tt.player.ai.Military.Fog f, float now)
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            java.util.List units = new java.util.ArrayList(intel.warriors);
            units.addAll(intel.peons);
            if (intel.chieftain != null && ((object)f.caster != (object)intel.chieftain))
            {
                units.add(intel.chieftain);
            }
            int grid = ai.map().getSize();
            float reach = (float)(com.oddlabs.tt.player.ai.Military.FOG_RADIUS + 3.0f);
            float until = java.lang.Math.min((float)(now + 4.0f), (float)((float)(f.cast + com.oddlabs.tt.player.ai.Military.FOG_RELEASE) + com.oddlabs.tt.player.ai.Military.FOG_TIME));
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(units))
            {
                if (u.isDead() || u.isMounted() || com.oddlabs.tt.player.ai.Intel.isStunned(u) || dodges.containsKey(u) || (u.getCurrentController() is com.oddlabs.tt.model.behaviour.MagicController))
                {
                    continue;
                }
                float dx = (float)(u.getPositionX() - f.x);
                float dy = (float)(u.getPositionY() - f.y);
                if ((float)((float)(dx * dx) + (float)(dy * dy)) > (float)(reach * reach))
                {
                    continue;
                }
                float d = ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy)))));
                if (d < 1.0f)
                {
                    dx = (float)((float)(staging_x * 2.0f) - f.x);
                    dy = (float)((float)(staging_y * 2.0f) - f.y);
                    d = java.lang.Math.max(1.0f, ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy))))));
                }
                float to = (float)(((float)(com.oddlabs.tt.player.ai.Military.FOG_RADIUS + 7.0f)) / d);
                int tx = java.lang.Math.clamp(com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate((float)(f.x + (float)(dx * to))), 0, grid - 1);
                int ty = java.lang.Math.clamp(com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate((float)(f.y + (float)(dy * to))), 0, grid - 1);
                dodges.put(u, new com.oddlabs.tt.player.ai.Military.Dodge(tx, ty, until));
                ai.owner().setLandscapeTarget(com.oddlabs.tt.model.Selectable.newArray(u), tx, ty, com.oddlabs.tt.model.Action.MOVE, false);
            }
        }
        // Java: 787
        public void dodgeStun(com.oddlabs.tt.model.Unit e, float now)
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            float release = (float)(now + com.oddlabs.tt.player.ai.Military.STUN_WINDUP);
            float sx = (float)(e.getPositionX() + (float)(com.oddlabs.tt.player.ai.Military.STUN_OFFSET * e.getDirectionX()));
            float sy = (float)(e.getPositionY() + (float)(com.oddlabs.tt.player.ai.Military.STUN_OFFSET * e.getDirectionY()));
            java.util.List units = new java.util.ArrayList(intel.warriors);
            units.addAll(intel.peons);
            if (intel.chieftain != null && !((intel.chieftain.getCurrentController() is com.oddlabs.tt.model.behaviour.MagicController)))
            {
                units.add(intel.chieftain);
            }
            int grid = ai.map().getSize();
            int n = 0;
            int hunters = 0;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(units))
            {
                if (u.isDead() || u.isMounted() || com.oddlabs.tt.player.ai.Intel.isStunned(u))
                {
                    continue;
                }
                float dx = (float)(u.getPositionX() - sx);
                float dy = (float)(u.getPositionY() - sy);
                float d = ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy)))));
                if (d > (float)(com.oddlabs.tt.player.ai.Military.STUN_RADIUS + 10.0f))
                {
                    continue;
                }
                float @out = (float)((float)(com.oddlabs.tt.player.ai.Military.STUN_RADIUS + 3.0f) - d);
                if (@out > (float)((float)(0.85f * ((com.oddlabs.tt.model.UnitTemplate)u.getTemplate()).getMetersPerSecond()) * com.oddlabs.tt.player.ai.Military.STUN_WINDUP))
                {
                    if (ai.strategy().hunt_caster && intel.warriors.contains(u) && com.oddlabs.tt.player.ai.MapAnalysis.dist2(u.getGridX(), u.getGridY(), e.getGridX(), e.getGridY()) <= ai.strategy().hunt_caster_cells * ai.strategy().hunt_caster_cells)
                    {
                        caster_hunters.put(u, release);
                        huntCaster(u, e);
                        hunters++;
                    }
                    continue;
                }
                float to = (float)(java.lang.Math.max(d, (float)(com.oddlabs.tt.player.ai.Military.STUN_RADIUS + 6.0f)) / java.lang.Math.max(d, 0.1f));
                int tx = java.lang.Math.clamp(com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate((float)(sx + (float)(dx * to))), 0, grid - 1);
                int ty = java.lang.Math.clamp(com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate((float)(sy + (float)(dy * to))), 0, grid - 1);
                dodges.put(u, new com.oddlabs.tt.player.ai.Military.Dodge(tx, ty, (float)(release + 0.3f)));
                ai.owner().setLandscapeTarget(com.oddlabs.tt.model.Selectable.newArray(u), tx, ty, com.oddlabs.tt.model.Action.MOVE, false);
                n++;
            }
            if (hunters > 0)
            {
                caster_targets.put(e, release);
            }
            if (n > 0 || hunters > 0)
            {
                ai.log("enemy chieftain winds up at " + (e.getGridX()) + "," + (e.getGridY()) + ": " + (n) + " units run out of reach, " + (hunters) + " go for him");
            }
        }
        // Java: 834
        public void huntCaster(com.oddlabs.tt.model.Unit u, com.oddlabs.tt.model.Unit chief)
        {
            hunt_targets.put(u, chief);
            last_order.put(u, ai.time());
            ai.intel().warrior_states.put(u, com.oddlabs.tt.player.ai.Intel.WarriorState.FIGHT);
            ai.owner().setTarget(com.oddlabs.tt.model.Selectable.newArray(u), chief, com.oddlabs.tt.model.Action.ATTACK, true);
        }
        // Java: 842
        public void keepHuntingCasters(float now)
        {
            caster_hunters.values().removeIf(new java.util.function.Predicate_L((object t_l) =>
            {
                float? t = (float?)t_l;
                return ((float)t) < now;
            }));
            caster_targets.entrySet().removeIf(new java.util.function.Predicate_L((object c_l) =>
            {
                java.util.Map.Entry c = (java.util.Map.Entry)c_l;
                return ((float)((float?)c.getValue())) < now || ((com.oddlabs.tt.model.Unit)c.getKey()).isDead();
            }));
            foreach (java.util.Map.Entry en in java.lang.JIter.of(caster_hunters.entrySet()))
            {
                com.oddlabs.tt.model.Unit u = ((com.oddlabs.tt.model.Unit)en.getKey());
                com.oddlabs.tt.model.Unit chief = ((com.oddlabs.tt.model.Unit)hunt_targets.get(u));
                if (u.isDead() || com.oddlabs.tt.player.ai.Intel.isStunned(u))
                {
                    continue;
                }
                if (chief == null || !caster_targets.containsKey(chief))
                {
                    chief = null;
                    foreach (com.oddlabs.tt.model.Unit c_2 in java.lang.JIter.of(caster_targets.keySet()))
                    {
                        if (chief == null || com.oddlabs.tt.player.ai.MapAnalysis.dist2(u.getGridX(), u.getGridY(), c_2.getGridX(), c_2.getGridY()) < com.oddlabs.tt.player.ai.MapAnalysis.dist2(u.getGridX(), u.getGridY(), chief.getGridX(), chief.getGridY()))
                        {
                            chief = c_2;
                        }
                    }
                    if (chief != null)
                    {
                        huntCaster(u, chief);
                    }
                }
                else if (!((u.getCurrentController() is com.oddlabs.tt.model.behaviour.HuntController)))
                {
                    huntCaster(u, chief);
                }
            }
        }
        // Java: 870
        public void huntChieftains(java.util.List units)
        {
            foreach (com.oddlabs.tt.model.Unit chief in java.lang.JIter.of(ai.intel().enemy_chieftains))
            {
                if (chief.isDead())
                {
                    continue;
                }
                if (ai.strategy().chief_per_hit && chief.getHitPoints() > 20 && !com.oddlabs.tt.player.ai.Intel.isStunned(chief))
                {
                    continue;
                }
                int[] c = com.oddlabs.tt.player.ai.Military.centroid(units);
                if (com.oddlabs.tt.player.ai.MapAnalysis.dist2(c[0], c[1], chief.getGridX(), chief.getGridY()) > 20 * 20)
                {
                    continue;
                }
                int wanted = java.lang.Math.min(8, java.lang.Math.max(3, units.size() / 3));
                java.util.List hunters = new java.util.ArrayList();
                for (int k = 0; k < wanted; k++)
                {
                    com.oddlabs.tt.model.Unit best = null;
                    int best_d = java.lang.Integer.MAX_VALUE;
                    foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(units))
                    {
                        if (hunters.contains(u))
                        {
                            continue;
                        }
                        com.oddlabs.tt.player.ai.Intel.WarriorState s = ((com.oddlabs.tt.player.ai.Intel.WarriorState)ai.intel().warrior_states.get(u));
                        if (((object)s == (object)com.oddlabs.tt.player.ai.Intel.WarriorState.STUNNED) || ((object)s == (object)com.oddlabs.tt.player.ai.Intel.WarriorState.ENTER))
                        {
                            continue;
                        }
                        int d = com.oddlabs.tt.player.ai.MapAnalysis.dist2(u.getGridX(), u.getGridY(), chief.getGridX(), chief.getGridY());
                        if (d < best_d)
                        {
                            best_d = d;
                            best = u;
                        }
                    }
                    if (best == null)
                    {
                        break;
                    }
                    hunters.add(best);
                }
                foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(hunters))
                {
                    float? last = ((float?)chief_orders.get(u_2));
                    if (last != null && (float)(ai.time() - ((float)last)) < 4.0f)
                    {
                        continue;
                    }
                    chief_orders.put(u_2, ai.time());
                    last_order.put(u_2, ai.time());
                    ai.owner().setTarget(com.oddlabs.tt.model.Selectable.newArray(u_2), chief, com.oddlabs.tt.model.Action.ATTACK, true);
                }
            }
        }
        // Java: 917
        public bool raiding(com.oddlabs.tt.model.Unit e, java.util.List own, int r2)
        {
            if (!com.oddlabs.tt.player.ai.Military.nearAny(own, e.getGridX(), e.getGridY(), r2))
            {
                return false;
            }
            foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(ai.intel().enemy_buildings))
            {
                if (com.oddlabs.tt.player.ai.MapAnalysis.dist2(e.getGridX(), e.getGridY(), b.getGridX(), b.getGridY()) > 30 * 30)
                {
                    continue;
                }
                if (!com.oddlabs.tt.player.ai.Military.nearAny(own, b.getGridX(), b.getGridY(), r2))
                {
                    return false;
                }
            }
            return true;
        }
        // Java: 929
        public static bool nearAny(java.util.List buildings, int x, int y, int r2)
        {
            foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(buildings))
            {
                if (com.oddlabs.tt.player.ai.MapAnalysis.dist2(x, y, b.getGridX(), b.getGridY()) <= r2)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 940
        public void peonMilitia()
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            militia_orders.keySet().removeIf(new java.util.function.Predicate_L((object a0) => { return ((com.oddlabs.tt.model.Unit)a0).isDead(); }));
            int[] c = com.oddlabs.tt.player.ai.Military.centroid(raiders);
            if (enemyStrengthNear(c[0], c[1], 20) > 1.0f)
            {
                return;
            }
            float guard = 0.0f;
            foreach (java.util.Map.Entry e in java.lang.JIter.of(roles.entrySet()))
            {
                com.oddlabs.tt.model.Unit u = ((com.oddlabs.tt.model.Unit)e.getKey());
                if (((object)((com.oddlabs.tt.player.ai.Military.Role)e.getValue()) != (object)com.oddlabs.tt.player.ai.Military.Role.TOWER) && com.oddlabs.tt.player.ai.MapAnalysis.dist2(u.getGridX(), u.getGridY(), c[0], c[1]) <= 30 * 30)
                {
                    guard = (float)(guard + com.oddlabs.tt.player.ai.Combat.@value(u));
                }
            }
            if (guard >= (float)(0.4f * raiders.size()))
            {
                return;
            }
            java.util.List militia = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit p in java.lang.JIter.of(intel.peons))
            {
                com.oddlabs.tt.player.ai.Intel.PeonState s = ((com.oddlabs.tt.player.ai.Intel.PeonState)intel.peon_states.get(p));
                if (((object)s == (object)com.oddlabs.tt.player.ai.Intel.PeonState.TRANSIT) || ((object)s == (object)com.oddlabs.tt.player.ai.Intel.PeonState.STUNNED) || ((object)s == (object)com.oddlabs.tt.player.ai.Intel.PeonState.SAPPER))
                {
                    continue;
                }
                if (com.oddlabs.tt.player.ai.MapAnalysis.dist2(p.getGridX(), p.getGridY(), c[0], c[1]) <= 30 * 30)
                {
                    militia.add(p);
                }
            }
            if (militia.size() < (float)(1.2f * raiders.size()))
            {
                foreach (com.oddlabs.tt.model.Unit p_2 in java.lang.JIter.of(militia))
                {
                    bool close = false;
                    foreach (com.oddlabs.tt.model.Unit e_2 in java.lang.JIter.of(raiders))
                    {
                        close |= com.oddlabs.tt.player.ai.MapAnalysis.dist2(p_2.getGridX(), p_2.getGridY(), e_2.getGridX(), e_2.getGridY()) <= 8 * 8;
                    }
                    com.oddlabs.tt.model.Building shelter = (close ? com.oddlabs.tt.player.ai.Military.nearest(intel.quarters, p_2) : (com.oddlabs.tt.model.Building)(null));
                    if (shelter != null && shelter.getUnitContainer() != null)
                    {
                        ai.owner().setTarget(com.oddlabs.tt.model.Selectable.newArray(p_2), shelter, com.oddlabs.tt.model.Action.DEFAULT, false);
                    }
                }
                return;
            }
            if ((float)(ai.time() - last_militia_log) > 15.0f)
            {
                last_militia_log = ai.time();
                ai.log((militia.size()) + " peons fight off " + (raiders.size()) + " raiding peons at " + (c[0]) + "," + (c[1]));
            }
            foreach (com.oddlabs.tt.model.Unit p_3 in java.lang.JIter.of(militia))
            {
                intel.peon_states.put(p_3, com.oddlabs.tt.player.ai.Intel.PeonState.FIGHT);
                float? last = ((float?)militia_orders.get(p_3));
                if (last != null && (float)(ai.time() - ((float)last)) < 3.0f)
                {
                    continue;
                }
                com.oddlabs.tt.model.Unit target = null;
                int best = java.lang.Integer.MAX_VALUE;
                foreach (com.oddlabs.tt.model.Unit e_3 in java.lang.JIter.of(raiders))
                {
                    int d = com.oddlabs.tt.player.ai.MapAnalysis.dist2(p_3.getGridX(), p_3.getGridY(), e_3.getGridX(), e_3.getGridY());
                    if (d < best)
                    {
                        best = d;
                        target = e_3;
                    }
                }
                if (target == null)
                {
                    continue;
                }
                militia_orders.put(p_3, ai.time());
                ai.owner().setTarget(com.oddlabs.tt.model.Selectable.newArray(p_3), target, com.oddlabs.tt.model.Action.ATTACK, true);
            }
        }
        // Java: 1004
        public void answerRaiders()
        {
            int[] c = com.oddlabs.tt.player.ai.Military.centroid(raiders);
            if (threat_level == 0)
            {
                java.util.List home = new java.util.ArrayList();
                foreach (java.util.Map.Entry e in java.lang.JIter.of(roles.entrySet()))
                {
                    if (((object)((com.oddlabs.tt.player.ai.Military.Role)e.getValue()) == (object)com.oddlabs.tt.player.ai.Military.Role.ARMY) && ((object)((com.oddlabs.tt.player.ai.Intel.WarriorState)ai.intel().warrior_states.get(((com.oddlabs.tt.model.Unit)e.getKey()))) != (object)com.oddlabs.tt.player.ai.Intel.WarriorState.STUNNED))
                    {
                        home.add(((com.oddlabs.tt.model.Unit)e.getKey()));
                    }
                }
                home.sort(new java.util.Comparator_L((object a_l, object b_l) =>
                {
                    com.oddlabs.tt.model.Unit a = (com.oddlabs.tt.model.Unit)a_l;
                    com.oddlabs.tt.model.Unit b = (com.oddlabs.tt.model.Unit)b_l;
                    return java.lang.Integer.compare(com.oddlabs.tt.player.ai.MapAnalysis.dist2(a.getGridX(), a.getGridY(), c[0], c[1]), com.oddlabs.tt.player.ai.MapAnalysis.dist2(b.getGridX(), b.getGridY(), c[0], c[1]));
                }));
                float needed = (float)((float)(0.4f * raiders.size()) + 1.0f);
                float sent = 0.0f;
                int n = 0;
                while (n < home.size() && sent < needed)
                {
                    sent = (float)(sent + com.oddlabs.tt.player.ai.Combat.@value(((com.oddlabs.tt.model.Unit)home.get(n++))));
                }
                if (sent >= needed)
                {
                    engageSpread(home.subList(0, n), raiders, c[0], c[1], false);
                    return;
                }
            }
            peonMilitia();
        }
        // Java: 1026
        public static com.oddlabs.tt.model.Building nearest(java.util.List buildings, com.oddlabs.tt.model.Unit u)
        {
            com.oddlabs.tt.model.Building best = null;
            int best_d = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(buildings))
            {
                int d = com.oddlabs.tt.player.ai.MapAnalysis.dist2(u.getGridX(), u.getGridY(), b.getGridX(), b.getGridY());
                if (d < best_d)
                {
                    best_d = d;
                    best = b;
                }
            }
            return best;
        }
        // Java: 1040
        public void peonRush()
        {
            if (!ai.strategy().peon_rush || ai.time() > 300.0f || (float)(ai.time() - last_peon_rush) < 4.0f)
            {
                return;
            }
            last_peon_rush = ai.time();
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            foreach (com.oddlabs.tt.model.Unit p in java.lang.JIter.of(intel.peons))
            {
                com.oddlabs.tt.model.Unit target = null;
                int best = java.lang.Integer.MAX_VALUE;
                foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(intel.enemy_peons))
                {
                    int d = com.oddlabs.tt.player.ai.MapAnalysis.dist2(p.getGridX(), p.getGridY(), e.getGridX(), e.getGridY());
                    if (d < best)
                    {
                        best = d;
                        target = e;
                    }
                }
                if (target != null)
                {
                    intel.peon_states.put(p, com.oddlabs.tt.player.ai.Intel.PeonState.FIGHT);
                    ai.owner().setTarget(com.oddlabs.tt.model.Selectable.newArray(p), target, com.oddlabs.tt.model.Action.ATTACK, true);
                }
            }
        }
        // Java: 1062
        public void evacuatePeons()
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            com.oddlabs.tt.model.Building armory = intel.armory();
            java.util.List evacuate = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit p in java.lang.JIter.of(intel.peons))
            {
                com.oddlabs.tt.player.ai.Intel.PeonState s = ((com.oddlabs.tt.player.ai.Intel.PeonState)intel.peon_states.get(p));
                if (((object)s == (object)com.oddlabs.tt.player.ai.Intel.PeonState.TRANSIT) || ((object)s == (object)com.oddlabs.tt.player.ai.Intel.PeonState.STUNNED) || ((object)s == (object)com.oddlabs.tt.player.ai.Intel.PeonState.SAPPER))
                {
                    continue;
                }
                float? militia = ((float?)militia_orders.get(p));
                if (militia != null && (float)(ai.time() - ((float)militia)) < 5.0f)
                {
                    continue;
                }
                if (!threatNear(p.getGridX(), p.getGridY(), 11))
                {
                    continue;
                }
                evacuate.add(p);
            }
            foreach (com.oddlabs.tt.model.Unit p_2 in java.lang.JIter.of(evacuate))
            {
                com.oddlabs.tt.model.Building shelter = null;
                int best = java.lang.Integer.MAX_VALUE;
                if (armory != null && !threatNear(armory.getGridX(), armory.getGridY(), 6))
                {
                    shelter = armory;
                }
                foreach (com.oddlabs.tt.model.Building q in java.lang.JIter.of(intel.quarters))
                {
                    if (threatNear(q.getGridX(), q.getGridY(), 6))
                    {
                        continue;
                    }
                    int d = com.oddlabs.tt.player.ai.MapAnalysis.dist2(q.getGridX(), q.getGridY(), p_2.getGridX(), p_2.getGridY());
                    int da = (((object)shelter == (object)armory) && armory != null ? com.oddlabs.tt.player.ai.MapAnalysis.dist2(armory.getGridX(), armory.getGridY(), p_2.getGridX(), p_2.getGridY()) : java.lang.Integer.MAX_VALUE);
                    if (d < best && d < da)
                    {
                        best = d;
                        shelter = q;
                    }
                }
                if (shelter != null && shelter.getUnitContainer() != null)
                {
                    ai.owner().setTarget(com.oddlabs.tt.model.Selectable.newArray(p_2), shelter, com.oddlabs.tt.model.Action.DEFAULT, false);
                }
            }
        }
        // Java: 1102
        public void manTowers()
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            foreach (com.oddlabs.tt.model.Building tower in java.lang.JIter.of(intel.towers))
            {
                if (com.oddlabs.tt.player.ai.Intel.isTowerManned(tower) || tower_assignments.containsValue(tower))
                {
                    continue;
                }
                com.oddlabs.tt.model.Unit best = null;
                int best_score = java.lang.Integer.MAX_VALUE;
                foreach (java.util.Map.Entry e in java.lang.JIter.of(roles.entrySet()))
                {
                    if (((object)((com.oddlabs.tt.player.ai.Military.Role)e.getValue()) != (object)com.oddlabs.tt.player.ai.Military.Role.ARMY))
                    {
                        continue;
                    }
                    com.oddlabs.tt.model.Unit u = ((com.oddlabs.tt.model.Unit)e.getKey());
                    com.oddlabs.tt.player.ai.Intel.WarriorState s = ((com.oddlabs.tt.player.ai.Intel.WarriorState)intel.warrior_states.get(u));
                    if (((object)s == (object)com.oddlabs.tt.player.ai.Intel.WarriorState.FIGHT) || ((object)s == (object)com.oddlabs.tt.player.ai.Intel.WarriorState.STUNNED) || s == null)
                    {
                        continue;
                    }
                    int d = com.oddlabs.tt.player.ai.MapAnalysis.dist2(u.getGridX(), u.getGridY(), tower.getGridX(), tower.getGridY());
                    if (d > 70 * 70)
                    {
                        continue;
                    }
                    int score = d;
                    com.oddlabs.tt.player.ai.Intel.WarriorType type = com.oddlabs.tt.player.ai.Intel.warriorType(u);
                    if (((object)type == (object)com.oddlabs.tt.player.ai.Intel.WarriorType.CHICKEN))
                    {
                        score -= 60 * 60;
                    }
                    else if (((object)type == (object)com.oddlabs.tt.player.ai.Intel.WarriorType.ROCK))
                    {
                        score += 30 * 30;
                    }
                    if (score < best_score)
                    {
                        best_score = score;
                        best = u;
                    }
                }
                if (best != null)
                {
                    roles.put(best, com.oddlabs.tt.player.ai.Military.Role.TOWER);
                    tower_assignments.put(best, tower);
                    ai.owner().setTarget(com.oddlabs.tt.model.Selectable.newArray(best), tower, com.oddlabs.tt.model.Action.DEFAULT, false);
                }
            }
            if (threat_level > 0)
            {
                return;
            }
            foreach (com.oddlabs.tt.model.Building tower_2 in java.lang.JIter.of(intel.towers))
            {
                if (!com.oddlabs.tt.player.ai.Intel.isTowerManned(tower_2))
                {
                    continue;
                }
                com.oddlabs.tt.model.Unit inside = (((com.oddlabs.tt.model.MountUnitContainer)tower_2.getUnitContainer())).getUnit();
                if (inside == null || ((object)com.oddlabs.tt.player.ai.Intel.warriorType(inside) == (object)com.oddlabs.tt.player.ai.Intel.WarriorType.CHICKEN))
                {
                    continue;
                }
                if (enemyStrengthNear(tower_2.getGridX(), tower_2.getGridY(), 30) > 0)
                {
                    continue;
                }
                foreach (java.util.Map.Entry e_2 in java.lang.JIter.of(roles.entrySet()))
                {
                    com.oddlabs.tt.model.Unit u_2 = ((com.oddlabs.tt.model.Unit)e_2.getKey());
                    if (((object)((com.oddlabs.tt.player.ai.Military.Role)e_2.getValue()) == (object)com.oddlabs.tt.player.ai.Military.Role.ARMY) && ((object)com.oddlabs.tt.player.ai.Intel.warriorType(u_2) == (object)com.oddlabs.tt.player.ai.Intel.WarriorType.CHICKEN) && ((object)((com.oddlabs.tt.player.ai.Intel.WarriorState)intel.warrior_states.get(u_2)) == (object)com.oddlabs.tt.player.ai.Intel.WarriorState.IDLE) && com.oddlabs.tt.player.ai.MapAnalysis.dist2(u_2.getGridX(), u_2.getGridY(), tower_2.getGridX(), tower_2.getGridY()) < 50 * 50)
                    {
                        ai.owner().exitTower(tower_2);
                        return;
                    }
                }
            }
        }
        // Java: 1163
        public void holdStaging()
        {
            bool escorting = ((object)mode_f == (object)com.oddlabs.tt.player.ai.Military.Mode.HOME) && (float)(ai.time() - escort_time) < 3.0f;
            int x = (escorting ? escort_x : staging_x);
            int y = (escorting ? escort_y : staging_y);
            foreach (java.util.Map.Entry e in java.lang.JIter.of(roles.entrySet()))
            {
                if (((object)((com.oddlabs.tt.player.ai.Military.Role)e.getValue()) != (object)com.oddlabs.tt.player.ai.Military.Role.ARMY))
                {
                    continue;
                }
                com.oddlabs.tt.model.Unit u = ((com.oddlabs.tt.model.Unit)e.getKey());
                if (threat_level > 0)
                {
                    continue;
                }
                com.oddlabs.tt.player.ai.Intel.WarriorState s = ((com.oddlabs.tt.player.ai.Intel.WarriorState)ai.intel().warrior_states.get(u));
                if (((object)s != (object)com.oddlabs.tt.player.ai.Intel.WarriorState.IDLE))
                {
                    continue;
                }
                if (com.oddlabs.tt.player.ai.MapAnalysis.dist2(u.getGridX(), u.getGridY(), x, y) > 7 * 7)
                {
                    attackGround(u, x, y, false);
                }
            }
        }
        // Java: 1185
        public bool canEscort()
        {
            return ((object)mode_f == (object)com.oddlabs.tt.player.ai.Military.Mode.HOME) && threat_level == 0 && armyStrength() >= java.lang.Math.max(12.0f, (float)(ai.strategy().forward_ratio * enemyFieldStrength()));
        }
        // Java: 1191
        public void escort(int x, int y)
        {
            escort_x = x;
            escort_y = y;
            escort_time = ai.time();
        }
        // Java: 1198
        public bool escortArrived(int x, int y)
        {
            int n = 0;
            int near = 0;
            foreach (java.util.Map.Entry e in java.lang.JIter.of(roles.entrySet()))
            {
                if (((object)((com.oddlabs.tt.player.ai.Military.Role)e.getValue()) != (object)com.oddlabs.tt.player.ai.Military.Role.ARMY))
                {
                    continue;
                }
                n++;
                com.oddlabs.tt.model.Unit u = ((com.oddlabs.tt.model.Unit)e.getKey());
                if (com.oddlabs.tt.player.ai.MapAnalysis.dist2(u.getGridX(), u.getGridY(), x, y) <= 14 * 14)
                {
                    near++;
                }
            }
            return n > 0 && near * 10 >= n * 6;
        }
        // Java: 1216
        public int[] forwardTarget()
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            com.oddlabs.tt.player.ai.DistanceField ours = ai.planner().getStartField();
            int[] best = null;
            int best_count = 3;
            foreach (com.oddlabs.tt.model.Unit p in java.lang.JIter.of(intel.enemy_peons))
            {
                int px = p.getGridX();
                int py = p.getGridY();
                bool at_iron = false;
                foreach (com.oddlabs.tt.model.IronSupply s in java.lang.JIter.of(ai.map().getIron()))
                {
                    if (!s.isEmpty() && com.oddlabs.tt.player.ai.MapAnalysis.dist2(px, py, s.getGridX(), s.getGridY()) <= 3 * 3)
                    {
                        at_iron = true;
                        break;
                    }
                }
                if (!at_iron || ours.getAround(px, py, 2) == com.oddlabs.tt.player.ai.DistanceField.UNREACHABLE)
                {
                    continue;
                }
                bool guarded = false;
                foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(intel.enemy_towers))
                {
                    guarded |= com.oddlabs.tt.player.ai.MapAnalysis.dist2(t.getGridX(), t.getGridY(), px, py) <= 16 * 16;
                }
                foreach (com.oddlabs.tt.model.Building a in java.lang.JIter.of(intel.enemy_armories))
                {
                    guarded |= com.oddlabs.tt.player.ai.MapAnalysis.dist2(a.getGridX(), a.getGridY(), px, py) <= 20 * 20;
                }
                if (guarded)
                {
                    continue;
                }
                int count = com.oddlabs.tt.player.ai.Combat.countNear(intel.enemy_peons, px, py, 8);
                if (count > best_count)
                {
                    best_count = count;
                    best = new int[] { px, py };
                }
            }
            return best;
        }
        // Java: 1249
        public float attackStrength()
        {
            float s = 0.0f;
            foreach (java.util.Map.Entry e in java.lang.JIter.of(roles.entrySet()))
            {
                if (((object)((com.oddlabs.tt.player.ai.Military.Role)e.getValue()) == (object)com.oddlabs.tt.player.ai.Military.Role.ATTACK))
                {
                    s = (float)(s + com.oddlabs.tt.player.ai.Combat.@value(((com.oddlabs.tt.model.Unit)e.getKey())));
                }
            }
            return s;
        }
        // Java: 1258
        public float defenseAt(int x, int y)
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            float s = (float)(1.1f * ((ai.enemiesAlive() > 1 ? enemyFieldStrengthNear(x, y, 150) : enemyFieldStrength())));
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(intel.enemy_towers))
            {
                if (com.oddlabs.tt.player.ai.MapAnalysis.dist2(t.getGridX(), t.getGridY(), x, y) <= 22 * 22)
                {
                    s = (float)(s + enemyTowerValue(t));
                }
            }
            s = (float)(s + (float)(0.5f * com.oddlabs.tt.player.ai.Combat.strengthNear(intel.enemy_peons, x, y, 40)));
            if (ai.strategy().hidden_info)
            {
                foreach (com.oddlabs.tt.model.Building a in java.lang.JIter.of(intel.enemy_armories))
                {
                    if (!a.isDead() && com.oddlabs.tt.player.ai.MapAnalysis.dist2(a.getGridX(), a.getGridY(), x, y) <= 40 * 40)
                    {
                        s = (float)(s + com.oddlabs.tt.player.ai.Military.stockStrength(a));
                    }
                }
            }
            return s;
        }
        // Java: 1276
        public com.oddlabs.tt.model.Selectable chooseTarget(int from_x, int from_y)
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            com.oddlabs.tt.model.Selectable best = null;
            float best_score = java.lang.Float.MAX_VALUE;
            java.util.List candidates = new java.util.ArrayList(intel.enemy_armories);
            candidates.addAll(intel.enemy_quarters);
            candidates.addAll(intel.enemy_towers);
            if (candidates.isEmpty())
            {
                candidates.addAll(intel.enemy_buildings);
            }
            foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(candidates))
            {
                if (b.isDead())
                {
                    continue;
                }
                float d = com.oddlabs.tt.player.ai.MapAnalysis.meters(from_x, from_y, b.getGridX(), b.getGridY());
                float priority = ((((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID()) switch { 1 => (float)(0.0f), 0 => (float)(60.0f), _ => (float)(120.0f) });
                float score = (float)((float)(d + priority) + (float)(8.0f * defenseAt(b.getGridX(), b.getGridY())));
                if (score < best_score)
                {
                    best_score = score;
                    best = b;
                }
            }
            if (best == null)
            {
                java.util.List units = new java.util.ArrayList(intel.enemy_peons);
                units.addAll(intel.enemy_warriors);
                units.addAll(intel.enemy_chieftains);
                int best_d = java.lang.Integer.MAX_VALUE;
                foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(units))
                {
                    int d_2 = com.oddlabs.tt.player.ai.MapAnalysis.dist2(from_x, from_y, u.getGridX(), u.getGridY());
                    if (d_2 < best_d)
                    {
                        best_d = d_2;
                        best = u;
                    }
                }
            }
            return best;
        }
        // Java: 1320
        public float enemyGrowthPerSecond()
        {
            if (enemy_history.size() < 2)
            {
                return 0.0f;
            }
            float[] first = ((float[])enemy_history.getFirst());
            float[] last = ((float[])enemy_history.getLast());
            float dt = (float)(last[0] - first[0]);
            return (dt > 0.0f ? (float)(((float)(last[1] - first[1])) / dt) : 0.0f);
        }
        // Java: 1329
        public void recordEnemyStrength()
        {
            enemy_history.addLast(new float[] { ai.time(), enemyFieldStrength() });
            while (enemy_history.size() > 2 && (float)(ai.time() - ((float[])enemy_history.getFirst())[0]) > 90.0f)
            {
                enemy_history.removeFirst();
            }
        }
        // Java: 1335
        public void considerAttack()
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            com.oddlabs.tt.player.ai.Strategy strategy = ai.strategy();
            com.oddlabs.tt.model.Selectable t = chooseTarget(staging_x, staging_y);
            if (t == null)
            {
                return;
            }
            float army = armyStrength();
            float potential = (float)(army + stockStrength());
            float defense = defenseAt(t.getGridX(), t.getGridY());
            if (strategy.project_defense)
            {
                int d = ai.planner().getEnemyField().get(staging_x, staging_y);
                float march = (d == com.oddlabs.tt.player.ai.DistanceField.UNREACHABLE ? 120.0f : (float)(d / 3.0f));
                defense = (float)(defense + (float)(java.lang.Math.max(0.0f, enemyGrowthPerSecond()) * march));
            }
            bool chief = intel.chieftain != null && intel.chieftain.getHitPoints() > 30;
            bool enemy_chief = false;
            foreach (com.oddlabs.tt.model.Unit c in java.lang.JIter.of(intel.enemy_chieftains))
            {
                enemy_chief |= !c.isDead() && c.getHitPoints() > 15;
            }
            float bonus = (chief && !enemy_chief ? 1.4f : (chief ? 1.05f : (enemy_chief ? 0.7f : 1.0f)));
            com.oddlabs.tt.player.Player owner = ai.owner();
            bool capped = owner.getUnitCountContainer().getNumSupplies() >= owner.getWorld().getMaxUnitCount() - 10;
            float caution = attack_caution;
            bool go = potential >= strategy.attack_min_strength && (float)(potential * bonus) >= (float)((float)(strategy.attack_ratio * caution) * defense);
            go |= potential >= (float)(strategy.attack_max_strength * caution) && (float)(potential * bonus) >= (float)((float)(0.8f * caution) * defense);
            go |= capped && (float)(potential * bonus) >= (float)((float)(strategy.capped_ratio * caution) * defense);
            if (!go || ai.time() < next_wave_time)
            {
                return;
            }
            target = t;
            mode_f = com.oddlabs.tt.player.ai.Military.Mode.MUSTER;
            muster_start = ai.time();
            ai.log(java.lang.JString.format("muster: army %.1f + stock %.1f vs defense %.1f at %d,%d", army, (float)(potential - army), defense, t.getGridX(), t.getGridY()));
        }
        // Java: 1377
        public void considerStrike()
        {
            com.oddlabs.tt.model.Building b = intruder();
            if (b == null)
            {
                return;
            }
            int bx = b.getGridX();
            int by = b.getGridY();
            float defense = (float)(1.1f * enemyFightersNear(bx, by, 24));
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(ai.intel().enemy_towers))
            {
                if (com.oddlabs.tt.player.ai.MapAnalysis.dist2(t.getGridX(), t.getGridY(), bx, by) <= 12 * 12)
                {
                    defense = (float)(defense + enemyTowerValue(t));
                }
            }
            float army = armyStrength();
            if (army < java.lang.Math.max(10.0f, (float)(2.0f * defense)))
            {
                return;
            }
            target = b;
            strike = true;
            ai.log(java.lang.JString.format("strike on %s at %d,%d: army %.1f vs %.1f", b, bx, by, army, defense));
            launchAttack();
        }
        // Java: 1398
        public com.oddlabs.tt.model.Building intruder()
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            java.util.List own = new java.util.ArrayList(intel.armories);
            own.addAll(intel.quarters);
            own.addAll(intel.towers);
            int r = ai.strategy().base_radius + 6;
            com.oddlabs.tt.model.Building best = null;
            int best_d = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(intel.enemy_buildings))
            {
                if (b.isDead())
                {
                    continue;
                }
                int d = java.lang.Integer.MAX_VALUE;
                foreach (com.oddlabs.tt.model.Building o in java.lang.JIter.of(own))
                {
                    d = java.lang.Math.min(d, com.oddlabs.tt.player.ai.MapAnalysis.dist2(o.getGridX(), o.getGridY(), b.getGridX(), b.getGridY()));
                }
                if (d > r * r && !gatherersNear(b.getGridX(), b.getGridY(), 16))
                {
                    continue;
                }
                if (d < best_d)
                {
                    best_d = d;
                    best = b;
                }
            }
            return best;
        }
        // Java: 1422
        public bool gatherersNear(int x, int y, int radius)
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            foreach (com.oddlabs.tt.model.Unit p in java.lang.JIter.of(intel.peons))
            {
                com.oddlabs.tt.player.ai.Intel.PeonState s = ((com.oddlabs.tt.player.ai.Intel.PeonState)intel.peon_states.get(p));
                if ((((object)s == (object)com.oddlabs.tt.player.ai.Intel.PeonState.GATHER_TREE) || ((object)s == (object)com.oddlabs.tt.player.ai.Intel.PeonState.GATHER_ROCK) || ((object)s == (object)com.oddlabs.tt.player.ai.Intel.PeonState.GATHER_IRON)) && com.oddlabs.tt.player.ai.MapAnalysis.dist2(x, y, p.getGridX(), p.getGridY()) <= radius * radius)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 1433
        public float stockStrength()
        {
            com.oddlabs.tt.model.Building armory = ai.intel().armory();
            return (armory == null ? 0.0f : com.oddlabs.tt.player.ai.Military.stockStrength(armory));
        }
        // Java: 1439
        public static float stockStrength(com.oddlabs.tt.model.Building armory)
        {
            int workers = armory.getUnitContainer().getNumSupplies();
            int iron = armory.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)).getNumSupplies();
            int chicken = armory.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)).getNumSupplies();
            int rock = armory.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)).getNumSupplies();
            float s = 0.0f;
            int left = workers;
            int c = java.lang.Math.min(chicken, left);
            s = (float)(s + (float)(c * com.oddlabs.tt.player.ai.Combat.CHICKEN));
            left -= c;
            int i = java.lang.Math.min(iron, left);
            s = (float)(s + (float)(i * com.oddlabs.tt.player.ai.Combat.IRON));
            left -= i;
            s = (float)(s + (float)(java.lang.Math.min(rock, left) * com.oddlabs.tt.player.ai.Combat.ROCK));
            return s;
        }
        // Java: 1457
        public void muster()
        {
            holdStaging();
            com.oddlabs.tt.model.Building armory = ai.intel().armory();
            bool stock_left = armory != null && stockStrength() > 0.5f && armory.getUnitContainer().getNumSupplies() > 0;
            int pending = 0;
            if (armory != null)
            {
                pending += armory.getDeployContainer(com.oddlabs.tt.model.DeployType.IRON_WARRIOR).getNumSupplies();
                pending += armory.getDeployContainer(com.oddlabs.tt.model.DeployType.RUBBER_WARRIOR).getNumSupplies();
                pending += armory.getDeployContainer(com.oddlabs.tt.model.DeployType.ROCK_WARRIOR).getNumSupplies();
            }
            int near = 0;
            int total = 0;
            foreach (java.util.Map.Entry e in java.lang.JIter.of(roles.entrySet()))
            {
                if (((object)((com.oddlabs.tt.player.ai.Military.Role)e.getValue()) != (object)com.oddlabs.tt.player.ai.Military.Role.ARMY))
                {
                    continue;
                }
                total++;
                com.oddlabs.tt.model.Unit u = ((com.oddlabs.tt.model.Unit)e.getKey());
                if (com.oddlabs.tt.player.ai.MapAnalysis.dist2(u.getGridX(), u.getGridY(), staging_x, staging_y) <= 12 * 12)
                {
                    near++;
                }
            }
            bool gathered = total > 0 && near >= total * 8 / 10;
            bool timeout = (float)(ai.time() - muster_start) > 45.0f;
            if ((!stock_left && pending == 0 && gathered) || timeout)
            {
                launchAttack();
            }
        }
        // Java: 1484
        public void launchAttack()
        {
            com.oddlabs.tt.model.Selectable t = (target != null && !target.isDead() ? target : chooseTarget(staging_x, staging_y));
            if (t == null)
            {
                mode_f = com.oddlabs.tt.player.ai.Military.Mode.HOME;
                return;
            }
            setTarget(t);
            float s = 0.0f;
            foreach (java.util.Map.Entry e in java.lang.JIter.of(roles.entrySet()))
            {
                if (((object)((com.oddlabs.tt.player.ai.Military.Role)e.getValue()) == (object)com.oddlabs.tt.player.ai.Military.Role.ARMY))
                {
                    e.setValue(com.oddlabs.tt.player.ai.Military.Role.ATTACK);
                    s = (float)(s + com.oddlabs.tt.player.ai.Combat.@value(((com.oddlabs.tt.model.Unit)e.getKey())));
                }
            }
            attack_initial_strength = s;
            attack_kills_start = ai.owner().getUnitsKilled();
            attack_losses_start = ai.owner().getUnitsLost();
            attack_running = true;
            attack_start = ai.time();
            last_progress_time = ai.time();
            best_target_dist = java.lang.Integer.MAX_VALUE;
            mode_f = (s > 0 ? com.oddlabs.tt.player.ai.Military.Mode.ATTACK : com.oddlabs.tt.player.ai.Military.Mode.HOME);
            ai.log(java.lang.JString.format("attack with %.1f on %s at %d,%d", s, t, t.getGridX(), t.getGridY()));
            if (((object)mode_f == (object)com.oddlabs.tt.player.ai.Military.Mode.ATTACK))
            {
                recruitSappers(t.getGridX(), t.getGridY());
            }
        }
        // Java: 1514
        public void recruitSappers(int x, int y)
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            if (!ai.strategy().sappers || !intel.sappers.isEmpty())
            {
                return;
            }
            int towers = 0;
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(intel.enemy_towers))
            {
                if (com.oddlabs.tt.player.ai.MapAnalysis.dist2(t.getGridX(), t.getGridY(), x, y) <= 30 * 30)
                {
                    towers++;
                }
            }
            if (towers == 0)
            {
                return;
            }
            int want = java.lang.Math.min(16, java.lang.Math.max(6, 2 * towers));
            java.util.List pool = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit p in java.lang.JIter.of(intel.peons))
            {
                com.oddlabs.tt.player.ai.Intel.PeonState s = ((com.oddlabs.tt.player.ai.Intel.PeonState)intel.peon_states.get(p));
                if (((object)s == (object)com.oddlabs.tt.player.ai.Intel.PeonState.IDLE) || ((object)s == (object)com.oddlabs.tt.player.ai.Intel.PeonState.GATHER_TREE) || ((object)s == (object)com.oddlabs.tt.player.ai.Intel.PeonState.GATHER_ROCK) || ((object)s == (object)com.oddlabs.tt.player.ai.Intel.PeonState.GATHER_IRON) || ((object)s == (object)com.oddlabs.tt.player.ai.Intel.PeonState.MOVE))
                {
                    pool.add(p);
                }
            }
            if (pool.size() < want + 15)
            {
                return;
            }
            pool.sort(new java.util.Comparator_L((object a_l, object b_l) =>
            {
                com.oddlabs.tt.model.Unit a = (com.oddlabs.tt.model.Unit)a_l;
                com.oddlabs.tt.model.Unit b = (com.oddlabs.tt.model.Unit)b_l;
                return java.lang.Integer.compare(com.oddlabs.tt.player.ai.MapAnalysis.dist2(a.getGridX(), a.getGridY(), staging_x, staging_y), com.oddlabs.tt.player.ai.MapAnalysis.dist2(b.getGridX(), b.getGridY(), staging_x, staging_y));
            }));
            foreach (com.oddlabs.tt.model.Unit p_2 in java.lang.JIter.of(pool.subList(0, want)))
            {
                intel.sappers.add(p_2);
                intel.peon_states.put(p_2, com.oddlabs.tt.player.ai.Intel.PeonState.SAPPER);
            }
            ai.log((want) + " peons go along to pull down " + (towers) + " towers");
        }
        // Java: 1547
        public void sapperTick()
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            intel.sappers.removeIf(new java.util.function.Predicate_L((object a0) => { return ((com.oddlabs.tt.model.Unit)a0).isDead(); }));
            sapper_orders.keySet().removeIf(new java.util.function.Predicate_L((object a0) => { return ((com.oddlabs.tt.model.Unit)a0).isDead(); }));
            if (intel.sappers.isEmpty())
            {
                return;
            }
            int[] c = (((object)mode_f == (object)com.oddlabs.tt.player.ai.Military.Mode.ATTACK) ? attackCenter() : (int[])(null));
            if (c == null)
            {
                com.oddlabs.tt.model.Building home = intel.armory();
                foreach (com.oddlabs.tt.model.Unit p in java.lang.JIter.of(intel.sappers))
                {
                    if (home != null && home.getUnitContainer() != null)
                    {
                        ai.owner().setTarget(com.oddlabs.tt.model.Selectable.newArray(p), home, com.oddlabs.tt.model.Action.DEFAULT, false);
                    }
                }
                ai.log((intel.sappers.size()) + " sappers go home");
                intel.sappers.clear();
                return;
            }
            float ours = 0.0f;
            foreach (java.util.Map.Entry e in java.lang.JIter.of(roles.entrySet()))
            {
                if (((object)((com.oddlabs.tt.player.ai.Military.Role)e.getValue()) == (object)com.oddlabs.tt.player.ai.Military.Role.ATTACK) && com.oddlabs.tt.player.ai.MapAnalysis.dist2(((com.oddlabs.tt.model.Unit)e.getKey()).getGridX(), ((com.oddlabs.tt.model.Unit)e.getKey()).getGridY(), c[0], c[1]) <= 20 * 20)
                {
                    ours = (float)(ours + com.oddlabs.tt.player.ai.Combat.@value(((com.oddlabs.tt.model.Unit)e.getKey())));
                }
            }
            int[] back = com.oddlabs.tt.player.ai.Military.stepTowards(c[0], c[1], staging_x, staging_y, 6);
            creepTick(c, ours);
            com.oddlabs.tt.model.Building site = creep_site;
            foreach (com.oddlabs.tt.model.Unit p_2 in java.lang.JIter.of(intel.sappers))
            {
                com.oddlabs.tt.model.Building tower = null;
                int best = 18 * 18;
                foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(intel.enemy_towers))
                {
                    int d = com.oddlabs.tt.player.ai.MapAnalysis.dist2(t.getGridX(), t.getGridY(), c[0], c[1]);
                    if (d > best)
                    {
                        continue;
                    }
                    bool quiet = !com.oddlabs.tt.player.ai.Intel.isTowerActive(t) || (enemyStrengthNear(t.getGridX(), t.getGridY(), 10) < (float)(0.5f * ours) && ours >= 8.0f);
                    if (quiet)
                    {
                        best = d;
                        tower = t;
                    }
                }
                float? last = ((float?)sapper_orders.get(p_2));
                if (last != null && (float)(ai.time() - ((float)last)) < 3.0f)
                {
                    continue;
                }
                if (tower != null && (p_2.getCurrentController() is com.oddlabs.tt.model.behaviour.AttackController))
                {
                    continue;
                }
                sapper_orders.put(p_2, ai.time());
                if (tower != null)
                {
                    ai.owner().setTarget(com.oddlabs.tt.model.Selectable.newArray(p_2), tower, com.oddlabs.tt.model.Action.ATTACK, true);
                }
                else if (site != null && !((p_2.getCurrentController() is com.oddlabs.tt.model.behaviour.RepairController)))
                {
                    ai.owner().setTarget(com.oddlabs.tt.model.Selectable.newArray(p_2), site, com.oddlabs.tt.model.Action.DEFAULT, false);
                }
                else if (site == null && com.oddlabs.tt.player.ai.MapAnalysis.dist2(p_2.getGridX(), p_2.getGridY(), back[0], back[1]) > 6 * 6)
                {
                    move(p_2, back[0], back[1]);
                }
            }
        }
        // Java: 1605
        public int creepTowerCount()
        {
            creep_towers.removeIf(new java.util.function.Predicate_L((object a0) => { return ((com.oddlabs.tt.model.Building)a0).isDead(); }));
            return creep_towers.size() + ((creep_site != null && !creep_site.isDead() ? 1 : 0));
        }
        // Java: 1614
        public void creepTick(int[] c, float ours)
        {
            creep_towers.removeIf(new java.util.function.Predicate_L((object a0) => { return ((com.oddlabs.tt.model.Building)a0).isDead(); }));
            if (creep_site != null && (creep_site.isDead() || creep_site.isComplete()))
            {
                if (!creep_site.isDead())
                {
                    creep_towers.add(creep_site);
                    ai.log("creep tower up at " + (creep_site.getGridX()) + "," + (creep_site.getGridY()));
                }
                creep_site = null;
            }
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(creep_towers))
            {
                if (com.oddlabs.tt.player.ai.Intel.isTowerManned(t) || tower_assignments.containsValue(t))
                {
                    continue;
                }
                com.oddlabs.tt.model.Unit best = null;
                int best_d = 30 * 30;
                foreach (java.util.Map.Entry e in java.lang.JIter.of(roles.entrySet()))
                {
                    com.oddlabs.tt.model.Unit u = ((com.oddlabs.tt.model.Unit)e.getKey());
                    if (((object)((com.oddlabs.tt.player.ai.Military.Role)e.getValue()) != (object)com.oddlabs.tt.player.ai.Military.Role.ATTACK) || ((object)((com.oddlabs.tt.player.ai.Intel.WarriorState)ai.intel().warrior_states.get(u)) == (object)com.oddlabs.tt.player.ai.Intel.WarriorState.STUNNED))
                    {
                        continue;
                    }
                    int d = com.oddlabs.tt.player.ai.MapAnalysis.dist2(u.getGridX(), u.getGridY(), t.getGridX(), t.getGridY());
                    if (((object)com.oddlabs.tt.player.ai.Intel.warriorType(u) == (object)com.oddlabs.tt.player.ai.Intel.WarriorType.CHICKEN))
                    {
                        d -= 15 * 15;
                    }
                    if (d < best_d)
                    {
                        best_d = d;
                        best = u;
                    }
                }
                if (best != null)
                {
                    roles.put(best, com.oddlabs.tt.player.ai.Military.Role.TOWER);
                    tower_assignments.put(best, t);
                    ai.owner().setTarget(com.oddlabs.tt.model.Selectable.newArray(best), t, com.oddlabs.tt.model.Action.DEFAULT, false);
                }
            }
            com.oddlabs.tt.model.Selectable goal = target;
            if (!ai.strategy().creep_towers || creep_site != null || goal == null || goal.isDead() || (float)(ai.time() - last_creep_try) < 20.0f || ai.intel().sappers.size() < 4)
            {
                return;
            }
            int gx = goal.getGridX();
            int gy = goal.getGridY();
            if (com.oddlabs.tt.player.ai.MapAnalysis.dist2(c[0], c[1], gx, gy) > 30 * 30 || ours < (float)((float)(1.2f * enemyStrengthNear(gx, gy, 20)) + 4.0f))
            {
                return;
            }
            int near = 0;
            foreach (com.oddlabs.tt.model.Building t_2 in java.lang.JIter.of(creep_towers))
            {
                if (com.oddlabs.tt.player.ai.MapAnalysis.dist2(t_2.getGridX(), t_2.getGridY(), gx, gy) <= 20 * 20)
                {
                    near++;
                }
            }
            if (near >= 2)
            {
                return;
            }
            last_creep_try = ai.time();
            int[] spot = creepSpot(gx, gy, c);
            if (spot == null)
            {
                return;
            }
            creep_site = ai.owner().getRace().getBuildingTemplate(com.oddlabs.tt.model.Race.BUILDING_TOWER).create(ai.owner(), spot[0], spot[1]);
            com.oddlabs.tt.model.Unit placer = null;
            int best_d_2 = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Unit p in java.lang.JIter.of(ai.intel().sappers))
            {
                int d_2 = com.oddlabs.tt.player.ai.MapAnalysis.dist2(p.getGridX(), p.getGridY(), spot[0], spot[1]);
                if (d_2 < best_d_2)
                {
                    best_d_2 = d_2;
                    placer = p;
                }
            }
            if (placer != null)
            {
                ai.owner().setTarget(com.oddlabs.tt.model.Selectable.newArray(placer), creep_site, com.oddlabs.tt.model.Action.DEFAULT, false);
            }
            ai.log("sappers raise a tower at " + (spot[0]) + "," + (spot[1]) + " by " + java.lang.JStr.valueOf(goal));
        }
        // Java: 1681
        public int[] creepSpot(int gx, int gy, int[] c)
        {
            com.oddlabs.tt.model.BuildingTemplate tower = ai.owner().getRace().getBuildingTemplate(com.oddlabs.tt.model.Race.BUILDING_TOWER);
            com.oddlabs.tt.player.ai.MapAnalysis map = ai.map();
            int[] best = null;
            float best_score = (float)(-java.lang.Float.MAX_VALUE);
            for (int y = gy - 11; y <= gy + 11; y++)
            {
                for (int x = gx - 11; x <= gx + 11; x++)
                {
                    int d2 = com.oddlabs.tt.player.ai.MapAnalysis.dist2(x, y, gx, gy);
                    if (d2 < 5 * 5 || d2 > 11 * 11 || !map.canPlace(tower, x, y))
                    {
                        continue;
                    }
                    bool covered = false;
                    foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(ai.intel().enemy_towers))
                    {
                        if (com.oddlabs.tt.player.ai.Intel.isTowerActive(t) && com.oddlabs.tt.player.ai.MapAnalysis.dist2(t.getGridX(), t.getGridY(), x, y) <= 12 * 12)
                        {
                            covered = true;
                            break;
                        }
                    }
                    if (covered)
                    {
                        continue;
                    }
                    int trees = map.treesAround(x, y, 8);
                    if (trees < 2)
                    {
                        continue;
                    }
                    float score = (float)((float)((float)(-((float)(java.lang.Math.sqrt(d2)))) - (float)(0.3f * ((float)(java.lang.Math.sqrt(com.oddlabs.tt.player.ai.MapAnalysis.dist2(x, y, c[0], c[1])))))) + java.lang.Math.min(trees, 8));
                    if (score > best_score)
                    {
                        best_score = score;
                        best = new int[] { x, y };
                    }
                }
            }
            return best;
        }
        // Java: 1716
        public static int[] stepTowards(int x, int y, int to_x, int to_y, int cells)
        {
            float dx = to_x - x;
            float dy = to_y - y;
            float len = ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy)))));
            if (len <= cells)
            {
                return new int[] { to_x, to_y };
            }
            return new int[] { x + java.lang.JC.f2i(((float)((float)(dx / len) * cells))), y + java.lang.JC.f2i(((float)((float)(dy / len) * cells))) };
        }
        // Java: 1725
        public void setTarget(com.oddlabs.tt.model.Selectable t)
        {
            target = t;
            if (target_field == null || com.oddlabs.tt.player.ai.MapAnalysis.dist2(target_x, target_y, t.getGridX(), t.getGridY()) > 8 * 8)
            {
                target_x = t.getGridX();
                target_y = t.getGridY();
                target_field = ai.map().computeField(target_x, target_y, java.lang.Integer.MAX_VALUE);
                best_target_dist = java.lang.Integer.MAX_VALUE;
                last_progress_time = ai.time();
            }
        }
        // Java: 1737
        public void easeCaution()
        {
            com.oddlabs.tt.player.Player owner = ai.owner();
            bool capped = owner.getUnitCountContainer().getNumSupplies() >= owner.getWorld().getMaxUnitCount() - 10;
            if (((object)mode_f != (object)com.oddlabs.tt.player.ai.Military.Mode.HOME) || !capped || attack_caution <= 1.0f || ai.strategy().caution_decay <= 1.0f)
            {
                last_caution_ease = ai.time();
                return;
            }
            if ((float)(ai.time() - last_caution_ease) < 60.0f)
            {
                return;
            }
            last_caution_ease = ai.time();
            float before = attack_caution;
            attack_caution = java.lang.Math.max(1.0f, (float)(attack_caution / ai.strategy().caution_decay));
            ai.log(java.lang.JString.format("capped at home: caution %.2f -> %.2f", before, attack_caution));
        }
        // Java: 1752
        public void endAttack()
        {
            if (((object)mode_f != (object)com.oddlabs.tt.player.ai.Military.Mode.HOME))
            {
                ai.log("attack over, back home");
            }
            if (attack_running && ai.strategy().adaptive_caution)
            {
                int kills = ai.owner().getUnitsKilled() - attack_kills_start;
                int losses = ai.owner().getUnitsLost() - attack_losses_start;
                if (kills + losses >= 10)
                {
                    float trade = (float)(kills / ((float)(java.lang.Math.max(1, losses))));
                    float before = attack_caution;
                    if (trade < 1.0f)
                    {
                        attack_caution = java.lang.Math.min(2.5f, (float)(attack_caution * 1.25f));
                    }
                    else if (trade > 1.5f)
                    {
                        attack_caution = java.lang.Math.max(1.0f, (float)(attack_caution / 1.25f));
                    }
                    ai.log(java.lang.JString.format("attack traded %d kills for %d losses: caution %.2f -> %.2f", kills, losses, before, attack_caution));
                }
            }
            attack_running = false;
            foreach (java.util.Map.Entry e in java.lang.JIter.of(roles.entrySet()))
            {
                if (((object)((com.oddlabs.tt.player.ai.Military.Role)e.getValue()) == (object)com.oddlabs.tt.player.ai.Military.Role.ATTACK) || ((object)((com.oddlabs.tt.player.ai.Military.Role)e.getValue()) == (object)com.oddlabs.tt.player.ai.Military.Role.REINFORCE))
                {
                    e.setValue(com.oddlabs.tt.player.ai.Military.Role.ARMY);
                }
            }
            mode_f = com.oddlabs.tt.player.ai.Military.Mode.HOME;
            target = null;
            next_wave_time = (float)(ai.time() + 20.0f);
            strike = false;
        }
        // Java: 1782
        public void attack()
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            java.util.List army = new java.util.ArrayList();
            foreach (java.util.Map.Entry e in java.lang.JIter.of(roles.entrySet()))
            {
                if (((object)((com.oddlabs.tt.player.ai.Military.Role)e.getValue()) == (object)com.oddlabs.tt.player.ai.Military.Role.ATTACK))
                {
                    army.add(((com.oddlabs.tt.model.Unit)e.getKey()));
                }
            }
            if (army.isEmpty())
            {
                endAttack();
                return;
            }
            int[] c = com.oddlabs.tt.player.ai.Military.centroid(army);
            float ours = 0.0f;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(army))
            {
                if (com.oddlabs.tt.player.ai.MapAnalysis.dist2(u.getGridX(), u.getGridY(), c[0], c[1]) <= 18 * 18)
                {
                    ours = (float)(ours + com.oddlabs.tt.player.ai.Combat.@value(u));
                }
            }
            float local_enemy = enemyFightersNear(c[0], c[1], com.oddlabs.tt.player.ai.Military.ENGAGE_RADIUS);
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(intel.enemy_towers))
            {
                if (com.oddlabs.tt.player.ai.MapAnalysis.dist2(t.getGridX(), t.getGridY(), c[0], c[1]) <= com.oddlabs.tt.player.ai.Military.ENGAGE_RADIUS * com.oddlabs.tt.player.ai.Military.ENGAGE_RADIUS)
                {
                    local_enemy = (float)(local_enemy + enemyTowerValue(t));
                }
            }
            ours = (float)(ours * terrainFactor(army, intel.enemy_warriors, c[0], c[1], com.oddlabs.tt.player.ai.Military.ENGAGE_RADIUS));
            bool toot = ai.chieftain().stunReady() && intel.chieftain != null && com.oddlabs.tt.player.ai.MapAnalysis.dist2(intel.chieftain.getGridX(), intel.chieftain.getGridY(), c[0], c[1]) < 20 * 20;
            if (enemyStunReadyNear(c[0], c[1], com.oddlabs.tt.player.ai.Military.ENGAGE_RADIUS + 4))
            {
                local_enemy = (float)(local_enemy * 1.5f);
            }
            float total = 0.0f;
            int stunned_count = 0;
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(army))
            {
                total = (float)(total + com.oddlabs.tt.player.ai.Combat.lastingValue(u_2));
                if (com.oddlabs.tt.player.ai.Intel.isStunned(u_2))
                {
                    stunned_count++;
                }
            }
            bool pinned = stunned_count * 10 > army.size() * 3;
            if (ai.logging() && (float)(ai.time() - last_trace) >= 4.0f)
            {
                last_trace = ai.time();
                int far = 0;
                int fighting = 0;
                foreach (com.oddlabs.tt.model.Unit u_3 in java.lang.JIter.of(army))
                {
                    if (com.oddlabs.tt.player.ai.MapAnalysis.dist2(u_3.getGridX(), u_3.getGridY(), c[0], c[1]) > 16 * 16)
                    {
                        far++;
                    }
                    if (((object)((com.oddlabs.tt.player.ai.Intel.WarriorState)intel.warrior_states.get(u_3)) == (object)com.oddlabs.tt.player.ai.Intel.WarriorState.FIGHT))
                    {
                        fighting++;
                    }
                }
                int enemies = com.oddlabs.tt.player.ai.Combat.countNear(intel.enemy_warriors, c[0], c[1], com.oddlabs.tt.player.ai.Military.ENGAGE_RADIUS);
                int towers = com.oddlabs.tt.player.ai.Combat.countNear(intel.enemy_towers, c[0], c[1], com.oddlabs.tt.player.ai.Military.ENGAGE_RADIUS);
                int target_d = (target_field != null ? target_field.get(c[0], c[1]) : (-1));
                if (enemies > 0)
                {
                    com.oddlabs.tt.player.ai.ExpertAI.debug_battle = new int[] { c[0], c[1] };
                }
                int[] types = new int[3];
                int stunned = 0;
                foreach (com.oddlabs.tt.model.Unit e_2 in java.lang.JIter.of(intel.enemy_warriors))
                {
                    if (com.oddlabs.tt.player.ai.MapAnalysis.dist2(e_2.getGridX(), e_2.getGridY(), c[0], c[1]) > com.oddlabs.tt.player.ai.Military.ENGAGE_RADIUS * com.oddlabs.tt.player.ai.Military.ENGAGE_RADIUS)
                    {
                        continue;
                    }
                    types[com.oddlabs.tt.player.ai.Intel.warriorType(e_2).ordinal()]++;
                    if (com.oddlabs.tt.player.ai.Intel.isStunned(e_2))
                    {
                        stunned++;
                    }
                }
                int our_stunned = 0;
                foreach (com.oddlabs.tt.model.Unit u_4 in java.lang.JIter.of(army))
                {
                    if (com.oddlabs.tt.player.ai.Intel.isStunned(u_4))
                    {
                        our_stunned++;
                    }
                }
                ai.log(java.lang.JString.format("battle: army %d (%.1f, %d far, %d fighting, %d stunned) at %d,%d h%.0f, %dm to target; near: %d warriors (r%d i%d c%d, %d stunned) h%.0f %d towers (%.1f)", army.size(), total, far, fighting, our_stunned, c[0], c[1], meanHeight(army, c[0], c[1], 18), target_d, enemies, types[0], types[1], types[2], stunned, meanHeight(intel.enemy_warriors, c[0], c[1], com.oddlabs.tt.player.ai.Military.ENGAGE_RADIUS), towers, local_enemy));
            }
            java.util.List stunned_2 = (pinned || !ai.strategy().exploit_stun ? java.util.List_.of() : stunnedEnemiesNear(c[0], c[1], 36));
            if (!stunned_2.isEmpty())
            {
                float asleep = 0.0f;
                foreach (com.oddlabs.tt.model.Unit e_3 in java.lang.JIter.of(stunned_2))
                {
                    asleep = (float)(asleep + com.oddlabs.tt.player.ai.Combat.lastingValue(e_3));
                }
                float awake = enemyFightersNear(c[0], c[1], 36);
                foreach (com.oddlabs.tt.model.Building t_2 in java.lang.JIter.of(intel.enemy_towers))
                {
                    if (com.oddlabs.tt.player.ai.MapAnalysis.dist2(t_2.getGridX(), t_2.getGridY(), c[0], c[1]) <= 36 * 36)
                    {
                        awake = (float)(awake + enemyTowerValue(t_2));
                    }
                }
                if (asleep >= 3.0f && total >= (float)(0.8f * awake) && !enemyStunReadyNear(c[0], c[1], 45))
                {
                    if ((float)(ai.time() - last_charge_log) > 10.0f)
                    {
                        last_charge_log = ai.time();
                        ai.log(java.lang.JString.format("charging %d stunned enemies (%.1f asleep, %.1f awake, army %.1f)", stunned_2.size(), asleep, awake, total));
                    }
                    int[] sc = com.oddlabs.tt.player.ai.Military.centroid(stunned_2);
                    java.util.List prey = new java.util.ArrayList(stunned_2);
                    if (ai.strategy().charge_mixed)
                    {
                        foreach (com.oddlabs.tt.model.Unit e_4 in java.lang.JIter.of(intel.enemy_warriors))
                        {
                            if (!com.oddlabs.tt.player.ai.Intel.isStunned(e_4) && com.oddlabs.tt.player.ai.MapAnalysis.dist2(e_4.getGridX(), e_4.getGridY(), c[0], c[1]) <= (com.oddlabs.tt.player.ai.Military.ENGAGE_RADIUS + 8) * (com.oddlabs.tt.player.ai.Military.ENGAGE_RADIUS + 8))
                            {
                                prey.add(e_4);
                            }
                        }
                    }
                    engageSpread(army, prey, sc[0], sc[1], true);
                    huntChieftains(army);
                    return;
                }
            }
            if (ai.strategy().siege && !pinned && siege(army, c, total))
            {
                return;
            }
            if (!toot && !pinned && ai.strategy().precontact_ratio > 0.0f && !anyFighting(army))
            {
                float wide = enemyFightersNear(c[0], c[1], 36);
                foreach (com.oddlabs.tt.model.Building t_3 in java.lang.JIter.of(intel.enemy_towers))
                {
                    if (com.oddlabs.tt.player.ai.MapAnalysis.dist2(t_3.getGridX(), t_3.getGridY(), c[0], c[1]) <= 36 * 36)
                    {
                        wide = (float)(wide + enemyTowerValue(t_3));
                    }
                }
                float terrain = terrainFactor(army, intel.enemy_warriors, c[0], c[1], 36);
                if (wide > 0.0f && (float)(total * terrain) < (float)(ai.strategy().precontact_ratio * wide))
                {
                    ai.log(java.lang.JString.format("turning back before contact: %.1f against %.1f", (float)(total * terrain), wide));
                    beginRetreat();
                    return;
                }
            }
            bool outmatched = local_enemy > (float)(ai.strategy().retreat_ratio * java.lang.Math.max(ours, 1.0f));
            bool worn = total < (float)(0.2f * attack_initial_strength) && local_enemy > total;
            if (!toot && !pinned && (outmatched || worn) && pillage(army, c, total))
            {
                return;
            }
            if (!toot && !pinned && outmatched)
            {
                ai.log(java.lang.JString.format("retreat: local %.1f vs enemy %.1f (army %.1f of %.1f)", ours, local_enemy, total, attack_initial_strength));
                beginRetreat();
                return;
            }
            if (!pinned && total < (float)(0.2f * attack_initial_strength) && local_enemy > total)
            {
                ai.log(java.lang.JString.format("retreat: worn down to %.1f of %.1f", total, attack_initial_strength));
                beginRetreat();
                return;
            }
            if (target == null || target.isDead())
            {
                if (strike)
                {
                    endAttack();
                    return;
                }
                com.oddlabs.tt.model.Selectable next = chooseTarget(c[0], c[1]);
                if (next == null)
                {
                    endAttack();
                    return;
                }
                setTarget(next);
            }
            if (holdForApproachingEnemy(army, c))
            {
                return;
            }
            int[] focus = enemyFocus(c[0], c[1]);
            if (focus != null)
            {
                java.util.List fighters = new java.util.ArrayList();
                foreach (com.oddlabs.tt.model.Unit e_5 in java.lang.JIter.of(intel.enemy_warriors))
                {
                    if (com.oddlabs.tt.player.ai.MapAnalysis.dist2(e_5.getGridX(), e_5.getGridY(), c[0], c[1]) <= (com.oddlabs.tt.player.ai.Military.ENGAGE_RADIUS + 8) * (com.oddlabs.tt.player.ai.Military.ENGAGE_RADIUS + 8))
                    {
                        fighters.add(e_5);
                    }
                }
                foreach (com.oddlabs.tt.model.Unit e_6 in java.lang.JIter.of(intel.enemy_chieftains))
                {
                    if (com.oddlabs.tt.player.ai.MapAnalysis.dist2(e_6.getGridX(), e_6.getGridY(), c[0], c[1]) <= (com.oddlabs.tt.player.ai.Military.ENGAGE_RADIUS + 8) * (com.oddlabs.tt.player.ai.Military.ENGAGE_RADIUS + 8))
                    {
                        fighters.add(e_6);
                    }
                }
                engageSpread(army, fighters, focus[0], focus[1], true);
                huntChieftains(army);
                return;
            }
            com.oddlabs.tt.player.ai.DistanceField field = target_field;
            int dist = com.oddlabs.tt.player.ai.DistanceField.UNREACHABLE;
            int[] waypoint = new int[] { target_x, target_y };
            com.oddlabs.tt.model.Unit pivot = null;
            int pivot_dist = 0;
            if (field != null)
            {
                java.util.List ranked = new java.util.ArrayList(army);
                ranked.removeIf(new java.util.function.Predicate_L((object u_5_l) =>
                {
                    com.oddlabs.tt.model.Unit u_5 = (com.oddlabs.tt.model.Unit)u_5_l;
                    return field.getAround(u_5.getGridX(), u_5.getGridY(), 1) == com.oddlabs.tt.player.ai.DistanceField.UNREACHABLE;
                }));
                ranked.sort(new java.util.Comparator_L((object a_l, object b_l) =>
                {
                    com.oddlabs.tt.model.Unit a = (com.oddlabs.tt.model.Unit)a_l;
                    com.oddlabs.tt.model.Unit b = (com.oddlabs.tt.model.Unit)b_l;
                    return java.lang.Integer.compare(field.getAround(a.getGridX(), a.getGridY(), 1), field.getAround(b.getGridX(), b.getGridY(), 1));
                }));
                if (!ranked.isEmpty())
                {
                    pivot = ((com.oddlabs.tt.model.Unit)ranked.get(ranked.size() / 3));
                    pivot_dist = field.getAround(pivot.getGridX(), pivot.getGridY(), 1);
                    dist = pivot_dist;
                }
            }
            int lead = 22 + 3 * java.lang.JC.d2i(java.lang.Math.sqrt(army.size()));
            if (pivot != null && dist >= 30)
            {
                waypoint = field.stepTowardsSource(pivot.getGridX(), pivot.getGridY(), lead);
            }
            foreach (com.oddlabs.tt.model.Unit u_6 in java.lang.JIter.of(army))
            {
                int d = (field != null ? field.getAround(u_6.getGridX(), u_6.getGridY(), 1) : com.oddlabs.tt.player.ai.DistanceField.UNREACHABLE);
                if (pivot != null && d != com.oddlabs.tt.player.ai.DistanceField.UNREACHABLE && d < pivot_dist - 24)
                {
                    continue;
                }
                attackGround(u_6, waypoint[0], waypoint[1], false);
            }
            if (dist != com.oddlabs.tt.player.ai.DistanceField.UNREACHABLE && dist < best_target_dist - 20)
            {
                best_target_dist = dist;
                last_progress_time = ai.time();
            }
            if ((float)(ai.time() - last_progress_time) > 75.0f && local_enemy == 0.0f)
            {
                ai.log("attack stalled");
                beginRetreat();
            }
        }
        // Java: 1990
        public float terrainFactor(java.util.List ours, java.util.List theirs, int x, int y, int radius)
        {
            float h_ours = meanHeight(ours, x, y, radius);
            float h_theirs = meanHeight(theirs, x, y, radius + 6);
            if (java.lang.Float.isNaN(h_ours) || java.lang.Float.isNaN(h_theirs))
            {
                return 1.0f;
            }
            float bonus = java.lang.Math.clamp((float)(((float)(h_ours - h_theirs)) / 80.0f), (float)(-0.25f), 0.25f);
            return ((float)(java.lang.Math.sqrt((float)(((float)(0.75f + bonus)) / ((float)(0.75f - bonus))))));
        }
        // Java: 2000
        public float meanHeight(java.util.List units, int x, int y, int radius)
        {
            float sum = 0.0f;
            int n = 0;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(units))
            {
                if (u.isDead() || com.oddlabs.tt.player.ai.MapAnalysis.dist2(x, y, u.getGridX(), u.getGridY()) > radius * radius)
                {
                    continue;
                }
                sum = (float)(sum + ai.map().height(u.getGridX(), u.getGridY()));
                n++;
            }
            return (n == 0 ? java.lang.Float.NaN : (float)(sum / n));
        }
        // Java: 2016
        public bool holdForApproachingEnemy(java.util.List army, int[] c)
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            int far = com.oddlabs.tt.player.ai.Combat.countNear(intel.enemy_warriors, c[0], c[1], 26);
            int near = 0;
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(intel.enemy_warriors))
            {
                if (e.isDead())
                {
                    continue;
                }
                foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(army))
                {
                    if (com.oddlabs.tt.player.ai.MapAnalysis.dist2(u.getGridX(), u.getGridY(), e.getGridX(), e.getGridY()) <= 11 * 11)
                    {
                        near++;
                        break;
                    }
                }
                if (near > 0)
                {
                    break;
                }
            }
            if (far < 3 || near > 0)
            {
                hold_until = (float)(-1.0f);
                return false;
            }
            int[] enemy = com.oddlabs.tt.player.ai.Military.nearestGroup(intel.enemy_warriors, c[0], c[1], 26);
            if (enemy == null)
            {
                return false;
            }
            int d = com.oddlabs.tt.player.ai.MapAnalysis.dist2(c[0], c[1], enemy[0], enemy[1]);
            bool closing = d < last_enemy_d2 - 4;
            last_enemy_d2 = d;
            if (hold_until < 0.0f)
            {
                if (!closing)
                {
                    return false;
                }
                hold_until = (float)(ai.time() + 12.0f);
                hold_spot = ai.map().highGround(c[0], c[1], 6);
                ai.log(java.lang.JString.format("holding at %d,%d (%.0fm up) for enemy army at %d,%d", hold_spot[0], hold_spot[1], (float)(ai.map().height(hold_spot[0], hold_spot[1]) - ai.map().height(c[0], c[1])), enemy[0], enemy[1]));
            }
            if (ai.time() > hold_until)
            {
                return false;
            }
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(army))
            {
                attackGround(u_2, hold_spot[0], hold_spot[1], false);
            }
            return true;
        }
        // Java: 2061
        public bool pillage(java.util.List army, int[] c, float total)
        {
            if (!ai.strategy().pillage || total < 6.0f || enemyStrengthNear(c[0], c[1], 30) > (float)(0.5f * total))
            {
                return false;
            }
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            java.util.List prey = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit p in java.lang.JIter.of(intel.enemy_peons))
            {
                if (p.isDead() || com.oddlabs.tt.player.ai.MapAnalysis.dist2(p.getGridX(), p.getGridY(), c[0], c[1]) > 45 * 45)
                {
                    continue;
                }
                bool covered = false;
                foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(intel.enemy_towers))
                {
                    if (com.oddlabs.tt.player.ai.Intel.isTowerActive(t) && com.oddlabs.tt.player.ai.MapAnalysis.dist2(t.getGridX(), t.getGridY(), p.getGridX(), p.getGridY()) <= 11 * 11)
                    {
                        covered = true;
                        break;
                    }
                }
                if (!covered && enemyStrengthNear(p.getGridX(), p.getGridY(), 16) < (float)(0.5f * total))
                {
                    prey.add(p);
                }
            }
            if (prey.size() < 3)
            {
                return false;
            }
            if ((float)(ai.time() - last_pillage_log) > 15.0f)
            {
                last_pillage_log = ai.time();
                ai.log(java.lang.JString.format("pillaging %d peons outside the towers with %.1f", prey.size(), total));
            }
            int[] pc = com.oddlabs.tt.player.ai.Military.centroid(prey);
            engageSpread(army, prey, pc[0], pc[1], false);
            return true;
        }
        // Java: 2095
        public bool chargeStunned(java.util.List units, int x, int y)
        {
            java.util.List stunned = stunnedEnemiesNear(x, y, 30);
            if (stunned.isEmpty() || units.isEmpty())
            {
                return false;
            }
            float asleep = 0.0f;
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(stunned))
            {
                asleep = (float)(asleep + com.oddlabs.tt.player.ai.Combat.lastingValue(e));
            }
            float ours = 0.0f;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(units))
            {
                ours = (float)(ours + com.oddlabs.tt.player.ai.Combat.lastingValue(u));
            }
            float awake = enemyFightersNear(x, y, 30);
            if (asleep < 3.0f || ours < (float)(0.8f * awake) || enemyStunReadyNear(x, y, 40))
            {
                return false;
            }
            int[] sc = com.oddlabs.tt.player.ai.Military.centroid(stunned);
            engageSpread(units, stunned, sc[0], sc[1], true);
            huntChieftains(units);
            return true;
        }
        // Java: 2115
        public int[] blastPlay()
        {
            return (ai.time() < blast_play_until ? new int[] { threat_x, threat_y } : (int[])(null));
        }
        // Java: 2123
        public bool blastDefense(java.util.List defenders, float enemy)
        {
            com.oddlabs.tt.player.ai.Strategy strategy = ai.strategy();
            float now = ai.time();
            if (!strategy.blast || !strategy.blast_defense)
            {
                return false;
            }
            if (now >= blast_play_until)
            {
                if (threat_level < 2 || enemy < strategy.blast_min || (float)(now - last_blast_play) < 60.0f || !ai.chieftain().blastReady())
                {
                    return false;
                }
                blast_play_until = (float)(now + strategy.blast_play_time);
                last_blast_play = now;
                ai.log(java.lang.JString.format("blast play against %.1f at %d,%d", enemy, threat_x, threat_y));
            }
            if (!ai.chieftain().blastReady() && ai.chieftain().sinceCast() > 5.0f)
            {
                blast_play_until = now;
                return false;
            }
            com.oddlabs.tt.model.Unit chief = ai.intel().chieftain;
            int cx = (chief != null ? chief.getGridX() : threat_x);
            int cy = (chief != null ? chief.getGridY() : threat_y);
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(defenders))
            {
                if (com.oddlabs.tt.player.ai.Intel.isStunned(u))
                {
                    continue;
                }
                int d2 = com.oddlabs.tt.player.ai.MapAnalysis.dist2(u.getGridX(), u.getGridY(), cx, cy);
                if (d2 >= 22 * 22)
                {
                    continue;
                }
                float dx = u.getGridX() - threat_x;
                float dy = u.getGridY() - threat_y;
                float len = java.lang.Math.max(1.0f, ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy))))));
                int[] to = new int[] { java.lang.Math.round((float)(threat_x + (float)((float)(dx / len) * 30.0f))), java.lang.Math.round((float)(threat_y + (float)((float)(dy / len) * 30.0f))) };
                int size = ai.map().getSize();
                move(u, java.lang.Math.clamp(to[0], 0, size - 1), java.lang.Math.clamp(to[1], 0, size - 1));
            }
            return true;
        }
        // Java: 2161
        public bool sieging()
        {
            return ((object)mode_f == (object)com.oddlabs.tt.player.ai.Military.Mode.ATTACK) && siege_start >= 0.0f;
        }
        // Java: 2165
        public void endSiege()
        {
            if (siege_start >= 0.0f)
            {
                ai.log(java.lang.JString.format("siege over after %.0fs", (float)(ai.time() - siege_start)));
            }
            siege_start = (float)(-1.0f);
            siege_assign.clear();
        }
        // Java: 2177
        public bool siege(java.util.List army, int[] c, float total)
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            float now = ai.time();
            java.util.List awake = new java.util.ArrayList();
            java.util.List helpless = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(intel.enemy_towers))
            {
                if (!com.oddlabs.tt.player.ai.Intel.isTowerManned(t) || com.oddlabs.tt.player.ai.MapAnalysis.dist2(t.getGridX(), t.getGridY(), c[0], c[1]) > 34 * 34)
                {
                    continue;
                }
                if (com.oddlabs.tt.player.ai.Intel.isTowerActive(t))
                {
                    awake.add(t);
                }
                else
                {
                    helpless.add(t);
                }
            }
            float field = (siege_start >= 0.0f ? 0.7f : 0.5f);
            if ((awake.isEmpty() && helpless.isEmpty()) || enemyStrengthNear(c[0], c[1], 30) > (float)(field * total) || total < 10.0f)
            {
                endSiege();
                return false;
            }
            com.oddlabs.tt.model.Unit chief = intel.chieftain;
            bool stunner = chief != null && !chief.isDead() && chief.getHitPoints() > 24 && ((object)ai.owner().getRace() == (object)ai.owner().getWorld().getRacesResources().getRace(com.oddlabs.tt.model.RacesResources.RACE_VIKINGS));
            float since = ai.chieftain().sinceCast();
            java.util.List goals = new java.util.ArrayList(helpless);
            foreach (com.oddlabs.tt.model.Building t_2 in java.lang.JIter.of(awake))
            {
                bool weak = t_2.getHitPoints() <= 50 && siege_assign.containsValue(t_2);
                bool about_to = stunner && since >= 1.8f && since < 4.5f && com.oddlabs.tt.player.ai.MapAnalysis.dist2(chief.getGridX(), chief.getGridY(), t_2.getGridX(), t_2.getGridY()) <= 300;
                if (weak || about_to)
                {
                    goals.add(t_2);
                }
            }
            siege_assign.entrySet().removeIf(new java.util.function.Predicate_L((object e_l) =>
            {
                java.util.Map.Entry e = (java.util.Map.Entry)e_l;
                return ((com.oddlabs.tt.model.Unit)e.getKey()).isDead() || !goals.contains(((com.oddlabs.tt.model.Building)e.getValue()));
            }));
            if (!goals.isEmpty())
            {
                if (siege_start < 0.0f)
                {
                    siege_start = now;
                }
                siege_progress = now;
                if ((float)(now - last_pull_log) >= 5.0f)
                {
                    last_pull_log = now;
                    int hp = 0;
                    foreach (com.oddlabs.tt.model.Building t_3 in java.lang.JIter.of(goals))
                    {
                        hp += t_3.getHitPoints();
                    }
                    ai.log(java.lang.JString.format("pulling down %d towers (%d stunned, %d hp) with %.1f", goals.size(), helpless.size(), hp, total));
                }
                pullDown(army, goals);
                return true;
            }
            bool pending = since < 5.0f;
            if (!stunner || (!pending && chief.getMagicProgress(com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_STUN) < 0.5f) || now < siege_cooldown)
            {
                endSiege();
                return false;
            }
            if (siege_start < 0.0f)
            {
                siege_start = now;
                siege_progress = now;
                ai.log(java.lang.JString.format("siege of %d towers with %.1f", awake.size(), total));
            }
            if ((float)(now - siege_progress) > ai.strategy().siege_patience)
            {
                ai.log("siege gives up");
                siege_cooldown = (float)(now + 60.0f);
                endSiege();
                return false;
            }
            siege_assign.clear();
            com.oddlabs.tt.model.Building first = null;
            int first_d = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Building t_4 in java.lang.JIter.of(awake))
            {
                int d = com.oddlabs.tt.player.ai.MapAnalysis.dist2(t_4.getGridX(), t_4.getGridY(), c[0], c[1]);
                if (d < first_d)
                {
                    first_d = d;
                    first = t_4;
                }
            }
            int hx = c[0];
            int hy = c[1];
            if (first != null && first_d > 0)
            {
                float len = ((float)(java.lang.Math.sqrt(first_d)));
                hx = first.getGridX() + java.lang.Math.round((float)((float)((c[0] - first.getGridX()) / len) * (com.oddlabs.tt.player.ai.Military.SIEGE_HOLD + 1)));
                hy = first.getGridY() + java.lang.Math.round((float)((float)((c[1] - first.getGridY()) / len) * (com.oddlabs.tt.player.ai.Military.SIEGE_HOLD + 1)));
            }
            int[] hold = outOfReach(hx, hy, awake);
            int r2 = com.oddlabs.tt.player.ai.Military.TOWER_REACH * com.oddlabs.tt.player.ai.Military.TOWER_REACH;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(army))
            {
                if (com.oddlabs.tt.player.ai.Intel.isStunned(u))
                {
                    continue;
                }
                bool exposed = false;
                foreach (com.oddlabs.tt.model.Building t_5 in java.lang.JIter.of(awake))
                {
                    exposed |= com.oddlabs.tt.player.ai.MapAnalysis.dist2(u.getGridX(), u.getGridY(), t_5.getGridX(), t_5.getGridY()) <= r2;
                }
                if (exposed)
                {
                    move(u, hold[0], hold[1]);
                }
                else
                {
                    attackGround(u, hold[0], hold[1], false);
                }
            }
            return true;
        }
        // Java: 2283
        public int[] outOfReach(int x, int y, java.util.List towers)
        {
            for (int pass = 0; pass < 4; pass++)
            {
                com.oddlabs.tt.model.Building near = null;
                int best = com.oddlabs.tt.player.ai.Military.SIEGE_HOLD * com.oddlabs.tt.player.ai.Military.SIEGE_HOLD;
                foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(towers))
                {
                    int d = com.oddlabs.tt.player.ai.MapAnalysis.dist2(x, y, t.getGridX(), t.getGridY());
                    if (d < best)
                    {
                        best = d;
                        near = t;
                    }
                }
                if (near == null)
                {
                    break;
                }
                float dx = x - near.getGridX();
                float dy = y - near.getGridY();
                if ((float)((float)(dx * dx) + (float)(dy * dy)) < 1.0f)
                {
                    dx = staging_x - near.getGridX();
                    dy = staging_y - near.getGridY();
                }
                float len = java.lang.Math.max(0.1f, ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy))))));
                x = near.getGridX() + java.lang.Math.round((float)((float)(dx / len) * (com.oddlabs.tt.player.ai.Military.SIEGE_HOLD + 1)));
                y = near.getGridY() + java.lang.Math.round((float)((float)(dy / len) * (com.oddlabs.tt.player.ai.Military.SIEGE_HOLD + 1)));
            }
            int size = ai.map().getSize();
            return new int[] { java.lang.Math.clamp(x, 0, size - 1), java.lang.Math.clamp(y, 0, size - 1) };
        }
        // Java: 2314
        public void pullDown(java.util.List army, java.util.List helpless)
        {
            java.util.List free = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(army))
            {
                if (!com.oddlabs.tt.player.ai.Intel.isStunned(u) && !u.isDead())
                {
                    free.add(u);
                }
            }
            int per = java.lang.Math.max(6, java.lang.Math.min(14, free.size() / helpless.size()));
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(helpless))
            {
                free.sort(new java.util.Comparator_L((object a_l, object b_l) =>
                {
                    com.oddlabs.tt.model.Unit a = (com.oddlabs.tt.model.Unit)a_l;
                    com.oddlabs.tt.model.Unit b = (com.oddlabs.tt.model.Unit)b_l;
                    return java.lang.Integer.compare(com.oddlabs.tt.player.ai.MapAnalysis.dist2(a.getGridX(), a.getGridY(), t.getGridX(), t.getGridY()), com.oddlabs.tt.player.ai.MapAnalysis.dist2(b.getGridX(), b.getGridY(), t.getGridX(), t.getGridY()));
                }));
                java.util.List squad = new java.util.ArrayList();
                for (int k = 0; k < per && !free.isEmpty(); k++)
                {
                    com.oddlabs.tt.model.Unit u_2 = ((com.oddlabs.tt.model.Unit)free.removeFirst());
                    if (((object)((com.oddlabs.tt.model.Building)siege_assign.get(u_2)) == (object)t))
                    {
                        continue;
                    }
                    siege_assign.put(u_2, t);
                    squad.add(u_2);
                }
                if (!squad.isEmpty())
                {
                    ai.owner().setTarget(((com.oddlabs.tt.model.Selectable[])squad.toArray(new com.oddlabs.tt.model.Selectable[0])), t, com.oddlabs.tt.model.Action.ATTACK, false);
                }
            }
            if (!free.isEmpty())
            {
                int[] tc = com.oddlabs.tt.player.ai.Military.centroid(new java.util.ArrayList(helpless));
                foreach (com.oddlabs.tt.model.Unit u_3 in java.lang.JIter.of(free))
                {
                    attackGround(u_3, tc[0], tc[1], true);
                }
            }
        }
        // Java: 2343
        public java.util.List stunnedEnemiesNear(int x, int y, int radius)
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            java.util.List stunned = new java.util.ArrayList();
            int r2 = radius * radius;
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(intel.enemy_warriors))
            {
                if (!e.isDead() && com.oddlabs.tt.player.ai.Intel.isStunned(e) && com.oddlabs.tt.player.ai.MapAnalysis.dist2(x, y, e.getGridX(), e.getGridY()) <= r2)
                {
                    stunned.add(e);
                }
            }
            foreach (com.oddlabs.tt.model.Unit e_2 in java.lang.JIter.of(intel.enemy_chieftains))
            {
                if (!e_2.isDead() && com.oddlabs.tt.player.ai.Intel.isStunned(e_2) && com.oddlabs.tt.player.ai.MapAnalysis.dist2(x, y, e_2.getGridX(), e_2.getGridY()) <= r2)
                {
                    stunned.add(e_2);
                }
            }
            return stunned;
        }
        // Java: 2357
        public int[] enemyFocus(int x, int y)
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            int[] w = com.oddlabs.tt.player.ai.Military.nearestGroup(intel.enemy_warriors, x, y, com.oddlabs.tt.player.ai.Military.ENGAGE_RADIUS);
            if (w != null)
            {
                return w;
            }
            int[] ch = com.oddlabs.tt.player.ai.Military.nearestGroup(intel.enemy_chieftains, x, y, com.oddlabs.tt.player.ai.Military.ENGAGE_RADIUS);
            if (ch != null)
            {
                return ch;
            }
            int[] t = com.oddlabs.tt.player.ai.Military.nearestGroup(intel.enemy_towers, x, y, com.oddlabs.tt.player.ai.Military.ENGAGE_RADIUS);
            if (t != null)
            {
                return t;
            }
            int[] p = com.oddlabs.tt.player.ai.Military.nearestGroup(intel.enemy_peons, x, y, 14);
            if (p != null)
            {
                return p;
            }
            return com.oddlabs.tt.player.ai.Military.nearestGroup(intel.enemy_buildings, x, y, 14);
        }
        // Java: 2374
        public static int[] nearestGroup(java.util.List units, int x, int y, int radius)
        {
            com.oddlabs.tt.model.Selectable nearest_v = null;
            int best = radius * radius;
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(units))
            {
                if (s.isDead())
                {
                    continue;
                }
                int d = com.oddlabs.tt.player.ai.MapAnalysis.dist2(x, y, s.getGridX(), s.getGridY());
                if (d <= best)
                {
                    best = d;
                    nearest_v = s;
                }
            }
            if (nearest_v == null)
            {
                return null;
            }
            long sx = 0;
            long sy = 0;
            int n = 0;
            foreach (com.oddlabs.tt.model.Selectable s_2 in java.lang.JIter.of(units))
            {
                if (s_2.isDead())
                {
                    continue;
                }
                if (com.oddlabs.tt.player.ai.MapAnalysis.dist2(nearest_v.getGridX(), nearest_v.getGridY(), s_2.getGridX(), s_2.getGridY()) <= 8 * 8)
                {
                    sx += s_2.getGridX();
                    sy += s_2.getGridY();
                    n++;
                }
            }
            return new int[] { unchecked((int)((sx / n))), unchecked((int)((sy / n))) };
        }
        // Java: 2405
        public void beginRetreat()
        {
            mode_f = com.oddlabs.tt.player.ai.Military.Mode.RETREAT;
            foreach (java.util.Map.Entry e in java.lang.JIter.of(roles.entrySet()))
            {
                if (((object)((com.oddlabs.tt.player.ai.Military.Role)e.getValue()) == (object)com.oddlabs.tt.player.ai.Military.Role.REINFORCE))
                {
                    e.setValue(com.oddlabs.tt.player.ai.Military.Role.ARMY);
                    move(((com.oddlabs.tt.model.Unit)e.getKey()), staging_x, staging_y);
                }
                else if (((object)((com.oddlabs.tt.player.ai.Military.Role)e.getValue()) == (object)com.oddlabs.tt.player.ai.Military.Role.ATTACK))
                {
                    move(((com.oddlabs.tt.model.Unit)e.getKey()), staging_x, staging_y);
                }
            }
        }
        // Java: 2421
        public void considerReinforcing()
        {
            java.util.List group = new java.util.ArrayList();
            float home = 0.0f;
            foreach (java.util.Map.Entry e in java.lang.JIter.of(roles.entrySet()))
            {
                if (((object)((com.oddlabs.tt.player.ai.Military.Role)e.getValue()) != (object)com.oddlabs.tt.player.ai.Military.Role.ARMY))
                {
                    continue;
                }
                com.oddlabs.tt.model.Unit u = ((com.oddlabs.tt.model.Unit)e.getKey());
                com.oddlabs.tt.player.ai.Intel.WarriorState s = ((com.oddlabs.tt.player.ai.Intel.WarriorState)ai.intel().warrior_states.get(u));
                if (((object)s == (object)com.oddlabs.tt.player.ai.Intel.WarriorState.STUNNED) || ((object)s == (object)com.oddlabs.tt.player.ai.Intel.WarriorState.ENTER))
                {
                    continue;
                }
                group.add(u);
                home = (float)(home + com.oddlabs.tt.player.ai.Combat.@value(u));
            }
            if (group.isEmpty())
            {
                return;
            }
            float away = attackStrength();
            com.oddlabs.tt.player.Player owner = ai.owner();
            bool capped = owner.getUnitCountContainer().getNumSupplies() >= owner.getWorld().getMaxUnitCount() - 10;
            if (home < java.lang.Math.max(12.0f, (float)(ai.strategy().reinforce_ratio * away)) && !(capped && home >= 12.0f))
            {
                return;
            }
            if (!capped && ai.enemiesAlive() > 1 && !ai.strategy().reinforce_multi)
            {
                return;
            }
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(group))
            {
                roles.put(u_2, com.oddlabs.tt.player.ai.Military.Role.REINFORCE);
            }
            ai.log(java.lang.JString.format("reinforcing the attack (%.1f) with %.1f", away, home));
        }
        // Java: 2450
        public void reinforce()
        {
            int[] front = null;
            foreach (java.util.Map.Entry e in java.lang.JIter.of(roles.entrySet()))
            {
                if (((object)((com.oddlabs.tt.player.ai.Military.Role)e.getValue()) != (object)com.oddlabs.tt.player.ai.Military.Role.REINFORCE))
                {
                    continue;
                }
                if (front == null)
                {
                    front = attackCenter();
                }
                com.oddlabs.tt.model.Unit u = ((com.oddlabs.tt.model.Unit)e.getKey());
                if (front == null)
                {
                    e.setValue(com.oddlabs.tt.player.ai.Military.Role.ARMY);
                    continue;
                }
                if (com.oddlabs.tt.player.ai.MapAnalysis.dist2(u.getGridX(), u.getGridY(), front[0], front[1]) <= 20 * 20)
                {
                    e.setValue(com.oddlabs.tt.player.ai.Military.Role.ATTACK);
                    attack_initial_strength += com.oddlabs.tt.player.ai.Combat.@value(u);
                    continue;
                }
                attackGround(u, front[0], front[1], false);
            }
        }
        // Java: 2471
        public void retreat()
        {
            int n = 0;
            int home = 0;
            foreach (java.util.Map.Entry e in java.lang.JIter.of(roles.entrySet()))
            {
                if (((object)((com.oddlabs.tt.player.ai.Military.Role)e.getValue()) != (object)com.oddlabs.tt.player.ai.Military.Role.ATTACK))
                {
                    continue;
                }
                com.oddlabs.tt.model.Unit u = ((com.oddlabs.tt.model.Unit)e.getKey());
                n++;
                if (com.oddlabs.tt.player.ai.MapAnalysis.dist2(u.getGridX(), u.getGridY(), staging_x, staging_y) <= 14 * 14)
                {
                    home++;
                }
                else if (((object)((com.oddlabs.tt.player.ai.Intel.WarriorState)ai.intel().warrior_states.get(u)) == (object)com.oddlabs.tt.player.ai.Intel.WarriorState.IDLE))
                {
                    move(u, staging_x, staging_y);
                }
            }
            if (n == 0 || home >= n * 7 / 10)
            {
                endAttack();
            }
        }
        // Java: 2491
        public void considerRaid()
        {
            com.oddlabs.tt.player.ai.Strategy strategy = ai.strategy();
            if (ai.time() < strategy.raid_time || (float)(ai.time() - last_raid_end) < 40.0f)
            {
                return;
            }
            if (countRole(com.oddlabs.tt.player.ai.Military.Role.RAID) > 0)
            {
                return;
            }
            int army = countRole(com.oddlabs.tt.player.ai.Military.Role.ARMY);
            if (army < strategy.raid_size + 6)
            {
                return;
            }
            int[] spot = raidSpot(staging_x, staging_y, (float)(strategy.raid_size * com.oddlabs.tt.player.ai.Combat.IRON));
            if (spot == null)
            {
                return;
            }
            java.util.List squad = new java.util.ArrayList();
            foreach (java.util.Map.Entry e in java.lang.JIter.of(roles.entrySet()))
            {
                if (squad.size() >= strategy.raid_size)
                {
                    break;
                }
                com.oddlabs.tt.model.Unit u = ((com.oddlabs.tt.model.Unit)e.getKey());
                if (((object)((com.oddlabs.tt.player.ai.Military.Role)e.getValue()) == (object)com.oddlabs.tt.player.ai.Military.Role.ARMY) && ((object)com.oddlabs.tt.player.ai.Intel.warriorType(u) == (object)com.oddlabs.tt.player.ai.Intel.WarriorType.IRON) && ((object)((com.oddlabs.tt.player.ai.Intel.WarriorState)ai.intel().warrior_states.get(u)) == (object)com.oddlabs.tt.player.ai.Intel.WarriorState.IDLE))
                {
                    squad.add(u);
                }
            }
            if (squad.size() < strategy.raid_size)
            {
                return;
            }
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(squad))
            {
                roles.put(u_2, com.oddlabs.tt.player.ai.Military.Role.RAID);
                attackGround(u_2, spot[0], spot[1], true);
            }
            raid_x = spot[0];
            raid_y = spot[1];
            raid_start = ai.time();
            ai.log("raid with " + (squad.size()) + " on peons at " + (raid_x) + "," + (raid_y));
        }
        // Java: 2525
        public int[] raidSpot(int from_x, int from_y, float strength)
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            int[] best = null;
            float best_score = java.lang.Float.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Unit p in java.lang.JIter.of(intel.enemy_peons))
            {
                int px = p.getGridX();
                int py = p.getGridY();
                float danger = enemyStrengthNear(px, py, 30);
                foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(intel.enemy_towers))
                {
                    if (com.oddlabs.tt.player.ai.MapAnalysis.dist2(t.getGridX(), t.getGridY(), px, py) <= 12 * 12)
                    {
                        danger = (float)(danger + enemyTowerValue(t));
                    }
                }
                if (danger > (float)(0.5f * strength))
                {
                    continue;
                }
                int count = com.oddlabs.tt.player.ai.Combat.countNear(intel.enemy_peons, px, py, 10);
                if (count < 3)
                {
                    continue;
                }
                float score = (float)(com.oddlabs.tt.player.ai.MapAnalysis.meters(from_x, from_y, px, py) - (float)(25.0f * count));
                if (score < best_score)
                {
                    best_score = score;
                    best = new int[] { px, py };
                }
            }
            return best;
        }
        // Java: 2550
        public void raid()
        {
            java.util.List squad = new java.util.ArrayList();
            foreach (java.util.Map.Entry e in java.lang.JIter.of(roles.entrySet()))
            {
                if (((object)((com.oddlabs.tt.player.ai.Military.Role)e.getValue()) == (object)com.oddlabs.tt.player.ai.Military.Role.RAID))
                {
                    squad.add(((com.oddlabs.tt.model.Unit)e.getKey()));
                }
            }
            if (squad.isEmpty())
            {
                return;
            }
            int[] c = com.oddlabs.tt.player.ai.Military.centroid(squad);
            float ours = 0.0f;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(squad))
            {
                ours = (float)(ours + com.oddlabs.tt.player.ai.Combat.@value(u));
            }
            float danger = enemyStrengthNear(c[0], c[1], 24);
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(ai.intel().enemy_towers))
            {
                if (com.oddlabs.tt.player.ai.MapAnalysis.dist2(t.getGridX(), t.getGridY(), c[0], c[1]) <= 10 * 10)
                {
                    danger = (float)(danger + enemyTowerValue(t));
                }
            }
            bool done = (float)(ai.time() - raid_start) > 150.0f;
            if (danger > (float)(0.8f * ours) || done || squad.size() < 2)
            {
                foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(squad))
                {
                    roles.put(u_2, com.oddlabs.tt.player.ai.Military.Role.ARMY);
                    move(u_2, staging_x, staging_y);
                }
                last_raid_end = ai.time();
                return;
            }
            int[] peons = com.oddlabs.tt.player.ai.Military.nearestGroup(ai.intel().enemy_peons, c[0], c[1], 18);
            if (peons != null)
            {
                foreach (com.oddlabs.tt.model.Unit u_3 in java.lang.JIter.of(squad))
                {
                    attackGround(u_3, peons[0], peons[1], true);
                }
                return;
            }
            int[] next = raidSpot(c[0], c[1], ours);
            if (next != null && com.oddlabs.tt.player.ai.MapAnalysis.dist2(next[0], next[1], c[0], c[1]) < 70 * 70)
            {
                raid_x = next[0];
                raid_y = next[1];
            }
            foreach (com.oddlabs.tt.model.Unit u_4 in java.lang.JIter.of(squad))
            {
                attackGround(u_4, raid_x, raid_y, false);
            }
            if (com.oddlabs.tt.player.ai.MapAnalysis.dist2(c[0], c[1], raid_x, raid_y) < 5 * 5 && peons == null)
            {
                foreach (com.oddlabs.tt.model.Unit u_5 in java.lang.JIter.of(squad))
                {
                    roles.put(u_5, com.oddlabs.tt.player.ai.Military.Role.ARMY);
                    move(u_5, staging_x, staging_y);
                }
                last_raid_end = ai.time();
            }
        }
        // Java: 2599
        public int countRole(com.oddlabs.tt.player.ai.Military.Role role)
        {
            int n = 0;
            foreach (com.oddlabs.tt.player.ai.Military.Role r in java.lang.JIter.of(roles.values()))
            {
                if (((object)r == (object)role))
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 2607
        public static int[] centroid(java.util.List units)
        {
            long sx = 0;
            long sy = 0;
            foreach (com.oddlabs.tt.model.Selectable u in java.lang.JIter.of(units))
            {
                sx += u.getGridX();
                sy += u.getGridY();
            }
            int n = java.lang.Math.max(1, units.size());
            return new int[] { unchecked((int)((sx / n))), unchecked((int)((sy / n))) };
        }
        // Java: 2622
        public void attackGround(com.oddlabs.tt.model.Unit u, int x, int y, bool urgent)
        {
            com.oddlabs.tt.player.ai.Intel.WarriorState s = ((com.oddlabs.tt.player.ai.Intel.WarriorState)ai.intel().warrior_states.get(u));
            if (((object)s == (object)com.oddlabs.tt.player.ai.Intel.WarriorState.FIGHT) || ((object)s == (object)com.oddlabs.tt.player.ai.Intel.WarriorState.STUNNED) || ((object)s == (object)com.oddlabs.tt.player.ai.Intel.WarriorState.ENTER))
            {
                return;
            }
            float? last = ((float?)last_order.get(u));
            int[] last_spot = ((int[])last_spots.get(u));
            bool same_spot = last_spot != null && com.oddlabs.tt.player.ai.MapAnalysis.dist2(last_spot[0], last_spot[1], x, y) <= 3 * 3;
            float period = (urgent ? 1.0f : (same_spot ? (float)(2 * com.oddlabs.tt.player.ai.Military.REORDER_PERIOD) : com.oddlabs.tt.player.ai.Military.REORDER_PERIOD));
            if (last != null && (float)(ai.time() - ((float)last)) < period && ((object)s != (object)com.oddlabs.tt.player.ai.Intel.WarriorState.IDLE))
            {
                return;
            }
            if (((object)s == (object)com.oddlabs.tt.player.ai.Intel.WarriorState.IDLE) && same_spot && com.oddlabs.tt.player.ai.MapAnalysis.dist2(u.getGridX(), u.getGridY(), x, y) <= 4 * 4)
            {
                return;
            }
            queueOrder(u, x, y, true);
        }
        // Java: 2642
        public void engageSpread(java.util.List units, java.util.List enemies, int fallback_x, int fallback_y, bool urgent)
        {
            java.util.Set targeted = (ai.strategy().micro_targets ? assignTargets(units, enemies) : java.util.Set_.of());
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(units))
            {
                if (targeted.contains(u))
                {
                    continue;
                }
                if (!ai.strategy().engage_spread)
                {
                    attackGround(u, fallback_x, fallback_y, urgent);
                    continue;
                }
                com.oddlabs.tt.model.Selectable nearest_v = null;
                int best = java.lang.Integer.MAX_VALUE;
                foreach (com.oddlabs.tt.model.Selectable e in java.lang.JIter.of(enemies))
                {
                    if (e.isDead())
                    {
                        continue;
                    }
                    int d = com.oddlabs.tt.player.ai.MapAnalysis.dist2(u.getGridX(), u.getGridY(), e.getGridX(), e.getGridY());
                    if (d < best)
                    {
                        best = d;
                        nearest_v = e;
                    }
                }
                if (nearest_v != null)
                {
                    attackGround(u, nearest_v.getGridX(), nearest_v.getGridY(), urgent);
                }
                else
                {
                    attackGround(u, fallback_x, fallback_y, urgent);
                }
            }
        }
        public const int THROW_CELLS = 9;
        // Java: 2678
        public java.util.Set assignTargets(java.util.List units, java.util.List enemies)
        {
            java.util.Set targeted = new java.util.LinkedHashSet();
            hunt_targets.entrySet().removeIf(new java.util.function.Predicate_L((object e_l) =>
            {
                java.util.Map.Entry e = (java.util.Map.Entry)e_l;
                return ((com.oddlabs.tt.model.Unit)e.getKey()).isDead() || ((com.oddlabs.tt.model.Unit)e.getValue()).isDead();
            }));
            java.util.List foes = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Selectable e_2 in java.lang.JIter.of(enemies))
            {
                if ((e_2 is com.oddlabs.tt.model.Unit u) && !u.isDead())
                {
                    foes.add(u);
                }
            }
            if (foes.isEmpty())
            {
                return targeted;
            }
            java.util.Map survive = new java.util.LinkedHashMap();
            foreach (com.oddlabs.tt.model.Unit w in java.lang.JIter.of(units))
            {
                com.oddlabs.tt.model.Unit t = ((com.oddlabs.tt.model.Unit)hunt_targets.get(w));
                if (t != null && (w.getCurrentController() is com.oddlabs.tt.model.behaviour.HuntController))
                {
                    survive.merge(t, (float)(1.0f - hitChance(w, t)), new java.util.function.BiFunction_L((object a_l, object b_l) =>
                    {
                        float? a = (float?)a_l;
                        float? b = (float?)b_l;
                        return (float)(((float)a) * ((float)b));
                    }));
                }
            }
            int r2 = com.oddlabs.tt.player.ai.Military.THROW_CELLS * com.oddlabs.tt.player.ai.Military.THROW_CELLS;
            foreach (com.oddlabs.tt.model.Unit w_2 in java.lang.JIter.of(units))
            {
                com.oddlabs.tt.player.ai.Intel.WarriorState s = ((com.oddlabs.tt.player.ai.Intel.WarriorState)ai.intel().warrior_states.get(w_2));
                if (((object)s == (object)com.oddlabs.tt.player.ai.Intel.WarriorState.STUNNED) || ((object)s == (object)com.oddlabs.tt.player.ai.Intel.WarriorState.ENTER) || w_2.isDead())
                {
                    continue;
                }
                com.oddlabs.tt.model.Unit current = ((com.oddlabs.tt.model.Unit)hunt_targets.get(w_2));
                bool hunting = current != null && (w_2.getCurrentController() is com.oddlabs.tt.model.behaviour.HuntController);
                if (hunting && com.oddlabs.tt.player.ai.MapAnalysis.dist2(w_2.getGridX(), w_2.getGridY(), current.getGridX(), current.getGridY()) <= (com.oddlabs.tt.player.ai.Military.THROW_CELLS + 3) * (com.oddlabs.tt.player.ai.Military.THROW_CELLS + 3))
                {
                    targeted.add(w_2);
                    continue;
                }
                com.oddlabs.tt.model.Unit best = null;
                float best_score = 0.0f;
                float best_p = 0.0f;
                foreach (com.oddlabs.tt.model.Unit e_3 in java.lang.JIter.of(foes))
                {
                    if (com.oddlabs.tt.player.ai.MapAnalysis.dist2(w_2.getGridX(), w_2.getGridY(), e_3.getGridX(), e_3.getGridY()) > r2)
                    {
                        continue;
                    }
                    float p = hitChance(w_2, e_3);
                    float score = (float)((float)(throwValue(w_2, e_3) * p) * survival(survive, e_3));
                    if (score > best_score)
                    {
                        best_score = score;
                        best = e_3;
                        best_p = p;
                    }
                }
                if (best == null)
                {
                    continue;
                }
                if (!isMultiHit(best))
                {
                    survive.merge(best, (float)(1.0f - best_p), new java.util.function.BiFunction_L((object a_2_l, object b_2_l) =>
                    {
                        float? a_2 = (float?)a_2_l;
                        float? b_2 = (float?)b_2_l;
                        return (float)(((float)a_2) * ((float)b_2));
                    }));
                }
                hunt_targets.put(w_2, best);
                last_order.put(w_2, ai.time());
                ai.intel().warrior_states.put(w_2, com.oddlabs.tt.player.ai.Intel.WarriorState.FIGHT);
                ai.owner().setTarget(com.oddlabs.tt.model.Selectable.newArray(w_2), best, com.oddlabs.tt.model.Action.ATTACK, true);
                targeted.add(w_2);
            }
            return targeted;
        }
        // Java: 2734
        public bool isMultiHit(com.oddlabs.tt.model.Unit e)
        {
            return ai.strategy().chief_per_hit && e.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC);
        }
        // Java: 2739
        public float survival(java.util.Map survive, com.oddlabs.tt.model.Unit e)
        {
            return (isMultiHit(e) ? 1.0f : ((float)((float?)survive.getOrDefault(e, 1.0f))));
        }
        // Java: 2748
        public float throwValue(com.oddlabs.tt.model.Unit thrower, com.oddlabs.tt.model.Unit e)
        {
            if (!isMultiHit(e))
            {
                return com.oddlabs.tt.player.ai.Combat.lastingValue(e);
            }
            int damage = (thrower.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.THROW) && ((object)com.oddlabs.tt.player.ai.Intel.warriorType(thrower) != (object)com.oddlabs.tt.player.ai.Intel.WarriorType.ROCK) ? 2 : 1);
            int hits = java.lang.Math.max(1, (e.getHitPoints() + damage - 1) / damage);
            return java.lang.Math.min(6.0f, (float)(25.0f / hits));
        }
        public const int TOWER_CELLS = 15;
        // Java: 2765
        public void towerFire()
        {
            if (!ai.strategy().tower_fire)
            {
                return;
            }
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            tower_targets.entrySet().removeIf(new java.util.function.Predicate_L((object e_l) =>
            {
                java.util.Map.Entry e = (java.util.Map.Entry)e_l;
                return ((com.oddlabs.tt.model.Building)e.getKey()).isDead() || ((com.oddlabs.tt.model.Unit)e.getValue()).isDead();
            }));
            java.util.Map survive = new java.util.LinkedHashMap();
            int r2 = com.oddlabs.tt.player.ai.Military.TOWER_CELLS * com.oddlabs.tt.player.ai.Military.TOWER_CELLS;
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(intel.towers))
            {
                if (!t.isComplete() || t.getUnitCount() == 0)
                {
                    continue;
                }
                com.oddlabs.tt.model.Unit gunner = (((com.oddlabs.tt.model.MountUnitContainer)t.getUnitContainer())).getUnit();
                if (gunner == null || gunner.isDead() || com.oddlabs.tt.player.ai.Intel.isStunned(gunner))
                {
                    continue;
                }
                com.oddlabs.tt.model.Unit current = ((com.oddlabs.tt.model.Unit)tower_targets.get(t));
                if (current != null && com.oddlabs.tt.player.ai.MapAnalysis.dist2(t.getGridX(), t.getGridY(), current.getGridX(), current.getGridY()) <= r2)
                {
                    survive.merge(current, (float)(1.0f - towerHitChance(gunner, t, current)), new java.util.function.BiFunction_L((object a_l, object b_l) =>
                    {
                        float? a = (float?)a_l;
                        float? b = (float?)b_l;
                        return (float)(((float)a) * ((float)b));
                    }));
                    continue;
                }
                com.oddlabs.tt.model.Unit best = null;
                float best_score = 0.0f;
                float best_p = 0.0f;
                foreach (java.util.List group in java.lang.JIter.of(java.util.List_.of(intel.enemy_warriors, intel.enemy_chieftains, intel.enemy_peons)))
                {
                    foreach (com.oddlabs.tt.model.Unit e_2 in java.lang.JIter.of(group))
                    {
                        if (e_2.isDead() || com.oddlabs.tt.player.ai.MapAnalysis.dist2(t.getGridX(), t.getGridY(), e_2.getGridX(), e_2.getGridY()) > r2)
                        {
                            continue;
                        }
                        float @value = throwValue(gunner, e_2);
                        if (((object)group == (object)intel.enemy_peons) && nearOwnTower(e_2))
                        {
                            @value = 1.5f;
                        }
                        float p = towerHitChance(gunner, t, e_2);
                        float score = (float)((float)(@value * p) * survival(survive, e_2));
                        if (score > best_score)
                        {
                            best_score = score;
                            best = e_2;
                            best_p = p;
                        }
                    }
                }
                if (best == null)
                {
                    tower_targets.remove(t);
                    continue;
                }
                if (!isMultiHit(best))
                {
                    survive.merge(best, (float)(1.0f - best_p), new java.util.function.BiFunction_L((object a_2_l, object b_2_l) =>
                    {
                        float? a_2 = (float?)a_2_l;
                        float? b_2 = (float?)b_2_l;
                        return (float)(((float)a_2) * ((float)b_2));
                    }));
                }
                tower_targets.put(t, best);
                ai.owner().setTarget(com.oddlabs.tt.model.Selectable.newArray(t), best, com.oddlabs.tt.model.Action.ATTACK, false);
            }
        }
        // Java: 2814
        public bool nearOwnTower(com.oddlabs.tt.model.Unit e)
        {
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(ai.intel().towers))
            {
                if (com.oddlabs.tt.player.ai.MapAnalysis.dist2(t.getGridX(), t.getGridY(), e.getGridX(), e.getGridY()) <= 3 * 3)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 2822
        public float towerHitChance(com.oddlabs.tt.model.Unit gunner, com.oddlabs.tt.model.Building tower, com.oddlabs.tt.model.Unit dst)
        {
            com.oddlabs.tt.player.ai.MapAnalysis map = ai.map();
            float dz = (float)(map.height(tower.getGridX(), tower.getGridY()) - map.height(dst.getGridX(), dst.getGridY()));
            float terrain = java.lang.Math.clamp((float)(dz / 80.0f), (float)(-0.25f), 0.25f);
            float p = (float)((float)(3.0f * ((float)((float)(gunner.getOwner().getHitBonus() + terrain) + com.oddlabs.tt.player.ai.Military.baseHitChance(gunner)))) * ((float)(1.0f - dst.getDefenseChance())));
            return java.lang.Math.clamp(p, 0.0f, 1.0f);
        }
        // Java: 2831
        public static float baseHitChance(com.oddlabs.tt.model.Unit src)
        {
            if (!src.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.THROW))
            {
                return 0.2f;
            }
            return ((com.oddlabs.tt.player.ai.Intel.warriorType(src)).ordinal() switch { 0 /*ROCK*/ => (float)(0.5f), 1 /*IRON*/ => (float)(0.75f), 2 /*CHICKEN*/ => (float)(0.95f), _ => throw new System.InvalidOperationException("MatchException") });
        }
        // Java: 2842
        public float hitChance(com.oddlabs.tt.model.Unit src, com.oddlabs.tt.model.Unit dst)
        {
            float @base = default;
            if (!src.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.THROW))
            {
                @base = 0.2f;
            }
            else
            {
                @base = ((com.oddlabs.tt.player.ai.Intel.warriorType(src)).ordinal() switch { 0 /*ROCK*/ => (float)(0.5f), 1 /*IRON*/ => (float)(0.75f), 2 /*CHICKEN*/ => (float)(0.95f), _ => throw new System.InvalidOperationException("MatchException") });
            }
            com.oddlabs.tt.player.ai.MapAnalysis map = ai.map();
            float dz = (float)(map.height(src.getGridX(), src.getGridY()) - map.height(dst.getGridX(), dst.getGridY()));
            float terrain = java.lang.Math.clamp((float)(dz / 80.0f), (float)(-0.25f), 0.25f);
            float p = (float)(((float)((float)(src.getOwner().getHitBonus() + terrain) + @base)) * ((float)(1.0f - dst.getDefenseChance())));
            return java.lang.Math.clamp(p, 0.0f, 1.0f);
        }
        // Java: 2863
        public void restoreDodge()
        {
            if (!ai.strategy().restore_dodge)
            {
                return;
            }
            dodge_orders.keySet().removeIf(new java.util.function.Predicate_L((object a0) => { return ((com.oddlabs.tt.model.Unit)a0).isDead(); }));
            foreach (com.oddlabs.tt.model.Unit w in java.lang.JIter.of(ai.intel().warriors))
            {
                if (!com.oddlabs.tt.player.ai.Intel.isDefenseless(w))
                {
                    continue;
                }
                float? last = ((float?)dodge_orders.get(w));
                if (last != null && (float)(ai.time() - ((float)last)) < 30.0f)
                {
                    continue;
                }
                dodge_orders.put(w, ai.time());
                queueOrder(w, w.getGridX(), w.getGridY(), true);
            }
        }
        // Java: 2878
        public void move(com.oddlabs.tt.model.Unit u, int x, int y)
        {
            queueOrder(u, x, y, false);
        }
        // Java: 2886
        public void queueOrder(com.oddlabs.tt.model.Unit u, int x, int y, bool aggressive)
        {
            last_order.put(u, ai.time());
            last_spots.put(u, new int[] { x, y });
            long key = (((long)(x)) << 32) | (((long)(y)) << 1) | ((aggressive ? 1 : 0));
            ((java.util.List)pending_orders.computeIfAbsent(key, new java.util.function.Function_L((object k_l) =>
            {
                long? k = (long?)k_l;
                return new java.util.ArrayList();
            }))).add(u);
        }
        // Java: 2893
        public void flushOrders()
        {
            foreach (java.util.Map.Entry e in java.lang.JIter.of(pending_orders.entrySet()))
            {
                long key = ((long)((long?)e.getKey()));
                int x = unchecked((int)((key >> 32)));
                int y = unchecked((int)(((key & 4294967295L) >> 1)));
                bool aggressive = (key & 1) != 0;
                java.util.List units = ((java.util.List)e.getValue());
                ai.owner().setLandscapeTarget(((com.oddlabs.tt.model.Selectable[])units.toArray(new com.oddlabs.tt.model.Selectable[0])), x, y, (aggressive ? com.oddlabs.tt.model.Action.ATTACK : com.oddlabs.tt.model.Action.MOVE), aggressive);
            }
            pending_orders.clear();
        }
        // Java: 2906
        public int stagingX()
        {
            return staging_x;
        }
        // Java: 2910
        public int stagingY()
        {
            return staging_y;
        }
        // Java: 2914
        public com.oddlabs.tt.player.ai.Military.Mode mode()
        {
            return mode_f;
        }
        // Java: 2920
        public float enemyTowerValue(com.oddlabs.tt.model.Building tower)
        {
            return (float)(com.oddlabs.tt.player.ai.Combat.towerValue(tower) * ai.strategy().tower_weight);
        }
        // Java: 2924
        public float threatStrength()
        {
            return threat_strength;
        }
        // Java: 2928
        public int threatX()
        {
            return threat_x;
        }
        // Java: 2932
        public int threatY()
        {
            return threat_y;
        }
        // Java: 2936
        public bool anyFighting(java.util.List units)
        {
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(units))
            {
                if (((object)((com.oddlabs.tt.player.ai.Intel.WarriorState)ai.intel().warrior_states.get(u)) == (object)com.oddlabs.tt.player.ai.Intel.WarriorState.FIGHT))
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 2944
        public int[] attackCenter()
        {
            java.util.List army = new java.util.ArrayList();
            foreach (java.util.Map.Entry e in java.lang.JIter.of(roles.entrySet()))
            {
                if (((object)((com.oddlabs.tt.player.ai.Military.Role)e.getValue()) == (object)com.oddlabs.tt.player.ai.Military.Role.ATTACK))
                {
                    army.add(((com.oddlabs.tt.model.Unit)e.getKey()));
                }
            }
            return (army.isEmpty() ? (int[])(null) : com.oddlabs.tt.player.ai.Military.centroid(army));
        }
        // Java: 2953
        public int[] battleFront()
        {
            if (threat_level > 0)
            {
                return new int[] { threat_x, threat_y };
            }
            java.util.List army = new java.util.ArrayList();
            foreach (java.util.Map.Entry e in java.lang.JIter.of(roles.entrySet()))
            {
                if (((object)((com.oddlabs.tt.player.ai.Military.Role)e.getValue()) == (object)com.oddlabs.tt.player.ai.Military.Role.ATTACK))
                {
                    army.add(((com.oddlabs.tt.model.Unit)e.getKey()));
                }
            }
            if (!army.isEmpty())
            {
                return com.oddlabs.tt.player.ai.Military.centroid(army);
            }
            return null;
        }
        // Java: 2965
        public string debugStatus()
        {
            return "mode=" + java.lang.JStr.valueOf(mode_f) + " army=" + (countRole(com.oddlabs.tt.player.ai.Military.Role.ARMY)) + " atk=" + (countRole(com.oddlabs.tt.player.ai.Military.Role.ATTACK)) + " raid=" + (countRole(com.oddlabs.tt.player.ai.Military.Role.RAID)) + " twr=" + (countRole(com.oddlabs.tt.player.ai.Military.Role.TOWER)) + java.lang.JStr.valueOf(java.lang.JString.format(" str=%.1f thr=%d/%.1f", armyStrength(), threat_level, threat_strength));
        }
        private void init_Military()
        {
            this.roles = new java.util.LinkedHashMap();
            this.tower_assignments = new java.util.LinkedHashMap();
            this.last_order = new java.util.LinkedHashMap();
            this.chief_orders = new java.util.LinkedHashMap();
            this.last_spots = new java.util.LinkedHashMap();
            this.pending_orders = new java.util.LinkedHashMap();
            this.enemy_history = new java.util.ArrayDeque();
            this.mode_f = com.oddlabs.tt.player.ai.Military.Mode.HOME;
            this.staging_time = (float)(-100.0f);
            this.threats = new java.util.ArrayList();
            this.raiders = new java.util.ArrayList();
            this.last_threat_time = (float)(-100.0f);
            this.hold_until = (float)(-1.0f);
            this.best_target_dist = java.lang.Integer.MAX_VALUE;
            this.hold_spot = new int[2];
            this.last_enemy_d2 = java.lang.Integer.MAX_VALUE;
            this.last_charge_log = (float)(-100.0f);
            this.hunt_targets = new java.util.LinkedHashMap();
            this.dodge_orders = new java.util.LinkedHashMap();
            this.tower_targets = new java.util.LinkedHashMap();
            this.sapper_orders = new java.util.LinkedHashMap();
            this.last_pillage_log = (float)(-100.0f);
            this.enemy_casts = new java.util.LinkedHashMap();
            this.dodges = new java.util.LinkedHashMap();
            this.caster_hunters = new java.util.LinkedHashMap();
            this.siege_start = (float)(-1.0f);
            this.siege_cooldown = (float)(-1.0f);
            this.blast_play_until = (float)(-1.0f);
            this.last_blast_play = (float)(-100.0f);
            this.siege_progress = (float)(-1.0f);
            this.last_pull_log = (float)(-100.0f);
            this.siege_assign = new java.util.LinkedHashMap();
            this.caster_targets = new java.util.LinkedHashMap();
            this.windups = new java.util.LinkedHashMap();
            this.fogs = new java.util.ArrayList();
            this.creep_towers = new java.util.ArrayList();
            this.last_creep_try = (float)(-100.0f);
            this.attack_caution = 1.0f;
            this.militia_orders = new java.util.LinkedHashMap();
            this.last_militia_log = (float)(-100.0f);
            this.last_peon_rush = (float)(-100.0f);
            this.escort_time = (float)(-100.0f);
            this.raid_start = (float)(-1000.0f);
            this.last_raid_end = (float)(-1000.0f);
        }
        public Military(com.oddlabs.tt.player.ai.ExpertAI ai) : base()
        {
            init_Military();
            this.ai = ai;
            staging_x = ai.planner().getStartX();
            staging_y = ai.planner().getStartY();
        }
    }
}
