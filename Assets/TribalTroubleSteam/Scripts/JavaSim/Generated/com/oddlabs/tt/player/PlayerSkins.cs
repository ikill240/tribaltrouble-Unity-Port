// Translated from PlayerSkins.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player
{
    // Java: 16
    public class PlayerSkins : java.lang.Object
    {
        public static com.oddlabs.tt.player.PlayerSkins NONE;
        public java.util.Map units;
        public java.util.Map buildings;
        public java.util.Map props;
        public java.util.Map items;
        // Java: 35
        public com.oddlabs.tt.render.SpriteKey rendererFor(com.oddlabs.tt.model.UnitTemplate template)
        {
            com.oddlabs.tt.render.SpriteKey skin = ((com.oddlabs.tt.render.SpriteKey)units.get(template));
            return (skin != null ? skin : template.getSpriteRenderer());
        }
        // Java: 40
        public com.oddlabs.tt.render.SpriteKey rendererFor(com.oddlabs.tt.model.BuildingTemplate template, com.oddlabs.tt.model.Building.BuildState stage)
        {
            com.oddlabs.tt.render.SpriteKey skin = ((com.oddlabs.tt.render.SpriteKey)((java.util.Map)buildings.getOrDefault(template, java.util.Map_.of())).get(stage));
            return (skin != null ? skin : template.getRenderer(stage));
        }
        // Java: 46
        public java.util.List propsFor(com.oddlabs.tt.model.BuildingTemplate template, com.oddlabs.tt.model.Building.BuildState stage)
        {
            java.util.List skin = ((java.util.List)((java.util.Map)props.getOrDefault(template, java.util.Map_.of())).get(stage));
            return (skin != null ? skin : template.getProps(stage));
        }
        // Java: 52
        public com.oddlabs.tt.render.SpriteKey itemFor(com.oddlabs.tt.render.SpriteKey item)
        {
            return ((com.oddlabs.tt.render.SpriteKey)items.getOrDefault(item, item));
        }
        static PlayerSkins()
        {
            PlayerSkins.NONE = new com.oddlabs.tt.player.PlayerSkins(java.util.Map_.of(), java.util.Map_.of(), java.util.Map_.of(), java.util.Map_.of());
        }
        public PlayerSkins(java.util.Map units, java.util.Map buildings, java.util.Map props, java.util.Map items) : base()
        {
            this.units = units;
            this.buildings = buildings;
            this.props = props;
            this.items = items;
        }
    }
}
