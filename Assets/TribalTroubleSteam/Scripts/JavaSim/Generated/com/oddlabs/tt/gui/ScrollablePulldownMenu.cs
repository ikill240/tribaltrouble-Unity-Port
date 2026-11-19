// Translated from ScrollablePulldownMenu.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 10
    public class ScrollablePulldownMenu : com.oddlabs.tt.gui.PulldownMenu, com.oddlabs.tt.gui.Scrollable, com.oddlabs.tt.gui.Clipped
    {
        public com.oddlabs.tt.gui.ScrollBar scroll_bar;
        public int offset_y;
        public int render_amount;
        public int normalizedItemHeight;
        public int button_width;
        // Java: 22
        public virtual int getScrollbarWidth()
        {
            return (scroll_bar != null ? scroll_bar.getWidth() : 0);
        }
        // Java: 26
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            com.oddlabs.tt.gui.Horizontal bot = com.oddlabs.tt.gui.Skin.getSkin().getPulldownData().pulldownBottom();
            bot.render(renderer, 0, 0, getWidth() - getScrollbarWidth(), com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL);
            com.oddlabs.tt.gui.Horizontal top = com.oddlabs.tt.gui.Skin.getSkin().getPulldownData().pulldownTop();
            int addAmount = (items.size() > render_amount ? top.getHeight() : 0);
            top.render(renderer, 0, getHeight() - top.getHeight() + addAmount, getWidth() - getScrollbarWidth(), com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL);
        }
        // Java: 37
        public virtual void setButtonWidth(int width)
        {
            this.button_width = width;
        }
        // Java: 41
        public override com.oddlabs.tt.gui.Renderable setDim(int width, int height)
        {
            if (button_width == 0 && width > 0 && items.isEmpty())
            {
                button_width = width;
            }
            if (items.size() <= render_amount)
            {
                base.setDim(width, height);
                return this;
            }
            calculateNormalizedItemHeight();
            com.oddlabs.tt.gui.Box item_box = com.oddlabs.tt.gui.Skin.getSkin().getPulldownData().pulldownItem();
            int text_width = 0;
            foreach (com.oddlabs.tt.gui.PulldownItem item in java.lang.JIter.of(items))
            {
                if (item.getTextWidth() > text_width)
                {
                    text_width = item.getTextWidth();
                }
            }
            int min_content_width = java.lang.Math.max(button_width, item_box.getLeftOffset() + text_width + item_box.getRightOffset());
            int item_pos_count = 0;
            foreach (com.oddlabs.tt.gui.PulldownItem item_2 in java.lang.JIter.of(items))
            {
                item_2.setDim(min_content_width, normalizedItemHeight);
                item_2.setPos(0, item_pos_count);
                item_pos_count -= normalizedItemHeight;
            }
            int item_height_shown = normalizedItemHeight * render_amount;
            if (scroll_bar == null)
            {
                scroll_bar = new com.oddlabs.tt.gui.ScrollBar(item_height_shown, this);
                addChild(scroll_bar);
                offset_y = 0;
            }
            scroll_bar.setDim(scroll_bar.getWidth(), item_height_shown);
            scroll_bar.setPos(min_content_width, 0);
            super_setDim(min_content_width + scroll_bar.getWidth(), item_height_shown);
            updateItemPositions();
            scroll_bar.update();
            return this;
        }
        // Java: 88
        public void calculateNormalizedItemHeight()
        {
            com.oddlabs.tt.gui.Box item_box = com.oddlabs.tt.gui.Skin.getSkin().getPulldownData().pulldownItem();
            normalizedItemHeight = 0;
            foreach (com.oddlabs.tt.gui.PulldownItem item in java.lang.JIter.of(items))
            {
                int h = item_box.getBottomOffset() + item.getTextHeight() + item_box.getTopOffset();
                if (h > normalizedItemHeight)
                {
                    normalizedItemHeight = h;
                }
            }
        }
        // Java: 97
        public void updateItemPositions()
        {
            int item_pos_count = offset_y;
            foreach (com.oddlabs.tt.gui.PulldownItem item in java.lang.JIter.of(items))
            {
                item.setPos(0, item_pos_count + getHeight() - normalizedItemHeight - 2);
                item_pos_count -= normalizedItemHeight;
            }
        }
        // Java: 105
        public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
            if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.PRESSED) && @event.consumeAction(com.oddlabs.tt.input.GameAction.UI_CANCEL))
            {
                remove();
                return;
            }
            base.handleInput(@event);
        }
        // Java: 114
        public override void mouseScrolled(int amount)
        {
            setOffsetY(offset_y + ((amount > 0 ? (-1) : 1)) * getStepHeight() * 3);
        }
        // Java: 119
        public virtual void setOffsetY(int new_offset)
        {
            if (items.size() <= render_amount)
            {
                return;
            }
            int max_offset = java.lang.Math.max(0, getTotalContentHeight() - getVisibleHeight());
            new_offset = java.lang.Math.max(0, java.lang.Math.min(new_offset, max_offset));
            new_offset = (new_offset / normalizedItemHeight) * normalizedItemHeight;
            offset_y = new_offset;
            updateItemPositions();
            if (scroll_bar != null)
            {
                scroll_bar.update();
            }
        }
        // Java: 130
        public virtual int getOffsetY()
        {
            return offset_y;
        }
        // Java: 135
        public virtual int getStepHeight()
        {
            return (normalizedItemHeight > 0 ? normalizedItemHeight : 16);
        }
        // Java: 140
        public virtual void jumpPage(bool up)
        {
            setOffsetY(offset_y + ((up ? -getVisibleHeight() : getVisibleHeight())));
        }
        // Java: 145
        public virtual float getScrollBarRatio()
        {
            int total = getTotalContentHeight();
            int visible = getVisibleHeight();
            if (total <= visible)
            {
                return 1.0f;
            }
            return (float)(((float)(visible)) / total);
        }
        // Java: 153
        public virtual float getScrollBarOffset()
        {
            int max = getTotalContentHeight() - getVisibleHeight();
            if (max <= 0)
            {
                return 0.0f;
            }
            return (float)(((float)(offset_y)) / max);
        }
        // Java: 160
        public virtual void setScrollBarOffset(float offset)
        {
            int max = getTotalContentHeight() - getVisibleHeight();
            if (max > 0)
            {
                setOffsetY(java.lang.JC.f2i(((float)(offset * max))));
            }
        }
        // Java: 166
        public int getTotalContentHeight()
        {
            return items.size() * normalizedItemHeight;
        }
        // Java: 170
        public int getVisibleHeight()
        {
            return render_amount * normalizedItemHeight;
        }
        public ScrollablePulldownMenu(int render_amount) : base()
        {
            this.render_amount = render_amount;
        }
    }
}
