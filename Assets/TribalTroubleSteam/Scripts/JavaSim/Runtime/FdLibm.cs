// java.lang.FdLibm from OpenJDK 25 (GPLv2 + Classpath Exception), translated by J2CS. Gives StrictMath bit-exact results on every platform.
// Translated from FdLibm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace j2cs.fdlibm
{
    // Java: 60
    public class FdLibm : java.lang.Object
    {
        public const double INFINITY = double.PositiveInfinity;
        public const double TWO24 = 1.6777216E7d;
        public const double TWO54 = 1.8014398509481984E16d;
        public const double HUGE = 1.0E300d;
        public const int SIGN_BIT = (-2147483648);
        public const int EXP_BITS = 2146435072;
        public const int EXP_SIGNIF_BITS = 2147483647;
        // Java: 85
        public static int __LO(double x)
        {
            long transducer = java.lang.Double.doubleToRawLongBits(x);
            return unchecked((int)(transducer));
        }
        // Java: 94
        public static double __LO(double x, int low)
        {
            long transX = java.lang.Double.doubleToRawLongBits(x);
            return java.lang.Double.longBitsToDouble((transX & (-4294967296L)) | (low & 4294967295L));
        }
        // Java: 103
        public static int __HI(double x)
        {
            long transducer = java.lang.Double.doubleToRawLongBits(x);
            return unchecked((int)((transducer >> 32)));
        }
        // Java: 112
        public static double __HI(double x, int high)
        {
            long transX = java.lang.Double.doubleToRawLongBits(x);
            return java.lang.Double.longBitsToDouble((transX & 4294967295L) | ((((long)(high)))) << 32);
        }
        // Java: 122
        public static double __HI_LO(int high, int low)
        {
            return java.lang.Double.longBitsToDouble((((long)(high)) << 32) | (low & 4294967295L));
        }
        // Java: 157
        public class Sin : java.lang.Object
        {
            // Java: 160
            public static double compute(double x)
            {
                double[] y = new double[2];
                double z = 0.0d;
                int n = default;
                int ix = default;
                ix = j2cs.fdlibm.FdLibm.__HI(x);
                ix &= j2cs.fdlibm.FdLibm.EXP_SIGNIF_BITS;
                if (ix <= 1072243195)
                {
                    return j2cs.fdlibm.FdLibm.Sin.__kernel_sin(x, z, 0);
                }
                else if (ix >= j2cs.fdlibm.FdLibm.EXP_BITS)
                {
                    return x - x;
                }
                else
                {
                    n = j2cs.fdlibm.FdLibm.RemPio2.__ieee754_rem_pio2(x, y);
                    switch ((n & 3))
                    {
                        case 0:
                        {
                            return j2cs.fdlibm.FdLibm.Sin.__kernel_sin(y[0], y[1], 1);
                        }
                        case 1:
                        {
                            return j2cs.fdlibm.FdLibm.Cos.__kernel_cos(y[0], y[1]);
                        }
                        case 2:
                        {
                            return -j2cs.fdlibm.FdLibm.Sin.__kernel_sin(y[0], y[1], 1);
                        }
                        default:
                        {
                            return -j2cs.fdlibm.FdLibm.Cos.__kernel_cos(y[0], y[1]);
                        }
                    }
                }
            }
            public const double S1 = (-0.16666666666666632d);
            public const double S2 = 0.00833333333332249d;
            public const double S3 = (-1.984126982985795E-4d);
            public const double S4 = 2.7557313707070068E-6d;
            public const double S5 = (-2.5050760253406863E-8d);
            public const double S6 = 1.58969099521155E-10d;
            // Java: 221
            public static double __kernel_sin(double x, double y, int iy)
            {
                double z = default;
                double r = default;
                double v = default;
                int ix = default;
                ix = j2cs.fdlibm.FdLibm.__HI(x) & j2cs.fdlibm.FdLibm.EXP_SIGNIF_BITS;
                if (ix < 1044381696)
                {
                    if (java.lang.JC.d2i(x) == 0)
                    {
                        return x;
                    }
                }
                z = x * x;
                v = z * x;
                r = j2cs.fdlibm.FdLibm.Sin.S2 + z * (j2cs.fdlibm.FdLibm.Sin.S3 + z * (j2cs.fdlibm.FdLibm.Sin.S4 + z * (j2cs.fdlibm.FdLibm.Sin.S5 + z * j2cs.fdlibm.FdLibm.Sin.S6)));
                if (iy == 0)
                {
                    return x + v * (j2cs.fdlibm.FdLibm.Sin.S1 + z * r);
                }
                else
                {
                    return x - ((z * (0.5d * y - v * r) - y) - v * j2cs.fdlibm.FdLibm.Sin.S1);
                }
            }
            public Sin() : base()
            {
                throw new System.NotSupportedException();
            }
        }
        // Java: 270
        public class Cos : java.lang.Object
        {
            // Java: 273
            public static double compute(double x)
            {
                double[] y = new double[2];
                double z = 0.0d;
                int n = default;
                int ix = default;
                ix = j2cs.fdlibm.FdLibm.__HI(x);
                ix &= j2cs.fdlibm.FdLibm.EXP_SIGNIF_BITS;
                if (ix <= 1072243195)
                {
                    return j2cs.fdlibm.FdLibm.Cos.__kernel_cos(x, z);
                }
                else if (ix >= j2cs.fdlibm.FdLibm.EXP_BITS)
                {
                    return x - x;
                }
                else
                {
                    n = j2cs.fdlibm.FdLibm.RemPio2.__ieee754_rem_pio2(x, y);
                    switch ((n & 3))
                    {
                        case 0:
                        {
                            return j2cs.fdlibm.FdLibm.Cos.__kernel_cos(y[0], y[1]);
                        }
                        case 1:
                        {
                            return -j2cs.fdlibm.FdLibm.Sin.__kernel_sin(y[0], y[1], 1);
                        }
                        case 2:
                        {
                            return -j2cs.fdlibm.FdLibm.Cos.__kernel_cos(y[0], y[1]);
                        }
                        default:
                        {
                            return j2cs.fdlibm.FdLibm.Sin.__kernel_sin(y[0], y[1], 1);
                        }
                    }
                }
            }
            public const double C1 = 0.0416666666666666d;
            public const double C2 = (-0.001388888888887411d);
            public const double C3 = 2.480158728947673E-5d;
            public const double C4 = (-2.7557314351390663E-7d);
            public const double C5 = 2.087572321298175E-9d;
            public const double C6 = (-1.1359647557788195E-11d);
            // Java: 341
            public static double __kernel_cos(double x, double y)
            {
                double a = default;
                double hz = default;
                double z = default;
                double r = default;
                double qx = 0.0d;
                int ix = default;
                ix = j2cs.fdlibm.FdLibm.__HI(x) & j2cs.fdlibm.FdLibm.EXP_SIGNIF_BITS;
                if (ix < 1044381696)
                {
                    if ((java.lang.JC.d2i(x)) == 0)
                    {
                        return 1.0d;
                    }
                }
                z = x * x;
                r = z * (j2cs.fdlibm.FdLibm.Cos.C1 + z * (j2cs.fdlibm.FdLibm.Cos.C2 + z * (j2cs.fdlibm.FdLibm.Cos.C3 + z * (j2cs.fdlibm.FdLibm.Cos.C4 + z * (j2cs.fdlibm.FdLibm.Cos.C5 + z * j2cs.fdlibm.FdLibm.Cos.C6)))));
                if (ix < 1070805811)
                {
                    return 1.0d - (0.5d * z - (z * r - x * y));
                }
                else
                {
                    if (ix > 1072234496)
                    {
                        qx = 0.28125d;
                    }
                    else
                    {
                        qx = j2cs.fdlibm.FdLibm.__HI_LO(ix - 2097152, 0);
                    }
                    hz = 0.5d * z - qx;
                    a = 1.0d - qx;
                    return a - (hz - (z * r - x * y));
                }
            }
            public Cos() : base()
            {
                throw new System.NotSupportedException();
            }
        }
        // Java: 396
        public class Tan : java.lang.Object
        {
            // Java: 399
            public static double compute(double x)
            {
                double[] y = new double[2];
                double z = 0.0d;
                int n = default;
                int ix = default;
                ix = j2cs.fdlibm.FdLibm.__HI(x);
                ix &= j2cs.fdlibm.FdLibm.EXP_SIGNIF_BITS;
                if (ix <= 1072243195)
                {
                    return j2cs.fdlibm.FdLibm.Tan.__kernel_tan(x, z, 1);
                }
                else if (ix >= j2cs.fdlibm.FdLibm.EXP_BITS)
                {
                    return x - x;
                }
                else
                {
                    n = j2cs.fdlibm.FdLibm.RemPio2.__ieee754_rem_pio2(x, y);
                    return j2cs.fdlibm.FdLibm.Tan.__kernel_tan(y[0], y[1], 1 - ((n & 1) << 1));
                }
            }
            public const double pio4 = 0.7853981633974483d;
            public const double pio4lo = 3.061616997868383E-17d;
            public static double[] T;
            // Java: 471
            public static double __kernel_tan(double x, double y, int iy)
            {
                double z = default;
                double r = default;
                double v = default;
                double w = default;
                double s = default;
                int ix = default;
                int hx = default;
                hx = j2cs.fdlibm.FdLibm.__HI(x);
                ix = hx & j2cs.fdlibm.FdLibm.EXP_SIGNIF_BITS;
                if (ix < 1043333120)
                {
                    if (java.lang.JC.d2i(x) == 0)
                    {
                        if (((ix | j2cs.fdlibm.FdLibm.__LO(x)) | (iy + 1)) == 0)
                        {
                            return 1.0d / java.lang.Math.abs(x);
                        }
                        else
                        {
                            if (iy == 1)
                            {
                                return x;
                            }
                            else
                            {
                                double a = default;
                                double t = default;
                                z = w = x + y;
                                z = j2cs.fdlibm.FdLibm.__LO(z, 0);
                                v = y - (z - x);
                                t = a = -1.0d / w;
                                t = j2cs.fdlibm.FdLibm.__LO(t, 0);
                                s = 1.0d + t * z;
                                return t + a * (s + t * v);
                            }
                        }
                    }
                }
                if (ix >= 1072010280)
                {
                    if (hx < 0)
                    {
                        x = -x;
                        y = -y;
                    }
                    z = j2cs.fdlibm.FdLibm.Tan.pio4 - x;
                    w = j2cs.fdlibm.FdLibm.Tan.pio4lo - y;
                    x = z + w;
                    y = 0.0d;
                }
                z = x * x;
                w = z * z;
                r = j2cs.fdlibm.FdLibm.Tan.T[1] + w * (j2cs.fdlibm.FdLibm.Tan.T[3] + w * (j2cs.fdlibm.FdLibm.Tan.T[5] + w * (j2cs.fdlibm.FdLibm.Tan.T[7] + w * (j2cs.fdlibm.FdLibm.Tan.T[9] + w * j2cs.fdlibm.FdLibm.Tan.T[11]))));
                v = z * (j2cs.fdlibm.FdLibm.Tan.T[2] + w * (j2cs.fdlibm.FdLibm.Tan.T[4] + w * (j2cs.fdlibm.FdLibm.Tan.T[6] + w * (j2cs.fdlibm.FdLibm.Tan.T[8] + w * (j2cs.fdlibm.FdLibm.Tan.T[10] + w * j2cs.fdlibm.FdLibm.Tan.T[12])))));
                s = z * x;
                r = y + z * (s * (r + v) + y);
                r += j2cs.fdlibm.FdLibm.Tan.T[0] * s;
                w = x + r;
                if (ix >= 1072010280)
                {
                    v = ((double)(iy));
                    return ((double)((1 - ((hx >> 30) & 2)))) * (v - 2.0d * (x - (w * w / (w + v) - r)));
                }
                if (iy == 1)
                {
                    return w;
                }
                else
                {
                    double a_2 = default;
                    double t_2 = default;
                    z = w;
                    z = j2cs.fdlibm.FdLibm.__LO(z, 0);
                    v = r - (z - x);
                    t_2 = a_2 = -1.0d / w;
                    t_2 = j2cs.fdlibm.FdLibm.__LO(t_2, 0);
                    s = 1.0d + t_2 * z;
                    return t_2 + a_2 * (s + t_2 * v);
                }
            }
            static Tan()
            {
                Tan.T = new double[] { 0.3333333333333341d, 0.13333333333320124d, 0.05396825397622605d, 0.021869488294859542d, 0.0088632398235993d, 0.0035920791075913124d, 0.0014562094543252903d, 5.880412408202641E-4d, 2.464631348184699E-4d, 7.817944429395571E-5d, 7.140724913826082E-5d, -1.8558637485527546E-5d, 2.590730518636337E-5d };
            }
            public Tan() : base()
            {
                throw new System.NotSupportedException();
            }
        }
        // Java: 545
        public class RemPio2 : java.lang.Object
        {
            public static int[] two_over_pi;
            public static int[] npio2_hw;
            public const double invpio2 = 0.6366197723675814d;
            public const double pio2_1 = 1.5707963267341256d;
            public const double pio2_1t = 6.077100506506192E-11d;
            public const double pio2_2 = 6.077100506303966E-11d;
            public const double pio2_2t = 2.0222662487959506E-21d;
            public const double pio2_3 = 2.0222662487111665E-21d;
            public const double pio2_3t = 8.4784276603689E-32d;
            // Java: 591
            public static int __ieee754_rem_pio2(double x, double[] y)
            {
                double z = 0.0d;
                double w = default;
                double t = default;
                double r = default;
                double fn = default;
                double[] tx = new double[3];
                int e0 = default;
                int i = default;
                int j = default;
                int nx = default;
                int n = default;
                int ix = default;
                int hx = default;
                hx = j2cs.fdlibm.FdLibm.__HI(x);
                ix = hx & j2cs.fdlibm.FdLibm.EXP_SIGNIF_BITS;
                if (ix <= 1072243195)
                {
                    y[0] = x;
                    y[1] = 0;
                    return 0;
                }
                if (ix < 1073928572)
                {
                    if (hx > 0)
                    {
                        z = x - j2cs.fdlibm.FdLibm.RemPio2.pio2_1;
                        if (ix != 1073291771)
                        {
                            y[0] = z - j2cs.fdlibm.FdLibm.RemPio2.pio2_1t;
                            y[1] = (z - y[0]) - j2cs.fdlibm.FdLibm.RemPio2.pio2_1t;
                        }
                        else
                        {
                            z -= j2cs.fdlibm.FdLibm.RemPio2.pio2_2;
                            y[0] = z - j2cs.fdlibm.FdLibm.RemPio2.pio2_2t;
                            y[1] = (z - y[0]) - j2cs.fdlibm.FdLibm.RemPio2.pio2_2t;
                        }
                        return 1;
                    }
                    else
                    {
                        z = x + j2cs.fdlibm.FdLibm.RemPio2.pio2_1;
                        if (ix != 1073291771)
                        {
                            y[0] = z + j2cs.fdlibm.FdLibm.RemPio2.pio2_1t;
                            y[1] = (z - y[0]) + j2cs.fdlibm.FdLibm.RemPio2.pio2_1t;
                        }
                        else
                        {
                            z += j2cs.fdlibm.FdLibm.RemPio2.pio2_2;
                            y[0] = z + j2cs.fdlibm.FdLibm.RemPio2.pio2_2t;
                            y[1] = (z - y[0]) + j2cs.fdlibm.FdLibm.RemPio2.pio2_2t;
                        }
                        return (-1);
                    }
                }
                if (ix <= 1094263291)
                {
                    t = java.lang.Math.abs(x);
                    n = java.lang.JC.d2i((t * j2cs.fdlibm.FdLibm.RemPio2.invpio2 + 0.5d));
                    fn = ((double)(n));
                    r = t - fn * j2cs.fdlibm.FdLibm.RemPio2.pio2_1;
                    w = fn * j2cs.fdlibm.FdLibm.RemPio2.pio2_1t;
                    if (n < 32 && ix != j2cs.fdlibm.FdLibm.RemPio2.npio2_hw[n - 1])
                    {
                        y[0] = r - w;
                    }
                    else
                    {
                        j = ix >> 20;
                        y[0] = r - w;
                        i = j - (((j2cs.fdlibm.FdLibm.__HI(y[0])) >> 20) & 2047);
                        if (i > 16)
                        {
                            t = r;
                            w = fn * j2cs.fdlibm.FdLibm.RemPio2.pio2_2;
                            r = t - w;
                            w = fn * j2cs.fdlibm.FdLibm.RemPio2.pio2_2t - ((t - r) - w);
                            y[0] = r - w;
                            i = j - (((j2cs.fdlibm.FdLibm.__HI(y[0])) >> 20) & 2047);
                            if (i > 49)
                            {
                                t = r;
                                w = fn * j2cs.fdlibm.FdLibm.RemPio2.pio2_3;
                                r = t - w;
                                w = fn * j2cs.fdlibm.FdLibm.RemPio2.pio2_3t - ((t - r) - w);
                                y[0] = r - w;
                            }
                        }
                    }
                    y[1] = (r - y[0]) - w;
                    if (hx < 0)
                    {
                        y[0] = -y[0];
                        y[1] = -y[1];
                        return -n;
                    }
                    else
                    {
                        return n;
                    }
                }
                if (ix >= j2cs.fdlibm.FdLibm.EXP_BITS)
                {
                    y[0] = y[1] = x - x;
                    return 0;
                }
                z = j2cs.fdlibm.FdLibm.__LO(z, j2cs.fdlibm.FdLibm.__LO(x));
                e0 = (ix >> 20) - 1046;
                z = j2cs.fdlibm.FdLibm.__HI(z, ix - (e0 << 20));
                for (i = 0; i < 2; i++)
                {
                    tx[i] = ((double)((java.lang.JC.d2i((z)))));
                    z = (z - tx[i]) * j2cs.fdlibm.FdLibm.TWO24;
                }
                tx[2] = z;
                nx = 3;
                while (tx[nx - 1] == 0.0d)
                {
                    nx--;
                }
                n = j2cs.fdlibm.FdLibm.KernelRemPio2.__kernel_rem_pio2(tx, y, e0, nx, 2, j2cs.fdlibm.FdLibm.RemPio2.two_over_pi);
                if (hx < 0)
                {
                    y[0] = -y[0];
                    y[1] = -y[1];
                    return -n;
                }
                return n;
            }
            static RemPio2()
            {
                RemPio2.two_over_pi = new int[] { 10680707, 7228996, 1387004, 2578385, 16069853, 12639074, 9804092, 4427841, 16666979, 11263675, 12935607, 2387514, 4345298, 14681673, 3074569, 13734428, 16653803, 1880361, 10960616, 8533493, 3062596, 8710556, 7349940, 6258241, 3772886, 3769171, 3798172, 8675211, 12450088, 3874808, 9961438, 366607, 15675153, 9132554, 7151469, 3571407, 2607881, 12013382, 4155038, 6285869, 7677882, 13102053, 15825725, 473591, 9065106, 15363067, 6271263, 9264392, 5636912, 4652155, 7056368, 13614112, 10155062, 1944035, 9527646, 15080200, 6658437, 6231200, 6832269, 16767104, 5075751, 3212806, 1398474, 7579849, 6349435, 12618859 };
                RemPio2.npio2_hw = new int[] { 1073291771, 1074340347, 1074977148, 1075388923, 1075800698, 1076025724, 1076231611, 1076437499, 1076643386, 1076849274, 1076971356, 1077074300, 1077177244, 1077280187, 1077383131, 1077486075, 1077589019, 1077691962, 1077794906, 1077897850, 1077968460, 1078019932, 1078071404, 1078122876, 1078174348, 1078225820, 1078277292, 1078328763, 1078380235, 1078431707, 1078483179, 1078534651 };
            }
            public RemPio2() : base()
            {
            }
        }
        // Java: 801
        public class KernelRemPio2 : java.lang.Object
        {
            public static int[] init_jk;
            public static double[] PIo2;
            public const double twon24 = 5.960464477539063E-8d;
            // Java: 826
            public static int __kernel_rem_pio2(double[] x, double[] y, int e0, int nx, int prec, int[] ipio2)
            {
                int jz = default;
                int jx = default;
                int jv = default;
                int jp = default;
                int jk = default;
                int carry = default;
                int n = default;
                int i = default;
                int j = default;
                int k = default;
                int m = default;
                int q0 = default;
                int ih = default;
                int[] iq = new int[20];
                double z = default;
                double fw = default;
                double[] f = new double[20];
                double[] fq = new double[20];
                double[] q = new double[20];
                jk = j2cs.fdlibm.FdLibm.KernelRemPio2.init_jk[prec];
                jp = jk;
                jx = nx - 1;
                jv = (e0 - 3) / 24;
                if (jv < 0)
                {
                    jv = 0;
                }
                q0 = e0 - 24 * (jv + 1);
                j = jv - jx;
                m = jx + jk;
                for (i = 0; i <= m; i++, j++)
                {
                    f[i] = (j < 0 ? 0.0d : ((double)(ipio2[j])));
                }
                for (i = 0; i <= jk; i++)
                {
                    for (j = 0, fw = 0.0d; j <= jx; j++)
                    {
                        fw += x[j] * f[jx + i - j];
                    }
                    q[i] = fw;
                }
                jz = jk;
                while (true)
                {
                    for (i = 0, j = jz, z = q[jz]; j > 0; i++, j--)
                    {
                        fw = ((double)((java.lang.JC.d2i((j2cs.fdlibm.FdLibm.KernelRemPio2.twon24 * z)))));
                        iq[i] = java.lang.JC.d2i((z - j2cs.fdlibm.FdLibm.TWO24 * fw));
                        z = q[j - 1] + fw;
                    }
                    z = java.lang.Math.scalb(z, q0);
                    z -= 8.0d * java.lang.Math.floor(z * 0.125d);
                    n = java.lang.JC.d2i(z);
                    z -= ((double)(n));
                    ih = 0;
                    if (q0 > 0)
                    {
                        i = (iq[jz - 1] >> (24 - q0));
                        n += i;
                        iq[jz - 1] -= i << (24 - q0);
                        ih = iq[jz - 1] >> (23 - q0);
                    }
                    else if (q0 == 0)
                    {
                        ih = iq[jz - 1] >> 23;
                    }
                    else if (z >= 0.5d)
                    {
                        ih = 2;
                    }
                    if (ih > 0)
                    {
                        n += 1;
                        carry = 0;
                        for (i = 0; i < jz; i++)
                        {
                            j = iq[i];
                            if (carry == 0)
                            {
                                if (j != 0)
                                {
                                    carry = 1;
                                    iq[i] = 16777216 - j;
                                }
                            }
                            else
                            {
                                iq[i] = 16777215 - j;
                            }
                        }
                        if (q0 > 0)
                        {
                            switch ((q0))
                            {
                                case 1:
                                {
                                    iq[jz - 1] &= 8388607;
                                    break;
                                }
                                case 2:
                                {
                                    iq[jz - 1] &= 4194303;
                                    break;
                                }
                            }
                        }
                        if (ih == 2)
                        {
                            z = 1.0d - z;
                            if (carry != 0)
                            {
                                z -= java.lang.Math.scalb(1.0d, q0);
                            }
                        }
                    }
                    if (z == 0.0d)
                    {
                        j = 0;
                        for (i = jz - 1; i >= jk; i--)
                        {
                            j |= iq[i];
                        }
                        if (j == 0)
                        {
                            for (k = 1; iq[jk - k] == 0; k++)
                            {
                                ;
                            }
                            for (i = jz + 1; i <= jz + k; i++)
                            {
                                f[jx + i] = ((double)(ipio2[jv + i]));
                                for (j = 0, fw = 0.0d; j <= jx; j++)
                                {
                                    fw += x[j] * f[jx + i - j];
                                }
                                q[i] = fw;
                            }
                            jz += k;
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                if (z == 0.0d)
                {
                    jz -= 1;
                    q0 -= 24;
                    while (iq[jz] == 0)
                    {
                        jz--;
                        q0 -= 24;
                    }
                }
                else
                {
                    z = java.lang.Math.scalb(z, -q0);
                    if (z >= j2cs.fdlibm.FdLibm.TWO24)
                    {
                        fw = ((double)((java.lang.JC.d2i((j2cs.fdlibm.FdLibm.KernelRemPio2.twon24 * z)))));
                        iq[jz] = java.lang.JC.d2i((z - j2cs.fdlibm.FdLibm.TWO24 * fw));
                        jz += 1;
                        q0 += 24;
                        iq[jz] = java.lang.JC.d2i(fw);
                    }
                    else
                    {
                        iq[jz] = java.lang.JC.d2i(z);
                    }
                }
                fw = java.lang.Math.scalb(1.0d, q0);
                for (i = jz; i >= 0; i--)
                {
                    q[i] = fw * ((double)(iq[i]));
                    fw *= j2cs.fdlibm.FdLibm.KernelRemPio2.twon24;
                }
                for (i = jz; i >= 0; i--)
                {
                    for (fw = 0.0d, k = 0; k <= jp && k <= jz - i; k++)
                    {
                        fw += j2cs.fdlibm.FdLibm.KernelRemPio2.PIo2[k] * q[i + k];
                    }
                    fq[jz - i] = fw;
                }
                switch ((prec))
                {
                    case 0:
                    {
                        fw = 0.0d;
                        for (i = jz; i >= 0; i--)
                        {
                            fw += fq[i];
                        }
                        y[0] = (ih == 0 ? fw : -fw);
                        break;
                    }
                    case 1:
                    case 2:
                    {
                        fw = 0.0d;
                        for (i = jz; i >= 0; i--)
                        {
                            fw += fq[i];
                        }
                        y[0] = (ih == 0 ? fw : -fw);
                        fw = fq[0] - fw;
                        for (i = 1; i <= jz; i++)
                        {
                            fw += fq[i];
                        }
                        y[1] = (ih == 0 ? fw : -fw);
                        break;
                    }
                    case 3:
                    {
                        for (i = jz; i > 0; i--)
                        {
                            fw = fq[i - 1] + fq[i];
                            fq[i] += fq[i - 1] - fw;
                            fq[i - 1] = fw;
                        }
                        for (i = jz; i > 1; i--)
                        {
                            fw = fq[i - 1] + fq[i];
                            fq[i] += fq[i - 1] - fw;
                            fq[i - 1] = fw;
                        }
                        for (fw = 0.0d, i = jz; i >= 2; i--)
                        {
                            fw += fq[i];
                        }
                        if (ih == 0)
                        {
                            y[0] = fq[0];
                            y[1] = fq[1];
                            y[2] = fw;
                        }
                        else
                        {
                            y[0] = -fq[0];
                            y[1] = -fq[1];
                            y[2] = -fw;
                        }
                        break;
                    }
                }
                return n & 7;
            }
            static KernelRemPio2()
            {
                KernelRemPio2.init_jk = new int[] { 2, 3, 4, 6 };
                KernelRemPio2.PIo2 = new double[] { 1.570796251296997d, 7.549789415861596E-8d, 5.390302529957765E-15d, 3.282003415807913E-22d, 1.270655753080676E-29d, 1.2293330898111133E-36d, 2.7337005381646456E-44d, 2.1674168387780482E-51d };
            }
            public KernelRemPio2() : base()
            {
            }
        }
        // Java: 1061
        public class Asin : java.lang.Object
        {
            public const double pio2_hi = 1.5707963267948966d;
            public const double pio2_lo = 6.123233995736766E-17d;
            public const double pio4_hi = 0.7853981633974483d;
            public const double pS0 = 0.16666666666666666d;
            public const double pS1 = (-0.3255658186224009d);
            public const double pS2 = 0.20121253213486293d;
            public const double pS3 = (-0.04005553450067941d);
            public const double pS4 = 7.915349942898145E-4d;
            public const double pS5 = 3.479331075960212E-5d;
            public const double qS1 = (-2.403394911734414d);
            public const double qS2 = 2.0209457602335057d;
            public const double qS3 = (-0.6882839716054533d);
            public const double qS4 = 0.07703815055590194d;
            // Java: 1080
            public static double compute(double x)
            {
                double t = 0;
                double w = default;
                double p = default;
                double q = default;
                double c = default;
                double r = default;
                double s = default;
                int hx = default;
                int ix = default;
                hx = j2cs.fdlibm.FdLibm.__HI(x);
                ix = hx & j2cs.fdlibm.FdLibm.EXP_SIGNIF_BITS;
                if (ix >= 1072693248)
                {
                    if (((ix - 1072693248) | j2cs.fdlibm.FdLibm.__LO(x)) == 0)
                    {
                        return x * j2cs.fdlibm.FdLibm.Asin.pio2_hi + x * j2cs.fdlibm.FdLibm.Asin.pio2_lo;
                    }
                    return (x - x) / (x - x);
                }
                else if (ix < 1071644672)
                {
                    if (ix < 1044381696)
                    {
                        if (j2cs.fdlibm.FdLibm.HUGE + x > 1.0d)
                        {
                            return x;
                        }
                    }
                    else
                    {
                        t = x * x;
                    }
                    p = t * (j2cs.fdlibm.FdLibm.Asin.pS0 + t * (j2cs.fdlibm.FdLibm.Asin.pS1 + t * (j2cs.fdlibm.FdLibm.Asin.pS2 + t * (j2cs.fdlibm.FdLibm.Asin.pS3 + t * (j2cs.fdlibm.FdLibm.Asin.pS4 + t * j2cs.fdlibm.FdLibm.Asin.pS5)))));
                    q = 1.0d + t * (j2cs.fdlibm.FdLibm.Asin.qS1 + t * (j2cs.fdlibm.FdLibm.Asin.qS2 + t * (j2cs.fdlibm.FdLibm.Asin.qS3 + t * j2cs.fdlibm.FdLibm.Asin.qS4)));
                    w = p / q;
                    return x + x * w;
                }
                w = 1.0d - java.lang.Math.abs(x);
                t = w * 0.5d;
                p = t * (j2cs.fdlibm.FdLibm.Asin.pS0 + t * (j2cs.fdlibm.FdLibm.Asin.pS1 + t * (j2cs.fdlibm.FdLibm.Asin.pS2 + t * (j2cs.fdlibm.FdLibm.Asin.pS3 + t * (j2cs.fdlibm.FdLibm.Asin.pS4 + t * j2cs.fdlibm.FdLibm.Asin.pS5)))));
                q = 1.0d + t * (j2cs.fdlibm.FdLibm.Asin.qS1 + t * (j2cs.fdlibm.FdLibm.Asin.qS2 + t * (j2cs.fdlibm.FdLibm.Asin.qS3 + t * j2cs.fdlibm.FdLibm.Asin.qS4)));
                s = java.lang.Math.sqrt(t);
                if (ix >= 1072640819)
                {
                    w = p / q;
                    t = j2cs.fdlibm.FdLibm.Asin.pio2_hi - (2.0d * (s + s * w) - j2cs.fdlibm.FdLibm.Asin.pio2_lo);
                }
                else
                {
                    w = s;
                    w = j2cs.fdlibm.FdLibm.__LO(w, 0);
                    c = (t - w * w) / (s + w);
                    r = p / q;
                    p = 2.0d * s * r - (j2cs.fdlibm.FdLibm.Asin.pio2_lo - 2.0d * c);
                    q = j2cs.fdlibm.FdLibm.Asin.pio4_hi - 2.0d * w;
                    t = j2cs.fdlibm.FdLibm.Asin.pio4_hi - (p - q);
                }
                return (hx > 0 ? t : -t);
            }
            public Asin() : base()
            {
                throw new System.NotSupportedException();
            }
        }
        // Java: 1149
        public class Acos : java.lang.Object
        {
            public const double pio2_hi = 1.5707963267948966d;
            public const double pio2_lo = 6.123233995736766E-17d;
            public const double pS0 = 0.16666666666666666d;
            public const double pS1 = (-0.3255658186224009d);
            public const double pS2 = 0.20121253213486293d;
            public const double pS3 = (-0.04005553450067941d);
            public const double pS4 = 7.915349942898145E-4d;
            public const double pS5 = 3.479331075960212E-5d;
            public const double qS1 = (-2.403394911734414d);
            public const double qS2 = 2.0209457602335057d;
            public const double qS3 = (-0.6882839716054533d);
            public const double qS4 = 0.07703815055590194d;
            // Java: 1166
            public static double compute(double x)
            {
                double z = default;
                double p = default;
                double q = default;
                double r = default;
                double w = default;
                double s = default;
                double c = default;
                double df = default;
                int hx = default;
                int ix = default;
                hx = j2cs.fdlibm.FdLibm.__HI(x);
                ix = hx & j2cs.fdlibm.FdLibm.EXP_SIGNIF_BITS;
                if (ix >= 1072693248)
                {
                    if (((ix - 1072693248) | j2cs.fdlibm.FdLibm.__LO(x)) == 0)
                    {
                        if (hx > 0)
                        {
                            return 0.0d;
                        }
                        else
                        {
                            return java.lang.Math.PI + 2.0d * j2cs.fdlibm.FdLibm.Acos.pio2_lo;
                        }
                    }
                    return (x - x) / (x - x);
                }
                if (ix < 1071644672)
                {
                    if (ix <= 1012924416)
                    {
                        return j2cs.fdlibm.FdLibm.Acos.pio2_hi + j2cs.fdlibm.FdLibm.Acos.pio2_lo;
                    }
                    z = x * x;
                    p = z * (j2cs.fdlibm.FdLibm.Acos.pS0 + z * (j2cs.fdlibm.FdLibm.Acos.pS1 + z * (j2cs.fdlibm.FdLibm.Acos.pS2 + z * (j2cs.fdlibm.FdLibm.Acos.pS3 + z * (j2cs.fdlibm.FdLibm.Acos.pS4 + z * j2cs.fdlibm.FdLibm.Acos.pS5)))));
                    q = 1.0d + z * (j2cs.fdlibm.FdLibm.Acos.qS1 + z * (j2cs.fdlibm.FdLibm.Acos.qS2 + z * (j2cs.fdlibm.FdLibm.Acos.qS3 + z * j2cs.fdlibm.FdLibm.Acos.qS4)));
                    r = p / q;
                    return j2cs.fdlibm.FdLibm.Acos.pio2_hi - (x - (j2cs.fdlibm.FdLibm.Acos.pio2_lo - x * r));
                }
                else if (hx < 0)
                {
                    z = (1.0d + x) * 0.5d;
                    p = z * (j2cs.fdlibm.FdLibm.Acos.pS0 + z * (j2cs.fdlibm.FdLibm.Acos.pS1 + z * (j2cs.fdlibm.FdLibm.Acos.pS2 + z * (j2cs.fdlibm.FdLibm.Acos.pS3 + z * (j2cs.fdlibm.FdLibm.Acos.pS4 + z * j2cs.fdlibm.FdLibm.Acos.pS5)))));
                    q = 1.0d + z * (j2cs.fdlibm.FdLibm.Acos.qS1 + z * (j2cs.fdlibm.FdLibm.Acos.qS2 + z * (j2cs.fdlibm.FdLibm.Acos.qS3 + z * j2cs.fdlibm.FdLibm.Acos.qS4)));
                    s = java.lang.Math.sqrt(z);
                    r = p / q;
                    w = r * s - j2cs.fdlibm.FdLibm.Acos.pio2_lo;
                    return java.lang.Math.PI - 2.0d * (s + w);
                }
                else
                {
                    z = (1.0d - x) * 0.5d;
                    s = java.lang.Math.sqrt(z);
                    df = s;
                    df = j2cs.fdlibm.FdLibm.__LO(df, 0);
                    c = (z - df * df) / (s + df);
                    p = z * (j2cs.fdlibm.FdLibm.Acos.pS0 + z * (j2cs.fdlibm.FdLibm.Acos.pS1 + z * (j2cs.fdlibm.FdLibm.Acos.pS2 + z * (j2cs.fdlibm.FdLibm.Acos.pS3 + z * (j2cs.fdlibm.FdLibm.Acos.pS4 + z * j2cs.fdlibm.FdLibm.Acos.pS5)))));
                    q = 1.0d + z * (j2cs.fdlibm.FdLibm.Acos.qS1 + z * (j2cs.fdlibm.FdLibm.Acos.qS2 + z * (j2cs.fdlibm.FdLibm.Acos.qS3 + z * j2cs.fdlibm.FdLibm.Acos.qS4)));
                    r = p / q;
                    w = r * s + c;
                    return 2.0d * (df + w);
                }
            }
            public Acos() : base()
            {
                throw new System.NotSupportedException();
            }
        }
        // Java: 1232
        public class Atan : java.lang.Object
        {
            public static double[] atanhi;
            public static double[] atanlo;
            public static double[] aT;
            // Java: 1263
            public static double compute(double x)
            {
                double w = default;
                double s1 = default;
                double s2 = default;
                double z = default;
                int ix = default;
                int hx = default;
                int id = default;
                hx = j2cs.fdlibm.FdLibm.__HI(x);
                ix = hx & j2cs.fdlibm.FdLibm.EXP_SIGNIF_BITS;
                if (ix >= 1141899264)
                {
                    if (ix > j2cs.fdlibm.FdLibm.EXP_BITS || (ix == j2cs.fdlibm.FdLibm.EXP_BITS && (j2cs.fdlibm.FdLibm.__LO(x) != 0)))
                    {
                        return x + x;
                    }
                    if (hx > 0)
                    {
                        return j2cs.fdlibm.FdLibm.Atan.atanhi[3] + j2cs.fdlibm.FdLibm.Atan.atanlo[3];
                    }
                    else
                    {
                        return -j2cs.fdlibm.FdLibm.Atan.atanhi[3] - j2cs.fdlibm.FdLibm.Atan.atanlo[3];
                    }
                }
                if (ix < 1071382528)
                {
                    if (ix < 1042284544)
                    {
                        if (j2cs.fdlibm.FdLibm.HUGE + x > 1.0d)
                        {
                            return x;
                        }
                    }
                    id = (-1);
                }
                else
                {
                    x = java.lang.Math.abs(x);
                    if (ix < 1072889856)
                    {
                        if (ix < 1072037888)
                        {
                            id = 0;
                            x = (2.0d * x - 1.0d) / (2.0d + x);
                        }
                        else
                        {
                            id = 1;
                            x = (x - 1.0d) / (x + 1.0d);
                        }
                    }
                    else
                    {
                        if (ix < 1073971200)
                        {
                            id = 2;
                            x = (x - 1.5d) / (1.0d + 1.5d * x);
                        }
                        else
                        {
                            id = 3;
                            x = -1.0d / x;
                        }
                    }
                }
                z = x * x;
                w = z * z;
                s1 = z * (j2cs.fdlibm.FdLibm.Atan.aT[0] + w * (j2cs.fdlibm.FdLibm.Atan.aT[2] + w * (j2cs.fdlibm.FdLibm.Atan.aT[4] + w * (j2cs.fdlibm.FdLibm.Atan.aT[6] + w * (j2cs.fdlibm.FdLibm.Atan.aT[8] + w * j2cs.fdlibm.FdLibm.Atan.aT[10])))));
                s2 = w * (j2cs.fdlibm.FdLibm.Atan.aT[1] + w * (j2cs.fdlibm.FdLibm.Atan.aT[3] + w * (j2cs.fdlibm.FdLibm.Atan.aT[5] + w * (j2cs.fdlibm.FdLibm.Atan.aT[7] + w * j2cs.fdlibm.FdLibm.Atan.aT[9]))));
                if (id < 0)
                {
                    return x - x * (s1 + s2);
                }
                else
                {
                    z = j2cs.fdlibm.FdLibm.Atan.atanhi[id] - ((x * (s1 + s2) - j2cs.fdlibm.FdLibm.Atan.atanlo[id]) - x);
                    return (hx < 0 ? -z : z);
                }
            }
            static Atan()
            {
                Atan.atanhi = new double[] { 0.4636476090008061d, 0.7853981633974483d, 0.982793723247329d, 1.5707963267948966d };
                Atan.atanlo = new double[] { 2.2698777452961687E-17d, 3.061616997868383E-17d, 1.3903311031230998E-17d, 6.123233995736766E-17d };
                Atan.aT = new double[] { 0.3333333333333293d, -0.19999999999876483d, 0.14285714272503466d, -0.11111110405462356d, 0.09090887133436507d, -0.0769187620504483d, 0.06661073137387531d, -0.058335701337905735d, 0.049768779946159324d, -0.036531572744216916d, 0.016285820115365782d };
            }
            public Atan() : base()
            {
                throw new System.NotSupportedException();
            }
        }
        // Java: 1350
        public class Atan2 : java.lang.Object
        {
            public const double tiny = 1.0E-300d;
            public const double pi_o_4 = 0.7853981633974483d;
            public const double pi_o_2 = 1.5707963267948966d;
            public const double pi_lo = 1.2246467991473532E-16d;
            // Java: 1359
            public static double compute(double y, double x)
            {
                double z = default;
                int k = default;
                int m = default;
                int hx = default;
                int hy = default;
                int ix = default;
                int iy = default;
                int lx = default;
                int ly = default;
                hx = j2cs.fdlibm.FdLibm.__HI(x);
                ix = hx & j2cs.fdlibm.FdLibm.EXP_SIGNIF_BITS;
                lx = j2cs.fdlibm.FdLibm.__LO(x);
                hy = j2cs.fdlibm.FdLibm.__HI(y);
                iy = hy & j2cs.fdlibm.FdLibm.EXP_SIGNIF_BITS;
                ly = j2cs.fdlibm.FdLibm.__LO(y);
                if (java.lang.Double.isNaN(x) || java.lang.Double.isNaN(y))
                {
                    return x + y;
                }
                if (((hx - 1072693248) | lx) == 0)
                {
                    return java.lang.Math.atan(y);
                }
                m = ((hy >> 31) & 1) | ((hx >> 30) & 2);
                if ((iy | ly) == 0)
                {
                    switch ((m))
                    {
                        case 0:
                        case 1:
                        {
                            return y;
                        }
                        case 2:
                        {
                            return java.lang.Math.PI + j2cs.fdlibm.FdLibm.Atan2.tiny;
                        }
                        case 3:
                        {
                            return -java.lang.Math.PI - j2cs.fdlibm.FdLibm.Atan2.tiny;
                        }
                    }
                }
                if ((ix | lx) == 0)
                {
                    return (hy < 0 ? -j2cs.fdlibm.FdLibm.Atan2.pi_o_2 - j2cs.fdlibm.FdLibm.Atan2.tiny : j2cs.fdlibm.FdLibm.Atan2.pi_o_2 + j2cs.fdlibm.FdLibm.Atan2.tiny);
                }
                if (ix == j2cs.fdlibm.FdLibm.EXP_BITS)
                {
                    if (iy == j2cs.fdlibm.FdLibm.EXP_BITS)
                    {
                        switch ((m))
                        {
                            case 0:
                            {
                                return j2cs.fdlibm.FdLibm.Atan2.pi_o_4 + j2cs.fdlibm.FdLibm.Atan2.tiny;
                            }
                            case 1:
                            {
                                return -j2cs.fdlibm.FdLibm.Atan2.pi_o_4 - j2cs.fdlibm.FdLibm.Atan2.tiny;
                            }
                            case 2:
                            {
                                return 3.0d * j2cs.fdlibm.FdLibm.Atan2.pi_o_4 + j2cs.fdlibm.FdLibm.Atan2.tiny;
                            }
                            case 3:
                            {
                                return -3.0d * j2cs.fdlibm.FdLibm.Atan2.pi_o_4 - j2cs.fdlibm.FdLibm.Atan2.tiny;
                            }
                        }
                    }
                    else
                    {
                        switch ((m))
                        {
                            case 0:
                            {
                                return 0.0d;
                            }
                            case 1:
                            {
                                return -0.0d;
                            }
                            case 2:
                            {
                                return java.lang.Math.PI + j2cs.fdlibm.FdLibm.Atan2.tiny;
                            }
                            case 3:
                            {
                                return -java.lang.Math.PI - j2cs.fdlibm.FdLibm.Atan2.tiny;
                            }
                        }
                    }
                }
                if (iy == j2cs.fdlibm.FdLibm.EXP_BITS)
                {
                    return (hy < 0 ? -j2cs.fdlibm.FdLibm.Atan2.pi_o_2 - j2cs.fdlibm.FdLibm.Atan2.tiny : j2cs.fdlibm.FdLibm.Atan2.pi_o_2 + j2cs.fdlibm.FdLibm.Atan2.tiny);
                }
                k = (iy - ix) >> 20;
                if (k > 60)
                {
                    z = j2cs.fdlibm.FdLibm.Atan2.pi_o_2 + 0.5d * j2cs.fdlibm.FdLibm.Atan2.pi_lo;
                }
                else if (hx < 0 && k < (-60))
                {
                    z = 0.0d;
                }
                else
                {
                    z = java.lang.Math.atan(java.lang.Math.abs(y / x));
                }
                switch ((m))
                {
                    case 0:
                    {
                        return z;
                    }
                    case 1:
                    {
                        return -z;
                    }
                    case 2:
                    {
                        return java.lang.Math.PI - (z - j2cs.fdlibm.FdLibm.Atan2.pi_lo);
                    }
                    default:
                    {
                        return (z - j2cs.fdlibm.FdLibm.Atan2.pi_lo) - java.lang.Math.PI;
                    }
                }
            }
            public Atan2() : base()
            {
                throw new System.NotSupportedException();
            }
        }
        // Java: 1500
        public class Sqrt : java.lang.Object
        {
            public const double tiny = 1.0E-300d;
            // Java: 1505
            public static double compute(double x)
            {
                double z = 0.0d;
                int sign = j2cs.fdlibm.FdLibm.SIGN_BIT;
                int r = default;
                int t1 = default;
                int s1 = default;
                int ix1 = default;
                int q1 = default;
                int ix0 = default;
                int s0 = default;
                int q = default;
                int m = default;
                int t = default;
                int i = default;
                ix0 = j2cs.fdlibm.FdLibm.__HI(x);
                ix1 = j2cs.fdlibm.FdLibm.__LO(x);
                if ((ix0 & j2cs.fdlibm.FdLibm.EXP_BITS) == j2cs.fdlibm.FdLibm.EXP_BITS)
                {
                    return x * x + x;
                }
                if (ix0 <= 0)
                {
                    if (((ix0 & (~sign)) | ix1) == 0)
                    {
                        return x;
                    }
                    else if (ix0 < 0)
                    {
                        return (x - x) / (x - x);
                    }
                }
                m = (ix0 >> 20);
                if (m == 0)
                {
                    while (ix0 == 0)
                    {
                        m -= 21;
                        ix0 |= (java.lang.JC.ushr(ix1, 11));
                        ix1 <<= 21;
                    }
                    for (i = 0; (ix0 & 1048576) == 0; i++)
                    {
                        ix0 <<= 1;
                    }
                    m -= i - 1;
                    ix0 |= (java.lang.JC.ushr(ix1, (32 - i)));
                    ix1 <<= i;
                }
                m -= 1023;
                ix0 = (ix0 & 1048575) | 1048576;
                if ((m & 1) != 0)
                {
                    ix0 += ix0 + (java.lang.JC.ushr((ix1 & sign), 31));
                    ix1 += ix1;
                }
                m >>= 1;
                ix0 += ix0 + (java.lang.JC.ushr((ix1 & sign), 31));
                ix1 += ix1;
                q = q1 = s0 = s1 = 0;
                r = 2097152;
                while (r != 0)
                {
                    t = s0 + r;
                    if (t <= ix0)
                    {
                        s0 = t + r;
                        ix0 -= t;
                        q += r;
                    }
                    ix0 += ix0 + (java.lang.JC.ushr((ix1 & sign), 31));
                    ix1 += ix1;
                    r = java.lang.JC.ushr(r, 1);
                }
                r = sign;
                while (r != 0)
                {
                    t1 = s1 + r;
                    t = s0;
                    if ((t < ix0) || ((t == ix0) && (java.lang.Integer.compareUnsigned(t1, ix1) <= 0)))
                    {
                        s1 = t1 + r;
                        if (((t1 & sign) == sign) && (s1 & sign) == 0)
                        {
                            s0 += 1;
                        }
                        ix0 -= t;
                        if (java.lang.Integer.compareUnsigned(ix1, t1) < 0)
                        {
                            ix0 -= 1;
                        }
                        ix1 -= t1;
                        q1 += r;
                    }
                    ix0 += ix0 + (java.lang.JC.ushr((ix1 & sign), 31));
                    ix1 += ix1;
                    r = java.lang.JC.ushr(r, 1);
                }
                if ((ix0 | ix1) != 0)
                {
                    z = 1.0d - j2cs.fdlibm.FdLibm.Sqrt.tiny;
                    if (z >= 1.0d)
                    {
                        z = 1.0d + j2cs.fdlibm.FdLibm.Sqrt.tiny;
                        if (q1 == (-1))
                        {
                            q1 = 0;
                            q += 1;
                        }
                        else if (z > 1.0d)
                        {
                            if (q1 == (-2))
                            {
                                q += 1;
                            }
                            q1 += 2;
                        }
                        else
                        {
                            q1 += (q1 & 1);
                        }
                    }
                }
                ix0 = (q >> 1) + 1071644672;
                ix1 = java.lang.JC.ushr(q1, 1);
                if ((q & 1) == 1)
                {
                    ix1 |= sign;
                }
                ix0 += (m << 20);
                return j2cs.fdlibm.FdLibm.__HI_LO(ix0, ix1);
            }
            public Sqrt() : base()
            {
                throw new System.NotSupportedException();
            }
        }
        // Java: 1878
        public class Cbrt : java.lang.Object
        {
            public const int B1 = 715094163;
            public const int B2 = 696219795;
            public const double C = 0.5428571428571428d;
            public const double D = (-0.7053061224489796d);
            public const double E = 1.4142857142857144d;
            public const double F = 1.6071428571428572d;
            public const double G = 0.35714285714285715d;
            // Java: 1893
            public static double compute(double x)
            {
                double t = 0.0d;
                double sign = default;
                if (x == 0.0d || !java.lang.Double.isFinite(x))
                {
                    return x;
                }
                sign = (x < 0.0d ? -1.0d : 1.0d);
                x = java.lang.Math.abs(x);
                if (x < 2.2250738585072014E-308d)
                {
                    t = 1.8014398509481984E16d;
                    t *= x;
                    t = j2cs.fdlibm.FdLibm.__HI(t, j2cs.fdlibm.FdLibm.__HI(t) / 3 + j2cs.fdlibm.FdLibm.Cbrt.B2);
                }
                else
                {
                    int hx = j2cs.fdlibm.FdLibm.__HI(x);
                    t = j2cs.fdlibm.FdLibm.__HI(t, hx / 3 + j2cs.fdlibm.FdLibm.Cbrt.B1);
                }
                double r = default;
                double s = default;
                double w = default;
                r = t * t / x;
                s = j2cs.fdlibm.FdLibm.Cbrt.C + r * t;
                t *= j2cs.fdlibm.FdLibm.Cbrt.G + j2cs.fdlibm.FdLibm.Cbrt.F / (s + j2cs.fdlibm.FdLibm.Cbrt.E + j2cs.fdlibm.FdLibm.Cbrt.D / s);
                t = j2cs.fdlibm.FdLibm.__LO(t, 0);
                t = j2cs.fdlibm.FdLibm.__HI(t, j2cs.fdlibm.FdLibm.__HI(t) + 1);
                s = t * t;
                r = x / s;
                w = t + t;
                r = (r - t) / (w + r);
                t = t + t * r;
                return sign * t;
            }
            public Cbrt() : base()
            {
                throw new System.NotSupportedException();
            }
        }
        // Java: 1968
        public class Hypot : java.lang.Object
        {
            public const double TWO_MINUS_600 = 2.409919865102884E-181d;
            public const double TWO_PLUS_600 = 4.149515568880993E180d;
            // Java: 1976
            public static double compute(double x, double y)
            {
                double a = java.lang.Math.abs(x);
                double b = java.lang.Math.abs(y);
                if (!java.lang.Double.isFinite(a) || !java.lang.Double.isFinite(b))
                {
                    if (a == j2cs.fdlibm.FdLibm.INFINITY || b == j2cs.fdlibm.FdLibm.INFINITY)
                    {
                        return j2cs.fdlibm.FdLibm.INFINITY;
                    }
                    else
                    {
                        return a + b;
                    }
                }
                if (b > a)
                {
                    double tmp = a;
                    a = b;
                    b = tmp;
                }
                int ha = j2cs.fdlibm.FdLibm.__HI(a);
                int hb = j2cs.fdlibm.FdLibm.__HI(b);
                if ((ha - hb) > 62914560)
                {
                    return a + b;
                }
                int k = 0;
                if (a > 3.2733937296446915E150d)
                {
                    ha -= 629145600;
                    hb -= 629145600;
                    a = a * j2cs.fdlibm.FdLibm.Hypot.TWO_MINUS_600;
                    b = b * j2cs.fdlibm.FdLibm.Hypot.TWO_MINUS_600;
                    k += 600;
                }
                double t1 = default;
                double t2 = default;
                if (b < 3.054936363499605E-151d)
                {
                    if (b < java.lang.Double.MIN_NORMAL)
                    {
                        if (b == 0.0d)
                        {
                            return a;
                        }
                        t1 = 4.49423283715579E307d;
                        b *= t1;
                        a *= t1;
                        k -= 1022;
                    }
                    else
                    {
                        ha += 629145600;
                        hb += 629145600;
                        a = a * j2cs.fdlibm.FdLibm.Hypot.TWO_PLUS_600;
                        b = b * j2cs.fdlibm.FdLibm.Hypot.TWO_PLUS_600;
                        k -= 600;
                    }
                }
                double w = a - b;
                if (w > b)
                {
                    t1 = 0;
                    t1 = j2cs.fdlibm.FdLibm.__HI(t1, ha);
                    t2 = a - t1;
                    w = java.lang.Math.sqrt(t1 * t1 - (b * (-b) - t2 * (a + t1)));
                }
                else
                {
                    double y1 = default;
                    double y2 = default;
                    a = a + a;
                    y1 = 0;
                    y1 = j2cs.fdlibm.FdLibm.__HI(y1, hb);
                    y2 = b - y1;
                    t1 = 0;
                    t1 = j2cs.fdlibm.FdLibm.__HI(t1, ha + 1048576);
                    t2 = a - t1;
                    w = java.lang.Math.sqrt(t1 * y1 - (w * (-w) - (t1 * y2 + t2 * b)));
                }
                if (k != 0)
                {
                    return java.lang.Double.longBitsToDouble(((((long)(k)) + 1023L) << 52) & 9218868437227405312L) * w;
                }
                else
                {
                    return w;
                }
            }
            public Hypot() : base()
            {
                throw new System.NotSupportedException();
            }
        }
        // Java: 2111
        public class Pow : java.lang.Object
        {
            // Java: 2116
            // hoisted out of compute(): per-call array allocations made the generator crawl under Mono's GC (Unity)
            static readonly double[] s_BP = { 1.0d, 1.5d }, s_DPH = { 0.0d, 0.5849624872207642d }, s_DPL = { 0.0d, 1.350039202129749E-8d };
            public static double compute(double x, double y)
            {
                double z = default;
                double r = default;
                double s = default;
                double t = default;
                double u = default;
                double v = default;
                double w = default;
                int i = default;
                int j = default;
                int k = default;
                int n = default;
                if (y == 0.0d)
                {
                    return 1.0d;
                }
                if (java.lang.Double.isNaN(x) || java.lang.Double.isNaN(y))
                {
                    return x + y;
                }
                double y_abs = java.lang.Math.abs(y);
                double x_abs = java.lang.Math.abs(x);
                if (y == 2.0d)
                {
                    return x * x;
                }
                else if (y == 0.5d)
                {
                    if (x >= -java.lang.Double.MAX_VALUE)
                    {
                        return java.lang.Math.sqrt(x + 0.0d);
                    }
                }
                else if (y_abs == 1.0d)
                {
                    return (y == 1.0d ? x : 1.0d / x);
                }
                else if (y_abs == j2cs.fdlibm.FdLibm.INFINITY)
                {
                    if (x_abs == 1.0d)
                    {
                        return y - y;
                    }
                    else if (x_abs > 1.0d)
                    {
                        return (y >= 0 ? y : 0.0d);
                    }
                    else
                    {
                        return (y < 0 ? -y : 0.0d);
                    }
                }
                int hx = j2cs.fdlibm.FdLibm.__HI(x);
                int ix = hx & j2cs.fdlibm.FdLibm.EXP_SIGNIF_BITS;
                int y_is_int = 0;
                if (hx < 0)
                {
                    if (y_abs >= 9.007199254740992E15d)
                    {
                        y_is_int = 2;
                    }
                    else if (y_abs >= 1.0d)
                    {
                        long y_abs_as_long = java.lang.JC.d2l(y_abs);
                        if ((((double)(y_abs_as_long))) == y_abs)
                        {
                            y_is_int = 2 - unchecked((int)((y_abs_as_long & 1L)));
                        }
                    }
                }
                if (x_abs == 0.0d || x_abs == j2cs.fdlibm.FdLibm.INFINITY || x_abs == 1.0d)
                {
                    z = x_abs;
                    if (y < 0.0d)
                    {
                        z = 1.0d / z;
                    }
                    if (hx < 0)
                    {
                        if (((ix - 1072693248) | y_is_int) == 0)
                        {
                            z = (z - z) / (z - z);
                        }
                        else if (y_is_int == 1)
                        {
                            z = -1.0d * z;
                        }
                    }
                    return z;
                }
                n = (hx >> 31) + 1;
                if ((n | y_is_int) == 0)
                {
                    return (x - x) / (x - x);
                }
                s = 1.0d;
                if ((n | (y_is_int - 1)) == 0)
                {
                    s = -1.0d;
                }
                double p_h = default;
                double p_l = default;
                double t1 = default;
                double t2 = default;
                if (y_abs > 2.1474856959999995E9d)
                {
                    double INV_LN2 = 1.4426950408889634d;
                    double INV_LN2_H = 1.4426950216293335d;
                    double INV_LN2_L = 1.9259629911266175E-8d;
                    if (x_abs < 0.9999995231628418d)
                    {
                        return (y < 0.0d ? s * j2cs.fdlibm.FdLibm.INFINITY : s * 0.0d);
                    }
                    if (x_abs > 1.0000009536743162d)
                    {
                        return (y > 0.0d ? s * j2cs.fdlibm.FdLibm.INFINITY : s * 0.0d);
                    }
                    t = x_abs - 1.0d;
                    w = (t * t) * (0.5d - t * (0.3333333333333333d - t * 0.25d));
                    u = INV_LN2_H * t;
                    v = t * INV_LN2_L - w * INV_LN2;
                    t1 = u + v;
                    t1 = j2cs.fdlibm.FdLibm.__LO(t1, 0);
                    t2 = v - (t1 - u);
                }
                else
                {
                    double CP = 0.9617966939259756d;
                    double CP_H = 0.9617967009544373d;
                    double CP_L = -7.028461650952758E-9d;
                    double z_h = default;
                    double z_l = default;
                    double ss = default;
                    double s2 = default;
                    double s_h = default;
                    double s_l = default;
                    double t_h = default;
                    double t_l = default;
                    n = 0;
                    if (ix < 1048576)
                    {
                        x_abs *= 9.007199254740992E15d;
                        n -= 53;
                        ix = j2cs.fdlibm.FdLibm.__HI(x_abs);
                    }
                    n += ((ix) >> 20) - 1023;
                    j = ix & 1048575;
                    ix = j | 1072693248;
                    if (j <= 235662)
                    {
                        k = 0;
                    }
                    else if (j < 767610)
                    {
                        k = 1;
                    }
                    else
                    {
                        k = 0;
                        n += 1;
                        ix -= 1048576;
                    }
                    x_abs = j2cs.fdlibm.FdLibm.__HI(x_abs, ix);
                    double[] BP = s_BP;
                    double[] DP_H = s_DPH;
                    double[] DP_L = s_DPL;
                    double L1 = 0.5999999999999946d;
                    double L2 = 0.4285714285785502d;
                    double L3 = 0.33333332981837743d;
                    double L4 = 0.272728123808534d;
                    double L5 = 0.23066074577556175d;
                    double L6 = 0.20697501780033842d;
                    u = x_abs - BP[k];
                    v = 1.0d / (x_abs + BP[k]);
                    ss = u * v;
                    s_h = ss;
                    s_h = j2cs.fdlibm.FdLibm.__LO(s_h, 0);
                    t_h = 0.0d;
                    t_h = j2cs.fdlibm.FdLibm.__HI(t_h, ((ix >> 1) | 536870912) + 524288 + (k << 18));
                    t_l = x_abs - (t_h - BP[k]);
                    s_l = v * ((u - s_h * t_h) - s_h * t_l);
                    s2 = ss * ss;
                    r = s2 * s2 * (L1 + s2 * (L2 + s2 * (L3 + s2 * (L4 + s2 * (L5 + s2 * L6)))));
                    r += s_l * (s_h + ss);
                    s2 = s_h * s_h;
                    t_h = 3.0d + s2 + r;
                    t_h = j2cs.fdlibm.FdLibm.__LO(t_h, 0);
                    t_l = r - ((t_h - 3.0d) - s2);
                    u = s_h * t_h;
                    v = s_l * t_h + t_l * ss;
                    p_h = u + v;
                    p_h = j2cs.fdlibm.FdLibm.__LO(p_h, 0);
                    p_l = v - (p_h - u);
                    z_h = CP_H * p_h;
                    z_l = CP_L * p_h + p_l * CP + DP_L[k];
                    t = ((double)(n));
                    t1 = (((z_h + z_l) + DP_H[k]) + t);
                    t1 = j2cs.fdlibm.FdLibm.__LO(t1, 0);
                    t2 = z_l - (((t1 - t) - DP_H[k]) - z_h);
                }
                double y1 = y;
                y1 = j2cs.fdlibm.FdLibm.__LO(y1, 0);
                p_l = (y - y1) * t1 + y * t2;
                p_h = y1 * t1;
                z = p_l + p_h;
                j = j2cs.fdlibm.FdLibm.__HI(z);
                i = j2cs.fdlibm.FdLibm.__LO(z);
                if (j >= 1083179008)
                {
                    if (((j - 1083179008) | i) != 0)
                    {
                        return s * j2cs.fdlibm.FdLibm.INFINITY;
                    }
                    else
                    {
                        double OVT = 8.008566259537294E-17d;
                        if (p_l + OVT > z - p_h)
                        {
                            return s * j2cs.fdlibm.FdLibm.INFINITY;
                        }
                    }
                }
                else if ((j & j2cs.fdlibm.FdLibm.EXP_SIGNIF_BITS) >= 1083231232)
                {
                    if (((j - (-1064252416)) | i) != 0)
                    {
                        return s * 0.0d;
                    }
                    else
                    {
                        if (p_l <= z - p_h)
                        {
                            return s * 0.0d;
                        }
                    }
                }
                double P1 = 0.16666666666666602d;
                double P2 = -0.0027777777777015593d;
                double P3 = 6.613756321437934E-5d;
                double P4 = -1.6533902205465252E-6d;
                double P5 = 4.1381367970572385E-8d;
                double LG2 = 0.6931471805599453d;
                double LG2_H = 0.6931471824645996d;
                double LG2_L = -1.904654299957768E-9d;
                i = j & j2cs.fdlibm.FdLibm.EXP_SIGNIF_BITS;
                k = (i >> 20) - 1023;
                n = 0;
                if (i > 1071644672)
                {
                    n = j + (1048576 >> (k + 1));
                    k = ((n & j2cs.fdlibm.FdLibm.EXP_SIGNIF_BITS) >> 20) - 1023;
                    t = 0.0d;
                    t = j2cs.fdlibm.FdLibm.__HI(t, (n & ~(1048575 >> k)));
                    n = ((n & 1048575) | 1048576) >> (20 - k);
                    if (j < 0)
                    {
                        n = -n;
                    }
                    p_h -= t;
                }
                t = p_l + p_h;
                t = j2cs.fdlibm.FdLibm.__LO(t, 0);
                u = t * LG2_H;
                v = (p_l - (t - p_h)) * LG2 + t * LG2_L;
                z = u + v;
                w = v - (z - u);
                t = z * z;
                t1 = z - t * (P1 + t * (P2 + t * (P3 + t * (P4 + t * P5))));
                r = (z * t1) / (t1 - 2.0d) - (w + z * w);
                z = 1.0d - (r - z);
                j = j2cs.fdlibm.FdLibm.__HI(z);
                j += (n << 20);
                if ((j >> 20) <= 0)
                {
                    z = java.lang.Math.scalb(z, n);
                }
                else
                {
                    int z_hi = j2cs.fdlibm.FdLibm.__HI(z);
                    z_hi += (n << 20);
                    z = j2cs.fdlibm.FdLibm.__HI(z, z_hi);
                }
                return s * z;
            }
            public Pow() : base()
            {
                throw new System.NotSupportedException();
            }
        }
        // Java: 2430
        public class Exp : java.lang.Object
        {
            public static double[] half;
            public const double huge = 1.0E300d;
            public const double twom1000 = 9.332636185032189E-302d;
            public const double o_threshold = 709.782712893384d;
            public const double u_threshold = (-745.1332191019411d);
            public static double[] ln2HI;
            public static double[] ln2LO;
            public const double invln2 = 1.4426950408889634d;
            public const double P1 = 0.16666666666666602d;
            public const double P2 = (-0.0027777777777015593d);
            public const double P3 = 6.613756321437934E-5d;
            public const double P4 = (-1.6533902205465252E-6d);
            public const double P5 = 4.1381367970572385E-8d;
            // Java: 2450
            public static double compute(double x)
            {
                double y = default;
                double hi = 0.0d;
                double lo = 0.0d;
                double c = default;
                double t = default;
                int k = 0;
                int xsb = default;
                int hx = default;
                hx = j2cs.fdlibm.FdLibm.__HI(x);
                xsb = (hx >> 31) & 1;
                hx &= j2cs.fdlibm.FdLibm.EXP_SIGNIF_BITS;
                if (hx >= 1082535490)
                {
                    if (hx >= 2146435072)
                    {
                        if (((hx & 1048575) | j2cs.fdlibm.FdLibm.__LO(x)) != 0)
                        {
                            return x + x;
                        }
                        else
                        {
                            return (xsb == 0 ? x : 0.0d);
                        }
                    }
                    if (x > j2cs.fdlibm.FdLibm.Exp.o_threshold)
                    {
                        return j2cs.fdlibm.FdLibm.Exp.huge * j2cs.fdlibm.FdLibm.Exp.huge;
                    }
                    if (x < j2cs.fdlibm.FdLibm.Exp.u_threshold)
                    {
                        return j2cs.fdlibm.FdLibm.Exp.twom1000 * j2cs.fdlibm.FdLibm.Exp.twom1000;
                    }
                }
                if (hx > 1071001154)
                {
                    if (hx < 1072734898)
                    {
                        hi = x - j2cs.fdlibm.FdLibm.Exp.ln2HI[xsb];
                        lo = j2cs.fdlibm.FdLibm.Exp.ln2LO[xsb];
                        k = 1 - xsb - xsb;
                    }
                    else
                    {
                        k = java.lang.JC.d2i((j2cs.fdlibm.FdLibm.Exp.invln2 * x + j2cs.fdlibm.FdLibm.Exp.half[xsb]));
                        t = k;
                        hi = x - t * j2cs.fdlibm.FdLibm.Exp.ln2HI[0];
                        lo = t * j2cs.fdlibm.FdLibm.Exp.ln2LO[0];
                    }
                    x = hi - lo;
                }
                else if (hx < 1043333120)
                {
                    if (j2cs.fdlibm.FdLibm.Exp.huge + x > 1.0d)
                    {
                        return 1.0d + x;
                    }
                }
                else
                {
                    k = 0;
                }
                t = x * x;
                c = x - t * (j2cs.fdlibm.FdLibm.Exp.P1 + t * (j2cs.fdlibm.FdLibm.Exp.P2 + t * (j2cs.fdlibm.FdLibm.Exp.P3 + t * (j2cs.fdlibm.FdLibm.Exp.P4 + t * j2cs.fdlibm.FdLibm.Exp.P5))));
                if (k == 0)
                {
                    return 1.0d - ((x * c) / (c - 2.0d) - x);
                }
                else
                {
                    y = 1.0d - ((lo - (x * c) / (2.0d - c)) - hi);
                }
                if (k >= (-1021))
                {
                    y = j2cs.fdlibm.FdLibm.__HI(y, j2cs.fdlibm.FdLibm.__HI(y) + (k << 20));
                    return y;
                }
                else
                {
                    y = j2cs.fdlibm.FdLibm.__HI(y, j2cs.fdlibm.FdLibm.__HI(y) + ((k + 1000) << 20));
                    return y * j2cs.fdlibm.FdLibm.Exp.twom1000;
                }
            }
            static Exp()
            {
                Exp.half = new double[] { 0.5d, -0.5d };
                Exp.ln2HI = new double[] { 0.6931471803691238d, -0.6931471803691238d };
                Exp.ln2LO = new double[] { 1.9082149292705877E-10d, -1.9082149292705877E-10d };
            }
            public Exp() : base()
            {
                throw new System.NotSupportedException();
            }
        }
        // Java: 2566
        public class Log : java.lang.Object
        {
            public const double ln2_hi = 0.6931471803691238d;
            public const double ln2_lo = 1.9082149292705877E-10d;
            public const double Lg1 = 0.6666666666666735d;
            public const double Lg2 = 0.3999999999940942d;
            public const double Lg3 = 0.2857142874366239d;
            public const double Lg4 = 0.22222198432149784d;
            public const double Lg5 = 0.1818357216161805d;
            public const double Lg6 = 0.15313837699209373d;
            public const double Lg7 = 0.14798198605116586d;
            // Java: 2581
            public static double compute(double x)
            {
                double hfsq = default;
                double f = default;
                double s = default;
                double z = default;
                double R = default;
                double w = default;
                double t1 = default;
                double t2 = default;
                double dk = default;
                int k = default;
                int hx = default;
                int i = default;
                int j = default;
                int lx = default;
                hx = j2cs.fdlibm.FdLibm.__HI(x);
                lx = j2cs.fdlibm.FdLibm.__LO(x);
                k = 0;
                if (hx < 1048576)
                {
                    if (((hx & j2cs.fdlibm.FdLibm.EXP_SIGNIF_BITS) | lx) == 0)
                    {
                        return -j2cs.fdlibm.FdLibm.TWO54 / 0.0d;
                    }
                    if (hx < 0)
                    {
                        return (x - x) / 0.0d;
                    }
                    k -= 54;
                    x *= j2cs.fdlibm.FdLibm.TWO54;
                    hx = j2cs.fdlibm.FdLibm.__HI(x);
                }
                if (hx >= j2cs.fdlibm.FdLibm.EXP_BITS)
                {
                    return x + x;
                }
                k += (hx >> 20) - 1023;
                hx &= 1048575;
                i = (hx + 614244) & 1048576;
                x = j2cs.fdlibm.FdLibm.__HI(x, hx | (i ^ 1072693248));
                k += (i >> 20);
                f = x - 1.0d;
                if ((1048575 & (2 + hx)) < 3)
                {
                    if (f == 0.0d)
                    {
                        if (k == 0)
                        {
                            return 0.0d;
                        }
                        else
                        {
                            dk = ((double)(k));
                            return dk * j2cs.fdlibm.FdLibm.Log.ln2_hi + dk * j2cs.fdlibm.FdLibm.Log.ln2_lo;
                        }
                    }
                    R = f * f * (0.5d - 0.3333333333333333d * f);
                    if (k == 0)
                    {
                        return f - R;
                    }
                    else
                    {
                        dk = ((double)(k));
                        return dk * j2cs.fdlibm.FdLibm.Log.ln2_hi - ((R - dk * j2cs.fdlibm.FdLibm.Log.ln2_lo) - f);
                    }
                }
                s = f / (2.0d + f);
                dk = ((double)(k));
                z = s * s;
                i = hx - 398458;
                w = z * z;
                j = 440401 - hx;
                t1 = w * (j2cs.fdlibm.FdLibm.Log.Lg2 + w * (j2cs.fdlibm.FdLibm.Log.Lg4 + w * j2cs.fdlibm.FdLibm.Log.Lg6));
                t2 = z * (j2cs.fdlibm.FdLibm.Log.Lg1 + w * (j2cs.fdlibm.FdLibm.Log.Lg3 + w * (j2cs.fdlibm.FdLibm.Log.Lg5 + w * j2cs.fdlibm.FdLibm.Log.Lg7)));
                i |= j;
                R = t2 + t1;
                if (i > 0)
                {
                    hfsq = 0.5d * f * f;
                    if (k == 0)
                    {
                        return f - (hfsq - s * (hfsq + R));
                    }
                    else
                    {
                        return dk * j2cs.fdlibm.FdLibm.Log.ln2_hi - ((hfsq - (s * (hfsq + R) + dk * j2cs.fdlibm.FdLibm.Log.ln2_lo)) - f);
                    }
                }
                else
                {
                    if (k == 0)
                    {
                        return f - s * (f - R);
                    }
                    else
                    {
                        return dk * j2cs.fdlibm.FdLibm.Log.ln2_hi - ((s * (f - R) - dk * j2cs.fdlibm.FdLibm.Log.ln2_lo) - f);
                    }
                }
            }
            public Log() : base()
            {
                throw new System.NotSupportedException();
            }
        }
        // Java: 2686
        public class Log10 : java.lang.Object
        {
            public const double ivln10 = 0.4342944819032518d;
            public const double log10_2hi = 0.30102999566361177d;
            public const double log10_2lo = 3.694239077158931E-13d;
            // Java: 2696
            public static double compute(double x)
            {
                double y = default;
                double z = default;
                int i = default;
                int k = default;
                int hx = j2cs.fdlibm.FdLibm.__HI(x);
                int lx = j2cs.fdlibm.FdLibm.__LO(x);
                k = 0;
                if (hx < 1048576)
                {
                    if (((hx & j2cs.fdlibm.FdLibm.EXP_SIGNIF_BITS) | lx) == 0)
                    {
                        return -j2cs.fdlibm.FdLibm.TWO54 / 0.0d;
                    }
                    if (hx < 0)
                    {
                        return (x - x) / 0.0d;
                    }
                    k -= 54;
                    x *= j2cs.fdlibm.FdLibm.TWO54;
                    hx = j2cs.fdlibm.FdLibm.__HI(x);
                }
                if (hx >= j2cs.fdlibm.FdLibm.EXP_BITS)
                {
                    return x + x;
                }
                k += (hx >> 20) - 1023;
                i = java.lang.JC.ushr((k & j2cs.fdlibm.FdLibm.SIGN_BIT), 31);
                hx = (hx & 1048575) | ((1023 - i) << 20);
                y = ((double)((k + i)));
                x = j2cs.fdlibm.FdLibm.__HI(x, hx);
                z = y * j2cs.fdlibm.FdLibm.Log10.log10_2lo + j2cs.fdlibm.FdLibm.Log10.ivln10 * java.lang.Math.log(x);
                return z + y * j2cs.fdlibm.FdLibm.Log10.log10_2hi;
            }
            public Log10() : base()
            {
                throw new System.NotSupportedException();
            }
        }
        // Java: 2795
        public class Log1p : java.lang.Object
        {
            public const double ln2_hi = 0.6931471803691238d;
            public const double ln2_lo = 1.9082149292705877E-10d;
            public const double Lp1 = 0.6666666666666735d;
            public const double Lp2 = 0.3999999999940942d;
            public const double Lp3 = 0.2857142874366239d;
            public const double Lp4 = 0.22222198432149784d;
            public const double Lp5 = 0.1818357216161805d;
            public const double Lp6 = 0.15313837699209373d;
            public const double Lp7 = 0.14798198605116586d;
            // Java: 2806
            public static double compute(double x)
            {
                double hfsq = default;
                double f = 0;
                double c = 0;
                double s = default;
                double z = default;
                double R = default;
                double u = default;
                int k = default;
                int hx = default;
                int hu = 0;
                int ax = default;
                hx = j2cs.fdlibm.FdLibm.__HI(x);
                ax = hx & j2cs.fdlibm.FdLibm.EXP_SIGNIF_BITS;
                k = 1;
                if (hx < 1071284858)
                {
                    if (ax >= 1072693248)
                    {
                        if (x == -1.0d)
                        {
                            return -j2cs.fdlibm.FdLibm.INFINITY;
                        }
                        else
                        {
                            return java.lang.Double.NaN;
                        }
                    }
                    if (ax < 1042284544)
                    {
                        if (j2cs.fdlibm.FdLibm.TWO54 + x > 0.0d && ax < 1016070144)
                        {
                            return x;
                        }
                        else
                        {
                            return x - x * x * 0.5d;
                        }
                    }
                    if (hx > 0 || hx <= (-1076707645))
                    {
                        k = 0;
                        f = x;
                        hu = 1;
                    }
                }
                if (hx >= j2cs.fdlibm.FdLibm.EXP_BITS)
                {
                    return x + x;
                }
                if (k != 0)
                {
                    if (hx < 1128267776)
                    {
                        u = 1.0d + x;
                        hu = j2cs.fdlibm.FdLibm.__HI(u);
                        k = (hu >> 20) - 1023;
                        c = (k > 0 ? 1.0d - (u - x) : x - (u - 1.0d));
                        c /= u;
                    }
                    else
                    {
                        u = x;
                        hu = j2cs.fdlibm.FdLibm.__HI(u);
                        k = (hu >> 20) - 1023;
                        c = 0;
                    }
                    hu &= 1048575;
                    if (hu < 434334)
                    {
                        u = j2cs.fdlibm.FdLibm.__HI(u, hu | 1072693248);
                    }
                    else
                    {
                        k += 1;
                        u = j2cs.fdlibm.FdLibm.__HI(u, hu | 1071644672);
                        hu = (1048576 - hu) >> 2;
                    }
                    f = u - 1.0d;
                }
                hfsq = 0.5d * f * f;
                if (hu == 0)
                {
                    if (f == 0.0d)
                    {
                        if (k == 0)
                        {
                            return 0.0d;
                        }
                        else
                        {
                            c += k * j2cs.fdlibm.FdLibm.Log1p.ln2_lo;
                            return k * j2cs.fdlibm.FdLibm.Log1p.ln2_hi + c;
                        }
                    }
                    R = hfsq * (1.0d - 0.6666666666666666d * f);
                    if (k == 0)
                    {
                        return f - R;
                    }
                    else
                    {
                        return k * j2cs.fdlibm.FdLibm.Log1p.ln2_hi - ((R - (k * j2cs.fdlibm.FdLibm.Log1p.ln2_lo + c)) - f);
                    }
                }
                s = f / (2.0d + f);
                z = s * s;
                R = z * (j2cs.fdlibm.FdLibm.Log1p.Lp1 + z * (j2cs.fdlibm.FdLibm.Log1p.Lp2 + z * (j2cs.fdlibm.FdLibm.Log1p.Lp3 + z * (j2cs.fdlibm.FdLibm.Log1p.Lp4 + z * (j2cs.fdlibm.FdLibm.Log1p.Lp5 + z * (j2cs.fdlibm.FdLibm.Log1p.Lp6 + z * j2cs.fdlibm.FdLibm.Log1p.Lp7))))));
                if (k == 0)
                {
                    return f - (hfsq - s * (hfsq + R));
                }
                else
                {
                    return k * j2cs.fdlibm.FdLibm.Log1p.ln2_hi - ((hfsq - (s * (hfsq + R) + (k * j2cs.fdlibm.FdLibm.Log1p.ln2_lo + c))) - f);
                }
            }
            public Log1p() : base()
            {
            }
        }
        // Java: 2987
        public class Expm1 : java.lang.Object
        {
            public const double huge = 1.0E300d;
            public const double tiny = 1.0E-300d;
            public const double o_threshold = 709.782712893384d;
            public const double ln2_hi = 0.6931471803691238d;
            public const double ln2_lo = 1.9082149292705877E-10d;
            public const double invln2 = 1.4426950408889634d;
            public const double Q1 = (-0.03333333333333313d);
            public const double Q2 = 0.0015873015872548146d;
            public const double Q3 = (-7.93650757867488E-5d);
            public const double Q4 = 4.008217827329362E-6d;
            public const double Q5 = (-2.0109921818362437E-7d);
            // Java: 3001
            public static double compute(double x)
            {
                double y = default;
                double hi = default;
                double lo = default;
                double c = 0;
                double t = default;
                double e = default;
                double hxs = default;
                double hfx = default;
                double r1 = default;
                int k = default;
                int xsb = default;
                int hx = default;
                hx = j2cs.fdlibm.FdLibm.__HI(x);
                xsb = hx & j2cs.fdlibm.FdLibm.SIGN_BIT;
                hx &= j2cs.fdlibm.FdLibm.EXP_SIGNIF_BITS;
                if (hx >= 1078159482)
                {
                    if (hx >= 1082535490)
                    {
                        if (hx >= 2146435072)
                        {
                            if (((hx & 1048575) | j2cs.fdlibm.FdLibm.__LO(x)) != 0)
                            {
                                return x + x;
                            }
                            else
                            {
                                return (xsb == 0 ? x : -1.0d);
                            }
                        }
                        if (x > j2cs.fdlibm.FdLibm.Expm1.o_threshold)
                        {
                            return j2cs.fdlibm.FdLibm.Expm1.huge * j2cs.fdlibm.FdLibm.Expm1.huge;
                        }
                    }
                    if (xsb != 0)
                    {
                        if (x + j2cs.fdlibm.FdLibm.Expm1.tiny < 0.0d)
                        {
                            return j2cs.fdlibm.FdLibm.Expm1.tiny - 1.0d;
                        }
                    }
                }
                if (hx > 1071001154)
                {
                    if (hx < 1072734898)
                    {
                        if (xsb == 0)
                        {
                            hi = x - j2cs.fdlibm.FdLibm.Expm1.ln2_hi;
                            lo = j2cs.fdlibm.FdLibm.Expm1.ln2_lo;
                            k = 1;
                        }
                        else
                        {
                            hi = x + j2cs.fdlibm.FdLibm.Expm1.ln2_hi;
                            lo = -j2cs.fdlibm.FdLibm.Expm1.ln2_lo;
                            k = (-1);
                        }
                    }
                    else
                    {
                        k = java.lang.JC.d2i((j2cs.fdlibm.FdLibm.Expm1.invln2 * x + ((xsb == 0 ? 0.5d : -0.5d))));
                        t = k;
                        hi = x - t * j2cs.fdlibm.FdLibm.Expm1.ln2_hi;
                        lo = t * j2cs.fdlibm.FdLibm.Expm1.ln2_lo;
                    }
                    x = hi - lo;
                    c = (hi - x) - lo;
                }
                else if (hx < 1016070144)
                {
                    t = j2cs.fdlibm.FdLibm.Expm1.huge + x;
                    return x - (t - (j2cs.fdlibm.FdLibm.Expm1.huge + x));
                }
                else
                {
                    k = 0;
                }
                hfx = 0.5d * x;
                hxs = x * hfx;
                r1 = 1.0d + hxs * (j2cs.fdlibm.FdLibm.Expm1.Q1 + hxs * (j2cs.fdlibm.FdLibm.Expm1.Q2 + hxs * (j2cs.fdlibm.FdLibm.Expm1.Q3 + hxs * (j2cs.fdlibm.FdLibm.Expm1.Q4 + hxs * j2cs.fdlibm.FdLibm.Expm1.Q5))));
                t = 3.0d - r1 * hfx;
                e = hxs * ((r1 - t) / (6.0d - x * t));
                if (k == 0)
                {
                    return x - (x * e - hxs);
                }
                else
                {
                    e = (x * (e - c) - c);
                    e -= hxs;
                    if (k == (-1))
                    {
                        return 0.5d * (x - e) - 0.5d;
                    }
                    if (k == 1)
                    {
                        if (x < -0.25d)
                        {
                            return -2.0d * (e - (x + 0.5d));
                        }
                        else
                        {
                            return 1.0d + 2.0d * (x - e);
                        }
                    }
                    if (k <= (-2) || k > 56)
                    {
                        y = 1.0d - (e - x);
                        y = j2cs.fdlibm.FdLibm.__HI(y, j2cs.fdlibm.FdLibm.__HI(y) + (k << 20));
                        return y - 1.0d;
                    }
                    t = 1.0d;
                    if (k < 20)
                    {
                        t = j2cs.fdlibm.FdLibm.__HI(t, 1072693248 - (2097152 >> k));
                        y = t - (e - x);
                        y = j2cs.fdlibm.FdLibm.__HI(y, j2cs.fdlibm.FdLibm.__HI(y) + (k << 20));
                    }
                    else
                    {
                        t = j2cs.fdlibm.FdLibm.__HI(t, ((1023 - k) << 20));
                        y = x - (e + t);
                        y += 1.0d;
                        y = j2cs.fdlibm.FdLibm.__HI(y, j2cs.fdlibm.FdLibm.__HI(y) + (k << 20));
                    }
                }
                return y;
            }
            public Expm1() : base()
            {
            }
        }
        // Java: 3117
        public class Sinh : java.lang.Object
        {
            public const double shuge = 1.0E307d;
            // Java: 3122
            public static double compute(double x)
            {
                double t = default;
                double w = default;
                double h = default;
                int ix = default;
                int jx = default;
                int lx = default;
                jx = j2cs.fdlibm.FdLibm.__HI(x);
                ix = jx & j2cs.fdlibm.FdLibm.EXP_SIGNIF_BITS;
                if (ix >= j2cs.fdlibm.FdLibm.EXP_BITS)
                {
                    return x + x;
                }
                h = 0.5d;
                if (jx < 0)
                {
                    h = -h;
                }
                if (ix < 1077280768)
                {
                    if (ix < 1043333120)
                    {
                        if (j2cs.fdlibm.FdLibm.Sinh.shuge + x > 1.0d)
                        {
                            return x;
                        }
                    }
                    t = java.lang.Math.expm1(java.lang.Math.abs(x));
                    if (ix < 1072693248)
                    {
                        return h * (2.0d * t - t * t / (t + 1.0d));
                    }
                    return h * (t + t / (t + 1.0d));
                }
                if (ix < 1082535490)
                {
                    return h * java.lang.Math.exp(java.lang.Math.abs(x));
                }
                lx = j2cs.fdlibm.FdLibm.__LO(x);
                if (ix < 1082536910 || ((ix == 1082536910) && (java.lang.Long.compareUnsigned(lx, (-1883637635)) <= 0)))
                {
                    w = java.lang.Math.exp(0.5d * java.lang.Math.abs(x));
                    t = h * w;
                    return t * w;
                }
                return x * j2cs.fdlibm.FdLibm.Sinh.shuge;
            }
            public Sinh() : base()
            {
                throw new System.NotSupportedException();
            }
        }
        // Java: 3193
        public class Cosh : java.lang.Object
        {
            public const double huge = 1.0E300d;
            // Java: 3198
            public static double compute(double x)
            {
                double t = default;
                double w = default;
                int ix = default;
                int lx = default;
                ix = j2cs.fdlibm.FdLibm.__HI(x);
                ix &= j2cs.fdlibm.FdLibm.EXP_SIGNIF_BITS;
                if (ix >= j2cs.fdlibm.FdLibm.EXP_BITS)
                {
                    return x * x;
                }
                if (ix < 1071001155)
                {
                    t = java.lang.Math.expm1(java.lang.Math.abs(x));
                    w = 1.0d + t;
                    if (ix < 1015021568)
                    {
                        return w;
                    }
                    return 1.0d + (t * t) / (w + w);
                }
                if (ix < 1077280768)
                {
                    t = java.lang.Math.exp(java.lang.Math.abs(x));
                    return 0.5d * t + 0.5d / t;
                }
                if (ix < 1082535490)
                {
                    return 0.5d * java.lang.Math.exp(java.lang.Math.abs(x));
                }
                lx = j2cs.fdlibm.FdLibm.__LO(x);
                if (ix < 1082536910 || ((ix == 1082536910) && (java.lang.Integer.compareUnsigned(lx, (-1883637635)) <= 0)))
                {
                    w = java.lang.Math.exp(0.5d * java.lang.Math.abs(x));
                    t = 0.5d * w;
                    return t * w;
                }
                return j2cs.fdlibm.FdLibm.Cosh.huge * j2cs.fdlibm.FdLibm.Cosh.huge;
            }
            public Cosh() : base()
            {
                throw new System.NotSupportedException();
            }
        }
        // Java: 3271
        public class Tanh : java.lang.Object
        {
            public const double tiny = 1.0E-300d;
            // Java: 3276
            public static double compute(double x)
            {
                double t = default;
                double z = default;
                int jx = default;
                int ix = default;
                jx = j2cs.fdlibm.FdLibm.__HI(x);
                ix = jx & j2cs.fdlibm.FdLibm.EXP_SIGNIF_BITS;
                if (ix >= j2cs.fdlibm.FdLibm.EXP_BITS)
                {
                    if (jx >= 0)
                    {
                        return 1.0d / x + 1.0d;
                    }
                    else
                    {
                        return 1.0d / x - 1.0d;
                    }
                }
                if (ix < 1077280768)
                {
                    if (ix < 1015021568)
                    {
                        return x * (1.0d + x);
                    }
                    if (ix >= 1072693248)
                    {
                        t = java.lang.Math.expm1(2.0d * java.lang.Math.abs(x));
                        z = 1.0d - 2.0d / (t + 2.0d);
                    }
                    else
                    {
                        t = java.lang.Math.expm1(-2.0d * java.lang.Math.abs(x));
                        z = -t / (t + 2.0d);
                    }
                }
                else
                {
                    z = 1.0d - j2cs.fdlibm.FdLibm.Tanh.tiny;
                }
                return (jx >= 0 ? z : -z);
            }
            public Tanh() : base()
            {
                throw new System.NotSupportedException();
            }
        }
        // Java: 3311
        public class IEEEremainder : java.lang.Object
        {
            // Java: 3314
            public static double compute(double x, double p)
            {
                int hx = default;
                int hp = default;
                int sx = default;
                int lx = default;
                int lp = default;
                double p_half = default;
                hx = j2cs.fdlibm.FdLibm.__HI(x);
                lx = j2cs.fdlibm.FdLibm.__LO(x);
                hp = j2cs.fdlibm.FdLibm.__HI(p);
                lp = j2cs.fdlibm.FdLibm.__LO(p);
                sx = hx & j2cs.fdlibm.FdLibm.SIGN_BIT;
                hp &= j2cs.fdlibm.FdLibm.EXP_SIGNIF_BITS;
                hx &= j2cs.fdlibm.FdLibm.EXP_SIGNIF_BITS;
                if ((hp | lp) == 0)
                {
                    return (x * p) / (x * p);
                }
                if ((hx >= j2cs.fdlibm.FdLibm.EXP_BITS) || ((hp >= j2cs.fdlibm.FdLibm.EXP_BITS) && (((hp - j2cs.fdlibm.FdLibm.EXP_BITS) | lp) != 0)))
                {
                    return (x * p) / (x * p);
                }
                if (hp <= 2145386495)
                {
                    x = j2cs.fdlibm.FdLibm.IEEEremainder.__ieee754_fmod(x, p + p);
                }
                if (((hx - hp) | (lx - lp)) == 0)
                {
                    return 0.0d * x;
                }
                x = java.lang.Math.abs(x);
                p = java.lang.Math.abs(p);
                if (hp < 2097152)
                {
                    if (x + x > p)
                    {
                        x -= p;
                        if (x + x >= p)
                        {
                            x -= p;
                        }
                    }
                }
                else
                {
                    p_half = 0.5d * p;
                    if (x > p_half)
                    {
                        x -= p;
                        if (x >= p_half)
                        {
                            x -= p;
                        }
                    }
                }
                return j2cs.fdlibm.FdLibm.__HI(x, j2cs.fdlibm.FdLibm.__HI(x) ^ sx);
            }
            // Java: 3363
            public static double __ieee754_fmod(double x, double y)
            {
                int n = default;
                int hx = default;
                int hy = default;
                int hz = default;
                int ix = default;
                int iy = default;
                int sx = default;
                int lx = default;
                int ly = default;
                int lz = default;
                hx = j2cs.fdlibm.FdLibm.__HI(x);
                lx = j2cs.fdlibm.FdLibm.__LO(x);
                hy = j2cs.fdlibm.FdLibm.__HI(y);
                ly = j2cs.fdlibm.FdLibm.__LO(y);
                sx = hx & j2cs.fdlibm.FdLibm.SIGN_BIT;
                hx ^= sx;
                hy &= j2cs.fdlibm.FdLibm.EXP_SIGNIF_BITS;
                if ((hy | ly) == 0 || (hx >= j2cs.fdlibm.FdLibm.EXP_BITS) || ((hy | (java.lang.JC.ushr((ly | -ly), 31))) > j2cs.fdlibm.FdLibm.EXP_BITS))
                {
                    return (x * y) / (x * y);
                }
                if (hx <= hy)
                {
                    if ((hx < hy) || (java.lang.Integer.compareUnsigned(lx, ly) < 0))
                    {
                        return x;
                    }
                    if (lx == ly)
                    {
                        return j2cs.fdlibm.FdLibm.IEEEremainder.signedZero(sx);
                    }
                }
                ix = j2cs.fdlibm.FdLibm.IEEEremainder.ilogb(hx, lx);
                iy = j2cs.fdlibm.FdLibm.IEEEremainder.ilogb(hy, ly);
                if (ix >= (-1022))
                {
                    hx = 1048576 | (1048575 & hx);
                }
                else
                {
                    n = (-1022) - ix;
                    if (n <= 31)
                    {
                        hx = (hx << n) | (java.lang.JC.ushr(lx, (32 - n)));
                        lx <<= n;
                    }
                    else
                    {
                        hx = lx << (n - 32);
                        lx = 0;
                    }
                }
                if (iy >= (-1022))
                {
                    hy = 1048576 | (1048575 & hy);
                }
                else
                {
                    n = (-1022) - iy;
                    if (n <= 31)
                    {
                        hy = (hy << n) | (java.lang.JC.ushr(ly, (32 - n)));
                        ly <<= n;
                    }
                    else
                    {
                        hy = ly << (n - 32);
                        ly = 0;
                    }
                }
                n = ix - iy;
                while (n-- != 0)
                {
                    hz = hx - hy;
                    lz = lx - ly;
                    if (java.lang.Integer.compareUnsigned(lx, ly) < 0)
                    {
                        hz -= 1;
                    }
                    if (hz < 0)
                    {
                        hx = hx + hx + (java.lang.JC.ushr(lx, 31));
                        lx = lx + lx;
                    }
                    else
                    {
                        if ((hz | lz) == 0)
                        {
                            return j2cs.fdlibm.FdLibm.IEEEremainder.signedZero(sx);
                        }
                        hx = hz + hz + (java.lang.JC.ushr(lz, 31));
                        lx = lz + lz;
                    }
                }
                hz = hx - hy;
                lz = lx - ly;
                if (java.lang.Integer.compareUnsigned(lx, ly) < 0)
                {
                    hz -= 1;
                }
                if (hz >= 0)
                {
                    hx = hz;
                    lx = lz;
                }
                if ((hx | lx) == 0)
                {
                    return j2cs.fdlibm.FdLibm.IEEEremainder.signedZero(sx);
                }
                while (hx < 1048576)
                {
                    hx = hx + hx + (java.lang.JC.ushr(lx, 31));
                    lx = lx + lx;
                    iy -= 1;
                }
                if (iy >= (-1022))
                {
                    hx = ((hx - 1048576) | ((iy + 1023) << 20));
                    x = j2cs.fdlibm.FdLibm.__HI_LO(hx | sx, lx);
                }
                else
                {
                    n = (-1022) - iy;
                    if (n <= 20)
                    {
                        lx = (java.lang.JC.ushr(lx, n)) | (hx << (32 - n));
                        hx >>= n;
                    }
                    else if (n <= 31)
                    {
                        lx = (hx << (32 - n)) | (java.lang.JC.ushr(lx, n));
                        hx = sx;
                    }
                    else
                    {
                        lx = hx >> (n - 32);
                        hx = sx;
                    }
                    x = j2cs.fdlibm.FdLibm.__HI_LO(hx | sx, lx);
                    x *= 1.0d;
                }
                return x;
            }
            // Java: 3480
            public static double signedZero(int sign)
            {
                return +0.0d * (((double)(sign)));
            }
            // Java: 3484
            public static int ilogb(int hz, int lz)
            {
                int iz = default;
                int i = default;
                if (hz < 1048576)
                {
                    if (hz == 0)
                    {
                        for (iz = (-1043), i = lz; i > 0; i <<= 1)
                        {
                            iz -= 1;
                        }
                    }
                    else
                    {
                        for (iz = (-1022), i = (hz << 11); i > 0; i <<= 1)
                        {
                            iz -= 1;
                        }
                    }
                }
                else
                {
                    iz = (hz >> 20) - 1023;
                }
                return iz;
            }
            public IEEEremainder() : base()
            {
                throw new System.NotSupportedException();
            }
        }
        public FdLibm() : base()
        {
            throw new System.NotSupportedException("No FdLibm instances for you.");
        }
    }
}
