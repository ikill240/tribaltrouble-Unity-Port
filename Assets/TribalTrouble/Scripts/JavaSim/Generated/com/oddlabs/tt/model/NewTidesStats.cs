// Translated from NewTidesStats.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 27
    public class NewTidesStats : java.lang.Object
    {
        public const int HEALTH = 0;
        public const int DAMAGE = 1;
        public const int REACH = 2;
        public const int SPEED = 3;
        public const int DODGE = 4;
        public const int TRAIN_SECONDS = 5;
        public const int WEAPONS = 6;
        public static float[][] VIKINGS;
        public static float[][] NATIVES;
        // Java: 89
        public static float[] row(bool vikings, int kind)
        {
            float[][] t = (vikings ? com.oddlabs.tt.model.NewTidesStats.VIKINGS : com.oddlabs.tt.model.NewTidesStats.NATIVES);
            return (kind >= 0 && kind < t.Length ? t[kind] : (float[])(null));
        }
        // Java: 94
        public static float trainSeconds(bool vikings, int kind, float fallback)
        {
            float[] r = com.oddlabs.tt.model.NewTidesStats.row(vikings, kind);
            return (r != null && r[com.oddlabs.tt.model.NewTidesStats.TRAIN_SECONDS] > 0.0f ? r[com.oddlabs.tt.model.NewTidesStats.TRAIN_SECONDS] : fallback);
        }
        // Java: 99
        public static int weapons(bool vikings, int kind, int fallback)
        {
            float[] r = com.oddlabs.tt.model.NewTidesStats.row(vikings, kind);
            return (r != null && r[com.oddlabs.tt.model.NewTidesStats.WEAPONS] > 0.0f ? java.lang.Math.round(r[com.oddlabs.tt.model.NewTidesStats.WEAPONS]) : fallback);
        }
        // Java: 105
        public static float[] lookup(com.oddlabs.tt.model.RacesResources rr, com.oddlabs.tt.model.UnitTemplate t)
        {
            for (int r = 0; r < 2; r++)
            {
                com.oddlabs.tt.model.Race race = rr.getRace(r);
                for (int k = 0; k < com.oddlabs.tt.model.Race.NUM_UNITS; k++)
                {
                    if (((object)race.getUnitTemplate(k) == (object)t))
                    {
                        return com.oddlabs.tt.model.NewTidesStats.row(((object)race == (object)rr.getRace(com.oddlabs.tt.model.RacesResources.RACE_VIKINGS)), k);
                    }
                }
            }
            return null;
        }
        // Java: 116
        public static int baseDamage(com.oddlabs.tt.model.weapon.WeaponFactory f)
        {
            if ((f is com.oddlabs.tt.model.weapon.MeleeFactory m))
            {
                return m.getDamage();
            }
            System.Type t = f.getType();
            string n = (t == null ? "" : t.getSimpleName());
            return (n.startsWith("Rock") ? 1 : (n.startsWith("Diamond") ? 4 : (n.startsWith("Crystal") ? 3 : 2)));
        }
        // Java: 125
        public static int damageDelta(float[] row_v, com.oddlabs.tt.model.UnitTemplate t)
        {
            return (row_v == null ? 0 : java.lang.Math.round(row_v[com.oddlabs.tt.model.NewTidesStats.DAMAGE]) - com.oddlabs.tt.model.NewTidesStats.baseDamage(t.getWeaponFactory()));
        }
        static NewTidesStats()
        {
            NewTidesStats.VIKINGS = new float[][] {
                //            health damage reach speed dodge train-s weapons   (see NewTidesStats.java)
                new float[] { 4, 1, 7, 3.9f, 0.3f, 1, 1 }, // 0 Rock warrior
                new float[] { 6, 2, 8, 3.5f, 0.5f, 1.5f, 1 }, // 1 Iron warrior
                new float[] { 5, 3, 9, 4, 0.7f, 2, 1 }, // 2 Chicken warrior
                new float[] { 2, 1, 0, 5, 0.05f, 0.5f, 0 }, // 3 Peon
                new float[] { 300, 4, 0, 4, 0.4f, 0, 0 }, // 4 Chieftain
                new float[] { 12, 3, 9, 3.4f, 0.8f, 3, 2 }, // 5 Champion
                new float[] { 3, 1, 6, 6, 0.35f, 0.75f, 1 }, // 6 Scout
                new float[] { 7, 1, 8, 3.6f, 0.5f, 4, 2 }, // 7 Skald
                new float[] { 3, 1, 13, 4.2f, 0.3f, 1, 1 }, // 8 Slinger
                new float[] { 3, 1, 0, 3.9f, 0.45f, 0.75f, 1 }, // 9 Axeman
                new float[] { 5, 2, 0, 3.5f, 0.65f, 1.5f, 1 }, // 10 Huscarl
                new float[] { 8, 4, 8, 4.6f, 0.35f, 1.25f, 1 }, // 11 Fire Thrower
                new float[] { 24, 2, 0, 5.2f, 0.3f, 2, 3 }, // 12 Berserker
                new float[] { 8, 3, 10, 4, 0.72f, 1.75f, 1 }, // 13 Crystal Raider
                new float[] { 24, 4, 0, 3.6f, 0.8f, 2.5f, 1 }, // 14 Diamond Guard
                new float[] { 20, 1, 0, 4.2f, 0.5f, 2, 2 }, // 15 Shield Maiden
                new float[] { 180, 4, 11, 5.4f, 0.55f, 40, 2 }, // 16 Valkyrie
                new float[] { 320, 5, 0, 4.2f, 0.6f, 70, 4 }, // 17 Thunder Lord
                null, // 18 (the Rift's walking dead: not trained)
                null, // 19 (the Rift's walking dead: not trained)
                new float[] { 32, 4, 0, 4.8f, 0.65f, 3, 5 }, // 20 Jarl
                new float[] { 16, 3, 11, 5, 0.5f, 2.5f, 2 }, // 21 Huntress
                new float[] { 16, 2, 8, 5.2f, 0.5f, 2.5f, 3 }, // 22 Horn Bearer
                new float[] { 32, 4, 0, 4.2f, 0.55f, 2.5f, 4 }, // 23 Whirlwind
            };
            NewTidesStats.NATIVES = new float[][] {
                //            health damage reach speed dodge train-s weapons   (see NewTidesStats.java)
                new float[] { 3, 1, 8, 4, 0.5f, 1, 1 }, // 0 Rock warrior
                new float[] { 5, 2, 8, 4, 0.6f, 1.5f, 1 }, // 1 Iron warrior
                new float[] { 4, 4, 9, 4, 0.7f, 2, 1 }, // 2 Chicken warrior
                new float[] { 1, 2, 0, 5, 0.1f, 0.5f, 0 }, // 3 Peon
                new float[] { 260, 5, 0, 4, 0.5f, 0, 0 }, // 4 Chieftain
                new float[] { 12, 3, 9, 3.4f, 0.8f, 3, 2 }, // 5 Champion
                new float[] { 3, 1, 7, 6, 0.45f, 0.75f, 1 }, // 6 Scout
                new float[] { 7, 2, 9, 3.6f, 0.5f, 4, 2 }, // 7 Shaman
                new float[] { 3, 2, 13, 4.2f, 0.3f, 1, 1 }, // 8 Slinger
                new float[] { 3, 2, 0, 5, 0.45f, 0.75f, 1 }, // 9 Club Warrior
                new float[] { 5, 3, 0, 3.8f, 0.65f, 1.5f, 1 }, // 10 Spear Guard
                new float[] { 8, 4, 10, 5.2f, 0.4f, 1.25f, 1 }, // 11 Blowgunner
                new float[] { 24, 3, 0, 6.2f, 0.7f, 2, 3 }, // 12 Jaguar Warrior
                new float[] { 8, 2, 10, 4, 0.72f, 1.75f, 1 }, // 13 Crystal Thrower
                new float[] { 24, 3, 0, 3.6f, 0.8f, 2.5f, 1 }, // 14 Diamond Guard
                new float[] { 20, 2, 12, 4.8f, 0.4f, 1.75f, 2 }, // 15 Dart Master
                new float[] { 180, 2, 0, 6.4f, 0.65f, 40, 2 }, // 16 Jaguar Spirit
                new float[] { 320, 5, 0, 4.2f, 0.6f, 70, 4 }, // 17 Great Tiki
                null, // 18 (the Rift's walking dead: not trained)
                null, // 19 (the Rift's walking dead: not trained)
                new float[] { 32, 3, 0, 5.2f, 0.65f, 3, 5 }, // 20 Eagle Knight
                new float[] { 16, 2, 12, 5.2f, 0.5f, 2.5f, 2 }, // 21 Hawk Eye
                new float[] { 16, 2, 8, 5.4f, 0.5f, 2.5f, 3 }, // 22 War Drummer
                new float[] { 32, 5, 0, 4.4f, 0.55f, 2.5f, 4 }, // 23 Feather Whirler
            };
        }
        public NewTidesStats() : base()
        {
        }
    }
}
