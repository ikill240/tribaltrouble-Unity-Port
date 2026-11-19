// Translated from TransferableMap.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.resource
{
    // Java: 16
    public interface TransferableMap
    {
        // Java: 18
        string getMapFileName();
        // Java: 21
        bool hasLocalCopy();
        // Java: 24
        sbyte[] readCompressed();
        // Java: 27
        java.nio.file.Path installDownloadedMap(sbyte[] compressed);
        // Java: 30
        public static com.oddlabs.tt.resource.TransferableMap of(com.oddlabs.tt.resource.WorldGenerator generator)
        {
            if ((generator is com.oddlabs.tt.resource.TransferableMap map))
            {
                return map;
            }
            if ((generator is com.oddlabs.tt.resource.IslandGenerator island))
            {
                com.oddlabs.tt.procedural.LandscapeOverride.Source source = island.getOverride();
                if ((source is com.oddlabs.tt.resource.TransferableMap map_2))
                {
                    return map_2;
                }
            }
            return null;
        }
    }
}
