// Translated from SaveDeterministic.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.@event
{
    // Java: 20
    public class SaveDeterministic : com.oddlabs.@event.Deterministic
    {
        public const short MAX_DEFAULTS = unchecked((short)32767);
        public java.nio.channels.ByteChannel channel;
        public java.nio.ByteBuffer buffer;
        public com.oddlabs.@event.SaveDeterministic.ByteBufferOutputStream byte_buffer_output_stream;
        public int total_bytes_written;
        public short num_defaults;
        // Java: 41
        public override bool isPlayback()
        {
            throw new System.NotSupportedException("J2CS stub: SaveDeterministic.isPlayback");
        }
        // Java: 46
        public void flushBuffer()
        {
            throw new System.NotSupportedException("J2CS stub: SaveDeterministic.flushBuffer");
        }
        // Java: 58
        public override void endLog()
        {
            throw new System.NotSupportedException("J2CS stub: SaveDeterministic.endLog");
        }
        // Java: 70
        public bool startLog(int num_bytes, bool def)
        {
            throw new System.NotSupportedException("J2CS stub: SaveDeterministic.startLog");
        }
        // Java: 86
        public override sbyte log(sbyte b, sbyte def)
        {
            throw new System.NotSupportedException("J2CS stub: SaveDeterministic.log");
        }
        // Java: 93
        public override char log(char c, char def)
        {
            throw new System.NotSupportedException("J2CS stub: SaveDeterministic.log");
        }
        // Java: 100
        public override int log(int i, int def)
        {
            throw new System.NotSupportedException("J2CS stub: SaveDeterministic.log");
        }
        // Java: 107
        public override long log(long l, long def)
        {
            throw new System.NotSupportedException("J2CS stub: SaveDeterministic.log");
        }
        // Java: 114
        public override float log(float f, float def)
        {
            throw new System.NotSupportedException("J2CS stub: SaveDeterministic.log");
        }
        // Java: 121
        public override object logObject(object o)
        {
            throw new System.NotSupportedException("J2CS stub: SaveDeterministic.logObject");
        }
        // Java: 131
        public override java.nio.file.Path log(java.nio.file.Path p, java.nio.file.Path def)
        {
            throw new System.NotSupportedException("J2CS stub: SaveDeterministic.log");
        }
        // Java: 141
        public override void logBuffer(java.nio.ByteBuffer b)
        {
            throw new System.NotSupportedException("J2CS stub: SaveDeterministic.logBuffer");
        }
        // Java: 156
        public class ByteBufferOutputStream : java.io.OutputStream
        {
            public com.oddlabs.@event.SaveDeterministic this_0;
            // Java: 157
            public override void write(int b)
            {
                this_0.log(unchecked((sbyte)(b)));
            }
            public ByteBufferOutputStream(com.oddlabs.@event.SaveDeterministic this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        public SaveDeterministic(java.nio.file.Path logging_file) { }
    }
}
