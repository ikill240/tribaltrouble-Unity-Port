// Translated from HashTable.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.util
{
    // Java: 15
    public class HashTable : java.lang.Object
    {
        public const long serialVersionUID = 1L;
        public const int DEFAULT_INITIAL_ENTRIES = 10;
        public const int DEFAULT_MUL_FACTOR = 2;
        public const float DEFAULT_LOAD_FACTOR = 0.75f;
        public com.oddlabs.util.LinkedList[] entries;
        public float load_factor;
        public int mul_factor;
        public int num_entries;
        // Java: 32
        public int size()
        {
            return num_entries;
        }
        // Java: 36
        public int hash(int key)
        {
            int hash_v = key % entries.Length;
            return (hash_v >= 0 ? hash_v : hash_v + entries.Length);
        }
        // Java: 41
        public object put(int key, object val)
        {
            int hash_v = hash(key);
            if (entries[hash_v] == null)
            {
                entries[hash_v] = new com.oddlabs.util.LinkedList();
            }
            else
            {
                com.oddlabs.util.HashEntry current_entry = ((com.oddlabs.util.HashEntry)entries[hash_v].getFirst());
                while (current_entry != null)
                {
                    int current_key = current_entry.getKey();
                    if (current_key == key)
                    {
                        return current_entry.setEntry(val);
                    }
                    current_entry = ((com.oddlabs.util.HashEntry)current_entry.getNext());
                }
            }
            com.oddlabs.util.HashEntry hash_entry = new com.oddlabs.util.HashEntry(key, val);
            entries[hash_v].addLast(hash_entry);
            num_entries++;
            if (num_entries > (float)(load_factor * entries.Length))
            {
                rehash();
            }
            return null;
        }
        // Java: 63
        public object get(int key)
        {
            int hash_v = hash(key);
            if (entries[hash_v] == null)
            {
                return null;
            }
            com.oddlabs.util.HashEntry current_entry = ((com.oddlabs.util.HashEntry)entries[hash_v].getFirst());
            while (current_entry != null)
            {
                int current_key = current_entry.getKey();
                if (current_key == key)
                {
                    return current_entry.getEntry();
                }
                current_entry = ((com.oddlabs.util.HashEntry)current_entry.getNext());
            }
            return null;
        }
        // Java: 77
        public object remove(int key)
        {
            int hash_v = hash(key);
            if (entries[hash_v] == null)
            {
                return null;
            }
            com.oddlabs.util.HashEntry current_entry = ((com.oddlabs.util.HashEntry)entries[hash_v].getFirst());
            while (current_entry != null)
            {
                int current_key = current_entry.getKey();
                if (current_key == key)
                {
                    object result = current_entry.getEntry();
                    entries[hash_v].remove(current_entry);
                    num_entries--;
                    return result;
                }
                current_entry = ((com.oddlabs.util.HashEntry)current_entry.getNext());
            }
            return null;
        }
        // Java: 96
        public void rehash()
        {
            com.oddlabs.util.LinkedList[] old_entries = entries;
            entries = ((com.oddlabs.util.LinkedList[])new com.oddlabs.util.LinkedList[entries.Length * mul_factor]);
            foreach (com.oddlabs.util.LinkedList old_entry in old_entries)
            {
                if (old_entry != null)
                {
                    com.oddlabs.util.HashEntry current_entry = ((com.oddlabs.util.HashEntry)old_entry.getFirst());
                    while (current_entry != null)
                    {
                        int hash_v = hash(current_entry.getKey());
                        com.oddlabs.util.HashEntry next_entry = ((com.oddlabs.util.HashEntry)current_entry.getNext());
                        if (entries[hash_v] == null)
                        {
                            entries[hash_v] = new com.oddlabs.util.LinkedList();
                        }
                        entries[hash_v].addLast(current_entry);
                        current_entry = next_entry;
                    }
                }
            }
        }
        // Java: 115
        public void writeObject(java.io.ObjectOutputStream s)
        {
            s.defaultWriteObject();
            s.writeInt(entries.Length);
            s.writeInt(num_entries);
            foreach (com.oddlabs.util.LinkedList list in entries)
            {
                if (list != null)
                {
                    com.oddlabs.util.HashEntry entry = ((com.oddlabs.util.HashEntry)list.getFirst());
                    while (entry != null)
                    {
                        s.writeInt(entry.getKey());
                        s.writeObject(entry.getEntry());
                        entry = ((com.oddlabs.util.HashEntry)entry.getNext());
                    }
                }
            }
        }
        // Java: 132
        public void readObject(java.io.ObjectInputStream s)
        {
            s.defaultReadObject();
            int length = s.readInt();
            entries = new com.oddlabs.util.LinkedList[length];
            int size_v = s.readInt();
            for (int i = 0; i < size_v; i++)
            {
                int key = s.readInt();
                object val = ((object)s.readObject());
                put(key, val);
            }
        }
        private void init_HashTable()
        {
            this.entries = new com.oddlabs.util.LinkedList[com.oddlabs.util.HashTable.DEFAULT_INITIAL_ENTRIES];
            this.load_factor = com.oddlabs.util.HashTable.DEFAULT_LOAD_FACTOR;
            this.mul_factor = com.oddlabs.util.HashTable.DEFAULT_MUL_FACTOR;
        }
        public HashTable() : base()
        {
            init_HashTable();
        }
    }
}
