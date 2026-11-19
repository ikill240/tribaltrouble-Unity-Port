// Translated from WordsEncoding.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.util
{
    // Java: 12
    public class WordsEncoding : java.lang.Object
    {
        public static java.util.List WORDS;
        public static java.util.Map WORD_TO_INDEX;
        public const int BITS_PER_WORD = 10;
        public static java.math.BigInteger MASK;
        // Java: 34
        public static java.math.BigInteger decode(string code)
        {
            java.math.BigInteger result = java.math.BigInteger.ZERO;
            string[] words = code.split(" ");
            foreach (string word in words)
            {
                string upper = word.toUpperCase();
                if (upper.isEmpty())
                {
                    continue;
                }
                int? index = ((int?)com.oddlabs.tt.util.WordsEncoding.WORD_TO_INDEX.get(upper));
                if (index == null)
                {
                    throw new System.ArgumentException("Invalid word: " + java.lang.JStr.valueOf(word));
                }
                result = result.shiftLeft(com.oddlabs.tt.util.WordsEncoding.BITS_PER_WORD);
                result = result.or(java.math.BigInteger.valueOf(((int)index)));
            }
            return result;
        }
        // Java: 50
        public static string encode(java.math.BigInteger code)
        {
            java.lang.StringBuilder result = new java.lang.StringBuilder();
            while (!java.lang.JO.equals(code, java.math.BigInteger.ZERO))
            {
                int index = code.and(com.oddlabs.tt.util.WordsEncoding.MASK).intValue();
                code = code.shiftRight(com.oddlabs.tt.util.WordsEncoding.BITS_PER_WORD);
                if (java.lang.JO.equals(code, java.math.BigInteger.ZERO) && index == 0)
                {
                    break;
                }
                if (!result.isEmpty())
                {
                    result.insert(0, ' ');
                }
                result.insert(0, ((string)com.oddlabs.tt.util.WordsEncoding.WORDS.get(index)));
            }
            return java.lang.JO.toString(result);
        }
        static WordsEncoding()
        {
            WordsEncoding.WORDS = new java.util.ArrayList();
            WordsEncoding.WORD_TO_INDEX = new java.util.HashMap();
            WordsEncoding.MASK = java.math.BigInteger.valueOf(1023);
            {
                {
                    java.io.InputStream stream = typeof(com.oddlabs.tt.util.WordsEncoding).getResourceAsStream("dictionary");
                    java.io.BufferedReader reader = new java.io.BufferedReader(new java.io.InputStreamReader(stream, java.nio.charset.StandardCharsets.UTF_8));
                    try
                    {
                        string line = default;
                        int i = 0;
                        while ((line = reader.readLine()) != null)
                        {
                            string word = line.toUpperCase();
                            com.oddlabs.tt.util.WordsEncoding.WORDS.add(word);
                            com.oddlabs.tt.util.WordsEncoding.WORD_TO_INDEX.put(word, i);
                            i++;
                        }
                    }
                    catch (System.Exception e)
                    {
                        java.lang.JSystem.@out.println("Exception during initializing WordsEncoding: " + java.lang.JStr.valueOf(e));
                    }
                    finally
                    {
                        if (reader != null) reader.close();
                        if (stream != null) stream.close();
                    }
                }
            }
        }
        public WordsEncoding() : base()
        {
        }
    }
}
