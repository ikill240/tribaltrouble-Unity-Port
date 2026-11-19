// Translated from CampaignForm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 35
    public class CampaignForm : com.oddlabs.tt.gui.Form, com.oddlabs.util.DeterministicSerializerLoopbackInterface
    {
        public static java.util.logging.Logger logger;
        public com.oddlabs.tt.gui.HorizButton button_vikings;
        public com.oddlabs.tt.gui.HorizButton button_load;
        public com.oddlabs.tt.gui.HorizButton button_delete;
        public com.oddlabs.tt.gui.LoadCampaignBox load_campaign_box;
        public static java.util.ResourceBundle bundle;
        // Java: 44
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.form.CampaignForm.bundle, key, args);
        }
        public com.oddlabs.tt.gui.GUIRoot gui_root;
        public com.oddlabs.net.NetworkSelector network;
        // Java: 113
        public override void setFocus(com.oddlabs.tt.gui.FocusDirection direction)
        {
            if (((object)direction == (object)com.oddlabs.tt.gui.FocusDirection.BACKWARD))
            {
                base.setFocus(direction);
            }
            else
            {
                button_vikings.setFocus(direction);
            }
        }
        // Java: 122
        public void load(com.oddlabs.tt.player.campaign.CampaignState campaign_state)
        {
            com.oddlabs.tt.player.campaign.Campaign campaign = (campaign_state.isSaga() ? (com.oddlabs.tt.player.campaign.Campaign)(new com.oddlabs.tt.player.campaign.SagaCampaign(network, gui_root, campaign_state)) : (campaign_state.getRace() == com.oddlabs.tt.player.campaign.CampaignState.RACE_VIKINGS ? (com.oddlabs.tt.player.campaign.Campaign)(new com.oddlabs.tt.player.campaign.VikingCampaign(network, gui_root, campaign_state)) : (com.oddlabs.tt.player.campaign.Campaign)(new com.oddlabs.tt.player.campaign.NativeCampaign(network, gui_root, campaign_state))));
            setDisabled(true);
            if (campaign_state.getIslandState(0) == com.oddlabs.tt.player.campaign.CampaignState.ISLAND_COMPLETED)
            {
                campaign.pushDelegate(network, gui_root.getGUI());
            }
            else
            {
                campaign.startIsland(network, gui_root, 0);
            }
        }
        // Java: 134
        public void saveSucceeded()
        {
            load_campaign_box.refresh();
        }
        // Java: 139
        public void loadSucceeded(object campaign_states_p)
        {
            com.oddlabs.tt.player.campaign.CampaignState[] campaign_states = (com.oddlabs.tt.player.campaign.CampaignState[])campaign_states_p;
            com.oddlabs.tt.player.campaign.CampaignState selected = load_campaign_box.getSelected();
            if (selected != null)
            {
                com.oddlabs.tt.player.campaign.CampaignState[] new_states = new com.oddlabs.tt.player.campaign.CampaignState[campaign_states.Length - 1];
                int offset = 0;
                for (int i = 0; i < campaign_states.Length; i++)
                {
                    if (java.lang.JO.equals(campaign_states[i].getName(), selected.getName()))
                    {
                        offset = 1;
                    }
                    else
                    {
                        new_states[i - offset] = campaign_states[i];
                    }
                }
                com.oddlabs.tt.gui.LoadCampaignBox.saveSavegames(new_states, this);
            }
        }
        // Java: 156
        public void failed(System.Exception e)
        {
            if ((e is System.IO.IOException) || (e is System.IO.IOException))
            {
            }
            else if ((e is System.IO.IOException))
            {
            }
            else
            {
                com.oddlabs.tt.form.CampaignForm.logger.log(java.util.logging.Level.SEVERE, "Load failed", e);
                string failed_message = i18n("failed_message", com.oddlabs.tt.global.Globals.getSavegamesFileName(), e.getMessage());
                gui_root.addModalForm(new com.oddlabs.tt.form.MessageForm(failed_message));
            }
        }
        // Java: 167
        public void mouseClickedDelete(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
        {
            com.oddlabs.tt.player.campaign.CampaignState state = load_campaign_box.getSelected();
            if (state != null)
            {
                string confirm_str = i18n("confirm_delete", state.getName());
                gui_root.addModalForm(new com.oddlabs.tt.form.QuestionForm(confirm_str, new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
                {
                    com.oddlabs.tt.gui.LoadCampaignBox.loadSavegames(this);
                })));
            }
        }
        static CampaignForm()
        {
            CampaignForm.logger = java.util.logging.Logger.getLogger(typeof(com.oddlabs.tt.form.CampaignForm).getSimpleName());
            CampaignForm.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.form.CampaignForm).getName());
        }
        public CampaignForm(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.@delegate.Menu main_menu) : base()
        {
            this.gui_root = gui_root;
            this.network = network;
            com.oddlabs.tt.gui.Label headline = new com.oddlabs.tt.gui.Label(i18n("campaign"), com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont());
            addChild(headline);
            button_delete = new com.oddlabs.tt.gui.HorizButton(i18n("delete"), 120);
            button_delete.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton a0, int a1, int a2, int a3) => { this.mouseClickedDelete(a0, a1, a2, a3); }));
            button_delete.setDisabled(true);
            button_vikings = new com.oddlabs.tt.gui.HorizButton(i18n("new"), 120);
            button_vikings.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                main_menu.setMenu(new com.oddlabs.tt.form.NewCampaignForm(network, gui_root, main_menu, this));
            }));
            button_load = new com.oddlabs.tt.gui.HorizButton(i18n("load"), 120);
            button_load.setDisabled(true);
            com.oddlabs.tt.gui.HorizButton button_cancel = new com.oddlabs.tt.gui.CancelButton(120);
            button_cancel.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_5, int unused_6, int unused_7, int unused_8) =>
            {
                this.cancel();
            }));
            com.oddlabs.tt.guievent.RowListener listListener = new CampaignForm.Anon1_RowListener(this);
            load_campaign_box = new com.oddlabs.tt.gui.LoadCampaignBox(gui_root, listListener);
            button_load.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_9, int unused_10, int unused_11, int unused_12) =>
            {
                com.oddlabs.tt.player.campaign.CampaignState selected = load_campaign_box.getSelected();
                if (selected != null)
                {
                    load(selected);
                }
            }));
            addChild(button_delete);
            addChild(button_vikings);
            addChild(load_campaign_box);
            addChild(button_load);
            addChild(button_cancel);
            headline.place();
            load_campaign_box.place(button_vikings, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            button_cancel.place(com.oddlabs.tt.gui.Origin.AT_END);
            button_delete.place(button_cancel, com.oddlabs.tt.gui.Placement.LEFT_MID);
            button_load.place(button_delete, com.oddlabs.tt.gui.Placement.LEFT_MID);
            button_vikings.place(button_load, com.oddlabs.tt.gui.Placement.LEFT_MID);
            compileCanvas();
            centerPos();
        }
        // Java: anonymous class at line 72
        public sealed class Anon1_RowListener : java.lang.Object, com.oddlabs.tt.guievent.RowListener
        {
            public com.oddlabs.tt.form.CampaignForm this_0;
            public Anon1_RowListener(com.oddlabs.tt.form.CampaignForm this_0_)
            {
                this.this_0 = this_0_;
            }
            // Java: 73
            public void rowDoubleClicked(object @object_p)
            {
                com.oddlabs.tt.player.campaign.CampaignState @object = (com.oddlabs.tt.player.campaign.CampaignState)@object_p;
                this_0.load(@object);
            }
            // Java: 78
            public void rowChosen(object @object_p)
            {
                com.oddlabs.tt.player.campaign.CampaignState @object = (com.oddlabs.tt.player.campaign.CampaignState)@object_p;
                this_0.button_delete.setDisabled(false);
                this_0.button_load.setDisabled(false);
            }
        }
    }
}
