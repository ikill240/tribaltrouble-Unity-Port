// Translated from ElementNode.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 5
    public class ElementNode : com.oddlabs.tt.model.AbstractElementNode
    {
        public const int MIN_NODE_SIZE = 4;
        public com.oddlabs.tt.model.AbstractElementNode child0;
        public com.oddlabs.tt.model.AbstractElementNode child1;
        public com.oddlabs.tt.model.AbstractElementNode child2;
        public com.oddlabs.tt.model.AbstractElementNode child3;
        // Java: 33
        public com.oddlabs.tt.model.AbstractElementNode createChild(int size, int x, int y)
        {
            if (size != com.oddlabs.tt.model.ElementNode.MIN_NODE_SIZE)
            {
                return new com.oddlabs.tt.model.ElementNode(this, size, x, y);
            }
            else
            {
                return new com.oddlabs.tt.model.ElementLeaf(this, size, x, y);
            }
        }
        // Java: 40
        public override com.oddlabs.tt.model.AbstractElementNode doInsertElement(com.oddlabs.tt.model.Element model)
        {
            incElementCount();
            if (model.bmin_x >= getCX())
            {
                if (model.bmin_y >= getCY())
                {
                    return child3.insertElement(model);
                }
                else if (model.bmax_y <= getCY())
                {
                    return child1.insertElement(model);
                }
            }
            else if (model.bmax_x <= getCX())
            {
                if (model.bmin_y >= getCY())
                {
                    return child2.insertElement(model);
                }
                else if (model.bmax_y <= getCY())
                {
                    return child0.insertElement(model);
                }
            }
            return addElement(model);
        }
        // Java: 57
        public override void visit(com.oddlabs.tt.model.ElementNodeVisitor visitor)
        {
            visitor.visitNode(this);
        }
        // Java: 62
        public void visitChildren(com.oddlabs.tt.model.ElementNodeVisitor visitor)
        {
            if (getChildCount() > 0)
            {
                child0.visit(visitor);
                child1.visit(visitor);
                child2.visit(visitor);
                child3.visit(visitor);
            }
        }
        public ElementNode(com.oddlabs.tt.model.AbstractElementNode owner, int size, int x, int y) : base(owner)
        {
            int child_size = size >> 1;
            child0 = createChild(child_size, x, y);
            child1 = createChild(child_size, x + child_size, y);
            child2 = createChild(child_size, x, y + child_size);
            child3 = createChild(child_size, x + child_size, y + child_size);
            checkBoundsXY(child0);
            checkBoundsXY(child1);
            checkBoundsXY(child2);
            checkBoundsXY(child3);
        }
    }
}
