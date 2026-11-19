// Float rule for all simulation code: every float arithmetic result is wrapped in (float)(...)
// because Unity's Mono keeps float intermediates in double precision, while Java rounds each
// operation. (PortTools/golden/FloatRounder.java inserts these casts into ported Java automatically.)
// Small support types the ported generator needs. Values are copied from the Java sources
// (global/Globals.java, landscape/HeightMap.java, model/RacesResources.java,
// resource/FogInfo.java, resource/DistanceFogInfo.java, resource/GL*Image.java, org.joml.Vector4f).
using System;

namespace TribalTrouble.Sim
{
    /// <summary>Subset of com.oddlabs.tt.global.Globals used by the simulation.</summary>
    public static class Globals
    {
        public const int LANDSCAPE_SEED = 1;
        public const int STRUCTURE_SIZE = 256;
        public const int DETAIL_SIZE = 256;
        public const float SEA_LEVEL = .1f;
    }

    /// <summary>com.oddlabs.tt.landscape.HeightMap constants.</summary>
    public static class HeightMap
    {
        public const int METERS_PER_UNIT_GRID = 2;
    }

    /// <summary>Building footprint sizes from com.oddlabs.tt.model.RacesResources (1.4.1 values).</summary>
    public static class RacesResources
    {
        public const int QUARTERS_SIZE = 6;
        public const int ARMORY_SIZE = 4;
        public const int TOWER_SIZE = 3;
        public const int SHIP_SIZE = 7;
        public static readonly int MAX_BUILDING_SIZE = Math.Max(QUARTERS_SIZE, Math.Max(ARMORY_SIZE, TOWER_SIZE));
    }

    /// <summary>Generation progress callback (com.oddlabs.tt.form.ProgressForm). Hook it up to a loading bar.</summary>
    public static class ProgressForm
    {
        public static Action<float> OnProgress;
        public static void progress() => OnProgress?.Invoke(0f);
        public static void progress(float fraction) => OnProgress?.Invoke(fraction);
    }

    /// <summary>org.joml.Vector4f subset (x(), y(), z(), w() accessors as in Java).</summary>
    public sealed class Vector4f
    {
        public float X, Y, Z, W;
        public Vector4f(float x, float y, float z, float w) { X = x; Y = y; Z = z; W = w; }
        public float x() => X;
        public float y() => Y;
        public float z() => Z;
        public float w() => W;
    }

    /// <summary>OpenGL enum values the Java code passes around as image formats.</summary>
    public static class GL11
    {
        public const int GL_RED = 0x1903;
        public const int GL_RGBA = 0x1908;
    }

    /// <summary>com.oddlabs.tt.resource.GLImage: a width x height pixel container (no GL here).</summary>
    public abstract class GLImage
    {
        public int width { get; }
        public int height { get; }
        public int format { get; }
        protected GLImage(int width, int height, int format) { this.width = width; this.height = height; this.format = format; }
        public int getWidth() => width;
        public int getHeight() => height;
        public abstract int getPixel(int x, int y);
        public abstract void putPixel(int x, int y, int pixel);
    }

    /// <summary>RGBA8 image, packed exactly like GLIntImage(Layer): (a&lt;&lt;24)|(b&lt;&lt;16)|(g&lt;&lt;8)|r.</summary>
    public sealed class GLIntImage : GLImage
    {
        public readonly int[] pixels;

        public GLIntImage(int width, int height, int format) : base(width, height, format)
        {
            pixels = new int[width * height];
        }

