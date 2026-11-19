// Translated from ObjectiveDirector.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gamemode
{
    // Java: 36
    public class ObjectiveDirector : java.lang.Object, com.oddlabs.tt.animation.Animated
    {
        public const float PERIOD = 3.0f;
        public const int MAX_HELD = 40;
        public com.oddlabs.tt.landscape.World world;
        public float[] xs;
        public float[] ys;
        public float radius;
        public int needed;
        public java.util.function.IntUnaryOperator controller;
        public java.util.function.IntSupplier leader;
        public java.util.function.Predicate defender;
        public java.util.List held;
        public java.util.List marching;
        public float time;
        public float next;
        // Java: 73
        public static void hold(com.oddlabs.tt.landscape.World world, float[] xs, float[] ys, float radius, int needed, java.util.function.IntUnaryOperator controller, java.util.function.IntSupplier leader)
        {
            if (com.oddlabs.tt.gamemode.ObjectiveDirector.hasAI(world))
            {
                world.getAnimationManagerGameTime().registerAnimation(new com.oddlabs.tt.gamemode.ObjectiveDirector(world, xs, ys, radius, needed, controller, leader, null));
            }
        }
        // Java: 81
        public static com.oddlabs.tt.gamemode.ObjectiveDirector survival(com.oddlabs.tt.landscape.World world, java.util.function.Predicate defender)
        {
            if (!com.oddlabs.tt.gamemode.ObjectiveDirector.hasAI(world))
            {
                return null;
            }
            com.oddlabs.tt.gamemode.ObjectiveDirector d = new com.oddlabs.tt.gamemode.ObjectiveDirector(world, new float[0], new float[0], 14.0f, 1, new java.util.function.IntUnaryOperator_L((int i) =>
            {
                return com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL;
            }), new java.util.function.IntSupplier_L(() =>
            {
                return com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL;
            }), defender);
            world.getAnimationManagerGameTime().registerAnimation(d);
            return d;
        }
        // Java: 91
        public void march(com.oddlabs.tt.model.Unit u)
        {
            if (u.getOwner().getAI() == null)
            {
                return;
            }
            u.setObjectiveHold(true);
            marching.add(u);
        }
        // Java: 98
        public static bool hasAI(com.oddlabs.tt.landscape.World world)
        {
            foreach (com.oddlabs.tt.player.Player p in world.getPlayers())
            {
                if (p.getAI() != null)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 105
        public void animate(float t)
        {
            time += t;
            if (time < next)
            {
                return;
            }
            next = (float)(time + com.oddlabs.tt.gamemode.ObjectiveDirector.PERIOD);
            com.oddlabs.tt.player.Player[] players = world.getPlayers();
            for (int i = 0; i < players.Length; i++)
            {
                com.oddlabs.tt.player.Player p = players[i];
                java.util.List mine = ((java.util.List)held.get(i));
                if (p.getAI() == null || !p.isAlive())
                {
                    com.oddlabs.tt.gamemode.ObjectiveDirector.release(mine);
                    continue;
                }
                if (defender != null)
                {
                    if (defender.test(p))
                    {
                        guardHome(p, mine);
                    }
                    else
                    {
                        com.oddlabs.tt.gamemode.ObjectiveDirector.release(mine);
                    }
                }
                else
                {
                    holdPoints(p, mine);
                }
            }
            if (defender != null)
            {
                marchOnDefenders();
            }
        }
        // Java: 133
        public void holdPoints(com.oddlabs.tt.player.Player p, java.util.List mine)
        {
            int team = p.getPlayerInfo().getTeam();
            if (team == com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL)
            {
                com.oddlabs.tt.gamemode.ObjectiveDirector.release(mine);
                return;
            }
            int?[] order_v = new int?[xs.Length];
            for (int k = 0; k < order_v.Length; k++)
            {
                order_v[k] = k;
            }
            float sx = p.getStartX();
            float sy = p.getStartY();
            java.util.Arrays.sort(order_v, java.util.Comparator_.comparingInt(new java.util.function.ToIntFunction_L((object k_2_l) =>
            {
                int? k_2 = (int?)k_2_l;
                return (controller.applyAsInt(((int)k_2)) == team ? 0 : 1);
            })).thenComparingDouble(new java.util.function.ToDoubleFunction_L((object k_3_l) =>
            {
                int? k_3 = (int?)k_3_l;
                return com.oddlabs.tt.gamemode.ObjectiveDirector.dist2(xs[((int)k_3)], ys[((int)k_3)], sx, sy);
            })).thenComparingInt(new java.util.function.ToIntFunction_L((object k_4_l) =>
            {
                int? k_4 = (int?)k_4_l;
                return ((int)k_4);
            })));
            int[] targets = new int[needed];
            for (int k_5 = 0; k_5 < needed; k_5++)
            {
                targets[k_5] = ((int)order_v[k_5]);
            }
            int lead = leader.getAsInt();
            bool urgent = lead != com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL && lead != team;
            java.util.List warriors_v = com.oddlabs.tt.gamemode.ObjectiveDirector.warriors(p);
            float share = (urgent ? 0.85f : (float)(0.45f + (float)(0.35f * java.lang.Math.min(1.0f, (float)(time / 900.0f)))));
            int quota = java.lang.JC.f2i(((float)(warriors_v.size() * share)));
            quota = java.lang.Math.min(com.oddlabs.tt.gamemode.ObjectiveDirector.MAX_HELD, java.lang.Math.max(java.lang.Math.min(4 * needed, warriors_v.size()), quota));
            fill(mine, warriors_v, quota, targets);
            for (int k_6 = 0; k_6 < targets.Length; k_6++)
            {
                int point = targets[k_6];
                java.util.List go = new java.util.ArrayList();
                for (int j = k_6; j < mine.size(); j += targets.Length)
                {
                    com.oddlabs.tt.model.Unit u = ((com.oddlabs.tt.model.Unit)mine.get(j));
                    if (!u.isMoving() && com.oddlabs.tt.gamemode.ObjectiveDirector.dist2(u.getPositionX(), u.getPositionY(), xs[point], ys[point]) > com.oddlabs.tt.gamemode.ObjectiveDirector.sq((float)(radius * 0.7f)))
                    {
                        go.add(u);
                    }
                }
                order(p, go, xs[point], ys[point]);
            }
        }
        // Java: 173
        public void guardHome(com.oddlabs.tt.player.Player p, java.util.List mine)
        {
            com.oddlabs.tt.model.Building q = p.getQuarters();
            float hx = (q != null && !q.isDead() ? q.getPositionX() : p.getStartX());
            float hy = (q != null && !q.isDead() ? q.getPositionY() : p.getStartY());
            java.util.List warriors_v = com.oddlabs.tt.gamemode.ObjectiveDirector.warriors(p);
            fill(mine, warriors_v, java.lang.Math.min(com.oddlabs.tt.gamemode.ObjectiveDirector.MAX_HELD, warriors_v.size() / 2), null);
            java.util.List go = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(mine))
            {
                if (!u.isMoving() && com.oddlabs.tt.gamemode.ObjectiveDirector.dist2(u.getPositionX(), u.getPositionY(), hx, hy) > com.oddlabs.tt.gamemode.ObjectiveDirector.sq(radius))
                {
                    go.add(u);
                }
            }
            order(p, go, hx, hy);
        }
        // Java: 186
        public void marchOnDefenders()
        {
            marching.removeIf(new java.util.function.Predicate_L((object u_l) =>
            {
                com.oddlabs.tt.model.Unit u = (com.oddlabs.tt.model.Unit)u_l;
                if (u.isDead())
                {
                    return true;
                }
                if (u.isMounted())
                {
                    u.setObjectiveHold(false);
                    return true;
                }
                return false;
            }));
            com.oddlabs.tt.player.Player[] players = world.getPlayers();
            foreach (com.oddlabs.tt.player.Player p in players)
            {
                java.util.List go = new java.util.ArrayList();
                float cx = 0.0f;
                float cy = 0.0f;
                foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(marching))
                {
                    if (((object)u_2.getOwner() == (object)p) && !u_2.isMoving())
                    {
                        go.add(u_2);
                        cx = (float)(cx + u_2.getPositionX());
                        cy = (float)(cy + u_2.getPositionY());
                    }
                }
                if (go.isEmpty())
                {
                    continue;
                }
                cx = (float)(cx / go.size());
                cy = (float)(cy / go.size());
                com.oddlabs.tt.model.Selectable target = nearestDefender(p, cx, cy);
                if (target == null)
                {
                    foreach (com.oddlabs.tt.model.Unit u_3 in java.lang.JIter.of(go))
                    {
                        u_3.setObjectiveHold(false);
                    }
                    marching.removeAll(go);
                    continue;
                }
                float tx = target.getPositionX();
                float ty = target.getPositionY();
                go.removeIf(new java.util.function.Predicate_L((object u_4_l) =>
                {
                    com.oddlabs.tt.model.Unit u_4 = (com.oddlabs.tt.model.Unit)u_4_l;
                    return com.oddlabs.tt.gamemode.ObjectiveDirector.dist2(u_4.getPositionX(), u_4.getPositionY(), tx, ty) < com.oddlabs.tt.gamemode.ObjectiveDirector.sq(6.0f);
                }));
                order(p, go, tx, ty);
            }
        }
        // Java: 226
        public com.oddlabs.tt.model.Selectable nearestDefender(com.oddlabs.tt.player.Player attacker, float x, float y)
        {
            com.oddlabs.tt.model.Selectable best = null;
            float best_d = java.lang.Float.MAX_VALUE;
            bool best_building = false;
            foreach (com.oddlabs.tt.player.Player d in world.getPlayers())
            {
                if (((object)d == (object)attacker) || !attacker.isEnemy(d) || defender == null || !defender.test(d))
                {
                    continue;
                }
                foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(d.getUnits().getSet()))
                {
                    if (s.isDead() || (s is com.oddlabs.tt.model.Unit u) && u.isMounted())
                    {
                        continue;
                    }
                    bool building = (s is com.oddlabs.tt.model.Building);
                    float dd = com.oddlabs.tt.gamemode.ObjectiveDirector.dist2(s.getPositionX(), s.getPositionY(), x, y);
                    if (building && !best_building || building == best_building && (dd < best_d || dd == best_d && best != null && com.oddlabs.tt.gamemode.ObjectiveDirector.tieBreak(s, best) < 0))
                    {
                        best = s;
                        best_d = dd;
                        best_building = building;
                    }
                }
            }
            return best;
        }
        // Java: 252
        public static java.util.List warriors(com.oddlabs.tt.player.Player p)
        {
            java.util.List list = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(p.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Unit u) && !u.isDead() && !u.isMounted() && u.isWarrior())
                {
                    list.add(u);
                }
            }
            return list;
        }
        // Java: 261
        public void fill(java.util.List mine, java.util.List warriors_v, int quota, int[] targets)
        {
            mine.removeIf(new java.util.function.Predicate_L((object u_l) =>
            {
                com.oddlabs.tt.model.Unit u = (com.oddlabs.tt.model.Unit)u_l;
                if (u.isDead())
                {
                    return true;
                }
                if (u.isMounted())
                {
                    u.setObjectiveHold(false);
                    return true;
                }
                return false;
            }));
            while (mine.size() > quota)
            {
                ((com.oddlabs.tt.model.Unit)mine.removeLast()).setObjectiveHold(false);
            }
            if (mine.size() >= quota)
            {
                return;
            }
            java.util.List free = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(warriors_v))
            {
                if (!u_2.isObjectiveHold())
                {
                    free.add(u_2);
                }
            }
            com.oddlabs.tt.player.Player owner = (warriors_v.isEmpty() ? (com.oddlabs.tt.player.Player)(null) : ((com.oddlabs.tt.model.Unit)warriors_v.getFirst()).getOwner());
            float ox = (owner != null ? owner.getStartX() : 0.0f);
            float oy = (owner != null ? owner.getStartY() : 0.0f);
            free.sort(java.util.Comparator_.comparingDouble(new java.util.function.ToDoubleFunction_L((object u_3_l) =>
            {
                com.oddlabs.tt.model.Unit u_3 = (com.oddlabs.tt.model.Unit)u_3_l;
                return nearestPoint(u_3, targets, ox, oy);
            })).thenComparing(new java.util.Comparator_L((object a0, object a1) => { return com.oddlabs.tt.gamemode.ObjectiveDirector.tieBreak(((com.oddlabs.tt.model.Selectable)a0), ((com.oddlabs.tt.model.Selectable)a1)); })));
            for (int k = 0; k < free.size() && mine.size() < quota; k++)
            {
                com.oddlabs.tt.model.Unit u_4 = ((com.oddlabs.tt.model.Unit)free.get(k));
                u_4.setObjectiveHold(true);
                mine.add(u_4);
            }
        }
        // Java: 292
        public float nearestPoint(com.oddlabs.tt.model.Unit u, int[] targets, float ox, float oy)
        {
            if (targets == null)
            {
                return com.oddlabs.tt.gamemode.ObjectiveDirector.dist2(u.getPositionX(), u.getPositionY(), ox, oy);
            }
            float best = java.lang.Float.MAX_VALUE;
            foreach (int k in targets)
            {
                best = java.lang.Math.min(best, com.oddlabs.tt.gamemode.ObjectiveDirector.dist2(u.getPositionX(), u.getPositionY(), xs[k], ys[k]));
            }
            return best;
        }
        // Java: 301
        public static int tieBreak(com.oddlabs.tt.model.Selectable a, com.oddlabs.tt.model.Selectable b)
        {
            int c = java.lang.Integer.compare(a.getGridY(), b.getGridY());
            return (c != 0 ? c : java.lang.Integer.compare(a.getGridX(), b.getGridX()));
        }
        // Java: 306
        public static void release(java.util.List mine)
        {
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(mine))
            {
                u.setObjectiveHold(false);
            }
            mine.clear();
        }
        // Java: 312
        public void order(com.oddlabs.tt.player.Player p, java.util.List units, float x, float y)
        {
            if (units.isEmpty())
            {
                return;
            }
            p.setDirecting(true);
            try
            {
                p.setLandscapeTarget(((com.oddlabs.tt.model.Selectable[])units.toArray(new com.oddlabs.tt.model.Selectable[0])), com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(x), com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(y), com.oddlabs.tt.model.Action.ATTACK, true);
            }
            finally
            {
                p.setDirecting(false);
            }
        }
        // Java: 324
        public static float dist2(float ax, float ay, float bx, float by)
        {
            float dx = (float)(ax - bx);
            float dy = (float)(ay - by);
            return (float)((float)(dx * dx) + (float)(dy * dy));
        }
        // Java: 330
        public static float sq(float v)
        {
            return (float)(v * v);
        }
        // Java: 335
        public int heldCount()
        {
            int n = 0;
            foreach (java.util.List l in java.lang.JIter.of(held))
            {
                n += l.size();
            }
            return n + marching.size();
        }
        private void init_ObjectiveDirector()
        {
            this.held = new java.util.ArrayList();
            this.marching = new java.util.ArrayList();
        }
        public ObjectiveDirector(com.oddlabs.tt.landscape.World world, float[] xs, float[] ys, float radius, int needed, java.util.function.IntUnaryOperator controller, java.util.function.IntSupplier leader, java.util.function.Predicate defender) : base()
        {
            init_ObjectiveDirector();
            this.world = world;
            this.xs = xs;
            this.ys = ys;
            this.radius = radius;
            this.needed = java.lang.Math.max(1, java.lang.Math.min(needed, xs.Length));
            this.controller = controller;
            this.leader = leader;
            this.defender = defender;
            for (int i = 0; i < world.getPlayers().Length; i++)
            {
                held.add(new java.util.ArrayList());
            }
        }
        // mixin of default method Animated.updateChecksum
        public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
        }
    }
}
