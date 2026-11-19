// Translated from ChatRoomEntry.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchmaking
{
    // Java: 6
    public class ChatRoomEntry : java.lang.Object
    {
        public const long serialVersionUID = 1L;
        public string name;
        public int num_joined;
        // Java: 18
        public string getName()
        {
            return name;
        }
        // Java: 22
        public int getNumJoined()
        {
            return num_joined;
        }
        public ChatRoomEntry(string name, int num_joined) : base()
        {
            this.name = name;
            this.num_joined = num_joined;
        }
    }
}
