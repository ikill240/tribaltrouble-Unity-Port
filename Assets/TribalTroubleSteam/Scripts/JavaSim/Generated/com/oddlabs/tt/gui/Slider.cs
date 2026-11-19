// Translated from Slider.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 12
    public class Slider : com.oddlabs.tt.gui.GUIObject
    {
        public java.util.Set value_listeners;
        public java.util.Set release_listeners;
        public com.oddlabs.tt.gui.SliderButton button;
        public int left_offset;
        public int right_offset;
        public int min;
        public int cardinality;
        public float step;
        public int @value;
        // Java: 44
        public void addReleaseListener(java.lang.Runnable listener)
        {
            release_listeners.add(listener);
        }
        // Java: 48
        public void notifyRelease()
        {
            foreach (java.lang.Runnable r in java.lang.JIter.of(release_listeners))
            {
                r.run();
            }
        }
        // Java: 54
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            com.oddlabs.tt.gui.Skin.getSkin().getSliderData().slider().render(renderer, 0, 0, getWidth(), (isDisabled() ? com.oddlabs.tt.gui.ModeIconQuads.Mode.DISABLED : com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL));
        }
        // Java: 60
        public int getValue()
        {
            return min + @value;
        }
        // Java: 64
        public float computeStep()
        {
            return (float)((getWidth() - left_offset - right_offset - button.getWidth()) / ((float)((cardinality - 1))));
        }
        // Java: 69
        public void setMax(int max)
        {
            int current = getValue();
            cardinality = max - min + 1;
            step = computeStep();
            setValue(current);
        }
        // Java: 77
        public int valueToOffset(int @value)
        {
            return java.lang.JC.f2i(((float)(@value * step))) + left_offset;
        }
        // Java: 81
        public int offsetToValue(int offset)
        {
            return java.lang.JC.f2i(((float)((float)(offset / step) + 0.5f)));
        }
        // Java: 85
        public override void mouseHeld(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
            mousePressed(button, x, y);
        }
        // Java: 90
        public override void mouseScrolled(int amount)
        {
            if (!isDisabled())
            {
                if (amount < 0)
                {
                    setValue(@value - 1 + min);
                }
                else
                {
                    setValue(@value + 1 + min);
                }
                button.setFocus();
            }
        }
        // Java: 101
        public override void mousePressed(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
            if (!isDisabled())
            {
                int dx = x - this.button.getX();
                if (dx < (float)((float)(-step) / 2))
                {
                    setValue(@value - 1 + min);
                }
                else if (dx > (float)(step / 2))
                {
                    setValue(@value + 1 + min);
                }
                this.button.setFocus();
            }
        }
        // Java: 115
        public override void mouseReleased(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
            if (!isDisabled())
            {
                notifyRelease();
            }
        }
        // Java: 122
        public void setValue(int @value)
        {
            int start_value = this.@value;
            this.@value = @value - min;
            cropValue();
            button.setPos(valueToOffset(this.@value), 0);
            if (start_value != this.@value)
            {
                valueSetAll(getValue());
            }
        }
        // Java: 131
        public void cropValue()
        {
            if (@value < 0)
            {
                @value = 0;
            }
            else if (@value > cardinality - 1)
            {
                @value = cardinality - 1;
            }
        }
        // Java: 139
        public void valueSetAll(int @value)
        {
            valueSet(@value);
            foreach (com.oddlabs.tt.guievent.ValueListener listener in java.lang.JIter.of(value_listeners))
            {
                listener.valueSet(@value);
            }
        }
        // Java: 146
        public void valueSet(int @value)
        {
        }
        // Java: 154
        public void addValueListener(com.oddlabs.tt.guievent.ValueListener listener)
        {
            value_listeners.add(listener);
        }
        // Java: 158
        public void removeValueListener(com.oddlabs.tt.guievent.ValueListener listener)
        {
            value_listeners.remove(listener);
        }
        // Java: 162
        public class DragListener : java.lang.Object, com.oddlabs.tt.guievent.MouseMotionListener, com.oddlabs.tt.guievent.MouseButtonListener
        {
            public com.oddlabs.tt.gui.Slider this_0;
            public com.oddlabs.tt.gui.SliderData data;
            public float start_offset;
            // Java: 166
            public void mousePressed(com.oddlabs.tt.gui.MouseButton button, int x, int y)
            {
                start_offset = this_0.valueToOffset(this_0.@value);
            }
            // Java: 171
            public void mouseDragged(com.oddlabs.tt.gui.MouseButton button, int x, int y, int rel_x, int rel_y, int abs_x, int abs_y)
            {
                if (!this_0.isDisabled())
                {
                    this_0.setValue(this_0.offsetToValue(java.lang.JC.f2i(((float)(start_offset + abs_x)))) + this_0.min);
                }
            }
            // Java: 179
            public void mouseMoved(int x, int y)
            {
            }
            // Java: 183
            public void mouseEntered()
            {
            }
            // Java: 187
            public void mouseExited()
            {
            }
            // Java: 191
            public void mouseReleased(com.oddlabs.tt.gui.MouseButton button, int x, int y)
            {
                this_0.notifyRelease();
            }
            // Java: 196
            public void mouseHeld(com.oddlabs.tt.gui.MouseButton button, int x, int y)
            {
            }
            // Java: 200
            public void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
            {
            }
            private void init_DragListener()
            {
                this.data = com.oddlabs.tt.gui.Skin.getSkin().getSliderData();
            }
            public DragListener(com.oddlabs.tt.gui.Slider this_0_) : base()
            {
                this.this_0 = this_0_;
                init_DragListener();
            }
        }
        private void init_Slider()
        {
            this.value_listeners = new java.util.concurrent.CopyOnWriteArraySet();
            this.release_listeners = new java.util.concurrent.CopyOnWriteArraySet();
        }
        public Slider(int width, int min, int max, int init_value) : base()
        {
            init_Slider();
            cardinality = max - min + 1;
            this.min = min;
            left_offset = com.oddlabs.tt.gui.Skin.getSkin().getSliderData().leftOffset();
            right_offset = com.oddlabs.tt.gui.Skin.getSkin().getSliderData().rightOffset();
            setDim(width, com.oddlabs.tt.gui.Skin.getSkin().getSliderData().slider().getHeight());
            setCanFocus(true);
            button = new com.oddlabs.tt.gui.SliderButton(this, com.oddlabs.tt.gui.Skin.getSkin().getSliderData().button());
            step = computeStep();
            setValue(init_value);
            addChild(button);
            com.oddlabs.tt.gui.Slider.DragListener drag_listener = new com.oddlabs.tt.gui.Slider.DragListener(this);
            button.addMouseMotionListener(drag_listener);
            button.addMouseButtonListener(drag_listener);
        }
    }
}
