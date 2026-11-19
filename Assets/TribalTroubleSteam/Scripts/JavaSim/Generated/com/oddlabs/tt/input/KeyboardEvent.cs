// Translated from KeyboardEvent.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.input
{
    // Java: 9
    public class KeyboardEvent : java.lang.Object
    {
        public com.oddlabs.tt.input.Key keyCode() { return keyCode_f; }
        public char keyChar() { return keyChar_f; }
        public java.util.Set modifiers() { return modifiers_f; }
        public int clicks() { return clicks_f; }
        public KeyboardEvent(com.oddlabs.tt.input.Key keyCode_v, char keyChar_v, java.util.Set modifiers_v, int clicks_v)
        {
            java.util.Objects.requireNonNull(keyCode_v, "keyCode");
            java.util.Objects.requireNonNull(modifiers_v, "modifiers");
            modifiers_v = java.util.EnumSet.copyOf(modifiers_v);
            this.keyCode_f = keyCode_v;
            this.keyChar_f = keyChar_v;
            this.modifiers_f = modifiers_v;
            this.clicks_f = clicks_v;
        }
        public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is KeyboardEvent r)) return false; return true && java.lang.JO.equals(keyCode_f, r.keyCode_f) && java.lang.JO.equals(keyChar_f, r.keyChar_f) && java.lang.JO.equals(modifiers_f, r.modifiers_f) && java.lang.JO.equals(clicks_f, r.clicks_f); }
        public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(keyCode_f); h = 31 * h + java.lang.JO.hashCode(keyChar_f); h = 31 * h + java.lang.JO.hashCode(modifiers_f); h = 31 * h + java.lang.JO.hashCode(clicks_f); } return h; }
        public override string toString() { return "KeyboardEvent[" + "keyCode=" + java.lang.JO.toString(keyCode_f) + ", keyChar=" + java.lang.JO.toString(keyChar_f) + ", modifiers=" + java.lang.JO.toString(modifiers_f) + ", clicks=" + java.lang.JO.toString(clicks_f) + "]"; }
        public com.oddlabs.tt.input.Key keyCode_f;
        public char keyChar_f;
        public java.util.Set modifiers_f;
        public int clicks_f;
        // Java: 22
        public static java.util.Set modifierSet(bool shiftDown_v, bool controlDown_v, bool altDown_v, bool metaDown_v)
        {
            java.util.Set set = java.util.EnumSet.noneOf(typeof(com.oddlabs.tt.input.Modifier));
            if (shiftDown_v)
            {
                set.add(com.oddlabs.tt.input.Modifier.SHIFT);
            }
            if (controlDown_v)
            {
                set.add(com.oddlabs.tt.input.Modifier.CONTROL);
            }
            if (altDown_v)
            {
                set.add(com.oddlabs.tt.input.Modifier.ALT);
            }
            if (metaDown_v)
            {
                set.add(com.oddlabs.tt.input.Modifier.META);
            }
            return set;
        }
        // Java: 32
        public bool shiftDown()
        {
            return modifiers_f.contains(com.oddlabs.tt.input.Modifier.SHIFT);
        }
        // Java: 36
        public bool controlDown()
        {
            return modifiers_f.contains(com.oddlabs.tt.input.Modifier.CONTROL);
        }
        // Java: 40
        public bool altDown()
        {
            return modifiers_f.contains(com.oddlabs.tt.input.Modifier.ALT);
        }
        // Java: 44
        public bool metaDown()
        {
            return modifiers_f.contains(com.oddlabs.tt.input.Modifier.META);
        }
        public KeyboardEvent(com.oddlabs.tt.input.Key keyCode_v, char keyChar_v, bool shiftDown_v, bool controlDown_v, bool altDown_v, bool metaDown_v, int clicks_v) : this(keyCode_v, keyChar_v, com.oddlabs.tt.input.KeyboardEvent.modifierSet(shiftDown_v, controlDown_v, altDown_v, metaDown_v), clicks_v)
        {
        }
    }
}
