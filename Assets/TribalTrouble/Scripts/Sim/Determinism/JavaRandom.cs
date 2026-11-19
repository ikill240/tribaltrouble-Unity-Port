using System;

namespace TribalTrouble.Sim.Determinism
{
    /// <summary>
    /// Bit-exact port of java.util.Random (48-bit LCG), used by World, Landscape and the AIs.
    /// The simulation must produce the same numbers as the Java client so that the same map
    /// code yields the same island and, later, so lockstep games stay in sync.
    /// All arithmetic is unchecked to reproduce Java's two's-complement wrap-around.
    /// </summary>
    public sealed class JavaRandom
    {
        const long Multiplier = 0x5DEECE66DL;
        const long Addend = 0xBL;
        const long Mask = (1L << 48) - 1;
        const float FloatUnit = 1.0f / (1 << 24);
        const double DoubleUnit = 1.0 / (1L << 53);

        long _seed;

        public JavaRandom(long seed)
        {
            SetSeed(seed);
        }

        public void SetSeed(long seed)
        {
            _seed = (seed ^ Multiplier) & Mask;
        }

        /// <summary>Internal 48-bit state (for checksums / save games).</summary>
        public long State => _seed;

        int Next(int bits)
        {
            unchecked
            {
                _seed = (_seed * Multiplier + Addend) & Mask;
                return (int)(_seed >> (48 - bits)); // seed is non-negative, so >> == Java >>>
            }
        }

        public int NextInt()
        {
            return Next(32);
        }

        /// <summary>java.util.Random.nextInt(int bound).</summary>
        public int NextInt(int bound)
        {
            if (bound <= 0)
                throw new ArgumentOutOfRangeException(nameof(bound), "bound must be positive");
            unchecked
            {
                int r = Next(31);
                int m = bound - 1;
                if ((bound & m) == 0)
                    return (int)((bound * (long)r) >> 31);
                for (int u = r; u - (r = u % bound) + m < 0; u = Next(31))
                {
                }
                return r;
            }
        }

        public long NextLong()
        {
            unchecked
            {
                return ((long)Next(32) << 32) + Next(32);
            }
        }

        public bool NextBoolean()
        {
            return Next(1) != 0;
        }

        public float NextFloat()
        {
            return Next(24) * FloatUnit;
        }

        public double NextDouble()
        {
            unchecked
            {
                return (((long)Next(26) << 27) + Next(27)) * DoubleUnit;
            }
        }

        // nextGaussian() needs StrictMath.log/sqrt (fdlibm) to be bit-exact; it is added
        // together with the StrictMath port in Stage 1.
    }
}
