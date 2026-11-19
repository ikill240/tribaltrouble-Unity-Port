// Translated from ChatRoom.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchserver
{
    // Java: 13
    public class ChatRoom : java.lang.Object
    {
        public static java.util.Map chat_rooms;
        public java.util.Set users;
        public string name;
        // Java: 24
        public static java.util.Map getChatRooms()
        {
            return com.oddlabs.matchserver.ChatRoom.chat_rooms;
        }
        // Java: 28
        public static void joinStandardChatRoom(com.oddlabs.matchserver.Client client)
        {
            int room_postfix = 0;
            while (true)
            {
                room_postfix++;
                com.oddlabs.matchserver.ChatRoom room = com.oddlabs.matchserver.ChatRoom.getChatRoom("Chatroom" + (room_postfix));
                if (room.getUsers().size() > com.oddlabs.matchmaking.MatchmakingServerInterface.MAX_ROOM_USERS / 2)
                {
                    continue;
                }
                client.joinRoom(room.getName());
                return;
            }
        }
        // Java: 42
        public static com.oddlabs.matchserver.ChatRoom getChatRoom(string room_name)
        {
            com.oddlabs.matchserver.ChatRoom room = ((com.oddlabs.matchserver.ChatRoom)com.oddlabs.matchserver.ChatRoom.chat_rooms.get(room_name));
            if (room == null)
            {
                room = new com.oddlabs.matchserver.ChatRoom(room_name);
                com.oddlabs.matchserver.ChatRoom.chat_rooms.put(room_name, room);
            }
            return room;
        }
        // Java: 51
        public static bool isNameValid(string name)
        {
            return name != null && name.length() <= com.oddlabs.matchmaking.MatchmakingServerInterface.MAX_ROOM_NAME_LENGTH && name.length() >= com.oddlabs.matchmaking.MatchmakingServerInterface.MIN_ROOM_NAME_LENGTH && com.oddlabs.matchserver.ChatRoom.areCharactersValid(name);
        }
        // Java: 56
        public static bool areCharactersValid(string name)
        {
            for (int i = 0; i < name.length(); i++)
            {
                if (com.oddlabs.matchmaking.MatchmakingServerInterface.ALLOWED_ROOM_CHARS.indexOf(name.charAt(i)) == (-1))
                {
                    return false;
                }
            }
            return true;
        }
        // Java: 63
        public bool join(com.oddlabs.matchserver.Client client)
        {
            if (users.size() >= com.oddlabs.matchmaking.MatchmakingServerInterface.MAX_ROOM_USERS)
            {
                return false;
            }
            users.add(client);
            return true;
        }
        // Java: 71
        public void sendUsers()
        {
            java.util.Iterator it = users.iterator();
            com.oddlabs.matchmaking.ChatRoomUser[] chat_room_users = new com.oddlabs.matchmaking.ChatRoomUser[users.size()];
            int i = 0;
            while (it.hasNext())
            {
                com.oddlabs.matchserver.Client client = ((com.oddlabs.matchserver.Client)it.next());
                chat_room_users[i] = new com.oddlabs.matchmaking.ChatRoomUser(client.getProfile().getNick(), client.isPlaying());
                i++;
            }
            it = users.iterator();
            while (it.hasNext())
            {
                com.oddlabs.matchserver.Client client_2 = ((com.oddlabs.matchserver.Client)it.next());
                client_2.getClientInterface().receiveChatRoomUsers(chat_room_users);
            }
        }
        // Java: 87
        public void sendMessage(string msg, string owner)
        {
            java.util.Iterator it = users.iterator();
            while (it.hasNext())
            {
                com.oddlabs.matchserver.Client client = ((com.oddlabs.matchserver.Client)it.next());
                client.getClientInterface().receiveChatRoomMessage(msg, owner);
            }
        }
        // Java: 95
        public java.util.Set getUsers()
        {
            return users;
        }
        // Java: 99
        public void leave(com.oddlabs.matchserver.Client client)
        {
            if (users.contains(client))
            {
                users.remove(client);
                if (users.size() == 0)
                {
                    com.oddlabs.matchserver.discord.DiscordBotService.getInstance().getChatroomCoordinator().ifPresent(new java.util.function.Consumer_L((object x_l) =>
                    {
                        com.oddlabs.matchserver.discord.DiscordChatroomCoordinator x = (com.oddlabs.matchserver.discord.DiscordChatroomCoordinator)x_l;
                        x.removeChatroom(this);
                    }));
                    com.oddlabs.matchserver.ChatRoom.chat_rooms.remove(getName());
                }
                else
                {
                    sendUsers();
                }
            }
        }
        // Java: 111
        public string getName()
        {
            return name;
        }
        static ChatRoom()
        {
            ChatRoom.chat_rooms = new java.util.HashMap();
        }
        private void init_ChatRoom()
        {
            this.users = new java.util.HashSet();
        }
        public ChatRoom(string name) : base()
        {
            init_ChatRoom();
            this.name = name;
            com.oddlabs.matchserver.discord.DiscordBotService.getInstance().getChatroomCoordinator().ifPresent(new java.util.function.Consumer_L((object x_l) =>
            {
                com.oddlabs.matchserver.discord.DiscordChatroomCoordinator x = (com.oddlabs.matchserver.discord.DiscordChatroomCoordinator)x_l;
                x.addChatroom(this);
            }));
        }
    }
}
