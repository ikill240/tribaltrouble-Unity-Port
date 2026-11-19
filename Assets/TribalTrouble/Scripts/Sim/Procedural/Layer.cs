// Ported from Java (Tribal Trouble 1.4.1). Mechanically transliterated by PortTools/j2cs.py,
// then reviewed by hand. Java identifiers are kept so this file diffs against its source.
using System;
using System.Collections.Generic;
using TribalTrouble.Sim.Determinism;
using TribalTrouble.Sim;

namespace TribalTrouble.Sim.Procedural
{
    /**
     * Represents a multichannel image, typically RGBA, where each channel is a 2D grid of floating-point values.
     * This class is used for procedural image generation and manipulation.
     */
    public sealed class Layer {
        private const float GAMMA_EXPONENT = 2.2f;
        private static readonly float INV_GAMMA_EXPONENT = (float)(1f / GAMMA_EXPONENT);

        private int width;
        private int height;
        public Channel r;
        public Channel g;
        public Channel b;
        public Channel a;

        /**
         * Constructs a new, opaque white Layer with the specified dimensions.
         *
         * @param width  the width of the layer in pixels.
         * @param height the height of the layer in pixels.
         * @throws ArgumentException if width or height are not positive.
         */
        public Layer(int width, int height) {
            if (width <= 0 || height <= 0) {
                throw new ArgumentException("Width and height must be positive.");
            }
            this.width = width;
            this.height = height;
            Channel empty = new Channel(width, height);
            empty.fill(1f);
            this.r = empty;
            this.g = empty.copy();
            this.b = empty.copy();
            this.a = null;
        }

