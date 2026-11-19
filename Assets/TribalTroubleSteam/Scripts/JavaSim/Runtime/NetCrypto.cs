// The slice of java.security / javax.crypto that SecureConnection and KeyManager use: Diffie-Hellman key agreement
// with X.509 / PKCS#8 encoded keys, DESede/ECB/PKCS5Padding, SealedObject, plus MessageDigest. Byte formats follow
// the JDK's SunJCE provider so a Java server and the port agree on the session key.
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;

namespace java.security
{
    public interface Key { string getAlgorithm(); string getFormat(); sbyte[] getEncoded(); }
    public interface PublicKey : Key { }
    public interface PrivateKey : Key { }

    public sealed class KeyPair : java.lang.Object
    {
        readonly PublicKey pub;
        readonly PrivateKey priv;
        public KeyPair(PublicKey pub, PrivateKey priv) { this.pub = pub; this.priv = priv; }
        public PublicKey getPublic() => pub;
        public PrivateKey getPrivate() => priv;
    }

    public class GeneralSecurityException : Exception
    {
        public GeneralSecurityException(string m) : base(m) { }
    }

    public sealed class KeyPairGenerator : java.lang.Object
    {
        javax.crypto.spec.DHParameterSpec spec;
        public static KeyPairGenerator getInstance(string alg)
        {
            if (alg != "DH" && alg != "DiffieHellman") throw new GeneralSecurityException("NoSuchAlgorithmException: " + alg);
            return new KeyPairGenerator();
        }
        public void initialize(spec.AlgorithmParameterSpec p) => spec = (javax.crypto.spec.DHParameterSpec)p;
        public void initialize(spec.AlgorithmParameterSpec p, SecureRandom r) => initialize(p);
        public KeyPair generateKeyPair()
        {
            if (spec == null) throw new GeneralSecurityException("DH key pair generator not initialized");
            var x = javax.crypto.DH.RandomExponent(spec);
            var y = BigInteger.ModPow(spec.G, x, spec.P);
            return new KeyPair(new javax.crypto.DH.Public(spec, y), new javax.crypto.DH.Private(spec, x));
        }
        public KeyPair genKeyPair() => generateKeyPair();
    }

    public sealed class KeyFactory : java.lang.Object
    {
        public static KeyFactory getInstance(string alg)
        {
            if (alg != "DH" && alg != "DiffieHellman") throw new GeneralSecurityException("NoSuchAlgorithmException: " + alg);
            return new KeyFactory();
        }
        public PublicKey generatePublic(spec.KeySpec s) => javax.crypto.DH.DecodePublic(((spec.X509EncodedKeySpec)s).getEncoded());
        public PrivateKey generatePrivate(spec.KeySpec s) => javax.crypto.DH.DecodePrivate(((spec.PKCS8EncodedKeySpec)s).getEncoded());
    }

    /// <summary>
    /// DH parameters for the embedded matchmaking server. The Java server searches a fresh 1024-bit safe prime at
    /// start-up; the port uses the standard 1024-bit MODP group (RFC 2409 Oakley group 2, g = 2), a safe prime of the
    /// same size. Clients take the parameters from the server's public key either way.
    /// </summary>
    public sealed class AlgorithmParameterGenerator : java.lang.Object
    {
        const string Oakley2 =
            "FFFFFFFFFFFFFFFFC90FDAA22168C234C4C6628B80DC1CD129024E088A67CC74020BBEA63B139B22514A08798E3404DD" +
            "EF9519B3CD3A431B302B0A6DF25F14374FE1356D6D51C245E485B576625E7EC6F44C42E9A637ED6B0BFF5CB6F406B7ED" +
            "EE386BFB5A899FA5AE9F24117C4B1FE649286651ECE65381FFFFFFFFFFFFFFFF";
        public static AlgorithmParameterGenerator getInstance(string alg)
        {
            if (alg != "DH" && alg != "DiffieHellman") throw new GeneralSecurityException("NoSuchAlgorithmException: " + alg);
            return new AlgorithmParameterGenerator();
        }
        public void init(int size) { }
        public AlgorithmParameters generateParameters()
        {
            var p = BigInteger.Parse("0" + Oakley2, System.Globalization.NumberStyles.HexNumber);
            return new AlgorithmParameters(new javax.crypto.spec.DHParameterSpec(p, new BigInteger(2)));
        }
    }

    public sealed class AlgorithmParameters : java.lang.Object
    {
        readonly object spec;
        public AlgorithmParameters(object spec) { this.spec = spec; }
        public object getParameterSpec(System.Type t) => spec;
    }

