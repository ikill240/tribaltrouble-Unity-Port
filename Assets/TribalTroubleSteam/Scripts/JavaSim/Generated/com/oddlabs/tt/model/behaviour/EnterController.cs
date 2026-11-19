// Translated from EnterController.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.behaviour
{
    // Java: 11
    public class EnterController : com.oddlabs.tt.model.behaviour.Controller
    {
        public com.oddlabs.tt.model.Building building;
        public com.oddlabs.tt.model.Unit unit;
        // Java: 21
        public override void decide()
        {
            if (building.isDead())
            {
                unit.popController();
            }
            else if (unit.isCloseEnough(0.0f, building))
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
                        if ((unit.getWeaponFactory() is com.oddlabs.tt.model.weapon.ThrowingFactory))
                        {
                            System.Type type_2 = unit.getWeaponFactory().getType();
                            building.getSupplyContainer(type_2).increaseSupply(1);
                        }
                    }
                    building.getUnitContainer().enter(unit);
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
                    unit.popController();
                }
                else
                {
                    unit.setBehaviour(new com.oddlabs.tt.model.behaviour.WalkBehaviour(unit, building, 0, false));
                }
            }
        }
        public EnterController(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.model.Building building) : base(1)
        {
            this.unit = unit;
            this.building = building.getEntrance();
        }
    }
}
