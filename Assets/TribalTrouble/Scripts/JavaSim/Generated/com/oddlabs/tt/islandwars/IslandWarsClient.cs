// Translated from IslandWarsClient.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.islandwars
{
    // Java: 21
    public class IslandWarsClient : java.lang.Object
    {
        public static com.oddlabs.tt.viewer.WorldViewer viewer;
        public static bool settled;
        public static com.oddlabs.tt.islandwars.HomeIsland home;
        // Java: 30
        public static string localNick(com.oddlabs.tt.islandwars.IslandWarsSetup s, int slot)
        {
            com.oddlabs.matchmaking.Profile p = com.oddlabs.tt.net.Network.getMatchmakingClient().getProfile();
            return (p != null ? p.getNick() : s.name(slot));
        }
        // Java: 35
        public static int slotOf(com.oddlabs.tt.viewer.WorldViewer v, com.oddlabs.tt.player.Player p)
        {
            com.oddlabs.tt.player.Player[] players = v.getWorld().getPlayers();
            for (int i = 0; i < players.Length; i++)
            {
                if (((object)players[i] == (object)p))
                {
                    return i;
                }
            }
            return (-1);
        }
        // Java: 43
        public static void gameStarted(com.oddlabs.tt.viewer.WorldViewer v)
        {
            com.oddlabs.tt.islandwars.IslandWarsSetup s = com.oddlabs.tt.islandwars.IslandWarsRules.currentSetup();
            com.oddlabs.tt.islandwars.IslandWarsClient.viewer = v;
            com.oddlabs.tt.islandwars.IslandWarsClient.settled = false;
            com.oddlabs.tt.islandwars.IslandWarsClient.home = null;
            if (s == null)
            {
                return;
            }
            com.oddlabs.tt.gui.GUIRoot root = v.getGUIRoot();
            int me = com.oddlabs.tt.islandwars.IslandWarsClient.slotOf(v, v.getLocalPlayer());
            if (s.kind == com.oddlabs.tt.islandwars.IslandWarsSetup.HOME)
            {
                com.oddlabs.tt.islandwars.IslandWarsClient.home = com.oddlabs.tt.islandwars.HomeIsland.load(com.oddlabs.tt.islandwars.IslandWarsClient.localNick(s, me), s.race);
                com.oddlabs.tt.gui.ObjectiveBanner banner = new com.oddlabs.tt.gui.ObjectiveBanner(root, "Your home island");
                banner.setText("Build your island as you want raiders to find it: up to " + (com.oddlabs.tt.islandwars.IslandWarsClient.home.buildingCap()) + " buildings. It is saved as you go.");
                root.addChild(banner);
                v.getWorld().getAnimationManagerRealTime().registerAnimation(new com.oddlabs.tt.islandwars.IslandWarsClient.HomeSaver(v, com.oddlabs.tt.islandwars.IslandWarsClient.home));
                return;
            }
            if (s.kind != com.oddlabs.tt.islandwars.IslandWarsSetup.RAID)
            {
                return;
            }
            com.oddlabs.tt.gui.ObjectiveBanner banner_2 = new com.oddlabs.tt.gui.ObjectiveBanner(root, "Raid on " + java.lang.JStr.valueOf(s.name(s.defender)) + "'s island");
            root.addChild(banner_2);
            com.oddlabs.tt.gui.HorizButton take = new com.oddlabs.tt.gui.HorizButton("Take command", 160);
            com.oddlabs.tt.gui.HorizButton hand = new com.oddlabs.tt.gui.HorizButton("Let the AI defend", 160);
            bool islander = me == s.defender;
            if (islander)
            {
                take.setPos(root.getWidth() - 170, root.getHeight() - 120);
                hand.setPos(root.getWidth() - 170, root.getHeight() - 120);
                take.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
                {
                    v.getPeerHub().getPlayerInterface().islandWarsControl(0);
                }));
                hand.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_5, int unused_6, int unused_7, int unused_8) =>
                {
                    v.getPeerHub().getPlayerInterface().islandWarsControl(1);
                }));
            }
            v.getWorld().getAnimationManagerRealTime().registerAnimation(new com.oddlabs.tt.islandwars.IslandWarsClient.RaidWatch(v, s, banner_2, (islander ? take : (com.oddlabs.tt.gui.HorizButton)(null)), (islander ? hand : (com.oddlabs.tt.gui.HorizButton)(null))));
        }
        // Java: 79
        public static void gameEnded(com.oddlabs.tt.viewer.WorldViewer v, bool won)
        {
            if (((object)v.getWorld().getGameMode() != (object)com.oddlabs.matchmaking.GameMode.ISLAND_WARS) || com.oddlabs.tt.islandwars.IslandWarsClient.settled)
            {
                return;
            }
            com.oddlabs.tt.islandwars.IslandWarsSetup s = com.oddlabs.tt.islandwars.IslandWarsRules.currentSetup();
            if (s == null || s.kind == com.oddlabs.tt.islandwars.IslandWarsSetup.HOME)
            {
                return;
            }
            if (s.kind == com.oddlabs.tt.islandwars.IslandWarsSetup.RAID)
            {
                com.oddlabs.tt.islandwars.IslandWarsRules.Raid r = com.oddlabs.tt.islandwars.IslandWarsRules.currentRaid();
                if (r != null)
                {
                    com.oddlabs.tt.islandwars.IslandWarsClient.settleRaid(v, s, r);
                }
                return;
            }
            com.oddlabs.tt.islandwars.IslandWarsClient.settled = true;
            int me = com.oddlabs.tt.islandwars.IslandWarsClient.slotOf(v, v.getLocalPlayer());
            com.oddlabs.tt.islandwars.HomeIsland h = com.oddlabs.tt.islandwars.HomeIsland.load(com.oddlabs.tt.islandwars.IslandWarsClient.localNick(s, me), 0);
            java.lang.StringBuilder against = new java.lang.StringBuilder();
            for (int i = 0; i < v.getWorld().getPlayers().Length; i++)
            {
                if (i != me && s.hasSlot(i))
                {
                    against.append((against.isEmpty() ? "" : ",")).append(s.name(i));
                }
            }
            h.earn((won ? 'W' : 'P'), (won ? new int[] { 80, 1, 6 } : new int[] { 25, 0, 2 }), java.lang.JO.toString(against), java.lang.JSystem.currentTimeMillis());
            h.save();
        }
        // Java: 102
        public static void settleRaid(com.oddlabs.tt.viewer.WorldViewer v, com.oddlabs.tt.islandwars.IslandWarsSetup s, com.oddlabs.tt.islandwars.IslandWarsRules.Raid r)
        {
            if (com.oddlabs.tt.islandwars.IslandWarsClient.settled)
            {
                return;
            }
            com.oddlabs.tt.islandwars.IslandWarsClient.settled = true;
            int me = com.oddlabs.tt.islandwars.IslandWarsClient.slotOf(v, v.getLocalPlayer());
            long now = java.lang.JSystem.currentTimeMillis();
            int[] loot = r.loot();
            if (me == s.attacker)
            {
                com.oddlabs.tt.islandwars.HomeIsland h = com.oddlabs.tt.islandwars.HomeIsland.load(com.oddlabs.tt.islandwars.IslandWarsClient.localNick(s, me), 0);
                h.earn('R', loot, s.name(s.defender), now);
                h.save();
            }
            else if (me == s.defender)
            {
                com.oddlabs.tt.islandwars.HomeIsland h_2 = com.oddlabs.tt.islandwars.HomeIsland.load(com.oddlabs.tt.islandwars.IslandWarsClient.localNick(s, me), s.race);
                h_2.lose(loot, s.name(s.attacker), now);
                if (!r.attackerWon())
                {
                    h_2.earn('D', new int[] { 40, 0, 2 }, s.name(s.attacker), now);
                }
                if (r.getDamage() >= com.oddlabs.tt.islandwars.HomeIsland.SHIELD_DAMAGE)
                {
                    h_2.shield_until = now + com.oddlabs.tt.islandwars.HomeIsland.SHIELD_MILLIS;
                }
                h_2.save();
            }
        }
        // Java: 125
        public class HomeSaver : java.lang.Object, com.oddlabs.tt.animation.Animated
        {
            public com.oddlabs.tt.viewer.WorldViewer v;
            public com.oddlabs.tt.islandwars.HomeIsland island;
            public float next;
            public float time;
            public float left_at_start;
            public int trees_at_start = -1;
            // Java: 135
            public void animate(float t)
            {
                time += t;
                if (trees_at_start < 0)
                {
                    trees_at_start = com.oddlabs.tt.player.campaign.SagaIsland.countTrees(v.getWorld());
                }
                if (time < next)
                {
                    return;
                }
                next = (float)(time + 5.0f);
                bool changed = false;
                if (trees_at_start > 0)
                {
                    float now = (float)(com.oddlabs.tt.player.campaign.SagaIsland.countTrees(v.getWorld()) / ((float)(trees_at_start)));
                    int used = java.lang.Math.round(100.0f * (1.0f - left_at_start * java.lang.Math.min(1.0f, now)));
                    if (used != island.depletion)
                    {
                        island.setDepletion(used, java.lang.JSystem.currentTimeMillis());
                        changed = true;
                    }
                }
                int[] b = com.oddlabs.tt.islandwars.IslandWarsRules.layout(v.getLocalPlayer());
                if (b.Length / 4 > island.buildingCap())
                {
                    int[] k = new int[island.buildingCap() * 4];
                    java.lang.JSystem.arraycopy(b, 0, k, 0, k.Length);
                    b = k;
                }
                if (!java.util.Arrays.equals(b, island.buildings))
                {
                    island.buildings = b;
                    changed = true;
                }
                if (changed)
                {
                    island.save();
                }
            }
            private void init_HomeSaver()
            {
                this.next = 5.0f;
            }
            public HomeSaver(com.oddlabs.tt.viewer.WorldViewer v, com.oddlabs.tt.islandwars.HomeIsland island) : base()
            {
                init_HomeSaver();
                this.v = v;
                this.island = island;
                left_at_start = java.lang.Math.max(0.25f, 1.0f - island.depletion / 100.0f);
            }
            // mixin of default method Animated.updateChecksum
            public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
            {
            }
        }
        // Java: 155
        public class RaidWatch : java.lang.Object, com.oddlabs.tt.animation.Animated
        {
            public com.oddlabs.tt.viewer.WorldViewer v;
            public com.oddlabs.tt.islandwars.IslandWarsSetup s;
            public com.oddlabs.tt.gui.ObjectiveBanner banner;
            public com.oddlabs.tt.gui.HorizButton take;
            public com.oddlabs.tt.gui.HorizButton hand;
            public float time;
            public float next_ask;
            // Java: 171
            public void animate(float t)
            {
                time += t;
                com.oddlabs.tt.islandwars.IslandWarsRules.Raid r = com.oddlabs.tt.islandwars.IslandWarsRules.currentRaid();
                if (r == null)
                {
                    return;
                }
                com.oddlabs.tt.player.Player islander = v.getWorld().getPlayers()[s.defender];
                string who = (islander.getStandbyAI() == null ? "" : (islander.isAIAsleep() ? "\n" + java.lang.JStr.valueOf(s.name(s.defender)) + " commands the defence" : "\nThe island's AI defends it"));
                banner.setText(java.lang.JStr.valueOf(r.getStatus()) + java.lang.JStr.valueOf(who) + "\nLoot so far: " + (r.loot()[0]) + " gold, " + (r.loot()[1]) + " diamonds, " + (r.loot()[2]) + " crystal");
                if (take != null && hand != null)
                {
                    bool asleep = islander.isAIAsleep();
                    com.oddlabs.tt.gui.GUIRoot root = v.getGUIRoot();
                    if (asleep && ((com.oddlabs.tt.gui.GUIObject)take.getParent()) != null)
                    {
                        take.remove();
                    }
                    if (!asleep && ((com.oddlabs.tt.gui.GUIObject)take.getParent()) == null && !r.isOver())
                    {
                        root.addChild(take);
                    }
                    if (!asleep && ((com.oddlabs.tt.gui.GUIObject)hand.getParent()) != null)
                    {
                        hand.remove();
                    }
                    if (asleep && ((com.oddlabs.tt.gui.GUIObject)hand.getParent()) == null && !r.isOver())
                    {
                        root.addChild(hand);
                    }
                }
                int me = com.oddlabs.tt.islandwars.IslandWarsClient.slotOf(v, v.getLocalPlayer());
                if (me == s.attacker && time >= next_ask && !r.isOver())
                {
                    next_ask = (float)(time + 10.0f);
                    if (islander.isAIAsleep() && islander.secondsSinceOrder() >= com.oddlabs.tt.player.Player.TAKE_OVER_SILENCE)
                    {
                        v.getPeerHub().getPlayerInterface().islandWarsControl(100 + s.defender);
                    }
                }
                if (r.isOver())
                {
                    com.oddlabs.tt.islandwars.IslandWarsClient.settleRaid(v, s, r);
                }
            }
            private void init_RaidWatch()
            {
                this.next_ask = 10.0f;
            }
            public RaidWatch(com.oddlabs.tt.viewer.WorldViewer v, com.oddlabs.tt.islandwars.IslandWarsSetup s, com.oddlabs.tt.gui.ObjectiveBanner banner, com.oddlabs.tt.gui.HorizButton take, com.oddlabs.tt.gui.HorizButton hand) : base()
            {
                init_RaidWatch();
                this.v = v;
                this.s = s;
                this.banner = banner;
                this.take = take;
                this.hand = hand;
            }
            // mixin of default method Animated.updateChecksum
            public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
            {
            }
        }
        public IslandWarsClient() : base()
        {
        }
    }
}
