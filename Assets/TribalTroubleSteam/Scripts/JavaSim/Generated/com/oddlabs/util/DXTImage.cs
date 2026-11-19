// Translated from DXTImage.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.util
{
    // Java: 23
    public class DXTImage : java.lang.Object
    {
        public const int MAGIC = 542327876;
        public const int FOURCC_DXT1 = 827611204;
        public const int FOURCC_DXT3 = 861165636;
        public const int FOURCC_DXT5 = 894720068;
        public const int FOURCC_DX10 = 808540228;
        public const int DXGI_FORMAT_BC1_UNORM = 71;
        public const int DXGI_FORMAT_BC1_UNORM_SRGB = 72;
        public const int DXGI_FORMAT_BC2_UNORM = 74;
        public const int DXGI_FORMAT_BC2_UNORM_SRGB = 75;
        public const int DXGI_FORMAT_BC3_UNORM = 77;
        public const int DXGI_FORMAT_BC3_UNORM_SRGB = 78;
        public short width;
        public short height;
        public int fourCC;
        public sbyte[][] mipmaps;
        // Java: 50
        public int getWidth()
        {
            throw new System.NotSupportedException("J2CS stub: DXTImage.getWidth");
        }
        // Java: 54
        public int getHeight()
        {
            throw new System.NotSupportedException("J2CS stub: DXTImage.getHeight");
        }
        // Java: 58
        public int getWidth(int mipmap_level)
        {
            throw new System.NotSupportedException("J2CS stub: DXTImage.getWidth");
        }
        // Java: 62
        public int getHeight(int mipmap_level)
        {
            throw new System.NotSupportedException("J2CS stub: DXTImage.getHeight");
        }
        // Java: 66
        public int getFourCC()
        {
            throw new System.NotSupportedException("J2CS stub: DXTImage.getFourCC");
        }
        // Java: 70
        public int getNumMipMaps()
        {
            throw new System.NotSupportedException("J2CS stub: DXTImage.getNumMipMaps");
        }
        // Java: 74
        public java.nio.ByteBuffer getMipMap()
        {
            throw new System.NotSupportedException("J2CS stub: DXTImage.getMipMap");
        }
        // Java: 78
        public java.nio.ByteBuffer getMipMap(int mipmap_level)
        {
            throw new System.NotSupportedException("J2CS stub: DXTImage.getMipMap");
        }
        // Java: 85
        public void position(int mipmap_level)
        {
            throw new System.NotSupportedException("J2CS stub: DXTImage.position");
        }
        // Java: 89
        public void write(java.nio.file.Path file)
        {
            throw new System.NotSupportedException("J2CS stub: DXTImage.write");
        }
        // Java: 130
        public static com.oddlabs.util.DXTImage read(java.net.URL url)
        {
            throw new System.NotSupportedException("J2CS stub: DXTImage.read");
        }
        // Java: 136
        public static com.oddlabs.util.DXTImage read(java.nio.file.Path file)
        {
            throw new System.NotSupportedException("J2CS stub: DXTImage.read");
        }
        // Java: 142
        public static com.oddlabs.util.DXTImage read(java.nio.channels.ReadableByteChannel @in)
        {
            throw new System.NotSupportedException("J2CS stub: DXTImage.read");
        }
        // Java: 201
        public static void writeContents(java.nio.channels.WritableByteChannel @out, java.nio.ByteBuffer data)
        {
            throw new System.NotSupportedException("J2CS stub: DXTImage.writeContents");
        }
        public DXTImage(short width, short height, int fourCC, sbyte[][] mipmaps) { }
    }
}
