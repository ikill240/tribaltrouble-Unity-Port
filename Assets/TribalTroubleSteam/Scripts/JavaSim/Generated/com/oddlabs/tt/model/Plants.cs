// Translated from Plants.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 9
    public class Plants : com.oddlabs.tt.model.SceneryModel
    {
        public bool fades_f;
        // Java: 25
        public bool fades()
        {
            return fades_f;
        }
        // Java: 29
        public override void doRegister()
        {
            if (com.oddlabs.tt.global.Globals.INSERT_PLANTS[com.oddlabs.tt.global.Settings.getSettings().graphic_detail])
            {
                register();
                reinsert();
            }
        }
        // Java: 37
        public override void visit(com.oddlabs.tt.model.ElementVisitor visitor)
        {
            visitor.visitPlants(this);
        }
        public Plants(com.oddlabs.tt.landscape.World world, float x, float y, float dir_x, float dir_y, com.oddlabs.tt.render.SpriteKey sprite_renderer, float shadow_diameter, float shadow_fade) : this(world, x, y, dir_x, dir_y, sprite_renderer, shadow_diameter, shadow_fade, true)
        {
        }
        public Plants(com.oddlabs.tt.landscape.World world, float x, float y, float dir_x, float dir_y, com.oddlabs.tt.render.SpriteKey sprite_renderer, float shadow_diameter, float shadow_fade, bool fades_v) : base(world, x, y, dir_x, dir_y, sprite_renderer, shadow_diameter, shadow_fade)
        {
            this.fades_f = fades_v;
        }
    }
}
