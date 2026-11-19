// Translated from GLImage.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.resource
{
    // Java: 13
    public abstract class GLImage : java.lang.Object
    {
        public int width;
        public int height;
        public int type;
        public int format;
        public java.nio.ByteBuffer pixel_data;
        // Java: 20
        public int getWidth()
        {
            return width;
        }
        // Java: 24
        public int getHeight()
        {
            return height;
        }
        // Java: 28
        public int getGLType()
        {
            return type;
        }
        // Java: 32
        public abstract int getPixel(int x, int y);
        // Java: 34
        public abstract void putPixel(int x, int y, int pixel);
        // Java: 46
        public int determineType(int format)
        {
            {
                switch ((format))
                {
                    case 6408:
                    case 32993:
                    case 34030:
                    {
                        return org.lwjgl.opengl.GL11.GL_UNSIGNED_BYTE;
                    }
                    case 6409:
                    case 34026:
                    case 6406:
                    case 34025:
                    case 6403:
                    {
                        return org.lwjgl.opengl.GL11.GL_UNSIGNED_BYTE;
                    }
                    default:
                    {
                        throw new System.ArgumentException("Invalid format: " + (format));
                    }
                }
                throw new System.InvalidOperationException("MatchException");
            }
        }
        // Java: 61
        public int getGLFormat()
        {
            return format;
        }
        // Java: 65
        public abstract com.oddlabs.tt.resource.GLImage createImage(int width, int height, int format);
        // Java: 67
        public abstract com.oddlabs.tt.resource.GLImage createFromLayer(com.oddlabs.procedural.Layer layer, int format);
        // Java: 69
        public com.oddlabs.tt.resource.GLImage[] createMipMaps()
        {
            return buildMipMaps(10000, 1.0f, false, false);
        }
        // Java: 73
        public com.oddlabs.tt.resource.GLImage[] buildMipMaps(int base_fadeout_level, float fadeout_factor, bool wrapping, bool max_alpha)
        {
            int max = java.lang.Math.max(height, width);
            int max_level = java.lang.JC.d2i((java.lang.StrictMath.log(max) / java.lang.StrictMath.log(2)));
            com.oddlabs.tt.resource.GLImage[] result = new com.oddlabs.tt.resource.GLImage[max_level + 1];
            result[0] = this;
            for (int i = 1; i < result.Length; i++)
            {
                int current_width = java.lang.Math.max(width >> i, 1);
                int current_height = java.lang.Math.max(height >> i, 1);
                result[i] = createImage(current_width, current_height, format);
                com.oddlabs.tt.resource.GLImage prev = result[i - 1];
                int width_div = prev.getWidth() / current_width;
                int height_div = prev.getHeight() / current_height;
                for (int y = 0; y < current_height; y++)
                {
                    for (int x = 0; x < current_width; x++)
                    {
                        result[i].putPixel(x, y, com.oddlabs.tt.resource.GLImage.averagePixel(prev, x * width_div, y * height_div, height_div, width_div, base_fadeout_level, fadeout_factor, i, max_alpha));
                    }
                }
            }
            return result;
        }
        // Java: 100
        public static void applyFadeout(com.oddlabs.tt.resource.GLImage image, float factor)
        {
            for (int y = 0; y < image.getHeight(); y++)
            {
                for (int x = 0; x < image.getWidth(); x++)
                {
                    int pixel = image.getPixel(x, y);
                    int a = (java.lang.JC.ushr(pixel, 24));
                    int r = (java.lang.JC.ushr(pixel, 16)) & 255;
                    int g = (java.lang.JC.ushr(pixel, 8)) & 255;
                    int b = pixel & 255;
                    a = java.lang.JC.f2i(((float)(a * factor)));
                    r = java.lang.JC.f2i(((float)(r * factor)));
                    g = java.lang.JC.f2i(((float)(g * factor)));
                    b = java.lang.JC.f2i(((float)(b * factor)));
                    image.putPixel(x, y, (a << 24) | (r << 16) | (g << 8) | b);
                }
            }
        }
        // Java: 132
        public static void updateMipMapsArea(com.oddlabs.tt.resource.GLImage[] mipmaps, int base_fadeout_level, float fadeout_factor, int start_x, int start_y, int width, int height, bool max_alpha)
        {
            for (int i = 1; i < mipmaps.Length; i++)
            {
                int height_div = mipmaps[i - 1].getHeight() / mipmaps[i].getHeight();
                int width_div = mipmaps[i - 1].getWidth() / mipmaps[i].getWidth();
                start_x /= width_div;
                start_y /= height_div;
                width = java.lang.JC.d2i(java.lang.Math.ceil((float)(((float)(width)) / width_div)));
                height = java.lang.JC.d2i(java.lang.Math.ceil((float)(((float)(height)) / height_div)));
                for (int y = start_y; y < start_y + height; y++)
                {
                    for (int x = start_x; x < start_x + width; x++)
                    {
                        mipmaps[i].putPixel(x, y, com.oddlabs.tt.resource.GLImage.averagePixel(mipmaps[i - 1], width_div * x, height_div * y, height_div, width_div, base_fadeout_level, fadeout_factor, i, max_alpha));
                    }
                }
            }
        }
        // Java: 150
        public static void blendMipMapsArea(com.oddlabs.tt.resource.GLImage[] dest_mipmaps, com.oddlabs.tt.resource.GLImage[] source_mipmaps, int base_fadeout_level, float fadeout_factor, int start_x, int start_y, int width, int height)
        {
            int mip_map_level = 0;
            while (source_mipmaps[0].getWidth() != dest_mipmaps[mip_map_level].getWidth() && source_mipmaps[0].getHeight() != dest_mipmaps[mip_map_level].getHeight())
            {
                mip_map_level++;
            }
            for (int i = 1; i < dest_mipmaps.Length; i++)
            {
                int height_div = dest_mipmaps[i - 1].getHeight() / dest_mipmaps[i].getHeight();
                int width_div = dest_mipmaps[i - 1].getWidth() / dest_mipmaps[i].getWidth();
                start_x /= width_div;
                start_y /= height_div;
                width = java.lang.JC.d2i(java.lang.Math.ceil((float)(((float)(width)) / width_div)));
                height = java.lang.JC.d2i(java.lang.Math.ceil((float)(((float)(height)) / height_div)));
                if (i >= base_fadeout_level)
                {
                    if (i >= mip_map_level)
                    {
                        dest_mipmaps[i].drawImageBlended(source_mipmaps[i - mip_map_level], start_x, start_y, start_x, start_y, width, height, (float)(1.0f - fadeout_factor));
                    }
                    fadeout_factor = (float)(fadeout_factor * fadeout_factor);
                }
            }
        }
        // Java: 189
        public static int averagePixel(com.oddlabs.tt.resource.GLImage last_img, int x, int y, int height_div, int width_div, int base_fadeout_level, float fadeout_factor, int current_level, bool max_alpha)
        {
            float inv_num_averaged = (float)(1.0f / (height_div * width_div));
            int a_acc = 0;
            long r_acc = 0;
            long g_acc = 0;
            long b_acc = 0;
            long weight_acc = 0;
            for (int offset_y = 0; offset_y < height_div; offset_y++)
            {
                for (int offset_x = 0; offset_x < width_div; offset_x++)
                {
                    int pixel = last_img.getPixel(x + offset_x, y + offset_y);
                    int a = (java.lang.JC.ushr(pixel, 24));
                    int r = (java.lang.JC.ushr(pixel, 16)) & 255;
                    int g = (java.lang.JC.ushr(pixel, 8)) & 255;
                    int b = pixel & 255;
                    if (a > 0)
                    {
                        r_acc += ((long)(r)) * a;
                        g_acc += ((long)(g)) * a;
                        b_acc += ((long)(b)) * a;
                        weight_acc += a;
                    }
                    if (max_alpha)
                    {
                        a_acc = java.lang.Math.max(a_acc, a);
                    }
                    else
                    {
                        a_acc += a;
                    }
                }
            }
            int r_avg = default;
            int g_avg = default;
            int b_avg = default;
            if (weight_acc > 0)
            {
                r_avg = unchecked((int)((r_acc / weight_acc)));
                g_avg = unchecked((int)((g_acc / weight_acc)));
                b_avg = unchecked((int)((b_acc / weight_acc)));
            }
            else
            {
                r_avg = 0;
                g_avg = 0;
                b_avg = 0;
            }
            if (current_level >= base_fadeout_level)
            {
                a_acc = java.lang.JC.f2i(((float)(a_acc * fadeout_factor)));
                r_avg = java.lang.JC.f2i(((float)(r_avg * fadeout_factor)));
                g_avg = java.lang.JC.f2i(((float)(g_avg * fadeout_factor)));
                b_avg = java.lang.JC.f2i(((float)(b_avg * fadeout_factor)));
            }
            if (max_alpha)
            {
                if (a_acc > 128)
                {
                    a_acc = 255;
                }
            }
            else
            {
                a_acc = java.lang.JC.f2i(((float)(a_acc * inv_num_averaged)));
            }
            return (a_acc << 24) | (r_avg << 16) | (g_avg << 8) | b_avg;
        }
        // Java: 275
        public static int averagePixelThreshold(com.oddlabs.tt.resource.GLImage last_img, int x, int y, int height_div, int width_div, int base_fadeout_level, float fadeout_factor, int current_level, bool max_alpha)
        {
            float inv_num_averaged = (float)(1.0f / (height_div * width_div));
            int col1 = 0;
            int col2 = 0;
            int col3 = 0;
            int col4 = 0;
            for (int offset_y = 0; offset_y < height_div; offset_y++)
            {
                for (int offset_x = 0; offset_x < width_div; offset_x++)
                {
                    int pixel = last_img.getPixel(x + offset_x, y + offset_y);
                    col1 += (java.lang.JC.ushr(pixel, 24));
                    col2 += (java.lang.JC.ushr(pixel, 16)) & 255;
                    col3 += (java.lang.JC.ushr(pixel, 8)) & 255;
                    col4 += pixel & 255;
                }
            }
            if (current_level >= base_fadeout_level)
            {
                col1 = java.lang.JC.f2i(((float)(col1 * fadeout_factor)));
                col2 = java.lang.JC.f2i(((float)(col2 * fadeout_factor)));
                col3 = java.lang.JC.f2i(((float)(col3 * fadeout_factor)));
                col4 = java.lang.JC.f2i(((float)(col4 * fadeout_factor)));
            }
            col1 = java.lang.JC.f2i(((float)(col1 * inv_num_averaged)));
            col2 = java.lang.JC.f2i(((float)(col2 * inv_num_averaged)));
            col3 = java.lang.JC.f2i(((float)(col3 * inv_num_averaged)));
            col4 = java.lang.JC.f2i(((float)(col4 * inv_num_averaged)));
            if (max_alpha)
            {
                if (col1 >= 128)
                {
                    col1 = 255;
                }
            }
            return (col1 << 24) + (col2 << 16) + (col3 << 8) + col4;
        }
        // Java: 311
        public void clearAll(int color)
        {
            clear(0, 0, width, height, color);
        }
        // Java: 318
        public void clear(int x, int y, int width, int height, int color)
        {
            int yy = y;
            for (; yy < y + height; yy++)
            {
                for (int xx = x; xx < x + width; xx++)
                {
                    putPixel(yy, xx, color);
                }
            }
        }
        // Java: 327
        public void drawImageBlended(com.oddlabs.tt.resource.GLImage img, int dx, int dy, int sx, int sy, int w, int h, float alpha_factor)
        {
            int spixel = default;
            int dpixel = default;
            int sr = default;
            int sg = default;
            int sb = default;
            int sa = default;
            int sa_inverse = default;
            int dr = default;
            int dg = default;
            int db = default;
            int da = default;
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    spixel = img.getPixel(x + sx, y + sy);
                    sa = (java.lang.JC.ushr(spixel, 24));
                    if (alpha_factor != 1.0f)
                    {
                        sa = java.lang.Math.round((float)(sa * alpha_factor)) & 255;
                    }
                    if (sa == 0)
                    {
                        continue;
                    }
                    if (sa == 255)
                    {
                        putPixel(x + dx, y + dy, spixel);
                        continue;
                    }
                    sa_inverse = 255 - sa;
                    dpixel = getPixel(x + dx, y + dy);
                    sr = java.lang.JC.ushr(spixel, 16) & 255;
                    sg = java.lang.JC.ushr(spixel, 8) & 255;
                    sb = spixel & 255;
                    dr = java.lang.JC.ushr(dpixel, 16) & 255;
                    dg = java.lang.JC.ushr(dpixel, 8) & 255;
                    db = dpixel & 255;
                    da = java.lang.JC.ushr(dpixel, 24);
                    putPixel(x + dx, y + dy, (((sa * sa + da * sa_inverse) / 255) << 24) + (((sr * sa + dr * sa_inverse) / 255) << 16) + (((sg * sa + dg * sa_inverse) / 255) << 8) + ((sb * sa + db * sa_inverse) / 255));
                }
            }
        }
        // Java: 372
        public abstract int getPixelSize();
        // Java: 374
        public java.nio.ByteBuffer getPixels()
        {
            return pixel_data;
        }
        // Java: 378
        public void drawImage(com.oddlabs.tt.resource.GLImage img, int dx, int dy, int sx, int sy, int w, int h)
        {
            int pixel_size = getPixelSize();
            java.nio.ByteBuffer pixels = getPixels();
            java.nio.ByteBuffer other_pixels = img.getPixels();
            int byte_width = w * pixel_size;
            for (int i = 0; i < h; i++)
            {
                int other_pos = ((sy + i) * img.getWidth() + sx) * pixel_size;
                int pos = ((dy + i) * getWidth() + dx) * pixel_size;
                other_pixels.position(other_pos);
                other_pixels.limit(other_pos + byte_width);
                pixels.position(pos);
                pixels.limit(pos + byte_width);
                pixels.put(other_pixels);
                pixels.clear();
                other_pixels.clear();
            }
        }
        // Java: 399
        public virtual com.oddlabs.procedural.Layer toLayer()
        {
            int width = getWidth();
            int height = getHeight();
            com.oddlabs.procedural.Channel r = new com.oddlabs.procedural.Channel(width, height);
            com.oddlabs.procedural.Channel g = new com.oddlabs.procedural.Channel(width, height);
            com.oddlabs.procedural.Channel b = new com.oddlabs.procedural.Channel(width, height);
            com.oddlabs.procedural.Channel a = new com.oddlabs.procedural.Channel(width, height);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int pixel = getPixel(x, y);
                    a.putPixel(x, y, (float)((java.lang.JC.ushr(pixel, 24)) / 255.0f));
                    b.putPixel(x, y, (float)(((pixel >> 16) & 255) / 255.0f));
                    g.putPixel(x, y, (float)(((pixel >> 8) & 255) / 255.0f));
                    r.putPixel(x, y, (float)((pixel & 255) / 255.0f));
                }
            }
            return new com.oddlabs.procedural.Layer(r, g, b, a);
        }
        // Java: 419
        public void saveAsPNG(string filename)
        {
            toLayer().saveAsPNG(filename);
        }
        // Java: 423
        public void saveAsBMP(string filename)
        {
            com.oddlabs.tt.util.Utils.saveAsBMP(filename, getPixels(), getWidth(), getHeight());
        }
        public GLImage(int width, int height, java.nio.ByteBuffer pixel_data, int format) : base()
        {
            this.width = width;
            this.height = height;
            this.pixel_data = pixel_data;
            this.format = format;
            this.type = determineType(format);
        }
    }
}
