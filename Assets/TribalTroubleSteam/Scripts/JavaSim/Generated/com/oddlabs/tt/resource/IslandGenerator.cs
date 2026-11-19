// Translated from IslandGenerator.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.resource
{
    // Java: 16
    public class IslandGenerator : java.lang.Object, com.oddlabs.tt.resource.WorldGenerator
    {
        public const long serialVersionUID = 1L;
        public const int TEXELS_PER_CHUNK = 512;
        public const int IDEAL_TEXELS_PER_DETAIL = 256;
        public const float IDEAL_DETAIL_ALPHA = 0.15f;
        public int meters_per_world;
        public com.oddlabs.tt.procedural.Landscape.TerrainType terrain;
        public int grid_units;
        public float hills;
        public float vegetation_amount;
        public float supplies_amount;
        public int seed;
        public bool archipelago;
        // Java: 46
        public static com.oddlabs.tt.render.Texture createDetail(com.oddlabs.tt.resource.GLImage detail_image, int base_level)
        {
            com.oddlabs.tt.resource.GLImage[] detail_mipmaps = detail_image.buildMipMaps(base_level, com.oddlabs.tt.global.Globals.LANDSCAPE_DETAIL_FADEOUT_FACTOR, true, false);
            return new com.oddlabs.tt.render.Texture(detail_mipmaps, org.lwjgl.opengl.GL11.GL_RGBA8, org.lwjgl.opengl.GL11.GL_LINEAR_MIPMAP_LINEAR, org.lwjgl.opengl.GL11.GL_LINEAR, org.lwjgl.opengl.GL11.GL_REPEAT, org.lwjgl.opengl.GL11.GL_REPEAT);
        }
        // Java: 53
        public static int getTexelsPerGridUnit()
        {
            int texels_per_grid_unit = com.oddlabs.tt.global.Globals.TEXELS_PER_GRID_UNIT / java.lang.JC.d2i(java.lang.StrictMath.pow(2, com.oddlabs.tt.global.Globals.TEXTURE_MIP_SHIFT[com.oddlabs.tt.global.Settings.getSettings().graphic_detail]));
            return texels_per_grid_unit;
        }
        // Java: 59
        public com.oddlabs.tt.procedural.Landscape.TerrainType getTerrainType()
        {
            return terrain;
        }
        // Java: 64
        public int getMetersPerWorld()
        {
            return meters_per_world;
        }
        // Java: 69
        public com.oddlabs.tt.resource.FogInfo getFogInfo()
        {
            return com.oddlabs.tt.procedural.Landscape.getFogInfo(terrain, meters_per_world);
        }
        // Java: 74
        public com.oddlabs.tt.resource.WorldInfo generate(int num_players, int initial_unit_count, float random_start_pos)
        {
            int colormap_size = grid_units * com.oddlabs.tt.resource.IslandGenerator.getTexelsPerGridUnit();
            int chunks_per_colormap = colormap_size / com.oddlabs.tt.resource.IslandGenerator.TEXELS_PER_CHUNK;
            java.time.Instant time_before = java.time.Instant.now();
            int base_level = com.oddlabs.tt.global.Globals.LANDSCAPE_DETAIL_FADEOUT_BASE_LEVEL;
            int detail_mip_level = com.oddlabs.tt.resource.IslandGenerator.IDEAL_TEXELS_PER_DETAIL / com.oddlabs.tt.global.Globals.DETAIL_SIZE - 1;
            int detail_prefade_level = java.lang.Math.max(detail_mip_level - base_level, 0);
            float detail_prefade = (float)(com.oddlabs.tt.resource.IslandGenerator.IDEAL_DETAIL_ALPHA * ((float)(java.lang.StrictMath.pow(com.oddlabs.tt.global.Globals.LANDSCAPE_DETAIL_FADEOUT_FACTOR, detail_prefade_level))));
            base_level -= detail_mip_level;
            base_level = java.lang.Math.min(base_level, 1);
            com.oddlabs.tt.procedural.Landscape landscape = new com.oddlabs.tt.procedural.Landscape(num_players, meters_per_world, terrain, detail_prefade, hills, vegetation_amount, supplies_amount, seed, initial_unit_count, random_start_pos, archipelago);
            java.time.Instant time_after = java.time.Instant.now();
            java.lang.IO.println("Landscape created in " + java.lang.JStr.valueOf(java.time.Duration.between(time_before, time_after)));
            com.oddlabs.tt.resource.BlendInfo[] blend_infos = landscape.getBlendInfos();
            com.oddlabs.tt.render.Texture detail = com.oddlabs.tt.resource.IslandGenerator.createDetail(landscape.getDetail(), base_level);
            com.oddlabs.tt.landscape.LandscapeBaker baker = new com.oddlabs.tt.landscape.LandscapeBaker();
            float textureScale = (float)(((float)(colormap_size)) / com.oddlabs.tt.global.Globals.STRUCTURE_SIZE);
            com.oddlabs.tt.resource.WorldInfo.Maps maps = baker.bake(colormap_size, textureScale, blend_infos).withShadows(landscape.getTerrainShadows(), landscape.getSupplyShadows());
            com.oddlabs.tt.form.ProgressForm.progress();
            return new com.oddlabs.tt.resource.WorldInfo(meters_per_world, landscape.getSeaLevelMeters(), colormap_size, chunks_per_colormap, null, maps, detail, landscape.getHeight(), landscape.getTrees(), landscape.getPalmtrees(), landscape.getRock(), landscape.getIron(), landscape.getPlants(), landscape.getAccessGrid(), landscape.getDockGrid(), landscape.getWaterGrid(), landscape.getBuildGrid(), landscape.getIslandIds(), landscape.getIslandInfos(), landscape.getStartingLocations(), blend_infos, landscape.getGround(), seed);
        }
        public IslandGenerator(int meters_per_world, com.oddlabs.tt.procedural.Landscape.TerrainType terrain, float hills, float vegetation_amount, float supplies_amount, int seed, bool archipelago) : base()
        {
            this.hills = hills;
            this.vegetation_amount = vegetation_amount;
            this.supplies_amount = supplies_amount;
            this.seed = seed;
            this.archipelago = archipelago;
            this.grid_units = meters_per_world / com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID;
            this.meters_per_world = meters_per_world;
            this.terrain = terrain;
        }
    }
}
