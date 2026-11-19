// Translated from Chieftain.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.ai
{
    // Java: 15
    public class Chieftain : java.lang.Object
    {
        public const int STUN_CELLS = 16;
        public const int STUN_REACH = 17;
        public const float MOVE_PERIOD = 1.5f;
        public const int TOWER_KEEP = 17;
        public const int TOWER_CAUGHT2 = 300;
        public com.oddlabs.tt.player.ai.ExpertAI ai;
        public float last_move;
        public float last_cast;
        public java.util.List cast_candidates;
        public float cast_check;
        public int cast_best;
        public float catch_rate;
        // Java: 43
        public int magicIndex()
        {
            if (((object)ai.owner().getRace() == (object)ai.owner().getWorld().getRacesResources().getRace(com.oddlabs.tt.model.RacesResources.RACE_VIKINGS)))
            {
                return com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_STUN;
            }
            return (ai.strategy().native_lightning ? com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_LIGHTNING : com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_POISON);
        }
        // Java: 50
        public float sinceCast()
        {
            return (float)(ai.time() - last_cast);
        }
        // Java: 54
        public bool isViking()
        {
            return ((object)ai.owner().getRace() == (object)ai.owner().getWorld().getRacesResources().getRace(com.oddlabs.tt.model.RacesResources.RACE_VIKINGS));
        }
        // Java: 62
        public bool shouldBlast(com.oddlabs.tt.model.Unit chief)
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            int x = chief.getGridX();
            int y = chief.getGridY();
            float theirs = 0.0f;
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(intel.enemy_warriors))
            {
                if (com.oddlabs.tt.player.ai.MapAnalysis.dist2(x, y, e.getGridX(), e.getGridY()) <= 16 * 16)
                {
                    theirs = (float)(theirs + com.oddlabs.tt.player.ai.Combat.@value(e));
                }
            }
            foreach (com.oddlabs.tt.model.Unit e_2 in java.lang.JIter.of(intel.enemy_peons))
            {
                if (com.oddlabs.tt.player.ai.MapAnalysis.dist2(x, y, e_2.getGridX(), e_2.getGridY()) <= 16 * 16)
                {
                    theirs = (float)(theirs + 0.3f);
                }
            }
            float ours = 0.0f;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(intel.warriors))
            {
                if (!u.isMounted() && com.oddlabs.tt.player.ai.MapAnalysis.dist2(x, y, u.getGridX(), u.getGridY()) <= 20 * 20)
                {
                    ours = (float)(ours + com.oddlabs.tt.player.ai.Combat.@value(u));
                }
            }
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(intel.peons))
            {
                if (com.oddlabs.tt.player.ai.MapAnalysis.dist2(x, y, u_2.getGridX(), u_2.getGridY()) <= 20 * 20)
                {
                    ours = (float)(ours + 0.3f);
                }
            }
            bool go = theirs >= ai.strategy().blast_min && theirs >= (float)(ai.strategy().blast_ratio * ours);
            if (go)
            {
                ai.log(java.lang.JString.format("chieftain blasts at %d,%d: %.1f of theirs against %.1f of ours", x, y, theirs, ours));
            }
            return go;
        }
        // Java: 88
        public bool blastReady()
        {
            com.oddlabs.tt.model.Unit chief = ai.intel().chieftain;
            return chief != null && !chief.isDead() && isViking() && chief.getHitPoints() > 40 && chief.canDoMagic(com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_BLAST) && !com.oddlabs.tt.player.ai.Intel.isStunned(chief);
        }
        // Java: 94
        public bool stunReady()
        {
            com.oddlabs.tt.model.Unit chief = ai.intel().chieftain;
            return chief != null && !chief.isDead() && chief.canDoMagic(magicIndex());
        }
        // Java: 99
        public com.oddlabs.tt.model.Building trainingQuarters()
        {
            foreach (com.oddlabs.tt.model.Building q in java.lang.JIter.of(ai.intel().quarters))
            {
                if (q.getChieftainContainer() != null && q.getChieftainContainer().isTraining())
                {
                    return q;
                }
            }
            return null;
        }
        // Java: 108
        public void tick()
        {
            checkCatch();
            com.oddlabs.tt.model.Unit chief = ai.intel().chieftain;
            if (chief == null)
            {
                considerTraining();
                return;
            }
            if (com.oddlabs.tt.player.ai.Intel.isStunned(chief))
            {
                return;
            }
            if (ai.strategy().blast && isViking() && chief.canDoMagic(com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_BLAST) && shouldBlast(chief))
            {
                ai.owner().doMagic(chief, com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_BLAST);
                last_cast = ai.time();
                return;
            }
            if (stunReady() && shouldStun(chief))
            {
                int x = chief.getGridX();
                int y = chief.getGridY();
                string rivals = "";
                foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(ai.intel().enemy_chieftains))
                {
                    if (!e.isDead())
                    {
                        rivals += java.lang.JStr.valueOf(java.lang.JString.format(" [enemy chief %d cells%s%s]", java.lang.JC.d2i(java.lang.Math.sqrt(com.oddlabs.tt.player.ai.MapAnalysis.dist2(x, y, e.getGridX(), e.getGridY()))), ((e.getCurrentController() is com.oddlabs.tt.model.behaviour.MagicController) ? (object)(" casting") : (object)("")), (ai.military().enemySpellReady(e) ? (object)(" ready") : (object)(""))));
                    }
                }
                ai.log("chieftain stuns at " + (x) + "," + (y) + ": " + (com.oddlabs.tt.player.ai.Combat.countNear(ai.intel().enemy_warriors, x, y, 17)) + " enemy warriors in reach" + java.lang.JStr.valueOf(rivals));
                ai.owner().doMagic(chief, magicIndex());
                last_cast = ai.time();
                if (isViking())
                {
                    cast_candidates.clear();
                    foreach (com.oddlabs.tt.model.Unit e_2 in java.lang.JIter.of(ai.intel().enemy_warriors))
                    {
                        if (!com.oddlabs.tt.player.ai.Intel.isStunned(e_2) && com.oddlabs.tt.player.ai.MapAnalysis.dist2(x, y, e_2.getGridX(), e_2.getGridY()) <= 17 * 17)
                        {
                            cast_candidates.add(e_2);
                        }
                    }
                    cast_check = (float)(ai.time() + 8.0f);
                    cast_best = 0;
                }
                return;
            }
            position(chief);
        }
        // Java: 151
        public void checkCatch()
        {
            if (cast_check < 0.0f)
            {
                return;
            }
            int alive = 0;
            int caught = 0;
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(cast_candidates))
            {
                if (e.isDead())
                {
                    continue;
                }
                alive++;
                if (com.oddlabs.tt.player.ai.Intel.isStunned(e))
                {
                    caught++;
                }
            }
            cast_best = java.lang.Math.max(cast_best, caught);
            if (ai.time() < cast_check)
            {
                return;
            }
            cast_check = (float)(-1.0f);
            caught = java.lang.Math.min(alive, cast_best);
            cast_candidates.clear();
            com.oddlabs.tt.model.Unit chief = ai.intel().chieftain;
            string state = (chief == null || chief.isDead() ? " (our chief died)" : (com.oddlabs.tt.player.ai.Intel.isStunned(chief) ? " (our chief stunned)" : ""));
            if (alive < 4)
            {
                if (!state.isEmpty())
                {
                    ai.log("our stun: " + java.lang.JStr.valueOf(state.trim()));
                }
                return;
            }
            float share = (float)(caught / ((float)(alive)));
            catch_rate = (float)((float)(0.6f * catch_rate) + (float)(0.4f * share));
            ai.log(java.lang.JString.format("our stun caught %d of %d in reach (running share %.2f)%s", caught, alive, catch_rate, state));
        }
        // Java: 184
        public bool enemyDodges()
        {
            return ai.strategy().stun_learn && catch_rate < ai.strategy().dodge_catch;
        }
        // Java: 188
        public void considerTraining()
        {
            com.oddlabs.tt.player.ai.Strategy strategy = ai.strategy();
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            if (ai.owner().isTrainingChieftain() || !ai.owner().canBuildChieftains())
            {
                return;
            }
            if (intel.quarters.size() < strategy.chieftain_min_quarters || ai.time() < strategy.chieftain_time)
            {
                return;
            }
            if (intel.armory() == null)
            {
                return;
            }
            com.oddlabs.tt.model.Building best = null;
            float best_score = (float)(-java.lang.Float.MAX_VALUE);
            foreach (com.oddlabs.tt.model.Building q in java.lang.JIter.of(intel.quarters))
            {
                if (!q.canBuildChieftain())
                {
                    continue;
                }
                float score = (float)(q.getUnitContainer().getNumSupplies() - (float)(20.0f * ai.planner().exposure(q.getGridX(), q.getGridY())));
                if (score > best_score)
                {
                    best_score = score;
                    best = q;
                }
            }
            if (best != null)
            {
                ai.log("training chieftain in quarters at " + (best.getGridX()) + "," + (best.getGridY()));
                ai.owner().trainChieftain(best, true);
            }
        }
        // Java: 215
        public bool shouldStun(com.oddlabs.tt.model.Unit chief)
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            int x = chief.getGridX();
            int y = chief.getGridY();
            int r2 = com.oddlabs.tt.player.ai.Chieftain.STUN_CELLS * com.oddlabs.tt.player.ai.Chieftain.STUN_CELLS;
            float warriors = 0.0f;
            bool dodges = enemyDodges();
            int core2 = ai.strategy().dodge_core * ai.strategy().dodge_core;
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(intel.enemy_warriors))
            {
                if (com.oddlabs.tt.player.ai.Intel.isStunned(e))
                {
                    continue;
                }
                int d2 = com.oddlabs.tt.player.ai.MapAnalysis.dist2(x, y, e.getGridX(), e.getGridY());
                if (d2 <= r2)
                {
                    warriors = (float)(warriors + (!dodges || d2 <= core2 ? 1.0f : 0.25f));
                }
            }
            int towers = 0;
            int tower_r2 = (ai.strategy().chief_tower_standoff ? com.oddlabs.tt.player.ai.Chieftain.TOWER_CAUGHT2 : r2);
            int[] army = ai.military().attackCenter();
            bool follow_up = !ai.strategy().tower_stun_follow_up || (army != null && com.oddlabs.tt.player.ai.MapAnalysis.dist2(x, y, army[0], army[1]) <= 25 * 25);
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(intel.enemy_towers))
            {
                if (follow_up && com.oddlabs.tt.player.ai.Intel.isTowerActive(t) && com.oddlabs.tt.player.ai.MapAnalysis.dist2(x, y, t.getGridX(), t.getGridY()) <= tower_r2)
                {
                    towers++;
                }
            }
            int chiefs = 0;
            foreach (com.oddlabs.tt.model.Unit e_2 in java.lang.JIter.of(intel.enemy_chieftains))
            {
                if (com.oddlabs.tt.player.ai.Intel.isStunned(e_2) || com.oddlabs.tt.player.ai.MapAnalysis.dist2(x, y, e_2.getGridX(), e_2.getGridY()) > com.oddlabs.tt.player.ai.Chieftain.STUN_REACH * com.oddlabs.tt.player.ai.Chieftain.STUN_REACH)
                {
                    continue;
                }
                chiefs++;
                if (ai.military().enemySpellReady(e_2))
                {
                    return true;
                }
            }
            if (towers > 0 && ai.military().sieging())
            {
                return true;
            }
            float caught = (float)((float)(warriors + (float)(3.0f * towers)) + (float)(3.0f * chiefs));
            if (chief.getHitPoints() <= 20 && caught >= 1.0f)
            {
                return true;
            }
            if (ai.strategy().blast && ai.strategy().blast_save && chief.getMagicProgress(com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_BLAST) >= 0.5f)
            {
                return false;
            }
            int ours = com.oddlabs.tt.player.ai.Combat.countNear(intel.warriors, x, y, 20);
            float needed = (ours >= 6 ? 4.0f : 6.0f);
            if (caught < needed)
            {
                return false;
            }
            if (!ai.strategy().stun_patience)
            {
                return true;
            }
            float coming = (float)((float)(warriors + com.oddlabs.tt.player.ai.Combat.countNear(intel.enemy_warriors, x, y, 32)) - com.oddlabs.tt.player.ai.Combat.countNear(intel.enemy_warriors, x, y, com.oddlabs.tt.player.ai.Chieftain.STUN_CELLS));
            float share = (float)(caught / java.lang.Math.max(1.0f, (float)((float)(coming + (float)(3.0f * towers)) + (float)(3.0f * chiefs))));
            bool answer = false;
            foreach (com.oddlabs.tt.model.Unit e_3 in java.lang.JIter.of(intel.enemy_chieftains))
            {
                answer |= !com.oddlabs.tt.player.ai.Intel.isStunned(e_3) && com.oddlabs.tt.player.ai.MapAnalysis.dist2(x, y, e_3.getGridX(), e_3.getGridY()) <= 45 * 45 && ai.military().enemySpellReady(e_3);
            }
            if (answer)
            {
                return caught >= 20.0f || (share >= 0.8f && caught >= 10.0f);
            }
            return caught >= 20.0f || share >= 0.55f;
        }
        // Java: 282
        public void position(com.oddlabs.tt.model.Unit chief)
        {
            int safe = ai.strategy().chief_safe;
            bool threatened = safe > 0 && !stunReady() && nearestWarriorDistance(chief.getGridX(), chief.getGridY()) <= safe;
            if (((float)(ai.time() - last_move) < com.oddlabs.tt.player.ai.Chieftain.MOVE_PERIOD && !threatened) || ai.military().isDodging(chief))
            {
                return;
            }
            com.oddlabs.tt.player.ai.Military military = ai.military();
            int[] army = military.attackCenter();
            int[] enemies = nearestEnemies(chief.getGridX(), chief.getGridY(), 30);
            int tx = default;
            int ty = default;
            if (chief.getHitPoints() <= ai.strategy().chief_flee_hp)
            {
                com.oddlabs.tt.model.Building armory = ai.intel().armory();
                tx = (armory != null ? armory.getGridX() : military.stagingX());
                ty = (armory != null ? armory.getGridY() : military.stagingY());
            }
            else if (ai.military().blastPlay() != null)
            {
                int[] at = ai.military().blastPlay();
                int nearest = nearestWarriorDistance(chief.getGridX(), chief.getGridY());
                if (nearest <= 9)
                {
                    tx = chief.getGridX();
                    ty = chief.getGridY();
                }
                else
                {
                    int[] stop = com.oddlabs.tt.player.ai.Chieftain.towards(chief.getGridX(), chief.getGridY(), at[0], at[1], java.lang.Math.max(3, nearest - 9));
                    tx = stop[0];
                    ty = stop[1];
                }
            }
            else if (enemies != null && stunReady())
            {
                tx = enemies[0];
                ty = enemies[1];
                int keep = ai.strategy().chief_keep_out;
                if (keep > 0)
                {
                    int nearest_2 = nearestWarriorDistance(chief.getGridX(), chief.getGridY());
                    if (nearest_2 <= keep + 1)
                    {
                        tx = chief.getGridX();
                        ty = chief.getGridY();
                    }
                    else
                    {
                        int[] stop_2 = com.oddlabs.tt.player.ai.Chieftain.towards(chief.getGridX(), chief.getGridY(), enemies[0], enemies[1], nearest_2 - keep);
                        tx = stop_2[0];
                        ty = stop_2[1];
                    }
                }
            }
            else if (army != null)
            {
                int[] back = com.oddlabs.tt.player.ai.Chieftain.towards(army[0], army[1], military.stagingX(), military.stagingY(), (enemies != null ? 8 : 3));
                tx = back[0];
                ty = back[1];
            }
            else if (military.baseThreatLevel() > 0)
            {
                int[] back_2 = com.oddlabs.tt.player.ai.Chieftain.towards(military.threatX(), military.threatY(), military.stagingX(), military.stagingY(), 12);
                tx = back_2[0];
                ty = back_2[1];
            }
            else
            {
                tx = military.stagingX();
                ty = military.stagingY();
            }
            if (safe > 0 && !stunReady())
            {
                int[] away = awayFromWarriors(tx, ty, safe + 1);
                tx = away[0];
                ty = away[1];
            }
            if (ai.strategy().chief_tower_standoff && chief.getHitPoints() > 24)
            {
                int[] clear = clearOfTowers(tx, ty, chief.getGridX(), chief.getGridY());
                tx = clear[0];
                ty = clear[1];
            }
            if (com.oddlabs.tt.player.ai.MapAnalysis.dist2(chief.getGridX(), chief.getGridY(), tx, ty) <= 3 * 3)
            {
                return;
            }
            last_move = ai.time();
            ai.owner().setLandscapeTarget(com.oddlabs.tt.model.Selectable.newArray(chief), tx, ty, com.oddlabs.tt.model.Action.MOVE, false);
        }
        // Java: 360
        public int[] clearOfTowers(int x, int y, int from_x, int from_y)
        {
            for (int pass = 0; pass < 3; pass++)
            {
                com.oddlabs.tt.model.Building near = null;
                int best = com.oddlabs.tt.player.ai.Chieftain.TOWER_KEEP * com.oddlabs.tt.player.ai.Chieftain.TOWER_KEEP;
                foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(ai.intel().enemy_towers))
                {
                    int d = com.oddlabs.tt.player.ai.MapAnalysis.dist2(x, y, t.getGridX(), t.getGridY());
                    if (com.oddlabs.tt.player.ai.Intel.isTowerActive(t) && d < best)
                    {
                        best = d;
                        near = t;
                    }
                }
                if (near == null)
                {
                    break;
                }
                float dx = x - near.getGridX();
                float dy = y - near.getGridY();
                if ((float)((float)(dx * dx) + (float)(dy * dy)) < 1.0f)
                {
                    dx = from_x - near.getGridX();
                    dy = from_y - near.getGridY();
                }
                float len = java.lang.Math.max(0.1f, ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy))))));
                x = near.getGridX() + java.lang.Math.round((float)((float)(dx / len) * com.oddlabs.tt.player.ai.Chieftain.TOWER_KEEP));
                y = near.getGridY() + java.lang.Math.round((float)((float)(dy / len) * com.oddlabs.tt.player.ai.Chieftain.TOWER_KEEP));
            }
            int size = ai.map().getSize();
            return new int[] { java.lang.Math.clamp(x, 0, size - 1), java.lang.Math.clamp(y, 0, size - 1) };
        }
        // Java: 388
        public int[] awayFromWarriors(int x, int y, int keep)
        {
            for (int pass = 0; pass < 3; pass++)
            {
                com.oddlabs.tt.model.Unit near = null;
                int best = keep * keep;
                foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(ai.intel().enemy_warriors))
                {
                    if (com.oddlabs.tt.player.ai.Intel.isStunned(e))
                    {
                        continue;
                    }
                    int d = com.oddlabs.tt.player.ai.MapAnalysis.dist2(x, y, e.getGridX(), e.getGridY());
                    if (d < best)
                    {
                        best = d;
                        near = e;
                    }
                }
                if (near == null)
                {
                    break;
                }
                float dx = x - near.getGridX();
                float dy = y - near.getGridY();
                if ((float)((float)(dx * dx) + (float)(dy * dy)) < 1.0f)
                {
                    dx = ai.military().stagingX() - near.getGridX();
                    dy = ai.military().stagingY() - near.getGridY();
                }
                float len = java.lang.Math.max(0.1f, ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy))))));
                x = near.getGridX() + java.lang.Math.round((float)((float)(dx / len) * keep));
                y = near.getGridY() + java.lang.Math.round((float)((float)(dy / len) * keep));
            }
            int size = ai.map().getSize();
            return new int[] { java.lang.Math.clamp(x, 0, size - 1), java.lang.Math.clamp(y, 0, size - 1) };
        }
        // Java: 418
        public int nearestWarriorDistance(int x, int y)
        {
            int best = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(ai.intel().enemy_warriors))
            {
                if (!com.oddlabs.tt.player.ai.Intel.isStunned(e))
                {
                    best = java.lang.Math.min(best, com.oddlabs.tt.player.ai.MapAnalysis.dist2(x, y, e.getGridX(), e.getGridY()));
                }
            }
            return (best == java.lang.Integer.MAX_VALUE ? 1000 : java.lang.JC.d2i(java.lang.Math.sqrt(best)));
        }
        // Java: 427
        public static int[] towards(int x, int y, int to_x, int to_y, int cells)
        {
            float dx = to_x - x;
            float dy = to_y - y;
            float len = ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy)))));
            if (len <= cells)
            {
                return new int[] { to_x, to_y };
            }
            return new int[] { x + java.lang.JC.f2i(((float)((float)(dx / len) * cells))), y + java.lang.JC.f2i(((float)((float)(dy / len) * cells))) };
        }
        // Java: 437
        public int[] nearestEnemies(int x, int y, int radius)
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            com.oddlabs.tt.model.Selectable nearest = null;
            int best = radius * radius;
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(intel.enemy_warriors))
            {
                if (com.oddlabs.tt.player.ai.Intel.isStunned(e))
                {
                    continue;
                }
                int d = com.oddlabs.tt.player.ai.MapAnalysis.dist2(x, y, e.getGridX(), e.getGridY());
                if (d < best)
                {
                    best = d;
                    nearest = e;
                }
            }
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(intel.enemy_towers))
            {
                if (!com.oddlabs.tt.player.ai.Intel.isTowerActive(t))
                {
                    continue;
                }
                int d_2 = com.oddlabs.tt.player.ai.MapAnalysis.dist2(x, y, t.getGridX(), t.getGridY());
                if (d_2 < best)
                {
                    best = d_2;
                    nearest = t;
                }
            }
            if (nearest == null)
            {
                return null;
            }
            long sx = 0;
            long sy = 0;
            int n = 0;
            foreach (com.oddlabs.tt.model.Unit e_2 in java.lang.JIter.of(intel.enemy_warriors))
            {
                if (com.oddlabs.tt.player.ai.MapAnalysis.dist2(nearest.getGridX(), nearest.getGridY(), e_2.getGridX(), e_2.getGridY()) <= 12 * 12)
                {
                    sx += e_2.getGridX();
                    sy += e_2.getGridY();
                    n++;
                }
            }
            if (n == 0)
            {
                return new int[] { nearest.getGridX(), nearest.getGridY() };
            }
            return new int[] { unchecked((int)((sx / n))), unchecked((int)((sy / n))) };
        }
        private void init_Chieftain()
        {
            this.last_move = (float)(-100.0f);
            this.last_cast = (float)(-100.0f);
            this.cast_candidates = new java.util.ArrayList();
            this.cast_check = (float)(-1.0f);
            this.catch_rate = 1.0f;
        }
        public Chieftain(com.oddlabs.tt.player.ai.ExpertAI ai) : base()
        {
            init_Chieftain();
            this.ai = ai;
        }
    }
}
