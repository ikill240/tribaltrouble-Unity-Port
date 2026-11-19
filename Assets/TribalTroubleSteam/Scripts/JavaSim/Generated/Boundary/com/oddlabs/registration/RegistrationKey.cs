// GUI port: the legacy letter map codes (TerrainMenu) need RegistrationKey.parseBits / createString; the signing and
// key-file parts of the Java class stay out of the port.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.registration
{
    public sealed class RegistrationKey : java.lang.Object
    {
        public const string CHAR_TO_WORD = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
        public const string LOWER_CASE_CHARS = "abcdefghjklmnpqrstuvwxyz";
        public const string SEPARATOR = "-";
        public const string ALLOWED_CHARS = CHAR_TO_WORD + SEPARATOR + LOWER_CASE_CHARS;
        const int Shifting = 5; // log2(32)

        public static java.math.BigInteger parseBits(string str)
        {
            var result = System.Numerics.BigInteger.Zero;
            int accum = 0;
            for (int i = str.Length - 1; i >= 0; i--)
            {
                char c = str[i];
                int index = CHAR_TO_WORD.IndexOf(c);
                if (index == -1) throw new RegistrationKeyFormatException(RegistrationKeyFormatException.TYPE_INVALID_CHAR, c);
                result |= new System.Numerics.BigInteger(index) << accum;
                accum += Shifting;
            }
            return java.math.BigInteger.FromSystem(result);
        }

        public static string createString(java.math.BigInteger valJ)
        {
            var val = valJ.ToSystem();
            string result = "";
            int bitLength = valJ.bitLength();
            for (int accum = 0; accum < bitLength; accum += Shifting)
            {
                int ci = (int)((val >> accum) & 31);
                result = CHAR_TO_WORD[ci] + result;
            }
            return result;
        }
        // ---- merged from the generated stub (members the translated code now uses)
        public RegistrationKey() { }
    }
}
