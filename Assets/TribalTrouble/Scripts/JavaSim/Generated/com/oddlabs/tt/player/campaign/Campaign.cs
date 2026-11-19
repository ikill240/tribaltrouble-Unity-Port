// Translated from Campaign.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.campaign
{
    // Java: 19
    public abstract class Campaign : java.lang.Object
    {
        public static java.util.ResourceBundle bundle;
        // Java: 22
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.player.campaign.Campaign.bundle, key, args);
        }
        public com.oddlabs.tt.player.campaign.CampaignState state;
        public com.oddlabs.tt.player.campaign.CampaignState[] campaign_states;
        // Java: 33
        public com.oddlabs.tt.player.campaign.CampaignState getState()
        {
            return state;
        }
        // Java: 37
        public void pushDelegate(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUI gui)
        {
            com.oddlabs.tt.gui.GUIRoot gui_root = gui.newFade();
            gui_root.pushDelegate(new com.oddlabs.tt.@delegate.CampaignMapForm(network, gui_root, this));
        }
        // Java: 42
        public virtual void defeated(com.oddlabs.tt.viewer.WorldViewer viewer, string game_over_message)
        {
            com.oddlabs.tt.gui.GUIRoot gui_root = viewer.getGUIRoot();
            new com.oddlabs.tt.trigger.GameOverDelayTrigger(viewer, gui_root.getDelegate().getCamera(), game_over_message);
            doDefeated();
            if (state.isSaga())
            {
                save(gui_root);
            }
        }
        // Java: 50
        public void doDefeated()
        {
            onDefeated();
            state.setCurrentIsland(state.getPrevIsland());
        }
        // Java: 56
        public virtual void onDefeated()
        {
        }
        // Java: 59
        public void victory(com.oddlabs.tt.viewer.WorldViewer viewer)
        {
            com.oddlabs.tt.gui.GUIRoot gui_root = viewer.getGUIRoot();
            new com.oddlabs.tt.trigger.GameOverDelayTrigger(viewer, gui_root.getDelegate().getCamera(), i18n("island_complete"));
            save(gui_root);
        }
        // Java: 66
        public void leave(com.oddlabs.tt.viewer.WorldViewer viewer, string message)
        {
            save(viewer.getGUIRoot());
            viewer.getGUIRoot().pushDelegate(new com.oddlabs.tt.@delegate.GameStatsDelegate(viewer, viewer.getGUIRoot().getDelegate().getCamera(), message));
        }
        // Java: 73
        public void save(com.oddlabs.tt.gui.GUIRoot gui_root)
        {
            com.oddlabs.tt.gui.LoadCampaignBox.loadSavegames(new Campaign.Anon1_DeterministicSerializerLoopbackInterface(this, gui_root));
        }
        // Java: 93
        public void doSave(com.oddlabs.tt.gui.GUIRoot gui_root)
        {
            for (int i = 0; i < campaign_states.Length; i++)
            {
                if (java.lang.JO.equals(campaign_states[i].getName(), getState().getName()))
                {
                    campaign_states[i] = getState();
                }
            }
            com.oddlabs.tt.gui.LoadCampaignBox.saveSavegames(campaign_states, new Campaign.Anon2_DeterministicSerializerLoopbackInterface(this, gui_root));
        }
        // Java: 116
        public void doFailed(System.Exception e, com.oddlabs.tt.gui.GUIRoot gui_root)
        {
            string failed_message = i18n("failed_message", com.oddlabs.tt.global.Globals.getSavegamesFileName(), e.getMessage());
            gui_root.addModalForm(new com.oddlabs.tt.form.MessageForm(failed_message));
        }
        // Java: 121
        public abstract com.oddlabs.tt.gui.CampaignIcons getIcons();
        // Java: 123
        public abstract void islandChosen(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root, int number);
        // Java: 125
        public abstract string getCurrentObjective();
        // Java: 127
        public abstract void startIsland(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root, int number);
        static Campaign()
        {
            Campaign.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.player.campaign.Campaign).getName());
        }
        public Campaign(com.oddlabs.tt.player.campaign.CampaignState state) : base()
        {
            this.state = state;
        }
        // Java: anonymous class at line 75
        public sealed class Anon1_DeterministicSerializerLoopbackInterface : java.lang.Object, com.oddlabs.util.DeterministicSerializerLoopbackInterface
        {
            public com.oddlabs.tt.player.campaign.Campaign this_0;
            public com.oddlabs.tt.gui.GUIRoot gui_root;
            public Anon1_DeterministicSerializerLoopbackInterface(com.oddlabs.tt.player.campaign.Campaign this_0_, com.oddlabs.tt.gui.GUIRoot gui_root_c)
            {
                this.this_0 = this_0_;
                this.gui_root = gui_root_c;
            }
            // Java: 76
            public void loadSucceeded(object campaign_states_p)
            {
                com.oddlabs.tt.player.campaign.CampaignState[] campaign_states = (com.oddlabs.tt.player.campaign.CampaignState[])campaign_states_p;
                this.this_0.campaign_states = campaign_states;
                this_0.doSave(gui_root);
            }
            // Java: 82
            public void saveSucceeded()
            {
            }
            // Java: 86
            public void failed(System.Exception e)
            {
                this_0.doFailed(e, gui_root);
            }
        }
        // Java: anonymous class at line 100
        public sealed class Anon2_DeterministicSerializerLoopbackInterface : java.lang.Object, com.oddlabs.util.DeterministicSerializerLoopbackInterface
        {
            public com.oddlabs.tt.player.campaign.Campaign this_0;
            public com.oddlabs.tt.gui.GUIRoot gui_root;
            public Anon2_DeterministicSerializerLoopbackInterface(com.oddlabs.tt.player.campaign.Campaign this_0_, com.oddlabs.tt.gui.GUIRoot gui_root_c)
            {
                this.this_0 = this_0_;
                this.gui_root = gui_root_c;
            }
            // Java: 101
            public void loadSucceeded(object @object_p)
            {
                com.oddlabs.tt.player.campaign.CampaignState[] @object = (com.oddlabs.tt.player.campaign.CampaignState[])@object_p;
            }
            // Java: 105
            public void saveSucceeded()
            {
            }
            // Java: 109
            public void failed(System.Exception e)
            {
                this_0.doFailed(e, gui_root);
            }
        }
    }
}
