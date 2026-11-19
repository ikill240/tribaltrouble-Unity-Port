// GUI port: a texture is a handle in the engine (TribalTrouble.GuiPort.Host.Backend), made lazily the first time the
// GUI asks for it (world loads run on a worker thread, and most generated textures - terrain, sky, sea, particles -
// are uploaded by the engine's own world view from 'images', never through a handle).
#pragma warning disable
using J2CS.Ext;
using TribalTrouble.GuiPort;
namespace com.oddlabs.tt.render
{
    public class Texture : com.oddlabs.tt.resource.NativeResource
    {
        /// <summary>The source images (generated textures), for engines that upload them themselves.</summary>
        public readonly com.oddlabs.tt.resource.GLImage[] images;
        readonly string _path;
        int _handle = -1, _w, _h;
        bool _made, _closed;

        public Texture() : base(default(com.oddlabs.tt.resource.NativeResource.NativeState)) { _made = true; }

        public Texture(com.oddlabs.tt.resource.TextureFile file) : base(default(com.oddlabs.tt.resource.NativeResource.NativeState))
        {
            string path = file.getURL().toString();
            if (path.StartsWith("res:")) path = path.Substring(4);
            _path = path;
        }

        public Texture(int width, int height, int internal_format, int min_filter, int mag_filter, int wrap) : base(default(com.oddlabs.tt.resource.NativeResource.NativeState))
        {
            _w = width; _h = height;
        }

        public Texture(com.oddlabs.tt.resource.GLImage image, int internal_format, int min_filter, int mag_filter, int wrap_s, int wrap_t)
            : this(new[] { image }, internal_format, min_filter, mag_filter, wrap_s, wrap_t) { }

        public Texture(com.oddlabs.tt.resource.GLImage[] mipmaps, int internal_format, int min_filter, int mag_filter, int wrap_s, int wrap_t)
            : base(default(com.oddlabs.tt.resource.NativeResource.NativeState))
        {
            images = mipmaps;
            _w = mipmaps[0].getWidth(); _h = mipmaps[0].getHeight();
        }

        int[] _pixels;   // ABGR, row 0 at the bottom (FromRGB)

        /// <summary>A texture from size x size RGB bytes (PortHost.textureFromRGB: map previews).</summary>
        public static Texture FromRGB(int size, sbyte[] rgb)
        {
            var t = new Texture(size, size, 0, 0, 0, 0);
            var px = new int[size * size];
            for (int i = 0; i < px.Length; i++)
                px[i] = (rgb[3 * i] & 0xff) | (rgb[3 * i + 1] & 0xff) << 8 | (rgb[3 * i + 2] & 0xff) << 16 | unchecked((int)0xff000000);
            t._pixels = px;
            return t;
        }

        void Make()
        {
            lock (this)
            {
                if (_made || _closed) return;
                _made = true;
                if (_path != null) _handle = Host.Backend.LoadTexture(_path, out _w, out _h);
                else if (images != null)
                {
                    var img = images[0];
                    var px = new int[_w * _h];
                    for (int y = 0; y < _h; y++)
                        for (int x = 0; x < _w; x++) px[y * _w + x] = img.getPixel(x, y);
                    _handle = Host.Backend.CreateTexture(_w, _h, px);
                }
                else _handle = Host.Backend.CreateTexture(_w, _h, _pixels ?? new int[_w * _h]);
            }
        }

        public virtual int getHandle() { Make(); return _handle; }
        public virtual int getWidth() { if (_path != null) Make(); return _w; }
        public virtual int getHeight() { if (_path != null) Make(); return _h; }
        public static int globalSize() => 0;
        public virtual void close() { lock (this) { _closed = true; if (_handle >= 0) Host.Backend.ReleaseTexture(_handle); _handle = -1; } }
    }
}
