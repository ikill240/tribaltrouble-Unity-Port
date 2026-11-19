// Translated from Languages.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 5
    public class Languages : java.lang.Object
    {
        public static string[][] languages;
        // Java: 8
        public static bool hasLanguage(string language)
        {
            foreach (string[] aLanguage in com.oddlabs.tt.gui.Languages.languages)
            {
                if (java.lang.JO.equals(aLanguage[0], language))
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 17
        public static string[][] getLanguages()
        {
            return com.oddlabs.tt.gui.Languages.languages;
        }
        // Java: 21
        public static com.oddlabs.tt.gui.IconQuad[] getFlags()
        {
            com.oddlabs.tt.gui.IconQuad[] flags = new com.oddlabs.tt.gui.IconQuad[] { com.oddlabs.tt.gui.Skin.getSkin().getFlagEn(), com.oddlabs.tt.gui.Skin.getSkin().getFlagDa(), com.oddlabs.tt.gui.Skin.getSkin().getFlagDe(), com.oddlabs.tt.gui.Skin.getSkin().getFlagEs(), com.oddlabs.tt.gui.Skin.getSkin().getFlagIt(), com.oddlabs.tt.gui.Skin.getSkin().getFlagPt(), com.oddlabs.tt.gui.Skin.getSkin().getFlagPl() };
            return flags;
        }
        static Languages()
        {
            Languages.languages = new string[][] { new string[] { "en", "English" }, new string[] { "da", "Dansk" }, new string[] { "de", "Deutsch" }, new string[] { "es", "Espa\u00f1ol" }, new string[] { "it", "Italiano" }, new string[] { "pt", "Portugu\u00eas brasileiro" }, new string[] { "pl", "Polski" } };
        }
        public Languages() : base()
        {
        }
    }
}
