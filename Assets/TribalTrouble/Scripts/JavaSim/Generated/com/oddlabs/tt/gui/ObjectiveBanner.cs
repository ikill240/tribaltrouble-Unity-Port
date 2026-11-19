// Translated from ObjectiveBanner.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 19
    public class ObjectiveBanner : com.oddlabs.tt.gui.GUIObject
    {
        public static org.joml.Vector4fc PANEL;
        public static org.joml.Vector4fc RIM;
        public static org.joml.Vector4fc TITLE;
        public const int PAD = 6;
        public const int MAX_WIDTH = 640;
        public const int TOP_MARGIN = 10;
        public com.oddlabs.tt.gui.GUIRoot root;
        public com.oddlabs.tt.font.Font font;
        public string title;
        public java.util.List lines;
        public string text;
        // Java: 40
        public void setText(string text)
        {
            string t = java.lang.JO.toString(text);
            if (java.lang.JO.equals(t, this.text))
            {
                return;
            }
            this.text = t;
            layout();
        }
        // Java: 48
        public void layout()
        {
            lines.clear();
            int max = java.lang.Math.min(com.oddlabs.tt.gui.ObjectiveBanner.MAX_WIDTH, java.lang.Math.max(200, root.getWidth() - 40)) - 2 * com.oddlabs.tt.gui.ObjectiveBanner.PAD;
            foreach (string para in text.split("\n"))
            {
                java.lang.StringBuilder line = new java.lang.StringBuilder();
                foreach (string word in para.split(" "))
                {
                    if (word.isEmpty())
                    {
                        continue;
                    }
                    string next = (line.isEmpty() ? word : java.lang.JStr.valueOf(line) + " " + java.lang.JStr.valueOf(word));
                    if (!line.isEmpty() && font.getWidth(next) > max)
                    {
                        lines.add(java.lang.JO.toString(line));
                        line.setLength(0);
                        line.append(word);
                    }
                    else
                    {
                        line.setLength(0);
                        line.append(next);
                    }
                }
                if (!line.isEmpty())
                {
                    lines.add(java.lang.JO.toString(line));
                }
            }
            int w = (title.isEmpty() ? 0 : font.getWidth(title));
            foreach (string l in java.lang.JIter.of(lines))
            {
                w = java.lang.Math.max(w, font.getWidth(l));
            }
            int line_h = font.getHeight() + 1;
            int n = lines.size() + ((title.isEmpty() ? 0 : 1));
            setDim(w + 2 * com.oddlabs.tt.gui.ObjectiveBanner.PAD, java.lang.Math.max(1, n) * line_h + 2 * com.oddlabs.tt.gui.ObjectiveBanner.PAD);
            setPos((root.getWidth() - getWidth()) / 2, root.getHeight() - getHeight() - com.oddlabs.tt.gui.ObjectiveBanner.TOP_MARGIN);
        }
        // Java: 78
        public override void displayChangedNotify(int width, int height)
        {
            layout();
        }
        // Java: 83
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            if (lines.isEmpty())
            {
                return;
            }
            int w = getWidth();
            int h = getHeight();
            renderer.drawColoredQuad(0, 0, w, h, com.oddlabs.tt.gui.ObjectiveBanner.PANEL);
            renderer.drawColoredQuad(0, h - 2, w, 2, com.oddlabs.tt.gui.ObjectiveBanner.RIM);
            renderer.drawColoredQuad(0, 0, w, 1, com.oddlabs.tt.gui.ObjectiveBanner.RIM);
            int line_h = font.getHeight() + 1;
            float y = h - com.oddlabs.tt.gui.ObjectiveBanner.PAD - line_h;
            if (!title.isEmpty())
            {
                com.oddlabs.tt.font.TextLineRenderer.render(renderer, font, title, (float)((w - font.getWidth(title)) / 2.0f), y, java.lang.Float.NEGATIVE_INFINITY, java.lang.Float.POSITIVE_INFINITY, com.oddlabs.tt.gui.ObjectiveBanner.TITLE);
                y = (float)(y - line_h);
            }
            foreach (string l in java.lang.JIter.of(lines))
            {
                com.oddlabs.tt.font.TextLineRenderer.render(renderer, font, l, (float)((w - font.getWidth(l)) / 2.0f), y, java.lang.Float.NEGATIVE_INFINITY, java.lang.Float.POSITIVE_INFINITY, com.oddlabs.util.Color.WHITE);
                y = (float)(y - line_h);
            }
        }
        static ObjectiveBanner()
        {
            ObjectiveBanner.PANEL = new org.joml.Vector4f(0.05f, 0.04f, 0.03f, 0.58f);
            ObjectiveBanner.RIM = new org.joml.Vector4f(0.86f, 0.68f, 0.32f, 0.6f);
            ObjectiveBanner.TITLE = new org.joml.Vector4f(1.0f, 0.84f, 0.45f, 1.0f);
        }
        private void init_ObjectiveBanner()
        {
            this.lines = new java.util.ArrayList();
            this.text = "";
        }
        public ObjectiveBanner(com.oddlabs.tt.gui.GUIRoot root, string title) : base()
        {
            init_ObjectiveBanner();
            this.root = root;
            this.font = com.oddlabs.tt.gui.Skin.getSkin().getEditFont();
            this.title = java.lang.JO.toString(title);
            layout();
        }
    }
}
