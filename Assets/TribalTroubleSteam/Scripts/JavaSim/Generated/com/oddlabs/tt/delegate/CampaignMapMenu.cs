// Translated from CampaignMapMenu.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.@delegate
{
    // Java: 21
    public class CampaignMapMenu : com.oddlabs.tt.gui.Form
    {
        public static org.joml.Vector4fc DARK_GLASS;
        public com.oddlabs.tt.gui.GUIRoot gui_root;
        public com.oddlabs.net.NetworkSelector network;
        public com.oddlabs.tt.gui.GUIImage overlay;
        public com.oddlabs.tt.gui.GUIImage logo;
        public com.oddlabs.tt.gui.MenuButton resumeButton;
        // Java: 44
        public void initBackground()
        {
            int width = gui_root.getWidth();
            int height = gui_root.getHeight();
            overlay = new com.oddlabs.tt.gui.GUIImage(width, height, 0.0f, 0.0f, (float)(800.0f / 1024.0f), (float)(600.0f / 1024.0f), "/textures/gui/mainmenu");
            overlay.setPos(0, 0);
            addChild(overlay);
            string logo_file = com.oddlabs.tt.@delegate.Menu.i18n("logo_file");
            float heightScale = (float)(height / 600.0f);
            int logoHeight = java.lang.JC.f2i(((float)(206.0f * heightScale)));
            int logoWidth = java.lang.JC.f2i(((float)(347.0f * heightScale)));
            logo = new com.oddlabs.tt.gui.GUIImage(logoWidth, logoHeight, 0.0f, 0.0f, (float)(347.0f / 512.0f), (float)(206.0f / 256.0f), logo_file);
            logo.setPos(0, height - logoHeight);
            addChild(logo);
        }
        // Java: 62
        public void addResumeButton()
        {
            resumeButton = new com.oddlabs.tt.gui.MenuButton(com.oddlabs.tt.@delegate.Menu.i18n("resume"), com.oddlabs.tt.@delegate.Menu.COLOR_NORMAL, com.oddlabs.tt.@delegate.Menu.COLOR_ACTIVE);
            addChild(resumeButton);
            resumeButton.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                cancel();
            }));
        }
        // Java: 68
        public override void setFocus(com.oddlabs.tt.gui.FocusDirection direction)
        {
            if (((object)direction == (object)com.oddlabs.tt.gui.FocusDirection.BACKWARD))
            {
                base.setFocus(direction);
            }
            else
            {
                setFocus();
            }
        }
        // Java: 77
        public override void setFocus()
        {
            if (resumeButton != null)
            {
                resumeButton.setFocus();
            }
            else
            {
                base.setFocus(com.oddlabs.tt.gui.FocusDirection.FORWARD);
            }
        }
        // Java: 86
        public void addOptionsButton()
        {
            com.oddlabs.tt.gui.MenuButton options = new com.oddlabs.tt.gui.MenuButton(com.oddlabs.tt.@delegate.Menu.i18n("options"), com.oddlabs.tt.@delegate.Menu.COLOR_NORMAL, com.oddlabs.tt.@delegate.Menu.COLOR_ACTIVE);
            addChild(options);
            options.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                gui_root.addModalForm(new com.oddlabs.tt.form.OptionsMenu(gui_root));
            }));
        }
        // Java: 92
        public void addAbortButton()
        {
            string abort_text = com.oddlabs.tt.@delegate.Menu.i18n("end_campaign");
            com.oddlabs.tt.gui.MenuButton abort = new com.oddlabs.tt.gui.MenuButton(abort_text, com.oddlabs.tt.@delegate.Menu.COLOR_NORMAL, com.oddlabs.tt.@delegate.Menu.COLOR_ACTIVE);
            addChild(abort);
            abort.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                gui_root.addModalForm(new com.oddlabs.tt.form.QuestionForm(com.oddlabs.tt.@delegate.Menu.i18n("end_game_confirm"), new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_5, int unused_6, int unused_7, int unused_8) =>
                {
                    com.oddlabs.tt.@delegate.CampaignMapForm.closeCampaign(network, gui_root.getGUI());
                })));
            }));
        }
        // Java: 101
        public void addExitButton()
        {
            com.oddlabs.tt.gui.MenuButton exit = new com.oddlabs.tt.gui.MenuButton(com.oddlabs.tt.@delegate.Menu.i18n("quit"), com.oddlabs.tt.@delegate.Menu.COLOR_NORMAL, com.oddlabs.tt.@delegate.Menu.COLOR_ACTIVE);
            addChild(exit);
            exit.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                gui_root.addModalForm(new com.oddlabs.tt.form.QuitForm(gui_root));
            }));
        }
        // Java: 107
        public void layoutButtons()
        {
            int width = gui_root.getWidth();
            int height = gui_root.getHeight();
            setDim(width, height);
            if (overlay != null)
            {
                overlay.setDim(width, height);
            }
            if (logo != null)
            {
                float heightScale = (float)(height / 600.0f);
                int logoHeight = java.lang.JC.f2i(((float)(206.0f * heightScale)));
                int logoWidth = java.lang.JC.f2i(((float)(347.0f * heightScale)));
                logo.setDim(logoWidth, logoHeight);
                logo.setPos(0, height - logoHeight);
            }
            int x = 15;
            int y = getHeight() - java.lang.JC.f2i(((float)((float)(190.0f * getHeight()) / 600.0f)));
            com.oddlabs.tt.gui.GUIObject child = ((com.oddlabs.tt.gui.GUIObject)getLastChild());
            while (child != null)
            {
                if ((child is com.oddlabs.tt.gui.MenuButton))
                {
                    child.setPos(x, y - child.getHeight());
                    y -= java.lang.JC.d2i((child.getHeight() * 0.875d));
                }
                child = ((com.oddlabs.tt.gui.GUIObject)child.getPrior());
            }
        }
        // Java: 137
        public override void displayChangedNotify(int width, int height)
        {
            setDim(width, height);
            layoutButtons();
        }
        // Java: 143
        public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
            if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.PRESSED))
            {
                if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_CANCEL))
                {
                    cancel();
                    @event.consume();
                    return;
                }
            }
            base.handleInput(@event);
        }
        // Java: 155
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            renderer.drawColoredQuad(0, 0, getWidth(), getHeight(), com.oddlabs.tt.@delegate.CampaignMapMenu.DARK_GLASS);
        }
        static CampaignMapMenu()
        {
            CampaignMapMenu.DARK_GLASS = new org.joml.Vector4f(0.0f, 0.0f, 0.0f, 0.7f);
        }
        public CampaignMapMenu(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root) : base()
        {
            this.network = network;
            this.gui_root = gui_root;
            initBackground();
            addResumeButton();
            addOptionsButton();
            addAbortButton();
            addExitButton();
            layoutButtons();
            setFocus();
        }
    }
}
