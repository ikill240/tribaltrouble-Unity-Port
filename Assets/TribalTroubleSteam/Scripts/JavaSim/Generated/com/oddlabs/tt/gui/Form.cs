// Translated from Form.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 17
    public class Form : com.oddlabs.tt.gui.Group
    {
        public java.util.Set close_listeners;
        public string caption;
        public bool drag;
        // Java: 33
        public override void compileCanvas()
        {
            int spacing = com.oddlabs.tt.gui.Skin.getSkin().getFormData().objectSpacing();
            com.oddlabs.tt.gui.Box form = default;
            if (caption != null)
            {
                form = com.oddlabs.tt.gui.Skin.getSkin().getFormData().form();
                base.compileCanvas(form.getLeftOffset() + spacing, form.getBottomOffset() + spacing, form.getRightOffset() + spacing, form.getTopOffset() + spacing);
                com.oddlabs.tt.gui.FormData form_data = com.oddlabs.tt.gui.Skin.getSkin().getFormData();
                com.oddlabs.tt.font.Font font = form_data.captionFont();
                com.oddlabs.tt.gui.GUIObject label = new com.oddlabs.tt.gui.Label(caption, font);
                label.setPos(form_data.captionLeft(), getHeight() - form_data.captionY() - font.getHeight() / 2);
                label.addMouseMotionListener(new com.oddlabs.tt.gui.Form.DragListener(this));
                addChild(label);
                com.oddlabs.tt.gui.GUIObject close_button = new com.oddlabs.tt.gui.IconButton(com.oddlabs.tt.gui.Skin.getSkin().getFormData().formClose(), null);
                close_button.setPos(getWidth() - close_button.getWidth() - form_data.closeRight(), getHeight() - close_button.getHeight() - form_data.closeTop());
                close_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
                {
                    this.cancel();
                }));
                addChild(close_button);
            }
            else
            {
                form = com.oddlabs.tt.gui.Skin.getSkin().getFormData().slimForm();
                base.compileCanvas(form.getLeftOffset() + spacing, form.getBottomOffset() + spacing, form.getRightOffset() + spacing, form.getTopOffset() + spacing);
            }
        }
        // Java: 67
        public void centerPos()
        {
            com.oddlabs.tt.gui.GUIRoot root = getParentGUIRoot();
            int w = default;
            int h = default;
            if (root != null)
            {
                w = root.getWidth();
                h = root.getHeight();
            }
            else
            {
                com.oddlabs.tt.render.Renderer renderer = com.oddlabs.tt.render.Renderer.getRenderer();
                int physW = renderer.getDisplayWidth();
                int physH = renderer.getDisplayHeight();
                float scale = com.oddlabs.tt.gui.GUIRoot.calculateEffectiveScale(physW, physH);
                w = java.lang.JC.f2i(((float)(physW / scale)));
                h = java.lang.JC.f2i(((float)(physH / scale)));
            }
            setPos((w - getWidth()) / 2, (h - getHeight()) / 2);
        }
        // Java: 84
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            com.oddlabs.tt.gui.FormData data = com.oddlabs.tt.gui.Skin.getSkin().getFormData();
            com.oddlabs.tt.gui.Box form = (caption != null ? data.form() : data.slimForm());
            com.oddlabs.tt.gui.ModeIconQuads.Mode skinMode = (isDisabled() ? com.oddlabs.tt.gui.ModeIconQuads.Mode.DISABLED : (isActive() ? com.oddlabs.tt.gui.ModeIconQuads.Mode.ACTIVE : com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL));
            form.render(renderer, 0.0f, 0.0f, getWidth(), getHeight(), skinMode);
        }
        // Java: 92
        public override void mousePressed(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
            if (caption != null && y >= getHeight() - com.oddlabs.tt.gui.Skin.getSkin().getFormData().form().getTopOffset())
            {
                drag = true;
            }
        }
        // Java: 98
        public override void mouseReleased(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
            drag = false;
        }
        // Java: 103
        public override void mouseDragged(com.oddlabs.tt.gui.MouseButton button, int x, int y, int rel_x, int rel_y, int abs_x, int abs_y)
        {
            if (drag)
            {
                setPos(getX() + rel_x, getY() + rel_y);
            }
        }
        // Java: 110
        public override void mouseScrolled(int amount)
        {
        }
        // Java: 114
        public override void mouseMoved(int x, int y)
        {
        }
        // Java: 118
        public override void mouseExited()
        {
        }
        // Java: 122
        public override void mouseEntered()
        {
        }
        // Java: 126
        public override void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
        {
        }
        // Java: 130
        public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
            if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.PRESSED) || ((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.REPEAT))
            {
                if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_NEXT_PANEL))
                {
                    cyclePanelGroup(this, com.oddlabs.tt.gui.FocusDirection.FORWARD);
                    @event.consume();
                    return;
                }
                if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_PREV_PANEL))
                {
                    cyclePanelGroup(this, com.oddlabs.tt.gui.FocusDirection.BACKWARD);
                    @event.consume();
                    return;
                }
                if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_CANCEL))
                {
                    cancel();
                    @event.consume();
                    return;
                }
            }
            base.handleInput(@event);
        }
        // Java: 152
        public bool cyclePanelGroup(com.oddlabs.tt.gui.GUIObject root, com.oddlabs.tt.gui.FocusDirection dir)
        {
            if ((root is com.oddlabs.tt.gui.PanelGroup pg))
            {
                pg.cyclePanel(dir);
                return true;
            }
            com.oddlabs.tt.gui.GUIObject child = ((com.oddlabs.tt.gui.GUIObject)root.getFirstChild());
            while (child != null)
            {
                if (cyclePanelGroup(child, dir))
                {
                    return true;
                }
                child = ((com.oddlabs.tt.gui.GUIObject)child.getNext());
            }
            return false;
        }
        // Java: 165
        public override void mouseHeld(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
        }
        // Java: 169
        public void closedAll()
        {
            closed();
            close_listeners.forEach(new java.util.function.Consumer_L((object a0) => { ((com.oddlabs.tt.guievent.CloseListener)a0).closed(); }));
        }
        // Java: 174
        public virtual void closed()
        {
        }
        // Java: 177
        public void addCloseListener(com.oddlabs.tt.guievent.CloseListener listener)
        {
            close_listeners.add(listener);
        }
        // Java: 181
        public void removeCloseListener(com.oddlabs.tt.guievent.CloseListener listener)
        {
            close_listeners.remove(listener);
        }
        // Java: 185
        public virtual void cancel()
        {
            doCancel();
            remove();
        }
        // Java: 190
        public virtual void doCancel()
        {
        }
        // Java: 193
        public override void remove()
        {
            if (((com.oddlabs.tt.gui.GUIObject)getParent()) != null)
            {
                closedAll();
            }
            base.remove();
        }
        // Java: 200
        public class DragListener : java.lang.Object, com.oddlabs.tt.guievent.MouseMotionListener
        {
            public com.oddlabs.tt.gui.Form owner;
            // Java: 207
            public void mouseDragged(com.oddlabs.tt.gui.MouseButton button, int x, int y, int rel_x, int rel_y, int abs_x, int abs_y)
            {
                owner.mouseDragged(button, x, y, rel_x, rel_y, abs_x, abs_y);
            }
            // Java: 213
            public void mouseMoved(int x, int y)
            {
            }
            // Java: 217
            public void mouseEntered()
            {
            }
            // Java: 221
            public void mouseExited()
            {
            }
            public DragListener(com.oddlabs.tt.gui.Form owner) : base()
            {
                this.owner = owner;
            }
        }
        private void init_Form()
        {
            this.close_listeners = new java.util.concurrent.CopyOnWriteArraySet();
            this.drag = false;
        }
        public Form(string caption) : base()
        {
            init_Form();
            this.caption = caption;
            setFocusCycle(true);
        }
        public Form() : this(null)
        {
        }
    }
}
