// Translated from StateChecksum.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.util
{
    // Java: 6
    public class StateChecksum : java.lang.Object
    {
        public java.util.zip.Checksum crc;
        // Java: 9
        public void update(long l)
        {
            int i0 = unchecked((int)((l & (-1))));
            int i1 = unchecked((int)(((l >> 32) & (-1))));
            update(i0);
            update(i1);
        }
        // Java: 17
        public void update(int i)
        {
            int b0 = (i >> 24) & 255;
            int b1 = (i >> 16) & 255;
            int b2 = (i >> 8) & 255;
            int b3 = i & 255;
            crc.update(b0);
            crc.update(b1);
            crc.update(b2);
            crc.update(b3);
        }
        // Java: 29
        public void update(float f)
        {
            int f_int = java.lang.Float.floatToRawIntBits(f);
            update(f_int);
        }
        // Java: 35
        public int getValue()
        {
            return unchecked((int)(crc.getValue()));
        }
        private void init_StateChecksum()
        {
            this.crc = new java.util.zip.Adler32();
        }
        public StateChecksum() : base()
        {
            init_StateChecksum();
        }
    }
}
