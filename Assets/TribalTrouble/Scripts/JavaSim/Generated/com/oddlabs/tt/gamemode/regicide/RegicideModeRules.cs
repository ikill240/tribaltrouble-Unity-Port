// Translated from RegicideModeRules.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gamemode.regicide
{
    // Java: 32
    public class RegicideModeRules : java.lang.Object, com.oddlabs.tt.gamemode.GameModeRules
    {
        public const float GUARD_SHARE = 0.35f;
        public const float HUNT_SHARE = 0.35f;
        public const float HUNT_EVERY = 40.0f;
        public const float GUARD_REACH = 8.0f;
        public string last_status;
        // Java: 40
        public java.util.List getOptions()
        {
            return java.util.List_.of();
        }
        // Java: 45
        public bool isPlayerAlive(com.oddlabs.tt.player.Player player)
        {
            return player.hasActiveChieftain();
        }
        // Java: 50
        public void onGameStart(com.oddlabs.tt.viewer.WorldViewer viewer)
        {
            com.oddlabs.tt.gui.Label label = new com.oddlabs.tt.gui.Label("", com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont(), 500, com.oddlabs.tt.gui.Origin.AT_MIDDLE);
            label.setPos((viewer.getGUIRoot().getWidth() - label.getWidth()) / 2, 10);
            viewer.getGUIRoot().addChild(label);
            onWorldStart(viewer.getWorld(), new java.util.function.Consumer_L((object a0) => { label.setText(((string)a0)); }));
        }
        // Java: 58
        public string describe()
        {
            return last_status;
        }
        // Java: 63
        public void onWorldStart(com.oddlabs.tt.landscape.World world, java.util.function.Consumer status)
        {
            last_status = "";
            foreach (com.oddlabs.tt.player.Player p in world.getPlayers())
            {
                if (p.hasActiveChieftain() || p.getPlayerInfo().getTeam() == com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL)
                {
                    continue;
                }
                com.oddlabs.tt.model.Unit chief = new com.oddlabs.tt.model.Unit(p, (float)(p.getStartX() + 3.0f), (float)(p.getStartY() + 3.0f), null, p.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_CHIEFTAIN), "Chieftain", false);
                chief.increaseMagicEnergy(0, 1000);
                chief.increaseMagicEnergy(1, 1000);
                p.setActiveChieftain(chief);
            }
            world.getAnimationManagerGameTime().registerAnimation(new com.oddlabs.tt.gamemode.regicide.RegicideModeRules.Court(this, world, status));
        }
        // Java: 78
        public class Court : java.lang.Object, com.oddlabs.tt.animation.Animated
        {
            public com.oddlabs.tt.gamemode.regicide.RegicideModeRules this_0;
            public com.oddlabs.tt.landscape.World world;
            public java.util.function.Consumer status;
            public java.util.List guards;
            public float time;
            public float next_guard;
            public float next_hunt;
            // Java: 93
            public void animate(float t)
            {
                time += t;
                com.oddlabs.tt.player.Player[] players = world.getPlayers();
                if (time >= next_guard)
                {
                    next_guard = (float)(time + 3.0f);
                    for (int i = 0; i < players.Length; i++)
                    {
                        guard(players[i], ((java.util.List)guards.get(i)));
                    }
                }
                if (time >= next_hunt)
                {
                    next_hunt = (float)(time + com.oddlabs.tt.gamemode.regicide.RegicideModeRules.HUNT_EVERY);
                    foreach (com.oddlabs.tt.player.Player p in players)
                    {
                        hunt(p);
                    }
                }
                java.lang.StringBuilder sb = new java.lang.StringBuilder("Chieftains standing: ");
                int standing = 0;
                foreach (com.oddlabs.tt.player.Player p_2 in players)
                {
                    if (p_2.hasActiveChieftain())
                    {
                        standing++;
                    }
                }
                sb.append(standing).append(" of ").append(players.Length);
                string text = java.lang.JO.toString(sb);
                if (!java.lang.JO.equals(text, this_0.last_status))
                {
                    this_0.last_status = text;
                    if (status != null)
                    {
                        status.accept(text);
                    }
                }
            }
            // Java: 121
            public java.util.List warriors(com.oddlabs.tt.player.Player p)
            {
                java.util.List list = new java.util.ArrayList();
                foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(p.getUnits().getSet()))
                {
                    if ((s is com.oddlabs.tt.model.Unit u) && !u.isDead() && !u.isMounted() && u.isWarrior() && ((object)u != (object)p.getChieftain()))
                    {
                        list.add(u);
                    }
                }
                return list;
            }
            // Java: 130
            public void guard(com.oddlabs.tt.player.Player p, java.util.List mine)
            {
                com.oddlabs.tt.model.Unit chief = p.getChieftain();
                if (p.getAI() == null || chief == null || chief.isDead())
                {
                    foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(mine))
                    {
                        u.setObjectiveHold(false);
                    }
                    mine.clear();
                    return;
                }
                mine.removeIf(new java.util.function.Predicate_L((object u_2_l) =>
                {
                    com.oddlabs.tt.model.Unit u_2 = (com.oddlabs.tt.model.Unit)u_2_l;
                    if (u_2.isDead() || u_2.isMounted())
                    {
                        u_2.setObjectiveHold(false);
                        return true;
                    }
                    return false;
                }));
                java.util.List all = warriors(p);
                int quota = java.lang.JC.f2i(((float)(all.size() * com.oddlabs.tt.gamemode.regicide.RegicideModeRules.GUARD_SHARE)));
                while (mine.size() > quota)
                {
                    ((com.oddlabs.tt.model.Unit)mine.removeLast()).setObjectiveHold(false);
                }
                all.removeIf(new java.util.function.Predicate_L((object a0) => { return ((com.oddlabs.tt.model.Unit)a0).isObjectiveHold(); }));
                all.sort(java.util.Comparator_.comparingDouble(new java.util.function.ToDoubleFunction_L((object u_3_l) =>
                {
                    com.oddlabs.tt.model.Unit u_3 = (com.oddlabs.tt.model.Unit)u_3_l;
                    return d2(u_3, chief);
                })).thenComparingInt(new java.util.function.ToIntFunction_L((object a0) => { return ((com.oddlabs.tt.model.Unit)a0).getGridY(); })).thenComparingInt(new java.util.function.ToIntFunction_L((object a0) => { return ((com.oddlabs.tt.model.Unit)a0).getGridX(); })));
                for (int k = 0; k < all.size() && mine.size() < quota; k++)
                {
                    ((com.oddlabs.tt.model.Unit)all.get(k)).setObjectiveHold(true);
                    mine.add(((com.oddlabs.tt.model.Unit)all.get(k)));
                }
                java.util.List go = new java.util.ArrayList();
                foreach (com.oddlabs.tt.model.Unit u_4 in java.lang.JIter.of(mine))
                {
                    if (!u_4.isMoving() && d2(u_4, chief) > (float)(com.oddlabs.tt.gamemode.regicide.RegicideModeRules.GUARD_REACH * com.oddlabs.tt.gamemode.regicide.RegicideModeRules.GUARD_REACH))
                    {
                        go.add(u_4);
                    }
                }
                if (!go.isEmpty())
                {
                    p.setDirecting(true);
                    try
                    {
                        p.setLandscapeTarget(((com.oddlabs.tt.model.Selectable[])go.toArray(new com.oddlabs.tt.model.Selectable[0])), com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(chief.getPositionX()), com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(chief.getPositionY()), com.oddlabs.tt.model.Action.ATTACK, true);
                    }
                    finally
                    {
                        p.setDirecting(false);
                    }
                }
            }
            // Java: 171
            public void hunt(com.oddlabs.tt.player.Player p)
            {
                if (p.getAI() == null || !p.isAlive())
                {
                    return;
                }
                com.oddlabs.tt.model.Unit target = null;
                float best = java.lang.Float.MAX_VALUE;
                foreach (com.oddlabs.tt.player.Player e in world.getPlayers())
                {
                    com.oddlabs.tt.model.Unit c = e.getChieftain();
                    if (!p.isEnemy(e) || c == null || c.isDead())
                    {
                        continue;
                    }
                    float d = (float)(java.lang.Math.abs((float)(c.getPositionX() - p.getStartX())) + java.lang.Math.abs((float)(c.getPositionY() - p.getStartY())));
                    if (d < best)
                    {
                        best = d;
                        target = c;
                    }
                }
                if (target == null)
                {
                    return;
                }
                java.util.List free = warriors(p);
                free.removeIf(new java.util.function.Predicate_L((object a0) => { return ((com.oddlabs.tt.model.Unit)a0).isObjectiveHold(); }));
                int n = java.lang.JC.d2i(java.lang.Math.ceil((float)((float)(free.size() * com.oddlabs.tt.gamemode.regicide.RegicideModeRules.HUNT_SHARE) / ((float)(1.0f - com.oddlabs.tt.gamemode.regicide.RegicideModeRules.GUARD_SHARE)))));
                if (n <= 0)
                {
                    return;
                }
                com.oddlabs.tt.model.Unit prey = target;
                free.sort(java.util.Comparator_.comparingDouble(new java.util.function.ToDoubleFunction_L((object u_l) =>
                {
                    com.oddlabs.tt.model.Unit u = (com.oddlabs.tt.model.Unit)u_l;
                    return d2(u, prey);
                })).thenComparingInt(new java.util.function.ToIntFunction_L((object a0) => { return ((com.oddlabs.tt.model.Unit)a0).getGridY(); })).thenComparingInt(new java.util.function.ToIntFunction_L((object a0) => { return ((com.oddlabs.tt.model.Unit)a0).getGridX(); })));
                p.setTarget(((com.oddlabs.tt.model.Selectable[])free.subList(0, java.lang.Math.min(n, free.size())).toArray(new com.oddlabs.tt.model.Selectable[0])), prey, com.oddlabs.tt.model.Action.ATTACK, true);
            }
            // Java: 198
            public float d2(com.oddlabs.tt.model.Selectable a, com.oddlabs.tt.model.Selectable b)
            {
                float dx = (float)(a.getPositionX() - b.getPositionX());
                float dy = (float)(a.getPositionY() - b.getPositionY());
                return (float)((float)(dx * dx) + (float)(dy * dy));
            }
            private void init_Court()
            {
                this.guards = new java.util.ArrayList();
                this.next_hunt = com.oddlabs.tt.gamemode.regicide.RegicideModeRules.HUNT_EVERY;
            }
            public Court(com.oddlabs.tt.gamemode.regicide.RegicideModeRules this_0_, com.oddlabs.tt.landscape.World world, java.util.function.Consumer status) : base()
            {
                this.this_0 = this_0_;
                init_Court();
                this.world = world;
                this.status = status;
                for (int i = 0; i < world.getPlayers().Length; i++)
                {
                    guards.add(new java.util.ArrayList());
                }
            }
            // mixin of default method Animated.updateChecksum
            public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
            {
            }
        }
        private void init_RegicideModeRules()
        {
            this.last_status = "";
        }
        public RegicideModeRules() : base()
        {
            init_RegicideModeRules();
        }
    }
}
