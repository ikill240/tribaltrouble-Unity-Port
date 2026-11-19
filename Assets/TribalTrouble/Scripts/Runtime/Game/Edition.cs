using UnityEngine;

namespace TribalTrouble.Game
{
    /// <summary>
    /// The games of this project: Tribal Trouble (the Steam release, v2.2.207 - a game of its own, TribalTroubleSteam.*,
    /// started by name), Tribal Trouble 2: New Tides (the new rules on by default, the modern interface, day / night,
    /// the new campaign) and Tiki Madness. The 1.4 classic game (this project's own Java port without the New Tides
    /// rules) is its own project now ("TribalTrouble 1.4", locked to "classic14"). Picked on the launcher screen at
    /// start (or -classic / -newtides / -tiki / -classic14 on the command line, or remembered).
    /// </summary>
    public static class Edition
    {
        public const string ClassicTitle = "Tribal Trouble";
        public const string Classic14Title = "Tribal Trouble 1.4";

        /// <summary>The games: the Steam release, New Tides, Tiki Madness, the 1.4 classic game.</summary>
        public const int GameSteam = 0, GameNewTides = 1, GameTiki = 2, GameClassic14 = 3;
        public const string NewTidesTitle = "Tribal Trouble 2: New Tides";

        /// <summary>True once New Tides was picked.</summary>
        public static bool NewTides { get; private set; }

        public const string TikiTitle = "Tiki Madness: New Tides";

        /// <summary>Set before reloading the scene to show the game picker even when a choice is remembered.</summary>
        public static bool ForceLauncher;

        /// <summary>-1 ask at every start, 0 always Tribal Trouble (the Steam release), 1 always New Tides, 2 always Tiki Madness.</summary>
        public static int StartWith
        {
            get { try { return PlayerPrefs.GetInt("tt.startWith", -1); } catch { return -1; } }
            set { try { PlayerPrefs.SetInt("tt.startWith", value); PlayerPrefs.Save(); } catch { } }
        }

        static bool s_lockRead;
        static string s_lock;

        /// <summary>
        /// A copy of the project made for one game (split_games.ps1 writes Resources/TTGame.txt: "classic" - the Steam
        /// release -, "classic14", "newtides" or "tiki"): that game starts straight away - no game picker, no "At start"
        /// setting, no way back to a picker. Null in the project with all the games.
        /// </summary>
        public static string Locked
        {
            get
            {
                if (!s_lockRead)
                {
                    s_lockRead = true;
                    try
                    {
                        var ta = Resources.Load<TextAsset>("TTGame");
                        s_lock = ta != null ? ta.text.Trim().ToLowerInvariant() : null;
                        if (s_lock != "classic" && s_lock != "classic14" && s_lock != "newtides" && s_lock != "tiki") s_lock = null;
                    }
                    catch { s_lock = null; }
                }
                return s_lock;
            }
        }

        /// <summary>The game to start without asking (command line first, then the remembered choice), if any:
        /// GameSteam, GameNewTides, GameTiki or GameClassic14.</summary>
        public static bool Preset(out int game)
        {
            game = GameSteam;
            if (Locked != null)
            {
                ForceLauncher = false;
                game = Locked == "newtides" ? GameNewTides : Locked == "tiki" ? GameTiki : Locked == "classic14" ? GameClassic14 : GameSteam;
                return true;
            }
            if (ForceLauncher) { ForceLauncher = false; return false; }
            foreach (var a in System.Environment.GetCommandLineArgs())
            {
                if (a == "-classic" || a == "-steam") { game = GameSteam; return true; }
                if (a == "-newtides") { game = GameNewTides; return true; }
                if (a == "-tiki") { game = GameTiki; return true; }
                if (a == "-classic14") { game = GameClassic14; return true; }
            }
            int s = StartWith;
            if (s < 0 || s > GameTiki) return false;
            game = s;
            return true;
        }

        /// <summary>Applies the choice to the game (before the GUI starts).</summary>
        public static void Apply(bool newTides)
        {
            NewTides = newTides;
            com.oddlabs.tt.global.Edition.new_tides = newTides;
            com.oddlabs.tt.global.Edition.extended_default = newTides;
            com.oddlabs.tt.gui.UiStyle.modern = newTides && GameSettings.modernUi;
        }
    }
}
