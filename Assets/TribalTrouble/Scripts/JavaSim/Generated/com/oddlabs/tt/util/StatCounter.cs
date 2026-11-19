// Translated from StatCounter.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.util
{
    // Java: 5
    public class StatCounter : java.lang.Object
    {
        public long[] values;
        public long old_val;
        public long sum;
        public int position;
        // Java: 15
        public void updateDelta(long val)
        {
            long diff = val - old_val;
            old_val = val;
            updateAbsolute(diff);
        }
        // Java: 21
        public void updateAbsolute(long val)
        {
            sum += val - values[position];
            values[position] = val;
            position = (position + 1) % values.Length;
        }
        // Java: 27
        public long getMax()
        {
            long max = java.lang.Long.MIN_VALUE;
            foreach (long @value in values)
            {
                if (@value > max)
                {
                    max = @value;
                }
            }
            return max;
        }
        // Java: 36
        public long getAveragePerUpdateFloored()
        {
            return sum / values.Length;
        }
        // Java: 40
        public float getAveragePerUpdate()
        {
            return (float)(((float)(sum)) / values.Length);
        }
        private void init_StatCounter()
        {
            this.old_val = 0;
            this.sum = 0;
            this.position = 0;
        }
        public StatCounter(int average_count) : base()
        {
            init_StatCounter();
            values = new long[average_count];
        }
    }
}
