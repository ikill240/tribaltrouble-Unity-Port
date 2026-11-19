// GUI port: the application object. Boot / Frame are Renderer.run and one pass of its main loop for an engine host
// (TribalTroubleSteam.GuiPort.Host); the rest is the Renderer API the Java GUI calls.
#pragma warning disable
using J2CS.Ext;
using TribalTroubleSteam.GuiPort;
namespace com.oddlabs.tt.render
{
    public class Renderer : java.lang.Object
    {
        static readonly Renderer s_instance = new Renderer();
        static com.oddlabs.tt.gui.LocalInput s_input;
        static readonly com.oddlabs.tt.render.state.RenderContext s_context = new TribalTroubleSteam.GuiPort.NullRenderContext();
        static com.oddlabs.tt.viewer.AmbientAudio s_ambient;
        com.oddlabs.tt.viewer.Cheat _cheat = new com.oddlabs.tt.viewer.Cheat();
        static java.lang.Runnable s_loadTask;
        static bool s_firstFrame = true;
        static int s_frames;
        static long s_fpsStart;

        public Renderer() { }

        // Steam: the display size the GUI is laid out for (Renderer.syncDisplaySize, from the game loop)
        int display_width, display_height;
        public virtual int getDisplayWidth() => display_width;
        public virtual int getDisplayHeight() => display_height;
        public virtual void syncDisplaySize(com.oddlabs.tt.gui.GUI gui)
        {
            var deterministic = com.oddlabs.tt.@event.LocalEventQueue.getQueue().getDeterministic();
            int width = Host.Window.getWidth(), height = Host.Window.getHeight();
            bool changed = width > 0 && height > 0 && (width != display_width || height != display_height);
            if (!deterministic.log(changed)) return;
            display_width = deterministic.log(width);
            display_height = deterministic.log(height);
            if (gui != null) gui.getGUIRoot().displayChanged(display_width, display_height);
        }

        public static com.oddlabs.tt.gui.LocalInput getLocalInput() => s_input ??= new com.oddlabs.tt.gui.LocalInput(Host.Window);
        public static bool isFinished() => Host.Finished;
        public virtual com.oddlabs.tt.window.Window getWindow() => Host.Window;
        public static Renderer getRenderer() => s_instance;
        public static void shutdown()
        {
            if (Host.Finished) return;
            Host.Finished = true;
            try { com.oddlabs.tt.global.Settings.getSettings().save(); } catch (System.Exception) { }
            Host.Backend.Quit();
        }
        public static void startMenu(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUI gui) => PortMenu.setupMainMenu(network, gui, false);
        public static bool isRegistered() => !TribalTroubleSteam.Bridge.OnlineHooks.Guest;
        public virtual void close() { }
        public virtual void setCheat(com.oddlabs.tt.viewer.Cheat cheat) => _cheat = cheat;
        public virtual bool isCheater() => _cheat != null && _cheat.isEnabled();
        public com.oddlabs.tt.viewer.Cheat getCheat() => _cheat;
        public virtual com.oddlabs.tt.render.state.RenderContext getRenderContext() => s_context;
        public virtual void toggleFullscreen() => Host.Backend.ToggleFullscreen();
        public virtual void startMovieRecording() { }
        public virtual com.oddlabs.tt.render.SerializableDisplayMode getCurrentDisplayMode() => null;
        public virtual void switchMode(com.oddlabs.tt.render.SerializableDisplayMode mode, bool switch_now) { }
        public virtual java.util.Locale getDefaultLocale() => java.util.Locale.getDefault();
        /// <summary>Java repaints from inside a load; the engine host draws every frame anyway (loads run on a worker).</summary>
        public virtual void updateProgress(com.oddlabs.tt.gui.GUI gui) { }
        public virtual void toggleMusic() { }
        public virtual com.oddlabs.tt.audio.AbstractAudioPlayer getMusicPlayer() => null;
        public virtual void toggleSound() { }
        public static int getTrianglesRendered() => 0;
        public static float getFPS() => Host.Fps;
        public virtual void setMusicPath(string path, float delay) => Host.Backend.SetMusic(path, delay);

        // ------------------------------------------------------------------ host

        /// <summary>Renderer.run up to the main loop. Returns the GUI.</summary>
        public static com.oddlabs.tt.gui.GUI Boot(string gameDir, com.oddlabs.net.NetworkSelector network)
        {
            Host.Finished = false;
            var guiOut = new com.oddlabs.tt.gui.GUI[1];
            s_loadTask = PortMenu.boot(java.nio.file.Paths.get(gameDir), network, guiOut);
            s_ambient = new com.oddlabs.tt.viewer.AmbientAudio();
            s_firstFrame = true;
            return guiOut[0];
        }

        /// <summary>One pass of the Java main loop: game loop (unless a world is loading), resize, render.</summary>
        /// <summary>Last frame: the game loop (ticks, picking, input) and the Java GUI's render, in ms.</summary>
        public static float LastLoopMs, LastGuiMs;

        public static void Frame(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUI gui)
        {
            if (Host.Finished) return;
            FrameLoop(network, gui);
            FrameRest(gui);
        }

        /// <summary>The first half of a frame: the game loop (ticks, input, network). The engine may run it on a
        /// thread of its own, as long as nothing else touches the game meanwhile; FrameRest follows it.</summary>
        public static void FrameLoop(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUI gui)
        {
            if (Host.Finished) return;
            long t0 = System.Diagnostics.Stopwatch.GetTimestamp();
            if (!TribalTroubleSteam.PortHooks.Loading)
                com.oddlabs.tt.animation.AnimationManager.runGameLoop(network, gui, false);
            LastLoopMs = (float)((System.Diagnostics.Stopwatch.GetTimestamp() - t0) * 1000.0 / System.Diagnostics.Stopwatch.Frequency);
        }

        /// <summary>The second half: resize and the GUI's render (on the engine's main thread).</summary>
        public static void FrameRest(com.oddlabs.tt.gui.GUI gui)
        {
            if (Host.Finished) return;
            long t1 = System.Diagnostics.Stopwatch.GetTimestamp();
            if (TribalTroubleSteam.PortHooks.Loading) s_instance.syncDisplaySize(gui);   // (the game loop does it otherwise)
            gui.render(s_ambient);
            long t2 = System.Diagnostics.Stopwatch.GetTimestamp();
            double f = 1000.0 / System.Diagnostics.Stopwatch.Frequency;
            LastGuiMs = (float)((t2 - t1) * f);
            Host.FrameCount++;
            s_frames++;
            long now = java.lang.JSystem.currentTimeMillis();
            if (now - s_fpsStart >= 1000) { Host.Fps = s_frames * 1000f / (now - s_fpsStart); s_frames = 0; s_fpsStart = now; }
            if (s_firstFrame)
            {
                s_firstFrame = false;
                var task = s_loadTask;
                s_loadTask = null;
                if (task != null)
                {
                    var det = com.oddlabs.tt.@event.LocalEventQueue.getQueue().getDeterministic();
                    det.setEnabled(true);
                    try { task.run(); } finally { det.setEnabled(false); }
                }
            }
        }
    }
}
