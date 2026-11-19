// Translated from GodContainer.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 16
    public class GodContainer : com.oddlabs.tt.model.DeployContainer
    {
        public int unit;
        public int weapons;
        public int[] cost;
        public com.oddlabs.tt.model.Building paid_at;
        public bool working;
        // Java: 30
        public int getUnit()
        {
            return unit;
        }
        // Java: 34
        public int getWeapons()
        {
            return weapons;
        }
        // Java: 38
        public int[] getCost()
        {
            return cost;
        }
        // Java: 42
        public float getSeconds()
        {
            return seconds_per_deploy;
        }
        // Java: 46
        public bool isWorking()
        {
            return working;
        }
        // Java: 51
        public bool isTaken()
        {
            com.oddlabs.tt.player.Player owner = building.getOwner();
            com.oddlabs.tt.model.UnitTemplate t = owner.getRace().getUnitTemplate(unit);
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(owner.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Unit u) && !u.isDead() && ((object)((com.oddlabs.tt.model.UnitTemplate)u.getTemplate()) == (object)t))
                {
                    return true;
                }
                if (((object)s != (object)building) && (s is com.oddlabs.tt.model.Building b) && !b.isDead() && (b.getDeployContainer(deploy_type) is com.oddlabs.tt.model.GodContainer g) && g.isWorking())
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 64
        public bool isLocked()
        {
            return building.getOwner().isLocked(com.oddlabs.tt.model.Unlocks.unit(unit));
        }
        // Java: 69
        public com.oddlabs.tt.model.Building findPayer()
        {
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(building.getOwner().getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Building b) && !b.isDead() && b.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD_ARMIES) && b.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.DiamondWeapon)) != null && b.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.DiamondWeapon)).getNumSupplies() >= weapons && com.oddlabs.tt.model.ExtendedResources.hasStores(b, cost))
                {
                    return b;
                }
            }
            return null;
        }
        // Java: 79
        public bool canOrder()
        {
            return !working && building.isComplete() && !isLocked() && !isTaken() && findPayer() != null;
        }
        // Java: 83
        public override void orderSupply(int orders)
        {
            if (orders > 0 && canOrder())
            {
                paid_at = findPayer();
                if (paid_at == null)
                {
                    return;
                }
                com.oddlabs.tt.model.ExtendedResources.pay(paid_at, cost, 1);
                paid_at.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.DiamondWeapon)).increaseSupply(-weapons);
                working = true;
                time = 0;
                supply_count = 1;
            }
            else if (orders < 0 && working)
            {
                if (paid_at != null && !paid_at.isDead())
                {
                    com.oddlabs.tt.model.ExtendedResources.pay(paid_at, cost, (-1));
                    com.oddlabs.tt.model.SupplyContainer w = paid_at.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.DiamondWeapon));
                    w.increaseSupply(java.lang.Math.min(weapons, w.getMaxSupplyCount() - w.getNumSupplies()));
                }
                stop();
            }
        }
        // Java: 104
        public void stop()
        {
            working = false;
            time = 0;
            supply_count = 0;
            paid_at = null;
        }
        // Java: 111
        public override int increaseSupply(int amount)
        {
            supply_count += amount;
            return amount;
        }
        // Java: 117
        public override void deploy(float amount)
        {
            if (!working)
            {
                return;
            }
            time += amount;
            if (time >= seconds_per_deploy)
            {
                stop();
                spawn();
            }
        }
        // Java: 128
        public void spawn()
        {
            com.oddlabs.tt.player.Player owner = building.getOwner();
            com.oddlabs.tt.pathfinder.UnitGrid grid = owner.getWorld().getUnitGrid();
            int gx = building.getGridX();
            int gy = building.getGridY();
            int reach = ((building is com.oddlabs.tt.model.LandBuilding lb) ? (((com.oddlabs.tt.model.BuildingTemplate)((com.oddlabs.tt.model.BuildingTemplate)lb.getTemplate()))).getPlacingSize() + 2 : 4);
            com.oddlabs.tt.util.Target[] free = grid.findGridTargets(gx, gy + reach, 1, true);
            if (free.Length == 0 || free[0] == null)
            {
                free = grid.findGridTargets(gx, gy, 1, true);
            }
            if (free.Length == 0 || free[0] == null)
            {
                return;
            }
            com.oddlabs.tt.util.Target rally = (building.hasRallyPoint() ? building.getRallyPoint() : (com.oddlabs.tt.util.Target)(null));
            new com.oddlabs.tt.model.Unit(owner, com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(free[0].getGridX()), com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(free[0].getGridY()), rally, owner.getRace().getUnitTemplate(unit));
        }
        // Java: 143
        public override float getBuildProgress()
        {
            return (working ? (float)(time / seconds_per_deploy) : 0.0f);
        }
        public GodContainer(com.oddlabs.tt.model.Building hall, com.oddlabs.tt.model.DeployType type, int unit, int weapons, int[] cost, float seconds) : base(hall, seconds, type, null)
        {
            this.unit = unit;
            this.weapons = weapons;
            this.cost = cost;
        }
    }
}
