// Translated from Building.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 15
    public abstract class Building : com.oddlabs.tt.model.Selectable, com.oddlabs.tt.pathfinder.Occupant
    {
        // Java: 16
        public sealed class BuildState : java.lang.Enum
        {
            public static BuildState[] values() { return (BuildState[])VALUES_.Clone(); }
            public static BuildState valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly BuildState START = new BuildState("START", 0);
            public static readonly BuildState HALFBUILT = new BuildState("HALFBUILT", 1);
            public static readonly BuildState BUILT = new BuildState("BUILT", 2);
            public static readonly BuildState[] VALUES_ = new BuildState[] { START, HALFBUILT, BUILT };
            public BuildState(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        // Java: 26
        public abstract bool hasRallyPoint();
        // Java: 28
        public abstract com.oddlabs.tt.util.Target getRallyPoint();
        // Java: 30
        public abstract com.oddlabs.tt.model.UnitContainer getUnitContainer();
        // Java: 32
        public abstract com.oddlabs.tt.model.SupplyContainer getSupplyContainer(System.Type key);
        // Java: 34
        public abstract com.oddlabs.tt.model.BuildSupplyContainer getBuildSupplyContainer(System.Type key);
        // Java: 36
        public abstract com.oddlabs.tt.model.DeployContainer getDeployContainer(com.oddlabs.tt.model.DeployType type);
        // Java: 38
        public abstract com.oddlabs.tt.model.ChieftainContainer getChieftainContainer();
        // Java: 40
        public abstract int getUnitCount();
        // Java: 42
        public abstract bool canExitTower();
        // Java: 44
        public abstract void exitTower();
        // Java: 46
        public abstract void deployUnits(com.oddlabs.tt.model.DeployType type, int num_units);
        // Java: 48
        public abstract void createHarvesters(int num_tree, int num_rock, int num_iron, int num_rubber);
        // Java: 50
        public abstract void buildWeapons(System.Type type, int num_weapons, bool infinite);
        // Java: 52
        public abstract bool canBuildChieftain();
        // Java: 54
        public abstract bool canStopChieftain();
        // Java: 56
        public abstract void trainChieftain(bool start);
        // Java: 58
        public abstract void deployChieftain();
        // Java: 60
        public abstract void createArmy(int num_peon, int num_rock, int num_iron, int num_rubber);
        // Java: 63
        public virtual void createUnits(int unit_template, int amount)
        {
        }
        // Java: 66
        public abstract void createTransporters(int num_tree, int num_rock, int num_iron, int num_rubber);
        // Java: 68
        public abstract bool isDamaged();
        // Java: 70
        public abstract int getHitPoints();
        // Java: 83
        public virtual int getEffectiveMaxHitPoints()
        {
            return ((com.oddlabs.tt.model.BuildingTemplate)getTemplate()).getMaxHitPoints();
        }
        // Java: 87
        public abstract void repair(int amount);
        // Java: 89
        public abstract bool isPlacingLegal();
        // Java: 91
        public abstract bool isPlaced();
        // Java: 93
        public abstract bool isComplete();
        // Java: 95
        public abstract void place();
        // Java: 97
        public abstract bool isValidRallyPoint(com.oddlabs.tt.util.Target t);
        // Java: 99
        public abstract void setRallyPoint(com.oddlabs.tt.util.Target target);
        // Java: 101
        public abstract void fillSupplies(System.Type key, int max);
        // Java: 103
        public abstract void removeSupplies(System.Type key);
        // Java: 105
        public abstract com.oddlabs.tt.model.Building.BuildState getRenderLevel();
        // Java: 107
        public virtual com.oddlabs.tt.model.Building getEntrance()
        {
            return this;
        }
        // Java: 115
        public virtual float getInteractDistance()
        {
            return 0.0f;
        }
        // Java: 119
        public virtual com.oddlabs.tt.model.Building getBase()
        {
            return this;
        }
        // Java: 123
        public virtual void printDebugInfo()
        {
            java.lang.IO.println("-----------------------------------");
            if (getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.REPRODUCE))
            {
                java.lang.IO.println("Units = " + (getUnitContainer().getNumSupplies()));
            }
            else if (getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD_ARMIES))
            {
                java.lang.IO.println("Units = " + (getUnitContainer().getNumSupplies()));
                java.lang.IO.println("Tree = " + (getSupplyContainer(typeof(com.oddlabs.tt.landscape.TreeSupply)).getNumSupplies()));
                java.lang.IO.println("Rock = " + (getSupplyContainer(typeof(com.oddlabs.tt.model.RockSupply)).getNumSupplies()));
                java.lang.IO.println("Iron = " + (getSupplyContainer(typeof(com.oddlabs.tt.model.IronSupply)).getNumSupplies()));
                java.lang.IO.println("Rubber = " + (getSupplyContainer(typeof(com.oddlabs.tt.model.RubberSupply)).getNumSupplies()));
                java.lang.IO.println("Rock Weapons = " + (getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)).getNumSupplies()));
                java.lang.IO.println("Iron Weapons = " + (getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)).getNumSupplies()));
                java.lang.IO.println("Rubber Weapons = " + (getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)).getNumSupplies()));
            }
        }
        public Building(com.oddlabs.tt.player.Player owner, com.oddlabs.tt.model.BuildingTemplate template) : base(owner, template)
        {
        }
        public abstract int getPenalty();
    }
}
