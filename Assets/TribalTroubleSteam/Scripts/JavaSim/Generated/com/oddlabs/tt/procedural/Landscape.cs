// Translated from Landscape.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.procedural
{
    // Java: 35
    public class Landscape : java.lang.Object
    {
        public const bool DEBUG = false;
        public const int STRUCTURE_SEED = 42;
        public const int NUM_PLANT_TYPES = 4;
        public const float VISIBLE_GROUND_ALPHA = 0.5f;
        public const float SEABOTTOM_DEPTH_OFFSET_METERS = 3.0f;
        public const float UNDERWATER_SHADING = 0.3f;
        public static org.joml.Vector4fc NATIVE_SEA_BOTTOM_COLOR;
        public static org.joml.Vector4fc VIKING_SEA_BOTTOM_COLOR;
        public static org.joml.Vector4fc NATIVE_FOG_COLOR;
        public static org.joml.Vector4fc VIKING_FOG_COLOR;
        public const float NATIVE_FOG_DENSITY = 0.0012f;
        public const float VIKING_FOG_DENSITY = 0.0016f;
        public const float NATIVE_FOG_HEIGHT = 1.2f;
        public const float VIKING_FOG_HEIGHT = 1.4f;
        public static org.joml.Vector4fc NATIVE_SAND_COLOR;
        public static org.joml.Vector4fc NATIVE_DIRT_COLOR;
        public static org.joml.Vector4fc NATIVE_GRASS_COLOR;
        public static org.joml.Vector4fc NATIVE_ROCK_TINT;
        public static org.joml.Vector4fc VIKING_GRAVEL_COLOR;
        public static org.joml.Vector4fc VIKING_SOIL_COLOR;
        public static org.joml.Vector4fc VIKING_GRASS_COLOR;
        public static org.joml.Vector4fc VIKING_SNOW_COLOR;
        public static org.joml.Vector4fc VIKING_CLIFF_GRASS_TINT;
        public static float DETAIL_GREY;
        public static org.joml.Vector4fc BLEND_LIGHTING_COLOR;
        // Java: 75
        public sealed class TerrainType : java.lang.Enum
        {
            public static TerrainType[] values() { return (TerrainType[])VALUES_.Clone(); }
            public static TerrainType valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly TerrainType NATIVE = new TerrainType("NATIVE", 0);
            public static readonly TerrainType VIKING = new TerrainType("VIKING", 1);
            public static readonly TerrainType[] VALUES_ = new TerrainType[] { NATIVE, VIKING };
            public TerrainType(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        // Java: 81
        public sealed class Ground : java.lang.Enum
        {
            public static Ground[] values() { return (Ground[])VALUES_.Clone(); }
            public static Ground valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly Ground BEACH = new Ground("BEACH", 0);
            public static readonly Ground DIRT = new Ground("DIRT", 1);
            public static readonly Ground ROCK = new Ground("ROCK", 2);
            public static readonly Ground GRASS = new Ground("GRASS", 3);
            public static readonly Ground SNOW = new Ground("SNOW", 4);
            public static readonly Ground[] VALUES_ = new Ground[] { BEACH, DIRT, ROCK, GRASS, SNOW };
            public Ground(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        public const int MIN_ISLAND_AREA = 2000;
        public java.util.Random random;
        public com.oddlabs.tt.resource.BlendInfo[] blend_infos;
        public com.oddlabs.tt.resource.StructureBlend supply_shadows;
        public com.oddlabs.tt.resource.StructureBlend terrain_shadows;
        // Java: 98
        public class StructureLayers : java.lang.Object
        {
            public com.oddlabs.procedural.Layer diffuse() { return diffuse_f; }
            public com.oddlabs.procedural.Layer normal() { return normal_f; }
            public StructureLayers(com.oddlabs.procedural.Layer diffuse, com.oddlabs.procedural.Layer normal)
            {
                this.diffuse_f = diffuse;
                this.normal_f = normal;
            }
            public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is StructureLayers r)) return false; return true && java.lang.JO.equals(diffuse_f, r.diffuse_f) && java.lang.JO.equals(normal_f, r.normal_f); }
            public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(diffuse_f); h = 31 * h + java.lang.JO.hashCode(normal_f); } return h; }
            public override string toString() { return "StructureLayers[" + "diffuse=" + java.lang.JO.toString(diffuse_f) + ", normal=" + java.lang.JO.toString(normal_f) + "]"; }
            public com.oddlabs.procedural.Layer diffuse_f;
            public com.oddlabs.procedural.Layer normal_f;
        }
        public com.oddlabs.tt.resource.GLIntImage[] structures;
        public com.oddlabs.tt.resource.GLIntImage[] structure_normals;
        public com.oddlabs.tt.resource.GLIntImage detail;
        public com.oddlabs.tt.resource.GLByteImage[] alpha_maps;
        public com.oddlabs.procedural.Channel height;
        public com.oddlabs.procedural.Channel slope;
        public com.oddlabs.procedural.Channel water_map;
        public com.oddlabs.procedural.Channel deep_water_map;
        public com.oddlabs.procedural.Channel dock_map;
        public com.oddlabs.procedural.Channel island_ids;
        public com.oddlabs.procedural.Channel access;
        public com.oddlabs.procedural.Channel access_exported;
        public com.oddlabs.procedural.Channel relheight;
        public com.oddlabs.procedural.Channel highlight;
        public com.oddlabs.procedural.Channel shadow;
        public com.oddlabs.procedural.Channel supply_shadow;
        public com.oddlabs.procedural.Channel trees;
        public com.oddlabs.procedural.Channel palmtrees;
        public com.oddlabs.procedural.Channel rock;
        public com.oddlabs.procedural.Channel iron;
        public java.util.List island_locations;
        public java.util.ArrayList island_areas;
        public java.util.Map island_info;
        public int num_players;
        public int meters_per_world;
        public int meters_per_height_unit;
        public int unit_grids_per_world;
        public int height_scale;
        public float sea_level_meters;
        public int detail_size;
        public int structure_size;
        public float detail_alpha_value;
        public int features;
        public float hills;
        public float vegetation_amount;
        public float supplies_amount;
        public int supplyshadow_size;
        public int seed;
        public float area;
        public int max_trees;
        public int max_palmtrees;
        public int max_rock;
        public int max_iron;
        public int max_plants;
        public float access_threshold;
        public float build_threshold;
        public com.oddlabs.tt.procedural.Landscape.TerrainType terrain;
        public bool archipelago;
        public sbyte[][] build;
        public sbyte[][] dock;
        public sbyte[][] water;
        public float[][] player_locations;
        public int[][] supply_locations;
        public float[][] plants;
        public sbyte[][] ground;
        // Java: 332
        public com.oddlabs.tt.procedural.Landscape.StructureLayers[] generateStructuresNative(com.oddlabs.procedural.Channel voronoi4, com.oddlabs.procedural.Channel voronoi8, com.oddlabs.procedural.Channel voronoi8_hit, com.oddlabs.procedural.Channel voronoi16, com.oddlabs.procedural.Channel voronoi16_hit, com.oddlabs.procedural.Channel voronoi32, com.oddlabs.procedural.Channel voronoi32_hit, com.oddlabs.procedural.Channel noise8, com.oddlabs.procedural.Channel noise256)
        {
            com.oddlabs.tt.procedural.Landscape.StructureLayers[] structures = new com.oddlabs.tt.procedural.Landscape.StructureLayers[7];
            com.oddlabs.tt.form.ProgressForm.progress((float)(1 / 8.0f));
            structures[0] = com.oddlabs.tt.procedural.Landscape.genSand(structure_size, noise8.copy(), noise256.copy());
            structures[1] = com.oddlabs.tt.procedural.Landscape.genDirt(structure_size, noise8.copy(), noise256.copy(), voronoi32.copy());
            com.oddlabs.procedural.Layer structure_rubble = com.oddlabs.tt.procedural.Landscape.genRubble(structure_size, noise8.copy(), voronoi4.copy(), voronoi8.copy(), voronoi16.copy(), structures[1].diffuse_f.copy()).diffuse_f;
            structures[2] = com.oddlabs.tt.procedural.Landscape.genRubble(structure_size, noise8.copy(), voronoi4.copy(), voronoi8.copy(), voronoi16.copy(), structures[1].diffuse_f.copy());
            structures[4] = genGrass(structure_size, noise8.copy(), noise256.copy());
            structures[3] = com.oddlabs.tt.procedural.Landscape.genRock(structure_size, noise8.copy(), noise256.copy(), voronoi4.copy(), voronoi8.copy(), voronoi16.copy(), structures[2].diffuse_f.copy(), structures[4].diffuse_f.copy());
            structures[5] = com.oddlabs.tt.procedural.Landscape.genBlack();
            structures[6] = com.oddlabs.tt.procedural.Landscape.genSeabottom(terrain, structure_size, noise8.copy(), noise256.copy(), voronoi4.copy(), voronoi8.copy());
            com.oddlabs.tt.procedural.Landscape.StructureLayers structure_detail = com.oddlabs.tt.procedural.Landscape.genDetail(detail_size, detail_alpha_value, com.oddlabs.tt.procedural.Landscape.STRUCTURE_SEED, noise8.copy());
            detail = new com.oddlabs.tt.resource.GLIntImage(structure_detail.diffuse_f);
            return structures;
        }
        // Java: 366
        public com.oddlabs.tt.procedural.Landscape.StructureLayers[] generateStructuresViking(com.oddlabs.procedural.Channel voronoi4, com.oddlabs.procedural.Channel voronoi8, com.oddlabs.procedural.Channel voronoi8_hit, com.oddlabs.procedural.Channel voronoi16, com.oddlabs.procedural.Channel voronoi16_hit, com.oddlabs.procedural.Channel voronoi32, com.oddlabs.procedural.Channel voronoi32_hit, com.oddlabs.procedural.Channel noise8, com.oddlabs.procedural.Channel noise256)
        {
            com.oddlabs.tt.procedural.Landscape.StructureLayers[] structures = new com.oddlabs.tt.procedural.Landscape.StructureLayers[7];
            com.oddlabs.tt.form.ProgressForm.progress((float)(1 / 8.0f));
            structures[0] = com.oddlabs.tt.procedural.Landscape.genGravel(structure_size, noise8.copy(), noise256.copy());
            structures[1] = com.oddlabs.tt.procedural.Landscape.genSoil(structure_size, noise8.copy(), noise256.copy(), voronoi32.copy());
            structures[3] = genGrass(structure_size, noise8.copy(), noise256.copy());
            structures[3].diffuse_f.multiply(0.75f, 0.9f, 1.1f);
            structures[2] = com.oddlabs.tt.procedural.Landscape.genCliff(structure_size, noise8.copy(), noise256.copy(), voronoi4.copy(), voronoi8.copy(), voronoi16.copy(), structures[1].diffuse_f.copy(), structures[3].diffuse_f.copy());
            structures[4] = com.oddlabs.tt.procedural.Landscape.genSnow(structure_size, noise8.copy(), noise256.copy());
            structures[5] = com.oddlabs.tt.procedural.Landscape.genBlack();
            structures[6] = com.oddlabs.tt.procedural.Landscape.genSeabottom(terrain, structure_size, noise8.copy(), noise256.copy(), voronoi4.copy(), voronoi8.copy());
            com.oddlabs.tt.procedural.Landscape.StructureLayers structure_detail = com.oddlabs.tt.procedural.Landscape.genDetail(detail_size, detail_alpha_value, com.oddlabs.tt.procedural.Landscape.STRUCTURE_SEED, noise8.copy());
            detail = new com.oddlabs.tt.resource.GLIntImage(structure_detail.diffuse_f);
            return structures;
        }
        // Java: 396
        public static com.oddlabs.tt.procedural.Landscape.StructureLayers genSand(int size, com.oddlabs.procedural.Channel noise8, com.oddlabs.procedural.Channel noise256)
        {
            com.oddlabs.procedural.Channel empty = new com.oddlabs.procedural.Channel(size, size).fill(1.0f);
            com.oddlabs.procedural.Channel sand_bump1 = noise8.brightness(0.75f);
            com.oddlabs.procedural.Channel sand_bump2 = noise256.brightness(0.15f);
            com.oddlabs.procedural.Layer sand = new com.oddlabs.procedural.Layer(empty.copy().fill(com.oddlabs.tt.procedural.Landscape.NATIVE_SAND_COLOR.x()), empty.copy().fill(com.oddlabs.tt.procedural.Landscape.NATIVE_SAND_COLOR.y()), empty.copy().fill(com.oddlabs.tt.procedural.Landscape.NATIVE_SAND_COLOR.z()));
            com.oddlabs.procedural.Channel bump = sand_bump1.channelAdd(sand_bump2);
            sand.bump(bump, (float)(size / 1024.0f), 0.0f, 0.1f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 0.0f);
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                sand.saveAsPNG("structure_sand");
            }
            return new com.oddlabs.tt.procedural.Landscape.StructureLayers(sand, com.oddlabs.tt.procedural.Landscape.getFlatNormal(size));
        }
        // Java: 409
        public static com.oddlabs.tt.procedural.Landscape.StructureLayers genGravel(int size, com.oddlabs.procedural.Channel noise8, com.oddlabs.procedural.Channel noise256)
        {
            com.oddlabs.procedural.Channel empty = new com.oddlabs.procedural.Channel(size, size).fill(1.0f);
            com.oddlabs.procedural.Channel gravel_bump1 = noise8.brightness(0.5f);
            com.oddlabs.procedural.Channel gravel_bump2 = noise256.brightness(0.5f);
            com.oddlabs.procedural.Layer gravel = new com.oddlabs.procedural.Layer(empty.copy().fill(com.oddlabs.tt.procedural.Landscape.VIKING_GRAVEL_COLOR.x()), empty.copy().fill(com.oddlabs.tt.procedural.Landscape.VIKING_GRAVEL_COLOR.y()), empty.copy().fill(com.oddlabs.tt.procedural.Landscape.VIKING_GRAVEL_COLOR.z()));
            com.oddlabs.procedural.Channel bump = gravel_bump1.channelAdd(gravel_bump2);
            gravel.bump(bump, (float)(size / 1024.0f), 0.0f, 0.1f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 0.0f);
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                gravel.saveAsPNG("structure_gravel");
            }
            return new com.oddlabs.tt.procedural.Landscape.StructureLayers(gravel, com.oddlabs.tt.procedural.Landscape.getFlatNormal(size));
        }
        // Java: 422
        public static com.oddlabs.tt.procedural.Landscape.StructureLayers genDirt(int size, com.oddlabs.procedural.Channel noise8, com.oddlabs.procedural.Channel noise256, com.oddlabs.procedural.Channel voronoi32)
        {
            com.oddlabs.procedural.Channel empty = new com.oddlabs.procedural.Channel(size, size).fill(1.0f);
            com.oddlabs.procedural.Channel dirt_bump1 = noise8.brightness(0.8f);
            com.oddlabs.procedural.Channel dirt_bump2 = noise256.brightness(0.1f);
            com.oddlabs.procedural.Channel dirt_bump3 = voronoi32.brightness(0.1f);
            com.oddlabs.procedural.Layer dirt = new com.oddlabs.procedural.Layer(empty.copy().fill(com.oddlabs.tt.procedural.Landscape.NATIVE_DIRT_COLOR.x()), empty.copy().fill(com.oddlabs.tt.procedural.Landscape.NATIVE_DIRT_COLOR.y()), empty.copy().fill(com.oddlabs.tt.procedural.Landscape.NATIVE_DIRT_COLOR.z()));
            com.oddlabs.procedural.Channel dirt_bump = dirt_bump1.channelAdd(dirt_bump2).channelAdd(dirt_bump3);
            dirt_bump.perturb(noise8, 0.05f);
            dirt.bump(dirt_bump, (float)(size / 128.0f), 0.0f, 0.5f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 0.0f);
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                dirt.saveAsPNG("structure_dirt");
            }
            return new com.oddlabs.tt.procedural.Landscape.StructureLayers(dirt, com.oddlabs.tt.procedural.Landscape.getFlatNormal(size));
        }
        // Java: 438
        public static com.oddlabs.tt.procedural.Landscape.StructureLayers genSoil(int size, com.oddlabs.procedural.Channel noise8, com.oddlabs.procedural.Channel noise256, com.oddlabs.procedural.Channel voronoi32)
        {
            com.oddlabs.procedural.Channel empty = new com.oddlabs.procedural.Channel(size, size).fill(1.0f);
            com.oddlabs.procedural.Channel soil_bump1 = noise8.brightness(0.8f);
            com.oddlabs.procedural.Channel soil_bump2 = noise256.brightness(0.1f);
            com.oddlabs.procedural.Channel soil_bump3 = voronoi32.brightness(0.1f);
            com.oddlabs.procedural.Layer soil = new com.oddlabs.procedural.Layer(empty.copy().fill(com.oddlabs.tt.procedural.Landscape.VIKING_SOIL_COLOR.x()), empty.copy().fill(com.oddlabs.tt.procedural.Landscape.VIKING_SOIL_COLOR.y()), empty.copy().fill(com.oddlabs.tt.procedural.Landscape.VIKING_SOIL_COLOR.z()));
            com.oddlabs.procedural.Channel soil_bump = soil_bump1.channelAdd(soil_bump2).channelAdd(soil_bump3);
            soil_bump.perturb(noise8, 0.05f);
            soil.bump(soil_bump, (float)(size / 128.0f), 0.0f, 0.5f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 0.0f);
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                soil.saveAsPNG("structure_soil");
            }
            com.oddlabs.procedural.Channel zero = new com.oddlabs.procedural.Channel(size, size).fill(0.0f);
            return new com.oddlabs.tt.procedural.Landscape.StructureLayers(soil, soil_bump.toNormalMap(1.5f, zero));
        }
        // Java: 455
        public com.oddlabs.tt.procedural.Landscape.StructureLayers genGrass(int size, com.oddlabs.procedural.Channel noise8, com.oddlabs.procedural.Channel noise256)
        {
            com.oddlabs.procedural.Channel empty = new com.oddlabs.procedural.Channel(size, size).fill(1.0f);
            com.oddlabs.procedural.Channel grass_bump = noise8.copy().rotate(90).channelAdd(noise256.brightness(0.02f));
            org.joml.Vector4fc grassColor = (((object)terrain == (object)com.oddlabs.tt.procedural.Landscape.TerrainType.NATIVE) ? com.oddlabs.tt.procedural.Landscape.NATIVE_GRASS_COLOR : com.oddlabs.tt.procedural.Landscape.VIKING_GRASS_COLOR);
            com.oddlabs.procedural.Layer grass = new com.oddlabs.procedural.Layer(empty.copy().fill(grassColor.x()), empty.copy().fill(grassColor.y()), empty.copy().fill(grassColor.z()));
            grass.r.channelAdd(noise8.brightness(0.2f));
            grass.bump(grass_bump, (float)(size / 256.0f), 0.0f, 0.6f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 0.0f);
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                grass.saveAsPNG("structure_grass");
            }
            com.oddlabs.procedural.Channel zero = new com.oddlabs.procedural.Channel(size, size).fill(0.0f);
            return new com.oddlabs.tt.procedural.Landscape.StructureLayers(grass, grass_bump.toNormalMap(0.5f, zero));
        }
        // Java: 469
        public static com.oddlabs.tt.procedural.Landscape.StructureLayers genRubble(int size, com.oddlabs.procedural.Channel noise8, com.oddlabs.procedural.Channel voronoi4, com.oddlabs.procedural.Channel voronoi8, com.oddlabs.procedural.Channel voronoi16, com.oddlabs.procedural.Layer rubble)
        {
            com.oddlabs.procedural.Channel rubble_bump1 = voronoi4.multiply(0.4f);
            com.oddlabs.procedural.Channel rubble_bump2 = voronoi8.multiply(0.3f);
            com.oddlabs.procedural.Channel rubble_bump3 = voronoi16.multiply(0.2f);
            com.oddlabs.procedural.Channel rubble_bump = rubble_bump1.channelAdd(rubble_bump2).channelAdd(rubble_bump3).dynamicRange();
            rubble_bump.perturb(noise8, 0.1f);
            rubble.multiply(0.9f).bump(rubble_bump, (float)(size / 128.0f), 0.0f, 0.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 0.0f);
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                rubble.saveAsPNG("structure_rubble");
            }
            com.oddlabs.procedural.Channel zero = new com.oddlabs.procedural.Channel(size, size).fill(0.0f);
            return new com.oddlabs.tt.procedural.Landscape.StructureLayers(rubble, rubble_bump.toNormalMap(2.5f, zero));
        }
        // Java: 483
        public static com.oddlabs.tt.procedural.Landscape.StructureLayers genRock(int size, com.oddlabs.procedural.Channel noise8, com.oddlabs.procedural.Channel noise256, com.oddlabs.procedural.Channel voronoi4, com.oddlabs.procedural.Channel voronoi8, com.oddlabs.procedural.Channel voronoi16, com.oddlabs.procedural.Layer rubble, com.oddlabs.procedural.Layer grass)
        {
            com.oddlabs.procedural.Channel rock_bump1 = voronoi4.dynamicRange().contrast(1.1f).brightness(2.0f).gamma2().multiply(0.35f);
            com.oddlabs.procedural.Channel rock_bump2 = voronoi8.dynamicRange().contrast(1.1f).brightness(2.0f).gamma2().multiply(0.3f);
            com.oddlabs.procedural.Channel rock_bump3 = voronoi16.dynamicRange().contrast(1.1f).brightness(2.0f).gamma2().multiply(0.2f);
            com.oddlabs.procedural.Channel rock_bump = rock_bump1.channelAdd(rock_bump2).channelAdd(rock_bump3).channelAdd(noise256.multiply(0.4f));
            rock_bump.perturb(noise8, 0.1f);
            com.oddlabs.procedural.Layer rock = rubble.copy();
            rock.toHSV();
            rock.r = noise8.copy().dynamicRange(0.05f, 0.1f);
            rock.toRGB();
            rock.layerBlend(rubble.multiply(com.oddlabs.tt.procedural.Landscape.NATIVE_ROCK_TINT.x(), com.oddlabs.tt.procedural.Landscape.NATIVE_ROCK_TINT.y(), com.oddlabs.tt.procedural.Landscape.NATIVE_ROCK_TINT.z()), noise8.gamma8().invert().contrast(4.0f));
            rock.layerBlend(grass.multiply(0.5f), noise8.rotate(90).multiply(0.5f));
            rock.bump(rock_bump, (float)(size / 192.0f), 0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 0.0f);
            rock.gamma2().multiply(0.9f);
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                rock.saveAsPNG("structure_rock");
            }
            com.oddlabs.procedural.Channel mica = noise256.copy().gamma(0.5f).threshold(0.6f, 1.0f).multiply(0.8f);
            com.oddlabs.procedural.Layer normalMapLayer = rock_bump.toNormalMap(3.0f, mica);
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                normalMapLayer.saveAsPNG("structure_rock_normal");
            }
            return new com.oddlabs.tt.procedural.Landscape.StructureLayers(rock, normalMapLayer);
        }
        // Java: 510
        public static com.oddlabs.tt.procedural.Landscape.StructureLayers genCliff(int size, com.oddlabs.procedural.Channel noise8, com.oddlabs.procedural.Channel noise256, com.oddlabs.procedural.Channel voronoi4, com.oddlabs.procedural.Channel voronoi8, com.oddlabs.procedural.Channel voronoi16, com.oddlabs.procedural.Layer rubble, com.oddlabs.procedural.Layer grass)
        {
            com.oddlabs.procedural.Channel cliff_bump1 = voronoi4.dynamicRange().contrast(1.1f).brightness(2.0f).gamma2().multiply(0.3f);
            com.oddlabs.procedural.Channel cliff_bump2 = voronoi8.dynamicRange().contrast(1.1f).brightness(2.0f).gamma2().multiply(0.3f);
            com.oddlabs.procedural.Channel cliff_bump3 = voronoi16.dynamicRange().contrast(1.1f).brightness(2.0f).gamma2().multiply(0.25f);
            com.oddlabs.procedural.Channel cliff_bump = cliff_bump1.channelAdd(cliff_bump2).channelAdd(cliff_bump3).channelAdd(noise256.multiply(0.15f));
            cliff_bump.dynamicRange().perturb(noise8, 0.1f);
            com.oddlabs.procedural.Layer cliff = rubble.copy();
            cliff.toHSV();
            cliff.r = noise8.copy().dynamicRange(0.05f, 0.1f);
            cliff.g.multiply(0.75f);
            cliff.toRGB();
            cliff.layerBlend(rubble, noise8.gamma8().invert().contrast(4.0f));
            cliff.layerBlend(grass.multiply(com.oddlabs.tt.procedural.Landscape.VIKING_CLIFF_GRASS_TINT.x(), com.oddlabs.tt.procedural.Landscape.VIKING_CLIFF_GRASS_TINT.y(), com.oddlabs.tt.procedural.Landscape.VIKING_CLIFF_GRASS_TINT.z()), noise8.rotate(90).multiply(0.75f));
            cliff.bump(cliff_bump, (float)(size / 192.0f), 0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 0.0f);
            cliff.gamma2().multiply(0.9f);
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                cliff.saveAsPNG("structure_cliff");
            }
            com.oddlabs.procedural.Channel zero = new com.oddlabs.procedural.Channel(size, size).fill(0.0f);
            return new com.oddlabs.tt.procedural.Landscape.StructureLayers(cliff, cliff_bump.toNormalMap(3.0f, zero));
        }
        // Java: 535
        public static com.oddlabs.tt.procedural.Landscape.StructureLayers genSnow(int size, com.oddlabs.procedural.Channel noise8, com.oddlabs.procedural.Channel noise256)
        {
            com.oddlabs.procedural.Channel empty = new com.oddlabs.procedural.Channel(size, size).fill(1.0f);
            com.oddlabs.procedural.Channel snow_bump1 = noise8.brightness(0.75f);
            com.oddlabs.procedural.Channel snow_bump2 = noise256.brightness(0.25f);
            com.oddlabs.procedural.Layer snow = new com.oddlabs.procedural.Layer(empty.copy().fill(com.oddlabs.tt.procedural.Landscape.VIKING_SNOW_COLOR.x()), empty.copy().fill(com.oddlabs.tt.procedural.Landscape.VIKING_SNOW_COLOR.y()), empty.copy().fill(com.oddlabs.tt.procedural.Landscape.VIKING_SNOW_COLOR.z()));
            com.oddlabs.procedural.Channel bump = snow_bump1.channelAdd(snow_bump2);
            snow.bump(bump, (float)(size / 1024.0f), 0.0f, 0.1f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 0.0f);
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                snow.saveAsPNG("structure_snow");
            }
            com.oddlabs.procedural.Channel snowSpecular = snow_bump1.copy().multiply(0.2f);
            return new com.oddlabs.tt.procedural.Landscape.StructureLayers(snow, bump.toNormalMap(0.5f, snowSpecular));
        }
        // Java: 549
        public static com.oddlabs.tt.procedural.Landscape.StructureLayers genBlack()
        {
            return new com.oddlabs.tt.procedural.Landscape.StructureLayers(new com.oddlabs.procedural.Channel(1, 1).fill(0.0f).toLayer(), com.oddlabs.tt.procedural.Landscape.getFlatNormal(1));
        }
        // Java: 553
        public static com.oddlabs.tt.procedural.Landscape.StructureLayers genSeabottom(com.oddlabs.tt.procedural.Landscape.TerrainType terrain, int size, com.oddlabs.procedural.Channel noise8, com.oddlabs.procedural.Channel noise256, com.oddlabs.procedural.Channel voronoi4, com.oddlabs.procedural.Channel voronoi8)
        {
            org.joml.Vector4fc color = ((terrain).ordinal() switch { 0 /*NATIVE*/ => (org.joml.Vector4fc)(com.oddlabs.tt.procedural.Landscape.NATIVE_SEA_BOTTOM_COLOR), 1 /*VIKING*/ => (org.joml.Vector4fc)(com.oddlabs.tt.procedural.Landscape.VIKING_SEA_BOTTOM_COLOR), _ => throw new System.InvalidOperationException("MatchException") });
            com.oddlabs.procedural.Layer seabottom = new com.oddlabs.procedural.Layer(new com.oddlabs.procedural.Channel(size, size).fill(color.x()), new com.oddlabs.procedural.Channel(size, size).fill(color.y()), new com.oddlabs.procedural.Channel(size, size).fill(color.z()));
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                seabottom.saveAsPNG("structure_seabottom");
            }
            com.oddlabs.procedural.Channel zeroSpecular = new com.oddlabs.procedural.Channel(size, size).fill(0.0f);
            com.oddlabs.procedural.Layer normalMapLayer = default;
            switch ((terrain).ordinal())
            {
                case 0 /*NATIVE*/:
                {
                    com.oddlabs.procedural.Channel seabottom_bump = noise8.copy().channelAdd(noise256.brightness(0.1f)).dynamicRange();
                    normalMapLayer = seabottom_bump.toNormalMap(0.3f, zeroSpecular);
                    break;
                }
                case 1 /*VIKING*/:
                {
                    com.oddlabs.procedural.Channel rock_texture = voronoi4.copy().dynamicRange(0.2f, 1.0f).multiply(0.7f);
                    com.oddlabs.procedural.Channel slime_undulations = noise256.copy().brightness(0.08f);
                    com.oddlabs.procedural.Channel viking_seabottom_bump = rock_texture.channelAdd(slime_undulations).dynamicRange();
                    com.oddlabs.procedural.Channel wetSpecular = voronoi4.copy().dynamicRange(0.0f, 0.15f);
                    normalMapLayer = viking_seabottom_bump.toNormalMap(1.2f, wetSpecular);
                    break;
                }
                default:
                {
                    normalMapLayer = com.oddlabs.tt.procedural.Landscape.getFlatNormal(size);
                    break;
                }
            }
            return new com.oddlabs.tt.procedural.Landscape.StructureLayers(seabottom, normalMapLayer);
        }
        // Java: 592
        public static com.oddlabs.tt.procedural.Landscape.StructureLayers genDetail(int size, float detail_alpha_value, int seed, com.oddlabs.procedural.Channel noise8)
        {
            com.oddlabs.procedural.Channel detail_noise = new com.oddlabs.tt.procedural.Midpoint(size, 4, 0.4f, seed).toChannel();
            detail_noise.perturb(noise8.scale(size, size), 0.05f);
            com.oddlabs.procedural.Channel detail_grey = new com.oddlabs.procedural.Channel(size, size).fill(com.oddlabs.tt.procedural.Landscape.DETAIL_GREY);
            detail_grey.bump(detail_noise, (float)(size / 64.0f), 0.0f, 0.0f, 1.0f, 0.0f).dynamicRange();
            com.oddlabs.procedural.Channel detail_alpha = new com.oddlabs.procedural.Channel(size, size).fill(detail_alpha_value);
            com.oddlabs.procedural.Layer detail = new com.oddlabs.procedural.Layer(detail_grey, detail_grey, detail_grey, detail_alpha);
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                detail.saveAsPNG("structure_detail");
            }
            return new com.oddlabs.tt.procedural.Landscape.StructureLayers(detail, com.oddlabs.tt.procedural.Landscape.getFlatNormal(size));
        }
        // Java: 604
        public static com.oddlabs.procedural.Layer getFlatNormal(int size)
        {
            return new com.oddlabs.procedural.Layer(new com.oddlabs.procedural.Channel(size, size).fill(0.5f), new com.oddlabs.procedural.Channel(size, size).fill(0.5f), new com.oddlabs.procedural.Channel(size, size).fill(1.0f), new com.oddlabs.procedural.Channel(size, size).fill(0.0f));
        }
        // Java: 617
        public void generateTerrainNative()
        {
            alpha_maps = new com.oddlabs.tt.resource.GLByteImage[8];
            height = new com.oddlabs.tt.procedural.Mountain(unit_grids_per_world, com.oddlabs.util.Utils.powerOf2Log2(unit_grids_per_world) - 6, 0.5f, seed).toChannel().multiply(0.67f);
            com.oddlabs.tt.procedural.Voronoi voronoi = new com.oddlabs.tt.procedural.Voronoi(unit_grids_per_world, features, features, 1, 1.0f, seed);
            com.oddlabs.procedural.Channel cliffs = voronoi.getDistance((float)(-1.0f), 1.0f, 0.0f).brightness(1.5f).multiply(0.33f);
            height.channelAdd(cliffs);
            if (unit_grids_per_world > 128)
            {
                height.channelSubtract(voronoi.getDistance(1.0f, 0.0f, 0.0f).gamma(0.5f).flipV().rotate(90));
            }
            else
            {
                height.channelSubtract(voronoi.getDistance((float)(-1.0f), 1.0f, 0.0f).gamma(0.5f).flipV().rotate(90));
            }
            height.perturb(new com.oddlabs.tt.procedural.Midpoint(unit_grids_per_world, 2, 0.5f, seed).toChannel(), 0.25f);
            com.oddlabs.procedural.Channel shape = new com.oddlabs.tt.procedural.Hill(unit_grids_per_world, com.oddlabs.tt.procedural.Hill.OVAL).toChannel();
            height.channelAdd(shape.copy().multiply(0.15f));
            height.channelSubtract(shape.copy().invert().multiply(0.5f));
            if (archipelago)
            {
                com.oddlabs.procedural.Channel sub_islands = new com.oddlabs.tt.procedural.Voronoi(unit_grids_per_world, features, features, 1, 1.0f, seed).getDistance((float)(-1.0f), 0.0f, 0.0f).gamma(0.11f);
                height.channelMultiply(sub_islands);
                if (com.oddlabs.tt.procedural.Landscape.DEBUG)
                {
                    sub_islands.toLayer().saveAsPNG("sub_islands");
                }
            }
            height.erode((float)(((float)(24.0f - (float)(hills * 12.0f))) / unit_grids_per_world), unit_grids_per_world >> 2);
            height.channelMultiply(shape.gamma2());
            height.smooth(1);
            height = com.oddlabs.tt.procedural.Landscape.beaches(height);
            for (int y = 0; y < unit_grids_per_world; y += (unit_grids_per_world - 1))
            {
                for (int x = 0; x < unit_grids_per_world; x++)
                {
                    height.putPixel(x, y, 0.0f);
                }
            }
            for (int y_2 = 1; y_2 < unit_grids_per_world - 1; y_2++)
            {
                for (int x_2 = 0; x_2 < unit_grids_per_world; x_2 += (unit_grids_per_world - 1))
                {
                    height.putPixel(x_2, y_2, 0.0f);
                }
            }
            generateWaterGrid();
            slope = height.copy().lineart();
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                slope.copy().dynamicRange().toLayer().saveAsPNG("slope");
            }
            relheight = height.copy().relativeIntensityNormalized(java.lang.Math.max(1, unit_grids_per_world >> 5));
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                relheight.toLayer().saveAsPNG("relheight");
            }
            if (archipelago)
            {
                com.oddlabs.procedural.Channel inv = water_map.copy().invert();
                access = generateThresholdMap(slope, access_threshold).channelMultiply(inv);
            }
            else
            {
                access = generateThresholdMap(slope, access_threshold).largestConnected(1.0f);
            }
            access_exported = access.copy();
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                access.toLayer().saveAsPNG("access");
            }
            build = com.oddlabs.tt.procedural.Landscape.generateBuildMap(generateThresholdMap(slope, build_threshold).channelMultiply(access));
        }
        // Java: 680
        public void generateTerrainViking()
        {
            alpha_maps = new com.oddlabs.tt.resource.GLByteImage[8];
            height = new com.oddlabs.tt.procedural.Mountain(unit_grids_per_world, com.oddlabs.util.Utils.powerOf2Log2(unit_grids_per_world) - 6, 0.5f, seed).toChannel().add(0.5f).dynamicRange().gamma2().multiply(0.67f);
            com.oddlabs.tt.procedural.Voronoi voronoi = new com.oddlabs.tt.procedural.Voronoi(unit_grids_per_world, 8, 8, 1, 1.0f, seed, true);
            com.oddlabs.procedural.Channel cliffs = voronoi.getDistance((float)(-1.0f), 1.0f, 0.0f).brightness(1.25f).multiply(0.33f);
            height.channelAdd(cliffs).dynamicRange();
            com.oddlabs.tt.procedural.Voronoi voronoi2 = new com.oddlabs.tt.procedural.Voronoi(unit_grids_per_world, 4, 4, 1, 1.0f, seed);
            if (unit_grids_per_world > 128)
            {
                height.channelSubtract(voronoi2.getDistance(1.0f, 0.0f, 0.0f).gamma(0.5f).multiply(0.5f));
            }
            else
            {
                height.channelSubtract(voronoi2.getDistance((float)(-1.0f), 1.0f, 0.0f).gamma(0.5f).multiply(0.5f));
            }
            com.oddlabs.procedural.Channel hitpoint = voronoi.getHitpoint().smooth(1);
            com.oddlabs.procedural.Channel hitpoint2 = hitpoint.copy().erodeThermal((float)(4.0f / unit_grids_per_world), unit_grids_per_world >> 3);
            com.oddlabs.procedural.Channel noise = new com.oddlabs.tt.procedural.Midpoint(unit_grids_per_world, 3, 0.25f, seed).toChannel().threshold((float)(0.75f * hills), 1.0f);
            com.oddlabs.procedural.Channel heightcut = hitpoint.channelMultiply(noise.copy().invert()).channelAdd(hitpoint2.copy().channelMultiply(noise));
            height.channelMultiply(heightcut);
            height.perturb(new com.oddlabs.tt.procedural.Midpoint(unit_grids_per_world, 2, 0.5f, seed).toChannel(), 0.25f);
            if (archipelago)
            {
                com.oddlabs.procedural.Channel sub_islands = new com.oddlabs.tt.procedural.Voronoi(unit_grids_per_world, features, features, 1, 1.0f, seed).getDistance((float)(-1.0f), 0.0f, 0.0f).gamma(0.11f);
                height.channelMultiply(sub_islands);
                if (com.oddlabs.tt.procedural.Landscape.DEBUG)
                {
                    sub_islands.toLayer().saveAsPNG("sub_islands");
                }
            }
            height.erode((float)(((float)(24.0f - (float)(hills * 12.0f))) / unit_grids_per_world), unit_grids_per_world >> 2);
            com.oddlabs.procedural.Channel shape = new com.oddlabs.tt.procedural.Hill(unit_grids_per_world, com.oddlabs.tt.procedural.Hill.SQUARE).toChannel().smoothGain().gamma8();
            height.channelMultiply(shape);
            slope = height.copy().lineart();
            com.oddlabs.procedural.Channel peakarea = slope.threshold(0.0f, access_threshold).largestConnected(1.0f).invert().channelMultiply(hitpoint2);
            com.oddlabs.procedural.Channel peaks = new com.oddlabs.tt.procedural.Midpoint(unit_grids_per_world, 4, 0.75f, 42).toChannel().channelMultiply(peakarea).multiply(0.1f);
            height.channelAdd(peaks);
            height.smooth(1);
            for (int y = 0; y < unit_grids_per_world; y += (unit_grids_per_world - 1))
            {
                for (int x = 0; x < unit_grids_per_world; x++)
                {
                    height.putPixel(x, y, 0.0f);
                }
            }
            for (int y_2 = 1; y_2 < unit_grids_per_world - 1; y_2++)
            {
                for (int x_2 = 0; x_2 < unit_grids_per_world; x_2 += (unit_grids_per_world - 1))
                {
                    height.putPixel(x_2, y_2, 0.0f);
                }
            }
            generateWaterGrid();
            slope = height.copy().lineart();
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                slope.copy().dynamicRange().toLayer().saveAsPNG("slope");
            }
            relheight = height.copy().relativeIntensityNormalized(java.lang.Math.max(1, unit_grids_per_world >> 5));
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                relheight.toLayer().saveAsPNG("relheight");
            }
            if (archipelago)
            {
                com.oddlabs.procedural.Channel inv = water_map.copy().invert();
                access = generateThresholdMap(slope, access_threshold).channelMultiply(inv);
            }
            else
            {
                access = generateThresholdMap(slope, access_threshold).largestConnected(1.0f);
            }
            access_exported = access.copy();
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                access.toLayer().saveAsPNG("access");
            }
            build = com.oddlabs.tt.procedural.Landscape.generateBuildMap(generateThresholdMap(slope, build_threshold).channelMultiply(access));
        }
        // Java: 758
        public static com.oddlabs.procedural.Channel beaches(com.oddlabs.procedural.Channel channel)
        {
            float sealevel = (float)(1.1f * com.oddlabs.tt.global.Globals.SEA_LEVEL);
            float threshold = (float)(2.0f * sealevel);
            for (int y = 0; y < channel.height; y++)
            {
                for (int x = 0; x < channel.width; x++)
                {
                    float @value = channel.getPixel(x, y);
                    if (@value < sealevel)
                    {
                        @value = com.oddlabs.procedural.Tools.interpolateSmooth(0, sealevel, (float)(@value / sealevel));
                        channel.putPixel(x, y, @value);
                    }
                    else if (@value < threshold)
                    {
                        @value = com.oddlabs.procedural.Tools.interpolateSmooth(sealevel, (float)((float)(2.0f * threshold) - sealevel), (float)((float)(0.5f * ((float)(@value - sealevel))) / ((float)(threshold - sealevel))));
                        channel.putPixel(x, y, @value);
                    }
                }
            }
            return channel;
        }
        // Java: 778
        public com.oddlabs.procedural.Channel generateThresholdMap(com.oddlabs.procedural.Channel slopemap, float threshold)
        {
            com.oddlabs.procedural.Channel channel = slopemap.copy().threshold(0.0f, threshold).channelSubtract(height.copy().threshold(0.0f, com.oddlabs.tt.global.Globals.SEA_LEVEL));
            for (int y = 0; y < unit_grids_per_world; y += (unit_grids_per_world - 1))
            {
                for (int x = 0; x < unit_grids_per_world; x++)
                {
                    channel.putPixel(x, y, 0.0f);
                }
            }
            for (int y_2 = 1; y_2 < unit_grids_per_world - 1; y_2++)
            {
                for (int x_2 = 0; x_2 < unit_grids_per_world; x_2 += (unit_grids_per_world - 1))
                {
                    channel.putPixel(x_2, y_2, 0.0f);
                }
            }
            return channel;
        }
        // Java: 796
        public static sbyte[][] generateBuildMap(com.oddlabs.procedural.Channel thresholdmap)
        {
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                thresholdmap.toLayer().saveAsPNG("build_tresholdmap");
            }
            int size = thresholdmap.getWidth();
            bool[][] build_grid = java.lang.JA.newArray2<bool>(size, size);
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    build_grid[y][x] = thresholdmap.getPixel(x, y) > 0;
                }
            }
            sbyte[][] byte_grid = java.lang.JA.newArray2<sbyte>(build_grid.Length, build_grid[0].Length);
            sbyte max = unchecked((sbyte)(com.oddlabs.tt.model.RacesResources.MAX_BUILDING_SIZE));
            for (sbyte i = unchecked((sbyte)(0)); i < max; i++)
            {
                for (int y_2 = 1; y_2 < build_grid.Length - 1; y_2++)
                {
                    for (int x_2 = 1; x_2 < build_grid[y_2].Length - 1; x_2++)
                    {
                        if (!build_grid[y_2][x_2] && byte_grid[y_2][x_2] == i)
                        {
                            for (int k = (-1); k <= 1; k++)
                            {
                                for (int l = (-1); l <= 1; l++)
                                {
                                    if (build_grid[y_2 + k][x_2 + l])
                                    {
                                        build_grid[y_2 + k][x_2 + l] = false;
                                        byte_grid[y_2 + k][x_2 + l] = unchecked((sbyte)((i + 1)));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            for (int y_3 = 1; y_3 < build_grid.Length - 1; y_3++)
            {
                for (int x_3 = 1; x_3 < build_grid[y_3].Length - 1; x_3++)
                {
                    if (build_grid[y_3][x_3])
                    {
                        byte_grid[y_3][x_3] = max;
                    }
                }
            }
            return byte_grid;
        }
        // Java: 836
        public com.oddlabs.procedural.Channel generateAlphas()
        {
            int seed = com.oddlabs.tt.global.Globals.LANDSCAPE_SEED;
            com.oddlabs.procedural.Channel alpha0 = default;
            com.oddlabs.procedural.Channel alpha1 = default;
            com.oddlabs.procedural.Channel alpha2 = default;
            com.oddlabs.procedural.Channel alpha3 = default;
            com.oddlabs.procedural.Channel grass_alpha = ((System.Func<com.oddlabs.procedural.Channel>)(() =>
            {
                switch ((terrain).ordinal())
                {
                    case 0 /*NATIVE*/:
                    {
                        alpha0 = generateDirtAlpha();
                        if (com.oddlabs.tt.procedural.Landscape.DEBUG)
                        {
                            alpha0.toLayer().saveAsPNG("alpha_dirt");
                        }
                        alpha1 = generateRubbleAlpha();
                        if (com.oddlabs.tt.procedural.Landscape.DEBUG)
                        {
                            alpha1.toLayer().saveAsPNG("alpha_rubble");
                        }
                        alpha2 = com.oddlabs.tt.procedural.Landscape.generateRockAlpha(slope, access_threshold);
                        if (com.oddlabs.tt.procedural.Landscape.DEBUG)
                        {
                            alpha2.toLayer().saveAsPNG("alpha_rock");
                        }
                        alpha3 = generateGrassAlpha(unit_grids_per_world, seed);
                        if (com.oddlabs.tt.procedural.Landscape.DEBUG)
                        {
                            alpha3.toLayer().saveAsPNG("alpha_grass");
                        }
                        alpha_maps[0] = new com.oddlabs.tt.resource.GLByteImage(alpha0, org.lwjgl.opengl.GL11.GL_RED);
                        alpha_maps[1] = new com.oddlabs.tt.resource.GLByteImage(alpha1, org.lwjgl.opengl.GL11.GL_RED);
                        alpha_maps[2] = new com.oddlabs.tt.resource.GLByteImage(alpha2, org.lwjgl.opengl.GL11.GL_RED);
                        alpha_maps[3] = new com.oddlabs.tt.resource.GLByteImage(alpha3, org.lwjgl.opengl.GL11.GL_RED);
                        ground = groundGrid(new com.oddlabs.tt.procedural.Landscape.Ground[] { com.oddlabs.tt.procedural.Landscape.Ground.DIRT, com.oddlabs.tt.procedural.Landscape.Ground.ROCK, com.oddlabs.tt.procedural.Landscape.Ground.ROCK, com.oddlabs.tt.procedural.Landscape.Ground.GRASS }, alpha0, alpha1, alpha2, alpha3);
                        return alpha3;
                    }
                    case 1 /*VIKING*/:
                    {
                        alpha0 = generateSoilAlpha();
                        if (com.oddlabs.tt.procedural.Landscape.DEBUG)
                        {
                            alpha0.toLayer().saveAsPNG("alpha_soil");
                        }
                        alpha1 = com.oddlabs.tt.procedural.Landscape.generateCliffAlpha(slope, access_threshold);
                        if (com.oddlabs.tt.procedural.Landscape.DEBUG)
                        {
                            alpha1.toLayer().saveAsPNG("alpha_cliff");
                        }
                        alpha2 = generateGrassAlpha(unit_grids_per_world, seed);
                        if (com.oddlabs.tt.procedural.Landscape.DEBUG)
                        {
                            alpha2.toLayer().saveAsPNG("alpha_grass");
                        }
                        alpha3 = com.oddlabs.tt.procedural.Landscape.generateSnowAlpha(height, alpha1.copy());
                        if (com.oddlabs.tt.procedural.Landscape.DEBUG)
                        {
                            alpha3.toLayer().saveAsPNG("alpha_snow");
                        }
                        alpha_maps[0] = new com.oddlabs.tt.resource.GLByteImage(alpha0, org.lwjgl.opengl.GL11.GL_RED);
                        alpha_maps[1] = new com.oddlabs.tt.resource.GLByteImage(alpha1, org.lwjgl.opengl.GL11.GL_RED);
                        alpha_maps[2] = new com.oddlabs.tt.resource.GLByteImage(alpha2, org.lwjgl.opengl.GL11.GL_RED);
                        alpha_maps[3] = new com.oddlabs.tt.resource.GLByteImage(alpha3, org.lwjgl.opengl.GL11.GL_RED);
                        ground = groundGrid(new com.oddlabs.tt.procedural.Landscape.Ground[] { com.oddlabs.tt.procedural.Landscape.Ground.DIRT, com.oddlabs.tt.procedural.Landscape.Ground.ROCK, com.oddlabs.tt.procedural.Landscape.Ground.GRASS, com.oddlabs.tt.procedural.Landscape.Ground.SNOW }, alpha0, alpha1, alpha2, alpha3);
                        return alpha2;
                    }
                }
                throw new System.InvalidOperationException("MatchException");
            }))();
            com.oddlabs.procedural.Channel seabottom_alpha = com.oddlabs.tt.procedural.Landscape.generateSeabottomAlpha(terrain, height, height_scale);
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                seabottom_alpha.toLayer().saveAsPNG("alpha_seabottom");
            }
            shadow = new com.oddlabs.procedural.Channel(unit_grids_per_world, unit_grids_per_world);
            supply_shadow = new com.oddlabs.procedural.Channel(unit_grids_per_world, unit_grids_per_world);
            highlight = new com.oddlabs.procedural.Channel(unit_grids_per_world, unit_grids_per_world);
            org.joml.Vector3f light_direction = com.oddlabs.tt.global.Globals.REAL_SKY_BODY_DIRECTION;
            float lx = light_direction.x_f;
            float ly = light_direction.y_f;
            float lz = light_direction.z_f;
            float threshold = lz;
            float nz = (float)((float)(2.0f * meters_per_height_unit) / height_scale);
            float nzlz = (float)(nz * lz);
            float nz2 = (float)(nz * nz);
            for (int y = 0; y < unit_grids_per_world; y++)
            {
                for (int x = 0; x < unit_grids_per_world; x++)
                {
                    float nx = (float)(height.getPixelWrap(x + 1, y) - height.getPixelWrap(x - 1, y));
                    float ny = (float)(height.getPixelWrap(x, y + 1) - height.getPixelWrap(x, y - 1));
                    float light = (float)(((float)((float)((float)((float)(-nx) * lx) - (float)(ny * ly)) + nzlz)) / (((float)(java.lang.Math.sqrt((float)((float)((float)(nx * nx) + (float)(ny * ny)) + nz2))))));
                    if (light > threshold)
                    {
                        highlight.putPixel(x, y, light);
                        shadow.putPixel(x, y, threshold);
                    }
                    else
                    {
                        highlight.putPixel(x, y, threshold);
                        shadow.putPixel(x, y, java.lang.Math.max(0, light));
                    }
                }
            }
            highlight.dynamicRange(0.0f, 0.25f);
            shadow.invert().dynamicRange(0.0f, 0.75f);
            com.oddlabs.tt.form.ProgressForm.progress((float)(1 / 14.0f));
            com.oddlabs.procedural.Channel shadowcast = generateShadowcast(light_direction);
            shadow.channelBrightest(shadowcast.smooth(1).brightness(0.67f));
            float shading_band = (float)(com.oddlabs.tt.procedural.Landscape.SEABOTTOM_DEPTH_OFFSET_METERS / height_scale);
            for (int y_2 = 0; y_2 < unit_grids_per_world; y_2++)
            {
                for (int x_2 = 0; x_2 < unit_grids_per_world; x_2++)
                {
                    float h = height.getPixel(x_2, y_2);
                    if (h < com.oddlabs.tt.global.Globals.SEA_LEVEL)
                    {
                        float fade = java.lang.Math.clamp((float)(((float)(h - ((float)(com.oddlabs.tt.global.Globals.SEA_LEVEL - shading_band)))) / shading_band), 0.0f, 1.0f);
                        float factor = (float)(com.oddlabs.tt.procedural.Landscape.UNDERWATER_SHADING + (float)(((float)(1.0f - com.oddlabs.tt.procedural.Landscape.UNDERWATER_SHADING)) * fade));
                        shadow.putPixel(x_2, y_2, (float)(shadow.getPixel(x_2, y_2) * factor));
                        highlight.putPixel(x_2, y_2, (float)(highlight.getPixel(x_2, y_2) * factor));
                    }
                }
            }
            shadow.channelMultiply(seabottom_alpha.copy().invert());
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                shadow.toLayer().saveAsPNG("alpha_shadow");
            }
            com.oddlabs.tt.form.ProgressForm.progress((float)(1 / 14.0f));
            alpha_maps[6] = new com.oddlabs.tt.resource.GLByteImage(seabottom_alpha, org.lwjgl.opengl.GL11.GL_RED);
            return grass_alpha;
        }
        // Java: 938
        public sbyte[][] groundGrid(com.oddlabs.tt.procedural.Landscape.Ground[] layer_grounds, params com.oddlabs.procedural.Channel[] alphas)
        {
            sbyte[][] grid = java.lang.JA.newArray2<sbyte>(unit_grids_per_world, unit_grids_per_world);
            for (int y = 0; y < unit_grids_per_world; y++)
            {
                for (int x = 0; x < unit_grids_per_world; x++)
                {
                    com.oddlabs.tt.procedural.Landscape.Ground cell = com.oddlabs.tt.procedural.Landscape.Ground.BEACH;
                    for (int i = 0; i < alphas.Length; i++)
                    {
                        if (alphas[i].getPixel(x, y) >= com.oddlabs.tt.procedural.Landscape.VISIBLE_GROUND_ALPHA)
                        {
                            cell = layer_grounds[i];
                        }
                    }
                    grid[y][x] = unchecked((sbyte)(cell.ordinal()));
                }
            }
            return grid;
        }
        // Java: 955
        public com.oddlabs.procedural.Channel generateDirtAlpha()
        {
            com.oddlabs.procedural.Channel dirt_alpha = height.copy().dynamicRange((float)(1.1f * com.oddlabs.tt.global.Globals.SEA_LEVEL), (float)(2.0f * com.oddlabs.tt.global.Globals.SEA_LEVEL), 0.0f, 1.0f);
            dirt_alpha.channelSubtract(relheight.copy().invert().dynamicRange(0.5f, 0.6f, 0.0f, 0.5f));
            return dirt_alpha;
        }
        // Java: 962
        public com.oddlabs.procedural.Channel generateRubbleAlpha()
        {
            com.oddlabs.procedural.Channel rubble_alpha = slope.copy().dynamicRange(build_threshold, access_threshold, 0.0f, 1.0f);
            rubble_alpha.channelSubtract(height.copy().invert().dynamicRange(0.8f, 1.0f, 0.0f, 1.0f));
            rubble_alpha.channelSubtract(relheight.copy().invert().dynamicRange(0.5f, 0.65f, 0.0f, 0.5f));
            return rubble_alpha;
        }
        // Java: 970
        public static com.oddlabs.procedural.Channel generateRockAlpha(com.oddlabs.procedural.Channel slope, float access_threshold)
        {
            com.oddlabs.procedural.Channel rock_alpha = slope.copy().threshold(access_threshold, 1.0f);
            return rock_alpha;
        }
        // Java: 976
        public com.oddlabs.procedural.Channel generateGrassAlpha(int size, int seed)
        {
            com.oddlabs.procedural.Channel grass_alpha = new com.oddlabs.tt.procedural.Midpoint(size, 4, 0.45f, seed).toChannel().dynamicRange((float)(1.0f - vegetation_amount), 1.0f, 0.0f, 1.0f).gamma2();
            grass_alpha.channelBrightest(slope.copy().dynamicRange(0.0f, access_threshold, 0.0f, 1.0f).invert().dynamicRange((float)(1.0f - vegetation_amount), 1.0f, 0.0f, 1.0f).gamma2());
            grass_alpha.channelAdd(relheight.copy().invert().add((float)(-0.5f)).multiply(2.0f));
            grass_alpha.channelSubtract(height.copy().invert().dynamicRange(0.6f, 0.8f, 0.0f, 1.0f));
            grass_alpha.channelSubtract(slope.copy().threshold((float)(0.75f * access_threshold), 1.0f).smooth(3));
            grass_alpha.channelSubtract(relheight.copy().invert().dynamicRange(0.5f, 0.7f, 0.0f, 0.5f));
            return grass_alpha;
        }
        // Java: 989
        public com.oddlabs.procedural.Channel generateSoilAlpha()
        {
            com.oddlabs.procedural.Channel soil_alpha = height.copy().dynamicRange((float)(1.1f * com.oddlabs.tt.global.Globals.SEA_LEVEL), (float)(2.0f * com.oddlabs.tt.global.Globals.SEA_LEVEL), 0.0f, 1.0f);
            soil_alpha.channelSubtract(relheight.copy().invert().dynamicRange(0.5f, 0.6f, 0.0f, 0.5f));
            return soil_alpha;
        }
        // Java: 996
        public static com.oddlabs.procedural.Channel generateCliffAlpha(com.oddlabs.procedural.Channel slope, float access_threshold)
        {
            com.oddlabs.procedural.Channel cliff_alpha = slope.copy().threshold(access_threshold, 1.0f);
            return cliff_alpha;
        }
        // Java: 1002
        public static com.oddlabs.procedural.Channel generateSnowAlpha(com.oddlabs.procedural.Channel height, com.oddlabs.procedural.Channel cliff_alpha)
        {
            com.oddlabs.procedural.Channel snow_alpha = height.copy().dynamicRange(0.5f, 0.6f, 0.0f, 1.0f);
            snow_alpha.channelSubtract(cliff_alpha);
            snow_alpha.smooth(1).smooth(1);
            return snow_alpha;
        }
        // Java: 1011
        public static com.oddlabs.procedural.Channel generateSeabottomAlpha(com.oddlabs.tt.procedural.Landscape.TerrainType terrain, com.oddlabs.procedural.Channel height, int height_scale)
        {
            float depth_offset = (float)(com.oddlabs.tt.procedural.Landscape.SEABOTTOM_DEPTH_OFFSET_METERS / height_scale);
            com.oddlabs.procedural.Channel seabottom_alpha = height.copy().invert().dynamicRange((float)((float)(1.0f - com.oddlabs.tt.global.Globals.SEA_LEVEL) + depth_offset), 1.0f, 0.0f, 1.0f);
            return ((terrain).ordinal() switch { 0 /*NATIVE*/ => (com.oddlabs.procedural.Channel)(seabottom_alpha.gamma(0.5f)), 1 /*VIKING*/ => (com.oddlabs.procedural.Channel)(seabottom_alpha.gamma(0.5f)), _ => throw new System.InvalidOperationException("MatchException") });
        }
        // Java: 1027
        public void generateSupplies(com.oddlabs.procedural.Channel grass_alpha)
        {
            com.oddlabs.procedural.Channel centerprob = new com.oddlabs.tt.procedural.Hill(unit_grids_per_world, com.oddlabs.tt.procedural.Hill.CIRCLE).toChannel().addClip((float)(-0.5f)).dynamicRange();
            com.oddlabs.procedural.Channel noise = new com.oddlabs.tt.procedural.Midpoint(unit_grids_per_world, com.oddlabs.util.Utils.powerOf2Log2(unit_grids_per_world) - 3, 0.33f, seed).toChannel();
            com.oddlabs.procedural.Channel tree_channel = grass_alpha.copy();
            com.oddlabs.procedural.Channel palmtree_channel = height.copy();
            com.oddlabs.tt.form.ProgressForm.progress((float)(1 / 14.0f));
            switch ((terrain).ordinal())
            {
                case 0 /*NATIVE*/:
                {
                    tree_channel.threshold(0.5f, 1.0f);
                    tree_channel.channelAdd(noise.rotate(90).copy().threshold(0.9f, 1.0f));
                    tree_channel.channelSubtract(slope.copy().threshold(access_threshold, 1.0f));
                    tree_channel.channelSubtract(noise.copy().dynamicRange(0.1f, 1.0f));
                    palmtree_channel.invert().dynamicRange(0.6f, 0.89f, 0.0f, 1.0f);
                    palmtree_channel.channelSubtract(height.copy().invert().dynamicRange(0.89f, 0.9f, 0.0f, 1.0f));
                    palmtree_channel.channelSubtract(noise.rotate(90)).channelAdd(noise.rotate(90).copy().threshold(0.9f, 1.0f)).channelSubtract(slope.copy().threshold(access_threshold, 1.0f));
                    break;
                }
                case 1 /*VIKING*/:
                {
                    tree_channel.gamma8();
                    tree_channel.channelMultiply(height.copy().dynamicRange(0.55f, 0.65f, 1.0f, 0.0f));
                    tree_channel.channelSubtract(slope.copy().threshold(access_threshold, 1.0f));
                    tree_channel.channelSubtract(noise.copy());
                    palmtree_channel = grass_alpha.copy().channelMultiply(height.copy().dynamicRange(0.5f, 0.6f, 1.0f, 0.0f)).invert();
                    palmtree_channel.channelSubtract(height.copy().invert().dynamicRange(0.8f, 0.875f, 0.0f, 1.0f));
                    palmtree_channel.channelSubtract(slope.copy().threshold(access_threshold, 1.0f));
                    palmtree_channel.channelSubtract(noise.rotate(90));
                    break;
                }
            }
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                tree_channel.toLayer().saveAsPNG("supplies_trees");
            }
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                palmtree_channel.toLayer().saveAsPNG("supplies_palmtrees");
            }
            com.oddlabs.tt.form.ProgressForm.progress((float)(1 / 14.0f));
            com.oddlabs.procedural.Channel rock_channel = relheight.copy();
            switch ((terrain).ordinal())
            {
                case 0 /*NATIVE*/:
                {
                    rock_channel.invert().threshold(0.5f, 1.0f).channelMultiply(noise.rotate(90).copy().gamma8().invert()).channelMultiply(centerprob);
                    break;
                }
                case 1 /*VIKING*/:
                {
                    rock_channel.threshold(0.0f, 0.5f).channelMultiply(noise.rotate(90).copy().gamma8().invert());
                    break;
                }
            }
            com.oddlabs.procedural.Channel iron_channel = rock_channel.copy().rotate(90).flipV();
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                rock_channel.toLayer().saveAsPNG("supplies_rocks");
            }
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                iron_channel.toLayer().saveAsPNG("supplies_iron");
            }
            com.oddlabs.procedural.Channel supplies = access.copy();
            float accessible = supplies.sum();
            trees = placeSupplies(tree_channel, supplies, 64, java.lang.JC.f2i(((float)((float)(vegetation_amount * max_trees) * ((float)(accessible / area))))), 0.33f);
            access.channelSubtract(trees);
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                trees.toLayer().saveAsPNG("supplies_trees_placed");
            }
            palmtrees = placeSupplies(palmtree_channel, supplies, 64, java.lang.JC.f2i(((float)((float)(vegetation_amount * max_palmtrees) * ((float)(accessible / area))))), 0.25f);
            access.channelSubtract(palmtrees);
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                palmtrees.toLayer().saveAsPNG("supplies_palmtrees_placed");
            }
            rock = placeSupplies(rock_channel, supplies, 64, java.lang.JC.f2i(((float)(supplies_amount * max_rock))), 0.0f);
            access.channelSubtract(rock);
            supply_shadow.channelBrightest(rock.copy().multiply(0.5f));
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                rock.toLayer().saveAsPNG("supplies_rock_placed");
            }
            iron = placeSupplies(iron_channel, supplies, 64, java.lang.JC.f2i(((float)(supplies_amount * max_iron))), 0.0f);
            access.channelSubtract(iron);
            supply_shadow.channelBrightest(iron.copy().multiply(0.5f));
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                iron.toLayer().saveAsPNG("supplies_iron_placed");
            }
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                java.lang.IO.println("Number of trees placed: " + (trees.count(1.0f)));
                java.lang.IO.println("Number of palmtrees placed: " + (palmtrees.count(1.0f)));
                java.lang.IO.println("Number of rocks placed: " + (rock.count(1.0f)));
                java.lang.IO.println("Number of iron ore placed: " + (iron.count(1.0f)));
            }
            if (!archipelago)
            {
                int num_rock = 2;
                int num_iron = 1;
                for (int p = 0; p < num_players; p++)
                {
                    for (int r = 0; r < num_rock; r++)
                    {
                        int[] location = access.find((unit_grids_per_world >> 1), supply_locations[p][0], supply_locations[p][1], 1.0f);
                        rock.putPixel(location[0], location[1], 1.0f);
                        access.putPixel(location[0], location[1], 0.0f);
                    }
                    for (int i = 0; i < num_iron; i++)
                    {
                        int[] location_2 = access.find((unit_grids_per_world >> 1), supply_locations[p][0], supply_locations[p][1], 1.0f);
                        iron.putPixel(location_2[0], location_2[1], 1.0f);
                        access.putPixel(location_2[0], location_2[1], 0.0f);
                    }
                }
            }
            alpha_maps[4] = new com.oddlabs.tt.resource.GLByteImage(highlight, org.lwjgl.opengl.GL11.GL_RED);
            alpha_maps[5] = new com.oddlabs.tt.resource.GLByteImage(shadow, org.lwjgl.opengl.GL11.GL_RED);
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                supply_shadow.toLayer().saveAsPNG("alpha_supply_shadow");
            }
            alpha_maps[7] = new com.oddlabs.tt.resource.GLByteImage(supply_shadow, org.lwjgl.opengl.GL11.GL_RED);
            com.oddlabs.tt.form.ProgressForm.progress((float)(1 / 14.0f));
            plants = java.lang.JA.newArray2<float>(com.oddlabs.tt.procedural.Landscape.NUM_PLANT_TYPES, max_plants << 1);
            tree_channel.channelBrightest(palmtree_channel).brightness(0.5f);
            noise.scaleFast(noise.width >> 1, noise.height >> 1).tileDouble();
            com.oddlabs.procedural.Channel plantsmap = grass_alpha.copy();
            plantsmap.multiply(0.25f).add(0.75f);
            plantsmap.channelMultiply(slope.copy().threshold(0.0f, access_threshold));
            plantsmap.channelMultiply(height.copy().threshold(com.oddlabs.tt.global.Globals.SEA_LEVEL, 1.0f));
            com.oddlabs.procedural.Channel plants1 = plantsmap.copy().channelMultiply(noise.copy());
            com.oddlabs.procedural.Channel plants2 = plantsmap.copy().channelMultiply(noise.rotate(90));
            com.oddlabs.procedural.Channel plants3 = plantsmap.copy().channelMultiply(noise.rotate(90));
            com.oddlabs.procedural.Channel plants4 = plantsmap.copy().channelMultiply(noise.rotate(90));
            com.oddlabs.procedural.Channel place = new com.oddlabs.procedural.Channel(unit_grids_per_world, unit_grids_per_world);
            place = placePlants(plants1, place, 64, max_plants >> 2, 0);
            place = placePlants(plants2, place, 64, max_plants >> 2, 1);
            place = placePlants(plants3, place, 64, max_plants >> 2, 2);
            place = placePlants(plants4, place, 64, max_plants >> 2, 3);
        }
        // Java: 1162
        public com.oddlabs.procedural.Channel generateShadowcast(org.joml.Vector3f light_direction)
        {
            int size = unit_grids_per_world;
            com.oddlabs.procedural.Channel shadowcast = new com.oddlabs.procedural.Channel(size, size);
            float travel_x = (float)(-light_direction.x_f);
            float travel_y = (float)(-light_direction.y_f);
            bool along_x = java.lang.Math.abs(travel_x) >= java.lang.Math.abs(travel_y);
            float travel_main = (along_x ? travel_x : travel_y);
            if (java.lang.Math.abs(travel_main) < 1.0E-4f)
            {
                return shadowcast;
            }
            float side_step = (float)(((along_x ? travel_y : travel_x)) / java.lang.Math.abs(travel_main));
            float descent = (float)((float)((float)(light_direction.z_f / java.lang.Math.abs(travel_main)) * meters_per_height_unit) / height_scale);
            int extra = java.lang.JC.d2i(java.lang.Math.ceil((float)(java.lang.Math.abs(side_step) * size)));
            for (int start = -extra; start < size + extra; start++)
            {
                float peak = java.lang.Float.NEGATIVE_INFINITY;
                for (int i = 0; i < size; i++)
                {
                    int main = (travel_main > 0 ? i : size - 1 - i);
                    int side = java.lang.Math.round((float)(start + (float)(i * side_step)));
                    peak = (float)(peak - descent);
                    if (side < 0 || side >= size)
                    {
                        continue;
                    }
                    int x = (along_x ? main : side);
                    int y = (along_x ? side : main);
                    float val = height.getPixel(x, y);
                    if (peak > val)
                    {
                        shadowcast.putPixel(x, y, 1.0f);
                    }
                    else
                    {
                        peak = val;
                    }
                }
            }
            return shadowcast;
        }
        // Java: 1199
        public com.oddlabs.procedural.Channel placeSupplies(com.oddlabs.procedural.Channel probability, com.oddlabs.procedural.Channel supplies, int intervals, int max_count, float shadow_alpha_val)
        {
            max_count = java.lang.Math.min(probability.width * probability.height, max_count);
            int scaleshift = com.oddlabs.util.Utils.powerOf2Log2(unit_grids_per_world * meters_per_height_unit / meters_per_world);
            int i = 0;
            float interval_size = (float)(1.0f / intervals);
            float upper_bound = 1.0f;
            float lower_bound = (float)(upper_bound - interval_size);
            com.oddlabs.procedural.Channel supplyshadow_alpha = new com.oddlabs.procedural.Channel(supplyshadow_size << 1, supplyshadow_size << 1).place(new com.oddlabs.procedural.Channel(supplyshadow_size, supplyshadow_size).fill(1.0f), supplyshadow_size >> 1, supplyshadow_size >> 1).smoothFast();
            com.oddlabs.procedural.Channel supplyshadow_alpha2 = supplyshadow_alpha.copy().brightness(shadow_alpha_val);
            com.oddlabs.procedural.Channel place = new com.oddlabs.procedural.Channel(probability.width, probability.height);
            while (i < max_count && lower_bound > interval_size)
            {
                for (int y = 1; y < probability.height - 1; y++)
                {
                    for (int x = 1; x < probability.width - 1; x++)
                    {
                        float val = probability.getPixel(x, y);
                        if (val <= upper_bound && val > lower_bound)
                        {
                            if (supplies.getPixel(x, y) > 0 && supplies.getPixel(x - 1, y) > 0 && supplies.getPixel(x + 1, y) > 0 && supplies.getPixel(x, y - 1) > 0 && supplies.getPixel(x, y - 1) > 0)
                            {
                                place.putPixel(x, y, 1.0f);
                                if (shadow_alpha_val > 0.0f)
                                {
                                    int x_pixel = (x - supplyshadow_size + 1) >> scaleshift;
                                    int y_pixel = (y - supplyshadow_size + 1) >> scaleshift;
                                    supply_shadow.placeBrightest(supplyshadow_alpha2, x_pixel, y_pixel);
                                }
                                for (int k = (-1); k <= 1; k++)
                                {
                                    for (int l = (-1); l <= 1; l++)
                                    {
                                        supplies.putPixelWrap(x + k, y + l, 0.0f);
                                    }
                                }
                                i++;
                                if (i >= max_count)
                                {
                                    goto brk_out_4;
                                }
                            }
                        }
                    }
                }
                lower_bound = (float)(lower_bound - interval_size);
                upper_bound = (float)(upper_bound - interval_size);
            }
            brk_out_4: ;
            return place;
        }
        // Java: 1254
        public com.oddlabs.procedural.Channel placePlants(com.oddlabs.procedural.Channel probability, com.oddlabs.procedural.Channel place, int intervals, int max_count, int plant_type)
        {
            max_count = java.lang.Math.min(probability.width * probability.height, max_count);
            int i = 0;
            float interval_size = (float)(1.0f / intervals);
            float upper_bound = 1.0f;
            float lower_bound = (float)(upper_bound - interval_size);
            while (i < max_count && lower_bound > interval_size)
            {
                for (int y = 2; y < probability.height - 2; y++)
                {
                    for (int x = 2; x < probability.width - 2; x++)
                    {
                        float val = probability.getPixel(x, y);
                        if (val <= upper_bound && val > lower_bound && place.getPixel(x, y) < 2 && random.nextFloat() < 0.25d)
                        {
                            plants[plant_type][2 * i] = (float)(meters_per_height_unit * ((float)(x + random.nextFloat())));
                            plants[plant_type][2 * i + 1] = (float)(meters_per_height_unit * ((float)(y + random.nextFloat())));
                            place.putPixelWrap(x, y, (float)(place.getPixel(x, y) + 1.0f));
                            i++;
                            if (i >= max_count)
                            {
                                goto brk_out_5;
                            }
                        }
                    }
                }
                lower_bound = (float)(lower_bound - interval_size);
                upper_bound = (float)(upper_bound - interval_size);
            }
            brk_out_5: ;
            return place;
        }
        // Java: 1300
        public void generateUnitLocations(int initial_unit_count, float random_start_pos)
        {
            com.oddlabs.procedural.Channel buildmap = new com.oddlabs.procedural.Channel(access.width, access.height);
            com.oddlabs.procedural.Channel buildmap_debug = new com.oddlabs.procedural.Channel(access.width, access.height);
            for (int y = 0; y < access.height; y++)
            {
                for (int x = 0; x < access.width; x++)
                {
                    if (build[y][x] == com.oddlabs.tt.model.RacesResources.QUARTERS_SIZE)
                    {
                        buildmap.putPixel(x, y, 1.0f);
                    }
                    if (com.oddlabs.tt.procedural.Landscape.DEBUG)
                    {
                        buildmap_debug.putPixel(x, y, (float)(0.2f * build[y][x]));
                    }
                }
            }
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                buildmap.toLayer().saveAsPNG("buildmap");
            }
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                buildmap_debug.toLayer().saveAsPNG("buildmap_debug");
            }
            player_locations = java.lang.JA.newArray2<float>(num_players, 2 * initial_unit_count);
            supply_locations = java.lang.JA.newArray2<int>(num_players, 2);
            float angle = (float)(0.5f * ((float)(java.lang.Math.PI)));
            angle = (float)(angle + (float)((float)(random_start_pos * ((float)(java.lang.Math.PI))) * 2));
            float angle_step = (float)((float)(2.0f * ((float)(java.lang.Math.PI))) / num_players);
            float radius = (float)(0.35f * unit_grids_per_world);
            int scale = meters_per_world / unit_grids_per_world;
            int[] location_quarters = new int[2];
            int[] location_armory = new int[2];
            for (int i = 0; i < num_players; i++)
            {
                int x_2 = java.lang.JC.f2i(((float)((float)((float)(radius * ((float)(java.lang.StrictMath.cos(angle)))) + (unit_grids_per_world >> 1)) + 0.5f)));
                int y_2 = java.lang.JC.f2i(((float)((float)((float)(radius * ((float)(java.lang.StrictMath.sin(angle)))) + (unit_grids_per_world >> 1)) + 0.5f)));
                angle = (float)(angle + angle_step);
                if (archipelago)
                {
                    int[] loc = deep_water_map.find(unit_grids_per_world >> 1, x_2, y_2, 1.0f);
                    for (int u = 0; u < initial_unit_count; u++)
                    {
                        player_locations[i][2 * u] = (loc[0] * scale);
                        player_locations[i][2 * u + 1] = (loc[1] * scale);
                    }
                }
                else
                {
                    location_quarters = buildmap.findNoWrap((unit_grids_per_world >> 1), x_2, y_2, 1.0f);
                    for (int k = -(com.oddlabs.tt.model.RacesResources.QUARTERS_SIZE); k <= (com.oddlabs.tt.model.RacesResources.QUARTERS_SIZE); k++)
                    {
                        for (int l = -(com.oddlabs.tt.model.RacesResources.QUARTERS_SIZE); l <= (com.oddlabs.tt.model.RacesResources.QUARTERS_SIZE); l++)
                        {
                            access.putPixelWrap(location_quarters[0] + k, location_quarters[1] + l, 0.0f);
                            buildmap.putPixelWrap(location_quarters[0] + k, location_quarters[1] + l, 0.0f);
                        }
                    }
                    location_armory = buildmap.find((unit_grids_per_world >> 1), location_quarters[0], location_quarters[1], 1.0f);
                    for (int k_2 = -(com.oddlabs.tt.model.RacesResources.ARMORY_SIZE); k_2 <= (com.oddlabs.tt.model.RacesResources.ARMORY_SIZE); k_2++)
                    {
                        for (int l_2 = -(com.oddlabs.tt.model.RacesResources.ARMORY_SIZE); l_2 <= (com.oddlabs.tt.model.RacesResources.ARMORY_SIZE); l_2++)
                        {
                            access.putPixelWrap(location_armory[0] + k_2, location_armory[1] + l_2, 0.0f);
                            buildmap.putPixelWrap(location_armory[0] + k_2, location_armory[1] + l_2, 0.0f);
                        }
                    }
                    int[] location_unit_start = default;
                    location_unit_start = access.find((unit_grids_per_world >> 1), location_quarters[0], location_quarters[1], 1.0f);
                    supply_locations[i][0] = location_armory[0];
                    supply_locations[i][1] = location_armory[1];
                    int[] location_unit = new int[2];
                    for (int u_2 = 0; u_2 < initial_unit_count; u_2++)
                    {
                        location_unit = access.find((unit_grids_per_world >> 1), location_unit_start[0], location_unit_start[1], 1.0f);
                        access.putPixelWrap(location_unit[0], location_unit[1], 0.0f);
                        player_locations[i][2 * u_2] = (location_unit[0] * scale);
                        player_locations[i][2 * u_2 + 1] = (location_unit[1] * scale);
                    }
                }
            }
            java.util.List player_locations_list = java.util.Arrays.asList(player_locations);
            java.util.Collections.shuffle(player_locations_list, random);
        }
        // Java: 1376
        public static com.oddlabs.tt.resource.FogInfo getFogInfo(com.oddlabs.tt.procedural.Landscape.TerrainType terrain, int meters_per_world)
        {
            return ((terrain).ordinal() switch { 0 /*NATIVE*/ => (com.oddlabs.tt.resource.FogInfo)(new com.oddlabs.tt.resource.DistanceFogInfo(com.oddlabs.tt.resource.FogInfo.Mode.EXP2, com.oddlabs.tt.procedural.Landscape.NATIVE_FOG_COLOR, com.oddlabs.tt.procedural.Landscape.NATIVE_FOG_DENSITY, (float)(com.oddlabs.tt.procedural.Landscape.NATIVE_FOG_HEIGHT * meters_per_world), 0.0f, meters_per_world >> 2)), 1 /*VIKING*/ => (com.oddlabs.tt.resource.FogInfo)(new com.oddlabs.tt.resource.DistanceFogInfo(com.oddlabs.tt.resource.FogInfo.Mode.EXP2, com.oddlabs.tt.procedural.Landscape.VIKING_FOG_COLOR, com.oddlabs.tt.procedural.Landscape.VIKING_FOG_DENSITY, (float)(com.oddlabs.tt.procedural.Landscape.VIKING_FOG_HEIGHT * meters_per_world), 0.0f, meters_per_world >> 2)), _ => throw new System.InvalidOperationException("MatchException") });
        }
        // Java: 1387
        public void generateWaterGrid()
        {
            this.dock = java.lang.JA.newArray2<sbyte>(unit_grids_per_world, unit_grids_per_world);
            this.water = java.lang.JA.newArray2<sbyte>(unit_grids_per_world, unit_grids_per_world);
            water_map = height.copy().threshold((float)(com.oddlabs.tt.global.Globals.SEA_LEVEL - 10.0f), com.oddlabs.tt.global.Globals.SEA_LEVEL).floodfill(0, 0, (float)(-1.0f), 0.1f, null, null).threshold((float)(-1.01f), (float)(-0.99f));
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                water_map.toLayer().saveAsPNG("water_map");
            }
            com.oddlabs.procedural.Channel shore_line = water_map.copy().smooth(2).threshold(0.4f, 0.6f);
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                shore_line.toLayer().saveAsPNG("shore_line");
            }
            com.oddlabs.procedural.Channel beach = height.copy().threshold((float)(com.oddlabs.tt.global.Globals.SEA_LEVEL - (float)(0.1f / height_scale)), (float)(com.oddlabs.tt.global.Globals.SEA_LEVEL + (float)(0.1f / height_scale)));
            dock_map = water_map.copy().smooth(6).threshold(0.0f, 0.99f).channelMultiply(shore_line);
            com.oddlabs.procedural.Channel near_beach = dock_map.copy().smooth(8);
            deep_water_map = water_map.copy().smooth(4).threshold(0.99f, 1.0f);
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                deep_water_map.toLayer().saveAsPNG("deep_water");
            }
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                beach.toLayer().saveAsPNG("beach");
            }
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                dock_map.toLayer().saveAsPNG("dock_map");
            }
            for (int y = 0; y < unit_grids_per_world; y++)
            {
                for (int x = 0; x < unit_grids_per_world; x++)
                {
                    sbyte water_val = unchecked((sbyte)(0));
                    water_val = unchecked((sbyte)(water_val + (water_map.getPixel(x, y) > 0.5f ? 1 : 0)));
                    water_val = unchecked((sbyte)(water_val + (deep_water_map.getPixel(x, y) > 0.5f ? 1 : 0)));
                    sbyte dock_val = unchecked((sbyte)(0));
                    if (dock_map.getPixel(x, y) > 0.5f)
                    {
                        dock_val = unchecked((sbyte)(1));
                    }
                    else if (water_val == 0 && near_beach.getPixel(x, y) > 0.0f)
                    {
                        dock_val = unchecked((sbyte)(2));
                    }
                    this.dock[y][x] = dock_val;
                    this.water[y][x] = water_val;
                }
            }
        }
        // Java: 1421
        public com.oddlabs.tt.resource.BlendInfo[] getBlendInfos()
        {
            return blend_infos;
        }
        // Java: 1425
        public com.oddlabs.tt.resource.StructureBlend getSupplyShadows()
        {
            return supply_shadows;
        }
        // Java: 1429
        public com.oddlabs.tt.resource.StructureBlend getTerrainShadows()
        {
            return terrain_shadows;
        }
        // Java: 1433
        public com.oddlabs.tt.resource.GLIntImage getDetail()
        {
            return detail;
        }
        // Java: 1437
        public float[][] getHeight()
        {
            return height.getPixels();
        }
        // Java: 1441
        public sbyte[][] getGround()
        {
            return ground;
        }
        // Java: 1445
        public bool[][] getAccessGrid()
        {
            int size = access_exported.getWidth();
            bool[][] access_grid = java.lang.JA.newArray2<bool>(size, size);
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    access_grid[y][x] = access_exported.getPixel(x, y) > 0;
                }
            }
            return access_grid;
        }
        // Java: 1456
        public sbyte[][] getDockGrid()
        {
            return dock;
        }
        // Java: 1460
        public sbyte[][] getWaterGrid()
        {
            return water;
        }
        // Java: 1464
        public int[][] getIslandIds()
        {
            int size = island_ids.getWidth();
            int[][] grid = java.lang.JA.newArray2<int>(size, size);
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    grid[y][x] = java.lang.JC.f2i(((float)(island_ids.getPixel(x, y) + 0.5f)));
                }
            }
            return grid;
        }
        // Java: 1478
        public java.util.List getIslandInfos()
        {
            java.util.Map counts = new java.util.HashMap();
            countSuppliesPerIsland(trees, counts, 0);
            countSuppliesPerIsland(rock, counts, 1);
            countSuppliesPerIsland(iron, counts, 2);
            java.util.List ret = new java.util.ArrayList();
            foreach (java.util.Map.Entry entry in java.lang.JIter.of(island_info.entrySet()))
            {
                int id = ((int)((int?)entry.getKey()));
                com.oddlabs.tt.landscape.IslandInfo info = ((com.oddlabs.tt.landscape.IslandInfo)entry.getValue());
                int[] c = ((int[])counts.get(id));
                int num_trees = (c != null ? c[0] : 0);
                int num_rocks = (c != null ? c[1] : 0);
                int num_iron = (c != null ? c[2] : 0);
                info.setIron(num_iron);
                info.setRocks(num_rocks);
                info.setTrees(num_trees);
                ret.add(info);
            }
            return ret;
        }
        // Java: 1501
        public void countSuppliesPerIsland(com.oddlabs.procedural.Channel channel, java.util.Map counts, int index)
        {
            for (int y = 0; y < channel.height; y++)
            {
                for (int x = 0; x < channel.width; x++)
                {
                    if (channel.getPixel(x, y) == 1.0f)
                    {
                        int id = java.lang.JC.f2i(((float)(island_ids.getPixel(x, y) + 0.5f)));
                        if (id != 0)
                        {
                            ((int[])counts.computeIfAbsent(id, new java.util.function.Function_L((object k_l) =>
                            {
                                int? k = (int?)k_l;
                                return new int[3];
                            })))[index]++;
                        }
                    }
                }
            }
        }
        // Java: 1513
        public sbyte[][] getBuildGrid()
        {
            return build;
        }
        // Java: 1517
        public java.util.List getTrees()
        {
            return getPositions(trees);
        }
        // Java: 1521
        public java.util.List getPalmtrees()
        {
            return getPositions(palmtrees);
        }
        // Java: 1525
        public java.util.List getRock()
        {
            return getPositions(rock);
        }
        // Java: 1529
        public java.util.List getIron()
        {
            return getPositions(iron);
        }
        // Java: 1533
        public java.util.List getPositions(com.oddlabs.procedural.Channel channel)
        {
            int count = channel.count(1.0f);
            java.util.List list = new java.util.ArrayList(count);
            for (int y = 0; y < channel.height; y++)
            {
                for (int x = 0; x < channel.width; x++)
                {
                    if (channel.getPixel(x, y) == 1.0f)
                    {
                        list.add(new int[] { x, y });
                    }
                }
            }
            return list;
        }
        // Java: 1547
        public float[][] getPlants()
        {
            return plants;
        }
        // Java: 1551
        public float[][] getStartingLocations()
        {
            return player_locations;
        }
        // Java: 1555
        public float getSeaLevelMeters()
        {
            return sea_level_meters;
        }
        static Landscape()
        {
            Landscape.NATIVE_SEA_BOTTOM_COLOR = com.oddlabs.util.Color.argb4v((-13155720));
            Landscape.VIKING_SEA_BOTTOM_COLOR = com.oddlabs.util.Color.argb4v((-15060163));
            Landscape.NATIVE_FOG_COLOR = com.oddlabs.util.Color.argb4v((-5914625));
            Landscape.VIKING_FOG_COLOR = com.oddlabs.util.Color.argb4v((-13408628));
            Landscape.NATIVE_SAND_COLOR = com.oddlabs.util.Color.argb4v((-6452));
            Landscape.NATIVE_DIRT_COLOR = com.oddlabs.util.Color.argb4v((-19584));
            Landscape.NATIVE_GRASS_COLOR = com.oddlabs.util.Color.argb4v((-13405440));
            Landscape.NATIVE_ROCK_TINT = com.oddlabs.util.Color.argb4v((-13159));
            Landscape.VIKING_GRAVEL_COLOR = com.oddlabs.util.Color.argb4v((-5010330));
            Landscape.VIKING_SOIL_COLOR = com.oddlabs.util.Color.argb4v((-5865383));
            Landscape.VIKING_GRASS_COLOR = com.oddlabs.util.Color.argb4v((-13405440));
            Landscape.VIKING_SNOW_COLOR = com.oddlabs.util.Color.argb4v((-855310));
            Landscape.VIKING_CLIFF_GRASS_TINT = com.oddlabs.util.Color.argb4v((-13405440));
            Landscape.DETAIL_GREY = com.oddlabs.util.Color.argb4v((-8355712)).x();
            Landscape.BLEND_LIGHTING_COLOR = com.oddlabs.util.Color.argb4v((-6503));
        }
        private void init_Landscape()
        {
            this.island_info = new java.util.LinkedHashMap();
        }
        public Landscape(int num_players, int meters_per_world, com.oddlabs.tt.procedural.Landscape.TerrainType terrain, float detail_alpha_value, float hills, float vegetation_amount, float supplies_amount, int seed, int initial_unit_count, float random_start_pos, bool archipelago) : base()
        {
            init_Landscape();
            this.terrain = terrain;
            hills = ((float)(java.lang.Math.sqrt(hills)));
            this.num_players = num_players;
            this.features = 4;
            this.hills = hills;
            this.vegetation_amount = (float)(0.25f + (float)(0.75f * vegetation_amount));
            this.supplies_amount = (float)(0.25f + (float)(0.75f * supplies_amount));
            this.seed = seed;
            this.meters_per_world = meters_per_world;
            this.unit_grids_per_world = meters_per_world / com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID;
            this.meters_per_height_unit = meters_per_world / unit_grids_per_world;
            int height_scale = 0;
            float access_threshold = 0.0f;
            this.island_locations = new java.util.ArrayList();
            int size_multiplier = default;
            switch ((meters_per_world))
            {
                case 256:
                {
                    size_multiplier = 1;
                    height_scale = 32;
                    access_threshold = 0.05f;
                    supplyshadow_size = java.lang.Math.max(unit_grids_per_world >> 5, 2);
                    break;
                }
                case 512:
                {
                    size_multiplier = 4;
                    height_scale = 48;
                    access_threshold = 0.0375f;
                    supplyshadow_size = java.lang.Math.max(unit_grids_per_world >> 6, 2);
                    break;
                }
                case 1024:
                {
                    size_multiplier = 16;
                    height_scale = 64;
                    access_threshold = 0.025f;
                    supplyshadow_size = java.lang.Math.max(unit_grids_per_world >> 7, 2);
                    break;
                }
                case 2048:
                {
                    size_multiplier = 40;
                    height_scale = 56;
                    access_threshold = 0.0325f;
                    supplyshadow_size = java.lang.Math.max(unit_grids_per_world >> 8, 2);
                    break;
                }
                default:
                {
                    size_multiplier = 0;
                    supplyshadow_size = 2;
                    break;
                }
            }
            this.height_scale = height_scale;
            this.access_threshold = access_threshold;
            this.build_threshold = (float)(access_threshold / 2.0f);
            this.sea_level_meters = (float)(height_scale * com.oddlabs.tt.global.Globals.SEA_LEVEL);
            this.detail_size = com.oddlabs.tt.global.Globals.DETAIL_SIZE;
            this.structure_size = com.oddlabs.tt.global.Globals.STRUCTURE_SIZE;
            this.detail_alpha_value = detail_alpha_value;
            this.archipelago = archipelago;
            this.area = (float)(size_multiplier * 10000.0f);
            max_plants = size_multiplier * 64;
            if (((object)terrain == (object)com.oddlabs.tt.procedural.Landscape.TerrainType.NATIVE))
            {
                max_trees = java.lang.JC.d2i(java.lang.StrictMath.pow(2, 2 * com.oddlabs.util.Utils.powerOf2Log2(meters_per_world) - 9));
                max_palmtrees = max_trees >> 1;
            }
            else
            {
                max_trees = java.lang.JC.d2i((0.75f * java.lang.StrictMath.pow(2, 2 * com.oddlabs.util.Utils.powerOf2Log2(meters_per_world) - 9)));
                max_palmtrees = max_trees;
            }
            max_rock = max_trees >> 3;
            max_iron = max_trees >> 4;
            random = new java.util.Random(seed);
            float c1 = (float)(-1.0f);
            float c2 = 1.0f;
            float c3 = 0.0f;
            com.oddlabs.tt.procedural.Voronoi voronoi = default;
            com.oddlabs.procedural.Channel voronoi4 = new com.oddlabs.tt.procedural.Voronoi(structure_size, 4, 4, 1, 1.0f, com.oddlabs.tt.procedural.Landscape.STRUCTURE_SEED).getDistance(c1, c2, c3);
            voronoi = new com.oddlabs.tt.procedural.Voronoi(structure_size, 8, 8, 1, 1.0f, com.oddlabs.tt.procedural.Landscape.STRUCTURE_SEED);
            com.oddlabs.procedural.Channel voronoi8 = voronoi.getDistance(c1, c2, c3);
            com.oddlabs.procedural.Channel voronoi8_hit = voronoi.getHitpoint();
            voronoi = new com.oddlabs.tt.procedural.Voronoi(structure_size, 16, 16, 1, 1.0f, com.oddlabs.tt.procedural.Landscape.STRUCTURE_SEED);
            com.oddlabs.procedural.Channel voronoi16 = voronoi.getDistance(c1, c2, c3);
            com.oddlabs.procedural.Channel voronoi16_hit = voronoi.getHitpoint();
            voronoi = new com.oddlabs.tt.procedural.Voronoi(structure_size, 32, 32, 1, 1.0f, com.oddlabs.tt.procedural.Landscape.STRUCTURE_SEED);
            com.oddlabs.procedural.Channel voronoi32 = voronoi.getDistance(c1, c2, c3);
            com.oddlabs.procedural.Channel voronoi32_hit = voronoi.getHitpoint();
            com.oddlabs.procedural.Channel noise8 = new com.oddlabs.tt.procedural.Midpoint(structure_size, 3, 0.45f, com.oddlabs.tt.procedural.Landscape.STRUCTURE_SEED).toChannel();
            com.oddlabs.procedural.Channel noise256 = new com.oddlabs.tt.procedural.Midpoint(structure_size, 8, 1.0f, com.oddlabs.tt.procedural.Landscape.STRUCTURE_SEED).toChannel();
            com.oddlabs.tt.procedural.Landscape.StructureLayers[] layers = ((System.Func<com.oddlabs.tt.procedural.Landscape.StructureLayers[]>)(() =>
            {
                switch ((terrain).ordinal())
                {
                    case 0 /*NATIVE*/:
                    {
                        com.oddlabs.tt.procedural.Landscape.StructureLayers[] natives = generateStructuresNative(voronoi4, voronoi8, voronoi8_hit, voronoi16, voronoi16_hit, voronoi32, voronoi32_hit, noise8, noise256);
                        com.oddlabs.tt.form.ProgressForm.progress();
                        generateTerrainNative();
                        return natives;
                    }
                    case 1 /*VIKING*/:
                    {
                        com.oddlabs.tt.procedural.Landscape.StructureLayers[] vikings = generateStructuresViking(voronoi4, voronoi8, voronoi8_hit, voronoi16, voronoi16_hit, voronoi32, voronoi32_hit, noise8, noise256);
                        com.oddlabs.tt.form.ProgressForm.progress();
                        generateTerrainViking();
                        return vikings;
                    }
                }
                throw new System.InvalidOperationException("MatchException");
            }))();
            island_ids = access.copy();
            int last_id = 2;
            int search_x = 0;
            int search_y = 0;
            com.oddlabs.procedural.Channel.ChannelVisitor visitor = island_ids.visitor(1.0f);
            while (true)
            {
                int[] pos = visitor.visitNext();
                if (pos[0] == (-1) || pos[1] == (-1))
                {
                    break;
                }
                int[] count = new int[1];
                int[] bounds = new int[4];
                island_ids.floodfill(pos[0], pos[1], ((float)(last_id)), 0.01f, count, bounds);
                int area = count[0];
                com.oddlabs.tt.landscape.IslandInfo info = new com.oddlabs.tt.landscape.IslandInfo(last_id, area, bounds[0], bounds[1], bounds[2], bounds[3], pos[0], pos[1], dock, island_ids);
                island_info.put(last_id, info);
                last_id++;
            }
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                island_ids.copy().multiply((float)(1.0f / last_id)).toLayer().saveAsPNG("island_ids");
            }
            this.structures = new com.oddlabs.tt.resource.GLIntImage[layers.Length];
            this.structure_normals = new com.oddlabs.tt.resource.GLIntImage[layers.Length];
            for (int i = 0; i < layers.Length; i++)
            {
                this.structures[i] = new com.oddlabs.tt.resource.GLIntImage(layers[i].diffuse_f);
                this.structure_normals[i] = new com.oddlabs.tt.resource.GLIntImage(layers[i].normal_f);
            }
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                height.toLayer().saveAsPNG("height");
            }
            com.oddlabs.tt.form.ProgressForm.progress();
            com.oddlabs.procedural.Channel grass_alpha = generateAlphas();
            com.oddlabs.tt.form.ProgressForm.progress();
            generateUnitLocations(initial_unit_count, random_start_pos);
            generateSupplies(grass_alpha);
            for (int y = 0; y < unit_grids_per_world; y++)
            {
                for (int x = 0; x < unit_grids_per_world; x++)
                {
                    height.putPixel(x, y, (float)(height_scale * height.getPixel(x, y)));
                }
            }
            if (com.oddlabs.tt.procedural.Landscape.DEBUG)
            {
                access.toLayer().saveAsPNG("access_connected");
            }
            blend_infos = new com.oddlabs.tt.resource.BlendInfo[] { new com.oddlabs.tt.resource.StructureBlend(structures[0], structure_normals[0], new com.oddlabs.tt.resource.GLByteImage(new com.oddlabs.procedural.Channel(1, 1).fill(1.0f), org.lwjgl.opengl.GL11.GL_RED)), new com.oddlabs.tt.resource.StructureBlend(structures[1], structure_normals[1], alpha_maps[0]), new com.oddlabs.tt.resource.StructureBlend(structures[2], structure_normals[2], alpha_maps[1]), new com.oddlabs.tt.resource.StructureBlend(structures[3], structure_normals[3], alpha_maps[2]), new com.oddlabs.tt.resource.StructureBlend(structures[4], structure_normals[4], alpha_maps[3]), new com.oddlabs.tt.resource.BlendLighting(alpha_maps[4], 1.0f, 0.9f, 0.6f), new com.oddlabs.tt.resource.StructureBlend(structures[6], structure_normals[6], alpha_maps[6]) };
            terrain_shadows = new com.oddlabs.tt.resource.StructureBlend(structures[5], structure_normals[5], alpha_maps[5]);
            supply_shadows = new com.oddlabs.tt.resource.StructureBlend(structures[5], structure_normals[5], alpha_maps[7]);
        }
    }
}
