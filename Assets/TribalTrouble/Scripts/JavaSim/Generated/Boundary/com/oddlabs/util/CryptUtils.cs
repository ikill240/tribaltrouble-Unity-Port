// Hand-written replacement of the generated stub: CryptUtils.digest exactly as Java computes it (the login password
// digest the matchmaking server compares) - SHA-1, as BigInteger(1, hash).toString(16), i.e. lowercase hex without
// leading zeros ("buggyDigest" in the Java source, which is what the game has always sent).
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.util
{
    public class CryptUtils : java.lang.Object
    {
        public static readonly int PASSWORD_DIGEST_LENGTH = 40;

        public static string digest(string str)
        {
            byte[] hash;
            using (var sha = System.Security.Cryptography.SHA1.Create()) hash = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(str));
            var sb = new System.Text.StringBuilder();
            foreach (var b in hash) sb.Append(b.ToString("x2"));
            string s = sb.ToString().TrimStart('0');
            return s.Length == 0 ? "0" : s;
        }

        public CryptUtils() { }
    }
}
