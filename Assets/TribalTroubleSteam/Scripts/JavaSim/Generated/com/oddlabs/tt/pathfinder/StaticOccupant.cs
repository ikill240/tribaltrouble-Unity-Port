// Translated from StaticOccupant.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.pathfinder
{
    // Java: 3
    public class StaticOccupant : java.lang.Object, com.oddlabs.tt.pathfinder.Occupant
    {
        // Java: 4
        public int getPenalty()
        {
            return com.oddlabs.tt.pathfinder.Occupant.STATIC;
        }
        // Java: 9
        public int getGridX()
        {
            throw new System.Exception();
        }
        // Java: 14
        public int getGridY()
        {
            throw new System.Exception();
        }
        // Java: 19
        public float getPositionX()
        {
            throw new System.Exception();
        }
        // Java: 24
        public float getPositionY()
        {
            throw new System.Exception();
        }
        // Java: 29
        public float getSize()
        {
            throw new System.Exception();
        }
        // Java: 34
        public bool isDead()
        {
            throw new System.Exception();
        }
        // Java: 39
        public void startRespond()
        {
            throw new System.Exception();
        }
        // Java: 43
        public void stopRespond()
        {
            throw new System.Exception();
        }
        public StaticOccupant() : base()
        {
        }
        // mixin of default method Target.isAlive
        public bool isAlive()
        {
            return !isDead();
        }
    }
}
