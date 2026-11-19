// Translated from Chieftain.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.gauntlet
{
    // Java: 15
    public class Chieftain : java.lang.Object
    {
        public const int STUN_CELLS = 16;
        public const int STUN_REACH = 17;
        public const float MOVE_PERIOD = 1.5f;
        public const int TOWER_KEEP = 17;
        public const int TOWER_CAUGHT2 = 300;
        public com.oddlabs.tt.player.gauntlet.GauntletAI ai;
        public float last_move;
        public float last_cast;
        public bool had_chief;
        public java.util.List cast_candidates;
        public float cast_check;
        public int cast_best;
        public float catch_rate;
        // Java: 44
        public int magicIndex()
        {
            if (((object)ai.owner().getRace() == (object)ai.owner().getWorld().getRacesResources().getRace(com.oddlabs.tt.model.RacesResources.RACE_VIKINGS)))
            {
                return com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_STUN;
            }
            return (ai.strategy().native_lightning ? com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_LIGHTNING : com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_POISON);
        }
        // Java: 51
        public float sinceCast()
        {
            return (float)(ai.time() - last_cast);
        }
        // Java: 55
        public bool isViking()
        {
            return ((object)ai.owner().getRace() == (object)ai.owner().getWorld().getRacesResources().getRace(com.oddlabs.tt.model.RacesResources.RACE_VIKINGS));
        }
        // Java: 63
        public bool shouldBlast(com.oddlabs.tt.model.Unit chief)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            int x = chief.getGridX();
            int y = chief.getGridY();
            float theirs = 0.0f;
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(intel.enemy_warriors))
            {
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, e.getGridX(), e.getGridY()) <= 16 * 16)
                {
                    theirs = (float)(theirs + com.oddlabs.tt.player.gauntlet.Combat.@value(e));
                }
            }
            foreach (com.oddlabs.tt.model.Unit e_2 in java.lang.JIter.of(intel.enemy_peons))
            {
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, e_2.getGridX(), e_2.getGridY()) <= 16 * 16)
                {
                    theirs = (float)(theirs + 0.3f);
                }
            }
            float ours = 0.0f;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(intel.warriors))
            {
                if (!u.isMounted() && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, u.getGridX(), u.getGridY()) <= 20 * 20)
                {
                    ours = (float)(ours + com.oddlabs.tt.player.gauntlet.Combat.@value(u));
                }
            }
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(intel.peons))
            {
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, u_2.getGridX(), u_2.getGridY()) <= 20 * 20)
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
        // Java: 89
        public bool blastReady()
        {
            com.oddlabs.tt.model.Unit chief = ai.intel().chieftain;
            return chief != null && !chief.isDead() && isViking() && chief.getHitPoints() > 40 && chief.canDoMagic(com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_BLAST) && !com.oddlabs.tt.player.gauntlet.Intel.isStunned(chief);
        }
        // Java: 95
        public bool stunReady()
        {
            com.oddlabs.tt.model.Unit chief = ai.intel().chieftain;
            return chief != null && !chief.isDead() && chief.canDoMagic(magicIndex());
        }
        // Java: 100
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
        // Java: 109
        public void tick()
        {
            checkCatch();
            com.oddlabs.tt.model.Unit chief = ai.intel().chieftain;
            if (chief == null)
            {
                if (had_chief)
                {
                    had_chief = false;
                    ai.aiLog().count("chief_lost");
                    float since = (float)(ai.time() - last_cast);
                    if (since >= 5.0f && since <= 40.0f)
                    {
                        ai.aiLog().count("chief_lost_wake");
                    }
                }
                considerTraining();
                return;
            }
            had_chief = true;
            if (com.oddlabs.tt.player.gauntlet.Intel.isStunned(chief))
            {
                return;
            }
            if (ai.strategy().blast && isViking() && chief.canDoMagic(com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_BLAST) && shouldBlast(chief))
            {
                ai.owner().doMagic(chief, com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_BLAST);
                last_cast = ai.time();
                return;
            }
            if (shred(chief))
            {
                return;
            }
            if (stunReady() && shouldStun(chief) && !savingForBlast(chief))
            {
                int x = chief.getGridX();
                int y = chief.getGridY();
                string rivals = "";
                foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(ai.intel().enemy_chieftains))
                {
                    if (!e.isDead())
                    {
                        rivals += java.lang.JStr.valueOf(java.lang.JString.format(" [enemy chief %d cells%s%s]", java.lang.JC.d2i(java.lang.Math.sqrt(com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, e.getGridX(), e.getGridY()))), ((e.getCurrentController() is com.oddlabs.tt.model.behaviour.MagicController) ? (object)(" casting") : (object)("")), (ai.military().enemySpellReady(e) ? (object)(" ready") : (object)(""))));
                    }
                }
                ai.log("chieftain stuns at " + (x) + "," + (y) + ": " + (com.oddlabs.tt.player.gauntlet.Combat.countNear(ai.intel().enemy_warriors, x, y, 17)) + " enemy warriors in reach" + java.lang.JStr.valueOf(rivals));
                ai.owner().doMagic(chief, magicIndex());
                last_cast = ai.time();
                if (isViking())
                {
                    cast_candidates.clear();
                    foreach (com.oddlabs.tt.model.Unit e_2 in java.lang.JIter.of(ai.intel().enemy_warriors))
                    {
                        if (!com.oddlabs.tt.player.gauntlet.Intel.isStunned(e_2) && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, e_2.getGridX(), e_2.getGridY()) <= 17 * 17)
                        {
                            cast_candidates.add(e_2);
                        }
                    }
                    cast_check = (float)(ai.time() + 8.0f);
                    cast_best = 0;
                }
                return;
            }
            if (ai.dodges().chiefBusy())
            {
                return;
            }
            if (ai.strategy().shred && ai.strategy().shred_strict && isViking())
            {
                den(chief);
            }
            else
            {
                position(chief);
            }
        }
        // Java: 172
        public void den(com.oddlabs.tt.model.Unit chief)
        {
            if ((float)(ai.time() - last_move) < com.oddlabs.tt.player.gauntlet.Chieftain.MOVE_PERIOD || ai.military().isDodging(chief))
            {
                return;
            }
            com.oddlabs.tt.player.gauntlet.Military military = ai.military();
            int tx = military.stagingX();
            int ty = military.stagingY();
            com.oddlabs.tt.model.Building home = ai.intel().armory();
            if (home != null)
            {
                int ax = home.getGridX();
                int ay = home.getGridY();
                float dx = ax - tx;
                float dy = ay - ty;
                float len = java.lang.Math.max(1.0f, ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy))))));
                int size = ai.map().getSize();
                tx = java.lang.Math.max(3, java.lang.Math.min(size - 4, ax + java.lang.Math.round((float)((float)(8 * dx) / len))));
                ty = java.lang.Math.max(3, java.lang.Math.min(size - 4, ay + java.lang.Math.round((float)((float)(8 * dy) / len))));
            }
            if (nearestWarriorDistance(chief.getGridX(), chief.getGridY()) <= 16)
            {
                com.oddlabs.tt.model.Building armory = ai.intel().armory();
                if (armory != null)
                {
                    tx = armory.getGridX();
                    ty = armory.getGridY();
                }
                int[] away = awayFromWarriors(tx, ty, 17);
                tx = away[0];
                ty = away[1];
            }
            if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(chief.getGridX(), chief.getGridY(), tx, ty) <= 4 * 4)
            {
                return;
            }
            last_move = ai.time();
            ai.landscapeOrder(com.oddlabs.tt.model.Selectable.newArray(chief), tx, ty, com.oddlabs.tt.model.Action.MOVE, false);
        }
        public int[] shred_target;
        public float shred_trace;
        public float shred_trace2;
        // Java: 218
        public bool shred(com.oddlabs.tt.model.Unit chief)
        {
            com.oddlabs.tt.player.gauntlet.Strategy strategy = ai.strategy();
            if (!strategy.shred || !isViking())
            {
                return false;
            }
            bool charged = chief.canDoMagic(com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_BLAST);
            if (ai.logging() && (float)(ai.time() - shred_trace) >= 20.0f)
            {
                shred_trace = ai.time();
                int[] b = findBlob(chief.getGridX(), chief.getGridY());
                ai.log(java.lang.JString.format("shred: charged %b hp %d blast %.2f blob %s", charged, chief.getHitPoints(), chief.getMagicProgress(com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_BLAST), (b == null ? (object)("none") : (object)((b[3]) + " at " + (b[0]) + "," + (b[1]) + " nearest " + (b[2])))));
            }
            if (!charged || chief.getHitPoints() <= strategy.shred_min_hp)
            {
                shred_target = null;
                return false;
            }
            int cx = chief.getGridX();
            int cy = chief.getGridY();
            if (strategy.shred_strict && blastHere(chief, cx, cy))
            {
                return true;
            }
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(ai.intel().enemy_warriors))
            {
                if (e.isDead() || com.oddlabs.tt.player.gauntlet.Intel.isParked(e))
                {
                    continue;
                }
                int abort = (strategy.shred_strict ? 9 : 13);
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(cx, cy, e.getGridX(), e.getGridY()) <= abort * abort)
                {
                    if (shred_target != null)
                    {
                        ai.aiLog().count("shred_abort");
                    }
                    shred_target = null;
                    return false;
                }
            }
            int[] blob = findBlob(cx, cy);
            if (blob == null)
            {
                shred_target = null;
                return false;
            }
            shred_target = blob;
            if (strategy.shred_strict)
            {
                java.util.List enemies = new java.util.ArrayList(ai.intel().enemy_warriors);
                enemies.addAll(ai.intel().enemy_chieftains);
                enemies.addAll(ai.intel().enemy_peons);
                int[] at = castPoint(blob, cx, cy);
                if (at == null && ai.logging() && (float)(ai.time() - shred_trace2) >= 20.0f)
                {
                    shred_trace2 = ai.time();
                    int[] seen_by = new int[4];
                    int cells = 0;
                    for (int r = 10; r <= 24; r += 2)
                    {
                        for (int a = 0; a < 24; a++)
                        {
                            double ang = a * java.lang.Math.PI / 12;
                            int x = blob[0] + unchecked((int)(java.lang.Math.round(r * java.lang.StrictMath.cos(ang))));
                            int y = blob[1] + unchecked((int)(java.lang.Math.round(r * java.lang.StrictMath.sin(ang))));
                            cells++;
                            foreach (com.oddlabs.tt.model.Unit e_2 in java.lang.JIter.of(enemies))
                            {
                                if (!e_2.isDead() && java.lang.Math.abs(e_2.getGridX() - x) <= 8 && java.lang.Math.abs(e_2.getGridY() - y) <= 8)
                                {
                                    int k = (ai.intel().enemy_peons.contains(e_2) ? 0 : (ai.intel().enemy_chieftains.contains(e_2) ? 1 : (com.oddlabs.tt.player.gauntlet.Intel.isParked(e_2) ? 2 : 3)));
                                    seen_by[k]++;
                                    break;
                                }
                            }
                        }
                    }
                    int fc = cells;
                    ai.log(java.lang.JString.format("shred: no point around blob %d at %d,%d: of %d ring cells seen by peon %d, chief %d, parked %d, awake %d", blob[3], blob[0], blob[1], fc, seen_by[0], seen_by[1], seen_by[2], seen_by[3]));
                }
                if (at == null)
                {
                    ai.aiLog().count("shred_nopoint");
                    return false;
                }
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(cx, cy, at[0], at[1]) <= 1 && at[2] >= strategy.shred_min)
                {
                    ai.log(java.lang.JString.format("chieftain blasts a parked blob at %d,%d from %d,%d (%d in reach)", blob[0], blob[1], cx, cy, at[2]));
                    ai.owner().doMagic(chief, com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_BLAST);
                    last_cast = ai.time();
                    last_move = ai.time();
                    ai.aiLog().count("shred_blast");
                    shred_target = null;
                    return true;
                }
                if ((float)(ai.time() - last_move) >= 1.0f && !ai.military().isDodging(chief))
                {
                    ai.landscapeOrder(com.oddlabs.tt.model.Selectable.newArray(chief), at[0], at[1], com.oddlabs.tt.model.Action.MOVE, false);
                    last_move = ai.time();
                }
                return true;
            }
            int nearest = blob[2];
            int centre2 = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(cx, cy, blob[0], blob[1]);
            if (nearest >= 9 && nearest <= 13 && centre2 <= 16 * 16)
            {
                ai.log(java.lang.JString.format("chieftain blasts a parked blob of %d at %d,%d (nearest %d cells)", blob[3], blob[0], blob[1], nearest));
                ai.owner().doMagic(chief, com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_BLAST);
                last_cast = ai.time();
                last_move = ai.time();
                ai.aiLog().count("shred_blast");
                shred_target = null;
                return true;
            }
            if ((float)(ai.time() - last_move) >= 1.0f && !ai.military().isDodging(chief))
            {
                int[] stop = (nearest > 11 ? com.oddlabs.tt.player.gauntlet.MapAnalysis.towards(cx, cy, blob[0], blob[1], java.lang.Math.max(2, nearest - 10)) : com.oddlabs.tt.player.gauntlet.MapAnalysis.towards(blob[0], blob[1], cx, cy, 12));
                ai.landscapeOrder(com.oddlabs.tt.model.Selectable.newArray(chief), stop[0], stop[1], com.oddlabs.tt.model.Action.MOVE, false);
                last_move = ai.time();
            }
            return true;
        }
        // Java: 331
        public bool savingForBlast(com.oddlabs.tt.model.Unit chief)
        {
            if (ai.strategy().shred && isViking() && ai.strategy().shred_strict)
            {
                return true;
            }
            if (!ai.strategy().shred || !isViking() || chief.canDoMagic(com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_BLAST))
            {
                return false;
            }
            if (ai.military().baseThreatLevel() >= 2 || chief.getHitPoints() <= ai.strategy().shred_min_hp)
            {
                return false;
            }
            return findBlob(chief.getGridX(), chief.getGridY()) != null;
        }
        // Java: 342
        public bool blastHere(com.oddlabs.tt.model.Unit chief, int cx, int cy)
        {
            java.util.List enemies = new java.util.ArrayList(ai.intel().enemy_warriors);
            enemies.addAll(ai.intel().enemy_chieftains);
            enemies.addAll(ai.intel().enemy_peons);
            java.util.List ours = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(ai.owner().getUnits().getSet()))
            {
                if (!s.isDead() && ((object)s != (object)chief))
                {
                    ours.add(s);
                }
            }
            int here = castValue(cx, cy, enemies, java.util.List_.of(), ours, null);
            if (here < ai.strategy().shred_min)
            {
                return false;
            }
            ai.log(java.lang.JString.format("chieftain blasts from %d,%d (%d enemy warriors in reach)", cx, cy, here));
            ai.owner().doMagic(chief, com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_BLAST);
            last_cast = ai.time();
            last_move = ai.time();
            ai.aiLog().count("shred_blast");
            for (int i = 0; i < here; i++)
            {
                ai.aiLog().count("shred_caught");
            }
            shred_target = null;
            return true;
        }
        // Java: 368
        public int castValue(int x, int y, java.util.List enemies, java.util.List parked, java.util.List ours, int[] why)
        {
            if (!ai.map().passable(x, y))
            {
                if (why != null)
                {
                    why[0]++;
                }
                return (-1);
            }
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(enemies))
            {
                if (!e.isDead() && java.lang.Math.abs(e.getGridX() - x) <= 5 && java.lang.Math.abs(e.getGridY() - y) <= 5)
                {
                    if (why != null)
                    {
                        why[1]++;
                    }
                    return (-1);
                }
            }
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(ai.intel().enemy_towers))
            {
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, t.getGridX(), t.getGridY()) <= 22 * 22)
                {
                    if (why != null)
                    {
                        why[2]++;
                    }
                    return (-1);
                }
            }
            int buildings = 0;
            int friends = 0;
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(ours))
            {
                int d2 = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, s.getGridX(), s.getGridY());
                if ((s is com.oddlabs.tt.model.Building b))
                {
                    if (d2 <= 18 * 18 && (++buildings > 2 || b.getHitPoints() < 40))
                    {
                        if (why != null)
                        {
                            why[3]++;
                        }
                        return (-1);
                    }
                }
                else if (d2 <= 19 * 19)
                {
                    friends++;
                }
            }
            int hit = 0;
            foreach (com.oddlabs.tt.model.Unit e_2 in java.lang.JIter.of(enemies))
            {
                if (!e_2.isDead() && !e_2.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD) && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, e_2.getGridX(), e_2.getGridY()) <= 17 * 17)
                {
                    hit++;
                }
            }
            if (hit < 3 * friends)
            {
                if (why != null)
                {
                    why[3]++;
                }
                return (-1);
            }
            if (hit < ai.strategy().shred_min)
            {
                if (why != null)
                {
                    why[4]++;
                }
                return (-1);
            }
            return hit;
        }
        // Java: 432
        public int[] castPoint(int[] blob, int cx, int cy)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            java.util.List enemies = new java.util.ArrayList(intel.enemy_warriors);
            enemies.addAll(intel.enemy_chieftains);
            enemies.addAll(intel.enemy_peons);
            java.util.List parked = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(intel.enemy_warriors))
            {
                if (!e.isDead() && com.oddlabs.tt.player.gauntlet.Intel.isParked(e))
                {
                    parked.add(e);
                }
            }
            java.util.List ours = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(ai.owner().getUnits().getSet()))
            {
                if (!s.isDead() && ((object)s != (object)intel.chieftain))
                {
                    ours.add(s);
                }
            }
            int[] best = null;
            float best_score = (float)(-java.lang.Float.MAX_VALUE);
            int[] why = new int[5];
            for (int r = 10; r <= 24; r += 2)
            {
                for (int a = 0; a < 24; a++)
                {
                    double ang = a * java.lang.Math.PI / 12;
                    int x = blob[0] + unchecked((int)(java.lang.Math.round(r * java.lang.StrictMath.cos(ang))));
                    int y = blob[1] + unchecked((int)(java.lang.Math.round(r * java.lang.StrictMath.sin(ang))));
                    int hit = castValue(x, y, enemies, parked, ours, why);
                    if (hit < 0)
                    {
                        continue;
                    }
                    int chiefs = 0;
                    foreach (com.oddlabs.tt.model.Unit c in java.lang.JIter.of(intel.enemy_chieftains))
                    {
                        if (!c.isDead() && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, c.getGridX(), c.getGridY()) <= 17 * 17)
                        {
                            chiefs++;
                        }
                    }
                    float score = (float)((float)(10.0f * (hit + 8 * chiefs)) - ((float)(java.lang.Math.sqrt(com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, cx, cy)))));
                    if (score > best_score)
                    {
                        best_score = score;
                        best = new int[] { x, y, hit };
                    }
                }
            }
            if (best == null)
            {
                int m = 0;
                for (int i = 1; i < why.Length; i++)
                {
                    if (why[i] > why[m])
                    {
                        m = i;
                    }
                }
                ai.aiLog().count("shred_nopoint_" + java.lang.JStr.valueOf(new string[] { "ground", "seen", "tower", "friends", "few" }[m]));
            }
            return best;
        }
        // Java: 482
        public int[] findBlob(int cx, int cy)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            com.oddlabs.tt.player.gauntlet.Strategy strategy = ai.strategy();
            com.oddlabs.tt.model.Building armory = intel.armory();
            int hx = (armory != null ? armory.getGridX() : ai.planner().getStartX());
            int hy = (armory != null ? armory.getGridY() : ai.planner().getStartY());
            java.util.List parked = new java.util.ArrayList();
            java.util.List awake = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(intel.enemy_warriors))
            {
                if (e.isDead())
                {
                    continue;
                }
                ((com.oddlabs.tt.player.gauntlet.Intel.isParked(e) ? parked : awake)).add(e);
            }
            int[] best = null;
            float best_score = 0.0f;
            int range2 = strategy.shred_range * strategy.shred_range;
            foreach (com.oddlabs.tt.model.Unit seed in java.lang.JIter.of(parked))
            {
                int sx = seed.getGridX();
                int sy = seed.getGridY();
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(sx, sy, hx, hy) > range2)
                {
                    continue;
                }
                long x = 0;
                long y = 0;
                int n = 0;
                foreach (com.oddlabs.tt.model.Unit e_2 in java.lang.JIter.of(parked))
                {
                    if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(sx, sy, e_2.getGridX(), e_2.getGridY()) <= 10 * 10)
                    {
                        x += e_2.getGridX();
                        y += e_2.getGridY();
                        n++;
                    }
                }
                if (n < strategy.shred_min)
                {
                    continue;
                }
                int bx = unchecked((int)((x / n)));
                int by = unchecked((int)((y / n)));
                if (!clearAround(bx, by, awake))
                {
                    continue;
                }
                float d = ((float)(java.lang.Math.sqrt(com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(cx, cy, bx, by))));
                float score = (float)(n / ((float)(30.0f + d)));
                if (score > best_score)
                {
                    int nearest = java.lang.Integer.MAX_VALUE;
                    foreach (com.oddlabs.tt.model.Unit e_3 in java.lang.JIter.of(parked))
                    {
                        nearest = java.lang.Math.min(nearest, com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(cx, cy, e_3.getGridX(), e_3.getGridY()));
                    }
                    best_score = score;
                    best = new int[] { bx, by, java.lang.JC.d2i(java.lang.Math.sqrt(nearest)), n };
                }
            }
            return best;
        }
        // Java: 532
        public bool clearAround(int bx, int by, java.util.List awake)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            if (ai.strategy().shred_strict)
            {
                foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(intel.enemy_towers))
                {
                    if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(bx, by, t.getGridX(), t.getGridY()) <= 22 * 22)
                    {
                        return false;
                    }
                }
                return true;
            }
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(awake))
            {
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(bx, by, e.getGridX(), e.getGridY()) <= 16 * 16)
                {
                    return false;
                }
            }
            foreach (com.oddlabs.tt.model.Building t_2 in java.lang.JIter.of(intel.enemy_towers))
            {
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(bx, by, t_2.getGridX(), t_2.getGridY()) <= 22 * 22)
                {
                    return false;
                }
            }
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(intel.warriors))
            {
                if (!u.isMounted() && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(bx, by, u.getGridX(), u.getGridY()) <= 20 * 20)
                {
                    return false;
                }
            }
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(intel.peons))
            {
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(bx, by, u_2.getGridX(), u_2.getGridY()) <= 20 * 20)
                {
                    return false;
                }
            }
            return true;
        }
        // Java: 558
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
                if (com.oddlabs.tt.player.gauntlet.Intel.isStunned(e))
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
            string state = (chief == null || chief.isDead() ? " (our chief died)" : (com.oddlabs.tt.player.gauntlet.Intel.isStunned(chief) ? " (our chief stunned)" : ""));
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
        // Java: 591
        public bool enemyDodges()
        {
            return ai.strategy().stun_learn && catch_rate < ai.strategy().dodge_catch;
        }
        public float train_possible;
        // Java: 602
        public com.oddlabs.tt.model.Building nearTrainer(com.oddlabs.tt.player.gauntlet.Intel intel)
        {
            com.oddlabs.tt.model.Building armory = intel.armory();
            com.oddlabs.tt.model.Building best = null;
            float best_score = (float)(-java.lang.Float.MAX_VALUE);
            foreach (com.oddlabs.tt.model.Building q in java.lang.JIter.of(intel.quarters))
            {
                if (!q.canBuildChieftain() || ai.military().enemyStrengthNear(q.getGridX(), q.getGridY(), 30) > 0.0f || q.getHitPoints() < ((com.oddlabs.tt.model.BuildingTemplate)q.getTemplate()).getMaxHitPoints() || ai.economy().isDoomed(q))
                {
                    continue;
                }
                float d = (armory == null ? 0.0f : ((float)(java.lang.Math.sqrt(com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(q.getGridX(), q.getGridY(), armory.getGridX(), armory.getGridY())))));
                float score = (float)(q.getUnitContainer().getNumSupplies() - (float)(0.25f * d));
                if (score > best_score)
                {
                    best_score = score;
                    best = q;
                }
            }
            if (best != null)
            {
                ai.aiLog().count("chief_train");
                return best;
            }
            if ((float)(ai.time() - train_possible) <= 60.0f)
            {
                return null;
            }
            int best_d = (-1);
            foreach (com.oddlabs.tt.model.Building q_2 in java.lang.JIter.of(intel.quarters))
            {
                if (!q_2.canBuildChieftain() || ai.economy().isDoomed(q_2))
                {
                    continue;
                }
                int nearest = java.lang.Integer.MAX_VALUE;
                foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(intel.enemy_warriors))
                {
                    if (!e.isDead())
                    {
                        nearest = java.lang.Math.min(nearest, com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(q_2.getGridX(), q_2.getGridY(), e.getGridX(), e.getGridY()));
                    }
                }
                if (nearest > best_d)
                {
                    best_d = nearest;
                    best = q_2;
                }
            }
            if (best != null)
            {
                ai.aiLog().count("chief_train_fallback");
            }
            return best;
        }
        // Java: 643
        public void considerTraining()
        {
            com.oddlabs.tt.player.gauntlet.Strategy strategy = ai.strategy();
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
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
            if (strategy.chief_trainer_near)
            {
                if (train_possible < 0.0f)
                {
                    train_possible = ai.time();
                }
                best = nearTrainer(intel);
                if (best == null && (float)(ai.time() - train_possible) <= 60.0f)
                {
                    return;
                }
            }
            else
            {
                foreach (com.oddlabs.tt.model.Building q in java.lang.JIter.of(intel.quarters))
                {
                    if (!q.canBuildChieftain() || ai.economy().isDoomed(q))
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
            }
            if (best != null)
            {
                ai.log("training chieftain in quarters at " + (best.getGridX()) + "," + (best.getGridY()));
                ai.owner().trainChieftain(best, true);
            }
        }
        // Java: 678
        public bool shouldStun(com.oddlabs.tt.model.Unit chief)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            int x = chief.getGridX();
            int y = chief.getGridY();
            int r2 = com.oddlabs.tt.player.gauntlet.Chieftain.STUN_CELLS * com.oddlabs.tt.player.gauntlet.Chieftain.STUN_CELLS;
            float warriors = 0.0f;
            bool dodges = enemyDodges();
            int core2 = ai.strategy().dodge_core * ai.strategy().dodge_core;
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(intel.enemy_warriors))
            {
                if (com.oddlabs.tt.player.gauntlet.Intel.isStunned(e))
                {
                    continue;
                }
                int d2 = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, e.getGridX(), e.getGridY());
                if (d2 <= r2)
                {
                    warriors = (float)(warriors + (!dodges || d2 <= core2 ? 1.0f : 0.25f));
                }
            }
            int towers = 0;
            int tower_r2 = (ai.strategy().chief_tower_standoff ? com.oddlabs.tt.player.gauntlet.Chieftain.TOWER_CAUGHT2 : r2);
            int[] army = ai.military().attackCenter();
            bool follow_up = !ai.strategy().tower_stun_follow_up || (army != null && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, army[0], army[1]) <= 25 * 25);
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(intel.enemy_towers))
            {
                if (follow_up && com.oddlabs.tt.player.gauntlet.Intel.isTowerActive(t) && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, t.getGridX(), t.getGridY()) <= tower_r2)
                {
                    towers++;
                }
            }
            int chiefs = 0;
            foreach (com.oddlabs.tt.model.Unit e_2 in java.lang.JIter.of(intel.enemy_chieftains))
            {
                if (com.oddlabs.tt.player.gauntlet.Intel.isStunned(e_2) || com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, e_2.getGridX(), e_2.getGridY()) > com.oddlabs.tt.player.gauntlet.Chieftain.STUN_REACH * com.oddlabs.tt.player.gauntlet.Chieftain.STUN_REACH)
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
            int ours = com.oddlabs.tt.player.gauntlet.Combat.countNear(intel.warriors, x, y, 20);
            float needed = (ours >= 6 ? 4.0f : 6.0f);
            if (caught < needed)
            {
                return false;
            }
            if (!ai.strategy().stun_patience)
            {
                return true;
            }
            float coming = (float)((float)(warriors + com.oddlabs.tt.player.gauntlet.Combat.countNear(intel.enemy_warriors, x, y, 32)) - com.oddlabs.tt.player.gauntlet.Combat.countNear(intel.enemy_warriors, x, y, com.oddlabs.tt.player.gauntlet.Chieftain.STUN_CELLS));
            float share = (float)(caught / java.lang.Math.max(1.0f, (float)((float)(coming + (float)(3.0f * towers)) + (float)(3.0f * chiefs))));
            bool answer = false;
            foreach (com.oddlabs.tt.model.Unit e_3 in java.lang.JIter.of(intel.enemy_chieftains))
            {
                answer |= !com.oddlabs.tt.player.gauntlet.Intel.isStunned(e_3) && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, e_3.getGridX(), e_3.getGridY()) <= 45 * 45 && ai.military().enemySpellReady(e_3);
            }
            if (answer)
            {
                return caught >= 20.0f || (share >= 0.8f && caught >= 10.0f);
            }
            return caught >= 20.0f || share >= 0.55f;
        }
        // Java: 745
        public void position(com.oddlabs.tt.model.Unit chief)
        {
            int safe = ai.strategy().chief_safe;
            bool threatened = safe > 0 && !stunReady() && nearestWarriorDistance(chief.getGridX(), chief.getGridY()) <= safe;
            float wake = ai.strategy().chief_wake_retreat;
            int wake_keep = ai.strategy().chief_wake_keep;
            bool waking = wake > 0.0f && !stunReady() && (float)(ai.time() - last_cast) >= wake;
            threatened |= waking && nearestWarriorDistance(chief.getGridX(), chief.getGridY(), true) <= wake_keep;
            if (((float)(ai.time() - last_move) < com.oddlabs.tt.player.gauntlet.Chieftain.MOVE_PERIOD && !threatened) || ai.military().isDodging(chief))
            {
                return;
            }
            com.oddlabs.tt.player.gauntlet.Military military = ai.military();
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
                    int[] stop = com.oddlabs.tt.player.gauntlet.MapAnalysis.towards(chief.getGridX(), chief.getGridY(), at[0], at[1], java.lang.Math.max(3, nearest - 9));
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
                        int[] stop_2 = com.oddlabs.tt.player.gauntlet.MapAnalysis.towards(chief.getGridX(), chief.getGridY(), enemies[0], enemies[1], nearest_2 - keep);
                        tx = stop_2[0];
                        ty = stop_2[1];
                    }
                }
            }
            else if (army != null)
            {
                int[] back = com.oddlabs.tt.player.gauntlet.MapAnalysis.towards(army[0], army[1], military.stagingX(), military.stagingY(), (enemies != null ? 8 : 3));
                tx = back[0];
                ty = back[1];
            }
            else if (military.baseThreatLevel() > 0)
            {
                int[] back_2 = com.oddlabs.tt.player.gauntlet.MapAnalysis.towards(military.threatX(), military.threatY(), military.stagingX(), military.stagingY(), 12);
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
            if (waking)
            {
                int[] away_2 = awayFromWarriors(tx, ty, wake_keep + 1, true);
                if (away_2[0] != tx || away_2[1] != ty)
                {
                    ai.aiLog().count("chief_wake_move");
                }
                tx = away_2[0];
                ty = away_2[1];
            }
            if (ai.strategy().chief_tower_standoff && chief.getHitPoints() > 24)
            {
                int[] clear = clearOfTowers(tx, ty, chief.getGridX(), chief.getGridY());
                tx = clear[0];
                ty = clear[1];
            }
            if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(chief.getGridX(), chief.getGridY(), tx, ty) <= 3 * 3)
            {
                return;
            }
            last_move = ai.time();
            ai.landscapeOrder(com.oddlabs.tt.model.Selectable.newArray(chief), tx, ty, com.oddlabs.tt.model.Action.MOVE, false);
        }
        // Java: 838
        public int[] clearOfTowers(int x, int y, int from_x, int from_y)
        {
            for (int pass = 0; pass < 3; pass++)
            {
                com.oddlabs.tt.model.Building near = null;
                int best = com.oddlabs.tt.player.gauntlet.Chieftain.TOWER_KEEP * com.oddlabs.tt.player.gauntlet.Chieftain.TOWER_KEEP;
                foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(ai.intel().enemy_towers))
                {
                    int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, t.getGridX(), t.getGridY());
                    if (com.oddlabs.tt.player.gauntlet.Intel.isTowerActive(t) && d < best)
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
                x = near.getGridX() + java.lang.Math.round((float)((float)(dx / len) * com.oddlabs.tt.player.gauntlet.Chieftain.TOWER_KEEP));
                y = near.getGridY() + java.lang.Math.round((float)((float)(dy / len) * com.oddlabs.tt.player.gauntlet.Chieftain.TOWER_KEEP));
            }
            int size = ai.map().getSize();
            return new int[] { java.lang.Math.clamp(x, 0, size - 1), java.lang.Math.clamp(y, 0, size - 1) };
        }
        // Java: 866
        public int[] awayFromWarriors(int x, int y, int keep)
        {
            return awayFromWarriors(x, y, keep, false);
        }
        // Java: 871
        public int[] awayFromWarriors(int x, int y, int keep, bool include_stunned)
        {
            for (int pass = 0; pass < 3; pass++)
            {
                com.oddlabs.tt.model.Unit near = null;
                int best = keep * keep;
                foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(ai.intel().enemy_warriors))
                {
                    if (!include_stunned && com.oddlabs.tt.player.gauntlet.Intel.isStunned(e))
                    {
                        continue;
                    }
                    int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, e.getGridX(), e.getGridY());
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
        // Java: 901
        public int nearestWarriorDistance(int x, int y)
        {
            return nearestWarriorDistance(x, y, false);
        }
        // Java: 906
        public int nearestWarriorDistance(int x, int y, bool include_stunned)
        {
            int best = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(ai.intel().enemy_warriors))
            {
                if (include_stunned || !com.oddlabs.tt.player.gauntlet.Intel.isStunned(e))
                {
                    best = java.lang.Math.min(best, com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, e.getGridX(), e.getGridY()));
                }
            }
            return (best == java.lang.Integer.MAX_VALUE ? 1000 : java.lang.JC.d2i(java.lang.Math.sqrt(best)));
        }
        // Java: 915
        public int[] nearestEnemies(int x, int y, int radius)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            com.oddlabs.tt.model.Selectable nearest = null;
            int best = radius * radius;
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(intel.enemy_warriors))
            {
                if (com.oddlabs.tt.player.gauntlet.Intel.isStunned(e))
                {
                    continue;
                }
                int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, e.getGridX(), e.getGridY());
                if (d < best)
                {
                    best = d;
                    nearest = e;
                }
            }
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(intel.enemy_towers))
            {
                if (!com.oddlabs.tt.player.gauntlet.Intel.isTowerActive(t))
                {
                    continue;
                }
                int d_2 = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, t.getGridX(), t.getGridY());
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
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(nearest.getGridX(), nearest.getGridY(), e_2.getGridX(), e_2.getGridY()) <= 12 * 12)
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
            this.shred_trace = (float)(-100.0f);
            this.shred_trace2 = (float)(-100.0f);
            this.train_possible = (float)(-1.0f);
        }
        public Chieftain(com.oddlabs.tt.player.gauntlet.GauntletAI ai) : base()
        {
            init_Chieftain();
            this.ai = ai;
        }
    }
}
