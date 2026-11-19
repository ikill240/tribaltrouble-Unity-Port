// Translated from HashEntry.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.util
{
    // Java: 5
    public class HashEntry : com.oddlabs.util.ListElementImpl
    {
        public object hash_entry;
        public int key;
        // Java: 14
        public override com.oddlabs.util.ListElementImpl self()
        {
            return this;
        }
        // Java: 19
        public object getEntry()
        {
            return hash_entry;
        }
        // Java: 23
        public object setEntry(object entry)
        {
            object old = hash_entry;
            hash_entry = entry;
            return old;
        }
        // Java: 29
        public int getKey()
        {
            return key;
        }
        public HashEntry(int key, object entry) : base()
        {
            this.key = key;
            this.hash_entry = entry;
        }
    }
}
