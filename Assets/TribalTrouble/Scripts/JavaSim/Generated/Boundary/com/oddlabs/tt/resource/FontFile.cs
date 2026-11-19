// GUI port: fonts are read from the text dumps of the game's .font files (font/<name>.txt among the resources;
// made by guiport/tools/FontDump.java from the serialized FontInfo).
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.resource
{
    public sealed class FontFile : com.oddlabs.tt.resource.File
    {
        readonly string _name;

        public FontFile(string file_name) : base(java.net.URI.create("res:" + file_name)) { _name = file_name; }

        public override object get() => new com.oddlabs.tt.font.Font(Load(_name));

        static com.oddlabs.util.FontInfo Load(string name)
        {
            var bytes = java.lang.JResources.Open?.Invoke(name + ".txt");
            if (bytes == null) throw new System.IO.IOException("font not found: " + name);
            string texture = null; int bx = 0, by = 0, h = 0, asc = 0, desc = 0;
            var map = new com.oddlabs.util.HashTable();
            foreach (var raw in System.Text.Encoding.UTF8.GetString(bytes).Split('\n'))
            {
                var p = raw.Trim().Split(' ');
                if (p.Length < 2) continue;
                switch (p[0])
                {
                    case "texture": texture = p[1]; break;
                    case "border": bx = int.Parse(p[1]); by = int.Parse(p[2]); break;
                    case "height": h = int.Parse(p[1]); break;
                    case "ascension": asc = int.Parse(p[1]); break;
                    case "descension": desc = int.Parse(p[1]); break;
                    case "q":
                        float F(string s) => System.BitConverter.Int32BitsToSingle(int.Parse(s));
                        map.put(int.Parse(p[1]), new com.oddlabs.util.Quad(F(p[2]), F(p[3]), F(p[4]), F(p[5]), int.Parse(p[6]), int.Parse(p[7])));
                        break;
                }
            }
            return new com.oddlabs.util.FontInfo(texture, map, bx, by, h, asc, desc);
        }

        public override bool equals(object o) => o is FontFile f && f._name == _name;
        // ---- merged from the generated stub (members the translated code now uses)
        public FontFile() : base(default(java.net.URI)) { }
    }
}
