// Ported from Java (Tribal Trouble 1.4.1). Mechanically transliterated by PortTools/j2cs.py,
// then reviewed by hand. Java identifiers are kept so this file diffs against its source.
using System;
using System.Collections.Generic;
using TribalTrouble.Sim.Determinism;
using TribalTrouble.Sim;

namespace TribalTrouble.Sim.Procedural
{
    public sealed class Landscape {
        public const bool DEBUG = false;
        // The only world sizes this class's constructor switch (below) actually knows how to configure
        // (size_multiplier / height_scale / access_threshold). Exposed so callers that derive
        // meters_per_world some other way - e.g. CustomMapGenerator, from an authored map file's actual
        // pixel dimensions - can validate against the same authoritative list instead of duplicating it,
        // and fail with a clear message of their own before ever reaching the `default` branch's bare
        // assertion here (which has no context about which map/caller triggered it, and does nothing at
        // all if assertions are disabled). //added by ikill240c 2026-09-14
        public static readonly int[] VALID_METERS_PER_WORLD = {256, 512, 1024, 2048, 4096}; //added by ikill240c 2026-09-14
        // The constructor's height_scale per world size, for code that converts meters back to the generator's 0..1
        // heights (the 3D map editor's saved maps, see IslandGenerator). Keep in step with the constructor's switch. //added by ikill240c
        public static int heightScaleFor(int meters_per_world) { //added by ikill240c
            return meters_per_world switch { //added by ikill240c
                256 => 32,
                512 => 48,
                1024 => 64,
                2048 => 56,
                4096 => 56,
                _ => throw new ArgumentException("illegal meters_per_world " + meters_per_world),
            };
        } //added by ikill240c
        // erode()/erodeThermal() below (in Channel) run a full pass over the ENTIRE grid per iteration,
        // and every call site here scales the iteration count linearly with unit_grids_per_world - so
        // total erosion cost is iterations * width * height, which is CUBIC in the map's side length,
        // not quadratic like the rest of generation. That scaling was only ever exercised up through
        // 2048 (512 iterations there), where it was tolerable; naively continuing the same formula for
        // a newer, larger tier multiplies total erosion work faster than the rest of generation scales
        // by (cubic vs quadratic) - that compounding is what actually caused the freeze at the largest
        // map size, not just "more pixels take longer". Capping the iteration count at what it already
        // was for 2048 keeps every existing, already-tuned map size completely unchanged, and keeps any
        // newer, larger tier's erosion cost proportional to its pixel count instead of its pixel count
        // times an ever-growing iteration count. Erosion's visual effect also genuinely saturates after
        // enough passes anyway (further iterations do less and less once most slopes have relaxed below
        // the talus threshold), so this isn't just a speed/quality tradeoff - past a point, more
        // iterations were already buying very little. //added by ikill240c 2026-09-18
        private const int MAX_ERODE_ITERATIONS = 2048 >> 2; //added by ikill240c 2026-09-18
        private const int MAX_ERODE_THERMAL_ITERATIONS = 2048 >> 3; //added by ikill240c 2026-09-18
        // Channel.find()/findNoWrap() do an expanding ring search outward from a starting pixel, costing
        // O(r) work per ring - so a full failed search out to radius r costs O(r^2) overall. Every call
        // site below passes unit_grids_per_world >> 1 as that radius, several of them inside a
        // per-starting-unit loop (once per player, again once per unit that player starts with) - so at
        // the previous max map size (2048, radius 1024) a single worst-case search was already ~1M pixel
        // checks, and a newer, larger tier multiplies that quadratically with radius, repeated for every
        // player and every one of their starting units. That's the actual freeze this was capping
        // erosion for above, just in the unit/building placement pass instead of erosion - erosion
        // finishing didn't help because this runs afterward, unconditionally, on every game start
        // regardless of map size. Capped at what the radius already was at 2048 (the largest tier that
        // was ever actually exercised before any larger tier was added) for the same reason as the
        // erosion cap: a ~1024-pixel search radius is already enormous for "find a buildable spot near
        // the starting position" - if nothing turns up within that, searching even further out serves
        // the "near start" intent poorly anyway, so there's no real downside to capping it here.
        // //added by ikill240c 2026-09-18
        private const int MAX_PLACEMENT_SEARCH_RADIUS = 2048 >> 1; //added by ikill240c 2026-09-18
        private const int STRUCTURE_SEED = 42; // must be constant; otherwise distinct repeating patterns might appear

        private const int NUM_PLANT_TYPES = 4;

        /** Depth (in meters below sea level) where the sea bottom color starts blending in. */
        private const float SEABOTTOM_DEPTH_OFFSET_METERS = 3f;

        /** Fraction of baked sun shading kept on terrain below the waterline. */
        private const float UNDERWATER_SHADING = 0.3f;

        public static readonly Vector4f NATIVE_SEA_BOTTOM_COLOR = Color.argb4v(unchecked((int)0xFF_37_42_78));  // (Deep Navy Blue)
        public static readonly Vector4f VIKING_SEA_BOTTOM_COLOR = Color.argb4v(unchecked((int)0xFF_1A_33_3D));  // (Dark Blue-Green)

        private static readonly Vector4f NATIVE_FOG_COLOR = Color.argb4v(unchecked((int)0xFF_A5_BF_FF));
        private static readonly Vector4f VIKING_FOG_COLOR = Color.argb4v(unchecked((int)0xFF_33_66_8C));
        private const float NATIVE_FOG_DENSITY = 0.0012f;
        private const float VIKING_FOG_DENSITY = 0.0016f;
        private const float NATIVE_FOG_HEIGHT = 1.2f;
        private const float VIKING_FOG_HEIGHT = 1.4f;

        // Native terrain colors (RGB)
        private static readonly Vector4f NATIVE_SAND_COLOR = Color.argb4v(unchecked((int)0xFF_FF_E6_CC));
        private static readonly Vector4f NATIVE_DIRT_COLOR = Color.argb4v(unchecked((int)0xFF_FF_B3_80));
        private static readonly Vector4f NATIVE_GRASS_COLOR = Color.argb4v(unchecked((int)0xFF_33_73_00));
        private static readonly Vector4f NATIVE_ROCK_TINT = Color.argb4v(unchecked((int)0xFF_FF_CC_99));

        // Viking terrain colors (RGB)
        private static readonly Vector4f VIKING_GRAVEL_COLOR = Color.argb4v(unchecked((int)0xFF_B3_8C_66));
        private static readonly Vector4f VIKING_SOIL_COLOR = Color.argb4v(unchecked((int)0xFF_A6_80_59));
        private static readonly Vector4f VIKING_GRASS_COLOR = Color.argb4v(unchecked((int)0xFF_33_73_00)); // Same as native, but gets color-shifted
        private static readonly Vector4f VIKING_SNOW_COLOR = Color.argb4v(unchecked((int)0xFF_F2_F2_F2));
        private static readonly Vector4f VIKING_CLIFF_GRASS_TINT = Color.argb4v(unchecked((int)0xFF_33_73_00));

        // Misc colors
        private static readonly float DETAIL_GREY = Color.argb4v(unchecked((int)0xFF_80_80_80)).x();
        private static readonly Vector4f BLEND_LIGHTING_COLOR = Color.argb4v(unchecked((int)0xFF_FF_E6_99)); // 1.0, 0.9, 0.6

        public enum TerrainType {
            NATIVE,
            VIKING
        }

        private const int MIN_ISLAND_AREA = 2000;

        private readonly JavaRandom random;
        private readonly BlendInfo[] blend_infos;

        private sealed class StructureLayers { // Java record
            public readonly Layer diffuse;
            public readonly Layer normal;
            public StructureLayers(Layer diffuse, Layer normal) { this.diffuse = diffuse; this.normal = normal; }
        }

        private GLIntImage[] structures;
        private GLIntImage[] structure_normals;
        private GLIntImage detail;
        private GLByteImage[] alpha_maps;

        private Channel height;
        private Channel slope;
        private Channel water_map;
        private Channel dock_map;
        private Channel island_ids;
        private Channel good_starts;
        private Channel access;
        private Channel access_exported;
        private Channel relheight;
        private Channel highlight;
        private Channel shadow;
        private Channel trees;
        private Channel palmtrees;
        private Channel rock;
        private Channel iron;

        private List<int[]> island_locations;
        private List<object> island_areas; // unused (raw ArrayList in Java)
        private readonly Dictionary<int, IslandInfo> island_info = new();

        private readonly int num_players;
        private readonly int meters_per_world;
        private readonly int meters_per_height_unit;
        private readonly int unit_grids_per_world;
        private readonly int height_scale;
        private readonly float sea_level_meters;
        private readonly int detail_size;
        private readonly int structure_size;
        private readonly float detail_alpha_value;
        private readonly int features;
        private readonly float hills;
        private readonly float vegetation_amount;
        private readonly float supplies_amount;
        private readonly int seed;
        private readonly float area;
        private readonly int max_trees;
        private readonly int max_palmtrees;
        private readonly int max_rock;
        private readonly int max_iron;
        private readonly int max_plants;
        private readonly float access_threshold;
        private readonly float build_threshold;
        private readonly TerrainType terrain;
        private readonly bool archipelago;
        // When non-null, terrain generation branches to loadAuthoredHeight()/loadAuthoredSupplies()
        // instead of the noise-driven generateTerrainNative()/Viking()/generateSupplies() paths - see
        // those methods for exactly what's substituted and what's left unchanged. //added by ikill240c
        private readonly AuthoredTerrain authored_terrain; //added by ikill240c
        // When true (and player_teams is non-null), generateUnitLocations()'s placement step
        // groups same-team players onto adjacent circle slots instead of a fully random shuffle - see
        // that method's own comment for the exact algorithm. //added by ikill240c
        private readonly bool team_together; //added by ikill240c
        private readonly int[] player_teams; //added by ikill240c

        private sbyte[][] build;
        private sbyte[][] dock;
        private sbyte[][] water;
        private float[][] player_locations;
        private int[][] supply_locations;
        private float[][] plants;

