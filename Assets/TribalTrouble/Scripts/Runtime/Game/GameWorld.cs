using System.Collections.Generic;
using UnityEngine;
using Landscape = TribalTrouble.Sim.Procedural.Landscape;

namespace TribalTrouble.Game
{
    public enum Race { Natives = 0, Vikings = 1 }
    public enum SupplyKind { Wood, Rock, Iron }

    /// <summary>A resource node (tree / rock / iron ore) on one grid cell.</summary>
    public sealed class Supply
    {
        public int id;
        public SupplyKind kind;
        public int gx, gy;
        public int remaining;
        public int visualIndex;   // which instance batch / variant it renders with
        public bool palm;         // second tree type (palm / pine)
        public bool Depleted => remaining <= 0;
    }

    /// <summary>
    /// The island as the game sees it: heights, walkable / buildable cells and resources, all taken
    /// straight from the ported Java generator (TribalTrouble.Sim.Procedural.Landscape).
    /// Grid cells are 2 m (HeightMap.METERS_PER_UNIT_GRID). Unity world = (x m, height, y m).
    /// </summary>
    public sealed class GameWorld
    {
        public const float MetersPerCell = 2f;

        public readonly Landscape landscape;
        public readonly Landscape.TerrainType terrain;
        public readonly int size;            // cells per side
        public readonly float meters;        // map side in metres
        public readonly float seaLevel;      // metres
        public readonly float[][] heights;   // [y][x] metres
        public readonly bool[,] walkable;    // [x,y] static walkability (land, gentle slope)
        public readonly sbyte[][] build;     // [y][x] building clearance from the generator
        public readonly int[,] blocker;      // [x,y] 0 = free, else entity id occupying the cell (building / supply)
        public readonly List<Supply> supplies = new List<Supply>();
        public readonly Supply[,] supplyAt;
        public readonly float[][] startLocations; // per player: x0,y0,x1,y1... (metres)
        int _nextSupplyId = 1;

        public GameWorld(Landscape landscape, Landscape.TerrainType terrain)
        {
            this.landscape = landscape;
            this.terrain = terrain;
            heights = landscape.getHeight();
            size = heights.Length;
            meters = size * MetersPerCell;
            seaLevel = landscape.getSeaLevelMeters();
            build = landscape.getBuildGrid();
            var access = landscape.getAccessGrid();
            walkable = new bool[size, size];
            blocker = new int[size, size];
            supplyAt = new Supply[size, size];
            for (int y = 0; y < size; y++)
                for (int x = 0; x < size; x++)
                    walkable[x, y] = access[y][x];

            AddSupplies(landscape.getTrees(), SupplyKind.Wood, 100, false);   // TreeSupply.INITIAL_SUPPLIES
            AddSupplies(landscape.getPalmtrees(), SupplyKind.Wood, 100, true);
            AddSupplies(landscape.getRock(), SupplyKind.Rock, 45);            // RockSupply.INITIAL_SUPPLIES
            AddSupplies(landscape.getIron(), SupplyKind.Iron, 25);            // IronSupply.INITIAL_SUPPLIES
            startLocations = landscape.getStartingLocations();
        }

        void AddSupplies(List<int[]> cells, SupplyKind kind, int amount, bool palm = false)
        {
            int variant = 0;
            foreach (var c in cells)
            {
                int x = c[0], y = c[1];
                if (x < 0 || y < 0 || x >= size || y >= size) continue;
                var s = new Supply { id = _nextSupplyId++, kind = kind, gx = x, gy = y, remaining = amount, palm = palm, visualIndex = variant++ };
                supplies.Add(s);
                supplyAt[x, y] = s;
                blocker[x, y] = -s.id;
            }
        }

        public void RemoveSupply(Supply s)
        {
            if (supplyAt[s.gx, s.gy] == s)
            {
                supplyAt[s.gx, s.gy] = null;
                if (blocker[s.gx, s.gy] == -s.id) blocker[s.gx, s.gy] = 0;
            }
        }

        public bool InBounds(int x, int y) => x >= 0 && y >= 0 && x < size && y < size;

