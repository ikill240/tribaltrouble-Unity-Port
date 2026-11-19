// Translated from Counters.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 13
    public class Counters : java.lang.Object
    {
        public const int PEON = 0;
        public const int RANGED = 1;
        public const int LIGHT = 2;
        public const int HEAVY = 3;
        public const int FAST = 4;
        public const int CASTER = 5;
        public const int ELITE = 6;
        public const int NUM_ROLES = 7;
        public static float[][] FACTOR;
        // Java: 40
        public static int unitIndex(com.oddlabs.tt.model.Race race, com.oddlabs.tt.model.UnitTemplate t)
        {
            for (int i = 0; i < com.oddlabs.tt.model.Race.NUM_UNITS; i++)
            {
                if (((object)race.getUnitTemplate(i) == (object)t))
                {
                    return i;
                }
            }
            return (-1);
        }
        // Java: 48
        public static int role(com.oddlabs.tt.model.Race race, int index, bool vikings)
        {
            return ((index) switch { 3 => (int)(com.oddlabs.tt.model.Counters.PEON), 9 => (int)(com.oddlabs.tt.model.Counters.LIGHT), 10 or 23 => (int)(com.oddlabs.tt.model.Counters.HEAVY), 12 or 6 => (int)(com.oddlabs.tt.model.Counters.FAST), 7 => (int)(com.oddlabs.tt.model.Counters.CASTER), 15 => (int)((vikings ? com.oddlabs.tt.model.Counters.HEAVY : com.oddlabs.tt.model.Counters.RANGED)), 5 or 14 or 4 or 16 or 17 or 19 or 20 or 24 => (int)(com.oddlabs.tt.model.Counters.ELITE), 18 => (int)(com.oddlabs.tt.model.Counters.LIGHT), _ => (int)((index >= 0 && (race.getUnitTemplate(index).getWeaponFactory() is com.oddlabs.tt.model.weapon.MeleeFactory) ? com.oddlabs.tt.model.Counters.LIGHT : com.oddlabs.tt.model.Counters.RANGED)) });
        }
        // Java: 63
        public static int role(com.oddlabs.tt.model.Unit u)
        {
            com.oddlabs.tt.model.Race race = u.getOwner().getRace();
            return com.oddlabs.tt.model.Counters.role(race, com.oddlabs.tt.model.Counters.unitIndex(race, ((com.oddlabs.tt.model.UnitTemplate)u.getTemplate())), com.oddlabs.tt.model.ExtendedCombat.isVikings(u.getOwner()));
        }
        // Java: 68
        public static float factor(int attacker, int target)
        {
            return com.oddlabs.tt.model.Counters.FACTOR[attacker][target];
        }
        // Java: 73
        public static int apply(com.oddlabs.tt.model.Unit src, com.oddlabs.tt.model.Selectable target, int damage)
        {
            if (damage <= 0 || !((target is com.oddlabs.tt.model.Unit t)) || !src.getOwner().getWorld().isExtended())
            {
                return damage;
            }
            float f = com.oddlabs.tt.model.Counters.FACTOR[com.oddlabs.tt.model.Counters.role(src)][com.oddlabs.tt.model.Counters.role(t)];
            if (f == 1.0f)
            {
                return damage;
            }
            float d = (float)(damage * f);
            int whole = java.lang.JC.f2i(d);
            if (src.getOwner().getWorld().getRandom().nextFloat() < (float)(d - whole))
            {
                whole++;
            }
            return java.lang.Math.max((f < 1.0f ? 1 : damage), whole);
        }
        static Counters()
        {
            Counters.FACTOR = java.lang.JA.newArray2<float>(com.oddlabs.tt.model.Counters.NUM_ROLES, com.oddlabs.tt.model.Counters.NUM_ROLES);
            {
                foreach (float[] row in com.oddlabs.tt.model.Counters.FACTOR)
                {
                    java.util.Arrays.fill(row, 1.0f);
                }
                com.oddlabs.tt.model.Counters.FACTOR[com.oddlabs.tt.model.Counters.HEAVY][com.oddlabs.tt.model.Counters.FAST] = 1.5f;
                com.oddlabs.tt.model.Counters.FACTOR[com.oddlabs.tt.model.Counters.HEAVY][com.oddlabs.tt.model.Counters.LIGHT] = 1.25f;
                com.oddlabs.tt.model.Counters.FACTOR[com.oddlabs.tt.model.Counters.HEAVY][com.oddlabs.tt.model.Counters.ELITE] = 1.15f;
                com.oddlabs.tt.model.Counters.FACTOR[com.oddlabs.tt.model.Counters.FAST][com.oddlabs.tt.model.Counters.RANGED] = 1.5f;
                com.oddlabs.tt.model.Counters.FACTOR[com.oddlabs.tt.model.Counters.FAST][com.oddlabs.tt.model.Counters.CASTER] = 1.5f;
                com.oddlabs.tt.model.Counters.FACTOR[com.oddlabs.tt.model.Counters.FAST][com.oddlabs.tt.model.Counters.PEON] = 1.25f;
                com.oddlabs.tt.model.Counters.FACTOR[com.oddlabs.tt.model.Counters.FAST][com.oddlabs.tt.model.Counters.HEAVY] = 0.75f;
                com.oddlabs.tt.model.Counters.FACTOR[com.oddlabs.tt.model.Counters.LIGHT][com.oddlabs.tt.model.Counters.RANGED] = 1.3f;
                com.oddlabs.tt.model.Counters.FACTOR[com.oddlabs.tt.model.Counters.LIGHT][com.oddlabs.tt.model.Counters.PEON] = 1.25f;
                com.oddlabs.tt.model.Counters.FACTOR[com.oddlabs.tt.model.Counters.RANGED][com.oddlabs.tt.model.Counters.HEAVY] = 1.3f;
                com.oddlabs.tt.model.Counters.FACTOR[com.oddlabs.tt.model.Counters.RANGED][com.oddlabs.tt.model.Counters.FAST] = 0.85f;
                com.oddlabs.tt.model.Counters.FACTOR[com.oddlabs.tt.model.Counters.CASTER][com.oddlabs.tt.model.Counters.ELITE] = 1.25f;
            }
        }
        public Counters() : base()
        {
        }
    }
}
