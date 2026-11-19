// Translated from Accessories.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 7
    public abstract class Accessories : com.oddlabs.tt.model.Model
    {
        public com.oddlabs.tt.render.SpriteKey sprite_renderer;
        // Java: 16
        public override com.oddlabs.tt.render.SpriteKey getSpriteRenderer()
        {
            return sprite_renderer;
        }
        public Accessories(com.oddlabs.tt.landscape.World world, com.oddlabs.tt.render.SpriteKey sprite_renderer) : base(world)
        {
            this.sprite_renderer = sprite_renderer;
            register();
        }
    }
}
