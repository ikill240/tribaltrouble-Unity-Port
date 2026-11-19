// Translated from ChatRoomUser.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchmaking
{
    // Java: 9
    public class ChatRoomUser : java.lang.Object
    {
        public const long serialVersionUID = 1L;
        public string nick;
        public bool playing;
        // Java: 21
        public string getNick()
        {
            return nick;
        }
        // Java: 25
        public bool isPlaying()
        {
            return playing;
        }
        // Java: 29
        public override int hashCode()
        {
            return java.lang.JO.hashCode(nick);
        }
        // Java: 34
        public override bool equals(object other)
        {
            return (other is com.oddlabs.matchmaking.ChatRoomUser) && java.lang.JO.equals((((com.oddlabs.matchmaking.ChatRoomUser)other)).nick, nick);
        }
        public ChatRoomUser(string nick, bool playing) : base()
        {
            this.nick = nick;
            this.playing = playing;
        }
    }
}
