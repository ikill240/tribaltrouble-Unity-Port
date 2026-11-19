// Translated from CustomMapGenerator.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.resource
{
    // Java: 65
    public class CustomMapGenerator : java.lang.Object, com.oddlabs.tt.resource.WorldGenerator, com.oddlabs.tt.resource.TransferableMap
    {
        public const long serialVersionUID = 3L;
        public const int TEXELS_PER_CHUNK = 512;
        public const int IDEAL_TEXELS_PER_DETAIL = 256;
        public const float IDEAL_DETAIL_ALPHA = 0.15f;
        public string map_file_path;
        public string map_file_name;
        public string map_fingerprint;
        public const string LOAD_FAILURE_PREFIX = "Failed to load custom map: ";
        public int meters_per_world;
        public com.oddlabs.tt.procedural.Landscape.TerrainType terrain;
        public int grid_units;
        // Java: 143
        public static com.oddlabs.tt.render.Texture createDetail(com.oddlabs.tt.resource.GLImage detail_image, int base_level)
        {
            com.oddlabs.tt.resource.GLImage[] detail_mipmaps = detail_image.buildMipMaps(base_level, com.oddlabs.tt.global.Globals.LANDSCAPE_DETAIL_FADEOUT_FACTOR, true, false);
            return new com.oddlabs.tt.render.Texture(detail_mipmaps, org.lwjgl.opengl.GL11.GL_RGBA8, org.lwjgl.opengl.GL11.GL_LINEAR_MIPMAP_LINEAR, org.lwjgl.opengl.GL11.GL_LINEAR, org.lwjgl.opengl.GL11.GL_REPEAT, org.lwjgl.opengl.GL11.GL_REPEAT);
        }
        // Java: 150
        public static int getTexelsPerGridUnit()
        {
            return com.oddlabs.tt.global.Globals.TEXELS_PER_GRID_UNIT / java.lang.JC.d2i(java.lang.StrictMath.pow(2, com.oddlabs.tt.global.Globals.TEXTURE_MIP_SHIFT[com.oddlabs.tt.global.Settings.getSettings().graphic_detail]));
        }
        // Java: 155
        public com.oddlabs.tt.procedural.Landscape.TerrainType getTerrainType()
        {
            return terrain;
        }
        // Java: 160
        public int getMetersPerWorld()
        {
            return meters_per_world;
        }
        // Java: 165
        public com.oddlabs.tt.resource.FogInfo getFogInfo()
        {
            return com.oddlabs.tt.procedural.Landscape.getFogInfo(terrain, meters_per_world);
        }
        // Java: 172
        public static java.nio.file.Path sharedMapsDir()
        {
            java.nio.file.Path game_dir = com.oddlabs.tt.render.Renderer.getLocalInput().getGameDir();
            return ((game_dir != null ? game_dir : java.nio.file.Path_.of(""))).resolve("maps");
        }
        // Java: 181
        public java.io.File resolveMapFile()
        {
            java.nio.file.Path shared_dir = com.oddlabs.tt.resource.CustomMapGenerator.sharedMapsDir();
            java.nio.file.Path host_path = null;
            try
            {
                host_path = java.nio.file.Path_.of(map_file_path);
            }
            catch (System.Exception e)
            {
            }
            java.nio.file.Path mismatched = null;
            foreach (java.nio.file.Path candidate in new java.nio.file.Path[] { host_path, shared_dir.resolve(map_file_name), shared_dir.resolve(alternateFileName()) })
            {
                if (candidate == null || !java.nio.file.Files.isRegularFile(candidate))
                {
                    continue;
                }
                if (map_fingerprint == null)
                {
                    return candidate.toFile();
                }
                try
                {
                    if (java.lang.JO.equals(map_fingerprint, com.oddlabs.tt.resource.CustomMapGenerator.fingerprint(candidate)))
                    {
                        return candidate.toFile();
                    }
                }
                catch (System.IO.IOException e_2)
                {
                    continue;
                }
                mismatched = candidate;
            }
            if (mismatched != null)
            {
                throw new System.IO.IOException(java.lang.JStr.valueOf(com.oddlabs.tt.resource.CustomMapGenerator.LOAD_FAILURE_PREFIX) + "Your copy of \"" + java.lang.JStr.valueOf(map_file_name) + "\" (" + java.lang.JStr.valueOf(mismatched) + ") is different from the host's. Every player needs the exact same file - copy the host's version into " + java.lang.JStr.valueOf(shared_dir) + " and join again.", (System.Exception)new System.IO.IOException("map fingerprint mismatch"));
            }
            throw new System.IO.IOException(java.lang.JStr.valueOf(com.oddlabs.tt.resource.CustomMapGenerator.LOAD_FAILURE_PREFIX) + "This game uses the custom map \"" + java.lang.JStr.valueOf(map_file_name) + "\", which isn't on this computer. Every player needs their own copy - put " + java.lang.JStr.valueOf(map_file_name) + " in " + java.lang.JStr.valueOf(shared_dir) + " and join again.", (System.Exception)new System.IO.IOException(map_file_name));
        }
        // Java: 214
        public static string fingerprint(java.nio.file.Path file)
        {
            {
                java.io.InputStream @in = java.nio.file.Files.newInputStream(file);
                try
                {
                    java.security.MessageDigest digest = java.security.MessageDigest.getInstance("SHA-256");
                    sbyte[] buffer = new sbyte[1 << 16];
                    for (int n = default; (n = @in.read(buffer)) > 0; )
                    {
                        digest.update(buffer, 0, n);
                    }
                    return java.util.HexFormat.of().formatHex(digest.digest());
                }
                catch (System.Exception e)
                {
                    throw new System.IO.IOException(java.lang.JStr.valueOf(e), (System.Exception)e);
                }
                finally
                {
                    if (@in != null) @in.close();
                }
            }
        }
        // Java: 226
        public com.oddlabs.tt.resource.WorldInfo generate(int num_players, int initial_unit_count, float random_start_pos, bool team_together, int[] player_teams)
        {
            com.oddlabs.procedural.AuthoredTerrain authored_terrain = default;
            try
            {
                authored_terrain = com.oddlabs.procedural.AuthoredTerrain.load(resolveMapFile());
            }
            catch (System.IO.IOException e)
            {
                throw new System.IO.IOException(java.lang.JStr.valueOf(com.oddlabs.tt.resource.CustomMapGenerator.LOAD_FAILURE_PREFIX) + "Couldn't read the custom map \"" + java.lang.JStr.valueOf(map_file_name) + "\": " + java.lang.JStr.valueOf(e.getMessage()), (System.Exception)e);
            }
            int colormap_size = grid_units * com.oddlabs.tt.resource.CustomMapGenerator.getTexelsPerGridUnit();
            int chunks_per_colormap = colormap_size / com.oddlabs.tt.resource.CustomMapGenerator.TEXELS_PER_CHUNK;
            java.time.Instant time_before = java.time.Instant.now();
            int base_level = com.oddlabs.tt.global.Globals.LANDSCAPE_DETAIL_FADEOUT_BASE_LEVEL;
            int detail_mip_level = com.oddlabs.tt.resource.CustomMapGenerator.IDEAL_TEXELS_PER_DETAIL / com.oddlabs.tt.global.Globals.DETAIL_SIZE - 1;
            int detail_prefade_level = java.lang.Math.max(detail_mip_level - base_level, 0);
            float detail_prefade = (float)(com.oddlabs.tt.resource.CustomMapGenerator.IDEAL_DETAIL_ALPHA * ((float)(java.lang.StrictMath.pow(com.oddlabs.tt.global.Globals.LANDSCAPE_DETAIL_FADEOUT_FACTOR, detail_prefade_level))));
            base_level -= detail_mip_level;
            base_level = java.lang.Math.min(base_level, 1);
            com.oddlabs.tt.procedural.Landscape landscape = new com.oddlabs.tt.procedural.Landscape(num_players, meters_per_world, terrain, detail_prefade, 0.0f, 0.0f, 0.0f, 0, initial_unit_count, random_start_pos, false, authored_terrain, team_together, player_teams);
            java.time.Instant time_after = java.time.Instant.now();
            java.lang.IO.println("Custom map loaded in " + java.lang.JStr.valueOf(java.time.Duration.between(time_before, time_after)));
            com.oddlabs.tt.resource.BlendInfo[] blend_infos = landscape.getBlendInfos();
            com.oddlabs.tt.render.Texture detail = com.oddlabs.tt.resource.CustomMapGenerator.createDetail(landscape.getDetail(), base_level);
            com.oddlabs.tt.landscape.LandscapeBaker baker = new com.oddlabs.tt.landscape.LandscapeBaker();
            float textureScale = (float)(((float)(colormap_size)) / com.oddlabs.tt.global.Globals.STRUCTURE_SIZE);
            com.oddlabs.tt.resource.WorldInfo.Maps maps = baker.bake(colormap_size, textureScale, blend_infos);
            com.oddlabs.tt.form.ProgressForm.progress();
            return new com.oddlabs.tt.resource.WorldInfo(meters_per_world, landscape.getSeaLevelMeters(), colormap_size, chunks_per_colormap, null, maps, detail, landscape.getHeight(), landscape.getTrees(), landscape.getPalmtrees(), landscape.getRock(), landscape.getIron(), landscape.getPlants(), landscape.getAccessGrid(), landscape.getDockGrid(), landscape.getWaterGrid(), landscape.getBuildGrid(), landscape.getIslandIds(), landscape.getIslandInfos(), landscape.getStartingLocations(), blend_infos);
        }
        // Java: 285
        public string getMapFileName()
        {
            return map_file_name;
        }
        // Java: 290
        public bool hasLocalCopy()
        {
            try
            {
                resolveMapFile();
                return true;
            }
            catch (System.IO.IOException e)
            {
                return false;
            }
        }
        // Java: 300
        public sbyte[] readCompressed()
        {
            java.io.ByteArrayOutputStream bytes = new java.io.ByteArrayOutputStream();
            {
                java.io.InputStream @in = java.nio.file.Files.newInputStream(resolveMapFile().toPath());
                java.io.OutputStream @out = new java.util.zip.GZIPOutputStream(bytes, 1 << 16);
                try
                {
                    @in.transferTo(@out);
                }
                finally
                {
                    if (@out != null) @out.close();
                    if (@in != null) @in.close();
                }
            }
            return bytes.toByteArray();
        }
        // Java: 314
        public java.nio.file.Path installDownloadedMap(sbyte[] compressed)
        {
            java.nio.file.Path dir = com.oddlabs.tt.resource.CustomMapGenerator.sharedMapsDir();
            java.nio.file.Files.createDirectories(dir);
            java.nio.file.Path temp = java.nio.file.Files.createTempFile(dir, "download-", ".part");
            try
            {
                {
                    java.io.InputStream @in = new java.util.zip.GZIPInputStream(new java.io.ByteArrayInputStream(compressed), 1 << 16);
                    java.io.OutputStream @out = java.nio.file.Files.newOutputStream(temp);
                    try
                    {
                        @in.transferTo(@out);
                    }
                    finally
                    {
                        if (@out != null) @out.close();
                        if (@in != null) @in.close();
                    }
                }
                if (map_fingerprint != null && !java.lang.JO.equals(map_fingerprint, com.oddlabs.tt.resource.CustomMapGenerator.fingerprint(temp)))
                {
                    throw new System.IO.IOException("the received map doesn't match the host's file");
                }
                com.oddlabs.procedural.AuthoredTerrain.load(temp.toFile());
                java.nio.file.Path target = dir.resolve(map_file_name);
                if (java.nio.file.Files.exists(target))
                {
                    target = dir.resolve(alternateFileName());
                }
                java.nio.file.Files.move(temp, target, java.nio.file.StandardCopyOption.REPLACE_EXISTING);
                return target;
            }
            finally
            {
                java.nio.file.Files.deleteIfExists(temp);
            }
        }
        // Java: 337
        public string alternateFileName()
        {
            string tag = (map_fingerprint != null ? map_fingerprint.substring(0, 8) : "copy");
            int dot = map_file_name.lastIndexOf('.');
            return (dot > 0 ? java.lang.JStr.valueOf(map_file_name.substring(0, dot)) + "-" + java.lang.JStr.valueOf(tag) + java.lang.JStr.valueOf(map_file_name.substring(dot)) : java.lang.JStr.valueOf(map_file_name) + "-" + java.lang.JStr.valueOf(tag));
        }
        public CustomMapGenerator(string map_file_path, int meters_per_world, com.oddlabs.tt.procedural.Landscape.TerrainType terrain) : base()
        {
            this.map_file_path = map_file_path;
            this.map_file_name = new java.io.File(map_file_path).getName();
            string fingerprint_v = default;
            try
            {
                fingerprint_v = com.oddlabs.tt.resource.CustomMapGenerator.fingerprint(java.nio.file.Path_.of(map_file_path));
            }
            catch (System.Exception e) when (e is System.IO.IOException || e is System.Exception)
            {
                fingerprint_v = null;
            }
            this.map_fingerprint = fingerprint_v;
            this.terrain = terrain;
            int authored_grid_size = default;
            try
            {
                authored_grid_size = com.oddlabs.procedural.AuthoredTerrain.load(new java.io.File(map_file_path)).getHeightChannel().getWidth();
            }
            catch (System.IO.IOException e_2)
            {
                authored_grid_size = meters_per_world / com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID;
            }
            this.meters_per_world = authored_grid_size * com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID;
            this.grid_units = authored_grid_size;
            if (java.util.Arrays.stream(com.oddlabs.tt.procedural.Landscape.VALID_METERS_PER_WORLD).noneMatch(new java.util.function.IntPredicate_L((int v) =>
            {
                return v == this.meters_per_world;
            })))
            {
                throw new System.ArgumentException("Custom map '" + java.lang.JStr.valueOf(map_file_path) + "' has a height field " + (authored_grid_size) + "x" + (authored_grid_size) + " (" + (this.meters_per_world) + " meters), which isn't one of the supported world sizes " + java.lang.JStr.valueOf(java.util.Arrays.toString(com.oddlabs.tt.procedural.Landscape.VALID_METERS_PER_WORLD)) + ". Resave the map at one of those sizes.");
            }
        }
    }
}
