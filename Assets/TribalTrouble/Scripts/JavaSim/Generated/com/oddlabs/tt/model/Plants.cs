// Translated from Plants.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 9
    public class Plants : com.oddlabs.tt.model.SceneryModel
    {
        // Java: 15
        public override void doRegister()
        {
            if (com.oddlabs.tt.global.Globals.INSERT_PLANTS[com.oddlabs.tt.global.Settings.getSettings().graphic_detail])
            {
                register();
                reinsert();
            }
        }
        // Java: 23
        public override void visit(com.oddlabs.tt.model.ElementVisitor visitor)
        {
            visitor.visitPlants(this);
        }
        public Plants(com.oddlabs.tt.landscape.World world, float x, float y, float dir_x, float dir_y, com.oddlabs.tt.render.SpriteKey sprite_renderer) : base(world, x, y, dir_x, dir_y, sprite_renderer)
        {
        }
    }
}
