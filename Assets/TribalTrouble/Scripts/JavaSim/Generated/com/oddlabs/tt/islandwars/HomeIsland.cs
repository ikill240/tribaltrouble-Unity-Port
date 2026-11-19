// Translated from HomeIsland.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.islandwars
{
    // Java: 20
    public class HomeIsland : java.lang.Object
    {
        public const float SHIELD_DAMAGE = 0.75f;
        public const long SHIELD_MILLIS = 172800000L;
        public static int[] START_TREASURY;
        public static int[] GAME_PRIZE_CAP;
        public static int[] RAID_LOOT_CAP;
        public const float LOOT_SHARE = 0.25f;
        public string nick;
        public int race;
        public int terrain;
        public int seed;
        public int size;
        public int[] treasury;
        public int[] upgrades;
        public int[] buildings;
        public long shield_until;
        // the wood and stone the stays at home have used up (per cent), and when it was first cut into //added (regrowth)
        public int depletion;
        public long depleted_at;
        public const long REGROW_MILLIS = 12L * 3600L * 1000L;
        public float resourcesLeft(long now)
        {
            if (depletion > 0 && (depleted_at <= 0 || now - depleted_at >= com.oddlabs.tt.islandwars.HomeIsland.REGROW_MILLIS || now < depleted_at))
            {
                depletion = 0;
                depleted_at = 0;
            }
            return java.lang.Math.max(0.25f, 1.0f - depletion / 100.0f);
        }
        public void setDepletion(int percent, long now)
        {
            int p = java.lang.Math.clamp(percent, 0, 75);
            if (p <= 0)
            {
                depleted_at = 0;
            }
            else if (depletion <= 0 || depleted_at <= 0)
            {
                depleted_at = now;
            }
            depletion = p;
        }
        public java.util.List ledger;
        public bool tampered;
        // Java: 51
        public static int seedOf(string nick)
        {
            int h = 625341585;
            foreach (char c in nick.toLowerCase().toCharArray())
            {
                h = h * 31 + c;
            }
            return h & 2147483647;
        }
        // Java: 58
        public bool isTampered()
        {
            return tampered;
        }
        // Java: 62
        public bool isShielded(long now)
        {
            return shield_until > now;
        }
        // Java: 66
        public int buildingCap()
        {
            return com.oddlabs.tt.islandwars.IslandUpgrades.SIZE_BUILDINGS[size];
        }
        // Java: 70
        public int metersPerWorld()
        {
            return com.oddlabs.tt.islandwars.IslandUpgrades.SIZE_METERS[size];
        }
        // Java: 76
        public bool canAfford(int[] cost)
        {
            for (int i = 0; i < 3; i++)
            {
                if (treasury[i] < cost[i])
                {
                    return false;
                }
            }
            return true;
        }
        // Java: 84
        public bool buyUpgrade(int upgrade, long now)
        {
            int level = upgrades[upgrade];
            if (level >= com.oddlabs.tt.islandwars.IslandUpgrades.maxLevel(upgrade))
            {
                return false;
            }
            int[] c = com.oddlabs.tt.islandwars.IslandUpgrades.cost(upgrade, level);
            if (!canAfford(c))
            {
                return false;
            }
            for (int i = 0; i < 3; i++)
            {
                treasury[i] -= c[i];
            }
            upgrades[upgrade]++;
            ledger.add((now) + ":U:" + (upgrade) + ":" + (upgrades[upgrade]));
            return true;
        }
        // Java: 98
        public bool growIsland(long now)
        {
            if (size >= com.oddlabs.tt.islandwars.IslandUpgrades.maxSize())
            {
                return false;
            }
            int[] c = com.oddlabs.tt.islandwars.IslandUpgrades.sizeCost(size + 1);
            if (!canAfford(c))
            {
                return false;
            }
            for (int i = 0; i < 3; i++)
            {
                treasury[i] -= c[i];
            }
            size++;
            ledger.add((now) + ":Z:" + (size));
            return true;
        }
        // Java: 112
        public void earn(char kind, int[] gain, string against, long now)
        {
            int[] cap = (kind == 'R' ? com.oddlabs.tt.islandwars.HomeIsland.RAID_LOOT_CAP : com.oddlabs.tt.islandwars.HomeIsland.GAME_PRIZE_CAP);
            int[] g = new int[3];
            for (int i = 0; i < 3; i++)
            {
                g[i] = java.lang.Math.clamp(gain[i], 0, cap[i]);
                treasury[i] += g[i];
            }
            ledger.add((now) + ":" + (kind).ToString() + ":" + (g[0]) + ":" + (g[1]) + ":" + (g[2]) + ":" + java.lang.JStr.valueOf(com.oddlabs.tt.islandwars.HomeIsland.clean(against)));
        }
        // Java: 123
        public void lose(int[] loss, string against, long now)
        {
            int[] l = new int[3];
            for (int i = 0; i < 3; i++)
            {
                l[i] = java.lang.Math.clamp(loss[i], 0, treasury[i]);
                treasury[i] -= l[i];
            }
            ledger.add((now) + ":L:" + (l[0]) + ":" + (l[1]) + ":" + (l[2]) + ":" + java.lang.JStr.valueOf(com.oddlabs.tt.islandwars.HomeIsland.clean(against)));
        }
        // Java: 133
        public int[] lootable()
        {
            int[] l = new int[3];
            for (int i = 0; i < 3; i++)
            {
                l[i] = java.lang.Math.min(com.oddlabs.tt.islandwars.HomeIsland.RAID_LOOT_CAP[i], java.lang.JC.f2i(((float)(treasury[i] * com.oddlabs.tt.islandwars.HomeIsland.LOOT_SHARE))));
            }
            return l;
        }
        // Java: 140
        public static string clean(string s)
        {
            return s.replace(':', '_').replace('\n', ' ').replace('|', '_').replace(';', '_');
        }
        // Java: 151
        public string audit()
        {
            if (tampered)
            {
                return "file edited";
            }
            long[] @in = new long[] { com.oddlabs.tt.islandwars.HomeIsland.START_TREASURY[0], com.oddlabs.tt.islandwars.HomeIsland.START_TREASURY[1], com.oddlabs.tt.islandwars.HomeIsland.START_TREASURY[2] };
            int[] lv = new int[com.oddlabs.tt.islandwars.IslandUpgrades.NUM];
            int sz = 0;
            long last = 0;
            foreach (string e in java.lang.JIter.of(ledger))
            {
                string[] p = e.split(":");
                if (p.Length < 3)
                {
                    return "bad ledger";
                }
                long t = default;
                try
                {
                    t = java.lang.Long.parseLong(p[0]);
                }
                catch (System.FormatException x)
                {
                    return "bad ledger";
                }
                if (t < last)
                {
                    return "ledger out of order";
                }
                last = t;
                try
                {
                    switch ((p[1]))
                    {
                        case "U":
                        {
                            int u = java.lang.Integer.parseInt(p[2]);
                            int l = java.lang.Integer.parseInt(p[3]);
                            if (u < 0 || u >= com.oddlabs.tt.islandwars.IslandUpgrades.NUM || l != lv[u] + 1 || l > com.oddlabs.tt.islandwars.IslandUpgrades.maxLevel(u))
                            {
                                return "upgrade out of turn";
                            }
                            int[] c = com.oddlabs.tt.islandwars.IslandUpgrades.cost(u, lv[u]);
                            for (int i = 0; i < 3; i++)
                            {
                                @in[i] -= c[i];
                            }
                            lv[u] = l;
                            break;
                        }
                        case "Z":
                        {
                            int s = java.lang.Integer.parseInt(p[2]);
                            if (s != sz + 1 || s > com.oddlabs.tt.islandwars.IslandUpgrades.maxSize())
                            {
                                return "size out of turn";
                            }
                            int[] c_2 = com.oddlabs.tt.islandwars.IslandUpgrades.sizeCost(s);
                            for (int i_2 = 0; i_2 < 3; i_2++)
                            {
                                @in[i_2] -= c_2[i_2];
                            }
                            sz = s;
                            break;
                        }
                        case "W":
                        case "P":
                        case "D":
                        case "R":
                        {
                            int[] cap = (java.lang.JO.equals(p[1], "R") ? com.oddlabs.tt.islandwars.HomeIsland.RAID_LOOT_CAP : com.oddlabs.tt.islandwars.HomeIsland.GAME_PRIZE_CAP);
                            for (int i_3 = 0; i_3 < 3; i_3++)
                            {
                                int g = java.lang.Integer.parseInt(p[2 + i_3]);
                                if (g < 0 || g > cap[i_3])
                                {
                                    return "a game paid too much";
                                }
                                @in[i_3] += g;
                            }
                            break;
                        }
                        case "L":
                        {
                            for (int i_4 = 0; i_4 < 3; i_4++)
                            {
                                @in[i_4] -= java.lang.Integer.parseInt(p[2 + i_4]);
                            }
                            break;
                        }
                        default:
                        {
                            return "bad ledger";
                        }
                    }
                }
                catch (System.Exception x_2)
                {
                    return "bad ledger";
                }
                foreach (long v in @in)
                {
                    if (v < 0)
                    {
                        return "spent more than it had";
                    }
                }
            }
            for (int i_5 = 0; i_5 < 3; i_5++)
            {
                if (@in[i_5] != treasury[i_5])
                {
                    return "treasury doesn't add up";
                }
            }
            for (int u_2 = 0; u_2 < com.oddlabs.tt.islandwars.IslandUpgrades.NUM; u_2++)
            {
                if (lv[u_2] != upgrades[u_2])
                {
                    return "upgrades not paid for";
                }
            }
            if (sz != size)
            {
                return "size not paid for";
            }
            if (buildings.Length / 4 > buildingCap())
            {
                return "too many buildings";
            }
            return null;
        }
        // Java: 231
        public string encode()
        {
            java.util.Map m = new java.util.LinkedHashMap();
            m.put("v", "1");
            m.put("nick", nick);
            m.put("race", java.lang.Integer.toString(race));
            m.put("terrain", java.lang.Integer.toString(terrain));
            m.put("seed", java.lang.Integer.toString(seed));
            m.put("size", java.lang.Integer.toString(size));
            m.put("treasury", com.oddlabs.tt.islandwars.HomeIsland.ints(treasury));
            m.put("upgrades", com.oddlabs.tt.islandwars.HomeIsland.ints(upgrades));
            m.put("buildings", com.oddlabs.tt.islandwars.HomeIsland.ints(buildings));
            m.put("shield", java.lang.Long.toString(shield_until));
            m.put("depletion", java.lang.Integer.toString(depletion));
            m.put("depleted_at", java.lang.Long.toString(depleted_at));
            m.put("ledger", java.lang.JString.join(";", ledger));
            java.lang.StringBuilder sb = new java.lang.StringBuilder();
            foreach (java.util.Map.Entry e in java.lang.JIter.of(m.entrySet()))
            {
                sb.append(((string)e.getKey())).append('=').append(((string)e.getValue())).append('\n');
            }
            return java.lang.JO.toString(sb);
        }
        // Java: 250
        public static com.oddlabs.tt.islandwars.HomeIsland decode(string text)
        {
            java.util.Map m = new java.util.LinkedHashMap();
            foreach (string line in text.split("\n"))
            {
                int eq = line.indexOf('=');
                if (eq > 0)
                {
                    m.put(line.substring(0, eq).trim(), line.substring(eq + 1).trim());
                }
            }
            com.oddlabs.tt.islandwars.HomeIsland h = new com.oddlabs.tt.islandwars.HomeIsland(((string)m.getOrDefault("nick", "?")));
            h.race = com.oddlabs.tt.islandwars.HomeIsland.parse(((string)m.get("race")));
            h.terrain = com.oddlabs.tt.islandwars.HomeIsland.parse(((string)m.get("terrain")));
            h.seed = (m.containsKey("seed") ? com.oddlabs.tt.islandwars.HomeIsland.parse(((string)m.get("seed"))) : h.seed);
            h.size = java.lang.Math.clamp(com.oddlabs.tt.islandwars.HomeIsland.parse(((string)m.get("size"))), 0, com.oddlabs.tt.islandwars.IslandUpgrades.maxSize());
            com.oddlabs.tt.islandwars.HomeIsland.copy(com.oddlabs.tt.islandwars.HomeIsland.ints(((string)m.get("treasury"))), h.treasury);
            com.oddlabs.tt.islandwars.HomeIsland.copy(com.oddlabs.tt.islandwars.HomeIsland.ints(((string)m.get("upgrades"))), h.upgrades);
            int[] b = com.oddlabs.tt.islandwars.HomeIsland.ints(((string)m.get("buildings")));
            h.buildings = (b.Length % 4 == 0 ? b : new int[0]);
            try
            {
                h.shield_until = java.lang.Long.parseLong(((string)m.getOrDefault("shield", "0")).trim());
            }
            catch (System.FormatException e)
            {
                h.shield_until = 0;
            }
            h.depletion = java.lang.Math.clamp(com.oddlabs.tt.islandwars.HomeIsland.parse(((string)m.get("depletion"))), 0, 75);
            try
            {
                h.depleted_at = java.lang.Long.parseLong(((string)m.getOrDefault("depleted_at", "0")).trim());
            }
            catch (System.FormatException e)
            {
                h.depleted_at = 0;
            }
            string l = ((string)m.getOrDefault("ledger", ""));
            if (!l.isEmpty())
            {
                foreach (string e_2 in l.split(";"))
                {
                    h.ledger.add(e_2);
                }
            }
            return h;
        }
        // Java: 278
        public static int parse(string s)
        {
            try
            {
                return (s == null ? 0 : java.lang.Integer.parseInt(s.trim()));
            }
            catch (System.FormatException e)
            {
                return 0;
            }
        }
        // Java: 286
        public static void copy(int[] from, int[] to)
        {
            for (int i = 0; i < to.Length && i < from.Length; i++)
            {
                to[i] = from[i];
            }
        }
        // Java: 291
        public static string ints(int[] a)
        {
            java.lang.StringBuilder sb = new java.lang.StringBuilder();
            for (int i = 0; i < a.Length; i++)
            {
                sb.append((i == 0 ? "" : ",")).append(a[i]);
            }
            return java.lang.JO.toString(sb);
        }
        // Java: 298
        public static int[] ints(string s)
        {
            if (s == null || s.isBlank())
            {
                return new int[0];
            }
            string[] p = s.trim().split(",");
            int[] a = new int[p.Length];
            for (int i = 0; i < p.Length; i++)
            {
                a[i] = com.oddlabs.tt.islandwars.HomeIsland.parse(p[i]);
            }
            return a;
        }
        // Java: 310
        public static java.nio.file.Path dir()
        {
            if (com.oddlabs.tt.render.Renderer.getLocalInput() == null || com.oddlabs.tt.render.Renderer.getLocalInput().getGameDir() == null)
            {
                return null;
            }
            return com.oddlabs.tt.render.Renderer.getLocalInput().getGameDir().resolve("islandwars");
        }
        // Java: 316
        public static string secret()
        {
            java.nio.file.Path d = com.oddlabs.tt.islandwars.HomeIsland.dir();
            if (d == null)
            {
                return "no-dir";
            }
            java.nio.file.Path f = d.resolve("island.key");
            try
            {
                if (java.nio.file.Files.exists(f))
                {
                    return java.nio.file.Files.readString(f).trim();
                }
                java.nio.file.Files.createDirectories(d);
                long x = java.lang.JSystem.nanoTime() ^ java.lang.JSystem.currentTimeMillis() * (-7046029254386353131L) ^ ((long)(java.lang.JO.hashCode(d))) << 17;
                java.util.Random r = new java.util.Random(x);
                java.lang.StringBuilder sb = new java.lang.StringBuilder();
                for (int i = 0; i < 4; i++)
                {
                    sb.append(java.lang.Long.toHexString(r.nextLong() ^ java.lang.JSystem.nanoTime()));
                }
                string key = com.oddlabs.tt.islandwars.Sha256.hex(com.oddlabs.tt.islandwars.Sha256.hash(com.oddlabs.tt.islandwars.Sha256.utf8(java.lang.JO.toString(sb))));
                java.nio.file.Files.writeString(f, key);
                return key;
            }
            catch (System.Exception e)
            {
                return "no-key";
            }
        }
        // Java: 338
        public static java.nio.file.Path file(string nick)
        {
            java.nio.file.Path d = com.oddlabs.tt.islandwars.HomeIsland.dir();
            return (d == null ? (java.nio.file.Path)(null) : d.resolve(java.lang.JStr.valueOf(nick.toLowerCase().replaceAll("[^a-z0-9_]", "_")) + ".island"));
        }
        // Java: 344
        public static com.oddlabs.tt.islandwars.HomeIsland load(string nick, int race)
        {
            java.nio.file.Path f = com.oddlabs.tt.islandwars.HomeIsland.file(nick);
            try
            {
                if (f != null && java.nio.file.Files.exists(f))
                {
                    string text = java.nio.file.Files.readString(f);
                    int s = text.lastIndexOf("sig=");
                    string body = (s >= 0 ? text.substring(0, s) : text);
                    string sig = (s >= 0 ? text.substring(s + 4).trim() : "");
                    com.oddlabs.tt.islandwars.HomeIsland h = com.oddlabs.tt.islandwars.HomeIsland.decode(body);
                    h.tampered = !java.lang.JO.equals(sig, com.oddlabs.tt.islandwars.Sha256.hmacHex(java.lang.JStr.valueOf(com.oddlabs.tt.islandwars.HomeIsland.secret()) + "|" + java.lang.JStr.valueOf(nick.toLowerCase()), body));
                    return h;
                }
            }
            catch (System.Exception e)
            {
            }
            com.oddlabs.tt.islandwars.HomeIsland h_2 = new com.oddlabs.tt.islandwars.HomeIsland(nick);
            h_2.race = race;
            for (int i = 0; i < 3; i++)
            {
                h_2.treasury[i] = com.oddlabs.tt.islandwars.HomeIsland.START_TREASURY[i];
            }
            return h_2;
        }
        // Java: 366
        public void save()
        {
            java.nio.file.Path f = com.oddlabs.tt.islandwars.HomeIsland.file(nick);
            if (f == null)
            {
                return;
            }
            try
            {
                java.nio.file.Files.createDirectories(f.getParent());
                string body = encode();
                java.nio.file.Files.writeString(f, java.lang.JStr.valueOf(body) + "sig=" + java.lang.JStr.valueOf(com.oddlabs.tt.islandwars.Sha256.hmacHex(java.lang.JStr.valueOf(com.oddlabs.tt.islandwars.HomeIsland.secret()) + "|" + java.lang.JStr.valueOf(nick.toLowerCase()), body)) + "\n");
            }
            catch (System.Exception e)
            {
                java.lang.IO.println("Island Wars: couldn't save the island: " + java.lang.JStr.valueOf(e));
            }
        }
        // Java: 380
        public static com.oddlabs.tt.islandwars.HomeIsland fromPeer(string text)
        {
            return com.oddlabs.tt.islandwars.HomeIsland.decode(text);
        }
        static HomeIsland()
        {
            HomeIsland.START_TREASURY = new int[] { 200, 2, 6 };
            HomeIsland.GAME_PRIZE_CAP = new int[] { 120, 3, 10 };
            HomeIsland.RAID_LOOT_CAP = new int[] { 2500, 25, 80 };
        }
        private void init_HomeIsland()
        {
            this.treasury = new int[3];
            this.upgrades = new int[com.oddlabs.tt.islandwars.IslandUpgrades.NUM];
            this.buildings = new int[0];
            this.ledger = new java.util.ArrayList();
        }
        public HomeIsland(string nick) : base()
        {
            init_HomeIsland();
            this.nick = nick;
            this.seed = com.oddlabs.tt.islandwars.HomeIsland.seedOf(nick);
        }
    }
}
