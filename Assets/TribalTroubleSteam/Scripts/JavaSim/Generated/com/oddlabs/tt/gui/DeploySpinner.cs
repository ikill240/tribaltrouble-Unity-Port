// Translated from DeploySpinner.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 17
    public class DeploySpinner : com.oddlabs.tt.gui.IconSpinner
    {
        public com.oddlabs.tt.player.PlayerInterface player_interface;
        public com.oddlabs.tt.player.Player player;
        public System.Type gather_supply_type;
        public System.Type supply_type;
        public com.oddlabs.tt.model.DeployType deploy_type;
        public com.oddlabs.tt.model.Building current_building;
        public int num_orders;
        public int order_size;
        // Java: 38
        public void setContainers(com.oddlabs.tt.model.Building current_building, com.oddlabs.tt.model.DeployType deploy_type, System.Type supply_type)
        {
            this.current_building = current_building;
            this.deploy_type = deploy_type;
            this.supply_type = supply_type;
            if (!current_building.isDead())
            {
                num_orders = current_building.getDeployContainer(deploy_type).getNumOrders();
            }
        }
        // Java: 47
        public override int getDisplayCount()
        {
            int count = computeCount();
            if (player != null && gather_supply_type != null)
            {
                count += player.getGathererCount(gather_supply_type, current_building);
            }
            return count;
        }
        // Java: 56
        public override int computeCount()
        {
            if (current_building != null && !current_building.isDead())
            {
                com.oddlabs.tt.model.DeployContainer deploy_container = current_building.getDeployContainer(deploy_type);
                return java.lang.Math.min(deploy_container.getMaxSupplyCount(), java.lang.Math.max(0, deploy_container.getNumSupplies() + getOrderDiff()));
            }
            else
            {
                return 0;
            }
        }
        // Java: 66
        public override bool renderInfinite()
        {
            return false;
        }
        // Java: 71
        public int getOrderDiff()
        {
            if (current_building != null && !current_building.isDead())
            {
                return num_orders - current_building.getDeployContainer(deploy_type).getNumOrders();
            }
            else
            {
                return 0;
            }
        }
        // Java: 79
        public void order(int num)
        {
            if (!current_building.isDead())
            {
                player_interface.deployUnits(current_building, deploy_type, num);
            }
        }
        // Java: 84
        public override void increase(int amount)
        {
            if (current_building.isDead())
            {
                return;
            }
            if ((current_building is com.oddlabs.tt.model.Ship ship))
            {
                com.oddlabs.tt.model.ShipHR hr = ship.getShipHR();
                int num_units = default;
                switch ((deploy_type).ordinal())
                {
                    case 0 /*ROCK_WARRIOR*/:
                    case 1 /*IRON_WARRIOR*/:
                    case 2 /*RUBBER_WARRIOR*/:
                    {
                        num_units = hr.countUnitsOfType(supply_type);
                        break;
                    }
                    case 3 /*PEON*/:
                    case 4 /*PEON_HARVEST_TREE*/:
                    case 6 /*PEON_HARVEST_ROCK*/:
                    case 8 /*PEON_HARVEST_IRON*/:
                    case 10 /*PEON_HARVEST_RUBBER*/:
                    {
                        num_units = hr.countPeons();
                        break;
                    }
                    default:
                    {
                        num_units = java.lang.Math.min(hr.countPeons(), ship.getSupplyContainer(supply_type).getNumSupplies());
                        break;
                    }
                }
                num_units -= ship.getDeployContainer(deploy_type).getNumSupplies();
                if (order_size + amount > num_units)
                {
                    amount = java.lang.Math.max(0, num_units - order_size);
                }
                order_size += amount;
                num_orders += amount;
            }
            else
            {
                int num_units_2 = current_building.getUnitContainer().getNumSupplies();
                int num_supplies = java.lang.Integer.MAX_VALUE;
                if (supply_type != null)
                {
                    num_supplies = current_building.getSupplyContainer(supply_type).getNumSupplies();
                }
                if (num_units_2 > getOrderDiff() && num_supplies > getOrderDiff())
                {
                    if (amount > num_units_2 - getOrderDiff())
                    {
                        amount = num_units_2 - getOrderDiff();
                    }
                    if (supply_type != null && amount > num_supplies - getOrderDiff())
                    {
                        amount = num_supplies - getOrderDiff();
                    }
                    order_size += amount;
                    num_orders += amount;
                }
            }
        }
        // Java: 133
        public override void decrease(int amount)
        {
            if (current_building.isDead())
            {
                return;
            }
            if (computeCount() > 0)
            {
                int num_units = current_building.getDeployContainer(deploy_type).getNumSupplies();
                if (num_units > -getOrderDiff())
                {
                    if (amount > num_units + getOrderDiff())
                    {
                        amount = num_units + getOrderDiff();
                    }
                    order_size -= amount;
                    num_orders -= amount;
                }
            }
            else if (player != null && gather_supply_type != null && player.getGathererCount(gather_supply_type, current_building) > 0)
            {
                player_interface.recallGatherers(current_building, gather_supply_type, amount);
            }
        }
        // Java: 153
        public override void release()
        {
            order(order_size);
            order_size = 0;
        }
        // Java: 159
        public override int getOrderSize()
        {
            return order_size;
        }
        // Java: 164
        public override float getProgress()
        {
            if (!current_building.isDead())
            {
                return current_building.getDeployContainer(deploy_type).getBuildProgress();
            }
            else
            {
                return 0;
            }
        }
        private void init_DeploySpinner()
        {
            this.num_orders = 0;
            this.order_size = 0;
        }
        public DeploySpinner(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.player.PlayerInterface player_interface, com.oddlabs.tt.gui.ModeIconQuads icon_quad, string tool_tip, java.util.List tool_tip_icons, com.oddlabs.tt.input.GameAction action, com.oddlabs.tt.input.GameAction dec_action, com.oddlabs.tt.player.Player player, System.Type gather_supply_type) : base(viewer, icon_quad, tool_tip, tool_tip_icons, action, dec_action)
        {
            init_DeploySpinner();
            this.player_interface = player_interface;
            this.player = player;
            this.gather_supply_type = gather_supply_type;
        }
    }
}
