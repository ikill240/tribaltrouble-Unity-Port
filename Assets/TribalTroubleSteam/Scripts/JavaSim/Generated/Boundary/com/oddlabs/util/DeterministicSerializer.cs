// Steam port: the saved campaigns. Java writes them with Java serialization; here they are a text file of the same
// fields (one "key=value" per line, a blank line between campaigns) next to the settings. The callbacks run straight
// away, like the Java ones do once the file is read.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.util
{
    public class DeterministicSerializer : java.lang.Object
    {
        static string PathOf(java.nio.file.Path p) => p == null ? null : p.toString();

        public static void save(com.oddlabs.@event.Deterministic p0, object p1, java.nio.file.Path p2, com.oddlabs.util.DeterministicSerializerLoopbackInterface p3)
        {
            try
            {
                string path = PathOf(p2);
                if (path == null) throw new System.IO.IOException("no game folder");
                if (!(p1 is com.oddlabs.tt.player.campaign.CampaignState[] states))
                    throw new System.IO.IOException("cannot save " + (p1 == null ? "null" : p1.GetType().Name));
                string text = EncodeAll(states);
                var dir = System.IO.Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dir)) System.IO.Directory.CreateDirectory(dir);
                // write next to it first, then swap in: a crash mid-write never loses the campaigns
                string tmp = path + ".tmp";
                System.IO.File.WriteAllText(tmp, text);
                if (System.IO.File.Exists(path)) System.IO.File.Replace(tmp, path, path + ".bak");
                else System.IO.File.Move(tmp, path);
            }
            catch (System.Exception e)
            {
                java.lang.IO.println("Saving campaigns failed: " + e.Message);
                p3?.failed(e);
                return;
            }
            p3?.saveSucceeded();
        }

        public static void load(com.oddlabs.@event.Deterministic p0, java.nio.file.Path p1, com.oddlabs.util.DeterministicSerializerLoopbackInterface p2)
        {
            com.oddlabs.tt.player.campaign.CampaignState[] states;
            try
            {
                string path = PathOf(p1);
                if (path == null || !System.IO.File.Exists(path)) throw new System.IO.IOException("NoSuchFileException: " + path);
                states = DecodeAll(System.IO.File.ReadAllText(path));
            }
            catch (System.Exception e)
            {
                p2?.failed(e);
                return;
            }
            p2?.loadSucceeded(states);
        }

        static string Ints(int[] a) => a == null ? "" : string.Join(",", a);
        static int[] Ints(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return new int[0];
            var parts = s.Split(',');
            var a = new int[parts.Length];
            for (int i = 0; i < parts.Length; i++) a[i] = int.Parse(parts[i].Trim(), System.Globalization.CultureInfo.InvariantCulture);
            return a;
        }

        public static string EncodeAll(com.oddlabs.tt.player.campaign.CampaignState[] states)
        {
            var sb = new System.Text.StringBuilder("# Tribal Trouble campaigns\n");
            var inv = System.Globalization.CultureInfo.InvariantCulture;
            foreach (var c in states)
            {
                sb.Append('\n');
                sb.Append("name=").Append((c.name ?? "").Replace('\n', ' ').Replace('\r', ' ')).Append('\n');
                sb.Append("date=").Append(c.date.ToString(inv)).Append('\n');
                sb.Append("race=").Append(c.race).Append('\n');
                sb.Append("difficulty=").Append(c.difficulty).Append('\n');
                sb.Append("islands=").Append(Ints(c.island_states)).Append('\n');
                sb.Append("current=").Append(c.current_island).Append('\n');
                sb.Append("prev=").Append(c.prev_island).Append('\n');
                sb.Append("peons=").Append(c.num_peons).Append('\n');
                sb.Append("rock=").Append(c.num_rock_warriors).Append('\n');
                sb.Append("iron=").Append(c.num_iron_warriors).Append('\n');
                sb.Append("rubber=").Append(c.num_rubber_warriors).Append('\n');
                sb.Append("rubber_weapons=").Append(c.has_rubber_weapons ? 1 : 0).Append('\n');
                sb.Append("magic0=").Append(c.has_magic0 ? 1 : 0).Append('\n');
                sb.Append("magic1=").Append(c.has_magic1 ? 1 : 0).Append('\n');
            }
            return sb.ToString();
        }

        public static com.oddlabs.tt.player.campaign.CampaignState[] DecodeAll(string text)
        {
            var list = new System.Collections.Generic.List<com.oddlabs.tt.player.campaign.CampaignState>();
            var m = new System.Collections.Generic.Dictionary<string, string>();
            void Flush()
            {
                if (m.ContainsKey("islands")) list.Add(Read(m));
                m = new System.Collections.Generic.Dictionary<string, string>();
            }
            foreach (var raw in (text ?? "").Replace("\r", "").Split('\n'))
            {
                string line = raw.Trim();
                if (line.Length == 0) { Flush(); continue; }
                if (line.StartsWith("#")) continue;
                int eq = line.IndexOf('=');
                if (eq > 0) m[line.Substring(0, eq).Trim()] = line.Substring(eq + 1);
            }
            Flush();
            return list.ToArray();
        }

        static com.oddlabs.tt.player.campaign.CampaignState Read(System.Collections.Generic.Dictionary<string, string> m)
        {
            string Get(string k, string d) => m.TryGetValue(k, out var v) ? v.Trim() : d;
            int I(string k, int d) => int.TryParse(Get(k, ""), System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out var v) ? v : d;
            var c = new com.oddlabs.tt.player.campaign.CampaignState(Ints(Get("islands", "")));
            c.name = m.TryGetValue("name", out var n) ? n : "";
            c.date = long.TryParse(Get("date", "0"), System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out var dt) ? dt : 0;
            c.race = I("race", 0);
            c.difficulty = I("difficulty", 0);
            c.current_island = I("current", -1);
            c.prev_island = I("prev", 0);
            c.num_peons = I("peons", 10);
            c.num_rock_warriors = I("rock", 0);
            c.num_iron_warriors = I("iron", 0);
            c.num_rubber_warriors = I("rubber", 0);
            c.has_rubber_weapons = Get("rubber_weapons", "0") == "1";
            c.has_magic0 = Get("magic0", "0") == "1";
            c.has_magic1 = Get("magic1", "0") == "1";
            return c;
        }

        public DeterministicSerializer() { }
    }
}
