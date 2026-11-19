// Translated from Afflictions.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 13
    public class Afflictions : java.lang.Object
    {
        public const float PERIOD = 1.5f;
        public const float FIRE_SPREAD = 0.4f;
        public const float POISON_SPREAD = 0.3f;
        public const float SPREAD_KEEP = 0.6f;
        public const float SPREAD_REACH = 2.5f;
        public const float OCCUPANT_CHOKE = 0.3f;
        public float poison_time;
        public float burn_time;
        public float slow_time;
        public float poison_tick;
        public float burn_tick;
        public int burn_damage;
        public com.oddlabs.tt.player.Player poison_by;
        public com.oddlabs.tt.player.Player burn_by;
        // Java: 32
        public void poison(float seconds, com.oddlabs.tt.player.Player by)
        {
            if (seconds > poison_time)
            {
                poison_time = seconds;
            }
            poison_by = by;
        }
        // Java: 38
        public void burn(float seconds, int damage_per_tick, com.oddlabs.tt.player.Player by)
        {
            if (seconds > burn_time)
            {
                burn_time = seconds;
            }
            burn_damage = java.lang.Math.max(1, damage_per_tick);
            burn_by = by;
        }
        // Java: 45
        public void slow(float seconds)
        {
            if (seconds > slow_time)
            {
                slow_time = seconds;
            }
        }
        // Java: 50
        public bool isPoisoned()
        {
            return poison_time > 0.0f;
        }
        // Java: 54
        public bool isBurning()
        {
            return burn_time > 0.0f;
        }
        // Java: 58
        public bool isSlowed()
        {
            return slow_time > 0.0f;
        }
        // Java: 62
        public float speedMultiplier()
        {
            return (slow_time > 0.0f ? 0.7f : 1.0f);
        }
        // Java: 66
        public void cure()
        {
            poison_time = 0.0f;
            burn_time = 0.0f;
        }
        // Java: 72
        public void animate(float t, com.oddlabs.tt.model.Selectable target)
        {
            if (slow_time > 0.0f)
            {
                slow_time -= t;
            }
            if (poison_time > 0.0f)
            {
                poison_time -= t;
                poison_tick += t;
                if (poison_tick >= com.oddlabs.tt.model.Afflictions.PERIOD)
                {
                    poison_tick -= com.oddlabs.tt.model.Afflictions.PERIOD;
                    if (!target.isDead() && poison_by != null)
                    {
                        target.hit(1, 0.0f, 0.0f, poison_by);
                        spread(target, false);
                    }
                }
            }
            else
            {
                poison_tick = 0.0f;
            }
            if (burn_time > 0.0f && !target.isDead())
            {
                burn_time -= t;
                burn_tick += t;
                if (burn_tick >= com.oddlabs.tt.model.Afflictions.PERIOD)
                {
                    burn_tick -= com.oddlabs.tt.model.Afflictions.PERIOD;
                    if (!target.isDead() && burn_by != null)
                    {
                        target.hit(burn_damage, 0.0f, 0.0f, burn_by);
                        spread(target, true);
                    }
                }
            }
            else
            {
                burn_tick = 0.0f;
            }
        }
        // Java: 102
        public void spread(com.oddlabs.tt.model.Selectable target, bool fire)
        {
            com.oddlabs.tt.player.Player by = (fire ? burn_by : poison_by);
            if (by == null || target.isDead())
            {
                return;
            }
            com.oddlabs.tt.landscape.World world = by.getWorld();
            if ((target is com.oddlabs.tt.model.LandBuilding b) && world.getRandom().nextFloat() < com.oddlabs.tt.model.Afflictions.OCCUPANT_CHOKE)
            {
                com.oddlabs.tt.model.ExtendedCombat.killOccupants(b, (by.isEnemy(b.getOwner()) ? by : (com.oddlabs.tt.player.Player)(null)), 1.0f, 1);
            }
            float left = (float)(((fire ? burn_time : poison_time)) * com.oddlabs.tt.model.Afflictions.SPREAD_KEEP);
            if (left < com.oddlabs.tt.model.Afflictions.PERIOD || world.getRandom().nextFloat() >= ((fire ? com.oddlabs.tt.model.Afflictions.FIRE_SPREAD : com.oddlabs.tt.model.Afflictions.POISON_SPREAD)))
            {
                return;
            }
            float reach = (float)(target.getSize() + com.oddlabs.tt.model.Afflictions.SPREAD_REACH);
            com.oddlabs.tt.pathfinder.FindOccupantFilter units = new com.oddlabs.tt.pathfinder.FindOccupantFilter(target.getPositionX(), target.getPositionY(), reach, target, typeof(com.oddlabs.tt.model.Unit));
            world.getUnitGrid().scan(units, target.getGridX(), target.getGridY());
            com.oddlabs.tt.pathfinder.FindOccupantFilter buildings = new com.oddlabs.tt.pathfinder.FindOccupantFilter(target.getPositionX(), target.getPositionY(), (float)(reach + 3.0f), target, typeof(com.oddlabs.tt.model.LandBuilding));
            world.getUnitGrid().scan(buildings, target.getGridX(), target.getGridY());
            java.util.List near = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(units.getResult()))
            {
                near.add(u);
            }
            foreach (com.oddlabs.tt.model.LandBuilding lb in java.lang.JIter.of(buildings.getResult()))
            {
                near.add(lb);
            }
            com.oddlabs.tt.model.Selectable best = null;
            float best_d = java.lang.Float.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(near))
            {
                if (((object)s == (object)target) || s.isDead() || !by.isEnemy(s.getOwner()))
                {
                    continue;
                }
                if ((s is com.oddlabs.tt.model.Unit u_2) && (u_2.isMounted() || ((fire ? u_2.isBurning() : u_2.isPoisoned()))))
                {
                    continue;
                }
                if ((s is com.oddlabs.tt.model.LandBuilding lb_2) && ((fire ? lb_2.afflictions().isBurning() : lb_2.afflictions().isPoisoned())))
                {
                    continue;
                }
                if (!((s is com.oddlabs.tt.model.Unit)) && !((s is com.oddlabs.tt.model.LandBuilding)))
                {
                    continue;
                }
                float dx = (float)(s.getPositionX() - target.getPositionX());
                float dy = (float)(s.getPositionY() - target.getPositionY());
                float d = (float)((float)(dx * dx) + (float)(dy * dy));
                if (d < best_d)
                {
                    best_d = d;
                    best = s;
                }
            }
            if (best == null)
            {
                return;
            }
            com.oddlabs.tt.model.Afflictions a = ((best is com.oddlabs.tt.model.Unit u_3) ? u_3.afflictions() : (((com.oddlabs.tt.model.LandBuilding)best)).afflictions());
            if (fire)
            {
                a.burn(left, burn_damage, by);
            }
            else
            {
                a.poison(left, by);
            }
        }
        private void init_Afflictions()
        {
            this.burn_damage = 1;
        }
        public Afflictions() : base()
        {
            init_Afflictions();
        }
    }
}
