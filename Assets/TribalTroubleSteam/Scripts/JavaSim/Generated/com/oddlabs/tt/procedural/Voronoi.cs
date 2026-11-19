// Translated from Voronoi.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.procedural
{
    // Java: 14
    public class Voronoi : java.lang.Object
    {
        public const int X = 0;
        public const int Y = 1;
        public const int SEED = 2;
        public int size;
        public java.util.Random random;
        public com.oddlabs.procedural.Channel dist1;
        public com.oddlabs.procedural.Channel dist2;
        public com.oddlabs.procedural.Channel dist3;
        public com.oddlabs.procedural.Channel hit;
        // Java: 131
        public com.oddlabs.procedural.Channel getDistance(float c1, float c2, float c3)
        {
            com.oddlabs.procedural.Channel channel = new com.oddlabs.procedural.Channel(size, size);
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    channel.putPixel(x, y, (float)((float)((float)(c1 * dist1.getPixel(x, y)) + (float)(c2 * dist2.getPixel(x, y))) + (float)(c3 * dist3.getPixel(x, y))));
                }
            }
            return channel.dynamicRange();
        }
        // Java: 142
        public com.oddlabs.procedural.Channel getHitpoint()
        {
            return hit;
        }
        public Voronoi(int size, int x_domains, int y_domains, int checkradius, float randomness, long seed) : this(size, x_domains, y_domains, checkradius, randomness, seed, false)
        {
        }
        public Voronoi(int size, int x_domains, int y_domains, int checkradius, float randomness, long seed, bool border) : base()
        {
            this.size = size;
            x_domains = java.lang.Math.max(1, x_domains);
            y_domains = java.lang.Math.max(1, y_domains);
            checkradius = java.lang.Math.min(java.lang.Math.max(1, checkradius), java.lang.Math.max(x_domains, y_domains));
            random = new java.util.Random(seed);
            dist1 = new com.oddlabs.procedural.Channel(size, size);
            dist2 = new com.oddlabs.procedural.Channel(size, size);
            dist3 = new com.oddlabs.procedural.Channel(size, size);
            hit = new com.oddlabs.procedural.Channel(size, size);
            float[][][] domains = java.lang.JA.newArray3<float>(x_domains, y_domains, 3);
            for (int j = 0; j < y_domains; j++)
            {
                for (int i = 0; i < x_domains; i++)
                {
                    domains[i][j][com.oddlabs.tt.procedural.Voronoi.X] = (float)((float)(((float)(1 - randomness)) * ((float)(((float)(i + 0.5f)) / x_domains))) + (float)(randomness * ((float)(((float)(i + random.nextFloat())) / x_domains))));
                    domains[i][j][com.oddlabs.tt.procedural.Voronoi.Y] = (float)((float)(((float)(1 - randomness)) * ((float)(((float)(j + 0.5f)) / y_domains))) + (float)(randomness * ((float)(((float)(j + random.nextFloat())) / y_domains))));
                    if (border && (j == 0 || j == y_domains - 1 || i == 0 || i == x_domains - 1))
                    {
                        domains[i][j][com.oddlabs.tt.procedural.Voronoi.SEED] = 0.0f;
                    }
                    else if (border && j != 0 && j != y_domains - 1 && i != 0 && i != x_domains - 1)
                    {
                        domains[i][j][com.oddlabs.tt.procedural.Voronoi.SEED] = 1.0f;
                    }
                    else
                    {
                        domains[i][j][com.oddlabs.tt.procedural.Voronoi.SEED] = random.nextFloat();
                    }
                }
            }
            for (int y = 0; y < size; y++)
            {
                float y_coord = (float)(((float)(y)) / size);
                int j_2 = java.lang.JC.f2i(((float)(y_coord * y_domains)));
                for (int x = 0; x < size; x++)
                {
                    float x_coord = (float)(((float)(x)) / size);
                    float d1 = java.lang.Float.MAX_VALUE;
                    float d2 = java.lang.Float.MAX_VALUE;
                    float d3 = java.lang.Float.MAX_VALUE;
                    float dist = 0;
                    float hitpoint = 0;
                    int i_2 = java.lang.JC.f2i(((float)(x_coord * x_domains)));
                    for (int l = -checkradius; l <= checkradius; l++)
                    {
                        int l_wrap = j_2 + l;
                        if (l_wrap < 0 || l_wrap >= y_domains)
                        {
                            l_wrap = l_wrap % y_domains;
                            if (l_wrap < 0)
                            {
                                l_wrap += y_domains;
                            }
                        }
                        for (int k = -checkradius; k <= checkradius; k++)
                        {
                            int k_wrap = i_2 + k;
                            if (k_wrap < 0 || k_wrap >= x_domains)
                            {
                                k_wrap = k_wrap % x_domains;
                                if (k_wrap < 0)
                                {
                                    k_wrap += x_domains;
                                }
                            }
                            float dx = 0;
                            float dy = 0;
                            if (i_2 + k >= 0 && i_2 + k < x_domains)
                            {
                                dx = java.lang.Math.abs((float)(domains[k_wrap][l_wrap][com.oddlabs.tt.procedural.Voronoi.X] - x_coord));
                            }
                            else if (i_2 + k < 0)
                            {
                                dx = java.lang.Math.abs((float)((float)(1 - domains[k_wrap][l_wrap][com.oddlabs.tt.procedural.Voronoi.X]) + x_coord));
                            }
                            else if (i_2 + k >= x_domains)
                            {
                                dx = java.lang.Math.abs((float)((float)(1 - x_coord) + domains[k_wrap][l_wrap][com.oddlabs.tt.procedural.Voronoi.X]));
                            }
                            if (j_2 + l >= 0 && j_2 + l < y_domains)
                            {
                                dy = java.lang.Math.abs((float)(domains[k_wrap][l_wrap][com.oddlabs.tt.procedural.Voronoi.Y] - y_coord));
                            }
                            else if (j_2 + l < 0)
                            {
                                dy = java.lang.Math.abs((float)((float)(1 - domains[k_wrap][l_wrap][com.oddlabs.tt.procedural.Voronoi.Y]) + y_coord));
                            }
                            else if (j_2 + l >= y_domains)
                            {
                                dy = java.lang.Math.abs((float)((float)(1 - y_coord) + domains[k_wrap][l_wrap][com.oddlabs.tt.procedural.Voronoi.Y]));
                            }
                            dx = (float)(dx * x_domains);
                            dy = (float)(dy * y_domains);
                            dist = (float)((float)(dx * dx) + (float)(dy * dy));
                            if (dist <= d1)
                            {
                                d3 = d2;
                                d2 = d1;
                                d1 = dist;
                                hitpoint = domains[k_wrap][l_wrap][com.oddlabs.tt.procedural.Voronoi.SEED];
                            }
                            else if (dist <= d2 && dist > d1)
                            {
                                d3 = d2;
                                d2 = dist;
                            }
                            else if (dist <= d3 && dist > d2)
                            {
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
    }
}
