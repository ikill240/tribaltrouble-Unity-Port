// Translated from ProgressBar.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 13
    public class ProgressBar : com.oddlabs.tt.gui.GUIObject
    {
        public static java.util.ResourceBundle bundle;
        // Java: 16
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.gui.ProgressBar.bundle, key, args);
        }
        // Java: 20
        public class Waypoint : java.lang.Object
        {
            public int point() { return point_f; }
            public float weight() { return weight_f; }
            public Waypoint(int point, float weight)
            {
                this.point_f = point;
                this.weight_f = weight;
            }
            public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is Waypoint r)) return false; return true && java.lang.JO.equals(point_f, r.point_f) && java.lang.JO.equals(weight_f, r.weight_f); }
            public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(point_f); h = 31 * h + java.lang.JO.hashCode(weight_f); } return h; }
            public override string toString() { return "Waypoint[" + "point=" + java.lang.JO.toString(point_f) + ", weight=" + java.lang.JO.toString(weight_f) + "]"; }
            public int point_f;
            public float weight_f;
        }
        public com.oddlabs.tt.gui.ProgressBarInfo[] info;
        public com.oddlabs.tt.gui.ProgressBar.Waypoint[] waypoints;
        public bool text_only;
        public com.oddlabs.net.NetworkSelector network;
        public int left_margin;
        public int right_margin;
        public int index;
        public float step;
        // Java: 52
        public void progress()
        {
            index++;
            step = 0;
            update();
        }
        // Java: 59
        public void progress(float fraction)
        {
            int current = 0;
            if (index > 0)
            {
                current = waypoints[index - 1].point();
            }
            step += (float)(fraction * (waypoints[index].point() - current));
            if (step > waypoints[index].point() - current)
            {
                step = waypoints[index].point() - current;
            }
            update();
        }
        // Java: 72
        public void renderText(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            int offset = (index > 0 ? waypoints[index - 1].point() : 0);
            float done = (float)(((float)(offset + step)) / getWidth());
            java.util.ResourceBundle bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.gui.ProgressBar).getName());
            int percentage = java.lang.JC.f2i(((float)(done * 100)));
            string @string = i18n("loading", percentage);
            com.oddlabs.tt.font.TextLineRenderer.render(renderer, com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont(), @string, 0, 0, java.lang.Float.NEGATIVE_INFINITY, java.lang.Float.POSITIVE_INFINITY, com.oddlabs.util.Color.WHITE);
        }
        // Java: 82
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            if (text_only)
            {
                renderText(renderer);
            }
            else
            {
                com.oddlabs.tt.gui.Skin.getSkin().getProgressBarData().progressBar().render(renderer, 0, 0, getWidth(), com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL);
                renderFill(renderer, 0);
            }
        }
        // Java: 93
        public com.oddlabs.tt.gui.ProgressBar.Waypoint[] pixelize(com.oddlabs.tt.gui.ProgressBarInfo[] info, int width)
        {
            float sum = ((float)(java.util.Arrays.stream(info).mapToDouble(new java.util.function.ToDoubleFunction_L((object a0) => { return ((com.oddlabs.tt.gui.ProgressBarInfo)a0).getWeight(); })).sum()));
            com.oddlabs.tt.gui.ProgressBar.Waypoint[] waypoints = new com.oddlabs.tt.gui.ProgressBar.Waypoint[info.Length];
            int currentWaypoint = 0;
            for (int i = 0; i < info.Length; i++)
            {
                currentWaypoint += java.lang.JC.f2i(((float)(((float)(info[i].getWeight() / sum)) * width)));
                int point = java.lang.Math.min(currentWaypoint, width - right_margin);
                point = java.lang.Math.max(point, left_margin);
                waypoints[i] = new com.oddlabs.tt.gui.ProgressBar.Waypoint(point, info[i].getWaypoint());
            }
            waypoints[info.Length - 1] = new com.oddlabs.tt.gui.ProgressBar.Waypoint(width - right_margin, waypoints[info.Length - 1].weight());
            return waypoints;
        }
        // Java: 108
        public void renderFill(com.oddlabs.tt.render.GUIRenderer renderer, int y)
        {
            com.oddlabs.tt.gui.ProgressBarData data = com.oddlabs.tt.gui.Skin.getSkin().getProgressBarData();
            com.oddlabs.tt.gui.ModeIconQuads left = data.leftFill();
            com.oddlabs.tt.gui.ModeIconQuads center = data.centerFill();
            com.oddlabs.tt.gui.ModeIconQuads right = data.rightFill();
            renderer.drawModeIcon(left, com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL, 0, y);
            int offset = (index > 0 ? waypoints[index - 1].point() : 0);
            int current_pos = offset + java.lang.JC.f2i(step);
            int width = current_pos - left_margin;
            if (width > 0)
            {
                com.oddlabs.tt.gui.IconQuad c = center.quad(com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL);
                renderer.drawTexture(c.getTexture(), left_margin, y, width, c.getHeight(), c.getU1(), c.getV1(), c.getU2(), c.getV2(), com.oddlabs.util.Color.WHITE);
                renderer.drawModeIcon(right, com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL, current_pos, y);
            }
        }
        // Java: 128
        public void update()
        {
            network.tick();
        }
        static ProgressBar()
        {
            ProgressBar.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.gui.ProgressBar).getName());
        }
        public ProgressBar(com.oddlabs.net.NetworkSelector network, int width, com.oddlabs.tt.gui.ProgressBarInfo[] info, bool text_only) : base()
        {
            this.info = info;
            this.network = network;
            this.text_only = text_only;
            if (text_only)
            {
                setDim(width, com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont().getHeight());
            }
            else
            {
                com.oddlabs.tt.gui.ProgressBarData data = com.oddlabs.tt.gui.Skin.getSkin().getProgressBarData();
                left_margin = data.leftFill().quad(com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL).getWidth();
                right_margin = data.rightFill().quad(com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL).getWidth();
                setDim(width, data.progressBar().getHeight());
            }
            this.waypoints = pixelize(info, width);
            setCanFocus(false);
        }
    }
}
