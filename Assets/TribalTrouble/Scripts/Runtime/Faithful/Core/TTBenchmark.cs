// Times the building blocks of the island generator, to compare runtimes (.NET in the test harness, Mono in Unity).
// No UnityEngine here.
using System;
using System.Diagnostics;
using System.Text;

namespace TribalTrouble.Faithful
{
    public static class TTBenchmark
    {
        public static string Run()
        {
            FaithfulMatch.InitGlobals();
            var sb = new StringBuilder();
            void T(string name, Action a)
            {
                var sw = Stopwatch.StartNew();
                try { a(); sb.Append(name).Append(": ").Append(sw.ElapsedMilliseconds).Append(" ms\n"); }
                catch (Exception e) { sb.Append(name).Append(": FAILED ").Append(e.GetType().Name).Append(' ').Append(e.Message).Append('\n'); }
            }
            double acc = 0;
            T("float loop 50M", () => { float f = 0; for (int i = 0; i < 50_000_000; i++) f += i * 0.5f; acc += f; });
            T("jagged float[][] 512^2 x20", () => { var a = new float[512][]; for (int y = 0; y < 512; y++) a[y] = new float[512]; for (int k = 0; k < 20; k++) for (int y = 0; y < 512; y++) for (int x = 0; x < 512; x++) a[y][x] += x * k; acc += a[3][3]; });
            T("Pow 100k (1.0001, 7)", () => { for (int i = 0; i < 100_000; i++) acc += j2cs.fdlibm.FdLibm.Pow.compute(1.0001, 7); });
            T("__LO(x,0) + __HI 10M", () => { for (int i = 0; i < 10_000_000; i++) acc += j2cs.fdlibm.FdLibm.__LO(i * 1.5, 0) + j2cs.fdlibm.FdLibm.__HI(i * 0.5); });
            T("StrictMath.pow 1M", () => { for (int i = 0; i < 1_000_000; i++) acc += java.lang.StrictMath.pow(1.0001, i % 300); });
            T("StrictMath.sin 1M", () => { for (int i = 0; i < 1_000_000; i++) acc += java.lang.StrictMath.sin(i * 0.001); });
            T("Math.pow 1M", () => { for (int i = 0; i < 1_000_000; i++) acc += java.lang.Math.pow(1.0001, i % 300); });
            T("JC.d2i 10M", () => { for (int i = 0; i < 10_000_000; i++) acc += java.lang.JC.d2i(i * 0.7); });
            T("ArrayList boxed 1M", () => { var l = new java.util.ArrayList(); for (int i = 0; i < 1_000_000; i++) l.add((object)i); long s = 0; for (int i = 0; i < l.size(); i++) s += (int)l.get(i); acc += s; });
            T("HashMap 200k", () => { var m = new java.util.HashMap(); for (int i = 0; i < 200_000; i++) m.put((object)i, (object)i); for (int i = 0; i < 200_000; i++) acc += (int)m.get((object)i); });
            T("java.util.Random 5M", () => { var r = new java.util.Random(5); for (int i = 0; i < 5_000_000; i++) acc += r.nextFloat(); });
            T("Midpoint 512", () => acc += new com.oddlabs.tt.procedural.Midpoint(512, 4, 0.5f, 7).toChannel().getPixel(1, 1));
            T("Perlin 256", () => acc += new com.oddlabs.tt.procedural.Perlin(256, 256, 8, 8, 0.5f, 3, 7, com.oddlabs.tt.procedural.Perlin.Interpolation.CUBIC, com.oddlabs.tt.procedural.Perlin.Summation.NORMAL).toChannel().getPixel(1, 1));
            T("Voronoi 256", () => acc += new com.oddlabs.tt.procedural.Voronoi(256, 8, 8, 1, 1f, 7).getDistance(1f, 0f, 0f).getPixel(1, 1));
            com.oddlabs.procedural.Channel ch = null;
            T("Channel ops 512 (copy, smooth, perturb, rotate)", () => { var c = new com.oddlabs.tt.procedural.Midpoint(512, 4, 0.5f, 3).toChannel(); ch = c.copy().smooth(2).perturb(c.copy().rotate(90), 0.05f).dynamicRange(); acc += ch.getPixel(2, 2); });
            T("Island small (generate)", () => new com.oddlabs.tt.mapeditor.MapSettings(0, 0, 10, 10, 10, 1234).createGenerator().generate(1, 10, 0f, false, null));
            T("Island medium (generate)", () => new com.oddlabs.tt.mapeditor.MapSettings(1, 0, 10, 10, 10, 1234).createGenerator().generate(1, 10, 0f, false, null));
            sb.Append("(checksum ").Append(acc.ToString("G3")).Append(")\n");
            return sb.ToString();
        }
    }
}
