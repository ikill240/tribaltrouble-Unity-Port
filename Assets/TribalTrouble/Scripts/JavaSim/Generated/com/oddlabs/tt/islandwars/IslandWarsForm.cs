// Translated from IslandWarsForm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.islandwars
{
    // Java: 39
    public class IslandWarsForm : com.oddlabs.tt.gui.Form
    {
        public static string[] NAMES;
        public static string[] WHAT;
        public const int NICK = 160;
        public com.oddlabs.tt.gui.GUIRoot gui_root;
        public com.oddlabs.tt.form.SelectGameMenu lobby;
        public com.oddlabs.net.NetworkSelector network;
        public com.oddlabs.tt.gui.MultiColumnComboBox players;
        // Java: 53
        public static string title()
        {
            return "Island Wars";
        }
        // Java: 202
        public static com.oddlabs.tt.gui.Label label(string text, com.oddlabs.tt.font.Font font)
        {
            return new com.oddlabs.tt.gui.Label(text, font);
        }
        // Java: 206
        public static com.oddlabs.tt.gui.Label label(string text, com.oddlabs.tt.font.Font font, int width)
        {
            return new com.oddlabs.tt.gui.Label(text, font, width);
        }
        // Java: 210
        public static java.util.List roomNicks()
        {
            java.util.List l = new java.util.ArrayList();
            com.oddlabs.tt.gui.ChatRoomInfo info = com.oddlabs.tt.net.Network.getMatchmakingClient().getChatRoomInfo();
            string me = com.oddlabs.tt.islandwars.IslandWarsNet.myNick();
            if (info != null && info.users() != null)
            {
                foreach (com.oddlabs.matchmaking.ChatRoomUser u in info.users())
                {
                    if (me == null || !u.getNick().equalsIgnoreCase(me))
                    {
                        l.add(u.getNick());
                    }
                }
            }
            return l;
        }
        // The server lets a player be in one chat room at a time and answers any other join with "invalid name": leave
        // the room one is in first (nothing to do when it is already the Island Wars room).
        public static void joinIslandWarsRoom(com.oddlabs.tt.gui.GUIRoot gui_root)
        {
            com.oddlabs.tt.net.MatchmakingClient mc = com.oddlabs.tt.net.Network.getMatchmakingClient();
            if (mc == null || !mc.isConnected() || mc.getProfile() == null)
            {
                gui_root.addModalForm(new com.oddlabs.tt.form.MessageForm("Log in to multiplayer (and pick a profile) to join the " + com.oddlabs.tt.islandwars.IslandWarsNet.ROOM + " room."));
                return;
            }
            com.oddlabs.tt.gui.ChatRoomInfo info = mc.getChatRoomInfo();
            if (info != null && com.oddlabs.tt.islandwars.IslandWarsNet.ROOM.Equals(info.name()))
            {
                return;
            }
            mc.leaveChatRoom();
            mc.joinRoom(gui_root, com.oddlabs.tt.islandwars.IslandWarsNet.ROOM);
        }
        // Java: 221
        public static void pingAll()
        {
            foreach (string n in java.lang.JIter.of(com.oddlabs.tt.islandwars.IslandWarsForm.roomNicks()))
            {
                com.oddlabs.tt.islandwars.IslandWarsNet.ping(n);
            }
        }
        // Java: 226
        public void fillPlayers()
        {
            java.util.Map infos = com.oddlabs.tt.islandwars.IslandWarsNet.infos();
            long now = java.lang.JSystem.currentTimeMillis();
            com.oddlabs.tt.font.Font f = com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font();
            foreach (string n in java.lang.JIter.of(com.oddlabs.tt.islandwars.IslandWarsForm.roomNicks()))
            {
                string info = ((string)infos.get(n));
                string island = default;
                string state = default;
                if (info == null)
                {
                    island = "(no answer yet)";
                    state = "";
                }
                else
                {
                    string[] p = info.split(" ");
                    long shield = (p.Length > 0 ? java.lang.Long.parseLong(p[0]) : 0);
                    int size = (p.Length > 1 ? java.lang.Integer.parseInt(p[1]) : 0);
                    island = (com.oddlabs.tt.islandwars.IslandUpgrades.SIZE_METERS[java.lang.Math.clamp(size, 0, com.oddlabs.tt.islandwars.IslandUpgrades.maxSize())]) + " m, upgrades " + java.lang.JStr.valueOf(((p.Length > 2 ? p[2] : "?")));
                    state = (shield > now ? "shielded" : (p.Length > 3 && java.lang.JO.equals(p[3], "1") ? "in a game" : "can be raided"));
                }
                players.addRow(new com.oddlabs.tt.gui.Row(java.util.List_.of(new com.oddlabs.tt.gui.Label(n, f, com.oddlabs.tt.islandwars.IslandWarsForm.NICK), new com.oddlabs.tt.gui.Label(island, f, 220), new com.oddlabs.tt.gui.Label(state, f, 120)), n));
            }
        }
        // Java: 248
        public void reopen()
        {
            com.oddlabs.tt.islandwars.IslandWarsNet.setOnChange(null);
            com.oddlabs.tt.islandwars.IslandWarsNet.setOnClose(null);
            remove();
            gui_root.addModalForm(new com.oddlabs.tt.islandwars.IslandWarsForm(gui_root, lobby, network));
        }
        // Java: 255
        public override void doCancel()
        {
            com.oddlabs.tt.islandwars.IslandWarsNet.setOnChange(null);
            com.oddlabs.tt.islandwars.IslandWarsNet.setOnClose(null);
        }
        // Java: 262
        public void buildHome()
        {
            com.oddlabs.tt.islandwars.HomeIsland h = com.oddlabs.tt.islandwars.IslandWarsNet.myIsland();
            com.oddlabs.tt.islandwars.IslandWarsSetup s = com.oddlabs.tt.islandwars.IslandWarsSetup.home(h);
            com.oddlabs.tt.landscape.WorldParameters @params = com.oddlabs.tt.landscape.WorldParameters.builder().initialGameSpeed(2).mapcode("island-wars").initialUnitCount(6).maxUnitCount(1000).mapSize(java.lang.Math.min(3, h.size)).mode(com.oddlabs.matchmaking.GameMode.ISLAND_WARS).maxBuildingCount(h.buildingCap()).extended(true).islandWars(s.encode()).build();
            string[] ai_names = new string[com.oddlabs.matchmaking.MatchmakingServerInterface.MAX_PLAYERS];
            for (int i = 0; i < ai_names.Length; i++)
            {
                ai_names[i] = "Driftwood";
            }
            com.oddlabs.tt.procedural.Landscape.TerrainType terrain = (h.terrain == 1 ? com.oddlabs.tt.procedural.Landscape.TerrainType.VIKING : com.oddlabs.tt.procedural.Landscape.TerrainType.NATIVE);
            float left = h.resourcesLeft(java.lang.JSystem.currentTimeMillis());
            h.save();
            com.oddlabs.tt.islandwars.IslandWarsNet.setOnChange(null);
            remove();
            com.oddlabs.tt.net.GameNetwork gn = com.oddlabs.tt.@delegate.Menu.startNewGame(network, gui_root, null, @params, new com.oddlabs.tt.viewer.DefaultInGameInfo(), new com.oddlabs.tt.@delegate.Menu.DefaultWorldInitAction(), null, h.metersPerWorld(), terrain, 0.5f, 0.6f * left, 0.5f * left, h.seed, false, ai_names, 2);
            gn.getClient().setUnitInfo(1, new com.oddlabs.tt.player.UnitInfo());
            gn.getClient().getServerInterface().setPlayerSlot(0, com.oddlabs.tt.net.PlayerSlot.HUMAN, java.lang.Math.clamp(h.race, 0, 1), 0, true, com.oddlabs.tt.net.PlayerSlot.AI_NONE);
            gn.getClient().getServerInterface().setPlayerSlot(1, com.oddlabs.tt.net.PlayerSlot.AI, java.lang.Math.clamp(h.race, 0, 1), 0, true, com.oddlabs.tt.net.PlayerSlot.AI_NEUTRAL_CAMPAIGN);
            gn.getClient().getServerInterface().startServer();
        }
        static IslandWarsForm()
        {
            IslandWarsForm.NAMES = new string[] { "Settlers", "War Band", "Storehouse", "Veterans", "Watchtowers", "Chief's Hall", "Raiding Fleet", "Masons" };
            IslandWarsForm.WHAT = new string[] { "+2 peons at the start", "+2 warriors at the start (iron from level 3)", "+20 wood, 10 rock, 5 iron in store at the start", "armor and range research start a level higher", "a tower more at the start", "start with your chieftain (level 2: a hardier one)", "raids land 4 more warriors", "raided buildings stand in stone, iron, then runes" };
        }
        public IslandWarsForm(com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.form.SelectGameMenu lobby, com.oddlabs.net.NetworkSelector network) : base(com.oddlabs.tt.islandwars.IslandWarsForm.title())
        {
            this.gui_root = gui_root;
            this.lobby = lobby;
            this.network = network;
            com.oddlabs.tt.islandwars.IslandWarsNet.setLobby(lobby, gui_root, network);
            com.oddlabs.tt.islandwars.HomeIsland h = com.oddlabs.tt.islandwars.IslandWarsNet.myIsland();
            long now = java.lang.JSystem.currentTimeMillis();
            com.oddlabs.tt.font.Font font = com.oddlabs.tt.gui.Skin.getSkin().getEditFont();
            com.oddlabs.tt.gui.Label head = new com.oddlabs.tt.gui.Label(java.lang.JStr.valueOf(h.nick) + "'s island", com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont());
            addChild(head);
            string problem = h.audit();
            java.util.List col = new java.util.ArrayList();
            col.add(com.oddlabs.tt.islandwars.IslandWarsForm.label("Treasury: " + (h.treasury[0]) + " gold, " + (h.treasury[1]) + " diamonds, " + (h.treasury[2]) + " crystal", font));
            col.add(com.oddlabs.tt.islandwars.IslandWarsForm.label("Island: " + (h.metersPerWorld()) + " m across, holds " + (h.buildingCap()) + " buildings (" + (h.buildings.Length / 4) + " standing) - " + java.lang.JStr.valueOf(((h.race == 1 ? "Vikings" : "Natives"))) + ", " + java.lang.JStr.valueOf(((h.terrain == 1 ? "northern" : "tropical"))), font));
            col.add(com.oddlabs.tt.islandwars.IslandWarsForm.label((h.isShielded(now) ? "Shield: " + (java.lang.Math.max(1, (h.shield_until - now) / 3600000L)) + " h left - no one can raid it" : "Shield: none - anyone online can raid it while you are online"), font));
            col.add(com.oddlabs.tt.islandwars.IslandWarsForm.label((problem == null ? "Check: everything adds up" : "Check: " + java.lang.JStr.valueOf(problem) + " - raids and matches ignore its upgrades"), font));
            com.oddlabs.tt.gui.GUIObject prev = head;
            foreach (com.oddlabs.tt.gui.GUIObject g in java.lang.JIter.of(col))
            {
                addChild(g);
                g.place(prev, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
                prev = g;
            }
            for (int u = 0; u < com.oddlabs.tt.islandwars.IslandUpgrades.NUM; u++)
            {
                int level = h.upgrades[u];
                bool top = level >= com.oddlabs.tt.islandwars.IslandUpgrades.maxLevel(u);
                int[] c = (top ? new int[3] : com.oddlabs.tt.islandwars.IslandUpgrades.cost(u, level));
                com.oddlabs.tt.gui.Label l = com.oddlabs.tt.islandwars.IslandWarsForm.label(java.lang.JStr.valueOf(com.oddlabs.tt.islandwars.IslandWarsForm.NAMES[u]) + " " + (level) + "/" + (com.oddlabs.tt.islandwars.IslandUpgrades.maxLevel(u)) + ": " + java.lang.JStr.valueOf(com.oddlabs.tt.islandwars.IslandWarsForm.WHAT[u]), font, 430);
                addChild(l);
                l.place(prev, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
                com.oddlabs.tt.gui.HorizButton b = new com.oddlabs.tt.gui.HorizButton((top ? "Done" : (c[0]) + "g " + (c[1]) + "d " + (c[2]) + "c"), 150);
                b.setDisabled(top || !h.canAfford(c));
                int up = u;
                b.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
                {
                    com.oddlabs.tt.islandwars.HomeIsland me = com.oddlabs.tt.islandwars.IslandWarsNet.myIsland();
                    if (me.buyUpgrade(up, java.lang.JSystem.currentTimeMillis()))
                    {
                        me.save();
                    }
                    reopen();
                }));
                addChild(b);
                b.place(l, com.oddlabs.tt.gui.Placement.RIGHT_MID);
                prev = l;
            }
            bool big = h.size >= com.oddlabs.tt.islandwars.IslandUpgrades.maxSize();
            int[] sc = (big ? new int[3] : com.oddlabs.tt.islandwars.IslandUpgrades.sizeCost(h.size + 1));
            com.oddlabs.tt.gui.Label grow = com.oddlabs.tt.islandwars.IslandWarsForm.label((big ? "The island is as big as it gets" : "Grow the island to " + (com.oddlabs.tt.islandwars.IslandUpgrades.SIZE_METERS[h.size + 1]) + " m (" + (com.oddlabs.tt.islandwars.IslandUpgrades.SIZE_BUILDINGS[h.size + 1]) + " buildings)"), font, 430);
            addChild(grow);
            grow.place(prev, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            com.oddlabs.tt.gui.HorizButton grow_b = new com.oddlabs.tt.gui.HorizButton((big ? "Done" : (sc[0]) + "g " + (sc[1]) + "d " + (sc[2]) + "c"), 150);
            grow_b.setDisabled(big || !h.canAfford(sc));
            grow_b.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_5, int unused_6, int unused_7, int unused_8) =>
            {
                com.oddlabs.tt.islandwars.HomeIsland me_2 = com.oddlabs.tt.islandwars.IslandWarsNet.myIsland();
                if (me_2.growIsland(java.lang.JSystem.currentTimeMillis()))
                {
                    me_2.save();
                }
                reopen();
            }));
            addChild(grow_b);
            grow_b.place(grow, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            prev = grow;
            if (h.buildings.Length == 0)
            {
                com.oddlabs.tt.gui.HorizButton tribe = new com.oddlabs.tt.gui.HorizButton((h.race == 1 ? "Tribe: Vikings" : "Tribe: Natives"), 150);
                tribe.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_9, int unused_10, int unused_11, int unused_12) =>
                {
                    com.oddlabs.tt.islandwars.HomeIsland me_3 = com.oddlabs.tt.islandwars.IslandWarsNet.myIsland();
                    me_3.race = 1 - me_3.race;
                    me_3.save();
                    reopen();
                }));
                com.oddlabs.tt.gui.HorizButton look = new com.oddlabs.tt.gui.HorizButton((h.terrain == 1 ? "Northern" : "Tropical"), 150);
                look.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_13, int unused_14, int unused_15, int unused_16) =>
                {
                    com.oddlabs.tt.islandwars.HomeIsland me_4 = com.oddlabs.tt.islandwars.IslandWarsNet.myIsland();
                    me_4.terrain = 1 - me_4.terrain;
                    me_4.save();
                    reopen();
                }));
                addChild(tribe);
                addChild(look);
                tribe.place(prev, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
                look.place(tribe, com.oddlabs.tt.gui.Placement.RIGHT_MID);
                prev = tribe;
            }
            com.oddlabs.tt.gui.Label raid_head = new com.oddlabs.tt.gui.Label("Raid an island (players in the " + java.lang.JStr.valueOf(com.oddlabs.tt.islandwars.IslandWarsNet.ROOM) + " chat room, online now)", com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont());
            addChild(raid_head);
            raid_head.place(prev, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            players = new com.oddlabs.tt.gui.MultiColumnComboBox(gui_root, new com.oddlabs.tt.gui.ColumnInfo[] { new com.oddlabs.tt.gui.ColumnInfo("Player", com.oddlabs.tt.islandwars.IslandWarsForm.NICK), new com.oddlabs.tt.gui.ColumnInfo("Island", 220), new com.oddlabs.tt.gui.ColumnInfo("", 120) }, 140);
            addChild(players);
            players.place(raid_head, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            fillPlayers();
            com.oddlabs.tt.gui.HorizButton build = new com.oddlabs.tt.gui.HorizButton("Build on my island", 170);
            build.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_17, int unused_18, int unused_19, int unused_20) =>
            {
                buildHome();
            }));
            com.oddlabs.tt.gui.HorizButton room = new com.oddlabs.tt.gui.HorizButton("Join " + java.lang.JStr.valueOf(com.oddlabs.tt.islandwars.IslandWarsNet.ROOM), 150);
            room.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_21, int unused_22, int unused_23, int unused_24) =>
            {
                com.oddlabs.tt.islandwars.IslandWarsForm.joinIslandWarsRoom(gui_root);
                reopen();
            }));
            com.oddlabs.tt.gui.HorizButton refresh = new com.oddlabs.tt.gui.HorizButton("Look again", 120);
            refresh.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_25, int unused_26, int unused_27, int unused_28) =>
            {
                com.oddlabs.tt.islandwars.IslandWarsForm.pingAll();
                reopen();
            }));
            com.oddlabs.tt.gui.HorizButton raid = new com.oddlabs.tt.gui.HorizButton("Raid!", 100);
            raid.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_29, int unused_30, int unused_31, int unused_32) =>
            {
                string nick = ((string)players.getSelected());
                if (nick != null && !h.isTampered())
                {
                    com.oddlabs.tt.islandwars.IslandWarsNet.requestRaid(nick);
                    reopen();
                }
            }));
            com.oddlabs.tt.gui.HorizButton close = new com.oddlabs.tt.gui.HorizButton("Close", 100);
            close.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_33, int unused_34, int unused_35, int unused_36) =>
            {
                cancel();
            }));
            addChild(build);
            addChild(room);
            addChild(refresh);
            addChild(raid);
            addChild(close);
            build.place(players, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            room.place(build, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            refresh.place(room, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            raid.place(refresh, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            close.place(raid, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            com.oddlabs.tt.gui.Label notice = com.oddlabs.tt.islandwars.IslandWarsForm.label(com.oddlabs.tt.islandwars.IslandWarsNet.notice(), font, 600);
            addChild(notice);
            notice.place(build, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            compileCanvas();
            com.oddlabs.tt.islandwars.IslandWarsNet.setOnChange(new java.lang.Runnable_L(() => { this.reopen(); }));
            com.oddlabs.tt.islandwars.IslandWarsNet.setOnClose(new java.lang.Runnable_L(() =>
            {
                com.oddlabs.tt.islandwars.IslandWarsNet.setOnChange(null);
                remove();
            }));
            if (com.oddlabs.tt.islandwars.IslandWarsNet.infos().isEmpty())
            {
                com.oddlabs.tt.islandwars.IslandWarsForm.pingAll();
            }
        }
    }
}
