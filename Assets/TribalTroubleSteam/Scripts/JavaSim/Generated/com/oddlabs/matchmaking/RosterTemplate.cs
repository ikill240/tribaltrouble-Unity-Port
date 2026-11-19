// Translated from RosterTemplate.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchmaking
{
    // Java: 16
    public class RosterTemplate : java.lang.Object
    {
        public const long serialVersionUID = 1L;
        // Java: 20
        public sealed class Fill : java.lang.Enum
        {
            public static Fill[] values() { return (Fill[])VALUES_.Clone(); }
            public static Fill valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly Fill HOST = new Fill("HOST", 0);
            public static readonly Fill OPEN = new Fill("OPEN", 1);
            public static readonly Fill CLOSED = new Fill("CLOSED", 2);
            public static readonly Fill EASY_AI = new Fill("EASY_AI", 3);
            public static readonly Fill NORMAL_AI = new Fill("NORMAL_AI", 4);
            public static readonly Fill HARD_AI = new Fill("HARD_AI", 5);
            public static readonly Fill[] VALUES_ = new Fill[] { HOST, OPEN, CLOSED, EASY_AI, NORMAL_AI, HARD_AI };
            public Fill(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        // Java: 29
        public class Slot : java.lang.Object
        {
            public const long serialVersionUID = 1L;
            public com.oddlabs.matchmaking.RosterTemplate.Fill fill;
            public int? race;
            public int? team;
            // Java: 46
            public com.oddlabs.matchmaking.RosterTemplate.Fill getFill()
            {
                return fill;
            }
            // Java: 50
            public int? getRace()
            {
                return race;
            }
            // Java: 54
            public int? getTeam()
            {
                return team;
            }
            public Slot(com.oddlabs.matchmaking.RosterTemplate.Fill fill, int? race, int? team) : base()
            {
                this.fill = fill;
                this.race = race;
                this.team = team;
            }
        }
        public com.oddlabs.matchmaking.RosterTemplate.Slot[] slots;
        // Java: 66
        public com.oddlabs.matchmaking.RosterTemplate.Slot[] getSlots()
        {
            return ((com.oddlabs.matchmaking.RosterTemplate.Slot[])java.util.Arrays.copyOf(slots, slots.Length));
        }
        public RosterTemplate(com.oddlabs.matchmaking.RosterTemplate.Slot[] slots) : base()
        {
            this.slots = ((com.oddlabs.matchmaking.RosterTemplate.Slot[])java.util.Arrays.copyOf(slots, slots.Length));
        }
    }
}
