// Translated from PulldownButton.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 7
    public class PulldownButton : com.oddlabs.tt.gui.GUIObject
    {
        public com.oddlabs.tt.gui.PulldownMenu menu;
        public com.oddlabs.tt.gui.Label label;
        public com.oddlabs.tt.gui.GUIRoot gui_root;
        public bool menu_active;
        // Java: 28
        public override com.oddlabs.tt.gui.Renderable setDim(int width, int height)
        {
            base.setDim(width, height);
            com.oddlabs.tt.gui.PulldownData data = com.oddlabs.tt.gui.Skin.getSkin().getPulldownData();
            label.setDim(getWidth() - data.textOffsetLeft() - data.arrowOffsetRight() - data.arrow().quad(com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL).getWidth(), label.getHeight());
            label.setPos(data.textOffsetLeft(), (getHeight() - label.getHeight()) / 2);
            if ((menu is com.oddlabs.tt.gui.ScrollablePulldownMenu scrollable))
            {
                scrollable.setButtonWidth(width);
            }
            if (menu.getWidth() < width)
            {
                menu.setDim(width, menu.getHeight());
            }
            return this;
        }
        // Java: 42
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            com.oddlabs.tt.gui.PulldownData data = com.oddlabs.tt.gui.Skin.getSkin().getPulldownData();
            com.oddlabs.tt.gui.Horizontal pulldownButton = data.pulldownButton();
            com.oddlabs.tt.gui.ModeIconQuads.Mode skinMode = (isDisabled() ? com.oddlabs.tt.gui.ModeIconQuads.Mode.DISABLED : (isActive() ? com.oddlabs.tt.gui.ModeIconQuads.Mode.ACTIVE : com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL));
            pulldownButton.render(renderer, 0, 0, getWidth(), skinMode);
            com.oddlabs.tt.gui.IconQuad arrowQuad = data.arrow().quad(skinMode);
            renderer.drawIcon(arrowQuad, getWidth() - data.arrowOffsetRight() - arrowQuad.getWidth(), 0);
        }
        // Java: 55
        public override void mousePressed(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
            if (menu_active)
            {
                deactivateMenu();
            }
            else
            {
                activateMenu();
            }
        }
        // Java: 64
        public override void mouseEntered()
        {
            menu_active = menu.isActive();
        }
        // Java: 69
        public override void mouseReleased(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
            if (!menu.isActive())
            {
                menu.getItem(menu.getChosenItemIndex()).setFocus();
            }
            menu.clickItem(button, x, y, 1);
        }
        // Java: 76
        public void activateMenu()
        {
            menu_active = true;
            int menu_x = java.lang.JC.f2i(((float)((float)(getRootX() + getWidth()) - menu.getWidth())));
            if ((menu is com.oddlabs.tt.gui.ScrollablePulldownMenu))
            {
                menu_x = java.lang.JC.f2i(getRootX());
            }
            menu.setPos(menu_x, java.lang.JC.f2i(((float)(getRootY() - menu.getHeight()))));
            gui_root.getDelegate().addChild(menu);
        }
        // Java: 87
        public void deactivateMenu()
        {
            menu_active = false;
            setFocus();
            menu.remove();
        }
        // Java: 93
        public com.oddlabs.tt.gui.PulldownMenu getMenu()
        {
            return menu;
        }
        // Java: 97
        public override void doRemove()
        {
            base.doRemove();
            if (!menu.isActive())
            {
                menu.remove();
            }
        }
        // Java: 104
        public void setLabelColor(org.joml.Vector4fc color)
        {
            label.setColor(color);
        }
        // Java: 109
        public void setSelected(int index)
        {
            menu.chooseItemSilently(index);
            applyLabel(menu.getItem(index));
        }
        // Java: 114
        public void applyLabel(com.oddlabs.tt.gui.PulldownItem item)
        {
            label.set(item.getLabelString());
            label.setColor(item.getLabelColor());
        }
        // Java: 119
        public void itemChosen(com.oddlabs.tt.gui.PulldownMenu menu, int item_index)
        {
            applyLabel(menu.getItem(item_index));
            if (menu.isActive())
            {
                deactivateMenu();
            }
        }
        public PulldownButton(com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.gui.PulldownMenu menu, int width) : base()
        {
            this.menu = menu;
            this.gui_root = gui_root;
            setCanFocus(true);
            menu.addItemChosenListener(new com.oddlabs.tt.guievent.ItemChosenListener_L((com.oddlabs.tt.gui.PulldownMenu a0, int a1) => { this.itemChosen(a0, a1); }));
            label = new com.oddlabs.tt.gui.Label("", com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), 0, com.oddlabs.tt.gui.Origin.AT_START);
            addChild(label);
            setDim(width, com.oddlabs.tt.gui.Skin.getSkin().getPulldownData().pulldownButton().getHeight());
        }
        public PulldownButton(com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.gui.PulldownMenu menu, int item_index, int width) : this(gui_root, menu, width)
        {
            menu.chooseItem(item_index);
        }
    }
}
