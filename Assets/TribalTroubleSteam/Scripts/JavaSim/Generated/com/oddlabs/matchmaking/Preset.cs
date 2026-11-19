// Translated from Preset.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchmaking
{
    // Java: 16
    public class Preset : java.lang.Object
    {
        public const long serialVersionUID = 3L;
        public string id;
        public string name;
        public com.oddlabs.matchmaking.WorldConfig world;
        public com.oddlabs.matchmaking.GameModeOptions mode_options;
        public com.oddlabs.matchmaking.RosterTemplate roster;
        public bool built_in;
        // Java: 42
        public string getId()
        {
            return id;
        }
        // Java: 46
        public string getName()
        {
            return name;
        }
        // Java: 50
        public com.oddlabs.matchmaking.WorldConfig getWorld()
        {
            return world;
        }
        // Java: 54
        public com.oddlabs.matchmaking.GameMode getMode()
        {
            return mode_options.getMode();
        }
        // Java: 59
        public com.oddlabs.matchmaking.GameModeOptions getModeOptions()
        {
            return mode_options;
        }
        // Java: 63
        public com.oddlabs.matchmaking.RosterTemplate getRoster()
        {
            return roster;
        }
        // Java: 67
        public bool isBuiltIn()
        {
            return built_in;
        }
        public Preset(string id, string name, com.oddlabs.matchmaking.WorldConfig world, com.oddlabs.matchmaking.GameModeOptions mode_options, com.oddlabs.matchmaking.RosterTemplate roster, bool built_in) : base()
        {
            this.id = id;
            this.name = name;
            this.world = (world != null ? world : com.oddlabs.matchmaking.WorldConfig.defaults());
            this.mode_options = mode_options;
            this.roster = roster;
            this.built_in = built_in;
        }
    }
}
