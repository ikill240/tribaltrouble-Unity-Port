// Translated from Construction.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.fable
{
    // Java: 25
    public class Construction : java.lang.Object
    {
        // Java: 26
        public sealed class State : java.lang.Enum
        {
            public static State[] values() { return (State[])VALUES_.Clone(); }
            public static State valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly State SCOUTING = new State("SCOUTING", 0);
            public static readonly State PLACED = new State("PLACED", 1);
            public static readonly State DONE = new State("DONE", 2);
            public static readonly State FAILED = new State("FAILED", 3);
            public static readonly State[] VALUES_ = new State[] { SCOUTING, PLACED, DONE, FAILED };
            public State(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        // Java: 33
        public class Job : java.lang.Object
        {
            public int template_id;
            public int gx;
            public int gy;
            public string purpose;
            public int crew_target;
            public com.oddlabs.tt.player.fable.Construction.State state;
            public com.oddlabs.tt.model.Building building;
            public com.oddlabs.tt.model.Unit scout;
            public float started_at;
            public float placed_at;
            public int last_hp;
            public float last_progress_at;
            public float last_progress_log;
            public float last_scout_order;
            public int failures;
            public int illegal_checks;
            public java.util.List crew;
            // Java: 62
            public bool active()
            {
                return ((object)state == (object)com.oddlabs.tt.player.fable.Construction.State.SCOUTING) || ((object)state == (object)com.oddlabs.tt.player.fable.Construction.State.PLACED);
            }
            // Java: 66
            public com.oddlabs.tt.model.LandBuilding placedBuilding()
            {
                return ((building is com.oddlabs.tt.model.LandBuilding b) && !b.isDead() && b.isPlaced() ? b : (com.oddlabs.tt.model.LandBuilding)(null));
            }
            // Java: 70
            public bool isPlaced()
            {
                return building != null && !building.isDead() && building.isPlaced();
            }
            // Java: 74
            public bool isComplete()
            {
                return building != null && !building.isDead() && building.isPlaced() && building.isComplete();
            }
            // Java: 78
            public override string toString()
            {
                return java.lang.JStr.valueOf(purpose) + "@" + (gx) + "," + (gy) + " " + java.lang.JStr.valueOf(state) + " crew=" + (crew.size()) + "/" + (crew_target);
            }
            private void init_Job()
            {
                this.state = com.oddlabs.tt.player.fable.Construction.State.SCOUTING;
                this.last_progress_log = (float)(-100.0f);
                this.crew = new java.util.ArrayList();
            }
            public Job(int template_id, int gx, int gy, string purpose, int crew_target, float now) : base()
            {
                init_Job();
                this.template_id = template_id;
                this.gx = gx;
                this.gy = gy;
                this.purpose = purpose;
                this.crew_target = crew_target;
                this.started_at = now;
            }
        }
        public com.oddlabs.tt.player.fable.Orders orders;
        public com.oddlabs.tt.player.fable.Jobs jobs;
        public com.oddlabs.tt.player.fable.SiteFinder sites;
        public com.oddlabs.tt.player.fable.Params @params;
        public com.oddlabs.tt.player.fable.AiLog log;
        public java.util.List active_f;
        public java.util.List finished;
        // Java: 101
        public java.util.List active()
        {
            return active_f;
        }
        // Java: 105
        public bool hasActive(int template_id)
        {
            foreach (com.oddlabs.tt.player.fable.Construction.Job j in java.lang.JIter.of(active_f))
            {
                if (j.template_id == template_id && j.active())
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 112
        public int countActive(int template_id)
        {
            int n = 0;
            foreach (com.oddlabs.tt.player.fable.Construction.Job j in java.lang.JIter.of(active_f))
            {
                if (j.template_id == template_id && j.active())
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 120
        public com.oddlabs.tt.player.fable.Construction.Job activeJob(int template_id)
        {
            foreach (com.oddlabs.tt.player.fable.Construction.Job j in java.lang.JIter.of(active_f))
            {
                if (j.template_id == template_id && j.active())
                {
                    return j;
                }
            }
            return null;
        }
        // Java: 128
        public bool isTracked(com.oddlabs.tt.model.Building b)
        {
            foreach (com.oddlabs.tt.player.fable.Construction.Job j in java.lang.JIter.of(active_f))
            {
                if (j.active() && ((object)j.building == (object)b))
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 136
        public java.util.List reservedCells()
        {
            java.util.List result = new java.util.ArrayList();
            foreach (com.oddlabs.tt.player.fable.Construction.Job j in java.lang.JIter.of(active_f))
            {
                if (j.active())
                {
                    result.add(new int[] { j.gx, j.gy });
                }
            }
            return result;
        }
        // Java: 145
        public int assignedPeons()
        {
            int n = 0;
            foreach (com.oddlabs.tt.player.fable.Construction.Job j in java.lang.JIter.of(active_f))
            {
                if (!j.active())
                {
                    continue;
                }
                if (j.scout != null && orders.usable(j.scout))
                {
                    n++;
                }
                foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(j.crew))
                {
                    if (orders.usable(u) && ((object)u != (object)j.scout))
                    {
                        n++;
                    }
                }
            }
            return n;
        }
        // Java: 163
        public com.oddlabs.tt.player.fable.Construction.Job start(int template_id, int gx, int gy, string purpose, com.oddlabs.tt.model.Unit scout, java.util.List helpers, int crew_target, float now)
        {
            if (!sites.isLegal(template_id, gx, gy))
            {
                return null;
            }
            com.oddlabs.tt.player.fable.Construction.Job job = new com.oddlabs.tt.player.fable.Construction.Job(template_id, gx, gy, purpose, crew_target, now);
            com.oddlabs.tt.model.Building b = orders.placeBuilding(template_id, gx, gy, java.util.List_.of(scout));
            if (b == null)
            {
                return null;
            }
            job.building = b;
            job.scout = scout;
            job.last_scout_order = now;
            jobs.set(scout, com.oddlabs.tt.player.fable.Jobs.Kind.BUILD, gx, gy, b, null, now, template_id);
            foreach (com.oddlabs.tt.model.Unit h in java.lang.JIter.of(helpers))
            {
                if (((object)h != (object)scout) && orders.usable(h))
                {
                    job.crew.add(h);
                    jobs.set(h, com.oddlabs.tt.player.fable.Jobs.Kind.BUILD, gx, gy, b, null, now, template_id);
                }
            }
            active_f.add(job);
            log.info(new java.util.function.Supplier_L(() =>
            {
                return "construction start " + java.lang.JStr.valueOf(job) + " scout " + (scout.getGridX()) + "," + (scout.getGridY());
            }));
            return job;
        }
        // Java: 188
        public void trimCrew(com.oddlabs.tt.player.fable.Construction.Job job, int keep)
        {
            int have = job.crew.size();
            if (have <= keep)
            {
                return;
            }
            java.util.List release = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(job.crew))
            {
                if (!com.oddlabs.tt.player.fable.Construction.isWorkingOn(u, job.building))
                {
                    release.add(u);
                }
            }
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(job.crew))
            {
                if (release.size() < have - keep && !release.contains(u_2))
                {
                    release.add(u_2);
                }
            }
            int n = 0;
            foreach (com.oddlabs.tt.model.Unit u_3 in java.lang.JIter.of(release))
            {
                if (n >= have - keep)
                {
                    break;
                }
                job.crew.remove(u_3);
                jobs.clear(u_3);
                if (orders.usable(u_3))
                {
                    orders.move(u_3, u_3.getGridX(), u_3.getGridY());
                }
                n++;
            }
        }
        // Java: 211
        public void addHelper(com.oddlabs.tt.player.fable.Construction.Job job, com.oddlabs.tt.model.Unit peon, float now)
        {
            if (!orders.usable(peon) || job.crew.contains(peon) || ((object)peon == (object)job.scout))
            {
                return;
            }
            job.crew.add(peon);
            jobs.set(peon, com.oddlabs.tt.player.fable.Jobs.Kind.BUILD, job.gx, job.gy, job.building, null, now, job.template_id);
            if (job.isPlaced())
            {
                orders.build(peon, job.building);
            }
            else
            {
                preposition(peon, job);
            }
        }
        // Java: 224
        public void preposition(com.oddlabs.tt.model.Unit peon, com.oddlabs.tt.player.fable.Construction.Job job)
        {
            int dx = job.gx - peon.getGridX();
            int dy = job.gy - peon.getGridY();
            float len = ((float)(java.lang.Math.sqrt(dx * dx + dy * dy)));
            if (len <= 9.0f)
            {
                return;
            }
            int tx = java.lang.Math.round((float)(job.gx - (float)((float)(dx / len) * 8.0f)));
            int ty = java.lang.Math.round((float)(job.gy - (float)((float)(dy / len) * 8.0f)));
            orders.move(peon, tx, ty);
        }
        // Java: 240
        public java.util.List tick(float now, com.oddlabs.tt.player.fable.Roster roster)
        {
            java.util.List done = new java.util.ArrayList();
            java.util.Iterator it = active_f.iterator();
            while (it.hasNext())
            {
                com.oddlabs.tt.player.fable.Construction.Job job = ((com.oddlabs.tt.player.fable.Construction.Job)it.next());
                if (!job.active())
                {
                    it.remove();
                    continue;
                }
                step(job, now, roster);
                if (((object)job.state == (object)com.oddlabs.tt.player.fable.Construction.State.DONE))
                {
                    done.add(job);
                    finished.add(job);
                    it.remove();
                }
                else if (((object)job.state == (object)com.oddlabs.tt.player.fable.Construction.State.FAILED))
                {
                    done.add(job);
                    finished.add(job);
                    it.remove();
                }
            }
            return done;
        }
        // Java: 264
        public void step(com.oddlabs.tt.player.fable.Construction.Job job, float now, com.oddlabs.tt.player.fable.Roster roster)
        {
            com.oddlabs.tt.model.Building b = job.building;
            if (b != null && b.isDead())
            {
                fail(job, "destroyed");
                return;
            }
            if (job.isComplete())
            {
                job.state = com.oddlabs.tt.player.fable.Construction.State.DONE;
                releaseCrew(job);
                log.info(new java.util.function.Supplier_L(() =>
                {
                    return "construction done " + java.lang.JStr.valueOf(job);
                }));
                return;
            }
            if (((object)job.state == (object)com.oddlabs.tt.player.fable.Construction.State.SCOUTING))
            {
                if (job.isPlaced())
                {
                    job.state = com.oddlabs.tt.player.fable.Construction.State.PLACED;
                    job.placed_at = now;
                    log.info(new java.util.function.Supplier_L(() =>
                    {
                        return "construction placed " + java.lang.JStr.valueOf(job);
                    }));
                    foreach (com.oddlabs.tt.model.Unit h in java.lang.JIter.of(job.crew))
                    {
                        if (orders.usable(h))
                        {
                            orders.build(h, b);
                        }
                    }
                    return;
                }
                if ((float)(now - job.started_at) > (float)(@params.site_fail_timeout * 2.0f))
                {
                    fail(job, "scout never placed the foundation");
                    return;
                }
                com.oddlabs.tt.model.Unit scout = job.scout;
                bool scout_ok = scout != null && orders.usable(scout);
                bool scout_busy = scout_ok && (scout.getPrimaryController() is com.oddlabs.tt.model.behaviour.PlaceBuildingController pc) && ((object)pc.getBuilding() == (object)b);
                if ((float)(now - job.last_progress_log) >= 30.0f)
                {
                    job.last_progress_log = now;
                    com.oddlabs.tt.model.Unit fs = scout;
                    log.info(new java.util.function.Supplier_L(() =>
                    {
                        return "construction scouting " + java.lang.JStr.valueOf(job) + " scout " + java.lang.JStr.valueOf(((fs == null || fs.isDead() ? "none" : "at " + (fs.getGridX()) + "," + (fs.getGridY()) + " dist=" + (java.lang.JC.d2i(java.lang.StrictMath.hypot(fs.getGridX() - job.gx, fs.getGridY() - job.gy))) + " ctrl=" + java.lang.JStr.valueOf(((fs.getPrimaryController() == null ? "none" : fs.getPrimaryController().GetType().getSimpleName()))))));
                    }));
                }
                if (!scout_ok || (!scout_busy && (float)(now - job.last_scout_order) > 3.0f))
                {
                    job.failures++;
                    if (job.failures > @params.site_retries || (float)(now - job.started_at) > @params.site_fail_timeout)
                    {
                        fail(job, (scout_ok ? "placement rejected " + (job.failures) + " times" : "scout lost"));
                        return;
                    }
                    if (!sites.isLegal(job.template_id, job.gx, job.gy))
                    {
                        job.illegal_checks++;
                        if (job.illegal_checks >= 4)
                        {
                            fail(job, "site illegal");
                            return;
                        }
                        job.last_scout_order = now;
                        return;
                    }
                    com.oddlabs.tt.model.Unit next = (scout_ok ? scout : nearestPeon(roster, job, null));
                    if (next == null)
                    {
                        return;
                    }
                    com.oddlabs.tt.model.Building fresh = orders.placeBuilding(job.template_id, job.gx, job.gy, java.util.List_.of(next));
                    if (fresh == null)
                    {
                        if (job.failures > @params.site_retries - 1)
                        {
                            fail(job, "cannot place");
                        }
                        return;
                    }
                    job.building = fresh;
                    job.scout = next;
                    job.last_scout_order = now;
                    job.crew.remove(next);
                    jobs.set(next, com.oddlabs.tt.player.fable.Jobs.Kind.BUILD, job.gx, job.gy, fresh, null, now, job.template_id);
                    foreach (com.oddlabs.tt.model.Unit h_2 in java.lang.JIter.of(job.crew))
                    {
                        if (orders.usable(h_2))
                        {
                            jobs.set(h_2, com.oddlabs.tt.player.fable.Jobs.Kind.BUILD, job.gx, job.gy, fresh, null, now, job.template_id);
                        }
                    }
                    log.info(new java.util.function.Supplier_L(() =>
                    {
                        return "construction retry " + java.lang.JStr.valueOf(job);
                    }));
                }
                return;
            }
            int hp = b.getHitPoints();
            if (hp > job.last_hp)
            {
                job.last_hp = hp;
                job.last_progress_at = now;
            }
            float stalled = (float)(now - java.lang.Math.max(job.placed_at, job.last_progress_at));
            int working = 0;
            java.util.Iterator it = job.crew.iterator();
            while (it.hasNext())
            {
                com.oddlabs.tt.model.Unit u = ((com.oddlabs.tt.model.Unit)it.next());
                if (!orders.usable(u))
                {
                    it.remove();
                    continue;
                }
                if (com.oddlabs.tt.player.fable.Construction.isWorkingOn(u, b))
                {
                    working++;
                }
                else if (((object)com.oddlabs.tt.player.fable.Roster.activity(u) == (object)com.oddlabs.tt.player.fable.Roster.Activity.IDLE) || ((object)com.oddlabs.tt.player.fable.Roster.activity(u) == (object)com.oddlabs.tt.player.fable.Roster.Activity.WALK))
                {
                    orders.build(u, b);
                    jobs.set(u, com.oddlabs.tt.player.fable.Jobs.Kind.BUILD, job.gx, job.gy, b, null, now, job.template_id);
                }
            }
            if (job.scout != null && orders.usable(job.scout) && com.oddlabs.tt.player.fable.Construction.isWorkingOn(job.scout, b))
            {
                working++;
            }
            job.crew_target = java.lang.Math.max(job.crew_target, 1);
            bool stuck_with_crew = working >= 3 && stalled > @params.site_stall_timeout;
            if (stuck_with_crew || stalled > (float)(@params.site_fail_timeout * 2.0f) || (float)(now - job.placed_at) > (float)(@params.site_fail_timeout * 8.0f))
            {
                fail(job, "build timeout (" + (java.lang.JC.f2i(stalled)) + " s without progress, " + (working) + " working)");
                return;
            }
            if ((float)(now - job.last_progress_log) >= 30.0f)
            {
                job.last_progress_log = now;
                int fw = working;
                int fhp = hp;
                log.info(new java.util.function.Supplier_L(() =>
                {
                    return "construction progress " + java.lang.JStr.valueOf(job) + " hp=" + (fhp) + "/" + (((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getMaxHitPoints()) + " working=" + (fw);
                }));
            }
        }
        // Java: 388
        public static bool isWorkingOn(com.oddlabs.tt.model.Unit u, com.oddlabs.tt.model.Building b)
        {
            if (b == null || u.isDead())
            {
                return false;
            }
            com.oddlabs.tt.model.behaviour.Controller c = u.getPrimaryController();
            return ((c is com.oddlabs.tt.model.behaviour.RepairController r) && ((object)r.getBuilding() == (object)b)) || ((c is com.oddlabs.tt.model.behaviour.PlaceBuildingController p) && ((object)p.getBuilding() == (object)b));
        }
        // Java: 397
        public int workers(com.oddlabs.tt.player.fable.Construction.Job job)
        {
            int n = 0;
            if (job.scout != null && orders.usable(job.scout) && com.oddlabs.tt.player.fable.Construction.isWorkingOn(job.scout, job.building))
            {
                n++;
            }
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(job.crew))
            {
                if (orders.usable(u) && ((object)u != (object)job.scout) && com.oddlabs.tt.player.fable.Construction.isWorkingOn(u, job.building))
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 407
        public void fail(com.oddlabs.tt.player.fable.Construction.Job job, string why)
        {
            job.state = com.oddlabs.tt.player.fable.Construction.State.FAILED;
            releaseCrew(job);
            java.util.List all = new java.util.ArrayList(job.crew);
            if (job.scout != null)
            {
                all.add(job.scout);
            }
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(all))
            {
                if (orders.usable(u))
                {
                    orders.move(u, u.getGridX(), u.getGridY());
                }
            }
            log.info(new java.util.function.Supplier_L(() =>
            {
                return "construction FAILED " + java.lang.JStr.valueOf(job) + ": " + java.lang.JStr.valueOf(why);
            }));
        }
        // Java: 421
        public void releaseCrew(com.oddlabs.tt.player.fable.Construction.Job job)
        {
            if (job.scout != null)
            {
                jobs.clear(job.scout);
            }
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(job.crew))
            {
                jobs.clear(u);
            }
        }
        // Java: 428
        public com.oddlabs.tt.model.Unit nearestPeon(com.oddlabs.tt.player.fable.Roster roster, com.oddlabs.tt.player.fable.Construction.Job job, com.oddlabs.tt.model.Unit exclude)
        {
            com.oddlabs.tt.model.Unit best = null;
            int best_d2 = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(job.crew))
            {
                if (((object)u == (object)exclude) || !orders.usable(u))
                {
                    continue;
                }
                int d2 = com.oddlabs.tt.player.fable.BasePlan.dist2(u.getGridX(), u.getGridY(), job.gx, job.gy);
                if (d2 < best_d2)
                {
                    best_d2 = d2;
                    best = u;
                }
            }
            if (best != null)
            {
                return best;
            }
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(roster.peons))
            {
                if (((object)u_2 == (object)exclude) || !orders.usable(u_2) || ((object)jobs.kindOf(u_2) == (object)com.oddlabs.tt.player.fable.Jobs.Kind.BUILD))
                {
                    continue;
                }
                int d2_2 = com.oddlabs.tt.player.fable.BasePlan.dist2(u_2.getGridX(), u_2.getGridY(), job.gx, job.gy);
                if (d2_2 < best_d2)
                {
                    best_d2 = d2_2;
                    best = u_2;
                }
            }
            return best;
        }
        // Java: 454
        public static string name(int template_id)
        {
            return ((template_id) switch { 0 => (string)("quarters"), 1 => (string)("armory"), 2 => (string)("tower"), _ => (string)("building") });
        }
        private void init_Construction()
        {
            this.active_f = new java.util.ArrayList();
            this.finished = new java.util.ArrayList();
        }
        public Construction(com.oddlabs.tt.player.fable.Orders orders, com.oddlabs.tt.player.fable.Jobs jobs, com.oddlabs.tt.player.fable.SiteFinder sites, com.oddlabs.tt.player.fable.Params @params, com.oddlabs.tt.player.fable.AiLog log) : base()
        {
            init_Construction();
            this.orders = orders;
            this.jobs = jobs;
            this.sites = sites;
            this.@params = @params;
            this.log = log;
        }
    }
}
