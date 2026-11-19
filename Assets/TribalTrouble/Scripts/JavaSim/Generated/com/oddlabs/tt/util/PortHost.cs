// Translated from PortHost.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.util
{
    // Java: 7
    public class PortHost : java.lang.Object
    {
        // Java: 11
        public static string getClipboard()
        {
            return TribalTrouble.PortHooks.GetClipboard();
        }
        // Java: 20
        public static void setClipboard(string text)
        {
            TribalTrouble.PortHooks.SetClipboard(text);
        }
        // Java: 33
        public static void runLoad(java.util.function.Supplier load, java.util.function.Consumer done)
        {
            TribalTrouble.PortHooks.RunLoad(() => load.get(), r => done.accept(r));
        }
        // Java: 38
        public static com.oddlabs.tt.render.Texture textureFromRGB(int size, sbyte[] rgb)
        {
            return com.oddlabs.tt.render.Texture.FromRGB(size, rgb);
        }
        // Java: 54
        public static void heightsEdited(int patch_x0, int patch_y0, int patch_x1, int patch_y1)
        {
            int n = com.oddlabs.tt.landscape.HeightMap.GRID_UNITS_PER_PATCH; TribalTrouble.GuiPort.EditorView.GroundEdited(patch_x0 * n - 1, patch_y0 * n - 1, (patch_x1 + 1) * n + 1, (patch_y1 + 1) * n + 1);
        }
        // Java: 58
        public static void openEngineSettings()
        {
            TribalTrouble.PortHooks.OpenEngineSettings();
        }
        // Java: 61
        public static void browse(string url)
        {
            TribalTrouble.PortHooks.Browse(url);
        }
        public PortHost() : base()
        {
        }
    }
}
