// Translated from PasswordLine.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 10
    public class PasswordLine : com.oddlabs.tt.gui.EditLine
    {
        public string password_digest;
        // Java: 24
        public override string getDisplayText()
        {
            if (getText().isEmpty())
            {
                return "";
            }
            if (isActive())
            {
                return "*".repeat(getText().length());
            }
            else
            {
                int asteriskWidth = getRenderedWidth("*");
                if (asteriskWidth == 0)
                {
                    return "";
                }
                int numAsterisks = max_text_width / asteriskWidth;
                return "*".repeat(numAsterisks);
            }
        }
        // Java: 39
        public override bool insert(int index, char key)
        {
            bool result = base.insert(index, key);
            updatePassword();
            return result;
        }
        // Java: 47
        public override void delete(int index)
        {
            base.delete(index);
            updatePassword();
        }
        // Java: 53
        public void updatePassword()
        {
            password_digest = com.oddlabs.util.CryptUtils.digest(java.lang.JO.toString(getText()));
        }
        // Java: 57
        public string getPasswordDigest()
        {
            return password_digest;
        }
        // Java: 62
        public void setPassword(string text)
        {
            set(text);
            updatePassword();
        }
        // Java: 67
        public void setPasswordDigest(string password_digest)
        {
            this.password_digest = password_digest;
        }
        public PasswordLine(int width, int max_chars) : base(width, max_chars)
        {
        }
    }
}
