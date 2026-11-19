// Translated from SavePresetDialog.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 21
    public class SavePresetDialog : com.oddlabs.tt.gui.Form
    {
        public const int BUTTON_WIDTH = 100;
        public const int EDITLINE_WIDTH = 240;
        public const int MAX_NAME_LENGTH = 64;
        public static java.util.ResourceBundle bundle;
        // Java: 27
        public interface SaveListener
        {
            // Java: 29
            void save(string name);
        }
        public sealed class SaveListener_L : java.lang.Object, SaveListener
        {
            readonly System.Action<string> f;
            public SaveListener_L(System.Action<string> f) { this.f = f; }
            public void save(string p0) { f(p0); }
        }
        public java.util.function.Predicate name_taken;
        public com.oddlabs.tt.form.SavePresetDialog.SaveListener listener;
        public com.oddlabs.tt.gui.EditLine editline_name;
        // Java: 64
        public static string i18n(string key)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.form.SavePresetDialog.bundle, key);
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
                editline_name.setFocus(direction);
            }
        }
        // Java: 77
        public void submit()
        {
            string name = editline_name.getContents().trim();
            if (name.isEmpty() || name_taken.test(name))
            {
                editline_name.triggerError();
                return;
            }
            remove();
            listener.save(name);
        }
        static SavePresetDialog()
        {
            SavePresetDialog.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.form.SavePresetDialog).getName());
        }
        public SavePresetDialog(java.util.function.Predicate name_taken, com.oddlabs.tt.form.SavePresetDialog.SaveListener listener) : base(com.oddlabs.tt.form.SavePresetDialog.i18n("save_preset_caption"))
        {
            this.name_taken = name_taken;
            this.listener = listener;
            com.oddlabs.tt.gui.Label label_name = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.SavePresetDialog.i18n("preset_name"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            editline_name = new com.oddlabs.tt.gui.EditLine(com.oddlabs.tt.form.SavePresetDialog.EDITLINE_WIDTH, com.oddlabs.tt.form.SavePresetDialog.MAX_NAME_LENGTH);
            editline_name.addEnterListener(new com.oddlabs.tt.guievent.EnterListener_L((string unused) =>
            {
                submit();
            }));
            com.oddlabs.tt.gui.HorizButton button_ok = new com.oddlabs.tt.gui.OKButton(com.oddlabs.tt.form.SavePresetDialog.BUTTON_WIDTH);
            button_ok.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_2, int unused_3, int unused_4, int unused_5) =>
            {
                submit();
            }));
            com.oddlabs.tt.gui.HorizButton button_cancel = new com.oddlabs.tt.gui.CancelButton(com.oddlabs.tt.form.SavePresetDialog.BUTTON_WIDTH);
            button_cancel.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_6, int unused_7, int unused_8, int unused_9) =>
            {
                this.cancel();
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
