// Translated from Sha256.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.islandwars
{
    // Java: 10
    public class Sha256 : java.lang.Object
    {
        public static int[] K;
        // Java: 24
        public static int rotr(int x, int n)
        {
            return (java.lang.JC.ushr(x, n)) | (x << (32 - n));
        }
        public static char[] HEX;
        // Java: 31
        public static sbyte[] utf8(string s)
        {
            int n = 0;
            for (int i = 0; i < s.length(); i++)
            {
                int c = s.charAt(i);
                n += (c < 128 ? 1 : (c < 2048 ? 2 : 3));
            }
            sbyte[] @out = new sbyte[n];
            int k = 0;
            for (int i_2 = 0; i_2 < s.length(); i_2++)
            {
                int c_2 = s.charAt(i_2);
                if (c_2 < 128)
                {
                    @out[k++] = unchecked((sbyte)(c_2));
                }
                else if (c_2 < 2048)
                {
                    @out[k++] = unchecked((sbyte)((192 | c_2 >> 6)));
                    @out[k++] = unchecked((sbyte)((128 | c_2 & 63)));
                }
                else
                {
                    @out[k++] = unchecked((sbyte)((224 | c_2 >> 12)));
                    @out[k++] = unchecked((sbyte)((128 | c_2 >> 6 & 63)));
                    @out[k++] = unchecked((sbyte)((128 | c_2 & 63)));
                }
            }
            return @out;
        }
        // Java: 55
        public static string fromUtf8(sbyte[] b)
        {
            java.lang.StringBuilder sb = new java.lang.StringBuilder(b.Length);
            for (int i = 0; i < b.Length; i++)
            {
                int c = b[i] & 255;
                if (c < 128)
                {
                    sb.append(unchecked((char)(c)));
                }
                else if (c < 224 && i + 1 < b.Length)
                {
                    sb.append(unchecked((char)(((c & 31) << 6 | b[++i] & 63))));
                }
                else if (i + 2 < b.Length)
                {
                    int c2 = b[++i] & 63;
                    int c3 = b[++i] & 63;
                    sb.append(unchecked((char)(((c & 15) << 12 | c2 << 6 | c3))));
                }
            }
            return java.lang.JO.toString(sb);
        }
        // Java: 71
        public static sbyte[] hash(sbyte[] msg)
        {
            int[] h = new int[] { 1779033703, (-1150833019), 1013904242, (-1521486534), 1359893119, (-1694144372), 528734635, 1541459225 };
            long bits = ((long)(msg.Length)) * 8;
            int padded = ((msg.Length + 9 + 63) / 64) * 64;
            sbyte[] m = new sbyte[padded];
            java.lang.JSystem.arraycopy(msg, 0, m, 0, msg.Length);
            m[msg.Length] = unchecked((sbyte)(128));
            for (int i = 0; i < 8; i++)
            {
                m[padded - 1 - i] = unchecked((sbyte)((java.lang.JC.ushr(bits, (8 * i)))));
            }
            int[] w = new int[64];
            for (int off = 0; off < padded; off += 64)
            {
                for (int i_2 = 0; i_2 < 16; i_2++)
                {
                    w[i_2] = (m[off + 4 * i_2] & 255) << 24 | (m[off + 4 * i_2 + 1] & 255) << 16 | (m[off + 4 * i_2 + 2] & 255) << 8 | (m[off + 4 * i_2 + 3] & 255);
                }
                for (int i_3 = 16; i_3 < 64; i_3++)
                {
                    int s0 = com.oddlabs.tt.islandwars.Sha256.rotr(w[i_3 - 15], 7) ^ com.oddlabs.tt.islandwars.Sha256.rotr(w[i_3 - 15], 18) ^ (java.lang.JC.ushr(w[i_3 - 15], 3));
                    int s1 = com.oddlabs.tt.islandwars.Sha256.rotr(w[i_3 - 2], 17) ^ com.oddlabs.tt.islandwars.Sha256.rotr(w[i_3 - 2], 19) ^ (java.lang.JC.ushr(w[i_3 - 2], 10));
                    w[i_3] = w[i_3 - 16] + s0 + w[i_3 - 7] + s1;
                }
                int a = h[0];
                int b = h[1];
                int c = h[2];
                int d = h[3];
                int e = h[4];
                int f = h[5];
                int g = h[6];
                int hh = h[7];
                for (int i_4 = 0; i_4 < 64; i_4++)
                {
                    int s1_2 = com.oddlabs.tt.islandwars.Sha256.rotr(e, 6) ^ com.oddlabs.tt.islandwars.Sha256.rotr(e, 11) ^ com.oddlabs.tt.islandwars.Sha256.rotr(e, 25);
                    int ch = (e & f) ^ (~e & g);
                    int t1 = hh + s1_2 + ch + com.oddlabs.tt.islandwars.Sha256.K[i_4] + w[i_4];
                    int s0_2 = com.oddlabs.tt.islandwars.Sha256.rotr(a, 2) ^ com.oddlabs.tt.islandwars.Sha256.rotr(a, 13) ^ com.oddlabs.tt.islandwars.Sha256.rotr(a, 22);
                    int maj = (a & b) ^ (a & c) ^ (b & c);
                    int t2 = s0_2 + maj;
                    hh = g;
                    g = f;
                    f = e;
                    e = d + t1;
                    d = c;
                    c = b;
                    b = a;
                    a = t1 + t2;
                }
                h[0] += a;
                h[1] += b;
                h[2] += c;
                h[3] += d;
                h[4] += e;
                h[5] += f;
                h[6] += g;
                h[7] += hh;
            }
            sbyte[] @out = new sbyte[32];
            for (int i_5 = 0; i_5 < 8; i_5++)
            {
                for (int j = 0; j < 4; j++)
                {
                    @out[4 * i_5 + j] = unchecked((sbyte)((java.lang.JC.ushr(h[i_5], (24 - 8 * j)))));
                }
            }
            return @out;
        }
        // Java: 123
        public static string hmacHex(string key, string msg)
        {
            sbyte[] k = com.oddlabs.tt.islandwars.Sha256.utf8(key);
            if (k.Length > 64)
            {
                k = com.oddlabs.tt.islandwars.Sha256.hash(k);
            }
            sbyte[] ipad = new sbyte[64];
            sbyte[] opad = new sbyte[64];
            for (int i = 0; i < 64; i++)
            {
                sbyte b = (i < k.Length ? k[i] : unchecked((sbyte)(0)));
                ipad[i] = unchecked((sbyte)((b ^ 54)));
                opad[i] = unchecked((sbyte)((b ^ 92)));
            }
            sbyte[] m = com.oddlabs.tt.islandwars.Sha256.utf8(msg);
            sbyte[] inner = new sbyte[64 + m.Length];
            java.lang.JSystem.arraycopy(ipad, 0, inner, 0, 64);
            java.lang.JSystem.arraycopy(m, 0, inner, 64, m.Length);
            sbyte[] ih = com.oddlabs.tt.islandwars.Sha256.hash(inner);
            sbyte[] outer = new sbyte[96];
            java.lang.JSystem.arraycopy(opad, 0, outer, 0, 64);
            java.lang.JSystem.arraycopy(ih, 0, outer, 64, 32);
            return com.oddlabs.tt.islandwars.Sha256.hex(com.oddlabs.tt.islandwars.Sha256.hash(outer));
        }
        // Java: 144
        public static string hex(sbyte[] b)
        {
            java.lang.StringBuilder sb = new java.lang.StringBuilder(b.Length * 2);
            foreach (sbyte x in b)
            {
                sb.append(com.oddlabs.tt.islandwars.Sha256.HEX[(x >> 4) & 15]);
                sb.append(com.oddlabs.tt.islandwars.Sha256.HEX[x & 15]);
            }
            return java.lang.JO.toString(sb);
        }
        static Sha256()
        {
            Sha256.K = new int[] { 1116352408, 1899447441, (-1245643825), (-373957723), 961987163, 1508970993, (-1841331548), (-1424204075), (-670586216), 310598401, 607225278, 1426881987, 1925078388, (-2132889090), (-1680079193), (-1046744716), (-459576895), (-272742522), 264347078, 604807628, 770255983, 1249150122, 1555081692, 1996064986, (-1740746414), (-1473132947), (-1341970488), (-1084653625), (-958395405), (-710438585), 113926993, 338241895, 666307205, 773529912, 1294757372, 1396182291, 1695183700, 1986661051, (-2117940946), (-1838011259), (-1564481375), (-1474664885), (-1035236496), (-949202525), (-778901479), (-694614492), (-200395387), 275423344, 430227734, 506948616, 659060556, 883997877, 958139571, 1322822218, 1537002063, 1747873779, 1955562222, 2024104815, (-2067236844), (-1933114872), (-1866530822), (-1538233109), (-1090935817), (-965641998) };
            Sha256.HEX = "0123456789abcdef".toCharArray();
        }
        public Sha256() : base()
        {
        }
    }
}
