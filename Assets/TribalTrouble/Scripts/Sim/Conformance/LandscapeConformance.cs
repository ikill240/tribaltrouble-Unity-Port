using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;
using TribalTrouble.Sim.Determinism;
using TribalTrouble.Sim.Procedural;

namespace TribalTrouble.Sim.Conformance
{
    /// <summary>
    /// Compares the C# island generator against golden output recorded from the real Java
    /// generator (PortTools/golden/*.ttl.gz, written by GoldenLandscape.java). A case passes only
    /// if every value is bit-identical: heights, access/build/dock/water grids, island ids,
    /// trees, palms, rocks, iron, plants and starting locations.
    /// Engine-independent: runs from the Unity menu or from a plain `dotnet run` console.
    /// </summary>
    public static class LandscapeConformance
    {
        public sealed class Case
        {
            public string id;
            public int players, meters;
            public Landscape.TerrainType terrain;
            public float hills, vegetation, supplies;
            public int seed, units;
            public float randomStart;
            public bool archipelago;
        }

        /// <summary>Same lines as PortTools/golden/cases.txt (what the Java harness was run with).</summary>
        public static readonly string[] CaseLines =
        {
            "c01 4 512 NATIVE 0.5 0.5 0.5 12345 10 0.5 false",
            "c02 2 256 VIKING 0 1 0 1 10 0.0 false",
            "c03 6 1024 NATIVE 1 0.25 0.75 39999 10 0.25 false",
            "c04 8 1024 VIKING 0.75 0.5 0.5 777 10 0.6 true",
            "c05 3 512 VIKING 0.3 0.8 0.2 2024 6 0.9 false",
            "c06 4 2048 NATIVE 0.5 0.5 0.5 4242 10 0.1 false",
            "c07 2 256 NATIVE 1 0 1 0 10 0.5 false",
            "c08 5 512 NATIVE 0.5 0.6 0.4 31337 10 0.33 true",
            "c09 6 2048 VIKING 0.2 0.9 0.9 15000 10 0.75 false",
            "c10 2 512 NATIVE 0.05 0.5 0.5 99 1 0.5 false",
        };

        public static Case Parse(string line)
        {
            var p = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var inv = System.Globalization.CultureInfo.InvariantCulture;
            return new Case
            {
                id = p[0],
                players = int.Parse(p[1], inv),
                meters = int.Parse(p[2], inv),
                terrain = (Landscape.TerrainType)Enum.Parse(typeof(Landscape.TerrainType), p[3]),
                hills = float.Parse(p[4], inv),
                vegetation = float.Parse(p[5], inv),
                supplies = float.Parse(p[6], inv),
                seed = int.Parse(p[7], inv),
                units = int.Parse(p[8], inv),
                randomStart = float.Parse(p[9], inv),
                archipelago = bool.Parse(p[10]),
            };
        }

        public static Landscape Generate(Case c)
        {
            const float detail_prefade = 0.15f; // as in GoldenLandscape.java
            return new Landscape(c.players, c.meters, c.terrain, detail_prefade, c.hills, c.vegetation, c.supplies,
                c.seed, c.units, c.randomStart, c.archipelago, null, false, null);
        }

        /// <summary>Runs every case whose golden file exists in goldenDir. Returns (passed, failed, report).</summary>
        public static (int passed, int failed, string report) RunAll(string goldenDir, Func<Case, bool> filter = null,
            Action<string> log = null)
        {
            var sb = new StringBuilder();
            int passed = 0, failed = 0;
            foreach (var line in CaseLines)
            {
                var c = Parse(line);
                if (filter != null && !filter(c)) continue;
                string path = Path.Combine(goldenDir, c.id + ".ttl.gz");
                if (!File.Exists(path))
                {
                    sb.AppendLine($"{c.id}: SKIP (no golden file {path})");
                    continue;
                }
                string result;
                bool ok;
                try
                {
                    var sw = Stopwatch.StartNew();
                    var landscape = Generate(c);
                    long ms = sw.ElapsedMilliseconds;
                    var golden = Golden.Load(path);
                    var diffs = Compare(golden, landscape);
                    ok = diffs.Count == 0;
                    result = ok
                        ? $"{c.id}: PASS ({c.meters}m {c.terrain}, {c.players}p, generated in {ms} ms)"
                        : $"{c.id}: FAIL ({c.meters}m {c.terrain})\n    " + string.Join("\n    ", diffs);
                }
                catch (Exception e)
                {
                    ok = false;
                    result = $"{c.id}: ERROR {e.GetType().Name}: {e.Message}\n{e.StackTrace}";
                }
                if (ok) passed++; else failed++;
                sb.AppendLine(result);
                log?.Invoke(result);
            }
            sb.AppendLine($"== {passed} passed, {failed} failed");
            return (passed, failed, sb.ToString());
        }

