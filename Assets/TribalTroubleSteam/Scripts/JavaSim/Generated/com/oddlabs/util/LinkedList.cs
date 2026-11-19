// Translated from LinkedList.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.util
{
    // Java: 6
    public class LinkedList : java.lang.Object
    {
        public com.oddlabs.util.ListElement first;
        public com.oddlabs.util.ListElement last;
        public int size_f;
        // Java: 14
        public bool checkOwner(com.oddlabs.util.ListElement elem)
        {
            if (((object)elem.getListOwner() == (object)this))
            {
                return true;
            }
            if (elem.getListOwner() != null)
            {
                elem.getListOwner().remove(elem);
            }
            elem.setListOwner(this);
            return false;
        }
        // Java: 23
        public void addLast(com.oddlabs.util.ListElement elem)
        {
            if (checkOwner(elem))
            {
                return;
            }
            if (last == null)
            {
                first = elem;
                last = elem;
                elem.setNext(null);
                elem.setPrior(null);
            }
            else
            {
                elem.setNext(null);
                elem.setPrior(last);
                last.setNext(elem);
                last = elem;
            }
            size_f++;
        }
        // Java: 39
        public void addFirst(com.oddlabs.util.ListElement elem)
        {
            if (checkOwner(elem))
            {
                return;
            }
            if (last == null)
            {
                first = elem;
                last = elem;
                elem.setNext(null);
                elem.setPrior(null);
            }
            else
            {
                elem.setNext(first);
                elem.setPrior(null);
                first.setPrior(elem);
                first = elem;
            }
            size_f++;
        }
        // Java: 55
        public void remove(com.oddlabs.util.ListElement element)
        {
            element.setListOwner(null);
            if (((object)last == (object)element) && ((object)first == (object)element))
            {
                first = null;
                last = null;
            }
            else if (((object)last == (object)element))
            {
                last = element.getPrior();
                last.setNext(null);
            }
            else if (((object)first == (object)element))
            {
                first = element.getNext();
                first.setPrior(null);
            }
            else
            {
                element.getPrior().setNext(element.getNext());
                element.getNext().setPrior(element.getPrior());
            }
            size_f--;
        }
        // Java: 74
        public void insert(com.oddlabs.util.ListElement element, com.oddlabs.util.ListElement next_elem)
        {
            if (next_elem == null)
            {
                addLast(element);
                return;
            }
            checkOwner(element);
            if (((object)first == (object)next_elem))
            {
                first = element;
                element.setPrior(null);
            }
            else
            {
                com.oddlabs.util.ListElement prev = next_elem.getPrior();
                element.setPrior(prev);
                prev.setNext(element);
            }
            next_elem.setPrior(element);
            element.setNext(next_elem);
            size_f++;
        }
        // Java: 94
        public int size()
        {
            return size_f;
        }
        // Java: 98
        public bool isEmpty()
        {
            return size_f == 0;
        }
        // Java: 102
        public com.oddlabs.util.ListElement getFirst()
        {
            return first;
        }
        // Java: 106
        public com.oddlabs.util.ListElement getLast()
        {
            return last;
        }
        // Java: 110
        public void putLast(com.oddlabs.util.ListElement element)
        {
            remove(element);
            addLast(element);
        }
        // Java: 115
        public void putFirst(com.oddlabs.util.ListElement element)
        {
            remove(element);
            addFirst(element);
        }
        private void init_LinkedList()
        {
            this.size_f = 0;
        }
        public LinkedList() : base()
        {
            init_LinkedList();
        }
    }
}
