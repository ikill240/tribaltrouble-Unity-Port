// Translated from UnitTemplate.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 14
    public class UnitTemplate : com.oddlabs.tt.model.Template
    {
        public float meters_per_second;
        public com.oddlabs.tt.model.weapon.WeaponFactory weapon_factory;
        public com.oddlabs.tt.render.SpriteKey sprite_renderer;
        public java.util.Map attachments;
        public java.util.Set default_attachments;
        public com.oddlabs.tt.model.UnitSupplyContainerFactory supply_container_factory;
        public com.oddlabs.tt.audio.Audio death_sound;
        public float death_pitch;
        public float selection_radius;
        public float selection_height;
        public int max_hit_points;
        public float stun_x;
        public float stun_y;
        public float stun_z;
        public int status_value;
        // Java: 71
        public float getSelectionRadius()
        {
            return selection_radius;
        }
        // Java: 75
        public float getSelectionHeight()
        {
            return selection_height;
        }
        // Java: 79
        public float getMetersPerSecond()
        {
            return meters_per_second;
        }
        // Java: 83
        public com.oddlabs.tt.model.weapon.WeaponFactory getWeaponFactory()
        {
            return weapon_factory;
        }
        // Java: 87
        public com.oddlabs.tt.render.SpriteKey getSpriteRenderer()
        {
            return sprite_renderer;
        }
        // Java: 94
        public java.util.Map getAttachments()
        {
            return attachments;
        }
        // Java: 101
        public java.util.Set getDefaultAttachments()
        {
            return default_attachments;
        }
        // Java: 106
        public java.util.List getStartingAttachments()
        {
            java.util.List keys = new java.util.ArrayList();
            foreach (string slot in java.lang.JIter.of(default_attachments))
            {
                ((java.util.Map)attachments.getOrDefault(slot, java.util.Map_.of())).values().stream().findFirst().ifPresent(new java.util.function.Consumer_L((object a0) => { keys.add(a0); }));
            }
            return java.util.List_.copyOf(keys);
        }
        // Java: 114
        public com.oddlabs.tt.model.UnitSupplyContainerFactory getUnitSupplyContainerFactory()
        {
            return supply_container_factory;
        }
        // Java: 118
        public com.oddlabs.tt.audio.Audio getDeathSound()
        {
            return death_sound;
        }
        // Java: 122
        public float getDeathPitch()
        {
            return death_pitch;
        }
        // Java: 126
        public int getMaxHitPoints()
        {
            return max_hit_points;
        }
        // Java: 130
        public float getStunX()
        {
            return stun_x;
        }
        // Java: 134
        public float getStunY()
        {
            return stun_y;
        }
        // Java: 138
        public float getStunZ()
        {
            return stun_z;
        }
        // Java: 142
        public int getStatusValue()
        {
            return status_value;
        }
        public UnitTemplate(float selection_radius, float selection_height, com.oddlabs.tt.model.Abilities abilities, float meters_per_second, com.oddlabs.tt.model.weapon.WeaponFactory weapon_factory, com.oddlabs.tt.render.SpriteKey sprite_renderer, float shadow_diameter, com.oddlabs.tt.render.ShadowListKey shadow_renderer, com.oddlabs.tt.model.UnitSupplyContainerFactory supply_container_factory, com.oddlabs.tt.audio.Audio death_sound, float death_pitch, float[] hit_offset_z, float no_detail_size, float defense_chance, string name, int max_hit_points, float stun_x, float stun_y, float stun_z, int status_value, java.util.Map attachments, java.util.Set default_attachments) : base(abilities, shadow_diameter, shadow_renderer, hit_offset_z, no_detail_size, defense_chance, name)
        {
            this.attachments = attachments;
            this.default_attachments = default_attachments;
            this.selection_radius = selection_radius;
            this.selection_height = selection_height;
            this.meters_per_second = meters_per_second;
            this.weapon_factory = weapon_factory;
            this.sprite_renderer = sprite_renderer;
            this.supply_container_factory = supply_container_factory;
            this.death_sound = death_sound;
            this.death_pitch = death_pitch;
            this.max_hit_points = max_hit_points;
            this.stun_x = stun_x;
            this.stun_y = stun_y;
            this.stun_z = stun_z;
            this.status_value = status_value;
        }
    }
}
