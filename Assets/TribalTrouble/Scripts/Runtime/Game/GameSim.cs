using System;
using System.Collections.Generic;
using TribalTrouble.Sim.Determinism;
using UnityEngine;

namespace TribalTrouble.Game
{
    /// <summary>
    /// Playable skirmish rules (interim layer). Fixed 20 ms ticks, seeded JavaRandom, no UnityEngine
    /// objects touched here except math types, so it can later be swapped for the faithful Java port.
    /// </summary>
    public sealed class GameSim
    {
        public readonly GameWorld world;
        public readonly PathFinder pathFinder;
        public readonly List<Player> players;
        public readonly List<Unit> units = new List<Unit>();
        public readonly List<Building> buildings = new List<Building>();
        public readonly JavaRandom random;
        public float time;
        public bool over;
        public int winningTeam = -1;

        public event Action<Entity> Spawned;
        public event Action<Entity> Removed;
        public event Action<string, Vector2> Sound;       // sfx name, position (metres)
        public event Action<Player, string> Notify;        // e.g. "Your quarters is under attack"

        int _nextId = 1;
        int _repathBudget;
        float _victoryTimer;
        readonly List<Unit> _deadScratch = new List<Unit>();

        public GameSim(GameWorld world, List<Player> players, long seed)
        {
            this.world = world;
            this.players = players;
            pathFinder = new PathFinder(world);
            random = new JavaRandom(seed);
        }

        // ------------------------------------------------------------------ setup

        public void SpawnStartingUnits(int perPlayer)
        {
            for (int p = 0; p < players.Count; p++)
            {
                var player = players[p];
                var locs = p < world.startLocations.Length ? world.startLocations[p] : world.startLocations[0];
                int n = Mathf.Min(perPlayer, locs.Length / 2);
                for (int u = 0; u < n; u++)
                {
                    var m = new Vector2(locs[2 * u] + 1f, locs[2 * u + 1] + 1f);
                    SpawnUnit(player, UnitType.Peon, m);
                }
                player.start = n > 0 ? new Vector2(locs[0], locs[1]) : new Vector2(world.meters / 2, world.meters / 2);
            }
        }

        public Unit SpawnUnit(Player owner, UnitType type, Vector2 pos)
        {
            var c = world.NearestPassable(GameWorld.CellOf(pos));
            var u = new Unit { id = _nextId++, owner = owner, type = type, pos = GameWorld.CellCenter(c.x, c.y), state = UnitState.Idle };
            u.facing = (float)(random.NextFloat() * Math.PI * 2);
            owner.units.Add(u);
            units.Add(u);
            Spawned?.Invoke(u);
            return u;
        }

        // ------------------------------------------------------------------ commands (UI + AI)

        public void CommandMove(IEnumerable<Unit> group, Vector2 dest, bool attackMove = false)
        {
            foreach (var u in group)
            {
                if (!u.alive) continue;
                ClearOrders(u);
                u.attackMove = attackMove && u.IsWarrior;
                SetPath(u, dest, 0);
                u.state = UnitState.Moving;
            }
        }

        public void CommandGather(IEnumerable<Unit> group, Supply s)
        {
            foreach (var u in group)
            {
                if (!u.alive) continue;
                if (u.type != UnitType.Peon) { CommandMove(new[] { u }, GameWorld.CellCenter(s.gx, s.gy)); continue; }
                ClearOrders(u);
                u.gatherTarget = s;
                u.lastGatherKind = s.kind;
                u.state = u.carrying ? UnitState.Delivering : UnitState.Gathering;
            }
        }

        public void CommandBuild(IEnumerable<Unit> group, Building b)
        {
            foreach (var u in group)
            {
                if (!u.alive || u.type != UnitType.Peon) continue;
                ClearOrders(u);
                u.buildTarget = b;
                u.state = UnitState.Building;
            }
        }

        public void CommandAttack(IEnumerable<Unit> group, Entity target)
        {
            foreach (var u in group)
            {
                if (!u.alive) continue;
                ClearOrders(u);
                u.attackTarget = target;
                u.state = UnitState.Attacking;
            }
        }

        public void CommandEnter(IEnumerable<Unit> group, Building b)
        {
            foreach (var u in group)
            {
                if (!u.alive) continue;
                ClearOrders(u);
                u.enterTarget = b;
                u.state = UnitState.Entering;
            }
        }

