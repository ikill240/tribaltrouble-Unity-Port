// Translated from LandscapeOverride.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.procedural
{
    // Java: 17
    public class LandscapeOverride : java.lang.Object
    {
        public float[][] heights() { return heights_f; }
        public com.oddlabs.tt.procedural.LandscapeOverride.Resources resources() { return resources_f; }
        public java.util.List starts() { return starts_f; }
        public LandscapeOverride(float[][] heights, com.oddlabs.tt.procedural.LandscapeOverride.Resources resources, java.util.List starts)
        {
            this.heights_f = heights;
            this.resources_f = resources;
            this.starts_f = starts;
        }
        public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is LandscapeOverride r)) return false; return true && java.lang.JO.equals(heights_f, r.heights_f) && java.lang.JO.equals(resources_f, r.resources_f) && java.lang.JO.equals(starts_f, r.starts_f); }
        public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(heights_f); h = 31 * h + java.lang.JO.hashCode(resources_f); h = 31 * h + java.lang.JO.hashCode(starts_f); } return h; }
        public override string toString() { return "LandscapeOverride[" + "heights=" + java.lang.JO.toString(heights_f) + ", resources=" + java.lang.JO.toString(resources_f) + ", starts=" + java.lang.JO.toString(starts_f) + "]"; }
        public float[][] heights_f;
        public com.oddlabs.tt.procedural.LandscapeOverride.Resources resources_f;
        public java.util.List starts_f;
        // Java: 20
        public class Resources : java.lang.Object
        {
            public java.util.List trees() { return trees_f; }
            public java.util.List palm_trees() { return palm_trees_f; }
            public java.util.List rocks() { return rocks_f; }
            public java.util.List iron() { return iron_f; }
            public Resources(java.util.List trees, java.util.List palm_trees, java.util.List rocks, java.util.List iron)
            {
                this.trees_f = trees;
                this.palm_trees_f = palm_trees;
                this.rocks_f = rocks;
                this.iron_f = iron;
            }
            public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is Resources r)) return false; return true && java.lang.JO.equals(trees_f, r.trees_f) && java.lang.JO.equals(palm_trees_f, r.palm_trees_f) && java.lang.JO.equals(rocks_f, r.rocks_f) && java.lang.JO.equals(iron_f, r.iron_f); }
            public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(trees_f); h = 31 * h + java.lang.JO.hashCode(palm_trees_f); h = 31 * h + java.lang.JO.hashCode(rocks_f); h = 31 * h + java.lang.JO.hashCode(iron_f); } return h; }
            public override string toString() { return "Resources[" + "trees=" + java.lang.JO.toString(trees_f) + ", palm_trees=" + java.lang.JO.toString(palm_trees_f) + ", rocks=" + java.lang.JO.toString(rocks_f) + ", iron=" + java.lang.JO.toString(iron_f) + "]"; }
            public java.util.List trees_f;
            public java.util.List palm_trees_f;
            public java.util.List rocks_f;
            public java.util.List iron_f;
        }
        // Java: 28
        public interface Source
        {
            // Java: 29
            com.oddlabs.tt.procedural.LandscapeOverride load();
        }
        public sealed class Source_L : java.lang.Object, Source
        {
            readonly System.Func<com.oddlabs.tt.procedural.LandscapeOverride> f;
            public Source_L(System.Func<com.oddlabs.tt.procedural.LandscapeOverride> f) { this.f = f; }
            public com.oddlabs.tt.procedural.LandscapeOverride load() { return f(); }
        }
    }
}