        public Landscape(int num_players, int meters_per_world, TerrainType terrain, float detail_alpha_value,
                float hills, float vegetation_amount, float supplies_amount, int seed, int initial_unit_count,
                float random_start_pos, bool archipelago, AuthoredTerrain authored_terrain,
                bool team_together, int[] player_teams) { //added by ikill240c
            this.authored_terrain = authored_terrain; //added by ikill240c
            this.team_together = team_together; //added by ikill240c
            this.player_teams = player_teams; //added by ikill240c
            this.terrain = terrain;
            hills = (float) J.Sqrt(hills);
            this.num_players = num_players;
            this.features = 4;
            this.hills = hills;
            this.vegetation_amount = (float)(0.25f + (float)(0.75f * vegetation_amount));
            this.supplies_amount = (float)(0.25f + (float)(0.75f * supplies_amount));
            this.seed = seed;
            this.meters_per_world = meters_per_world;
            this.unit_grids_per_world = meters_per_world / HeightMap.METERS_PER_UNIT_GRID;
            this.meters_per_height_unit = meters_per_world / unit_grids_per_world;
            int height_scale = 0;
            float access_threshold = 0f;
            this.island_locations = new List<int[]>();
            int size_multiplier;
            switch (meters_per_world) {
                case 256: {
                    size_multiplier = 1;
                    height_scale = 32;
                    access_threshold = 0.05f;
                    break;
                }
                case 512: {
                    size_multiplier = 4;
                    height_scale = 48;
                    access_threshold = 0.0375f;
                    break;
                }
                case 1024: {
                    size_multiplier = 16;
                    height_scale = 64;
                    access_threshold = 0.025f;
                    break;
                }
                case 2048: {
                    size_multiplier = 40;
                    height_scale = 56;
                    access_threshold = 0.0325f;
                    break;
                }
                case 4096: { //added by ikill240c - SIZE_UNREAL, 2x SIZE_ENORMOUS's side length (4x area)
                    // Not a strict continuation of any formula - size_multiplier's own growth rate
                    // already SLOWS at each existing tier (256=>512 is 4x, 512=>1024 is 4x, 1024=>2048
                    // is only 2.5x), i.e. deliberately tempered down from a naive area-based scaling
                    // (which would give 64, not 40, at 2048) to avoid overcrowding a huge map with
                    // resources. Continuing that same slowing trend conservatively rather than guessing
                    // at an aggressive value with no way to playtest it here. height_scale and
                    // access_threshold are carried over unchanged from Enormous - both are about
                    // vertical exaggeration and walkable-slope feel respectively, neither obviously
                    // needs to change just because the map is wider. Re-tune any of these three if the
                    // actual in-game density/feel at this size isn't right - there was no way to verify
                    // that from here. //added by ikill240c
                    size_multiplier = 60; //added by ikill240c
                    height_scale = 56; //added by ikill240c
                    access_threshold = 0.0325f; //added by ikill240c
                    break;
                } //added by ikill240c
                default: {
                    size_multiplier = 0;
                    System.Diagnostics.Debug.Assert(false);
                    break;
                }
            }
            this.height_scale = height_scale;
            this.access_threshold = access_threshold;
            this.build_threshold = (float)(access_threshold / 2f);
            this.sea_level_meters = (float)(height_scale * Globals.SEA_LEVEL);
            this.detail_size = Globals.DETAIL_SIZE;
            this.structure_size = Globals.STRUCTURE_SIZE;
            this.detail_alpha_value = detail_alpha_value;
            this.archipelago = archipelago;

            area = (float)(size_multiplier * 10000f);
            max_plants = size_multiplier * 64;

            if (terrain == TerrainType.NATIVE) {
                max_trees = J.I(J.Pow(2, 2 * Utils.powerOf2Log2(meters_per_world) - 9));
                max_palmtrees = max_trees >> 1;
            } else {
                max_trees = J.I((.75f * J.Pow(2, 2 * Utils.powerOf2Log2(meters_per_world) - 9)));
                max_palmtrees = max_trees;
            }

            max_rock = max_trees >> 3;
            max_iron = max_trees >> 4;
            random = new JavaRandom(seed);

            // generate shared voronoi and noise maps
            float c1 = -1f;
            float c2 = 1f;
            float c3 = 0f;
            Voronoi voronoi;
            Channel voronoi4 = new Voronoi(structure_size, 4, 4, 1, 1f, STRUCTURE_SEED).getDistance(c1, c2, c3);
            voronoi = new Voronoi(structure_size, 8, 8, 1, 1f, STRUCTURE_SEED);
            Channel voronoi8 = voronoi.getDistance(c1, c2, c3);
            Channel voronoi8_hit = voronoi.getHitpoint();
            voronoi = new Voronoi(structure_size, 16, 16, 1, 1f, STRUCTURE_SEED);
            Channel voronoi16 = voronoi.getDistance(c1, c2, c3);
            Channel voronoi16_hit = voronoi.getHitpoint();
            voronoi = new Voronoi(structure_size, 32, 32, 1, 1f, STRUCTURE_SEED);
            Channel voronoi32 = voronoi.getDistance(c1, c2, c3);
            Channel voronoi32_hit = voronoi.getHitpoint();
            Channel noise8 = new Midpoint(structure_size, 3, 0.45f, STRUCTURE_SEED).toChannel();
            Channel noise256 = new Midpoint(structure_size, 8, 1f, STRUCTURE_SEED).toChannel();

            StructureLayers[] layers;
            if (terrain == TerrainType.NATIVE) {
                    var natives = generateStructuresNative(voronoi4, voronoi8, voronoi8_hit, voronoi16, voronoi16_hit,
                            voronoi32, voronoi32_hit, noise8, noise256);
                    ProgressForm.progress();
                    if (authored_terrain != null && authored_terrain.hasHeights()) { //added by ikill240c
                        loadAuthoredHeight(); //added by ikill240c //added by ikill240c
                    } else { //added by ikill240c
                        generateTerrainNative();
                    } //added by ikill240c
                layers = natives;
            } else {
                    var vikings = generateStructuresViking(voronoi4, voronoi8, voronoi8_hit, voronoi16, voronoi16_hit,
                            voronoi32, voronoi32_hit, noise8, noise256);
                    ProgressForm.progress();
                    if (authored_terrain != null && authored_terrain.hasHeights()) { //added by ikill240c
                        loadAuthoredHeight(); //added by ikill240c //added by ikill240c
                    } else { //added by ikill240c
                        generateTerrainViking();
                    } //added by ikill240c
                layers = vikings;
            }

            island_ids = access.copy();
            int last_id = 2;
            int search_x = 0;
            int search_y = 0;
            Channel.ChannelVisitor visitor = island_ids.visitor(1.0f);
            while (true) {
                int[] pos = visitor.visitNext();
                if (pos[0] == -1 || pos[1] == -1) {
                    break;
                }
                int[] count = new int[1];
                int[] bounds = new int[4];
                island_ids.floodfill(pos[0], pos[1], (float) last_id, 0.01f, count, bounds);
                int area = count[0];
                if (count[0] < MIN_ISLAND_AREA) {
                    // If not big enough, put it back
                    island_ids.floodfill(pos[0], pos[1], 0.0f, 0.01f, null, null);
                }
                IslandInfo info = new IslandInfo(last_id, area, bounds[0], bounds[1], bounds[2], bounds[3], pos[0], pos[1]);
                island_info.put(last_id, info);
                last_id++;
            }
            if (DEBUG) {
                island_ids.copy().multiply((float)(1.0f / last_id)).toLayer().saveAsPNG("island_ids");
            }
            good_starts = island_ids.copy().threshold(0.5f, (float)(last_id + 1.0f));

            this.structures = new GLIntImage[layers.Length];
            this.structure_normals = new GLIntImage[layers.Length];
            for (int i = 0; i < layers.Length; i++) {
                this.structures[i] = new GLIntImage(layers[i].diffuse);
                this.structure_normals[i] = new GLIntImage(layers[i].normal);
            }

            if (DEBUG) height.toLayer().saveAsPNG("height");
            ProgressForm.progress();
            Channel grass_alpha = generateAlphas();
            ProgressForm.progress();
            generateUnitLocations(initial_unit_count, random_start_pos);
            if (authored_terrain != null && authored_terrain.hasAuthoredResources()) { //added by ikill240c
                loadAuthoredSupplies(); //added by ikill240c //added by ikill240c
            } else { //added by ikill240c
                generateSupplies(grass_alpha);
            } //added by ikill240c

            // scale height map vertically
            for (int y = 0; y < unit_grids_per_world; y++) {
                for (int x = 0; x < unit_grids_per_world; x++) {
                    height.putPixel(x, y, (float)(height_scale * height.getPixel(x, y)));
                }
            }

            if (DEBUG) access.toLayer().saveAsPNG("access_connected");

            // create blend infos
            blend_infos = new BlendInfo[]{new StructureBlend(structures[0], structure_normals[0], new GLByteImage(
                    new Channel(1, 1).fill(1f), GL11.GL_RED)), new StructureBlend(structures[1], structure_normals[1],
                            alpha_maps[0]), new StructureBlend(structures[2], structure_normals[2],
                                    alpha_maps[1]), new StructureBlend(structures[3], structure_normals[3],
                                            alpha_maps[2]), new StructureBlend(structures[4], structure_normals[4],
                                                    alpha_maps[3]), new BlendLighting(alpha_maps[4], 1f, 0.9f,
                                                            0.6f), new StructureBlend(structures[5], structure_normals[5],
                                                                    alpha_maps[5]), new StructureBlend(structures[6],
                                                                            structure_normals[6], alpha_maps[6])
            };
        }

        // **************
        // * STRUCTURES *
        // **************
        private StructureLayers[] generateStructuresNative(Channel voronoi4,
                Channel voronoi8, Channel voronoi8_hit, Channel voronoi16, Channel voronoi16_hit,
                Channel voronoi32, Channel voronoi32_hit, Channel noise8, Channel noise256) {
            var structures = new StructureLayers[7];
            ProgressForm.progress((float)(1 / 8f));

            structures[0] = Landscape.genSand(structure_size, noise8.copy(), noise256.copy());

            structures[1] = Landscape.genDirt(structure_size, noise8.copy(), noise256.copy(), voronoi32.copy());

            Layer structure_rubble = Landscape.genRubble(structure_size, noise8.copy(), voronoi4.copy(), voronoi8.copy(),
                    voronoi16.copy(), structures[1].diffuse.copy()).diffuse; // Rubble uses dirt as base? Wait, check old code.
            // Old code: Layer structure_rubble = Landscape.genRubble(..., structure_dirt.copy());
            // genRubble logic: rubble.multiply(.9f).bump(...)
            // I need to pass the base layer.
            structures[2] = Landscape.genRubble(structure_size, noise8.copy(), voronoi4.copy(), voronoi8.copy(),
                    voronoi16.copy(), structures[1].diffuse.copy());

            structures[4] = genGrass(structure_size, noise8.copy(), noise256.copy());

            structures[3] = Landscape.genRock(structure_size, noise8.copy(), noise256.copy(), voronoi4.copy(),
                    voronoi8.copy(), voronoi16.copy(), structures[2].diffuse.copy(), structures[4].diffuse.copy());

            structures[5] = Landscape.genBlack();

            structures[6] = Landscape.genSeabottom(terrain, structure_size, noise8.copy(), noise256.copy(), voronoi4.copy(),
                    voronoi8.copy());

            StructureLayers structure_detail = Landscape.genDetail(detail_size, detail_alpha_value, STRUCTURE_SEED,
                    noise8.copy());
            detail = new GLIntImage(structure_detail.diffuse);
            return structures;
        }

        private StructureLayers[] generateStructuresViking(Channel voronoi4,
                Channel voronoi8, Channel voronoi8_hit, Channel voronoi16, Channel voronoi16_hit,
                Channel voronoi32, Channel voronoi32_hit, Channel noise8, Channel noise256) {
            var structures = new StructureLayers[7];
            ProgressForm.progress((float)(1 / 8f));

            structures[0] = Landscape.genGravel(structure_size, noise8.copy(), noise256.copy());

            structures[1] = Landscape.genSoil(structure_size, noise8.copy(), noise256.copy(), voronoi32.copy());

            structures[3] = genGrass(structure_size, noise8.copy(), noise256.copy());
            structures[3].diffuse.multiply(.75f, .9f, 1.1f);

            structures[2] = genCliff(structure_size, noise8.copy(), noise256.copy(), voronoi4.copy(), voronoi8.copy(),
                    voronoi16.copy(), structures[1].diffuse.copy(), structures[3].diffuse.copy());

            structures[4] = Landscape.genSnow(structure_size, noise8.copy(), noise256.copy());

            structures[5] = Landscape.genBlack();

            structures[6] = Landscape.genSeabottom(terrain, structure_size, noise8.copy(), noise256.copy(), voronoi4.copy(),
                    voronoi8.copy());

            StructureLayers structure_detail = Landscape.genDetail(detail_size, detail_alpha_value, STRUCTURE_SEED,
                    noise8.copy());
            detail = new GLIntImage(structure_detail.diffuse);

            return structures;
        }

        private static StructureLayers genSand(int size, Channel noise8, Channel noise256) {
            Channel empty = new Channel(size, size).fill(1f);
            Channel sand_bump1 = noise8.brightness(0.75f);
            Channel sand_bump2 = noise256.brightness(0.15f);
            Layer sand = new Layer(empty.copy().fill(NATIVE_SAND_COLOR.x()), empty.copy().fill(NATIVE_SAND_COLOR.y()),
                    empty.copy().fill(NATIVE_SAND_COLOR.z()));
            Channel bump = sand_bump1.channelAdd(sand_bump2);
            sand.bump(bump, (float)(size / 1024f), 0f, 0.1f, 1f, 1f, 1f, 0f, 0f, 0f);
            if (DEBUG) sand.saveAsPNG("structure_sand");

            return new StructureLayers(sand, getFlatNormal(size)); // Forced flat normal
        }

