// Translated from AiLog.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.fable
{
    // Java: 8
    public class AiLog : java.lang.Object
    {
        public string prefix;
        public java.util.function.Supplier clock;
        // Java: 17
        public bool enabled()
        {
            return com.oddlabs.tt.player.fable.AiDevOptions.isDebug();
        }
        // Java: 21
        public void info(string message)
        {
            if (enabled())
            {
                java.lang.IO.println(java.lang.JString.format("[AI %s %6.1fs] %s", prefix, ((float?)clock.get()), message));
            }
        }
        // Java: 27
        public void info(java.util.function.Supplier message)
        {
            if (enabled())
            {
                info(((string)message.get()));
            }
        }
        public AiLog(string prefix, java.util.function.Supplier game_seconds) : base()
        {
            this.prefix = prefix;
            this.clock = game_seconds;
        }
    }
}
