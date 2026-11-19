// Translated from Brush.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.mapeditor
{
    // Java: 8
    public sealed class Brush : java.lang.Enum
    {
        public static Brush[] values() { return (Brush[])VALUES_.Clone(); }
        public static Brush valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
        public static readonly Brush HEIGHT = new Brush("HEIGHT", 0, "brush_height", "hint_height", null);
        public static readonly Brush FLATTEN = new Brush("FLATTEN", 1, "brush_flatten", "hint_flatten", null);
        public static readonly Brush SMOOTH = new Brush("SMOOTH", 2, "brush_smooth", "hint_smooth", null);
        public static readonly Brush RANDOM = new Brush("RANDOM", 3, "brush_random", "hint_random", null);
        public static readonly Brush RAMP = new Brush("RAMP", 4, "brush_ramp", "hint_ramp", null);
        public static readonly Brush CLIFF = new Brush("CLIFF", 5, "brush_cliff", "hint_cliff", null);
        public static readonly Brush PLATEAU = new Brush("PLATEAU", 6, "brush_plateau", "hint_plateau", null);
        public static readonly Brush TREES = new Brush("TREES", 7, "brush_trees", "hint_trees", com.oddlabs.tt.mapeditor.Resource.TREE);
        public static readonly Brush PALMS = new Brush("PALMS", 8, "brush_palms", "hint_trees", com.oddlabs.tt.mapeditor.Resource.PALM);
        public static readonly Brush ROCK = new Brush("ROCK", 9, "brush_rock", "hint_rock", com.oddlabs.tt.mapeditor.Resource.ROCK);
        public static readonly Brush IRON = new Brush("IRON", 10, "brush_iron", "hint_iron", com.oddlabs.tt.mapeditor.Resource.IRON);
        public static readonly Brush ERASE = new Brush("ERASE", 11, "brush_erase", "hint_erase", null);
        public static readonly Brush START = new Brush("START", 12, "brush_start", "hint_start", null);
        public string name_key;
        public string hint_key;
        public com.oddlabs.tt.mapeditor.Resource resource;
        // Java: 36
        public string getName(com.oddlabs.tt.procedural.Landscape.TerrainType terrain)
        {
            if (((object)resource == (object)com.oddlabs.tt.mapeditor.Resource.TREE) || ((object)resource == (object)com.oddlabs.tt.mapeditor.Resource.PALM))
            {
                return com.oddlabs.tt.mapeditor.MapEditor.i18n(java.lang.JStr.valueOf(name_key) + "_" + java.lang.JStr.valueOf(terrain.name().toLowerCase(java.util.Locale.ROOT)));
            }
            return com.oddlabs.tt.mapeditor.MapEditor.i18n(name_key);
        }
        // Java: 42
        public string getHint()
        {
            return com.oddlabs.tt.mapeditor.MapEditor.i18n(hint_key);
        }
        // Java: 47
        public bool isDragShape()
        {
            return ((object)this == (object)com.oddlabs.tt.mapeditor.Brush.RAMP);
        }
        // Java: 52
        public bool isResourceBrush()
        {
            return resource != null || ((object)this == (object)com.oddlabs.tt.mapeditor.Brush.ERASE);
        }
        // Java: 57
        public bool isPlacementBrush()
        {
            return ((object)this == (object)com.oddlabs.tt.mapeditor.Brush.START);
        }
        // Java: 62
        public com.oddlabs.tt.mapeditor.Resource getResource()
        {
            return resource;
        }
        public static readonly Brush[] VALUES_ = new Brush[] { HEIGHT, FLATTEN, SMOOTH, RANDOM, RAMP, CLIFF, PLATEAU, TREES, PALMS, ROCK, IRON, ERASE, START };
        public Brush(string name_, int ordinal_, string name_key, string hint_key, com.oddlabs.tt.mapeditor.Resource resource) : base(name_, ordinal_)
        {
            this.name_key = name_key;
            this.hint_key = hint_key;
            this.resource = resource;
        }
    }
}
