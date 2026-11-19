// Hand-written replacement for the generated stub: records sprite registrations instead of loading OpenGL data.
#pragma warning disable
namespace com.oddlabs.tt.render
{
    public class RenderQueues : java.lang.Object
    {
        public virtual com.oddlabs.tt.render.SpriteKey register(com.oddlabs.tt.resource.SpriteFile f) { return new com.oddlabs.tt.render.SpriteKey(f.getLocation(), 0); }
        public virtual com.oddlabs.tt.render.SpriteKey register(com.oddlabs.tt.resource.SpriteFile f, int texture) { return new com.oddlabs.tt.render.SpriteKey(f.getLocation(), texture); }
        public virtual com.oddlabs.tt.render.ShadowListKey registerSelectableShadowList(java.util.function.Supplier p0) { return new com.oddlabs.tt.render.ShadowListKey(); }
        public virtual com.oddlabs.tt.render.TextureKey registerTexture(java.util.function.Supplier p0, int p1) { return new com.oddlabs.tt.render.TextureKey(p0, p1); }
        public virtual com.oddlabs.tt.render.TextureKey registerTexture(java.util.function.Supplier p0) { return new com.oddlabs.tt.render.TextureKey(p0, 0); }
        public RenderQueues() { }
        public virtual void close() { }
        // ---- merged from the generated stub (members the translated code now uses)
        public virtual com.oddlabs.tt.render.SpriteRenderer getRenderer(com.oddlabs.tt.render.SpriteKey p0) { return new com.oddlabs.tt.render.SpriteRenderer(); }
    }
}
