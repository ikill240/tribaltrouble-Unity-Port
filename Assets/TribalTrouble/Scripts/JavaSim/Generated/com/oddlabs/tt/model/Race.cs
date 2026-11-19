// Translated from Race.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 10
    public class Race : java.lang.Object
    {
        public const int BUILDING_QUARTERS = 0;
        public const int BUILDING_ARMORY = 1;
        public const int BUILDING_TOWER = 2;
        public const int BUILDING_SHIP = 3;
        public const int BUILDING_PALISADE = 4;
        public const int BUILDING_SHRINE = 5;
        public const int NUM_CLASSIC_BUILDINGS = 4;
        public const int BUILDING_HALL = 6;
        public const int BUILDING_FORGE = 7;
        public const int BUILDING_TOTEM = 8;
        public const int BUILDING_DOCK = 9;
        public const int BUILDING_MARKET = 10;
        public const int BUILDING_BRIDGE = 11;
        public const int NUM_BUILDINGS = 12;
        public const int UNIT_WARRIOR_ROCK = 0;
        public const int UNIT_WARRIOR_IRON = 1;
        public const int UNIT_WARRIOR_RUBBER = 2;
        public const int UNIT_PEON = 3;
        public const int UNIT_CHIEFTAIN = 4;
        public const int UNIT_CHAMPION = 5;
        public const int UNIT_SCOUT = 6;
        public const int UNIT_SHAMAN = 7;
        public const int UNIT_SLINGER = 8;
        public const int UNIT_MELEE_LIGHT = 9;
        public const int UNIT_MELEE_HEAVY = 10;
        public const int UNIT_UNIQUE_1 = 11;
        public const int UNIT_UNIQUE_2 = 12;
        public const int UNIT_CRYSTAL = 13;
        public const int UNIT_DIAMOND = 14;
        public const int UNIT_UNIQUE_3 = 15;
        public const int UNIT_MINOR_GOD = 16;
        public const int UNIT_MAJOR_GOD = 17;
        public const int UNIT_ZOMBIE = 18;
        public const int UNIT_ZOMBIE_BRUTE = 19;
        public const int UNIT_ELITE_1 = 20;
        public const int UNIT_ELITE_2 = 21;
        public const int UNIT_ELITE_3 = 22;
        public const int UNIT_SPINNER = 23;
        public const int UNIT_ZOMBIE_BOSS = 24;
        public const int NUM_UNITS = 25;
        public com.oddlabs.tt.model.BuildingTemplate[] buildings;
        public com.oddlabs.tt.model.UnitTemplate[] units;
        public com.oddlabs.tt.render.SpriteKey rally_point;
        public com.oddlabs.tt.gui.RaceIcons icons;
        public com.oddlabs.tt.audio.Audio attack_notification;
        public com.oddlabs.tt.audio.Audio building_notification;
        public com.oddlabs.tt.model.weapon.MagicFactory[] magic_factory;
        public com.oddlabs.tt.player.ChieftainAI chieftain_ai;
        public string music_path;
        // Java: 159
        public com.oddlabs.tt.model.BuildingTemplate getBuildingTemplate(int index)
        {
            return buildings[index];
        }
        // Java: 163
        public com.oddlabs.tt.model.UnitTemplate getUnitTemplate(int index)
        {
            return units[index];
        }
        // Java: 167
        public com.oddlabs.tt.render.SpriteKey getRallyPoint()
        {
            return rally_point;
        }
        // Java: 171
        public com.oddlabs.tt.gui.RaceIcons getIcons()
        {
            return icons;
        }
        // Java: 175
        public com.oddlabs.tt.audio.Audio getAttackNotificationAudio()
        {
            return attack_notification;
        }
        // Java: 179
        public com.oddlabs.tt.audio.Audio getBuildingNotificationAudio()
        {
            return building_notification;
        }
        // Java: 183
        public com.oddlabs.tt.model.weapon.MagicFactory getMagicFactory(int i)
        {
            return magic_factory[i];
        }
        // Java: 187
        public com.oddlabs.tt.player.ChieftainAI getChieftainAI()
        {
            return chieftain_ai;
        }
        // Java: 191
        public string getMusicPath()
        {
            return music_path;
        }
        private void init_Race()
        {
            this.buildings = new com.oddlabs.tt.model.BuildingTemplate[com.oddlabs.tt.model.Race.NUM_BUILDINGS];
            this.units = new com.oddlabs.tt.model.UnitTemplate[com.oddlabs.tt.model.Race.NUM_UNITS];
        }
        public Race(com.oddlabs.tt.model.BuildingTemplate quarters, com.oddlabs.tt.model.BuildingTemplate armory, com.oddlabs.tt.model.BuildingTemplate tower, com.oddlabs.tt.model.BuildingTemplate ship, com.oddlabs.tt.model.BuildingTemplate palisade, com.oddlabs.tt.model.BuildingTemplate shrine, com.oddlabs.tt.model.BuildingTemplate hall, com.oddlabs.tt.model.BuildingTemplate forge, com.oddlabs.tt.model.BuildingTemplate totem, com.oddlabs.tt.model.BuildingTemplate dock, com.oddlabs.tt.model.BuildingTemplate market, com.oddlabs.tt.model.BuildingTemplate bridge, com.oddlabs.tt.model.UnitTemplate warrior_rock, com.oddlabs.tt.model.UnitTemplate warrior_iron, com.oddlabs.tt.model.UnitTemplate warrior_rubber, com.oddlabs.tt.model.UnitTemplate peon, com.oddlabs.tt.model.UnitTemplate chieftain, com.oddlabs.tt.model.UnitTemplate champion, com.oddlabs.tt.model.UnitTemplate scout, com.oddlabs.tt.model.UnitTemplate shaman, com.oddlabs.tt.model.UnitTemplate slinger, com.oddlabs.tt.model.UnitTemplate melee_light, com.oddlabs.tt.model.UnitTemplate melee_heavy, com.oddlabs.tt.model.UnitTemplate unique_1, com.oddlabs.tt.model.UnitTemplate unique_2, com.oddlabs.tt.model.UnitTemplate crystal, com.oddlabs.tt.model.UnitTemplate diamond, com.oddlabs.tt.model.UnitTemplate unique_3, com.oddlabs.tt.model.UnitTemplate minor_god, com.oddlabs.tt.model.UnitTemplate major_god, com.oddlabs.tt.model.UnitTemplate zombie, com.oddlabs.tt.model.UnitTemplate zombie_brute, com.oddlabs.tt.model.UnitTemplate elite_1, com.oddlabs.tt.model.UnitTemplate elite_2, com.oddlabs.tt.model.UnitTemplate elite_3, com.oddlabs.tt.model.UnitTemplate spinner, com.oddlabs.tt.model.UnitTemplate zombie_boss, com.oddlabs.tt.render.SpriteKey rally_point, com.oddlabs.tt.gui.RaceIcons icons, com.oddlabs.tt.audio.Audio attack_notification, com.oddlabs.tt.audio.Audio building_notification, com.oddlabs.tt.model.weapon.MagicFactory[] magic_factory, com.oddlabs.tt.player.ChieftainAI chieftain_ai, string music_path) : base()
        {
            init_Race();
            buildings[com.oddlabs.tt.model.Race.BUILDING_QUARTERS] = quarters;
            buildings[com.oddlabs.tt.model.Race.BUILDING_ARMORY] = armory;
            buildings[com.oddlabs.tt.model.Race.BUILDING_TOWER] = tower;
            buildings[com.oddlabs.tt.model.Race.BUILDING_SHIP] = ship;
            buildings[com.oddlabs.tt.model.Race.BUILDING_PALISADE] = palisade;
            buildings[com.oddlabs.tt.model.Race.BUILDING_SHRINE] = shrine;
            buildings[com.oddlabs.tt.model.Race.BUILDING_HALL] = hall;
            buildings[com.oddlabs.tt.model.Race.BUILDING_FORGE] = forge;
            buildings[com.oddlabs.tt.model.Race.BUILDING_TOTEM] = totem;
            buildings[com.oddlabs.tt.model.Race.BUILDING_DOCK] = dock;
            buildings[com.oddlabs.tt.model.Race.BUILDING_MARKET] = market;
            buildings[com.oddlabs.tt.model.Race.BUILDING_BRIDGE] = bridge;
            for (int i = 0; i < buildings.Length; i++)
            {
            }
            units[com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK] = warrior_rock;
            units[com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON] = warrior_iron;
            units[com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER] = warrior_rubber;
            units[com.oddlabs.tt.model.Race.UNIT_PEON] = peon;
            units[com.oddlabs.tt.model.Race.UNIT_CHIEFTAIN] = chieftain;
            units[com.oddlabs.tt.model.Race.UNIT_CHAMPION] = champion;
            units[com.oddlabs.tt.model.Race.UNIT_SCOUT] = scout;
            units[com.oddlabs.tt.model.Race.UNIT_SHAMAN] = shaman;
            units[com.oddlabs.tt.model.Race.UNIT_SLINGER] = slinger;
            units[com.oddlabs.tt.model.Race.UNIT_MELEE_LIGHT] = melee_light;
            units[com.oddlabs.tt.model.Race.UNIT_MELEE_HEAVY] = melee_heavy;
            units[com.oddlabs.tt.model.Race.UNIT_UNIQUE_1] = unique_1;
            units[com.oddlabs.tt.model.Race.UNIT_UNIQUE_2] = unique_2;
            units[com.oddlabs.tt.model.Race.UNIT_CRYSTAL] = crystal;
            units[com.oddlabs.tt.model.Race.UNIT_DIAMOND] = diamond;
            units[com.oddlabs.tt.model.Race.UNIT_UNIQUE_3] = unique_3;
            units[com.oddlabs.tt.model.Race.UNIT_MINOR_GOD] = minor_god;
            units[com.oddlabs.tt.model.Race.UNIT_MAJOR_GOD] = major_god;
            units[com.oddlabs.tt.model.Race.UNIT_ZOMBIE] = zombie;
            units[com.oddlabs.tt.model.Race.UNIT_ZOMBIE_BRUTE] = zombie_brute;
            units[com.oddlabs.tt.model.Race.UNIT_ELITE_1] = elite_1;
            units[com.oddlabs.tt.model.Race.UNIT_ELITE_2] = elite_2;
            units[com.oddlabs.tt.model.Race.UNIT_ELITE_3] = elite_3;
            units[com.oddlabs.tt.model.Race.UNIT_SPINNER] = spinner;
            units[com.oddlabs.tt.model.Race.UNIT_ZOMBIE_BOSS] = zombie_boss;
            this.rally_point = rally_point;
            this.icons = icons;
            this.attack_notification = attack_notification;
            this.building_notification = building_notification;
            this.magic_factory = magic_factory;
            this.chieftain_ai = chieftain_ai;
            this.music_path = music_path;
        }
    }
}
