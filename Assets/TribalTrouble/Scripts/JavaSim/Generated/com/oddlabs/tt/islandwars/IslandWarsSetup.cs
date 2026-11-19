// Translated from IslandWarsSetup.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.islandwars
{
    // Java: 15
    public class IslandWarsSetup : java.lang.Object
    {
        public const int HOME = 0;
        public const int RAID = 1;
        public const int MATCH = 2;
        public const float RAID_SECONDS = 600.0f;
        public const int RAID_BASE_BAND = 10;
        public int kind;
        public int defender;
        public int attacker;
        public int race;
        public int[] buildings;
        public int[] loot;
        public java.util.Map upgrades_f;
        public java.util.Map names;
        // Java: 31
        public int[] upgrades(int slot)
        {
            int[] u = ((int[])upgrades_f.get(slot));
            return (u != null ? u : new int[com.oddlabs.tt.islandwars.IslandUpgrades.NUM]);
        }
        // Java: 36
        public void setUpgrades(int slot, int[] levels, string name_v)
        {
            int[] u = new int[com.oddlabs.tt.islandwars.IslandUpgrades.NUM];
            for (int i = 0; i < u.Length && i < levels.Length; i++)
            {
                u[i] = java.lang.Math.clamp(levels[i], 0, com.oddlabs.tt.islandwars.IslandUpgrades.maxLevel(i));
            }
            upgrades_f.put(slot, u);
            names.put(slot, name_v.replace('|', '_').replace('=', '_'));
        }
        // Java: 44
        public string name(int slot)
        {
            return ((string)names.getOrDefault(slot, "?"));
        }
        // Java: 48
        public bool hasSlot(int slot)
        {
            return upgrades_f.containsKey(slot);
        }
        // Java: 52
        public static com.oddlabs.tt.islandwars.IslandWarsSetup home(com.oddlabs.tt.islandwars.HomeIsland island)
        {
            com.oddlabs.tt.islandwars.IslandWarsSetup s = new com.oddlabs.tt.islandwars.IslandWarsSetup();
            s.kind = com.oddlabs.tt.islandwars.IslandWarsSetup.HOME;
            s.defender = 0;
            s.race = island.race;
            s.buildings = ((int[])island.buildings.clone());
            s.setUpgrades(0, island.upgrades, island.nick);
            return s;
        }
        // Java: 63
        public static com.oddlabs.tt.islandwars.IslandWarsSetup raid(com.oddlabs.tt.islandwars.HomeIsland island, int[] attacker_upgrades, string attacker, bool island_suspicious)
        {
            com.oddlabs.tt.islandwars.IslandWarsSetup s = new com.oddlabs.tt.islandwars.IslandWarsSetup();
            s.kind = com.oddlabs.tt.islandwars.IslandWarsSetup.RAID;
            s.attacker = 0;
            s.defender = 1;
            s.race = island.race;
            s.buildings = ((int[])island.buildings.clone());
            s.setUpgrades(0, attacker_upgrades, attacker);
            s.setUpgrades(1, (island_suspicious ? new int[com.oddlabs.tt.islandwars.IslandUpgrades.NUM] : island.upgrades), island.nick);
            int[] l = island.lootable();
            java.lang.JSystem.arraycopy(l, 0, s.loot, 0, 3);
            return s;
        }
        // Java: 79
        public com.oddlabs.tt.player.UnitInfo unitInfo(int slot)
        {
            int[] up = upgrades(slot);
            int[] st = com.oddlabs.tt.islandwars.IslandUpgrades.start(up);
            bool chief = up[com.oddlabs.tt.islandwars.IslandUpgrades.CHIEF_HALL] > 0;
            return ((kind) switch { 1 => (com.oddlabs.tt.player.UnitInfo)((slot == attacker ? new com.oddlabs.tt.player.UnitInfo(false, false, 0, 0, chief, 0, com.oddlabs.tt.islandwars.IslandWarsSetup.RAID_BASE_BAND + 4 * up[com.oddlabs.tt.islandwars.IslandUpgrades.RAIDING_FLEET] + st[1], st[2] + up[com.oddlabs.tt.islandwars.IslandUpgrades.RAIDING_FLEET], st[3]) : new com.oddlabs.tt.player.UnitInfo(buildings.Length == 0, buildings.Length == 0, 0, 0, chief, st[0], st[1], st[2], st[3]))), 0 => (com.oddlabs.tt.player.UnitInfo)(new com.oddlabs.tt.player.UnitInfo(buildings.Length == 0, buildings.Length == 0, 0, 0, true, st[0] + 4, 0, 0, 0)), _ => (com.oddlabs.tt.player.UnitInfo)(new com.oddlabs.tt.player.UnitInfo(true, true, 0, st[4], chief, st[0], st[1], st[2], st[3])) });
        }
        // Java: 93
        public string encode()
        {
            java.lang.StringBuilder sb = new java.lang.StringBuilder("iw1");
            sb.append("|k=").append(kind).append("|d=").append(defender).append("|a=").append(attacker).append("|r=").append(race);
            sb.append("|b=").append(com.oddlabs.tt.islandwars.HomeIsland.ints(buildings)).append("|l=").append(com.oddlabs.tt.islandwars.HomeIsland.ints(loot));
            foreach (java.util.Map.Entry e in java.lang.JIter.of(upgrades_f.entrySet()))
            {
                sb.append("|u").append(((int?)e.getKey())).append('=').append(com.oddlabs.tt.islandwars.HomeIsland.ints(((int[])e.getValue())));
            }
            foreach (java.util.Map.Entry e_2 in java.lang.JIter.of(names.entrySet()))
            {
                sb.append("|n").append(((int?)e_2.getKey())).append('=').append(((string)e_2.getValue()));
            }
            return java.lang.JO.toString(sb);
        }
        // Java: 104
        public static com.oddlabs.tt.islandwars.IslandWarsSetup decode(string text)
        {
            if (!text.startsWith("iw1"))
            {
                return null;
            }
            com.oddlabs.tt.islandwars.IslandWarsSetup s = new com.oddlabs.tt.islandwars.IslandWarsSetup();
            foreach (string part in text.split("\\|"))
            {
                int eq = part.indexOf('=');
                if (eq <= 0)
                {
                    continue;
                }
                string k = part.substring(0, eq);
                string v = part.substring(eq + 1);
                try
                {
                    switch ((k))
                    {
                        case "k":
                        {
                            s.kind = java.lang.Integer.parseInt(v);
                            break;
                        }
                        case "d":
                        {
                            s.defender = java.lang.Integer.parseInt(v);
                            break;
                        }
                        case "a":
                        {
                            s.attacker = java.lang.Integer.parseInt(v);
                            break;
                        }
                        case "r":
                        {
                            s.race = java.lang.Integer.parseInt(v);
                            break;
                        }
                        case "b":
                        {
                            int[] b = com.oddlabs.tt.islandwars.HomeIsland.ints(v);
                            s.buildings = (b.Length % 4 == 0 ? b : new int[0]);
                            break;
                        }
                        case "l":
                        {
                            int[] l = com.oddlabs.tt.islandwars.HomeIsland.ints(v);
                            for (int i = 0; i < 3 && i < l.Length; i++)
                            {
                                s.loot[i] = java.lang.Math.max(0, l[i]);
                            }
                            break;
                        }
                        default:
                        {
                            if (k.charAt(0) == 'u')
                            {
                                s.upgrades_f.put(java.lang.Integer.parseInt(k.substring(1)), com.oddlabs.tt.islandwars.HomeIsland.ints(v));
                            }
                            else if (k.charAt(0) == 'n')
                            {
                                s.names.put(java.lang.Integer.parseInt(k.substring(1)), v);
                            }
                            break;
                        }
                    }
                }
                catch (System.Exception e)
                {
                }
            }
            foreach (java.util.Map.Entry e_2 in java.lang.JIter.of(s.upgrades_f.entrySet()))
            {
                int[] u = new int[com.oddlabs.tt.islandwars.IslandUpgrades.NUM];
                for (int i_2 = 0; i_2 < u.Length && i_2 < ((int[])e_2.getValue()).Length; i_2++)
                {
                    u[i_2] = java.lang.Math.clamp(((int[])e_2.getValue())[i_2], 0, com.oddlabs.tt.islandwars.IslandUpgrades.maxLevel(i_2));
                }
                e_2.setValue(u);
            }
            return s;
        }
        private void init_IslandWarsSetup()
        {
            this.defender = (-1);
            this.attacker = (-1);
            this.buildings = new int[0];
            this.loot = new int[3];
            this.upgrades_f = new java.util.LinkedHashMap();
            this.names = new java.util.LinkedHashMap();
        }
        public IslandWarsSetup() : base()
        {
            init_IslandWarsSetup();
        }
    }
}
