// Translated from ChieftainAI.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player
{
    // Java: 8
    public abstract class ChieftainAI : java.lang.Object
    {
        // Java: 9
        public abstract void decide(com.oddlabs.tt.model.Unit chieftain);
        // Java: 11
        public int numEnemyUnits(com.oddlabs.tt.player.Player owner)
        {
            com.oddlabs.tt.player.Player[] players = owner.getWorld().getPlayers();
            int count = java.util.Arrays.stream(players).filter(new java.util.function.Predicate_L((object a0) => { return owner.isEnemy(((com.oddlabs.tt.player.Player)a0)); })).mapToInt(new java.util.function.ToIntFunction_L((object p_l) =>
            {
                com.oddlabs.tt.player.Player p = (com.oddlabs.tt.player.Player)p_l;
                return p.getUnits().size();
            })).sum();
            return count;
        }
        public ChieftainAI() : base()
        {
        }
    }
}
