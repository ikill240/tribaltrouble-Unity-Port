// Translated from Hill.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.procedural
{
    // Java: 7
    public class Hill : java.lang.Object
    {
        public com.oddlabs.procedural.Channel channel;
        public int size;
        public const int CIRCLE = 1;
        public const int OVAL = 2;
        public const int SQUARE = 3;
        // Java: 33
        public void circle()
        {
            com.oddlabs.procedural.Channel quarter = new com.oddlabs.procedural.Channel(size >> 1, size >> 1);
            for (int y = 0; y < size >> 1; y++)
            {
                float y_coord = (float)(((float)(y)) / size);
                float dy = (float)(y_coord - 0.5f);
                for (int x = 0; x < size >> 1; x++)
                {
                    float x_coord = (float)(((float)(x)) / size);
                    float dx = (float)(x_coord - 0.5f);
                    float dist = java.lang.Math.min(1.0f, (float)(4 * ((float)((float)(dx * dx) + (float)(dy * dy)))));
                    quarter.putPixel(x, y, (float)(1.0f - dist));
                }
            }
            channel = new com.oddlabs.procedural.Channel(size, size);
            channel.quadJoin(quarter, quarter.copy().rotate(270), quarter.copy().rotate(90), quarter.copy().rotate(180));
        }
        // Java: 49
        public void oval()
        {
            com.oddlabs.procedural.Channel quarter = new com.oddlabs.procedural.Channel(size >> 1, size >> 1);
            for (int y = 0; y < size >> 1; y++)
            {
                float y_coord = (float)(((float)(y)) / size);
                float dy = java.lang.Math.abs((float)(y_coord - 0.5f));
                for (int x = 0; x < size >> 1; x++)
                {
                    float x_coord = (float)(((float)(x)) / size);
                    float dx = java.lang.Math.abs((float)(x_coord - 0.5f));
                    float dist = java.lang.Math.min(1.0f, (float)(8 * ((float)((float)((float)(dx * dx) * dx) + (float)((float)(dy * dy) * dy)))));
                    quarter.putPixel(x, y, (float)(1.0f - dist));
                }
            }
            channel = new com.oddlabs.procedural.Channel(size, size);
            channel.quadJoin(quarter, quarter.copy().rotate(270), quarter.copy().rotate(90), quarter.copy().rotate(180));
        }
        // Java: 65
        public void square()
        {
            com.oddlabs.procedural.Channel quarter = new com.oddlabs.procedural.Channel(size >> 1, size >> 1);
            for (int y = 0; y < size >> 1; y++)
            {
                float y_coord = (float)(((float)(y)) / size);
                float dy = (float)(y_coord - 0.5f);
                for (int x = 0; x < size >> 1; x++)
                {
                    float x_coord = (float)(((float)(x)) / size);
                    float dx = (float)(x_coord - 0.5f);
                    float dx2 = (float)(dx * dx);
                    float dy2 = (float)(dy * dy);
                    float dist = java.lang.Math.min(1.0f, (float)(16 * ((float)((float)(dx2 * dx2) + (float)(dy2 * dy2)))));
                    quarter.putPixel(x, y, (float)(1.0f - dist));
                }
            }
            channel = new com.oddlabs.procedural.Channel(size, size);
            channel.quadJoin(quarter, quarter.copy().rotate(270), quarter.copy().rotate(90), quarter.copy().rotate(180));
        }
        // Java: 83
        public com.oddlabs.procedural.Layer toLayer()
        {
            return new com.oddlabs.procedural.Layer(channel, channel.copy(), channel.copy());
        }
        // Java: 87
        public com.oddlabs.procedural.Channel toChannel()
        {
            return channel;
        }
        public Hill(int size, int shape) : base()
        {
            this.size = size;
            switch ((shape))
            {
                case 1:
                {
                    circle();
                    break;
                }
                case 2:
                {
                    oval();
                    break;
                }
                case 3:
                {
                    square();
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
