// Translated from PasswordLine.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 10
    public class PasswordLine : com.oddlabs.tt.gui.EditLine
    {
        public string password_digest;
        public bool remembered;
        // Java: 26
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
        // Java: 41
        public override bool insert(int index, char key)
        {
            if (remembered)
            {
                remembered = false;
                clear();
                index = 0;
            }
            bool result = base.insert(index, key);
            updatePassword();
            return result;
        }
        // Java: 54
        public override void delete(int index)
        {
            if (remembered)
            {
                forget();
                return;
            }
            base.delete(index);
            updatePassword();
        }
        // Java: 64
        public void updatePassword()
        {
            password_digest = com.oddlabs.util.CryptUtils.digest(java.lang.JO.toString(getText()));
        }
        // Java: 68
        public string getPasswordDigest()
        {
            return password_digest;
        }
        // Java: 72
        public void setPasswordDigest(string password_digest)
        {
            this.password_digest = password_digest;
        }
        // Java: 77
        public void setRemembered(string digest)
        {
            clear();
            append("*************");
            password_digest = digest;
            remembered = true;
        }
        // Java: 85
        public void forget()
        {
            remembered = false;
            clear();
            password_digest = com.oddlabs.util.CryptUtils.digest("");
        }
        public PasswordLine(int width, int max_chars) : base(width, max_chars)
        {
        }
    }
}
