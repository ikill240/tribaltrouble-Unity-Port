// GUI port: TerrainMenu's preset library. The Java game stores presets.json with Jackson; the port keeps the same
// Preset objects, saved with Java serialization (presets.ser next to the settings).
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gamemode
{
    public sealed class PresetLibrary : java.lang.Object
    {
        readonly java.util.ArrayList presets = new java.util.ArrayList();

        public PresetLibrary() { }

        public java.util.List all() => java.util.Collections.unmodifiableList(presets);

        public java.util.List forMode(com.oddlabs.matchmaking.GameMode mode)
        {
            var filtered = new java.util.ArrayList();
            for (int i = 0; i < presets.size(); i++)
            {
                var p = (com.oddlabs.matchmaking.Preset)presets.get(i);
                if (p.getMode() == mode) filtered.add(p);
            }
            return java.util.Collections.unmodifiableList(filtered);
        }

        public com.oddlabs.matchmaking.Preset findById(string id)
        {
            for (int i = 0; i < presets.size(); i++) { var p = (com.oddlabs.matchmaking.Preset)presets.get(i); if (p.getId() == id) return p; }
            return null;
        }

        public bool hasName(string name)
        {
            for (int i = 0; i < presets.size(); i++) if (((com.oddlabs.matchmaking.Preset)presets.get(i)).getName() == name) return true;
            return false;
        }

        public void add(com.oddlabs.matchmaking.Preset preset) => presets.add(preset);

        public bool remove(com.oddlabs.matchmaking.Preset preset)
        {
            for (int i = 0; i < presets.size(); i++)
                if (((com.oddlabs.matchmaking.Preset)presets.get(i)).getId() == preset.getId()) { presets.remove(i); return true; }
            return false;
        }

        static string Ser(java.nio.file.Path file) => System.IO.Path.ChangeExtension(file.toString(), ".ser");

        public void load(java.nio.file.Path file)
        {
            presets.clear();
            string path = Ser(file);
            if (!System.IO.File.Exists(path)) return;
            try
            {
                var bytes = System.IO.File.ReadAllBytes(path);
                var input = new java.io.ObjectInputStream(new java.io.ByteArrayInputStream((sbyte[])(System.Array)bytes));
                var list = (java.util.List)input.readObject();
                for (int i = 0; i < list.size(); i++) presets.add(list.get(i));
            }
            catch (System.Exception e) { java.lang.IO.println("Failed to read presets from " + path + "; starting empty. " + e.Message); }
        }

        public void save(java.nio.file.Path file)
        {
            try
            {
                var bos = new java.io.ByteArrayOutputStream();
                var oos = new java.io.ObjectOutputStream(bos);
                oos.writeObject(new java.util.ArrayList(presets));
                oos.flush();
                System.IO.File.WriteAllBytes(Ser(file), (byte[])(System.Array)bos.toByteArray());
            }
            catch (System.Exception e) { java.lang.IO.println("Failed to write presets: " + e.Message); }
        }
    }
}
