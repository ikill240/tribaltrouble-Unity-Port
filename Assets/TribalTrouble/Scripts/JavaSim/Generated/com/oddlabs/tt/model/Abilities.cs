// Translated from Abilities.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 6
    public class Abilities : java.lang.Object
    {
        public const int NONE = 0;
        public const int BUILD = 1;
        public const int ATTACK = 2;
        public const int HARVEST = 4;
        public const int SUPPLY_CONTAINER = 8;
        public const int BUILD_ARMIES = 16;
        public const int REPRODUCE = 32;
        public const int TARGET = 64;
        public const int THROW = 128;
        public const int RALLY_TO = 256;
        public const int MAGIC = 512;
        public const int SAIL = 1024;
        public const int MELEE = 2048;
        public int abilities;
        // Java: 40
        public bool hasAbilities(int abilities)
        {
            return (this.abilities | abilities) == this.abilities;
        }
        // Java: 45
        public bool isWarrior()
        {
            return hasAbilities(com.oddlabs.tt.model.Abilities.THROW) || hasAbilities(com.oddlabs.tt.model.Abilities.MELEE);
        }
        // Java: 49
        public void addAbilities(com.oddlabs.tt.model.Abilities abilities)
        {
            addAbilities(abilities.abilities);
        }
        // Java: 53
        public void addAbilities(int abilities)
        {
            this.abilities = this.abilities | abilities;
        }
        // Java: 57
        public void removeAbilities(com.oddlabs.tt.model.Abilities abilities)
        {
            removeAbilities(abilities.abilities);
        }
        // Java: 61
        public void removeAbilities(int abilities)
        {
            this.abilities = this.abilities & ~abilities;
        }
        public Abilities(int abilities) : base()
        {
            this.abilities = abilities;
        }
    }
}
