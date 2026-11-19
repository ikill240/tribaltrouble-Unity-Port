// Translated from TextureFile.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.resource
{
    // Java: 23
    public class TextureFile : com.oddlabs.tt.resource.File
    {
        public static java.util.logging.Logger logger;
        public static string[] EXTENSIONS;
        public int internal_format;
        public int min_filter;
        public int mag_filter;
        public int wrap_s;
        public int wrap_t;
        public int base_fadeout_level;
        public int max_mipmap_level;
        public float fadeout_factor;
        public bool max_alpha;
        public bool is_dxt;
        // Java: 99
        public static java.net.URI locateTexture(string location)
        {
            return java.net.URI.create("res:" + location);
        }
        // Java: 109
        public bool isDXTImage()
        {
            return is_dxt;
        }
        // Java: 113
        public com.oddlabs.util.DXTImage getDXTImage()
        {
            throw new System.NotSupportedException("J2CS stub: TextureFile.getDXTImage");
        }
        // Java: 125
        public com.oddlabs.tt.resource.GLImage getImage()
        {
            throw new System.NotSupportedException("J2CS stub: TextureFile.getImage");
        }
        // Java: 137
        public override object get()
        {
            return new com.oddlabs.tt.render.Texture(this);
        }
        // Java: 142
        public override bool equals(object o)
        {
            return (o is com.oddlabs.tt.resource.TextureFile other) && internal_format == other.internal_format && min_filter == other.min_filter && mag_filter == other.mag_filter && max_mipmap_level == other.max_mipmap_level && wrap_s == other.wrap_s && wrap_t == other.wrap_t && base_fadeout_level == other.base_fadeout_level && fadeout_factor == other.fadeout_factor && max_alpha == other.max_alpha && base.equals(o);
        }
        // Java: 154
        public int getInternalFormat()
        {
            if (is_dxt)
            {
                {
                    switch ((getDXTImage().getFourCC()))
                    {
                        case 827611204:
                        {
                            return org.lwjgl.opengl.EXTTextureCompressionS3TC.GL_COMPRESSED_RGB_S3TC_DXT1_EXT;
                        }
                        case 894720068:
                        {
                            return org.lwjgl.opengl.EXTTextureCompressionS3TC.GL_COMPRESSED_RGBA_S3TC_DXT5_EXT;
                        }
                        default:
                        {
                            string msg = "Unsupported DXT format (FourCC): " + java.lang.JStr.valueOf(java.lang.Integer.toHexString(getDXTImage().getFourCC())) + " for texture: " + java.lang.JStr.valueOf(getURL());
                            com.oddlabs.tt.resource.TextureFile.logger.severe(msg);
                            throw new System.ArgumentException(msg);
                        }
                    }
                    throw new System.InvalidOperationException("MatchException");
                }
            }
            return internal_format;
        }
        // Java: 170
        public int getMinFilter()
        {
            return min_filter;
        }
        // Java: 174
        public int getMagFilter()
        {
            return mag_filter;
        }
        // Java: 178
        public int getWrapS()
        {
            return wrap_s;
        }
        // Java: 182
        public int getWrapT()
        {
            return wrap_t;
        }
        // Java: 186
        public int getBaseFadeoutLevel()
        {
            return base_fadeout_level;
        }
        // Java: 190
        public int getMaxMipmapLevel()
        {
            return max_mipmap_level;
        }
        // Java: 194
        public float getFadeoutFactor()
        {
            return fadeout_factor;
        }
        // Java: 198
        public bool hasMaxAlpha()
        {
            return max_alpha;
        }
        static TextureFile()
        {
            TextureFile.logger = java.util.logging.Logger.getLogger(typeof(com.oddlabs.tt.resource.TextureFile).getSimpleName());
            TextureFile.EXTENSIONS = new string[] { ".dds", ".image", ".png", ".jpg", ".jpeg" };
        }
        public TextureFile(string location) : this(location, com.oddlabs.tt.global.Globals.COMPRESSED_RGBA_FORMAT)
        {
        }
        public TextureFile(string location, int internal_format) : this(location, internal_format, org.lwjgl.opengl.GL11.GL_LINEAR_MIPMAP_LINEAR, org.lwjgl.opengl.GL11.GL_LINEAR, org.lwjgl.opengl.GL11.GL_REPEAT, org.lwjgl.opengl.GL11.GL_REPEAT)
        {
        }
        public TextureFile(string location, int internal_format, int min_filter, int mag_filter, int wrap_s, int wrap_t) : this(location, internal_format, min_filter, mag_filter, wrap_s, wrap_t, com.oddlabs.tt.global.Globals.NO_MIPMAP_CUTOFF, 10000, 1.0f)
        {
        }
        public TextureFile(string location, int internal_format, int min_filter, int mag_filter, int wrap_s, int wrap_t, int max_mipmap_level, int base_fadeout_level, float fadeout_factor) : this(location, internal_format, min_filter, mag_filter, wrap_s, wrap_t, max_mipmap_level, base_fadeout_level, fadeout_factor, false)
        {
        }
        public TextureFile(string location, int internal_format, int min_filter, int mag_filter, int wrap_s, int wrap_t, int max_mipmap_level, int base_fadeout_level, float fadeout_factor, bool max_alpha) : base(com.oddlabs.tt.resource.TextureFile.locateTexture(location))
        {
            this.is_dxt = java.lang.JO.toString(getURL()).endsWith(".dds");
            this.internal_format = internal_format;
            this.min_filter = min_filter;
            this.mag_filter = mag_filter;
            this.wrap_s = wrap_s;
            this.wrap_t = wrap_t;
            this.base_fadeout_level = base_fadeout_level;
            this.max_mipmap_level = max_mipmap_level;
            this.fadeout_factor = fadeout_factor;
            this.max_alpha = max_alpha;
        }
    }
}
