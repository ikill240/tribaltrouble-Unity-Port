// Translated from Jams.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.gauntlet
{
    // Java: 22
    public class Jams : java.lang.Object
    {
        public const float PERIOD = 5.0f;
        public const int CLUSTER = 4;
        public const int RADIUS = 4;
        public com.oddlabs.tt.player.gauntlet.GauntletAI ai;
        public java.util.Map last_cells;
        public float last_scan;
        public float last_log;
        public float last_pic;
        // Java: 37
        public void tick()
        {
            if ((float)(ai.time() - last_scan) < com.oddlabs.tt.player.gauntlet.Jams.PERIOD)
            {
                return;
            }
            last_scan = ai.time();
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            java.util.List blocked_peons = new java.util.ArrayList();
            java.util.List blocked_warriors = new java.util.ArrayList();
            java.util.Map cells = new java.util.LinkedHashMap();
            foreach (java.util.List group in java.lang.JIter.of(java.util.List_.of(intel.peons, intel.warriors)))
            {
                foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(group))
                {
                    if (u.isDead() || u.isMounted())
                    {
                        continue;
                    }
                    int[] cell = new int[] { u.getGridX(), u.getGridY() };
                    cells.put(u, cell);
                    int[] before = ((int[])last_cells.get(u));
                    if (before == null || before[0] != cell[0] || before[1] != cell[1])
                    {
                        continue;
                    }
                    if (!((u.getCurrentBehaviour() is com.oddlabs.tt.model.behaviour.WalkBehaviour)))
                    {
                        continue;
                    }
                    ((((object)group == (object)intel.peons) ? blocked_peons : blocked_warriors)).add(u);
                }
            }
            last_cells.clear();
            last_cells.putAll(cells);
            ai.military().noteBlocked(blocked_warriors);
            count(blocked_peons, "peon_blocked", "peon_jam", "peons");
            count(blocked_warriors, "warrior_blocked", "warrior_jam", "warriors");
        }
        // Java: 68
        public static string describePeons(java.util.List jam)
        {
            java.lang.StringBuilder sb = new java.lang.StringBuilder();
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(jam))
            {
                if (!sb.isEmpty())
                {
                    sb.append(", ");
                }
                sb.append(u.getPrimaryController().GetType().getSimpleName().replace("Controller", ""));
                if ((u.getCurrentController() is com.oddlabs.tt.model.behaviour.WalkController w))
                {
                    sb.append(" to ").append(w.getTarget().getGridX()).append(',').append(w.getTarget().getGridY());
                }
                else if ((u.getPrimaryController() is com.oddlabs.tt.model.behaviour.RepairController r))
                {
                    com.oddlabs.tt.model.Building b = r.getBuilding();
                    sb.append(" site ").append(com.oddlabs.tt.player.gauntlet.Intel.kind(b)).append(" at ").append(b.getGridX()).append(',').append(b.getGridY()).append((b.isComplete() ? " (complete)" : ""));
                }
            }
            return java.lang.JO.toString(sb);
        }
        // Java: 86
        public void count(java.util.List blocked, string blocked_key, string jam_key, string what)
        {
            for (int i = 0; i < blocked.size(); i++)
            {
                ai.aiLog().count(blocked_key);
            }
            java.util.List left = new java.util.ArrayList(blocked);
            while (!left.isEmpty())
            {
                com.oddlabs.tt.model.Unit seed = ((com.oddlabs.tt.model.Unit)left.removeFirst());
                java.util.List jam = new java.util.ArrayList();
                jam.add(seed);
                foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(left))
                {
                    if (java.lang.Math.max(java.lang.Math.abs(u.getGridX() - seed.getGridX()), java.lang.Math.abs(u.getGridY() - seed.getGridY())) <= com.oddlabs.tt.player.gauntlet.Jams.RADIUS)
                    {
                        jam.add(u);
                    }
                }
                if (jam.size() < com.oddlabs.tt.player.gauntlet.Jams.CLUSTER)
                {
                    continue;
                }
                left.removeAll(jam);
                ai.aiLog().count(jam_key);
                if ((float)(ai.time() - last_log) >= 20.0f)
                {
                    last_log = ai.time();
                    int size = jam.size();
                    int x = seed.getGridX();
                    int y = seed.getGridY();
                    ai.log(java.lang.JString.format("jam: %d %s blocked around %d,%d", size, what, x, y));
                    if (ai.logging() && java.lang.JO.equals(what, "peons"))
                    {
                        ai.log("jam peons: " + java.lang.JStr.valueOf(com.oddlabs.tt.player.gauntlet.Jams.describePeons(jam)));
                    }
                }
                if (ai.logging() && jam.size() >= ((java.lang.JO.equals(what, "warriors") ? 12 : 6)) && (float)(ai.time() - last_pic) >= 150.0f)
                {
                    last_pic = ai.time();
                    try
                    {
                        ai.military().describeJam(seed.getGridX(), seed.getGridY());
                    }
                    catch (System.Exception e)
                    {
                        ai.aiLog().error("Jams.describeJam", e);
                    }
                }
            }
        }
        private void init_Jams()
        {
            this.last_cells = new java.util.LinkedHashMap();
            this.last_scan = (float)(-10.0f);
            this.last_log = (float)(-100.0f);
            this.last_pic = (float)(-1000.0f);
        }
        public Jams(com.oddlabs.tt.player.gauntlet.GauntletAI ai) : base()
        {
            init_Jams();
            this.ai = ai;
        }
    }
}
