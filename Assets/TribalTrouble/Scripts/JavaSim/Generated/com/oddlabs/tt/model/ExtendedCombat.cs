// Translated from ExtendedCombat.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 16
    public class ExtendedCombat : java.lang.Object
    {
        public const int NONE = 0;
        public const int UNIQUE_1 = 1;
        public const int UNIQUE_2 = 2;
        public const int UNIQUE_3 = 3;
        public const int CRYSTAL = 4;
        public const int DIAMOND = 5;
        public const float SHARD_RADIUS = 2.5f;
        public const float SHIELD_BLOCK = 0.4f;
        public const float REFLECT_RANGE = 30.0f;
        // Java: 31
        public static bool isVikings(com.oddlabs.tt.player.Player p)
        {
            return ((object)p.getRace() == (object)p.getWorld().getRacesResources().getRace(com.oddlabs.tt.model.RacesResources.RACE_VIKINGS));
        }
        // Java: 36
        public static int uniqueKind(com.oddlabs.tt.model.Unit u)
        {
            com.oddlabs.tt.model.Race race = u.getOwner().getRace();
            com.oddlabs.tt.model.UnitTemplate t = ((com.oddlabs.tt.model.UnitTemplate)u.getTemplate());
            if (((object)t == (object)race.getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_UNIQUE_1)))
            {
                return com.oddlabs.tt.model.ExtendedCombat.UNIQUE_1;
            }
            if (((object)t == (object)race.getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_UNIQUE_2)))
            {
                return com.oddlabs.tt.model.ExtendedCombat.UNIQUE_2;
            }
            if (((object)t == (object)race.getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_UNIQUE_3)))
            {
                return com.oddlabs.tt.model.ExtendedCombat.UNIQUE_3;
            }
            if (((object)t == (object)race.getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_CRYSTAL)))
            {
                return com.oddlabs.tt.model.ExtendedCombat.CRYSTAL;
            }
            if (((object)t == (object)race.getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_DIAMOND)))
            {
                return com.oddlabs.tt.model.ExtendedCombat.DIAMOND;
            }
            return com.oddlabs.tt.model.ExtendedCombat.NONE;
        }
        // Java: 53
        public static int adjustDamage(com.oddlabs.tt.model.Unit src, int damage)
        {
            if (!src.getOwner().getWorld().isExtended() || damage <= 0)
            {
                return damage;
            }
            damage = java.lang.Math.max(1, damage + com.oddlabs.tt.model.NewTidesStats.damageDelta(src.newTidesRow(), ((com.oddlabs.tt.model.UnitTemplate)src.getTemplate())));
            if (com.oddlabs.tt.model.ExtendedCombat.isVikings(src.getOwner()) && com.oddlabs.tt.model.ExtendedCombat.uniqueKind(src) == com.oddlabs.tt.model.ExtendedCombat.UNIQUE_2 && src.getHitPoints() * 2 <= src.getEffectiveMaxHitPoints())
            {
                damage += 1;
            }
            if (src.getGodKind() != com.oddlabs.tt.model.GodPowers.NONE)
            {
                damage += com.oddlabs.tt.model.GodPowers.bonusDamage(src);
            }
            damage += com.oddlabs.tt.model.ElitePowers.bonusDamage(src);
            return com.oddlabs.tt.model.ExtendedCombat.smith(src, damage);
        }
        // Java: 67
        public static int adjustDamage(com.oddlabs.tt.model.Unit src, com.oddlabs.tt.model.Selectable target, int damage)
        {
            damage = com.oddlabs.tt.model.ExtendedCombat.adjustDamage(src, damage);
            if (damage > 0 && src.getOwner().getWorld().isExtended() && (target is com.oddlabs.tt.model.Building) && com.oddlabs.tt.model.ExtendedCombat.uniqueKind(src) == com.oddlabs.tt.model.ExtendedCombat.DIAMOND)
            {
                damage += 2;
            }
            if (damage > 0 && src.getOwner().getWorld().isExtended())
            {
                damage = com.oddlabs.tt.model.ExtendedCombat.edge(src, target, damage);
            }
            return com.oddlabs.tt.model.Counters.apply(src, target, damage);
        }
        public const float ARC_REACH = 2.5f;
        public const float ARC_CHANCE = 0.75f;
        // Java: 86
        public static java.util.List chainLightning(com.oddlabs.tt.landscape.World world, com.oddlabs.tt.player.Player by, com.oddlabs.tt.model.Unit first, int damage, int jumps)
        {
            java.util.List arcs = new java.util.ArrayList();
            if (!world.isExtended())
            {
                return arcs;
            }
            java.util.List hit = new java.util.ArrayList();
            hit.add(first);
            com.oddlabs.tt.model.Unit from = first;
            float chance = com.oddlabs.tt.model.ExtendedCombat.ARC_CHANCE;
            for (int j = 0; j < jumps; j++)
            {
                if (world.getRandom().nextFloat() >= chance)
                {
                    break;
                }
                com.oddlabs.tt.pathfinder.FindOccupantFilter filter = new com.oddlabs.tt.pathfinder.FindOccupantFilter(from.getPositionX(), from.getPositionY(), (float)(from.getSize() + com.oddlabs.tt.model.ExtendedCombat.ARC_REACH), from, typeof(com.oddlabs.tt.model.Unit));
                world.getUnitGrid().scan(filter, from.getGridX(), from.getGridY());
                com.oddlabs.tt.model.Unit best = null;
                float best_d = java.lang.Float.MAX_VALUE;
                foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(filter.getResult()))
                {
                    if (u.isDead() || u.isMounted() || hit.contains(u) || (by != null && !by.isEnemy(u.getOwner())))
                    {
                        continue;
                    }
                    float dx = (float)(u.getPositionX() - from.getPositionX());
                    float dy = (float)(u.getPositionY() - from.getPositionY());
                    float d = (float)((float)(dx * dx) + (float)(dy * dy));
                    if (d < best_d)
                    {
                        best_d = d;
                        best = u;
                    }
                }
                if (best == null)
                {
                    break;
                }
                damage = java.lang.Math.max(1, damage / 2);
                float len = ((float)(java.lang.Math.sqrt(best_d)));
                arcs.add(new com.oddlabs.tt.model.Unit[] { from, best });
                best.hit(damage, (len > 0.0f ? (float)(((float)(best.getPositionX() - from.getPositionX())) / len) : 0.0f), (len > 0.0f ? (float)(((float)(best.getPositionY() - from.getPositionY())) / len) : 1.0f), (by != null ? by : best.getOwner()));
                hit.add(best);
                from = best;
                chance = (float)(chance * 0.85f);
            }
            return arcs;
        }
        // Java: 128
        public static int magicDamage(com.oddlabs.tt.player.Player caster, com.oddlabs.tt.model.Selectable target, int damage)
        {
            if (!caster.getWorld().isExtended())
            {
                return damage;
            }
            if (caster.isEnemy(target.getOwner()))
            {
                return java.lang.Math.round((float)(damage * ((float)(1.0f + (float)(0.15f * caster.getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.ARCANE_POWER))))));
            }
            return java.lang.Math.round((float)(damage * 0.25f));
        }
        // Java: 137
        public static int edge(com.oddlabs.tt.model.Unit src, com.oddlabs.tt.model.Selectable target, int damage)
        {
            com.oddlabs.tt.player.Player p = src.getOwner();
            int kind = com.oddlabs.tt.model.ExtendedCombat.uniqueKind(src);
            System.Type type = src.getWeaponFactory().getType();
            if (kind == com.oddlabs.tt.model.ExtendedCombat.DIAMOND || src.getGodKind() != com.oddlabs.tt.model.GodPowers.NONE)
            {
                int lvl = p.getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.DIAMOND_EDGE);
                if (kind == com.oddlabs.tt.model.ExtendedCombat.DIAMOND || lvl > 0)
                {
                    damage += (kind == com.oddlabs.tt.model.ExtendedCombat.DIAMOND ? lvl : lvl / 2);
                }
                return damage;
            }
            if (kind == com.oddlabs.tt.model.ExtendedCombat.CRYSTAL)
            {
                return damage;
            }
            if (((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)) || ((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RockSpearWeapon)))
            {
                int lvl_2 = p.getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.ROCK_EDGE);
                if (lvl_2 >= 2 || (lvl_2 == 1 && src.nextSmithHit() % 2 == 0))
                {
                    damage += 1;
                }
                if (lvl_2 >= 3 && src.nextSmithHit() % 2 == 0)
                {
                    damage += 1;
                }
            }
            else if ((((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)) || ((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RubberSpearWeapon))) && p.getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.CHICKEN_BOUNCE) >= 3)
            {
                damage += 1;
            }
            return damage;
        }
        // Java: 162
        public static int adjustThrown(com.oddlabs.tt.model.Unit src, com.oddlabs.tt.model.Selectable target, int damage)
        {
            damage = com.oddlabs.tt.model.ExtendedCombat.adjustThrown(src, damage);
            if (damage <= 0 || !src.getOwner().getWorld().isExtended())
            {
                return damage;
            }
            return com.oddlabs.tt.model.Counters.apply(src, target, com.oddlabs.tt.model.ExtendedCombat.edge(src, target, damage));
        }
        // Java: 170
        public static int adjustThrown(com.oddlabs.tt.model.Unit src, int damage)
        {
            if (!src.getOwner().getWorld().isExtended() || damage <= 0)
            {
                return damage;
            }
            damage = java.lang.Math.max(1, damage + com.oddlabs.tt.model.NewTidesStats.damageDelta(src.newTidesRow(), ((com.oddlabs.tt.model.UnitTemplate)src.getTemplate())));
            if (src.getGodKind() != com.oddlabs.tt.model.GodPowers.NONE)
            {
                damage += com.oddlabs.tt.model.GodPowers.bonusDamage(src);
            }
            damage += com.oddlabs.tt.model.ElitePowers.bonusDamage(src);
            return com.oddlabs.tt.model.ExtendedCombat.smith(src, damage);
        }
        // Java: 181
        public static int smith(com.oddlabs.tt.model.Unit src, int damage)
        {
            int level = src.getOwner().getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.SMITHING);
            if (level == 0 || !src.isWarrior())
            {
                return damage;
            }
            if (level >= 3)
            {
                return damage + 1;
            }
            return (src.nextSmithHit() % ((level >= 2 ? 2 : 3)) == 0 ? damage + 1 : damage);
        }
        // Java: 191
        public static bool stakes(com.oddlabs.tt.model.LandBuilding lb)
        {
            if (((com.oddlabs.tt.model.BuildingTemplate)lb.getTemplate()).getTemplateID() != com.oddlabs.tt.model.Race.BUILDING_PALISADE)
            {
                return false;
            }
            int level = lb.getOwner().getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.STAKES);
            return level >= 2 || (level == 1 && lb.nextReflect());
        }
        // Java: 199
        public static void afterHit(com.oddlabs.tt.model.Unit src, com.oddlabs.tt.model.Selectable target, int damage)
        {
            com.oddlabs.tt.landscape.World world = src.getOwner().getWorld();
            if (!world.isExtended() || damage <= 0)
            {
                return;
            }
            com.oddlabs.tt.player.Player owner = src.getOwner();
            if ((target is com.oddlabs.tt.model.LandBuilding lb) && !src.isDead() && owner.isEnemy(lb.getOwner()) && ((lb.hasChickenBarbs() ? lb.nextReflect() : com.oddlabs.tt.model.ExtendedCombat.stakes(lb))))
            {
                float dx = (float)(src.getPositionX() - lb.getPositionX());
                float dy = (float)(src.getPositionY() - lb.getPositionY());
                if ((float)((float)(dx * dx) + (float)(dy * dy)) <= (float)(com.oddlabs.tt.model.ExtendedCombat.REFLECT_RANGE * com.oddlabs.tt.model.ExtendedCombat.REFLECT_RANGE))
                {
                    float len = ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy)))));
                    float inv = (len > 0.0f ? (float)(1.0f / len) : 0.0f);
                    src.hit(java.lang.Math.max(1, damage / 2), (float)(dx * inv), (float)(dy * inv), lb.getOwner());
                }
            }
            if (src.getEliteKind() != com.oddlabs.tt.model.ElitePowers.NONE)
            {
                com.oddlabs.tt.model.ElitePowers.afterHit(src, target, damage);
            }
            if (target.isDead())
            {
                if ((target is com.oddlabs.tt.model.Unit) && src.getGodKind() != com.oddlabs.tt.model.GodPowers.NONE)
                {
                    com.oddlabs.tt.model.GodPowers.killed(src);
                }
                return;
            }
            int kind = com.oddlabs.tt.model.ExtendedCombat.uniqueKind(src);
            bool thrower = src.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.THROW) && src.isWarrior();
            if (com.oddlabs.tt.model.ExtendedCombat.isVikings(owner))
            {
                int fire_axes = owner.getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.RACE_1);
                if (kind == com.oddlabs.tt.model.ExtendedCombat.UNIQUE_1 || (thrower && fire_axes > 0))
                {
                    float seconds = (kind == com.oddlabs.tt.model.ExtendedCombat.UNIQUE_1 ? 4.5f : (float)(1.5f + (float)(1.5f * fire_axes)));
                    if ((target is com.oddlabs.tt.model.Unit u))
                    {
                        u.afflictions().burn(seconds, 1, owner);
                    }
                    else if ((target is com.oddlabs.tt.model.LandBuilding b))
                    {
                        b.afflictions().burn((float)(seconds * 2.0f), (kind == com.oddlabs.tt.model.ExtendedCombat.UNIQUE_1 ? 3 : 2), owner);
                    }
                }
            }
            else
            {
                int poison_tips = owner.getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.RACE_1);
                if ((target is com.oddlabs.tt.model.Unit u_2) && (kind == com.oddlabs.tt.model.ExtendedCombat.UNIQUE_1 || (thrower && poison_tips > 0)))
                {
                    u_2.afflictions().poison((kind == com.oddlabs.tt.model.ExtendedCombat.UNIQUE_1 ? 6.0f : (float)(1.5f + (float)(1.5f * poison_tips))), owner);
                }
                if ((target is com.oddlabs.tt.model.Unit u_3) && kind == com.oddlabs.tt.model.ExtendedCombat.UNIQUE_3)
                {
                    u_3.afflictions().poison(4.0f, owner);
                    u_3.afflictions().slow(3.0f);
                }
            }
            if (kind == com.oddlabs.tt.model.ExtendedCombat.CRYSTAL && (target is com.oddlabs.tt.model.Unit))
            {
                com.oddlabs.tt.model.ExtendedCombat.shatter(src, target);
            }
        }
        // Java: 246
        public static void shatter(com.oddlabs.tt.model.Unit src, com.oddlabs.tt.model.Selectable target)
        {
            com.oddlabs.tt.pathfinder.FindOccupantFilter filter = new com.oddlabs.tt.pathfinder.FindOccupantFilter(target.getPositionX(), target.getPositionY(), com.oddlabs.tt.model.ExtendedCombat.SHARD_RADIUS, target, typeof(com.oddlabs.tt.model.Unit));
            src.getOwner().getWorld().getUnitGrid().scan(filter, target.getGridX(), target.getGridY());
            java.util.List near = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(filter.getResult()))
            {
                if (((object)u != (object)target) && !u.isDead() && !u.isMounted() && src.getOwner().isEnemy(u.getOwner()))
                {
                    near.add(u);
                }
            }
            near.sort(new java.util.Comparator_L((object a_l, object b_l) =>
            {
                com.oddlabs.tt.model.Unit a = (com.oddlabs.tt.model.Unit)a_l;
                com.oddlabs.tt.model.Unit b = (com.oddlabs.tt.model.Unit)b_l;
                float da = com.oddlabs.tt.model.ExtendedCombat.d2(a, target);
                float db = com.oddlabs.tt.model.ExtendedCombat.d2(b, target);
                if (da != db)
                {
                    return (da < db ? (-1) : 1);
                }
                return (a.getGridY() != b.getGridY() ? java.lang.Integer.compare(a.getGridY(), b.getGridY()) : java.lang.Integer.compare(a.getGridX(), b.getGridX()));
            }));
            int shards = 1 + src.getOwner().getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.CRYSTAL_FACET);
            for (int k = 0; k < shards && k < near.size(); k++)
            {
                com.oddlabs.tt.model.Unit u_2 = ((com.oddlabs.tt.model.Unit)near.get(k));
                float len = ((float)(java.lang.Math.sqrt(com.oddlabs.tt.model.ExtendedCombat.d2(u_2, target))));
                u_2.hit(1, (len > 0.0f ? (float)(((float)(u_2.getPositionX() - target.getPositionX())) / len) : 0.0f), (len > 0.0f ? (float)(((float)(u_2.getPositionY() - target.getPositionY())) / len) : 1.0f), src.getOwner());
            }
        }
        // Java: 270
        public static float d2(com.oddlabs.tt.model.Selectable a, com.oddlabs.tt.model.Selectable b)
        {
            float dx = (float)(a.getPositionX() - b.getPositionX());
            float dy = (float)(a.getPositionY() - b.getPositionY());
            return (float)((float)(dx * dx) + (float)(dy * dy));
        }
        // Java: 281
        public static int killOccupants(com.oddlabs.tt.model.Selectable target, com.oddlabs.tt.player.Player attacker, float chance, int max)
        {
            if (!((target is com.oddlabs.tt.model.LandBuilding b)) || b.isDead())
            {
                return 0;
            }
            if (attacker != null && (!attacker.getWorld().isExtended() || !attacker.isEnemy(b.getOwner())))
            {
                return 0;
            }
            com.oddlabs.tt.model.UnitContainer c = b.getUnitContainer();
            if (c == null)
            {
                return 0;
            }
            int killed = 0;
            int n = java.lang.Math.min(c.getNumSupplies(), max);
            for (int i = 0; i < n && c.getNumSupplies() > 0; i++)
            {
                if (b.getOwner().getWorld().getRandom().nextFloat() >= chance)
                {
                    continue;
                }
                com.oddlabs.tt.model.Unit u = c.exit();
                if (u != null)
                {
                    if (!u.isDead())
                    {
                        u.hit(u.getHitPoints(), 0.0f, 1.0f, (attacker != null ? attacker : u.getOwner()));
                    }
                }
                else
                {
                    b.getOwner().unitLost();
                    if (attacker != null)
                    {
                        attacker.unitKilled();
                    }
                }
                killed++;
            }
            return killed;
        }
        public const float CLOSE_RANGE = 2.0f;
        public const float SPEAR_FALLOFF = 0.3f;
        public const float AXE_FALLOFF = 0.4f;
        public const float BUILDING_FALLOFF = 0.5f;
        public const float MOVING_NEAR = 0.1f;
        public const float MOVING_FAR = 0.35f;
        public const float MOVING_MELEE = 0.1f;
        public const float WALK_SPEED = 4.0f;
        public const float HIGH_PER_METER = 0.025f;
        public const float HIGH_MAX = 0.3f;
        public const float LOW_PER_METER = 0.02f;
        public const float LOW_MAX = 0.2f;
        public const float TOWER_STEADY = 2.0f;
        public const float MIN_HIT = 0.05f;
        public const float MELEE_SURE = 1.3f;
        public const float MAX_HIT = 0.98f;
        // Java: 330
        public static float skill(com.oddlabs.tt.model.Unit u)
        {
            com.oddlabs.tt.model.Race race = u.getOwner().getRace();
            com.oddlabs.tt.model.UnitTemplate t = ((com.oddlabs.tt.model.UnitTemplate)u.getTemplate());
            if (((object)t == (object)race.getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_CHAMPION)))
            {
                return 1.1f;
            }
            if (((object)t == (object)race.getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_SCOUT)))
            {
                return 0.95f;
            }
            return 1.0f;
        }
        // Java: 340
        public static float steadiness(com.oddlabs.tt.model.Unit u)
        {
            com.oddlabs.tt.model.Race race = u.getOwner().getRace();
            com.oddlabs.tt.model.UnitTemplate t = ((com.oddlabs.tt.model.UnitTemplate)u.getTemplate());
            if (((object)t == (object)race.getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_SLINGER)))
            {
                return 0.6f;
            }
            if (com.oddlabs.tt.model.ExtendedCombat.uniqueKind(u) == com.oddlabs.tt.model.ExtendedCombat.UNIQUE_1 && !com.oddlabs.tt.model.ExtendedCombat.isVikings(u.getOwner()))
            {
                return 0.7f;
            }
            if (com.oddlabs.tt.model.ExtendedCombat.uniqueKind(u) == com.oddlabs.tt.model.ExtendedCombat.UNIQUE_3 && !com.oddlabs.tt.model.ExtendedCombat.isVikings(u.getOwner()))
            {
                return 0.55f;
            }
            if (((object)t == (object)race.getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_SCOUT)))
            {
                return 1.2f;
            }
            return 1.0f;
        }
        // Java: 354
        public static bool isAxe(System.Type type)
        {
            return ((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)) || ((object)type == (object)typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)) || ((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon));
        }
        // Java: 363
        public static float hitChance(com.oddlabs.tt.model.Unit src, com.oddlabs.tt.model.Selectable target, float factor, float accuracy, bool ranged, System.Type type)
        {
            com.oddlabs.tt.landscape.HeightMap hm = src.getOwner().getWorld().getHeightMap();
            if (src.isMounted())
            {
                factor = (float)((float)(factor / 3.0f) * com.oddlabs.tt.model.ExtendedCombat.TOWER_STEADY);
            }
            float acc = (float)((float)(accuracy * factor) * com.oddlabs.tt.model.ExtendedCombat.skill(src));
            if (src.getEliteKind() != com.oddlabs.tt.model.ElitePowers.NONE)
            {
                acc = (float)(acc * com.oddlabs.tt.model.ElitePowers.accuracy(src));
            }
            if (((object)type == (object)typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)) || ((object)type == (object)typeof(com.oddlabs.tt.model.weapon.IronSpearWeapon)))
            {
                acc = (float)(acc * (float)(1.0f + (float)(0.1f * src.getOwner().getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.IRON_TEMPER))));
            }
            float dx = (float)(target.getPositionX() - src.getPositionX());
            float dy = (float)(target.getPositionY() - src.getPositionY());
            float dist = ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy)))));
            float frac = 0.0f;
            if (ranged)
            {
                float reach = java.lang.Math.max((float)(src.getRange(target) - target.getSize()), (float)(com.oddlabs.tt.model.ExtendedCombat.CLOSE_RANGE + 1.0f));
                frac = java.lang.Math.clamp((float)(((float)((float)(dist - (float)(target.getSize() * 0.5f)) - com.oddlabs.tt.model.ExtendedCombat.CLOSE_RANGE)) / ((float)(reach - com.oddlabs.tt.model.ExtendedCombat.CLOSE_RANGE))), 0.0f, 1.25f);
                float falloff = (float)(((com.oddlabs.tt.model.ExtendedCombat.isAxe(type) ? com.oddlabs.tt.model.ExtendedCombat.AXE_FALLOFF : com.oddlabs.tt.model.ExtendedCombat.SPEAR_FALLOFF)) * com.oddlabs.tt.model.ExtendedCombat.steadiness(src));
                if ((target is com.oddlabs.tt.model.Building))
                {
                    falloff = (float)(falloff * com.oddlabs.tt.model.ExtendedCombat.BUILDING_FALLOFF);
                }
                acc = (float)(acc * (float)(1.0f - (float)(falloff * frac)));
            }
            if (!ranged)
            {
                acc = (float)(acc * com.oddlabs.tt.model.ExtendedCombat.MELEE_SURE);
            }
            if ((target is com.oddlabs.tt.model.Unit u) && u.isMoving())
            {
                float speed = java.lang.Math.min((float)(u.getMetersPerSecond() / com.oddlabs.tt.model.ExtendedCombat.WALK_SPEED), 1.6f);
                float dodge = (ranged ? (float)(com.oddlabs.tt.model.ExtendedCombat.MOVING_NEAR + (float)(((float)(com.oddlabs.tt.model.ExtendedCombat.MOVING_FAR - com.oddlabs.tt.model.ExtendedCombat.MOVING_NEAR)) * java.lang.Math.min(frac, 1.0f))) : com.oddlabs.tt.model.ExtendedCombat.MOVING_MELEE);
                acc = (float)(acc * (float)(1.0f - (float)(dodge * speed)));
            }
            float src_z = (float)(hm.getNearestHeight(src.getPositionX(), src.getPositionY()) + src.getMountOffset());
            float dst_z = (float)(hm.getNearestHeight(target.getPositionX(), target.getPositionY()) + (((target is com.oddlabs.tt.model.Unit u_2) ? u_2.getMountOffset() : 0.0f)));
            float dz = (float)(src_z - dst_z);
            float height = (dz >= 0.0f ? java.lang.Math.min((float)(dz * com.oddlabs.tt.model.ExtendedCombat.HIGH_PER_METER), com.oddlabs.tt.model.ExtendedCombat.HIGH_MAX) : java.lang.Math.max((float)(dz * com.oddlabs.tt.model.ExtendedCombat.LOW_PER_METER), (float)(-com.oddlabs.tt.model.ExtendedCombat.LOW_MAX)));
            acc = (float)(acc * (float)(1.0f + height));
            acc = (float)(acc * (float)(1.0f - target.getDefenseChance()));
            if (ranged && (target is com.oddlabs.tt.model.Unit tu) && com.oddlabs.tt.model.ExtendedCombat.uniqueKind(tu) == com.oddlabs.tt.model.ExtendedCombat.UNIQUE_3 && com.oddlabs.tt.model.ExtendedCombat.isVikings(tu.getOwner()))
            {
                acc = (float)(acc * (float)(1.0f - com.oddlabs.tt.model.ExtendedCombat.SHIELD_BLOCK));
            }
            if (ranged && src.getOwner().getWorld().getWeather() != null)
            {
                acc = (float)(acc * src.getOwner().getWorld().getWeather().getAccuracyFactor());
            }
            return java.lang.Math.clamp(acc, com.oddlabs.tt.model.ExtendedCombat.MIN_HIT, com.oddlabs.tt.model.ExtendedCombat.MAX_HIT);
        }
        public ExtendedCombat() : base()
        {
        }
    }
}
