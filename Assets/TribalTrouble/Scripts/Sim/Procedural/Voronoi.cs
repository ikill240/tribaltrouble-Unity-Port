// Ported from Java (Tribal Trouble 1.4.1). Mechanically transliterated by PortTools/j2cs.py,
// then reviewed by hand. Java identifiers are kept so this file diffs against its source.
using System;
using System.Collections.Generic;
using TribalTrouble.Sim.Determinism;
using TribalTrouble.Sim;

namespace TribalTrouble.Sim.Procedural
{
    /*
     * TODO
     *
     * omskriv if-check ved modulo til (x+n)%n
     */

    public sealed class Voronoi {
        // coordinate indexing
        public const int X = 0;
        public const int Y = 1;
        public const int SEED = 2;

        private readonly int size;
        private readonly JavaRandom random;
        private readonly Channel dist1;
        private readonly Channel dist2;
        private readonly Channel dist3;
        private readonly Channel hit;

        public Voronoi(int size, int x_domains, int y_domains, int checkradius, float randomness, long seed) : this(size, x_domains, y_domains, checkradius, randomness, seed, false) {
        }

        public Voronoi(int size, int x_domains, int y_domains, int checkradius, float randomness, long seed,
                bool border) {
            this.size = size;
            x_domains = J.Max(1, x_domains);
            y_domains = J.Max(1, y_domains);
            checkradius = J.Min(J.Max(1, checkradius), J.Max(x_domains, y_domains));
            random = new JavaRandom(seed);
            dist1 = new Channel(size, size);
            dist2 = new Channel(size, size);
            dist3 = new Channel(size, size);
            hit = new Channel(size, size);

            // fill in hitpoints according to distribution
            float[][][] domains = J.New3<float>(x_domains, y_domains, 3);
            for (int j = 0; j < y_domains; j++) {
                for (int i = 0; i < x_domains; i++) {
                    domains[i][j][X] = (float)((float)(((float)(1 - randomness)) * ((float)(((float)(i + .5f)) / x_domains))) + (float)(randomness * ((float)(((float)(i + random.NextFloat())) / x_domains))));
                    domains[i][j][Y] = (float)((float)(((float)(1 - randomness)) * ((float)(((float)(j + .5f)) / y_domains))) + (float)(randomness * ((float)(((float)(j + random.NextFloat())) / y_domains))));
                    if (border && (j == 0 || j == y_domains - 1 || i == 0 || i == x_domains - 1)) {
                        domains[i][j][SEED] = 0f;
                    } else if (border && j != 0 && j != y_domains - 1 && i != 0 && i != x_domains - 1) {
                        domains[i][j][SEED] = 1f;
                    } else {
                        domains[i][j][SEED] = random.NextFloat();
                    }
                }
            }

            // fill in pixelvalues
            for (int y = 0; y < size; y++) {
                float y_coord = (float)((float) y / size);
                int j = J.I(((float)(y_coord * y_domains)));
                for (int x = 0; x < size; x++) {
                    float x_coord = (float)((float) x / size);
                    float d1 = float.MaxValue;
                    float d2 = float.MaxValue;
                    float d3 = float.MaxValue;
                    float dist = 0;
                    float hitpoint = 0;
                    int i = J.I(((float)(x_coord * x_domains)));

                    // traverse neighboring domains in wrap-around style (for seamless textures)
                    for (int l = -checkradius; l <= checkradius; l++) {
                        int l_wrap = j + l;
                        if (l_wrap < 0 || l_wrap >= y_domains) {
                            l_wrap = l_wrap % y_domains;
                            if (l_wrap < 0) l_wrap += y_domains;
                        }
                        for (int k = -checkradius; k <= checkradius; k++) {

                            // calculate wrapped domain coords
                            int k_wrap = i + k;
                            if (k_wrap < 0 || k_wrap >= x_domains) {
                                k_wrap = k_wrap % x_domains;
                                if (k_wrap < 0) k_wrap += x_domains;
                            }
                            float dx = 0;
                            float dy = 0;

                            // calculate distance to current hit point taking wrap-around into consideration
                            if (i + k >= 0 && i + k < x_domains) {
                                dx = J.Abs((float)(domains[k_wrap][l_wrap][X] - x_coord));
                            } else if (i + k < 0) {
                                dx = J.Abs((float)((float)(1 - domains[k_wrap][l_wrap][X]) + x_coord));
                            } else if (i + k >= x_domains) {
                                dx = J.Abs((float)((float)(1 - x_coord) + domains[k_wrap][l_wrap][X]));
                            }
                            if (j + l >= 0 && j + l < y_domains) {
                                dy = J.Abs((float)(domains[k_wrap][l_wrap][Y] - y_coord));
                            } else if (j + l < 0) {
                                dy = J.Abs((float)((float)(1 - domains[k_wrap][l_wrap][Y]) + y_coord));
                            } else if (j + l >= y_domains) {
                                dy = J.Abs((float)((float)(1 - y_coord) + domains[k_wrap][l_wrap][Y]));
                            }
                            dx = (float)(dx * (x_domains));
                            dy = (float)(dy * (y_domains));

                            // maintain F1, F2, F3 and nearest hitpoint values
                            dist = (float)((float)(dx * dx) + (float)(dy * dy));
                            if (dist <= d1) {
                                d3 = d2;
                                d2 = d1;
                                d1 = dist;
                                hitpoint = domains[k_wrap][l_wrap][SEED];
                            } else if (dist <= d2 && dist > d1) {
                                d3 = d2;
                                d2 = dist;
                            } else if (dist <= d3 && dist > d2) {
                                d3 = dist;
                            }
                        }
                    }
                    dist1.putPixel(x, y, d1);
                    dist2.putPixel(x, y, d2);
                    dist3.putPixel(x, y, d3);
                    hit.putPixel(x, y, hitpoint);
                }
            }
        }

        public Channel getDistance(float c1, float c2, float c3) {
            Channel channel = new Channel(size, size);
            for (int y = 0; y < size; y++) {
                for (int x = 0; x < size; x++) {
                    channel.putPixel(x, y, (float)((float)((float)(c1 * dist1.getPixel(x, y)) + (float)(c2 * dist2.getPixel(x, y))) + (float)(c3 * dist3.getPixel(x,
                            y))));
                }
            }
            return channel.dynamicRange();
        }

        public Channel getHitpoint() {
            return hit;
        }

    }
}
