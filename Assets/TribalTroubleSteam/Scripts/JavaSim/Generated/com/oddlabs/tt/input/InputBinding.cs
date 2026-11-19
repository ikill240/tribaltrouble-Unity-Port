// Translated from InputBinding.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.input
{
    // Java: 12
    public class InputBinding : java.lang.Object, java.lang.Comparable
    {
        public com.oddlabs.tt.input.Key key() { return key_f; }
        public java.util.Set modifiers() { return modifiers_f; }
        public com.oddlabs.tt.input.GameAction action() { return action_f; }
        public InputBinding(com.oddlabs.tt.input.Key key_v, java.util.Set modifiers_v, com.oddlabs.tt.input.GameAction action_v)
        {
            java.util.Objects.requireNonNull(key_v, "key");
            java.util.Objects.requireNonNull(modifiers_v, "modifiers");
            java.util.Objects.requireNonNull(action_v, "action");
            modifiers_v = java.util.EnumSet.copyOf(modifiers_v);
            this.key_f = key_v;
            this.modifiers_f = modifiers_v;
            this.action_f = action_v;
        }
        public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is InputBinding r)) return false; return true && java.lang.JO.equals(key_f, r.key_f) && java.lang.JO.equals(modifiers_f, r.modifiers_f) && java.lang.JO.equals(action_f, r.action_f); }
        public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(key_f); h = 31 * h + java.lang.JO.hashCode(modifiers_f); h = 31 * h + java.lang.JO.hashCode(action_f); } return h; }
        public com.oddlabs.tt.input.Key key_f;
        public java.util.Set modifiers_f;
        public com.oddlabs.tt.input.GameAction action_f;
        public static bool IS_MACOS;
        // Java: 25
        public bool matches(com.oddlabs.tt.input.KeyboardEvent @event)
        {
            return ((object)@event.keyCode() == (object)key_f) && java.lang.JO.equals(modifiers_f, @event.modifiers());
        }
        // Java: 29
        public bool shift()
        {
            return modifiers_f.contains(com.oddlabs.tt.input.Modifier.SHIFT);
        }
        // Java: 33
        public bool control()
        {
            return modifiers_f.contains(com.oddlabs.tt.input.Modifier.CONTROL);
        }
        // Java: 37
        public bool alt()
        {
            return modifiers_f.contains(com.oddlabs.tt.input.Modifier.ALT);
        }
        // Java: 41
        public bool meta()
        {
            return modifiers_f.contains(com.oddlabs.tt.input.Modifier.META);
        }
        // Java: 45
        public override string toString()
        {
            string s = "";
            if (com.oddlabs.tt.input.InputBinding.IS_MACOS)
            {
                if (control())
                {
                    s = java.lang.JStr.valueOf(s) + "\u2303";
                }
                if (alt())
                {
                    s = java.lang.JStr.valueOf(s) + "\u2325";
                }
                if (shift())
                {
                    s = java.lang.JStr.valueOf(s) + "\u21e7";
                }
                if (meta())
                {
                    s = java.lang.JStr.valueOf(s) + "\u2318";
                }
            }
            else
            {
                if (control())
                {
                    s = java.lang.JStr.valueOf(s) + "Ctrl+";
                }
                if (alt())
                {
                    s = java.lang.JStr.valueOf(s) + "Alt+";
                }
                if (shift())
                {
                    s = java.lang.JStr.valueOf(s) + "Shift+";
                }
                if (meta())
                {
                    s = java.lang.JStr.valueOf(s) + "Meta+";
                }
            }
            return java.lang.JStr.valueOf(s) + java.lang.JStr.valueOf(key().getDisplayName());
        }
        // Java: 62
        public int compareTo(object o_p)
        {
            com.oddlabs.tt.input.InputBinding o = (com.oddlabs.tt.input.InputBinding)o_p;
            if (this.control() != o.control())
            {
                return (this.control() ? 1 : (-1));
            }
            if (this.alt() != o.alt())
            {
                return (this.alt() ? 1 : (-1));
            }
            if (this.shift() != o.shift())
            {
                return (this.shift() ? 1 : (-1));
            }
            if (this.meta() != o.meta())
            {
                return (this.meta() ? 1 : (-1));
            }
            int keyCompare = this.key().getDisplayName().compareTo(o.key().getDisplayName());
            if (keyCompare != 0)
            {
                return keyCompare;
            }
            return this.action().compareTo(o.action());
        }
        static InputBinding()
        {
            InputBinding.IS_MACOS = java.lang.JSystem.getProperty("os.name", "").toLowerCase().contains("mac");
        }
    }
}
