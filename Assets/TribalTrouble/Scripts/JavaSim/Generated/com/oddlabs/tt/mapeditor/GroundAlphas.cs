// Translated from GroundAlphas.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.mapeditor
{
    // Java: 26
    public class GroundAlphas : java.lang.Object
    {
        public const float SEABOTTOM_DEPTH_OFFSET_METERS = 3.0f;
        public const float UNDERWATER_SHADING = 0.3f;
        public const float RANGE_TOLERANCE = 0.001953125f;
        public static float LIGHT_THRESHOLD;
        public const int MARGIN = 8;
        public const int NUM_ALPHAS = 7;
        public float[][] heights;
        public int size;
        public com.oddlabs.tt.procedural.Landscape.TerrainType terrain;
        public float height_scale;
        public float access_threshold;
        public float build_threshold;
        public float vegetation_amount;
        public int rel_radius;
        public float light_nz;
        public com.oddlabs.procedural.Channel grass_noise;
        public float[][] supply_highlight;
        public float[][] supply_shadow;
        public int shadow_size;
        public com.oddlabs.procedural.Channel stamp;
        public float[][] rel_intensity;
        public float[][] light;
        public float max_height;
        public com.oddlabs.tt.mapeditor.GroundAlphas.Ranges ranges;
        // Java: 65
        public class Ranges : java.lang.Object
        {
            public float rel_min() { return rel_min_f; }
            public float rel_max() { return rel_max_f; }
            public float highlight_min() { return highlight_min_f; }
            public float highlight_max() { return highlight_max_f; }
            public float shadow_min() { return shadow_min_f; }
            public float shadow_max() { return shadow_max_f; }
            public Ranges(float rel_min, float rel_max, float highlight_min, float highlight_max, float shadow_min, float shadow_max)
            {
                this.rel_min_f = rel_min;
                this.rel_max_f = rel_max;
                this.highlight_min_f = highlight_min;
                this.highlight_max_f = highlight_max;
                this.shadow_min_f = shadow_min;
                this.shadow_max_f = shadow_max;
            }
            public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is Ranges r)) return false; return true && java.lang.JO.equals(rel_min_f, r.rel_min_f) && java.lang.JO.equals(rel_max_f, r.rel_max_f) && java.lang.JO.equals(highlight_min_f, r.highlight_min_f) && java.lang.JO.equals(highlight_max_f, r.highlight_max_f) && java.lang.JO.equals(shadow_min_f, r.shadow_min_f) && java.lang.JO.equals(shadow_max_f, r.shadow_max_f); }
            public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(rel_min_f); h = 31 * h + java.lang.JO.hashCode(rel_max_f); h = 31 * h + java.lang.JO.hashCode(highlight_min_f); h = 31 * h + java.lang.JO.hashCode(highlight_max_f); h = 31 * h + java.lang.JO.hashCode(shadow_min_f); h = 31 * h + java.lang.JO.hashCode(shadow_max_f); } return h; }
            public override string toString() { return "Ranges[" + "rel_min=" + java.lang.JO.toString(rel_min_f) + ", rel_max=" + java.lang.JO.toString(rel_max_f) + ", highlight_min=" + java.lang.JO.toString(highlight_min_f) + ", highlight_max=" + java.lang.JO.toString(highlight_max_f) + ", shadow_min=" + java.lang.JO.toString(shadow_min_f) + ", shadow_max=" + java.lang.JO.toString(shadow_max_f) + "]"; }
            public float rel_min_f;
            public float rel_max_f;
            public float highlight_min_f;
            public float highlight_max_f;
            public float shadow_min_f;
            public float shadow_max_f;
        }
        // Java: 104
        public int getSize()
        {
            return size;
        }
        // Java: 109
        public int getSupplyShadowReach()
        {
            return shadow_size;
        }
        // Java: 117
        public void restampSupplies(java.util.List trees, java.util.List palm_trees, java.util.List shaded_supplies, int x0, int y0, int x1, int y1)
        {
            x0 = java.lang.Math.max(0, x0);
            y0 = java.lang.Math.max(0, y0);
            x1 = java.lang.Math.min(size - 1, x1);
            y1 = java.lang.Math.min(size - 1, y1);
            for (int y = y0; y <= y1; y++)
            {
                java.util.Arrays.fill(supply_highlight[y], x0, x1 + 1, 1.0f);
                java.util.Arrays.fill(supply_shadow[y], x0, x1 + 1, 0.0f);
            }
            stampSupplies(trees, palm_trees, shaded_supplies, x0, y0, x1, y1);
        }
        // Java: 130
        public void stampSupplies(java.util.List trees, java.util.List palm_trees, java.util.List shaded_supplies, int x0, int y0, int x1, int y1)
        {
            stampTrees(trees, 0.33f, x0, y0, x1, y1);
            stampTrees(palm_trees, 0.25f, x0, y0, x1, y1);
            foreach (int[] supply in java.lang.JIter.of(shaded_supplies))
            {
                if (supply[0] >= x0 && supply[0] <= x1 && supply[1] >= y0 && supply[1] <= y1)
                {
                    supply_shadow[supply[1]][supply[0]] = java.lang.Math.max(supply_shadow[supply[1]][supply[0]], 0.5f);
                }
            }
        }
        // Java: 143
        public void stampTrees(java.util.List positions, float strength, int x0, int y0, int x1, int y1)
        {
            foreach (int[] tree in java.lang.JIter.of(positions))
            {
                int sx = tree[0] - shadow_size + 1;
                int sy = tree[1] - shadow_size + 1;
                for (int y = 0; y < stamp.getHeight(); y++)
                {
                    for (int x = 0; x < stamp.getWidth(); x++)
                    {
                        int mx = java.lang.Math.floorMod(sx + x, size);
                        int my = java.lang.Math.floorMod(sy + y, size);
                        if (mx < x0 || mx > x1 || my < y0 || my > y1)
                        {
                            continue;
                        }
                        float alpha = stamp.getPixel(x, y);
                        supply_highlight[my][mx] *= (float)(1.0f - alpha);
                        supply_shadow[my][mx] = java.lang.Math.max(supply_shadow[my][mx], (float)(alpha * strength));
                    }
                }
            }
        }
        // Java: 161
        public float normalized(int x, int y)
        {
            return (float)(heights[y][x] / height_scale);
        }
        // Java: 165
        public float normalizedWrap(int x, int y)
        {
            return (float)(heights[java.lang.Math.floorMod(y, size)][java.lang.Math.floorMod(x, size)] / height_scale);
        }
        // Java: 172
        public void heightsChanged(int x0, int y0, int x1, int y1)
        {
            refreshCaches(x0 - rel_radius, y0 - rel_radius, x1 + rel_radius, y1 + rel_radius);
            for (int y = java.lang.Math.max(0, y0); y <= java.lang.Math.min(size - 1, y1); y++)
            {
                for (int x = java.lang.Math.max(0, x0); x <= java.lang.Math.min(size - 1, x1); x++)
                {
                    max_height = java.lang.Math.max(max_height, heights[y][x]);
                }
            }
        }
        // Java: 180
        public void measureMaxHeight()
        {
            max_height = 0.0f;
            foreach (float[] row in heights)
            {
                foreach (float h in row)
                {
                    max_height = java.lang.Math.max(max_height, h);
                }
            }
        }
        // Java: 187
        public void refreshCaches(int x0, int y0, int x1, int y1)
        {
            x0 = java.lang.Math.max(0, x0);
            y0 = java.lang.Math.max(0, y0);
            x1 = java.lang.Math.min(size - 1, x1);
            y1 = java.lang.Math.min(size - 1, y1);
            int wx0 = x0 - rel_radius;
            int wy0 = y0 - rel_radius;
            int w = x1 - x0 + 1 + 2 * rel_radius;
            int h = y1 - y0 + 1 + 2 * rel_radius;
            double[][] sums = java.lang.JA.newArray2<double>(h + 1, w + 1);
            for (int y = 0; y < h; y++)
            {
                double row = 0;
                for (int x = 0; x < w; x++)
                {
                    row += normalizedWrap(wx0 + x, wy0 + y);
                    sums[y + 1][x + 1] = sums[y][x + 1] + row;
                }
            }
            int box = 2 * rel_radius + 1;
            float factor = (float)(1.0f / (box * box));
            for (int y_2 = y0; y_2 <= y1; y_2++)
            {
                int by = y_2 - y0;
                for (int x_2 = x0; x_2 <= x1; x_2++)
                {
                    int bx = x_2 - x0;
                    double sum = sums[by + box][bx + box] - sums[by][bx + box] - sums[by + box][bx] + sums[by][bx];
                    rel_intensity[y_2][x_2] = (float)((float)(normalized(x_2, y_2) - (float)(((float)(sum)) * factor)) + 0.5f);
                }
            }
            float lx = (float)(1.0f / ((float)(java.lang.Math.sqrt(2.0f))));
            float nzlz = (float)(light_nz * lx);
            float nz2 = (float)(light_nz * light_nz);
            for (int y_3 = y0; y_3 <= y1; y_3++)
            {
                for (int x_3 = x0; x_3 <= x1; x_3++)
                {
                    float nx = (float)(normalizedWrap(x_3 + 1, y_3) - normalizedWrap(x_3 - 1, y_3));
                    float ny = (float)(normalizedWrap(x_3, y_3 + 1) - normalizedWrap(x_3, y_3 - 1));
                    light[y_3][x_3] = (float)(((float)((float)(nx * lx) + nzlz)) / ((float)(java.lang.Math.sqrt((float)((float)((float)(nx * nx) + (float)(ny * ny)) + nz2)))));
                }
            }
        }
        // Java: 230
        public com.oddlabs.tt.mapeditor.GroundAlphas.Ranges measureRanges()
        {
            float rel_min = java.lang.Float.POSITIVE_INFINITY;
            float rel_max = java.lang.Float.NEGATIVE_INFINITY;
            float hl_min = java.lang.Float.POSITIVE_INFINITY;
            float hl_max = java.lang.Float.NEGATIVE_INFINITY;
            float sh_min = java.lang.Float.POSITIVE_INFINITY;
            float sh_max = java.lang.Float.NEGATIVE_INFINITY;
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float rel = rel_intensity[y][x];
                    rel_min = java.lang.Math.min(rel_min, rel);
                    rel_max = java.lang.Math.max(rel_max, rel);
                    float highlight = com.oddlabs.tt.mapeditor.GroundAlphas.rawHighlight(light[y][x]);
                    hl_min = java.lang.Math.min(hl_min, highlight);
                    hl_max = java.lang.Math.max(hl_max, highlight);
                    float shadow = (float)(1.0f - com.oddlabs.tt.mapeditor.GroundAlphas.rawShadow(light[y][x]));
                    sh_min = java.lang.Math.min(sh_min, shadow);
                    sh_max = java.lang.Math.max(sh_max, shadow);
                }
            }
            if (rel_min > (float)(1.0f - rel_max))
            {
                rel_min = (float)(1.0f - rel_max);
            }
            if (rel_max < (float)(1.0f - rel_min))
            {
                rel_max = (float)(1.0f - rel_min);
            }
            return new com.oddlabs.tt.mapeditor.GroundAlphas.Ranges(rel_min, rel_max, hl_min, hl_max, sh_min, sh_max);
        }
        // Java: 262
        public bool rangesMoved()
        {
            measureMaxHeight();
            com.oddlabs.tt.mapeditor.GroundAlphas.Ranges measured = measureRanges();
            bool moved_v = com.oddlabs.tt.mapeditor.GroundAlphas.moved(ranges.rel_min(), ranges.rel_max(), measured.rel_min(), measured.rel_max()) || com.oddlabs.tt.mapeditor.GroundAlphas.moved(ranges.highlight_min(), ranges.highlight_max(), measured.highlight_min(), measured.highlight_max()) || com.oddlabs.tt.mapeditor.GroundAlphas.moved(ranges.shadow_min(), ranges.shadow_max(), measured.shadow_min(), measured.shadow_max());
            if (moved_v)
            {
                ranges = measured;
            }
            return moved_v;
        }
        // Java: 274
        public static bool moved(float min, float max, float new_min, float new_max)
        {
            float range = java.lang.Math.max((float)(max - min), 1.0E-6f);
            return (float)(java.lang.Math.abs((float)(new_min - min)) / range) > com.oddlabs.tt.mapeditor.GroundAlphas.RANGE_TOLERANCE || (float)(java.lang.Math.abs((float)(new_max - max)) / range) > com.oddlabs.tt.mapeditor.GroundAlphas.RANGE_TOLERANCE;
        }
        // Java: 279
        public static float rawHighlight(float light)
        {
            return (light > com.oddlabs.tt.mapeditor.GroundAlphas.LIGHT_THRESHOLD ? light : com.oddlabs.tt.mapeditor.GroundAlphas.LIGHT_THRESHOLD);
        }
        // Java: 283
        public static float rawShadow(float light)
        {
            return (light > com.oddlabs.tt.mapeditor.GroundAlphas.LIGHT_THRESHOLD ? com.oddlabs.tt.mapeditor.GroundAlphas.LIGHT_THRESHOLD : java.lang.Math.max(0, light));
        }
        // Java: 294
        public int[] reach(int x0, int y0, int x1, int y1)
        {
            int spread = rel_radius + com.oddlabs.tt.mapeditor.GroundAlphas.MARGIN;
            return new int[] { java.lang.Math.max(0, x0 - spread), java.lang.Math.max(0, y0 - spread), java.lang.Math.min(size - 1, x1 + spread + shadowReach()), java.lang.Math.min(size - 1, y1 + spread) };
        }
        // Java: 301
        public int shadowReach()
        {
            return java.lang.Math.min(size, java.lang.JC.d2i(java.lang.Math.ceil((float)((float)(max_height / height_scale) / shadowDescent()))) + 2);
        }
        // Java: 305
        public float shadowDescent()
        {
            return (float)(8.0f / size);
        }
        // Java: 313
        public void compute(int x0, int y0, int x1, int y1, java.nio.ByteBuffer out_low, java.nio.ByteBuffer out_high)
        {
            int wx0 = java.lang.Math.max(0, x0 - java.lang.Math.max(com.oddlabs.tt.mapeditor.GroundAlphas.MARGIN, shadowReach()));
            int wy0 = java.lang.Math.max(0, y0 - com.oddlabs.tt.mapeditor.GroundAlphas.MARGIN);
            int wx1 = java.lang.Math.min(size - 1, x1 + com.oddlabs.tt.mapeditor.GroundAlphas.MARGIN);
            int wy1 = java.lang.Math.min(size - 1, y1 + com.oddlabs.tt.mapeditor.GroundAlphas.MARGIN);
            int w = wx1 - wx0 + 1;
            int h = wy1 - wy0 + 1;
            com.oddlabs.procedural.Channel height = new com.oddlabs.procedural.Channel(w, h);
            com.oddlabs.procedural.Channel slope = new com.oddlabs.procedural.Channel(w, h);
            com.oddlabs.procedural.Channel relheight = new com.oddlabs.procedural.Channel(w, h);
            com.oddlabs.procedural.Channel noise = new com.oddlabs.procedural.Channel(w, h);
            float rel_factor = (float)(1.0f / ((float)(ranges.rel_max() - ranges.rel_min())));
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int mx = wx0 + x;
                    int my = wy0 + y;
                    float center = normalized(mx, my);
                    height.putPixel(x, y, center);
                    slope.putPixel(x, y, java.lang.Math.max(java.lang.Math.max(java.lang.Math.abs((float)(center - normalizedWrap(mx - 1, my))), java.lang.Math.abs((float)(center - normalizedWrap(mx + 1, my)))), java.lang.Math.max(java.lang.Math.abs((float)(center - normalizedWrap(mx, my - 1))), java.lang.Math.abs((float)(center - normalizedWrap(mx, my + 1))))));
                    relheight.putPixel(x, y, (float)(rel_factor * ((float)(rel_intensity[my][mx] - ranges.rel_min()))));
                    noise.putPixel(x, y, grass_noise.getPixel(mx, my));
                }
            }
            com.oddlabs.procedural.Channel[] alphas = new com.oddlabs.procedural.Channel[com.oddlabs.tt.mapeditor.GroundAlphas.NUM_ALPHAS];
            switch ((terrain.@base()).ordinal())   // (the Elemental ground is painted like the Natives')
            {
                case 0 /*NATIVE*/:
                {
                    alphas[0] = com.oddlabs.tt.mapeditor.GroundAlphas.dirtAlpha(height, relheight);
                    alphas[1] = rubbleAlpha(height, slope, relheight);
                    alphas[2] = slope.copy().threshold(access_threshold, 1.0f);
                    alphas[3] = grassAlpha(height, slope, relheight, noise);
                    break;
                }
                case 1 /*VIKING*/:
                {
                    alphas[0] = com.oddlabs.tt.mapeditor.GroundAlphas.dirtAlpha(height, relheight);
                    alphas[1] = slope.copy().threshold(access_threshold, 1.0f);
                    alphas[2] = grassAlpha(height, slope, relheight, noise);
                    com.oddlabs.procedural.Channel snow = height.copy().dynamicRange(0.5f, 0.6f, 0.0f, 1.0f);
                    snow.channelSubtract(alphas[1].copy());
                    alphas[3] = snow.smooth(1).smooth(1);
                    break;
                }
            }
            com.oddlabs.procedural.Channel[] lighting_v = lighting(height, wx0, wy0);
            alphas[4] = lighting_v[0];
            alphas[5] = lighting_v[1];
            float depth_offset = (float)(com.oddlabs.tt.mapeditor.GroundAlphas.SEABOTTOM_DEPTH_OFFSET_METERS / height_scale);
            alphas[6] = height.copy().invert().dynamicRange((float)((float)(1.0f - com.oddlabs.tt.global.Globals.SEA_LEVEL) + depth_offset), 1.0f, 0.0f, 1.0f).gamma(0.5f);
            for (int y_2 = y0; y_2 <= y1; y_2++)
            {
                for (int x_2 = x0; x_2 <= x1; x_2++)
                {
                    int cx = x_2 - wx0;
                    int cy = y_2 - wy0;
                    for (int i = 0; i < 4; i++)
                    {
                        out_low.put(com.oddlabs.tt.mapeditor.GroundAlphas.toByte(alphas[i].getPixel(cx, cy)));
                    }
                    for (int i_2 = 4; i_2 < 7; i_2++)
                    {
                        out_high.put(com.oddlabs.tt.mapeditor.GroundAlphas.toByte(alphas[i_2].getPixel(cx, cy)));
                    }
                    out_high.put(unchecked((sbyte)(0)));
                }
            }
        }
        // Java: 384
        public static sbyte toByte(float @value)
        {
            return unchecked((sbyte)(java.lang.Math.clamp(java.lang.Math.round((float)(@value * 255)), 0, 255)));
        }
        // Java: 389
        public static com.oddlabs.procedural.Channel dirtAlpha(com.oddlabs.procedural.Channel height, com.oddlabs.procedural.Channel relheight)
        {
            com.oddlabs.procedural.Channel alpha = height.copy().dynamicRange((float)(1.1f * com.oddlabs.tt.global.Globals.SEA_LEVEL), (float)(2.0f * com.oddlabs.tt.global.Globals.SEA_LEVEL), 0.0f, 1.0f);
            alpha.channelSubtract(relheight.copy().invert().dynamicRange(0.5f, 0.6f, 0.0f, 0.5f));
            return alpha;
        }
        // Java: 396
        public com.oddlabs.procedural.Channel rubbleAlpha(com.oddlabs.procedural.Channel height, com.oddlabs.procedural.Channel slope, com.oddlabs.procedural.Channel relheight)
        {
            com.oddlabs.procedural.Channel alpha = slope.copy().dynamicRange(build_threshold, access_threshold, 0.0f, 1.0f);
            alpha.channelSubtract(height.copy().invert().dynamicRange(0.8f, 1.0f, 0.0f, 1.0f));
            alpha.channelSubtract(relheight.copy().invert().dynamicRange(0.5f, 0.65f, 0.0f, 0.5f));
            return alpha;
        }
        // Java: 405
        public com.oddlabs.procedural.Channel grassAlpha(com.oddlabs.procedural.Channel height, com.oddlabs.procedural.Channel slope, com.oddlabs.procedural.Channel relheight, com.oddlabs.procedural.Channel noise)
        {
            com.oddlabs.procedural.Channel alpha = noise.copy().dynamicRange((float)(1.0f - vegetation_amount), 1.0f, 0.0f, 1.0f).gamma2();
            alpha.channelBrightest(slope.copy().dynamicRange(0.0f, access_threshold, 0.0f, 1.0f).invert().dynamicRange((float)(1.0f - vegetation_amount), 1.0f, 0.0f, 1.0f).gamma2());
            alpha.channelAdd(relheight.copy().invert().add((float)(-0.5f)).multiply(2.0f));
            alpha.channelSubtract(height.copy().invert().dynamicRange(0.6f, 0.8f, 0.0f, 1.0f));
            alpha.channelSubtract(slope.copy().threshold((float)(0.75f * access_threshold), 1.0f).smooth(3));
            alpha.channelSubtract(relheight.copy().invert().dynamicRange(0.5f, 0.7f, 0.0f, 0.5f));
            return alpha;
        }
        // Java: 418
        public com.oddlabs.procedural.Channel[] lighting(com.oddlabs.procedural.Channel height, int wx0, int wy0)
        {
            int w = height.getWidth();
            int h = height.getHeight();
            com.oddlabs.procedural.Channel highlight = new com.oddlabs.procedural.Channel(w, h);
            com.oddlabs.procedural.Channel shadow = new com.oddlabs.procedural.Channel(w, h);
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    float @value = light[wy0 + y][wx0 + x];
                    highlight.putPixel(x, y, (float)((float)(0.25f * ((float)(com.oddlabs.tt.mapeditor.GroundAlphas.rawHighlight(@value) - ranges.highlight_min()))) / ((float)(ranges.highlight_max() - ranges.highlight_min()))));
                    shadow.putPixel(x, y, (float)((float)(0.75f * ((float)((float)(1.0f - com.oddlabs.tt.mapeditor.GroundAlphas.rawShadow(@value)) - ranges.shadow_min()))) / ((float)(ranges.shadow_max() - ranges.shadow_min()))));
                }
            }
            com.oddlabs.procedural.Channel shadowcast = new com.oddlabs.procedural.Channel(w, h);
            float descent = shadowDescent();
            for (int y_2 = 0; y_2 < h; y_2++)
            {
                float peak = 0.0f;
                for (int x_2 = 0; x_2 < w; x_2++)
                {
                    float value_2 = height.getPixel(x_2, y_2);
                    peak = (float)(peak - descent);
                    if (peak > value_2)
                    {
                        shadowcast.putPixel(x_2, y_2, 1.0f);
                    }
                    else
                    {
                        peak = value_2;
                    }
                }
            }
            shadow.channelBrightest(shadowcast.smooth(1).brightness(0.67f));
            float shading_band = (float)(com.oddlabs.tt.mapeditor.GroundAlphas.SEABOTTOM_DEPTH_OFFSET_METERS / height_scale);
            for (int y_3 = 0; y_3 < h; y_3++)
            {
                for (int x_3 = 0; x_3 < w; x_3++)
                {
                    float value_3 = height.getPixel(x_3, y_3);
                    if (value_3 < com.oddlabs.tt.global.Globals.SEA_LEVEL)
                    {
                        float fade = java.lang.Math.clamp((float)(((float)(value_3 - ((float)(com.oddlabs.tt.global.Globals.SEA_LEVEL - shading_band)))) / shading_band), 0.0f, 1.0f);
                        float factor = (float)(com.oddlabs.tt.mapeditor.GroundAlphas.UNDERWATER_SHADING + (float)(((float)(1.0f - com.oddlabs.tt.mapeditor.GroundAlphas.UNDERWATER_SHADING)) * fade));
                        shadow.putPixel(x_3, y_3, (float)(shadow.getPixel(x_3, y_3) * factor));
                        highlight.putPixel(x_3, y_3, (float)(highlight.getPixel(x_3, y_3) * factor));
                    }
                    highlight.putPixel(x_3, y_3, (float)(highlight.getPixel(x_3, y_3) * supply_highlight[wy0 + y_3][wx0 + x_3]));
                    shadow.putPixel(x_3, y_3, java.lang.Math.max(shadow.getPixel(x_3, y_3), supply_shadow[wy0 + y_3][wx0 + x_3]));
                }
            }
            return new com.oddlabs.procedural.Channel[] { highlight, shadow };
        }
        static GroundAlphas()
        {
            GroundAlphas.LIGHT_THRESHOLD = ((float)(java.lang.Math.sqrt(0.5f)));
        }
        public GroundAlphas(float[][] heights, com.oddlabs.tt.mapeditor.MapSettings settings, java.util.List trees, java.util.List palm_trees, java.util.List shaded_supplies) : base()
        {
            this.heights = heights;
            this.size = heights.Length;
            this.terrain = com.oddlabs.tt.procedural.Landscape.TerrainType.values()[settings.terrain()];
            int meters = settings.getMetersPerWorld();
            this.height_scale = settings.getHeightScale();
            this.access_threshold = settings.getAccessThreshold();
            this.build_threshold = (float)(access_threshold / 2.0f);
            this.vegetation_amount = (float)(0.25f + (float)((float)(0.75f * settings.trees()) / ((float)(com.oddlabs.tt.mapeditor.MapSettings.SLIDER_UNIT))));
            this.rel_radius = java.lang.Math.max(1, size >> 5);
            this.light_nz = (float)((float)(2.0f * com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID) / height_scale);
            this.grass_noise = new com.oddlabs.tt.procedural.Midpoint(size, 4, 0.45f, com.oddlabs.tt.global.Globals.LANDSCAPE_SEED).toChannel();
            this.supply_highlight = java.lang.JA.newArray2<float>(size, size);
            this.supply_shadow = java.lang.JA.newArray2<float>(size, size);
            foreach (float[] row in supply_highlight)
            {
                java.util.Arrays.fill(row, 1.0f);
            }
            this.shadow_size = ((meters) switch { 256 => (int)(java.lang.Math.max(size >> 5, 2)), 512 => (int)(java.lang.Math.max(size >> 6, 2)), 1024 => (int)(java.lang.Math.max(size >> 7, 2)), _ => (int)(java.lang.Math.max(size >> 8, 2)) });
            this.stamp = new com.oddlabs.procedural.Channel(shadow_size << 1, shadow_size << 1).place(new com.oddlabs.procedural.Channel(shadow_size, shadow_size).fill(1.0f), shadow_size >> 1, shadow_size >> 1).smoothFast();
            stampSupplies(trees, palm_trees, shaded_supplies, 0, 0, size - 1, size - 1);
            this.rel_intensity = java.lang.JA.newArray2<float>(size, size);
            this.light = java.lang.JA.newArray2<float>(size, size);
            refreshCaches(0, 0, size - 1, size - 1);
            this.ranges = measureRanges();
            measureMaxHeight();
        }
    }
}