        /// <summary>Can a unit stand on / walk through this cell?</summary>
        public bool IsPassable(int x, int y) => InBounds(x, y) && walkable[x, y] && blocker[x, y] == 0;

        public float HeightAt(float mx, float my)
        {
            float fx = Mathf.Clamp(mx / MetersPerCell, 0, size - 1.001f);
            float fy = Mathf.Clamp(my / MetersPerCell, 0, size - 1.001f);
            int x0 = (int)fx, y0 = (int)fy;
            float tx = fx - x0, ty = fy - y0;
            float h00 = heights[y0][x0], h10 = heights[y0][x0 + 1], h01 = heights[y0 + 1][x0], h11 = heights[y0 + 1][x0 + 1];
            return Mathf.Lerp(Mathf.Lerp(h00, h10, tx), Mathf.Lerp(h01, h11, tx), ty);
        }

        public Vector3 ToWorld(Vector2 m) => new Vector3(m.x, Mathf.Max(HeightAt(m.x, m.y), seaLevel - 0.3f), m.y);
        public static Vector2 CellCenter(int x, int y) => new Vector2((x + 0.5f) * MetersPerCell, (y + 0.5f) * MetersPerCell);
        public static Vector2Int CellOf(Vector2 m) => new Vector2Int(Mathf.FloorToInt(m.x / MetersPerCell), Mathf.FloorToInt(m.y / MetersPerCell));

        /// <summary>Footprint radius (cells) of a building of the given RacesResources size.</summary>
        public static int FootprintRadius(int buildingSize) => Mathf.Max(1, buildingSize - 2);

        public bool CanPlaceBuilding(int cx, int cy, int buildingSize)
        {
            if (!InBounds(cx, cy) || build[cy][cx] < buildingSize) return false;
            int r = FootprintRadius(buildingSize);
            for (int y = cy - r; y <= cy + r; y++)
                for (int x = cx - r; x <= cx + r; x++)
                    if (!InBounds(x, y) || !walkable[x, y] || blocker[x, y] != 0) return false;
            return true;
        }

        public void SetFootprint(int cx, int cy, int buildingSize, int entityId)
        {
            int r = FootprintRadius(buildingSize);
            for (int y = cy - r; y <= cy + r; y++)
                for (int x = cx - r; x <= cx + r; x++)
                    if (InBounds(x, y)) blocker[x, y] = entityId;
        }

        /// <summary>Spiral search for the nearest valid building site.</summary>
        public bool FindBuildSite(Vector2Int near, int buildingSize, int maxRadius, out Vector2Int site)
        {
            for (int r = 0; r <= maxRadius; r++)
            {
                for (int dy = -r; dy <= r; dy++)
                    for (int dx = -r; dx <= r; dx++)
                    {
                        if (Mathf.Max(Mathf.Abs(dx), Mathf.Abs(dy)) != r) continue;
                        int x = near.x + dx, y = near.y + dy;
                        if (CanPlaceBuilding(x, y, buildingSize)) { site = new Vector2Int(x, y); return true; }
                    }
            }
            site = default;
            return false;
        }

        /// <summary>Nearest passable cell to a target (spiral), for spawning / unblocking.</summary>
        public Vector2Int NearestPassable(Vector2Int c, int maxRadius = 30)
        {
            for (int r = 0; r <= maxRadius; r++)
                for (int dy = -r; dy <= r; dy++)
                    for (int dx = -r; dx <= r; dx++)
                    {
                        if (Mathf.Max(Mathf.Abs(dx), Mathf.Abs(dy)) != r) continue;
                        if (IsPassable(c.x + dx, c.y + dy)) return new Vector2Int(c.x + dx, c.y + dy);
                    }
            return c;
        }

        public Supply FindNearestSupply(SupplyKind kind, Vector2 from, float maxMeters = 400f)
        {
            Supply best = null;
            float bestD = maxMeters * maxMeters;
            foreach (var s in supplies)
            {
                if (s.kind != kind || s.Depleted) continue;
                float d = (CellCenter(s.gx, s.gy) - from).sqrMagnitude;
                if (d < bestD) { bestD = d; best = s; }
            }
            return best;
        }
    }
}
