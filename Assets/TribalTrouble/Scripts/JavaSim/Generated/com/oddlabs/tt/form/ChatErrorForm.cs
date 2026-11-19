// Translated from ChatErrorForm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 9
    public class ChatErrorForm : com.oddlabs.tt.form.MessageForm
    {
        public static java.util.ResourceBundle bundle;
        // Java: 12
        public static string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.form.ChatErrorForm.bundle, key, args);
        }
        // Java: 16
        public static string getErrorFromCode(int error_code)
        {
            return com.oddlabs.tt.form.ChatErrorForm.i18n(((error_code) switch { 10 => (string)("chat_error_too_many_users"), 11 => (string)("chat_error_invalid_name"), 12 => (string)("chat_error_no_such_nick"), 14 => (string)("chat_error_spectate_failed"), _ => throw new System.ArgumentException("Unknown error code: " + (error_code)) }));
        }
        static ChatErrorForm()
        {
            ChatErrorForm.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.form.ChatErrorForm).getName());
        }
        public ChatErrorForm(int error_code) : base(com.oddlabs.tt.form.ChatErrorForm.getErrorFromCode(error_code))
        {
        }
    }
}
