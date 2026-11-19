// Translated from RegistrationKeyFormatException.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.registration
{
    // Java: 5
    public class RegistrationKeyFormatException : System.FormatException
    {
        public const long serialVersionUID = 3673901824484153336L;
        public const int TYPE_INVALID_CHAR = 0;
        public const int TYPE_INVALID_LENGTH = 1;
        public const int TYPE_INVALID_KEY = 2;
        public int type;
        public int stripped_length;
        public char invalid_char;
        // Java: 32
        public int getType()
        {
            return type;
        }
        // Java: 36
        public char getInvalidChar()
        {
            return invalid_char;
        }
        // Java: 40
        public int getStrippedLength()
        {
            return stripped_length;
        }
        public RegistrationKeyFormatException(int type) : base()
        {
            this.type = type;
        }
        public RegistrationKeyFormatException(int type, char invalid_char) : this(type)
        {
            this.invalid_char = invalid_char;
        }
        public RegistrationKeyFormatException(int type, int stripped_length) : this(type)
        {
            this.stripped_length = stripped_length;
        }
    }
}
