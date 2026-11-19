using System.Collections.Generic;
using UnityEngine;

namespace TribalTrouble.FaithfulGame
{
    /// <summary>
    /// The Java game's ResourceBundle texts (all .properties files, every language), from Resources/TTStrings.txt
    /// (bundle TAB locale TAB key TAB value). Feeds java.util.ResourceBundle in the translated code, with Java's
    /// fallback: language-specific bundle first, then the base bundle.
    /// </summary>
    public static class TTStrings
    {
        static Dictionary<string, string> s_map;
        public static string Language = "";   // "", "de", "es", "it", "da", "pt"

        public static void Install()
        {
            if (s_map == null)
            {
                s_map = new Dictionary<string, string>(8192);
                var asset = Resources.Load<TextAsset>("TTStrings");
                if (asset != null)
                    foreach (var line in asset.text.Split('\n'))
                    {
                        var p = line.Split('\t');
                        if (p.Length < 4) continue;
                        s_map[p[0] + "|" + p[1] + "|" + p[2]] = Unescape(p[3]);
                    }
                else Debug.LogWarning("[Tribal Trouble] Resources/TTStrings.txt missing: in-game texts will show their keys.");
                Language = Application.systemLanguage switch
                {
                    SystemLanguage.German => "de", SystemLanguage.Spanish => "es", SystemLanguage.Italian => "it",
                    SystemLanguage.Danish => "da", SystemLanguage.Portuguese => "pt", _ => "",
                };
            }
            java.util.ResourceBundle.Lookup = Get;
        }

        public static string Get(string bundle, string key)
        {
            if (s_map == null) return key;
            if (Language.Length > 0 && s_map.TryGetValue(bundle + "|" + Language + "|" + key, out var v)) return v;
            return s_map.TryGetValue(bundle + "||" + key, out v) ? v : key;
        }

        static string Unescape(string s)
        {
            if (s.IndexOf('\\') < 0) return s.TrimEnd('\r');
            var sb = new System.Text.StringBuilder(s.Length);
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if (c == '\\' && i + 1 < s.Length) { c = s[++i]; c = c == 'n' ? '\n' : c == 't' ? '\t' : c; }
                if (c != '\r') sb.Append(c);
            }
            return sb.ToString();
        }
    }
}
