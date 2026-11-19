// Translated from PanelGroup.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 7
    public class PanelGroup : com.oddlabs.tt.gui.GUIObject
    {
        public com.oddlabs.tt.gui.Group focus_group;
        public com.oddlabs.tt.gui.PanelGroup.PanelBox box;
        public com.oddlabs.tt.gui.Panel[] panels;
        public int selected;
        // Java: 54
        public override void setFocus(com.oddlabs.tt.gui.FocusDirection direction)
        {
            focus_group.setGroupFocus(direction);
        }
        // Java: 59
        public void cyclePanel(com.oddlabs.tt.gui.FocusDirection dir)
        {
            int intDir = (((object)dir == (object)com.oddlabs.tt.gui.FocusDirection.BACKWARD) ? (-1) : 1);
            int next = (selected + intDir + panels.Length) % panels.Length;
            selectPanel(next);
        }
        // Java: 65
        public void selectPanel(int index)
        {
            focus_group.clearChildren();
            for (int i = 0; i < panels.Length; i++)
            {
                if (i != index)
                {
                    focus_group.addChild(panels[i].getTab());
                    panels[i].getTab().select(false);
                }
            }
            focus_group.addChild(box);
            focus_group.addChild(panels[index].getTab());
            panels[index].getTab().select(true);
            focus_group.addChild(panels[index]);
            selected = index;
            panels[index].setFocus();
        }
        // Java: 81
        public class PanelBox : com.oddlabs.tt.gui.GUIObject
        {
            public com.oddlabs.tt.gui.PanelGroup this_0;
            // Java: 87
            public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
            {
                com.oddlabs.tt.gui.Box panelBox = com.oddlabs.tt.gui.Skin.getSkin().getPanelData().box();
                panelBox.render(renderer, 0.0f, 0.0f, getWidth(), getHeight(), this_0.panels[this_0.selected].getTab().getRenderState());
            }
            public PanelBox(com.oddlabs.tt.gui.PanelGroup this_0_, int width, int height) : base()
            {
                this.this_0 = this_0_;
                setDim(width, height);
                setPos(0, 0);
            }
        }
        // Java: 94
        public class TabListener : java.lang.Object, com.oddlabs.tt.guievent.MouseButtonListener
        {
            public com.oddlabs.tt.gui.PanelGroup this_0;
            public int index;
            // Java: 101
            public void mousePressed(com.oddlabs.tt.gui.MouseButton button, int x, int y)
            {
                this_0.selectPanel(index);
            }
            // Java: 106
            public void mouseReleased(com.oddlabs.tt.gui.MouseButton button, int x, int y)
            {
            }
            // Java: 110
            public void mouseHeld(com.oddlabs.tt.gui.MouseButton button, int x, int y)
            {
            }
            // Java: 114
            public void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
            {
            }
            public TabListener(com.oddlabs.tt.gui.PanelGroup this_0_, int index) : base()
            {
                this.this_0 = this_0_;
                this.index = index;
            }
        }
        private void init_PanelGroup()
        {
            this.focus_group = new com.oddlabs.tt.gui.Group();
        }
        public PanelGroup(params com.oddlabs.tt.gui.Panel[] panels) : this(0, panels)
        {
        }
        public PanelGroup(int selected, params com.oddlabs.tt.gui.Panel[] panels) : base()
        {
            init_PanelGroup();
            this.panels = panels;
            int tab_height = panels[0].getTab().getHeight();
            int width = 0;
            int height = 0;
            foreach (com.oddlabs.tt.gui.Panel panel in panels)
            {
                if (width < panel.getWidth())
                {
                    width = panel.getWidth();
                }
                if (height < panel.getHeight())
                {
                    height = panel.getHeight();
                }
            }
            int total_height = height + tab_height;
            setDim(width, total_height);
            int x = com.oddlabs.tt.gui.Skin.getSkin().getPanelData().leftTabOffset();
            int y = height;
            for (int i = 0; i < panels.Length; i++)
            {
                panels[i].setPos((width - panels[i].getWidth()) / 2, com.oddlabs.tt.gui.Skin.getSkin().getPanelData().bottomTabOffset() + (height - panels[i].getHeight()) / 2);
                panels[i].getTab().setPos(x, y);
                x += panels[i].getTab().getWidth();
                panels[i].getTab().addMouseButtonListener(new com.oddlabs.tt.gui.PanelGroup.TabListener(this, i));
            }
            box = new com.oddlabs.tt.gui.PanelGroup.PanelBox(this, width, total_height - panels[0].getTab().getHeight() + com.oddlabs.tt.gui.Skin.getSkin().getPanelData().bottomTabOffset());
            focus_group.setDim(width, total_height);
            focus_group.setPos(0, 0);
            addChild(focus_group);
            setCanFocus(true);
            selectPanel(selected);
        }
    }
}
