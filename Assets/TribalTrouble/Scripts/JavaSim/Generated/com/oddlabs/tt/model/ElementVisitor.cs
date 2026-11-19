// Translated from ElementVisitor.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 11
    public interface ElementVisitor
    {
        // Java: 12
        void visitUnit(com.oddlabs.tt.model.Unit selectable);
        // Java: 14
        void visitBuilding(com.oddlabs.tt.model.Building selectable);
        // Java: 16
        void visitEmitter(com.oddlabs.tt.particle.Emitter emitter);
        // Java: 18
        void visitLightning(com.oddlabs.tt.particle.Lightning lightning);
        // Java: 20
        void visitSonicBlastEffect(com.oddlabs.tt.particle.SonicBlastEffect effect);
        // Java: 22
        void visitRespond(com.oddlabs.tt.landscape.LandscapeTargetRespond respond);
        // Java: 24
        void visitSupplyModel(com.oddlabs.tt.model.SupplyModel model);
        // Java: 26
        void visitSceneryModel(com.oddlabs.tt.model.SceneryModel model);
        // Java: 28
        void visitRubberSupply(com.oddlabs.tt.model.RubberSupply model);
        // Java: 30
        void visitDirectedThrowingWeapon(com.oddlabs.tt.model.weapon.DirectedThrowingWeapon model);
        // Java: 32
        void visitRotatingThrowingWeapon(com.oddlabs.tt.model.weapon.RotatingThrowingWeapon model);
        // Java: 34
        void visitPlants(com.oddlabs.tt.model.Plants plants);
    }
}
