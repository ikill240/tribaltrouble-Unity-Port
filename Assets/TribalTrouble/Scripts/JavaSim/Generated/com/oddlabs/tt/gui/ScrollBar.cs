// Translated from ScrollBar.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 11
    public class ScrollBar : com.oddlabs.tt.gui.GUIObject
    {
        public com.oddlabs.tt.gui.Group focus_group;
        public com.oddlabs.tt.gui.ArrowButton less_button;
        public com.oddlabs.tt.gui.ArrowButton more_button;
        public com.oddlabs.tt.gui.ScrollButton scroll_button;
        public com.oddlabs.tt.gui.Scrollable owner;
        // Java: 48
        public void update()
        {
            scroll_button.setupPos(this);
        }
        // Java: 52
        public override void setPos(int x, int y)
        {
            base.setPos(x, y);
            scroll_button.setupPos(this);
        }
        // Java: 58
        public override com.oddlabs.tt.gui.Renderable setDim(int width, int height)
        {
            base.setDim(width, height);
            scroll_button.setupPos(this);
            return this;
        }
        // Java: 65
        public override void setFocus(com.oddlabs.tt.gui.FocusDirection direction)
        {
            focus_group.setGroupFocus(direction);
        }
        // Java: 70
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            com.oddlabs.tt.gui.ScrollBarData data = com.oddlabs.tt.gui.Skin.getSkin().getScrollBarData();
            com.oddlabs.tt.gui.Vertical scroll_bar = data.scrollBar();
            scroll_bar.render(renderer, 0, less_button.getHeight(), getHeight() - less_button.getHeight() - more_button.getHeight(), com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL);
        }
        // Java: 78
        public int getButtonX()
        {
            return com.oddlabs.tt.gui.Skin.getSkin().getScrollBarData().leftOffset();
        }
        // Java: 82
        public int getButtonY()
        {
            com.oddlabs.tt.gui.ScrollBarData data = com.oddlabs.tt.gui.Skin.getSkin().getScrollBarData();
            int max_height = getHeight() - less_button.getHeight() - more_button.getHeight() - data.bottomOffset() - data.topOffset();
            int size = getButtonHeight();
            int offset = max_height - size - java.lang.JC.f2i(((float)((max_height - size) * owner.getScrollBarOffset())));
            return less_button.getHeight() + data.bottomOffset() + offset;
        }
        // Java: 90
        public int getButtonHeight()
        {
            com.oddlabs.tt.gui.ScrollBarData data = com.oddlabs.tt.gui.Skin.getSkin().getScrollBarData();
            int max_height = getHeight() - less_button.getHeight() - more_button.getHeight() - data.bottomOffset() - data.topOffset();
            float ratio = owner.getScrollBarRatio();
            int size = java.lang.Math.max(java.lang.JC.f2i(((float)(ratio * max_height))), data.scrollButton().getMinHeight());
            return size;
        }
        // Java: 98
        public override void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
        {
            int button_y = getButtonY();
            owner.jumpPage(y > button_y);
            scroll_button.setupPos(this);
        }
        // Java: 105
        public class LessListener : java.lang.Object, com.oddlabs.tt.guievent.MouseButtonListener
        {
            public com.oddlabs.tt.gui.ScrollBar this_0;
            // Java: 106
            public void mousePressed(com.oddlabs.tt.gui.MouseButton button, int x, int y)
            {
                this_0.owner.setOffsetY(this_0.owner.getOffsetY() + this_0.owner.getStepHeight());
                this_0.scroll_button.setupPos(this.this_0);
            }
            // Java: 112
            public void mouseReleased(com.oddlabs.tt.gui.MouseButton button, int x, int y)
            {
            }
            // Java: 116
            public void mouseHeld(com.oddlabs.tt.gui.MouseButton button, int x, int y)
            {
            }
            // Java: 120
            public void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
            {
            }
            public LessListener(com.oddlabs.tt.gui.ScrollBar this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        // Java: 125
        public class MoreListener : java.lang.Object, com.oddlabs.tt.guievent.MouseButtonListener
        {
            public com.oddlabs.tt.gui.ScrollBar this_0;
            // Java: 126
            public void mousePressed(com.oddlabs.tt.gui.MouseButton button, int x, int y)
            {
                this_0.owner.setOffsetY(this_0.owner.getOffsetY() - this_0.owner.getStepHeight());
                this_0.scroll_button.setupPos(this.this_0);
            }
            // Java: 132
            public void mouseReleased(com.oddlabs.tt.gui.MouseButton button, int x, int y)
            {
            }
            // Java: 136
            public void mouseHeld(com.oddlabs.tt.gui.MouseButton button, int x, int y)
            {
            }
            // Java: 140
            public void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
            {
            }
            public MoreListener(com.oddlabs.tt.gui.ScrollBar this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        // Java: 145
        public class DragListener : java.lang.Object, com.oddlabs.tt.guievent.MouseMotionListener, com.oddlabs.tt.guievent.MouseButtonListener
        {
            public com.oddlabs.tt.gui.ScrollBar this_0;
            public com.oddlabs.tt.gui.ScrollBarData data;
            public float start_offset;
            // Java: 149
            public void mousePressed(com.oddlabs.tt.gui.MouseButton button, int x, int y)
            {
                start_offset = this_0.owner.getScrollBarOffset();
            }
            // Java: 154
            public void mouseDragged(com.oddlabs.tt.gui.MouseButton button, int x, int y, int rel_x, int rel_y, int abs_x, int abs_y)
            {
                int max_height = this_0.getHeight() - this_0.less_button.getHeight() - this_0.more_button.getHeight() - data.bottomOffset() - data.topOffset();
                float ratio = this_0.owner.getScrollBarRatio();
                int size = java.lang.JC.f2i(((float)(ratio * max_height)));
                int scroll_button_space = max_height - size;
                this_0.owner.setScrollBarOffset((float)(start_offset - (float)(abs_y / ((float)(scroll_button_space)))));
                this_0.scroll_button.setupPos(this.this_0);
            }
            // Java: 165
            public void mouseMoved(int x, int y)
            {
            }
            // Java: 169
            public void mouseEntered()
            {
            }
            // Java: 173
            public void mouseExited()
            {
            }
            // Java: 177
            public void mouseReleased(com.oddlabs.tt.gui.MouseButton button, int x, int y)
            {
            }
            // Java: 181
            public void mouseHeld(com.oddlabs.tt.gui.MouseButton button, int x, int y)
            {
            }
            // Java: 185
            public void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
            {
            }
            private void init_DragListener()
            {
                this.data = com.oddlabs.tt.gui.Skin.getSkin().getScrollBarData();
            }
            public DragListener(com.oddlabs.tt.gui.ScrollBar this_0_) : base()
            {
                this.this_0 = this_0_;
                init_DragListener();
            }
        }
        // Java: 190
        public void handleButtonInput(com.oddlabs.tt.input.InputEvent @event)
        {
            if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.REPEAT) || ((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.PRESSED))
            {
                if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_NAV_UP))
                {
                    owner.setOffsetY(owner.getOffsetY() - owner.getStepHeight());
                    scroll_button.setupPos(this);
                }
                else if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_NAV_DOWN))
                {
                    owner.setOffsetY(owner.getOffsetY() + owner.getStepHeight());
                    scroll_button.setupPos(this);
                }
            }
        }
        private void init_ScrollBar()
        {
            this.focus_group = new com.oddlabs.tt.gui.Group();
            this.scroll_button = new com.oddlabs.tt.gui.ScrollButton();
        }
        public ScrollBar(int height, com.oddlabs.tt.gui.Scrollable owner) : base()
        {
            init_ScrollBar();
            this.owner = owner;
            less_button = new com.oddlabs.tt.gui.ArrowButton(com.oddlabs.tt.gui.Skin.getSkin().getScrollBarData().scrollDownButtonPressed(), com.oddlabs.tt.gui.Skin.getSkin().getScrollBarData().scrollDownButtonUnpressed(), com.oddlabs.tt.gui.Skin.getSkin().getScrollBarData().scrollDownArrow());
            more_button = new com.oddlabs.tt.gui.ArrowButton(com.oddlabs.tt.gui.Skin.getSkin().getScrollBarData().scrollUpButtonPressed(), com.oddlabs.tt.gui.Skin.getSkin().getScrollBarData().scrollUpButtonUnpressed(), com.oddlabs.tt.gui.Skin.getSkin().getScrollBarData().scrollUpArrow());
            less_button.setPos(0, 0);
            more_button.setPos(0, height - more_button.getHeight());
            focus_group.addChild(more_button);
            less_button.addMouseButtonListener(new com.oddlabs.tt.gui.ScrollBar.LessListener(this));
            more_button.addMouseButtonListener(new com.oddlabs.tt.gui.ScrollBar.MoreListener(this));
            setDim(less_button.getWidth(), height);
            setCanFocus(true);
            focus_group.addChild(scroll_button);
            focus_group.addChild(less_button);
            com.oddlabs.tt.gui.ScrollBar.DragListener drag_listener = new com.oddlabs.tt.gui.ScrollBar.DragListener(this);
            scroll_button.addMouseMotionListener(drag_listener);
            scroll_button.addMouseButtonListener(drag_listener);
            scroll_button.addInputListener(new com.oddlabs.tt.guievent.InputListener_L((com.oddlabs.tt.input.InputEvent a0) => { this.handleButtonInput(a0); }));
            focus_group.setDim(getWidth(), getHeight());
            focus_group.setPos(0, 0);
            addChild(focus_group);
        }
    }
}
