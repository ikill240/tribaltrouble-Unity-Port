// Translated from ElementLeaf.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 6
    public class ElementLeaf : com.oddlabs.tt.model.AbstractElementNode
    {
        // Java: 14
        public override com.oddlabs.tt.model.AbstractElementNode doInsertElement(com.oddlabs.tt.model.Element model)
        {
            incElementCount();
            return addElement(model);
        }
        // Java: 20
        public override void visit(com.oddlabs.tt.model.ElementNodeVisitor visitor)
        {
            visitor.visitLeaf(this);
        }
        public ElementLeaf(com.oddlabs.tt.model.AbstractElementNode owner, int size, int x, int y) : base(owner)
        {
            setBounds(x * com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID, (x + size) * com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID, y * com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID, (y + size) * com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID, java.lang.Float.POSITIVE_INFINITY, java.lang.Float.NEGATIVE_INFINITY);
        }
    }
}
