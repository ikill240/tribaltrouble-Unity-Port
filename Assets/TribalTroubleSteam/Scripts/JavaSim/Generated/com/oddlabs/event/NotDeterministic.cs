// Translated from NotDeterministic.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.@event
{
    // Java: 8
    public class NotDeterministic : com.oddlabs.@event.Deterministic
    {
        // Java: 9
        public override bool isPlayback()
        {
            return false;
        }
        // Java: 14
        public override void endLog()
        {
        }
        // Java: 18
        public override sbyte log(sbyte b, sbyte def)
        {
            return b;
        }
        // Java: 23
        public override char log(char c, char def)
        {
            return c;
        }
        // Java: 28
        public override int log(int i, int def)
        {
            return i;
        }
        // Java: 33
        public override long log(long l, long def)
        {
            return l;
        }
        // Java: 38
        public override float log(float f, float def)
        {
            return f;
        }
        // Java: 43
        public override object logObject(object o)
        {
            return o;
        }
        // Java: 48
        public override void logBuffer(java.nio.ByteBuffer b)
        {
            b.position(b.limit());
        }
        // Java: 53
        public override java.nio.file.Path log(java.nio.file.Path p, java.nio.file.Path def)
        {
            return p;
        }
        public NotDeterministic() : base()
        {
        }
    }
}
