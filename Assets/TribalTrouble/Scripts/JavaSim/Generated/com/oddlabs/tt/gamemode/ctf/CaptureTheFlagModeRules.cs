// Translated from CaptureTheFlagModeRules.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gamemode.ctf
{
    // Java: 36
    public class CaptureTheFlagModeRules : java.lang.Object, com.oddlabs.tt.gamemode.GameModeRules
    {
        public const int CAPTURES_TO_WIN = 3;
        public const float TOUCH = 2.5f;
        public const float GUARD_SHARE = 0.25f;
        public const float RAID_SHARE = 0.35f;
        public const float RAID_EVERY = 30.0f;
        public const float GUARD_REACH = 9.0f;
        public const int FLAG_SPRITE = 2;
        public int winning_team;
        public string last_status;
        public java.util.List flags;
        // Java: 50
        public class Flag : java.lang.Object
        {
            public int team;
            public float home_x;
            public float home_y;
            public float x;
            public float y;
            public com.oddlabs.tt.model.Unit carrier;
            public int captures;
            public com.oddlabs.tt.model.SceneryModel model;
            // Java: 64
            public int getTeam()
            {
                return team;
            }
            // Java: 68
            public float getX()
            {
                return x;
            }
            // Java: 72
            public float getY()
            {
                return y;
            }
            // Java: 76
            public bool isHome()
            {
                return carrier == null && x == home_x && y == home_y;
            }
            // Java: 80
            public com.oddlabs.tt.model.Unit getCarrier()
            {
                return carrier;
            }
            // Java: 84
            public int getCaptures()
            {
                return captures;
            }
            public Flag(int team, float x, float y) : base()
            {
                this.team = team;
                this.home_x = this.x = x;
                this.home_y = this.y = y;
            }
        }
        // Java: 89
        public java.util.List getFlags()
        {
            return flags;
        }
        // Java: 93
        public int getWinningTeam()
        {
            return winning_team;
        }
        // Java: 97
        public java.util.List getOptions()
        {
            return java.util.List_.of();
        }
        // Java: 102
        public bool isPlayerAlive(com.oddlabs.tt.player.Player player)
        {
            if (winning_team != com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL)
            {
                return player.getPlayerInfo().getTeam() == winning_team;
            }
            int units = player.getUnitCountContainer().getNumSupplies();
            return units > 0 || player.hasActiveChieftain() || player.getQuarters() != null;
        }
        // Java: 110
        public void onGameStart(com.oddlabs.tt.viewer.WorldViewer viewer)
        {
            com.oddlabs.tt.gui.Label label = new com.oddlabs.tt.gui.Label("", com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont(), 560, com.oddlabs.tt.gui.Origin.AT_MIDDLE);
            label.setPos((viewer.getGUIRoot().getWidth() - label.getWidth()) / 2, 10);
            viewer.getGUIRoot().addChild(label);
            onWorldStart(viewer.getWorld(), new java.util.function.Consumer_L((object a0) => { label.setText(((string)a0)); }));
        }
        // Java: 118
        public string describe()
        {
            return last_status;
        }
        // Java: 123
        public void onWorldStart(com.oddlabs.tt.landscape.World world, java.util.function.Consumer status)
        {
            winning_team = com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL;
            last_status = "";
            flags.clear();
            float center = (float)(world.getHeightMap().getMetersPerWorld() / 2.0f);
            foreach (com.oddlabs.tt.player.Player p in world.getPlayers())
            {
                int team = p.getPlayerInfo().getTeam();
                if (team == com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL || team == com.oddlabs.tt.landscape.World.ZOMBIE_TEAM || flagOf(team) != null)
                {
                    continue;
                }
                float dx = (float)(center - p.getStartX());
                float dy = (float)(center - p.getStartY());
                float d = java.lang.Math.max(1.0f, ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy))))));
                com.oddlabs.tt.gamemode.ctf.CaptureTheFlagModeRules.Flag f = new com.oddlabs.tt.gamemode.ctf.CaptureTheFlagModeRules.Flag(team, (float)(p.getStartX() + (float)((float)(12.0f * dx) / d)), (float)(p.getStartY() + (float)((float)(12.0f * dy) / d)));
                f.model = new com.oddlabs.tt.model.SceneryModel(world, f.x, f.y, 0.0f, 1.0f, world.getRacesResources().getTreasures()[com.oddlabs.tt.gamemode.ctf.CaptureTheFlagModeRules.FLAG_SPRITE], 2.0f, false, "Flag of team " + ((team + 1)));
                flags.add(f);
            }
            world.getAnimationManagerGameTime().registerAnimation(new com.oddlabs.tt.gamemode.ctf.CaptureTheFlagModeRules.Referee(this, world, status));
        }
        // Java: 144
        public com.oddlabs.tt.gamemode.ctf.CaptureTheFlagModeRules.Flag flagOf(int team)
        {
            foreach (com.oddlabs.tt.gamemode.ctf.CaptureTheFlagModeRules.Flag f in java.lang.JIter.of(flags))
            {
                if (f.team == team)
                {
                    return f;
                }
            }
            return null;
        }
        // Java: 151
        public class Referee : java.lang.Object, com.oddlabs.tt.animation.Animated
        {
            public com.oddlabs.tt.gamemode.ctf.CaptureTheFlagModeRules this_0;
            public com.oddlabs.tt.landscape.World world;
            public java.util.function.Consumer status;
            public java.util.List guards;
            public float time;
            public float next_check;
            public float next_guard;
            public float next_raid;
            // Java: 167
            public void animate(float t)
            {
                time += t;
                if (this_0.winning_team != com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL || time < next_check)
                {
                    return;
                }
                next_check = (float)(time + 0.25f);
                foreach (com.oddlabs.tt.gamemode.ctf.CaptureTheFlagModeRules.Flag f in java.lang.JIter.of(this_0.flags))
                {
                    referee(f);
                }
                com.oddlabs.tt.player.Player[] players = world.getPlayers();
                if (time >= next_guard)
                {
                    next_guard = (float)(time + 3.0f);
                    for (int i = 0; i < players.Length; i++)
                    {
                        guard(players[i], ((java.util.List)guards.get(i)));
                    }
                    foreach (com.oddlabs.tt.gamemode.ctf.CaptureTheFlagModeRules.Flag f_2 in java.lang.JIter.of(this_0.flags))
                    {
                        carry(f_2);
                    }
                }
                if (time >= next_raid)
                {
                    next_raid = (float)(time + com.oddlabs.tt.gamemode.ctf.CaptureTheFlagModeRules.RAID_EVERY);
                    foreach (com.oddlabs.tt.player.Player p in players)
                    {
                        raid(p);
                    }
                }
                java.lang.StringBuilder sb = new java.lang.StringBuilder("Flags:");
                foreach (com.oddlabs.tt.gamemode.ctf.CaptureTheFlagModeRules.Flag f_3 in java.lang.JIter.of(this_0.flags))
                {
                    sb.append("   Team ").append(f_3.team + 1).append(": ").append(f_3.captures);
                    if (f_3.carrier != null)
                    {
                        sb.append(" [its flag taken]");
                    }
                    else if (!f_3.isHome())
                    {
                        sb.append(" [its flag dropped]");
                    }
                }
                sb.append("  - first to ").append(com.oddlabs.tt.gamemode.ctf.CaptureTheFlagModeRules.CAPTURES_TO_WIN);
                if (this_0.winning_team != com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL)
                {
                    sb = new java.lang.StringBuilder("Team " + ((this_0.winning_team + 1)) + " has captured the flags!");
                }
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
            // Java: 207
            public void referee(com.oddlabs.tt.gamemode.ctf.CaptureTheFlagModeRules.Flag f)
            {
                if (f.carrier != null)
                {
                    com.oddlabs.tt.model.Unit c = f.carrier;
                    if (c.isDead() || c.isMounted())
                    {
                        f.carrier = null;
                    }
                    else
                    {
                        move(f, c.getPositionX(), c.getPositionY());
                        com.oddlabs.tt.gamemode.ctf.CaptureTheFlagModeRules.Flag own = this_0.flagOf(c.getOwner().getPlayerInfo().getTeam());
                        if (own != null && own.isHome() && com.oddlabs.tt.gamemode.ctf.CaptureTheFlagModeRules.near(c, own.home_x, own.home_y, (float)(com.oddlabs.tt.gamemode.ctf.CaptureTheFlagModeRules.TOUCH + 1.5f)))
                        {
                            own.captures++;
                            f.carrier = null;
                            move(f, f.home_x, f.home_y);
                            if (own.captures >= com.oddlabs.tt.gamemode.ctf.CaptureTheFlagModeRules.CAPTURES_TO_WIN)
                            {
                                this_0.winning_team = own.team;
                            }
                        }
                    }
                    return;
                }
                com.oddlabs.tt.model.Unit toucher = null;
                foreach (com.oddlabs.tt.player.Player p in world.getPlayers())
                {
                    int team = p.getPlayerInfo().getTeam();
                    if (team == com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL || team == com.oddlabs.tt.landscape.World.ZOMBIE_TEAM || (team == f.team && f.isHome()))
                    {
                        continue;
                    }
                    foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(p.getUnits().getSet()))
                    {
                        if ((s is com.oddlabs.tt.model.Unit u) && !u.isDead() && !u.isMounted() && com.oddlabs.tt.gamemode.ctf.CaptureTheFlagModeRules.near(u, f.x, f.y, com.oddlabs.tt.gamemode.ctf.CaptureTheFlagModeRules.TOUCH) && (toucher == null || u.getGridY() * 4096 + u.getGridX() < toucher.getGridY() * 4096 + toucher.getGridX()))
                        {
                            toucher = u;
                        }
                    }
                }
                if (toucher == null)
                {
                    return;
                }
                if (toucher.getOwner().getPlayerInfo().getTeam() == f.team)
                {
                    move(f, f.home_x, f.home_y);
                }
                else
                {
                    f.carrier = toucher;
                }
            }
            // Java: 244
            public void move(com.oddlabs.tt.gamemode.ctf.CaptureTheFlagModeRules.Flag f, float x, float y)
            {
                f.x = x;
                f.y = y;
                if (f.model != null && (java.lang.Math.abs((float)(f.model.getPositionX() - x)) > 0.5f || java.lang.Math.abs((float)(f.model.getPositionY() - y)) > 0.5f))
                {
                    f.model.setPosition(x, y);
                }
            }
            // Java: 252
            public void carry(com.oddlabs.tt.gamemode.ctf.CaptureTheFlagModeRules.Flag f)
            {
                com.oddlabs.tt.model.Unit c = f.carrier;
                if (c != null)
                {
                    com.oddlabs.tt.player.Player p = c.getOwner();
                    com.oddlabs.tt.gamemode.ctf.CaptureTheFlagModeRules.Flag own = this_0.flagOf(p.getPlayerInfo().getTeam());
                    if (p.getAI() != null && own != null && !c.isMoving())
                    {
                        order(p, new com.oddlabs.tt.model.Selectable[] { c }, own.home_x, own.home_y, com.oddlabs.tt.model.Action.MOVE);
                    }
                    return;
                }
                if (f.isHome())
                {
                    return;
                }
                foreach (com.oddlabs.tt.player.Player p_2 in world.getPlayers())
                {
                    if (p_2.getAI() == null || p_2.getPlayerInfo().getTeam() != f.team)
                    {
                        continue;
                    }
                    com.oddlabs.tt.model.Unit best = null;
                    float best_d = (float)(30.0f * 30.0f);
                    foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(p_2.getUnits().getSet()))
                    {
                        if ((s is com.oddlabs.tt.model.Unit u) && !u.isDead() && !u.isMounted() && u.isWarrior() && com.oddlabs.tt.gamemode.ctf.CaptureTheFlagModeRules.d2(u, f.x, f.y) < best_d)
                        {
                            best_d = com.oddlabs.tt.gamemode.ctf.CaptureTheFlagModeRules.d2(u, f.x, f.y);
                            best = u;
                        }
                    }
                    if (best != null)
                    {
                        order(p_2, new com.oddlabs.tt.model.Selectable[] { best }, f.x, f.y, com.oddlabs.tt.model.Action.MOVE);
                    }
                }
            }
            // Java: 278
            public void order(com.oddlabs.tt.player.Player p, com.oddlabs.tt.model.Selectable[] units, float x, float y, com.oddlabs.tt.model.Action action)
            {
                p.setDirecting(true);
                try
                {
                    p.setLandscapeTarget(units, com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(x), com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(y), action, true);
                }
                finally
                {
                    p.setDirecting(false);
                }
            }
            // Java: 287
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
            // Java: 296
            public void guard(com.oddlabs.tt.player.Player p, java.util.List mine)
            {
                com.oddlabs.tt.gamemode.ctf.CaptureTheFlagModeRules.Flag own = this_0.flagOf(p.getPlayerInfo().getTeam());
                if (p.getAI() == null || own == null)
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
                    if (u_2.isDead() || u_2.isMounted() || ((object)u_2 == (object)own.carrier))
                    {
                        u_2.setObjectiveHold(false);
                        return true;
                    }
                    return false;
                }));
                java.util.List all = warriors(p);
                int quota = java.lang.JC.f2i(((float)(all.size() * com.oddlabs.tt.gamemode.ctf.CaptureTheFlagModeRules.GUARD_SHARE)));
                while (mine.size() > quota)
                {
                    ((com.oddlabs.tt.model.Unit)mine.removeLast()).setObjectiveHold(false);
                }
                all.removeIf(new java.util.function.Predicate_L((object a0) => { return ((com.oddlabs.tt.model.Unit)a0).isObjectiveHold(); }));
                all.sort(java.util.Comparator_.comparingDouble(new java.util.function.ToDoubleFunction_L((object u_3_l) =>
                {
                    com.oddlabs.tt.model.Unit u_3 = (com.oddlabs.tt.model.Unit)u_3_l;
                    return com.oddlabs.tt.gamemode.ctf.CaptureTheFlagModeRules.d2(u_3, own.home_x, own.home_y);
                })).thenComparingInt(new java.util.function.ToIntFunction_L((object a0) => { return ((com.oddlabs.tt.model.Unit)a0).getGridY(); })).thenComparingInt(new java.util.function.ToIntFunction_L((object a0) => { return ((com.oddlabs.tt.model.Unit)a0).getGridX(); })));
                for (int k = 0; k < all.size() && mine.size() < quota; k++)
                {
                    ((com.oddlabs.tt.model.Unit)all.get(k)).setObjectiveHold(true);
                    mine.add(((com.oddlabs.tt.model.Unit)all.get(k)));
                }
                java.util.List go = new java.util.ArrayList();
                foreach (com.oddlabs.tt.model.Unit u_4 in java.lang.JIter.of(mine))
                {
                    if (!u_4.isMoving() && com.oddlabs.tt.gamemode.ctf.CaptureTheFlagModeRules.d2(u_4, own.home_x, own.home_y) > (float)(com.oddlabs.tt.gamemode.ctf.CaptureTheFlagModeRules.GUARD_REACH * com.oddlabs.tt.gamemode.ctf.CaptureTheFlagModeRules.GUARD_REACH))
                    {
                        go.add(u_4);
                    }
                }
                if (!go.isEmpty())
                {
                    order(p, ((com.oddlabs.tt.model.Selectable[])go.toArray(new com.oddlabs.tt.model.Selectable[0])), own.home_x, own.home_y, com.oddlabs.tt.model.Action.ATTACK);
                }
            }
            // Java: 331
            public void raid(com.oddlabs.tt.player.Player p)
            {
                if (p.getAI() == null || !p.isAlive())
                {
                    return;
                }
                int team = p.getPlayerInfo().getTeam();
                com.oddlabs.tt.gamemode.ctf.CaptureTheFlagModeRules.Flag target = null;
                float best = java.lang.Float.MAX_VALUE;
                foreach (com.oddlabs.tt.gamemode.ctf.CaptureTheFlagModeRules.Flag f in java.lang.JIter.of(this_0.flags))
                {
                    if (f.team == team || f.carrier != null)
                    {
                        continue;
                    }
                    float d = (float)(java.lang.Math.abs((float)(f.x - p.getStartX())) + java.lang.Math.abs((float)(f.y - p.getStartY())));
                    if (d < best)
                    {
                        best = d;
                        target = f;
                    }
                }
                if (target == null)
                {
                    return;
                }
                java.util.List free = warriors(p);
                free.removeIf(new java.util.function.Predicate_L((object a0) => { return ((com.oddlabs.tt.model.Unit)a0).isObjectiveHold(); }));
                int n = java.lang.JC.d2i(java.lang.Math.ceil((float)((float)(free.size() * com.oddlabs.tt.gamemode.ctf.CaptureTheFlagModeRules.RAID_SHARE) / ((float)(1.0f - com.oddlabs.tt.gamemode.ctf.CaptureTheFlagModeRules.GUARD_SHARE)))));
                if (n <= 0)
                {
                    return;
                }
                com.oddlabs.tt.gamemode.ctf.CaptureTheFlagModeRules.Flag prey = target;
                free.sort(java.util.Comparator_.comparingDouble(new java.util.function.ToDoubleFunction_L((object u_l) =>
                {
                    com.oddlabs.tt.model.Unit u = (com.oddlabs.tt.model.Unit)u_l;
                    return com.oddlabs.tt.gamemode.ctf.CaptureTheFlagModeRules.d2(u, prey.x, prey.y);
                })).thenComparingInt(new java.util.function.ToIntFunction_L((object a0) => { return ((com.oddlabs.tt.model.Unit)a0).getGridY(); })).thenComparingInt(new java.util.function.ToIntFunction_L((object a0) => { return ((com.oddlabs.tt.model.Unit)a0).getGridX(); })));
                order(p, ((com.oddlabs.tt.model.Selectable[])free.subList(0, java.lang.Math.min(n, free.size())).toArray(new com.oddlabs.tt.model.Selectable[0])), prey.x, prey.y, com.oddlabs.tt.model.Action.ATTACK);
            }
            private void init_Referee()
            {
                this.guards = new java.util.ArrayList();
                this.next_raid = com.oddlabs.tt.gamemode.ctf.CaptureTheFlagModeRules.RAID_EVERY;
            }
            public Referee(com.oddlabs.tt.gamemode.ctf.CaptureTheFlagModeRules this_0_, com.oddlabs.tt.landscape.World world, java.util.function.Consumer status) : base()
            {
                this.this_0 = this_0_;
                init_Referee();
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
        // Java: 361
        public static bool near(com.oddlabs.tt.model.Unit u, float x, float y, float r)
        {
            return com.oddlabs.tt.gamemode.ctf.CaptureTheFlagModeRules.d2(u, x, y) < (float)(r * r);
        }
        // Java: 365
        public static float d2(com.oddlabs.tt.model.Unit u, float x, float y)
        {
            float dx = (float)(u.getPositionX() - x);
            float dy = (float)(u.getPositionY() - y);
            return (float)((float)(dx * dx) + (float)(dy * dy));
        }
        private void init_CaptureTheFlagModeRules()
        {
            this.winning_team = com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL;
            this.last_status = "";
            this.flags = new java.util.ArrayList();
        }
        public CaptureTheFlagModeRules() : base()
        {
            init_CaptureTheFlagModeRules();
        }
    }
}
