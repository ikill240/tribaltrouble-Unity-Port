// Translated from InGameDelegate.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.@delegate
{
    // Java: 21
    public abstract class InGameDelegate : com.oddlabs.tt.@delegate.CameraDelegate
    {
        public com.oddlabs.tt.viewer.WorldViewer viewer;
        // Java: 29
        public bool cheat(java.util.Set actions)
        {
            com.oddlabs.tt.viewer.Cheat cheat_v = viewer.getCheat();
            if (!cheat_v.isEnabled())
            {
                return false;
            }
            com.oddlabs.tt.render.LandscapeLocation landscape_hit = new com.oddlabs.tt.render.LandscapeLocation();
            viewer.getPicker().pickLocation(getCamera().getState(), landscape_hit);
            float landscape_x = landscape_hit.x;
            float landscape_y = landscape_hit.y;
            if (actions.contains(com.oddlabs.tt.input.GameAction.CHEAT_1))
            {
                if (viewer.getLocalPlayer().getUnitCountContainer().getNumSupplies() != viewer.getParameters().getMaxUnitCount())
                {
                    new com.oddlabs.tt.model.Unit(viewer.getLocalPlayer(), landscape_x, landscape_y, null, viewer.getLocalPlayer().getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
                    return true;
                }
            }
            if (actions.contains(com.oddlabs.tt.input.GameAction.CHEAT_2))
            {
                if (viewer.getLocalPlayer().getUnitCountContainer().getNumSupplies() != viewer.getParameters().getMaxUnitCount())
                {
                    new com.oddlabs.tt.model.Unit(viewer.getLocalPlayer(), landscape_x, landscape_y, null, viewer.getLocalPlayer().getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
                    return true;
                }
            }
            if (actions.contains(com.oddlabs.tt.input.GameAction.CHEAT_3))
            {
                if (viewer.getLocalPlayer().getUnitCountContainer().getNumSupplies() != viewer.getParameters().getMaxUnitCount())
                {
                    new com.oddlabs.tt.model.Unit(viewer.getLocalPlayer(), landscape_x, landscape_y, null, viewer.getLocalPlayer().getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
                    return true;
                }
            }
            if (actions.contains(com.oddlabs.tt.input.GameAction.CHEAT_4))
            {
                if (viewer.getLocalPlayer().getUnitCountContainer().getNumSupplies() != viewer.getParameters().getMaxUnitCount())
                {
                    new com.oddlabs.tt.model.Unit(viewer.getLocalPlayer(), landscape_x, landscape_y, null, viewer.getLocalPlayer().getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
                    return true;
                }
            }
            if (actions.contains(com.oddlabs.tt.input.GameAction.CHEAT_5))
            {
                if (!viewer.getLocalPlayer().hasActiveChieftain() && !viewer.getLocalPlayer().isTrainingChieftain())
                {
                    com.oddlabs.tt.model.Unit chieftain = new com.oddlabs.tt.model.Unit(viewer.getLocalPlayer(), landscape_x, landscape_y, null, viewer.getLocalPlayer().getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_CHIEFTAIN));
                    viewer.getLocalPlayer().setActiveChieftain(chieftain);
                    return true;
                }
            }
            if (actions.contains(com.oddlabs.tt.input.GameAction.CHEAT_6))
            {
                viewer.getLocalPlayer().killSelection(viewer.getSelection().getCurrentSelection().filter(com.oddlabs.tt.model.Abilities.NONE));
                return true;
            }
            if (actions.contains(com.oddlabs.tt.input.GameAction.CHEAT_7))
            {
                cheat_v.draw_trees = !cheat_v.draw_trees;
                return true;
            }
            if (actions.contains(com.oddlabs.tt.input.GameAction.CHEAT_8))
            {
                cheat_v.line_mode = !cheat_v.line_mode;
                return true;
            }
            if (actions.contains(com.oddlabs.tt.input.GameAction.CHEAT_9))
            {
                com.oddlabs.tt.resource.FogInfo fog_info = viewer.getGUIRoot().getDelegate().getCamera().getState().getFog();
                fog_info.setEnabled(!fog_info.isEnabled());
                return true;
            }
            if (actions.contains(com.oddlabs.tt.input.GameAction.CHEAT_10))
            {
                viewer.getWorld().toggleNight();
                return true;
            }
            if (!com.oddlabs.tt.global.Settings.getSettings().inDeveloperMode())
            {
                return false;
            }
            if (actions.contains(com.oddlabs.tt.input.GameAction.DEBUG_PRINT_INFO))
            {
                java.util.Set set = viewer.getSelection().getCurrentSelection().getSet();
                if (!set.isEmpty())
                {
                    com.oddlabs.tt.model.Selectable s = ((com.oddlabs.tt.model.Selectable)set.iterator().next());
                    if ((s is com.oddlabs.tt.model.Building building))
                    {
                        if (!building.isDead() && !building.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.ATTACK))
                        {
                            building.printDebugInfo();
                        }
                    }
                    else if ((s is com.oddlabs.tt.model.Unit unit))
                    {
                        if (!unit.isDead())
                        {
                            unit.printDebugInfo();
                        }
                    }
                }
                return true;
            }
            return false;
        }
        // Java: 129
        public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
            base.handleInput(@event);
            if (@event.isConsumed())
            {
                return;
            }
            if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.PRESSED))
            {
                if (@event.consumeAction(com.oddlabs.tt.input.GameAction.GLOBAL_MENU))
                {
                    getGUIRoot().pushDelegate(new com.oddlabs.tt.@delegate.InGameMainMenu(viewer, new com.oddlabs.tt.camera.StaticCamera(getCamera().getState())));
                    @event.consume();
                    return;
                }
                if (cheat(@event.getActions()))
                {
                    @event.consume();
                    return;
                }
            }
        }
        // Java: 148
        public com.oddlabs.tt.viewer.WorldViewer getViewer()
        {
            return viewer;
        }
        public InGameDelegate(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.camera.Camera camera) : base(viewer.getGUIRoot(), camera)
        {
            this.viewer = viewer;
        }
    }
}