        public GLIntImage(TribalTrouble.Sim.Procedural.Layer layer) : this(layer.getWidth(), layer.getHeight(), GL11.GL_RGBA)
        {
            for (int y = 0; y < getHeight(); y++)
            {
                for (int x = 0; x < getWidth(); x++)
                {
                    int ri = Determinism.J.I((float)((float)(layer.r.getPixel(x, y) * 255) + .5f)) & 0xff;
                    int gi = Determinism.J.I((float)((float)(layer.g.getPixel(x, y) * 255) + .5f)) & 0xff;
                    int bi = Determinism.J.I((float)((float)(layer.b.getPixel(x, y) * 255) + .5f)) & 0xff;
                    int ai = layer.a != null ? Determinism.J.I((float)((float)(layer.a.getPixel(x, y) * 255) + .5f)) & 0xff : 255;
                    putPixel(x, y, (ai << 24) | (bi << 16) | (gi << 8) | ri);
                }
            }
        }

        public override int getPixel(int x, int y) => pixels[y * getWidth() + x];
        public override void putPixel(int x, int y, int pixel) => pixels[y * getWidth() + x] = pixel;
    }

    /// <summary>8-bit single channel image, filled exactly like GLByteImage(Channel, format).</summary>
    public sealed class GLByteImage : GLImage
    {
        public readonly byte[] pixels;

        public GLByteImage(int width, int height, int format) : base(width, height, format)
        {
            pixels = new byte[(width * height + 3) & ~3];
        }

        public GLByteImage(TribalTrouble.Sim.Procedural.Channel channel) : this(channel, GL11.GL_RED) { }

        public GLByteImage(TribalTrouble.Sim.Procedural.Channel channel, int format) : this(channel.getWidth(), channel.getHeight(), format)
        {
            for (int y = 0; y < getHeight(); y++)
            {
                for (int x = 0; x < getWidth(); x++)
                {
                    int pixel = Determinism.J.Round((float)(channel.getPixel(x, y) * 255));
                    putPixel(x, y, Determinism.J.Clamp(pixel, 0, 255));
                }
            }
        }

        public override int getPixel(int x, int y) => pixels[y * getWidth() + x];
        public override void putPixel(int x, int y, int pixel) => pixels[y * getWidth() + x] = unchecked((byte)pixel);
    }

    /// <summary>One terrain texture blend layer (resource/BlendInfo).</summary>
    public abstract class BlendInfo
    {
        readonly GLByteImage alpha;
        protected BlendInfo(GLByteImage alpha) { this.alpha = alpha; }
        public GLByteImage getSourceImage() => alpha;
    }

    public sealed class StructureBlend : BlendInfo
    {
        public readonly GLIntImage structure;
        public readonly GLIntImage normal;
        public StructureBlend(GLIntImage structure_image, GLIntImage normal_image, GLByteImage alpha_image) : base(alpha_image)
        {
            structure = structure_image;
            normal = normal_image;
        }
        public GLIntImage getStructureImage() => structure;
        public GLIntImage getNormalImage() => normal;
    }

    public sealed class BlendLighting : BlendInfo
    {
        readonly float r, g, b;
        public BlendLighting(GLByteImage alpha_image, float r, float g, float b) : base(alpha_image) { this.r = r; this.g = g; this.b = b; }
        public float getR() => r;
        public float getG() => g;
        public float getB() => b;
    }

    public class FogInfo
    {
        public enum Mode { NONE, LINEAR, EXP, EXP2, RADIAL }

        protected readonly Mode mode;
        protected readonly Vector4f color;
        protected readonly float density;
        bool enabled = true;

        public FogInfo(Mode mode, Vector4f color, float density)
        {
            this.mode = mode;
            this.color = color;
            this.density = density;
        }

        public void setEnabled(bool enabled) => this.enabled = enabled;
        public bool isEnabled() => enabled;
        public Vector4f getColor() => color;
        public float getDensity() => density;
        public Mode getMode() => mode;
    }

    public sealed class DistanceFogInfo : FogInfo
    {
        readonly float start, end, height_factor;

        public DistanceFogInfo(Mode mode, Vector4f color, float density, float height_factor, float start, float end)
            : base(mode, color, density)
        {
            this.height_factor = height_factor;
            this.start = start;
            this.end = end;
        }

        public float getStart() => start;
        public float getEnd() => end;
        public float getHeightFactor() => height_factor;
    }
}
