// Translated from CameraPanel.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 15
    public class CameraPanel : com.oddlabs.tt.gui.Panel
    {
        public const int SLIDER_WIDTH = 270;
        public const int MAX_VALUE = 20;
        // Java: 95
        public com.oddlabs.tt.gui.Group speedGroup(string headline_key, com.oddlabs.tt.gui.Slider slider)
        {
            com.oddlabs.tt.gui.Group group = new com.oddlabs.tt.gui.Group();
            addChild(group);
            com.oddlabs.tt.gui.Label label_headline = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.AbstractOptionsMenu.i18n(headline_key), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group.addChild(label_headline);
            com.oddlabs.tt.gui.Label label_slow = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("speed_slow"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group.addChild(label_slow);
            com.oddlabs.tt.gui.Label label_fast = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("speed_fast"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group.addChild(label_fast);
            group.addChild(slider);
            label_headline.place();
            label_slow.place(label_headline, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            slider.place(label_slow, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            label_fast.place(slider, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            group.compileCanvas();
            return group;
        }
        public CameraPanel() : base(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("camera_caption"))
        {
            com.oddlabs.tt.gui.Slider slider_pan = new com.oddlabs.tt.gui.Slider(com.oddlabs.tt.form.CameraPanel.SLIDER_WIDTH, com.oddlabs.tt.form.CameraPanel.MAX_VALUE / 4, com.oddlabs.tt.form.CameraPanel.MAX_VALUE * 7 / 4, java.lang.Math.round((float)(com.oddlabs.tt.global.Settings.getSettings().camera_pan_speed * com.oddlabs.tt.form.CameraPanel.MAX_VALUE)));
            slider_pan.addValueListener(new com.oddlabs.tt.guievent.ValueListener_L((long @value) =>
            {
                com.oddlabs.tt.global.Settings.getSettings().camera_pan_speed = (float)(((float)(@value)) / com.oddlabs.tt.form.CameraPanel.MAX_VALUE);
            }));
            com.oddlabs.tt.gui.Group group_pan = speedGroup("camera_pan_speed", slider_pan);
            com.oddlabs.tt.gui.Group group_pan_accel = new com.oddlabs.tt.gui.Group();
            addChild(group_pan_accel);
            com.oddlabs.tt.gui.CheckBox cb_pan_accel = new com.oddlabs.tt.gui.CheckBox(com.oddlabs.tt.global.Settings.getSettings().camera_pan_acceleration, com.oddlabs.tt.form.AbstractOptionsMenu.i18n("camera_pan_acceleration"), com.oddlabs.tt.form.AbstractOptionsMenu.i18n("camera_pan_acceleration_tip"));
            cb_pan_accel.addCheckBoxListener(new com.oddlabs.tt.guievent.CheckBoxListener_L((bool marked) =>
            {
                com.oddlabs.tt.global.Settings.getSettings().camera_pan_acceleration = marked;
            }));
            group_pan_accel.addChild(cb_pan_accel);
            cb_pan_accel.place();
            group_pan_accel.compileCanvas();
            com.oddlabs.tt.gui.Slider slider_rotate = new com.oddlabs.tt.gui.Slider(com.oddlabs.tt.form.CameraPanel.SLIDER_WIDTH, com.oddlabs.tt.form.CameraPanel.MAX_VALUE / 4, com.oddlabs.tt.form.CameraPanel.MAX_VALUE * 7 / 4, java.lang.Math.round((float)(com.oddlabs.tt.global.Settings.getSettings().camera_rotate_speed * com.oddlabs.tt.form.CameraPanel.MAX_VALUE)));
            slider_rotate.addValueListener(new com.oddlabs.tt.guievent.ValueListener_L((long value_2) =>
            {
                com.oddlabs.tt.global.Settings.getSettings().camera_rotate_speed = (float)(((float)(value_2)) / com.oddlabs.tt.form.CameraPanel.MAX_VALUE);
            }));
            com.oddlabs.tt.gui.Group group_rotate = speedGroup("camera_rotate_speed", slider_rotate);
            com.oddlabs.tt.gui.Slider slider_zoom = new com.oddlabs.tt.gui.Slider(com.oddlabs.tt.form.CameraPanel.SLIDER_WIDTH, com.oddlabs.tt.form.CameraPanel.MAX_VALUE / 4, com.oddlabs.tt.form.CameraPanel.MAX_VALUE * 7 / 4, java.lang.Math.round((float)(com.oddlabs.tt.global.Settings.getSettings().camera_zoom_speed * com.oddlabs.tt.form.CameraPanel.MAX_VALUE)));
            slider_zoom.addValueListener(new com.oddlabs.tt.guievent.ValueListener_L((long value_3) =>
            {
                com.oddlabs.tt.global.Settings.getSettings().camera_zoom_speed = (float)(((float)(value_3)) / com.oddlabs.tt.form.CameraPanel.MAX_VALUE);
            }));
            com.oddlabs.tt.gui.Group group_zoom = speedGroup("camera_zoom_speed", slider_zoom);
            com.oddlabs.tt.gui.Slider slider_cinematic = new com.oddlabs.tt.gui.Slider(com.oddlabs.tt.form.CameraPanel.SLIDER_WIDTH, 1, com.oddlabs.tt.form.CameraPanel.MAX_VALUE, java.lang.Math.round((float)(com.oddlabs.tt.global.Settings.getSettings().cinematic_camera_speed * com.oddlabs.tt.form.CameraPanel.MAX_VALUE)));
            slider_cinematic.addValueListener(new com.oddlabs.tt.guievent.ValueListener_L((long value_4) =>
            {
                com.oddlabs.tt.global.Settings.getSettings().cinematic_camera_speed = (float)(((float)(value_4)) / com.oddlabs.tt.form.CameraPanel.MAX_VALUE);
            }));
            com.oddlabs.tt.gui.Group group_cinematic = speedGroup("cinematic_camera_speed", slider_cinematic);
            com.oddlabs.tt.gui.Group group_limits = new com.oddlabs.tt.gui.Group();
            addChild(group_limits);
            com.oddlabs.tt.gui.CheckBox cb_unlock_limits = new com.oddlabs.tt.gui.CheckBox(com.oddlabs.tt.global.Settings.getSettings().cinematic_unlock_limits, com.oddlabs.tt.form.AbstractOptionsMenu.i18n("cinematic_unlock_limits"), com.oddlabs.tt.form.AbstractOptionsMenu.i18n("cinematic_unlock_limits_tip"));
            cb_unlock_limits.addCheckBoxListener(new com.oddlabs.tt.guievent.CheckBoxListener_L((bool marked_2) =>
            {
                com.oddlabs.tt.global.Settings.getSettings().cinematic_unlock_limits = marked_2;
            }));
            group_limits.addChild(cb_unlock_limits);
            cb_unlock_limits.place();
            group_limits.compileCanvas();
            com.oddlabs.tt.gui.Group group_invert_camera = new com.oddlabs.tt.gui.Group();
            addChild(group_invert_camera);
            com.oddlabs.tt.gui.CheckBox cb_invert_camera = new com.oddlabs.tt.gui.CheckBox(com.oddlabs.tt.global.Settings.getSettings().invert_camera_pitch, com.oddlabs.tt.form.AbstractOptionsMenu.i18n("invert_camera_pitch"), com.oddlabs.tt.form.AbstractOptionsMenu.i18n("invert_camera_pitch_tip"));
            cb_invert_camera.addCheckBoxListener(new com.oddlabs.tt.guievent.CheckBoxListener_L((bool marked_3) =>
            {
                com.oddlabs.tt.global.Settings.getSettings().invert_camera_pitch = marked_3;
            }));
            group_invert_camera.addChild(cb_invert_camera);
            com.oddlabs.tt.gui.CheckBox cb_invert_camera_yaw = new com.oddlabs.tt.gui.CheckBox(com.oddlabs.tt.global.Settings.getSettings().invert_camera_yaw, com.oddlabs.tt.form.AbstractOptionsMenu.i18n("invert_camera_yaw"), com.oddlabs.tt.form.AbstractOptionsMenu.i18n("invert_camera_yaw_tip"));
            cb_invert_camera_yaw.addCheckBoxListener(new com.oddlabs.tt.guievent.CheckBoxListener_L((bool marked_4) =>
            {
                com.oddlabs.tt.global.Settings.getSettings().invert_camera_yaw = marked_4;
            }));
            group_invert_camera.addChild(cb_invert_camera_yaw);
            cb_invert_camera.place();
            cb_invert_camera_yaw.place(cb_invert_camera, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            group_invert_camera.compileCanvas();
            group_pan.place();
            group_pan_accel.place(group_pan, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            group_rotate.place(group_pan_accel, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            group_zoom.place(group_rotate, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            group_cinematic.place(group_zoom, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            group_limits.place(group_cinematic, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            group_invert_camera.place(group_limits, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            compileCanvas();
        }
    }
}
