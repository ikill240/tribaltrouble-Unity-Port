// Translated from SpriteFile.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.resource
{
    // Java: 7
    public class SpriteFile : com.oddlabs.tt.resource.File
    {
        public bool lighting;
        public bool cullface;
        public bool alpha;
        public bool modulate_color;
        public bool max_alpha;
        public int mipmap_cutoff;
        // Java: 31
        public override object get()
        {
            return new com.oddlabs.tt.render.SpriteList(this);
        }
        // Java: 36
        public override bool equals(object o)
        {
            return (o is com.oddlabs.tt.resource.SpriteFile other) && mipmap_cutoff == other.mipmap_cutoff && lighting == other.lighting && cullface == other.cullface && alpha == other.alpha && modulate_color == other.modulate_color && base.equals(o);
        }
        // Java: 47
        public com.oddlabs.tt.resource.SpriteFile withLocation(string location)
        {
            return new com.oddlabs.tt.resource.SpriteFile(location, mipmap_cutoff, lighting, cullface, alpha, modulate_color, max_alpha);
        }
        // Java: 51
        public string getLocation()
        {
            return java.lang.JO.toString(getURL());
        }
        // Java: 55
        public int getMipmapCutoff()
        {
            return mipmap_cutoff;
        }
        // Java: 59
        public bool isLighted()
        {
            return lighting;
        }
        // Java: 63
        public bool isCulled()
        {
            return cullface;
        }
        // Java: 67
        public bool hasAlpha()
        {
            return alpha;
        }
        // Java: 71
        public bool hasModulateColor()
        {
            return modulate_color;
        }
        // Java: 75
        public bool hasMaxAlpha()
        {
            return max_alpha;
        }
        public SpriteFile(string location, int mipmap_cutoff, bool lighting, bool cullface, bool alpha, bool modulate_color) : this(location, mipmap_cutoff, lighting, cullface, alpha, modulate_color, false)
        {
        }
        public SpriteFile(string location, int mipmap_cutoff, bool lighting, bool cullface, bool alpha, bool modulate_color, bool max_alpha) : base(location)
        {
            this.lighting = lighting;
            this.cullface = cullface;
            this.alpha = alpha;
            this.modulate_color = modulate_color;
            this.mipmap_cutoff = mipmap_cutoff;
            this.max_alpha = max_alpha;
        }
    }
}
