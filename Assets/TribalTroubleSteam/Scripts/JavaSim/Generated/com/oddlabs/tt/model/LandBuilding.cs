// Translated from LandBuilding.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 40
    public class LandBuilding : com.oddlabs.tt.model.Building
    {
        public const float REMOVE_DELAY = 0.1f;
        public const int PLACING_BORDER = 1;
        public const int MAX_SUPPLY_COUNT = 200;
        public static com.oddlabs.tt.model.Cost COST_ROCK_WEAPON;
        public static com.oddlabs.tt.model.Cost COST_IRON_WEAPON;
        public static com.oddlabs.tt.model.Cost COST_RUBBER_WEAPON;
        public const float DAMAGED_PARTICLE_ALPHA = 3.0f;
        public java.util.Map supply_containers;
        public java.util.Map build_containers;
        public java.util.Map deploy_containers;
        public com.oddlabs.tt.particle.LinearEmitter damaged_emitter;
        public com.oddlabs.tt.particle.LinearEmitter production_emitter;
        public com.oddlabs.tt.model.ChieftainContainer chieftain_container;
        public com.oddlabs.tt.model.WeaponsProducer weapons_producer;
        public float remove_delay;
        public int hit_points;
        public int build_points;
        public float[][] old_landscape_heights;
        public com.oddlabs.tt.util.Target rally_point;
        public bool is_training_chieftain;
        // Java: 124
        public override void visit(com.oddlabs.tt.model.ElementVisitor visitor)
        {
            visitor.visitBuilding(this);
        }
        // Java: 129
        public override bool hasRallyPoint()
        {
            return ((object)rally_point != (object)this);
        }
        // Java: 133
        public override com.oddlabs.tt.util.Target getRallyPoint()
        {
            return rally_point;
        }
        // Java: 137
        public override void onReinsert()
        {
            base.onReinsert();
            float xc = (float)(getPositionX() + ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getChimneyX());
            float yc = (float)(getPositionY() + ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getChimneyY());
            float zc = (float)(getPositionZ() + ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getChimneyZ());
            production_emitter.setPosition(new org.joml.Vector3f(xc, yc, zc));
        }
        // Java: 146
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
        // Java: 196
        public override com.oddlabs.tt.model.UnitContainer getUnitContainer()
        {
            return ((com.oddlabs.tt.model.UnitContainer)getSupplyContainer(typeof(com.oddlabs.tt.model.Unit)));
        }
        // Java: 201
        public override com.oddlabs.tt.model.SupplyContainer getSupplyContainer(System.Type key)
        {
            return ((com.oddlabs.tt.model.SupplyContainer)supply_containers.get(key));
        }
        // Java: 206
        public override com.oddlabs.tt.model.BuildSupplyContainer getBuildSupplyContainer(System.Type key)
        {
            return ((com.oddlabs.tt.model.BuildProductionContainer)build_containers.get(key));
        }
        // Java: 211
        public override com.oddlabs.tt.model.DeployContainer getDeployContainer(com.oddlabs.tt.model.DeployType type)
        {
            return ((com.oddlabs.tt.model.DeployContainer)deploy_containers.get(type));
        }
        // Java: 216
        public override com.oddlabs.tt.model.ChieftainContainer getChieftainContainer()
        {
            return chieftain_container;
        }
        // Java: 221
        public override bool isEnabled()
        {
            return !isDead();
        }
        // Java: 226
        public override int getUnitCount()
        {
            return getUnitContainer().getNumSupplies();
        }
        // Java: 231
        public override bool canExitTower()
        {
            return !isDead() && getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.ATTACK) && getUnitContainer().getNumSupplies() > 0 && getOwner().canExitTowers() && !(((((com.oddlabs.tt.model.MountUnitContainer)getUnitContainer())).getUnit().getCurrentController() is com.oddlabs.tt.model.behaviour.StunController));
        }
        // Java: 237
        public override void exitTower()
        {
            com.oddlabs.tt.model.UnitContainer container = getUnitContainer();
            if (canExitTower() && hasExitCell())
            {
                com.oddlabs.tt.model.Unit unit = container.exit();
            }
        }
        // Java: 250
        public override void deployUnits(com.oddlabs.tt.model.DeployType type, int num_units)
        {
            getOwner().getWorld().updateGlobalChecksum(type.ordinal());
            getOwner().getWorld().updateGlobalChecksum(num_units);
            getDeployContainer(type).orderSupply(num_units);
        }
        // Java: 257
        public override void createHarvesters(int num_tree, int num_rock, int num_iron, int num_rubber)
        {
            createHarvesters(typeof(com.oddlabs.tt.landscape.TreeSupply), num_tree);
            createHarvesters(typeof(com.oddlabs.tt.model.RockSupply), num_rock);
            createHarvesters(typeof(com.oddlabs.tt.model.IronSupply), num_iron);
            createHarvesters(typeof(com.oddlabs.tt.model.RubberSupply), num_rubber);
        }
        // Java: 265
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
        // Java: 275
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
        // Java: 284
        public override bool canBuildChieftain()
        {
            return !isDead() && chieftain_container != null && getOwner().canBuildChieftains() && !getOwner().hasActiveChieftain() && !getOwner().isTrainingChieftain();
        }
        // Java: 289
        public override bool canStopChieftain()
        {
            return !isDead() && chieftain_container != null && chieftain_container.isTraining();
        }
        // Java: 293
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
        // Java: 305
        public override void deployChieftain()
        {
            chieftain_container.stopTraining();
            getOwner().setTrainingChieftain(false);
            is_training_chieftain = false;
            com.oddlabs.tt.model.Unit chieftain = createUnit(null, getOwner().getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_CHIEFTAIN));
            getOwner().setActiveChieftain(chieftain);
        }
        // Java: 313
        public com.oddlabs.tt.model.Unit createUnit(com.oddlabs.tt.util.Target rally_point, com.oddlabs.tt.model.UnitTemplate template)
        {
            return new com.oddlabs.tt.model.Unit(getOwner(), getPositionX(), getPositionY(), rally_point, template, null, true, true);
        }
        // Java: 317
        public override void createArmy(int num_peon, int num_rock, int num_iron, int num_rubber)
        {
            createArmy(num_peon, com.oddlabs.tt.model.Race.UNIT_PEON);
            createArmy(num_rock, com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK);
            createArmy(num_iron, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON);
            createArmy(num_rubber, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER);
        }
        // Java: 325
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
        // Java: 338
        public override void createTransporters(int num_tree, int num_rock, int num_iron, int num_rubber)
        {
            createTransporters(num_tree, typeof(com.oddlabs.tt.landscape.TreeSupply));
            createTransporters(num_rock, typeof(com.oddlabs.tt.model.RockSupply));
            createTransporters(num_iron, typeof(com.oddlabs.tt.model.IronSupply));
            createTransporters(num_rubber, typeof(com.oddlabs.tt.model.RubberSupply));
        }
        // Java: 346
        public void checkRallyPoint()
        {
            if (hasRallyPoint() && rally_point.isDead())
            {
                rally_point = this;
            }
        }
        // Java: 351
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
        // Java: 365
        public override bool isDamaged()
        {
            return hit_points > 0 && hit_points < ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getMaxHitPoints();
        }
        // Java: 370
        public override int getHitPoints()
        {
            return hit_points;
        }
        // Java: 374
        public void setHitPoints(int new_hit_points)
        {
            float MIN_ENERGY = 3.0f;
            float MAX_ENERGY = 5.0f;
            int START_SMOKE = ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getMaxHitPoints() / 2;
            hit_points = java.lang.Math.max(java.lang.Math.min(new_hit_points, ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getMaxHitPoints()), 0);
            if (build_points == ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getMaxHitPoints() && hit_points < START_SMOKE)
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
        // Java: 388
        public override void repair(int amount)
        {
            if (!isDamaged())
            {
                return;
            }
            setHitPoints(hit_points + amount);
            if (build_points < ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getMaxHitPoints())
            {
                build_points = java.lang.Math.min(build_points + amount, ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getMaxHitPoints());
                reinsert();
                if (build_points == ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getMaxHitPoints())
                {
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
                    }
                    else if (getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.REPRODUCE))
                    {
                        chieftain_container = new com.oddlabs.tt.model.ChieftainContainer(this);
                        deploy_containers.put(com.oddlabs.tt.model.DeployType.PEON, new com.oddlabs.tt.model.DeployContainer(this, 0.5f, com.oddlabs.tt.model.DeployType.PEON, null));
                    }
                }
            }
        }
        // Java: 477
        public static bool isPlacingLegal(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, com.oddlabs.tt.model.BuildingTemplate template, int grid_x, int grid_y)
        {
            return com.oddlabs.tt.model.LandBuilding.doIsPlacingLegal(unit_grid, grid_x, grid_y, template.getPlacingSize());
        }
        // Java: 482
        public override bool isPlacingLegal()
        {
            return !isDead() && getOwner().canBuild(((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getTemplateID()) && com.oddlabs.tt.model.LandBuilding.doIsPlacingLegal(getUnitGrid(), getGridX(), getGridY(), ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getPlacingSize() - com.oddlabs.tt.model.LandBuilding.PLACING_BORDER);
        }
        // Java: 488
        public override bool isPlaced()
        {
            return build_points != 0;
        }
        // Java: 493
        public override bool isComplete()
        {
            return build_points == ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getMaxHitPoints();
        }
        // Java: 497
        public override float getHitOffsetZ()
        {
            return ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getHitOffsetZ(getRenderLevel().ordinal());
        }
        // Java: 502
        public static bool doIsPlacingLegal(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, int grid_x, int grid_y, int size)
        {
            if (!unit_grid.getHeightMap().canBuild(grid_x, grid_y, size))
            {
                return false;
            }
            int gsize = unit_grid.getGridSize();
            for (int y = 0; y < size * 2 - 1; y++)
            {
                for (int x = 0; x < size * 2 - 1; x++)
                {
                    int cx = grid_x + x - (size - 1);
                    int cy = grid_y + y - (size - 1);
                    if (cx >= gsize || cy >= gsize || cx < 0 || cy < 0)
                    {
                        return false;
                    }
                    com.oddlabs.tt.pathfinder.Occupant occ = unit_grid.getOccupant(cx, cy);
                    if (occ != null)
                    {
                        if ((occ is com.oddlabs.tt.model.Unit unit))
                        {
                            if (unit.isMoving())
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        // Java: 530
        public override com.oddlabs.tt.model.AttackScanFilter.Priority getAttackPriority()
        {
            return (getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.ATTACK) ? com.oddlabs.tt.model.AttackScanFilter.Priority.TOWER : (getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD_ARMIES) ? com.oddlabs.tt.model.AttackScanFilter.Priority.ARMORY : com.oddlabs.tt.model.AttackScanFilter.Priority.QUARTERS));
        }
        // Java: 537
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
        // Java: 551
        public override void place()
        {
            register();
            occupy();
            flattenLandscape();
            int result = getOwner().getBuildingCountContainer().increaseSupply(1);
            build_points = 1;
            reinsert();
        }
        // Java: 563
        public override float getSize()
        {
            float radius = (((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getPlacingSize() - 1);
            return (float)((float)(((float)(java.lang.Math.sqrt(2))) * radius) + 0.1f);
        }
        // Java: 569
        public override int getPenalty()
        {
            return com.oddlabs.tt.pathfinder.Occupant.STATIC;
        }
        // Java: 575
        public override void removeDying()
        {
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
            undoLandscape();
            int result_3 = getOwner().getBuildingCountContainer().increaseSupply((-1));
            base.removeDying();
        }
        // Java: 617
        public override bool isValidRallyPoint(com.oddlabs.tt.util.Target t)
        {
            if (!((t is com.oddlabs.tt.model.Building b)))
            {
                return false;
            }
            return ((object)getOwner() == (object)b.getOwner()) && b.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.RALLY_TO);
        }
        // Java: 623
        public override void setRallyPoint(com.oddlabs.tt.util.Target target)
        {
            if (!getOwner().canSetRallyPoints())
            {
                return;
            }
            rally_point = (isValidRallyPoint(target) ? target : getUnitGrid().findGridTargets(target.getGridX(), target.getGridY(), 1, false)[0]);
        }
        // Java: 630
        public override com.oddlabs.tt.model.Building.BuildState getRenderLevel()
        {
            return (build_points == ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getMaxHitPoints() ? com.oddlabs.tt.model.Building.BuildState.BUILT : ((float)(((float)(build_points)) / ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getMaxHitPoints()) < 0.5d ? com.oddlabs.tt.model.Building.BuildState.START : com.oddlabs.tt.model.Building.BuildState.HALFBUILT));
        }
        // Java: 635
        public override com.oddlabs.tt.render.SpriteKey getSpriteRenderer()
        {
            return getOwner().getSkins().rendererFor(((com.oddlabs.tt.model.BuildingTemplate)getTemplate()), getRenderLevel());
        }
        // Java: 640
        public override void visit(com.oddlabs.tt.model.ToolTipVisitor visitor)
        {
            visitor.visitBuilding(this);
        }
        // Java: 645
        public void flattenLandscape()
        {
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
            for (int y_2 = 0; y_2 < height_points; y_2++)
            {
                for (int x_2 = 0; x_2 < height_points; x_2++)
                {
                    getOwner().getWorld().getHeightMap().editHeight(offset_x + x_2 + com.oddlabs.tt.model.LandBuilding.PLACING_BORDER, offset_y + y_2 + com.oddlabs.tt.model.LandBuilding.PLACING_BORDER, new_height);
                }
            }
        }
        // Java: 670
        public void undoLandscape()
        {
            int size = ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getPlacingSize();
            int offset_x = getGridX() - (size - 1);
            int offset_y = getGridY() - (size - 1);
            for (int y = 0; y < old_landscape_heights.Length; y++)
            {
                for (int x = 0; x < old_landscape_heights[y].Length; x++)
                {
                    getOwner().getWorld().getHeightMap().editHeight(offset_x + x + com.oddlabs.tt.model.LandBuilding.PLACING_BORDER, offset_y + y + com.oddlabs.tt.model.LandBuilding.PLACING_BORDER, old_landscape_heights[y][x]);
                }
            }
        }
        // Java: 682
        public void occupy()
        {
            com.oddlabs.tt.pathfinder.UnitGrid grid = getUnitGrid();
            grid.getRegion(getGridX(), getGridY()).registerObject(typeof(com.oddlabs.tt.model.Building), this);
            int size = ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getPlacingSize() * 2 - 1;
            java.util.Set trappedUnits = new java.util.LinkedHashSet();
            for (int y = com.oddlabs.tt.model.LandBuilding.PLACING_BORDER; y < size - com.oddlabs.tt.model.LandBuilding.PLACING_BORDER; y++)
            {
                for (int x = com.oddlabs.tt.model.LandBuilding.PLACING_BORDER; x < size - com.oddlabs.tt.model.LandBuilding.PLACING_BORDER; x++)
                {
                    int cx = getGridX() - size / 2 + x;
                    int cy = getGridY() - size / 2 + y;
                    com.oddlabs.tt.pathfinder.Occupant occ = grid.getOccupant(cx, cy);
                    if ((occ is com.oddlabs.tt.model.Unit unit))
                    {
                        trappedUnits.add(unit);
                        unit.free();
                    }
                    grid.occupyGrid(cx, cy, this);
                }
            }
            foreach (com.oddlabs.tt.model.Unit unit_2 in java.lang.JIter.of(trappedUnits))
            {
                unit_2.reposition(this);
            }
        }
        // Java: 705
        public void free()
        {
            com.oddlabs.tt.pathfinder.UnitGrid grid = getUnitGrid();
            grid.getRegion(getGridX(), getGridY()).unregisterObject(typeof(com.oddlabs.tt.model.Building), this);
            int size = ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getPlacingSize() * 2 - 1;
            for (int y = com.oddlabs.tt.model.LandBuilding.PLACING_BORDER; y < size - com.oddlabs.tt.model.LandBuilding.PLACING_BORDER; y++)
            {
                for (int x = com.oddlabs.tt.model.LandBuilding.PLACING_BORDER; x < size - com.oddlabs.tt.model.LandBuilding.PLACING_BORDER; x++)
                {
                    grid.freeGrid(getGridX() - size / 2 + x, getGridY() - size / 2 + y, this);
                }
            }
        }
        // Java: 717
        public override void hit(int damage, float dir_x, float dir_y, com.oddlabs.tt.player.Player owner)
        {
            base.hit(damage, dir_x, dir_y, owner);
            if (!isDead())
            {
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
                    removeDying();
                }
            }
        }
        // Java: 738
        public override string toString()
        {
            return "Building at " + (getGridX()) + "," + (getGridY());
        }
        // Java: 743
        public override void fillSupplies(System.Type key, int max)
        {
            com.oddlabs.tt.model.SupplyContainer container = getSupplyContainer(key);
            if (container != null)
            {
                container.increaseSupply(java.lang.Math.min(container.getMaxSupplyCount() - container.getNumSupplies(), max));
            }
        }
        // Java: 750
        public override void removeSupplies(System.Type key)
        {
            com.oddlabs.tt.model.SupplyContainer container = getSupplyContainer(key);
            if (container != null)
            {
                container.increaseSupply(-container.getNumSupplies());
            }
        }
        // Java: 757
        public override int getStatusValue()
        {
            return (getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.REPRODUCE) ? getUnitContainer().getNumSupplies() : (getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD_ARMIES) ? getUnitContainer().getNumSupplies() + getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)).getNumSupplies() + getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)).getNumSupplies() * 3 + getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)).getNumSupplies() * 8 : 0));
        }
        // Java: 767
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
        }
        private void init_LandBuilding()
        {
            this.supply_containers = new java.util.LinkedHashMap();
            this.build_containers = new java.util.LinkedHashMap();
            this.deploy_containers = new java.util.EnumMap(typeof(com.oddlabs.tt.model.DeployType));
            this.chieftain_container = null;
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
