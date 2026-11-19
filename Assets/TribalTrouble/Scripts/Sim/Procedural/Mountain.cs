// Ported from Java (Tribal Trouble 1.4.1). Mechanically transliterated by PortTools/j2cs.py,
// then reviewed by hand. Java identifiers are kept so this file diffs against its source.
using System;
using System.Collections.Generic;
using TribalTrouble.Sim.Determinism;
using TribalTrouble.Sim;

namespace TribalTrouble.Sim.Procedural
{
    public sealed class Mountain {

        private readonly JavaRandom random;
        public Channel channel;

        public Mountain(int size, int base_freq, float pers, long seed) {
            System.Diagnostics.Debug.Assert(Utils.isPowerOf2(size));
            int iterations = Utils.powerOf2Log2(size);
            base_freq = J.Max(base_freq, 0);
            base_freq = J.Min(base_freq, iterations);
            random = new JavaRandom(seed);
            channel = new Channel(size, size);

            if (base_freq > 0) {
                int block_size0 = size >> base_freq; // renamed: C# forbids reusing x/y/x_block/y_block declared later in the method
                for (int xb = 0; xb < (1 << base_freq); xb++) {
                    for (int yb = 0; yb < (1 << base_freq); yb++) {
                        int x0 = xb * block_size0;
                        int y0 = yb * block_size0;
                        channel.putPixel(x0, y0, random.NextFloat());
                    }
                }
            }

            float v1, v2, v3, v4, v5, v6, v7, v8, v9;
            int x_block, y_block, x, y;

            for (int i = base_freq; i < iterations; i++) {
                int block_size = size >> i;
                int block_size_half = size >> (i + 1);
                float amp = (float) J.Pow(pers, i - base_freq);
                float amp_half = (float)(0.5f * amp);
                float avr;
                // calculate center midpoints
                if (i < 2) {
                    for (x_block = 0, x = 0; x_block < (1 << i); x_block++) {
                        for (y_block = 0, y = 0; y_block < (1 << i); y_block++) {
                            v1 = channel.getPixel(x, y);
                            v2 = channel.getPixel((x + block_size) % size, y);
                            v3 = channel.getPixel(x, (y + block_size) % size);
                            v4 = channel.getPixel((x + block_size) % size, (y + block_size) % size);
                            avr = (float)(0.25f * ((float)((float)((float)(v1 + v2) + v3) + v4)));
                            v5 = (float)(avr * ((float)((float)(1f + (float)(random.NextFloat() * amp)) - amp_half)));
                            channel.putPixel(x + block_size_half, y + block_size_half, v5);
                            y += block_size;
                        }
                        x += block_size;
                    }
                } else {
                    // safe blocks
                    for (x_block = 1, x = block_size; x_block < (1 << i) - 1; x_block++) {
                        for (y_block = 1, y = block_size; y_block < (1 << i) - 1; y_block++) {
                            v1 = channel.getPixel(x, y);
                            v2 = channel.getPixel(x + block_size, y);
                            v3 = channel.getPixel(x, y + block_size);
                            v4 = channel.getPixel(x + block_size, y + block_size);
                            avr = (float)(0.25f * ((float)((float)((float)(v1 + v2) + v3) + v4)));
                            v5 = (float)(avr * ((float)((float)(1f + (float)(random.NextFloat() * amp)) - amp_half)));
                            channel.putPixel(x + block_size_half, y + block_size_half, v5);
                            y += block_size;
                        }
                        x += block_size;
                    }
                    // left and right edge blocks
                    for (x_block = 0; x_block < (1 << i); x_block += (1 << i) - 1) {
                        x = x_block * block_size;
                        for (y_block = 0, y = 0; y_block < (1 << i); y_block++) {
                            v1 = channel.getPixel(x, y);
                            v2 = channel.getPixel((x + block_size) % size, y);
                            v3 = channel.getPixel(x, (y + block_size) % size);
                            v4 = channel.getPixel((x + block_size) % size, (y + block_size) % size);
                            avr = (float)(0.25f * ((float)((float)((float)(v1 + v2) + v3) + v4)));
                            v5 = (float)(avr * ((float)((float)(1f + (float)(random.NextFloat() * amp)) - amp_half)));
                            channel.putPixel(x + block_size_half, y + block_size_half, v5);
                            y += block_size;
                        }
                    }
                    // top and bottom edge blocks
                    for (x_block = 1, x = block_size; x_block < (1 << i) - 1; x_block++) {
                        for (y_block = 0; y_block < (1 << i); y_block += (1 << i) - 1) {
                            y = y_block * block_size;
                            v1 = channel.getPixel(x, y);
                            v2 = channel.getPixel((x + block_size) % size, y);
                            v3 = channel.getPixel(x, (y + block_size) % size);
                            v4 = channel.getPixel((x + block_size) % size, (y + block_size) % size);
                            avr = (float)(0.25f * ((float)((float)((float)(v1 + v2) + v3) + v4)));
                            v5 = (float)(avr * ((float)((float)(1f + (float)(random.NextFloat() * amp)) - amp_half)));
                            channel.putPixel(x + block_size_half, y + block_size_half, v5);
                        }
                        x += block_size;
                    }
                }
                // calculate left and bottom edge midpoints
                if (i < 2) {
                    for (x_block = 0, x = 0; x_block < (1 << i); x_block++) {
                        for (y_block = 0, y = 0; y_block < (1 << i); y_block++) {
                            v1 = channel.getPixel(x, y);
                            v5 = channel.getPixel(x + block_size_half, y + block_size_half);
                            v2 = channel.getPixel((x + block_size) % size, y);
                            v3 = channel.getPixel(x, (y + block_size) % size);
                            v6 = channel.getPixel(((x - block_size_half) + size) % size, (y + block_size_half) % size);
                            v7 = channel.getPixel((x + block_size_half) % size, ((y - block_size_half) + size) % size);
                            avr = (float)(0.25f * ((float)((float)((float)(v1 + v3) + v5) + v6)));
                            v8 = (float)(avr * ((float)((float)(1f + (float)(random.NextFloat() * amp)) - amp_half)));
                            avr = (float)(0.25f * ((float)((float)((float)(v1 + v2) + v5) + v7)));
                            v9 = (float)(avr * ((float)((float)(1f + (float)(random.NextFloat() * amp)) - amp_half)));
                            channel.putPixel(x, y + block_size_half, v8);
                            channel.putPixel(x + block_size_half, y, v9);
                            y += block_size;
                        }
                        x += block_size;
                    }
                } else {
                    // safe blocks
                    for (x_block = 1, x = block_size; x_block < (1 << i) - 1; x_block++) {
                        for (y_block = 1, y = block_size; y_block < (1 << i) - 1; y_block++) {
                            v1 = channel.getPixel(x, y);
                            v5 = channel.getPixel(x + block_size_half, y + block_size_half);
                            v2 = channel.getPixel(x + block_size, y);
                            v3 = channel.getPixel(x, y + block_size);
                            v6 = channel.getPixel(x - block_size_half, y + block_size_half);
                            v7 = channel.getPixel(x + block_size_half, y - block_size_half);
                            avr = (float)(0.25f * ((float)((float)((float)(v1 + v3) + v5) + v6)));
                            v8 = (float)(avr * ((float)((float)(1f + (float)(random.NextFloat() * amp)) - amp_half)));
                            avr = (float)(0.25f * ((float)((float)((float)(v1 + v2) + v5) + v7)));
                            v9 = (float)(avr * ((float)((float)(1f + (float)(random.NextFloat() * amp)) - amp_half)));
                            channel.putPixel(x, y + block_size_half, v8);
                            channel.putPixel(x + block_size_half, y, v9);
                            y += block_size;
                        }
                        x += block_size;
                    }
                    // left and right edge blocks
                    for (x_block = 0; x_block < (1 << i); x_block += (1 << i) - 1) {
                        x = x_block * block_size;
                        for (y_block = 0, y = 0; y_block < (1 << i); y_block++) {
                            v1 = channel.getPixel(x, y);
                            v5 = channel.getPixel(x + block_size_half, y + block_size_half);
                            v2 = channel.getPixel((x + block_size) % size, y);
                            v3 = channel.getPixel(x, (y + block_size) % size);
                            v6 = channel.getPixel(((x - block_size_half) + size) % size, (y + block_size_half) % size);
                            v7 = channel.getPixel((x + block_size_half) % size, ((y - block_size_half) + size) % size);
                            avr = (float)(0.25f * ((float)((float)((float)(v1 + v3) + v5) + v6)));
                            v8 = (float)(avr * ((float)((float)(1f + (float)(random.NextFloat() * amp)) - amp_half)));
                            avr = (float)(0.25f * ((float)((float)((float)(v1 + v2) + v5) + v7)));
                            v9 = (float)(avr * ((float)((float)(1f + (float)(random.NextFloat() * amp)) - amp_half)));
                            channel.putPixel(x, y + block_size_half, v8);
                            channel.putPixel(x + block_size_half, y, v9);
                            y += block_size;
                        }
                    }
                    // top and bottom edge blocks
                    for (x_block = 1, x = block_size; x_block < (1 << i) - 1; x_block++) {
                        for (y_block = 0; y_block < (1 << i); y_block += (1 << i) - 1) {
                            y = y_block * block_size;
                            v1 = channel.getPixel(x, y);
                            v5 = channel.getPixel(x + block_size_half, y + block_size_half);
                            v2 = channel.getPixel((x + block_size) % size, y);
                            v3 = channel.getPixel(x, (y + block_size) % size);
                            v6 = channel.getPixel(((x - block_size_half) + size) % size, (y + block_size_half) % size);
                            v7 = channel.getPixel((x + block_size_half) % size, ((y - block_size_half) + size) % size);
                            avr = (float)(0.25f * ((float)((float)((float)(v1 + v3) + v5) + v6)));
                            v8 = (float)(avr * ((float)((float)(1f + (float)(random.NextFloat() * amp)) - amp_half)));
                            avr = (float)(0.25f * ((float)((float)((float)(v1 + v2) + v5) + v7)));
                            v9 = (float)(avr * ((float)((float)(1f + (float)(random.NextFloat() * amp)) - amp_half)));
                            channel.putPixel(x, y + block_size_half, v8);
                            channel.putPixel(x + block_size_half, y, v9);
                        }
                        x += block_size;
                    }
                }
            }
            channel.dynamicRange();
        }

        public Layer toLayer() {
            return new Layer(channel, channel.copy(), channel.copy());
        }

        public Channel toChannel() {
            return channel;
        }

    }
}
