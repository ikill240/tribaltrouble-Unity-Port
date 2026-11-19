// Translated from LoadCampaignBox.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 25
    public class LoadCampaignBox : com.oddlabs.tt.gui.GUIObject, com.oddlabs.util.DeterministicSerializerLoopbackInterface
    {
        public static java.util.logging.Logger logger;
        public const int WIDTH_NAME = 210;
        public const int WIDTH_RACE = 100;
        public const int WIDTH_DIFFICULTY = 130;
        public const int WIDTH_DATE = 170;
        public com.oddlabs.tt.gui.MultiColumnComboBox list_box;
        public com.oddlabs.tt.gui.GUIRoot gui_root;
        public static java.util.ResourceBundle bundle;
        // Java: 36
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.gui.LoadCampaignBox.bundle, key, args);
        }
        // Java: 54
        public static void saveSavegames(com.oddlabs.tt.player.campaign.CampaignState[] states, com.oddlabs.util.DeterministicSerializerLoopbackInterface callback)
        {
            com.oddlabs.util.DeterministicSerializer.save(com.oddlabs.tt.@event.LocalEventQueue.getQueue().getDeterministic(), states, com.oddlabs.tt.gui.LoadCampaignBox.getSaveSavegamesFile(), callback);
        }
        // Java: 60
        public static java.nio.file.Path getSaveSavegamesFile()
        {
            return com.oddlabs.tt.render.Renderer.getLocalInput().getGameDir().resolve(com.oddlabs.tt.global.Globals.getSavegamesFileName());
        }
        // Java: 64
        public static void loadSavegames(com.oddlabs.util.DeterministicSerializerLoopbackInterface callback)
        {
            com.oddlabs.util.DeterministicSerializer.load(com.oddlabs.tt.@event.LocalEventQueue.getQueue().getDeterministic(), com.oddlabs.tt.gui.LoadCampaignBox.getLoadSavegamesFile(), callback);
        }
        // Java: 68
        public static java.nio.file.Path getLoadSavegamesFile()
        {
            java.nio.file.Path file = com.oddlabs.tt.gui.LoadCampaignBox.getSaveSavegamesFile();
            return (!java.nio.file.Files.isReadable(file) ? com.oddlabs.tt.util.Utils.getInstallDir().resolve(com.oddlabs.tt.global.Globals.getSavegamesFileName()) : file);
        }
        // Java: 73
        public override void setFocus(com.oddlabs.tt.gui.FocusDirection direction)
        {
            list_box.setFocus(direction);
        }
        // Java: 78
        public com.oddlabs.tt.player.campaign.CampaignState getSelected()
        {
            return ((com.oddlabs.tt.player.campaign.CampaignState)list_box.getSelected());
        }
        // Java: 82
        public void refresh()
        {
            list_box.clear();
            com.oddlabs.tt.gui.LoadCampaignBox.loadSavegames(this);
        }
        // Java: 87
        public void fillSlots(com.oddlabs.tt.player.campaign.CampaignState[] campaign_states)
        {
            com.oddlabs.tt.gui.Box box = com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().box();
            foreach (com.oddlabs.tt.player.campaign.CampaignState campaign_state in campaign_states)
            {
                string race = ((campaign_state.getRace()) switch { 0 => (string)(i18n("vikings")), 1 => (string)(i18n("natives")), _ => throw new System.ArgumentException("invalid race") });
                string difficulty = ((campaign_state.getDifficulty()) switch { 1 => (string)(i18n("easy")), 0 => (string)(i18n("normal")), 2 => (string)(i18n("hard")), _ => throw new System.ArgumentException("invalid difficulty") });
                com.oddlabs.tt.gui.Row row = new com.oddlabs.tt.gui.Row(java.util.List_.of(new com.oddlabs.tt.gui.Label(campaign_state.getName(), com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font(), com.oddlabs.tt.gui.LoadCampaignBox.WIDTH_NAME - box.getLeftOffset() - 1), new com.oddlabs.tt.gui.Label(race, com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font(), com.oddlabs.tt.gui.LoadCampaignBox.WIDTH_RACE), new com.oddlabs.tt.gui.Label(difficulty, com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font(), com.oddlabs.tt.gui.LoadCampaignBox.WIDTH_DIFFICULTY), new com.oddlabs.tt.gui.DateLabel(campaign_state.getDate(), com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font(), com.oddlabs.tt.gui.LoadCampaignBox.WIDTH_DATE - box.getRightOffset() + 1)), campaign_state);
                list_box.addRow(row);
            }
        }
        // Java: 114
        public void loadSucceeded(object campaign_states_p)
        {
            com.oddlabs.tt.player.campaign.CampaignState[] campaign_states = (com.oddlabs.tt.player.campaign.CampaignState[])campaign_states_p;
            fillSlots(campaign_states);
            if (list_box.getSize() > 0)
            {
                list_box.selectFirst();
            }
        }
        // Java: 122
        public void saveSucceeded()
        {
        }
        // Java: 126
        public void failed(System.Exception e)
        {
            com.oddlabs.tt.gui.LoadCampaignBox.logger.log(java.util.logging.Level.SEVERE, "Failed to load savegames", e);
            if ((e is System.IO.IOException) || (e is System.IO.IOException))
            {
            }
            else if ((e is System.IO.IOException))
            {
                string invalid_message = i18n("invalid_message", com.oddlabs.tt.global.Globals.getSavegamesFileName());
                gui_root.addModalForm(new com.oddlabs.tt.form.MessageForm(invalid_message));
            }
            else
            {
                string failed_message = i18n("failed_message", com.oddlabs.tt.global.Globals.getSavegamesFileName(), e.getMessage());
                gui_root.addModalForm(new com.oddlabs.tt.form.MessageForm(failed_message));
            }
        }
        static LoadCampaignBox()
        {
            LoadCampaignBox.logger = java.util.logging.Logger.getLogger(typeof(com.oddlabs.tt.gui.LoadCampaignBox).getSimpleName());
            LoadCampaignBox.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.gui.LoadCampaignBox).getName());
        }
        public LoadCampaignBox(com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.guievent.RowListener listener) : base()
        {
            this.gui_root = gui_root;
            com.oddlabs.tt.gui.ColumnInfo[] infos = new com.oddlabs.tt.gui.ColumnInfo[] { new com.oddlabs.tt.gui.ColumnInfo(i18n("name"), com.oddlabs.tt.gui.LoadCampaignBox.WIDTH_NAME), new com.oddlabs.tt.gui.ColumnInfo(i18n("race"), com.oddlabs.tt.gui.LoadCampaignBox.WIDTH_RACE), new com.oddlabs.tt.gui.ColumnInfo(i18n("difficulty"), com.oddlabs.tt.gui.LoadCampaignBox.WIDTH_DIFFICULTY), new com.oddlabs.tt.gui.ColumnInfo(i18n("date"), com.oddlabs.tt.gui.LoadCampaignBox.WIDTH_DATE) };
            list_box = new com.oddlabs.tt.gui.MultiColumnComboBox(gui_root, infos, 262);
            list_box.addRowListener(listener);
            addChild(list_box);
            setCanFocus(true);
            setDim(list_box.getWidth(), list_box.getHeight());
            refresh();
        }
    }
}
