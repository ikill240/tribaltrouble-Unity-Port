// Translated from Combat.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.gauntlet
{
    // Java: 17
    public class Combat : java.lang.Object
    {
        public const float IRON = 1.0f;
        public const float CHICKEN = 2.0f;
        public const float ROCK = 0.6f;
        public const float PEON = 0.2f;
        public const float CHIEFTAIN = 2.5f;
        public const float TOWER = 4.0f;
        // Java: 27
        public static float @value(com.oddlabs.tt.model.Unit unit)
        {
            if (com.oddlabs.tt.player.gauntlet.Intel.isStunned(unit))
            {
                return 0.0f;
            }
            return com.oddlabs.tt.player.gauntlet.Combat.lastingValue(unit);
        }
        // Java: 34
        public static float lastingValue(com.oddlabs.tt.model.Unit unit)
        {
            if (unit.isDead())
            {
                return 0.0f;
            }
            if (unit.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC))
            {
                return (float)(com.oddlabs.tt.player.gauntlet.Combat.CHIEFTAIN * java.lang.Math.max(0.3f, (float)(unit.getHitPoints() / 60.0f)));
            }
            if (!unit.getAbilities().isWarrior())
            {
                return com.oddlabs.tt.player.gauntlet.Combat.PEON;
            }
            return ((com.oddlabs.tt.player.gauntlet.Intel.warriorType(unit)).ordinal() switch { 1 /*IRON*/ => (float)(com.oddlabs.tt.player.gauntlet.Combat.IRON), 2 /*CHICKEN*/ => (float)(com.oddlabs.tt.player.gauntlet.Combat.CHICKEN), 0 /*ROCK*/ => (float)(com.oddlabs.tt.player.gauntlet.Combat.ROCK), _ => throw new System.InvalidOperationException("MatchException") });
        }
        // Java: 48
        public static float towerValue(com.oddlabs.tt.model.Building tower)
        {
            if (!com.oddlabs.tt.player.gauntlet.Intel.isTowerActive(tower))
            {
                return 0.0f;
            }
            return (float)(com.oddlabs.tt.player.gauntlet.Combat.TOWER * java.lang.Math.max(0.35f, (float)(tower.getHitPoints() / ((float)(((com.oddlabs.tt.model.BuildingTemplate)tower.getTemplate()).getMaxHitPoints())))));
        }
        // Java: 54
        public static float @value(com.oddlabs.tt.model.Selectable s)
        {
            if ((s is com.oddlabs.tt.model.Unit unit))
            {
                return com.oddlabs.tt.player.gauntlet.Combat.@value(unit);
            }
            if ((s is com.oddlabs.tt.model.Building b) && ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID() == com.oddlabs.tt.model.Race.BUILDING_TOWER)
            {
                return com.oddlabs.tt.player.gauntlet.Combat.towerValue(b);
            }
            return 0.0f;
        }
        // Java: 65
        public static float strengthNear(java.util.List units, int x, int y, int radius)
        {
            int r2 = radius * radius;
            float sum = 0.0f;
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(units))
            {
                if (s.isDead())
                {
                    continue;
                }
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, s.getGridX(), s.getGridY()) <= r2)
                {
                    sum = (float)(sum + com.oddlabs.tt.player.gauntlet.Combat.@value(s));
                }
            }
            return sum;
        }
        // Java: 78
        public static float total(java.util.List units)
        {
            float sum = 0.0f;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(units))
            {
                sum = (float)(sum + com.oddlabs.tt.player.gauntlet.Combat.@value(u));
            }
            return sum;
        }
        // Java: 85
        public static int countNear(java.util.List units, int x, int y, int radius)
        {
            int r2 = radius * radius;
            int n = 0;
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(units))
            {
                if (!s.isDead() && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, s.getGridX(), s.getGridY()) <= r2)
                {
                    n++;
                }
            }
            return n;
        }
        public Combat() : base()
        {
        }
    }
}
