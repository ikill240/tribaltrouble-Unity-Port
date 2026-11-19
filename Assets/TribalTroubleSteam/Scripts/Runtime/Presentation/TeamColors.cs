using UnityEngine;

namespace TribalTroubleSteam.Presentation
{
    /// <summary>Default team colours, from global/Settings.java DEFAULT_TEAM_COLOURS (first 11 hand-picked).</summary>
    public static class TeamColors
    {
        public static readonly Color[] Defaults =
        {
            Argb(0xFFFFBF00), // 0 Orange
            Argb(0xFF007FFF), // 1 Royal Blue
            Argb(0xFFFF0040), // 2 Red
            Argb(0xFF00FFBF), // 3 Teal
            Argb(0xFFBF00FF), // 4 Purple
            Argb(0xFFBFFF00), // 5 Lime
            Argb(0xFFFFFF00), // 6 Yellow
            Argb(0xFFFF69B4), // 7 Hot Pink
            Argb(0xFF006400), // 8 Dark Green
            Argb(0xFF8B0000), // 9 Maroon
            Argb(0xFF00BFFF), // 10 Sky Blue
        };

        public static readonly string[] Names =
        {
            "Orange", "Royal Blue", "Red", "Teal", "Purple", "Lime", "Yellow", "Hot Pink", "Dark Green", "Maroon", "Sky Blue",
        };

        public static Color Argb(uint argb)
        {
            return new Color32((byte)(argb >> 16), (byte)(argb >> 8), (byte)argb, (byte)(argb >> 24));
        }
    }
}
