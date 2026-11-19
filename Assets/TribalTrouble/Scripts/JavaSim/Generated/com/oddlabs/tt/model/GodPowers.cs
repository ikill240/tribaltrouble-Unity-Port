// Translated from GodPowers.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 30
    public class GodPowers : java.lang.Object
    {
        public const int NONE = 0;
        public const int MINOR = 1;
        public const int MAJOR = 2;
        public const int MINOR_HP_PER_KILL = 6;
        public const int MAJOR_HP_PER_KILL = 10;
        public const int MAX_DAMAGE_BONUS = 4;
        public const float POWER_MINOR_JAGUAR = 12.0f;
        public const float POWER_MINOR_VALKYRIE = 14.0f;
        public const float POWER_MAJOR_TIKI = 18.0f;
        public const float POWER_MAJOR_THUNDER = 15.0f;
        public const float WEATHER_EVERY = 120.0f;
        public static int[] SUMMONED;
        // Java: 45
        public static int kind(com.oddlabs.tt.model.Unit u)
        {
            com.oddlabs.tt.model.Race race = u.getOwner().getRace();
            com.oddlabs.tt.model.UnitTemplate t = ((com.oddlabs.tt.model.UnitTemplate)u.getTemplate());
            if (((object)t == (object)race.getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_MINOR_GOD)))
            {
                return com.oddlabs.tt.model.GodPowers.MINOR;
            }
            if (((object)t == (object)race.getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_MAJOR_GOD)))
            {
                return com.oddlabs.tt.model.GodPowers.MAJOR;
            }
            return com.oddlabs.tt.model.GodPowers.NONE;
        }
        // Java: 56
        public static int bonusHitPoints(com.oddlabs.tt.model.Unit u, int kind_v)
        {
            return u.getGodKills() * ((kind_v == com.oddlabs.tt.model.GodPowers.MAJOR ? com.oddlabs.tt.model.GodPowers.MAJOR_HP_PER_KILL : com.oddlabs.tt.model.GodPowers.MINOR_HP_PER_KILL));
        }
        // Java: 61
        public static int bonusDamage(com.oddlabs.tt.model.Unit u)
        {
            return java.lang.Math.min(com.oddlabs.tt.model.GodPowers.MAX_DAMAGE_BONUS, u.getGodKills() / 3);
        }
        // Java: 66
        public static float growth(com.oddlabs.tt.model.Unit u)
        {
            return (float)(1.0f + java.lang.Math.min(0.6f, (float)(0.03f * u.getGodKills())));
        }
        // Java: 71
        public static void killed(com.oddlabs.tt.model.Unit god)
        {
            if (god.isDead())
            {
                return;
            }
            int before = god.getEffectiveMaxHitPoints();
            god.addGodKill();
            int gained = god.getEffectiveMaxHitPoints() - before;
            if (gained > 0)
            {
                god.heal(gained);
            }
        }
        // Java: 82
        public static void animate(com.oddlabs.tt.model.Unit god, int kind_v, float t)
        {
            com.oddlabs.tt.landscape.World world = god.getOwner().getWorld();
            bool vik = com.oddlabs.tt.model.ExtendedCombat.isVikings(god.getOwner());
            if (god.tickGodPower(t) >= ((kind_v == com.oddlabs.tt.model.GodPowers.MAJOR ? ((vik ? com.oddlabs.tt.model.GodPowers.POWER_MAJOR_THUNDER : com.oddlabs.tt.model.GodPowers.POWER_MAJOR_TIKI)) : ((vik ? com.oddlabs.tt.model.GodPowers.POWER_MINOR_VALKYRIE : com.oddlabs.tt.model.GodPowers.POWER_MINOR_JAGUAR)))))
            {
                bool used = (kind_v == com.oddlabs.tt.model.GodPowers.MAJOR ? ((vik ? com.oddlabs.tt.model.GodPowers.thunder(god) : com.oddlabs.tt.model.GodPowers.tikiFire(god))) : ((vik ? com.oddlabs.tt.model.GodPowers.valkyrie(god) : com.oddlabs.tt.model.GodPowers.pounce(god))));
                if (used)
                {
                    god.resetGodPower();
                }
            }
            if (kind_v == com.oddlabs.tt.model.GodPowers.MAJOR && god.tickGodWeather(t) >= com.oddlabs.tt.model.GodPowers.WEATHER_EVERY)
            {
                if (com.oddlabs.tt.model.GodPowers.summonWeather(god, world))
                {
                    god.resetGodWeather();
                }
            }
        }
        // Java: 99
        public static java.util.List enemiesNear(com.oddlabs.tt.model.Unit god, float radius)
        {
            com.oddlabs.tt.pathfinder.FindOccupantFilter filter = new com.oddlabs.tt.pathfinder.FindOccupantFilter(god.getPositionX(), god.getPositionY(), radius, god, typeof(com.oddlabs.tt.model.Unit));
            god.getOwner().getWorld().getUnitGrid().scan(filter, god.getGridX(), god.getGridY());
            java.util.List list = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(filter.getResult()))
            {
                if (!u.isDead() && !u.isMounted() && god.getOwner().isEnemy(u.getOwner()))
                {
                    list.add(u);
                }
            }
            return list;
        }
        // Java: 110
        public static com.oddlabs.tt.model.Unit nearest(com.oddlabs.tt.model.Unit god, java.util.List units)
        {
            com.oddlabs.tt.model.Unit best = null;
            float best_d = java.lang.Float.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(units))
            {
                float dx = (float)(u.getPositionX() - god.getPositionX());
                float dy = (float)(u.getPositionY() - god.getPositionY());
                float d = (float)((float)(dx * dx) + (float)(dy * dy));
                if (d < best_d)
                {
                    best_d = d;
                    best = u;
                }
            }
            return best;
        }
        // Java: 124
        public static void strike(com.oddlabs.tt.model.Unit god, com.oddlabs.tt.model.Unit victim, int damage)
        {
            float dx = (float)(victim.getPositionX() - god.getPositionX());
            float dy = (float)(victim.getPositionY() - god.getPositionY());
            float len = ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy)))));
            victim.hit(damage, (len > 0.0f ? (float)(dx / len) : 0.0f), (len > 0.0f ? (float)(dy / len) : 1.0f), god.getOwner());
            if (victim.isDead())
            {
                com.oddlabs.tt.model.GodPowers.killed(god);
            }
        }
        // Java: 133
        public static bool pounce(com.oddlabs.tt.model.Unit god)
        {
            com.oddlabs.tt.model.Unit v = com.oddlabs.tt.model.GodPowers.nearest(god, com.oddlabs.tt.model.GodPowers.enemiesNear(god, 10.0f));
            if (v == null)
            {
                return false;
            }
            com.oddlabs.tt.model.GodPowers.strike(god, v, 2 + com.oddlabs.tt.model.GodPowers.bonusDamage(god));
            if (!v.isDead())
            {
                v.stun(3.0f);
            }
            return true;
        }
        // Java: 144
        public static bool valkyrie(com.oddlabs.tt.model.Unit god)
        {
            if (com.oddlabs.tt.model.GodPowers.enemiesNear(god, 16.0f).isEmpty())
            {
                return false;
            }
            com.oddlabs.tt.pathfinder.FindOccupantFilter filter = new com.oddlabs.tt.pathfinder.FindOccupantFilter(god.getPositionX(), god.getPositionY(), 8.0f, null, typeof(com.oddlabs.tt.model.Unit));
            god.getOwner().getWorld().getUnitGrid().scan(filter, god.getGridX(), god.getGridY());
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(filter.getResult()))
            {
                if (!u.isDead() && !god.getOwner().isEnemy(u.getOwner()))
                {
                    u.heal(1);
                    u.applyCourageBuff(6.0f, 1.1f, 0.0f, 1.2f, 1.2f, 0.0f);
                }
            }
            return true;
        }
        // Java: 159
        public static bool tikiFire(com.oddlabs.tt.model.Unit god)
        {
            java.util.List near = com.oddlabs.tt.model.GodPowers.enemiesNear(god, 14.0f);
            com.oddlabs.tt.model.Unit centre = null;
            int best = 0;
            foreach (com.oddlabs.tt.model.Unit c in java.lang.JIter.of(near))
            {
                int n = 0;
                foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(near))
                {
                    float dx = (float)(u.getPositionX() - c.getPositionX());
                    float dy = (float)(u.getPositionY() - c.getPositionY());
                    if ((float)((float)(dx * dx) + (float)(dy * dy)) <= 16.0f)
                    {
                        n++;
                    }
                }
                if (n > best)
                {
                    best = n;
                    centre = c;
                }
            }
            if (centre == null)
            {
                return false;
            }
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(near))
            {
                float dx_2 = (float)(u_2.getPositionX() - centre.getPositionX());
                float dy_2 = (float)(u_2.getPositionY() - centre.getPositionY());
                if ((float)((float)(dx_2 * dx_2) + (float)(dy_2 * dy_2)) <= 16.0f)
                {
                    u_2.afflictions().burn(6.0f, 1 + com.oddlabs.tt.model.GodPowers.bonusDamage(god) / 2, god.getOwner());
                    com.oddlabs.tt.model.GodPowers.strike(god, u_2, 1);
                }
            }
            return true;
        }
        // Java: 188
        public static bool thunder(com.oddlabs.tt.model.Unit god)
        {
            com.oddlabs.tt.model.Unit v = com.oddlabs.tt.model.GodPowers.nearest(god, com.oddlabs.tt.model.GodPowers.enemiesNear(god, 16.0f));
            if (v == null)
            {
                return false;
            }
            com.oddlabs.tt.landscape.World world = god.getOwner().getWorld();
            int damage = 3 + com.oddlabs.tt.model.GodPowers.bonusDamage(god);
            com.oddlabs.tt.model.GodPowers.strike(god, v, damage);
            com.oddlabs.tt.model.Unit arc_from = v;
            foreach (com.oddlabs.tt.model.Unit[] arc in java.lang.JIter.of(com.oddlabs.tt.model.ExtendedCombat.chainLightning(world, god.getOwner(), arc_from, damage, 4)))
            {
                if (arc[1].isDead())
                {
                    com.oddlabs.tt.model.GodPowers.killed(god);
                }
            }
            com.oddlabs.tt.landscape.Weather.bolt(world, v.getPositionX(), v.getPositionY());
            return true;
        }
        // Java: 204
        public static bool summonWeather(com.oddlabs.tt.model.Unit god, com.oddlabs.tt.landscape.World world)
        {
            com.oddlabs.tt.player.Player me = god.getOwner();
            java.util.List bases = new java.util.ArrayList();
            java.util.List units = new java.util.ArrayList();
            foreach (com.oddlabs.tt.player.Player p in world.getPlayers())
            {
                if (((object)p == (object)me) || !me.isEnemy(p) || !p.isAlive())
                {
                    continue;
                }
                foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(p.getUnits().getSet()))
                {
                    if (s.isDead())
                    {
                        continue;
                    }
                    if ((s is com.oddlabs.tt.model.Building b) && (b.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.REPRODUCE) || b.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD_ARMIES)))
                    {
                        bases.add(b);
                    }
                    else if ((s is com.oddlabs.tt.model.Unit u) && !u.isMounted())
                    {
                        units.add(u);
                    }
                }
            }
            bool at_base = !bases.isEmpty() && (units.isEmpty() || world.getRandom().nextFloat() < 0.7f);
            java.util.List pool = (at_base ? bases : units);
            if (pool.isEmpty())
            {
                return false;
            }
            com.oddlabs.tt.model.Selectable at = null;
            float best = java.lang.Float.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Selectable s_2 in java.lang.JIter.of(pool))
            {
                float dx = (float)(s_2.getPositionX() - god.getPositionX());
                float dy = (float)(s_2.getPositionY() - god.getPositionY());
                float d = (float)((float)(dx * dx) + (float)(dy * dy));
                if (d < best)
                {
                    best = d;
                    at = s_2;
                }
            }
            int kind_v = com.oddlabs.tt.model.GodPowers.SUMMONED[world.getRandom().nextInt(com.oddlabs.tt.model.GodPowers.SUMMONED.Length)];
            return world.summonWeather(kind_v, at.getPositionX(), at.getPositionY());
        }
        static GodPowers()
        {
            GodPowers.SUMMONED = new int[] { com.oddlabs.tt.landscape.Weather.STORM, com.oddlabs.tt.landscape.Weather.TORNADO, com.oddlabs.tt.landscape.Weather.QUAKE, com.oddlabs.tt.landscape.Weather.HAIL, com.oddlabs.tt.landscape.Weather.METEORS, com.oddlabs.tt.landscape.Weather.WILDFIRE, com.oddlabs.tt.landscape.Weather.BLIZZARD };
        }
        public GodPowers() : base()
        {
        }
    }
}
