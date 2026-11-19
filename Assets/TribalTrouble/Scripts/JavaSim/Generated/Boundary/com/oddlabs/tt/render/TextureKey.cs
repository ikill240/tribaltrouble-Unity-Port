// Hand-written replacement for the generated stub: a TextureKey remembers what produced the texture
// (a procedural TextureGenerator or a TextureFile) and which of its textures, so the Unity view can build it.
#pragma warning disable
namespace com.oddlabs.tt.render
{
    public class TextureKey : com.oddlabs.tt.render.RenderQueueKey
    {
        public readonly java.util.function.Supplier source;
        public readonly int index;
        public TextureKey() : base(default(int)) { }
        public TextureKey(java.util.function.Supplier source, int index) : base(default(int)) { this.source = source; this.index = index; }

        /// <summary>"GeneratorSmoke", "GeneratorLightning", ... or the TextureFile location.</summary>
        public string Name => source == null ? null
            : source is com.oddlabs.tt.resource.TextureFile f ? f.getURL()?.toString() ?? f.ToString()
            : J2CS.Ext.JTypeExt.getSimpleName(source.GetType());
    }
}
