// Translated from InGameMainMenu.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.@delegate
{
    // Java: 18
    public class InGameMainMenu : com.oddlabs.tt.@delegate.Menu
    {
        public com.oddlabs.tt.viewer.WorldViewer viewer;
        public com.oddlabs.tt.gui.Group game_infos;
        // Java: 29
        public override void doAdd()
        {
            base.doAdd();
            viewer.setPaused(true);
        }
        // Java: 35
        public override void doRemove()
        {
            base.doRemove();
            viewer.setPaused(false);
        }
        // Java: 41
        public void addAbortButton(string abort_text)
        {
            com.oddlabs.tt.gui.MenuButton abort = new com.oddlabs.tt.gui.MenuButton(abort_text, com.oddlabs.tt.@delegate.Menu.COLOR_NORMAL, com.oddlabs.tt.@delegate.Menu.COLOR_ACTIVE);
            addChild(abort);
            abort.addMouseClickListener(new com.oddlabs.tt.@delegate.InGameMainMenu.AbortListener(this));
        }
        // Java: 48
        public void addButton(string text, java.lang.Runnable action)
        {
            com.oddlabs.tt.gui.MenuButton button = new com.oddlabs.tt.gui.MenuButton(text, com.oddlabs.tt.@delegate.Menu.COLOR_NORMAL, com.oddlabs.tt.@delegate.Menu.COLOR_ACTIVE);
            addChild(button);
            button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                action.run();
            }));
        }
        // Java: 54
        public override void addButtons()
        {
            addResumeButton();
            addOptionsButton(new com.oddlabs.tt.@delegate.FormFactory_L(() =>
            {
                return new com.oddlabs.tt.form.InGameOptionsMenu(getGUIRoot(), viewer);
            }));
            game_infos = new com.oddlabs.tt.gui.Group(true);
            viewer.addGUI(this, game_infos);
            addChild(game_infos);
            addExitButton();
        }
        // Java: 67
        public override void displayChangedNotify(int width, int height)
        {
            base.displayChangedNotify(width, height);
            if (game_infos != null)
            {
                game_infos.setPos((width - game_infos.getWidth()) / 2, (height - game_infos.getHeight()) / 2);
            }
        }
        // Java: 74
        public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
            if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.PRESSED) || ((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.REPEAT))
            {
                if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_CANCEL))
                {
                    pop();
                    @event.consume();
                    return;
                }
            }
            base.handleInput(@event);
        }
        // Java: 86
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            base.renderGeometry(renderer);
            renderBackgroundAlpha(renderer);
        }
        // Java: 92
        public class AbortListener : java.lang.Object, com.oddlabs.tt.guievent.MouseClickListener
        {
            public com.oddlabs.tt.@delegate.InGameMainMenu this_0;
            // Java: 93
            public void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
            {
                this_0.setMenuCentered(new com.oddlabs.tt.form.QuestionForm(com.oddlabs.tt.@delegate.Menu.i18n("end_game_confirm"), new com.oddlabs.tt.@delegate.InGameMainMenu.ActionAbortListener(this.this_0)));
            }
            public AbortListener(com.oddlabs.tt.@delegate.InGameMainMenu this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        // Java: 99
        public class ActionAbortListener : java.lang.Object, com.oddlabs.tt.guievent.MouseClickListener
        {
            public com.oddlabs.tt.@delegate.InGameMainMenu this_0;
            // Java: 100
            public void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
            {
                this_0.viewer.abort();
            }
            public ActionAbortListener(com.oddlabs.tt.@delegate.InGameMainMenu this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        public InGameMainMenu(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.camera.Camera camera) : base(viewer.getNetwork(), viewer.getGUIRoot(), camera)
        {
            this.viewer = viewer;
            reload();
        }
    }
}
