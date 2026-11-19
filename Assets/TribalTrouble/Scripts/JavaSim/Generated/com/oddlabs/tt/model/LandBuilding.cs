// Translated from LandBuilding.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 42
    public class LandBuilding : com.oddlabs.tt.model.Building
    {
        public const float REMOVE_DELAY = 0.1f;
        public const int PLACING_BORDER = 1;
        public const int MAX_SUPPLY_COUNT = 100000;
        public static com.oddlabs.tt.model.Cost COST_ROCK_WEAPON;
        public static com.oddlabs.tt.model.Cost COST_IRON_WEAPON;
        public static com.oddlabs.tt.model.Cost COST_RUBBER_WEAPON;
        public const float DAMAGED_PARTICLE_ALPHA = 3.0f;
        public static int[][] UNIQUE_1_COST;
        public static int[][] UNIQUE_2_COST;
        public static int[] NO_COST;
        public static int[][] UNIQUE_3_COST;
        public static int[] MINOR_GOD_COST;
        public static int[][] ELITE_1_COST;
        public static int[] ELITE_2_COST;
        public static int[][] ELITE_3_COST;
        public const int ELITE_BASE_LIMIT = 2;
        public static int[][] MAJOR_GOD_COST;
        public const int DEFAULT_MAX_CHIEFTAINS_PER_QUARTERS = 10;
        public java.util.Map supply_containers;
        public java.util.Map build_containers;
        public com.oddlabs.tt.model.ShrineAura shrine_aura;
        public int armor_hits;
        public int tier;
        public bool chicken_barbs;
        public int reflect_hits;
        public com.oddlabs.tt.model.Afflictions afflictions_f;
        public com.oddlabs.tt.model.HallAura hall_aura;
        public com.oddlabs.tt.model.TotemAura totem_aura;
        public com.oddlabs.tt.model.DockAura dock_aura;
        // Java: 92
        public int armorLevel()
        {
            if (!getOwner().getWorld().isExtended())
            {
                return 0;
            }
            int id = ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getTemplateID();
            int level = tier;
            if (id == com.oddlabs.tt.model.Race.BUILDING_TOWER)
            {
                level += getOwner().getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.TOWER_ARMOR);
            }
            else if (id == com.oddlabs.tt.model.Race.BUILDING_PALISADE || id == com.oddlabs.tt.model.Race.BUILDING_SHRINE)
            {
                level += getOwner().getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.WALLS);
            }
            return java.lang.Math.min(level, 3);
        }
        // Java: 105
        public int getTier()
        {
            return tier;
        }
        // Java: 110
        public void setTier(int new_tier)
        {
            int old_max = getEffectiveMaxHitPoints();
            bool complete = build_points == old_max;
            tier = new_tier;
            int new_max = getEffectiveMaxHitPoints();
            if (complete)
            {
                build_points = new_max;
            }
            setHitPoints(hit_points + (new_max - old_max));
            reinsert();
        }
        // Java: 121
        public bool hasChickenBarbs()
        {
            return chicken_barbs;
        }
        // Java: 125
        public void setChickenBarbs()
        {
            chicken_barbs = true;
        }
        // Java: 130
        public bool nextReflect()
        {
            return (++reflect_hits & 1) == 0;
        }
        // Java: 135
        public float getTierRangeBonus()
        {
            return (((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getTemplateID() == com.oddlabs.tt.model.Race.BUILDING_TOWER ? (float)(1.5f * tier) : 0.0f);
        }
        // Java: 139
        public com.oddlabs.tt.model.Afflictions afflictions()
        {
            if (afflictions_f == null)
            {
                afflictions_f = new com.oddlabs.tt.model.Afflictions();
            }
            return afflictions_f;
        }
        // Java: 145
        public bool isBurning()
        {
            return afflictions_f != null && afflictions_f.isBurning();
        }
        public java.util.Map deploy_containers;
        public com.oddlabs.tt.particle.LinearEmitter damaged_emitter;
        public com.oddlabs.tt.particle.LinearEmitter production_emitter;
        public com.oddlabs.tt.model.ChieftainContainer chieftain_container;
        public int chieftains_produced;
        public com.oddlabs.tt.model.WeaponsProducer weapons_producer;
        public float remove_delay;
        public int hit_points;
        public int build_points;
        public float[][] old_landscape_heights;
        public int pad_x;
        public int pad_y;
        public com.oddlabs.tt.util.Target rally_point;
        public bool is_training_chieftain;
        // Java: 216
        public override void visit(com.oddlabs.tt.model.ElementVisitor visitor)
        {
            visitor.visitBuilding(this);
        }
        // Java: 221
        public override bool hasRallyPoint()
        {
            return ((object)rally_point != (object)this);
        }
        // Java: 225
        public override com.oddlabs.tt.util.Target getRallyPoint()
        {
            return rally_point;
        }
        // Java: 229
        public override void onReinsert()
        {
            base.onReinsert();
            float xc = (float)(getPositionX() + ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getChimneyX());
            float yc = (float)(getPositionY() + ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getChimneyY());
            float zc = (float)(getPositionZ() + ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getChimneyZ());
            production_emitter.setPosition(new org.joml.Vector3f(xc, yc, zc));
        }
        // Java: 238
        public override void doAnimate(float t)
        {
            if (!isDead())
            {
                com.oddlabs.tt.model.UnitContainer unit_container = getUnitContainer();
                if (unit_container != null)
                {
                    unit_container.animate(t);
                }
                if (weapons_producer != null)
                {
                    weapons_producer.animate(t);
                }
                int num_deploying = 0;
                foreach (com.oddlabs.tt.model.DeployContainer deploy_container in java.lang.JIter.of(deploy_containers.values()))
                {
                    if (deploy_container.getNumSupplies() > 0)
                    {
                        num_deploying++;
                    }
                }
                if (num_deploying > 0)
                {
                    float amount = (float)(t / num_deploying);
                    foreach (com.oddlabs.tt.model.DeployContainer deploy_container_2 in java.lang.JIter.of(deploy_containers.values()))
                    {
                        if (deploy_container_2.getNumSupplies() > 0)
                        {
                            deploy_container_2.deploy(amount);
                        }
                    }
                }
                if (shrine_aura != null)
                {
                    shrine_aura.animate(t);
                }
                if (hall_aura != null)
                {
                    hall_aura.animate(t);
                }
                if (totem_aura != null)
                {
                    totem_aura.animate(t);
                }
                if (dock_aura != null)
                {
                    dock_aura.animate(t);
                }
                if (afflictions_f != null && !isDead())
                {
                    afflictions_f.animate(t, this);
                }
            }
            if (remove_delay > 0)
            {
                remove_delay -= t;
                if (remove_delay <= 0)
                {
                    remove();
                    damaged_emitter.done();
                    production_emitter.done();
                    if (weapons_producer != null)
                    {
                        weapons_producer.stopSound();
                    }
                    float energy = 3.0f;
                    float fade_speed = 2.5f;
                    new com.oddlabs.tt.particle.RandomVelocityEmitter(getOwner().getWorld(), new org.joml.Vector3f(getPositionX(), getPositionY(), getPositionZ()), 0.0f, ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getSmokeRadius(), ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getSmokeHeight(), 0.05f, ((float)(java.lang.Math.PI)), ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getNumFragments(), ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getNumFragments(), new org.joml.Vector3f(0.0f, 0.0f, 5.0f), new org.joml.Vector3f(0.0f, 0.0f, (float)(-25.0f)), new org.joml.Vector4f(1.0f, 1.0f, 1.0f, (float)(energy * fade_speed)), new org.joml.Vector4f(0.0f, 0.0f, 0.0f, (float)(-fade_speed)), new org.joml.Vector3f(1.0f, 1.0f, 1.0f), new org.joml.Vector3f(0.0f, 0.0f, 0.0f), energy, 0.75f, getOwner().getWorld().getRacesResources().getWoodFragments(), getOwner().getWorld().getAnimationManagerRealTime());
                }
            }
        }
        // Java: 298
        public override com.oddlabs.tt.model.UnitContainer getUnitContainer()
        {
            return ((com.oddlabs.tt.model.UnitContainer)getSupplyContainer(typeof(com.oddlabs.tt.model.Unit)));
        }
        // Java: 303
        public override com.oddlabs.tt.model.SupplyContainer getSupplyContainer(System.Type key)
        {
            return ((com.oddlabs.tt.model.SupplyContainer)supply_containers.get(key));
        }
        // Java: 308
        public override com.oddlabs.tt.model.BuildSupplyContainer getBuildSupplyContainer(System.Type key)
        {
            return ((com.oddlabs.tt.model.BuildProductionContainer)build_containers.get(key));
        }
        // Java: 313
        public override com.oddlabs.tt.model.DeployContainer getDeployContainer(com.oddlabs.tt.model.DeployType type)
        {
            return ((com.oddlabs.tt.model.DeployContainer)deploy_containers.get(type));
        }
        // Java: 318
        public override com.oddlabs.tt.model.ChieftainContainer getChieftainContainer()
        {
            return chieftain_container;
        }
        // Java: 324
        public int getChieftainsProduced()
        {
            return chieftains_produced;
        }
        // Java: 329
        public int getMaxChieftainsPerQuarters()
        {
            return getOwner().getWorld().getMaxChieftainsPerQuarters();
        }
        // Java: 333
        public override bool isEnabled()
        {
            return !isDead();
        }
        // Java: 338
        public override int getUnitCount()
        {
            return getUnitContainer().getNumSupplies();
        }
        // Java: 343
        public override bool canExitTower()
        {
            return !isDead() && getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.ATTACK) && getUnitContainer().getNumSupplies() > 0 && getOwner().canExitTowers() && !(((((com.oddlabs.tt.model.MountUnitContainer)getUnitContainer())).getUnit().getCurrentController() is com.oddlabs.tt.model.behaviour.StunController));
        }
        // Java: 349
        public override void exitTower()
        {
            com.oddlabs.tt.model.UnitContainer container = getUnitContainer();
            if (canExitTower())
            {
                com.oddlabs.tt.model.Unit unit = container.exit();
            }
        }
        // Java: 362
        public override void deployUnits(com.oddlabs.tt.model.DeployType type, int num_units)
        {
            getOwner().getWorld().updateGlobalChecksum(type.ordinal());
            getOwner().getWorld().updateGlobalChecksum(num_units);
            com.oddlabs.tt.model.DeployContainer container = getDeployContainer(type);
            if (container != null)
            {
                container.orderSupply(num_units);
            }
        }
        // Java: 372
        public void createRareHarvesters(System.Type supply_type, int amount)
        {
            createHarvesters(supply_type, amount);
        }
        // Java: 377
        public override void createHarvesters(int num_tree, int num_rock, int num_iron, int num_rubber)
        {
            createHarvesters(typeof(com.oddlabs.tt.landscape.TreeSupply), num_tree);
            createHarvesters(typeof(com.oddlabs.tt.model.RockSupply), num_rock);
            createHarvesters(typeof(com.oddlabs.tt.model.IronSupply), num_iron);
            createHarvesters(typeof(com.oddlabs.tt.model.RubberSupply), num_rubber);
        }
        // Java: 385
        public void createHarvesters(System.Type supply_type, int amount)
        {
            com.oddlabs.tt.model.Race race = getOwner().getRace();
            for (int i = 0; i < amount; i++)
            {
                getUnitContainer().prepareDeploy((-1));
                getUnitContainer().exit();
                com.oddlabs.tt.model.Unit unit = createUnit(null, race.getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
                unit.pushController(new com.oddlabs.tt.model.behaviour.GatherController(unit, null, supply_type, this));
            }
        }
        // Java: 395
        public override void buildWeapons(System.Type type, int num_weapons, bool infinite)
        {
            if (infinite)
            {
                getOwner().getWorld().updateGlobalChecksum(num_weapons);
            }
            else
            {
                getOwner().getWorld().updateGlobalChecksum(1000000);
            }
            (((com.oddlabs.tt.model.BuildProductionContainer)getBuildSupplyContainer(type))).orderSupply(num_weapons, infinite);
        }
        // Java: 404
        public override bool canBuildChieftain()
        {
            return !isDead() && chieftain_container != null && getOwner().canBuildChieftains() && !is_training_chieftain && chieftains_produced < getMaxChieftainsPerQuarters() && getOwner().canTrainMoreChieftains();
        }
        // Java: 413
        public override bool canStopChieftain()
        {
            return !isDead() && chieftain_container != null && chieftain_container.isTraining();
        }
        // Java: 417
        public override void trainChieftain(bool start)
        {
            if (canBuildChieftain() && start)
            {
                chieftain_container.startTraining();
                getOwner().setTrainingChieftain(true);
                is_training_chieftain = true;
            }
            else if (canStopChieftain() && !start)
            {
                chieftain_container.stopTraining();
                getOwner().setTrainingChieftain(false);
                is_training_chieftain = false;
            }
        }
        // Java: 429
        public override void deployChieftain()
        {
            chieftain_container.stopTraining();
            getOwner().setTrainingChieftain(false);
            is_training_chieftain = false;
            chieftains_produced++;
            com.oddlabs.tt.model.Unit chieftain = createUnit(null, getOwner().getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_CHIEFTAIN));
            if (getOwner().hasActiveChieftain())
            {
                getOwner().addExtraChieftain(chieftain);
            }
            else
            {
                getOwner().setActiveChieftain(chieftain);
            }
        }
        // Java: 444
        public com.oddlabs.tt.model.Unit createUnit(com.oddlabs.tt.util.Target rally_point, com.oddlabs.tt.model.UnitTemplate template)
        {
            if (getOwner().getWorld().isExtended())
            {
                float[] at = spawnPoint(rally_point);
                return new com.oddlabs.tt.model.Unit(getOwner(), at[0], at[1], rally_point, template, null, true, true);
            }
            return new com.oddlabs.tt.model.Unit(getOwner(), getPositionX(), getPositionY(), rally_point, template, null, true, true);
        }
        // Java: 457
        public override float getInteractDistance()
        {
            if (!getOwner().getWorld().isExtended())
            {
                return 0.0f;
            }
            int size = ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getPlacingSize();
            return (size >= 4 ? 5.0f : (size == 3 ? 3.0f : 0.0f));
        }
        public static int[][] SPAWN_DIRS;
        public int spawn_index;
        // Java: 469
        public float[] spawnPoint(com.oddlabs.tt.util.Target rally_point)
        {
            float r = (float)(getInteractDistance() * com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID);
            int best = spawn_index++ % com.oddlabs.tt.model.LandBuilding.SPAWN_DIRS.Length;
            if (rally_point != null && ((object)rally_point != (object)this))
            {
                float rx = (float)(rally_point.getPositionX() - getPositionX());
                float ry = (float)(rally_point.getPositionY() - getPositionY());
                float best_dot = (float)(-java.lang.Float.MAX_VALUE);
                for (int i = 0; i < com.oddlabs.tt.model.LandBuilding.SPAWN_DIRS.Length; i++)
                {
                    float dot = (float)(((float)((float)(rx * com.oddlabs.tt.model.LandBuilding.SPAWN_DIRS[i][0]) + (float)(ry * com.oddlabs.tt.model.LandBuilding.SPAWN_DIRS[i][1]))) / ((com.oddlabs.tt.model.LandBuilding.SPAWN_DIRS[i][0] != 0 && com.oddlabs.tt.model.LandBuilding.SPAWN_DIRS[i][1] != 0 ? 1.4142135f : 1.0f)));
                    if (dot > best_dot)
                    {
                        best_dot = dot;
                        best = i;
                    }
                }
            }
            float k = (com.oddlabs.tt.model.LandBuilding.SPAWN_DIRS[best][0] != 0 && com.oddlabs.tt.model.LandBuilding.SPAWN_DIRS[best][1] != 0 ? 0.70710677f : 1.0f);
            float x = (float)(getPositionX() + (float)((float)(com.oddlabs.tt.model.LandBuilding.SPAWN_DIRS[best][0] * k) * r));
            float y = (float)(getPositionY() + (float)((float)(com.oddlabs.tt.model.LandBuilding.SPAWN_DIRS[best][1] * k) * r));
            float max = getOwner().getWorld().getHeightMap().getMetersPerWorld() - com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID;
            x = java.lang.Math.max(com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID, java.lang.Math.min(max, x));
            y = java.lang.Math.max(com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID, java.lang.Math.min(max, y));
            return new float[] { x, y };
        }
        // Java: 491
        public override void createArmy(int num_peon, int num_rock, int num_iron, int num_rubber)
        {
            createArmy(num_peon, com.oddlabs.tt.model.Race.UNIT_PEON);
            createArmy(num_rock, com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK);
            createArmy(num_iron, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON);
            createArmy(num_rubber, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER);
        }
        // Java: 499
        public override void createUnits(int unit_template, int amount)
        {
            createArmy(amount, unit_template);
        }
        // Java: 505
        public void createArmy(int amount, int template)
        {
            com.oddlabs.tt.model.Race race = getOwner().getRace();
            checkRallyPoint();
            for (int i = 0; i < amount; i++)
            {
                getUnitContainer().prepareDeploy((-1));
                getUnitContainer().exit();
                com.oddlabs.tt.model.Unit unit = createUnit((hasRallyPoint() ? rally_point : (com.oddlabs.tt.util.Target)(null)), race.getUnitTemplate(template));
                if (getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.REPRODUCE) && !hasRallyPoint())
                {
                    unit.pushController(new com.oddlabs.tt.model.behaviour.TransferUnitController(unit));
                }
            }
        }
        // Java: 518
        public override void createTransporters(int num_tree, int num_rock, int num_iron, int num_rubber)
        {
            createTransporters(num_tree, typeof(com.oddlabs.tt.landscape.TreeSupply));
            createTransporters(num_rock, typeof(com.oddlabs.tt.model.RockSupply));
            createTransporters(num_iron, typeof(com.oddlabs.tt.model.IronSupply));
            createTransporters(num_rubber, typeof(com.oddlabs.tt.model.RubberSupply));
        }
        // Java: 526
        public void checkRallyPoint()
        {
            if (hasRallyPoint() && rally_point.isDead())
            {
                rally_point = this;
            }
        }
        // Java: 531
        public void createTransporters(int amount, System.Type supply)
        {
            com.oddlabs.tt.model.Race race = getOwner().getRace();
            checkRallyPoint();
            for (int i = 0; i < amount; i++)
            {
                getUnitContainer().prepareDeploy((-1));
                getUnitContainer().exit();
                com.oddlabs.tt.model.Unit unit = createUnit((hasRallyPoint() ? rally_point : (com.oddlabs.tt.util.Target)(null)), race.getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
                unit.getSupplyContainer().increaseSupply(unit.getSupplyContainer().getMaxSupplyCount(), supply);
            }
        }
        // Java: 551
        public override int getEffectiveMaxHitPoints()
        {
            int max = java.lang.Math.round((float)(((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getMaxHitPoints() * getOwner().getWorld().getBuildingHealthMultiplier()));
            return (tier == 0 ? max : max * (tier + 2) / 2);
        }
        // Java: 558
        public int getBuildPoints()
        {
            return build_points;
        }
        // Java: 562
        public override bool isDamaged()
        {
            return hit_points > 0 && hit_points < getEffectiveMaxHitPoints();
        }
        // Java: 567
        public override int getHitPoints()
        {
            return hit_points;
        }
        // Java: 572
        public com.oddlabs.tt.model.DeployContainer train(com.oddlabs.tt.model.DeployType type, bool vikings, int kind, System.Type weapon)
        {
            int defaults = (kind == com.oddlabs.tt.model.Race.UNIT_CHAMPION || kind == com.oddlabs.tt.model.Race.UNIT_SHAMAN || kind == com.oddlabs.tt.model.Race.UNIT_SPINNER ? 2 : 1);
            float defaultSeconds = (kind == com.oddlabs.tt.model.Race.UNIT_CHAMPION ? 3.0f : (kind == com.oddlabs.tt.model.Race.UNIT_SCOUT || kind == com.oddlabs.tt.model.Race.UNIT_MELEE_LIGHT ? 0.75f : (kind == com.oddlabs.tt.model.Race.UNIT_SHAMAN ? 4.0f : (kind == com.oddlabs.tt.model.Race.UNIT_MELEE_HEAVY ? 1.5f : (kind == com.oddlabs.tt.model.Race.UNIT_CRYSTAL ? 1.75f : (kind == com.oddlabs.tt.model.Race.UNIT_SPINNER ? 2.5f : 1.0f))))));
            float seconds = com.oddlabs.tt.model.NewTidesStats.trainSeconds(vikings, kind, defaultSeconds);
            int weapons = com.oddlabs.tt.model.NewTidesStats.weapons(vikings, kind, defaults);
            return (weapons > 1 ? (com.oddlabs.tt.model.DeployContainer)(new com.oddlabs.tt.model.MultiWeaponDeployContainer(this, seconds, type, weapon, weapons)) : new com.oddlabs.tt.model.DeployContainer(this, seconds, type, weapon));
        }
        // Java: 582
        public void setHitPoints(int new_hit_points)
        {
            float MIN_ENERGY = 3.0f;
            float MAX_ENERGY = 5.0f;
            int START_SMOKE = getEffectiveMaxHitPoints() / 2;
            hit_points = java.lang.Math.max(java.lang.Math.min(new_hit_points, getEffectiveMaxHitPoints()), 0);
            if (build_points == getEffectiveMaxHitPoints() && hit_points < START_SMOKE)
            {
                float energy = (float)(MIN_ENERGY + ((float)(((float)(1 - (float)(((float)(hit_points)) / (START_SMOKE)))) * ((float)(MAX_ENERGY - MIN_ENERGY)))));
                damaged_emitter.start();
                damaged_emitter.setDeltaColor(new org.joml.Vector4f(0.0f, 0.0f, 0.0f, (float)((float)(-com.oddlabs.tt.model.LandBuilding.DAMAGED_PARTICLE_ALPHA) / energy)));
                damaged_emitter.setEnergy(energy);
            }
            else
            {
                damaged_emitter.stop();
            }
        }
        // Java: 596
        public override void repair(int amount)
        {
            if (!isDamaged())
            {
                return;
            }
            setHitPoints(hit_points + amount);
            if (build_points < getEffectiveMaxHitPoints())
            {
                build_points = java.lang.Math.min(build_points + amount, getEffectiveMaxHitPoints());
                reinsert();
                if (build_points == getEffectiveMaxHitPoints())
                {
                    if (isBridge())
                    {
                        com.oddlabs.tt.model.BridgeSpan.open(this);
                    }
                    getOwner().getWorld().getNotificationListener().newSelectableNotification(this);
                    getAbilities().addAbilities(((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getAbilities());
                    supply_containers.put(typeof(com.oddlabs.tt.model.Unit), ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getUnitContainerFactory().createContainer(this));
                    if (getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.SUPPLY_CONTAINER))
                    {
                        com.oddlabs.tt.model.SupplyContainer tree_supply = new com.oddlabs.tt.model.SupplyContainer(com.oddlabs.tt.model.LandBuilding.MAX_SUPPLY_COUNT);
                        com.oddlabs.tt.model.SupplyContainer rock_supply = new com.oddlabs.tt.model.SupplyContainer(com.oddlabs.tt.model.LandBuilding.MAX_SUPPLY_COUNT);
                        com.oddlabs.tt.model.SupplyContainer iron_supply = new com.oddlabs.tt.model.SupplyContainer(com.oddlabs.tt.model.LandBuilding.MAX_SUPPLY_COUNT);
                        com.oddlabs.tt.model.SupplyContainer rubber_supply = new com.oddlabs.tt.model.SupplyContainer(com.oddlabs.tt.model.LandBuilding.MAX_SUPPLY_COUNT);
                        supply_containers.put(typeof(com.oddlabs.tt.landscape.TreeSupply), tree_supply);
                        supply_containers.put(typeof(com.oddlabs.tt.model.RockSupply), rock_supply);
                        supply_containers.put(typeof(com.oddlabs.tt.model.IronSupply), iron_supply);
                        supply_containers.put(typeof(com.oddlabs.tt.model.RubberSupply), rubber_supply);
                        com.oddlabs.tt.model.SupplyContainer rock_weapon_container = new com.oddlabs.tt.model.SupplyContainer(com.oddlabs.tt.model.LandBuilding.MAX_SUPPLY_COUNT);
                        supply_containers.put(typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon), rock_weapon_container);
                        supply_containers.put(typeof(com.oddlabs.tt.model.weapon.RockSpearWeapon), rock_weapon_container);
                        com.oddlabs.tt.model.SupplyContainer iron_weapon_container = new com.oddlabs.tt.model.SupplyContainer(com.oddlabs.tt.model.LandBuilding.MAX_SUPPLY_COUNT);
                        supply_containers.put(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon), iron_weapon_container);
                        supply_containers.put(typeof(com.oddlabs.tt.model.weapon.IronSpearWeapon), iron_weapon_container);
                        com.oddlabs.tt.model.SupplyContainer rubber_weapon_container = new com.oddlabs.tt.model.SupplyContainer(com.oddlabs.tt.model.LandBuilding.MAX_SUPPLY_COUNT);
                        supply_containers.put(typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon), rubber_weapon_container);
                        supply_containers.put(typeof(com.oddlabs.tt.model.weapon.RubberSpearWeapon), rubber_weapon_container);
                        com.oddlabs.tt.model.BuildProductionContainer rock_axe_weapon = new com.oddlabs.tt.model.BuildProductionContainer(com.oddlabs.tt.gui.BuildSpinner.INFINITE_LIMIT, rock_weapon_container, this, com.oddlabs.tt.model.LandBuilding.COST_ROCK_WEAPON, 40.0f);
                        com.oddlabs.tt.model.BuildProductionContainer iron_axe_weapon = new com.oddlabs.tt.model.BuildProductionContainer(com.oddlabs.tt.gui.BuildSpinner.INFINITE_LIMIT, iron_weapon_container, this, com.oddlabs.tt.model.LandBuilding.COST_IRON_WEAPON, 80.0f);
                        com.oddlabs.tt.model.BuildProductionContainer rubber_axe_weapon = new com.oddlabs.tt.model.BuildProductionContainer(com.oddlabs.tt.gui.BuildSpinner.INFINITE_LIMIT, rubber_weapon_container, this, com.oddlabs.tt.model.LandBuilding.COST_RUBBER_WEAPON, 120.0f);
                        build_containers.put(typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon), rock_axe_weapon);
                        build_containers.put(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon), iron_axe_weapon);
                        build_containers.put(typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon), rubber_axe_weapon);
                        com.oddlabs.tt.model.BuildProductionContainer[] production_containers = new com.oddlabs.tt.model.BuildProductionContainer[] { rock_axe_weapon, iron_axe_weapon, rubber_axe_weapon };
                        weapons_producer = new com.oddlabs.tt.model.WeaponsProducer(this, ((com.oddlabs.tt.model.WorkerUnitContainer)getUnitContainer()), production_containers, production_emitter);
                        deploy_containers.put(com.oddlabs.tt.model.DeployType.ROCK_WARRIOR, new com.oddlabs.tt.model.DeployContainer(this, 1.0f, com.oddlabs.tt.model.DeployType.ROCK_WARRIOR, typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)));
                        deploy_containers.put(com.oddlabs.tt.model.DeployType.IRON_WARRIOR, new com.oddlabs.tt.model.DeployContainer(this, 1.5f, com.oddlabs.tt.model.DeployType.IRON_WARRIOR, typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)));
                        deploy_containers.put(com.oddlabs.tt.model.DeployType.RUBBER_WARRIOR, new com.oddlabs.tt.model.DeployContainer(this, 2.0f, com.oddlabs.tt.model.DeployType.RUBBER_WARRIOR, typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)));
                        deploy_containers.put(com.oddlabs.tt.model.DeployType.PEON, new com.oddlabs.tt.model.DeployContainer(this, 0.5f, com.oddlabs.tt.model.DeployType.PEON, null));
                        deploy_containers.put(com.oddlabs.tt.model.DeployType.PEON_HARVEST_TREE, new com.oddlabs.tt.model.DeployContainer(this, 0.5f, com.oddlabs.tt.model.DeployType.PEON_HARVEST_TREE, null));
                        deploy_containers.put(com.oddlabs.tt.model.DeployType.PEON_TRANSPORT_TREE, new com.oddlabs.tt.model.DeployContainer(this, 0.5f, com.oddlabs.tt.model.DeployType.PEON_TRANSPORT_TREE, typeof(com.oddlabs.tt.landscape.TreeSupply)));
                        deploy_containers.put(com.oddlabs.tt.model.DeployType.PEON_HARVEST_ROCK, new com.oddlabs.tt.model.DeployContainer(this, 0.5f, com.oddlabs.tt.model.DeployType.PEON_HARVEST_ROCK, null));
                        deploy_containers.put(com.oddlabs.tt.model.DeployType.PEON_TRANSPORT_ROCK, new com.oddlabs.tt.model.DeployContainer(this, 0.5f, com.oddlabs.tt.model.DeployType.PEON_TRANSPORT_ROCK, typeof(com.oddlabs.tt.model.RockSupply)));
                        deploy_containers.put(com.oddlabs.tt.model.DeployType.PEON_HARVEST_IRON, new com.oddlabs.tt.model.DeployContainer(this, 0.5f, com.oddlabs.tt.model.DeployType.PEON_HARVEST_IRON, null));
                        deploy_containers.put(com.oddlabs.tt.model.DeployType.PEON_TRANSPORT_IRON, new com.oddlabs.tt.model.DeployContainer(this, 0.5f, com.oddlabs.tt.model.DeployType.PEON_TRANSPORT_IRON, typeof(com.oddlabs.tt.model.IronSupply)));
                        deploy_containers.put(com.oddlabs.tt.model.DeployType.PEON_HARVEST_RUBBER, new com.oddlabs.tt.model.DeployContainer(this, 0.5f, com.oddlabs.tt.model.DeployType.PEON_HARVEST_RUBBER, null));
                        deploy_containers.put(com.oddlabs.tt.model.DeployType.PEON_TRANSPORT_RUBBER, new com.oddlabs.tt.model.DeployContainer(this, 0.5f, com.oddlabs.tt.model.DeployType.PEON_TRANSPORT_RUBBER, typeof(com.oddlabs.tt.model.RubberSupply)));
                        if (getOwner().getWorld().isExtended())
                        {
                            foreach (System.Type rare in com.oddlabs.tt.model.ExtendedResources.RARE)
                            {
                                supply_containers.put(rare, new com.oddlabs.tt.model.SupplyContainer(com.oddlabs.tt.model.LandBuilding.MAX_SUPPLY_COUNT));
                            }
                            deploy_containers.put(com.oddlabs.tt.model.DeployType.PEON_HARVEST_GOLD, new com.oddlabs.tt.model.DeployContainer(this, 0.5f, com.oddlabs.tt.model.DeployType.PEON_HARVEST_GOLD, null));
                            deploy_containers.put(com.oddlabs.tt.model.DeployType.PEON_HARVEST_CRYSTAL, new com.oddlabs.tt.model.DeployContainer(this, 0.5f, com.oddlabs.tt.model.DeployType.PEON_HARVEST_CRYSTAL, null));
                            deploy_containers.put(com.oddlabs.tt.model.DeployType.PEON_HARVEST_HERBS, new com.oddlabs.tt.model.DeployContainer(this, 0.5f, com.oddlabs.tt.model.DeployType.PEON_HARVEST_HERBS, null));
                            deploy_containers.put(com.oddlabs.tt.model.DeployType.PEON_HARVEST_COAL, new com.oddlabs.tt.model.DeployContainer(this, 0.5f, com.oddlabs.tt.model.DeployType.PEON_HARVEST_COAL, null));
                            deploy_containers.put(com.oddlabs.tt.model.DeployType.PEON_HARVEST_DIAMOND, new com.oddlabs.tt.model.DeployContainer(this, 0.5f, com.oddlabs.tt.model.DeployType.PEON_HARVEST_DIAMOND, null));
                            bool vikings = com.oddlabs.tt.model.ExtendedCombat.isVikings(getOwner());
                            supply_containers.put(typeof(com.oddlabs.tt.model.weapon.CrystalWeapon), new com.oddlabs.tt.model.SupplyContainer(com.oddlabs.tt.model.LandBuilding.MAX_SUPPLY_COUNT));
                            supply_containers.put(typeof(com.oddlabs.tt.model.weapon.DiamondWeapon), new com.oddlabs.tt.model.SupplyContainer(com.oddlabs.tt.model.LandBuilding.MAX_SUPPLY_COUNT));
                            supply_containers.put(typeof(com.oddlabs.tt.model.weapon.DartWeapon), new com.oddlabs.tt.model.SupplyContainer(com.oddlabs.tt.model.LandBuilding.MAX_SUPPLY_COUNT));
                            foreach (com.oddlabs.tt.model.DeployType craft in new com.oddlabs.tt.model.DeployType[] { com.oddlabs.tt.model.DeployType.CRAFT_CRYSTAL, com.oddlabs.tt.model.DeployType.CRAFT_DIAMOND, com.oddlabs.tt.model.DeployType.CRAFT_DARTS })
                            {
                                deploy_containers.put(craft, com.oddlabs.tt.model.CraftContainer.create(this, craft));
                            }
                            deploy_containers.put(com.oddlabs.tt.model.DeployType.CRYSTAL_WARRIOR, train(com.oddlabs.tt.model.DeployType.CRYSTAL_WARRIOR, vikings, com.oddlabs.tt.model.Race.UNIT_CRYSTAL, typeof(com.oddlabs.tt.model.weapon.CrystalWeapon)));
                            deploy_containers.put(com.oddlabs.tt.model.DeployType.DIAMOND_GUARD, new com.oddlabs.tt.model.RareDeployContainer(this, com.oddlabs.tt.model.NewTidesStats.trainSeconds(vikings, com.oddlabs.tt.model.Race.UNIT_DIAMOND, 2.5f), com.oddlabs.tt.model.DeployType.DIAMOND_GUARD, typeof(com.oddlabs.tt.model.weapon.DiamondWeapon), com.oddlabs.tt.model.NewTidesStats.weapons(vikings, com.oddlabs.tt.model.Race.UNIT_DIAMOND, 1), com.oddlabs.tt.model.LandBuilding.NO_COST, com.oddlabs.tt.model.Race.BUILDING_FORGE));
                            deploy_containers.put(com.oddlabs.tt.model.DeployType.UNIQUE_3, (vikings ? (com.oddlabs.tt.model.DeployContainer)(new com.oddlabs.tt.model.RareDeployContainer(this, com.oddlabs.tt.model.NewTidesStats.trainSeconds(vikings, com.oddlabs.tt.model.Race.UNIT_UNIQUE_3, 2.0f), com.oddlabs.tt.model.DeployType.UNIQUE_3, typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon), com.oddlabs.tt.model.NewTidesStats.weapons(vikings, com.oddlabs.tt.model.Race.UNIT_UNIQUE_3, 1), com.oddlabs.tt.model.LandBuilding.UNIQUE_3_COST[1], com.oddlabs.tt.model.Race.BUILDING_HALL)) : (com.oddlabs.tt.model.DeployContainer)(new com.oddlabs.tt.model.RareDeployContainer(this, com.oddlabs.tt.model.NewTidesStats.trainSeconds(vikings, com.oddlabs.tt.model.Race.UNIT_UNIQUE_3, 1.75f), com.oddlabs.tt.model.DeployType.UNIQUE_3, typeof(com.oddlabs.tt.model.weapon.DartWeapon), com.oddlabs.tt.model.NewTidesStats.weapons(vikings, com.oddlabs.tt.model.Race.UNIT_UNIQUE_3, 2), com.oddlabs.tt.model.LandBuilding.UNIQUE_3_COST[0], com.oddlabs.tt.model.Race.BUILDING_HALL))));
                            deploy_containers.put(com.oddlabs.tt.model.DeployType.ELITE_1, new com.oddlabs.tt.model.RareDeployContainer(this, com.oddlabs.tt.model.NewTidesStats.trainSeconds(vikings, com.oddlabs.tt.model.Race.UNIT_ELITE_1, 3.0f), com.oddlabs.tt.model.DeployType.ELITE_1, typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon), com.oddlabs.tt.model.NewTidesStats.weapons(vikings, com.oddlabs.tt.model.Race.UNIT_ELITE_1, 1), com.oddlabs.tt.model.LandBuilding.ELITE_1_COST[(vikings ? 1 : 0)], com.oddlabs.tt.model.Race.BUILDING_TOTEM).limit(com.oddlabs.tt.model.LandBuilding.ELITE_BASE_LIMIT));
                            deploy_containers.put(com.oddlabs.tt.model.DeployType.ELITE_2, new com.oddlabs.tt.model.RareDeployContainer(this, com.oddlabs.tt.model.NewTidesStats.trainSeconds(vikings, com.oddlabs.tt.model.Race.UNIT_ELITE_2, 2.5f), com.oddlabs.tt.model.DeployType.ELITE_2, typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon), com.oddlabs.tt.model.NewTidesStats.weapons(vikings, com.oddlabs.tt.model.Race.UNIT_ELITE_2, 1), com.oddlabs.tt.model.LandBuilding.ELITE_2_COST, com.oddlabs.tt.model.Race.BUILDING_TOTEM).limit(com.oddlabs.tt.model.LandBuilding.ELITE_BASE_LIMIT));
                            deploy_containers.put(com.oddlabs.tt.model.DeployType.ELITE_3, new com.oddlabs.tt.model.RareDeployContainer(this, com.oddlabs.tt.model.NewTidesStats.trainSeconds(vikings, com.oddlabs.tt.model.Race.UNIT_ELITE_3, 2.5f), com.oddlabs.tt.model.DeployType.ELITE_3, typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon), com.oddlabs.tt.model.NewTidesStats.weapons(vikings, com.oddlabs.tt.model.Race.UNIT_ELITE_3, 1), com.oddlabs.tt.model.LandBuilding.ELITE_3_COST[(vikings ? 1 : 0)], com.oddlabs.tt.model.Race.BUILDING_TOTEM).limit(com.oddlabs.tt.model.LandBuilding.ELITE_BASE_LIMIT));
                            deploy_containers.put(com.oddlabs.tt.model.DeployType.UNIQUE_1, new com.oddlabs.tt.model.RareDeployContainer(this, com.oddlabs.tt.model.NewTidesStats.trainSeconds(vikings, com.oddlabs.tt.model.Race.UNIT_UNIQUE_1, 1.25f), com.oddlabs.tt.model.DeployType.UNIQUE_1, typeof(com.oddlabs.tt.model.weapon.DartWeapon), com.oddlabs.tt.model.NewTidesStats.weapons(vikings, com.oddlabs.tt.model.Race.UNIT_UNIQUE_1, 1), com.oddlabs.tt.model.LandBuilding.NO_COST, com.oddlabs.tt.model.Race.BUILDING_HALL));
                            deploy_containers.put(com.oddlabs.tt.model.DeployType.UNIQUE_2, new com.oddlabs.tt.model.RareDeployContainer(this, com.oddlabs.tt.model.NewTidesStats.trainSeconds(vikings, com.oddlabs.tt.model.Race.UNIT_UNIQUE_2, 2.0f), com.oddlabs.tt.model.DeployType.UNIQUE_2, typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon), com.oddlabs.tt.model.NewTidesStats.weapons(vikings, com.oddlabs.tt.model.Race.UNIT_UNIQUE_2, 1), com.oddlabs.tt.model.LandBuilding.UNIQUE_2_COST[(vikings ? 1 : 0)], com.oddlabs.tt.model.Race.BUILDING_HALL));
                            deploy_containers.put(com.oddlabs.tt.model.DeployType.CHAMPION, train(com.oddlabs.tt.model.DeployType.CHAMPION, vikings, com.oddlabs.tt.model.Race.UNIT_CHAMPION, typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)));
                            deploy_containers.put(com.oddlabs.tt.model.DeployType.SCOUT, train(com.oddlabs.tt.model.DeployType.SCOUT, vikings, com.oddlabs.tt.model.Race.UNIT_SCOUT, typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)));
                            deploy_containers.put(com.oddlabs.tt.model.DeployType.RESEARCH_ARMOR, new com.oddlabs.tt.model.ResearchContainer(this, com.oddlabs.tt.model.DeployType.RESEARCH_ARMOR, com.oddlabs.tt.model.ResearchContainer.ARMOR));
                            deploy_containers.put(com.oddlabs.tt.model.DeployType.RESEARCH_SPEED, new com.oddlabs.tt.model.ResearchContainer(this, com.oddlabs.tt.model.DeployType.RESEARCH_SPEED, com.oddlabs.tt.model.ResearchContainer.SPEED));
                            deploy_containers.put(com.oddlabs.tt.model.DeployType.RESEARCH_TOOLS, new com.oddlabs.tt.model.ResearchContainer(this, com.oddlabs.tt.model.DeployType.RESEARCH_TOOLS, com.oddlabs.tt.model.ResearchContainer.TOOLS));
                            deploy_containers.put(com.oddlabs.tt.model.DeployType.RESEARCH_RANGE, new com.oddlabs.tt.model.ResearchContainer(this, com.oddlabs.tt.model.DeployType.RESEARCH_RANGE, com.oddlabs.tt.model.ResearchContainer.RANGE));
                            deploy_containers.put(com.oddlabs.tt.model.DeployType.RESEARCH_SMITHING, new com.oddlabs.tt.model.ResearchContainer(this, com.oddlabs.tt.model.DeployType.RESEARCH_SMITHING, com.oddlabs.tt.model.ResearchContainer.SMITHING));
                            deploy_containers.put(com.oddlabs.tt.model.DeployType.RESEARCH_DRILL, new com.oddlabs.tt.model.ResearchContainer(this, com.oddlabs.tt.model.DeployType.RESEARCH_DRILL, com.oddlabs.tt.model.ResearchContainer.DRILL));
                            deploy_containers.put(com.oddlabs.tt.model.DeployType.SHAMAN, train(com.oddlabs.tt.model.DeployType.SHAMAN, vikings, com.oddlabs.tt.model.Race.UNIT_SHAMAN, typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)));
                            deploy_containers.put(com.oddlabs.tt.model.DeployType.SLINGER, train(com.oddlabs.tt.model.DeployType.SLINGER, vikings, com.oddlabs.tt.model.Race.UNIT_SLINGER, typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)));
                            deploy_containers.put(com.oddlabs.tt.model.DeployType.MELEE_LIGHT, train(com.oddlabs.tt.model.DeployType.MELEE_LIGHT, vikings, com.oddlabs.tt.model.Race.UNIT_MELEE_LIGHT, typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)));
                            deploy_containers.put(com.oddlabs.tt.model.DeployType.MELEE_HEAVY, train(com.oddlabs.tt.model.DeployType.MELEE_HEAVY, vikings, com.oddlabs.tt.model.Race.UNIT_MELEE_HEAVY, typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)));
                            deploy_containers.put(com.oddlabs.tt.model.DeployType.SPINNER, train(com.oddlabs.tt.model.DeployType.SPINNER, vikings, com.oddlabs.tt.model.Race.UNIT_SPINNER, typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)));
                        }
                    }
                    else if (getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.REPRODUCE))
                    {
                        chieftain_container = new com.oddlabs.tt.model.ChieftainContainer(this);
                        deploy_containers.put(com.oddlabs.tt.model.DeployType.PEON, new com.oddlabs.tt.model.DeployContainer(this, 0.5f, com.oddlabs.tt.model.DeployType.PEON, null));
                    }
                    if (((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getTemplateID() == com.oddlabs.tt.model.Race.BUILDING_SHRINE)
                    {
                        shrine_aura = new com.oddlabs.tt.model.ShrineAura(this);
                    }
                    if (((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getTemplateID() == com.oddlabs.tt.model.Race.BUILDING_HALL)
                    {
                        hall_aura = new com.oddlabs.tt.model.HallAura(this, com.oddlabs.tt.model.ExtendedCombat.isVikings(getOwner()));
                    }
                    if (((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getTemplateID() == com.oddlabs.tt.model.Race.BUILDING_TOTEM)
                    {
                        totem_aura = new com.oddlabs.tt.model.TotemAura(this, com.oddlabs.tt.model.ExtendedCombat.isVikings(getOwner()));
                    }
                    if (((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getTemplateID() == com.oddlabs.tt.model.Race.BUILDING_DOCK)
                    {
                        dock_aura = new com.oddlabs.tt.model.DockAura(this);
                    }
                    if (getOwner().getWorld().isExtended())
                    {
                        deploy_containers.put(com.oddlabs.tt.model.DeployType.UPGRADE_STONE, new com.oddlabs.tt.model.TierContainer(this, com.oddlabs.tt.model.DeployType.UPGRADE_STONE, com.oddlabs.tt.model.TierContainer.STONE));
                        deploy_containers.put(com.oddlabs.tt.model.DeployType.UPGRADE_IRON, new com.oddlabs.tt.model.TierContainer(this, com.oddlabs.tt.model.DeployType.UPGRADE_IRON, com.oddlabs.tt.model.TierContainer.IRON));
                        deploy_containers.put(com.oddlabs.tt.model.DeployType.UPGRADE_CHICKEN, new com.oddlabs.tt.model.TierContainer(this, com.oddlabs.tt.model.DeployType.UPGRADE_CHICKEN, com.oddlabs.tt.model.TierContainer.CHICKEN));
                        deploy_containers.put(com.oddlabs.tt.model.DeployType.UPGRADE_RUNE, new com.oddlabs.tt.model.TierContainer(this, com.oddlabs.tt.model.DeployType.UPGRADE_RUNE, com.oddlabs.tt.model.TierContainer.RUNE));
                    }
                    if (getOwner().getWorld().isExtended())
                    {
                        switch ((((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getTemplateID()))
                        {
                            case 0:
                            {
                                deploy_containers.put(com.oddlabs.tt.model.DeployType.RESEARCH_GROWTH, new com.oddlabs.tt.model.ResearchContainer(this, com.oddlabs.tt.model.DeployType.RESEARCH_GROWTH, com.oddlabs.tt.model.ResearchContainer.GROWTH));
                                deploy_containers.put(com.oddlabs.tt.model.DeployType.RESEARCH_STURDY, new com.oddlabs.tt.model.ResearchContainer(this, com.oddlabs.tt.model.DeployType.RESEARCH_STURDY, com.oddlabs.tt.model.ResearchContainer.STURDY));
                                deploy_containers.put(com.oddlabs.tt.model.DeployType.RESEARCH_BASKETS, new com.oddlabs.tt.model.ResearchContainer(this, com.oddlabs.tt.model.DeployType.RESEARCH_BASKETS, com.oddlabs.tt.model.ResearchContainer.BASKETS));
                                deploy_containers.put(com.oddlabs.tt.model.DeployType.RESEARCH_VIGOR, new com.oddlabs.tt.model.ResearchContainer(this, com.oddlabs.tt.model.DeployType.RESEARCH_VIGOR, com.oddlabs.tt.model.ResearchContainer.VIGOR));
                                deploy_containers.put(com.oddlabs.tt.model.DeployType.RESEARCH_BUILDINGS, new com.oddlabs.tt.model.ResearchContainer(this, com.oddlabs.tt.model.DeployType.RESEARCH_BUILDINGS, com.oddlabs.tt.model.ResearchContainer.BUILDINGS));
                                deploy_containers.put(com.oddlabs.tt.model.DeployType.RESEARCH_GREAT_TRIBE, new com.oddlabs.tt.model.ResearchContainer(this, com.oddlabs.tt.model.DeployType.RESEARCH_GREAT_TRIBE, com.oddlabs.tt.model.ResearchContainer.GREAT_TRIBE));
                                break;
                            }
                            case 2:
                            {
                                deploy_containers.put(com.oddlabs.tt.model.DeployType.RESEARCH_TOWER_RANGE, new com.oddlabs.tt.model.ResearchContainer(this, com.oddlabs.tt.model.DeployType.RESEARCH_TOWER_RANGE, com.oddlabs.tt.model.ResearchContainer.TOWER_RANGE));
                                deploy_containers.put(com.oddlabs.tt.model.DeployType.RESEARCH_TOWER_ARMOR, new com.oddlabs.tt.model.ResearchContainer(this, com.oddlabs.tt.model.DeployType.RESEARCH_TOWER_ARMOR, com.oddlabs.tt.model.ResearchContainer.TOWER_ARMOR));
                                deploy_containers.put(com.oddlabs.tt.model.DeployType.RESEARCH_VOLLEY, new com.oddlabs.tt.model.ResearchContainer(this, com.oddlabs.tt.model.DeployType.RESEARCH_VOLLEY, com.oddlabs.tt.model.ResearchContainer.VOLLEY));
                                break;
                            }
                            case 5:
                            {
                                deploy_containers.put(com.oddlabs.tt.model.DeployType.RESEARCH_BLESSING, new com.oddlabs.tt.model.ResearchContainer(this, com.oddlabs.tt.model.DeployType.RESEARCH_BLESSING, com.oddlabs.tt.model.ResearchContainer.BLESSING));
                                deploy_containers.put(com.oddlabs.tt.model.DeployType.RESEARCH_LORE, new com.oddlabs.tt.model.ResearchContainer(this, com.oddlabs.tt.model.DeployType.RESEARCH_LORE, com.oddlabs.tt.model.ResearchContainer.LORE));
                                deploy_containers.put(com.oddlabs.tt.model.DeployType.RESEARCH_ARCANE_POWER, new com.oddlabs.tt.model.ResearchContainer(this, com.oddlabs.tt.model.DeployType.RESEARCH_ARCANE_POWER, com.oddlabs.tt.model.ResearchContainer.ARCANE_POWER));
                                deploy_containers.put(com.oddlabs.tt.model.DeployType.RESEARCH_COURAGE_MASTERY, new com.oddlabs.tt.model.ResearchContainer(this, com.oddlabs.tt.model.DeployType.RESEARCH_COURAGE_MASTERY, com.oddlabs.tt.model.ResearchContainer.COURAGE_MASTERY));
                                break;
                            }
                            case 4:
                            {
                                deploy_containers.put(com.oddlabs.tt.model.DeployType.RESEARCH_WALLS, new com.oddlabs.tt.model.ResearchContainer(this, com.oddlabs.tt.model.DeployType.RESEARCH_WALLS, com.oddlabs.tt.model.ResearchContainer.WALLS));
                                deploy_containers.put(com.oddlabs.tt.model.DeployType.RESEARCH_STAKES, new com.oddlabs.tt.model.ResearchContainer(this, com.oddlabs.tt.model.DeployType.RESEARCH_STAKES, com.oddlabs.tt.model.ResearchContainer.STAKES));
                                break;
                            }
                            case 7:
                            {
                                com.oddlabs.tt.model.DeployType[] types = new com.oddlabs.tt.model.DeployType[] { com.oddlabs.tt.model.DeployType.RESEARCH_CRYSTAL_CRAFT, com.oddlabs.tt.model.DeployType.RESEARCH_DIAMOND_CRAFT, com.oddlabs.tt.model.DeployType.RESEARCH_ROCK_EDGE, com.oddlabs.tt.model.DeployType.RESEARCH_IRON_TEMPER, com.oddlabs.tt.model.DeployType.RESEARCH_CHICKEN_BOUNCE, com.oddlabs.tt.model.DeployType.RESEARCH_CRYSTAL_FACET, com.oddlabs.tt.model.DeployType.RESEARCH_DIAMOND_EDGE };
                                for (int k = 0; k < types.Length; k++)
                                {
                                    deploy_containers.put(types[k], new com.oddlabs.tt.model.ResearchContainer(this, types[k], com.oddlabs.tt.model.ResearchContainer.CRYSTAL_CRAFT + k));
                                }
                                break;
                            }
                            case 8:
                            {
                                deploy_containers.put(com.oddlabs.tt.model.DeployType.RESEARCH_RUNE_REACH, new com.oddlabs.tt.model.ResearchContainer(this, com.oddlabs.tt.model.DeployType.RESEARCH_RUNE_REACH, com.oddlabs.tt.model.ResearchContainer.RUNE_REACH));
                                deploy_containers.put(com.oddlabs.tt.model.DeployType.RESEARCH_RUNE_MIGHT, new com.oddlabs.tt.model.ResearchContainer(this, com.oddlabs.tt.model.DeployType.RESEARCH_RUNE_MIGHT, com.oddlabs.tt.model.ResearchContainer.RUNE_MIGHT));
                                deploy_containers.put(com.oddlabs.tt.model.DeployType.RESEARCH_RUNE_WARD, new com.oddlabs.tt.model.ResearchContainer(this, com.oddlabs.tt.model.DeployType.RESEARCH_RUNE_WARD, com.oddlabs.tt.model.ResearchContainer.RUNE_WARD));
                                break;
                            }
                            case 9:
                            {
                                deploy_containers.put(com.oddlabs.tt.model.DeployType.RESEARCH_SHIP_OARS, new com.oddlabs.tt.model.ResearchContainer(this, com.oddlabs.tt.model.DeployType.RESEARCH_SHIP_OARS, com.oddlabs.tt.model.ResearchContainer.SHIP_OARS));
                                deploy_containers.put(com.oddlabs.tt.model.DeployType.RESEARCH_SHIP_HULL, new com.oddlabs.tt.model.ResearchContainer(this, com.oddlabs.tt.model.DeployType.RESEARCH_SHIP_HULL, com.oddlabs.tt.model.ResearchContainer.SHIP_HULL));
                                deploy_containers.put(com.oddlabs.tt.model.DeployType.RESEARCH_FLEET, new com.oddlabs.tt.model.ResearchContainer(this, com.oddlabs.tt.model.DeployType.RESEARCH_FLEET, com.oddlabs.tt.model.ResearchContainer.FLEET));
                                deploy_containers.put(com.oddlabs.tt.model.DeployType.RESEARCH_CARGO, new com.oddlabs.tt.model.ResearchContainer(this, com.oddlabs.tt.model.DeployType.RESEARCH_CARGO, com.oddlabs.tt.model.ResearchContainer.CARGO));
                                deploy_containers.put(com.oddlabs.tt.model.DeployType.RESEARCH_BRIDGES, new com.oddlabs.tt.model.ResearchContainer(this, com.oddlabs.tt.model.DeployType.RESEARCH_BRIDGES, com.oddlabs.tt.model.ResearchContainer.BRIDGES));
                                break;
                            }
                            case 10:
                            {
                                for (int k_2 = com.oddlabs.tt.model.DeployType.MARKET_WOOD.ordinal(); k_2 <= com.oddlabs.tt.model.DeployType.GIFT_GOLD.ordinal(); k_2++)
                                {
                                    com.oddlabs.tt.model.DeployType type = com.oddlabs.tt.model.DeployType.values()[k_2];
                                    deploy_containers.put(type, new com.oddlabs.tt.model.MarketContainer(this, type));
                                }
                                break;
                            }
                            case 6:
                            {
                                bool vik = com.oddlabs.tt.model.ExtendedCombat.isVikings(getOwner());
                                deploy_containers.put(com.oddlabs.tt.model.DeployType.MINOR_GOD, new com.oddlabs.tt.model.GodContainer(this, com.oddlabs.tt.model.DeployType.MINOR_GOD, com.oddlabs.tt.model.Race.UNIT_MINOR_GOD, com.oddlabs.tt.model.NewTidesStats.weapons(vik, com.oddlabs.tt.model.Race.UNIT_MINOR_GOD, 1), com.oddlabs.tt.model.LandBuilding.MINOR_GOD_COST, com.oddlabs.tt.model.NewTidesStats.trainSeconds(vik, com.oddlabs.tt.model.Race.UNIT_MINOR_GOD, 40.0f)));
                                deploy_containers.put(com.oddlabs.tt.model.DeployType.MAJOR_GOD, new com.oddlabs.tt.model.GodContainer(this, com.oddlabs.tt.model.DeployType.MAJOR_GOD, com.oddlabs.tt.model.Race.UNIT_MAJOR_GOD, com.oddlabs.tt.model.NewTidesStats.weapons(vik, com.oddlabs.tt.model.Race.UNIT_MAJOR_GOD, 2), com.oddlabs.tt.model.LandBuilding.MAJOR_GOD_COST[(vik ? 1 : 0)], com.oddlabs.tt.model.NewTidesStats.trainSeconds(vik, com.oddlabs.tt.model.Race.UNIT_MAJOR_GOD, 70.0f)));
                                deploy_containers.put(com.oddlabs.tt.model.DeployType.RESEARCH_RACE_1, new com.oddlabs.tt.model.ResearchContainer(this, com.oddlabs.tt.model.DeployType.RESEARCH_RACE_1, com.oddlabs.tt.model.ResearchContainer.RACE_1));
                                deploy_containers.put(com.oddlabs.tt.model.DeployType.RESEARCH_RACE_2, new com.oddlabs.tt.model.ResearchContainer(this, com.oddlabs.tt.model.DeployType.RESEARCH_RACE_2, com.oddlabs.tt.model.ResearchContainer.RACE_2));
                                deploy_containers.put(com.oddlabs.tt.model.DeployType.RESEARCH_FOCUS, new com.oddlabs.tt.model.ResearchContainer(this, com.oddlabs.tt.model.DeployType.RESEARCH_FOCUS, com.oddlabs.tt.model.ResearchContainer.FOCUS));
                                deploy_containers.put(com.oddlabs.tt.model.DeployType.RESEARCH_PROSPECTING, new com.oddlabs.tt.model.ResearchContainer(this, com.oddlabs.tt.model.DeployType.RESEARCH_PROSPECTING, com.oddlabs.tt.model.ResearchContainer.PROSPECTING));
                                deploy_containers.put(com.oddlabs.tt.model.DeployType.RESEARCH_ELITE_RANKS, new com.oddlabs.tt.model.ResearchContainer(this, com.oddlabs.tt.model.DeployType.RESEARCH_ELITE_RANKS, com.oddlabs.tt.model.ResearchContainer.ELITE_RANKS));
                                deploy_containers.put(com.oddlabs.tt.model.DeployType.RESEARCH_ELITE_TRAINING, new com.oddlabs.tt.model.ResearchContainer(this, com.oddlabs.tt.model.DeployType.RESEARCH_ELITE_TRAINING, com.oddlabs.tt.model.ResearchContainer.ELITE_TRAINING));
                                break;
                            }
                            default:
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }
        // Java: 869
        public static bool isPlacingLegal(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, com.oddlabs.tt.model.BuildingTemplate template, int grid_x, int grid_y)
        {
            if (template.hasCompactFootprint() && (unit_grid.getHeightMap().getWorld().isFreeForm() || unit_grid.getHeightMap().getWorld().isExtended()))
            {
                return com.oddlabs.tt.model.LandBuilding.freeFormPlacingLegal(unit_grid, grid_x, grid_y, template.getPlacingSize(), true);
            }
            return com.oddlabs.tt.model.LandBuilding.doIsPlacingLegal(unit_grid, grid_x, grid_y, template.getPlacingSize());
        }
        public com.oddlabs.tt.pathfinder.Region bridge_region;
        // Java: 879
        public com.oddlabs.tt.pathfinder.Region bridgeRegion()
        {
            return bridge_region;
        }
        // Java: 883
        public void setBridgeRegion(com.oddlabs.tt.pathfinder.Region region)
        {
            bridge_region = region;
        }
        public int bridge_sea_mask;
        // Java: 889
        public int bridgeSeaMask()
        {
            return bridge_sea_mask;
        }
        // Java: 893
        public void setBridgeSeaMask(int mask)
        {
            bridge_sea_mask = mask;
        }
        // Java: 898
        public bool onBridgeDeck()
        {
            com.oddlabs.tt.pathfinder.UnitGrid grid = getUnitGrid();
            for (int dy = (-2); dy <= 2; dy++)
            {
                for (int dx = (-2); dx <= 2; dx++)
                {
                    if (grid.isBridgeDeck(getGridX() + dx, getGridY() + dy))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        // Java: 907
        public bool isBridge()
        {
            return ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getType() == com.oddlabs.tt.model.BuildingTemplate.TYPE_BRIDGE;
        }
        // Java: 911
        public override bool isPlacingLegal()
        {
            if (isBridge())
            {
                return !isDead() && getOwner().canBuild(((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getTemplateID()) && com.oddlabs.tt.model.BridgeSpan.isPlacingLegal(getUnitGrid(), getGridX(), getGridY());
            }
            return !isDead() && getOwner().canBuild(((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getTemplateID()) && com.oddlabs.tt.model.LandBuilding.doIsPlacingLegal(getUnitGrid(), getGridX(), getGridY(), ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getPlacingSize() - com.oddlabs.tt.model.LandBuilding.PLACING_BORDER);
        }
        // Java: 920
        public override bool isPlaced()
        {
            return build_points != 0;
        }
        // Java: 925
        public override bool isComplete()
        {
            return build_points == getEffectiveMaxHitPoints();
        }
        // Java: 929
        public override float getHitOffsetZ()
        {
            return ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getHitOffsetZ(getRenderLevel().ordinal());
        }
        // Java: 938
        public static bool inRoundFootprint(bool free_form, int x, int y, int n)
        {
            if (!free_form)
            {
                return true;
            }
            int dx2 = 2 * x - (n - 1);
            int dy2 = 2 * y - (n - 1);
            int r2 = n + 1;
            return dx2 * dx2 + dy2 * dy2 <= r2 * r2 - ((n > 3 ? 2 * n : 0));
        }
        // Java: 951
        public static int freeFormDiameter(int placing_size)
        {
            return ((placing_size) switch { 6 or 5 => (int)(5), 4 => (int)(5), 3 => (int)(3), 2 => (int)(1), _ => (int)(java.lang.Math.max(1, 2 * placing_size - 3)) });
        }
        // Java: 962
        public static bool inFreeFormFootprint(int dx, int dy, int n)
        {
            return com.oddlabs.tt.model.LandBuilding.inFreeFormFootprint(dx, dy, n, false);
        }
        // Java: 968
        public static bool inFreeFormFootprint(int dx, int dy, int n, bool compact)
        {
            int r2 = n * n - ((n == 3 ? 2 : 0)) - ((compact && n == 5 ? 6 : 0));
            return 4 * (dx * dx + dy * dy) <= r2;
        }
        // Java: 973
        public static bool freeFormPlacingLegal(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, int grid_x, int grid_y, int size)
        {
            return com.oddlabs.tt.model.LandBuilding.freeFormPlacingLegal(unit_grid, grid_x, grid_y, size, false);
        }
        // Java: 977
        public static bool freeFormPlacingLegal(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, int grid_x, int grid_y, int size, bool compact)
        {
            int n = com.oddlabs.tt.model.LandBuilding.freeFormDiameter(size);
            int h = n / 2;
            int gsize = unit_grid.getGridSize();
            for (int dy = -h; dy <= h; dy++)
            {
                for (int dx = -h; dx <= h; dx++)
                {
                    if (!com.oddlabs.tt.model.LandBuilding.inFreeFormFootprint(dx, dy, n, compact))
                    {
                        continue;
                    }
                    int cx = grid_x + dx;
                    int cy = grid_y + dy;
                    if (cx >= gsize || cy >= gsize || cx < 0 || cy < 0)
                    {
                        return false;
                    }
                    com.oddlabs.tt.pathfinder.Occupant occ = unit_grid.getOccupant(cx, cy);
                    if (occ != null && !((occ is com.oddlabs.tt.model.Unit)))
                    {
                        return false;
                    }
                    if (!unit_grid.getHeightMap().canBuild(cx, cy, 1) && !unit_grid.isBridgeDeck(cx, cy))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        // Java: 999
        public static bool doIsPlacingLegal(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, int grid_x, int grid_y, int size)
        {
            bool free_form = unit_grid.getHeightMap().getWorld().isFreeForm();
            if (free_form || unit_grid.getHeightMap().getWorld().isExtended())
            {
                return com.oddlabs.tt.model.LandBuilding.freeFormPlacingLegal(unit_grid, grid_x, grid_y, size);
            }
            if (!free_form && !unit_grid.getHeightMap().canBuild(grid_x, grid_y, size))
            {
                return false;
            }
            int gsize = unit_grid.getGridSize();
            for (int y = 0; y < size * 2 - 1; y++)
            {
                for (int x = 0; x < size * 2 - 1; x++)
                {
                    if (!com.oddlabs.tt.model.LandBuilding.inRoundFootprint(free_form, x, y, size * 2 - 1))
                    {
                        continue;
                    }
                    int cx = grid_x + x - (size - 1);
                    int cy = grid_y + y - (size - 1);
                    if (cx >= gsize || cy >= gsize || cx < 0 || cy < 0)
                    {
                        return false;
                    }
                    com.oddlabs.tt.pathfinder.Occupant occ = unit_grid.getOccupant(cx, cy);
                    if (occ != null && !((occ is com.oddlabs.tt.model.Unit)))
                    {
                        return false;
                    }
                    if (free_form && !unit_grid.getHeightMap().canBuild(cx, cy, 1))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        // Java: 1028
        public override com.oddlabs.tt.model.AttackScanFilter.Priority getAttackPriority()
        {
            return (getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.ATTACK) ? com.oddlabs.tt.model.AttackScanFilter.Priority.TOWER : (getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD_ARMIES) ? com.oddlabs.tt.model.AttackScanFilter.Priority.ARMORY : com.oddlabs.tt.model.AttackScanFilter.Priority.QUARTERS));
        }
        // Java: 1035
        public override void setTarget(com.oddlabs.tt.util.Target target, com.oddlabs.tt.model.Action action, bool aggressive)
        {
            if (getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.ATTACK))
            {
                if (((object)target != (object)this))
                {
                    com.oddlabs.tt.model.Unit unit = (((com.oddlabs.tt.model.MountUnitContainer)getUnitContainer())).getUnit();
                    bool kill_friendly = ((object)action == (object)com.oddlabs.tt.model.Action.ATTACK);
                    if (unit != null && unit.canAttack(target, kill_friendly))
                    {
                        unit.pushController(new com.oddlabs.tt.model.behaviour.AttackController(unit, ((com.oddlabs.tt.model.Selectable)target)));
                    }
                }
            }
            else
            {
                setRallyPoint(target);
            }
        }
        // Java: 1050
        public bool wallUncounted()
        {
            return getWorld().isExtended() && (((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getTemplateID() == com.oddlabs.tt.model.Race.BUILDING_PALISADE || isBridge());
        }
        // Java: 1054
        public override void place()
        {
            register();
            occupy();
            if (!isBridge() && !onBridgeDeck())
            {
                flattenLandscape();
            }
            if (!wallUncounted())
            {
                int result = getOwner().getBuildingCountContainer().increaseSupply(1);
            }
            build_points = 1;
            reinsert();
        }
        // Java: 1069
        public override float getSize()
        {
            if (isBridge())
            {
                return com.oddlabs.tt.model.BridgeSpan.SIZE;
            }
            float radius = (roundFootprint() ? (com.oddlabs.tt.model.LandBuilding.freeFormDiameter(((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getPlacingSize()) + 1) / 2 : (((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getPlacingSize() - 1));
            return (float)((float)(((float)(java.lang.Math.sqrt(2))) * radius) + 0.1f);
        }
        // Java: 1078
        public override int getPenalty()
        {
            return com.oddlabs.tt.pathfinder.Occupant.STATIC;
        }
        // Java: 1084
        public override void removeDying()
        {
            if (isBridge())
            {
                com.oddlabs.tt.model.BridgeSpan.close(this);
            }
            new com.oddlabs.tt.particle.RandomVelocityEmitter(getOwner().getWorld(), new org.joml.Vector3f(getPositionX(), getPositionY(), getPositionZ()), 0.0f, 0.0f, ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getSmokeRadius(), ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getSmokeHeight(), 1.0f, 1.0f, 30, 400.0f, new org.joml.Vector3f(0.0f, 0.0f, 0.1f), new org.joml.Vector3f(0.0f, 0.0f, (float)(-2.5f)), new org.joml.Vector4f(1.0f, 0.8f, 0.6f, 1.0f), new org.joml.Vector4f(0.0f, 0.0f, 0.0f, (float)(-1.0f)), new org.joml.Vector3f(1.0f, 1.0f, 1.0f), new org.joml.Vector3f(7.5f, 7.5f, 7.5f), 1, 0.75f, org.lwjgl.opengl.GL11.GL_SRC_ALPHA, org.lwjgl.opengl.GL11.GL_ONE_MINUS_SRC_ALPHA, getOwner().getWorld().getRacesResources().getSmokeTextures(), getOwner().getWorld().getAnimationManagerRealTime());
            remove_delay = com.oddlabs.tt.model.LandBuilding.REMOVE_DELAY;
            getOwner().getWorld().getAudio().newAudio(new com.oddlabs.tt.audio.AudioParameters(getOwner().getWorld().getRacesResources().getBuildingCollapseSound(), getPositionX(), getPositionY(), getPositionZ(), com.oddlabs.tt.audio.AudioPlayer.AUDIO_RANK_BUILDING_COLLAPSE, com.oddlabs.tt.audio.AudioPlayer.AUDIO_DISTANCE_BUILDING_COLLAPSE, com.oddlabs.tt.audio.AudioPlayer.AUDIO_GAIN_BUILDING_COLLAPSE, com.oddlabs.tt.audio.AudioPlayer.AUDIO_RADIUS_BUILDING_COLLAPSE));
            if (getUnitContainer() != null)
            {
                while (getUnitContainer().getNumSupplies() > 0)
                {
                    com.oddlabs.tt.model.Unit unit = getUnitContainer().exit();
                    if (unit != null)
                    {
                        unit.removeNow();
                    }
                }
            }
            com.oddlabs.tt.model.SupplyContainer worker_container = getBuildSupplyContainer(typeof(com.oddlabs.tt.model.Unit));
            if (worker_container != null)
            {
                int result = getOwner().getUnitCountContainer().increaseSupply(-worker_container.getNumSupplies());
            }
            foreach (com.oddlabs.tt.model.DeployContainer deploy_container in java.lang.JIter.of(deploy_containers.values()))
            {
                int result_2 = getOwner().getUnitCountContainer().increaseSupply(-deploy_container.getNumSupplies());
            }
            free();
            if (old_landscape_heights != null)
            {
                undoLandscape();
            }
            int result_3 = (wallUncounted() ? (-1) : getOwner().getBuildingCountContainer().increaseSupply((-1)));
            base.removeDying();
        }
        // Java: 1129
        public override bool isValidRallyPoint(com.oddlabs.tt.util.Target t)
        {
            if (!((t is com.oddlabs.tt.model.Building b)))
            {
                return false;
            }
            return ((object)getOwner() == (object)b.getOwner()) && b.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.RALLY_TO);
        }
        // Java: 1135
        public override void setRallyPoint(com.oddlabs.tt.util.Target target)
        {
            if (!getOwner().canSetRallyPoints())
            {
                return;
            }
            if (isValidRallyPoint(target))
            {
                rally_point = target;
            }
            else
            {
                com.oddlabs.tt.util.Target found = getUnitGrid().findGridTargets(target.getGridX(), target.getGridY(), 1, false)[0];
                if (found != null)
                {
                    rally_point = found;
                }
            }
        }
        // Java: 1153
        public override com.oddlabs.tt.model.Building.BuildState getRenderLevel()
        {
            return (build_points == getEffectiveMaxHitPoints() ? com.oddlabs.tt.model.Building.BuildState.BUILT : ((float)(((float)(build_points)) / getEffectiveMaxHitPoints()) < 0.5d ? com.oddlabs.tt.model.Building.BuildState.START : com.oddlabs.tt.model.Building.BuildState.HALFBUILT));
        }
        // Java: 1158
        public override com.oddlabs.tt.render.SpriteKey getSpriteRenderer()
        {
            com.oddlabs.tt.model.Building.BuildState render_level = getRenderLevel();
            return ((render_level).ordinal() switch { 0 /*START*/ => (com.oddlabs.tt.render.SpriteKey)(((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getStartRenderer()), 1 /*HALFBUILT*/ => (com.oddlabs.tt.render.SpriteKey)(((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getHalfbuiltRenderer()), 2 /*BUILT*/ => (com.oddlabs.tt.render.SpriteKey)(((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getBuiltRenderer()), _ => throw new System.InvalidOperationException("MatchException") });
        }
        // Java: 1168
        public override void visit(com.oddlabs.tt.model.ToolTipVisitor visitor)
        {
            visitor.visitBuilding(this);
        }
        // Java: 1174
        public bool roundFootprint()
        {
            return getOwner().getWorld().isFreeForm() || getOwner().getWorld().isExtended();
        }
        // Java: 1178
        public void flattenLandscape()
        {
            if (roundFootprint())
            {
                flattenRoundPad();
                return;
            }
            int size = ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getPlacingSize();
            int height_points = (size - com.oddlabs.tt.model.LandBuilding.PLACING_BORDER) * 2;
            int offset_x = getGridX() - (size - 1);
            int offset_y = getGridY() - (size - 1);
            float total_height = 0;
            old_landscape_heights = java.lang.JA.newArray2<float>(height_points, height_points);
            for (int y = 0; y < height_points; y++)
            {
                for (int x = 0; x < height_points; x++)
                {
                    float old_height = getOwner().getWorld().getHeightMap().getWrappedHeight(offset_x + x + com.oddlabs.tt.model.LandBuilding.PLACING_BORDER, offset_y + y + com.oddlabs.tt.model.LandBuilding.PLACING_BORDER);
                    old_landscape_heights[y][x] = old_height;
                    total_height = (float)(total_height + old_height);
                }
            }
            float new_height = (float)(total_height / (height_points * height_points));
            bool free_form = getOwner().getWorld().isFreeForm();
            for (int y_2 = 0; y_2 < height_points; y_2++)
            {
                for (int x_2 = 0; x_2 < height_points; x_2++)
                {
                    float h = new_height;
                    if (free_form)
                    {
                        int dx2 = 2 * x_2 - (height_points - 1);
                        int dy2 = 2 * y_2 - (height_points - 1);
                        int d2 = dx2 * dx2 + dy2 * dy2;
                        int r2 = height_points * height_points;
                        if (d2 > r2)
                        {
                            continue;
                        }
                        if (d2 * 2 > r2)
                        {
                            h = (float)(old_landscape_heights[y_2][x_2] + (float)(((float)(new_height - old_landscape_heights[y_2][x_2])) * ((float)(((float)((r2 - d2))) / ((float)((r2 - r2 / 2)))))));
                        }
                    }
                    getOwner().getWorld().getHeightMap().editHeight(offset_x + x_2 + com.oddlabs.tt.model.LandBuilding.PLACING_BORDER, offset_y + y_2 + com.oddlabs.tt.model.LandBuilding.PLACING_BORDER, h);
                }
            }
        }
        // Java: 1217
        public void undoLandscape()
        {
            if (roundFootprint())
            {
                for (int y = 0; y < old_landscape_heights.Length; y++)
                {
                    for (int x = 0; x < old_landscape_heights[y].Length; x++)
                    {
                        getOwner().getWorld().getHeightMap().editHeight(pad_x + x, pad_y + y, old_landscape_heights[y][x]);
                    }
                }
                return;
            }
            int size = ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getPlacingSize();
            int offset_x = getGridX() - (size - 1);
            int offset_y = getGridY() - (size - 1);
            for (int y_2 = 0; y_2 < old_landscape_heights.Length; y_2++)
            {
                for (int x_2 = 0; x_2 < old_landscape_heights[y_2].Length; x_2++)
                {
                    getOwner().getWorld().getHeightMap().editHeight(offset_x + x_2 + com.oddlabs.tt.model.LandBuilding.PLACING_BORDER, offset_y + y_2 + com.oddlabs.tt.model.LandBuilding.PLACING_BORDER, old_landscape_heights[y_2][x_2]);
                }
            }
        }
        // Java: 1239
        public void flattenRoundPad()
        {
            int n = com.oddlabs.tt.model.LandBuilding.freeFormDiameter(((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getPlacingSize());
            int points = n + 3;
            pad_x = getGridX() - (points - 1) / 2;
            pad_y = getGridY() - (points - 1) / 2;
            com.oddlabs.tt.landscape.HeightMap heightmap = getOwner().getWorld().getHeightMap();
            old_landscape_heights = java.lang.JA.newArray2<float>(points, points);
            float total = 0.0f;
            int count = 0;
            float c = (float)((points - 1) * 0.5f);
            float inner = (float)(n * 0.5f);
            float outer = (float)(inner + 1.5f);
            for (int y = 0; y < points; y++)
            {
                for (int x = 0; x < points; x++)
                {
                    float h = heightmap.getWrappedHeight(pad_x + x, pad_y + y);
                    old_landscape_heights[y][x] = h;
                    float d2 = (float)((float)(((float)(x - c)) * ((float)(x - c))) + (float)(((float)(y - c)) * ((float)(y - c))));
                    if (d2 <= (float)(inner * inner))
                    {
                        total = (float)(total + h);
                        count++;
                    }
                }
            }
            float level = (count > 0 ? (float)(total / count) : heightmap.getWrappedHeight(getGridX(), getGridY()));
            for (int y_2 = 0; y_2 < points; y_2++)
            {
                for (int x_2 = 0; x_2 < points; x_2++)
                {
                    float d = ((float)(java.lang.Math.sqrt((float)((float)(((float)(x_2 - c)) * ((float)(x_2 - c))) + (float)(((float)(y_2 - c)) * ((float)(y_2 - c)))))));
                    if (d >= outer)
                    {
                        continue;
                    }
                    float old = old_landscape_heights[y_2][x_2];
                    float h_2 = (d <= inner ? level : (float)(old + (float)(((float)(level - old)) * ((float)(((float)(outer - d)) / ((float)(outer - inner)))))));
                    heightmap.editHeight(pad_x + x_2, pad_y + y_2, h_2);
                }
            }
        }
        // Java: 1279
        public void freeFormOffset(float dx, float dy)
        {
            if (!getOwner().getWorld().isFreeForm() || isPlaced())
            {
                return;
            }
            float limit = (float)(com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID * 0.49f);
            setPosition((float)(com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(getGridX()) + java.lang.Math.max((float)(-limit), java.lang.Math.min(limit, dx))), (float)(com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(getGridY()) + java.lang.Math.max((float)(-limit), java.lang.Math.min(limit, dy))));
        }
        // Java: 1287
        public void occupy()
        {
            if (isBridge())
            {
                com.oddlabs.tt.model.BridgeSpan.occupy(this);
                return;
            }
            com.oddlabs.tt.pathfinder.UnitGrid grid = getUnitGrid();
            grid.getRegion(getGridX(), getGridY()).registerObject(typeof(com.oddlabs.tt.model.Building), this);
            if (roundFootprint())
            {
                int n = com.oddlabs.tt.model.LandBuilding.freeFormDiameter(((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getPlacingSize());
                java.util.Set trapped = new java.util.LinkedHashSet();
                for (int dy = -(n / 2); dy <= n / 2; dy++)
                {
                    for (int dx = -(n / 2); dx <= n / 2; dx++)
                    {
                        if (!com.oddlabs.tt.model.LandBuilding.inFreeFormFootprint(dx, dy, n, ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).hasCompactFootprint()))
                        {
                            continue;
                        }
                        com.oddlabs.tt.pathfinder.Occupant occ = grid.getOccupant(getGridX() + dx, getGridY() + dy);
                        if ((occ is com.oddlabs.tt.model.Unit unit))
                        {
                            trapped.add(unit);
                            unit.free();
                        }
                        grid.occupyGrid(getGridX() + dx, getGridY() + dy, this);
                    }
                }
                foreach (com.oddlabs.tt.model.Unit unit_2 in java.lang.JIter.of(trapped))
                {
                    unit_2.reposition();
                }
                return;
            }
            int size = ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getPlacingSize() * 2 - 1;
            bool free_form = getOwner().getWorld().isFreeForm();
            java.util.Set trappedUnits = new java.util.LinkedHashSet();
            for (int y = com.oddlabs.tt.model.LandBuilding.PLACING_BORDER; y < size - com.oddlabs.tt.model.LandBuilding.PLACING_BORDER; y++)
            {
                for (int x = com.oddlabs.tt.model.LandBuilding.PLACING_BORDER; x < size - com.oddlabs.tt.model.LandBuilding.PLACING_BORDER; x++)
                {
                    if (!com.oddlabs.tt.model.LandBuilding.inRoundFootprint(free_form, x - com.oddlabs.tt.model.LandBuilding.PLACING_BORDER, y - com.oddlabs.tt.model.LandBuilding.PLACING_BORDER, size - 2 * com.oddlabs.tt.model.LandBuilding.PLACING_BORDER))
                    {
                        continue;
                    }
                    int cx = getGridX() - size / 2 + x;
                    int cy = getGridY() - size / 2 + y;
                    com.oddlabs.tt.pathfinder.Occupant occ_2 = grid.getOccupant(cx, cy);
                    if ((occ_2 is com.oddlabs.tt.model.Unit unit_3))
                    {
                        trappedUnits.add(unit_3);
                        unit_3.free();
                    }
                    grid.occupyGrid(cx, cy, this);
                }
            }
            foreach (com.oddlabs.tt.model.Unit unit_4 in java.lang.JIter.of(trappedUnits))
            {
                unit_4.reposition();
            }
        }
        // Java: 1335
        public void free()
        {
            if (isBridge())
            {
                com.oddlabs.tt.model.BridgeSpan.close(this);
                com.oddlabs.tt.model.BridgeSpan.free(this);
                return;
            }
            com.oddlabs.tt.pathfinder.UnitGrid grid = getUnitGrid();
            grid.getRegion(getGridX(), getGridY()).unregisterObject(typeof(com.oddlabs.tt.model.Building), this);
            if (roundFootprint())
            {
                int n = com.oddlabs.tt.model.LandBuilding.freeFormDiameter(((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getPlacingSize());
                for (int dy = -(n / 2); dy <= n / 2; dy++)
                {
                    for (int dx = -(n / 2); dx <= n / 2; dx++)
                    {
                        if (com.oddlabs.tt.model.LandBuilding.inFreeFormFootprint(dx, dy, n, ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).hasCompactFootprint()))
                        {
                            grid.freeGrid(getGridX() + dx, getGridY() + dy, this);
                        }
                    }
                }
                return;
            }
            int size = ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getPlacingSize() * 2 - 1;
            bool free_form = getOwner().getWorld().isFreeForm();
            for (int y = com.oddlabs.tt.model.LandBuilding.PLACING_BORDER; y < size - com.oddlabs.tt.model.LandBuilding.PLACING_BORDER; y++)
            {
                for (int x = com.oddlabs.tt.model.LandBuilding.PLACING_BORDER; x < size - com.oddlabs.tt.model.LandBuilding.PLACING_BORDER; x++)
                {
                    if (!com.oddlabs.tt.model.LandBuilding.inRoundFootprint(free_form, x - com.oddlabs.tt.model.LandBuilding.PLACING_BORDER, y - com.oddlabs.tt.model.LandBuilding.PLACING_BORDER, size - 2 * com.oddlabs.tt.model.LandBuilding.PLACING_BORDER))
                    {
                        continue;
                    }
                    grid.freeGrid(getGridX() - size / 2 + x, getGridY() - size / 2 + y, this);
                }
            }
        }
        // Java: 1363
        public override void hit(int damage, float dir_x, float dir_y, com.oddlabs.tt.player.Player owner)
        {
            base.hit(damage, dir_x, dir_y, owner);
            if (!isDead())
            {
                int armor = armorLevel();
                if (armor > 0 && damage > 0 && (++armor_hits % (5 - armor)) == 0)
                {
                    damage = 0;
                }
                setHitPoints(hit_points - damage);
                com.oddlabs.tt.landscape.World world = getOwner().getWorld();
                world.getAudio().newAudio(new com.oddlabs.tt.audio.AudioParameters(world.getRacesResources().getBuildingHitSound(world.getRandom()), getPositionX(), getPositionY(), getPositionZ(), com.oddlabs.tt.audio.AudioPlayer.AUDIO_RANK_WEAPON_HIT, com.oddlabs.tt.audio.AudioPlayer.AUDIO_DISTANCE_WEAPON_HIT, com.oddlabs.tt.audio.AudioPlayer.AUDIO_GAIN_WEAPON_HIT, com.oddlabs.tt.audio.AudioPlayer.AUDIO_RADIUS_WEAPON_HIT));
                if (hit_points == 0)
                {
                    getOwner().buildingLost();
                    owner.buildingDestroyed();
                    if (is_training_chieftain)
                    {
                        getOwner().setTrainingChieftain(false);
                    }
                    if (world.isExtended() && ((object)owner != (object)getOwner()) && owner.isEnemy(getOwner()))
                    {
                        plunder(owner);
                    }
                    removeDying();
                }
            }
        }
        public const float PLUNDER_SHARE = 0.3f;
        // Java: 1393
        public void plunder(com.oddlabs.tt.player.Player attacker)
        {
            com.oddlabs.tt.model.LandBuilding to = null;
            float best = java.lang.Float.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(attacker.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.LandBuilding b) && !b.isDead() && b.isPlaced() && b.isComplete() && b.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD_ARMIES) && b.getSupplyContainer(typeof(com.oddlabs.tt.landscape.TreeSupply)) != null)
                {
                    float dx = (float)(b.getPositionX() - getPositionX());
                    float dy = (float)(b.getPositionY() - getPositionY());
                    float d = (float)((float)(dx * dx) + (float)(dy * dy));
                    if (d < best)
                    {
                        best = d;
                        to = b;
                    }
                }
            }
            if (to == null)
            {
                return;
            }
            foreach (System.Type key in com.oddlabs.tt.model.LandBuilding.PLUNDER_KEYS)
            {
                com.oddlabs.tt.model.SupplyContainer from = getSupplyContainer(key);
                com.oddlabs.tt.model.SupplyContainer into = to.getSupplyContainer(key);
                if (from == null || into == null)
                {
                    continue;
                }
                int n = java.lang.JC.f2i(((float)(from.getNumSupplies() * com.oddlabs.tt.model.LandBuilding.PLUNDER_SHARE)));
                if (n > 0)
                {
                    into.increaseSupply(java.lang.Math.min(n, into.getMaxSupplyCount() - into.getNumSupplies()));
                }
            }
            com.oddlabs.tt.model.UnitContainer peons = getUnitContainer();
            com.oddlabs.tt.model.UnitContainer into_2 = to.getUnitContainer();
            if (peons != null && into_2 != null)
            {
                int room = attacker.getWorld().getMaxUnitCount() - attacker.getUnitCountContainer().getNumSupplies();
                int n_2 = java.lang.Math.min(java.lang.JC.f2i(((float)(peons.getNumSupplies() * com.oddlabs.tt.model.LandBuilding.PLUNDER_SHARE))), java.lang.Math.max(0, room));
                n_2 = into_2.capAmount(n_2);
                if (n_2 > 0)
                {
                    into_2.increaseSupply(n_2);
                }
            }
        }
        public static System.Type[] PLUNDER_KEYS;
        // Java: 1433
        public override string toString()
        {
            return "Building at " + (getGridX()) + "," + (getGridY());
        }
        // Java: 1438
        public override void fillSupplies(System.Type key, int max)
        {
            com.oddlabs.tt.model.SupplyContainer container = getSupplyContainer(key);
            if (container != null)
            {
                container.increaseSupply(java.lang.Math.min(container.getMaxSupplyCount() - container.getNumSupplies(), max));
            }
        }
        // Java: 1445
        public override void removeSupplies(System.Type key)
        {
            com.oddlabs.tt.model.SupplyContainer container = getSupplyContainer(key);
            if (container != null)
            {
                container.increaseSupply(-container.getNumSupplies());
            }
        }
        // Java: 1452
        public override int getStatusValue()
        {
            return (getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.REPRODUCE) ? getUnitContainer().getNumSupplies() : (getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD_ARMIES) ? getUnitContainer().getNumSupplies() + getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)).getNumSupplies() + getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)).getNumSupplies() * 3 + getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)).getNumSupplies() * 8 : 0));
        }
        // Java: 1462
        public override void printDebugInfo()
        {
            java.lang.IO.println("-----------------------------------");
            if (getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.REPRODUCE))
            {
                java.lang.IO.println("Units = " + (getUnitContainer().getNumSupplies()));
            }
            else if (getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD_ARMIES))
            {
                java.lang.IO.println("Units = " + (getUnitContainer().getNumSupplies()));
                java.lang.IO.println("Tree = " + (getSupplyContainer(typeof(com.oddlabs.tt.landscape.TreeSupply)).getNumSupplies()));
                java.lang.IO.println("Rock = " + (getSupplyContainer(typeof(com.oddlabs.tt.model.RockSupply)).getNumSupplies()));
                java.lang.IO.println("Iron = " + (getSupplyContainer(typeof(com.oddlabs.tt.model.IronSupply)).getNumSupplies()));
                java.lang.IO.println("Rubber = " + (getSupplyContainer(typeof(com.oddlabs.tt.model.RubberSupply)).getNumSupplies()));
                java.lang.IO.println("Rock Weapons = " + (getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)).getNumSupplies()));
                java.lang.IO.println("Iron Weapons = " + (getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)).getNumSupplies()));
                java.lang.IO.println("Rubber Weapons = " + (getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)).getNumSupplies()));
            }
        }
        static LandBuilding()
        {
            LandBuilding.COST_ROCK_WEAPON = new com.oddlabs.tt.model.Cost(new System.Type[] { typeof(com.oddlabs.tt.landscape.TreeSupply), typeof(com.oddlabs.tt.model.RockSupply) }, new int[] { 2, 1 });
            LandBuilding.COST_IRON_WEAPON = new com.oddlabs.tt.model.Cost(new System.Type[] { typeof(com.oddlabs.tt.landscape.TreeSupply), typeof(com.oddlabs.tt.model.IronSupply) }, new int[] { 2, 1 });
            LandBuilding.COST_RUBBER_WEAPON = new com.oddlabs.tt.model.Cost(new System.Type[] { typeof(com.oddlabs.tt.landscape.TreeSupply), typeof(com.oddlabs.tt.model.RockSupply), typeof(com.oddlabs.tt.model.IronSupply), typeof(com.oddlabs.tt.model.RubberSupply) }, new int[] { 2, 1, 1, 1 });
            LandBuilding.UNIQUE_1_COST = new int[][] { new int[] { 0, 0, 0, 0, 0, 0, 3, 0 }, new int[] { 0, 0, 0, 0, 0, 0, 0, 3 } };
            LandBuilding.UNIQUE_2_COST = new int[][] { new int[] { 0, 0, 0, 0, 3, 2, 0, 0 }, new int[] { 0, 0, 0, 0, 5, 0, 0, 0 } };
            LandBuilding.NO_COST = new int[com.oddlabs.tt.model.ExtendedResources.NUM_STORES];
            LandBuilding.UNIQUE_3_COST = new int[][] { new int[] { 0, 0, 0, 0, 1, 2, 0, 0 }, new int[] { 0, 0, 0, 0, 3, 1, 0, 0 } };
            LandBuilding.MINOR_GOD_COST = new int[] { 0, 0, 0, 0, 10, 6, 0, 0, 1 };
            LandBuilding.ELITE_1_COST = new int[][] { new int[] { 0, 0, 0, 0, 6, 1, 2, 0 }, new int[] { 0, 0, 0, 0, 6, 1, 0, 2 } };
            LandBuilding.ELITE_2_COST = new int[] { 0, 0, 0, 0, 5, 2, 0, 0 };
            LandBuilding.ELITE_3_COST = new int[][] { new int[] { 0, 0, 0, 0, 4, 1, 3, 0 }, new int[] { 0, 0, 0, 0, 4, 1, 0, 3 } };
            LandBuilding.MAJOR_GOD_COST = new int[][] { new int[] { 0, 0, 0, 0, 20, 14, 8, 0, 3 }, new int[] { 0, 0, 0, 0, 20, 14, 0, 8, 3 } };
            LandBuilding.SPAWN_DIRS = new int[][] { new int[] { 1, 0 }, new int[] { 0, 1 }, new int[] { (-1), 0 }, new int[] { 0, (-1) }, new int[] { 1, 1 }, new int[] { (-1), 1 }, new int[] { (-1), (-1) }, new int[] { 1, (-1) } };
            LandBuilding.PLUNDER_KEYS = new System.Type[] { typeof(com.oddlabs.tt.landscape.TreeSupply), typeof(com.oddlabs.tt.model.RockSupply), typeof(com.oddlabs.tt.model.IronSupply), typeof(com.oddlabs.tt.model.RubberSupply), typeof(com.oddlabs.tt.model.GoldSupply), typeof(com.oddlabs.tt.model.CrystalSupply), typeof(com.oddlabs.tt.model.HerbSupply), typeof(com.oddlabs.tt.model.CoalSupply), typeof(com.oddlabs.tt.model.DiamondSupply), typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon), typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon), typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon), typeof(com.oddlabs.tt.model.weapon.CrystalWeapon), typeof(com.oddlabs.tt.model.weapon.DiamondWeapon), typeof(com.oddlabs.tt.model.weapon.DartWeapon) };
        }
        private void init_LandBuilding()
        {
            this.supply_containers = new java.util.HashMap();
            this.build_containers = new java.util.HashMap();
            this.deploy_containers = new java.util.EnumMap(typeof(com.oddlabs.tt.model.DeployType));
            this.chieftain_container = null;
            this.chieftains_produced = 0;
            this.weapons_producer = null;
            this.remove_delay = 0;
            this.hit_points = 1;
            this.build_points = 0;
            this.rally_point = this;
            this.is_training_chieftain = false;
        }
        public LandBuilding(com.oddlabs.tt.player.Player owner, com.oddlabs.tt.model.BuildingTemplate template, int grid_x, int grid_y) : base(owner, template)
        {
            init_LandBuilding();
            setGridPosition(grid_x, grid_y);
            com.oddlabs.tt.pathfinder.UnitGrid unit_grid = getUnitGrid();
            float x = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(grid_x);
            float y = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(grid_y);
            setPosition(x, y);
            pushController(new com.oddlabs.tt.model.behaviour.NullController(this));
            damaged_emitter = new com.oddlabs.tt.particle.RandomVelocityEmitter(getOwner().getWorld(), new org.joml.Vector3f(getPositionX(), getPositionY(), (float)(getPositionZ() + getHitOffsetZ())), 0.0f, 0.0f, 0.01f, 0.01f, 0.5f, 0.7f, (-1), 4.0f, new org.joml.Vector3f(0.0f, 0.0f, 5.0f), new org.joml.Vector3f(0.0f, 0.0f, 0.0f), new org.joml.Vector4f(0.3f, 0.3f, 0.3f, com.oddlabs.tt.model.LandBuilding.DAMAGED_PARTICLE_ALPHA), new org.joml.Vector4f(0.0f, 0.0f, 0.0f, 0.0f), new org.joml.Vector3f(1.5f, 1.5f, 1.5f), new org.joml.Vector3f(0.6f, 0.6f, 0.6f), 1.5f, 0.75f, org.lwjgl.opengl.GL11.GL_SRC_ALPHA, org.lwjgl.opengl.GL11.GL_ONE_MINUS_SRC_ALPHA, owner.getWorld().getRacesResources().getDamageSmokeTextures(), owner.getWorld().getAnimationManagerRealTime());
            damaged_emitter.stop();
            float xc = (float)(getPositionX() + ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getChimneyX());
            float yc = (float)(getPositionY() + ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getChimneyY());
            float zc = (float)(getPositionZ() + ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getChimneyZ());
            float energy = 4.0f;
            float alpha = 0.6f;
            production_emitter = new com.oddlabs.tt.particle.RandomAccelerationEmitter(owner.getWorld(), new org.joml.Vector3f(xc, yc, zc), 0.0f, 0.01f, 0.01f, 1.5f, 0.1f, (-1), 6.0f, new org.joml.Vector3f(0.0f, 0.0f, 1.3f), new org.joml.Vector3f(0.0f, 0.0f, 0.25f), 0.7f, new org.joml.Vector4f(0.35f, 0.35f, 0.35f, alpha), new org.joml.Vector4f(0.0f, 0.0f, 0.0f, (float)((float)(-alpha) / energy)), new org.joml.Vector3f(0.3f, 0.3f, 0.3f), new org.joml.Vector3f(0.5f, 0.5f, 0.5f), energy, 1.0f, org.lwjgl.opengl.GL11.GL_SRC_ALPHA, org.lwjgl.opengl.GL11.GL_ONE_MINUS_SRC_ALPHA, owner.getWorld().getRacesResources().getSmokeTextures(), owner.getWorld().getAnimationManagerRealTime());
            production_emitter.stop();
        }
    }
}
