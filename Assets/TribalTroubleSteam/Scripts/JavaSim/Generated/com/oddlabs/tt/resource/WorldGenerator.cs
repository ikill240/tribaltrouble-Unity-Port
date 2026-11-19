// Translated from WorldGenerator.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.resource
{
    // Java: 8
    public interface WorldGenerator
    {
        // Java: 9
        com.oddlabs.tt.resource.WorldInfo generate(int num_players, int initial_unit_count, float random_start_pos);
        // Java: 12
        com.oddlabs.tt.procedural.Landscape.TerrainType getTerrainType();
        // Java: 14
        int getMetersPerWorld();
        // Java: 16
        com.oddlabs.tt.resource.FogInfo getFogInfo();
    }
}
