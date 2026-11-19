// Ported from Java (Tribal Trouble 1.4.1). Mechanically transliterated by PortTools/j2cs.py,
// then reviewed by hand. Java identifiers are kept so this file diffs against its source.
using System;
using System.Collections.Generic;
using TribalTrouble.Sim.Determinism;
using TribalTrouble.Sim;

namespace TribalTrouble.Sim.Procedural
{
    public sealed class Hill {
        private Channel channel;
        private readonly int size;

        public const int CIRCLE = 1;
        public const int OVAL = 2;
        public const int SQUARE = 3;

        public Hill(int size, int shape) {
            this.size = size;
            switch (shape) {
                case CIRCLE:
                    circle();
                    break;
                case OVAL:
                    oval();
                    break;
                case SQUARE:
                    square();
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false);
                    break;
            }
        }

        private void circle() {
            Channel quarter = new Channel(size >> 1, size >> 1);
            for (int y = 0; y < size >> 1; y++) {
                float y_coord = (float)((float) y / size);
                float dy = (float)(y_coord - 0.5f);
                for (int x = 0; x < size >> 1; x++) {
                    float x_coord = (float)((float) x / size);
                    float dx = (float)(x_coord - 0.5f);
                    float dist = J.Min(1f, (float)(4 * ((float)((float)(dx * dx) + (float)(dy * dy)))));
                    quarter.putPixel(x, y, (float)(1f - dist));
                }
            }
            channel = new Channel(size, size);
            channel.quadJoin(quarter, quarter.copy().rotate(270), quarter.copy().rotate(90), quarter.copy().rotate(180));
        }

        private void oval() {
            Channel quarter = new Channel(size >> 1, size >> 1);
            for (int y = 0; y < size >> 1; y++) {
                float y_coord = (float)((float) y / size);
                float dy = J.Abs((float)(y_coord - 0.5f));
                for (int x = 0; x < size >> 1; x++) {
                    float x_coord = (float)((float) x / size);
                    float dx = J.Abs((float)(x_coord - 0.5f));
                    float dist = J.Min(1f, (float)(8 * ((float)((float)((float)(dx * dx) * dx) + (float)((float)(dy * dy) * dy)))));
                    quarter.putPixel(x, y, (float)(1f - dist));
                }
            }
            channel = new Channel(size, size);
            channel.quadJoin(quarter, quarter.copy().rotate(270), quarter.copy().rotate(90), quarter.copy().rotate(180));
        }

        private void square() {
            Channel quarter = new Channel(size >> 1, size >> 1);
            for (int y = 0; y < size >> 1; y++) {
                float y_coord = (float)((float) y / size);
                float dy = (float)(y_coord - 0.5f);
                for (int x = 0; x < size >> 1; x++) {
                    float x_coord = (float)((float) x / size);
                    float dx = (float)(x_coord - 0.5f);
                    float dx2 = (float)(dx * dx);
                    float dy2 = (float)(dy * dy);
                    float dist = J.Min(1f, (float)(16 * ((float)((float)(dx2 * dx2) + (float)(dy2 * dy2)))));
                    quarter.putPixel(x, y, (float)(1f - dist));
                }
            }
            channel = new Channel(size, size);
            channel.quadJoin(quarter, quarter.copy().rotate(270), quarter.copy().rotate(90), quarter.copy().rotate(180));
        }

        public Layer toLayer() {
            return new Layer(channel, channel.copy(), channel.copy());
        }

        public Channel toChannel() {
            return channel;
        }

    }
}
