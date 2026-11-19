// Translated from MapSettings.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.mapeditor
{
    // Java: 22
    public class MapSettings : java.lang.Object
    {
        public int size() { return size_f; }
        public int terrain() { return terrain_f; }
        public int hills() { return hills_f; }
        public int trees() { return trees_f; }
        public int supplies() { return supplies_f; }
        public int seed() { return seed_f; }
        public MapSettings(int size_v, int terrain_v, int hills_v, int trees_v, int supplies_v, int seed_v)
        {
            size_v = java.lang.Math.clamp(size_v, 0, com.oddlabs.tt.mapeditor.MapSettings.NUM_SIZES - 1);
            terrain_v = java.lang.Math.clamp(terrain_v, 0, com.oddlabs.tt.mapeditor.MapSettings.NUM_TERRAINS - 1);
            hills_v = java.lang.Math.clamp(hills_v, 0, com.oddlabs.tt.mapeditor.MapSettings.SLIDER_MAX);
            trees_v = java.lang.Math.clamp(trees_v, 0, com.oddlabs.tt.mapeditor.MapSettings.SLIDER_MAX);
            supplies_v = java.lang.Math.clamp(supplies_v, 0, com.oddlabs.tt.mapeditor.MapSettings.SLIDER_MAX);
            seed_v = java.lang.Math.floorMod(seed_v, com.oddlabs.tt.mapeditor.MapSettings.SEED_CARDINALITY);
            this.size_f = size_v;
            this.terrain_f = terrain_v;
            this.hills_f = hills_v;
            this.trees_f = trees_v;
            this.supplies_f = supplies_v;
            this.seed_f = seed_v;
        }
        public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is MapSettings r)) return false; return true && java.lang.JO.equals(size_f, r.size_f) && java.lang.JO.equals(terrain_f, r.terrain_f) && java.lang.JO.equals(hills_f, r.hills_f) && java.lang.JO.equals(trees_f, r.trees_f) && java.lang.JO.equals(supplies_f, r.supplies_f) && java.lang.JO.equals(seed_f, r.seed_f); }
        public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(size_f); h = 31 * h + java.lang.JO.hashCode(terrain_f); h = 31 * h + java.lang.JO.hashCode(hills_f); h = 31 * h + java.lang.JO.hashCode(trees_f); h = 31 * h + java.lang.JO.hashCode(supplies_f); h = 31 * h + java.lang.JO.hashCode(seed_f); } return h; }
        public override string toString() { return "MapSettings[" + "size=" + java.lang.JO.toString(size_f) + ", terrain=" + java.lang.JO.toString(terrain_f) + ", hills=" + java.lang.JO.toString(hills_f) + ", trees=" + java.lang.JO.toString(trees_f) + ", supplies=" + java.lang.JO.toString(supplies_f) + ", seed=" + java.lang.JO.toString(seed_f) + "]"; }
        public int size_f;
        public int terrain_f;
        public int hills_f;
        public int trees_f;
        public int supplies_f;
        public int seed_f;
        public const int SLIDER_MAX = 20;
        public const int SLIDER_UNIT = 10;
        public const int NUM_SIZES = 6;
        public const int NUM_TERRAINS = 2;
        public static int[] METERS_PER_WORLD;
        public static bool[] ARCHIPELAGO;
        public const int SEED_CARDINALITY = 40000;
        public const int SLIDER_CARDINALITY = 21;
        public const int TERRAIN_TYPE_CARDINALITY = 4;
        public const int TERRAIN_TYPE_CARDINALITY_LEGACY = 2;
        public const int SIZE_CARDINALITY = 7;
        public const int SIZE_CARDINALITY_LEGACY = 4;
        // Java: 56
        public static com.oddlabs.tt.mapeditor.MapSettings random(java.util.Random random_v)
        {
            return new com.oddlabs.tt.mapeditor.MapSettings(com.oddlabs.matchmaking.Game.SIZE_MEDIUM, com.oddlabs.matchmaking.Game.TERRAIN_TYPE_NATIVE, random_v.nextInt(com.oddlabs.tt.mapeditor.MapSettings.SLIDER_CARDINALITY), random_v.nextInt(com.oddlabs.tt.mapeditor.MapSettings.SLIDER_CARDINALITY), random_v.nextInt(com.oddlabs.tt.mapeditor.MapSettings.SLIDER_CARDINALITY), random_v.nextInt(com.oddlabs.tt.mapeditor.MapSettings.SEED_CARDINALITY));
        }
        // Java: 62
        public com.oddlabs.tt.mapeditor.MapSettings withSize(int size_v)
        {
            return new com.oddlabs.tt.mapeditor.MapSettings(size_v, terrain_f, hills_f, trees_f, supplies_f, seed_f);
        }
        // Java: 66
        public com.oddlabs.tt.mapeditor.MapSettings withTerrain(int terrain_v)
        {
            return new com.oddlabs.tt.mapeditor.MapSettings(size_f, terrain_v, hills_f, trees_f, supplies_f, seed_f);
        }
        // Java: 70
        public com.oddlabs.tt.mapeditor.MapSettings withHills(int hills_v)
        {
            return new com.oddlabs.tt.mapeditor.MapSettings(size_f, terrain_f, hills_v, trees_f, supplies_f, seed_f);
        }
        // Java: 74
        public com.oddlabs.tt.mapeditor.MapSettings withTrees(int trees_v)
        {
            return new com.oddlabs.tt.mapeditor.MapSettings(size_f, terrain_f, hills_f, trees_v, supplies_f, seed_f);
        }
        // Java: 78
        public com.oddlabs.tt.mapeditor.MapSettings withSupplies(int supplies_v)
        {
            return new com.oddlabs.tt.mapeditor.MapSettings(size_f, terrain_f, hills_f, trees_f, supplies_v, seed_f);
        }
        // Java: 82
        public int getMetersPerWorld()
        {
            return com.oddlabs.tt.mapeditor.MapSettings.METERS_PER_WORLD[size_f];
        }
        // Java: 87
        public float getHeightScale()
        {
            return ((getMetersPerWorld()) switch { 256 => (float)(32.0f), 512 => (float)(48.0f), 1024 => (float)(64.0f), _ => (float)(56.0f) });
        }
        // Java: 100
        public float getAccessThreshold()
        {
            return ((getMetersPerWorld()) switch { 256 => (float)(0.05f), 512 => (float)(0.0375f), 1024 => (float)(0.025f), _ => (float)(0.0325f) });
        }
        // Java: 109
        public bool isArchipelago()
        {
            return com.oddlabs.tt.mapeditor.MapSettings.ARCHIPELAGO[size_f] && com.oddlabs.tt.global.Globals.SHIPS_ENABLED;
        }
        // Java: 113
        public com.oddlabs.tt.resource.IslandGenerator createGenerator()
        {
            return createGenerator(null);
        }
        // Java: 120
        public com.oddlabs.tt.resource.IslandGenerator createGenerator(com.oddlabs.tt.procedural.LandscapeOverride.Source @override)
        {
            return new com.oddlabs.tt.resource.IslandGenerator(getMetersPerWorld(), com.oddlabs.tt.procedural.Landscape.TerrainType.values()[terrain_f], (float)(hills_f / ((float)(com.oddlabs.tt.mapeditor.MapSettings.SLIDER_UNIT))), (float)(trees_f / ((float)(com.oddlabs.tt.mapeditor.MapSettings.SLIDER_UNIT))), (float)(supplies_f / ((float)(com.oddlabs.tt.mapeditor.MapSettings.SLIDER_UNIT))), seed_f * seed_f, isArchipelago(), @override);
        }
        // Java: 127
        public string toMapcode()
        {
            java.math.BigInteger result = java.math.BigInteger.ZERO;
            java.math.BigInteger weight = java.math.BigInteger.ONE;
            int[] digits = new int[] { seed_f, hills_f, trees_f, supplies_f, terrain_f, size_f };
            int[] cardinalities = new int[] { com.oddlabs.tt.mapeditor.MapSettings.SEED_CARDINALITY, com.oddlabs.tt.mapeditor.MapSettings.SLIDER_CARDINALITY, com.oddlabs.tt.mapeditor.MapSettings.SLIDER_CARDINALITY, com.oddlabs.tt.mapeditor.MapSettings.SLIDER_CARDINALITY, com.oddlabs.tt.mapeditor.MapSettings.TERRAIN_TYPE_CARDINALITY, com.oddlabs.tt.mapeditor.MapSettings.SIZE_CARDINALITY };
            for (int i = 0; i < digits.Length; i++)
            {
                result = result.add(java.math.BigInteger.valueOf(digits[i]).multiply(weight));
                weight = weight.multiply(java.math.BigInteger.valueOf(cardinalities[i]));
            }
            return com.oddlabs.tt.util.WordsEncoding.encode(result);
        }
        // Java: 146
        public com.oddlabs.tt.mapeditor.MapSettings parseMapcode(string text)
        {
            string code = text.trim();
            if (code.isEmpty())
            {
                return null;
            }
            try
            {
                if (code.indexOf(' ') == (-1))
                {
                    return fromDigits(com.oddlabs.registration.RegistrationKey.parseBits(code.toUpperCase()), com.oddlabs.tt.mapeditor.MapSettings.SIZE_CARDINALITY_LEGACY, com.oddlabs.tt.mapeditor.MapSettings.TERRAIN_TYPE_CARDINALITY_LEGACY);
                }
                return fromDigits(com.oddlabs.tt.util.WordsEncoding.decode(code), com.oddlabs.tt.mapeditor.MapSettings.SIZE_CARDINALITY, com.oddlabs.tt.mapeditor.MapSettings.TERRAIN_TYPE_CARDINALITY);
            }
            catch (System.ArgumentException unused)
            {
                return null;
            }
        }
        // Java: 162
        public com.oddlabs.tt.mapeditor.MapSettings fromDigits(java.math.BigInteger @value, int size_cardinality, int terrain_cardinality)
        {
            java.math.BigInteger[] qr = @value.divideAndRemainder(java.math.BigInteger.valueOf(com.oddlabs.tt.mapeditor.MapSettings.SEED_CARDINALITY));
            int new_seed = qr[1].intValue();
            qr = qr[0].divideAndRemainder(java.math.BigInteger.valueOf(com.oddlabs.tt.mapeditor.MapSettings.SLIDER_CARDINALITY));
            int new_hills = qr[1].intValue();
            qr = qr[0].divideAndRemainder(java.math.BigInteger.valueOf(com.oddlabs.tt.mapeditor.MapSettings.SLIDER_CARDINALITY));
            int new_trees = qr[1].intValue();
            qr = qr[0].divideAndRemainder(java.math.BigInteger.valueOf(com.oddlabs.tt.mapeditor.MapSettings.SLIDER_CARDINALITY));
            int new_supplies = qr[1].intValue();
            qr = qr[0].divideAndRemainder(java.math.BigInteger.valueOf(terrain_cardinality));
            int new_terrain = qr[1].intValue();
            qr = qr[0].divideAndRemainder(java.math.BigInteger.valueOf(size_cardinality));
            int new_size = qr[1].intValue();
            return new com.oddlabs.tt.mapeditor.MapSettings((new_size < com.oddlabs.tt.mapeditor.MapSettings.NUM_SIZES ? new_size : size_f), (new_terrain < com.oddlabs.tt.mapeditor.MapSettings.NUM_TERRAINS ? new_terrain : terrain_f), new_hills, new_trees, new_supplies, new_seed);
        }
        static MapSettings()
        {
            MapSettings.METERS_PER_WORLD = new int[] { 256, 512, 1024, 2048, 4096, 2048 };
            MapSettings.ARCHIPELAGO = new bool[] { false, false, false, false, false, true };
        }
    }
}
