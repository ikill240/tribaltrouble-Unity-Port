// Translated from WorldModel.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.ultra
{
    // Java: 42
    public class WorldModel : java.lang.Object
    {
        // Java: 43
        public sealed class Kind : java.lang.Enum
        {
            public static Kind[] values() { return (Kind[])VALUES_.Clone(); }
            public static Kind valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly Kind PEON = new Kind("PEON", 0);
            public static readonly Kind ROCK = new Kind("ROCK", 1);
            public static readonly Kind IRON = new Kind("IRON", 2);
            public static readonly Kind RUBBER = new Kind("RUBBER", 3);
            public static readonly Kind CHIEFTAIN = new Kind("CHIEFTAIN", 4);
            public static readonly Kind[] VALUES_ = new Kind[] { PEON, ROCK, IRON, RUBBER, CHIEFTAIN };
            public Kind(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        // Java: 51
        public sealed class Activity : java.lang.Enum
        {
            public static Activity[] values() { return (Activity[])VALUES_.Clone(); }
            public static Activity valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly Activity IDLE = new Activity("IDLE", 0);
            public static readonly Activity GATHER = new Activity("GATHER", 1);
            public static readonly Activity BUILD = new Activity("BUILD", 2);
            public static readonly Activity PLACE = new Activity("PLACE", 3);
            public static readonly Activity WALK = new Activity("WALK", 4);
            public static readonly Activity ATTACK_MOVE = new Activity("ATTACK_MOVE", 5);
            public static readonly Activity HUNT = new Activity("HUNT", 6);
            public static readonly Activity ENTER = new Activity("ENTER", 7);
            public static readonly Activity STUNNED = new Activity("STUNNED", 8);
            public static readonly Activity MAGIC = new Activity("MAGIC", 9);
            public static readonly Activity OTHER = new Activity("OTHER", 10);
            public static readonly Activity[] VALUES_ = new Activity[] { IDLE, GATHER, BUILD, PLACE, WALK, ATTACK_MOVE, HUNT, ENTER, STUNNED, MAGIC, OTHER };
            public Activity(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        // Java: 66
        public class Side : java.lang.Object
        {
            public java.util.List peons;
            public java.util.List warriors;
            public java.util.List units;
            public com.oddlabs.tt.model.Unit chieftain;
            public java.util.List chieftains;
            public java.util.List quarters;
            public java.util.List armories;
            public java.util.List towers;
            public java.util.List sites;
            public java.util.List buildings;
            public int rock_warriors;
            public int iron_warriors;
            public int rubber_warriors;
            public int garrisoned;
            public int peons_inside;
            public int armory_workers;
            public int weapons_rock;
            public int weapons_iron;
            public int weapons_rubber;
            public int unit_count;
            // Java: 90
            public void clear()
            {
                peons.clear();
                warriors.clear();
                units.clear();
                chieftain = null;
                chieftains.clear();
                quarters.clear();
                armories.clear();
                towers.clear();
                sites.clear();
                buildings.clear();
                rock_warriors = iron_warriors = rubber_warriors = 0;
                garrisoned = peons_inside = armory_workers = 0;
                weapons_rock = weapons_iron = weapons_rubber = 0;
                unit_count = 0;
            }
            // Java: 107
            public int fieldWarriors()
            {
                return warriors.size();
            }
            // Java: 115
            public float latentStrength()
            {
                int workers = armory_workers;
                int rub = java.lang.Math.min(workers, weapons_rubber);
                workers -= rub;
                int iron = java.lang.Math.min(workers, weapons_iron);
                workers -= iron;
                int rock = java.lang.Math.min(workers, weapons_rock);
                return (float)((float)((float)(1.35f * rub) + iron) + (float)(0.63f * rock));
            }
            // Java: 125
            public int potentialWarriors()
            {
                return warriors.size() + garrisoned + java.lang.Math.min(peons_inside, weapons_rock + weapons_iron + weapons_rubber);
            }
            private void init_Side()
            {
                this.peons = new java.util.ArrayList();
                this.warriors = new java.util.ArrayList();
                this.units = new java.util.ArrayList();
                this.chieftains = new java.util.ArrayList();
                this.quarters = new java.util.ArrayList();
                this.armories = new java.util.ArrayList();
                this.towers = new java.util.ArrayList();
                this.sites = new java.util.ArrayList();
                this.buildings = new java.util.ArrayList();
            }
            public Side() : base()
            {
                init_Side();
            }
        }
        public com.oddlabs.tt.player.ultra.WorldModel.Side me;
        public com.oddlabs.tt.player.ultra.WorldModel.Side enemy;
        public com.oddlabs.tt.player.Player owner;
        public int version;
        // Java: 141
        public void update()
        {
            version++;
            me.clear();
            com.oddlabs.tt.player.ultra.WorldModel.fill(me, owner, true);
            enemy.clear();
            foreach (com.oddlabs.tt.player.Player p in owner.getWorld().getPlayers())
            {
                if (owner.isEnemy(p))
                {
                    com.oddlabs.tt.player.ultra.WorldModel.fill(enemy, p, false);
                }
            }
        }
        // Java: 156
        public static void fill(com.oddlabs.tt.player.ultra.WorldModel.Side side, com.oddlabs.tt.player.Player p, bool own)
        {
            if (own)
            {
                side.unit_count += p.getUnitCountContainer().getNumSupplies();
            }
            com.oddlabs.tt.model.Selectable[] all = ((com.oddlabs.tt.model.Selectable[])p.getUnits().getSet().toArray(new com.oddlabs.tt.model.Selectable[0]));
            foreach (com.oddlabs.tt.model.Selectable s in all)
            {
                if (s.isDead())
                {
                    continue;
                }
                if ((s is com.oddlabs.tt.model.Unit u))
                {
                    if (u.isMounted())
                    {
                        continue;
                    }
                    com.oddlabs.tt.player.ultra.WorldModel.Kind kind = com.oddlabs.tt.player.ultra.WorldModel.kindOf(u);
                    switch ((kind).ordinal())
                    {
                        case 0 /*PEON*/:
                        {
                            side.peons.add(u);
                            break;
                        }
                        case 4 /*CHIEFTAIN*/:
                        {
                            side.chieftain = u;
                            side.chieftains.add(u);
                            break;
                        }
                        case 1 /*ROCK*/:
                        {
                            side.warriors.add(u);
                            side.rock_warriors++;
                            break;
                        }
                        case 2 /*IRON*/:
                        {
                            side.warriors.add(u);
                            side.iron_warriors++;
                            break;
                        }
                        case 3 /*RUBBER*/:
                        {
                            side.warriors.add(u);
                            side.rubber_warriors++;
                            break;
                        }
                    }
                    side.units.add(u);
                }
                else if ((s is com.oddlabs.tt.model.Building b))
                {
                    side.buildings.add(b);
                    if (!b.isComplete())
                    {
                        if (b.isPlaced())
                        {
                            side.sites.add(b);
                        }
                        continue;
                    }
                    int id = ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID();
                    if (id == com.oddlabs.tt.model.Race.BUILDING_QUARTERS)
                    {
                        side.quarters.add(b);
                        if (own)
                        {
                            side.peons_inside += b.getUnitContainer().getNumSupplies();
                        }
                    }
                    else if (id == com.oddlabs.tt.model.Race.BUILDING_ARMORY)
                    {
                        side.armories.add(b);
                        if (!own)
                        {
                            continue;
                        }
                        side.peons_inside += b.getUnitContainer().getNumSupplies();
                        side.armory_workers += b.getUnitContainer().getNumSupplies();
                        side.weapons_rock += b.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)).getNumSupplies();
                        side.weapons_iron += b.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)).getNumSupplies();
                        side.weapons_rubber += b.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)).getNumSupplies();
                    }
                    else if (id == com.oddlabs.tt.model.Race.BUILDING_TOWER)
                    {
                        side.towers.add(b);
                        com.oddlabs.tt.model.UnitContainer c = b.getUnitContainer();
                        if (c != null && c.getNumSupplies() > 0)
                        {
                            side.garrisoned++;
                        }
                    }
                }
            }
        }
        // Java: 221
        public static com.oddlabs.tt.player.ultra.WorldModel.Kind kindOf(com.oddlabs.tt.model.Unit u)
        {
            com.oddlabs.tt.model.Abilities a = u.getAbilities();
            if (a.hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC))
            {
                return com.oddlabs.tt.player.ultra.WorldModel.Kind.CHIEFTAIN;
            }
            if (a.hasAbilities(com.oddlabs.tt.model.Abilities.BUILD))
            {
                return com.oddlabs.tt.player.ultra.WorldModel.Kind.PEON;
            }
            com.oddlabs.tt.model.Race race = u.getOwner().getRace();
            if (((object)((com.oddlabs.tt.model.UnitTemplate)u.getTemplate()) == (object)race.getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER)))
            {
                return com.oddlabs.tt.player.ultra.WorldModel.Kind.RUBBER;
            }
            if (((object)((com.oddlabs.tt.model.UnitTemplate)u.getTemplate()) == (object)race.getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON)))
            {
                return com.oddlabs.tt.player.ultra.WorldModel.Kind.IRON;
            }
            return com.oddlabs.tt.player.ultra.WorldModel.Kind.ROCK;
        }
        // Java: 236
        public static com.oddlabs.tt.model.behaviour.Controller primary(com.oddlabs.tt.model.Unit u)
        {
            return (u.isDead() ? (com.oddlabs.tt.model.behaviour.Controller)(null) : u.getPrimaryController());
        }
        // Java: 240
        public static com.oddlabs.tt.player.ultra.WorldModel.Activity activityOf(com.oddlabs.tt.model.Unit u)
        {
            if (u.isDead())
            {
                return com.oddlabs.tt.player.ultra.WorldModel.Activity.OTHER;
            }
            com.oddlabs.tt.model.behaviour.Controller current = u.getCurrentController();
            if ((current is com.oddlabs.tt.model.behaviour.StunController))
            {
                return com.oddlabs.tt.player.ultra.WorldModel.Activity.STUNNED;
            }
            if ((current is com.oddlabs.tt.model.behaviour.MagicController))
            {
                return com.oddlabs.tt.player.ultra.WorldModel.Activity.MAGIC;
            }
            com.oddlabs.tt.model.behaviour.Controller primary_v = u.getPrimaryController();
            if ((primary_v is com.oddlabs.tt.model.behaviour.IdleController))
            {
                return ((current is com.oddlabs.tt.model.behaviour.HuntController) ? com.oddlabs.tt.player.ultra.WorldModel.Activity.HUNT : com.oddlabs.tt.player.ultra.WorldModel.Activity.IDLE);
            }
            if ((primary_v is com.oddlabs.tt.model.behaviour.GatherController))
            {
                return com.oddlabs.tt.player.ultra.WorldModel.Activity.GATHER;
            }
            if ((primary_v is com.oddlabs.tt.model.behaviour.RepairController))
            {
                return com.oddlabs.tt.player.ultra.WorldModel.Activity.BUILD;
            }
            if ((primary_v is com.oddlabs.tt.model.behaviour.PlaceBuildingController))
            {
                return com.oddlabs.tt.player.ultra.WorldModel.Activity.PLACE;
            }
            if ((primary_v is com.oddlabs.tt.model.behaviour.HuntController) || (primary_v is com.oddlabs.tt.model.behaviour.AttackController))
            {
                return com.oddlabs.tt.player.ultra.WorldModel.Activity.HUNT;
            }
            if ((primary_v is com.oddlabs.tt.model.behaviour.WalkController w))
            {
                return ((current is com.oddlabs.tt.model.behaviour.HuntController) ? com.oddlabs.tt.player.ultra.WorldModel.Activity.HUNT : (w.isAgressive() ? com.oddlabs.tt.player.ultra.WorldModel.Activity.ATTACK_MOVE : com.oddlabs.tt.player.ultra.WorldModel.Activity.WALK));
            }
            if ((primary_v is com.oddlabs.tt.model.behaviour.DefendController))
            {
                return ((current is com.oddlabs.tt.model.behaviour.HuntController) ? com.oddlabs.tt.player.ultra.WorldModel.Activity.HUNT : com.oddlabs.tt.player.ultra.WorldModel.Activity.ATTACK_MOVE);
            }
            if ((primary_v is com.oddlabs.tt.model.behaviour.EnterController) || (primary_v is com.oddlabs.tt.model.behaviour.TransferUnitController))
            {
                return com.oddlabs.tt.player.ultra.WorldModel.Activity.ENTER;
            }
            return com.oddlabs.tt.player.ultra.WorldModel.Activity.OTHER;
        }
        // Java: 272
        public static com.oddlabs.tt.player.ultra.WorldModel.Activity visibleActivity(com.oddlabs.tt.model.Unit u)
        {
            if (u.isDead())
            {
                return com.oddlabs.tt.player.ultra.WorldModel.Activity.OTHER;
            }
            com.oddlabs.tt.model.behaviour.Behaviour b = u.getCurrentBehaviour();
            if ((b is com.oddlabs.tt.model.behaviour.StunBehaviour))
            {
                return com.oddlabs.tt.player.ultra.WorldModel.Activity.STUNNED;
            }
            if ((b is com.oddlabs.tt.model.behaviour.MagicBehaviour))
            {
                return com.oddlabs.tt.player.ultra.WorldModel.Activity.MAGIC;
            }
            if ((b is com.oddlabs.tt.model.behaviour.HarvestBehaviour))
            {
                return com.oddlabs.tt.player.ultra.WorldModel.Activity.GATHER;
            }
            if ((b is com.oddlabs.tt.model.behaviour.RepairBehaviour))
            {
                return com.oddlabs.tt.player.ultra.WorldModel.Activity.BUILD;
            }
            if ((b is com.oddlabs.tt.model.behaviour.AttackBehaviour))
            {
                return com.oddlabs.tt.player.ultra.WorldModel.Activity.HUNT;
            }
            if ((b is com.oddlabs.tt.model.behaviour.WalkBehaviour))
            {
                com.oddlabs.tt.model.UnitSupplyContainer c = u.getSupplyContainer();
                return (c != null && c.getNumSupplies() > 0 ? com.oddlabs.tt.player.ultra.WorldModel.Activity.GATHER : com.oddlabs.tt.player.ultra.WorldModel.Activity.WALK);
            }
            return com.oddlabs.tt.player.ultra.WorldModel.Activity.IDLE;
        }
        // Java: 294
        public static bool isStunned(com.oddlabs.tt.model.Unit u)
        {
            return !u.isDead() && (u.getCurrentBehaviour() is com.oddlabs.tt.model.behaviour.StunBehaviour);
        }
        // Java: 299
        public static com.oddlabs.tt.model.Building buildTarget(com.oddlabs.tt.model.Unit u)
        {
            com.oddlabs.tt.model.behaviour.Controller primary_v = com.oddlabs.tt.player.ultra.WorldModel.primary(u);
            if ((primary_v is com.oddlabs.tt.model.behaviour.RepairController r))
            {
                return r.getBuilding();
            }
            if ((primary_v is com.oddlabs.tt.model.behaviour.PlaceBuildingController p))
            {
                return p.getBuilding();
            }
            return null;
        }
        // Java: 309
        public static com.oddlabs.tt.model.Unit garrisonOf(com.oddlabs.tt.model.Building tower)
        {
            if (tower.isDead() || !tower.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.ATTACK))
            {
                return null;
            }
            com.oddlabs.tt.model.UnitContainer c = tower.getUnitContainer();
            if ((c is com.oddlabs.tt.model.MountUnitContainer m))
            {
                return m.getUnit();
            }
            return null;
        }
        private void init_WorldModel()
        {
            this.me = new com.oddlabs.tt.player.ultra.WorldModel.Side();
            this.enemy = new com.oddlabs.tt.player.ultra.WorldModel.Side();
        }
        public WorldModel(com.oddlabs.tt.player.Player owner) : base()
        {
            init_WorldModel();
            this.owner = owner;
        }
    }
}