        /// <summary>Right-click semantics: gather / build / enter / attack / move depending on the target.</summary>
        public void CommandSmart(Player player, List<Unit> group, Vector2 point, Entity targetEntity, Supply targetSupply)
        {
            if (group.Count == 0) return;
            if (targetEntity is Building b && b.alive)
            {
                if (player.IsEnemy(b.owner)) { CommandAttack(group, b); return; }
                if (b.owner == player)
                {
                    if (!b.complete) { CommandBuild(group.FindAll(u => u.type == UnitType.Peon), b); CommandMove(group.FindAll(u => u.type != UnitType.Peon), point); return; }
                    if (b.type == BuildingType.Armory)
                    {
                        // carrying peons deliver, others become workers
                        foreach (var u in group)
                        {
                            if (u.type == UnitType.Peon && u.carrying) { ClearOrders(u); u.state = UnitState.Delivering; }
                            else CommandEnter(new[] { u }, b);
                        }
                        return;
                    }
                    CommandEnter(group, b);
                    return;
                }
            }
            if (targetEntity is Unit eu && eu.alive && player.IsEnemy(eu.owner)) { CommandAttack(group, eu); return; }
            if (targetSupply != null && !targetSupply.Depleted)
            {
                CommandGather(group.FindAll(u => u.type == UnitType.Peon), targetSupply);
                var others = group.FindAll(u => u.type != UnitType.Peon);
                if (others.Count > 0) CommandMove(others, point);
                return;
            }
            CommandMove(group, point, attackMove: true);
        }

        public Building PlaceBuilding(Player owner, BuildingType type, Vector2Int cell)
        {
            int size = Rules.Size(type);
            if (!world.CanPlaceBuilding(cell.x, cell.y, size)) return null;
            var b = new Building
            {
                id = _nextId++, owner = owner, type = type, cell = cell,
                pos = GameWorld.CellCenter(cell.x, cell.y), maxHp = Rules.MaxHp(type), hp = 1f,
            };
            world.SetFootprint(cell.x, cell.y, size, b.id);
            owner.buildings.Add(b);
            buildings.Add(b);
            // units standing on the new footprint step aside
            foreach (var u in units)
            {
                var c = GameWorld.CellOf(u.pos);
                if (world.InBounds(c.x, c.y) && world.blocker[c.x, c.y] == b.id)
                {
                    var free = world.NearestPassable(c);
                    u.pos = GameWorld.CellCenter(free.x, free.y);
                    u.path = null;
                }
            }
            Spawned?.Invoke(b);
            return b;
        }

        public void OrderWeapons(Player p, UnitType weapon, int count)
        {
            if (weapon == UnitType.WarriorIron) p.orderedIron = Mathf.Max(0, p.orderedIron + count);
            else p.orderedRock = Mathf.Max(0, p.orderedRock + count);
        }

        public int DeployPeons(Building b, int count)
        {
            int n = 0;
            for (int i = b.inside.Count - 1; i >= 0 && n < count; i--)
            {
                if (b.inside[i].type != UnitType.Peon) continue;
                var u = b.inside[i];
                b.inside.RemoveAt(i);
                Exit(b, u);
                n++;
            }
            return n;
        }

        public int DeployWarriors(Building armory, UnitType type, int count)
        {
            var p = armory.owner;
            int n = 0;
            while (n < count)
            {
                bool haveWeapon = type == UnitType.WarriorIron ? p.ironWeapons > 0 : p.rockWeapons > 0;
                int peonIdx = armory.inside.FindIndex(u => u.type == UnitType.Peon);
                if (!haveWeapon || peonIdx < 0) break;
                if (type == UnitType.WarriorIron) p.ironWeapons--; else p.rockWeapons--;
                var peon = armory.inside[peonIdx];
                armory.inside.RemoveAt(peonIdx);
                peon.type = type;          // the peon picks up the weapon and becomes a warrior
                Exit(armory, peon);
                n++;
            }
            if (n > 0) Sound?.Invoke("armory", armory.pos);
            return n;
        }

        public int EjectAll(Building b)
        {
            int n = b.inside.Count;
            while (b.inside.Count > 0)
            {
                var u = b.inside[b.inside.Count - 1];
                b.inside.RemoveAt(b.inside.Count - 1);
                Exit(b, u);
            }
            return n;
        }

        void Exit(Building b, Unit u)
        {
            int r = GameWorld.FootprintRadius(Rules.Size(b.type));
            var c = world.NearestPassable(new Vector2Int(b.cell.x, b.cell.y - r - 2));
            u.pos = GameWorld.CellCenter(c.x, c.y);
            u.alive = true;
            u.state = UnitState.Idle;
            u.owner.units.Add(u);
            units.Add(u);
            Spawned?.Invoke(u);
        }

