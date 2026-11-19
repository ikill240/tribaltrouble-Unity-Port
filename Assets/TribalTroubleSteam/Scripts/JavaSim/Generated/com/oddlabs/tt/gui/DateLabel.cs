// Translated from DateLabel.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 11
    public class DateLabel : com.oddlabs.tt.gui.Label
    {
        public long when;
        // Java: 24
        public static string format(long date)
        {
            return (date > 0 ? java.time.format.DateTimeFormatter.ofLocalizedDateTime(java.time.format.FormatStyle.MEDIUM, java.time.format.FormatStyle.SHORT).withZone(java.time.ZoneId.systemDefault()).format(java.time.Instant.ofEpochMilli(date)) : "-");
        }
        // Java: 29
        public override int compareTo(object o_p)
        {
            com.oddlabs.tt.gui.Label o = (com.oddlabs.tt.gui.Label)o_p;
            return ((o is com.oddlabs.tt.gui.DateLabel other) ? java.lang.Long.compare(when, other.when) : (-1));
        }
        public DateLabel(long when, com.oddlabs.tt.font.Font font, int width) : base(com.oddlabs.tt.gui.DateLabel.format(when), font, width)
        {
            this.when = when;
        }
        public DateLabel(long when, com.oddlabs.tt.font.Font font) : base(com.oddlabs.tt.gui.DateLabel.format(when), font)
        {
            this.when = when;
        }
    }
}
