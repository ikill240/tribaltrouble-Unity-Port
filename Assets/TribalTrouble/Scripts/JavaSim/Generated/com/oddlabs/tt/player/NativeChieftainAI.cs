// Translated from NativeChieftainAI.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player
{
    // Java: 11
    public class NativeChieftainAI : com.oddlabs.tt.player.ChieftainAI
    {
        public const int NUM_UNITS_FOR_LIGHTNING = 8;
        public const int NUM_UNITS_FOR_POISON = 10;
        public const int NUM_UNITS_FOR_CONVERT = 15;
        // Java: 16
        public override void decide(com.oddlabs.tt.model.Unit chieftain)
        {
            nodeLightningCloud(chieftain);
            nodePoisonFog(chieftain);
            nodeConvert(chieftain);
        }
        // Java: 23
        public void nodeLightningCloud(com.oddlabs.tt.model.Unit chieftain)
        {
            if (chieftain.getMagicProgress(com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_LIGHTNING) < 1)
            {
                return;
            }
            float hit_radius = 38.0f;
            int num_enemy_units = numEnemyUnits(chieftain.getOwner());
            int num_enemy_units_close = getNumEnemyUnitsClose(chieftain, hit_radius);
            if (num_enemy_units_close >= com.oddlabs.tt.player.NativeChieftainAI.NUM_UNITS_FOR_LIGHTNING || (num_enemy_units < com.oddlabs.tt.player.NativeChieftainAI.NUM_UNITS_FOR_LIGHTNING && num_enemy_units_close > 2) || (chieftain.getHitPoints() <= 2 && num_enemy_units_close > 1))
            {
                chieftain.doMagic(com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_LIGHTNING, false);
            }
        }
        // Java: 37
        public void nodePoisonFog(com.oddlabs.tt.model.Unit chieftain)
        {
            if (chieftain.getMagicProgress(com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_POISON) < 1)
            {
                return;
            }
            float hit_radius = chieftain.getOwner().getRace().getMagicFactory(com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_POISON).getHitRadius();
            int num_enemy_units = numEnemyUnits(chieftain.getOwner());
            int num_enemy_units_close = getNumEnemyUnitsClose(chieftain, hit_radius);
            int num_friendly_units_close = getNumFriendlyUnitsClose(chieftain, hit_radius);
            if (2 * num_friendly_units_close < num_enemy_units_close && (num_enemy_units_close >= com.oddlabs.tt.player.NativeChieftainAI.NUM_UNITS_FOR_POISON || (num_enemy_units < com.oddlabs.tt.player.NativeChieftainAI.NUM_UNITS_FOR_POISON && num_enemy_units_close > 4) || (chieftain.getHitPoints() <= 2 && num_enemy_units_close > 5)))
            {
                chieftain.doMagic(com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_POISON, false);
            }
        }
        // Java: 54
        public void nodeConvert(com.oddlabs.tt.model.Unit chieftain)
        {
            if (chieftain.getMagicProgress(com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_CONVERT) < 1)
            {
                return;
            }
            float hit_radius = chieftain.getOwner().getRace().getMagicFactory(com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_CONVERT).getHitRadius();
            int num_enemy_units = numEnemyUnits(chieftain.getOwner());
            int num_enemy_units_close = getNumEnemyUnitsClose(chieftain, hit_radius);
            if (num_enemy_units_close >= com.oddlabs.tt.player.NativeChieftainAI.NUM_UNITS_FOR_CONVERT || (num_enemy_units < com.oddlabs.tt.player.NativeChieftainAI.NUM_UNITS_FOR_CONVERT && num_enemy_units_close > 3) || (chieftain.getHitPoints() <= 3 && num_enemy_units_close > 2))
            {
                chieftain.doMagic(com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_CONVERT, false);
            }
        }
        // Java: 69
        public int getNumEnemyUnitsClose(com.oddlabs.tt.model.Unit chieftain, float hit_radius)
        {
            com.oddlabs.tt.pathfinder.FindOccupantFilter filter = new com.oddlabs.tt.pathfinder.FindOccupantFilter(chieftain.getPositionX(), chieftain.getPositionY(), hit_radius, chieftain, typeof(com.oddlabs.tt.model.Unit));
            chieftain.getUnitGrid().scan(filter, chieftain.getGridX(), chieftain.getGridY());
            long num_enemy_units_close = java.util.stream.StreamSupport.stream(filter.getResult().spliterator(), false).filter(new java.util.function.Predicate_L((object a0) => { return ((com.oddlabs.tt.model.Selectable)a0).isAlive(); })).filter(new java.util.function.Predicate_L((object unit_l) =>
            {
                com.oddlabs.tt.model.Unit unit = (com.oddlabs.tt.model.Unit)unit_l;
                float dx = (float)(unit.getPositionX() - chieftain.getPositionX());
                float dy = (float)(unit.getPositionY() - chieftain.getPositionY());
                float squared_dist = (float)((float)(dx * dx) + (float)(dy * dy));
                return chieftain.getOwner().isEnemy(unit.getOwner()) && squared_dist < (float)(hit_radius * hit_radius);
            })).count();
            return unchecked((int)(num_enemy_units_close));
        }
        // Java: 83
        public int getNumFriendlyUnitsClose(com.oddlabs.tt.model.Unit chieftain, float hit_radius)
        {
            com.oddlabs.tt.pathfinder.FindOccupantFilter filter = new com.oddlabs.tt.pathfinder.FindOccupantFilter(chieftain.getPositionX(), chieftain.getPositionY(), hit_radius, chieftain, com.oddlabs.tt.model.Selectable.genericClass());
            chieftain.getUnitGrid().scan(filter, chieftain.getGridX(), chieftain.getGridY());
            long num_friendly_units_close = java.util.stream.StreamSupport.stream(filter.getResult().spliterator(), false).filter(new java.util.function.Predicate_L((object a0) => { return ((com.oddlabs.tt.model.Selectable)a0).isAlive(); })).filter(new java.util.function.Predicate_L((object s_l) =>
            {
                com.oddlabs.tt.model.Selectable s = (com.oddlabs.tt.model.Selectable)s_l;
                float dx = (float)(s.getPositionX() - chieftain.getPositionX());
                float dy = (float)(s.getPositionY() - chieftain.getPositionY());
                float squared_dist = (float)((float)(dx * dx) + (float)(dy * dy));
                return !chieftain.getOwner().isEnemy(s.getOwner()) && squared_dist < (float)(hit_radius * hit_radius);
            })).count();
            return unchecked((int)(num_friendly_units_close));
        }
        public NativeChieftainAI() : base()
        {
        }
    }
}
