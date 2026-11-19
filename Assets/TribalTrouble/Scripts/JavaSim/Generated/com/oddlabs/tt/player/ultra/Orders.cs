// Translated from Orders.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.ultra
{
    // Java: 30
    public class Orders : java.lang.Object
    {
        // Java: 31
        public sealed class Kind : java.lang.Enum
        {
            public static Kind[] values() { return (Kind[])VALUES_.Clone(); }
            public static Kind valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly Kind MOVE = new Kind("MOVE", 0);
            public static readonly Kind ATTACK_MOVE = new Kind("ATTACK_MOVE", 1);
            public static readonly Kind ATTACK = new Kind("ATTACK", 2);
            public static readonly Kind GATHER = new Kind("GATHER", 3);
            public static readonly Kind BUILD = new Kind("BUILD", 4);
            public static readonly Kind REPAIR = new Kind("REPAIR", 5);
            public static readonly Kind ENTER = new Kind("ENTER", 6);
            public static readonly Kind STOP = new Kind("STOP", 7);
            public static readonly Kind DODGE = new Kind("DODGE", 8);
            public static readonly Kind[] VALUES_ = new Kind[] { MOVE, ATTACK_MOVE, ATTACK, GATHER, BUILD, REPAIR, ENTER, STOP, DODGE };
            public Kind(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        // Java: 44
        public class Last : java.lang.Object
        {
            public com.oddlabs.tt.player.ultra.Orders.Kind kind() { return kind_f; }
            public object target() { return target_f; }
            public int x() { return x_f; }
            public int y() { return y_f; }
            public float time() { return time_f; }
            public Last(com.oddlabs.tt.player.ultra.Orders.Kind kind, object target, int x, int y, float time)
            {
                this.kind_f = kind;
                this.target_f = target;
                this.x_f = x;
                this.y_f = y;
                this.time_f = time;
            }
            public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is Last r)) return false; return true && java.lang.JO.equals(kind_f, r.kind_f) && java.lang.JO.equals(target_f, r.target_f) && java.lang.JO.equals(x_f, r.x_f) && java.lang.JO.equals(y_f, r.y_f) && java.lang.JO.equals(time_f, r.time_f); }
            public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(kind_f); h = 31 * h + java.lang.JO.hashCode(target_f); h = 31 * h + java.lang.JO.hashCode(x_f); h = 31 * h + java.lang.JO.hashCode(y_f); h = 31 * h + java.lang.JO.hashCode(time_f); } return h; }
            public override string toString() { return "Last[" + "kind=" + java.lang.JO.toString(kind_f) + ", target=" + java.lang.JO.toString(target_f) + ", x=" + java.lang.JO.toString(x_f) + ", y=" + java.lang.JO.toString(y_f) + ", time=" + java.lang.JO.toString(time_f) + "]"; }
            public com.oddlabs.tt.player.ultra.Orders.Kind kind_f;
            public object target_f;
            public int x_f;
            public int y_f;
            public float time_f;
        }
        public com.oddlabs.tt.player.ultra.Context ctx;
        public com.oddlabs.tt.player.Player p;
        public java.util.Map last;
        public int issued;
        // Java: 61
        public bool canOrder(com.oddlabs.tt.model.Unit u)
        {
            if (u == null || u.isDead() || u.isMounted() || ((object)u.getOwner() != (object)p) || (u.getCurrentBehaviour() is com.oddlabs.tt.model.behaviour.StunBehaviour))
            {
                return false;
            }
            float? hold = ((float?)hold_until.get(u));
            return hold == null || ctx.now >= ((float)hold);
        }
        public java.util.Map hold_until;
        // Java: 73
        public void dodge(com.oddlabs.tt.model.Unit u, int gx, int gy, float until)
        {
            if (!canOrder(u) || !ctx.map.inside(gx, gy))
            {
                return;
            }
            p.setLandscapeTarget(com.oddlabs.tt.model.Selectable.newArray(u), gx, gy, com.oddlabs.tt.model.Action.MOVE, false);
            @record(u, com.oddlabs.tt.player.ultra.Orders.Kind.DODGE, null, gx, gy);
            hold_until.put(u, until);
        }
        // Java: 81
        public static bool alive(com.oddlabs.tt.util.Target t)
        {
            return t != null && !t.isDead();
        }
        // Java: 85
        public com.oddlabs.tt.player.ultra.Orders.Last lastOrder(com.oddlabs.tt.model.Unit u)
        {
            return ((com.oddlabs.tt.player.ultra.Orders.Last)last.get(u));
        }
        // Java: 94
        public bool recently(com.oddlabs.tt.model.Unit u, com.oddlabs.tt.player.ultra.Orders.Kind kind, object target, int x, int y, float window)
        {
            com.oddlabs.tt.player.ultra.Orders.Last l = ((com.oddlabs.tt.player.ultra.Orders.Last)last.get(u));
            if (l == null || ((object)l.kind_f != (object)kind) || ((object)l.target_f != (object)target) || (float)(ctx.now - l.time_f) >= window)
            {
                return false;
            }
            if (target != null)
            {
                return true;
            }
            int dist = com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(u.getGridX(), u.getGridY(), x, y);
            int tolerance = java.lang.Math.max(2, dist / 8);
            return com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(l.x_f, l.y_f, x, y) <= tolerance;
        }
        // Java: 105
        public bool recently(com.oddlabs.tt.model.Unit u, float window)
        {
            com.oddlabs.tt.player.ultra.Orders.Last l = ((com.oddlabs.tt.player.ultra.Orders.Last)last.get(u));
            return l != null && (float)(ctx.now - l.time_f) < window;
        }
        // Java: 111
        public void releaseHolds()
        {
            hold_until.clear();
        }
        // Java: 115
        public void prune()
        {
            last.keySet().removeIf(new java.util.function.Predicate_L((object a0) => { return ((com.oddlabs.tt.model.Unit)a0).isDead(); }));
            hold_until.entrySet().removeIf(new java.util.function.Predicate_L((object e_l) =>
            {
                java.util.Map.Entry e = (java.util.Map.Entry)e_l;
                return ((com.oddlabs.tt.model.Unit)e.getKey()).isDead() || ctx.now >= ((float)((float?)e.getValue()));
            }));
        }
        // Java: 120
        public void @record(com.oddlabs.tt.model.Unit u, com.oddlabs.tt.player.ultra.Orders.Kind kind, object target, int x, int y)
        {
            last.put(u, new com.oddlabs.tt.player.ultra.Orders.Last(kind, target, x, y, ctx.now));
            issued++;
        }
        // Java: 128
        public void move(com.oddlabs.tt.model.Unit u, int gx, int gy)
        {
            if (!canOrder(u) || !ctx.map.inside(gx, gy))
            {
                return;
            }
            p.setLandscapeTarget(com.oddlabs.tt.model.Selectable.newArray(u), gx, gy, com.oddlabs.tt.model.Action.MOVE, false);
            @record(u, com.oddlabs.tt.player.ultra.Orders.Kind.MOVE, null, gx, gy);
        }
        // Java: 135
        public void attackMove(com.oddlabs.tt.model.Unit u, int gx, int gy)
        {
            if (!canOrder(u) || !ctx.map.inside(gx, gy))
            {
                return;
            }
            p.setLandscapeTarget(com.oddlabs.tt.model.Selectable.newArray(u), gx, gy, com.oddlabs.tt.model.Action.ATTACK, true);
            @record(u, com.oddlabs.tt.player.ultra.Orders.Kind.ATTACK_MOVE, null, gx, gy);
        }
        // Java: 143
        public void group(java.util.List units, int gx, int gy, bool attack_v)
        {
            if (!ctx.map.inside(gx, gy))
            {
                return;
            }
            int n = 0;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(units))
            {
                if (canOrder(u))
                {
                    n++;
                }
            }
            if (n == 0)
            {
                return;
            }
            com.oddlabs.tt.model.Selectable[] sel = com.oddlabs.tt.model.Selectable.newArray(n);
            int i = 0;
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(units))
            {
                if (canOrder(u_2))
                {
                    sel[i++] = u_2;
                    @record(u_2, (attack_v ? com.oddlabs.tt.player.ultra.Orders.Kind.ATTACK_MOVE : com.oddlabs.tt.player.ultra.Orders.Kind.MOVE), null, gx, gy);
                }
            }
            p.setLandscapeTarget(sel, gx, gy, (attack_v ? com.oddlabs.tt.model.Action.ATTACK : com.oddlabs.tt.model.Action.MOVE), attack_v);
        }
        // Java: 164
        public void attack(com.oddlabs.tt.model.Unit u, com.oddlabs.tt.model.Selectable target)
        {
            if (!canOrder(u) || !com.oddlabs.tt.player.ultra.Orders.alive(target) || ((object)target.getOwner() == (object)p))
            {
                return;
            }
            if ((target is com.oddlabs.tt.model.Unit t) && t.isMounted())
            {
                return;
            }
            p.setTarget(com.oddlabs.tt.model.Selectable.newArray(u), target, com.oddlabs.tt.model.Action.ATTACK, true);
            @record(u, com.oddlabs.tt.player.ultra.Orders.Kind.ATTACK, target, target.getGridX(), target.getGridY());
        }
        // Java: 173
        public void gather(com.oddlabs.tt.model.Unit u, com.oddlabs.tt.model.Supply s)
        {
            if (!canOrder(u) || !com.oddlabs.tt.player.ultra.Orders.alive(s) || s.isEmpty() || !u.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD))
            {
                return;
            }
            p.setTarget(com.oddlabs.tt.model.Selectable.newArray(u), s, com.oddlabs.tt.model.Action.GATHER_REPAIR, false);
            @record(u, com.oddlabs.tt.player.ultra.Orders.Kind.GATHER, s, s.getGridX(), s.getGridY());
        }
        // Java: 181
        public void build(com.oddlabs.tt.model.Unit u, com.oddlabs.tt.model.Building b)
        {
            if (!canOrder(u) || b.isDead() || ((object)b.getOwner() != (object)p) || b.isComplete() || !u.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD))
            {
                return;
            }
            if (!b.isPlaced())
            {
                int half = ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getPlacingSize() / 2 + 2;
                int dx = u.getGridX() - b.getGridX();
                int dy = u.getGridY() - b.getGridY();
                int m = java.lang.Math.max(1, java.lang.Math.max(java.lang.Math.abs(dx), java.lang.Math.abs(dy)));
                int sx = b.getGridX() + java.lang.Math.round((float)((float)(((float)(dx)) * half) / m));
                int sy = b.getGridY() + java.lang.Math.round((float)((float)(((float)(dy)) * half) / m));
                if (java.lang.Math.max(java.lang.Math.abs(dx), java.lang.Math.abs(dy)) > half && !recently(u, com.oddlabs.tt.player.ultra.Orders.Kind.MOVE, null, sx, sy, 4.0f))
                {
                    move(u, sx, sy);
                }
                return;
            }
            p.setTarget(com.oddlabs.tt.model.Selectable.newArray(u), b, com.oddlabs.tt.model.Action.DEFAULT, false);
            @record(u, com.oddlabs.tt.player.ultra.Orders.Kind.BUILD, b, b.getGridX(), b.getGridY());
        }
        // Java: 202
        public void repair(com.oddlabs.tt.model.Unit u, com.oddlabs.tt.model.Building b)
        {
            if (!canOrder(u) || b.isDead() || ((object)b.getOwner() != (object)p) || !b.isPlaced() || !b.isDamaged() || !u.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD))
            {
                return;
            }
            p.setTarget(com.oddlabs.tt.model.Selectable.newArray(u), b, com.oddlabs.tt.model.Action.GATHER_REPAIR, false);
            @record(u, com.oddlabs.tt.player.ultra.Orders.Kind.REPAIR, b, b.getGridX(), b.getGridY());
        }
        // Java: 211
        public void enter(com.oddlabs.tt.model.Unit u, com.oddlabs.tt.model.Building b)
        {
            if (!canOrder(u) || b.isDead() || ((object)b.getOwner() != (object)p) || !b.isComplete() || b.getUnitContainer() == null)
            {
                return;
            }
            if (ctx.economy != null && ctx.economy.shelterBlocked(u, b))
            {
                int[] r = ctx.military.rearPoint();
                if (r != null)
                {
                    if (ctx.military.inDanger(r[0], r[1]))
                    {
                        if (ctx.military.inDanger(u.getGridX(), u.getGridY()) && !recently(u, 2.5f))
                        {
                            int[] away = ctx.military.awayFromEnemy(u.getGridX(), u.getGridY(), 14);
                            move(u, away[0], away[1]);
                        }
                        return;
                    }
                    if (!recently(u, com.oddlabs.tt.player.ultra.Orders.Kind.MOVE, null, r[0], r[1], 3.0f))
                    {
                        move(u, r[0], r[1]);
                    }
                    return;
                }
            }
            p.setTarget(com.oddlabs.tt.model.Selectable.newArray(u), b, com.oddlabs.tt.model.Action.MOVE, false);
            @record(u, com.oddlabs.tt.player.ultra.Orders.Kind.ENTER, b, b.getGridX(), b.getGridY());
        }
        // Java: 235
        public void stop(com.oddlabs.tt.model.Unit u)
        {
            if (!canOrder(u))
            {
                return;
            }
            p.setTarget(com.oddlabs.tt.model.Selectable.newArray(u), u, com.oddlabs.tt.model.Action.DEFAULT, false);
            @record(u, com.oddlabs.tt.player.ultra.Orders.Kind.STOP, null, u.getGridX(), u.getGridY());
        }
        // Java: 249
        public com.oddlabs.tt.model.Building place(java.util.List builders, int template_id, int gx, int gy)
        {
            int n = 0;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(builders))
            {
                if (canOrder(u) && u.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD))
                {
                    n++;
                }
            }
            if (n == 0 || !p.canBuild(template_id) || !com.oddlabs.tt.model.LandBuilding.isPlacingLegal(ctx.grid, ctx.race.getBuildingTemplate(template_id), gx, gy))
            {
                return null;
            }
            com.oddlabs.tt.model.Building b = ctx.race.getBuildingTemplate(template_id).create(p, gx, gy);
            com.oddlabs.tt.model.Selectable[] sel = com.oddlabs.tt.model.Selectable.newArray(n);
            int i = 0;
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(builders))
            {
                if (canOrder(u_2) && u_2.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD))
                {
                    sel[i++] = u_2;
                    @record(u_2, com.oddlabs.tt.player.ultra.Orders.Kind.BUILD, b, gx, gy);
                }
            }
            p.setTarget(sel, b, com.oddlabs.tt.model.Action.DEFAULT, false);
            return b;
        }
        // Java: 273
        public static bool complete(com.oddlabs.tt.model.Building b)
        {
            return b != null && !b.isDead() && b.isComplete();
        }
        // Java: 277
        public void deploy(com.oddlabs.tt.model.Building b, com.oddlabs.tt.model.DeployType type, int n)
        {
            if (!com.oddlabs.tt.player.ultra.Orders.complete(b) || n == 0 || ((object)b.getOwner() != (object)p))
            {
                return;
            }
            com.oddlabs.tt.model.DeployContainer c = b.getDeployContainer(type);
            if (c == null)
            {
                return;
            }
            p.deployUnits(b, type, n);
            issued++;
        }
        // Java: 288
        public void weapons(com.oddlabs.tt.model.Building armory, System.Type type, bool on)
        {
            if (!com.oddlabs.tt.player.ultra.Orders.complete(armory) || !armory.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD_ARMIES))
            {
                return;
            }
            com.oddlabs.tt.model.BuildProductionContainer c = ((com.oddlabs.tt.model.BuildProductionContainer)armory.getBuildSupplyContainer(type));
            if (c == null)
            {
                return;
            }
            if (on)
            {
                if (c.isInfinite() && c.getNumSupplies() >= 30)
                {
                    return;
                }
                if (((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)))
                {
                    p.buildRockWeapons(armory, 30, true);
                }
                else if (((object)type == (object)typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)))
                {
                    p.buildIronWeapons(armory, 30, true);
                }
                else if (((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)))
                {
                    p.buildRubberWeapons(armory, 30, true);
                }
            }
            else
            {
                if (!c.isInfinite() && c.getNumSupplies() == 0)
                {
                    return;
                }
                if (((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)))
                {
                    p.buildRockWeapons(armory, (-30), false);
                }
                else if (((object)type == (object)typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)))
                {
                    p.buildIronWeapons(armory, (-30), false);
                }
                else if (((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)))
                {
                    p.buildRubberWeapons(armory, (-30), false);
                }
            }
            issued++;
        }
        // Java: 320
        public void recallGatherers(com.oddlabs.tt.model.Building armory, System.Type type, int n)
        {
            if (!com.oddlabs.tt.player.ultra.Orders.complete(armory) || n <= 0 || !typeof(com.oddlabs.tt.model.Supply).isAssignableFrom(type))
            {
                return;
            }
            java.util.List picks = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(ctx.model.me.peons))
            {
                if (canOrder(u) && (com.oddlabs.tt.player.ultra.WorldModel.primary(u) is com.oddlabs.tt.model.behaviour.GatherController gc) && ((object)gc.getSupplyType() == (object)type) && ((object)gc.getAssignedBuilding() == (object)armory))
                {
                    picks.add(u);
                }
            }
            picks.sort(java.util.Comparator_.comparingInt(new java.util.function.ToIntFunction_L((object u_2_l) =>
            {
                com.oddlabs.tt.model.Unit u_2 = (com.oddlabs.tt.model.Unit)u_2_l;
                return com.oddlabs.tt.player.ultra.MapAnalysis.dist2(u_2.getGridX(), u_2.getGridY(), armory.getGridX(), armory.getGridY());
            })));
            for (int i = 0; i < java.lang.Math.min(n, picks.size()); i++)
            {
                enter(((com.oddlabs.tt.model.Unit)picks.get(i)), armory);
            }
        }
        // Java: 335
        public void rally(com.oddlabs.tt.model.Building b, int gx, int gy)
        {
            if (!com.oddlabs.tt.player.ultra.Orders.complete(b) || !ctx.map.inside(gx, gy) || !p.canSetRallyPoints())
            {
                return;
            }
            com.oddlabs.tt.util.Target r = b.getRallyPoint();
            if (r != null && ((object)r != (object)b) && !((r is com.oddlabs.tt.model.Building)) && r.getGridX() == gx && r.getGridY() == gy)
            {
                return;
            }
            p.setRallyPoint(b, gx, gy);
            issued++;
        }
        // Java: 345
        public void clearRally(com.oddlabs.tt.model.Building b)
        {
            if (!com.oddlabs.tt.player.ultra.Orders.complete(b) || !b.hasRallyPoint() || !p.canSetRallyPoints())
            {
                return;
            }
            p.setRallyPoint(b, b);
            issued++;
        }
        // Java: 353
        public void towerTarget(com.oddlabs.tt.model.Building tower, com.oddlabs.tt.model.Selectable target)
        {
            if (!com.oddlabs.tt.player.ultra.Orders.complete(tower) || !com.oddlabs.tt.player.ultra.Orders.alive(target) || !tower.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.ATTACK) || tower.getUnitCount() == 0)
            {
                return;
            }
            if ((target is com.oddlabs.tt.model.Unit t) && t.isMounted())
            {
                return;
            }
            com.oddlabs.tt.model.Unit g = com.oddlabs.tt.player.ultra.WorldModel.garrisonOf(tower);
            if (g != null && com.oddlabs.tt.player.ultra.WorldModel.isStunned(g))
            {
                return;
            }
            p.setTarget(com.oddlabs.tt.model.Selectable.newArray(tower), target, com.oddlabs.tt.model.Action.ATTACK, false);
            issued++;
        }
        // Java: 366
        public bool exitTower(com.oddlabs.tt.model.Building tower)
        {
            if (!com.oddlabs.tt.player.ultra.Orders.complete(tower) || !tower.canExitTower())
            {
                return false;
            }
            p.exitTower(tower);
            issued++;
            return true;
        }
        // Java: 375
        public bool magic(com.oddlabs.tt.model.Unit chief, int index)
        {
            if (chief == null || chief.isDead() || ((object)chief.getOwner() != (object)p) || !chief.canDoMagic(index) || com.oddlabs.tt.player.ultra.WorldModel.isStunned(chief))
            {
                return false;
            }
            p.doMagic(chief, index);
            issued++;
            return (chief.getCurrentController() is com.oddlabs.tt.model.behaviour.MagicController);
        }
        // Java: 386
        public void trainChieftain(com.oddlabs.tt.model.Building quarters)
        {
            if (!com.oddlabs.tt.player.ultra.Orders.complete(quarters) || ((com.oddlabs.tt.model.BuildingTemplate)quarters.getTemplate()).getTemplateID() != com.oddlabs.tt.model.Race.BUILDING_QUARTERS || !quarters.canBuildChieftain())
            {
                return;
            }
            p.trainChieftain(quarters, true);
            issued++;
        }
        private void init_Orders()
        {
            this.last = new java.util.LinkedHashMap();
            this.hold_until = new java.util.LinkedHashMap();
        }
        public Orders(com.oddlabs.tt.player.ultra.Context ctx) : base()
        {
            init_Orders();
            this.ctx = ctx;
            this.p = ctx.owner;
        }
    }
}