        // ------------------------------------------------------------------ tick

        public void Tick()
        {
            float dt = Rules.TickSeconds;
            time += dt;
            _repathBudget = 24;
            for (int i = 0; i < units.Count; i++)
            {
                var u = units[i];
                if (u.state == UnitState.Dead)
                {
                    u.deathTimer += dt;
                    u.animTicks += dt;
                    if (u.deathTimer > Rules.DeathLinger) _deadScratch.Add(u);
                    continue;
                }
                UpdateUnit(u, dt);
            }
            foreach (var d in _deadScratch)
            {
                units.Remove(d);
                Removed?.Invoke(d);
            }
            _deadScratch.Clear();

            for (int i = buildings.Count - 1; i >= 0; i--)
                UpdateBuilding(buildings[i], dt);

            _victoryTimer += dt;
            if (_victoryTimer >= 1f) { _victoryTimer = 0; CheckVictory(); }
        }

        void UpdateUnit(Unit u, float dt)
        {
            u.cooldown -= dt;
            switch (u.state)
            {
                case UnitState.Idle:
                    u.anim = "idle";
                    u.animTicks += dt;
                    if (u.IsWarrior) AutoAcquire(u);
                    break;
                case UnitState.Moving:
                    if (u.attackMove && AutoAcquire(u)) break;
                    if (FollowPath(u, dt)) { u.state = UnitState.Idle; u.attackMove = false; }
                    break;
                case UnitState.Gathering: UpdateGather(u, dt); break;
                case UnitState.Delivering: UpdateDeliver(u, dt); break;
                case UnitState.Building: UpdateBuild(u, dt); break;
                case UnitState.Attacking: UpdateAttack(u, dt); break;
                case UnitState.Entering: UpdateEnter(u, dt); break;
            }
        }

        void ClearOrders(Unit u)
        {
            u.gatherTarget = null;
            u.buildTarget = null;
            u.enterTarget = null;
            u.attackTarget = null;
            u.attackMove = false;
            u.path = null;
            u.actionTimer = 0;
            u.repathTimer = 0;
        }

        void SetPath(Unit u, Vector2 dest, int goalRadius)
        {
            u.moveDest = dest;
            var from = GameWorld.CellOf(u.pos);
            var to = GameWorld.CellOf(dest);
            u.path = pathFinder.Find(from, to, goalRadius);
            u.pathIndex = 0;
            u.repathTimer = 0;
        }

        /// <summary>Moves along the path. Returns true when the end has been reached (or no path).</summary>
        bool FollowPath(Unit u, float dt)
        {
            if (u.path == null || u.pathIndex >= u.path.Count) return true;
            float step = Rules.Speed(u.type) * dt;
            while (step > 0 && u.pathIndex < u.path.Count)
            {
                var cell = u.path[u.pathIndex];
                bool last = u.pathIndex == u.path.Count - 1;
                if (!world.IsPassable(cell.x, cell.y) && u.pathIndex > 0)
                {
                    // something was built / placed in the way: repath (budgeted)
                    if (_repathBudget-- > 0) SetPath(u, u.moveDest, 1);
                    else u.path = null;
                    return u.path == null;
                }
                Vector2 target = last && GameWorld.CellOf(u.moveDest) == cell ? u.moveDest : GameWorld.CellCenter(cell.x, cell.y);
                Vector2 d = target - u.pos;
                float len = d.magnitude;
                if (len <= step)
                {
                    u.pos = target;
                    step -= len;
                    u.pathIndex++;
                    if (len > 0.001f) Advance(u, d / len, len);
                }
                else
                {
                    u.pos += d / len * step;
                    Advance(u, d / len, step);
                    step = 0;
                }
            }
            return u.pathIndex >= u.path.Count;
        }

        static void Advance(Unit u, Vector2 dir, float dist)
        {
            u.facing = Mathf.Atan2(dir.y, dir.x);
            if (u.anim != "run") { u.anim = "run"; u.animTicks = 0; }
            u.animTicks += dist;   // run cycles are wpc metres long
        }

        void FaceTowards(Unit u, Vector2 p)
        {
            var d = p - u.pos;
            if (d.sqrMagnitude > 0.0001f) u.facing = Mathf.Atan2(d.y, d.x);
        }

