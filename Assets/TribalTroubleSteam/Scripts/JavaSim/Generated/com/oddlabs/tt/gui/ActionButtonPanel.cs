// Translated from ActionButtonPanel.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 43
    public class ActionButtonPanel : com.oddlabs.tt.gui.GUIObject, com.oddlabs.tt.animation.Animated
    {
        public const int GROUP_LEFT_OFFSET = 10;
        public const int GROUP_BOTTOM_OFFSET = 10;
        public const int GROUP_RIGHT_OFFSET = 10;
        public const int GROUP_TOP_OFFSET = 20;
        public static java.util.Set ARMORY_SUBMENU_ACTIONS;
        public com.oddlabs.tt.gui.Group unit_group;
        public com.oddlabs.tt.gui.Group peon_group;
        public com.oddlabs.tt.gui.Group chieftain_group;
        public com.oddlabs.tt.gui.Group tower_group;
        public com.oddlabs.tt.gui.Group quarters_status_group;
        public com.oddlabs.tt.gui.Group quarters_group;
        public com.oddlabs.tt.gui.Group status_group;
        public com.oddlabs.tt.gui.Group armory_group;
        public com.oddlabs.tt.gui.Group ship_group;
        public com.oddlabs.tt.gui.Group harvest_group;
        public com.oddlabs.tt.gui.Group build_group;
        public com.oddlabs.tt.gui.Group army_group;
        public com.oddlabs.tt.gui.Group ship_army_group;
        public com.oddlabs.tt.gui.Group transport_group;
        public com.oddlabs.tt.gui.NonFocusIconButton tower_attack_button;
        public com.oddlabs.tt.gui.NonFocusIconButton tower_exit_button;
        public com.oddlabs.tt.gui.NonFocusIconButton move_button;
        public com.oddlabs.tt.gui.NonFocusIconButton attack_button;
        public com.oddlabs.tt.gui.NonFocusIconButton gather_repair_button;
        public com.oddlabs.tt.gui.NonFocusIconButton quarters_button;
        public com.oddlabs.tt.gui.RechargeButton magic1_button;
        public com.oddlabs.tt.gui.RechargeButton magic2_button;
        public com.oddlabs.tt.gui.NonFocusIconButton armory_button;
        public com.oddlabs.tt.gui.NonFocusIconButton tower_button;
        public com.oddlabs.tt.gui.NonFocusIconButton ship_button;
        public com.oddlabs.tt.gui.NonFocusIconButton harvest_button;
        public com.oddlabs.tt.gui.NonFocusIconButton build_button;
        public com.oddlabs.tt.gui.NonFocusIconButton army_button;
        public com.oddlabs.tt.gui.NonFocusIconButton transport_button;
        public com.oddlabs.tt.gui.NonFocusIconButton rally_point_button;
        public com.oddlabs.tt.gui.NonFocusIconButton ship_harvest_button;
        public com.oddlabs.tt.gui.NonFocusIconButton ship_army_button;
        public com.oddlabs.tt.gui.NonFocusIconButton ship_rally_point_button;
        public com.oddlabs.tt.gui.NonFocusIconButton ship_transport_button;
        public com.oddlabs.tt.gui.NonFocusIconButton ship_sail_button;
        public com.oddlabs.tt.gui.StatusIcon unit_status;
        public com.oddlabs.tt.gui.StatusIcon weapon_rock_status;
        public com.oddlabs.tt.gui.StatusIcon weapon_iron_status;
        public com.oddlabs.tt.gui.StatusIcon weapon_rubber_status;
        public com.oddlabs.tt.gui.StatusIcon tree_status;
        public com.oddlabs.tt.gui.StatusIcon rock_status;
        public com.oddlabs.tt.gui.StatusIcon iron_status;
        public com.oddlabs.tt.gui.StatusIcon rubber_status;
        public com.oddlabs.tt.gui.WatchStatusIcon quarters_unit_status;
        public com.oddlabs.tt.gui.DeploySpinner quarters_peon_button;
        public com.oddlabs.tt.gui.ChieftainButton quarters_chieftain_button;
        public com.oddlabs.tt.gui.NonFocusIconButton quarters_rally_point_button;
        public com.oddlabs.tt.gui.DeploySpinner harvest_tree_button;
        public com.oddlabs.tt.gui.DeploySpinner harvest_rock_button;
        public com.oddlabs.tt.gui.DeploySpinner harvest_iron_button;
        public com.oddlabs.tt.gui.DeploySpinner harvest_rubber_button;
        public com.oddlabs.tt.gui.NonFocusIconButton harvest_back_button;
        public com.oddlabs.tt.gui.BuildSpinner build_weapon_rock_button;
        public com.oddlabs.tt.gui.BuildSpinner build_weapon_iron_button;
        public com.oddlabs.tt.gui.BuildSpinner build_weapon_rubber_button;
        public com.oddlabs.tt.gui.NonFocusIconButton build_back_button;
        public com.oddlabs.tt.gui.DeploySpinner army_peon_button;
        public com.oddlabs.tt.gui.DeploySpinner army_warrior_rock_button;
        public com.oddlabs.tt.gui.DeploySpinner army_warrior_iron_button;
        public com.oddlabs.tt.gui.DeploySpinner army_warrior_rubber_button;
        public com.oddlabs.tt.gui.NonFocusIconButton army_back_button;
        public com.oddlabs.tt.gui.DeploySpinner ship_army_peon_button;
        public com.oddlabs.tt.gui.DeploySpinner ship_army_warrior_rock_button;
        public com.oddlabs.tt.gui.DeploySpinner ship_army_warrior_iron_button;
        public com.oddlabs.tt.gui.DeploySpinner ship_army_warrior_rubber_button;
        public com.oddlabs.tt.gui.NonFocusIconButton ship_army_chieftain_button;
        public com.oddlabs.tt.gui.NonFocusIconButton ship_army_back_button;
        public com.oddlabs.tt.gui.DeploySpinner transport_tree_button;
        public com.oddlabs.tt.gui.DeploySpinner transport_rock_button;
        public com.oddlabs.tt.gui.DeploySpinner transport_iron_button;
        public com.oddlabs.tt.gui.DeploySpinner transport_rubber_button;
        public com.oddlabs.tt.gui.NonFocusIconButton transport_back_button;
        public static java.util.ResourceBundle bundle;
        // Java: 147
        public static string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.gui.ActionButtonPanel.bundle, key, args);
        }
        // Java: 151
        public static string getBinding(com.oddlabs.tt.input.GameAction action)
        {
            return com.oddlabs.tt.render.Renderer.getLocalInput().getInputManager().getBindingString(action);
        }
        public com.oddlabs.tt.camera.GameCamera camera;
        public com.oddlabs.tt.viewer.WorldViewer viewer;
        public com.oddlabs.tt.player.Player player;
        public com.oddlabs.tt.viewer.Selection selection;
        public bool read_only;
        public com.oddlabs.tt.gui.Group current_submenu;
        public bool update;
        public bool current_quarters;
        public bool current_armory;
        public bool current_ship;
        public com.oddlabs.tt.model.Building current_building;
        public bool current_unit;
        public bool current_peon;
        public com.oddlabs.tt.model.Unit current_chieftain;
        public bool current_magic1;
        public bool current_magic2;
        public bool current_tower;
        // Java: 560
        public override void doAdd()
        {
            base.doAdd();
            viewer.getAnimationManagerLocal().registerAnimation(this);
            com.oddlabs.tt.gui.GUIRoot root = getParentGUIRoot();
            if (root != null)
            {
                displayChangedNotify(root.getWidth(), root.getHeight());
            }
        }
        // Java: 570
        public override void doRemove()
        {
            base.doRemove();
            viewer.getAnimationManagerLocal().removeAnimation(this);
        }
        // Java: 576
        public void animate(float t)
        {
            com.oddlabs.tt.model.Building new_building = selection.getCurrentSelection().getBuilding();
            bool different_building = ((object)new_building != (object)current_building);
            current_building = new_building;
            viewer.getRenderer().setSelectedBuilding(new_building);
            com.oddlabs.tt.model.Unit new_chieftain = selection.getCurrentSelection().getChieftain();
            bool different_chieftain = ((object)new_chieftain != (object)current_chieftain);
            current_chieftain = new_chieftain;
            int current_num_units = selection.getCurrentSelection().getNumUnits();
            int current_num_peons = selection.getCurrentSelection().getNumBuilders();
            bool new_quarters = current_building != null && current_building.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.REPRODUCE);
            bool new_armory = current_building != null && current_building.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD_ARMIES);
            bool new_ship = current_building != null && current_building.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.SAIL);
            bool new_unit = current_num_units > 0;
            bool new_peon = current_num_peons > 0;
            bool new_tower = current_building != null && current_building.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.ATTACK);
            bool new_magic1 = player.canDoMagic(0);
            bool new_magic2 = player.canDoMagic(1);
            update = update || different_building || different_chieftain || new_quarters != current_quarters || new_armory != current_armory || new_ship != current_ship || new_unit != current_unit || new_peon != current_peon || new_tower != current_tower || new_magic1 != current_magic1 || new_magic2 != current_magic2;
            if (update)
            {
                current_quarters = new_quarters;
                current_armory = new_armory;
                current_ship = new_ship;
                current_tower = new_tower;
                current_unit = new_unit;
                current_peon = new_peon;
                current_magic1 = new_magic1;
                current_magic2 = new_magic2;
                update = false;
                removeGroups();
                if (current_unit)
                {
                    addChild(unit_group);
                }
                if (current_peon)
                {
                    addChild(peon_group);
                }
                if (current_chieftain != null)
                {
                    addChild(chieftain_group);
                    updateGroups();
                    if (player.canDoMagic(0))
                    {
                        magic1_button.setUnit(current_chieftain);
                        magic1_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
                        {
                            return !current_chieftain.canDoMagic(0);
                        }));
                        chieftain_group.addChild(magic1_button);
                    }
                    else
                    {
                        magic1_button.remove();
                    }
                    if (player.canDoMagic(1))
                    {
                        magic2_button.setUnit(current_chieftain);
                        magic2_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
                        {
                            return !current_chieftain.canDoMagic(1);
                        }));
                        chieftain_group.addChild(magic2_button);
                    }
                    else
                    {
                        magic2_button.remove();
                    }
                }
                if (current_tower)
                {
                    addChild(tower_group);
                    tower_attack_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
                    {
                        return current_building == null || !current_building.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.ATTACK);
                    }));
                    tower_exit_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
                    {
                        return current_building == null || !current_building.canExitTower();
                    }));
                }
                if (current_quarters)
                {
                    addChild(quarters_status_group);
                    addChild(quarters_group);
                    com.oddlabs.tt.model.SupplyCounter unit_counter = new com.oddlabs.tt.model.SupplyCounter(current_building, typeof(com.oddlabs.tt.model.Unit));
                    quarters_unit_status.setCounter(unit_counter);
                    quarters_unit_status.setUnitContainerBuilding(current_building);
                    quarters_peon_button.setContainers(current_building, com.oddlabs.tt.model.DeployType.PEON, null);
                    quarters_peon_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
                    {
                        return unit_counter.getNumSupplies() == 0;
                    }));
                    quarters_chieftain_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
                    {
                        return current_building != null && !current_building.canBuildChieftain() && !current_building.canStopChieftain();
                    }));
                    quarters_chieftain_button.setBuilding(current_building);
                }
                if (current_armory)
                {
                    addChild(status_group);
                    addChild(armory_group);
                    if (player.canUseRubber())
                    {
                        build_group.addChild(build_weapon_rubber_button);
                        army_group.addChild(army_warrior_rubber_button);
                    }
                    else
                    {
                        build_weapon_rubber_button.remove();
                        army_warrior_rubber_button.remove();
                    }
                    updateCounters();
                }
                if (current_ship)
                {
                    addChild(status_group);
                    addChild(ship_group);
                    if (player.canUseRubber())
                    {
                        ship_army_group.addChild(ship_army_warrior_rubber_button);
                    }
                    else
                    {
                        ship_army_warrior_rubber_button.remove();
                    }
                    updateCounters();
                }
            }
            updateButtons();
        }
        // Java: 683
        public void updateButtons()
        {
            if (current_building != null && current_building.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.ATTACK))
            {
                tower_attack_button.doUpdate();
                tower_exit_button.doUpdate();
            }
            else if (current_building != null && current_building.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD_ARMIES))
            {
                unit_status.doUpdate();
                weapon_rock_status.doUpdate();
                weapon_iron_status.doUpdate();
                weapon_rubber_status.doUpdate();
                tree_status.doUpdate();
                rock_status.doUpdate();
                iron_status.doUpdate();
                rubber_status.doUpdate();
                harvest_button.doUpdate();
                build_button.doUpdate();
                army_button.doUpdate();
                harvest_tree_button.doUpdate();
                harvest_rock_button.doUpdate();
                harvest_iron_button.doUpdate();
                harvest_rubber_button.doUpdate();
                build_weapon_rock_button.doUpdate();
                build_weapon_iron_button.doUpdate();
                build_weapon_rubber_button.doUpdate();
                army_warrior_rubber_button.doUpdate();
                army_warrior_iron_button.doUpdate();
                army_warrior_rock_button.doUpdate();
                army_peon_button.doUpdate();
                transport_tree_button.doUpdate();
                transport_rock_button.doUpdate();
                transport_iron_button.doUpdate();
                transport_rubber_button.doUpdate();
            }
            else if (current_building != null && current_building.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.REPRODUCE))
            {
                quarters_unit_status.doUpdate();
                quarters_peon_button.doUpdate();
                quarters_chieftain_button.doUpdate();
            }
            else if (current_building != null && current_building.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.SAIL))
            {
                unit_status.doUpdate();
                weapon_rock_status.doUpdate();
                weapon_iron_status.doUpdate();
                weapon_rubber_status.doUpdate();
                tree_status.doUpdate();
                rock_status.doUpdate();
                iron_status.doUpdate();
                rubber_status.doUpdate();
                ship_harvest_button.doUpdate();
                ship_army_button.doUpdate();
                ship_sail_button.doUpdate();
                harvest_tree_button.doUpdate();
                harvest_rock_button.doUpdate();
                harvest_iron_button.doUpdate();
                harvest_rubber_button.doUpdate();
                ship_army_warrior_rubber_button.doUpdate();
                ship_army_warrior_iron_button.doUpdate();
                ship_army_warrior_rock_button.doUpdate();
                ship_army_peon_button.doUpdate();
                ship_army_chieftain_button.doUpdate();
                transport_tree_button.doUpdate();
                transport_rock_button.doUpdate();
                transport_iron_button.doUpdate();
                transport_rubber_button.doUpdate();
            }
            else if (current_peon)
            {
                quarters_button.doUpdate();
                armory_button.doUpdate();
                tower_button.doUpdate();
                if (viewer.getWorld().isShipsEnabled())
                {
                    ship_button.doUpdate();
                }
            }
            if (current_unit)
            {
                move_button.doUpdate();
                attack_button.doUpdate();
                gather_repair_button.doUpdate();
            }
            if (current_chieftain != null)
            {
                magic1_button.doUpdate();
                magic2_button.doUpdate();
            }
        }
        // Java: 768
        public void removeGroups()
        {
            unit_group.remove();
            peon_group.remove();
            chieftain_group.remove();
            tower_group.remove();
            quarters_status_group.remove();
            quarters_group.remove();
            status_group.remove();
            armory_group.remove();
            ship_group.remove();
            harvest_group.remove();
            build_group.remove();
            army_group.remove();
            ship_army_group.remove();
            transport_group.remove();
            current_submenu = null;
        }
        // Java: 787
        public void refreshCounters()
        {
            if (read_only && getSubmenu() != com.oddlabs.tt.gui.ActionButtonPanel.SUBMENU_NONE && (current_armory || current_ship) && current_building != null && !current_building.isDead())
            {
                updateCounters();
            }
        }
        // Java: 793
        public void updateCounters()
        {
            com.oddlabs.tt.model.SupplyCounter unit_counter = new com.oddlabs.tt.model.SupplyCounter(current_building, typeof(com.oddlabs.tt.model.Unit));
            unit_status.setCounter(unit_counter);
            com.oddlabs.tt.model.SupplyCounter weapon_rock_counter = new com.oddlabs.tt.model.SupplyCounter(current_building, typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon));
            weapon_rock_status.setCounter(weapon_rock_counter);
            com.oddlabs.tt.model.SupplyCounter weapon_iron_counter = new com.oddlabs.tt.model.SupplyCounter(current_building, typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon));
            weapon_iron_status.setCounter(weapon_iron_counter);
            com.oddlabs.tt.model.SupplyCounter weapon_rubber_counter = new com.oddlabs.tt.model.SupplyCounter(current_building, typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon));
            weapon_rubber_status.setCounter(weapon_rubber_counter);
            com.oddlabs.tt.model.SupplyCounter tree_counter = new com.oddlabs.tt.model.SupplyCounter(current_building, typeof(com.oddlabs.tt.landscape.TreeSupply));
            tree_status.setCounter(tree_counter);
            com.oddlabs.tt.model.SupplyCounter rock_counter = new com.oddlabs.tt.model.SupplyCounter(current_building, typeof(com.oddlabs.tt.model.RockSupply));
            rock_status.setCounter(rock_counter);
            com.oddlabs.tt.model.SupplyCounter iron_counter = new com.oddlabs.tt.model.SupplyCounter(current_building, typeof(com.oddlabs.tt.model.IronSupply));
            iron_status.setCounter(iron_counter);
            com.oddlabs.tt.model.SupplyCounter rubber_counter = new com.oddlabs.tt.model.SupplyCounter(current_building, typeof(com.oddlabs.tt.model.RubberSupply));
            rubber_status.setCounter(rubber_counter);
            harvest_tree_button.setContainers(current_building, com.oddlabs.tt.model.DeployType.PEON_HARVEST_TREE, null);
            harvest_tree_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return unit_counter.getNumSupplies() == 0;
            }));
            harvest_rock_button.setContainers(current_building, com.oddlabs.tt.model.DeployType.PEON_HARVEST_ROCK, null);
            harvest_rock_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return unit_counter.getNumSupplies() == 0;
            }));
            harvest_iron_button.setContainers(current_building, com.oddlabs.tt.model.DeployType.PEON_HARVEST_IRON, null);
            harvest_iron_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return unit_counter.getNumSupplies() == 0;
            }));
            harvest_rubber_button.setContainers(current_building, com.oddlabs.tt.model.DeployType.PEON_HARVEST_RUBBER, null);
            harvest_rubber_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return unit_counter.getNumSupplies() == 0;
            }));
            build_weapon_rock_button.setBuildSupplyContainer(current_building, typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon));
            build_weapon_iron_button.setBuildSupplyContainer(current_building, typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon));
            build_weapon_rubber_button.setBuildSupplyContainer(current_building, typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon));
            army_peon_button.setContainers(current_building, com.oddlabs.tt.model.DeployType.PEON, null);
            army_peon_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return unit_counter.getNumSupplies() == 0;
            }));
            army_warrior_rock_button.setContainers(current_building, com.oddlabs.tt.model.DeployType.ROCK_WARRIOR, typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon));
            army_warrior_rock_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return suppliesEmpty(unit_counter, weapon_rock_counter);
            }));
            army_warrior_iron_button.setContainers(current_building, com.oddlabs.tt.model.DeployType.IRON_WARRIOR, typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon));
            army_warrior_iron_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return suppliesEmpty(unit_counter, weapon_iron_counter);
            }));
            army_warrior_rubber_button.setContainers(current_building, com.oddlabs.tt.model.DeployType.RUBBER_WARRIOR, typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon));
            army_warrior_rubber_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return suppliesEmpty(unit_counter, weapon_rubber_counter);
            }));
            ship_army_peon_button.setContainers(current_building, com.oddlabs.tt.model.DeployType.PEON, null);
            ship_army_peon_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return unit_counter.getNumSupplies() == 0;
            }));
            ship_army_warrior_rock_button.setContainers(current_building, com.oddlabs.tt.model.DeployType.ROCK_WARRIOR, typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon));
            ship_army_warrior_rock_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return suppliesEmpty(unit_counter, weapon_rock_counter);
            }));
            ship_army_warrior_iron_button.setContainers(current_building, com.oddlabs.tt.model.DeployType.IRON_WARRIOR, typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon));
            ship_army_warrior_iron_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return suppliesEmpty(unit_counter, weapon_iron_counter);
            }));
            ship_army_warrior_rubber_button.setContainers(current_building, com.oddlabs.tt.model.DeployType.RUBBER_WARRIOR, typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon));
            ship_army_warrior_rubber_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return suppliesEmpty(unit_counter, weapon_rubber_counter);
            }));
            transport_tree_button.setContainers(current_building, com.oddlabs.tt.model.DeployType.PEON_TRANSPORT_TREE, typeof(com.oddlabs.tt.landscape.TreeSupply));
            transport_tree_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return suppliesEmpty(unit_counter, tree_counter);
            }));
            transport_rock_button.setContainers(current_building, com.oddlabs.tt.model.DeployType.PEON_TRANSPORT_ROCK, typeof(com.oddlabs.tt.model.RockSupply));
            transport_rock_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return suppliesEmpty(unit_counter, rock_counter);
            }));
            transport_iron_button.setContainers(current_building, com.oddlabs.tt.model.DeployType.PEON_TRANSPORT_IRON, typeof(com.oddlabs.tt.model.IronSupply));
            transport_iron_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return suppliesEmpty(unit_counter, iron_counter);
            }));
            transport_rubber_button.setContainers(current_building, com.oddlabs.tt.model.DeployType.PEON_TRANSPORT_RUBBER, typeof(com.oddlabs.tt.model.RubberSupply));
            transport_rubber_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return suppliesEmpty(unit_counter, rubber_counter);
            }));
        }
        // Java: 854
        public override void displayChangedNotify(int width, int height)
        {
            setDim(width, height);
            updateGroups();
        }
        // Java: 860
        public void updateGroups()
        {
            int width = getWidth();
            int height = getHeight();
            unit_group.setPos(width - unit_group.getWidth(), height - unit_group.getHeight());
            peon_group.setPos(width - peon_group.getWidth(), unit_group.getY() - peon_group.getHeight());
            if (current_peon)
            {
                chieftain_group.setPos(width - chieftain_group.getWidth(), peon_group.getY() - chieftain_group.getHeight());
            }
            else
            {
                chieftain_group.setPos(width - chieftain_group.getWidth(), unit_group.getY() - chieftain_group.getHeight());
            }
            tower_group.setPos(width - tower_group.getWidth(), height - tower_group.getHeight());
            quarters_status_group.setPos(width - quarters_status_group.getWidth(), height - quarters_status_group.getHeight());
            quarters_group.setPos(width - quarters_group.getWidth(), quarters_status_group.getY() - quarters_group.getHeight());
            status_group.setPos(width - status_group.getWidth(), height - status_group.getHeight());
            armory_group.setPos(width - armory_group.getWidth(), status_group.getY() - armory_group.getHeight());
            ship_group.setPos(width - ship_group.getWidth(), status_group.getY() - ship_group.getHeight());
            harvest_group.setPos(width - harvest_group.getWidth(), status_group.getY() - harvest_group.getHeight());
            build_group.setPos(width - build_group.getWidth(), status_group.getY() - build_group.getHeight());
            army_group.setPos(width - army_group.getWidth(), status_group.getY() - army_group.getHeight());
            ship_army_group.setPos(width - ship_army_group.getWidth(), status_group.getY() - ship_army_group.getHeight());
            transport_group.setPos(width - transport_group.getWidth(), status_group.getY() - transport_group.getHeight());
        }
        // Java: 885
        public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
            if (read_only)
            {
                return;
            }
            com.oddlabs.tt.input.InputPhase phase = @event.getPhase();
            bool pressed = ((object)phase == (object)com.oddlabs.tt.input.InputPhase.PRESSED) || ((object)phase == (object)com.oddlabs.tt.input.InputPhase.REPEAT);
            bool released = ((object)phase == (object)com.oddlabs.tt.input.InputPhase.RELEASED);
            bool repeat = ((object)phase == (object)com.oddlabs.tt.input.InputPhase.REPEAT);
            if (pressed)
            {
                if (!repeat)
                {
                    if (current_armory && canSwitchSubmenu(@event) && @event.consumeAction(com.oddlabs.tt.input.GameAction.PROD_WEAPONS))
                    {
                        activate(@event, build_button);
                    }
                    else if (current_armory && canSwitchSubmenu(@event) && @event.consumeAction(com.oddlabs.tt.input.GameAction.PROD_ARMY))
                    {
                        activate(@event, army_button);
                    }
                    else if (current_armory && canSwitchSubmenu(@event) && @event.consumeAction(com.oddlabs.tt.input.GameAction.PROD_TRANSPORT))
                    {
                        activate(@event, transport_button);
                    }
                    else if (current_armory && canSwitchSubmenu(@event) && @event.consumeAction(com.oddlabs.tt.input.GameAction.PROD_HARVEST))
                    {
                        activate(@event, harvest_button);
                    }
                    else if (current_ship && canSwitchSubmenu(@event) && @event.consumeAction(com.oddlabs.tt.input.GameAction.PROD_ARMY))
                    {
                        activate(@event, ship_army_button);
                    }
                    else if (current_ship && canSwitchSubmenu(@event) && @event.consumeAction(com.oddlabs.tt.input.GameAction.PROD_TRANSPORT))
                    {
                        activate(@event, ship_transport_button);
                    }
                    else if (current_ship && canSwitchSubmenu(@event) && @event.consumeAction(com.oddlabs.tt.input.GameAction.PROD_HARVEST))
                    {
                        activate(@event, ship_harvest_button);
                    }
                    else if ((current_unit || current_ship) && @event.consumeAction(com.oddlabs.tt.input.GameAction.UNIT_MOVE))
                    {
                        if (current_unit)
                        {
                            activate(@event, move_button);
                        }
                        else if (current_submenu == null)
                        {
                            activate(@event, ship_sail_button);
                        }
                    }
                    else if (current_unit && current_peon && @event.consumeAction(com.oddlabs.tt.input.GameAction.UNIT_BUILD_QUARTERS))
                    {
                        activate(@event, quarters_button);
                    }
                    else if (current_unit && current_peon && @event.consumeAction(com.oddlabs.tt.input.GameAction.UNIT_BUILD_SHIP))
                    {
                        if (viewer.getWorld().isShipsEnabled())
                        {
                            activate(@event, ship_button);
                        }
                    }
                    else if ((current_unit || current_tower) && @event.consumeAction(com.oddlabs.tt.input.GameAction.UNIT_ATTACK))
                    {
                        if (current_unit)
                        {
                            activate(@event, attack_button);
                        }
                        else if (current_tower)
                        {
                            activate(@event, tower_attack_button);
                        }
                    }
                    else if ((current_unit || current_armory || current_ship) && (@event.consumeAction(com.oddlabs.tt.input.GameAction.UNIT_GATHER) || @event.consumeAction(com.oddlabs.tt.input.GameAction.PROD_HARVEST)))
                    {
                        if (current_unit)
                        {
                            activate(@event, gather_repair_button);
                        }
                        else if (current_armory && current_submenu == null)
                        {
                            activate(@event, harvest_button);
                        }
                        else if (current_ship && current_submenu == null)
                        {
                            activate(@event, ship_harvest_button);
                        }
                    }
                    else if ((current_peon || current_armory || current_ship) && @event.consumeAction(com.oddlabs.tt.input.GameAction.UNIT_BUILD_TOWER))
                    {
                        if (current_peon)
                        {
                            activate(@event, tower_button);
                        }
                    }
                    else if (current_quarters && @event.consumeAction(com.oddlabs.tt.input.GameAction.TRAIN_CHIEFTAIN))
                    {
                        activate(@event, quarters_chieftain_button);
                    }
                    else if (current_ship && ((object)current_submenu == (object)ship_army_group) && @event.consumeAction(com.oddlabs.tt.input.GameAction.DEPLOY_CHIEFTAIN))
                    {
                        activate(@event, ship_army_chieftain_button);
                    }
                    else if (current_chieftain != null && @event.consumeAction(com.oddlabs.tt.input.GameAction.MAGIC_2))
                    {
                        if (player.canDoMagic(1))
                        {
                            activate(@event, magic2_button);
                        }
                    }
                    else if ((current_armory || current_ship) && current_submenu != null && @event.consumeAction(com.oddlabs.tt.input.GameAction.GAMEPLAY_BACK))
                    {
                        if (((object)current_submenu == (object)harvest_group))
                        {
                            activate(@event, harvest_back_button);
                        }
                        else if (((object)current_submenu == (object)build_group))
                        {
                            activate(@event, build_back_button);
                        }
                        else if (((object)current_submenu == (object)army_group))
                        {
                            activate(@event, army_back_button);
                        }
                        else if (((object)current_submenu == (object)ship_army_group))
                        {
                            activate(@event, ship_army_back_button);
                        }
                        else if (((object)current_submenu == (object)transport_group))
                        {
                            activate(@event, transport_back_button);
                        }
                    }
                    else if (current_building == null && current_peon && @event.consumeAction(com.oddlabs.tt.input.GameAction.UNIT_BUILD_ARMORY))
                    {
                        activate(@event, armory_button);
                    }
                    else if (current_building != null && @event.consumeAction(com.oddlabs.tt.input.GameAction.UNIT_SET_RALLY))
                    {
                        if (current_armory && current_submenu == null)
                        {
                            activate(@event, rally_point_button);
                        }
                        else if (current_ship && current_submenu == null)
                        {
                            activate(@event, ship_rally_point_button);
                        }
                        else if (current_quarters)
                        {
                            activate(@event, quarters_rally_point_button);
                        }
                    }
                    else if (current_tower && @event.consumeAction(com.oddlabs.tt.input.GameAction.UNIT_EXIT_TOWER))
                    {
                        activate(@event, tower_exit_button);
                    }
                    else if (current_chieftain != null && @event.consumeAction(com.oddlabs.tt.input.GameAction.MAGIC_1))
                    {
                        if (player.canDoMagic(0))
                        {
                            activate(@event, magic1_button);
                        }
                    }
                    if (@event.isConsumed())
                    {
                        return;
                    }
                }
                if (current_building != null)
                {
                    com.oddlabs.tt.gui.ActionButtonPanel.ResourceAction peon = checkResourceAction(@event, com.oddlabs.tt.input.GameAction.TRAIN_PEON, com.oddlabs.tt.input.GameAction.TRAIN_PEON_DEC, com.oddlabs.tt.input.GameAction.TRAIN_PEON_BATCH, com.oddlabs.tt.input.GameAction.TRAIN_PEON_BATCH_DEC);
                    if (peon.active())
                    {
                        if (current_quarters)
                        {
                            quarters_peon_button.shortcutPressed(peon.decrement(), peon.batch());
                            @event.getActions().clear();
                        }
                        else if (current_armory && ((object)current_submenu == (object)army_group))
                        {
                            army_peon_button.shortcutPressed(peon.decrement(), peon.batch());
                            @event.getActions().clear();
                        }
                        else if (current_ship && ((object)current_submenu == (object)ship_army_group))
                        {
                            ship_army_peon_button.shortcutPressed(peon.decrement(), peon.batch());
                            @event.getActions().clear();
                        }
                    }
                    com.oddlabs.tt.gui.ActionButtonPanel.ResourceAction chicken = checkResourceAction(@event, com.oddlabs.tt.input.GameAction.RES_CHICKEN, com.oddlabs.tt.input.GameAction.RES_CHICKEN_DEC, com.oddlabs.tt.input.GameAction.RES_CHICKEN_BATCH, com.oddlabs.tt.input.GameAction.RES_CHICKEN_BATCH_DEC);
                    if (chicken.active())
                    {
                        handleArmoryShortcut(true, chicken, harvest_rubber_button, build_weapon_rubber_button, army_warrior_rubber_button, ship_army_warrior_rubber_button, transport_rubber_button);
                        @event.getActions().clear();
                    }
                    com.oddlabs.tt.gui.ActionButtonPanel.ResourceAction iron = checkResourceAction(@event, com.oddlabs.tt.input.GameAction.RES_IRON, com.oddlabs.tt.input.GameAction.RES_IRON_DEC, com.oddlabs.tt.input.GameAction.RES_IRON_BATCH, com.oddlabs.tt.input.GameAction.RES_IRON_BATCH_DEC);
                    if (iron.active())
                    {
                        handleArmoryShortcut(true, iron, harvest_iron_button, build_weapon_iron_button, army_warrior_iron_button, ship_army_warrior_iron_button, transport_iron_button);
                        @event.getActions().clear();
                    }
                    com.oddlabs.tt.gui.ActionButtonPanel.ResourceAction tree = checkResourceAction(@event, com.oddlabs.tt.input.GameAction.RES_TREE, com.oddlabs.tt.input.GameAction.RES_TREE_DEC, com.oddlabs.tt.input.GameAction.RES_TREE_BATCH, com.oddlabs.tt.input.GameAction.RES_TREE_BATCH_DEC);
                    if (tree.active())
                    {
                        handleArmoryShortcut(true, tree, harvest_tree_button, null, null, null, transport_tree_button);
                        @event.getActions().clear();
                    }
                    com.oddlabs.tt.gui.ActionButtonPanel.ResourceAction rock = checkResourceAction(@event, com.oddlabs.tt.input.GameAction.RES_ROCK, com.oddlabs.tt.input.GameAction.RES_ROCK_DEC, com.oddlabs.tt.input.GameAction.RES_ROCK_BATCH, com.oddlabs.tt.input.GameAction.RES_ROCK_BATCH_DEC);
                    if (rock.active())
                    {
                        handleArmoryShortcut(true, rock, harvest_rock_button, build_weapon_rock_button, army_warrior_rock_button, ship_army_warrior_rock_button, transport_rock_button);
                        @event.getActions().clear();
                    }
                }
            }
            else if (released)
            {
                com.oddlabs.tt.gui.ActionButtonPanel.ResourceAction chicken_2 = checkResourceAction(@event, com.oddlabs.tt.input.GameAction.RES_CHICKEN, com.oddlabs.tt.input.GameAction.RES_CHICKEN_DEC, com.oddlabs.tt.input.GameAction.RES_CHICKEN_BATCH, com.oddlabs.tt.input.GameAction.RES_CHICKEN_BATCH_DEC);
                if (chicken_2.active())
                {
                    handleArmoryShortcut(false, chicken_2, harvest_rubber_button, build_weapon_rubber_button, army_warrior_rubber_button, ship_army_warrior_rubber_button, transport_rubber_button);
                }
                else
                {
                    com.oddlabs.tt.gui.ActionButtonPanel.ResourceAction peon_2 = checkResourceAction(@event, com.oddlabs.tt.input.GameAction.TRAIN_PEON, com.oddlabs.tt.input.GameAction.TRAIN_PEON_DEC, com.oddlabs.tt.input.GameAction.TRAIN_PEON_BATCH, com.oddlabs.tt.input.GameAction.TRAIN_PEON_BATCH_DEC);
                    if (peon_2.active())
                    {
                        if (current_quarters)
                        {
                            quarters_peon_button.shortcutReleased(peon_2.decrement(), peon_2.batch());
                            @event.getActions().clear();
                        }
                        else if (current_armory && ((object)current_submenu == (object)army_group))
                        {
                            army_peon_button.shortcutReleased(peon_2.decrement(), peon_2.batch());
                            @event.getActions().clear();
                        }
                        else if (current_ship && ((object)current_submenu == (object)ship_army_group))
                        {
                            ship_army_peon_button.shortcutReleased(peon_2.decrement(), peon_2.batch());
                            @event.getActions().clear();
                        }
                    }
                    else
                    {
                        com.oddlabs.tt.gui.ActionButtonPanel.ResourceAction iron_2 = checkResourceAction(@event, com.oddlabs.tt.input.GameAction.RES_IRON, com.oddlabs.tt.input.GameAction.RES_IRON_DEC, com.oddlabs.tt.input.GameAction.RES_IRON_BATCH, com.oddlabs.tt.input.GameAction.RES_IRON_BATCH_DEC);
                        if (iron_2.active())
                        {
                            handleArmoryShortcut(false, iron_2, harvest_iron_button, build_weapon_iron_button, army_warrior_iron_button, ship_army_warrior_iron_button, transport_iron_button);
                        }
                        else
                        {
                            com.oddlabs.tt.gui.ActionButtonPanel.ResourceAction rock_2 = checkResourceAction(@event, com.oddlabs.tt.input.GameAction.RES_ROCK, com.oddlabs.tt.input.GameAction.RES_ROCK_DEC, com.oddlabs.tt.input.GameAction.RES_ROCK_BATCH, com.oddlabs.tt.input.GameAction.RES_ROCK_BATCH_DEC);
                            if (rock_2.active())
                            {
                                handleArmoryShortcut(false, rock_2, harvest_rock_button, build_weapon_rock_button, army_warrior_rock_button, ship_army_warrior_rock_button, transport_rock_button);
                            }
                            else
                            {
                                com.oddlabs.tt.gui.ActionButtonPanel.ResourceAction tree_2 = checkResourceAction(@event, com.oddlabs.tt.input.GameAction.RES_TREE, com.oddlabs.tt.input.GameAction.RES_TREE_DEC, com.oddlabs.tt.input.GameAction.RES_TREE_BATCH, com.oddlabs.tt.input.GameAction.RES_TREE_BATCH_DEC);
                                if (tree_2.active())
                                {
                                    handleArmoryShortcut(false, tree_2, harvest_tree_button, null, null, null, transport_tree_button);
                                }
                                else if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UNIT_BUILD_TOWER) || @event.consumeAction(com.oddlabs.tt.input.GameAction.PROD_TRANSPORT))
                                {
                                    if (current_armory && current_submenu == null)
                                    {
                                        transport_button.mouseClickedAll(com.oddlabs.tt.gui.MouseButton.LEFT, 0, 0, 1);
                                    }
                                    else if (current_ship && current_submenu == null)
                                    {
                                        ship_transport_button.mouseClickedAll(com.oddlabs.tt.gui.MouseButton.LEFT, 0, 0, 1);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        // Java: 1135
        public void activate(com.oddlabs.tt.input.InputEvent @event, com.oddlabs.tt.gui.GUIObject button)
        {
            button.mouseClickedAll(com.oddlabs.tt.gui.MouseButton.LEFT, 0, 0, 1);
            @event.consume();
        }
        // Java: 1140
        public class ResourceAction : java.lang.Object
        {
            public bool active() { return active_f; }
            public bool decrement() { return decrement_f; }
            public bool batch() { return batch_f; }
            public ResourceAction(bool active, bool decrement, bool batch)
            {
                this.active_f = active;
                this.decrement_f = decrement;
                this.batch_f = batch;
            }
            public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is ResourceAction r)) return false; return true && java.lang.JO.equals(active_f, r.active_f) && java.lang.JO.equals(decrement_f, r.decrement_f) && java.lang.JO.equals(batch_f, r.batch_f); }
            public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(active_f); h = 31 * h + java.lang.JO.hashCode(decrement_f); h = 31 * h + java.lang.JO.hashCode(batch_f); } return h; }
            public override string toString() { return "ResourceAction[" + "active=" + java.lang.JO.toString(active_f) + ", decrement=" + java.lang.JO.toString(decrement_f) + ", batch=" + java.lang.JO.toString(batch_f) + "]"; }
            public bool active_f;
            public bool decrement_f;
            public bool batch_f;
        }
        // Java: 1143
        public com.oddlabs.tt.gui.ActionButtonPanel.ResourceAction checkResourceAction(com.oddlabs.tt.input.InputEvent @event, com.oddlabs.tt.input.GameAction @base, com.oddlabs.tt.input.GameAction dec, com.oddlabs.tt.input.GameAction batch, com.oddlabs.tt.input.GameAction batchDec)
        {
            if (@event.consumeAction(@base))
            {
                return new com.oddlabs.tt.gui.ActionButtonPanel.ResourceAction(true, false, false);
            }
            if (@event.consumeAction(dec))
            {
                return new com.oddlabs.tt.gui.ActionButtonPanel.ResourceAction(true, true, false);
            }
            if (@event.consumeAction(batch))
            {
                return new com.oddlabs.tt.gui.ActionButtonPanel.ResourceAction(true, false, true);
            }
            if (@event.consumeAction(batchDec))
            {
                return new com.oddlabs.tt.gui.ActionButtonPanel.ResourceAction(true, true, true);
            }
            return new com.oddlabs.tt.gui.ActionButtonPanel.ResourceAction(false, false, false);
        }
        // Java: 1152
        public void handleArmoryShortcut(bool pressed, com.oddlabs.tt.gui.ActionButtonPanel.ResourceAction action, com.oddlabs.tt.gui.IconSpinner harvestBtn, com.oddlabs.tt.gui.IconSpinner buildBtn, com.oddlabs.tt.gui.IconSpinner armyBtn, com.oddlabs.tt.gui.IconSpinner shipArmyBtn, com.oddlabs.tt.gui.IconSpinner transportBtn)
        {
            if (!current_armory && !current_ship)
            {
                return;
            }
            com.oddlabs.tt.gui.IconSpinner target = null;
            if (((object)current_submenu == (object)harvest_group))
            {
                target = harvestBtn;
            }
            else if (((object)current_submenu == (object)build_group))
            {
                target = buildBtn;
            }
            else if (((object)current_submenu == (object)army_group))
            {
                target = armyBtn;
            }
            else if (((object)current_submenu == (object)ship_army_group))
            {
                target = shipArmyBtn;
            }
            else if (((object)current_submenu == (object)transport_group))
            {
                target = transportBtn;
            }
            if (target != null)
            {
                if (pressed)
                {
                    target.shortcutPressed(action.decrement(), action.batch());
                }
                else
                {
                    target.shortcutReleased(action.decrement(), action.batch());
                }
            }
        }
        // Java: 1173
        public override bool canHoverBehind()
        {
            return true;
        }
        public const int SUBMENU_NONE = 0;
        public const int SUBMENU_HARVEST = 1;
        public const int SUBMENU_BUILD = 2;
        public const int SUBMENU_ARMY = 3;
        public const int SUBMENU_TRANSPORT = 4;
        // Java: 1184
        public int getSubmenu()
        {
            if (((object)current_submenu == (object)harvest_group))
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.SUBMENU_HARVEST;
            }
            if (((object)current_submenu == (object)build_group))
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.SUBMENU_BUILD;
            }
            if (((object)current_submenu == (object)army_group))
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.SUBMENU_ARMY;
            }
            if (((object)current_submenu == (object)transport_group))
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.SUBMENU_TRANSPORT;
            }
            return com.oddlabs.tt.gui.ActionButtonPanel.SUBMENU_NONE;
        }
        // Java: 1196
        public void setSubmenu(int submenu)
        {
            if (submenu == getSubmenu() || (submenu != com.oddlabs.tt.gui.ActionButtonPanel.SUBMENU_NONE && !current_armory && !current_ship))
            {
                return;
            }
            switch ((submenu))
            {
                case 1:
                {
                    openSubmenu(harvest_group);
                    break;
                }
                case 2:
                {
                    openSubmenu(build_group);
                    updateCounters();
                    break;
                }
                case 3:
                {
                    openSubmenu(army_group);
                    break;
                }
                case 4:
                {
                    openSubmenu(transport_group);
                    break;
                }
                default:
                {
                    removeGroups();
                    update = true;
                    break;
                }
            }
        }
        // Java: 1214
        public bool inHarvestMenu()
        {
            return ((object)current_submenu == (object)harvest_group);
        }
        // Java: 1218
        public bool inBuildMenu()
        {
            return ((object)current_submenu == (object)build_group);
        }
        // Java: 1222
        public bool inArmyMenu()
        {
            return ((object)current_submenu == (object)army_group) || ((object)current_submenu == (object)ship_army_group);
        }
        // Java: 1226
        public bool inTransportMenu()
        {
            return ((object)current_submenu == (object)transport_group);
        }
        // Java: 1231
        public override void mouseDragged(com.oddlabs.tt.gui.MouseButton button, int x, int y, int relative_x, int relative_y, int absolute_x, int absolute_y)
        {
            if (((com.oddlabs.tt.gui.GUIObject)getParent()) != null)
            {
                ((com.oddlabs.tt.gui.GUIObject)getParent()).mouseDragged(button, x, y, relative_x, relative_y, absolute_x, absolute_y);
            }
        }
        // Java: 1238
        public void setRallyPoint(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
        {
            if (current_building != null && !current_building.isDead())
            {
                pushDelegate(new com.oddlabs.tt.@delegate.RallyPointDelegate(viewer, camera, current_building));
            }
            removeGroups();
            update = true;
        }
        // Java: 1246
        public void pushDelegate(com.oddlabs.tt.@delegate.CameraDelegate @delegate)
        {
            com.oddlabs.tt.gui.GUIRoot root = viewer.getGUIRoot();
            com.oddlabs.tt.@delegate.CameraDelegate current = root.getDelegate();
            if ((current is com.oddlabs.tt.@delegate.TargetDelegate) || (current is com.oddlabs.tt.@delegate.PlacingDelegate) || (current is com.oddlabs.tt.@delegate.RallyPointDelegate))
            {
                root.removeDelegate(current);
            }
            root.pushDelegate(@delegate);
        }
        // Java: 1256
        public void deployChieftainFromShip(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
        {
            if (current_ship && (current_building is com.oddlabs.tt.model.Ship ship))
            {
                viewer.getPeerHub().getPlayerInterface().deployChieftainFromShip(ship);
            }
        }
        // Java: 1262
        public void cancelSubMenu(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
        {
            removeGroups();
            update = true;
        }
        // Java: 1267
        public void openSubmenu(com.oddlabs.tt.gui.Group submenu)
        {
            if (current_submenu != null)
            {
                current_submenu.remove();
            }
            armory_group.remove();
            ship_group.remove();
            addChild(submenu);
            current_submenu = submenu;
        }
        // Java: 1281
        public bool canSwitchSubmenu(com.oddlabs.tt.input.InputEvent @event)
        {
            if (current_submenu == null)
            {
                return true;
            }
            foreach (com.oddlabs.tt.input.GameAction action in java.lang.JIter.of(com.oddlabs.tt.gui.ActionButtonPanel.ARMORY_SUBMENU_ACTIONS))
            {
                if (@event.hasAction(action))
                {
                    return false;
                }
            }
            return true;
        }
        // Java: 1296
        public bool tryCloseSubmenu(com.oddlabs.tt.input.InputEvent @event)
        {
            if ((current_armory || current_ship) && current_submenu != null)
            {
                @event.consumeAction(com.oddlabs.tt.input.GameAction.GLOBAL_MENU);
                @event.consumeAction(com.oddlabs.tt.input.GameAction.UI_CANCEL);
                removeGroups();
                update = true;
                return true;
            }
            return false;
        }
        // Java: 1307
        public bool suppliesEmpty(params com.oddlabs.tt.model.SupplyCounter[] counters)
        {
            return java.util.Arrays.stream(counters).anyMatch(new java.util.function.Predicate_L((object c_l) =>
            {
                com.oddlabs.tt.model.SupplyCounter c = (com.oddlabs.tt.model.SupplyCounter)c_l;
                return c.getNumSupplies() == 0;
            }));
        }
        static ActionButtonPanel()
        {
            ActionButtonPanel.ARMORY_SUBMENU_ACTIONS = java.util.EnumSet.of(com.oddlabs.tt.input.GameAction.RES_TREE, com.oddlabs.tt.input.GameAction.RES_TREE_DEC, com.oddlabs.tt.input.GameAction.RES_TREE_BATCH, com.oddlabs.tt.input.GameAction.RES_TREE_BATCH_DEC, com.oddlabs.tt.input.GameAction.RES_ROCK, com.oddlabs.tt.input.GameAction.RES_ROCK_DEC, com.oddlabs.tt.input.GameAction.RES_ROCK_BATCH, com.oddlabs.tt.input.GameAction.RES_ROCK_BATCH_DEC, com.oddlabs.tt.input.GameAction.RES_IRON, com.oddlabs.tt.input.GameAction.RES_IRON_DEC, com.oddlabs.tt.input.GameAction.RES_IRON_BATCH, com.oddlabs.tt.input.GameAction.RES_IRON_BATCH_DEC, com.oddlabs.tt.input.GameAction.RES_CHICKEN, com.oddlabs.tt.input.GameAction.RES_CHICKEN_DEC, com.oddlabs.tt.input.GameAction.RES_CHICKEN_BATCH, com.oddlabs.tt.input.GameAction.RES_CHICKEN_BATCH_DEC, com.oddlabs.tt.input.GameAction.TRAIN_PEON, com.oddlabs.tt.input.GameAction.TRAIN_PEON_DEC, com.oddlabs.tt.input.GameAction.TRAIN_PEON_BATCH, com.oddlabs.tt.input.GameAction.TRAIN_PEON_BATCH_DEC, com.oddlabs.tt.input.GameAction.GAMEPLAY_BACK);
            ActionButtonPanel.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.gui.ActionButtonPanel).getName());
        }
        private void init_ActionButtonPanel()
        {
            this.unit_group = new com.oddlabs.tt.gui.NonFocusGroup();
            this.peon_group = new com.oddlabs.tt.gui.NonFocusGroup();
            this.chieftain_group = new com.oddlabs.tt.gui.NonFocusGroup();
            this.tower_group = new com.oddlabs.tt.gui.NonFocusGroup();
            this.quarters_status_group = new com.oddlabs.tt.gui.NonFocusGroup();
            this.quarters_group = new com.oddlabs.tt.gui.NonFocusGroup();
            this.status_group = new com.oddlabs.tt.gui.NonFocusGroup();
            this.armory_group = new com.oddlabs.tt.gui.NonFocusGroup();
            this.ship_group = new com.oddlabs.tt.gui.NonFocusGroup();
            this.harvest_group = new com.oddlabs.tt.gui.NonFocusGroup();
            this.build_group = new com.oddlabs.tt.gui.NonFocusGroup();
            this.army_group = new com.oddlabs.tt.gui.NonFocusGroup();
            this.ship_army_group = new com.oddlabs.tt.gui.NonFocusGroup();
            this.transport_group = new com.oddlabs.tt.gui.NonFocusGroup();
            this.current_submenu = null;
            this.update = false;
            this.current_quarters = false;
            this.current_armory = false;
            this.current_ship = false;
            this.current_unit = false;
            this.current_peon = false;
            this.current_magic1 = false;
            this.current_magic2 = false;
            this.current_tower = false;
        }
        public ActionButtonPanel(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.camera.GameCamera camera) : this(viewer, camera, viewer.getGUIRoot().getWidth(), viewer.getGUIRoot().getHeight())
        {
        }
        public ActionButtonPanel(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.camera.GameCamera camera, int width, int height) : this(viewer, camera, width, height, viewer.getLocalPlayer(), viewer.getSelection(), false)
        {
        }
        public ActionButtonPanel(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.camera.GameCamera camera, int width, int height, com.oddlabs.tt.player.Player player, com.oddlabs.tt.viewer.Selection selection, bool read_only) : base()
        {
            init_ActionButtonPanel();
            this.viewer = viewer;
            this.camera = camera;
            this.player = player;
            this.selection = selection;
            this.read_only = read_only;
            com.oddlabs.tt.gui.RaceIcons race_icons = player.getRace().getIcons();
            com.oddlabs.tt.gui.Skin skin = com.oddlabs.tt.gui.Skin.getSkin();
            com.oddlabs.tt.gui.GUIIcons icons = com.oddlabs.tt.gui.GUIIcons.getIcons();
            string widest_char = java.lang.JString.create(java.lang.Character.toChars(skin.getEditFont().getWidestCodepoint("0123456789")));
            int label_width = skin.getEditFont().getWidth(java.lang.JStr.valueOf(widest_char) + java.lang.JStr.valueOf(widest_char) + java.lang.JStr.valueOf(widest_char));
            move_button = new com.oddlabs.tt.gui.NonFocusIconButton(race_icons.moveIcon(), com.oddlabs.tt.input.GameAction.UNIT_MOVE, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("move_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.UNIT_MOVE));
            }));
            move_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return !player.canMove();
            }));
            unit_group.addChild(move_button);
            move_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                pushDelegate(new com.oddlabs.tt.@delegate.TargetDelegate(viewer, camera, com.oddlabs.tt.model.Action.MOVE));
            }));
            attack_button = new com.oddlabs.tt.gui.NonFocusIconButton(race_icons.attackIcon(), com.oddlabs.tt.input.GameAction.UNIT_ATTACK, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("attack_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.UNIT_ATTACK));
            }));
            attack_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return !player.canAttack();
            }));
            unit_group.addChild(attack_button);
            attack_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_5, int unused_6, int unused_7, int unused_8) =>
            {
                pushDelegate(new com.oddlabs.tt.@delegate.TargetDelegate(viewer, camera, com.oddlabs.tt.model.Action.ATTACK));
            }));
            move_button.place();
            attack_button.place(move_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            unit_group.compileCanvas(com.oddlabs.tt.gui.ActionButtonPanel.GROUP_LEFT_OFFSET, 0, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_RIGHT_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_BOTTOM_OFFSET);
            gather_repair_button = new com.oddlabs.tt.gui.NonFocusIconButton(race_icons.gatherRepairIcon(), com.oddlabs.tt.input.GameAction.UNIT_GATHER, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("gather_repair_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.UNIT_GATHER));
            }));
            peon_group.addChild(gather_repair_button);
            gather_repair_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_9, int unused_10, int unused_11, int unused_12) =>
            {
                pushDelegate(new com.oddlabs.tt.@delegate.TargetDelegate(viewer, camera, com.oddlabs.tt.model.Action.GATHER_REPAIR));
            }));
            gather_repair_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return !player.canRepair();
            }));
            quarters_button = new com.oddlabs.tt.gui.NonFocusIconButton(race_icons.quartersIcon(), com.oddlabs.tt.input.GameAction.UNIT_BUILD_QUARTERS, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("quarters_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.UNIT_BUILD_QUARTERS));
            }));
            peon_group.addChild(quarters_button);
            quarters_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_13, int unused_14, int unused_15, int unused_16) =>
            {
                pushDelegate(new com.oddlabs.tt.@delegate.PlacingDelegate(viewer, camera.getState(), com.oddlabs.tt.model.Race.BUILDING_QUARTERS));
            }));
            quarters_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return !player.canBuild(com.oddlabs.tt.model.Race.BUILDING_QUARTERS);
            }));
            armory_button = new com.oddlabs.tt.gui.NonFocusIconButton(race_icons.armoryIcon(), com.oddlabs.tt.input.GameAction.UNIT_BUILD_ARMORY, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("armory_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.UNIT_BUILD_ARMORY));
            }));
            peon_group.addChild(armory_button);
            armory_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_17, int unused_18, int unused_19, int unused_20) =>
            {
                pushDelegate(new com.oddlabs.tt.@delegate.PlacingDelegate(viewer, camera.getState(), com.oddlabs.tt.model.Race.BUILDING_ARMORY));
            }));
            armory_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return !player.canBuild(com.oddlabs.tt.model.Race.BUILDING_ARMORY);
            }));
            tower_button = new com.oddlabs.tt.gui.NonFocusIconButton(race_icons.towerIcon(), com.oddlabs.tt.input.GameAction.UNIT_BUILD_TOWER, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("tower_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.UNIT_BUILD_TOWER));
            }));
            peon_group.addChild(tower_button);
            tower_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_21, int unused_22, int unused_23, int unused_24) =>
            {
                pushDelegate(new com.oddlabs.tt.@delegate.PlacingDelegate(viewer, camera.getState(), com.oddlabs.tt.model.Race.BUILDING_TOWER));
            }));
            tower_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return !player.canBuild(com.oddlabs.tt.model.Race.BUILDING_TOWER);
            }));
            ship_button = new com.oddlabs.tt.gui.NonFocusIconButton(race_icons.shipIcon(), com.oddlabs.tt.input.GameAction.UNIT_BUILD_SHIP, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("ship_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.UNIT_BUILD_SHIP));
            }));
            if (viewer.getWorld().isShipsEnabled())
            {
                peon_group.addChild(ship_button);
                ship_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_25, int unused_26, int unused_27, int unused_28) =>
                {
                    pushDelegate(new com.oddlabs.tt.@delegate.PlacingDelegate(viewer, camera.getState(), com.oddlabs.tt.model.Race.BUILDING_SHIP));
                }));
                ship_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
                {
                    return !player.canBuild(com.oddlabs.tt.model.Race.BUILDING_SHIP);
                }));
            }
            gather_repair_button.place();
            quarters_button.place(gather_repair_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            armory_button.place(quarters_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            tower_button.place(armory_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            if (viewer.getWorld().isShipsEnabled())
            {
                ship_button.place(tower_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            }
            peon_group.compileCanvas(com.oddlabs.tt.gui.ActionButtonPanel.GROUP_LEFT_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_BOTTOM_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_RIGHT_OFFSET, 0);
            com.oddlabs.tt.player.PlayerInterface player_interface = viewer.getPeerHub().getPlayerInterface();
            magic1_button = new com.oddlabs.tt.gui.RechargeButton(player_interface, race_icons.magic1Icon(), com.oddlabs.tt.input.GameAction.MAGIC_1, race_icons.magic1Desc(), 0);
            chieftain_group.addChild(magic1_button);
            magic2_button = new com.oddlabs.tt.gui.RechargeButton(player_interface, race_icons.magic2Icon(), com.oddlabs.tt.input.GameAction.MAGIC_2, race_icons.magic2Desc(), 1);
            chieftain_group.addChild(magic2_button);
            magic1_button.place();
            magic2_button.place(magic1_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            chieftain_group.compileCanvas(com.oddlabs.tt.gui.ActionButtonPanel.GROUP_LEFT_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_BOTTOM_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_RIGHT_OFFSET, 0);
            tower_attack_button = new com.oddlabs.tt.gui.NonFocusIconButton(race_icons.attackIcon(), com.oddlabs.tt.input.GameAction.UNIT_ATTACK, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("attack_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.UNIT_ATTACK));
            }));
            tower_group.addChild(tower_attack_button);
            tower_attack_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_29, int unused_30, int unused_31, int unused_32) =>
            {
                pushDelegate(new com.oddlabs.tt.@delegate.TargetDelegate(viewer, camera, com.oddlabs.tt.model.Action.ATTACK));
            }));
            tower_exit_button = new com.oddlabs.tt.gui.NonFocusIconButton(race_icons.towerExitIcon(), com.oddlabs.tt.input.GameAction.UNIT_EXIT_TOWER, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("exit_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.UNIT_EXIT_TOWER));
            }));
            tower_group.addChild(tower_exit_button);
            tower_exit_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_33, int unused_34, int unused_35, int unused_36) =>
            {
                if (current_building != null && !current_building.isDead())
                {
                    viewer.getPeerHub().getPlayerInterface().exitTower(current_building);
                }
                removeGroups();
                update = true;
            }));
            tower_attack_button.place();
            tower_exit_button.place(tower_attack_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            tower_group.compileCanvas();
            unit_status = new com.oddlabs.tt.gui.StatusIcon(label_width, race_icons.unitStatusIcon(), com.oddlabs.tt.gui.ActionButtonPanel.i18n("units_tip"));
            status_group.addChild(unit_status);
            weapon_rock_status = new com.oddlabs.tt.gui.StatusIcon(label_width, race_icons.weaponRockStatusIcon(), com.oddlabs.tt.gui.ActionButtonPanel.i18n("rock_weapons_tip"));
            status_group.addChild(weapon_rock_status);
            weapon_iron_status = new com.oddlabs.tt.gui.StatusIcon(label_width, race_icons.weaponIronStatusIcon(), com.oddlabs.tt.gui.ActionButtonPanel.i18n("iron_weapons_tip"));
            status_group.addChild(weapon_iron_status);
            weapon_rubber_status = new com.oddlabs.tt.gui.StatusIcon(label_width, race_icons.weaponRubberStatusIcon(), com.oddlabs.tt.gui.ActionButtonPanel.i18n("chicken_weapons_tip"));
            status_group.addChild(weapon_rubber_status);
            tree_status = new com.oddlabs.tt.gui.StatusIcon(label_width, icons.getTreeStatusIcon(), com.oddlabs.tt.gui.ActionButtonPanel.i18n("tree_resources_tip"));
            status_group.addChild(tree_status);
            rock_status = new com.oddlabs.tt.gui.StatusIcon(label_width, icons.getRockStatusIcon(), com.oddlabs.tt.gui.ActionButtonPanel.i18n("rock_resources_tip"));
            status_group.addChild(rock_status);
            iron_status = new com.oddlabs.tt.gui.StatusIcon(label_width, icons.getIronStatusIcon(), com.oddlabs.tt.gui.ActionButtonPanel.i18n("iron_resources_tip"));
            status_group.addChild(iron_status);
            rubber_status = new com.oddlabs.tt.gui.StatusIcon(label_width, icons.getRubberStatusIcon(), com.oddlabs.tt.gui.ActionButtonPanel.i18n("chicken_resources_tip"));
            status_group.addChild(rubber_status);
            unit_status.place();
            weapon_rock_status.place(unit_status, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            weapon_iron_status.place(weapon_rock_status, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            weapon_rubber_status.place(weapon_iron_status, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            tree_status.place(unit_status, com.oddlabs.tt.gui.Placement.LEFT_MID, 5);
            rock_status.place(tree_status, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            iron_status.place(rock_status, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            rubber_status.place(iron_status, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            status_group.compileCanvas(5, 5, 5, 5);
            quarters_unit_status = new com.oddlabs.tt.gui.WatchStatusIcon(label_width, race_icons.unitStatusIcon(), com.oddlabs.tt.gui.ActionButtonPanel.i18n("units_tip"));
            quarters_status_group.addChild(quarters_unit_status);
            quarters_unit_status.place();
            quarters_status_group.compileCanvas(5, 5, 5, 5);
            quarters_peon_button = new com.oddlabs.tt.gui.DeploySpinner(viewer, player_interface, race_icons.peonIcon(), com.oddlabs.tt.gui.ActionButtonPanel.i18n("deploy_peon_tip"), java.util.List_.of(race_icons.unitStatusIcon()), com.oddlabs.tt.input.GameAction.TRAIN_PEON, com.oddlabs.tt.input.GameAction.TRAIN_PEON_DEC, null, null);
            quarters_group.addChild(quarters_peon_button);
            quarters_chieftain_button = new com.oddlabs.tt.gui.ChieftainButton(viewer, player_interface, race_icons.chieftainIcon());
            quarters_group.addChild(quarters_chieftain_button);
            quarters_rally_point_button = new com.oddlabs.tt.gui.NonFocusIconButton(race_icons.rallyPointIcon(), com.oddlabs.tt.input.GameAction.UNIT_SET_RALLY, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("rally_point_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.UNIT_SET_RALLY));
            }));
            quarters_group.addChild(quarters_rally_point_button);
            quarters_rally_point_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton a0, int a1, int a2, int a3) => { this.setRallyPoint(a0, a1, a2, a3); }));
            quarters_peon_button.place();
            quarters_chieftain_button.place(quarters_peon_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            quarters_rally_point_button.place(quarters_chieftain_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            quarters_group.compileCanvas(com.oddlabs.tt.gui.ActionButtonPanel.GROUP_LEFT_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_BOTTOM_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_RIGHT_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_TOP_OFFSET);
            harvest_button = new com.oddlabs.tt.gui.NonFocusIconButton(icons.getHarvestIcon(), com.oddlabs.tt.input.GameAction.PROD_HARVEST, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("gather_resources_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.PROD_HARVEST));
            }));
            harvest_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return !player.canHarvest();
            }));
            armory_group.addChild(harvest_button);
            harvest_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_37, int unused_38, int unused_39, int unused_40) =>
            {
                openSubmenu(harvest_group);
            }));
            build_button = new com.oddlabs.tt.gui.NonFocusIconButton(race_icons.buildWeaponsIcon(), com.oddlabs.tt.input.GameAction.PROD_WEAPONS, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("produce_weapons_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.PROD_WEAPONS));
            }));
            build_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return !player.canBuildWeapons();
            }));
            armory_group.addChild(build_button);
            build_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_41, int unused_42, int unused_43, int unused_44) =>
            {
                openSubmenu(build_group);
                updateCounters();
            }));
            army_button = new com.oddlabs.tt.gui.NonFocusIconButton(race_icons.armyIcon(), com.oddlabs.tt.input.GameAction.PROD_ARMY, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("deploy_army_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.PROD_ARMY));
            }));
            army_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return !player.canBuildArmies();
            }));
            armory_group.addChild(army_button);
            army_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_45, int unused_46, int unused_47, int unused_48) =>
            {
                openSubmenu(army_group);
            }));
            transport_button = new com.oddlabs.tt.gui.NonFocusIconButton(race_icons.transportIcon(), com.oddlabs.tt.input.GameAction.PROD_TRANSPORT, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("transport_resources_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.PROD_TRANSPORT));
            }));
            armory_group.addChild(transport_button);
            transport_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_49, int unused_50, int unused_51, int unused_52) =>
            {
                openSubmenu(transport_group);
            }));
            rally_point_button = new com.oddlabs.tt.gui.NonFocusIconButton(race_icons.rallyPointIcon(), com.oddlabs.tt.input.GameAction.UNIT_SET_RALLY, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("rally_point_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.UNIT_SET_RALLY));
            }));
            rally_point_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return !player.canSetRallyPoints();
            }));
            armory_group.addChild(rally_point_button);
            rally_point_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton a0, int a1, int a2, int a3) => { this.setRallyPoint(a0, a1, a2, a3); }));
            harvest_button.place();
            build_button.place(harvest_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            army_button.place(build_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            transport_button.place(army_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            rally_point_button.place(transport_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            armory_group.compileCanvas(com.oddlabs.tt.gui.ActionButtonPanel.GROUP_LEFT_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_BOTTOM_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_RIGHT_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_TOP_OFFSET);
            ship_harvest_button = new com.oddlabs.tt.gui.NonFocusIconButton(icons.getHarvestIcon(), com.oddlabs.tt.input.GameAction.PROD_HARVEST, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("gather_resources_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.PROD_HARVEST));
            }));
            ship_group.addChild(ship_harvest_button);
            ship_harvest_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_53, int unused_54, int unused_55, int unused_56) =>
            {
                openSubmenu(harvest_group);
            }));
            ship_army_button = new com.oddlabs.tt.gui.NonFocusIconButton(race_icons.armyIcon(), com.oddlabs.tt.input.GameAction.PROD_ARMY, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("deploy_army_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.PROD_ARMY));
            }));
            ship_group.addChild(ship_army_button);
            ship_army_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_57, int unused_58, int unused_59, int unused_60) =>
            {
                openSubmenu(ship_army_group);
            }));
            ship_transport_button = new com.oddlabs.tt.gui.NonFocusIconButton(race_icons.transportIcon(), com.oddlabs.tt.input.GameAction.PROD_TRANSPORT, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("transport_resources_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.PROD_TRANSPORT));
            }));
            ship_group.addChild(ship_transport_button);
            ship_transport_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_61, int unused_62, int unused_63, int unused_64) =>
            {
                openSubmenu(transport_group);
            }));
            ship_rally_point_button = new com.oddlabs.tt.gui.NonFocusIconButton(race_icons.rallyPointIcon(), com.oddlabs.tt.input.GameAction.UNIT_SET_RALLY, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("rally_point_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.UNIT_SET_RALLY));
            }));
            ship_group.addChild(ship_rally_point_button);
            ship_rally_point_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton a0, int a1, int a2, int a3) => { this.setRallyPoint(a0, a1, a2, a3); }));
            ship_harvest_button.place();
            ship_army_button.place(ship_harvest_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            ship_transport_button.place(ship_army_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            ship_rally_point_button.place(ship_transport_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            ship_sail_button = new com.oddlabs.tt.gui.NonFocusIconButton(race_icons.shipIcon(), com.oddlabs.tt.input.GameAction.UNIT_MOVE, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("sail_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.UNIT_MOVE));
            }));
            ship_group.addChild(ship_sail_button);
            ship_sail_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_65, int unused_66, int unused_67, int unused_68) =>
            {
                pushDelegate(new com.oddlabs.tt.@delegate.TargetDelegate(viewer, camera, com.oddlabs.tt.model.Action.MOVE, true));
            }));
            ship_sail_button.place(ship_rally_point_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            ship_group.compileCanvas(com.oddlabs.tt.gui.ActionButtonPanel.GROUP_LEFT_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_BOTTOM_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_RIGHT_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_TOP_OFFSET);
            com.oddlabs.tt.player.Player local_player = player;
            harvest_tree_button = new com.oddlabs.tt.gui.DeploySpinner(viewer, player_interface, icons.getTreeIcon(), com.oddlabs.tt.gui.ActionButtonPanel.i18n("harvest_tree_tip"), java.util.List_.of(race_icons.unitStatusIcon()), com.oddlabs.tt.input.GameAction.RES_TREE, com.oddlabs.tt.input.GameAction.RES_TREE_DEC, local_player, typeof(com.oddlabs.tt.landscape.TreeSupply));
            harvest_group.addChild(harvest_tree_button);
            harvest_rock_button = new com.oddlabs.tt.gui.DeploySpinner(viewer, player_interface, icons.getRockIcon(), com.oddlabs.tt.gui.ActionButtonPanel.i18n("harvest_rock_tip"), java.util.List_.of(race_icons.unitStatusIcon()), com.oddlabs.tt.input.GameAction.RES_ROCK, com.oddlabs.tt.input.GameAction.RES_ROCK_DEC, local_player, typeof(com.oddlabs.tt.model.RockSupply));
            harvest_group.addChild(harvest_rock_button);
            harvest_iron_button = new com.oddlabs.tt.gui.DeploySpinner(viewer, player_interface, icons.getIronIcon(), com.oddlabs.tt.gui.ActionButtonPanel.i18n("harvest_iron_tip"), java.util.List_.of(race_icons.unitStatusIcon()), com.oddlabs.tt.input.GameAction.RES_IRON, com.oddlabs.tt.input.GameAction.RES_IRON_DEC, local_player, typeof(com.oddlabs.tt.model.IronSupply));
            harvest_group.addChild(harvest_iron_button);
            harvest_rubber_button = new com.oddlabs.tt.gui.DeploySpinner(viewer, player_interface, icons.getRubberIcon(), com.oddlabs.tt.gui.ActionButtonPanel.i18n("harvest_chicken_tip"), java.util.List_.of(race_icons.unitStatusIcon()), com.oddlabs.tt.input.GameAction.RES_CHICKEN, com.oddlabs.tt.input.GameAction.RES_CHICKEN_DEC, local_player, typeof(com.oddlabs.tt.model.RubberSupply));
            harvest_group.addChild(harvest_rubber_button);
            harvest_back_button = new com.oddlabs.tt.gui.NonFocusIconButton(skin.getBackButton(), com.oddlabs.tt.input.GameAction.GAMEPLAY_BACK, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("back_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.GAMEPLAY_BACK));
            }));
            harvest_back_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton a0, int a1, int a2, int a3) => { this.cancelSubMenu(a0, a1, a2, a3); }));
            harvest_group.addChild(harvest_back_button);
            harvest_tree_button.place();
            harvest_rock_button.place(harvest_tree_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            harvest_iron_button.place(harvest_rock_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            harvest_rubber_button.place(harvest_iron_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            harvest_back_button.place(harvest_rubber_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            harvest_group.compileCanvas(com.oddlabs.tt.gui.ActionButtonPanel.GROUP_LEFT_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_BOTTOM_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_RIGHT_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_TOP_OFFSET);
            build_weapon_rock_button = new com.oddlabs.tt.gui.BuildSpinner(viewer, player_interface, race_icons.buildWeaponRockIcon(), com.oddlabs.tt.gui.ActionButtonPanel.i18n("build_rock_tip"), com.oddlabs.tt.model.LandBuilding.COST_ROCK_WEAPON.iconList(), com.oddlabs.tt.input.GameAction.RES_ROCK, com.oddlabs.tt.input.GameAction.RES_ROCK_DEC);
            build_group.addChild(build_weapon_rock_button);
            build_weapon_iron_button = new com.oddlabs.tt.gui.BuildSpinner(viewer, player_interface, race_icons.buildWeaponIronIcon(), com.oddlabs.tt.gui.ActionButtonPanel.i18n("build_iron_tip"), com.oddlabs.tt.model.LandBuilding.COST_IRON_WEAPON.iconList(), com.oddlabs.tt.input.GameAction.RES_IRON, com.oddlabs.tt.input.GameAction.RES_IRON_DEC);
            build_group.addChild(build_weapon_iron_button);
            build_weapon_rubber_button = new com.oddlabs.tt.gui.BuildSpinner(viewer, player_interface, race_icons.buildWeaponRubberIcon(), com.oddlabs.tt.gui.ActionButtonPanel.i18n("build_chicken_tip"), com.oddlabs.tt.model.LandBuilding.COST_RUBBER_WEAPON.iconList(), com.oddlabs.tt.input.GameAction.RES_CHICKEN, com.oddlabs.tt.input.GameAction.RES_CHICKEN_DEC);
            build_group.addChild(build_weapon_rubber_button);
            build_back_button = new com.oddlabs.tt.gui.NonFocusIconButton(skin.getBackButton(), com.oddlabs.tt.input.GameAction.GAMEPLAY_BACK, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("back_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.GAMEPLAY_BACK));
            }));
            build_back_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton a0, int a1, int a2, int a3) => { this.cancelSubMenu(a0, a1, a2, a3); }));
            build_group.addChild(build_back_button);
            build_weapon_rock_button.place();
            build_weapon_iron_button.place(build_weapon_rock_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            build_weapon_rubber_button.place(build_weapon_iron_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            build_back_button.place(build_weapon_rubber_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            build_group.compileCanvas(com.oddlabs.tt.gui.ActionButtonPanel.GROUP_LEFT_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_BOTTOM_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_RIGHT_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_TOP_OFFSET);
            army_peon_button = new com.oddlabs.tt.gui.DeploySpinner(viewer, player_interface, race_icons.peonIcon(), com.oddlabs.tt.gui.ActionButtonPanel.i18n("deploy_peon_tip"), java.util.List_.of(race_icons.unitStatusIcon()), com.oddlabs.tt.input.GameAction.TRAIN_PEON, com.oddlabs.tt.input.GameAction.TRAIN_PEON_DEC, null, null);
            army_group.addChild(army_peon_button);
            army_warrior_rock_button = new com.oddlabs.tt.gui.DeploySpinner(viewer, player_interface, race_icons.warriorRockIcon(), com.oddlabs.tt.gui.ActionButtonPanel.i18n("deploy_rock_tip"), java.util.List_.of(race_icons.unitStatusIcon(), race_icons.weaponRockStatusIcon()), com.oddlabs.tt.input.GameAction.RES_ROCK, com.oddlabs.tt.input.GameAction.RES_ROCK_DEC, null, null);
            army_group.addChild(army_warrior_rock_button);
            army_warrior_iron_button = new com.oddlabs.tt.gui.DeploySpinner(viewer, player_interface, race_icons.warriorIronIcon(), com.oddlabs.tt.gui.ActionButtonPanel.i18n("deploy_iron_tip"), java.util.List_.of(race_icons.unitStatusIcon(), race_icons.weaponIronStatusIcon()), com.oddlabs.tt.input.GameAction.RES_IRON, com.oddlabs.tt.input.GameAction.RES_IRON_DEC, null, null);
            army_group.addChild(army_warrior_iron_button);
            army_warrior_rubber_button = new com.oddlabs.tt.gui.DeploySpinner(viewer, player_interface, race_icons.warriorRubberIcon(), com.oddlabs.tt.gui.ActionButtonPanel.i18n("deploy_chicken_tip"), java.util.List_.of(race_icons.unitStatusIcon(), race_icons.weaponRubberStatusIcon()), com.oddlabs.tt.input.GameAction.RES_CHICKEN, com.oddlabs.tt.input.GameAction.RES_CHICKEN_DEC, null, null);
            army_group.addChild(army_warrior_rubber_button);
            army_back_button = new com.oddlabs.tt.gui.NonFocusIconButton(skin.getBackButton(), com.oddlabs.tt.input.GameAction.GAMEPLAY_BACK, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("back_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.GAMEPLAY_BACK));
            }));
            army_back_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton a0, int a1, int a2, int a3) => { this.cancelSubMenu(a0, a1, a2, a3); }));
            army_group.addChild(army_back_button);
            army_peon_button.place();
            army_warrior_rock_button.place(army_peon_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            army_warrior_iron_button.place(army_warrior_rock_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            army_warrior_rubber_button.place(army_warrior_iron_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            army_back_button.place(army_warrior_rubber_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            army_group.compileCanvas(com.oddlabs.tt.gui.ActionButtonPanel.GROUP_LEFT_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_BOTTOM_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_RIGHT_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_TOP_OFFSET);
            ship_army_peon_button = new com.oddlabs.tt.gui.DeploySpinner(viewer, player_interface, race_icons.peonIcon(), com.oddlabs.tt.gui.ActionButtonPanel.i18n("deploy_peon_tip"), java.util.List_.of(race_icons.unitStatusIcon()), com.oddlabs.tt.input.GameAction.TRAIN_PEON, com.oddlabs.tt.input.GameAction.TRAIN_PEON_DEC, null, null);
            ship_army_group.addChild(ship_army_peon_button);
            ship_army_warrior_rock_button = new com.oddlabs.tt.gui.DeploySpinner(viewer, player_interface, race_icons.warriorRockIcon(), com.oddlabs.tt.gui.ActionButtonPanel.i18n("deploy_rock_tip"), java.util.List_.of(race_icons.unitStatusIcon(), race_icons.weaponRockStatusIcon()), com.oddlabs.tt.input.GameAction.RES_ROCK, com.oddlabs.tt.input.GameAction.RES_ROCK_DEC, null, null);
            ship_army_group.addChild(ship_army_warrior_rock_button);
            ship_army_warrior_iron_button = new com.oddlabs.tt.gui.DeploySpinner(viewer, player_interface, race_icons.warriorIronIcon(), com.oddlabs.tt.gui.ActionButtonPanel.i18n("deploy_iron_tip"), java.util.List_.of(race_icons.unitStatusIcon(), race_icons.weaponIronStatusIcon()), com.oddlabs.tt.input.GameAction.RES_IRON, com.oddlabs.tt.input.GameAction.RES_IRON_DEC, null, null);
            ship_army_group.addChild(ship_army_warrior_iron_button);
            ship_army_warrior_rubber_button = new com.oddlabs.tt.gui.DeploySpinner(viewer, player_interface, race_icons.warriorRubberIcon(), com.oddlabs.tt.gui.ActionButtonPanel.i18n("deploy_chicken_tip"), java.util.List_.of(race_icons.unitStatusIcon(), race_icons.weaponRubberStatusIcon()), com.oddlabs.tt.input.GameAction.RES_CHICKEN, com.oddlabs.tt.input.GameAction.RES_CHICKEN_DEC, null, null);
            ship_army_group.addChild(ship_army_warrior_rubber_button);
            ship_army_chieftain_button = new com.oddlabs.tt.gui.NonFocusIconButton(race_icons.chieftainIcon(), com.oddlabs.tt.input.GameAction.DEPLOY_CHIEFTAIN, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("deploy_chieftain_tip");
            }));
            ship_army_chieftain_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return !current_building.canBuildChieftain();
            }));
            ship_army_chieftain_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton a0, int a1, int a2, int a3) => { this.deployChieftainFromShip(a0, a1, a2, a3); }));
            ship_army_group.addChild(ship_army_chieftain_button);
            ship_army_back_button = new com.oddlabs.tt.gui.NonFocusIconButton(skin.getBackButton(), com.oddlabs.tt.input.GameAction.GAMEPLAY_BACK, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("back_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.GAMEPLAY_BACK));
            }));
            ship_army_back_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton a0, int a1, int a2, int a3) => { this.cancelSubMenu(a0, a1, a2, a3); }));
            ship_army_group.addChild(ship_army_back_button);
            ship_army_peon_button.place();
            ship_army_warrior_rock_button.place(ship_army_peon_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            ship_army_warrior_iron_button.place(ship_army_warrior_rock_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            ship_army_warrior_rubber_button.place(ship_army_warrior_iron_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            ship_army_chieftain_button.place(ship_army_warrior_rubber_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            ship_army_back_button.place(ship_army_chieftain_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            ship_army_group.compileCanvas(com.oddlabs.tt.gui.ActionButtonPanel.GROUP_LEFT_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_BOTTOM_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_RIGHT_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_TOP_OFFSET);
            transport_tree_button = new com.oddlabs.tt.gui.DeploySpinner(viewer, player_interface, icons.getTreeIcon(), com.oddlabs.tt.gui.ActionButtonPanel.i18n("transport_tree_tip"), java.util.List_.of(race_icons.unitStatusIcon(), icons.getTreeStatusIcon()), com.oddlabs.tt.input.GameAction.RES_TREE, com.oddlabs.tt.input.GameAction.RES_TREE_DEC, null, null);
            transport_group.addChild(transport_tree_button);
            transport_rock_button = new com.oddlabs.tt.gui.DeploySpinner(viewer, player_interface, icons.getRockIcon(), com.oddlabs.tt.gui.ActionButtonPanel.i18n("transport_rock_tip"), java.util.List_.of(race_icons.unitStatusIcon(), icons.getRockStatusIcon()), com.oddlabs.tt.input.GameAction.RES_ROCK, com.oddlabs.tt.input.GameAction.RES_ROCK_DEC, null, null);
            transport_group.addChild(transport_rock_button);
            transport_iron_button = new com.oddlabs.tt.gui.DeploySpinner(viewer, player_interface, icons.getIronIcon(), com.oddlabs.tt.gui.ActionButtonPanel.i18n("transport_iron_tip"), java.util.List_.of(race_icons.unitStatusIcon(), icons.getIronStatusIcon()), com.oddlabs.tt.input.GameAction.RES_IRON, com.oddlabs.tt.input.GameAction.RES_IRON_DEC, null, null);
            transport_group.addChild(transport_iron_button);
            transport_rubber_button = new com.oddlabs.tt.gui.DeploySpinner(viewer, player_interface, icons.getRubberIcon(), com.oddlabs.tt.gui.ActionButtonPanel.i18n("transport_chicken_tip"), java.util.List_.of(race_icons.unitStatusIcon(), icons.getRubberStatusIcon()), com.oddlabs.tt.input.GameAction.RES_CHICKEN, com.oddlabs.tt.input.GameAction.RES_CHICKEN_DEC, null, null);
            transport_group.addChild(transport_rubber_button);
            transport_back_button = new com.oddlabs.tt.gui.NonFocusIconButton(skin.getBackButton(), com.oddlabs.tt.input.GameAction.GAMEPLAY_BACK, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("back_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.GAMEPLAY_BACK));
            }));
            transport_back_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton a0, int a1, int a2, int a3) => { this.cancelSubMenu(a0, a1, a2, a3); }));
            transport_group.addChild(transport_back_button);
            transport_tree_button.place();
            transport_rock_button.place(transport_tree_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            transport_iron_button.place(transport_rock_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            transport_rubber_button.place(transport_iron_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            transport_back_button.place(transport_rubber_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            transport_group.compileCanvas(com.oddlabs.tt.gui.ActionButtonPanel.GROUP_LEFT_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_BOTTOM_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_RIGHT_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_TOP_OFFSET);
            setCanFocus(!read_only);
            displayChangedNotify(width, height);
        }
        // mixin of default method Animated.updateChecksum
        public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
        }
    }
}
