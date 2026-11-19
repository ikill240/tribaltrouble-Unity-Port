// Translated from NewCampaignForm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 43
    public class NewCampaignForm : com.oddlabs.tt.gui.Form, com.oddlabs.util.DeterministicSerializerLoopbackInterface
    {
        public const int BUTTON_WIDTH = 100;
        public const int PULLDOWNBUTTON_WIDTH = 110;
        public const int EDITLINE_WIDTH = 240;
        public const int INDEX_VIKINGS = 0;
        public const int INDEX_NATIVES = 1;
        public com.oddlabs.tt.@delegate.Menu main_menu;
        public com.oddlabs.tt.form.CampaignForm campaign_form;
        public static java.util.ResourceBundle bundle;
        // Java: 55
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.form.NewCampaignForm.bundle, key, args);
        }
        public com.oddlabs.tt.gui.EditLine editline_name;
        public com.oddlabs.tt.gui.PulldownMenu race_pulldown;
        public com.oddlabs.tt.gui.PulldownMenu difficulty_pulldown;
        public com.oddlabs.tt.gui.GUIRoot gui_root;
        public com.oddlabs.net.NetworkSelector network;
        public com.oddlabs.tt.player.campaign.CampaignState[] campaign_states;
        // Java: 136
        public override void doCancel()
        {
            main_menu.setMenu(campaign_form);
        }
        // Java: 141
        public override void setFocus(com.oddlabs.tt.gui.FocusDirection direction)
        {
            if (((object)direction == (object)com.oddlabs.tt.gui.FocusDirection.BACKWARD))
            {
                base.setFocus(direction);
            }
            else
            {
                editline_name.setFocus(direction);
            }
        }
        // Java: 150
        public bool nameIsUnique(string name)
        {
            if (campaign_states != null)
            {
                foreach (com.oddlabs.tt.player.campaign.CampaignState campaign_state in campaign_states)
                {
                    if (java.lang.JO.equals(campaign_state.getName(), name))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        // Java: 161
        public void save()
        {
            string name = editline_name.getContents().trim();
            if (name.isEmpty())
            {
                gui_root.addModalForm(new com.oddlabs.tt.form.MessageForm(i18n("invalid")));
                return;
            }
            if (!nameIsUnique(name))
            {
                gui_root.addModalForm(new com.oddlabs.tt.form.MessageForm(i18n("exists")));
                return;
            }
            com.oddlabs.tt.player.campaign.CampaignState[] new_states = default;
            if (campaign_states != null)
            {
                new_states = new com.oddlabs.tt.player.campaign.CampaignState[campaign_states.Length + 1];
                java.lang.JSystem.arraycopy(campaign_states, 0, new_states, 0, campaign_states.Length);
            }
            else
            {
                new_states = new com.oddlabs.tt.player.campaign.CampaignState[1];
            }
            com.oddlabs.tt.player.campaign.Campaign campaign = default;
            switch ((race_pulldown.getChosenItemIndex()))
            {
                case 0:
                {
                    campaign = new com.oddlabs.tt.player.campaign.VikingCampaign(network, gui_root);
                    campaign.getState().setRace(com.oddlabs.tt.player.campaign.CampaignState.RACE_VIKINGS);
                    break;
                }
                case 1:
                {
                    campaign = new com.oddlabs.tt.player.campaign.NativeCampaign(network, gui_root);
                    campaign.getState().setRace(com.oddlabs.tt.player.campaign.CampaignState.RACE_NATIVES);
                    break;
                }
                default:
                {
                    throw new System.ArgumentException();
                }
            }
            campaign.getState().setName(name);
            campaign.getState().setDate(java.lang.JSystem.currentTimeMillis());
            int difficulty = ((difficulty_pulldown.getChosenItemIndex()) switch { 0 => (int)(com.oddlabs.tt.player.campaign.CampaignState.DIFFICULTY_EASY), 1 => (int)(com.oddlabs.tt.player.campaign.CampaignState.DIFFICULTY_NORMAL), 2 => (int)(com.oddlabs.tt.player.campaign.CampaignState.DIFFICULTY_HARD), _ => throw new System.ArgumentException() });
            campaign.getState().setDifficulty(difficulty);
            new_states[new_states.Length - 1] = campaign.getState();
            com.oddlabs.tt.gui.LoadCampaignBox.saveSavegames(new_states, this);
            remove();
        }
        // Java: 207
        public void saveSucceeded()
        {
        }
        // Java: 211
        public void loadSucceeded(object campaign_states_p)
        {
            com.oddlabs.tt.player.campaign.CampaignState[] campaign_states = (com.oddlabs.tt.player.campaign.CampaignState[])campaign_states_p;
            this.campaign_states = campaign_states;
        }
        // Java: 216
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
                string failed_message = i18n("failed_message", com.oddlabs.tt.global.Globals.getSavegamesFileName(), e.getMessage());
                gui_root.addModalForm(new com.oddlabs.tt.form.MessageForm(failed_message));
            }
        }
        // Java: 226
        public class NameListener : java.lang.Object, com.oddlabs.tt.guievent.MouseClickListener, com.oddlabs.tt.guievent.EnterListener
        {
            public com.oddlabs.tt.form.NewCampaignForm this_0;
            // Java: 227
            public void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
            {
                this_0.save();
            }
            // Java: 232
            public void enterPressed(string text)
            {
                this_0.save();
            }
            public NameListener(com.oddlabs.tt.form.NewCampaignForm this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        // Java: 238
        public class RaceListener : java.lang.Object, com.oddlabs.tt.guievent.ItemChosenListener
        {
            public com.oddlabs.tt.form.NewCampaignForm this_0;
            // Java: 239
            public void itemChosen(com.oddlabs.tt.gui.PulldownMenu menu, int item_index)
            {
                if (item_index == com.oddlabs.tt.form.NewCampaignForm.INDEX_NATIVES && (!com.oddlabs.tt.global.Settings.getSettings().has_native_campaign))
                {
                    menu.chooseItem(com.oddlabs.tt.form.NewCampaignForm.INDEX_VIKINGS);
                    this_0.gui_root.addModalForm(new com.oddlabs.tt.form.MessageForm(this_0.i18n("native_unavailable")));
                }
            }
            public RaceListener(com.oddlabs.tt.form.NewCampaignForm this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        static NewCampaignForm()
        {
            NewCampaignForm.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.form.NewCampaignForm).getName());
        }
        public NewCampaignForm(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.@delegate.Menu main_menu, com.oddlabs.tt.form.CampaignForm campaign_form) : base()
        {
            this.network = network;
            this.gui_root = gui_root;
            this.main_menu = main_menu;
            this.campaign_form = campaign_form;
            com.oddlabs.tt.gui.Label label_headline = new com.oddlabs.tt.gui.Label(i18n("caption"), com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont());
            addChild(label_headline);
            com.oddlabs.tt.gui.Group group = new com.oddlabs.tt.gui.Group();
            com.oddlabs.tt.gui.Label name_label = new com.oddlabs.tt.gui.Label(i18n("name"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            editline_name = new com.oddlabs.tt.gui.EditLine(com.oddlabs.tt.form.NewCampaignForm.EDITLINE_WIDTH, 200);
            editline_name.addEnterListener(new com.oddlabs.tt.form.NewCampaignForm.NameListener(this));
            group.addChild(name_label);
            group.addChild(editline_name);
            com.oddlabs.tt.gui.Label race_label = new com.oddlabs.tt.gui.Label(i18n("race"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            race_pulldown = new com.oddlabs.tt.gui.PulldownMenu();
            race_pulldown.addItem(new com.oddlabs.tt.gui.PulldownItem(i18n("vikings")));
            race_pulldown.addItem(new com.oddlabs.tt.gui.PulldownItem(i18n("natives")));
            race_pulldown.addItemChosenListener(new com.oddlabs.tt.form.NewCampaignForm.RaceListener(this));
            com.oddlabs.tt.gui.PulldownButton race_pb = new com.oddlabs.tt.gui.PulldownButton(gui_root, race_pulldown, com.oddlabs.tt.form.NewCampaignForm.INDEX_VIKINGS, com.oddlabs.tt.form.NewCampaignForm.PULLDOWNBUTTON_WIDTH);
            group.addChild(race_label);
            group.addChild(race_pb);
            com.oddlabs.tt.gui.Label difficulty_label = new com.oddlabs.tt.gui.Label(i18n("difficulty"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            difficulty_pulldown = new com.oddlabs.tt.gui.PulldownMenu();
            difficulty_pulldown.addItem(new com.oddlabs.tt.gui.PulldownItem(i18n("easy")));
            difficulty_pulldown.addItem(new com.oddlabs.tt.gui.PulldownItem(i18n("normal")));
            difficulty_pulldown.addItem(new com.oddlabs.tt.gui.PulldownItem(i18n("hard")));
            com.oddlabs.tt.gui.PulldownButton difficulty_pb = new com.oddlabs.tt.gui.PulldownButton(gui_root, difficulty_pulldown, 1, com.oddlabs.tt.form.NewCampaignForm.PULLDOWNBUTTON_WIDTH);
            group.addChild(difficulty_label);
            group.addChild(difficulty_pb);
            editline_name.place();
            name_label.place(editline_name, com.oddlabs.tt.gui.Placement.LEFT_MID);
            race_pb.place(editline_name, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            race_label.place(race_pb, com.oddlabs.tt.gui.Placement.LEFT_MID);
            difficulty_pb.place(race_pb, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            difficulty_label.place(difficulty_pb, com.oddlabs.tt.gui.Placement.LEFT_MID);
            group.compileCanvas();
            addChild(group);
            com.oddlabs.tt.gui.ButtonObject button_ok = new com.oddlabs.tt.gui.OKButton(com.oddlabs.tt.form.NewCampaignForm.BUTTON_WIDTH);
            button_ok.addMouseClickListener(new com.oddlabs.tt.form.NewCampaignForm.NameListener(this));
            addChild(button_ok);
            com.oddlabs.tt.gui.ButtonObject button_cancel = new com.oddlabs.tt.gui.CancelButton(com.oddlabs.tt.form.NewCampaignForm.BUTTON_WIDTH);
            button_cancel.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                this.cancel();
            }));
            addChild(button_cancel);
            label_headline.place();
            group.place(label_headline, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            button_cancel.place(com.oddlabs.tt.gui.Origin.AT_END);
            button_ok.place(button_cancel, com.oddlabs.tt.gui.Placement.LEFT_MID);
            compileCanvas();
            centerPos();
            com.oddlabs.tt.gui.LoadCampaignBox.loadSavegames(this);
        }
    }
}
