// Translated from DeployContainer.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 3
    public class DeployContainer : com.oddlabs.tt.model.SupplyContainer
    {
        public com.oddlabs.tt.model.Building building;
        public com.oddlabs.tt.model.DeployType deploy_type;
        public System.Type supply_type;
        public float seconds_per_deploy;
        public float time;
        public int num_orders;
        // Java: 14
        public float getSecondsPerDeploy()
        {
            return seconds_per_deploy;
        }
        // Java: 18
        public System.Type getDeploySupplyType()
        {
            return supply_type;
        }
        // Java: 30
        public virtual void orderSupply(int orders)
        {
            int capped_amount = capAmount(orders);
            int result = -building.getUnitContainer().capAmount(-capped_amount);
            if (supply_type != null)
            {
                result = -building.getSupplyContainer(supply_type).capAmount(-result);
            }
            if (result > 0)
            {
                if (supply_type != null)
                {
                    building.getSupplyContainer(supply_type).prepareDeploy(result);
                }
                building.getUnitContainer().prepareDeploy(result);
                orderSupply(result, orders);
            }
            else
            {
                orderSupply(result, orders);
                building.getUnitContainer().prepareDeploy(result);
                if (supply_type != null)
                {
                    com.oddlabs.tt.model.SupplyContainer supply_container = building.getSupplyContainer(supply_type);
                    if (!supply_container.isSupplyFull())
                    {
                        supply_container.prepareDeploy(result);
                    }
                }
            }
        }
        // Java: 51
        public virtual void orderSupply(int amount, int orders)
        {
            increaseSupply(amount);
            num_orders += orders;
        }
        // Java: 56
        public virtual com.oddlabs.tt.model.Building getBuilding()
        {
            return building;
        }
        // Java: 60
        public virtual int getNumOrders()
        {
            return num_orders;
        }
        // Java: 64
        public virtual void deploy(float amount)
        {
            time += (float)(amount * drillMultiplier());
            if (time >= seconds_per_deploy)
            {
                time = 0;
                increaseSupply((-1));
                doDeploy();
            }
        }
        // Java: 74
        public float drillMultiplier()
        {
            if (!building.getOwner().getWorld().isExtended())
            {
                return 1.0f;
            }
            int level = building.getOwner().getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.DRILL);
            if (level == 0)
            {
                return 1.0f;
            }
            com.oddlabs.tt.model.DeployType d = deploy_type;
            bool warrior = ((object)d == (object)com.oddlabs.tt.model.DeployType.ROCK_WARRIOR) || ((object)d == (object)com.oddlabs.tt.model.DeployType.IRON_WARRIOR) || ((object)d == (object)com.oddlabs.tt.model.DeployType.RUBBER_WARRIOR) || ((object)d == (object)com.oddlabs.tt.model.DeployType.CHAMPION) || ((object)d == (object)com.oddlabs.tt.model.DeployType.SCOUT) || ((object)d == (object)com.oddlabs.tt.model.DeployType.SHAMAN) || ((object)d == (object)com.oddlabs.tt.model.DeployType.SLINGER) || ((object)d == (object)com.oddlabs.tt.model.DeployType.MELEE_LIGHT) || ((object)d == (object)com.oddlabs.tt.model.DeployType.MELEE_HEAVY) || ((object)d == (object)com.oddlabs.tt.model.DeployType.UNIQUE_1) || ((object)d == (object)com.oddlabs.tt.model.DeployType.UNIQUE_2) || ((object)d == (object)com.oddlabs.tt.model.DeployType.CRYSTAL_WARRIOR) || ((object)d == (object)com.oddlabs.tt.model.DeployType.DIAMOND_GUARD) || ((object)d == (object)com.oddlabs.tt.model.DeployType.UNIQUE_3) || ((object)d == (object)com.oddlabs.tt.model.DeployType.ELITE_1) || ((object)d == (object)com.oddlabs.tt.model.DeployType.ELITE_2) || ((object)d == (object)com.oddlabs.tt.model.DeployType.ELITE_3) || ((object)d == (object)com.oddlabs.tt.model.DeployType.SPINNER);
            return (warrior ? (float)(1.0f + (float)(0.25f * level)) : 1.0f);
        }
        // Java: 90
        public virtual void doDeploy()
        {
            switch ((deploy_type).ordinal())
            {
                case 0 /*ROCK_WARRIOR*/:
                {
                    building.createArmy(0, 1, 0, 0);
                    break;
                }
                case 1 /*IRON_WARRIOR*/:
                {
                    building.createArmy(0, 0, 1, 0);
                    break;
                }
                case 2 /*RUBBER_WARRIOR*/:
                {
                    building.createArmy(0, 0, 0, 1);
                    break;
                }
                case 3 /*PEON*/:
                {
                    building.createArmy(1, 0, 0, 0);
                    break;
                }
                case 4 /*PEON_HARVEST_TREE*/:
                {
                    building.createHarvesters(1, 0, 0, 0);
                    break;
                }
                case 5 /*PEON_TRANSPORT_TREE*/:
                {
                    building.createTransporters(1, 0, 0, 0);
                    break;
                }
                case 6 /*PEON_HARVEST_ROCK*/:
                {
                    building.createHarvesters(0, 1, 0, 0);
                    break;
                }
                case 7 /*PEON_TRANSPORT_ROCK*/:
                {
                    building.createTransporters(0, 1, 0, 0);
                    break;
                }
                case 8 /*PEON_HARVEST_IRON*/:
                {
                    building.createHarvesters(0, 0, 1, 0);
                    break;
                }
                case 9 /*PEON_TRANSPORT_IRON*/:
                {
                    building.createTransporters(0, 0, 1, 0);
                    break;
                }
                case 10 /*PEON_HARVEST_RUBBER*/:
                {
                    building.createHarvesters(0, 0, 0, 1);
                    break;
                }
                case 11 /*PEON_TRANSPORT_RUBBER*/:
                {
                    building.createTransporters(0, 0, 0, 1);
                    break;
                }
                case 12 /*CHAMPION*/:
                {
                    building.createUnits(com.oddlabs.tt.model.Race.UNIT_CHAMPION, 1);
                    break;
                }
                case 13 /*SCOUT*/:
                {
                    building.createUnits(com.oddlabs.tt.model.Race.UNIT_SCOUT, 1);
                    break;
                }
                case 18 /*SHAMAN*/:
                {
                    building.createUnits(com.oddlabs.tt.model.Race.UNIT_SHAMAN, 1);
                    break;
                }
                case 19 /*SLINGER*/:
                {
                    building.createUnits(com.oddlabs.tt.model.Race.UNIT_SLINGER, 1);
                    break;
                }
                case 26 /*MELEE_LIGHT*/:
                {
                    building.createUnits(com.oddlabs.tt.model.Race.UNIT_MELEE_LIGHT, 1);
                    break;
                }
                case 27 /*MELEE_HEAVY*/:
                {
                    building.createUnits(com.oddlabs.tt.model.Race.UNIT_MELEE_HEAVY, 1);
                    break;
                }
                case 35 /*UNIQUE_1*/:
                {
                    building.createUnits(com.oddlabs.tt.model.Race.UNIT_UNIQUE_1, 1);
                    break;
                }
                case 36 /*UNIQUE_2*/:
                {
                    building.createUnits(com.oddlabs.tt.model.Race.UNIT_UNIQUE_2, 1);
                    break;
                }
                case 51 /*CRYSTAL_WARRIOR*/:
                {
                    building.createUnits(com.oddlabs.tt.model.Race.UNIT_CRYSTAL, 1);
                    break;
                }
                case 52 /*DIAMOND_GUARD*/:
                {
                    building.createUnits(com.oddlabs.tt.model.Race.UNIT_DIAMOND, 1);
                    break;
                }
                case 53 /*UNIQUE_3*/:
                {
                    building.createUnits(com.oddlabs.tt.model.Race.UNIT_UNIQUE_3, 1);
                    break;
                }
                case 54 /*MINOR_GOD*/:
                {
                    building.createUnits(com.oddlabs.tt.model.Race.UNIT_MINOR_GOD, 1);
                    break;
                }
                case 55 /*MAJOR_GOD*/:
                {
                    building.createUnits(com.oddlabs.tt.model.Race.UNIT_MAJOR_GOD, 1);
                    break;
                }
                case 82 /*ELITE_1*/:
                {
                    building.createUnits(com.oddlabs.tt.model.Race.UNIT_ELITE_1, 1);
                    break;
                }
                case 83 /*ELITE_2*/:
                {
                    building.createUnits(com.oddlabs.tt.model.Race.UNIT_ELITE_2, 1);
                    break;
                }
                case 84 /*ELITE_3*/:
                {
                    building.createUnits(com.oddlabs.tt.model.Race.UNIT_ELITE_3, 1);
                    break;
                }
                case 93 /*SPINNER*/:
                {
                    building.createUnits(com.oddlabs.tt.model.Race.UNIT_SPINNER, 1);
                    break;
                }
                case 28 /*PEON_HARVEST_GOLD*/:
                case 29 /*PEON_HARVEST_CRYSTAL*/:
                case 30 /*PEON_HARVEST_HERBS*/:
                case 31 /*PEON_HARVEST_COAL*/:
                {
                    if ((building is com.oddlabs.tt.model.LandBuilding land))
                    {
                        land.createRareHarvesters(com.oddlabs.tt.model.ExtendedResources.RARE[deploy_type.ordinal() - com.oddlabs.tt.model.DeployType.PEON_HARVEST_GOLD.ordinal()], 1);
                    }
                    break;
                }
                case 81 /*PEON_HARVEST_DIAMOND*/:
                {
                    if ((building is com.oddlabs.tt.model.LandBuilding land_2))
                    {
                        land_2.createRareHarvesters(typeof(com.oddlabs.tt.model.DiamondSupply), 1);
                    }
                    break;
                }
                default:
                {
                    break;
                }
            }
        }
        // Java: 195
        public override int increaseSupply(int amount)
        {
            int result = building.getOwner().getUnitCountContainer().increaseSupply(amount);
            return base.increaseSupply(amount);
        }
        // Java: 202
        public virtual float getBuildProgress()
        {
            return (float)(time / seconds_per_deploy);
        }
        private void init_DeployContainer()
        {
            this.time = 0;
            this.num_orders = 0;
        }
        public DeployContainer(com.oddlabs.tt.model.Building building, float seconds_per_deploy, com.oddlabs.tt.model.DeployType deploy_type, System.Type supply_type) : base(java.lang.Integer.MAX_VALUE)
        {
            init_DeployContainer();
            this.building = building;
            this.seconds_per_deploy = seconds_per_deploy;
            this.deploy_type = deploy_type;
            this.supply_type = supply_type;
        }
    }
}
