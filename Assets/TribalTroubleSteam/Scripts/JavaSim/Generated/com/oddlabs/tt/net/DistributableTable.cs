// Translated from DistributableTable.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.net
{
    // Java: 10
    public class DistributableTable : java.lang.Object
    {
        public com.oddlabs.util.HashTable distributables;
        public java.util.Map names;
        public int current_name;
        // Java: 15
        public int register(com.oddlabs.tt.net.Distributable distributable)
        {
            int name = current_name++;
            com.oddlabs.tt.net.Distributable o = ((com.oddlabs.tt.net.Distributable)distributables.put(name, distributable));
            int? p = ((int?)names.put(distributable, name));
            return name;
        }
        // Java: 24
        public void unregister(com.oddlabs.tt.net.Distributable distributable)
        {
            int? name = ((int?)names.remove(distributable));
            com.oddlabs.tt.net.Distributable o = ((com.oddlabs.tt.net.Distributable)distributables.remove(((int)name)));
        }
        // Java: 32
        public int getName(com.oddlabs.tt.net.Distributable distributable)
        {
            int? val = ((int?)names.get(distributable));
            return ((int)val);
        }
        // Java: 38
        public com.oddlabs.tt.net.Distributable getDistributable(int name)
        {
            return ((com.oddlabs.tt.net.Distributable)distributables.get(name));
        }
        private void init_DistributableTable()
        {
            this.distributables = new com.oddlabs.util.HashTable();
            this.names = new java.util.HashMap();
            this.current_name = 1;
        }
        public DistributableTable() : base()
        {
            init_DistributableTable();
        }
    }
}
