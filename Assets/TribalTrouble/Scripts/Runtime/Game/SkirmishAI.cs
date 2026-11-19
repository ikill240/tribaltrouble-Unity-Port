using System.Collections.Generic;
using UnityEngine;

namespace TribalTrouble.Game
{
    public enum AIDifficulty { Easy, Normal, Hard }

    /// <summary>
    /// Opponent for the playable layer. Uses only the public command API (like a human would).
    /// Plan: build quarters + armory near the start, gather wood/rock/iron, keep peons breeding in
    /// quarters, keep armory workers making weapons, deploy warriors and attack in waves.
    /// The original AIs (AdvancedAI, Expert, Ultra, Fable, Gauntlet) arrive with the faithful port.
    /// </summary>
    public sealed class SkirmishAI
    {
        readonly GameSim _sim;
        readonly Player _p;
        readonly AIDifficulty _difficulty;
        float _think;
        float _attackTimer;
        readonly float _thinkInterval;
        readonly int _waveSize;
        readonly int _breeders;
        readonly int _workers;

        public SkirmishAI(GameSim sim, Player player, AIDifficulty difficulty)
        {
            _sim = sim;
            _p = player;
            _difficulty = difficulty;
            _thinkInterval = difficulty == AIDifficulty.Easy ? 2.5f : difficulty == AIDifficulty.Normal ? 1.5f : 0.8f;
            _waveSize = difficulty == AIDifficulty.Easy ? 14 : difficulty == AIDifficulty.Normal ? 10 : 8;
            _breeders = difficulty == AIDifficulty.Easy ? 4 : difficulty == AIDifficulty.Normal ? 8 : 12;
            _workers = difficulty == AIDifficulty.Easy ? 3 : difficulty == AIDifficulty.Normal ? 5 : 8;
        }

