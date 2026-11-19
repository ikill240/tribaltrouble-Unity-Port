// Translated from Skin.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 20
    public class Skin : java.lang.Object
    {
        public static com.oddlabs.tt.gui.Skin SKIN;
        public com.oddlabs.tt.font.Font edit_font;
        public com.oddlabs.tt.font.Font button_font;
        public com.oddlabs.tt.font.Font headline_font;
        public com.oddlabs.tt.gui.ModeIconQuads plus_button;
        public com.oddlabs.tt.gui.ModeIconQuads minus_button;
        public com.oddlabs.tt.gui.ModeIconQuads accept_button;
        public com.oddlabs.tt.gui.ModeIconQuads cancel_button;
        public com.oddlabs.tt.gui.ModeIconQuads back_button;
        public com.oddlabs.tt.gui.Horizontal horiz_button_pressed;
        public com.oddlabs.tt.gui.Horizontal horiz_button_unpressed;
        public com.oddlabs.tt.gui.FormData form_data;
        public com.oddlabs.tt.gui.Box edit_box;
        public com.oddlabs.tt.gui.Box background_box;
        public com.oddlabs.tt.gui.ModeIconQuads check_box_marked;
        public com.oddlabs.tt.gui.ModeIconQuads check_box_unmarked;
        public com.oddlabs.tt.gui.ModeIconQuads radio_button_marked;
        public com.oddlabs.tt.gui.ModeIconQuads radio_button_unmarked;
        public com.oddlabs.tt.gui.GroupData group_data;
        public com.oddlabs.tt.gui.ScrollBarData scroll_bar_data;
        public com.oddlabs.tt.gui.SliderData slider_data;
        public com.oddlabs.tt.gui.PulldownData pulldown_data;
        public com.oddlabs.tt.gui.ProgressBarData progress_bar_data;
        public com.oddlabs.tt.gui.MultiColumnComboBoxData multi_columnCombo_box_data;
        public com.oddlabs.tt.gui.ToolTipBoxInfo tool_tip;
        public com.oddlabs.tt.gui.ModeIconQuads diode;
        public com.oddlabs.tt.gui.PanelData panel_data;
        public com.oddlabs.tt.gui.IconQuad flag_default;
        public com.oddlabs.tt.gui.IconQuad flag_da;
        public com.oddlabs.tt.gui.IconQuad flag_en;
        public com.oddlabs.tt.gui.IconQuad flag_de;
        public com.oddlabs.tt.gui.IconQuad flag_es;
        public com.oddlabs.tt.gui.IconQuad flag_it;
        public com.oddlabs.tt.gui.IconQuad flag_pt;
        // Java: 59
        public static com.oddlabs.tt.gui.Skin getSkin()
        {
            return com.oddlabs.tt.gui.Skin.SKIN;
        }
        // Java: 104
        public com.oddlabs.tt.gui.Horizontal getHorizontal(org.w3c.dom.Node n, com.oddlabs.tt.render.Texture texture)
        {
            org.w3c.dom.Node horizontal_node = com.oddlabs.tt.gui.Icons.getNodeByName("horizontal", n);
            return new com.oddlabs.tt.gui.Horizontal(com.oddlabs.tt.gui.Icons.getIconQuads(com.oddlabs.tt.gui.Icons.getNodeByName("left", horizontal_node), texture), com.oddlabs.tt.gui.Icons.getIconQuads(com.oddlabs.tt.gui.Icons.getNodeByName("center", horizontal_node), texture), com.oddlabs.tt.gui.Icons.getIconQuads(com.oddlabs.tt.gui.Icons.getNodeByName("right", horizontal_node), texture));
        }
        // Java: 113
        public com.oddlabs.tt.gui.Vertical getVertical(org.w3c.dom.Node n, com.oddlabs.tt.render.Texture texture)
        {
            org.w3c.dom.Node vertical_node = com.oddlabs.tt.gui.Icons.getNodeByName("vertical", n);
            return new com.oddlabs.tt.gui.Vertical(com.oddlabs.tt.gui.Icons.getIconQuads(com.oddlabs.tt.gui.Icons.getNodeByName("bottom", vertical_node), texture), com.oddlabs.tt.gui.Icons.getIconQuads(com.oddlabs.tt.gui.Icons.getNodeByName("center", vertical_node), texture), com.oddlabs.tt.gui.Icons.getIconQuads(com.oddlabs.tt.gui.Icons.getNodeByName("top", vertical_node), texture));
        }
        // Java: 122
        public com.oddlabs.tt.gui.Box getBox(org.w3c.dom.Node n, com.oddlabs.tt.render.Texture texture)
        {
            org.w3c.dom.Node box_node = com.oddlabs.tt.gui.Icons.getNodeByName("box", n);
            com.oddlabs.tt.gui.ModeIconQuads left_bottom = com.oddlabs.tt.gui.Icons.getIconQuads(com.oddlabs.tt.gui.Icons.getNodeByName("left_bottom", box_node), texture);
            com.oddlabs.tt.gui.ModeIconQuads bottom = com.oddlabs.tt.gui.Icons.getIconQuads(com.oddlabs.tt.gui.Icons.getNodeByName("bottom", box_node), texture);
            com.oddlabs.tt.gui.ModeIconQuads right_bottom = com.oddlabs.tt.gui.Icons.getIconQuads(com.oddlabs.tt.gui.Icons.getNodeByName("right_bottom", box_node), texture);
            com.oddlabs.tt.gui.ModeIconQuads right = com.oddlabs.tt.gui.Icons.getIconQuads(com.oddlabs.tt.gui.Icons.getNodeByName("right", box_node), texture);
            com.oddlabs.tt.gui.ModeIconQuads right_top = com.oddlabs.tt.gui.Icons.getIconQuads(com.oddlabs.tt.gui.Icons.getNodeByName("right_top", box_node), texture);
            com.oddlabs.tt.gui.ModeIconQuads top = com.oddlabs.tt.gui.Icons.getIconQuads(com.oddlabs.tt.gui.Icons.getNodeByName("top", box_node), texture);
            com.oddlabs.tt.gui.ModeIconQuads left_top = com.oddlabs.tt.gui.Icons.getIconQuads(com.oddlabs.tt.gui.Icons.getNodeByName("left_top", box_node), texture);
            com.oddlabs.tt.gui.ModeIconQuads left = com.oddlabs.tt.gui.Icons.getIconQuads(com.oddlabs.tt.gui.Icons.getNodeByName("left", box_node), texture);
            com.oddlabs.tt.gui.ModeIconQuads center = com.oddlabs.tt.gui.Icons.getIconQuads(com.oddlabs.tt.gui.Icons.getNodeByName("center", box_node), texture);
            int left_offset = com.oddlabs.tt.gui.Icons.getInt(box_node, "left_offset");
            int bottom_offset = com.oddlabs.tt.gui.Icons.getInt(box_node, "bottom_offset");
            int right_offset = com.oddlabs.tt.gui.Icons.getInt(box_node, "right_offset");
            int top_offset = com.oddlabs.tt.gui.Icons.getInt(box_node, "top_offset");
            return new com.oddlabs.tt.gui.Box(left_bottom, bottom, right_bottom, right, right_top, top, left_top, left, center, left_offset, bottom_offset, right_offset, top_offset);
        }
        public const string TAHOMA_FONT = "/font/tahoma_13.font";
        public const string IMPACT_FONT = "/font/impact_24.font";
        public const string FALLBACK_EDIT_FONT = "/font/inter-light_13.font";
        public const string FALLBACK_HEADLINE_FONT = "/font/intertight-black_28.font";
        // Java: 156
        public com.oddlabs.tt.font.Font getFont(org.w3c.dom.Node n)
        {
            string path = n.getFirstChild().getNodeValue();
            if (this.GetType().getResource(path) == null)
            {
                if (java.lang.JO.equals(path, com.oddlabs.tt.gui.Skin.IMPACT_FONT))
                {
                    path = com.oddlabs.tt.gui.Skin.FALLBACK_HEADLINE_FONT;
                }
                else if (java.lang.JO.equals(path, com.oddlabs.tt.gui.Skin.TAHOMA_FONT))
                {
                    path = com.oddlabs.tt.gui.Skin.FALLBACK_EDIT_FONT;
                }
            }
            com.oddlabs.tt.resource.FontFile font_file = new com.oddlabs.tt.resource.FontFile(path);
            return ((com.oddlabs.tt.font.Font)com.oddlabs.tt.resource.Resources.findResource(font_file));
        }
        // Java: 169
        public com.oddlabs.tt.font.Font parseEditFont(org.w3c.dom.Node n)
        {
            org.w3c.dom.Node node = com.oddlabs.tt.gui.Icons.getNodeByName("editfont", n);
            return getFont(node);
        }
        // Java: 174
        public com.oddlabs.tt.font.Font getEditFont()
        {
            return edit_font;
        }
        // Java: 178
        public com.oddlabs.tt.font.Font parseButtonFont(org.w3c.dom.Node n)
        {
            org.w3c.dom.Node node = com.oddlabs.tt.gui.Icons.getNodeByName("buttonfont", n);
            return getFont(node);
        }
        // Java: 183
        public com.oddlabs.tt.font.Font getButtonFont()
        {
            return button_font;
        }
        // Java: 187
        public com.oddlabs.tt.font.Font parseHeadlineFont(org.w3c.dom.Node n)
        {
            org.w3c.dom.Node node = com.oddlabs.tt.gui.Icons.getNodeByName("headlinefont", n);
            return getFont(node);
        }
        // Java: 192
        public com.oddlabs.tt.font.Font getHeadlineFont()
        {
            return headline_font;
        }
        // Java: 196
        public com.oddlabs.tt.gui.ModeIconQuads parseCheckBoxMarked(org.w3c.dom.Node n, com.oddlabs.tt.render.Texture texture)
        {
            org.w3c.dom.Node node = com.oddlabs.tt.gui.Icons.getNodeByName("checkbox", n);
            node = com.oddlabs.tt.gui.Icons.getNodeByName("marked", node);
            return com.oddlabs.tt.gui.Icons.getIconQuads(node, texture);
        }
        // Java: 202
        public com.oddlabs.tt.gui.ModeIconQuads getCheckBoxMarked()
        {
            return check_box_marked;
        }
        // Java: 206
        public com.oddlabs.tt.gui.ModeIconQuads parseCheckBoxUnmarked(org.w3c.dom.Node n, com.oddlabs.tt.render.Texture texture)
        {
            org.w3c.dom.Node node = com.oddlabs.tt.gui.Icons.getNodeByName("checkbox", n);
            node = com.oddlabs.tt.gui.Icons.getNodeByName("unmarked", node);
            return com.oddlabs.tt.gui.Icons.getIconQuads(node, texture);
        }
        // Java: 212
        public com.oddlabs.tt.gui.ModeIconQuads getCheckBoxUnmarked()
        {
            return check_box_unmarked;
        }
        // Java: 216
        public com.oddlabs.tt.gui.ModeIconQuads parseRadioButtonMarked(org.w3c.dom.Node n, com.oddlabs.tt.render.Texture texture)
        {
            org.w3c.dom.Node node = com.oddlabs.tt.gui.Icons.getNodeByName("radiobutton", n);
            node = com.oddlabs.tt.gui.Icons.getNodeByName("marked", node);
            return com.oddlabs.tt.gui.Icons.getIconQuads(node, texture);
        }
        // Java: 222
        public com.oddlabs.tt.gui.ModeIconQuads getRadioButtonMarked()
        {
            return radio_button_marked;
        }
        // Java: 226
        public com.oddlabs.tt.gui.ModeIconQuads parseRadioButtonUnmarked(org.w3c.dom.Node n, com.oddlabs.tt.render.Texture texture)
        {
            org.w3c.dom.Node node = com.oddlabs.tt.gui.Icons.getNodeByName("radiobutton", n);
            node = com.oddlabs.tt.gui.Icons.getNodeByName("unmarked", node);
            return com.oddlabs.tt.gui.Icons.getIconQuads(node, texture);
        }
        // Java: 232
        public com.oddlabs.tt.gui.ModeIconQuads getRadioButtonUnmarked()
        {
            return radio_button_unmarked;
        }
        // Java: 236
        public com.oddlabs.tt.gui.Horizontal parseHorizButtonPressed(org.w3c.dom.Node n, com.oddlabs.tt.render.Texture texture)
        {
            org.w3c.dom.Node node = com.oddlabs.tt.gui.Icons.getNodeByName("horiz_button", n);
            node = com.oddlabs.tt.gui.Icons.getNodeByName("horiz_pressed", node);
            return getHorizontal(node, texture);
        }
        // Java: 242
        public com.oddlabs.tt.gui.Horizontal getHorizButtonPressed()
        {
            return horiz_button_pressed;
        }
        // Java: 246
        public com.oddlabs.tt.gui.Horizontal parseHorizButtonUnpressed(org.w3c.dom.Node n, com.oddlabs.tt.render.Texture texture)
        {
            org.w3c.dom.Node node = com.oddlabs.tt.gui.Icons.getNodeByName("horiz_button", n);
            node = com.oddlabs.tt.gui.Icons.getNodeByName("horiz_unpressed", node);
            return getHorizontal(node, texture);
        }
        // Java: 252
        public com.oddlabs.tt.gui.Horizontal getHorizButtonUnpressed()
        {
            return horiz_button_unpressed;
        }
        // Java: 256
        public com.oddlabs.tt.gui.ScrollBarData parseScrollBarData(org.w3c.dom.Node n, com.oddlabs.tt.render.Texture texture)
        {
            org.w3c.dom.Node node = com.oddlabs.tt.gui.Icons.getNodeByName("vert_scroll", n);
            com.oddlabs.tt.gui.Vertical scroll_bar = getVertical(node, texture);
            org.w3c.dom.Node temp = default;
            temp = com.oddlabs.tt.gui.Icons.getNodeByName("less", node);
            temp = com.oddlabs.tt.gui.Icons.getNodeByName("pushbutton", temp);
            temp = com.oddlabs.tt.gui.Icons.getNodeByName("pressed", temp);
            com.oddlabs.tt.gui.ModeIconQuads scroll_down_button_pressed = com.oddlabs.tt.gui.Icons.getIconQuads(temp, texture);
            temp = com.oddlabs.tt.gui.Icons.getNodeByName("less", node);
            temp = com.oddlabs.tt.gui.Icons.getNodeByName("pushbutton", temp);
            temp = com.oddlabs.tt.gui.Icons.getNodeByName("unpressed", temp);
            com.oddlabs.tt.gui.ModeIconQuads scroll_down_button_unpressed = com.oddlabs.tt.gui.Icons.getIconQuads(temp, texture);
            temp = com.oddlabs.tt.gui.Icons.getNodeByName("less", node);
            com.oddlabs.tt.gui.ModeIconQuads scroll_down_arrow = com.oddlabs.tt.gui.Icons.getIconQuads(temp, texture);
            temp = com.oddlabs.tt.gui.Icons.getNodeByName("more", node);
            temp = com.oddlabs.tt.gui.Icons.getNodeByName("pushbutton", temp);
            temp = com.oddlabs.tt.gui.Icons.getNodeByName("pressed", temp);
            com.oddlabs.tt.gui.ModeIconQuads scroll_up_button_pressed = com.oddlabs.tt.gui.Icons.getIconQuads(temp, texture);
            temp = com.oddlabs.tt.gui.Icons.getNodeByName("more", node);
            temp = com.oddlabs.tt.gui.Icons.getNodeByName("pushbutton", temp);
            temp = com.oddlabs.tt.gui.Icons.getNodeByName("unpressed", temp);
            com.oddlabs.tt.gui.ModeIconQuads scroll_up_button_unpressed = com.oddlabs.tt.gui.Icons.getIconQuads(temp, texture);
            temp = com.oddlabs.tt.gui.Icons.getNodeByName("more", node);
            com.oddlabs.tt.gui.ModeIconQuads scroll_up_arrow = com.oddlabs.tt.gui.Icons.getIconQuads(temp, texture);
            temp = com.oddlabs.tt.gui.Icons.getNodeByName("vert_scroll_button", n);
            com.oddlabs.tt.gui.Vertical scroll_button = getVertical(temp, texture);
            return new com.oddlabs.tt.gui.ScrollBarData(scroll_bar, scroll_down_button_pressed, scroll_down_button_unpressed, scroll_down_arrow, scroll_up_button_pressed, scroll_up_button_unpressed, scroll_up_arrow, scroll_button, com.oddlabs.tt.gui.Icons.getInt(node, "left_offset"), com.oddlabs.tt.gui.Icons.getInt(node, "bottom_offset"), com.oddlabs.tt.gui.Icons.getInt(node, "top_offset"));
        }
        // Java: 303
        public com.oddlabs.tt.gui.ScrollBarData getScrollBarData()
        {
            return scroll_bar_data;
        }
        // Java: 307
        public com.oddlabs.tt.gui.SliderData parseSliderData(org.w3c.dom.Node n, com.oddlabs.tt.render.Texture texture)
        {
            org.w3c.dom.Node node = com.oddlabs.tt.gui.Icons.getNodeByName("slider", n);
            com.oddlabs.tt.gui.Horizontal slider = getHorizontal(node, texture);
            com.oddlabs.tt.gui.ModeIconQuads button = com.oddlabs.tt.gui.Icons.getIconQuads(node, texture);
            return new com.oddlabs.tt.gui.SliderData(slider, button, com.oddlabs.tt.gui.Icons.getInt(node, "left_offset"), com.oddlabs.tt.gui.Icons.getInt(node, "right_offset"));
        }
        // Java: 319
        public com.oddlabs.tt.gui.SliderData getSliderData()
        {
            return slider_data;
        }
        // Java: 323
        public com.oddlabs.tt.gui.PulldownData parsePulldownData(org.w3c.dom.Node n, com.oddlabs.tt.render.Texture texture)
        {
            org.w3c.dom.Node node = com.oddlabs.tt.gui.Icons.getNodeByName("pulldown_menu", n);
            org.w3c.dom.Node temp = default;
            temp = com.oddlabs.tt.gui.Icons.getNodeByName("pulldown_top", node);
            com.oddlabs.tt.gui.Horizontal pulldownTop = getHorizontal(temp, texture);
            temp = com.oddlabs.tt.gui.Icons.getNodeByName("pulldown_bottom", node);
            com.oddlabs.tt.gui.Horizontal pulldownBottom = getHorizontal(temp, texture);
            org.w3c.dom.Node item_node = com.oddlabs.tt.gui.Icons.getNodeByName("pulldown_item", n);
            com.oddlabs.tt.gui.Box pulldownItem = getBox(item_node, texture);
            org.w3c.dom.Node button_node = com.oddlabs.tt.gui.Icons.getNodeByName("pulldown_button", n);
            com.oddlabs.tt.gui.Horizontal pulldown_button = getHorizontal(button_node, texture);
            com.oddlabs.tt.gui.ModeIconQuads arrow = com.oddlabs.tt.gui.Icons.getIconQuads(button_node, texture);
            return new com.oddlabs.tt.gui.PulldownData(pulldownTop, pulldownBottom, pulldownItem, pulldown_button, arrow, com.oddlabs.tt.gui.Icons.getInt(button_node, "arrow_offset_right"), com.oddlabs.tt.gui.Icons.getInt(button_node, "text_offset_left"), getFont(com.oddlabs.tt.gui.Icons.getNodeByName("pulldownfont", n)));
        }
        // Java: 351
        public com.oddlabs.tt.gui.PulldownData getPulldownData()
        {
            return pulldown_data;
        }
        // Java: 355
        public com.oddlabs.tt.gui.ProgressBarData parseProgressBarData(org.w3c.dom.Node n, com.oddlabs.tt.render.Texture texture)
        {
            org.w3c.dom.Node node = com.oddlabs.tt.gui.Icons.getNodeByName("progressbar", n);
            com.oddlabs.tt.gui.Horizontal progressbar = getHorizontal(node, texture);
            org.w3c.dom.Node temp = default;
            temp = com.oddlabs.tt.gui.Icons.getNodeByName("left", node);
            com.oddlabs.tt.gui.ModeIconQuads left = com.oddlabs.tt.gui.Icons.getIconQuads(temp, texture);
            temp = com.oddlabs.tt.gui.Icons.getNodeByName("center", node);
            com.oddlabs.tt.gui.ModeIconQuads center = com.oddlabs.tt.gui.Icons.getIconQuads(temp, texture);
            temp = com.oddlabs.tt.gui.Icons.getNodeByName("right", node);
            com.oddlabs.tt.gui.ModeIconQuads right = com.oddlabs.tt.gui.Icons.getIconQuads(temp, texture);
            return new com.oddlabs.tt.gui.ProgressBarData(progressbar, left, center, right, getFont(com.oddlabs.tt.gui.Icons.getNodeByName("progressfont", n)));
        }
        // Java: 376
        public com.oddlabs.tt.gui.ProgressBarData getProgressBarData()
        {
            return progress_bar_data;
        }
        // Java: 380
        public com.oddlabs.tt.gui.FormData parseFormData(org.w3c.dom.Node n, com.oddlabs.tt.render.Texture texture)
        {
            org.w3c.dom.Node node = com.oddlabs.tt.gui.Icons.getNodeByName("slim_form", n);
            com.oddlabs.tt.gui.Box slim_form = getBox(node, texture);
            node = com.oddlabs.tt.gui.Icons.getNodeByName("form", n);
            return new com.oddlabs.tt.gui.FormData(getBox(node, texture), slim_form, com.oddlabs.tt.gui.Icons.getIconQuads(node, texture), com.oddlabs.tt.gui.Icons.getInt(node, "spacing"), com.oddlabs.tt.gui.Icons.getInt(node, "section_spacing"), com.oddlabs.tt.gui.Icons.getInt(node, "caption_left"), com.oddlabs.tt.gui.Icons.getInt(node, "caption_y"), com.oddlabs.tt.gui.Icons.getInt(node, "close_right"), com.oddlabs.tt.gui.Icons.getInt(node, "close_top"), getFont(com.oddlabs.tt.gui.Icons.getNodeByName("formfont", n)));
        }
        // Java: 397
        public com.oddlabs.tt.gui.FormData getFormData()
        {
            return form_data;
        }
        // Java: 401
        public com.oddlabs.tt.gui.ModeIconQuads getPlusButton()
        {
            return plus_button;
        }
        // Java: 405
        public com.oddlabs.tt.gui.ModeIconQuads getMinusButton()
        {
            return minus_button;
        }
        // Java: 409
        public com.oddlabs.tt.gui.ModeIconQuads getAcceptButton()
        {
            return accept_button;
        }
        // Java: 413
        public com.oddlabs.tt.gui.ModeIconQuads getCancelButton()
        {
            return cancel_button;
        }
        // Java: 417
        public com.oddlabs.tt.gui.ModeIconQuads getBackButton()
        {
            return back_button;
        }
        // Java: 421
        public com.oddlabs.tt.gui.ModeIconQuads getDiode()
        {
            return diode;
        }
        // Java: 425
        public com.oddlabs.tt.gui.Box parseBox(org.w3c.dom.Node n, string name, com.oddlabs.tt.render.Texture texture)
        {
            org.w3c.dom.Node node = com.oddlabs.tt.gui.Icons.getNodeByName(name, n);
            return getBox(node, texture);
        }
        // Java: 430
        public com.oddlabs.tt.gui.Box getEditBox()
        {
            return edit_box;
        }
        // Java: 434
        public com.oddlabs.tt.gui.Box getBackgroundBox()
        {
            return background_box;
        }
        // Java: 438
        public com.oddlabs.tt.gui.GroupData parseGroupData(org.w3c.dom.Node n, com.oddlabs.tt.render.Texture texture)
        {
            org.w3c.dom.Node node = com.oddlabs.tt.gui.Icons.getNodeByName("group", n);
            return new com.oddlabs.tt.gui.GroupData(getBox(node, texture), com.oddlabs.tt.gui.Icons.getInt(node, "caption_left"), com.oddlabs.tt.gui.Icons.getInt(node, "caption_y"), com.oddlabs.tt.gui.Icons.getInt(node, "caption_offset"), getFont(com.oddlabs.tt.gui.Icons.getNodeByName("groupfont", n)));
        }
        // Java: 447
        public com.oddlabs.tt.gui.GroupData getGroupData()
        {
            return group_data;
        }
        // Java: 451
        public com.oddlabs.tt.gui.MultiColumnComboBoxData parseMultiColumnComboBoxData(org.w3c.dom.Node n, com.oddlabs.tt.render.Texture texture)
        {
            org.w3c.dom.Node node = com.oddlabs.tt.gui.Icons.getNodeByName("multi_column_combo", n);
            org.w3c.dom.Node desc = com.oddlabs.tt.gui.Icons.getNodeByName("descending", node);
            org.w3c.dom.Node asc = com.oddlabs.tt.gui.Icons.getNodeByName("ascending", node);
            return new com.oddlabs.tt.gui.MultiColumnComboBoxData(getBox(node, texture), parseHorizButtonPressed(node, texture), parseHorizButtonUnpressed(node, texture), com.oddlabs.tt.gui.Icons.getIconQuads(desc, texture), com.oddlabs.tt.gui.Icons.getIconQuads(asc, texture), com.oddlabs.tt.gui.Icons.getNamedColor(node, "color1"), com.oddlabs.tt.gui.Icons.getNamedColor(node, "color2"), com.oddlabs.tt.gui.Icons.getNamedColor(node, "color_marked"), getFont(com.oddlabs.tt.gui.Icons.getNodeByName("combofont", n)), com.oddlabs.tt.gui.Icons.getInt(node, "caption_offset"));
        }
        // Java: 467
        public com.oddlabs.tt.gui.MultiColumnComboBoxData getMultiColumnComboBoxData()
        {
            return multi_columnCombo_box_data;
        }
        // Java: 471
        public com.oddlabs.tt.gui.ToolTipBoxInfo parseToolTipInfo(org.w3c.dom.Node n, com.oddlabs.tt.render.Texture texture)
        {
            org.w3c.dom.Node node = com.oddlabs.tt.gui.Icons.getNodeByName("tool_tip", n);
            return new com.oddlabs.tt.gui.ToolTipBoxInfo(getHorizontal(node, texture), com.oddlabs.tt.gui.Icons.getInt(node, "left_offset"), com.oddlabs.tt.gui.Icons.getInt(node, "bottom_offset"), com.oddlabs.tt.gui.Icons.getInt(node, "right_offset"), com.oddlabs.tt.gui.Icons.getInt(node, "top_offset"));
        }
        // Java: 480
        public com.oddlabs.tt.gui.ToolTipBoxInfo getToolTipInfo()
        {
            return tool_tip;
        }
        // Java: 484
        public com.oddlabs.tt.gui.PanelData parsePanelData(org.w3c.dom.Node n, com.oddlabs.tt.render.Texture texture)
        {
            org.w3c.dom.Node node = com.oddlabs.tt.gui.Icons.getNodeByName("panel", n);
            return new com.oddlabs.tt.gui.PanelData(getBox(node, texture), getHorizontal(node, texture), com.oddlabs.tt.gui.Icons.getInt(node, "left_caption_offset"), com.oddlabs.tt.gui.Icons.getInt(node, "right_caption_offset"), com.oddlabs.tt.gui.Icons.getInt(node, "bottom_caption_offset"), com.oddlabs.tt.gui.Icons.getInt(node, "left_tab_offset"), com.oddlabs.tt.gui.Icons.getInt(node, "bottom_tab_offset"));
        }
        // Java: 495
        public com.oddlabs.tt.gui.PanelData getPanelData()
        {
            return panel_data;
        }
        // Java: 499
        public com.oddlabs.tt.gui.IconQuad getFlagDefault()
        {
            return flag_default;
        }
        // Java: 503
        public com.oddlabs.tt.gui.IconQuad getFlagDa()
        {
            return flag_da;
        }
        // Java: 507
        public com.oddlabs.tt.gui.IconQuad getFlagEn()
        {
            return flag_en;
        }
        // Java: 511
        public com.oddlabs.tt.gui.IconQuad getFlagDe()
        {
            return flag_de;
        }
        // Java: 515
        public com.oddlabs.tt.gui.IconQuad getFlagEs()
        {
            return flag_es;
        }
        // Java: 519
        public com.oddlabs.tt.gui.IconQuad getFlagIt()
        {
            return flag_it;
        }
        // Java: 523
        public com.oddlabs.tt.gui.IconQuad getFlagPt()
        {
            return flag_pt;
        }
        static Skin()
        {
            Skin.SKIN = new com.oddlabs.tt.gui.Skin("/gui/gui_skin.xml");
        }
        public Skin(string xml_file) : base()
        {
            org.w3c.dom.Node root = com.oddlabs.tt.gui.Icons.loadFile(xml_file, new com.oddlabs.tt.gui.GUIErrorHandler());
            com.oddlabs.tt.render.Texture texture = (com.oddlabs.tt.gui.UiStyle.modern ? com.oddlabs.tt.gui.Icons.loadTexture(java.lang.JStr.valueOf(root.getAttributes().getNamedItem("texture").getNodeValue()) + "_modern") : com.oddlabs.tt.gui.Icons.loadTexture(root));
            edit_font = parseEditFont(root);
            button_font = parseButtonFont(root);
            headline_font = parseHeadlineFont(root);
            plus_button = com.oddlabs.tt.gui.Icons.getNamedIconQuads(root, "plus_button", texture);
            minus_button = com.oddlabs.tt.gui.Icons.getNamedIconQuads(root, "minus_button", texture);
            accept_button = com.oddlabs.tt.gui.Icons.getNamedIconQuads(root, "accept_button", texture);
            cancel_button = com.oddlabs.tt.gui.Icons.getNamedIconQuads(root, "cancel_button", texture);
            back_button = com.oddlabs.tt.gui.Icons.getNamedIconQuads(root, "back_button", texture);
            check_box_marked = parseCheckBoxMarked(root, texture);
            check_box_unmarked = parseCheckBoxUnmarked(root, texture);
            radio_button_marked = parseRadioButtonMarked(root, texture);
            radio_button_unmarked = parseRadioButtonUnmarked(root, texture);
            horiz_button_pressed = parseHorizButtonPressed(root, texture);
            horiz_button_unpressed = parseHorizButtonUnpressed(root, texture);
            form_data = parseFormData(root, texture);
            edit_box = parseBox(root, "editbox", texture);
            background_box = parseBox(root, "backgroundbox", texture);
            group_data = parseGroupData(root, texture);
            scroll_bar_data = parseScrollBarData(root, texture);
            slider_data = parseSliderData(root, texture);
            pulldown_data = parsePulldownData(root, texture);
            progress_bar_data = parseProgressBarData(root, texture);
            multi_columnCombo_box_data = parseMultiColumnComboBoxData(root, texture);
            tool_tip = parseToolTipInfo(root, texture);
            diode = com.oddlabs.tt.gui.Icons.getNamedIconQuads(root, "diode", texture);
            panel_data = parsePanelData(root, texture);
            flag_default = com.oddlabs.tt.gui.Icons.getNamedIconQuad(root, "flag_default", texture);
            flag_da = com.oddlabs.tt.gui.Icons.getNamedIconQuad(root, "flag_da", texture);
            flag_en = com.oddlabs.tt.gui.Icons.getNamedIconQuad(root, "flag_en", texture);
            flag_de = com.oddlabs.tt.gui.Icons.getNamedIconQuad(root, "flag_de", texture);
            flag_es = com.oddlabs.tt.gui.Icons.getNamedIconQuad(root, "flag_es", texture);
            flag_it = com.oddlabs.tt.gui.Icons.getNamedIconQuad(root, "flag_it", texture);
            flag_pt = com.oddlabs.tt.gui.Icons.getNamedIconQuad(root, "flag_pt", texture);
        }
    }
}