        // ------------------------------------------------------------------ golden format

        public sealed class Golden
        {
            public float seaLevel;
            public float[][] height;
            public bool[][] access;
            public sbyte[][] build, dock, water;
            public int[][] islandIds;
            public List<int[]> trees, palms, rock, iron;
            public float[][] plants, starts;

            public static Golden Load(string gzPath)
            {
                using (var fs = File.OpenRead(gzPath))
                using (var gz = new GZipStream(fs, CompressionMode.Decompress))
                using (var bs = new BufferedStream(gz, 1 << 16))
                {
                    var i = new JDataInput(bs);
                    if (i.readInt() != 0x54544C31) throw new InvalidDataException("not a TTL1 golden file");
                    var g = new Golden { seaLevel = i.readFloat() };
                    int h = i.readInt(), w = i.readInt();
                    g.height = J.New2<float>(h, w);
                    for (int y = 0; y < h; y++) for (int x = 0; x < w; x++) g.height[y][x] = i.readFloat();
                    h = i.readInt(); w = i.readInt();
                    g.access = J.New2<bool>(h, w);
                    for (int y = 0; y < h; y++) for (int x = 0; x < w; x++) g.access[y][x] = i.readByte() != 0;
                    g.build = ReadBytes(i); g.dock = ReadBytes(i); g.water = ReadBytes(i);
                    h = i.readInt(); w = i.readInt();
                    g.islandIds = J.New2<int>(h, w);
                    for (int y = 0; y < h; y++) for (int x = 0; x < w; x++) g.islandIds[y][x] = i.readInt();
                    g.trees = ReadList(i); g.palms = ReadList(i); g.rock = ReadList(i); g.iron = ReadList(i);
                    g.plants = ReadFloats(i); g.starts = ReadFloats(i);
                    return g;
                }
            }

            static sbyte[][] ReadBytes(JDataInput i)
            {
                int h = i.readInt(), w = i.readInt();
                var g = J.New2<sbyte>(h, w);
                for (int y = 0; y < h; y++) for (int x = 0; x < w; x++) g[y][x] = i.readByte();
                return g;
            }

            static List<int[]> ReadList(JDataInput i)
            {
                int n = i.readInt();
                var l = new List<int[]>(n);
                for (int k = 0; k < n; k++)
                {
                    var p = new int[i.readInt()];
                    for (int j = 0; j < p.Length; j++) p[j] = i.readInt();
                    l.Add(p);
                }
                return l;
            }

            static float[][] ReadFloats(JDataInput i)
            {
                int n = i.readInt();
                var f = new float[n][];
                for (int k = 0; k < n; k++)
                {
                    f[k] = new float[i.readInt()];
                    for (int j = 0; j < f[k].Length; j++) f[k][j] = i.readFloat();
                }
                return f;
            }
        }

        // ------------------------------------------------------------------ comparison

