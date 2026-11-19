// Translated from Economy.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.fable
{
    // Java: 33
    public class Economy : java.lang.Object
    {
        // Java: 34
        public sealed class Mode : java.lang.Enum
        {
            public static Mode[] values() { return (Mode[])VALUES_.Clone(); }
            public static Mode valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly Mode IRON = new Mode("IRON", 0);
            public static readonly Mode ROCK = new Mode("ROCK", 1);
            public static readonly Mode[] VALUES_ = new Mode[] { IRON, ROCK };
            public Mode(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        // Java: 40
        public class Demands : java.lang.Object
        {
            public bool emergency;
            public bool armory_threat;
            public java.util.List hot_cells;
            public int hot_radius;
            public bool want_rock_warriors;
            public bool cap_assault;
            private void init_Demands()
            {
                this.hot_cells = new java.util.ArrayList();
                this.hot_radius = 20;
            }
            public Demands() : base()
            {
                init_Demands();
            }
        }
        public com.oddlabs.tt.player.Player player;
        public com.oddlabs.tt.player.fable.Orders orders;
        public com.oddlabs.tt.player.fable.Jobs jobs;
        public com.oddlabs.tt.player.fable.ResourceMap resources;
        public com.oddlabs.tt.player.fable.BasePlan plan;
        public com.oddlabs.tt.player.fable.Params @params;
        public com.oddlabs.tt.player.fable.AiLog log;
        public com.oddlabs.tt.player.fable.Economy.Demands demands;
        public com.oddlabs.tt.player.fable.Economy.Mode mode;
        public float rate_star;
        public int workers_star;
        public int wood_star;
        public int iron_star;
        public int rock_star;
        public float d_wood_m;
        public float d_iron_m;
        public float last_conversion_wood;
        public float last_conversion_iron;
        public float last_conversion_rock;
        public float last_conversion_rubber;
        public float last_hunt_check;
        public java.util.List hunters;
        public java.util.Set reserved;
        public com.oddlabs.tt.model.LandBuilding rally_armory;
        public bool cap_mode;
        public int quarters_rally_set_count;
        public float bootstrap_until;
        public float starved_iron_since;
        // Java: 97
        public static int stock(com.oddlabs.tt.model.LandBuilding armory, System.Type key)
        {
            if (armory.isDead() || !armory.isComplete())
            {
                return 0;
            }
            com.oddlabs.tt.model.SupplyContainer c = armory.getSupplyContainer(key);
            return (c == null ? 0 : c.getNumSupplies());
        }
        // Java: 104
        public static int queued(com.oddlabs.tt.model.LandBuilding armory, com.oddlabs.tt.model.DeployType type)
        {
            if (armory.isDead() || !armory.isComplete())
            {
                return 0;
            }
            com.oddlabs.tt.model.DeployContainer c = armory.getDeployContainer(type);
            return (c == null ? 0 : c.getNumSupplies());
        }
        // Java: 111
        public static int workers(com.oddlabs.tt.model.LandBuilding armory)
        {
            if (armory.isDead() || !armory.isComplete())
            {
                return 0;
            }
            return (armory.getUnitContainer() == null ? 0 : armory.getUnitContainer().getNumSupplies());
        }
        // Java: 117
        public static bool starved(com.oddlabs.tt.model.LandBuilding armory, System.Type weapon)
        {
            if (armory.isDead() || !armory.isComplete())
            {
                return false;
            }
            com.oddlabs.tt.model.BuildSupplyContainer c = armory.getBuildSupplyContainer(weapon);
            return (c is com.oddlabs.tt.model.BuildProductionContainer p) && !p.hasEnoughSupplies();
        }
        // Java: 125
        public static float tripCost(float d_metres)
        {
            return (float)(12.0f + (float)(0.45f * ((float)(d_metres + 4.0f))));
        }
        // Java: 130
        public float meanDistance(com.oddlabs.tt.model.LandBuilding armory, com.oddlabs.tt.player.fable.ResourceMap.Kind kind, int k)
        {
            java.util.List nodes = resources.nearestAlive(kind, armory.getGridX(), armory.getGridY(), @params.gather_radius * 2, k);
            if (nodes.isEmpty())
            {
                return (float)((float)(@params.gather_radius * 2.0f) * 2.0f);
            }
            float sum = 0.0f;
            foreach (com.oddlabs.tt.player.fable.ResourceMap.Node n in java.lang.JIter.of(nodes))
            {
                sum = (float)(sum + (float)(n.distanceTo(armory.getGridX(), armory.getGridY()) * 2.0f));
            }
            return (float)(sum / nodes.size());
        }
        // Java: 147
        public void tick(float now, com.oddlabs.tt.player.fable.Roster roster, com.oddlabs.tt.player.fable.Intel intel, com.oddlabs.tt.model.LandBuilding training_quarters, int total_peons)
        {
            last_intel = intel;
            com.oddlabs.tt.model.LandBuilding armory = roster.mainArmory();
            cap_mode = roster.unitCount() >= roster.unitCap() - @params.cap_margin;
            manageQuarters(now, roster, armory, training_quarters, total_peons);
            if (armory == null)
            {
                return;
            }
            if (((object)rally_armory != (object)armory))
            {
                rally_armory = armory;
                quarters_rally_set_count = (-1);
            }
            foreach (com.oddlabs.tt.model.LandBuilding a in java.lang.JIter.of(roster.armories))
            {
                if (orders.complete(a))
                {
                    manageWeaponQueues(a);
                }
            }
            allocate(now, roster, armory);
            manageExpansion(now, roster, armory);
            hunts(now, roster, intel, armory);
        }
        public float last_a2_iron;
        public float last_a2_wood;
        public float last_a2_transfer;
        // Java: 174
        public static com.oddlabs.tt.model.LandBuilding expansion(com.oddlabs.tt.player.fable.Roster roster)
        {
            return (roster.armories.size() >= 2 ? ((com.oddlabs.tt.model.LandBuilding)roster.armories.get(1)) : (com.oddlabs.tt.model.LandBuilding)(null));
        }
        // Java: 183
        public void manageExpansion(float now, com.oddlabs.tt.player.fable.Roster roster, com.oddlabs.tt.model.LandBuilding main)
        {
            com.oddlabs.tt.model.LandBuilding a2 = com.oddlabs.tt.player.fable.Economy.expansion(roster);
            if (a2 == null || !orders.complete(a2))
            {
                return;
            }
            int W2 = com.oddlabs.tt.player.fable.Economy.workers(a2);
            int S2_i = com.oddlabs.tt.player.fable.Economy.stock(a2, typeof(com.oddlabs.tt.model.IronSupply));
            int S2_w = com.oddlabs.tt.player.fable.Economy.stock(a2, typeof(com.oddlabs.tt.landscape.TreeSupply));
            if (W2 < @params.expand_workers && (float)(now - last_a2_transfer) > 15.0f && com.oddlabs.tt.player.fable.Economy.queued(main, com.oddlabs.tt.model.DeployType.PEON) == 0)
            {
                int spare = com.oddlabs.tt.player.fable.Economy.workers(main) - workers_star - 4;
                int n = java.lang.Math.min(java.lang.Math.min(spare, 6), @params.expand_workers - W2);
                if (n > 0)
                {
                    orders.setRally(main, a2.getGridX(), a2.getGridY());
                    orders.deploy(main, com.oddlabs.tt.model.DeployType.PEON, n);
                    rally_restore = main;
                    last_a2_transfer = now;
                    int fn = n;
                    log.info(new java.util.function.Supplier_L(() =>
                    {
                        return "expansion: " + (fn) + " workers sent over";
                    }));
                }
            }
            else if (rally_restore != null && (float)(now - last_a2_transfer) > 3.0f)
            {
                rally_restore = null;
                rally_dirty = true;
            }
            if (W2 <= 4)
            {
                return;
            }
            if (S2_i < 10 && com.oddlabs.tt.player.fable.Economy.queued(a2, com.oddlabs.tt.model.DeployType.PEON_HARVEST_IRON) == 0 && (float)(now - last_a2_iron) > 8.0f && W2 > 5)
            {
                orders.deploy(a2, com.oddlabs.tt.model.DeployType.PEON_HARVEST_IRON, java.lang.Math.min(3, W2 - 4));
                last_a2_iron = now;
            }
            else if (S2_w < 8 && com.oddlabs.tt.player.fable.Economy.queued(a2, com.oddlabs.tt.model.DeployType.PEON_HARVEST_TREE) == 0 && (float)(now - last_a2_wood) > 8.0f && W2 > 5)
            {
                orders.deploy(a2, com.oddlabs.tt.model.DeployType.PEON_HARVEST_TREE, java.lang.Math.min(2, W2 - 4));
                last_a2_wood = now;
            }
            if (S2_i >= 25)
            {
                orders.recallGatherers(a2, typeof(com.oddlabs.tt.model.IronSupply), 2);
            }
            if (S2_w >= 40)
            {
                orders.recallGatherers(a2, typeof(com.oddlabs.tt.landscape.TreeSupply), 2);
            }
        }
        public com.oddlabs.tt.model.LandBuilding rally_restore;
        public bool rally_dirty;
        // Java: 228
        public com.oddlabs.tt.model.LandBuilding nearestArmory(com.oddlabs.tt.player.fable.Roster roster, com.oddlabs.tt.model.LandBuilding fallback, int gx, int gy)
        {
            com.oddlabs.tt.model.LandBuilding best = fallback;
            int best_d2 = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.LandBuilding a in java.lang.JIter.of(roster.armories))
            {
                if (!orders.complete(a))
                {
                    continue;
                }
                int d2 = com.oddlabs.tt.player.fable.BasePlan.dist2(a.getGridX(), a.getGridY(), gx, gy);
                if (d2 < best_d2)
                {
                    best_d2 = d2;
                    best = a;
                }
            }
            return best;
        }
        // Java: 246
        public void manageQuarters(float now, com.oddlabs.tt.player.fable.Roster roster, com.oddlabs.tt.model.LandBuilding armory, com.oddlabs.tt.model.LandBuilding training_quarters, int total_peons)
        {
            if (roster.quarters.isEmpty())
            {
                return;
            }
            if (armory != null && roster.quarters.size() != quarters_rally_set_count)
            {
                foreach (com.oddlabs.tt.model.LandBuilding q in java.lang.JIter.of(roster.quarters))
                {
                    orders.setRally(q, armory);
                }
                quarters_rally_set_count = roster.quarters.size();
            }
            int base_target = (cap_mode ? 1 : (armory == null ? @params.breeders_early : (roster.unitCount() < @params.boom_units ? @params.breeders_boom : @params.breeders_late)));
            if (demands.emergency && armory != null)
            {
                base_target = java.lang.Math.min(base_target, @params.breeders_late);
            }
            com.oddlabs.tt.model.LandBuilding rear = null;
            if (armory != null && roster.quarters.size() >= 3 && base_target <= @params.breeders_late)
            {
                int best_d2 = (-1);
                foreach (com.oddlabs.tt.model.LandBuilding q_2 in java.lang.JIter.of(roster.quarters))
                {
                    int d2 = com.oddlabs.tt.player.fable.BasePlan.dist2(q_2.getGridX(), q_2.getGridY(), armory.getGridX(), armory.getGridY());
                    if (d2 > best_d2)
                    {
                        best_d2 = d2;
                        rear = q_2;
                    }
                }
            }
            foreach (com.oddlabs.tt.model.LandBuilding q_3 in java.lang.JIter.of(roster.quarters))
            {
                int inside = com.oddlabs.tt.player.fable.Roster.unitsInside(q_3);
                int target = base_target;
                if (((object)q_3 == (object)training_quarters))
                {
                    target = @params.breeders_training;
                }
                else if (((object)q_3 == (object)rear))
                {
                    target = java.lang.Math.max(1, base_target - 1);
                }
                if (cap_mode)
                {
                    target = 1;
                }
                if (inside < target && armory != null && !demands.emergency && roster.unitCount() < @params.boom_units)
                {
                    int spare = com.oddlabs.tt.player.fable.Economy.workers(armory) - workers_star - 2;
                    if (spare > 0 && com.oddlabs.tt.player.fable.Economy.queued(armory, com.oddlabs.tt.model.DeployType.PEON) == 0 && com.oddlabs.tt.player.fable.Economy.queued(q_3, com.oddlabs.tt.model.DeployType.PEON) == 0)
                    {
                        int n = java.lang.Math.min(spare, target - inside);
                        orders.setRally(armory, q_3);
                        orders.deploy(armory, com.oddlabs.tt.model.DeployType.PEON, n);
                    }
                }
                if (inside > target)
                {
                    int n_2 = inside - target;
                    if (armory != null)
                    {
                        if (com.oddlabs.tt.player.fable.Economy.queued(q_3, com.oddlabs.tt.model.DeployType.PEON) == 0)
                        {
                            orders.deploy(q_3, com.oddlabs.tt.model.DeployType.PEON, n_2);
                        }
                    }
                    else if (opening_site != null)
                    {
                        if (com.oddlabs.tt.player.fable.Economy.queued(q_3, com.oddlabs.tt.model.DeployType.PEON) == 0)
                        {
                            orders.setRally(q_3, opening_site[0], opening_site[1]);
                            orders.deploy(q_3, com.oddlabs.tt.model.DeployType.PEON, n_2);
                        }
                    }
                }
            }
        }
        public int[] opening_site;
        // Java: 313
        public void manageWeaponQueues(com.oddlabs.tt.model.LandBuilding armory)
        {
            bool rock = ((object)mode == (object)com.oddlabs.tt.player.fable.Economy.Mode.ROCK) || rock_fallback || demands.want_rock_warriors || (com.oddlabs.tt.player.fable.Economy.stock(armory, typeof(com.oddlabs.tt.model.IronSupply)) == 0 && com.oddlabs.tt.player.fable.Economy.stock(armory, typeof(com.oddlabs.tt.model.RockSupply)) >= 1);
            orders.queueWeapons(armory, rock, true, true);
        }
        // Java: 323
        public void allocate(float now, com.oddlabs.tt.player.fable.Roster roster, com.oddlabs.tt.model.LandBuilding armory)
        {
            int W = com.oddlabs.tt.player.fable.Economy.workers(armory);
            int G_w = roster.gatherers_tree;
            int G_i = roster.gatherers_iron;
            int G_k = roster.gatherers_rock;
            int G_r = roster.gatherers_rubber;
            int S_w = com.oddlabs.tt.player.fable.Economy.stock(armory, typeof(com.oddlabs.tt.landscape.TreeSupply));
            int S_i = com.oddlabs.tt.player.fable.Economy.stock(armory, typeof(com.oddlabs.tt.model.IronSupply));
            int S_k = com.oddlabs.tt.player.fable.Economy.stock(armory, typeof(com.oddlabs.tt.model.RockSupply));
            int S_r = com.oddlabs.tt.player.fable.Economy.stock(armory, typeof(com.oddlabs.tt.model.RubberSupply));
            int K_i = com.oddlabs.tt.player.fable.Economy.stock(armory, typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon));
            int K_c = com.oddlabs.tt.player.fable.Economy.stock(armory, typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon));
            int queued_peons = com.oddlabs.tt.player.fable.Economy.queued(armory, com.oddlabs.tt.model.DeployType.PEON) + com.oddlabs.tt.player.fable.Economy.queued(armory, com.oddlabs.tt.model.DeployType.PEON_HARVEST_TREE) + com.oddlabs.tt.player.fable.Economy.queued(armory, com.oddlabs.tt.model.DeployType.PEON_HARVEST_IRON) + com.oddlabs.tt.player.fable.Economy.queued(armory, com.oddlabs.tt.model.DeployType.PEON_HARVEST_ROCK) + com.oddlabs.tt.player.fable.Economy.queued(armory, com.oddlabs.tt.model.DeployType.PEON_HARVEST_RUBBER);
            d_wood_m = meanDistance(armory, com.oddlabs.tt.player.fable.ResourceMap.Kind.TREE, java.lang.Math.max(4, G_w));
            d_iron_m = meanDistance(armory, com.oddlabs.tt.player.fable.ResourceMap.Kind.IRON, java.lang.Math.max(3, G_i));
            int iron_reachable = resources.countAlive(com.oddlabs.tt.player.fable.ResourceMap.Kind.IRON, armory.getGridX(), armory.getGridY(), @params.gather_radius * 2);
            if (((object)mode == (object)com.oddlabs.tt.player.fable.Economy.Mode.IRON) && iron_reachable == 0 && S_i == 0 && com.oddlabs.tt.player.fable.Economy.starved(armory, typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)))
            {
                if (starved_iron_since < 0)
                {
                    starved_iron_since = now;
                }
                else if ((float)(now - starved_iron_since) > 30.0f)
                {
                    mode = com.oddlabs.tt.player.fable.Economy.Mode.ROCK;
                    log.info("economy: iron exhausted -> ROCK mode");
                }
            }
            else
            {
                starved_iron_since = (float)(-1.0f);
                if (((object)mode == (object)com.oddlabs.tt.player.fable.Economy.Mode.ROCK) && iron_reachable >= 6)
                {
                    mode = com.oddlabs.tt.player.fable.Economy.Mode.IRON;
                    log.info("economy: iron available again -> IRON mode");
                }
            }
            int N_a = W + G_w + G_i + G_k + queued_peons;
            float k = (((object)mode == (object)com.oddlabs.tt.player.fable.Economy.Mode.IRON) ? 1.0f : 0.5f);
            float c_w = com.oddlabs.tt.player.fable.Economy.tripCost(d_wood_m);
            float c_main = (((object)mode == (object)com.oddlabs.tt.player.fable.Economy.Mode.IRON) ? com.oddlabs.tt.player.fable.Economy.tripCost(d_iron_m) : com.oddlabs.tt.player.fable.Economy.tripCost(meanDistance(armory, com.oddlabs.tt.player.fable.ResourceMap.Kind.ROCK, 2)));
            rate_star = (float)(N_a / ((float)((float)((float)(80.0f * k) + (float)(2.0f * c_w)) + c_main)));
            int W_star = java.lang.Math.max(@params.workers_min, java.lang.Math.round((float)((float)(80.0f * k) * rate_star)));
            int Gw_star = java.lang.JC.d2i(java.lang.Math.ceil((float)((float)(2.0f * rate_star) * c_w)));
            int Gi_star = (((object)mode == (object)com.oddlabs.tt.player.fable.Economy.Mode.IRON) ? java.lang.JC.d2i(java.lang.Math.ceil((float)(rate_star * c_main))) : 0);
            int Gk_star = (((object)mode == (object)com.oddlabs.tt.player.fable.Economy.Mode.ROCK) ? java.lang.JC.d2i(java.lang.Math.ceil((float)(rate_star * c_main))) : ((S_k < @params.rock_reserve && (S_r > 0 || !hunters.isEmpty() || now < 360.0f || K_c > 0) ? 1 : 0)));
            int T_w = java.lang.Math.clamp(java.lang.Math.round((float)(@params.stock_target_wood_per_rate * rate_star)), @params.stock_wood_min, @params.stock_wood_max);
            int T_i = java.lang.Math.clamp(java.lang.Math.round((float)(@params.stock_target_iron_per_rate * rate_star)), @params.stock_iron_min, @params.stock_iron_max);
            float e_w = (float)((S_w - T_w) / ((float)(T_w)));
            float e_i = (float)((S_i - T_i) / ((float)(T_i)));
            if (e_w > 1.0f)
            {
                Gw_star = java.lang.Math.max(1, Gw_star - java.lang.JC.f2i(e_w));
            }
            else if (e_w < (float)(-0.5f) && com.oddlabs.tt.player.fable.Economy.starved(armory, typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)) && S_w < 2)
            {
                Gw_star += 1;
            }
            if (((object)mode == (object)com.oddlabs.tt.player.fable.Economy.Mode.IRON))
            {
                if (e_i > 1.0f)
                {
                    Gi_star = java.lang.Math.max(1, Gi_star - java.lang.JC.f2i(e_i));
                }
                else if (e_i < (float)(-0.5f) && S_i == 0 && iron_reachable > 0)
                {
                    Gi_star += 1;
                }
            }
            if (S_k >= @params.stock_rock_target * 3)
            {
                Gk_star = 0;
            }
            if (cap_mode)
            {
                W_star = java.lang.Math.max(@params.cap_workers, W_star);
                Gk_star = java.lang.Math.min(Gk_star, 1);
            }
            bool iron_dry = S_i == 0 && (com.oddlabs.tt.player.fable.Economy.starved(armory, typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)) || iron_reachable == 0);
            if (iron_dry && iron_dry_since < 0.0f)
            {
                iron_dry_since = now;
            }
            if (!iron_dry)
            {
                iron_dry_since = (float)(-1.0f);
            }
            rock_fallback = ((object)mode == (object)com.oddlabs.tt.player.fable.Economy.Mode.ROCK) || (iron_dry && (cap_mode || (float)(now - iron_dry_since) > 20.0f) && S_k + G_k * 3 < 30);
            if (rock_fallback && Gk_star < 2)
            {
                Gk_star = java.lang.Math.max(Gk_star, (cap_mode ? java.lang.Math.max(2, Gw_star / 2) : 2));
            }
            workers_star = W_star;
            wood_star = Gw_star;
            iron_star = Gi_star;
            rock_star = Gk_star;
            int conversions = 0;
            if (G_w > Gw_star + 1 && conversions < @params.conversions_per_tick && cooled(now, com.oddlabs.tt.player.fable.ResourceMap.Kind.TREE, false))
            {
                orders.recallGatherers(armory, typeof(com.oddlabs.tt.landscape.TreeSupply), java.lang.Math.min(G_w - Gw_star, 3));
                last_conversion_wood = now;
                conversions++;
            }
            if (G_i > Gi_star + 1 && conversions < @params.conversions_per_tick && cooled(now, com.oddlabs.tt.player.fable.ResourceMap.Kind.IRON, false))
            {
                orders.recallGatherers(armory, typeof(com.oddlabs.tt.model.IronSupply), java.lang.Math.min(G_i - Gi_star, 3));
                last_conversion_iron = now;
                conversions++;
            }
            if (G_k > Gk_star && conversions < @params.conversions_per_tick && cooled(now, com.oddlabs.tt.player.fable.ResourceMap.Kind.ROCK, false))
            {
                orders.recallGatherers(armory, typeof(com.oddlabs.tt.model.RockSupply), G_k - Gk_star);
                last_conversion_rock = now;
                conversions++;
            }
            int spare = W - @params.workers_min;
            bool far_iron = d_iron_m > @params.far_iron_m;
            if (spare > 0 && conversions < @params.conversions_per_tick && rock_fallback && G_k < Gk_star && cooled(now, com.oddlabs.tt.player.fable.ResourceMap.Kind.ROCK, S_k == 0))
            {
                int n = java.lang.Math.min(Gk_star - G_k, java.lang.Math.min(spare, 4));
                orders.deploy(armory, com.oddlabs.tt.model.DeployType.PEON_HARVEST_ROCK, n);
                last_conversion_rock = now;
                conversions++;
                spare -= n;
                log.info(new java.util.function.Supplier_L(() =>
                {
                    return "economy: +" + (n) + " rock gatherers (iron dry)";
                }));
            }
            if (spare > 0 && conversions < @params.conversions_per_tick && W > W_star + @params.workers_surplus_max)
            {
                int n_2 = java.lang.Math.min(W - W_star - @params.workers_surplus_max / 2, 6);
                bool wood = S_w < @params.stock_wood_max * 4 && G_w < Gw_star + 10;
                orders.deploy(armory, (wood ? com.oddlabs.tt.model.DeployType.PEON_HARVEST_TREE : com.oddlabs.tt.model.DeployType.PEON_HARVEST_ROCK), n_2);
                if (wood)
                {
                    last_conversion_wood = now;
                }
                else
                {
                    last_conversion_rock = now;
                }
                conversions++;
                spare -= n_2;
                int fw = W;
                int fws = W_star;
                log.info(new java.util.function.Supplier_L(() =>
                {
                    return "economy: " + (n_2) + " surplus workers out to " + java.lang.JStr.valueOf(((wood ? "wood" : "rock"))) + " (W=" + (fw) + " W*=" + (fws) + ")";
                }));
            }
            if (spare > 0 && conversions < @params.conversions_per_tick)
            {
                bool starved_w = S_w < 2;
                bool starved_i = ((object)mode == (object)com.oddlabs.tt.player.fable.Economy.Mode.IRON) && S_i == 0;
                if (G_i + pending_iron < Gi_star && cooled(now, com.oddlabs.tt.player.fable.ResourceMap.Kind.IRON, starved_i) && iron_reachable > 0)
                {
                    int n_3 = java.lang.Math.min(Gi_star - G_i - pending_iron, java.lang.Math.min(spare, 4));
                    if (far_iron)
                    {
                        orders.setRally(armory, armory.getGridX() + 5, armory.getGridY() + 5);
                        orders.deploy(armory, com.oddlabs.tt.model.DeployType.PEON, n_3);
                        pending_iron += n_3;
                        pending_iron_since = now;
                    }
                    else
                    {
                        orders.deploy(armory, com.oddlabs.tt.model.DeployType.PEON_HARVEST_IRON, n_3);
                    }
                    int nn = n_3;
                    log.info(new java.util.function.Supplier_L(() =>
                    {
                        return "economy: +" + (nn) + " iron gatherers (" + java.lang.JStr.valueOf(((far_iron ? "assigned" : "engine"))) + ")";
                    }));
                    last_conversion_iron = now;
                    conversions++;
                    spare -= n_3;
                }
                else if (G_w < Gw_star && cooled(now, com.oddlabs.tt.player.fable.ResourceMap.Kind.TREE, starved_w))
                {
                    int n_4 = java.lang.Math.min(Gw_star - G_w, java.lang.Math.min(spare, 5));
                    orders.deploy(armory, com.oddlabs.tt.model.DeployType.PEON_HARVEST_TREE, n_4);
                    last_conversion_wood = now;
                    conversions++;
                    spare -= n_4;
                }
                else if (G_k < Gk_star && cooled(now, com.oddlabs.tt.player.fable.ResourceMap.Kind.ROCK, ((object)mode == (object)com.oddlabs.tt.player.fable.Economy.Mode.ROCK) && S_k == 0))
                {
                    int n_5 = java.lang.Math.min(Gk_star - G_k, java.lang.Math.min(spare, 2));
                    orders.deploy(armory, com.oddlabs.tt.model.DeployType.PEON_HARVEST_ROCK, n_5);
                    last_conversion_rock = now;
                    conversions++;
                }
            }
            if ((float)(now - pending_iron_since) > 20.0f)
            {
                pending_iron = 0;
            }
            int supply_orders = 0;
            foreach (com.oddlabs.tt.model.Unit p in java.lang.JIter.of(roster.peons))
            {
                if (!orders.usable(p) || ((object)com.oddlabs.tt.player.fable.Roster.activity(p) != (object)com.oddlabs.tt.player.fable.Roster.Activity.IDLE))
                {
                    continue;
                }
                if (jobs.has(p) && ((object)jobs.kindOf(p) != (object)com.oddlabs.tt.player.fable.Jobs.Kind.NONE) && (float)(now - jobs.get(p).issued_at) < 20.0f)
                {
                    continue;
                }
                if (hunters.contains(p) || reserved.contains(p))
                {
                    continue;
                }
                if (far_iron && G_i < Gi_star && supply_orders < @params.max_supply_orders_per_tick && com.oddlabs.tt.player.fable.BasePlan.dist2(p.getGridX(), p.getGridY(), armory.getGridX(), armory.getGridY()) <= 30 * 30)
                {
                    com.oddlabs.tt.player.fable.ResourceMap.Node node = pickIronNode(armory, roster);
                    if (node != null)
                    {
                        com.oddlabs.tt.model.Supply s = node.live(player.getWorld().getUnitGrid());
                        if (s != null)
                        {
                            orders.gather(p, s);
                            jobs.set(p, com.oddlabs.tt.player.fable.Jobs.Kind.GATHER, node.grid_x, node.grid_y, null, node, now, 0);
                            supply_orders++;
                            G_i++;
                            if (pending_iron > 0)
                            {
                                pending_iron--;
                            }
                            continue;
                        }
                    }
                }
                com.oddlabs.tt.model.LandBuilding into = nearestArmory(roster, armory, p.getGridX(), p.getGridY());
                orders.enter(p, into);
                jobs.set(p, com.oddlabs.tt.player.fable.Jobs.Kind.ENTER, into.getGridX(), into.getGridY(), into, null, now, 0);
            }
            if (!demands.hot_cells.isEmpty())
            {
                int r2 = demands.hot_radius * demands.hot_radius;
                foreach (com.oddlabs.tt.model.Unit p_2 in java.lang.JIter.of(roster.peons))
                {
                    if (!orders.usable(p_2))
                    {
                        continue;
                    }
                    com.oddlabs.tt.player.fable.Roster.Activity a = com.oddlabs.tt.player.fable.Roster.activity(p_2);
                    if (((object)a != (object)com.oddlabs.tt.player.fable.Roster.Activity.GATHER_TREE) && ((object)a != (object)com.oddlabs.tt.player.fable.Roster.Activity.GATHER_IRON) && ((object)a != (object)com.oddlabs.tt.player.fable.Roster.Activity.GATHER_ROCK) && ((object)a != (object)com.oddlabs.tt.player.fable.Roster.Activity.GATHER_RUBBER))
                    {
                        continue;
                    }
                    foreach (int[] c in java.lang.JIter.of(demands.hot_cells))
                    {
                        if (com.oddlabs.tt.player.fable.BasePlan.dist2(p_2.getGridX(), p_2.getGridY(), c[0], c[1]) <= r2)
                        {
                            com.oddlabs.tt.model.LandBuilding into_2 = nearestArmory(roster, armory, p_2.getGridX(), p_2.getGridY());
                            orders.enter(p_2, into_2);
                            jobs.set(p_2, com.oddlabs.tt.player.fable.Jobs.Kind.ENTER, into_2.getGridX(), into_2.getGridY(), into_2, null, now, 1);
                            break;
                        }
                    }
                }
            }
            if (demands.armory_threat && armory.getHitPoints() < 80 && W > 0 && !roster.quarters.isEmpty())
            {
                com.oddlabs.tt.model.LandBuilding rear = ((com.oddlabs.tt.model.LandBuilding)roster.quarters.getFirst());
                int best_d2 = (-1);
                foreach (com.oddlabs.tt.model.LandBuilding q in java.lang.JIter.of(roster.quarters))
                {
                    int d2 = com.oddlabs.tt.player.fable.BasePlan.dist2(q.getGridX(), q.getGridY(), armory.getGridX(), armory.getGridY());
                    if (d2 > best_d2)
                    {
                        best_d2 = d2;
                        rear = q;
                    }
                }
                orders.setRally(armory, rear);
                orders.deploy(armory, com.oddlabs.tt.model.DeployType.PEON, W);
                log.info("economy: evacuating armory workers");
            }
        }
        public int pending_iron;
        public float pending_iron_since;
        public float iron_dry_since;
        public bool rock_fallback;
        // Java: 577
        public com.oddlabs.tt.player.fable.ResourceMap.Node pickIronNode(com.oddlabs.tt.model.LandBuilding armory, com.oddlabs.tt.player.fable.Roster roster)
        {
            java.util.List nodes = new java.util.ArrayList(resources.nearestAlive(com.oddlabs.tt.player.fable.ResourceMap.Kind.IRON, armory.getGridX(), armory.getGridY(), @params.gather_radius * 2, 12));
            int ax = armory.getGridX();
            int ay = armory.getGridY();
            nodes.sort(new java.util.Comparator_L((object a_l, object b_l) =>
            {
                com.oddlabs.tt.player.fable.ResourceMap.Node a = (com.oddlabs.tt.player.fable.ResourceMap.Node)a_l;
                com.oddlabs.tt.player.fable.ResourceMap.Node b = (com.oddlabs.tt.player.fable.ResourceMap.Node)b_l;
                int c = java.lang.Float.compare(plan.walk(ax, ay, a.grid_x, a.grid_y), plan.walk(ax, ay, b.grid_x, b.grid_y));
                if (c != 0)
                {
                    return c;
                }
                c = java.lang.Integer.compare(a.grid_x, b.grid_x);
                return (c != 0 ? c : java.lang.Integer.compare(a.grid_y, b.grid_y));
            }));
            com.oddlabs.tt.player.fable.ResourceMap.Node fallback = null;
            foreach (com.oddlabs.tt.player.fable.ResourceMap.Node n in java.lang.JIter.of(nodes))
            {
                if (underEnemyTower(n.grid_x, n.grid_y) || contested(n.grid_x, n.grid_y))
                {
                    continue;
                }
                if (fallback == null)
                {
                    fallback = n;
                }
                if (jobs.gatherersOn(n) < @params.max_peons_per_node)
                {
                    return n;
                }
            }
            return fallback;
        }
        // Java: 602
        public bool contested(int gx, int gy)
        {
            return last_intel != null && last_intel.warriorsNear(gx, gy, 25) >= 2;
        }
        // Java: 607
        public bool underEnemyTower(int gx, int gy)
        {
            if (last_intel == null)
            {
                return false;
            }
            foreach (com.oddlabs.tt.model.LandBuilding t in java.lang.JIter.of(last_intel.manned_towers))
            {
                if (com.oddlabs.tt.player.fable.BasePlan.dist2(t.getGridX(), t.getGridY(), gx, gy) <= 20 * 20)
                {
                    return true;
                }
            }
            return false;
        }
        public com.oddlabs.tt.player.fable.Intel last_intel;
        // Java: 618
        public bool cooled(float now, com.oddlabs.tt.player.fable.ResourceMap.Kind kind, bool starved_v)
        {
            if (starved_v)
            {
                return true;
            }
            float last = ((kind).ordinal() switch { 0 /*TREE*/ => (float)(last_conversion_wood), 2 /*IRON*/ => (float)(last_conversion_iron), 1 /*ROCK*/ => (float)(last_conversion_rock), _ => throw new System.InvalidOperationException("MatchException") });
            return (float)(now - last) >= @params.conversion_cooldown;
        }
        // Java: 636
        public bool deployWarriors(com.oddlabs.tt.player.fable.Roster roster)
        {
            bool any = false;
            foreach (com.oddlabs.tt.model.LandBuilding a in java.lang.JIter.of(roster.armories))
            {
                if (orders.complete(a) && deployWarriorsFrom(a))
                {
                    any = true;
                }
            }
            return any;
        }
        // Java: 644
        public bool deployWarriorsFrom(com.oddlabs.tt.model.LandBuilding armory)
        {
            int W = com.oddlabs.tt.player.fable.Economy.workers(armory);
            int K_c = com.oddlabs.tt.player.fable.Economy.stock(armory, typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon));
            int K_i = com.oddlabs.tt.player.fable.Economy.stock(armory, typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon));
            int K_k = com.oddlabs.tt.player.fable.Economy.stock(armory, typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon));
            int q = com.oddlabs.tt.player.fable.Economy.queued(armory, com.oddlabs.tt.model.DeployType.RUBBER_WARRIOR) + com.oddlabs.tt.player.fable.Economy.queued(armory, com.oddlabs.tt.model.DeployType.IRON_WARRIOR) + com.oddlabs.tt.player.fable.Economy.queued(armory, com.oddlabs.tt.model.DeployType.ROCK_WARRIOR);
            bool rock_ok = ((object)mode == (object)com.oddlabs.tt.player.fable.Economy.Mode.ROCK) || rock_fallback || demands.emergency || demands.want_rock_warriors || (K_i == 0 && K_c == 0);
            if (demands.emergency)
            {
                int spare = java.lang.Math.max(0, W - 1);
                if (spare == 0 || q > 0)
                {
                    return false;
                }
                int n = java.lang.Math.min(spare, K_c);
                if (n > 0)
                {
                    orders.deploy(armory, com.oddlabs.tt.model.DeployType.RUBBER_WARRIOR, n);
                    spare -= n;
                }
                n = java.lang.Math.min(spare, K_i);
                if (n > 0)
                {
                    orders.deploy(armory, com.oddlabs.tt.model.DeployType.IRON_WARRIOR, n);
                    spare -= n;
                }
                n = java.lang.Math.min(spare, K_k);
                if (n > 0)
                {
                    orders.deploy(armory, com.oddlabs.tt.model.DeployType.ROCK_WARRIOR, n);
                }
                return true;
            }
            int min_workers = @params.workers_min;
            if (cap_mode)
            {
                if (q > 1 || W <= @params.cap_workers)
                {
                    return false;
                }
            }
            else if (q > 0 || W < min_workers + 1)
            {
                return false;
            }
            if (K_c > 0)
            {
                orders.deploy(armory, com.oddlabs.tt.model.DeployType.RUBBER_WARRIOR, 1);
                return true;
            }
            if (K_i > 0)
            {
                orders.deploy(armory, com.oddlabs.tt.model.DeployType.IRON_WARRIOR, 1);
                return true;
            }
            if (K_k > 0 && rock_ok)
            {
                orders.deploy(armory, com.oddlabs.tt.model.DeployType.ROCK_WARRIOR, 1);
                return true;
            }
            return false;
        }
        // Java: 701
        public java.util.List requestCrew(int n, int gx, int gy, com.oddlabs.tt.player.fable.Roster roster, float now, bool allow_workers)
        {
            java.util.List result = new java.util.ArrayList();
            if (n <= 0)
            {
                return result;
            }
            java.util.List candidates = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit p in java.lang.JIter.of(roster.peons))
            {
                if (!orders.usable(p) || hunters.contains(p) || reserved.contains(p))
                {
                    continue;
                }
                com.oddlabs.tt.player.fable.Roster.Activity a = com.oddlabs.tt.player.fable.Roster.activity(p);
                com.oddlabs.tt.player.fable.Jobs.Kind kind = jobs.kindOf(p);
                if (((object)kind == (object)com.oddlabs.tt.player.fable.Jobs.Kind.BUILD) || ((object)kind == (object)com.oddlabs.tt.player.fable.Jobs.Kind.HOLD))
                {
                    continue;
                }
                if (((object)a == (object)com.oddlabs.tt.player.fable.Roster.Activity.IDLE) || ((object)a == (object)com.oddlabs.tt.player.fable.Roster.Activity.WALK) || ((object)a == (object)com.oddlabs.tt.player.fable.Roster.Activity.ENTER) || ((object)a == (object)com.oddlabs.tt.player.fable.Roster.Activity.GATHER_TREE) || ((object)a == (object)com.oddlabs.tt.player.fable.Roster.Activity.GATHER_ROCK))
                {
                    candidates.add(p);
                }
            }
            candidates.sort(new java.util.Comparator_L((object u_l, object v_l) =>
            {
                com.oddlabs.tt.model.Unit u = (com.oddlabs.tt.model.Unit)u_l;
                com.oddlabs.tt.model.Unit v = (com.oddlabs.tt.model.Unit)v_l;
                int ru = com.oddlabs.tt.player.fable.Economy.rank(u);
                int rv = com.oddlabs.tt.player.fable.Economy.rank(v);
                if (ru != rv)
                {
                    return java.lang.Integer.compare(ru, rv);
                }
                return java.lang.Integer.compare(com.oddlabs.tt.player.fable.BasePlan.dist2(u.getGridX(), u.getGridY(), gx, gy), com.oddlabs.tt.player.fable.BasePlan.dist2(v.getGridX(), v.getGridY(), gx, gy));
            }));
            foreach (com.oddlabs.tt.model.Unit p_2 in java.lang.JIter.of(candidates))
            {
                if (result.size() >= n)
                {
                    break;
                }
                result.add(p_2);
            }
            if (result.size() < n && allow_workers)
            {
                com.oddlabs.tt.model.LandBuilding armory = roster.mainArmory();
                if (armory != null)
                {
                    int spare = com.oddlabs.tt.player.fable.Economy.workers(armory) - @params.workers_min - 2;
                    int deploy = java.lang.Math.min(spare, n - result.size());
                    if (deploy > 0 && com.oddlabs.tt.player.fable.Economy.queued(armory, com.oddlabs.tt.model.DeployType.PEON) == 0)
                    {
                        orders.setRally(armory, gx, gy);
                        orders.deploy(armory, com.oddlabs.tt.model.DeployType.PEON, deploy);
                        log.info(new java.util.function.Supplier_L(() =>
                        {
                            return "economy: deploying " + (deploy) + " workers as builders toward " + (gx) + "," + (gy);
                        }));
                    }
                }
            }
            return result;
        }
        // Java: 746
        public static int rank(com.oddlabs.tt.model.Unit u)
        {
            return ((com.oddlabs.tt.player.fable.Roster.activity(u)).ordinal() switch { 0 /*IDLE*/ => (int)(0), 5 /*WALK*/ or 11 /*ENTER*/ => (int)(1), 1 /*GATHER_TREE*/ => (int)(2), 2 /*GATHER_ROCK*/ => (int)(3), _ => (int)(4) });
        }
        // Java: 758
        public void hunts(float now, com.oddlabs.tt.player.fable.Roster roster, com.oddlabs.tt.player.fable.Intel intel, com.oddlabs.tt.model.LandBuilding armory)
        {
            hunters.removeIf(new java.util.function.Predicate_L((object u_l) =>
            {
                com.oddlabs.tt.model.Unit u = (com.oddlabs.tt.model.Unit)u_l;
                return !orders.usable(u) || ((object)com.oddlabs.tt.player.fable.Roster.activity(u) == (object)com.oddlabs.tt.player.fable.Roster.Activity.IDLE) || ((object)com.oddlabs.tt.player.fable.Roster.activity(u) == (object)com.oddlabs.tt.player.fable.Roster.Activity.ENTER);
            }));
            if ((float)(now - last_hunt_check) < 5.0f)
            {
                return;
            }
            last_hunt_check = now;
            if (now < @params.hunt_min_time || demands.emergency)
            {
                return;
            }
            int S_r = com.oddlabs.tt.player.fable.Economy.stock(armory, typeof(com.oddlabs.tt.model.RubberSupply));
            int K_c = com.oddlabs.tt.player.fable.Economy.stock(armory, typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon));
            if (S_r + K_c >= 4 || hunters.size() >= @params.hunt_max_peons)
            {
                return;
            }
            java.util.List chickens = resources.findChickens();
            if (chickens.isEmpty())
            {
                return;
            }
            int ax = armory.getGridX();
            int ay = armory.getGridY();
            int max_d2 = @params.hunt_max_dist * @params.hunt_max_dist;
            java.util.List targets = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.RubberSupply c in java.lang.JIter.of(chickens))
            {
                int d2 = com.oddlabs.tt.player.fable.BasePlan.dist2(c.getGridX(), c.getGridY(), ax, ay);
                if (d2 > max_d2)
                {
                    continue;
                }
                com.oddlabs.tt.model.LandBuilding enemy_armory = (intel.armories.isEmpty() ? (com.oddlabs.tt.model.LandBuilding)(null) : ((com.oddlabs.tt.model.LandBuilding)intel.armories.getFirst()));
                if (enemy_armory != null && (float)(com.oddlabs.tt.player.fable.BasePlan.dist2(c.getGridX(), c.getGridY(), enemy_armory.getGridX(), enemy_armory.getGridY()) * 1.4f) < d2)
                {
                    continue;
                }
                if (intel.warriorsNear(c.getGridX(), c.getGridY(), 30) > 0)
                {
                    continue;
                }
                targets.add(c);
            }
            if (targets.isEmpty())
            {
                return;
            }
            int want = java.lang.Math.min(@params.hunt_max_peons - hunters.size(), targets.size());
            java.util.List pool = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit p in java.lang.JIter.of(roster.peons))
            {
                if (orders.usable(p) && !hunters.contains(p) && !reserved.contains(p) && ((object)jobs.kindOf(p) != (object)com.oddlabs.tt.player.fable.Jobs.Kind.BUILD) && (((object)com.oddlabs.tt.player.fable.Roster.activity(p) == (object)com.oddlabs.tt.player.fable.Roster.Activity.GATHER_TREE) || ((object)com.oddlabs.tt.player.fable.Roster.activity(p) == (object)com.oddlabs.tt.player.fable.Roster.Activity.IDLE)))
                {
                    pool.add(p);
                }
            }
            com.oddlabs.tt.model.RubberSupply first = ((com.oddlabs.tt.model.RubberSupply)targets.getFirst());
            pool.sort(new java.util.Comparator_L((object u_2_l, object v_l) =>
            {
                com.oddlabs.tt.model.Unit u_2 = (com.oddlabs.tt.model.Unit)u_2_l;
                com.oddlabs.tt.model.Unit v = (com.oddlabs.tt.model.Unit)v_l;
                return java.lang.Integer.compare(com.oddlabs.tt.player.fable.BasePlan.dist2(u_2.getGridX(), u_2.getGridY(), first.getGridX(), first.getGridY()), com.oddlabs.tt.player.fable.BasePlan.dist2(v.getGridX(), v.getGridY(), first.getGridX(), first.getGridY()));
            }));
            int sent = 0;
            for (int i = 0; i < pool.size() && sent < want; i++)
            {
                com.oddlabs.tt.model.Unit p_2 = ((com.oddlabs.tt.model.Unit)pool.get(i));
                com.oddlabs.tt.model.RubberSupply c_2 = ((com.oddlabs.tt.model.RubberSupply)targets.get(sent));
                if (c_2.isDead() || c_2.isEmpty())
                {
                    continue;
                }
                orders.gather(p_2, c_2);
                jobs.set(p_2, com.oddlabs.tt.player.fable.Jobs.Kind.GATHER, c_2.getGridX(), c_2.getGridY(), null, null, now, 9);
                hunters.add(p_2);
                sent++;
            }
            if (sent > 0)
            {
                int n = sent;
                log.info(new java.util.function.Supplier_L(() =>
                {
                    return "economy: " + (n) + " peons hunting chickens near " + (first.getGridX()) + "," + (first.getGridY());
                }));
            }
        }
        // Java: 822
        public int hunterCount()
        {
            return hunters.size();
        }
        // Java: 826
        public bool isCapMode()
        {
            return cap_mode;
        }
        // Java: 831
        public void parkIdlePeons(float now, com.oddlabs.tt.player.fable.Roster roster)
        {
            if (roster.quarters.isEmpty())
            {
                return;
            }
            foreach (com.oddlabs.tt.model.Unit p in java.lang.JIter.of(roster.peons))
            {
                if (!orders.usable(p) || ((object)com.oddlabs.tt.player.fable.Roster.activity(p) != (object)com.oddlabs.tt.player.fable.Roster.Activity.IDLE) || ((object)jobs.kindOf(p) == (object)com.oddlabs.tt.player.fable.Jobs.Kind.BUILD) || reserved.contains(p))
                {
                    continue;
                }
                com.oddlabs.tt.model.LandBuilding q = com.oddlabs.tt.player.fable.Economy.nearestQuarters(roster, p.getGridX(), p.getGridY());
                if (q != null)
                {
                    orders.enter(p, q);
                    jobs.set(p, com.oddlabs.tt.player.fable.Jobs.Kind.ENTER, q.getGridX(), q.getGridY(), q, null, now, 0);
                }
            }
        }
        // Java: 846
        public static com.oddlabs.tt.model.LandBuilding nearestQuarters(com.oddlabs.tt.player.fable.Roster roster, int gx, int gy)
        {
            com.oddlabs.tt.model.LandBuilding best = null;
            int best_d2 = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.LandBuilding q in java.lang.JIter.of(roster.quarters))
            {
                int d2 = com.oddlabs.tt.player.fable.BasePlan.dist2(q.getGridX(), q.getGridY(), gx, gy);
                if (d2 < best_d2)
                {
                    best_d2 = d2;
                    best = q;
                }
            }
            return best;
        }
        // Java: 860
        public void fillQuarters(com.oddlabs.tt.model.LandBuilding q, int wanted, com.oddlabs.tt.player.fable.Roster roster, float now)
        {
            int inside = com.oddlabs.tt.player.fable.Roster.unitsInside(q);
            if (inside >= wanted)
            {
                return;
            }
            int need = wanted - inside;
            com.oddlabs.tt.model.LandBuilding armory = roster.mainArmory();
            if (armory != null && com.oddlabs.tt.player.fable.Economy.workers(armory) > @params.workers_min + need && com.oddlabs.tt.player.fable.Economy.queued(armory, com.oddlabs.tt.model.DeployType.PEON) == 0)
            {
                orders.setRally(armory, q);
                orders.deploy(armory, com.oddlabs.tt.model.DeployType.PEON, need);
                return;
            }
            java.util.List pool = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit p in java.lang.JIter.of(roster.peons))
            {
                if (orders.usable(p) && ((object)jobs.kindOf(p) != (object)com.oddlabs.tt.player.fable.Jobs.Kind.BUILD) && !reserved.contains(p) && (((object)com.oddlabs.tt.player.fable.Roster.activity(p) == (object)com.oddlabs.tt.player.fable.Roster.Activity.IDLE) || ((object)com.oddlabs.tt.player.fable.Roster.activity(p) == (object)com.oddlabs.tt.player.fable.Roster.Activity.GATHER_TREE)))
                {
                    pool.add(p);
                }
            }
            pool.sort(new java.util.Comparator_L((object u_l, object v_l) =>
            {
                com.oddlabs.tt.model.Unit u = (com.oddlabs.tt.model.Unit)u_l;
                com.oddlabs.tt.model.Unit v = (com.oddlabs.tt.model.Unit)v_l;
                return java.lang.Integer.compare(com.oddlabs.tt.player.fable.BasePlan.dist2(u.getGridX(), u.getGridY(), q.getGridX(), q.getGridY()), com.oddlabs.tt.player.fable.BasePlan.dist2(v.getGridX(), v.getGridY(), q.getGridX(), q.getGridY()));
            }));
            for (int i = 0; i < pool.size() && i < need; i++)
            {
                orders.enter(((com.oddlabs.tt.model.Unit)pool.get(i)), q);
                jobs.set(((com.oddlabs.tt.model.Unit)pool.get(i)), com.oddlabs.tt.player.fable.Jobs.Kind.ENTER, q.getGridX(), q.getGridY(), q, null, now, 0);
            }
        }
        // Java: 885
        public string status(com.oddlabs.tt.player.fable.Roster roster)
        {
            com.oddlabs.tt.model.LandBuilding a = roster.mainArmory();
            if (a == null)
            {
                return "no armory";
            }
            string s = java.lang.JString.format("%s R*=%.2f W=%d/%d wood=%d/%d iron=%d/%d rock=%d/%d stock w%d i%d k%d r%d wpn i%d c%d d_w=%.0fm d_i=%.0fm", mode, rate_star, com.oddlabs.tt.player.fable.Economy.workers(a), workers_star, roster.gatherers_tree, wood_star, roster.gatherers_iron, iron_star, roster.gatherers_rock, rock_star, com.oddlabs.tt.player.fable.Economy.stock(a, typeof(com.oddlabs.tt.landscape.TreeSupply)), com.oddlabs.tt.player.fable.Economy.stock(a, typeof(com.oddlabs.tt.model.IronSupply)), com.oddlabs.tt.player.fable.Economy.stock(a, typeof(com.oddlabs.tt.model.RockSupply)), com.oddlabs.tt.player.fable.Economy.stock(a, typeof(com.oddlabs.tt.model.RubberSupply)), com.oddlabs.tt.player.fable.Economy.stock(a, typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)), com.oddlabs.tt.player.fable.Economy.stock(a, typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)), d_wood_m, d_iron_m);
            com.oddlabs.tt.model.LandBuilding a2 = com.oddlabs.tt.player.fable.Economy.expansion(roster);
            if (a2 != null && orders.complete(a2))
            {
                s += java.lang.JStr.valueOf(java.lang.JString.format(" | A2 W=%d stock w%d i%d wpn i%d", com.oddlabs.tt.player.fable.Economy.workers(a2), com.oddlabs.tt.player.fable.Economy.stock(a2, typeof(com.oddlabs.tt.landscape.TreeSupply)), com.oddlabs.tt.player.fable.Economy.stock(a2, typeof(com.oddlabs.tt.model.IronSupply)), com.oddlabs.tt.player.fable.Economy.stock(a2, typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon))));
            }
            return s;
        }
        private void init_Economy()
        {
            this.demands = new com.oddlabs.tt.player.fable.Economy.Demands();
            this.mode = com.oddlabs.tt.player.fable.Economy.Mode.IRON;
            this.last_conversion_wood = (float)(-100.0f);
            this.last_conversion_iron = (float)(-100.0f);
            this.last_conversion_rock = (float)(-100.0f);
            this.last_conversion_rubber = (float)(-100.0f);
            this.last_hunt_check = (float)(-100.0f);
            this.hunters = new java.util.ArrayList();
            this.reserved = new java.util.LinkedHashSet();
            this.quarters_rally_set_count = (-1);
            this.bootstrap_until = (float)(-1.0f);
            this.starved_iron_since = (float)(-1.0f);
            this.last_a2_iron = (float)(-100.0f);
            this.last_a2_wood = (float)(-100.0f);
            this.last_a2_transfer = (float)(-100.0f);
            this.pending_iron_since = (float)(-100.0f);
            this.iron_dry_since = (float)(-1.0f);
        }
        public Economy(com.oddlabs.tt.player.Player player, com.oddlabs.tt.player.fable.Orders orders, com.oddlabs.tt.player.fable.Jobs jobs, com.oddlabs.tt.player.fable.ResourceMap resources, com.oddlabs.tt.player.fable.BasePlan plan, com.oddlabs.tt.player.fable.Params @params, com.oddlabs.tt.player.fable.AiLog log) : base()
        {
            init_Economy();
            this.player = player;
            this.orders = orders;
            this.jobs = jobs;
            this.resources = resources;
            this.plan = plan;
            this.@params = @params;
            this.log = log;
        }
    }
}
