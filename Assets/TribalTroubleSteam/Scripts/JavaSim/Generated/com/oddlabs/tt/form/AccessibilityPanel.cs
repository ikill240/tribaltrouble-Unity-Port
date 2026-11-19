// Translated from AccessibilityPanel.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 26
    public class AccessibilityPanel : com.oddlabs.tt.gui.Panel
    {
        public const int SLIDER_WIDTH = 270;
        public const int MAX_VALUE = 20;
        public AccessibilityPanel(com.oddlabs.tt.gui.GUIRoot gui_root) : base(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("accessibility_caption"))
        {
            com.oddlabs.tt.gui.Group group_contrast = new com.oddlabs.tt.gui.Group();
            addChild(group_contrast);
            com.oddlabs.tt.gui.CheckBox cb_high_contrast = new com.oddlabs.tt.gui.CheckBox(com.oddlabs.tt.global.Settings.getSettings().high_contrast, com.oddlabs.tt.form.AbstractOptionsMenu.i18n("high_contrast"), com.oddlabs.tt.form.AbstractOptionsMenu.i18n("high_contrast_tip"));
            group_contrast.addChild(cb_high_contrast);
            com.oddlabs.tt.gui.Label label_contrast_intensity = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("contrast_intensity"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_contrast.addChild(label_contrast_intensity);
            com.oddlabs.tt.gui.Label label_contrast_low = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("low"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_contrast.addChild(label_contrast_low);
            com.oddlabs.tt.gui.Label label_contrast_high = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("high"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_contrast.addChild(label_contrast_high);
            com.oddlabs.tt.gui.Slider slider_contrast = new com.oddlabs.tt.gui.Slider(com.oddlabs.tt.form.AccessibilityPanel.SLIDER_WIDTH, 0, 2 * com.oddlabs.tt.form.AccessibilityPanel.MAX_VALUE, java.lang.JC.f2i(((float)(com.oddlabs.tt.global.Settings.getSettings().contrast_intensity * com.oddlabs.tt.form.AccessibilityPanel.MAX_VALUE))));
            slider_contrast.setDisabled(!com.oddlabs.tt.global.Settings.getSettings().high_contrast);
            group_contrast.addChild(slider_contrast);
            cb_high_contrast.addCheckBoxListener(new com.oddlabs.tt.guievent.CheckBoxListener_L((bool marked) =>
            {
                com.oddlabs.tt.global.Settings.getSettings().high_contrast = marked;
                slider_contrast.setDisabled(!marked);
            }));
            slider_contrast.addValueListener(new com.oddlabs.tt.guievent.ValueListener_L((long @value) =>
            {
                com.oddlabs.tt.global.Settings.getSettings().contrast_intensity = (float)(((float)(@value)) / com.oddlabs.tt.form.AccessibilityPanel.MAX_VALUE);
            }));
            cb_high_contrast.place();
            label_contrast_intensity.place(cb_high_contrast, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            label_contrast_low.place(label_contrast_intensity, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            slider_contrast.place(label_contrast_low, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            label_contrast_high.place(slider_contrast, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            group_contrast.compileCanvas();
            com.oddlabs.tt.gui.Group group_cvd = new com.oddlabs.tt.gui.Group();
            addChild(group_cvd);
            com.oddlabs.tt.gui.Label label_cvd = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("colour_vision"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_cvd.addChild(label_cvd);
            com.oddlabs.tt.gui.PulldownMenu pm_cvd = new com.oddlabs.tt.gui.PulldownMenu();
            pm_cvd.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("cvd_standard")));
            pm_cvd.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("cvd_protanopia")));
            pm_cvd.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("cvd_deuteranopia")));
            pm_cvd.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("cvd_tritanopia")));
            com.oddlabs.tt.gui.PulldownButton pb_cvd = new com.oddlabs.tt.gui.PulldownButton(gui_root, pm_cvd, com.oddlabs.tt.global.Settings.getSettings().cvd_mode, com.oddlabs.tt.form.AccessibilityPanel.SLIDER_WIDTH - label_cvd.getWidth() - com.oddlabs.tt.gui.Skin.getSkin().getFormData().objectSpacing());
            group_cvd.addChild(pb_cvd);
            com.oddlabs.tt.gui.Label label_cvd_intensity = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("cvd_intensity"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_cvd.addChild(label_cvd_intensity);
            com.oddlabs.tt.gui.Label label_cvd_low = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("low"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_cvd.addChild(label_cvd_low);
            com.oddlabs.tt.gui.Label label_cvd_high = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("high"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_cvd.addChild(label_cvd_high);
            com.oddlabs.tt.gui.Slider slider_cvd = new com.oddlabs.tt.gui.Slider(com.oddlabs.tt.form.AccessibilityPanel.SLIDER_WIDTH, 0, 2 * com.oddlabs.tt.form.AccessibilityPanel.MAX_VALUE, java.lang.JC.f2i(((float)(com.oddlabs.tt.global.Settings.getSettings().cvd_intensity * com.oddlabs.tt.form.AccessibilityPanel.MAX_VALUE))));
            slider_cvd.setDisabled(com.oddlabs.tt.global.Settings.getSettings().cvd_mode == 0);
            group_cvd.addChild(slider_cvd);
            pm_cvd.addItemChosenListener(new com.oddlabs.tt.guievent.ItemChosenListener_L((com.oddlabs.tt.gui.PulldownMenu unused, int index) =>
            {
                com.oddlabs.tt.global.Settings.getSettings().cvd_mode = index;
                slider_cvd.setDisabled(index == 0);
            }));
            slider_cvd.addValueListener(new com.oddlabs.tt.guievent.ValueListener_L((long value_2) =>
            {
                com.oddlabs.tt.global.Settings.getSettings().cvd_intensity = (float)(((float)(value_2)) / com.oddlabs.tt.form.AccessibilityPanel.MAX_VALUE);
            }));
            label_cvd.place();
            pb_cvd.place(label_cvd, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            label_cvd_intensity.place(label_cvd, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            label_cvd_low.place(label_cvd_intensity, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            slider_cvd.place(label_cvd_low, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            label_cvd_high.place(slider_cvd, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            group_cvd.compileCanvas();
            com.oddlabs.tt.gui.Group group_team_colours = new com.oddlabs.tt.gui.Group();
            addChild(group_team_colours);
            com.oddlabs.tt.gui.Label label_team_colours = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("team_colours"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_team_colours.addChild(label_team_colours);
            com.oddlabs.tt.gui.ScrollablePulldownMenu pm_team = new com.oddlabs.tt.gui.ScrollablePulldownMenu(6);
            for (int i = 0; i < com.oddlabs.tt.global.Settings.getSettings().team_colours.Length; i++)
            {
                string player_str = com.oddlabs.tt.form.AbstractOptionsMenu.i18n("player", java.lang.Integer.toString(i + 1));
                com.oddlabs.tt.gui.PulldownItem item = new com.oddlabs.tt.gui.PulldownItem(player_str);
                item.setLabelColor(com.oddlabs.tt.global.Settings.getSettings().team_colours[i]);
                pm_team.addItem(item);
            }
            com.oddlabs.tt.gui.PulldownButton pb_team = new com.oddlabs.tt.gui.PulldownButton(gui_root, pm_team, 0, 150);
            group_team_colours.addChild(pb_team);
            AccessibilityPanel.ColorBox_Local1 colorBox = new AccessibilityPanel.ColorBox_Local1(this);
            group_team_colours.addChild(colorBox);
            com.oddlabs.tt.gui.Slider slider_hue = new com.oddlabs.tt.gui.Slider(com.oddlabs.tt.form.AccessibilityPanel.SLIDER_WIDTH, 0, 360, 0);
            group_team_colours.addChild(slider_hue);
            com.oddlabs.tt.gui.HorizButton button_reset = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("reset"), 100);
            group_team_colours.addChild(button_reset);
            java.lang.Runnable updateColor = new java.lang.Runnable_L(() =>
            {
                int teamIndex = pm_team.getChosenItemIndex();
                float hue = slider_hue.getValue();
                int rgb = java.awt.Color.HSBtoRGB((float)(hue / 360.0f), 1.0f, 1.0f);
                org.joml.Vector4f newColor = com.oddlabs.util.Color.argb4v((255 << 24) | (rgb & 16777215));
                com.oddlabs.tt.global.Settings.getSettings().team_colours[teamIndex] = newColor;
                colorBox.setColor(newColor);
                pm_team.getItem(teamIndex).setLabelColor(newColor);
                pb_team.setLabelColor(newColor);
            });
            java.lang.Runnable refreshUI = new java.lang.Runnable_L(() =>
            {
                int index_2 = pm_team.getChosenItemIndex();
                org.joml.Vector4fc currentColor = com.oddlabs.tt.global.Settings.getSettings().team_colours[index_2];
                float[] hsb = java.awt.Color.RGBtoHSB(java.lang.JC.f2i(((float)(currentColor.x() * 255))), java.lang.JC.f2i(((float)(currentColor.y() * 255))), java.lang.JC.f2i(((float)(currentColor.z() * 255))), null);
                slider_hue.setValue(java.lang.JC.f2i(((float)(hsb[0] * 360))));
                colorBox.setColor(currentColor);
            });
            pm_team.addItemChosenListener(new com.oddlabs.tt.guievent.ItemChosenListener_L((com.oddlabs.tt.gui.PulldownMenu unused_2, int unused_3) =>
            {
                refreshUI.run();
            }));
            refreshUI.run();
            slider_hue.addValueListener(new com.oddlabs.tt.guievent.ValueListener_L((long unused_4) =>
            {
                updateColor.run();
            }));
            button_reset.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_5, int unused_6, int unused_7, int unused_8) =>
            {
                int index_3 = pm_team.getChosenItemIndex();
                com.oddlabs.tt.global.Settings.getSettings().team_colours[index_3] = new org.joml.Vector4f(com.oddlabs.tt.global.Settings.DEFAULT_TEAM_COLOURS[index_3]);
                refreshUI.run();
                pm_team.getItem(index_3).setLabelColor(com.oddlabs.tt.global.Settings.getSettings().team_colours[index_3]);
                pb_team.setLabelColor(com.oddlabs.tt.global.Settings.getSettings().team_colours[index_3]);
            }));
            com.oddlabs.tt.gui.CheckBox cb_team_stencil = new com.oddlabs.tt.gui.CheckBox(com.oddlabs.tt.global.Settings.getSettings().team_stencil, com.oddlabs.tt.form.AbstractOptionsMenu.i18n("team_stencil"), com.oddlabs.tt.form.AbstractOptionsMenu.i18n("team_stencil_tip"));
            cb_team_stencil.addCheckBoxListener(new com.oddlabs.tt.guievent.CheckBoxListener_L((bool marked_2) =>
            {
                com.oddlabs.tt.global.Settings.getSettings().team_stencil = marked_2;
            }));
            group_team_colours.addChild(cb_team_stencil);
            label_team_colours.place();
            pb_team.place(label_team_colours, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            colorBox.place(pb_team, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            button_reset.place(colorBox, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            slider_hue.place(label_team_colours, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            cb_team_stencil.place(slider_hue, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            group_team_colours.compileCanvas();
            group_contrast.place();
            group_cvd.place(group_contrast, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            group_team_colours.place(group_cvd, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            compileCanvas();
        }
        // Java: 132
        public new class ColorBox_Local1 : com.oddlabs.tt.gui.GUIObject
        {
            public com.oddlabs.tt.form.AccessibilityPanel this_0;
            public org.joml.Vector4f color;
            // Java: 139
            public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
            {
                renderer.drawColoredQuad(0, 0, getWidth(), getHeight(), color);
            }
            // Java: 144
            public virtual void setColor(org.joml.Vector4fc c)
            {
                this.color.set(c);
            }
            private void init_ColorBox_Local1()
            {
                this.color = new org.joml.Vector4f(com.oddlabs.util.Color.WHITE);
            }
            public ColorBox_Local1(com.oddlabs.tt.form.AccessibilityPanel this_0_) : base()
            {
                this.this_0 = this_0_;
                init_ColorBox_Local1();
                setDim(20, 20);
            }
        }
    }
}
