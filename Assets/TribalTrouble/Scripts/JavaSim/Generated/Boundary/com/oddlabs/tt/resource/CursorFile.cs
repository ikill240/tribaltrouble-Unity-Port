// GUI port: cursor images are shown by the engine; this only names the image and hot spot.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.resource
{
    public class CursorFile : com.oddlabs.tt.resource.File
    {
        readonly string _source;
        readonly int _x, _y;
        public CursorFile(string source, int xHot, int yHot) : base(java.net.URI.create("res:" + source)) { _source = source; _x = xHot; _y = yHot; }
        public override object get() => new com.oddlabs.tt.gui.Cursor(_source, _x, _y);
        public override bool equals(object o) => o is CursorFile f && f._source == _source;
        // ---- merged from the generated stub (members the translated code now uses)
        public CursorFile() : base(default(java.net.URI)) { }
    }
}
