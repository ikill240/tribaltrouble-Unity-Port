// Translated from GameModeOption.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchmaking
{
    // Java: 14
    public class GameModeOption : java.lang.Object
    {
        public const long serialVersionUID = 1L;
        // Java: 18
        public sealed class Type : java.lang.Enum
        {
            public static Type[] values() { return (Type[])VALUES_.Clone(); }
            public static Type valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly Type BOOL = new Type("BOOL", 0);
            public static readonly Type INT = new Type("INT", 1);
            public static readonly Type FLOAT = new Type("FLOAT", 2);
            public static readonly Type ENUM = new Type("ENUM", 3);
            public static readonly Type STRING = new Type("STRING", 4);
            public static readonly Type[] VALUES_ = new Type[] { BOOL, INT, FLOAT, ENUM, STRING };
            public Type(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        public string key;
        public com.oddlabs.matchmaking.GameModeOption.Type type;
        public object default_value;
        public string i18n_key;
        public object min;
        public object max;
        public string[] enum_values;
        // Java: 51
        public string getKey()
        {
            return key;
        }
        // Java: 55
        public com.oddlabs.matchmaking.GameModeOption.Type getType()
        {
            return type;
        }
        // Java: 59
        public object getDefaultValue()
        {
            return default_value;
        }
        // Java: 63
        public string getI18nKey()
        {
            return i18n_key;
        }
        // Java: 67
        public object getMin()
        {
            return min;
        }
        // Java: 71
        public object getMax()
        {
            return max;
        }
        // Java: 75
        public string[] getEnumValues()
        {
            return enum_values;
        }
        public GameModeOption(string key, com.oddlabs.matchmaking.GameModeOption.Type type, object default_value, string i18n_key) : this(key, type, default_value, i18n_key, null, null, null)
        {
        }
        public GameModeOption(string key, com.oddlabs.matchmaking.GameModeOption.Type type, object default_value, string i18n_key, object min, object max, string[] enum_values) : base()
        {
            this.key = key;
            this.type = type;
            this.default_value = default_value;
            this.i18n_key = i18n_key;
            this.min = min;
            this.max = max;
            this.enum_values = enum_values;
        }
    }
}
