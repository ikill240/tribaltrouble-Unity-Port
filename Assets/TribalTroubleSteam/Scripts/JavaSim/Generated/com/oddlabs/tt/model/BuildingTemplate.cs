// Translated from BuildingTemplate.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 12
    public class BuildingTemplate : com.oddlabs.tt.model.Template
    {
        public const int TYPE_BUILDING = 0;
        public const int TYPE_SHIP = 1;
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
        public java.util.Map props;
        // Java: 100
        public int getTemplateID()
        {
            return template_id;
        }
        // Java: 104
        public int getType()
        {
            return type;
        }
        // Java: 108
        public com.oddlabs.tt.model.Building create(com.oddlabs.tt.player.Player owner, int grid_x, int grid_y)
        {
            if (type == com.oddlabs.tt.model.BuildingTemplate.TYPE_SHIP)
            {
                return new com.oddlabs.tt.model.Ship(owner, this, grid_x, grid_y);
            }
            return new com.oddlabs.tt.model.LandBuilding(owner, this, grid_x, grid_y);
        }
        // Java: 115
        public bool isPlacingLegal(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, int grid_x, int grid_y)
        {
            if (type == com.oddlabs.tt.model.BuildingTemplate.TYPE_SHIP)
            {
                return com.oddlabs.tt.model.Ship.isPlacingLegal(unit_grid, this, grid_x, grid_y);
            }
            return com.oddlabs.tt.model.LandBuilding.isPlacingLegal(unit_grid, this, grid_x, grid_y);
        }
        // Java: 122
        public float getBuiltSelectionRadius()
        {
            return built_selection_radius;
        }
        // Java: 126
        public float getBuiltSelectionHeight()
        {
            return built_selection_height;
        }
        // Java: 130
        public float getHalfbuiltSelectionRadius()
        {
            return halfbuilt_selection_radius;
        }
        // Java: 134
        public float getHalfbuiltSelectionHeight()
        {
            return halfbuilt_selection_height;
        }
        // Java: 138
        public float getStartSelectionRadius()
        {
            return start_selection_radius;
        }
        // Java: 142
        public float getStartSelectionHeight()
        {
            return start_selection_height;
        }
        // Java: 146
        public int getPlacingSize()
        {
            return placing_size;
        }
        // Java: 150
        public float getSmokeRadius()
        {
            return smoke_radius;
        }
        // Java: 154
        public float getSmokeHeight()
        {
            return smoke_height;
        }
        // Java: 158
        public int getNumFragments()
        {
            return num_fragments;
        }
        // Java: 162
        public com.oddlabs.tt.render.SpriteKey getBuiltRenderer()
        {
            return built_renderer;
        }
        // Java: 166
        public com.oddlabs.tt.render.SpriteKey getStartRenderer()
        {
            return start_renderer;
        }
        // Java: 170
        public com.oddlabs.tt.render.SpriteKey getHalfbuiltRenderer()
        {
            return halfbuilt_renderer;
        }
        // Java: 174
        public com.oddlabs.tt.render.SpriteKey getRenderer(com.oddlabs.tt.model.Building.BuildState stage)
        {
            return ((stage).ordinal() switch { 0 /*START*/ => (com.oddlabs.tt.render.SpriteKey)(start_renderer), 1 /*HALFBUILT*/ => (com.oddlabs.tt.render.SpriteKey)(halfbuilt_renderer), 2 /*BUILT*/ => (com.oddlabs.tt.render.SpriteKey)(built_renderer), _ => throw new System.InvalidOperationException("MatchException") });
        }
        // Java: 182
        public int getMaxHitPoints()
        {
            return max_hit_points;
        }
        // Java: 186
        public com.oddlabs.tt.model.UnitContainerFactory getUnitContainerFactory()
        {
            return unit_container_factory;
        }
        // Java: 190
        public float getMountOffset()
        {
            return mount_offset;
        }
        // Java: 194
        public float getRallyX()
        {
            return rally_x;
        }
        // Java: 198
        public float getRallyY()
        {
            return rally_y;
        }
        // Java: 202
        public float getRallyZ()
        {
            return rally_z;
        }
        // Java: 206
        public float getChimneyX()
        {
            return chimney_x;
        }
        // Java: 210
        public float getChimneyY()
        {
            return chimney_y;
        }
        // Java: 214
        public float getChimneyZ()
        {
            return chimney_z;
        }
        // Java: 218
        public bool isVikings()
        {
            return vikings;
        }
        // Java: 222
        public java.util.List getProps(com.oddlabs.tt.model.Building.BuildState stage)
        {
            return ((java.util.List)props.getOrDefault(stage, java.util.List_.of()));
        }
        public BuildingTemplate(int template_id, int type, int placing_size, float smoke_radius, float smoke_height, int num_fragments, float shadow_diameter, com.oddlabs.tt.render.ShadowListKey shadow_renderer, com.oddlabs.tt.render.SpriteKey built_renderer, float built_selection_radius, float built_selection_height, com.oddlabs.tt.render.SpriteKey halfbuilt_renderer, float halfbuilt_selection_radius, float halfbuilt_selection_height, com.oddlabs.tt.render.SpriteKey start_renderer, float start_selection_radius, float start_selection_height, int max_hit_points, com.oddlabs.tt.model.UnitContainerFactory unit_container_factory, com.oddlabs.tt.model.Abilities abilities, float[] hit_offset_z, float mount_offset, float no_detail_size, float defense_chance, float rally_x, float rally_y, float rally_z, float chimney_x, float chimney_y, float chimney_z, bool vikings, string name, java.util.Map props) : base(abilities, shadow_diameter, shadow_renderer, hit_offset_z, no_detail_size, defense_chance, name)
        {
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
            this.props = props;
        }
    }
}
