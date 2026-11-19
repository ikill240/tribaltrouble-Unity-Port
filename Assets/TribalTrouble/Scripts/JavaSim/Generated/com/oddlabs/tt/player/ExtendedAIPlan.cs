// Translated from ExtendedAIPlan.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player
{
    // Java: 42
    public class ExtendedAIPlan : java.lang.Object, com.oddlabs.tt.animation.Animated
    {
        public const float PERIOD = 4.0f;
        public const float RARE_TIME = 150.0f;
        public const float HALL_TIME = 540.0f;
        public const float TIER_TIME = 420.0f;
        public const float IRON_TIME = 840.0f;
        public const float UNIT_TIME = 600.0f;
        public const int SCAN_RADIUS = 45;
        public com.oddlabs.tt.player.Player owner;
        public bool vikings;
        public bool sea;
        public int[] near;
        public float time;
        public float next_tick;
        public float next_scan;
        public float next_hall;
        public com.oddlabs.tt.model.LandBuilding hall_site;
        // Java: 68
        public void animate(float t)
        {
            time += t;
            if (time < next_tick)
            {
                return;
            }
            next_tick = (float)(time + com.oddlabs.tt.player.ExtendedAIPlan.PERIOD);
            if (!owner.isAlive())
            {
                owner.getWorld().getAnimationManagerGameTime().removeAnimation(this);
                return;
            }
            try
            {
                tick();
            }
            catch (System.Exception e)
            {
            }
        }
        // Java: 85
        public void tick()
        {
            if (time >= next_scan)
            {
                next_scan = (float)(time + 120.0f);
                scanDeposits();
            }
            com.oddlabs.tt.model.LandBuilding armory_v = armory();
            if (armory_v == null)
            {
                return;
            }
            bool saving = sea && saveForFleet(armory_v);
            if (time >= com.oddlabs.tt.player.ExtendedAIPlan.RARE_TIME)
            {
                gatherRare(armory_v);
            }
            if (time >= next_hall)
            {
                buildHall(armory_v);
            }
            if (!saving && time >= com.oddlabs.tt.player.ExtendedAIPlan.TIER_TIME)
            {
                upgradeBuildings();
            }
            if (time >= com.oddlabs.tt.player.ExtendedAIPlan.UNIT_TIME)
            {
                trainRaceUnits(armory_v);
            }
            if (!saving && time >= com.oddlabs.tt.player.ExtendedAIPlan.FORGE_TIME && hasHall() && time >= next_site[com.oddlabs.tt.model.Race.BUILDING_FORGE])
            {
                buildNear(armory_v, com.oddlabs.tt.model.Race.BUILDING_FORGE);
            }
            if (!saving && time >= com.oddlabs.tt.player.ExtendedAIPlan.TOTEM_TIME && time >= next_site[com.oddlabs.tt.model.Race.BUILDING_TOTEM])
            {
                buildNear(armory_v, com.oddlabs.tt.model.Race.BUILDING_TOTEM);
            }
            if (!saving && time >= com.oddlabs.tt.player.ExtendedAIPlan.FORGE_TIME)
            {
                arsenal(armory_v);
            }
            if (sea && time >= com.oddlabs.tt.player.ExtendedAIPlan.DOCK_TIME && time >= next_site[com.oddlabs.tt.model.Race.BUILDING_DOCK])
            {
                buildNear(armory_v, com.oddlabs.tt.model.Race.BUILDING_DOCK);
            }
            research(saving);
        }
        public const float FORGE_TIME = 660.0f;
        public const float TOTEM_TIME = 780.0f;
        public const float MINOR_GOD_TIME = 900.0f;
        public const float MAJOR_GOD_TIME = 1200.0f;
        public const float ELITE_TIME = 960.0f;
        public const float RUNE_TIME = 1500.0f;
        public const float DOCK_TIME = 360.0f;
        public static com.oddlabs.tt.model.DeployType[] DOCK_RESEARCH;
        public float[] next_site;
        public com.oddlabs.tt.model.LandBuilding[] sites;
        // Java: 126
        public bool has(int id)
        {
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(owner.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Building b) && !b.isDead() && ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID() == id)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 134
        public void buildNear(com.oddlabs.tt.model.LandBuilding armory_v, int id)
        {
            next_site[id] = (float)(time + 30.0f);
            com.oddlabs.tt.model.LandBuilding site = sites[id];
            if (site != null && !site.isDead() && site.isPlaced())
            {
                if (!site.isComplete())
                {
                    sendBuilders(site);
                }
                return;
            }
            if (has(id) && (site == null || site.isDead() || site.isPlaced()))
            {
                return;
            }
            if (!owner.canBuild(id))
            {
                return;
            }
            com.oddlabs.tt.model.BuildingTemplate template = owner.getRace().getBuildingTemplate(id);
            com.oddlabs.tt.pathfinder.UnitGrid grid = owner.getWorld().getUnitGrid();
            if (site == null || site.isDead())
            {
                site = null;
                int ax = armory_v.getGridX();
                int ay = armory_v.getGridY();
                for (int r = 7; r <= 24; r++)
                {
                    for (int i = 0; i < 8 * r; i++)
                    {
                        int side = i / (2 * r);
                        int d = i % (2 * r);
                        int x = (side == 0 ? ax - r + d : (side == 1 ? ax + r : (side == 2 ? ax + r - d : ax - r)));
                        int y = (side == 0 ? ay - r : (side == 1 ? ay - r + d : (side == 2 ? ay + r : ay + r - d)));
                        if (x < 4 || y < 4 || x >= grid.getGridSize() - 4 || y >= grid.getGridSize() - 4)
                        {
                            continue;
                        }
                        if (grid.getRegion(x, y) == null || !com.oddlabs.tt.model.LandBuilding.isPlacingLegal(grid, template, x, y))
                        {
                            continue;
                        }
                        site = ((com.oddlabs.tt.model.LandBuilding)template.create(owner, x, y));
                        goto brk_search_7;
                    }
                }
                brk_search_7: ;
                sites[id] = site;
                if (site == null)
                {
                    return;
                }
            }
            sendBuilders(site);
        }
        // Java: 175
        public void sendBuilders(com.oddlabs.tt.model.LandBuilding site)
        {
            int at = 0;
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(owner.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Unit u) && !u.isDead() && ((u.getPrimaryController() is com.oddlabs.tt.model.behaviour.RepairController rc) && ((object)rc.getBuilding() == (object)site) || (u.getPrimaryController() is com.oddlabs.tt.model.behaviour.PlaceBuildingController pc) && ((object)pc.getBuilding() == (object)site)))
                {
                    at++;
                }
            }
            java.util.List builders = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Selectable s_2 in java.lang.JIter.of(owner.getUnits().getSet()))
            {
                if (at + builders.size() < 3 && (s_2 is com.oddlabs.tt.model.Unit u_2) && !u_2.isDead() && !u_2.isMounted() && u_2.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD) && (u_2.getPrimaryController() is com.oddlabs.tt.model.behaviour.GatherController g) && ((object)g.getSupplyType() == (object)typeof(com.oddlabs.tt.landscape.TreeSupply)))
                {
                    builders.add(u_2);
                }
            }
            if (!builders.isEmpty())
            {
                owner.setTarget(((com.oddlabs.tt.model.Selectable[])builders.toArray(new com.oddlabs.tt.model.Selectable[0])), site, com.oddlabs.tt.model.Action.DEFAULT, false);
            }
        }
        // Java: 192
        public int stock(com.oddlabs.tt.model.LandBuilding armory_v, System.Type type)
        {
            com.oddlabs.tt.model.SupplyContainer c = armory_v.getSupplyContainer(type);
            return (c == null ? 0 : c.getNumSupplies());
        }
        // Java: 197
        public void craft(com.oddlabs.tt.model.LandBuilding armory_v, com.oddlabs.tt.model.DeployType type, int keep, System.Type product)
        {
            if ((armory_v.getDeployContainer(type) is com.oddlabs.tt.model.CraftContainer c) && c.isAvailable() && c.getNumSupplies() == 0 && stock(armory_v, product) < keep && c.affordable() > 0)
            {
                owner.deployUnits(armory_v, type, 1);
            }
        }
        // Java: 203
        public void train(com.oddlabs.tt.model.LandBuilding armory_v, com.oddlabs.tt.model.DeployType type)
        {
            com.oddlabs.tt.model.DeployContainer c = armory_v.getDeployContainer(type);
            if (c == null || c.getNumSupplies() > 0 || armory_v.getUnitContainer().getNumSupplies() < 2)
            {
                return;
            }
            if ((c is com.oddlabs.tt.model.RareDeployContainer r) && (!r.hallReady() || r.affordable() == 0 || r.room() == 0))
            {
                return;
            }
            if (!com.oddlabs.tt.player.ExtendedAIPlan.enoughWeapons(armory_v, c))
            {
                return;
            }
            owner.deployUnits(armory_v, type, 1);
        }
        public float hold_since;
        public float hold_rest;
        public int hold_iron;
        // Java: 225
        public bool saveForFleet(com.oddlabs.tt.model.LandBuilding armory_v)
        {
            bool want_v = owner.getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.FLEET) == 0 && !owner.isResearching(com.oddlabs.tt.model.ResearchContainer.FLEET) && owner.getResearchCap(com.oddlabs.tt.model.ResearchContainer.FLEET) > 0 && time >= hold_rest;
            int iron = stock(armory_v, typeof(com.oddlabs.tt.model.IronSupply));
            if (want_v)
            {
                bool dock = false;
                foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(owner.getUnits().getSet()))
                {
                    if ((s is com.oddlabs.tt.model.LandBuilding b) && !b.isDead() && b.isComplete() && ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID() == com.oddlabs.tt.model.Race.BUILDING_DOCK)
                    {
                        dock = true;
                    }
                }
                int[] cost = com.oddlabs.tt.model.ResearchContainer.getCost(com.oddlabs.tt.model.ResearchContainer.FLEET, 0);
                bool short_of = false;
                for (int i = 0; i < cost.Length; i++)
                {
                    if (cost[i] > 0 && stock(armory_v, com.oddlabs.tt.model.ExtendedResources.STORES[i]) < cost[i])
                    {
                        short_of = true;
                    }
                }
                want_v = dock && short_of;
            }
            if (want_v && hold_since < 0)
            {
                hold_since = time;
                hold_iron = iron;
            }
            if (want_v && (float)(time - hold_since) > 180.0f)
            {
                want_v = false;
                hold_rest = (float)(time + ((iron > hold_iron ? 60.0f : 300.0f)));
            }
            if (!want_v)
            {
                hold_since = (float)(-1.0f);
                owner.setHoldWeapons(false);
                return false;
            }
            owner.setHoldWeapons(true);
            System.Type[] types = new System.Type[] { typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon), typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon), typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon) };
            foreach (com.oddlabs.tt.model.Selectable s_2 in java.lang.JIter.of(owner.getUnits().getSet()))
            {
                if ((s_2 is com.oddlabs.tt.model.LandBuilding b_2) && !b_2.isDead() && b_2.isComplete() && b_2.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD_ARMIES))
                {
                    foreach (System.Type t in types)
                    {
                        if ((b_2.getBuildSupplyContainer(t) is com.oddlabs.tt.model.BuildProductionContainer bp) && bp.getNumSupplies() > 0)
                        {
                            bp.orderSupply(-bp.getNumSupplies(), false);
                        }
                    }
                }
            }
            return true;
        }
        // Java: 266
        public static bool enoughWeapons(com.oddlabs.tt.model.LandBuilding armory_v, com.oddlabs.tt.model.DeployContainer c)
        {
            System.Type w = c.getDeploySupplyType();
            if (w == null)
            {
                return true;
            }
            int per = ((c is com.oddlabs.tt.model.MultiWeaponDeployContainer m) ? m.getWeaponsPerUnit() : ((c is com.oddlabs.tt.model.RareDeployContainer r) ? r.getWeaponsPerUnit() : 1));
            com.oddlabs.tt.model.SupplyContainer store = armory_v.getSupplyContainer(w);
            return store != null && store.getNumSupplies() >= per;
        }
        // Java: 277
        public int godWeapons(com.oddlabs.tt.model.DeployType type)
        {
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(owner.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.LandBuilding b) && !b.isDead() && b.isComplete() && (b.getDeployContainer(type) is com.oddlabs.tt.model.GodContainer g))
                {
                    return (g.isLocked() || g.isTaken() ? 0 : g.getWeapons());
                }
            }
            return 0;
        }
        // Java: 286
        public void callGod(com.oddlabs.tt.model.DeployType type)
        {
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(owner.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.LandBuilding b) && !b.isDead() && b.isComplete() && (b.getDeployContainer(type) is com.oddlabs.tt.model.GodContainer g))
                {
                    if (g.canOrder())
                    {
                        owner.deployUnits(b, type, 1);
                    }
                    return;
                }
            }
        }
        // Java: 297
        public void arsenal(com.oddlabs.tt.model.LandBuilding armory_v)
        {
            craft(armory_v, com.oddlabs.tt.model.DeployType.CRAFT_DARTS, 3, typeof(com.oddlabs.tt.model.weapon.DartWeapon));
            craft(armory_v, com.oddlabs.tt.model.DeployType.CRAFT_CRYSTAL, 3, typeof(com.oddlabs.tt.model.weapon.CrystalWeapon));
            int god_need = java.lang.Math.max((time >= com.oddlabs.tt.player.ExtendedAIPlan.MINOR_GOD_TIME ? godWeapons(com.oddlabs.tt.model.DeployType.MINOR_GOD) : 0), (time >= com.oddlabs.tt.player.ExtendedAIPlan.MAJOR_GOD_TIME ? godWeapons(com.oddlabs.tt.model.DeployType.MAJOR_GOD) : 0));
            craft(armory_v, com.oddlabs.tt.model.DeployType.CRAFT_DIAMOND, java.lang.Math.max((time >= com.oddlabs.tt.player.ExtendedAIPlan.MAJOR_GOD_TIME ? 3 : 2), god_need + 1), typeof(com.oddlabs.tt.model.weapon.DiamondWeapon));
            if (stock(armory_v, typeof(com.oddlabs.tt.model.weapon.CrystalWeapon)) > 0)
            {
                train(armory_v, com.oddlabs.tt.model.DeployType.CRYSTAL_WARRIOR);
            }
            if (time >= com.oddlabs.tt.player.ExtendedAIPlan.MINOR_GOD_TIME)
            {
                callGod(com.oddlabs.tt.model.DeployType.MINOR_GOD);
            }
            if (time >= com.oddlabs.tt.player.ExtendedAIPlan.MAJOR_GOD_TIME)
            {
                callGod(com.oddlabs.tt.model.DeployType.MAJOR_GOD);
            }
            if (stock(armory_v, typeof(com.oddlabs.tt.model.weapon.DiamondWeapon)) > java.lang.Math.max((time >= com.oddlabs.tt.player.ExtendedAIPlan.MAJOR_GOD_TIME ? 2 : 1), god_need))
            {
                train(armory_v, com.oddlabs.tt.model.DeployType.DIAMOND_GUARD);
            }
            if (time >= com.oddlabs.tt.player.ExtendedAIPlan.ELITE_TIME)
            {
                foreach (com.oddlabs.tt.model.DeployType t in new com.oddlabs.tt.model.DeployType[] { com.oddlabs.tt.model.DeployType.ELITE_1, com.oddlabs.tt.model.DeployType.ELITE_2, com.oddlabs.tt.model.DeployType.ELITE_3 })
                {
                    train(armory_v, t);
                }
            }
        }
        // Java: 318
        public com.oddlabs.tt.model.LandBuilding armory()
        {
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(owner.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.LandBuilding b) && !b.isDead() && b.isComplete() && b.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD_ARMIES) && b.getDeployContainer(com.oddlabs.tt.model.DeployType.PEON_HARVEST_GOLD) != null)
                {
                    return b;
                }
            }
            return null;
        }
        // Java: 327
        public void scanDeposits()
        {
            com.oddlabs.tt.pathfinder.UnitGrid grid = owner.getWorld().getUnitGrid();
            int sx = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(owner.getStartX());
            int sy = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(owner.getStartY());
            java.util.Arrays.fill(near, 0);
            int size = grid.getGridSize();
            for (int y = java.lang.Math.max(0, sy - com.oddlabs.tt.player.ExtendedAIPlan.SCAN_RADIUS); y < java.lang.Math.min(size, sy + com.oddlabs.tt.player.ExtendedAIPlan.SCAN_RADIUS); y++)
            {
                for (int x = java.lang.Math.max(0, sx - com.oddlabs.tt.player.ExtendedAIPlan.SCAN_RADIUS); x < java.lang.Math.min(size, sx + com.oddlabs.tt.player.ExtendedAIPlan.SCAN_RADIUS); x++)
                {
                    com.oddlabs.tt.pathfinder.Occupant o = grid.getOccupant(x, y);
                    int k = (o == null ? (-1) : com.oddlabs.tt.model.ExtendedResources.rareIndex(o.GetType()));
                    if (k >= 0)
                    {
                        near[k]++;
                    }
                }
            }
        }
        // Java: 341
        public int want(int rare)
        {
            if (near[rare] == 0)
            {
                return 0;
            }
            return ((rare) switch { 0 => (int)(2), 1 => (int)(1), 2 => (int)((vikings ? 0 : 2)), 3 => (int)((vikings ? 2 : 1)), _ => (int)(1) });
        }
        // Java: 353
        public void gatherRare(com.oddlabs.tt.model.LandBuilding armory_v)
        {
            int[] have = new int[com.oddlabs.tt.model.ExtendedResources.RARE.Length];
            java.util.List tree_gatherers = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(owner.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Unit u) && !u.isDead() && (u.getPrimaryController() is com.oddlabs.tt.model.behaviour.GatherController g))
                {
                    int k = com.oddlabs.tt.model.ExtendedResources.rareIndex(g.getSupplyType());
                    if (k >= 0)
                    {
                        have[k]++;
                    }
                    else if (((object)g.getSupplyType() == (object)typeof(com.oddlabs.tt.landscape.TreeSupply)) && u.getSupplyContainer().getNumSupplies() == 0)
                    {
                        tree_gatherers.add(u);
                    }
                }
            }
            com.oddlabs.tt.model.DeployType[] types = new com.oddlabs.tt.model.DeployType[] { com.oddlabs.tt.model.DeployType.PEON_HARVEST_GOLD, com.oddlabs.tt.model.DeployType.PEON_HARVEST_CRYSTAL, com.oddlabs.tt.model.DeployType.PEON_HARVEST_HERBS, com.oddlabs.tt.model.DeployType.PEON_HARVEST_COAL, com.oddlabs.tt.model.DeployType.PEON_HARVEST_DIAMOND };
            for (int k_2 = 0; k_2 < types.Length; k_2++)
            {
                if (have[k_2] >= want(k_2))
                {
                    continue;
                }
                if (armory_v.getUnitContainer().getNumSupplies() >= 3)
                {
                    owner.deployUnits(armory_v, types[k_2], 1);
                }
                else if (tree_gatherers.size() > 4)
                {
                    ((com.oddlabs.tt.model.Unit)tree_gatherers.removeLast()).initGather(com.oddlabs.tt.model.ExtendedResources.RARE[k_2], armory_v);
                }
            }
        }
        // Java: 376
        public bool hasHall()
        {
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(owner.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Building b) && !b.isDead() && ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID() == com.oddlabs.tt.model.Race.BUILDING_HALL)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 383
        public void buildHall(com.oddlabs.tt.model.LandBuilding armory_v)
        {
            next_hall = (float)(time + 30.0f);
            if (hall_site != null && !hall_site.isDead() && hall_site.isPlaced())
            {
                if (!hall_site.isComplete())
                {
                    sendBuilders(hall_site);
                }
                return;
            }
            if (hasHall() && (hall_site == null || hall_site.isDead() || hall_site.isPlaced()))
            {
                return;
            }
            if (!owner.canBuild(com.oddlabs.tt.model.Race.BUILDING_HALL))
            {
                return;
            }
            com.oddlabs.tt.model.BuildingTemplate template = owner.getRace().getBuildingTemplate(com.oddlabs.tt.model.Race.BUILDING_HALL);
            com.oddlabs.tt.pathfinder.UnitGrid grid = owner.getWorld().getUnitGrid();
            if (hall_site == null || hall_site.isDead())
            {
                hall_site = null;
                int ax = armory_v.getGridX();
                int ay = armory_v.getGridY();
                for (int r = 7; r <= 22; r++)
                {
                    for (int i = 0; i < 8 * r; i++)
                    {
                        int side = i / (2 * r);
                        int d = i % (2 * r);
                        int x = (side == 0 ? ax - r + d : (side == 1 ? ax + r : (side == 2 ? ax + r - d : ax - r)));
                        int y = (side == 0 ? ay - r : (side == 1 ? ay - r + d : (side == 2 ? ay + r : ay + r - d)));
                        if (x < 4 || y < 4 || x >= grid.getGridSize() - 4 || y >= grid.getGridSize() - 4)
                        {
                            continue;
                        }
                        if (grid.getRegion(x, y) == null || !com.oddlabs.tt.model.LandBuilding.isPlacingLegal(grid, template, x, y))
                        {
                            continue;
                        }
                        hall_site = ((com.oddlabs.tt.model.LandBuilding)template.create(owner, x, y));
                        goto brk_search_8;
                    }
                }
                brk_search_8: ;
                if (hall_site == null)
                {
                    return;
                }
            }
            sendBuilders(hall_site);
        }
        // Java: 420
        public void upgradeBuildings()
        {
            com.oddlabs.tt.model.LandBuilding pick = null;
            com.oddlabs.tt.model.DeployType pick_type = null;
            int best = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(owner.getUnits().getSet()))
            {
                if (!((s is com.oddlabs.tt.model.LandBuilding b)) || b.isDead() || !b.isComplete())
                {
                    continue;
                }
                int id = ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID();
                foreach (com.oddlabs.tt.model.DeployType t in new com.oddlabs.tt.model.DeployType[] { com.oddlabs.tt.model.DeployType.UPGRADE_STONE, com.oddlabs.tt.model.DeployType.UPGRADE_IRON, com.oddlabs.tt.model.DeployType.UPGRADE_CHICKEN, com.oddlabs.tt.model.DeployType.UPGRADE_RUNE })
                {
                    com.oddlabs.tt.model.DeployContainer c = b.getDeployContainer(t);
                    if (!((c is com.oddlabs.tt.model.TierContainer tc)))
                    {
                        continue;
                    }
                    if (tc.isWorking())
                    {
                        return;
                    }
                    if (((object)t == (object)com.oddlabs.tt.model.DeployType.UPGRADE_IRON) && time < com.oddlabs.tt.player.ExtendedAIPlan.IRON_TIME)
                    {
                        continue;
                    }
                    if (((object)t == (object)com.oddlabs.tt.model.DeployType.UPGRADE_CHICKEN) && id != com.oddlabs.tt.model.Race.BUILDING_TOWER)
                    {
                        continue;
                    }
                    if (((object)t == (object)com.oddlabs.tt.model.DeployType.UPGRADE_RUNE) && (time < com.oddlabs.tt.player.ExtendedAIPlan.RUNE_TIME || id != com.oddlabs.tt.model.Race.BUILDING_TOWER && id != com.oddlabs.tt.model.Race.BUILDING_ARMORY))
                    {
                        continue;
                    }
                    if (id == com.oddlabs.tt.model.Race.BUILDING_PALISADE || !tc.canOrder())
                    {
                        continue;
                    }
                    int rank = ((id == com.oddlabs.tt.model.Race.BUILDING_TOWER ? 0 : (id == com.oddlabs.tt.model.Race.BUILDING_ARMORY ? 1 : 2))) * 10 + tc.getKind();
                    if (rank < best)
                    {
                        best = rank;
                        pick = b;
                        pick_type = t;
                    }
                }
            }
            if (pick != null)
            {
                owner.deployUnits(pick, pick_type, 1);
            }
        }
        // Java: 456
        public void trainRaceUnits(com.oddlabs.tt.model.LandBuilding armory_v)
        {
            int warriors = 0;
            int race_units = 0;
            int spinners = 0;
            com.oddlabs.tt.model.UnitTemplate spinner = owner.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_SPINNER);
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(owner.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Unit u) && !u.isDead() && u.isWarrior())
                {
                    warriors++;
                    if (com.oddlabs.tt.model.ExtendedCombat.uniqueKind(u) != com.oddlabs.tt.model.ExtendedCombat.NONE)
                    {
                        race_units++;
                    }
                    if (((object)((com.oddlabs.tt.model.UnitTemplate)u.getTemplate()) == (object)spinner))
                    {
                        spinners++;
                    }
                }
            }
            com.oddlabs.tt.model.DeployContainer sc = armory_v.getDeployContainer(com.oddlabs.tt.model.DeployType.SPINNER);
            com.oddlabs.tt.model.SupplyContainer rubber = armory_v.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon));
            if (sc != null && rubber != null && sc.getNumSupplies() == 0 && com.oddlabs.tt.player.ExtendedAIPlan.enoughWeapons(armory_v, sc) && rubber.getNumSupplies() >= (((sc is com.oddlabs.tt.model.MultiWeaponDeployContainer m) ? m.getWeaponsPerUnit() : 1)) + 2 && spinners < java.lang.Math.max(2, warriors / 6) && armory_v.getUnitContainer().getNumSupplies() >= 2 && !owner.isLocked(com.oddlabs.tt.model.Unlocks.unit(com.oddlabs.tt.model.Race.UNIT_SPINNER)))
            {
                owner.deployUnits(armory_v, com.oddlabs.tt.model.DeployType.SPINNER, 1);
            }
            if (race_units >= java.lang.Math.max(4, warriors / 4))
            {
                return;
            }
            foreach (com.oddlabs.tt.model.DeployType t in new com.oddlabs.tt.model.DeployType[] { com.oddlabs.tt.model.DeployType.UNIQUE_1, com.oddlabs.tt.model.DeployType.UNIQUE_2, com.oddlabs.tt.model.DeployType.UNIQUE_3 })
            {
                if ((armory_v.getDeployContainer(t) is com.oddlabs.tt.model.RareDeployContainer c) && c.hallReady() && c.getNumSupplies() == 0 && c.affordable() > 0 && armory_v.getUnitContainer().getNumSupplies() >= 2 && com.oddlabs.tt.player.ExtendedAIPlan.enoughWeapons(armory_v, c))
                {
                    owner.deployUnits(armory_v, t, 1);
                }
            }
        }
        public const float GENERAL_RESEARCH_TIME = 480.0f;
        public static com.oddlabs.tt.model.DeployType[] HALL_RESEARCH;
        public static com.oddlabs.tt.model.DeployType[] ARMORY_RESEARCH;
        public static com.oddlabs.tt.model.DeployType[] QUARTERS_RESEARCH;
        public static com.oddlabs.tt.model.DeployType[] FORGE_RESEARCH;
        public static com.oddlabs.tt.model.DeployType[] TOTEM_RESEARCH;
        public static com.oddlabs.tt.model.DeployType[] TOWER_RESEARCH;
        // Java: 503
        public void research(bool saving)
        {
            if (sea && owner.getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.FLEET) == 0 && !owner.isResearching(com.oddlabs.tt.model.ResearchContainer.FLEET))
            {
                foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(owner.getUnits().getSet()))
                {
                    if ((s is com.oddlabs.tt.model.LandBuilding b) && !b.isDead() && b.isComplete() && ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID() == com.oddlabs.tt.model.Race.BUILDING_DOCK && (b.getDeployContainer(com.oddlabs.tt.model.DeployType.RESEARCH_FLEET) is com.oddlabs.tt.model.ResearchContainer rc) && rc.canAfford())
                    {
                        owner.deployUnits(b, com.oddlabs.tt.model.DeployType.RESEARCH_FLEET, 1);
                        owner.setHoldWeapons(false);
                        return;
                    }
                }
            }
            if (saving)
            {
                return;
            }
            int busy = 0;
            for (int u = 0; u < com.oddlabs.tt.model.ResearchContainer.NUM_UPGRADES; u++)
            {
                if (owner.isResearching(u))
                {
                    busy++;
                }
            }
            if (busy >= 2)
            {
                return;
            }
            bool general = time >= com.oddlabs.tt.player.ExtendedAIPlan.GENERAL_RESEARCH_TIME && busy == 0;
            foreach (com.oddlabs.tt.model.Selectable s_2 in java.lang.JIter.of(owner.getUnits().getSet()))
            {
                if (!((s_2 is com.oddlabs.tt.model.LandBuilding b_2)) || b_2.isDead() || !b_2.isComplete())
                {
                    continue;
                }
                int id = ((com.oddlabs.tt.model.BuildingTemplate)b_2.getTemplate()).getTemplateID();
                com.oddlabs.tt.model.DeployType[] list = (id == com.oddlabs.tt.model.Race.BUILDING_HALL ? com.oddlabs.tt.player.ExtendedAIPlan.HALL_RESEARCH : (id == com.oddlabs.tt.model.Race.BUILDING_FORGE ? com.oddlabs.tt.player.ExtendedAIPlan.FORGE_RESEARCH : (id == com.oddlabs.tt.model.Race.BUILDING_DOCK ? ((sea ? com.oddlabs.tt.player.ExtendedAIPlan.DOCK_RESEARCH : (com.oddlabs.tt.model.DeployType[])(null))) : (!general ? (com.oddlabs.tt.model.DeployType[])(null) : (id == com.oddlabs.tt.model.Race.BUILDING_ARMORY ? com.oddlabs.tt.player.ExtendedAIPlan.ARMORY_RESEARCH : (id == com.oddlabs.tt.model.Race.BUILDING_QUARTERS ? com.oddlabs.tt.player.ExtendedAIPlan.QUARTERS_RESEARCH : (id == com.oddlabs.tt.model.Race.BUILDING_TOWER ? com.oddlabs.tt.player.ExtendedAIPlan.TOWER_RESEARCH : (id == com.oddlabs.tt.model.Race.BUILDING_TOTEM ? com.oddlabs.tt.player.ExtendedAIPlan.TOTEM_RESEARCH : (com.oddlabs.tt.model.DeployType[])(null)))))))));
                if (list == null)
                {
                    continue;
                }
                foreach (com.oddlabs.tt.model.DeployType t in list)
                {
                    if ((b_2.getDeployContainer(t) is com.oddlabs.tt.model.ResearchContainer rc_2) && !rc_2.isResearching() && !owner.isResearching(rc_2.getUpgrade()) && rc_2.canAfford())
                    {
                        owner.deployUnits(b_2, t, 1);
                        return;
                    }
                }
            }
        }
        static ExtendedAIPlan()
        {
            ExtendedAIPlan.DOCK_RESEARCH = new com.oddlabs.tt.model.DeployType[] { com.oddlabs.tt.model.DeployType.RESEARCH_FLEET, com.oddlabs.tt.model.DeployType.RESEARCH_SHIP_HULL, com.oddlabs.tt.model.DeployType.RESEARCH_SHIP_OARS };
            ExtendedAIPlan.HALL_RESEARCH = new com.oddlabs.tt.model.DeployType[] { com.oddlabs.tt.model.DeployType.RESEARCH_RACE_1, com.oddlabs.tt.model.DeployType.RESEARCH_RACE_2, com.oddlabs.tt.model.DeployType.RESEARCH_PROSPECTING, com.oddlabs.tt.model.DeployType.RESEARCH_FOCUS, com.oddlabs.tt.model.DeployType.RESEARCH_ELITE_RANKS, com.oddlabs.tt.model.DeployType.RESEARCH_ELITE_TRAINING };
            ExtendedAIPlan.ARMORY_RESEARCH = new com.oddlabs.tt.model.DeployType[] { com.oddlabs.tt.model.DeployType.RESEARCH_ARMOR, com.oddlabs.tt.model.DeployType.RESEARCH_SMITHING, com.oddlabs.tt.model.DeployType.RESEARCH_TOOLS, com.oddlabs.tt.model.DeployType.RESEARCH_DRILL, com.oddlabs.tt.model.DeployType.RESEARCH_RANGE, com.oddlabs.tt.model.DeployType.RESEARCH_SPEED };
            ExtendedAIPlan.QUARTERS_RESEARCH = new com.oddlabs.tt.model.DeployType[] { com.oddlabs.tt.model.DeployType.RESEARCH_GROWTH, com.oddlabs.tt.model.DeployType.RESEARCH_STURDY, com.oddlabs.tt.model.DeployType.RESEARCH_VIGOR, com.oddlabs.tt.model.DeployType.RESEARCH_BASKETS };
            ExtendedAIPlan.FORGE_RESEARCH = new com.oddlabs.tt.model.DeployType[] { com.oddlabs.tt.model.DeployType.RESEARCH_CRYSTAL_CRAFT, com.oddlabs.tt.model.DeployType.RESEARCH_DIAMOND_CRAFT, com.oddlabs.tt.model.DeployType.RESEARCH_CHICKEN_BOUNCE, com.oddlabs.tt.model.DeployType.RESEARCH_IRON_TEMPER, com.oddlabs.tt.model.DeployType.RESEARCH_ROCK_EDGE, com.oddlabs.tt.model.DeployType.RESEARCH_CRYSTAL_FACET, com.oddlabs.tt.model.DeployType.RESEARCH_DIAMOND_EDGE };
            ExtendedAIPlan.TOTEM_RESEARCH = new com.oddlabs.tt.model.DeployType[] { com.oddlabs.tt.model.DeployType.RESEARCH_RUNE_MIGHT, com.oddlabs.tt.model.DeployType.RESEARCH_RUNE_REACH, com.oddlabs.tt.model.DeployType.RESEARCH_RUNE_WARD };
            ExtendedAIPlan.TOWER_RESEARCH = new com.oddlabs.tt.model.DeployType[] { com.oddlabs.tt.model.DeployType.RESEARCH_TOWER_ARMOR, com.oddlabs.tt.model.DeployType.RESEARCH_VOLLEY, com.oddlabs.tt.model.DeployType.RESEARCH_TOWER_RANGE };
        }
        private void init_ExtendedAIPlan()
        {
            this.near = new int[com.oddlabs.tt.model.ExtendedResources.RARE.Length];
            this.next_tick = com.oddlabs.tt.player.ExtendedAIPlan.PERIOD;
            this.next_hall = com.oddlabs.tt.player.ExtendedAIPlan.HALL_TIME;
            this.next_site = new float[com.oddlabs.tt.model.Race.NUM_BUILDINGS];
            this.sites = new com.oddlabs.tt.model.LandBuilding[com.oddlabs.tt.model.Race.NUM_BUILDINGS];
            this.hold_since = (float)(-1.0f);
        }
        public ExtendedAIPlan(com.oddlabs.tt.player.Player owner) : base()
        {
            init_ExtendedAIPlan();
            this.owner = owner;
            this.vikings = com.oddlabs.tt.model.ExtendedCombat.isVikings(owner);
            this.sea = owner.getWorld().isArchipelago();
            owner.getWorld().getAnimationManagerGameTime().registerAnimation(this);
        }
        // mixin of default method Animated.updateChecksum
        public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
        }
    }
}
