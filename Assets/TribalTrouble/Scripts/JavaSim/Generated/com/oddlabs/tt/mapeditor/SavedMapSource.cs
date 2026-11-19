// Translated from SavedMapSource.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.mapeditor
{
    // Java: 14
    public class SavedMapSource : java.lang.Object, com.oddlabs.tt.procedural.LandscapeOverride.Source, com.oddlabs.tt.resource.TransferableMap
    {
        public const long serialVersionUID = 2L;
        public com.oddlabs.tt.resource.SharedMapLocator map_file;
        // Java: 26
        public com.oddlabs.tt.procedural.LandscapeOverride load()
        {
            com.oddlabs.tt.mapeditor.MapFile map = com.oddlabs.tt.mapeditor.MapFile.load(map_file.resolve());
            com.oddlabs.tt.mapeditor.MapFile.Resources resources = map.resources();
            return new com.oddlabs.tt.procedural.LandscapeOverride(map.heights(), (resources == null ? (com.oddlabs.tt.procedural.LandscapeOverride.Resources)(null) : new com.oddlabs.tt.procedural.LandscapeOverride.Resources(resources.of(com.oddlabs.tt.mapeditor.Resource.TREE), resources.of(com.oddlabs.tt.mapeditor.Resource.PALM), resources.of(com.oddlabs.tt.mapeditor.Resource.ROCK), resources.of(com.oddlabs.tt.mapeditor.Resource.IRON))), map.starts());
        }
        // Java: 35
        public string getMapFileName()
        {
            return map_file.getFileName();
        }
        // Java: 40
        public bool hasLocalCopy()
        {
            return map_file.hasLocalCopy();
        }
        // Java: 45
        public sbyte[] readCompressed()
        {
            return map_file.readCompressed();
        }
        // Java: 50
        public java.nio.file.Path installDownloadedMap(sbyte[] compressed)
        {
            return map_file.install(compressed, new com.oddlabs.tt.resource.SharedMapLocator.Validator_L((java.nio.file.Path a0) => { com.oddlabs.tt.mapeditor.MapFile.load(a0); }));
        }
        public SavedMapSource(java.nio.file.Path path) : base()
        {
            this.map_file = new com.oddlabs.tt.resource.SharedMapLocator(path);
        }
    }
}
