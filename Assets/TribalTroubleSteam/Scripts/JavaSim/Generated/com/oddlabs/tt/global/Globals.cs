// Translated from Globals.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.global
{
    // Java: 11
    public class Globals : java.lang.Object
    {
        public const int DETAIL_LOW = 0;
        public const int DETAIL_NORMAL = 1;
        public const int DETAIL_HIGH = 2;
        public const int SHADOWS_STATIC = 0;
        public const int SHADOWS_DYNAMIC = 1;
        public static org.joml.Vector3f FAKE_SKY_BODY_DIRECTION;
        public static org.joml.Vector3f REAL_SKY_BODY_DIRECTION;
        public static int[] TEXTURE_MIP_SHIFT;
        public static int[] UNIT_HIGH_POLY_COUNT;
        public static int[] LANDSCAPE_POLY_COUNT;
        public static bool[] INSERT_PLANTS;
        public const bool SHIPS_ENABLED = true;
        public const string GAME_NAME = "TribalTrouble";
        public const string SETTINGS_FILE_NAME = "settings";
        public const string SAVEGAMES_FILE_NAME = "savegames";
        public const string PRESETS_FILE_NAME = "presets.json";
        // Java: 39
        public static java.nio.file.Path getSettingsFileName()
        {
            return com.oddlabs.tt.global.Globals.steamPrefixed(com.oddlabs.tt.global.Globals.SETTINGS_FILE_NAME);
        }
        // Java: 43
        public static java.nio.file.Path getSavegamesFileName()
        {
            return com.oddlabs.tt.global.Globals.steamPrefixed(com.oddlabs.tt.global.Globals.SAVEGAMES_FILE_NAME);
        }
        // Java: 47
        public static java.nio.file.Path getPresetsFileName()
        {
            return com.oddlabs.tt.global.Globals.steamPrefixed(com.oddlabs.tt.global.Globals.PRESETS_FILE_NAME);
        }
        // Java: 51
        public static java.nio.file.Path steamPrefixed(string name)
        {
            com.oddlabs.tt.steam.SteamManager steam = com.oddlabs.tt.steam.SteamManager.getInstance();
            if (steam != null)
            {
                return java.nio.file.Path_.of((steam.getAccountID()) + "." + java.lang.JStr.valueOf(name));
            }
            return java.nio.file.Path_.of(name);
        }
        public static bool debug_steam_auth_localhost;
        public static bool run_ai;
        public static int gamespeed;
        public const bool process_landscape = true;
        public const bool process_trees = true;
        public static bool process_misc;
        public const bool process_shadows = true;
        public static bool draw_status;
        public static bool draw_hud;
        public static bool cinematic_camera;
        public const bool draw_landscape = true;
        public const bool draw_trees = true;
        public static bool draw_misc;
        public static bool draw_particles;
        public static bool draw_water;
        public const bool draw_sky = true;
        public static bool draw_axes;
        public static bool draw_detail;
        public static bool draw_shadows;
        public static bool draw_light;
        public static bool draw_plants;
        public static bool draw_debug_maps;
        public static bool classic_lighting;
        public const bool line_mode = false;
        public static bool clear_frame_buffer;
        public static bool frustum_freeze;
        public static bool slowmotion;
        public static bool checksum_error_in_last_game;
        public static com.oddlabs.tt.global.BoundingMode bounding;
        // Java: 101
        public static void switchBoundingMode()
        {
            com.oddlabs.tt.global.Globals.bounding = com.oddlabs.tt.global.Globals.bounding.next();
            java.lang.IO.println("Bounding mode: " + java.lang.JStr.valueOf(com.oddlabs.tt.global.Globals.bounding));
        }
        // Java: 106
        public static bool isBoundsEnabled(com.oddlabs.tt.global.BoundingMode mode)
        {
            return ((object)com.oddlabs.tt.global.Globals.bounding == (object)mode) || ((object)com.oddlabs.tt.global.Globals.bounding == (object)com.oddlabs.tt.global.BoundingMode.ALL);
        }
        // Java: 110
        public static bool debugRenderingEnabled()
        {
            return com.oddlabs.tt.global.Globals.draw_axes || ((object)com.oddlabs.tt.global.Globals.bounding != (object)com.oddlabs.tt.global.BoundingMode.NONE);
        }
        public const int COMPRESSED_RGB_FORMAT = 34029;
        public const int COMPRESSED_RGBA_FORMAT = 34030;
        public const int COMPRESSED_A_FORMAT = 33321;
        public const int COMPRESSED_LUMINANCE_FORMAT = 6403;
        public static int LOW_DETAIL_TEXTURE_SHIFT;
        public const float LANDSCAPE_HILLS = 1.0f;
        public const float LANDSCAPE_VEGETATION = 2.0f;
        public const float LANDSCAPE_RESOURCES = 0.0f;
        public const int LANDSCAPE_SEED = 1;
        public const float LANDSCAPE_TEXTURE_SCALE = 0.0625f;
        public const int VIEW_BIT_DEPTH = 16;
        public const float FOV = 45.0f;
        public const float VIEW_MIN = 1.2f;
        public const float VIEW_MAX = 9000.0f;
        public const int NET_PORT = 21000;
        public const int NO_MIPMAP_CUTOFF = 1000;
        public const int STRUCTURE_SIZE = 256;
        public const int DETAIL_SIZE = 256;
        public const int TEXELS_PER_GRID_UNIT = 8;
        public const float LANDSCAPE_DETAIL_REPEAT_RATE = 0.25f;
        public const float WATER_REPEAT_RATE = 0.001f;
        public const float WATER_DETAIL_REPEAT_RATE = 0.01f;
        public const int LANDSCAPE_DETAIL_FADEOUT_BASE_LEVEL = 2;
        public const float LANDSCAPE_DETAIL_FADEOUT_FACTOR = 0.75f;
        public const int MAX_RENDERNODE_DEPTH = 5;
        public const string SCREENSHOT_DEFAULT = "screenshot";
        public const float TREE_ERROR_DISTANCE = 100.0f;
        public const float WHEEL_SCALE = 0.01f;
        public const int CURSOR_BLINK_TIME = 1000;
        public const int FPS_WIDTH = 800;
        public const int SHELL_HISTORY_SIZE = 50;
        public const int SHELL_HISTORY_PAGE_SIZE = 10;
        public const int MIN_TEXTURE_POWER = 2;
        public const int MIN_TEXTURE_SIZE = 4;
        public static int MAX_TEXTURE_POWER;
        public static int MAX_TEXTURE_SIZE;
        public const float TEXTURE_WEIGHT = 0.5f;
        public static int[] TEXTURE_SIZES;
        public static sbyte[] TEXTURE_SPLITS;
        public static int[] BEST_SIZES;
        public const float SEA_LEVEL = 0.1f;
        public const int TEXELS_PER_CHUNK_BORDER = 4;
        public const int BLOCK_SCROLL_AMOUNT = 20;
        public const float ERROR_TOLERANCE = 10.0f;
        static Globals()
        {
            Globals.FAKE_SKY_BODY_DIRECTION = new org.joml.Vector3f(+0.0f, (float)(-1.0f), +0.2f).normalize();
            Globals.REAL_SKY_BODY_DIRECTION = new org.joml.Vector3f(+0.0f, (float)(-1.0f), +0.99f).normalize();
            Globals.TEXTURE_MIP_SHIFT = new int[] { 0, 0, 0 };
            Globals.UNIT_HIGH_POLY_COUNT = new int[] { 20000, 80000, 200000 };
            Globals.LANDSCAPE_POLY_COUNT = new int[] { 10000, 40000, 100000 };
            Globals.INSERT_PLANTS = new bool[] { true, true, true };
            Globals.debug_steam_auth_localhost = false;
            Globals.run_ai = true;
            Globals.gamespeed = 2;
            Globals.process_misc = true;
            Globals.draw_status = false;
            Globals.draw_hud = true;
            Globals.cinematic_camera = false;
            Globals.draw_misc = true;
            Globals.draw_particles = true;
            Globals.draw_water = true;
            Globals.draw_axes = false;
            Globals.draw_detail = true;
            Globals.draw_shadows = true;
            Globals.draw_light = true;
            Globals.draw_plants = true;
            Globals.draw_debug_maps = false;
            Globals.classic_lighting = true;
            Globals.clear_frame_buffer = false;
            Globals.frustum_freeze = false;
            Globals.slowmotion = false;
            Globals.checksum_error_in_last_game = false;
            Globals.bounding = com.oddlabs.tt.global.BoundingMode.NONE;
            Globals.LOW_DETAIL_TEXTURE_SHIFT = 1;
        }
        public Globals() : base()
        {
        }
    }
}
