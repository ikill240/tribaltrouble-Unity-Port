// Translated from Element.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 9
    public abstract class Element : com.oddlabs.tt.util.BoundingBox, com.oddlabs.util.ListElement
    {
        public com.oddlabs.tt.model.AbstractElementNode element_root;
        public com.oddlabs.tt.model.AbstractElementNode node_parent;
        public com.oddlabs.util.LinkedList parent;
        public com.oddlabs.tt.model.Element next;
        public com.oddlabs.tt.model.Element prior;
        public float render_pos_z;
        public float x;
        public float y;
        public float dir_x;
        public float dir_y;
        public com.oddlabs.tt.model.Element reference;
        // Java: 30
        public abstract com.oddlabs.tt.model.Element self();
        // Java: 32
        public abstract void visit(com.oddlabs.tt.model.ElementVisitor visitor);
        // Java: 34
        public virtual void register()
        {
            node_parent = element_root.insertElement(self());
        }
        // Java: 39
        public void reregister()
        {
            node_parent = element_root.reinsertElement(self());
        }
        // Java: 44
        public bool isRegistered()
        {
            return node_parent != null;
        }
        // Java: 48
        public virtual void remove()
        {
            node_parent.removeElement(self());
            node_parent = null;
        }
        // Java: 54
        public com.oddlabs.tt.model.Element getReference()
        {
            return reference;
        }
        // Java: 58
        public void setReference(com.oddlabs.tt.model.Element @ref)
        {
            reference = @ref;
        }
        // Java: 62
        public float getDirectionX()
        {
            return dir_x;
        }
        // Java: 66
        public float getDirectionY()
        {
            return dir_y;
        }
        // Java: 70
        public void setDirection(float dir_x, float dir_y)
        {
            this.dir_x = dir_x;
            this.dir_y = dir_y;
        }
        // Java: 75
        public virtual void setPosition(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        // Java: 81
        public virtual void setPositionZ(float z)
        {
            render_pos_z = z;
        }
        // Java: 85
        public float getPositionX()
        {
            return x;
        }
        // Java: 89
        public float getPositionY()
        {
            return y;
        }
        // Java: 93
        public float getPositionZ()
        {
            return render_pos_z;
        }
        // Java: 97
        public void setListOwner(com.oddlabs.util.LinkedList parent)
        {
            this.parent = parent;
        }
        // Java: 102
        public com.oddlabs.util.LinkedList getListOwner()
        {
            return parent;
        }
        // Java: 107
        public void setPrior(com.oddlabs.util.ListElement prior_p)
        {
            com.oddlabs.tt.model.Element prior = (com.oddlabs.tt.model.Element)prior_p;
            this.prior = prior;
        }
        // Java: 112
        public void setNext(com.oddlabs.util.ListElement next_p)
        {
            com.oddlabs.tt.model.Element next = (com.oddlabs.tt.model.Element)next_p;
            this.next = next;
        }
        // Java: 117
        public com.oddlabs.util.ListElement getPrior()
        {
            return prior;
        }
        // Java: 122
        public com.oddlabs.util.ListElement getNext()
        {
            return next;
        }
        private void init_Element()
        {
            this.next = null;
            this.prior = null;
            this.dir_x = 1.0f;
            this.dir_y = 0.0f;
            this.reference = null;
        }
        public Element(com.oddlabs.tt.model.AbstractElementNode element_root) : base()
        {
            init_Element();
            this.element_root = element_root;
        }
    }
}
