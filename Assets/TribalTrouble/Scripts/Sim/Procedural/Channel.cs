// Ported from Java (Tribal Trouble 1.4.1). Mechanically transliterated by PortTools/j2cs.py,
// then reviewed by hand. Java identifiers are kept so this file diffs against its source.
using System;
using System.Collections.Generic;
using TribalTrouble.Sim.Determinism;
using TribalTrouble.Sim;

namespace TribalTrouble.Sim.Procedural
{
    /**
     * Represents a 2D grid of floating-point values, typically used to store a single color channel
     * (e.g., red, green, blue, or alpha) or other procedural generation data like heightmaps.
     */
    public sealed class Channel { //added by ikill240c
        // Was never needed before CustomMapGenerator started holding an AuthoredTerrain (which holds
        // a Channel) as a field on a class that implements WorldGenerator extends Serializable - the
        // whole generator gets shipped across the network to joining players (even in singleplayer,
        // which still routes through loopback networking), so every field it transitively holds must
        // be Serializable too. All of Channel's own fields (a float[][], two ints, a bool) are
        // already trivially serializable once the class itself opts in - no custom
        // readObject/writeObject needed. //added by ikill240c
        //added by ikill240c

        private float[][] pixels;
        public int width;
        public int height;
        private readonly bool powerof2;

        public class ChannelVisitor {
            private int x = 0;
            private int y = 0;
            private bool done = false;
            private float value;
            private Channel channel;

            internal ChannelVisitor(Channel channel, float value) {
                this.channel = channel;
                this.value = value;
            }

            public int[] visitNext() {
                while (!done) {
                    if (channel.getPixel(x, y) == value) {
                        break;
                    }
                    x++;
                    if (x >= channel.width) {
                        x = 0;
                        y++;
                        if (y >= channel.height) {
                            done = true;
                            x = -1;
                            y = -1;
                        }
                    }
                }
                return new int[]{x, y};
            }
        }

        /**
         * Constructs a new Channel with the specified dimensions.
         *
         * @param width  the width of the channel in pixels.
         * @param height the height of the channel in pixels.
         * @throws ArgumentException if width or height are not positive.
         */
        public Channel(int width, int height) {
            if (width <= 0 || height <= 0) {
                throw new ArgumentException("Width and height must be positive.");
            }
            pixels = J.New2<float>(height, width);
            this.width = width;
            this.height = height;
            this.powerof2 = Utils.isPowerOf2(width) && Utils.isPowerOf2(height);
        }

        public ChannelVisitor visitor(float value) {
            return new ChannelVisitor(this, value);
        }

        /**
         * Converts this single channel into a 3-channel (grayscale) Layer.
         *
         * @return a new Layer with this channel copied to its R, G, and B channels.
         */
        public Layer toLayer() {
            return new Layer(this.copy(), this.copy(), this.copy());
        }

