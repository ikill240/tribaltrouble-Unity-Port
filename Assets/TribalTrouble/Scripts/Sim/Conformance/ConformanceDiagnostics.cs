using System;
using System.Text;
using TribalTrouble.Sim.Determinism;
using TribalTrouble.Sim.Procedural;

namespace TribalTrouble.Sim.Conformance
{
    /// <summary>
    /// Pinpoints where the C# generator first drifts from Java: float-precision probes (does this
    /// runtime round every float operation like Java?) and channel checksums after each step of
    /// generateTerrainNative for golden case c01. Reference values printed by ProbeStages.java.
    /// </summary>
    public static class ConformanceDiagnostics
    {
        public static readonly uint[] ProbeBits = {0x00000000, 0x00000000, 0x3CF5C295, 0x3FECCCCC, 0x3BCEC08A, 0x3EE7C33A, 0xBF9A1BC8, 0x3F71A8E4, 0x3EA8F4DD, 0x3EA1E89B, };
        public static readonly string[] StageNames = {"midpoint_42_s3", "voronoi4_42", "mountain", "mountain*0.67", "voronoi_dist", "cliffs", "h+cliffs", "vor_d1", "vor_d1_gamma", "vor_d1_flip_rot", "h-sub", "midpoint2", "perturb", "hill_oval", "h+shape", "h-invshape", "erode", "h*shape2", "smooth", };
        public static readonly long[] StageCrcs = {0x4775D88EL, 0x5ACE7F2EL, 0xE85E9AE8L, 0xBF4796A2L, 0xC425229CL, 0x4CC2E043L, 0x03034C40L, 0xD064CD04L, 0xEFBEF0CDL, 0x2881802AL, 0x07C5C19BL, 0x1307D2AEL, 0x3E96991BL, 0xE6CDD39FL, 0xD332CEE5L, 0x9B61BFA0L, 0x7FE91D8CL, 0xDB696571L, 0x9CF47BF9L, };
        public static readonly uint[] StageSamples = {0x3EB0A641, 0x3F1A9604, 0x3E8C5F69, 0x3E997F59, 0x3DBCE51F, 0x3E467479, 0x3F075CBD, 0x3EBAB767, 0x3EA8A0F1, 0x3EB562AC, 0x3E7A3333, 0x3E61F662, 0x3D916391, 0x3D336F5F, 0x3F09E217, 0x3D0FEF60, 0x3CB1A405, 0x3E88811C, 0x3EC76098, 0x3E8833DA, 0x3EF97C4D, 0x3D9B22A3, 0x3F4971F1, 0x3E39FC80, 0x3BBC05EC, 0x3F1E841F, 0x3D071EEA, 0x3BB2AB40, 0x3DD7D756, 0x3D0B43F8, 0x3EC495EB, 0x3E247C09, 0x3EE813CE, 0x3F2FC381, 0x3EC123C4, 0x3EEAE2C3, 0x3EE1920C, 0x3E92B35C, 0x3F030B33, 0x00000000, 0x3B383000, 0x3F7FFFF0, 0x3EE1920C, 0x3E92EA9E, 0x3F297197, 0x00000000, 0x00000000, 0x3F29718F, 0x3CA5FF52, 0x3BEB8D4B, 0x3F159E28, 0x00000000, 0x38293C88, 0x3F159E28, 0x00000000, 0x38293C88, 0x3F156A7C, };
        // volatile-ish inputs (static fields) so the compiler cannot constant-fold the probes
        static float va = 16777216f, vb = 1f, vc = 0.1f, vd = 3f, ve = 1e-8f;

        public static string Run()
        {
            var sb = new StringBuilder();
            sb.AppendLine("== Float probes (Java bits vs C# bits)");
            float[] probes =
            {
                va + vb - va,                               // raw: shows whether this runtime rounds like Java
                (vc * vd) - 0.3f,
                vc * vd * vc + ve,
                0.25f * (vc + vd + vc + vd) + vc * vd - ve,
                (float)J.Pow(vc, 2.2),
                (float)J.Pow(0.7f, 1.0 / 0.45f),
                (float)J.Log(0.3f),
                (float)J.Sin(1.2345),
                (float)J.Cos(1.2345),
                (float)J.Sqrt(vc),
            };
            string[] names = { "a+b-a (float rounding)", "c*d-0.3f", "c*d*c+e", "midpoint-style expr", "pow(.1,2.2)", "pow(.7,1/.45)", "log(.3)", "sin", "cos", "sqrt" };
            for (int i = 0; i < probes.Length; i++)
            {
                int bits = J.FloatToIntBits(probes[i]);
                sb.AppendLine(string.Format("  {0,-24} {1}  C# 0x{2:X8} Java 0x{3:X8}", names[i], (uint)bits == ProbeBits[i] ? "ok  " : "DIFF", (uint)bits, ProbeBits[i]));
            }

            sb.AppendLine("== Stage checksums (case c01 terrain pipeline)");
            int stage = 0;
            void Rec(Channel c)
            {
                long crc = c.getChecksum();
                uint s0 = (uint)J.FloatToIntBits(c.getPixel(10, 10)), s1 = (uint)J.FloatToIntBits(c.getPixel(93, 1)), s2 = (uint)J.FloatToIntBits(c.getPixel(128, 128));
                bool ok = crc == StageCrcs[stage];
                sb.AppendLine(string.Format("  {0,-18} {1}  crc C# {2:X8} Java {3:X8}  samples C# {4:X8} {5:X8} {6:X8} / Java {7:X8} {8:X8} {9:X8}",
                    StageNames[stage], ok ? "ok  " : "DIFF", crc, StageCrcs[stage], s0, s1, s2,
                    StageSamples[stage * 3], StageSamples[stage * 3 + 1], StageSamples[stage * 3 + 2]));
                stage++;
            }
            int size = 256, seed = 12345, features = 4;
            float hills = (float)J.Sqrt(0.5f);
            Rec(new Midpoint(256, 3, 0.45f, 42).toChannel());
            Rec(new Voronoi(256, 4, 4, 1, 1f, 42).getDistance(-1f, 1f, 0f));
            Channel height = new Mountain(size, 2, 0.5f, seed).toChannel(); Rec(height);
            height.multiply(0.67f); Rec(height);
            var voronoi = new Voronoi(size, features, features, 1, 1f, seed);
            Channel cliffs = voronoi.getDistance(-1f, 1f, 0f); Rec(cliffs);
            cliffs.brightness(1.5f).multiply(0.33f); Rec(cliffs);
            height.channelAdd(cliffs); Rec(height);
            Channel sub = voronoi.getDistance(1f, 0f, 0f); Rec(sub);
            sub.gamma(.5f); Rec(sub);
            sub.flipV().rotate(90); Rec(sub);
            height.channelSubtract(sub); Rec(height);
            Channel mp = new Midpoint(size, 2, 0.5f, seed).toChannel(); Rec(mp);
            height.perturb(mp, 0.25f); Rec(height);
            Channel shape = new Hill(size, Hill.OVAL).toChannel(); Rec(shape);
            height.channelAdd(shape.copy().multiply(0.15f)); Rec(height);
            height.channelSubtract(shape.copy().invert().multiply(0.5f)); Rec(height);
            height.erode((float)((float)(24f - (float)(hills * 12f)) / size), J.Min(size >> 2, 512)); Rec(height);
            height.channelMultiply(shape.gamma2()); Rec(height);
            height.smooth(1); Rec(height);
            sb.AppendLine("Runtime: " + System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription
                          + ", 64-bit process: " + Environment.Is64BitProcess);
            return sb.ToString();
        }
    }
}
