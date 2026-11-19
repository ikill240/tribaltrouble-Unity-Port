// Translated from CombatModel.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.ultra
{
    // Java: 14
    public class CombatModel : java.lang.Object
    {
        // Java: 18
        public static float weight(com.oddlabs.tt.player.ultra.WorldModel.Kind kind)
        {
            return ((kind).ordinal() switch { 1 /*ROCK*/ => (float)(0.63f), 2 /*IRON*/ => (float)(1.0f), 3 /*RUBBER*/ => (float)(1.35f), 0 /*PEON*/ => (float)(0.05f), 4 /*CHIEFTAIN*/ => (float)(1.5f), _ => throw new System.InvalidOperationException("MatchException") });
        }
        // Java: 28
        public static float weight(com.oddlabs.tt.model.Unit u)
        {
            return com.oddlabs.tt.player.ultra.CombatModel.weight(com.oddlabs.tt.player.ultra.WorldModel.kindOf(u));
        }
        // Java: 33
        public static float towerValue(com.oddlabs.tt.model.Building tower)
        {
            com.oddlabs.tt.model.Unit g = com.oddlabs.tt.player.ultra.WorldModel.garrisonOf(tower);
            if (g == null || com.oddlabs.tt.player.ultra.WorldModel.isStunned(g))
            {
                return 0.0f;
            }
            float hp = (float)(tower.getHitPoints() / 100.0f);
            float @base = ((com.oddlabs.tt.player.ultra.WorldModel.kindOf(g)).ordinal() switch { 3 /*RUBBER*/ => (float)(16.0f), 2 /*IRON*/ => (float)(11.0f), _ => (float)(7.5f) });
            return (float)(@base * ((float)(0.4f + (float)(0.6f * hp))));
        }
        // Java: 47
        public static float sumWeights(java.util.List units)
        {
            float e = 0.0f;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(units))
            {
                e = (float)(e + com.oddlabs.tt.player.ultra.CombatModel.weight(u));
            }
            return e;
        }
        // Java: 55
        public static float strength(float units, float tower_sq_sum)
        {
            return ((float)(java.lang.Math.sqrt((float)((float)(units * units) + tower_sq_sum))));
        }
        // Java: 60
        public static float hitChance(com.oddlabs.tt.model.Unit a, com.oddlabs.tt.model.Unit t, com.oddlabs.tt.player.ultra.MapAnalysis map)
        {
            float h = ((com.oddlabs.tt.player.ultra.WorldModel.kindOf(a)).ordinal() switch { 1 /*ROCK*/ => (float)(0.5f), 2 /*IRON*/ => (float)(0.75f), 3 /*RUBBER*/ => (float)(0.95f), 0 /*PEON*/ => (float)(0.2f), 4 /*CHIEFTAIN*/ => (float)(0.75f), _ => throw new System.InvalidOperationException("MatchException") });
            if (map != null)
            {
                float dz = (float)(map.height(a.getGridX(), a.getGridY()) - map.height(t.getGridX(), t.getGridY()));
                h = (float)(h + java.lang.Math.max((float)(-0.25f), java.lang.Math.min(0.25f, (float)(dz * 0.0125f))));
            }
            float def = (com.oddlabs.tt.player.ultra.WorldModel.isStunned(t) ? 0.0f : ((com.oddlabs.tt.model.UnitTemplate)t.getTemplate()).getDefenseChance());
            return java.lang.Math.max(0.0f, java.lang.Math.min(1.0f, (float)(h * ((float)(1.0f - def)))));
        }
        // Java: 79
        public static float buildingDamage(com.oddlabs.tt.model.Unit a)
        {
            return ((com.oddlabs.tt.player.ultra.WorldModel.kindOf(a)).ordinal() switch { 1 /*ROCK*/ => (float)(0.5f), 2 /*IRON*/ => (float)(1.5f), 3 /*RUBBER*/ => (float)(3.8f), 0 /*PEON*/ or 4 /*CHIEFTAIN*/ => (float)(0.2f), _ => throw new System.InvalidOperationException("MatchException") });
        }
        public CombatModel() : base()
        {
        }
    }
}
