// Translated from TierButton.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 19
    public class TierButton : com.oddlabs.tt.gui.NonFocusIconButton
    {
        public static string[] RESOURCES;
        public static string[] NAMES;
        public com.oddlabs.tt.player.PlayerInterface player_interface;
        public com.oddlabs.tt.model.DeployType type;
        public int kind;
        public com.oddlabs.tt.model.Building building;
        // Java: 39
        public void setBuilding(com.oddlabs.tt.model.Building building)
        {
            this.building = building;
        }
        // Java: 43
        public com.oddlabs.tt.model.TierContainer container()
        {
            if (building == null || building.isDead())
            {
                return null;
            }
            com.oddlabs.tt.model.DeployContainer c = building.getDeployContainer(type);
            return ((c is com.oddlabs.tt.model.TierContainer tc) ? tc : (com.oddlabs.tt.model.TierContainer)(null));
        }
        // Java: 50
        public override bool isHidden()
        {
            com.oddlabs.tt.model.TierContainer tc = container();
            return tc != null && (tc.getBuilding().getOwner().isLocked(com.oddlabs.tt.model.Unlocks.TIERS) || kind == com.oddlabs.tt.model.TierContainer.RUNE && tc.getBuilding().getOwner().isLocked(com.oddlabs.tt.model.Unlocks.RUNE_TIER));
        }
        // Java: 57
        public bool canClick()
        {
            com.oddlabs.tt.model.TierContainer tc = container();
            if (tc != null && tc.getBuilding().getOwner().isLocked(com.oddlabs.tt.model.Unlocks.TIERS))
            {
                return false;
            }
            if (tc != null && kind == com.oddlabs.tt.model.TierContainer.RUNE && tc.getBuilding().getOwner().isLocked(com.oddlabs.tt.model.Unlocks.RUNE_TIER))
            {
                return false;
            }
            return tc != null && (tc.isWorking() || tc.canOrder());
        }
        // Java: 66
        public override void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
        {
            com.oddlabs.tt.model.TierContainer tc = container();
            if (tc != null && building != null && canClick())
            {
                player_interface.deployUnits(building, type, (tc.isWorking() ? (-1) : 1));
            }
        }
        // Java: 73
        public override bool hasToolTip()
        {
            return true;
        }
        // Java: 78
        public override void appendToolTip(com.oddlabs.tt.gui.ToolTipBox tool_tip_box)
        {
            tool_tip_box.append(toolTip());
        }
        // Java: 83
        public static string costText(int[] cost)
        {
            java.util.List parts = new java.util.ArrayList();
            for (int i = 0; i < cost.Length && i < com.oddlabs.tt.gui.TierButton.RESOURCES.Length; i++)
            {
                if (cost[i] > 0)
                {
                    parts.add(com.oddlabs.tt.gui.ActionButtonPanel.i18n(com.oddlabs.tt.gui.TierButton.RESOURCES[i], cost[i]));
                }
            }
            return java.lang.JString.join(", ", parts);
        }
        // Java: 91
        public string toolTip()
        {
            string name = com.oddlabs.tt.gui.ActionButtonPanel.i18n(com.oddlabs.tt.gui.TierButton.NAMES[kind]);
            com.oddlabs.tt.model.TierContainer tc = container();
            if (tc == null)
            {
                return name;
            }
            if (tc.isWorking())
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("tier_busy", name, java.lang.Math.round((float)(tc.getBuildProgress() * 100)));
            }
            if (tc.isDone())
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("tier_done", name);
            }
            if (tc.isLocked())
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n((kind == com.oddlabs.tt.model.TierContainer.RUNE ? "tier_locked_rune" : "tier_locked"), name);
            }
            return com.oddlabs.tt.gui.ActionButtonPanel.i18n("tier_next", name, com.oddlabs.tt.gui.TierButton.costText(com.oddlabs.tt.model.TierContainer.getCost(kind)), java.lang.Math.round(com.oddlabs.tt.model.TierContainer.getSeconds(kind)));
        }
        // Java: 106
        public override void postRender(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            com.oddlabs.tt.model.TierContainer tc = container();
            if (tc != null && tc.isWorking())
            {
                com.oddlabs.tt.gui.IconQuad watchQuad = com.oddlabs.tt.gui.GUIIcons.getIcons().getWatch(tc.getBuildProgress());
                renderer.drawIcon(watchQuad, getWidth() - watchQuad.getWidth(), getHeight() - watchQuad.getHeight());
            }
        }
        static TierButton()
        {
            TierButton.RESOURCES = new string[] { "research_wood", "research_rock", "research_iron", "research_chicken", "research_gold", "research_crystal", "research_herbs", "research_coal", "research_diamond" };
            TierButton.NAMES = new string[] { "upgrade_stone_tip", "upgrade_iron_tip", "upgrade_chicken_tip", "upgrade_rune_tip" };
        }
        public TierButton(com.oddlabs.tt.player.PlayerInterface player_interface, com.oddlabs.tt.gui.ModeIconQuads icon, com.oddlabs.tt.model.DeployType type, int kind) : base(icon, null, new java.util.function.Supplier_L(() =>
        {
            return "";
        }))
        {
            this.player_interface = player_interface;
            this.type = type;
            this.kind = kind;
            setCanFocus(true);
            setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return !canClick();
            }));
        }
    }
}
