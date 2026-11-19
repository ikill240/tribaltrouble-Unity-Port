// Hand-written (not translated): a bit-exact parallel version of Channel.erode / erodeThermal.
// The Java loops update the height field in place, row by row (Gauss-Seidel), so iterations can't simply run in
// parallel. They can be pipelined though: a cell update reads and writes only its own row and the rows next to it,
// so iteration k may process row y as soon as iteration k-1 has finished rows up to y+2. Every cell then sees
// exactly the values the sequential loop would give it, and the result is identical bit for bit (same float
// operations in the same dependency order). Unreal islands (2048 x 2048 grid, 512 iterations) go from ~30 s to a
// few seconds on a multi-core machine.
using System.Threading;

namespace com.oddlabs.procedural
{
    public static class ChannelErodeFast
    {
        public static void Erode(float[][] p, int width, int height, float talus, int iterations, bool thermal)
        {
            if (iterations <= 0 || width < 3 || height < 3) return;
            var clock = System.Diagnostics.Stopwatch.StartNew();
            // leave cores to the engine (its render and job threads): busy-waiting workers that outnumber the free
            // cores stall the whole pipeline
            int threads = System.Math.Min(System.Math.Min(System.Environment.ProcessorCount / 2, 8), iterations);
            // small maps: one thread is quicker than starting and handing rows between several
            if (threads <= 1 || (long)width * height < 1024 * 1024)
            {
                for (int k = 0; k < iterations; k++) Pass(p, width, height, talus, thermal, 1, height - 2, null, -1, null, 0);
                Report(clock, width, height, iterations, 1);
                return;
            }
            var progress = new int[iterations];   // last completed row of each iteration (height = finished)
            var workers = new Thread[threads];
            System.Exception error = null;
            for (int t = 0; t < threads; t++)
            {
                int first = t;
                workers[t] = new Thread(() =>
                {
                    try
                    {
                        for (int k = first; k < iterations; k += threads)
                        {
                            Pass(p, width, height, talus, thermal, 1, height - 2, progress, k, progress, k - 1);
                            Volatile.Write(ref progress[k], height);
                        }
                    }
                    catch (System.Exception e) { error = e; for (int k = 0; k < iterations; k++) Volatile.Write(ref progress[k], int.MaxValue); }
                }, 1 << 20) { IsBackground = true, Name = "Erode " + t };
                workers[t].Start();
            }
            foreach (var w in workers) w.Join();
            if (error != null) throw new System.InvalidOperationException("erode failed", error);
            Report(clock, width, height, iterations, threads);
        }

        /// <summary>Slow erosion goes to the log (it should take well under a second, a few on Unreal islands).</summary>
        static void Report(System.Diagnostics.Stopwatch clock, int width, int height, int iterations, int threads)
        {
            if (clock.Elapsed.TotalSeconds > 3)
                java.lang.IO.println("Erosion " + width + "x" + height + ", " + iterations + " iterations, " + threads + " threads: " + clock.Elapsed.TotalSeconds.ToString("0.0") + " s");
        }

        static void Pass(float[][] p, int width, int height, float talus, bool thermal, int y0, int y1,
                         int[] mine, int k, int[] prev, int pk)
        {
            for (int y = y0; y <= y1; y++)
            {
                if (prev != null && pk >= 0)
                {
                    int need = System.Math.Min(y + 2, height - 2);
                    // busy-wait: the neighbour iteration is at most a few rows behind. Never Thread.Sleep (Unity's
                    // Mono SpinWait sleeps 1 ms+ every few spins, which stalls the whole pipeline on Windows)
                    for (int n = 0; Volatile.Read(ref prev[pk]) < need; n++)
                    {
                        if (n < 200) Thread.SpinWait(20); else Thread.Yield();
                    }
                }
                float[] row = p[y], up = p[y + 1], down = p[y - 1];
                for (int x = 1; x < width - 1; x++)
                {
                    float h = row[x];
                    float h1 = up[x];
                    float h2 = row[x - 1];
                    float h3 = row[x + 1];
                    float h4 = down[x];
                    float d1 = h - h1;
                    float d2 = h - h2;
                    float d3 = h - h3;
                    float d4 = h - h4;
                    int i = 0, j = 0;
                    float max_d = 0f;
                    if (d1 > max_d) { max_d = d1; j = 1; }
                    if (d2 > max_d) { max_d = d2; i = -1; j = 0; }
                    if (d3 > max_d) { max_d = d3; i = 1; j = 0; }
                    if (d4 > max_d) { max_d = d4; i = 0; j = -1; }
                    if (thermal ? max_d < talus : max_d > talus) continue;
                    max_d *= 0.5f;
                    row[x] = row[x] - max_d;
                    float[] r2 = p[y + j];
                    r2[x + i] = r2[x + i] + max_d;
                }
                if (mine != null) Volatile.Write(ref mine[k], y);
            }
        }
    }
}
