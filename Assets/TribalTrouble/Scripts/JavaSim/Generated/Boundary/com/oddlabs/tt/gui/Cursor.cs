// GUI port: a cursor is an image of /textures/gui plus its hot spot; the engine shows it (Host.Backend.SetCursor).
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    public class Cursor : com.oddlabs.tt.resource.NativeResource
    {
        static readonly System.Collections.Generic.List<Cursor> s_all = new System.Collections.Generic.List<Cursor>();
        public static com.oddlabs.tt.gui.Cursor NULL_CURSOR = new Cursor(null, 0, 0);
        public readonly string image;
        public readonly int hotX, hotY;
        readonly long _id;

        public Cursor(string image, int hotX, int hotY) : base(default(com.oddlabs.tt.resource.NativeResource.NativeState))
        {
            this.image = image; this.hotX = hotX; this.hotY = hotY;
            lock (s_all) { s_all.Add(this); _id = s_all.Count; }
        }

        public Cursor() : this(null, 0, 0) { }

        public virtual long getCursor() => image == null ? 0 : _id;

        public static Cursor ById(long id)
        {
            lock (s_all) return id > 0 && id <= s_all.Count ? s_all[(int)id - 1] : null;
        }

        public virtual void close() { }
    }
}
