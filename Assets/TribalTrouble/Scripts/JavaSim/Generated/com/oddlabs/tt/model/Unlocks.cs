// Translated from Unlocks.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 14
    public class Unlocks : java.lang.Object
    {
        public const int TIERS = 100;
        public const int RARE = 101;
        public const int RUNE_TIER = 102;
        public const int NUM_KEYS = 150;
        // Java: 23
        public static int building(int b)
        {
            return b;
        }
        // Java: 27
        public static int unit(int u)
        {
            return 20 + u;
        }
        // Java: 31
        public static int upgrade(int r)
        {
            return (r < 40 ? 50 + r : 110 + (r - 40));
        }
        // Java: 38
        public static int upgradeOf(int key)
        {
            if (key >= 50 && key < 90)
            {
                return key - 50;
            }
            if (key >= 110 && key < 110 + (com.oddlabs.tt.model.ResearchContainer.NUM_UPGRADES - 40))
            {
                return key - 110 + 40;
            }
            return (-1);
        }
        // Java: 46
        public static int spell(int m)
        {
            return 90 + m;
        }
        public static com.oddlabs.tt.model.DeployType[] RESEARCH;
        // Java: 66
        public static int researchOf(com.oddlabs.tt.model.DeployType type)
        {
            for (int i = 0; i < com.oddlabs.tt.model.Unlocks.RESEARCH.Length; i++)
            {
                if (((object)com.oddlabs.tt.model.Unlocks.RESEARCH[i] == (object)type))
                {
                    return i;
                }
            }
            return (-1);
        }
        // Java: 74
        public static int keyFor(com.oddlabs.tt.model.DeployType type)
        {
            if (type == null)
            {
                return (-1);
            }
            int r = com.oddlabs.tt.model.Unlocks.researchOf(type);
            if (r >= 0)
            {
                return com.oddlabs.tt.model.Unlocks.upgrade(r);
            }
            return ((type).ordinal() switch { 12 /*CHAMPION*/ => (int)(com.oddlabs.tt.model.Unlocks.unit(com.oddlabs.tt.model.Race.UNIT_CHAMPION)), 13 /*SCOUT*/ => (int)(com.oddlabs.tt.model.Unlocks.unit(com.oddlabs.tt.model.Race.UNIT_SCOUT)), 18 /*SHAMAN*/ => (int)(com.oddlabs.tt.model.Unlocks.unit(com.oddlabs.tt.model.Race.UNIT_SHAMAN)), 19 /*SLINGER*/ => (int)(com.oddlabs.tt.model.Unlocks.unit(com.oddlabs.tt.model.Race.UNIT_SLINGER)), 26 /*MELEE_LIGHT*/ => (int)(com.oddlabs.tt.model.Unlocks.unit(com.oddlabs.tt.model.Race.UNIT_MELEE_LIGHT)), 27 /*MELEE_HEAVY*/ => (int)(com.oddlabs.tt.model.Unlocks.unit(com.oddlabs.tt.model.Race.UNIT_MELEE_HEAVY)), 35 /*UNIQUE_1*/ or 50 /*CRAFT_DARTS*/ => (int)(com.oddlabs.tt.model.Unlocks.unit(com.oddlabs.tt.model.Race.UNIT_UNIQUE_1)), 36 /*UNIQUE_2*/ => (int)(com.oddlabs.tt.model.Unlocks.unit(com.oddlabs.tt.model.Race.UNIT_UNIQUE_2)), 51 /*CRYSTAL_WARRIOR*/ or 48 /*CRAFT_CRYSTAL*/ => (int)(com.oddlabs.tt.model.Unlocks.unit(com.oddlabs.tt.model.Race.UNIT_CRYSTAL)), 52 /*DIAMOND_GUARD*/ or 49 /*CRAFT_DIAMOND*/ => (int)(com.oddlabs.tt.model.Unlocks.unit(com.oddlabs.tt.model.Race.UNIT_DIAMOND)), 53 /*UNIQUE_3*/ => (int)(com.oddlabs.tt.model.Unlocks.unit(com.oddlabs.tt.model.Race.UNIT_UNIQUE_3)), 54 /*MINOR_GOD*/ => (int)(com.oddlabs.tt.model.Unlocks.unit(com.oddlabs.tt.model.Race.UNIT_MINOR_GOD)), 55 /*MAJOR_GOD*/ => (int)(com.oddlabs.tt.model.Unlocks.unit(com.oddlabs.tt.model.Race.UNIT_MAJOR_GOD)), 82 /*ELITE_1*/ => (int)(com.oddlabs.tt.model.Unlocks.unit(com.oddlabs.tt.model.Race.UNIT_ELITE_1)), 83 /*ELITE_2*/ => (int)(com.oddlabs.tt.model.Unlocks.unit(com.oddlabs.tt.model.Race.UNIT_ELITE_2)), 84 /*ELITE_3*/ => (int)(com.oddlabs.tt.model.Unlocks.unit(com.oddlabs.tt.model.Race.UNIT_ELITE_3)), 93 /*SPINNER*/ => (int)(com.oddlabs.tt.model.Unlocks.unit(com.oddlabs.tt.model.Race.UNIT_SPINNER)), 32 /*UPGRADE_STONE*/ or 33 /*UPGRADE_IRON*/ or 34 /*UPGRADE_CHICKEN*/ => (int)(com.oddlabs.tt.model.Unlocks.TIERS), 87 /*UPGRADE_RUNE*/ => (int)(com.oddlabs.tt.model.Unlocks.RUNE_TIER), 28 /*PEON_HARVEST_GOLD*/ or 29 /*PEON_HARVEST_CRYSTAL*/ or 30 /*PEON_HARVEST_HERBS*/ or 31 /*PEON_HARVEST_COAL*/ => (int)(com.oddlabs.tt.model.Unlocks.RARE), 81 /*PEON_HARVEST_DIAMOND*/ => (int)(com.oddlabs.tt.model.Unlocks.unit(com.oddlabs.tt.model.Race.UNIT_DIAMOND)), _ => (int)((-1)) });
        }
        // Java: 107
        public static void lockNewTides(com.oddlabs.tt.player.Player p)
        {
            for (int b = com.oddlabs.tt.model.Race.NUM_CLASSIC_BUILDINGS; b < com.oddlabs.tt.model.Race.NUM_BUILDINGS; b++)
            {
                p.@lock(com.oddlabs.tt.model.Unlocks.building(b));
            }
            for (int u = com.oddlabs.tt.model.Race.UNIT_CHAMPION; u < com.oddlabs.tt.model.Race.UNIT_ZOMBIE; u++)
            {
                p.@lock(com.oddlabs.tt.model.Unlocks.unit(u));
            }
            for (int u_2 = com.oddlabs.tt.model.Race.UNIT_ELITE_1; u_2 <= com.oddlabs.tt.model.Race.UNIT_ELITE_3; u_2++)
            {
                p.@lock(com.oddlabs.tt.model.Unlocks.unit(u_2));
            }
            p.@lock(com.oddlabs.tt.model.Unlocks.unit(com.oddlabs.tt.model.Race.UNIT_SPINNER));
            p.@lock(com.oddlabs.tt.model.Unlocks.RUNE_TIER);
            for (int r = 0; r < com.oddlabs.tt.model.ResearchContainer.NUM_UPGRADES; r++)
            {
                p.@lock(com.oddlabs.tt.model.Unlocks.upgrade(r));
            }
            for (int m = 2; m < com.oddlabs.tt.model.RacesResources.NUM_MAGIC; m++)
            {
                p.@lock(com.oddlabs.tt.model.Unlocks.spell(m));
            }
            p.@lock(com.oddlabs.tt.model.Unlocks.TIERS);
            p.@lock(com.oddlabs.tt.model.Unlocks.RARE);
        }
        // Java: 125
        public static void apply(com.oddlabs.tt.player.Player p)
        {
            for (int b = 0; b < com.oddlabs.tt.model.Race.NUM_BUILDINGS; b++)
            {
                if (p.isLocked(com.oddlabs.tt.model.Unlocks.building(b)))
                {
                    p.enableBuilding(b, false);
                }
            }
            for (int m = 0; m < com.oddlabs.tt.model.RacesResources.NUM_MAGIC; m++)
            {
                if (p.isLocked(com.oddlabs.tt.model.Unlocks.spell(m)))
                {
                    p.enableMagic(m, false);
                }
            }
        }
        static Unlocks()
        {
            Unlocks.RESEARCH = new com.oddlabs.tt.model.DeployType[] { com.oddlabs.tt.model.DeployType.RESEARCH_ARMOR, com.oddlabs.tt.model.DeployType.RESEARCH_SPEED, com.oddlabs.tt.model.DeployType.RESEARCH_TOOLS, com.oddlabs.tt.model.DeployType.RESEARCH_RANGE, com.oddlabs.tt.model.DeployType.RESEARCH_GROWTH, com.oddlabs.tt.model.DeployType.RESEARCH_TOWER_RANGE, com.oddlabs.tt.model.DeployType.RESEARCH_TOWER_ARMOR, com.oddlabs.tt.model.DeployType.RESEARCH_BLESSING, com.oddlabs.tt.model.DeployType.RESEARCH_LORE, com.oddlabs.tt.model.DeployType.RESEARCH_WALLS, com.oddlabs.tt.model.DeployType.RESEARCH_RACE_1, com.oddlabs.tt.model.DeployType.RESEARCH_RACE_2, com.oddlabs.tt.model.DeployType.RESEARCH_SMITHING, com.oddlabs.tt.model.DeployType.RESEARCH_DRILL, com.oddlabs.tt.model.DeployType.RESEARCH_STURDY, com.oddlabs.tt.model.DeployType.RESEARCH_BASKETS, com.oddlabs.tt.model.DeployType.RESEARCH_VOLLEY, com.oddlabs.tt.model.DeployType.RESEARCH_STAKES, com.oddlabs.tt.model.DeployType.RESEARCH_FOCUS, com.oddlabs.tt.model.DeployType.RESEARCH_PROSPECTING, com.oddlabs.tt.model.DeployType.RESEARCH_VIGOR, com.oddlabs.tt.model.DeployType.RESEARCH_CRYSTAL_CRAFT, com.oddlabs.tt.model.DeployType.RESEARCH_DIAMOND_CRAFT, com.oddlabs.tt.model.DeployType.RESEARCH_ROCK_EDGE, com.oddlabs.tt.model.DeployType.RESEARCH_IRON_TEMPER, com.oddlabs.tt.model.DeployType.RESEARCH_CHICKEN_BOUNCE, com.oddlabs.tt.model.DeployType.RESEARCH_CRYSTAL_FACET, com.oddlabs.tt.model.DeployType.RESEARCH_DIAMOND_EDGE, com.oddlabs.tt.model.DeployType.RESEARCH_SHIP_OARS, com.oddlabs.tt.model.DeployType.RESEARCH_SHIP_HULL, com.oddlabs.tt.model.DeployType.RESEARCH_ARCANE_POWER, com.oddlabs.tt.model.DeployType.RESEARCH_COURAGE_MASTERY, com.oddlabs.tt.model.DeployType.RESEARCH_FLEET, com.oddlabs.tt.model.DeployType.RESEARCH_CARGO, com.oddlabs.tt.model.DeployType.RESEARCH_BUILDINGS, com.oddlabs.tt.model.DeployType.RESEARCH_ELITE_RANKS, com.oddlabs.tt.model.DeployType.RESEARCH_ELITE_TRAINING, com.oddlabs.tt.model.DeployType.RESEARCH_GREAT_TRIBE, com.oddlabs.tt.model.DeployType.RESEARCH_BRIDGES, com.oddlabs.tt.model.DeployType.RESEARCH_RUNE_REACH, com.oddlabs.tt.model.DeployType.RESEARCH_RUNE_MIGHT, com.oddlabs.tt.model.DeployType.RESEARCH_RUNE_WARD };
        }
        public Unlocks() : base()
        {
        }
    }
}
