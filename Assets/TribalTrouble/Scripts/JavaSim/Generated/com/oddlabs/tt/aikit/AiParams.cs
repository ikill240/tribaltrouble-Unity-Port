// Translated from AiParams.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.aikit
{
    // Java: 20
    public class AiParams : java.lang.Object
    {
        public java.util.Map given;
        public java.util.Map effective;
        // Java: 28
        public static com.oddlabs.tt.aikit.AiParams parse(string text)
        {
            com.oddlabs.tt.aikit.AiParams @params = new com.oddlabs.tt.aikit.AiParams();
            if (text != null && !text.isBlank())
            {
                foreach (string pair in text.split(","))
                {
                    int eq = pair.indexOf('=');
                    if (eq <= 0 || ((string)@params.given.put(pair.substring(0, eq).trim(), pair.substring(eq + 1).trim())) != null)
                    {
                        throw new System.ArgumentException("bad AI param '" + java.lang.JStr.valueOf(pair) + "' in '" + java.lang.JStr.valueOf(text) + "'");
                    }
                }
            }
            return @params;
        }
        // Java: 41
        public int getInt(string key, int fallback)
        {
            string @value = get(key, java.lang.Integer.toString(fallback));
            try
            {
                return java.lang.Integer.parseInt(@value);
            }
            catch (System.FormatException e)
            {
                throw new System.ArgumentException("AI param " + java.lang.JStr.valueOf(key) + " must be a whole number, not '" + java.lang.JStr.valueOf(@value) + "'", (System.Exception)e);
            }
        }
        // Java: 50
        public double getDouble(string key, double fallback)
        {
            string @value = get(key, java.lang.Double.toString(fallback));
            try
            {
                return java.lang.Double.parseDouble(@value);
            }
            catch (System.FormatException e)
            {
                throw new System.ArgumentException("AI param " + java.lang.JStr.valueOf(key) + " must be a number, not '" + java.lang.JStr.valueOf(@value) + "'", (System.Exception)e);
            }
        }
        // Java: 60
        public bool getBoolean(string key, bool fallback)
        {
            string @value = get(key, java.lang.Boolean.toString(fallback));
            return ((@value) switch { "true" or "1" => (bool)(true), "false" or "0" => (bool)(false), _ => throw new System.ArgumentException("AI param " + java.lang.JStr.valueOf(key) + " must be true/false/1/0, not '" + java.lang.JStr.valueOf(@value) + "'") });
        }
        // Java: 70
        public string getString(string key, string fallback)
        {
            return get(key, fallback);
        }
        // Java: 75
        public string done()
        {
            java.util.List unknown = new java.util.ArrayList();
            foreach (string key in java.lang.JIter.of(given.keySet()))
            {
                if (!effective.containsKey(key))
                {
                    unknown.add(key);
                }
            }
            if (!unknown.isEmpty())
            {
                throw new System.ArgumentException("unknown AI params " + java.lang.JStr.valueOf(unknown) + "; known: " + java.lang.JStr.valueOf(effective.keySet()));
            }
            java.util.List pairs = new java.util.ArrayList();
            effective.forEach(new java.util.function.BiConsumer_L((object key_2_l, object @value_l) =>
            {
                string key_2 = (string)key_2_l;
                string @value = (string)@value_l;
                pairs.add(java.lang.JStr.valueOf(key_2) + "=" + java.lang.JStr.valueOf(@value));
            }));
            return java.lang.JString.join(" ", pairs);
        }
        // Java: 90
        public string get(string key, string fallback)
        {
            string @value = ((string)given.getOrDefault(key, fallback));
            effective.put(key, @value);
            return @value;
        }
        private void init_AiParams()
        {
            this.given = new java.util.LinkedHashMap();
            this.effective = new java.util.TreeMap();
        }
        public AiParams() : base()
        {
            init_AiParams();
        }
    }
}
