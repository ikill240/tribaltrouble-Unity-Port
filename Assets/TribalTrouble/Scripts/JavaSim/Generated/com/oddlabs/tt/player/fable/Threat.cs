// Translated from Threat.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.fable
{
    // Java: 15
    public class Threat : java.lang.Object
    {
        // Java: 16
        public sealed class Level : java.lang.Enum
        {
            public static Level[] values() { return (Level[])VALUES_.Clone(); }
            public static Level valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly Level NONE = new Level("NONE", 0);
            public static readonly Level WATCH = new Level("WATCH", 1);
            public static readonly Level ALERT = new Level("ALERT", 2);
            public static readonly Level ENGAGED = new Level("ENGAGED", 3);
            public static readonly Level[] VALUES_ = new Level[] { NONE, WATCH, ALERT, ENGAGED };
            public Level(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        public const int RING = 10;
        public float[] dist_ring;
        public int ring_index;
        public int ring_filled;
        public com.oddlabs.tt.player.fable.Threat.Level level;
        public float nearest_dist;
        public bool approaching;
        public float eta;
        public int[] approach_centroid;
        public float approach_value;
        public int approach_count;
        public float bearing_x;
        public float bearing_y;
        public com.oddlabs.tt.model.LandBuilding threatened;
        public int enemy_peons_near_buildings;
        public int enemy_units_near_buildings;
        public float since_level_change;
        public com.oddlabs.tt.player.fable.Threat.Level last_level;
        public int recent_enemy_losses_near_base;
        public java.util.List loss_times;
        public int last_enemy_warriors_near_base;
        // Java: 52
        public void update(float now, float dt, com.oddlabs.tt.player.fable.Roster roster, com.oddlabs.tt.player.fable.Intel intel, com.oddlabs.tt.player.fable.BasePlan plan, com.oddlabs.tt.player.fable.Params @params)
        {
            com.oddlabs.tt.model.LandBuilding anchor = roster.mainArmory();
            int ax = default;
            int ay = default;
            if (anchor != null)
            {
                ax = anchor.getGridX();
                ay = anchor.getGridY();
            }
            else if (!roster.quarters.isEmpty())
            {
                ax = ((com.oddlabs.tt.model.LandBuilding)roster.quarters.getFirst()).getGridX();
                ay = ((com.oddlabs.tt.model.LandBuilding)roster.quarters.getFirst()).getGridY();
            }
            else
            {
                ax = plan.start_x;
                ay = plan.start_y;
            }
            com.oddlabs.tt.model.Unit leader = null;
            float best = java.lang.Float.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(intel.warriors))
            {
                float d = com.oddlabs.tt.player.fable.BasePlan.dist(u.getGridX(), u.getGridY(), ax, ay);
                if (d < best)
                {
                    best = d;
                    leader = u;
                }
            }
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(intel.chieftains))
            {
                float d_2 = com.oddlabs.tt.player.fable.BasePlan.dist(u_2.getGridX(), u_2.getGridY(), ax, ay);
                if (d_2 < best)
                {
                    best = d_2;
                    leader = u_2;
                }
            }
            nearest_dist = best;
            dist_ring[ring_index] = best;
            ring_index = (ring_index + 1) % com.oddlabs.tt.player.fable.Threat.RING;
            if (ring_filled < com.oddlabs.tt.player.fable.Threat.RING)
            {
                ring_filled++;
            }
            float oldest = dist_ring[(ring_filled < com.oddlabs.tt.player.fable.Threat.RING ? 0 : ring_index)];
            approaching = leader != null && ring_filled >= com.oddlabs.tt.player.fable.Threat.RING && best <= (float)(oldest - 4.0f);
            eta = (leader == null ? java.lang.Float.MAX_VALUE : (float)(best / 2.0f));
            approach_centroid = null;
            approach_value = 0.0f;
            approach_count = 0;
            if (leader != null)
            {
                int[] c = intel.armyCentroid(leader.getGridX(), leader.getGridY(), 15);
                if (c != null)
                {
                    approach_centroid = c;
                    approach_value = intel.strengthNear(c[0], c[1], 15);
                    approach_count = intel.warriorsNear(c[0], c[1], 15);
                }
                float dx = leader.getGridX() - ax;
                float dy = leader.getGridY() - ay;
                float len = ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy)))));
                if (len > 0.5f)
                {
                    bearing_x = (float)(dx / len);
                    bearing_y = (float)(dy / len);
                }
            }
            else
            {
                bearing_x = plan.ux;
                bearing_y = plan.uy;
            }
            enemy_peons_near_buildings = 0;
            enemy_units_near_buildings = 0;
            threatened = null;
            float best_threat_d = java.lang.Float.MAX_VALUE;
            bool engaged = false;
            bool alert = false;
            bool watch = false;
            java.util.List buildings = new java.util.ArrayList(roster.quarters);
            buildings.addAll(roster.armories);
            buildings.addAll(roster.towers);
            buildings.addAll(roster.under_construction);
            foreach (com.oddlabs.tt.model.LandBuilding b in java.lang.JIter.of(buildings))
            {
                int w20 = intel.warriorsNear(b.getGridX(), b.getGridY(), 20);
                int w60 = intel.warriorsNear(b.getGridX(), b.getGridY(), 60);
                int peons30 = 0;
                foreach (com.oddlabs.tt.model.Unit p in java.lang.JIter.of(intel.peons))
                {
                    if (com.oddlabs.tt.player.fable.BasePlan.dist2(p.getGridX(), p.getGridY(), b.getGridX(), b.getGridY()) <= 30 * 30)
                    {
                        peons30++;
                    }
                }
                if (w20 > 0)
                {
                    engaged = true;
                }
                if (w60 >= 3 || peons30 >= 3)
                {
                    alert = true;
                }
                if (w60 > 0)
                {
                    watch = true;
                }
                enemy_peons_near_buildings = java.lang.Math.max(enemy_peons_near_buildings, peons30);
                enemy_units_near_buildings = java.lang.Math.max(enemy_units_near_buildings, w20);
                if (approach_centroid != null)
                {
                    float d_3 = com.oddlabs.tt.player.fable.BasePlan.dist(b.getGridX(), b.getGridY(), approach_centroid[0], approach_centroid[1]);
                    if (d_3 < best_threat_d)
                    {
                        best_threat_d = d_3;
                        threatened = b;
                    }
                }
                foreach (com.oddlabs.tt.model.Unit ch in java.lang.JIter.of(intel.chieftains))
                {
                    if (com.oddlabs.tt.player.fable.BasePlan.dist2(ch.getGridX(), ch.getGridY(), b.getGridX(), b.getGridY()) <= 40 * 40)
                    {
                        alert = true;
                    }
                }
                foreach (com.oddlabs.tt.model.LandBuilding site in java.lang.JIter.of(intel.construction_sites))
                {
                    if (com.oddlabs.tt.player.fable.BasePlan.dist2(site.getGridX(), site.getGridY(), b.getGridX(), b.getGridY()) <= 40 * 40)
                    {
                        alert = true;
                    }
                }
            }
            if (approaching && eta < @params.alert_eta)
            {
                alert = true;
            }
            if (approaching && eta < 90.0f)
            {
                watch = true;
            }
            com.oddlabs.tt.player.fable.Threat.Level new_level = (engaged ? com.oddlabs.tt.player.fable.Threat.Level.ENGAGED : (alert ? com.oddlabs.tt.player.fable.Threat.Level.ALERT : (watch ? com.oddlabs.tt.player.fable.Threat.Level.WATCH : com.oddlabs.tt.player.fable.Threat.Level.NONE)));
            if (((object)new_level != (object)last_level))
            {
                since_level_change = 0.0f;
                last_level = new_level;
            }
            else
            {
                since_level_change += dt;
            }
            level = new_level;
            int near_now = (anchor == null ? 0 : intel.warriorsNear(ax, ay, 60));
            if (near_now < last_enemy_warriors_near_base && !approaching)
            {
                int lost = last_enemy_warriors_near_base - near_now;
                for (int i = 0; i < lost; i++)
                {
                    loss_times.add(now);
                }
            }
            last_enemy_warriors_near_base = near_now;
            loss_times.removeIf(new java.util.function.Predicate_L((object t_l) =>
            {
                float? t = (float?)t_l;
                return (float)(now - ((float)t)) > 60.0f;
            }));
            recent_enemy_losses_near_base = loss_times.size();
        }
        // Java: 178
        public void hotCells(com.oddlabs.tt.player.fable.Intel intel, java.util.List @out)
        {
            @out.clear();
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(intel.warriors))
            {
                @out.add(new int[] { u.getGridX(), u.getGridY() });
            }
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(intel.chieftains))
            {
                @out.add(new int[] { u_2.getGridX(), u_2.getGridY() });
            }
            foreach (com.oddlabs.tt.model.LandBuilding t in java.lang.JIter.of(intel.manned_towers))
            {
                @out.add(new int[] { t.getGridX(), t.getGridY() });
            }
        }
        // Java: 188
        public override string toString()
        {
            return java.lang.JStr.valueOf(level) + java.lang.JStr.valueOf(((approaching ? java.lang.JString.format(" approaching eta=%.0fs n=%d val=%.1f", eta, approach_count, approach_value) : java.lang.JString.format(" nearest=%.0f", (nearest_dist == java.lang.Float.MAX_VALUE ? (float)(-1.0f) : nearest_dist)))));
        }
        private void init_Threat()
        {
            this.dist_ring = new float[com.oddlabs.tt.player.fable.Threat.RING];
            this.level = com.oddlabs.tt.player.fable.Threat.Level.NONE;
            this.nearest_dist = java.lang.Float.MAX_VALUE;
            this.eta = java.lang.Float.MAX_VALUE;
            this.last_level = com.oddlabs.tt.player.fable.Threat.Level.NONE;
            this.loss_times = new java.util.ArrayList();
        }
        public Threat() : base()
        {
            init_Threat();
        }
    }
}
