// Translated from ExtendedTactics.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player
{
    // Java: 41
    public class ExtendedTactics : java.lang.Object, com.oddlabs.tt.animation.Animated
    {
        public const float PERIOD = 5.0f;
        public const float PATROL_TIME = 180.0f;
        public const float TOWER_TIME = 240.0f;
        public const float RAID_TIME = 420.0f;
        public com.oddlabs.tt.player.Player owner;
        public com.oddlabs.tt.player.AI ai;
        public bool gauntlet;
        public float time;
        public float next;
        public java.util.List patrol_f;
        public com.oddlabs.tt.model.LandBuilding tower_site;
        public float next_tower;
        public java.util.List flank_f;
        public int flank_tx;
        public int flank_ty;
        public int flank_wx;
        public int flank_wy;
        public float flank_since;
        public java.util.Map flanked;
        public java.util.List raid_f;
        public float raid_since;
        public float next_raid;
        public static int stat_patrolled;
        public static int stat_towers;
        public static int stat_flanks;
        public static int stat_raids;
        // Java: 76
        public void lateStart(float seconds)
        {
            time = seconds;
            next = (float)(seconds + com.oddlabs.tt.player.ExtendedTactics.PERIOD);
            next_tower = java.lang.Math.max(next_tower, (float)(seconds + 30.0f));
            next_raid = java.lang.Math.max(next_raid, (float)(seconds + 60.0f));
        }
        // Java: 83
        public void animate(float t)
        {
            time += t;
            if (time < next)
            {
                return;
            }
            next = (float)(time + com.oddlabs.tt.player.ExtendedTactics.PERIOD);
            if (owner.getUnits().getSet().isEmpty())
            {
                return;
            }
            com.oddlabs.tt.player.Player enemy = nearestEnemy();
            if (enemy == null)
            {
                return;
            }
            java.util.List warriors_v = warriors();
            patrol(enemy, warriors_v);
            towers(enemy, warriors_v);
            flank(warriors_v);
            raid(enemy, warriors_v);
        }
        // Java: 103
        public com.oddlabs.tt.player.Player nearestEnemy()
        {
            com.oddlabs.tt.player.Player best = null;
            float bd = java.lang.Float.MAX_VALUE;
            foreach (com.oddlabs.tt.player.Player p in owner.getWorld().getPlayers())
            {
                if (((object)p == (object)owner) || !owner.isEnemy(p) || p.getUnits().getSet().isEmpty())
                {
                    continue;
                }
                float dx = (float)(p.getStartX() - owner.getStartX());
                float dy = (float)(p.getStartY() - owner.getStartY());
                float d = (float)((float)(dx * dx) + (float)(dy * dy));
                if (d < bd)
                {
                    bd = d;
                    best = p;
                }
            }
            return best;
        }
        // Java: 119
        public bool special(com.oddlabs.tt.model.Unit u)
        {
            com.oddlabs.tt.model.Race r = owner.getRace();
            com.oddlabs.tt.model.UnitTemplate t = ((com.oddlabs.tt.model.UnitTemplate)((com.oddlabs.tt.model.UnitTemplate)u.getTemplate()));
            return ((object)t == (object)r.getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON)) || ((object)t == (object)r.getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_CHIEFTAIN)) || ((object)t == (object)r.getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_MINOR_GOD)) || ((object)t == (object)r.getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_MAJOR_GOD));
        }
        // Java: 127
        public java.util.List warriors()
        {
            java.util.List l = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(owner.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Unit u) && !u.isDead() && !u.isMounted() && !special(u) && (u.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.THROW) || u.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MELEE)))
                {
                    l.add(u);
                }
            }
            return l;
        }
        // Java: 136
        public static bool idle(com.oddlabs.tt.model.Unit u)
        {
            return !u.isObjectiveHold() && (u.getPrimaryController() is com.oddlabs.tt.model.behaviour.IdleController) && !u.isMoving();
        }
        // Java: 140
        public void release(java.util.List units)
        {
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(units))
            {
                if (!u.isDead())
                {
                    u.setObjectiveHold(false);
                }
            }
            units.clear();
        }
        // Java: 147
        public com.oddlabs.tt.util.Target ground(float x, float y)
        {
            com.oddlabs.tt.pathfinder.UnitGrid g = owner.getWorld().getUnitGrid();
            int gx = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(x);
            int gy = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(y);
            int n = g.getGridSize();
            if (gx < 2 || gy < 2 || gx >= n - 2 || gy >= n - 2 || g.getRegion(gx, gy) == null)
            {
                return null;
            }
            com.oddlabs.tt.util.Target[] t = g.findGridTargets(gx, gy, 1, false);
            return (t.Length > 0 ? t[0] : (com.oddlabs.tt.util.Target)(null));
        }
        // Java: 156
        public void order(java.util.List units, int gx, int gy, bool attack)
        {
            owner.setDirecting(true);
            try
            {
                owner.setLandscapeTarget(((com.oddlabs.tt.model.Selectable[])units.toArray(new com.oddlabs.tt.model.Selectable[0])), gx, gy, (attack ? com.oddlabs.tt.model.Action.ATTACK : com.oddlabs.tt.model.Action.DEFAULT), attack);
            }
            finally
            {
                owner.setDirecting(false);
            }
        }
        // Java: 166
        public float[] front(com.oddlabs.tt.player.Player enemy)
        {
            float dx = (float)(enemy.getStartX() - owner.getStartX());
            float dy = (float)(enemy.getStartY() - owner.getStartY());
            float l = ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy)))));
            return (l < 0.001f ? new float[] { 1.0f, 0.0f } : new float[] { (float)(dx / l), (float)(dy / l) });
        }
        // Java: 174
        public void patrol(com.oddlabs.tt.player.Player enemy, java.util.List warriors_v)
        {
            patrol_f.removeIf(new java.util.function.Predicate_L((object u_l) =>
            {
                com.oddlabs.tt.model.Unit u = (com.oddlabs.tt.model.Unit)u_l;
                return u.isDead() || u.isMounted() || !u.isObjectiveHold();
            }));
            if (time < com.oddlabs.tt.player.ExtendedTactics.PATROL_TIME || warriors_v.size() < 8)
            {
                release(patrol_f);
                return;
            }
            int want = java.lang.Math.min((gauntlet ? 6 : 4), warriors_v.size() / 6);
            float[] f = front(enemy);
            float cx = (float)(owner.getStartX() + (float)(f[0] * 34.0f));
            float cy = (float)(owner.getStartY() + (float)(f[1] * 34.0f));
            com.oddlabs.tt.util.Target a = ground((float)(cx - (float)(f[1] * 22.0f)), (float)(cy + (float)(f[0] * 22.0f)));
            com.oddlabs.tt.util.Target b = ground((float)(cx + (float)(f[1] * 22.0f)), (float)(cy - (float)(f[0] * 22.0f)));
            if (a == null || b == null)
            {
                return;
            }
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(warriors_v))
            {
                if (patrol_f.size() >= want)
                {
                    break;
                }
                if (com.oddlabs.tt.player.ExtendedTactics.idle(u_2) && !patrol_f.contains(u_2))
                {
                    u_2.setObjectiveHold(true);
                    patrol_f.add(u_2);
                    com.oddlabs.tt.player.ExtendedTactics.stat_patrolled++;
                }
            }
            foreach (com.oddlabs.tt.model.Unit u_3 in java.lang.JIter.of(patrol_f))
            {
                if (!((u_3.getPrimaryController() is com.oddlabs.tt.model.behaviour.PatrolController)) && (u_3.getPrimaryController() is com.oddlabs.tt.model.behaviour.IdleController))
                {
                    u_3.pushController(new com.oddlabs.tt.model.behaviour.PatrolController(u_3, a, b));
                }
            }
        }
        // Java: 203
        public int towerCount()
        {
            int n = 0;
            com.oddlabs.tt.model.BuildingTemplate tt = owner.getRace().getBuildingTemplate(com.oddlabs.tt.model.Race.BUILDING_TOWER);
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(owner.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.LandBuilding b) && !b.isDead() && ((object)((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()) == (object)tt))
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 212
        public void towers(com.oddlabs.tt.player.Player enemy, java.util.List warriors_v)
        {
            if (time < next_tower)
            {
                return;
            }
            next_tower = (float)(time + 30.0f);
            int want = java.lang.Math.min((gauntlet ? 6 : 4), 1 + java.lang.JC.f2i(((float)(((float)(time - com.oddlabs.tt.player.ExtendedTactics.TOWER_TIME)) / ((gauntlet ? 200.0f : 260.0f))))));
            int have = towerCount();
            if (have > 0)
            {
                ai.manTowers(have);
            }
            if (tower_site != null && (tower_site.isDead() || tower_site.isComplete()))
            {
                tower_site = null;
            }
            if (have >= want || !owner.canBuild(com.oddlabs.tt.model.Race.BUILDING_TOWER))
            {
                return;
            }
            com.oddlabs.tt.model.BuildingTemplate template = owner.getRace().getBuildingTemplate(com.oddlabs.tt.model.Race.BUILDING_TOWER);
            com.oddlabs.tt.pathfinder.UnitGrid grid = owner.getWorld().getUnitGrid();
            if (tower_site == null)
            {
                float[] f = front(enemy);
                int k = have;
                float side = ((k == 0 ? 0.0f : (float)(((k % 2 == 1 ? 1.0f : (float)(-1.0f))) * ((float)(14.0f + (float)(8.0f * ((k - 1) / 2)))))));
                float dist = (float)(30.0f + (float)(6.0f * ((k + 1) / 2)));
                float x = (float)((float)(owner.getStartX() + (float)(f[0] * dist)) - (float)(f[1] * side));
                float y = (float)((float)(owner.getStartY() + (float)(f[1] * dist)) + (float)(f[0] * side));
                int cx = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(x);
                int cy = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(y);
                int n = grid.getGridSize();
                for (int r = 0; r <= 8; r++)
                {
                    for (int i = 0; i < java.lang.Math.max(1, 8 * r); i++)
                    {
                        int px = default;
                        int py = default;
                        if (r == 0)
                        {
                            px = cx;
                            py = cy;
                        }
                        else
                        {
                            int s = i / (2 * r);
                            int d = i % (2 * r);
                            px = (s == 0 ? cx - r + d : (s == 1 ? cx + r : (s == 2 ? cx + r - d : cx - r)));
                            py = (s == 0 ? cy - r : (s == 1 ? cy - r + d : (s == 2 ? cy + r : cy + r - d)));
                        }
                        if (px < 4 || py < 4 || px >= n - 4 || py >= n - 4 || grid.getRegion(px, py) == null || !com.oddlabs.tt.model.LandBuilding.isPlacingLegal(grid, template, px, py))
                        {
                            continue;
                        }
                        tower_site = ((com.oddlabs.tt.model.LandBuilding)template.create(owner, px, py));
                        com.oddlabs.tt.player.ExtendedTactics.stat_towers++;
                        goto brk_search_9;
                    }
                }
                brk_search_9: ;
                if (tower_site == null)
                {
                    return;
                }
            }
            java.util.List builders = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Selectable s_2 in java.lang.JIter.of(owner.getUnits().getSet()))
            {
                if (builders.size() < 2 && (s_2 is com.oddlabs.tt.model.Unit u) && !u.isDead() && !u.isMounted() && u.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD) && (u.getPrimaryController() is com.oddlabs.tt.model.behaviour.GatherController g) && ((object)g.getSupplyType() == (object)typeof(com.oddlabs.tt.landscape.TreeSupply)))
                {
                    builders.add(u);
                }
            }
            if (!builders.isEmpty())
            {
                owner.setTarget(((com.oddlabs.tt.model.Selectable[])builders.toArray(new com.oddlabs.tt.model.Selectable[0])), tower_site, com.oddlabs.tt.model.Action.DEFAULT, false);
            }
        }
        // Java: 269
        public void flank(java.util.List warriors_v)
        {
            flank_f.removeIf(new java.util.function.Predicate_L((object u_l) =>
            {
                com.oddlabs.tt.model.Unit u = (com.oddlabs.tt.model.Unit)u_l;
                return u.isDead() || u.isMounted() || !u.isObjectiveHold();
            }));
            if (!flank_f.isEmpty())
            {
                int at = 0;
                foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(flank_f))
                {
                    float dx = (float)(u_2.getPositionX() - com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(flank_wx));
                    float dy = (float)(u_2.getPositionY() - com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(flank_wy));
                    if ((float)((float)(dx * dx) + (float)(dy * dy)) < (float)(14.0f * 14.0f))
                    {
                        at++;
                    }
                }
                if (at * 3 >= flank_f.size() * 2 || (float)(time - flank_since) > 45.0f)
                {
                    java.util.List go = new java.util.ArrayList(flank_f);
                    order(go, flank_tx, flank_ty, true);
                    release(flank_f);
                }
                return;
            }
            if (warriors_v.size() < ((gauntlet ? 12 : 14)))
            {
                return;
            }
            java.util.Map bands = new java.util.HashMap();
            java.util.List order_v = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit u_3 in java.lang.JIter.of(warriors_v))
            {
                if (u_3.isObjectiveHold() || !((u_3.getPrimaryController() is com.oddlabs.tt.model.behaviour.WalkController w)) || !w.isAgressive())
                {
                    continue;
                }
                com.oddlabs.tt.util.Target t = w.getTarget();
                int key = (t.getGridX() / 16) * 4096 + t.getGridY() / 16;
                ((java.util.List)bands.computeIfAbsent(key, new java.util.function.Function_L((object k_l) =>
                {
                    int? k = (int?)k_l;
                    order_v.add(k);
                    return new java.util.ArrayList();
                }))).add(u_3);
            }
            foreach (int key_2 in java.lang.JIter.of(order_v))
            {
                java.util.List band = ((java.util.List)bands.get(key_2));
                float? when = ((float?)flanked.get(key_2));
                if (band.size() < 10 || when != null && (float)(time - ((float)when)) < 120.0f)
                {
                    continue;
                }
                com.oddlabs.tt.util.Target t_2 = (((com.oddlabs.tt.model.behaviour.WalkController)((com.oddlabs.tt.model.Unit)band.get(0)).getPrimaryController())).getTarget();
                float tx = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(t_2.getGridX());
                float ty = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(t_2.getGridY());
                float mx = 0;
                float my = 0;
                foreach (com.oddlabs.tt.model.Unit u_4 in java.lang.JIter.of(band))
                {
                    mx = (float)(mx + u_4.getPositionX());
                    my = (float)(my + u_4.getPositionY());
                }
                mx = (float)(mx / band.size());
                my = (float)(my / band.size());
                float dx_2 = (float)(tx - mx);
                float dy_2 = (float)(ty - my);
                float l = ((float)(java.lang.Math.sqrt((float)((float)(dx_2 * dx_2) + (float)(dy_2 * dy_2)))));
                if (l < 50.0f)
                {
                    continue;
                }
                dx_2 = (float)(dx_2 / l);
                dy_2 = (float)(dy_2 / l);
                com.oddlabs.tt.util.Target way = null;
                foreach (int side in (gauntlet ? new int[] { 1, (-1) } : new int[] { (-1), 1 }))
                {
                    way = ground((float)((float)(tx - (float)(dx_2 * 18.0f)) + (float)((float)(side * (float)(-dy_2)) * 34.0f)), (float)((float)(ty - (float)(dy_2 * 18.0f)) + (float)((float)(side * dx_2) * 34.0f)));
                    if (way != null)
                    {
                        break;
                    }
                }
                if (way == null)
                {
                    continue;
                }
                band.sort(new java.util.Comparator_L((object a_l, object b_l) =>
                {
                    com.oddlabs.tt.model.Unit a = (com.oddlabs.tt.model.Unit)a_l;
                    com.oddlabs.tt.model.Unit b = (com.oddlabs.tt.model.Unit)b_l;
                    float da = (float)((float)(((float)(a.getPositionX() - tx)) * ((float)(a.getPositionX() - tx))) + (float)(((float)(a.getPositionY() - ty)) * ((float)(a.getPositionY() - ty))));
                    float db = (float)((float)(((float)(b.getPositionX() - tx)) * ((float)(b.getPositionX() - tx))) + (float)(((float)(b.getPositionY() - ty)) * ((float)(b.getPositionY() - ty))));
                    return java.lang.Float.compare(db, da);
                }));
                int n = java.lang.Math.max(3, band.size() * ((gauntlet ? 2 : 1)) / ((gauntlet ? 5 : 3)));
                for (int i = 0; i < n; i++)
                {
                    ((com.oddlabs.tt.model.Unit)band.get(i)).setObjectiveHold(true);
                    flank_f.add(((com.oddlabs.tt.model.Unit)band.get(i)));
                }
                flank_tx = t_2.getGridX();
                flank_ty = t_2.getGridY();
                flank_wx = way.getGridX();
                flank_wy = way.getGridY();
                flank_since = time;
                flanked.put(key_2, time);
                com.oddlabs.tt.player.ExtendedTactics.stat_flanks++;
                order(new java.util.ArrayList(flank_f), flank_wx, flank_wy, false);
                return;
            }
        }
        // Java: 353
        public void raid(com.oddlabs.tt.player.Player enemy, java.util.List warriors_v)
        {
            raid_f.removeIf(new java.util.function.Predicate_L((object u_l) =>
            {
                com.oddlabs.tt.model.Unit u = (com.oddlabs.tt.model.Unit)u_l;
                return u.isDead() || u.isMounted() || !u.isObjectiveHold();
            }));
            if (!raid_f.isEmpty())
            {
                bool busy = false;
                foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(raid_f))
                {
                    busy |= !((u_2.getPrimaryController() is com.oddlabs.tt.model.behaviour.IdleController));
                }
                if (!busy || (float)(time - raid_since) > 70.0f)
                {
                    release(raid_f);
                }
                return;
            }
            if (time < next_raid || warriors_v.size() < ((gauntlet ? 12 : 16)))
            {
                return;
            }
            next_raid = (float)(time + ((gauntlet ? 110.0f : 150.0f)));
            com.oddlabs.tt.model.Unit prey = null;
            float bd = java.lang.Float.MAX_VALUE;
            com.oddlabs.tt.model.UnitTemplate peon = enemy.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON);
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(enemy.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Unit u_3) && !u_3.isDead() && !u_3.isMounted() && ((object)((com.oddlabs.tt.model.UnitTemplate)u_3.getTemplate()) == (object)peon) && (u_3.getPrimaryController() is com.oddlabs.tt.model.behaviour.GatherController))
                {
                    float dx = (float)(u_3.getPositionX() - owner.getStartX());
                    float dy = (float)(u_3.getPositionY() - owner.getStartY());
                    float d = (float)((float)(dx * dx) + (float)(dy * dy));
                    if (d < bd)
                    {
                        bd = d;
                        prey = u_3;
                    }
                }
            }
            if (prey == null)
            {
                return;
            }
            foreach (com.oddlabs.tt.model.Unit u_4 in java.lang.JIter.of(warriors_v))
            {
                if (raid_f.size() >= ((gauntlet ? 5 : 4)))
                {
                    break;
                }
                if (com.oddlabs.tt.player.ExtendedTactics.idle(u_4))
                {
                    u_4.setObjectiveHold(true);
                    raid_f.add(u_4);
                }
            }
            if (raid_f.size() < 3)
            {
                release(raid_f);
                return;
            }
            raid_since = time;
            com.oddlabs.tt.player.ExtendedTactics.stat_raids++;
            owner.setDirecting(true);
            try
            {
                owner.setTarget(((com.oddlabs.tt.model.Selectable[])raid_f.toArray(new com.oddlabs.tt.model.Selectable[0])), prey, com.oddlabs.tt.model.Action.ATTACK, true);
            }
            finally
            {
                owner.setDirecting(false);
            }
        }
        private void init_ExtendedTactics()
        {
            this.next = com.oddlabs.tt.player.ExtendedTactics.PERIOD;
            this.patrol_f = new java.util.ArrayList();
            this.next_tower = com.oddlabs.tt.player.ExtendedTactics.TOWER_TIME;
            this.flank_f = new java.util.ArrayList();
            this.flanked = new java.util.HashMap();
            this.raid_f = new java.util.ArrayList();
            this.next_raid = com.oddlabs.tt.player.ExtendedTactics.RAID_TIME;
        }
        public ExtendedTactics(com.oddlabs.tt.player.Player owner, com.oddlabs.tt.player.AI ai, bool gauntlet) : base()
        {
            init_ExtendedTactics();
            this.owner = owner;
            this.ai = ai;
            this.gauntlet = gauntlet;
            owner.getWorld().getAnimationManagerGameTime().registerAnimation(this);
        }
        // mixin of default method Animated.updateChecksum
        public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
        }
    }
}