        private static StructureLayers genGravel(int size, Channel noise8, Channel noise256) {
            Channel empty = new Channel(size, size).fill(1f);
            Channel gravel_bump1 = noise8.brightness(0.5f);
            Channel gravel_bump2 = noise256.brightness(0.5f);
            Layer gravel = new Layer(empty.copy().fill(VIKING_GRAVEL_COLOR.x()), empty.copy().fill(VIKING_GRAVEL_COLOR.y()),
                    empty.copy().fill(VIKING_GRAVEL_COLOR.z()));
            Channel bump = gravel_bump1.channelAdd(gravel_bump2);
            gravel.bump(bump, (float)(size / 1024f), 0f, 0.1f, 1f, 1f, 1f, 0f, 0f, 0f);
            if (DEBUG) gravel.saveAsPNG("structure_gravel");

            return new StructureLayers(gravel, getFlatNormal(size)); // Forced flat normal
        }

        private static StructureLayers genDirt(int size, Channel noise8, Channel noise256,
                Channel voronoi32) {
            Channel empty = new Channel(size, size).fill(1f);
            Channel dirt_bump1 = noise8.brightness(0.8f);
            Channel dirt_bump2 = noise256.brightness(0.1f);
            Channel dirt_bump3 = voronoi32.brightness(0.1f);
            Layer dirt = new Layer(empty.copy().fill(NATIVE_DIRT_COLOR.x()), empty.copy().fill(NATIVE_DIRT_COLOR.y()),
                    empty.copy().fill(NATIVE_DIRT_COLOR.z()));
            Channel dirt_bump = dirt_bump1.channelAdd(dirt_bump2).channelAdd(dirt_bump3);
            dirt_bump.perturb(noise8, 0.05f);
            dirt.bump(dirt_bump, (float)(size / 128f), 0f, 0.5f, 1f, 1f, 1f, 0f, 0f, 0f);
            if (DEBUG) dirt.saveAsPNG("structure_dirt");

            return new StructureLayers(dirt, getFlatNormal(size)); // Forced flat normal
        }

        private static StructureLayers genSoil(int size, Channel noise8, Channel noise256,
                Channel voronoi32) {
            Channel empty = new Channel(size, size).fill(1f);
            Channel soil_bump1 = noise8.brightness(0.8f);
            Channel soil_bump2 = noise256.brightness(0.1f);
            Channel soil_bump3 = voronoi32.brightness(0.1f);
            Layer soil = new Layer(empty.copy().fill(VIKING_SOIL_COLOR.x()), empty.copy().fill(VIKING_SOIL_COLOR.y()),
                    empty.copy().fill(VIKING_SOIL_COLOR.z()));
            Channel soil_bump = soil_bump1.channelAdd(soil_bump2).channelAdd(soil_bump3);
            soil_bump.perturb(noise8, 0.05f);
            soil.bump(soil_bump, (float)(size / 128f), 0f, 0.5f, 1f, 1f, 1f, 0f, 0f, 0f);
            if (DEBUG) soil.saveAsPNG("structure_soil");

            Channel zero = new Channel(size, size).fill(0f);
            return new StructureLayers(soil, soil_bump.toNormalMap(1.5f, zero));
        }

        private StructureLayers genGrass(int size, Channel noise8, Channel noise256) {
            Channel empty = new Channel(size, size).fill(1f);
            Channel grass_bump = noise8.copy().rotate(90).channelAdd(noise256.brightness(0.02f));
            Vector4f grassColor = terrain == TerrainType.NATIVE ? NATIVE_GRASS_COLOR : VIKING_GRASS_COLOR;
            Layer grass = new Layer(empty.copy().fill(grassColor.x()), empty.copy().fill(grassColor.y()), empty.copy().fill(
                    grassColor.z()));
            grass.r.channelAdd(noise8.brightness(0.2f));
            grass.bump(grass_bump, (float)(size / 256f), 0f, 0.6f, 1f, 1f, 1f, 0f, 0f, 0f);
            if (DEBUG) grass.saveAsPNG("structure_grass");

            Channel zero = new Channel(size, size).fill(0f);
            return new StructureLayers(grass, grass_bump.toNormalMap(0.5f, zero));
        }

        private static StructureLayers genRubble(int size, Channel noise8, Channel voronoi4,
                Channel voronoi8, Channel voronoi16, Layer rubble) {
            Channel rubble_bump1 = voronoi4.multiply(0.4f);
            Channel rubble_bump2 = voronoi8.multiply(0.3f);
            Channel rubble_bump3 = voronoi16.multiply(0.2f);
            Channel rubble_bump = rubble_bump1.channelAdd(rubble_bump2).channelAdd(rubble_bump3).dynamicRange();
            rubble_bump.perturb(noise8, 0.1f);
            rubble.multiply(.9f).bump(rubble_bump, (float)(size / 128f), 0f, 0f, 1f, 1f, 1f, 0f, 0f, 0f);
            if (DEBUG) rubble.saveAsPNG("structure_rubble");

            Channel zero = new Channel(size, size).fill(0f);
            return new StructureLayers(rubble, rubble_bump.toNormalMap(2.5f, zero));
        }

        private static StructureLayers genRock(int size, Channel noise8, Channel noise256,
                Channel voronoi4, Channel voronoi8, Channel voronoi16, Layer rubble,
                Layer grass) {
            Channel rock_bump1 = voronoi4.dynamicRange().contrast(1.1f).brightness(2f).gamma2().multiply(0.35f);
            Channel rock_bump2 = voronoi8.dynamicRange().contrast(1.1f).brightness(2f).gamma2().multiply(0.3f);
            Channel rock_bump3 = voronoi16.dynamicRange().contrast(1.1f).brightness(2f).gamma2().multiply(0.2f);
            Channel rock_bump = rock_bump1.channelAdd(rock_bump2).channelAdd(rock_bump3).channelAdd(noise256.multiply(
                    0.4f));
            rock_bump.perturb(noise8, 0.1f);
            Layer rock = rubble.copy();
            rock.toHSV();
            rock.r = noise8.copy().dynamicRange(0.05f, 0.1f);
            rock.toRGB();
            rock.layerBlend(rubble.multiply(NATIVE_ROCK_TINT.x(), NATIVE_ROCK_TINT.y(), NATIVE_ROCK_TINT.z()),
                    noise8.gamma8().invert().contrast(4f));
            rock.layerBlend(grass.multiply(0.5f), noise8.rotate(90).multiply(0.5f)); // grass tint
            rock.bump(rock_bump, (float)(size / 192f), 0f, 1f, 1f, 1f, 1f, 0f, 0f, 0f);
            rock.gamma2().multiply(0.9f);
            if (DEBUG) rock.saveAsPNG("structure_rock");

            Channel mica = noise256.copy().gamma(0.5f).threshold(0.6f, 1.0f).multiply(0.8f);
            Layer normalMapLayer = rock_bump.toNormalMap(3.0f, mica);
            if (DEBUG) normalMapLayer.saveAsPNG("structure_rock_normal");

            return new StructureLayers(rock, normalMapLayer);
        }

        private static StructureLayers genCliff(int size, Channel noise8, Channel noise256,
                Channel voronoi4, Channel voronoi8, Channel voronoi16, Layer rubble,
                Layer grass) {
            Channel cliff_bump1 = voronoi4.dynamicRange().contrast(1.1f).brightness(2f).gamma2().multiply(0.3f);
            Channel cliff_bump2 = voronoi8.dynamicRange().contrast(1.1f).brightness(2f).gamma2().multiply(0.3f);
            Channel cliff_bump3 = voronoi16.dynamicRange().contrast(1.1f).brightness(2f).gamma2().multiply(0.25f);
            Channel cliff_bump = cliff_bump1.channelAdd(cliff_bump2).channelAdd(cliff_bump3).channelAdd(noise256.multiply(
                    0.15f));
            cliff_bump.dynamicRange().perturb(noise8, 0.1f);
            Layer cliff = rubble.copy();
            cliff.toHSV();
            cliff.r = noise8.copy().dynamicRange(0.05f, 0.1f);
            cliff.g.multiply(0.75f);
            cliff.toRGB();
            cliff.layerBlend(rubble, noise8.gamma8().invert().contrast(4f));
            cliff.layerBlend(grass.multiply(VIKING_CLIFF_GRASS_TINT.x(), VIKING_CLIFF_GRASS_TINT.y(),
                    VIKING_CLIFF_GRASS_TINT.z()), noise8.rotate(90).multiply(0.75f));
            cliff.bump(cliff_bump, (float)(size / 192f), 0f, 1f, 1f, 1f, 1f, 0f, 0f, 0f);
            cliff.gamma2().multiply(0.9f);
            if (DEBUG) cliff.saveAsPNG("structure_cliff");

            Channel zero = new Channel(size, size).fill(0f);
            return new StructureLayers(cliff, cliff_bump.toNormalMap(3.0f, zero));
        }

        private static StructureLayers genSnow(int size, Channel noise8, Channel noise256) {
            Channel empty = new Channel(size, size).fill(1f);
            Channel snow_bump1 = noise8.brightness(0.75f);
            Channel snow_bump2 = noise256.brightness(0.25f);
            Layer snow = new Layer(empty.copy().fill(VIKING_SNOW_COLOR.x()), empty.copy().fill(VIKING_SNOW_COLOR.y()),
                    empty.copy().fill(VIKING_SNOW_COLOR.z()));
            Channel bump = snow_bump1.channelAdd(snow_bump2);
            snow.bump(bump, (float)(size / 1024f), 0f, 0.1f, 1f, 1f, 1f, 0f, 0f, 0f);
            if (DEBUG) snow.saveAsPNG("structure_snow");

            Channel snowSpecular = snow_bump1.copy().multiply(0.2f); // Subtle specular based on roughness
            return new StructureLayers(snow, bump.toNormalMap(0.5f, snowSpecular));
        }

        private static StructureLayers genBlack() {
            return new StructureLayers(new Channel(1, 1).fill(0f).toLayer(), getFlatNormal(1));
        }

        private static StructureLayers genSeabottom(TerrainType terrain, int size,
                Channel noise8, Channel noise256, Channel voronoi4, Channel voronoi8) {
            var color = terrain == TerrainType.NATIVE ? NATIVE_SEA_BOTTOM_COLOR : VIKING_SEA_BOTTOM_COLOR;
            Layer seabottom = new Layer(
                    new Channel(size, size).fill(color.x()),
                    new Channel(size, size).fill(color.y()),
                    new Channel(size, size).fill(color.z()));
            if (DEBUG) seabottom.saveAsPNG("structure_seabottom");

            Channel zeroSpecular = new Channel(size, size).fill(0f);
            Layer normalMapLayer;

            switch (terrain) {
                case TerrainType.NATIVE: {
                    // Sand dunes/ripples
                    Channel seabottom_bump = noise8.copy().channelAdd(noise256.brightness(0.1f)).dynamicRange();
                    normalMapLayer = seabottom_bump.toNormalMap(0.3f, zeroSpecular); // Subtle sand dunes
                    break;
                }
                case TerrainType.VIKING: {
                    // Slime-covered, rock-strewn
                    // Combine Voronoi for larger rocks and noise for slime/undulations
                    Channel rock_texture = voronoi4.copy().dynamicRange(0.2f, 1f).multiply(0.7f); // Larger rock shapes
                    Channel slime_undulations = noise256.copy().brightness(0.08f); // Subtle slime undulations
                    Channel viking_seabottom_bump = rock_texture.channelAdd(slime_undulations).dynamicRange();

                    // Subtle wet specular for slime/wet rocks
                    Channel wetSpecular = voronoi4.copy().dynamicRange(0.0f, 0.15f); // Slightly more visible specular
                    normalMapLayer = viking_seabottom_bump.toNormalMap(1.2f, wetSpecular); // Stronger normal for rocks, subtle specular
                    break;
                }
                default: {
                    normalMapLayer = getFlatNormal(size); // Fallback
                    break;
                }
            }
            return new StructureLayers(seabottom, normalMapLayer);
        }

