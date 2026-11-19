// Translated from WorldGenerator.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.resource
{
    // Java: 9
    public interface WorldGenerator
    {
        // Java: 10
        com.oddlabs.tt.resource.WorldInfo generate(int num_players, int initial_unit_count, float random_start_pos, bool team_together, int[] player_teams);
        // Java: 22
        com.oddlabs.tt.procedural.Landscape.TerrainType getTerrainType();
        // Java: 24
        int getMetersPerWorld();
        // Java: 26
        com.oddlabs.tt.resource.FogInfo getFogInfo();
    }
}
