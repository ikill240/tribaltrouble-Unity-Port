// Translated from UnitTemplate.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 9
    public class UnitTemplate : com.oddlabs.tt.model.Template
    {
        public float meters_per_second;
        public com.oddlabs.tt.model.weapon.WeaponFactory weapon_factory;
        public com.oddlabs.tt.render.SpriteKey sprite_renderer;
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
        // Java: 60
        public float getSelectionRadius()
        {
            return (fit_radius > 0.0f ? fit_radius : selection_radius);
        }
        // Java: 64
        public float getSelectionHeight()
        {
            return (fit_height > 0.0f ? fit_height : selection_height);
        }
        public float fit_radius;
        public float fit_height;
        // Java: 73
        public void fitSelection(float radius, float height)
        {
            fit_radius = radius;
            fit_height = height;
        }
        // Java: 78
        public bool isSelectionFitted()
        {
            return fit_radius > 0.0f;
        }
        // Java: 82
        public float getMetersPerSecond()
        {
            return meters_per_second;
        }
        // Java: 86
        public com.oddlabs.tt.model.weapon.WeaponFactory getWeaponFactory()
        {
            return weapon_factory;
        }
        // Java: 90
        public com.oddlabs.tt.render.SpriteKey getSpriteRenderer()
        {
            return sprite_renderer;
        }
        // Java: 94
        public com.oddlabs.tt.model.UnitSupplyContainerFactory getUnitSupplyContainerFactory()
        {
            return supply_container_factory;
        }
        // Java: 98
        public com.oddlabs.tt.audio.Audio getDeathSound()
        {
            return death_sound;
        }
        // Java: 102
        public float getDeathPitch()
        {
            return death_pitch;
        }
        // Java: 106
        public int getMaxHitPoints()
        {
            return max_hit_points;
        }
        // Java: 110
        public float getStunX()
        {
            return stun_x;
        }
        // Java: 114
        public float getStunY()
        {
            return stun_y;
        }
        // Java: 118
        public float getStunZ()
        {
            return stun_z;
        }
        // Java: 122
        public int getStatusValue()
        {
            return status_value;
        }
        private void init_UnitTemplate()
        {
            this.fit_radius = (float)(-1.0f);
            this.fit_height = (float)(-1.0f);
        }
        public UnitTemplate(float selection_radius, float selection_height, com.oddlabs.tt.model.Abilities abilities, float meters_per_second, com.oddlabs.tt.model.weapon.WeaponFactory weapon_factory, com.oddlabs.tt.render.SpriteKey sprite_renderer, float shadow_diameter, com.oddlabs.tt.render.ShadowListKey shadow_renderer, com.oddlabs.tt.model.UnitSupplyContainerFactory supply_container_factory, com.oddlabs.tt.audio.Audio death_sound, float death_pitch, float[] hit_offset_z, float no_detail_size, float defense_chance, string name, int max_hit_points, float stun_x, float stun_y, float stun_z, int status_value) : base(abilities, shadow_diameter, shadow_renderer, hit_offset_z, no_detail_size, defense_chance, name)
        {
            init_UnitTemplate();
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
