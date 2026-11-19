// One-time set-up of the translated Steam game, and the settings the Unity side reads from the game's own Settings
// (the Java options form changes them: sound and music volume, shadows, detail).
namespace TribalTroubleSteam
{
    public static class SteamGlobals
    {
        static readonly object s_lock = new object();
        static bool s_inited;

        /// <summary>Settings, static tables and the network's method tables, once.</summary>
        public static void InitGlobals()
        {
            lock (s_lock)
            {
                if (s_inited) return;
                java.net.URL.ResourceRoot = n => "res:" + n;   // the simulation only needs names; models / sounds live in Unity
                com.oddlabs.tt.global.Settings.setSettings(new com.oddlabs.tt.global.Settings());
                com.oddlabs.tt.global.GlobalsInit.init();
                J2CS.Generated.NetTables.Register();
                s_inited = true;
            }
        }

        static com.oddlabs.tt.global.Settings S => com.oddlabs.tt.global.Settings.getSettings();

        /// <summary>The game's sound volume (Settings.play_sfx, sound_gain).</summary>
        public static float SfxVolume { get { var s = S; return s == null ? 1f : s.play_sfx ? s.sound_gain : 0f; } }

        /// <summary>The game's music volume (Settings.play_music, music_gain).</summary>
        public static float MusicVolume { get { var s = S; return s == null ? 0.5f : s.play_music ? s.music_gain : 0f; } }

        /// <summary>The game's shadow setting: the Java game draws round blob shadows (static) or dynamic ones.</summary>
        public static bool DynamicShadows { get { var s = S; return s != null && s.shadow_mode != com.oddlabs.tt.global.Globals.SHADOWS_STATIC; } }

        /// <summary>Settings.graphic_detail: low / normal / high (Globals.DETAIL_*).</summary>
        public static int Detail { get { var s = S; return s == null ? com.oddlabs.tt.global.Globals.DETAIL_NORMAL : s.graphic_detail; } }

        public static org.joml.Vector4fc TeamColour(int index)
        {
            var c = com.oddlabs.tt.global.Settings.getSettings().team_colours;
            return (org.joml.Vector4fc)c[((index % c.Length) + c.Length) % c.Length];
        }
    }
}
