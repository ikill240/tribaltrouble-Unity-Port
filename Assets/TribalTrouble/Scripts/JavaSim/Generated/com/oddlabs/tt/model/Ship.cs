// Translated from Ship.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 37
    public class Ship : com.oddlabs.tt.model.Building, com.oddlabs.tt.pathfinder.Movable
    {
        public const float REMOVE_DELAY = 0.1f;
        public const int MAX_SUPPLY_COUNT = 200;
        public const int OCCUPY_LENGTH_CELLS = 13;
        public const int OCCUPY_WIDTH_CELLS = 4;
        public static com.oddlabs.tt.model.Cost COST_ROCK_WEAPON;
        public static com.oddlabs.tt.model.Cost COST_IRON_WEAPON;
        public static com.oddlabs.tt.model.Cost COST_RUBBER_WEAPON;
        public const int KEY_DEPLOY_ROCK_WARRIOR = 0;
        public const int KEY_DEPLOY_IRON_WARRIOR = 1;
        public const int KEY_DEPLOY_RUBBER_WARRIOR = 2;
        public const int KEY_DEPLOY_PEON = 3;
        public const int KEY_DEPLOY_PEON_HARVEST_TREE = 4;
        public const int KEY_DEPLOY_PEON_TRANSPORT_TREE = 5;
        public const int KEY_DEPLOY_PEON_HARVEST_ROCK = 6;
        public const int KEY_DEPLOY_PEON_TRANSPORT_ROCK = 7;
        public const int KEY_DEPLOY_PEON_HARVEST_IRON = 8;
        public const int KEY_DEPLOY_PEON_TRANSPORT_IRON = 9;
        public const int KEY_DEPLOY_PEON_HARVEST_RUBBER = 10;
        public const int KEY_DEPLOY_PEON_TRANSPORT_RUBBER = 11;
        // Java: 66
        public static System.Type deployTypeToGatherSupply(int deploy_type)
        {
            switch ((deploy_type))
            {
                case 4:
                {
                    return typeof(com.oddlabs.tt.landscape.TreeSupply);
                }
                case 6:
                {
                    return typeof(com.oddlabs.tt.model.RockSupply);
                }
                case 8:
                {
                    return typeof(com.oddlabs.tt.model.IronSupply);
                }
                case 10:
                {
                    return typeof(com.oddlabs.tt.model.RubberSupply);
                }
                default:
                {
                    return null;
                }
            }
        }
        public const float DAMAGED_PARTICLE_ALPHA = 3.0f;
        public java.util.Map supply_containers;
        public java.util.Map build_containers;
        public java.util.Map deploy_containers;
        public com.oddlabs.tt.particle.LinearEmitter damaged_emitter;
        public float remove_delay;
        public int hit_points;
        public int hull_hits;
        public int build_points;
        public bool slid_f;
        public bool flattened;
        public com.oddlabs.tt.util.Target rally_point;
        public com.oddlabs.tt.model.ShipProxy proxy;
        public com.oddlabs.tt.model.ShipHR ship_hr;
        public float anim_time;
        // Java: 156
        public static float[] getInitDirection(com.oddlabs.tt.pathfinder.UnitGrid grid, int x, int y)
        {
            float[] ret = new float[2];
            int grid_size = grid.getGridSize();
            int x0 = java.lang.Math.max(0, x - 8);
            int x1 = java.lang.Math.min(grid_size, x0 + 16);
            int y0 = java.lang.Math.max(0, y - 8);
            int y1 = java.lang.Math.min(grid_size, y0 + 16);
            int samples = 20;
            int best_gap = 0;
            double best_gap_dx = 0.0f;
            double best_gap_dy = 0.0f;
            double delta = java.lang.Math.toRadians((float)(360.0f / samples));
            for (int i = 0; i < samples; i++)
            {
                double angle = delta * i;
                double cos = java.lang.StrictMath.cos(angle);
                double sin = java.lang.StrictMath.sin(angle);
                int weight_a = 0;
                int weight_b = 0;
                for (int tmpy = y0; tmpy < y1; tmpy++)
                {
                    for (int tmpx = x0; tmpx < x1; tmpx++)
                    {
                        int h = (grid.isWater(tmpx, tmpy) ? 0 : 1);
                        int dx = tmpx - x;
                        int dy = tmpy - y;
                        double cross = dx * sin - dy * cos;
                        if (cross > 0)
                        {
                            weight_a += h;
                        }
                        else
                        {
                            weight_b += h;
                        }
                    }
                }
                int gap = weight_b - weight_a;
                if (i == 0 || gap < best_gap)
                {
                    best_gap = gap;
                    best_gap_dx = cos;
                    best_gap_dy = sin;
                }
            }
            ret[0] = ((float)(best_gap_dy));
            ret[1] = ((float)(-best_gap_dx));
            return ret;
        }
        // Java: 206
        public void setInitialShipDirection()
        {
            float[] dir = com.oddlabs.tt.model.Ship.getInitDirection(getUnitGrid(), getGridX(), getGridY());
            setDirection(dir[0], dir[1]);
        }
        // Java: 211
        public override float getOffsetZ()
        {
            return 0;
        }
        // Java: 215
        public override void visit(com.oddlabs.tt.model.ElementVisitor visitor)
        {
            visitor.visitBuilding(this);
        }
        // Java: 219
        public void flattenLandscape()
        {
            com.oddlabs.tt.pathfinder.UnitGrid grid = getUnitGrid();
            float center_x = getPositionX();
            float center_y = getPositionY();
            float dir_x = getDirectionX();
            float dir_y = getDirectionY();
            float half_length_meters = (float)((com.oddlabs.tt.model.Ship.OCCUPY_LENGTH_CELLS + 2) * com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID * 0.5f);
            float half_width_meters = (float)((com.oddlabs.tt.model.Ship.OCCUPY_WIDTH_CELLS + 2) * com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID * 0.5f);
            float half_diagonal_meters = ((float)(java.lang.StrictMath.sqrt((float)((float)(half_length_meters * half_length_meters) + (float)(half_width_meters * half_width_meters)))));
            int radius_cells = java.lang.JC.d2i(java.lang.StrictMath.ceil((float)(half_diagonal_meters / com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID))) + 1;
            int grid_size = grid.getGridSize();
            int start_x = java.lang.StrictMath.max(0, getGridX() - radius_cells);
            int end_x = java.lang.StrictMath.min(grid_size - 1, getGridX() + radius_cells);
            int start_y = java.lang.StrictMath.max(0, getGridY() - radius_cells);
            int end_y = java.lang.StrictMath.min(grid_size - 1, getGridY() + radius_cells);
            com.oddlabs.tt.landscape.HeightMap hm = getOwner().getWorld().getHeightMap();
            float h = (float)(hm.getSeaLevelMeters() + 0.1f);
            for (int y = start_y; y <= end_y; y++)
            {
                for (int x = start_x; x <= end_x; x++)
                {
                    if (com.oddlabs.tt.model.Ship.IsInsideShape(x, y, center_x, center_y, dir_x, dir_y, half_length_meters, half_width_meters))
                    {
                        float curr = hm.getWrappedHeight(x, y);
                        if (curr > h)
                        {
                            hm.editHeight(x, y, h);
                        }
                    }
                }
            }
        }
        // Java: 261
        public com.oddlabs.tt.model.BuildingTemplate getBuildingTemplate()
        {
            return ((com.oddlabs.tt.model.BuildingTemplate)((com.oddlabs.tt.model.BuildingTemplate)getTemplate()));
        }
        // Java: 265
        public override bool hasRallyPoint()
        {
            return ((object)rally_point != (object)this);
        }
        // Java: 269
        public override com.oddlabs.tt.util.Target getRallyPoint()
        {
            return rally_point;
        }
        // Java: 274
        public override void doAnimate(float t)
        {
            if (!isDead() && focus != null)
            {
                chaseFocus(t);
            }
            if (!isDead())
            {
                if (!isMoving() && proxy != null)
                {
                    com.oddlabs.tt.model.UnitContainer unit_container = getUnitContainer();
                    if (unit_container != null)
                    {
                        unit_container.animate(t);
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
            }
            if (remove_delay > 0)
            {
                remove_delay -= t;
                if (remove_delay <= 0)
                {
                    remove();
                    damaged_emitter.done();
                    float energy = 3.0f;
                    float fade_speed = 2.5f;
                    new com.oddlabs.tt.particle.RandomVelocityEmitter(getOwner().getWorld(), new org.joml.Vector3f(getPositionX(), getPositionY(), getPositionZ()), 0.0f, ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getSmokeRadius(), ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getSmokeHeight(), 0.05f, ((float)(java.lang.Math.PI)), ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getNumFragments(), ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getNumFragments(), new org.joml.Vector3f(0.0f, 0.0f, 5.0f), new org.joml.Vector3f(0.0f, 0.0f, (float)(-25.0f)), new org.joml.Vector4f(1.0f, 1.0f, 1.0f, (float)(energy * fade_speed)), new org.joml.Vector4f(0.0f, 0.0f, 0.0f, (float)(-fade_speed)), new org.joml.Vector3f(1.0f, 1.0f, 1.0f), new org.joml.Vector3f(0.0f, 0.0f, 0.0f), energy, 0.75f, getOwner().getWorld().getRacesResources().getWoodFragments(), getOwner().getWorld().getAnimationManagerRealTime());
                }
            }
        }
        // Java: 322
        public override com.oddlabs.tt.model.UnitContainer getUnitContainer()
        {
            return ((com.oddlabs.tt.model.UnitContainer)((com.oddlabs.tt.model.SupplyContainer)supply_containers.get(typeof(com.oddlabs.tt.model.Unit))));
        }
        // Java: 327
        public override com.oddlabs.tt.model.SupplyContainer getSupplyContainer(System.Type key)
        {
            return ((com.oddlabs.tt.model.SupplyContainer)((com.oddlabs.tt.model.SupplyContainer)supply_containers.get(key)));
        }
        // Java: 332
        public override com.oddlabs.tt.model.DeployContainer getDeployContainer(com.oddlabs.tt.model.DeployType type)
        {
            return ((com.oddlabs.tt.model.ShipDeployContainer)deploy_containers.get(type));
        }
        // Java: 337
        public override com.oddlabs.tt.model.ChieftainContainer getChieftainContainer()
        {
            return null;
        }
        // Java: 341
        public override bool isEnabled()
        {
            return !isDead();
        }
        // Java: 345
        public override int getUnitCount()
        {
            return getUnitContainer().getNumSupplies();
        }
        // Java: 351
        public com.oddlabs.tt.model.ShipHR getShipHR()
        {
            return ship_hr;
        }
        // Java: 355
        public override com.oddlabs.tt.model.Building getEntrance()
        {
            return (proxy != null && !proxy.isDead() ? (com.oddlabs.tt.model.Building)(proxy) : (com.oddlabs.tt.model.Building)(this));
        }
        // Java: 360
        public void removeProxy()
        {
            if (proxy != null && !proxy.isDead())
            {
                proxy.removeDying();
            }
            proxy = null;
        }
        // Java: 367
        public void updateProxy()
        {
            if (build_points < getBuildingTemplate().getMaxHitPoints())
            {
                return;
            }
            com.oddlabs.tt.pathfinder.UnitGrid grid = getUnitGrid();
            float half_length_meters = (float)((com.oddlabs.tt.model.Ship.OCCUPY_LENGTH_CELLS + 8) * com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID * 0.5f);
            int cx = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate((float)(getPositionX() + (float)(getDirectionX() * half_length_meters)));
            int cy = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate((float)(getPositionY() + (float)(getDirectionY() * half_length_meters)));
            int grid_size = grid.getGridSize();
            for (int radius = 0; radius <= 8; radius++)
            {
                for (int dy = -radius; dy <= radius; dy++)
                {
                    for (int dx = -radius; dx <= radius; dx++)
                    {
                        int nx = cx + dx;
                        int ny = cy + dy;
                        if (nx < 0 || ny < 0 || nx >= grid_size || ny >= grid_size)
                        {
                            continue;
                        }
                        if (grid.getRegion(nx, ny, com.oddlabs.tt.pathfinder.UnitGrid.LAND) != null && !grid.isWater(nx, ny) && (!grid.isGridOccupied(nx, ny, com.oddlabs.tt.pathfinder.UnitGrid.LAND) || ((object)grid.getOccupant(nx, ny, com.oddlabs.tt.pathfinder.UnitGrid.LAND) == (object)proxy)))
                        {
                            if (proxy != null && !proxy.isDead())
                            {
                                if (proxy.getGridX() != nx || proxy.getGridY() != ny)
                                {
                                    proxy.moveTo(nx, ny);
                                }
                            }
                            else
                            {
                                proxy = new com.oddlabs.tt.model.ShipProxy(nx, ny, this);
                                proxy.place();
                            }
                            return;
                        }
                    }
                }
            }
            removeProxy();
        }
        // Java: 402
        public override bool canExitTower()
        {
            return false;
        }
        // Java: 406
        public override void exitTower()
        {
        }
        // Java: 409
        public override void deployUnits(com.oddlabs.tt.model.DeployType type, int num_units)
        {
            if (getDeployContainer(type) == null)
            {
                return;
            }
            getOwner().getWorld().updateGlobalChecksum(type.ordinal());
            getOwner().getWorld().updateGlobalChecksum(num_units);
            getDeployContainer(type).orderSupply(num_units);
        }
        // Java: 419
        public override void createHarvesters(int num_tree, int num_rock, int num_iron, int num_rubber)
        {
            createHarvesters(typeof(com.oddlabs.tt.landscape.TreeSupply), num_tree);
            createHarvesters(typeof(com.oddlabs.tt.model.RockSupply), num_rock);
            createHarvesters(typeof(com.oddlabs.tt.model.IronSupply), num_iron);
            createHarvesters(typeof(com.oddlabs.tt.model.RubberSupply), num_rubber);
        }
        // Java: 427
        public void createHarvesters(System.Type supply_type, int amount)
        {
            com.oddlabs.tt.model.Race race = getOwner().getRace();
            for (int i = 0; i < amount; i++)
            {
                getUnitContainer().prepareDeploy((-1));
                getUnitContainer().exit();
                com.oddlabs.tt.model.Unit unit = createUnit(null, race.getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
                if (unit != null)
                {
                    unit.pushController(new com.oddlabs.tt.model.behaviour.GatherController(unit, null, supply_type, this));
                }
            }
        }
        // Java: 439
        public override bool canBuildChieftain()
        {
            return false;
        }
        // Java: 443
        public override bool canStopChieftain()
        {
            return false;
        }
        // Java: 447
        public override void trainChieftain(bool start)
        {
        }
        // Java: 450
        public override void deployChieftain()
        {
        }
        // Java: 453
        public com.oddlabs.tt.model.Unit createUnit(com.oddlabs.tt.util.Target rally_point, com.oddlabs.tt.model.UnitTemplate template)
        {
            if (proxy != null && ship_hr != null)
            {
                com.oddlabs.tt.model.Unit unit = ship_hr.exitUnit(template);
                if (unit != null && rally_point != null)
                {
                    unit.setTarget(rally_point, com.oddlabs.tt.model.Action.MOVE, false);
                }
                return unit;
            }
            return null;
        }
        // Java: 464
        public override void createArmy(int num_peon, int num_rock, int num_iron, int num_rubber)
        {
            createArmy(num_peon, com.oddlabs.tt.model.Race.UNIT_PEON);
            createArmy(num_rock, com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK);
            createArmy(num_iron, com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON);
            createArmy(num_rubber, com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER);
        }
        // Java: 472
        public void createArmy(int amount, int template)
        {
            com.oddlabs.tt.model.Race race = getOwner().getRace();
            checkRallyPoint();
            for (int i = 0; i < amount; i++)
            {
                getUnitContainer().prepareDeploy((-1));
                getUnitContainer().exit();
                com.oddlabs.tt.model.Unit unit = createUnit((hasRallyPoint() ? rally_point : (com.oddlabs.tt.util.Target)(null)), race.getUnitTemplate(template));
            }
        }
        // Java: 483
        public override void createTransporters(int num_tree, int num_rock, int num_iron, int num_rubber)
        {
            createTransporters(num_tree, typeof(com.oddlabs.tt.landscape.TreeSupply));
            createTransporters(num_rock, typeof(com.oddlabs.tt.model.RockSupply));
            createTransporters(num_iron, typeof(com.oddlabs.tt.model.IronSupply));
            createTransporters(num_rubber, typeof(com.oddlabs.tt.model.RubberSupply));
        }
        // Java: 491
        public void checkRallyPoint()
        {
            if (hasRallyPoint() && rally_point.isDead())
            {
                rally_point = this;
            }
        }
        // Java: 495
        public void createTransporters(int amount, System.Type supply)
        {
            com.oddlabs.tt.model.Race race = getOwner().getRace();
            checkRallyPoint();
            for (int i = 0; i < amount; i++)
            {
                getUnitContainer().prepareDeploy((-1));
                getUnitContainer().exit();
                com.oddlabs.tt.model.Unit unit = createUnit((hasRallyPoint() ? rally_point : (com.oddlabs.tt.util.Target)(null)), race.getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
                if (unit != null)
                {
                    unit.getSupplyContainer().increaseSupply(unit.getSupplyContainer().getMaxSupplyCount(), supply);
                }
            }
        }
        // Java: 510
        public override bool isDamaged()
        {
            return hit_points > 0 && hit_points < getBuildingTemplate().getMaxHitPoints();
        }
        // Java: 515
        public override int getHitPoints()
        {
            return hit_points;
        }
        // Java: 519
        public void setHitPoints(int new_hit_points)
        {
            float MIN_ENERGY = 3.0f;
            float MAX_ENERGY = 5.0f;
            int START_SMOKE = getBuildingTemplate().getMaxHitPoints() / 2;
            hit_points = java.lang.StrictMath.max(java.lang.StrictMath.min(new_hit_points, getBuildingTemplate().getMaxHitPoints()), 0);
            if (build_points == getBuildingTemplate().getMaxHitPoints() && hit_points < START_SMOKE)
            {
                float energy = (float)(MIN_ENERGY + ((float)(((float)(1 - (float)(((float)(hit_points)) / (START_SMOKE)))) * ((float)(MAX_ENERGY - MIN_ENERGY)))));
                damaged_emitter.start();
                damaged_emitter.setDeltaColor(new org.joml.Vector4f(0.0f, 0.0f, 0.0f, (float)((float)(-com.oddlabs.tt.model.Ship.DAMAGED_PARTICLE_ALPHA) / energy)));
                damaged_emitter.setEnergy(energy);
            }
            else
            {
                damaged_emitter.stop();
            }
        }
        // Java: 534
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
                    ship_hr = new com.oddlabs.tt.model.ShipHR(this, ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).isVikings());
                    updateProxy();
                    getOwner().getWorld().getNotificationListener().newSelectableNotification(this);
                    getAbilities().addAbilities(((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getAbilities());
                    supply_containers.put(typeof(com.oddlabs.tt.model.Unit), new com.oddlabs.tt.model.ShipUnitContainer(this));
                    com.oddlabs.tt.model.SupplyContainer tree_supply = new com.oddlabs.tt.model.SupplyContainer(com.oddlabs.tt.model.Ship.MAX_SUPPLY_COUNT);
                    com.oddlabs.tt.model.SupplyContainer rock_supply = new com.oddlabs.tt.model.SupplyContainer(com.oddlabs.tt.model.Ship.MAX_SUPPLY_COUNT);
                    com.oddlabs.tt.model.SupplyContainer iron_supply = new com.oddlabs.tt.model.SupplyContainer(com.oddlabs.tt.model.Ship.MAX_SUPPLY_COUNT);
                    com.oddlabs.tt.model.SupplyContainer rubber_supply = new com.oddlabs.tt.model.SupplyContainer(com.oddlabs.tt.model.Ship.MAX_SUPPLY_COUNT);
                    supply_containers.put(typeof(com.oddlabs.tt.landscape.TreeSupply), tree_supply);
                    supply_containers.put(typeof(com.oddlabs.tt.model.RockSupply), rock_supply);
                    supply_containers.put(typeof(com.oddlabs.tt.model.IronSupply), iron_supply);
                    supply_containers.put(typeof(com.oddlabs.tt.model.RubberSupply), rubber_supply);
                    supply_containers.put(typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon), new com.oddlabs.tt.model.ShipSupplyContainer(ship_hr, typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)));
                    supply_containers.put(typeof(com.oddlabs.tt.model.weapon.RockSpearWeapon), new com.oddlabs.tt.model.ShipSupplyContainer(ship_hr, typeof(com.oddlabs.tt.model.weapon.RockSpearWeapon)));
                    supply_containers.put(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon), new com.oddlabs.tt.model.ShipSupplyContainer(ship_hr, typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)));
                    supply_containers.put(typeof(com.oddlabs.tt.model.weapon.IronSpearWeapon), new com.oddlabs.tt.model.ShipSupplyContainer(ship_hr, typeof(com.oddlabs.tt.model.weapon.IronSpearWeapon)));
                    supply_containers.put(typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon), new com.oddlabs.tt.model.ShipSupplyContainer(ship_hr, typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)));
                    supply_containers.put(typeof(com.oddlabs.tt.model.weapon.RubberSpearWeapon), new com.oddlabs.tt.model.ShipSupplyContainer(ship_hr, typeof(com.oddlabs.tt.model.weapon.RubberSpearWeapon)));
                    deploy_containers.put(com.oddlabs.tt.model.DeployType.ROCK_WARRIOR, new com.oddlabs.tt.model.ShipDeployContainer(this, 1.0f, com.oddlabs.tt.model.DeployType.ROCK_WARRIOR, typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon), false));
                    deploy_containers.put(com.oddlabs.tt.model.DeployType.IRON_WARRIOR, new com.oddlabs.tt.model.ShipDeployContainer(this, 1.5f, com.oddlabs.tt.model.DeployType.IRON_WARRIOR, typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon), false));
                    deploy_containers.put(com.oddlabs.tt.model.DeployType.RUBBER_WARRIOR, new com.oddlabs.tt.model.ShipDeployContainer(this, 2.0f, com.oddlabs.tt.model.DeployType.RUBBER_WARRIOR, typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon), false));
                    deploy_containers.put(com.oddlabs.tt.model.DeployType.PEON, new com.oddlabs.tt.model.ShipDeployContainer(this, 0.5f, com.oddlabs.tt.model.DeployType.PEON, null, false));
                    deploy_containers.put(com.oddlabs.tt.model.DeployType.PEON_HARVEST_TREE, new com.oddlabs.tt.model.ShipDeployContainer(this, 0.5f, com.oddlabs.tt.model.DeployType.PEON_HARVEST_TREE, null, false));
                    deploy_containers.put(com.oddlabs.tt.model.DeployType.PEON_TRANSPORT_TREE, new com.oddlabs.tt.model.ShipDeployContainer(this, 0.5f, com.oddlabs.tt.model.DeployType.PEON_TRANSPORT_TREE, typeof(com.oddlabs.tt.landscape.TreeSupply), true));
                    deploy_containers.put(com.oddlabs.tt.model.DeployType.PEON_HARVEST_ROCK, new com.oddlabs.tt.model.ShipDeployContainer(this, 0.5f, com.oddlabs.tt.model.DeployType.PEON_HARVEST_ROCK, null, false));
                    deploy_containers.put(com.oddlabs.tt.model.DeployType.PEON_TRANSPORT_ROCK, new com.oddlabs.tt.model.ShipDeployContainer(this, 0.5f, com.oddlabs.tt.model.DeployType.PEON_TRANSPORT_ROCK, typeof(com.oddlabs.tt.model.RockSupply), true));
                    deploy_containers.put(com.oddlabs.tt.model.DeployType.PEON_HARVEST_IRON, new com.oddlabs.tt.model.ShipDeployContainer(this, 0.5f, com.oddlabs.tt.model.DeployType.PEON_HARVEST_IRON, null, false));
                    deploy_containers.put(com.oddlabs.tt.model.DeployType.PEON_TRANSPORT_IRON, new com.oddlabs.tt.model.ShipDeployContainer(this, 0.5f, com.oddlabs.tt.model.DeployType.PEON_TRANSPORT_IRON, typeof(com.oddlabs.tt.model.IronSupply), true));
                    deploy_containers.put(com.oddlabs.tt.model.DeployType.PEON_HARVEST_RUBBER, new com.oddlabs.tt.model.ShipDeployContainer(this, 0.5f, com.oddlabs.tt.model.DeployType.PEON_HARVEST_RUBBER, null, false));
                    deploy_containers.put(com.oddlabs.tt.model.DeployType.PEON_TRANSPORT_RUBBER, new com.oddlabs.tt.model.ShipDeployContainer(this, 0.5f, com.oddlabs.tt.model.DeployType.PEON_TRANSPORT_RUBBER, typeof(com.oddlabs.tt.model.RubberSupply), true));
                }
            }
        }
        // Java: 596
        public static bool isPlacingLegal(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, com.oddlabs.tt.model.BuildingTemplate template, int grid_x, int grid_y)
        {
            return com.oddlabs.tt.model.Ship.doIsPlacingLegal(unit_grid, grid_x, grid_y, 14);
        }
        // Java: 601
        public override bool isPlacingLegal()
        {
            return !isDead() && getOwner().canBuild(getBuildingTemplate().getTemplateID()) && com.oddlabs.tt.model.Ship.doIsPlacingLegal(getUnitGrid(), getGridX(), getGridY(), 14);
        }
        // Java: 607
        public override bool isPlaced()
        {
            return build_points != 0;
        }
        // Java: 612
        public override bool isComplete()
        {
            return build_points == getBuildingTemplate().getMaxHitPoints();
        }
        // Java: 616
        public override float getHitOffsetZ()
        {
            return ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getHitOffsetZ(getRenderLevel().ordinal());
        }
        // Java: 631
        public static bool doIsPlacingLegal(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, int grid_x, int grid_y, int size)
        {
            if (!unit_grid.isDockable(grid_x, grid_y))
            {
                return false;
            }
            if (unit_grid.getRegion(grid_x, grid_y, com.oddlabs.tt.pathfinder.UnitGrid.LAND) == null)
            {
                return false;   // (Steam 2.2.207 fix: a site the builders can't reach left them slacking)
            }
            float[] dir = com.oddlabs.tt.model.Ship.getInitDirection(unit_grid, grid_x, grid_y);
            float dir_x = dir[0];
            float dir_y = dir[1];
            int center_x = com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID * grid_x;
            int center_y = com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID * grid_y;
            float half_length_meters = (float)(com.oddlabs.tt.model.Ship.OCCUPY_LENGTH_CELLS * com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID * 0.5f);
            float half_width_meters = (float)(com.oddlabs.tt.model.Ship.OCCUPY_WIDTH_CELLS * com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID * 0.5f);
            float half_diagonal_meters = ((float)(java.lang.StrictMath.sqrt((float)((float)(half_length_meters * half_length_meters) + (float)(half_width_meters * half_width_meters)))));
            int radius_cells = java.lang.JC.d2i(java.lang.StrictMath.ceil((float)(half_diagonal_meters / com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID))) + 1;
            int grid_size = unit_grid.getGridSize();
            int start_x = java.lang.StrictMath.max(0, grid_x - radius_cells);
            int end_x = java.lang.StrictMath.min(grid_size - 1, grid_x + radius_cells);
            int start_y = java.lang.StrictMath.max(0, grid_y - radius_cells);
            int end_y = java.lang.StrictMath.min(grid_size - 1, grid_y + radius_cells);
            for (int y = start_y; y <= end_y; y++)
            {
                for (int x = start_x; x <= end_x; x++)
                {
                    if (com.oddlabs.tt.model.Ship.IsInsideShape(x, y, center_x, center_y, dir_x, dir_y, half_length_meters, half_width_meters))
                    {
                        if (!unit_grid.isWater(x, y) && unit_grid.isGridOccupied(x, y, com.oddlabs.tt.pathfinder.UnitGrid.LAND))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        // Java: 671
        public override com.oddlabs.tt.model.AttackScanFilter.Priority getAttackPriority()
        {
            return com.oddlabs.tt.model.AttackScanFilter.Priority.SHIP;
        }
        public com.oddlabs.tt.model.Selectable focus;
        public float focus_check;
        public const float FOCUS_STOP = 0.8f;
        // Java: 682
        public com.oddlabs.tt.model.Selectable getFocus()
        {
            if (focus != null && (focus.isDead() || !getOwner().isEnemy(focus.getOwner())))
            {
                focus = null;
            }
            return focus;
        }
        // Java: 689
        public bool focusInReach()
        {
            float range = (float)((float)((float)(com.oddlabs.tt.model.RacesResources.THROW_RANGE * com.oddlabs.tt.model.Ship.FOCUS_STOP) + focus.getSize()) + getSize());
            float dx = (float)(focus.getPositionX() - getPositionX());
            float dy = (float)(focus.getPositionY() - getPositionY());
            return (float)((float)(dx * dx) + (float)(dy * dy)) <= (float)(range * range);
        }
        // Java: 696
        public void chaseFocus(float t)
        {
            if ((focus_check += t) < 0.5f)
            {
                return;
            }
            focus_check = 0.0f;
            if (getFocus() == null)
            {
                return;
            }
            bool sailing = (getCurrentController() is com.oddlabs.tt.model.behaviour.SailController);
            if (focusInReach() && sailing)
            {
                popController();
            }
            else if (!focusInReach() && !sailing && slid())
            {
                pushController(new com.oddlabs.tt.model.behaviour.SailController(this, focus));
            }
        }
        // Java: 710
        public override void setTarget(com.oddlabs.tt.util.Target target, com.oddlabs.tt.model.Action action, bool aggressive)
        {
            focus = (getOwner().getWorld().isExtended() && (target is com.oddlabs.tt.model.Selectable s) && ((object)s != (object)this) && !s.isDead() && getOwner().isEnemy(s.getOwner()) ? s : (com.oddlabs.tt.model.Selectable)(null));
            if (focus != null && focusInReach())
            {
                forceDecide();
                clearControllerStack();
                pushController(new com.oddlabs.tt.model.behaviour.NullController(this));
                return;
            }
            forceDecide();
            clearControllerStack();
            pushController(new com.oddlabs.tt.model.behaviour.NullController(this));
            pushController(new com.oddlabs.tt.model.behaviour.SailController(this, target));
            free();
            occupy();
        }
        // Java: 736
        public void instantBuild()
        {
            setLayer(com.oddlabs.tt.pathfinder.UnitGrid.SEA);
            register();
            occupy();
            int result = getOwner().getBuildingCountContainer().increaseSupply(1);
            reinsert();
            build_points = 1;
            repair(((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getMaxHitPoints());
            slid_f = true;
        }
        // Java: 748
        public override void place()
        {
            register();
            occupy();
            int result = getOwner().getBuildingCountContainer().increaseSupply(1);
            build_points = 1;
            reinsert();
            if (!flattened)
            {
                flattenLandscape();
                flattened = true;
            }
        }
        // Java: 763
        public override float getSize()
        {
            float radius = (getBuildingTemplate().getPlacingSize() - 1);
            return (float)((float)(((float)(java.lang.StrictMath.sqrt(2))) * radius) + 0.1f);
        }
        // Java: 768
        public override int getPenalty()
        {
            return com.oddlabs.tt.pathfinder.Occupant.STATIC;
        }
        // Java: 772
        public override void removeDying()
        {
            removeProxy();
            pushController(new com.oddlabs.tt.model.behaviour.NullController(this));
            forceDecide();
            if (ship_hr != null)
            {
                ship_hr.killCrew();
            }
            new com.oddlabs.tt.particle.RandomVelocityEmitter(getOwner().getWorld(), new org.joml.Vector3f(getPositionX(), getPositionY(), getPositionZ()), 0.0f, 0.0f, getBuildingTemplate().getSmokeRadius(), getBuildingTemplate().getSmokeHeight(), 1.0f, 1.0f, 30, 400.0f, new org.joml.Vector3f(0.0f, 0.0f, 0.1f), new org.joml.Vector3f(0.0f, 0.0f, (float)(-2.5f)), new org.joml.Vector4f(1.0f, 0.8f, 0.6f, 1.0f), new org.joml.Vector4f(0.0f, 0.0f, 0.0f, (float)(-1.0f)), new org.joml.Vector3f(1.0f, 1.0f, 1.0f), new org.joml.Vector3f(7.5f, 7.5f, 7.5f), 1, 0.75f, org.lwjgl.opengl.GL11.GL_SRC_ALPHA, org.lwjgl.opengl.GL11.GL_ONE_MINUS_SRC_ALPHA, getOwner().getWorld().getRacesResources().getSmokeTextures(), getOwner().getWorld().getAnimationManagerRealTime());
            remove_delay = com.oddlabs.tt.model.Ship.REMOVE_DELAY;
            getOwner().getWorld().getAudio().newAudio(new com.oddlabs.tt.audio.AudioParameters(getOwner().getWorld().getRacesResources().getBuildingCollapseSound(), getPositionX(), getPositionY(), getPositionZ(), com.oddlabs.tt.audio.AudioPlayer.AUDIO_RANK_BUILDING_COLLAPSE, com.oddlabs.tt.audio.AudioPlayer.AUDIO_DISTANCE_BUILDING_COLLAPSE, com.oddlabs.tt.audio.AudioPlayer.AUDIO_GAIN_BUILDING_COLLAPSE, com.oddlabs.tt.audio.AudioPlayer.AUDIO_RADIUS_BUILDING_COLLAPSE));
            free();
            int result = getOwner().getBuildingCountContainer().increaseSupply((-1));
            base.removeDying();
        }
        // Java: 824
        public override bool isValidRallyPoint(com.oddlabs.tt.util.Target t)
        {
            if (!((t is com.oddlabs.tt.model.Building)))
            {
                return false;
            }
            com.oddlabs.tt.model.Building b = ((com.oddlabs.tt.model.Building)t);
            if (b != null)
            {
                return ((object)getOwner() == (object)b.getOwner()) && b.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.RALLY_TO);
            }
            com.oddlabs.tt.model.Ship s = ((com.oddlabs.tt.model.Ship)t);
            return ((object)getOwner() == (object)s.getOwner()) && s.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.RALLY_TO);
        }
        // Java: 834
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
                com.oddlabs.tt.util.Target found = getUnitGrid().findGridTargets(target.getGridX(), target.getGridY(), 1, false, com.oddlabs.tt.pathfinder.UnitGrid.LAND)[0];
                if (found != null)
                {
                    rally_point = found;
                }
            }
        }
        // Java: 849
        public override com.oddlabs.tt.model.Building.BuildState getRenderLevel()
        {
            if (build_points == getBuildingTemplate().getMaxHitPoints())
            {
                return com.oddlabs.tt.model.Building.BuildState.BUILT;
            }
            else if ((float)(((float)(build_points)) / getBuildingTemplate().getMaxHitPoints()) < 0.5d)
            {
                return com.oddlabs.tt.model.Building.BuildState.START;
            }
            else
            {
                return com.oddlabs.tt.model.Building.BuildState.HALFBUILT;
            }
        }
        // Java: 857
        public override com.oddlabs.tt.render.SpriteKey getSpriteRenderer()
        {
            com.oddlabs.tt.model.Building.BuildState render_level = getRenderLevel();
            return ((render_level).ordinal() switch { 0 /*START*/ => (com.oddlabs.tt.render.SpriteKey)(((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getStartRenderer()), 1 /*HALFBUILT*/ => (com.oddlabs.tt.render.SpriteKey)(((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getHalfbuiltRenderer()), 2 /*BUILT*/ => (com.oddlabs.tt.render.SpriteKey)(((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getBuiltRenderer()), _ => throw new System.InvalidOperationException("MatchException") });
        }
        // Java: 867
        public override void visit(com.oddlabs.tt.model.ToolTipVisitor visitor)
        {
            visitor.visitBuilding(this);
        }
        // Java: 871
        public static bool IsInsideShape(int grid_x, int grid_y, float center_x, float center_y, float dir_x, float dir_y, float half_length_meters, float half_width_meters)
        {
            float cell_x = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(grid_x);
            float cell_y = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(grid_y);
            float rel_x = (float)(cell_x - center_x);
            float rel_y = (float)(cell_y - center_y);
            float along = (float)((float)(rel_x * dir_x) + (float)(rel_y * dir_y));
            float side = (float)((float)(rel_x * ((float)(-dir_y))) + (float)(rel_y * dir_x));
            return java.lang.StrictMath.abs(along) <= half_length_meters && java.lang.StrictMath.abs(side) <= half_width_meters;
        }
        // Java: 892
        public void occupy()
        {
            com.oddlabs.tt.pathfinder.UnitGrid grid = getUnitGrid();
            float center_x = getPositionX();
            float center_y = getPositionY();
            float dir_x = getDirectionX();
            float dir_y = getDirectionY();
            float half_length_meters = (float)(com.oddlabs.tt.model.Ship.OCCUPY_LENGTH_CELLS * com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID * 0.5f);
            float half_width_meters = (float)(com.oddlabs.tt.model.Ship.OCCUPY_WIDTH_CELLS * com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID * 0.5f);
            float half_diagonal_meters = ((float)(java.lang.StrictMath.sqrt((float)((float)(half_length_meters * half_length_meters) + (float)(half_width_meters * half_width_meters)))));
            int radius_cells = java.lang.JC.d2i(java.lang.StrictMath.ceil((float)(half_diagonal_meters / com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID))) + 1;
            int grid_size = grid.getGridSize();
            int start_x = java.lang.StrictMath.max(0, getGridX() - radius_cells);
            int end_x = java.lang.StrictMath.min(grid_size - 1, getGridX() + radius_cells);
            int start_y = java.lang.StrictMath.max(0, getGridY() - radius_cells);
            int end_y = java.lang.StrictMath.min(grid_size - 1, getGridY() + radius_cells);
            for (int y = start_y; y <= end_y; y++)
            {
                for (int x = start_x; x <= end_x; x++)
                {
                    if (com.oddlabs.tt.model.Ship.IsInsideShape(x, y, center_x, center_y, dir_x, dir_y, half_length_meters, half_width_meters))
                    {
                        if (!grid.isGridOccupied(x, y, getLayer()))
                        {
                            grid.occupyGrid(x, y, this, getLayer());
                        }
                    }
                }
            }
            updateProxy();
        }
        // Java: 931
        public void free()
        {
            com.oddlabs.tt.pathfinder.UnitGrid grid = getUnitGrid();
            float center_x = getPositionX();
            float center_y = getPositionY();
            float dir_x = getDirectionX();
            float dir_y = getDirectionY();
            float half_length_meters = (float)(com.oddlabs.tt.model.Ship.OCCUPY_LENGTH_CELLS * com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID * 0.5f);
            float half_width_meters = (float)(com.oddlabs.tt.model.Ship.OCCUPY_WIDTH_CELLS * com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID * 0.5f);
            float half_diagonal_meters = ((float)(java.lang.StrictMath.sqrt((float)((float)(half_length_meters * half_length_meters) + (float)(half_width_meters * half_width_meters)))));
            int radius_cells = java.lang.JC.d2i(java.lang.StrictMath.ceil((float)(half_diagonal_meters / com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID))) + 1;
            int grid_size = grid.getGridSize();
            int start_x = java.lang.StrictMath.max(0, getGridX() - radius_cells);
            int end_x = java.lang.StrictMath.min(grid_size - 1, getGridX() + radius_cells);
            int start_y = java.lang.StrictMath.max(0, getGridY() - radius_cells);
            int end_y = java.lang.StrictMath.min(grid_size - 1, getGridY() + radius_cells);
            for (int y = start_y; y <= end_y; y++)
            {
                for (int x = start_x; x <= end_x; x++)
                {
                    if (com.oddlabs.tt.model.Ship.IsInsideShape(x, y, center_x, center_y, dir_x, dir_y, half_length_meters, half_width_meters))
                    {
                        // (Steam 2.2.207 fix: a ship launched from its build site still held the LAND cells there)
                        if (((object)grid.getOccupant(x, y, com.oddlabs.tt.pathfinder.UnitGrid.LAND) == (object)this))
                        {
                            grid.freeGrid(x, y, this, com.oddlabs.tt.pathfinder.UnitGrid.LAND);
                        }
                        if (((object)grid.getOccupant(x, y, com.oddlabs.tt.pathfinder.UnitGrid.SEA) == (object)this))
                        {
                            grid.freeGrid(x, y, this, com.oddlabs.tt.pathfinder.UnitGrid.SEA);
                        }
                    }
                }
            }
            removeProxy();
        }
        // Java: 969
        public com.oddlabs.tt.model.Unit pickVictim()
        {
            if (ship_hr != null)
            {
                com.oddlabs.tt.landscape.World world = getOwner().getWorld();
                float prob = world.getRandom().nextFloat();
                return ship_hr.pickVictim(prob);
            }
            return null;
        }
        // Java: 978
        public override void hit(int damage, float dir_x, float dir_y, com.oddlabs.tt.player.Player owner)
        {
            base.hit(damage, dir_x, dir_y, owner);
            if (!isDead())
            {
                int hull = (getOwner().getWorld().isExtended() ? getOwner().getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.SHIP_HULL) : 0);
                if (hull > 0 && damage > 0 && (++hull_hits % (5 - hull)) == 0)
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
                    removeDying();
                }
            }
        }
        // Java: 1005
        public override string toString()
        {
            return "Ship at " + (getGridX()) + "," + (getGridY());
        }
        // Java: 1009
        public override float getAnimationTicks()
        {
            return anim_time;
        }
        // Java: 1013
        public override int getAnimation()
        {
            return 0;
        }
        // Java: 1017
        public override void fillSupplies(System.Type key, int max)
        {
            com.oddlabs.tt.model.SupplyContainer container = getSupplyContainer(key);
            if (container != null)
            {
                container.increaseSupply((java.lang.StrictMath.min(container.getMaxSupplyCount() - container.getNumSupplies(), max)));
            }
        }
        // Java: 1027
        public override void removeSupplies(System.Type key)
        {
            com.oddlabs.tt.model.SupplyContainer container = getSupplyContainer(key);
            if (container != null)
            {
                container.increaseSupply(-container.getNumSupplies());
            }
        }
        // Java: 1034
        public override int getStatusValue()
        {
            return 0;
        }
        // Java: 1038
        public override void printDebugInfo()
        {
            java.lang.JSystem.@out.println("-----------------------------------");
            java.lang.JSystem.@out.println("Units = " + (getUnitContainer().getNumSupplies()));
            java.lang.JSystem.@out.println("Units = " + (getUnitContainer().getNumSupplies()));
            java.lang.JSystem.@out.println("Tree = " + (getSupplyContainer(typeof(com.oddlabs.tt.landscape.TreeSupply)).getNumSupplies()));
            java.lang.JSystem.@out.println("Rock = " + (getSupplyContainer(typeof(com.oddlabs.tt.model.RockSupply)).getNumSupplies()));
            java.lang.JSystem.@out.println("Iron = " + (getSupplyContainer(typeof(com.oddlabs.tt.model.IronSupply)).getNumSupplies()));
            java.lang.JSystem.@out.println("Rubber = " + (getSupplyContainer(typeof(com.oddlabs.tt.model.RubberSupply)).getNumSupplies()));
            java.lang.JSystem.@out.println("Rock Weapons = " + (getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)).getNumSupplies()));
            java.lang.JSystem.@out.println("Iron Weapons = " + (getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)).getNumSupplies()));
            java.lang.JSystem.@out.println("Rubber Weapons = " + (getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)).getNumSupplies()));
        }
        // Java: 1054
        public float getHealth()
        {
            return (float)(build_points / ((float)(getBuildingTemplate().getMaxHitPoints())));
        }
        // Java: 1058
        public bool isMoving()
        {
            return ((getCurrentController() is com.oddlabs.tt.model.behaviour.SailController));
        }
        // Java: 1062
        public void debugRender()
        {
            if ((getCurrentBehaviour() is com.oddlabs.tt.model.behaviour.SailBehaviour sail))
            {
                com.oddlabs.tt.pathfinder.ShipTrajectory traj = sail.getTrajectory();
                if (traj != null)
                {
                    traj.debugRender(getUnitGrid().getHeightMap());
                }
            }
        }
        // Java: 1071
        public com.oddlabs.tt.pathfinder.PathTracker getTracker()
        {
            return null;
        }
        // Java: 1075
        public void reportStuck()
        {
            forceDecide();
        }
        // Java: 1079
        public void endTrip()
        {
            forceDecide();
            clearControllerStack();
            pushController(new com.oddlabs.tt.model.behaviour.NullController(this));
            free();
            reinsert();
            occupy();
        }
        // Java: 1088
        public virtual bool slid()
        {
            return slid_f;
        }
        // Java: 1092
        public void endSlide()
        {
            forceDecide();
            free();
            reinsert();
            occupy();
            slid_f = true;
        }
        // Java: 1100
        public override void setPosition(float x, float y)
        {
            if (isDead())
            {
                return;
            }
            base.setPosition(x, y);
            damaged_emitter.setPosition(new org.joml.Vector3f(getPositionX(), getPositionY(), (float)(getPositionZ() + getHitOffsetZ())));
            float xc = (float)(x + getBuildingTemplate().getChimneyX());
            float yc = (float)(y + getBuildingTemplate().getChimneyY());
            float zc = (float)(getPositionZ() + getBuildingTemplate().getChimneyZ());
        }
        // Java: 1110
        public void setPosition(int x, int y)
        {
            if (isDead())
            {
                return;
            }
            free();
            base.setPosition(x, y);
            reinsert();
            occupy();
        }
        // Java: 1118
        public void markBlocking()
        {
        }
        // Java: 1121
        public override com.oddlabs.tt.model.BuildSupplyContainer getBuildSupplyContainer(System.Type key)
        {
            return null;
        }
        // Java: 1125
        public override void buildWeapons(System.Type type, int num_weapons, bool infinite)
        {
        }
        static Ship()
        {
            Ship.COST_ROCK_WEAPON = new com.oddlabs.tt.model.Cost(new System.Type[] { typeof(com.oddlabs.tt.landscape.TreeSupply), typeof(com.oddlabs.tt.model.RockSupply) }, new int[] { 2, 1 });
            Ship.COST_IRON_WEAPON = new com.oddlabs.tt.model.Cost(new System.Type[] { typeof(com.oddlabs.tt.landscape.TreeSupply), typeof(com.oddlabs.tt.model.IronSupply) }, new int[] { 2, 1 });
            Ship.COST_RUBBER_WEAPON = new com.oddlabs.tt.model.Cost(new System.Type[] { typeof(com.oddlabs.tt.landscape.TreeSupply), typeof(com.oddlabs.tt.model.RockSupply), typeof(com.oddlabs.tt.model.IronSupply), typeof(com.oddlabs.tt.model.RubberSupply) }, new int[] { 2, 1, 1, 1 });
        }
        private void init_Ship()
        {
            this.supply_containers = new java.util.HashMap();
            this.build_containers = new java.util.HashMap();
            this.deploy_containers = new java.util.EnumMap(typeof(com.oddlabs.tt.model.DeployType));
            this.remove_delay = 0;
            this.hit_points = 1;
            this.build_points = 0;
            this.slid_f = false;
            this.flattened = false;
            this.rally_point = this;
            this.proxy = null;
            this.ship_hr = null;
        }
        public Ship(com.oddlabs.tt.player.Player owner, com.oddlabs.tt.model.BuildingTemplate template, int grid_x, int grid_y) : base(owner, template)
        {
            init_Ship();
            setGridPosition(grid_x, grid_y);
            com.oddlabs.tt.pathfinder.UnitGrid unit_grid = getUnitGrid();
            float x = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(grid_x);
            float y = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(grid_y);
            base.setPosition(x, y);
            setPositionZ((float)(java.lang.Math.max(unit_grid.getHeightMap().getSeaLevelMeters(), unit_grid.getHeightMap().getNearestHeight(x, y)) + getOffsetZ()));
            setInitialShipDirection();
            pushController(new com.oddlabs.tt.model.behaviour.NullController(this));
            damaged_emitter = new com.oddlabs.tt.particle.RandomVelocityEmitter(getOwner().getWorld(), new org.joml.Vector3f(getPositionX(), getPositionY(), (float)(getPositionZ() + getHitOffsetZ())), 0.0f, 0.0f, 0.01f, 0.01f, 0.5f, 0.7f, (-1), 4.0f, new org.joml.Vector3f(0.0f, 0.0f, 5.0f), new org.joml.Vector3f(0.0f, 0.0f, 0.0f), new org.joml.Vector4f(0.3f, 0.3f, 0.3f, com.oddlabs.tt.model.Ship.DAMAGED_PARTICLE_ALPHA), new org.joml.Vector4f(0.0f, 0.0f, 0.0f, 0.0f), new org.joml.Vector3f(1.5f, 1.5f, 1.5f), new org.joml.Vector3f(0.6f, 0.6f, 0.6f), 1.5f, 0.75f, org.lwjgl.opengl.GL11.GL_SRC_ALPHA, org.lwjgl.opengl.GL11.GL_ONE_MINUS_SRC_ALPHA, owner.getWorld().getRacesResources().getDamageSmokeTextures(), owner.getWorld().getAnimationManagerRealTime());
            damaged_emitter.stop();
            float xc = (float)(getPositionX() + ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getChimneyX());
            float yc = (float)(getPositionY() + ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getChimneyY());
            float zc = (float)(getPositionZ() + ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getChimneyZ());
            float energy = 4.0f;
            float alpha = 0.6f;
        }
    }
}
