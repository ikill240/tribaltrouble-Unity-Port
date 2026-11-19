// Translated from ShipProxy.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 13
    public class ShipProxy : com.oddlabs.tt.model.Building
    {
        public com.oddlabs.tt.model.Ship ship;
        public com.oddlabs.tt.pathfinder.Region last_region;
        // Java: 28
        public override bool isEnabled()
        {
            return ship.isEnabled();
        }
        // Java: 32
        public override int getUnitCount()
        {
            return ship.getUnitCount();
        }
        // Java: 37
        public override void setTarget(com.oddlabs.tt.util.Target target, com.oddlabs.tt.model.Action action, bool aggressive)
        {
        }
        // Java: 41
        public override void visit(com.oddlabs.tt.model.ElementVisitor visitor)
        {
        }
        // Java: 45
        public override void visit(com.oddlabs.tt.model.ToolTipVisitor visitor)
        {
        }
        // Java: 49
        public void occupy()
        {
            com.oddlabs.tt.pathfinder.UnitGrid grid = getUnitGrid();
            last_region = grid.getRegion(getGridX(), getGridY(), com.oddlabs.tt.pathfinder.UnitGrid.LAND);
            if (last_region != null)
            {
                last_region.registerObject(typeof(com.oddlabs.tt.model.Building), this);
            }
            grid.occupyGrid(getGridX(), getGridY(), this, com.oddlabs.tt.pathfinder.UnitGrid.LAND);
        }
        // Java: 56
        public void free()
        {
            if (last_region != null)
            {
                last_region.unregisterObject(typeof(com.oddlabs.tt.model.Building), this);
                last_region = null;
            }
            getUnitGrid().freeGrid(getGridX(), getGridY(), this, com.oddlabs.tt.pathfinder.UnitGrid.LAND);
        }
        // Java: 64
        public void moveTo(int grid_x, int grid_y)
        {
            free();
            setGridPosition(grid_x, grid_y);
            setPosition(com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(grid_x), com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(grid_y));
            occupy();
        }
        // Java: 71
        public override void place()
        {
            register();
            occupy();
            getAbilities().addAbilities(((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getAbilities());
            reinsert();
        }
        // Java: 79
        public override void removeDying()
        {
            free();
            base.removeDying();
            remove();
        }
        // Java: 85
        public override void hit(int damage, float dir_x, float dir_y, com.oddlabs.tt.player.Player owner)
        {
            ship.hit(damage, dir_x, dir_y, owner);
        }
        // Java: 89
        public override com.oddlabs.tt.model.UnitContainer getUnitContainer()
        {
            return ship.getUnitContainer();
        }
        // Java: 93
        public override com.oddlabs.tt.model.SupplyContainer getSupplyContainer(System.Type key)
        {
            return ship.getSupplyContainer(key);
        }
        // Java: 97
        public override float getOffsetZ()
        {
            return 0.0f;
        }
        // Java: 101
        public com.oddlabs.tt.model.BuildingTemplate getBuildingTemplate()
        {
            return ((com.oddlabs.tt.model.BuildingTemplate)((com.oddlabs.tt.model.BuildingTemplate)getTemplate()));
        }
        // Java: 105
        public override float getSize()
        {
            return 0.0f;
        }
        // Java: 109
        public override int getPenalty()
        {
            return com.oddlabs.tt.pathfinder.Occupant.STATIC;
        }
        // Java: 113
        public override com.oddlabs.tt.model.Building.BuildState getRenderLevel()
        {
            return com.oddlabs.tt.model.Building.BuildState.BUILT;
        }
        // Java: 118
        public override com.oddlabs.tt.render.SpriteKey getSpriteRenderer()
        {
            return getOwner().getRace().getRallyPoint();
        }
        // Java: 123
        public override float getAnimationTicks()
        {
            return 0.0f;
        }
        // Java: 127
        public override int getAnimation()
        {
            return 0;
        }
        // Java: 131
        public override bool isPlaced()
        {
            return ship.isPlaced();
        }
        // Java: 135
        public override bool isComplete()
        {
            return ship.isComplete();
        }
        // Java: 139
        public override bool isDamaged()
        {
            return ship.isDamaged();
        }
        // Java: 143
        public override int getHitPoints()
        {
            return ship.getHitPoints();
        }
        // Java: 147
        public float getHealth()
        {
            return ship.getHealth();
        }
        // Java: 151
        public override void repair(int amount)
        {
            ship.repair(amount);
        }
        // Java: 155
        public override float getHitOffsetZ()
        {
            return 0.0f;
        }
        // Java: 159
        public override com.oddlabs.tt.model.AttackScanFilter.Priority getAttackPriority()
        {
            return ship.getAttackPriority();
        }
        // Java: 164
        public override bool isPlacingLegal()
        {
            return !isDead();
        }
        // Java: 168
        public override bool hasRallyPoint()
        {
            return ship.hasRallyPoint();
        }
        // Java: 172
        public override com.oddlabs.tt.util.Target getRallyPoint()
        {
            return ship.getRallyPoint();
        }
        // Java: 176
        public override bool isValidRallyPoint(com.oddlabs.tt.util.Target t)
        {
            return ship.isValidRallyPoint(t);
        }
        // Java: 180
        public override void setRallyPoint(com.oddlabs.tt.util.Target target)
        {
            ship.setRallyPoint(target);
        }
        // Java: 184
        public override com.oddlabs.tt.model.DeployContainer getDeployContainer(com.oddlabs.tt.model.DeployType type)
        {
            return ship.getDeployContainer(type);
        }
        // Java: 188
        public override com.oddlabs.tt.model.ChieftainContainer getChieftainContainer()
        {
            return null;
        }
        // Java: 192
        public override com.oddlabs.tt.model.BuildSupplyContainer getBuildSupplyContainer(System.Type key)
        {
            return ship.getBuildSupplyContainer(key);
        }
        // Java: 196
        public override void deployUnits(com.oddlabs.tt.model.DeployType type, int num_units)
        {
            ship.deployUnits(type, num_units);
        }
        // Java: 201
        public override void createHarvesters(int num_tree, int num_rock, int num_iron, int num_rubber)
        {
            ship.createHarvesters(num_tree, num_rock, num_iron, num_rubber);
        }
        // Java: 205
        public override void createArmy(int num_peon, int num_rock, int num_iron, int num_rubber)
        {
            ship.createArmy(num_peon, num_rock, num_iron, num_rubber);
        }
        // Java: 209
        public override void createTransporters(int num_tree, int num_rock, int num_iron, int num_rubber)
        {
            ship.createTransporters(num_tree, num_rock, num_iron, num_rubber);
        }
        // Java: 213
        public override com.oddlabs.tt.model.Building getBase()
        {
            return ship;
        }
        // Java: 217
        public override void buildWeapons(System.Type type, int num_weapons, bool infinite)
        {
        }
        // Java: 220
        public override bool canBuildChieftain()
        {
            return false;
        }
        // Java: 224
        public override bool canStopChieftain()
        {
            return false;
        }
        // Java: 228
        public override void trainChieftain(bool start)
        {
        }
        // Java: 231
        public override void deployChieftain()
        {
        }
        // Java: 234
        public override bool canExitTower()
        {
            return false;
        }
        // Java: 238
        public override void exitTower()
        {
        }
        // Java: 241
        public override void fillSupplies(System.Type key, int max)
        {
            ship.fillSupplies(key, max);
        }
        // Java: 245
        public override void removeSupplies(System.Type key)
        {
            ship.removeSupplies(key);
        }
        // Java: 249
        public override int getStatusValue()
        {
            return ship.getStatusValue();
        }
        // Java: 253
        public override string toString()
        {
            return "ShipProxy at " + (getGridX()) + "," + (getGridY()) + " for " + java.lang.JStr.valueOf(ship);
        }
        // Java: 257
        public override void printDebugInfo()
        {
        }
        private void init_ShipProxy()
        {
            this.last_region = null;
        }
        public ShipProxy(int grid_x, int grid_y, com.oddlabs.tt.model.Ship ship) : base(ship.getOwner(), ship.getBuildingTemplate())
        {
            init_ShipProxy();
            this.ship = ship;
            setGridPosition(grid_x, grid_y);
            com.oddlabs.tt.pathfinder.UnitGrid unit_grid = getUnitGrid();
            float x = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(grid_x);
            float y = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(grid_y);
            base.setPosition(x, y);
            pushController(new com.oddlabs.tt.model.behaviour.NullController(this));
        }
    }
}
