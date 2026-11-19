// Translated from Army.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 8
    public class Army : java.lang.Object
    {
        public java.util.Set selection;
        // Java: 11
        public com.oddlabs.tt.model.Selectable[] filter(int ability_filter)
        {
            return ((com.oddlabs.tt.model.Selectable[])((com.oddlabs.tt.model.Selectable[])selection.stream().filter(new java.util.function.Predicate_L((object s_l) =>
            {
                com.oddlabs.tt.model.Selectable s = (com.oddlabs.tt.model.Selectable)s_l;
                return s.getAbilities().hasAbilities(ability_filter);
            })).toArray(new java.util.function.IntFunction_L((int a0) => { return new com.oddlabs.tt.model.Selectable[a0]; }))));
        }
        // Java: 16
        public bool containsAbility(int ability_filter)
        {
            return selection.stream().anyMatch(new java.util.function.Predicate_L((object s_l) =>
            {
                com.oddlabs.tt.model.Selectable s = (com.oddlabs.tt.model.Selectable)s_l;
                return s.getAbilities().hasAbilities(ability_filter);
            }));
        }
        // Java: 20
        public virtual void clear()
        {
            selection.clear();
        }
        // Java: 24
        public virtual void remove(com.oddlabs.tt.model.Selectable selectable)
        {
            selection.remove(selectable);
        }
        // Java: 28
        public bool contains(com.oddlabs.tt.model.Selectable selectable)
        {
            return selection.contains(selectable);
        }
        // Java: 32
        public java.util.Set getSet()
        {
            return selection;
        }
        // Java: 36
        public virtual void add(com.oddlabs.tt.model.Selectable selectable)
        {
            selection.add(selectable);
        }
        // Java: 40
        public int size()
        {
            return selection.size();
        }
        private void init_Army()
        {
            this.selection = new java.util.LinkedHashSet();
        }
        public Army() : base()
        {
            init_Army();
        }
    }
}
