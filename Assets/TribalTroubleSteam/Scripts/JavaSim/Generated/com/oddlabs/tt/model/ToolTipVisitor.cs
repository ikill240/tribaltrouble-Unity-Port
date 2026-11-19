// Translated from ToolTipVisitor.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 3
    public interface ToolTipVisitor
    {
        // Java: 4
        void visitUnit(com.oddlabs.tt.model.Unit unit);
        // Java: 6
        void visitBuilding(com.oddlabs.tt.model.Building building);
        // Java: 8
        void visitSupply(com.oddlabs.tt.model.Supply model);
        // Java: 10
        void visitSceneryModel(com.oddlabs.tt.model.SceneryModel model);
    }
}
