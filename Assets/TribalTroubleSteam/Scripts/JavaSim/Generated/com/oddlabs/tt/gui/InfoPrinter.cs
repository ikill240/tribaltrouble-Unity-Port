// Translated from InfoPrinter.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 18
    public class InfoPrinter : com.oddlabs.tt.gui.GUIObject, com.oddlabs.tt.animation.Animated, com.oddlabs.tt.net.ChatListener
    {
        public const float SECONDS_PER_TIMEOUT = 8.0f;
        public static org.joml.Vector4fc PRIVATE_COLOR;
        public static org.joml.Vector4fc TEAM_COLOR;
        public com.oddlabs.tt.font.Font font;
        public java.util.List history;
        public java.util.List timers;
        public int lines;
        public com.oddlabs.tt.gui.GUIRoot gui_root;
        public float time;
        // Java: 40
        public com.oddlabs.tt.gui.GUIRoot getGUIRoot()
        {
            return gui_root;
        }
        // Java: 44
        public override void render(com.oddlabs.tt.render.GUIRenderer renderer, float clip_left, float clip_right, float clip_bottom, float clip_top)
        {
            if (com.oddlabs.tt.global.Globals.draw_hud)
            {
                base.render(renderer, clip_left, clip_right, clip_bottom, clip_top);
            }
        }
        // Java: 51
        public override void doAdd()
        {
            base.doAdd();
            com.oddlabs.tt.net.Network.getChatHub().addListener(this);
        }
        // Java: 57
        public override void doRemove()
        {
            base.doRemove();
            com.oddlabs.tt.net.Network.getChatHub().removeListener(this);
            com.oddlabs.tt.@event.LocalEventQueue.getQueue().getManager().removeAnimation(this);
        }
        // Java: 64
        public override void displayChangedNotify(int width, int height)
        {
            setDim(width, height);
        }
        // Java: 69
        public void chat(com.oddlabs.tt.net.ChatMessage message)
        {
            chat(message.formatShort(), message.type());
        }
        // Java: 74
        public void chat(string text, com.oddlabs.tt.net.ChatMessage.Type type)
        {
            switch ((type).ordinal())
            {
                case 0 /*NORMAL*/:
                {
                    print(text);
                    break;
                }
                case 1 /*TEAM*/:
                {
                    print(text, com.oddlabs.tt.gui.InfoPrinter.TEAM_COLOR);
                    break;
                }
                case 2 /*PRIVATE*/:
                {
                    print(text, com.oddlabs.tt.gui.InfoPrinter.PRIVATE_COLOR);
                    break;
                }
                case 5 /*SPECTATOR_CHAT*/:
                {
                    print(text, com.oddlabs.tt.gui.InfoPrinter.TEAM_COLOR);
                    break;
                }
                default:
                {
                    break;
                }
            }
        }
        // Java: 93
        public void print(string text)
        {
            print(text, com.oddlabs.util.Color.TRANSPARENT);
        }
        // Java: 97
        public void print(string text, org.joml.Vector4fc color)
        {
            int width = java.lang.Math.min(font.getWidth(text), getWidth());
            com.oddlabs.tt.gui.LabelBox label_box = new com.oddlabs.tt.gui.BackgroundLabelBox(text, font, width);
            if (color.w() > 0.2f)
            {
                label_box.setColor(color);
            }
            addChild(label_box);
            history.add(label_box);
            timers.add((float)(time + com.oddlabs.tt.gui.InfoPrinter.SECONDS_PER_TIMEOUT));
            while (history.size() > lines)
            {
                removeLine(0);
            }
            setLabelsPos();
        }
        // Java: 112
        public void removeLine(int index)
        {
            com.oddlabs.tt.gui.LabelBox label_box = ((com.oddlabs.tt.gui.LabelBox)history.get(index));
            label_box.remove();
            history.remove(index);
            timers.remove(index);
            setLabelsPos();
        }
        // Java: 120
        public void animate(float t)
        {
            time += t;
            for (int i = timers.size() - 1; i >= 0; i--)
            {
                float remove_time = ((float)((float?)timers.get(i)));
                if (time > remove_time)
                {
                    removeLine(i);
                }
            }
        }
        // Java: 131
        public void setLabelsPos()
        {
            int y = getHeight();
            foreach (com.oddlabs.tt.gui.LabelBox label_box in java.lang.JIter.of(history))
            {
                y -= label_box.getHeight();
                label_box.setPos(0, y);
            }
        }
        static InfoPrinter()
        {
            InfoPrinter.PRIVATE_COLOR = com.oddlabs.util.Color.argb4v((-13408513));
            InfoPrinter.TEAM_COLOR = com.oddlabs.util.Color.argb4v((-11763713));
        }
        private void init_InfoPrinter()
        {
            this.history = new java.util.ArrayList();
            this.timers = new java.util.ArrayList();
        }
        public InfoPrinter(com.oddlabs.tt.gui.GUIRoot gui_root, int lines, com.oddlabs.tt.font.Font font) : base()
        {
            init_InfoPrinter();
            this.gui_root = gui_root;
            this.lines = lines;
            this.font = font;
            displayChangedNotify(gui_root.getWidth(), gui_root.getHeight());
            com.oddlabs.tt.@event.LocalEventQueue.getQueue().getManager().registerAnimation(this);
            time = 0;
        }
        // mixin of default method Animated.updateChecksum
        public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
        }
    }
}
