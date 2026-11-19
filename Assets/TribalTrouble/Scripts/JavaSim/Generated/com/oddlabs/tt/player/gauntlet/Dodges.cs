// Translated from Dodges.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.gauntlet
{
    // Java: 30
    public class Dodges : java.lang.Object
    {
        public com.oddlabs.tt.player.gauntlet.GauntletAI ai;
        public java.util.Map running;
        public float chief_move;
        // Java: 41
        public bool chiefBusy()
        {
            return (float)(ai.time() - chief_move) < 2.0f;
        }
        // Java: 46
        public void guard()
        {
            com.oddlabs.tt.player.gauntlet.Strategy strategy = ai.strategy();
            if (!strategy.peon_dodge && !strategy.chief_dodge)
            {
                return;
            }
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            if (!running.isEmpty())
            {
                running.entrySet().removeIf(new java.util.function.Predicate_L((object e_l) =>
                {
                    java.util.Map.Entry e = (java.util.Map.Entry)e_l;
                    return ((com.oddlabs.tt.model.Unit)e.getKey()).isDead() || (float)(ai.time() - ((float)((float?)e.getValue()))) > 12.0f;
                }));
                intel.dodging.retainAll(running.keySet());
            }
            java.util.Map locks = new java.util.LinkedHashMap();
            foreach (java.util.List group in java.lang.JIter.of(java.util.List_.of(intel.enemy_warriors, intel.enemy_chieftains)))
            {
                foreach (com.oddlabs.tt.model.Unit e_2 in java.lang.JIter.of(group))
                {
                    if (e_2.isDead())
                    {
                        continue;
                    }
                    com.oddlabs.tt.model.behaviour.Controller c = e_2.getCurrentController();
                    com.oddlabs.tt.model.Selectable t = ((c is com.oddlabs.tt.model.behaviour.HuntController h) ? h.getTarget() : ((c is com.oddlabs.tt.model.behaviour.AttackController a) ? a.getTarget() : (com.oddlabs.tt.model.Selectable)(null)));
                    if ((t is com.oddlabs.tt.model.Unit u) && ((object)u.getOwner() == (object)ai.owner()) && !u.isDead())
                    {
                        ((java.util.List)locks.computeIfAbsent(u, new java.util.function.Function_L((object k_l) =>
                        {
                            com.oddlabs.tt.model.Unit k = (com.oddlabs.tt.model.Unit)k_l;
                            return new java.util.ArrayList();
                        }))).add(e_2);
                    }
                }
            }
            if (locks.isEmpty())
            {
                return;
            }
            if (strategy.chief_dodge && intel.chieftain != null && !intel.chieftain.isDead())
            {
                dodgeChief(intel.chieftain, ((java.util.List)locks.get(intel.chieftain)));
            }
            if (strategy.peon_dodge)
            {
                foreach (java.util.Map.Entry e_3 in java.lang.JIter.of(locks.entrySet()))
                {
                    dodgePeon(((com.oddlabs.tt.model.Unit)e_3.getKey()), ((java.util.List)e_3.getValue()));
                }
            }
        }
        // Java: 76
        public void dodgePeon(com.oddlabs.tt.model.Unit p, java.util.List hunters)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            if (!p.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD) || p.isMounted() || running.containsKey(p) || intel.lures.contains(p) || intel.shepherds.contains(p) || com.oddlabs.tt.player.gauntlet.Intel.isStunned(p) || ai.economy().reservedPlacer(p))
            {
                return;
            }
            int px = p.getGridX();
            int py = p.getGridY();
            long hx = 0;
            long hy = 0;
            foreach (com.oddlabs.tt.model.Unit h in java.lang.JIter.of(hunters))
            {
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(px, py, h.getGridX(), h.getGridY()) <= 62)
                {
                    return;
                }
                hx += h.getGridX();
                hy += h.getGridY();
            }
            int cx = unchecked((int)((hx / hunters.size())));
            int cy = unchecked((int)((hy / hunters.size())));
            com.oddlabs.tt.model.Building best = null;
            int best_d = 60 * 60;
            java.util.List homes = intel.homes();
            foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(homes))
            {
                if (b.isDead() || !b.isComplete() || ai.economy().isEvacuating(b))
                {
                    continue;
                }
                int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(px, py, b.getGridX(), b.getGridY());
                if (d >= best_d || com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(cx, cy, b.getGridX(), b.getGridY()) <= d)
                {
                    continue;
                }
                best_d = d;
                best = b;
            }
            running.put(p, ai.time());
            intel.dodging.add(p);
            intel.peon_states.put(p, com.oddlabs.tt.player.gauntlet.Intel.PeonState.SHEPHERD);
            ai.aiLog().count("peon_dodge");
            if (best != null)
            {
                ai.owner().setTarget(com.oddlabs.tt.model.Selectable.newArray(p), best, com.oddlabs.tt.model.Action.MOVE, false);
                return;
            }
            int[] away = com.oddlabs.tt.player.gauntlet.Dodges.awayFrom(px, py, cx, cy, 14);
            ai.landscapeOrder(com.oddlabs.tt.model.Selectable.newArray(p), away[0], away[1], com.oddlabs.tt.model.Action.MOVE, false);
        }
        // Java: 120
        public void dodgeChief(com.oddlabs.tt.model.Unit chief, java.util.List hunters)
        {
            if (hunters == null || hunters.isEmpty() || com.oddlabs.tt.player.gauntlet.Intel.isStunned(chief) || (chief.getCurrentController() is com.oddlabs.tt.model.behaviour.MagicController) || (float)(ai.time() - chief_move) < 1.0f)
            {
                return;
            }
            int x = chief.getGridX();
            int y = chief.getGridY();
            long hx = 0;
            long hy = 0;
            int near = 0;
            foreach (com.oddlabs.tt.model.Unit h in java.lang.JIter.of(hunters))
            {
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, h.getGridX(), h.getGridY()) <= 12 * 12)
                {
                    hx += h.getGridX();
                    hy += h.getGridY();
                    near++;
                }
            }
            if (near == 0)
            {
                return;
            }
            int cx = unchecked((int)((hx / near)));
            int cy = unchecked((int)((hy / near)));
            int[] goal = null;
            int best = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(ai.intel().towers))
            {
                if (!com.oddlabs.tt.player.gauntlet.Intel.isTowerActive(t))
                {
                    continue;
                }
                int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, t.getGridX(), t.getGridY());
                if (d < best && d <= 50 * 50 && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(cx, cy, t.getGridX(), t.getGridY()) > d)
                {
                    best = d;
                    goal = new int[] { t.getGridX(), t.getGridY() };
                }
            }
            int[] step = (goal != null ? com.oddlabs.tt.player.gauntlet.Dodges.stepTowards(x, y, goal[0], goal[1], 8) : com.oddlabs.tt.player.gauntlet.Dodges.awayFrom(x, y, cx, cy, 8));
            chief_move = ai.time();
            ai.aiLog().count("chief_dodge");
            ai.landscapeOrder(com.oddlabs.tt.model.Selectable.newArray(chief), step[0], step[1], com.oddlabs.tt.model.Action.MOVE, false);
        }
        // Java: 157
        public static int[] awayFrom(int x, int y, int fx, int fy, int cells)
        {
            float dx = x - fx;
            float dy = y - fy;
            float len = java.lang.Math.max(1.0f, ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy))))));
            return new int[] { x + java.lang.Math.round((float)((float)(cells * dx) / len)), y + java.lang.Math.round((float)((float)(cells * dy) / len)) };
        }
        // Java: 164
        public static int[] stepTowards(int x, int y, int tx, int ty, int cells)
        {
            float dx = tx - x;
            float dy = ty - y;
            float len = ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy)))));
            if (len <= cells)
            {
                return new int[] { tx, ty };
            }
            return new int[] { x + java.lang.Math.round((float)((float)(cells * dx) / len)), y + java.lang.Math.round((float)((float)(cells * dy) / len)) };
        }
        private void init_Dodges()
        {
            this.running = new java.util.LinkedHashMap();
            this.chief_move = (float)(-10.0f);
        }
        public Dodges(com.oddlabs.tt.player.gauntlet.GauntletAI ai) : base()
        {
            init_Dodges();
            this.ai = ai;
        }
    }
}
