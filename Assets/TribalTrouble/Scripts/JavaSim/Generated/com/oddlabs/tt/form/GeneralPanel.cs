// Translated from GeneralPanel.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 27
    public class GeneralPanel : com.oddlabs.tt.gui.Panel
    {
        public const int SLIDER_WIDTH = 270;
        public const int MAX_VALUE = 20;
        public com.oddlabs.tt.gui.PulldownMenu pm_gamespeed;
        // Java: 224
        public virtual void chooseGamespeed(int speed)
        {
            pm_gamespeed.chooseItem(speed);
        }
        private void init_GeneralPanel()
        {
            this.pm_gamespeed = new com.oddlabs.tt.gui.PulldownMenu();
        }
        public GeneralPanel(com.oddlabs.tt.gui.GUIRoot gui_root, java.util.function.IntConsumer onGamespeedChange) : base(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("general_settings_caption"))
        {
            init_GeneralPanel();
            com.oddlabs.tt.gui.Group group_invert_camera = new com.oddlabs.tt.gui.Group();
            addChild(group_invert_camera);
            com.oddlabs.tt.gui.CheckBox cb_invert_camera = new com.oddlabs.tt.gui.CheckBox(com.oddlabs.tt.global.Settings.getSettings().invert_camera_pitch, com.oddlabs.tt.form.AbstractOptionsMenu.i18n("invert_camera_pitch"), com.oddlabs.tt.form.AbstractOptionsMenu.i18n("invert_camera_pitch_tip"));
            cb_invert_camera.addCheckBoxListener(new com.oddlabs.tt.guievent.CheckBoxListener_L((bool marked) =>
            {
                com.oddlabs.tt.global.Settings.getSettings().invert_camera_pitch = marked;
            }));
            group_invert_camera.addChild(cb_invert_camera);
            com.oddlabs.tt.gui.CheckBox cb_invert_camera_yaw = new com.oddlabs.tt.gui.CheckBox(com.oddlabs.tt.global.Settings.getSettings().invert_camera_yaw, com.oddlabs.tt.form.AbstractOptionsMenu.i18n("invert_camera_yaw"), com.oddlabs.tt.form.AbstractOptionsMenu.i18n("invert_camera_yaw_tip"));
            cb_invert_camera_yaw.addCheckBoxListener(new com.oddlabs.tt.guievent.CheckBoxListener_L((bool marked_2) =>
            {
                com.oddlabs.tt.global.Settings.getSettings().invert_camera_yaw = marked_2;
            }));
            group_invert_camera.addChild(cb_invert_camera_yaw);
            cb_invert_camera.place();
            cb_invert_camera_yaw.place(cb_invert_camera, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            group_invert_camera.compileCanvas();
            com.oddlabs.tt.gui.Group group_aggressive_units = new com.oddlabs.tt.gui.Group();
            addChild(group_aggressive_units);
            com.oddlabs.tt.gui.CheckBox cb_aggressive_units = new com.oddlabs.tt.gui.CheckBox(com.oddlabs.tt.global.Settings.getSettings().aggressive_units, com.oddlabs.tt.form.AbstractOptionsMenu.i18n("aggressive_units"), com.oddlabs.tt.form.AbstractOptionsMenu.i18n("aggressive_units_tip", "Ctrl-A"));
            cb_aggressive_units.addCheckBoxListener(new com.oddlabs.tt.guievent.CheckBoxListener_L((bool marked_3) =>
            {
                com.oddlabs.tt.global.Settings.getSettings().aggressive_units = marked_3;
            }));
            group_aggressive_units.addChild(cb_aggressive_units);
            cb_aggressive_units.place();
            group_aggressive_units.compileCanvas();
            com.oddlabs.tt.gui.Group group_mapmode = new com.oddlabs.tt.gui.Group();
            addChild(group_mapmode);
            com.oddlabs.tt.gui.Label label_mapmode_headline = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("map_mode_delay"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_mapmode.addChild(label_mapmode_headline);
            com.oddlabs.tt.gui.Label label_mapmode_none = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("delay_none"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_mapmode.addChild(label_mapmode_none);
            com.oddlabs.tt.gui.Label label_mapmode_high = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("delay_high"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_mapmode.addChild(label_mapmode_high);
            com.oddlabs.tt.gui.Slider slider_mapmode = new com.oddlabs.tt.gui.Slider(com.oddlabs.tt.form.GeneralPanel.SLIDER_WIDTH, 0, com.oddlabs.tt.form.GeneralPanel.MAX_VALUE, java.lang.JC.f2i(((float)(com.oddlabs.tt.global.Settings.getSettings().mapmode_delay * com.oddlabs.tt.form.GeneralPanel.MAX_VALUE))));
            group_mapmode.addChild(slider_mapmode);
            slider_mapmode.addValueListener(new com.oddlabs.tt.guievent.ValueListener_L((long @value) =>
            {
                com.oddlabs.tt.global.Settings.getSettings().mapmode_delay = (float)(((float)(@value)) / (com.oddlabs.tt.form.GeneralPanel.MAX_VALUE));
            }));
            label_mapmode_headline.place();
            label_mapmode_none.place(label_mapmode_headline, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            slider_mapmode.place(label_mapmode_none, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            label_mapmode_high.place(slider_mapmode, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            group_mapmode.compileCanvas();
            com.oddlabs.tt.gui.Group group_tooltip = new com.oddlabs.tt.gui.Group();
            addChild(group_tooltip);
            com.oddlabs.tt.gui.Label label_tooltip_headline = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("tool_tip_delay"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_tooltip.addChild(label_tooltip_headline);
            com.oddlabs.tt.gui.Label label_tooltip_none = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("delay_none"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_tooltip.addChild(label_tooltip_none);
            com.oddlabs.tt.gui.Label label_tooltip_high = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("delay_high"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_tooltip.addChild(label_tooltip_high);
            com.oddlabs.tt.gui.Slider slider_tooltip = new com.oddlabs.tt.gui.Slider(com.oddlabs.tt.form.GeneralPanel.SLIDER_WIDTH, 0, com.oddlabs.tt.form.GeneralPanel.MAX_VALUE, java.lang.JC.f2i(((float)(com.oddlabs.tt.global.Settings.getSettings().tooltip_delay * com.oddlabs.tt.form.GeneralPanel.MAX_VALUE))));
            group_tooltip.addChild(slider_tooltip);
            slider_tooltip.addValueListener(new com.oddlabs.tt.guievent.ValueListener_L((long value_2) =>
            {
                com.oddlabs.tt.global.Settings.getSettings().tooltip_delay = (float)(((float)(value_2)) / (com.oddlabs.tt.form.GeneralPanel.MAX_VALUE));
                gui_root.setToolTipTimer();
            }));
            label_tooltip_headline.place();
            label_tooltip_none.place(label_tooltip_headline, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            slider_tooltip.place(label_tooltip_none, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            label_tooltip_high.place(slider_tooltip, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            group_tooltip.compileCanvas();
            com.oddlabs.tt.gui.Group group_gamespeed = new com.oddlabs.tt.gui.Group();
            addChild(group_gamespeed);
            com.oddlabs.tt.gui.Label label_gamespeed = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("gamespeed"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_gamespeed.addChild(label_gamespeed);
            pm_gamespeed.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.util.ServerMessageBundler.getGamespeedString(com.oddlabs.matchmaking.Game.GAMESPEED_PAUSE)));
            pm_gamespeed.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.util.ServerMessageBundler.getGamespeedString(com.oddlabs.matchmaking.Game.GAMESPEED_SLOW)));
            pm_gamespeed.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.util.ServerMessageBundler.getGamespeedString(com.oddlabs.matchmaking.Game.GAMESPEED_NORMAL)));
            pm_gamespeed.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.util.ServerMessageBundler.getGamespeedString(com.oddlabs.matchmaking.Game.GAMESPEED_FAST)));
            pm_gamespeed.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.util.ServerMessageBundler.getGamespeedString(com.oddlabs.matchmaking.Game.GAMESPEED_LUDICROUS)));
            com.oddlabs.tt.gui.PulldownButton pb_gamespeed = new com.oddlabs.tt.gui.PulldownButton(gui_root, pm_gamespeed, 150);
            pm_gamespeed.addItemChosenListener(new com.oddlabs.tt.guievent.ItemChosenListener_L((com.oddlabs.tt.gui.PulldownMenu unused, int item_index) =>
            {
                onGamespeedChange.accept(item_index);
            }));
            group_gamespeed.addChild(pb_gamespeed);
            label_gamespeed.place();
            pb_gamespeed.place(label_gamespeed, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            group_gamespeed.compileCanvas();
            com.oddlabs.tt.gui.Group group_show_compass = new com.oddlabs.tt.gui.Group();
            addChild(group_show_compass);
            com.oddlabs.tt.gui.CheckBox cb_show_compass = new com.oddlabs.tt.gui.CheckBox(com.oddlabs.tt.global.Settings.getSettings().show_compass, com.oddlabs.tt.form.AbstractOptionsMenu.i18n("show_compass"), com.oddlabs.tt.form.AbstractOptionsMenu.i18n("show_compass_tip"));
            cb_show_compass.addCheckBoxListener(new com.oddlabs.tt.guievent.CheckBoxListener_L((bool marked_4) =>
            {
                com.oddlabs.tt.global.Settings.getSettings().show_compass = marked_4;
            }));
            group_show_compass.addChild(cb_show_compass);
            cb_show_compass.place();
            group_show_compass.compileCanvas();
            com.oddlabs.tt.gui.Group group_health_halos = new com.oddlabs.tt.gui.Group();
            addChild(group_health_halos);
            com.oddlabs.tt.gui.CheckBox cb_health_halos = new com.oddlabs.tt.gui.CheckBox(com.oddlabs.tt.global.Settings.getSettings().health_halos_enabled, com.oddlabs.tt.form.AbstractOptionsMenu.i18n("health_halos_enabled"), com.oddlabs.tt.form.AbstractOptionsMenu.i18n("health_halos_enabled_tip"));
            cb_health_halos.addCheckBoxListener(new com.oddlabs.tt.guievent.CheckBoxListener_L((bool marked_5) =>
            {
                com.oddlabs.tt.global.Settings.getSettings().health_halos_enabled = marked_5;
            }));
            group_health_halos.addChild(cb_health_halos);
            cb_health_halos.place();
            group_health_halos.compileCanvas();
            com.oddlabs.tt.gui.Group group_domain = new com.oddlabs.tt.gui.Group();
            addChild(group_domain);
            com.oddlabs.tt.gui.Label label_domain = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("multiplayer_domain"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_domain.addChild(label_domain);
            com.oddlabs.tt.gui.Label label_domain_updated = new com.oddlabs.tt.gui.Label("", com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), 100);
            group_domain.addChild(label_domain_updated);
            com.oddlabs.tt.gui.EditLine editline_domain = new com.oddlabs.tt.gui.EditLine(200, 250);
            editline_domain.set(com.oddlabs.tt.global.Settings.getSettings().getDomainName());
            group_domain.addChild(editline_domain);
            com.oddlabs.tt.gui.HorizButton btn_update_domain = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("domain_update"), 130);
            btn_update_domain.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_2, int unused_3, int unused_4, int unused_5) =>
            {
                string domain = editline_domain.getContents();
                if (!domain.isEmpty())
                {
                    com.oddlabs.tt.global.Settings.getSettings().setDomain(domain);
                    label_domain_updated.set(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("domain_updated"));
                }
            }));
            group_domain.addChild(btn_update_domain);
            com.oddlabs.tt.gui.HorizButton btn_reset_domain = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("domain_reset"), 130);
            btn_reset_domain.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_6, int unused_7, int unused_8, int unused_9) =>
            {
                editline_domain.set(com.oddlabs.tt.global.Settings.OFFICIAL_DOMAIN);
                com.oddlabs.tt.global.Settings.getSettings().setDomain(com.oddlabs.tt.global.Settings.OFFICIAL_DOMAIN);
                label_domain_updated.set("");
            }));
            group_domain.addChild(btn_reset_domain);
            label_domain.place();
            editline_domain.place(label_domain, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            label_domain_updated.place(editline_domain, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            btn_update_domain.place(editline_domain, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            btn_reset_domain.place(btn_update_domain, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            group_domain.compileCanvas();
            com.oddlabs.tt.gui.Group group_adaptive_ai = new com.oddlabs.tt.gui.Group();
            addChild(group_adaptive_ai);
            com.oddlabs.tt.gui.Label label_adaptive_ai = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("adaptive_ai_reset"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_adaptive_ai.addChild(label_adaptive_ai);
            com.oddlabs.tt.gui.Label label_adaptive_ai_done = new com.oddlabs.tt.gui.Label("", com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), 150);
            group_adaptive_ai.addChild(label_adaptive_ai_done);
            com.oddlabs.tt.gui.HorizButton btn_reset_adaptive_ai = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("adaptive_ai_reset_button"), 130);
            btn_reset_adaptive_ai.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_10, int unused_11, int unused_12, int unused_13) =>
            {
                com.oddlabs.tt.player.AdaptiveAIProfile.get().reset();
                com.oddlabs.tt.player.PlayerBehaviorProfile.get().reset();
                label_adaptive_ai_done.set(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("adaptive_ai_reset_done"));
            }));
            group_adaptive_ai.addChild(btn_reset_adaptive_ai);
            label_adaptive_ai.place();
            btn_reset_adaptive_ai.place(label_adaptive_ai, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            label_adaptive_ai_done.place(btn_reset_adaptive_ai, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            group_adaptive_ai.compileCanvas();
            group_gamespeed.place();
            group_mapmode.place(group_gamespeed, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            group_tooltip.place(group_mapmode, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            group_invert_camera.place(group_tooltip, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            group_aggressive_units.place(group_invert_camera, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            group_show_compass.place(group_aggressive_units, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            group_health_halos.place(group_show_compass, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            group_domain.place(group_health_halos, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            group_adaptive_ai.place(group_domain, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            compileCanvas();
        }
    }
}
