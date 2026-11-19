// Translated from DeploySpinner.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 18
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
        // Java: 39
        public void setContainers(com.oddlabs.tt.model.Building current_building, com.oddlabs.tt.model.DeployType deploy_type, System.Type supply_type)
        {
            this.current_building = current_building;
            this.deploy_type = deploy_type;
            this.supply_type = supply_type;
            if (!current_building.isDead() && current_building.getDeployContainer(deploy_type) != null)
            {
                num_orders = current_building.getDeployContainer(deploy_type).getNumOrders();
            }
        }
        // Java: 48
        public override int getDisplayCount()
        {
            int count = computeCount();
            if (player != null && gather_supply_type != null)
            {
                count += player.getGathererCount(gather_supply_type, current_building);
            }
            return count;
        }
        // Java: 57
        public override int computeCount()
        {
            if (current_building != null && !current_building.isDead())
            {
                com.oddlabs.tt.model.DeployContainer deploy_container = current_building.getDeployContainer(deploy_type);
                if (deploy_container == null)
                {
                    return 0;
                }
                return java.lang.Math.min(deploy_container.getMaxSupplyCount(), java.lang.Math.max(0, deploy_container.getNumSupplies() + getOrderDiff()));
            }
            else
            {
                return 0;
            }
        }
        // Java: 69
        public override bool renderInfinite()
        {
            return false;
        }
        // Java: 74
        public int getOrderDiff()
        {
            if (current_building != null && !current_building.isDead() && current_building.getDeployContainer(deploy_type) != null)
            {
                return num_orders - current_building.getDeployContainer(deploy_type).getNumOrders();
            }
            else
            {
                return 0;
            }
        }
        // Java: 82
        public void order(int num)
        {
            if (current_building != null && !current_building.isDead() && num != 0 && current_building.getDeployContainer(deploy_type) != null)
            {
                player_interface.deployUnits(current_building, deploy_type, num);
            }
        }
        // Java: 88
        public override void increase(int amount)
        {
            if (current_building == null || current_building.isDead())
            {
                return;
            }
            if (current_building.getDeployContainer(deploy_type) == null)
            {
                return;
            }
            if ((current_building is com.oddlabs.tt.model.Ship ship))
            {
                com.oddlabs.tt.model.ShipHR hr = ship.getShipHR();
                if (hr == null)
                {
                    return;
                }
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
                        com.oddlabs.tt.model.SupplyContainer held = ship.getSupplyContainer(supply_type);
                        num_units = (held == null ? 0 : java.lang.Math.min(hr.countPeons(), held.getNumSupplies()));
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
                    com.oddlabs.tt.model.SupplyContainer held_2 = current_building.getSupplyContainer(supply_type);
                    num_supplies = (held_2 == null ? 0 : held_2.getNumSupplies());
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
        // Java: 143
        public override void decrease(int amount)
        {
            if (current_building == null || current_building.isDead() || current_building.getDeployContainer(deploy_type) == null)
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
        // Java: 163
        public override void release()
        {
            order(order_size);
            order_size = 0;
        }
        // Java: 169
        public override int getOrderSize()
        {
            return order_size;
        }
        // Java: 174
        public override float getProgress()
        {
            if (current_building != null && !current_building.isDead() && current_building.getDeployContainer(deploy_type) != null)
            {
                return current_building.getDeployContainer(deploy_type).getBuildProgress();
            }
            else
            {
                return 0;
            }
        }
        // Java: 182
        public override bool isLocked()
        {
            return deploy_type != null && viewer.getLocalPlayer().isLocked(com.oddlabs.tt.model.Unlocks.keyFor(deploy_type));
        }
        // Java: 187
        public override bool hasToolTip()
        {
            return true;
        }
        // Java: 192
        public override void appendToolTip(com.oddlabs.tt.gui.ToolTipBox tool_tip_box)
        {
            base.appendToolTip(tool_tip_box);
            if (isLocked())
            {
                tool_tip_box.append(" - " + java.lang.JStr.valueOf(com.oddlabs.tt.gui.UnitInfoText.locked()));
            }
            int unit = com.oddlabs.tt.gui.UnitInfoText.unitFor(deploy_type);
            if (unit >= 0 && viewer.getWorld().isExtended())
            {
                string uses = usesLine();
                if (uses != null)
                {
                    tool_tip_box.append("\n" + java.lang.JStr.valueOf(uses));
                }
                tool_tip_box.append("\n" + java.lang.JStr.valueOf(com.oddlabs.tt.gui.UnitInfoText.forTraining(viewer.getLocalPlayer(), unit)));
            }
        }
        // Java: 207
        public string usesLine()
        {
            if (current_building == null || current_building.isDead())
            {
                return null;
            }
            com.oddlabs.tt.model.DeployContainer c = current_building.getDeployContainer(deploy_type);
            if (c == null || c.getDeploySupplyType() == null)
            {
                return null;
            }
            System.Type w = c.getDeploySupplyType();
            int per = ((c is com.oddlabs.tt.model.MultiWeaponDeployContainer m) ? m.getWeaponsPerUnit() : ((c is com.oddlabs.tt.model.RareDeployContainer r) ? r.getWeaponsPerUnit() : 1));
            string key = com.oddlabs.tt.gui.DeploySpinner.weaponKey(w, com.oddlabs.tt.model.ExtendedCombat.isVikings(current_building.getOwner()));
            if (key == null)
            {
                return null;
            }
            string what = com.oddlabs.tt.gui.ActionButtonPanel.i18n(key, per);
            if ((c is com.oddlabs.tt.model.RareDeployContainer rare))
            {
                string extra = com.oddlabs.tt.gui.TierButton.costText(rare.getCost());
                if (!extra.isEmpty())
                {
                    what += ", " + java.lang.JStr.valueOf(extra);
                }
            }
            int tenths = java.lang.Math.round((float)(c.getSecondsPerDeploy() * 10));
            string time = (tenths % 10 == 0 ? "" + ((tenths / 10)) : ((tenths / 10)) + "." + ((tenths % 10)));
            return com.oddlabs.tt.gui.ActionButtonPanel.i18n("train_uses", what, time);
        }
        // Java: 230
        public static string weaponKey(System.Type w, bool vikings)
        {
            if (((object)w == (object)typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)))
            {
                return "uses_rock";
            }
            if (((object)w == (object)typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)))
            {
                return "uses_iron";
            }
            if (((object)w == (object)typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)))
            {
                return "uses_chicken";
            }
            if (((object)w == (object)typeof(com.oddlabs.tt.model.weapon.CrystalWeapon)))
            {
                return "uses_crystal";
            }
            if (((object)w == (object)typeof(com.oddlabs.tt.model.weapon.DiamondWeapon)))
            {
                return "uses_diamond";
            }
            if (((object)w == (object)typeof(com.oddlabs.tt.model.weapon.DartWeapon)))
            {
                return (vikings ? "uses_firepots" : "uses_darts");
            }
            return null;
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
