// Translated from BuildingFinder.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 10
    public class BuildingFinder : java.lang.Object, com.oddlabs.tt.pathfinder.FinderFilter
    {
        public com.oddlabs.tt.player.Player owner;
        public int abilities;
        // Java: 19
        public com.oddlabs.tt.pathfinder.Occupant getOccupantFromRegion(com.oddlabs.tt.pathfinder.Region region, bool one_region)
        {
            return ((com.oddlabs.tt.model.Building)region.getObjects(typeof(com.oddlabs.tt.model.Building)).stream().filter(new java.util.function.Predicate_L((object a0) => { return this.accept(((com.oddlabs.tt.model.Building)a0)); })).findFirst().orElse(null));
        }
        // Java: 24
        public com.oddlabs.tt.pathfinder.Occupant getBest()
        {
            return null;
        }
        // Java: 29
        public bool accept(com.oddlabs.tt.model.Building building)
        {
            return ((object)building.getOwner() == (object)owner) && building.getAbilities().hasAbilities(abilities);
        }
        // Java: 33
        public bool acceptOccupant(com.oddlabs.tt.pathfinder.Occupant occ)
        {
            return (occ is com.oddlabs.tt.model.Building building) && accept(building);
        }
        public BuildingFinder(com.oddlabs.tt.player.Player owner, int abilities) : base()
        {
            this.owner = owner;
            this.abilities = abilities;
        }
    }
}
