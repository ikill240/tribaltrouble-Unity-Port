// Hand-written replacement for the generated stub: records sprite registrations instead of loading OpenGL data.
// Steam port: like the Java RenderQueues it remembers each key's sprite file, texture and props (the geometry
// registry's extra meshes drawn with it), and which sprite an active event (Halloween..) draws in its place.
#pragma warning disable
namespace com.oddlabs.tt.render
{
    public class RenderQueues : java.lang.Object
    {
        sealed class Entry { public com.oddlabs.tt.resource.SpriteFile file; public int tex; public java.util.List props; }
        sealed class ByIdentity : System.Collections.Generic.IEqualityComparer<com.oddlabs.tt.render.SpriteKey>
        {
            public bool Equals(com.oddlabs.tt.render.SpriteKey a, com.oddlabs.tt.render.SpriteKey b) => ReferenceEquals(a, b);
            public int GetHashCode(com.oddlabs.tt.render.SpriteKey k) => System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(k);
        }
        readonly System.Collections.Generic.Dictionary<com.oddlabs.tt.render.SpriteKey, Entry> _entries =
            new System.Collections.Generic.Dictionary<com.oddlabs.tt.render.SpriteKey, Entry>(new ByIdentity());

        public virtual com.oddlabs.tt.render.SpriteKey register(com.oddlabs.tt.resource.SpriteFile f) => register(f, 0);

        public virtual com.oddlabs.tt.render.SpriteKey register(com.oddlabs.tt.resource.SpriteFile f, int texture)
        {
            var key = new com.oddlabs.tt.render.SpriteKey(f.getLocation(), texture);
            key.drawn = com.oddlabs.tt.model.GeometryRegistry.EventSkinLocation(f.getLocation());
            var e = new Entry { file = f, tex = texture, props = new java.util.ArrayList() };
            _entries[key] = e;
            // a skin only changes the look: the sprite keeps its own props and the skin adds its own
            var names = new System.Collections.Generic.List<string>();
            var own = com.oddlabs.tt.model.GeometryRegistry.getProps(f.getLocation());
            for (int i = 0; i < own.size(); i++) names.Add((string)own.get(i));
            if (key.drawn != f.getLocation())
            {
                var skin = com.oddlabs.tt.model.GeometryRegistry.getProps(key.drawn);
                for (int i = 0; i < skin.size(); i++) names.Add((string)skin.get(i));
            }
            foreach (var prop in names) e.props.add(register(f.withLocation(prop), texture));
            return key;
        }

        public virtual java.util.List getProps(com.oddlabs.tt.render.SpriteKey key) => key != null && _entries.TryGetValue(key, out var e) ? e.props : new java.util.ArrayList();
        public virtual int getTexIndex(com.oddlabs.tt.render.SpriteKey key) => key != null && _entries.TryGetValue(key, out var e) ? e.tex : 0;
        public virtual com.oddlabs.tt.resource.SpriteFile getSpriteFile(com.oddlabs.tt.render.SpriteKey key) => key != null && _entries.TryGetValue(key, out var e) ? e.file : null;

        public virtual com.oddlabs.tt.render.ShadowListKey registerSelectableShadowList(java.util.function.Supplier p0) { return new com.oddlabs.tt.render.ShadowListKey(p0); }
        public virtual com.oddlabs.tt.render.TextureKey registerTexture(java.util.function.Supplier p0, int p1) { return new com.oddlabs.tt.render.TextureKey(p0, p1); }
        public virtual com.oddlabs.tt.render.TextureKey registerTexture(java.util.function.Supplier p0) { return new com.oddlabs.tt.render.TextureKey(p0, 0); }
        public RenderQueues() { }
        public virtual void close() { }
    }
}
