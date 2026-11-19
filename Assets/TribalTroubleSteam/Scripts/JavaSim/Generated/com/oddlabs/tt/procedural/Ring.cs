// Translated from Ring.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.procedural
{
    // Java: 8
    public class Ring : java.lang.Object
    {
        public com.oddlabs.procedural.Channel channel;
        // Java: 11
        public sealed class Interpolation : java.lang.Enum
        {
            public static Interpolation[] values() { return (Interpolation[])VALUES_.Clone(); }
            public static Interpolation valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly Interpolation LINEAR = new Interpolation("LINEAR", 0);
            public static readonly Interpolation SMOOTH = new Interpolation("SMOOTH", 1);
            public static readonly Interpolation[] VALUES_ = new Interpolation[] { LINEAR, SMOOTH };
            public Interpolation(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        // Java: 61
        public com.oddlabs.procedural.Layer toLayer()
        {
            return new com.oddlabs.procedural.Layer(channel, channel.copy(), channel.copy());
        }
        // Java: 65
        public com.oddlabs.procedural.Channel toChannel()
        {
            return channel;
        }
        public Ring(int width, int height, float[][] gradient_list, com.oddlabs.tt.procedural.Ring.Interpolation interpolation) : base()
        {
            com.oddlabs.procedural.Channel quarter = new com.oddlabs.procedural.Channel(width >> 1, height >> 1);
            float x_coord = default;
            float y_coord = default;
            float radius = default;
            int index = default;
            int index_max = gradient_list.Length - 1;
            float @value = 0;
            for (int x = 0; x < width >> 1; x++)
            {
                for (int y = 0; y < height >> 1; y++)
                {
                    x_coord = (float)(0.5f - (float)(((float)(x + 0.5f)) / width));
                    y_coord = (float)(0.5f - (float)(((float)(y + 0.5f)) / height));
                    radius = ((float)(java.lang.Math.sqrt((float)((float)(x_coord * x_coord) + (float)(y_coord * y_coord)))));
                    index = 0;
                    while (radius >= gradient_list[index][0] && index < index_max)
                    {
                        index++;
                    }
                    if (radius < gradient_list[0][0])
                    {
                        @value = gradient_list[0][1];
                    }
                    else
                    {
                        if (radius >= gradient_list[index_max][0])
                        {
                            @value = gradient_list[index_max][1];
                        }
                        else
                        {
                            @value = ((interpolation).ordinal() switch { 0 /*LINEAR*/ => (float)(com.oddlabs.procedural.Tools.interpolateLinear(gradient_list[index - 1][1], gradient_list[index][1], (float)(((float)(radius - gradient_list[index - 1][0])) / ((float)(gradient_list[index][0] - gradient_list[index - 1][0]))))), 1 /*SMOOTH*/ => (float)(com.oddlabs.procedural.Tools.interpolateSmooth(gradient_list[index - 1][1], gradient_list[index][1], (float)(((float)(radius - gradient_list[index - 1][0])) / ((float)(gradient_list[index][0] - gradient_list[index - 1][0]))))), _ => throw new System.InvalidOperationException("MatchException") });
                        }
                    }
                    quarter.putPixel(x, y, @value);
                }
            }
            channel = new com.oddlabs.procedural.Channel(width, height);
            channel.quadJoin(quarter, quarter.copy().rotate(270), quarter.copy().rotate(90), quarter.copy().rotate(180));
        }
    }
}
