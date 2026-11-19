// Translated from Convert.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.weapon
{
    // Java: 17
    public class Convert : java.lang.Object, com.oddlabs.tt.model.weapon.Magic
    {
        public const float SUCCESS_RATE_PEON = 0.85f;
        public const float SUCCESS_RATE_ROCK_WARRIOR = 0.75f;
        public const float SUCCESS_RATE_IRON_WARRIOR = 0.55f;
        public const float SUCCESS_RATE_RUBBER_WARRIOR = 0.35f;
        public const float SUCCESS_RATE_CHIEF = 0.15f;
        public const float SUCCESS_RATE_BUILDING = 0.075f;
        public float hit_radius;
        public com.oddlabs.tt.player.Player owner;
        public float start_x;
        public float start_y;
        public bool affects_chieftains;
        public java.lang.Iterable target_list;
        // Java: 72
        public static float getSuccessRate(com.oddlabs.tt.model.Selectable target)
        {
            if ((target is com.oddlabs.tt.model.ShipProxy))
            {
                return 0.0f;
            }
            if ((target is com.oddlabs.tt.model.Building))
            {
                return com.oddlabs.tt.model.weapon.Convert.SUCCESS_RATE_BUILDING;
            }
            if (!((target is com.oddlabs.tt.model.Unit unit)))
            {
                return 0.0f;
            }
            if (unit.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC))
            {
                return com.oddlabs.tt.model.weapon.Convert.SUCCESS_RATE_CHIEF;
            }
            if (unit.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.HARVEST))
            {
                return com.oddlabs.tt.model.weapon.Convert.SUCCESS_RATE_PEON;
            }
            System.Type weapon_type = unit.getWeaponFactory().getType();
            if (((object)weapon_type == (object)typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)) || ((object)weapon_type == (object)typeof(com.oddlabs.tt.model.weapon.RockSpearWeapon)))
            {
                return com.oddlabs.tt.model.weapon.Convert.SUCCESS_RATE_ROCK_WARRIOR;
            }
            if (((object)weapon_type == (object)typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)) || ((object)weapon_type == (object)typeof(com.oddlabs.tt.model.weapon.IronSpearWeapon)))
            {
                return com.oddlabs.tt.model.weapon.Convert.SUCCESS_RATE_IRON_WARRIOR;
            }
            if (((object)weapon_type == (object)typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)) || ((object)weapon_type == (object)typeof(com.oddlabs.tt.model.weapon.RubberSpearWeapon)))
            {
                return com.oddlabs.tt.model.weapon.Convert.SUCCESS_RATE_RUBBER_WARRIOR;
            }
            return 0.0f;
        }
        // Java: 101
        public void animate(float t)
        {
            java.util.List to_convert = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Selectable selectable in java.lang.JIter.of(target_list))
            {
                if (selectable.isDead())
                {
                    continue;
                }
                if ((selectable is com.oddlabs.tt.model.Unit unit))
                {
                    if (unit.isMounted())
                    {
                        continue;
                    }
                    if (!affects_chieftains && unit.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC))
                    {
                        continue;
                    }
                }
                else if ((selectable is com.oddlabs.tt.model.ShipProxy) || !((selectable is com.oddlabs.tt.model.Building)))
                {
                    continue;
                }
                float dx = (float)(selectable.getPositionX() - start_x);
                float dy = (float)(selectable.getPositionY() - start_y);
                float squared_dist = (float)((float)(dx * dx) + (float)(dy * dy));
                if (owner.isEnemy(selectable.getOwner()) && squared_dist < (float)(hit_radius * hit_radius))
                {
                    to_convert.add(selectable);
                }
            }
            foreach (com.oddlabs.tt.model.Selectable selectable_2 in java.lang.JIter.of(to_convert))
            {
                float success_rate = com.oddlabs.tt.model.weapon.Convert.getSuccessRate(selectable_2);
                if (owner.getWorld().getRandom().nextFloat() >= success_rate)
                {
                    continue;
                }
                if ((selectable_2 is com.oddlabs.tt.model.Building building))
                {
                    convertBuilding(building);
                }
                else if ((selectable_2 is com.oddlabs.tt.model.Unit unit_2))
                {
                    convertUnit(unit_2);
                }
            }
            interrupt();
        }
        // Java: 154
        public void convertBuilding(com.oddlabs.tt.model.Building building)
        {
            if (building.isDead())
            {
                return;
            }
            int building_type = ((com.oddlabs.tt.model.BuildingTemplate)building.getTemplate()).getTemplateID();
            int grid_x = building.getGridX();
            int grid_y = building.getGridY();
            bool has_room = owner.canBuild(building_type);
            building.hit(building.getHitPoints() + 1000, 0.0f, 1.0f, owner);
            if (!has_room)
            {
                return;
            }
            com.oddlabs.tt.model.Building new_building = owner.getRace().getBuildingTemplate(building_type).create(owner, grid_x, grid_y);
            new_building.place();
            new_building.repair(((com.oddlabs.tt.model.BuildingTemplate)new_building.getTemplate()).getMaxHitPoints());
        }
        // Java: 182
        public void convertUnit(com.oddlabs.tt.model.Unit unit)
        {
            bool is_chieftain = unit.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC);
            com.oddlabs.tt.model.UnitTemplate template = ((com.oddlabs.tt.model.UnitTemplate)unit.getTemplate());
            float x = unit.getPositionX();
            float y = unit.getPositionY();
            int original_race_index = unit.getOwner().getPlayerInfo().getRace();
            unit.hit(unit.getHitPoints() + 1000, 0.0f, 1.0f, owner);
            bool has_room = is_chieftain || owner.getUnitCountContainer().getNumSupplies() < owner.getUnitCountContainer().getMaxSupplyCount();
            if (!has_room)
            {
                return;
            }
            com.oddlabs.tt.model.Unit converted = new com.oddlabs.tt.model.Unit(owner, x, y, null, template);
            if (is_chieftain)
            {
                owner.addExtraChieftain(converted);
                if (original_race_index != owner.getPlayerInfo().getRace())
                {
                    converted.setMagicRaceOverride(original_race_index);
                    converted.heal(converted.getEffectiveMaxHitPoints());
                }
            }
        }
        // Java: 235
        public void interrupt()
        {
            owner.getWorld().getAnimationManagerGameTime().removeAnimation(this);
        }
        public Convert(float offset_x, float offset_y, float hit_radius, com.oddlabs.tt.model.Unit src, bool affects_chieftains) : base()
        {
            this.hit_radius = hit_radius;
            this.owner = src.getOwner();
            this.affects_chieftains = affects_chieftains;
            start_x = (float)((float)(src.getPositionX() + (float)(offset_x * src.getDirectionX())) - (float)(offset_y * ((float)(-src.getDirectionY()))));
            start_y = (float)((float)(src.getPositionY() + (float)(offset_x * src.getDirectionY())) + (float)(offset_y * src.getDirectionX()));
            com.oddlabs.tt.pathfinder.FindOccupantFilter filter = new com.oddlabs.tt.pathfinder.FindOccupantFilter(src.getPositionX(), src.getPositionY(), hit_radius, src, com.oddlabs.tt.model.Selectable.genericClass());
            com.oddlabs.tt.pathfinder.UnitGrid unit_grid = owner.getWorld().getUnitGrid();
            unit_grid.scan(filter, com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(src.getPositionX()), com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(src.getPositionY()));
            target_list = filter.getResult();
        }
        // mixin of default method Animated.updateChecksum
        public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
        }
    }
}
