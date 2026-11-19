// Translated from MapEditor.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.mapeditor
{
    // Java: 21
    public class MapEditor : java.lang.Object
    {
        public static java.util.ResourceBundle bundle;
        public static java.util.ResourceBundle terrain_bundle;
        public static java.util.ResourceBundle mapcode_bundle;
        public const string MAPS_DIR = "maps";
        // Java: 32
        public static string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.mapeditor.MapEditor.bundle, key, args);
        }
        // Java: 36
        public static string terrainI18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.mapeditor.MapEditor.terrain_bundle, key, args);
        }
        // Java: 40
        public static string mapcodeI18n(string key)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.mapeditor.MapEditor.mapcode_bundle, key);
        }
        // Java: 47
        public static void chooseMapToPlay(com.oddlabs.tt.gui.GUIRoot gui_root, java.util.function.Consumer play)
        {
            java.nio.file.Path dir = com.oddlabs.tt.mapeditor.MapEditor.getMapsDir();
            if (dir != null)
            {
                try
                {
                    java.nio.file.Files.createDirectories(dir);
                }
                catch (System.IO.IOException unused)
                {
                    dir = null;
                }
            }
            java.nio.file.Path start = (dir != null ? dir : java.nio.file.Path_.of(java.lang.JSystem.getProperty("user.home")));
            gui_root.addModalForm(new com.oddlabs.tt.mapeditor.LoadMapDialog(gui_root, start, com.oddlabs.tt.mapeditor.MapEditor.i18n("play_caption"), com.oddlabs.tt.mapeditor.MapEditor.i18n("play_button"), new java.util.function.Consumer_L((object entry_l) =>
            {
                com.oddlabs.tt.mapeditor.MapFile.Entry entry = (com.oddlabs.tt.mapeditor.MapFile.Entry)entry_l;
                com.oddlabs.tt.mapeditor.MapFile map = default;
                try
                {
                    map = com.oddlabs.tt.mapeditor.MapFile.load(entry.path());
                }
                catch (System.IO.IOException e)
                {
                    gui_root.addModalForm(new com.oddlabs.tt.form.MessageForm(com.oddlabs.tt.mapeditor.MapEditor.i18n("load_failed", e.getMessage())));
                    return;
                }
                play.accept(new com.oddlabs.tt.mapeditor.CustomMap(map.name(), map.settings(), entry.path()));
            })));
        }
        // Java: 72
        public static bool isEditorMap(java.nio.file.Path path)
        {
            {
                java.io.InputStream @in = java.nio.file.Files.newInputStream(path);
                try
                {
                    return @in.read() == 31 && @in.read() == 139;
                }
                catch (System.IO.IOException e)
                {
                    return false;
                }
                finally
                {
                    if (@in != null) @in.close();
                }
            }
        }
        // Java: 81
        public static com.oddlabs.tt.mapeditor.CustomMap customMapFromFile(java.nio.file.Path path)
        {
            com.oddlabs.tt.mapeditor.MapFile map = com.oddlabs.tt.mapeditor.MapFile.load(path);
            return new com.oddlabs.tt.mapeditor.CustomMap(map.name(), map.settings(), path);
        }
        // Java: 86
        public static java.nio.file.Path getMapsDir()
        {
            java.nio.file.Path game_dir = com.oddlabs.tt.render.Renderer.getLocalInput().getGameDir();
            return (game_dir == null ? (java.nio.file.Path)(null) : game_dir.resolve(com.oddlabs.tt.mapeditor.MapEditor.MAPS_DIR));
        }
        static MapEditor()
        {
            MapEditor.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.mapeditor.MapEditor).getName());
            MapEditor.terrain_bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.form.TerrainMenu).getName());
            MapEditor.mapcode_bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.form.MapcodeForm).getName());
        }
        public MapEditor() : base()
        {
        }
    }
}
