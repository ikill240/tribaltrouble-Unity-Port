// Translated from LoadDeterministic.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.@event
{
    // Java: 18
    public class LoadDeterministic : com.oddlabs.@event.Deterministic
    {
        public java.nio.channels.ReadableByteChannel channel;
        public java.nio.ByteBuffer buffer;
        public com.oddlabs.@event.LoadDeterministic.ByteBufferInputStream byte_buffer_input_stream;
        public int total_bytes_read;
        public int num_defaults;
        // Java: 41
        public override bool isPlayback()
        {
            throw new System.NotSupportedException("J2CS stub: LoadDeterministic.isPlayback");
        }
        // Java: 46
        public override void endLog()
        {
            throw new System.NotSupportedException("J2CS stub: LoadDeterministic.endLog");
        }
        // Java: 56
        public bool isDefault(int num_bytes)
        {
            throw new System.NotSupportedException("J2CS stub: LoadDeterministic.isDefault");
        }
        // Java: 68
        public void fillBuffer(int num_bytes)
        {
            throw new System.NotSupportedException("J2CS stub: LoadDeterministic.fillBuffer");
        }
        // Java: 74
        public void fillBuffer()
        {
            throw new System.NotSupportedException("J2CS stub: LoadDeterministic.fillBuffer");
        }
        // Java: 79
        public bool tryFillBuffer()
        {
            throw new System.NotSupportedException("J2CS stub: LoadDeterministic.tryFillBuffer");
        }
        // Java: 98
        public bool isEndOfLog()
        {
            throw new System.NotSupportedException("J2CS stub: LoadDeterministic.isEndOfLog");
        }
        // Java: 105
        public void getDefaults()
        {
            throw new System.NotSupportedException("J2CS stub: LoadDeterministic.getDefaults");
        }
        // Java: 112
        public override sbyte log(sbyte b, sbyte def)
        {
            throw new System.NotSupportedException("J2CS stub: LoadDeterministic.log");
        }
        // Java: 123
        public override char log(char c, char def)
        {
            throw new System.NotSupportedException("J2CS stub: LoadDeterministic.log");
        }
        // Java: 134
        public override int log(int i, int def)
        {
            throw new System.NotSupportedException("J2CS stub: LoadDeterministic.log");
        }
        // Java: 145
        public override long log(long l, long def)
        {
            throw new System.NotSupportedException("J2CS stub: LoadDeterministic.log");
        }
        // Java: 156
        public override float log(float f, float def)
        {
            throw new System.NotSupportedException("J2CS stub: LoadDeterministic.log");
        }
        // Java: 167
        public override java.nio.file.Path log(java.nio.file.Path p, java.nio.file.Path def)
        {
            throw new System.NotSupportedException("J2CS stub: LoadDeterministic.log");
        }
        // Java: 177
        public override object logObject(object o)
        {
            throw new System.NotSupportedException("J2CS stub: LoadDeterministic.logObject");
        }
        // Java: 188
        public override void logBuffer(java.nio.ByteBuffer b)
        {
            throw new System.NotSupportedException("J2CS stub: LoadDeterministic.logBuffer");
        }
        // Java: 204
        public class ByteBufferInputStream : java.io.InputStream
        {
            public com.oddlabs.@event.LoadDeterministic this_0;
            // Java: 205
            public override int read()
            {
                sbyte b = this_0.log(unchecked((sbyte)(0)));
                return (unchecked((int)(b))) & 255;
            }
            public ByteBufferInputStream(com.oddlabs.@event.LoadDeterministic this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        public LoadDeterministic(java.nio.file.Path logging_file, bool zipped) { }
    }
}
