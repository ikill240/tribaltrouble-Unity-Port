// Translated from ListElement.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.util
{
    // Java: 5
    public interface ListElement
    {
        // Java: 6
        com.oddlabs.util.ListElement getPrior();
        // Java: 9
        void setPrior(com.oddlabs.util.ListElement prior);
        // Java: 11
        com.oddlabs.util.ListElement getNext();
        // Java: 14
        void setNext(com.oddlabs.util.ListElement next);
        // Java: 19
        com.oddlabs.util.LinkedList getListOwner();
        // Java: 22
        void setListOwner(com.oddlabs.util.LinkedList list);
    }
}
