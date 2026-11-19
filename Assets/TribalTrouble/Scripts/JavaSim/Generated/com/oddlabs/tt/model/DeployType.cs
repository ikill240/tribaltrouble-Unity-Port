// Translated from DeployType.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 3
    public sealed class DeployType : java.lang.Enum
    {
        public static DeployType[] values() { return (DeployType[])VALUES_.Clone(); }
        public static DeployType valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
        public static readonly DeployType ROCK_WARRIOR = new DeployType("ROCK_WARRIOR", 0);
        public static readonly DeployType IRON_WARRIOR = new DeployType("IRON_WARRIOR", 1);
        public static readonly DeployType RUBBER_WARRIOR = new DeployType("RUBBER_WARRIOR", 2);
        public static readonly DeployType PEON = new DeployType("PEON", 3);
        public static readonly DeployType PEON_HARVEST_TREE = new DeployType("PEON_HARVEST_TREE", 4);
        public static readonly DeployType PEON_TRANSPORT_TREE = new DeployType("PEON_TRANSPORT_TREE", 5);
        public static readonly DeployType PEON_HARVEST_ROCK = new DeployType("PEON_HARVEST_ROCK", 6);
        public static readonly DeployType PEON_TRANSPORT_ROCK = new DeployType("PEON_TRANSPORT_ROCK", 7);
        public static readonly DeployType PEON_HARVEST_IRON = new DeployType("PEON_HARVEST_IRON", 8);
        public static readonly DeployType PEON_TRANSPORT_IRON = new DeployType("PEON_TRANSPORT_IRON", 9);
        public static readonly DeployType PEON_HARVEST_RUBBER = new DeployType("PEON_HARVEST_RUBBER", 10);
        public static readonly DeployType PEON_TRANSPORT_RUBBER = new DeployType("PEON_TRANSPORT_RUBBER", 11);
        public static readonly DeployType CHAMPION = new DeployType("CHAMPION", 12);
        public static readonly DeployType SCOUT = new DeployType("SCOUT", 13);
        public static readonly DeployType RESEARCH_ARMOR = new DeployType("RESEARCH_ARMOR", 14);
        public static readonly DeployType RESEARCH_SPEED = new DeployType("RESEARCH_SPEED", 15);
        public static readonly DeployType RESEARCH_TOOLS = new DeployType("RESEARCH_TOOLS", 16);
        public static readonly DeployType RESEARCH_RANGE = new DeployType("RESEARCH_RANGE", 17);
        public static readonly DeployType SHAMAN = new DeployType("SHAMAN", 18);
        public static readonly DeployType SLINGER = new DeployType("SLINGER", 19);
        public static readonly DeployType RESEARCH_GROWTH = new DeployType("RESEARCH_GROWTH", 20);
        public static readonly DeployType RESEARCH_TOWER_RANGE = new DeployType("RESEARCH_TOWER_RANGE", 21);
        public static readonly DeployType RESEARCH_TOWER_ARMOR = new DeployType("RESEARCH_TOWER_ARMOR", 22);
        public static readonly DeployType RESEARCH_BLESSING = new DeployType("RESEARCH_BLESSING", 23);
        public static readonly DeployType RESEARCH_LORE = new DeployType("RESEARCH_LORE", 24);
        public static readonly DeployType RESEARCH_WALLS = new DeployType("RESEARCH_WALLS", 25);
        public static readonly DeployType MELEE_LIGHT = new DeployType("MELEE_LIGHT", 26);
        public static readonly DeployType MELEE_HEAVY = new DeployType("MELEE_HEAVY", 27);
        public static readonly DeployType PEON_HARVEST_GOLD = new DeployType("PEON_HARVEST_GOLD", 28);
        public static readonly DeployType PEON_HARVEST_CRYSTAL = new DeployType("PEON_HARVEST_CRYSTAL", 29);
        public static readonly DeployType PEON_HARVEST_HERBS = new DeployType("PEON_HARVEST_HERBS", 30);
        public static readonly DeployType PEON_HARVEST_COAL = new DeployType("PEON_HARVEST_COAL", 31);
        public static readonly DeployType UPGRADE_STONE = new DeployType("UPGRADE_STONE", 32);
        public static readonly DeployType UPGRADE_IRON = new DeployType("UPGRADE_IRON", 33);
        public static readonly DeployType UPGRADE_CHICKEN = new DeployType("UPGRADE_CHICKEN", 34);
        public static readonly DeployType UNIQUE_1 = new DeployType("UNIQUE_1", 35);
        public static readonly DeployType UNIQUE_2 = new DeployType("UNIQUE_2", 36);
        public static readonly DeployType RESEARCH_RACE_1 = new DeployType("RESEARCH_RACE_1", 37);
        public static readonly DeployType RESEARCH_RACE_2 = new DeployType("RESEARCH_RACE_2", 38);
        public static readonly DeployType RESEARCH_SMITHING = new DeployType("RESEARCH_SMITHING", 39);
        public static readonly DeployType RESEARCH_DRILL = new DeployType("RESEARCH_DRILL", 40);
        public static readonly DeployType RESEARCH_STURDY = new DeployType("RESEARCH_STURDY", 41);
        public static readonly DeployType RESEARCH_BASKETS = new DeployType("RESEARCH_BASKETS", 42);
        public static readonly DeployType RESEARCH_VOLLEY = new DeployType("RESEARCH_VOLLEY", 43);
        public static readonly DeployType RESEARCH_STAKES = new DeployType("RESEARCH_STAKES", 44);
        public static readonly DeployType RESEARCH_FOCUS = new DeployType("RESEARCH_FOCUS", 45);
        public static readonly DeployType RESEARCH_PROSPECTING = new DeployType("RESEARCH_PROSPECTING", 46);
        public static readonly DeployType RESEARCH_VIGOR = new DeployType("RESEARCH_VIGOR", 47);
        public static readonly DeployType CRAFT_CRYSTAL = new DeployType("CRAFT_CRYSTAL", 48);
        public static readonly DeployType CRAFT_DIAMOND = new DeployType("CRAFT_DIAMOND", 49);
        public static readonly DeployType CRAFT_DARTS = new DeployType("CRAFT_DARTS", 50);
        public static readonly DeployType CRYSTAL_WARRIOR = new DeployType("CRYSTAL_WARRIOR", 51);
        public static readonly DeployType DIAMOND_GUARD = new DeployType("DIAMOND_GUARD", 52);
        public static readonly DeployType UNIQUE_3 = new DeployType("UNIQUE_3", 53);
        public static readonly DeployType MINOR_GOD = new DeployType("MINOR_GOD", 54);
        public static readonly DeployType MAJOR_GOD = new DeployType("MAJOR_GOD", 55);
        public static readonly DeployType RESEARCH_CRYSTAL_CRAFT = new DeployType("RESEARCH_CRYSTAL_CRAFT", 56);
        public static readonly DeployType RESEARCH_DIAMOND_CRAFT = new DeployType("RESEARCH_DIAMOND_CRAFT", 57);
        public static readonly DeployType RESEARCH_ROCK_EDGE = new DeployType("RESEARCH_ROCK_EDGE", 58);
        public static readonly DeployType RESEARCH_IRON_TEMPER = new DeployType("RESEARCH_IRON_TEMPER", 59);
        public static readonly DeployType RESEARCH_CHICKEN_BOUNCE = new DeployType("RESEARCH_CHICKEN_BOUNCE", 60);
        public static readonly DeployType RESEARCH_CRYSTAL_FACET = new DeployType("RESEARCH_CRYSTAL_FACET", 61);
        public static readonly DeployType RESEARCH_DIAMOND_EDGE = new DeployType("RESEARCH_DIAMOND_EDGE", 62);
        public static readonly DeployType RESEARCH_SHIP_OARS = new DeployType("RESEARCH_SHIP_OARS", 63);
        public static readonly DeployType RESEARCH_SHIP_HULL = new DeployType("RESEARCH_SHIP_HULL", 64);
        public static readonly DeployType MARKET_WOOD = new DeployType("MARKET_WOOD", 65);
        public static readonly DeployType MARKET_ROCK = new DeployType("MARKET_ROCK", 66);
        public static readonly DeployType MARKET_IRON = new DeployType("MARKET_IRON", 67);
        public static readonly DeployType MARKET_CHICKEN = new DeployType("MARKET_CHICKEN", 68);
        public static readonly DeployType MARKET_CRYSTAL = new DeployType("MARKET_CRYSTAL", 69);
        public static readonly DeployType MARKET_HERBS = new DeployType("MARKET_HERBS", 70);
        public static readonly DeployType MARKET_COAL = new DeployType("MARKET_COAL", 71);
        public static readonly DeployType GIFT_WOOD = new DeployType("GIFT_WOOD", 72);
        public static readonly DeployType GIFT_ROCK = new DeployType("GIFT_ROCK", 73);
        public static readonly DeployType GIFT_IRON = new DeployType("GIFT_IRON", 74);
        public static readonly DeployType GIFT_GOLD = new DeployType("GIFT_GOLD", 75);
        public static readonly DeployType RESEARCH_ARCANE_POWER = new DeployType("RESEARCH_ARCANE_POWER", 76);
        public static readonly DeployType RESEARCH_COURAGE_MASTERY = new DeployType("RESEARCH_COURAGE_MASTERY", 77);
        public static readonly DeployType RESEARCH_FLEET = new DeployType("RESEARCH_FLEET", 78);
        public static readonly DeployType RESEARCH_CARGO = new DeployType("RESEARCH_CARGO", 79);
        public static readonly DeployType RESEARCH_BUILDINGS = new DeployType("RESEARCH_BUILDINGS", 80);
        public static readonly DeployType PEON_HARVEST_DIAMOND = new DeployType("PEON_HARVEST_DIAMOND", 81);
        public static readonly DeployType ELITE_1 = new DeployType("ELITE_1", 82);
        public static readonly DeployType ELITE_2 = new DeployType("ELITE_2", 83);
        public static readonly DeployType ELITE_3 = new DeployType("ELITE_3", 84);
        public static readonly DeployType RESEARCH_ELITE_RANKS = new DeployType("RESEARCH_ELITE_RANKS", 85);
        public static readonly DeployType RESEARCH_ELITE_TRAINING = new DeployType("RESEARCH_ELITE_TRAINING", 86);
        public static readonly DeployType UPGRADE_RUNE = new DeployType("UPGRADE_RUNE", 87);
        public static readonly DeployType RESEARCH_GREAT_TRIBE = new DeployType("RESEARCH_GREAT_TRIBE", 88);
        public static readonly DeployType RESEARCH_BRIDGES = new DeployType("RESEARCH_BRIDGES", 89);
        public static readonly DeployType RESEARCH_RUNE_REACH = new DeployType("RESEARCH_RUNE_REACH", 90);
        public static readonly DeployType RESEARCH_RUNE_MIGHT = new DeployType("RESEARCH_RUNE_MIGHT", 91);
        public static readonly DeployType RESEARCH_RUNE_WARD = new DeployType("RESEARCH_RUNE_WARD", 92);
        public static readonly DeployType SPINNER = new DeployType("SPINNER", 93);
        public static readonly DeployType[] VALUES_ = new DeployType[] { ROCK_WARRIOR, IRON_WARRIOR, RUBBER_WARRIOR, PEON, PEON_HARVEST_TREE, PEON_TRANSPORT_TREE, PEON_HARVEST_ROCK, PEON_TRANSPORT_ROCK, PEON_HARVEST_IRON, PEON_TRANSPORT_IRON, PEON_HARVEST_RUBBER, PEON_TRANSPORT_RUBBER, CHAMPION, SCOUT, RESEARCH_ARMOR, RESEARCH_SPEED, RESEARCH_TOOLS, RESEARCH_RANGE, SHAMAN, SLINGER, RESEARCH_GROWTH, RESEARCH_TOWER_RANGE, RESEARCH_TOWER_ARMOR, RESEARCH_BLESSING, RESEARCH_LORE, RESEARCH_WALLS, MELEE_LIGHT, MELEE_HEAVY, PEON_HARVEST_GOLD, PEON_HARVEST_CRYSTAL, PEON_HARVEST_HERBS, PEON_HARVEST_COAL, UPGRADE_STONE, UPGRADE_IRON, UPGRADE_CHICKEN, UNIQUE_1, UNIQUE_2, RESEARCH_RACE_1, RESEARCH_RACE_2, RESEARCH_SMITHING, RESEARCH_DRILL, RESEARCH_STURDY, RESEARCH_BASKETS, RESEARCH_VOLLEY, RESEARCH_STAKES, RESEARCH_FOCUS, RESEARCH_PROSPECTING, RESEARCH_VIGOR, CRAFT_CRYSTAL, CRAFT_DIAMOND, CRAFT_DARTS, CRYSTAL_WARRIOR, DIAMOND_GUARD, UNIQUE_3, MINOR_GOD, MAJOR_GOD, RESEARCH_CRYSTAL_CRAFT, RESEARCH_DIAMOND_CRAFT, RESEARCH_ROCK_EDGE, RESEARCH_IRON_TEMPER, RESEARCH_CHICKEN_BOUNCE, RESEARCH_CRYSTAL_FACET, RESEARCH_DIAMOND_EDGE, RESEARCH_SHIP_OARS, RESEARCH_SHIP_HULL, MARKET_WOOD, MARKET_ROCK, MARKET_IRON, MARKET_CHICKEN, MARKET_CRYSTAL, MARKET_HERBS, MARKET_COAL, GIFT_WOOD, GIFT_ROCK, GIFT_IRON, GIFT_GOLD, RESEARCH_ARCANE_POWER, RESEARCH_COURAGE_MASTERY, RESEARCH_FLEET, RESEARCH_CARGO, RESEARCH_BUILDINGS, PEON_HARVEST_DIAMOND, ELITE_1, ELITE_2, ELITE_3, RESEARCH_ELITE_RANKS, RESEARCH_ELITE_TRAINING, UPGRADE_RUNE, RESEARCH_GREAT_TRIBE, RESEARCH_BRIDGES, RESEARCH_RUNE_REACH, RESEARCH_RUNE_MIGHT, RESEARCH_RUNE_WARD, SPINNER };
        public DeployType(string name_, int ordinal_) : base(name_, ordinal_)
        {
        }
    }
}
