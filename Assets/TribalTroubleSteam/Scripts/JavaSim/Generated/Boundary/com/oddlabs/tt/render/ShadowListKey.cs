// Hand-written replacement for the generated stub: a ShadowListKey remembers the halo textures' generator it was
// registered with (RenderQueues.registerSelectableShadowList: GeneratorHalos), so the Unity view can build them -
// each template draws its own halos (the buildings' are bigger and softer than the units').
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.render
{
    public class ShadowListKey : com.oddlabs.tt.render.RenderQueueKey
    {
        /// <summary>What makes the textures (GeneratorHalos: [SHADOWED, SELECTED]), or null.</summary>
        public readonly java.util.function.Supplier source;
        public ShadowListKey() : base(default(int)) { }
        public ShadowListKey(java.util.function.Supplier source) : base(default(int)) { this.source = source; }
    }
}
