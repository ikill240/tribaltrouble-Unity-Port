// Translated from Chieftain.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.fable
{
    // Java: 25
    public class Chieftain : java.lang.Object
    {
        public com.oddlabs.tt.player.fable.Orders orders;
        public com.oddlabs.tt.player.fable.Params @params;
        public com.oddlabs.tt.player.fable.AiLog log;
        public float last_stun_at;
        public int[] last_stun_cell;
        public float last_blast_at;
        public bool siege_cast_requested;
        public bool hold_for_siege;
        public float last_move_order;
        public int last_move_x;
        public int last_move_y;
        // Java: 49
        public static bool isStunned(com.oddlabs.tt.model.Unit u)
        {
            return !u.isDead() && (u.getCurrentController() is com.oddlabs.tt.model.behaviour.StunController);
        }
        // Java: 57
        public static bool isFrozen(com.oddlabs.tt.model.Unit u)
        {
            return !u.isDead() && (u.getCurrentBehaviour() is com.oddlabs.tt.model.behaviour.StunBehaviour);
        }
        // Java: 61
        public static bool isCasting(com.oddlabs.tt.model.Unit u)
        {
            return !u.isDead() && (u.getCurrentController() is com.oddlabs.tt.model.behaviour.MagicController);
        }
        // Java: 65
        public static bool stunReady(com.oddlabs.tt.model.Unit chief)
        {
            return !chief.isDead() && chief.canDoMagic(com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_STUN);
        }
        // Java: 69
        public static float stunProgress(com.oddlabs.tt.model.Unit chief)
        {
            return (chief.isDead() ? 0.0f : chief.getMagicProgress(com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_STUN));
        }
        // Java: 74
        public float valueInDisc(com.oddlabs.tt.player.fable.Intel intel, int cx, int cy, float radius)
        {
            float r2 = (float)(radius * radius);
            float v = 0.0f;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(intel.warriors))
            {
                if (com.oddlabs.tt.player.fable.BasePlan.dist2(u.getGridX(), u.getGridY(), cx, cy) <= r2 && !com.oddlabs.tt.player.fable.Chieftain.isFrozen(u))
                {
                    v = (float)(v + com.oddlabs.tt.player.fable.Intel.unitValue(u));
                }
            }
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(intel.chieftains))
            {
                if (com.oddlabs.tt.player.fable.BasePlan.dist2(u_2.getGridX(), u_2.getGridY(), cx, cy) <= r2 && !com.oddlabs.tt.player.fable.Chieftain.isFrozen(u_2))
                {
                    v = (float)(v + 3.0f);
                }
            }
            foreach (com.oddlabs.tt.model.LandBuilding t in java.lang.JIter.of(intel.manned_towers))
            {
                if (com.oddlabs.tt.player.fable.BasePlan.dist2(t.getGridX(), t.getGridY(), cx, cy) <= r2 && !com.oddlabs.tt.player.fable.Chieftain.towerStunned(t))
                {
                    v = (float)(v + 4.0f);
                }
            }
            return v;
        }
        // Java: 89
        public int warriorsInDisc(com.oddlabs.tt.player.fable.Intel intel, int cx, int cy, float radius)
        {
            float r2 = (float)(radius * radius);
            int n = 0;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(intel.warriors))
            {
                if (com.oddlabs.tt.player.fable.BasePlan.dist2(u.getGridX(), u.getGridY(), cx, cy) <= r2)
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 98
        public static bool towerStunned(com.oddlabs.tt.model.LandBuilding tower)
        {
            if (tower.isDead() || !tower.isComplete() || tower.getUnitContainer() == null)
            {
                return false;
            }
            if ((tower.getUnitContainer() is com.oddlabs.tt.model.MountUnitContainer m) && m.getUnit() != null)
            {
                return com.oddlabs.tt.player.fable.Chieftain.isFrozen(m.getUnit());
            }
            return false;
        }
        // Java: 107
        public static com.oddlabs.tt.model.Unit nearestEnemyChief(com.oddlabs.tt.player.fable.Intel intel, int gx, int gy)
        {
            return ((com.oddlabs.tt.model.Unit)com.oddlabs.tt.player.fable.Intel.nearest(intel.chieftains, gx, gy));
        }
        // Java: 116
        public bool decideCast(float now, com.oddlabs.tt.model.Unit chief, com.oddlabs.tt.player.fable.Intel intel, com.oddlabs.tt.player.fable.Threat threat, bool fighting, int own_near, int own_units_near_18, bool defending, java.util.List own_warriors)
        {
            if (!orders.usable(chief) || com.oddlabs.tt.player.fable.Chieftain.isCasting(chief) || com.oddlabs.tt.player.fable.Chieftain.isStunned(chief))
            {
                return false;
            }
            int cx = chief.getGridX();
            int cy = chief.getGridY();
            float R = @params.stun_radius_cells;
            bool stun_ready = chief.canDoMagic(com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_STUN);
            bool blast_ready = chief.canDoMagic(com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_BLAST);
            if (!stun_ready && !blast_ready)
            {
                return false;
            }
            com.oddlabs.tt.model.Unit echief = com.oddlabs.tt.player.fable.Chieftain.nearestEnemyChief(intel, cx, cy);
            if (stun_ready && echief != null && !com.oddlabs.tt.player.fable.Chieftain.isStunned(echief))
            {
                int d2 = com.oddlabs.tt.player.fable.BasePlan.dist2(echief.getGridX(), echief.getGridY(), cx, cy);
                if (d2 <= 17 * 17 && (com.oddlabs.tt.player.fable.Chieftain.stunProgress(echief) >= @params.counter_toot_progress || com.oddlabs.tt.player.fable.Chieftain.isCasting(echief)))
                {
                    return cast(now, chief, true, "counter-toot vs enemy chieftain");
                }
            }
            int enemy_close8 = warriorsInDisc(intel, cx, cy, 8.0f);
            if (chief.getHitPoints() <= 20 && enemy_close8 >= 2)
            {
                if (blast_ready && own_units_near_18 <= @params.blast_max_own && enemy_close8 >= 4)
                {
                    return cast(now, chief, false, "last stand blast");
                }
                if (stun_ready)
                {
                    return cast(now, chief, true, "panic stun");
                }
            }
            if (siege_cast_requested && stun_ready)
            {
                siege_cast_requested = false;
                return cast(now, chief, true, "siege stun");
            }
            float v15 = valueInDisc(intel, cx, cy, @params.stun_cast_dist);
            int n15 = warriorsInDisc(intel, cx, cy, @params.stun_cast_dist);
            int n10 = warriorsInDisc(intel, cx, cy, 10.0f);
            if (blast_ready && !defending)
            {
                int peons = 0;
                foreach (com.oddlabs.tt.model.Unit p in java.lang.JIter.of(intel.peons))
                {
                    if (com.oddlabs.tt.player.fable.BasePlan.dist2(p.getGridX(), p.getGridY(), cx, cy) <= 15 * 15)
                    {
                        peons++;
                    }
                }
                if ((float)(peons + (float)(1.5f * n15)) >= @params.blast_min_peons && own_units_near_18 <= @params.blast_max_own)
                {
                    return cast(now, chief, false, "blast on " + (peons) + " peons/" + (n15) + " warriors");
                }
            }
            if (!stun_ready)
            {
                return false;
            }
            if (echief != null && !com.oddlabs.tt.player.fable.Chieftain.isFrozen(echief))
            {
                int ex = echief.getGridX();
                int ey = echief.getGridY();
                if (com.oddlabs.tt.player.fable.BasePlan.dist2(ex, ey, cx, cy) <= 13 * 13 && own_near >= 6 && com.oddlabs.tt.player.fable.Chieftain.ownWarriorsNear(own_warriors, ex, ey, 12) >= 8)
                {
                    return cast(now, chief, true, "stun on the enemy chieftain");
                }
            }
            if (defending)
            {
                if (v15 >= @params.base_stun_value || (n10 >= 3 && v15 >= 3.0f && own_near >= 2))
                {
                    return cast(now, chief, true, "base stun value=" + java.lang.JStr.valueOf(v15));
                }
            }
            else if (fighting && !hold_for_siege)
            {
                float v25 = valueInDisc(intel, cx, cy, 25.0f);
                if ((v15 >= 5.0f || n10 >= 3) && own_near >= 4 && v15 >= (float)(0.5f * v25))
                {
                    return cast(now, chief, true, "field stun value=" + java.lang.JStr.valueOf(v15) + " of " + java.lang.JStr.valueOf(v25) + " n10=" + (n10));
                }
            }
            return false;
        }
        // Java: 186
        public bool cast(float now, com.oddlabs.tt.model.Unit chief, bool stun, string why)
        {
            bool ok = (stun ? orders.stun(chief) : orders.blast(chief));
            if (ok)
            {
                if (stun)
                {
                    last_stun_at = now;
                    last_stun_cell = new int[] { chief.getGridX(), chief.getGridY() };
                }
                else
                {
                    last_blast_at = now;
                }
                log.info(new java.util.function.Supplier_L(() =>
                {
                    return "chieftain " + java.lang.JStr.valueOf(((stun ? "STUN" : "BLAST"))) + ": " + java.lang.JStr.valueOf(why);
                }));
            }
            return ok;
        }
        // Java: 201
        public void moveTo(float now, com.oddlabs.tt.model.Unit chief, int gx, int gy, bool urgent)
        {
            if (!orders.usable(chief) || com.oddlabs.tt.player.fable.Chieftain.isCasting(chief))
            {
                return;
            }
            int d2 = com.oddlabs.tt.player.fable.BasePlan.dist2(gx, gy, last_move_x, last_move_y);
            if (!urgent && (float)(now - last_move_order) < 3.0f && d2 <= 16)
            {
                return;
            }
            if (com.oddlabs.tt.player.fable.BasePlan.dist2(chief.getGridX(), chief.getGridY(), gx, gy) <= 4 && !chief.isMoving())
            {
                return;
            }
            orders.move(chief, gx, gy);
            last_move_order = now;
            last_move_x = gx;
            last_move_y = gy;
        }
        // Java: 216
        public bool shouldEscape(com.oddlabs.tt.model.Unit chief, com.oddlabs.tt.player.fable.Intel intel)
        {
            if (com.oddlabs.tt.player.fable.Chieftain.isCasting(chief))
            {
                return false;
            }
            int hp = chief.getHitPoints();
            int near = warriorsInDisc(intel, chief.getGridX(), chief.getGridY(), 10.0f);
            return (hp <= @params.chief_escape_hp && near >= 2) || (hp <= @params.chief_fragile_hp && near >= 4);
        }
        // Java: 225
        public static int ownUnitsNear(com.oddlabs.tt.player.fable.Roster roster, int cx, int cy, int radius)
        {
            int r2 = radius * radius;
            int n = 0;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(roster.warriors))
            {
                if (com.oddlabs.tt.player.fable.BasePlan.dist2(u.getGridX(), u.getGridY(), cx, cy) <= r2)
                {
                    n++;
                }
            }
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(roster.peons))
            {
                if (com.oddlabs.tt.player.fable.BasePlan.dist2(u_2.getGridX(), u_2.getGridY(), cx, cy) <= r2)
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 237
        public static int ownWarriorsNear(java.util.List warriors, int cx, int cy, int radius)
        {
            int r2 = radius * radius;
            int n = 0;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(warriors))
            {
                if (!u.isDead() && u.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.THROW) && com.oddlabs.tt.player.fable.BasePlan.dist2(u.getGridX(), u.getGridY(), cx, cy) <= r2)
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 248
        public static void stunnedEnemies(com.oddlabs.tt.player.fable.Intel intel, int cx, int cy, int radius, java.util.List @out)
        {
            @out.clear();
            int r2 = radius * radius;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(intel.chieftains))
            {
                if (com.oddlabs.tt.player.fable.Chieftain.isFrozen(u) && com.oddlabs.tt.player.fable.BasePlan.dist2(u.getGridX(), u.getGridY(), cx, cy) <= r2)
                {
                    @out.add(u);
                }
            }
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(intel.warriors))
            {
                if (com.oddlabs.tt.player.fable.Chieftain.isFrozen(u_2) && com.oddlabs.tt.player.fable.BasePlan.dist2(u_2.getGridX(), u_2.getGridY(), cx, cy) <= r2)
                {
                    @out.add(u_2);
                }
            }
            foreach (com.oddlabs.tt.model.LandBuilding t in java.lang.JIter.of(intel.manned_towers))
            {
                if (com.oddlabs.tt.player.fable.Chieftain.towerStunned(t) && com.oddlabs.tt.player.fable.BasePlan.dist2(t.getGridX(), t.getGridY(), cx, cy) <= r2)
                {
                    @out.add(t);
                }
            }
            @out.sort(new java.util.Comparator_L((object a_l, object b_l) =>
            {
                com.oddlabs.tt.model.Selectable a = (com.oddlabs.tt.model.Selectable)a_l;
                com.oddlabs.tt.model.Selectable b = (com.oddlabs.tt.model.Selectable)b_l;
                return java.lang.Integer.compare(com.oddlabs.tt.player.fable.BasePlan.dist2(a.getGridX(), a.getGridY(), cx, cy), com.oddlabs.tt.player.fable.BasePlan.dist2(b.getGridX(), b.getGridY(), cx, cy));
            }));
        }
        private void init_Chieftain()
        {
            this.last_stun_at = (float)(-100.0f);
            this.last_blast_at = (float)(-100.0f);
            this.last_move_order = (float)(-100.0f);
            this.last_move_x = (-1);
            this.last_move_y = (-1);
        }
        public Chieftain(com.oddlabs.tt.player.fable.Orders orders, com.oddlabs.tt.player.fable.Params @params, com.oddlabs.tt.player.fable.AiLog log) : base()
        {
            init_Chieftain();
            this.orders = orders;
            this.@params = @params;
            this.log = log;
        }
    }
}
