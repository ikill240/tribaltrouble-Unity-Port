// Translated from WatchStatusIcon.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 10
    public class WatchStatusIcon : com.oddlabs.tt.gui.StatusIcon
    {
        public static org.joml.Vector4fc COLOR;
        public com.oddlabs.tt.model.Building building;
        // Java: 18
        public void setUnitContainerBuilding(com.oddlabs.tt.model.Building building)
        {
            this.building = building;
        }
        // Java: 22
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            base.renderGeometry(renderer);
            if (!building.isDead() && !building.getChieftainContainer().isTraining() && building.getOwner().getUnitCountContainer().getNumSupplies() < building.getOwner().getWorld().getMaxUnitCount())
            {
                float progress = (((com.oddlabs.tt.model.ReproduceUnitContainer)(building.getUnitContainer()))).getBuildProgress();
                com.oddlabs.tt.gui.IconQuad watch = com.oddlabs.tt.gui.GUIIcons.getIcons().getWatch(progress);
                int x = getWidth() - watch.getWidth();
                int y = (getHeight() - watch.getHeight()) / 2;
                x -= 5;
                renderer.drawTexture(watch.getTexture(), x, y, watch.getWidth(), watch.getHeight(), watch.getU1(), watch.getV1(), watch.getU2(), watch.getV2(), com.oddlabs.tt.gui.WatchStatusIcon.COLOR);
            }
        }
        static WatchStatusIcon()
        {
            WatchStatusIcon.COLOR = new org.joml.Vector4f(1.0f, 1.0f, 1.0f, 0.75f);
        }
        public WatchStatusIcon(int label_width, com.oddlabs.tt.gui.IconQuad icon, string tooltip) : base(label_width, icon, tooltip)
        {
        }
    }
}
