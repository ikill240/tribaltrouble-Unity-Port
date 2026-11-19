// Translated from Group.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 8
    public class Group : com.oddlabs.tt.gui.GUIObject
    {
        // Java: 17
        public virtual void compileCanvas()
        {
            compileCanvas(0, 0, 0, 0);
        }
        // Java: 21
        public virtual void compileCanvas(int left_offset, int bottom_offset, int right_offset, int top_offset)
        {
            int min_x_tl = 0;
            int min_y_tl = 0;
            int max_x_tl = 0;
            int max_y_tl = 0;
            int min_x_br = 0;
            int min_y_br = 0;
            int max_x_br = 0;
            int max_y_br = 0;
            bool origin_top_left = false;
            bool origin_bottom_right = false;
            com.oddlabs.tt.gui.GUIObject current = ((com.oddlabs.tt.gui.GUIObject)getFirstChild());
            while (current != null)
            {
                if (((object)current.getOrigin() == (object)com.oddlabs.tt.gui.Origin.AT_START))
                {
                    origin_top_left = true;
                    int x = current.getX();
                    int y = current.getY();
                    if (x < min_x_tl)
                    {
                        min_x_tl = x;
                    }
                    if (y < min_y_tl)
                    {
                        min_y_tl = y;
                    }
                    x += current.getWidth();
                    y += current.getHeight();
                    if (x > max_x_tl)
                    {
                        max_x_tl = x;
                    }
                    if (y > max_y_tl)
                    {
                        max_y_tl = y;
                    }
                }
                else
                {
                    origin_bottom_right = true;
                    int x_2 = current.getX();
                    int y_2 = current.getY();
                    if (x_2 < min_x_br)
                    {
                        min_x_br = x_2;
                    }
                    if (y_2 < min_y_br)
                    {
                        min_y_br = y_2;
                    }
                    x_2 += current.getWidth();
                    y_2 += current.getHeight();
                    if (x_2 > max_x_br)
                    {
                        max_x_br = x_2;
                    }
                    if (y_2 > max_y_br)
                    {
                        max_y_br = y_2;
                    }
                }
                current = ((com.oddlabs.tt.gui.GUIObject)current.getNext());
            }
            int top_left_width = max_x_tl - min_x_tl + left_offset + right_offset;
            int bottom_right_width = max_x_br - min_x_br + left_offset + right_offset;
            int width = java.lang.Math.max(top_left_width, bottom_right_width);
            int height = (max_y_tl - min_y_tl) + (max_y_br - min_y_br) + top_offset + bottom_offset;
            if (origin_top_left && origin_bottom_right)
            {
                height += com.oddlabs.tt.gui.Skin.getSkin().getFormData().sectionSpacing();
            }
            setDim(width, height);
            current = ((com.oddlabs.tt.gui.GUIObject)getFirstChild());
            while (current != null)
            {
                com.oddlabs.tt.gui.GUIObject gui_object = current;
                if (((object)gui_object.getOrigin() == (object)com.oddlabs.tt.gui.Origin.AT_START))
                {
                    gui_object.correctPos(-min_x_tl + left_offset, height - max_y_tl - top_offset);
                }
                else
                {
                    gui_object.correctPos(width - max_x_br - right_offset, -min_y_br + bottom_offset);
                }
                current = ((com.oddlabs.tt.gui.GUIObject)current.getNext());
            }
        }
        // Java: 94
        public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
            if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.PRESSED) || ((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.REPEAT))
            {
                if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_FOCUS_NEXT))
                {
                    switchFocus(com.oddlabs.tt.gui.FocusDirection.FORWARD);
                    return;
                }
                if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_FOCUS_PREV))
                {
                    switchFocus(com.oddlabs.tt.gui.FocusDirection.BACKWARD);
                    return;
                }
            }
            base.handleInput(@event);
        }
        // Java: 109
        public virtual void setGroupFocus(com.oddlabs.tt.gui.FocusDirection dir)
        {
            setFocus(dir);
        }
        // Java: 113
        public override void setFocus(com.oddlabs.tt.gui.FocusDirection direction)
        {
            base.setFocus(direction);
            switchFocus(direction, false);
        }
        public Group() : this(true)
        {
        }
        public Group(bool can_focus) : base()
        {
            setCanFocus(can_focus);
        }
    }
}
