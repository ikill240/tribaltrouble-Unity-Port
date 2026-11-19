// Translated from ScrollableGroup.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 8
    public class ScrollableGroup : com.oddlabs.tt.gui.Group, com.oddlabs.tt.gui.Scrollable
    {
        public com.oddlabs.tt.gui.ScrollBar scroll_bar;
        public int content_height;
        public int total_content_height;
        public int scroll_bar_left_margin;
        public int offset_y;
        // Java: 25
        public override void compileCanvas()
        {
            if (scroll_bar != null)
            {
                removeChild(scroll_bar);
            }
            base.compileCanvas(0, 0, 0, 0);
            total_content_height = getHeight();
            if (scroll_bar != null)
            {
                addChild(scroll_bar);
            }
            int scrollbar_width = (scroll_bar != null ? scroll_bar.getWidth() : 0);
            if (total_content_height < content_height)
            {
                int remainingHeight = total_content_height % content_height;
                if (scroll_bar != null)
                {
                    removeChild(scroll_bar);
                }
                com.oddlabs.tt.gui.GUIObject current = ((com.oddlabs.tt.gui.GUIObject)getFirstChild());
                while (current != null)
                {
                    if (((object)current != (object)scroll_bar))
                    {
                        current.setPos(current.getX(), current.getY() + content_height - remainingHeight);
                    }
                    current = ((com.oddlabs.tt.gui.GUIObject)current.getNext());
                }
                scroll_bar = null;
                setDim(getWidth() + scrollbar_width + scroll_bar_left_margin, content_height);
                return;
            }
            if (scroll_bar != null)
            {
                scroll_bar.update();
                setDim(getWidth() + scroll_bar.getWidth() + scroll_bar_left_margin, content_height);
                scroll_bar.setPos(getWidth() - scroll_bar.getWidth(), 0);
                scroll_bar.update();
            }
            com.oddlabs.tt.gui.GUIObject current_2 = ((com.oddlabs.tt.gui.GUIObject)getFirstChild());
            while (current_2 != null)
            {
                if (((object)current_2 != (object)scroll_bar))
                {
                    int offset = java.lang.Math.max(0, total_content_height - content_height);
                    current_2.setPos(current_2.getX(), current_2.getY() - offset);
                }
                current_2 = ((com.oddlabs.tt.gui.GUIObject)current_2.getNext());
            }
        }
        // Java: 70
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            renderer.flush();
            org.lwjgl.opengl.GL11.glEnable(org.lwjgl.opengl.GL11.GL_SCISSOR_TEST);
            float scale = getGlobalScale();
            org.lwjgl.opengl.GL11.glScissor(java.lang.JC.f2i(((float)(getRootX() * scale))), java.lang.JC.f2i(((float)(getRootY() * scale))), java.lang.JC.f2i(((float)(getWidth() * scale))), java.lang.JC.f2i(((float)(content_height * scale))));
        }
        // Java: 80
        public override void postRender(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            renderer.flush();
            org.lwjgl.opengl.GL11.glDisable(org.lwjgl.opengl.GL11.GL_SCISSOR_TEST);
        }
        // Java: 86
        public virtual void setContentHeight(int height)
        {
            content_height = height;
        }
        // Java: 90
        public virtual int getContentHeight()
        {
            return content_height;
        }
        // Java: 94
        public int getOffsetY()
        {
            return offset_y;
        }
        // Java: 99
        public int getStepHeight()
        {
            return 28;
        }
        // Java: 104
        public void jumpPage(bool up)
        {
            setOffsetY(offset_y + ((up ? -getVisibleHeight() : getVisibleHeight())));
        }
        // Java: 109
        public override void mouseScrolled(int amount)
        {
            setOffsetY(offset_y + ((amount > 0 ? (-1) : 1)) * getStepHeight() * 3);
        }
        // Java: 114
        public float getScrollBarRatio()
        {
            int totalHeight = getTotalContentHeight();
            int visibleHeight = getVisibleHeight();
            if (totalHeight <= visibleHeight)
            {
                return 1.0f;
            }
            return (float)(visibleHeight / ((float)(totalHeight)));
        }
        // Java: 122
        public float getScrollBarOffset()
        {
            int maxOffset = getTotalContentHeight() - getVisibleHeight();
            if (maxOffset <= 0)
            {
                return 0.0f;
            }
            return (float)(offset_y / ((float)(maxOffset)));
        }
        // Java: 129
        public void setScrollBarOffset(float offset)
        {
            int maxOffset = getTotalContentHeight() - getVisibleHeight();
            if (maxOffset > 0)
            {
                setOffsetY(java.lang.JC.f2i(((float)(offset * maxOffset))));
            }
            else
            {
                setOffsetY(0);
            }
        }
        // Java: 139
        public void setOffsetY(int new_offset)
        {
            if (scroll_bar == null)
            {
                return;
            }
            int diff = new_offset - offset_y;
            if (offset_y + diff <= 0)
            {
                diff = -offset_y;
                offset_y = 0;
            }
            int max_offset_y = getTotalContentHeight() - getVisibleHeight();
            if (offset_y + diff > max_offset_y)
            {
                diff = max_offset_y - offset_y;
            }
            offset_y = offset_y + diff;
            if (offset_y < 0)
            {
                offset_y = 0;
            }
            if (offset_y > max_offset_y)
            {
                offset_y = max_offset_y;
            }
            com.oddlabs.tt.gui.GUIObject current = ((com.oddlabs.tt.gui.GUIObject)getFirstChild());
            while (current != null)
            {
                if (((object)current != (object)scroll_bar))
                {
                    current.setPos(current.getX(), current.getY() + diff);
                }
                current = ((com.oddlabs.tt.gui.GUIObject)current.getNext());
            }
            scroll_bar.update();
        }
        // Java: 168
        public int getTotalContentHeight()
        {
            return total_content_height;
        }
        // Java: 172
        public int getVisibleHeight()
        {
            return content_height;
        }
        public ScrollableGroup(int content_height, int scroll_bar_left_margin) : base()
        {
            setCanFocus(true);
            this.content_height = content_height;
            this.scroll_bar_left_margin = scroll_bar_left_margin;
            scroll_bar = new com.oddlabs.tt.gui.ScrollBar(content_height, this);
            addChild(scroll_bar);
            scroll_bar.setCanFocus(true);
            scroll_bar.place();
        }
    }
}
