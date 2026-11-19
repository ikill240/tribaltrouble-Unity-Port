// Translated from Channel.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.procedural
{
    // Java: 19
    public class Channel : java.lang.Object
    {
        public const long serialVersionUID = 1L;
        public float[][] pixels;
        public int width;
        public int height;
        public bool powerof2;
        // Java: 36
        public class ChannelVisitor : java.lang.Object
        {
            public com.oddlabs.procedural.Channel this_0;
            public int x;
            public int y;
            public bool done;
            public float @value;
            public com.oddlabs.procedural.Channel channel;
            // Java: 48
            public virtual int[] visitNext()
            {
                while (!done)
                {
                    if (channel.getPixel(x, y) == @value)
                    {
                        break;
                    }
                    x++;
                    if (x >= channel.width)
                    {
                        x = 0;
                        y++;
                        if (y >= channel.height)
                        {
                            done = true;
                            x = (-1);
                            y = (-1);
                        }
                    }
                }
                return new int[] { x, y };
            }
            private void init_ChannelVisitor()
            {
                this.x = 0;
                this.y = 0;
                this.done = false;
            }
            public ChannelVisitor(com.oddlabs.procedural.Channel this_0_, com.oddlabs.procedural.Channel channel, float @value) : base()
            {
                this.this_0 = this_0_;
                init_ChannelVisitor();
                this.channel = channel;
                this.@value = @value;
            }
        }
        // Java: 85
        public com.oddlabs.procedural.Channel.ChannelVisitor visitor(float @value)
        {
            return new com.oddlabs.procedural.Channel.ChannelVisitor(this, this, @value);
        }
        // Java: 94
        public com.oddlabs.procedural.Layer toLayer()
        {
            return new com.oddlabs.procedural.Layer(this.copy(), this.copy(), this.copy());
        }
        // Java: 103
        public long getChecksum()
        {
            sbyte[] bytes = new sbyte[4];
            java.nio.ByteBuffer bytebuffer = java.nio.ByteBuffer.wrap(bytes);
            java.util.zip.Checksum checksum = new java.util.zip.CRC32();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bytebuffer.putFloat(0, getPixel(x, y));
                    checksum.update(bytes, 0, bytes.Length);
                }
            }
            return checksum.getValue();
        }
        // Java: 121
        public int getWidth()
        {
            return width;
        }
        // Java: 130
        public int getHeight()
        {
            return height;
        }
        // Java: 142
        public void putPixel(int x, int y, float @value)
        {
            java.util.Objects.checkIndex(x, width);
            java.util.Objects.checkIndex(y, height);
            pixels[y][x] = @value;
        }
        // Java: 156
        public float getPixel(int x, int y)
        {
            java.util.Objects.checkIndex(x, width);
            java.util.Objects.checkIndex(y, height);
            return pixels[y][x];
        }
        // Java: 167
        public float[][] getPixels()
        {
            return pixels;
        }
        // Java: 179
        public void putPixelWrap(int x, int y, float @value)
        {
            if (this.powerof2)
            {
                if (x < 0 || x >= width)
                {
                    x = (width + x) & (width - 1);
                }
                if (y < 0 || y >= height)
                {
                    y = (height + y) & (height - 1);
                }
                pixels[y][x] = @value;
            }
            else
            {
                if (x < 0 || x >= width || y < 0 || y >= height)
                {
                    pixels[(y + height) % height][(x + width) % width] = @value;
                }
                else
                {
                    pixels[y][x] = @value;
                }
            }
        }
        // Java: 201
        public float getPixelWrap(int x, int y)
        {
            if (this.powerof2)
            {
                if (x < 0 || x >= width)
                {
                    x = (width + x) & (width - 1);
                }
                if (y < 0 || y >= height)
                {
                    y = (height + y) & (height - 1);
                }
                return pixels[y][x];
            }
            else
            {
                if (x < 0 || x >= width || y < 0 || y >= height)
                {
                    return pixels[(y + height) % height][(x + width) % width];
                }
                else
                {
                    return pixels[y][x];
                }
            }
        }
        // Java: 222
        public float getPixelSafe(int x, int y)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
            {
                return 0.0f;
            }
            else
            {
                return pixels[y][x];
            }
        }
        // Java: 237
        public void putPixelSafe(int x, int y, float @value)
        {
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                pixels[y][x] = @value;
            }
        }
        // Java: 250
        public void putPixelClip(int x, int y, float @value)
        {
            if (@value < 0)
            {
                pixels[y][x] = 0;
            }
            else if (@value > 1)
            {
                pixels[y][x] = 1;
            }
            else
            {
                pixels[y][x] = @value;
            }
        }
        // Java: 262
        public com.oddlabs.procedural.Channel fill(float @value)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    pixels[y][x] = @value;
                }
            }
            return this;
        }
        // Java: 271
        public com.oddlabs.procedural.Channel fill(float @value, float min, float max)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (pixels[y][x] >= min && pixels[y][x] <= max)
                    {
                        pixels[y][x] = @value;
                    }
                }
            }
            return this;
        }
        // Java: 282
        public float findMin()
        {
            float min = java.lang.Float.MAX_VALUE;
            float val = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    val = pixels[y][x];
                    if (val < min)
                    {
                        min = val;
                    }
                }
            }
            return min;
        }
        // Java: 294
        public float findMax()
        {
            float max = java.lang.Float.MIN_VALUE;
            float val = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    val = pixels[y][x];
                    if (val > max)
                    {
                        max = val;
                    }
                }
            }
            return max;
        }
        // Java: 306
        public float[] findMinMax()
        {
            float min = java.lang.Float.MAX_VALUE;
            float max = java.lang.Float.MIN_VALUE;
            float val1 = 0;
            float val2 = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x += 2)
                {
                    val1 = pixels[y][x];
                    val2 = pixels[y][x + 1];
                    if (val1 <= val2)
                    {
                        if (val1 < min)
                        {
                            min = val1;
                        }
                        if (val2 > max)
                        {
                            max = val2;
                        }
                    }
                    else
                    {
                        if (val2 < min)
                        {
                            min = val2;
                        }
                        if (val1 > max)
                        {
                            max = val1;
                        }
                    }
                }
            }
            return new float[] { min, max };
        }
        // Java: 327
        public float sum()
        {
            float sum_v = 0.0f;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    sum_v = (float)(sum_v + pixels[y][x]);
                }
            }
            return sum_v;
        }
        // Java: 337
        public com.oddlabs.procedural.Channel copy()
        {
            com.oddlabs.procedural.Channel channel = new com.oddlabs.procedural.Channel(width, height);
            float[][] new_pixels = channel.getPixels();
            for (int y = 0; y < height; y++)
            {
                java.lang.JSystem.arraycopy(pixels[y], 0, new_pixels[y], 0, width);
            }
            return channel;
        }
        // Java: 346
        public com.oddlabs.procedural.Channel dynamicRange()
        {
            float[] minmax = findMinMax();
            float min = minmax[0];
            float max = minmax[1];
            float factor = (float)(1.0f / ((float)(max - min)));
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    putPixel(x, y, (float)(factor * ((float)(pixels[y][x] - min))));
                }
            }
            return this;
        }
        // Java: 359
        public com.oddlabs.procedural.Channel dynamicRange(float new_min, float new_max)
        {
            float min = findMin();
            float max = findMax();
            float inv_maxmin = (float)(1.0f / ((float)(max - min)));
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    putPixel(x, y, com.oddlabs.procedural.Tools.interpolateLinear(new_min, new_max, (float)(((float)(pixels[y][x] - min)) * inv_maxmin)));
                }
            }
            return this;
        }
        // Java: 371
        public com.oddlabs.procedural.Channel dynamicRange(float min, float max, float new_min, float new_max)
        {
            float val = default;
            float inv_maxmin = (float)(1.0f / ((float)(max - min)));
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    val = getPixel(x, y);
                    if (val >= min && val <= max)
                    {
                        putPixel(x, y, com.oddlabs.procedural.Tools.interpolateLinear(new_min, new_max, (float)(((float)(pixels[y][x] - min)) * inv_maxmin)));
                    }
                    else
                    {
                        if (val < min)
                        {
                            putPixel(x, y, new_min);
                        }
                        else
                        {
                            putPixel(x, y, new_max);
                        }
                    }
                }
            }
            return this;
        }
        // Java: 391
        public com.oddlabs.procedural.Channel dynamicRangeSymmetric()
        {
            float[] minmax = findMinMax();
            float min = minmax[0];
            float max = minmax[1];
            if (min > ((float)(1 - max)))
            {
                min = (float)(1.0f - max);
            }
            if (max < ((float)(1 - min)))
            {
                max = (float)(1.0f - min);
            }
            float factor = (float)(1.0f / ((float)(max - min)));
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    putPixel(x, y, (float)(factor * ((float)(pixels[y][x] - min))));
                }
            }
            return this;
        }
        // Java: 410
        public com.oddlabs.procedural.Channel clip()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    putPixelClip(x, y, getPixel(x, y));
                }
            }
            return this;
        }
        // Java: 419
        public com.oddlabs.procedural.Channel crop(int x_lo, int y_lo, int x_hi, int y_hi)
        {
            int new_width = x_hi - x_lo + 1;
            int new_height = y_hi - y_lo + 1;
            com.oddlabs.procedural.Channel channel = new com.oddlabs.procedural.Channel(new_width, new_height);
            float[][] new_pixels = channel.getPixels();
            for (int y = y_lo; y <= y_hi; y++)
            {
                java.lang.JSystem.arraycopy(pixels[y], x_lo, new_pixels[y - y_lo], 0, new_width);
            }
            return channel;
        }
        // Java: 430
        public com.oddlabs.procedural.Channel cropWrap(int x_lo, int y_lo, int x_hi, int y_hi)
        {
            int new_width = x_hi - x_lo + 1;
            int new_height = y_hi - y_lo + 1;
            com.oddlabs.procedural.Channel channel = new com.oddlabs.procedural.Channel(new_width, new_height);
            for (int y = 0; y < new_height; y++)
            {
                int y_old = y + y_lo;
                for (int x = 0; x < new_width; x++)
                {
                    int x_old = x + x_lo;
                    if (x_old < 0 || x_old >= width || y_old < 0 || y_old >= height)
                    {
                        channel.putPixel(x, y, getPixelWrap(x_old, y_old));
                    }
                    else
                    {
                        channel.putPixel(x, y, getPixel(x_old, y_old));
                    }
                }
            }
            return channel;
        }
        // Java: 448
        public com.oddlabs.procedural.Channel tile(int new_width, int new_height)
        {
            com.oddlabs.procedural.Channel channel = new com.oddlabs.procedural.Channel(new_width, new_height);
            for (int y = 0; y < new_height; y++)
            {
                for (int x = 0; x < new_width; x++)
                {
                    if (x < width && y < height)
                    {
                        channel.putPixel(x, y, getPixel(x, y));
                    }
                    else
                    {
                        channel.putPixel(x, y, getPixelWrap(x, y));
                    }
                }
            }
            pixels = channel.getPixels();
            width = new_width;
            height = new_height;
            return this;
        }
        // Java: 465
        public com.oddlabs.procedural.Channel tileDouble()
        {
            com.oddlabs.procedural.Channel channel = new com.oddlabs.procedural.Channel(width << 1, height << 1);
            float[][] new_pixels = channel.getPixels();
            for (int y = 0; y < height; y++)
            {
                java.lang.JSystem.arraycopy(pixels[y], 0, new_pixels[y], 0, width);
                java.lang.JSystem.arraycopy(pixels[y], 0, new_pixels[y], width, width);
                java.lang.JSystem.arraycopy(pixels[y], 0, new_pixels[y + height], 0, width);
                java.lang.JSystem.arraycopy(pixels[y], 0, new_pixels[y + height], width, width);
            }
            pixels = channel.getPixels();
            width = width << 1;
            height = height << 1;
            return this;
        }
        // Java: 480
        public com.oddlabs.procedural.Channel[] quadSplit()
        {
            com.oddlabs.procedural.Channel channel1 = this.copy().crop(0, 0, (width >> 1) - 1, (height >> 1) - 1);
            com.oddlabs.procedural.Channel channel2 = this.copy().crop(width >> 1, 0, width - 1, (height >> 1) - 1);
            com.oddlabs.procedural.Channel channel3 = this.copy().crop(0, height >> 1, (width >> 1) - 1, height - 1);
            com.oddlabs.procedural.Channel channel4 = this.copy().crop(width >> 1, height >> 1, width - 1, height - 1);
            return new com.oddlabs.procedural.Channel[] { channel1, channel2, channel3, channel4 };
        }
        // Java: 489
        public com.oddlabs.procedural.Channel quadJoin(com.oddlabs.procedural.Channel channel1, com.oddlabs.procedural.Channel channel2, com.oddlabs.procedural.Channel channel3, com.oddlabs.procedural.Channel channel4)
        {
            com.oddlabs.procedural.Channel channel = new com.oddlabs.procedural.Channel(channel1.width << 1, channel1.height << 1);
            channel.place(channel1, 0, 0);
            channel.place(channel2, channel1.width, 0);
            channel.place(channel3, 0, channel1.height);
            channel.place(channel4, channel1.width, channel1.height);
            pixels = channel.getPixels();
            return this;
        }
        // Java: 504
        public com.oddlabs.procedural.Channel offset(int x_offset, int y_offset)
        {
            com.oddlabs.procedural.Channel channel = new com.oddlabs.procedural.Channel(width, height);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    channel.putPixel(x, y, getPixelWrap(x - x_offset, y - y_offset));
                }
            }
            pixels = channel.getPixels();
            return this;
        }
        // Java: 515
        public com.oddlabs.procedural.Channel brightness(float brightness_v)
        {
            if (brightness_v > 1.0f)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        putPixelClip(x, y, (float)(brightness_v * getPixel(x, y)));
                    }
                }
            }
            else
            {
                for (int y_2 = 0; y_2 < height; y_2++)
                {
                    for (int x_2 = 0; x_2 < width; x_2++)
                    {
                        putPixel(x_2, y_2, (float)(brightness_v * getPixel(x_2, y_2)));
                    }
                }
            }
            return this;
        }
        // Java: 532
        public com.oddlabs.procedural.Channel multiply(float factor)
        {
            if (factor == 1)
            {
                return this;
            }
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    putPixel(x, y, (float)(factor * getPixel(x, y)));
                }
            }
            return this;
        }
        // Java: 543
        public com.oddlabs.procedural.Channel power(float exponent)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    putPixel(x, y, ((float)(java.lang.StrictMath.pow(getPixel(x, y), exponent))));
                }
            }
            return this;
        }
        // Java: 552
        public com.oddlabs.procedural.Channel power2()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float val = getPixel(x, y);
                    putPixel(x, y, (float)(val * val));
                }
            }
            return this;
        }
        // Java: 562
        public com.oddlabs.procedural.Channel log()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    putPixel(x, y, ((float)(java.lang.StrictMath.log(getPixel(x, y)))));
                }
            }
            return this;
        }
        // Java: 571
        public com.oddlabs.procedural.Channel add(float add_v)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    putPixel(x, y, (float)(getPixel(x, y) + add_v));
                }
            }
            return this;
        }
        // Java: 580
        public com.oddlabs.procedural.Channel addClip(float add_v)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    putPixelClip(x, y, (float)(getPixel(x, y) + add_v));
                }
            }
            return this;
        }
        // Java: 589
        public com.oddlabs.procedural.Channel contrast(float contrast_v)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    putPixelClip(x, y, (float)(((float)(((float)(getPixel(x, y) - 0.5f)) * contrast_v)) + 0.5f));
                }
            }
            return this;
        }
        // Java: 598
        public com.oddlabs.procedural.Channel gamma(float gamma_v)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    putPixel(x, y, ((float)(java.lang.StrictMath.pow(getPixel(x, y), (float)(1 / gamma_v)))));
                }
            }
            return this;
        }
        // Java: 607
        public com.oddlabs.procedural.Channel gamma2()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float val = (float)(1.0f - getPixel(x, y));
                    putPixel(x, y, (float)(1 - (float)(val * val)));
                }
            }
            return this;
        }
        // Java: 617
        public com.oddlabs.procedural.Channel gamma4()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float val = (float)(1.0f - getPixel(x, y));
                    val = (float)(val * val);
                    putPixel(x, y, (float)(1 - (float)(val * val)));
                }
            }
            return this;
        }
        // Java: 628
        public com.oddlabs.procedural.Channel gamma8()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float val = (float)(1.0f - getPixel(x, y));
                    val = (float)(val * val);
                    val = (float)(val * val);
                    putPixel(x, y, (float)(1 - (float)(val * val)));
                }
            }
            return this;
        }
        // Java: 640
        public com.oddlabs.procedural.Channel gain(float gain_v)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (getPixel(x, y) < 0.5f)
                    {
                        putPixel(x, y, ((float)((java.lang.StrictMath.pow((float)(2 * getPixel(x, y)), java.lang.StrictMath.log((float)(1 - gain_v)) / java.lang.StrictMath.log(0.5d)) / 2.0f))));
                    }
                    else
                    {
                        putPixel(x, y, (float)(1.0f - ((float)((java.lang.StrictMath.pow((float)(2 - (float)(2 * getPixel(x, y))), java.lang.StrictMath.log((float)(1 - gain_v)) / java.lang.StrictMath.log(0.5d)) / 2.0f)))));
                    }
                }
            }
            return this;
        }
        // Java: 653
        public com.oddlabs.procedural.Channel smoothGain()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    putPixel(x, y, com.oddlabs.procedural.Tools.interpolateSmooth(0.0f, 1.0f, getPixel(x, y)));
                }
            }
            return this;
        }
        // Java: 662
        public com.oddlabs.procedural.Channel invert()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    putPixel(x, y, (float)(1.0f - getPixel(x, y)));
                }
            }
            return this;
        }
        // Java: 671
        public com.oddlabs.procedural.Channel threshold(float start, float end)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float val = getPixel(x, y);
                    if (val >= start && val <= end)
                    {
                        putPixel(x, y, 1.0f);
                    }
                    else
                    {
                        putPixel(x, y, 0.0f);
                    }
                }
            }
            return this;
        }
        // Java: 685
        public com.oddlabs.procedural.Channel scale(int new_width, int new_height)
        {
            return this.scaleLinear(new_width, new_height);
        }
        // Java: 689
        public com.oddlabs.procedural.Channel scaleHalf()
        {
            int new_width = width / 2;
            int new_height = height / 2;
            com.oddlabs.procedural.Channel channel = new com.oddlabs.procedural.Channel(new_width, new_height);
            for (int y = 0; y < new_height; y++)
            {
                int y_coord_lo = y * 2;
                int y_coord_hi = y_coord_lo + 1;
                for (int x = 0; x < new_width; x++)
                {
                    int x_coord_lo = x * 2;
                    int x_coord_hi = x_coord_lo + 1;
                    float val1 = com.oddlabs.procedural.Tools.interpolateLinear(getPixelWrap(x_coord_lo, y_coord_lo), getPixelWrap(x_coord_hi, y_coord_lo), 0.5f);
                    float val2 = com.oddlabs.procedural.Tools.interpolateLinear(getPixelWrap(x_coord_lo, y_coord_hi), getPixelWrap(x_coord_hi, y_coord_hi), 0.5f);
                    channel.putPixel(x, y, com.oddlabs.procedural.Tools.interpolateLinear(val1, val2, 0.5f));
                }
            }
            pixels = channel.getPixels();
            width = new_width;
            height = new_height;
            return this;
        }
        // Java: 714
        public com.oddlabs.procedural.Channel scaleLinear(int new_width, int new_height)
        {
            if (width == new_width && height == new_height)
            {
                return this;
            }
            com.oddlabs.procedural.Channel channel = new com.oddlabs.procedural.Channel(new_width, new_height);
            float x_coord = 0;
            float y_coord = 0;
            float val1 = 0;
            float val2 = 0;
            float height_ratio = (float)(((float)(height)) / new_height);
            float width_ratio = (float)(((float)(width)) / new_width);
            for (int y = 0; y < new_height; y++)
            {
                y_coord = (float)((float)(y * height_ratio) - 0.5f);
                int y_coord_lo = java.lang.JC.f2i(y_coord);
                int y_coord_hi = y_coord_lo + 1;
                for (int x = 0; x < new_width; x++)
                {
                    x_coord = (float)((float)(x * width_ratio) - 0.5f);
                    int x_coord_lo = java.lang.JC.f2i(x_coord);
                    int x_coord_hi = x_coord_lo + 1;
                    float x_diff = (float)(x_coord - x_coord_lo);
                    val1 = com.oddlabs.procedural.Tools.interpolateLinear(getPixelWrap(x_coord_lo, y_coord_lo), getPixelWrap(x_coord_hi, y_coord_lo), x_diff);
                    val2 = com.oddlabs.procedural.Tools.interpolateLinear(getPixelWrap(x_coord_lo, y_coord_hi), getPixelWrap(x_coord_hi, y_coord_hi), x_diff);
                    channel.putPixel(x, y, com.oddlabs.procedural.Tools.interpolateLinear(val1, val2, (float)(y_coord - y_coord_lo)));
                }
            }
            pixels = channel.getPixels();
            width = new_width;
            height = new_height;
            return this;
        }
        // Java: 749
        public com.oddlabs.procedural.Channel scaleCubicWrapping(int new_width, int new_height)
        {
            if (width == new_width && height == new_height)
            {
                return this;
            }
            com.oddlabs.procedural.Channel channel = new com.oddlabs.procedural.Channel(new_width, new_height);
            float x_coord = default;
            float y_coord = default;
            float val0 = default;
            float val1 = default;
            float val2 = default;
            float val3 = default;
            float height_ratio = (float)(((float)(height)) / new_height);
            float width_ratio = (float)(((float)(width)) / new_width);
            float x_diff = default;
            float y_diff = default;
            int x_coord_lo = default;
            int x_coord_lolo = default;
            int x_coord_hi = default;
            int x_coord_hihi = default;
            int y_coord_lo = default;
            int y_coord_lolo = default;
            int y_coord_hi = default;
            int y_coord_hihi = default;
            for (int y = 0; y < new_height; y++)
            {
                y_coord = (float)((float)(y * height_ratio) - 0.5f);
                y_coord_lo = java.lang.JC.f2i(y_coord);
                y_coord_lolo = y_coord_lo - 1;
                y_coord_hi = y_coord_lo + 1;
                y_coord_hihi = y_coord_hi + 1;
                y_diff = (float)(y_coord - y_coord_lo);
                for (int x = 0; x < new_width; x++)
                {
                    x_coord = (float)((float)(x * width_ratio) - 0.5f);
                    x_coord_lo = java.lang.JC.f2i(x_coord);
                    x_coord_lolo = x_coord_lo - 1;
                    x_coord_hi = x_coord_lo + 1;
                    x_coord_hihi = x_coord_hi + 1;
                    x_diff = (float)(x_coord - x_coord_lo);
                    val0 = com.oddlabs.procedural.Tools.interpolateCubic(getPixelWrap(x_coord_lolo, y_coord_lolo), getPixelWrap(x_coord_lolo, y_coord_lo), getPixelWrap(x_coord_lolo, y_coord_hi), getPixelWrap(x_coord_lolo, y_coord_hihi), y_diff);
                    val1 = com.oddlabs.procedural.Tools.interpolateCubic(getPixelWrap(x_coord_lo, y_coord_lolo), getPixelWrap(x_coord_lo, y_coord_lo), getPixelWrap(x_coord_lo, y_coord_hi), getPixelWrap(x_coord_lo, y_coord_hihi), y_diff);
                    val2 = com.oddlabs.procedural.Tools.interpolateCubic(getPixelWrap(x_coord_hi, y_coord_lolo), getPixelWrap(x_coord_hi, y_coord_lo), getPixelWrap(x_coord_hi, y_coord_hi), getPixelWrap(x_coord_hi, y_coord_hihi), y_diff);
                    val3 = com.oddlabs.procedural.Tools.interpolateCubic(getPixelWrap(x_coord_hihi, y_coord_lolo), getPixelWrap(x_coord_hihi, y_coord_lo), getPixelWrap(x_coord_hihi, y_coord_hi), getPixelWrap(x_coord_hihi, y_coord_hihi), y_diff);
                    channel.putPixel(x, y, com.oddlabs.procedural.Tools.interpolateCubic(val0, val1, val2, val3, x_diff));
                }
            }
            pixels = channel.getPixels();
            width = new_width;
            height = new_height;
            return this;
        }
        // Java: 807
        public com.oddlabs.procedural.Channel scaleCubicNonWrapping(int new_width, int new_height)
        {
            if (width == new_width && height == new_height)
            {
                return this;
            }
            com.oddlabs.procedural.Channel channel = new com.oddlabs.procedural.Channel(new_width, new_height);
            float x_coord = default;
            float y_coord = default;
            float val0 = default;
            float val1 = default;
            float val2 = default;
            float val3 = default;
            float height_ratio = (float)(((float)(height)) / new_height);
            float width_ratio = (float)(((float)(width)) / new_width);
            float x_diff = default;
            float y_diff = default;
            int x_coord_lo = default;
            int x_coord_lolo = default;
            int x_coord_hi = default;
            int x_coord_hihi = default;
            int y_coord_lo = default;
            int y_coord_lolo = default;
            int y_coord_hi = default;
            int y_coord_hihi = default;
            for (int y = 0; y < new_height; y++)
            {
                y_coord = (float)((float)(y * height_ratio) - 0.5f);
                y_coord_lo = java.lang.JC.f2i(y_coord);
                y_coord_lolo = y_coord_lo - 1;
                y_coord_hi = y_coord_lo + 1;
                y_coord_hihi = y_coord_hi + 1;
                y_diff = (float)(y_coord - y_coord_lo);
                for (int x = 0; x < new_width; x++)
                {
                    x_coord = (float)((float)(x * width_ratio) - 0.5f);
                    x_coord_lo = java.lang.JC.f2i(x_coord);
                    x_coord_lolo = x_coord_lo - 1;
                    x_coord_hi = x_coord_lo + 1;
                    x_coord_hihi = x_coord_hi + 1;
                    x_diff = (float)(x_coord - x_coord_lo);
                    val0 = com.oddlabs.procedural.Tools.interpolateCubic(getPixelSafe(x_coord_lolo, y_coord_lolo), getPixelSafe(x_coord_lolo, y_coord_lo), getPixelSafe(x_coord_lolo, y_coord_hi), getPixelSafe(x_coord_lolo, y_coord_hihi), y_diff);
                    val1 = com.oddlabs.procedural.Tools.interpolateCubic(getPixelSafe(x_coord_lo, y_coord_lolo), getPixelSafe(x_coord_lo, y_coord_lo), getPixelSafe(x_coord_lo, y_coord_hi), getPixelSafe(x_coord_lo, y_coord_hihi), y_diff);
                    val2 = com.oddlabs.procedural.Tools.interpolateCubic(getPixelSafe(x_coord_hi, y_coord_lolo), getPixelSafe(x_coord_hi, y_coord_lo), getPixelSafe(x_coord_hi, y_coord_hi), getPixelSafe(x_coord_hi, y_coord_hihi), y_diff);
                    val3 = com.oddlabs.procedural.Tools.interpolateCubic(getPixelSafe(x_coord_hihi, y_coord_lolo), getPixelSafe(x_coord_hihi, y_coord_lo), getPixelSafe(x_coord_hihi, y_coord_hi), getPixelSafe(x_coord_hihi, y_coord_hihi), y_diff);
                    channel.putPixel(x, y, com.oddlabs.procedural.Tools.interpolateCubic(val0, val1, val2, val3, x_diff));
                }
            }
            pixels = channel.getPixels();
            width = new_width;
            height = new_height;
            return this;
        }
        // Java: 867
        public com.oddlabs.procedural.Channel scaleFast(int new_width, int new_height)
        {
            if (width == new_width && height == new_height)
            {
                return this;
            }
            com.oddlabs.procedural.Channel channel = new com.oddlabs.procedural.Channel(new_width, new_height);
            int x_coord = 0;
            int y_coord = 0;
            for (int y = 0; y < new_height; y++)
            {
                for (int x = 0; x < new_width; x++)
                {
                    x_coord = x * width / new_width;
                    y_coord = y * height / new_height;
                    channel.putPixel(x, y, getPixel(x_coord, y_coord));
                }
            }
            pixels = channel.getPixels();
            width = new_width;
            height = new_height;
            return this;
        }
        // Java: 887
        public com.oddlabs.procedural.Channel scaleDouble()
        {
            com.oddlabs.procedural.Channel filter = new com.oddlabs.procedural.Channel(width << 1, height << 1);
            for (int y = 0; y < height; y++)
            {
                int y_shift = y << 1;
                for (int x = 0; x < width; x++)
                {
                    int x_shift = x << 1;
                    float @value = (float)(0.25f * getPixel(x, y));
                    filter.putPixel(x_shift, y_shift, @value);
                    filter.putPixel(x_shift + 1, y_shift, @value);
                    filter.putPixel(x_shift, y_shift + 1, @value);
                    filter.putPixel(x_shift + 1, y_shift + 1, @value);
                }
            }
            com.oddlabs.procedural.Channel channel = new com.oddlabs.procedural.Channel(width << 1, height << 1);
            for (int y_2 = 1; y_2 < (height << 1) - 1; y_2++)
            {
                for (int x_2 = 1; x_2 < (width << 1) - 1; x_2++)
                {
                    channel.putPixel(x_2, y_2, (float)((float)((float)(filter.getPixel(x_2 - 1, y_2) + filter.getPixel(x_2 + 1, y_2)) + filter.getPixel(x_2, y_2 - 1)) + filter.getPixel(x_2, y_2 + 1)));
                }
            }
            int max = (width << 1) - 1;
            for (int i = 0; i < max; i++)
            {
                channel.putPixel(0, i, (float)((float)((float)(filter.getPixelWrap((-1), i) + filter.getPixelWrap(1, i)) + filter.getPixelWrap(0, i - 1)) + filter.getPixelWrap(0, i + 1)));
                channel.putPixel(i, 0, (float)((float)((float)(filter.getPixelWrap(i, (-1)) + filter.getPixelWrap(i, 1)) + filter.getPixelWrap(i - 1, 0)) + filter.getPixelWrap(i + 1, 0)));
                channel.putPixel(max, i, (float)((float)((float)(filter.getPixelWrap(max - 1, i) + filter.getPixelWrap(max + 1, i)) + filter.getPixelWrap(max, i - 1)) + filter.getPixelWrap(max, i + 1)));
                channel.putPixel(i, max, (float)((float)((float)(filter.getPixelWrap(i, max - 1) + filter.getPixelWrap(i, max + 1)) + filter.getPixelWrap(i - 1, max)) + filter.getPixelWrap(i + 1, max)));
            }
            pixels = channel.getPixels();
            width = width << 1;
            height = height << 1;
            return this;
        }
        // Java: 931
        public com.oddlabs.procedural.Channel rotate(int degrees)
        {
            com.oddlabs.procedural.Channel channel = null;
            int tmp = width;
            switch ((degrees))
            {
                case 90:
                {
                    channel = new com.oddlabs.procedural.Channel(height, width);
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            channel.putPixel(y, width - x - 1, getPixel(x, y));
                        }
                    }
                    width = height;
                    height = tmp;
                    break;
                }
                case 180:
                {
                    channel = new com.oddlabs.procedural.Channel(width, height);
                    for (int y_2 = 0; y_2 < height; y_2++)
                    {
                        for (int x_2 = 0; x_2 < width; x_2++)
                        {
                            channel.putPixel(width - x_2 - 1, height - y_2 - 1, getPixel(x_2, y_2));
                        }
                    }
                    break;
                }
                case 270:
                {
                    channel = new com.oddlabs.procedural.Channel(height, width);
                    for (int y_3 = 0; y_3 < height; y_3++)
                    {
                        for (int x_3 = 0; x_3 < width; x_3++)
                        {
                            channel.putPixel(height - y_3 - 1, x_3, getPixel(x_3, y_3));
                        }
                    }
                    width = height;
                    height = tmp;
                    break;
                }
                default:
                {
                    break;
                }
            }
            pixels = channel.getPixels();
            return this;
        }
        // Java: 970
        public com.oddlabs.procedural.Channel shear(float offset_v)
        {
            com.oddlabs.procedural.Channel channel = new com.oddlabs.procedural.Channel(width, height);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    channel.putPixel(x, y, getPixelWrap(java.lang.JC.f2i(((float)(x + (float)((float)(offset_v * width) * ((float)(((float)(y)) / height)))))), y));
                }
            }
            pixels = channel.getPixels();
            return this;
        }
        // Java: 981
        public com.oddlabs.procedural.Channel sine(int frequency)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    putPixel(x, y, ((float)(java.lang.StrictMath.sin(java.lang.Math.PI * 2 * frequency * getPixel(x, y)))));
                }
            }
            return this;
        }
        // Java: 990
        public com.oddlabs.procedural.Channel xsine(int frequency)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    putPixel(x, y, ((float)(java.lang.StrictMath.sin(2 * java.lang.Math.PI * ((float)((float)(((float)(((float)(x)) / width)) * frequency) + getPixel(x, y)))))));
                }
            }
            return this.dynamicRange();
        }
        // Java: 999
        public com.oddlabs.procedural.Channel perturb(com.oddlabs.procedural.Channel channel1, com.oddlabs.procedural.Channel channel2)
        {
            com.oddlabs.procedural.Channel channel = new com.oddlabs.procedural.Channel(width, height);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float x_coord = (float)(x + (float)(width * channel1.getPixel(x, y)));
                    int x_coord_lo = java.lang.JC.f2i(x_coord);
                    int x_coord_hi = x_coord_lo + 1;
                    float x_frac = (float)(x_coord - x_coord_lo);
                    float y_coord = (float)(y + (float)(height * channel2.getPixel(x, y)));
                    int y_coord_lo = java.lang.JC.f2i(y_coord);
                    int y_coord_hi = y_coord_lo + 1;
                    float y_frac = (float)(y_coord - y_coord_lo);
                    float val1 = com.oddlabs.procedural.Tools.interpolateLinear(getPixelWrap(x_coord_lo, y_coord_lo), getPixelWrap(x_coord_hi, y_coord_lo), x_frac);
                    float val2 = com.oddlabs.procedural.Tools.interpolateLinear(getPixelWrap(x_coord_lo, y_coord_hi), getPixelWrap(x_coord_hi, y_coord_hi), x_frac);
                    channel.putPixel(x, y, com.oddlabs.procedural.Tools.interpolateLinear(val1, val2, y_frac));
                }
            }
            pixels = channel.getPixels();
            return this;
        }
        // Java: 1022
        public com.oddlabs.procedural.Channel perturb(com.oddlabs.procedural.Channel perturb_v, float magnitude)
        {
            com.oddlabs.procedural.Channel channel = new com.oddlabs.procedural.Channel(width, height);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float x_coord = (float)(x + (float)((float)(width * magnitude) * ((float)(perturb_v.getPixel(x, y) - 0.5f))));
                    int x_coord_lo = java.lang.JC.f2i(x_coord);
                    int x_coord_hi = x_coord_lo + 1;
                    float x_frac = (float)(x_coord - x_coord_lo);
                    float y_coord = (float)(y + (float)((float)(height * magnitude) * ((float)(perturb_v.getPixel(y, x) - 0.5f))));
                    int y_coord_lo = java.lang.JC.f2i(y_coord);
                    int y_coord_hi = y_coord_lo + 1;
                    float y_frac = (float)(y_coord - y_coord_lo);
                    float val1 = com.oddlabs.procedural.Tools.interpolateLinear(getPixelWrap(x_coord_lo, y_coord_lo), getPixelWrap(x_coord_hi, y_coord_lo), x_frac);
                    float val2 = com.oddlabs.procedural.Tools.interpolateLinear(getPixelWrap(x_coord_lo, y_coord_hi), getPixelWrap(x_coord_hi, y_coord_hi), x_frac);
                    channel.putPixel(x, y, com.oddlabs.procedural.Tools.interpolateLinear(val1, val2, y_frac));
                }
            }
            pixels = channel.getPixels();
            return this;
        }
        // Java: 1045
        public com.oddlabs.procedural.Channel flipH()
        {
            com.oddlabs.procedural.Channel channel = new com.oddlabs.procedural.Channel(width, height);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    channel.putPixel(x, y, getPixel(width - x - 1, y));
                }
            }
            pixels = channel.getPixels();
            return this;
        }
        // Java: 1056
        public com.oddlabs.procedural.Channel flipV()
        {
            float[] tmp = default;
            for (int y = 0; y < height >> 1; y++)
            {
                tmp = pixels[y];
                pixels[y] = pixels[height - y - 1];
                pixels[height - y - 1] = tmp;
            }
            return this;
        }
        // Java: 1066
        public com.oddlabs.procedural.Channel smoothFast()
        {
            com.oddlabs.procedural.Channel filter = new com.oddlabs.procedural.Channel(width, height);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    filter.putPixel(x, y, (float)(0.25f * getPixel(x, y)));
                }
            }
            for (int y_2 = 0; y_2 < height; y_2++)
            {
                for (int x_2 = 0; x_2 < width; x_2++)
                {
                    putPixel(x_2, y_2, (float)((float)((float)(filter.getPixelWrap(x_2 - 1, y_2) + filter.getPixelWrap(x_2 + 1, y_2)) + filter.getPixelWrap(x_2, y_2 - 1)) + filter.getPixelWrap(x_2, y_2 + 1)));
                }
            }
            return this;
        }
        // Java: 1082
        public com.oddlabs.procedural.Channel smooth(int radius)
        {
            radius = java.lang.Math.max(1, radius);
            com.oddlabs.procedural.Channel filter = new com.oddlabs.procedural.Channel(width, height);
            float factor = (float)(1.0f / ((2 * radius + 1) * (2 * radius + 1)));
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    filter.putPixel(x, y, (float)(factor * getPixel(x, y)));
                }
            }
            for (int x_2 = radius; x_2 < width - radius; x_2++)
            {
                int y_2 = radius;
                float sum_v = 0.0f;
                for (int i = -radius; i < radius + 1; i++)
                {
                    for (int j = -radius; j < radius + 1; j++)
                    {
                        sum_v = (float)(sum_v + filter.getPixel(x_2 + j, y_2 + i));
                    }
                }
                for (y_2++; y_2 < height - radius; y_2++)
                {
                    for (int j_2 = -radius; j_2 < radius + 1; j_2++)
                    {
                        sum_v = (float)(sum_v - filter.getPixel(x_2 + j_2, y_2 - radius - 1));
                        sum_v = (float)(sum_v + filter.getPixel(x_2 + j_2, y_2 + radius));
                    }
                    putPixel(x_2, y_2, sum_v);
                }
            }
            return this;
        }
        // Java: 1110
        public com.oddlabs.procedural.Channel smoothWrap(int radius)
        {
            radius = java.lang.Math.max(1, radius);
            com.oddlabs.procedural.Channel filter = new com.oddlabs.procedural.Channel(width, height);
            float factor = (float)(1.0f / ((2 * radius + 1) * (2 * radius + 1)));
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    filter.putPixel(x, y, (float)(factor * getPixel(x, y)));
                }
            }
            for (int x_2 = 0; x_2 < width; x_2++)
            {
                for (int y_2 = 0; y_2 < height; y_2++)
                {
                    float sum_v = 0.0f;
                    for (int i = -radius; i < radius + 1; i++)
                    {
                        for (int j = -radius; j < radius + 1; j++)
                        {
                            sum_v = (float)(sum_v + filter.getPixelWrap(x_2 + j, y_2 + i));
                        }
                    }
                    putPixel(x_2, y_2, sum_v);
                }
            }
            return this;
        }
        // Java: 1133
        public com.oddlabs.procedural.Channel smooth(int radius, com.oddlabs.procedural.Channel mask)
        {
            radius = java.lang.Math.max(1, radius);
            com.oddlabs.procedural.Channel filter = new com.oddlabs.procedural.Channel(width, height);
            float factor = (float)(1.0f / ((2 * radius + 1) * (2 * radius + 1)));
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    filter.putPixel(x, y, (float)(factor * getPixel(x, y)));
                }
            }
            for (int x_2 = radius; x_2 < width - radius; x_2++)
            {
                int y_2 = radius;
                float sum_v = 0.0f;
                for (int i = -radius; i < radius + 1; i++)
                {
                    for (int j = -radius; j < radius + 1; j++)
                    {
                        sum_v = (float)(sum_v + filter.getPixel(x_2 + j, y_2 + i));
                    }
                }
                for (y_2++; y_2 < height - radius; y_2++)
                {
                    float alpha = mask.getPixel(x_2, y_2);
                    if (alpha > 0)
                    {
                        for (int j_2 = -radius; j_2 < radius + 1; j_2++)
                        {
                            sum_v = (float)(sum_v - filter.getPixel(x_2 + j_2, y_2 - radius - 1));
                            sum_v = (float)(sum_v + filter.getPixel(x_2 + j_2, y_2 + radius));
                        }
                        putPixel(x_2, y_2, (float)((float)(alpha * sum_v) + (float)(((float)(1.0f - alpha)) * getPixel(x_2, y_2))));
                    }
                }
            }
            return this;
        }
        // Java: 1164
        public com.oddlabs.procedural.Channel sharpen(int radius)
        {
            radius = java.lang.Math.max(1, radius);
            com.oddlabs.procedural.Channel channel = new com.oddlabs.procedural.Channel(width, height);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float @value = 0;
                    for (int i = -radius; i <= radius; i++)
                    {
                        for (int j = -radius; j <= radius; j++)
                        {
                            if (i == 0 && j == 0)
                            {
                                @value = (float)(@value + (float)((2 * radius + 1) * (2 * radius + 1) * getPixel(x, y)));
                            }
                            else
                            {
                                @value = (float)(@value - getPixelWrap(x + i, y + j));
                            }
                        }
                    }
                    channel.putPixel(x, y, @value);
                }
            }
            pixels = channel.getPixels();
            return this;
        }
        // Java: 1186
        public com.oddlabs.procedural.Channel convolution(float[][] filter, float divisor, float offset_v)
        {
            int radius = (filter[0].Length - 1) / 2;
            com.oddlabs.procedural.Channel channel = new com.oddlabs.procedural.Channel(width, height);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float @value = 0;
                    for (int i = -radius; i <= radius; i++)
                    {
                        for (int j = -radius; j <= radius; j++)
                        {
                            @value = (float)(@value + (float)(filter[i + radius][j + radius] * getPixelWrap(x + i, y + j)));
                        }
                    }
                    @value = (float)((float)(@value / divisor) + offset_v);
                    channel.putPixel(x, y, @value);
                }
            }
            pixels = channel.getPixels();
            return this;
        }
        // Java: 1205
        public com.oddlabs.procedural.Channel floodfill(int init_x, int init_y, float @value, float tol, int[] countResult, int[] bounds)
        {
            int min_x = init_x;
            int min_y = init_y;
            int max_x = init_x;
            int max_y = init_y;
            float oldval = getPixel(init_x, init_y);
            bool[][] marked = java.lang.JA.newArray2<bool>(width, height);
            marked[init_x][init_y] = true;
            java.util.List list = new java.util.LinkedList();
            list.add(new int[] { init_x, init_y });
            int count_v = 1;
            while (!list.isEmpty())
            {
                int[] coords = ((int[])list.removeFirst());
                int x = coords[0];
                int y = coords[1];
                putPixel(x, y, @value);
                if (x < min_x)
                {
                    min_x = x;
                }
                if (x > max_x)
                {
                    max_x = x;
                }
                if (y < min_y)
                {
                    min_y = y;
                }
                if (y > max_y)
                {
                    max_y = y;
                }
                if (x > 0 && java.lang.Math.abs((float)(getPixel(x - 1, y) - oldval)) < tol && !marked[x - 1][y])
                {
                    marked[x - 1][y] = true;
                    list.add(new int[] { x - 1, y });
                    count_v++;
                }
                if (x < width - 1 && java.lang.Math.abs((float)(getPixel(x + 1, y) - oldval)) < tol && !marked[x + 1][y])
                {
                    marked[x + 1][y] = true;
                    list.add(new int[] { x + 1, y });
                    count_v++;
                }
                if (y > 0 && java.lang.Math.abs((float)(getPixel(x, y - 1) - oldval)) < tol && !marked[x][y - 1])
                {
                    marked[x][y - 1] = true;
                    list.add(new int[] { x, y - 1 });
                    count_v++;
                }
                if (y < height - 1 && java.lang.Math.abs((float)(getPixel(x, y + 1) - oldval)) < tol && !marked[x][y + 1])
                {
                    marked[x][y + 1] = true;
                    list.add(new int[] { x, y + 1 });
                    count_v++;
                }
            }
            if (bounds != null)
            {
                bounds[0] = min_x;
                bounds[1] = min_y;
                bounds[2] = max_x;
                bounds[3] = max_y;
            }
            if (countResult != null)
            {
                countResult[0] = count_v;
            }
            return this;
        }
        // Java: 1266
        public com.oddlabs.procedural.Channel largestConnected(float @value)
        {
            com.oddlabs.procedural.Channel tmp = this.copy();
            com.oddlabs.procedural.Channel fillmap = new com.oddlabs.procedural.Channel(width, height);
            int[] fillcoords = tmp.findFirst(@value);
            int max_count = 0;
            bool[][] marked = java.lang.JA.newArray2<bool>(width, height);
            while (fillcoords[0] != (-1))
            {
                int count_v = 0;
                int init_x = fillcoords[0];
                int init_y = fillcoords[1];
                fillmap.fill(0.0f);
                marked[init_x][init_y] = true;
                java.util.List list = new java.util.LinkedList();
                list.add(new int[] { init_x, init_y });
                while (!list.isEmpty())
                {
                    int[] coords = ((int[])list.removeFirst());
                    int x = coords[0];
                    int y = coords[1];
                    tmp.putPixel(x, y, (float)(-1.0f));
                    fillmap.putPixel(x, y, 1.0f);
                    count_v++;
                    if (x > 0 && tmp.getPixel(x - 1, y) == 1.0f && !marked[x - 1][y])
                    {
                        marked[x - 1][y] = true;
                        list.add(new int[] { x - 1, y });
                    }
                    if (x < width - 1 && tmp.getPixel(x + 1, y) == 1.0f && !marked[x + 1][y])
                    {
                        marked[x + 1][y] = true;
                        list.add(new int[] { x + 1, y });
                    }
                    if (y > 0 && tmp.getPixel(x, y - 1) == 1.0f && !marked[x][y - 1])
                    {
                        marked[x][y - 1] = true;
                        list.add(new int[] { x, y - 1 });
                    }
                    if (y < height - 1 && tmp.getPixel(x, y + 1) == 1.0f && !marked[x][y + 1])
                    {
                        marked[x][y + 1] = true;
                        list.add(new int[] { x, y + 1 });
                    }
                }
                if (count_v > max_count)
                {
                    pixels = fillmap.copy().pixels;
                    max_count = count_v;
                }
                fillcoords = tmp.findFirst(@value);
            }
            return this;
        }
        // Java: 1326
        public float averageConnected(float @value)
        {
            com.oddlabs.procedural.Channel tmp = this.copy();
            int[] fillcoords = tmp.findFirst(@value);
            int area_count = 0;
            int area_total = 0;
            bool[][] marked = java.lang.JA.newArray2<bool>(width, height);
            while (fillcoords[0] != (-1))
            {
                area_count++;
                int count_v = 0;
                int init_x = fillcoords[0];
                int init_y = fillcoords[1];
                marked[init_x][init_y] = true;
                java.util.List list = new java.util.LinkedList();
                list.add(new int[] { init_x, init_y });
                while (!list.isEmpty())
                {
                    int[] coords = ((int[])list.removeFirst());
                    int x = coords[0];
                    int y = coords[1];
                    tmp.putPixel(x, y, (float)(-1.0f));
                    count_v++;
                    if (x > 0 && tmp.getPixel(x - 1, y) == 1.0f && !marked[x - 1][y])
                    {
                        marked[x - 1][y] = true;
                        list.add(new int[] { x - 1, y });
                    }
                    if (x < width - 1 && tmp.getPixel(x + 1, y) == 1.0f && !marked[x + 1][y])
                    {
                        marked[x + 1][y] = true;
                        list.add(new int[] { x + 1, y });
                    }
                    if (y > 0 && tmp.getPixel(x, y - 1) == 1.0f && !marked[x][y - 1])
                    {
                        marked[x][y - 1] = true;
                        list.add(new int[] { x, y - 1 });
                    }
                    if (y < height - 1 && tmp.getPixel(x, y + 1) == 1.0f && !marked[x][y + 1])
                    {
                        marked[x][y + 1] = true;
                        list.add(new int[] { x, y + 1 });
                    }
                }
                fillcoords = tmp.findFirst(@value);
                area_total += count_v;
            }
            return (area_count == 0 ? (float)(-1.0f) : (float)(((float)(area_total)) / area_count));
        }
        // Java: 1373
        public com.oddlabs.procedural.Channel squareFit(float @value, int size)
        {
            com.oddlabs.procedural.Channel channel = new com.oddlabs.procedural.Channel(width, height);
            bool match = default;
            for (int y = 0; y <= height - size; y++)
            {
                for (int x = 0; x <= width - size; x++)
                {
                    match = true;
                    for (int i = 0; i < size; i++)
                    {
                        for (int j = 0; j < size; j++)
                        {
                            match = match && (getPixel(x + i, y + j) == @value);
                            if (!match)
                            {
                                goto brk_out_1;
                            }
                        }
                    }
                    brk_out_1: ;
                    if (match)
                    {
                        channel.putPixel(x, y, @value);
                    }
                }
            }
            pixels = channel.copy().pixels;
            return this;
        }
        // Java: 1395
        public com.oddlabs.procedural.Channel boxFit(float @value, int width, int height)
        {
            com.oddlabs.procedural.Channel channel = new com.oddlabs.procedural.Channel(this.width, this.height);
            bool match = default;
            for (int y = 0; y <= this.height - height; y++)
            {
                for (int x = 0; x <= this.width - width; x++)
                {
                    match = true;
                    for (int i = 0; i < width; i++)
                    {
                        for (int j = 0; j < height; j++)
                        {
                            match = match && (getPixel(x + i, y + j) == @value);
                            if (!match)
                            {
                                goto brk_out_2;
                            }
                        }
                    }
                    brk_out_2: ;
                    if (match)
                    {
                        channel.putPixel(x, y, @value);
                    }
                }
            }
            pixels = channel.copy().pixels;
            return this;
        }
        // Java: 1417
        public int count(float @value)
        {
            int count_v = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (getPixel(x, y) == @value)
                    {
                        count_v++;
                    }
                }
            }
            return count_v;
        }
        // Java: 1428
        public com.oddlabs.procedural.Channel grow(float @value, int radius)
        {
            com.oddlabs.procedural.Channel channel = this.copy();
            for (int y = radius; y < height - radius; y++)
            {
                for (int x = radius; x < width - radius; x++)
                {
                    if (getPixel(x, y) == @value)
                    {
                        for (int i = -radius; i <= radius; i++)
                        {
                            for (int j = -radius; j <= radius; j++)
                            {
                                channel.putPixel(x + i, y + j, @value);
                            }
                        }
                    }
                }
            }
            pixels = channel.getPixels();
            return this;
        }
        // Java: 1445
        public com.oddlabs.procedural.Channel squareGrow(float @value, int size)
        {
            com.oddlabs.procedural.Channel channel = this.copy();
            for (int y = 0; y <= height - size; y++)
            {
                for (int x = 0; x <= width - size; x++)
                {
                    if (getPixel(x, y) == @value)
                    {
                        for (int i = 0; i < size; i++)
                        {
                            for (int j = 0; j < size; j++)
                            {
                                channel.putPixel(x + i, y + j, @value);
                            }
                        }
                    }
                }
            }
            pixels = channel.getPixels();
            return this;
        }
        // Java: 1462
        public int[] find(int radius, int x_start, int y_start, float @value)
        {
            if (getPixelWrap(x_start, y_start) == @value)
            {
                return new int[] { (x_start + width) % width, (y_start + height) % height };
            }
            int r = 1;
            while (r <= radius)
            {
                int x = x_start - r;
                int x2 = x_start + r;
                for (int i = 0; i < 2 * r - 1; i++)
                {
                    int y_i = y_start - r + 1 + i;
                    if (getPixelWrap(x, y_i) == @value)
                    {
                        return new int[] { (x + width) % width, (y_i + height) % height };
                    }
                    if (getPixelWrap(x2, y_i) == @value)
                    {
                        return new int[] { (x2 + width) % width, (y_i + height) % height };
                    }
                }
                int y = y_start - r;
                int y2 = y_start + r;
                for (int i_2 = 0; i_2 < 2 * r + 1; i_2++)
                {
                    int x_i = x_start - r + i_2;
                    if (getPixelWrap(x_i, y) == @value)
                    {
                        return new int[] { (x_i + width) % width, (y + height) % height };
                    }
                    if (getPixelWrap(x_i, y2) == @value)
                    {
                        return new int[] { (x_i + width) % width, (y2 + height) % height };
                    }
                }
                r++;
            }
            return new int[] { (-1), (-1) };
        }
        // Java: 1497
        public int[] findNoWrap(int radius, int x_start, int y_start, float @value)
        {
            if (getPixel(x_start, y_start) == @value)
            {
                return new int[] { x_start, y_start };
            }
            int r = 1;
            while (r <= radius)
            {
                int x = x_start - r;
                int x2 = x_start + r;
                for (int i = 0; i < 2 * r - 1; i++)
                {
                    int y_i = y_start - r + 1 + i;
                    if (x >= 0 && x < width && y_i >= 0 && y_i < height && getPixel(x, y_i) == @value)
                    {
                        return new int[] { x, y_i };
                    }
                    if (x2 >= 0 && x2 < width && y_i >= 0 && y_i < height && getPixel(x2, y_i) == @value)
                    {
                        return new int[] { x2, y_i };
                    }
                }
                int y = y_start - r;
                int y2 = y_start + r;
                for (int i_2 = 0; i_2 < 2 * r + 1; i_2++)
                {
                    int x_i = x_start - r + i_2;
                    if (x_i >= 0 && x_i < width && y >= 0 && y < height && getPixel(x_i, y) == @value)
                    {
                        return new int[] { x_i, y };
                    }
                    if (x_i >= 0 && x_i < width && y2 >= 0 && y2 < height && getPixel(x_i, y2) == @value)
                    {
                        return new int[] { x_i, y2 };
                    }
                }
                r++;
            }
            return new int[] { (-1), (-1) };
        }
        // Java: 1525
        public int[] findFirst(float @value)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (getPixel(x, y) == @value)
                    {
                        return new int[] { x, y };
                    }
                }
            }
            return new int[] { (-1), (-1) };
        }
        // Java: 1535
        public com.oddlabs.procedural.Channel bump(com.oddlabs.procedural.Channel bumpmap, float lx, float ly, float shadow, float light, float ambient)
        {
            com.oddlabs.procedural.Channel channel = new com.oddlabs.procedural.Channel(width, height);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float nx = (float)(bumpmap.getPixelWrap(x + 1, y) - bumpmap.getPixelWrap(x - 1, y));
                    float ny = (float)(bumpmap.getPixelWrap(x, y + 1) - bumpmap.getPixelWrap(x, y - 1));
                    float brightness_v = (float)((float)(nx * lx) + (float)(ny * ly));
                    if (brightness_v >= 0)
                    {
                        channel.putPixelClip(x, y, (float)(((float)(getPixel(x, y) + (float)(brightness_v * light))) * ((float)((float)((float)(bumpmap.getPixel(x, y) * shadow) + 1) - shadow))));
                    }
                    else
                    {
                        channel.putPixelClip(x, y, (float)(((float)(getPixel(x, y) + (float)(brightness_v * ((float)(1 - ambient))))) * ((float)((float)((float)(bumpmap.getPixel(x, y) * shadow) + 1) - shadow))));
                    }
                }
            }
            pixels = channel.getPixels();
            return this;
        }
        // Java: 1557
        public com.oddlabs.procedural.Channel bumpSpecular(com.oddlabs.procedural.Channel bumpmap, float lx, float ly, float lz, float shadow, float light, int specular)
        {
            float lnorm = ((float)(java.lang.Math.sqrt((float)((float)((float)(lx * lx) + (float)(ly * ly)) + (float)(lz * lz)))));
            float nz = (float)(4 * ((float)(1.0f / java.lang.Math.min(width, height))));
            float nzlz = (float)(nz * lz);
            float nz2 = (float)(nz * nz);
            int power_v = 2 << specular;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float nx = (float)(bumpmap.getPixelWrap(x + 1, y) - bumpmap.getPixelWrap(x - 1, y));
                    float ny = (float)(bumpmap.getPixelWrap(x, y + 1) - bumpmap.getPixelWrap(x, y - 1));
                    float brightness_v = (float)((float)(nx * lx) + (float)(ny * ly));
                    float costheta = (float)(((float)(brightness_v + nzlz)) / ((float)(((float)(java.lang.Math.sqrt((float)((float)((float)(nx * nx) + (float)(ny * ny)) + nz2)))) * lnorm)));
                    float highlight = default;
                    if (costheta > 0)
                    {
                        highlight = ((float)(java.lang.StrictMath.pow(costheta, power_v)));
                    }
                    else
                    {
                        highlight = 0;
                    }
                    putPixelClip(x, y, (float)(((float)(getPixel(x, y) + (float)(highlight * light))) * ((float)((float)((float)(bumpmap.getPixel(x, y) * shadow) + 1) - shadow))));
                }
            }
            return this;
        }
        // Java: 1584
        public com.oddlabs.procedural.Channel lineart()
        {
            com.oddlabs.procedural.Channel channel = new com.oddlabs.procedural.Channel(width, height);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    channel.putPixel(x, y, java.lang.Math.max(java.lang.Math.max(java.lang.Math.abs((float)(getPixelWrap(x, y) - getPixelWrap(x - 1, y))), java.lang.Math.abs((float)(getPixelWrap(x, y) - getPixelWrap(x + 1, y)))), java.lang.Math.max(java.lang.Math.abs((float)(getPixelWrap(x, y) - getPixelWrap(x, y - 1))), java.lang.Math.abs((float)(getPixelWrap(x, y) - getPixelWrap(x, y + 1))))));
                }
            }
            pixels = channel.getPixels();
            return this;
        }
        // Java: 1602
        public com.oddlabs.procedural.Channel relativeIntensity(int radius)
        {
            radius = java.lang.Math.max(1, radius);
            com.oddlabs.procedural.Channel relint = new com.oddlabs.procedural.Channel(width, height);
            float factor = (float)(1.0f / ((2 * radius + 1) * (2 * radius + 1)));
            float sum_v = default;
            float avr = default;
            for (int x = 0; x < width; x++)
            {
                int y = 0;
                sum_v = 0.0f;
                for (int i = -radius; i < radius + 1; i++)
                {
                    for (int j = -radius; j < radius + 1; j++)
                    {
                        sum_v = (float)(sum_v + getPixelWrap(x + j, y + i));
                    }
                }
                for (; y < height; y++)
                {
                    if (y > 0)
                    {
                        for (int j_2 = -radius; j_2 < radius + 1; j_2++)
                        {
                            sum_v = (float)(sum_v - getPixelWrap(x + j_2, y - radius - 1));
                            sum_v = (float)(sum_v + getPixelWrap(x + j_2, y + radius));
                        }
                    }
                    avr = (float)(sum_v * factor);
                    relint.putPixel(x, y, (float)(getPixel(x, y) - avr));
                }
            }
            return relint.add(0.5f);
        }
        // Java: 1630
        public com.oddlabs.procedural.Channel relativeIntensityNormalized(int radius)
        {
            return this.relativeIntensity(radius).dynamicRangeSymmetric();
        }
        // Java: 1634
        public com.oddlabs.procedural.Channel[] fft()
        {
            int size = width;
            float[] a = new float[size * size * 2 + 1];
            int n = 1;
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    a[n] = getPixel(x, y);
                    n += 2;
                }
            }
            fastFourierTransform(a, size, 1);
            n = 1;
            com.oddlabs.procedural.Channel magnitude = new com.oddlabs.procedural.Channel(size, size);
            com.oddlabs.procedural.Channel phase = new com.oddlabs.procedural.Channel(size, size);
            float real = default;
            float imag = default;
            for (int x_2 = 0; x_2 < size; x_2++)
            {
                for (int y_2 = 0; y_2 < size; y_2++)
                {
                    real = a[n++];
                    imag = a[n++];
                    magnitude.putPixel(x_2, y_2, ((float)(java.lang.Math.sqrt((float)((float)(real * real) + (float)(imag * imag))))));
                    if (imag == 0 && real >= 0)
                    {
                        phase.putPixel(x_2, y_2, (float)(((float)(java.lang.Math.PI)) / 2.0f));
                    }
                    else if (imag == 0 && real < 0)
                    {
                        phase.putPixel(x_2, y_2, (float)(((float)(java.lang.Math.PI)) / (float)(-2.0f)));
                    }
                    else
                    {
                        phase.putPixel(x_2, y_2, ((float)(java.lang.StrictMath.atan((float)(real / imag)))));
                    }
                }
            }
            return new com.oddlabs.procedural.Channel[] { magnitude.offset(size >> 1, size >> 1), phase };
        }
        // Java: 1676
        public com.oddlabs.procedural.Channel fftInv(com.oddlabs.procedural.Channel magni, com.oddlabs.procedural.Channel phase)
        {
            int size = magni.width;
            com.oddlabs.procedural.Channel magnitude = magni.copy().offset(size >> 1, size >> 1);
            float mag = default;
            float pha = default;
            float[] a = new float[size * size * 2 + 1];
            int n = 1;
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    mag = magnitude.getPixel(x, y);
                    pha = phase.getPixel(x, y);
                    a[n++] = (float)(mag * ((float)(java.lang.StrictMath.cos(pha))));
                    a[n++] = (float)(mag * ((float)(java.lang.StrictMath.sin(pha))));
                }
            }
            fastFourierTransform(a, size, (-1));
            n = 1;
            for (int x_2 = 0; x_2 < size; x_2++)
            {
                for (int y_2 = 0; y_2 < size; y_2++)
                {
                    putPixel(x_2, y_2, a[n]);
                    n += 2;
                }
            }
            return this;
        }
        // Java: 1712
        public void fastFourierTransform(float[] data, int size, int isign)
        {
            int i1 = default;
            int i2 = default;
            int i3 = default;
            int i2rev = default;
            int i3rev = default;
            int ip1 = default;
            int ip2 = default;
            int ip3 = default;
            int ifp1 = default;
            int ifp2 = default;
            int ibit = default;
            int idim = default;
            int k1 = default;
            int k2 = default;
            int n = default;
            int nprev = default;
            int nrem = default;
            int ntot = default;
            float tempi = default;
            float tempr = default;
            float theta = default;
            float wi = default;
            float wpi = default;
            float wpr = default;
            float wr = default;
            float wtemp = default;
            ntot = size * size;
            nprev = 1;
            for (idim = 2; idim >= 1; idim--)
            {
                n = size;
                nrem = ntot / (n * nprev);
                ip1 = nprev << 1;
                ip2 = ip1 * n;
                ip3 = ip2 * nrem;
                i2rev = 1;
                for (i2 = 1; i2 <= ip2; i2 += ip1)
                {
                    if (i2 < i2rev)
                    {
                        for (i1 = i2; i1 <= i2 + ip1 - 2; i1 += 2)
                        {
                            for (i3 = i1; i3 <= ip3; i3 += ip2)
                            {
                                i3rev = i2rev + i3 - i2;
                                tempr = data[i3];
                                data[i3] = (data[i3rev]);
                                data[i3rev] = tempr;
                                tempr = data[i3 + 1];
                                data[i3 + 1] = data[i3rev + 1];
                                data[i3rev + 1] = tempr;
                            }
                        }
                    }
                    ibit = ip2 >> 1;
                    while (ibit >= ip1 && i2rev > ibit)
                    {
                        i2rev -= ibit;
                        ibit >>= 1;
                    }
                    i2rev += ibit;
                }
                ifp1 = ip1;
                while (ifp1 < ip2)
                {
                    ifp2 = ifp1 << 1;
                    theta = (float)((float)(isign * ((float)(((float)(java.lang.Math.PI)) * 2))) / ((float)(ifp2 / ((float)(ip1)))));
                    wtemp = ((float)(java.lang.StrictMath.sin(0.5d * theta)));
                    wpr = (float)((float)((float)(-2.0f) * wtemp) * wtemp);
                    wpi = ((float)(java.lang.StrictMath.sin(theta)));
                    wr = 1.0f;
                    wi = 0.0f;
                    for (i3 = 1; i3 <= ifp1; i3 += ip1)
                    {
                        for (i1 = i3; i1 <= i3 + ip1 - 2; i1 += 2)
                        {
                            for (i2 = i1; i2 <= ip3; i2 += ifp2)
                            {
                                k1 = i2;
                                k2 = k1 + ifp1;
                                tempr = (float)((float)(wr * data[k2]) - (float)(wi * data[k2 + 1]));
                                tempi = (float)((float)(wr * data[k2 + 1]) + (float)(wi * data[k2]));
                                data[k2] = (float)(data[k1] - tempr);
                                data[k2 + 1] = (float)(data[k1 + 1] - tempi);
                                data[k1] += tempr;
                                data[k1 + 1] += tempi;
                            }
                        }
                        wr = (float)((float)((float)((wtemp = wr) * wpr) - (float)(wi * wpi)) + wr);
                        wi = (float)((float)((float)(wi * wpr) + (float)(wtemp * wpi)) + wi);
                    }
                    ifp1 = ifp2;
                }
                nprev *= n;
            }
        }
        // Java: 1779
        public com.oddlabs.procedural.Layer toNormalMap(float strength, com.oddlabs.procedural.Channel specularMap)
        {
            com.oddlabs.procedural.Layer layer = toNormalMap(strength);
            layer.addAlpha(specularMap);
            return layer;
        }
        // Java: 1785
        public com.oddlabs.procedural.Layer toNormalMap(float strength)
        {
            com.oddlabs.procedural.Channel r = new com.oddlabs.procedural.Channel(width, height);
            com.oddlabs.procedural.Channel g = new com.oddlabs.procedural.Channel(width, height);
            com.oddlabs.procedural.Channel b = new com.oddlabs.procedural.Channel(width, height);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float tl = getPixelWrap(x - 1, y - 1);
                    float t = getPixelWrap(x, y - 1);
                    float tr = getPixelWrap(x + 1, y - 1);
                    float l = getPixelWrap(x - 1, y);
                    float ri = getPixelWrap(x + 1, y);
                    float bl = getPixelWrap(x - 1, y + 1);
                    float bo = getPixelWrap(x, y + 1);
                    float br = getPixelWrap(x + 1, y + 1);
                    float dX = (float)(((float)((float)(tr + (float)(2.0f * ri)) + br)) - ((float)((float)(tl + (float)(2.0f * l)) + bl)));
                    float dY = (float)(((float)((float)(bl + (float)(2.0f * bo)) + br)) - ((float)((float)(tl + (float)(2.0f * t)) + tr)));
                    float nx = (float)((float)(-dX) * strength);
                    float ny = (float)((float)(-dY) * strength);
                    float nz = 1.0f;
                    float len = ((float)(java.lang.Math.sqrt((float)((float)((float)(nx * nx) + (float)(ny * ny)) + (float)(nz * nz)))));
                    nx = (float)(nx / len);
                    ny = (float)(ny / len);
                    nz = (float)(nz / len);
                    r.putPixel(x, y, (float)(((float)(nx + 1.0f)) * 0.5f));
                    g.putPixel(x, y, (float)(((float)(ny + 1.0f)) * 0.5f));
                    b.putPixel(x, y, (float)(((float)(nz + 1.0f)) * 0.5f));
                }
            }
            return new com.oddlabs.procedural.Layer(r, g, b);
        }
        // Java: 1821
        public com.oddlabs.procedural.Channel erode(float talus, int iterations)
        {
            com.oddlabs.procedural.ChannelErodeFast.Erode(pixels, width, height, talus, iterations, false);
            return this;
        }
        // Java: 1826
        public com.oddlabs.procedural.Channel erodeThermal(float talus, int iterations)
        {
            com.oddlabs.procedural.ChannelErodeFast.Erode(pixels, width, height, talus, iterations, true);
            return this;
        }
        // Java: 1831
        public com.oddlabs.procedural.Channel place(com.oddlabs.procedural.Channel sprite, int x_offset, int y_offset)
        {
            for (int y = y_offset; y < y_offset + sprite.getHeight(); y++)
            {
                for (int x = x_offset; x < x_offset + sprite.getWidth(); x++)
                {
                    putPixelWrap(x, y, sprite.getPixelWrap(x - x_offset, y - y_offset));
                }
            }
            return this;
        }
        // Java: 1840
        public com.oddlabs.procedural.Channel place(com.oddlabs.procedural.Channel sprite, com.oddlabs.procedural.Channel alpha, int x_offset, int y_offset)
        {
            float alpha_val = default;
            for (int y = y_offset; y < y_offset + sprite.getHeight(); y++)
            {
                for (int x = x_offset; x < x_offset + sprite.getWidth(); x++)
                {
                    alpha_val = alpha.getPixel(x - x_offset, y - y_offset);
                    putPixelWrap(x, y, (float)((float)(alpha_val * sprite.getPixelWrap(x - x_offset, y - y_offset)) + (float)(((float)(1 - alpha_val)) * getPixelWrap(x, y))));
                }
            }
            return this;
        }
        // Java: 1852
        public com.oddlabs.procedural.Channel placeBrightest(com.oddlabs.procedural.Channel sprite, int x_offset, int y_offset)
        {
            for (int y = y_offset; y < y_offset + sprite.getHeight(); y++)
            {
                for (int x = x_offset; x < x_offset + sprite.getWidth(); x++)
                {
                    putPixelWrap(x, y, java.lang.Math.max(getPixelWrap(x, y), sprite.getPixelWrap(x - x_offset, y - y_offset)));
                }
            }
            return this;
        }
        // Java: 1861
        public com.oddlabs.procedural.Channel placeDarkest(com.oddlabs.procedural.Channel sprite, int x_offset, int y_offset)
        {
            for (int y = y_offset; y < y_offset + sprite.getHeight(); y++)
            {
                for (int x = x_offset; x < x_offset + sprite.getWidth(); x++)
                {
                    putPixelWrap(x, y, java.lang.Math.min(getPixelWrap(x, y), sprite.getPixelWrap(x - x_offset, y - y_offset)));
                }
            }
            return this;
        }
        // Java: 1870
        public com.oddlabs.procedural.Channel abs()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    putPixel(x, y, (float)(2.0f * java.lang.Math.abs((float)(getPixel(x, y) - 0.5f))));
                }
            }
            return this;
        }
        // Java: 1880
        public com.oddlabs.procedural.Channel channelBlend(com.oddlabs.procedural.Channel channel, float alpha)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    putPixel(x, y, (float)((float)(alpha * channel.getPixel(x, y)) + (float)(((float)(1 - alpha)) * getPixel(x, y))));
                }
            }
            return this;
        }
        // Java: 1889
        public com.oddlabs.procedural.Channel channelBlend(com.oddlabs.procedural.Channel channel, com.oddlabs.procedural.Channel alpha)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float alpha_val = alpha.getPixel(x, y);
                    putPixel(x, y, (float)((float)(alpha_val * channel.getPixel(x, y)) + (float)(((float)(1 - alpha_val)) * getPixel(x, y))));
                }
            }
            return this;
        }
        // Java: 1899
        public com.oddlabs.procedural.Channel channelAdd(com.oddlabs.procedural.Channel channel)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    putPixelClip(x, y, (float)(getPixel(x, y) + channel.getPixel(x, y)));
                }
            }
            return this;
        }
        // Java: 1908
        public com.oddlabs.procedural.Channel channelAddNoClip(com.oddlabs.procedural.Channel channel)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    putPixel(x, y, (float)(getPixel(x, y) + channel.getPixel(x, y)));
                }
            }
            return this;
        }
        // Java: 1917
        public com.oddlabs.procedural.Channel channelSubtract(com.oddlabs.procedural.Channel channel)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    putPixelClip(x, y, (float)(getPixel(x, y) - channel.getPixel(x, y)));
                }
            }
            return this;
        }
        // Java: 1926
        public com.oddlabs.procedural.Channel channelSubtractNoClip(com.oddlabs.procedural.Channel channel)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    putPixel(x, y, (float)(getPixel(x, y) - channel.getPixel(x, y)));
                }
            }
            return this;
        }
        // Java: 1935
        public com.oddlabs.procedural.Channel channelAverage(com.oddlabs.procedural.Channel channel)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    putPixel(x, y, (float)(((float)(getPixel(x, y) + channel.getPixel(x, y))) / 2.0f));
                }
            }
            return this;
        }
        // Java: 1944
        public com.oddlabs.procedural.Channel channelMultiply(com.oddlabs.procedural.Channel channel)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    putPixel(x, y, (float)(getPixel(x, y) * channel.getPixel(x, y)));
                }
            }
            return this;
        }
        // Java: 1953
        public com.oddlabs.procedural.Channel channelDivide(com.oddlabs.procedural.Channel channel)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    putPixel(x, y, (float)(getPixel(x, y) / channel.getPixel(x, y)));
                }
            }
            return this;
        }
        // Java: 1962
        public com.oddlabs.procedural.Channel channelDifference(com.oddlabs.procedural.Channel channel)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    putPixel(x, y, java.lang.Math.abs((float)(getPixel(x, y) - channel.getPixel(x, y))));
                }
            }
            return this;
        }
        // Java: 1971
        public com.oddlabs.procedural.Channel channelDarkest(com.oddlabs.procedural.Channel channel)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    putPixel(x, y, java.lang.Math.min(getPixel(x, y), channel.getPixel(x, y)));
                }
            }
            return this;
        }
        // Java: 1980
        public com.oddlabs.procedural.Channel channelBrightest(com.oddlabs.procedural.Channel channel)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    putPixel(x, y, java.lang.Math.max(getPixel(x, y), channel.getPixel(x, y)));
                }
            }
            return this;
        }
        public Channel(int width, int height) : base()
        {
            if (width <= 0 || height <= 0)
            {
                throw new System.ArgumentException("Width and height must be positive.");
            }
            pixels = java.lang.JA.newArray2<float>(height, width);
            this.width = width;
            this.height = height;
            this.powerof2 = com.oddlabs.util.Utils.isPowerOf2(width) && com.oddlabs.util.Utils.isPowerOf2(height);
        }
    }
}
