// Translated from SelectionArmy.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.viewer
{
    // Java: 12
    public class SelectionArmy : com.oddlabs.tt.model.Army
    {
        public com.oddlabs.tt.player.Player local_player;
        public int num_units;
        public int num_builders;
        public com.oddlabs.tt.model.Unit chieftain;
        public com.oddlabs.tt.model.Building building;
        // Java: 24
        public int getNumBuilders()
        {
            return num_builders;
        }
        // Java: 28
        public int getNumUnits()
        {
            return num_units;
        }
        // Java: 32
        public com.oddlabs.tt.model.Unit getChieftain()
        {
            return chieftain;
        }
        // Java: 36
        public com.oddlabs.tt.model.Building getBuilding()
        {
            return building;
        }
        // Java: 40
        public void update()
        {
            num_units = 0;
            num_builders = 0;
            chieftain = null;
            building = null;
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(getSet()))
            {
                if (((object)s.getOwner() != (object)local_player))
                {
                    continue;
                }
                com.oddlabs.tt.model.Abilities abilities = s.getAbilities();
                if (abilities.hasAbilities(com.oddlabs.tt.model.Abilities.BUILD))
                {
                    num_builders++;
                }
                else if (abilities.hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC))
                {
                    chieftain = ((com.oddlabs.tt.model.Unit)s);
                }
                if ((s is com.oddlabs.tt.model.Building building1))
                {
                    building = building1;
                }
                else
                {
                    num_units++;
                }
            }
        }
        // Java: 61
        public override void clear()
        {
            base.clear();
            update();
        }
        // Java: 67
        public override void remove(com.oddlabs.tt.model.Selectable selectable)
        {
            base.remove(selectable);
            update();
        }
        // Java: 73
        public override void add(com.oddlabs.tt.model.Selectable selectable)
        {
            base.add(selectable);
            update();
        }
        public SelectionArmy(com.oddlabs.tt.player.Player local_player) : base()
        {
            this.local_player = local_player;
        }
    }
}
