using System.Collections.Generic;
using UnityEngine;

namespace TribalTrouble.Game
{
    public enum UnitType { Peon, WarriorRock, WarriorIron }
    public enum BuildingType { Quarters, Armory, Tower }
    public enum UnitState { Idle, Moving, Gathering, Delivering, Building, Attacking, Entering, Dead }

    /// <summary>
    /// Gameplay numbers, taken from the 1.4.1 Java sources (RacesResources, LandBuilding, weapons,
    /// ReproduceUnitContainer, supplies). Values marked "tuned" are interim choices for this playable
    /// layer until the faithful simulation port replaces it.
    /// </summary>
    public static class Rules
    {
        public const float TickSeconds = 0.02f;           // AnimationManager: 20 ms ticks
        public const float PeonSpeed = 5f;                // peon UnitTemplate meters_per_second
        public const float WarriorSpeed = 4f;             // warrior UnitTemplate meters_per_second
        public const float ThrowRange = 9f;               // RacesResources.THROW_RANGE
        public const float MeleeRange = 2.2f;
        public const float AttackSeconds = 1.0f;          // tuned
        public const float MeleeSeconds = 1.0f;           // tuned
        public const float AutoAcquireRange = 14f;        // tuned
        public const float TowerRangeBonus = 4f;          // tuned
        public const float HarvestSeconds = 1.6f;         // tuned
        public const int QuartersHp = 200, ArmoryHp = 200, TowerHp = 100;   // RacesResources
        public const int QuartersSize = 6, ArmorySize = 4, TowerSize = 3;   // RacesResources
        public const float BuildHpPerPeonSecond = 4f;     // tuned
        public const int MaxUnits = 250;                  // Player.DEFAULT_MAX_UNIT_COUNT
        public const int InitialUnits = 20;               // Player.INITIAL_UNIT_COUNT
        public const int QuartersCapacity = 40;           // tuned
        public const int ArmoryWorkerCapacity = 20;       // tuned
        public const float WeaponWorkerSeconds = 6f;      // tuned
        public const int WeaponWood = 2, WeaponRockOrIron = 1;  // LandBuilding.COST_ROCK/IRON_WEAPON
        public const float DeathLinger = 3f;

        public static float Speed(UnitType t) => t == UnitType.Peon ? PeonSpeed : WarriorSpeed;
        public static float Defense(UnitType t) => t == UnitType.WarriorRock ? 0.5f : t == UnitType.WarriorIron ? 0.7f : 0f; // defense_chance
        public static int Damage(UnitType t) => t == UnitType.WarriorIron ? 2 : 1;   // Iron/Rock weapon getDamage()
        public static float Range(UnitType t) => t == UnitType.Peon ? MeleeRange : ThrowRange;
        public static int Size(BuildingType b) => b == BuildingType.Quarters ? QuartersSize : b == BuildingType.Armory ? ArmorySize : TowerSize;
        public static int MaxHp(BuildingType b) => b == BuildingType.Quarters ? QuartersHp : b == BuildingType.Armory ? ArmoryHp : TowerHp;
        /// <summary>ReproduceUnitContainer: (1/11) * cbrt(max(units, .5)) peons per second.</summary>
        public static float ReproductionPerSecond(int unitsInside) => (1f / 11f) * Mathf.Pow(Mathf.Max(unitsInside, .5f), 1f / 3f);
    }

    public sealed class Player
    {
        public int index;
        public string name;
        public Race race;
        public Color color;
        public int team;
        public bool human;
        public bool defeated;
        public int wood, rock, iron;
        public int rockWeapons, ironWeapons;
        public int orderedRock, orderedIron;
        public readonly List<Unit> units = new List<Unit>();
        public readonly List<Building> buildings = new List<Building>();
        public Vector2 start;

        public int UnitCount
        {
            get
            {
                int n = units.Count;
                foreach (var b in buildings) n += b.inside.Count;
                return n;
            }
        }

        public bool IsEnemy(Player other) => other != null && other != this && other.team != team;
    }

    public abstract class Entity
    {
        public int id;
        public Player owner;
        public Vector2 pos;           // metres (x, y) on the island
        public bool alive = true;
        public GameObject view;
        public abstract float Radius { get; }
    }

    public sealed class Unit : Entity
    {
        public UnitType type;
        public UnitState state;
        public List<Vector2Int> path;
        public int pathIndex;
        public Vector2 moveDest;
        public Supply gatherTarget;
        public SupplyKind lastGatherKind;
        public bool carrying;
        public SupplyKind carryKind;
        public Building buildTarget;
        public Building enterTarget;
        public Entity attackTarget;
        public bool attackMove;
        public float actionTimer;
        public float cooldown;
        public float facing;          // radians, 0 = +x
        public string anim = "idle";
        public float animTicks;
        public float deathTimer;
        public float repathTimer;
        public override float Radius => 0.6f;
        public bool IsWarrior => type != UnitType.Peon;
    }

    public sealed class Building : Entity
    {
        public BuildingType type;
        public Vector2Int cell;
        public float hp;
        public int maxHp;
        public bool complete;
        public readonly List<Unit> inside = new List<Unit>();
        public float reproduce;       // ReproduceUnitContainer.unit_reproduction
        public float weaponProgress;
        public float attackCooldown;
        public string stage = "start";
        public override float Radius => GameWorld.FootprintRadius(Rules.Size(type)) * GameWorld.MetersPerCell + 1f;
        public int Capacity => type == BuildingType.Quarters ? Rules.QuartersCapacity : type == BuildingType.Armory ? Rules.ArmoryWorkerCapacity : 1;
    }
}
