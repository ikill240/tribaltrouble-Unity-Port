// Translated from Layer.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.procedural
{
    // Java: 21
    public class Layer : java.lang.Object
    {
        public static java.util.logging.Logger logger;
        public const float GAMMA_EXPONENT = 2.2f;
        public const float INV_GAMMA_EXPONENT = 0.45454544f;
        public int width;
        public int height;
        public com.oddlabs.procedural.Channel r;
        public com.oddlabs.procedural.Channel g;
        public com.oddlabs.procedural.Channel b;
        public com.oddlabs.procedural.Channel a;
        // Java: 108
        public void loadFromBytes(sbyte[] data)
        {
            float inv_255 = (float)(1.0f / 255.0f);
            int index = 0;
            for (int y = 0; y < getHeight(); y++)
            {
                for (int x = 0; x < getWidth(); x++)
                {
                    int br = java.lang.Byte.toUnsignedInt(data[index++]);
                    int bg = java.lang.Byte.toUnsignedInt(data[index++]);
                    int bb = java.lang.Byte.toUnsignedInt(data[index++]);
                    int ba = java.lang.Byte.toUnsignedInt(data[index++]);
                    float fr = (float)(br * inv_255);
                    float fg = (float)(bg * inv_255);
                    float fb = (float)(bb * inv_255);
                    float fa = (float)(ba * inv_255);
                    r.putPixel(x, y, fr);
                    g.putPixel(x, y, fg);
                    b.putPixel(x, y, fb);
                    if (a != null)
                    {
                        a.putPixel(x, y, fa);
                    }
                }
            }
        }
        // Java: 130
        public sbyte[] convertToBytes()
        {
            sbyte[] byte_pixel_data = new sbyte[getWidth() * getHeight() * 4];
            for (int y = 0; y < getHeight(); y++)
            {
                for (int x = 0; x < getWidth(); x++)
                {
                    int ri = (java.lang.JC.f2i(((float)((float)(r.getPixel(x, y) * 255) + 0.5f)))) & 255;
                    int gi = (java.lang.JC.f2i(((float)((float)(g.getPixel(x, y) * 255) + 0.5f)))) & 255;
                    int bi = (java.lang.JC.f2i(((float)((float)(b.getPixel(x, y) * 255) + 0.5f)))) & 255;
                    int ai = default;
                    if (a != null)
                    {
                        ai = (java.lang.JC.f2i(((float)((float)(a.getPixel(x, y) * 255) + 0.5f)))) & 255;
                    }
                    else
                    {
                        ai = 255;
                    }
                    int index = y * getWidth() + x;
                    byte_pixel_data[index * 4] = unchecked((sbyte)(ri));
                    byte_pixel_data[index * 4 + 1] = unchecked((sbyte)(gi));
                    byte_pixel_data[index * 4 + 2] = unchecked((sbyte)(bi));
                    byte_pixel_data[index * 4 + 3] = unchecked((sbyte)(ai));
                }
            }
            return byte_pixel_data;
        }
        // Java: 153
        public java.awt.image.BufferedImage convertToImage()
        {
            throw new System.NotSupportedException("J2CS stub: Layer.convertToImage");
        }
        // Java: 160
        public void saveAsPNG(string filename)
        {
            throw new System.NotSupportedException("J2CS stub: Layer.saveAsPNG");
        }
        // Java: 170
        public void saveAsPNG(java.nio.file.Path file)
        {
            throw new System.NotSupportedException("J2CS stub: Layer.saveAsPNG");
        }
        // Java: 176
        public void addAlpha()
        {
            a = new com.oddlabs.procedural.Channel(width, height);
        }
        // Java: 180
        public void addAlpha(com.oddlabs.procedural.Channel alpha)
        {
            a = alpha;
        }
        // Java: 184
        public int getWidth()
        {
            return width;
        }
        // Java: 188
        public int getHeight()
        {
            return height;
        }
        // Java: 192
        public com.oddlabs.procedural.Channel getR()
        {
            return r;
        }
        // Java: 196
        public com.oddlabs.procedural.Channel getG()
        {
            return g;
        }
        // Java: 200
        public com.oddlabs.procedural.Channel getB()
        {
            return b;
        }
        // Java: 204
        public com.oddlabs.procedural.Channel getA()
        {
            return a;
        }
        // Java: 218
        public void putPixel(int x, int y, float r, float g, float b)
        {
            this.r.putPixel(x, y, r);
            this.g.putPixel(x, y, g);
            this.b.putPixel(x, y, b);
        }
        // Java: 235
        public void putPixel(int x, int y, float r, float g, float b, float a)
        {
            this.r.putPixel(x, y, r);
            this.g.putPixel(x, y, g);
            this.b.putPixel(x, y, b);
            if (this.a != null)
            {
                this.a.putPixel(x, y, a);
            }
        }
        // Java: 244
        public void putPixelWrap(int x, int y, float r, float g, float b)
        {
            this.r.putPixelWrap(x, y, r);
            this.g.putPixelWrap(x, y, g);
            this.b.putPixelWrap(x, y, b);
        }
        // Java: 250
        public void putPixelWrap(int x, int y, float r, float g, float b, float a)
        {
            this.r.putPixelWrap(x, y, r);
            this.g.putPixelWrap(x, y, g);
            this.b.putPixelWrap(x, y, b);
            if (this.a != null)
            {
                this.a.putPixelWrap(x, y, a);
            }
        }
        // Java: 259
        public void putPixelClip(int x, int y, float r, float g, float b)
        {
            this.r.putPixelClip(x, y, r);
            this.g.putPixelClip(x, y, g);
            this.b.putPixelClip(x, y, b);
        }
        // Java: 265
        public void putPixelClip(int x, int y, float r, float g, float b, float a)
        {
            this.r.putPixelClip(x, y, r);
            this.g.putPixelClip(x, y, g);
            this.b.putPixelClip(x, y, b);
            if (this.a != null)
            {
                this.a.putPixelClip(x, y, a);
            }
        }
        // Java: 274
        public void fill(float @value)
        {
            r.fill(@value);
            g.fill(@value);
            b.fill(@value);
        }
        // Java: 280
        public void fill(float r, float g, float b)
        {
            this.r.fill(r);
            this.g.fill(g);
            this.b.fill(b);
        }
        // Java: 286
        public void fill(float r, float g, float b, float a)
        {
            this.r.fill(r);
            this.g.fill(g);
            this.b.fill(b);
            if (this.a != null)
            {
                this.a.fill(a);
            }
        }
        // Java: 295
        public float findMin()
        {
            float min_r = r.findMin();
            float min_g = g.findMin();
            float min_b = b.findMin();
            return java.lang.Math.min(min_r, java.lang.Math.min(min_g, min_b));
        }
        // Java: 302
        public float findMax()
        {
            float max_r = r.findMax();
            float max_g = g.findMax();
            float max_b = b.findMax();
            return java.lang.Math.max(max_r, java.lang.Math.max(max_g, max_b));
        }
        // Java: 309
        public com.oddlabs.procedural.Layer copy()
        {
            if (a != null)
            {
                return new com.oddlabs.procedural.Layer(r.copy(), g.copy(), b.copy(), a.copy());
            }
            else
            {
                return new com.oddlabs.procedural.Layer(r.copy(), g.copy(), b.copy());
            }
        }
        // Java: 317
        public com.oddlabs.procedural.Layer dynamicRange()
        {
            float min_r = r.findMin();
            float min_g = g.findMin();
            float min_b = b.findMin();
            float max_r = r.findMax();
            float max_g = g.findMax();
            float max_b = b.findMax();
            float min = java.lang.Math.min(min_r, java.lang.Math.min(min_g, min_b));
            float max = java.lang.Math.max(max_r, java.lang.Math.max(max_g, max_b));
            min_r = com.oddlabs.procedural.Tools.interpolateLinear(0, 1, (float)(((float)(min_r - min)) / ((float)(max - min))));
            min_g = com.oddlabs.procedural.Tools.interpolateLinear(0, 1, (float)(((float)(min_g - min)) / ((float)(max - min))));
            min_b = com.oddlabs.procedural.Tools.interpolateLinear(0, 1, (float)(((float)(min_b - min)) / ((float)(max - min))));
            max_r = com.oddlabs.procedural.Tools.interpolateLinear(0, 1, (float)(((float)(max_r - min)) / ((float)(max - min))));
            max_g = com.oddlabs.procedural.Tools.interpolateLinear(0, 1, (float)(((float)(max_g - min)) / ((float)(max - min))));
            max_b = com.oddlabs.procedural.Tools.interpolateLinear(0, 1, (float)(((float)(max_b - min)) / ((float)(max - min))));
            r.dynamicRange(min_r, max_r);
            g.dynamicRange(min_g, max_g);
            b.dynamicRange(min_b, max_b);
            return this;
        }
        // Java: 338
        public com.oddlabs.procedural.Layer dynamicRange(float new_min, float new_max)
        {
            float min_r = r.findMin();
            float min_g = g.findMin();
            float min_b = b.findMin();
            float max_r = r.findMax();
            float max_g = g.findMax();
            float max_b = b.findMax();
            float min = java.lang.Math.min(min_r, java.lang.Math.min(min_g, min_b));
            float max = java.lang.Math.max(max_r, java.lang.Math.max(max_g, max_b));
            min_r = com.oddlabs.procedural.Tools.interpolateLinear(new_min, new_max, (float)(((float)(min_r - min)) / ((float)(max - min))));
            min_g = com.oddlabs.procedural.Tools.interpolateLinear(new_min, new_max, (float)(((float)(min_g - min)) / ((float)(max - min))));
            min_b = com.oddlabs.procedural.Tools.interpolateLinear(new_min, new_max, (float)(((float)(min_b - min)) / ((float)(max - min))));
            max_r = com.oddlabs.procedural.Tools.interpolateLinear(new_min, new_max, (float)(((float)(max_r - min)) / ((float)(max - min))));
            max_g = com.oddlabs.procedural.Tools.interpolateLinear(new_min, new_max, (float)(((float)(max_g - min)) / ((float)(max - min))));
            max_b = com.oddlabs.procedural.Tools.interpolateLinear(new_min, new_max, (float)(((float)(max_b - min)) / ((float)(max - min))));
            r.dynamicRange(min_r, max_r);
            g.dynamicRange(min_g, max_g);
            b.dynamicRange(min_b, max_b);
            return this;
        }
        // Java: 359
        public com.oddlabs.procedural.Layer dynamicRange(float min, float max, float new_min, float new_max)
        {
            r.dynamicRange(min, max, new_min, new_max);
            g.dynamicRange(min, max, new_min, new_max);
            b.dynamicRange(min, max, new_min, new_max);
            return this;
        }
        // Java: 366
        public com.oddlabs.procedural.Layer clip()
        {
            r.clip();
            g.clip();
            b.clip();
            return this;
        }
        // Java: 373
        public com.oddlabs.procedural.Layer crop(int x_lo, int y_lo, int x_hi, int y_hi)
        {
            r = r.crop(x_lo, y_lo, x_hi, y_hi);
            g = g.crop(x_lo, y_lo, x_hi, y_hi);
            b = b.crop(x_lo, y_lo, x_hi, y_hi);
            if (a != null)
            {
                a = a.crop(x_lo, y_lo, x_hi, y_hi);
            }
            width = r.getWidth();
            height = r.getHeight();
            return this;
        }
        // Java: 385
        public com.oddlabs.procedural.Layer cropWrap(int x_lo, int y_lo, int x_hi, int y_hi)
        {
            r = r.cropWrap(x_lo, y_lo, x_hi, y_hi);
            g = g.cropWrap(x_lo, y_lo, x_hi, y_hi);
            b = b.cropWrap(x_lo, y_lo, x_hi, y_hi);
            if (a != null)
            {
                a = a.cropWrap(x_lo, y_lo, x_hi, y_hi);
            }
            width = r.getWidth();
            height = r.getHeight();
            return this;
        }
        // Java: 397
        public com.oddlabs.procedural.Layer tile(int new_width, int new_height)
        {
            r = r.tile(new_width, new_height);
            g = g.tile(new_width, new_height);
            b = b.tile(new_width, new_height);
            if (a != null)
            {
                a = a.tile(new_width, new_height);
            }
            width = r.getWidth();
            height = r.getHeight();
            return this;
        }
        // Java: 409
        public com.oddlabs.procedural.Layer tileDouble()
        {
            r = r.tileDouble();
            g = g.tileDouble();
            b = b.tileDouble();
            if (a != null)
            {
                a = a.tileDouble();
            }
            width = width << 1;
            height = height << 1;
            return this;
        }
        // Java: 421
        public com.oddlabs.procedural.Layer offset(int x_offset, int y_offset)
        {
            r = r.offset(x_offset, y_offset);
            g = g.offset(x_offset, y_offset);
            b = b.offset(x_offset, y_offset);
            if (a != null)
            {
                a = a.offset(x_offset, y_offset);
            }
            return this;
        }
        // Java: 431
        public com.oddlabs.procedural.Layer brightness(float brightness_v)
        {
            r.brightness(brightness_v);
            g.brightness(brightness_v);
            b.brightness(brightness_v);
            return this;
        }
        // Java: 438
        public com.oddlabs.procedural.Layer brightness(float r, float g, float b)
        {
            this.r.brightness(r);
            this.g.brightness(g);
            this.b.brightness(b);
            return this;
        }
        // Java: 445
        public com.oddlabs.procedural.Layer multiply(float factor)
        {
            r.multiply(factor);
            g.multiply(factor);
            b.multiply(factor);
            return this;
        }
        // Java: 452
        public com.oddlabs.procedural.Layer multiply(float r, float g, float b)
        {
            this.r.multiply(r);
            this.g.multiply(g);
            this.b.multiply(b);
            return this;
        }
        // Java: 459
        public com.oddlabs.procedural.Layer multiply(float r, float g, float b, float a)
        {
            this.r.multiply(r);
            this.g.multiply(g);
            this.b.multiply(b);
            if (this.a != null)
            {
                this.a.multiply(a);
            }
            return this;
        }
        // Java: 469
        public com.oddlabs.procedural.Layer add(float add_v)
        {
            r.add(add_v);
            g.add(add_v);
            b.add(add_v);
            return this;
        }
        // Java: 476
        public com.oddlabs.procedural.Layer add(float r, float g, float b)
        {
            this.r.add(r);
            this.g.add(g);
            this.b.add(b);
            return this;
        }
        // Java: 483
        public com.oddlabs.procedural.Layer addClip(float r, float g, float b)
        {
            this.r.addClip(r);
            this.g.addClip(g);
            this.b.addClip(b);
            return this;
        }
        // Java: 490
        public com.oddlabs.procedural.Layer addClip(float r, float g, float b, float a)
        {
            this.r.addClip(r);
            this.g.addClip(g);
            this.b.addClip(b);
            if (this.a != null)
            {
                this.a.addClip(a);
            }
            return this;
        }
        // Java: 500
        public com.oddlabs.procedural.Layer contrast(float contrast_v)
        {
            r.contrast(contrast_v);
            g.contrast(contrast_v);
            b.contrast(contrast_v);
            return this;
        }
        // Java: 507
        public com.oddlabs.procedural.Layer contrast(float r, float g, float b)
        {
            this.r.contrast(r);
            this.g.contrast(g);
            this.b.contrast(b);
            return this;
        }
        // Java: 514
        public com.oddlabs.procedural.Layer gamma(float gamma_v)
        {
            r.gamma(gamma_v);
            g.gamma(gamma_v);
            b.gamma(gamma_v);
            return this;
        }
        // Java: 521
        public com.oddlabs.procedural.Layer gamma(float r, float g, float b)
        {
            this.r.gamma(r);
            this.g.gamma(g);
            this.b.gamma(b);
            return this;
        }
        // Java: 528
        public com.oddlabs.procedural.Layer gamma2()
        {
            r.gamma2();
            g.gamma2();
            b.gamma2();
            return this;
        }
        // Java: 535
        public com.oddlabs.procedural.Layer gamma4()
        {
            r.gamma4();
            g.gamma4();
            b.gamma4();
            return this;
        }
        // Java: 542
        public com.oddlabs.procedural.Layer gamma8()
        {
            r.gamma8();
            g.gamma8();
            b.gamma8();
            return this;
        }
        // Java: 549
        public com.oddlabs.procedural.Layer invert()
        {
            r.invert();
            g.invert();
            b.invert();
            return this;
        }
        // Java: 556
        public com.oddlabs.procedural.Layer threshold(float start, float end)
        {
            r.threshold(start, end);
            g.threshold(start, end);
            b.threshold(start, end);
            return this;
        }
        // Java: 563
        public com.oddlabs.procedural.Layer scaleHalf()
        {
            r = r.scaleHalf();
            g = g.scaleHalf();
            b = b.scaleHalf();
            if (a != null)
            {
                a = a.scaleHalf();
            }
            width = r.getWidth();
            height = r.getHeight();
            return this;
        }
        // Java: 575
        public com.oddlabs.procedural.Layer scale(int new_width, int new_height)
        {
            r = r.scale(new_width, new_height);
            g = g.scale(new_width, new_height);
            b = b.scale(new_width, new_height);
            if (a != null)
            {
                a = a.scale(new_width, new_height);
            }
            width = new_width;
            height = new_height;
            return this;
        }
        // Java: 587
        public com.oddlabs.procedural.Layer scaleCubicNonWrapping(int new_width, int new_height)
        {
            r = r.scaleCubicNonWrapping(new_width, new_height);
            g = g.scaleCubicNonWrapping(new_width, new_height);
            b = b.scaleCubicNonWrapping(new_width, new_height);
            if (a != null)
            {
                a = a.scaleCubicNonWrapping(new_width, new_height);
            }
            width = new_width;
            height = new_height;
            return this;
        }
        // Java: 599
        public com.oddlabs.procedural.Layer scaleCubicWrapping(int new_width, int new_height)
        {
            r = r.scaleCubicWrapping(new_width, new_height);
            g = g.scaleCubicWrapping(new_width, new_height);
            b = b.scaleCubicWrapping(new_width, new_height);
            if (a != null)
            {
                a = a.scaleCubicWrapping(new_width, new_height);
            }
            width = new_width;
            height = new_height;
            return this;
        }
        // Java: 611
        public com.oddlabs.procedural.Layer scaleFast(int new_width, int new_height)
        {
            r = r.scaleFast(new_width, new_height);
            g = g.scaleFast(new_width, new_height);
            b = b.scaleFast(new_width, new_height);
            if (a != null)
            {
                a = a.scaleFast(new_width, new_height);
            }
            width = new_width;
            height = new_height;
            return this;
        }
        // Java: 623
        public com.oddlabs.procedural.Layer rotate(int degrees)
        {
            r = r.rotate(degrees);
            g = g.rotate(degrees);
            b = b.rotate(degrees);
            if (a != null)
            {
                a = a.rotate(degrees);
            }
            width = r.getWidth();
            height = r.getHeight();
            return this;
        }
        // Java: 635
        public com.oddlabs.procedural.Layer shear(float offset_v)
        {
            r = r.shear(offset_v);
            g = g.shear(offset_v);
            b = b.shear(offset_v);
            if (a != null)
            {
                a = a.shear(offset_v);
            }
            return this;
        }
        // Java: 645
        public com.oddlabs.procedural.Layer flipH()
        {
            r = r.flipH();
            g = g.flipH();
            b = b.flipH();
            if (a != null)
            {
                a = a.flipH();
            }
            return this;
        }
        // Java: 655
        public com.oddlabs.procedural.Layer flipV()
        {
            r = r.flipV();
            g = g.flipV();
            b = b.flipV();
            if (a != null)
            {
                a = a.flipV();
            }
            return this;
        }
        // Java: 665
        public com.oddlabs.procedural.Layer smooth(int radius)
        {
            r.smooth(radius);
            g.smooth(radius);
            b.smooth(radius);
            return this;
        }
        // Java: 672
        public com.oddlabs.procedural.Layer sharpen(int radius)
        {
            r.sharpen(radius);
            g.sharpen(radius);
            b.sharpen(radius);
            return this;
        }
        // Java: 680
        public com.oddlabs.procedural.Layer convolution(float[][] filter, float divisor, float offset_v)
        {
            r.convolution(filter, divisor, offset_v);
            g.convolution(filter, divisor, offset_v);
            b.convolution(filter, divisor, offset_v);
            return this;
        }
        // Java: 687
        public com.oddlabs.procedural.Layer grow(float r, float g, float b, int radius)
        {
            this.r.grow(r, radius);
            this.g.grow(g, radius);
            this.b.grow(b, radius);
            return this;
        }
        // Java: 694
        public com.oddlabs.procedural.Layer bump(com.oddlabs.procedural.Channel bumpmap, float lx, float ly, float shadow, float light_r, float light_g, float light_b, float ambient_r, float ambient_g, float ambient_b)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float nx = (float)(bumpmap.getPixelWrap(x + 1, y) - bumpmap.getPixelWrap(x - 1, y));
                    float ny = (float)(bumpmap.getPixelWrap(x, y + 1) - bumpmap.getPixelWrap(x, y - 1));
                    float brightness_v = (float)((float)(nx * lx) + (float)(ny * ly));
                    if (brightness_v >= 0)
                    {
                        putPixelClip(x, y, (float)(((float)(r.getPixel(x, y) + (float)(brightness_v * light_r))) * ((float)((float)((float)(bumpmap.getPixel(x, y) * shadow) + 1) - shadow))), (float)(((float)(g.getPixel(x, y) + (float)(brightness_v * light_g))) * ((float)((float)((float)(bumpmap.getPixel(x, y) * shadow) + 1) - shadow))), (float)(((float)(b.getPixel(x, y) + (float)(brightness_v * light_b))) * ((float)((float)((float)(bumpmap.getPixel(x, y) * shadow) + 1) - shadow))));
                    }
                    else
                    {
                        putPixelClip(x, y, (float)(((float)(r.getPixel(x, y) + (float)(brightness_v * ((float)(1 - ambient_r))))) * ((float)((float)((float)(bumpmap.getPixel(x, y) * shadow) + 1) - shadow))), (float)(((float)(g.getPixel(x, y) + (float)(brightness_v * ((float)(1 - ambient_g))))) * ((float)((float)((float)(bumpmap.getPixel(x, y) * shadow) + 1) - shadow))), (float)(((float)(b.getPixel(x, y) + (float)(brightness_v * ((float)(1 - ambient_b))))) * ((float)((float)((float)(bumpmap.getPixel(x, y) * shadow) + 1) - shadow))));
                    }
                }
            }
            return this;
        }
        // Java: 720
        public com.oddlabs.procedural.Layer bumpFast(com.oddlabs.procedural.Channel bumpmap, float lx, float light, float ambient)
        {
            ambient = (float)(1.0f - ambient);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float brightness_v = (float)(lx * ((float)(bumpmap.getPixelWrap(x + 1, y) - bumpmap.getPixelWrap(x - 1, y))));
                    if (brightness_v >= 0)
                    {
                        brightness_v = (float)(brightness_v * light);
                        putPixel(x, y, (float)(r.getPixel(x, y) + brightness_v), (float)(g.getPixel(x, y) + brightness_v), (float)(b.getPixel(x, y) + brightness_v));
                    }
                    else
                    {
                        brightness_v = (float)(brightness_v * ambient);
                        putPixel(x, y, (float)(r.getPixel(x, y) + brightness_v), (float)(g.getPixel(x, y) + brightness_v), (float)(b.getPixel(x, y) + brightness_v));
                    }
                }
            }
            return this;
        }
        // Java: 742
        public com.oddlabs.procedural.Layer bumpSpecular(com.oddlabs.procedural.Channel bumpmap, float lx, float ly, float lz, float shadow, float light_r, float light_g, float light_b, int specular)
        {
            float lnorm = ((float)(java.lang.Math.sqrt((float)((float)((float)(lx * lx) + (float)(ly * ly)) + (float)(lz * lz)))));
            float nz = (float)(4 * ((float)(1.0f / java.lang.Math.min(width, height))));
            float nzlz = (float)(nz * lz);
            float nz2 = (float)(nz * nz);
            int power = 2 << specular;
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
                        highlight = ((float)(java.lang.StrictMath.pow(costheta, power)));
                    }
                    else
                    {
                        highlight = 0;
                    }
                    putPixelClip(x, y, (float)(((float)(r.getPixel(x, y) + (float)(highlight * light_r))) * ((float)((float)((float)(bumpmap.getPixel(x, y) * shadow) + 1) - shadow))), (float)(((float)(g.getPixel(x, y) + (float)(highlight * light_g))) * ((float)((float)((float)(bumpmap.getPixel(x, y) * shadow) + 1) - shadow))), (float)(((float)(b.getPixel(x, y) + (float)(highlight * light_b))) * ((float)((float)((float)(bumpmap.getPixel(x, y) * shadow) + 1) - shadow))));
                }
            }
            return this;
        }
        // Java: 771
        public com.oddlabs.procedural.Layer toHSV()
        {
            float min = 0;
            float max = 0;
            float delta = 0;
            float h_val = 0;
            float s_val = 0;
            float v_val = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float r_val = r.getPixel(x, y);
                    float g_val = g.getPixel(x, y);
                    float b_val = b.getPixel(x, y);
                    min = java.lang.Math.min(r_val, java.lang.Math.min(g_val, b_val));
                    max = java.lang.Math.max(r_val, java.lang.Math.max(g_val, b_val));
                    v_val = max;
                    delta = (float)(max - min);
                    if (max != 0)
                    {
                        s_val = (float)(delta / max);
                    }
                    else
                    {
                        s_val = 0;
                    }
                    if (max == r_val)
                    {
                        h_val = (float)(((float)(g_val - b_val)) / delta);
                    }
                    if (max == g_val)
                    {
                        h_val = (float)(2 + (float)(((float)(b_val - r_val)) / delta));
                    }
                    if (max == b_val)
                    {
                        h_val = (float)(4 + (float)(((float)(r_val - g_val)) / delta));
                    }
                    h_val = (float)(h_val / 6);
                    if (h_val < 0)
                    {
                        h_val = (float)(h_val + 1);
                    }
                    r.putPixel(x, y, h_val);
                    g.putPixel(x, y, s_val);
                    b.putPixel(x, y, v_val);
                }
            }
            return this;
        }
        // Java: 819
        public com.oddlabs.procedural.Layer toRGB()
        {
            int i = default;
            float f = default;
            float p = default;
            float q = default;
            float t = default;
            float r_val = 0;
            float g_val = 0;
            float b_val = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float h_val = r.getPixel(x, y);
                    float s_val = g.getPixel(x, y);
                    float v_val = b.getPixel(x, y);
                    if (s_val == 0)
                    {
                        r.putPixel(x, y, v_val);
                        g.putPixel(x, y, v_val);
                        b.putPixel(x, y, v_val);
                        continue;
                    }
                    h_val = (float)(h_val * 6);
                    i = java.lang.JC.f2i(h_val);
                    f = (float)(h_val - i);
                    p = (float)(v_val * ((float)(1 - s_val)));
                    q = (float)(v_val * ((float)(1 - (float)(s_val * f))));
                    t = (float)(v_val * ((float)(1 - (float)(s_val * ((float)(1 - f))))));
                    switch ((i))
                    {
                        case 0:
                        {
                            r_val = v_val;
                            g_val = t;
                            b_val = p;
                            break;
                        }
                        case 1:
                        {
                            r_val = q;
                            g_val = v_val;
                            b_val = p;
                            break;
                        }
                        case 2:
                        {
                            r_val = p;
                            g_val = v_val;
                            b_val = t;
                            break;
                        }
                        case 3:
                        {
                            r_val = p;
                            g_val = q;
                            b_val = v_val;
                            break;
                        }
                        case 4:
                        {
                            r_val = t;
                            g_val = p;
                            b_val = v_val;
                            break;
                        }
                        case 5:
                        {
                            r_val = v_val;
                            g_val = p;
                            b_val = q;
                            break;
                        }
                        case 6:
                        {
                            r_val = v_val;
                            g_val = p;
                            b_val = q;
                            break;
                        }
                        default:
                        {
                            break;
                        }
                    }
                    r.putPixel(x, y, r_val);
                    g.putPixel(x, y, g_val);
                    b.putPixel(x, y, b_val);
                }
            }
            return this;
        }
        // Java: 897
        public com.oddlabs.procedural.Layer saturation(float saturation_v)
        {
            toHSV();
            float s_val = default;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    s_val = (float)(g.getPixel(x, y) * saturation_v);
                    if (s_val < 0)
                    {
                        s_val = 0;
                    }
                    else
                    {
                        if (s_val > 1)
                        {
                            s_val = 1;
                        }
                    }
                    g.putPixel(x, y, s_val);
                }
            }
            return toRGB();
        }
        // Java: 916
        public com.oddlabs.procedural.Layer hue(float hue_v)
        {
            toHSV();
            float h_val = default;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    h_val = (float)(r.getPixel(x, y) + hue_v);
                    if (h_val < 0)
                    {
                        h_val = (float)(h_val + 1);
                    }
                    else
                    {
                        if (h_val > 1)
                        {
                            h_val = (float)(h_val - 1);
                        }
                    }
                    r.putPixel(x, y, h_val);
                }
            }
            return toRGB();
        }
        // Java: 935
        public com.oddlabs.procedural.Layer hueRotation(float min, float max, float new_min, float new_max)
        {
            toHSV();
            r.dynamicRange(min, max, new_min, new_max);
            return toRGB();
        }
        // Java: 941
        public com.oddlabs.procedural.Layer lineart()
        {
            r.lineart();
            g.lineart();
            b.lineart();
            return this;
        }
        // Java: 948
        public com.oddlabs.procedural.Layer place(com.oddlabs.procedural.Layer sprite, int x_offset, int y_offset)
        {
            r.place(sprite.r, x_offset, y_offset);
            g.place(sprite.g, x_offset, y_offset);
            b.place(sprite.b, x_offset, y_offset);
            if (a != null && sprite.a != null)
            {
                a.place(sprite.a, x_offset, y_offset);
            }
            return this;
        }
        // Java: 957
        public com.oddlabs.procedural.Layer abs()
        {
            r.abs();
            g.abs();
            b.abs();
            return this;
        }
        // Java: 964
        public com.oddlabs.procedural.Layer layerBlend(com.oddlabs.procedural.Layer layer, float alpha)
        {
            r.channelBlend(layer.r, alpha);
            g.channelBlend(layer.g, alpha);
            b.channelBlend(layer.b, alpha);
            return this;
        }
        // Java: 971
        public com.oddlabs.procedural.Layer layerBlend(com.oddlabs.procedural.Layer rgb, com.oddlabs.procedural.Channel a)
        {
            return layerBlend(new com.oddlabs.procedural.Layer(rgb.r, rgb.g, rgb.b, a));
        }
        // Java: 975
        public com.oddlabs.procedural.Layer layerBlend(com.oddlabs.procedural.Layer layer)
        {
            if (a == null)
            {
                r.channelBlend(layer.r, layer.a);
                g.channelBlend(layer.g, layer.a);
                b.channelBlend(layer.b, layer.a);
            }
            else
            {
                float alpha = default;
                float alpha_inv = default;
                float r1 = default;
                float g1 = default;
                float b1 = default;
                float a2 = default;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (a.getPixel(x, y) == 0)
                        {
                            putPixel(x, y, layer.r.getPixel(x, y), layer.g.getPixel(x, y), layer.b.getPixel(x, y), layer.a.getPixel(x, y));
                        }
                        else if (layer.a.getPixel(x, y) == 0)
                        {
                        }
                        else
                        {
                            alpha = (float)(1.0f - (float)(((float)(1.0f - a.getPixel(x, y))) * ((float)(1.0f - layer.a.getPixel(x, y)))));
                            alpha_inv = (float)(1.0f / alpha);
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
        // Java: 1013
        public com.oddlabs.procedural.Layer layerAdd(com.oddlabs.procedural.Layer layer)
        {
            r.channelAdd(layer.r);
            g.channelAdd(layer.g);
            b.channelAdd(layer.b);
            return this;
        }
        // Java: 1020
        public com.oddlabs.procedural.Layer layerSubtract(com.oddlabs.procedural.Layer layer)
        {
            r.channelSubtract(layer.r);
            g.channelSubtract(layer.g);
            b.channelSubtract(layer.b);
            return this;
        }
        // Java: 1027
        public com.oddlabs.procedural.Layer layerAverage(com.oddlabs.procedural.Layer layer)
        {
            r.channelAverage(layer.r);
            g.channelAverage(layer.g);
            b.channelAverage(layer.b);
            return this;
        }
        // Java: 1034
        public com.oddlabs.procedural.Layer layerMultiply(com.oddlabs.procedural.Layer layer)
        {
            r.channelMultiply(layer.r);
            g.channelMultiply(layer.g);
            b.channelMultiply(layer.b);
            return this;
        }
        // Java: 1041
        public com.oddlabs.procedural.Layer layerDifference(com.oddlabs.procedural.Layer layer)
        {
            r.channelDifference(layer.r);
            g.channelDifference(layer.g);
            b.channelDifference(layer.b);
            return this;
        }
        // Java: 1048
        public com.oddlabs.procedural.Layer layerDarkest(com.oddlabs.procedural.Layer layer)
        {
            r.channelDarkest(layer.r);
            g.channelDarkest(layer.g);
            b.channelDarkest(layer.b);
            return this;
        }
        // Java: 1055
        public com.oddlabs.procedural.Layer layerBrightest(com.oddlabs.procedural.Layer layer)
        {
            r.channelBrightest(layer.r);
            g.channelBrightest(layer.g);
            b.channelBrightest(layer.b);
            return this;
        }
        static Layer()
        {
            Layer.logger = java.util.logging.Logger.getLogger(typeof(com.oddlabs.procedural.Layer).getSimpleName());
        }
        public Layer(int width, int height) : base()
        {
            if (width <= 0 || height <= 0)
            {
                throw new System.ArgumentException("Width and height must be positive.");
            }
            this.width = width;
            this.height = height;
            com.oddlabs.procedural.Channel empty = new com.oddlabs.procedural.Channel(width, height);
            empty.fill(1.0f);
            this.r = empty;
            this.g = empty.copy();
            this.b = empty.copy();
            this.a = null;
        }
        public Layer(com.oddlabs.procedural.Channel r, com.oddlabs.procedural.Channel g, com.oddlabs.procedural.Channel b, com.oddlabs.procedural.Channel a) : base()
        {
            java.util.Objects.requireNonNull(r, "Red channel cannot be null.");
            java.util.Objects.requireNonNull(g, "Green channel cannot be null.");
            java.util.Objects.requireNonNull(b, "Blue channel cannot be null.");
            this.width = r.getWidth();
            this.height = r.getHeight();
            if (g.getWidth() != width || b.getWidth() != width || g.getHeight() != height || b.getHeight() != height || (a != null && (a.getWidth() != width || a.getHeight() != height)))
            {
                throw new System.ArgumentException("All channels must have the same dimensions.");
            }
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }
        public Layer(com.oddlabs.procedural.Channel r, com.oddlabs.procedural.Channel g, com.oddlabs.procedural.Channel b) : this(r, g, b, null)
        {
        }
        public Layer(com.oddlabs.procedural.Layer rgb, com.oddlabs.procedural.Channel a) : this(rgb.r, rgb.g, rgb.b, a)
        {
        }
    }
}
