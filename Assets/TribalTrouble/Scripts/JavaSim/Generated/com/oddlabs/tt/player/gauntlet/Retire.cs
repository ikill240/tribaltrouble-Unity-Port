// Translated from Retire.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.gauntlet
{
    // Java: 37
    public class Retire : java.lang.Object
    {
        public static string[] KINDS;
        public const int SITE = 0;
        public const int TOWER = 1;
        public const int ARMORY = 2;
        public const int QUARTERS = 3;
        public const int ANY_TOWER = 4;
        public const int COVER_CELLS = 25;
        public const float SITE_AGE = 120.0f;
        public const float GAP = 120.0f;
        public const float RETRY = 30.0f;
        public const float SEARCH_EVERY = 5.0f;
        public const float EMPTY_WAIT_TOWER = 10.0f;
        public const float EMPTY_WAIT = 60.0f;
        public const float UNITS_WAIT = 30.0f;
        public const float REORDER = 3.0f;
        public const int UNIT_CELLS = 80;
        public const int WARRIOR_CELLS = 60;
        public const int RETIRED_CELLS = 12;
        public const float RETIRED_MEMORY = 60.0f;
        public const float SLOT_USE = 60.0f;
        public com.oddlabs.tt.player.gauntlet.GauntletAI ai;
        public com.oddlabs.tt.player.gauntlet.Economy economy;
        public java.util.Set doomed;
        public java.util.Map site_seen;
        public java.util.List retired;
        public java.util.Map passed;
        public const float PASS_OVER = 120.0f;
        public com.oddlabs.tt.model.Building target;
        public int kind;
        public bool razing;
        public float started;
        public float raze_started;
        public float last_order;
        public java.util.List razers;
        public bool lent;
        public int buildings_before;
        public float next_deploy;
        public float capped_since;
        public float next_allowed;
        public float next_search;
        public float last_done;
        public float next_none_log;
        // Java: 110
        public bool isDoomed(com.oddlabs.tt.model.Building b)
        {
            return b != null && !doomed.isEmpty() && doomed.contains(b);
        }
        // Java: 115
        public bool retiredNear(int x, int y)
        {
            foreach (float[] r in java.lang.JIter.of(retired))
            {
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, java.lang.JC.f2i(r[0]), java.lang.JC.f2i(r[1])) <= com.oddlabs.tt.player.gauntlet.Retire.RETIRED_CELLS * com.oddlabs.tt.player.gauntlet.Retire.RETIRED_CELLS)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 123
        public void flaggedPlaced()
        {
            if ((float)(ai.time() - last_done) <= com.oddlabs.tt.player.gauntlet.Retire.SLOT_USE)
            {
                ai.aiLog().count("retire_slot_used");
            }
        }
        // Java: 129
        public void tick()
        {
            float now = ai.time();
            trackSites(now);
            retired.removeIf(new java.util.function.Predicate_L((object r_l) =>
            {
                float[] r = (float[])r_l;
                return (float)(now - r[2]) > com.oddlabs.tt.player.gauntlet.Retire.RETIRED_MEMORY;
            }));
            passed.entrySet().removeIf(new java.util.function.Predicate_L((object e_l) =>
            {
                java.util.Map.Entry e = (java.util.Map.Entry)e_l;
                return ((com.oddlabs.tt.model.Building)e.getKey()).isDead() || now >= ((float)((float?)e.getValue()));
            }));
            if (target != null)
            {
                follow(now);
                return;
            }
            com.oddlabs.tt.player.gauntlet.Economy.Project flagged = economy.flaggedWaiting();
            bool capped = !ai.owner().canBuild(com.oddlabs.tt.model.Race.BUILDING_ARMORY);
            if (flagged == null || !capped)
            {
                capped_since = (float)(-1.0f);
                return;
            }
            if (capped_since < 0.0f)
            {
                capped_since = now;
            }
            if ((float)(now - capped_since) < ai.strategy().retire_wait)
            {
                return;
            }
            ai.aiLog().count("retire_armed");
            if (now < next_allowed || now < next_search)
            {
                return;
            }
            next_search = (float)(now + com.oddlabs.tt.player.gauntlet.Retire.SEARCH_EVERY);
            int[][] why = java.lang.JA.newArray2<int>(com.oddlabs.tt.player.gauntlet.Retire.KINDS.Length, 2);
            com.oddlabs.tt.model.Building b = stalledSite(now, why[com.oddlabs.tt.player.gauntlet.Retire.SITE]);
            int k = com.oddlabs.tt.player.gauntlet.Retire.SITE;
            if (b == null)
            {
                b = quietTower(why[com.oddlabs.tt.player.gauntlet.Retire.TOWER], com.oddlabs.tt.player.gauntlet.Retire.COVER_CELLS);
                k = com.oddlabs.tt.player.gauntlet.Retire.TOWER;
            }
            if (b == null)
            {
                b = drainedArmory(why[com.oddlabs.tt.player.gauntlet.Retire.ARMORY]);
                k = com.oddlabs.tt.player.gauntlet.Retire.ARMORY;
            }
            if (b == null)
            {
                b = farQuarters(why[com.oddlabs.tt.player.gauntlet.Retire.QUARTERS]);
                k = com.oddlabs.tt.player.gauntlet.Retire.QUARTERS;
            }
            if (b == null && ai.strategy().retire_any_tower)
            {
                b = quietTower(why[com.oddlabs.tt.player.gauntlet.Retire.ANY_TOWER], 0);
                k = com.oddlabs.tt.player.gauntlet.Retire.ANY_TOWER;
            }
            if (b == null)
            {
                ai.aiLog().count("retire_none");
                if (ai.logging() && now >= next_none_log)
                {
                    next_none_log = (float)(now + 60.0f);
                    int pop = ai.owner().getUnitCountContainer().getNumSupplies();
                    ai.aiLog().log("RETIRE", new java.util.function.Supplier_L(() =>
                    {
                        return java.lang.JString.format("nothing to retire for %s (%d s at the cap): passed over (threat/other) sites %d/%d, towers %d/%d, armories %d/%d, quarters %d/%d; units %d", flagged.describe(), java.lang.JC.f2i(((float)(now - capped_since))), why[com.oddlabs.tt.player.gauntlet.Retire.SITE][0], why[com.oddlabs.tt.player.gauntlet.Retire.SITE][1], why[com.oddlabs.tt.player.gauntlet.Retire.TOWER][0], why[com.oddlabs.tt.player.gauntlet.Retire.TOWER][1], why[com.oddlabs.tt.player.gauntlet.Retire.ARMORY][0], why[com.oddlabs.tt.player.gauntlet.Retire.ARMORY][1], why[com.oddlabs.tt.player.gauntlet.Retire.QUARTERS][0], why[com.oddlabs.tt.player.gauntlet.Retire.QUARTERS][1], pop);
                    }));
                }
                return;
            }
            start(b, k, flagged, now);
        }
        // Java: 188
        public void trackSites(float now)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            site_seen.keySet().removeIf(new java.util.function.Predicate_L((object b_l) =>
            {
                com.oddlabs.tt.model.Building b = (com.oddlabs.tt.model.Building)b_l;
                return b.isDead() || b.isComplete();
            }));
            foreach (java.util.List group in java.lang.JIter.of(java.util.List_.of(intel.quarters_sites, intel.armory_sites, intel.tower_sites)))
            {
                foreach (com.oddlabs.tt.model.Building b_2 in java.lang.JIter.of(group))
                {
                    site_seen.putIfAbsent(b_2, now);
                }
            }
        }
        // Java: 197
        public com.oddlabs.tt.model.Building stalledSite(float now, int[] why)
        {
            com.oddlabs.tt.model.Building best = null;
            float oldest = java.lang.Float.MAX_VALUE;
            foreach (java.util.Map.Entry e in java.lang.JIter.of(site_seen.entrySet()))
            {
                com.oddlabs.tt.model.Building b = ((com.oddlabs.tt.model.Building)e.getKey());
                if (b.isDead() || (float)(now - ((float)((float?)e.getValue()))) < com.oddlabs.tt.player.gauntlet.Retire.SITE_AGE || passed.containsKey(b))
                {
                    continue;
                }
                if (economy.buildersOn(b) > 0)
                {
                    why[1]++;
                    continue;
                }
                if (ai.military().threatNear(b.getGridX(), b.getGridY(), 20))
                {
                    why[0]++;
                    continue;
                }
                if (((float)((float?)e.getValue())) >= oldest)
                {
                    continue;
                }
                if (!razersAt(b, 1))
                {
                    why[1]++;
                    continue;
                }
                oldest = ((float)((float?)e.getValue()));
                best = b;
            }
            return best;
        }
        // Java: 228
        public com.oddlabs.tt.model.Building quietTower(int[] why, int cover)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            int[] from = anchor();
            com.oddlabs.tt.model.Building best = null;
            int best_d = (-1);
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(intel.towers))
            {
                if (t.isDead() || passed.containsKey(t))
                {
                    continue;
                }
                bool covers = false;
                foreach (java.util.List group in java.lang.JIter.of(java.util.List_.of(intel.quarters, intel.armories)))
                {
                    foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(group))
                    {
                        covers |= cover > 0 && !b.isDead() && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(t.getGridX(), t.getGridY(), b.getGridX(), b.getGridY()) <= cover * cover;
                    }
                }
                if (covers)
                {
                    why[1]++;
                    continue;
                }
                if (ai.military().threatNear(t.getGridX(), t.getGridY(), 20))
                {
                    why[0]++;
                    continue;
                }
                int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(t.getGridX(), t.getGridY(), from[0], from[1]);
                if (d <= best_d)
                {
                    continue;
                }
                if (!razersAt(t, 4))
                {
                    why[1]++;
                    continue;
                }
                best_d = d;
                best = t;
            }
            return best;
        }
        // Java: 266
        public com.oddlabs.tt.model.Building drainedArmory(int[] why)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            com.oddlabs.tt.model.Building primary = intel.armory();
            if (primary == null)
            {
                return null;
            }
            com.oddlabs.tt.model.Building best = null;
            int best_d = (-1);
            foreach (com.oddlabs.tt.model.Building a in java.lang.JIter.of(intel.armories))
            {
                if (((object)a == (object)primary) || a.isDead() || passed.containsKey(a))
                {
                    continue;
                }
                if (!economy.isDrained(a))
                {
                    why[1]++;
                    continue;
                }
                if (ai.military().threatNear(a.getGridX(), a.getGridY(), 30))
                {
                    why[0]++;
                    continue;
                }
                int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(a.getGridX(), a.getGridY(), primary.getGridX(), primary.getGridY());
                if (d <= best_d)
                {
                    continue;
                }
                if (ai.military().lendable(a.getGridX(), a.getGridY(), com.oddlabs.tt.player.gauntlet.Retire.WARRIOR_CELLS) < 8)
                {
                    why[1]++;
                    continue;
                }
                best_d = d;
                best = a;
            }
            return best;
        }
        // Java: 304
        public com.oddlabs.tt.model.Building farQuarters(int[] why)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            com.oddlabs.tt.player.gauntlet.Strategy st = ai.strategy();
            com.oddlabs.tt.model.Building primary = intel.armory();
            if (primary == null || ai.owner().getUnitCountContainer().getNumSupplies() < st.retire_pop)
            {
                return null;
            }
            com.oddlabs.tt.model.Building best = null;
            int best_d = st.retire_quarters_dist * st.retire_quarters_dist;
            foreach (com.oddlabs.tt.model.Building q in java.lang.JIter.of(intel.quarters))
            {
                if (q.isDead() || passed.containsKey(q))
                {
                    continue;
                }
                int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(q.getGridX(), q.getGridY(), primary.getGridX(), primary.getGridY());
                if (d <= best_d)
                {
                    continue;
                }
                bool training = q.getChieftainContainer() != null && q.getChieftainContainer().isTraining();
                bool neighbour = false;
                foreach (com.oddlabs.tt.model.Building o in java.lang.JIter.of(intel.quarters))
                {
                    neighbour |= ((object)o != (object)q) && !o.isDead() && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(q.getGridX(), q.getGridY(), o.getGridX(), o.getGridY()) <= 25 * 25;
                }
                if (training || !neighbour)
                {
                    why[1]++;
                    continue;
                }
                if (ai.military().threatNear(q.getGridX(), q.getGridY(), 20))
                {
                    why[0]++;
                    continue;
                }
                if (!razersAt(q, 8))
                {
                    why[1]++;
                    continue;
                }
                best_d = d;
                best = q;
            }
            return best;
        }
        // Java: 342
        public int[] anchor()
        {
            com.oddlabs.tt.model.Building primary = ai.intel().armory();
            return (primary != null ? new int[] { primary.getGridX(), primary.getGridY() } : new int[] { ai.planner().getStartX(), ai.planner().getStartY() });
        }
        // Java: 347
        public void start(com.oddlabs.tt.model.Building b, int k, com.oddlabs.tt.player.gauntlet.Economy.Project flagged, float now)
        {
            target = b;
            kind = k;
            razing = false;
            started = now;
            lent = false;
            razers.clear();
            doomed.add(b);
            buildings_before = ai.owner().getBuildingCountContainer().getNumSupplies();
            int @in = com.oddlabs.tt.player.gauntlet.Retire.inside(b);
            ai.aiLog().count("retire_start_" + java.lang.JStr.valueOf(com.oddlabs.tt.player.gauntlet.Retire.KINDS[k]));
            ai.aiLog().log("RETIRE", new java.util.function.Supplier_L(() =>
            {
                return java.lang.JString.format("retiring the %s at %d,%d (hp %d, %d inside) for %s, %d s at the cap", com.oddlabs.tt.player.gauntlet.Retire.KINDS[k], b.getGridX(), b.getGridY(), b.getHitPoints(), @in, flagged.describe(), java.lang.JC.f2i(((float)(now - capped_since))));
            }));
            empty(b);
        }
        // Java: 369
        public bool lastOfItsKind(com.oddlabs.tt.model.Building b)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            if (kind == com.oddlabs.tt.player.gauntlet.Retire.ARMORY)
            {
                if (((object)b == (object)intel.armory()))
                {
                    return true;
                }
                foreach (com.oddlabs.tt.model.Building a in java.lang.JIter.of(intel.armories))
                {
                    if (((object)a != (object)b) && !a.isDead() && a.isComplete())
                    {
                        return false;
                    }
                }
                return true;
            }
            if (kind == com.oddlabs.tt.player.gauntlet.Retire.QUARTERS)
            {
                foreach (com.oddlabs.tt.model.Building q in java.lang.JIter.of(intel.quarters))
                {
                    if (((object)q != (object)b) && !q.isDead() && q.isComplete())
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
        // Java: 389
        public static int inside(com.oddlabs.tt.model.Building b)
        {
            return (b.isComplete() && b.getUnitContainer() != null ? b.getUnitContainer().getNumSupplies() : 0);
        }
        // Java: 394
        public void empty(com.oddlabs.tt.model.Building b)
        {
            int @in = com.oddlabs.tt.player.gauntlet.Retire.inside(b);
            if (@in == 0)
            {
                return;
            }
            if (kind == com.oddlabs.tt.player.gauntlet.Retire.TOWER || kind == com.oddlabs.tt.player.gauntlet.Retire.ANY_TOWER)
            {
                com.oddlabs.tt.model.Unit gunner = com.oddlabs.tt.player.gauntlet.Intel.gunner(b);
                if (gunner != null && !com.oddlabs.tt.player.gauntlet.Intel.isStunned(gunner))
                {
                    ai.owner().exitTower(b);
                }
                return;
            }
            int pending = b.getDeployContainer(com.oddlabs.tt.model.DeployType.PEON).getNumSupplies();
            if (@in - pending > 0)
            {
                ai.owner().deployUnits(b, com.oddlabs.tt.model.DeployType.PEON, @in - pending);
            }
        }
        // Java: 409
        public void follow(float now)
        {
            com.oddlabs.tt.model.Building b = target;
            if (b.isDead())
            {
                done(b, now);
                return;
            }
            if (economy.flaggedWaiting() == null || ai.owner().canBuild(com.oddlabs.tt.model.Race.BUILDING_ARMORY))
            {
                stop(b, "unneeded", now);
                return;
            }
            if (lastOfItsKind(b))
            {
                stop(b, "last", now);
                return;
            }
            if (ai.military().threatNear(b.getGridX(), b.getGridY(), (kind == com.oddlabs.tt.player.gauntlet.Retire.ARMORY ? 30 : 20)))
            {
                stop(b, "threat", now);
                return;
            }
            if (!razing)
            {
                if (com.oddlabs.tt.player.gauntlet.Retire.inside(b) > 0)
                {
                    empty(b);
                    bool tower = kind == com.oddlabs.tt.player.gauntlet.Retire.TOWER || kind == com.oddlabs.tt.player.gauntlet.Retire.ANY_TOWER;
                    float wait_v = (tower ? com.oddlabs.tt.player.gauntlet.Retire.EMPTY_WAIT_TOWER : com.oddlabs.tt.player.gauntlet.Retire.EMPTY_WAIT);
                    if ((float)(now - started) < wait_v)
                    {
                        return;
                    }
                    if (!tower)
                    {
                        stop(b, "giveup_empty", now);
                        return;
                    }
                    ai.aiLog().count("retire_gunner_lost");
                }
                if (!beginRaze(b, now) && (float)(now - started) > com.oddlabs.tt.player.gauntlet.Retire.UNITS_WAIT)
                {
                    stop(b, "nounits", now);
                }
                return;
            }
            if (kind == com.oddlabs.tt.player.gauntlet.Retire.ARMORY || kind == com.oddlabs.tt.player.gauntlet.Retire.QUARTERS)
            {
                empty(b);
            }
            razers.removeIf(new java.util.function.Predicate_L((object a0) => { return ((com.oddlabs.tt.model.Unit)a0).isDead(); }));
            java.util.List repairers = new java.util.ArrayList();
            foreach (java.util.Map.Entry e in java.lang.JIter.of(ai.intel().builder_sites.entrySet()))
            {
                if (((object)((com.oddlabs.tt.model.Building)e.getValue()) == (object)b) && !razers.contains(((com.oddlabs.tt.model.Unit)e.getKey())) && !((com.oddlabs.tt.model.Unit)e.getKey()).isDead())
                {
                    repairers.add(((com.oddlabs.tt.model.Unit)e.getKey()));
                }
            }
            if (!repairers.isEmpty())
            {
                razers.addAll(repairers);
                attack(repairers, b);
                for (int i = 0; i < repairers.size(); i++)
                {
                    ai.aiLog().count("retire_repairers");
                }
            }
            if (razers.isEmpty())
            {
                razing = false;
                return;
            }
            if ((float)(now - last_order) >= com.oddlabs.tt.player.gauntlet.Retire.REORDER)
            {
                last_order = now;
                java.util.List idle = new java.util.ArrayList();
                foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(razers))
                {
                    if (!u.isMounted() && !com.oddlabs.tt.player.gauntlet.Intel.isStunned(u) && !((u.getCurrentController() is com.oddlabs.tt.model.behaviour.HuntController hunt) && ((object)hunt.getTarget() == (object)b)))
                    {
                        idle.add(u);
                    }
                }
                if (!idle.isEmpty())
                {
                    attack(idle, b);
                    for (int i_2 = 0; i_2 < idle.size(); i_2++)
                    {
                        ai.aiLog().count("retire_reorder");
                    }
                }
            }
            bool tower_2 = ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID() == com.oddlabs.tt.model.Race.BUILDING_TOWER;
            float limit = (tower_2 ? 60.0f : (lent ? 90.0f : 180.0f));
            if ((float)(now - raze_started) > limit)
            {
                stop(b, "giveup_raze", now);
            }
        }
        // Java: 495
        public bool beginRaze(com.oddlabs.tt.model.Building b, float now)
        {
            int hp = b.getHitPoints();
            bool tower = ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID() == com.oddlabs.tt.model.Race.BUILDING_TOWER;
            java.util.List units = default;
            if (tower)
            {
                int n = java.lang.Math.clamp(java.lang.JC.d2i(java.lang.Math.ceil((float)(hp / 12.5f))), (b.isComplete() ? 4 : 2), 8);
                units = peons(b, n, java.lang.Math.min(n, (b.isComplete() ? 4 : 1)));
            }
            else
            {
                int n_2 = java.lang.Math.clamp(java.lang.JC.d2i(java.lang.Math.ceil((float)(hp / 20.0f))), 2, 12);
                units = ai.military().lend(b.getGridX(), b.getGridY(), n_2, java.lang.Math.min(n_2, 8), com.oddlabs.tt.player.gauntlet.Retire.WARRIOR_CELLS, 240.0f);
                lent = !units.isEmpty();
                if (!lent && kind != com.oddlabs.tt.player.gauntlet.Retire.ARMORY)
                {
                    int np = java.lang.Math.clamp(java.lang.JC.d2i(java.lang.Math.ceil((float)(hp / 10.0f))), 2, 20);
                    units = peons(b, np, java.lang.Math.min(np, 8));
                }
            }
            if (units.isEmpty())
            {
                return false;
            }
            razers.addAll(units);
            razing = true;
            raze_started = now;
            last_order = now;
            attack(units, b);
            ai.aiLog().count((lent ? "retire_raze_warriors" : "retire_raze_peons"));
            if (ai.logging())
            {
                int n_3 = units.size();
                float walk = 0.0f;
                foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(units))
                {
                    walk = (float)(walk + ((float)(java.lang.Math.sqrt(com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(u.getGridX(), u.getGridY(), b.getGridX(), b.getGridY())))));
                }
                float mean = (float)(walk / n_3);
                ai.aiLog().log("RETIRE", new java.util.function.Supplier_L(() =>
                {
                    return java.lang.JString.format("razing the %s at %d,%d (hp %d) with %d %s, %.0f cells away", com.oddlabs.tt.player.gauntlet.Retire.KINDS[kind], b.getGridX(), b.getGridY(), hp, n_3, (lent ? (object)("lent warriors") : (object)("peons")), mean);
                }));
            }
            return true;
        }
        // Java: 537
        public java.util.List peons(com.oddlabs.tt.model.Building b, int n, int min)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            int bx = b.getGridX();
            int by = b.getGridY();
            java.util.List candidates = new java.util.ArrayList();
            java.util.List dists = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit p in java.lang.JIter.of(intel.peons))
            {
                if (!razerPeon(p))
                {
                    continue;
                }
                int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(bx, by, p.getGridX(), p.getGridY());
                if (d > com.oddlabs.tt.player.gauntlet.Retire.UNIT_CELLS * com.oddlabs.tt.player.gauntlet.Retire.UNIT_CELLS)
                {
                    continue;
                }
                int i = 0;
                while (i < dists.size() && ((int)((int?)dists.get(i))) <= d)
                {
                    i++;
                }
                candidates.add(i, p);
                dists.add(i, d);
            }
            if (candidates.size() >= min)
            {
                return new java.util.ArrayList(candidates.subList(0, java.lang.Math.min(n, candidates.size())));
            }
            float now = ai.time();
            if (now >= next_deploy)
            {
                com.oddlabs.tt.model.Building from = null;
                int best = java.lang.Integer.MAX_VALUE;
                foreach (com.oddlabs.tt.model.Building a in java.lang.JIter.of(intel.armories))
                {
                    if (a.isDead() || isDoomed(a) || a.getUnitContainer().getNumSupplies() < n + 4)
                    {
                        continue;
                    }
                    int d_2 = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(bx, by, a.getGridX(), a.getGridY());
                    if (d_2 < best && d_2 <= com.oddlabs.tt.player.gauntlet.Retire.UNIT_CELLS * com.oddlabs.tt.player.gauntlet.Retire.UNIT_CELLS)
                    {
                        best = d_2;
                        from = a;
                    }
                }
                if (from != null)
                {
                    next_deploy = (float)(now + 10.0f);
                    ai.owner().deployUnits(from, com.oddlabs.tt.model.DeployType.PEON, n - candidates.size());
                    ai.aiLog().count("retire_deploy");
                }
            }
            return java.util.List_.of();
        }
        // Java: 583
        public void attack(java.util.List units, com.oddlabs.tt.model.Building b)
        {
            ai.owner().setTarget(((com.oddlabs.tt.model.Selectable[])units.toArray(new com.oddlabs.tt.model.Selectable[0])), b, com.oddlabs.tt.model.Action.ATTACK, false);
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(units))
            {
                if (intel.peon_states.containsKey(u))
                {
                    intel.peon_states.put(u, com.oddlabs.tt.player.gauntlet.Intel.PeonState.FIGHT);
                }
            }
        }
        public static java.util.EnumSet RAZER_STATES;
        // Java: 596
        public bool razerPeon(com.oddlabs.tt.model.Unit p)
        {
            com.oddlabs.tt.player.gauntlet.Intel.PeonState s = ((com.oddlabs.tt.player.gauntlet.Intel.PeonState)ai.intel().peon_states.get(p));
            return !p.isDead() && s != null && com.oddlabs.tt.player.gauntlet.Retire.RAZER_STATES.contains(s) && !economy.reservedPlacer(p);
        }
        // Java: 606
        public bool razersAt(com.oddlabs.tt.model.Building b, int min)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            int n = 0;
            foreach (com.oddlabs.tt.model.Unit p in java.lang.JIter.of(intel.peons))
            {
                if (razerPeon(p) && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(b.getGridX(), b.getGridY(), p.getGridX(), p.getGridY()) <= com.oddlabs.tt.player.gauntlet.Retire.UNIT_CELLS * com.oddlabs.tt.player.gauntlet.Retire.UNIT_CELLS && ++n >= min)
                {
                    return true;
                }
            }
            foreach (com.oddlabs.tt.model.Building a in java.lang.JIter.of(intel.armories))
            {
                if (!a.isDead() && !isDoomed(a) && a.getUnitContainer().getNumSupplies() >= min + 4 && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(b.getGridX(), b.getGridY(), a.getGridX(), a.getGridY()) <= com.oddlabs.tt.player.gauntlet.Retire.UNIT_CELLS * com.oddlabs.tt.player.gauntlet.Retire.UNIT_CELLS)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 622
        public void done(com.oddlabs.tt.model.Building b, float now)
        {
            ai.aiLog().count("retire_done_" + java.lang.JStr.valueOf(com.oddlabs.tt.player.gauntlet.Retire.KINDS[kind]));
            retired.add(new float[] { b.getGridX(), b.getGridY(), now });
            last_done = now;
            next_allowed = (float)(started + com.oddlabs.tt.player.gauntlet.Retire.GAP);
            int after = ai.owner().getBuildingCountContainer().getNumSupplies();
            float order = (razing ? (float)(now - raze_started) : (float)(-1.0f));
            int n = razers.size();
            ai.aiLog().log("RETIRE", new java.util.function.Supplier_L(() =>
            {
                return java.lang.JString.format("retired the %s at %d,%d: %.0f s after it was chosen, %.0f s razing, %d %s left, buildings %d -> %d", com.oddlabs.tt.player.gauntlet.Retire.KINDS[kind], b.getGridX(), b.getGridY(), (float)(now - started), order, n, (lent ? (object)("lent warriors") : (object)("peons")), buildings_before, after);
            }));
            release(false);
        }
        // Java: 638
        public void stop(com.oddlabs.tt.model.Building b, string why, float now)
        {
            if (java.lang.JO.equals(why, "nounits"))
            {
                passed.put(b, (float)(now + com.oddlabs.tt.player.gauntlet.Retire.PASS_OVER));
            }
            ai.aiLog().count("retire_stop_" + java.lang.JStr.valueOf(why));
            next_allowed = (float)(now + com.oddlabs.tt.player.gauntlet.Retire.RETRY);
            int hp = b.getHitPoints();
            ai.aiLog().log("RETIRE", new java.util.function.Supplier_L(() =>
            {
                return java.lang.JString.format("the %s at %d,%d stays (%s): hp %d, %.0f s after it was chosen", com.oddlabs.tt.player.gauntlet.Retire.KINDS[kind], b.getGridX(), b.getGridY(), why, hp, (float)(now - started));
            }));
            release(true);
        }
        // Java: 654
        public void release(bool stand)
        {
            com.oddlabs.tt.model.Building b = target;
            java.util.List busy = new java.util.ArrayList();
            if (stand && b != null)
            {
                foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(razers))
                {
                    if (!u.isDead() && !u.isMounted() && (u.getCurrentController() is com.oddlabs.tt.model.behaviour.HuntController hunt) && ((object)hunt.getTarget() == (object)b))
                    {
                        busy.add(u);
                    }
                }
            }
            if (!busy.isEmpty())
            {
                com.oddlabs.tt.model.Building home = ai.intel().armory();
                if (lent || home == null)
                {
                    foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(busy))
                    {
                        ai.landscapeOrder(com.oddlabs.tt.model.Selectable.newArray(u_2), u_2.getGridX(), u_2.getGridY(), com.oddlabs.tt.model.Action.MOVE, false);
                    }
                }
                else
                {
                    ai.owner().setTarget(((com.oddlabs.tt.model.Selectable[])busy.toArray(new com.oddlabs.tt.model.Selectable[0])), home, com.oddlabs.tt.model.Action.DEFAULT, false);
                }
            }
            if (lent)
            {
                ai.military().release(razers);
            }
            razers.clear();
            if (b != null)
            {
                doomed.remove(b);
            }
            target = null;
            razing = false;
            lent = false;
        }
        static Retire()
        {
            Retire.KINDS = new string[] { "site", "tower", "armory", "quarters", "anytower" };
            Retire.RAZER_STATES = java.util.EnumSet.of(com.oddlabs.tt.player.gauntlet.Intel.PeonState.IDLE, com.oddlabs.tt.player.gauntlet.Intel.PeonState.MOVE, com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_TREE, com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_IRON, com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_ROCK, com.oddlabs.tt.player.gauntlet.Intel.PeonState.TRANSIT);
        }
        private void init_Retire()
        {
            this.doomed = new java.util.LinkedHashSet();
            this.site_seen = new java.util.LinkedHashMap();
            this.retired = new java.util.ArrayList();
            this.passed = new java.util.LinkedHashMap();
            this.razers = new java.util.ArrayList();
            this.next_deploy = (float)(-1.0f);
            this.capped_since = (float)(-1.0f);
            this.next_allowed = (float)(-1.0f);
            this.next_search = (float)(-1.0f);
            this.last_done = (float)(-1000.0f);
        }
        public Retire(com.oddlabs.tt.player.gauntlet.GauntletAI ai, com.oddlabs.tt.player.gauntlet.Economy economy) : base()
        {
            init_Retire();
            this.ai = ai;
            this.economy = economy;
        }
    }
}
