// GUI port: the saved campaigns. Java writes them with Java serialization; here they are a text file (the same data,
// CampaignState.encodeAll / decodeAll) next to the settings. The callbacks run straight away. If there is no file yet,
// the campaigns.txt an earlier Unity version wrote is read instead. //added (saga)
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
                string text = com.oddlabs.tt.player.campaign.CampaignState.encodeAll(states);
                var dir = System.IO.Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dir)) System.IO.Directory.CreateDirectory(dir);
                // write next to it first, then swap in: a crash mid-write never loses the campaigns
                string tmp = path + ".tmp";
                System.IO.File.WriteAllText(tmp, text);
                if (System.IO.File.Exists(path)) System.IO.File.Replace(tmp, path, path + ".bak");
                else System.IO.File.Move(tmp, path);
                java.lang.IO.println("Saved " + states.Length + " campaign(s) to " + path);
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
                if (path == null) throw new System.IO.IOException("no game folder");
                if (!System.IO.File.Exists(path))
                {
                    string old = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(path) ?? "", "campaigns.txt");
                    if (!System.IO.File.Exists(old)) throw new System.IO.IOException("NoSuchFileException: " + path);
                    path = old;
                }
                states = com.oddlabs.tt.player.campaign.CampaignState.decodeAll(System.IO.File.ReadAllText(path));
            }
            catch (System.Exception e)
            {
                p2?.failed(e);
                return;
            }
            p2?.loadSucceeded(states);
        }

        public DeterministicSerializer() { }
    }
}
