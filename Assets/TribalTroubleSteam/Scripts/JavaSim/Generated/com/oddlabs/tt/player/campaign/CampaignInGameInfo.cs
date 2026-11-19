// Translated from CampaignInGameInfo.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.campaign
{
    // Java: 18
    public class CampaignInGameInfo : java.lang.Object, com.oddlabs.tt.viewer.InGameInfo
    {
        public com.oddlabs.tt.player.campaign.Campaign campaign;
        // Java: 25
        public bool isRated()
        {
            return false;
        }
        // Java: 30
        public bool isMultiplayer()
        {
            return false;
        }
        // Java: 35
        public float getRandomStartPosition()
        {
            return 0.0f;
        }
        // Java: 40
        public void addGUI(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.@delegate.InGameMainMenu menu, com.oddlabs.tt.gui.Group game_infos)
        {
            menu.addAbortButton(com.oddlabs.tt.@delegate.Menu.i18n("end_game"));
            int screen_width = viewer.getGUIRoot().getWidth();
            com.oddlabs.tt.gui.LabelBox label_objective = new com.oddlabs.tt.gui.LabelBox(com.oddlabs.tt.@delegate.Menu.i18n("objective"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), screen_width / 2);
            com.oddlabs.tt.gui.LabelBox label_description = new com.oddlabs.tt.gui.LabelBox(campaign.getCurrentObjective(), com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), screen_width / 2);
            game_infos.addChild(label_objective);
            game_infos.addChild(label_description);
            label_objective.place();
            label_description.place(label_objective, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            game_infos.compileCanvas();
        }
        // Java: 54
        public void addGameOverGUI(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.@delegate.GameStatsDelegate @delegate, int header_y, com.oddlabs.tt.gui.Group group)
        {
            com.oddlabs.tt.gui.HorizButton button_ok = new com.oddlabs.tt.gui.OKButton(150);
            button_ok.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                @delegate.startMenu();
            }));
            group.addChild(button_ok);
            button_ok.place();
        }
        // Java: 64
        public void close(com.oddlabs.tt.viewer.WorldViewer viewer)
        {
            if (campaign.getState().getIslandState(0) != com.oddlabs.tt.player.campaign.CampaignState.ISLAND_COMPLETED)
            {
                com.oddlabs.tt.render.Renderer.startMenu(viewer.getNetwork(), viewer.getGUIRoot().getGUI());
            }
            else
            {
                campaign.pushDelegate(viewer.getNetwork(), viewer.getGUIRoot().getGUI());
            }
        }
        // Java: 74
        public void abort(com.oddlabs.tt.viewer.WorldViewer viewer)
        {
            viewer.getGUIRoot().pushDelegate(new com.oddlabs.tt.@delegate.GameStatsDelegate(viewer, viewer.getGUIRoot().getDelegate().getCamera(), com.oddlabs.tt.@delegate.Menu.i18n("game_aborted")));
            campaign.doDefeated();
        }
        public CampaignInGameInfo(com.oddlabs.tt.player.campaign.Campaign campaign) : base()
        {
            this.campaign = campaign;
        }
    }
}
