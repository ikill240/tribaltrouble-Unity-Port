// Translated from ChiefNames.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player
{
    // Java: 11
    public class ChiefNames : java.lang.Object
    {
        public static string[] VIKINGS;
        public static string[] NATIVES;
        // Java: 23
        public static string of(com.oddlabs.tt.player.Player owner, int n)
        {
            string[] names = (com.oddlabs.tt.model.ExtendedCombat.isVikings(owner) ? com.oddlabs.tt.player.ChiefNames.VIKINGS : com.oddlabs.tt.player.ChiefNames.NATIVES);
            int index = 0;
            com.oddlabs.tt.player.Player[] players = owner.getWorld().getPlayers();
            for (int i = 0; i < players.Length; i++)
            {
                if (((object)players[i] == (object)owner))
                {
                    index = i;
                }
            }
            return names[java.lang.Math.floorMod(index * 7 + n * 5 + java.lang.JO.hashCode(owner.getWorld().getMapCode()), names.Length)];
        }
        static ChiefNames()
        {
            ChiefNames.VIKINGS = new string[] { "Ragnar", "Bjorn", "Ivar", "Sigurd", "Harald", "Leif", "Eirik", "Gunnar", "Halfdan", "Ulf", "Torvald", "Rollo", "Sven", "Knut", "Olaf", "Hakon", "Egil", "Thorkel", "Ragnhild", "Astrid", "Freydis", "Sigrid", "Ingrid", "Brynja" };
            ChiefNames.NATIVES = new string[] { "Kaimana", "Makoa", "Keola", "Akamu", "Ikaika", "Nohea", "Kalani", "Mano", "Koa", "Tane", "Rangi", "Tama", "Kahiau", "Iolana", "Leilani", "Malia", "Moana", "Nalani", "Pua", "Wikolia", "Kekoa", "Hoku", "Alapai", "Kanoa" };
        }
        public ChiefNames() : base()
        {
        }
    }
}
