// Translated from PulldownMenu.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 16
    public class PulldownMenu : com.oddlabs.tt.gui.Group
    {
        public java.util.Set chosen_listeners;
        public java.util.List items;
        public int chosen_item_index;
        // Java: 27
        public virtual com.oddlabs.tt.gui.PulldownItem getItem(int index)
        {
            return ((com.oddlabs.tt.gui.PulldownItem)items.get(index));
        }
        // Java: 31
        public virtual int getSize()
        {
            return items.size();
        }
        // Java: 35
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            com.oddlabs.tt.gui.Horizontal bot = com.oddlabs.tt.gui.Skin.getSkin().getPulldownData().pulldownBottom();
            bot.render(renderer, 0, 0, getWidth(), com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL);
            com.oddlabs.tt.gui.Horizontal top = com.oddlabs.tt.gui.Skin.getSkin().getPulldownData().pulldownTop();
            top.render(renderer, 0, getHeight() - top.getHeight(), getWidth(), com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL);
        }
        // Java: 46
        public virtual void addItem(com.oddlabs.tt.gui.PulldownItem item)
        {
            items.add(item);
            addChild(item);
            item.addMouseClickListener(new com.oddlabs.tt.gui.PulldownMenu.ItemListener(this, items.size() - 1));
            setDim(getWidth(), getHeight());
        }
        // Java: 53
        public virtual void clear()
        {
            items.clear();
            clearChildren();
            chosen_item_index = (-1);
            setDim(getWidth(), getHeight());
        }
        // Java: 60
        public override com.oddlabs.tt.gui.Renderable setDim(int width, int height)
        {
            int min_width = 0;
            com.oddlabs.tt.gui.Box item_box = com.oddlabs.tt.gui.Skin.getSkin().getPulldownData().pulldownItem();
            foreach (com.oddlabs.tt.gui.PulldownItem item in java.lang.JIter.of(items))
            {
                if (item.getTextWidth() > min_width)
                {
                    min_width = item.getTextWidth();
                }
            }
            int item_pos_count = com.oddlabs.tt.gui.Skin.getSkin().getPulldownData().pulldownBottom().getHeight();
            min_width = java.lang.Math.max(width, item_box.getLeftOffset() + min_width + item_box.getRightOffset());
            for (int i = 0; i < items.size(); i++)
            {
                com.oddlabs.tt.gui.PulldownItem item_2 = ((com.oddlabs.tt.gui.PulldownItem)items.get(items.size() - 1 - i));
                int item_height = item_box.getBottomOffset() + item_2.getTextHeight() + item_box.getTopOffset();
                item_2.setDim(min_width, item_height);
                item_2.setPos(0, item_pos_count);
                item_pos_count += item_height;
            }
            int min_height = java.lang.Math.max(height, item_pos_count + com.oddlabs.tt.gui.Skin.getSkin().getPulldownData().pulldownTop().getHeight());
            base.setDim(min_width, min_height);
            return this;
        }
        // Java: 82
        public virtual void super_setDim(int width, int height)
        {
            base.setDim(width, height);
        }
        // Java: 86
        public virtual int getChosenItemIndex()
        {
            return chosen_item_index;
        }
        // Java: 90
        public virtual void chooseItem(int index)
        {
            chosen_item_index = index;
            itemChosenAll();
        }
        // Java: 97
        public virtual void chooseItemSilently(int index)
        {
            chosen_item_index = index;
        }
        // Java: 101
        public override void focusNotify(bool focus)
        {
            if (!focus)
            {
                remove();
            }
        }
        // Java: 108
        public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
            if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.PRESSED) || ((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.REPEAT))
            {
                if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_NAV_UP))
                {
                    focusPrior();
                    @event.consume();
                    return;
                }
                if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_NAV_DOWN))
                {
                    focusNext();
                    @event.consume();
                    return;
                }
            }
            base.handleInput(@event);
        }
        // Java: 126
        public virtual void clickItem(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
        {
            foreach (com.oddlabs.tt.gui.PulldownItem item in java.lang.JIter.of(items))
            {
                if (item.isHovered())
                {
                    item.mouseClickedAll(button, x, y, clicks);
                }
            }
        }
        // Java: 133
        public virtual void itemChosenAll()
        {
            foreach (com.oddlabs.tt.guievent.ItemChosenListener listener in java.lang.JIter.of(chosen_listeners))
            {
                listener.itemChosen(this, chosen_item_index);
            }
        }
        // Java: 139
        public virtual void addItemChosenListener(com.oddlabs.tt.guievent.ItemChosenListener listener)
        {
            chosen_listeners.add(listener);
        }
        // Java: 143
        public virtual void removeItemChosenListener(com.oddlabs.tt.guievent.ItemChosenListener listener)
        {
            chosen_listeners.remove(listener);
        }
        // Java: 147
        public class ItemListener : java.lang.Object, com.oddlabs.tt.guievent.MouseClickListener
        {
            public com.oddlabs.tt.gui.PulldownMenu this_0;
            public int index;
            // Java: 154
            public void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
            {
                this_0.chooseItem(index);
            }
            public ItemListener(com.oddlabs.tt.gui.PulldownMenu this_0_, int index) : base()
            {
                this.this_0 = this_0_;
                this.index = index;
            }
        }
        private void init_PulldownMenu()
        {
            this.chosen_listeners = new java.util.concurrent.CopyOnWriteArraySet();
            this.items = new java.util.ArrayList();
            this.chosen_item_index = (-1);
        }
        public PulldownMenu() : base()
        {
            init_PulldownMenu();
            setCanFocus(true);
            setFocusCycle(true);
        }
    }
}
