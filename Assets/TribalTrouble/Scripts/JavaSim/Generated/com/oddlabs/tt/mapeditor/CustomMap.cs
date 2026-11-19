// Translated from CustomMap.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.mapeditor
{
    // Java: 9
    public class CustomMap : java.lang.Object
    {
        public string name;
        public com.oddlabs.tt.mapeditor.MapSettings settings;
        public java.nio.file.Path path;
        // Java: 20
        public string getName()
        {
            return name;
        }
        // Java: 25
        public int getSizeIndex()
        {
            return settings.size();
        }
        // Java: 30
        public int getMetersPerWorld()
        {
            return settings.getMetersPerWorld();
        }
        // Java: 34
        public bool isArchipelago()
        {
            return settings.isArchipelago();
        }
        // Java: 38
        public string getMapcode()
        {
            return settings.toMapcode();
        }
        // Java: 43
        public com.oddlabs.tt.resource.WorldGenerator createGenerator()
        {
            return settings.createGenerator(new com.oddlabs.tt.mapeditor.SavedMapSource(path));
        }
        public CustomMap(string name, com.oddlabs.tt.mapeditor.MapSettings settings, java.nio.file.Path path) : base()
        {
            this.name = name;
            this.settings = settings;
            this.path = path;
        }
    }
}
