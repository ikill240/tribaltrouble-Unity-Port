#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.registration
{
    public sealed class RegistrationKeyFormatException : System.FormatException
    {
        public const int TYPE_INVALID_CHAR = 0, TYPE_INVALID_LENGTH = 1, TYPE_INVALID_KEY = 2;
        readonly int type;
        int stripped_length;
        char invalid_char;
        public RegistrationKeyFormatException(int type) : base("RegistrationKeyFormatException " + type) { this.type = type; }
        public RegistrationKeyFormatException(int type, char invalid_char) : this(type) { this.invalid_char = invalid_char; }
        public RegistrationKeyFormatException(int type, int stripped_length) : this(type) { this.stripped_length = stripped_length; }
        public int getType() => type;
        public char getInvalidChar() => invalid_char;
        public int getStrippedLength() => stripped_length;
        // ---- merged from the generated stub (members the translated code now uses)
        public RegistrationKeyFormatException() { }
    }
}
