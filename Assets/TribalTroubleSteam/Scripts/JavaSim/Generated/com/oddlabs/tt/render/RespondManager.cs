// Translated from RespondManager.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.render
{
    // Java: 13
    public class RespondManager : java.lang.Object, com.oddlabs.tt.animation.Animated
    {
        public const float SECONDS_PER_PICK_RESPOND = 0.33333334f;
        public java.util.NavigableMap respond_timeouts;
        public java.util.Map respond_targets;
        public int current_id;
        public float time;
        // Java: 27
        public void animate(float t)
        {
            time += t;
            timeout();
        }
        // Java: 33
        public void timeout()
        {
            com.oddlabs.tt.render.RespondManager.Timeout head_timeout = default;
            while (!respond_timeouts.isEmpty() && (head_timeout = ((com.oddlabs.tt.render.RespondManager.Timeout)respond_timeouts.firstKey())).timeout_f <= time)
            {
                removeResponder(head_timeout.target_f);
            }
        }
        // Java: 40
        public void addResponder(object target)
        {
            addResponder(target, null);
        }
        // Java: 44
        public void addResponder(object target, java.lang.Runnable stop_action)
        {
            addResponder(com.oddlabs.tt.render.RespondManager.SECONDS_PER_PICK_RESPOND, target, null);
        }
        // Java: 48
        public void addResponder(float respond_time, object target, java.lang.Runnable stop_action)
        {
            removeResponder(target);
            com.oddlabs.tt.render.RespondManager.Timeout timeout_v = new com.oddlabs.tt.render.RespondManager.Timeout((float)(time + respond_time), current_id++, target, stop_action);
            respond_targets.put(target, timeout_v);
            respond_timeouts.put(timeout_v, target);
        }
        // Java: 55
        public void removeResponder(object target)
        {
            com.oddlabs.tt.render.RespondManager.Timeout timeout_v = ((com.oddlabs.tt.render.RespondManager.Timeout)respond_targets.remove(target));
            if (timeout_v != null)
            {
                respond_timeouts.remove(timeout_v);
                if (timeout_v.stop_action_f != null)
                {
                    timeout_v.stop_action_f.run();
                }
            }
        }
        // Java: 64
        public bool isResponding(object target)
        {
            if (respond_targets.isEmpty())
            {
                return false;
            }
            else
            {
                return isResponding(((com.oddlabs.tt.render.RespondManager.Timeout)respond_targets.get(target)));
            }
        }
        // Java: 71
        public bool isResponding(com.oddlabs.tt.render.RespondManager.Timeout timeout_v)
        {
            if (timeout_v == null)
            {
                return false;
            }
            float time_diff = (float)(timeout_v.timeout_f - time);
            float blink = (float)(com.oddlabs.tt.render.RespondManager.SECONDS_PER_PICK_RESPOND / 4.0f);
            return time_diff > 0 && (time_diff >= (float)(com.oddlabs.tt.render.RespondManager.SECONDS_PER_PICK_RESPOND - blink) || time_diff <= blink);
        }
        // Java: 79
        public class Timeout : java.lang.Object, java.lang.Comparable
        {
            public float timeout() { return timeout_f; }
            public int id() { return id_f; }
            public object target() { return target_f; }
            public java.lang.Runnable stop_action() { return stop_action_f; }
            public Timeout(float timeout, int id, object target, java.lang.Runnable stop_action)
            {
                this.timeout_f = timeout;
                this.id_f = id;
                this.target_f = target;
                this.stop_action_f = stop_action;
            }
            public override string toString() { return "Timeout[" + "timeout=" + java.lang.JO.toString(timeout_f) + ", id=" + java.lang.JO.toString(id_f) + ", target=" + java.lang.JO.toString(target_f) + ", stop_action=" + java.lang.JO.toString(stop_action_f) + "]"; }
            public float timeout_f;
            public int id_f;
            public object target_f;
            public java.lang.Runnable stop_action_f;
            // Java: 82
            public override bool equals(object other)
            {
                if ((other is com.oddlabs.tt.render.RespondManager.Timeout timeout_obj))
                {
                    return timeout_obj.timeout_f == timeout_f && timeout_obj.id_f == id_f;
                }
                return false;
            }
            // Java: 90
            public override int hashCode()
            {
                int hash = 3;
                hash = 59 * hash + java.lang.Float.floatToIntBits(this.timeout_f);
                hash = 59 * hash + this.id_f;
                return hash;
            }
            // Java: 98
            public int compareTo(object other_p)
            {
                com.oddlabs.tt.render.RespondManager.Timeout other = (com.oddlabs.tt.render.RespondManager.Timeout)other_p;
                float diff = (float)(timeout_f - other.timeout_f);
                return (diff != 0.0f ? java.lang.JC.f2i(diff) : id_f - other.id_f);
            }
        }
        private void init_RespondManager()
        {
            this.respond_timeouts = new java.util.TreeMap();
            this.respond_targets = new java.util.HashMap();
        }
        public RespondManager(com.oddlabs.tt.animation.AnimationManager manager) : base()
        {
            init_RespondManager();
            manager.registerAnimation(this);
        }
        // mixin of default method Animated.updateChecksum
        public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
        }
    }
}
