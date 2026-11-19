// Hand-written replacement for the generated stub: a SpriteKey remembers which sprite file and texture
// variant the simulation registered, so the Unity view can draw the right model. Animation types and
// bounds come from SpriteKey.Info (set by the presentation layer; defaults keep the simulation running headless).
#pragma warning disable
namespace com.oddlabs.tt.render
{
    public class SpriteKey : com.oddlabs.tt.render.RenderQueueKey
    {
        /// <summary>Presentation hook: animation types (0 = loop, 1 = plain) for a sprite location.</summary>
        public static System.Func<string, int[]> AnimationTypes = loc => null;
        public static readonly com.oddlabs.tt.util.BoundingBox EMPTY_BOUNDS = new com.oddlabs.tt.util.BoundingBox();

        public readonly string location;
        /// <summary>Steam: the sprite drawn for it (an active event's skin, else location itself).</summary>
        public string drawn;
        public readonly int texture;
        int[] anim_types;

        public SpriteKey() : base(0) { }
        public SpriteKey(string location, int texture) : base(0) { this.location = location; this.texture = texture; drawn = location; }

        /// <summary>The sprite's box around its origin. The port has no sprite geometry, and an empty box (min +inf,
        /// max -inf) sent every model down the element tree's top-right branch, into one corner leaf: the view's culling
        /// then hid rocks, iron, the new deposits and plants whenever that corner was off screen (zoomed in). A small
        /// box keeps each model in the leaf where it stands.</summary>
        public static readonly com.oddlabs.tt.util.BoundingBox DEFAULT_BOUNDS = MakeDefault();
        static com.oddlabs.tt.util.BoundingBox MakeDefault()
        {
            var b = new com.oddlabs.tt.util.BoundingBox();
            b.setBounds(-1.5f, 1.5f, -1.5f, 1.5f, 0f, 3f);
            return b;
        }

        public virtual com.oddlabs.tt.util.BoundingBox getBounds(int anim) { return DEFAULT_BOUNDS; }

        public virtual int getAnimationType(int anim)
        {
            if (anim_types == null) anim_types = (location != null ? AnimationTypes(location) : null) ?? new int[0];
            return anim >= 0 && anim < anim_types.Length ? anim_types[anim] : 0;
        }
    }
}
