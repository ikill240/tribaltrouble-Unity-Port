// Translated from Strategy.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.ultra
{
    // Java: 15
    public abstract class Strategy : java.lang.Object
    {
        public com.oddlabs.tt.player.ultra.Params @params;
        public java.util.Map defaults;
        public java.util.Map multi_defaults;
        public bool multi;
        // Java: 27
        public abstract string name();
        // Java: 30
        public abstract int[] openingOrder(bool threatened);
        // Java: 32
        public abstract void defineDefaults();
        // Java: 34
        public void def(string key, float @value)
        {
            defaults.put(key, @value);
        }
        // Java: 41
        public void defMulti(string key, float @value)
        {
            multi_defaults.put(key, @value);
        }
        // Java: 46
        public void setEnemyCount(int enemies)
        {
            multi = enemies >= i("multiN");
        }
        // Java: 50
        public bool multiEnemy()
        {
            return multi;
        }
        // Java: 54
        public float f(string key)
        {
            float? d = ((float?)defaults.get(key));
            if (d == null)
            {
                throw new System.ArgumentException("Unknown strategy parameter " + java.lang.JStr.valueOf(key));
            }
            if (multi)
            {
                float? m = ((float?)multi_defaults.get(key));
                if (m != null)
                {
                    d = m;
                }
            }
            return @params.get(key, ((float)d));
        }
        // Java: 66
        public int i(string key)
        {
            return java.lang.Math.round(f(key));
        }
        // Java: 70
        public bool b(string key)
        {
            return f(key) != 0.0f;
        }
        // Java: 74
        public static com.oddlabs.tt.player.ultra.Strategy forMap(com.oddlabs.tt.player.ultra.Params @params, int map_size)
        {
            return new com.oddlabs.tt.player.ultra.LargeIslandStrategy(@params);
        }
        public const int Q = 0;
        public const int A = 1;
        public const int T = 2;
        private void init_Strategy()
        {
            this.defaults = new java.util.LinkedHashMap();
            this.multi_defaults = new java.util.LinkedHashMap();
        }
        public Strategy(com.oddlabs.tt.player.ultra.Params @params) : base()
        {
            init_Strategy();
            this.@params = @params;
            defineDefaults();
        }
    }
}
