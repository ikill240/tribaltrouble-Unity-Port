using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace TribalTrouble.FaithfulGame
{
    /// <summary>
    /// Identity keys for the view's dictionaries of simulation objects. The translated Java objects hash through
    /// java.lang.Object.hashCode (a locked weak-table lookup per call), which cost thousands of locks a frame with
    /// many units and trees; the view only ever needs reference identity.
    /// </summary>
    public sealed class RefEq : IEqualityComparer<object>
    {
        public static readonly RefEq Instance = new RefEq();
        public new bool Equals(object a, object b) => ReferenceEquals(a, b);
        public int GetHashCode(object o) => RuntimeHelpers.GetHashCode(o);
    }
}
