// Translated from Gradient.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.procedural
{
    // Java: 8
    public class Gradient : java.lang.Object
    {
        public com.oddlabs.procedural.Channel channel;
        public const int HORIZONTAL = 1;
        public const int VERTICAL = 2;
        public const int LINEAR = 1;
        public const int SMOOTH = 2;
        public const int POLYNOMIAL = 3;
        // Java: 63
        public com.oddlabs.procedural.Layer toLayer()
        {
            return new com.oddlabs.procedural.Layer(channel, channel, channel);
        }
        // Java: 67
        public com.oddlabs.procedural.Channel toChannel()
        {
            return channel;
        }
        public Gradient(int width, int height, float[][] gradient_list, int orientation, int interpolation) : base()
        {
            channel = new com.oddlabs.procedural.Channel(width, height);
            float x_coord = 0;
            int index = 0;
            int index_max = gradient_list.Length - 1;
            float @value = 0;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    x_coord = (float)(((float)(x)) / width);
                    if (x_coord >= gradient_list[index][0] && index < index_max)
                    {
                        index++;
                    }
                    if (x_coord < gradient_list[0][0])
                    {
                        @value = gradient_list[0][1];
                    }
                    else
                    {
                        if (x_coord >= gradient_list[index_max][0])
                        {
                            @value = gradient_list[index_max][1];
                        }
                        else
                        {
                            switch ((interpolation))
                            {
                                case 1:
                                {
                                    @value = com.oddlabs.procedural.Tools.interpolateLinear(gradient_list[index - 1][1], gradient_list[index][1], (float)(((float)(x_coord - gradient_list[index - 1][0])) / ((float)(gradient_list[index][0] - gradient_list[index - 1][0]))));
                                    break;
                                }
                                case 2:
                                {
                                    @value = com.oddlabs.procedural.Tools.interpolateSmooth(gradient_list[index - 1][1], gradient_list[index][1], (float)(((float)(x_coord - gradient_list[index - 1][0])) / ((float)(gradient_list[index][0] - gradient_list[index - 1][0]))));
                                    break;
                                }
                                default:
                                {
                                    break;
                                }
                            }
                        }
                    }
                    switch ((orientation))
                    {
                        case 1:
                        {
                            channel.putPixel(x, y, @value);
                            break;
                        }
                        case 2:
                        {
                            channel.putPixel(y, x, @value);
                            break;
                        }
                        default:
                        {
                            break;
                        }
                    }
                }
            }
        }
    }
}
