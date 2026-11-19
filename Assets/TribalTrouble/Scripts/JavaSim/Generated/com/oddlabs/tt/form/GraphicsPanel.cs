// Translated from GraphicsPanel.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 30
    public class GraphicsPanel : com.oddlabs.tt.gui.Panel
    {
        public com.oddlabs.tt.gui.Label label_pct;
        // Java: 200
        public virtual void updateScaleLabel()
        {
            int w = com.oddlabs.tt.render.Renderer.getRenderer().getWindow().getWidth();
            int h = com.oddlabs.tt.render.Renderer.getRenderer().getWindow().getHeight();
            float scale = com.oddlabs.tt.gui.GUIRoot.calculateEffectiveScale(w, h);
            label_pct.setText(java.lang.JString.format("%d%%", java.lang.JC.f2i(((float)(scale * 100)))));
        }
        public GraphicsPanel(com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.gui.Form options) : base(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("graphics_caption"))
        {
            com.oddlabs.tt.font.Font labelFont = com.oddlabs.tt.gui.Skin.getSkin().getEditFont();
            com.oddlabs.tt.gui.Group group_fullscreen = null;
            com.oddlabs.tt.gui.CheckBox cb_fullscreen = null;
            com.oddlabs.tt.gui.Group group_confine_cursor = null;
            if (!com.oddlabs.tt.util.OsPlatform.IS_MAC)
            {
                group_fullscreen = new com.oddlabs.tt.gui.Group();
                addChild(group_fullscreen);
                cb_fullscreen = new com.oddlabs.tt.gui.CheckBox(com.oddlabs.tt.global.Settings.getSettings().fullscreen, com.oddlabs.tt.form.AbstractOptionsMenu.i18n("fullscreen"), com.oddlabs.tt.form.AbstractOptionsMenu.i18n("fullscreen_tip"));
                cb_fullscreen.addCheckBoxListener(new com.oddlabs.tt.guievent.CheckBoxListener_L((bool marked) =>
                {
                    com.oddlabs.tt.form.DisplayChangeForm display_change_form = new com.oddlabs.tt.form.DisplayChangeForm(new com.oddlabs.tt.gui.DoNowListener_L((bool switch_now) =>
                    {
                        if (switch_now)
                        {
                            com.oddlabs.tt.render.Renderer.getRenderer().toggleFullscreen();
                        }
                        else
                        {
                            com.oddlabs.tt.global.Settings.getSettings().fullscreen = marked;
                        }
                    }));
                    gui_root.addModalForm(display_change_form);
                }));
                group_fullscreen.addChild(cb_fullscreen);
                cb_fullscreen.place();
                group_fullscreen.compileCanvas();
                group_confine_cursor = new com.oddlabs.tt.gui.Group();
                addChild(group_confine_cursor);
                com.oddlabs.tt.gui.CheckBox cb_confine_cursor = new com.oddlabs.tt.gui.CheckBox(com.oddlabs.tt.global.Settings.getSettings().confine_cursor, com.oddlabs.tt.form.AbstractOptionsMenu.i18n("confine_cursor"), com.oddlabs.tt.form.AbstractOptionsMenu.i18n("confine_cursor_tip"));
                cb_confine_cursor.addCheckBoxListener(new com.oddlabs.tt.guievent.CheckBoxListener_L((bool marked_2) =>
                {
                    com.oddlabs.tt.global.Settings.getSettings().confine_cursor = marked_2;
                    com.oddlabs.tt.input.InputProvider input = com.oddlabs.tt.render.Renderer.getLocalInput().getInputProvider();
                    input.setGrabbed(input.isGrabbed());
                }));
                group_confine_cursor.addChild(cb_confine_cursor);
                cb_confine_cursor.place();
                group_confine_cursor.compileCanvas();
            }
            com.oddlabs.tt.gui.Group group_ui_scale = new com.oddlabs.tt.gui.Group();
            addChild(group_ui_scale);
            com.oddlabs.tt.gui.Label label_ui_scale = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("ui_scale"), labelFont);
            group_ui_scale.addChild(label_ui_scale);
            label_pct = new com.oddlabs.tt.gui.Label("9.9.9%", labelFont);
            updateScaleLabel();
            group_ui_scale.addChild(label_pct);
            int initialValue = java.lang.Math.clamp(java.lang.JC.f2l(((float)(com.oddlabs.tt.global.Settings.getSettings().ui_scale * 1000))), 0, 1000);
            com.oddlabs.tt.gui.Slider slider_ui_scale = new com.oddlabs.tt.gui.Slider(150, 0, 1000, initialValue);
            group_ui_scale.addChild(slider_ui_scale);
            slider_ui_scale.addValueListener(new com.oddlabs.tt.guievent.ValueListener_L((long @value) =>
            {
                com.oddlabs.tt.global.Settings.getSettings().ui_scale = (float)(@value / 1000.0f);
                updateScaleLabel();
            }));
            slider_ui_scale.addReleaseListener(new java.lang.Runnable_L(() =>
            {
                gui_root.displayChanged(com.oddlabs.tt.render.Renderer.getRenderer().getWindow().getWidth(), com.oddlabs.tt.render.Renderer.getRenderer().getWindow().getHeight());
            }));
            label_ui_scale.place();
            label_pct.place(label_ui_scale, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            slider_ui_scale.place(label_ui_scale, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            group_ui_scale.compileCanvas();
            com.oddlabs.tt.gui.Group group_detail = new com.oddlabs.tt.gui.Group();
            addChild(group_detail);
            com.oddlabs.tt.gui.Label label_detail = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("graphical_detail"), labelFont);
            group_detail.addChild(label_detail);
            int initial_detail_value = com.oddlabs.tt.global.Settings.getSettings().graphic_detail;
            com.oddlabs.tt.gui.PulldownMenu pm_detail = new com.oddlabs.tt.gui.PulldownMenu();
            pm_detail.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("low")));
            pm_detail.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("medium")));
            pm_detail.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("high")));
            com.oddlabs.tt.gui.PulldownButton pb_detail = new com.oddlabs.tt.gui.PulldownButton(gui_root, pm_detail, initial_detail_value, 150);
            group_detail.addChild(pb_detail);
            options.addCloseListener(new com.oddlabs.tt.guievent.CloseListener_L(() =>
            {
                int slider_value = pm_detail.getChosenItemIndex();
                if (initial_detail_value != slider_value)
                {
                    com.oddlabs.tt.global.Settings.getSettings().graphic_detail = slider_value;
                    gui_root.addModalForm(new com.oddlabs.tt.form.MessageForm(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("change_next_run")));
                }
            }));
            label_detail.place();
            pb_detail.place(label_detail, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            group_detail.compileCanvas();
            com.oddlabs.tt.gui.Group mode_group = new com.oddlabs.tt.gui.Group();
            addChild(mode_group);
            com.oddlabs.tt.gui.Label mode_label = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("display_mode"), labelFont);
            mode_group.addChild(mode_label);
            com.oddlabs.tt.gui.ColumnInfo[] mode_infos = new com.oddlabs.tt.gui.ColumnInfo[] { new com.oddlabs.tt.gui.ColumnInfo("", 150) };
            com.oddlabs.tt.gui.MultiColumnComboBox mode_list_box = new com.oddlabs.tt.gui.MultiColumnComboBox(gui_root, mode_infos, 200, false);
            com.oddlabs.tt.render.SerializableDisplayMode[] modes = com.oddlabs.tt.render.Renderer.getRenderer().getWindow().getAvailableDisplayModes();
            com.oddlabs.tt.render.SerializableDisplayMode current_mode = com.oddlabs.tt.render.Renderer.getRenderer().getCurrentDisplayMode();
            com.oddlabs.tt.gui.Row current_row = null;
            for (int i = 0; i < modes.Length; i++)
            {
                if (modes[i].getBitsPerPixel() == current_mode.getBitsPerPixel())
                {
                    string mode_string = com.oddlabs.tt.form.AbstractOptionsMenu.i18n("mode", java.lang.Integer.toString(modes[i].getWidth()), java.lang.Integer.toString(modes[i].getHeight()), java.lang.Integer.toString(modes[i].getFrequency()));
                    com.oddlabs.tt.gui.Label label = new com.oddlabs.tt.gui.SortedLabel(mode_string, i, com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font());
                    com.oddlabs.tt.gui.Row row = new com.oddlabs.tt.gui.Row(new com.oddlabs.tt.gui.Label[] { label }, modes[i]);
                    mode_list_box.addRow(row);
                    if (java.lang.JO.equals(modes[i], current_mode))
                    {
                        current_row = row;
                    }
                }
            }
            if (current_row != null)
            {
                mode_list_box.selectRow(current_row);
            }
            mode_list_box.addRowListener(new GraphicsPanel.Anon1_RowListener(this, gui_root));
            mode_group.addChild(mode_list_box);
            mode_label.place();
            mode_list_box.place(mode_label, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            mode_group.compileCanvas();
            if (cb_fullscreen != null)
            {
                mode_group.setDisabled(com.oddlabs.tt.global.Settings.getSettings().fullscreen);
                cb_fullscreen.addCheckBoxListener(new com.oddlabs.tt.guievent.CheckBoxListener_L((bool marked_3) =>
                {
                    mode_group.setDisabled(marked_3);
                }));
            }
            mode_group.place();
            group_detail.place(mode_group, com.oddlabs.tt.gui.Placement.RIGHT_TOP);
            group_ui_scale.place(group_detail, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            if (group_fullscreen != null)
            {
                group_fullscreen.place(group_ui_scale, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            }
            if (group_confine_cursor != null && group_fullscreen != null)
            {
                group_confine_cursor.place(group_fullscreen, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            }
            compileCanvas();
        }
        // Java: anonymous class at line 162
        public sealed class Anon1_RowListener : java.lang.Object, com.oddlabs.tt.guievent.RowListener
        {
            public com.oddlabs.tt.form.GraphicsPanel this_0;
            public com.oddlabs.tt.gui.GUIRoot gui_root;
            public Anon1_RowListener(com.oddlabs.tt.form.GraphicsPanel this_0_, com.oddlabs.tt.gui.GUIRoot gui_root_c)
            {
                this.this_0 = this_0_;
                this.gui_root = gui_root_c;
            }
            // Java: 163
            public void rowDoubleClicked(object mode_p)
            {
                com.oddlabs.tt.render.SerializableDisplayMode mode = (com.oddlabs.tt.render.SerializableDisplayMode)mode_p;
                if (!com.oddlabs.tt.global.Settings.getSettings().fullscreen)
                {
                    gui_root.addModalForm(new com.oddlabs.tt.form.DisplayChangeForm(new com.oddlabs.tt.gui.DoNowListener_L((bool switch_now_2) =>
                    {
                        com.oddlabs.tt.render.Renderer.getRenderer().switchMode(mode, switch_now_2);
                        if (switch_now_2)
                        {
                            gui_root.displayChanged(mode.getWidth(), mode.getHeight());
                        }
                    })));
                }
            }
            // mixin of default method RowListener.rowChosen
            public void rowChosen(object row_context)
            {
            }
        }
    }
}
