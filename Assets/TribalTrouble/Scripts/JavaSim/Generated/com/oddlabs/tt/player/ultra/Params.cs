// Translated from Params.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.ultra
{
    // Java: 13
    public class Params : java.lang.Object
    {
        public java.util.Map values;
        // Java: 33
        public float get(string key, float def)
        {
            float? v = ((float?)values.get(key));
            return (v != null ? ((float)v) : def);
        }
        // Java: 38
        public int getInt(string key, int def)
        {
            float? v = ((float?)values.get(key));
            return (v != null ? java.lang.Math.round(((float)v)) : def);
        }
        // Java: 43
        public bool getBool(string key, bool def)
        {
            float? v = ((float?)values.get(key));
            return (v != null ? ((float)v) != 0.0f : def);
        }
        // Java: 48
        public override string toString()
        {
            return java.lang.JO.toString(values);
        }
        private void init_Params()
        {
            this.values = new java.util.LinkedHashMap();
        }
        public Params(string spec) : base()
        {
            init_Params();
            if (spec == null || spec.isBlank())
            {
                return;
            }
            foreach (string part in spec.split(","))
            {
                int eq = part.indexOf('=');
                if (eq <= 0)
                {
                    continue;
                }
                string key = part.substring(0, eq).trim();
                string @value = part.substring(eq + 1).trim();
                try
                {
                    values.put(key, java.lang.Float.parseFloat(@value));
                }
                catch (System.FormatException e)
                {
                }
            }
        }
    }
}
