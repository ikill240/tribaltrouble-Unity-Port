// Translated from AttackTowerTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.tutorial
{
    // Java: 11
    public class AttackTowerTrigger : com.oddlabs.tt.tutorial.TutorialTrigger
    {
        public const int NUM_UNITS = 12;
        public com.oddlabs.tt.model.Building tower;
        public com.oddlabs.tt.player.Player ai;
        // Java: 29
        public override void run(com.oddlabs.tt.tutorial.Tutorial tutorial)
        {
            if (ai.getUnitCountContainer().getNumSupplies() == 0)
            {
                tutorial.next(new com.oddlabs.tt.tutorial.RepairTowerTrigger(tower));
            }
        }
        public AttackTowerTrigger(com.oddlabs.tt.model.Building tower) : base(0.1f, 0.0f, "attack_tower")
        {
            this.ai = tower.getOwner().getWorld().getPlayers()[1];
            this.tower = tower;
            com.oddlabs.tt.model.Selectable[] units = com.oddlabs.tt.model.Selectable.newArray(com.oddlabs.tt.tutorial.AttackTowerTrigger.NUM_UNITS);
            for (int i = 0; i < units.Length; i++)
            {
                units[i] = new com.oddlabs.tt.model.Unit(ai, (float)(tower.getPositionX() - 50), (float)(tower.getPositionY() - 50), null, ai.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
            }
            ai.setTarget(units, tower, com.oddlabs.tt.model.Action.ATTACK, false);
        }
    }
}