        void PathTowards(Unit u, Vector2 dest, int goalRadius, float dt)
        {
            u.repathTimer -= dt;
            // path == null alone does not force a repath: a unit whose path ended short of its goal
            // waits for repathTimer instead of re-running A* (and draining _repathBudget) every tick
            if (u.repathTimer <= 0 || (u.moveDest - dest).sqrMagnitude > 4f)
            {
                if (_repathBudget-- <= 0) return;
                SetPath(u, dest, goalRadius);
                u.repathTimer = 1.5f;
                if (u.path == null) return;
            }
            if (FollowPath(u, dt)) u.path = null;
        }

        // ---- gathering

        void UpdateGather(Unit u, float dt)
        {
            if (u.carrying) { u.state = UnitState.Delivering; return; }
            var s = u.gatherTarget;
            if (s == null || s.Depleted)
            {
                s = u.gatherTarget = world.FindNearestSupply(u.lastGatherKind, u.pos, 60f);
                if (s == null) { u.state = UnitState.Idle; return; }
            }
            var center = GameWorld.CellCenter(s.gx, s.gy);
            if ((center - u.pos).magnitude <= 3f)   // diagonal neighbour centre is 2.83 m away
            {
                FaceTowards(u, center);
                if (u.anim != "attack") { u.anim = "attack"; u.animTicks = 0; }
                u.animTicks += dt / Rules.HarvestSeconds;
                u.actionTimer += dt;
                if (u.actionTimer >= Rules.HarvestSeconds)
                {
                    u.actionTimer = 0;
                    s.remaining--;
                    u.carrying = true;
                    u.carryKind = s.kind;
                    Sound?.Invoke(s.kind == SupplyKind.Wood ? "axe_cutting_wood" : "axe_cutting_stone", center);
                    if (s.Depleted)
                    {
                        world.RemoveSupply(s);
                        if (s.kind == SupplyKind.Wood) Sound?.Invoke(s.palm ? "felling_palmtree" : "felling_tree", center);
                        Removed?.Invoke(null); // view refreshes supply instances
                    }
                    u.state = UnitState.Delivering;
                    u.path = null;
                }
            }
            else
            {
                PathTowards(u, center, 1, dt);
                if (u.path == null && (center - u.pos).magnitude > 3f && u.repathTimer > 0 && _repathBudget > 0)
                {
                    // unreachable: try another node of the same kind
                    u.gatherTarget = null;
                }
            }
        }

        /// <summary>Distance (m) from a point to the edge of a building's square footprint.</summary>
        public static float DistToFootprint(Building b, Vector2 p)
        {
            float half = (GameWorld.FootprintRadius(Rules.Size(b.type)) + 0.5f) * GameWorld.MetersPerCell;
            float dx = Mathf.Max(Mathf.Abs(p.x - b.pos.x) - half, 0f);
            float dy = Mathf.Max(Mathf.Abs(p.y - b.pos.y) - half, 0f);
            return Mathf.Sqrt(dx * dx + dy * dy);
        }

        public Building NearestComplete(Player p, BuildingType type, Vector2 from)
        {
            Building best = null;
            float bestD = float.MaxValue;
            foreach (var b in p.buildings)
            {
                if (!b.alive || !b.complete || b.type != type) continue;
                float d = (b.pos - from).sqrMagnitude;
                if (d < bestD) { bestD = d; best = b; }
            }
            return best;
        }

        void UpdateDeliver(Unit u, float dt)
        {
            var armory = NearestComplete(u.owner, BuildingType.Armory, u.pos);
            if (armory == null) { u.state = UnitState.Idle; u.anim = "idle"; return; }
            if (DistToFootprint(armory, u.pos) <= 2f)
            {
                switch (u.carryKind)
                {
                    case SupplyKind.Wood: u.owner.wood++; break;
                    case SupplyKind.Rock: u.owner.rock++; break;
                    default: u.owner.iron++; break;
                }
                u.carrying = false;
                u.path = null;
                u.state = UnitState.Gathering;
                return;
            }
            PathTowards(u, armory.pos, GameWorld.FootprintRadius(Rules.Size(armory.type)) + 1, dt);
        }

        // ---- construction