        private static StructureLayers genDetail(int size, float detail_alpha_value, int seed,
                Channel noise8) {
            Channel detail_noise = new Midpoint(size, 4, 0.4f, seed).toChannel();
            detail_noise.perturb(noise8.scale(size, size), 0.05f);
            Channel detail_grey = new Channel(size, size).fill(DETAIL_GREY);
            detail_grey.bump(detail_noise, (float)(size / 64f), 0f, 0f, 1f, 0f).dynamicRange();
            Channel detail_alpha = new Channel(size, size).fill(detail_alpha_value);
            Layer detail = new Layer(detail_grey, detail_grey, detail_grey, detail_alpha);
            if (DEBUG) detail.saveAsPNG("structure_detail");
            return new StructureLayers(detail, getFlatNormal(size));
        }

        private static Layer getFlatNormal(int size) {
            return new Layer(
                    new Channel(size, size).fill(0.5f),
                    new Channel(size, size).fill(0.5f),
                    new Channel(size, size).fill(1.0f),
                    new Channel(size, size).fill(0.0f) // Specular = 0
            );
        }


        // ***********
        // * TERRAIN *
        // ***********
        private void generateTerrainNative() {
            alpha_maps = new GLByteImage[7];

            // generate height map
            height = new Mountain(unit_grids_per_world, Utils.powerOf2Log2(unit_grids_per_world) - 6, 0.5f,
                    seed).toChannel().multiply(0.67f);
            Voronoi voronoi = new Voronoi(unit_grids_per_world, features, features, 1, 1f, seed);
            Channel cliffs = voronoi.getDistance(-1f, 1f, 0f).brightness(1.5f).multiply(0.33f);
            height.channelAdd(cliffs);

            // Fist of God (tm)
            if (unit_grids_per_world > 128) {
                height.channelSubtract(voronoi.getDistance(1f, 0f, 0f).gamma(.5f).flipV().rotate(90));
            } else {
                height.channelSubtract(voronoi.getDistance(-1f, 1f, 0f).gamma(.5f).flipV().rotate(90));
            }

            height.perturb(new Midpoint(unit_grids_per_world, 2, 0.5f, seed).toChannel(), 0.25f);
            Channel shape = new Hill(unit_grids_per_world, Hill.OVAL).toChannel();
            height.channelAdd(shape.copy().multiply(0.15f));
            height.channelSubtract(shape.copy().invert().multiply(0.5f));

            if (archipelago) {
                Channel sub_islands = new Voronoi(unit_grids_per_world, features, features, 1, 1f, seed).getDistance(-1.0f,
                        0.0f, 0.0f).gamma(0.11f);
                height.channelMultiply(sub_islands);
                if (DEBUG) sub_islands.toLayer().saveAsPNG("sub_islands");
            }

            height.erode((float)(((float)(24f - (float)(hills * 12f))) / unit_grids_per_world), J.Min(unit_grids_per_world >> 2, MAX_ERODE_ITERATIONS)); //added by ikill240c 2026-09-18
            height.channelMultiply(shape.gamma2());
            height.smooth(1);
            height = Landscape.beaches(height);

            // fix edges
            for (int y = 0; y < unit_grids_per_world; y += (unit_grids_per_world - 1)) {
                for (int x = 0; x < unit_grids_per_world; x++) {
                    height.putPixel(x, y, 0f);
                }
            }
            for (int y = 1; y < unit_grids_per_world - 1; y++) {
                for (int x = 0; x < unit_grids_per_world; x += (unit_grids_per_world - 1)) {
                    height.putPixel(x, y, 0f);
                }
            }

            generateWaterGrid();

            slope = height.copy().lineart();
            if (DEBUG) slope.copy().dynamicRange().toLayer().saveAsPNG("slope");
            relheight = height.copy().relativeIntensityNormalized(J.Max(1, unit_grids_per_world >> 5));
            if (DEBUG) relheight.toLayer().saveAsPNG("relheight");
            if (archipelago) {
                Channel inv = water_map.copy().invert();
                access = generateThresholdMap(slope, access_threshold).channelMultiply(inv);
            } else {
                access = generateThresholdMap(slope, access_threshold).largestConnected(1f);
            }
            access_exported = access.copy();
            if (DEBUG) access.toLayer().saveAsPNG("access");
            build = Landscape.generateBuildMap(generateThresholdMap(slope, build_threshold).channelMultiply(access));
        }

        private void generateTerrainViking() {
            alpha_maps = new GLByteImage[7];

            // generate height map
            height = new Mountain(unit_grids_per_world, Utils.powerOf2Log2(unit_grids_per_world) - 6, 0.5f,
                    seed).toChannel().add(.5f).dynamicRange().gamma2().multiply(0.67f);

            Voronoi voronoi = new Voronoi(unit_grids_per_world, 8, 8, 1, 1f, seed, true);
            Channel cliffs = voronoi.getDistance(-1f, 1f, 0f).brightness(1.25f).multiply(0.33f);
            height.channelAdd(cliffs).dynamicRange();

            // Fist of God (tm)
            Voronoi voronoi2 = new Voronoi(unit_grids_per_world, 4, 4, 1, 1f, seed);
            if (unit_grids_per_world > 128) {
                height.channelSubtract(voronoi2.getDistance(1f, 0f, 0f).gamma(.5f).multiply(.5f));
            } else {
                height.channelSubtract(voronoi2.getDistance(-1f, 1f, 0f).gamma(.5f).multiply(.5f));
            }

            Channel hitpoint = voronoi.getHitpoint().smooth(1);
            Channel hitpoint2 = hitpoint.copy().erodeThermal((float)(4f / unit_grids_per_world), J.Min(unit_grids_per_world >> 3, MAX_ERODE_THERMAL_ITERATIONS)); //added by ikill240c 2026-09-18
            Channel noise = new Midpoint(unit_grids_per_world, 3, 0.25f, seed).toChannel().threshold((float)(0.75f * hills), 1f);
            Channel heightcut = hitpoint.channelMultiply(noise.copy().invert()).channelAdd(hitpoint2.copy().channelMultiply(
                    noise));
            height.channelMultiply(heightcut);
            height.perturb(new Midpoint(unit_grids_per_world, 2, 0.5f, seed).toChannel(), 0.25f);

            if (archipelago) {
                Channel sub_islands = new Voronoi(unit_grids_per_world, features, features, 1, 1f, seed).getDistance(-1.0f,
                        0.0f, 0.0f).gamma(0.11f);
                height.channelMultiply(sub_islands);
                if (DEBUG) sub_islands.toLayer().saveAsPNG("sub_islands");
            }

            height.erode((float)(((float)(24f - (float)(hills * 12f))) / unit_grids_per_world), J.Min(unit_grids_per_world >> 2, MAX_ERODE_ITERATIONS)); //added by ikill240c 2026-09-18

            Channel shape = new Hill(unit_grids_per_world, Hill.SQUARE).toChannel().smoothGain().gamma8();
            height.channelMultiply(shape);

            // add roughness to inaccessible areas
            slope = height.copy().lineart();
            Channel peakarea = slope.threshold(0f, access_threshold).largestConnected(1f).invert().channelMultiply(
                    hitpoint2);
            Channel peaks = new Midpoint(unit_grids_per_world, 4, 0.75f, 42).toChannel().channelMultiply(peakarea).multiply(
                    .1f);
            height.channelAdd(peaks);
            height.smooth(1);

            // fix edges
            for (int y = 0; y < unit_grids_per_world; y += (unit_grids_per_world - 1)) {
                for (int x = 0; x < unit_grids_per_world; x++) {
                    height.putPixel(x, y, 0f);
                }
            }
            for (int y = 1; y < unit_grids_per_world - 1; y++) {
                for (int x = 0; x < unit_grids_per_world; x += (unit_grids_per_world - 1)) {
                    height.putPixel(x, y, 0f);
                }
            }

            generateWaterGrid();

            slope = height.copy().lineart();
            if (DEBUG) slope.copy().dynamicRange().toLayer().saveAsPNG("slope");
            relheight = height.copy().relativeIntensityNormalized(J.Max(1, unit_grids_per_world >> 5));
            if (DEBUG) relheight.toLayer().saveAsPNG("relheight");
            if (archipelago) {
                Channel inv = water_map.copy().invert();
                access = generateThresholdMap(slope, access_threshold).channelMultiply(inv);
            } else {
                access = generateThresholdMap(slope, access_threshold).largestConnected(1f);
            }
            access_exported = access.copy();
            if (DEBUG) access.toLayer().saveAsPNG("access");
            build = Landscape.generateBuildMap(generateThresholdMap(slope, build_threshold).channelMultiply(access));
        }

        /**
         * Populates height/slope/relheight/access/access_exported/build/water_map/dock_map/
         * water/dock from an authored (hand-painted) height Channel instead of noise, used in place
         * of generateTerrainNative()/generateTerrainViking() when authored_terrain != null. Mirrors
         * the tail of those two methods EXACTLY (from generateWaterGrid() onward) so authored and
         * procedural maps get identical derived data through identical code - the only difference is
         * where `height` itself comes from.
         *
         * IMPORTANT: generateWaterGrid() is NOT optional here even though it looks unrelated to
         * "height painting" - it populates water_map/dock_map AND the water/dock sbyte grids that
         * getWaterGrid()/getDockGrid() return, purely by deriving from `height`. Skipping it would
         * leave those fields null and getWaterGrid()/getDockGrid() would fail. //added by ikill240c
         */
        private void loadAuthoredHeight() { //added by ikill240c
            // Missing this line is exactly what caused the NullReferenceException in generateAlphas() -
            // alpha_maps is allocated as the very first line of BOTH generateTerrainNative() and
            // generateTerrainViking() (not anywhere in the shared derivation tail this method mirrors
            // from generateWaterGrid() onward), so replacing those two methods entirely without this
            // line left alpha_maps null going into generateAlphas(), which unconditionally writes
            // alpha_maps[0] through [3]. //added by ikill240c
            alpha_maps = new GLByteImage[7]; //added by ikill240c
            height = authored_terrain.getHeightChannel();
            // Same edge-zeroing generateTerrainNative()/Viking() apply to their noise-generated
            // height before deriving anything from it, applied here too so an authored map gets the
            // same guaranteed-water border the rest of the engine assumes exists at the map edge,
            // regardless of what the editor's own tools did or didn't enforce.
            for (int y = 0; y < unit_grids_per_world; y += (unit_grids_per_world - 1)) {
                for (int x = 0; x < unit_grids_per_world; x++) {
                    height.putPixel(x, y, 0f);
                }
            }
            for (int y = 1; y < unit_grids_per_world - 1; y++) {
                for (int x = 0; x < unit_grids_per_world; x += (unit_grids_per_world - 1)) {
                    height.putPixel(x, y, 0f);
                }
            }

            generateWaterGrid();

            slope = height.copy().lineart();
            relheight = height.copy().relativeIntensityNormalized(J.Max(1, unit_grids_per_world >> 5));
            // Authored maps don't support archipelago mode (no sub-island noise to react to), so this
            // always takes the non-archipelago branch that generateTerrainNative()/Viking() use.
            access = generateThresholdMap(slope, access_threshold).largestConnected(1f);
            access_exported = access.copy();
            build = Landscape.generateBuildMap(generateThresholdMap(slope, build_threshold).channelMultiply(access));
        }

