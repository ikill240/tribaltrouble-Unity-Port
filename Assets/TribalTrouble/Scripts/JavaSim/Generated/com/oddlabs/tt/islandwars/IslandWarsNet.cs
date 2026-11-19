// Translated from IslandWarsNet.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.islandwars
{
    // Java: 41
    public class IslandWarsNet : java.lang.Object
    {
        public const string P = "~iw~";
        public const string ROOM = "IslandWars";
        public const int CHUNK = 360;
        public const float JOIN_WAIT = 45.0f;
        public static com.oddlabs.tt.form.SelectGameMenu lobby_f;
        public static com.oddlabs.tt.gui.GUIRoot gui_root;
        public static com.oddlabs.net.NetworkSelector network;
        public static java.util.Map infos_f;
        public static java.lang.Runnable on_change;
        public static string notice_f;
        public static string raid_target;
        public static java.util.Map inbox;
        public static com.oddlabs.tt.islandwars.IslandWarsSetup hosting;
        public static com.oddlabs.tt.net.GameNetwork hosted;
        public static float host_wait;
        public static bool islander_busy;
        public static com.oddlabs.tt.animation.TimerAnimation timer;
        public static string join_name;
        public static bool joining;
        public static java.util.Map match_islands;
        public static com.oddlabs.tt.net.GameNetwork match_network;
        // Java: 76
        public static void setLobby(com.oddlabs.tt.form.SelectGameMenu menu, com.oddlabs.tt.gui.GUIRoot root, com.oddlabs.net.NetworkSelector net)
        {
            com.oddlabs.tt.islandwars.IslandWarsNet.lobby_f = menu;
            com.oddlabs.tt.islandwars.IslandWarsNet.gui_root = root;
            com.oddlabs.tt.islandwars.IslandWarsNet.network = net;
        }
        // Java: 82
        public static void setOnChange(java.lang.Runnable r)
        {
            com.oddlabs.tt.islandwars.IslandWarsNet.on_change = r;
        }
        public static java.lang.Runnable on_close;
        // Java: 89
        public static void setOnClose(java.lang.Runnable r)
        {
            com.oddlabs.tt.islandwars.IslandWarsNet.on_close = r;
        }
        // Java: 93
        public static void changed()
        {
            java.lang.Runnable r = com.oddlabs.tt.islandwars.IslandWarsNet.on_change;
            if (r != null)
            {
                r.run();
            }
        }
        // Java: 99
        public static java.util.Map infos()
        {
            return com.oddlabs.tt.islandwars.IslandWarsNet.infos_f;
        }
        // Java: 104
        public static string notice()
        {
            return com.oddlabs.tt.islandwars.IslandWarsNet.notice_f;
        }
        // Java: 108
        public static string myNick()
        {
            com.oddlabs.matchmaking.Profile p = com.oddlabs.tt.net.Network.getMatchmakingClient().getProfile();
            return (p == null ? (string)(null) : p.getNick());
        }
        // Java: 113
        public static com.oddlabs.tt.islandwars.HomeIsland myIsland()
        {
            string nick = com.oddlabs.tt.islandwars.IslandWarsNet.myNick();
            return com.oddlabs.tt.islandwars.HomeIsland.load((nick != null ? nick : "local"), 0);
        }
        // Java: 118
        public static void send(string nick, string msg)
        {
            if (com.oddlabs.tt.net.Network.getMatchmakingClient().getInterface() != null)
            {
                com.oddlabs.tt.net.Network.getMatchmakingClient().getInterface().sendPrivateMessage(nick, java.lang.JStr.valueOf(com.oddlabs.tt.islandwars.IslandWarsNet.P) + java.lang.JStr.valueOf(msg));
            }
        }
        // Java: 123
        public static string hex(string s)
        {
            return com.oddlabs.tt.islandwars.Sha256.hex(com.oddlabs.tt.islandwars.Sha256.utf8(s));
        }
        // Java: 127
        public static string unhex(string h)
        {
            sbyte[] b = new sbyte[h.length() / 2];
            for (int i = 0; i < b.Length; i++)
            {
                b[i] = unchecked((sbyte)(java.lang.Integer.parseInt(h.substring(2 * i, 2 * i + 2), 16)));
            }
            return com.oddlabs.tt.islandwars.Sha256.fromUtf8(b);
        }
        // Java: 136
        public static void ping(string nick)
        {
            com.oddlabs.tt.islandwars.IslandWarsNet.send(nick, "ping");
        }
        // Java: 141
        public static void requestRaid(string nick)
        {
            com.oddlabs.tt.islandwars.IslandWarsNet.raid_target = nick;
            com.oddlabs.tt.islandwars.IslandWarsNet.inbox.remove(nick.toLowerCase());
            com.oddlabs.tt.islandwars.IslandWarsNet.notice_f = "Asking " + java.lang.JStr.valueOf(nick) + "'s island for a raid...";
            com.oddlabs.tt.islandwars.IslandWarsNet.send(nick, "raid");
            com.oddlabs.tt.islandwars.IslandWarsNet.changed();
        }
        // Java: 150
        public static bool privateMessage(string nick, string msg)
        {
            if (!msg.startsWith(com.oddlabs.tt.islandwars.IslandWarsNet.P))
            {
                return false;
            }
            string me = com.oddlabs.tt.islandwars.IslandWarsNet.myNick();
            if (me != null && me.equalsIgnoreCase(nick))
            {
                return true;
            }
            try
            {
                com.oddlabs.tt.islandwars.IslandWarsNet.handle(nick, msg.substring(com.oddlabs.tt.islandwars.IslandWarsNet.P.length()).trim());
            }
            catch (System.Exception e)
            {
                java.lang.IO.println("Island Wars: bad message from " + java.lang.JStr.valueOf(nick) + ": " + java.lang.JStr.valueOf(e));
            }
            return true;
        }
        // Java: 164
        public static bool busy()
        {
            return com.oddlabs.tt.viewer.WorldViewer.open_viewers > 0 || com.oddlabs.tt.islandwars.IslandWarsNet.lobby_f == null || com.oddlabs.tt.islandwars.IslandWarsNet.lobby_f.isInGameLobby();
        }
        // Java: 168
        public static void handle(string nick, string msg)
        {
            string[] p = msg.split(" ");
            long now = java.lang.JSystem.currentTimeMillis();
            switch ((p[0]))
            {
                case "ping":
                {
                    com.oddlabs.tt.islandwars.HomeIsland h = com.oddlabs.tt.islandwars.IslandWarsNet.myIsland();
                    int level = 0;
                    foreach (int u in h.upgrades)
                    {
                        level += u;
                    }
                    com.oddlabs.tt.islandwars.IslandWarsNet.send(nick, "info " + (h.shield_until) + " " + (h.size) + " " + (level) + " " + (((com.oddlabs.tt.islandwars.IslandWarsNet.busy() ? 1 : 0))));
                    break;
                }
                case "info":
                {
                    com.oddlabs.tt.islandwars.IslandWarsNet.infos_f.put(nick, msg.substring(5));
                    com.oddlabs.tt.islandwars.IslandWarsNet.changed();
                    break;
                }
                case "raid":
                {
                    com.oddlabs.tt.islandwars.HomeIsland h_2 = com.oddlabs.tt.islandwars.IslandWarsNet.myIsland();
                    if (h_2.isShielded(now))
                    {
                        com.oddlabs.tt.islandwars.IslandWarsNet.send(nick, "shield " + (h_2.shield_until));
                        return;
                    }
                    if (h_2.isTampered())
                    {
                        com.oddlabs.tt.islandwars.IslandWarsNet.send(nick, "no edited");
                        return;
                    }
                    string text = com.oddlabs.tt.islandwars.IslandWarsNet.hex(h_2.encode());
                    int n = (text.length() + com.oddlabs.tt.islandwars.IslandWarsNet.CHUNK - 1) / com.oddlabs.tt.islandwars.IslandWarsNet.CHUNK;
                    for (int i = 0; i < n; i++)
                    {
                        com.oddlabs.tt.islandwars.IslandWarsNet.send(nick, "isl " + (i) + " " + (n) + " " + java.lang.JStr.valueOf(text.substring(i * com.oddlabs.tt.islandwars.IslandWarsNet.CHUNK, java.lang.Math.min(text.length(), (i + 1) * com.oddlabs.tt.islandwars.IslandWarsNet.CHUNK))));
                    }
                    com.oddlabs.tt.islandwars.IslandWarsNet.notice_f = java.lang.JStr.valueOf(nick) + " is setting out to raid your island!";
                    com.oddlabs.tt.islandwars.IslandWarsNet.changed();
                    break;
                }
                case "isl":
                {
                    if (com.oddlabs.tt.islandwars.IslandWarsNet.raid_target == null || !com.oddlabs.tt.islandwars.IslandWarsNet.raid_target.equalsIgnoreCase(nick))
                    {
                        return;
                    }
                    int i_2 = java.lang.Integer.parseInt(p[1]);
                    int n_2 = java.lang.Integer.parseInt(p[2]);
                    if (n_2 <= 0 || n_2 > 400 || i_2 < 0 || i_2 >= n_2)
                    {
                        return;
                    }
                    string[] parts = ((string[])com.oddlabs.tt.islandwars.IslandWarsNet.inbox.computeIfAbsent(nick.toLowerCase(), new java.util.function.Function_L((object k_l) =>
                    {
                        string k = (string)k_l;
                        return new string[n_2];
                    })));
                    if (parts.Length != n_2)
                    {
                        return;
                    }
                    parts[i_2] = (p.Length > 3 ? p[3] : "");
                    foreach (string s in parts)
                    {
                        if (s == null)
                        {
                            return;
                        }
                    }
                    com.oddlabs.tt.islandwars.IslandWarsNet.inbox.remove(nick.toLowerCase());
                    com.oddlabs.tt.islandwars.IslandWarsNet.raid_target = null;
                    com.oddlabs.tt.islandwars.IslandWarsNet.hostRaid(nick, com.oddlabs.tt.islandwars.HomeIsland.fromPeer(com.oddlabs.tt.islandwars.IslandWarsNet.unhex(java.lang.JString.join("", parts))));
                    break;
                }
                case "shield":
                {
                    long until = java.lang.Long.parseLong(p[1]);
                    com.oddlabs.tt.islandwars.IslandWarsNet.notice_f = java.lang.JStr.valueOf(nick) + "'s island is shielded for another " + (java.lang.Math.max(1, (until - now) / 3600000L)) + " h.";
                    com.oddlabs.tt.islandwars.IslandWarsNet.raid_target = null;
                    com.oddlabs.tt.islandwars.IslandWarsNet.changed();
                    break;
                }
                case "no":
                {
                    com.oddlabs.tt.islandwars.IslandWarsNet.notice_f = java.lang.JStr.valueOf(nick) + "'s island can't be raided (" + java.lang.JStr.valueOf(((p.Length > 1 ? p[1] : "?"))) + ").";
                    com.oddlabs.tt.islandwars.IslandWarsNet.raid_target = null;
                    com.oddlabs.tt.islandwars.IslandWarsNet.changed();
                    break;
                }
                case "go":
                {
                    string name = msg.substring(3);
                    com.oddlabs.tt.islandwars.IslandWarsNet.notice_f = java.lang.JStr.valueOf(nick) + " is raiding your island!";
                    if (com.oddlabs.tt.islandwars.IslandWarsNet.busy())
                    {
                        com.oddlabs.tt.islandwars.IslandWarsNet.send(nick, "busy");
                    }
                    else
                    {
                        com.oddlabs.tt.islandwars.IslandWarsNet.join_name = name;
                        com.oddlabs.tt.islandwars.IslandWarsNet.joining = false;
                        com.oddlabs.tt.net.Network.getMatchmakingClient().requestList(com.oddlabs.matchmaking.MatchmakingServerInterface.TYPE_GAME);
                    }
                    com.oddlabs.tt.islandwars.IslandWarsNet.changed();
                    break;
                }
                case "busy":
                {
                    com.oddlabs.tt.islandwars.IslandWarsNet.islander_busy = true;
                    break;
                }
                default:
                {
                    break;
                }
            }
        }
        // Java: 247
        public static void gamesListed(object[] names)
        {
            if (com.oddlabs.tt.islandwars.IslandWarsNet.join_name == null || com.oddlabs.tt.islandwars.IslandWarsNet.lobby_f == null)
            {
                return;
            }
            foreach (object o in names)
            {
                if ((o is com.oddlabs.matchmaking.GameHost h) && java.lang.JO.equals(h.getGame().getName(), com.oddlabs.tt.islandwars.IslandWarsNet.join_name))
                {
                    com.oddlabs.tt.islandwars.IslandWarsNet.join_name = null;
                    com.oddlabs.tt.islandwars.IslandWarsNet.joining = true;
                    com.oddlabs.tt.islandwars.IslandWarsNet.lobby_f.joinIslandWarsGame(h);
                    return;
                }
            }
        }
        // Java: 261
        public static void hostRaid(string islander, com.oddlabs.tt.islandwars.HomeIsland island)
        {
            if (com.oddlabs.tt.islandwars.IslandWarsNet.lobby_f == null || com.oddlabs.tt.islandwars.IslandWarsNet.gui_root == null || com.oddlabs.tt.islandwars.IslandWarsNet.network == null || com.oddlabs.tt.islandwars.IslandWarsNet.busy())
            {
                return;
            }
            string problem = island.audit();
            com.oddlabs.tt.islandwars.HomeIsland me = com.oddlabs.tt.islandwars.IslandWarsNet.myIsland();
            com.oddlabs.tt.islandwars.IslandWarsSetup s = com.oddlabs.tt.islandwars.IslandWarsSetup.raid(island, me.upgrades, (com.oddlabs.tt.islandwars.IslandWarsNet.myNick() != null ? com.oddlabs.tt.islandwars.IslandWarsNet.myNick() : "raider"), problem != null);
            com.oddlabs.tt.islandwars.IslandWarsNet.notice_f = (problem == null ? "Raiding " + java.lang.JStr.valueOf(islander) + "'s island!" : "Raiding " + java.lang.JStr.valueOf(islander) + "'s island - it doesn't add up (" + java.lang.JStr.valueOf(problem) + "), so its upgrades don't count.");
            string name = "Raid: " + java.lang.JStr.valueOf(com.oddlabs.tt.islandwars.IslandWarsNet.myNick()) + " on " + java.lang.JStr.valueOf(islander);
            com.oddlabs.matchmaking.Game game = com.oddlabs.matchmaking.Game.builder().name(name).size(unchecked((sbyte)(java.lang.Math.min(3, island.size)))).terrain(unchecked((sbyte)(island.terrain))).hills(unchecked((sbyte)(5))).trees(unchecked((sbyte)(6))).supplies(unchecked((sbyte)(5))).rated(false).gamespeed(unchecked((sbyte)(2))).mapcode("island-wars").randomStartPos(0.0f).maxUnitCount(1000).build();
            com.oddlabs.tt.landscape.WorldParameters @params = com.oddlabs.tt.landscape.WorldParameters.builder().initialGameSpeed(game.getGamespeed()).mapcode("island-wars").initialUnitCount(6).maxUnitCount(1000).mapSize(java.lang.Math.min(3, island.size)).mode(com.oddlabs.matchmaking.GameMode.ISLAND_WARS).maxBuildingCount(200).extended(true).islandWars(s.encode()).build();
            string[] ai_names = new string[com.oddlabs.matchmaking.MatchmakingServerInterface.MAX_PLAYERS];
            for (int i = 0; i < ai_names.Length; i++)
            {
                ai_names[i] = island.nick;
            }
            com.oddlabs.tt.procedural.Landscape.TerrainType terrain = (island.terrain == 1 ? com.oddlabs.tt.procedural.Landscape.TerrainType.VIKING : com.oddlabs.tt.procedural.Landscape.TerrainType.NATIVE);
            java.lang.Runnable close = com.oddlabs.tt.islandwars.IslandWarsNet.on_close;
            com.oddlabs.tt.islandwars.IslandWarsNet.on_change = null;
            com.oddlabs.tt.islandwars.IslandWarsNet.on_close = null;
            if (close != null)
            {
                close.run();
            }
            com.oddlabs.tt.islandwars.IslandWarsNet.hosted = com.oddlabs.tt.@delegate.Menu.startNewGame(com.oddlabs.tt.islandwars.IslandWarsNet.network, com.oddlabs.tt.islandwars.IslandWarsNet.gui_root, com.oddlabs.tt.islandwars.IslandWarsNet.lobby_f, @params, new com.oddlabs.tt.viewer.MultiplayerInGameInfo(0.0f, false), new com.oddlabs.tt.@delegate.Menu.DefaultWorldInitAction(), game, island.metersPerWorld(), terrain, 0.5f, 0.6f, 0.5f, island.seed, false, ai_names, 2);
            com.oddlabs.tt.islandwars.IslandWarsNet.hosting = s;
            com.oddlabs.tt.islandwars.IslandWarsNet.host_wait = 0.0f;
            com.oddlabs.tt.islandwars.IslandWarsNet.islander_busy = false;
            com.oddlabs.tt.islandwars.IslandWarsNet.hosted.getClient().getServerInterface().setPlayerSlot(0, com.oddlabs.tt.net.PlayerSlot.HUMAN, me.race, 0, false, com.oddlabs.tt.net.PlayerSlot.AI_NONE);
            com.oddlabs.tt.islandwars.IslandWarsNet.send(islander, "go " + java.lang.JStr.valueOf(name));
            com.oddlabs.tt.islandwars.IslandWarsNet.startTimer();
        }
        // Java: 296
        public static void startTimer()
        {
            if (com.oddlabs.tt.islandwars.IslandWarsNet.timer == null)
            {
                com.oddlabs.tt.islandwars.IslandWarsNet.timer = new com.oddlabs.tt.animation.TimerAnimation(new IslandWarsNet.Anon1_Updatable(), 1.0f);
            }
            com.oddlabs.tt.islandwars.IslandWarsNet.timer.start();
        }
        // Java: 308
        public static void tick()
        {
            com.oddlabs.tt.islandwars.IslandWarsSetup s = com.oddlabs.tt.islandwars.IslandWarsNet.hosting;
            com.oddlabs.tt.net.GameNetwork gn = com.oddlabs.tt.islandwars.IslandWarsNet.hosted;
            if (s == null || gn == null)
            {
                if (com.oddlabs.tt.islandwars.IslandWarsNet.timer != null)
                {
                    com.oddlabs.tt.islandwars.IslandWarsNet.timer.stop();
                }
                return;
            }
            com.oddlabs.tt.islandwars.IslandWarsNet.host_wait += 1.0f;
            if (com.oddlabs.tt.islandwars.IslandWarsNet.islander_busy || com.oddlabs.tt.islandwars.IslandWarsNet.host_wait >= com.oddlabs.tt.islandwars.IslandWarsNet.JOIN_WAIT)
            {
                com.oddlabs.tt.islandwars.IslandWarsNet.startRaid(gn, s, false);
            }
        }
        // Java: 322
        public static void lobby(com.oddlabs.tt.form.GameMenu menu, com.oddlabs.tt.net.GameNetwork gn, com.oddlabs.tt.net.PlayerSlot[] players, int local)
        {
            com.oddlabs.tt.landscape.WorldParameters wp = gn.getClient().getWorldParameters();
            if (wp == null || ((object)wp.getGameMode() != (object)com.oddlabs.matchmaking.GameMode.ISLAND_WARS))
            {
                return;
            }
            com.oddlabs.tt.islandwars.IslandWarsSetup setup = com.oddlabs.tt.islandwars.IslandWarsSetup.decode(wp.getIslandWars());
            if (setup != null && setup.kind == com.oddlabs.tt.islandwars.IslandWarsSetup.RAID)
            {
                if (local == setup.defender && com.oddlabs.tt.islandwars.IslandWarsNet.joining && !players[local].isReady())
                {
                    com.oddlabs.tt.islandwars.IslandWarsNet.joining = false;
                    gn.getClient().getServerInterface().setPlayerSlot(local, com.oddlabs.tt.net.PlayerSlot.HUMAN, java.lang.Math.clamp(setup.race, 0, 1), 1, true, com.oddlabs.tt.net.PlayerSlot.AI_NONE);
                }
                if (local == 0 && com.oddlabs.tt.islandwars.IslandWarsNet.hosting != null && ((object)com.oddlabs.tt.islandwars.IslandWarsNet.hosted == (object)gn) && players.Length > 1 && players[1].getType() == com.oddlabs.tt.net.PlayerSlot.HUMAN && players[1].isReady())
                {
                    com.oddlabs.tt.islandwars.IslandWarsNet.startRaid(gn, com.oddlabs.tt.islandwars.IslandWarsNet.hosting, true);
                }
                return;
            }
            if (((object)com.oddlabs.tt.islandwars.IslandWarsNet.match_network != (object)gn))
            {
                com.oddlabs.tt.islandwars.IslandWarsNet.match_network = gn;
                com.oddlabs.tt.islandwars.IslandWarsNet.match_islands.clear();
                gn.getClient().getServerInterface().chat(java.lang.JStr.valueOf(com.oddlabs.tt.islandwars.IslandWarsNet.P) + "u " + java.lang.JStr.valueOf(com.oddlabs.tt.islandwars.IslandWarsNet.hex(com.oddlabs.tt.islandwars.IslandWarsNet.myIsland().encode())));
            }
        }
        // Java: 346
        public static void startRaid(com.oddlabs.tt.net.GameNetwork gn, com.oddlabs.tt.islandwars.IslandWarsSetup s, bool islander_joined)
        {
            com.oddlabs.tt.islandwars.IslandWarsNet.hosting = null;
            com.oddlabs.tt.islandwars.IslandWarsNet.hosted = null;
            if (com.oddlabs.tt.islandwars.IslandWarsNet.timer != null)
            {
                com.oddlabs.tt.islandwars.IslandWarsNet.timer.stop();
            }
            com.oddlabs.tt.islandwars.HomeIsland me = com.oddlabs.tt.islandwars.IslandWarsNet.myIsland();
            com.oddlabs.tt.net.GameServerInterface server = gn.getClient().getServerInterface();
            server.setPlayerSlot(0, com.oddlabs.tt.net.PlayerSlot.HUMAN, java.lang.Math.clamp(me.race, 0, 1), 0, true, com.oddlabs.tt.net.PlayerSlot.AI_NONE);
            if (!islander_joined)
            {
                server.setPlayerSlot(1, com.oddlabs.tt.net.PlayerSlot.AI, java.lang.Math.clamp(s.race, 0, 1), 1, true, com.oddlabs.tt.net.PlayerSlot.AI_HARD);
            }
            server.startServer();
        }
        // Java: 362
        public static bool lobbyChat(int slot, string chat)
        {
            if (!chat.startsWith(com.oddlabs.tt.islandwars.IslandWarsNet.P))
            {
                return false;
            }
            string m = chat.substring(com.oddlabs.tt.islandwars.IslandWarsNet.P.length()).trim();
            if (m.startsWith("u "))
            {
                com.oddlabs.tt.islandwars.IslandWarsNet.match_islands.put(slot, m.substring(2).trim());
            }
            return true;
        }
        // Java: 372
        public static void beforeStart(com.oddlabs.tt.net.GameNetwork gn)
        {
            com.oddlabs.tt.landscape.WorldParameters wp = gn.getClient().getWorldParameters();
            if (wp == null || ((object)wp.getGameMode() != (object)com.oddlabs.matchmaking.GameMode.ISLAND_WARS) || gn.getServer() == null)
            {
                return;
            }
            com.oddlabs.tt.islandwars.IslandWarsSetup cur = com.oddlabs.tt.islandwars.IslandWarsSetup.decode(wp.getIslandWars());
            if (cur != null && cur.kind == com.oddlabs.tt.islandwars.IslandWarsSetup.RAID)
            {
                return;
            }
            com.oddlabs.tt.islandwars.IslandWarsSetup s = new com.oddlabs.tt.islandwars.IslandWarsSetup();
            s.kind = com.oddlabs.tt.islandwars.IslandWarsSetup.MATCH;
            com.oddlabs.tt.net.PlayerSlot[] players = gn.getClient().getPlayers();
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].getInfo() == null)
                {
                    continue;
                }
                if (players[i].getType() == com.oddlabs.tt.net.PlayerSlot.HUMAN)
                {
                    string text = ((string)com.oddlabs.tt.islandwars.IslandWarsNet.match_islands.get(i));
                    com.oddlabs.tt.islandwars.HomeIsland h = null;
                    try
                    {
                        h = (text != null ? com.oddlabs.tt.islandwars.HomeIsland.fromPeer(com.oddlabs.tt.islandwars.IslandWarsNet.unhex(text)) : (com.oddlabs.tt.islandwars.HomeIsland)(null));
                    }
                    catch (System.Exception e)
                    {
                        h = null;
                    }
                    bool ok = h != null && h.audit() == null;
                    s.setUpgrades(i, (ok ? h.upgrades : new int[com.oddlabs.tt.islandwars.IslandUpgrades.NUM]), players[i].getInfo().getName());
                }
                else
                {
                    s.setUpgrades(i, new int[com.oddlabs.tt.islandwars.IslandUpgrades.NUM], players[i].getInfo().getName());
                }
            }
            wp.setIslandWars(s.encode());
            gn.getServer().updateWorldParameters(wp);
        }
        static IslandWarsNet()
        {
            IslandWarsNet.infos_f = new java.util.LinkedHashMap();
            IslandWarsNet.notice_f = "";
            IslandWarsNet.inbox = new java.util.LinkedHashMap();
            IslandWarsNet.match_islands = new java.util.LinkedHashMap();
        }
        public IslandWarsNet() : base()
        {
        }
        // Java: anonymous class at line 298
        public sealed class Anon1_Updatable : java.lang.Object, com.oddlabs.tt.animation.Updatable
        {
            public Anon1_Updatable()
            {
            }
            // Java: 299
            public void update(object anim_p)
            {
                com.oddlabs.tt.animation.TimerAnimation anim = (com.oddlabs.tt.animation.TimerAnimation)anim_p;
                com.oddlabs.tt.islandwars.IslandWarsNet.tick();
            }
        }
    }
}
