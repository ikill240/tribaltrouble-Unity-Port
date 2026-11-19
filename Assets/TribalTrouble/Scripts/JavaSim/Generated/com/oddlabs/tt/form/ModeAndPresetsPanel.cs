// Translated from ModeAndPresetsPanel.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 39
    public class ModeAndPresetsPanel : com.oddlabs.tt.gui.Panel
    {
        public const int CARD_WIDTH = 220;
        public const int GRID_VISIBLE_HEIGHT = 150;
        public const int BANNER_BUTTON_WIDTH = 90;
        public const int MODE_PULLDOWN_WIDTH = 180;
        public static java.util.ResourceBundle bundle;
        public com.oddlabs.tt.gamemode.PresetLibrary library;
        public com.oddlabs.tt.form.ModeAndPresetsHandler handler;
        public com.oddlabs.tt.gui.Group mode_row;
        public com.oddlabs.tt.form.ModeAndPresetsPanel.PresetBanner banner;
        public com.oddlabs.tt.gui.ScrollableGroup preset_grid;
        public string selected_preset_id;
        public com.oddlabs.matchmaking.GameMode current_mode;
        // an option that belongs to one mode (King of the Island's statue count): beside the mode while it is chosen
        public com.oddlabs.matchmaking.GameMode option_mode;
        public com.oddlabs.tt.gui.GUIObject mode_option;
        public com.oddlabs.tt.gui.GUIObject mode_button_ref;
        public bool mode_option_shown;
        public void setModeOption(com.oddlabs.matchmaking.GameMode mode, com.oddlabs.tt.gui.GUIObject option)
        {
            option_mode = mode;
            mode_option = option;
            updateModeOption();
        }
        public void updateModeOption()
        {
            if (mode_option == null || mode_row == null)
            {
                return;
            }
            bool show = ((object)current_mode == (object)option_mode);
            if (show && !mode_option_shown)
            {
                mode_row.addChild(mode_option);
                mode_option.place(mode_button_ref, com.oddlabs.tt.gui.Placement.RIGHT_MID, 3 * com.oddlabs.tt.gui.Skin.getSkin().getFormData().sectionSpacing());
                mode_row.compileCanvas();
                mode_option_shown = true;
            }
            else if (!show && mode_option_shown)
            {
                mode_row.removeChild(mode_option);
                mode_row.compileCanvas();
                mode_option_shown = false;
            }
        }
        // Java: 105
        public void refreshPresets()
        {
            rebuildGrid();
        }
        // Java: 109
        public void rebuildGrid()
        {
            int previous_offset = 0;
            if (preset_grid != null)
            {
                previous_offset = preset_grid.getOffsetY();
                removeChild(preset_grid);
            }
            com.oddlabs.tt.gui.ScrollableGroup new_grid = new com.oddlabs.tt.gui.ScrollableGroup(com.oddlabs.tt.form.ModeAndPresetsPanel.GRID_VISIBLE_HEIGHT, 8);
            com.oddlabs.tt.gui.RadioButtonGroup preset_group = new com.oddlabs.tt.gui.RadioButtonGroup();
            java.util.List cards = new java.util.ArrayList();
            foreach (com.oddlabs.matchmaking.Preset preset in java.lang.JIter.of(library.forMode(current_mode)))
            {
                bool marked = java.lang.JO.equals(preset.getId(), selected_preset_id);
                cards.add(new com.oddlabs.tt.gui.PresetCard(preset, com.oddlabs.tt.form.ModeAndPresetsPanel.CARD_WIDTH, marked, preset_group, new com.oddlabs.tt.guievent.PresetChosenListener_L((com.oddlabs.matchmaking.Preset a0) => { handler.presetChosen(a0); }), new com.oddlabs.tt.guievent.PresetDeleteListener_L((com.oddlabs.matchmaking.Preset a0) => { handler.presetDeleted(a0); })));
            }
            cards.add(new com.oddlabs.tt.form.ModeAndPresetsPanel.SavePresetCard(com.oddlabs.tt.form.ModeAndPresetsPanel.CARD_WIDTH, new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                handler.saveClicked();
            })));
            com.oddlabs.tt.gui.GUIObject previous_row_anchor = null;
            com.oddlabs.tt.gui.GUIObject row_anchor = null;
            for (int i = 0; i < cards.size(); i++)
            {
                com.oddlabs.tt.gui.GUIObject card = ((com.oddlabs.tt.gui.GUIObject)cards.get(i));
                new_grid.addChild(card);
                if (i % 2 == 0)
                {
                    if (previous_row_anchor == null)
                    {
                        card.place();
                    }
                    else
                    {
                        card.place(previous_row_anchor, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
                    }
                    previous_row_anchor = card;
                    row_anchor = card;
                }
                else
                {
                    card.place(row_anchor, com.oddlabs.tt.gui.Placement.RIGHT_MID);
                }
            }
            new_grid.compileCanvas();
            preset_grid = new_grid;
            addChild(new_grid);
            new_grid.place(banner, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT, com.oddlabs.tt.gui.Skin.getSkin().getFormData().sectionSpacing());
            new_grid.setOffsetY(previous_offset);
        }
        // Java: 159
        public void setPresetState(com.oddlabs.matchmaking.Preset preset, bool modified)
        {
            selected_preset_id = (preset != null ? preset.getId() : (string)(null));
            banner.setState(preset, modified);
        }
        // Java: 164
        public static string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.form.ModeAndPresetsPanel.bundle, key, args);
        }
        // Java: 172
        public class SavePresetCard : com.oddlabs.tt.gui.Group
        {
            // Java: 186
            public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
            {
                com.oddlabs.tt.gui.PresetCard.renderCardBackground(renderer, this, isActive() || isHovered());
            }
            public SavePresetCard(int width, com.oddlabs.tt.guievent.MouseClickListener listener) : base()
            {
                com.oddlabs.tt.font.Font title_font = com.oddlabs.tt.gui.Skin.getSkin().getButtonFont();
                int padding_x = 12;
                int padding_y = 10;
                com.oddlabs.tt.gui.Label label = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.ModeAndPresetsPanel.i18n("save_card"), title_font, width - 2 * padding_x);
                int height = padding_y + label.getHeight() + padding_y;
                setDim(width, height);
                label.setPos(padding_x, padding_y);
                addChild(label);
                setCanFocus(true);
                addMouseClickListener(listener);
            }
        }
        // Java: 198
        public class PresetBanner : com.oddlabs.tt.gui.Group
        {
            public com.oddlabs.tt.gui.Label preset_label;
            public com.oddlabs.tt.gui.HorizButton reset_button;
            public com.oddlabs.tt.gui.HorizButton update_button;
            public com.oddlabs.tt.gui.Label modified_label;
            // Java: 227
            public void setState(com.oddlabs.matchmaking.Preset preset, bool modified)
            {
                preset_label.setText(com.oddlabs.tt.form.ModeAndPresetsPanel.i18n("banner_preset", (preset != null ? (object)(preset.getName()) : (object)(com.oddlabs.tt.form.ModeAndPresetsPanel.i18n("banner_no_preset")))));
                bool preset_dirty = preset != null && modified;
                reset_button.setDisabled(!preset_dirty);
                update_button.setDisabled(!preset_dirty);
                modified_label.setText((modified ? com.oddlabs.tt.form.ModeAndPresetsPanel.i18n("banner_modified") : ""));
            }
            public PresetBanner(com.oddlabs.tt.guievent.MouseClickListener reset_listener, com.oddlabs.tt.guievent.MouseClickListener update_listener) : base()
            {
                com.oddlabs.tt.font.Font font = com.oddlabs.tt.gui.Skin.getSkin().getEditFont();
                int label_width = 180;
                preset_label = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.ModeAndPresetsPanel.i18n("banner_preset", com.oddlabs.tt.form.ModeAndPresetsPanel.i18n("banner_no_preset")), font, label_width);
                reset_button = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.form.ModeAndPresetsPanel.i18n("reset_button"), com.oddlabs.tt.form.ModeAndPresetsPanel.BANNER_BUTTON_WIDTH);
                reset_button.addMouseClickListener(reset_listener);
                reset_button.setDisabled(true);
                update_button = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.form.ModeAndPresetsPanel.i18n("update_button"), com.oddlabs.tt.form.ModeAndPresetsPanel.BANNER_BUTTON_WIDTH);
                update_button.addMouseClickListener(update_listener);
                update_button.setDisabled(true);
                modified_label = new com.oddlabs.tt.gui.Label("", font, 80);
                addChild(preset_label);
                addChild(reset_button);
                addChild(update_button);
                addChild(modified_label);
                preset_label.place();
                reset_button.place(preset_label, com.oddlabs.tt.gui.Placement.RIGHT_MID);
                update_button.place(reset_button, com.oddlabs.tt.gui.Placement.RIGHT_MID);
                modified_label.place(update_button, com.oddlabs.tt.gui.Placement.RIGHT_MID);
                compileCanvas();
            }
        }
        static ModeAndPresetsPanel()
        {
            ModeAndPresetsPanel.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.form.ModeAndPresetsPanel).getName());
        }
        private void init_ModeAndPresetsPanel()
        {
            this.current_mode = com.oddlabs.matchmaking.GameMode.STANDARD;
        }
        public ModeAndPresetsPanel(com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.gamemode.PresetLibrary library, com.oddlabs.tt.form.ModeAndPresetsHandler handler) : base(com.oddlabs.tt.form.ModeAndPresetsPanel.i18n("caption"))
        {
            init_ModeAndPresetsPanel();
            this.library = library;
            this.handler = handler;
            mode_row = new com.oddlabs.tt.gui.Group();
            com.oddlabs.tt.gui.Label mode_label = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.ModeAndPresetsPanel.i18n("mode_label"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            com.oddlabs.tt.gui.PulldownMenu mode_menu = new com.oddlabs.tt.gui.PulldownMenu();
            mode_menu.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.form.ModeAndPresetsPanel.i18n("standard_title"), com.oddlabs.matchmaking.GameMode.STANDARD));
            mode_menu.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.form.ModeAndPresetsPanel.i18n("koth_title"), com.oddlabs.matchmaking.GameMode.KING_OF_THE_ISLAND));
            mode_menu.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.form.ModeAndPresetsPanel.i18n("kingofthehill_title"), com.oddlabs.matchmaking.GameMode.KING_OF_THE_HILL));
            mode_menu.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.form.ModeAndPresetsPanel.i18n("survival_title"), com.oddlabs.matchmaking.GameMode.SURVIVAL));
            if (com.oddlabs.tt.global.Edition.new_tides)
            {
                mode_menu.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.form.ModeAndPresetsPanel.i18n("zombies_title"), com.oddlabs.matchmaking.GameMode.ZOMBIES));
                mode_menu.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.form.ModeAndPresetsPanel.i18n("regicide_title"), com.oddlabs.matchmaking.GameMode.REGICIDE));
                mode_menu.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.form.ModeAndPresetsPanel.i18n("ctf_title"), com.oddlabs.matchmaking.GameMode.CAPTURE_THE_FLAG));
                mode_menu.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.form.ModeAndPresetsPanel.i18n("rampage_title"), com.oddlabs.matchmaking.GameMode.MAGIC_RAMPAGE));
                mode_menu.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.form.ModeAndPresetsPanel.i18n("island_wars_title"), com.oddlabs.matchmaking.GameMode.ISLAND_WARS));
            }
            com.oddlabs.tt.gui.PulldownButton mode_button = new com.oddlabs.tt.gui.PulldownButton(gui_root, mode_menu, 0, com.oddlabs.tt.form.ModeAndPresetsPanel.MODE_PULLDOWN_WIDTH);
            mode_menu.addItemChosenListener(new com.oddlabs.tt.guievent.ItemChosenListener_L((com.oddlabs.tt.gui.PulldownMenu menu, int index) =>
            {
                com.oddlabs.matchmaking.GameMode mode = ((com.oddlabs.matchmaking.GameMode)menu.getItem(index).getAttachment());
                if (mode != null)
                {
                    current_mode = mode;
                    handler.modeChosen(mode);
                    updateModeOption();
                }
            }));
            mode_row.addChild(mode_label);
            mode_row.addChild(mode_button);
            mode_label.place();
            mode_button.place(mode_label, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            mode_button_ref = mode_button;
            mode_row.compileCanvas();
            addChild(mode_row);
            mode_row.place();
            banner = new com.oddlabs.tt.form.ModeAndPresetsPanel.PresetBanner(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                handler.resetClicked();
            }), new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_5, int unused_6, int unused_7, int unused_8) =>
            {
                handler.updateClicked();
            }));
            addChild(banner);
            banner.place(mode_row, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT, com.oddlabs.tt.gui.Skin.getSkin().getFormData().sectionSpacing());
            rebuildGrid();
            compileCanvas();
        }
    }
}
