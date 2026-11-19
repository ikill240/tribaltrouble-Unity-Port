// Translated from ListElementImpl.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.util
{
    // Java: 6
    public abstract class ListElementImpl : java.lang.Object, com.oddlabs.util.ListElement
    {
        public com.oddlabs.util.LinkedList parent;
        public com.oddlabs.util.ListElementImpl next;
        public com.oddlabs.util.ListElementImpl prior;
        // Java: 12
        public abstract com.oddlabs.util.ListElementImpl self();
        // Java: 14
        public void setListOwner(com.oddlabs.util.LinkedList owner)
        {
            parent = owner;
        }
        // Java: 19
        public com.oddlabs.util.LinkedList getListOwner()
        {
            return parent;
        }
        // Java: 24
        public void setPrior(com.oddlabs.util.ListElement prior_p)
        {
            com.oddlabs.util.ListElementImpl prior = (com.oddlabs.util.ListElementImpl)prior_p;
            this.prior = prior;
        }
        // Java: 29
        public void setNext(com.oddlabs.util.ListElement next_p)
        {
            com.oddlabs.util.ListElementImpl next = (com.oddlabs.util.ListElementImpl)next_p;
            this.next = next;
        }
        // Java: 34
        public com.oddlabs.util.ListElement getPrior()
        {
            return prior;
        }
        // Java: 39
        public com.oddlabs.util.ListElement getNext()
        {
            return next;
        }
        public ListElementImpl() : base()
        {
        }
    }
}
