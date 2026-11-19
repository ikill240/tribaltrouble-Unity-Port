// Translated from KeyManager.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.util
{
    // Java: 36
    public class KeyManager : java.lang.Object
    {
        public const int KEY_SIZE = 1024;
        public const string AGREEMENT_ALGORITHM = "DH";
        public const string PASSWORD_ALGORITHM = "PBEWithMD5AndDES";
        // Java: 41
        public static java.security.spec.AlgorithmParameterSpec generateParameterSpec()
        {
            java.security.AlgorithmParameterGenerator paramGen = java.security.AlgorithmParameterGenerator.getInstance(com.oddlabs.util.KeyManager.AGREEMENT_ALGORITHM);
            paramGen.init(com.oddlabs.util.KeyManager.KEY_SIZE);
            java.security.AlgorithmParameters @params = paramGen.generateParameters();
            return ((javax.crypto.spec.DHParameterSpec)@params.getParameterSpec(typeof(javax.crypto.spec.DHParameterSpec)));
        }
        // Java: 48
        public static java.security.KeyPair generateInitialKeyPair(java.security.spec.AlgorithmParameterSpec param_spec)
        {
            try
            {
                java.security.KeyPairGenerator key_pair_gen = java.security.KeyPairGenerator.getInstance(com.oddlabs.util.KeyManager.AGREEMENT_ALGORITHM);
                key_pair_gen.initialize(param_spec);
                java.security.KeyPair key_pair = key_pair_gen.generateKeyPair();
                return key_pair;
            }
            catch (System.Exception e)
            {
                throw new System.Exception(java.lang.JStr.valueOf(e), (System.Exception)e);
            }
        }
        // Java: 59
        public static javax.crypto.Cipher createCipher(int cipher_mode, javax.crypto.KeyAgreement key_agreement, java.security.PublicKey public_key)
        {
            try
            {
                key_agreement.doPhase(public_key, true);
                javax.crypto.SecretKey secret_key = key_agreement.generateSecret("DESede");
                javax.crypto.Cipher cipher = javax.crypto.Cipher.getInstance("DESede/ECB/PKCS5Padding");
                cipher.init(cipher_mode, secret_key);
                return cipher;
            }
            catch (System.Exception e) when (e is System.Exception || e is System.Exception)
            {
                throw new System.Exception(java.lang.JStr.valueOf(e), (System.Exception)e);
            }
        }
        // Java: 72
        public static java.security.PrivateKey readPrivateKey(sbyte[] encoded_private_key, string algorithm)
        {
            try
            {
                java.security.KeyFactory key_factory = java.security.KeyFactory.getInstance(algorithm);
                java.security.spec.KeySpec key_spec = new java.security.spec.PKCS8EncodedKeySpec(encoded_private_key);
                return key_factory.generatePrivate(key_spec);
            }
            catch (System.Exception e)
            {
                throw new System.Exception(java.lang.JStr.valueOf(e), (System.Exception)e);
            }
        }
        // Java: 83
        public static java.security.PublicKey readPublicKey(sbyte[] encoded_public_key, string algorithm)
        {
            try
            {
                java.security.KeyFactory key_factory = java.security.KeyFactory.getInstance(algorithm);
                java.security.spec.KeySpec key_spec = new java.security.spec.X509EncodedKeySpec(encoded_public_key);
                return key_factory.generatePublic(key_spec);
            }
            catch (System.Exception e)
            {
                throw new System.Exception(java.lang.JStr.valueOf(e), (System.Exception)e);
            }
        }
        // Java: 94
        public static javax.crypto.KeyAgreement generateAgreement(java.security.PrivateKey private_key)
        {
            try
            {
                javax.crypto.KeyAgreement key_agreement = javax.crypto.KeyAgreement.getInstance(com.oddlabs.util.KeyManager.AGREEMENT_ALGORITHM);
                key_agreement.init(private_key);
                return key_agreement;
            }
            catch (System.Exception e)
            {
                throw new System.Exception(java.lang.JStr.valueOf(e), (System.Exception)e);
            }
        }
        // Java: 104
        public static java.security.KeyPair generateKeyPairFromKey(java.security.PublicKey public_key)
        {
            if (!((public_key is javax.crypto.interfaces.DHPublicKey)))
            {
                throw new System.Exception("Not a public key");
            }
            try
            {
                java.security.KeyPairGenerator key_pair_gen = java.security.KeyPairGenerator.getInstance(com.oddlabs.util.KeyManager.AGREEMENT_ALGORITHM);
                java.security.spec.AlgorithmParameterSpec dh_param_spec = (((javax.crypto.interfaces.DHKey)public_key)).getParams();
                key_pair_gen.initialize(dh_param_spec);
                return key_pair_gen.generateKeyPair();
            }
            catch (System.Exception e)
            {
                throw new System.Exception(java.lang.JStr.valueOf(e), (System.Exception)e);
            }
        }
        // Java: 117
        public static javax.crypto.Cipher createPasswordCipherFromPassword(char[] password, int mode)
        {
            javax.crypto.spec.PBEKeySpec pbeKeySpec = default;
            javax.crypto.spec.PBEParameterSpec pbeParamSpec = default;
            javax.crypto.SecretKeyFactory keyFac = default;
            sbyte[] salt = new sbyte[] { unchecked((sbyte)(199)), unchecked((sbyte)(115)), unchecked((sbyte)(33)), unchecked((sbyte)(140)), unchecked((sbyte)(126)), unchecked((sbyte)(200)), unchecked((sbyte)(238)), unchecked((sbyte)(153)) };
            int count = 20;
            pbeParamSpec = new javax.crypto.spec.PBEParameterSpec(salt, count);
            pbeKeySpec = new javax.crypto.spec.PBEKeySpec(password);
            keyFac = javax.crypto.SecretKeyFactory.getInstance(com.oddlabs.util.KeyManager.PASSWORD_ALGORITHM);
            javax.crypto.SecretKey pbeKey = keyFac.generateSecret(pbeKeySpec);
            javax.crypto.Cipher pbeCipher = javax.crypto.Cipher.getInstance(com.oddlabs.util.KeyManager.PASSWORD_ALGORITHM);
            pbeCipher.init(mode, pbeKey, pbeParamSpec);
            return pbeCipher;
        }
        // Java: 150
        public static javax.crypto.Cipher createPasswordCipher(string pass_prompt, int mode)
        {
            return com.oddlabs.util.KeyManager.createPasswordCipherFromPassword(com.oddlabs.util.KeyManager.readPassword(pass_prompt, java.lang.JSystem.@in), mode);
        }
        // Java: 158
        public static char[] readPassword(string pass_prompt, java.io.InputStream @in)
        {
            char[] lineBuffer = default;
            char[] buf = default;
            buf = lineBuffer = new char[128];
            int room = buf.Length;
            int offset = 0;
            int c = default;
            java.lang.IO.print(pass_prompt);
            java.lang.JSystem.@out.flush();
            while (true)
            {
                switch ((c = @in.read()))
                {
                    case (-1):
                    case '\n':
                    {
                        goto brk_loop_6;
                    }
                    case '\r':
                    {
                        int c2 = @in.read();
                        if ((c2 != '\n') && (c2 != (-1)))
                        {
                            if (!((@in is java.io.PushbackInputStream)))
                            {
                                @in = new java.io.PushbackInputStream(@in);
                            }
                            (((java.io.PushbackInputStream)@in)).unread(c2);
                        }
                        goto brk_loop_6;
                    }
                    default:
                    {
                        if (--room < 0)
                        {
                            buf = new char[offset + 128];
                            room = buf.Length - offset - 1;
                            java.lang.JSystem.arraycopy(lineBuffer, 0, buf, 0, offset);
                            java.util.Arrays.fill(lineBuffer, ' ');
                            lineBuffer = buf;
                        }
                        buf[offset++] = unchecked((char)(c));
                        break;
                    }
                }
            }
            brk_loop_6: ;
            if (offset == 0)
            {
                return null;
            }
            char[] ret = new char[offset];
            java.lang.JSystem.arraycopy(buf, 0, ret, 0, offset);
            java.util.Arrays.fill(buf, ' ');
            return ret;
        }
        public KeyManager() : base()
        {
        }
    }
}
