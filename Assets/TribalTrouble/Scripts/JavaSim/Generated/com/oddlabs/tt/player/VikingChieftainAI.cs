// Translated from VikingChieftainAI.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player
{
    // Java: 11
    public class VikingChieftainAI : com.oddlabs.tt.player.ChieftainAI
    {
        public const int NUM_UNITS_FOR_STUN = 7;
        public const int NUM_UNITS_FOR_BLAST = 12;
        public const int NUM_UNITS_FOR_CONVERT = 15;
        // Java: 16
        public override void decide(com.oddlabs.tt.model.Unit chieftain)
        {
            nodeBlast(chieftain);
            nodeStun(chieftain);
            nodeConvert(chieftain);
        }
        // Java: 23
        public void nodeStun(com.oddlabs.tt.model.Unit chieftain)
        {
            if (chieftain.getMagicProgress(com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_STUN) < 1)
            {
                return;
            }
            float hit_radius = 33.0f;
            int num_enemy_units = numEnemyUnits(chieftain.getOwner());
            int num_enemy_units_close = getNumEnemyUnitsClose(chieftain, hit_radius, typeof(com.oddlabs.tt.model.Unit));
            if (num_enemy_units_close >= com.oddlabs.tt.player.VikingChieftainAI.NUM_UNITS_FOR_STUN || (num_enemy_units < com.oddlabs.tt.player.VikingChieftainAI.NUM_UNITS_FOR_STUN && num_enemy_units_close > 3) || (chieftain.getHitPoints() <= 2 && num_enemy_units_close > 2))
            {
                chieftain.doMagic(com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_STUN, false);
            }
        }
        // Java: 37
        public void nodeBlast(com.oddlabs.tt.model.Unit chieftain)
        {
            if (chieftain.getMagicProgress(com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_BLAST) < 1)
            {
                return;
            }
            float hit_radius = chieftain.getOwner().getRace().getMagicFactory(1).getHitRadius();
            int num_enemy_units = numEnemyUnits(chieftain.getOwner());
            int num_enemy_units_close = getNumEnemyUnitsClose(chieftain, hit_radius, com.oddlabs.tt.model.Selectable.genericClass());
            int num_friendly_units_close = getNumFriendlyUnitsClose(chieftain, hit_radius);
            if (2 * num_friendly_units_close < num_enemy_units_close && (num_enemy_units_close >= com.oddlabs.tt.player.VikingChieftainAI.NUM_UNITS_FOR_BLAST || (num_enemy_units < com.oddlabs.tt.player.VikingChieftainAI.NUM_UNITS_FOR_BLAST && num_enemy_units_close > 6) || (chieftain.getHitPoints() <= 4 && num_enemy_units_close > 5)))
            {
                chieftain.doMagic(com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_BLAST, false);
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
            int num_enemy_units_close = getNumEnemyUnitsClose(chieftain, hit_radius, typeof(com.oddlabs.tt.model.Unit));
            if (num_enemy_units_close >= com.oddlabs.tt.player.VikingChieftainAI.NUM_UNITS_FOR_CONVERT || (num_enemy_units < com.oddlabs.tt.player.VikingChieftainAI.NUM_UNITS_FOR_CONVERT && num_enemy_units_close > 4) || (chieftain.getHitPoints() <= 3 && num_enemy_units_close > 1))
            {
                chieftain.doMagic(com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_CONVERT, false);
            }
        }
        // Java: 69
        public int getNumEnemyUnitsClose(com.oddlabs.tt.model.Unit chieftain, float hit_radius, System.Type type)
        {
            com.oddlabs.tt.pathfinder.FindOccupantFilter filter = new com.oddlabs.tt.pathfinder.FindOccupantFilter(chieftain.getPositionX(), chieftain.getPositionY(), hit_radius, chieftain, type);
            chieftain.getUnitGrid().scan(filter, chieftain.getGridX(), chieftain.getGridY());
            long num_enemy_units_close = java.util.stream.StreamSupport.stream(filter.getResult().spliterator(), false).filter(new java.util.function.Predicate_L((object a0) => { return ((com.oddlabs.tt.model.Selectable)a0).isAlive(); })).filter(new java.util.function.Predicate_L((object s_l) =>
            {
                com.oddlabs.tt.model.Selectable s = (com.oddlabs.tt.model.Selectable)s_l;
                float dx = (float)(s.getPositionX() - chieftain.getPositionX());
                float dy = (float)(s.getPositionY() - chieftain.getPositionY());
                float squared_dist = (float)((float)(dx * dx) + (float)(dy * dy));
                return chieftain.getOwner().isEnemy(s.getOwner()) && squared_dist < (float)(hit_radius * hit_radius);
            })).count();
            return unchecked((int)(num_enemy_units_close));
        }
        // Java: 84
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
        public VikingChieftainAI() : base()
        {
        }
    }
}
