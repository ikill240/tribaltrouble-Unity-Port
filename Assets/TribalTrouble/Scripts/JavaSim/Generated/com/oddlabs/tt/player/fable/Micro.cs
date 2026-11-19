// Translated from Micro.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.fable
{
    // Java: 36
    public class Micro : java.lang.Object
    {
        public const float RANGE_CELLS = 7.9f;
        public const float TERRAIN_BONUS_PER_HEIGHT = 0.0125f;
        public const float CHIEF_KILL_VALUE = 25.0f;
        public com.oddlabs.tt.player.Player player;
        public com.oddlabs.tt.landscape.HeightMap heights;
        public com.oddlabs.tt.player.fable.Orders orders;
        public com.oddlabs.tt.player.fable.Jobs jobs;
        public com.oddlabs.tt.player.fable.Params @params;
        public java.util.Map survive;
        public java.util.List enemies;
        public java.util.List buildings;
        public int retargeted;
        public int moved;
        public int on_buildings;
        public int in_reach;
        // Java: 67
        public float hitChance(com.oddlabs.tt.model.Unit a, com.oddlabs.tt.model.Selectable t)
        {
            float @base = default;
            if (a.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC))
            {
                @base = 0.75f;
            }
            else if (a.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD))
            {
                @base = 0.2f;
            }
            else
            {
                System.Type type = a.getWeaponFactory().getType();
                if (((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)) || ((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RockSpearWeapon)))
                {
                    @base = 0.5f;
                }
                else if (((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)) || ((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RubberSpearWeapon)))
                {
                    @base = 0.95f;
                }
                else
                {
                    @base = 0.75f;
                }
            }
            float dz = (float)(heights.getNearestHeight(a.getPositionX(), a.getPositionY()) - heights.getNearestHeight(t.getPositionX(), t.getPositionY()));
            float terrain = java.lang.Math.clamp((float)(dz * com.oddlabs.tt.player.fable.Micro.TERRAIN_BONUS_PER_HEIGHT), (float)(-0.25f), 0.25f);
            float p = (float)(((float)((float)(@base + terrain) + player.getHitBonus())) * ((float)(1.0f - t.getDefenseChance())));
            return java.lang.Math.clamp(p, 0.0f, 1.0f);
        }
        // Java: 90
        public static float @value(com.oddlabs.tt.model.Unit e)
        {
            if (e.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC))
            {
                int hits = java.lang.Math.max(1, (e.getHitPoints() + 1) / 2);
                return java.lang.Math.min(6.0f, (float)(com.oddlabs.tt.player.fable.Micro.CHIEF_KILL_VALUE / hits));
            }
            if (e.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD))
            {
                com.oddlabs.tt.model.behaviour.Controller c = e.getPrimaryController();
                if ((c is com.oddlabs.tt.model.behaviour.RepairController) || (c is com.oddlabs.tt.model.behaviour.PlaceBuildingController))
                {
                    return 0.9f;
                }
                return 0.3f;
            }
            return com.oddlabs.tt.player.fable.Intel.unitValue(e);
        }
        // Java: 105
        public static com.oddlabs.tt.model.Selectable currentTarget(com.oddlabs.tt.model.Unit u)
        {
            return ((u.getCurrentController() is com.oddlabs.tt.model.behaviour.HuntController hc) ? hc.getTarget() : (com.oddlabs.tt.model.Selectable)(null));
        }
        // Java: 117
        public int fight(float now, java.util.Collection squad, com.oddlabs.tt.player.fable.Intel intel, int fx, int fy, bool siege, bool advance, com.oddlabs.tt.model.Selectable objective, int tag)
        {
            retargeted = 0;
            moved = 0;
            on_buildings = 0;
            in_reach = 0;
            if (squad.isEmpty())
            {
                return 0;
            }
            int minx = java.lang.Integer.MAX_VALUE;
            int miny = java.lang.Integer.MAX_VALUE;
            int maxx = java.lang.Integer.MIN_VALUE;
            int maxy = java.lang.Integer.MIN_VALUE;
            foreach (com.oddlabs.tt.model.Unit w in java.lang.JIter.of(squad))
            {
                if (w.isDead())
                {
                    continue;
                }
                minx = java.lang.Math.min(minx, w.getGridX());
                miny = java.lang.Math.min(miny, w.getGridY());
                maxx = java.lang.Math.max(maxx, w.getGridX());
                maxy = java.lang.Math.max(maxy, w.getGridY());
            }
            if (minx == java.lang.Integer.MAX_VALUE)
            {
                return 0;
            }
            int reach = java.lang.JC.f2i(com.oddlabs.tt.player.fable.Micro.RANGE_CELLS) + 4;
            minx -= reach;
            miny -= reach;
            maxx += reach;
            maxy += reach;
            enemies.clear();
            collect(intel.warriors, minx, miny, maxx, maxy);
            collect(intel.chieftains, minx, miny, maxx, maxy);
            collect(intel.peons, minx, miny, maxx, maxy);
            buildings.clear();
            collectBuildings(intel.towers, minx - 8, miny - 8, maxx + 8, maxy + 8);
            collectBuildings(intel.armories, minx - 8, miny - 8, maxx + 8, maxy + 8);
            collectBuildings(intel.quarters, minx - 8, miny - 8, maxx + 8, maxy + 8);
            collectBuildings(intel.construction_sites, minx - 8, miny - 8, maxx + 8, maxy + 8);
            if (enemies.isEmpty() && buildings.isEmpty())
            {
                java.util.List go = new java.util.ArrayList();
                int hold2 = (advance ? 16 : 64);
                foreach (com.oddlabs.tt.model.Unit w_2 in java.lang.JIter.of(squad))
                {
                    if (!orders.usable(w_2) || com.oddlabs.tt.player.fable.Chieftain.isStunned(w_2))
                    {
                        continue;
                    }
                    com.oddlabs.tt.player.fable.Jobs.Job j = jobs.get(w_2);
                    if (j != null && ((object)j.kind == (object)com.oddlabs.tt.player.fable.Jobs.Kind.MOVE) && (float)(now - j.issued_at) < 5.0f)
                    {
                        continue;
                    }
                    bool same = j != null && ((object)j.kind == (object)com.oddlabs.tt.player.fable.Jobs.Kind.ATTACK_MOVE) && (float)(now - j.issued_at) < 4.0f && com.oddlabs.tt.player.fable.BasePlan.dist2(j.gx, j.gy, fx, fy) <= 16;
                    if (!same && com.oddlabs.tt.player.fable.BasePlan.dist2(w_2.getGridX(), w_2.getGridY(), fx, fy) > hold2)
                    {
                        go.add(w_2);
                    }
                }
                if (!go.isEmpty())
                {
                    orders.attackMove(go, fx, fy);
                    foreach (com.oddlabs.tt.model.Unit w_3 in java.lang.JIter.of(go))
                    {
                        jobs.set(w_3, com.oddlabs.tt.player.fable.Jobs.Kind.ATTACK_MOVE, fx, fy, null, null, now, tag);
                    }
                    moved = go.size();
                }
                return 0;
            }
            survive.clear();
            foreach (com.oddlabs.tt.model.Unit w_4 in java.lang.JIter.of(squad))
            {
                if (w_4.isDead())
                {
                    continue;
                }
                if ((com.oddlabs.tt.player.fable.Micro.currentTarget(w_4) is com.oddlabs.tt.model.Unit t) && !t.isDead())
                {
                    float p = hitChance(w_4, t);
                    survive.merge(t, (float)(1.0f - p), new java.util.function.BiFunction_L((object a_l, object b_l) =>
                    {
                        float? a = (float?)a_l;
                        float? b = (float?)b_l;
                        return (float)(((float)a) * ((float)b));
                    }));
                }
            }
            float r2_near = (float)(((float)(com.oddlabs.tt.player.fable.Micro.RANGE_CELLS + 1.5f)) * ((float)(com.oddlabs.tt.player.fable.Micro.RANGE_CELLS + 1.5f)));
            float r2_in = (float)(com.oddlabs.tt.player.fable.Micro.RANGE_CELLS * com.oddlabs.tt.player.fable.Micro.RANGE_CELLS);
            foreach (com.oddlabs.tt.model.Unit w_5 in java.lang.JIter.of(squad))
            {
                if (!orders.usable(w_5) || com.oddlabs.tt.player.fable.Chieftain.isStunned(w_5) || com.oddlabs.tt.player.fable.Chieftain.isFrozen(w_5))
                {
                    continue;
                }
                if ((w_5.getCurrentBehaviour() is com.oddlabs.tt.model.behaviour.AttackBehaviour))
                {
                    continue;
                }
                com.oddlabs.tt.player.fable.Jobs.Job job = jobs.get(w_5);
                if (job != null && ((object)job.kind == (object)com.oddlabs.tt.player.fable.Jobs.Kind.MOVE) && (float)(now - job.issued_at) < 5.0f)
                {
                    continue;
                }
                com.oddlabs.tt.model.Selectable current = com.oddlabs.tt.player.fable.Micro.currentTarget(w_5);
                if (current != null && current.isDead())
                {
                    current = null;
                }
                int wx = w_5.getGridX();
                int wy = w_5.getGridY();
                com.oddlabs.tt.model.Unit best = null;
                float best_score = 0.0f;
                float best_p = 0.0f;
                float cur_score = 0.0f;
                int near_d2 = java.lang.Integer.MAX_VALUE;
                int near_x = fx;
                int near_y = fy;
                float own = ((current is com.oddlabs.tt.model.Unit cu) ? hitChance(w_5, cu) : 0.0f);
                foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(enemies))
                {
                    int d2 = com.oddlabs.tt.player.fable.BasePlan.dist2(wx, wy, e.getGridX(), e.getGridY());
                    if (d2 < near_d2)
                    {
                        near_d2 = d2;
                        near_x = e.getGridX();
                        near_y = e.getGridY();
                    }
                    if (d2 > r2_near)
                    {
                        continue;
                    }
                    float p_2 = hitChance(w_5, e);
                    float? s0 = ((float?)survive.get(e));
                    float s = (s0 != null ? ((float)s0) : 1.0f);
                    if (((object)e == (object)current) && own < 0.999f)
                    {
                        s = (float)(s / ((float)(1.0f - own)));
                    }
                    float score = (float)((float)((float)((float)(com.oddlabs.tt.player.fable.Micro.@value(e) * p_2) * java.lang.Math.max(s, 0.05f)) * ((s < 0.3f ? 0.2f : 1.0f))) * ((d2 <= r2_in ? 1.0f : 0.75f)));
                    if (((object)e == (object)current))
                    {
                        cur_score = score;
                    }
                    if (score > best_score)
                    {
                        best_score = score;
                        best = e;
                        best_p = p_2;
                    }
                }
                if (best != null)
                {
                    in_reach++;
                    if (((object)best != (object)current) && (current == null || best_score > (float)(1.25f * cur_score)))
                    {
                        orders.attack(w_5, best);
                        jobs.set(w_5, com.oddlabs.tt.player.fable.Jobs.Kind.HUNT, best.getGridX(), best.getGridY(), best, null, now, tag);
                        survive.merge(best, (float)(1.0f - best_p), new java.util.function.BiFunction_L((object a_2_l, object b_2_l) =>
                        {
                            float? a_2 = (float?)a_2_l;
                            float? b_2 = (float?)b_2_l;
                            return (float)(((float)a_2) * ((float)b_2));
                        }));
                        retargeted++;
                    }
                    continue;
                }
                if ((current is com.oddlabs.tt.model.Unit))
                {
                    continue;
                }
                com.oddlabs.tt.model.LandBuilding target = null;
                float target_score = 0.0f;
                foreach (com.oddlabs.tt.model.LandBuilding b_3 in java.lang.JIter.of(buildings))
                {
                    int d2_2 = com.oddlabs.tt.player.fable.BasePlan.dist2(wx, wy, b_3.getGridX(), b_3.getGridY());
                    bool tower = b_3.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.ATTACK);
                    float range = (tower ? 12.0f : 11.0f);
                    bool in_range = d2_2 <= (float)(range * range);
                    if (!in_range && !siege)
                    {
                        continue;
                    }
                    float score_2 = (float)(((tower ? 3.0f : (((object)b_3 == (object)objective) ? 2.0f : 1.0f))) / ((float)(1.0f + (float)(((float)(java.lang.Math.sqrt(d2_2))) / 10.0f))));
                    if (!b_3.isComplete())
                    {
                        score_2 = (float)(score_2 * 2.0f);
                    }
                    if (score_2 > target_score)
                    {
                        target_score = score_2;
                        target = b_3;
                    }
                }
                if (target != null)
                {
                    if (((object)current != (object)target))
                    {
                        orders.attack(w_5, target);
                        jobs.set(w_5, com.oddlabs.tt.player.fable.Jobs.Kind.HUNT, target.getGridX(), target.getGridY(), target, null, now, tag);
                        on_buildings++;
                    }
                    continue;
                }
                if (current == null)
                {
                    bool stale = job == null || (float)(now - job.issued_at) >= 4.0f;
                    bool idle = ((object)com.oddlabs.tt.player.fable.Roster.activity(w_5) == (object)com.oddlabs.tt.player.fable.Roster.Activity.IDLE) && (job == null || (float)(now - job.issued_at) >= 1.5f);
                    if (!stale && !idle)
                    {
                        continue;
                    }
                    if (job != null && ((object)job.kind == (object)com.oddlabs.tt.player.fable.Jobs.Kind.ATTACK_MOVE) && !idle && com.oddlabs.tt.player.fable.BasePlan.dist2(job.gx, job.gy, near_x, near_y) <= 36)
                    {
                        continue;
                    }
                    if (advance && !enemies.isEmpty())
                    {
                        orders.attackMove(w_5, near_x, near_y);
                        jobs.set(w_5, com.oddlabs.tt.player.fable.Jobs.Kind.ATTACK_MOVE, near_x, near_y, null, null, now, tag);
                        moved++;
                    }
                    else if (com.oddlabs.tt.player.fable.BasePlan.dist2(wx, wy, fx, fy) > ((advance ? 16 : 64)))
                    {
                        orders.attackMove(w_5, fx, fy);
                        jobs.set(w_5, com.oddlabs.tt.player.fable.Jobs.Kind.ATTACK_MOVE, fx, fy, null, null, now, tag);
                        moved++;
                    }
                }
            }
            return in_reach;
        }
        // Java: 291
        public void collect(java.util.List from, int minx, int miny, int maxx, int maxy)
        {
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(from))
            {
                if (e.isDead() || e.isMounted())
                {
                    continue;
                }
                int x = e.getGridX();
                int y = e.getGridY();
                if (x >= minx && x <= maxx && y >= miny && y <= maxy)
                {
                    enemies.add(e);
                }
            }
        }
        // Java: 302
        public void collectBuildings(java.util.List from, int minx, int miny, int maxx, int maxy)
        {
            foreach (com.oddlabs.tt.model.LandBuilding b in java.lang.JIter.of(from))
            {
                if (b.isDead())
                {
                    continue;
                }
                int x = b.getGridX();
                int y = b.getGridY();
                if (x >= minx && x <= maxx && y >= miny && y <= maxy)
                {
                    buildings.add(b);
                }
            }
        }
        // Java: 313
        public override string toString()
        {
            return "micro reach=" + (in_reach) + " retarget=" + (retargeted) + " bld=" + (on_buildings) + " move=" + (moved);
        }
        private void init_Micro()
        {
            this.survive = new java.util.LinkedHashMap();
            this.enemies = new java.util.ArrayList();
            this.buildings = new java.util.ArrayList();
        }
        public Micro(com.oddlabs.tt.player.Player player, com.oddlabs.tt.player.fable.Orders orders, com.oddlabs.tt.player.fable.Jobs jobs, com.oddlabs.tt.player.fable.Params @params) : base()
        {
            init_Micro();
            this.player = player;
            this.heights = player.getWorld().getHeightMap();
            this.orders = orders;
            this.jobs = jobs;
            this.@params = @params;
        }
    }
}
