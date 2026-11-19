// Translated from GUITreeDump.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 13
    public class GUITreeDump : java.lang.Object
    {
        // Java: 17
        public static string dump(com.oddlabs.tt.gui.GUIRoot root, int framebuffer_height)
        {
            java.lang.StringBuilder @out = new java.lang.StringBuilder();
            com.oddlabs.tt.gui.GUITreeDump.append(@out, root, 0, 0, 0, root.getGlobalScale(), framebuffer_height);
            return java.lang.JO.toString(@out);
        }
        // Java: 23
        public static void append(java.lang.StringBuilder @out, com.oddlabs.tt.gui.GUIObject @object, int depth, int parent_x, int parent_y, float scale, int framebuffer_height)
        {
            int abs_x = parent_x + @object.getX();
            int abs_y = parent_y + @object.getY();
            int px = java.lang.Math.round((float)(abs_x * scale));
            int py = framebuffer_height - java.lang.Math.round((float)((abs_y + @object.getHeight()) * scale));
            int pw = java.lang.Math.round((float)(@object.getWidth() * scale));
            int ph = java.lang.Math.round((float)(@object.getHeight() * scale));
            @out.append(depth).append(' ').append(@object.GetType().getSimpleName()).append(' ').append(px).append(' ').append(py).append(' ').append(pw).append(' ').append(ph);
            string text = com.oddlabs.tt.gui.GUITreeDump.textOf(@object);
            if (!text.isEmpty())
            {
                @out.append(' ').append(text);
            }
            @out.append('\n');
            com.oddlabs.tt.gui.GUIObject child = ((com.oddlabs.tt.gui.GUIObject)@object.getFirstChild());
            while (child != null)
            {
                com.oddlabs.tt.gui.GUITreeDump.append(@out, child, depth + 1, abs_x, abs_y, scale, framebuffer_height);
                child = ((com.oddlabs.tt.gui.GUIObject)child.getNext());
            }
        }
        // Java: 45
        public static string textOf(com.oddlabs.tt.gui.GUIObject @object)
        {
            if ((@object is com.oddlabs.tt.gui.PasswordLine field))
            {
                return "*".repeat(field.getContents().length());
            }
            if ((@object is com.oddlabs.tt.gui.TextField field_2))
            {
                return field_2.getContents().replace('\n', ' ');
            }
            if ((@object is com.oddlabs.tt.gui.MenuButton button))
            {
                return java.lang.JO.toString(button.getText());
            }
            if ((@object is com.oddlabs.tt.gui.Diode diode))
            {
                return (diode.isLit() ? "on" : "off");
            }
            return "";
        }
        // Java: 61
        public class Placed : java.lang.Object
        {
            public com.oddlabs.tt.gui.GUIObject @object() { return object_f; }
            public int x() { return x_f; }
            public int y() { return y_f; }
            public int w() { return w_f; }
            public int h() { return h_f; }
            public string text() { return text_f; }
            public bool clickable() { return clickable_f; }
            public Placed(com.oddlabs.tt.gui.GUIObject @object, int x, int y, int w, int h, string text, bool clickable)
            {
                this.object_f = @object;
                this.x_f = x;
                this.y_f = y;
                this.w_f = w;
                this.h_f = h;
                this.text_f = text;
                this.clickable_f = clickable;
            }
            public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is Placed r)) return false; return true && java.lang.JO.equals(object_f, r.object_f) && java.lang.JO.equals(x_f, r.x_f) && java.lang.JO.equals(y_f, r.y_f) && java.lang.JO.equals(w_f, r.w_f) && java.lang.JO.equals(h_f, r.h_f) && java.lang.JO.equals(text_f, r.text_f) && java.lang.JO.equals(clickable_f, r.clickable_f); }
            public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(object_f); h = 31 * h + java.lang.JO.hashCode(x_f); h = 31 * h + java.lang.JO.hashCode(y_f); h = 31 * h + java.lang.JO.hashCode(w_f); h = 31 * h + java.lang.JO.hashCode(h_f); h = 31 * h + java.lang.JO.hashCode(text_f); h = 31 * h + java.lang.JO.hashCode(clickable_f); } return h; }
            public override string toString() { return "Placed[" + "object=" + java.lang.JO.toString(object_f) + ", x=" + java.lang.JO.toString(x_f) + ", y=" + java.lang.JO.toString(y_f) + ", w=" + java.lang.JO.toString(w_f) + ", h=" + java.lang.JO.toString(h_f) + ", text=" + java.lang.JO.toString(text_f) + ", clickable=" + java.lang.JO.toString(clickable_f) + "]"; }
            public com.oddlabs.tt.gui.GUIObject object_f;
            public int x_f;
            public int y_f;
            public int w_f;
            public int h_f;
            public string text_f;
            public bool clickable_f;
            // Java: 63
            public int centerX()
            {
                return x_f + w_f / 2;
            }
            // Java: 67
            public int centerY()
            {
                return y_f + h_f / 2;
            }
        }
        // Java: 72
        public static bool isClickable(com.oddlabs.tt.gui.GUIObject @object)
        {
            return (@object is com.oddlabs.tt.gui.ButtonObject) || (@object is com.oddlabs.tt.gui.PanelTab) || (@object is com.oddlabs.tt.gui.PulldownButton) || (@object is com.oddlabs.tt.gui.EditLine);
        }
        // Java: 77
        public static void collect(java.util.List @out, com.oddlabs.tt.gui.GUIObject @object, int parent_x, int parent_y, float scale, int framebuffer_height, bool inside_clickable)
        {
            int abs_x = parent_x + @object.getX();
            int abs_y = parent_y + @object.getY();
            int px = java.lang.Math.round((float)(abs_x * scale));
            int py = framebuffer_height - java.lang.Math.round((float)((abs_y + @object.getHeight()) * scale));
            bool clickable = inside_clickable || com.oddlabs.tt.gui.GUITreeDump.isClickable(@object);
            @out.add(new com.oddlabs.tt.gui.GUITreeDump.Placed(@object, px, py, java.lang.Math.round((float)(@object.getWidth() * scale)), java.lang.Math.round((float)(@object.getHeight() * scale)), com.oddlabs.tt.gui.GUITreeDump.textOf(@object), clickable));
            com.oddlabs.tt.gui.GUIObject child = ((com.oddlabs.tt.gui.GUIObject)@object.getFirstChild());
            while (child != null)
            {
                com.oddlabs.tt.gui.GUITreeDump.collect(@out, child, abs_x, abs_y, scale, framebuffer_height, clickable);
                child = ((com.oddlabs.tt.gui.GUIObject)child.getNext());
            }
        }
        // Java: 93
        public static void collect(java.util.List @out, com.oddlabs.tt.gui.GUIObject @object, int parent_x, int parent_y, float scale, int framebuffer_height)
        {
            com.oddlabs.tt.gui.GUITreeDump.collect(@out, @object, parent_x, parent_y, scale, framebuffer_height, false);
        }
        // Java: 100
        public static com.oddlabs.tt.gui.GUITreeDump.Placed last(com.oddlabs.tt.gui.GUIRoot root, int framebuffer_height, string text)
        {
            java.util.List placed = new java.util.ArrayList();
            com.oddlabs.tt.gui.GUITreeDump.collect(placed, root, 0, 0, root.getGlobalScale(), framebuffer_height);
            com.oddlabs.tt.gui.GUITreeDump.Placed match = null;
            foreach (com.oddlabs.tt.gui.GUITreeDump.Placed p in java.lang.JIter.of(placed))
            {
                if (p.text().equalsIgnoreCase(text.strip()) && (match == null || p.clickable() || !match.clickable()))
                {
                    match = p;
                }
            }
            return match;
        }
        // Java: 113
        public static int[] find(com.oddlabs.tt.gui.GUIRoot root, int framebuffer_height, string text)
        {
            com.oddlabs.tt.gui.GUITreeDump.Placed p = com.oddlabs.tt.gui.GUITreeDump.last(root, framebuffer_height, text);
            return (p == null ? (int[])(null) : new int[] { p.centerX(), p.centerY() });
        }
        // Java: 120
        public static com.oddlabs.tt.gui.GUITreeDump.Placed onRow(com.oddlabs.tt.gui.GUIRoot root, int framebuffer_height, string label, System.Type type)
        {
            com.oddlabs.tt.gui.GUITreeDump.Placed anchor = com.oddlabs.tt.gui.GUITreeDump.last(root, framebuffer_height, label);
            if (anchor == null)
            {
                return null;
            }
            if (type.isInstance(anchor.@object()))
            {
                return anchor;
            }
            java.util.List placed = new java.util.ArrayList();
            com.oddlabs.tt.gui.GUITreeDump.collect(placed, root, 0, 0, root.getGlobalScale(), framebuffer_height);
            com.oddlabs.tt.gui.GUITreeDump.Placed best = null;
            foreach (com.oddlabs.tt.gui.GUITreeDump.Placed p in java.lang.JIter.of(placed))
            {
                if (type.isInstance(p.@object()) && p.x() >= anchor.x() && java.lang.Math.abs(p.centerY() - anchor.centerY()) <= anchor.h() && (best == null || p.x() < best.x()))
                {
                    best = p;
                }
            }
            return best;
        }
        // Java: 143
        public static com.oddlabs.tt.gui.TextField findField(com.oddlabs.tt.gui.GUIRoot root, int framebuffer_height, string label)
        {
            com.oddlabs.tt.gui.GUITreeDump.Placed p = com.oddlabs.tt.gui.GUITreeDump.onRow(root, framebuffer_height, label, typeof(com.oddlabs.tt.gui.EditLine));
            return (p == null ? (com.oddlabs.tt.gui.TextField)(null) : ((com.oddlabs.tt.gui.TextField)p.@object()));
        }
        // Java: 150
        public static int[] findPulldown(com.oddlabs.tt.gui.GUIRoot root, int framebuffer_height, string label)
        {
            com.oddlabs.tt.gui.GUITreeDump.Placed p = com.oddlabs.tt.gui.GUITreeDump.onRow(root, framebuffer_height, label, typeof(com.oddlabs.tt.gui.PulldownButton));
            return (p == null ? (int[])(null) : new int[] { p.centerX(), p.centerY() });
        }
        // Java: 157
        public static bool has(com.oddlabs.tt.gui.GUIRoot root, int framebuffer_height, string text)
        {
            return com.oddlabs.tt.gui.GUITreeDump.last(root, framebuffer_height, text) != null;
        }
        public GUITreeDump() : base()
        {
        }
    }
}
