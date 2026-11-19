// Translated from TextBox.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 10
    public class TextBox : com.oddlabs.tt.gui.TextField, com.oddlabs.tt.gui.Scrollable, com.oddlabs.tt.gui.Clipped
    {
        public com.oddlabs.tt.font.TextLayout textLayout;
        public com.oddlabs.tt.gui.ScrollBar scroll_bar;
        public int offset_y;
        // Java: 30
        public void updateLayout()
        {
            com.oddlabs.tt.gui.Box edit_box = com.oddlabs.tt.gui.Skin.getSkin().getEditBox();
            int wrapWidth = getWidth() - edit_box.getLeftOffset() - edit_box.getRightOffset() - scroll_bar.getWidth();
            textLayout = new com.oddlabs.tt.font.TextLayout(getFont(), getText(), wrapWidth);
            scroll_bar.update();
        }
        // Java: 37
        public override com.oddlabs.tt.gui.TextField setText(string text)
        {
            base.setText(text);
            updateLayout();
            return this;
        }
        // Java: 44
        public override void append(string str)
        {
            base.append(str);
            updateLayout();
        }
        // Java: 50
        public com.oddlabs.tt.font.TextLayout getTextLayout()
        {
            return textLayout;
        }
        // Java: 54
        public void renderBox(com.oddlabs.tt.render.GUIRenderer renderer, com.oddlabs.tt.gui.ModeIconQuads.Mode skinMode)
        {
            com.oddlabs.tt.gui.Box edit_box = com.oddlabs.tt.gui.Skin.getSkin().getEditBox();
            edit_box.render(renderer, 0.0f, 0.0f, getWidth() - scroll_bar.getWidth(), getHeight(), skinMode);
        }
        // Java: 59
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            com.oddlabs.tt.gui.Box edit_box = com.oddlabs.tt.gui.Skin.getSkin().getEditBox();
            renderBox(renderer, com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL);
            com.oddlabs.tt.font.TextLineRenderer.render(renderer, textLayout, edit_box.getLeftOffset(), getHeight() - edit_box.getBottomOffset() - getFont().getHeight() + offset_y, edit_box.getLeftOffset(), getWidth() - edit_box.getRightOffset(), com.oddlabs.util.Color.WHITE);
        }
        // Java: 68
        public override void mouseScrolled(int amount)
        {
            setOffsetY(offset_y + ((amount > 0 ? (-3) : 3)) * getFont().getHeight());
        }
        // Java: 73
        public void setOffsetY(int new_offset)
        {
            offset_y = java.lang.Math.max(new_offset, 0);
            com.oddlabs.tt.gui.Box edit_box = com.oddlabs.tt.gui.Skin.getSkin().getEditBox();
            int max_offset_y = java.lang.Math.max(0, textLayout.getTextHeight() - (getHeight() - edit_box.getBottomOffset() - edit_box.getTopOffset()));
            offset_y = java.lang.Math.min(offset_y, max_offset_y);
            scroll_bar.update();
        }
        // Java: 84
        public int getOffsetY()
        {
            return offset_y;
        }
        // Java: 89
        public int getStepHeight()
        {
            return getFont().getHeight();
        }
        // Java: 94
        public void jumpPage(bool up)
        {
            com.oddlabs.tt.gui.Box edit_box = com.oddlabs.tt.gui.Skin.getSkin().getEditBox();
            int inner_height = getHeight() - edit_box.getBottomOffset() - edit_box.getTopOffset();
            setOffsetY(offset_y + ((up ? -inner_height : inner_height)));
        }
        // Java: 101
        public float getScrollBarRatio()
        {
            int text_height = textLayout.getTextHeight();
            if (text_height <= 0)
            {
                return 1.0f;
            }
            com.oddlabs.tt.gui.Box edit_box = com.oddlabs.tt.gui.Skin.getSkin().getEditBox();
            int inner_height = getHeight() - edit_box.getBottomOffset() - edit_box.getTopOffset();
            return java.lang.Math.min(1.0f, (float)(((float)(inner_height)) / text_height));
        }
        // Java: 110
        public float getScrollBarOffset()
        {
            int text_height = textLayout.getTextHeight();
            com.oddlabs.tt.gui.Box edit_box = com.oddlabs.tt.gui.Skin.getSkin().getEditBox();
            int inner_height = getHeight() - edit_box.getBottomOffset() - edit_box.getTopOffset();
            int max_offset = text_height - inner_height;
            return (max_offset <= 0 ? 0 : (float)(((float)(offset_y)) / max_offset));
        }
        // Java: 119
        public void setScrollBarOffset(float offset)
        {
            int text_height = textLayout.getTextHeight();
            com.oddlabs.tt.gui.Box edit_box = com.oddlabs.tt.gui.Skin.getSkin().getEditBox();
            int inner_height = getHeight() - edit_box.getBottomOffset() - edit_box.getTopOffset();
            int max_offset = text_height - inner_height;
            if (max_offset <= 0)
            {
                return;
            }
            setOffsetY(java.lang.JC.f2i(((float)(offset * max_offset))));
        }
        public TextBox(int width, int height, com.oddlabs.tt.font.Font font, int max_chars) : base(font, max_chars)
        {
            setDim(width, height);
            setCanFocus(true);
            offset_y = 0;
            textLayout = new com.oddlabs.tt.font.TextLayout(font, getText(), width);
            scroll_bar = new com.oddlabs.tt.gui.ScrollBar(height, this);
            updateLayout();
            scroll_bar.setPos(width - scroll_bar.getWidth(), 0);
            addChild(scroll_bar);
        }
    }
}
