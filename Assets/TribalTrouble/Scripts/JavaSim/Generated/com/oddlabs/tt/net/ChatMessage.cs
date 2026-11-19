// Translated from ChatMessage.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.net
{
    // Java: 6
    public class ChatMessage : java.lang.Object
    {
        public string nick() { return nick_f; }
        public string message() { return message_f; }
        public com.oddlabs.tt.net.ChatMessage.Type type() { return type_f; }
        public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is ChatMessage r)) return false; return true && java.lang.JO.equals(nick_f, r.nick_f) && java.lang.JO.equals(message_f, r.message_f) && java.lang.JO.equals(type_f, r.type_f); }
        public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(nick_f); h = 31 * h + java.lang.JO.hashCode(message_f); h = 31 * h + java.lang.JO.hashCode(type_f); } return h; }
        public override string toString() { return "ChatMessage[" + "nick=" + java.lang.JO.toString(nick_f) + ", message=" + java.lang.JO.toString(message_f) + ", type=" + java.lang.JO.toString(type_f) + "]"; }
        public string nick_f;
        public string message_f;
        public com.oddlabs.tt.net.ChatMessage.Type type_f;
        // Java: 7
        public sealed class Type : java.lang.Enum
        {
            public static Type[] values() { return (Type[])VALUES_.Clone(); }
            public static Type valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly Type NORMAL = new Type("NORMAL", 0);
            public static readonly Type TEAM = new Type("TEAM", 1);
            public static readonly Type PRIVATE = new Type("PRIVATE", 2);
            public static readonly Type CHATROOM = new Type("CHATROOM", 3);
            public static readonly Type GAME_MENU = new Type("GAME_MENU", 4);
            public static readonly Type[] VALUES_ = new Type[] { NORMAL, TEAM, PRIVATE, CHATROOM, GAME_MENU };
            public Type(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        // Java: 21
        public string formatShort()
        {
            return "<" + java.lang.JStr.valueOf(nick_f) + "> " + java.lang.JStr.valueOf(message_f);
        }
        // Java: 25
        public string formatLong()
        {
            return ((type_f).ordinal() switch { 1 /*TEAM*/ => (string)("(Team) " + java.lang.JStr.valueOf(formatShort())), 2 /*PRIVATE*/ => (string)("(Private) " + java.lang.JStr.valueOf(formatShort())), 0 /*NORMAL*/ or 3 /*CHATROOM*/ or 4 /*GAME_MENU*/ => (string)(formatShort()), _ => throw new System.InvalidOperationException("MatchException") });
        }
        public ChatMessage(string nick_v, string message_v, com.oddlabs.tt.net.ChatMessage.Type type_v) : base()
        {
            this.nick_f = nick_v;
            this.message_f = com.oddlabs.tt.util.SpamFilter.scan(message_v);
            this.type_f = type_v;
        }
    }
}
