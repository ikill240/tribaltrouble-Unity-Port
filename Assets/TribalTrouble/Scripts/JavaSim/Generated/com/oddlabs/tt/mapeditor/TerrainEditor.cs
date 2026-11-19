// Translated from TerrainEditor.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.mapeditor
{
    // Java: 23
    public class TerrainEditor : java.lang.Object
    {
        public const float MIN_HEIGHT = 0.0f;
        public const float MAX_HEIGHT = 150.0f;
        public const float RAISE_RATE = 30.0f;
        public const float SETTLE_RATE = 8.0f;
        public const float MAX_SETTLE_STEP = 0.5f;
        public const float FLATTEN_RATE = 15.0f;
        public const float PLATEAU_CORE = 0.6f;
        public const int MAX_UNDO_STEPS = 32;
        public com.oddlabs.tt.landscape.HeightMap height_map;
        public float[][] heights;
        public int size;
        public java.nio.FloatBuffer upload;
        public float[][] scratch;
        public java.util.Deque undo_steps;
        public java.util.Deque redo_steps;
        // Java: 52
        public sealed class Shape : java.lang.Enum
        {
            public static Shape[] values() { return (Shape[])VALUES_.Clone(); }
            public static Shape valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly Shape CIRCLE = new Shape("CIRCLE", 0);
            public static readonly Shape SQUARE = new Shape("SQUARE", 1);
            public static readonly Shape DIAMOND = new Shape("DIAMOND", 2);
            public static readonly Shape[] VALUES_ = new Shape[] { CIRCLE, SQUARE, DIAMOND };
            public Shape(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        public com.oddlabs.tt.mapeditor.TerrainEditor.Shape shape;
        public const float CLIFF_CORE = 0.85f;
        public int dirty_x0;
        public int dirty_y0;
        public int dirty_x1;
        public int dirty_y1;
        public float[][] stroke_backup;
        public int stroke_x0;
        public int stroke_y0;
        public int stroke_x1;
        public int stroke_y1;
        public bool modified;
        // Java: 76
        public class UndoStep : java.lang.Object
        {
            public int x0() { return x0_f; }
            public int y0() { return y0_f; }
            public float[][] heights() { return heights_f; }
            public UndoStep(int x0, int y0, float[][] heights)
            {
                this.x0_f = x0;
                this.y0_f = y0;
                this.heights_f = heights;
            }
            public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is UndoStep r)) return false; return true && java.lang.JO.equals(x0_f, r.x0_f) && java.lang.JO.equals(y0_f, r.y0_f) && java.lang.JO.equals(heights_f, r.heights_f); }
            public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(x0_f); h = 31 * h + java.lang.JO.hashCode(y0_f); h = 31 * h + java.lang.JO.hashCode(heights_f); } return h; }
            public override string toString() { return "UndoStep[" + "x0=" + java.lang.JO.toString(x0_f) + ", y0=" + java.lang.JO.toString(y0_f) + ", heights=" + java.lang.JO.toString(heights_f) + "]"; }
            public int x0_f;
            public int y0_f;
            public float[][] heights_f;
        }
        // Java: 80
        public interface ChangeListener
        {
            // Java: 82
            void heightsChanged(int x0, int y0, int x1, int y1);
        }
        public sealed class ChangeListener_L : java.lang.Object, ChangeListener
        {
            readonly System.Action<int, int, int, int> f;
            public ChangeListener_L(System.Action<int, int, int, int> f) { this.f = f; }
            public void heightsChanged(int p0, int p1, int p2, int p3) { f(p0, p1, p2, p3); }
        }
        public com.oddlabs.tt.mapeditor.TerrainEditor.ChangeListener listener;
        // Java: 100
        public int getSize()
        {
            return size;
        }
        // Java: 104
        public bool isModified()
        {
            return modified;
        }
        // Java: 108
        public void markSaved()
        {
            modified = false;
        }
        // Java: 113
        public float[][] peekHeights()
        {
            return heights;
        }
        // Java: 117
        public float[][] copyHeights()
        {
            float[][] copy = new float[size][];
            for (int y = 0; y < size; y++)
            {
                copy[y] = ((float[])heights[y].clone());
            }
            return copy;
        }
        // Java: 125
        public float getHeight(float gx, float gy)
        {
            gx = java.lang.Math.clamp(gx, 0.0f, size - 1);
            gy = java.lang.Math.clamp(gy, 0.0f, size - 1);
            int x0 = java.lang.Math.min(java.lang.JC.f2i(gx), size - 2);
            int y0 = java.lang.Math.min(java.lang.JC.f2i(gy), size - 2);
            float fx = (float)(gx - x0);
            float fy = (float)(gy - y0);
            float h0 = (float)((float)(heights[y0][x0] * ((float)(1 - fx))) + (float)(heights[y0][x0 + 1] * fx));
            float h1 = (float)((float)(heights[y0 + 1][x0] * ((float)(1 - fx))) + (float)(heights[y0 + 1][x0 + 1] * fx));
            return (float)((float)(h0 * ((float)(1 - fy))) + (float)(h1 * fy));
        }
        // Java: 139
        public void beginStroke()
        {
            if (stroke_backup != null)
            {
                endStroke();
            }
            stroke_backup = copyHeights();
            stroke_x0 = java.lang.Integer.MAX_VALUE;
            stroke_y0 = java.lang.Integer.MAX_VALUE;
            stroke_x1 = java.lang.Integer.MIN_VALUE;
            stroke_y1 = java.lang.Integer.MIN_VALUE;
        }
        // Java: 150
        public bool endStroke()
        {
            float[][] backup = stroke_backup;
            stroke_backup = null;
            if (backup == null || stroke_x0 > stroke_x1)
            {
                return false;
            }
            int w = stroke_x1 - stroke_x0 + 1;
            int h = stroke_y1 - stroke_y0 + 1;
            float[][] saved = java.lang.JA.newArray2<float>(h, w);
            for (int y = 0; y < h; y++)
            {
                java.lang.JSystem.arraycopy(backup[stroke_y0 + y], stroke_x0, saved[y], 0, w);
            }
            undo_steps.push(new com.oddlabs.tt.mapeditor.TerrainEditor.UndoStep(stroke_x0, stroke_y0, saved));
            redo_steps.clear();
            while (undo_steps.size() > com.oddlabs.tt.mapeditor.TerrainEditor.MAX_UNDO_STEPS)
            {
                undo_steps.removeLast();
            }
            return true;
        }
        // Java: 169
        public bool undo()
        {
            endStroke();
            com.oddlabs.tt.mapeditor.TerrainEditor.UndoStep step = ((com.oddlabs.tt.mapeditor.TerrainEditor.UndoStep)undo_steps.poll());
            if (step == null)
            {
                return false;
            }
            redo_steps.push(snapshot(step));
            for (int y = 0; y < step.heights().Length; y++)
            {
                float[] row = step.heights()[y];
                java.lang.JSystem.arraycopy(row, 0, heights[step.y0() + y], step.x0(), row.Length);
            }
            markDirty(step.x0(), step.y0(), step.x0() + step.heights()[0].Length - 1, step.y0() + step.heights().Length - 1);
            modified = true;
            flush();
            return true;
        }
        // Java: 187
        public bool redo()
        {
            endStroke();
            com.oddlabs.tt.mapeditor.TerrainEditor.UndoStep step = ((com.oddlabs.tt.mapeditor.TerrainEditor.UndoStep)redo_steps.poll());
            if (step == null)
            {
                return false;
            }
            undo_steps.push(snapshot(step));
            restore(step);
            return true;
        }
        // Java: 198
        public void clearRedo()
        {
            redo_steps.clear();
        }
        // Java: 203
        public com.oddlabs.tt.mapeditor.TerrainEditor.UndoStep snapshot(com.oddlabs.tt.mapeditor.TerrainEditor.UndoStep area)
        {
            int h = area.heights().Length;
            int w = area.heights()[0].Length;
            float[][] current = java.lang.JA.newArray2<float>(h, w);
            for (int y = 0; y < h; y++)
            {
                java.lang.JSystem.arraycopy(heights[area.y0() + y], area.x0(), current[y], 0, w);
            }
            return new com.oddlabs.tt.mapeditor.TerrainEditor.UndoStep(area.x0(), area.y0(), current);
        }
        // Java: 212
        public void restore(com.oddlabs.tt.mapeditor.TerrainEditor.UndoStep step)
        {
            for (int y = 0; y < step.heights().Length; y++)
            {
                float[] row = step.heights()[y];
                java.lang.JSystem.arraycopy(row, 0, heights[step.y0() + y], step.x0(), row.Length);
            }
            markDirty(step.x0(), step.y0(), step.x0() + step.heights()[0].Length - 1, step.y0() + step.heights().Length - 1);
            modified = true;
            flush();
        }
        // Java: 223
        public void setShape(com.oddlabs.tt.mapeditor.TerrainEditor.Shape shape)
        {
            this.shape = shape;
        }
        // Java: 227
        public com.oddlabs.tt.mapeditor.TerrainEditor.Shape getShape()
        {
            return shape;
        }
        // Java: 234
        public void applyCliff(float cx, float cy, float radius, float intensity, int sign, float dt)
        {
            float amount = (float)((float)((float)(sign * intensity) * com.oddlabs.tt.mapeditor.TerrainEditor.RAISE_RATE) * dt);
            forEachCell(cx, cy, radius, com.oddlabs.tt.mapeditor.TerrainEditor.CLIFF_CORE, new com.oddlabs.tt.mapeditor.TerrainEditor.CellFunction_L((int x, int y, float weight) =>
            {
                return (float)(heights[y][x] + (float)(amount * weight));
            }));
        }
        // Java: 240
        public void applyPlateau(float cx, float cy, float radius, float target)
        {
            forEachCell(cx, cy, radius, 1.0f, new com.oddlabs.tt.mapeditor.TerrainEditor.CellFunction_L((int x, int y, float weight) =>
            {
                return target;
            }));
        }
        // Java: 245
        public void applyHeight(float cx, float cy, float radius, float intensity, int sign, float dt)
        {
            float amount = (float)((float)((float)(sign * intensity) * com.oddlabs.tt.mapeditor.TerrainEditor.RAISE_RATE) * dt);
            forEachCell(cx, cy, radius, new com.oddlabs.tt.mapeditor.TerrainEditor.CellFunction_L((int x, int y, float weight) =>
            {
                return (float)(heights[y][x] + (float)(amount * weight));
            }));
        }
        // Java: 254
        public void applyFlatten(float cx, float cy, float radius, float intensity, int sign, float dt, float target)
        {
            float blend = (intensity >= 1.0f ? 1.0f : (float)(1.0f - ((float)(java.lang.StrictMath.exp((float)((float)((float)((float)(-com.oddlabs.tt.mapeditor.TerrainEditor.FLATTEN_RATE) * intensity) / ((float)(1.0f - intensity))) * dt))))));
            forEachCell(cx, cy, radius, com.oddlabs.tt.mapeditor.TerrainEditor.PLATEAU_CORE, new com.oddlabs.tt.mapeditor.TerrainEditor.CellFunction_L((int x, int y, float weight) =>
            {
                float h = heights[y][x];
                float gap = (float)(target - h);
                if ((float)(gap * sign) <= 0.0f)
                {
                    return h;
                }
                return (float)(h + (float)((float)(gap * blend) * weight));
            }));
        }
        // Java: 267
        public void applySmooth(float cx, float cy, float radius, float intensity, int sign, float dt)
        {
            int x0 = java.lang.Math.max(0, java.lang.JC.d2i(java.lang.Math.floor((float)(cx - radius))) - 1);
            int y0 = java.lang.Math.max(0, java.lang.JC.d2i(java.lang.Math.floor((float)(cy - radius))) - 1);
            int x1 = java.lang.Math.min(size - 1, java.lang.JC.d2i(java.lang.Math.ceil((float)(cx + radius))) + 1);
            int y1 = java.lang.Math.min(size - 1, java.lang.JC.d2i(java.lang.Math.ceil((float)(cy + radius))) + 1);
            for (int y = y0; y <= y1; y++)
            {
                java.lang.JSystem.arraycopy(heights[y], x0, scratch[y], x0, x1 - x0 + 1);
            }
            float rate = (float)((float)(intensity * com.oddlabs.tt.mapeditor.TerrainEditor.SETTLE_RATE) * dt);
            forEachCell(cx, cy, radius, new com.oddlabs.tt.mapeditor.TerrainEditor.CellFunction_L((int x, int y_2, float weight) =>
            {
                float h = scratch[y_2][x];
                float average = localAverage(x, y_2);
                return (float)(h + (float)((float)(sign * ((float)(average - h))) * java.lang.Math.min((float)(rate * weight), com.oddlabs.tt.mapeditor.TerrainEditor.MAX_SETTLE_STEP)));
            }));
        }
        // Java: 284
        public void applyRandom(float cx, float cy, float radius, float intensity, int sign, float dt, int seed)
        {
            float amount = (float)((float)((float)(sign * intensity) * com.oddlabs.tt.mapeditor.TerrainEditor.RAISE_RATE) * dt);
            float feature = java.lang.Math.max(2.0f, (float)(radius / 3.0f));
            forEachCell(cx, cy, radius, new com.oddlabs.tt.mapeditor.TerrainEditor.CellFunction_L((int x, int y, float weight) =>
            {
                return (float)(heights[y][x] + (float)((float)(amount * weight) * com.oddlabs.tt.mapeditor.TerrainEditor.valueNoise((float)(x / feature), (float)(y / feature), seed)));
            }));
        }
        // Java: 296
        public void applyRamp(float ax, float ay, float ha, float bx, float by, float hb, float radius, float intensity, int sign)
        {
            float dx = (float)(bx - ax);
            float dy = (float)(by - ay);
            float length_sq = (float)((float)(dx * dx) + (float)(dy * dy));
            int x0 = java.lang.Math.max(0, java.lang.JC.d2i(java.lang.Math.floor((float)(java.lang.Math.min(ax, bx) - radius))));
            int y0 = java.lang.Math.max(0, java.lang.JC.d2i(java.lang.Math.floor((float)(java.lang.Math.min(ay, by) - radius))));
            int x1 = java.lang.Math.min(size - 1, java.lang.JC.d2i(java.lang.Math.ceil((float)(java.lang.Math.max(ax, bx) + radius))));
            int y1 = java.lang.Math.min(size - 1, java.lang.JC.d2i(java.lang.Math.ceil((float)(java.lang.Math.max(ay, by) + radius))));
            float core = (float)(radius * com.oddlabs.tt.mapeditor.TerrainEditor.PLATEAU_CORE);
            bool changed = false;
            for (int y = y0; y <= y1; y++)
            {
                for (int x = x0; x <= x1; x++)
                {
                    float t = (length_sq > 0.0f ? java.lang.Math.clamp((float)(((float)((float)(((float)(x - ax)) * dx) + (float)(((float)(y - ay)) * dy))) / length_sq), 0.0f, 1.0f) : 0.0f);
                    float px = (float)((float)(ax + (float)(dx * t)) - x);
                    float py = (float)((float)(ay + (float)(dy * t)) - y);
                    float distance = ((float)(java.lang.Math.sqrt((float)((float)(px * px) + (float)(py * py)))));
                    if (distance >= radius)
                    {
                        continue;
                    }
                    float weight = com.oddlabs.tt.mapeditor.TerrainEditor.plateau(distance, radius, core);
                    float target = (float)(ha + (float)(((float)(hb - ha)) * t));
                    float h = heights[y][x];
                    float gap = (float)(target - h);
                    if ((float)(gap * sign) <= 0.0f)
                    {
                        continue;
                    }
                    heights[y][x] = clampHeight(x, y, (float)(h + (float)((float)(gap * weight) * intensity)));
                    changed = true;
                }
            }
            if (changed)
            {
                markDirty(x0, y0, x1, y1);
            }
        }
        // Java: 329
        public interface CellFunction
        {
            // Java: 331
            float apply(int x, int y, float weight);
        }
        public sealed class CellFunction_L : java.lang.Object, CellFunction
        {
            readonly System.Func<int, int, float, float> f;
            public CellFunction_L(System.Func<int, int, float, float> f) { this.f = f; }
            public float apply(int p0, int p1, float p2) { return f(p0, p1, p2); }
        }
        // Java: 334
        public void forEachCell(float cx, float cy, float radius, com.oddlabs.tt.mapeditor.TerrainEditor.CellFunction function)
        {
            forEachCell(cx, cy, radius, 0.0f, function);
        }
        // Java: 339
        public void forEachCell(float cx, float cy, float radius, float core, com.oddlabs.tt.mapeditor.TerrainEditor.CellFunction function)
        {
            int x0 = java.lang.Math.max(0, java.lang.JC.d2i(java.lang.Math.floor((float)(cx - radius))));
            int y0 = java.lang.Math.max(0, java.lang.JC.d2i(java.lang.Math.floor((float)(cy - radius))));
            int x1 = java.lang.Math.min(size - 1, java.lang.JC.d2i(java.lang.Math.ceil((float)(cx + radius))));
            int y1 = java.lang.Math.min(size - 1, java.lang.JC.d2i(java.lang.Math.ceil((float)(cy + radius))));
            if (x0 > x1 || y0 > y1)
            {
                return;
            }
            for (int y = y0; y <= y1; y++)
            {
                for (int x = x0; x <= x1; x++)
                {
                    float dx = (float)(x - cx);
                    float dy = (float)(y - cy);
                    float distance = ((shape).ordinal() switch { 0 /*CIRCLE*/ => (float)(((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy)))))), 1 /*SQUARE*/ => (float)(java.lang.Math.max(java.lang.Math.abs(dx), java.lang.Math.abs(dy))), 2 /*DIAMOND*/ => (float)((float)(java.lang.Math.abs(dx) + java.lang.Math.abs(dy))), _ => throw new System.InvalidOperationException("MatchException") });
                    if (distance >= radius)
                    {
                        continue;
                    }
                    heights[y][x] = clampHeight(x, y, function.apply(x, y, com.oddlabs.tt.mapeditor.TerrainEditor.plateau(distance, radius, (float)(radius * core))));
                }
            }
            markDirty(x0, y0, x1, y1);
        }
        // Java: 365
        public static float falloff(float distance, float radius)
        {
            return (float)(0.5f * ((float)(1.0f + ((float)(java.lang.StrictMath.cos(java.lang.Math.PI * distance / radius))))));
        }
        // Java: 370
        public static float plateau(float distance, float radius, float core)
        {
            return (distance <= core ? 1.0f : com.oddlabs.tt.mapeditor.TerrainEditor.falloff((float)(distance - core), (float)(radius - core)));
        }
        // Java: 374
        public float localAverage(int x, int y)
        {
            float sum = 0.0f;
            float total = 0.0f;
            for (int oy = (-1); oy <= 1; oy++)
            {
                int sy = java.lang.Math.clamp(y + oy, 0, size - 1);
                for (int ox = (-1); ox <= 1; ox++)
                {
                    int sx = java.lang.Math.clamp(x + ox, 0, size - 1);
                    float w = (2 - java.lang.Math.abs(ox)) * (2 - java.lang.Math.abs(oy));
                    sum = (float)(sum + (float)(scratch[sy][sx] * w));
                    total = (float)(total + w);
                }
            }
            return (float)(sum / total);
        }
        // Java: 391
        public static float valueNoise(float x, float y, int seed)
        {
            int xi = java.lang.JC.d2i(java.lang.Math.floor(x));
            int yi = java.lang.JC.d2i(java.lang.Math.floor(y));
            float fx = com.oddlabs.tt.mapeditor.TerrainEditor.smooth((float)(x - xi));
            float fy = com.oddlabs.tt.mapeditor.TerrainEditor.smooth((float)(y - yi));
            float a = (float)((float)(com.oddlabs.tt.mapeditor.TerrainEditor.lattice(xi, yi, seed) * ((float)(1 - fx))) + (float)(com.oddlabs.tt.mapeditor.TerrainEditor.lattice(xi + 1, yi, seed) * fx));
            float b = (float)((float)(com.oddlabs.tt.mapeditor.TerrainEditor.lattice(xi, yi + 1, seed) * ((float)(1 - fx))) + (float)(com.oddlabs.tt.mapeditor.TerrainEditor.lattice(xi + 1, yi + 1, seed) * fx));
            return (float)((float)(a * ((float)(1 - fy))) + (float)(b * fy));
        }
        // Java: 401
        public static float smooth(float t)
        {
            return (float)((float)(t * t) * ((float)(3 - (float)(2 * t))));
        }
        // Java: 405
        public static float lattice(int x, int y, int seed)
        {
            int h = x * 374761393 + y * 668265263 + seed * 144665;
            h = (h ^ (java.lang.JC.ushr(h, 13))) * 1274126177;
            h ^= java.lang.JC.ushr(h, 16);
            return (float)((h & 16777215) / ((float)(16777215)));
        }
        // Java: 417
        public float clampHeight(int x, int y, float h)
        {
            if (x == 0 || y == 0 || x == size - 1 || y == size - 1)
            {
                return com.oddlabs.tt.mapeditor.TerrainEditor.MIN_HEIGHT;
            }
            return java.lang.Math.clamp(h, com.oddlabs.tt.mapeditor.TerrainEditor.MIN_HEIGHT, com.oddlabs.tt.mapeditor.TerrainEditor.MAX_HEIGHT);
        }
        // Java: 424
        public static void pinEdges(float[][] heights)
        {
            int last = heights.Length - 1;
            for (int i = 0; i <= last; i++)
            {
                heights[0][i] = com.oddlabs.tt.mapeditor.TerrainEditor.MIN_HEIGHT;
                heights[last][i] = com.oddlabs.tt.mapeditor.TerrainEditor.MIN_HEIGHT;
                heights[i][0] = com.oddlabs.tt.mapeditor.TerrainEditor.MIN_HEIGHT;
                heights[i][last] = com.oddlabs.tt.mapeditor.TerrainEditor.MIN_HEIGHT;
            }
        }
        // Java: 436
        public void markDirty(int x0, int y0, int x1, int y1)
        {
            modified = true;
            dirty_x0 = java.lang.Math.min(dirty_x0, x0);
            dirty_y0 = java.lang.Math.min(dirty_y0, y0);
            dirty_x1 = java.lang.Math.max(dirty_x1, x1);
            dirty_y1 = java.lang.Math.max(dirty_y1, y1);
            if (stroke_backup != null)
            {
                stroke_x0 = java.lang.Math.min(stroke_x0, x0);
                stroke_y0 = java.lang.Math.min(stroke_y0, y0);
                stroke_x1 = java.lang.Math.max(stroke_x1, x1);
                stroke_y1 = java.lang.Math.max(stroke_y1, y1);
            }
        }
        // Java: 451
        public void flush()
        {
            if (dirty_x0 > dirty_x1)
            {
                return;
            }
            int x0 = dirty_x0;
            int y0 = dirty_y0;
            int x1 = dirty_x1;
            int y1 = dirty_y1;
            dirty_x0 = java.lang.Integer.MAX_VALUE;
            dirty_y0 = java.lang.Integer.MAX_VALUE;
            dirty_x1 = java.lang.Integer.MIN_VALUE;
            dirty_y1 = java.lang.Integer.MIN_VALUE;
            int previous_texture = org.lwjgl.opengl.GL11.glGetInteger(org.lwjgl.opengl.GL11.GL_TEXTURE_BINDING_2D);
            try
            {
                uploadRect(x0, y0, x1, y1);
                growPatchBounds(x0, y0, x1, y1);
            }
            finally
            {
                org.lwjgl.opengl.GL11.glBindTexture(org.lwjgl.opengl.GL11.GL_TEXTURE_2D, previous_texture);
            }
            listener.heightsChanged(x0, y0, x1, y1);
        }
        // Java: 474
        public void uploadRect(int x0, int y0, int x1, int y1)
        {
            int w = x1 - x0 + 1;
            int h = y1 - y0 + 1;
            upload.clear();
            for (int y = y0; y <= y1; y++)
            {
                upload.put(heights[y], x0, w);
            }
            upload.flip();
            org.lwjgl.opengl.GL11.glBindTexture(org.lwjgl.opengl.GL11.GL_TEXTURE_2D, height_map.getHeightTexture().getHandle());
            org.lwjgl.opengl.GL11.glPixelStorei(org.lwjgl.opengl.GL11.GL_UNPACK_ROW_LENGTH, 0);
            org.lwjgl.opengl.GL11.glPixelStorei(org.lwjgl.opengl.GL11.GL_UNPACK_SKIP_PIXELS, 0);
            org.lwjgl.opengl.GL11.glPixelStorei(org.lwjgl.opengl.GL11.GL_UNPACK_SKIP_ROWS, 0);
            org.lwjgl.opengl.GL11.glPixelStorei(org.lwjgl.opengl.GL11.GL_UNPACK_ALIGNMENT, 1);
            org.lwjgl.opengl.GL11.glTexSubImage2D(org.lwjgl.opengl.GL11.GL_TEXTURE_2D, 0, x0, y0, w, h, org.lwjgl.opengl.GL11.GL_RED, org.lwjgl.opengl.GL11.GL_FLOAT, upload);
        }
        // Java: 489
        public void growPatchBounds(int x0, int y0, int x1, int y1)
        {
            int cells = com.oddlabs.tt.landscape.HeightMap.GRID_UNITS_PER_PATCH;
            int patches = size / cells;
            float meters_per_cell = com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID;
            int px0 = java.lang.Math.max(0, (x0 - 1) / cells);
            int py0 = java.lang.Math.max(0, (y0 - 1) / cells);
            int px1 = java.lang.Math.min(patches - 1, x1 / cells);
            int py1 = java.lang.Math.min(patches - 1, y1 / cells);
            for (int py = py0; py <= py1; py++)
            {
                for (int px = px0; px <= px1; px++)
                {
                    com.oddlabs.tt.landscape.LandscapeLeaf leaf = height_map.getLeafFromCoordinates((float)((float)(((float)(px + 0.5f)) * cells) * meters_per_cell), (float)((float)(((float)(py + 0.5f)) * cells) * meters_per_cell));
                    int min_x = (-1);
                    int min_y = (-1);
                    int max_x = (-1);
                    int max_y = (-1);
                    float min = java.lang.Float.POSITIVE_INFINITY;
                    float max = java.lang.Float.NEGATIVE_INFINITY;
                    int cx1 = java.lang.Math.min(size - 1, (px + 1) * cells);
                    int cy1 = java.lang.Math.min(size - 1, (py + 1) * cells);
                    for (int y = py * cells; y <= cy1; y++)
                    {
                        for (int x = px * cells; x <= cx1; x++)
                        {
                            float h = heights[y][x];
                            if (h < min)
                            {
                                min = h;
                                min_x = x;
                                min_y = y;
                            }
                            if (h > max)
                            {
                                max = h;
                                max_x = x;
                                max_y = y;
                            }
                        }
                    }
                    if (min < leaf.bmin_z)
                    {
                        height_map.editHeight(min_x, min_y, min);
                    }
                    if (max > leaf.bmax_z)
                    {
                        height_map.editHeight(max_x, max_y, max);
                    }
                }
            }
        }
        private void init_TerrainEditor()
        {
            this.undo_steps = new java.util.ArrayDeque();
            this.redo_steps = new java.util.ArrayDeque();
            this.shape = com.oddlabs.tt.mapeditor.TerrainEditor.Shape.CIRCLE;
            this.dirty_x0 = java.lang.Integer.MAX_VALUE;
            this.dirty_y0 = java.lang.Integer.MAX_VALUE;
            this.dirty_x1 = java.lang.Integer.MIN_VALUE;
            this.dirty_y1 = java.lang.Integer.MIN_VALUE;
        }
        public TerrainEditor(com.oddlabs.tt.landscape.HeightMap height_map, float[][] heights, com.oddlabs.tt.mapeditor.TerrainEditor.ChangeListener listener) : base()
        {
            init_TerrainEditor();
            this.height_map = height_map;
            this.listener = listener;
            this.heights = heights;
            this.size = heights.Length;
            this.upload = org.lwjgl.BufferUtils.createFloatBuffer(size * size);
            this.scratch = java.lang.JA.newArray2<float>(size, size);
        }
    }
}