        // shape beaches
        private static Channel beaches(Channel channel) {
            float sealevel = (float)(1.1f * Globals.SEA_LEVEL);
            float threshold = (float)(2f * sealevel);
            for (int y = 0; y < channel.height; y++) {
                for (int x = 0; x < channel.width; x++) {
                    float value = channel.getPixel(x, y);
                    if (value < sealevel) {
                        value = Tools.interpolateSmooth(0, sealevel, (float)(value / sealevel));
                        channel.putPixel(x, y, value);
                    } else if (value < threshold) {
                        value = Tools.interpolateSmooth(sealevel, (float)((float)(2f * threshold) - sealevel),
                                (float)((float)(0.5f * ((float)(value - sealevel))) / ((float)(threshold - sealevel))));
                        channel.putPixel(x, y, value);
                    }
                }
            }
            return channel;
        }

        // generate threshold map
        private Channel generateThresholdMap(Channel slopemap, float threshold) {
            Channel channel = slopemap.copy().threshold(0f, threshold).channelSubtract(height.copy().threshold(0f,
                    Globals.SEA_LEVEL));
            // fix edges
            for (int y = 0; y < unit_grids_per_world; y += (unit_grids_per_world - 1)) {
                for (int x = 0; x < unit_grids_per_world; x++) {
                    channel.putPixel(x, y, 0f);
                }
            }
            for (int y = 1; y < unit_grids_per_world - 1; y++) {
                for (int x = 0; x < unit_grids_per_world; x += (unit_grids_per_world - 1)) {
                    channel.putPixel(x, y, 0f);
                }
            }
            return channel;
        }

