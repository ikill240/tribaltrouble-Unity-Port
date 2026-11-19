// Translated from BuildingTemplate.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 9
    public class BuildingTemplate : com.oddlabs.tt.model.Template
    {
        public const int TYPE_BUILDING = 0;
        public const int TYPE_SHIP = 1;
        public const int TYPE_BRIDGE = 2;
        public int template_id;
        public int type;
        public int placing_size;
        public float smoke_radius;
        public float smoke_height;
        public int num_fragments;
        public com.oddlabs.tt.render.SpriteKey built_renderer;
        public com.oddlabs.tt.render.SpriteKey halfbuilt_renderer;
        public com.oddlabs.tt.render.SpriteKey start_renderer;
        public int max_hit_points;
        public com.oddlabs.tt.model.UnitContainerFactory unit_container_factory;
        public float mount_offset;
        public float built_selection_radius;
        public float built_selection_height;
        public float halfbuilt_selection_radius;
        public float halfbuilt_selection_height;
        public float start_selection_radius;
        public float start_selection_height;
        public float rally_x;
        public float rally_y;
        public float rally_z;
        public float chimney_x;
        public float chimney_y;
        public float chimney_z;
        public bool vikings;
        // Java: 95
        public int getTemplateID()
        {
            return template_id;
        }
        // Java: 99
        public int getType()
        {
            return type;
        }
        // Java: 103
        public com.oddlabs.tt.model.Building create(com.oddlabs.tt.player.Player owner, int grid_x, int grid_y)
        {
            if (type == com.oddlabs.tt.model.BuildingTemplate.TYPE_SHIP)
            {
                return new com.oddlabs.tt.model.Ship(owner, this, grid_x, grid_y);
            }
            return new com.oddlabs.tt.model.LandBuilding(owner, this, grid_x, grid_y);
        }
        // Java: 110
        public bool isPlacingLegal(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, int grid_x, int grid_y)
        {
            if (type == com.oddlabs.tt.model.BuildingTemplate.TYPE_SHIP)
            {
                return com.oddlabs.tt.model.Ship.isPlacingLegal(unit_grid, this, grid_x, grid_y);
            }
            if (type == com.oddlabs.tt.model.BuildingTemplate.TYPE_BRIDGE)
            {
                return com.oddlabs.tt.model.BridgeSpan.isPlacingLegal(unit_grid, grid_x, grid_y);
            }
            return com.oddlabs.tt.model.LandBuilding.isPlacingLegal(unit_grid, this, grid_x, grid_y);
        }
        public float[] fit_radius;
        public float[] fit_height;
        // Java: 124
        public void fitSelection(int level, float radius, float height)
        {
            fit_radius[level] = radius;
            fit_height[level] = height;
        }
        public bool compact_footprint;
        // Java: 132
        public bool hasCompactFootprint()
        {
            return compact_footprint;
        }
        // Java: 136
        public void setCompactFootprint(bool compact)
        {
            compact_footprint = compact;
        }
        // Java: 140
        public bool isSelectionFitted(int level)
        {
            return fit_radius[level] > 0.0f;
        }
        // Java: 144
        public void clearFit()
        {
            java.util.Arrays.fill(fit_radius, (float)(-1.0f));
            java.util.Arrays.fill(fit_height, (float)(-1.0f));
        }
        // Java: 149
        public float getBuiltSelectionRadius()
        {
            return (fit_radius[2] > 0.0f ? fit_radius[2] : built_selection_radius);
        }
        // Java: 153
        public float getBuiltSelectionHeight()
        {
            return (fit_height[2] > 0.0f ? fit_height[2] : built_selection_height);
        }
        // Java: 157
        public float getHalfbuiltSelectionRadius()
        {
            return (fit_radius[1] > 0.0f ? fit_radius[1] : halfbuilt_selection_radius);
        }
        // Java: 161
        public float getHalfbuiltSelectionHeight()
        {
            return (fit_height[1] > 0.0f ? fit_height[1] : halfbuilt_selection_height);
        }
        // Java: 165
        public float getStartSelectionRadius()
        {
            return (fit_radius[0] > 0.0f ? fit_radius[0] : start_selection_radius);
        }
        // Java: 169
        public float getStartSelectionHeight()
        {
            return (fit_height[0] > 0.0f ? fit_height[0] : start_selection_height);
        }
        // Java: 173
        public int getPlacingSize()
        {
            return placing_size;
        }
        // Java: 177
        public float getSmokeRadius()
        {
            return smoke_radius;
        }
        // Java: 181
        public float getSmokeHeight()
        {
            return smoke_height;
        }
        // Java: 185
        public int getNumFragments()
        {
            return num_fragments;
        }
        // Java: 189
        public com.oddlabs.tt.render.SpriteKey getBuiltRenderer()
        {
            return built_renderer;
        }
        // Java: 193
        public com.oddlabs.tt.render.SpriteKey getStartRenderer()
        {
            return start_renderer;
        }
        // Java: 197
        public com.oddlabs.tt.render.SpriteKey getHalfbuiltRenderer()
        {
            return halfbuilt_renderer;
        }
        // Java: 201
        public int getMaxHitPoints()
        {
            return max_hit_points;
        }
        // Java: 205
        public com.oddlabs.tt.model.UnitContainerFactory getUnitContainerFactory()
        {
            return unit_container_factory;
        }
        // Java: 209
        public float getMountOffset()
        {
            return mount_offset;
        }
        // Java: 213
        public float getRallyX()
        {
            return rally_x;
        }
        // Java: 217
        public float getRallyY()
        {
            return rally_y;
        }
        // Java: 221
        public float getRallyZ()
        {
            return rally_z;
        }
        // Java: 225
        public float getChimneyX()
        {
            return chimney_x;
        }
        // Java: 229
        public float getChimneyY()
        {
            return chimney_y;
        }
        // Java: 233
        public float getChimneyZ()
        {
            return chimney_z;
        }
        // Java: 237
        public bool isVikings()
        {
            return vikings;
        }
        private void init_BuildingTemplate()
        {
            this.fit_radius = new float[] { (float)(-1.0f), (float)(-1.0f), (float)(-1.0f) };
            this.fit_height = new float[] { (float)(-1.0f), (float)(-1.0f), (float)(-1.0f) };
        }
        public BuildingTemplate(int template_id, int type, int placing_size, float smoke_radius, float smoke_height, int num_fragments, float shadow_diameter, com.oddlabs.tt.render.ShadowListKey shadow_renderer, com.oddlabs.tt.render.SpriteKey built_renderer, float built_selection_radius, float built_selection_height, com.oddlabs.tt.render.SpriteKey halfbuilt_renderer, float halfbuilt_selection_radius, float halfbuilt_selection_height, com.oddlabs.tt.render.SpriteKey start_renderer, float start_selection_radius, float start_selection_height, int max_hit_points, com.oddlabs.tt.model.UnitContainerFactory unit_container_factory, com.oddlabs.tt.model.Abilities abilities, float[] hit_offset_z, float mount_offset, float no_detail_size, float defense_chance, float rally_x, float rally_y, float rally_z, float chimney_x, float chimney_y, float chimney_z, bool vikings, string name) : base(abilities, shadow_diameter, shadow_renderer, hit_offset_z, no_detail_size, defense_chance, name)
        {
            init_BuildingTemplate();
            this.template_id = template_id;
            this.type = type;
            this.built_selection_radius = built_selection_radius;
            this.built_selection_height = built_selection_height;
            this.halfbuilt_selection_radius = halfbuilt_selection_radius;
            this.halfbuilt_selection_height = halfbuilt_selection_height;
            this.start_selection_radius = start_selection_radius;
            this.start_selection_height = start_selection_height;
            this.placing_size = placing_size;
            this.smoke_radius = smoke_radius;
            this.smoke_height = smoke_height;
            this.num_fragments = num_fragments;
            this.built_renderer = built_renderer;
            this.halfbuilt_renderer = halfbuilt_renderer;
            this.start_renderer = start_renderer;
            this.max_hit_points = max_hit_points;
            this.unit_container_factory = unit_container_factory;
            this.mount_offset = mount_offset;
            this.rally_x = rally_x;
            this.rally_y = rally_y;
            this.rally_z = rally_z;
            this.chimney_x = chimney_x;
            this.chimney_y = chimney_y;
            this.chimney_z = chimney_z;
            this.vikings = vikings;
        }
    }
}
