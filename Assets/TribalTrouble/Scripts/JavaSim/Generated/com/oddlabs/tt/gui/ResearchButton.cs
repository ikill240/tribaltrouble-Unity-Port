// Translated from ResearchButton.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 20
    public class ResearchButton : com.oddlabs.tt.gui.NonFocusIconButton
    {
        public static string[] RESOURCES;
        public com.oddlabs.tt.player.PlayerInterface player_interface;
        public com.oddlabs.tt.player.Player player;
        public com.oddlabs.tt.model.DeployType type;
        public int upgrade;
        public com.oddlabs.tt.gui.Label level_label;
        public com.oddlabs.tt.model.Building building;
        public int shown_level;
        // Java: 45
        public void setBuilding(com.oddlabs.tt.model.Building building)
        {
            this.building = building;
        }
        // Java: 49
        public com.oddlabs.tt.model.ResearchContainer container()
        {
            if (building == null || building.isDead())
            {
                return null;
            }
            com.oddlabs.tt.model.DeployContainer c = building.getDeployContainer(type);
            return ((c is com.oddlabs.tt.model.ResearchContainer rc) ? rc : (com.oddlabs.tt.model.ResearchContainer)(null));
        }
        // Java: 56
        public override bool isHidden()
        {
            if (upgrade == com.oddlabs.tt.model.ResearchContainer.GREAT_TRIBE && !player.isCampaignTribe())
            {
                return true;
            }
            return player.isLocked(com.oddlabs.tt.model.Unlocks.upgrade(upgrade));
        }
        // Java: 63
        public bool canClick()
        {
            com.oddlabs.tt.model.ResearchContainer rc = container();
            if (rc == null || player.isLocked(com.oddlabs.tt.model.Unlocks.upgrade(upgrade)))
            {
                return false;
            }
            int level = player.getUpgradeLevel(upgrade);
            return rc.isResearching() || (level < player.getResearchCap(upgrade) && !player.isResearching(upgrade) && rc.canAfford());
        }
        // Java: 72
        public override void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
        {
            com.oddlabs.tt.model.ResearchContainer rc = container();
            if (rc != null && building != null && canClick())
            {
                player_interface.deployUnits(building, type, (rc.isResearching() ? (-1) : 1));
            }
        }
        // Java: 79
        public override bool hasToolTip()
        {
            return true;
        }
        // Java: 84
        public override void appendToolTip(com.oddlabs.tt.gui.ToolTipBox tool_tip_box)
        {
            tool_tip_box.append(toolTip());
        }
        // Java: 89
        public string toolTip()
        {
            if (player.isLocked(com.oddlabs.tt.model.Unlocks.upgrade(upgrade)))
            {
                return java.lang.JStr.valueOf(com.oddlabs.tt.gui.ActionButtonPanel.i18n("research_" + (upgrade) + java.lang.JStr.valueOf(((upgrade == com.oddlabs.tt.model.ResearchContainer.RACE_1 || upgrade == com.oddlabs.tt.model.ResearchContainer.RACE_2 ? ((com.oddlabs.tt.model.ExtendedCombat.isVikings(player) ? "_viking" : "_native")) : ""))) + "_tip")) + " - " + java.lang.JStr.valueOf(com.oddlabs.tt.gui.UnitInfoText.locked());
            }
            bool vikings = com.oddlabs.tt.model.ExtendedCombat.isVikings(player);
            string name = com.oddlabs.tt.gui.ActionButtonPanel.i18n("research_" + (upgrade) + java.lang.JStr.valueOf(((upgrade == com.oddlabs.tt.model.ResearchContainer.RACE_1 || upgrade == com.oddlabs.tt.model.ResearchContainer.RACE_2 ? ((vikings ? "_viking" : "_native")) : ""))) + "_tip");
            int level = player.getUpgradeLevel(upgrade);
            com.oddlabs.tt.model.ResearchContainer rc = container();
            if (rc != null && rc.isResearching())
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("research_busy", name, java.lang.Math.round((float)(rc.getBuildProgress() * 100)));
            }
            if (level >= com.oddlabs.tt.model.ResearchContainer.getMaxLevel(upgrade))
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("research_max", name, level);
            }
            if (level >= player.getResearchCap(upgrade))
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("research_home_cap", name, level);
            }
            int[] cost = com.oddlabs.tt.model.ResearchContainer.getCost(upgrade, level, player);
            java.util.List parts = new java.util.ArrayList();
            for (int i = 0; i < cost.Length && i < com.oddlabs.tt.gui.ResearchButton.RESOURCES.Length; i++)
            {
                if (cost[i] > 0)
                {
                    parts.add(com.oddlabs.tt.gui.ActionButtonPanel.i18n(com.oddlabs.tt.gui.ResearchButton.RESOURCES[i], cost[i]));
                }
            }
            return com.oddlabs.tt.gui.ActionButtonPanel.i18n("research_next", name, level, com.oddlabs.tt.model.ResearchContainer.getMaxLevel(upgrade), java.lang.JString.join(", ", parts), java.lang.Math.round(com.oddlabs.tt.model.ResearchContainer.getSeconds(level)));
        }
        // Java: 114
        public override void postRender(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            int level = player.getUpgradeLevel(upgrade);
            if (level != shown_level)
            {
                shown_level = level;
                level_label.clear();
                if (level > 0)
                {
                    level_label.append(java.lang.Integer.toString(level));
                }
            }
            com.oddlabs.tt.model.ResearchContainer rc = container();
            if (rc != null && rc.isResearching())
            {
                com.oddlabs.tt.gui.IconQuad watchQuad = com.oddlabs.tt.gui.GUIIcons.getIcons().getWatch(rc.getBuildProgress());
                renderer.drawIcon(watchQuad, getWidth() - watchQuad.getWidth(), getHeight() - watchQuad.getHeight());
            }
        }
        static ResearchButton()
        {
            ResearchButton.RESOURCES = com.oddlabs.tt.gui.TierButton.RESOURCES;
        }
        public ResearchButton(com.oddlabs.tt.player.PlayerInterface player_interface, com.oddlabs.tt.player.Player player, com.oddlabs.tt.gui.ModeIconQuads icon, com.oddlabs.tt.model.DeployType type, int upgrade) : base(icon, null, new java.util.function.Supplier_L(() =>
        {
            return "";
        }))
        {
            this.player_interface = player_interface;
            this.player = player;
            this.type = type;
            this.upgrade = upgrade;
            setCanFocus(true);
            level_label = new com.oddlabs.tt.gui.Label("", com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), getWidth() - 4, com.oddlabs.tt.gui.Origin.AT_END);
            level_label.setPos(0, 2);
            addChild(level_label);
            setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return !canClick();
            }));
        }
    }
}
