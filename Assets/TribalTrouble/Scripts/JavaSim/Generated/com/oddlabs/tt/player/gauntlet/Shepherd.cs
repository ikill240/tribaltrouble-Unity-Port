// Translated from Shepherd.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.gauntlet
{
    // Java: 38
    public class Shepherd : java.lang.Object
    {
        public const int CLEAR_CELLS = 12;
        public const int DEFENSE_CELLS = 17;
        public const int TOWER_CELLS = 19;
        public const int STICKY_CELLS = 4;
        public const int SAFE_TRIES = 12;
        public const int HOME_CELLS = 40;
        public const float FLEE_HOLD = 3.0f;
        public com.oddlabs.tt.player.gauntlet.GauntletAI ai;
        public java.util.List flocks;
        public float last_tick;
        public int threat_coming;
        public int threat_peons;
        public int threat_hunters;
        public int threat_warriors;
        public int[] cand_x;
        public int[] cand_y;
        public float[] cand_score;
        // Java: 66
        public class Flock : java.lang.Object
        {
            public com.oddlabs.tt.player.Player copy;
            public com.oddlabs.tt.model.Unit shepherd;
            public int spot_x;
            public int spot_y;
            public com.oddlabs.tt.model.Unit leader;
            public float last_order;
            public float nospot_since;
            public float blocked_since;
            public float flee_until;
            public string last_state;
            public float lost_at;
            public float recruited;
            public int last_x;
            public int last_y;
            public int launches;
            public bool imminent;
            public float armory_at;
            public int base_waves;
            public bool arrived;
            public int rec_x;
            public int rec_y;
            public int rec_spot_x;
            public int rec_spot_y;
            public int prev_spot_x;
            public int prev_spot_y;
            public string origin;
            public int lead_x;
            public int lead_y;
            public com.oddlabs.tt.model.Unit prev_wave;
            public bool home;
            public com.oddlabs.tt.player.gauntlet.Shepherd.Flock partner;
            private void init_Flock()
            {
                this.spot_x = (-1);
                this.spot_y = (-1);
                this.last_order = (float)(-100.0f);
                this.nospot_since = (float)(-1.0f);
                this.blocked_since = (float)(-1.0f);
                this.flee_until = (float)(-1.0f);
                this.last_state = "walk";
                this.lost_at = (float)(-1000.0f);
                this.armory_at = (float)(-1.0f);
                this.prev_spot_x = (-1);
                this.origin = "";
            }
            public Flock(com.oddlabs.tt.player.Player copy, bool home) : base()
            {
                init_Flock();
                this.copy = copy;
                this.home = home;
            }
        }
        // Java: 130
        public void tick()
        {
            com.oddlabs.tt.player.gauntlet.Strategy strategy = ai.strategy();
            if (!strategy.shepherd || (float)(ai.time() - last_tick) < 0.5f)
            {
                return;
            }
            last_tick = ai.time();
            float from = (strategy.shepherd_lead > 0.0f ? java.lang.Math.min(90.0f, strategy.shepherd_time) : strategy.shepherd_time);
            if (ai.time() < from || ai.time() > strategy.shepherd_until)
            {
                releaseAll();
                return;
            }
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            java.util.List fresh = new java.util.ArrayList();
            foreach (com.oddlabs.tt.player.Player p in ai.owner().getWorld().getPlayers())
            {
                if (ai.owner().isEnemy(p) && p.isAlive() && flockOf(p) == null)
                {
                    fresh.add(p);
                }
            }
            if (strategy.shepherd_lead > 0.0f && fresh.size() > 1)
            {
                int sx = ai.planner().getStartX();
                int sy = ai.planner().getStartY();
                fresh.sort(java.util.Comparator_.comparingInt(new java.util.function.ToIntFunction_L((object p_2_l) =>
                {
                    com.oddlabs.tt.player.Player p_2 = (com.oddlabs.tt.player.Player)p_2_l;
                    return -com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(sx, sy, com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(p_2.getStartX()), com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(p_2.getStartY()));
                })));
            }
            foreach (com.oddlabs.tt.player.Player p_3 in java.lang.JIter.of(fresh))
            {
                flocks.add(new com.oddlabs.tt.player.gauntlet.Shepherd.Flock(p_3, false));
            }
            int pair = strategy.shepherd_home_pair;
            if (pair > 0)
            {
                foreach (com.oddlabs.tt.player.Player p_4 in java.lang.JIter.of(fresh))
                {
                    int d2 = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(ai.planner().getStartX(), ai.planner().getStartY(), com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(p_4.getStartX()), com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(p_4.getStartY()));
                    com.oddlabs.tt.player.gauntlet.Shepherd.Flock main = flockOf(p_4);
                    if (d2 >= pair * pair && main != null)
                    {
                        com.oddlabs.tt.player.gauntlet.Shepherd.Flock h = new com.oddlabs.tt.player.gauntlet.Shepherd.Flock(p_4, true);
                        h.partner = main;
                        main.partner = h;
                        flocks.add(h);
                    }
                }
            }
            foreach (com.oddlabs.tt.player.gauntlet.Shepherd.Flock f in java.lang.JIter.of(flocks))
            {
                if (f.shepherd != null && f.shepherd.isDead())
                {
                    ai.aiLog().count("shepherd_lost");
                    ai.aiLog().count("shepherd_lost_" + java.lang.JStr.valueOf(f.last_state));
                    f.lost_at = ai.time();
                    if (ai.logging())
                    {
                        ai.log("shepherd of " + java.lang.JStr.valueOf(com.oddlabs.tt.player.gauntlet.Shepherd.name(f)) + " lost at " + (f.last_x) + "," + (f.last_y) + " (spot " + (f.spot_x) + "," + (f.spot_y) + ", nearest enemy warrior " + (com.oddlabs.tt.player.gauntlet.Shepherd.nearestEnemy(ai.intel().enemy_warriors, f.last_x, f.last_y)) + " cells, peon " + (com.oddlabs.tt.player.gauntlet.Shepherd.nearestEnemy(ai.intel().enemy_peons, f.last_x, f.last_y)) + ", tower " + (nearestTower(f.last_x, f.last_y)) + ", recruited " + (java.lang.JC.f2i(((float)(ai.time() - f.recruited)))) + " s ago, " + java.lang.JStr.valueOf(f.last_state) + ", last flee " + java.lang.JStr.valueOf(((f.flee_until < 0.0f ? "never" : (java.lang.JC.f2i(((float)((float)(ai.time() - f.flee_until) + com.oddlabs.tt.player.gauntlet.Shepherd.FLEE_HOLD)))) + " s ago"))) + ")");
                    }
                    release(f);
                }
                if (!f.copy.isAlive())
                {
                    release(f);
                    continue;
                }
                tend(f, intel);
            }
        }
        // Java: 192
        public void guard()
        {
            if (!ai.strategy().shepherd || flocks.isEmpty())
            {
                return;
            }
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            foreach (com.oddlabs.tt.player.gauntlet.Shepherd.Flock f in java.lang.JIter.of(flocks))
            {
                com.oddlabs.tt.model.Unit s = f.shepherd;
                if (s == null || s.isDead() || s.isMounted())
                {
                    continue;
                }
                int clear = (ai.strategy().shepherd_hold && f.imminent ? 9 : com.oddlabs.tt.player.gauntlet.Shepherd.CLEAR_CELLS);
                int[] away = threatAway(s, intel, clear);
                if (away != null && (float)(ai.time() - f.last_order) >= 0.3f)
                {
                    flee(f, s, away);
                }
            }
        }
        // Java: 208
        public static string name(com.oddlabs.tt.player.gauntlet.Shepherd.Flock f)
        {
            return java.lang.JStr.valueOf(f.copy.getPlayerInfo().getName()) + java.lang.JStr.valueOf(((f.home ? "/home" : "")));
        }
        // Java: 212
        public void flee(com.oddlabs.tt.player.gauntlet.Shepherd.Flock f, com.oddlabs.tt.model.Unit s, int[] away)
        {
            ai.landscapeOrder(com.oddlabs.tt.model.Selectable.newArray(s), away[0], away[1], com.oddlabs.tt.model.Action.MOVE, false);
            f.last_order = ai.time();
            f.flee_until = (float)(ai.time() + com.oddlabs.tt.player.gauntlet.Shepherd.FLEE_HOLD);
        }
        // Java: 218
        public com.oddlabs.tt.player.gauntlet.Shepherd.Flock flockOf(com.oddlabs.tt.player.Player p)
        {
            foreach (com.oddlabs.tt.player.gauntlet.Shepherd.Flock f in java.lang.JIter.of(flocks))
            {
                if (((object)f.copy == (object)p))
                {
                    return f;
                }
            }
            return null;
        }
        // Java: 225
        public void releaseAll()
        {
            foreach (com.oddlabs.tt.player.gauntlet.Shepherd.Flock f in java.lang.JIter.of(flocks))
            {
                release(f);
            }
        }
        // Java: 230
        public void release(com.oddlabs.tt.player.gauntlet.Shepherd.Flock f)
        {
            if (f.shepherd != null)
            {
                ai.intel().shepherds.remove(f.shepherd);
                if (!f.shepherd.isDead())
                {
                    sendHome(f.shepherd);
                }
            }
            f.shepherd = null;
            f.spot_x = (-1);
            f.leader = null;
        }
        // Java: 245
        public bool giveUp(com.oddlabs.tt.model.Unit u)
        {
            foreach (com.oddlabs.tt.player.gauntlet.Shepherd.Flock f in java.lang.JIter.of(flocks))
            {
                if (((object)f.shepherd == (object)u))
                {
                    ai.intel().shepherds.remove(u);
                    f.shepherd = null;
                    f.spot_x = (-1);
                    f.leader = null;
                    f.lost_at = ai.time();
                    ai.aiLog().count("shepherd_given_up");
                    return true;
                }
            }
            return false;
        }
        // Java: 259
        public void sendHome(com.oddlabs.tt.model.Unit u)
        {
            com.oddlabs.tt.model.Building home = ai.intel().armory();
            if (home == null && !ai.intel().quarters.isEmpty())
            {
                home = ((com.oddlabs.tt.model.Building)ai.intel().quarters.getFirst());
            }
            if (home != null && !home.isDead())
            {
                ai.owner().setTarget(com.oddlabs.tt.model.Selectable.newArray(u), home, com.oddlabs.tt.model.Action.DEFAULT, false);
            }
        }
        // Java: 267
        public void tend(com.oddlabs.tt.player.gauntlet.Shepherd.Flock f, com.oddlabs.tt.player.gauntlet.Intel intel)
        {
            com.oddlabs.tt.player.gauntlet.Strategy strategy = ai.strategy();
            if (strategy.shepherd_lead > 0.0f && f.armory_at < 0.0f && com.oddlabs.tt.player.gauntlet.Shepherd.armory(f.copy) != null)
            {
                f.armory_at = ai.time();
            }
            com.oddlabs.tt.model.Unit leader = com.oddlabs.tt.player.gauntlet.Shepherd.oldestIdleWarrior(f.copy);
            int ox = default;
            int oy = default;
            string origin = default;
            if (f.home)
            {
                com.oddlabs.tt.model.Building armory_v = com.oddlabs.tt.player.gauntlet.Shepherd.armory(f.copy);
                if (armory_v == null)
                {
                    return;
                }
                bool at_home = leader != null && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(leader.getGridX(), leader.getGridY(), armory_v.getGridX(), armory_v.getGridY()) <= com.oddlabs.tt.player.gauntlet.Shepherd.HOME_CELLS * com.oddlabs.tt.player.gauntlet.Shepherd.HOME_CELLS;
                ox = (at_home ? leader.getGridX() : armory_v.getGridX());
                oy = (at_home ? leader.getGridY() : armory_v.getGridY());
                origin = (at_home ? "leader" : "armory");
            }
            else if (leader != null)
            {
                ox = leader.getGridX();
                oy = leader.getGridY();
                origin = "leader";
            }
            else
            {
                com.oddlabs.tt.model.Building armory_2 = com.oddlabs.tt.player.gauntlet.Shepherd.armory(f.copy);
                if (armory_2 == null)
                {
                    return;
                }
                ox = armory_2.getGridX();
                oy = armory_2.getGridY();
                origin = "armory";
            }
            if (!f.home && f.leader != null && ((object)f.leader != (object)leader) && !f.leader.isDead() && (f.leader.getPrimaryController() is com.oddlabs.tt.model.behaviour.WalkController walk) && walk.isAgressive())
            {
                int tx = walk.getTarget().getGridX();
                int ty = walk.getTarget().getGridY();
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(tx, ty, f.leader.getGridX(), f.leader.getGridY()) > 20 * 20)
                {
                    f.launches++;
                    if (f.spot_x >= 0 && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(tx, ty, f.spot_x, f.spot_y) <= 4 * 4)
                    {
                        ai.aiLog().count("wave_drawn");
                        ai.log("wave of " + java.lang.JStr.valueOf(com.oddlabs.tt.player.gauntlet.Shepherd.name(f)) + " drawn to " + (tx) + "," + (ty));
                    }
                    else
                    {
                        com.oddlabs.tt.model.Building near = nearestOwnBuilding(tx, ty);
                        bool @base = near != null && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(near.getGridX(), near.getGridY(), tx, ty) <= 20 * 20;
                        bool decoy = @base && ai.decoys().isDecoy(near);
                        if (@base && !decoy)
                        {
                            f.base_waves++;
                        }
                        ai.aiLog().count((decoy ? "wave_to_decoy" : (@base ? "wave_to_base" : "wave_elsewhere")));
                        ai.log("wave of " + java.lang.JStr.valueOf(com.oddlabs.tt.player.gauntlet.Shepherd.name(f)) + " goes to " + (tx) + "," + (ty) + java.lang.JStr.valueOf(((decoy ? " (our decoy)" : (@base ? " (our base)" : "")))) + " (spot " + (f.spot_x) + "," + (f.spot_y) + ")");
                    }
                    if (nearShepherd(tx, ty, 6))
                    {
                        ai.aiLog().count("wave_to_shepherd");
                    }
                    if (ai.logging())
                    {
                        logLaunch(f, tx, ty);
                    }
                    if (strategy.raid_evac)
                    {
                        ai.economy().waveLaunched(f.copy, tx, ty);
                    }
                    f.prev_wave = f.leader;
                }
            }
            f.leader = leader;
            if (leader != null)
            {
                f.lead_x = leader.getGridX();
                f.lead_y = leader.getGridY();
            }
            if (!f.home && leader != null && (strategy.shepherd_follow || f.partner != null))
            {
                com.oddlabs.tt.model.Unit prev = f.prev_wave;
                com.oddlabs.tt.model.Building armory_3 = com.oddlabs.tt.player.gauntlet.Shepherd.armory(f.copy);
                if (prev != null && !prev.isDead() && armory_3 != null && !((prev.getPrimaryController() is com.oddlabs.tt.model.behaviour.IdleController)) && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(ox, oy, armory_3.getGridX(), armory_3.getGridY()) <= com.oddlabs.tt.player.gauntlet.Shepherd.HOME_CELLS * com.oddlabs.tt.player.gauntlet.Shepherd.HOME_CELLS)
                {
                    if ((prev.getPrimaryController() is com.oddlabs.tt.model.behaviour.WalkController walk_2) && walk_2.isAgressive())
                    {
                        ox = walk_2.getTarget().getGridX();
                        oy = walk_2.getTarget().getGridY();
                    }
                    else
                    {
                        ox = prev.getGridX();
                        oy = prev.getGridY();
                    }
                    origin = "wave";
                    ai.aiLog().count("shepherd_follow");
                }
            }
            if (ai.strategy().shepherd_hold)
            {
                int num = java.lang.Math.min(40, 10 + 5 * f.launches);
                int idle = 0;
                foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(intel.enemy_warriors))
                {
                    if (!e.isDead() && ((object)e.getOwner() == (object)f.copy) && (e.getPrimaryController() is com.oddlabs.tt.model.behaviour.IdleController))
                    {
                        idle++;
                    }
                }
                bool was = f.imminent;
                f.imminent = idle >= num && (num < 20 || f.copy.hasActiveChieftain());
                if (f.imminent && !was)
                {
                    ai.aiLog().count("shepherd_imminent");
                }
            }
            if (ai.time() < strategy.shepherd_time)
            {
                return;
            }
            if (f.shepherd == null)
            {
                if ((float)(ai.time() - f.lost_at) < strategy.shepherd_gap)
                {
                    ai.aiLog().count("shepherd_gap_wait");
                    return;
                }
                int range = ai.strategy().shepherd_range;
                if (range < 100000 && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(ox, oy, ai.planner().getStartX(), ai.planner().getStartY()) > range * range)
                {
                    return;
                }
                int[] probe = findSpot(f, ox, oy, null, intel);
                if (probe == null)
                {
                    ai.aiLog().count("shepherd_t_none");
                    if (ai.strategy().site_shepherd && ai.time() >= ai.strategy().shepherd_time)
                    {
                        ai.decoys().placeHome(f.copy, ox, oy);
                    }
                    return;
                }
                com.oddlabs.tt.model.Unit candidate = recruit(ox, oy, intel);
                if (candidate == null)
                {
                    ai.aiLog().count("shepherd_t_none");
                    ai.aiLog().count("shepherd_norecruit");
                    return;
                }
                bool lead = strategy.shepherd_lead > 0.0f && f.launches == 0 && leader == null;
                int walk2 = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(candidate.getGridX(), candidate.getGridY(), probe[0], probe[1]);
                if (lead)
                {
                    float eta = (float)((float)(((float)(java.lang.Math.sqrt(walk2))) / strategy.shepherd_speed) + 5.0f);
                    if (f.armory_at < 0.0f || ai.time() < (float)((float)((float)(f.armory_at + strategy.shepherd_lead) - eta) - strategy.shepherd_lead_margin))
                    {
                        ai.aiLog().count("shepherd_wait");
                        return;
                    }
                }
                com.oddlabs.tt.player.gauntlet.Intel.PeonState state = ((com.oddlabs.tt.player.gauntlet.Intel.PeonState)intel.peon_states.get(candidate));
                f.shepherd = candidate;
                intel.shepherds.add(candidate);
                f.recruited = ai.time();
                f.nospot_since = (float)(-1.0f);
                ai.aiLog().count("shepherd_recruit");
                if (com.oddlabs.tt.player.gauntlet.Shepherd.loaded(candidate))
                {
                    ai.aiLog().count("shepherd_recruit_loaded");
                }
                if (f.home)
                {
                    ai.aiLog().count("shepherd_recruit_home");
                }
                if (lead)
                {
                    ai.aiLog().count("shepherd_lead_recruit");
                }
                f.arrived = false;
                f.last_state = "walk";
                f.flee_until = (float)(-1.0f);
                f.rec_x = candidate.getGridX();
                f.rec_y = candidate.getGridY();
                f.rec_spot_x = probe[0];
                f.rec_spot_y = probe[1];
                f.prev_spot_x = (-1);
                if (ai.logging())
                {
                    ai.log("shepherd of " + java.lang.JStr.valueOf(com.oddlabs.tt.player.gauntlet.Shepherd.name(f)) + " recruited: " + java.lang.JStr.valueOf(((state == null ? "?" : state.name().toLowerCase(java.util.Locale.ROOT)))) + java.lang.JStr.valueOf(((com.oddlabs.tt.player.gauntlet.Shepherd.loaded(candidate) ? " loaded" : ""))) + " peon at " + (f.rec_x) + "," + (f.rec_y) + ", spot " + (probe[0]) + "," + (probe[1]) + ", walk " + (java.lang.JC.d2i(java.lang.Math.sqrt(walk2))) + " cells, origin " + java.lang.JStr.valueOf(origin) + java.lang.JStr.valueOf(((lead ? ", lead (armory at " + (java.lang.JC.f2i(f.armory_at)) + " s)" : ""))));
                }
            }
            com.oddlabs.tt.model.Unit s = f.shepherd;
            int moved = java.lang.Math.abs(s.getGridX() - f.last_x) + java.lang.Math.abs(s.getGridY() - f.last_y);
            f.last_x = s.getGridX();
            f.last_y = s.getGridY();
            int[] away = threatAway(s, intel, (ai.strategy().shepherd_hold && f.imminent ? 9 : com.oddlabs.tt.player.gauntlet.Shepherd.CLEAR_CELLS));
            if (away != null)
            {
                ai.aiLog().count("shepherd_t_flee");
                f.last_state = "flee";
                if (ai.logging())
                {
                    ai.log("flee of " + java.lang.JStr.valueOf(com.oddlabs.tt.player.gauntlet.Shepherd.name(f)) + " at " + (s.getGridX()) + "," + (s.getGridY()) + " (moved " + (moved) + ") from " + (threat_warriors) + " warriors, " + (threat_hunters) + " hunters, " + (threat_peons) + " peons, " + (threat_coming) + " coming, to " + (away[0]) + "," + (away[1]));
                }
                if ((float)(ai.time() - f.last_order) >= 0.3f)
                {
                    flee(f, s, away);
                }
                return;
            }
            int[] spot = findSpot(f, ox, oy, s, intel);
            if (spot == null)
            {
                f.spot_x = (-1);
                ai.aiLog().count("shepherd_t_nospot");
                f.last_state = "nospot";
                if (f.nospot_since < 0.0f)
                {
                    f.nospot_since = ai.time();
                }
                else if ((float)(ai.time() - f.nospot_since) > ai.strategy().shepherd_patience)
                {
                    ai.aiLog().count("shepherd_home");
                    release(f);
                }
                return;
            }
            f.nospot_since = (float)(-1.0f);
            if (f.prev_spot_x >= 0 && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(f.prev_spot_x, f.prev_spot_y, spot[0], spot[1]) > 30 * 30)
            {
                ai.aiLog().count("shepherd_spot_jump");
                if (ai.logging())
                {
                    ai.log("spot of " + java.lang.JStr.valueOf(com.oddlabs.tt.player.gauntlet.Shepherd.name(f)) + " jumps " + (java.lang.JC.d2i(java.lang.Math.sqrt(com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(f.prev_spot_x, f.prev_spot_y, spot[0], spot[1])))) + " cells from " + (f.prev_spot_x) + "," + (f.prev_spot_y) + " (origin " + java.lang.JStr.valueOf(f.origin) + ") to " + (spot[0]) + "," + (spot[1]) + " (origin " + java.lang.JStr.valueOf(origin) + ")");
                }
            }
            if (spot[0] != f.spot_x || spot[1] != f.spot_y)
            {
                f.blocked_since = (float)(-1.0f);
            }
            f.prev_spot_x = spot[0];
            f.prev_spot_y = spot[1];
            f.origin = origin;
            f.spot_x = spot[0];
            f.spot_y = spot[1];
            if (!f.arrived && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(s.getGridX(), s.getGridY(), spot[0], spot[1]) <= 3 * 3)
            {
                f.arrived = true;
                ai.aiLog().count("shepherd_at_spot");
                if (ai.logging())
                {
                    ai.log("shepherd of " + java.lang.JStr.valueOf(com.oddlabs.tt.player.gauntlet.Shepherd.name(f)) + " at spot after " + (java.lang.JC.f2i(((float)(ai.time() - f.recruited)))) + " s (from " + (f.rec_x) + "," + (f.rec_y) + ", " + (java.lang.JC.d2i(java.lang.Math.sqrt(com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(f.rec_x, f.rec_y, s.getGridX(), s.getGridY())))) + " cells; spot moved " + (java.lang.JC.d2i(java.lang.Math.sqrt(com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(f.rec_spot_x, f.rec_spot_y, spot[0], spot[1])))) + " cells since recruited)");
                }
            }
            bool on_spot = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(s.getGridX(), s.getGridY(), spot[0], spot[1]) <= 3 * 3;
            ai.aiLog().count((on_spot ? "shepherd_t_atspot" : "shepherd_t_walk"));
            f.last_state = (on_spot ? "at" : "walk");
            bool held = strategy.shepherd_safe_walk && ai.time() < f.flee_until;
            if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(s.getGridX(), s.getGridY(), spot[0], spot[1]) > 2 * 2 && (float)(ai.time() - f.last_order) >= 2.0f && !held)
            {
                ai.landscapeOrder(com.oddlabs.tt.model.Selectable.newArray(s), spot[0], spot[1], com.oddlabs.tt.model.Action.MOVE, false);
                f.last_order = ai.time();
            }
        }
        // Java: 504
        public static com.oddlabs.tt.model.Unit oldestIdleWarrior(com.oddlabs.tt.player.Player p)
        {
            foreach (com.oddlabs.tt.model.Selectable sel in java.lang.JIter.of(p.getUnits().getSet()))
            {
                if (!((sel is com.oddlabs.tt.model.Unit u)) || u.isDead() || u.isMounted())
                {
                    continue;
                }
                if (!((u.getPrimaryController() is com.oddlabs.tt.model.behaviour.IdleController)))
                {
                    continue;
                }
                if (u.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD) || u.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC))
                {
                    continue;
                }
                if (u.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.ATTACK))
                {
                    return u;
                }
            }
            return null;
        }
        // Java: 518
        public static com.oddlabs.tt.model.Building armory(com.oddlabs.tt.player.Player p)
        {
            foreach (com.oddlabs.tt.model.Selectable sel in java.lang.JIter.of(p.getUnits().getSet()))
            {
                if ((sel is com.oddlabs.tt.model.Building b) && !b.isDead() && b.isComplete() && ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID() == com.oddlabs.tt.model.Race.BUILDING_ARMORY)
                {
                    return b;
                }
            }
            return null;
        }
        // Java: 526
        public com.oddlabs.tt.model.Unit recruit(int ox, int oy, com.oddlabs.tt.player.gauntlet.Intel intel)
        {
            com.oddlabs.tt.model.Unit best = null;
            int best_d = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Unit p in java.lang.JIter.of(intel.peons))
            {
                com.oddlabs.tt.player.gauntlet.Intel.PeonState st = ((com.oddlabs.tt.player.gauntlet.Intel.PeonState)intel.peon_states.get(p));
                if (((object)st != (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.IDLE) && ((object)st != (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_TREE) && ((object)st != (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_ROCK) && ((object)st != (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_IRON) && ((object)st != (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.TRANSIT) && ((object)st != (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.MOVE))
                {
                    continue;
                }
                if (intel.shepherds.contains(p) || ai.economy().reservedPlacer(p))
                {
                    continue;
                }
                int danger = com.oddlabs.tt.player.gauntlet.Shepherd.nearestEnemy(intel.enemy_warriors, p.getGridX(), p.getGridY());
                if (danger >= 0 && danger <= 14)
                {
                    continue;
                }
                int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(p.getGridX(), p.getGridY(), ox, oy);
                if (d < best_d)
                {
                    best_d = d;
                    best = p;
                }
            }
            return best;
        }
        // Java: 551
        public static bool loaded(com.oddlabs.tt.model.Unit peon)
        {
            return peon.getSupplyContainer() != null && peon.getSupplyContainer().getNumSupplies() > 0;
        }
        // Java: 556
        public int[] threatAway(com.oddlabs.tt.model.Unit s, com.oddlabs.tt.player.gauntlet.Intel intel, int clear)
        {
            int sx = s.getGridX();
            int sy = s.getGridY();
            long ex = 0;
            long ey = 0;
            int n = 0;
            com.oddlabs.tt.player.gauntlet.EnemyIndex index = intel.enemyIndex(ai.ticks());
            threat_coming = 0;
            threat_peons = 0;
            threat_hunters = 0;
            threat_warriors = 0;
            int[] candidates = index.queryUnordered(sx, sy, java.lang.Math.max(40 * 40, 2 * clear * clear));
            for (int k = 0, m = index.count(); k < m; k++)
            {
                sbyte group = index.group(candidates[k]);
                com.oddlabs.tt.model.Unit e = index.unit(candidates[k]);
                if (group == com.oddlabs.tt.player.gauntlet.EnemyIndex.CHIEFTAIN || e.isDead())
                {
                    continue;
                }
                int dx = e.getGridX() - sx;
                int dy = e.getGridY() - sy;
                bool near = java.lang.Math.abs(dx) <= clear && java.lang.Math.abs(dy) <= clear;
                bool coming = false;
                if (group == com.oddlabs.tt.player.gauntlet.EnemyIndex.WARRIOR && !near && (e.getPrimaryController() is com.oddlabs.tt.model.behaviour.WalkController w) && w.isAgressive() && dx * dx + dy * dy <= 40 * 40)
                {
                    int tx = w.getTarget().getGridX() - sx;
                    int ty = w.getTarget().getGridY() - sy;
                    coming = tx * tx + ty * ty <= 14 * 14;
                }
                if (near || coming)
                {
                    ex += e.getGridX();
                    ey += e.getGridY();
                    n++;
                    if (coming)
                    {
                        threat_coming++;
                    }
                    else if (group == com.oddlabs.tt.player.gauntlet.EnemyIndex.PEON)
                    {
                        threat_peons++;
                    }
                    else if ((e.getCurrentController() is com.oddlabs.tt.model.behaviour.HuntController hunt) && ((object)hunt.getTarget() == (object)s))
                    {
                        threat_hunters++;
                    }
                    else
                    {
                        threat_warriors++;
                    }
                }
            }
            if (n == 0)
            {
                return null;
            }
            float cx = (float)(((float)(ex)) / n);
            float cy = (float)(((float)(ey)) / n);
            float dx_2 = (float)(sx - cx);
            float dy_2 = (float)(sy - cy);
            float len = ((float)(java.lang.Math.sqrt((float)((float)(dx_2 * dx_2) + (float)(dy_2 * dy_2)))));
            if (len < 0.5f)
            {
                dx_2 = ai.planner().getStartX() - sx;
                dy_2 = ai.planner().getStartY() - sy;
                len = java.lang.Math.max(1.0f, ((float)(java.lang.Math.sqrt((float)((float)(dx_2 * dx_2) + (float)(dy_2 * dy_2))))));
            }
            return new int[] { sx + java.lang.Math.round((float)((float)(22 * dx_2) / len)), sy + java.lang.Math.round((float)((float)(22 * dy_2) / len)) };
        }
        // Java: 622
        public int[] findSpot(com.oddlabs.tt.player.gauntlet.Shepherd.Flock f, int ox, int oy, com.oddlabs.tt.model.Unit s, com.oddlabs.tt.player.gauntlet.Intel intel)
        {
            int building2 = nearestOwnBuilding2(ox, oy);
            if (building2 == java.lang.Integer.MAX_VALUE)
            {
                return null;
            }
            int unit2 = nearestOtherUnit2(ox, oy, s, (f.partner == null ? (com.oddlabs.tt.model.Unit)(null) : f.partner.shepherd));
            float limit = ((float)(java.lang.Math.sqrt(java.lang.Math.min((float)(building2 * 0.44f), (float)(unit2 * 0.8f)))));
            int max_r = java.lang.JC.f2i(java.lang.Math.min(ai.strategy().shepherd_max_r, limit));
            bool rejections = ai.logging();
            if (rejections)
            {
                for (int r = 14; r <= ai.strategy().shepherd_max_r; r += (r < 22 ? 2 : 4))
                {
                    if (r > max_r)
                    {
                        for (int a = 0; a < 24; a++)
                        {
                            ai.aiLog().count("shepherd_rej_leash");
                        }
                    }
                }
            }
            if (max_r < 14)
            {
                ai.aiLog().count(((float)(building2 * 0.44f) < (float)(unit2 * 0.8f) ? "shepherd_nospot_building" : "shepherd_nospot_unit"));
                return null;
            }
            java.util.List guarded = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Selectable sel in java.lang.JIter.of(f.copy.getUnits().getSet()))
            {
                if ((sel is com.oddlabs.tt.model.Building b) && !b.isDead() && ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getType() == com.oddlabs.tt.model.BuildingTemplate.TYPE_BUILDING && ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID() != com.oddlabs.tt.model.Race.BUILDING_TOWER)
                {
                    guarded.add(b);
                }
            }
            com.oddlabs.tt.player.gauntlet.DistanceField reach = ai.planner().getStartField();
            int bx = ai.planner().getStartX();
            int by = ai.planner().getStartY();
            com.oddlabs.tt.player.gauntlet.Strategy strategy = ai.strategy();
            bool sticky = s != null && strategy.shepherd_sticky > 0.0f && f.spot_x >= 0;
            float travel = (s == null ? 0.0f : strategy.shepherd_travel);
            bool safe = s != null && strategy.shepherd_safe_walk;
            int n = 0;
            if (sticky)
            {
                int x = f.spot_x;
                int y = f.spot_y;
                int r2 = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, ox, oy);
                if (r2 >= 12 * 12 && r2 <= max_r * max_r && reach.reachable(x, y) && com.oddlabs.tt.player.gauntlet.Shepherd.coverAt(guarded, intel, x, y) == 0)
                {
                    bool blocked = enemyNear(intel, x, y) != null;
                    if (!blocked)
                    {
                        f.blocked_since = (float)(-1.0f);
                    }
                    else if (f.blocked_since < 0.0f)
                    {
                        f.blocked_since = ai.time();
                    }
                    if (!blocked || (float)(ai.time() - f.blocked_since) < strategy.shepherd_grace)
                    {
                        float score = (float)(spotScore(x, y, ((float)(java.lang.Math.sqrt(r2))), bx, by, guarded) + strategy.shepherd_sticky);
                        if (travel > 0.0f)
                        {
                            score = (float)(score - (float)(travel * ((float)(java.lang.Math.sqrt(com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, s.getGridX(), s.getGridY()))))));
                        }
                        n = addCandidate(n, x, y, score);
                    }
                }
            }
            for (int r_2 = 14; r_2 <= max_r; r_2 += (r_2 < 22 ? 2 : 4))
            {
                for (int a_2 = 0; a_2 < 24; a_2++)
                {
                    double ang = a_2 * java.lang.Math.PI / 12;
                    int x_2 = ox + unchecked((int)(java.lang.Math.round(r_2 * java.lang.StrictMath.cos(ang))));
                    int y_2 = oy + unchecked((int)(java.lang.Math.round(r_2 * java.lang.StrictMath.sin(ang))));
                    if (!reach.reachable(x_2, y_2))
                    {
                        if (rejections)
                        {
                            ai.aiLog().count("shepherd_rej_reach");
                        }
                        continue;
                    }
                    string enemy = enemyNear(intel, x_2, y_2);
                    if (enemy != null)
                    {
                        if (rejections)
                        {
                            ai.aiLog().count(enemy);
                        }
                        continue;
                    }
                    int cover = com.oddlabs.tt.player.gauntlet.Shepherd.coverAt(guarded, intel, x_2, y_2);
                    if (cover != 0)
                    {
                        if (rejections)
                        {
                            ai.aiLog().count((cover == 1 ? "shepherd_rej_defense17" : "shepherd_rej_tower19"));
                        }
                        continue;
                    }
                    float score_2 = spotScore(x_2, y_2, r_2, bx, by, guarded);
                    if (sticky && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x_2, y_2, f.spot_x, f.spot_y) <= com.oddlabs.tt.player.gauntlet.Shepherd.STICKY_CELLS * com.oddlabs.tt.player.gauntlet.Shepherd.STICKY_CELLS)
                    {
                        score_2 = (float)(score_2 + strategy.shepherd_sticky);
                    }
                    if (travel > 0.0f)
                    {
                        score_2 = (float)(score_2 - (float)(travel * ((float)(java.lang.Math.sqrt(com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x_2, y_2, s.getGridX(), s.getGridY()))))));
                    }
                    n = addCandidate(n, x_2, y_2, score_2);
                }
            }
            if (n == 0)
            {
                ai.aiLog().count("shepherd_nospot_ground");
                return null;
            }
            int first = bestCandidate(n);
            if (!safe)
            {
                return new int[] { cand_x[first], cand_y[first] };
            }
            int pick = first;
            int first_x = cand_x[first];
            int first_y = cand_y[first];
            for (int tries = 0; tries < com.oddlabs.tt.player.gauntlet.Shepherd.SAFE_TRIES; tries++)
            {
                if (safePath(s.getGridX(), s.getGridY(), cand_x[pick], cand_y[pick], intel))
                {
                    if (tries > 0)
                    {
                        ai.aiLog().count("shepherd_safe_detour");
                    }
                    return new int[] { cand_x[pick], cand_y[pick] };
                }
                cand_score[pick] = (float)(-java.lang.Float.MAX_VALUE);
                pick = bestCandidate(n);
                if (cand_score[pick] == (float)(-java.lang.Float.MAX_VALUE))
                {
                    break;
                }
            }
            ai.aiLog().count("shepherd_safe_none");
            return new int[] { first_x, first_y };
        }
        // Java: 735
        public float spotScore(int x, int y, float r, int bx, int by, java.util.List guarded)
        {
            float score = (float)(((float)(java.lang.Math.sqrt(com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, bx, by)))) - (float)(r * 0.5f));
            float home_weight = ai.strategy().shepherd_home_weight;
            if (home_weight > 0.0f && !guarded.isEmpty())
            {
                int home = java.lang.Integer.MAX_VALUE;
                foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(guarded))
                {
                    home = java.lang.Math.min(home, com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(b.getGridX(), b.getGridY(), x, y));
                }
                score = (float)(score + (float)(home_weight * ((float)(java.lang.Math.sqrt(home)))));
            }
            return score;
        }
        // Java: 751
        public static int coverAt(java.util.List guarded, com.oddlabs.tt.player.gauntlet.Intel intel, int x, int y)
        {
            foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(guarded))
            {
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(b.getGridX(), b.getGridY(), x, y) <= com.oddlabs.tt.player.gauntlet.Shepherd.DEFENSE_CELLS * com.oddlabs.tt.player.gauntlet.Shepherd.DEFENSE_CELLS)
                {
                    return 1;
                }
            }
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(intel.enemy_towers))
            {
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(t.getGridX(), t.getGridY(), x, y) <= com.oddlabs.tt.player.gauntlet.Shepherd.TOWER_CELLS * com.oddlabs.tt.player.gauntlet.Shepherd.TOWER_CELLS)
                {
                    return 2;
                }
            }
            return 0;
        }
        // Java: 761
        public int addCandidate(int n, int x, int y, float score)
        {
            if (n == cand_x.Length)
            {
                cand_x = java.util.Arrays.copyOf(cand_x, n * 2);
                cand_y = java.util.Arrays.copyOf(cand_y, n * 2);
                cand_score = java.util.Arrays.copyOf(cand_score, n * 2);
            }
            cand_x[n] = x;
            cand_y[n] = y;
            cand_score[n] = score;
            return n + 1;
        }
        // Java: 774
        public int bestCandidate(int n)
        {
            int best = 0;
            for (int i = 1; i < n; i++)
            {
                if (cand_score[i] > cand_score[best])
                {
                    best = i;
                }
            }
            return best;
        }
        // Java: 786
        public bool safePath(int sx, int sy, int x, int y, com.oddlabs.tt.player.gauntlet.Intel intel)
        {
            com.oddlabs.tt.player.gauntlet.Strategy strategy = ai.strategy();
            float dx = x - sx;
            float dy = y - sy;
            float len = ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy)))));
            if (len < 1.0f)
            {
                return true;
            }
            float look = java.lang.Math.min(len, strategy.shepherd_safe_look);
            int clear2 = strategy.shepherd_safe_clear * strategy.shepherd_safe_clear;
            com.oddlabs.tt.player.gauntlet.EnemyIndex index = intel.enemyIndex(ai.ticks());
            for (float d = 0.0f; ; d = (float)(d + 6.0f))
            {
                float t = java.lang.Math.min(d, look);
                int px = sx + java.lang.Math.round((float)((float)(dx * t) / len));
                int py = sy + java.lang.Math.round((float)((float)(dy * t) / len));
                int[] found = index.queryUnordered(px, py, clear2);
                for (int k = 0, m = index.count(); k < m; k++)
                {
                    if (!index.isPeon(found[k]) && !index.unit(found[k]).isDead())
                    {
                        return false;
                    }
                }
                if (t >= look)
                {
                    return true;
                }
            }
        }
        // Java: 813
        public string enemyNear(com.oddlabs.tt.player.gauntlet.Intel intel, int x, int y)
        {
            int clear = ai.strategy().shepherd_clear;
            com.oddlabs.tt.player.gauntlet.EnemyIndex index = intel.enemyIndex(ai.ticks());
            int groups = index.groupsInBox(x, y, clear);
            if ((groups & 1 << com.oddlabs.tt.player.gauntlet.EnemyIndex.WARRIOR) != 0)
            {
                return "shepherd_rej_warrior";
            }
            if ((groups & 1 << com.oddlabs.tt.player.gauntlet.EnemyIndex.PEON) != 0)
            {
                return "shepherd_rej_peon";
            }
            return ((groups & 1 << com.oddlabs.tt.player.gauntlet.EnemyIndex.CHIEFTAIN) != 0 ? "shepherd_rej_chief" : (string)(null));
        }
        // Java: 825
        public int baseWaves(com.oddlabs.tt.player.Player p)
        {
            com.oddlabs.tt.player.gauntlet.Shepherd.Flock f = flockOf(p);
            return (f == null ? 0 : f.base_waves);
        }
        // Java: 831
        public bool nearShepherd(int x, int y, int r)
        {
            foreach (com.oddlabs.tt.player.gauntlet.Shepherd.Flock f in java.lang.JIter.of(flocks))
            {
                com.oddlabs.tt.model.Unit s = f.shepherd;
                if (s != null && !s.isDead() && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(s.getGridX(), s.getGridY(), x, y) <= r * r)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 844
        public void logLaunch(com.oddlabs.tt.player.gauntlet.Shepherd.Flock f, int tx, int ty)
        {
            com.oddlabs.tt.model.Unit prev = f.prev_wave;
            string prev_state = (prev == null ? "none" : (prev.isDead() ? "dead" : ((prev.getPrimaryController() is com.oddlabs.tt.model.behaviour.WalkController) ? "walking" : ((prev.getPrimaryController() is com.oddlabs.tt.model.behaviour.IdleController) ? "idle" : ((prev.getPrimaryController() is com.oddlabs.tt.model.behaviour.HuntController) ? "hunting" : "busy")))));
            com.oddlabs.tt.model.Unit nearest = null;
            int nearest_d = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Selectable sel in java.lang.JIter.of(ai.owner().getUnits().getSet()))
            {
                if ((sel is com.oddlabs.tt.model.Unit u) && !u.isDead() && !u.isMounted())
                {
                    int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(u.getGridX(), u.getGridY(), tx, ty);
                    if (d < nearest_d)
                    {
                        nearest_d = d;
                        nearest = u;
                    }
                }
            }
            com.oddlabs.tt.model.Unit s = f.shepherd;
            int shepherd_d = (s == null || s.isDead() ? (-1) : java.lang.JC.d2i(java.lang.Math.sqrt(com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(s.getGridX(), s.getGridY(), tx, ty))));
            string own = (s == null || s.isDead() ? "none" : java.lang.JStr.valueOf(((f.spot_x < 0 ? "nospot" : (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(s.getGridX(), s.getGridY(), f.spot_x, f.spot_y) <= 6 * 6 ? "atspot" : "away")))) + " " + (java.lang.JC.d2i(java.lang.Math.sqrt(com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(s.getGridX(), s.getGridY(), f.lead_x, f.lead_y)))) + " cells");
            int building2 = nearestOwnBuilding2(f.lead_x, f.lead_y);
            string prev_at = (prev == null || prev.isDead() ? "none" : (prev.getGridX()) + "," + (prev.getGridY()));
            ai.log("launch of " + java.lang.JStr.valueOf(com.oddlabs.tt.player.gauntlet.Shepherd.name(f)) + " from " + (f.lead_x) + "," + (f.lead_y) + " (previous wave " + java.lang.JStr.valueOf(prev_state) + ") to " + (tx) + "," + (ty) + ": target " + java.lang.JStr.valueOf(targetClass(tx, ty)) + ", nearest unit " + java.lang.JStr.valueOf(((nearest == null ? "none" : java.lang.JStr.valueOf(roleOf(nearest)) + " " + (java.lang.JC.d2i(java.lang.Math.sqrt(nearest_d))) + " cells"))) + ", own shepherd " + (shepherd_d) + " cells; origin: shepherd " + java.lang.JStr.valueOf(own) + ", building " + (((building2 == java.lang.Integer.MAX_VALUE ? (-1) : java.lang.JC.d2i(java.lang.Math.sqrt(building2))))) + " cells, previous wave at " + java.lang.JStr.valueOf(prev_at));
        }
        // Java: 878
        public string targetClass(int tx, int ty)
        {
            com.oddlabs.tt.model.Selectable best = null;
            int best_d = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Selectable sel in java.lang.JIter.of(ai.owner().getUnits().getSet()))
            {
                if (sel.isDead() || ((sel is com.oddlabs.tt.model.Unit u) && u.isMounted()))
                {
                    continue;
                }
                int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(sel.getGridX(), sel.getGridY(), tx, ty);
                if (d < best_d)
                {
                    best_d = d;
                    best = sel;
                }
            }
            if (best == null || best_d > 4 * 4)
            {
                return "none";
            }
            if ((best is com.oddlabs.tt.model.Building b))
            {
                bool done = b.isComplete();
                return ((((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID()) switch { 2 => (string)((done ? "tower" : (ai.decoys().isDecoy(b) ? "decoy_site" : "tower_site"))), 1 => (string)((done ? "armory" : "armory_site")), 0 => (string)((done ? "quarters" : "quarters_site")), _ => (string)("building") });
            }
            if (nearShepherd(tx, ty, 6))
            {
                return "unit_by_shepherd";
            }
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            foreach (com.oddlabs.tt.model.Building b_2 in java.lang.JIter.of(intel.quarters))
            {
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(b_2.getGridX(), b_2.getGridY(), tx, ty) <= 20 * 20)
                {
                    return "unit_by_base";
                }
            }
            foreach (com.oddlabs.tt.model.Building b_3 in java.lang.JIter.of(intel.armories))
            {
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(b_3.getGridX(), b_3.getGridY(), tx, ty) <= 20 * 20)
                {
                    return "unit_by_base";
                }
            }
            return "field_unit";
        }
        // Java: 914
        public string roleOf(com.oddlabs.tt.model.Unit u)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            foreach (com.oddlabs.tt.player.gauntlet.Shepherd.Flock f in java.lang.JIter.of(flocks))
            {
                if (((object)f.shepherd == (object)u))
                {
                    return "shepherd of " + java.lang.JStr.valueOf(com.oddlabs.tt.player.gauntlet.Shepherd.name(f));
                }
            }
            if (intel.shepherds.contains(u))
            {
                return "shepherd";
            }
            if (intel.lures.contains(u))
            {
                return "lure";
            }
            if (intel.sappers.contains(u))
            {
                return "sapper";
            }
            com.oddlabs.tt.player.gauntlet.Intel.PeonState state = ((com.oddlabs.tt.player.gauntlet.Intel.PeonState)intel.peon_states.get(u));
            if (state != null)
            {
                return state.name().toLowerCase(java.util.Locale.ROOT);
            }
            if (((object)u == (object)intel.chieftain))
            {
                return "chieftain";
            }
            string role = ai.military().roleOf(u);
            return (role != null ? role : "warrior");
        }
        // Java: 934
        public static int nearestEnemy(java.util.List units, int x, int y)
        {
            int best = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(units))
            {
                if (!u.isDead())
                {
                    best = java.lang.Math.min(best, com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(u.getGridX(), u.getGridY(), x, y));
                }
            }
            return (best == java.lang.Integer.MAX_VALUE ? (-1) : java.lang.JC.d2i(java.lang.Math.sqrt(best)));
        }
        // Java: 942
        public int nearestTower(int x, int y)
        {
            int best = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(ai.intel().enemy_towers))
            {
                best = java.lang.Math.min(best, com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(t.getGridX(), t.getGridY(), x, y));
            }
            return (best == java.lang.Integer.MAX_VALUE ? (-1) : java.lang.JC.d2i(java.lang.Math.sqrt(best)));
        }
        // Java: 950
        public com.oddlabs.tt.model.Building nearestOwnBuilding(int x, int y)
        {
            com.oddlabs.tt.model.Building best = null;
            int best_d = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Selectable sel in java.lang.JIter.of(ai.owner().getUnits().getSet()))
            {
                if ((sel is com.oddlabs.tt.model.Building b) && !b.isDead() && ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getType() == com.oddlabs.tt.model.BuildingTemplate.TYPE_BUILDING)
                {
                    int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(b.getGridX(), b.getGridY(), x, y);
                    if (d < best_d)
                    {
                        best_d = d;
                        best = b;
                    }
                }
            }
            return best;
        }
        // Java: 966
        public int nearestOwnBuilding2(int x, int y)
        {
            com.oddlabs.tt.model.Building b = nearestOwnBuilding(x, y);
            return (b == null ? java.lang.Integer.MAX_VALUE : com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(b.getGridX(), b.getGridY(), x, y));
        }
        // Java: 971
        public int nearestOtherUnit2(int x, int y, com.oddlabs.tt.model.Unit self, com.oddlabs.tt.model.Unit partner)
        {
            int best = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Selectable sel in java.lang.JIter.of(ai.owner().getUnits().getSet()))
            {
                if ((sel is com.oddlabs.tt.model.Unit u) && ((object)u != (object)self) && ((object)u != (object)partner) && !u.isDead() && !u.isMounted())
                {
                    best = java.lang.Math.min(best, com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(u.getGridX(), u.getGridY(), x, y));
                }
            }
            return best;
        }
        private void init_Shepherd()
        {
            this.flocks = new java.util.ArrayList();
            this.last_tick = (float)(-10.0f);
            this.cand_x = new int[128];
            this.cand_y = new int[128];
            this.cand_score = new float[128];
        }
        public Shepherd(com.oddlabs.tt.player.gauntlet.GauntletAI ai) : base()
        {
            init_Shepherd();
            this.ai = ai;
        }
    }
}
