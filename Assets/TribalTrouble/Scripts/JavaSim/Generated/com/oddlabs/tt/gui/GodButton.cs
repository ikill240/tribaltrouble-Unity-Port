// Translated from GodButton.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 16
    public class GodButton : com.oddlabs.tt.gui.NonFocusIconButton
    {
        public com.oddlabs.tt.player.PlayerInterface player_interface;
        public com.oddlabs.tt.model.DeployType type;
        public string tip_key;
        public com.oddlabs.tt.model.Building building;
        // Java: 32
        public void setBuilding(com.oddlabs.tt.model.Building building)
        {
            this.building = building;
        }
        // Java: 36
        public com.oddlabs.tt.model.GodContainer container()
        {
            if (building == null || building.isDead())
            {
                return null;
            }
            return ((building.getDeployContainer(type) is com.oddlabs.tt.model.GodContainer g) ? g : (com.oddlabs.tt.model.GodContainer)(null));
        }
        // Java: 42
        public override bool isHidden()
        {
            com.oddlabs.tt.model.GodContainer g = container();
            return g != null && g.isLocked();
        }
        // Java: 48
        public bool canClick()
        {
            com.oddlabs.tt.model.GodContainer g = container();
            return g != null && (g.isWorking() || g.canOrder());
        }
        // Java: 53
        public override void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
        {
            com.oddlabs.tt.model.GodContainer g = container();
            if (g != null && building != null && canClick())
            {
                player_interface.deployUnits(building, type, (g.isWorking() ? (-1) : 1));
            }
        }
        // Java: 60
        public override bool hasToolTip()
        {
            return true;
        }
        // Java: 65
        public override void appendToolTip(com.oddlabs.tt.gui.ToolTipBox tool_tip_box)
        {
            tool_tip_box.append(toolTip());
        }
        // Java: 70
        public string toolTip()
        {
            string name = com.oddlabs.tt.gui.ActionButtonPanel.i18n(tip_key);
            com.oddlabs.tt.model.GodContainer g = container();
            if (g == null)
            {
                return name;
            }
            string what = com.oddlabs.tt.gui.ActionButtonPanel.i18n("god_cost", g.getWeapons(), com.oddlabs.tt.gui.TierButton.costText(g.getCost()), java.lang.Math.round(g.getSeconds()));
            if (g.isWorking())
            {
                return java.lang.JStr.valueOf(name) + "\n" + java.lang.JStr.valueOf(com.oddlabs.tt.gui.ActionButtonPanel.i18n("god_busy", java.lang.Math.round((float)(g.getBuildProgress() * 100))));
            }
            if (g.isTaken())
            {
                return java.lang.JStr.valueOf(name) + "\n" + java.lang.JStr.valueOf(com.oddlabs.tt.gui.ActionButtonPanel.i18n("god_taken"));
            }
            if (g.findPayer() == null)
            {
                return java.lang.JStr.valueOf(name) + "\n" + java.lang.JStr.valueOf(what) + "\n" + java.lang.JStr.valueOf(com.oddlabs.tt.gui.ActionButtonPanel.i18n("god_need"));
            }
            return java.lang.JStr.valueOf(name) + "\n" + java.lang.JStr.valueOf(what);
        }
        // Java: 85
        public override void postRender(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            com.oddlabs.tt.model.GodContainer g = container();
            if (g != null && g.isWorking())
            {
                com.oddlabs.tt.gui.IconQuad watchQuad = com.oddlabs.tt.gui.GUIIcons.getIcons().getWatch(g.getBuildProgress());
                renderer.drawIcon(watchQuad, getWidth() - watchQuad.getWidth(), getHeight() - watchQuad.getHeight());
            }
        }
        public GodButton(com.oddlabs.tt.player.PlayerInterface player_interface, com.oddlabs.tt.gui.ModeIconQuads icon, com.oddlabs.tt.model.DeployType type, string tip_key, com.oddlabs.tt.input.GameAction action) : base(icon, action, new java.util.function.Supplier_L(() =>
        {
            return "";
        }))
        {
            this.player_interface = player_interface;
            this.type = type;
            this.tip_key = tip_key;
            setCanFocus(true);
            setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return !canClick();
            }));
        }
    }
}
