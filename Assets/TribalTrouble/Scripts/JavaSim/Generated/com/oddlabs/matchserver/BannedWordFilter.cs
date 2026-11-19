// Translated from BannedWordFilter.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchserver
{
    // Java: 14
    public class BannedWordFilter : java.lang.Object
    {
        public const string MATCH_SUBSTRING = "substring";
        public const string MATCH_EXACT = "exact";
        public const long CACHE_TTL_MILLIS = 86400000L;
        public const string CENSOR_REPLACEMENT = "****";
        // Java: 22
        public class BannedWord : java.lang.Object
        {
            public string lite() { return lite_f; }
            public string collapsed() { return collapsed_f; }
            public bool substring_match() { return substring_match_f; }
            public BannedWord(string lite, string collapsed, bool substring_match)
            {
                this.lite_f = lite;
                this.collapsed_f = collapsed;
                this.substring_match_f = substring_match;
            }
            public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is BannedWord r)) return false; return true && java.lang.JO.equals(lite_f, r.lite_f) && java.lang.JO.equals(collapsed_f, r.collapsed_f) && java.lang.JO.equals(substring_match_f, r.substring_match_f); }
            public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(lite_f); h = 31 * h + java.lang.JO.hashCode(collapsed_f); h = 31 * h + java.lang.JO.hashCode(substring_match_f); } return h; }
            public override string toString() { return "BannedWord[" + "lite=" + java.lang.JO.toString(lite_f) + ", collapsed=" + java.lang.JO.toString(collapsed_f) + ", substring_match=" + java.lang.JO.toString(substring_match_f) + "]"; }
            public string lite_f;
            public string collapsed_f;
            public bool substring_match_f;
        }
        public static java.util.List cached_words;
        public static long cache_loaded_time;
        // Java: 35
        public static void invalidateCache()
        {
            com.oddlabs.matchserver.BannedWordFilter.cached_words = null;
        }
        // Java: 39
        public static bool isAllowed(string name)
        {
            string lite = com.oddlabs.matchserver.BannedWordFilter.normalize(name, false);
            string collapsed = com.oddlabs.matchserver.BannedWordFilter.normalize(name, true);
            foreach (com.oddlabs.matchserver.BannedWordFilter.BannedWord word in java.lang.JIter.of(com.oddlabs.matchserver.BannedWordFilter.getBannedWords()))
            {
                if (com.oddlabs.matchserver.BannedWordFilter.matches(word, lite, collapsed))
                {
                    return false;
                }
            }
            return true;
        }
        // Java: 52
        public static string censorChatMessage(string msg)
        {
            if (msg == null || msg.isEmpty())
            {
                return msg;
            }
            string[] tokens = msg.split(" ", (-1));
            bool censored = false;
            for (int i = 0; i < tokens.Length; i++)
            {
                string lite = com.oddlabs.matchserver.BannedWordFilter.normalize(tokens[i], false);
                if (lite.isEmpty())
                {
                    continue;
                }
                string collapsed = com.oddlabs.matchserver.BannedWordFilter.normalize(tokens[i], true);
                foreach (com.oddlabs.matchserver.BannedWordFilter.BannedWord word in java.lang.JIter.of(com.oddlabs.matchserver.BannedWordFilter.getBannedWords()))
                {
                    if (com.oddlabs.matchserver.BannedWordFilter.matches(word, lite, collapsed))
                    {
                        tokens[i] = com.oddlabs.matchserver.BannedWordFilter.CENSOR_REPLACEMENT;
                        censored = true;
                        break;
                    }
                }
            }
            return (censored ? java.lang.JString.join(" ", tokens) : msg);
        }
        // Java: 73
        public static bool matches(com.oddlabs.matchserver.BannedWordFilter.BannedWord word, string lite, string collapsed)
        {
            if (word.substring_match())
            {
                return collapsed.contains(word.collapsed()) || lite.contains(word.lite());
            }
            return java.lang.JO.equals(lite, word.lite());
        }
        // Java: 79
        public static java.util.List getBannedWords()
        {
            long now = java.lang.JSystem.currentTimeMillis();
            java.util.List words = com.oddlabs.matchserver.BannedWordFilter.cached_words;
            if (words == null || now - com.oddlabs.matchserver.BannedWordFilter.cache_loaded_time > com.oddlabs.matchserver.BannedWordFilter.CACHE_TTL_MILLIS)
            {
                words = new java.util.ArrayList();
                foreach (string[] entry in java.lang.JIter.of(com.oddlabs.matchserver.DBInterface.getBannedWords()))
                {
                    words.add(new com.oddlabs.matchserver.BannedWordFilter.BannedWord(com.oddlabs.matchserver.BannedWordFilter.normalize(entry[0], false), com.oddlabs.matchserver.BannedWordFilter.normalize(entry[0], true), java.lang.JO.equals(com.oddlabs.matchserver.BannedWordFilter.MATCH_SUBSTRING, entry[1])));
                }
                com.oddlabs.matchserver.BannedWordFilter.cached_words = words;
                com.oddlabs.matchserver.BannedWordFilter.cache_loaded_time = now;
            }
            return words;
        }
        // Java: 101
        public static string normalize(string name, bool collapse_runs)
        {
            java.lang.StringBuilder sb = new java.lang.StringBuilder(name.length());
            string lower = name.toLowerCase();
            for (int i = 0; i < lower.length(); i++)
            {
                char c = ((lower.charAt(i)) switch { '0' => (char)('o'), '1' => (char)('i'), '2' => (char)('z'), '3' => (char)('e'), '4' => (char)('a'), '5' => (char)('s'), '6' or '9' => (char)('g'), '7' => (char)('t'), '8' => (char)('b'), _ => (char)(lower.charAt(i)) });
                if (c < 'a' || c > 'z')
                {
                    continue;
                }
                if (collapse_runs && sb.length() > 0 && sb.charAt(sb.length() - 1) == c)
                {
                    continue;
                }
                sb.append(c);
            }
            return java.lang.JO.toString(sb);
        }
        public BannedWordFilter() : base()
        {
        }
    }
}