        void UpdateBuild(Unit u, float dt)
        {
            var b = u.buildTarget;
            if (b == null || !b.alive || b.complete) { u.state = UnitState.Idle; return; }
            if (DistToFootprint(b, u.pos) <= 2f)
            {
                FaceTowards(u, b.pos);
                if (u.anim != "attack") { u.anim = "attack"; u.animTicks = 0; }
                u.animTicks += dt;
                b.hp += Rules.BuildHpPerPeonSecond * dt;
                b.stage = b.hp >= b.maxHp * 0.5f ? "halfbuilt" : "start";
                if (b.hp >= b.maxHp)
                {
                    b.hp = b.maxHp;
                    b.complete = true;
                    b.stage = "built";
                    Sound?.Invoke(b.owner.race == Race.Vikings ? "buildingnotify_viking" : "buildingnotify_native", b.pos);
                }
                return;
            }
            PathTowards(u, b.pos, GameWorld.FootprintRadius(Rules.Size(b.type)) + 1, dt);
        }

        // ---- garrison

        void UpdateEnter(Unit u, float dt)
        {
            var b = u.enterTarget;
            if (b == null || !b.alive || !b.complete) { u.state = UnitState.Idle; return; }
            if (DistToFootprint(b, u.pos) <= 2f)
            {
                bool allowed = b.inside.Count < b.Capacity &&
                               (b.type == BuildingType.Quarters ||
                                (b.type == BuildingType.Armory && u.type == UnitType.Peon) ||
                                (b.type == BuildingType.Tower && u.IsWarrior));
                if (!allowed) { u.state = UnitState.Idle; return; }
                if (u.carrying && b.type == BuildingType.Armory) { u.state = UnitState.Delivering; return; }
                u.state = UnitState.Idle;
                u.path = null;
                b.inside.Add(u);
                u.owner.units.Remove(u);
                units.Remove(u);
                Removed?.Invoke(u);
                return;
            }
            PathTowards(u, b.pos, GameWorld.FootprintRadius(Rules.Size(b.type)) + 1, dt);
        }

        // ---- combat

        bool AutoAcquire(Unit u)
        {
            if (u.cooldown > 0.5f) return false;
            var enemy = FindNearestEnemy(u.owner, u.pos, Rules.AutoAcquireRange);
            if (enemy == null) return false;
            u.attackTarget = enemy;
            u.state = UnitState.Attacking;
            return true;
        }

        public Entity FindNearestEnemy(Player p, Vector2 from, float range, bool unitsFirst = true)
        {
            Entity best = null;
            float bestD = range * range;
            foreach (var e in units)
            {
                if (!e.alive || !p.IsEnemy(e.owner)) continue;
                float d = (e.pos - from).sqrMagnitude;
                if (d < bestD) { bestD = d; best = e; }
            }
            if (best != null && unitsFirst) return best;
            foreach (var b in buildings)
            {
                if (!b.alive || !p.IsEnemy(b.owner)) continue;
                float d = (b.pos - from).magnitude - b.Radius;
                if (d * Mathf.Abs(d) < bestD) { bestD = d * Mathf.Abs(d); best = b; }
            }
            return best;
        }

        void UpdateAttack(Unit u, float dt)
        {
            var t = u.attackTarget;
            if (t == null || !t.alive)
            {
                u.attackTarget = null;
                u.state = UnitState.Idle;
                if (u.IsWarrior) AutoAcquire(u);
                return;
            }
            float range, dist;
            if (t is Building tbld) { range = Rules.Range(u.type); dist = DistToFootprint(tbld, u.pos); }
            else { range = Rules.Range(u.type) + t.Radius; dist = (t.pos - u.pos).magnitude; }
            if (dist <= range)
            {
                u.path = null;
                FaceTowards(u, t.pos);
                if (u.anim != "attack") { u.anim = "attack"; u.animTicks = 0; }
                float cycle = u.IsWarrior ? Rules.AttackSeconds : Rules.MeleeSeconds;
                u.animTicks += dt / cycle;
                if (u.cooldown <= 0)
                {
                    u.cooldown = cycle;
                    u.animTicks = 0;
                    Hit(u.owner, Rules.Damage(u.type), t, u.pos, u.IsWarrior);
                }
                return;
            }
            int goal = t is Building ? GameWorld.FootprintRadius(Rules.Size(((Building)t).type)) + Mathf.Max(1, (int)(Rules.Range(u.type) / 3f)) : 0;
            PathTowards(u, t.pos, goal, dt);
        }