        /**
         * Calculates a CRC32 checksum of the channel's pixel data.
         *
         * @return the CRC32 checksum value.
         */
        public long getChecksum() {
            // CRC32 over each pixel's big-endian float bits, like ByteBuffer.putFloat + java.util.zip.CRC32
            var crc = new JCrc32();
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    crc.updateInt(J.FloatToIntBits(getPixel(x, y)));
                }
            }
            return crc.getValue();
        }

        /**
         * Returns the width of the channel.
         *
         * @return the width in pixels.
         */
        public int getWidth() {
            return width;
        }

        /**
         * Returns the height of the channel.
         *
         * @return the height in pixels.
         */
        public int getHeight() {
            return height;
        }

        /**
         * Sets the value of a pixel at the specified coordinates.
         *
         * @param x     the x-coordinate of the pixel.
         * @param y     the y-coordinate of the pixel.
         * @param value the new value for the pixel.
         * @throws IndexOutOfRangeException if the coordinates are out of bounds.
         */
        public void putPixel(int x, int y, float value) {
            J.CheckIndex(x, width);
            J.CheckIndex(y, height);
            pixels[y][x] = value;
        }

        /**
         * Gets the value of a pixel at the specified coordinates.
         *
         * @param x the x-coordinate of the pixel.
         * @param y the y-coordinate of the pixel.
         * @return the value of the pixel.
         * @throws IndexOutOfRangeException if the coordinates are out of bounds.
         */
        public float getPixel(int x, int y) {
            J.CheckIndex(x, width);
            J.CheckIndex(y, height);
            return pixels[y][x];
        }

        /**
         * Returns the raw 2D float array backing this channel.
         *
         * @return the pixel data.
         */
        public float[][] getPixels() {
            return pixels;
        }

        /**
         * Sets the value of a pixel with wrapping coordinates.
         * If coordinates are out of bounds, they are wrapped around to the other side.
         *
         * @param x     the x-coordinate.
         * @param y     the y-coordinate.
         * @param value the new value.
         */
        public void putPixelWrap(int x, int y, float value) {
            if (this.powerof2) {
                if (x < 0 || x >= width) x = (width + x) & (width - 1);
                if (y < 0 || y >= height) y = (height + y) & (height - 1);
                pixels[y][x] = value;
            } else {
                if (x < 0 || x >= width || y < 0 || y >= height) {
                    pixels[(y + height) % height][(x + width) % width] = value;
                } else {
                    pixels[y][x] = value;
                }
            }
        }

        /**
         * Gets the value of a pixel with wrapping coordinates.
         * If coordinates are out of bounds, they are wrapped around to the other side.
         *
         * @param x the x-coordinate.
         * @param y the y-coordinate.
         * @return the pixel value.
         */
        public float getPixelWrap(int x, int y) {
            if (this.powerof2) {
                if (x < 0 || x >= width) x = (width + x) & (width - 1);
                if (y < 0 || y >= height) y = (height + y) & (height - 1);
                return pixels[y][x];
            } else {
                if (x < 0 || x >= width || y < 0 || y >= height) {
                    return pixels[(y + height) % height][(x + width) % width];
                } else {
                    return pixels[y][x];
                }
            }
        }

        /**
         * Gets the value of a pixel safely. If coordinates are out of bounds, returns 0.
         *
         * @param x the x-coordinate.
         * @param y the y-coordinate.
         * @return the pixel value or 0 if out of bounds.
         */
        public float getPixelSafe(int x, int y) {
            if (x < 0 || x >= width || y < 0 || y >= height) {
                return 0f;
            } else {
                return pixels[y][x];
            }
        }

        /**
         * Sets the value of a pixel safely. If coordinates are out of bounds, the operation is ignored.
         *
         * @param x     the x-coordinate.
         * @param y     the y-coordinate.
         * @param value the new value.
         */
        public void putPixelSafe(int x, int y, float value) {
            if (x >= 0 && x < width && y >= 0 && y < height) {
                pixels[y][x] = value;
            }
        }

        /**
         * Sets the value of a pixel, clamping it to the [0, 1] range.
         *
         * @param x     the x-coordinate.
         * @param y     the y-coordinate.
         * @param value the new value.
         */
        public void putPixelClip(int x, int y, float value) {
            if (value < 0) pixels[y][x] = 0;
            else if (value > 1) pixels[y][x] = 1;
            else pixels[y][x] = value;
        }

        /**
         * Fills the entire channel with a single value.
         *
         * @param value the value to fill with.
         * @return this channel, for chaining.
         */
        public Channel fill(float value) {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    pixels[y][x] = value;
                }
            }
            return this;
        }

        public Channel fill(float value, float min, float max) {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    if (pixels[y][x] >= min && pixels[y][x] <= max) {
                        pixels[y][x] = value;
                    }
                }
            }
            return this;
        }

        public float findMin() {
            float min = float.MaxValue;
            float val = 0;
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    val = pixels[y][x];
                    if (val < min) min = val;
                }
            }
            return min;
        }

        public float findMax() {
            float max = float.Epsilon;
            float val = 0;
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    val = pixels[y][x];
                    if (val > max) max = val;
                }
            }
            return max;
        }

        public float[] findMinMax() {
            float min = float.MaxValue;
            float max = float.Epsilon;
            float val1 = 0;
            float val2 = 0;
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x += 2) {
                    val1 = pixels[y][x];
                    val2 = pixels[y][x + 1];
                    if (val1 <= val2) {
                        if (val1 < min) min = val1;
                        if (val2 > max) max = val2;
                    } else {
                        if (val2 < min) min = val2;
                        if (val1 > max) max = val1;
                    }
                }
            }
            return new float[]{min, max};
        }

        public float sum() {
            float sum = 0f;
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    sum = (float)(sum + (pixels[y][x]));
                }
            }
            return sum;
        }

        public Channel copy() {
            Channel channel = new Channel(width, height);
            float[][] new_pixels = channel.getPixels();
            for (int y = 0; y < height; y++) {
                Array.Copy(pixels[y], 0, new_pixels[y], 0, width);
            }
            return channel;
        }

        public Channel dynamicRange() {
            float[] minmax = findMinMax();
            float min = minmax[0];
            float max = minmax[1];
            float factor = (float)(1f / ((float)(max - min)));
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    putPixel(x, y, (float)(factor * ((float)(pixels[y][x] - min))));
                }
            }
            return this;
        }

        public Channel dynamicRange(float new_min, float new_max) {
            float min = findMin();
            float max = findMax();
            float inv_maxmin = (float)(1f / ((float)(max - min)));
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    putPixel(x, y, Tools.interpolateLinear(new_min, new_max, (float)(((float)(pixels[y][x] - min)) * inv_maxmin)));
                }
            }
            return this;
        }

        public Channel dynamicRange(float min, float max, float new_min, float new_max) {
            float val;
            float inv_maxmin = (float)(1f / ((float)(max - min)));
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    val = getPixel(x, y);
                    if (val >= min && val <= max) {
                        putPixel(x, y, Tools.interpolateLinear(new_min, new_max, (float)(((float)(pixels[y][x] - min)) * inv_maxmin)));
                    } else {
                        if (val < min) {
                            putPixel(x, y, new_min);
                        } else {
                            putPixel(x, y, new_max);
                        }
                    }
                }
            }
            return this;
        }

        public Channel dynamicRangeSymmetric() {
            float[] minmax = findMinMax();
            float min = minmax[0];
            float max = minmax[1];
            if (min > ((float)(1 - max))) {
                min = (float)(1f - max);
            }
            if (max < ((float)(1 - min))) {
                max = (float)(1f - min);
            }
            float factor = (float)(1f / ((float)(max - min)));
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    putPixel(x, y, (float)(factor * ((float)(pixels[y][x] - min))));
                }
            }
            return this;
        }

        public Channel clip() {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    putPixelClip(x, y, getPixel(x, y));
                }
            }
            return this;
        }

        public Channel crop(int x_lo, int y_lo, int x_hi, int y_hi) {
            int new_width = x_hi - x_lo + 1;
            int new_height = y_hi - y_lo + 1;
            Channel channel = new Channel(new_width, new_height);
            float[][] new_pixels = channel.getPixels();
            for (int y = y_lo; y <= y_hi; y++) {
                Array.Copy(pixels[y], x_lo, new_pixels[y - y_lo], 0, new_width);
            }
            return channel;
        }

        public Channel cropWrap(int x_lo, int y_lo, int x_hi, int y_hi) {
            int new_width = x_hi - x_lo + 1;
            int new_height = y_hi - y_lo + 1;
            Channel channel = new Channel(new_width, new_height);
            for (int y = 0; y < new_height; y++) {
                int y_old = y + y_lo;
                for (int x = 0; x < new_width; x++) {
                    int x_old = x + x_lo;
                    if (x_old < 0 || x_old >= width || y_old < 0 || y_old >= height) {
                        channel.putPixel(x, y, getPixelWrap(x_old, y_old));
                    } else {
                        channel.putPixel(x, y, getPixel(x_old, y_old));
                    }
                }
            }
            return channel;
        }

        public Channel tile(int new_width, int new_height) {
            Channel channel = new Channel(new_width, new_height);
            for (int y = 0; y < new_height; y++) {
                for (int x = 0; x < new_width; x++) {
                    if (x < width && y < height) {
                        channel.putPixel(x, y, getPixel(x, y));
                    } else {
                        channel.putPixel(x, y, getPixelWrap(x, y));
                    }
                }
            }
            pixels = channel.getPixels();
            width = new_width;
            height = new_height;
            return this;
        }

        public Channel tileDouble() {
            Channel channel = new Channel(width << 1, height << 1);
            float[][] new_pixels = channel.getPixels();
            for (int y = 0; y < height; y++) {
                Array.Copy(pixels[y], 0, new_pixels[y], 0, width);
                Array.Copy(pixels[y], 0, new_pixels[y], width, width);
                Array.Copy(pixels[y], 0, new_pixels[y + height], 0, width);
                Array.Copy(pixels[y], 0, new_pixels[y + height], width, width);
            }
            pixels = channel.getPixels();
            width = width << 1;
            height = height << 1;
            return this;
        }

        public Channel[] quadSplit() {
            System.Diagnostics.Debug.Assert(Utils.isPowerOf2(width) && Utils.isPowerOf2(height));
            Channel channel1 = this.copy().crop(0, 0, (width >> 1) - 1, (height >> 1) - 1);
            Channel channel2 = this.copy().crop(width >> 1, 0, width - 1, (height >> 1) - 1);
            Channel channel3 = this.copy().crop(0, height >> 1, (width >> 1) - 1, height - 1);
            Channel channel4 = this.copy().crop(width >> 1, height >> 1, width - 1, height - 1);
            return new Channel[]{channel1, channel2, channel3, channel4};
        }

        public Channel quadJoin(Channel channel1, Channel channel2, Channel channel3,
                Channel channel4) {
            System.Diagnostics.Debug.Assert(channel1.width == channel2.width && channel2.width == channel3.width && channel3.width == channel4.width
                    && channel1.height == channel2.height && channel2.height == channel3.height
                    && channel3.height == channel4.height);
            System.Diagnostics.Debug.Assert(width == channel1.width << 1 && height == channel1.height << 1);
            Channel channel = new Channel(channel1.width << 1, channel1.height << 1);
            channel.place(channel1, 0, 0);
            channel.place(channel2, channel1.width, 0);
            channel.place(channel3, 0, channel1.height);
            channel.place(channel4, channel1.width, channel1.height);
            pixels = channel.getPixels();
            return this;
        }

        public Channel offset(int x_offset, int y_offset) {
            Channel channel = new Channel(width, height);
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    channel.putPixel(x, y, getPixelWrap(x - x_offset, y - y_offset));
                }
            }
            pixels = channel.getPixels();
            return this;
        }

        public Channel brightness(float brightness) {
            if (brightness > 1f) {
                for (int y = 0; y < height; y++) {
                    for (int x = 0; x < width; x++) {
                        putPixelClip(x, y, (float)(brightness * getPixel(x, y)));
                    }
                }
            } else {
                for (int y = 0; y < height; y++) {
                    for (int x = 0; x < width; x++) {
                        putPixel(x, y, (float)(brightness * getPixel(x, y)));
                    }
                }
            }
            return this;
        }

        public Channel multiply(float factor) {
            if (factor == 1)
                return this;
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    putPixel(x, y, (float)(factor * getPixel(x, y)));
                }
            }
            return this;
        }

        public Channel power(float exponent) {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    putPixel(x, y, (float) J.Pow(getPixel(x, y), exponent));
                }
            }
            return this;
        }

        public Channel power2() {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    float val = getPixel(x, y);
                    putPixel(x, y, (float)(val * val));
                }
            }
            return this;
        }

        public Channel log() {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    putPixel(x, y, (float) J.Log(getPixel(x, y)));
                }
            }
            return this;
        }

        public Channel add(float add) {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    putPixel(x, y, (float)(getPixel(x, y) + add));
                }
            }
            return this;
        }

        public Channel addClip(float add) {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    putPixelClip(x, y, (float)(getPixel(x, y) + add));
                }
            }
            return this;
        }

        public Channel contrast(float contrast) {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    putPixelClip(x, y, (float)(((float)(((float)(getPixel(x, y) - 0.5f)) * contrast)) + 0.5f));
                }
            }
            return this;
        }

        public Channel gamma(float gamma) {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    putPixel(x, y, (float) J.Pow(getPixel(x, y), (float)(1 / gamma)));
                }
            }
            return this;
        }

        public Channel gamma2() {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    float val = (float)(1f - getPixel(x, y));
                    putPixel(x, y, (float)(1 - (float)(val * val)));
                }
            }
            return this;
        }

        public Channel gamma4() {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    float val = (float)(1f - getPixel(x, y));
                    val = (float)(val * val);
                    putPixel(x, y, (float)(1 - (float)(val * val)));
                }
            }
            return this;
        }

        public Channel gamma8() {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    float val = (float)(1f - getPixel(x, y));
                    val = (float)(val * val);
                    val = (float)(val * val);
                    putPixel(x, y, (float)(1 - (float)(val * val)));
                }
            }
            return this;
        }

        public Channel gain(float gain) {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    if (getPixel(x, y) < 0.5f)
                        putPixel(x, y, (float) (J.Pow((float)(2 * getPixel(x, y)), J.Log((float)(1 - gain)) / J.Log(0.5d)) / 2f));
                    else
                        putPixel(x, y, (float)(1f - (float) (J.Pow((float)(2 - (float)(2 * getPixel(x, y))), J.Log((float)(1 - gain)) / J.Log(
                                0.5d)) / 2f)));
                }
            }
            return this;
        }

        public Channel smoothGain() {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    putPixel(x, y, Tools.interpolateSmooth(0f, 1f, getPixel(x, y)));
                }
            }
            return this;
        }

        public Channel invert() {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    putPixel(x, y, (float)(1f - getPixel(x, y)));
                }
            }
            return this;
        }

        public Channel threshold(float start, float end) {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    float val = getPixel(x, y);
                    if (val >= start && val <= end) {
                        putPixel(x, y, 1f);
                    } else {
                        putPixel(x, y, 0f);
                    }
                }
            }
            return this;
        }

        public Channel scale(int new_width, int new_height) {
            return this.scaleLinear(new_width, new_height);
        }

        public Channel scaleHalf() {
            int new_width = width / 2;
            int new_height = height / 2;
            Channel channel = new Channel(new_width, new_height);
            for (int y = 0; y < new_height; y++) {
                int y_coord_lo = y * 2;
                int y_coord_hi = y_coord_lo + 1;
                for (int x = 0; x < new_width; x++) {
                    int x_coord_lo = x * 2;
                    int x_coord_hi = x_coord_lo + 1;
                    float val1 = Tools.interpolateLinear(getPixelWrap(x_coord_lo, y_coord_lo),
                            getPixelWrap(x_coord_hi, y_coord_lo),
                            .5f);
                    float val2 = Tools.interpolateLinear(getPixelWrap(x_coord_lo, y_coord_hi),
                            getPixelWrap(x_coord_hi, y_coord_hi),
                            .5f);
                    channel.putPixel(x, y, Tools.interpolateLinear(val1, val2, .5f));
                }
            }
            pixels = channel.getPixels();
            width = new_width;
            height = new_height;
            return this;
        }

        public Channel scaleLinear(int new_width, int new_height) {
            if (width == new_width && height == new_height) {
                return this;
            }
            Channel channel = new Channel(new_width, new_height);
            float x_coord = 0;
            float y_coord = 0;
            float val1 = 0;
            float val2 = 0;
            float height_ratio = (float)((float) height / new_height);
            float width_ratio = (float)((float) width / new_width);
            for (int y = 0; y < new_height; y++) {
                y_coord = (float)((float)(y * height_ratio) - 0.5f);
                int y_coord_lo = J.I(y_coord);
                int y_coord_hi = y_coord_lo + 1;
                for (int x = 0; x < new_width; x++) {
                    x_coord = (float)((float)(x * width_ratio) - 0.5f);
                    int x_coord_lo = J.I(x_coord);
                    int x_coord_hi = x_coord_lo + 1;
                    float x_diff = (float)(x_coord - x_coord_lo);
                    val1 = Tools.interpolateLinear(getPixelWrap(x_coord_lo, y_coord_lo),
                            getPixelWrap(x_coord_hi, y_coord_lo),
                            x_diff);
                    val2 = Tools.interpolateLinear(getPixelWrap(x_coord_lo, y_coord_hi),
                            getPixelWrap(x_coord_hi, y_coord_hi),
                            x_diff);
                    channel.putPixel(x, y, Tools.interpolateLinear(val1, val2, (float)(y_coord - y_coord_lo)));
                }
            }
            pixels = channel.getPixels();
            width = new_width;
            height = new_height;
            return this;
        }

        public Channel scaleCubicWrapping(int new_width, int new_height) {
            if (width == new_width && height == new_height) {
                return this;
            }
            Channel channel = new Channel(new_width, new_height);
            float x_coord, y_coord;
            float val0, val1, val2, val3;
            float height_ratio = (float)((float) height / new_height);
            float width_ratio = (float)((float) width / new_width);
            float x_diff, y_diff;
            int x_coord_lo, x_coord_lolo, x_coord_hi, x_coord_hihi, y_coord_lo, y_coord_lolo, y_coord_hi, y_coord_hihi;
            for (int y = 0; y < new_height; y++) {
                y_coord = (float)((float)(y * height_ratio) - 0.5f);
                y_coord_lo = J.I(y_coord);
                y_coord_lolo = y_coord_lo - 1;
                y_coord_hi = y_coord_lo + 1;
                y_coord_hihi = y_coord_hi + 1;
                y_diff = (float)(y_coord - y_coord_lo);
                for (int x = 0; x < new_width; x++) {
                    x_coord = (float)((float)(x * width_ratio) - 0.5f);
                    x_coord_lo = J.I(x_coord);
                    x_coord_lolo = x_coord_lo - 1;
                    x_coord_hi = x_coord_lo + 1;
                    x_coord_hihi = x_coord_hi + 1;
                    x_diff = (float)(x_coord - x_coord_lo);
                    val0 = Tools.interpolateCubic(
                            getPixelWrap(x_coord_lolo, y_coord_lolo),
                            getPixelWrap(x_coord_lolo, y_coord_lo),
                            getPixelWrap(x_coord_lolo, y_coord_hi),
                            getPixelWrap(x_coord_lolo, y_coord_hihi),
                            y_diff);
                    val1 = Tools.interpolateCubic(
                            getPixelWrap(x_coord_lo, y_coord_lolo),
                            getPixelWrap(x_coord_lo, y_coord_lo),
                            getPixelWrap(x_coord_lo, y_coord_hi),
                            getPixelWrap(x_coord_lo, y_coord_hihi),
                            y_diff);
                    val2 = Tools.interpolateCubic(
                            getPixelWrap(x_coord_hi, y_coord_lolo),
                            getPixelWrap(x_coord_hi, y_coord_lo),
                            getPixelWrap(x_coord_hi, y_coord_hi),
                            getPixelWrap(x_coord_hi, y_coord_hihi),
                            y_diff);
                    val3 = Tools.interpolateCubic(
                            getPixelWrap(x_coord_hihi, y_coord_lolo),
                            getPixelWrap(x_coord_hihi, y_coord_lo),
                            getPixelWrap(x_coord_hihi, y_coord_hi),
                            getPixelWrap(x_coord_hihi, y_coord_hihi),
                            y_diff);
                    channel.putPixel(x, y, Tools.interpolateCubic(val0, val1, val2, val3, x_diff));
                }
            }
            pixels = channel.getPixels();
            width = new_width;
            height = new_height;
            return this;
        }

        public Channel scaleCubicNonWrapping(int new_width, int new_height) {
            if (width == new_width && height == new_height) {
                return this;
            }
            Channel channel = new Channel(new_width, new_height);
            float x_coord, y_coord;
            float val0, val1, val2, val3;
            float height_ratio = (float)((float) height / new_height);
            float width_ratio = (float)((float) width / new_width);
            float x_diff, y_diff;
            int x_coord_lo, x_coord_lolo, x_coord_hi, x_coord_hihi, y_coord_lo, y_coord_lolo, y_coord_hi, y_coord_hihi;
            for (int y = 0; y < new_height; y++) {
                y_coord = (float)((float)(y * height_ratio) - 0.5f);
                y_coord_lo = J.I(y_coord);
                y_coord_lolo = y_coord_lo - 1;
                y_coord_hi = y_coord_lo + 1;
                y_coord_hihi = y_coord_hi + 1;
                y_diff = (float)(y_coord - y_coord_lo);
                for (int x = 0; x < new_width; x++) {
                    x_coord = (float)((float)(x * width_ratio) - 0.5f);
                    x_coord_lo = J.I(x_coord);
                    x_coord_lolo = x_coord_lo - 1;
                    x_coord_hi = x_coord_lo + 1;
                    x_coord_hihi = x_coord_hi + 1;
                    x_diff = (float)(x_coord - x_coord_lo);

                    // Use getPixelSafe for non-wrapping behavior
                    val0 = Tools.interpolateCubic(
                            getPixelSafe(x_coord_lolo, y_coord_lolo),
                            getPixelSafe(x_coord_lolo, y_coord_lo),
                            getPixelSafe(x_coord_lolo, y_coord_hi),
                            getPixelSafe(x_coord_lolo, y_coord_hihi),
                            y_diff);
                    val1 = Tools.interpolateCubic(
                            getPixelSafe(x_coord_lo, y_coord_lolo),
                            getPixelSafe(x_coord_lo, y_coord_lo),
                            getPixelSafe(x_coord_lo, y_coord_hi),
                            getPixelSafe(x_coord_lo, y_coord_hihi),
                            y_diff);
                    val2 = Tools.interpolateCubic(
                            getPixelSafe(x_coord_hi, y_coord_lolo),
                            getPixelSafe(x_coord_hi, y_coord_lo),
                            getPixelSafe(x_coord_hi, y_coord_hi),
                            getPixelSafe(x_coord_hi, y_coord_hihi),
                            y_diff);
                    val3 = Tools.interpolateCubic(
                            getPixelSafe(x_coord_hihi, y_coord_lolo),
                            getPixelSafe(x_coord_hihi, y_coord_lo),
                            getPixelSafe(x_coord_hihi, y_coord_hi),
                            getPixelSafe(x_coord_hihi, y_coord_hihi),
                            y_diff);
                    channel.putPixel(x, y, Tools.interpolateCubic(val0, val1, val2, val3, x_diff));
                }
            }
            pixels = channel.getPixels();
            width = new_width;
            height = new_height;
            return this;
        }

        public Channel scaleFast(int new_width, int new_height) {
            if (width == new_width && height == new_height) {
                return this;
            }
            Channel channel = new Channel(new_width, new_height);
            int x_coord = 0;
            int y_coord = 0;
            for (int y = 0; y < new_height; y++) {
                for (int x = 0; x < new_width; x++) {
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

        public Channel scaleDouble() {
            System.Diagnostics.Debug.Assert(width == height);

            // calculate filter
            Channel filter = new Channel(width << 1, height << 1);
            for (int y = 0; y < height; y++) {
                int y_shift = y << 1;
                for (int x = 0; x < width; x++) {
                    int x_shift = x << 1;
                    float value = (float)(0.25f * getPixel(x, y));
                    filter.putPixel(x_shift, y_shift, value);
                    filter.putPixel(x_shift + 1, y_shift, value);
                    filter.putPixel(x_shift, y_shift + 1, value);
                    filter.putPixel(x_shift + 1, y_shift + 1, value);
                }
            }

            // draw image
            Channel channel = new Channel(width << 1, height << 1);
            for (int y = 1; y < (height << 1) - 1; y++) {
                for (int x = 1; x < (width << 1) - 1; x++) {
                    channel.putPixel(x, y, (float)((float)((float)(filter.getPixel(x - 1, y) + filter.getPixel(x + 1, y)) + filter.getPixel(x,
                            y - 1)) + filter.getPixel(x, y + 1)));
                }
            }

            // fix edges
            int max = (width << 1) - 1;
            for (int i = 0; i < max; i++) {
                channel.putPixel(0, i, (float)((float)((float)(filter.getPixelWrap(-1, i) + filter.getPixelWrap(1, i)) + filter.getPixelWrap(0,
                        i - 1)) + filter.getPixelWrap(0, i + 1)));
                channel.putPixel(i, 0, (float)((float)((float)(filter.getPixelWrap(i, -1) + filter.getPixelWrap(i, 1)) + filter.getPixelWrap(i - 1,
                        0)) + filter.getPixelWrap(i + 1, 0)));
                channel.putPixel(max, i, (float)((float)((float)(filter.getPixelWrap(max - 1, i) + filter.getPixelWrap(max + 1,
                        i)) + filter.getPixelWrap(max, i - 1)) + filter.getPixelWrap(max, i + 1)));
                channel.putPixel(i, max, (float)((float)((float)(filter.getPixelWrap(i, max - 1) + filter.getPixelWrap(i,
                        max + 1)) + filter.getPixelWrap(i - 1, max)) + filter.getPixelWrap(i + 1, max)));
            }
            pixels = channel.getPixels();
            width = width << 1;
            height = height << 1;
            return this;
        }

        public Channel rotate(int degrees) {
            Channel channel = null;
            int tmp = width;
            switch (degrees) {
                case 90:
                    channel = new Channel(height, width);
                    for (int y = 0; y < height; y++) {
                        for (int x = 0; x < width; x++) {
                            channel.putPixel(y, width - x - 1, getPixel(x, y));
                        }
                    }
                    width = height;
                    height = tmp;
                    break;
                case 180:
                    channel = new Channel(width, height);
                    for (int y = 0; y < height; y++) {
                        for (int x = 0; x < width; x++) {
                            channel.putPixel(width - x - 1, height - y - 1, getPixel(x, y));
                        }
                    }
                    break;
                case 270:
                    channel = new Channel(height, width);
                    for (int y = 0; y < height; y++) {
                        for (int x = 0; x < width; x++) {
                            channel.putPixel(height - y - 1, x, getPixel(x, y));
                        }
                    }
                    width = height;
                    height = tmp;
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false);
                    break;
            }
            pixels = channel.getPixels();
            return this;
        }

        public Channel shear(float offset) {
            Channel channel = new Channel(width, height);
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    channel.putPixel(x, y, getPixelWrap(J.I(((float)(x + (float)((float)(offset * width) * ((float)((float) y / height)))))), y));
                }
            }
            pixels = channel.getPixels();
            return this;
        }

        public Channel sine(int frequency) {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    putPixel(x, y, (float) J.Sin(J.PI * 2 * frequency * getPixel(x, y)));
                }
            }
            return this;
        }

        public Channel xsine(int frequency) {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    putPixel(x, y, (float) J.Sin(2 * J.PI * ((float)((float)(((float)((float) x / width)) * frequency) + getPixel(x, y)))));
                }
            }
            return this.dynamicRange();
        }

        public Channel perturb(Channel channel1, Channel channel2) {
            Channel channel = new Channel(width, height);
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    float x_coord = (float)(x + (float)(width * channel1.getPixel(x, y)));
                    int x_coord_lo = J.I(x_coord);
                    int x_coord_hi = x_coord_lo + 1;
                    float x_frac = (float)(x_coord - x_coord_lo);
                    float y_coord = (float)(y + (float)(height * channel2.getPixel(x, y)));
                    int y_coord_lo = J.I(y_coord);
                    int y_coord_hi = y_coord_lo + 1;
                    float y_frac = (float)(y_coord - y_coord_lo);
                    float val1 = Tools.interpolateLinear(getPixelWrap(x_coord_lo, y_coord_lo), getPixelWrap(x_coord_hi,
                            y_coord_lo), x_frac);
                    float val2 = Tools.interpolateLinear(getPixelWrap(x_coord_lo, y_coord_hi), getPixelWrap(x_coord_hi,
                            y_coord_hi), x_frac);
                    channel.putPixel(x, y, Tools.interpolateLinear(val1, val2, y_frac));
                }
            }
            pixels = channel.getPixels();
            return this;
        }

        public Channel perturb(Channel perturb, float magnitude) {
            Channel channel = new Channel(width, height);
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    float x_coord = (float)(x + (float)((float)(width * magnitude) * ((float)(perturb.getPixel(x, y) - 0.5f))));
                    int x_coord_lo = J.I(x_coord);
                    int x_coord_hi = x_coord_lo + 1;
                    float x_frac = (float)(x_coord - x_coord_lo);
                    float y_coord = (float)(y + (float)((float)(height * magnitude) * ((float)(perturb.getPixel(y, x) - 0.5f))));
                    int y_coord_lo = J.I(y_coord);
                    int y_coord_hi = y_coord_lo + 1;
                    float y_frac = (float)(y_coord - y_coord_lo);
                    float val1 = Tools.interpolateLinear(getPixelWrap(x_coord_lo, y_coord_lo), getPixelWrap(x_coord_hi,
                            y_coord_lo), x_frac);
                    float val2 = Tools.interpolateLinear(getPixelWrap(x_coord_lo, y_coord_hi), getPixelWrap(x_coord_hi,
                            y_coord_hi), x_frac);
                    channel.putPixel(x, y, Tools.interpolateLinear(val1, val2, y_frac));
                }
            }
            pixels = channel.getPixels();
            return this;
        }

        public Channel flipH() {
            Channel channel = new Channel(width, height);
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    channel.putPixel(x, y, getPixel(width - x - 1, y));
                }
            }
            pixels = channel.getPixels();
            return this;
        }

        public Channel flipV() {
            float[] tmp;
            for (int y = 0; y < height >> 1; y++) {
                tmp = pixels[y];
                pixels[y] = pixels[height - y - 1];
                pixels[height - y - 1] = tmp;
            }
            return this;
        }

        public Channel smoothFast() {
            Channel filter = new Channel(width, height);
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    filter.putPixel(x, y, (float)(0.25f * getPixel(x, y)));
                }
            }
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    putPixel(x, y, (float)((float)((float)(filter.getPixelWrap(x - 1, y) + filter.getPixelWrap(x + 1, y)) + filter.getPixelWrap(x,
                            y - 1)) + filter.getPixelWrap(x, y + 1)));
                }
            }
            return this;
        }

        public Channel smooth(int radius) {
            radius = J.Max(1, radius);
            Channel filter = new Channel(width, height);
            float factor = (float)(1f / ((2 * radius + 1) * (2 * radius + 1)));
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    filter.putPixel(x, y, (float)(factor * getPixel(x, y)));
                }
            }
            for (int x = radius; x < width - radius; x++) {
                int y = radius;
                float sum = 0f;
                for (int i = -radius; i < radius + 1; i++) {
                    for (int j = -radius; j < radius + 1; j++) {
                        sum = (float)(sum + (filter.getPixel(x + j, y + i)));
                    }
                }
                for (y++; y < height - radius; y++) {
                    for (int j = -radius; j < radius + 1; j++) {
                        sum = (float)(sum - (filter.getPixel(x + j, y - radius - 1)));
                        sum = (float)(sum + (filter.getPixel(x + j, y + radius)));
                    }
                    putPixel(x, y, sum);
                }
            }
            return this;
        }

        public Channel smoothWrap(int radius) {
            radius = J.Max(1, radius);
            Channel filter = new Channel(width, height);
            float factor = (float)(1f / ((2 * radius + 1) * (2 * radius + 1)));
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    filter.putPixel(x, y, (float)(factor * getPixel(x, y)));
                }
            }
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    float sum = 0f;
                    for (int i = -radius; i < radius + 1; i++) {
                        for (int j = -radius; j < radius + 1; j++) {
                            sum = (float)(sum + (filter.getPixelWrap(x + j, y + i)));
                        }
                    }
                    putPixel(x, y, sum);
                }
            }
            return this;
        }

        public Channel smooth(int radius, Channel mask) {
            radius = J.Max(1, radius);
            Channel filter = new Channel(width, height);
            float factor = (float)(1f / ((2 * radius + 1) * (2 * radius + 1)));
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    filter.putPixel(x, y, (float)(factor * getPixel(x, y)));
                }
            }
            for (int x = radius; x < width - radius; x++) {
                int y = radius;
                float sum = 0f;
                for (int i = -radius; i < radius + 1; i++) {
                    for (int j = -radius; j < radius + 1; j++) {
                        sum = (float)(sum + (filter.getPixel(x + j, y + i)));
                    }
                }
                for (y++; y < height - radius; y++) {
                    float alpha = mask.getPixel(x, y);
                    if (alpha > 0) {
                        for (int j = -radius; j < radius + 1; j++) {
                            sum = (float)(sum - (filter.getPixel(x + j, y - radius - 1)));
                            sum = (float)(sum + (filter.getPixel(x + j, y + radius)));
                        }
                        putPixel(x, y, (float)((float)(alpha * sum) + (float)(((float)(1f - alpha)) * getPixel(x, y))));
                    }
                }
            }
            return this;
        }

        public Channel sharpen(int radius) {
            radius = J.Max(1, radius);
            Channel channel = new Channel(width, height);
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    float value = 0;
                    for (int i = -radius; i <= radius; i++) {
                        for (int j = -radius; j <= radius; j++) {
                            if (i == 0 && j == 0) {
                                value = (float)(value + ((float)((2 * radius + 1) * (2 * radius + 1) * getPixel(x, y))));
                            } else {
                                value = (float)(value - (getPixelWrap(x + i, y + j)));
                            }
                        }
                    }
                    channel.putPixel(x, y, value);
                }
            }
            pixels = channel.getPixels();
            return this;
        }

        public Channel convolution(float[][] filter, float divisor, float offset) {
            int radius = (filter[0].Length - 1) / 2;
            Channel channel = new Channel(width, height);
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    float value = 0;
                    for (int i = -radius; i <= radius; i++) {
                        for (int j = -radius; j <= radius; j++) {
                            value = (float)(value + ((float)(filter[i + radius][j + radius] * getPixelWrap(x + i, y + j))));
                        }
                    }
                    value = (float)((float)(value / divisor) + offset);
                    channel.putPixel(x, y, value);
                }
            }
            pixels = channel.getPixels();
            return this;
        }

        public Channel floodfill(int init_x, int init_y, float value, float tol, int[] countResult,
                int[] bounds) {
            System.Diagnostics.Debug.Assert(init_x < width && init_x >= 0);
            System.Diagnostics.Debug.Assert(init_y < height && init_y >= 0);
            int min_x = init_x;
            int min_y = init_y;
            int max_x = init_x;
            int max_y = init_y;
            float oldval = getPixel(init_x, init_y);
            bool[][] marked = J.New2<bool>(width, height);
            marked[init_x][init_y] = true;
            JDeque<int[]> list = new JDeque<int[]>();
            list.add(new int[]{init_x, init_y});

            int count = 1;
            while (!list.isEmpty()) {
                int[] coords = list.removeFirst();
                int x = coords[0];
                int y = coords[1];
                putPixel(x, y, value);
                if (x < min_x)
                    min_x = x;
                if (x > max_x)
                    max_x = x;
                if (y < min_y)
                    min_y = y;
                if (y > max_y)
                    max_y = y;
                if (x > 0 && J.Abs((float)(getPixel(x - 1, y) - oldval)) < tol && !marked[x - 1][y]) {
                    marked[x - 1][y] = true;
                    list.add(new int[]{x - 1, y});
                    count++;
                }
                if (x < width - 1 && J.Abs((float)(getPixel(x + 1, y) - oldval)) < tol && !marked[x + 1][y]) {
                    marked[x + 1][y] = true;
                    list.add(new int[]{x + 1, y});
                    count++;
                }
                if (y > 0 && J.Abs((float)(getPixel(x, y - 1) - oldval)) < tol && !marked[x][y - 1]) {
                    marked[x][y - 1] = true;
                    list.add(new int[]{x, y - 1});
                    count++;
                }
                if (y < height - 1 && J.Abs((float)(getPixel(x, y + 1) - oldval)) < tol && !marked[x][y + 1]) {
                    marked[x][y + 1] = true;
                    list.add(new int[]{x, y + 1});
                    count++;
                }
            }
            if (bounds != null) {
                bounds[0] = min_x;
                bounds[1] = min_y;
                bounds[2] = max_x;
                bounds[3] = max_y;
            }
            if (countResult != null) {
                countResult[0] = count;
            }
            return this;
        }

        public Channel largestConnected(float value) {
            Channel tmp = this.copy();
            Channel fillmap = new Channel(width, height);
            int[] fillcoords = tmp.findFirst(value);
            int max_count = 0;
            // Was allocated fresh INSIDE the loop below - once per disconnected region found. On a
            // large, irregular map (worst case observed: SIZE_UNREAL at 4096x4096, but any map with
            // many small disconnected patches is affected) that meant a fresh width*height bool
            // array - tens of megabytes at that size - allocated and discarded per region, potentially
            // hundreds or thousands of times over the course of one call. That's enough transient
            // allocation to cause severe GC pressure/thrashing that looks indistinguishable from a
            // hang. Hoisted out and reused across every region instead - safe to do because a pixel is
            // only ever marked true once it's been enqueued as part of SOME region's flood-fill, and by
            // definition two different connected regions never share a pixel, so a stale `true` left
            // over from an earlier region can never incorrectly block exploration of a later one; it
            // would only ever suppress re-visiting a pixel that's already been (or is about to be)
            // fully processed, which is exactly what it's supposed to do. //added by ikill240c
            bool[][] marked = J.New2<bool>(width, height); //added by ikill240c - hoisted out of the while loop below
            while (fillcoords[0] != -1) { // while reachable pixels remain
                int count = 0;
                int init_x = fillcoords[0];
                int init_y = fillcoords[1];
                fillmap.fill(0f);
                // flood fill
                marked[init_x][init_y] = true;
                JDeque<int[]> list = new JDeque<int[]>();
                list.add(new int[]{init_x, init_y});
                while (!list.isEmpty()) {
                    int[] coords = list.removeFirst();
                    int x = coords[0];
                    int y = coords[1];
                    tmp.putPixel(x, y, -1f);
                    fillmap.putPixel(x, y, 1f);
                    count++;
                    if (x > 0 && tmp.getPixel(x - 1, y) == 1f && !marked[x - 1][y]) {
                        marked[x - 1][y] = true;
                        list.add(new int[]{x - 1, y});
                    }
                    if (x < width - 1 && tmp.getPixel(x + 1, y) == 1f && !marked[x + 1][y]) {
                        marked[x + 1][y] = true;
                        list.add(new int[]{x + 1, y});
                    }
                    if (y > 0 && tmp.getPixel(x, y - 1) == 1f && !marked[x][y - 1]) {
                        marked[x][y - 1] = true;
                        list.add(new int[]{x, y - 1});
                    }
                    if (y < height - 1 && tmp.getPixel(x, y + 1) == 1f && !marked[x][y + 1]) {
                        marked[x][y + 1] = true;
                        list.add(new int[]{x, y + 1});
                    }
                }
                if (count > max_count) {
                    pixels = fillmap.copy().pixels;
                    max_count = count;
                }
                fillcoords = tmp.findFirst(value);
            }
            return this;
        }

        public float averageConnected(float value) {
            Channel tmp = this.copy();
            int[] fillcoords = tmp.findFirst(value);
            int area_count = 0;
            int area_total = 0;
            // Same fix as largestConnected() above, and for the same reason - see its comment for the
            // full explanation of why hoisting this out and reusing it across regions is safe.
            // //added by ikill240c
            bool[][] marked = J.New2<bool>(width, height); //added by ikill240c - hoisted out of the while loop below
            while (fillcoords[0] != -1) { // while reachable pixels remain
                area_count++;
                int count = 0;
                int init_x = fillcoords[0];
                int init_y = fillcoords[1];
                // flood fill
                marked[init_x][init_y] = true;
                JDeque<int[]> list = new JDeque<int[]>();
                list.add(new int[]{init_x, init_y});
                while (!list.isEmpty()) {
                    int[] coords = list.removeFirst();
                    int x = coords[0];
                    int y = coords[1];
                    tmp.putPixel(x, y, -1f);
                    count++;
                    if (x > 0 && tmp.getPixel(x - 1, y) == 1f && !marked[x - 1][y]) {
                        marked[x - 1][y] = true;
                        list.add(new int[]{x - 1, y});
                    }
                    if (x < width - 1 && tmp.getPixel(x + 1, y) == 1f && !marked[x + 1][y]) {
                        marked[x + 1][y] = true;
                        list.add(new int[]{x + 1, y});
                    }
                    if (y > 0 && tmp.getPixel(x, y - 1) == 1f && !marked[x][y - 1]) {
                        marked[x][y - 1] = true;
                        list.add(new int[]{x, y - 1});
                    }
                    if (y < height - 1 && tmp.getPixel(x, y + 1) == 1f && !marked[x][y + 1]) {
                        marked[x][y + 1] = true;
                        list.add(new int[]{x, y + 1});
                    }
                }
                fillcoords = tmp.findFirst(value);
                area_total += count;
            }
            return area_count == 0 ? -1f : (float)((float) area_total / area_count);
        }

        public Channel squareFit(float value, int size) {
            Channel channel = new Channel(width, height);
            bool match;
            for (int y = 0; y <= height - size; y++) {
                for (int x = 0; x <= width - size; x++) {
                    match = true;
                    for (int i = 0; i < size; i++) {
                        for (int j = 0; j < size; j++) {
                            match = match && (getPixel(x + i, y + j) == value);
                            if (!match) goto out_end;
                        }
                    }
                    out_end:
                    if (match) {
                        channel.putPixel(x, y, value);
                    }
                }
            }
            pixels = channel.copy().pixels;
            return this;
        }

        public Channel boxFit(float value, int width, int height) {
            Channel channel = new Channel(this.width, this.height);
            bool match;
            for (int y = 0; y <= this.height - height; y++) {
                for (int x = 0; x <= this.width - width; x++) {
                    match = true;
                    for (int i = 0; i < width; i++) {
                        for (int j = 0; j < height; j++) {
                            match = match && (getPixel(x + i, y + j) == value);
                            if (!match) goto out_end;
                        }
                    }
                    out_end:
                    if (match) {
                        channel.putPixel(x, y, value);
                    }
                }
            }
            pixels = channel.copy().pixels;
            return this;
        }

        public int count(float value) {
            int count = 0;
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    if (getPixel(x, y) == value)
                        count++;
                }
            }
            return count;
        }

        public Channel grow(float value, int radius) {
            Channel channel = this.copy();
            for (int y = radius; y < height - radius; y++) {
                for (int x = radius; x < width - radius; x++) {
                    if (getPixel(x, y) == value) {
                        for (int i = -radius; i <= radius; i++) {
                            for (int j = -radius; j <= radius; j++) {
                                channel.putPixel(x + i, y + j, value);
                            }
                        }
                    }
                }
            }
            pixels = channel.getPixels();
            return this;
        }

        public Channel squareGrow(float value, int size) {
            Channel channel = this.copy();
            for (int y = 0; y <= height - size; y++) {
                for (int x = 0; x <= width - size; x++) {
                    if (getPixel(x, y) == value) {
                        for (int i = 0; i < size; i++) {
                            for (int j = 0; j < size; j++) {
                                channel.putPixel(x + i, y + j, value);
                            }
                        }
                    }
                }
            }
            pixels = channel.getPixels();
            return this;
        }

        public int[] find(int radius, int x_start, int y_start, float value) {
            // Was getPixel(x_start, y_start) - a plain bounds-checked lookup, inconsistent with the
            // wrap-around getPixelWrap() used everywhere else in this method (and with the "find" name,
            // which implies searching the whole toroidal map like findNoWrap's non-wrapping counterpart
            // does not). Callers do legitimately pass out-of-range starting coordinates here - e.g.
            // Landscape.generateUnitLocations() chains this straight off another find's result - so an
            // out-of-range x_start/y_start threw IndexOutOfRangeException instead of wrapping and
            // searching normally. //added by ikill240c 2026-09-12
            if (getPixelWrap(x_start, y_start) == value) //added by ikill240c 2026-09-12
                return new int[]{(x_start + width) % width, (y_start + height) % height}; //added by ikill240c 2026-09-12
            int r = 1;
            while (r <= radius) {
                int x = x_start - r;
                int x2 = x_start + r;
                for (int i = 0; i < 2 * r - 1; i++) {
                    int y_i = y_start - r + 1 + i;
                    if (getPixelWrap(x, y_i) == value)
                        return new int[]{(x + width) % width, (y_i + height) % height};
                    if (getPixelWrap(x2, y_i) == value)
                        return new int[]{(x2 + width) % width, (y_i + height) % height};
                }
                int y = y_start - r;
                int y2 = y_start + r;
                for (int i = 0; i < 2 * r + 1; i++) {
                    int x_i = x_start - r + i;
                    if (getPixelWrap(x_i, y) == value)
                        return new int[]{(x_i + width) % width, (y + height) % height};
                    if (getPixelWrap(x_i, y2) == value)
                        return new int[]{(x_i + width) % width, (y2 + height) % height};
                }
                r++;
            }
            return new int[]{-1, -1};
        }

        public int[] findNoWrap(int radius, int x_start, int y_start, float value) {
            if (getPixel(x_start, y_start) == value)
                return new int[]{x_start, y_start};
            int r = 1;
            while (r <= radius) {
                int x = x_start - r;
                int x2 = x_start + r;
                for (int i = 0; i < 2 * r - 1; i++) {
                    int y_i = y_start - r + 1 + i;
                    if (x >= 0 && x < width && y_i >= 0 && y_i < height && getPixel(x, y_i) == value)
                        return new int[]{x, y_i};
                    if (x2 >= 0 && x2 < width && y_i >= 0 && y_i < height && getPixel(x2, y_i) == value)
                        return new int[]{x2, y_i};
                }
                int y = y_start - r;
                int y2 = y_start + r;
                for (int i = 0; i < 2 * r + 1; i++) {
                    int x_i = x_start - r + i;
                    if (x_i >= 0 && x_i < width && y >= 0 && y < height && getPixel(x_i, y) == value)
                        return new int[]{x_i, y};
                    if (x_i >= 0 && x_i < width && y2 >= 0 && y2 < height && getPixel(x_i, y2) == value)
                        return new int[]{x_i, y2};
                }
                r++;
            }
            return new int[]{-1, -1};
        }

        public int[] findFirst(float value) {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    if (getPixel(x, y) == value)
                        return new int[]{x, y};
                }
            }
            return new int[]{-1, -1};
        }

        public Channel bump(Channel bumpmap, float lx, float ly, float shadow, float light,
                float ambient) {
            System.Diagnostics.Debug.Assert(bumpmap.getWidth() == width && bumpmap.getHeight() == height);
            Channel channel = new Channel(width, height);
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    float nx = (float)(bumpmap.getPixelWrap(x + 1, y) - bumpmap.getPixelWrap(x - 1, y));
                    float ny = (float)(bumpmap.getPixelWrap(x, y + 1) - bumpmap.getPixelWrap(x, y - 1));
                    float brightness = (float)((float)(nx * lx) + (float)(ny * ly));
                    if (brightness >= 0) {
                        channel.putPixelClip(x, y, (float)(((float)(getPixel(x, y) + (float)(brightness * light))) * ((float)((float)((float)(bumpmap.getPixel(x,
                                y) * shadow) + 1) - shadow))));
                    } else {
                        channel.putPixelClip(x, y, (float)(((float)(getPixel(x, y) + (float)(brightness * ((float)(1 - ambient))))) * ((float)((float)((float)(bumpmap.getPixel(x,
                                y) * shadow) + 1) - shadow))));
                    }
                }
            }
            pixels = channel.getPixels();
            return this;
        }

        public Channel bumpSpecular(Channel bumpmap, float lx, float ly, float lz, float shadow,
                float light, int specular) {
            System.Diagnostics.Debug.Assert(bumpmap.getWidth() == width && bumpmap.getHeight() == height);
            float lnorm = (float) J.Sqrt((float)((float)((float)(lx * lx) + (float)(ly * ly)) + (float)(lz * lz)));
            float nz = (float)(4 * ((float)(1f / J.Min(width, height))));
            float nzlz = (float)(nz * lz);
            float nz2 = (float)(nz * nz);
            int power = 2 << specular;
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    float nx = (float)(bumpmap.getPixelWrap(x + 1, y) - bumpmap.getPixelWrap(x - 1, y));
                    float ny = (float)(bumpmap.getPixelWrap(x, y + 1) - bumpmap.getPixelWrap(x, y - 1));
                    float brightness = (float)((float)(nx * lx) + (float)(ny * ly));
                    float costheta = (float)(((float)(brightness + nzlz)) / ((float)((float) J.Sqrt((float)((float)((float)(nx * nx) + (float)(ny * ny)) + nz2)) * lnorm))); // can use math here, not game state affecting
                    float highlight;
                    if (costheta > 0) {
                        highlight = (float) J.Pow(costheta, power); // can use math here, not game state affecting
                    } else {
                        highlight = 0;
                    }
                    putPixelClip(x, y, (float)(((float)(getPixel(x, y) + (float)(highlight * light))) * ((float)((float)((float)(bumpmap.getPixel(x,
                            y) * shadow) + 1) - shadow))));
                }
            }
            return this;
        }

        public Channel lineart() {
            Channel channel = new Channel(width, height);
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    channel.putPixel(x, y, J.Max(
                            J.Max(
                                    J.Abs((float)(getPixelWrap(x, y) - getPixelWrap(x - 1, y))),
                                    J.Abs((float)(getPixelWrap(x, y) - getPixelWrap(x + 1, y)))),
                            J.Max(
                                    J.Abs((float)(getPixelWrap(x, y) - getPixelWrap(x, y - 1))),
                                    J.Abs((float)(getPixelWrap(x, y) - getPixelWrap(x, y + 1))))
                    ));
                }
            }
            pixels = channel.getPixels();
            return this;
        }

        public Channel relativeIntensity(int radius) {
            radius = J.Max(1, radius);
            Channel relint = new Channel(width, height);
            float factor = (float)(1f / ((2 * radius + 1) * (2 * radius + 1)));
            float sum, avr;

            for (int x = 0; x < width; x++) {
                int y = 0;
                sum = 0f;
                for (int i = -radius; i < radius + 1; i++) {
                    for (int j = -radius; j < radius + 1; j++) {
                        sum = (float)(sum + (getPixelWrap(x + j, y + i)));
                    }
                }
                for (; y < height; y++) {
                    if (y > 0) {
                        for (int j = -radius; j < radius + 1; j++) {
                            sum = (float)(sum - (getPixelWrap(x + j, y - radius - 1)));
                            sum = (float)(sum + (getPixelWrap(x + j, y + radius)));
                        }
                    }
                    avr = (float)(sum * factor);
                    relint.putPixel(x, y, (float)(getPixel(x, y) - avr));
                }
            }
            return relint.add(0.5f);
        }

        public Channel relativeIntensityNormalized(int radius) {
            return this.relativeIntensity(radius).dynamicRangeSymmetric();
        }

        public Channel[] fft() {
            System.Diagnostics.Debug.Assert(width == height);
            int size = width;
            System.Diagnostics.Debug.Assert(Utils.isPowerOf2(size));

            // convert channel to complex number array
            float[] a = new float[size * size * 2 + 1];
            int n = 1;
            for (int x = 0; x < size; x++) {
                for (int y = 0; y < size; y++) {
                    a[n] = getPixel(x, y);
                    n += 2;
                }
            }

            // perform fast fourier transform
            fastFourierTransform(a, size, 1);

            // convert complex number array to channels
            n = 1;
            Channel magnitude = new Channel(size, size);
            Channel phase = new Channel(size, size);
            float real, imag;
            for (int x = 0; x < size; x++) {
                for (int y = 0; y < size; y++) {
                    real = a[n++];
                    imag = a[n++];
                    magnitude.putPixel(x, y, (float) J.Sqrt((float)((float)(real * real) + (float)(imag * imag))));
                    if (imag == 0 && real >= 0) {
                        phase.putPixel(x, y, (float)((float) J.PI / 2f));
                    } else if (imag == 0 && real < 0) {
                        phase.putPixel(x, y, (float)((float) J.PI / -2f));
                    } else {
                        phase.putPixel(x, y, (float) J.Atan((float)(real / imag)));
                    }
                }
            }

            // return magnitude and phase channels
            return new Channel[]{magnitude.offset(size >> 1, size >> 1), phase};
        }

        public Channel fftInv(Channel magni, Channel phase) {
            System.Diagnostics.Debug.Assert(magni.width == magni.height && phase.width == phase.height
                    && magni.width == phase.width);
            int size = magni.width;
            System.Diagnostics.Debug.Assert(Utils.isPowerOf2(size));

            // convert channels to complex number array
            Channel magnitude = magni.copy().offset(size >> 1, size >> 1);
            float mag, pha;
            float[] a = new float[size * size * 2 + 1];
            int n = 1;
            for (int x = 0; x < size; x++) {
                for (int y = 0; y < size; y++) {
                    mag = magnitude.getPixel(x, y);
                    pha = phase.getPixel(x, y);
                    a[n++] = (float)(mag * (float) J.Cos(pha));
                    a[n++] = (float)(mag * (float) J.Sin(pha));
                }
            }

            // perform fast fourier transform
            fastFourierTransform(a, size, -1);

            // convert complex number array to channel
            n = 1;
            for (int x = 0; x < size; x++) {
                for (int y = 0; y < size; y++) {
                    putPixel(x, y, a[n]);
                    n += 2;
                }
            }

            // return real component channel
            return this;
        }

        private void fastFourierTransform(float[] data, int size, int isign) {
            int i1, i2, i3;
            int i2rev, i3rev, ip1, ip2, ip3, ifp1, ifp2;
            int ibit, idim, k1, k2, n, nprev, nrem, ntot;
            float tempi, tempr;
            float theta, wi, wpi, wpr, wr, wtemp;
            ntot = size * size;
            nprev = 1;
            for (idim = 2; idim >= 1; idim--) {
                n = size;
                nrem = ntot / (n * nprev);
                ip1 = nprev << 1;
                ip2 = ip1 * n;
                ip3 = ip2 * nrem;
                i2rev = 1;
                for (i2 = 1; i2 <= ip2; i2 += ip1) {
                    if (i2 < i2rev) {
                        for (i1 = i2; i1 <= i2 + ip1 - 2; i1 += 2) {
                            for (i3 = i1; i3 <= ip3; i3 += ip2) {
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
                    while (ibit >= ip1 && i2rev > ibit) {
                        i2rev -= ibit;
                        ibit >>= 1;
                    }
                    i2rev += ibit;
                }
                ifp1 = ip1;
                while (ifp1 < ip2) {
                    ifp2 = ifp1 << 1;
                    theta = (float)((float)(isign * ((float)((float) J.PI * 2))) / ((float)(ifp2 / (float) ip1)));
                    wtemp = (float) J.Sin(0.5 * theta);
                    wpr = (float)((float)(-2.0f * wtemp) * wtemp);
                    wpi = (float) J.Sin(theta);
                    wr = 1.0f;
                    wi = 0.0f;
                    for (i3 = 1; i3 <= ifp1; i3 += ip1) {
                        for (i1 = i3; i1 <= i3 + ip1 - 2; i1 += 2) {
                            for (i2 = i1; i2 <= ip3; i2 += ifp2) {
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

        public Layer toNormalMap(float strength, Channel specularMap) {
            Layer layer = toNormalMap(strength);
            layer.addAlpha(specularMap);
            return layer;
        }

        public Layer toNormalMap(float strength) {
            Channel r = new Channel(width, height);
            Channel g = new Channel(width, height);
            Channel b = new Channel(width, height);
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    // Sobel filter for better quality
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

                    float nx = (float)(-dX * strength);
                    float ny = (float)(-dY * strength);
                    float nz = 1.0f;

                    float len = (float) J.Sqrt((float)((float)((float)(nx * nx) + (float)(ny * ny)) + (float)(nz * nz)));
                    nx = (float)(nx / (len));
                    ny = (float)(ny / (len));
                    nz = (float)(nz / (len));

                    r.putPixel(x, y, (float)(((float)(nx + 1.0f)) * 0.5f));
                    g.putPixel(x, y, (float)(((float)(ny + 1.0f)) * 0.5f));
                    b.putPixel(x, y, (float)(((float)(nz + 1.0f)) * 0.5f));
                }
            }
            return new Layer(r, g, b);
        }

        public Channel erode(float talus, int iterations) {
            float h, h1, h2, h3, h4, d1, d2, d3, d4, max_d;
            int i, j;
            for (int iter = 0; iter < iterations; iter++) {
                for (int y = 1; y < height - 1; y++) {
                    for (int x = 1; x < width - 1; x++) {
                        h = getPixel(x, y);
                        h1 = getPixel(x, y + 1);
                        h2 = getPixel(x - 1, y);
                        h3 = getPixel(x + 1, y);
                        h4 = getPixel(x, y - 1);
                        d1 = (float)(h - h1);
                        d2 = (float)(h - h2);
                        d3 = (float)(h - h3);
                        d4 = (float)(h - h4);
                        i = 0;
                        j = 0;
                        max_d = 0f;
                        if (d1 > max_d) {
                            max_d = d1;
                            j = 1;
                        }
                        if (d2 > max_d) {
                            max_d = d2;
                            i = -1;
                            j = 0;
                        }
                        if (d3 > max_d) {
                            max_d = d3;
                            i = 1;
                            j = 0;
                        }
                        if (d4 > max_d) {
                            max_d = d4;
                            i = 0;
                            j = -1;
                        }
                        if (max_d > talus) {
                            continue;
                        }
                        max_d = (float)(max_d * (0.5f));
                        putPixel(x, y, (float)(getPixel(x, y) - max_d));
                        putPixel(x + i, y + j, (float)(getPixel(x + i, y + j) + max_d));
                    }
                }
            }
            return this;
        }

        public Channel erodeThermal(float talus, int iterations) {
            float h, h1, h2, h3, h4, d1, d2, d3, d4, max_d;
            int i, j;
            for (int iter = 0; iter < iterations; iter++) {
                for (int y = 1; y < height - 1; y++) {
                    for (int x = 1; x < width - 1; x++) {
                        h = getPixel(x, y);
                        h1 = getPixel(x, y + 1);
                        h2 = getPixel(x - 1, y);
                        h3 = getPixel(x + 1, y);
                        h4 = getPixel(x, y - 1);
                        d1 = (float)(h - h1);
                        d2 = (float)(h - h2);
                        d3 = (float)(h - h3);
                        d4 = (float)(h - h4);
                        i = 0;
                        j = 0;
                        max_d = 0f;
                        if (d1 > max_d) {
                            max_d = d1;
                            j = 1;
                        }
                        if (d2 > max_d) {
                            max_d = d2;
                            i = -1;
                            j = 0;
                        }
                        if (d3 > max_d) {
                            max_d = d3;
                            i = 1;
                            j = 0;
                        }
                        if (d4 > max_d) {
                            max_d = d4;
                            i = 0;
                            j = -1;
                        }
                        if (max_d < talus) {
                            continue;
                        }
                        max_d = (float)(max_d * (0.5f));
                        putPixel(x, y, (float)(getPixel(x, y) - max_d));
                        putPixel(x + i, y + j, (float)(getPixel(x + i, y + j) + max_d));
                    }
                }
            }
            return this;
        }

        public Channel place(Channel sprite, int x_offset, int y_offset) {
            for (int y = y_offset; y < y_offset + sprite.getHeight(); y++) {
                for (int x = x_offset; x < x_offset + sprite.getWidth(); x++) {
                    putPixelWrap(x, y, sprite.getPixelWrap(x - x_offset, y - y_offset));
                }
            }
            return this;
        }

        public Channel place(Channel sprite, Channel alpha, int x_offset, int y_offset) {
            float alpha_val;
            for (int y = y_offset; y < y_offset + sprite.getHeight(); y++) {
                for (int x = x_offset; x < x_offset + sprite.getWidth(); x++) {
                    alpha_val = alpha.getPixel(x - x_offset, y - y_offset);
                    putPixelWrap(x, y, (float)((float)(alpha_val * sprite.getPixelWrap(x - x_offset,
                            y - y_offset)) + (float)(((float)(1 - alpha_val)) * getPixelWrap(x, y))));
                }
            }
            return this;
        }

        public Channel placeBrightest(Channel sprite, int x_offset, int y_offset) {
            for (int y = y_offset; y < y_offset + sprite.getHeight(); y++) {
                for (int x = x_offset; x < x_offset + sprite.getWidth(); x++) {
                    putPixelWrap(x, y, J.Max(getPixelWrap(x, y), sprite.getPixelWrap(x - x_offset, y - y_offset)));
                }
            }
            return this;
        }

        public Channel placeDarkest(Channel sprite, int x_offset, int y_offset) {
            for (int y = y_offset; y < y_offset + sprite.getHeight(); y++) {
                for (int x = x_offset; x < x_offset + sprite.getWidth(); x++) {
                    putPixelWrap(x, y, J.Min(getPixelWrap(x, y), sprite.getPixelWrap(x - x_offset, y - y_offset)));
                }
            }
            return this;
        }

        public Channel abs() {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    putPixel(x, y, (float)(2f * J.Abs((float)(getPixel(x, y) - 0.5f))));
                }
            }
            return this;
        }


        public Channel channelBlend(Channel channel, float alpha) {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    putPixel(x, y, (float)((float)(alpha * channel.getPixel(x, y)) + (float)(((float)(1 - alpha)) * getPixel(x, y))));
                }
            }
            return this;
        }

        public Channel channelBlend(Channel channel, Channel alpha) {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    float alpha_val = alpha.getPixel(x, y);
                    putPixel(x, y, (float)((float)(alpha_val * channel.getPixel(x, y)) + (float)(((float)(1 - alpha_val)) * getPixel(x, y))));
                }
            }
            return this;
        }

        public Channel channelAdd(Channel channel) {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    putPixelClip(x, y, (float)(getPixel(x, y) + channel.getPixel(x, y)));
                }
            }
            return this;
        }

        public Channel channelAddNoClip(Channel channel) {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    putPixel(x, y, (float)(getPixel(x, y) + channel.getPixel(x, y)));
                }
            }
            return this;
        }

        public Channel channelSubtract(Channel channel) {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    putPixelClip(x, y, (float)(getPixel(x, y) - channel.getPixel(x, y)));
                }
            }
            return this;
        }

        public Channel channelSubtractNoClip(Channel channel) {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    putPixel(x, y, (float)(getPixel(x, y) - channel.getPixel(x, y)));
                }
            }
            return this;
        }

        public Channel channelAverage(Channel channel) {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    putPixel(x, y, (float)(((float)(getPixel(x, y) + channel.getPixel(x, y))) / 2f));
                }
            }
            return this;
        }

        public Channel channelMultiply(Channel channel) {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    putPixel(x, y, (float)(getPixel(x, y) * channel.getPixel(x, y)));
                }
            }
            return this;
        }

        public Channel channelDivide(Channel channel) {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    putPixel(x, y, (float)(getPixel(x, y) / channel.getPixel(x, y)));
                }
            }
            return this;
        }

        public Channel channelDifference(Channel channel) {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    putPixel(x, y, J.Abs((float)(getPixel(x, y) - channel.getPixel(x, y))));
                }
            }
            return this;
        }

        public Channel channelDarkest(Channel channel) {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    putPixel(x, y, J.Min(getPixel(x, y), channel.getPixel(x, y)));
                }
            }
            return this;
        }

        public Channel channelBrightest(Channel channel) {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    putPixel(x, y, J.Max(getPixel(x, y), channel.getPixel(x, y)));
                }
            }
            return this;
        }

    }
}
