// Translated from ToolTipAdapter.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.render
{
    // Java: 24
    public class ToolTipAdapter : java.lang.Object, com.oddlabs.tt.gui.ToolTip
    {
        public com.oddlabs.tt.model.ModelToolTip model;
        public com.oddlabs.tt.player.Player local_player;
        // Java: 33
        public void visitPlayer(com.oddlabs.tt.gui.ToolTipBox tool_tip, com.oddlabs.tt.player.Player player)
        {
            tool_tip.append(player.getPlayerInfo().getName());
            tool_tip.append(" - ");
        }
        // Java: 45
        public void visitSelectable(com.oddlabs.tt.gui.ToolTipBox tool_tip, com.oddlabs.tt.model.Selectable selectable)
        {
            visitPlayer(tool_tip, selectable.getOwner());
        }
        // Java: 56
        public void appendToolTip(com.oddlabs.tt.gui.ToolTipBox tool_tip)
        {
            switch ((model))
            {
                case com.oddlabs.tt.model.Unit unit:
                {
                    visitUnit(tool_tip, unit);
                    break;
                }
                case com.oddlabs.tt.model.Building building:
                {
                    visitBuilding(tool_tip, building);
                    break;
                }
                case com.oddlabs.tt.model.Supply supply:
                {
                    visitSupply(tool_tip, supply);
                    break;
                }
                case com.oddlabs.tt.model.SceneryModel scenery:
                {
                    visitSceneryModel(tool_tip, scenery);
                    break;
                }
                default:
                {
                    break;
                }
            }
        }
        // Java: 68
        public void visitSceneryModel(com.oddlabs.tt.gui.ToolTipBox tool_tip, com.oddlabs.tt.model.SceneryModel model)
        {
            string name = model.getName();
            if (name != null)
            {
                tool_tip.append(name);
            }
        }
        // Java: 74
        public void visitSupply(com.oddlabs.tt.gui.ToolTipBox tool_tip, com.oddlabs.tt.model.Supply supply)
        {
            tool_tip.append(supply.getName());
            tool_tip.append(com.oddlabs.tt.gui.GUIIcons.getIcons().getToolTipIcon(supply.GetType()));
        }
        // Java: 79
        public void visitBuilding(com.oddlabs.tt.gui.ToolTipBox tool_tip, com.oddlabs.tt.model.Building building)
        {
            visitSelectable(tool_tip, building);
            tool_tip.append(((com.oddlabs.tt.model.BuildingTemplate)building.getTemplate()).getName());
            float health = (float)(((float)(building.getHitPoints())) / ((com.oddlabs.tt.model.BuildingTemplate)building.getTemplate()).getMaxHitPoints());
            java.util.List watch = java.util.List_.of(com.oddlabs.tt.gui.GUIIcons.getIcons().getWatch(health));
            tool_tip.append(watch);
        }
        // Java: 92
        public void visitUnit(com.oddlabs.tt.gui.ToolTipBox tool_tip, com.oddlabs.tt.model.Unit unit)
        {
            visitSelectable(tool_tip, unit);
            string name = unit.getName();
            tool_tip.append(((string)java.util.Objects.requireNonNullElseGet(name, new java.util.function.Supplier_L(() =>
            {
                return ((com.oddlabs.tt.model.UnitTemplate)unit.getTemplate()).getName();
            }))));
            com.oddlabs.tt.model.behaviour.Controller c = unit.getPrimaryController();
            if (unit.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC))
            {
                float health = (float)(((float)(unit.getHitPoints())) / ((com.oddlabs.tt.model.UnitTemplate)unit.getTemplate()).getMaxHitPoints());
                tool_tip.append(java.util.List_.of(com.oddlabs.tt.gui.GUIIcons.getIcons().getWatch(health)));
            }
            else if (((object)unit.getOwner() == (object)local_player) && (c is com.oddlabs.tt.model.behaviour.GatherController gc))
            {
                tool_tip.append(com.oddlabs.tt.gui.GUIIcons.getIcons().getToolTipIcon(gc.getSupplyType()));
            }
        }
        public ToolTipAdapter(com.oddlabs.tt.model.ModelToolTip model, com.oddlabs.tt.player.Player local_player) : base()
        {
            this.local_player = local_player;
            this.model = model;
        }
        // mixin of default method ToolTip.hasToolTip
        public bool hasToolTip()
        {
            return true;
        }
    }
}
