// Translated from EnterController.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.behaviour
{
    // Java: 16
    public class EnterController : com.oddlabs.tt.model.behaviour.Controller
    {
        public com.oddlabs.tt.model.Building building;
        public com.oddlabs.tt.model.Unit unit;
        public const int MAX_STUCK_REROUTES = 3;
        public int stuck_reroutes;
        // Java: 41
        public static com.oddlabs.tt.model.Building findNearestAvailableTower(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.model.Building full_tower)
        {
            com.oddlabs.tt.model.Building nearest = null;
            float nearest_dist2 = java.lang.Float.MAX_VALUE;
            foreach (com.oddlabs.tt.player.Player player in unit.getWorld().getPlayers())
            {
                if (((object)player != (object)unit.getOwner()) && unit.getOwner().isEnemy(player))
                {
                    continue;
                }
                foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(player.getUnits().getSet()))
                {
                    if (s.isDead() || !((s is com.oddlabs.tt.model.Building candidate)) || ((object)candidate == (object)full_tower))
                    {
                        continue;
                    }
                    if (((com.oddlabs.tt.model.BuildingTemplate)candidate.getTemplate()).getTemplateID() != ((com.oddlabs.tt.model.BuildingTemplate)full_tower.getTemplate()).getTemplateID())
                    {
                        continue;
                    }
                    if (!((candidate.getUnitContainer() is com.oddlabs.tt.model.MountUnitContainer)) || !candidate.getUnitContainer().canEnter(unit))
                    {
                        continue;
                    }
                    float dx = (float)(candidate.getPositionX() - unit.getPositionX());
                    float dy = (float)(candidate.getPositionY() - unit.getPositionY());
                    float dist2 = (float)((float)(dx * dx) + (float)(dy * dy));
                    if (dist2 < nearest_dist2)
                    {
                        nearest_dist2 = dist2;
                        nearest = candidate;
                    }
                }
            }
            return nearest;
        }
        // Java: 69
        public override void decide()
        {
            if (building.isDead())
            {
                unit.popController();
            }
            else if (unit.isCloseEnough(0.0f, building) || near())
            {
                if (building.getUnitContainer() != null && building.getUnitContainer().canEnter(unit))
                {
                    com.oddlabs.tt.model.UnitSupplyContainer unitSupply = unit.getSupplyContainer();
                    int numSupply = (unitSupply != null ? unitSupply.getNumSupplies() : 0);
                    if (building.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.SUPPLY_CONTAINER))
                    {
                        if (unit.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.HARVEST) && numSupply > 0)
                        {
                            System.Type type = unitSupply.getSupplyType();
                            building.getSupplyContainer(type).increaseSupply(numSupply);
                            unitSupply.increaseSupply(-numSupply, type);
                        }
                        if ((unit.getWeaponFactory() is com.oddlabs.tt.model.weapon.ThrowingFactory) || (unit.getWeaponFactory() is com.oddlabs.tt.model.weapon.MeleeFactory))
                        {
                            System.Type type_2 = unit.getWeaponFactory().getType();
                            building.getSupplyContainer(type_2).increaseSupply(1);
                        }
                    }
                    building.getUnitContainer().enter(unit);
                }
                else if (unit.getOwner().usesReferenceCommandRules())
                {
                    unit.popController();
                }
                else if ((building.getUnitContainer() is com.oddlabs.tt.model.MountUnitContainer mount_container) && mount_container.isSupplyFull() && unit.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.THROW))
                {
                    com.oddlabs.tt.model.Unit mounted = mount_container.getUnit();
                    bool arriving_is_stronger = mounted != null && !mounted.getOwner().usesReferenceCommandRules() && ((com.oddlabs.tt.model.UnitTemplate)unit.getTemplate()).getMaxHitPoints() > ((com.oddlabs.tt.model.UnitTemplate)mounted.getTemplate()).getMaxHitPoints();
                    if (arriving_is_stronger)
                    {
                        mount_container.exit();
                        building.getUnitContainer().enter(unit);
                    }
                    else
                    {
                        com.oddlabs.tt.model.Building next_tower = com.oddlabs.tt.model.behaviour.EnterController.findNearestAvailableTower(unit, building);
                        if (next_tower != null)
                        {
                            unit.swapController(new com.oddlabs.tt.model.behaviour.EnterController(unit, next_tower));
                        }
                        else
                        {
                            unit.popController();
                        }
                    }
                }
                else if ((building.getUnitContainer() is com.oddlabs.tt.model.MountUnitContainer) && ((com.oddlabs.tt.model.BuildingTemplate)building.getTemplate()).getTemplateID() == com.oddlabs.tt.model.Race.BUILDING_TOWER)
                {
                    com.oddlabs.tt.model.Building next_tower_2 = com.oddlabs.tt.model.behaviour.EnterController.findNearestAvailableTower(unit, building);
                    if (next_tower_2 != null)
                    {
                        unit.swapController(new com.oddlabs.tt.model.behaviour.EnterController(unit, next_tower_2));
                    }
                    else
                    {
                        unit.popController();
                    }
                }
                else
                {
                    unit.popController();
                }
            }
            else
            {
                if (shouldGiveUp(0))
                {
                    if (com.oddlabs.tt.model.behaviour.WaitRetryController.applies(unit))
                    {
                        resetGiveUpCounters();
                        unit.pushController(new com.oddlabs.tt.model.behaviour.WaitRetryController(unit, com.oddlabs.tt.model.behaviour.WaitRetryController.delay(unit)));
                    }
                    else
                    {
                        unit.popController();
                    }
                }
                else
                {
                    unit.setBehaviour(new com.oddlabs.tt.model.behaviour.WalkBehaviour(unit, building, building.getInteractDistance(), false));
                }
            }
        }
        // Java: 166
        public void towerTaken()
        {
            if (building.isDead() || !((building.getUnitContainer() is com.oddlabs.tt.model.MountUnitContainer mount)) || !mount.isSupplyFull() || unit.isMounted() || !unit.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.THROW) || unit.getOwner().usesReferenceCommandRules() || ((com.oddlabs.tt.model.BuildingTemplate)building.getTemplate()).getTemplateID() != com.oddlabs.tt.model.Race.BUILDING_TOWER)
            {
                return;
            }
            com.oddlabs.tt.model.Unit mounted = mount.getUnit();
            if (((object)mounted == (object)unit))
            {
                return;
            }
            if (mounted != null && !mounted.getOwner().usesReferenceCommandRules() && ((com.oddlabs.tt.model.UnitTemplate)unit.getTemplate()).getMaxHitPoints() > ((com.oddlabs.tt.model.UnitTemplate)mounted.getTemplate()).getMaxHitPoints())
            {
                return;
            }
            com.oddlabs.tt.model.Building next_tower = com.oddlabs.tt.model.behaviour.EnterController.findNearestAvailableTower(unit, building);
            if (next_tower != null)
            {
                unit.swapController(new com.oddlabs.tt.model.behaviour.EnterController(unit, next_tower));
            }
            else
            {
                unit.popController();
            }
        }
        // Java: 185
        public bool near()
        {
            float d = building.getInteractDistance();
            return d > 0.0f && unit.isCloseEnough(d, building);
        }
        // Java: 190
        public override bool onStuck()
        {
            if (building.isDead() || ++stuck_reroutes > com.oddlabs.tt.model.behaviour.EnterController.MAX_STUCK_REROUTES)
            {
                return false;
            }
            unit.setBehaviour(new com.oddlabs.tt.model.behaviour.WalkBehaviour(unit, building, building.getInteractDistance(), false));
            return true;
        }
        public EnterController(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.model.Building building) : base(1)
        {
            this.unit = unit;
            this.building = building.getEntrance();
        }
    }
}
