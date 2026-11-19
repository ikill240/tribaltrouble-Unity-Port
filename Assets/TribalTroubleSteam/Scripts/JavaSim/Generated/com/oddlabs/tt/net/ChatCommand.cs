// Translated from ChatCommand.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.net
{
    // Java: 13
    public class ChatCommand : java.lang.Object
    {
        public static java.util.Map commands;
        public static java.util.Set ignored_nicks;
        public static java.util.ResourceBundle bundle;
        // Java: 29
        public static string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.net.ChatCommand.bundle, key, args);
        }
        // Java: 33
        public static bool filterCommand(com.oddlabs.tt.gui.InfoPrinter info_printer, string text)
        {
            return com.oddlabs.tt.net.ChatCommand.filterCommand(info_printer, java.util.Collections.emptyMap(), text);
        }
        // Java: 37
        public static bool filterCommand(com.oddlabs.tt.gui.InfoPrinter info_printer, java.util.Map custom_commands, string text)
        {
            if (!text.startsWith("/"))
            {
                return false;
            }
            int fist_space = com.oddlabs.tt.net.ChatCommand.firstSpace(text);
            string cmd = text.substring(1, fist_space);
            string args = text.substring(fist_space).trim();
            com.oddlabs.tt.net.ChatMethod method = ((com.oddlabs.tt.net.ChatMethod)custom_commands.getOrDefault(cmd, ((com.oddlabs.tt.net.ChatMethod)com.oddlabs.tt.net.ChatCommand.commands.get(cmd))));
            if (method != null)
            {
                method.execute(info_printer, args);
            }
            else
            {
                string unknown_cmd_message = com.oddlabs.tt.net.ChatCommand.i18n("unknown_command", cmd);
                info_printer.print(unknown_cmd_message);
            }
            return true;
        }
        // Java: 54
        public static int firstSpace(string text)
        {
            int first_space = text.indexOf(' ');
            return (first_space == (-1) ? text.length() : first_space);
        }
        // Java: 59
        public static void sendMessage(com.oddlabs.tt.gui.InfoPrinter info_printer, string text)
        {
            int first_space = com.oddlabs.tt.net.ChatCommand.firstSpace(text);
            string nick = text.substring(0, first_space);
            string message = text.substring(first_space).trim();
            if (!com.oddlabs.tt.net.Network.getMatchmakingClient().isConnected())
            {
                info_printer.print(com.oddlabs.tt.net.ChatCommand.i18n("not_connected"));
            }
            else
            {
                com.oddlabs.tt.net.Network.getMatchmakingClient().sendPrivateMessage(info_printer.getGUIRoot(), nick, message);
            }
        }
        // Java: 69
        public static void getInfo(com.oddlabs.tt.gui.InfoPrinter info_printer, string text)
        {
            int first_space = com.oddlabs.tt.net.ChatCommand.firstSpace(text);
            string nick = text.substring(0, first_space);
            if (!com.oddlabs.tt.net.Network.getMatchmakingClient().isConnected())
            {
                info_printer.print(com.oddlabs.tt.net.ChatCommand.i18n("not_connected"));
            }
            else
            {
                com.oddlabs.tt.net.Network.getMatchmakingClient().requestInfo(info_printer.getGUIRoot(), nick);
            }
        }
        // Java: 78
        public static void ignore(com.oddlabs.tt.gui.InfoPrinter info_printer, string text)
        {
            int first_space = com.oddlabs.tt.net.ChatCommand.firstSpace(text);
            string nick = text.substring(0, first_space);
            bool result = com.oddlabs.tt.net.ChatCommand.ignored_nicks.add(nick.toLowerCase());
            if (result)
            {
                string msg = com.oddlabs.tt.net.ChatCommand.i18n("ignoring", nick);
                info_printer.print(msg);
            }
        }
        // Java: 88
        public static void unignore(com.oddlabs.tt.gui.InfoPrinter info_printer, string text)
        {
            int first_space = com.oddlabs.tt.net.ChatCommand.firstSpace(text);
            string nick = text.substring(0, first_space);
            bool result = com.oddlabs.tt.net.ChatCommand.ignored_nicks.remove(nick.toLowerCase());
            if (result)
            {
                string msg = com.oddlabs.tt.net.ChatCommand.i18n("unignoring", nick);
                info_printer.print(msg);
            }
        }
        // Java: 98
        public static bool isIgnoring(string nick)
        {
            return com.oddlabs.tt.net.ChatCommand.ignored_nicks.contains(nick.toLowerCase());
        }
        // Java: 102
        public static void ignoreList(com.oddlabs.tt.gui.InfoPrinter info_printer, string text)
        {
            string[] nicks = new string[com.oddlabs.tt.net.ChatCommand.ignored_nicks.size()];
            com.oddlabs.tt.net.ChatCommand.ignored_nicks.toArray(nicks);
            string result = default;
            if (nicks.Length == 0)
            {
                result = com.oddlabs.tt.net.ChatCommand.i18n("ignore_list_empty");
            }
            else
            {
                result = com.oddlabs.tt.net.ChatCommand.i18n("ignore_list");
                result += " " + java.lang.JStr.valueOf(java.lang.JString.join(" ", nicks));
            }
            info_printer.print(result);
        }
        static ChatCommand()
        {
            ChatCommand.commands = java.util.Map_.of("message", new com.oddlabs.tt.net.ChatMethod_L((com.oddlabs.tt.gui.InfoPrinter a0, string a1) => { com.oddlabs.tt.net.ChatCommand.sendMessage(a0, a1); }), "msg", new com.oddlabs.tt.net.ChatMethod_L((com.oddlabs.tt.gui.InfoPrinter a0, string a1) => { com.oddlabs.tt.net.ChatCommand.sendMessage(a0, a1); }), "tell", new com.oddlabs.tt.net.ChatMethod_L((com.oddlabs.tt.gui.InfoPrinter a0, string a1) => { com.oddlabs.tt.net.ChatCommand.sendMessage(a0, a1); }), "whisper", new com.oddlabs.tt.net.ChatMethod_L((com.oddlabs.tt.gui.InfoPrinter a0, string a1) => { com.oddlabs.tt.net.ChatCommand.sendMessage(a0, a1); }), "info", new com.oddlabs.tt.net.ChatMethod_L((com.oddlabs.tt.gui.InfoPrinter a0, string a1) => { com.oddlabs.tt.net.ChatCommand.getInfo(a0, a1); }), "finger", new com.oddlabs.tt.net.ChatMethod_L((com.oddlabs.tt.gui.InfoPrinter a0, string a1) => { com.oddlabs.tt.net.ChatCommand.getInfo(a0, a1); }), "ignore", new com.oddlabs.tt.net.ChatMethod_L((com.oddlabs.tt.gui.InfoPrinter a0, string a1) => { com.oddlabs.tt.net.ChatCommand.ignore(a0, a1); }), "unignore", new com.oddlabs.tt.net.ChatMethod_L((com.oddlabs.tt.gui.InfoPrinter a0, string a1) => { com.oddlabs.tt.net.ChatCommand.unignore(a0, a1); }), "ignorelist", new com.oddlabs.tt.net.ChatMethod_L((com.oddlabs.tt.gui.InfoPrinter a0, string a1) => { com.oddlabs.tt.net.ChatCommand.ignoreList(a0, a1); }));
            ChatCommand.ignored_nicks = new java.util.HashSet();
            ChatCommand.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.net.ChatCommand).getName());
        }
        public ChatCommand() : base()
        {
        }
    }
}
