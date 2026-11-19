// GUI port: a cursor is an image of /textures/gui plus its hot spot; the engine shows it (Host.Backend.SetCursor).
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    public class Cursor : com.oddlabs.tt.resource.NativeResource
    {
        static readonly System.Collections.Generic.List<Cursor> s_all = new System.Collections.Generic.List<Cursor>();
        public static com.oddlabs.tt.gui.Cursor NULL_CURSOR = new Cursor((string)null, 0, 0);
        /// <summary>The image the host hides the pointer for (Steam: CursorType.HIDDEN, a blank 1x1 image).</summary>
        public const string HIDDEN = "#hidden";
        public readonly string image;
        public readonly int hotX, hotY;
        readonly long _id;

        public Cursor(string image, int hotX, int hotY) : base(default(com.oddlabs.tt.resource.NativeResource.NativeState))
        {
            this.image = image; this.hotX = hotX; this.hotY = hotY;
            lock (s_all) { s_all.Add(this); _id = s_all.Count; }
        }

        public Cursor() : this((string)null, 0, 0) { }

        /// <summary>Steam: a cursor made from an image in memory - only the blank HIDDEN one is.</summary>
        public Cursor(com.oddlabs.tt.resource.GLImage image, int hotX, int hotY) : this(HIDDEN, hotX, hotY) { }

        public virtual long getCursor() => image == null ? 0 : _id;

        public static Cursor ById(long id)
        {
            lock (s_all) return id > 0 && id <= s_all.Count ? s_all[(int)id - 1] : null;
        }

        public virtual void close() { }
    }
}