    public class SecureRandom : java.lang.Object
    {
        public void nextBytes(sbyte[] b)
        {
            var r = new byte[b.Length];
            using (var g = RandomNumberGenerator.Create()) g.GetBytes(r);
            Buffer.BlockCopy(r, 0, b, 0, r.Length);
        }
        public int nextInt() { var b = new sbyte[4]; nextBytes(b); return BitConverter.ToInt32((byte[])(System.Array)b, 0); }
    }

    public sealed class MessageDigest : java.lang.Object
    {
        readonly HashAlgorithm h;
        readonly string alg;
        MessageDigest(string alg, HashAlgorithm h) { this.alg = alg; this.h = h; }
        public static MessageDigest getInstance(string alg)
        {
            switch (alg.ToUpperInvariant())
            {
                case "SHA-256": case "SHA256": return new MessageDigest(alg, SHA256.Create());
                case "SHA-1": case "SHA": case "SHA1": return new MessageDigest(alg, SHA1.Create());
                case "MD5": return new MessageDigest(alg, MD5.Create());
                case "SHA-512": return new MessageDigest(alg, SHA512.Create());
            }
            throw new GeneralSecurityException("NoSuchAlgorithmException: " + alg);
        }
        public string getAlgorithm() => alg;
        public void update(sbyte b) => update(new[] { b }, 0, 1);
        public void update(sbyte[] b) => update(b, 0, b.Length);
        public void update(sbyte[] b, int off, int len) => h.TransformBlock((byte[])(System.Array)b, off, len, null, 0);
        public sbyte[] digest()
        {
            h.TransformFinalBlock(new byte[0], 0, 0);
            var r = (sbyte[])(System.Array)h.Hash;
            h.Initialize();
            return r;
        }
        public sbyte[] digest(sbyte[] b) { update(b); return digest(); }
        public void reset() => h.Initialize();
        public static bool isEqual(sbyte[] a, sbyte[] b)
        {
            if (a == null || b == null || a.Length != b.Length) return false;
            int d = 0;
            for (int i = 0; i < a.Length; i++) d |= a[i] ^ b[i];
            return d == 0;
        }
    }

    /// <summary>java.security.SignedObject as data (the field names are its serialized form). Signing is not used by the game client.</summary>
    public sealed class SignedObject : java.lang.Object
    {
        public sbyte[] content;
        public sbyte[] signature;
        public string thealgorithm;
        SignedObject() { }
        public string getAlgorithm() => thealgorithm;
        public sbyte[] getSignature() => (sbyte[])signature?.Clone();
        public object getObject() => new java.io.ObjectInputStream(new java.io.ByteArrayInputStream(content)).readObject();
    }

    public sealed class CodeSource : java.lang.Object
    {
        public java.net.URL getLocation() => null;
    }

    namespace spec
    {
        public interface AlgorithmParameterSpec { }
        public interface KeySpec { }
        public class EncodedKeySpec : java.lang.Object, KeySpec
        {
            readonly sbyte[] encoded;
            public EncodedKeySpec(sbyte[] e) { encoded = (sbyte[])e.Clone(); }
            public sbyte[] getEncoded() => (sbyte[])encoded.Clone();
        }
        public sealed class X509EncodedKeySpec : EncodedKeySpec { public X509EncodedKeySpec(sbyte[] e) : base(e) { } public string getFormat() => "X.509"; }
        public sealed class PKCS8EncodedKeySpec : EncodedKeySpec { public PKCS8EncodedKeySpec(sbyte[] e) : base(e) { } public string getFormat() => "PKCS#8"; }
    }
}

namespace javax.crypto
{
    using java.security;

    public interface SecretKey : Key { }

    public sealed class SecretKeySpec : java.lang.Object, SecretKey
    {
        readonly sbyte[] key;
        readonly string alg;
        public SecretKeySpec(sbyte[] key, string alg) { this.key = (sbyte[])key.Clone(); this.alg = alg; }
        public string getAlgorithm() => alg;
        public string getFormat() => "RAW";
        public sbyte[] getEncoded() => (sbyte[])key.Clone();
    }

    namespace interfaces
    {
        public interface DHKey { spec.DHParameterSpec getParams(); }
        public interface DHPublicKey : DHKey, PublicKey { }
        public interface DHPrivateKey : DHKey, PrivateKey { }
    }

    namespace spec
    {
        public sealed class DHParameterSpec : java.lang.Object, java.security.spec.AlgorithmParameterSpec
        {
            public readonly BigInteger P, G;
            public readonly int L;
            public DHParameterSpec(BigInteger p, BigInteger g, int l = 0) { P = p; G = g; L = l; }
            public int getL() => L;
        }
    }

