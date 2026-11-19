// Translated from Controller.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.behaviour
{
    // Java: 7
    public abstract class Controller : java.lang.Object
    {
        public const int MAX_TRIES = 1;
        public int[] give_up_counters;
        // Java: 15
        public void resetGiveUpCounters()
        {
            java.util.Arrays.fill(give_up_counters, 0);
        }
        // Java: 19
        public void resetGiveUpCounter(int state_index)
        {
            give_up_counters[state_index] = 0;
        }
        // Java: 23
        public bool shouldGiveUp(int state_index)
        {
            if (give_up_counters[state_index] != com.oddlabs.tt.model.behaviour.Controller.MAX_TRIES)
            {
                give_up_counters[state_index]++;
                return false;
            }
            else
            {
                return true;
            }
        }
        // Java: 42
        public virtual bool onStuck()
        {
            return false;
        }
        // Java: 46
        public virtual string getKey()
        {
            return this.GetType().getName();
        }
        // Java: 50
        public abstract void decide();
        public Controller(int num_states) : base()
        {
            give_up_counters = new int[num_states];
        }
    }
}