        public static List<string> Compare(Golden g, Landscape l)
        {
            var d = new List<string>();
            if (J.FloatToIntBits(g.seaLevel) != J.FloatToIntBits(l.getSeaLevelMeters()))
                d.Add($"sea level {l.getSeaLevelMeters():R} != {g.seaLevel:R}");
            CompareFloats("height", g.height, l.getHeight(), d);
            CompareGrid("access", g.access, l.getAccessGrid(), (a, b) => a == b, d);
            CompareGrid("build", g.build, l.getBuildGrid(), (a, b) => a == b, d);
            CompareGrid("dock", g.dock, l.getDockGrid(), (a, b) => a == b, d);
            CompareGrid("water", g.water, l.getWaterGrid(), (a, b) => a == b, d);
            CompareGrid("island ids", g.islandIds, l.getIslandIds(), (a, b) => a == b, d);
            CompareList("trees", g.trees, l.getTrees(), d);
            CompareList("palms", g.palms, l.getPalmtrees(), d);
            CompareList("rock", g.rock, l.getRock(), d);
            CompareList("iron", g.iron, l.getIron(), d);
            CompareFloatRows("plants", g.plants, l.getPlants(), d);
            CompareFloatRows("starts", g.starts, l.getStartingLocations(), d);
            return d;
        }

        static void CompareFloats(string what, float[][] a, float[][] b, List<string> d)
        {
            if (a.Length != b.Length || a[0].Length != b[0].Length)
            {
                d.Add($"{what}: size {b.Length}x{b[0].Length} != {a.Length}x{a[0].Length}");
                return;
            }
            int bad = 0;
            double maxDiff = 0;
            string first = null;
            for (int y = 0; y < a.Length; y++)
                for (int x = 0; x < a[y].Length; x++)
                    if (J.FloatToIntBits(a[y][x]) != J.FloatToIntBits(b[y][x]))
                    {
                        bad++;
                        maxDiff = Math.Max(maxDiff, Math.Abs((double)a[y][x] - b[y][x]));
                        if (first == null) first = $"first at ({x},{y}): C# {b[y][x]:R} vs Java {a[y][x]:R}";
                    }
            if (bad > 0) d.Add($"{what}: {bad} values differ (max |diff| {maxDiff:G4}); {first}");
        }

        static void CompareGrid<T>(string what, T[][] a, T[][] b, Func<T, T, bool> eq, List<string> d)
        {
            if (b == null) { d.Add($"{what}: missing"); return; }
            if (a.Length != b.Length || a[0].Length != b[0].Length)
            {
                d.Add($"{what}: size {b.Length}x{b[0].Length} != {a.Length}x{a[0].Length}");
                return;
            }
            int bad = 0;
            string first = null;
            for (int y = 0; y < a.Length; y++)
                for (int x = 0; x < a[y].Length; x++)
                    if (!eq(a[y][x], b[y][x]))
                    {
                        bad++;
                        if (first == null) first = $"first at ({x},{y}): C# {b[y][x]} vs Java {a[y][x]}";
                    }
            if (bad > 0) d.Add($"{what}: {bad} cells differ; {first}");
        }

        static void CompareList(string what, List<int[]> a, List<int[]> b, List<string> d)
        {
            if (a.Count != b.Count) { d.Add($"{what}: count {b.Count} != {a.Count}"); return; }
            for (int i = 0; i < a.Count; i++)
                if (a[i].Length != b[i].Length || a[i][0] != b[i][0] || a[i][1] != b[i][1])
                {
                    d.Add($"{what}: entry {i} is ({b[i][0]},{b[i][1]}) vs Java ({a[i][0]},{a[i][1]})");
                    return;
                }
        }

        static void CompareFloatRows(string what, float[][] a, float[][] b, List<string> d)
        {
            if (b == null) { d.Add($"{what}: missing"); return; }
            if (a.Length != b.Length) { d.Add($"{what}: rows {b.Length} != {a.Length}"); return; }
            for (int r = 0; r < a.Length; r++)
            {
                if (a[r].Length != b[r].Length) { d.Add($"{what}[{r}]: length {b[r].Length} != {a[r].Length}"); return; }
                for (int k = 0; k < a[r].Length; k++)
                    if (J.FloatToIntBits(a[r][k]) != J.FloatToIntBits(b[r][k]))
                    {
                        d.Add($"{what}[{r}][{k}]: C# {b[r][k]:R} vs Java {a[r][k]:R}");
                        return;
                    }
            }
        }
    }
}
