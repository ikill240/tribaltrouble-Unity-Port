// Translated from BuildPlanner.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.fable
{
    // Java: 17
    public class BuildPlanner : java.lang.Object
    {
        public com.oddlabs.tt.player.fable.Orders orders;
        public com.oddlabs.tt.player.fable.Jobs jobs;
        public com.oddlabs.tt.player.fable.Construction construction;
        public com.oddlabs.tt.player.fable.BasePlan plan;
        public com.oddlabs.tt.player.fable.Economy economy;
        public com.oddlabs.tt.player.fable.Params @params;
        public com.oddlabs.tt.player.fable.AiLog log;
        public bool opening_started;
        public bool armory_ever_complete;
        public float last_tower_started;
        public float last_quarters_started;
        public float last_site_failure;
        public int quarters_built;
        public int towers_built;
        public com.oddlabs.tt.player.fable.Construction.Job q1_job;
        public com.oddlabs.tt.player.fable.Construction.Job q2_job;
        public com.oddlabs.tt.player.fable.Construction.Job armory_job;
        // Java: 48
        public void tick(float now, com.oddlabs.tt.player.fable.Roster roster, com.oddlabs.tt.player.fable.Intel intel, com.oddlabs.tt.player.fable.Threat threat, com.oddlabs.tt.player.fable.Military military)
        {
            java.util.List done = construction.tick(now, roster);
            foreach (com.oddlabs.tt.player.fable.Construction.Job job in java.lang.JIter.of(done))
            {
                if (((object)job.state == (object)com.oddlabs.tt.player.fable.Construction.State.FAILED))
                {
                    failed_sites.add(new int[] { job.gx, job.gy, java.lang.JC.f2i(now) });
                    if (java.lang.JO.equals(job.purpose, "A2"))
                    {
                        expand_blocked_until = (float)(now + @params.expand_retry_after);
                    }
                    continue;
                }
                onDone(job, now, roster);
            }
            failed_sites.removeIf(new java.util.function.Predicate_L((object f_l) =>
            {
                int[] f = (int[])f_l;
                return (float)(now - f[2]) > 150.0f;
            }));
            if (!opening_started)
            {
                opening(now, roster);
                return;
            }
            com.oddlabs.tt.model.LandBuilding armory = roster.mainArmory();
            bool armory_pending = construction.hasActive(com.oddlabs.tt.model.Race.BUILDING_ARMORY);
            if (armory == null && !armory_pending)
            {
                rebuildArmory(now, roster, intel);
                return;
            }
            if (armory == null)
            {
                com.oddlabs.tt.player.fable.Construction.Job aj = construction.activeJob(com.oddlabs.tt.model.Race.BUILDING_ARMORY);
                economy.opening_site = (aj == null ? (int[])(null) : new int[] { aj.gx, aj.gy });
                keepOpeningCrews(now, roster);
                return;
            }
            economy.opening_site = null;
            armory_ever_complete = true;
            foreach (com.oddlabs.tt.player.fable.Construction.Job job_2 in java.lang.JIter.of(construction.active()))
            {
                if (job_2.template_id == com.oddlabs.tt.model.Race.BUILDING_QUARTERS && job_2.crew_target > @params.crew_quarters + 4)
                {
                    job_2.crew_target = @params.crew_quarters + 4;
                }
            }
            keepCrewsStaffed(now, roster);
            bool emergency = economy.demands.emergency;
            if (emergency)
            {
                return;
            }
            int peons = roster.totalPeons();
            int active_jobs = construction.active().size();
            int max_jobs = (peons >= 50 ? 2 : 1);
            if (active_jobs >= max_jobs)
            {
                return;
            }
            if ((float)(now - last_site_failure) < 8.0f)
            {
                return;
            }
            if ((((object)threat.level == (object)com.oddlabs.tt.player.fable.Threat.Level.ALERT) || ((object)threat.level == (object)com.oddlabs.tt.player.fable.Threat.Level.ENGAGED)) && roster.towers.size() >= 2 && threat.approach_value >= 6.0f)
            {
                return;
            }
            if (maybeExpand(now, roster, intel, threat, military, armory, active_jobs))
            {
                return;
            }
            int quarters_target = java.lang.Math.clamp(java.lang.JC.d2i(java.lang.Math.ceil((float)(economy.rate_star / 0.131f))) + 1, @params.quarters_min, @params.quarters_max);
            if (now < 300.0f)
            {
                quarters_target = java.lang.Math.min(quarters_target, @params.opening_quarters);
            }
            if (roster.unitCount() >= roster.unitCap() - 20)
            {
                quarters_target = java.lang.Math.min(quarters_target, roster.quarters.size());
            }
            int quarters_now = roster.quarters.size() + construction.countActive(com.oddlabs.tt.model.Race.BUILDING_QUARTERS) + countUnderConstruction(roster, com.oddlabs.tt.model.Race.BUILDING_QUARTERS);
            int towers_now = roster.towers.size() + construction.countActive(com.oddlabs.tt.model.Race.BUILDING_TOWER) + countUnderConstruction(roster, com.oddlabs.tt.model.Race.BUILDING_TOWER);
            int towers_target = towersWanted(now, roster, threat);
            bool can_build_more = roster.buildingCount() + active_jobs < roster.buildingCap();
            if (!can_build_more)
            {
                return;
            }
            bool want_quarters = quarters_now < quarters_target && (float)(now - last_quarters_started) > 20.0f;
            bool want_tower = towers_now < towers_target && (float)(now - last_tower_started) > 15.0f;
            if (want_quarters && (quarters_now < 3 || !want_tower || towers_now >= 2))
            {
                if (startQuarters(now, roster, armory))
                {
                    return;
                }
            }
            if (want_tower)
            {
                startTower(now, roster, intel, threat, armory, towers_now);
            }
        }
        public float last_expand_try;
        public java.util.List failed_sites;
        public float expand_blocked_until;
        // Java: 140
        public bool maybeExpand(float now, com.oddlabs.tt.player.fable.Roster roster, com.oddlabs.tt.player.fable.Intel intel, com.oddlabs.tt.player.fable.Threat threat, com.oddlabs.tt.player.fable.Military military, com.oddlabs.tt.model.LandBuilding armory, int active_jobs)
        {
            if (!@params.expansion_armory || now < @params.expand_min_time || (float)(now - last_expand_try) < 60.0f || now < expand_blocked_until)
            {
                return false;
            }
            if (roster.armories.size() != 1 || construction.hasActive(com.oddlabs.tt.model.Race.BUILDING_ARMORY) || countUnderConstruction(roster, com.oddlabs.tt.model.Race.BUILDING_ARMORY) > 0)
            {
                return false;
            }
            if (economy.d_iron_m < @params.expand_iron_m || ((object)military.posture == (object)com.oddlabs.tt.player.fable.Military.Posture.DEFEND) || ((object)threat.level == (object)com.oddlabs.tt.player.fable.Threat.Level.ALERT) || ((object)threat.level == (object)com.oddlabs.tt.player.fable.Threat.Level.ENGAGED))
            {
                return false;
            }
            if (roster.buildingCount() + active_jobs >= roster.buildingCap() - 1 || roster.totalPeons() < 60)
            {
                return false;
            }
            last_expand_try = now;
            com.oddlabs.tt.player.fable.SiteFinder.Site site = plan.chooseExpansionSite(armory.getGridX(), armory.getGridY(), @params.expand_max_dist);
            if (site == null)
            {
                log.info("expansion: no site by fresh iron on our half");
                return false;
            }
            if (intel.warriorsNear(site.gx(), site.gy(), 40) > 0)
            {
                log.info(new java.util.function.Supplier_L(() =>
                {
                    return "expansion: site " + (site.gx()) + "," + (site.gy()) + " contested, waiting";
                }));
                return false;
            }
            java.util.List crew = economy.requestCrew(@params.crew_armory_expand, site.gx(), site.gy(), roster, now, true);
            if (crew.size() < 4)
            {
                return false;
            }
            com.oddlabs.tt.model.Unit scout = com.oddlabs.tt.player.fable.BuildPlanner.nearest(crew, site.gx(), site.gy(), null);
            crew.remove(scout);
            com.oddlabs.tt.player.fable.Construction.Job job = construction.start(com.oddlabs.tt.model.Race.BUILDING_ARMORY, site.gx(), site.gy(), "A2", scout, crew, @params.crew_armory_expand, now);
            if (job == null)
            {
                return false;
            }
            log.info(new java.util.function.Supplier_L(() =>
            {
                return java.lang.JString.format("EXPANSION armory at %d,%d (iron %.0f m from the main one)", site.gx(), site.gy(), economy.d_iron_m);
            }));
            return true;
        }
        // Java: 177
        public int countUnderConstruction(com.oddlabs.tt.player.fable.Roster roster, int template_id)
        {
            int n = 0;
            foreach (com.oddlabs.tt.model.LandBuilding b in java.lang.JIter.of(roster.under_construction))
            {
                if (((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID() == template_id && !construction.isTracked(b))
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 185
        public int towersWanted(float now, com.oddlabs.tt.player.fable.Roster roster, com.oddlabs.tt.player.fable.Threat threat)
        {
            int t = 2;
            if (now >= 480.0f)
            {
                t = 4;
            }
            if (now >= 720.0f)
            {
                t = 6 + java.lang.JC.f2i(((float)(((float)(now - 720.0f)) / 60.0f)));
            }
            if (((object)threat.level == (object)com.oddlabs.tt.player.fable.Threat.Level.ALERT) || ((object)threat.level == (object)com.oddlabs.tt.player.fable.Threat.Level.ENGAGED))
            {
                t += 1;
            }
            com.oddlabs.tt.model.LandBuilding armory = roster.mainArmory();
            if (armory != null && now >= 600.0f && com.oddlabs.tt.player.fable.Economy.stock(armory, typeof(com.oddlabs.tt.landscape.TreeSupply)) >= 120)
            {
                t += 2;
            }
            return java.lang.Math.min(t, java.lang.Math.min(@params.towers_max, roster.buildingCap() - @params.quarters_max - 1));
        }
        public java.util.List opening_sites;
        public int opening_index;
        // Java: 212
        public void opening(float now, com.oddlabs.tt.player.fable.Roster roster)
        {
            if (roster.peons.isEmpty())
            {
                return;
            }
            opening_started = true;
            java.util.List peons = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit p in java.lang.JIter.of(roster.peons))
            {
                if (orders.usable(p))
                {
                    peons.add(p);
                }
            }
            if (peons.isEmpty())
            {
                return;
            }
            java.util.List reserved = new java.util.ArrayList();
            reserved.add(new int[] { plan.armory_x, plan.armory_y });
            int[] q1 = new int[] { plan.q1_x, plan.q1_y };
            reserved.add(q1);
            opening_sites.add(q1);
            for (int i = 1; i < @params.opening_quarters; i++)
            {
                float f = (float)(((float)(i)) / @params.opening_quarters);
                int ax = java.lang.Math.round((float)(plan.q1_x + (float)((plan.armory_x - plan.q1_x) * f)));
                int ay = java.lang.Math.round((float)(plan.q1_y + (float)((plan.armory_y - plan.q1_y) * f)));
                int fax = ax;
                int fay = ay;
                com.oddlabs.tt.player.fable.SiteFinder.Site s = plan.nextQuartersSite(ax, ay, reserved);
                if (s == null)
                {
                    log.info("opening: no site for quarters " + ((i + 1)) + " near " + (fax) + "," + (fay));
                    continue;
                }
                int[] c = new int[] { s.gx(), s.gy() };
                reserved.add(c);
                opening_sites.add(c);
            }
            com.oddlabs.tt.model.Unit s1 = com.oddlabs.tt.player.fable.BuildPlanner.nearest(peons, plan.q1_x, plan.q1_y, null);
            peons.remove(s1);
            com.oddlabs.tt.model.Unit s_armory = (peons.size() > 6 ? com.oddlabs.tt.player.fable.BuildPlanner.nearest(peons, plan.armory_x, plan.armory_y, null) : (com.oddlabs.tt.model.Unit)(null));
            if (s_armory != null)
            {
                peons.remove(s_armory);
            }
            q1_job = construction.start(com.oddlabs.tt.model.Race.BUILDING_QUARTERS, plan.q1_x, plan.q1_y, "Q1", s1, peons, 99, now);
            if (q1_job == null)
            {
                com.oddlabs.tt.player.fable.SiteFinder.Site alt = plan.nextQuartersSite(plan.start_x, plan.start_y, reserved);
                if (alt != null)
                {
                    opening_sites.set(0, new int[] { alt.gx(), alt.gy() });
                    q1_job = construction.start(com.oddlabs.tt.model.Race.BUILDING_QUARTERS, alt.gx(), alt.gy(), "Q1", s1, peons, 99, now);
                }
            }
            opening_index = 1;
            if (s_armory != null)
            {
                armory_job = construction.start(com.oddlabs.tt.model.Race.BUILDING_ARMORY, plan.armory_x, plan.armory_y, "armory", s_armory, java.util.List_.of(), @params.crew_armory_opening, now);
                if (armory_job == null)
                {
                    plan.chooseArmorySite();
                    armory_job = construction.start(com.oddlabs.tt.model.Race.BUILDING_ARMORY, plan.armory_x, plan.armory_y, "armory", s_armory, java.util.List_.of(), @params.crew_armory_opening, now);
                }
                if (armory_job != null)
                {
                    armory_job.crew_target = 1;
                }
            }
            log.info(new java.util.function.Supplier_L(() =>
            {
                return "opening: " + (opening_sites.size()) + " quarters sites, armory scout " + java.lang.JStr.valueOf((armory_job != null));
            }));
        }
        // Java: 281
        public void keepOpeningCrews(float now, com.oddlabs.tt.player.fable.Roster roster)
        {
            int active_q = construction.countActive(com.oddlabs.tt.model.Race.BUILDING_QUARTERS);
            int quarters_done = roster.quarters.size();
            if (active_q < @params.opening_parallel && quarters_done + active_q < @params.opening_quarters && opening_index < opening_sites.size() && (quarters_done >= 1 || active_q == 0))
            {
                int[] site = ((int[])opening_sites.get(opening_index));
                int target = (quarters_done + active_q >= 2 ? @params.crew_quarters + 4 : 99);
                java.util.List crew = economy.requestCrew(target, site[0], site[1], roster, now, false);
                if (crew.size() >= 3)
                {
                    com.oddlabs.tt.model.Unit scout = com.oddlabs.tt.player.fable.BuildPlanner.nearest(crew, site[0], site[1], null);
                    crew.remove(scout);
                    com.oddlabs.tt.player.fable.Construction.Job job = construction.start(com.oddlabs.tt.model.Race.BUILDING_QUARTERS, site[0], site[1], "Q" + ((quarters_done + active_q + 1)), scout, crew, target, now);
                    opening_index++;
                    if (job == null)
                    {
                        last_site_failure = now;
                        return;
                    }
                    last_quarters_started = now;
                }
            }
            com.oddlabs.tt.player.fable.Construction.Job aj = construction.activeJob(com.oddlabs.tt.model.Race.BUILDING_ARMORY);
            if (aj != null)
            {
                if (quarters_done >= @params.opening_quarters || (active_q == 0 && opening_index >= opening_sites.size()))
                {
                    aj.crew_target = 99;
                }
                else if (quarters_done >= 2)
                {
                    aj.crew_target = @params.crew_armory_opening;
                }
            }
            keepCrewsStaffed(now, roster);
        }
        // Java: 315
        public void onDone(com.oddlabs.tt.player.fable.Construction.Job job, float now, com.oddlabs.tt.player.fable.Roster roster)
        {
            com.oddlabs.tt.model.LandBuilding b = job.placedBuilding();
            if (b == null)
            {
                return;
            }
            switch ((job.template_id))
            {
                case 0:
                {
                    quarters_built++;
                    java.util.List crew = crewOf(job);
                    int breeders = 0;
                    foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(crew))
                    {
                        if (breeders >= @params.breeders_early)
                        {
                            break;
                        }
                        orders.enter(u, b);
                        jobs.set(u, com.oddlabs.tt.player.fable.Jobs.Kind.ENTER, b.getGridX(), b.getGridY(), b, null, now, 0);
                        breeders++;
                    }
                    if (roster.mainArmory() == null)
                    {
                        orders.setRally(b, plan.armory_x, plan.armory_y);
                    }
                    break;
                }
                case 1:
                {
                    foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(crewOf(job)))
                    {
                        orders.enter(u_2, b);
                        jobs.set(u_2, com.oddlabs.tt.player.fable.Jobs.Kind.ENTER, b.getGridX(), b.getGridY(), b, null, now, 0);
                    }
                    economy.bootstrap_until = (float)(now + 25.0f);
                    break;
                }
                case 2:
                {
                    towers_built++;
                    break;
                }
                default:
                {
                    break;
                }
            }
        }
        // Java: 351
        public java.util.List crewOf(com.oddlabs.tt.player.fable.Construction.Job job)
        {
            java.util.List result = new java.util.ArrayList();
            if (job.scout != null && orders.usable(job.scout))
            {
                result.add(job.scout);
            }
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(job.crew))
            {
                if (orders.usable(u) && !result.contains(u))
                {
                    result.add(u);
                }
            }
            return result;
        }
        // Java: 362
        public void keepCrewsStaffed(float now, com.oddlabs.tt.player.fable.Roster roster)
        {
            foreach (com.oddlabs.tt.player.fable.Construction.Job job in java.lang.JIter.of(construction.active()))
            {
                if (!job.active())
                {
                    continue;
                }
                if (!job.isPlaced())
                {
                    continue;
                }
                int have = construction.workers(job) + countArriving(job);
                int need = job.crew_target - have;
                if (need <= 0)
                {
                    continue;
                }
                bool allow_workers = roster.mainArmory() != null && job.template_id != com.oddlabs.tt.model.Race.BUILDING_ARMORY;
                java.util.List crew = economy.requestCrew(need, job.gx, job.gy, roster, now, allow_workers);
                foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(crew))
                {
                    construction.addHelper(job, u, now);
                }
            }
        }
        // Java: 381
        public int countArriving(com.oddlabs.tt.player.fable.Construction.Job job)
        {
            int n = 0;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(job.crew))
            {
                if (orders.usable(u) && !com.oddlabs.tt.player.fable.Construction.isWorkingOn(u, job.building))
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 391
        public bool startQuarters(float now, com.oddlabs.tt.player.fable.Roster roster, com.oddlabs.tt.model.LandBuilding armory)
        {
            java.util.List reserved = reservedCells(roster);
            com.oddlabs.tt.player.fable.SiteFinder.Site site = plan.nextQuartersSite(armory.getGridX(), armory.getGridY(), reserved);
            if (site == null)
            {
                last_site_failure = now;
                log.info("no quarters site found");
                return false;
            }
            java.util.List crew = economy.requestCrew(@params.crew_quarters, site.gx(), site.gy(), roster, now, true);
            if (crew.isEmpty())
            {
                return false;
            }
            com.oddlabs.tt.model.Unit scout = com.oddlabs.tt.player.fable.BuildPlanner.nearest(crew, site.gx(), site.gy(), null);
            crew.remove(scout);
            com.oddlabs.tt.player.fable.Construction.Job job = construction.start(com.oddlabs.tt.model.Race.BUILDING_QUARTERS, site.gx(), site.gy(), "Q" + ((roster.quarters.size() + 1)), scout, crew, @params.crew_quarters, now);
            if (job == null)
            {
                last_site_failure = now;
                return false;
            }
            last_quarters_started = now;
            return true;
        }
        // Java: 414
        public bool startTower(float now, com.oddlabs.tt.player.fable.Roster roster, com.oddlabs.tt.player.fable.Intel intel, com.oddlabs.tt.player.fable.Threat threat, com.oddlabs.tt.model.LandBuilding armory, int index)
        {
            java.util.List towers = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.LandBuilding t in java.lang.JIter.of(roster.towers))
            {
                towers.add(com.oddlabs.tt.player.fable.BasePlan.centre(t));
            }
            foreach (com.oddlabs.tt.model.LandBuilding t_2 in java.lang.JIter.of(roster.under_construction))
            {
                if (((com.oddlabs.tt.model.BuildingTemplate)t_2.getTemplate()).getTemplateID() == com.oddlabs.tt.model.Race.BUILDING_TOWER)
                {
                    towers.add(com.oddlabs.tt.player.fable.BasePlan.centre(t_2));
                }
            }
            foreach (com.oddlabs.tt.player.fable.Construction.Job j in java.lang.JIter.of(construction.active()))
            {
                if (j.template_id == com.oddlabs.tt.model.Race.BUILDING_TOWER)
                {
                    towers.add(new int[] { j.gx, j.gy });
                }
            }
            java.util.List reserved = reservedCells(roster);
            com.oddlabs.tt.player.fable.SiteFinder.Site site = default;
            com.oddlabs.tt.model.LandBuilding a2 = (roster.armories.size() >= 2 ? ((com.oddlabs.tt.model.LandBuilding)roster.armories.get(1)) : (com.oddlabs.tt.model.LandBuilding)(null));
            if (a2 != null)
            {
                int near_a2 = 0;
                foreach (int[] t_3 in java.lang.JIter.of(towers))
                {
                    if (com.oddlabs.tt.player.fable.BasePlan.dist2(t_3[0], t_3[1], a2.getGridX(), a2.getGridY()) <= 20 * 20)
                    {
                        near_a2++;
                    }
                }
                if (near_a2 < 2)
                {
                    java.util.List avoid = new java.util.ArrayList(reserved);
                    avoid.addAll(towers);
                    site = plan.towerSiteNear(a2.getGridX(), a2.getGridY(), 14, avoid, 8);
                    if (site != null)
                    {
                        java.util.List crew = economy.requestCrew(@params.crew_tower, site.gx(), site.gy(), roster, now, true);
                        if (!crew.isEmpty())
                        {
                            com.oddlabs.tt.model.Unit scout = com.oddlabs.tt.player.fable.BuildPlanner.nearest(crew, site.gx(), site.gy(), null);
                            crew.remove(scout);
                            if (construction.start(com.oddlabs.tt.model.Race.BUILDING_TOWER, site.gx(), site.gy(), "T-A2", scout, crew, @params.crew_tower, now) != null)
                            {
                                last_tower_started = now;
                                return true;
                            }
                        }
                    }
                }
            }
            if (index >= 2 && index % 3 == 2 && economy.d_iron_m > 30.0f)
            {
                site = ironFieldTowerSite(armory, roster, reserved, towers);
                if (site == null)
                {
                    site = plan.nextTowerSite(armory.getGridX(), armory.getGridY(), index, towers, reserved);
                }
            }
            else
            {
                site = plan.nextTowerSite(armory.getGridX(), armory.getGridY(), index, towers, reserved);
            }
            for (int k = 1; site == null && k < 8; k++)
            {
                site = plan.nextTowerSite(armory.getGridX(), armory.getGridY(), index + k, towers, reserved);
            }
            if (site == null)
            {
                java.util.List avoid_2 = new java.util.ArrayList(reserved);
                avoid_2.addAll(towers);
                site = plan.towerSiteNear(armory.getGridX(), armory.getGridY(), @params.tower_ring_radius + 10, avoid_2, 8);
            }
            if (site == null)
            {
                last_site_failure = now;
                log.info("no tower site found for index " + (index));
                return false;
            }
            int crew_size = @params.crew_tower;
            if (((object)threat.level == (object)com.oddlabs.tt.player.fable.Threat.Level.ALERT) && roster.towers.size() < 2)
            {
                crew_size = @params.crew_tower * 2;
            }
            java.util.List crew_2 = economy.requestCrew(crew_size, site.gx(), site.gy(), roster, now, true);
            if (crew_2.isEmpty())
            {
                return false;
            }
            com.oddlabs.tt.model.Unit scout_2 = com.oddlabs.tt.player.fable.BuildPlanner.nearest(crew_2, site.gx(), site.gy(), null);
            crew_2.remove(scout_2);
            com.oddlabs.tt.player.fable.Construction.Job job = construction.start(com.oddlabs.tt.model.Race.BUILDING_TOWER, site.gx(), site.gy(), "T" + ((index + 1)), scout_2, crew_2, crew_size, now);
            if (job == null)
            {
                last_site_failure = now;
                return false;
            }
            last_tower_started = now;
            return true;
        }
        // Java: 492
        public com.oddlabs.tt.player.fable.SiteFinder.Site ironFieldTowerSite(com.oddlabs.tt.model.LandBuilding armory, com.oddlabs.tt.player.fable.Roster roster, java.util.List reserved, java.util.List towers)
        {
            java.util.List nodes = economyIronNodes(armory);
            if (nodes.isEmpty())
            {
                return null;
            }
            int sx = 0;
            int sy = 0;
            foreach (com.oddlabs.tt.player.fable.ResourceMap.Node n in java.lang.JIter.of(nodes))
            {
                sx += n.grid_x;
                sy += n.grid_y;
            }
            int cx = sx / nodes.size();
            int cy = sy / nodes.size();
            java.util.List all = new java.util.ArrayList(reserved);
            all.addAll(towers);
            foreach (int[] t in java.lang.JIter.of(towers))
            {
                if (com.oddlabs.tt.player.fable.BasePlan.dist2(t[0], t[1], cx, cy) <= 12 * 12)
                {
                    return null;
                }
            }
            return plan.towerSiteNear(cx, cy, 10, all, 3);
        }
        // Java: 513
        public java.util.List economyIronNodes(com.oddlabs.tt.model.LandBuilding armory)
        {
            return plan_resources().nearestAlive(com.oddlabs.tt.player.fable.ResourceMap.Kind.IRON, armory.getGridX(), armory.getGridY(), @params.gather_radius, 8);
        }
        public com.oddlabs.tt.player.fable.ResourceMap plan_resources_f;
        // Java: 520
        public void setResources(com.oddlabs.tt.player.fable.ResourceMap r)
        {
            plan_resources_f = r;
        }
        // Java: 524
        public com.oddlabs.tt.player.fable.ResourceMap plan_resources()
        {
            return plan_resources_f;
        }
        // Java: 528
        public void rebuildArmory(float now, com.oddlabs.tt.player.fable.Roster roster, com.oddlabs.tt.player.fable.Intel intel)
        {
            if ((float)(now - last_site_failure) < 10.0f)
            {
                return;
            }
            int gx = plan.fallback_x;
            int gy = plan.fallback_y;
            if (!armory_ever_complete || gx < 0 || !sitesLegal(com.oddlabs.tt.model.Race.BUILDING_ARMORY, gx, gy))
            {
                int ax = (roster.quarters.isEmpty() ? plan.start_x : ((com.oddlabs.tt.model.LandBuilding)roster.quarters.getFirst()).getGridX());
                int ay = (roster.quarters.isEmpty() ? plan.start_y : ((com.oddlabs.tt.model.LandBuilding)roster.quarters.getFirst()).getGridY());
                if (!armory_ever_complete)
                {
                    plan.chooseArmorySite();
                    gx = plan.armory_x;
                    gy = plan.armory_y;
                }
                else
                {
                    com.oddlabs.tt.player.fable.SiteFinder.Site s = plan.towerSiteNear(ax, ay, 30, reservedCells(roster), 11);
                    s = quartersSizedSiteNear(ax, ay, roster);
                    if (s == null)
                    {
                        last_site_failure = now;
                        return;
                    }
                    gx = s.gx();
                    gy = s.gy();
                }
            }
            java.util.List crew = economy.requestCrew(@params.crew_armory_rebuild, gx, gy, roster, now, false);
            foreach (com.oddlabs.tt.model.LandBuilding q in java.lang.JIter.of(roster.quarters))
            {
                int inside = com.oddlabs.tt.player.fable.Roster.unitsInside(q);
                if (inside > 1)
                {
                    orders.setRally(q, gx, gy);
                    orders.deploy(q, com.oddlabs.tt.model.DeployType.PEON, inside - 1);
                }
            }
            if (crew.isEmpty())
            {
                last_site_failure = now;
                return;
            }
            com.oddlabs.tt.model.Unit scout = com.oddlabs.tt.player.fable.BuildPlanner.nearest(crew, gx, gy, null);
            crew.remove(scout);
            com.oddlabs.tt.player.fable.Construction.Job job = construction.start(com.oddlabs.tt.model.Race.BUILDING_ARMORY, gx, gy, "armory-rebuild", scout, crew, @params.crew_armory_rebuild, now);
            if (job == null)
            {
                last_site_failure = now;
                return;
            }
            log.info("REBUILDING ARMORY at " + (gx) + "," + (gy));
        }
        // Java: 577
        public com.oddlabs.tt.player.fable.SiteFinder.Site quartersSizedSiteNear(int ax, int ay, com.oddlabs.tt.player.fable.Roster roster)
        {
            java.util.List reserved = reservedCells(roster);
            return plan.nextQuartersSite(ax, ay, reserved);
        }
        // Java: 582
        public bool sitesLegal(int template_id, int gx, int gy)
        {
            return construction_sites().isLegal(template_id, gx, gy);
        }
        public com.oddlabs.tt.player.fable.SiteFinder site_finder;
        // Java: 588
        public void setSiteFinder(com.oddlabs.tt.player.fable.SiteFinder s)
        {
            site_finder = s;
        }
        // Java: 592
        public com.oddlabs.tt.player.fable.SiteFinder construction_sites()
        {
            return site_finder;
        }
        // Java: 596
        public java.util.List reservedCells(com.oddlabs.tt.player.fable.Roster roster)
        {
            java.util.List reserved = new java.util.ArrayList(construction.reservedCells());
            foreach (com.oddlabs.tt.model.LandBuilding b in java.lang.JIter.of(roster.quarters))
            {
                reserved.add(com.oddlabs.tt.player.fable.BasePlan.centre(b));
            }
            foreach (com.oddlabs.tt.model.LandBuilding b_2 in java.lang.JIter.of(roster.armories))
            {
                reserved.add(com.oddlabs.tt.player.fable.BasePlan.centre(b_2));
            }
            foreach (com.oddlabs.tt.model.LandBuilding b_3 in java.lang.JIter.of(roster.towers))
            {
                reserved.add(com.oddlabs.tt.player.fable.BasePlan.centre(b_3));
            }
            foreach (com.oddlabs.tt.model.LandBuilding b_4 in java.lang.JIter.of(roster.under_construction))
            {
                reserved.add(com.oddlabs.tt.player.fable.BasePlan.centre(b_4));
            }
            foreach (int[] f in java.lang.JIter.of(failed_sites))
            {
                reserved.add(new int[] { f[0], f[1] });
            }
            return reserved;
        }
        // Java: 612
        public static com.oddlabs.tt.model.Unit nearest(java.util.List units, int gx, int gy, com.oddlabs.tt.model.Unit exclude)
        {
            com.oddlabs.tt.model.Unit best = null;
            int best_d2 = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(units))
            {
                if (((object)u == (object)exclude) || u.isDead())
                {
                    continue;
                }
                int d2 = com.oddlabs.tt.player.fable.BasePlan.dist2(u.getGridX(), u.getGridY(), gx, gy);
                if (d2 < best_d2)
                {
                    best_d2 = d2;
                    best = u;
                }
            }
            return best;
        }
        // Java: 627
        public bool openingDone()
        {
            return armory_ever_complete;
        }
        // Java: 631
        public string status()
        {
            java.lang.StringBuilder sb = new java.lang.StringBuilder("q" + (quarters_built) + " t" + (towers_built));
            foreach (com.oddlabs.tt.player.fable.Construction.Job j in java.lang.JIter.of(construction.active()))
            {
                sb.append(' ').append(j);
            }
            return java.lang.JO.toString(sb);
        }
        private void init_BuildPlanner()
        {
            this.last_tower_started = (float)(-1000.0f);
            this.last_quarters_started = (float)(-1000.0f);
            this.last_site_failure = (float)(-1000.0f);
            this.last_expand_try = (float)(-1000.0f);
            this.failed_sites = new java.util.ArrayList();
            this.expand_blocked_until = (float)(-1.0f);
            this.opening_sites = new java.util.ArrayList();
        }
        public BuildPlanner(com.oddlabs.tt.player.fable.Orders orders, com.oddlabs.tt.player.fable.Jobs jobs, com.oddlabs.tt.player.fable.Construction construction, com.oddlabs.tt.player.fable.BasePlan plan, com.oddlabs.tt.player.fable.Economy economy, com.oddlabs.tt.player.fable.Params @params, com.oddlabs.tt.player.fable.AiLog log) : base()
        {
            init_BuildPlanner();
            this.orders = orders;
            this.jobs = jobs;
            this.construction = construction;
            this.plan = plan;
            this.economy = economy;
            this.@params = @params;
            this.log = log;
        }
    }
}
