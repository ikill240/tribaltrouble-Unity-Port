// Translated from ChatRoomInfo.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 7
    public class ChatRoomInfo : java.lang.Object
    {
        public string name() { return name_f; }
        public com.oddlabs.matchmaking.ChatRoomUser[] users() { return users_f; }
        public ChatRoomInfo(string name, com.oddlabs.matchmaking.ChatRoomUser[] users)
        {
            this.name_f = name;
            this.users_f = users;
        }
        public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is ChatRoomInfo r)) return false; return true && java.lang.JO.equals(name_f, r.name_f) && java.lang.JO.equals(users_f, r.users_f); }
        public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(name_f); h = 31 * h + java.lang.JO.hashCode(users_f); } return h; }
        public override string toString() { return "ChatRoomInfo[" + "name=" + java.lang.JO.toString(name_f) + ", users=" + java.lang.JO.toString(users_f) + "]"; }
        public string name_f;
        public com.oddlabs.matchmaking.ChatRoomUser[] users_f;
    }
}
