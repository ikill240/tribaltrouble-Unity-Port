// Translated from UnitInfo.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player
{
    // Java: 5
    public class UnitInfo : java.lang.Object
    {
        public bool hasQuarters() { return hasQuarters_f; }
        public bool hasArmory() { return hasArmory_f; }
        public int numTowers() { return numTowers_f; }
        public bool hasChieftain() { return hasChieftain_f; }
        public int numPeons() { return numPeons_f; }
        public int numRockWarriors() { return numRockWarriors_f; }
        public int numIronWarriors() { return numIronWarriors_f; }
        public int numRubberWarriors() { return numRubberWarriors_f; }
        public UnitInfo(bool hasQuarters, bool hasArmory, int numTowers, bool hasChieftain, int numPeons, int numRockWarriors, int numIronWarriors, int numRubberWarriors)
        {
            this.hasQuarters_f = hasQuarters;
            this.hasArmory_f = hasArmory;
            this.numTowers_f = numTowers;
            this.hasChieftain_f = hasChieftain;
            this.numPeons_f = numPeons;
            this.numRockWarriors_f = numRockWarriors;
            this.numIronWarriors_f = numIronWarriors;
            this.numRubberWarriors_f = numRubberWarriors;
        }
        public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is UnitInfo r)) return false; return true && java.lang.JO.equals(hasQuarters_f, r.hasQuarters_f) && java.lang.JO.equals(hasArmory_f, r.hasArmory_f) && java.lang.JO.equals(numTowers_f, r.numTowers_f) && java.lang.JO.equals(hasChieftain_f, r.hasChieftain_f) && java.lang.JO.equals(numPeons_f, r.numPeons_f) && java.lang.JO.equals(numRockWarriors_f, r.numRockWarriors_f) && java.lang.JO.equals(numIronWarriors_f, r.numIronWarriors_f) && java.lang.JO.equals(numRubberWarriors_f, r.numRubberWarriors_f); }
        public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(hasQuarters_f); h = 31 * h + java.lang.JO.hashCode(hasArmory_f); h = 31 * h + java.lang.JO.hashCode(numTowers_f); h = 31 * h + java.lang.JO.hashCode(hasChieftain_f); h = 31 * h + java.lang.JO.hashCode(numPeons_f); h = 31 * h + java.lang.JO.hashCode(numRockWarriors_f); h = 31 * h + java.lang.JO.hashCode(numIronWarriors_f); h = 31 * h + java.lang.JO.hashCode(numRubberWarriors_f); } return h; }
        public override string toString() { return "UnitInfo[" + "hasQuarters=" + java.lang.JO.toString(hasQuarters_f) + ", hasArmory=" + java.lang.JO.toString(hasArmory_f) + ", numTowers=" + java.lang.JO.toString(numTowers_f) + ", hasChieftain=" + java.lang.JO.toString(hasChieftain_f) + ", numPeons=" + java.lang.JO.toString(numPeons_f) + ", numRockWarriors=" + java.lang.JO.toString(numRockWarriors_f) + ", numIronWarriors=" + java.lang.JO.toString(numIronWarriors_f) + ", numRubberWarriors=" + java.lang.JO.toString(numRubberWarriors_f) + "]"; }
        public bool hasQuarters_f;
        public bool hasArmory_f;
        public int numTowers_f;
        public bool hasChieftain_f;
        public int numPeons_f;
        public int numRockWarriors_f;
        public int numIronWarriors_f;
        public int numRubberWarriors_f;
        // Java: 17
        public int numPeonsAndWarriors()
        {
            return numPeons_f + numRockWarriors_f + numIronWarriors_f + numRubberWarriors_f;
        }
        public UnitInfo() : this(false, false, 0, false, 0, 0, 0, 0)
        {
        }
    }
}
