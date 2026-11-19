// Translated from SpamFilter.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.util
{
    // Java: 5
    public class SpamFilter : java.lang.Object
    {
        // Java: 6
        public static string scan(string @string)
        {
            return @string.replaceAll("[\\t\\r\\n\\f]+", " ");
        }
        public SpamFilter() : base()
        {
        }
    }
}
