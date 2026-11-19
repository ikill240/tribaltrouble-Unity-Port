// Translated from MarketSpinner.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 19
    public class MarketSpinner : com.oddlabs.tt.gui.IconSpinner
    {
        public com.oddlabs.tt.player.PlayerInterface player_interface;
        public com.oddlabs.tt.model.DeployType type;
        public string base_tip;
        public com.oddlabs.tt.model.Building market;
        public int order_size;
        // Java: 35
        public void setBuilding(com.oddlabs.tt.model.Building building)
        {
            market = building;
        }
        // Java: 39
        public com.oddlabs.tt.model.MarketContainer container()
        {
            return (market != null && !market.isDead() && (market.getDeployContainer(type) is com.oddlabs.tt.model.MarketContainer m) ? m : (com.oddlabs.tt.model.MarketContainer)(null));
        }
        // Java: 43
        public override int computeCount()
        {
            com.oddlabs.tt.model.MarketContainer m = container();
            if (m == null || m.isGift())
            {
                return 0;
            }
            return com.oddlabs.tt.model.MarketContainer.buyPrice(market.getOwner().getWorld(), m.getGood());
        }
        // Java: 51
        public override void increase(int amount)
        {
            order_size += java.lang.Math.min(amount, 10);
        }
        // Java: 56
        public override void decrease(int amount)
        {
            com.oddlabs.tt.model.MarketContainer m = container();
            if (m != null && !m.isGift())
            {
                order_size -= java.lang.Math.min(amount, 10);
            }
        }
        // Java: 63
        public override void release()
        {
            if (order_size != 0 && market != null && !market.isDead())
            {
                player_interface.deployUnits(market, type, order_size);
            }
            order_size = 0;
        }
        // Java: 70
        public override int getOrderSize()
        {
            return 0;
        }
        // Java: 75
        public override bool renderInfinite()
        {
            return false;
        }
        // Java: 80
        public override float getProgress()
        {
            return 0.0f;
        }
        // Java: 85
        public override void appendToolTip(com.oddlabs.tt.gui.ToolTipBox tool_tip_box)
        {
            com.oddlabs.tt.model.MarketContainer m = container();
            if (m != null && !m.isGift())
            {
                com.oddlabs.tt.landscape.World world = market.getOwner().getWorld();
                tool_tip_box.append(com.oddlabs.tt.gui.ActionButtonPanel.i18n("market_price", base_tip, com.oddlabs.tt.model.MarketContainer.LOT, com.oddlabs.tt.model.MarketContainer.buyPrice(world, m.getGood()), com.oddlabs.tt.model.MarketContainer.sellPrice(world, m.getGood())));
                return;
            }
            base.appendToolTip(tool_tip_box);
        }
        public MarketSpinner(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.player.PlayerInterface player_interface, com.oddlabs.tt.gui.ModeIconQuads icon_quad, string tool_tip, java.util.List tool_tip_icons, com.oddlabs.tt.input.GameAction action, com.oddlabs.tt.input.GameAction dec_action, com.oddlabs.tt.model.DeployType type) : base(viewer, icon_quad, tool_tip, tool_tip_icons, action, dec_action)
        {
            this.player_interface = player_interface;
            this.type = type;
            this.base_tip = tool_tip;
        }
    }
}
