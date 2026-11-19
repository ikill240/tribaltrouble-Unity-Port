// Translated from HeightMap.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.landscape
{
    // Java: 13
    public class HeightMap : java.lang.Object
    {
        public const int METERS_PER_UNIT_GRID = 2;
        public const int GRID_UNITS_PER_PATCH_EXP = 4;
        public const int GRID_UNITS_PER_PATCH = 16;
        public const int MIN_INTERSECTING_LEVEL = 5;
        public static org.joml.Vector3f plane;
        public float[][] world;
        public com.oddlabs.tt.landscape.LandscapeLeaf[][] landscape_leaves;
        public java.util.List trees;
        public bool[][] access_grid;
        public sbyte[][] dock_grid;
        public sbyte[][] water_grid;
        public int[][] island_ids;
        public java.util.Map island_info;
        public sbyte[][] build_grid;
        public int meters_per_world;
        public int patches_per_world;
        public int meters_per_patch;
        public int grid_units_per_world;
        public float inv_meters_per_patch;
        public float inv_meters_per_grid_unit;
        public float sea_level_meters;
        public int meters_per_chunk;
        public int quadtree_min_level;
        public int patches_per_chunk;
        public float meters_per_chunk_border;
        public float chunk_tex_scale;
        public com.oddlabs.tt.landscape.World world_instance;
        public com.oddlabs.tt.render.Texture heightTexture;
        // Java: 110
        public com.oddlabs.tt.render.Texture getHeightTexture()
        {
            return heightTexture;
        }
        // Java: 114
        public com.oddlabs.tt.landscape.World getWorld()
        {
            return world_instance;
        }
        // Java: 118
        public bool isGridInside(int x, int y)
        {
            bool inside_world = x >= 0 && y >= 0 && x < getGridUnitsPerWorld() && y < getGridUnitsPerWorld();
            return inside_world;
        }
        // Java: 123
        public bool isInside(float x, float y)
        {
            bool inside_world = x >= 0 && y >= 0 && x <= getMetersPerWorld() && y <= getMetersPerWorld();
            return inside_world;
        }
        // Java: 128
        public int getMetersPerWorld()
        {
            return meters_per_world;
        }
        // Java: 132
        public int getGridUnitsPerPatch()
        {
            return com.oddlabs.tt.landscape.HeightMap.GRID_UNITS_PER_PATCH;
        }
        // Java: 136
        public float getInvMetersPerGridUnit()
        {
            return inv_meters_per_grid_unit;
        }
        // Java: 140
        public int getPatchesPerWorld()
        {
            return patches_per_world;
        }
        // Java: 144
        public int getMetersPerPatch()
        {
            return meters_per_patch;
        }
        // Java: 148
        public int getGridUnitsPerWorld()
        {
            return grid_units_per_world;
        }
        // Java: 152
        public float getSeaLevelMeters()
        {
            return sea_level_meters;
        }
        // Java: 156
        public int getMetersPerChunk()
        {
            return meters_per_chunk;
        }
        // Java: 160
        public int getQuadtreeMinLevel()
        {
            return quadtree_min_level;
        }
        // Java: 164
        public int getPatchesPerChunk()
        {
            return patches_per_chunk;
        }
        // Java: 168
        public float getMetersPerChunkBorder()
        {
            return meters_per_chunk_border;
        }
        // Java: 172
        public float getChunkTexScale()
        {
            return chunk_tex_scale;
        }
        // Java: 176
        public void registerLeaf(int x, int y, com.oddlabs.tt.landscape.LandscapeLeaf leaf)
        {
            landscape_leaves[y][x] = leaf;
        }
        // Java: 180
        public java.util.List getTrees()
        {
            return trees;
        }
        // Java: 184
        public bool[][] getAccessGrid()
        {
            return access_grid;
        }
        // Java: 188
        public sbyte[][] getDockGrid()
        {
            return dock_grid;
        }
        // Java: 192
        public sbyte[][] getWaterGrid()
        {
            return water_grid;
        }
        // Java: 196
        public int[][] getIslandIdGrid()
        {
            return island_ids;
        }
        // Java: 200
        public int getIslandId(int grid_x, int grid_y)
        {
            return island_ids[wrapGridCoord(grid_y)][wrapGridCoord(grid_x)];
        }
        // Java: 204
        public java.util.List getIslandIds()
        {
            return new java.util.ArrayList(island_info.keySet());
        }
        // Java: 208
        public com.oddlabs.tt.landscape.IslandInfo getIslandInfo(int island_id)
        {
            return ((com.oddlabs.tt.landscape.IslandInfo)island_info.get(island_id));
        }
        // Java: 212
        public java.util.List getIslandInfos()
        {
            return new java.util.ArrayList(island_info.values());
        }
        // Java: 216
        public int getIslandTreeCount(int island_id)
        {
            com.oddlabs.tt.landscape.IslandInfo info = ((com.oddlabs.tt.landscape.IslandInfo)island_info.get(island_id));
            return (info != null ? info.trees() : 0);
        }
        // Java: 221
        public int getIslandRockCount(int island_id)
        {
            com.oddlabs.tt.landscape.IslandInfo info = ((com.oddlabs.tt.landscape.IslandInfo)island_info.get(island_id));
            return (info != null ? info.rocks() : 0);
        }
        // Java: 226
        public int getIslandIronCount(int island_id)
        {
            com.oddlabs.tt.landscape.IslandInfo info = ((com.oddlabs.tt.landscape.IslandInfo)island_info.get(island_id));
            return (info != null ? info.iron() : 0);
        }
        // Java: 231
        public void makePlaneVector(int x0, int y0, int x1, int y1, int x2, int y2, org.joml.Vector3f plane)
        {
            com.oddlabs.tt.landscape.HeightMap.makePlaneVector(x0, y0, getWrappedHeight(x0, y0), x1, y1, getWrappedHeight(x1, y1), x2, y2, getWrappedHeight(x2, y2), plane);
        }
        // Java: 237
        public static void makePlaneVector(float h1x, float h1y, float h1z, float h2x, float h2y, float h2z, float h3x, float h3y, float h3z, org.joml.Vector3f plane)
        {
            float v1x = (float)(h2x - h1x);
            float v1y = (float)(h2y - h1y);
            float v1z = (float)(h2z - h1z);
            float v2x = (float)(h3x - h1x);
            float v2y = (float)(h3y - h1y);
            float v2z = (float)(h3z - h1z);
            org.joml.Vector3f vec1 = new org.joml.Vector3f(v1x, v1y, v1z);
            org.joml.Vector3f vec2 = new org.joml.Vector3f(v2x, v2y, v2z);
            vec2.cross(vec1);
            float inv_z = (float)((float)(-1.0f) / vec2.z_f);
            plane.set((float)(vec2.x_f * inv_z), (float)(vec2.y_f * inv_z), (float)((float)(((float)((float)((float)(-vec2.x_f) * h1x) - (float)(vec2.y_f * h1y))) * inv_z) + h1z));
        }
        // Java: 255
        public static float planeHeight(float x, float y, org.joml.Vector3f plane)
        {
            return (float)((float)((float)(plane.x_f * x) + (float)(plane.y_f * y)) + plane.z_f);
        }
        // Java: 259
        public static float doPlane(float x, float y, float h1x, float h1y, float h1z, float h2x, float h2y, float h2z, float h3x, float h3y, float h3z)
        {
            com.oddlabs.tt.landscape.HeightMap.makePlaneVector(h1x, h1y, h1z, h2x, h2y, h2z, h3x, h3y, h3z, com.oddlabs.tt.landscape.HeightMap.plane);
            return com.oddlabs.tt.landscape.HeightMap.planeHeight(x, y, com.oddlabs.tt.landscape.HeightMap.plane);
        }
        // Java: 265
        public bool isBelowSeaLevel(int patch_x, int patch_y)
        {
            int offset_x = patch_x * getGridUnitsPerPatch();
            int offset_y = patch_y * getGridUnitsPerPatch();
            for (int y = 0; y < getGridUnitsPerPatch(); y++)
            {
                for (int x = 0; x < getGridUnitsPerPatch(); x++)
                {
                    float height = getWrappedHeight(offset_x + x, offset_y + y);
                    if (height < getSeaLevelMeters())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        // Java: 278
        public com.oddlabs.tt.landscape.LandscapeLeaf getLeafFromCoordinates(float x_f, float y_f)
        {
            int patch_x = coordinateToPatch(x_f);
            int patch_y = coordinateToPatch(y_f);
            return landscape_leaves[patch_y][patch_x];
        }
        // Java: 284
        public int coordinateToPatch(float f)
        {
            return java.lang.JC.f2i(((float)(f * inv_meters_per_patch)));
        }
        // Java: 288
        public float computeInterpolatedHeight(int lod, float x_f, float y_f)
        {
            x_f = (float)(x_f * inv_meters_per_grid_unit);
            y_f = (float)(y_f * inv_meters_per_grid_unit);
            int size = getGridUnitsPerWorld();
            x_f = com.oddlabs.tt.landscape.HeightMap.wrap(x_f, size);
            y_f = com.oddlabs.tt.landscape.HeightMap.wrap(y_f, size);
            int x0 = java.lang.JC.f2i(x_f);
            int y0 = java.lang.JC.f2i(y_f);
            int x1 = (x0 + 1 == size ? 0 : x0 + 1);
            int y1 = (y0 + 1 == size ? 0 : y0 + 1);
            float dx = (float)(x_f - x0);
            float dy = (float)(y_f - y0);
            float h00 = world[y0][x0];
            float h10 = world[y0][x1];
            float h01 = world[y1][x0];
            float h11 = world[y1][x1];
            float h0 = (float)((float)(h00 * ((float)(1 - dx))) + (float)(h10 * dx));
            float h1 = (float)((float)(h01 * ((float)(1 - dx))) + (float)(h11 * dx));
            return (float)((float)(h0 * ((float)(1 - dy))) + (float)(h1 * dy));
        }
        // Java: 320
        public static float wrap(float v, int size)
        {
            if (v >= 0.0f && v < size)
            {
                float s = (float)(v + size);
                return (s >= 2 * size ? 0.0f : (float)(s - size));
            }
            return (float)(((float)((float)(v % size) + size)) % size);
        }
        // Java: 328
        public float getNearestHeight(float x_f, float y_f)
        {
            return computeInterpolatedHeight(0, x_f, y_f);
        }
        // Java: 332
        public float getClampedHeight(int grid_x, int grid_y)
        {
            if (grid_x < 0 || grid_x >= world.Length)
            {
                grid_x = 0;
            }
            if (grid_y < 0 || grid_y >= world.Length)
            {
                grid_y = 0;
            }
            return getHeight(grid_x, grid_y);
        }
        // Java: 341
        public sbyte getBuildValue(int grid_x, int grid_y)
        {
            return build_grid[grid_y][grid_x];
        }
        // Java: 345
        public bool canBuild(int grid_x, int grid_y, int val)
        {
            grid_x = wrapGridCoord(grid_x);
            grid_y = wrapGridCoord(grid_y);
            return build_grid[grid_y][grid_x] >= val;
        }
        // Java: 351
        public bool canDock(int grid_x, int grid_y)
        {
            grid_x = wrapGridCoord(grid_x);
            grid_y = wrapGridCoord(grid_y);
            return dock_grid[grid_y][grid_x] == 1;
        }
        // Java: 357
        public float getWrappedHeight(int grid_x, int grid_y)
        {
            grid_x = wrapGridCoord(grid_x);
            grid_y = wrapGridCoord(grid_y);
            return getHeight(grid_x, grid_y);
        }
        // Java: 363
        public int wrapGridCoord(int coord)
        {
            return (coord + getGridUnitsPerWorld()) & (getGridUnitsPerWorld() - 1);
        }
        // Java: 367
        public float getHeight(int grid_x, int grid_y)
        {
            return world[grid_y][grid_x];
        }
        // Java: 371
        public void editHeight(int grid_x, int grid_y, float height)
        {
            grid_x = wrapGridCoord(grid_x);
            grid_y = wrapGridCoord(grid_y);
            world[grid_y][grid_x] = height;
            java.nio.FloatBuffer buf = org.lwjgl.BufferUtils.createFloatBuffer(1);
            buf.put(height).flip();
            org.lwjgl.opengl.GL11.glBindTexture(org.lwjgl.opengl.GL11.GL_TEXTURE_2D, heightTexture.getHandle());
            org.lwjgl.opengl.GL11.glPixelStorei(org.lwjgl.opengl.GL11.GL_UNPACK_ROW_LENGTH, 0);
            org.lwjgl.opengl.GL11.glPixelStorei(org.lwjgl.opengl.GL11.GL_UNPACK_SKIP_PIXELS, 0);
            org.lwjgl.opengl.GL11.glPixelStorei(org.lwjgl.opengl.GL11.GL_UNPACK_SKIP_ROWS, 0);
            org.lwjgl.opengl.GL11.glPixelStorei(org.lwjgl.opengl.GL11.GL_UNPACK_ALIGNMENT, 1);
            org.lwjgl.opengl.GL11.glTexSubImage2D(org.lwjgl.opengl.GL11.GL_TEXTURE_2D, 0, grid_x, grid_y, 1, 1, org.lwjgl.opengl.GL11.GL_RED, org.lwjgl.opengl.GL11.GL_FLOAT, buf);
            int patch_x1 = grid_x / com.oddlabs.tt.landscape.HeightMap.GRID_UNITS_PER_PATCH;
            int patch_y1 = grid_y / com.oddlabs.tt.landscape.HeightMap.GRID_UNITS_PER_PATCH;
            bool x_border = patch_x1 * com.oddlabs.tt.landscape.HeightMap.GRID_UNITS_PER_PATCH == grid_x;
            bool y_border = patch_y1 * com.oddlabs.tt.landscape.HeightMap.GRID_UNITS_PER_PATCH == grid_y;
            int patch_x0 = (patch_x1 - ((x_border ? 1 : 0)) + patches_per_world) & (patches_per_world - 1);
            int patch_y0 = (patch_y1 - ((y_border ? 1 : 0)) + patches_per_world) & (patches_per_world - 1);
            for (int y = patch_y0; y <= patch_y1; y++)
            {
                for (int x = patch_x0; x <= patch_x1; x++)
                {
                    landscape_leaves[y][x].editHeight(height);
                }
            }
            world_instance.getNotificationListener().patchesEdited(patch_x0, patch_y0, patch_x1, patch_y1);
        }
        static HeightMap()
        {
            HeightMap.plane = new org.joml.Vector3f();
        }
        public HeightMap(com.oddlabs.tt.landscape.World world_instance, int meters_per_world, float sea_level_meters, int texels_per_colormap, int chunks_per_colormap, float[][] world, java.util.List trees, bool[][] access_grid, sbyte[][] dock_grid, sbyte[][] water_grid, sbyte[][] build_grid, int[][] island_ids, java.util.List island_infos) : base()
        {
            this.world = world;
            this.world_instance = world_instance;
            this.trees = trees;
            this.access_grid = access_grid;
            this.dock_grid = dock_grid;
            this.water_grid = water_grid;
            this.build_grid = build_grid;
            this.island_ids = island_ids;
            java.util.Map island_info_map = new java.util.LinkedHashMap();
            foreach (com.oddlabs.tt.landscape.IslandInfo info in java.lang.JIter.of(island_infos))
            {
                island_info_map.put(info.id(), info);
            }
            this.island_info = island_info_map;
            this.meters_per_world = meters_per_world;
            this.sea_level_meters = sea_level_meters;
            patches_per_world = world.Length / com.oddlabs.tt.landscape.HeightMap.GRID_UNITS_PER_PATCH;
            meters_per_patch = com.oddlabs.tt.landscape.HeightMap.GRID_UNITS_PER_PATCH * com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID;
            grid_units_per_world = getPatchesPerWorld() * com.oddlabs.tt.landscape.HeightMap.GRID_UNITS_PER_PATCH;
            inv_meters_per_patch = (float)(1.0f / getMetersPerPatch());
            inv_meters_per_grid_unit = (float)(1.0f / com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID);
            meters_per_chunk = getMetersPerWorld() / chunks_per_colormap;
            quadtree_min_level = java.lang.JC.d2i((java.lang.StrictMath.log(chunks_per_colormap) / java.lang.StrictMath.log(2)));
            patches_per_chunk = meters_per_chunk / getMetersPerPatch();
            int texels_per_colormap_noborder = texels_per_colormap - 2 * com.oddlabs.tt.global.Globals.TEXELS_PER_CHUNK_BORDER * chunks_per_colormap;
            float meters_per_texel = (float)(((float)(getMetersPerWorld())) / texels_per_colormap_noborder);
            meters_per_chunk_border = (float)(meters_per_texel * com.oddlabs.tt.global.Globals.TEXELS_PER_CHUNK_BORDER);
            chunk_tex_scale = (float)(1.0f / ((float)(meters_per_chunk + (float)(2.0f * meters_per_chunk_border))));
            landscape_leaves = java.lang.JA.newArray2<com.oddlabs.tt.landscape.LandscapeLeaf>(getPatchesPerWorld(), getPatchesPerWorld());
            java.nio.FloatBuffer buffer = org.lwjgl.BufferUtils.createFloatBuffer(grid_units_per_world * grid_units_per_world);
            for (int y = 0; y < grid_units_per_world; y++)
            {
                for (int x = 0; x < grid_units_per_world; x++)
                {
                    buffer.put(world[y][x]);
                }
            }
            buffer.flip();
            heightTexture = new com.oddlabs.tt.render.Texture(grid_units_per_world, grid_units_per_world, org.lwjgl.opengl.GL30.GL_R32F, org.lwjgl.opengl.GL11.GL_LINEAR, org.lwjgl.opengl.GL11.GL_LINEAR, org.lwjgl.opengl.GL11.GL_REPEAT);
            org.lwjgl.opengl.GL11.glBindTexture(org.lwjgl.opengl.GL11.GL_TEXTURE_2D, heightTexture.getHandle());
            org.lwjgl.opengl.GL11.glPixelStorei(org.lwjgl.opengl.GL11.GL_UNPACK_ROW_LENGTH, 0);
            org.lwjgl.opengl.GL11.glPixelStorei(org.lwjgl.opengl.GL11.GL_UNPACK_SKIP_PIXELS, 0);
            org.lwjgl.opengl.GL11.glPixelStorei(org.lwjgl.opengl.GL11.GL_UNPACK_SKIP_ROWS, 0);
            org.lwjgl.opengl.GL11.glPixelStorei(org.lwjgl.opengl.GL11.GL_UNPACK_ALIGNMENT, 1);
            org.lwjgl.opengl.GL11.glTexSubImage2D(org.lwjgl.opengl.GL11.GL_TEXTURE_2D, 0, 0, 0, grid_units_per_world, grid_units_per_world, org.lwjgl.opengl.GL11.GL_RED, org.lwjgl.opengl.GL11.GL_FLOAT, buffer);
        }
    }
}