        // generate build map
        private static sbyte[][] generateBuildMap(Channel thresholdmap) {
            if (DEBUG) thresholdmap.toLayer().saveAsPNG("build_tresholdmap");
            int size = thresholdmap.getWidth();
            bool[][] build_grid = J.New2<bool>(size, size);
            for (int y = 0; y < size; y++) {
                for (int x = 0; x < size; x++) {
                    build_grid[y][x] = thresholdmap.getPixel(x, y) > 0;
                }
            }
            sbyte[][] byte_grid = J.New2<sbyte>(build_grid.Length, build_grid[0].Length);
            sbyte max = J.B(RacesResources.MAX_BUILDING_SIZE);
            for (sbyte i = 0; i < max; i++) {
                for (int y = 1; y < build_grid.Length - 1; y++) {
                    for (int x = 1; x < build_grid[y].Length - 1; x++) {
                        if (!build_grid[y][x] && byte_grid[y][x] == i) {
                            for (int k = -1; k <= 1; k++) {
                                for (int l = -1; l <= 1; l++) {
                                    if (build_grid[y + k][x + l]) {
                                        build_grid[y + k][x + l] = false;
                                        byte_grid[y + k][x + l] = J.B((i + 1));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            for (int y = 1; y < build_grid.Length - 1; y++) {
                for (int x = 1; x < build_grid[y].Length - 1; x++) {
                    if (build_grid[y][x])
                        byte_grid[y][x] = max;
                }
            }
            return byte_grid;
        }


        // **********
        // * ALPHAS *
        // **********
        private Channel generateAlphas() {
            int seed = Globals.LANDSCAPE_SEED;
            Channel alpha0, alpha1, alpha2, alpha3;
            Channel grass_alpha;
            if (terrain == TerrainType.NATIVE) {
                    alpha0 = generateDirtAlpha();
                    if (DEBUG) alpha0.toLayer().saveAsPNG("alpha_dirt");
                    alpha1 = generateRubbleAlpha();
                    if (DEBUG) alpha1.toLayer().saveAsPNG("alpha_rubble");
                    alpha2 = Landscape.generateRockAlpha(slope, access_threshold);
                    if (DEBUG) alpha2.toLayer().saveAsPNG("alpha_rock");
                    alpha3 = generateGrassAlpha(unit_grids_per_world, seed);
                    if (DEBUG) alpha3.toLayer().saveAsPNG("alpha_grass");
                    alpha_maps[0] = new GLByteImage(alpha0, GL11.GL_RED);
                    alpha_maps[1] = new GLByteImage(alpha1, GL11.GL_RED);
                    alpha_maps[2] = new GLByteImage(alpha2, GL11.GL_RED);
                    alpha_maps[3] = new GLByteImage(alpha3, GL11.GL_RED);
                grass_alpha = alpha3;
            } else {
                    alpha0 = generateSoilAlpha();
                    if (DEBUG) alpha0.toLayer().saveAsPNG("alpha_soil");
                    alpha1 = Landscape.generateCliffAlpha(slope, access_threshold);
                    if (DEBUG) alpha1.toLayer().saveAsPNG("alpha_cliff");
                    alpha2 = generateGrassAlpha(unit_grids_per_world, seed);
                    if (DEBUG) alpha2.toLayer().saveAsPNG("alpha_grass");
                    alpha3 = Landscape.generateSnowAlpha(height, alpha1.copy());
                    if (DEBUG) alpha3.toLayer().saveAsPNG("alpha_snow");
                    alpha_maps[0] = new GLByteImage(alpha0, GL11.GL_RED);
                    alpha_maps[1] = new GLByteImage(alpha1, GL11.GL_RED);
                    alpha_maps[2] = new GLByteImage(alpha2, GL11.GL_RED);
                    alpha_maps[3] = new GLByteImage(alpha3, GL11.GL_RED);
                grass_alpha = alpha2;
            }

            Channel seabottom_alpha = Landscape.generateSeabottomAlpha(terrain, height, height_scale);
            if (DEBUG) seabottom_alpha.toLayer().saveAsPNG("alpha_seabottom");

            // generate shadow and highlight alpha
            shadow = new Channel(unit_grids_per_world, unit_grids_per_world);
            highlight = new Channel(unit_grids_per_world, unit_grids_per_world);
            float lx = 1;
            float lz = 1;
            float lnorm = (float)(1f / (float) J.Sqrt((float)((float)(lx * lx) + (float)(lz * lz))));
            lx = (float)(lx * lnorm);
            lz = (float)(lz * lnorm);
            float threshold = (float) J.Sqrt(0.5f);
            float nz = (float)((float)(2f * meters_per_height_unit) / height_scale);
            float nzlz = (float)(nz * lz);
            float nz2 = (float)(nz * nz);
            for (int y = 0; y < unit_grids_per_world; y++) {
                for (int x = 0; x < unit_grids_per_world; x++) {
                    float nx = (float)(height.getPixelWrap(x + 1, y) - height.getPixelWrap(x - 1, y));
                    float ny = (float)(height.getPixelWrap(x, y + 1) - height.getPixelWrap(x, y - 1));
                    float light = (float)(((float)((float)(nx * lx) + nzlz)) / ((float) J.Sqrt((float)((float)((float)(nx * nx) + (float)(ny * ny)) + nz2)))); // Can use Math here - calculation is not game state affecting
                    if (light > threshold) {
                        highlight.putPixel(x, y, light);
                        shadow.putPixel(x, y, threshold);
                    } else {
                        highlight.putPixel(x, y, threshold);
                        shadow.putPixel(x, y, J.Max(0, light));
                    }
                }
            }
            highlight.dynamicRange(0f, 0.25f);
            shadow.invert().dynamicRange(0f, 0.75f);
            ProgressForm.progress((float)(1 / 14f));

            // generate shadowcasting
            Channel shadowcast = new Channel(unit_grids_per_world, unit_grids_per_world);
            float val = 0;
            float peak = 0;
            float descent = (float)(8f / unit_grids_per_world);
            for (int y = 0; y < unit_grids_per_world; y++) {
                for (int x = 0; x < unit_grids_per_world; x++) {
                    val = height.getPixel(x, y);
                    peak = (float)(peak - descent);
                    if (peak > val) {
                        shadowcast.putPixel(x, y, 1f);
                    } else {
                        peak = val;
                    }
                }
                peak = 0;
            }
            shadow.channelBrightest(shadowcast.smooth(1).brightness(0.67f));

            // Submerged terrain lit like dry land reads as shadowed cliffs through clear water; fade the bake out
            float shading_band = (float)(SEABOTTOM_DEPTH_OFFSET_METERS / height_scale);
            for (int y = 0; y < unit_grids_per_world; y++) {
                for (int x = 0; x < unit_grids_per_world; x++) {
                    float h = height.getPixel(x, y);
                    if (h < Globals.SEA_LEVEL) {
                        float fade = J.Clamp((float)(((float)(h - ((float)(Globals.SEA_LEVEL - shading_band)))) / shading_band), 0f, 1f);
                        float factor = (float)(UNDERWATER_SHADING + (float)(((float)(1f - UNDERWATER_SHADING)) * fade));
                        shadow.putPixel(x, y, (float)(shadow.getPixel(x, y) * factor));
                        highlight.putPixel(x, y, (float)(highlight.getPixel(x, y) * factor));
                    }
                }
            }
            if (DEBUG) shadow.toLayer().saveAsPNG("alpha_shadow");
            ProgressForm.progress((float)(1 / 14f));

            alpha_maps[6] = new GLByteImage(seabottom_alpha, GL11.GL_RED);

            return grass_alpha;
        }

        // generate dirt alpha
        private Channel generateDirtAlpha() {
            Channel dirt_alpha = height.copy().dynamicRange((float)(1.1f * Globals.SEA_LEVEL), (float)(2f * Globals.SEA_LEVEL), 0f, 1f);
            dirt_alpha.channelSubtract(relheight.copy().invert().dynamicRange(0.5f, 0.6f, 0f, 0.5f));
            return dirt_alpha;
        }

        // generate rubble alpha
        private Channel generateRubbleAlpha() {
            Channel rubble_alpha = slope.copy().dynamicRange(build_threshold, access_threshold, 0f, 1f);
            rubble_alpha.channelSubtract(height.copy().invert().dynamicRange(0.8f, 1f, 0f, 1f));
            rubble_alpha.channelSubtract(relheight.copy().invert().dynamicRange(0.5f, 0.65f, 0f, 0.5f));
            return rubble_alpha;
        }

        // generate rock alpha
        private static Channel generateRockAlpha(Channel slope, float access_threshold) {
            Channel rock_alpha = slope.copy().threshold(access_threshold, 1f);
            return rock_alpha;
        }

        // generate grass alpha
        private Channel generateGrassAlpha(int size, int seed) {
            Channel grass_alpha = new Midpoint(size, 4, 0.45f, seed).toChannel().dynamicRange((float)(1f - vegetation_amount), 1f,
                    0f, 1f).gamma2();
            grass_alpha.channelBrightest(slope.copy().dynamicRange(0f, access_threshold, 0f, 1f).invert().dynamicRange(
                    (float)(1f - vegetation_amount), 1f, 0f, 1f).gamma2());
            grass_alpha.channelAdd(relheight.copy().invert().add(-0.5f).multiply(2f));
            grass_alpha.channelSubtract(height.copy().invert().dynamicRange(0.6f, 0.8f, 0f, 1f));
            grass_alpha.channelSubtract(slope.copy().threshold((float)(0.75f * access_threshold), 1f).smooth(3));
            grass_alpha.channelSubtract(relheight.copy().invert().dynamicRange(0.5f, 0.7f, 0f, 0.5f));
            return grass_alpha;
        }

        // generate soil alpha
        private Channel generateSoilAlpha() {
            Channel soil_alpha = height.copy().dynamicRange((float)(1.1f * Globals.SEA_LEVEL), (float)(2f * Globals.SEA_LEVEL), 0f, 1f);
            soil_alpha.channelSubtract(relheight.copy().invert().dynamicRange(0.5f, 0.6f, 0f, 0.5f));
            return soil_alpha;
        }

        // generate cliff alpha
        private static Channel generateCliffAlpha(Channel slope, float access_threshold) {
            Channel cliff_alpha = slope.copy().threshold(access_threshold, 1f);
            return cliff_alpha;
        }

        // generate snow alpha
        private static Channel generateSnowAlpha(Channel height, Channel cliff_alpha) {
            Channel snow_alpha = height.copy().dynamicRange(0.5f, 0.6f, 0f, 1f);
            snow_alpha.channelSubtract(cliff_alpha);
            snow_alpha.smooth(1).smooth(1);

            return snow_alpha;
        }

        // generate seabottom alpha
        private static Channel generateSeabottomAlpha(TerrainType terrain, Channel height,
                int height_scale) {
            // Start the tint below the waterline so shallow water seen through transparent water stays sand colored
            float depth_offset = (float)(SEABOTTOM_DEPTH_OFFSET_METERS / height_scale);
            Channel seabottom_alpha = height.copy().invert().dynamicRange((float)((float)(1f - Globals.SEA_LEVEL) + depth_offset), 1f, 0f,
                    1f);
            return seabottom_alpha.gamma(0.5f); // same for NATIVE and VIKING
        }

        // ************
        // * SUPPLIES *
        // ************
        private void generateSupplies(Channel grass_alpha) {
            // generate overall probability map for rock and iron resources
            Channel centerprob = new Hill(unit_grids_per_world, Hill.CIRCLE).toChannel().addClip(-.5f).dynamicRange();

            // generate (oak)tree/palmtree(/pine) maps
            Channel noise = new Midpoint(unit_grids_per_world, Utils.powerOf2Log2(unit_grids_per_world) - 3, 0.33f,
                    seed).toChannel();
            Channel tree_channel = grass_alpha.copy();
            Channel palmtree_channel = height.copy();
            ProgressForm.progress((float)(1 / 14f));

            switch (terrain) {
                case TerrainType.NATIVE: {
                    tree_channel.threshold(0.5f, 1f);
                    tree_channel.channelAdd(noise.rotate(90).copy().threshold(0.9f, 1f));
                    tree_channel.channelSubtract(slope.copy().threshold(access_threshold, 1f));
                    tree_channel.channelSubtract(noise.copy().dynamicRange(0.1f, 1f));

                    palmtree_channel.invert().dynamicRange(0.6f, 0.89f, 0f, 1f);
                    palmtree_channel.channelSubtract(height.copy().invert().dynamicRange(0.89f, 0.9f, 0f, 1f));
                    palmtree_channel.channelSubtract(noise.rotate(90)).channelAdd(noise.rotate(90).copy().threshold(0.9f,
                            1f)).channelSubtract(slope.copy().threshold(access_threshold, 1f));
                    break;
                }
                case TerrainType.VIKING: {
                    tree_channel.gamma8();
                    tree_channel.channelMultiply(height.copy().dynamicRange(0.55f, 0.65f, 1f, 0f));
                    tree_channel.channelSubtract(slope.copy().threshold(access_threshold, 1f));
                    tree_channel.channelSubtract(noise.copy());

                    palmtree_channel = grass_alpha.copy().channelMultiply(height.copy().dynamicRange(0.5f, 0.6f, 1f,
                            0f)).invert();
                    palmtree_channel.channelSubtract(height.copy().invert().dynamicRange(0.8f, 0.875f, 0f, 1f));
                    palmtree_channel.channelSubtract(slope.copy().threshold(access_threshold, 1f));
                    palmtree_channel.channelSubtract(noise.rotate(90));
                    break;
                }
            }

            if (DEBUG) tree_channel.toLayer().saveAsPNG("supplies_trees");
            if (DEBUG) palmtree_channel.toLayer().saveAsPNG("supplies_palmtrees");
            ProgressForm.progress((float)(1 / 14f));

            // generate rock and iron supplies map
            Channel rock_channel = relheight.copy();

            switch (terrain) {
                case TerrainType.NATIVE: rock_channel.invert().threshold(0.5f, 1f).channelMultiply(noise.rotate(
                        90).copy().gamma8().invert()).channelMultiply(centerprob); break;
                case TerrainType.VIKING: rock_channel.threshold(0f, 0.5f).channelMultiply(noise.rotate(90).copy().gamma8().invert()); break;
            }

            Channel iron_channel = rock_channel.copy().rotate(90).flipV();
            if (DEBUG) rock_channel.toLayer().saveAsPNG("supplies_rocks");
            if (DEBUG) iron_channel.toLayer().saveAsPNG("supplies_iron");

            Channel supplies = access.copy();
            float accessible = supplies.sum();

            // place trees
            trees = placeSupplies(tree_channel, supplies, 64, J.I(((float)((float)(vegetation_amount * max_trees) * ((float)(accessible / area))))),
                    0.33f);
            access.channelSubtract(trees);
            if (DEBUG) trees.toLayer().saveAsPNG("supplies_trees_placed");

            // place palmtrees
            palmtrees = placeSupplies(palmtree_channel, supplies, 64,
                    J.I(((float)((float)(vegetation_amount * max_palmtrees) * ((float)(accessible / area))))), 0.25f);
            access.channelSubtract(palmtrees);
            if (DEBUG) palmtrees.toLayer().saveAsPNG("supplies_palmtrees_placed");

            // place rock
            rock = placeSupplies(rock_channel, supplies, 64, J.I(((float)(supplies_amount * max_rock))), 0f);
            access.channelSubtract(rock);
            shadow.channelBrightest(rock.copy().multiply(0.5f));
            if (DEBUG) rock.toLayer().saveAsPNG("supplies_rock_placed");

            // place iron
            iron = placeSupplies(iron_channel, supplies, 64, J.I(((float)(supplies_amount * max_iron))), 0f);
            access.channelSubtract(iron);
            shadow.channelBrightest(iron.copy().multiply(0.5f));
            if (DEBUG) iron.toLayer().saveAsPNG("supplies_iron_placed");

            if (DEBUG) {
                // IO.println("Number of trees placed: " + trees.count(1f));
                // IO.println("Number of palmtrees placed: " + palmtrees.count(1f));
                // IO.println("Number of rocks placed: " + rock.count(1f));
                // IO.println("Number of iron ore placed: " + iron.count(1f));
            }

            // place extra supplies around starting locations
            int num_rock = 2;
            int num_iron = 1;
            for (int p = 0; p < num_players; p++) {
                for (int r = 0; r < num_rock; r++) {
                    int[] location = access.find(J.Min(unit_grids_per_world >> 1, MAX_PLACEMENT_SEARCH_RADIUS), supply_locations[p][0],
                            supply_locations[p][1], 1f);
                    rock.putPixel(location[0], location[1], 1f);
                    access.putPixel(location[0], location[1], 0f);
                }
                for (int i = 0; i < num_iron; i++) {
                    int[] location = access.find(J.Min(unit_grids_per_world >> 1, MAX_PLACEMENT_SEARCH_RADIUS), supply_locations[p][0],
                            supply_locations[p][1], 1f);
                    iron.putPixel(location[0], location[1], 1f);
                    access.putPixel(location[0], location[1], 0f);
                }
            }

            // shadow and highlight are changed by supply placement
            alpha_maps[4] = new GLByteImage(highlight, GL11.GL_RED);
            alpha_maps[5] = new GLByteImage(shadow, GL11.GL_RED);
            ProgressForm.progress((float)(1 / 14f));

            // generate plant maps
            plants = J.New2<float>(NUM_PLANT_TYPES, max_plants << 1);
            tree_channel.channelBrightest(palmtree_channel).brightness(.5f);
            noise.scaleFast(noise.width >> 1, noise.height >> 1).tileDouble();

            Channel plantsmap = grass_alpha.copy();
            plantsmap.multiply(0.25f).add(0.75f);
            plantsmap.channelMultiply(slope.copy().threshold(0f, access_threshold));
            plantsmap.channelMultiply(height.copy().threshold(Globals.SEA_LEVEL, 1f));

            Channel plants1 = plantsmap.copy().channelMultiply(noise.copy());
            Channel plants2 = plantsmap.copy().channelMultiply(noise.rotate(90));
            Channel plants3 = plantsmap.copy().channelMultiply(noise.rotate(90));
            Channel plants4 = plantsmap.copy().channelMultiply(noise.rotate(90));
            Channel place = new Channel(unit_grids_per_world, unit_grids_per_world);
            place = placePlants(plants1, place, 64, max_plants >> 2, 0);
            place = placePlants(plants2, place, 64, max_plants >> 2, 1);
            place = placePlants(plants3, place, 64, max_plants >> 2, 2);
            place = placePlants(plants4, place, 64, max_plants >> 2, 3);
        }

        /**
         * Populates trees/palmtrees/rock/iron from authored_terrain's placed resource list instead of
         * the procedural probability-map scatter in generateSupplies(), used in place of that method
         * when authored_terrain != null. Skips the entire noise-driven probability-map machinery
         * (Hill/Midpoint channels, native/viking-specific tree/rock probability shaping, the "extra
         * guaranteed resources near start" logic) since none of that applies once placement is
         * already decided by hand.
         *
         * IMPORTANT, found only by tracing generateSupplies() in full: alpha_maps[4]/alpha_maps[5]
         * and the `plants` field are BOTH only ever assigned inside generateSupplies() itself, never
         * in generateAlphas() (which only initializes the underlying shadow/highlight Channels, not
         * the GLByteImage wrappers or the plants array). Skipping generateSupplies() entirely without
         * setting these explicitly would leave them null - and the constructor's blend_infos array
         * construction, and getPlants(), both dereference them unconditionally right after this
         * runs. //added by ikill240c
         */
        private void loadAuthoredSupplies() { //added by ikill240c
            trees = new Channel(unit_grids_per_world, unit_grids_per_world);
            palmtrees = new Channel(unit_grids_per_world, unit_grids_per_world);
            rock = new Channel(unit_grids_per_world, unit_grids_per_world);
            iron = new Channel(unit_grids_per_world, unit_grids_per_world);
            int placed = 0; //added by ikill240c
            int skipped = 0; //added by ikill240c
            foreach (AuthoredTerrain.ResourceNode node in authored_terrain.getResources()) {
                Channel target = node.type() switch {
                    AuthoredTerrain.ResourceType.TREE => trees,
                    AuthoredTerrain.ResourceType.PALM_TREE => palmtrees,
                    AuthoredTerrain.ResourceType.ROCK => rock,
                    _ => iron,
                };
                // Skip entirely if this cell isn't actually available - either because a player
                // start/quarters/armory footprint already claimed it (access already reflects that,
                // since generateUnitLocations() runs before this method), it's off the map, or an
                // earlier resource in THIS SAME LOOP already placed something here (e.g. two authored
                // nodes landing on the same or an adjacent cell, easy to do by clicking twice in the
                // editor). Without this check, two game objects trying to occupy the same grid cell
                // crashes later in World's constructor with an AssertionError - two objects can never
                // share one cell. getPixelSafe() (not getPixel()) throughout, since an authored
                // coordinate from a hand-edited or corrupted file could be out of bounds, and this
                // should skip that gracefully rather than throw. //added by ikill240c
                if (access.getPixelSafe(node.x(), node.y()) <= 0f //added by ikill240c
                        || trees.getPixelSafe(node.x(), node.y()) > 0f //added by ikill240c
                        || palmtrees.getPixelSafe(node.x(), node.y()) > 0f //added by ikill240c
                        || rock.getPixelSafe(node.x(), node.y()) > 0f //added by ikill240c
                        || iron.getPixelSafe(node.x(), node.y()) > 0f) { //added by ikill240c
                    skipped++; //added by ikill240c
                    continue; //added by ikill240c
                }
                // getPositions() (backing getTrees()/getRock()/etc.) scans for pixels valued exactly
                // 1f, so this must write exactly 1f, not some other truthy value.
                target.putPixelSafe(node.x(), node.y(), 1f);
                placed++; //added by ikill240c
            }
            if (skipped > 0) { //added by ikill240c
                // IO.println("Authored resources: " + placed + " placed, " + skipped + " skipped"); //added by ikill240c
            } //added by ikill240c
            access.channelSubtract(trees);
            access.channelSubtract(palmtrees);
            access.channelSubtract(rock);
            access.channelSubtract(iron);
            shadow.channelBrightest(rock.copy().multiply(0.5f));
            shadow.channelBrightest(iron.copy().multiply(0.5f));

            // Required fix - see method comment above: without these two lines, both the
            // blend_infos construction just after this method runs and getPlants() afterward would
            // throw a NullReferenceException. //added by ikill240c
            alpha_maps[4] = new GLByteImage(highlight, GL11.GL_RED);
            alpha_maps[5] = new GLByteImage(shadow, GL11.GL_RED);
            // No decorative plants for authored maps yet - an empty (all-zero) array is a valid,
            // safe "zero plants placed" state for getPlants()'s consumers.
            plants = J.New2<float>(NUM_PLANT_TYPES, max_plants << 1);
        }

        // place supplies on map
        private Channel placeSupplies(Channel probability, Channel supplies, int intervals,
                int max_count, float shadow_alpha_val) {
            max_count = J.Min(probability.width * probability.height, max_count);
            int scaleshift = Utils.powerOf2Log2(unit_grids_per_world * meters_per_height_unit / meters_per_world);
            int i = 0;
            float interval_size = (float)(1f / intervals);
            float upper_bound = 1f;
            float lower_bound = (float)(upper_bound - interval_size);
            int supplyshadow_size = J.Max(unit_grids_per_world >> 7, 2);
            Channel supplyshadow_alpha = new Channel(supplyshadow_size << 1, supplyshadow_size << 1).place(new Channel(
                    supplyshadow_size, supplyshadow_size).fill(1f), supplyshadow_size >> 1,
                    supplyshadow_size >> 1).smoothFast();
            Channel supplyshadow = new Channel(supplyshadow_size << 1, supplyshadow_size << 1);
            Channel supplyshadow_alpha2 = supplyshadow_alpha.copy().brightness(shadow_alpha_val);
            Channel place = new Channel(probability.width, probability.height);

            // place supplies
            while (i < max_count && lower_bound > interval_size) {
                for (int y = 1; y < probability.height - 1; y++) {
                    for (int x = 1; x < probability.width - 1; x++) {
                        float val = probability.getPixel(x, y);
                        if (val <= upper_bound && val > lower_bound) {
                            // place resource if grid and its 4 neighbours are unoccupied
                            if (supplies.getPixel(x, y) > 0
                                    && supplies.getPixel(x - 1, y) > 0
                                    && supplies.getPixel(x + 1, y) > 0
                                    && supplies.getPixel(x, y - 1) > 0
                                    && supplies.getPixel(x, y - 1) > 0
                            ) {
                                place.putPixel(x, y, 1f);
                                // place shadow
                                if (shadow_alpha_val > 0f) {
                                    int x_pixel = (x - supplyshadow_size + 1) >> scaleshift;
                                    int y_pixel = (y - supplyshadow_size + 1) >> scaleshift;
                                    highlight.place(supplyshadow, supplyshadow_alpha, x_pixel, y_pixel);
                                    shadow.placeBrightest(supplyshadow_alpha2, x_pixel, y_pixel);
                                }
                                // make node neighbourhood inaccessible
                                for (int k = -1; k <= 1; k++) {
                                    for (int l = -1; l <= 1; l++) {
                                        supplies.putPixelWrap(x + k, y + l, 0f);
                                    }
                                }

                                i++;
                                if (i >= max_count) goto out_end;
                            }
                        }
                    }
                }
                lower_bound = (float)(lower_bound - (interval_size));
                upper_bound = (float)(upper_bound - (interval_size));
            }
        out_end:
            return place;
        }

        // place plants on map
        private Channel placePlants(Channel probability, Channel place, int intervals,
                int max_count, int plant_type) {
            max_count = J.Min(probability.width * probability.height, max_count);
            int i = 0;
            float interval_size = (float)(1f / intervals);
            float upper_bound = 1f;
            float lower_bound = (float)(upper_bound - interval_size);

            // place plants
            while (i < max_count && lower_bound > interval_size) {
                for (int y = 2; y < probability.height - 2; y++) {
                    for (int x = 2; x < probability.width - 2; x++) {
                        float val = probability.getPixel(x, y);
                        if (val <= upper_bound && val > lower_bound && place.getPixel(x, y) < 2
                                && random.NextFloat() < .25) {
                            // place plant
                            plants[plant_type][2 * i] = (float)(meters_per_height_unit * ((float)(x + random.NextFloat())));
                            plants[plant_type][2 * i + 1] = (float)(meters_per_height_unit * ((float)(y + random.NextFloat())));

                            place.putPixelWrap(x, y, (float)(place.getPixel(x, y) + 1f));

                            /*
                            // make node neighbourhood inaccessible
                            for (int k = -1; k <= 1; k++) {
                            	for (int l = -1; l <= 1; l++) {
                            		place.putPixelWrap(x + k, y + l, place.getPixel(x + k, y + l) + 1f);
                            	}
                            }
                            */

                            i++;
                            if (i >= max_count) goto out_end;
                        }
                    }
                }
                lower_bound = (float)(lower_bound - (interval_size));
                upper_bound = (float)(upper_bound - (interval_size));
            }
        out_end:
            return place;
        }


        // ******************
        // * UNIT LOCATIONS *
        // ******************
        private void generateUnitLocations(int initial_unit_count, float random_start_pos) {
            // create building placement map
            Channel buildmap = new Channel(access.width, access.height);
            Channel buildmap_debug = new Channel(access.width, access.height);
            for (int y = 0; y < access.height; y++) {
                for (int x = 0; x < access.width; x++) {
                    if (build[y][x] == RacesResources.QUARTERS_SIZE) {
                        buildmap.putPixel(x, y, 1f);
                    }
                    if (DEBUG) buildmap_debug.putPixel(x, y, (float)(.2f * build[y][x]));
                }
            }
            if (DEBUG) buildmap.toLayer().saveAsPNG("buildmap");
            if (DEBUG) buildmap_debug.toLayer().saveAsPNG("buildmap_debug");
            // find initial starting locations
            player_locations = J.New2<float>(num_players, 2 * initial_unit_count);
            supply_locations = J.New2<int>(num_players, 2);
            float angle = (float)(0.5f * (float) J.PI);
            angle = (float)(angle + ((float)((float)(random_start_pos * (float) J.PI) * 2))); // random start for multiplayer games
            float angle_step = (float)((float)(2f * (float) J.PI) / num_players);
            float radius = (float)(0.35f * unit_grids_per_world);
            int scale = meters_per_world / unit_grids_per_world;
            int[] location_quarters = new int[2];
            int[] location_armory = new int[2];
            for (int i = 0; i < num_players; i++) {
                int x = J.I(((float)((float)((float)(radius * (float) J.Cos(angle)) + (unit_grids_per_world >> 1)) + 0.5f)));
                int y = J.I(((float)((float)((float)(radius * (float) J.Sin(angle)) + (unit_grids_per_world >> 1)) + 0.5f)));
                angle = (float)(angle + (angle_step));
                // Authored start override - falls back to the procedural circular position above if
                // this player index has no authored start (e.g. the map was painted for fewer
                // players than the lobby chose), rather than crashing outright. The footprint
                // clearing and initial-unit placement below this point are identical either way -
                // only the candidate x/y source differs. //added by ikill240c
                if (authored_terrain != null) { //added by ikill240c
                    foreach (AuthoredTerrain.StartPosition start in authored_terrain.getStarts()) { //added by ikill240c
                        if (start.player_index() == i) { //added by ikill240c
                            x = start.x(); //added by ikill240c
                            y = start.y(); //added by ikill240c
                            break; //added by ikill240c
                        } //added by ikill240c
                    } //added by ikill240c
                } //added by ikill240c
                location_quarters = buildmap.findNoWrap(J.Min(unit_grids_per_world >> 1, MAX_PLACEMENT_SEARCH_RADIUS), x, y, 1f);
                // findNoWrap() returns the {-1, -1} sentinel if it searched the whole radius around
                // (x, y) without finding any buildable spot at all, clipped at the map edge - possible
                // with a start position near the edge of a small/custom-generated map. Falling through
                // with -1, -1 used as a real position doesn't just look wrong: passed into the wrapping
                // find() below for the armory search, it would wrap to the opposite corner of the map
                // and could still land somewhere illegal, or previously crashed outright before find()
                // was made wrap-safe (see Channel.find()). Retrying with the wrapping find() - which
                // searches strictly more area than findNoWrap did, since it continues across the map
                // edge instead of stopping there - gives this a real chance of finding a valid spot
                // instead of just reproducing the same failure. //added by ikill240c 2026-09-12
                if (location_quarters[0] == -1 && location_quarters[1] == -1) { //added by ikill240c 2026-09-12
                    location_quarters = buildmap.find(J.Min(unit_grids_per_world >> 1, MAX_PLACEMENT_SEARCH_RADIUS), x, y, 1f); //added by ikill240c 2026-09-12
                } //added by ikill240c 2026-09-12
                for (int k = -(RacesResources.QUARTERS_SIZE/* - 1*/); k <= (RacesResources.QUARTERS_SIZE/* - 1*/); k++) {
                    for (int l = -(RacesResources.QUARTERS_SIZE/* - 1*/); l <= (RacesResources.QUARTERS_SIZE/* - 1*/); l++) {
                        access.putPixelWrap(location_quarters[0] + k, location_quarters[1] + l, 0f);
                        good_starts.putPixelWrap(location_quarters[0] + k, location_quarters[1] + l, 0f);
                        buildmap.putPixelWrap(location_quarters[0] + k, location_quarters[1] + l, 0f);
                    }
                }
                location_armory = buildmap.find(J.Min(unit_grids_per_world >> 1, MAX_PLACEMENT_SEARCH_RADIUS), location_quarters[0], location_quarters[1],
                        1f);
                for (int k = -(RacesResources.ARMORY_SIZE/* - 1*/); k <= (RacesResources.ARMORY_SIZE/* - 1*/); k++) {
                    for (int l = -(RacesResources.ARMORY_SIZE/* - 1*/); l <= (RacesResources.ARMORY_SIZE/* - 1*/); l++) {
                        access.putPixelWrap(location_armory[0] + k, location_armory[1] + l, 0f);
                        good_starts.putPixelWrap(location_armory[0] + k, location_armory[1] + l, 0f);
                        buildmap.putPixelWrap(location_armory[0] + k, location_armory[1] + l, 0f);
                    }
                }
                int[] location_unit_start;
                if (archipelago) {
                    location_unit_start = good_starts.find(J.Min(unit_grids_per_world >> 1, MAX_PLACEMENT_SEARCH_RADIUS), location_quarters[0],
                            location_quarters[1], 1f);
                } else {
                    location_unit_start = access.find(J.Min(unit_grids_per_world >> 1, MAX_PLACEMENT_SEARCH_RADIUS), location_quarters[0],
                            location_quarters[1], 1f);
                }
                supply_locations[i][0] = location_armory[0];
                supply_locations[i][1] = location_armory[1];
                int[] location_unit = new int[2];
                for (int u = 0; u < initial_unit_count; u++) {
                    if (archipelago) {
                        location_unit = good_starts.find(J.Min(unit_grids_per_world >> 1, MAX_PLACEMENT_SEARCH_RADIUS), location_unit_start[0],
                                location_unit_start[1], 1f);
                    } else {
                        location_unit = access.find(J.Min(unit_grids_per_world >> 1, MAX_PLACEMENT_SEARCH_RADIUS), location_unit_start[0],
                                location_unit_start[1], 1f);
                    }
                    access.putPixelWrap(location_unit[0], location_unit[1], 0f);
                    good_starts.putPixelWrap(location_unit[0], location_unit[1], 0f);
                    player_locations[i][2 * u] = (location_unit[0] * scale);
                    player_locations[i][2 * u + 1] = (location_unit[1] * scale);
                }
            }

            // shuffle player starting locations
            // Java: Arrays.asList(player_locations) is a view - shuffling it reorders player_locations in place.
            // "Team together": group same-team players onto adjacent circle slots (slots 0..n-1 were
            // just assigned in order, evenly spaced around the circle above, so adjacent slot indices
            // ARE adjacent physical positions) instead of the fully-random per-player shuffle below.
            // Falls back to the original unconditional shuffle whenever the feature isn't actually
            // usable (no team data, or a mismatched array - e.g. an older network peer, or a caller
            // that legitimately has no real roster like the decorative main-menu background world),
            // so this never changes behavior unless a caller explicitly opted in with real data.
            // Both branches only ever call random's own shuffle methods, in a fixed order determined
            // solely by player_teams' contents (identical on every peer in a lockstep game), so this
            // stays fully deterministic either way. //added by ikill240c
            if (team_together && player_teams != null && player_teams.Length == num_players) { //added by ikill240c
                Dictionary<int, List<int>> by_team = new(); //added by ikill240c
                for (int i = 0; i < num_players; i++) { //added by ikill240c
                    by_team.computeIfAbsent(player_teams[i], _k => new List<int>()).add(i); //added by ikill240c
                } //added by ikill240c
                List<int> team_order = new List<int>(by_team.Keys);
                team_order.Sort(); // java.util.HashMap iterates small non-negative Integer keys in ascending order //added by ikill240c
                J.Shuffle(team_order, random); //added by ikill240c
                List<int> grouped_order = new(num_players); //added by ikill240c
                foreach (int team in team_order) { //added by ikill240c
                    List<int> members = by_team.get(team); //added by ikill240c
                    J.Shuffle(members, random); //added by ikill240c
                    grouped_order.addAll(members); //added by ikill240c
                } //added by ikill240c
                // grouped_order[s] is the player index that should occupy circle slot s - i.e. the
                // player at grouped position s inherits the location generated for slot s above.
                // //added by ikill240c
                float[][] regrouped = new float[num_players][]; //added by ikill240c
                for (int slot = 0; slot < num_players; slot++) { //added by ikill240c
                    regrouped[grouped_order.get(slot)] = player_locations[slot]; //added by ikill240c
                } //added by ikill240c
                player_locations = regrouped; //added by ikill240c
            } else { //added by ikill240c
                J.Shuffle(player_locations, random);
            } //added by ikill240c
        }


        // ***************
        // * GET METHODS *
        // ***************

        public static FogInfo getFogInfo(TerrainType terrain, int meters_per_world) {
            return terrain == TerrainType.NATIVE
                ? new DistanceFogInfo(FogInfo.Mode.EXP2, NATIVE_FOG_COLOR, NATIVE_FOG_DENSITY,
                            (float)(NATIVE_FOG_HEIGHT * meters_per_world), 0f, meters_per_world >> 2)
                : new DistanceFogInfo(FogInfo.Mode.EXP2, VIKING_FOG_COLOR, VIKING_FOG_DENSITY,
                            (float)(VIKING_FOG_HEIGHT * meters_per_world), 0f, meters_per_world >> 2);
        }

        private void generateWaterGrid() {
            this.dock = J.New2<sbyte>(unit_grids_per_world, unit_grids_per_world);
            this.water = J.New2<sbyte>(unit_grids_per_world, unit_grids_per_world);
            water_map = height.copy().threshold((float)(Globals.SEA_LEVEL - 10.0f), Globals.SEA_LEVEL).floodfill(0, 0, -1.0f, 0.1f,
                    null, null).threshold(-1.01f, -0.99f);
            if (DEBUG) water_map.toLayer().saveAsPNG("water_map");
            Channel shore_line = water_map.copy().smooth(2).threshold(0.4f, 0.6f);
            if (DEBUG) shore_line.toLayer().saveAsPNG("shore_line");
            Channel beach = height.copy().threshold(
                    (float)(Globals.SEA_LEVEL - (float)(0.1f / height_scale)),
                    (float)(Globals.SEA_LEVEL + (float)(0.1f / height_scale)));
            dock_map = water_map.copy().smooth(6).threshold(0.0f, 0.99f).channelMultiply(beach).channelMultiply(shore_line);
            Channel near_beach = dock_map.copy().smooth(8);
            Channel deep_water_map = water_map.copy().smooth(4).threshold(0.99f, 1.0f);
            if (DEBUG) deep_water_map.toLayer().saveAsPNG("deep_water");
            if (DEBUG) beach.toLayer().saveAsPNG("beach");
            if (DEBUG) dock_map.toLayer().saveAsPNG("dock_map");
            for (int y = 0; y < unit_grids_per_world; y++) {
                for (int x = 0; x < unit_grids_per_world; x++) {
                    sbyte water_val = 0;
                    water_val += (sbyte)(water_map.getPixel(x, y) > 0.5f ? 1 : 0);
                    water_val += (sbyte)(deep_water_map.getPixel(x, y) > 0.5f ? 1 : 0);
                    var dockf = dock_map.getPixel(x, y);
                    sbyte dock_val = 0;
                    if (dockf > 0.5f) {
                        dock_val = 1;
                    } else if (water_val == 0 && near_beach.getPixel(x, y) > 0.0f) {
                        dock_val = 2;
                    }
                    this.dock[y][x] = dock_val;
                    this.water[y][x] = water_val;
                }
            }
        }

        public BlendInfo[] getBlendInfos() {
            return blend_infos;
        }

        public GLIntImage getDetail() {
            return detail;
        }

        public float[][] getHeight() {
            return height.getPixels();
        }

        public bool[][] getAccessGrid() {
            int size = access_exported.getWidth();
            bool[][] access_grid = J.New2<bool>(size, size);
            for (int y = 0; y < size; y++) {
                for (int x = 0; x < size; x++) {
                    access_grid[y][x] = access_exported.getPixel(x, y) > 0;
                }
            }
            return access_grid;
        }

        public sbyte[][] getDockGrid() {
            return dock;
        }

        public sbyte[][] getWaterGrid() {
            return water;
        }

        public int[][] getIslandIds() {
            int size = island_ids.getWidth();
            int[][] grid = J.New2<int>(size, size);
            for (int y = 0; y < size; y++) {
                for (int x = 0; x < size; x++) {
                    grid[y][x] = J.I(((float)(island_ids.getPixel(x, y) + 0.5f)));
                }
            }
            return grid;
        }

        // Per-island metadata (area + tree/rock/iron supply counts) for every kept island. Area
        // comes from the flood fill; resource counts are tallied by mapping each supply position
        // to the island id underneath it.
        public List<IslandInfo> getIslandInfos() {
            Dictionary<int, int[]> counts = new();
            countSuppliesPerIsland(trees, counts, 0);
            countSuppliesPerIsland(rock, counts, 1);
            countSuppliesPerIsland(iron, counts, 2);

            List<IslandInfo> ret = new();
            foreach (var entry in island_info) { // LinkedHashMap -> insertion order
                int id = entry.Key;
                IslandInfo info = entry.Value;
                int[] c = counts.get(id);
                int num_trees = c != null ? c[0] : 0;
                int num_rocks = c != null ? c[1] : 0;
                int num_iron = c != null ? c[2] : 0;
                info.setIron(num_iron);
                info.setRocks(num_rocks);
                info.setTrees(num_trees);
                ret.add(info);
            }

            return ret;
        }

        private void countSuppliesPerIsland(Channel channel, Dictionary<int, int[]> counts, int index) {
            for (int y = 0; y < channel.height; y++) {
                for (int x = 0; x < channel.width; x++) {
                    if (channel.getPixel(x, y) == 1f) {
                        int id = J.I(((float)(island_ids.getPixel(x, y) + 0.5f)));
                        if (id != 0)
                            counts.computeIfAbsent(id, k => new int[3])[index]++;
                    }
                }
            }
        }

        public sbyte[][] getBuildGrid() {
            return build;
        }

        public List<int[]> getTrees() {
            return getPositions(trees);
        }

        public List<int[]> getPalmtrees() {
            return getPositions(palmtrees);
        }

        public List<int[]> getRock() {
            return getPositions(rock);
        }

        public List<int[]> getIron() {
            return getPositions(iron);
        }

        private List<int[]> getPositions(Channel channel) {
            int count = channel.count(1f);
            List<int[]> list = new(count);
            for (int y = 0; y < channel.height; y++) {
                for (int x = 0; x < channel.width; x++) {
                    if (channel.getPixel(x, y) == 1f) {
                        list.add(new int[]{x, y});
                    }
                }
            }
            System.Diagnostics.Debug.Assert(count == list.size());
            return list;
        }

        public float[][] getPlants() {
            return plants;
        }

        public float[][] getStartingLocations() {
            return player_locations;
        }

        public float getSeaLevelMeters() {
            return sea_level_meters;
        }

        /**
         * Builds an AuthoredTerrain snapshot of this Landscape's current state, so a map generated in
         * a normal singleplayer or multiplayer game (procedurally, or even an already-authored map
         * that's since had gameplay run on it) can be saved as a .ttmap and opened for further editing
         * - not just maps that started life in the standalone editor.
         *
         * <p>Only the FIRST starting unit's position is used as each player's spawn point
         * (player_locations[i][0]/[1]) - player_locations holds every initial unit's position packed
         * as (x0,y0,x1,y1,...) per player (see generateUnitLocations()), but AuthoredTerrain's
         * StartPosition is a single representative spawn point per player, matching what the map
         * editor itself produces when a player places one Start marker. Coordinates are converted
         * from world meters back to grid units (dividing by HeightMap.METERS_PER_UNIT_GRID) to match
         * AuthoredTerrain's grid-coordinate convention throughout.
         *
         * <p>This lives here (in the tt module) rather than as a constructor/factory on
         * AuthoredTerrain itself (in the common module) because common cannot depend on tt's
         * Landscape class - only tt is allowed to depend on common, not the reverse.
         * //added by ikill240c
         */
        public AuthoredTerrain exportToAuthoredTerrain() { //added by ikill240c
            int w = height.getWidth(); //added by ikill240c
            int h = height.getHeight(); //added by ikill240c
            Channel exported_height = new Channel(w, h); //added by ikill240c
            float[][] source_pixels = height.getPixels(); //added by ikill240c
            float[][] dest_pixels = exported_height.getPixels(); //added by ikill240c
            for (int y = 0; y < h; y++) { //added by ikill240c
                Array.Copy(source_pixels[y], 0, dest_pixels[y], 0, w); //added by ikill240c
            } //added by ikill240c
            AuthoredTerrain terrain = new AuthoredTerrain(exported_height); //added by ikill240c
            foreach (int[] pos in getTrees()) { //added by ikill240c
                terrain.addResource(AuthoredTerrain.ResourceType.TREE, pos[0], pos[1]); //added by ikill240c
            } //added by ikill240c
            foreach (int[] pos in getPalmtrees()) { //added by ikill240c
                terrain.addResource(AuthoredTerrain.ResourceType.PALM_TREE, pos[0], pos[1]); //added by ikill240c
            } //added by ikill240c
            foreach (int[] pos in getRock()) { //added by ikill240c
                terrain.addResource(AuthoredTerrain.ResourceType.ROCK, pos[0], pos[1]); //added by ikill240c
            } //added by ikill240c
            foreach (int[] pos in getIron()) { //added by ikill240c
                terrain.addResource(AuthoredTerrain.ResourceType.IRON, pos[0], pos[1]); //added by ikill240c
            } //added by ikill240c
            for (int player_index = 0; player_index < player_locations.Length; player_index++) { //added by ikill240c
                float[] locations = player_locations[player_index]; //added by ikill240c
                if (locations.Length < 2) //added by ikill240c
                    continue; // no starting unit recorded for this player slot //added by ikill240c
                int grid_x = J.Round((float)(locations[0] / HeightMap.METERS_PER_UNIT_GRID)); //added by ikill240c
                int grid_y = J.Round((float)(locations[1] / HeightMap.METERS_PER_UNIT_GRID)); //added by ikill240c
                terrain.addStart(player_index, grid_x, grid_y); // team defaults to -1 - not tracked by a live game world //added by ikill240c
            } //added by ikill240c
            return terrain; //added by ikill240c
        }

    }
}
