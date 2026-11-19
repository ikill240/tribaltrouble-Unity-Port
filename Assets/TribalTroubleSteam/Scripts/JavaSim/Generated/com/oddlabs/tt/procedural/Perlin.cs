// Translated from Perlin.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.procedural
{
    // Java: 11
    public class Perlin : java.lang.Object
    {
        // Java: 13
        public sealed class Interpolation : java.lang.Enum
        {
            public static Interpolation[] values() { return (Interpolation[])VALUES_.Clone(); }
            public static Interpolation valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly Interpolation LINEAR = new Interpolation("LINEAR", 0);
            public static readonly Interpolation SMOOTH = new Interpolation("SMOOTH", 1);
            public static readonly Interpolation CUBIC = new Interpolation("CUBIC", 2);
            public static readonly Interpolation AUTO = new Interpolation("AUTO", 3);
            public static readonly Interpolation[] VALUES_ = new Interpolation[] { LINEAR, SMOOTH, CUBIC, AUTO };
            public Interpolation(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        // Java: 20
        public sealed class Summation : java.lang.Enum
        {
            public static Summation[] values() { return (Summation[])VALUES_.Clone(); }
            public static Summation valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly Summation NORMAL = new Summation("NORMAL", 0);
            public static readonly Summation ABS = new Summation("ABS", 1);
            public static readonly Summation SINE = new Summation("SINE", 2);
            public static readonly Summation XSINE = new Summation("XSINE", 3);
            public static readonly Summation NORMAL_MOD = new Summation("NORMAL_MOD", 4);
            public static readonly Summation ABS_MOD = new Summation("ABS_MOD", 5);
            public static readonly Summation XSINE_MOD = new Summation("XSINE_MOD", 6);
            public static readonly Summation WOOD1 = new Summation("WOOD1", 7);
            public static readonly Summation WOOD2 = new Summation("WOOD2", 8);
            public static readonly Summation[] VALUES_ = new Summation[] { NORMAL, ABS, SINE, XSINE, NORMAL_MOD, ABS_MOD, XSINE_MOD, WOOD1, WOOD2 };
            public Summation(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        public com.oddlabs.procedural.Channel channel;
        public com.oddlabs.procedural.Channel[] noise_channels;
        // Java: 48
        public void generateNoiseChannels(int x_factor, int y_factor, int oct, long seed)
        {
            x_factor = java.lang.Math.max(2, x_factor);
            y_factor = java.lang.Math.max(2, y_factor);
            noise_channels = new com.oddlabs.procedural.Channel[oct];
            java.util.Random random = new java.util.Random(seed);
            for (int i = 0; i < oct; i++)
            {
                int width_noise = x_factor * (1 << i);
                int height_noise = y_factor * (1 << i);
                noise_channels[i] = new com.oddlabs.procedural.Channel(width_noise, height_noise);
                for (int y = 0; y < height_noise; y++)
                {
                    for (int x = 0; x < width_noise; x++)
                    {
                        noise_channels[i].putPixel(x, y, random.nextFloat());
                    }
                }
            }
        }
        // Java: 66
        public void mergeNoiseChannels(int width, int height, float pers, int oct, com.oddlabs.tt.procedural.Perlin.Interpolation interpolation)
        {
            channel = new com.oddlabs.procedural.Channel(width, height);
            int method_threshold = 0;
            if (((object)interpolation == (object)com.oddlabs.tt.procedural.Perlin.Interpolation.SMOOTH))
            {
                method_threshold = noise_channels.Length;
            }
            else if (((object)interpolation == (object)com.oddlabs.tt.procedural.Perlin.Interpolation.AUTO))
            {
                while (width >> 3 > java.lang.JC.d2i(java.lang.StrictMath.pow(2, method_threshold)))
                {
                    method_threshold++;
                }
            }
            for (int i = 0; i < oct; i++)
            {
                com.oddlabs.procedural.Channel octave = noise_channels[i];
                float amplitude = (float)(2 * ((float)(java.lang.StrictMath.pow(pers, i))));
                float height_ratio = (float)(((float)(octave.height)) / height);
                float width_ratio = (float)(((float)(octave.width)) / width);
                int block_height = height / octave.height;
                int block_width = height / octave.width;
                int y_blocks = octave.height;
                int x_blocks = octave.width;
                int y_block_lo = default;
                int y_block_hi = default;
                int x_block_lo = default;
                int x_block_hi = default;
                int x_pixel = default;
                int y_pixel = default;
                float val = default;
                float val1 = default;
                float val2 = default;
                if (width == octave.width && height == octave.height)
                {
                    for (y_pixel = 0; y_pixel < height; y_pixel++)
                    {
                        for (x_pixel = 0; x_pixel < width; x_pixel++)
                        {
                            channel.putPixel(x_pixel, y_pixel, (float)(channel.getPixel(x_pixel, y_pixel) + (float)(octave.getPixel(x_pixel, y_pixel) * amplitude)));
                        }
                    }
                }
                else if (((object)interpolation != (object)com.oddlabs.tt.procedural.Perlin.Interpolation.CUBIC) && i >= method_threshold)
                {
                    float y_incr1 = default;
                    float y_incr2 = default;
                    float x_incr = default;
                    for (y_block_lo = 0; y_block_lo < y_blocks; y_block_lo++)
                    {
                        y_block_hi = (y_block_lo + 1) % octave.height;
                        for (x_block_lo = 0; x_block_lo < x_blocks; x_block_lo++)
                        {
                            x_block_hi = (x_block_lo + 1) % octave.width;
                            y_incr1 = (float)(((float)(octave.getPixel(x_block_lo, y_block_hi) - octave.getPixel(x_block_lo, y_block_lo))) / block_height);
                            y_incr2 = (float)(((float)(octave.getPixel(x_block_hi, y_block_hi) - octave.getPixel(x_block_hi, y_block_lo))) / block_height);
                            val1 = (float)(octave.getPixel(x_block_lo, y_block_lo) - (float)(0.5f * y_incr1));
                            val2 = (float)(octave.getPixel(x_block_hi, y_block_lo) - (float)(0.5f * y_incr2));
                            for (int y = 0; y < block_height; y++)
                            {
                                y_pixel = y + y_block_lo * block_height;
                                val1 = (float)(val1 + y_incr1);
                                val2 = (float)(val2 + y_incr2);
                                x_incr = (float)(((float)(val2 - val1)) / block_width);
                                val = (float)(val1 - (float)(0.5f * x_incr));
                                for (int x = 0; x < block_width; x++)
                                {
                                    x_pixel = x + x_block_lo * block_width;
                                    val = (float)(val + x_incr);
                                    channel.putPixel(x_pixel, y_pixel, (float)(channel.getPixel(x_pixel, y_pixel) + (float)(val * amplitude)));
                                }
                            }
                        }
                    }
                }
                else if (((object)interpolation != (object)com.oddlabs.tt.procedural.Perlin.Interpolation.CUBIC))
                {
                    float y_coord = default;
                    float x_coord = default;
                    float y_diff = default;
                    float x_diff = default;
                    for (y_block_lo = 0; y_block_lo < y_blocks; y_block_lo++)
                    {
                        y_block_hi = (y_block_lo + 1) % octave.height;
                        for (int y_2 = 0; y_2 < block_height; y_2++)
                        {
                            y_pixel = y_2 + y_block_lo * block_height;
                            y_coord = (float)(y_pixel * height_ratio);
                            y_diff = (float)(y_coord - y_block_lo);
                            for (x_block_lo = 0; x_block_lo < x_blocks; x_block_lo++)
                            {
                                x_block_hi = (x_block_lo + 1) % octave.width;
                                val1 = com.oddlabs.procedural.Tools.interpolateSmooth(octave.getPixel(x_block_lo, y_block_lo), octave.getPixel(x_block_lo, y_block_hi), y_diff);
                                val2 = com.oddlabs.procedural.Tools.interpolateSmooth(octave.getPixel(x_block_hi, y_block_lo), octave.getPixel(x_block_hi, y_block_hi), y_diff);
                                for (int x_2 = 0; x_2 < block_width; x_2++)
                                {
                                    x_pixel = x_2 + x_block_lo * block_width;
                                    x_coord = (float)(x_pixel * width_ratio);
                                    x_diff = (float)(x_coord - x_block_lo);
                                    val = com.oddlabs.procedural.Tools.interpolateSmooth(val1, val2, x_diff);
                                    channel.putPixel(x_pixel, y_pixel, (float)(channel.getPixel(x_pixel, y_pixel) + (float)(val * amplitude)));
                                }
                            }
                        }
                    }
                }
                else
                {
                    float y_coord_2 = default;
                    float x_coord_2 = default;
                    float y_diff_2 = default;
                    float x_diff_2 = default;
                    float val0 = default;
                    float val3 = default;
                    int y_block_lolo = default;
                    int y_block_hihi = default;
                    int x_block_lolo = default;
                    int x_block_hihi = default;
                    for (y_block_lo = 0; y_block_lo < y_blocks; y_block_lo++)
                    {
                        y_block_hi = y_block_lo + 1;
                        y_block_lolo = y_block_lo - 1;
                        y_block_hihi = y_block_hi + 1;
                        for (int y_3 = 0; y_3 < block_height; y_3++)
                        {
                            y_pixel = y_3 + y_block_lo * block_height;
                            y_coord_2 = (float)(y_pixel * height_ratio);
                            y_diff_2 = (float)(y_coord_2 - y_block_lo);
                            for (x_block_lo = 0; x_block_lo < x_blocks; x_block_lo++)
                            {
                                x_block_hi = x_block_lo + 1;
                                x_block_lolo = x_block_lo - 1;
                                x_block_hihi = x_block_hi + 1;
                                val0 = com.oddlabs.procedural.Tools.interpolateCubic(octave.getPixelWrap(x_block_lolo, y_block_lolo), octave.getPixelWrap(x_block_lolo, y_block_lo), octave.getPixelWrap(x_block_lolo, y_block_hi), octave.getPixelWrap(x_block_lolo, y_block_hihi), y_diff_2);
                                val1 = com.oddlabs.procedural.Tools.interpolateCubic(octave.getPixelWrap(x_block_lo, y_block_lolo), octave.getPixelWrap(x_block_lo, y_block_lo), octave.getPixelWrap(x_block_lo, y_block_hi), octave.getPixelWrap(x_block_lo, y_block_hihi), y_diff_2);
                                val2 = com.oddlabs.procedural.Tools.interpolateCubic(octave.getPixelWrap(x_block_hi, y_block_lolo), octave.getPixelWrap(x_block_hi, y_block_lo), octave.getPixelWrap(x_block_hi, y_block_hi), octave.getPixelWrap(x_block_hi, y_block_hihi), y_diff_2);
                                val3 = com.oddlabs.procedural.Tools.interpolateCubic(octave.getPixelWrap(x_block_hihi, y_block_lolo), octave.getPixelWrap(x_block_hihi, y_block_lo), octave.getPixelWrap(x_block_hihi, y_block_hi), octave.getPixelWrap(x_block_hihi, y_block_hihi), y_diff_2);
                                for (int x_3 = 0; x_3 < block_width; x_3++)
                                {
                                    x_pixel = x_3 + x_block_lo * block_width;
                                    x_coord_2 = (float)(x_pixel * width_ratio);
                                    x_diff_2 = (float)(x_coord_2 - x_block_lo);
                                    val = com.oddlabs.procedural.Tools.interpolateCubic(val0, val1, val2, val3, x_diff_2);
                                    channel.putPixelWrap(x_pixel, y_pixel, (float)(channel.getPixelWrap(x_pixel, y_pixel) + (float)(val * amplitude)));
                                }
                            }
                        }
                    }
                }
            }
        }
        // Java: 203
        public void transformImage(int width, int height, com.oddlabs.tt.procedural.Perlin.Summation summation)
        {
            float @value = 0;
            switch ((summation).ordinal())
            {
                case 0 /*NORMAL*/:
                {
                    break;
                }
                case 1 /*ABS*/:
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            channel.putPixel(x, y, java.lang.Math.abs(channel.getPixel(x, y)));
                        }
                    }
                    break;
                }
                case 2 /*SINE*/:
                {
                    for (int y_2 = 0; y_2 < height; y_2++)
                    {
                        for (int x_2 = 0; x_2 < width; x_2++)
                        {
                            channel.putPixel(x_2, y_2, ((float)(java.lang.StrictMath.sin(java.lang.Math.PI * 8 * channel.getPixel(x_2, y_2)))));
                        }
                    }
                    break;
                }
                case 3 /*XSINE*/:
                {
                    for (int y_3 = 0; y_3 < height; y_3++)
                    {
                        for (int x_3 = 0; x_3 < width; x_3++)
                        {
                            channel.putPixel(x_3, y_3, ((float)(java.lang.StrictMath.sin(x_3 / (((float)(width / 256.0f)) * java.lang.Math.PI) + channel.getPixel(x_3, y_3)))));
                        }
                    }
                    break;
                }
                case 4 /*NORMAL_MOD*/:
                {
                    for (int y_4 = 0; y_4 < height; y_4++)
                    {
                        for (int x_4 = 0; x_4 < width; x_4++)
                        {
                            @value = channel.getPixel(x_4, y_4);
                            @value = (float)(@value - java.lang.JC.f2i((@value)));
                            channel.putPixel(x_4, y_4, @value);
                        }
                    }
                    break;
                }
                case 5 /*ABS_MOD*/:
                {
                    for (int y_5 = 0; y_5 < height; y_5++)
                    {
                        for (int x_5 = 0; x_5 < width; x_5++)
                        {
                            @value = java.lang.Math.abs(channel.getPixel(x_5, y_5));
                            @value = (float)(@value - java.lang.JC.f2i((@value)));
                            channel.putPixel(x_5, y_5, @value);
                        }
                    }
                    break;
                }
                case 6 /*XSINE_MOD*/:
                {
                    for (int y_6 = 0; y_6 < height; y_6++)
                    {
                        for (int x_6 = 0; x_6 < width; x_6++)
                        {
                            @value = ((float)(java.lang.StrictMath.sin(x_6 / (((float)(width / 256.0f)) * java.lang.Math.PI) + channel.getPixel(x_6, y_6))));
                            @value = (float)(@value - java.lang.JC.f2i((@value)));
                            channel.putPixel(x_6, y_6, @value);
                        }
                    }
                    break;
                }
                case 7 /*WOOD1*/:
                {
                    for (int y_7 = 0; y_7 < height; y_7++)
                    {
                        for (int x_7 = 0; x_7 < width; x_7++)
                        {
                            @value = (float)(4 * channel.getPixel(x_7, y_7));
                            @value = (float)(@value - java.lang.JC.f2i((@value)));
                            @value = (float)(1 - (float)(@value * @value));
                            channel.putPixel(x_7, y_7, @value);
                        }
                    }
                    break;
                }
                case 8 /*WOOD2*/:
                {
                    for (int y_8 = 0; y_8 < height; y_8++)
                    {
                        for (int x_8 = 0; x_8 < width; x_8++)
                        {
                            @value = (float)(0.5f * ((float)(1 + ((float)(java.lang.StrictMath.sin(x_8 / (((float)(width / 256.0f)) * java.lang.Math.PI) + channel.getPixel(x_8, y_8)))))));
                            @value = (float)(1 - (float)((float)((float)(@value * @value) * @value) * @value));
                            channel.putPixel(x_8, y_8, @value);
                        }
                    }
                    break;
                }
                default:
                {
                    break;
                }
            }
        }
        // Java: 281
        public com.oddlabs.procedural.Layer toLayer()
        {
            return new com.oddlabs.procedural.Layer(channel, channel.copy(), channel.copy());
        }
        // Java: 285
        public com.oddlabs.procedural.Channel toChannel()
        {
            return channel;
        }
        public Perlin(int width, int height, int x_factor, int y_factor, float pers, int oct, long seed, com.oddlabs.tt.procedural.Perlin.Interpolation interpolation, com.oddlabs.tt.procedural.Perlin.Summation summation) : base()
        {
            generateNoiseChannels(x_factor, y_factor, oct, seed);
            mergeNoiseChannels(width, height, pers, oct, interpolation);
            transformImage(width, height, summation);
            channel.dynamicRange();
        }
    }
}
