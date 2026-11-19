// Translated from BuildSpinner.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 17
    public class BuildSpinner : com.oddlabs.tt.gui.IconSpinner
    {
        public const int INFINITE_LIMIT = 30;
        public com.oddlabs.tt.player.PlayerInterface player_interface;
        public com.oddlabs.tt.model.Building current_building;
        public System.Type type;
        public int num_orders;
        public int order_size;
        public bool infinite;
        // Java: 35
        public void setBuildSupplyContainer(com.oddlabs.tt.model.Building current_building, System.Type type)
        {
            this.current_building = current_building;
            this.type = type;
            if (!current_building.isDead() && current_building.getBuildSupplyContainer(type) != null)
            {
                num_orders = current_building.getBuildSupplyContainer(type).getNumOrders();
            }
        }
        // Java: 42
        public override int computeCount()
        {
            if (!current_building.isDead())
            {
                com.oddlabs.tt.model.BuildSupplyContainer build_container = current_building.getBuildSupplyContainer(type);
                int count = java.lang.Math.min(build_container.getMaxSupplyCount(), java.lang.Math.max(0, build_container.getNumSupplies() + getOrderDiff()));
                infinite = count >= com.oddlabs.tt.gui.BuildSpinner.INFINITE_LIMIT;
                return count;
            }
            else
            {
                return 0;
            }
        }
        // Java: 54
        public override bool renderInfinite()
        {
            return infinite;
        }
        // Java: 59
        public void order(int num)
        {
            if (!current_building.isDead())
            {
                if (((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)))
                {
                    player_interface.buildRockWeapons(current_building, num, infinite);
                }
                else if (((object)type == (object)typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)))
                {
                    player_interface.buildIronWeapons(current_building, num, infinite);
                }
                else if (((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)))
                {
                    player_interface.buildRubberWeapons(current_building, num, infinite);
                }
                else
                {
                    throw new System.ArgumentException();
                }
            }
        }
        // Java: 73
        public override void appendToolTip(com.oddlabs.tt.gui.ToolTipBox tool_tip_box)
        {
            if (!isDisabled())
            {
                base.appendToolTip(tool_tip_box);
            }
        }
        // Java: 79
        public override float getProgress()
        {
            return (current_building.isDead() ? 0 : (((com.oddlabs.tt.model.BuildProductionContainer)current_building.getBuildSupplyContainer(type))).getBuildProgress());
        }
        // Java: 85
        public com.oddlabs.tt.model.Building getBuilding()
        {
            return current_building;
        }
        // Java: 89
        public int getOrderDiff()
        {
            return (current_building.isDead() ? 0 : num_orders - current_building.getBuildSupplyContainer(type).getNumOrders());
        }
        // Java: 94
        public override void increase(int amount)
        {
            order_size += amount;
            num_orders += amount;
        }
        // Java: 100
        public override void decrease(int amount)
        {
            order_size -= amount;
            num_orders -= amount;
        }
        // Java: 106
        public override void release()
        {
            order(order_size);
            order_size = 0;
        }
        // Java: 112
        public override int getOrderSize()
        {
            return order_size;
        }
        public BuildSpinner(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.player.PlayerInterface player_interface, com.oddlabs.tt.gui.ModeIconQuads icon_quad, string tool_tip, java.util.List tool_tip_icons, com.oddlabs.tt.input.GameAction action, com.oddlabs.tt.input.GameAction dec_action) : base(viewer, icon_quad, tool_tip, tool_tip_icons, action, dec_action)
        {
            this.player_interface = player_interface;
        }
    }
}
