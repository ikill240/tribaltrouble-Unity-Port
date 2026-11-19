// Translated from Intel.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.ai
{
    // Java: 45
    public class Intel : java.lang.Object
    {
        // Java: 46
        public sealed class PeonState : java.lang.Enum
        {
            public static PeonState[] values() { return (PeonState[])VALUES_.Clone(); }
            public static PeonState valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly PeonState IDLE = new PeonState("IDLE", 0);
            public static readonly PeonState GATHER_TREE = new PeonState("GATHER_TREE", 1);
            public static readonly PeonState GATHER_ROCK = new PeonState("GATHER_ROCK", 2);
            public static readonly PeonState GATHER_IRON = new PeonState("GATHER_IRON", 3);
            public static readonly PeonState GATHER_CHICKEN = new PeonState("GATHER_CHICKEN", 4);
            public static readonly PeonState BUILD = new PeonState("BUILD", 5);
            public static readonly PeonState TRANSIT = new PeonState("TRANSIT", 6);
            public static readonly PeonState FIGHT = new PeonState("FIGHT", 7);
            public static readonly PeonState MOVE = new PeonState("MOVE", 8);
            public static readonly PeonState STUNNED = new PeonState("STUNNED", 9);
            public static readonly PeonState SAPPER = new PeonState("SAPPER", 10);
            public static readonly PeonState GATHER_RARE = new PeonState("GATHER_RARE", 11);
            public static readonly PeonState[] VALUES_ = new PeonState[] { IDLE, GATHER_TREE, GATHER_ROCK, GATHER_IRON, GATHER_CHICKEN, BUILD, TRANSIT, FIGHT, MOVE, STUNNED, SAPPER, GATHER_RARE };
            public PeonState(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        // Java: 63
        public sealed class WarriorState : java.lang.Enum
        {
            public static WarriorState[] values() { return (WarriorState[])VALUES_.Clone(); }
            public static WarriorState valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly WarriorState IDLE = new WarriorState("IDLE", 0);
            public static readonly WarriorState ATTACK_MOVE = new WarriorState("ATTACK_MOVE", 1);
            public static readonly WarriorState MOVE = new WarriorState("MOVE", 2);
            public static readonly WarriorState FIGHT = new WarriorState("FIGHT", 3);
            public static readonly WarriorState ENTER = new WarriorState("ENTER", 4);
            public static readonly WarriorState STUNNED = new WarriorState("STUNNED", 5);
            public static readonly WarriorState[] VALUES_ = new WarriorState[] { IDLE, ATTACK_MOVE, MOVE, FIGHT, ENTER, STUNNED };
            public WarriorState(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        // Java: 72
        public sealed class WarriorType : java.lang.Enum
        {
            public static WarriorType[] values() { return (WarriorType[])VALUES_.Clone(); }
            public static WarriorType valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly WarriorType ROCK = new WarriorType("ROCK", 0);
            public static readonly WarriorType IRON = new WarriorType("IRON", 1);
            public static readonly WarriorType CHICKEN = new WarriorType("CHICKEN", 2);
            public static readonly WarriorType[] VALUES_ = new WarriorType[] { ROCK, IRON, CHICKEN };
            public WarriorType(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        public com.oddlabs.tt.player.Player owner;
        public java.util.List quarters;
        public java.util.List armories;
        public java.util.List towers;
        public java.util.List quarters_sites;
        public java.util.List armory_sites;
        public java.util.List tower_sites;
        public java.util.List peons;
        public java.util.List warriors;
        public java.util.Set sappers;
        public java.util.Map peon_states;
        public java.util.Map warrior_states;
        public java.util.Map builder_sites;
        public java.util.Map gather_buildings;
        public com.oddlabs.tt.model.Unit chieftain;
        public com.oddlabs.tt.model.Building primary_armory;
        public java.util.List enemy_warriors;
        public java.util.List enemy_peons;
        public java.util.List enemy_buildings;
        public java.util.List enemy_towers;
        public java.util.List enemy_armories;
        public java.util.List enemy_quarters;
        public java.util.List enemy_chieftains;
        // Java: 115
        public void update()
        {
            quarters.clear();
            armories.clear();
            towers.clear();
            quarters_sites.clear();
            armory_sites.clear();
            tower_sites.clear();
            peons.clear();
            warriors.clear();
            peon_states.clear();
            warrior_states.clear();
            builder_sites.clear();
            gather_buildings.clear();
            chieftain = null;
            enemy_warriors.clear();
            enemy_peons.clear();
            enemy_buildings.clear();
            enemy_towers.clear();
            enemy_armories.clear();
            enemy_quarters.clear();
            enemy_chieftains.clear();
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(owner.getUnits().getSet()))
            {
                if (s.isDead())
                {
                    continue;
                }
                if ((s is com.oddlabs.tt.model.Building building))
                {
                    classifyOwnBuilding(building);
                }
                else if ((s is com.oddlabs.tt.model.Unit unit))
                {
                    classifyOwnUnit(unit);
                }
            }
            foreach (com.oddlabs.tt.player.Player player in owner.getWorld().getPlayers())
            {
                if (!owner.isEnemy(player))
                {
                    continue;
                }
                foreach (com.oddlabs.tt.model.Selectable s_2 in java.lang.JIter.of(player.getUnits().getSet()))
                {
                    if (s_2.isDead())
                    {
                        continue;
                    }
                    if ((s_2 is com.oddlabs.tt.model.Building building_2))
                    {
                        if (((com.oddlabs.tt.model.BuildingTemplate)building_2.getTemplate()).getType() != com.oddlabs.tt.model.BuildingTemplate.TYPE_BUILDING)
                        {
                            continue;
                        }
                        enemy_buildings.add(building_2);
                        if (!building_2.isComplete())
                        {
                            continue;
                        }
                        switch ((((com.oddlabs.tt.model.BuildingTemplate)building_2.getTemplate()).getTemplateID()))
                        {
                            case 2:
                            {
                                enemy_towers.add(building_2);
                                break;
                            }
                            case 1:
                            {
                                enemy_armories.add(building_2);
                                break;
                            }
                            case 0:
                            {
                                enemy_quarters.add(building_2);
                                break;
                            }
                            default:
                            {
                                break;
                            }
                        }
                    }
                    else if ((s_2 is com.oddlabs.tt.model.Unit unit_2))
                    {
                        if (unit_2.isMounted())
                        {
                            continue;
                        }
                        if (unit_2.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC))
                        {
                            enemy_chieftains.add(unit_2);
                        }
                        else if (unit_2.getAbilities().isWarrior())
                        {
                            enemy_warriors.add(unit_2);
                        }
                        else if (unit_2.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.HARVEST))
                        {
                            enemy_peons.add(unit_2);
                        }
                    }
                }
            }
        }
        // Java: 179
        public void classifyOwnBuilding(com.oddlabs.tt.model.Building building)
        {
            if (((com.oddlabs.tt.model.BuildingTemplate)building.getTemplate()).getType() != com.oddlabs.tt.model.BuildingTemplate.TYPE_BUILDING)
            {
                return;
            }
            bool complete = building.isComplete();
            switch ((((com.oddlabs.tt.model.BuildingTemplate)building.getTemplate()).getTemplateID()))
            {
                case 0:
                {
                    ((complete ? quarters : quarters_sites)).add(building);
                    break;
                }
                case 1:
                {
                    ((complete ? armories : armory_sites)).add(building);
                    break;
                }
                case 2:
                {
                    ((complete ? towers : tower_sites)).add(building);
                    break;
                }
                default:
                {
                    break;
                }
            }
        }
        // Java: 192
        public void classifyOwnUnit(com.oddlabs.tt.model.Unit unit)
        {
            if (unit.isMounted())
            {
                return;
            }
            com.oddlabs.tt.model.behaviour.Controller controller = unit.getPrimaryController();
            if (unit.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC))
            {
                chieftain = unit;
            }
            else if (unit.getAbilities().isWarrior())
            {
                warriors.add(unit);
                warrior_states.put(unit, com.oddlabs.tt.player.ai.Intel.warriorState(unit, controller));
            }
            else if (unit.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.HARVEST))
            {
                peons.add(unit);
                peon_states.put(unit, peonState(unit, controller));
            }
        }
        // Java: 207
        public com.oddlabs.tt.player.ai.Intel.PeonState peonState(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.model.behaviour.Controller controller)
        {
            if (com.oddlabs.tt.player.ai.Intel.isStunned(unit))
            {
                return com.oddlabs.tt.player.ai.Intel.PeonState.STUNNED;
            }
            if (sappers.contains(unit))
            {
                return com.oddlabs.tt.player.ai.Intel.PeonState.SAPPER;
            }
            if ((controller is com.oddlabs.tt.model.behaviour.IdleController) && (unit.getCurrentController() is com.oddlabs.tt.model.behaviour.HuntController))
            {
                return com.oddlabs.tt.player.ai.Intel.PeonState.FIGHT;
            }
            if ((controller is com.oddlabs.tt.model.behaviour.IdleController))
            {
                return com.oddlabs.tt.player.ai.Intel.PeonState.IDLE;
            }
            if ((controller is com.oddlabs.tt.model.behaviour.GatherController gather))
            {
                gather_buildings.put(unit, gather.getAssignedBuilding());
                System.Type type = gather.getSupplyType();
                if (((object)type == (object)typeof(com.oddlabs.tt.landscape.TreeSupply)))
                {
                    return com.oddlabs.tt.player.ai.Intel.PeonState.GATHER_TREE;
                }
                if (((object)type == (object)typeof(com.oddlabs.tt.model.RockSupply)))
                {
                    return com.oddlabs.tt.player.ai.Intel.PeonState.GATHER_ROCK;
                }
                if (((object)type == (object)typeof(com.oddlabs.tt.model.IronSupply)))
                {
                    return com.oddlabs.tt.player.ai.Intel.PeonState.GATHER_IRON;
                }
                if (((object)type == (object)typeof(com.oddlabs.tt.model.RubberSupply)))
                {
                    return com.oddlabs.tt.player.ai.Intel.PeonState.GATHER_CHICKEN;
                }
                if (com.oddlabs.tt.model.ExtendedResources.isRare(type))
                {
                    return com.oddlabs.tt.player.ai.Intel.PeonState.GATHER_RARE;
                }
                return com.oddlabs.tt.player.ai.Intel.PeonState.IDLE;
            }
            if ((controller is com.oddlabs.tt.model.behaviour.RepairController repair))
            {
                builder_sites.put(unit, repair.getBuilding());
                return com.oddlabs.tt.player.ai.Intel.PeonState.BUILD;
            }
            if ((controller is com.oddlabs.tt.model.behaviour.PlaceBuildingController place))
            {
                builder_sites.put(unit, place.getBuilding());
                return com.oddlabs.tt.player.ai.Intel.PeonState.BUILD;
            }
            if ((controller is com.oddlabs.tt.model.behaviour.EnterController) || (controller is com.oddlabs.tt.model.behaviour.TransferUnitController))
            {
                return com.oddlabs.tt.player.ai.Intel.PeonState.TRANSIT;
            }
            if ((controller is com.oddlabs.tt.model.behaviour.HuntController) || (controller is com.oddlabs.tt.model.behaviour.AttackController) || (controller is com.oddlabs.tt.model.behaviour.DefendController))
            {
                return com.oddlabs.tt.player.ai.Intel.PeonState.FIGHT;
            }
            if ((controller is com.oddlabs.tt.model.behaviour.WalkController walk))
            {
                return (walk.isAgressive() ? com.oddlabs.tt.player.ai.Intel.PeonState.FIGHT : com.oddlabs.tt.player.ai.Intel.PeonState.MOVE);
            }
            if ((controller is com.oddlabs.tt.model.behaviour.StunController))
            {
                return com.oddlabs.tt.player.ai.Intel.PeonState.STUNNED;
            }
            return com.oddlabs.tt.player.ai.Intel.PeonState.IDLE;
        }
        // Java: 251
        public static com.oddlabs.tt.player.ai.Intel.WarriorState warriorState(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.model.behaviour.Controller controller)
        {
            com.oddlabs.tt.model.behaviour.Controller current = unit.getCurrentController();
            if (com.oddlabs.tt.player.ai.Intel.isStunned(unit))
            {
                return com.oddlabs.tt.player.ai.Intel.WarriorState.STUNNED;
            }
            if ((current is com.oddlabs.tt.model.behaviour.HuntController) || (current is com.oddlabs.tt.model.behaviour.AttackController))
            {
                return com.oddlabs.tt.player.ai.Intel.WarriorState.FIGHT;
            }
            if ((controller is com.oddlabs.tt.model.behaviour.IdleController))
            {
                return com.oddlabs.tt.player.ai.Intel.WarriorState.IDLE;
            }
            if ((controller is com.oddlabs.tt.model.behaviour.WalkController walk))
            {
                return (walk.isAgressive() ? com.oddlabs.tt.player.ai.Intel.WarriorState.ATTACK_MOVE : com.oddlabs.tt.player.ai.Intel.WarriorState.MOVE);
            }
            if ((controller is com.oddlabs.tt.model.behaviour.HuntController) || (controller is com.oddlabs.tt.model.behaviour.AttackController) || (controller is com.oddlabs.tt.model.behaviour.DefendController))
            {
                return com.oddlabs.tt.player.ai.Intel.WarriorState.FIGHT;
            }
            if ((controller is com.oddlabs.tt.model.behaviour.EnterController))
            {
                return com.oddlabs.tt.player.ai.Intel.WarriorState.ENTER;
            }
            if ((controller is com.oddlabs.tt.model.behaviour.StunController))
            {
                return com.oddlabs.tt.player.ai.Intel.WarriorState.STUNNED;
            }
            return com.oddlabs.tt.player.ai.Intel.WarriorState.IDLE;
        }
        // Java: 272
        public static com.oddlabs.tt.player.ai.Intel.WarriorType warriorType(com.oddlabs.tt.model.Unit unit)
        {
            if (unit.isDead())
            {
                return com.oddlabs.tt.player.ai.Intel.WarriorType.IRON;
            }
            System.Type type = unit.getWeaponFactory().getType();
            if (((object)type == (object)typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)) || ((object)type == (object)typeof(com.oddlabs.tt.model.weapon.IronSpearWeapon)))
            {
                return com.oddlabs.tt.player.ai.Intel.WarriorType.IRON;
            }
            if (((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)) || ((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RubberSpearWeapon)))
            {
                return com.oddlabs.tt.player.ai.Intel.WarriorType.CHICKEN;
            }
            if (((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)) || ((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RockSpearWeapon)))
            {
                return com.oddlabs.tt.player.ai.Intel.WarriorType.ROCK;
            }
            return com.oddlabs.tt.player.ai.Intel.WarriorType.IRON;
        }
        // Java: 289
        public static bool isStunned(com.oddlabs.tt.model.Unit unit)
        {
            return !unit.isDead() && ((unit.getCurrentController() is com.oddlabs.tt.model.behaviour.StunController) || (unit.getCurrentBehaviour() is com.oddlabs.tt.model.behaviour.StunBehaviour));
        }
        // Java: 295
        public static bool isDefenseless(com.oddlabs.tt.model.Unit unit)
        {
            return !unit.isDead() && (unit.getCurrentController() is com.oddlabs.tt.model.behaviour.StunController);
        }
        // Java: 300
        public static bool isTowerActive(com.oddlabs.tt.model.Building tower)
        {
            if (tower.isDead() || !tower.isComplete() || tower.getUnitContainer() == null)
            {
                return false;
            }
            if (tower.getUnitContainer().getNumSupplies() == 0)
            {
                return false;
            }
            com.oddlabs.tt.model.Unit unit = (((com.oddlabs.tt.model.MountUnitContainer)tower.getUnitContainer())).getUnit();
            return unit != null && !com.oddlabs.tt.player.ai.Intel.isStunned(unit);
        }
        // Java: 309
        public static bool isTowerManned(com.oddlabs.tt.model.Building tower)
        {
            return !tower.isDead() && tower.isComplete() && tower.getUnitContainer() != null && tower.getUnitContainer().getNumSupplies() > 0;
        }
        // Java: 314
        public int countPeons(com.oddlabs.tt.player.ai.Intel.PeonState state)
        {
            int n = 0;
            foreach (com.oddlabs.tt.player.ai.Intel.PeonState s in java.lang.JIter.of(peon_states.values()))
            {
                if (((object)s == (object)state))
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 326
        public com.oddlabs.tt.model.Building armory()
        {
            if (primary_armory != null && armories.contains(primary_armory))
            {
                return primary_armory;
            }
            return (armories.isEmpty() ? (com.oddlabs.tt.model.Building)(null) : ((com.oddlabs.tt.model.Building)armories.getFirst()));
        }
        // Java: 333
        public void setPrimaryArmory(com.oddlabs.tt.model.Building armory_v)
        {
            primary_armory = armory_v;
        }
        // Java: 339
        public int countLinkedGatherers(com.oddlabs.tt.player.ai.Intel.PeonState state, com.oddlabs.tt.model.Building armory_v)
        {
            int n = 0;
            foreach (java.util.Map.Entry e in java.lang.JIter.of(peon_states.entrySet()))
            {
                if (((object)((com.oddlabs.tt.player.ai.Intel.PeonState)e.getValue()) == (object)state) && ((object)((com.oddlabs.tt.model.Building)gather_buildings.get(((com.oddlabs.tt.model.Unit)e.getKey()))) == (object)armory_v))
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 347
        public int countGatherers(com.oddlabs.tt.player.ai.Intel.PeonState state, com.oddlabs.tt.model.Building armory_v)
        {
            int n = 0;
            foreach (java.util.Map.Entry e in java.lang.JIter.of(peon_states.entrySet()))
            {
                if (((object)((com.oddlabs.tt.player.ai.Intel.PeonState)e.getValue()) != (object)state))
                {
                    continue;
                }
                com.oddlabs.tt.model.Building b = ((com.oddlabs.tt.model.Building)gather_buildings.get(((com.oddlabs.tt.model.Unit)e.getKey())));
                if (b == null || ((object)b == (object)armory_v) || armory_v == null)
                {
                    n++;
                }
            }
            return n;
        }
        private void init_Intel()
        {
            this.quarters = new java.util.ArrayList();
            this.armories = new java.util.ArrayList();
            this.towers = new java.util.ArrayList();
            this.quarters_sites = new java.util.ArrayList();
            this.armory_sites = new java.util.ArrayList();
            this.tower_sites = new java.util.ArrayList();
            this.peons = new java.util.ArrayList();
            this.warriors = new java.util.ArrayList();
            this.sappers = new java.util.LinkedHashSet();
            this.peon_states = new java.util.LinkedHashMap();
            this.warrior_states = new java.util.LinkedHashMap();
            this.builder_sites = new java.util.LinkedHashMap();
            this.gather_buildings = new java.util.LinkedHashMap();
            this.enemy_warriors = new java.util.ArrayList();
            this.enemy_peons = new java.util.ArrayList();
            this.enemy_buildings = new java.util.ArrayList();
            this.enemy_towers = new java.util.ArrayList();
            this.enemy_armories = new java.util.ArrayList();
            this.enemy_quarters = new java.util.ArrayList();
            this.enemy_chieftains = new java.util.ArrayList();
        }
        public Intel(com.oddlabs.tt.player.Player owner) : base()
        {
            init_Intel();
            this.owner = owner;
        }
    }
}