        public void Update(float dt)
        {
            if (_p.defeated || _sim.over) return;
            _think -= dt;
            _attackTimer -= dt;
            if (_think > 0) return;
            _think = _thinkInterval;

            var quarters = _p.buildings.Find(b => b.type == BuildingType.Quarters);
            var armory = _p.buildings.Find(b => b.type == BuildingType.Armory);
            var peons = _p.units.FindAll(u => u.type == UnitType.Peon && u.alive);
            var idlePeons = peons.FindAll(u => u.state == UnitState.Idle);

            // 1. buildings
            if (quarters == null && peons.Count > 0)
                quarters = TryBuild(BuildingType.Quarters, peons, 5);
            if (armory == null && peons.Count > 4)
                armory = TryBuild(BuildingType.Armory, peons, 4);
            foreach (var b in _p.buildings)
                if (!b.complete && CountBuilders(b) < 3 && idlePeons.Count > 0)
                {
                    _sim.CommandBuild(Take(idlePeons, 2), b);
                }
            if (_difficulty != AIDifficulty.Easy && armory != null && armory.complete &&
                _p.buildings.FindAll(b => b.type == BuildingType.Tower).Count < 2 && _p.wood > 20 && idlePeons.Count > 2)
                TryBuild(BuildingType.Tower, idlePeons, 2);

            // 2. economy
            if (quarters != null && quarters.complete)
            {
                int inside = quarters.inside.Count;
                if (inside < _breeders && idlePeons.Count > 0)
                    _sim.CommandEnter(Take(idlePeons, _breeders - inside), quarters);
                else if (inside > _breeders + 6)
                    _sim.DeployPeons(quarters, inside - _breeders);
            }
            if (armory != null && armory.complete)
            {
                if (armory.inside.Count < _workers && idlePeons.Count > 0)
                    _sim.CommandEnter(Take(idlePeons, _workers - armory.inside.Count), armory);
                // keep a few weapons on order
                if (_p.orderedRock + _p.orderedIron < 3)
                {
                    if (_p.iron >= 1 && _p.rock < 1) _sim.OrderWeapons(_p, UnitType.WarriorIron, 2);
                    else _sim.OrderWeapons(_p, UnitType.WarriorRock, 2);
                }
                int weapons = _p.rockWeapons + _p.ironWeapons;
                if (weapons > 0 && armory.inside.Count > 1)
                {
                    _sim.DeployWarriors(armory, _p.ironWeapons > 0 ? UnitType.WarriorIron : UnitType.WarriorRock, Mathf.Min(weapons, armory.inside.Count - 1));
                }
            }
            // remaining idle peons gather: mostly wood, some rock, a little iron
            foreach (var u in idlePeons)
            {
                if (u.carrying || u.state != UnitState.Idle) continue;   // may have been given a build / enter order above
                int wood = 0, rock = 0, iron = 0;
                foreach (var q in peons)
                    if (q.state == UnitState.Gathering || q.state == UnitState.Delivering)
                    {
                        if (q.lastGatherKind == SupplyKind.Wood) wood++;
                        else if (q.lastGatherKind == SupplyKind.Rock) rock++;
                        else iron++;
                    }
                var kind = rock * 2 < wood ? SupplyKind.Rock : (iron * 4 < wood && _difficulty != AIDifficulty.Easy ? SupplyKind.Iron : SupplyKind.Wood);
                var s = _sim.world.FindNearestSupply(kind, u.pos, 120f) ?? _sim.world.FindNearestSupply(SupplyKind.Wood, u.pos, 200f);
                if (s != null) _sim.CommandGather(new[] { u }, s);
            }

            // 3. army
            var warriors = _p.units.FindAll(u => u.IsWarrior && u.alive);
            var idleWarriors = warriors.FindAll(u => u.state == UnitState.Idle);
            if (warriors.Count >= _waveSize && _attackTimer <= 0)
            {
                var target = FindTarget();
                if (target != null)
                {
                    _sim.CommandMove(warriors, target.pos, attackMove: true);
                    _attackTimer = _difficulty == AIDifficulty.Hard ? 20f : 35f;
                }
            }
            else if (idleWarriors.Count > 0 && quarters != null)
            {
                // rally near home
                var home = quarters.pos + new Vector2(0, -14f);
                foreach (var w in idleWarriors)
                    if ((w.pos - home).sqrMagnitude > 25f * 25f) _sim.CommandMove(new[] { w }, home, attackMove: true);
            }
        }

        int CountBuilders(Building b)
        {
            int n = 0;
            foreach (var u in _p.units) if (u.state == UnitState.Building && u.buildTarget == b) n++;
            return n;
        }

        Building TryBuild(BuildingType type, List<Unit> peons, int builders)
        {
            var near = GameWorld.CellOf(_p.start);
            if (type == BuildingType.Tower)
            {
                var enemy = FindTarget();
                if (enemy != null) near = GameWorld.CellOf(Vector2.Lerp(_p.start, enemy.pos, 0.15f));
            }
            if (!_sim.world.FindBuildSite(near, Rules.Size(type), 60, out var site)) return null;
            var b = _sim.PlaceBuilding(_p, type, site);
            if (b != null) _sim.CommandBuild(Take(peons, builders), b);
            return b;
        }

        static List<Unit> Take(List<Unit> pool, int n)
        {
            var r = new List<Unit>();
            for (int i = pool.Count - 1; i >= 0 && r.Count < n; i--)
            {
                r.Add(pool[i]);
                pool.RemoveAt(i);
            }
            return r;
        }

        Entity FindTarget()
        {
            Entity best = null;
            float bestD = float.MaxValue;
            foreach (var other in _sim.players)
            {
                if (!_p.IsEnemy(other) || other.defeated) continue;
                foreach (var b in other.buildings)
                {
                    float d = (b.pos - _p.start).sqrMagnitude;
                    if (d < bestD) { bestD = d; best = b; }
                }
                if (best == null)
                    foreach (var u in other.units)
                    {
                        float d = (u.pos - _p.start).sqrMagnitude;
                        if (d < bestD) { bestD = d; best = u; }
                    }
            }
            return best;
        }
    }
}
