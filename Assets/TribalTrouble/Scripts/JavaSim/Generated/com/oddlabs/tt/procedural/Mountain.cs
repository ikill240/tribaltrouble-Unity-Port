// Translated from Mountain.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.procedural
{
    // Java: 10
    public class Mountain : java.lang.Object
    {
        public java.util.Random random;
        public com.oddlabs.procedural.Channel channel;
        // Java: 185
        public com.oddlabs.procedural.Layer toLayer()
        {
            return new com.oddlabs.procedural.Layer(channel, channel.copy(), channel.copy());
        }
        // Java: 189
        public com.oddlabs.procedural.Channel toChannel()
        {
            return channel;
        }
        public Mountain(int size, int base_freq, float pers, long seed) : base()
        {
            int iterations = com.oddlabs.util.Utils.powerOf2Log2(size);
            base_freq = java.lang.Math.max(base_freq, 0);
            base_freq = java.lang.Math.min(base_freq, iterations);
            random = new java.util.Random(seed);
            channel = new com.oddlabs.procedural.Channel(size, size);
            if (base_freq > 0)
            {
                int block_size = size >> base_freq;
                for (int x_block = 0; x_block < (1 << base_freq); x_block++)
                {
                    for (int y_block = 0; y_block < (1 << base_freq); y_block++)
                    {
                        int x = x_block * block_size;
                        int y = y_block * block_size;
                        channel.putPixel(x, y, random.nextFloat());
                    }
                }
            }
            float v1 = default;
            float v2 = default;
            float v3 = default;
            float v4 = default;
            float v5 = default;
            float v6 = default;
            float v7 = default;
            float v8 = default;
            float v9 = default;
            int x_block_2 = default;
            int y_block_2 = default;
            int x_2 = default;
            int y_2 = default;
            for (int i = base_freq; i < iterations; i++)
            {
                int block_size_2 = size >> i;
                int block_size_half = size >> (i + 1);
                float amp = ((float)(java.lang.StrictMath.pow(pers, i - base_freq)));
                float amp_half = (float)(0.5f * amp);
                float avr = default;
                if (i < 2)
                {
                    for (x_block_2 = 0, x_2 = 0; x_block_2 < (1 << i); x_block_2++)
                    {
                        for (y_block_2 = 0, y_2 = 0; y_block_2 < (1 << i); y_block_2++)
                        {
                            v1 = channel.getPixel(x_2, y_2);
                            v2 = channel.getPixel((x_2 + block_size_2) % size, y_2);
                            v3 = channel.getPixel(x_2, (y_2 + block_size_2) % size);
                            v4 = channel.getPixel((x_2 + block_size_2) % size, (y_2 + block_size_2) % size);
                            avr = (float)(0.25f * ((float)((float)((float)(v1 + v2) + v3) + v4)));
                            v5 = (float)(avr * ((float)((float)(1.0f + (float)(random.nextFloat() * amp)) - amp_half)));
                            channel.putPixel(x_2 + block_size_half, y_2 + block_size_half, v5);
                            y_2 += block_size_2;
                        }
                        x_2 += block_size_2;
                    }
                }
                else
                {
                    for (x_block_2 = 1, x_2 = block_size_2; x_block_2 < (1 << i) - 1; x_block_2++)
                    {
                        for (y_block_2 = 1, y_2 = block_size_2; y_block_2 < (1 << i) - 1; y_block_2++)
                        {
                            v1 = channel.getPixel(x_2, y_2);
                            v2 = channel.getPixel(x_2 + block_size_2, y_2);
                            v3 = channel.getPixel(x_2, y_2 + block_size_2);
                            v4 = channel.getPixel(x_2 + block_size_2, y_2 + block_size_2);
                            avr = (float)(0.25f * ((float)((float)((float)(v1 + v2) + v3) + v4)));
                            v5 = (float)(avr * ((float)((float)(1.0f + (float)(random.nextFloat() * amp)) - amp_half)));
                            channel.putPixel(x_2 + block_size_half, y_2 + block_size_half, v5);
                            y_2 += block_size_2;
                        }
                        x_2 += block_size_2;
                    }
                    for (x_block_2 = 0; x_block_2 < (1 << i); x_block_2 += (1 << i) - 1)
                    {
                        x_2 = x_block_2 * block_size_2;
                        for (y_block_2 = 0, y_2 = 0; y_block_2 < (1 << i); y_block_2++)
                        {
                            v1 = channel.getPixel(x_2, y_2);
                            v2 = channel.getPixel((x_2 + block_size_2) % size, y_2);
                            v3 = channel.getPixel(x_2, (y_2 + block_size_2) % size);
                            v4 = channel.getPixel((x_2 + block_size_2) % size, (y_2 + block_size_2) % size);
                            avr = (float)(0.25f * ((float)((float)((float)(v1 + v2) + v3) + v4)));
                            v5 = (float)(avr * ((float)((float)(1.0f + (float)(random.nextFloat() * amp)) - amp_half)));
                            channel.putPixel(x_2 + block_size_half, y_2 + block_size_half, v5);
                            y_2 += block_size_2;
                        }
                    }
                    for (x_block_2 = 1, x_2 = block_size_2; x_block_2 < (1 << i) - 1; x_block_2++)
                    {
                        for (y_block_2 = 0; y_block_2 < (1 << i); y_block_2 += (1 << i) - 1)
                        {
                            y_2 = y_block_2 * block_size_2;
                            v1 = channel.getPixel(x_2, y_2);
                            v2 = channel.getPixel((x_2 + block_size_2) % size, y_2);
                            v3 = channel.getPixel(x_2, (y_2 + block_size_2) % size);
                            v4 = channel.getPixel((x_2 + block_size_2) % size, (y_2 + block_size_2) % size);
                            avr = (float)(0.25f * ((float)((float)((float)(v1 + v2) + v3) + v4)));
                            v5 = (float)(avr * ((float)((float)(1.0f + (float)(random.nextFloat() * amp)) - amp_half)));
                            channel.putPixel(x_2 + block_size_half, y_2 + block_size_half, v5);
                        }
                        x_2 += block_size_2;
                    }
                }
                if (i < 2)
                {
                    for (x_block_2 = 0, x_2 = 0; x_block_2 < (1 << i); x_block_2++)
                    {
                        for (y_block_2 = 0, y_2 = 0; y_block_2 < (1 << i); y_block_2++)
                        {
                            v1 = channel.getPixel(x_2, y_2);
                            v5 = channel.getPixel(x_2 + block_size_half, y_2 + block_size_half);
                            v2 = channel.getPixel((x_2 + block_size_2) % size, y_2);
                            v3 = channel.getPixel(x_2, (y_2 + block_size_2) % size);
                            v6 = channel.getPixel(((x_2 - block_size_half) + size) % size, (y_2 + block_size_half) % size);
                            v7 = channel.getPixel((x_2 + block_size_half) % size, ((y_2 - block_size_half) + size) % size);
                            avr = (float)(0.25f * ((float)((float)((float)(v1 + v3) + v5) + v6)));
                            v8 = (float)(avr * ((float)((float)(1.0f + (float)(random.nextFloat() * amp)) - amp_half)));
                            avr = (float)(0.25f * ((float)((float)((float)(v1 + v2) + v5) + v7)));
                            v9 = (float)(avr * ((float)((float)(1.0f + (float)(random.nextFloat() * amp)) - amp_half)));
                            channel.putPixel(x_2, y_2 + block_size_half, v8);
                            channel.putPixel(x_2 + block_size_half, y_2, v9);
                            y_2 += block_size_2;
                        }
                        x_2 += block_size_2;
                    }
                }
                else
                {
                    for (x_block_2 = 1, x_2 = block_size_2; x_block_2 < (1 << i) - 1; x_block_2++)
                    {
                        for (y_block_2 = 1, y_2 = block_size_2; y_block_2 < (1 << i) - 1; y_block_2++)
                        {
                            v1 = channel.getPixel(x_2, y_2);
                            v5 = channel.getPixel(x_2 + block_size_half, y_2 + block_size_half);
                            v2 = channel.getPixel(x_2 + block_size_2, y_2);
                            v3 = channel.getPixel(x_2, y_2 + block_size_2);
                            v6 = channel.getPixel(x_2 - block_size_half, y_2 + block_size_half);
                            v7 = channel.getPixel(x_2 + block_size_half, y_2 - block_size_half);
                            avr = (float)(0.25f * ((float)((float)((float)(v1 + v3) + v5) + v6)));
                            v8 = (float)(avr * ((float)((float)(1.0f + (float)(random.nextFloat() * amp)) - amp_half)));
                            avr = (float)(0.25f * ((float)((float)((float)(v1 + v2) + v5) + v7)));
                            v9 = (float)(avr * ((float)((float)(1.0f + (float)(random.nextFloat() * amp)) - amp_half)));
                            channel.putPixel(x_2, y_2 + block_size_half, v8);
                            channel.putPixel(x_2 + block_size_half, y_2, v9);
                            y_2 += block_size_2;
                        }
                        x_2 += block_size_2;
                    }
                    for (x_block_2 = 0; x_block_2 < (1 << i); x_block_2 += (1 << i) - 1)
                    {
                        x_2 = x_block_2 * block_size_2;
                        for (y_block_2 = 0, y_2 = 0; y_block_2 < (1 << i); y_block_2++)
                        {
                            v1 = channel.getPixel(x_2, y_2);
                            v5 = channel.getPixel(x_2 + block_size_half, y_2 + block_size_half);
                            v2 = channel.getPixel((x_2 + block_size_2) % size, y_2);
                            v3 = channel.getPixel(x_2, (y_2 + block_size_2) % size);
                            v6 = channel.getPixel(((x_2 - block_size_half) + size) % size, (y_2 + block_size_half) % size);
                            v7 = channel.getPixel((x_2 + block_size_half) % size, ((y_2 - block_size_half) + size) % size);
                            avr = (float)(0.25f * ((float)((float)((float)(v1 + v3) + v5) + v6)));
                            v8 = (float)(avr * ((float)((float)(1.0f + (float)(random.nextFloat() * amp)) - amp_half)));
                            avr = (float)(0.25f * ((float)((float)((float)(v1 + v2) + v5) + v7)));
                            v9 = (float)(avr * ((float)((float)(1.0f + (float)(random.nextFloat() * amp)) - amp_half)));
                            channel.putPixel(x_2, y_2 + block_size_half, v8);
                            channel.putPixel(x_2 + block_size_half, y_2, v9);
                            y_2 += block_size_2;
                        }
                    }
                    for (x_block_2 = 1, x_2 = block_size_2; x_block_2 < (1 << i) - 1; x_block_2++)
                    {
                        for (y_block_2 = 0; y_block_2 < (1 << i); y_block_2 += (1 << i) - 1)
                        {
                            y_2 = y_block_2 * block_size_2;
                            v1 = channel.getPixel(x_2, y_2);
                            v5 = channel.getPixel(x_2 + block_size_half, y_2 + block_size_half);
                            v2 = channel.getPixel((x_2 + block_size_2) % size, y_2);
                            v3 = channel.getPixel(x_2, (y_2 + block_size_2) % size);
                            v6 = channel.getPixel(((x_2 - block_size_half) + size) % size, (y_2 + block_size_half) % size);
                            v7 = channel.getPixel((x_2 + block_size_half) % size, ((y_2 - block_size_half) + size) % size);
                            avr = (float)(0.25f * ((float)((float)((float)(v1 + v3) + v5) + v6)));
                            v8 = (float)(avr * ((float)((float)(1.0f + (float)(random.nextFloat() * amp)) - amp_half)));
                            avr = (float)(0.25f * ((float)((float)((float)(v1 + v2) + v5) + v7)));
                            v9 = (float)(avr * ((float)((float)(1.0f + (float)(random.nextFloat() * amp)) - amp_half)));
                            channel.putPixel(x_2, y_2 + block_size_half, v8);
                            channel.putPixel(x_2 + block_size_half, y_2, v9);
                        }
                        x_2 += block_size_2;
                    }
                }
            }
            channel.dynamicRange();
        }
    }
}
