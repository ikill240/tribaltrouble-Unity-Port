// Translated from UnitSupplyContainer.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 7
    public class UnitSupplyContainer : com.oddlabs.tt.model.SupplyContainer
    {
        public java.util.Map supply_sprite_renderers;
        public System.Type type;
        // Java: 18
        public override int increaseSupply(int amount)
        {
            throw new System.Exception();
        }
        // Java: 23
        public int increaseSupply(int amount, System.Type type)
        {
            if (((object)this.type != (object)type))
            {
                this.type = type;
                base.increaseSupply(-base.getNumSupplies());
            }
            return base.increaseSupply(amount);
        }
        // Java: 31
        public void resetSupply(System.Type type)
        {
            if (((object)this.type == (object)type))
            {
                base.increaseSupply(-base.getNumSupplies());
            }
        }
        // Java: 37
        public System.Type getSupplyType()
        {
            return type;
        }
        // Java: 41
        public com.oddlabs.tt.render.SpriteKey getSupplySpriteRenderer(System.Type key)
        {
            return ((com.oddlabs.tt.render.SpriteKey)supply_sprite_renderers.get(key));
        }
        public UnitSupplyContainer(int max_resource_count, java.util.Map supply_sprite_renderers) : base(max_resource_count)
        {
            this.supply_sprite_renderers = supply_sprite_renderers;
        }
    }
}