    /// <summary>Diffie-Hellman keys in the SunJCE encodings.</summary>
    public static class DH
    {
        // OID 1.2.840.113549.1.3.1 (PKCS #3 dhKeyAgreement)
        static readonly byte[] OID = { 0x2a, 0x86, 0x48, 0x86, 0xf7, 0x0d, 0x01, 0x03, 0x01 };

        public sealed class Public : java.lang.Object, interfaces.DHPublicKey
        {
            public readonly spec.DHParameterSpec Params;
            public readonly BigInteger Y;
            public Public(spec.DHParameterSpec p, BigInteger y) { Params = p; Y = y; }
            public spec.DHParameterSpec getParams() => Params;
            public string getAlgorithm() => "DH";
            public string getFormat() => "X.509";
            public sbyte[] getEncoded() => (sbyte[])(System.Array)EncodePublic(this);
        }

        public sealed class Private : java.lang.Object, interfaces.DHPrivateKey
        {
            public readonly spec.DHParameterSpec Params;
            public readonly BigInteger X;
            public Private(spec.DHParameterSpec p, BigInteger x) { Params = p; X = x; }
            public spec.DHParameterSpec getParams() => Params;
            public string getAlgorithm() => "DH";
            public string getFormat() => "PKCS#8";
            public sbyte[] getEncoded() => (sbyte[])(System.Array)EncodePrivate(this);
        }

        public static BigInteger RandomExponent(spec.DHParameterSpec s)
        {
            int pBits = BitLength(s.P);
            int bits = s.L > 0 ? s.L : pBits - 1;
            var bytes = new byte[(bits + 7) / 8 + 1];
            using (var g = RandomNumberGenerator.Create())
            {
                while (true)
                {
                    g.GetBytes(bytes);
                    bytes[bytes.Length - 1] = 0;                                     // little-endian sign byte: positive
                    int extra = (bytes.Length - 1) * 8 - bits;
                    bytes[bytes.Length - 2] &= (byte)(0xff >> extra);
                    var x = new BigInteger(bytes);
                    if (x > 1 && x < s.P - 1) return x;
                }
            }
        }

        public static int BitLength(BigInteger v)
        {
            var b = v.ToByteArray();
            int n = (b.Length - 1) * 8;
            for (int top = b[b.Length - 1]; top != 0; top >>= 1) n++;
            return n;
        }

        /// <summary>Unsigned big-endian magnitude.</summary>
        public static byte[] Magnitude(BigInteger v)
        {
            var le = v.ToByteArray();
            int n = le.Length;
            while (n > 1 && le[n - 1] == 0) n--;
            var be = new byte[n];
            for (int i = 0; i < n; i++) be[i] = le[n - 1 - i];
            return be;
        }

        public static BigInteger FromMagnitude(byte[] be, int off, int len)
        {
            var le = new byte[len + 1];
            for (int i = 0; i < len; i++) le[i] = be[off + len - 1 - i];
            return new BigInteger(le);
        }

        // ------------------------------------------------------------ DER
        static void Len(List<byte> o, int n)
        {
            if (n < 0x80) o.Add((byte)n);
            else if (n < 0x100) { o.Add(0x81); o.Add((byte)n); }
            else if (n < 0x10000) { o.Add(0x82); o.Add((byte)(n >> 8)); o.Add((byte)n); }
            else { o.Add(0x83); o.Add((byte)(n >> 16)); o.Add((byte)(n >> 8)); o.Add((byte)n); }
        }
        static byte[] Tlv(byte tag, byte[] body) { var o = new List<byte> { tag }; Len(o, body.Length); foreach (var x in body) o.Add(x); return o.ToArray(); }
        static byte[] Cat(params byte[][] parts) { var o = new List<byte>(); foreach (var p in parts) foreach (var x in p) o.Add(x); return o.ToArray(); }
        static byte[] Int(BigInteger v)
        {
            var m = Magnitude(v);
            if ((m[0] & 0x80) != 0) m = Cat(new byte[] { 0 }, m);   // DER INTEGER is signed
            return Tlv(0x02, m);
        }
        static byte[] Int(int v) => Int(new BigInteger(v));

        static byte[] AlgId(spec.DHParameterSpec s)
        {
            var ps = s.L != 0 ? Cat(Int(s.P), Int(s.G), Int(s.L)) : Cat(Int(s.P), Int(s.G));
            return Tlv(0x30, Cat(Tlv(0x06, OID), Tlv(0x30, ps)));
        }

