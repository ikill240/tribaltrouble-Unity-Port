// Translated from FontInfo.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.util
{
    // Java: 15
    public class FontInfo : java.lang.Object
    {
        public const long serialVersionUID = 1L;
        public string texture_name;
        public com.oddlabs.util.HashTable key_map;
        public int x_border;
        public int y_border;
        public int font_height;
        public int max_ascension;
        public int max_descension;
        // Java: 40
        public string getTextureName()
        {
            return texture_name;
        }
        // Java: 44
        public com.oddlabs.util.HashTable getKeyMap()
        {
            return key_map;
        }
        // Java: 48
        public int getBorderX()
        {
            return x_border;
        }
        // Java: 52
        public int getBorderY()
        {
            return y_border;
        }
        // Java: 56
        public int getHeight()
        {
            return font_height;
        }
        // Java: 60
        public int getMaxAscension()
        {
            return max_ascension;
        }
        // Java: 64
        public int getMaxDescension()
        {
            return max_descension;
        }
        // Java: 68
        public void saveToFile(java.nio.file.Path file_name)
        {
            {
                java.io.ObjectOutputStream os = new java.io.ObjectOutputStream(new java.io.BufferedOutputStream(java.nio.file.Files.newOutputStream(file_name)));
                try
                {
                    os.writeObject(this);
                }
                catch (System.IO.IOException e)
                {
                    throw new System.IO.IOException(java.lang.JStr.valueOf(e), (System.Exception)e);
                }
                finally
                {
                    if (os != null) os.close();
                }
            }
        }
        // Java: 77
        public static com.oddlabs.util.FontInfo loadFromFile(java.net.URL url)
        {
            return ((com.oddlabs.util.FontInfo)com.oddlabs.util.Utils.loadObject(url));
        }
        public FontInfo(string texture_name, com.oddlabs.util.HashTable key_map, int x_border, int y_border, int font_height, int max_ascension, int max_descension) : base()
        {
            this.texture_name = texture_name;
            this.key_map = key_map;
            this.x_border = x_border;
            this.y_border = y_border;
            this.font_height = font_height;
            this.max_ascension = max_ascension;
            this.max_descension = max_descension;
        }
    }
}
