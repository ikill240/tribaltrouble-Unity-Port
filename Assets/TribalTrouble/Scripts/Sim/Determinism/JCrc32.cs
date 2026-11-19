namespace TribalTrouble.Sim.Determinism
{
    /// <summary>java.util.zip.CRC32 (same polynomial and init/final xor).</summary>
    public sealed class JCrc32
    {
        static readonly uint[] Table = BuildTable();
        uint _crc = 0xFFFFFFFFu;

        static uint[] BuildTable()
        {
            var t = new uint[256];
            for (uint n = 0; n < 256; n++)
            {
                uint c = n;
                for (int k = 0; k < 8; k++)
                    c = (c & 1) != 0 ? 0xEDB88320u ^ (c >> 1) : c >> 1;
                t[n] = c;
            }
            return t;
        }

        public void update(int b)
        {
            _crc = Table[(_crc ^ (uint)b) & 0xFF] ^ (_crc >> 8);
        }

        public void update(byte[] data, int offset, int length)
        {
            for (int i = offset; i < offset + length; i++)
                update(data[i]);
        }

        /// <summary>Four bytes of an int, big-endian (as ByteBuffer.putInt / putFloat would lay them out).</summary>
        public void updateInt(int v)
        {
            update((v >> 24) & 0xFF);
            update((v >> 16) & 0xFF);
            update((v >> 8) & 0xFF);
            update(v & 0xFF);
        }

        public long getValue() => _crc ^ 0xFFFFFFFFu;

        public void reset() => _crc = 0xFFFFFFFFu;
    }
}
