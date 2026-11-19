// Translated from ChieftainManager.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.ultra
{
    // Java: 17
    public class ChieftainManager : java.lang.Object
    {
        public const float STUN_RADIUS_CELLS = 16.5f;
        public com.oddlabs.tt.player.ultra.Context ctx;
        public com.oddlabs.tt.model.Building training_q;
        public float lock_until;
        public float energy_full_since;
        public float last_check;
        public float theta;
        public bool close_cast;
        public float preempt;
        public float toot_tower_value;
        public bool dodge;
        public bool dodge_smart;
        public float dodge_max;
        public float dodge_to;
        public bool toot_after_enemy;
        public bool toot_base_cover;
        public float chief_time;
        public int chief_min_quarters;
        public int chief_keep;
        // Java: 59
        public com.oddlabs.tt.model.Building trainingQuarters()
        {
            if (training_q != null && (training_q.isDead() || !ctx.owner.isTrainingChieftain()))
            {
                training_q = null;
            }
            return training_q;
        }
        // Java: 66
        public int keepInside()
        {
            return chief_keep;
        }
        // Java: 71
        public int peonsWanted()
        {
            com.oddlabs.tt.model.Building q = trainingQuarters();
            if (q == null)
            {
                return 0;
            }
            return java.lang.Math.max(0, chief_keep - q.getUnitContainer().getNumSupplies());
        }
        // Java: 78
        public void tick()
        {
            if (!((ctx.race.getChieftainAI() is com.oddlabs.tt.player.VikingChieftainAI)))
            {
                return;
            }
            com.oddlabs.tt.model.Unit chief = ctx.owner.getChieftain();
            if (chief == null || chief.isDead())
            {
                if ((float)(ctx.now - last_check) >= 2.0f)
                {
                    last_check = ctx.now;
                    maybeTrain();
                }
                return;
            }
            if (ctx.now < lock_until)
            {
                return;
            }
            if ((chief.getCurrentController() is com.oddlabs.tt.model.behaviour.MagicController) || (chief.getCurrentBehaviour() is com.oddlabs.tt.model.behaviour.MagicBehaviour) || (chief.getCurrentBehaviour() is com.oddlabs.tt.model.behaviour.StunBehaviour))
            {
                return;
            }
            float[] pending = ctx.military.pendingEnemyCastNear(chief.getGridX(), chief.getGridY());
            if (dodge && pending != null)
            {
                float dx = (float)(chief.getGridX() - pending[0]);
                float dy = (float)(chief.getGridY() - pending[1]);
                float d = ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy)))));
                if (d >= 1.0f && d <= dodge_max)
                {
                    float[] turns = (dodge_smart ? new float[] { 0.0f, 0.52f, (float)(-0.52f), 1.05f, (float)(-1.05f) } : new float[] { 0.0f });
                    foreach (float turn in turns)
                    {
                        float cos = ((float)(java.lang.StrictMath.cos(turn)));
                        float sin = ((float)(java.lang.StrictMath.sin(turn)));
                        float ux = (float)(((float)((float)(dx * cos) - (float)(dy * sin))) / d);
                        float uy = (float)(((float)((float)(dx * sin) + (float)(dy * cos))) / d);
                        int tx = ctx.map.clamp(java.lang.Math.round((float)(pending[0] + (float)(ux * dodge_to))));
                        int ty = ctx.map.clamp(java.lang.Math.round((float)(pending[1] + (float)(uy * dodge_to))));
                        int[] acc = ctx.map.nearestAccessible(tx, ty);
                        if (acc == null)
                        {
                            continue;
                        }
                        if (dodge_smart && (nearestMannedTower(acc[0], acc[1]) < 17.0f || nearestEnemyWarrior(acc[0], acc[1]) < 9.0f))
                        {
                            continue;
                        }
                        ctx.orders.dodge(chief, acc[0], acc[1], pending[2]);
                        return;
                    }
                }
            }
            if (chief.canDoMagic(com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_STUN))
            {
                if (energy_full_since < 0.0f)
                {
                    energy_full_since = ctx.now;
                }
                if (tryToot(chief, chief.getHitPoints() < 20))
                {
                    return;
                }
            }
            else if (!chief.canDoMagic(com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_STUN))
            {
                energy_full_since = (float)(-1.0f);
            }
            position(chief);
        }
        // Java: 134
        public void maybeTrain()
        {
            if (ctx.owner.isTrainingChieftain() || ctx.owner.hasActiveChieftain())
            {
                return;
            }
            com.oddlabs.tt.model.Building a = ctx.economy.armory();
            if (a == null)
            {
                return;
            }
            int pop = ctx.owner.getUnitCountContainer().getNumSupplies();
            bool trigger = (ctx.model.me.quarters.size() >= chief_min_quarters && ctx.now >= chief_time) || ctx.model.enemy.chieftain != null || pop >= ctx.world.getMaxUnitCount() - 20;
            if (!trigger)
            {
                return;
            }
            com.oddlabs.tt.model.Building best = null;
            float best_f = java.lang.Float.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Building q in java.lang.JIter.of(ctx.model.me.quarters))
            {
                if (q.isDead())
                {
                    continue;
                }
                float f = ctx.map.frontness(q.getGridX(), q.getGridY());
                if (f < best_f)
                {
                    best_f = f;
                    best = q;
                }
            }
            if (best == null)
            {
                return;
            }
            ctx.orders.trainChieftain(best);
            if (ctx.owner.isTrainingChieftain())
            {
                training_q = best;
                ctx.log(new java.util.function.Supplier_L(() =>
                {
                    return "training chieftain";
                }));
            }
        }
        // Java: 166
        public float tootValue(float sx, float sy)
        {
            float w = 0.0f;
            float r2 = (float)(com.oddlabs.tt.player.ultra.ChieftainManager.STUN_RADIUS_CELLS * com.oddlabs.tt.player.ultra.ChieftainManager.STUN_RADIUS_CELLS);
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(ctx.model.enemy.units))
            {
                float dx = (float)(e.getGridX() - sx);
                float dy = (float)(e.getGridY() - sy);
                if ((float)((float)(dx * dx) + (float)(dy * dy)) > r2)
                {
                    continue;
                }
                w = (float)(w + ((com.oddlabs.tt.player.ultra.WorldModel.kindOf(e)).ordinal() switch { 3 /*RUBBER*/ => (float)(1.4f), 2 /*IRON*/ => (float)(1.0f), 1 /*ROCK*/ => (float)(0.6f), 0 /*PEON*/ => (float)(0.2f), 4 /*CHIEFTAIN*/ => (float)(3.0f), _ => throw new System.InvalidOperationException("MatchException") }));
            }
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(ctx.model.enemy.towers))
            {
                if (com.oddlabs.tt.player.ultra.WorldModel.garrisonOf(t) == null)
                {
                    continue;
                }
                float dx_2 = (float)(t.getGridX() - sx);
                float dy_2 = (float)(t.getGridY() - sy);
                if ((float)((float)(dx_2 * dx_2) + (float)(dy_2 * dy_2)) <= r2)
                {
                    w = (float)(w + toot_tower_value);
                }
            }
            return w;
        }
        public int plan_x;
        public int plan_y;
        public float plan_value;
        public float plan_time;
        // Java: 205
        public bool tryToot(com.oddlabs.tt.model.Unit chief, bool wounded)
        {
            int cx = chief.getGridX();
            int cy = chief.getGridY();
            com.oddlabs.tt.model.Unit target = null;
            int target_d = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(ctx.model.enemy.warriors))
            {
                int d = com.oddlabs.tt.player.ultra.MapAnalysis.dist2(e.getGridX(), e.getGridY(), cx, cy);
                if (d < target_d)
                {
                    target_d = d;
                    target = e;
                }
            }
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(ctx.model.enemy.towers))
            {
                if (com.oddlabs.tt.player.ultra.WorldModel.garrisonOf(t) == null)
                {
                    continue;
                }
                int d_2 = com.oddlabs.tt.player.ultra.MapAnalysis.dist2(t.getGridX(), t.getGridY(), cx, cy);
                if (d_2 < target_d)
                {
                    target_d = d_2;
                    target = null;
                    plan_x = t.getGridX();
                    plan_y = t.getGridY();
                }
            }
            if (target_d > 34 * 34)
            {
                return false;
            }
            int tx = (target != null ? target.getGridX() : plan_x);
            int ty = (target != null ? target.getGridY() : plan_y);
            float th = theta;
            if (energy_full_since >= 0.0f && (float)(ctx.now - energy_full_since) > 30.0f)
            {
                th = (float)(theta * 0.7f);
            }
            if (ctx.military.stunSieging())
            {
                th = java.lang.Math.min(th, (float)(0.9f * toot_tower_value));
            }
            float since_enemy = (float)(ctx.now - ctx.military.lastEnemyCast());
            if (toot_after_enemy && since_enemy > 3.9f && since_enemy < 35.0f)
            {
                th = java.lang.Math.min(th, (float)(0.7f * theta));
            }
            bool base_attacked = ctx.military.isDefending();
            com.oddlabs.tt.model.Unit enemy_chief = ctx.model.enemy.chieftain;
            if (enemy_chief != null && !enemy_chief.isDead() && com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(enemy_chief.getGridX(), enemy_chief.getGridY(), cx, cy) <= 30)
            {
                th = (float)(th * preempt);
            }
            float dx = tx - cx;
            float dy = ty - cy;
            float len = ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy)))));
            float best_value = (float)(-1.0f);
            int best_x = cx;
            int best_y = cy;
            for (int step = 0; step <= 16; step += 2)
            {
                int px = (len < 1.0f ? cx : java.lang.Math.round((float)(cx + (float)((float)(dx / len) * step))));
                int py = (len < 1.0f ? cy : java.lang.Math.round((float)(cy + (float)((float)(dy / len) * step))));
                if (step > 0 && (wounded || !ctx.map.isAccessible(px, py)))
                {
                    break;
                }
                if (nearestEnemyWarrior(px, py) < 9.0f && (step > 0 || !(close_cast || wounded)))
                {
                    break;
                }
                if (step > 0 && nearestMannedTower(px, py) < 15.5f)
                {
                    break;
                }
                float sx = (len < 1.0f ? px : (float)(px + (float)((float)(dx / len) * 1.3f)));
                float sy = (len < 1.0f ? py : (float)(py + (float)((float)(dy / len) * 1.3f)));
                float v = (float)(tootValue(sx, sy) - (float)(0.02f * step));
                if (v > best_value)
                {
                    best_value = v;
                    best_x = px;
                    best_y = py;
                }
            }
            if (best_value < th)
            {
                return false;
            }
            int ours = 0;
            foreach (com.oddlabs.tt.model.Unit w in java.lang.JIter.of(ctx.model.me.warriors))
            {
                if (com.oddlabs.tt.player.ultra.MapAnalysis.dist2(w.getGridX(), w.getGridY(), best_x, best_y) <= 20 * 20)
                {
                    ours++;
                }
            }
            if (ours < 3 && base_attacked && toot_base_cover && !baseCastCovered(chief, best_x, best_y))
            {
                return false;
            }
            if (ours < 3 && !base_attacked)
            {
                return false;
            }
            plan_x = best_x;
            plan_y = best_y;
            plan_value = best_value;
            plan_time = ctx.now;
            if (com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(cx, cy, best_x, best_y) <= 1)
            {
                if (ctx.orders.magic(chief, com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_STUN))
                {
                    lock_until = (float)(ctx.now + 6.3f);
                    energy_full_since = (float)(-1.0f);
                    float v_2 = best_value;
                    int o = ours;
                    ctx.log(new java.util.function.Supplier_L(() =>
                    {
                        return java.lang.JString.format("TOOT value=%.1f ours=%d at %d,%d", v_2, o, cx, cy);
                    }));
                    ctx.military.onToot(cx, cy);
                    return true;
                }
                return false;
            }
            if (!ctx.orders.recently(chief, com.oddlabs.tt.player.ultra.Orders.Kind.MOVE, null, best_x, best_y, 1.5f))
            {
                ctx.orders.move(chief, best_x, best_y);
            }
            return true;
        }
        // Java: 309
        public bool baseCastCovered(com.oddlabs.tt.model.Unit chief, int x, int y)
        {
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(ctx.model.me.towers))
            {
                if (!t.isDead() && com.oddlabs.tt.player.ultra.WorldModel.garrisonOf(t) != null && com.oddlabs.tt.player.ultra.MapAnalysis.dist2(t.getGridX(), t.getGridY(), x, y) <= 14 * 14)
                {
                    return true;
                }
            }
            return chief.getHitPoints() < 20 && nearestEnemyWarrior(chief.getGridX(), chief.getGridY()) < 8.0f;
        }
        // Java: 319
        public float nearestMannedTower(int x, int y)
        {
            int best = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(ctx.model.enemy.towers))
            {
                if (!t.isDead() && com.oddlabs.tt.player.ultra.CombatModel.towerValue(t) > 0.0f)
                {
                    best = java.lang.Math.min(best, com.oddlabs.tt.player.ultra.MapAnalysis.dist2(t.getGridX(), t.getGridY(), x, y));
                }
            }
            return (best == java.lang.Integer.MAX_VALUE ? java.lang.Float.MAX_VALUE : ((float)(java.lang.Math.sqrt(best))));
        }
        // Java: 328
        public float nearestEnemyWarrior(int x, int y)
        {
            int best = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(ctx.model.enemy.warriors))
            {
                if (com.oddlabs.tt.player.ultra.WorldModel.isStunned(e))
                {
                    continue;
                }
                best = java.lang.Math.min(best, com.oddlabs.tt.player.ultra.MapAnalysis.dist2(e.getGridX(), e.getGridY(), x, y));
            }
            return (best == java.lang.Integer.MAX_VALUE ? java.lang.Float.MAX_VALUE : ((float)(java.lang.Math.sqrt(best))));
        }
        // Java: 338
        public void position(com.oddlabs.tt.model.Unit chief)
        {
            int cx = chief.getGridX();
            int cy = chief.getGridY();
            com.oddlabs.tt.model.Unit nearest = null;
            int nearest_d = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(ctx.model.enemy.warriors))
            {
                int d = com.oddlabs.tt.player.ultra.MapAnalysis.dist2(e.getGridX(), e.getGridY(), cx, cy);
                if (d < nearest_d)
                {
                    nearest_d = d;
                    nearest = e;
                }
            }
            bool near_enemy = nearest_d <= 9 * 9;
            int[] dest = escortPoint();
            if (chief.getHitPoints() < 25 && !ctx.military.isDefending())
            {
                dest = (ctx.military.rallyPoint() != null ? ctx.military.rallyPoint() : dest);
            }
            bool hunting = (chief.getCurrentController() is com.oddlabs.tt.model.behaviour.HuntController);
            bool idle = (chief.getCurrentController() is com.oddlabs.tt.model.behaviour.IdleController);
            int off = com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(cx, cy, dest[0], dest[1]);
            if (hunting || (near_enemy && !ctx.orders.recently(chief, 1.5f)) || (off > 4 && !ctx.orders.recently(chief, com.oddlabs.tt.player.ultra.Orders.Kind.MOVE, null, dest[0], dest[1], 3.0f)) || (idle && off > 2 && !ctx.orders.recently(chief, 2.0f)))
            {
                if (near_enemy)
                {
                    dest = retreatPoint(cx, cy, nearest);
                }
                ctx.orders.move(chief, dest[0], dest[1]);
            }
        }
        // Java: 367
        public int[] escortPoint()
        {
            int[] a = ctx.military.squadAnchor();
            int[] home = ctx.military.rallyPoint();
            if (a == null)
            {
                int[] f = ctx.military.defenseFront();
                if (f != null)
                {
                    int dx = f[0] - f[2];
                    int dy = f[1] - f[3];
                    float len = ((float)(java.lang.Math.sqrt(dx * dx + dy * dy)));
                    int x = (len < 1.0f ? f[0] : java.lang.Math.round((float)(f[0] + (float)((float)(dx / len) * 6.0f))));
                    int y = (len < 1.0f ? f[1] : java.lang.Math.round((float)(f[1] + (float)((float)(dy / len) * 6.0f))));
                    int[] acc = ctx.map.nearestAccessible(ctx.map.clamp(x), ctx.map.clamp(y));
                    if (acc != null)
                    {
                        return acc;
                    }
                }
                return (home != null ? home : new int[] { ctx.map.home_x, ctx.map.home_y });
            }
            int hx = (home != null ? home[0] : ctx.map.home_x);
            int hy = (home != null ? home[1] : ctx.map.home_y);
            int dx_2 = hx - a[0];
            int dy_2 = hy - a[1];
            float len_2 = ((float)(java.lang.Math.sqrt(dx_2 * dx_2 + dy_2 * dy_2)));
            if (len_2 < 1.0f)
            {
                return a;
            }
            int x_2 = java.lang.Math.round((float)(a[0] + (float)((float)(dx_2 / len_2) * 7.0f)));
            int y_2 = java.lang.Math.round((float)(a[1] + (float)((float)(dy_2 / len_2) * 7.0f)));
            int[] acc_2 = ctx.map.nearestAccessible(ctx.map.clamp(x_2), ctx.map.clamp(y_2));
            return (acc_2 != null ? acc_2 : a);
        }
        // Java: 398
        public int[] retreatPoint(int cx, int cy, com.oddlabs.tt.model.Unit threat)
        {
            if (threat == null)
            {
                return new int[] { cx, cy };
            }
            int dx = cx - threat.getGridX();
            int dy = cy - threat.getGridY();
            float len = ((float)(java.lang.Math.sqrt(dx * dx + dy * dy)));
            if (len < 1.0f)
            {
                return escortPoint();
            }
            int x = java.lang.Math.round((float)(cx + (float)((float)(dx / len) * 10.0f)));
            int y = java.lang.Math.round((float)(cy + (float)((float)(dy / len) * 10.0f)));
            int[] acc = ctx.map.nearestAccessible(ctx.map.clamp(x), ctx.map.clamp(y));
            return (acc != null ? acc : new int[] { cx, cy });
        }
        private void init_ChieftainManager()
        {
            this.energy_full_since = (float)(-1.0f);
            this.plan_time = java.lang.Float.NEGATIVE_INFINITY;
        }
        public ChieftainManager(com.oddlabs.tt.player.ultra.Context ctx) : base()
        {
            init_ChieftainManager();
            this.ctx = ctx;
            chief_time = ctx.strategy.f("chiefTime");
            chief_min_quarters = ctx.strategy.i("chiefMinQuarters");
            chief_keep = ctx.strategy.i("chiefKeep");
            theta = ctx.strategy.f("tootTheta");
            close_cast = ctx.strategy.b("tootCloseCast");
            preempt = ctx.strategy.f("tootPreempt");
            toot_tower_value = ctx.strategy.f("tootTowerValue");
            dodge = ctx.strategy.b("dodge");
            dodge_smart = ctx.strategy.b("dodgeSmart");
            dodge_max = ctx.strategy.f("dodgeMax");
            dodge_to = ctx.strategy.f("dodgeTo");
            toot_after_enemy = ctx.strategy.b("tootAfterEnemy");
            toot_base_cover = ctx.strategy.b("tootBaseCover");
        }
    }
}
