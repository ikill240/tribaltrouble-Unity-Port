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
        public const int NUM_BUILDINGS = 4;
        public const int UNIT_WARRIOR_ROCK = 0;
        public const int UNIT_WARRIOR_IRON = 1;
        public const int UNIT_WARRIOR_RUBBER = 2;
        public const int UNIT_PEON = 3;
        public const int UNIT_CHIEFTAIN = 4;
        public const int NUM_UNITS = 5;
        public com.oddlabs.tt.model.BuildingTemplate[] buildings;
        public com.oddlabs.tt.model.UnitTemplate[] units;
        public com.oddlabs.tt.render.SpriteKey rally_point;
        public com.oddlabs.tt.gui.RaceIcons icons;
        public com.oddlabs.tt.audio.Audio attack_notification;
        public com.oddlabs.tt.audio.Audio building_notification;
        public com.oddlabs.tt.model.weapon.MagicFactory[] magic_factory;
        public com.oddlabs.tt.player.ChieftainAI chieftain_ai;
        public string music_path;
        // Java: 72
        public com.oddlabs.tt.model.BuildingTemplate getBuildingTemplate(int index)
        {
            return buildings[index];
        }
        // Java: 76
        public com.oddlabs.tt.model.UnitTemplate getUnitTemplate(int index)
        {
            return units[index];
        }
        // Java: 80
        public com.oddlabs.tt.render.SpriteKey getRallyPoint()
        {
            return rally_point;
        }
        // Java: 84
        public com.oddlabs.tt.gui.RaceIcons getIcons()
        {
            return icons;
        }
        // Java: 88
        public com.oddlabs.tt.audio.Audio getAttackNotificationAudio()
        {
            return attack_notification;
        }
        // Java: 92
        public com.oddlabs.tt.audio.Audio getBuildingNotificationAudio()
        {
            return building_notification;
        }
        // Java: 96
        public com.oddlabs.tt.model.weapon.MagicFactory getMagicFactory(int i)
        {
            return magic_factory[i];
        }
        // Java: 100
        public com.oddlabs.tt.player.ChieftainAI getChieftainAI()
        {
            return chieftain_ai;
        }
        // Java: 104
        public string getMusicPath()
        {
            return music_path;
        }
        private void init_Race()
        {
            this.buildings = new com.oddlabs.tt.model.BuildingTemplate[com.oddlabs.tt.model.Race.NUM_BUILDINGS];
            this.units = new com.oddlabs.tt.model.UnitTemplate[com.oddlabs.tt.model.Race.NUM_UNITS];
        }
        public Race(com.oddlabs.tt.model.BuildingTemplate quarters, com.oddlabs.tt.model.BuildingTemplate armory, com.oddlabs.tt.model.BuildingTemplate tower, com.oddlabs.tt.model.BuildingTemplate ship, com.oddlabs.tt.model.UnitTemplate warrior_rock, com.oddlabs.tt.model.UnitTemplate warrior_iron, com.oddlabs.tt.model.UnitTemplate warrior_rubber, com.oddlabs.tt.model.UnitTemplate peon, com.oddlabs.tt.model.UnitTemplate chieftain, com.oddlabs.tt.render.SpriteKey rally_point, com.oddlabs.tt.gui.RaceIcons icons, com.oddlabs.tt.audio.Audio attack_notification, com.oddlabs.tt.audio.Audio building_notification, com.oddlabs.tt.model.weapon.MagicFactory[] magic_factory, com.oddlabs.tt.player.ChieftainAI chieftain_ai, string music_path) : base()
        {
            init_Race();
            buildings[com.oddlabs.tt.model.Race.BUILDING_QUARTERS] = quarters;
            buildings[com.oddlabs.tt.model.Race.BUILDING_ARMORY] = armory;
            buildings[com.oddlabs.tt.model.Race.BUILDING_TOWER] = tower;
            buildings[com.oddlabs.tt.model.Race.BUILDING_SHIP] = ship;
            for (int i = 0; i < buildings.Length; i++)
            {
            }
            units[com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK] = warrior_rock;
            units[com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON] = warrior_iron;
            units[com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER] = warrior_rubber;
            units[com.oddlabs.tt.model.Race.UNIT_PEON] = peon;
            units[com.oddlabs.tt.model.Race.UNIT_CHIEFTAIN] = chieftain;
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
