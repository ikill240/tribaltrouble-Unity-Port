// Services the host application (Unity) provides to the translated Java GUI: clipboard and web browser.
namespace TribalTroubleSteam
{
    public static class PortHooks
    {
        public static System.Func<string> GetClipboardImpl;
        public static System.Action<string> SetClipboardImpl;
        public static System.Action<string> BrowseImpl;
        static string s_clip = "";

        public static string GetClipboard() => GetClipboardImpl != null ? GetClipboardImpl() : s_clip;
        public static void SetClipboard(string s) { if (SetClipboardImpl != null) SetClipboardImpl(s); else s_clip = s ?? ""; }
        public static void Browse(string url) { BrowseImpl?.Invoke(url); }

        /// <summary>GLUtils.takeScreenshot: the host saves the screen and returns the file's path ("" = the host picks).</summary>
        public static System.Func<string, string> ScreenshotImpl;
        public static string Screenshot(string filename) => ScreenshotImpl != null ? ScreenshotImpl(filename) : filename;

        /// <summary>The options menu's "Modern graphics..." button: the host's own settings page.</summary>
        public static System.Action OpenEngineSettingsImpl;
        public static void OpenEngineSettings() { OpenEngineSettingsImpl?.Invoke(); }

        /// <summary>World loads (ProgressForm). Default: inline. The Unity host sets RunLoadImpl to load on a worker
        /// thread and call 'done' on the main thread; while Loading is true the host must not run the game loop.</summary>
        public static System.Action<System.Func<object>, System.Action<object>> RunLoadImpl;
        public static volatile bool Loading;
        public static void RunLoad(System.Func<object> load, System.Action<object> done)
        {
            if (RunLoadImpl != null) RunLoadImpl(load, done);
            else done(load());
        }
    }
}
