using System.IO;

namespace TribalTrouble.Sim.Determinism
{
    /// <summary>java.io.DataInputStream: big-endian primitives.</summary>
    public sealed class JDataInput
    {
        readonly Stream _s;
        readonly byte[] _buf = new byte[8];

        public JDataInput(Stream s) { _s = s; }

        void Fill(int n)
        {
            int off = 0;
            while (off < n)
            {
                int r = _s.Read(_buf, off, n - off);
                if (r <= 0) throw new EndOfStreamException();
                off += r;
            }
        }

        public int readInt()
        {
            Fill(4);
            return (_buf[0] << 24) | (_buf[1] << 16) | (_buf[2] << 8) | _buf[3];
        }

        public long readLong()
        {
            long hi = (uint)readInt();
            long lo = (uint)readInt();
            return (hi << 32) | lo;
        }

        public float readFloat() => J.IntBitsToFloat(readInt());
        public double readDouble() => System.BitConverter.Int64BitsToDouble(readLong());

        public byte readUnsignedByte()
        {
            Fill(1);
            return _buf[0];
        }

        public sbyte readByte() => unchecked((sbyte)readUnsignedByte());
        public bool readBoolean() => readUnsignedByte() != 0;

        public short readShort()
        {
            Fill(2);
            return (short)((_buf[0] << 8) | _buf[1]);
        }
    }
}
