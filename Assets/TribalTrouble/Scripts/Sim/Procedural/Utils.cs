// Ported from com.oddlabs.util.Utils (the math helpers only; networking/IO helpers are not simulation code).
namespace TribalTrouble.Sim.Procedural
{
    public static class Utils
    {
        public static int powerOf2Log2(int n)
        {
            System.Diagnostics.Debug.Assert(isPowerOf2(n));
            for (int i = 0; i < 31; i++)
            {
                if ((n & 1) == 1)
                    return i;
                n >>= 1;
            }
            return 0;
        }

        public static bool isPowerOf2(int n) => n == 0 || (n > 0 && (n & (n - 1)) == 0);

        public static int nextPowerOf2(int n)
        {
            int x = 1;
            while (x < n)
                x <<= 1;
            return x;
        }
    }
}
