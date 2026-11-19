// Translated from Template.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 6
    public abstract class Template : java.lang.Object
    {
        public com.oddlabs.tt.model.Abilities abilities;
        public com.oddlabs.tt.render.ShadowListKey shadow_renderer;
        public float[] hit_offset_z;
        public float no_detail_size;
        public float defense_chance;
        public float shadow_diameter;
        public string name;
        // Java: 26
        public string getName()
        {
            return name;
        }
        // Java: 30
        public com.oddlabs.tt.model.Abilities getAbilities()
        {
            return abilities;
        }
        // Java: 34
        public float getShadowDiameter()
        {
            return shadow_diameter;
        }
        // Java: 38
        public com.oddlabs.tt.render.ShadowListKey getSelectableShadowRenderer()
        {
            return shadow_renderer;
        }
        // Java: 42
        public float getHitOffsetZ(int index)
        {
            return hit_offset_z[index];
        }
        // Java: 46
        public float getNoDetailSize()
        {
            return no_detail_size;
        }
        // Java: 50
        public float getDefenseChance()
        {
            return defense_chance;
        }
        public Template(com.oddlabs.tt.model.Abilities abilities, float shadow_diameter, com.oddlabs.tt.render.ShadowListKey shadow_renderer, float[] hit_offset_z, float no_detail_size, float defense_chance, string name) : base()
        {
            this.abilities = abilities;
            this.shadow_renderer = shadow_renderer;
            this.hit_offset_z = hit_offset_z;
            this.no_detail_size = no_detail_size;
            this.defense_chance = defense_chance;
            this.name = name;
            this.shadow_diameter = shadow_diameter;
        }
    }
}
