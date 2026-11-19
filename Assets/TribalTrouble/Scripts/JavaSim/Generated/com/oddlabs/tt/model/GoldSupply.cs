// Translated from GoldSupply.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 8
    public class GoldSupply : com.oddlabs.tt.model.SupplyModel
    {
        public const int INITIAL_SUPPLIES = 20;
        // Java: 16
        public override com.oddlabs.tt.model.Supply respawn()
        {
            return new com.oddlabs.tt.model.GoldSupply(getWorld(), getSpriteRenderer(), getSize(), getGridX(), getGridY(), getPositionX(), getPositionY(), 0, false);
        }
        public GoldSupply(com.oddlabs.tt.landscape.World world, com.oddlabs.tt.render.SpriteKey sprite_renderer, float size, int grid_x, int grid_y, float x, float y, float rotation, bool increase) : base(world, sprite_renderer, size, grid_x, grid_y, x, y, rotation, com.oddlabs.tt.model.GoldSupply.INITIAL_SUPPLIES, increase)
        {
        }
    }
}
