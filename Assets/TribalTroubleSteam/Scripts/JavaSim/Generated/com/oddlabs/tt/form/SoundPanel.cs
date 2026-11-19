// Translated from SoundPanel.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 22
    public class SoundPanel : com.oddlabs.tt.gui.Panel
    {
        public const int SLIDER_WIDTH = 270;
        public const int MAX_VALUE = 20;
        public const bool TEMPORARILY_DISABLE_MUSIC_CONTROLS = false;
        public SoundPanel(com.oddlabs.tt.gui.GUIRoot gui_root) : base(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("sound_caption"))
        {
            com.oddlabs.tt.gui.Group group_music = new com.oddlabs.tt.gui.Group();
            addChild(group_music);
            com.oddlabs.tt.gui.Label label_music_low = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("low"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_music.addChild(label_music_low);
            com.oddlabs.tt.gui.Label label_music_high = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("high"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_music.addChild(label_music_high);
            com.oddlabs.tt.gui.CheckBox cb_music = new com.oddlabs.tt.gui.CheckBox(com.oddlabs.tt.global.Settings.getSettings().play_music, com.oddlabs.tt.form.AbstractOptionsMenu.i18n("music"));
            group_music.addChild(cb_music);
            com.oddlabs.tt.gui.Label label_music = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("music_volume"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_music.addChild(label_music);
            com.oddlabs.tt.gui.Slider slider_music = new com.oddlabs.tt.gui.Slider(com.oddlabs.tt.form.SoundPanel.SLIDER_WIDTH, 0, com.oddlabs.tt.form.SoundPanel.MAX_VALUE, java.lang.JC.f2i(((float)(com.oddlabs.tt.global.Settings.getSettings().music_gain * (com.oddlabs.tt.form.SoundPanel.MAX_VALUE)))));
            slider_music.setDisabled(com.oddlabs.tt.form.SoundPanel.TEMPORARILY_DISABLE_MUSIC_CONTROLS || !cb_music.isMarked());
            group_music.addChild(slider_music);
            cb_music.addCheckBoxListener(new com.oddlabs.tt.guievent.CheckBoxListener_L((bool marked) =>
            {
                if (com.oddlabs.tt.global.Settings.getSettings().play_music != marked)
                {
                    com.oddlabs.tt.render.Renderer.getRenderer().toggleMusic();
                }
                slider_music.setDisabled(!marked);
                com.oddlabs.tt.global.Settings.getSettings().play_music = marked;
            }));
            slider_music.addValueListener(new com.oddlabs.tt.guievent.ValueListener_L((long @value) =>
            {
                float music_gain = (float)(((float)(@value)) / (com.oddlabs.tt.form.SoundPanel.MAX_VALUE));
                com.oddlabs.tt.global.Settings.getSettings().music_gain = music_gain;
                com.oddlabs.tt.render.Renderer.getRenderer().getMusicPlayer().setGain(music_gain);
            }));
            cb_music.place();
            label_music.place(cb_music, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            label_music_low.place(label_music, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            slider_music.place(label_music_low, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            label_music_high.place(slider_music, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            group_music.compileCanvas();
            group_music.setDisabled(com.oddlabs.tt.form.SoundPanel.TEMPORARILY_DISABLE_MUSIC_CONTROLS || !com.oddlabs.tt.render.Renderer.getLocalInput().audioIsCreated());
            com.oddlabs.tt.gui.Group group_sound = new com.oddlabs.tt.gui.Group();
            addChild(group_sound);
            com.oddlabs.tt.gui.Label label_sound_low = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("low"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_sound.addChild(label_sound_low);
            com.oddlabs.tt.gui.Label label_sound_high = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("high"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_sound.addChild(label_sound_high);
            com.oddlabs.tt.gui.CheckBox cb_sound = new com.oddlabs.tt.gui.CheckBox(com.oddlabs.tt.global.Settings.getSettings().play_sfx, com.oddlabs.tt.form.AbstractOptionsMenu.i18n("sound_effects"));
            group_sound.addChild(cb_sound);
            com.oddlabs.tt.gui.Label label_sound = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("sound_effects_volume"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_sound.addChild(label_sound);
            com.oddlabs.tt.gui.Slider slider_sound = new com.oddlabs.tt.gui.Slider(com.oddlabs.tt.form.SoundPanel.SLIDER_WIDTH, 0, com.oddlabs.tt.form.SoundPanel.MAX_VALUE, java.lang.JC.f2i(((float)(com.oddlabs.tt.global.Settings.getSettings().sound_gain * (com.oddlabs.tt.form.SoundPanel.MAX_VALUE)))));
            slider_sound.setDisabled(!cb_sound.isMarked());
            group_sound.addChild(slider_sound);
            cb_sound.addCheckBoxListener(new com.oddlabs.tt.guievent.CheckBoxListener_L((bool marked_2) =>
            {
                if (com.oddlabs.tt.global.Settings.getSettings().play_sfx != marked_2)
                {
                    com.oddlabs.tt.render.Renderer.getRenderer().toggleSound();
                }
                slider_sound.setDisabled(!marked_2);
                com.oddlabs.tt.global.Settings.getSettings().play_sfx = marked_2;
            }));
            slider_sound.addValueListener(new com.oddlabs.tt.guievent.ValueListener_L((long value_2) =>
            {
                com.oddlabs.tt.global.Settings.getSettings().sound_gain = (float)(((float)(value_2)) / (com.oddlabs.tt.form.SoundPanel.MAX_VALUE));
            }));
            cb_sound.place();
            label_sound.place(cb_sound, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            label_sound_low.place(label_sound, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            slider_sound.place(label_sound_low, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            label_sound_high.place(slider_sound, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            group_sound.compileCanvas();
            bool audioCreated = com.oddlabs.tt.render.Renderer.getLocalInput().audioIsCreated();
            group_sound.setDisabled(!audioCreated);
            com.oddlabs.tt.gui.Group group_output = new com.oddlabs.tt.gui.Group();
            addChild(group_output);
            com.oddlabs.tt.gui.Label label_output = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("audio_output"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_output.addChild(label_output);
            com.oddlabs.tt.gui.PulldownMenu pm_output = new com.oddlabs.tt.gui.PulldownMenu();
            pm_output.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("audio_output_speakers")));
            pm_output.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("audio_output_headphones")));
            int initialOutput = (com.oddlabs.tt.global.Settings.getSettings().headphone_mode ? 1 : 0);
            com.oddlabs.tt.gui.PulldownButton pb_output = new com.oddlabs.tt.gui.PulldownButton(gui_root, pm_output, initialOutput, 150);
            group_output.addChild(pb_output);
            label_output.place();
            pb_output.place(label_output, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            com.oddlabs.tt.audio.AudioManager manager = null;
            if (audioCreated)
            {
                try
                {
                    manager = com.oddlabs.tt.audio.AudioManager.getManager();
                }
                catch (System.Exception e)
                {
                }
            }
            bool hrtfSupported = false;
            if (manager != null)
            {
                hrtfSupported = manager.isHRTFSupported();
            }
            if (hrtfSupported)
            {
                com.oddlabs.tt.audio.AudioManager mgr = manager;
                pm_output.addItemChosenListener(new com.oddlabs.tt.guievent.ItemChosenListener_L((com.oddlabs.tt.gui.PulldownMenu unused, int index) =>
                {
                    bool headphone = (index == 1);
                    com.oddlabs.tt.global.Settings.getSettings().headphone_mode = headphone;
                    if ((mgr is com.oddlabs.tt.audio.openal.OpenALManager alManager))
                    {
                        alManager.setHeadphoneMode(headphone);
                    }
                }));
            }
            else
            {
                pb_output.setDisabled(true);
                pm_output.chooseItem(0);
            }
            group_output.compileCanvas();
            com.oddlabs.tt.gui.CheckBox cb_classic = new com.oddlabs.tt.gui.CheckBox(com.oddlabs.tt.global.Settings.getSettings().classic_audio, com.oddlabs.tt.form.AbstractOptionsMenu.i18n("classic_audio"));
            addChild(cb_classic);
            if (com.oddlabs.tt.global.Settings.getSettings().classic_audio)
            {
                group_output.setDisabled(true);
            }
            cb_classic.addCheckBoxListener(new com.oddlabs.tt.guievent.CheckBoxListener_L((bool marked_3) =>
            {
                com.oddlabs.tt.global.Settings.getSettings().classic_audio = marked_3;
                com.oddlabs.tt.audio.AbstractAudioPlayer.CLASSIC_AUDIO = marked_3;
                group_output.setDisabled(marked_3);
            }));
            group_music.place();
            group_sound.place(group_music, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            group_output.place(group_sound, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            cb_classic.place(group_output, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            compileCanvas();
        }
    }
}
