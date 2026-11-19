// Translated from ProgressForm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 27
    public class ProgressForm : java.lang.Object
    {
        public const int PROGRESSBAR_LOADINGTIP_SPACING = 45;
        public const int NUM_TIPS = 39;
        public const string TIP_PREFIX = "tip";
        public static java.util.ResourceBundle bundle;
        // Java: 33
        public static string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.form.ProgressForm.bundle, key, args);
        }
        public static string[] LOADING_TIPS;
        public static com.oddlabs.tt.form.ProgressForm current_progress;
        public com.oddlabs.tt.gui.ProgressBar progress_bar;
        public com.oddlabs.tt.gui.GUI gui;
        // Java: 45
        public static void setProgressForm(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUI gui, com.oddlabs.tt.form.LoadCallback callback_v)
        {
            com.oddlabs.tt.form.ProgressForm.setProgressForm(network, gui, callback_v, false);
        }
        // Java: 50
        public static java.lang.Runnable setProgressForm(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUI gui, com.oddlabs.tt.form.LoadCallback callback_v, bool first_progress)
        {
            string texture = default;
            int texture_width = default;
            int texture_height = default;
            int image_width = default;
            int image_height = default;
            int progress_x = default;
            int progress_y = default;
            int progress_width = default;
            bool show_tip = default;
            if (first_progress)
            {
                texture = "/textures/gui/oddlabs";
                texture_width = 1024;
                texture_height = 1024;
                image_width = 800;
                image_height = 600;
                progress_x = 320;
                progress_y = 145;
                progress_width = 200;
                show_tip = false;
            }
            else
            {
                texture = "/textures/gui/startup";
                texture_width = 1024;
                texture_height = 1024;
                image_width = 800;
                image_height = 600;
                progress_x = 250;
                progress_y = 145;
                progress_width = 300;
                show_tip = true;
            }
            com.oddlabs.tt.gui.Fadable load_fadable = new com.oddlabs.tt.gui.Fadable_L(() =>
            {
                com.oddlabs.tt.form.ProgressForm.callback(gui, callback_v, first_progress);
            });
            com.oddlabs.tt.form.ProgressForm.current_progress = new com.oddlabs.tt.form.ProgressForm(network, gui, load_fadable, first_progress, new com.oddlabs.tt.gui.ProgressBarInfo[] { new com.oddlabs.tt.gui.ProgressBarInfo("", 10), new com.oddlabs.tt.gui.ProgressBarInfo("", 30), new com.oddlabs.tt.gui.ProgressBarInfo("", 5), new com.oddlabs.tt.gui.ProgressBarInfo("", 5), new com.oddlabs.tt.gui.ProgressBarInfo("", 5), new com.oddlabs.tt.gui.ProgressBarInfo("", 2.0f), new com.oddlabs.tt.gui.ProgressBarInfo("", 5), new com.oddlabs.tt.gui.ProgressBarInfo("", 6) }, texture, texture_width, texture_height, image_width, image_height, progress_x, progress_y, progress_width, show_tip);
            return (first_progress ? new java.lang.Runnable_L(() => { load_fadable.fadingDone(); }) : (java.lang.Runnable)(null));
        }
        // Java: 139
        public static void callback(com.oddlabs.tt.gui.GUI gui, com.oddlabs.tt.form.LoadCallback callback_v, bool first_progress)
        {
            com.oddlabs.tt.gui.Fadable start_sources_fadable = new com.oddlabs.tt.gui.Fadable_L(() =>
            {
                com.oddlabs.tt.audio.AudioManager.getManager().startSources();
            });
            com.oddlabs.tt.gui.GUIRoot client_root = gui.createRoot();
            com.oddlabs.tt.util.PortHost.runLoad(new java.util.function.Supplier_L(() =>
            {
                return callback_v.load(client_root);
            }), new java.util.function.Consumer_L((object renderer) =>
            {
                gui.newFade(start_sources_fadable, client_root, ((com.oddlabs.tt.render.UIRenderer)renderer));
            }));
        }
        // Java: 147
        public static void progress()
        {
            if (null != com.oddlabs.tt.form.ProgressForm.current_progress)
            {
                com.oddlabs.tt.form.ProgressForm.current_progress.progress_bar.progress();
                com.oddlabs.tt.render.Renderer.getRenderer().updateProgress(com.oddlabs.tt.form.ProgressForm.current_progress.gui);
            }
        }
        // Java: 154
        public static void progress(float step)
        {
            if (null != com.oddlabs.tt.form.ProgressForm.current_progress)
            {
                com.oddlabs.tt.form.ProgressForm.current_progress.progress_bar.progress(step);
                com.oddlabs.tt.render.Renderer.getRenderer().updateProgress(com.oddlabs.tt.form.ProgressForm.current_progress.gui);
            }
        }
        static ProgressForm()
        {
            ProgressForm.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.form.ProgressForm).getName());
            ProgressForm.LOADING_TIPS = ((string[])java.util.stream.IntStream_.range(0, com.oddlabs.tt.form.ProgressForm.NUM_TIPS).mapToObj(new java.util.function.IntFunction_L((int idx) =>
            {
                return com.oddlabs.tt.form.ProgressForm.i18n(java.lang.JStr.valueOf(com.oddlabs.tt.form.ProgressForm.TIP_PREFIX) + (idx));
            })).toArray(new java.util.function.IntFunction_L((int a0) => { return new string[a0]; })));
            ProgressForm.current_progress = null;
        }
        public ProgressForm(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUI gui, com.oddlabs.tt.gui.Fadable load_fadable, bool first_progress, com.oddlabs.tt.gui.ProgressBarInfo[] info, string texture_name, int texture_width, int texture_height, int image_width, int image_height, int progress_x, int progress_y, int progress_width, bool show_tip) : base()
        {
            this.gui = gui;
            com.oddlabs.tt.audio.AudioManager.getManager().stopSources();
            com.oddlabs.tt.gui.GUIRoot gui_root = (first_progress ? gui.getGUIRoot() : gui.newFade(load_fadable, null));
            com.oddlabs.tt.@delegate.CameraDelegate @delegate = new com.oddlabs.tt.@delegate.NullDelegate(gui_root, false);
            gui_root.pushDelegate(@delegate);
            int screen_width = gui_root.getWidth();
            int screen_height = gui_root.getHeight();
            progress_width = java.lang.JC.f2i(((float)((float)(progress_width * ((float)(screen_width))) / image_width)));
            progress_x = java.lang.JC.f2i(((float)((float)(progress_x * ((float)(screen_width))) / image_width)));
            progress_y = java.lang.JC.f2i(((float)((float)(progress_y * ((float)(screen_height))) / image_height)));
            com.oddlabs.tt.gui.GUIImage image = new com.oddlabs.tt.gui.GUIImage(screen_width, screen_height, 0.0f, 0.0f, (float)(((float)(image_width)) / texture_width), (float)(((float)(image_height)) / texture_height), texture_name);
            image.setPos(0, 0);
            progress_bar = new com.oddlabs.tt.gui.ProgressBar(network, progress_width, info, false);
            progress_y -= progress_bar.getHeight();
            progress_bar.setPos(progress_x, progress_y);
            @delegate.addChild(image);
            @delegate.addChild(progress_bar);
            if (show_tip)
            {
                java.util.Random random = new java.util.Random(com.oddlabs.tt.@event.LocalEventQueue.getQueue().getHighPrecisionManager().getTick());
                string tip_string = com.oddlabs.tt.form.ProgressForm.LOADING_TIPS[random.nextInt(com.oddlabs.tt.form.ProgressForm.LOADING_TIPS.Length)];
                int tip_width = java.lang.Math.min(gui_root.getWidth() - 10, com.oddlabs.tt.gui.Skin.getSkin().getEditFont().getWidth(tip_string));
                com.oddlabs.tt.gui.LabelBox tip = new com.oddlabs.tt.gui.LabelBox(tip_string, com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), tip_width);
                tip.setPos(progress_bar.getX() + progress_bar.getWidth() / 2 - tip.getWidth() / 2, progress_bar.getY() - tip.getHeight() - com.oddlabs.tt.form.ProgressForm.PROGRESSBAR_LOADINGTIP_SPACING);
                @delegate.addChild(tip);
            }
            com.oddlabs.tt.render.Renderer.getRenderer().updateProgress(gui);
        }
    }
}
