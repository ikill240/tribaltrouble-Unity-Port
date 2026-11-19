// Translated from CraftSpinner.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 20
    public class CraftSpinner : com.oddlabs.tt.gui.IconSpinner
    {
        public com.oddlabs.tt.player.PlayerInterface player_interface;
        public com.oddlabs.tt.model.DeployType type;
        public com.oddlabs.tt.model.Building current_building;
        public int num_orders;
        public int order_size;
        // Java: 39
        public void setBuilding(com.oddlabs.tt.model.Building building)
        {
            current_building = building;
            com.oddlabs.tt.model.DeployContainer c = (building.isDead() ? (com.oddlabs.tt.model.DeployContainer)(null) : building.getDeployContainer(type));
            num_orders = (c != null ? c.getNumOrders() : 0);
        }
        // Java: 45
        public com.oddlabs.tt.model.CraftContainer container()
        {
            if (current_building == null || current_building.isDead())
            {
                return null;
            }
            return ((current_building.getDeployContainer(type) is com.oddlabs.tt.model.CraftContainer c) ? c : (com.oddlabs.tt.model.CraftContainer)(null));
        }
        // Java: 51
        public int orderDiff()
        {
            com.oddlabs.tt.model.CraftContainer c = container();
            return (c != null ? num_orders - c.getNumOrders() : 0);
        }
        // Java: 56
        public override int computeCount()
        {
            com.oddlabs.tt.model.CraftContainer c = container();
            return (c != null ? java.lang.Math.max(0, c.getNumSupplies() + orderDiff()) : 0);
        }
        // Java: 62
        public override void increase(int amount)
        {
            com.oddlabs.tt.model.CraftContainer c = container();
            if (c == null || !c.isAvailable())
            {
                return;
            }
            int room = c.affordable() - java.lang.Math.max(0, order_size);
            amount = java.lang.Math.min(amount, room);
            if (amount <= 0)
            {
                return;
            }
            order_size += amount;
            num_orders += amount;
        }
        // Java: 75
        public override void decrease(int amount)
        {
            com.oddlabs.tt.model.CraftContainer c = container();
            if (c == null)
            {
                return;
            }
            amount = java.lang.Math.min(amount, computeCount());
            if (amount <= 0)
            {
                return;
            }
            order_size -= amount;
            num_orders -= amount;
        }
        // Java: 87
        public override void release()
        {
            if (order_size != 0 && current_building != null && !current_building.isDead())
            {
                player_interface.deployUnits(current_building, type, order_size);
            }
            order_size = 0;
        }
        // Java: 94
        public override int getOrderSize()
        {
            return order_size;
        }
        // Java: 99
        public override bool renderInfinite()
        {
            return false;
        }
        // Java: 104
        public override float getProgress()
        {
            com.oddlabs.tt.model.CraftContainer c = container();
            return (c != null ? c.getBuildProgress() : 0.0f);
        }
        public CraftSpinner(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.player.PlayerInterface player_interface, com.oddlabs.tt.gui.ModeIconQuads icon_quad, string tool_tip, java.util.List tool_tip_icons, com.oddlabs.tt.input.GameAction action, com.oddlabs.tt.input.GameAction dec_action, com.oddlabs.tt.model.DeployType type) : base(viewer, icon_quad, tool_tip, tool_tip_icons, action, dec_action)
        {
            this.player_interface = player_interface;
            this.type = type;
            setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                com.oddlabs.tt.model.CraftContainer c = container();
                return c == null || !c.isAvailable() || c.affordable() <= 0;
            }));
        }
    }
}
