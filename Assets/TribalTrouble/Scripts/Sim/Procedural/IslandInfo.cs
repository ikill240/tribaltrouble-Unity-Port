// Ported from Java (Tribal Trouble 1.4.1). Mechanically transliterated by PortTools/j2cs.py,
// then reviewed by hand. Java identifiers are kept so this file diffs against its source.
using System;
using System.Collections.Generic;
using TribalTrouble.Sim.Determinism;
using TribalTrouble.Sim;

namespace TribalTrouble.Sim.Procedural
{
    public class IslandInfo {
        private int id_;
        private int area_;
        private int trees_;
        private int rocks_;
        private int iron_;
        private int min_x;
        private int min_y;
        private int max_x;
        private int max_y;
        private int start_x;
        private int start_y;

        public IslandInfo(int id, int area, int min_x, int min_y, int max_x, int max_y, int start_x, int start_y) {
            this.id_ = id;
            this.area_ = area;
            this.min_x = min_x;
            this.min_y = min_y;
            this.max_x = max_x;
            this.max_y = max_y;
            this.start_x = start_x;
            this.start_y = start_y;
            this.rocks_ = 0;
            this.iron_ = 0;
            this.trees_ = 0;
        }

        public void setTrees(int trees) {
            this.trees_ = trees;
        }

        public void setIron(int iron) {
            this.iron_ = iron;
        }

        public void setRocks(int rocks) {
            this.rocks_ = rocks;
        }

        public int rocks() {
            return rocks_;
        }

        public int iron() {
            return iron_;
        }

        public int trees() {
            return trees_;
        }

        public int minX() {
            return min_x;
        }

        public int minY() {
            return min_y;
        }

        public int maxX() {
            return max_x;
        }

        public int maxY() {
            return max_y;
        }

        public int centerX() {
            return (min_x + max_x) / 2;
        }

        public int centerY() {
            return (min_y + max_y) / 2;
        }

        public int startX() {
            return start_x;
        }

        public int startY() {
            return start_y;
        }

        public int area() {
            return area_;
        }

        public int id() {
            return id_;
        }
    }
}