        public static byte[] EncodePublic(Public k) =>
            Tlv(0x30, Cat(AlgId(k.Params), Tlv(0x03, Cat(new byte[] { 0 }, Int(k.Y)))));

        public static byte[] EncodePrivate(Private k) =>
            Tlv(0x30, Cat(Int(0), AlgId(k.Params), Tlv(0x04, Int(k.X))));

        sealed class Der
        {
            readonly byte[] b;
            public int pos;
            public Der(byte[] b) { this.b = b; }
            public (byte tag, int start, int len) Next()
            {
                byte tag = b[pos++];
                int n = b[pos++];
                if ((n & 0x80) != 0)
                {
                    int k = n & 0x7f;
                    n = 0;
                    for (int i = 0; i < k; i++) n = (n << 8) | b[pos++];
                }
                int start = pos;
                pos += n;
                return (tag, start, n);
            }
            public void Enter(int start) => pos = start;
            public BigInteger Integer()
            {
                var (t, s, n) = Next();
                if (t != 0x02) throw new GeneralSecurityException("InvalidKeySpecException: expected INTEGER");
                return FromMagnitude(b, s, n);
            }
        }

        static spec.DHParameterSpec ParseAlgId(Der d)
        {
            var (t, s, n) = d.Next();
            if (t != 0x30) throw new GeneralSecurityException("InvalidKeySpecException: bad AlgorithmIdentifier");
            int end = s + n;
            d.Enter(s);
            d.Next();                                 // OID
            var (pt, ps, pn) = d.Next();
            if (pt != 0x30) throw new GeneralSecurityException("InvalidKeySpecException: bad DH parameters");
            d.Enter(ps);
            var p = d.Integer();
            var g = d.Integer();
            int l = 0;
            if (d.pos < ps + pn) l = (int)d.Integer();
            d.pos = end;
            return new spec.DHParameterSpec(p, g, l);
        }

        public static Public DecodePublic(sbyte[] enc)
        {
            try
            {
                var d = new Der((byte[])(System.Array)enc);
                var (t, s, n) = d.Next();
                if (t != 0x30) throw new GeneralSecurityException("InvalidKeySpecException: not a SubjectPublicKeyInfo");
                d.Enter(s);
                var ps = ParseAlgId(d);
                var (bt, bs, bn) = d.Next();
                if (bt != 0x03) throw new GeneralSecurityException("InvalidKeySpecException: expected BIT STRING");
                d.Enter(bs + 1);
                return new Public(ps, d.Integer());
            }
            catch (IndexOutOfRangeException) { throw new GeneralSecurityException("InvalidKeySpecException: truncated key"); }
        }

        public static Private DecodePrivate(sbyte[] enc)
        {
            var d = new Der((byte[])(System.Array)enc);
            var (t, s, n) = d.Next();
            d.Enter(s);
            d.Integer();                              // version
            var ps = ParseAlgId(d);
            var (ot, os, on) = d.Next();
            d.Enter(os);
            return new Private(ps, d.Integer());
        }
    }

    public sealed class KeyAgreement : java.lang.Object
    {
        DH.Private priv;
        byte[] secret;
        public static KeyAgreement getInstance(string alg)
        {
            if (alg != "DH" && alg != "DiffieHellman") throw new GeneralSecurityException("NoSuchAlgorithmException: " + alg);
            return new KeyAgreement();
        }
        public void init(Key k) { priv = (DH.Private)k; secret = null; }
        public void init(Key k, java.security.spec.AlgorithmParameterSpec p) => init(k);
        public Key doPhase(Key k, bool lastPhase)
        {
            var pub = (DH.Public)k;
            if (pub.Params.P != priv.Params.P || pub.Params.G != priv.Params.G) throw new GeneralSecurityException("InvalidKeyException: incompatible parameters");
            var y = pub.Y;
            if (y <= 1 || y >= priv.Params.P - 1) throw new GeneralSecurityException("InvalidKeyException: public key out of range");
            var s = BigInteger.ModPow(y, priv.X, priv.Params.P);
            // SunJCE pads the shared secret with leading zeros to the modulus length
            int len = (DH.BitLength(priv.Params.P) + 7) / 8;
            var m = DH.Magnitude(s);
            secret = new byte[len];
            Buffer.BlockCopy(m, 0, secret, len - m.Length, m.Length);
            return null;
        }
        public sbyte[] generateSecret()
        {
            var r = (sbyte[])(System.Array)secret;
            secret = null;
            return r;
        }
        /// <summary>"DESede": the first 24 bytes of the shared secret (SunJCE DHKeyAgreement / DESedeKeySpec).</summary>
        public SecretKey generateSecret(string alg)
        {
            if (secret == null) throw new InvalidOperationException("key agreement not complete");
            int n;
            switch (alg.ToUpperInvariant())
            {
                case "DESEDE": case "TRIPLEDES": n = 24; break;
                case "DES": n = 8; break;
                case "AES": n = 16; break;
                default: throw new GeneralSecurityException("NoSuchAlgorithmException: " + alg);
            }
            var k = new sbyte[n];
            Buffer.BlockCopy(secret, 0, k, 0, n);
            if (n == 24 || n == 8) for (int i = 0; i < n; i++) k[i] = unchecked((sbyte)OddParity((byte)k[i]));
            secret = null;
            return new SecretKeySpec(k, alg == "TripleDES" ? "DESede" : alg);
        }
        static int OddParity(byte b)
        {
            int ones = 0;
            for (int v = b >> 1; v != 0; v >>= 1) ones += v & 1;
            return (b & 0xfe) | ((ones & 1) == 0 ? 1 : 0);
        }
    }

