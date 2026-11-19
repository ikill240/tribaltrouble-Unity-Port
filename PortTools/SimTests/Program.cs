using System;
using System.IO;
using TribalTrouble.Sim.Conformance;

static class Program
{
    static int Main(string[] args)
    {
        string golden = args.Length > 0 ? args[0] : Path.Combine("..", "golden");
        bool quick = Array.IndexOf(args, "--quick") >= 0;
        Console.WriteLine($"Island generator conformance vs Java goldens in {Path.GetFullPath(golden)}");
        var (passed, failed, report) = LandscapeConformance.RunAll(golden, quick ? (Func<LandscapeConformance.Case, bool>)(c => c.meters <= 512) : null,
            line => Console.WriteLine(line));
        File.WriteAllText(Path.Combine(golden, "..", "conformance_report.txt"), report);
        Console.WriteLine($"== {passed} passed, {failed} failed");
        return failed == 0 ? 0 : 1;
    }
}