        void Hit(Player attacker, int damage, Entity target, Vector2 from, bool thrown)
        {
            Sound?.Invoke(thrown ? (attacker.race == Race.Vikings ? "weapon_axe" : "weapon_spear") : "hit" + (1 + random.NextInt(7)), from);
            if (target is Unit tu)
            {
                if (random.NextFloat() < Rules.Defense(tu.type))
                {
                    Sound?.Invoke("hit" + (1 + random.NextInt(7)), tu.pos);
                    return;
                }
                Kill(tu);
            }
            else if (target is Building tb)
            {
                tb.hp -= damage;
                Sound?.Invoke("impact_wood" + (1 + random.NextInt(4)), tb.pos);
                if (tb.owner.human && tb.hp > 0) Notify?.Invoke(tb.owner, "attack");
                if (tb.hp <= 0) DestroyBuilding(tb);
            }
        }

        void Kill(Unit u)
        {
            if (!u.alive) return;
            u.alive = false;
            u.state = UnitState.Dead;
            u.anim = "die";
            u.animTicks = 0;
            u.deathTimer = 0;
            u.path = null;
            u.owner.units.Remove(u);
            bool viking = u.owner.race == Race.Vikings;
            Sound?.Invoke(u.type == UnitType.Peon ? "death_peon" : (viking ? "death_viking_warrior" : "death_native_warrior") + (1 + random.NextInt(2)), u.pos);
            if (u.owner.human) Notify?.Invoke(u.owner, "attack");
        }

        void DestroyBuilding(Building b)
        {
            if (!b.alive) return;
            b.alive = false;
            // garrison escapes
            EjectAll(b);
            world.SetFootprint(b.cell.x, b.cell.y, Rules.Size(b.type), 0);
            b.owner.buildings.Remove(b);
            buildings.Remove(b);
            Sound?.Invoke("building_crash", b.pos);
            Removed?.Invoke(b);
        }

        // ---- buildings

        void UpdateBuilding(Building b, float dt)
        {
            if (!b.alive || !b.complete) return;
            var p = b.owner;
            switch (b.type)
            {
                case BuildingType.Quarters:
                {
                    int peonsInside = b.inside.Count;
                    b.reproduce += Rules.ReproductionPerSecond(peonsInside) * dt;
                    while (b.reproduce >= 1f)
                    {
                        b.reproduce -= 1f;
                        if (p.UnitCount >= Rules.MaxUnits || b.inside.Count >= b.Capacity) continue;
                        var baby = new Unit { id = _nextId++, owner = p, type = UnitType.Peon, state = UnitState.Idle, pos = b.pos };
                        b.inside.Add(baby);
                    }
                    break;
                }
                case BuildingType.Armory:
                {
                    int workers = b.inside.Count;
                    if (workers == 0 || (p.orderedRock <= 0 && p.orderedIron <= 0)) { b.weaponProgress = 0; break; }
                    bool rock = p.orderedRock > 0 && p.wood >= Rules.WeaponWood && p.rock >= Rules.WeaponRockOrIron;
                    bool iron = !rock && p.orderedIron > 0 && p.wood >= Rules.WeaponWood && p.iron >= Rules.WeaponRockOrIron;
                    if (!rock && !iron) break;
                    b.weaponProgress += workers * dt / Rules.WeaponWorkerSeconds;
                    if (b.weaponProgress >= 1f)
                    {
                        b.weaponProgress = 0;
                        p.wood -= Rules.WeaponWood;
                        if (rock) { p.rock -= Rules.WeaponRockOrIron; p.rockWeapons++; p.orderedRock--; }
                        else { p.iron -= Rules.WeaponRockOrIron; p.ironWeapons++; p.orderedIron--; }
                    }
                    break;
                }
                case BuildingType.Tower:
                {
                    b.attackCooldown -= dt;
                    if (b.inside.Count == 0 || b.attackCooldown > 0) break;
                    var guard = b.inside[0];
                    var enemy = FindNearestEnemy(p, b.pos, Rules.ThrowRange + Rules.TowerRangeBonus);
                    if (enemy == null) break;
                    b.attackCooldown = Rules.AttackSeconds;
                    Hit(p, Rules.Damage(guard.type), enemy, b.pos, true);
                    break;
                }
            }
        }

        // ---- victory

        void CheckVictory()
        {
            foreach (var p in players)
                if (!p.defeated && p.UnitCount == 0 && p.buildings.Count == 0)
                {
                    p.defeated = true;
                    Notify?.Invoke(p, "defeated");
                }
            int aliveTeam = -1;
            bool several = false;
            foreach (var p in players)
            {
                if (p.defeated) continue;
                if (aliveTeam == -1) aliveTeam = p.team;
                else if (aliveTeam != p.team) several = true;
            }
            if (!several && !over)
            {
                over = true;
                winningTeam = aliveTeam;
            }
        }
    }
}
