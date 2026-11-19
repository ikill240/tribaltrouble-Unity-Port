// Translated from MethodComparator.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.net
{
    // Java: 7
    public class MethodComparator : java.lang.Object, java.util.Comparator
    {
        // Java: 8
        public int compare(object o1, object o2)
        {
            return java.lang.JO.toString(o1).compareTo(java.lang.JO.toString(o2));
        }
        public MethodComparator() : base()
        {
        }
    }
}
