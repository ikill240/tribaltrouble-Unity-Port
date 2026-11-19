// Translated from MilitaryManager.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.ultra
{
    // Java: 23
    public class MilitaryManager : java.lang.Object
    {
        // Java: 24
        public sealed class State : java.lang.Enum
        {
            public static State[] values() { return (State[])VALUES_.Clone(); }
            public static State valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly State GATHER = new State("GATHER", 0);
            public static readonly State MARCH = new State("MARCH", 1);
            public static readonly State ENGAGE = new State("ENGAGE", 2);
            public static readonly State SIEGE = new State("SIEGE", 3);
            public static readonly State RETREAT = new State("RETREAT", 4);
            public static readonly State[] VALUES_ = new State[] { GATHER, MARCH, ENGAGE, SIEGE, RETREAT };
            public State(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        public const int HOME = 0;
        public const int MAIN = 1;
        public const int REINF = 2;
        public const int GARRISON = 3;
        public const int RAID = 4;
        public const float UNIT_RANGE = 7.9f;
        public const float TOWER_TARGET_RANGE = 8.93f;
        public com.oddlabs.tt.player.ultra.Context ctx;
        public java.util.Map squad_of;
        public java.util.List home;
        public java.util.List main;
        public java.util.List reinf;
        public java.util.List raid;
        public com.oddlabs.tt.player.ultra.MilitaryManager.State state_f;
        public float state_since;
        public int[] gather;
        public float gather_time;
        public com.oddlabs.tt.model.Building objective;
        public int obj_x;
        public int obj_y;
        public int[] route;
        public int[] waypoint;
        public float leg_start;
        public float last_enemy_near;
        public java.util.List intruders;
        public float intruder_e;
        public int intr_x;
        public int intr_y;
        public bool defending;
        public float last_defend_order;
        public int attack_min;
        public float commit_ratio;
        public float grind_ratio;
        public float retreat_ratio;
        public int defend_radius;
        public int reinforce_batch;
        public int engage_range;
        public float cohesion_frac;
        public int leg_length;
        public float cap_attack_frac;
        public bool micro;
        public bool raid_enabled;
        public bool recall;
        public bool peon_strike;
        public int raid_size;
        // Java: 136
        public com.oddlabs.tt.player.ultra.MilitaryManager.State state()
        {
            return state_f;
        }
        // Java: 141
        public int[] rallyPoint()
        {
            return gather;
        }
        // Java: 149
        public int latentReserve()
        {
            if (weapon_buffer <= 0 || defending || ((object)state_f != (object)com.oddlabs.tt.player.ultra.MilitaryManager.State.GATHER) || ctx.now < muster_until)
            {
                return 0;
            }
            if (ctx.owner.getUnitCountContainer().getNumSupplies() >= ctx.world.getMaxUnitCount() - 10)
            {
                return 0;
            }
            return weapon_buffer;
        }
        public int weapon_buffer;
        public float enemy_latent_guess;
        public bool attack_learn;
        public bool stun_tower_focus;
        public float tower_linear;
        public float cover_weight;
        public bool stun_siege;
        public float learn_decay;
        public bool siege_evac;
        public float siege_hp;
        public bool march_watch;
        public bool dodge;
        public bool assault_gate;
        public bool frozen_window;
        public float toot_grace;
        public int frozen_min;
        public float frozen_reengage;
        public float assault_ratio;
        public float tower_lin_w;
        public int assault_r;
        public float assault_chief;
        public float assault_memory;
        public bool approach_towers;
        public float dodge_min;
        public float dodge_max;
        public float dodge_to;
        public float dodge_hold;
        public bool dodge_peons;
        public bool dodge_smart;
        public float defense_mult;
        public float last_decay;
        public float last_gather_trace;
        public bool attack_active;
        public int attack_lost0;
        public int attack_killed0;
        // Java: 205
        public void startAttack()
        {
            attack_active = true;
            attack_lost0 = ctx.owner.getUnitsLost();
            attack_killed0 = ctx.owner.getUnitsKilled();
        }
        // Java: 211
        public void endAttack()
        {
            if (!attack_active)
            {
                return;
            }
            attack_active = false;
            if (!attack_learn)
            {
                return;
            }
            int lost = ctx.owner.getUnitsLost() - attack_lost0;
            int killed = ctx.owner.getUnitsKilled() - attack_killed0;
            float old = defense_mult;
            if (lost >= 8 && lost > (float)(1.25f * killed))
            {
                defense_mult = java.lang.Math.min(3.0f, (float)(defense_mult * ((float)(1.0f + java.lang.Math.min(0.8f, (float)((lost - killed) / ((float)(lost))))))));
            }
            else if (killed >= lost)
            {
                defense_mult = java.lang.Math.max(1.0f, (float)(defense_mult * 0.88f));
            }
            float now = defense_mult;
            ctx.log(new java.util.function.Supplier_L(() =>
            {
                return java.lang.JString.format("attack over: lost %d killed %d, enemy defense x%.2f -> x%.2f", lost, killed, old, now);
            }));
        }
        public float enemy_chief_factor;
        public float muster_until;
        // Java: 234
        public int stockedIron()
        {
            com.oddlabs.tt.model.Building a = ctx.economy.armory();
            if (a == null || weapon_buffer <= 0)
            {
                return 0;
            }
            return java.lang.Math.min(a.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)).getNumSupplies(), a.getUnitContainer().getNumSupplies());
        }
        // Java: 242
        public bool isDefending()
        {
            return defending;
        }
        // Java: 250
        public int[] defenseFront()
        {
            if (!defending)
            {
                return null;
            }
            com.oddlabs.tt.player.ultra.MilitaryManager.Threat best = null;
            foreach (com.oddlabs.tt.player.ultra.MilitaryManager.Threat t in java.lang.JIter.of(threats))
            {
                if (!t.defenders.isEmpty() && (best == null || t.e > best.e))
                {
                    best = t;
                }
            }
            if (best == null)
            {
                return null;
            }
            long sx = 0;
            long sy = 0;
            foreach (com.oddlabs.tt.model.Unit w in java.lang.JIter.of(best.defenders))
            {
                sx += w.getGridX();
                sy += w.getGridY();
            }
            int n = best.defenders.size();
            return new int[] { unchecked((int)((sx / n))), unchecked((int)((sy / n))), best.cx, best.cy };
        }
        // Java: 271
        public int[] squadAnchor()
        {
            if (main.isEmpty() || ((object)state_f == (object)com.oddlabs.tt.player.ultra.MilitaryManager.State.GATHER))
            {
                return null;
            }
            com.oddlabs.tt.model.Unit u = anchor(main);
            return new int[] { u.getGridX(), u.getGridY() };
        }
        public float toot_until;
        public int best_prog;
        public float best_prog_t;
        public int leg_anchor_v;
        public java.util.Map stalled;
        public float frozen_until;
        public float recall_until;
        public bool toot_return;
        public bool closeout;
        public int toot_x;
        public int toot_y;
        // Java: 295
        public void onToot(int x, int y)
        {
            toot_order_time = ctx.now;
            toot_until = (float)(ctx.now + ((frozen_window ? toot_grace : 12.0f)));
            toot_x = x;
            toot_y = y;
            if (!main.isEmpty() && (((object)state_f == (object)com.oddlabs.tt.player.ultra.MilitaryManager.State.GATHER) || ((object)state_f == (object)com.oddlabs.tt.player.ultra.MilitaryManager.State.RETREAT) || ((object)state_f == (object)com.oddlabs.tt.player.ultra.MilitaryManager.State.MARCH)))
            {
                com.oddlabs.tt.model.Unit a = anchor(main);
                if (com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(a.getGridX(), a.getGridY(), x, y) <= 30)
                {
                    last_enemy_near = ctx.now;
                    if (((object)state_f != (object)com.oddlabs.tt.player.ultra.MilitaryManager.State.MARCH))
                    {
                        toot_return = true;
                    }
                    setState(com.oddlabs.tt.player.ultra.MilitaryManager.State.ENGAGE);
                }
            }
        }
        // Java: 312
        public int[] fightPoint()
        {
            if (tootActive() || ctx.now < frozen_until)
            {
                return new int[] { toot_x, toot_y };
            }
            if ((objective != null && !objective.isDead()) || closeout)
            {
                return new int[] { obj_x, obj_y };
            }
            return gather;
        }
        // Java: 320
        public bool tootActive()
        {
            return ctx.now < toot_until;
        }
        // Java: 325
        public com.oddlabs.tt.model.Building objectiveBuilding()
        {
            if (((object)state_f == (object)com.oddlabs.tt.player.ultra.MilitaryManager.State.GATHER) || ((object)state_f == (object)com.oddlabs.tt.player.ultra.MilitaryManager.State.RETREAT) || closeout)
            {
                return null;
            }
            return (objective != null && !objective.isDead() ? objective : (com.oddlabs.tt.model.Building)(null));
        }
        // Java: 333
        public bool holdingGround()
        {
            return main.size() >= 8 && (((object)state_f == (object)com.oddlabs.tt.player.ultra.MilitaryManager.State.SIEGE) || ((object)state_f == (object)com.oddlabs.tt.player.ultra.MilitaryManager.State.ENGAGE)) && localRatio(main) >= 1.3f;
        }
        // Java: 338
        public int[] objectivePoint()
        {
            if (((object)state_f == (object)com.oddlabs.tt.player.ultra.MilitaryManager.State.GATHER) || ((object)state_f == (object)com.oddlabs.tt.player.ultra.MilitaryManager.State.RETREAT) || (objective == null && !closeout))
            {
                return null;
            }
            return new int[] { obj_x, obj_y };
        }
        // Java: 344
        public int armySize()
        {
            return home.size() + main.size() + reinf.size() + raid.size();
        }
        // Java: 349
        public bool inDanger(int x, int y)
        {
            return ctx.resources.threatened(x, y, 10);
        }
        // Java: 353
        public bool quartersThreatened(com.oddlabs.tt.model.Building q)
        {
            return q.getHitPoints() < (float)(((com.oddlabs.tt.model.BuildingTemplate)q.getTemplate()).getMaxHitPoints() * 0.6f) && ctx.resources.threatened(q.getGridX(), q.getGridY(), 12);
        }
        // Java: 361
        public void rebuildSquads()
        {
            home.clear();
            main.clear();
            reinf.clear();
            raid.clear();
            foreach (com.oddlabs.tt.model.Unit w in java.lang.JIter.of(ctx.model.me.warriors))
            {
                int? s = ((int?)squad_of.get(w));
                if (s == null)
                {
                    s = com.oddlabs.tt.player.ultra.MilitaryManager.HOME;
                    squad_of.put(w, s);
                }
                switch (((int)(s)))
                {
                    case 1:
                    {
                        main.add(w);
                        break;
                    }
                    case 2:
                    {
                        reinf.add(w);
                        break;
                    }
                    case 4:
                    {
                        raid.add(w);
                        break;
                    }
                    case 3:
                    {
                        if (!ctx.towers.isPendingGarrison(w))
                        {
                            squad_of.put(w, com.oddlabs.tt.player.ultra.MilitaryManager.HOME);
                            home.add(w);
                        }
                        break;
                    }
                    default:
                    {
                        home.add(w);
                        break;
                    }
                }
            }
        }
        // Java: 387
        public void prune()
        {
            squad_of.keySet().removeIf(new java.util.function.Predicate_L((object u_l) =>
            {
                com.oddlabs.tt.model.Unit u = (com.oddlabs.tt.model.Unit)u_l;
                return u.isDead() || u.isMounted();
            }));
            stalled.entrySet().removeIf(new java.util.function.Predicate_L((object e_l) =>
            {
                java.util.Map.Entry e = (java.util.Map.Entry)e_l;
                return ((com.oddlabs.tt.model.Building)e.getKey()).isDead() || ctx.now >= ((float)((float?)e.getValue()));
            }));
        }
        // Java: 393
        public void readMultiParams()
        {
            siege_evac = ctx.strategy.b("siegeEvac");
            march_watch = ctx.strategy.b("marchWatch");
        }
        // Java: 398
        public bool enemyBuildingsLeft()
        {
            foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(ctx.model.enemy.buildings))
            {
                if (!b.isDead())
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 407
        public com.oddlabs.tt.model.Unit takeForGarrison(int x, int y)
        {
            com.oddlabs.tt.model.Unit near = takeForGarrison(x, y, 60);
            return (near != null ? near : takeForGarrison(x, y, java.lang.Integer.MAX_VALUE));
        }
        // Java: 413
        public com.oddlabs.tt.model.Unit takeForGarrison(int x, int y, int max_dist)
        {
            com.oddlabs.tt.model.Unit best = null;
            float best_score = java.lang.Float.NEGATIVE_INFINITY;
            java.util.List pool = com.oddlabs.tt.player.ultra.MilitaryManager.concat(com.oddlabs.tt.player.ultra.MilitaryManager.concat(home, main), reinf);
            foreach (com.oddlabs.tt.model.Unit w in java.lang.JIter.of(pool))
            {
                if (com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(w.getGridX(), w.getGridY(), x, y) > max_dist)
                {
                    continue;
                }
                com.oddlabs.tt.player.ultra.WorldModel.Kind k = com.oddlabs.tt.player.ultra.WorldModel.kindOf(w);
                float score = ((k).ordinal() switch { 3 /*RUBBER*/ => (float)(300.0f), 2 /*IRON*/ => (float)(200.0f), _ => (float)(100.0f) });
                score = (float)(score - ((float)(java.lang.Math.sqrt(com.oddlabs.tt.player.ultra.MapAnalysis.dist2(w.getGridX(), w.getGridY(), x, y)))));
                if (score > best_score)
                {
                    best_score = score;
                    best = w;
                }
            }
            if (best != null)
            {
                squad_of.put(best, com.oddlabs.tt.player.ultra.MilitaryManager.GARRISON);
                home.remove(best);
                main.remove(best);
                reinf.remove(best);
            }
            return best;
        }
        // Java: 442
        public void releaseFromGarrison(com.oddlabs.tt.model.Unit w)
        {
            if (!w.isDead() && !w.isMounted())
            {
                squad_of.put(w, com.oddlabs.tt.player.ultra.MilitaryManager.HOME);
            }
        }
        // Java: 447
        public static java.util.List concat(java.util.List a, java.util.List b)
        {
            java.util.List r = new java.util.ArrayList(a.size() + b.size());
            r.addAll(a);
            r.addAll(b);
            return r;
        }
        // Java: 458
        public class Threat : java.lang.Object
        {
            public java.util.List units;
            public java.util.List sites;
            public java.util.List defenders;
            public float e;
            public int cx;
            public int cy;
            private void init_Threat()
            {
                this.units = new java.util.ArrayList();
                this.sites = new java.util.ArrayList();
                this.defenders = new java.util.ArrayList();
            }
            public Threat() : base()
            {
                init_Threat();
            }
        }
        public java.util.List threats;
        // Java: 469
        public void threatTick()
        {
            rebuildSquads();
            updateGather();
            intruders.clear();
            intruder_e = 0.0f;
            java.util.List mine = ctx.model.me.buildings;
            com.oddlabs.tt.model.Building a = ctx.economy.armory();
            java.util.List hostile = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(ctx.model.enemy.units))
            {
                com.oddlabs.tt.player.ultra.WorldModel.Kind k = com.oddlabs.tt.player.ultra.WorldModel.kindOf(e);
                if (((object)k == (object)com.oddlabs.tt.player.ultra.WorldModel.Kind.PEON) && ((object)com.oddlabs.tt.player.ultra.WorldModel.visibleActivity(e) == (object)com.oddlabs.tt.player.ultra.WorldModel.Activity.GATHER))
                {
                    continue;
                }
                if (nearOurAssets(e.getGridX(), e.getGridY(), mine) || (((object)k != (object)com.oddlabs.tt.player.ultra.WorldModel.Kind.PEON) && nearOurPeons(e)))
                {
                    hostile.add(e);
                }
            }
            threats.clear();
            bool[] used = new bool[hostile.size()];
            for (int i = 0; i < hostile.size(); i++)
            {
                if (used[i])
                {
                    continue;
                }
                com.oddlabs.tt.player.ultra.MilitaryManager.Threat t = new com.oddlabs.tt.player.ultra.MilitaryManager.Threat();
                java.util.List queue = t.units;
                queue.add(((com.oddlabs.tt.model.Unit)hostile.get(i)));
                used[i] = true;
                for (int q = 0; q < queue.size(); q++)
                {
                    com.oddlabs.tt.model.Unit u = ((com.oddlabs.tt.model.Unit)queue.get(q));
                    for (int j = 0; j < hostile.size(); j++)
                    {
                        if (!used[j] && com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(u.getGridX(), u.getGridY(), ((com.oddlabs.tt.model.Unit)hostile.get(j)).getGridX(), ((com.oddlabs.tt.model.Unit)hostile.get(j)).getGridY()) <= 10)
                        {
                            used[j] = true;
                            queue.add(((com.oddlabs.tt.model.Unit)hostile.get(j)));
                        }
                    }
                }
                long sx = 0;
                long sy = 0;
                foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(t.units))
                {
                    t.e += com.oddlabs.tt.player.ultra.CombatModel.weight(u_2);
                    sx += u_2.getGridX();
                    sy += u_2.getGridY();
                }
                t.cx = unchecked((int)((sx / t.units.size())));
                t.cy = unchecked((int)((sy / t.units.size())));
                threats.add(t);
            }
            foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(ctx.model.enemy.buildings))
            {
                if (b.isDead() || !nearOurAssets(b.getGridX(), b.getGridY(), mine))
                {
                    continue;
                }
                com.oddlabs.tt.player.ultra.MilitaryManager.Threat nearest = null;
                foreach (com.oddlabs.tt.player.ultra.MilitaryManager.Threat t_2 in java.lang.JIter.of(threats))
                {
                    if (com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(t_2.cx, t_2.cy, b.getGridX(), b.getGridY()) <= 12)
                    {
                        nearest = t_2;
                    }
                }
                if (nearest == null)
                {
                    nearest = new com.oddlabs.tt.player.ultra.MilitaryManager.Threat();
                    nearest.cx = b.getGridX();
                    nearest.cy = b.getGridY();
                    threats.add(nearest);
                }
                nearest.sites.add(b);
                nearest.e += (b.isComplete() ? java.lang.Math.max(0.5f, (float)(com.oddlabs.tt.player.ultra.CombatModel.towerValue(b) * 0.6f)) : 0.5f);
            }
            foreach (com.oddlabs.tt.player.ultra.MilitaryManager.Threat t_3 in java.lang.JIter.of(threats))
            {
                intruders.addAll(t_3.units);
                intruders.addAll(t_3.sites);
                intruder_e += t_3.e;
            }
            if (!threats.isEmpty())
            {
                com.oddlabs.tt.player.ultra.MilitaryManager.Threat worst = ((com.oddlabs.tt.player.ultra.MilitaryManager.Threat)threats.get(0));
                foreach (com.oddlabs.tt.player.ultra.MilitaryManager.Threat t_4 in java.lang.JIter.of(threats))
                {
                    if (t_4.e > worst.e)
                    {
                        worst = t_4;
                    }
                }
                intr_x = worst.cx;
                intr_y = worst.cy;
            }
            assignDefenders();
            peonStrike();
            bool was = defending;
            defending = !threats.isEmpty();
            if (defending != was)
            {
                int count = intruders.size();
                float e_2 = intruder_e;
                int groups = threats.size();
                ctx.log(new java.util.function.Supplier_L(() =>
                {
                    return "defending=" + java.lang.JStr.valueOf(defending) + " intruders=" + (count) + " groups=" + (groups) + " E=" + java.lang.JStr.valueOf(e_2);
                }));
            }
            ctx.economy.emergency = false;
            if (a != null)
            {
                foreach (com.oddlabs.tt.player.ultra.MilitaryManager.Threat t_5 in java.lang.JIter.of(threats))
                {
                    if (t_5.e >= 3.0f && com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(t_5.cx, t_5.cy, a.getGridX(), a.getGridY()) < 40)
                    {
                        ctx.economy.emergency = true;
                    }
                }
            }
            evacuate();
        }
        public java.util.Map strikers;
        // Java: 574
        public void peonStrike()
        {
            strikers.keySet().removeIf(new java.util.function.Predicate_L((object u_l) =>
            {
                com.oddlabs.tt.model.Unit u = (com.oddlabs.tt.model.Unit)u_l;
                com.oddlabs.tt.model.Building t = ((com.oddlabs.tt.model.Building)strikers.get(u));
                bool done = u.isDead() || t == null || t.isDead() || ((object)ctx.roleOf(u) != (object)com.oddlabs.tt.player.ultra.Context.Role.CORPS);
                if (done && !u.isDead() && ((object)ctx.roleOf(u) == (object)com.oddlabs.tt.player.ultra.Context.Role.CORPS))
                {
                    ctx.clearRole(u);
                    com.oddlabs.tt.model.Building a = ctx.economy.armory();
                    if (a != null)
                    {
                        ctx.orders.enter(u, a);
                    }
                }
                return done;
            }));
            if (!peon_strike)
            {
                return;
            }
            java.util.List mine = ctx.model.me.buildings;
            java.util.List towers = new java.util.ArrayList(ctx.model.enemy.towers);
            foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(ctx.model.enemy.sites))
            {
                if (((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID() == com.oddlabs.tt.model.Race.BUILDING_TOWER)
                {
                    towers.add(b);
                }
            }
            foreach (com.oddlabs.tt.model.Building t_2 in java.lang.JIter.of(towers))
            {
                if (t_2.isDead() || !nearOurAssets(t_2.getGridX(), t_2.getGridY(), mine))
                {
                    continue;
                }
                int want = default;
                com.oddlabs.tt.model.Unit g = com.oddlabs.tt.player.ultra.WorldModel.garrisonOf(t_2);
                if (!t_2.isComplete())
                {
                    want = java.lang.Math.max(3, java.lang.Math.min(6, (t_2.getHitPoints() + 5) / 6 + 2));
                }
                else if (g == null || com.oddlabs.tt.player.ultra.WorldModel.isStunned(g) || warriorsNear(t_2, 12) >= 3)
                {
                    want = 8;
                }
                else
                {
                    continue;
                }
                int have = 0;
                foreach (com.oddlabs.tt.model.Building target in java.lang.JIter.of(strikers.values()))
                {
                    if (((object)target == (object)t_2))
                    {
                        have++;
                    }
                }
                for (int k = have; k < want; k++)
                {
                    com.oddlabs.tt.model.Unit best = null;
                    int best_d = 35 * 35;
                    foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(ctx.model.me.peons))
                    {
                        if (u_2.isDead() || (ctx.hasRole(u_2) && ((object)ctx.roleOf(u_2) != (object)com.oddlabs.tt.player.ultra.Context.Role.HUNTER)))
                        {
                            continue;
                        }
                        if (strikers.containsKey(u_2))
                        {
                            continue;
                        }
                        int d = com.oddlabs.tt.player.ultra.MapAnalysis.dist2(u_2.getGridX(), u_2.getGridY(), t_2.getGridX(), t_2.getGridY());
                        if (d < best_d)
                        {
                            best_d = d;
                            best = u_2;
                        }
                    }
                    if (best == null)
                    {
                        break;
                    }
                    ctx.setRole(best, com.oddlabs.tt.player.ultra.Context.Role.CORPS);
                    strikers.put(best, t_2);
                    ctx.orders.attack(best, t_2);
                }
                foreach (java.util.Map.Entry e in java.lang.JIter.of(strikers.entrySet()))
                {
                    com.oddlabs.tt.model.Unit u_3 = ((com.oddlabs.tt.model.Unit)e.getKey());
                    if (((object)((com.oddlabs.tt.model.Building)e.getValue()) == (object)t_2) && !((u_3.getCurrentController() is com.oddlabs.tt.model.behaviour.HuntController)) && !ctx.orders.recently(u_3, 2.0f))
                    {
                        ctx.orders.attack(u_3, t_2);
                    }
                }
            }
        }
        // Java: 640
        public int warriorsNear(com.oddlabs.tt.model.Building t, int r)
        {
            int n = 0;
            foreach (com.oddlabs.tt.model.Unit w in java.lang.JIter.of(ctx.model.me.warriors))
            {
                if (com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(w.getGridX(), w.getGridY(), t.getGridX(), t.getGridY()) <= r)
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 650
        public bool nearOurPeons(com.oddlabs.tt.model.Unit e)
        {
            if (ctx.map.frontness(e.getGridX(), e.getGridY()) > 0.55f)
            {
                return false;
            }
            foreach (com.oddlabs.tt.model.Unit p in java.lang.JIter.of(ctx.model.me.peons))
            {
                if (com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(p.getGridX(), p.getGridY(), e.getGridX(), e.getGridY()) <= 14)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 664
        public void assignDefenders()
        {
            java.util.List pool = new java.util.ArrayList(home);
            if (((object)state_f == (object)com.oddlabs.tt.player.ultra.MilitaryManager.State.GATHER))
            {
                pool.addAll(main);
            }
            pool.addAll(reinf);
            java.util.List order = new java.util.ArrayList(threats);
            order.sort(new java.util.Comparator_L((object x_l, object y_l) =>
            {
                com.oddlabs.tt.player.ultra.MilitaryManager.Threat x = (com.oddlabs.tt.player.ultra.MilitaryManager.Threat)x_l;
                com.oddlabs.tt.player.ultra.MilitaryManager.Threat y = (com.oddlabs.tt.player.ultra.MilitaryManager.Threat)y_l;
                return java.lang.Float.compare(y.e, x.e);
            }));
            foreach (com.oddlabs.tt.player.ultra.MilitaryManager.Threat t in java.lang.JIter.of(order))
            {
                float need = java.lang.Math.max(2.0f, (float)((float)(1.5f * t.e) + 0.5f));
                float got = 0.0f;
                while (got < need && !pool.isEmpty())
                {
                    com.oddlabs.tt.model.Unit best = null;
                    int best_d = java.lang.Integer.MAX_VALUE;
                    foreach (com.oddlabs.tt.model.Unit w in java.lang.JIter.of(pool))
                    {
                        int d = com.oddlabs.tt.player.ultra.MapAnalysis.dist2(w.getGridX(), w.getGridY(), t.cx, t.cy);
                        if (d < best_d)
                        {
                            best_d = d;
                            best = w;
                        }
                    }
                    pool.remove(best);
                    t.defenders.add(best);
                    got = (float)(got + com.oddlabs.tt.player.ultra.CombatModel.weight(best));
                }
            }
        }
        // Java: 695
        public void recallIfCoreThreatened()
        {
            if (!recall || ((object)state_f == (object)com.oddlabs.tt.player.ultra.MilitaryManager.State.GATHER) || ((object)state_f == (object)com.oddlabs.tt.player.ultra.MilitaryManager.State.RETREAT) || main.isEmpty() || toot_return)
            {
                return;
            }
            com.oddlabs.tt.model.Building a = ctx.economy.armory();
            if (a == null)
            {
                return;
            }
            com.oddlabs.tt.model.Unit an = anchor(main);
            if (com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(an.getGridX(), an.getGridY(), a.getGridX(), a.getGridY()) <= 35)
            {
                return;
            }
            float threat = 0.0f;
            foreach (com.oddlabs.tt.player.ultra.MilitaryManager.Threat t in java.lang.JIter.of(threats))
            {
                if (com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(t.cx, t.cy, a.getGridX(), a.getGridY()) <= 30)
                {
                    threat = (float)(threat + t.e);
                }
            }
            if (threat < 4.0f)
            {
                return;
            }
            float home_def = (float)(com.oddlabs.tt.player.ultra.CombatModel.sumWeights(home) + com.oddlabs.tt.player.ultra.CombatModel.sumWeights(reinf));
            float towers = 0.0f;
            foreach (com.oddlabs.tt.model.Building tw in java.lang.JIter.of(ctx.model.me.towers))
            {
                if (!tw.isDead() && com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(tw.getGridX(), tw.getGridY(), a.getGridX(), a.getGridY()) <= 20)
                {
                    towers = (float)(towers + com.oddlabs.tt.player.ultra.CombatModel.towerValue(tw));
                }
            }
            if ((float)(home_def + (float)(0.5f * towers)) >= (float)(1.2f * threat))
            {
                return;
            }
            if (((object)state_f == (object)com.oddlabs.tt.player.ultra.MilitaryManager.State.SIEGE) && objective != null && !objective.isDead() && ((com.oddlabs.tt.model.BuildingTemplate)objective.getTemplate()).getTemplateID() == com.oddlabs.tt.model.Race.BUILDING_ARMORY && objective.getHitPoints() < 80)
            {
                return;
            }
            float t_2 = threat;
            ctx.log(new java.util.function.Supplier_L(() =>
            {
                return java.lang.JString.format("RECALL: core threat %.1f vs home %.1f", t_2, home_def);
            }));
            recall_until = (float)(ctx.now + 30.0f);
            retreat();
        }
        // Java: 731
        public bool defendingUnits()
        {
            foreach (com.oddlabs.tt.player.ultra.MilitaryManager.Threat t in java.lang.JIter.of(threats))
            {
                if (!t.units.isEmpty())
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 740
        public bool majorThreat()
        {
            com.oddlabs.tt.model.Building a = ctx.economy.armory();
            float army = (float)(com.oddlabs.tt.player.ultra.CombatModel.sumWeights(main) + com.oddlabs.tt.player.ultra.CombatModel.sumWeights(home));
            foreach (com.oddlabs.tt.player.ultra.MilitaryManager.Threat t in java.lang.JIter.of(threats))
            {
                if (t.e < java.lang.Math.max(3.0f, (float)(0.3f * army)))
                {
                    continue;
                }
                if (a == null || com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(t.cx, t.cy, a.getGridX(), a.getGridY()) <= 45)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 752
        public bool nearOurAssets(int x, int y, java.util.List mine)
        {
            int r2 = defend_radius * defend_radius;
            foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(mine))
            {
                if (b.isDead())
                {
                    continue;
                }
                int rr = (((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID() == com.oddlabs.tt.model.Race.BUILDING_ARMORY ? r2 : (r2 * 2) / 3);
                if (com.oddlabs.tt.player.ultra.MapAnalysis.dist2(b.getGridX(), b.getGridY(), x, y) <= rr)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 764
        public void evacuate()
        {
            com.oddlabs.tt.model.Building a = ctx.economy.armory();
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(ctx.model.me.peons))
            {
                if (!ctx.resources.threatened(u.getGridX(), u.getGridY(), 9))
                {
                    continue;
                }
                if (ctx.orders.recently(u, 2.5f) && ((object)ctx.orders.lastOrder(u).kind() == (object)com.oddlabs.tt.player.ultra.Orders.Kind.ENTER))
                {
                    continue;
                }
                if (((object)ctx.roleOf(u) == (object)com.oddlabs.tt.player.ultra.Context.Role.CORPS))
                {
                    continue;
                }
                ctx.build.release(u);
                if (((object)ctx.roleOf(u) == (object)com.oddlabs.tt.player.ultra.Context.Role.HUNTER))
                {
                    ctx.clearRole(u);
                }
                if (a != null && com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(u.getGridX(), u.getGridY(), a.getGridX(), a.getGridY()) < 45)
                {
                    ctx.orders.enter(u, a);
                    continue;
                }
                com.oddlabs.tt.model.Building q = nearestSafeQuarters(u.getGridX(), u.getGridY());
                if (q != null && !com.oddlabs.tt.player.ultra.BuildManager.carriesAnything(u) && com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(u.getGridX(), u.getGridY(), q.getGridX(), q.getGridY()) < 30)
                {
                    ctx.orders.enter(u, q);
                    continue;
                }
                int[] away = awayFromEnemy(u.getGridX(), u.getGridY(), 14);
                ctx.orders.move(u, away[0], away[1]);
            }
        }
        // Java: 792
        public com.oddlabs.tt.model.Building nearestSafeQuarters(int x, int y)
        {
            com.oddlabs.tt.model.Building best = null;
            int best_d = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Building q in java.lang.JIter.of(ctx.model.me.quarters))
            {
                if (q.isDead() || quartersThreatened(q) || ctx.resources.threatened(q.getGridX(), q.getGridY(), 14))
                {
                    continue;
                }
                int d = com.oddlabs.tt.player.ultra.MapAnalysis.dist2(q.getGridX(), q.getGridY(), x, y);
                if (d < best_d)
                {
                    best_d = d;
                    best = q;
                }
            }
            return best;
        }
        // Java: 807
        public int[] awayFromEnemy(int x, int y, int dist)
        {
            float dx = 0.0f;
            float dy = 0.0f;
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(ctx.model.enemy.warriors))
            {
                int d2 = com.oddlabs.tt.player.ultra.MapAnalysis.dist2(e.getGridX(), e.getGridY(), x, y);
                if (d2 > 400 || d2 == 0)
                {
                    continue;
                }
                float inv = (float)(1.0f / d2);
                dx = (float)(dx + (float)((x - e.getGridX()) * inv));
                dy = (float)(dy + (float)((y - e.getGridY()) * inv));
            }
            com.oddlabs.tt.model.Unit chief = ctx.model.enemy.chieftain;
            if (chief != null && !chief.isDead())
            {
                int d2_2 = com.oddlabs.tt.player.ultra.MapAnalysis.dist2(chief.getGridX(), chief.getGridY(), x, y);
                if (d2_2 > 0 && d2_2 <= 400)
                {
                    float inv_2 = (float)(1.0f / d2_2);
                    dx = (float)(dx + (float)((x - chief.getGridX()) * inv_2));
                    dy = (float)(dy + (float)((y - chief.getGridY()) * inv_2));
                }
            }
            float len = ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy)))));
            if (len < 1.0E-6f)
            {
                dx = ctx.map.home_x - x;
                dy = ctx.map.home_y - y;
                len = ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy)))));
                if (len < 1.0E-6f)
                {
                    return new int[] { x, y };
                }
            }
            int tx = ctx.map.clamp(java.lang.Math.round((float)(x + (float)((float)(dx / len) * dist))));
            int ty = ctx.map.clamp(java.lang.Math.round((float)(y + (float)((float)(dy / len) * dist))));
            int[] acc = ctx.map.nearestAccessible(tx, ty);
            return (acc != null ? acc : new int[] { x, y });
        }
        // Java: 841
        public void updateGather()
        {
            if ((float)(ctx.now - gather_time) < 20.0f)
            {
                return;
            }
            gather_time = ctx.now;
            com.oddlabs.tt.model.Building a = ctx.economy.armory();
            int bx = (a != null ? a.getGridX() : ctx.map.home_x);
            int by = (a != null ? a.getGridY() : ctx.map.home_y);
            int[] p = com.oddlabs.tt.player.ultra.BuildManager.towards(bx, by, ctx.map.enemy_x, ctx.map.enemy_y, 0);
            int dx = p[0] - bx;
            int dy = p[1] - by;
            float len = ((float)(java.lang.Math.sqrt(dx * dx + dy * dy)));
            int gx = bx;
            int gy = by;
            if (len > 1.0f)
            {
                gx = java.lang.Math.round((float)(bx + (float)((float)(dx / len) * 12.0f)));
                gy = java.lang.Math.round((float)(by + (float)((float)(dy / len) * 12.0f)));
            }
            int[] acc = ctx.map.nearestAccessible(ctx.map.clamp(gx), ctx.map.clamp(gy));
            if (acc != null)
            {
                gather = acc;
            }
        }
        // Java: 866
        public void postureTick()
        {
            rebuildSquads();
            switch ((state_f).ordinal())
            {
                case 0 /*GATHER*/:
                {
                    gatherState();
                    break;
                }
                case 1 /*MARCH*/:
                {
                    marchState();
                    break;
                }
                case 2 /*ENGAGE*/:
                {
                    engageState();
                    break;
                }
                case 3 /*SIEGE*/:
                {
                    siegeState();
                    break;
                }
                case 4 /*RETREAT*/:
                {
                    retreatState();
                    break;
                }
            }
            recallIfCoreThreatened();
            reinforce();
            raidTick();
            if (defending)
            {
                defend();
            }
            else
            {
                idleHome();
            }
        }
        // Java: 884
        public void setState(com.oddlabs.tt.player.ultra.MilitaryManager.State s)
        {
            if (((object)s == (object)state_f))
            {
                return;
            }
            if (((object)s != (object)com.oddlabs.tt.player.ultra.MilitaryManager.State.ENGAGE))
            {
                toot_return = false;
            }
            if (((object)s == (object)com.oddlabs.tt.player.ultra.MilitaryManager.State.GATHER))
            {
                endAttack();
            }
            if (((object)s == (object)com.oddlabs.tt.player.ultra.MilitaryManager.State.GATHER) || ((object)s == (object)com.oddlabs.tt.player.ultra.MilitaryManager.State.RETREAT))
            {
                resetSiege();
            }
            if (((object)s == (object)com.oddlabs.tt.player.ultra.MilitaryManager.State.MARCH))
            {
                best_prog = java.lang.Integer.MAX_VALUE;
                best_prog_t = ctx.now;
            }
            com.oddlabs.tt.player.ultra.MilitaryManager.State old = state_f;
            state_f = s;
            state_since = ctx.now;
            int size = main.size();
            ctx.log(new java.util.function.Supplier_L(() =>
            {
                return "army " + java.lang.JStr.valueOf(old) + " -> " + java.lang.JStr.valueOf(s) + " size=" + (size) + java.lang.JStr.valueOf(((objective != null ? " obj=" + (((com.oddlabs.tt.model.BuildingTemplate)objective.getTemplate()).getTemplateID()) + "@" + (obj_x) + "," + (obj_y) : "")));
            }));
        }
        // Java: 904
        public float enemyFieldStrength()
        {
            com.oddlabs.tt.player.ultra.WorldModel.Side e = ctx.model.enemy;
            float units = com.oddlabs.tt.player.ultra.CombatModel.sumWeights(e.warriors);
            if (e.chieftain != null)
            {
                units = (float)(units + 1.5f);
            }
            units = (float)(units + (float)(enemy_latent_guess * e.armories.size()));
            return units;
        }
        // Java: 919
        public float withTowers(float units, int x, int y, int r)
        {
            if (tower_linear <= 0.0f)
            {
                return com.oddlabs.tt.player.ultra.CombatModel.strength(units, towerStrengthNear(x, y, r));
            }
            float sum = 0.0f;
            int r2 = r * r;
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(ctx.model.enemy.towers))
            {
                if (com.oddlabs.tt.player.ultra.MapAnalysis.dist2(t.getGridX(), t.getGridY(), x, y) <= r2)
                {
                    sum = (float)(sum + (float)(com.oddlabs.tt.player.ultra.CombatModel.towerValue(t) * tower_linear));
                }
            }
            return (float)(units + sum);
        }
        // Java: 931
        public float towerStrengthNear(int x, int y, int r)
        {
            float sq = 0.0f;
            int r2 = r * r;
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(ctx.model.enemy.towers))
            {
                if (com.oddlabs.tt.player.ultra.MapAnalysis.dist2(t.getGridX(), t.getGridY(), x, y) <= r2)
                {
                    float v = com.oddlabs.tt.player.ultra.CombatModel.towerValue(t);
                    sq = (float)(sq + (float)(v * v));
                }
            }
            return sq;
        }
        // Java: 943
        public void gatherState()
        {
            main.addAll(home);
            foreach (com.oddlabs.tt.model.Unit w in java.lang.JIter.of(home))
            {
                squad_of.put(w, com.oddlabs.tt.player.ultra.MilitaryManager.MAIN);
            }
            home.clear();
            bool trace = (float)(ctx.now - last_gather_trace) >= 30.0f;
            if (trace)
            {
                last_gather_trace = ctx.now;
            }
            if (majorThreat())
            {
                if (trace)
                {
                    ctx.log(new java.util.function.Supplier_L(() =>
                    {
                        return "gather: held by major threat, main=" + (main.size());
                    }));
                }
                return;
            }
            int stocked = stockedIron();
            float ours = (float)(com.oddlabs.tt.player.ultra.CombatModel.sumWeights(main) + stocked);
            com.oddlabs.tt.model.Building first = chooseTarget();
            if (first == null)
            {
                if (!enemyBuildingsLeft() && !ctx.model.enemy.units.isEmpty() && !main.isEmpty())
                {
                    closeout = true;
                    closeoutTick();
                }
                return;
            }
            com.oddlabs.tt.model.Building target = (assault_gate ? chooseTarget(ours) : first);
            if (target == null)
            {
                if (trace)
                {
                    ctx.log(new java.util.function.Supplier_L(() =>
                    {
                        return "gather: tower line at " + (gate_x) + "," + (gate_y) + " still too strong";
                    }));
                }
                return;
            }
            float theirs = withTowers(enemyFieldStrength(), target.getGridX(), target.getGridY(), 26);
            if (assault_gate && approach_towers)
            {
                theirs = (float)(enemyFieldStrength() + approachLine(gather[0], gather[1], target.getGridX(), target.getGridY()));
            }
            com.oddlabs.tt.model.Unit their_chief = ctx.model.enemy.chieftain;
            com.oddlabs.tt.model.Unit our_chief = ctx.owner.getChieftain();
            bool answer = our_chief != null && !our_chief.isDead() && our_chief.getMagicProgress(com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_STUN) >= 0.99f;
            if (their_chief != null && !their_chief.isDead() && !answer)
            {
                theirs = (float)(theirs * (float)(1.0f + enemy_chief_factor));
            }
            int pop = ctx.owner.getUnitCountContainer().getNumSupplies();
            if (learn_decay > 0.0f && pop >= ctx.world.getMaxUnitCount() - 10 && defense_mult > 1.0f)
            {
                float dt = java.lang.Math.max(0.0f, (float)(ctx.now - last_decay));
                float old = defense_mult;
                defense_mult = (float)(1.0f + (float)(((float)(defense_mult - 1.0f)) * ((float)(java.lang.StrictMath.pow(0.5d, (float)(dt / learn_decay))))));
                if (java.lang.JC.f2i(((float)(old * 10.0f))) != java.lang.JC.f2i(((float)(defense_mult * 10.0f))))
                {
                    float now = defense_mult;
                    ctx.log(new java.util.function.Supplier_L(() =>
                    {
                        return java.lang.JString.format("enemy defense decays x%.2f -> x%.2f", old, now);
                    }));
                }
            }
            last_decay = ctx.now;
            theirs = (float)(theirs * defense_mult);
            bool at_cap = pop >= (float)(cap_attack_frac * ctx.world.getMaxUnitCount());
            if (trace)
            {
                float o = ours;
                float t = theirs;
                ctx.log(new java.util.function.Supplier_L(() =>
                {
                    return java.lang.JString.format("gather: ours=%.1f theirs=%.1f (x%.2f) main=%d target=%s", o, t, defense_mult, main.size(), (((com.oddlabs.tt.model.BuildingTemplate)target.getTemplate()).getTemplateID()) + "@" + (target.getGridX()) + "," + (target.getGridY()));
                }));
            }
            bool commit = main.size() + stocked >= attack_min && ours >= (float)(commit_ratio * theirs);
            bool grind = at_cap && main.size() + stocked >= attack_min / 2 && ours >= (float)(grind_ratio * theirs);
            if ((commit || grind) && stocked >= 3 && ctx.now >= (float)(muster_until + 20.0f))
            {
                muster_until = (float)((float)(ctx.now + (float)(1.5f * stocked)) + 4.0f);
                int n = stocked;
                ctx.log(new java.util.function.Supplier_L(() =>
                {
                    return "muster " + (n) + " stocked weapons";
                }));
                return;
            }
            if (ctx.now < muster_until)
            {
                return;
            }
            if (commit || grind)
            {
                setObjective(target);
                float o_2 = ours;
                float t_2 = theirs;
                ctx.log(new java.util.function.Supplier_L(() =>
                {
                    return java.lang.JString.format("attack: ours=%.1f theirs=%.1f", o_2, t_2);
                }));
                startAttack();
                setState(com.oddlabs.tt.player.ultra.MilitaryManager.State.MARCH);
                newLeg();
            }
        }
        // Java: 1027
        public void setObjective(com.oddlabs.tt.model.Building target)
        {
            closeout = false;
            toot_return = false;
            objective = target;
            obj_x = target.getGridX();
            obj_y = target.getGridY();
            int from_x = (main.isEmpty() ? gather[0] : anchor(main).getGridX());
            int from_y = (main.isEmpty() ? gather[1] : anchor(main).getGridY());
            int limit = 3 * com.oddlabs.tt.player.ultra.MapAnalysis.octile(obj_x - from_x, obj_y - from_y) / 2 + 200;
            route = ctx.map.distanceField(obj_x, obj_y, limit, route);
            best_prog = java.lang.Integer.MAX_VALUE;
            best_prog_t = ctx.now;
        }
        // Java: 1043
        public com.oddlabs.tt.model.Building chooseTarget()
        {
            return chooseTarget((float)(-1.0f));
        }
        // Java: 1048
        public com.oddlabs.tt.model.Building chooseTarget(float ours)
        {
            com.oddlabs.tt.model.Building best = null;
            float best_score = java.lang.Float.NEGATIVE_INFINITY;
            int fx = (main.isEmpty() ? gather[0] : anchor(main).getGridX());
            int fy = (main.isEmpty() ? gather[1] : anchor(main).getGridY());
            bool has_armory = !ctx.model.enemy.armories.isEmpty();
            foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(ctx.model.enemy.buildings))
            {
                if (b.isDead())
                {
                    continue;
                }
                if (ours >= 0.0f && gateMemoryBlocks(b, ours, fx, fy))
                {
                    continue;
                }
                float? su = ((float?)stalled.get(b));
                if (su != null && ctx.now < ((float)su))
                {
                    continue;
                }
                float value_v = default;
                int id = ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID();
                if (!b.isComplete())
                {
                    value_v = 6.0f;
                }
                else if (id == com.oddlabs.tt.model.Race.BUILDING_ARMORY)
                {
                    value_v = 80.0f;
                }
                else if (id == com.oddlabs.tt.model.Race.BUILDING_QUARTERS)
                {
                    value_v = (has_armory ? 35.0f : 70.0f);
                }
                else
                {
                    value_v = 14.0f;
                }
                float d = ((float)(java.lang.Math.sqrt(com.oddlabs.tt.player.ultra.MapAnalysis.dist2(b.getGridX(), b.getGridY(), fx, fy))));
                float cover = 0.0f;
                foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(ctx.model.enemy.towers))
                {
                    if (((object)t != (object)b) && com.oddlabs.tt.player.ultra.MapAnalysis.dist2(t.getGridX(), t.getGridY(), b.getGridX(), b.getGridY()) <= 324)
                    {
                        cover = (float)(cover + (float)((float)(com.oddlabs.tt.player.ultra.CombatModel.towerValue(t) * 0.6f) * cover_weight));
                    }
                }
                float score = (float)((float)(value_v / ((float)(1.0f + (float)(d / 70.0f)))) - cover);
                if (score > best_score)
                {
                    best_score = score;
                    best = b;
                }
            }
            return best;
        }
        // Java: 1087
        public com.oddlabs.tt.model.Unit anchor(java.util.List squad)
        {
            long sx = 0;
            long sy = 0;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(squad))
            {
                sx += u.getGridX();
                sy += u.getGridY();
            }
            int cx = unchecked((int)((sx / squad.size())));
            int cy = unchecked((int)((sy / squad.size())));
            com.oddlabs.tt.model.Unit best = ((com.oddlabs.tt.model.Unit)squad.get(0));
            int best_d = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(squad))
            {
                int d = com.oddlabs.tt.player.ultra.MapAnalysis.dist2(u_2.getGridX(), u_2.getGridY(), cx, cy);
                if (d < best_d)
                {
                    best_d = d;
                    best = u_2;
                }
            }
            return best;
        }
        // Java: 1109
        public int routeAt(int x, int y)
        {
            int[] f = route;
            return (f == null || !ctx.map.inside(x, y) ? com.oddlabs.tt.player.ultra.MapAnalysis.UNREACHABLE : f[y * ctx.map.n + x]);
        }
        // Java: 1114
        public void newLeg()
        {
            newLeg(false);
        }
        // Java: 1119
        public void newLeg(bool from_wp)
        {
            if (main.isEmpty())
            {
                return;
            }
            com.oddlabs.tt.model.Unit a = anchor(main);
            int x = a.getGridX();
            int y = a.getGridY();
            int av = routeAt(x, y);
            leg_anchor_v = av;
            if (from_wp && waypoint != null)
            {
                int wv = routeAt(waypoint[0], waypoint[1]);
                if (wv != com.oddlabs.tt.player.ultra.MapAnalysis.UNREACHABLE && (av == com.oddlabs.tt.player.ultra.MapAnalysis.UNREACHABLE || wv < av))
                {
                    x = waypoint[0];
                    y = waypoint[1];
                }
            }
            int[] field = route;
            if (field != null && field[y * ctx.map.n + x] == com.oddlabs.tt.player.ultra.MapAnalysis.UNREACHABLE)
            {
                field = null;
            }
            if (field != null)
            {
                int n = ctx.map.n;
                for (int step = 0; step < leg_length; step++)
                {
                    int best = field[y * n + x];
                    int bx = x;
                    int by = y;
                    for (int dy = (-1); dy <= 1; dy++)
                    {
                        for (int dx = (-1); dx <= 1; dx++)
                        {
                            int nx = x + dx;
                            int ny = y + dy;
                            if (!ctx.map.inside(nx, ny))
                            {
                                continue;
                            }
                            int v = field[ny * n + nx];
                            if (v < best)
                            {
                                best = v;
                                bx = nx;
                                by = ny;
                            }
                        }
                    }
                    if (bx == x && by == y)
                    {
                        break;
                    }
                    x = bx;
                    y = by;
                }
            }
            else
            {
                int[] t = com.oddlabs.tt.player.ultra.BuildManager.towards(x, y, obj_x, obj_y, 0);
                x = t[0];
                y = t[1];
            }
            waypoint = new int[] { x, y };
            leg_start = ctx.now;
            ctx.orders.group(main, x, y, false);
        }
        // Java: 1172
        public bool enemiesNear(java.util.List squad, int r)
        {
            if (squad.isEmpty())
            {
                return false;
            }
            com.oddlabs.tt.model.Unit a = anchor(squad);
            int r2 = r * r;
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(ctx.model.enemy.warriors))
            {
                if (com.oddlabs.tt.player.ultra.MapAnalysis.dist2(e.getGridX(), e.getGridY(), a.getGridX(), a.getGridY()) <= r2)
                {
                    return true;
                }
            }
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(ctx.model.enemy.towers))
            {
                if (com.oddlabs.tt.player.ultra.WorldModel.garrisonOf(t) != null && com.oddlabs.tt.player.ultra.MapAnalysis.dist2(t.getGridX(), t.getGridY(), a.getGridX(), a.getGridY()) <= r2)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 1189
        public void marchState()
        {
            if (main.isEmpty())
            {
                setState(com.oddlabs.tt.player.ultra.MilitaryManager.State.GATHER);
                return;
            }
            if (objective == null || objective.isDead())
            {
                nextObjectiveOrHome();
                return;
            }
            if (enemiesNear(main, 24))
            {
                last_enemy_near = ctx.now;
                setState(com.oddlabs.tt.player.ultra.MilitaryManager.State.ENGAGE);
                return;
            }
            com.oddlabs.tt.model.Unit a = anchor(main);
            if (com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(a.getGridX(), a.getGridY(), obj_x, obj_y) <= 14)
            {
                if (assaultBlocked(obj_x, obj_y))
                {
                    stopPush(obj_x, obj_y);
                    return;
                }
                setState(com.oddlabs.tt.player.ultra.MilitaryManager.State.SIEGE);
                return;
            }
            int av = routeAt(a.getGridX(), a.getGridY());
            if (march_watch)
            {
                if (av == com.oddlabs.tt.player.ultra.MapAnalysis.UNREACHABLE)
                {
                    best_prog = java.lang.Integer.MAX_VALUE;
                    best_prog_t = ctx.now;
                }
                else if (av < best_prog - 10)
                {
                    best_prog = av;
                    best_prog_t = ctx.now;
                }
                else if ((float)(ctx.now - best_prog_t) > 60.0f)
                {
                    com.oddlabs.tt.model.Building o = objective;
                    int v = av;
                    ctx.log(new java.util.function.Supplier_L(() =>
                    {
                        return "march stalled at route " + (v) + ": objective dropped for 300 s";
                    }));
                    if (o != null)
                    {
                        stalled.put(o, (float)(ctx.now + 300.0f));
                    }
                    if (ctx.owner.getUnitsLost() == attack_lost0 && ctx.owner.getUnitsKilled() == attack_killed0)
                    {
                        attack_active = false;
                    }
                    retreat();
                    return;
                }
            }
            int rc = 3 + java.lang.Math.round((float)(0.8f * ((float)(java.lang.Math.sqrt(main.size())))));
            int wv = (march_watch ? routeAt(waypoint[0], waypoint[1]) : com.oddlabs.tt.player.ultra.MapAnalysis.UNREACHABLE);
            int close = 0;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(main))
            {
                if (wv != com.oddlabs.tt.player.ultra.MapAnalysis.UNREACHABLE)
                {
                    int uv = routeAt(u.getGridX(), u.getGridY());
                    if (uv != com.oddlabs.tt.player.ultra.MapAnalysis.UNREACHABLE && uv <= wv + 2 * (rc + 2) && com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(u.getGridX(), u.getGridY(), waypoint[0], waypoint[1]) <= rc + 2)
                    {
                        close++;
                    }
                }
                else if (com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(u.getGridX(), u.getGridY(), waypoint[0], waypoint[1]) <= rc + 2)
                {
                    close++;
                }
            }
            float leg_timeout = (float)((float)((float)(leg_length * 1.4f) / 2.0f) + 6.0f);
            if (close >= (float)(cohesion_frac * main.size()))
            {
                newLeg(march_watch);
            }
            else if ((float)(ctx.now - leg_start) > leg_timeout)
            {
                bool stuck = march_watch && av != com.oddlabs.tt.player.ultra.MapAnalysis.UNREACHABLE && leg_anchor_v != com.oddlabs.tt.player.ultra.MapAnalysis.UNREACHABLE && av > leg_anchor_v - 6;
                newLeg(stuck);
            }
        }
        // Java: 1259
        public void engageState()
        {
            if (main.isEmpty())
            {
                setState(com.oddlabs.tt.player.ultra.MilitaryManager.State.GATHER);
                return;
            }
            bool warriors_near = enemyWarriorsNear(main, 28);
            if (enemiesNear(main, 28))
            {
                last_enemy_near = ctx.now;
            }
            if (!tootActive() && localRatio(main) < retreat_ratio)
            {
                retreat();
                return;
            }
            com.oddlabs.tt.model.Unit gate_anchor = anchor(main);
            if (assaultBlocked(gate_anchor.getGridX(), gate_anchor.getGridY()))
            {
                stopPush(gate_anchor.getGridX(), gate_anchor.getGridY());
                return;
            }
            if (!warriors_near && !closeout)
            {
                com.oddlabs.tt.model.Building tower = nearestMannedTower(main, 24);
                if (tower != null)
                {
                    if (assaultBlocked(tower.getGridX(), tower.getGridY()))
                    {
                        stopPush(tower.getGridX(), tower.getGridY());
                        return;
                    }
                    if (objective == null || objective.isDead() || com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(obj_x, obj_y, tower.getGridX(), tower.getGridY()) > 24)
                    {
                        setObjective(tower);
                    }
                    setState(com.oddlabs.tt.player.ultra.MilitaryManager.State.SIEGE);
                    return;
                }
            }
            if ((float)(ctx.now - last_enemy_near) > 5.0f)
            {
                if (toot_return)
                {
                    toot_return = false;
                    objective = null;
                    setState(com.oddlabs.tt.player.ultra.MilitaryManager.State.GATHER);
                    ctx.orders.group(main, gather[0], gather[1], false);
                }
                else if (closeout)
                {
                    closeoutTick();
                }
                else if (objective == null || objective.isDead())
                {
                    nextObjectiveOrHome();
                }
                else if (!pressOnOk())
                {
                    ctx.log(new java.util.function.Supplier_L(() =>
                    {
                        return "attack called off before the tower line";
                    }));
                    com.oddlabs.tt.model.Unit ga = anchor(main);
                    gate_need = (float)((float)(grind_ratio * defense_mult) * ((float)(enemyFieldStrength() + approachLine(ga.getGridX(), ga.getGridY(), obj_x, obj_y))));
                    gate_x = obj_x;
                    gate_y = obj_y;
                    gate_t = ctx.now;
                    retreat();
                }
                else
                {
                    setState(com.oddlabs.tt.player.ultra.MilitaryManager.State.MARCH);
                    newLeg();
                }
            }
        }
        // Java: 1317
        public void retreat()
        {
            resetSiege();
            endAttack();
            objective = null;
            closeout = false;
            toot_return = false;
            setState(com.oddlabs.tt.player.ultra.MilitaryManager.State.RETREAT);
            ctx.orders.group(main, gather[0], gather[1], false);
        }
        // Java: 1327
        public bool enemyWarriorsNear(java.util.List squad, int r)
        {
            if (squad.isEmpty())
            {
                return false;
            }
            com.oddlabs.tt.model.Unit a = anchor(squad);
            int r2 = r * r;
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(ctx.model.enemy.warriors))
            {
                if (com.oddlabs.tt.player.ultra.MapAnalysis.dist2(e.getGridX(), e.getGridY(), a.getGridX(), a.getGridY()) <= r2)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 1339
        public com.oddlabs.tt.model.Building nearestMannedTower(java.util.List squad, int r)
        {
            com.oddlabs.tt.model.Unit a = anchor(squad);
            com.oddlabs.tt.model.Building best = null;
            int best_d = r * r + 1;
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(ctx.model.enemy.towers))
            {
                if (t.isDead() || com.oddlabs.tt.player.ultra.WorldModel.garrisonOf(t) == null)
                {
                    continue;
                }
                int d = com.oddlabs.tt.player.ultra.MapAnalysis.dist2(t.getGridX(), t.getGridY(), a.getGridX(), a.getGridY());
                if (d < best_d)
                {
                    best_d = d;
                    best = t;
                }
            }
            return best;
        }
        // Java: 1356
        public void closeoutTick()
        {
            if (main.isEmpty() || ctx.model.enemy.units.isEmpty())
            {
                closeout = false;
                return;
            }
            com.oddlabs.tt.model.Unit a = anchor(main);
            com.oddlabs.tt.model.Unit best = null;
            int best_d = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(ctx.model.enemy.units))
            {
                int d = com.oddlabs.tt.player.ultra.MapAnalysis.dist2(e.getGridX(), e.getGridY(), a.getGridX(), a.getGridY());
                if (d < best_d)
                {
                    best_d = d;
                    best = e;
                }
            }
            if (best == null)
            {
                return;
            }
            bool moved = com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(best.getGridX(), best.getGridY(), obj_x, obj_y) > 6;
            obj_x = best.getGridX();
            obj_y = best.getGridY();
            java.util.List go = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit w in java.lang.JIter.of(main))
            {
                if (!((w.getCurrentController() is com.oddlabs.tt.model.behaviour.HuntController)) && (moved || !ctx.orders.recently(w, 4.0f)))
                {
                    go.add(w);
                }
            }
            if (!go.isEmpty())
            {
                ctx.orders.group(go, obj_x, obj_y, true);
            }
            last_enemy_near = ctx.now;
            setState(com.oddlabs.tt.player.ultra.MilitaryManager.State.ENGAGE);
        }
        // Java: 1387
        public void siegeState()
        {
            if (main.isEmpty())
            {
                setState(com.oddlabs.tt.player.ultra.MilitaryManager.State.GATHER);
                return;
            }
            if (!tootActive() && localRatio(main) < retreat_ratio)
            {
                retreat();
                return;
            }
            com.oddlabs.tt.model.Unit gate_anchor = anchor(main);
            if (assaultBlocked(gate_anchor.getGridX(), gate_anchor.getGridY()))
            {
                stopPush(gate_anchor.getGridX(), gate_anchor.getGridY());
                return;
            }
            if (objective == null || objective.isDead())
            {
                nextObjectiveOrHome();
            }
        }
        // Java: 1405
        public void retreatState()
        {
            if (main.isEmpty() || (float)(ctx.now - state_since) > 25.0f)
            {
                setState(com.oddlabs.tt.player.ultra.MilitaryManager.State.GATHER);
                return;
            }
            resendStragglers(main, state_since);
            com.oddlabs.tt.model.Unit a = anchor(main);
            if (com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(a.getGridX(), a.getGridY(), gather[0], gather[1]) < 10)
            {
                setState(com.oddlabs.tt.player.ultra.MilitaryManager.State.GATHER);
            }
        }
        // Java: 1416
        public void nextObjectiveOrHome()
        {
            com.oddlabs.tt.model.Building next = chooseTarget();
            float ours = com.oddlabs.tt.player.ultra.CombatModel.sumWeights(main);
            if (next != null && assault_gate)
            {
                next = chooseTarget(ours);
                if (next == null)
                {
                    retreat();
                    return;
                }
            }
            if (next != null)
            {
                float theirs = (float)(defense_mult * withTowers(enemyFieldStrength(), next.getGridX(), next.getGridY(), 26));
                if (assault_gate)
                {
                    com.oddlabs.tt.model.Unit an = anchor(main);
                    theirs = (float)(defense_mult * ((float)(enemyFieldStrength() + approachLine(an.getGridX(), an.getGridY(), next.getGridX(), next.getGridY()))));
                }
                if (attack_active && ours >= (float)(grind_ratio * theirs) && main.size() >= 4 && (!assault_gate || main.size() >= attack_min / 2))
                {
                    setObjective(next);
                    setState(com.oddlabs.tt.player.ultra.MilitaryManager.State.MARCH);
                    newLeg();
                    return;
                }
            }
            else if (!ctx.model.enemy.units.isEmpty() && !enemyBuildingsLeft())
            {
                objective = null;
                route = null;
                closeout = true;
                closeoutTick();
                return;
            }
            retreat();
        }
        // Java: 1452
        public float localRatio(java.util.List squad)
        {
            com.oddlabs.tt.model.Unit a = anchor(squad);
            int ax = a.getGridX();
            int ay = a.getGridY();
            float ours = 0.0f;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(squad))
            {
                if (com.oddlabs.tt.player.ultra.MapAnalysis.dist2(u.getGridX(), u.getGridY(), ax, ay) <= 625)
                {
                    ours = (float)(ours + com.oddlabs.tt.player.ultra.CombatModel.weight(u));
                }
            }
            float theirs = 0.0f;
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(ctx.model.enemy.warriors))
            {
                if (com.oddlabs.tt.player.ultra.MapAnalysis.dist2(e.getGridX(), e.getGridY(), ax, ay) <= 625 && !com.oddlabs.tt.player.ultra.WorldModel.isStunned(e))
                {
                    theirs = (float)(theirs + com.oddlabs.tt.player.ultra.CombatModel.weight(e));
                }
            }
            float t = withTowers(theirs, ax, ay, 20);
            return (t < 0.5f ? 99.0f : (float)(ours / t));
        }
        // Java: 1470
        public void reinforce()
        {
            bool away = ((object)state_f != (object)com.oddlabs.tt.player.ultra.MilitaryManager.State.GATHER) && ((object)state_f != (object)com.oddlabs.tt.player.ultra.MilitaryManager.State.RETREAT);
            if (!away)
            {
                if (!reinf.isEmpty())
                {
                    ctx.orders.group(reinf, gather[0], gather[1], false);
                }
                foreach (com.oddlabs.tt.model.Unit w in java.lang.JIter.of(reinf))
                {
                    squad_of.put(w, com.oddlabs.tt.player.ultra.MilitaryManager.MAIN);
                }
                reinf.clear();
                return;
            }
            if (!reinf.isEmpty() && !main.isEmpty())
            {
                com.oddlabs.tt.model.Unit ma = anchor(main);
                java.util.List joined = new java.util.ArrayList();
                foreach (com.oddlabs.tt.model.Unit w_2 in java.lang.JIter.of(reinf))
                {
                    if (com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(w_2.getGridX(), w_2.getGridY(), ma.getGridX(), ma.getGridY()) <= 12)
                    {
                        joined.add(w_2);
                    }
                }
                foreach (com.oddlabs.tt.model.Unit w_3 in java.lang.JIter.of(joined))
                {
                    squad_of.put(w_3, com.oddlabs.tt.player.ultra.MilitaryManager.MAIN);
                    main.add(w_3);
                    reinf.remove(w_3);
                }
            }
            if (defendingUnits())
            {
                return;
            }
            if (home.size() >= reinforce_batch)
            {
                foreach (com.oddlabs.tt.model.Unit w_4 in java.lang.JIter.of(home))
                {
                    squad_of.put(w_4, com.oddlabs.tt.player.ultra.MilitaryManager.REINF);
                    reinf.add(w_4);
                }
                home.clear();
            }
            if (reinf.isEmpty() || main.isEmpty())
            {
                return;
            }
            com.oddlabs.tt.model.Unit a = anchor(main);
            com.oddlabs.tt.model.Unit r = anchor(reinf);
            if (com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(a.getGridX(), a.getGridY(), r.getGridX(), r.getGridY()) <= 12)
            {
                foreach (com.oddlabs.tt.model.Unit w_5 in java.lang.JIter.of(reinf))
                {
                    squad_of.put(w_5, com.oddlabs.tt.player.ultra.MilitaryManager.MAIN);
                    main.add(w_5);
                }
                reinf.clear();
                return;
            }
            java.util.List to_order = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit w_6 in java.lang.JIter.of(reinf))
            {
                if (!ctx.orders.recently(w_6, com.oddlabs.tt.player.ultra.Orders.Kind.ATTACK_MOVE, null, a.getGridX(), a.getGridY(), 6.0f) && !((w_6.getCurrentController() is com.oddlabs.tt.model.behaviour.HuntController)))
                {
                    to_order.add(w_6);
                }
            }
            if (!to_order.isEmpty())
            {
                ctx.orders.group(to_order, a.getGridX(), a.getGridY(), true);
            }
        }
        // Java: 1524
        public void defend()
        {
            com.oddlabs.tt.model.Building a = ctx.economy.armory();
            foreach (com.oddlabs.tt.player.ultra.MilitaryManager.Threat t in java.lang.JIter.of(threats))
            {
                if (t.defenders.isEmpty() || canEngage(t))
                {
                    continue;
                }
                if (a != null)
                {
                    java.util.List far = new java.util.ArrayList();
                    foreach (com.oddlabs.tt.model.Unit w in java.lang.JIter.of(t.defenders))
                    {
                        if (com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(w.getGridX(), w.getGridY(), a.getGridX(), a.getGridY()) > 10 && !ctx.orders.recently(w, 3.0f))
                        {
                            far.add(w);
                        }
                    }
                    if (!far.isEmpty())
                    {
                        ctx.orders.group(far, a.getGridX(), a.getGridY(), false);
                    }
                }
            }
        }
        // Java: 1543
        public bool canEngage(com.oddlabs.tt.player.ultra.MilitaryManager.Threat t)
        {
            float ours = com.oddlabs.tt.player.ultra.CombatModel.sumWeights(t.defenders);
            float towers = 0.0f;
            foreach (com.oddlabs.tt.model.Building tw in java.lang.JIter.of(ctx.model.me.towers))
            {
                if (!tw.isDead() && com.oddlabs.tt.player.ultra.MapAnalysis.dist2(tw.getGridX(), tw.getGridY(), t.cx, t.cy) <= 18 * 18)
                {
                    towers = (float)(towers + com.oddlabs.tt.player.ultra.CombatModel.towerValue(tw));
                }
            }
            return (float)(ours + (float)(0.5f * towers)) >= (float)(0.8f * t.e) || t.e < 0.5f;
        }
        // Java: 1553
        public void idleHome()
        {
            if (((object)state_f != (object)com.oddlabs.tt.player.ultra.MilitaryManager.State.GATHER) && home.isEmpty())
            {
                return;
            }
            java.util.List squad = (((object)state_f == (object)com.oddlabs.tt.player.ultra.MilitaryManager.State.GATHER) ? main : home);
            java.util.List far = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit w in java.lang.JIter.of(squad))
            {
                if (com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(w.getGridX(), w.getGridY(), gather[0], gather[1]) > 9 && ((object)com.oddlabs.tt.player.ultra.WorldModel.activityOf(w) == (object)com.oddlabs.tt.player.ultra.WorldModel.Activity.IDLE) && !ctx.orders.recently(w, 5.0f))
                {
                    far.add(w);
                }
            }
            if (!far.isEmpty())
            {
                ctx.orders.group(far, gather[0], gather[1], false);
            }
        }
        // Java: 1571
        public void microTick()
        {
            rebuildSquads();
            dodgeTick();
            followFrozen();
            if (!raid.isEmpty() && ((object)raid_state == (object)com.oddlabs.tt.player.ultra.MilitaryManager.RaidState.STRIKE))
            {
                engage(raid, raid_x, raid_y, false, 12);
            }
            if (defending)
            {
                foreach (com.oddlabs.tt.player.ultra.MilitaryManager.Threat t in java.lang.JIter.of(threats))
                {
                    if (!t.defenders.isEmpty() && canEngage(t))
                    {
                        engage(t.defenders, t.cx, t.cy, !t.sites.isEmpty(), 16);
                    }
                }
            }
            if (((object)state_f == (object)com.oddlabs.tt.player.ultra.MilitaryManager.State.ENGAGE) || ((object)state_f == (object)com.oddlabs.tt.player.ultra.MilitaryManager.State.SIEGE) || (((object)state_f == (object)com.oddlabs.tt.player.ultra.MilitaryManager.State.MARCH) && enemiesNear(main, 14)))
            {
                int[] hold = stunSiegeHold();
                if (hold != null && hold[2] == 0)
                {
                    holdAt(hold);
                }
                else if (hold != null)
                {
                    engage(main, hold[0], hold[1], false, engage_range);
                }
                else
                {
                    int[] fp = fightPoint();
                    engage(main, fp[0], fp[1], ((object)state_f == (object)com.oddlabs.tt.player.ultra.MilitaryManager.State.SIEGE), engage_range + ((tootActive() || ctx.now < frozen_until ? 8 : 0)));
                }
            }
        }
        public float toot_order_time;
        public int gate_x;
        public int gate_y;
        public float gate_t;
        public float gate_need;
        // Java: 1607
        public float towerLine(int x, int y, int r)
        {
            float sum = 0.0f;
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(ctx.model.enemy.towers))
            {
                if (!t.isDead() && com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(t.getGridX(), t.getGridY(), x, y) <= r)
                {
                    sum = (float)(sum + (float)(com.oddlabs.tt.player.ultra.CombatModel.towerValue(t) * tower_lin_w));
                }
            }
            return sum;
        }
        // Java: 1616
        public int mannedTowers(int x, int y, int r)
        {
            int n = 0;
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(ctx.model.enemy.towers))
            {
                if (!t.isDead() && com.oddlabs.tt.player.ultra.CombatModel.towerValue(t) > 0.0f && com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(t.getGridX(), t.getGridY(), x, y) <= r)
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 1627
        public float groundStrength(int x, int y)
        {
            float s = 0.0f;
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(ctx.model.enemy.warriors))
            {
                if (!com.oddlabs.tt.player.ultra.WorldModel.isStunned(e) && com.oddlabs.tt.player.ultra.MapAnalysis.dist2(e.getGridX(), e.getGridY(), x, y) <= 625)
                {
                    s = (float)(s + com.oddlabs.tt.player.ultra.CombatModel.weight(e));
                }
            }
            com.oddlabs.tt.model.Unit ec = ctx.model.enemy.chieftain;
            if (ec != null && !ec.isDead() && !com.oddlabs.tt.player.ultra.WorldModel.isStunned(ec) && com.oddlabs.tt.player.ultra.MapAnalysis.dist2(ec.getGridX(), ec.getGridY(), x, y) <= 900)
            {
                s = (float)(s + assault_chief);
            }
            return (float)(s + towerLine(x, y, assault_r));
        }
        // Java: 1640
        public float oursNearAnchor()
        {
            com.oddlabs.tt.model.Unit a = anchor(main);
            float ours = 0.0f;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(main))
            {
                if (com.oddlabs.tt.player.ultra.MapAnalysis.dist2(u.getGridX(), u.getGridY(), a.getGridX(), a.getGridY()) <= 625)
                {
                    ours = (float)(ours + com.oddlabs.tt.player.ultra.CombatModel.weight(u));
                }
            }
            return ours;
        }
        // Java: 1650
        public bool assaultBlocked(int x, int y)
        {
            if (!assault_gate || closeout || main.isEmpty() || ctx.now < (float)(toot_order_time + 4.5f))
            {
                return false;
            }
            if (mannedTowers(x, y, assault_r) < 2)
            {
                return false;
            }
            if (siege_holding)
            {
                com.oddlabs.tt.model.Unit a = anchor(main);
                if (!nearMannedEnemyTower(a.getGridX(), a.getGridY(), 12))
                {
                    return false;
                }
            }
            return oursNearAnchor() < (float)(assault_ratio * groundStrength(x, y));
        }
        // Java: 1665
        public bool gateMemoryBlocks(com.oddlabs.tt.model.Building b, float ours, int fx, int fy)
        {
            if ((float)(ctx.now - gate_t) >= assault_memory || mannedTowers(gate_x, gate_y, assault_r) < 2)
            {
                return false;
            }
            int tx = b.getGridX();
            int ty = b.getGridY();
            bool near = com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(gate_x, gate_y, tx, ty) <= 40;
            if (!near)
            {
                float vx = tx - fx;
                float vy = ty - fy;
                float l2 = (float)((float)(vx * vx) + (float)(vy * vy));
                float px = gate_x - fx;
                float py = gate_y - fy;
                float u = (l2 < 1.0f ? 0.0f : java.lang.Math.max(0.0f, java.lang.Math.min(1.0f, (float)(((float)((float)(px * vx) + (float)(py * vy))) / l2))));
                float dx = (float)(px - (float)(u * vx));
                float dy = (float)(py - (float)(u * vy));
                near = (float)((float)(dx * dx) + (float)(dy * dy)) <= assault_r * assault_r;
            }
            return near && ours < java.lang.Math.max(gate_need, (float)(assault_ratio * groundStrength(gate_x, gate_y)));
        }
        // Java: 1685
        public void stopPush(int x, int y)
        {
            ctx.log(new java.util.function.Supplier_L(() =>
            {
                return "push stopped at tower line " + (x) + "," + (y);
            }));
            gate_x = x;
            gate_y = y;
            gate_t = ctx.now;
            gate_need = (float)((float)((float)(com.oddlabs.tt.player.ultra.CombatModel.sumWeights(main) * assault_ratio) * groundStrength(x, y)) / java.lang.Math.max(1.0f, oursNearAnchor()));
            if (ctx.owner.getUnitsLost() == attack_lost0 && ctx.owner.getUnitsKilled() == attack_killed0)
            {
                attack_active = false;
            }
            retreat();
        }
        // Java: 1698
        public float approachLine(int fx, int fy, int tx, int ty)
        {
            float sum = 0.0f;
            float vx = tx - fx;
            float vy = ty - fy;
            float l2 = (float)((float)(vx * vx) + (float)(vy * vy));
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(ctx.model.enemy.towers))
            {
                if (t.isDead())
                {
                    continue;
                }
                float v = com.oddlabs.tt.player.ultra.CombatModel.towerValue(t);
                if (v <= 0.0f)
                {
                    continue;
                }
                float px = t.getGridX() - fx;
                float py = t.getGridY() - fy;
                float u = (l2 < 1.0f ? 0.0f : java.lang.Math.max(0.0f, java.lang.Math.min(1.0f, (float)(((float)((float)(px * vx) + (float)(py * vy))) / l2))));
                float dx = (float)(px - (float)(u * vx));
                float dy = (float)(py - (float)(u * vy));
                bool near_path = (float)((float)(dx * dx) + (float)(dy * dy)) <= 16 * 16;
                bool near_target = com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(t.getGridX(), t.getGridY(), tx, ty) <= 26;
                if (near_path || near_target)
                {
                    sum = (float)(sum + (float)(v * tower_lin_w));
                }
            }
            return sum;
        }
        // Java: 1723
        public bool pressOnOk()
        {
            if (!assault_gate || objective == null)
            {
                return true;
            }
            com.oddlabs.tt.model.Unit a = anchor(main);
            float theirs = (float)(enemyFieldStrength() + approachLine(a.getGridX(), a.getGridY(), obj_x, obj_y));
            return main.size() >= attack_min / 2 && com.oddlabs.tt.player.ultra.CombatModel.sumWeights(main) >= (float)((float)(grind_ratio * defense_mult) * theirs);
        }
        // Java: 1735
        public void followFrozen()
        {
            if (!frozen_window || main.isEmpty())
            {
                return;
            }
            com.oddlabs.tt.model.Unit a = anchor(main);
            int n = 0;
            float weight = 0.0f;
            long sx = 0;
            long sy = 0;
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(ctx.model.enemy.warriors))
            {
                if (com.oddlabs.tt.player.ultra.WorldModel.isStunned(e) && com.oddlabs.tt.player.ultra.MapAnalysis.dist2(e.getGridX(), e.getGridY(), a.getGridX(), a.getGridY()) <= 22 * 22)
                {
                    n++;
                    weight = (float)(weight + com.oddlabs.tt.player.ultra.CombatModel.weight(e));
                    sx += e.getGridX();
                    sy += e.getGridY();
                }
            }
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(ctx.model.enemy.towers))
            {
                com.oddlabs.tt.model.Unit g = com.oddlabs.tt.player.ultra.WorldModel.garrisonOf(t);
                if (g != null && com.oddlabs.tt.player.ultra.WorldModel.isStunned(g) && com.oddlabs.tt.player.ultra.MapAnalysis.dist2(t.getGridX(), t.getGridY(), a.getGridX(), a.getGridY()) <= 22 * 22)
                {
                    n++;
                    weight = (float)(weight + 2.0f);
                    sx += t.getGridX();
                    sy += t.getGridY();
                }
            }
            if (n < frozen_min || weight < 4.0f)
            {
                return;
            }
            frozen_until = (float)(ctx.now + 1.0f);
            toot_x = unchecked((int)((sx / n)));
            toot_y = unchecked((int)((sy / n)));
            bool gated = ctx.now < recall_until || (assault_gate && ((float)(ctx.now - gate_t) < 5.0f || assaultBlocked(a.getGridX(), a.getGridY())));
            if ((((object)state_f == (object)com.oddlabs.tt.player.ultra.MilitaryManager.State.RETREAT) || ((object)state_f == (object)com.oddlabs.tt.player.ultra.MilitaryManager.State.GATHER)) && !gated && localRatio(main) >= frozen_reengage)
            {
                last_enemy_near = ctx.now;
                toot_return = true;
                setState(com.oddlabs.tt.player.ultra.MilitaryManager.State.ENGAGE);
                int k = n;
                ctx.log(new java.util.function.Supplier_L(() =>
                {
                    return "stun window: " + (k) + " frozen enemies, re-engaging";
                }));
            }
        }
        public float sieged_until;
        public float sieged_since;
        public float siege_checked;
        public int[] rear;
        public float rear_time;
        // Java: 1789
        public bool armorySieged()
        {
            if (siege_checked != ctx.now)
            {
                siege_checked = ctx.now;
                updateArmorySiege();
            }
            return ctx.now < sieged_until;
        }
        // Java: 1798
        public float armorySiegedSince()
        {
            return sieged_since;
        }
        // Java: 1803
        public int[] rearPoint()
        {
            return (armorySieged() ? rear : (int[])(null));
        }
        // Java: 1807
        public void updateArmorySiege()
        {
            com.oddlabs.tt.model.Building a = ctx.economy.armory();
            if (!siege_evac || a == null || a.isDead())
            {
                sieged_until = java.lang.Float.NEGATIVE_INFINITY;
                rear = null;
                return;
            }
            float enemy = 0.0f;
            long sx = 0;
            long sy = 0;
            int n = 0;
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(ctx.model.enemy.warriors))
            {
                if (com.oddlabs.tt.player.ultra.WorldModel.isStunned(e) || com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(e.getGridX(), e.getGridY(), a.getGridX(), a.getGridY()) > 14)
                {
                    continue;
                }
                enemy = (float)(enemy + com.oddlabs.tt.player.ultra.CombatModel.weight(e));
                sx += e.getGridX();
                sy += e.getGridY();
                n++;
            }
            if (n > 0)
            {
                float def = 0.0f;
                foreach (com.oddlabs.tt.model.Unit w in java.lang.JIter.of(ctx.model.me.warriors))
                {
                    if (com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(w.getGridX(), w.getGridY(), a.getGridX(), a.getGridY()) <= 14)
                    {
                        def = (float)(def + com.oddlabs.tt.player.ultra.CombatModel.weight(w));
                    }
                }
                foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(ctx.model.me.towers))
                {
                    if (!t.isDead() && com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(t.getGridX(), t.getGridY(), a.getGridX(), a.getGridY()) <= 12)
                    {
                        def = (float)(def + (float)(0.5f * com.oddlabs.tt.player.ultra.CombatModel.towerValue(t)));
                    }
                }
                float hp = (float)(a.getHitPoints() / ((float)(((com.oddlabs.tt.model.BuildingTemplate)a.getTemplate()).getMaxHitPoints())));
                if ((hp < siege_hp && enemy > (float)(0.5f * def)) || enemy > (float)(def + 2.0f))
                {
                    if (ctx.now >= sieged_until)
                    {
                        sieged_since = ctx.now;
                        float e_2 = enemy;
                        float d = def;
                        ctx.log(new java.util.function.Supplier_L(() =>
                        {
                            return java.lang.JString.format("armory siege: enemy %.1f vs defence %.1f, peons kept out", e_2, d);
                        }));
                    }
                    sieged_until = (float)(ctx.now + 5.0f);
                    if (rear == null || (float)(ctx.now - rear_time) > 20.0f || ((float)(ctx.now - rear_time) > 2.0f && ctx.resources.threatened(rear[0], rear[1], 14)))
                    {
                        rear = computeRear(a, unchecked((int)((sx / n))), unchecked((int)((sy / n))));
                        rear_time = ctx.now;
                    }
                }
            }
            if (ctx.now >= sieged_until)
            {
                rear = null;
            }
        }
        // Java: 1859
        public int[] computeRear(com.oddlabs.tt.model.Building a, int cx, int cy)
        {
            int bx = (-1);
            int by = (-1);
            int best = (-1);
            java.util.List cands = new java.util.ArrayList(ctx.model.me.quarters);
            cands.addAll(ctx.model.me.towers);
            foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(cands))
            {
                if (b.isDead() || ((object)b == (object)a) || com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(b.getGridX(), b.getGridY(), a.getGridX(), a.getGridY()) > 70 || ctx.resources.threatened(b.getGridX(), b.getGridY(), 14) || ctx.map.frontness(b.getGridX(), b.getGridY()) > ctx.map.frontness(a.getGridX(), a.getGridY()))
                {
                    continue;
                }
                int d = com.oddlabs.tt.player.ultra.MapAnalysis.dist2(b.getGridX(), b.getGridY(), cx, cy);
                if (d > best)
                {
                    best = d;
                    bx = b.getGridX();
                    by = b.getGridY();
                }
            }
            if (best < 0)
            {
                float dx = a.getGridX() - cx;
                float dy = a.getGridY() - cy;
                float len = ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy)))));
                if (len < 6.0f)
                {
                    dx = a.getGridX() - ctx.map.enemy_x;
                    dy = a.getGridY() - ctx.map.enemy_y;
                    len = java.lang.Math.max(1.0f, ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy))))));
                }
                bx = ctx.map.clamp(java.lang.Math.round((float)(a.getGridX() + (float)((float)(dx / len) * 25.0f))));
                by = ctx.map.clamp(java.lang.Math.round((float)(a.getGridY() + (float)((float)(dy / len) * 25.0f))));
            }
            return ctx.map.nearestAccessible(bx, by);
        }
        public const float CAST_WINDOW = 3.9f;
        // Java: 1900
        public class Cast : java.lang.Object
        {
            public com.oddlabs.tt.model.Unit by;
            public float seen;
            public float x;
            public float y;
            public float until;
            public bool doomed;
            public java.util.Set collected;
            public Cast(com.oddlabs.tt.model.Unit by, float seen, float x, float y, float until) : base()
            {
                this.by = by;
                this.seen = seen;
                this.x = x;
                this.y = y;
                this.until = until;
            }
        }
        public java.util.List casts;
        public java.util.List casting;
        public float cast_seen;
        public float own_cast_seen;
        // Java: 1927
        public float[] pendingEnemyCastNear(int x, int y)
        {
            com.oddlabs.tt.player.ultra.MilitaryManager.Cast best = null;
            float best_d = java.lang.Float.MAX_VALUE;
            foreach (com.oddlabs.tt.player.ultra.MilitaryManager.Cast c in java.lang.JIter.of(casts))
            {
                if (ctx.now >= c.until || c.doomed)
                {
                    continue;
                }
                float d = (float)((float)(((float)(c.x - x)) * ((float)(c.x - x))) + (float)(((float)(c.y - y)) * ((float)(c.y - y))));
                if (d < best_d)
                {
                    best_d = d;
                    best = c;
                }
            }
            return (best == null ? (float[])(null) : new float[] { best.x, best.y, best.until });
        }
        // Java: 1943
        public float lastEnemyCast()
        {
            return cast_seen;
        }
        // Java: 1947
        public void dodgeTick()
        {
            com.oddlabs.tt.model.Unit mc = ctx.owner.getChieftain();
            bool mine = mc != null && !mc.isDead() && (mc.getCurrentBehaviour() is com.oddlabs.tt.model.behaviour.MagicBehaviour);
            if (!mine)
            {
                own_cast_seen = java.lang.Float.NEGATIVE_INFINITY;
            }
            else if (own_cast_seen == java.lang.Float.NEGATIVE_INFINITY)
            {
                own_cast_seen = ctx.now;
            }
            bool interrupted = false;
            for (java.util.Iterator it = casts.iterator(); it.hasNext(); )
            {
                com.oddlabs.tt.player.ultra.MilitaryManager.Cast c = ((com.oddlabs.tt.player.ultra.MilitaryManager.Cast)it.next());
                if (ctx.now >= c.until)
                {
                    it.remove();
                }
                else if (c.by.isDead() || com.oddlabs.tt.player.ultra.WorldModel.isStunned(c.by))
                {
                    it.remove();
                    interrupted = true;
                    ctx.log(new java.util.function.Supplier_L(() =>
                    {
                        return "enemy cast interrupted";
                    }));
                }
                else if (c.doomed && !mine)
                {
                    c.doomed = false;
                }
            }
            if (interrupted && dodge)
            {
                ctx.orders.releaseHolds();
            }
            casting.removeIf(new java.util.function.Predicate_L((object u_l) =>
            {
                com.oddlabs.tt.model.Unit u = (com.oddlabs.tt.model.Unit)u_l;
                return u.isDead() || ((object)com.oddlabs.tt.player.ultra.WorldModel.visibleActivity(u) != (object)com.oddlabs.tt.player.ultra.WorldModel.Activity.MAGIC);
            }));
            foreach (com.oddlabs.tt.model.Unit ec in java.lang.JIter.of(ctx.model.enemy.chieftains))
            {
                if (casting.contains(ec) || ((object)com.oddlabs.tt.player.ultra.WorldModel.visibleActivity(ec) != (object)com.oddlabs.tt.player.ultra.WorldModel.Activity.MAGIC))
                {
                    continue;
                }
                casting.add(ec);
                com.oddlabs.tt.player.ultra.MilitaryManager.Cast c_2 = new com.oddlabs.tt.player.ultra.MilitaryManager.Cast(ec, ctx.now, (float)(ec.getGridX() + (float)(ec.getDirectionX() * 1.3f)), (float)(ec.getGridY() + (float)(ec.getDirectionY() * 1.3f)), (float)(ctx.now + com.oddlabs.tt.player.ultra.MilitaryManager.CAST_WINDOW));
                c_2.doomed = dodge_smart && mine && own_cast_seen <= (float)(ctx.now - 0.25f) && com.oddlabs.tt.player.ultra.MapAnalysis.dist2(ec.getGridX(), ec.getGridY(), mc.getGridX(), mc.getGridY()) <= (float)(16.5f * 16.5f);
                casts.add(c_2);
                cast_seen = ctx.now;
                float x = c_2.x;
                float y = c_2.y;
                bool d = c_2.doomed;
                ctx.log(new java.util.function.Supplier_L(() =>
                {
                    return java.lang.JString.format("enemy cast at %.0f,%.0f%s", x, y, (d ? (object)(" (our stun lands first)") : (object)("")));
                }));
            }
            if (!dodge)
            {
                return;
            }
            java.util.List live = new java.util.ArrayList();
            foreach (com.oddlabs.tt.player.ultra.MilitaryManager.Cast c_3 in java.lang.JIter.of(casts))
            {
                if (!casting.contains(c_3.by) || c_3.doomed)
                {
                    continue;
                }
                if (dodge_smart)
                {
                    if (ctx.now >= (float)(c_3.seen + 3.77f))
                    {
                        continue;
                    }
                    if (c_3.collected == null && ctx.now >= (float)(c_3.seen + 1.9f))
                    {
                        java.util.Set set = new java.util.LinkedHashSet();
                        foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(ctx.model.me.warriors))
                        {
                            if (com.oddlabs.tt.player.ultra.MapAnalysis.dist2(u_2.getGridX(), u_2.getGridY(), c_3.by.getGridX(), c_3.by.getGridY()) <= 19 * 19)
                            {
                                set.add(u_2);
                            }
                        }
                        c_3.collected = set;
                    }
                }
                live.add(c_3);
            }
            if (live.isEmpty())
            {
                return;
            }
            int moved = 0;
            moved += dodgeUnits(ctx.model.me.warriors, live);
            if (dodge_peons)
            {
                moved += dodgeUnits(ctx.model.me.peons, live);
            }
            int n = moved;
            if (n > 0)
            {
                ctx.log(new java.util.function.Supplier_L(() =>
                {
                    return "DODGE moved " + (n);
                }));
            }
        }
        // Java: 2020
        public int dodgeUnits(java.util.List units, java.util.List live)
        {
            int moved = 0;
            float min2 = (float)(dodge_min * dodge_min);
            float max2 = (float)(dodge_max * dodge_max);
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(units))
            {
                if (!ctx.orders.canOrder(u))
                {
                    continue;
                }
                com.oddlabs.tt.player.ultra.MilitaryManager.Cast c = ((com.oddlabs.tt.player.ultra.MilitaryManager.Cast)live.get(0));
                float dx = (float)(u.getGridX() - c.x);
                float dy = (float)(u.getGridY() - c.y);
                float d2 = (float)((float)(dx * dx) + (float)(dy * dy));
                for (int i = 1; i < live.size(); i++)
                {
                    com.oddlabs.tt.player.ultra.MilitaryManager.Cast o = ((com.oddlabs.tt.player.ultra.MilitaryManager.Cast)live.get(i));
                    float ox = (float)(u.getGridX() - o.x);
                    float oy = (float)(u.getGridY() - o.y);
                    if ((float)((float)(ox * ox) + (float)(oy * oy)) < d2)
                    {
                        c = o;
                        dx = ox;
                        dy = oy;
                        d2 = (float)((float)(ox * ox) + (float)(oy * oy));
                    }
                }
                if (d2 < min2 || d2 > max2 || d2 < 1.0f)
                {
                    continue;
                }
                if (c.collected != null && !c.collected.contains(u))
                {
                    continue;
                }
                float d = ((float)(java.lang.Math.sqrt(d2)));
                int tx = ctx.map.clamp(java.lang.Math.round((float)(c.x + (float)((float)(dx / d) * dodge_to))));
                int ty = ctx.map.clamp(java.lang.Math.round((float)(c.y + (float)((float)(dy / d) * dodge_to))));
                int[] acc = ctx.map.nearestAccessible(tx, ty);
                if (acc == null || nearMannedEnemyTower(acc[0], acc[1], 16))
                {
                    continue;
                }
                ctx.orders.dodge(u, acc[0], acc[1], java.lang.Math.min((float)(ctx.now + dodge_hold), c.until));
                moved++;
            }
            return moved;
        }
        // Java: 2060
        public bool nearMannedEnemyTower(int x, int y, int r)
        {
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(ctx.model.enemy.towers))
            {
                if (!t.isDead() && com.oddlabs.tt.player.ultra.CombatModel.towerValue(t) > 0.0f && com.oddlabs.tt.player.ultra.MapAnalysis.dist2(t.getGridX(), t.getGridY(), x, y) < r * r)
                {
                    return true;
                }
            }
            return false;
        }
        public com.oddlabs.tt.model.Building siege_obj;
        public com.oddlabs.tt.model.Building siege_done_obj;
        public float siege_wait_since;
        public bool siege_released;
        public bool siege_holding;
        // Java: 2079
        public void resetSiege()
        {
            siege_wait_since = (float)(-1.0f);
            siege_released = false;
            siege_holding = false;
        }
        // Java: 2086
        public int[] stunSiegeHold()
        {
            if (!stun_siege || main.isEmpty() || objective == null || objective.isDead())
            {
                resetSiege();
                return null;
            }
            if (((object)objective != (object)siege_obj))
            {
                siege_obj = objective;
                resetSiege();
            }
            if (((object)objective == (object)siege_done_obj))
            {
                return null;
            }
            if (tootActive())
            {
                if (siege_holding)
                {
                    siege_done_obj = objective;
                }
                resetSiege();
                return null;
            }
            com.oddlabs.tt.model.Unit a = anchor(main);
            com.oddlabs.tt.model.Unit chief = ctx.owner.getChieftain();
            if (chief == null || chief.isDead() || chief.getHitPoints() < 25 || chief.getMagicProgress(com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_STUN) < 0.6f || com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(chief.getGridX(), chief.getGridY(), a.getGridX(), a.getGridY()) > 30)
            {
                resetSiege();
                return null;
            }
            java.util.List cover = new java.util.ArrayList();
            java.util.List reach = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(ctx.model.enemy.towers))
            {
                if (t.isDead() || com.oddlabs.tt.player.ultra.CombatModel.towerValue(t) <= 0.0f)
                {
                    continue;
                }
                int d = com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(t.getGridX(), t.getGridY(), obj_x, obj_y);
                if (d <= 20)
                {
                    cover.add(t);
                }
                if (d <= 40)
                {
                    reach.add(t);
                }
            }
            if (cover.size() < 2)
            {
                resetSiege();
                return null;
            }
            if (!siege_holding)
            {
                foreach (com.oddlabs.tt.model.Building t_2 in java.lang.JIter.of(cover))
                {
                    if (com.oddlabs.tt.player.ultra.MapAnalysis.dist2(t_2.getGridX(), t_2.getGridY(), a.getGridX(), a.getGridY()) < 16 * 16)
                    {
                        return null;
                    }
                }
            }
            int[] p = holdPoint(a, reach);
            if (p == null)
            {
                return null;
            }
            if (!siege_holding && com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(a.getGridX(), a.getGridY(), p[0], p[1]) > 30)
            {
                return null;
            }
            if (!siege_holding)
            {
                siege_holding = true;
                int n = cover.size();
                ctx.log(new java.util.function.Supplier_L(() =>
                {
                    return "stun siege: holding outside " + (n) + " towers";
                }));
            }
            if (siege_wait_since < 0.0f && com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(a.getGridX(), a.getGridY(), p[0], p[1]) <= 10)
            {
                siege_wait_since = ctx.now;
            }
            if (siege_wait_since >= 0.0f && (float)(ctx.now - siege_wait_since) > 60.0f)
            {
                siege_done_obj = objective;
                resetSiege();
                return null;
            }
            bool close = enemyWarriorsNear(main, (siege_released ? 16 : 12));
            siege_released = close;
            return new int[] { p[0], p[1], (close ? 1 : 0) };
        }
        // Java: 2158
        public int[] holdPoint(com.oddlabs.tt.model.Unit a, java.util.List towers)
        {
            float dx = a.getGridX() - obj_x;
            float dy = a.getGridY() - obj_y;
            float len = ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy)))));
            if (len < 1.0f)
            {
                return null;
            }
            for (int step = 0; step <= 44; step += 2)
            {
                int px = ctx.map.clamp(java.lang.Math.round((float)(obj_x + (float)((float)(dx / len) * step))));
                int py = ctx.map.clamp(java.lang.Math.round((float)(obj_y + (float)((float)(dy / len) * step))));
                int[] acc = ctx.map.nearestAccessible(px, py);
                if (acc == null)
                {
                    continue;
                }
                if (!com.oddlabs.tt.player.ultra.MilitaryManager.clearOfTowers(acc[0], acc[1], towers))
                {
                    continue;
                }
                int[] field = route;
                if (field != null && field[acc[1] * ctx.map.n + acc[0]] == com.oddlabs.tt.player.ultra.MapAnalysis.UNREACHABLE)
                {
                    continue;
                }
                return acc;
            }
            return null;
        }
        // Java: 2180
        public static bool clearOfTowers(int x, int y, java.util.List towers)
        {
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(towers))
            {
                if (com.oddlabs.tt.player.ultra.MapAnalysis.dist2(t.getGridX(), t.getGridY(), x, y) < 18 * 18)
                {
                    return false;
                }
            }
            return true;
        }
        // Java: 2188
        public void holdAt(int[] p)
        {
            int tolerance = 3 + java.lang.Math.round((float)(1.2f * ((float)(java.lang.Math.sqrt(main.size())))));
            java.util.List go = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit w in java.lang.JIter.of(main))
            {
                if (com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(w.getGridX(), w.getGridY(), p[0], p[1]) > tolerance && !ctx.orders.recently(w, com.oddlabs.tt.player.ultra.Orders.Kind.ATTACK_MOVE, null, p[0], p[1], 3.0f))
                {
                    go.add(w);
                }
            }
            if (!go.isEmpty())
            {
                ctx.orders.group(go, p[0], p[1], true);
            }
        }
        // Java: 2201
        public bool stunSieging()
        {
            return siege_holding;
        }
        public float chief_target_value;
        // Java: 2208
        public float @value(com.oddlabs.tt.model.Unit e)
        {
            {
                switch ((com.oddlabs.tt.player.ultra.WorldModel.kindOf(e)).ordinal())
                {
                    case 3 /*RUBBER*/:
                    {
                        return 1.7f;
                    }
                    case 2 /*IRON*/:
                    {
                        return 1.0f;
                    }
                    case 1 /*ROCK*/:
                    {
                        return 0.55f;
                    }
                    case 4 /*CHIEFTAIN*/:
                    {
                        return chief_target_value;
                    }
                    case 0 /*PEON*/:
                    {
                        com.oddlabs.tt.player.ultra.WorldModel.Activity a = com.oddlabs.tt.player.ultra.WorldModel.visibleActivity(e);
                        if (((object)a == (object)com.oddlabs.tt.player.ultra.WorldModel.Activity.HUNT))
                        {
                            return 0.8f;
                        }
                        if (((object)a == (object)com.oddlabs.tt.player.ultra.WorldModel.Activity.BUILD))
                        {
                            return 0.9f;
                        }
                        return 0.3f;
                    }
                }
                throw new System.InvalidOperationException("MatchException");
            }
        }
        // Java: 2229
        public void engage(java.util.List squad, int fx, int fy, bool siege, int reach)
        {
            if (squad.isEmpty())
            {
                return;
            }
            int minx = java.lang.Integer.MAX_VALUE;
            int miny = java.lang.Integer.MAX_VALUE;
            int maxx = java.lang.Integer.MIN_VALUE;
            int maxy = java.lang.Integer.MIN_VALUE;
            foreach (com.oddlabs.tt.model.Unit w in java.lang.JIter.of(squad))
            {
                minx = java.lang.Math.min(minx, w.getGridX());
                miny = java.lang.Math.min(miny, w.getGridY());
                maxx = java.lang.Math.max(maxx, w.getGridX());
                maxy = java.lang.Math.max(maxy, w.getGridY());
            }
            minx -= reach;
            miny -= reach;
            maxx += reach;
            maxy += reach;
            java.util.List enemies = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(ctx.model.enemy.units))
            {
                int x = e.getGridX();
                int y = e.getGridY();
                if (x >= minx && x <= maxx && y >= miny && y <= maxy)
                {
                    enemies.add(e);
                }
            }
            java.util.List buildings = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(ctx.model.enemy.buildings))
            {
                int x_2 = b.getGridX();
                int y_2 = b.getGridY();
                if (!b.isDead() && x_2 >= minx - 6 && x_2 <= maxx + 6 && y_2 >= miny - 6 && y_2 <= maxy + 6)
                {
                    buildings.add(b);
                }
            }
            if (enemies.isEmpty() && buildings.isEmpty())
            {
                java.util.List go = new java.util.ArrayList();
                foreach (com.oddlabs.tt.model.Unit w_2 in java.lang.JIter.of(squad))
                {
                    if (!ctx.orders.recently(w_2, com.oddlabs.tt.player.ultra.Orders.Kind.ATTACK_MOVE, null, fx, fy, 4.0f) && com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(w_2.getGridX(), w_2.getGridY(), fx, fy) > 4)
                    {
                        go.add(w_2);
                    }
                }
                if (!go.isEmpty())
                {
                    ctx.orders.group(go, fx, fy, true);
                }
                return;
            }
            if (!micro)
            {
                int nx = fx;
                int ny = fy;
                if (!enemies.isEmpty())
                {
                    nx = ((com.oddlabs.tt.model.Unit)enemies.get(0)).getGridX();
                    ny = ((com.oddlabs.tt.model.Unit)enemies.get(0)).getGridY();
                }
                java.util.List go_2 = new java.util.ArrayList();
                foreach (com.oddlabs.tt.model.Unit w_3 in java.lang.JIter.of(squad))
                {
                    if (!((w_3.getCurrentController() is com.oddlabs.tt.model.behaviour.HuntController)) && !ctx.orders.recently(w_3, 3.0f))
                    {
                        go_2.add(w_3);
                    }
                }
                if (!go_2.isEmpty())
                {
                    ctx.orders.group(go_2, nx, ny, true);
                }
                return;
            }
            java.util.Map survive = new java.util.LinkedHashMap();
            foreach (com.oddlabs.tt.model.Unit w_4 in java.lang.JIter.of(squad))
            {
                if ((w_4.getCurrentController() is com.oddlabs.tt.model.behaviour.HuntController hc) && (hc.getTarget() is com.oddlabs.tt.model.Unit t) && !t.isDead())
                {
                    float p = java.lang.Math.min(0.99f, com.oddlabs.tt.player.ultra.CombatModel.hitChance(w_4, t, ctx.map));
                    survive.merge(t, (float)(1.0f - p), new java.util.function.BiFunction_L((object a_l, object b_2_l) =>
                    {
                        float? a = (float?)a_l;
                        float? b_2 = (float?)b_2_l;
                        return (float)(((float)a) * ((float)b_2));
                    }));
                }
            }
            java.util.Set on_tower = new java.util.LinkedHashSet();
            if (stun_tower_focus)
            {
                foreach (com.oddlabs.tt.model.Building b_3 in java.lang.JIter.of(buildings))
                {
                    if (!b_3.isComplete() || !b_3.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.ATTACK))
                    {
                        continue;
                    }
                    com.oddlabs.tt.model.Unit g = com.oddlabs.tt.player.ultra.WorldModel.garrisonOf(b_3);
                    if (g == null || !com.oddlabs.tt.player.ultra.WorldModel.isStunned(g))
                    {
                        continue;
                    }
                    java.util.List cands = new java.util.ArrayList();
                    foreach (com.oddlabs.tt.model.Unit w_5 in java.lang.JIter.of(squad))
                    {
                        if (!on_tower.contains(w_5) && ctx.orders.canOrder(w_5) && com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(w_5.getGridX(), w_5.getGridY(), b_3.getGridX(), b_3.getGridY()) <= 14)
                        {
                            cands.add(w_5);
                        }
                    }
                    cands.sort(java.util.Comparator_.comparingInt(new java.util.function.ToIntFunction_L((object w_6_l) =>
                    {
                        com.oddlabs.tt.model.Unit w_6 = (com.oddlabs.tt.model.Unit)w_6_l;
                        return com.oddlabs.tt.player.ultra.MapAnalysis.dist2(w_6.getGridX(), w_6.getGridY(), b_3.getGridX(), b_3.getGridY());
                    })));
                    for (int i = 0; i < java.lang.Math.min(6, cands.size()); i++)
                    {
                        com.oddlabs.tt.model.Unit w_7 = ((com.oddlabs.tt.model.Unit)cands.get(i));
                        on_tower.add(w_7);
                        com.oddlabs.tt.model.Selectable cur = ((w_7.getCurrentController() is com.oddlabs.tt.model.behaviour.HuntController hc_2) ? hc_2.getTarget() : (com.oddlabs.tt.model.Selectable)(null));
                        if (((object)cur != (object)b_3) && !((w_7.getCurrentBehaviour() is com.oddlabs.tt.model.behaviour.AttackBehaviour)))
                        {
                            ctx.orders.attack(w_7, b_3);
                        }
                    }
                }
            }
            int nearest_x = fx;
            int nearest_y = fy;
            foreach (com.oddlabs.tt.model.Unit w_8 in java.lang.JIter.of(squad))
            {
                if (!ctx.orders.canOrder(w_8) || on_tower.contains(w_8))
                {
                    continue;
                }
                com.oddlabs.tt.model.Selectable current = ((w_8.getCurrentController() is com.oddlabs.tt.model.behaviour.HuntController hc_3) ? hc_3.getTarget() : (com.oddlabs.tt.model.Selectable)(null));
                bool mid_cycle = (w_8.getCurrentBehaviour() is com.oddlabs.tt.model.behaviour.AttackBehaviour);
                if (mid_cycle && current != null && !current.isDead() && (current is com.oddlabs.tt.model.Unit))
                {
                    continue;
                }
                com.oddlabs.tt.model.Unit best = null;
                float best_score = 0.0f;
                float best_p = 0.0f;
                int wx = w_8.getGridX();
                int wy = w_8.getGridY();
                int near_d = java.lang.Integer.MAX_VALUE;
                float own = ((current is com.oddlabs.tt.model.Unit cu) && !cu.isDead() ? java.lang.Math.min(0.99f, com.oddlabs.tt.player.ultra.CombatModel.hitChance(w_8, cu, ctx.map)) : 0.0f);
                foreach (com.oddlabs.tt.model.Unit e_2 in java.lang.JIter.of(enemies))
                {
                    int d2 = com.oddlabs.tt.player.ultra.MapAnalysis.dist2(wx, wy, e_2.getGridX(), e_2.getGridY());
                    if (d2 < near_d)
                    {
                        near_d = d2;
                        nearest_x = e_2.getGridX();
                        nearest_y = e_2.getGridY();
                    }
                    if (d2 > (float)(((float)(com.oddlabs.tt.player.ultra.MilitaryManager.UNIT_RANGE + 1.5f)) * ((float)(com.oddlabs.tt.player.ultra.MilitaryManager.UNIT_RANGE + 1.5f))))
                    {
                        continue;
                    }
                    float p_2 = com.oddlabs.tt.player.ultra.CombatModel.hitChance(w_8, e_2, ctx.map);
                    float? s0 = ((float?)survive.get(e_2));
                    float s = (s0 != null ? ((float)s0) : 1.0f);
                    if (((object)e_2 == (object)current))
                    {
                        s = (float)(s / ((float)(1.0f - own)));
                    }
                    float score = (float)((float)((float)((float)((float)(@value(e_2) * p_2) * java.lang.Math.max(s, 0.05f)) * ((s < 0.3f ? 0.2f : 1.0f))) * ((d2 <= (float)(com.oddlabs.tt.player.ultra.MilitaryManager.UNIT_RANGE * com.oddlabs.tt.player.ultra.MilitaryManager.UNIT_RANGE) ? 1.0f : 0.75f))) * ((((object)e_2 == (object)current) ? 1.1f : 1.0f)));
                    if (score > best_score)
                    {
                        best_score = score;
                        best = e_2;
                        best_p = p_2;
                    }
                }
                if (best != null)
                {
                    if (((object)best != (object)current))
                    {
                        ctx.orders.attack(w_8, best);
                        survive.merge(best, (float)(1.0f - best_p), new java.util.function.BiFunction_L((object a_2_l, object b_4_l) =>
                        {
                            float? a_2 = (float?)a_2_l;
                            float? b_4 = (float?)b_4_l;
                            return (float)(((float)a_2) * ((float)b_4));
                        }));
                    }
                    continue;
                }
                com.oddlabs.tt.model.Building target = null;
                float target_score = 0.0f;
                foreach (com.oddlabs.tt.model.Building b_5 in java.lang.JIter.of(buildings))
                {
                    int d2_2 = com.oddlabs.tt.player.ultra.MapAnalysis.dist2(wx, wy, b_5.getGridX(), b_5.getGridY());
                    bool tower = b_5.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.ATTACK);
                    float range = (tower ? com.oddlabs.tt.player.ultra.MilitaryManager.TOWER_TARGET_RANGE : 11.76f);
                    bool in_range = d2_2 <= (float)(range * range);
                    if (!in_range && !siege)
                    {
                        continue;
                    }
                    float score_2 = (float)(((tower ? 3.0f : (((object)b_5 == (object)objective) ? 2.0f : 1.0f))) / ((float)(1.0f + (float)(((float)(java.lang.Math.sqrt(d2_2))) / 10.0f))));
                    if (!b_5.isComplete())
                    {
                        score_2 = (float)(score_2 * 2.0f);
                    }
                    if (score_2 > target_score)
                    {
                        target_score = score_2;
                        target = b_5;
                    }
                }
                if (target != null)
                {
                    if (((object)current != (object)target))
                    {
                        ctx.orders.attack(w_8, target);
                    }
                    continue;
                }
                if (current == null && !ctx.orders.recently(w_8, 2.0f))
                {
                    if (!enemies.isEmpty())
                    {
                        ctx.orders.attackMove(w_8, nearest_x, nearest_y);
                    }
                    else
                    {
                        com.oddlabs.tt.model.Building nearest_tower = null;
                        int nt = java.lang.Integer.MAX_VALUE;
                        foreach (com.oddlabs.tt.model.Building b_6 in java.lang.JIter.of(buildings))
                        {
                            if (!b_6.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.ATTACK))
                            {
                                continue;
                            }
                            int d2_3 = com.oddlabs.tt.player.ultra.MapAnalysis.dist2(wx, wy, b_6.getGridX(), b_6.getGridY());
                            if (d2_3 < nt)
                            {
                                nt = d2_3;
                                nearest_tower = b_6;
                            }
                        }
                        if (nearest_tower != null && (siege || nt <= 20 * 20))
                        {
                            ctx.orders.attack(w_8, nearest_tower);
                        }
                        else if (com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(wx, wy, fx, fy) > 4)
                        {
                            ctx.orders.attackMove(w_8, fx, fy);
                        }
                    }
                }
            }
        }
        // Java: 2419
        public sealed class RaidState : java.lang.Enum
        {
            public static RaidState[] values() { return (RaidState[])VALUES_.Clone(); }
            public static RaidState valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly RaidState NONE = new RaidState("NONE", 0);
            public static readonly RaidState MOVE = new RaidState("MOVE", 1);
            public static readonly RaidState STRIKE = new RaidState("STRIKE", 2);
            public static readonly RaidState RETURN = new RaidState("RETURN", 3);
            public static readonly RaidState[] VALUES_ = new RaidState[] { NONE, MOVE, STRIKE, RETURN };
            public RaidState(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        public com.oddlabs.tt.player.ultra.MilitaryManager.RaidState raid_state;
        public int raid_x;
        public int raid_y;
        public float raid_since;
        public float raid_cooldown;
        // Java: 2432
        public void setRaid(com.oddlabs.tt.player.ultra.MilitaryManager.RaidState s)
        {
            raid_state = s;
            raid_since = ctx.now;
            int n = raid.size();
            ctx.log(new java.util.function.Supplier_L(() =>
            {
                return "raid " + java.lang.JStr.valueOf(s) + " size=" + (n) + " at " + (raid_x) + "," + (raid_y);
            }));
        }
        // Java: 2439
        public void raidTick()
        {
            if (!raid_enabled)
            {
                return;
            }
            if (((object)raid_state != (object)com.oddlabs.tt.player.ultra.MilitaryManager.RaidState.NONE) && raid.isEmpty())
            {
                raid_state = com.oddlabs.tt.player.ultra.MilitaryManager.RaidState.NONE;
                raid_cooldown = (float)(ctx.now + 30.0f);
                return;
            }
            switch ((raid_state).ordinal())
            {
                case 0 /*NONE*/:
                {
                    startRaid();
                    break;
                }
                case 1 /*MOVE*/:
                {
                    com.oddlabs.tt.model.Unit a = anchor(raid);
                    if (raidDanger(a.getGridX(), a.getGridY()))
                    {
                        returnRaid();
                    }
                    else if (com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(a.getGridX(), a.getGridY(), raid_x, raid_y) <= 12)
                    {
                        setRaid(com.oddlabs.tt.player.ultra.MilitaryManager.RaidState.STRIKE);
                    }
                    else if ((float)(ctx.now - raid_since) > 90.0f)
                    {
                        returnRaid();
                    }
                    else if (!ctx.orders.recently(((com.oddlabs.tt.model.Unit)raid.get(0)), 5.0f))
                    {
                        ctx.orders.group(raid, raid_x, raid_y, false);
                    }
                    break;
                }
                case 2 /*STRIKE*/:
                {
                    com.oddlabs.tt.model.Unit a_2 = anchor(raid);
                    if (raidDanger(a_2.getGridX(), a_2.getGridY()))
                    {
                        returnRaid();
                        return;
                    }
                    int[] next = raidTarget(a_2.getGridX(), a_2.getGridY(), 20);
                    if (next == null)
                    {
                        returnRaid();
                    }
                    else
                    {
                        raid_x = next[0];
                        raid_y = next[1];
                    }
                    break;
                }
                case 3 /*RETURN*/:
                {
                    resendStragglers(raid, raid_since);
                    com.oddlabs.tt.model.Unit a_3 = anchor(raid);
                    if (com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(a_3.getGridX(), a_3.getGridY(), gather[0], gather[1]) <= 10 || (float)(ctx.now - raid_since) > 90.0f)
                    {
                        foreach (com.oddlabs.tt.model.Unit w in java.lang.JIter.of(raid))
                        {
                            squad_of.put(w, com.oddlabs.tt.player.ultra.MilitaryManager.HOME);
                        }
                        raid.clear();
                        raid_state = com.oddlabs.tt.player.ultra.MilitaryManager.RaidState.NONE;
                        raid_cooldown = (float)(ctx.now + 30.0f);
                    }
                    break;
                }
            }
        }
        // Java: 2494
        public void resendStragglers(java.util.List squad, float since)
        {
            java.util.List late = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit w in java.lang.JIter.of(squad))
            {
                if (!ctx.orders.canOrder(w))
                {
                    continue;
                }
                com.oddlabs.tt.player.ultra.Orders.Last last = ctx.orders.lastOrder(w);
                if (last == null || last.time() < since || ((object)last.kind() == (object)com.oddlabs.tt.player.ultra.Orders.Kind.DODGE))
                {
                    late.add(w);
                }
            }
            if (!late.isEmpty())
            {
                ctx.orders.group(late, gather[0], gather[1], false);
            }
        }
        // Java: 2507
        public void returnRaid()
        {
            setRaid(com.oddlabs.tt.player.ultra.MilitaryManager.RaidState.RETURN);
            ctx.orders.group(raid, gather[0], gather[1], false);
        }
        // Java: 2512
        public void startRaid()
        {
            if (ctx.now < raid_cooldown || defending || ((object)state_f != (object)com.oddlabs.tt.player.ultra.MilitaryManager.State.GATHER))
            {
                return;
            }
            java.util.List pool = main;
            if (pool.size() < raid_size + 4)
            {
                return;
            }
            int[] t = raidTarget(gather[0], gather[1], 170);
            if (t == null)
            {
                return;
            }
            raid_x = t[0];
            raid_y = t[1];
            java.util.List sorted = new java.util.ArrayList(pool);
            sorted.sort(new java.util.Comparator_L((object x_l, object y_l) =>
            {
                com.oddlabs.tt.model.Unit x = (com.oddlabs.tt.model.Unit)x_l;
                com.oddlabs.tt.model.Unit y = (com.oddlabs.tt.model.Unit)y_l;
                return java.lang.Integer.compare(com.oddlabs.tt.player.ultra.MapAnalysis.dist2(x.getGridX(), x.getGridY(), raid_x, raid_y), com.oddlabs.tt.player.ultra.MapAnalysis.dist2(y.getGridX(), y.getGridY(), raid_x, raid_y));
            }));
            foreach (com.oddlabs.tt.model.Unit w in java.lang.JIter.of(sorted))
            {
                if (raid.size() >= raid_size)
                {
                    break;
                }
                if (((object)com.oddlabs.tt.player.ultra.WorldModel.kindOf(w) == (object)com.oddlabs.tt.player.ultra.WorldModel.Kind.ROCK))
                {
                    continue;
                }
                squad_of.put(w, com.oddlabs.tt.player.ultra.MilitaryManager.RAID);
                raid.add(w);
                main.remove(w);
            }
            if (raid.size() < java.lang.Math.max(2, raid_size - 1))
            {
                foreach (com.oddlabs.tt.model.Unit w_2 in java.lang.JIter.of(raid))
                {
                    squad_of.put(w_2, com.oddlabs.tt.player.ultra.MilitaryManager.MAIN);
                }
                main.addAll(raid);
                raid.clear();
                return;
            }
            setRaid(com.oddlabs.tt.player.ultra.MilitaryManager.RaidState.MOVE);
            ctx.orders.group(raid, raid_x, raid_y, false);
        }
        // Java: 2548
        public bool raidDanger(int x, int y)
        {
            float ours = com.oddlabs.tt.player.ultra.CombatModel.sumWeights(raid);
            float theirs = 0.0f;
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(ctx.model.enemy.warriors))
            {
                if (com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(e.getGridX(), e.getGridY(), x, y) <= 18 && !com.oddlabs.tt.player.ultra.WorldModel.isStunned(e))
                {
                    theirs = (float)(theirs + com.oddlabs.tt.player.ultra.CombatModel.weight(e));
                }
            }
            com.oddlabs.tt.model.Unit chief = ctx.model.enemy.chieftain;
            if (chief != null && com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(chief.getGridX(), chief.getGridY(), x, y) <= 18)
            {
                theirs = (float)(theirs + 1.5f);
            }
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(ctx.model.enemy.towers))
            {
                if (com.oddlabs.tt.player.ultra.WorldModel.garrisonOf(t) != null && com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(t.getGridX(), t.getGridY(), x, y) <= 17)
                {
                    return true;
                }
            }
            return theirs >= (float)(0.7f * ours);
        }
        // Java: 2570
        public int[] raidTarget(int fx, int fy, int reach)
        {
            int best_x = (-1);
            int best_y = (-1);
            float best_v = 1.5f;
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(ctx.model.enemy.peons))
            {
                int x = e.getGridX();
                int y = e.getGridY();
                if (com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(x, y, fx, fy) > reach)
                {
                    continue;
                }
                bool guarded = false;
                foreach (com.oddlabs.tt.model.Unit w in java.lang.JIter.of(ctx.model.enemy.warriors))
                {
                    if (com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(w.getGridX(), w.getGridY(), x, y) <= 22)
                    {
                        guarded = true;
                        break;
                    }
                }
                if (guarded)
                {
                    continue;
                }
                foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(ctx.model.enemy.towers))
                {
                    if (com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(t.getGridX(), t.getGridY(), x, y) <= 19)
                    {
                        guarded = true;
                        break;
                    }
                }
                if (guarded)
                {
                    continue;
                }
                int n = 0;
                foreach (com.oddlabs.tt.model.Unit o in java.lang.JIter.of(ctx.model.enemy.peons))
                {
                    if (com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(o.getGridX(), o.getGridY(), x, y) <= 8)
                    {
                        n++;
                    }
                }
                float v = (float)(n - (float)(((float)(java.lang.Math.sqrt(com.oddlabs.tt.player.ultra.MapAnalysis.dist2(x, y, fx, fy)))) / 60.0f));
                if (v > best_v)
                {
                    best_v = v;
                    best_x = x;
                    best_y = y;
                }
            }
            return (best_x < 0 ? (int[])(null) : new int[] { best_x, best_y });
        }
        private void init_MilitaryManager()
        {
            this.squad_of = new java.util.LinkedHashMap();
            this.home = new java.util.ArrayList();
            this.main = new java.util.ArrayList();
            this.reinf = new java.util.ArrayList();
            this.raid = new java.util.ArrayList();
            this.state_f = com.oddlabs.tt.player.ultra.MilitaryManager.State.GATHER;
            this.gather_time = java.lang.Float.NEGATIVE_INFINITY;
            this.intruders = new java.util.ArrayList();
            this.defense_mult = 1.0f;
            this.last_gather_trace = java.lang.Float.NEGATIVE_INFINITY;
            this.muster_until = java.lang.Float.NEGATIVE_INFINITY;
            this.toot_until = java.lang.Float.NEGATIVE_INFINITY;
            this.best_prog = java.lang.Integer.MAX_VALUE;
            this.leg_anchor_v = java.lang.Integer.MAX_VALUE;
            this.stalled = new java.util.LinkedHashMap();
            this.frozen_until = java.lang.Float.NEGATIVE_INFINITY;
            this.recall_until = java.lang.Float.NEGATIVE_INFINITY;
            this.threats = new java.util.ArrayList();
            this.strikers = new java.util.LinkedHashMap();
            this.toot_order_time = java.lang.Float.NEGATIVE_INFINITY;
            this.gate_t = java.lang.Float.NEGATIVE_INFINITY;
            this.sieged_until = java.lang.Float.NEGATIVE_INFINITY;
            this.sieged_since = java.lang.Float.NEGATIVE_INFINITY;
            this.siege_checked = java.lang.Float.NaN;
            this.rear_time = java.lang.Float.NEGATIVE_INFINITY;
            this.casts = new java.util.ArrayList();
            this.casting = new java.util.ArrayList();
            this.cast_seen = java.lang.Float.NEGATIVE_INFINITY;
            this.own_cast_seen = java.lang.Float.NEGATIVE_INFINITY;
            this.siege_wait_since = (float)(-1.0f);
            this.raid_state = com.oddlabs.tt.player.ultra.MilitaryManager.RaidState.NONE;
        }
        public MilitaryManager(com.oddlabs.tt.player.ultra.Context ctx) : base()
        {
            init_MilitaryManager();
            this.ctx = ctx;
            this.chief_target_value = ctx.strategy.f("chiefTargetValue");
            com.oddlabs.tt.player.ultra.Strategy s = ctx.strategy;
            attack_min = s.i("attackMin");
            commit_ratio = s.f("commitRatio");
            weapon_buffer = s.i("weaponBuffer");
            enemy_latent_guess = s.f("enemyLatentGuess");
            attack_learn = s.b("attackLearn");
            stun_tower_focus = s.b("stunTowerFocus");
            tower_linear = s.f("towerLinear");
            cover_weight = s.f("coverWeight");
            stun_siege = s.b("stunSiege");
            learn_decay = s.f("learnDecay");
            siege_evac = s.b("siegeEvac");
            siege_hp = s.f("siegeHp");
            march_watch = s.b("marchWatch");
            dodge = s.b("dodge");
            assault_gate = s.b("assaultGate");
            frozen_window = s.b("frozenWindow");
            toot_grace = s.f("tootGrace");
            frozen_min = s.i("frozenMin");
            frozen_reengage = s.f("frozenReengage");
            assault_ratio = s.f("assaultRatio");
            tower_lin_w = s.f("towerLinW");
            assault_r = s.i("assaultR");
            assault_chief = s.f("assaultChief");
            assault_memory = s.f("assaultMemory");
            approach_towers = s.b("approachTowers");
            dodge_min = s.f("dodgeMin");
            dodge_max = s.f("dodgeMax");
            dodge_to = s.f("dodgeTo");
            dodge_hold = s.f("dodgeHold");
            dodge_peons = s.b("dodgePeons");
            dodge_smart = s.b("dodgeSmart");
            enemy_chief_factor = s.f("enemyChiefFactor");
            grind_ratio = s.f("grindRatio");
            retreat_ratio = s.f("retreatRatio");
            defend_radius = s.i("defendRadius");
            reinforce_batch = s.i("reinforceBatch");
            engage_range = s.i("engageRange");
            cohesion_frac = s.f("cohesionFrac");
            leg_length = s.i("legLength");
            cap_attack_frac = s.f("capAttackFrac");
            micro = s.b("micro");
            raid_enabled = s.b("raid");
            recall = s.b("recall");
            peon_strike = s.b("peonStrike");
            raid_size = s.i("raidSize");
            gather = new int[] { ctx.map.home_x, ctx.map.home_y };
            waypoint = gather;
        }
    }
}
