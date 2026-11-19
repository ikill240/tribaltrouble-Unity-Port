// The island settings exactly as your Java TerrainMenu holds them (size pulldown index, terrain index, the three sliders
// in 0..20 steps of 0.1, and the 0..39999 seed whose square is the world seed), plus its word map code
// (TerrainMenu.setMapcode / parseBigInteger, WordsEncoding). A code made in the Java game gives the same island here.
namespace TribalTrouble.Faithful
{
    public sealed class IslandSettings
    {
        // TerrainMenu.SIZES / ARCHIPELAGO, in pulldown order (= Game.SIZE_*)
        public static readonly int[] Meters = { 256, 512, 1024, 2048, 4096, 2048 };
        public static readonly bool[] Archipelago = { false, false, false, false, false, true };
        public static readonly string[] SizeNames = { "Small", "Medium", "Large", "Enormous", "Unreal", "Archipelago" };

        const long SeedCard = 40000, SliderCard = 21, TerrainCard = 4, SizeCard = 7;
        public const int SliderMax = 20, SliderUnit = 10;   // MAP_SETTING_SLIDER_MAX, SLIDER_MAX_VALUE

        public int size = 1, terrain, hills = SliderUnit / 2, trees = SliderUnit / 2, supplies = SliderUnit / 2, seed = -1;

        static readonly System.Random s_rng = new System.Random();
        public void RandomSeed() => seed = s_rng.Next((int)SeedCard);


        public string Code()
        {
            long v = 0, m = 1;
            v += System.Math.Max(0, seed) * m; m *= SeedCard;
            v += hills * m; m *= SliderCard;
            v += trees * m; m *= SliderCard;
            v += supplies * m; m *= SliderCard;
            v += terrain * m; m *= TerrainCard;
            v += size * m;
            try { return com.oddlabs.tt.util.WordsEncoding.encode(java.math.BigInteger.valueOf(v)); }
            catch (System.Exception) { return ""; }
        }

        /// <summary>Reads a word map code (Java parseBigInteger). Returns false if it isn't one.</summary>
        public bool Parse(string text)
        {
            text = (text ?? "").Trim();
            if (text.IndexOf(' ') < 0) return false;   // the old letter codes held per-player settings, not supported here
            long v;
            try
            {
                const long max = SeedCard * SliderCard * SliderCard * SliderCard * TerrainCard * SizeCard;
                v = com.oddlabs.tt.util.WordsEncoding.decode(text).mod(java.math.BigInteger.valueOf(max)).longValue();
            }
            catch (System.Exception) { return false; }
            seed = (int)(v % SeedCard); v /= SeedCard;
            hills = (int)(v % SliderCard); v /= SliderCard;
            trees = (int)(v % SliderCard); v /= SliderCard;
            supplies = (int)(v % SliderCard); v /= SliderCard;
            int t = (int)(v % TerrainCard); v /= TerrainCard;
            int sz = (int)v;
            if (t < 2) terrain = t;                    // the Java pulldown only has the first two terrain types
            if (sz < Meters.Length) size = sz;
            return true;
        }
    }
}
