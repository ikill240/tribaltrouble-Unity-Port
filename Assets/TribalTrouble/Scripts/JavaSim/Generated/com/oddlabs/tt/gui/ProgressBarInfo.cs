// Translated from ProgressBarInfo.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 6
    public class ProgressBarInfo : java.lang.Object
    {
        public com.oddlabs.tt.gui.Label label;
        public float weight;
        public int waypoint;
        // Java: 17
        public float getWeight()
        {
            return weight;
        }
        // Java: 21
        public void setWaypoint(int waypoint)
        {
            this.waypoint = waypoint;
        }
        // Java: 25
        public int getWaypoint()
        {
            return waypoint;
        }
        // Java: 29
        public com.oddlabs.tt.gui.Label getLabel()
        {
            return label;
        }
        public ProgressBarInfo(string title, float weight) : base()
        {
            com.oddlabs.tt.font.Font font = com.oddlabs.tt.gui.Skin.getSkin().getProgressBarData().font();
            label = new com.oddlabs.tt.gui.Label(title, font);
            this.weight = weight;
        }
    }
}
