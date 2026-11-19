// Translated from Freeze.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.gauntlet
{
    // Java: 49
    public class Freeze : java.lang.Object
    {
        // Java: 50
        public sealed class Phase : java.lang.Enum
        {
            public static Phase[] values() { return (Phase[])VALUES_.Clone(); }
            public static Phase valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly Phase WALK = new Phase("WALK", 0);
            public static readonly Phase STRIKE = new Phase("STRIKE", 1);
            public static readonly Phase STAGE = new Phase("STAGE", 2);
            public static readonly Phase WAIT = new Phase("WAIT", 3);
            public static readonly Phase CUT = new Phase("CUT", 4);
            public static readonly Phase RAZE = new Phase("RAZE", 5);
            public static readonly Phase DONE = new Phase("DONE", 6);
            public static readonly Phase[] VALUES_ = new Phase[] { WALK, STRIKE, STAGE, WAIT, CUT, RAZE, DONE };
            public Phase(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        public const float PEON_SPEED = 5.0f;
        public const int STAGE_CELLS = 19;
        public const int ARRIVE_CELLS = 12;
        public const int MIN_SQUAD = 3;
        public const float WAIT_LIMIT = 150.0f;
        public const float STRIKE_LIMIT = 100.0f;
        public const float SITE_GONE_LIMIT = 60.0f;
        public const int FIGHT_CELLS = 6;
        public const int DEFENSE_CELLS = 16;
        public com.oddlabs.tt.player.gauntlet.GauntletAI ai;
        public java.util.List strikes;
        public java.util.List frozen;
        public java.util.List frozen_sites;
        // Java: 98
        public bool isFrozen(com.oddlabs.tt.player.Player p)
        {
            return !frozen.isEmpty() && frozen.contains(p);
        }
        // Java: 103
        public bool isFrozenSite(com.oddlabs.tt.model.Building b)
        {
            return !frozen.isEmpty() && !b.isDead() && !b.isComplete() && ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID() == com.oddlabs.tt.model.Race.BUILDING_ARMORY && frozen.contains(b.getOwner());
        }
        // Java: 113
        public bool plan(com.oddlabs.tt.player.gauntlet.DistanceField start_field)
        {
            com.oddlabs.tt.player.gauntlet.Strategy strategy = ai.strategy();
            if (!strategy.freeze_open || strategy.freeze_squad <= 0)
            {
                return false;
            }
            com.oddlabs.tt.player.Player me = ai.owner();
            java.util.List copies = new java.util.ArrayList();
            java.util.List etas = new java.util.ArrayList();
            foreach (com.oddlabs.tt.player.Player p in me.getWorld().getPlayers())
            {
                if (!me.isEnemy(p) || !p.isAlive())
                {
                    continue;
                }
                int walk = start_field.getAround(com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(p.getStartX()), com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(p.getStartY()), 3);
                if (walk == com.oddlabs.tt.player.gauntlet.DistanceField.UNREACHABLE)
                {
                    continue;
                }
                float eta = (float)(walk / com.oddlabs.tt.player.gauntlet.Freeze.PEON_SPEED);
                int at = 0;
                while (at < etas.size() && ((float)((float?)etas.get(at))) <= eta)
                {
                    at++;
                }
                copies.add(at, p);
                etas.add(at, eta);
            }
            if (copies.isEmpty() || ((float)((float?)etas.getFirst())) > strategy.freeze_eta)
            {
                string nearest = (copies.isEmpty() ? "none reachable" : java.lang.JStr.valueOf(com.oddlabs.tt.player.gauntlet.Freeze.name(((com.oddlabs.tt.player.Player)copies.getFirst()))) + " at " + (java.lang.JC.f2i((((float)((float?)etas.getFirst()))))) + "s");
                log(new java.util.function.Supplier_L(() =>
                {
                    return "no strike: nearest copy " + java.lang.JStr.valueOf(nearest) + ", freeze_eta " + (java.lang.JC.f2i(strategy.freeze_eta)) + "s";
                }));
                return false;
            }
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            for (int i = 0; i < copies.size() && strikes.size() < java.lang.Math.max(1, strategy.freeze_targets); i++)
            {
                bool first = strikes.isEmpty();
                float limit = (first || strategy.freeze_eta2 <= 0.0f ? strategy.freeze_eta : strategy.freeze_eta2);
                if (((float)((float?)etas.get(i))) > limit)
                {
                    break;
                }
                java.util.List pool = new java.util.ArrayList(intel.peons);
                pool.removeAll(intel.strikers);
                int wanted = (first || strategy.freeze_squad2 <= 0 ? strategy.freeze_squad : strategy.freeze_squad2);
                int size = java.lang.Math.min(wanted, pool.size() - java.lang.Math.max(1, strategy.freeze_keep));
                if (size < com.oddlabs.tt.player.gauntlet.Freeze.MIN_SQUAD)
                {
                    int left = pool.size();
                    log(new java.util.function.Supplier_L(() =>
                    {
                        return "no " + java.lang.JStr.valueOf(((first ? "" : "further "))) + "strike: only " + (left) + " peons";
                    }));
                    break;
                }
                com.oddlabs.tt.player.Player best = ((com.oddlabs.tt.player.Player)copies.get(i));
                int sx = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(best.getStartX());
                int sy = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(best.getStartY());
                pool.sort(java.util.Comparator_.comparingInt(new java.util.function.ToIntFunction_L((object u_l) =>
                {
                    com.oddlabs.tt.model.Unit u = (com.oddlabs.tt.model.Unit)u_l;
                    return com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(u.getGridX(), u.getGridY(), sx, sy);
                })));
                com.oddlabs.tt.player.gauntlet.Freeze.Strike strike = new com.oddlabs.tt.player.gauntlet.Freeze.Strike(this, best, pool.subList(0, size));
                strikes.add(strike);
                intel.strikers.addAll(strike.squad);
                strike.setPhase(com.oddlabs.tt.player.gauntlet.Freeze.Phase.WALK);
                ai.aiLog().count((first ? "freeze_start" : "freeze_start_more"));
                float eta_2 = ((float)((float?)etas.get(i)));
                log(new java.util.function.Supplier_L(() =>
                {
                    return java.lang.JStr.valueOf(((first ? "" : "further "))) + "strike on " + java.lang.JStr.valueOf(com.oddlabs.tt.player.gauntlet.Freeze.name(best)) + " at " + (sx) + "," + (sy) + " with " + (strike.squad.size()) + " peons, eta " + (java.lang.JC.f2i(eta_2)) + "s";
                }));
                strike.walk(best, ai.time());
            }
            return !strikes.isEmpty();
        }
        // Java: 174
        public void tick()
        {
            if (ai.strategy().freeze_unfreeze && !frozen.isEmpty())
            {
                unfreeze();
            }
            foreach (com.oddlabs.tt.player.gauntlet.Freeze.Strike strike in java.lang.JIter.of(strikes))
            {
                strike.tick();
            }
        }
        // Java: 182
        public bool struck(com.oddlabs.tt.player.Player p, com.oddlabs.tt.player.gauntlet.Freeze.Strike self)
        {
            foreach (com.oddlabs.tt.player.gauntlet.Freeze.Strike other in java.lang.JIter.of(strikes))
            {
                if (((object)other != (object)self) && other.active() && ((object)other.target == (object)p))
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 190
        public class Strike : java.lang.Object
        {
            public com.oddlabs.tt.player.gauntlet.Freeze this_0;
            public java.util.List squad;
            public java.util.List seen;
            public bool a_out;
            public bool retargeted;
            public com.oddlabs.tt.player.Player target;
            public int start_x;
            public int start_y;
            public com.oddlabs.tt.player.gauntlet.Freeze.Phase phase;
            public float phase_time;
            public float last_order;
            public int stage_x;
            public int stage_y;
            public bool out_counted;
            // Java: 215
            public bool active()
            {
                return ((object)phase != (object)com.oddlabs.tt.player.gauntlet.Freeze.Phase.DONE) && target != null;
            }
            // Java: 219
            public void tick()
            {
                if (!active())
                {
                    return;
                }
                com.oddlabs.tt.player.Player t = target;
                float now = this_0.ai.time();
                countLosses();
                countKills(t);
                if (!t.isAlive())
                {
                    if (!out_counted)
                    {
                        out_counted = true;
                        this_0.ai.aiLog().count("freeze_target_out");
                    }
                    this_0.log(new java.util.function.Supplier_L(() =>
                    {
                        return java.lang.JStr.valueOf(com.oddlabs.tt.player.gauntlet.Freeze.name(t)) + " is out at " + (java.lang.JC.f2i(now)) + "s (" + java.lang.JStr.valueOf(phase) + ", " + (squad.size()) + " peons left)";
                    }));
                    if (((object)phase == (object)com.oddlabs.tt.player.gauntlet.Freeze.Phase.WALK) || ((object)phase == (object)com.oddlabs.tt.player.gauntlet.Freeze.Phase.STRIKE))
                    {
                        a_out = true;
                        if (this_0.ai.strategy().freeze_retarget && !retargeted && squad.size() >= com.oddlabs.tt.player.gauntlet.Freeze.MIN_SQUAD && retarget(now))
                        {
                            return;
                        }
                    }
                    goHome();
                    return;
                }
                if (squad.size() < com.oddlabs.tt.player.gauntlet.Freeze.MIN_SQUAD)
                {
                    abort("squad down to " + (squad.size()) + " peons");
                    return;
                }
                switch ((phase).ordinal())
                {
                    case 0 /*WALK*/:
                    {
                        walk(t, now);
                        break;
                    }
                    case 1 /*STRIKE*/:
                    {
                        strike(t, now);
                        break;
                    }
                    case 2 /*STAGE*/:
                    {
                        stage(t, now);
                        break;
                    }
                    case 3 /*WAIT*/:
                    {
                        await(t, now);
                        break;
                    }
                    case 4 /*CUT*/:
                    {
                        cut(t, now);
                        break;
                    }
                    case 5 /*RAZE*/:
                    {
                        raze(t, now);
                        break;
                    }
                    default:
                    {
                        break;
                    }
                }
            }
            // Java: 260
            public void walk(com.oddlabs.tt.player.Player t, float now)
            {
                if (quartersStood(t, "its quarters stood before the squad arrived"))
                {
                    return;
                }
                if ((float)(now - phase_time) > (float)(this_0.ai.strategy().freeze_eta + com.oddlabs.tt.player.gauntlet.Freeze.STRIKE_LIMIT))
                {
                    abort("the squad never reached its peons");
                    return;
                }
                java.util.List prey = com.oddlabs.tt.player.gauntlet.Freeze.outsideUnits(t);
                int[] goal = (prey.isEmpty() ? new int[] { start_x, start_y } : com.oddlabs.tt.player.gauntlet.MapAnalysis.centroid(prey));
                int[] c = com.oddlabs.tt.player.gauntlet.MapAnalysis.centroid(squad);
                if (!prey.isEmpty() && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(c[0], c[1], goal[0], goal[1]) <= com.oddlabs.tt.player.gauntlet.Freeze.ARRIVE_CELLS * com.oddlabs.tt.player.gauntlet.Freeze.ARRIVE_CELLS)
                {
                    setPhase(com.oddlabs.tt.player.gauntlet.Freeze.Phase.STRIKE);
                    this_0.ai.aiLog().count("freeze_path_a");
                    this_0.log(new java.util.function.Supplier_L(() =>
                    {
                        return "squad at " + java.lang.JStr.valueOf(com.oddlabs.tt.player.gauntlet.Freeze.name(t)) + " at " + (java.lang.JC.f2i(now)) + "s: striking " + (prey.size()) + " peons";
                    }));
                    strike(t, now);
                    return;
                }
                moveSquad(now, goal[0], goal[1]);
            }
            // Java: 280
            public void strike(com.oddlabs.tt.player.Player t, float now)
            {
                if (quartersStood(t, "its quarters stood during the strike"))
                {
                    return;
                }
                if ((float)(now - phase_time) > com.oddlabs.tt.player.gauntlet.Freeze.STRIKE_LIMIT)
                {
                    abort("strike took over " + (java.lang.JC.f2i(com.oddlabs.tt.player.gauntlet.Freeze.STRIKE_LIMIT)) + "s");
                    return;
                }
                java.util.List prey = com.oddlabs.tt.player.gauntlet.Freeze.outsideUnits(t);
                if (!prey.isEmpty())
                {
                    assign(prey);
                }
            }
            // Java: 298
            public bool retarget(float now)
            {
                com.oddlabs.tt.player.Player done = target;
                retargeted = true;
                com.oddlabs.tt.player.Player me = this_0.ai.owner();
                float limit = this_0.ai.strategy().freeze_eta;
                int[] c = com.oddlabs.tt.player.gauntlet.MapAnalysis.centroid(squad);
                com.oddlabs.tt.player.gauntlet.DistanceField field = this_0.ai.map().computeField(c[0], c[1], java.lang.JC.d2i(java.lang.Math.ceil((float)(limit * com.oddlabs.tt.player.gauntlet.Freeze.PEON_SPEED))) + 10);
                com.oddlabs.tt.player.Player best = null;
                float best_eta = java.lang.Float.MAX_VALUE;
                com.oddlabs.tt.player.Player nearest = null;
                float nearest_eta = java.lang.Float.MAX_VALUE;
                foreach (com.oddlabs.tt.player.Player p in me.getWorld().getPlayers())
                {
                    if (((object)p == (object)done) || !me.isEnemy(p) || !p.isAlive() || this_0.struck(p, this))
                    {
                        continue;
                    }
                    int walk_v = field.getAround(com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(p.getStartX()), com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(p.getStartY()), 3);
                    if (walk_v == com.oddlabs.tt.player.gauntlet.DistanceField.UNREACHABLE)
                    {
                        continue;
                    }
                    float eta = (float)(walk_v / com.oddlabs.tt.player.gauntlet.Freeze.PEON_SPEED);
                    if (eta < nearest_eta)
                    {
                        nearest_eta = eta;
                        nearest = p;
                    }
                    if (eta < best_eta && !com.oddlabs.tt.player.gauntlet.Freeze.quartersStands(p))
                    {
                        best_eta = eta;
                        best = p;
                    }
                }
                if (best == null || best_eta > limit)
                {
                    if (nearest != null && nearest_eta <= limit)
                    {
                        this_0.ai.aiLog().count("freeze_retarget_quartered");
                    }
                    com.oddlabs.tt.player.Player n = nearest;
                    float n_eta = nearest_eta;
                    this_0.log(new java.util.function.Supplier_L(() =>
                    {
                        return "no second strike: nearest living copy " + java.lang.JStr.valueOf(((n == null ? "none within reach" : java.lang.JStr.valueOf(com.oddlabs.tt.player.gauntlet.Freeze.name(n)) + " at " + (java.lang.JC.f2i(n_eta)) + "s" + java.lang.JStr.valueOf(((com.oddlabs.tt.player.gauntlet.Freeze.quartersStands(n) ? ", its quarters stands" : "")))))) + ", freeze_eta " + (java.lang.JC.f2i(limit)) + "s";
                    }));
                    return false;
                }
                target = best;
                start_x = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(best.getStartX());
                start_y = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(best.getStartY());
                out_counted = false;
                seen.clear();
                setPhase(com.oddlabs.tt.player.gauntlet.Freeze.Phase.WALK);
                this_0.ai.aiLog().count("freeze_retarget");
                float eta_2 = best_eta;
                com.oddlabs.tt.player.Player t = best;
                this_0.log(new java.util.function.Supplier_L(() =>
                {
                    return "second strike on " + java.lang.JStr.valueOf(com.oddlabs.tt.player.gauntlet.Freeze.name(t)) + " at " + (start_x) + "," + (start_y) + " with " + (squad.size()) + " peons, eta " + (java.lang.JC.f2i(eta_2)) + "s";
                }));
                walk(best, now);
                return true;
            }
            // Java: 358
            public bool quartersStood(com.oddlabs.tt.player.Player t, string why)
            {
                if (!com.oddlabs.tt.player.gauntlet.Freeze.quartersStands(t))
                {
                    return false;
                }
                if (retargeted)
                {
                    abort(why);
                }
                else
                {
                    fallback(t, why);
                }
                return true;
            }
            // Java: 371
            public void fallback(com.oddlabs.tt.player.Player t, string why)
            {
                setPhase(com.oddlabs.tt.player.gauntlet.Freeze.Phase.STAGE);
                this_0.ai.aiLog().count("freeze_fallback_c");
                this_0.log(new java.util.function.Supplier_L(() =>
                {
                    return "falling back to the armory cut on " + java.lang.JStr.valueOf(com.oddlabs.tt.player.gauntlet.Freeze.name(t)) + ": " + java.lang.JStr.valueOf(why);
                }));
                stage(t, this_0.ai.time());
            }
            // Java: 378
            public void stage(com.oddlabs.tt.player.Player t, float now)
            {
                stagePoint(t);
                if (armoryCheck(t))
                {
                    return;
                }
                if ((float)(now - phase_time) > com.oddlabs.tt.player.gauntlet.Freeze.WAIT_LIMIT)
                {
                    abort("the squad never reached its stage point");
                    return;
                }
                int[] c = com.oddlabs.tt.player.gauntlet.MapAnalysis.centroid(squad);
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(c[0], c[1], stage_x, stage_y) <= 7 * 7)
                {
                    setPhase(com.oddlabs.tt.player.gauntlet.Freeze.Phase.WAIT);
                    this_0.log(new java.util.function.Supplier_L(() =>
                    {
                        return "squad staged by " + java.lang.JStr.valueOf(com.oddlabs.tt.player.gauntlet.Freeze.name(t)) + " at " + (stage_x) + "," + (stage_y);
                    }));
                    await(t, now);
                    return;
                }
                moveSquad(now, stage_x, stage_y);
            }
            // Java: 396
            public void await(com.oddlabs.tt.player.Player t, float now)
            {
                if (armoryCheck(t))
                {
                    return;
                }
                if ((float)(now - phase_time) > com.oddlabs.tt.player.gauntlet.Freeze.WAIT_LIMIT)
                {
                    abort("no armory site within " + (java.lang.JC.f2i(com.oddlabs.tt.player.gauntlet.Freeze.WAIT_LIMIT)) + "s");
                    return;
                }
                stagePoint(t);
                if (this_0.ai.strategy().freeze_fight)
                {
                    fight(t);
                    return;
                }
                moveSquad(now, stage_x, stage_y);
            }
            // Java: 418
            public void fight(com.oddlabs.tt.player.Player t)
            {
                java.util.List guards = new java.util.ArrayList();
                foreach (com.oddlabs.tt.model.Selectable sel in java.lang.JIter.of(t.getUnits().getSet()))
                {
                    if ((sel is com.oddlabs.tt.model.Building b) && !b.isDead() && b.isComplete() && (((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID() == com.oddlabs.tt.model.Race.BUILDING_QUARTERS || ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID() == com.oddlabs.tt.model.Race.BUILDING_ARMORY))
                    {
                        guards.add(b);
                    }
                }
                java.util.List prey = new java.util.ArrayList();
                foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(com.oddlabs.tt.player.gauntlet.Freeze.outsideUnits(t)))
                {
                    if (!com.oddlabs.tt.player.gauntlet.Freeze.guarded(guards, u) && nearSquad(u))
                    {
                        prey.add(u);
                    }
                }
                java.util.List fighters = new java.util.ArrayList();
                java.util.List back = new java.util.ArrayList();
                foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(squad))
                {
                    bool walking = (u_2.getPrimaryController() is com.oddlabs.tt.model.behaviour.WalkController w) && !w.isAgressive() && ((object)u_2.getCurrentController() == (object)w);
                    com.oddlabs.tt.model.Selectable hunted = com.oddlabs.tt.player.gauntlet.Freeze.huntTarget(u_2);
                    bool hunting = hunted != null && !hunted.isDead();
                    bool away = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(u_2.getGridX(), u_2.getGridY(), stage_x, stage_y) > 7 * 7;
                    if (com.oddlabs.tt.player.gauntlet.Freeze.guarded(guards, u_2))
                    {
                        if (!walking)
                        {
                            back.add(u_2);
                        }
                    }
                    else if (!prey.isEmpty())
                    {
                        fighters.add(u_2);
                    }
                    else if (!walking && (hunting || away))
                    {
                        back.add(u_2);
                    }
                }
                if (!fighters.isEmpty())
                {
                    this_0.ai.aiLog().count("freeze_fight");
                    this.this_0.assign(fighters, prey);
                }
                if (!back.isEmpty())
                {
                    this_0.ai.aiLog().count("freeze_fight_back");
                    this_0.ai.landscapeOrder(((com.oddlabs.tt.model.Selectable[])back.toArray(new com.oddlabs.tt.model.Selectable[0])), stage_x, stage_y, com.oddlabs.tt.model.Action.MOVE, false);
                }
            }
            // Java: 458
            public bool nearSquad(com.oddlabs.tt.model.Unit u)
            {
                foreach (com.oddlabs.tt.model.Unit s in java.lang.JIter.of(squad))
                {
                    int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(s.getGridX(), s.getGridY(), u.getGridX(), u.getGridY());
                    if (d <= com.oddlabs.tt.player.gauntlet.Freeze.FIGHT_CELLS * com.oddlabs.tt.player.gauntlet.Freeze.FIGHT_CELLS)
                    {
                        return true;
                    }
                }
                return false;
            }
            // Java: 471
            public bool armoryCheck(com.oddlabs.tt.player.Player t)
            {
                com.oddlabs.tt.model.Building armory = com.oddlabs.tt.player.gauntlet.Freeze.building(t, com.oddlabs.tt.model.Race.BUILDING_ARMORY);
                if (armory == null)
                {
                    return false;
                }
                if (armory.isComplete())
                {
                    abort("its armory stands");
                    return true;
                }
                setPhase(com.oddlabs.tt.player.gauntlet.Freeze.Phase.CUT);
                this_0.log(new java.util.function.Supplier_L(() =>
                {
                    return "cut on " + java.lang.JStr.valueOf(com.oddlabs.tt.player.gauntlet.Freeze.name(t)) + ": armory site at " + (armory.getGridX()) + "," + (armory.getGridY()) + ", " + (com.oddlabs.tt.player.gauntlet.Freeze.outsideUnits(t).size()) + " peons outside";
                }));
                cut(t, this_0.ai.time());
                return true;
            }
            // Java: 487
            public void cut(com.oddlabs.tt.player.Player t, float now)
            {
                com.oddlabs.tt.model.Building armory = com.oddlabs.tt.player.gauntlet.Freeze.building(t, com.oddlabs.tt.model.Race.BUILDING_ARMORY);
                if (armory != null && armory.isComplete())
                {
                    abort("its armory stood during the cut");
                    return;
                }
                java.util.List prey = com.oddlabs.tt.player.gauntlet.Freeze.outsideUnits(t);
                if (prey.isEmpty())
                {
                    if (armory == null)
                    {
                        if ((float)(now - phase_time) > com.oddlabs.tt.player.gauntlet.Freeze.SITE_GONE_LIMIT)
                        {
                            abort("no armory site to cut");
                        }
                        return;
                    }
                    if (!this_0.frozen.contains(t))
                    {
                        this_0.frozen.add(t);
                        this_0.frozen_sites.add(armory);
                        this_0.ai.aiLog().count("freeze_frozen");
                        this_0.log(new java.util.function.Supplier_L(() =>
                        {
                            return "froze " + java.lang.JStr.valueOf(com.oddlabs.tt.player.gauntlet.Freeze.name(t)) + " at " + (java.lang.JC.f2i(now)) + "s, armory site at " + (armory.getGridX()) + "," + (armory.getGridY());
                        }));
                    }
                    if (this_0.ai.strategy().freeze_raze && com.oddlabs.tt.player.gauntlet.Freeze.building(t, com.oddlabs.tt.model.Race.BUILDING_QUARTERS) != null)
                    {
                        setPhase(com.oddlabs.tt.player.gauntlet.Freeze.Phase.RAZE);
                    }
                    else
                    {
                        goHome();
                    }
                    return;
                }
                if ((float)(now - phase_time) > com.oddlabs.tt.player.gauntlet.Freeze.STRIKE_LIMIT)
                {
                    abort("cut took over " + (java.lang.JC.f2i(com.oddlabs.tt.player.gauntlet.Freeze.STRIKE_LIMIT)) + "s");
                    return;
                }
                assign(prey);
            }
            // Java: 521
            public void raze(com.oddlabs.tt.player.Player t, float now)
            {
                com.oddlabs.tt.model.Building quarters = com.oddlabs.tt.player.gauntlet.Freeze.building(t, com.oddlabs.tt.model.Race.BUILDING_QUARTERS);
                if (quarters == null)
                {
                    abort("no quarters left to raze");
                    return;
                }
                java.util.List prey = com.oddlabs.tt.player.gauntlet.Freeze.outsideUnits(t);
                foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(prey))
                {
                    if (u.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.THROW) || u.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC))
                    {
                        abort("warriors or a chieftain came out");
                        return;
                    }
                }
                if (!prey.isEmpty())
                {
                    assign(prey);
                    return;
                }
                if ((float)(now - last_order) >= 5.0f)
                {
                    last_order = now;
                    this_0.ai.owner().setTarget(((com.oddlabs.tt.model.Selectable[])squad.toArray(new com.oddlabs.tt.model.Selectable[0])), quarters, com.oddlabs.tt.model.Action.ATTACK, false);
                }
            }
            // Java: 545
            public void stagePoint(com.oddlabs.tt.player.Player t)
            {
                com.oddlabs.tt.model.Building q = com.oddlabs.tt.player.gauntlet.Freeze.building(t, com.oddlabs.tt.model.Race.BUILDING_QUARTERS);
                int cx = (q != null ? q.getGridX() : start_x);
                int cy = (q != null ? q.getGridY() : start_y);
                float dx = this_0.ai.planner().getStartX() - cx;
                float dy = this_0.ai.planner().getStartY() - cy;
                float len = ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy)))));
                if (len < 1.0f)
                {
                    stage_x = cx;
                    stage_y = cy;
                    return;
                }
                stage_x = cx + java.lang.Math.round((float)((float)(com.oddlabs.tt.player.gauntlet.Freeze.STAGE_CELLS * dx) / len));
                stage_y = cy + java.lang.Math.round((float)((float)(com.oddlabs.tt.player.gauntlet.Freeze.STAGE_CELLS * dy) / len));
            }
            // Java: 565
            public void moveSquad(float now, int x, int y)
            {
                if ((float)(now - last_order) < 6.0f)
                {
                    return;
                }
                last_order = now;
                this_0.ai.landscapeOrder(((com.oddlabs.tt.model.Selectable[])squad.toArray(new com.oddlabs.tt.model.Selectable[0])), x, y, com.oddlabs.tt.model.Action.MOVE, false);
            }
            // Java: 573
            public void assign(java.util.List prey)
            {
                this.this_0.assign(squad, prey);
            }
            // Java: 577
            public void countLosses()
            {
                for (int i = squad.size() - 1; i >= 0; i--)
                {
                    com.oddlabs.tt.model.Unit u = ((com.oddlabs.tt.model.Unit)squad.get(i));
                    if (u.isDead())
                    {
                        squad.remove(i);
                        this_0.ai.intel().strikers.remove(u);
                        this_0.ai.aiLog().count("freeze_lost");
                    }
                }
            }
            // Java: 592
            public void countKills(com.oddlabs.tt.player.Player t)
            {
                foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(seen))
                {
                    if (u.isDead())
                    {
                        this_0.ai.aiLog().count("freeze_kills");
                    }
                }
                seen.clear();
                seen.addAll(com.oddlabs.tt.player.gauntlet.Freeze.outsideUnits(t));
            }
            // Java: 600
            public void abort(string why)
            {
                com.oddlabs.tt.player.Player t = target;
                this_0.ai.aiLog().count("freeze_abort");
                this_0.log(new java.util.function.Supplier_L(() =>
                {
                    return "strike on " + java.lang.JStr.valueOf(((t != null ? com.oddlabs.tt.player.gauntlet.Freeze.name(t) : "?"))) + " given up in " + java.lang.JStr.valueOf(phase) + ": " + java.lang.JStr.valueOf(why) + " (" + (squad.size()) + " peons left)";
                }));
                goHome();
            }
            // Java: 609
            public void goHome()
            {
                com.oddlabs.tt.player.gauntlet.Intel intel = this_0.ai.intel();
                foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(squad))
                {
                    intel.strikers.remove(u);
                }
                setPhase(com.oddlabs.tt.player.gauntlet.Freeze.Phase.DONE);
                seen.clear();
                com.oddlabs.tt.model.Building push = (a_out && this_0.ai.strategy().freeze_armory_push ? this_0.ai.economy().pushArmory() : (com.oddlabs.tt.model.Building)(null));
                if (squad.isEmpty())
                {
                    return;
                }
                com.oddlabs.tt.model.Selectable[] units = ((com.oddlabs.tt.model.Selectable[])squad.toArray(new com.oddlabs.tt.model.Selectable[0]));
                if (push != null)
                {
                    this_0.ai.owner().setTarget(units, push, com.oddlabs.tt.model.Action.DEFAULT, false);
                    for (int i = 0; i < units.Length; i++)
                    {
                        this_0.ai.aiLog().count("freeze_push_builders");
                    }
                    this_0.log(new java.util.function.Supplier_L(() =>
                    {
                        return (units.Length) + " peons walk to build our armory at " + (push.getGridX()) + "," + (push.getGridY());
                    }));
                    squad.clear();
                    return;
                }
                int[] c = com.oddlabs.tt.player.gauntlet.MapAnalysis.centroid(squad);
                java.util.List homes = intel.homes();
                com.oddlabs.tt.model.Building home = com.oddlabs.tt.player.gauntlet.MapAnalysis.nearest(homes, c[0], c[1]);
                if (home == null)
                {
                    home = com.oddlabs.tt.player.gauntlet.MapAnalysis.nearest(intel.quarters_sites, c[0], c[1]);
                }
                if (home != null)
                {
                    this_0.ai.owner().setTarget(units, home, com.oddlabs.tt.model.Action.DEFAULT, false);
                }
                else
                {
                    this_0.ai.landscapeOrder(units, this_0.ai.planner().getStartX(), this_0.ai.planner().getStartY(), com.oddlabs.tt.model.Action.MOVE, false);
                }
                com.oddlabs.tt.model.Building h = home;
                this_0.log(new java.util.function.Supplier_L(() =>
                {
                    return (squad.size()) + " peons walk home" + java.lang.JStr.valueOf(((h != null ? " to " + (h.getGridX()) + "," + (h.getGridY()) : "")));
                }));
                squad.clear();
            }
            // Java: 642
            public void setPhase(com.oddlabs.tt.player.gauntlet.Freeze.Phase next)
            {
                phase = next;
                phase_time = this_0.ai.time();
                last_order = (float)(-100.0f);
            }
            private void init_Strike()
            {
                this.squad = new java.util.ArrayList();
                this.seen = new java.util.ArrayList();
                this.phase = com.oddlabs.tt.player.gauntlet.Freeze.Phase.DONE;
                this.last_order = (float)(-100.0f);
            }
            public Strike(com.oddlabs.tt.player.gauntlet.Freeze this_0_, com.oddlabs.tt.player.Player target, java.util.List squad) : base()
            {
                this.this_0 = this_0_;
                init_Strike();
                this.target = target;
                this.squad.addAll(squad);
                start_x = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(target.getStartX());
                start_y = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(target.getStartY());
            }
        }
        // Java: 650
        public static bool guarded(java.util.List guards, com.oddlabs.tt.model.Unit u)
        {
            foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(guards))
            {
                int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(b.getGridX(), b.getGridY(), u.getGridX(), u.getGridY());
                if (d <= com.oddlabs.tt.player.gauntlet.Freeze.DEFENSE_CELLS * com.oddlabs.tt.player.gauntlet.Freeze.DEFENSE_CELLS)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 660
        public void assign(java.util.List peons, java.util.List prey)
        {
            java.util.List busy = new java.util.ArrayList();
            java.util.List targets = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(peons))
            {
                com.oddlabs.tt.model.Selectable t = com.oddlabs.tt.player.gauntlet.Freeze.huntTarget(u);
                if ((t is com.oddlabs.tt.model.Unit tu) && !tu.isDead() && prey.contains(tu))
                {
                    busy.add(u);
                    targets.add(tu);
                }
            }
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(peons))
            {
                if (busy.contains(u_2))
                {
                    continue;
                }
                com.oddlabs.tt.model.Unit best = null;
                int best_d = java.lang.Integer.MAX_VALUE;
                foreach (com.oddlabs.tt.model.Unit p in java.lang.JIter.of(prey))
                {
                    int hunters = 0;
                    foreach (com.oddlabs.tt.model.Unit t_2 in java.lang.JIter.of(targets))
                    {
                        if (((object)t_2 == (object)p))
                        {
                            hunters++;
                        }
                    }
                    if (hunters >= 3)
                    {
                        continue;
                    }
                    int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(u_2.getGridX(), u_2.getGridY(), p.getGridX(), p.getGridY());
                    if (d < best_d)
                    {
                        best_d = d;
                        best = p;
                    }
                }
                if (best == null)
                {
                    best = ((com.oddlabs.tt.model.Unit)prey.getFirst());
                }
                ai.owner().setTarget(com.oddlabs.tt.model.Selectable.newArray(u_2), best, com.oddlabs.tt.model.Action.ATTACK, false);
                targets.add(best);
            }
        }
        // Java: 696
        public static com.oddlabs.tt.model.Selectable huntTarget(com.oddlabs.tt.model.Unit u)
        {
            com.oddlabs.tt.model.behaviour.Controller c = u.getCurrentController();
            return ((c is com.oddlabs.tt.model.behaviour.HuntController h) ? h.getTarget() : ((c is com.oddlabs.tt.model.behaviour.AttackController a) ? a.getTarget() : (com.oddlabs.tt.model.Selectable)(null)));
        }
        // Java: 706
        public void unfreeze()
        {
            for (int i = frozen.size() - 1; i >= 0; i--)
            {
                com.oddlabs.tt.player.Player p = ((com.oddlabs.tt.player.Player)frozen.get(i));
                com.oddlabs.tt.model.Building site = ((com.oddlabs.tt.model.Building)frozen_sites.get(i));
                com.oddlabs.tt.model.Building armory = com.oddlabs.tt.player.gauntlet.Freeze.building(p, com.oddlabs.tt.model.Race.BUILDING_ARMORY);
                bool armed = armory != null && armory.isComplete();
                if (p.isAlive() && !site.isDead() && !site.isComplete() && !armed)
                {
                    continue;
                }
                frozen.remove(i);
                frozen_sites.remove(i);
                if (!p.isAlive())
                {
                    continue;
                }
                ai.aiLog().count("freeze_unfrozen");
                float now = ai.time();
                string why = (armed ? "its armory stands" : "its frozen armory site is gone");
                log(new java.util.function.Supplier_L(() =>
                {
                    return java.lang.JStr.valueOf(com.oddlabs.tt.player.gauntlet.Freeze.name(p)) + " unfrozen at " + (java.lang.JC.f2i(now)) + "s: " + java.lang.JStr.valueOf(why);
                }));
            }
        }
        // Java: 725
        public static bool quartersStands(com.oddlabs.tt.player.Player p)
        {
            com.oddlabs.tt.model.Building q = com.oddlabs.tt.player.gauntlet.Freeze.building(p, com.oddlabs.tt.model.Race.BUILDING_QUARTERS);
            return q != null && q.isComplete();
        }
        // Java: 731
        public static com.oddlabs.tt.model.Building building(com.oddlabs.tt.player.Player p, int type)
        {
            com.oddlabs.tt.model.Building site = null;
            foreach (com.oddlabs.tt.model.Selectable sel in java.lang.JIter.of(p.getUnits().getSet()))
            {
                if ((sel is com.oddlabs.tt.model.Building b) && !b.isDead() && ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID() == type)
                {
                    if (b.isComplete())
                    {
                        return b;
                    }
                    if (site == null)
                    {
                        site = b;
                    }
                }
            }
            return site;
        }
        // Java: 744
        public static java.util.List outsideUnits(com.oddlabs.tt.player.Player p)
        {
            java.util.List units = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Selectable sel in java.lang.JIter.of(p.getUnits().getSet()))
            {
                if ((sel is com.oddlabs.tt.model.Unit u) && !u.isDead() && !u.isMounted())
                {
                    units.add(u);
                }
            }
            return units;
        }
        // Java: 752
        public static string name(com.oddlabs.tt.player.Player p)
        {
            return p.getPlayerInfo().getName();
        }
        // Java: 756
        public void log(java.util.function.Supplier message)
        {
            ai.aiLog().log("FREEZE", message);
        }
        private void init_Freeze()
        {
            this.strikes = new java.util.ArrayList();
            this.frozen = new java.util.ArrayList();
            this.frozen_sites = new java.util.ArrayList();
        }
        public Freeze(com.oddlabs.tt.player.gauntlet.GauntletAI ai) : base()
        {
            init_Freeze();
            this.ai = ai;
        }
    }
}
