// Translated from CampaignDialogForm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 23
    public class CampaignDialogForm : com.oddlabs.tt.gui.Form
    {
        public const int WIDTH = 300;
        public java.lang.Runnable runnable;
        public bool cancel_f;
        public bool dismissed;
        public com.oddlabs.tt.gui.HorizButton ok_button;
        // Java: 84
        public virtual void run()
        {
            if (runnable != null)
            {
                runnable.run();
            }
        }
        // Java: 89
        public override void cancel()
        {
            if (dismissed)
            {
                return;
            }
            dismissed = true;
            base.cancel();
        }
        // Java: 96
        public override void doCancel()
        {
            if (!cancel_f)
            {
                run();
            }
        }
        // Java: 102
        public void buildForm(string header, string text, com.oddlabs.tt.gui.IconQuad image, com.oddlabs.tt.gui.Origin align, bool cancel_v)
        {
            com.oddlabs.tt.gui.GUIIcon gui_icon = null;
            if (image != null)
            {
                gui_icon = new com.oddlabs.tt.gui.GUIIcon(image);
                addChild(gui_icon);
            }
            com.oddlabs.tt.gui.Label header_label = new com.oddlabs.tt.gui.Label(header, com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont());
            addChild(header_label);
            com.oddlabs.tt.gui.LabelBox label_box = new com.oddlabs.tt.gui.LabelBox(text, com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), com.oddlabs.tt.form.CampaignDialogForm.WIDTH);
            addChild(label_box);
            addChild(ok_button);
            if (gui_icon != null)
            {
                gui_icon.place();
                label_box.place(gui_icon, (((object)align == (object)com.oddlabs.tt.gui.Origin.AT_START) ? com.oddlabs.tt.gui.Placement.RIGHT_MID : com.oddlabs.tt.gui.Placement.LEFT_MID));
            }
            else
            {
                label_box.place();
            }
            header_label.place(label_box, com.oddlabs.tt.gui.Placement.TOP_LEFT);
            ok_button.place(com.oddlabs.tt.gui.Origin.AT_END);
            if (cancel_v)
            {
                com.oddlabs.tt.gui.HorizButton cancel_button = new com.oddlabs.tt.gui.CancelButton(80);
                addChild(cancel_button);
                cancel_button.place(ok_button, com.oddlabs.tt.gui.Placement.RIGHT_MID);
                cancel_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
                {
                    this.cancel();
                }));
            }
            compileCanvas();
            centerPos();
        }
        // Java: 134
        public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
            base.handleInput(@event);
        }
        // Java: 139
        public override void setFocus()
        {
            ok_button.setFocus();
        }
        private void init_CampaignDialogForm()
        {
            this.dismissed = false;
            this.ok_button = new CampaignDialogForm.Anon1_OKButton(this, 80);
        }
        public CampaignDialogForm(string header, string text, com.oddlabs.tt.gui.IconQuad image, com.oddlabs.tt.gui.Origin align) : this(header, text, image, align, null)
        {
        }
        public CampaignDialogForm(string header, string text, com.oddlabs.tt.gui.IconQuad image, com.oddlabs.tt.gui.Origin align, java.lang.Runnable runnable) : this(header, text, image, align, runnable, false)
        {
        }
        public CampaignDialogForm(string header, string text, com.oddlabs.tt.gui.IconQuad image, com.oddlabs.tt.gui.Origin align, java.lang.Runnable runnable, bool cancel_v) : base()
        {
            init_CampaignDialogForm();
            this.runnable = runnable;
            this.cancel_f = cancel_v;
            buildForm(header, text, image, align, cancel_v);
            ok_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                if (dismissed)
                {
                    return;
                }
                dismissed = true;
                run();
                try
                {
                    remove();
                }
                catch (System.Exception e)
                {
                    e.printStackTrace();
                }
            }));
            ok_button.addInputListener(new com.oddlabs.tt.guievent.InputListener_L((com.oddlabs.tt.input.InputEvent @event) =>
            {
                if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.PRESSED))
                {
                    if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_CANCEL))
                    {
                        this.cancel();
                        @event.consume();
                    }
                }
            }));
        }
        // Java: anonymous class at line 30
        public sealed class Anon1_OKButton : com.oddlabs.tt.gui.OKButton
        {
            public com.oddlabs.tt.form.CampaignDialogForm this_0;
            public Anon1_OKButton(com.oddlabs.tt.form.CampaignDialogForm this_0_, int a0_) : base(a0_)
            {
                this.this_0 = this_0_;
            }
            // Java: 31
            public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
            {
                if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.PRESSED) && @event.consumeAction(com.oddlabs.tt.input.GameAction.UI_ACTIVATE))
                {
                    if (this_0.dismissed)
                    {
                        return;
                    }
                    this_0.dismissed = true;
                    this_0.run();
                    try
                    {
                        this.this_0.remove();
                    }
                    catch (System.Exception e)
                    {
                        e.printStackTrace();
                    }
                    @event.consume();
                    return;
                }
                base.handleInput(@event);
            }
        }
    }
}