        /**
         * Constructs a new Layer from the given R, G, B, and optional A channels.
         *
         * @param r the red channel.
         * @param g the green channel.
         * @param b the blue channel.
         * @param a the alpha channel (optional, can be null).
         * @throws NullReferenceException     if R, G, or B channels are null.
         * @throws ArgumentException if channels have mismatching dimensions.
         */
        public Layer(Channel r, Channel g, Channel b,
                Channel a) {
            J.RequireNonNull(r, "Red channel cannot be null.");
            J.RequireNonNull(g, "Green channel cannot be null.");
            J.RequireNonNull(b, "Blue channel cannot be null.");

            this.width = r.getWidth();
            this.height = r.getHeight();
            if (g.getWidth() != width || b.getWidth() != width || g.getHeight() != height || b.getHeight() != height
                    || (a != null && (a.getWidth() != width || a.getHeight() != height))) {
                throw new ArgumentException("All channels must have the same dimensions.");
            }
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        /**
         * Constructs a new opaque Layer from the given R, G, and B channels.
         *
         * @param r the red channel.
         * @param g the green channel.
         * @param b the blue channel.
         * @throws NullReferenceException     if R, G, or B channels are null.
         * @throws ArgumentException if channels have mismatching dimensions.
         */
        public Layer(Channel r, Channel g,
                Channel b) : this(r, g, b, null) {
        }

        /**
         * Constructs a new Layer from an existing RGB Layer and a new alpha channel.
         *
         * @param rgb the source RGB layer.
         * @param a   the new alpha channel.
         * @throws NullReferenceException     if the rgb layer or alpha channel is null.
         * @throws ArgumentException if the alpha channel's dimensions do not match the layer's.
         */
        public Layer(Layer rgb, Channel a) : this(rgb.r, rgb.g, rgb.b, a) {
        }

        public void loadFromBytes(sbyte[] data) {
            float inv_255 = (float)(1f / 255f);
            int index = 0;
            for (int y = 0; y < getHeight(); y++) {
                for (int x = 0; x < getWidth(); x++) {
                    int br = J.U8(data[index++]);
                    int bg = J.U8(data[index++]);
                    int bb = J.U8(data[index++]);
                    int ba = J.U8(data[index++]);
                    float fr = (float)(br * inv_255);
                    float fg = (float)(bg * inv_255);
                    float fb = (float)(bb * inv_255);
                    float fa = (float)(ba * inv_255);
                    r.putPixel(x, y, fr);
                    g.putPixel(x, y, fg);
                    b.putPixel(x, y, fb);
                    if (a != null)
                        a.putPixel(x, y, fa);
                }
            }
        }

        public sbyte[] convertToBytes() {
            sbyte[] byte_pixel_data = new sbyte[getWidth() * getHeight() * 4];
            for (int y = 0; y < getHeight(); y++) {
                for (int x = 0; x < getWidth(); x++) {
                    int ri = (J.I(((float)((float)(r.getPixel(x, y) * 255) + .5f)))) & 0xff;
                    int gi = (J.I(((float)((float)(g.getPixel(x, y) * 255) + .5f)))) & 0xff;
                    int bi = (J.I(((float)((float)(b.getPixel(x, y) * 255) + .5f)))) & 0xff;
                    int ai;
                    if (a != null) {
                        ai = (J.I(((float)((float)(a.getPixel(x, y) * 255) + .5f)))) & 0xff;
                    } else {
                        ai = 255;
                    }
                    int index = y * getWidth() + x;
                    byte_pixel_data[index * 4] = J.B(ri);
                    byte_pixel_data[index * 4 + 1] = J.B(gi);
                    byte_pixel_data[index * 4 + 2] = J.B(bi);
                    byte_pixel_data[index * 4 + 3] = J.B(ai);
                }
            }
            return byte_pixel_data;
        }

        public void saveAsPNG(string filename) {
            // Debug image dump (Landscape.DEBUG). Not needed in the port; hook up if wanted.
        }

        public void addAlpha() {
            a = new Channel(width, height);
        }

        public void addAlpha(Channel alpha) {
            a = alpha;
        }

        public int getWidth() {
            return width;
        }

        public int getHeight() {
            return height;
        }

        public Channel getR() {
            return r;
        }

        public Channel getG() {
            return g;
        }

        public Channel getB() {
            return b;
        }

        public Channel getA() {
            return a;
        }

        /**
         * Sets the RGB values of a pixel at the specified coordinates.
         *
         * @param x the x-coordinate of the pixel.
         * @param y the y-coordinate of the pixel.
         * @param r the new red value.
         * @param g the new green value.
         * @param b the new blue value.
         * @throws IndexOutOfRangeException if the coordinates are out of bounds.
         */
        public void putPixel(int x, int y, float r, float g, float b) {
            this.r.putPixel(x, y, r);
            this.g.putPixel(x, y, g);
            this.b.putPixel(x, y, b);
        }

        /**
         * Sets the RGBA values of a pixel at the specified coordinates.
         *
         * @param x the x-coordinate of the pixel.
         * @param y the y-coordinate of the pixel.
         * @param r the new red value.
         * @param g the new green value.
         * @param b the new blue value.
         * @param a the new alpha value.
         * @throws IndexOutOfRangeException if the coordinates are out of bounds.
         */
        public void putPixel(int x, int y, float r, float g, float b, float a) {
            this.r.putPixel(x, y, r);
            this.g.putPixel(x, y, g);
            this.b.putPixel(x, y, b);
            if (this.a != null) {
                this.a.putPixel(x, y, a);
            }
        }

        public void putPixelWrap(int x, int y, float r, float g, float b) {
            this.r.putPixelWrap(x, y, r);
            this.g.putPixelWrap(x, y, g);
            this.b.putPixelWrap(x, y, b);
        }

        public void putPixelWrap(int x, int y, float r, float g, float b, float a) {
            this.r.putPixelWrap(x, y, r);
            this.g.putPixelWrap(x, y, g);
            this.b.putPixelWrap(x, y, b);
            if (this.a != null) {
                this.a.putPixelWrap(x, y, a);
            }
        }

        public void putPixelClip(int x, int y, float r, float g, float b) {
            this.r.putPixelClip(x, y, r);
            this.g.putPixelClip(x, y, g);
            this.b.putPixelClip(x, y, b);
        }

        public void putPixelClip(int x, int y, float r, float g, float b, float a) {
            this.r.putPixelClip(x, y, r);
            this.g.putPixelClip(x, y, g);
            this.b.putPixelClip(x, y, b);
            if (this.a != null) {
                this.a.putPixelClip(x, y, a);
            }
        }

        public void fill(float value) {
            r.fill(value);
            g.fill(value);
            b.fill(value);
        }

        public void fill(float r, float g, float b) {
            this.r.fill(r);
            this.g.fill(g);
            this.b.fill(b);
        }

        public void fill(float r, float g, float b, float a) {
            this.r.fill(r);
            this.g.fill(g);
            this.b.fill(b);
            if (this.a != null) {
                this.a.fill(a);
            }
        }

        public float findMin() {
            float min_r = r.findMin();
            float min_g = g.findMin();
            float min_b = b.findMin();
            return J.Min(min_r, J.Min(min_g, min_b));
        }

        public float findMax() {
            float max_r = r.findMax();
            float max_g = g.findMax();
            float max_b = b.findMax();
            return J.Max(max_r, J.Max(max_g, max_b));
        }

        public Layer copy() {
            if (a != null) {
                return new Layer(r.copy(), g.copy(), b.copy(), a.copy());
            } else {
                return new Layer(r.copy(), g.copy(), b.copy());
            }
        }

        public Layer dynamicRange() {
            float min_r = r.findMin();
            float min_g = g.findMin();
            float min_b = b.findMin();
            float max_r = r.findMax();
            float max_g = g.findMax();
            float max_b = b.findMax();
            float min = J.Min(min_r, J.Min(min_g, min_b));
            float max = J.Max(max_r, J.Max(max_g, max_b));
            min_r = Tools.interpolateLinear(0, 1, (float)(((float)(min_r - min)) / ((float)(max - min))));
            min_g = Tools.interpolateLinear(0, 1, (float)(((float)(min_g - min)) / ((float)(max - min))));
            min_b = Tools.interpolateLinear(0, 1, (float)(((float)(min_b - min)) / ((float)(max - min))));
            max_r = Tools.interpolateLinear(0, 1, (float)(((float)(max_r - min)) / ((float)(max - min))));
            max_g = Tools.interpolateLinear(0, 1, (float)(((float)(max_g - min)) / ((float)(max - min))));
            max_b = Tools.interpolateLinear(0, 1, (float)(((float)(max_b - min)) / ((float)(max - min))));
            r.dynamicRange(min_r, max_r);
            g.dynamicRange(min_g, max_g);
            b.dynamicRange(min_b, max_b);
            return this;
        }

        public Layer dynamicRange(float new_min, float new_max) {
            float min_r = r.findMin();
            float min_g = g.findMin();
            float min_b = b.findMin();
            float max_r = r.findMax();
            float max_g = g.findMax();
            float max_b = b.findMax();
            float min = J.Min(min_r, J.Min(min_g, min_b));
            float max = J.Max(max_r, J.Max(max_g, max_b));
            min_r = Tools.interpolateLinear(new_min, new_max, (float)(((float)(min_r - min)) / ((float)(max - min))));
            min_g = Tools.interpolateLinear(new_min, new_max, (float)(((float)(min_g - min)) / ((float)(max - min))));
            min_b = Tools.interpolateLinear(new_min, new_max, (float)(((float)(min_b - min)) / ((float)(max - min))));
            max_r = Tools.interpolateLinear(new_min, new_max, (float)(((float)(max_r - min)) / ((float)(max - min))));
            max_g = Tools.interpolateLinear(new_min, new_max, (float)(((float)(max_g - min)) / ((float)(max - min))));
            max_b = Tools.interpolateLinear(new_min, new_max, (float)(((float)(max_b - min)) / ((float)(max - min))));
            r.dynamicRange(min_r, max_r);
            g.dynamicRange(min_g, max_g);
            b.dynamicRange(min_b, max_b);
            return this;
        }

        public Layer dynamicRange(float min, float max, float new_min, float new_max) {
            r.dynamicRange(min, max, new_min, new_max);
            g.dynamicRange(min, max, new_min, new_max);
            b.dynamicRange(min, max, new_min, new_max);
            return this;
        }

        public Layer clip() {
            r.clip();
            g.clip();
            b.clip();
            return this;
        }

        public Layer crop(int x_lo, int y_lo, int x_hi, int y_hi) {
            r = r.crop(x_lo, y_lo, x_hi, y_hi);
            g = g.crop(x_lo, y_lo, x_hi, y_hi);
            b = b.crop(x_lo, y_lo, x_hi, y_hi);
            if (a != null) {
                a = a.crop(x_lo, y_lo, x_hi, y_hi);
            }
            width = r.getWidth();
            height = r.getHeight();
            return this;
        }

        public Layer cropWrap(int x_lo, int y_lo, int x_hi, int y_hi) {
            r = r.cropWrap(x_lo, y_lo, x_hi, y_hi);
            g = g.cropWrap(x_lo, y_lo, x_hi, y_hi);
            b = b.cropWrap(x_lo, y_lo, x_hi, y_hi);
            if (a != null) {
                a = a.cropWrap(x_lo, y_lo, x_hi, y_hi);
            }
            width = r.getWidth();
            height = r.getHeight();
            return this;
        }

        public Layer tile(int new_width, int new_height) {
            r = r.tile(new_width, new_height);
            g = g.tile(new_width, new_height);
            b = b.tile(new_width, new_height);
            if (a != null) {
                a = a.tile(new_width, new_height);
            }
            width = r.getWidth();
            height = r.getHeight();
            return this;
        }

        public Layer tileDouble() {
            r = r.tileDouble();
            g = g.tileDouble();
            b = b.tileDouble();
            if (a != null) {
                a = a.tileDouble();
            }
            width = width << 1;
            height = height << 1;
            return this;
        }

        public Layer offset(int x_offset, int y_offset) {
            r = r.offset(x_offset, y_offset);
            g = g.offset(x_offset, y_offset);
            b = b.offset(x_offset, y_offset);
            if (a != null) {
                a = a.offset(x_offset, y_offset);
            }
            return this;
        }

        public Layer brightness(float brightness) {
            r.brightness(brightness);
            g.brightness(brightness);
            b.brightness(brightness);
            return this;
        }

        public Layer brightness(float r, float g, float b) {
            this.r.brightness(r);
            this.g.brightness(g);
            this.b.brightness(b);
            return this;
        }

        public Layer multiply(float factor) {
            r.multiply(factor);
            g.multiply(factor);
            b.multiply(factor);
            return this;
        }

        public Layer multiply(float r, float g, float b) {
            this.r.multiply(r);
            this.g.multiply(g);
            this.b.multiply(b);
            return this;
        }

        public Layer multiply(float r, float g, float b, float a) {
            this.r.multiply(r);
            this.g.multiply(g);
            this.b.multiply(b);
            if (this.a != null) {
                this.a.multiply(a);
            }
            return this;
        }

        public Layer add(float add) {
            r.add(add);
            g.add(add);
            b.add(add);
            return this;
        }

        public Layer add(float r, float g, float b) {
            this.r.add(r);
            this.g.add(g);
            this.b.add(b);
            return this;
        }

        public Layer addClip(float r, float g, float b) {
            this.r.addClip(r);
            this.g.addClip(g);
            this.b.addClip(b);
            return this;
        }

        public Layer addClip(float r, float g, float b, float a) {
            this.r.addClip(r);
            this.g.addClip(g);
            this.b.addClip(b);
            if (this.a != null) {
                this.a.addClip(a);
            }
            return this;
        }

        public Layer contrast(float contrast) {
            r.contrast(contrast);
            g.contrast(contrast);
            b.contrast(contrast);
            return this;
        }

        public Layer contrast(float r, float g, float b) {
            this.r.contrast(r);
            this.g.contrast(g);
            this.b.contrast(b);
            return this;
        }

        public Layer gamma(float gamma) {
            r.gamma(gamma);
            g.gamma(gamma);
            b.gamma(gamma);
            return this;
        }

        public Layer gamma(float r, float g, float b) {
            this.r.gamma(r);
            this.g.gamma(g);
            this.b.gamma(b);
            return this;
        }

        public Layer gamma2() {
            r.gamma2();
            g.gamma2();
            b.gamma2();
            return this;
        }

        public Layer gamma4() {
            r.gamma4();
            g.gamma4();
            b.gamma4();
            return this;
        }

        public Layer gamma8() {
            r.gamma8();
            g.gamma8();
            b.gamma8();
            return this;
        }

        public Layer invert() {
            r.invert();
            g.invert();
            b.invert();
            return this;
        }

        public Layer threshold(float start, float end) {
            r.threshold(start, end);
            g.threshold(start, end);
            b.threshold(start, end);
            return this;
        }

        public Layer scaleHalf() {
            r = r.scaleHalf();
            g = g.scaleHalf();
            b = b.scaleHalf();
            if (a != null) {
                a = a.scaleHalf();
            }
            width = r.getWidth();
            height = r.getHeight();
            return this;
        }

        public Layer scale(int new_width, int new_height) {
            r = r.scale(new_width, new_height);
            g = g.scale(new_width, new_height);
            b = b.scale(new_width, new_height);
            if (a != null) {
                a = a.scale(new_width, new_height);
            }
            width = new_width;
            height = new_height;
            return this;
        }

        public Layer scaleCubicNonWrapping(int new_width, int new_height) {
            r = r.scaleCubicNonWrapping(new_width, new_height);
            g = g.scaleCubicNonWrapping(new_width, new_height);
            b = b.scaleCubicNonWrapping(new_width, new_height);
            if (a != null) {
                a = a.scaleCubicNonWrapping(new_width, new_height);
            }
            width = new_width;
            height = new_height;
            return this;
        }

        public Layer scaleCubicWrapping(int new_width, int new_height) {
            r = r.scaleCubicWrapping(new_width, new_height);
            g = g.scaleCubicWrapping(new_width, new_height);
            b = b.scaleCubicWrapping(new_width, new_height);
            if (a != null) {
                a = a.scaleCubicWrapping(new_width, new_height);
            }
            width = new_width;
            height = new_height;
            return this;
        }

        public Layer scaleFast(int new_width, int new_height) {
            r = r.scaleFast(new_width, new_height);
            g = g.scaleFast(new_width, new_height);
            b = b.scaleFast(new_width, new_height);
            if (a != null) {
                a = a.scaleFast(new_width, new_height);
            }
            width = new_width;
            height = new_height;
            return this;
        }

        public Layer rotate(int degrees) {
            r = r.rotate(degrees);
            g = g.rotate(degrees);
            b = b.rotate(degrees);
            if (a != null) {
                a = a.rotate(degrees);
            }
            width = r.getWidth();
            height = r.getHeight();
            return this;
        }

        public Layer shear(float offset) {
            r = r.shear(offset);
            g = g.shear(offset);
            b = b.shear(offset);
            if (a != null) {
                a = a.shear(offset);
            }
            return this;
        }

        public Layer flipH() {
            r = r.flipH();
            g = g.flipH();
            b = b.flipH();
            if (a != null) {
                a = a.flipH();
            }
            return this;
        }

        public Layer flipV() {
            r = r.flipV();
            g = g.flipV();
            b = b.flipV();
            if (a != null) {
                a = a.flipV();
            }
            return this;
        }

        public Layer smooth(int radius) {
            r.smooth(radius);
            g.smooth(radius);
            b.smooth(radius);
            return this;
        }

        public Layer sharpen(int radius) {
            r.sharpen(radius);
            g.sharpen(radius);
            b.sharpen(radius);

            return this;
        }

        public Layer convolution(float[][] filter, float divisor, float offset) {
            r.convolution(filter, divisor, offset);
            g.convolution(filter, divisor, offset);
            b.convolution(filter, divisor, offset);
            return this;
        }

        public Layer grow(float r, float g, float b, int radius) {
            this.r.grow(r, radius);
            this.g.grow(g, radius);
            this.b.grow(b, radius);
            return this;
        }

        public Layer bump(Channel bumpmap, float lx, float ly, float shadow, float light_r, float light_g,
                float light_b, float ambient_r, float ambient_g, float ambient_b) {
            System.Diagnostics.Debug.Assert(bumpmap.getWidth() == width && bumpmap.getHeight() == height);
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    float nx = (float)(bumpmap.getPixelWrap(x + 1, y) - bumpmap.getPixelWrap(x - 1, y));
                    float ny = (float)(bumpmap.getPixelWrap(x, y + 1) - bumpmap.getPixelWrap(x, y - 1));
                    float brightness = (float)((float)(nx * lx) + (float)(ny * ly));
                    if (brightness >= 0) {
                        putPixelClip(x, y, (float)(((float)(r.getPixel(x, y) + (float)(brightness * light_r))) * ((float)((float)((float)(bumpmap.getPixel(x,
                                y) * shadow) + 1) - shadow))),
                                (float)(((float)(g.getPixel(x, y) + (float)(brightness * light_g))) * ((float)((float)((float)(bumpmap.getPixel(x, y) * shadow) + 1) - shadow))),
                                (float)(((float)(b.getPixel(x, y) + (float)(brightness * light_b))) * ((float)((float)((float)(bumpmap.getPixel(x, y) * shadow) + 1) - shadow))));
                    } else {
                        putPixelClip(x, y, (float)(((float)(r.getPixel(x, y) + (float)(brightness * ((float)(1 - ambient_r))))) * ((float)((float)((float)(bumpmap.getPixel(x,
                                y) * shadow) + 1) - shadow))),
                                (float)(((float)(g.getPixel(x, y) + (float)(brightness * ((float)(1 - ambient_g))))) * ((float)((float)((float)(bumpmap.getPixel(x,
                                        y) * shadow) + 1) - shadow))),
                                (float)(((float)(b.getPixel(x, y) + (float)(brightness * ((float)(1 - ambient_b))))) * ((float)((float)((float)(bumpmap.getPixel(x,
                                        y) * shadow) + 1) - shadow))));
                    }
                }
            }
            return this;
        }

