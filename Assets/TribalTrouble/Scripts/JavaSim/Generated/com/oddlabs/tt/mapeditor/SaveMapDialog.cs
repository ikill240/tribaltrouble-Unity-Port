// Translated from SaveMapDialog.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.mapeditor
{
    // Java: 24
    public class SaveMapDialog : com.oddlabs.tt.gui.Form
    {
        public const int BUTTON_WIDTH = 100;
        public const int EDITLINE_WIDTH = 280;
        public com.oddlabs.tt.gui.GUIRoot gui_root;
        public java.nio.file.Path dir;
        public java.util.function.Consumer save;
        public com.oddlabs.tt.gui.EditLine editline_name;
        // Java: 62
        public override void setFocus(com.oddlabs.tt.gui.FocusDirection direction)
        {
            if (((object)direction == (object)com.oddlabs.tt.gui.FocusDirection.BACKWARD))
            {
                base.setFocus(direction);
            }
            else
            {
                editline_name.setFocus(direction);
            }
        }
        // Java: 71
        public void submit()
        {
            string name = editline_name.getContents().trim();
            if (!com.oddlabs.tt.mapeditor.MapFile.isValidName(name))
            {
                editline_name.triggerError();
                return;
            }
            remove();
            if (java.nio.file.Files.exists(com.oddlabs.tt.mapeditor.MapFile.pathFor(dir, name)))
            {
                gui_root.addModalForm(new com.oddlabs.tt.form.QuestionForm(com.oddlabs.tt.mapeditor.MapEditor.i18n("overwrite_confirm", name), new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
                {
                    save.accept(name);
                })));
            }
            else
            {
                save.accept(name);
            }
        }
        public SaveMapDialog(com.oddlabs.tt.gui.GUIRoot gui_root, java.nio.file.Path dir, string initial_name, java.util.function.Consumer save) : base(com.oddlabs.tt.mapeditor.MapEditor.i18n("save_caption"))
        {
            this.gui_root = gui_root;
            this.dir = dir;
            this.save = save;
            com.oddlabs.tt.gui.Label label_name = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.mapeditor.MapEditor.i18n("map_name"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            editline_name = new com.oddlabs.tt.gui.EditLine(com.oddlabs.tt.mapeditor.SaveMapDialog.EDITLINE_WIDTH, com.oddlabs.tt.mapeditor.MapFile.getMaxNameLength());
            editline_name.append(initial_name);
            editline_name.addEnterListener(new com.oddlabs.tt.guievent.EnterListener_L((string unused) =>
            {
                submit();
            }));
            com.oddlabs.tt.gui.HorizButton button_ok = new com.oddlabs.tt.gui.OKButton(com.oddlabs.tt.mapeditor.SaveMapDialog.BUTTON_WIDTH);
            button_ok.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_2, int unused_3, int unused_4, int unused_5) =>
            {
                submit();
            }));
            com.oddlabs.tt.gui.HorizButton button_cancel = new com.oddlabs.tt.gui.CancelButton(com.oddlabs.tt.mapeditor.SaveMapDialog.BUTTON_WIDTH);
            button_cancel.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_6, int unused_7, int unused_8, int unused_9) =>
            {
                cancel();
            }));
            addChild(label_name);
            addChild(editline_name);
            addChild(button_ok);
            addChild(button_cancel);
            label_name.place();
            editline_name.place(label_name, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            button_cancel.place(editline_name, com.oddlabs.tt.gui.Placement.BOTTOM_RIGHT);
            button_ok.place(button_cancel, com.oddlabs.tt.gui.Placement.LEFT_MID);
            compileCanvas();
            centerPos();
        }
    }
}