    public sealed class Cipher : java.lang.Object
    {
        public const int ENCRYPT_MODE = 1, DECRYPT_MODE = 2, WRAP_MODE = 3, UNWRAP_MODE = 4;
        readonly string transformation;
        ICryptoTransform t;
        readonly List<byte> pending = new List<byte>();

        Cipher(string tr) { transformation = tr; }
        public static Cipher getInstance(string tr)
        {
            if (tr != "DESede/ECB/PKCS5Padding" && tr != "DESede") throw new GeneralSecurityException("NoSuchAlgorithmException: " + tr);
            return new Cipher(tr);
        }
        public string getAlgorithm() => transformation;
        public int getBlockSize() => 8;
        public void init(int mode, Key key)
        {
            var tdes = TripleDES.Create();
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            var k = (byte[])(System.Array)key.getEncoded();
            t = mode == ENCRYPT_MODE ? tdes.CreateEncryptor(k, null) : tdes.CreateDecryptor(k, null);
            pending.Clear();
        }
        public void init(int mode, Key key, java.security.spec.AlgorithmParameterSpec p) => init(mode, key);
        public sbyte[] update(sbyte[] b) { foreach (var x in b) pending.Add(unchecked((byte)x)); return new sbyte[0]; }
        public sbyte[] doFinal(sbyte[] b) { update(b); return doFinal(); }
        public sbyte[] doFinal(sbyte[] b, int off, int len) { var c = new sbyte[len]; java.lang.JSystem.ArrCopy(b, off, c, 0, len); return doFinal(c); }
        public sbyte[] doFinal()
        {
            var input = pending.ToArray();
            pending.Clear();
            try { return (sbyte[])(System.Array)t.TransformFinalBlock(input, 0, input.Length); }
            catch (CryptographicException e) { throw new GeneralSecurityException("BadPaddingException: " + e.Message); }
        }
    }

    /// <summary>javax.crypto.SealedObject: a serialized object encrypted with a Cipher (field names are its serialized form).</summary>
    public sealed class SealedObject : java.lang.Object
    {
        public sbyte[] encodedParams;
        public sbyte[] encryptedContent;
        public string paramsAlg;
        public string sealAlg;

        public SealedObject(object o, Cipher c)
        {
            var bytes = new java.io.ByteArrayOutputStream();
            var oos = new java.io.ObjectOutputStream(bytes);
            oos.writeObject(o);
            oos.flush();
            encryptedContent = c.doFinal(bytes.toByteArray());
            sealAlg = c.getAlgorithm();
        }

        public string getAlgorithm() => sealAlg;

        public object getObject(Cipher c)
        {
            var plain = c.doFinal(encryptedContent);
            return new java.io.ObjectInputStream(new java.io.ByteArrayInputStream(plain)).readObject();
        }
    }
}

namespace javax.crypto
{
    /// <summary>Password-based encryption: only the server's key tools use it.</summary>
    public sealed class SecretKeyFactory : java.lang.Object
    {
        public static SecretKeyFactory getInstance(string alg) => throw new NotSupportedException("password-based encryption is not used by the game client");
        public SecretKey generateSecret(java.security.spec.KeySpec s) => throw new NotSupportedException();
    }
    namespace spec
    {
        public sealed class PBEKeySpec : java.lang.Object, java.security.spec.KeySpec { public PBEKeySpec(char[] p) { } }
        public sealed class PBEParameterSpec : java.lang.Object, java.security.spec.AlgorithmParameterSpec { public PBEParameterSpec(sbyte[] salt, int count) { } }
    }
}