        public Layer bumpFast(Channel bumpmap, float lx, float light, float ambient) {
            System.Diagnostics.Debug.Assert(bumpmap.getWidth() == width && bumpmap.getHeight() == height);
            ambient = (float)(1f - ambient);
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    float brightness = (float)(lx * ((float)(bumpmap.getPixelWrap(x + 1, y) - bumpmap.getPixelWrap(x - 1, y))));
                    if (brightness >= 0) {
                        brightness = (float)(brightness * light);
                        putPixel(x, y, (float)(r.getPixel(x, y) + brightness),
                                (float)(g.getPixel(x, y) + brightness),
                                (float)(b.getPixel(x, y) + brightness));
                    } else {
                        brightness = (float)(brightness * ambient);
                        putPixel(x, y, (float)(r.getPixel(x, y) + brightness),
                                (float)(g.getPixel(x, y) + brightness),
                                (float)(b.getPixel(x, y) + brightness));
                    }
                }
            }
            return this;
        }

        public Layer bumpSpecular(Channel bumpmap, float lx, float ly, float lz, float shadow,
                float light_r, float light_g, float light_b, int specular) {
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
                    putPixelClip(x, y,
                            (float)(((float)(r.getPixel(x, y) + (float)(highlight * light_r))) * ((float)((float)((float)(bumpmap.getPixel(x, y) * shadow) + 1) - shadow))),
                            (float)(((float)(g.getPixel(x, y) + (float)(highlight * light_g))) * ((float)((float)((float)(bumpmap.getPixel(x, y) * shadow) + 1) - shadow))),
                            (float)(((float)(b.getPixel(x, y) + (float)(highlight * light_b))) * ((float)((float)((float)(bumpmap.getPixel(x, y) * shadow) + 1) - shadow))));
                }
            }
            return this;
        }

        public Layer toHSV() {
            float min = 0;
            float max = 0;
            float delta = 0;
            float h_val = 0;
            float s_val = 0;
            float v_val = 0;

            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    float r_val = r.getPixel(x, y);
                    float g_val = g.getPixel(x, y);
                    float b_val = b.getPixel(x, y);
                    min = J.Min(r_val, J.Min(g_val, b_val));
                    max = J.Max(r_val, J.Max(g_val, b_val));

                    v_val = max;
                    delta = (float)(max - min);

                    if (max != 0) {
                        s_val = (float)(delta / max);
                    } else {
                        s_val = 0;
                    }

                    if (max == r_val) {
                        h_val = (float)(((float)(g_val - b_val)) / delta);
                    }
                    if (max == g_val) {
                        h_val = (float)(2 + (float)(((float)(b_val - r_val)) / delta));
                    }
                    if (max == b_val) {
                        h_val = (float)(4 + (float)(((float)(r_val - g_val)) / delta));
                    }

                    h_val = (float)(h_val / (6));
                    if (h_val < 0) {
                        h_val = (float)(h_val + (1));
                    }

                    r.putPixel(x, y, h_val);
                    g.putPixel(x, y, s_val);
                    b.putPixel(x, y, v_val);
                }
            }
            return this;
        }

        public Layer toRGB() {
            int i;
            float f;
            float p;
            float q;
            float t;
            float r_val = 0;
            float g_val = 0;
            float b_val = 0;

            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    float h_val = r.getPixel(x, y);
                    float s_val = g.getPixel(x, y);
                    float v_val = b.getPixel(x, y);

                    if (s_val == 0) {
                        r.putPixel(x, y, v_val);
                        g.putPixel(x, y, v_val);
                        b.putPixel(x, y, v_val);
                        continue;
                    }

                    h_val = (float)(h_val * (6));
                    i = J.I(h_val);
                    f = (float)(h_val - i);
                    p = (float)(v_val * ((float)(1 - s_val)));
                    q = (float)(v_val * ((float)(1 - (float)(s_val * f))));
                    t = (float)(v_val * ((float)(1 - (float)(s_val * ((float)(1 - f))))));

                    switch (i) {
                        case 0:
                            r_val = v_val;
                            g_val = t;
                            b_val = p;
                            break;
                        case 1:
                            r_val = q;
                            g_val = v_val;
                            b_val = p;
                            break;
                        case 2:
                            r_val = p;
                            g_val = v_val;
                            b_val = t;
                            break;
                        case 3:
                            r_val = p;
                            g_val = q;
                            b_val = v_val;
                            break;
                        case 4:
                            r_val = t;
                            g_val = p;
                            b_val = v_val;
                            break;
                        case 5:
                            r_val = v_val;
                            g_val = p;
                            b_val = q;
                            break;
                        case 6:
                            r_val = v_val;
                            g_val = p;
                            b_val = q;
                            break;
                        default:
                            System.Diagnostics.Debug.Assert(false);
                            break;
                    }

                    r.putPixel(x, y, r_val);
                    g.putPixel(x, y, g_val);
                    b.putPixel(x, y, b_val);
                }
            }
            return this;
        }

        public Layer saturation(float saturation) {
            toHSV();
            float s_val;
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    s_val = (float)(g.getPixel(x, y) * saturation);
                    if (s_val < 0) {
                        s_val = 0;
                    } else {
                        if (s_val > 1) {
                            s_val = 1;
                        }
                    }
                    g.putPixel(x, y, s_val);
                }
            }
            return toRGB();
        }

        public Layer hue(float hue) {
            toHSV();
            float h_val;
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    h_val = (float)(r.getPixel(x, y) + hue);
                    if (h_val < 0) {
                        h_val = (float)(h_val + (1));
                    } else {
                        if (h_val > 1) {
                            h_val = (float)(h_val - (1));
                        }
                    }
                    r.putPixel(x, y, h_val);
                }
            }
            return toRGB();
        }

        public Layer hueRotation(float min, float max, float new_min, float new_max) {
            toHSV();
            r.dynamicRange(min, max, new_min, new_max);
            return toRGB();
        }

        public Layer lineart() {
            r.lineart();
            g.lineart();
            b.lineart();
            return this;
        }

        public Layer place(Layer sprite, int x_offset, int y_offset) {
            r.place(sprite.r, x_offset, y_offset);
            g.place(sprite.g, x_offset, y_offset);
            b.place(sprite.b, x_offset, y_offset);
            if (a != null && sprite.a != null)
                a.place(sprite.a, x_offset, y_offset);
            return this;
        }

        public Layer abs() {
            r.abs();
            g.abs();
            b.abs();
            return this;
        }

        public Layer layerBlend(Layer layer, float alpha) {
            r.channelBlend(layer.r, alpha);
            g.channelBlend(layer.g, alpha);
            b.channelBlend(layer.b, alpha);
            return this;
        }

        public Layer layerBlend(Layer rgb, Channel a) {
            return layerBlend(new Layer(rgb.r, rgb.g, rgb.b, a));
        }

        public Layer layerBlend(Layer layer) {
            System.Diagnostics.Debug.Assert(layer.a != null);

            if (a == null) {
                r.channelBlend(layer.r, layer.a);
                g.channelBlend(layer.g, layer.a);
                b.channelBlend(layer.b, layer.a);
            } else {
                float alpha;
                float alpha_inv;
                float r1;
                float g1;
                float b1;
                float a2;
                for (int y = 0; y < height; y++) {
                    for (int x = 0; x < width; x++) {
                        if (a.getPixel(x, y) == 0) {
                            putPixel(x, y, layer.r.getPixel(x, y), layer.g.getPixel(x, y), layer.b.getPixel(x, y),
                                    layer.a.getPixel(x, y));
                        } else if (layer.a.getPixel(x, y) == 0) {
                        } else {
                            alpha = (float)(1f - (float)(((float)(1f - a.getPixel(x, y))) * ((float)(1f - layer.a.getPixel(x, y)))));
                            alpha_inv = (float)(1f / alpha);
                            r1 = r.getPixel(x, y);
                            g1 = g.getPixel(x, y);
                            b1 = b.getPixel(x, y);
                            a2 = layer.a.getPixel(x, y);
                            r.putPixel(x, y, (float)((float)(r1 - (float)((float)(r1 * a2) * alpha_inv)) + (float)((float)(layer.r.getPixel(x, y) * a2) * alpha_inv)));
                            g.putPixel(x, y, (float)((float)(g1 - (float)((float)(g1 * a2) * alpha_inv)) + (float)((float)(layer.g.getPixel(x, y) * a2) * alpha_inv)));
                            b.putPixel(x, y, (float)((float)(b1 - (float)((float)(b1 * a2) * alpha_inv)) + (float)((float)(layer.b.getPixel(x, y) * a2) * alpha_inv)));
                            a.putPixel(x, y, alpha);
                        }
                    }
                }
            }
            return this;
        }

        public Layer layerAdd(Layer layer) {
            r.channelAdd(layer.r);
            g.channelAdd(layer.g);
            b.channelAdd(layer.b);
            return this;
        }

        public Layer layerSubtract(Layer layer) {
            r.channelSubtract(layer.r);
            g.channelSubtract(layer.g);
            b.channelSubtract(layer.b);
            return this;
        }

        public Layer layerAverage(Layer layer) {
            r.channelAverage(layer.r);
            g.channelAverage(layer.g);
            b.channelAverage(layer.b);
            return this;
        }

        public Layer layerMultiply(Layer layer) {
            r.channelMultiply(layer.r);
            g.channelMultiply(layer.g);
            b.channelMultiply(layer.b);
            return this;
        }

        public Layer layerDifference(Layer layer) {
            r.channelDifference(layer.r);
            g.channelDifference(layer.g);
            b.channelDifference(layer.b);
            return this;
        }

        public Layer layerDarkest(Layer layer) {
            r.channelDarkest(layer.r);
            g.channelDarkest(layer.g);
            b.channelDarkest(layer.b);
            return this;
        }

        public Layer layerBrightest(Layer layer) {
            r.channelBrightest(layer.r);
            g.channelBrightest(layer.g);
            b.channelBrightest(layer.b);
            return this;
        }
    }
}
