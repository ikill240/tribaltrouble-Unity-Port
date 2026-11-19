// Translated from Military.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.gauntlet
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
            public static readonly Role CHASE = new Role("CHASE", 5);
            public static readonly Role[] VALUES_ = new Role[] { ARMY, TOWER, ATTACK, REINFORCE, RAID, CHASE };
            public Role(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        public const int ENGAGE_RADIUS = 22;
        public const float REORDER_PERIOD = 2.5f;
        public com.oddlabs.tt.player.gauntlet.GauntletAI ai;
        public java.util.Map roles;
        public java.util.Map tower_assignments;
        public java.util.Map last_order;
        public java.util.Map chief_orders;
        public java.util.Map last_spots;
        public java.util.Map pending_orders;
        public java.util.ArrayDeque enemy_history;
        public java.util.Map lent;
        public com.oddlabs.tt.player.gauntlet.Military.Mode mode;
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
        public int last_logged_threat;
        public bool last_logged_engage;
        public int engage_state;
        public com.oddlabs.tt.model.Selectable target;
        public java.util.Map stalled_targets;
        public const float STALL_MEMORY = 600.0f;
        public java.util.List dead_regions;
        public java.util.List dead_region_times;
        // Java: 98
        public bool inDeadRegion(int x, int y)
        {
            foreach (com.oddlabs.tt.player.gauntlet.DistanceField f in java.lang.JIter.of(dead_regions))
            {
                if (f.getAround(x, y, 2) != com.oddlabs.tt.player.gauntlet.DistanceField.UNREACHABLE)
                {
                    return true;
                }
            }
            return false;
        }
        public com.oddlabs.tt.player.Player focus_owner;
        public float sweep_until;
        public float sweep_ring_start;
        public float last_out_time;
        public int last_alive;
        public java.util.List explain;
        public java.util.List frozen_deferred;
        public int target_x;
        public int target_y;
        public com.oddlabs.tt.player.gauntlet.DistanceField target_field;
        public float attack_initial_strength;
        public float worn_peak;
        public java.util.ArrayDeque worn_history;
        public bool worn_skip_counted;
        public bool split_guard_counted;
        public float muster_start;
        public float next_wave_time;
        public float last_trace;
        public float hold_until;
        public float last_progress_time;
        public int best_target_dist;
        public int[] hold_spot;
        public int last_enemy_d2;
        public java.util.ArrayDeque hold_groups;
        public float last_hold_skip;
        public float last_charge_log;
        public java.util.Map hunt_targets;
        public java.util.Map dodge_orders;
        public java.util.Map tower_targets;
        public java.util.Map sapper_orders;
        public float last_pillage_log;
        public java.util.Map enemy_casts;
        public java.util.Map enemy_chief_seen;
        public int own_stunned_before;
        public const float SPELL_RECHARGE = 40.0f;
        public const float STUN_WINDUP = 3.7f;
        public const float STUN_RADIUS = 36.0f;
        public const float STUN_OFFSET = 2.57f;
        // Java: 164
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
        // Java: 189
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
        // Java: 242
        public com.oddlabs.tt.player.gauntlet.EnemyIndex enemyIndex()
        {
            return ai.intel().enemyIndex(ai.ticks());
        }
        // Java: 255
        public int baseThreatLevel()
        {
            return threat_level;
        }
        // Java: 265
        public java.util.List lend(int x, int y, int n, int min, int r, float seconds)
        {
            java.util.List @out = new java.util.ArrayList();
            if (((object)mode != (object)com.oddlabs.tt.player.gauntlet.Military.Mode.HOME) || wantsEverything())
            {
                return @out;
            }
            java.util.List candidates = new java.util.ArrayList();
            java.util.List dists = new java.util.ArrayList();
            foreach (java.util.Map.Entry e in java.lang.JIter.of(roles.entrySet()))
            {
                com.oddlabs.tt.model.Unit u = ((com.oddlabs.tt.model.Unit)e.getKey());
                if (!lendable(u, ((com.oddlabs.tt.player.gauntlet.Military.Role)e.getValue())))
                {
                    continue;
                }
                int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, u.getGridX(), u.getGridY());
                if (d > r * r)
                {
                    continue;
                }
                int i = 0;
                while (i < dists.size() && ((int)((int?)dists.get(i))) <= d)
                {
                    i++;
                }
                candidates.add(i, u);
                dists.add(i, d);
            }
            if (candidates.size() < min)
            {
                return @out;
            }
            for (int i_2 = 0; i_2 < java.lang.Math.min(n, candidates.size()); i_2++)
            {
                com.oddlabs.tt.model.Unit u_2 = ((com.oddlabs.tt.model.Unit)candidates.get(i_2));
                roles.remove(u_2);
                tower_assignments.remove(u_2);
                front_entry.remove(u_2);
                last_order.remove(u_2);
                lent.put(u_2, (float)(ai.time() + seconds));
                @out.add(u_2);
            }
            return @out;
        }
        // Java: 300
        public int lendable(int x, int y, int r)
        {
            if (((object)mode != (object)com.oddlabs.tt.player.gauntlet.Military.Mode.HOME) || wantsEverything())
            {
                return 0;
            }
            int n = 0;
            foreach (java.util.Map.Entry e in java.lang.JIter.of(roles.entrySet()))
            {
                com.oddlabs.tt.model.Unit u = ((com.oddlabs.tt.model.Unit)e.getKey());
                if (lendable(u, ((com.oddlabs.tt.player.gauntlet.Military.Role)e.getValue())) && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, u.getGridX(), u.getGridY()) <= r * r)
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 313
        public bool lendable(com.oddlabs.tt.model.Unit u, com.oddlabs.tt.player.gauntlet.Military.Role role)
        {
            return ((object)role == (object)com.oddlabs.tt.player.gauntlet.Military.Role.ARMY) && !u.isDead() && !u.isMounted() && ((object)((com.oddlabs.tt.player.gauntlet.Intel.WarriorState)ai.intel().warrior_states.get(u)) == (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorState.IDLE) && ((object)com.oddlabs.tt.player.gauntlet.Intel.warriorType(u) != (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorType.ROCK);
        }
        // Java: 319
        public void release(java.util.List units)
        {
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(units))
            {
                lent.remove(u);
            }
        }
        // Java: 325
        public string roleOf(com.oddlabs.tt.model.Unit u)
        {
            com.oddlabs.tt.player.gauntlet.Military.Role role = ((com.oddlabs.tt.player.gauntlet.Military.Role)roles.get(u));
            return (role == null ? (string)(null) : role.name().toLowerCase(java.util.Locale.ROOT));
        }
        // Java: 331
        public bool threatNear(int x, int y, int radius)
        {
            int r2 = radius * radius;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(threats))
            {
                if (!u.isDead() && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, u.getGridX(), u.getGridY()) <= r2)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 344
        public bool threatNearEcon(int x, int y, int radius)
        {
            int scan = ai.strategy().parked_scan_econ;
            if (scan <= 0)
            {
                return threatNear(x, y, radius);
            }
            int r2 = radius * radius;
            int box = java.lang.Math.min(radius, scan);
            bool exempt = false;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(threats))
            {
                if (u.isDead() || com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, u.getGridX(), u.getGridY()) > r2)
                {
                    continue;
                }
                if (com.oddlabs.tt.player.gauntlet.Intel.isParked(u) && java.lang.Math.max(java.lang.Math.abs(u.getGridX() - x), java.lang.Math.abs(u.getGridY() - y)) > box)
                {
                    exempt = true;
                    continue;
                }
                return true;
            }
            if (exempt)
            {
                ai.aiLog().count("parked_exempt_econ");
            }
            return false;
        }
        // Java: 365
        public float enemyStrengthNear(int x, int y, int radius)
        {
            return (float)(com.oddlabs.tt.player.gauntlet.Combat.strengthNear(ai.intel().enemy_warriors, x, y, radius) + com.oddlabs.tt.player.gauntlet.Combat.strengthNear(ai.intel().enemy_chieftains, x, y, radius));
        }
        // Java: 371
        public float enemyFightersNear(int x, int y, int radius)
        {
            return (float)(enemyStrengthNear(x, y, radius) + com.oddlabs.tt.player.gauntlet.Combat.strengthNear(ai.intel().enemy_peons, x, y, radius));
        }
        // Java: 376
        public bool wantsEverything()
        {
            return threat_level >= 2 || ((object)mode == (object)com.oddlabs.tt.player.gauntlet.Military.Mode.MUSTER);
        }
        // Java: 380
        public int towerSeatsFree()
        {
            int n = 0;
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(ai.intel().towers))
            {
                if (!com.oddlabs.tt.player.gauntlet.Intel.isTowerManned(t) && !tower_assignments.containsValue(t) && !ai.economy().isDoomed(t))
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 388
        public float armyStrength()
        {
            float s = 0.0f;
            foreach (java.util.Map.Entry e in java.lang.JIter.of(roles.entrySet()))
            {
                if (((object)((com.oddlabs.tt.player.gauntlet.Military.Role)e.getValue()) == (object)com.oddlabs.tt.player.gauntlet.Military.Role.ARMY) || ((object)((com.oddlabs.tt.player.gauntlet.Military.Role)e.getValue()) == (object)com.oddlabs.tt.player.gauntlet.Military.Role.RAID))
                {
                    s = (float)(s + com.oddlabs.tt.player.gauntlet.Combat.@value(((com.oddlabs.tt.model.Unit)e.getKey())));
                }
            }
            return s;
        }
        // Java: 397
        public float armyStrengthWanted()
        {
            float enemy = enemyFieldStrength();
            float wanted = java.lang.Math.max((float)(5.0f + (float)(ai.time() / 90.0f)), (float)(0.75f * enemy));
            return java.lang.Math.min(wanted, 45.0f);
        }
        // Java: 404
        public float enemyFieldStrengthNear(int x, int y, int radius)
        {
            int r2 = radius * radius;
            float s = 0.0f;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(ai.intel().enemy_warriors))
            {
                s = (float)(s + (float)(com.oddlabs.tt.player.gauntlet.Combat.@value(u) * ((com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, u.getGridX(), u.getGridY()) <= r2 ? 1.0f : 0.3f))));
            }
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(ai.intel().enemy_chieftains))
            {
                s = (float)(s + (float)(com.oddlabs.tt.player.gauntlet.Combat.@value(u_2) * ((com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, u_2.getGridX(), u_2.getGridY()) <= r2 ? 1.0f : 0.3f))));
            }
            return s;
        }
        // Java: 414
        public float enemyFieldStrength()
        {
            float s = 0.0f;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(ai.intel().enemy_warriors))
            {
                s = (float)(s + com.oddlabs.tt.player.gauntlet.Combat.@value(u));
            }
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(ai.intel().enemy_chieftains))
            {
                s = (float)(s + com.oddlabs.tt.player.gauntlet.Combat.@value(u_2));
            }
            return s;
        }
        public float last_provoke_probe;
        // Java: 434
        public void provokeProbe()
        {
            if ((float)(ai.time() - last_provoke_probe) < 5.0f || ai.time() > 900.0f)
            {
                return;
            }
            last_provoke_probe = ai.time();
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            java.util.List homes = new java.util.ArrayList(intel.enemy_quarters);
            homes.addAll(intel.enemy_armories);
            if (homes.isEmpty())
            {
                return;
            }
            java.util.List ours = new java.util.ArrayList(intel.peons);
            ours.addAll(intel.warriors);
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(ours))
            {
                if (u.isDead() || u.isMounted())
                {
                    continue;
                }
                bool near = false;
                foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(homes))
                {
                    if (!b.isDead() && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(u.getGridX(), u.getGridY(), b.getGridX(), b.getGridY()) <= 15 * 15)
                    {
                        near = true;
                        break;
                    }
                }
                if (!near)
                {
                    continue;
                }
                com.oddlabs.tt.player.gauntlet.Intel.PeonState state = ((com.oddlabs.tt.player.gauntlet.Intel.PeonState)intel.peon_states.get(u));
                com.oddlabs.tt.player.gauntlet.Military.Role role = ((com.oddlabs.tt.player.gauntlet.Military.Role)roles.get(u));
                string what = (state != null ? state.name() : (role != null ? role.name() : "OTHER"));
                ai.aiLog().count("prov_" + java.lang.JStr.valueOf(what));
                ai.aiLog().count((ai.time() < 480.0f ? "prov_early" : "prov_mid"));
            }
        }
        // Java: 465
        public void tick()
        {
            watchEnemyCasts();
            updateRoles();
            provokeProbe();
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
            if (((object)mode != (object)com.oddlabs.tt.player.gauntlet.Military.Mode.ATTACK) && siege_start >= 0.0f)
            {
                endSiege();
            }
            switch ((mode).ordinal())
            {
                case 0 /*HOME*/:
                {
                    if (sweep_until >= 0.0f)
                    {
                        sweep();
                    }
                    else
                    {
                        holdStaging();
                    }
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
            chase();
            peonRush();
            restoreDodge();
            towerFire();
            sapperTick();
            flushOrders();
        }
        // Java: 499
        public void plan()
        {
            updateRoles();
            recordEnemyStrength();
            int alive = ai.enemiesAlive();
            if (alive != last_alive)
            {
                if (last_alive >= 0 && alive < last_alive)
                {
                    last_out_time = ai.time();
                }
                last_alive = alive;
            }
            if (ai.strategy().ring_sweep)
            {
                considerSweep();
            }
            easeCaution();
            if (((object)mode == (object)com.oddlabs.tt.player.gauntlet.Military.Mode.HOME) && threat_level < 2 && ai.strategy().strikes)
            {
                considerStrike();
            }
            bool small_threat = threat_level >= 2 && base_threat_strength < (float)(ai.strategy().attack_threat_ratio * ((ai.strategy().launch_recheck ? stagingStrength(40) : armyStrength())));
            if (((object)mode == (object)com.oddlabs.tt.player.gauntlet.Military.Mode.HOME) && (threat_level < 2 || small_threat) && sweep_until < 0.0f)
            {
                considerAttack();
            }
            bool reinforce_ok = threat_level < 2 || base_threat_strength < (float)(ai.strategy().reinforce_threat_ratio * ((float)(armyStrength() + attackStrength())));
            if (((object)mode == (object)com.oddlabs.tt.player.gauntlet.Military.Mode.ATTACK) && reinforce_ok && ai.strategy().reinforce)
            {
                considerReinforcing();
            }
            if (((object)mode == (object)com.oddlabs.tt.player.gauntlet.Military.Mode.HOME) && threat_level <= ai.strategy().raid_threat)
            {
                considerRaid();
            }
            if (ai.strategy().chief_hunt && countRole(com.oddlabs.tt.player.gauntlet.Military.Role.CHASE) == 0 && (float)(ai.time() - last_chase_end) >= 10.0f)
            {
                considerChase();
            }
        }
        // Java: 531
        public void updateRoles()
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            for (java.util.Iterator it = roles.entrySet().iterator(); it.hasNext(); )
            {
                java.util.Map.Entry e = ((java.util.Map.Entry)it.next());
                com.oddlabs.tt.model.Unit u = ((com.oddlabs.tt.model.Unit)e.getKey());
                if (u.isDead() || u.isMounted())
                {
                    if (u.isDead() && ((object)((com.oddlabs.tt.player.gauntlet.Military.Role)e.getValue()) == (object)com.oddlabs.tt.player.gauntlet.Military.Role.CHASE))
                    {
                        ai.aiLog().count("hunt_lost");
                    }
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
                bool doomed = ai.economy().isDoomed(t);
                if (u_2.isDead() || u_2.isMounted() || t.isDead() || doomed || com.oddlabs.tt.player.gauntlet.Intel.isTowerManned(t) || (((object)((com.oddlabs.tt.player.gauntlet.Intel.WarriorState)intel.warrior_states.get(u_2)) != (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorState.ENTER) && !front_entry.containsKey(u_2)))
                {
                    if (!u_2.isDead() && !u_2.isMounted() && ((object)((com.oddlabs.tt.player.gauntlet.Military.Role)roles.get(u_2)) == (object)com.oddlabs.tt.player.gauntlet.Military.Role.TOWER))
                    {
                        roles.put(u_2, com.oddlabs.tt.player.gauntlet.Military.Role.ARMY);
                    }
                    if (doomed && !u_2.isDead() && !u_2.isMounted())
                    {
                        ai.landscapeOrder(com.oddlabs.tt.model.Selectable.newArray(u_2), u_2.getGridX(), u_2.getGridY(), com.oddlabs.tt.model.Action.MOVE, false);
                    }
                    it_2.remove();
                }
            }
            if (!lent.isEmpty())
            {
                lent.entrySet().removeIf(new java.util.function.Predicate_L((object e_3_l) =>
                {
                    java.util.Map.Entry e_3 = (java.util.Map.Entry)e_3_l;
                    return ((com.oddlabs.tt.model.Unit)e_3.getKey()).isDead() || ai.time() > ((float)((float?)e_3.getValue()));
                }));
            }
            foreach (com.oddlabs.tt.model.Unit w in java.lang.JIter.of(intel.warriors))
            {
                if (!roles.containsKey(w) && !lent.containsKey(w))
                {
                    roles.put(w, com.oddlabs.tt.player.gauntlet.Military.Role.ARMY);
                }
            }
        }
        // Java: 569
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
        // Java: 590
        public void updateThreat()
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
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
            int parked_scan = ai.strategy().parked_scan_threat;
            int np = 0;
            int[] pxs = new int[intel.peons.size()];
            int[] pys = new int[intel.peons.size()];
            foreach (com.oddlabs.tt.model.Unit p in java.lang.JIter.of(intel.peons))
            {
                com.oddlabs.tt.player.gauntlet.Intel.PeonState s = ((com.oddlabs.tt.player.gauntlet.Intel.PeonState)intel.peon_states.get(p));
                if (((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_CHICKEN) || ((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.SHEPHERD))
                {
                    continue;
                }
                pxs[np] = p.getGridX();
                pys[np++] = p.getGridY();
            }
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(enemies))
            {
                if (ai.decoys().caged(e))
                {
                    continue;
                }
                int box = (parked_scan > 0 && !e.isDead() && com.oddlabs.tt.player.gauntlet.Intel.isParked(e) ? parked_scan : java.lang.Integer.MAX_VALUE);
                bool exempt = false;
                bool near_base = false;
                foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(own))
                {
                    if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(e.getGridX(), e.getGridY(), b.getGridX(), b.getGridY()) <= r2)
                    {
                        if (box != java.lang.Integer.MAX_VALUE && java.lang.Math.max(java.lang.Math.abs(e.getGridX() - b.getGridX()), java.lang.Math.abs(e.getGridY() - b.getGridY())) > box)
                        {
                            exempt = true;
                            continue;
                        }
                        near_base = true;
                        break;
                    }
                }
                bool near_peons = false;
                if (!near_base)
                {
                    for (int k = 0; k < np; k++)
                    {
                        if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(e.getGridX(), e.getGridY(), pxs[k], pys[k]) <= 12 * 12)
                        {
                            if (box != java.lang.Integer.MAX_VALUE && java.lang.Math.max(java.lang.Math.abs(e.getGridX() - pxs[k]), java.lang.Math.abs(e.getGridY() - pys[k])) > box)
                            {
                                exempt = true;
                                continue;
                            }
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
                else if (exempt)
                {
                    ai.aiLog().count("parked_exempt");
                }
            }
            foreach (com.oddlabs.tt.model.Unit e_2 in java.lang.JIter.of(intel.enemy_peons))
            {
                bool hostile = false;
                foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(intel.towers))
                {
                    if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(e_2.getGridX(), e_2.getGridY(), t.getGridX(), t.getGridY()) <= 5 * 5)
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
            base_threat_strength = 0.0f;
            foreach (com.oddlabs.tt.model.Unit e_3 in java.lang.JIter.of(at_base))
            {
                base_threat_strength += com.oddlabs.tt.player.gauntlet.Combat.@value(e_3);
            }
            com.oddlabs.tt.model.Building armory = intel.armory();
            float best = (float)(-1.0f);
            int nt = threats.size();
            int[] txs = new int[nt];
            int[] tys = new int[nt];
            float[] vals = new float[nt];
            bool[] in_base = new bool[nt];
            java.util.Set base_set = new java.util.LinkedHashSet(at_base);
            for (int i = 0; i < nt; i++)
            {
                com.oddlabs.tt.model.Unit e_4 = ((com.oddlabs.tt.model.Unit)threats.get(i));
                txs[i] = e_4.getGridX();
                tys[i] = e_4.getGridY();
                vals[i] = java.lang.Math.max(0.2f, com.oddlabs.tt.player.gauntlet.Combat.@value(e_4));
                in_base[i] = base_set.contains(e_4);
            }
            for (int i_2 = 0; i_2 < nt; i_2++)
            {
                long sx = 0;
                long sy = 0;
                int n = 0;
                float strength = 0.0f;
                bool @base = false;
                for (int j = 0; j < nt; j++)
                {
                    if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(txs[i_2], tys[i_2], txs[j], tys[j]) <= 15 * 15)
                    {
                        sx += txs[j];
                        sy += tys[j];
                        n++;
                        strength = (float)(strength + vals[j]);
                        @base |= in_base[j];
                    }
                }
                int cx = unchecked((int)((sx / n)));
                int cy = unchecked((int)((sy / n)));
                float weight = (float)(strength * ((@base ? 2.0f : 1.0f)));
                if (armory != null && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(cx, cy, armory.getGridX(), armory.getGridY()) <= 25 * 25)
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
            bool at_armory = armory != null && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(threat_x, threat_y, armory.getGridX(), armory.getGridY()) <= 25 * 25;
            threat_level = (base_threat_strength >= 3.0f || at_armory ? 2 : 1);
        }
        // Java: 742
        public void defend()
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            com.oddlabs.tt.model.Building armory = intel.armory();
            float home = (float)(armyStrength() + towersStrength());
            if ((((object)mode == (object)com.oddlabs.tt.player.gauntlet.Military.Mode.ATTACK) || ((object)mode == (object)com.oddlabs.tt.player.gauntlet.Military.Mode.MUSTER)) && base_threat_strength > home && base_threat_strength > (float)(ai.strategy().recall_ratio * attackStrength()))
            {
                ai.log(java.lang.JString.format("calling the army home: %.1f in the base against %.1f", base_threat_strength, home));
                recalled = true;
                endAttack();
            }
            java.util.List defenders = new java.util.ArrayList();
            foreach (java.util.Map.Entry e in java.lang.JIter.of(roles.entrySet()))
            {
                com.oddlabs.tt.player.gauntlet.Military.Role r = ((com.oddlabs.tt.player.gauntlet.Military.Role)e.getValue());
                com.oddlabs.tt.model.Unit u = ((com.oddlabs.tt.model.Unit)e.getKey());
                if (((object)r == (object)com.oddlabs.tt.player.gauntlet.Military.Role.ARMY) || (((object)r == (object)com.oddlabs.tt.player.gauntlet.Military.Role.RAID) && threat_level >= 2))
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
                    return java.lang.Integer.compare(com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(a.getGridX(), a.getGridY(), threat_x, threat_y), com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(b.getGridX(), b.getGridY(), threat_x, threat_y));
                }));
                float needed = (float)((float)(response * threat_strength) + 2.0f);
                float picked = 0.0f;
                int n = 0;
                while (n < defenders.size() && picked < needed)
                {
                    picked = (float)(picked + com.oddlabs.tt.player.gauntlet.Combat.@value(((com.oddlabs.tt.model.Unit)defenders.get(n++))));
                }
                defenders = new java.util.ArrayList(defenders.subList(0, n));
            }
            float ours = com.oddlabs.tt.player.gauntlet.Combat.total(defenders);
            float towers = 0.0f;
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(intel.towers))
            {
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(t.getGridX(), t.getGridY(), threat_x, threat_y) <= 16 * 16)
                {
                    towers = (float)(towers + com.oddlabs.tt.player.gauntlet.Combat.towerValue(t));
                }
            }
            com.oddlabs.tt.model.Unit chief = intel.chieftain;
            bool stable = ai.strategy().defend_stable;
            bool toot_ready = chief != null && ai.chieftain().stunReady() && !(ai.strategy().shred && ai.strategy().shred_strict);
            if (stable && toot_ready && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(chief.getGridX(), chief.getGridY(), threat_x, threat_y) > 30 * 30)
            {
                toot_ready = false;
                ai.aiLog().count("stun_credit_denied");
            }
            float effective = (float)((float)(ours + towers) + ((toot_ready ? (float)(0.6f * threat_strength) : 0.0f)));
            float behind = (ai.strategy().threat_look > 0 && ai.enemiesAlive() == 1 ? enemyStrengthNear(threat_x, threat_y, ai.strategy().threat_look) : 0.0f);
            float enemy_raw = java.lang.Math.max(threat_strength, behind);
            float enemy = (float)(enemy_raw * ((enemyStunReadyNear(threat_x, threat_y, 25) ? ai.strategy().enemy_stun_mult : 1.0f)));
            float engage_ratio = (float)(0.8f - (float)(engage_state * ai.strategy().defend_hysteresis));
            int armory_cells = (stable && engage_state == 1 ? 17 : 14);
            bool at_armory = armory != null && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(threat_x, threat_y, armory.getGridX(), armory.getGridY()) <= armory_cells * armory_cells;
            bool engage = effective >= (float)(engage_ratio * enemy) || at_armory;
            if (stable)
            {
                if (engage && engage_state != 1)
                {
                    engage_since = ai.time();
                }
                else if (!engage && engage_state == 1 && (float)(ai.time() - engage_since) < 3.0f)
                {
                    engage = true;
                    ai.aiLog().count("engage_held");
                }
            }
            if (!engage && engage_state != (-1) && effective >= (float)(engage_ratio * enemy_raw))
            {
                ai.aiLog().count("stun_fear_fallback");
            }
            if (threat_level != last_logged_threat || engage != last_logged_engage)
            {
                last_logged_threat = threat_level;
                last_logged_engage = engage;
                ai.log(java.lang.JString.format("threat %d: %.1f at %d,%d; defenders %.1f towers %.1f -> %s", threat_level, threat_strength, threat_x, threat_y, ours, towers, (engage ? (object)("engage") : (object)("fall back"))));
            }
            engage_state = (engage ? 1 : (-1));
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
            if (engage && hold > 0.0f && (ai.enemiesAlive() == 1 || ai.strategy().hold_multi) && enemy >= (float)(hold * java.lang.Math.max(1.0f, ours)) && holdAtPost(defenders))
            {
                if (ai.enemiesAlive() != 1)
                {
                    ai.aiLog().count("hold_post_multi");
                }
                evacuatePeons();
                return;
            }
            if (engage)
            {
                foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(defenders))
                {
                    if (((object)((com.oddlabs.tt.player.gauntlet.Military.Role)roles.get(u_2)) == (object)com.oddlabs.tt.player.gauntlet.Military.Role.RAID))
                    {
                        roles.put(u_2, com.oddlabs.tt.player.gauntlet.Military.Role.ARMY);
                    }
                }
                engageSpread(defenders, threats, threat_x, threat_y, false);
                huntChieftains(defenders);
            }
            else if (armory != null)
            {
                foreach (com.oddlabs.tt.model.Unit u_3 in java.lang.JIter.of(defenders))
                {
                    attackGround(u_3, armory.getGridX(), armory.getGridY(), false);
                }
            }
            evacuatePeons();
        }
        // Java: 856
        public bool holdAtPost(java.util.List defenders)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            com.oddlabs.tt.model.Building post = null;
            int best = java.lang.Integer.MAX_VALUE;
            java.util.List own = new java.util.ArrayList(intel.towers);
            own.addAll(intel.armories);
            own.addAll(intel.quarters);
            foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(own))
            {
                int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(b.getGridX(), b.getGridY(), threat_x, threat_y);
                if (((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID() == com.oddlabs.tt.model.Race.BUILDING_TOWER && com.oddlabs.tt.player.gauntlet.Intel.isTowerActive(b))
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
                if (!e.isDead() && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(e.getGridX(), e.getGridY(), px, py) <= 14 * 14)
                {
                    close.add(e);
                }
            }
            java.util.List at_post = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(defenders))
            {
                if (((object)((com.oddlabs.tt.player.gauntlet.Military.Role)roles.get(u)) == (object)com.oddlabs.tt.player.gauntlet.Military.Role.RAID))
                {
                    roles.put(u, com.oddlabs.tt.player.gauntlet.Military.Role.ARMY);
                }
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(u.getGridX(), u.getGridY(), px, py) <= 12 * 12)
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
        // Java: 896
        public float towersStrength()
        {
            float s = 0.0f;
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(ai.intel().towers))
            {
                s = (float)(s + com.oddlabs.tt.player.gauntlet.Combat.towerValue(t));
            }
            return s;
        }
        // Java: 904
        public bool enemyStunReadyNear(int x, int y, int radius)
        {
            foreach (com.oddlabs.tt.model.Unit c in java.lang.JIter.of(ai.intel().enemy_chieftains))
            {
                if (c.isDead() || com.oddlabs.tt.player.gauntlet.Intel.isStunned(c))
                {
                    continue;
                }
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, c.getGridX(), c.getGridY()) > radius * radius)
                {
                    continue;
                }
                if (enemyThreatReady(c))
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 921
        public bool enemyThreatReady(com.oddlabs.tt.model.Unit chief)
        {
            if (ai.strategy().hidden_info)
            {
                return enemySpellReady(chief);
            }
            float? cast = ((float?)enemy_casts.get(chief));
            if (cast != null)
            {
                return (float)(ai.time() - ((float)cast)) >= ai.strategy().enemy_spell_recharge;
            }
            if (!ai.strategy().enemy_first_seen)
            {
                return true;
            }
            float? seen = ((float?)enemy_chief_seen.get(chief));
            return seen == null || (float)(ai.time() - ((float)seen)) >= ai.strategy().enemy_spell_recharge;
        }
        // Java: 937
        public bool enemySpellReady(com.oddlabs.tt.model.Unit chief)
        {
            if (ai.strategy().hidden_info)
            {
                return chief.getMagicProgress(0) >= 1.0f || chief.getMagicProgress(1) >= 1.0f;
            }
            float? cast = ((float?)enemy_casts.get(chief));
            return cast == null || (float)(ai.time() - ((float)cast)) >= com.oddlabs.tt.player.gauntlet.Military.SPELL_RECHARGE;
        }
        // Java: 948
        public void watchEnemyCasts()
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            enemy_casts.keySet().removeIf(new java.util.function.Predicate_L((object a0) => { return ((com.oddlabs.tt.model.Unit)a0).isDead(); }));
            if (ai.strategy().enemy_first_seen)
            {
                enemy_chief_seen.keySet().removeIf(new java.util.function.Predicate_L((object a0) => { return ((com.oddlabs.tt.model.Unit)a0).isDead(); }));
                foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(intel.enemy_chieftains))
                {
                    if (!e.isDead())
                    {
                        enemy_chief_seen.putIfAbsent(e, ai.time());
                    }
                }
            }
            foreach (com.oddlabs.tt.model.Unit e_2 in java.lang.JIter.of(intel.enemy_chieftains))
            {
                if (!e_2.isDead() && (e_2.getCurrentController() is com.oddlabs.tt.model.behaviour.MagicController))
                {
                    enemy_casts.put(e_2, ai.time());
                }
            }
            java.util.List stunned = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(intel.warriors))
            {
                if (com.oddlabs.tt.player.gauntlet.Intel.isStunned(u))
                {
                    stunned.add(u);
                }
            }
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(intel.peons))
            {
                if (com.oddlabs.tt.player.gauntlet.Intel.isStunned(u_2))
                {
                    stunned.add(u_2);
                }
            }
            if (stunned.size() >= own_stunned_before + 2)
            {
                int[] c = com.oddlabs.tt.player.gauntlet.MapAnalysis.centroid(stunned);
                com.oddlabs.tt.model.Unit caster = null;
                int best = 25 * 25;
                foreach (com.oddlabs.tt.model.Unit e_3 in java.lang.JIter.of(intel.enemy_chieftains))
                {
                    int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(e_3.getGridX(), e_3.getGridY(), c[0], c[1]);
                    if (!e_3.isDead() && d <= best)
                    {
                        best = d;
                        caster = e_3;
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
        // Java: 988
        public bool isDodging(com.oddlabs.tt.model.Unit u)
        {
            return dodges.containsKey(u);
        }
        // Java: 996
        public void dodgeSpells()
        {
            float now = ai.time();
            if (!dodges.isEmpty())
            {
                dodges.values().removeIf(new java.util.function.Predicate_L((object d_l) =>
                {
                    com.oddlabs.tt.player.gauntlet.Military.Dodge d = (com.oddlabs.tt.player.gauntlet.Military.Dodge)d_l;
                    return d.until() < now;
                }));
            }
            if (!windups.isEmpty())
            {
                windups.entrySet().removeIf(new java.util.function.Predicate_L((object w_l) =>
                {
                    java.util.Map.Entry w = (java.util.Map.Entry)w_l;
                    return ((float)((float?)w.getValue())) < now && (((com.oddlabs.tt.model.Unit)w.getKey()).isDead() || !((((com.oddlabs.tt.model.Unit)w.getKey()).getCurrentController() is com.oddlabs.tt.model.behaviour.MagicController)));
                }));
            }
            updateFogs(now);
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(intel.enemy_chieftains))
            {
                watchCaster(e, now);
            }
            if (intel.chieftain != null)
            {
                watchCaster(intel.chieftain, now);
            }
            foreach (com.oddlabs.tt.player.gauntlet.Military.Fog f in java.lang.JIter.of(fogs))
            {
                keepOutOf(f, now);
            }
            keepHuntingCasters(now);
            foreach (java.util.Map.Entry en in java.lang.JIter.of(dodges.entrySet()))
            {
                com.oddlabs.tt.model.Unit u = ((com.oddlabs.tt.model.Unit)en.getKey());
                com.oddlabs.tt.player.gauntlet.Military.Dodge d_2 = ((com.oddlabs.tt.player.gauntlet.Military.Dodge)en.getValue());
                if (u.isDead() || u.isMounted() || com.oddlabs.tt.player.gauntlet.Intel.isStunned(u) || com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(u.getGridX(), u.getGridY(), d_2.x(), d_2.y()) <= 2 * 2)
                {
                    continue;
                }
                if ((u.getCurrentController() is com.oddlabs.tt.model.behaviour.WalkController w_2) && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(w_2.getTarget().getGridX(), w_2.getTarget().getGridY(), d_2.x(), d_2.y()) <= 3 * 3)
                {
                    continue;
                }
                ai.landscapeOrder(com.oddlabs.tt.model.Selectable.newArray(u), d_2.x(), d_2.y(), com.oddlabs.tt.model.Action.MOVE, false);
            }
        }
        // Java: 1029
        public void watchCaster(com.oddlabs.tt.model.Unit e, float now)
        {
            if (e.isDead() || windups.containsKey(e) || !((e.getCurrentController() is com.oddlabs.tt.model.behaviour.MagicController)))
            {
                return;
            }
            windups.put(e, (float)(now + com.oddlabs.tt.player.gauntlet.Military.STUN_WINDUP));
            com.oddlabs.tt.model.Race vikings = ai.owner().getWorld().getRacesResources().getRace(com.oddlabs.tt.model.RacesResources.RACE_VIKINGS);
            if (((object)e.getOwner().getRace() != (object)vikings))
            {
                if (ai.strategy().dodge_fog)
                {
                    fogs.add(new com.oddlabs.tt.player.gauntlet.Military.Fog(e, (float)(e.getPositionX() + (float)(com.oddlabs.tt.player.gauntlet.Military.FOG_OFFSET * e.getDirectionX())), (float)(e.getPositionY() + (float)(com.oddlabs.tt.player.gauntlet.Military.FOG_OFFSET * e.getDirectionY())), now));
                }
            }
            else if (((object)e != (object)ai.intel().chieftain) && ai.strategy().dodge_stun)
            {
                bool blast = e.getLastMagicIndex() == com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_BLAST;
                if (blast)
                {
                    ai.aiLog().count("enemy_blast");
                }
                if (!ai.strategy().dodge_blast_only || !ai.strategy().stun_cancel || blast)
                {
                    dodgeStun(e, now);
                }
            }
        }
        // Java: 1053
        public void updateFogs(float now)
        {
            for (java.util.Iterator it = fogs.iterator(); it.hasNext(); )
            {
                com.oddlabs.tt.player.gauntlet.Military.Fog f = ((com.oddlabs.tt.player.gauntlet.Military.Fog)it.next());
                if (now > (float)((float)((float)(f.cast + com.oddlabs.tt.player.gauntlet.Military.FOG_RELEASE) + com.oddlabs.tt.player.gauntlet.Military.FOG_TIME) + 0.5f))
                {
                    it.remove();
                }
                else if (!f.confirmed && now >= (float)((float)(f.cast + com.oddlabs.tt.player.gauntlet.Military.FOG_RELEASE) + 0.1f))
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
        // Java: 1076
        public void keepOutOf(com.oddlabs.tt.player.gauntlet.Military.Fog f, float now)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            java.util.List units = new java.util.ArrayList(intel.warriors);
            units.addAll(intel.peons);
            if (intel.chieftain != null && ((object)f.caster != (object)intel.chieftain))
            {
                units.add(intel.chieftain);
            }
            int grid = ai.map().getSize();
            float reach = (float)(com.oddlabs.tt.player.gauntlet.Military.FOG_RADIUS + 3.0f);
            float until = java.lang.Math.min((float)(now + 4.0f), (float)((float)(f.cast + com.oddlabs.tt.player.gauntlet.Military.FOG_RELEASE) + com.oddlabs.tt.player.gauntlet.Military.FOG_TIME));
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(units))
            {
                if (u.isDead() || u.isMounted() || com.oddlabs.tt.player.gauntlet.Intel.isStunned(u) || dodges.containsKey(u) || (u.getCurrentController() is com.oddlabs.tt.model.behaviour.MagicController))
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
                float to = (float)(((float)(com.oddlabs.tt.player.gauntlet.Military.FOG_RADIUS + 7.0f)) / d);
                int tx = java.lang.Math.clamp(com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate((float)(f.x + (float)(dx * to))), 0, grid - 1);
                int ty = java.lang.Math.clamp(com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate((float)(f.y + (float)(dy * to))), 0, grid - 1);
                dodges.put(u, new com.oddlabs.tt.player.gauntlet.Military.Dodge(tx, ty, until));
                ai.landscapeOrder(com.oddlabs.tt.model.Selectable.newArray(u), tx, ty, com.oddlabs.tt.model.Action.MOVE, false);
            }
        }
        // Java: 1112
        public void dodgeStun(com.oddlabs.tt.model.Unit e, float now)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            float release_v = (float)(now + com.oddlabs.tt.player.gauntlet.Military.STUN_WINDUP);
            float sx = (float)(e.getPositionX() + (float)(com.oddlabs.tt.player.gauntlet.Military.STUN_OFFSET * e.getDirectionX()));
            float sy = (float)(e.getPositionY() + (float)(com.oddlabs.tt.player.gauntlet.Military.STUN_OFFSET * e.getDirectionY()));
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
                if (u.isDead() || u.isMounted() || com.oddlabs.tt.player.gauntlet.Intel.isStunned(u))
                {
                    continue;
                }
                float dx = (float)(u.getPositionX() - sx);
                float dy = (float)(u.getPositionY() - sy);
                float d = ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy)))));
                if (d > (float)(com.oddlabs.tt.player.gauntlet.Military.STUN_RADIUS + 10.0f))
                {
                    continue;
                }
                float @out = (float)((float)(com.oddlabs.tt.player.gauntlet.Military.STUN_RADIUS + 3.0f) - d);
                if (@out > (float)((float)(0.85f * ((com.oddlabs.tt.model.UnitTemplate)u.getTemplate()).getMetersPerSecond()) * com.oddlabs.tt.player.gauntlet.Military.STUN_WINDUP))
                {
                    if (ai.strategy().hunt_caster && intel.warriors.contains(u) && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(u.getGridX(), u.getGridY(), e.getGridX(), e.getGridY()) <= ai.strategy().hunt_caster_cells * ai.strategy().hunt_caster_cells)
                    {
                        caster_hunters.put(u, release_v);
                        huntCaster(u, e);
                        hunters++;
                    }
                    continue;
                }
                float to = (float)(java.lang.Math.max(d, (float)(com.oddlabs.tt.player.gauntlet.Military.STUN_RADIUS + 6.0f)) / java.lang.Math.max(d, 0.1f));
                int tx = java.lang.Math.clamp(com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate((float)(sx + (float)(dx * to))), 0, grid - 1);
                int ty = java.lang.Math.clamp(com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate((float)(sy + (float)(dy * to))), 0, grid - 1);
                dodges.put(u, new com.oddlabs.tt.player.gauntlet.Military.Dodge(tx, ty, (float)(release_v + 0.3f)));
                ai.landscapeOrder(com.oddlabs.tt.model.Selectable.newArray(u), tx, ty, com.oddlabs.tt.model.Action.MOVE, false);
                n++;
            }
            if (hunters > 0)
            {
                caster_targets.put(e, release_v);
            }
            if (n > 0 || hunters > 0)
            {
                ai.log("enemy chieftain winds up at " + (e.getGridX()) + "," + (e.getGridY()) + ": " + (n) + " units run out of reach, " + (hunters) + " go for him");
            }
        }
        // Java: 1159
        public void huntCaster(com.oddlabs.tt.model.Unit u, com.oddlabs.tt.model.Unit chief)
        {
            hunt_targets.put(u, chief);
            last_order.put(u, ai.time());
            ai.intel().warrior_states.put(u, com.oddlabs.tt.player.gauntlet.Intel.WarriorState.FIGHT);
            ai.owner().setTarget(com.oddlabs.tt.model.Selectable.newArray(u), chief, com.oddlabs.tt.model.Action.ATTACK, true);
        }
        // Java: 1167
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
                if (u.isDead() || com.oddlabs.tt.player.gauntlet.Intel.isStunned(u))
                {
                    continue;
                }
                if (chief == null || !caster_targets.containsKey(chief))
                {
                    chief = null;
                    foreach (com.oddlabs.tt.model.Unit c_2 in java.lang.JIter.of(caster_targets.keySet()))
                    {
                        if (chief == null || com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(u.getGridX(), u.getGridY(), c_2.getGridX(), c_2.getGridY()) < com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(u.getGridX(), u.getGridY(), chief.getGridX(), chief.getGridY()))
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
        // Java: 1195
        public void huntChieftains(java.util.List units)
        {
            foreach (com.oddlabs.tt.model.Unit chief in java.lang.JIter.of(ai.intel().enemy_chieftains))
            {
                if (chief.isDead())
                {
                    continue;
                }
                if (ai.strategy().chief_per_hit && chief.getHitPoints() > 20 && !com.oddlabs.tt.player.gauntlet.Intel.isStunned(chief))
                {
                    continue;
                }
                int[] c = com.oddlabs.tt.player.gauntlet.MapAnalysis.centroid(units);
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(c[0], c[1], chief.getGridX(), chief.getGridY()) > 20 * 20)
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
                        com.oddlabs.tt.player.gauntlet.Intel.WarriorState s = ((com.oddlabs.tt.player.gauntlet.Intel.WarriorState)ai.intel().warrior_states.get(u));
                        if (((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorState.STUNNED) || ((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorState.ENTER))
                        {
                            continue;
                        }
                        int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(u.getGridX(), u.getGridY(), chief.getGridX(), chief.getGridY());
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
        // Java: 1241
        public bool raiding(com.oddlabs.tt.model.Unit e, java.util.List own, int r2)
        {
            if (!com.oddlabs.tt.player.gauntlet.Military.nearAny(own, e.getGridX(), e.getGridY(), r2))
            {
                return false;
            }
            foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(ai.intel().enemy_buildings))
            {
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(e.getGridX(), e.getGridY(), b.getGridX(), b.getGridY()) > 30 * 30)
                {
                    continue;
                }
                if (!com.oddlabs.tt.player.gauntlet.Military.nearAny(own, b.getGridX(), b.getGridY(), r2))
                {
                    return false;
                }
            }
            return true;
        }
        // Java: 1253
        public static bool nearAny(java.util.List buildings, int x, int y, int r2)
        {
            foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(buildings))
            {
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, b.getGridX(), b.getGridY()) <= r2)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 1264
        public void peonMilitia()
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            militia_orders.keySet().removeIf(new java.util.function.Predicate_L((object a0) => { return ((com.oddlabs.tt.model.Unit)a0).isDead(); }));
            int[] c = com.oddlabs.tt.player.gauntlet.MapAnalysis.centroid(raiders);
            if (enemyStrengthNear(c[0], c[1], 20) > 1.0f)
            {
                return;
            }
            float guard = 0.0f;
            foreach (java.util.Map.Entry e in java.lang.JIter.of(roles.entrySet()))
            {
                com.oddlabs.tt.model.Unit u = ((com.oddlabs.tt.model.Unit)e.getKey());
                if (((object)((com.oddlabs.tt.player.gauntlet.Military.Role)e.getValue()) != (object)com.oddlabs.tt.player.gauntlet.Military.Role.TOWER) && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(u.getGridX(), u.getGridY(), c[0], c[1]) <= 30 * 30)
                {
                    guard = (float)(guard + com.oddlabs.tt.player.gauntlet.Combat.@value(u));
                }
            }
            if (guard >= (float)(0.4f * raiders.size()))
            {
                return;
            }
            java.util.List militia = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit p in java.lang.JIter.of(intel.peons))
            {
                com.oddlabs.tt.player.gauntlet.Intel.PeonState s = ((com.oddlabs.tt.player.gauntlet.Intel.PeonState)intel.peon_states.get(p));
                if (((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.TRANSIT) || ((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.STUNNED) || ((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.SAPPER) || ((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.SHEPHERD))
                {
                    continue;
                }
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(p.getGridX(), p.getGridY(), c[0], c[1]) <= 30 * 30)
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
                        close |= com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(p_2.getGridX(), p_2.getGridY(), e_2.getGridX(), e_2.getGridY()) <= 8 * 8;
                    }
                    com.oddlabs.tt.model.Building shelter = (close ? com.oddlabs.tt.player.gauntlet.MapAnalysis.nearest(intel.quarters, p_2.getGridX(), p_2.getGridY()) : (com.oddlabs.tt.model.Building)(null));
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
                intel.peon_states.put(p_3, com.oddlabs.tt.player.gauntlet.Intel.PeonState.FIGHT);
                float? last = ((float?)militia_orders.get(p_3));
                if (last != null && (float)(ai.time() - ((float)last)) < 3.0f)
                {
                    continue;
                }
                com.oddlabs.tt.model.Unit target = null;
                int best = java.lang.Integer.MAX_VALUE;
                foreach (com.oddlabs.tt.model.Unit e_3 in java.lang.JIter.of(raiders))
                {
                    int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(p_3.getGridX(), p_3.getGridY(), e_3.getGridX(), e_3.getGridY());
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
        // Java: 1328
        public void answerRaiders()
        {
            int[] c = com.oddlabs.tt.player.gauntlet.MapAnalysis.centroid(raiders);
            if (threat_level == 0)
            {
                java.util.List home = new java.util.ArrayList();
                foreach (java.util.Map.Entry e in java.lang.JIter.of(roles.entrySet()))
                {
                    if (((object)((com.oddlabs.tt.player.gauntlet.Military.Role)e.getValue()) == (object)com.oddlabs.tt.player.gauntlet.Military.Role.ARMY) && ((object)((com.oddlabs.tt.player.gauntlet.Intel.WarriorState)ai.intel().warrior_states.get(((com.oddlabs.tt.model.Unit)e.getKey()))) != (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorState.STUNNED))
                    {
                        home.add(((com.oddlabs.tt.model.Unit)e.getKey()));
                    }
                }
                home.sort(new java.util.Comparator_L((object a_l, object b_l) =>
                {
                    com.oddlabs.tt.model.Unit a = (com.oddlabs.tt.model.Unit)a_l;
                    com.oddlabs.tt.model.Unit b = (com.oddlabs.tt.model.Unit)b_l;
                    return java.lang.Integer.compare(com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(a.getGridX(), a.getGridY(), c[0], c[1]), com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(b.getGridX(), b.getGridY(), c[0], c[1]));
                }));
                float needed = (float)((float)(0.4f * raiders.size()) + 1.0f);
                float sent = 0.0f;
                int n = 0;
                while (n < home.size() && sent < needed)
                {
                    sent = (float)(sent + com.oddlabs.tt.player.gauntlet.Combat.@value(((com.oddlabs.tt.model.Unit)home.get(n++))));
                }
                if (sent >= needed)
                {
                    engageSpread(home.subList(0, n), raiders, c[0], c[1], false);
                    return;
                }
            }
            peonMilitia();
        }
        // Java: 1351
        public void peonRush()
        {
            if (!ai.strategy().peon_rush || ai.time() > 300.0f || (float)(ai.time() - last_peon_rush) < 4.0f)
            {
                return;
            }
            last_peon_rush = ai.time();
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            foreach (com.oddlabs.tt.model.Unit p in java.lang.JIter.of(intel.peons))
            {
                com.oddlabs.tt.model.Unit target = null;
                int best = java.lang.Integer.MAX_VALUE;
                foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(intel.enemy_peons))
                {
                    int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(p.getGridX(), p.getGridY(), e.getGridX(), e.getGridY());
                    if (d < best)
                    {
                        best = d;
                        target = e;
                    }
                }
                if (target != null)
                {
                    intel.peon_states.put(p, com.oddlabs.tt.player.gauntlet.Intel.PeonState.FIGHT);
                    ai.owner().setTarget(com.oddlabs.tt.model.Selectable.newArray(p), target, com.oddlabs.tt.model.Action.ATTACK, true);
                }
            }
        }
        // Java: 1373
        public void evacuatePeons()
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            com.oddlabs.tt.model.Building armory = intel.armory();
            java.util.List evacuate = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit p in java.lang.JIter.of(intel.peons))
            {
                com.oddlabs.tt.player.gauntlet.Intel.PeonState s = ((com.oddlabs.tt.player.gauntlet.Intel.PeonState)intel.peon_states.get(p));
                if (((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.TRANSIT) || ((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.STUNNED) || ((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.SAPPER) || ((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.SHEPHERD))
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
                if (ai.economy().evacExempt(p))
                {
                    continue;
                }
                evacuate.add(p);
            }
            bool bank_full = !evacuate.isEmpty() && ai.economy().bankFull(armory);
            if (!evacuate.isEmpty() && ai.economy().raidEvacuating(armory))
            {
                armory = null;
            }
            foreach (com.oddlabs.tt.model.Unit p_2 in java.lang.JIter.of(evacuate))
            {
                com.oddlabs.tt.model.Building shelter = null;
                int best = java.lang.Integer.MAX_VALUE;
                if (armory != null && !bank_full && !threatNear(armory.getGridX(), armory.getGridY(), 6))
                {
                    shelter = armory;
                }
                foreach (com.oddlabs.tt.model.Building q in java.lang.JIter.of(intel.quarters))
                {
                    if (threatNear(q.getGridX(), q.getGridY(), 6) || ai.economy().isDoomed(q))
                    {
                        continue;
                    }
                    int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(q.getGridX(), q.getGridY(), p_2.getGridX(), p_2.getGridY());
                    int da = (((object)shelter == (object)armory) && armory != null ? com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(armory.getGridX(), armory.getGridY(), p_2.getGridX(), p_2.getGridY()) : java.lang.Integer.MAX_VALUE);
                    if (d < best && d < da)
                    {
                        best = d;
                        shelter = q;
                    }
                }
                if (shelter == null && bank_full && armory != null && !threatNear(armory.getGridX(), armory.getGridY(), 6))
                {
                    shelter = armory;
                }
                else if (bank_full && shelter != null)
                {
                    ai.aiLog().count("bank_shelter_quarters");
                }
                if (shelter != null && shelter.getUnitContainer() != null)
                {
                    ai.owner().setTarget(com.oddlabs.tt.model.Selectable.newArray(p_2), shelter, com.oddlabs.tt.model.Action.DEFAULT, false);
                }
            }
        }
        public java.util.Map front_entry;
        // Java: 1435
        public int[] frontCell(com.oddlabs.tt.model.Building tower)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            int tx = tower.getGridX();
            int ty = tower.getGridY();
            long ex = 0;
            long ey = 0;
            int n = 0;
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(intel.enemy_warriors))
            {
                if (!e.isDead() && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(tx, ty, e.getGridX(), e.getGridY()) <= 45 * 45)
                {
                    ex += e.getGridX();
                    ey += e.getGridY();
                    n++;
                }
            }
            float dx = default;
            float dy = default;
            float[] live = (n == 0 && ai.strategy().tower_face_live ? liveFacing(tx, ty) : (float[])(null));
            if (n > 0)
            {
                dx = (float)((float)(ex / ((float)(n))) - tx);
                dy = (float)((float)(ey / ((float)(n))) - ty);
            }
            else if (live != null)
            {
                dx = live[0];
                dy = live[1];
                ai.aiLog().count("tower_face_live");
            }
            else
            {
                dx = ai.planner().getEnemyX() - tx;
                dy = ai.planner().getEnemyY() - ty;
                ai.aiLog().count("tower_face_fallback");
            }
            double @base = java.lang.StrictMath.atan2(dy, dx);
            foreach (double turn in new double[] { 0, java.lang.Math.PI / 4, -java.lang.Math.PI / 4, java.lang.Math.PI / 2, -java.lang.Math.PI / 2 })
            {
                int fx = tx + unchecked((int)(java.lang.Math.round(3 * java.lang.StrictMath.cos(@base + turn))));
                int fy = ty + unchecked((int)(java.lang.Math.round(3 * java.lang.StrictMath.sin(@base + turn))));
                if (!ai.map().passable(fx, fy))
                {
                    continue;
                }
                bool hot = false;
                foreach (com.oddlabs.tt.model.Unit e_2 in java.lang.JIter.of(intel.enemy_warriors))
                {
                    if (!e_2.isDead() && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(fx, fy, e_2.getGridX(), e_2.getGridY()) <= 10 * 10)
                    {
                        hot = true;
                        break;
                    }
                }
                return (hot ? (int[])(null) : new int[] { fx, fy });
            }
            return null;
        }
        // Java: 1485
        public float[] liveFacing(int tx, int ty)
        {
            float[] live = ai.liveEnemyCenter();
            if (live == null)
            {
                return null;
            }
            float ux = (float)(live[0] - tx);
            float uy = (float)(live[1] - ty);
            float nu = ((float)(java.lang.Math.sqrt((float)((float)(ux * ux) + (float)(uy * uy)))));
            if (nu < 0.001f)
            {
                return null;
            }
            ux = (float)(ux / nu);
            uy = (float)(uy / nu);
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            long cx = 0;
            long cy = 0;
            int n = 0;
            foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(intel.quarters))
            {
                cx += b.getGridX();
                cy += b.getGridY();
                n++;
            }
            foreach (com.oddlabs.tt.model.Building b_2 in java.lang.JIter.of(intel.armories))
            {
                cx += b_2.getGridX();
                cy += b_2.getGridY();
                n++;
            }
            float bx = (n > 0 ? (float)(cx / ((float)(n))) : ai.planner().getStartX());
            float by = (n > 0 ? (float)(cy / ((float)(n))) : ai.planner().getStartY());
            float wx = (float)(tx - bx);
            float wy = (float)(ty - by);
            float nw = ((float)(java.lang.Math.sqrt((float)((float)(wx * wx) + (float)(wy * wy)))));
            float dx = (float)(ux + ((nw > 0.0f ? (float)(wx / nw) : 0.0f)));
            float dy = (float)(uy + ((nw > 0.0f ? (float)(wy / nw) : 0.0f)));
            if (java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy))) < 0.5f)
            {
                return new float[] { ux, uy };
            }
            return new float[] { dx, dy };
        }
        public java.util.Map reaimed;
        public float last_reaim;
        // Java: 1532
        public void reaimTowers()
        {
            if (!ai.strategy().tower_reaim || (float)(ai.time() - last_reaim) < 2.0f)
            {
                return;
            }
            last_reaim = ai.time();
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            java.util.List idle = new java.util.ArrayList();
            java.util.List awake = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(intel.enemy_warriors))
            {
                if (e.isDead())
                {
                    continue;
                }
                ((com.oddlabs.tt.player.gauntlet.Intel.isParked(e) ? idle : awake)).add(e);
            }
            if (idle.isEmpty())
            {
                return;
            }
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(intel.towers))
            {
                if (tower_assignments.containsValue(t) || ai.economy().isDoomed(t))
                {
                    continue;
                }
                com.oddlabs.tt.model.Unit gunner = com.oddlabs.tt.player.gauntlet.Military.readyGunner(t);
                if (gunner == null)
                {
                    continue;
                }
                float? last = ((float?)reaimed.get(t));
                if (last != null && (float)(ai.time() - ((float)last)) < 30.0f)
                {
                    continue;
                }
                int tx = t.getGridX();
                int ty = t.getGridY();
                bool quiet = true;
                foreach (com.oddlabs.tt.model.Unit e_2 in java.lang.JIter.of(awake))
                {
                    if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(tx, ty, e_2.getGridX(), e_2.getGridY()) <= 14 * 14)
                    {
                        quiet = false;
                        break;
                    }
                }
                if (!quiet)
                {
                    continue;
                }
                int gx = gunner.getGridX();
                int gy = gunner.getGridY();
                int now = 0;
                foreach (com.oddlabs.tt.model.Unit e_3 in java.lang.JIter.of(idle))
                {
                    if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(gx, gy, e_3.getGridX(), e_3.getGridY()) <= com.oddlabs.tt.player.gauntlet.Military.GARRISON_REACH2)
                    {
                        now++;
                    }
                }
                if (now > 0)
                {
                    continue;
                }
                int[] best = null;
                int best_n = 0;
                for (int dy = (-2); dy <= 2; dy++)
                {
                    for (int dx = (-2); dx <= 2; dx++)
                    {
                        if (java.lang.Math.max(java.lang.Math.abs(dx), java.lang.Math.abs(dy)) != 2)
                        {
                            continue;
                        }
                        int cx = tx + dx;
                        int cy = ty + dy;
                        if (!ai.map().passable(cx, cy))
                        {
                            continue;
                        }
                        int n = 0;
                        foreach (com.oddlabs.tt.model.Unit e_4 in java.lang.JIter.of(idle))
                        {
                            if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(cx, cy, e_4.getGridX(), e_4.getGridY()) <= com.oddlabs.tt.player.gauntlet.Military.GARRISON_REACH2)
                            {
                                n++;
                            }
                        }
                        if (n > best_n)
                        {
                            best_n = n;
                            best = new int[] { cx, cy };
                        }
                    }
                }
                if (best == null || best_n < 3)
                {
                    continue;
                }
                bool safe = true;
                foreach (com.oddlabs.tt.model.Unit e_5 in java.lang.JIter.of(awake))
                {
                    if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(best[0], best[1], e_5.getGridX(), e_5.getGridY()) <= 12 * 12)
                    {
                        safe = false;
                        break;
                    }
                }
                if (!safe)
                {
                    continue;
                }
                reaimed.put(t, ai.time());
                ai.owner().exitTower(t);
                if (gunner.isDead() || gunner.isMounted())
                {
                    continue;
                }
                roles.put(gunner, com.oddlabs.tt.player.gauntlet.Military.Role.TOWER);
                tower_assignments.put(gunner, t);
                front_entry.put(gunner, new float[] { best[0], best[1], (float)(ai.time() - 6.0f) });
                ai.landscapeOrder(com.oddlabs.tt.model.Selectable.newArray(gunner), best[0], best[1], com.oddlabs.tt.model.Action.MOVE, false);
                ai.aiLog().count("tower_reaim");
                for (int i = 0; i < best_n; i++)
                {
                    ai.aiLog().count("reaim_gain");
                }
            }
        }
        // Java: 1616
        public void manTowers()
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            reaimTowers();
            for (java.util.Iterator it = front_entry.entrySet().iterator(); it.hasNext(); )
            {
                java.util.Map.Entry e = ((java.util.Map.Entry)it.next());
                com.oddlabs.tt.model.Unit u = ((com.oddlabs.tt.model.Unit)e.getKey());
                com.oddlabs.tt.model.Building t = ((com.oddlabs.tt.model.Building)tower_assignments.get(u));
                if (u.isDead() || t == null || t.isDead() || com.oddlabs.tt.player.gauntlet.Intel.isTowerManned(t) || ai.economy().isDoomed(t))
                {
                    it.remove();
                    continue;
                }
                float[] f = ((float[])e.getValue());
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(u.getGridX(), u.getGridY(), java.lang.JC.f2i(f[0]), java.lang.JC.f2i(f[1])) <= 2 * 2 || (float)(ai.time() - f[2]) > 12.0f)
                {
                    it.remove();
                    ai.owner().setTarget(com.oddlabs.tt.model.Selectable.newArray(u), t, com.oddlabs.tt.model.Action.DEFAULT, false);
                }
            }
            foreach (com.oddlabs.tt.model.Building tower in java.lang.JIter.of(intel.towers))
            {
                if (com.oddlabs.tt.player.gauntlet.Intel.isTowerManned(tower) || tower_assignments.containsValue(tower) || ai.economy().isDoomed(tower))
                {
                    continue;
                }
                com.oddlabs.tt.model.Unit best = null;
                int best_score = java.lang.Integer.MAX_VALUE;
                foreach (java.util.Map.Entry e_2 in java.lang.JIter.of(roles.entrySet()))
                {
                    if (((object)((com.oddlabs.tt.player.gauntlet.Military.Role)e_2.getValue()) != (object)com.oddlabs.tt.player.gauntlet.Military.Role.ARMY))
                    {
                        continue;
                    }
                    com.oddlabs.tt.model.Unit u_2 = ((com.oddlabs.tt.model.Unit)e_2.getKey());
                    com.oddlabs.tt.player.gauntlet.Intel.WarriorState s = ((com.oddlabs.tt.player.gauntlet.Intel.WarriorState)intel.warrior_states.get(u_2));
                    if (((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorState.FIGHT) || ((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorState.STUNNED) || s == null)
                    {
                        continue;
                    }
                    int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(u_2.getGridX(), u_2.getGridY(), tower.getGridX(), tower.getGridY());
                    if (d > 70 * 70)
                    {
                        continue;
                    }
                    int score = d;
                    com.oddlabs.tt.player.gauntlet.Intel.WarriorType type = com.oddlabs.tt.player.gauntlet.Intel.warriorType(u_2);
                    if (((object)type == (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorType.CHICKEN))
                    {
                        score -= 60 * 60;
                    }
                    else if (((object)type == (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorType.ROCK))
                    {
                        score += 30 * 30;
                    }
                    if (score < best_score)
                    {
                        best_score = score;
                        best = u_2;
                    }
                }
                if (best != null)
                {
                    roles.put(best, com.oddlabs.tt.player.gauntlet.Military.Role.TOWER);
                    tower_assignments.put(best, tower);
                    int[] front = (ai.strategy().tower_front_entry ? frontCell(tower) : (int[])(null));
                    if (front != null)
                    {
                        front_entry.put(best, new float[] { front[0], front[1], ai.time() });
                        ai.landscapeOrder(com.oddlabs.tt.model.Selectable.newArray(best), front[0], front[1], com.oddlabs.tt.model.Action.MOVE, false);
                        ai.aiLog().count("tower_front_entry");
                    }
                    else
                    {
                        ai.owner().setTarget(com.oddlabs.tt.model.Selectable.newArray(best), tower, com.oddlabs.tt.model.Action.DEFAULT, false);
                    }
                }
            }
            bool chickens = ai.strategy().chicken_gunners;
            if (threat_level > 0 && !chickens)
            {
                return;
            }
            foreach (com.oddlabs.tt.model.Building tower_2 in java.lang.JIter.of(intel.towers))
            {
                if (!com.oddlabs.tt.player.gauntlet.Intel.isTowerManned(tower_2))
                {
                    continue;
                }
                com.oddlabs.tt.model.Unit inside = com.oddlabs.tt.player.gauntlet.Intel.gunner(tower_2);
                if (inside == null || ((object)com.oddlabs.tt.player.gauntlet.Intel.warriorType(inside) == (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorType.CHICKEN))
                {
                    continue;
                }
                if (enemyStrengthNear(tower_2.getGridX(), tower_2.getGridY(), (chickens ? 20 : 30)) > 0)
                {
                    continue;
                }
                foreach (java.util.Map.Entry e_3 in java.lang.JIter.of(roles.entrySet()))
                {
                    com.oddlabs.tt.model.Unit u_3 = ((com.oddlabs.tt.model.Unit)e_3.getKey());
                    if (((object)((com.oddlabs.tt.player.gauntlet.Military.Role)e_3.getValue()) == (object)com.oddlabs.tt.player.gauntlet.Military.Role.ARMY) && ((object)com.oddlabs.tt.player.gauntlet.Intel.warriorType(u_3) == (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorType.CHICKEN) && ((object)((com.oddlabs.tt.player.gauntlet.Intel.WarriorState)intel.warrior_states.get(u_3)) == (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorState.IDLE) && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(u_3.getGridX(), u_3.getGridY(), tower_2.getGridX(), tower_2.getGridY()) < 50 * 50)
                    {
                        ai.owner().exitTower(tower_2);
                        return;
                    }
                }
            }
        }
        // Java: 1703
        public void holdStaging()
        {
            bool escorting = ((object)mode == (object)com.oddlabs.tt.player.gauntlet.Military.Mode.HOME) && (float)(ai.time() - escort_time) < 3.0f;
            int x = (escorting ? escort_x : staging_x);
            int y = (escorting ? escort_y : staging_y);
            foreach (java.util.Map.Entry e in java.lang.JIter.of(roles.entrySet()))
            {
                if (((object)((com.oddlabs.tt.player.gauntlet.Military.Role)e.getValue()) != (object)com.oddlabs.tt.player.gauntlet.Military.Role.ARMY))
                {
                    continue;
                }
                com.oddlabs.tt.model.Unit u = ((com.oddlabs.tt.model.Unit)e.getKey());
                if (threat_level > 0)
                {
                    continue;
                }
                com.oddlabs.tt.player.gauntlet.Intel.WarriorState s = ((com.oddlabs.tt.player.gauntlet.Intel.WarriorState)ai.intel().warrior_states.get(u));
                if (((object)s != (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorState.IDLE))
                {
                    continue;
                }
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(u.getGridX(), u.getGridY(), x, y) > 7 * 7)
                {
                    attackGround(u, x, y, false);
                }
            }
        }
        // Java: 1725
        public bool canEscort()
        {
            return ((object)mode == (object)com.oddlabs.tt.player.gauntlet.Military.Mode.HOME) && threat_level <= ai.strategy().forward_threat && armyStrength() >= java.lang.Math.max(12.0f, (float)(ai.strategy().forward_ratio * enemyFieldStrength()));
        }
        // Java: 1731
        public void escort(int x, int y)
        {
            escort_x = x;
            escort_y = y;
            escort_time = ai.time();
        }
        // Java: 1738
        public bool escortArrived(int x, int y)
        {
            int n = 0;
            int near = 0;
            foreach (java.util.Map.Entry e in java.lang.JIter.of(roles.entrySet()))
            {
                if (((object)((com.oddlabs.tt.player.gauntlet.Military.Role)e.getValue()) != (object)com.oddlabs.tt.player.gauntlet.Military.Role.ARMY))
                {
                    continue;
                }
                n++;
                com.oddlabs.tt.model.Unit u = ((com.oddlabs.tt.model.Unit)e.getKey());
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(u.getGridX(), u.getGridY(), x, y) <= 14 * 14)
                {
                    near++;
                }
            }
            return n > 0 && near * 10 >= n * 6;
        }
        // Java: 1756
        public int[] forwardTarget()
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            com.oddlabs.tt.player.gauntlet.DistanceField ours = ai.planner().getStartField();
            int[] best = null;
            int best_count = 3;
            foreach (com.oddlabs.tt.model.Unit p in java.lang.JIter.of(intel.enemy_peons))
            {
                int px = p.getGridX();
                int py = p.getGridY();
                bool at_iron = false;
                foreach (com.oddlabs.tt.model.IronSupply s in java.lang.JIter.of(ai.map().getIron()))
                {
                    if (!s.isEmpty() && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(px, py, s.getGridX(), s.getGridY()) <= 3 * 3)
                    {
                        at_iron = true;
                        break;
                    }
                }
                if (!at_iron || ours.getAround(px, py, 2) == com.oddlabs.tt.player.gauntlet.DistanceField.UNREACHABLE)
                {
                    continue;
                }
                bool guarded_v = false;
                foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(intel.enemy_towers))
                {
                    guarded_v |= com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(t.getGridX(), t.getGridY(), px, py) <= 16 * 16;
                }
                foreach (com.oddlabs.tt.model.Building a in java.lang.JIter.of(intel.enemy_armories))
                {
                    guarded_v |= com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(a.getGridX(), a.getGridY(), px, py) <= 20 * 20;
                }
                if (guarded_v)
                {
                    continue;
                }
                int count = com.oddlabs.tt.player.gauntlet.Combat.countNear(intel.enemy_peons, px, py, 8);
                if (count > best_count)
                {
                    best_count = count;
                    best = new int[] { px, py };
                }
            }
            return best;
        }
        // Java: 1789
        public float attackStrength()
        {
            float s = 0.0f;
            foreach (java.util.Map.Entry e in java.lang.JIter.of(roles.entrySet()))
            {
                if (((object)((com.oddlabs.tt.player.gauntlet.Military.Role)e.getValue()) == (object)com.oddlabs.tt.player.gauntlet.Military.Role.ATTACK))
                {
                    s = (float)(s + com.oddlabs.tt.player.gauntlet.Combat.@value(((com.oddlabs.tt.model.Unit)e.getKey())));
                }
            }
            return s;
        }
        // Java: 1798
        public float defenseAt(int x, int y)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            float s = (float)(1.1f * ((ai.enemiesAlive() > 1 ? enemyFieldStrengthNear(x, y, ai.strategy().defense_radius) : enemyFieldStrength())));
            s = withEnemyTowers(s, x, y, 22);
            s = (float)(s + (float)(0.5f * com.oddlabs.tt.player.gauntlet.Combat.strengthNear(intel.enemy_peons, x, y, 40)));
            if (ai.strategy().hidden_info)
            {
                foreach (com.oddlabs.tt.model.Building a in java.lang.JIter.of(intel.enemy_armories))
                {
                    if (!a.isDead() && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(a.getGridX(), a.getGridY(), x, y) <= 40 * 40)
                    {
                        s = (float)(s + com.oddlabs.tt.player.gauntlet.Military.stockStrength(a));
                    }
                }
            }
            return s;
        }
        // Java: 1821
        public float defenseFor(com.oddlabs.tt.model.Selectable t)
        {
            int x = t.getGridX();
            int y = t.getGridY();
            if (!ai.strategy().gate_owner || ai.enemiesAlive() <= 1)
            {
                return defenseAt(x, y);
            }
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            com.oddlabs.tt.player.Player owner = t.getOwner();
            int r = ai.strategy().defense_radius;
            int r2 = r * r;
            float s = 0.0f;
            foreach (java.util.List group in java.lang.JIter.of(java.util.List_.of(intel.enemy_warriors, intel.enemy_chieftains)))
            {
                foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(group))
                {
                    if (u.isDead())
                    {
                        continue;
                    }
                    bool near = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, u.getGridX(), u.getGridY()) <= r2;
                    if (((object)u.getOwner() == (object)owner))
                    {
                        s = (float)(s + (float)(com.oddlabs.tt.player.gauntlet.Combat.@value(u) * ((near ? 1.0f : 0.5f))));
                    }
                    else if (near)
                    {
                        s = (float)(s + com.oddlabs.tt.player.gauntlet.Combat.@value(u));
                    }
                }
            }
            s = (float)(s * 1.1f);
            s = withEnemyTowers(s, x, y, 22);
            s = (float)(s + (float)(0.5f * com.oddlabs.tt.player.gauntlet.Combat.strengthNear(intel.enemy_peons, x, y, 40)));
            return s;
        }
        // Java: 1848
        public bool homeless(com.oddlabs.tt.player.Player p)
        {
            foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(ai.intel().enemy_buildings))
            {
                if (b.isDead() || ((object)b.getOwner() != (object)p))
                {
                    continue;
                }
                int id = ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID();
                if (b.isComplete() && (id == com.oddlabs.tt.model.Race.BUILDING_QUARTERS || id == com.oddlabs.tt.model.Race.BUILDING_ARMORY))
                {
                    return false;
                }
            }
            return true;
        }
        // Java: 1865
        public com.oddlabs.tt.model.Selectable finishTarget(int from_x, int from_y)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            int range2 = ai.strategy().finish_range * ai.strategy().finish_range;
            com.oddlabs.tt.model.Selectable best = null;
            float best_score = java.lang.Float.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(intel.enemy_buildings))
            {
                if (b.isDead() || b.isComplete() || stalled_targets.containsKey(b) || inDeadRegion(b.getGridX(), b.getGridY()))
                {
                    continue;
                }
                int id = ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID();
                if (id != com.oddlabs.tt.model.Race.BUILDING_QUARTERS && id != com.oddlabs.tt.model.Race.BUILDING_ARMORY)
                {
                    continue;
                }
                int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(from_x, from_y, b.getGridX(), b.getGridY());
                if (d > range2 || !homeless(b.getOwner()))
                {
                    continue;
                }
                if (d < best_score)
                {
                    best_score = d;
                    best = b;
                }
            }
            if (best != null)
            {
                return best;
            }
            com.oddlabs.tt.player.gauntlet.Strategy s = ai.strategy();
            bool lean = s.finish_skip_out || s.finish_units >= 0 || s.finish_ratio > 0.0f;
            if (lean)
            {
                countBases();
            }
            float @ref = (((object)mode == (object)com.oddlabs.tt.player.gauntlet.Military.Mode.ATTACK) ? attackStrength() : armyStrength());
            java.util.Map skip = new java.util.LinkedHashMap();
            foreach (java.util.List group in java.lang.JIter.of(java.util.List_.of(intel.enemy_chieftains, intel.enemy_warriors, intel.enemy_peons)))
            {
                foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(group))
                {
                    if (u.isDead() || stalled_targets.containsKey(u) || inDeadRegion(u.getGridX(), u.getGridY()))
                    {
                        continue;
                    }
                    int d_2 = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(from_x, from_y, u.getGridX(), u.getGridY());
                    if (d_2 > range2 || !homeless(u.getOwner()))
                    {
                        continue;
                    }
                    com.oddlabs.tt.player.Player p = u.getOwner();
                    if (lean && ((bool)((bool?)skip.computeIfAbsent(p, new java.util.function.Function_L((object k_l) =>
                    {
                        com.oddlabs.tt.player.Player k = (com.oddlabs.tt.player.Player)k_l;
                        return leanSkip(k, from_x, from_y, @ref);
                    })))))
                    {
                        continue;
                    }
                    if (s.finish_units >= 0 && ((object)group != (object)intel.enemy_chieftains) && p.getUnitCountContainer().getNumSupplies() - ((p.hasActiveChieftain() ? 1 : 0)) <= s.finish_units)
                    {
                        continue;
                    }
                    if (s.finish_ratio > 0.0f && guarded(u, @ref))
                    {
                        continue;
                    }
                    float score = (((object)group == (object)intel.enemy_chieftains) ? (float)(d_2 / 4.0f) : d_2);
                    if (score < best_score)
                    {
                        best_score = score;
                        best = u;
                    }
                }
            }
            return best;
        }
        // Java: 1919
        public bool leanSkip(com.oddlabs.tt.player.Player p, int fx, int fy, float @ref)
        {
            com.oddlabs.tt.player.gauntlet.Strategy s = ai.strategy();
            int[] c = ((int[])qa_counts.getOrDefault(p, new int[2]));
            if (s.finish_skip_out && p.getUnitCountContainer().getNumSupplies() <= 8 && !p.hasActiveChieftain() && c[1] == 0)
            {
                ai.aiLog().count("finish_skip_out");
                return true;
            }
            if (s.finish_ratio > 0.0f)
            {
                int r2 = s.finish_range * s.finish_range;
                float remnant = 0.0f;
                foreach (java.util.List g in java.lang.JIter.of(java.util.List_.of(ai.intel().enemy_warriors, ai.intel().enemy_chieftains)))
                {
                    foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(g))
                    {
                        if (!u.isDead() && ((object)u.getOwner() == (object)p) && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(fx, fy, u.getGridX(), u.getGridY()) <= r2)
                        {
                            remnant = (float)(remnant + com.oddlabs.tt.player.gauntlet.Combat.@value(u));
                        }
                    }
                }
                if (remnant > (float)(s.finish_ratio * @ref))
                {
                    ai.aiLog().count("finish_skip_big");
                    return true;
                }
            }
            return false;
        }
        // Java: 1943
        public bool guarded(com.oddlabs.tt.model.Unit t, float @ref)
        {
            float others = 0.0f;
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(ai.intel().enemy_warriors))
            {
                if (!e.isDead() && ((object)e.getOwner() != (object)t.getOwner()) && !((e.getPrimaryController() is com.oddlabs.tt.model.behaviour.IdleController)) && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(e.getGridX(), e.getGridY(), t.getGridX(), t.getGridY()) <= 25 * 25)
                {
                    others = (float)(others + com.oddlabs.tt.player.gauntlet.Combat.@value(e));
                }
            }
            if (others > (float)(0.3f * @ref))
            {
                ai.aiLog().count("finish_skip_guarded");
                return true;
            }
            return false;
        }
        // Java: 1957
        public com.oddlabs.tt.model.Selectable chooseTarget(int from_x, int from_y)
        {
            return chooseTarget(from_x, from_y, null);
        }
        // Java: 1962
        public com.oddlabs.tt.model.Selectable retarget(int[] c)
        {
            if (!ai.strategy().target_path)
            {
                return chooseTarget(c[0], c[1]);
            }
            return chooseTarget(c[0], c[1], ai.map().computeField(c[0], c[1], 1400));
        }
        // Java: 1969
        public com.oddlabs.tt.model.Selectable chooseTarget(int from_x, int from_y, com.oddlabs.tt.player.gauntlet.DistanceField path)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            com.oddlabs.tt.player.gauntlet.Strategy strategy = ai.strategy();
            if (focus_owner != null && !focus_owner.isAlive())
            {
                focus_owner = null;
            }
            if (focus_owner != null && strategy.focus_finish)
            {
                com.oddlabs.tt.model.Unit prey = focusRemnant(focus_owner, from_x, from_y);
                if (prey != null)
                {
                    return prey;
                }
            }
            if (strategy.finish_copies && ai.enemiesAlive() > 1)
            {
                com.oddlabs.tt.model.Selectable finish = finishTarget(from_x, from_y);
                if (finish != null)
                {
                    ai.aiLog().count("finish_target");
                    return finish;
                }
            }
            com.oddlabs.tt.model.Selectable best = null;
            float best_score = java.lang.Float.MAX_VALUE;
            java.util.List quartered = new java.util.ArrayList();
            if (strategy.gate_freeze)
            {
                foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(intel.enemy_buildings))
                {
                    if (!b.isDead() && ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID() == com.oddlabs.tt.model.Race.BUILDING_QUARTERS && !quartered.contains(b.getOwner()))
                    {
                        quartered.add(b.getOwner());
                    }
                }
            }
            bool skip_frozen = strategy.gate_freeze && !quartered.isEmpty();
            java.util.Map base_threat = new java.util.LinkedHashMap();
            if (strategy.target_threat_weight > 0.0f)
            {
                foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(threats))
                {
                    if (!u.isDead() && !u.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD))
                    {
                        base_threat.merge(u.getOwner(), com.oddlabs.tt.player.gauntlet.Combat.@value(u), new java.util.function.BiFunction_L((object a0, object a1) => { return java.lang.Float.sum(((float)a0), ((float)a1)); }));
                    }
                }
            }
            stalled_targets.entrySet().removeIf(new java.util.function.Predicate_L((object e_l) =>
            {
                java.util.Map.Entry e = (java.util.Map.Entry)e_l;
                return ((com.oddlabs.tt.model.Selectable)e.getKey()).isDead() || (float)(ai.time() - ((float)((float?)e.getValue()))) > com.oddlabs.tt.player.gauntlet.Military.STALL_MEMORY;
            }));
            while (!dead_region_times.isEmpty() && (float)(ai.time() - ((float)((float?)dead_region_times.getFirst()))) > com.oddlabs.tt.player.gauntlet.Military.STALL_MEMORY)
            {
                dead_regions.removeFirst();
                dead_region_times.removeFirst();
            }
            java.util.List candidates = new java.util.ArrayList(intel.enemy_armories);
            candidates.addAll(intel.enemy_quarters);
            candidates.addAll(intel.enemy_towers);
            candidates.removeIf(new java.util.function.Predicate_L((object b_2_l) =>
            {
                com.oddlabs.tt.model.Building b_2 = (com.oddlabs.tt.model.Building)b_2_l;
                return stalled_targets.containsKey(b_2) || inDeadRegion(b_2.getGridX(), b_2.getGridY());
            }));
            if (strategy.gate_freeze)
            {
                foreach (com.oddlabs.tt.model.Building b_3 in java.lang.JIter.of(intel.enemy_buildings))
                {
                    if (!b_3.isComplete() && ((com.oddlabs.tt.model.BuildingTemplate)b_3.getTemplate()).getTemplateID() == com.oddlabs.tt.model.Race.BUILDING_QUARTERS)
                    {
                        candidates.add(b_3);
                    }
                }
            }
            if (candidates.isEmpty())
            {
                candidates.addAll(intel.enemy_buildings);
                candidates.removeIf(new java.util.function.Predicate_L((object b_4_l) =>
                {
                    com.oddlabs.tt.model.Building b_4 = (com.oddlabs.tt.model.Building)b_4_l;
                    return stalled_targets.containsKey(b_4) || inDeadRegion(b_4.getGridX(), b_4.getGridY());
                }));
            }
            com.oddlabs.tt.model.Building best_line = null;
            float best_line_score = java.lang.Float.MAX_VALUE;
            com.oddlabs.tt.model.Building best_frozen = null;
            float best_frozen_score = java.lang.Float.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Building b_5 in java.lang.JIter.of(candidates))
            {
                if (b_5.isDead() || (skip_frozen && !quartered.contains(b_5.getOwner())) || ai.freeze().isFrozenSite(b_5))
                {
                    continue;
                }
                float line = com.oddlabs.tt.player.gauntlet.MapAnalysis.meters(from_x, from_y, b_5.getGridX(), b_5.getGridY());
                float d = line;
                if (path != null)
                {
                    int walk = path.getAround(b_5.getGridX(), b_5.getGridY(), 4);
                    if (walk == com.oddlabs.tt.player.gauntlet.DistanceField.UNREACHABLE)
                    {
                        continue;
                    }
                    d = walk;
                }
                float priority = ((((com.oddlabs.tt.model.BuildingTemplate)b_5.getTemplate()).getTemplateID()) switch { 1 => (float)((strategy.quarters_first || strategy.gate_freeze ? 60.0f : 0.0f)), 0 => (float)((strategy.quarters_first || strategy.gate_freeze ? 0.0f : 60.0f)), _ => (float)(120.0f) });
                float defense = (float)(strategy.target_defense_weight * defenseFor(b_5));
                if (ai.freeze().isFrozen(b_5.getOwner()))
                {
                    priority = 0.0f;
                    defense = 0.0f;
                }
                float score = (float)((float)(d + priority) + defense);
                if (strategy.target_home_weight > 0.0f)
                {
                    score = (float)(score + (float)(strategy.target_home_weight * com.oddlabs.tt.player.gauntlet.MapAnalysis.meters(staging_x, staging_y, b_5.getGridX(), b_5.getGridY())));
                }
                if (focus_owner != null && ((object)b_5.getOwner() == (object)focus_owner))
                {
                    score = (float)(score - strategy.focus_bonus);
                }
                score = (float)(score - (float)(strategy.target_threat_weight * ((float)((float?)base_threat.getOrDefault(b_5.getOwner(), 0.0f)))));
                if (explain != null)
                {
                    explain.add(java.lang.JString.format("%07.1f %s %s at %d,%d: d %.0f + pri %.0f + def %.0f", score, b_5.getOwner().getPlayerInfo().getName(), com.oddlabs.tt.player.gauntlet.Intel.kind(b_5), b_5.getGridX(), b_5.getGridY(), d, priority, defense));
                }
                if (strategy.frozen_last && ai.freeze().isFrozen(b_5.getOwner()))
                {
                    if (score < best_frozen_score)
                    {
                        best_frozen_score = score;
                        best_frozen = b_5;
                    }
                    continue;
                }
                if (score < best_score)
                {
                    best_score = score;
                    best = b_5;
                }
                if (path != null && (float)((float)(score - d) + line) < best_line_score)
                {
                    best_line_score = (float)((float)(score - d) + line);
                    best_line = b_5;
                }
            }
            if (path != null && ((object)best != (object)best_line))
            {
                ai.aiLog().count("retarget_path_changed");
            }
            if (best_frozen != null)
            {
                if (best == null)
                {
                    best = best_frozen;
                    ai.aiLog().count("frozen_last_target");
                }
                else if (!frozen_deferred.contains(best_frozen.getOwner()))
                {
                    frozen_deferred.add(best_frozen.getOwner());
                    ai.aiLog().count("frozen_deferred");
                }
            }
            return (best != null ? best : (com.oddlabs.tt.model.Selectable)(nearestEnemyUnit(from_x, from_y)));
        }
        // Java: 2094
        public com.oddlabs.tt.model.Unit focusRemnant(com.oddlabs.tt.player.Player focus, int from_x, int from_y)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(intel.enemy_buildings))
            {
                if (!b.isDead() && ((object)b.getOwner() == (object)focus))
                {
                    return null;
                }
            }
            com.oddlabs.tt.model.Unit prey = null;
            int best_d = java.lang.Integer.MAX_VALUE;
            foreach (java.util.List group in java.lang.JIter.of(java.util.List_.of(intel.enemy_peons, intel.enemy_warriors, intel.enemy_chieftains)))
            {
                foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(group))
                {
                    if (u.isDead() || ((object)u.getOwner() != (object)focus))
                    {
                        continue;
                    }
                    int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(from_x, from_y, u.getGridX(), u.getGridY());
                    if (d < best_d)
                    {
                        best_d = d;
                        prey = u;
                    }
                }
            }
            return (prey != null && best_d <= 90 * 90 ? prey : (com.oddlabs.tt.model.Unit)(null));
        }
        // Java: 2115
        public com.oddlabs.tt.model.Unit nearestEnemyUnit(int from_x, int from_y)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            com.oddlabs.tt.model.Unit best = null;
            int best_d = java.lang.Integer.MAX_VALUE;
            foreach (java.util.List group in java.lang.JIter.of(java.util.List_.of(intel.enemy_peons, intel.enemy_warriors, intel.enemy_chieftains)))
            {
                foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(group))
                {
                    if (u.isDead() || stalled_targets.containsKey(u) || inDeadRegion(u.getGridX(), u.getGridY()))
                    {
                        continue;
                    }
                    int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(from_x, from_y, u.getGridX(), u.getGridY());
                    if (d < best_d)
                    {
                        best_d = d;
                        best = u;
                    }
                }
            }
            return best;
        }
        // Java: 2136
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
        // Java: 2145
        public void recordEnemyStrength()
        {
            enemy_history.addLast(new float[] { ai.time(), enemyFieldStrength() });
            while (enemy_history.size() > 2 && (float)(ai.time() - ((float[])enemy_history.getFirst())[0]) > 90.0f)
            {
                enemy_history.removeFirst();
            }
        }
        // Java: 2151
        public void considerAttack()
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            com.oddlabs.tt.player.gauntlet.Strategy strategy = ai.strategy();
            if (ai.logging())
            {
                explain = new java.util.ArrayList();
            }
            com.oddlabs.tt.model.Selectable t = chooseTarget(staging_x, staging_y);
            java.util.List why = explain;
            explain = null;
            if (t == null)
            {
                return;
            }
            float army = armyStrength();
            float potential = (float)(army + stockStrength());
            float defense = defenseFor(t);
            if (strategy.project_defense)
            {
                int d = ai.planner().getEnemyField().get(staging_x, staging_y);
                float march = (d == com.oddlabs.tt.player.gauntlet.DistanceField.UNREACHABLE ? 120.0f : (float)(d / 3.0f));
                defense = (float)(defense + (float)(java.lang.Math.max(0.0f, enemyGrowthPerSecond()) * march));
            }
            bool chief = intel.chieftain != null && intel.chieftain.getHitPoints() > 30 && !(strategy.shred && strategy.shred_strict);
            bool enemy_chief = false;
            int cr2 = strategy.defense_radius * strategy.defense_radius;
            foreach (com.oddlabs.tt.model.Unit c in java.lang.JIter.of(intel.enemy_chieftains))
            {
                enemy_chief |= !c.isDead() && c.getHitPoints() > 15 && (!strategy.gate_owner || ai.enemiesAlive() <= 1 || ((object)c.getOwner() == (object)t.getOwner()) || com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(c.getGridX(), c.getGridY(), t.getGridX(), t.getGridY()) <= cr2);
            }
            float bonus = (chief && !enemy_chief ? 1.4f : (chief ? 1.05f : (enemy_chief ? 0.7f : 1.0f)));
            bool capped = nearUnitCap();
            float caution = attack_caution;
            bool go = potential >= strategy.attack_min_strength && (float)(potential * bonus) >= (float)((float)(strategy.attack_ratio * caution) * defense);
            go |= potential >= (float)(strategy.attack_max_strength * caution) && (float)(potential * bonus) >= (float)((float)(0.8f * caution) * defense);
            bool capped_go = capped && (float)(potential * bonus) >= (float)((float)(strategy.capped_ratio * caution) * defense);
            if (capped_go && potential < strategy.capped_min_strength)
            {
                capped_go = false;
                if (!go)
                {
                    ai.aiLog().count("capped_min_blocked");
                }
            }
            go |= capped_go;
            if (!go || ai.time() < next_wave_time)
            {
                return;
            }
            target = t;
            mode = com.oddlabs.tt.player.gauntlet.Military.Mode.MUSTER;
            muster_start = ai.time();
            if (why != null && !why.isEmpty())
            {
                why.sort(null);
                ai.log("muster candidates from " + (staging_x) + "," + (staging_y) + ": " + java.lang.JStr.valueOf(java.lang.JString.join("; ", why.subList(0, java.lang.Math.min(4, why.size())))));
            }
            ai.log(java.lang.JString.format("muster: army %.1f + stock %.1f vs defense %.1f at %d,%d", army, (float)(potential - army), defense, t.getGridX(), t.getGridY()));
        }
        // Java: 2212
        public void considerStrike()
        {
            com.oddlabs.tt.model.Building b = intruder();
            if (b == null)
            {
                return;
            }
            int bx = b.getGridX();
            int by = b.getGridY();
            float defense = withEnemyTowers((float)(1.1f * enemyFightersNear(bx, by, 24)), bx, by, 12);
            float army = armyStrength();
            if (army < java.lang.Math.max(10.0f, (float)(2.0f * defense)))
            {
                return;
            }
            target = b;
            strike = true;
            ai.log(java.lang.JString.format("strike on %s's %s at %d,%d: army %.1f vs %.1f", b.getOwner().getPlayerInfo().getName(), com.oddlabs.tt.player.gauntlet.Intel.kind(b), bx, by, army, defense));
            launchAttack();
        }
        // Java: 2231
        public com.oddlabs.tt.model.Building intruder()
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            java.util.List own = intel.finishedBuildings();
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
                    d = java.lang.Math.min(d, com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(o.getGridX(), o.getGridY(), b.getGridX(), b.getGridY()));
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
        // Java: 2253
        public bool gatherersNear(int x, int y, int radius)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            foreach (com.oddlabs.tt.model.Unit p in java.lang.JIter.of(intel.peons))
            {
                com.oddlabs.tt.player.gauntlet.Intel.PeonState s = ((com.oddlabs.tt.player.gauntlet.Intel.PeonState)intel.peon_states.get(p));
                if ((((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_TREE) || ((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_ROCK) || ((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_IRON)) && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, p.getGridX(), p.getGridY()) <= radius * radius)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 2264
        public float stockStrength()
        {
            com.oddlabs.tt.model.Building armory = ai.intel().armory();
            return (armory == null ? 0.0f : com.oddlabs.tt.player.gauntlet.Military.stockStrength(armory));
        }
        // Java: 2270
        public static float stockStrength(com.oddlabs.tt.model.Building armory)
        {
            int workers = armory.getUnitContainer().getNumSupplies();
            int iron = armory.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)).getNumSupplies();
            int chicken = armory.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)).getNumSupplies();
            int rock = armory.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)).getNumSupplies();
            float s = 0.0f;
            int left = workers;
            int c = java.lang.Math.min(chicken, left);
            s = (float)(s + (float)(c * com.oddlabs.tt.player.gauntlet.Combat.CHICKEN));
            left -= c;
            int i = java.lang.Math.min(iron, left);
            s = (float)(s + (float)(i * com.oddlabs.tt.player.gauntlet.Combat.IRON));
            left -= i;
            s = (float)(s + (float)(java.lang.Math.min(rock, left) * com.oddlabs.tt.player.gauntlet.Combat.ROCK));
            return s;
        }
        // Java: 2288
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
                if (((object)((com.oddlabs.tt.player.gauntlet.Military.Role)e.getValue()) != (object)com.oddlabs.tt.player.gauntlet.Military.Role.ARMY))
                {
                    continue;
                }
                total++;
                com.oddlabs.tt.model.Unit u = ((com.oddlabs.tt.model.Unit)e.getKey());
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(u.getGridX(), u.getGridY(), staging_x, staging_y) <= 12 * 12)
                {
                    near++;
                }
            }
            bool gathered = total > 0 && near >= total * 8 / 10;
            bool timeout = (float)(ai.time() - muster_start) > 45.0f;
            if (ai.strategy().launch_recheck && timeout && !gathered && threat_level >= 2 && base_threat_strength > (float)(ai.strategy().attack_threat_ratio * ((float)(stagingStrength(30) + stockStrength()))))
            {
                mode = com.oddlabs.tt.player.gauntlet.Military.Mode.HOME;
                next_wave_time = (float)(ai.time() + 60.0f);
                ai.log(java.lang.JString.format("muster dropped: %.1f in the base", base_threat_strength));
                ai.aiLog().count("muster_dropped");
                return;
            }
            if ((!stock_left && pending == 0 && gathered) || timeout)
            {
                launchAttack();
            }
        }
        // Java: 2323
        public void launchAttack()
        {
            com.oddlabs.tt.model.Selectable t = (target != null && !target.isDead() ? target : chooseTarget(staging_x, staging_y));
            if (t == null)
            {
                mode = com.oddlabs.tt.player.gauntlet.Military.Mode.HOME;
                return;
            }
            setTarget(t);
            float s = 0.0f;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(beyondGuard(withRole(com.oddlabs.tt.player.gauntlet.Military.Role.ARMY))))
            {
                roles.put(u, com.oddlabs.tt.player.gauntlet.Military.Role.ATTACK);
                s = (float)(s + com.oddlabs.tt.player.gauntlet.Combat.@value(u));
            }
            attack_initial_strength = s;
            worn_peak = s;
            worn_history.clear();
            worn_skip_counted = false;
            split_guard_counted = false;
            attack_kills_start = ai.owner().getUnitsKilled();
            attack_losses_start = ai.owner().getUnitsLost();
            attack_running = true;
            last_progress_time = ai.time();
            best_target_dist = java.lang.Integer.MAX_VALUE;
            mode = (s > 0 ? com.oddlabs.tt.player.gauntlet.Military.Mode.ATTACK : com.oddlabs.tt.player.gauntlet.Military.Mode.HOME);
            string what = java.lang.JStr.valueOf((((t is com.oddlabs.tt.model.Building) ? "building " : "unit "))) + java.lang.JStr.valueOf(t.getTemplate().GetType().getSimpleName()) + " of " + java.lang.JStr.valueOf(t.getOwner().getPlayerInfo().getName());
            ai.log(java.lang.JString.format("attack with %.1f on %s at %d,%d", s, what, t.getGridX(), t.getGridY()));
            if (((object)mode == (object)com.oddlabs.tt.player.gauntlet.Military.Mode.ATTACK))
            {
                recruitSappers(t.getGridX(), t.getGridY());
            }
        }
        // Java: 2356
        public void recruitSappers(int x, int y)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            if (!ai.strategy().sappers || !intel.sappers.isEmpty())
            {
                return;
            }
            int towers = 0;
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(intel.enemy_towers))
            {
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(t.getGridX(), t.getGridY(), x, y) <= 30 * 30)
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
                com.oddlabs.tt.player.gauntlet.Intel.PeonState s = ((com.oddlabs.tt.player.gauntlet.Intel.PeonState)intel.peon_states.get(p));
                if ((((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.IDLE) || ((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_TREE) || ((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_ROCK) || ((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_IRON) || ((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.MOVE)) && !ai.economy().reservedPlacer(p))
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
                return java.lang.Integer.compare(com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(a.getGridX(), a.getGridY(), staging_x, staging_y), com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(b.getGridX(), b.getGridY(), staging_x, staging_y));
            }));
            foreach (com.oddlabs.tt.model.Unit p_2 in java.lang.JIter.of(pool.subList(0, want)))
            {
                intel.sappers.add(p_2);
                intel.peon_states.put(p_2, com.oddlabs.tt.player.gauntlet.Intel.PeonState.SAPPER);
            }
            ai.log((want) + " peons go along to pull down " + (towers) + " towers");
        }
        // Java: 2389
        public void sapperTick()
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            intel.sappers.removeIf(new java.util.function.Predicate_L((object a0) => { return ((com.oddlabs.tt.model.Unit)a0).isDead(); }));
            sapper_orders.keySet().removeIf(new java.util.function.Predicate_L((object a0) => { return ((com.oddlabs.tt.model.Unit)a0).isDead(); }));
            if (intel.sappers.isEmpty())
            {
                return;
            }
            int[] c = (((object)mode == (object)com.oddlabs.tt.player.gauntlet.Military.Mode.ATTACK) ? attackCenter() : (int[])(null));
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
                if (((object)((com.oddlabs.tt.player.gauntlet.Military.Role)e.getValue()) == (object)com.oddlabs.tt.player.gauntlet.Military.Role.ATTACK) && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(((com.oddlabs.tt.model.Unit)e.getKey()).getGridX(), ((com.oddlabs.tt.model.Unit)e.getKey()).getGridY(), c[0], c[1]) <= 20 * 20)
                {
                    ours = (float)(ours + com.oddlabs.tt.player.gauntlet.Combat.@value(((com.oddlabs.tt.model.Unit)e.getKey())));
                }
            }
            int[] back = com.oddlabs.tt.player.gauntlet.MapAnalysis.towards(c[0], c[1], staging_x, staging_y, 6);
            creepTick(c, ours);
            com.oddlabs.tt.model.Building site = creep_site;
            foreach (com.oddlabs.tt.model.Unit p_2 in java.lang.JIter.of(intel.sappers))
            {
                com.oddlabs.tt.model.Building tower = null;
                int best = 18 * 18;
                foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(intel.enemy_towers))
                {
                    int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(t.getGridX(), t.getGridY(), c[0], c[1]);
                    if (d > best)
                    {
                        continue;
                    }
                    bool quiet = !com.oddlabs.tt.player.gauntlet.Intel.isTowerActive(t) || (enemyStrengthNear(t.getGridX(), t.getGridY(), 10) < (float)(0.5f * ours) && ours >= 8.0f);
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
                else if (site == null && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(p_2.getGridX(), p_2.getGridY(), back[0], back[1]) > 6 * 6)
                {
                    move(p_2, back[0], back[1]);
                }
            }
        }
        // Java: 2447
        public int creepTowerCount()
        {
            creep_towers.removeIf(new java.util.function.Predicate_L((object a0) => { return ((com.oddlabs.tt.model.Building)a0).isDead(); }));
            return creep_towers.size() + ((creep_site != null && !creep_site.isDead() ? 1 : 0));
        }
        // Java: 2456
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
                if (com.oddlabs.tt.player.gauntlet.Intel.isTowerManned(t) || tower_assignments.containsValue(t))
                {
                    continue;
                }
                com.oddlabs.tt.model.Unit best = null;
                int best_d = 30 * 30;
                foreach (java.util.Map.Entry e in java.lang.JIter.of(roles.entrySet()))
                {
                    com.oddlabs.tt.model.Unit u = ((com.oddlabs.tt.model.Unit)e.getKey());
                    if (((object)((com.oddlabs.tt.player.gauntlet.Military.Role)e.getValue()) != (object)com.oddlabs.tt.player.gauntlet.Military.Role.ATTACK) || ((object)((com.oddlabs.tt.player.gauntlet.Intel.WarriorState)ai.intel().warrior_states.get(u)) == (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorState.STUNNED))
                    {
                        continue;
                    }
                    int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(u.getGridX(), u.getGridY(), t.getGridX(), t.getGridY());
                    if (((object)com.oddlabs.tt.player.gauntlet.Intel.warriorType(u) == (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorType.CHICKEN))
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
                    roles.put(best, com.oddlabs.tt.player.gauntlet.Military.Role.TOWER);
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
            if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(c[0], c[1], gx, gy) > 30 * 30 || ours < (float)((float)(1.2f * enemyStrengthNear(gx, gy, 20)) + 4.0f))
            {
                return;
            }
            int near = 0;
            foreach (com.oddlabs.tt.model.Building t_2 in java.lang.JIter.of(creep_towers))
            {
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(t_2.getGridX(), t_2.getGridY(), gx, gy) <= 20 * 20)
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
            com.oddlabs.tt.model.Unit placer = null;
            int best_d_2 = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Unit p in java.lang.JIter.of(ai.intel().sappers))
            {
                int d_2 = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(p.getGridX(), p.getGridY(), spot[0], spot[1]);
                if (d_2 < best_d_2)
                {
                    best_d_2 = d_2;
                    placer = p;
                }
            }
            if (placer != null)
            {
                creep_site = ai.placeSite(java.util.List_.of(placer), com.oddlabs.tt.model.Race.BUILDING_TOWER, spot[0], spot[1]);
            }
            ai.log("sappers raise a tower at " + (spot[0]) + "," + (spot[1]) + " by " + java.lang.JStr.valueOf(goal));
        }
        // Java: 2521
        public int[] creepSpot(int gx, int gy, int[] c)
        {
            com.oddlabs.tt.model.BuildingTemplate tower = ai.owner().getRace().getBuildingTemplate(com.oddlabs.tt.model.Race.BUILDING_TOWER);
            com.oddlabs.tt.player.gauntlet.MapAnalysis map = ai.map();
            int[] best = null;
            float best_score = (float)(-java.lang.Float.MAX_VALUE);
            for (int y = gy - 11; y <= gy + 11; y++)
            {
                for (int x = gx - 11; x <= gx + 11; x++)
                {
                    int d2 = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, gx, gy);
                    if (d2 < 5 * 5 || d2 > 11 * 11 || !map.canPlace(tower, x, y))
                    {
                        continue;
                    }
                    bool covered = false;
                    foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(ai.intel().enemy_towers))
                    {
                        if (com.oddlabs.tt.player.gauntlet.Intel.isTowerActive(t) && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(t.getGridX(), t.getGridY(), x, y) <= 12 * 12)
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
                    float score = (float)((float)((float)(-((float)(java.lang.Math.sqrt(d2)))) - (float)(0.3f * ((float)(java.lang.Math.sqrt(com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, c[0], c[1])))))) + java.lang.Math.min(trees, 8));
                    if (score > best_score)
                    {
                        best_score = score;
                        best = new int[] { x, y };
                    }
                }
            }
            return best;
        }
        // Java: 2555
        public void setTarget(com.oddlabs.tt.model.Selectable t)
        {
            if (ai.strategy().focus_bonus > 0.0f && ((object)t.getOwner() != (object)ai.owner()))
            {
                focus_owner = t.getOwner();
            }
            target = t;
            if (target_field == null || com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(target_x, target_y, t.getGridX(), t.getGridY()) > 8 * 8)
            {
                target_x = t.getGridX();
                target_y = t.getGridY();
                target_field = ai.map().computeField(target_x, target_y, java.lang.Integer.MAX_VALUE);
                best_target_dist = java.lang.Integer.MAX_VALUE;
                last_progress_time = ai.time();
            }
        }
        // Java: 2569
        public bool nearUnitCap()
        {
            com.oddlabs.tt.player.Player owner = ai.owner();
            return owner.getUnitCountContainer().getNumSupplies() >= owner.getWorld().getMaxUnitCount() - 10;
        }
        // Java: 2575
        public void easeCaution()
        {
            bool capped = nearUnitCap();
            if (((object)mode != (object)com.oddlabs.tt.player.gauntlet.Military.Mode.HOME) || !capped || attack_caution <= 1.0f || ai.strategy().caution_decay <= 1.0f)
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
        // Java: 2589
        public void endAttack()
        {
            if (((object)mode != (object)com.oddlabs.tt.player.gauntlet.Military.Mode.HOME))
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
            if (column_until >= 0.0f)
            {
                column_until = (float)(-1.0f);
                column_target = null;
                ai.aiLog().count("unjam_ended");
                ai.log("unjam: column march over, the attack ended");
            }
            jam_scans = 0;
            foreach (java.util.Map.Entry e in java.lang.JIter.of(roles.entrySet()))
            {
                if (((object)((com.oddlabs.tt.player.gauntlet.Military.Role)e.getValue()) == (object)com.oddlabs.tt.player.gauntlet.Military.Role.ATTACK) || ((object)((com.oddlabs.tt.player.gauntlet.Military.Role)e.getValue()) == (object)com.oddlabs.tt.player.gauntlet.Military.Role.REINFORCE))
                {
                    e.setValue(com.oddlabs.tt.player.gauntlet.Military.Role.ARMY);
                }
            }
            mode = com.oddlabs.tt.player.gauntlet.Military.Mode.HOME;
            target = null;
            next_wave_time = (float)(ai.time() + ((recalled ? ai.strategy().recall_cooldown : 20.0f)));
            recalled = false;
            strike = false;
        }
        public float engage_since;
        public bool recalled;
        // Java: 2631
        public float stagingStrength(int radius)
        {
            float s = 0.0f;
            int r2 = radius * radius;
            foreach (java.util.Map.Entry e in java.lang.JIter.of(roles.entrySet()))
            {
                if (((object)((com.oddlabs.tt.player.gauntlet.Military.Role)e.getValue()) != (object)com.oddlabs.tt.player.gauntlet.Military.Role.ARMY))
                {
                    continue;
                }
                com.oddlabs.tt.model.Unit u = ((com.oddlabs.tt.model.Unit)e.getKey());
                com.oddlabs.tt.player.gauntlet.Intel.WarriorState st = ((com.oddlabs.tt.player.gauntlet.Intel.WarriorState)ai.intel().warrior_states.get(u));
                if (u.isDead() || ((object)st == (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorState.STUNNED) || ((object)st == (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorState.ENTER))
                {
                    continue;
                }
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(u.getGridX(), u.getGridY(), staging_x, staging_y) <= r2)
                {
                    s = (float)(s + com.oddlabs.tt.player.gauntlet.Combat.@value(u));
                }
            }
            return s;
        }
        // Java: 2650
        public void attack()
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            java.util.List army = withRole(com.oddlabs.tt.player.gauntlet.Military.Role.ATTACK);
            if (army.isEmpty())
            {
                endAttack();
                return;
            }
            int[] c = com.oddlabs.tt.player.gauntlet.MapAnalysis.centroid(army);
            float ours = 0.0f;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(army))
            {
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(u.getGridX(), u.getGridY(), c[0], c[1]) <= 18 * 18)
                {
                    ours = (float)(ours + com.oddlabs.tt.player.gauntlet.Combat.@value(u));
                }
            }
            float local_enemy = enemyFightersNear(c[0], c[1], com.oddlabs.tt.player.gauntlet.Military.ENGAGE_RADIUS);
            float armed = (ai.strategy().stall_peons ? enemyStrengthNear(c[0], c[1], com.oddlabs.tt.player.gauntlet.Military.ENGAGE_RADIUS) : 0.0f);
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(intel.enemy_towers))
            {
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(t.getGridX(), t.getGridY(), c[0], c[1]) <= com.oddlabs.tt.player.gauntlet.Military.ENGAGE_RADIUS * com.oddlabs.tt.player.gauntlet.Military.ENGAGE_RADIUS)
                {
                    float v = enemyTowerValue(t);
                    local_enemy = (float)(local_enemy + v);
                    armed = (float)(armed + v);
                }
            }
            ours = (float)(ours * terrainFactor(army, intel.enemy_warriors, c[0], c[1], com.oddlabs.tt.player.gauntlet.Military.ENGAGE_RADIUS));
            bool toot = ai.chieftain().stunReady() && intel.chieftain != null && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(intel.chieftain.getGridX(), intel.chieftain.getGridY(), c[0], c[1]) < 20 * 20;
            float local_raw = local_enemy;
            if (enemyStunReadyNear(c[0], c[1], com.oddlabs.tt.player.gauntlet.Military.ENGAGE_RADIUS + 4))
            {
                local_enemy = (float)(local_enemy * ai.strategy().enemy_stun_mult);
            }
            bool stall_peons = ai.strategy().stall_peons;
            if (ai.strategy().stall_calm && ((stall_peons ? armed > 0.0f : local_enemy > 0.0f || anyFighting(army))))
            {
                last_progress_time = ai.time();
            }
            else if (stall_peons && ai.strategy().stall_calm && local_enemy > 0.0f && anyFighting(army))
            {
                ai.aiLog().count("stall_peon_pin");
            }
            float total = 0.0f;
            int stunned_count = 0;
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(army))
            {
                total = (float)(total + com.oddlabs.tt.player.gauntlet.Combat.lastingValue(u_2));
                if (com.oddlabs.tt.player.gauntlet.Intel.isStunned(u_2))
                {
                    stunned_count++;
                }
            }
            bool pinned = stunned_count * 10 > army.size() * 3;
            float worn_base = wornBasis(total);
            if (ai.logging() && (float)(ai.time() - last_trace) >= 4.0f)
            {
                traceBattle(army, c, total, local_enemy);
            }
            if (!pinned && ai.strategy().exploit_stun && chargeStunned(army, c, total))
            {
                return;
            }
            if (ai.strategy().siege && !pinned && siege(army, c, total))
            {
                return;
            }
            if (!toot && !pinned && ai.strategy().precontact_ratio > 0.0f && !anyFighting(army))
            {
                float wide = withEnemyTowers(enemyFightersNear(c[0], c[1], 36), c[0], c[1], 36);
                float terrain = terrainFactor(army, intel.enemy_warriors, c[0], c[1], 36);
                if (wide > 0.0f && (float)(total * terrain) < (float)(ai.strategy().precontact_ratio * wide))
                {
                    ai.log(java.lang.JString.format("turning back before contact: %.1f against %.1f", (float)(total * terrain), wide));
                    beginRetreat();
                    return;
                }
            }
            com.oddlabs.tt.player.gauntlet.Strategy strategy = ai.strategy();
            float weighed = (strategy.retreat_split_guard && ours < (float)(0.5f * total) ? total : ours);
            bool outmatched = local_enemy > (float)(strategy.retreat_ratio * java.lang.Math.max(weighed, 1.0f));
            bool worn = total < (float)(strategy.worn_ratio * worn_base) && local_enemy > total;
            if (!toot && !pinned && (outmatched || worn) && pillage(army, c, total))
            {
                return;
            }
            if (!toot && !pinned && !outmatched && !split_guard_counted && local_enemy > (float)(strategy.retreat_ratio * java.lang.Math.max(ours, 1.0f)))
            {
                split_guard_counted = true;
                ai.aiLog().count("split_guard_kept");
            }
            if (!toot && !pinned && outmatched)
            {
                if (local_raw <= (float)(strategy.retreat_ratio * java.lang.Math.max(weighed, 1.0f)))
                {
                    ai.aiLog().count("stun_fear_retreat");
                }
                ai.log(java.lang.JString.format("retreat: local %.1f vs enemy %.1f (army %.1f of %.1f)", ours, local_enemy, total, attack_initial_strength));
                beginRetreat();
                return;
            }
            if (!pinned && worn)
            {
                ai.aiLog().count("worn_fired");
                if (local_raw <= total)
                {
                    ai.aiLog().count("stun_fear_worn");
                }
                ai.log(java.lang.JString.format("retreat: worn down to %.1f of %.1f", total, worn_base));
                beginRetreat();
                return;
            }
            if (!pinned && !worn_skip_counted && strategy.worn_basis != 0 && total < (float)(strategy.worn_ratio * attack_initial_strength) && local_enemy > total)
            {
                worn_skip_counted = true;
                ai.aiLog().count("worn_skipped");
            }
            if (target == null || target.isDead())
            {
                if (strike)
                {
                    endAttack();
                    return;
                }
                com.oddlabs.tt.model.Selectable next = retarget(c);
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
                foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(intel.enemy_warriors))
                {
                    if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(e.getGridX(), e.getGridY(), c[0], c[1]) <= (com.oddlabs.tt.player.gauntlet.Military.ENGAGE_RADIUS + 8) * (com.oddlabs.tt.player.gauntlet.Military.ENGAGE_RADIUS + 8))
                    {
                        fighters.add(e);
                    }
                }
                foreach (com.oddlabs.tt.model.Unit e_2 in java.lang.JIter.of(intel.enemy_chieftains))
                {
                    if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(e_2.getGridX(), e_2.getGridY(), c[0], c[1]) <= (com.oddlabs.tt.player.gauntlet.Military.ENGAGE_RADIUS + 8) * (com.oddlabs.tt.player.gauntlet.Military.ENGAGE_RADIUS + 8))
                    {
                        fighters.add(e_2);
                    }
                }
                engageSpread(army, fighters, focus[0], focus[1], true);
                huntChieftains(army);
                return;
            }
            com.oddlabs.tt.player.gauntlet.DistanceField field = target_field;
            int dist = com.oddlabs.tt.player.gauntlet.DistanceField.UNREACHABLE;
            int[] waypoint = new int[] { target_x, target_y };
            com.oddlabs.tt.model.Unit pivot = null;
            int pivot_dist = 0;
            if (field != null)
            {
                java.util.List ranked = new java.util.ArrayList(army);
                ranked.removeIf(new java.util.function.Predicate_L((object u_3_l) =>
                {
                    com.oddlabs.tt.model.Unit u_3 = (com.oddlabs.tt.model.Unit)u_3_l;
                    return field.getAround(u_3.getGridX(), u_3.getGridY(), 1) == com.oddlabs.tt.player.gauntlet.DistanceField.UNREACHABLE;
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
            march_wp = waypoint;
            march_calm_time = ai.time();
            if (ai.strategy().reinforce_intercept && waitForReinforcements(c, total))
            {
                return;
            }
            bool column = field != null && ai.time() < column_until;
            if (column_until >= 0.0f && !column)
            {
                endColumnMarch(dist, c);
            }
            foreach (com.oddlabs.tt.model.Unit u_4 in java.lang.JIter.of(army))
            {
                int d = (field != null ? field.getAround(u_4.getGridX(), u_4.getGridY(), 1) : com.oddlabs.tt.player.gauntlet.DistanceField.UNREACHABLE);
                if (column && d != com.oddlabs.tt.player.gauntlet.DistanceField.UNREACHABLE)
                {
                    int step = (pivot != null ? java.lang.Math.min(lead, d - (pivot_dist - 2 * lead)) : lead);
                    if (step >= 8)
                    {
                        int[] own = field.stepTowardsSource(u_4.getGridX(), u_4.getGridY(), step);
                        attackGround(u_4, own[0], own[1], false);
                    }
                    continue;
                }
                if (pivot != null && d != com.oddlabs.tt.player.gauntlet.DistanceField.UNREACHABLE && d < pivot_dist - 24)
                {
                    continue;
                }
                attackGround(u_4, waypoint[0], waypoint[1], false);
            }
            if (dist != com.oddlabs.tt.player.gauntlet.DistanceField.UNREACHABLE && dist < best_target_dist - 20)
            {
                best_target_dist = dist;
                last_progress_time = ai.time();
            }
            if ((float)(ai.time() - last_progress_time) > 75.0f && ((ai.strategy().stall_peons ? armed == 0.0f : local_enemy == 0.0f)))
            {
                stalled(c);
            }
        }
        // Java: 2844
        public void traceBattle(java.util.List army, int[] c, float total, float local_enemy)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            last_trace = ai.time();
            int far = 0;
            int fighting = 0;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(army))
            {
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(u.getGridX(), u.getGridY(), c[0], c[1]) > 16 * 16)
                {
                    far++;
                }
                if (((object)((com.oddlabs.tt.player.gauntlet.Intel.WarriorState)intel.warrior_states.get(u)) == (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorState.FIGHT))
                {
                    fighting++;
                }
            }
            int enemies = com.oddlabs.tt.player.gauntlet.Combat.countNear(intel.enemy_warriors, c[0], c[1], com.oddlabs.tt.player.gauntlet.Military.ENGAGE_RADIUS);
            int towers = com.oddlabs.tt.player.gauntlet.Combat.countNear(intel.enemy_towers, c[0], c[1], com.oddlabs.tt.player.gauntlet.Military.ENGAGE_RADIUS);
            int target_d = (target_field != null ? target_field.get(c[0], c[1]) : (-1));
            int[] types = new int[3];
            int stunned = 0;
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(intel.enemy_warriors))
            {
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(e.getGridX(), e.getGridY(), c[0], c[1]) > com.oddlabs.tt.player.gauntlet.Military.ENGAGE_RADIUS * com.oddlabs.tt.player.gauntlet.Military.ENGAGE_RADIUS)
                {
                    continue;
                }
                types[com.oddlabs.tt.player.gauntlet.Intel.warriorType(e).ordinal()]++;
                if (com.oddlabs.tt.player.gauntlet.Intel.isStunned(e))
                {
                    stunned++;
                }
            }
            int our_stunned = 0;
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(army))
            {
                if (com.oddlabs.tt.player.gauntlet.Intel.isStunned(u_2))
                {
                    our_stunned++;
                }
            }
            ai.log(java.lang.JString.format("battle: army %d (%.1f, %d far, %d fighting, %d stunned) at %d,%d h%.0f, %dm to target; near: %d warriors (r%d i%d c%d, %d stunned) h%.0f %d towers (%.1f)", army.size(), total, far, fighting, our_stunned, c[0], c[1], meanHeight(army, c[0], c[1], 18), target_d, enemies, types[0], types[1], types[2], stunned, meanHeight(intel.enemy_warriors, c[0], c[1], com.oddlabs.tt.player.gauntlet.Military.ENGAGE_RADIUS), towers, local_enemy));
        }
        // Java: 2884
        public bool chargeStunned(java.util.List army, int[] c, float total)
        {
            java.util.List stunned = stunnedEnemiesNear(c[0], c[1], 36);
            if (stunned.isEmpty())
            {
                return false;
            }
            float asleep = 0.0f;
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(stunned))
            {
                asleep = (float)(asleep + com.oddlabs.tt.player.gauntlet.Combat.lastingValue(e));
            }
            float awake = withEnemyTowers(enemyFightersNear(c[0], c[1], 36), c[0], c[1], 36);
            if (!(asleep >= 3.0f && total >= (float)(0.8f * awake) && !(ai.strategy().enemy_stun_mult > 1.0f && enemyStunReadyNear(c[0], c[1], 45))))
            {
                return false;
            }
            if ((float)(ai.time() - last_charge_log) > 10.0f)
            {
                last_charge_log = ai.time();
                ai.log(java.lang.JString.format("charging %d stunned enemies (%.1f asleep, %.1f awake, army %.1f)", stunned.size(), asleep, awake, total));
            }
            int[] sc = com.oddlabs.tt.player.gauntlet.MapAnalysis.centroid(stunned);
            java.util.List prey = new java.util.ArrayList(stunned);
            if (ai.strategy().charge_mixed)
            {
                foreach (com.oddlabs.tt.model.Unit e_2 in java.lang.JIter.of(ai.intel().enemy_warriors))
                {
                    if (!com.oddlabs.tt.player.gauntlet.Intel.isStunned(e_2) && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(e_2.getGridX(), e_2.getGridY(), c[0], c[1]) <= (com.oddlabs.tt.player.gauntlet.Military.ENGAGE_RADIUS + 8) * (com.oddlabs.tt.player.gauntlet.Military.ENGAGE_RADIUS + 8))
                    {
                        prey.add(e_2);
                    }
                }
            }
            engageSpread(army, prey, sc[0], sc[1], true);
            huntChieftains(army);
            return true;
        }
        // Java: 2919
        public void endColumnMarch(int dist, int[] c)
        {
            column_until = (float)(-1.0f);
            int gain = (column_start_dist != com.oddlabs.tt.player.gauntlet.DistanceField.UNREACHABLE && dist != com.oddlabs.tt.player.gauntlet.DistanceField.UNREACHABLE ? column_start_dist - dist : 0);
            com.oddlabs.tt.model.Selectable was = column_target;
            string outcome = (was == null || was.isDead() ? "through" : (((object)was != (object)target) ? "retarget" : (gain >= 30 ? "through" : "still")));
            ai.aiLog().count("unjam_" + java.lang.JStr.valueOf(outcome));
            ai.log(java.lang.JString.format("unjam: column march over (%s), pivot %d m from the target at %d,%d (%d m at the start), army at %d,%d", outcome, dist, target_x, target_y, column_start_dist, c[0], c[1]));
            column_target = null;
        }
        // Java: 2938
        public void stalled(int[] c)
        {
            if (ai.logging())
            {
                com.oddlabs.tt.player.gauntlet.DistanceField f = target_field;
                int cells = 0;
                if (f != null)
                {
                    foreach (int cost in f.raw())
                    {
                        if (cost != com.oddlabs.tt.player.gauntlet.DistanceField.UNREACHABLE)
                        {
                            cells++;
                        }
                    }
                }
                int fc = cells;
                ai.log("attack stalled (field from " + (target_x) + "," + (target_y) + ", target region " + (fc) + " cells, staging " + java.lang.JStr.valueOf(((f != null && f.getAround(staging_x, staging_y, 2) != com.oddlabs.tt.player.gauntlet.DistanceField.UNREACHABLE ? "reaches it" : "cut off"))) + ")");
            }
            else
            {
                ai.log("attack stalled");
            }
            if (target != null && ai.strategy().skip_stalled)
            {
                stalled_targets.put(target, ai.time());
                ai.aiLog().count("target_stalled");
                com.oddlabs.tt.player.gauntlet.DistanceField f_2 = target_field;
                if (f_2 != null && f_2.getAround(staging_x, staging_y, 2) == com.oddlabs.tt.player.gauntlet.DistanceField.UNREACHABLE)
                {
                    dead_regions.add(f_2);
                    dead_region_times.add(ai.time());
                    if (dead_regions.size() > 3)
                    {
                        dead_regions.removeFirst();
                        dead_region_times.removeFirst();
                    }
                    ai.aiLog().count("target_region_dead");
                }
                if (ai.strategy().stall_calm && (f_2 == null || f_2.getAround(staging_x, staging_y, 2) != com.oddlabs.tt.player.gauntlet.DistanceField.UNREACHABLE))
                {
                    com.oddlabs.tt.model.Selectable next = retarget(c);
                    if (next != null)
                    {
                        setTarget(next);
                        last_progress_time = ai.time();
                        best_target_dist = java.lang.Integer.MAX_VALUE;
                        ai.aiLog().count("stall_retarget");
                        return;
                    }
                }
            }
            beginRetreat();
        }
        // Java: 2990
        public float wornBasis(float total)
        {
            com.oddlabs.tt.player.gauntlet.Strategy strategy = ai.strategy();
            if (strategy.worn_basis == 1)
            {
                worn_peak = java.lang.Math.max(worn_peak, total);
                return worn_peak;
            }
            if (strategy.worn_basis == 2)
            {
                float now = ai.time();
                while (!worn_history.isEmpty() && ((float[])worn_history.getLast())[1] <= total)
                {
                    worn_history.removeLast();
                }
                worn_history.addLast(new float[] { now, total });
                while (worn_history.size() > 1 && ((float[])worn_history.getFirst())[0] < (float)(now - strategy.worn_window))
                {
                    worn_history.removeFirst();
                }
                return ((float[])worn_history.getFirst())[1];
            }
            return attack_initial_strength;
        }
        // Java: 3013
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
        // Java: 3023
        public float meanHeight(java.util.List units, int x, int y, int radius)
        {
            float sum = 0.0f;
            int n = 0;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(units))
            {
                if (u.isDead() || com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, u.getGridX(), u.getGridY()) > radius * radius)
                {
                    continue;
                }
                sum = (float)(sum + ai.map().height(u.getGridX(), u.getGridY()));
                n++;
            }
            return (n == 0 ? java.lang.Float.NaN : (float)(sum / n));
        }
        // Java: 3039
        public bool holdForApproachingEnemy(java.util.List army, int[] c)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            int far = com.oddlabs.tt.player.gauntlet.Combat.countNear(intel.enemy_warriors, c[0], c[1], 26);
            int near = 0;
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(intel.enemy_warriors))
            {
                if (e.isDead())
                {
                    continue;
                }
                foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(army))
                {
                    if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(u.getGridX(), u.getGridY(), e.getGridX(), e.getGridY()) <= 11 * 11)
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
            int[] enemy = com.oddlabs.tt.player.gauntlet.Military.nearestGroup(intel.enemy_warriors, c[0], c[1], 26);
            if (enemy == null)
            {
                return false;
            }
            int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(c[0], c[1], enemy[0], enemy[1]);
            bool closing = d < last_enemy_d2 - 4;
            last_enemy_d2 = d;
            bool motion = ai.strategy().hold_closing == 1;
            if (motion)
            {
                bool coming = groupComing(intel.enemy_warriors, enemy, c);
                if (hold_until < 0.0f && closing && !coming && (float)(ai.time() - last_hold_skip) >= 12.0f)
                {
                    last_hold_skip = ai.time();
                    ai.aiLog().count("hold_skipped_static");
                }
                closing = coming;
            }
            if (hold_until < 0.0f)
            {
                if (!closing)
                {
                    return false;
                }
                if (motion)
                {
                    ai.aiLog().count("hold_coming");
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
        // Java: 3096
        public bool groupComing(java.util.List units, int[] enemy, int[] c)
        {
            float now = ai.time();
            hold_groups.removeIf(new java.util.function.Predicate_L((object g_l) =>
            {
                float[] g = (float[])g_l;
                return g[0] < (float)(now - 4.0f);
            }));
            float[] then = null;
            foreach (float[] g_2 in java.lang.JIter.of(hold_groups))
            {
                if (g_2[0] <= (float)(now - 2.0f))
                {
                    then = g_2;
                }
            }
            hold_groups.addLast(new float[] { now, enemy[0], enemy[1] });
            if (then == null)
            {
                return false;
            }
            float tx = (float)(c[0] - then[1]);
            float ty = (float)(c[1] - then[2]);
            float len = ((float)(java.lang.Math.sqrt((float)((float)(tx * tx) + (float)(ty * ty)))));
            if (len < 1.0f)
            {
                return false;
            }
            float toward = (float)(((float)((float)(((float)(enemy[0] - then[1])) * tx) + (float)(((float)(enemy[1] - then[2])) * ty))) / len);
            if (toward < 2.0f)
            {
                return false;
            }
            com.oddlabs.tt.model.Unit nearest = null;
            int best = 26 * 26;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(units))
            {
                if (u.isDead())
                {
                    continue;
                }
                int dd = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(c[0], c[1], u.getGridX(), u.getGridY());
                if (dd <= best)
                {
                    best = dd;
                    nearest = u;
                }
            }
            if (nearest == null)
            {
                return false;
            }
            int n = 0;
            int parked = 0;
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(units))
            {
                if (u_2.isDead() || com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(nearest.getGridX(), nearest.getGridY(), u_2.getGridX(), u_2.getGridY()) > 8 * 8)
                {
                    continue;
                }
                n++;
                if (com.oddlabs.tt.player.gauntlet.Intel.isParked(u_2))
                {
                    parked++;
                }
            }
            return parked * 2 < n;
        }
        // Java: 3145
        public bool pillage(java.util.List army, int[] c, float total)
        {
            if (!ai.strategy().pillage || total < 6.0f || enemyStrengthNear(c[0], c[1], 30) > (float)(0.5f * total))
            {
                return false;
            }
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            java.util.List prey = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit p in java.lang.JIter.of(intel.enemy_peons))
            {
                if (p.isDead() || com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(p.getGridX(), p.getGridY(), c[0], c[1]) > 45 * 45)
                {
                    continue;
                }
                bool covered = false;
                foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(intel.enemy_towers))
                {
                    if (com.oddlabs.tt.player.gauntlet.Intel.isTowerActive(t) && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(t.getGridX(), t.getGridY(), p.getGridX(), p.getGridY()) <= 11 * 11)
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
            int[] pc = com.oddlabs.tt.player.gauntlet.MapAnalysis.centroid(prey);
            engageSpread(army, prey, pc[0], pc[1], false);
            return true;
        }
        // Java: 3179
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
                asleep = (float)(asleep + com.oddlabs.tt.player.gauntlet.Combat.lastingValue(e));
            }
            float ours = 0.0f;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(units))
            {
                ours = (float)(ours + com.oddlabs.tt.player.gauntlet.Combat.lastingValue(u));
            }
            float awake = enemyFightersNear(x, y, 30);
            if (asleep < 3.0f || ours < (float)(0.8f * awake) || (ai.strategy().enemy_stun_mult > 1.0f && enemyStunReadyNear(x, y, 40)))
            {
                return false;
            }
            int[] sc = com.oddlabs.tt.player.gauntlet.MapAnalysis.centroid(stunned);
            engageSpread(units, stunned, sc[0], sc[1], true);
            huntChieftains(units);
            return true;
        }
        // Java: 3199
        public int[] blastPlay()
        {
            return (ai.time() < blast_play_until ? new int[] { threat_x, threat_y } : (int[])(null));
        }
        // Java: 3207
        public bool blastDefense(java.util.List defenders, float enemy)
        {
            com.oddlabs.tt.player.gauntlet.Strategy strategy = ai.strategy();
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
                if (com.oddlabs.tt.player.gauntlet.Intel.isStunned(u))
                {
                    continue;
                }
                int d2 = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(u.getGridX(), u.getGridY(), cx, cy);
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
        // Java: 3245
        public bool sieging()
        {
            return ((object)mode == (object)com.oddlabs.tt.player.gauntlet.Military.Mode.ATTACK) && siege_start >= 0.0f;
        }
        // Java: 3249
        public void endSiege()
        {
            if (siege_start >= 0.0f)
            {
                ai.log(java.lang.JString.format("siege over after %.0fs", (float)(ai.time() - siege_start)));
            }
            siege_start = (float)(-1.0f);
            siege_assign.clear();
        }
        // Java: 3261
        public bool siege(java.util.List army, int[] c, float total)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            float now = ai.time();
            java.util.List awake = new java.util.ArrayList();
            java.util.List helpless = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(intel.enemy_towers))
            {
                if (!com.oddlabs.tt.player.gauntlet.Intel.isTowerManned(t) || com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(t.getGridX(), t.getGridY(), c[0], c[1]) > 34 * 34)
                {
                    continue;
                }
                if (com.oddlabs.tt.player.gauntlet.Intel.isTowerActive(t))
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
                bool about_to = stunner && since >= 1.8f && since < 4.5f && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(chief.getGridX(), chief.getGridY(), t_2.getGridX(), t_2.getGridY()) <= 300;
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
                int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(t_4.getGridX(), t_4.getGridY(), c[0], c[1]);
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
                hx = first.getGridX() + java.lang.Math.round((float)((float)((c[0] - first.getGridX()) / len) * (com.oddlabs.tt.player.gauntlet.Military.SIEGE_HOLD + 1)));
                hy = first.getGridY() + java.lang.Math.round((float)((float)((c[1] - first.getGridY()) / len) * (com.oddlabs.tt.player.gauntlet.Military.SIEGE_HOLD + 1)));
            }
            int[] hold = outOfReach(hx, hy, awake);
            int r2 = com.oddlabs.tt.player.gauntlet.Military.TOWER_REACH * com.oddlabs.tt.player.gauntlet.Military.TOWER_REACH;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(army))
            {
                if (com.oddlabs.tt.player.gauntlet.Intel.isStunned(u))
                {
                    continue;
                }
                bool exposed = false;
                foreach (com.oddlabs.tt.model.Building t_5 in java.lang.JIter.of(awake))
                {
                    exposed |= com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(u.getGridX(), u.getGridY(), t_5.getGridX(), t_5.getGridY()) <= r2;
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
        // Java: 3367
        public int[] outOfReach(int x, int y, java.util.List towers)
        {
            for (int pass = 0; pass < 4; pass++)
            {
                com.oddlabs.tt.model.Building near = null;
                int best = com.oddlabs.tt.player.gauntlet.Military.SIEGE_HOLD * com.oddlabs.tt.player.gauntlet.Military.SIEGE_HOLD;
                foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(towers))
                {
                    int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, t.getGridX(), t.getGridY());
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
                x = near.getGridX() + java.lang.Math.round((float)((float)(dx / len) * (com.oddlabs.tt.player.gauntlet.Military.SIEGE_HOLD + 1)));
                y = near.getGridY() + java.lang.Math.round((float)((float)(dy / len) * (com.oddlabs.tt.player.gauntlet.Military.SIEGE_HOLD + 1)));
            }
            int size = ai.map().getSize();
            return new int[] { java.lang.Math.clamp(x, 0, size - 1), java.lang.Math.clamp(y, 0, size - 1) };
        }
        // Java: 3398
        public void pullDown(java.util.List army, java.util.List helpless)
        {
            java.util.List free = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(army))
            {
                if (!com.oddlabs.tt.player.gauntlet.Intel.isStunned(u) && !u.isDead())
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
                    return java.lang.Integer.compare(com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(a.getGridX(), a.getGridY(), t.getGridX(), t.getGridY()), com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(b.getGridX(), b.getGridY(), t.getGridX(), t.getGridY()));
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
                int[] tc = com.oddlabs.tt.player.gauntlet.MapAnalysis.centroid(new java.util.ArrayList(helpless));
                foreach (com.oddlabs.tt.model.Unit u_3 in java.lang.JIter.of(free))
                {
                    attackGround(u_3, tc[0], tc[1], true);
                }
            }
        }
        // Java: 3427
        public java.util.List stunnedEnemiesNear(int x, int y, int radius)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            java.util.List stunned = new java.util.ArrayList();
            int r2 = radius * radius;
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(intel.enemy_warriors))
            {
                if (!e.isDead() && com.oddlabs.tt.player.gauntlet.Intel.isStunned(e) && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, e.getGridX(), e.getGridY()) <= r2)
                {
                    stunned.add(e);
                }
            }
            foreach (com.oddlabs.tt.model.Unit e_2 in java.lang.JIter.of(intel.enemy_chieftains))
            {
                if (!e_2.isDead() && com.oddlabs.tt.player.gauntlet.Intel.isStunned(e_2) && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, e_2.getGridX(), e_2.getGridY()) <= r2)
                {
                    stunned.add(e_2);
                }
            }
            return stunned;
        }
        // Java: 3441
        public int[] enemyFocus(int x, int y)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            int[] w = com.oddlabs.tt.player.gauntlet.Military.nearestGroup(intel.enemy_warriors, x, y, com.oddlabs.tt.player.gauntlet.Military.ENGAGE_RADIUS);
            if (w != null)
            {
                return w;
            }
            int[] ch = com.oddlabs.tt.player.gauntlet.Military.nearestGroup(intel.enemy_chieftains, x, y, com.oddlabs.tt.player.gauntlet.Military.ENGAGE_RADIUS);
            if (ch != null)
            {
                return ch;
            }
            int[] t = com.oddlabs.tt.player.gauntlet.Military.nearestGroup(intel.enemy_towers, x, y, com.oddlabs.tt.player.gauntlet.Military.ENGAGE_RADIUS);
            if (t != null)
            {
                return t;
            }
            int[] p = com.oddlabs.tt.player.gauntlet.Military.nearestGroup(intel.enemy_peons, x, y, 14);
            if (p != null)
            {
                return p;
            }
            return com.oddlabs.tt.player.gauntlet.Military.nearestGroup(intel.enemy_buildings, x, y, 14);
        }
        // Java: 3458
        public static int[] nearestGroup(java.util.List units, int x, int y, int radius)
        {
            com.oddlabs.tt.model.Selectable nearest = null;
            int best = radius * radius;
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(units))
            {
                if (s.isDead())
                {
                    continue;
                }
                int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, s.getGridX(), s.getGridY());
                if (d <= best)
                {
                    best = d;
                    nearest = s;
                }
            }
            if (nearest == null)
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
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(nearest.getGridX(), nearest.getGridY(), s_2.getGridX(), s_2.getGridY()) <= 8 * 8)
                {
                    sx += s_2.getGridX();
                    sy += s_2.getGridY();
                    n++;
                }
            }
            return new int[] { unchecked((int)((sx / n))), unchecked((int)((sy / n))) };
        }
        // Java: 3489
        public void beginRetreat()
        {
            mode = com.oddlabs.tt.player.gauntlet.Military.Mode.RETREAT;
            foreach (java.util.Map.Entry e in java.lang.JIter.of(roles.entrySet()))
            {
                if (((object)((com.oddlabs.tt.player.gauntlet.Military.Role)e.getValue()) == (object)com.oddlabs.tt.player.gauntlet.Military.Role.REINFORCE))
                {
                    e.setValue(com.oddlabs.tt.player.gauntlet.Military.Role.ARMY);
                    move(((com.oddlabs.tt.model.Unit)e.getKey()), staging_x, staging_y);
                }
                else if (((object)((com.oddlabs.tt.player.gauntlet.Military.Role)e.getValue()) == (object)com.oddlabs.tt.player.gauntlet.Military.Role.ATTACK))
                {
                    if (ai.strategy().retreat_rearguard && inFight(((com.oddlabs.tt.model.Unit)e.getKey())))
                    {
                        ai.aiLog().count("rearguard_units");
                        continue;
                    }
                    move(((com.oddlabs.tt.model.Unit)e.getKey()), staging_x, staging_y);
                }
            }
        }
        // Java: 3506
        public bool inFight(com.oddlabs.tt.model.Unit u)
        {
            if (((object)((com.oddlabs.tt.player.gauntlet.Intel.WarriorState)ai.intel().warrior_states.get(u)) == (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorState.FIGHT))
            {
                return true;
            }
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(ai.intel().enemy_warriors))
            {
                if (!e.isDead() && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(u.getGridX(), u.getGridY(), e.getGridX(), e.getGridY()) <= 9 * 9)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 3519
        public void considerReinforcing()
        {
            java.util.List group = new java.util.ArrayList();
            foreach (java.util.Map.Entry e in java.lang.JIter.of(roles.entrySet()))
            {
                if (((object)((com.oddlabs.tt.player.gauntlet.Military.Role)e.getValue()) != (object)com.oddlabs.tt.player.gauntlet.Military.Role.ARMY))
                {
                    continue;
                }
                com.oddlabs.tt.model.Unit u = ((com.oddlabs.tt.model.Unit)e.getKey());
                com.oddlabs.tt.player.gauntlet.Intel.WarriorState s = ((com.oddlabs.tt.player.gauntlet.Intel.WarriorState)ai.intel().warrior_states.get(u));
                if (((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorState.STUNNED) || ((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorState.ENTER))
                {
                    continue;
                }
                group.add(u);
            }
            group = beyondGuard(group);
            float home = com.oddlabs.tt.player.gauntlet.Combat.total(group);
            if (group.isEmpty())
            {
                return;
            }
            float away = attackStrength();
            bool capped = nearUnitCap();
            float capped_need = (capped ? cappedNeed(away) : 12.0f);
            if (home < java.lang.Math.max(12.0f, (float)(ai.strategy().reinforce_ratio * away)) && !(capped && home >= capped_need))
            {
                if (capped && home >= 12.0f)
                {
                    ai.aiLog().count("capped_clump_wait");
                }
                return;
            }
            if (!capped && ai.enemiesAlive() > 1 && !ai.strategy().reinforce_multi)
            {
                return;
            }
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(group))
            {
                roles.put(u_2, com.oddlabs.tt.player.gauntlet.Military.Role.REINFORCE);
            }
            ai.log(java.lang.JString.format("reinforcing the attack (%.1f) with %.1f", away, home));
        }
        // Java: 3554
        public float cappedNeed(float away)
        {
            com.oddlabs.tt.player.gauntlet.Strategy strategy = ai.strategy();
            if (strategy.capped_clump <= 0.0f)
            {
                return 12.0f;
            }
            int[] front = attackCenter();
            com.oddlabs.tt.model.Building armory = ai.intel().armory();
            if (front == null || armory == null || com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(front[0], front[1], armory.getGridX(), armory.getGridY()) < strategy.capped_clump_cells * strategy.capped_clump_cells)
            {
                return 12.0f;
            }
            return java.lang.Math.max(12.0f, java.lang.Math.max(strategy.capped_clump_min, (float)(strategy.capped_clump * away)));
        }
        // Java: 3570
        public java.util.List beyondGuard(java.util.List group)
        {
            float guard = ai.strategy().home_guard;
            com.oddlabs.tt.model.Building armory = ai.intel().armory();
            if (guard <= 0.0f || armory == null)
            {
                return group;
            }
            java.util.List sorted = new java.util.ArrayList(group);
            int ax = armory.getGridX();
            int ay = armory.getGridY();
            sorted.sort(java.util.Comparator_.comparingInt(new java.util.function.ToIntFunction_L((object u_l) =>
            {
                com.oddlabs.tt.model.Unit u = (com.oddlabs.tt.model.Unit)u_l;
                return com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(u.getGridX(), u.getGridY(), ax, ay);
            })));
            float kept = 0.0f;
            java.util.List leave = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(sorted))
            {
                if (kept < guard)
                {
                    kept = (float)(kept + com.oddlabs.tt.player.gauntlet.Combat.@value(u_2));
                }
                else
                {
                    leave.add(u_2);
                }
            }
            return leave;
        }
        // Java: 3591
        public void reinforce()
        {
            int[] front = null;
            bool intercept = ai.strategy().reinforce_intercept;
            java.util.List attackers = null;
            foreach (java.util.Map.Entry e in java.lang.JIter.of(roles.entrySet()))
            {
                if (((object)((com.oddlabs.tt.player.gauntlet.Military.Role)e.getValue()) != (object)com.oddlabs.tt.player.gauntlet.Military.Role.REINFORCE))
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
                    e.setValue(com.oddlabs.tt.player.gauntlet.Military.Role.ARMY);
                    continue;
                }
                bool joined = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(u.getGridX(), u.getGridY(), front[0], front[1]) <= 20 * 20;
                if (!joined && intercept)
                {
                    if (attackers == null)
                    {
                        attackers = withRole(com.oddlabs.tt.player.gauntlet.Military.Role.ATTACK);
                    }
                    foreach (com.oddlabs.tt.model.Unit a in java.lang.JIter.of(attackers))
                    {
                        if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(u.getGridX(), u.getGridY(), a.getGridX(), a.getGridY()) <= 12 * 12)
                        {
                            joined = true;
                            break;
                        }
                    }
                }
                if (joined)
                {
                    e.setValue(com.oddlabs.tt.player.gauntlet.Military.Role.ATTACK);
                    attack_initial_strength += com.oddlabs.tt.player.gauntlet.Combat.@value(u);
                    ai.aiLog().count("reinforce_joined");
                    continue;
                }
                int[] goal = (intercept && march_wp != null && (float)(ai.time() - march_calm_time) < 2.0f ? march_wp : front);
                attackGround(u, goal[0], goal[1], false);
            }
        }
        public int[] march_wp;
        public float march_calm_time;
        public float column_until;
        public int jam_scans;
        public int jam_first_dist;
        public com.oddlabs.tt.player.gauntlet.DistanceField jam_field;
        public int column_start_dist;
        public com.oddlabs.tt.model.Selectable column_target;
        // Java: 3651
        public void noteBlocked(java.util.List blocked)
        {
            com.oddlabs.tt.player.gauntlet.Strategy strategy = ai.strategy();
            if (strategy.unjam <= 0)
            {
                return;
            }
            java.util.List stuck = new java.util.ArrayList();
            if (((object)mode == (object)com.oddlabs.tt.player.gauntlet.Military.Mode.ATTACK) && ai.time() >= strategy.unjam_from)
            {
                foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(blocked))
                {
                    if (((object)((com.oddlabs.tt.player.gauntlet.Military.Role)roles.get(u)) == (object)com.oddlabs.tt.player.gauntlet.Military.Role.ATTACK))
                    {
                        stuck.add(u);
                    }
                }
            }
            if (stuck.size() < strategy.unjam || target_field == null)
            {
                jam_scans = 0;
                return;
            }
            int[] jc = com.oddlabs.tt.player.gauntlet.MapAnalysis.centroid(stuck);
            int r = com.oddlabs.tt.player.gauntlet.Military.ENGAGE_RADIUS + 8;
            bool fight = enemyStrengthNear(jc[0], jc[1], r) > 0.0f;
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(ai.intel().enemy_towers))
            {
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(t.getGridX(), t.getGridY(), jc[0], jc[1]) <= r * r)
                {
                    fight = true;
                }
            }
            if (fight)
            {
                jam_scans = 0;
                return;
            }
            int pivot = attackPivotDist(target_field);
            if (jam_scans == 0 || ((object)jam_field != (object)target_field))
            {
                jam_scans = 0;
                jam_field = target_field;
                jam_first_dist = pivot;
            }
            jam_scans++;
            if ((float)((jam_scans - 1) * com.oddlabs.tt.player.gauntlet.Jams.PERIOD) < strategy.unjam_after)
            {
                return;
            }
            if (column_until < 0.0f)
            {
                if (pivot == com.oddlabs.tt.player.gauntlet.DistanceField.UNREACHABLE || jam_first_dist == com.oddlabs.tt.player.gauntlet.DistanceField.UNREACHABLE || pivot <= jam_first_dist - strategy.unjam_progress)
                {
                    ai.aiLog().count("unjam_moving");
                    jam_scans = 1;
                    jam_first_dist = pivot;
                    return;
                }
                column_start_dist = pivot;
                column_target = target;
                ai.aiLog().count("unjam_column");
                if (ai.logging())
                {
                    ai.log(java.lang.JString.format("unjam: %d of %d attack units blocked around %d,%d, pivot %d m from the target at %d,%d (%d m %.0f s ago): column march", stuck.size(), countRole(com.oddlabs.tt.player.gauntlet.Military.Role.ATTACK), jc[0], jc[1], pivot, target_x, target_y, jam_first_dist, (float)((jam_scans - 1) * com.oddlabs.tt.player.gauntlet.Jams.PERIOD)));
                }
            }
            column_until = (float)(ai.time() + strategy.unjam_time);
        }
        // Java: 3707
        public int attackPivotDist(com.oddlabs.tt.player.gauntlet.DistanceField field)
        {
            java.util.List ds = new java.util.ArrayList();
            foreach (java.util.Map.Entry e in java.lang.JIter.of(roles.entrySet()))
            {
                com.oddlabs.tt.model.Unit u = ((com.oddlabs.tt.model.Unit)e.getKey());
                if (((object)((com.oddlabs.tt.player.gauntlet.Military.Role)e.getValue()) != (object)com.oddlabs.tt.player.gauntlet.Military.Role.ATTACK) || u.isDead())
                {
                    continue;
                }
                int d = field.getAround(u.getGridX(), u.getGridY(), 1);
                if (d != com.oddlabs.tt.player.gauntlet.DistanceField.UNREACHABLE)
                {
                    ds.add(d);
                }
            }
            if (ds.isEmpty())
            {
                return com.oddlabs.tt.player.gauntlet.DistanceField.UNREACHABLE;
            }
            ds.sort(null);
            return ((int)((int?)ds.get(ds.size() / 3)));
        }
        public com.oddlabs.tt.model.Selectable hold_target;
        public float hold_since;
        // Java: 3727
        public bool waitForReinforcements(int[] c, float army_strength)
        {
            float s = 0.0f;
            long x = 0;
            long y = 0;
            int n = 0;
            foreach (java.util.Map.Entry e in java.lang.JIter.of(roles.entrySet()))
            {
                if (((object)((com.oddlabs.tt.player.gauntlet.Military.Role)e.getValue()) == (object)com.oddlabs.tt.player.gauntlet.Military.Role.REINFORCE))
                {
                    com.oddlabs.tt.model.Unit u = ((com.oddlabs.tt.model.Unit)e.getKey());
                    s = (float)(s + com.oddlabs.tt.player.gauntlet.Combat.@value(u));
                    x += u.getGridX();
                    y += u.getGridY();
                    n++;
                }
            }
            if (n == 0 || s < (float)(0.4f * army_strength))
            {
                return false;
            }
            int gx = unchecked((int)((x / n)));
            int gy = unchecked((int)((y / n)));
            if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(gx, gy, c[0], c[1]) > 80 * 80)
            {
                return false;
            }
            if (((object)hold_target != (object)target))
            {
                hold_target = target;
                hold_since = ai.time();
            }
            if ((float)(ai.time() - hold_since) > 20.0f)
            {
                return false;
            }
            ai.aiLog().count("reinforce_hold");
            return true;
        }
        // Java: 3756
        public void retreat()
        {
            int n = 0;
            int home = 0;
            foreach (java.util.Map.Entry e in java.lang.JIter.of(roles.entrySet()))
            {
                if (((object)((com.oddlabs.tt.player.gauntlet.Military.Role)e.getValue()) != (object)com.oddlabs.tt.player.gauntlet.Military.Role.ATTACK))
                {
                    continue;
                }
                com.oddlabs.tt.model.Unit u = ((com.oddlabs.tt.model.Unit)e.getKey());
                n++;
                com.oddlabs.tt.player.gauntlet.Intel.WarriorState st = ((com.oddlabs.tt.player.gauntlet.Intel.WarriorState)ai.intel().warrior_states.get(u));
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(u.getGridX(), u.getGridY(), staging_x, staging_y) <= 14 * 14)
                {
                    home++;
                }
                else if (((object)st == (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorState.IDLE) || (ai.strategy().retreat_rearguard && ((object)st != (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorState.MOVE) && ((object)st != (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorState.FIGHT) && ((object)st != (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorState.STUNNED)))
                {
                    move(u, staging_x, staging_y);
                }
            }
            if (n == 0 || home >= n * 7 / 10)
            {
                endAttack();
            }
        }
        // Java: 3778
        public void considerRaid()
        {
            com.oddlabs.tt.player.gauntlet.Strategy strategy = ai.strategy();
            if (ai.time() < strategy.raid_time || (float)(ai.time() - last_raid_end) < 40.0f)
            {
                return;
            }
            if (countRole(com.oddlabs.tt.player.gauntlet.Military.Role.RAID) > 0)
            {
                return;
            }
            int army = countRole(com.oddlabs.tt.player.gauntlet.Military.Role.ARMY);
            if (army < strategy.raid_size + 6)
            {
                return;
            }
            int[] spot = raidSpot(staging_x, staging_y, (float)(strategy.raid_size * com.oddlabs.tt.player.gauntlet.Combat.IRON));
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
                if (((object)((com.oddlabs.tt.player.gauntlet.Military.Role)e.getValue()) == (object)com.oddlabs.tt.player.gauntlet.Military.Role.ARMY) && ((object)com.oddlabs.tt.player.gauntlet.Intel.warriorType(u) == (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorType.IRON) && ((object)((com.oddlabs.tt.player.gauntlet.Intel.WarriorState)ai.intel().warrior_states.get(u)) == (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorState.IDLE))
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
                roles.put(u_2, com.oddlabs.tt.player.gauntlet.Military.Role.RAID);
                attackGround(u_2, spot[0], spot[1], true);
            }
            raid_x = spot[0];
            raid_y = spot[1];
            raid_start = ai.time();
            ai.log("raid with " + (squad.size()) + " on peons at " + (raid_x) + "," + (raid_y));
        }
        // Java: 3812
        public int[] raidSpot(int from_x, int from_y, float strength)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            int[] best = null;
            float best_score = java.lang.Float.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Unit p in java.lang.JIter.of(intel.enemy_peons))
            {
                int px = p.getGridX();
                int py = p.getGridY();
                float danger = withEnemyTowers(enemyStrengthNear(px, py, 30), px, py, 12);
                if (danger > (float)(0.5f * strength))
                {
                    continue;
                }
                int count = com.oddlabs.tt.player.gauntlet.Combat.countNear(intel.enemy_peons, px, py, 10);
                if (count < 3)
                {
                    continue;
                }
                float score = (float)(com.oddlabs.tt.player.gauntlet.MapAnalysis.meters(from_x, from_y, px, py) - (float)(25.0f * count));
                if (score < best_score)
                {
                    best_score = score;
                    best = new int[] { px, py };
                }
            }
            return best;
        }
        public com.oddlabs.tt.model.Selectable chase_target;
        public com.oddlabs.tt.player.Player chase_owner;
        public float chase_start;
        public float last_chase_end;
        public java.util.Map chase_banned;
        public java.util.Map qa_counts;
        // Java: 3845
        public void countBases()
        {
            qa_counts.clear();
            foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(ai.intel().enemy_buildings))
            {
                int id = ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID();
                if (b.isDead() || (id != com.oddlabs.tt.model.Race.BUILDING_QUARTERS && id != com.oddlabs.tt.model.Race.BUILDING_ARMORY))
                {
                    continue;
                }
                ((int[])qa_counts.computeIfAbsent(b.getOwner(), new java.util.function.Function_L((object k_l) =>
                {
                    com.oddlabs.tt.player.Player k = (com.oddlabs.tt.player.Player)k_l;
                    return new int[2];
                })))[(b.isComplete() ? 0 : 1)]++;
            }
        }
        // Java: 3857
        public bool huntable(com.oddlabs.tt.player.Player p)
        {
            int[] c = ((int[])qa_counts.getOrDefault(p, new int[2]));
            if (c[0] > 0)
            {
                return false;
            }
            int after = p.getUnitCountContainer().getNumSupplies() - ((p.hasActiveChieftain() ? 1 : 0));
            if (after > 8)
            {
                return false;
            }
            return p.hasActiveChieftain() || c[1] > 0;
        }
        // Java: 3867
        public bool clearForChase(com.oddlabs.tt.model.Selectable t, int[] from, int[] army_c)
        {
            com.oddlabs.tt.player.gauntlet.Strategy s = ai.strategy();
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            int x = t.getGridX();
            int y = t.getGridY();
            if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(from[0], from[1], x, y) > s.chief_hunt_range * s.chief_hunt_range)
            {
                ai.aiLog().count("hunt_skip_far");
                return false;
            }
            if (inDeadRegion(x, y) || stalled_targets.containsKey(t))
            {
                return false;
            }
            if (com.oddlabs.tt.player.gauntlet.Combat.countNear(intel.enemy_warriors, x, y, 15) > s.chief_hunt_escort)
            {
                ai.aiLog().count("hunt_skip_escort");
                return false;
            }
            foreach (com.oddlabs.tt.model.Building tw in java.lang.JIter.of(intel.enemy_towers))
            {
                if (com.oddlabs.tt.player.gauntlet.Intel.isTowerActive(tw) && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(tw.getGridX(), tw.getGridY(), x, y) <= 22 * 22)
                {
                    ai.aiLog().count("hunt_skip_tower");
                    return false;
                }
            }
            if (army_c != null && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(army_c[0], army_c[1], x, y) <= 25 * 25)
            {
                ai.aiLog().count("hunt_skip_army");
                return false;
            }
            foreach (com.oddlabs.tt.model.Selectable sel in java.lang.JIter.of(ai.owner().getUnits().getSet()))
            {
                if ((sel is com.oddlabs.tt.model.Building b) && !b.isDead() && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(b.getGridX(), b.getGridY(), x, y) <= 25 * 25)
                {
                    ai.aiLog().count("hunt_skip_base");
                    return false;
                }
            }
            return true;
        }
        // Java: 3901
        public void considerChase()
        {
            com.oddlabs.tt.player.gauntlet.Strategy s = ai.strategy();
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            com.oddlabs.tt.player.gauntlet.Military.Role source = (((object)mode == (object)com.oddlabs.tt.player.gauntlet.Military.Mode.ATTACK) ? com.oddlabs.tt.player.gauntlet.Military.Role.ATTACK : (((object)mode == (object)com.oddlabs.tt.player.gauntlet.Military.Mode.HOME) && threat_level < 2 ? com.oddlabs.tt.player.gauntlet.Military.Role.ARMY : (com.oddlabs.tt.player.gauntlet.Military.Role)(null)));
            if (source == null)
            {
                return;
            }
            java.util.List pool = new java.util.ArrayList();
            foreach (java.util.Map.Entry e in java.lang.JIter.of(roles.entrySet()))
            {
                com.oddlabs.tt.model.Unit u = ((com.oddlabs.tt.model.Unit)e.getKey());
                com.oddlabs.tt.player.gauntlet.Intel.WarriorState st = ((com.oddlabs.tt.player.gauntlet.Intel.WarriorState)intel.warrior_states.get(u));
                if (((object)((com.oddlabs.tt.player.gauntlet.Military.Role)e.getValue()) == (object)source) && ((object)com.oddlabs.tt.player.gauntlet.Intel.warriorType(u) == (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorType.IRON) && ((object)st != (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorState.STUNNED) && ((object)st != (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorState.ENTER) && ((object)st != (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorState.FIGHT))
                {
                    pool.add(u);
                }
            }
            if (pool.size() < s.chief_hunt_size + ((((object)source == (object)com.oddlabs.tt.player.gauntlet.Military.Role.ATTACK) ? 12 : 6)))
            {
                return;
            }
            int[] army_c = (((object)source == (object)com.oddlabs.tt.player.gauntlet.Military.Role.ATTACK) ? com.oddlabs.tt.player.gauntlet.MapAnalysis.centroid(pool) : (int[])(null));
            int[] from = (army_c != null ? army_c : new int[] { staging_x, staging_y });
            countBases();
            chase_banned.entrySet().removeIf(new java.util.function.Predicate_L((object e_2_l) =>
            {
                java.util.Map.Entry e_2 = (java.util.Map.Entry)e_2_l;
                return ((com.oddlabs.tt.model.Selectable)e_2.getKey()).isDead() || (float)(ai.time() - ((float)((float?)e_2.getValue()))) > 120.0f;
            }));
            com.oddlabs.tt.model.Selectable best = null;
            int best_d = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Unit ch in java.lang.JIter.of(intel.enemy_chieftains))
            {
                if (ch.isDead() || chase_banned.containsKey(ch) || !huntable(ch.getOwner()) || !clearForChase(ch, from, army_c))
                {
                    continue;
                }
                int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(from[0], from[1], ch.getGridX(), ch.getGridY());
                if (d < best_d)
                {
                    best_d = d;
                    best = ch;
                }
            }
            if (s.hunt_sites)
            {
                foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(intel.enemy_buildings))
                {
                    int id = ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID();
                    if (b.isDead() || b.isComplete() || chase_banned.containsKey(b) || (id != com.oddlabs.tt.model.Race.BUILDING_QUARTERS && id != com.oddlabs.tt.model.Race.BUILDING_ARMORY) || !huntable(b.getOwner()) || !clearForChase(b, from, army_c))
                    {
                        continue;
                    }
                    int d_2 = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(from[0], from[1], b.getGridX(), b.getGridY());
                    if (d_2 < best_d)
                    {
                        best_d = d_2;
                        best = b;
                    }
                }
            }
            if (best == null)
            {
                return;
            }
            com.oddlabs.tt.model.Selectable t = best;
            pool.sort(new java.util.Comparator_L((object a_l, object b_2_l) =>
            {
                com.oddlabs.tt.model.Unit a = (com.oddlabs.tt.model.Unit)a_l;
                com.oddlabs.tt.model.Unit b_2 = (com.oddlabs.tt.model.Unit)b_2_l;
                return java.lang.Integer.compare(com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(a.getGridX(), a.getGridY(), t.getGridX(), t.getGridY()), com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(b_2.getGridX(), b_2.getGridY(), t.getGridX(), t.getGridY()));
            }));
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(pool.subList(0, s.chief_hunt_size)))
            {
                roles.put(u_2, com.oddlabs.tt.player.gauntlet.Military.Role.CHASE);
                last_order.put(u_2, ai.time());
                ai.owner().setTarget(com.oddlabs.tt.model.Selectable.newArray(u_2), t, com.oddlabs.tt.model.Action.ATTACK, true);
            }
            chase_target = t;
            chase_owner = t.getOwner();
            chase_start = ai.time();
            ai.aiLog().count(((t is com.oddlabs.tt.model.Unit) ? "hunt_start_chief" : "hunt_start_site"));
            int dist = java.lang.JC.d2i(java.lang.Math.sqrt(best_d));
            ai.log(java.lang.JString.format("chase: %d on %s of %s at %d,%d, %d cells out", s.chief_hunt_size, ((t is com.oddlabs.tt.model.Unit) ? (object)("chieftain") : (object)("site")), t.getOwner().getPlayerInfo().getName(), t.getGridX(), t.getGridY(), dist));
        }
        // Java: 3968
        public com.oddlabs.tt.model.Selectable nextBlocker(com.oddlabs.tt.player.Player owner, int[] c, int cells)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            countBases();
            if (!huntable(owner))
            {
                return null;
            }
            com.oddlabs.tt.model.Selectable best = null;
            int best_d = cells * cells;
            java.util.List candidates = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit ch in java.lang.JIter.of(intel.enemy_chieftains))
            {
                if (!ch.isDead() && ((object)ch.getOwner() == (object)owner))
                {
                    candidates.add(ch);
                }
            }
            if (ai.strategy().hunt_sites)
            {
                foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(intel.enemy_buildings))
                {
                    int id = ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID();
                    if (!b.isDead() && !b.isComplete() && ((object)b.getOwner() == (object)owner) && (id == com.oddlabs.tt.model.Race.BUILDING_QUARTERS || id == com.oddlabs.tt.model.Race.BUILDING_ARMORY))
                    {
                        candidates.add(b);
                    }
                }
            }
            foreach (com.oddlabs.tt.model.Selectable t in java.lang.JIter.of(candidates))
            {
                int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(c[0], c[1], t.getGridX(), t.getGridY());
                if (d <= best_d && !chase_banned.containsKey(t) && clearForChase(t, c, null))
                {
                    best_d = d;
                    best = t;
                }
            }
            return best;
        }
        // Java: 3997
        public void chase()
        {
            java.util.List squad = withRole(com.oddlabs.tt.player.gauntlet.Military.Role.CHASE);
            if (squad.isEmpty())
            {
                return;
            }
            com.oddlabs.tt.player.gauntlet.Strategy s = ai.strategy();
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            com.oddlabs.tt.model.Selectable t = chase_target;
            int[] c = com.oddlabs.tt.player.gauntlet.MapAnalysis.centroid(squad);
            if (t == null || t.isDead())
            {
                if (t != null)
                {
                    ai.aiLog().count(((t is com.oddlabs.tt.model.Unit) ? "hunt_kill" : "hunt_site_razed"));
                    ai.log("chase: " + java.lang.JStr.valueOf((((t is com.oddlabs.tt.model.Unit) ? "chieftain" : "site"))) + " of " + java.lang.JStr.valueOf(t.getOwner().getPlayerInfo().getName()) + " done");
                }
                com.oddlabs.tt.model.Selectable next = (chase_owner == null ? (com.oddlabs.tt.model.Selectable)(null) : nextBlocker(chase_owner, c, 60));
                if (next != null)
                {
                    chase_target = next;
                    chase_start = ai.time();
                    ai.aiLog().count("hunt_chain");
                    return;
                }
                endChase(squad);
                return;
            }
            float ours = com.oddlabs.tt.player.gauntlet.Combat.total(squad);
            float danger = com.oddlabs.tt.player.gauntlet.Combat.strengthNear(intel.enemy_warriors, c[0], c[1], 24);
            foreach (com.oddlabs.tt.model.Unit ch in java.lang.JIter.of(intel.enemy_chieftains))
            {
                if (((object)ch != (object)t) && !ch.isDead() && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(ch.getGridX(), ch.getGridY(), c[0], c[1]) <= 24 * 24)
                {
                    danger = (float)(danger + com.oddlabs.tt.player.gauntlet.Combat.@value(ch));
                }
            }
            foreach (com.oddlabs.tt.model.Building tw in java.lang.JIter.of(intel.enemy_towers))
            {
                if (!tw.isDead() && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(tw.getGridX(), tw.getGridY(), c[0], c[1]) <= 10 * 10)
                {
                    danger = (float)(danger + enemyTowerValue(tw));
                }
            }
            string abort = (danger > (float)(0.8f * ours) ? "hunt_abort_danger" : ((float)(ai.time() - chase_start) > s.chief_hunt_time ? "hunt_abort_time" : (squad.size() < 3 ? "hunt_abort_small" : (com.oddlabs.tt.player.gauntlet.Combat.countNear(intel.enemy_warriors, t.getGridX(), t.getGridY(), 15) > s.chief_hunt_escort + 2 ? "hunt_abort_escort" : (string)(null)))));
            if (abort == null)
            {
                countBases();
                if (!huntable(t.getOwner()))
                {
                    abort = "hunt_abort_changed";
                }
            }
            if (abort != null)
            {
                ai.aiLog().count(abort);
                chase_banned.put(t, ai.time());
                endChase(squad);
                return;
            }
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(squad))
            {
                com.oddlabs.tt.player.gauntlet.Intel.WarriorState st = ((com.oddlabs.tt.player.gauntlet.Intel.WarriorState)intel.warrior_states.get(u));
                if (((object)st == (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorState.FIGHT) || ((object)st == (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorState.STUNNED) || ((object)st == (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorState.ENTER))
                {
                    continue;
                }
                float? last = ((float?)last_order.get(u));
                if (last != null && (float)(ai.time() - ((float)last)) < 2.0f)
                {
                    continue;
                }
                last_order.put(u, ai.time());
                ai.owner().setTarget(com.oddlabs.tt.model.Selectable.newArray(u), t, com.oddlabs.tt.model.Action.ATTACK, true);
            }
        }
        // Java: 4054
        public void endChase(java.util.List squad)
        {
            com.oddlabs.tt.player.gauntlet.Military.Role back = (((object)mode == (object)com.oddlabs.tt.player.gauntlet.Military.Mode.ATTACK) ? com.oddlabs.tt.player.gauntlet.Military.Role.ATTACK : com.oddlabs.tt.player.gauntlet.Military.Role.ARMY);
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(squad))
            {
                roles.put(u, back);
                if (((object)back == (object)com.oddlabs.tt.player.gauntlet.Military.Role.ARMY))
                {
                    move(u, staging_x, staging_y);
                }
            }
            chase_target = null;
            chase_owner = null;
            last_chase_end = ai.time();
        }
        // Java: 4066
        public void raid()
        {
            java.util.List squad = withRole(com.oddlabs.tt.player.gauntlet.Military.Role.RAID);
            if (squad.isEmpty())
            {
                return;
            }
            int[] c = com.oddlabs.tt.player.gauntlet.MapAnalysis.centroid(squad);
            float ours = com.oddlabs.tt.player.gauntlet.Combat.total(squad);
            float danger = withEnemyTowers(enemyStrengthNear(c[0], c[1], 24), c[0], c[1], 10);
            bool done = (float)(ai.time() - raid_start) > 150.0f;
            if (danger > (float)(0.8f * ours) || done || squad.size() < 2)
            {
                foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(squad))
                {
                    roles.put(u, com.oddlabs.tt.player.gauntlet.Military.Role.ARMY);
                    move(u, staging_x, staging_y);
                }
                last_raid_end = ai.time();
                return;
            }
            int[] peons = com.oddlabs.tt.player.gauntlet.Military.nearestGroup(ai.intel().enemy_peons, c[0], c[1], 18);
            if (peons != null)
            {
                foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(squad))
                {
                    attackGround(u_2, peons[0], peons[1], true);
                }
                return;
            }
            int[] next = raidSpot(c[0], c[1], ours);
            if (next != null && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(next[0], next[1], c[0], c[1]) < 70 * 70)
            {
                raid_x = next[0];
                raid_y = next[1];
            }
            foreach (com.oddlabs.tt.model.Unit u_3 in java.lang.JIter.of(squad))
            {
                attackGround(u_3, raid_x, raid_y, false);
            }
            if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(c[0], c[1], raid_x, raid_y) < 5 * 5 && peons == null)
            {
                foreach (com.oddlabs.tt.model.Unit u_4 in java.lang.JIter.of(squad))
                {
                    roles.put(u_4, com.oddlabs.tt.player.gauntlet.Military.Role.ARMY);
                    move(u_4, staging_x, staging_y);
                }
                last_raid_end = ai.time();
            }
        }
        // Java: 4108
        public java.util.List withRole(com.oddlabs.tt.player.gauntlet.Military.Role role)
        {
            java.util.List units = new java.util.ArrayList();
            foreach (java.util.Map.Entry e in java.lang.JIter.of(roles.entrySet()))
            {
                if (((object)((com.oddlabs.tt.player.gauntlet.Military.Role)e.getValue()) == (object)role))
                {
                    units.add(((com.oddlabs.tt.model.Unit)e.getKey()));
                }
            }
            return units;
        }
        // Java: 4116
        public int countRole(com.oddlabs.tt.player.gauntlet.Military.Role role)
        {
            int n = 0;
            foreach (com.oddlabs.tt.player.gauntlet.Military.Role r in java.lang.JIter.of(roles.values()))
            {
                if (((object)r == (object)role))
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 4128
        public void attackGround(com.oddlabs.tt.model.Unit u, int x, int y, bool urgent)
        {
            com.oddlabs.tt.player.gauntlet.Intel.WarriorState s = ((com.oddlabs.tt.player.gauntlet.Intel.WarriorState)ai.intel().warrior_states.get(u));
            if (((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorState.FIGHT) || ((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorState.STUNNED) || ((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorState.ENTER))
            {
                return;
            }
            float? last = ((float?)last_order.get(u));
            int[] last_spot = ((int[])last_spots.get(u));
            bool same_spot = last_spot != null && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(last_spot[0], last_spot[1], x, y) <= 3 * 3;
            float period = (urgent ? 1.0f : (same_spot ? (float)(2 * com.oddlabs.tt.player.gauntlet.Military.REORDER_PERIOD) : com.oddlabs.tt.player.gauntlet.Military.REORDER_PERIOD));
            if (last != null && (float)(ai.time() - ((float)last)) < period && ((object)s != (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorState.IDLE))
            {
                return;
            }
            if (((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorState.IDLE) && same_spot && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(u.getGridX(), u.getGridY(), x, y) <= 4 * 4)
            {
                return;
            }
            queueOrder(u, x, y, true);
        }
        // Java: 4148
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
                com.oddlabs.tt.model.Selectable nearest = null;
                int best = java.lang.Integer.MAX_VALUE;
                foreach (com.oddlabs.tt.model.Selectable e in java.lang.JIter.of(enemies))
                {
                    if (e.isDead())
                    {
                        continue;
                    }
                    int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(u.getGridX(), u.getGridY(), e.getGridX(), e.getGridY());
                    if (d < best)
                    {
                        best = d;
                        nearest = e;
                    }
                }
                if (nearest != null)
                {
                    attackGround(u, nearest.getGridX(), nearest.getGridY(), urgent);
                }
                else
                {
                    attackGround(u, fallback_x, fallback_y, urgent);
                }
            }
        }
        public const int THROW_CELLS = 9;
        // Java: 4184
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
            int r2 = com.oddlabs.tt.player.gauntlet.Military.THROW_CELLS * com.oddlabs.tt.player.gauntlet.Military.THROW_CELLS;
            foreach (com.oddlabs.tt.model.Unit w_2 in java.lang.JIter.of(units))
            {
                com.oddlabs.tt.player.gauntlet.Intel.WarriorState s = ((com.oddlabs.tt.player.gauntlet.Intel.WarriorState)ai.intel().warrior_states.get(w_2));
                if (((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorState.STUNNED) || ((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorState.ENTER) || w_2.isDead())
                {
                    continue;
                }
                com.oddlabs.tt.model.Unit current = ((com.oddlabs.tt.model.Unit)hunt_targets.get(w_2));
                bool hunting = current != null && (w_2.getCurrentController() is com.oddlabs.tt.model.behaviour.HuntController);
                if (hunting && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(w_2.getGridX(), w_2.getGridY(), current.getGridX(), current.getGridY()) <= (com.oddlabs.tt.player.gauntlet.Military.THROW_CELLS + 3) * (com.oddlabs.tt.player.gauntlet.Military.THROW_CELLS + 3))
                {
                    targeted.add(w_2);
                    continue;
                }
                com.oddlabs.tt.model.Unit best = null;
                float best_score = 0.0f;
                float best_p = 0.0f;
                foreach (com.oddlabs.tt.model.Unit e_3 in java.lang.JIter.of(foes))
                {
                    if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(w_2.getGridX(), w_2.getGridY(), e_3.getGridX(), e_3.getGridY()) > r2)
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
                ai.intel().warrior_states.put(w_2, com.oddlabs.tt.player.gauntlet.Intel.WarriorState.FIGHT);
                ai.owner().setTarget(com.oddlabs.tt.model.Selectable.newArray(w_2), best, com.oddlabs.tt.model.Action.ATTACK, true);
                targeted.add(w_2);
            }
            return targeted;
        }
        // Java: 4244
        public void armyReflex()
        {
            if (!ai.strategy().army_reflex || hunt_targets.isEmpty())
            {
                return;
            }
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            java.util.List retarget_v = null;
            foreach (java.util.Map.Entry e in java.lang.JIter.of(hunt_targets.entrySet()))
            {
                if (!((com.oddlabs.tt.model.Unit)e.getKey()).isDead() && ((com.oddlabs.tt.model.Unit)e.getValue()).isDead())
                {
                    if (retarget_v == null)
                    {
                        retarget_v = new java.util.ArrayList();
                    }
                    retarget_v.add(((com.oddlabs.tt.model.Unit)e.getKey()));
                }
            }
            if (retarget_v == null)
            {
                return;
            }
            int r2 = com.oddlabs.tt.player.gauntlet.Military.THROW_CELLS * com.oddlabs.tt.player.gauntlet.Military.THROW_CELLS;
            foreach (com.oddlabs.tt.model.Unit w in java.lang.JIter.of(retarget_v))
            {
                hunt_targets.remove(w);
                com.oddlabs.tt.player.gauntlet.Military.Role role = ((com.oddlabs.tt.player.gauntlet.Military.Role)roles.get(w));
                if (w.isMounted() || com.oddlabs.tt.player.gauntlet.Intel.isStunned(w) || ((object)role == (object)com.oddlabs.tt.player.gauntlet.Military.Role.TOWER) || (((object)mode == (object)com.oddlabs.tt.player.gauntlet.Military.Mode.RETREAT) && ((object)role == (object)com.oddlabs.tt.player.gauntlet.Military.Role.ATTACK)))
                {
                    continue;
                }
                com.oddlabs.tt.model.Unit best = null;
                float best_score = 0.0f;
                foreach (java.util.List group in java.lang.JIter.of(java.util.List_.of(intel.enemy_warriors, intel.enemy_chieftains, intel.enemy_peons)))
                {
                    foreach (com.oddlabs.tt.model.Unit e_2 in java.lang.JIter.of(group))
                    {
                        if (e_2.isDead() || com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(w.getGridX(), w.getGridY(), e_2.getGridX(), e_2.getGridY()) > r2)
                        {
                            continue;
                        }
                        int others = 0;
                        foreach (com.oddlabs.tt.model.Unit o in java.lang.JIter.of(hunt_targets.values()))
                        {
                            if (((object)o == (object)e_2))
                            {
                                others++;
                            }
                        }
                        float score = (float)((float)(throwValue(w, e_2) * hitChance(w, e_2)) / (1 << java.lang.Math.min(others, 4)));
                        if (score > best_score)
                        {
                            best_score = score;
                            best = e_2;
                        }
                    }
                }
                if (best == null)
                {
                    continue;
                }
                hunt_targets.put(w, best);
                last_order.put(w, ai.time());
                ai.owner().setTarget(com.oddlabs.tt.model.Selectable.newArray(w), best, com.oddlabs.tt.model.Action.ATTACK, true);
                ai.aiLog().count("army_reflex");
            }
        }
        // Java: 4291
        public bool isMultiHit(com.oddlabs.tt.model.Unit e)
        {
            return ai.strategy().chief_per_hit && e.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC);
        }
        // Java: 4296
        public float survival(java.util.Map survive, com.oddlabs.tt.model.Unit e)
        {
            return (isMultiHit(e) ? 1.0f : ((float)((float?)survive.getOrDefault(e, 1.0f))));
        }
        // Java: 4305
        public float throwValue(com.oddlabs.tt.model.Unit thrower, com.oddlabs.tt.model.Unit e)
        {
            if (!isMultiHit(e))
            {
                return com.oddlabs.tt.player.gauntlet.Combat.lastingValue(e);
            }
            int damage = (thrower.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.THROW) && ((object)com.oddlabs.tt.player.gauntlet.Intel.warriorType(thrower) != (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorType.ROCK) ? 2 : 1);
            int hits = java.lang.Math.max(1, (e.getHitPoints() + damage - 1) / damage);
            return java.lang.Math.min(6.0f, (float)(25.0f / hits));
        }
        public const int TOWER_CELLS = 15;
        public const int GARRISON_REACH2 = 252;
        // Java: 4324
        public int towerReach2()
        {
            return (ai.strategy().tower_full_reach || ai.strategy().tower_gunner_reach ? com.oddlabs.tt.player.gauntlet.Military.GARRISON_REACH2 : com.oddlabs.tt.player.gauntlet.Military.TOWER_CELLS * com.oddlabs.tt.player.gauntlet.Military.TOWER_CELLS);
        }
        // Java: 4330
        public static com.oddlabs.tt.model.Unit readyGunner(com.oddlabs.tt.model.Building t)
        {
            if (t.isDead() || !t.isComplete() || t.getUnitContainer() == null || t.getUnitCount() == 0)
            {
                return null;
            }
            com.oddlabs.tt.model.Unit gunner = com.oddlabs.tt.player.gauntlet.Intel.gunner(t);
            return (gunner == null || gunner.isDead() || com.oddlabs.tt.player.gauntlet.Intel.isStunned(gunner) ? (com.oddlabs.tt.model.Unit)(null) : gunner);
        }
        // Java: 4338
        public com.oddlabs.tt.model.Unit liveTarget(com.oddlabs.tt.model.Building t, int ox, int oy, int r2)
        {
            com.oddlabs.tt.model.Unit current = ((com.oddlabs.tt.model.Unit)tower_targets.get(t));
            return (current != null && !current.isDead() && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(ox, oy, current.getGridX(), current.getGridY()) <= r2 ? current : (com.oddlabs.tt.model.Unit)(null));
        }
        // Java: 4350
        public int originX(com.oddlabs.tt.model.Building t, com.oddlabs.tt.model.Unit gunner)
        {
            return (ai.strategy().tower_gunner_reach ? gunner.getGridX() : t.getGridX());
        }
        // Java: 4354
        public int originY(com.oddlabs.tt.model.Building t, com.oddlabs.tt.model.Unit gunner)
        {
            return (ai.strategy().tower_gunner_reach ? gunner.getGridY() : t.getGridY());
        }
        // Java: 4359
        public float towerSelfFactor(com.oddlabs.tt.model.Building t, com.oddlabs.tt.model.Unit e)
        {
            if (!ai.strategy().tower_self_first)
            {
                return 1.0f;
            }
            com.oddlabs.tt.model.behaviour.Controller c = e.getCurrentController();
            com.oddlabs.tt.model.Selectable target = ((c is com.oddlabs.tt.model.behaviour.AttackController a) ? a.getTarget() : ((c is com.oddlabs.tt.model.behaviour.HuntController h) ? h.getTarget() : (com.oddlabs.tt.model.Selectable)(null)));
            return (((object)target == (object)t) ? 2.0f : 1.0f);
        }
        // Java: 4372
        public void towerReflex()
        {
            com.oddlabs.tt.player.gauntlet.Strategy strategy = ai.strategy();
            if ((!strategy.tower_reflex && !strategy.tower_prequeue) || !strategy.tower_fire)
            {
                return;
            }
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            int r2 = towerReach2();
            if (!inflight.isEmpty())
            {
                inflight.entrySet().removeIf(new java.util.function.Predicate_L((object e_l) =>
                {
                    java.util.Map.Entry e = (java.util.Map.Entry)e_l;
                    return ((com.oddlabs.tt.model.Unit)e.getKey()).isDead() || (float)(ai.time() - ((float)((float?)e.getValue()))) > 2.5f;
                }));
            }
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(intel.towers))
            {
                com.oddlabs.tt.model.Unit gunner = com.oddlabs.tt.player.gauntlet.Military.readyGunner(t);
                if (gunner == null)
                {
                    continue;
                }
                int ox = originX(t, gunner);
                int oy = originY(t, gunner);
                if (strategy.tower_prequeue && prequeue(t, gunner, ox, oy, r2))
                {
                    continue;
                }
                if (!strategy.tower_reflex || liveTarget(t, ox, oy, r2) != null)
                {
                    continue;
                }
                com.oddlabs.tt.model.Unit best = bestTowerTarget(t, gunner, ox, oy, r2, null);
                if (best == null)
                {
                    tower_targets.remove(t);
                    continue;
                }
                tower_targets.put(t, best);
                ai.owner().setTarget(com.oddlabs.tt.model.Selectable.newArray(t), best, com.oddlabs.tt.model.Action.ATTACK, false);
                ai.aiLog().count("tower_reflex");
            }
        }
        public java.util.Map inflight;
        public java.util.Map tower_throws;
        public java.util.Map tower_queued;
        // Java: 4413
        public bool prequeue(com.oddlabs.tt.model.Building t, com.oddlabs.tt.model.Unit gunner, int ox, int oy, int r2)
        {
            com.oddlabs.tt.model.behaviour.Behaviour b = gunner.getCurrentBehaviour();
            if (!((b is com.oddlabs.tt.model.behaviour.AttackBehaviour)))
            {
                tower_throws.remove(t);
                tower_queued.remove(t);
                return false;
            }
            if (((object)((com.oddlabs.tt.model.behaviour.Behaviour)tower_throws.get(t)) == (object)b))
            {
                return false;
            }
            tower_throws.put(t, b);
            if (!((gunner.getCurrentController() is com.oddlabs.tt.model.behaviour.AttackController a)) || !((a.getTarget() is com.oddlabs.tt.model.Unit x)) || x.isDead() || isMultiHit(x))
            {
                return false;
            }
            float px = towerHitChance(gunner, t, x);
            if (px < 0.99f && !ai.strategy().tower_prequeue_any)
            {
                return false;
            }
            com.oddlabs.tt.player.gauntlet.Intel.WarriorType type = com.oddlabs.tt.player.gauntlet.Intel.warriorType(gunner);
            float fly = (((object)type == (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorType.ROCK) ? 9.5f : (((object)type == (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorType.CHICKEN) ? 14.5f : 12.0f));
            if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(t.getGridX(), t.getGridY(), x.getGridX(), x.getGridY()) <= (float)(fly * fly))
            {
                return false;
            }
            if (px >= 0.99f)
            {
                inflight.put(x, ai.time());
            }
            int queued = ((int)((int?)tower_queued.getOrDefault(t, 0)));
            if (queued >= 20)
            {
                return false;
            }
            com.oddlabs.tt.model.Unit next = bestTowerTarget(t, gunner, ox, oy, r2, x);
            if (next == null)
            {
                return false;
            }
            tower_queued.put(t, queued + 1);
            tower_targets.put(t, next);
            ai.owner().setTarget(com.oddlabs.tt.model.Selectable.newArray(t), next, com.oddlabs.tt.model.Action.ATTACK, false);
            ai.aiLog().count("tower_prequeue");
            return true;
        }
        // Java: 4452
        public com.oddlabs.tt.model.Unit bestTowerTarget(com.oddlabs.tt.model.Building t, com.oddlabs.tt.model.Unit gunner, int ox, int oy, int r2, com.oddlabs.tt.model.Unit skip)
        {
            com.oddlabs.tt.model.Unit best = null;
            float best_score = 0.0f;
            com.oddlabs.tt.player.gauntlet.EnemyIndex index = enemyIndex();
            int[] near = index.query(ox, oy, r2);
            for (int k = 0, n = index.count(); k < n; k++)
            {
                com.oddlabs.tt.model.Unit e = index.unit(near[k]);
                if (((object)e == (object)skip) || e.isDead() || inflight.containsKey(e))
                {
                    continue;
                }
                int others = 0;
                foreach (com.oddlabs.tt.model.Unit other in java.lang.JIter.of(tower_targets.values()))
                {
                    if (((object)other == (object)e))
                    {
                        others++;
                    }
                }
                float score = (float)((float)((float)(throwValue(gunner, e) * towerSelfFactor(t, e)) * towerHitChance(gunner, t, e)) / (1 << java.lang.Math.min(others, 4)));
                if (score > best_score)
                {
                    best_score = score;
                    best = e;
                }
            }
            return best;
        }
        // Java: 4481
        public void towerFire()
        {
            if (!ai.strategy().tower_fire)
            {
                return;
            }
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            tower_targets.entrySet().removeIf(new java.util.function.Predicate_L((object e_l) =>
            {
                java.util.Map.Entry e = (java.util.Map.Entry)e_l;
                return ((com.oddlabs.tt.model.Building)e.getKey()).isDead() || ((com.oddlabs.tt.model.Unit)e.getValue()).isDead();
            }));
            java.util.Map survive = new java.util.LinkedHashMap();
            int r2 = towerReach2();
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(intel.towers))
            {
                if (!t.isComplete() || t.getUnitCount() == 0)
                {
                    continue;
                }
                com.oddlabs.tt.model.Unit gunner = com.oddlabs.tt.player.gauntlet.Intel.gunner(t);
                if (gunner == null || gunner.isDead() || com.oddlabs.tt.player.gauntlet.Intel.isStunned(gunner))
                {
                    continue;
                }
                int ox = originX(t, gunner);
                int oy = originY(t, gunner);
                com.oddlabs.tt.model.Unit current = ((com.oddlabs.tt.model.Unit)tower_targets.get(t));
                if (current != null && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(ox, oy, current.getGridX(), current.getGridY()) <= r2)
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
                com.oddlabs.tt.player.gauntlet.EnemyIndex index = enemyIndex();
                int[] near = index.query(ox, oy, r2);
                for (int k = 0, n = index.count(); k < n; k++)
                {
                    com.oddlabs.tt.model.Unit e_2 = index.unit(near[k]);
                    if (e_2.isDead() || inflight.containsKey(e_2))
                    {
                        continue;
                    }
                    float @value = (float)(throwValue(gunner, e_2) * towerSelfFactor(t, e_2));
                    if (index.isPeon(near[k]) && nearOwnTower(e_2))
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
        // Java: 4536
        public com.oddlabs.tt.model.Unit towerTargetFor(com.oddlabs.tt.model.Building t, com.oddlabs.tt.model.Unit gunner)
        {
            int r2 = towerReach2();
            int ox = originX(t, gunner);
            int oy = originY(t, gunner);
            com.oddlabs.tt.model.Unit current = liveTarget(t, ox, oy, r2);
            if (current != null)
            {
                return current;
            }
            com.oddlabs.tt.model.Unit best = null;
            float best_score = 0.0f;
            com.oddlabs.tt.player.gauntlet.EnemyIndex index = enemyIndex();
            int[] near = index.query(ox, oy, r2);
            for (int k = 0, n = index.count(); k < n; k++)
            {
                com.oddlabs.tt.model.Unit e = index.unit(near[k]);
                if (e.isDead())
                {
                    continue;
                }
                float score = (float)(throwValue(gunner, e) * towerHitChance(gunner, t, e));
                if (score > best_score)
                {
                    best_score = score;
                    best = e;
                }
            }
            if (best != null)
            {
                tower_targets.put(t, best);
            }
            return best;
        }
        // Java: 4563
        public bool nearOwnTower(com.oddlabs.tt.model.Unit e)
        {
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(ai.intel().towers))
            {
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(t.getGridX(), t.getGridY(), e.getGridX(), e.getGridY()) <= 3 * 3)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 4571
        public float towerHitChance(com.oddlabs.tt.model.Unit gunner, com.oddlabs.tt.model.Building tower, com.oddlabs.tt.model.Unit dst)
        {
            float p = (float)((float)(3.0f * throwSkill(gunner, tower.getGridX(), tower.getGridY(), dst)) * ((float)(1.0f - dst.getDefenseChance())));
            return java.lang.Math.clamp(p, 0.0f, 1.0f);
        }
        // Java: 4577
        public float hitChance(com.oddlabs.tt.model.Unit src, com.oddlabs.tt.model.Unit dst)
        {
            float p = (float)(throwSkill(src, src.getGridX(), src.getGridY(), dst) * ((float)(1.0f - dst.getDefenseChance())));
            return java.lang.Math.clamp(p, 0.0f, 1.0f);
        }
        // Java: 4586
        public float throwSkill(com.oddlabs.tt.model.Unit src, int x, int y, com.oddlabs.tt.model.Unit dst)
        {
            com.oddlabs.tt.player.gauntlet.MapAnalysis map = ai.map();
            float dz = (float)(map.height(x, y) - map.height(dst.getGridX(), dst.getGridY()));
            float terrain = java.lang.Math.clamp((float)(dz / 80.0f), (float)(-0.25f), 0.25f);
            return (float)((float)(src.getOwner().getHitBonus() + terrain) + com.oddlabs.tt.player.gauntlet.Military.baseHitChance(src));
        }
        // Java: 4593
        public static float baseHitChance(com.oddlabs.tt.model.Unit src)
        {
            if (!src.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.THROW))
            {
                return 0.2f;
            }
            return ((com.oddlabs.tt.player.gauntlet.Intel.warriorType(src)).ordinal() switch { 0 /*ROCK*/ => (float)(0.5f), 1 /*IRON*/ => (float)(0.75f), 2 /*CHICKEN*/ => (float)(0.95f), _ => throw new System.InvalidOperationException("MatchException") });
        }
        // Java: 4607
        public void restoreDodge()
        {
            if (!ai.strategy().restore_dodge)
            {
                return;
            }
            dodge_orders.keySet().removeIf(new java.util.function.Predicate_L((object a0) => { return ((com.oddlabs.tt.model.Unit)a0).isDead(); }));
            foreach (com.oddlabs.tt.model.Unit w in java.lang.JIter.of(ai.intel().warriors))
            {
                if (!com.oddlabs.tt.player.gauntlet.Intel.isDefenseless(w))
                {
                    continue;
                }
                float? last = ((float?)dodge_orders.get(w));
                if (last != null && (float)(ai.time() - ((float)last)) < ai.strategy().restore_dodge_gap)
                {
                    continue;
                }
                dodge_orders.put(w, ai.time());
                queueOrder(w, w.getGridX(), w.getGridY(), true);
            }
        }
        // Java: 4622
        public void move(com.oddlabs.tt.model.Unit u, int x, int y)
        {
            queueOrder(u, x, y, false);
        }
        // Java: 4630
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
        // Java: 4637
        public void flushOrders()
        {
            foreach (java.util.Map.Entry e in java.lang.JIter.of(pending_orders.entrySet()))
            {
                long key = ((long)((long?)e.getKey()));
                int x = unchecked((int)((key >> 32)));
                int y = unchecked((int)(((key & 4294967295L) >> 1)));
                bool aggressive = (key & 1) != 0;
                java.util.List units = ((java.util.List)e.getValue());
                ai.landscapeOrder(((com.oddlabs.tt.model.Selectable[])units.toArray(new com.oddlabs.tt.model.Selectable[0])), x, y, (aggressive ? com.oddlabs.tt.model.Action.ATTACK : com.oddlabs.tt.model.Action.MOVE), aggressive);
            }
            pending_orders.clear();
        }
        // Java: 4650
        public int stagingX()
        {
            return staging_x;
        }
        // Java: 4654
        public int stagingY()
        {
            return staging_y;
        }
        // Java: 4659
        public float withEnemyTowers(float s, int x, int y, int r)
        {
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(ai.intel().enemy_towers))
            {
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(t.getGridX(), t.getGridY(), x, y) <= r * r)
                {
                    s = (float)(s + enemyTowerValue(t));
                }
            }
            return s;
        }
        // Java: 4667
        public float enemyTowerValue(com.oddlabs.tt.model.Building tower)
        {
            return (float)(com.oddlabs.tt.player.gauntlet.Combat.towerValue(tower) * ai.strategy().tower_weight);
        }
        // Java: 4671
        public float threatStrength()
        {
            return threat_strength;
        }
        // Java: 4675
        public int threatX()
        {
            return threat_x;
        }
        // Java: 4679
        public int threatY()
        {
            return threat_y;
        }
        // Java: 4683
        public bool anyFighting(java.util.List units)
        {
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(units))
            {
                if (((object)((com.oddlabs.tt.player.gauntlet.Intel.WarriorState)ai.intel().warrior_states.get(u)) == (object)com.oddlabs.tt.player.gauntlet.Intel.WarriorState.FIGHT))
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 4691
        public int[] attackCenter()
        {
            java.util.List army = withRole(com.oddlabs.tt.player.gauntlet.Military.Role.ATTACK);
            return (army.isEmpty() ? (int[])(null) : com.oddlabs.tt.player.gauntlet.MapAnalysis.centroid(army));
        }
        // Java: 4696
        public string debugStatus()
        {
            return "mode=" + java.lang.JStr.valueOf(mode) + " army=" + (countRole(com.oddlabs.tt.player.gauntlet.Military.Role.ARMY)) + " atk=" + (countRole(com.oddlabs.tt.player.gauntlet.Military.Role.ATTACK)) + " raid=" + (countRole(com.oddlabs.tt.player.gauntlet.Military.Role.RAID)) + " twr=" + (countRole(com.oddlabs.tt.player.gauntlet.Military.Role.TOWER)) + java.lang.JStr.valueOf(java.lang.JString.format(" str=%.1f thr=%d/%.1f", armyStrength(), threat_level, threat_strength)) + java.lang.JStr.valueOf(parkedStatus());
        }
        // Java: 4710
        public void describeJam(int cx, int cy)
        {
            if (!ai.logging())
            {
                return;
            }
            com.oddlabs.tt.player.gauntlet.MapAnalysis map = ai.map();
            com.oddlabs.tt.pathfinder.UnitGrid grid = map.getGrid();
            int rx = 32;
            int ry = 22;
            java.util.Map walk_targets = new java.util.LinkedHashMap();
            int[] kinds = new int[6];
            java.lang.StringBuilder head = new java.lang.StringBuilder("jampic head x0=" + ((cx - rx)) + " y0=" + ((cy - ry)) + " mode=" + java.lang.JStr.valueOf(mode) + " target=" + (target_x) + "," + (target_y) + " wp=" + java.lang.JStr.valueOf(((march_wp != null ? (march_wp[0]) + "," + (march_wp[1]) : "-"))));
            com.oddlabs.tt.player.gauntlet.DistanceField f = target_field;
            if (f != null)
            {
                com.oddlabs.tt.player.gauntlet.DistanceField fresh = map.computeField(target_x, target_y, java.lang.Integer.MAX_VALUE);
                head.append(" field@c=").append(f.getAround(cx, cy, 1)).append(" fresh@c=").append(fresh.getAround(cx, cy, 1));
                if (march_wp != null)
                {
                    head.append(" field@wp=").append(f.getAround(march_wp[0], march_wp[1], 1)).append(" fresh@wp=").append(fresh.getAround(march_wp[0], march_wp[1], 1));
                }
            }
            ai.log(java.lang.JO.toString(head));
            for (int y = cy - ry; y <= cy + ry; y++)
            {
                java.lang.StringBuilder row = new java.lang.StringBuilder();
                for (int x = cx - rx; x <= cx + rx; x++)
                {
                    char ch = default;
                    if (march_wp != null && x == march_wp[0] && y == march_wp[1])
                    {
                        ch = 'W';
                    }
                    else if (!map.walkable(x, y))
                    {
                        ch = '#';
                    }
                    else
                    {
                        com.oddlabs.tt.pathfinder.Occupant occ = grid.getOccupant(x, y);
                        if (occ == null)
                        {
                            ch = (f != null && !f.reachable(x, y) ? ',' : '.');
                        }
                        else if ((occ is com.oddlabs.tt.model.Unit u) && !u.isDead())
                        {
                            if (((object)u.getOwner() == (object)ai.owner()))
                            {
                                com.oddlabs.tt.player.gauntlet.Military.Role role = ((com.oddlabs.tt.player.gauntlet.Military.Role)roles.get(u));
                                com.oddlabs.tt.model.behaviour.Behaviour b = u.getCurrentBehaviour();
                                if (((object)role == (object)com.oddlabs.tt.player.gauntlet.Military.Role.ATTACK))
                                {
                                    if ((b is com.oddlabs.tt.model.behaviour.WalkBehaviour))
                                    {
                                        ch = (b.isBlocking() ? 'b' : 'w');
                                        kinds[(b.isBlocking() ? 0 : 1)]++;
                                    }
                                    else if ((b is com.oddlabs.tt.model.behaviour.IdleBehaviour))
                                    {
                                        ch = 'i';
                                        kinds[2]++;
                                    }
                                    else
                                    {
                                        ch = 'f';
                                        kinds[3]++;
                                    }
                                    if ((u.getPrimaryController() is com.oddlabs.tt.model.behaviour.WalkController wc))
                                    {
                                        string k = (wc.getTarget().getGridX()) + "," + (wc.getTarget().getGridY());
                                        walk_targets.merge(k, 1, new java.util.function.BiFunction_L((object a0, object a1) => { return java.lang.Integer.sum(((int)a0), ((int)a1)); }));
                                    }
                                }
                                else
                                {
                                    ch = 'o';
                                    kinds[4]++;
                                }
                            }
                            else
                            {
                                ch = 'e';
                                kinds[5]++;
                            }
                        }
                        else if ((occ is com.oddlabs.tt.model.Building))
                        {
                            ch = 'B';
                        }
                        else if (occ.getPenalty() >= com.oddlabs.tt.pathfinder.Occupant.STATIC)
                        {
                            ch = 'T';
                        }
                        else
                        {
                            ch = '?';
                        }
                    }
                    row.append(ch);
                }
                ai.log(java.lang.JString.format("jampic %4d %s", y, row));
            }
            ai.log("jampic kinds blocked=" + (kinds[0]) + " walking=" + (kinds[1]) + " idle=" + (kinds[2]) + " other=" + (kinds[3]) + " own_other=" + (kinds[4]) + " enemy=" + (kinds[5]) + " walk_targets=" + java.lang.JStr.valueOf(walk_targets));
        }
        // Java: 4785
        public float ringStrength()
        {
            float s = 0.0f;
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(ai.intel().enemy_warriors))
            {
                if (inRing(e))
                {
                    s = (float)(s + com.oddlabs.tt.player.gauntlet.Combat.@value(e));
                }
            }
            return s;
        }
        // Java: 4797
        public bool inRing(com.oddlabs.tt.model.Unit e)
        {
            return !e.isDead() && com.oddlabs.tt.player.gauntlet.Intel.isParked(e) && nearOwnBuilding(e.getGridX(), e.getGridY(), 45);
        }
        // Java: 4801
        public bool nearOwnBuilding(int x, int y, int cells)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            int r2 = cells * cells;
            foreach (java.util.List group in java.lang.JIter.of(java.util.List_.of(intel.armories, intel.quarters, intel.towers)))
            {
                foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(group))
                {
                    if (!b.isDead() && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(b.getGridX(), b.getGridY(), x, y) <= r2)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        // Java: 4812
        public void considerSweep()
        {
            com.oddlabs.tt.player.gauntlet.Strategy strategy = ai.strategy();
            float now = ai.time();
            if (((object)mode != (object)com.oddlabs.tt.player.gauntlet.Military.Mode.ATTACK) || sweep_until >= 0.0f || now < strategy.ring_sweep_from || now > strategy.ring_sweep_until || (float)(now - last_out_time) < strategy.ring_sweep_quiet)
            {
                return;
            }
            float ring = ringStrength();
            float away = attackStrength();
            if (ring < 3.0f || away < (float)(strategy.ring_sweep_ratio * ring))
            {
                return;
            }
            int[] c = attackCenter();
            com.oddlabs.tt.model.Building armory = ai.intel().armory();
            if (c == null || armory == null || com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(c[0], c[1], armory.getGridX(), armory.getGridY()) > strategy.ring_sweep_reach * strategy.ring_sweep_reach)
            {
                return;
            }
            if (target != null && target.getOwner() != null)
            {
                int standing = 0;
                foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(ai.intel().enemy_buildings))
                {
                    if (!b.isDead() && b.isComplete() && ((object)b.getOwner() == (object)target.getOwner()))
                    {
                        standing++;
                    }
                }
                if (standing < 2)
                {
                    return;
                }
            }
            ai.aiLog().count("ring_sweep");
            ai.log(java.lang.JString.format("ring sweep: attack %.1f comes home against a parked ring of %.1f", away, ring));
            endAttack();
            sweep_until = (float)(strategy.ring_sweep_until + 60.0f);
            sweep_ring_start = ring;
        }
        // Java: 4846
        public void sweep()
        {
            float now = ai.time();
            float ring = ringStrength();
            float army = armyStrength();
            if (now > sweep_until || army < java.lang.Math.max(12.0f, (float)(0.8f * ring)) || ring < java.lang.Math.max(3.0f, (float)(0.3f * sweep_ring_start)))
            {
                sweep_until = (float)(-1.0f);
                ai.aiLog().count("ring_sweep_end");
                ai.log(java.lang.JString.format("ring sweep over: ring %.1f, army %.1f", ring, army));
                holdStaging();
                return;
            }
            if (threat_level >= 2)
            {
                return;
            }
            com.oddlabs.tt.model.Building armory = ai.intel().armory();
            int ax = (armory != null ? armory.getGridX() : staging_x);
            int ay = (armory != null ? armory.getGridY() : staging_y);
            com.oddlabs.tt.model.Unit prey = null;
            int best = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(ai.intel().enemy_warriors))
            {
                if (!inRing(e))
                {
                    continue;
                }
                int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(ax, ay, e.getGridX(), e.getGridY());
                if (d < best)
                {
                    best = d;
                    prey = e;
                }
            }
            if (prey == null)
            {
                holdStaging();
                return;
            }
            int ordered = 0;
            foreach (java.util.Map.Entry e_2 in java.lang.JIter.of(roles.entrySet()))
            {
                if (((object)((com.oddlabs.tt.player.gauntlet.Military.Role)e_2.getValue()) != (object)com.oddlabs.tt.player.gauntlet.Military.Role.ARMY))
                {
                    continue;
                }
                attackGround(((com.oddlabs.tt.model.Unit)e_2.getKey()), prey.getGridX(), prey.getGridY(), false);
                ordered++;
            }
            if (ordered > 0)
            {
                ai.aiLog().count("ring_sweep_order");
            }
        }
        // Java: 4892
        public string parkedStatus()
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            java.util.List own = intel.finishedBuildings();
            int @out = 0;
            int @in = 0;
            int[] by_building = new int[4];
            int[] by_tower = new int[4];
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(intel.enemy_warriors))
            {
                if (!inRing(e))
                {
                    continue;
                }
                bool reach = false;
                foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(intel.towers))
                {
                    if (com.oddlabs.tt.player.gauntlet.Intel.isTowerActive(t) && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(t.getGridX(), t.getGridY(), e.getGridX(), e.getGridY()) <= 15 * 15)
                    {
                        reach = true;
                        break;
                    }
                }
                if (reach)
                {
                    @in++;
                }
                else
                {
                    @out++;
                }
                int nb = java.lang.Integer.MAX_VALUE;
                foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(own))
                {
                    if (!b.isDead())
                    {
                        nb = java.lang.Math.min(nb, com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(b.getGridX(), b.getGridY(), e.getGridX(), e.getGridY()));
                    }
                }
                int nt = java.lang.Integer.MAX_VALUE;
                foreach (com.oddlabs.tt.model.Building t_2 in java.lang.JIter.of(intel.towers))
                {
                    if (com.oddlabs.tt.player.gauntlet.Intel.isTowerActive(t_2))
                    {
                        nt = java.lang.Math.min(nt, com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(t_2.getGridX(), t_2.getGridY(), e.getGridX(), e.getGridY()));
                    }
                }
                by_building[(nb <= 8 * 8 ? 0 : (nb <= 16 * 16 ? 1 : (nb <= 28 * 28 ? 2 : 3)))]++;
                by_tower[(nt <= 15 * 15 ? 0 : (nt <= 25 * 25 ? 1 : (nt <= 45 * 45 ? 2 : 3)))]++;
            }
            int manned = 0;
            foreach (com.oddlabs.tt.model.Building t_3 in java.lang.JIter.of(intel.towers))
            {
                if (com.oddlabs.tt.player.gauntlet.Intel.isTowerActive(t_3))
                {
                    manned++;
                }
            }
            return " park=" + (@out) + "/" + (@in) + " man=" + (manned) + "/" + (intel.towers.size()) + " pb=" + java.lang.JStr.valueOf(java.util.Arrays.toString(by_building).replace(" ", "")) + " pt=" + java.lang.JStr.valueOf(java.util.Arrays.toString(by_tower).replace(" ", ""));
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
            this.lent = new java.util.LinkedHashMap();
            this.mode = com.oddlabs.tt.player.gauntlet.Military.Mode.HOME;
            this.staging_time = (float)(-100.0f);
            this.threats = new java.util.ArrayList();
            this.raiders = new java.util.ArrayList();
            this.stalled_targets = new java.util.LinkedHashMap();
            this.dead_regions = new java.util.ArrayList();
            this.dead_region_times = new java.util.ArrayList();
            this.sweep_until = (float)(-1.0f);
            this.last_alive = (-1);
            this.frozen_deferred = new java.util.ArrayList();
            this.worn_history = new java.util.ArrayDeque();
            this.hold_until = (float)(-1.0f);
            this.best_target_dist = java.lang.Integer.MAX_VALUE;
            this.hold_spot = new int[2];
            this.last_enemy_d2 = java.lang.Integer.MAX_VALUE;
            this.hold_groups = new java.util.ArrayDeque();
            this.last_hold_skip = (float)(-100.0f);
            this.last_charge_log = (float)(-100.0f);
            this.hunt_targets = new java.util.LinkedHashMap();
            this.dodge_orders = new java.util.LinkedHashMap();
            this.tower_targets = new java.util.LinkedHashMap();
            this.sapper_orders = new java.util.LinkedHashMap();
            this.last_pillage_log = (float)(-100.0f);
            this.enemy_casts = new java.util.LinkedHashMap();
            this.enemy_chief_seen = new java.util.LinkedHashMap();
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
            this.last_provoke_probe = (float)(-10.0f);
            this.front_entry = new java.util.LinkedHashMap();
            this.reaimed = new java.util.LinkedHashMap();
            this.last_reaim = (float)(-10.0f);
            this.engage_since = (float)(-10.0f);
            this.march_calm_time = (float)(-100.0f);
            this.column_until = (float)(-1.0f);
            this.jam_first_dist = com.oddlabs.tt.player.gauntlet.DistanceField.UNREACHABLE;
            this.column_start_dist = com.oddlabs.tt.player.gauntlet.DistanceField.UNREACHABLE;
            this.chase_start = (float)(-1000.0f);
            this.last_chase_end = (float)(-1000.0f);
            this.chase_banned = new java.util.LinkedHashMap();
            this.qa_counts = new java.util.LinkedHashMap();
            this.inflight = new java.util.LinkedHashMap();
            this.tower_throws = new java.util.LinkedHashMap();
            this.tower_queued = new java.util.LinkedHashMap();
        }
        public Military(com.oddlabs.tt.player.gauntlet.GauntletAI ai) : base()
        {
            init_Military();
            this.ai = ai;
            staging_x = ai.planner().getStartX();
            staging_y = ai.planner().getStartY();
        }
    }
}
