// Translated from ElitePowers.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 27
    public class ElitePowers : java.lang.Object
    {
        public const int NONE = 0;
        public const int KNIGHT = 1;
        public const int MARKSMAN = 2;
        public const int DRUMMER = 3;
        public const int MAX_RANK = 3;
        public static int[] RANK_XP;
        public const int XP_UNIT = 4;
        public const int XP_HIT = 1;
        public const int XP_KILL = 4;
        public const float CLEAVE_RADIUS = 2.5f;
        public const float LAST_STAND_EVERY = 2.0f;
        public const float BEAT_EVERY = 2.0f;
        public const float BEAT_LASTS = 3.0f;
        public const int BEAT_RADIUS = 4;
        public const int BEAT_RADIUS_WIDE = 6;
        public const float LONG_SHOT = 2.0f;
        // Java: 43
        public static int kind(com.oddlabs.tt.model.Unit u)
        {
            com.oddlabs.tt.model.Race race = u.getOwner().getRace();
            com.oddlabs.tt.model.UnitTemplate t = ((com.oddlabs.tt.model.UnitTemplate)u.getTemplate());
            if (((object)t == (object)race.getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_ELITE_1)))
            {
                return com.oddlabs.tt.model.ElitePowers.KNIGHT;
            }
            if (((object)t == (object)race.getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_ELITE_2)))
            {
                return com.oddlabs.tt.model.ElitePowers.MARKSMAN;
            }
            if (((object)t == (object)race.getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_ELITE_3)))
            {
                return com.oddlabs.tt.model.ElitePowers.DRUMMER;
            }
            return com.oddlabs.tt.model.ElitePowers.NONE;
        }
        // Java: 56
        public static int rank(com.oddlabs.tt.model.Unit u)
        {
            int xp = u.getEliteXp();
            int r = 0;
            while (r < com.oddlabs.tt.model.ElitePowers.MAX_RANK && xp >= com.oddlabs.tt.model.ElitePowers.RANK_XP[r] * com.oddlabs.tt.model.ElitePowers.XP_UNIT)
            {
                r++;
            }
            return r;
        }
        // Java: 65
        public static int points(com.oddlabs.tt.model.Unit u)
        {
            return u.getEliteXp() / com.oddlabs.tt.model.ElitePowers.XP_UNIT;
        }
        // Java: 69
        public static int nextRankPoints(com.oddlabs.tt.model.Unit u)
        {
            int r = com.oddlabs.tt.model.ElitePowers.rank(u);
            return (r >= com.oddlabs.tt.model.ElitePowers.MAX_RANK ? 0 : com.oddlabs.tt.model.ElitePowers.RANK_XP[r]);
        }
        // Java: 75
        public static string passiveKey(int kind_v, int r)
        {
            return ((kind_v) switch { 1 => (string)((r == 1 ? "elite_hardened" : (r == 2 ? "elite_cleave" : "elite_last_stand"))), 2 => (string)((r == 1 ? "elite_keen_eye" : (r == 2 ? "elite_long_shot" : "elite_piercing"))), _ => (string)((r == 1 ? "elite_mending" : (r == 2 ? "elite_fury" : "elite_thunder"))) });
        }
        // Java: 83
        public static bool has(com.oddlabs.tt.model.Unit u, int kind_v, int r)
        {
            return u.getEliteKind() == kind_v && com.oddlabs.tt.model.ElitePowers.rank(u) >= r;
        }
        // Java: 88
        public static void gain(com.oddlabs.tt.model.Unit u, int points_v)
        {
            if (u.isDead() || u.getEliteKind() == com.oddlabs.tt.model.ElitePowers.NONE)
            {
                return;
            }
            int before = com.oddlabs.tt.model.ElitePowers.rank(u);
            int training = u.getOwner().getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.ELITE_TRAINING);
            u.addEliteXp(points_v * (com.oddlabs.tt.model.ElitePowers.XP_UNIT + 2 * training));
            if (com.oddlabs.tt.model.ElitePowers.rank(u) > before && com.oddlabs.tt.model.ElitePowers.has(u, com.oddlabs.tt.model.ElitePowers.KNIGHT, 1) && before < 1)
            {
                u.heal(u.getEffectiveMaxHitPoints());
            }
        }
        // Java: 101
        public static int hitPoints(com.oddlabs.tt.model.Unit u, int hp)
        {
            return (com.oddlabs.tt.model.ElitePowers.has(u, com.oddlabs.tt.model.ElitePowers.KNIGHT, 1) ? hp + (hp + 3) / 4 : hp);
        }
        // Java: 106
        public static int bonusDamage(com.oddlabs.tt.model.Unit src)
        {
            int bonus = 0;
            if (com.oddlabs.tt.model.ElitePowers.has(src, com.oddlabs.tt.model.ElitePowers.KNIGHT, 3) && src.getHitPoints() * 3 <= src.getEffectiveMaxHitPoints())
            {
                bonus++;
            }
            if (com.oddlabs.tt.model.ElitePowers.has(src, com.oddlabs.tt.model.ElitePowers.MARKSMAN, 3))
            {
                bonus++;
            }
            if (src.isDrummed() && src.getDrumRank() >= 2 && src.nextFuryHit() % 2 == 0)
            {
                bonus++;
            }
            return bonus;
        }
        // Java: 118
        public static float accuracy(com.oddlabs.tt.model.Unit src)
        {
            return (com.oddlabs.tt.model.ElitePowers.has(src, com.oddlabs.tt.model.ElitePowers.MARKSMAN, 1) ? 1.2f : 1.0f);
        }
        // Java: 123
        public static float rangeBonus(com.oddlabs.tt.model.Unit u)
        {
            return (com.oddlabs.tt.model.ElitePowers.has(u, com.oddlabs.tt.model.ElitePowers.MARKSMAN, 2) ? com.oddlabs.tt.model.ElitePowers.LONG_SHOT : 0.0f);
        }
        // Java: 128
        public static float attackSpeed(com.oddlabs.tt.model.Unit u)
        {
            if (!u.isDrummed())
            {
                return 1.0f;
            }
            return (u.getDrumRank() >= 3 ? 1.25f : 1.15f);
        }
        // Java: 135
        public static void afterHit(com.oddlabs.tt.model.Unit src, com.oddlabs.tt.model.Selectable target, int damage)
        {
            int kind_v = src.getEliteKind();
            if (kind_v == com.oddlabs.tt.model.ElitePowers.NONE || src.isDead() || !((target is com.oddlabs.tt.model.Unit)) && !((target is com.oddlabs.tt.model.Building)))
            {
                return;
            }
            if (!src.getOwner().isEnemy(target.getOwnerNoCheck()))
            {
                return;
            }
            com.oddlabs.tt.model.ElitePowers.gain(src, (target.isDead() && (target is com.oddlabs.tt.model.Unit) ? com.oddlabs.tt.model.ElitePowers.XP_HIT + com.oddlabs.tt.model.ElitePowers.XP_KILL : com.oddlabs.tt.model.ElitePowers.XP_HIT));
            if (kind_v == com.oddlabs.tt.model.ElitePowers.KNIGHT && com.oddlabs.tt.model.ElitePowers.rank(src) >= 2 && (target is com.oddlabs.tt.model.Unit) && !src.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.THROW))
            {
                com.oddlabs.tt.model.ElitePowers.cleave(src, target, java.lang.Math.max(1, damage / 2));
            }
        }
        // Java: 146
        public static void cleave(com.oddlabs.tt.model.Unit src, com.oddlabs.tt.model.Selectable target, int damage)
        {
            com.oddlabs.tt.pathfinder.FindOccupantFilter filter = new com.oddlabs.tt.pathfinder.FindOccupantFilter(target.getPositionX(), target.getPositionY(), com.oddlabs.tt.model.ElitePowers.CLEAVE_RADIUS, target, typeof(com.oddlabs.tt.model.Unit));
            src.getOwner().getWorld().getUnitGrid().scan(filter, target.getGridX(), target.getGridY());
            com.oddlabs.tt.model.Unit best = null;
            float best_d = java.lang.Float.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(filter.getResult()))
            {
                if (((object)u == (object)target) || u.isDead() || u.isMounted() || !src.getOwner().isEnemy(u.getOwner()))
                {
                    continue;
                }
                float dx = (float)(u.getPositionX() - target.getPositionX());
                float dy = (float)(u.getPositionY() - target.getPositionY());
                float d = (float)((float)(dx * dx) + (float)(dy * dy));
                if (d < best_d)
                {
                    best_d = d;
                    best = u;
                }
            }
            if (best == null)
            {
                return;
            }
            float len = ((float)(java.lang.Math.sqrt(best_d)));
            best.hit(damage, (len > 0.0f ? (float)(((float)(best.getPositionX() - target.getPositionX())) / len) : 0.0f), (len > 0.0f ? (float)(((float)(best.getPositionY() - target.getPositionY())) / len) : 1.0f), src.getOwner());
            if (best.isDead())
            {
                com.oddlabs.tt.model.ElitePowers.gain(src, com.oddlabs.tt.model.ElitePowers.XP_KILL);
            }
        }
        // Java: 174
        public static void animate(com.oddlabs.tt.model.Unit u, float t)
        {
            int kind_v = u.getEliteKind();
            if (kind_v == com.oddlabs.tt.model.ElitePowers.KNIGHT && com.oddlabs.tt.model.ElitePowers.rank(u) >= 3 && u.getHitPoints() * 3 <= u.getEffectiveMaxHitPoints())
            {
                if (u.tickEliteTimer(t, com.oddlabs.tt.model.ElitePowers.LAST_STAND_EVERY))
                {
                    u.heal(1);
                }
            }
            else if (kind_v == com.oddlabs.tt.model.ElitePowers.DRUMMER && u.tickEliteTimer(t, com.oddlabs.tt.model.ElitePowers.BEAT_EVERY))
            {
                com.oddlabs.tt.model.ElitePowers.beat(u);
            }
        }
        // Java: 184
        public static void beat(com.oddlabs.tt.model.Unit drummer)
        {
            int r = com.oddlabs.tt.model.ElitePowers.rank(drummer);
            com.oddlabs.tt.model.ElitePowers.Beat scan = new com.oddlabs.tt.model.ElitePowers.Beat(drummer, (r >= 3 ? com.oddlabs.tt.model.ElitePowers.BEAT_RADIUS_WIDE : com.oddlabs.tt.model.ElitePowers.BEAT_RADIUS));
            drummer.getUnitGrid().scan(scan, com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(drummer.getPositionX()), com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(drummer.getPositionY()));
            bool mend = r >= 1 && drummer.nextBeat() % 2 == 0;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(scan.friends))
            {
                if (u.isDead())
                {
                    continue;
                }
                u.drum(com.oddlabs.tt.model.ElitePowers.BEAT_LASTS, r);
                if (mend)
                {
                    u.heal(1);
                }
            }
            if (scan.enemies > 0)
            {
                com.oddlabs.tt.model.ElitePowers.gain(drummer, com.oddlabs.tt.model.ElitePowers.XP_HIT);
            }
        }
        // Java: 202
        public class Beat : java.lang.Object, com.oddlabs.tt.pathfinder.ScanFilter
        {
            public com.oddlabs.tt.model.Unit drummer;
            public int radius;
            public java.util.List friends;
            public int enemies;
            // Java: 213
            public int getMinRadius()
            {
                return 0;
            }
            // Java: 218
            public int getMaxRadius()
            {
                return radius;
            }
            // Java: 223
            public bool filter(int grid_x, int grid_y, com.oddlabs.tt.pathfinder.Occupant occ)
            {
                if (!((occ is com.oddlabs.tt.model.Unit u)) || u.isDead())
                {
                    return false;
                }
                com.oddlabs.tt.player.Player me = drummer.getOwnerNoCheck();
                if (((object)u.getOwnerNoCheck() == (object)me))
                {
                    if (((object)u != (object)drummer) && u.isWarrior() && !friends.contains(u))
                    {
                        friends.add(u);
                    }
                }
                else if (me.isEnemy(u.getOwnerNoCheck()))
                {
                    enemies++;
                }
                return false;
            }
            private void init_Beat()
            {
                this.friends = new java.util.ArrayList();
            }
            public Beat(com.oddlabs.tt.model.Unit drummer, int radius) : base()
            {
                init_Beat();
                this.drummer = drummer;
                this.radius = radius;
            }
        }
        static ElitePowers()
        {
            ElitePowers.RANK_XP = new int[] { 12, 35, 70 };
        }
        public ElitePowers() : base()
        {
        }
    }
}
