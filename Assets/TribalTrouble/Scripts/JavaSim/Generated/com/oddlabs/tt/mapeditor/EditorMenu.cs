// Translated from EditorMenu.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.mapeditor
{
    // Java: 11
    public class EditorMenu : com.oddlabs.tt.gui.Form
    {
        public const int BUTTON_WIDTH = 200;
        public com.oddlabs.tt.gui.HorizButton button_resume;
        // Java: 41
        public override void setFocus(com.oddlabs.tt.gui.FocusDirection direction)
        {
            if (((object)direction == (object)com.oddlabs.tt.gui.FocusDirection.BACKWARD))
            {
                base.setFocus(direction);
            }
            else
            {
                button_resume.setFocus(direction);
            }
        }
        public EditorMenu(java.lang.Runnable save, java.lang.Runnable exit) : base(com.oddlabs.tt.mapeditor.MapEditor.i18n("headline"))
        {
            button_resume = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.mapeditor.MapEditor.i18n("resume"), com.oddlabs.tt.mapeditor.EditorMenu.BUTTON_WIDTH);
            button_resume.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                remove();
            }));
            com.oddlabs.tt.gui.HorizButton button_save = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.mapeditor.MapEditor.i18n("save"), com.oddlabs.tt.mapeditor.EditorMenu.BUTTON_WIDTH);
            button_save.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_5, int unused_6, int unused_7, int unused_8) =>
            {
                remove();
                save.run();
            }));
            com.oddlabs.tt.gui.HorizButton button_exit = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.mapeditor.MapEditor.i18n("exit_editor"), com.oddlabs.tt.mapeditor.EditorMenu.BUTTON_WIDTH);
            button_exit.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_9, int unused_10, int unused_11, int unused_12) =>
            {
                remove();
                exit.run();
            }));
            addChild(button_resume);
            addChild(button_save);
            addChild(button_exit);
            button_resume.place();
            button_save.place(button_resume, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            button_exit.place(button_save, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            compileCanvas();
            centerPos();
        }
    }
}
