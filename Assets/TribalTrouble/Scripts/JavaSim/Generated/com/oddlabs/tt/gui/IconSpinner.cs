// Translated from IconSpinner.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 16
    public abstract class IconSpinner : com.oddlabs.tt.gui.GUIObject
    {
        public static java.util.ResourceBundle bundle;
        public const int MAX_DEPLOY_AMOUNT = 1000000;
        // Java: 22
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.gui.IconSpinner.bundle, key, args);
        }
        public com.oddlabs.tt.gui.ModeIconQuads icon_quad;
        public string tool_tip;
        public java.util.List tool_tip_icons;
        public com.oddlabs.tt.gui.IconQuad infinite_icon;
        public com.oddlabs.tt.gui.TextField label;
        public com.oddlabs.tt.gui.IconButton button_plus;
        public com.oddlabs.tt.gui.IconButton button_minus;
        public com.oddlabs.tt.viewer.WorldViewer viewer;
        public com.oddlabs.tt.gui.IconDisabler icon_disabler;
        public int text_count;
        // Java: 78
        public override void setFocus()
        {
            viewer.getGUIRoot().getDelegate().setFocus();
        }
        // Java: 83
        public void setIconDisabler(com.oddlabs.tt.gui.IconDisabler icon_disabler)
        {
            this.icon_disabler = icon_disabler;
        }
        // Java: 88
        public virtual bool isLocked()
        {
            return false;
        }
        // Java: 92
        public override bool isHidden()
        {
            return isLocked();
        }
        // Java: 97
        public void doUpdate()
        {
            setCount();
            if (isLocked())
            {
                setDisabled(true);
                return;
            }
            if (icon_disabler != null)
            {
                bool no_supply = computeCount() == 0 && getOrderSize() == 0 && icon_disabler.isDisabled();
                setDisabled(no_supply && getDisplayCount() == 0);
                if (!isDisabled())
                {
                    button_plus.setDisabled(no_supply);
                }
            }
        }
        // Java: 112
        public abstract int computeCount();
        // Java: 114
        public abstract void increase(int amount);
        // Java: 116
        public abstract void decrease(int amount);
        // Java: 118
        public abstract void release();
        // Java: 120
        public abstract int getOrderSize();
        // Java: 122
        public abstract bool renderInfinite();
        // Java: 124
        public abstract float getProgress();
        // Java: 126
        public virtual int getDisplayCount()
        {
            return computeCount();
        }
        // Java: 130
        public void setCount()
        {
            int count = getDisplayCount();
            if (count != text_count)
            {
                text_count = count;
                label.clear();
                if (text_count != 0)
                {
                    label.append((renderInfinite() ? "\u221e" : java.lang.Integer.toString(text_count)));
                }
            }
        }
        // Java: 141
        public override bool hasToolTip()
        {
            return !tool_tip.isEmpty();
        }
        // Java: 146
        public override void appendToolTip(com.oddlabs.tt.gui.ToolTipBox tool_tip_box)
        {
            tool_tip_box.append(tool_tip);
            tool_tip_box.append(tool_tip_icons);
        }
        // Java: 152
        public void shortcutPressed(bool decrement, bool batch)
        {
            if (!isDisabled())
            {
                com.oddlabs.tt.gui.MouseButton mouse_button = (batch ? com.oddlabs.tt.gui.MouseButton.RIGHT : com.oddlabs.tt.gui.MouseButton.LEFT);
                ((decrement ? button_minus : button_plus)).mousePressedAll(mouse_button, 0, 0);
            }
        }
        // Java: 160
        public void shortcutReleased(bool decrement, bool batch)
        {
            if (!isDisabled())
            {
                release();
            }
        }
        // Java: 166
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            int x = (getWidth() - icon_quad.quad(com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL).getWidth()) / 2;
            int y = (getHeight() - icon_quad.quad(com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL).getHeight()) / 2;
            com.oddlabs.tt.gui.ModeIconQuads.Mode skinMode = (isDisabled() ? com.oddlabs.tt.gui.ModeIconQuads.Mode.DISABLED : (isHovered() ? com.oddlabs.tt.gui.ModeIconQuads.Mode.ACTIVE : com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL));
            renderer.drawIcon(icon_quad.quad(skinMode), x, y);
            if (computeCount() > 0)
            {
                com.oddlabs.tt.gui.IconQuad watchQuad = com.oddlabs.tt.gui.GUIIcons.getIcons().getWatch(getProgress());
                renderer.drawIcon(watchQuad, getWidth() - watchQuad.getWidth(), getHeight() - watchQuad.getHeight());
            }
            if (renderInfinite())
            {
                renderer.drawIcon(infinite_icon, 0, 0);
            }
        }
        // Java: 185
        public override void mouseReleased(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
        }
        // Java: 189
        public override void mousePressed(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
        }
        // Java: 193
        public override void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
        {
        }
        // Java: 197
        public override void mouseHeld(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
        }
        // Java: 201
        public class IncreaseListener : java.lang.Object, com.oddlabs.tt.guievent.MouseButtonListener
        {
            public com.oddlabs.tt.gui.IconSpinner this_0;
            // Java: 202
            public void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
            {
            }
            // Java: 206
            public void mouseHeld(com.oddlabs.tt.gui.MouseButton button, int x, int y)
            {
                mousePressed(button, x, y);
            }
            // Java: 211
            public void mousePressed(com.oddlabs.tt.gui.MouseButton button, int x, int y)
            {
                if (com.oddlabs.tt.render.Renderer.getLocalInput().isShiftDownCurrently())
                {
                    this_0.increase(com.oddlabs.tt.gui.IconSpinner.MAX_DEPLOY_AMOUNT);
                }
                else
                {
                    this_0.increase((((object)button == (object)com.oddlabs.tt.gui.MouseButton.RIGHT) ? 10 : 1));
                }
            }
            // Java: 232
            public void mouseReleased(com.oddlabs.tt.gui.MouseButton button, int x, int y)
            {
                this_0.release();
            }
            public IncreaseListener(com.oddlabs.tt.gui.IconSpinner this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        // Java: 238
        public class DecreaseListener : java.lang.Object, com.oddlabs.tt.guievent.MouseButtonListener
        {
            public com.oddlabs.tt.gui.IconSpinner this_0;
            // Java: 239
            public void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
            {
            }
            // Java: 243
            public void mouseHeld(com.oddlabs.tt.gui.MouseButton button, int x, int y)
            {
                mousePressed(button, x, y);
            }
            // Java: 248
            public void mousePressed(com.oddlabs.tt.gui.MouseButton button, int x, int y)
            {
                if (com.oddlabs.tt.render.Renderer.getLocalInput().isShiftDownCurrently())
                {
                    this_0.decrease(100);
                }
                else
                {
                    this_0.decrease((((object)button == (object)com.oddlabs.tt.gui.MouseButton.RIGHT) ? 10 : 1));
                }
            }
            // Java: 264
            public void mouseReleased(com.oddlabs.tt.gui.MouseButton button, int x, int y)
            {
                this_0.release();
            }
            public DecreaseListener(com.oddlabs.tt.gui.IconSpinner this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        static IconSpinner()
        {
            IconSpinner.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.gui.IconSpinner).getName());
        }
        private void init_IconSpinner()
        {
            this.icon_disabler = null;
            this.text_count = 0;
        }
        public IconSpinner(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.gui.ModeIconQuads icon_quad, string tool_tip, java.util.List tool_tip_icons, com.oddlabs.tt.input.GameAction action, com.oddlabs.tt.input.GameAction dec_action) : base()
        {
            init_IconSpinner();
            this.icon_quad = icon_quad;
            this.infinite_icon = com.oddlabs.tt.gui.GUIIcons.getIcons().getInfinite();
            this.tool_tip = tool_tip;
            this.tool_tip_icons = tool_tip_icons;
            this.viewer = viewer;
            setCanFocus(true);
            setDim(icon_quad.quad(com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL).getWidth(), icon_quad.quad(com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL).getHeight());
            button_plus = new com.oddlabs.tt.gui.IconSpinnerButton(com.oddlabs.tt.gui.Skin.getSkin().getPlusButton(), action, new java.util.function.Supplier_L(() =>
            {
                return i18n("increase", com.oddlabs.tt.render.Renderer.getLocalInput().getInputManager().getBindingString(action));
            }), this);
            button_plus.setPos(0, 0);
            button_plus.addMouseButtonListener(new com.oddlabs.tt.gui.IconSpinner.IncreaseListener(this));
            addChild(button_plus);
            button_minus = new com.oddlabs.tt.gui.IconSpinnerButton(com.oddlabs.tt.gui.Skin.getSkin().getMinusButton(), dec_action, new java.util.function.Supplier_L(() =>
            {
                return i18n("decrease", com.oddlabs.tt.render.Renderer.getLocalInput().getInputManager().getBindingString(dec_action));
            }), this);
            button_minus.setPos(button_plus.getWidth(), 0);
            button_minus.addMouseButtonListener(new com.oddlabs.tt.gui.IconSpinner.DecreaseListener(this));
            addChild(button_minus);
            int icon_width = icon_quad.quad(com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL).getWidth();
            int label_width = java.lang.Math.max(icon_width, com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont().getWidth("999"));
            label = new com.oddlabs.tt.gui.Label("", com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont(), label_width, com.oddlabs.tt.gui.Origin.AT_MIDDLE);
            label.setPos((icon_width - label_width) / 2, (getHeight() - label.getHeight()) / 2);
            addChild(label);
        }
    }
}
