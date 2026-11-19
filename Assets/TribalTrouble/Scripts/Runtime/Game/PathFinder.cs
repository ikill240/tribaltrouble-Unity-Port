using System.Collections.Generic;
using UnityEngine;

namespace TribalTrouble.Game
{
    /// <summary>
    /// Grid A* (8 directions, straight cost 2 / diagonal 3 like TT's GridPathFinder, no corner cutting).
    /// Stops when it reaches the goal cell, or any passable cell within goalRadius of it (so units can
    /// path to a tree, rock or building that itself blocks its cell). Node budget keeps it bounded.
    /// </summary>
    public sealed class PathFinder
    {
        readonly GameWorld _world;
        readonly int _size;
        readonly int[] _g;
        readonly int[] _parent;
        readonly int[] _stamp;
        readonly bool[] _closed;
        int _search;
        readonly List<int> _heap = new List<int>();
        readonly int[] _f;
        static readonly int[] DX = { 1, -1, 0, 0, 1, 1, -1, -1 };
        static readonly int[] DY = { 0, 0, 1, -1, 1, -1, 1, -1 };

        public PathFinder(GameWorld world)
        {
            _world = world;
            _size = world.size;
            int n = _size * _size;
            _g = new int[n];
            _f = new int[n];
            _parent = new int[n];
            _stamp = new int[n];
            _closed = new bool[n];
        }

        static int Octile(int dx, int dy)
        {
            dx = Mathf.Abs(dx); dy = Mathf.Abs(dy);
            return 2 * Mathf.Max(dx, dy) + Mathf.Min(dx, dy);
        }

        public List<Vector2Int> Find(Vector2Int start, Vector2Int goal, int goalRadius = 0, int maxNodes = 30000)
        {
            _search++;
            if (_search == int.MaxValue) { System.Array.Clear(_stamp, 0, _stamp.Length); _search = 1; }
            _heap.Clear();
            if (!_world.InBounds(start.x, start.y)) return null;
            int startIdx = start.y * _size + start.x;
            Open(startIdx, 0, Octile(goal.x - start.x, goal.y - start.y), -1);
            int best = startIdx, bestH = int.MaxValue;
            int expanded = 0;
            while (_heap.Count > 0)
            {
                int cur = Pop();
                if (_closed[cur] && _stamp[cur] == _search) continue;
                _closed[cur] = true;
                int cx = cur % _size, cy = cur / _size;
                int h = Octile(goal.x - cx, goal.y - cy);
                if (h < bestH) { bestH = h; best = cur; }
                bool atGoal = (cx == goal.x && cy == goal.y) ||
                              (goalRadius > 0 && Mathf.Max(Mathf.Abs(cx - goal.x), Mathf.Abs(cy - goal.y)) <= goalRadius);
                if (atGoal) return Build(cur);
                if (++expanded > maxNodes) break;
                for (int d = 0; d < 8; d++)
                {
                    int nx = cx + DX[d], ny = cy + DY[d];
                    if (!_world.IsPassable(nx, ny)) continue;
                    if (d >= 4 && (!_world.IsPassable(cx + DX[d], cy) || !_world.IsPassable(cx, cy + DY[d]))) continue;
                    int ni = ny * _size + nx;
                    if (_stamp[ni] == _search && _closed[ni]) continue;
                    int g = _g[cur] + (d < 4 ? 2 : 3);
                    if (_stamp[ni] != _search || g < _g[ni])
                        Open(ni, g, g + Octile(goal.x - nx, goal.y - ny), cur);
                }
            }
            // unreachable: walk as close as we got
            return best != startIdx ? Build(best) : null;
        }

        void Open(int idx, int g, int f, int parent)
        {
            if (_stamp[idx] != _search) { _stamp[idx] = _search; _closed[idx] = false; }
            _g[idx] = g;
            _f[idx] = f;
            _parent[idx] = parent;
            _heap.Add(idx);
            int i = _heap.Count - 1;
            while (i > 0)
            {
                int p = (i - 1) >> 1;
                if (_f[_heap[p]] <= _f[_heap[i]]) break;
                (_heap[p], _heap[i]) = (_heap[i], _heap[p]);
                i = p;
            }
        }

        int Pop()
        {
            int top = _heap[0];
            int last = _heap[_heap.Count - 1];
            _heap.RemoveAt(_heap.Count - 1);
            if (_heap.Count > 0)
            {
                _heap[0] = last;
                int i = 0;
                while (true)
                {
                    int l = 2 * i + 1, r = l + 1, m = i;
                    if (l < _heap.Count && _f[_heap[l]] < _f[_heap[m]]) m = l;
                    if (r < _heap.Count && _f[_heap[r]] < _f[_heap[m]]) m = r;
                    if (m == i) break;
                    (_heap[m], _heap[i]) = (_heap[i], _heap[m]);
                    i = m;
                }
            }
            return top;
        }

        List<Vector2Int> Build(int end)
        {
            var path = new List<Vector2Int>();
            for (int i = end; i != -1; i = _parent[i])
                path.Add(new Vector2Int(i % _size, i / _size));
            path.Reverse();
            return path;
        }
    }
}
