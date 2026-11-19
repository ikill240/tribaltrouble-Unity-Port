// Translated from WorldInfo.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.resource
{
    // Java: 10
    public class WorldInfo : java.lang.Object
    {
        public int meters_per_world() { return meters_per_world_f; }
        public float sea_level_meters() { return sea_level_meters_f; }
        public int texels_per_colormap() { return texels_per_colormap_f; }
        public int chunks_per_colormap() { return chunks_per_colormap_f; }
        public com.oddlabs.tt.render.Texture[][] colormaps() { return colormaps_f; }
        public com.oddlabs.tt.resource.WorldInfo.Maps maps() { return maps_f; }
        public com.oddlabs.tt.render.Texture detail() { return detail_f; }
        public float[][] heightmap() { return heightmap_f; }
        public java.util.List trees() { return trees_f; }
        public java.util.List palm_trees() { return palm_trees_f; }
        public java.util.List rocks() { return rocks_f; }
        public java.util.List iron() { return iron_f; }
        public float[][] plants() { return plants_f; }
        public bool[][] access_grid() { return access_grid_f; }
        public sbyte[][] dock_grid() { return dock_grid_f; }
        public sbyte[][] water_grid() { return water_grid_f; }
        public sbyte[][] build_grid() { return build_grid_f; }
        public int[][] island_ids() { return island_ids_f; }
        public java.util.List island_infos() { return island_infos_f; }
        public float[][] starting_locations() { return starting_locations_f; }
        public com.oddlabs.tt.resource.BlendInfo[] blend_infos() { return blend_infos_f; }
        public sbyte[][] ground() { return ground_f; }
        public int seed() { return seed_f; }
        public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is WorldInfo r)) return false; return true && java.lang.JO.equals(meters_per_world_f, r.meters_per_world_f) && java.lang.JO.equals(sea_level_meters_f, r.sea_level_meters_f) && java.lang.JO.equals(texels_per_colormap_f, r.texels_per_colormap_f) && java.lang.JO.equals(chunks_per_colormap_f, r.chunks_per_colormap_f) && java.lang.JO.equals(colormaps_f, r.colormaps_f) && java.lang.JO.equals(maps_f, r.maps_f) && java.lang.JO.equals(detail_f, r.detail_f) && java.lang.JO.equals(heightmap_f, r.heightmap_f) && java.lang.JO.equals(trees_f, r.trees_f) && java.lang.JO.equals(palm_trees_f, r.palm_trees_f) && java.lang.JO.equals(rocks_f, r.rocks_f) && java.lang.JO.equals(iron_f, r.iron_f) && java.lang.JO.equals(plants_f, r.plants_f) && java.lang.JO.equals(access_grid_f, r.access_grid_f) && java.lang.JO.equals(dock_grid_f, r.dock_grid_f) && java.lang.JO.equals(water_grid_f, r.water_grid_f) && java.lang.JO.equals(build_grid_f, r.build_grid_f) && java.lang.JO.equals(island_ids_f, r.island_ids_f) && java.lang.JO.equals(island_infos_f, r.island_infos_f) && java.lang.JO.equals(starting_locations_f, r.starting_locations_f) && java.lang.JO.equals(blend_infos_f, r.blend_infos_f) && java.lang.JO.equals(ground_f, r.ground_f) && java.lang.JO.equals(seed_f, r.seed_f); }
        public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(meters_per_world_f); h = 31 * h + java.lang.JO.hashCode(sea_level_meters_f); h = 31 * h + java.lang.JO.hashCode(texels_per_colormap_f); h = 31 * h + java.lang.JO.hashCode(chunks_per_colormap_f); h = 31 * h + java.lang.JO.hashCode(colormaps_f); h = 31 * h + java.lang.JO.hashCode(maps_f); h = 31 * h + java.lang.JO.hashCode(detail_f); h = 31 * h + java.lang.JO.hashCode(heightmap_f); h = 31 * h + java.lang.JO.hashCode(trees_f); h = 31 * h + java.lang.JO.hashCode(palm_trees_f); h = 31 * h + java.lang.JO.hashCode(rocks_f); h = 31 * h + java.lang.JO.hashCode(iron_f); h = 31 * h + java.lang.JO.hashCode(plants_f); h = 31 * h + java.lang.JO.hashCode(access_grid_f); h = 31 * h + java.lang.JO.hashCode(dock_grid_f); h = 31 * h + java.lang.JO.hashCode(water_grid_f); h = 31 * h + java.lang.JO.hashCode(build_grid_f); h = 31 * h + java.lang.JO.hashCode(island_ids_f); h = 31 * h + java.lang.JO.hashCode(island_infos_f); h = 31 * h + java.lang.JO.hashCode(starting_locations_f); h = 31 * h + java.lang.JO.hashCode(blend_infos_f); h = 31 * h + java.lang.JO.hashCode(ground_f); h = 31 * h + java.lang.JO.hashCode(seed_f); } return h; }
        public override string toString() { return "WorldInfo[" + "meters_per_world=" + java.lang.JO.toString(meters_per_world_f) + ", sea_level_meters=" + java.lang.JO.toString(sea_level_meters_f) + ", texels_per_colormap=" + java.lang.JO.toString(texels_per_colormap_f) + ", chunks_per_colormap=" + java.lang.JO.toString(chunks_per_colormap_f) + ", colormaps=" + java.lang.JO.toString(colormaps_f) + ", maps=" + java.lang.JO.toString(maps_f) + ", detail=" + java.lang.JO.toString(detail_f) + ", heightmap=" + java.lang.JO.toString(heightmap_f) + ", trees=" + java.lang.JO.toString(trees_f) + ", palm_trees=" + java.lang.JO.toString(palm_trees_f) + ", rocks=" + java.lang.JO.toString(rocks_f) + ", iron=" + java.lang.JO.toString(iron_f) + ", plants=" + java.lang.JO.toString(plants_f) + ", access_grid=" + java.lang.JO.toString(access_grid_f) + ", dock_grid=" + java.lang.JO.toString(dock_grid_f) + ", water_grid=" + java.lang.JO.toString(water_grid_f) + ", build_grid=" + java.lang.JO.toString(build_grid_f) + ", island_ids=" + java.lang.JO.toString(island_ids_f) + ", island_infos=" + java.lang.JO.toString(island_infos_f) + ", starting_locations=" + java.lang.JO.toString(starting_locations_f) + ", blend_infos=" + java.lang.JO.toString(blend_infos_f) + ", ground=" + java.lang.JO.toString(ground_f) + ", seed=" + java.lang.JO.toString(seed_f) + "]"; }
        public int meters_per_world_f;
        public float sea_level_meters_f;
        public int texels_per_colormap_f;
        public int chunks_per_colormap_f;
        public com.oddlabs.tt.render.Texture[][] colormaps_f;
        public com.oddlabs.tt.resource.WorldInfo.Maps maps_f;
        public com.oddlabs.tt.render.Texture detail_f;
        public float[][] heightmap_f;
        public java.util.List trees_f;
        public java.util.List palm_trees_f;
        public java.util.List rocks_f;
        public java.util.List iron_f;
        public float[][] plants_f;
        public bool[][] access_grid_f;
        public sbyte[][] dock_grid_f;
        public sbyte[][] water_grid_f;
        public sbyte[][] build_grid_f;
        public int[][] island_ids_f;
        public java.util.List island_infos_f;
        public float[][] starting_locations_f;
        public com.oddlabs.tt.resource.BlendInfo[] blend_infos_f;
        public sbyte[][] ground_f;
        public int seed_f;
        // Java: 23
        public class Maps : java.lang.Object
        {
            public com.oddlabs.tt.render.Texture diffuse() { return diffuse_f; }
            public com.oddlabs.tt.render.Texture normal() { return normal_f; }
            public com.oddlabs.tt.resource.StructureBlend terrainShadows() { return terrainShadows_f; }
            public com.oddlabs.tt.resource.StructureBlend supplyShadows() { return supplyShadows_f; }
            public Maps(com.oddlabs.tt.render.Texture diffuse, com.oddlabs.tt.render.Texture normal, com.oddlabs.tt.resource.StructureBlend terrainShadows, com.oddlabs.tt.resource.StructureBlend supplyShadows)
            {
                this.diffuse_f = diffuse;
                this.normal_f = normal;
                this.terrainShadows_f = terrainShadows;
                this.supplyShadows_f = supplyShadows;
            }
            public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is Maps r)) return false; return true && java.lang.JO.equals(diffuse_f, r.diffuse_f) && java.lang.JO.equals(normal_f, r.normal_f) && java.lang.JO.equals(terrainShadows_f, r.terrainShadows_f) && java.lang.JO.equals(supplyShadows_f, r.supplyShadows_f); }
            public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(diffuse_f); h = 31 * h + java.lang.JO.hashCode(normal_f); h = 31 * h + java.lang.JO.hashCode(terrainShadows_f); h = 31 * h + java.lang.JO.hashCode(supplyShadows_f); } return h; }
            public override string toString() { return "Maps[" + "diffuse=" + java.lang.JO.toString(diffuse_f) + ", normal=" + java.lang.JO.toString(normal_f) + ", terrainShadows=" + java.lang.JO.toString(terrainShadows_f) + ", supplyShadows=" + java.lang.JO.toString(supplyShadows_f) + "]"; }
            public com.oddlabs.tt.render.Texture diffuse_f;
            public com.oddlabs.tt.render.Texture normal_f;
            public com.oddlabs.tt.resource.StructureBlend terrainShadows_f;
            public com.oddlabs.tt.resource.StructureBlend supplyShadows_f;
            // Java: 29
            public com.oddlabs.tt.resource.WorldInfo.Maps withShadows(com.oddlabs.tt.resource.StructureBlend terrainShadows_v, com.oddlabs.tt.resource.StructureBlend supplyShadows_v)
            {
                return new com.oddlabs.tt.resource.WorldInfo.Maps(diffuse_f, normal_f, terrainShadows_v, supplyShadows_v);
            }
            public Maps(com.oddlabs.tt.render.Texture diffuse_v, com.oddlabs.tt.render.Texture normal_v) : this(diffuse_v, normal_v, null, null)
            {
            }
        }
        public WorldInfo(int meters_per_world_v, float sea_level_meters_v, int texels_per_colormap_v, int chunks_per_colormap_v, com.oddlabs.tt.render.Texture[][] colormaps_v, com.oddlabs.tt.resource.WorldInfo.Maps maps_v, com.oddlabs.tt.render.Texture detail_v, float[][] heightmap_v, java.util.List trees_v, java.util.List palm_trees_v, java.util.List rocks_v, java.util.List iron_v, float[][] plants_v, bool[][] access_grid_v, sbyte[][] dock_grid_v, sbyte[][] water_grid_v, sbyte[][] build_grid_v, int[][] island_ids_v, java.util.List island_infos_v, float[][] starting_locations_v, com.oddlabs.tt.resource.BlendInfo[] blend_infos_v, sbyte[][] ground_v, int seed_v) : base()
        {
            this.texels_per_colormap_f = texels_per_colormap_v;
            this.chunks_per_colormap_f = chunks_per_colormap_v;
            this.sea_level_meters_f = sea_level_meters_v;
            this.meters_per_world_f = meters_per_world_v;
            this.colormaps_f = colormaps_v;
            this.maps_f = maps_v;
            this.detail_f = detail_v;
            this.heightmap_f = heightmap_v;
            this.trees_f = trees_v;
            this.rocks_f = rocks_v;
            this.iron_f = iron_v;
            this.plants_f = plants_v;
            this.palm_trees_f = palm_trees_v;
            this.access_grid_f = access_grid_v;
            this.dock_grid_f = dock_grid_v;
            this.water_grid_f = water_grid_v;
            this.island_ids_f = island_ids_v;
            this.island_infos_f = island_infos_v;
            this.build_grid_f = build_grid_v;
            this.starting_locations_f = starting_locations_v;
            this.blend_infos_f = blend_infos_v;
            this.ground_f = ground_v;
            this.seed_f = seed_v;
        }
    }
}
