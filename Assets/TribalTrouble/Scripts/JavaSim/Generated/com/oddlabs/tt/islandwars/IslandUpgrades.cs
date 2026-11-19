// Translated from IslandUpgrades.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.islandwars
{
    // Java: 11
    public class IslandUpgrades : java.lang.Object
    {
        public const int SETTLERS = 0;
        public const int WAR_BAND = 1;
        public const int STOREHOUSE = 2;
        public const int VETERANS = 3;
        public const int WATCHTOWERS = 4;
        public const int CHIEF_HALL = 5;
        public const int RAIDING_FLEET = 6;
        public const int MASONS = 7;
        public const int NUM = 8;
        public static int[] MAX;
        public static string[] KEYS;
        public static int[][] BASE;
        public static int[] SIZE_METERS;
        public static int[] SIZE_BUILDINGS;
        public static int[][] SIZE_COST;
        // Java: 38
        public static int maxLevel(int upgrade)
        {
            return com.oddlabs.tt.islandwars.IslandUpgrades.MAX[upgrade];
        }
        // Java: 42
        public static string key(int upgrade)
        {
            return com.oddlabs.tt.islandwars.IslandUpgrades.KEYS[upgrade];
        }
        // Java: 47
        public static int[] cost(int upgrade, int level)
        {
            int[] c = ((int[])com.oddlabs.tt.islandwars.IslandUpgrades.BASE[upgrade].clone());
            for (int l = 0; l < level; l++)
            {
                for (int i = 0; i < 3; i++)
                {
                    c[i] = c[i] + (c[i] + 1) / 2;
                }
            }
            return c;
        }
        // Java: 56
        public static int[] sizeCost(int size_level)
        {
            return ((int[])com.oddlabs.tt.islandwars.IslandUpgrades.SIZE_COST[size_level].clone());
        }
        // Java: 60
        public static int maxSize()
        {
            return com.oddlabs.tt.islandwars.IslandUpgrades.SIZE_METERS.Length - 1;
        }
        // Java: 65
        public static int[] start(int[] levels)
        {
            int peons = 6 + 2 * levels[com.oddlabs.tt.islandwars.IslandUpgrades.SETTLERS];
            int band = 2 * levels[com.oddlabs.tt.islandwars.IslandUpgrades.WAR_BAND];
            int iron = (levels[com.oddlabs.tt.islandwars.IslandUpgrades.WAR_BAND] >= 3 ? band / 2 : 0);
            int chicken = (levels[com.oddlabs.tt.islandwars.IslandUpgrades.WAR_BAND] >= 5 ? 2 : 0);
            return new int[] { peons, band - iron, iron - chicken, chicken, levels[com.oddlabs.tt.islandwars.IslandUpgrades.WATCHTOWERS] };
        }
        static IslandUpgrades()
        {
            IslandUpgrades.MAX = new int[] { 5, 5, 5, 3, 3, 2, 5, 3 };
            IslandUpgrades.KEYS = new string[] { "settlers", "war_band", "storehouse", "veterans", "watchtowers", "chief_hall", "raiding_fleet", "masons" };
            IslandUpgrades.BASE = new int[][] { new int[] { 120, 0, 4 }, new int[] { 160, 1, 4 }, new int[] { 140, 0, 6 }, new int[] { 240, 2, 8 }, new int[] { 200, 1, 6 }, new int[] { 300, 3, 10 }, new int[] { 180, 1, 6 }, new int[] { 260, 3, 8 } };
            IslandUpgrades.SIZE_METERS = new int[] { 256, 512, 1024, 2048 };
            IslandUpgrades.SIZE_BUILDINGS = new int[] { 14, 26, 42, 64 };
            IslandUpgrades.SIZE_COST = new int[][] { new int[] { 0, 0, 0 }, new int[] { 400, 4, 12 }, new int[] { 1200, 12, 30 }, new int[] { 3000, 30, 80 } };
        }
        public IslandUpgrades() : base()
        {
        }
    }
}
