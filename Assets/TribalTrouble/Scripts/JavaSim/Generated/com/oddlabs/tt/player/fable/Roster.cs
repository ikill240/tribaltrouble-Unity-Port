// Translated from Roster.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.fable
{
    // Java: 36
    public class Roster : java.lang.Object
    {
        // Java: 38
        public sealed class Activity : java.lang.Enum
        {
            public static Activity[] values() { return (Activity[])VALUES_.Clone(); }
            public static Activity valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly Activity IDLE = new Activity("IDLE", 0);
            public static readonly Activity GATHER_TREE = new Activity("GATHER_TREE", 1);
            public static readonly Activity GATHER_ROCK = new Activity("GATHER_ROCK", 2);
            public static readonly Activity GATHER_IRON = new Activity("GATHER_IRON", 3);
            public static readonly Activity GATHER_RUBBER = new Activity("GATHER_RUBBER", 4);
            public static readonly Activity WALK = new Activity("WALK", 5);
            public static readonly Activity ATTACK_MOVE = new Activity("ATTACK_MOVE", 6);
            public static readonly Activity DEFEND = new Activity("DEFEND", 7);
            public static readonly Activity PLACE_BUILDING = new Activity("PLACE_BUILDING", 8);
            public static readonly Activity REPAIR = new Activity("REPAIR", 9);
            public static readonly Activity HUNT = new Activity("HUNT", 10);
            public static readonly Activity ENTER = new Activity("ENTER", 11);
            public static readonly Activity STUNNED = new Activity("STUNNED", 12);
            public static readonly Activity OTHER = new Activity("OTHER", 13);
            public static readonly Activity[] VALUES_ = new Activity[] { IDLE, GATHER_TREE, GATHER_ROCK, GATHER_IRON, GATHER_RUBBER, WALK, ATTACK_MOVE, DEFEND, PLACE_BUILDING, REPAIR, HUNT, ENTER, STUNNED, OTHER };
            public Activity(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        public com.oddlabs.tt.player.Player player;
        public java.util.List peons;
        public java.util.List warriors;
        public com.oddlabs.tt.model.Unit chieftain;
        public java.util.List quarters;
        public java.util.List armories;
        public java.util.List towers;
        public java.util.List under_construction;
        public int idle_peons;
        public int gatherers_tree;
        public int gatherers_rock;
        public int gatherers_iron;
        public int gatherers_rubber;
        public int builders;
        public int idle_warriors;
        public int rubber_warriors;
        public int peons_in_quarters;
        public int peons_in_armory;
        public int warriors_in_towers;
        // Java: 82
        public void refresh()
        {
            peons.clear();
            warriors.clear();
            chieftain = null;
            quarters.clear();
            armories.clear();
            towers.clear();
            under_construction.clear();
            idle_peons = gatherers_tree = gatherers_rock = gatherers_iron = gatherers_rubber = builders = 0;
            idle_warriors = rubber_warriors = peons_in_quarters = peons_in_armory = warriors_in_towers = 0;
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(player.getUnits().getSet()))
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
                    if (u.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC))
                    {
                        chieftain = u;
                    }
                    else if (u.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD))
                    {
                        peons.add(u);
                        switch ((com.oddlabs.tt.player.fable.Roster.activity(u)).ordinal())
                        {
                            case 0 /*IDLE*/:
                            {
                                idle_peons++;
                                break;
                            }
                            case 1 /*GATHER_TREE*/:
                            {
                                gatherers_tree++;
                                break;
                            }
                            case 2 /*GATHER_ROCK*/:
                            {
                                gatherers_rock++;
                                break;
                            }
                            case 3 /*GATHER_IRON*/:
                            {
                                gatherers_iron++;
                                break;
                            }
                            case 4 /*GATHER_RUBBER*/:
                            {
                                gatherers_rubber++;
                                break;
                            }
                            case 8 /*PLACE_BUILDING*/:
                            case 9 /*REPAIR*/:
                            {
                                builders++;
                                break;
                            }
                            default:
                            {
                                break;
                            }
                        }
                    }
                    else if (u.getAbilities().isWarrior())
                    {
                        warriors.add(u);
                        if (((object)com.oddlabs.tt.player.fable.Roster.activity(u) == (object)com.oddlabs.tt.player.fable.Roster.Activity.IDLE))
                        {
                            idle_warriors++;
                        }
                        if (com.oddlabs.tt.player.fable.Roster.isRubber(u))
                        {
                            rubber_warriors++;
                        }
                    }
                }
                else if ((s is com.oddlabs.tt.model.LandBuilding b))
                {
                    if (!b.isPlaced())
                    {
                        continue;
                    }
                    if (!b.isComplete())
                    {
                        under_construction.add(b);
                        continue;
                    }
                    switch ((((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID()))
                    {
                        case 0:
                        {
                            quarters.add(b);
                            peons_in_quarters += com.oddlabs.tt.player.fable.Roster.unitsInside(b);
                            break;
                        }
                        case 1:
                        {
                            armories.add(b);
                            peons_in_armory += com.oddlabs.tt.player.fable.Roster.unitsInside(b);
                            break;
                        }
                        case 2:
                        {
                            towers.add(b);
                            warriors_in_towers += com.oddlabs.tt.player.fable.Roster.unitsInside(b);
                            break;
                        }
                        default:
                        {
                            break;
                        }
                    }
                }
            }
        }
        // Java: 146
        public static int unitsInside(com.oddlabs.tt.model.LandBuilding b)
        {
            if (b.isDead() || !b.isComplete() || b.getUnitContainer() == null)
            {
                return 0;
            }
            return b.getUnitContainer().getNumSupplies();
        }
        // Java: 152
        public static bool isRubber(com.oddlabs.tt.model.Unit u)
        {
            System.Type type = u.getWeaponFactory().getType();
            return ((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)) || ((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RubberSpearWeapon));
        }
        // Java: 158
        public static com.oddlabs.tt.player.fable.Roster.Activity activity(com.oddlabs.tt.model.Unit u)
        {
            if (u.isDead())
            {
                return com.oddlabs.tt.player.fable.Roster.Activity.OTHER;
            }
            com.oddlabs.tt.model.behaviour.Controller c = u.getPrimaryController();
            if ((u.getCurrentController() is com.oddlabs.tt.model.behaviour.StunController))
            {
                return com.oddlabs.tt.player.fable.Roster.Activity.STUNNED;
            }
            if ((c is com.oddlabs.tt.model.behaviour.IdleController))
            {
                return com.oddlabs.tt.player.fable.Roster.Activity.IDLE;
            }
            if ((c is com.oddlabs.tt.model.behaviour.GatherController g))
            {
                System.Type type = g.getSupplyType();
                if (((object)type == (object)typeof(com.oddlabs.tt.landscape.TreeSupply)))
                {
                    return com.oddlabs.tt.player.fable.Roster.Activity.GATHER_TREE;
                }
                if (((object)type == (object)typeof(com.oddlabs.tt.model.RockSupply)))
                {
                    return com.oddlabs.tt.player.fable.Roster.Activity.GATHER_ROCK;
                }
                if (((object)type == (object)typeof(com.oddlabs.tt.model.IronSupply)))
                {
                    return com.oddlabs.tt.player.fable.Roster.Activity.GATHER_IRON;
                }
                if (((object)type == (object)typeof(com.oddlabs.tt.model.RubberSupply)))
                {
                    return com.oddlabs.tt.player.fable.Roster.Activity.GATHER_RUBBER;
                }
                return com.oddlabs.tt.player.fable.Roster.Activity.OTHER;
            }
            if ((c is com.oddlabs.tt.model.behaviour.WalkController w))
            {
                return (w.isAgressive() ? com.oddlabs.tt.player.fable.Roster.Activity.ATTACK_MOVE : com.oddlabs.tt.player.fable.Roster.Activity.WALK);
            }
            if ((c is com.oddlabs.tt.model.behaviour.DefendController))
            {
                return com.oddlabs.tt.player.fable.Roster.Activity.DEFEND;
            }
            if ((c is com.oddlabs.tt.model.behaviour.PlaceBuildingController))
            {
                return com.oddlabs.tt.player.fable.Roster.Activity.PLACE_BUILDING;
            }
            if ((c is com.oddlabs.tt.model.behaviour.RepairController))
            {
                return com.oddlabs.tt.player.fable.Roster.Activity.REPAIR;
            }
            if ((c is com.oddlabs.tt.model.behaviour.HuntController))
            {
                return com.oddlabs.tt.player.fable.Roster.Activity.HUNT;
            }
            if ((c is com.oddlabs.tt.model.behaviour.EnterController))
            {
                return com.oddlabs.tt.player.fable.Roster.Activity.ENTER;
            }
            return com.oddlabs.tt.player.fable.Roster.Activity.OTHER;
        }
        // Java: 194
        public static com.oddlabs.tt.model.LandBuilding buildingWorkedOn(com.oddlabs.tt.model.Unit u)
        {
            if (u.isDead())
            {
                return null;
            }
            com.oddlabs.tt.model.behaviour.Controller c = u.getPrimaryController();
            if ((c is com.oddlabs.tt.model.behaviour.PlaceBuildingController p) && (p.getBuilding() is com.oddlabs.tt.model.LandBuilding b))
            {
                return b;
            }
            if ((c is com.oddlabs.tt.model.behaviour.RepairController r) && (r.getBuilding() is com.oddlabs.tt.model.LandBuilding b_2))
            {
                return b_2;
            }
            return null;
        }
        // Java: 205
        public int unitCount()
        {
            return player.getUnitCountContainer().getNumSupplies();
        }
        // Java: 209
        public int unitCap()
        {
            return player.getWorld().getMaxUnitCount();
        }
        // Java: 213
        public int buildingCount()
        {
            return player.getBuildingCountContainer().getNumSupplies();
        }
        // Java: 217
        public int buildingCap()
        {
            return player.getWorld().getMaxBuildingCount();
        }
        // Java: 221
        public com.oddlabs.tt.model.LandBuilding mainArmory()
        {
            return (armories.isEmpty() ? (com.oddlabs.tt.model.LandBuilding)(null) : ((com.oddlabs.tt.model.LandBuilding)armories.getFirst()));
        }
        // Java: 225
        public int totalWarriors()
        {
            return warriors.size() + warriors_in_towers;
        }
        // Java: 229
        public int totalPeons()
        {
            return peons.size() + peons_in_quarters + peons_in_armory;
        }
        private void init_Roster()
        {
            this.peons = new java.util.ArrayList();
            this.warriors = new java.util.ArrayList();
            this.quarters = new java.util.ArrayList();
            this.armories = new java.util.ArrayList();
            this.towers = new java.util.ArrayList();
            this.under_construction = new java.util.ArrayList();
        }
        public Roster(com.oddlabs.tt.player.Player player) : base()
        {
            init_Roster();
            this.player = player;
        }
    }
}
