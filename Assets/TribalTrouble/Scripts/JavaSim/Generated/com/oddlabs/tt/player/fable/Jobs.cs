// Translated from Jobs.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.fable
{
    // Java: 20
    public class Jobs : java.lang.Object
    {
        // Java: 21
        public sealed class Kind : java.lang.Enum
        {
            public static Kind[] values() { return (Kind[])VALUES_.Clone(); }
            public static Kind valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly Kind NONE = new Kind("NONE", 0);
            public static readonly Kind GATHER = new Kind("GATHER", 1);
            public static readonly Kind BUILD = new Kind("BUILD", 2);
            public static readonly Kind ENTER = new Kind("ENTER", 3);
            public static readonly Kind ATTACK_MOVE = new Kind("ATTACK_MOVE", 4);
            public static readonly Kind MOVE = new Kind("MOVE", 5);
            public static readonly Kind DEFEND = new Kind("DEFEND", 6);
            public static readonly Kind HUNT = new Kind("HUNT", 7);
            public static readonly Kind GARRISON = new Kind("GARRISON", 8);
            public static readonly Kind HOLD = new Kind("HOLD", 9);
            public static readonly Kind[] VALUES_ = new Kind[] { NONE, GATHER, BUILD, ENTER, ATTACK_MOVE, MOVE, DEFEND, HUNT, GARRISON, HOLD };
            public Kind(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        // Java: 35
        public class Job : java.lang.Object
        {
            public com.oddlabs.tt.player.fable.Jobs.Kind kind;
            public int gx;
            public int gy;
            public com.oddlabs.tt.model.Selectable target;
            public com.oddlabs.tt.player.fable.ResourceMap.Node node;
            public float issued_at;
            public int tag;
            // Java: 56
            public bool sameAs(com.oddlabs.tt.player.fable.Jobs.Kind kind, int gx, int gy, com.oddlabs.tt.model.Selectable target, com.oddlabs.tt.player.fable.ResourceMap.Node node)
            {
                return ((object)this.kind == (object)kind) && this.gx == gx && this.gy == gy && ((object)this.target == (object)target) && ((object)this.node == (object)node);
            }
            // Java: 61
            public com.oddlabs.tt.model.LandBuilding building()
            {
                return ((target is com.oddlabs.tt.model.LandBuilding b) ? b : (com.oddlabs.tt.model.LandBuilding)(null));
            }
            // Java: 65
            public override string toString()
            {
                return java.lang.JStr.valueOf(kind) + java.lang.JStr.valueOf(((node != null ? " " + java.lang.JStr.valueOf(node) : ""))) + java.lang.JStr.valueOf(((target != null ? " " + java.lang.JStr.valueOf(target) : ""))) + java.lang.JStr.valueOf(((((object)kind == (object)com.oddlabs.tt.player.fable.Jobs.Kind.ATTACK_MOVE) || ((object)kind == (object)com.oddlabs.tt.player.fable.Jobs.Kind.MOVE) || ((object)kind == (object)com.oddlabs.tt.player.fable.Jobs.Kind.DEFEND) ? " " + (gx) + "," + (gy) : ""))) + " tag=" + (tag);
            }
            public Job(com.oddlabs.tt.player.fable.Jobs.Kind kind, int gx, int gy, com.oddlabs.tt.model.Selectable target, com.oddlabs.tt.player.fable.ResourceMap.Node node, float issued_at, int tag) : base()
            {
                this.kind = kind;
                this.gx = gx;
                this.gy = gy;
                this.target = target;
                this.node = node;
                this.issued_at = issued_at;
                this.tag = tag;
            }
        }
        public java.util.Map jobs;
        // Java: 74
        public com.oddlabs.tt.player.fable.Jobs.Job get(com.oddlabs.tt.model.Unit unit)
        {
            return ((com.oddlabs.tt.player.fable.Jobs.Job)jobs.get(unit));
        }
        // Java: 78
        public com.oddlabs.tt.player.fable.Jobs.Kind kindOf(com.oddlabs.tt.model.Unit unit)
        {
            com.oddlabs.tt.player.fable.Jobs.Job j = ((com.oddlabs.tt.player.fable.Jobs.Job)jobs.get(unit));
            return (j == null ? com.oddlabs.tt.player.fable.Jobs.Kind.NONE : j.kind);
        }
        // Java: 83
        public bool has(com.oddlabs.tt.model.Unit unit)
        {
            return jobs.containsKey(unit);
        }
        // Java: 88
        public com.oddlabs.tt.player.fable.Jobs.Job set(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.player.fable.Jobs.Kind kind, int gx, int gy, com.oddlabs.tt.model.Selectable target, com.oddlabs.tt.player.fable.ResourceMap.Node node, float now, int tag)
        {
            com.oddlabs.tt.player.fable.Jobs.Job job = new com.oddlabs.tt.player.fable.Jobs.Job(kind, gx, gy, target, node, now, tag);
            jobs.put(unit, job);
            return job;
        }
        // Java: 95
        public void clear(com.oddlabs.tt.model.Unit unit)
        {
            jobs.remove(unit);
        }
        // Java: 100
        public void prune()
        {
            java.util.Iterator it = jobs.entrySet().iterator();
            while (it.hasNext())
            {
                java.util.Map.Entry e = ((java.util.Map.Entry)it.next());
                com.oddlabs.tt.model.Unit u = ((com.oddlabs.tt.model.Unit)e.getKey());
                com.oddlabs.tt.player.fable.Jobs.Job j = ((com.oddlabs.tt.player.fable.Jobs.Job)e.getValue());
                if (u.isDead() || u.isMounted() || (j.target != null && j.target.isDead()))
                {
                    it.remove();
                }
            }
        }
        // Java: 112
        public int count(com.oddlabs.tt.player.fable.Jobs.Kind kind)
        {
            int n = 0;
            foreach (com.oddlabs.tt.player.fable.Jobs.Job j in java.lang.JIter.of(jobs.values()))
            {
                if (((object)j.kind == (object)kind))
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 120
        public int count(com.oddlabs.tt.player.fable.Jobs.Kind kind, int tag)
        {
            int n = 0;
            foreach (com.oddlabs.tt.player.fable.Jobs.Job j in java.lang.JIter.of(jobs.values()))
            {
                if (((object)j.kind == (object)kind) && j.tag == tag)
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 129
        public java.util.List units(com.oddlabs.tt.player.fable.Jobs.Kind kind, int tag)
        {
            java.util.List result = new java.util.ArrayList();
            foreach (java.util.Map.Entry e in java.lang.JIter.of(jobs.entrySet()))
            {
                if (((object)((com.oddlabs.tt.player.fable.Jobs.Job)e.getValue()).kind == (object)kind) && (tag < 0 || ((com.oddlabs.tt.player.fable.Jobs.Job)e.getValue()).tag == tag))
                {
                    result.add(((com.oddlabs.tt.model.Unit)e.getKey()));
                }
            }
            return result;
        }
        // Java: 138
        public java.util.List unitsOn(com.oddlabs.tt.model.Selectable target)
        {
            java.util.List result = new java.util.ArrayList();
            foreach (java.util.Map.Entry e in java.lang.JIter.of(jobs.entrySet()))
            {
                if (((object)((com.oddlabs.tt.player.fable.Jobs.Job)e.getValue()).target == (object)target))
                {
                    result.add(((com.oddlabs.tt.model.Unit)e.getKey()));
                }
            }
            return result;
        }
        // Java: 147
        public int gatherersOn(com.oddlabs.tt.player.fable.ResourceMap.Node node)
        {
            int n = 0;
            foreach (com.oddlabs.tt.player.fable.Jobs.Job j in java.lang.JIter.of(jobs.values()))
            {
                if (((object)j.kind == (object)com.oddlabs.tt.player.fable.Jobs.Kind.GATHER) && ((object)j.node == (object)node))
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 155
        public java.lang.Iterable entries()
        {
            return jobs.entrySet();
        }
        // Java: 159
        public int size()
        {
            return jobs.size();
        }
        private void init_Jobs()
        {
            this.jobs = new java.util.LinkedHashMap();
        }
        public Jobs() : base()
        {
            init_Jobs();
        }
    }
}
