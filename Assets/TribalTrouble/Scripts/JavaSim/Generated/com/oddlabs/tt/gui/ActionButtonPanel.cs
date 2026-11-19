// Translated from ActionButtonPanel.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 46
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
        public com.oddlabs.tt.gui.Group transport_group;
        public com.oddlabs.tt.gui.Group research_group;
        public com.oddlabs.tt.gui.Group shrine_group;
        public com.oddlabs.tt.gui.Group palisade_group;
        public bool extended;
        public bool current_shrine;
        public bool current_palisade;
        public com.oddlabs.tt.gui.NonFocusIconButton stand_button;
        public com.oddlabs.tt.gui.NonFocusIconButton palisade_button;
        public com.oddlabs.tt.gui.NonFocusIconButton shrine_button;
        public com.oddlabs.tt.gui.RechargeButton magic4_button;
        public com.oddlabs.tt.gui.RechargeButton magic5_button;
        public com.oddlabs.tt.gui.NonFocusIconButton research_button;
        public com.oddlabs.tt.gui.NonFocusIconButton research_back_button;
        public com.oddlabs.tt.gui.ResearchButton[] research_buttons;
        public static int[] ARMORY_RESEARCH;
        public com.oddlabs.tt.gui.DeploySpinner army_champion_button;
        public com.oddlabs.tt.gui.DeploySpinner army_scout_button;
        public com.oddlabs.tt.gui.DeploySpinner army_shaman_button;
        public com.oddlabs.tt.gui.DeploySpinner army_slinger_button;
        public com.oddlabs.tt.gui.DeploySpinner army_melee_light_button;
        public com.oddlabs.tt.gui.DeploySpinner army_melee_heavy_button;
        public com.oddlabs.tt.gui.DeploySpinner army_spinner_button;
        public com.oddlabs.tt.gui.Group tier_group;
        public com.oddlabs.tt.gui.Group hall_group;
        public bool current_hall;
        public com.oddlabs.tt.gui.Group forge_group;
        public com.oddlabs.tt.gui.Group dock_group;
        public com.oddlabs.tt.gui.Group market_group;
        public com.oddlabs.tt.gui.Group totem_group;
        public bool current_dock;
        public bool current_market;
        public com.oddlabs.tt.gui.MarketSpinner[] market_buttons;
        public com.oddlabs.tt.gui.NonFocusIconButton dock_button;
        public com.oddlabs.tt.gui.NonFocusIconButton bridge_button;
        public com.oddlabs.tt.gui.NonFocusIconButton market_button;
        public bool current_forge;
        public bool current_totem;
        public bool current_tiered;
        public com.oddlabs.tt.gui.TierButton[] tier_buttons;
        public com.oddlabs.tt.gui.NonFocusIconButton hall_button;
        public com.oddlabs.tt.gui.RechargeButton magic6_button;
        public com.oddlabs.tt.gui.DeploySpinner army_unique1_button;
        public com.oddlabs.tt.gui.DeploySpinner army_unique2_button;
        public com.oddlabs.tt.gui.DeploySpinner army_crystal_button;
        public com.oddlabs.tt.gui.DeploySpinner army_diamond_button;
        public com.oddlabs.tt.gui.DeploySpinner army_unique3_button;
        public com.oddlabs.tt.gui.DeploySpinner army_minor_god_button;
        public com.oddlabs.tt.gui.DeploySpinner army_major_god_button;
        public com.oddlabs.tt.gui.GodButton[] god_buttons;
        public com.oddlabs.tt.gui.DeploySpinner[] army_elite_buttons;
        public com.oddlabs.tt.gui.CraftSpinner[] craft_buttons;
        public static com.oddlabs.tt.model.DeployType[] CRAFTS;
        public com.oddlabs.tt.gui.StatusIcon[] arsenal_status;
        public static System.Type[] ARSENAL;
        public com.oddlabs.tt.gui.NonFocusIconButton forge_button;
        public com.oddlabs.tt.gui.NonFocusIconButton totem_button;
        public com.oddlabs.tt.gui.DeploySpinner[] harvest_rare_buttons;
        public com.oddlabs.tt.gui.StatusIcon[] rare_status;
        public static com.oddlabs.tt.input.GameAction[] RARE_INC;
        public static com.oddlabs.tt.input.GameAction[] RARE_DEC;
        public static com.oddlabs.tt.model.DeployType[] RARE_HARVEST;
        public com.oddlabs.tt.gui.NonFocusIconButton tower_attack_button;
        public com.oddlabs.tt.gui.NonFocusIconButton tower_exit_button;
        public com.oddlabs.tt.gui.NonFocusIconButton move_button;
        public com.oddlabs.tt.gui.NonFocusIconButton attack_button;
        public com.oddlabs.tt.gui.NonFocusIconButton guard_button;
        public com.oddlabs.tt.gui.NonFocusIconButton patrol_button;
        public com.oddlabs.tt.gui.NonFocusIconButton gather_repair_button;
        public com.oddlabs.tt.gui.NonFocusIconButton quarters_button;
        public com.oddlabs.tt.gui.RechargeButton magic1_button;
        public com.oddlabs.tt.gui.RechargeButton magic2_button;
        public com.oddlabs.tt.gui.RechargeButton magic3_button;
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
        public com.oddlabs.tt.gui.DeploySpinner transport_tree_button;
        public com.oddlabs.tt.gui.DeploySpinner transport_rock_button;
        public com.oddlabs.tt.gui.DeploySpinner transport_iron_button;
        public com.oddlabs.tt.gui.DeploySpinner transport_rubber_button;
        public com.oddlabs.tt.gui.NonFocusIconButton transport_back_button;
        public static java.util.ResourceBundle bundle;
        // Java: 227
        public static string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.gui.ActionButtonPanel.bundle, key, args);
        }
        // Java: 231
        public static string getBinding(com.oddlabs.tt.input.GameAction action)
        {
            return com.oddlabs.tt.render.Renderer.getLocalInput().getInputManager().getBindingString(action);
        }
        public com.oddlabs.tt.camera.GameCamera camera;
        public com.oddlabs.tt.viewer.WorldViewer viewer;
        public com.oddlabs.tt.gui.Group current_submenu;
        public bool update;
        public bool current_quarters;
        public bool current_armory;
        public bool current_ship;
        public com.oddlabs.tt.model.Building current_building;
        public bool current_unit;
        public bool current_peon;
        public com.oddlabs.tt.model.Unit current_chieftain;
        public bool current_tower;
        public int current_magics;   // (Steam 2.2.207 fix: rebuild when a chieftain magic unlocks)
        // Java: 1048
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
        // Java: 1058
        public override void doRemove()
        {
            base.doRemove();
            viewer.getAnimationManagerLocal().removeAnimation(this);
        }
        // Java: 1064
        public void animate(float t)
        {
            com.oddlabs.tt.model.Building new_building = viewer.getSelection().getCurrentSelection().getBuilding();
            bool different_building = ((object)new_building != (object)current_building);
            current_building = new_building;
            viewer.getRenderer().setSelectedBuilding(new_building);
            com.oddlabs.tt.model.Unit new_chieftain = viewer.getSelection().getCurrentSelection().getChieftain();
            bool different_chieftain = ((object)new_chieftain != (object)current_chieftain);
            current_chieftain = new_chieftain;
            int current_num_units = viewer.getSelection().getCurrentSelection().getNumUnits();
            int current_num_peons = viewer.getSelection().getCurrentSelection().getNumBuilders();
            bool new_quarters = current_building != null && current_building.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.REPRODUCE);
            bool new_armory = current_building != null && current_building.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD_ARMIES);
            bool new_ship = current_building != null && current_building.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.SAIL);
            bool new_unit = current_num_units > 0;
            bool new_peon = current_num_peons > 0;
            bool new_tower = current_building != null && current_building.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.ATTACK);
            int template_id = (current_building != null && ((object)current_building.getRenderLevel() == (object)com.oddlabs.tt.model.Building.BuildState.BUILT) ? ((com.oddlabs.tt.model.BuildingTemplate)current_building.getTemplate()).getTemplateID() : (-1));
            bool new_shrine = extended && template_id == com.oddlabs.tt.model.Race.BUILDING_SHRINE;
            bool new_palisade = extended && template_id == com.oddlabs.tt.model.Race.BUILDING_PALISADE;
            bool new_hall = extended && template_id == com.oddlabs.tt.model.Race.BUILDING_HALL;
            bool new_tiered = extended && template_id >= 0 && ((object)current_building.getOwner() == (object)viewer.getLocalPlayer()) && current_building.getDeployContainer(com.oddlabs.tt.model.DeployType.UPGRADE_STONE) != null;
            int new_magics = 0;
            com.oddlabs.tt.player.Player magic_player = viewer.getLocalPlayer();
            for (int mi = 0; magic_player != null && mi < com.oddlabs.tt.model.RacesResources.NUM_MAGIC; mi++)
            {
                if (magic_player.canDoMagic(mi)) new_magics |= 1 << mi;
            }
            update = update || new_magics != current_magics || different_building || different_chieftain || new_quarters != current_quarters || new_armory != current_armory || new_ship != current_ship || new_unit != current_unit || new_peon != current_peon || new_tower != current_tower || new_shrine != current_shrine || new_palisade != current_palisade || new_hall != current_hall || new_tiered != current_tiered || (extended && (template_id == com.oddlabs.tt.model.Race.BUILDING_FORGE) != current_forge) || (extended && (template_id == com.oddlabs.tt.model.Race.BUILDING_TOTEM) != current_totem) || (extended && (template_id == com.oddlabs.tt.model.Race.BUILDING_DOCK) != current_dock) || (extended && (template_id == com.oddlabs.tt.model.Race.BUILDING_MARKET) != current_market);
            if (update)
            {
                current_magics = new_magics;
                current_quarters = new_quarters;
                current_armory = new_armory;
                current_ship = new_ship;
                current_tower = new_tower;
                current_unit = new_unit;
                current_peon = new_peon;
                current_shrine = new_shrine;
                current_palisade = new_palisade;
                current_hall = new_hall;
                current_forge = extended && template_id == com.oddlabs.tt.model.Race.BUILDING_FORGE;
                current_totem = extended && template_id == com.oddlabs.tt.model.Race.BUILDING_TOTEM;
                current_dock = extended && template_id == com.oddlabs.tt.model.Race.BUILDING_DOCK;
                current_market = extended && template_id == com.oddlabs.tt.model.Race.BUILDING_MARKET && ((object)current_building.getOwner() == (object)viewer.getLocalPlayer());
                current_tiered = new_tiered;
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
                    com.oddlabs.tt.player.Player player = viewer.getLocalPlayer();
                    com.oddlabs.tt.gui.RaceIcons chieftain_icons = (current_chieftain.getMagicRaceOverride() >= 0 ? viewer.getWorld().getRacesResources().getRace(current_chieftain.getMagicRaceOverride()).getIcons() : player.getRace().getIcons());
                    if (player.canDoMagic(0))
                    {
                        magic1_button.setUnit(current_chieftain);
                        magic1_button.setIcon(chieftain_icons.magic1Icon());
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
                        magic2_button.setIcon(chieftain_icons.magic2Icon());
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
                    if (player.canDoMagic(2))
                    {
                        magic3_button.setUnit(current_chieftain);
                        magic3_button.setIcon(chieftain_icons.magic3Icon());
                        magic3_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
                        {
                            return !current_chieftain.canDoMagic(2);
                        }));
                        chieftain_group.addChild(magic3_button);
                    }
                    else
                    {
                        magic3_button.remove();
                    }
                    if (extended)
                    {
                        com.oddlabs.tt.model.Race magic_race = (current_chieftain.getMagicRaceOverride() >= 0 ? viewer.getWorld().getRacesResources().getRace(current_chieftain.getMagicRaceOverride()) : player.getRace());
                        com.oddlabs.tt.gui.RechargeButton[] extra = new com.oddlabs.tt.gui.RechargeButton[] { magic4_button, magic5_button, magic6_button };
                        for (int i = 0; i < extra.Length; i++)
                        {
                            int index = com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_RALLY + i;
                            com.oddlabs.tt.gui.RechargeButton b = extra[i];
                            if (player.canDoMagic(index))
                            {
                                b.setUnit(current_chieftain);
                                b.setIcon(com.oddlabs.tt.gui.ExtendedIcons.getIcons().get(magic_race, "magic" + ((index + 1)) + "_icon"));
                                b.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
                                {
                                    return !current_chieftain.canDoMagic(index);
                                }));
                                chieftain_group.addChild(b);
                            }
                            else
                            {
                                b.remove();
                            }
                        }
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
                    research_buttons[com.oddlabs.tt.model.ResearchContainer.TOWER_RANGE].setBuilding(current_building);
                    research_buttons[com.oddlabs.tt.model.ResearchContainer.TOWER_ARMOR].setBuilding(current_building);
                    research_buttons[com.oddlabs.tt.model.ResearchContainer.VOLLEY].setBuilding(current_building);
                }
                if (current_shrine)
                {
                    addChild(shrine_group);
                    research_buttons[com.oddlabs.tt.model.ResearchContainer.BLESSING].setBuilding(current_building);
                    research_buttons[com.oddlabs.tt.model.ResearchContainer.LORE].setBuilding(current_building);
                    research_buttons[com.oddlabs.tt.model.ResearchContainer.ARCANE_POWER].setBuilding(current_building);
                    research_buttons[com.oddlabs.tt.model.ResearchContainer.COURAGE_MASTERY].setBuilding(current_building);
                }
                if (current_palisade)
                {
                    addChild(palisade_group);
                    research_buttons[com.oddlabs.tt.model.ResearchContainer.WALLS].setBuilding(current_building);
                    research_buttons[com.oddlabs.tt.model.ResearchContainer.STAKES].setBuilding(current_building);
                }
                if (current_hall)
                {
                    addChild(hall_group);
                    research_buttons[com.oddlabs.tt.model.ResearchContainer.RACE_1].setBuilding(current_building);
                    research_buttons[com.oddlabs.tt.model.ResearchContainer.RACE_2].setBuilding(current_building);
                    research_buttons[com.oddlabs.tt.model.ResearchContainer.FOCUS].setBuilding(current_building);
                    research_buttons[com.oddlabs.tt.model.ResearchContainer.PROSPECTING].setBuilding(current_building);
                    research_buttons[com.oddlabs.tt.model.ResearchContainer.ELITE_RANKS].setBuilding(current_building);
                    research_buttons[com.oddlabs.tt.model.ResearchContainer.ELITE_TRAINING].setBuilding(current_building);
                    foreach (com.oddlabs.tt.gui.GodButton g in god_buttons)
                    {
                        g.setBuilding(current_building);
                    }
                }
                if (current_dock)
                {
                    addChild(dock_group);
                    research_buttons[com.oddlabs.tt.model.ResearchContainer.SHIP_OARS].setBuilding(current_building);
                    research_buttons[com.oddlabs.tt.model.ResearchContainer.SHIP_HULL].setBuilding(current_building);
                    research_buttons[com.oddlabs.tt.model.ResearchContainer.FLEET].setBuilding(current_building);
                    research_buttons[com.oddlabs.tt.model.ResearchContainer.CARGO].setBuilding(current_building);
                    research_buttons[com.oddlabs.tt.model.ResearchContainer.BRIDGES].setBuilding(current_building);
                }
                if (current_market)
                {
                    addChild(market_group);
                    foreach (com.oddlabs.tt.gui.MarketSpinner m in market_buttons)
                    {
                        m.setBuilding(current_building);
                    }
                }
                if (current_totem)
                {
                    addChild(totem_group);
                    for (int u = com.oddlabs.tt.model.ResearchContainer.RUNE_REACH; u <= com.oddlabs.tt.model.ResearchContainer.RUNE_WARD; u++)
                    {
                        research_buttons[u].setBuilding(current_building);
                    }
                }
                if (current_forge)
                {
                    addChild(forge_group);
                    for (int u_2 = com.oddlabs.tt.model.ResearchContainer.CRYSTAL_CRAFT; u_2 <= com.oddlabs.tt.model.ResearchContainer.DIAMOND_EDGE; u_2++)
                    {
                        research_buttons[u_2].setBuilding(current_building);
                    }
                }
                if (current_tiered)
                {
                    addChild(tier_group);
                    foreach (com.oddlabs.tt.gui.TierButton b_2 in tier_buttons)
                    {
                        b_2.setBuilding(current_building);
                    }
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
                    research_buttons[com.oddlabs.tt.model.ResearchContainer.GROWTH].setBuilding(current_building);
                    research_buttons[com.oddlabs.tt.model.ResearchContainer.STURDY].setBuilding(current_building);
                    research_buttons[com.oddlabs.tt.model.ResearchContainer.BASKETS].setBuilding(current_building);
                    research_buttons[com.oddlabs.tt.model.ResearchContainer.VIGOR].setBuilding(current_building);
                    research_buttons[com.oddlabs.tt.model.ResearchContainer.BUILDINGS].setBuilding(current_building);
                    research_buttons[com.oddlabs.tt.model.ResearchContainer.GREAT_TRIBE].setBuilding(current_building);
                }
                if (current_armory)
                {
                    addChild(status_group);
                    addChild(armory_group);
                    if (extended)
                    {
                        foreach (com.oddlabs.tt.gui.DeploySpinner b_3 in harvest_rare_buttons)
                        {
                            if (((com.oddlabs.tt.gui.GUIObject)b_3.getParent()) == null)
                            {
                                harvest_group.addChild(b_3);
                            }
                        }
                    }
                    if (viewer.getLocalPlayer().canUseRubber())
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
                    foreach (com.oddlabs.tt.gui.DeploySpinner b_4 in harvest_rare_buttons)
                    {
                        b_4.remove();
                    }
                    if (viewer.getLocalPlayer().canUseRubber())
                    {
                        army_group.addChild(army_warrior_rubber_button);
                    }
                    else
                    {
                        army_warrior_rubber_button.remove();
                    }
                    updateCounters();
                }
                updateGroups();
            }
            updateButtons();
        }
        // Java: 1292
        public void updateButtons()
        {
            if (current_building != null && current_building.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.ATTACK))
            {
                tower_attack_button.doUpdate();
                tower_exit_button.doUpdate();
                research_buttons[com.oddlabs.tt.model.ResearchContainer.TOWER_RANGE].doUpdate();
                research_buttons[com.oddlabs.tt.model.ResearchContainer.TOWER_ARMOR].doUpdate();
                research_buttons[com.oddlabs.tt.model.ResearchContainer.VOLLEY].doUpdate();
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
                if (extended)
                {
                    research_button.doUpdate();
                    foreach (int u in com.oddlabs.tt.gui.ActionButtonPanel.ARMORY_RESEARCH)
                    {
                        research_buttons[u].doUpdate();
                    }
                    army_champion_button.doUpdate();
                    army_scout_button.doUpdate();
                    army_shaman_button.doUpdate();
                    army_slinger_button.doUpdate();
                    army_melee_light_button.doUpdate();
                    army_melee_heavy_button.doUpdate();
                    army_unique1_button.doUpdate();
                    army_unique2_button.doUpdate();
                    army_unique3_button.doUpdate();
                    army_crystal_button.doUpdate();
                    army_diamond_button.doUpdate();
                    foreach (com.oddlabs.tt.gui.DeploySpinner e in army_elite_buttons)
                    {
                        e.doUpdate();
                    }
                    army_spinner_button.doUpdate();
                    foreach (com.oddlabs.tt.gui.CraftSpinner c in craft_buttons)
                    {
                        c.doUpdate();
                    }
                    foreach (com.oddlabs.tt.gui.StatusIcon a in arsenal_status)
                    {
                        a.doUpdate();
                    }
                    for (int i = 0; i < rare_status.Length; i++)
                    {
                        rare_status[i].doUpdate();
                        harvest_rare_buttons[i].doUpdate();
                    }
                }
            }
            else if (current_building != null && current_building.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.REPRODUCE))
            {
                quarters_unit_status.doUpdate();
                quarters_peon_button.doUpdate();
                quarters_chieftain_button.doUpdate();
                research_buttons[com.oddlabs.tt.model.ResearchContainer.GROWTH].doUpdate();
                research_buttons[com.oddlabs.tt.model.ResearchContainer.STURDY].doUpdate();
                research_buttons[com.oddlabs.tt.model.ResearchContainer.BASKETS].doUpdate();
                research_buttons[com.oddlabs.tt.model.ResearchContainer.VIGOR].doUpdate();
                research_buttons[com.oddlabs.tt.model.ResearchContainer.BUILDINGS].doUpdate();
                research_buttons[com.oddlabs.tt.model.ResearchContainer.GREAT_TRIBE].doUpdate();
            }
            else if (current_dock)
            {
                research_buttons[com.oddlabs.tt.model.ResearchContainer.SHIP_OARS].doUpdate();
                research_buttons[com.oddlabs.tt.model.ResearchContainer.SHIP_HULL].doUpdate();
                research_buttons[com.oddlabs.tt.model.ResearchContainer.FLEET].doUpdate();
                research_buttons[com.oddlabs.tt.model.ResearchContainer.CARGO].doUpdate();
                research_buttons[com.oddlabs.tt.model.ResearchContainer.BRIDGES].doUpdate();
            }
            else if (current_market)
            {
                foreach (com.oddlabs.tt.gui.MarketSpinner m in market_buttons)
                {
                    m.doUpdate();
                }
            }
            else if (current_forge)
            {
                for (int u_2 = com.oddlabs.tt.model.ResearchContainer.CRYSTAL_CRAFT; u_2 <= com.oddlabs.tt.model.ResearchContainer.DIAMOND_EDGE; u_2++)
                {
                    research_buttons[u_2].doUpdate();
                }
            }
            else if (current_totem)
            {
                for (int u_3 = com.oddlabs.tt.model.ResearchContainer.RUNE_REACH; u_3 <= com.oddlabs.tt.model.ResearchContainer.RUNE_WARD; u_3++)
                {
                    research_buttons[u_3].doUpdate();
                }
            }
            else if (current_shrine || current_palisade || current_hall)
            {
                research_buttons[com.oddlabs.tt.model.ResearchContainer.BLESSING].doUpdate();
                research_buttons[com.oddlabs.tt.model.ResearchContainer.LORE].doUpdate();
                research_buttons[com.oddlabs.tt.model.ResearchContainer.ARCANE_POWER].doUpdate();
                research_buttons[com.oddlabs.tt.model.ResearchContainer.COURAGE_MASTERY].doUpdate();
                research_buttons[com.oddlabs.tt.model.ResearchContainer.WALLS].doUpdate();
                research_buttons[com.oddlabs.tt.model.ResearchContainer.RACE_1].doUpdate();
                research_buttons[com.oddlabs.tt.model.ResearchContainer.RACE_2].doUpdate();
                research_buttons[com.oddlabs.tt.model.ResearchContainer.STAKES].doUpdate();
                research_buttons[com.oddlabs.tt.model.ResearchContainer.FOCUS].doUpdate();
                research_buttons[com.oddlabs.tt.model.ResearchContainer.PROSPECTING].doUpdate();
                research_buttons[com.oddlabs.tt.model.ResearchContainer.ELITE_RANKS].doUpdate();
                research_buttons[com.oddlabs.tt.model.ResearchContainer.ELITE_TRAINING].doUpdate();
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
                army_warrior_rubber_button.doUpdate();
                army_warrior_iron_button.doUpdate();
                army_warrior_rock_button.doUpdate();
                army_peon_button.doUpdate();
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
                if (com.oddlabs.tt.global.Globals.SHIPS_ENABLED)
                {
                    ship_button.doUpdate();
                }
                palisade_button.doUpdate();
                shrine_button.doUpdate();
                hall_button.doUpdate();
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
                magic3_button.doUpdate();
                if (extended)
                {
                    magic4_button.doUpdate();
                    magic5_button.doUpdate();
                    magic6_button.doUpdate();
                }
            }
        }
        // Java: 1449
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
            transport_group.remove();
            research_group.remove();
            shrine_group.remove();
            palisade_group.remove();
            hall_group.remove();
            forge_group.remove();
            totem_group.remove();
            dock_group.remove();
            market_group.remove();
            tier_group.remove();
            current_submenu = null;
        }
        // Java: 1475
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
            if (extended && current_building.getDeployContainer(com.oddlabs.tt.model.DeployType.PEON_HARVEST_GOLD) != null)
            {
                for (int i = 0; i < rare_status.Length; i++)
                {
                    rare_status[i].setCounter(new com.oddlabs.tt.model.SupplyCounter(current_building, com.oddlabs.tt.model.ExtendedResources.RARE[i]));
                    harvest_rare_buttons[i].setContainers(current_building, com.oddlabs.tt.gui.ActionButtonPanel.RARE_HARVEST[i], null);
                    harvest_rare_buttons[i].setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
                    {
                        return unit_counter.getNumSupplies() == 0;
                    }));
                }
            }
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
            if (extended && current_building.getDeployContainer(com.oddlabs.tt.model.DeployType.CHAMPION) != null)
            {
                foreach (int u in com.oddlabs.tt.gui.ActionButtonPanel.ARMORY_RESEARCH)
                {
                    research_buttons[u].setBuilding(current_building);
                }
                army_champion_button.setContainers(current_building, com.oddlabs.tt.model.DeployType.CHAMPION, typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon));
                army_champion_button.setIconDisabler(com.oddlabs.tt.gui.ActionButtonPanel.lacks(unit_counter, weapon_iron_counter, com.oddlabs.tt.gui.ActionButtonPanel.weaponsPer(current_building, com.oddlabs.tt.model.DeployType.CHAMPION)));
                army_scout_button.setContainers(current_building, com.oddlabs.tt.model.DeployType.SCOUT, typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon));
                army_scout_button.setIconDisabler(com.oddlabs.tt.gui.ActionButtonPanel.lacks(unit_counter, weapon_rock_counter, com.oddlabs.tt.gui.ActionButtonPanel.weaponsPer(current_building, com.oddlabs.tt.model.DeployType.SCOUT)));
                army_shaman_button.setContainers(current_building, com.oddlabs.tt.model.DeployType.SHAMAN, typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon));
                army_shaman_button.setIconDisabler(com.oddlabs.tt.gui.ActionButtonPanel.lacks(unit_counter, weapon_rock_counter, com.oddlabs.tt.gui.ActionButtonPanel.weaponsPer(current_building, com.oddlabs.tt.model.DeployType.SHAMAN)));
                army_slinger_button.setContainers(current_building, com.oddlabs.tt.model.DeployType.SLINGER, typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon));
                army_slinger_button.setIconDisabler(com.oddlabs.tt.gui.ActionButtonPanel.lacks(unit_counter, weapon_rock_counter, com.oddlabs.tt.gui.ActionButtonPanel.weaponsPer(current_building, com.oddlabs.tt.model.DeployType.SLINGER)));
                if ((current_building.getDeployContainer(com.oddlabs.tt.model.DeployType.UNIQUE_1) is com.oddlabs.tt.model.RareDeployContainer u1) && (current_building.getDeployContainer(com.oddlabs.tt.model.DeployType.UNIQUE_2) is com.oddlabs.tt.model.RareDeployContainer u2))
                {
                    com.oddlabs.tt.model.SupplyCounter darts_counter = new com.oddlabs.tt.model.SupplyCounter(current_building, typeof(com.oddlabs.tt.model.weapon.DartWeapon));
                    army_unique1_button.setContainers(current_building, com.oddlabs.tt.model.DeployType.UNIQUE_1, typeof(com.oddlabs.tt.model.weapon.DartWeapon));
                    army_unique1_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
                    {
                        return darts_counter.getNumSupplies() < u1.getWeaponsPerUnit() || suppliesEmpty(unit_counter) || !u1.hallReady() || u1.affordable() == 0;
                    }));
                    army_unique2_button.setContainers(current_building, com.oddlabs.tt.model.DeployType.UNIQUE_2, typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon));
                    army_unique2_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
                    {
                        return weapon_iron_counter.getNumSupplies() < u2.getWeaponsPerUnit() || suppliesEmpty(unit_counter) || !u2.hallReady() || u2.affordable() == 0;
                    }));
                }
                if (current_building.getDeployContainer(com.oddlabs.tt.model.DeployType.CRYSTAL_WARRIOR) != null)
                {
                    com.oddlabs.tt.model.SupplyCounter[] arsenal = new com.oddlabs.tt.model.SupplyCounter[com.oddlabs.tt.gui.ActionButtonPanel.ARSENAL.Length];
                    for (int i_2 = 0; i_2 < com.oddlabs.tt.gui.ActionButtonPanel.ARSENAL.Length; i_2++)
                    {
                        arsenal[i_2] = new com.oddlabs.tt.model.SupplyCounter(current_building, com.oddlabs.tt.gui.ActionButtonPanel.ARSENAL[i_2]);
                        arsenal_status[i_2].setCounter(arsenal[i_2]);
                    }
                    foreach (com.oddlabs.tt.gui.CraftSpinner c in craft_buttons)
                    {
                        c.setBuilding(current_building);
                    }
                    army_crystal_button.setContainers(current_building, com.oddlabs.tt.model.DeployType.CRYSTAL_WARRIOR, com.oddlabs.tt.gui.ActionButtonPanel.ARSENAL[0]);
                    army_crystal_button.setIconDisabler(com.oddlabs.tt.gui.ActionButtonPanel.lacks(unit_counter, arsenal[0], com.oddlabs.tt.gui.ActionButtonPanel.weaponsPer(current_building, com.oddlabs.tt.model.DeployType.CRYSTAL_WARRIOR)));
                    if ((current_building.getDeployContainer(com.oddlabs.tt.model.DeployType.DIAMOND_GUARD) is com.oddlabs.tt.model.RareDeployContainer d) && (current_building.getDeployContainer(com.oddlabs.tt.model.DeployType.UNIQUE_3) is com.oddlabs.tt.model.RareDeployContainer u3))
                    {
                        army_diamond_button.setContainers(current_building, com.oddlabs.tt.model.DeployType.DIAMOND_GUARD, com.oddlabs.tt.gui.ActionButtonPanel.ARSENAL[1]);
                        army_diamond_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
                        {
                            return arsenal[1].getNumSupplies() < d.getWeaponsPerUnit() || suppliesEmpty(unit_counter) || !d.hallReady();
                        }));
                        bool vik = com.oddlabs.tt.model.ExtendedCombat.isVikings(current_building.getOwner());
                        System.Type u3_weapon = (vik ? typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon) : com.oddlabs.tt.gui.ActionButtonPanel.ARSENAL[2]);
                        com.oddlabs.tt.model.SupplyCounter u3_counter = (vik ? weapon_iron_counter : arsenal[2]);
                        army_unique3_button.setContainers(current_building, com.oddlabs.tt.model.DeployType.UNIQUE_3, u3_weapon);
                        army_unique3_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
                        {
                            return u3_counter.getNumSupplies() < u3.getWeaponsPerUnit() || suppliesEmpty(unit_counter) || !u3.hallReady() || u3.affordable() == 0;
                        }));
                        {
                            com.oddlabs.tt.model.DeployType[] elite_types = new com.oddlabs.tt.model.DeployType[] { com.oddlabs.tt.model.DeployType.ELITE_1, com.oddlabs.tt.model.DeployType.ELITE_2, com.oddlabs.tt.model.DeployType.ELITE_3 };
                            for (int k = 0; k < elite_types.Length; k++)
                            {
                                if (!((current_building.getDeployContainer(elite_types[k]) is com.oddlabs.tt.model.RareDeployContainer e)))
                                {
                                    continue;
                                }
                                System.Type w = (k == 2 ? typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon) : typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon));
                                com.oddlabs.tt.model.SupplyCounter wc = (k == 2 ? weapon_rock_counter : weapon_iron_counter);
                                army_elite_buttons[k].setContainers(current_building, elite_types[k], w);
                                army_elite_buttons[k].setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
                                {
                                    return wc.getNumSupplies() < e.getWeaponsPerUnit() || suppliesEmpty(unit_counter) || !e.hallReady() || e.affordable() == 0 || e.room() == 0;
                                }));
                            }
                        }
                    }
                }
                if (current_building.getDeployContainer(com.oddlabs.tt.model.DeployType.MELEE_LIGHT) != null)
                {
                    army_melee_light_button.setContainers(current_building, com.oddlabs.tt.model.DeployType.MELEE_LIGHT, typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon));
                    army_melee_light_button.setIconDisabler(com.oddlabs.tt.gui.ActionButtonPanel.lacks(unit_counter, weapon_rock_counter, com.oddlabs.tt.gui.ActionButtonPanel.weaponsPer(current_building, com.oddlabs.tt.model.DeployType.MELEE_LIGHT)));
                    army_melee_heavy_button.setContainers(current_building, com.oddlabs.tt.model.DeployType.MELEE_HEAVY, typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon));
                    army_melee_heavy_button.setIconDisabler(com.oddlabs.tt.gui.ActionButtonPanel.lacks(unit_counter, weapon_iron_counter, com.oddlabs.tt.gui.ActionButtonPanel.weaponsPer(current_building, com.oddlabs.tt.model.DeployType.MELEE_HEAVY)));
                }
                if (current_building.getDeployContainer(com.oddlabs.tt.model.DeployType.SPINNER) != null)
                {
                    army_spinner_button.setContainers(current_building, com.oddlabs.tt.model.DeployType.SPINNER, typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon));
                    army_spinner_button.setIconDisabler(com.oddlabs.tt.gui.ActionButtonPanel.lacks(unit_counter, weapon_rubber_counter, com.oddlabs.tt.gui.ActionButtonPanel.weaponsPer(current_building, com.oddlabs.tt.model.DeployType.SPINNER)));
                }
            }
        }
        public static org.joml.Vector4fc PANEL_BODY;
        public static org.joml.Vector4fc PANEL_EDGE;
        public static org.joml.Vector4fc PANEL_ACCENT;
        // Java: 1605
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            if (!com.oddlabs.tt.gui.UiStyle.modern || !com.oddlabs.tt.gui.UiStyle.panels)
            {
                return;
            }
            for (com.oddlabs.tt.gui.Renderable c = ((com.oddlabs.tt.gui.GUIObject)getFirstChild()); c != null; c = ((com.oddlabs.tt.gui.Renderable)((com.oddlabs.util.ListElementImpl)c.getNext())))
            {
                if (!((c is com.oddlabs.tt.gui.Group g)) || g.getWidth() <= 0 || g.getHeight() <= 0)
                {
                    continue;
                }
                float x = g.getX() - 5;
                float y = g.getY() - 5;
                float w = g.getWidth() + 10;
                float h = g.getHeight() + 10;
                renderer.drawColoredQuad(x, y, w, h, com.oddlabs.tt.gui.ActionButtonPanel.PANEL_BODY);
                renderer.drawColoredQuad(x, y, w, 1, com.oddlabs.tt.gui.ActionButtonPanel.PANEL_EDGE);
                renderer.drawColoredQuad(x, y, 1, h, com.oddlabs.tt.gui.ActionButtonPanel.PANEL_EDGE);
                renderer.drawColoredQuad((float)((float)(x + w) - 1), y, 1, h, com.oddlabs.tt.gui.ActionButtonPanel.PANEL_EDGE);
                renderer.drawColoredQuad(x, (float)((float)(y + h) - 2), w, 2, com.oddlabs.tt.gui.ActionButtonPanel.PANEL_ACCENT);
            }
        }
        // Java: 1621
        public override void displayChangedNotify(int width, int height)
        {
            setDim(width, height);
            updateGroups();
        }
        // Java: 1627
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
            transport_group.setPos(width - transport_group.getWidth(), status_group.getY() - transport_group.getHeight());
            research_group.setPos(width - research_group.getWidth(), status_group.getY() - research_group.getHeight());
            shrine_group.setPos(width - shrine_group.getWidth(), height - shrine_group.getHeight());
            palisade_group.setPos(width - palisade_group.getWidth(), height - palisade_group.getHeight());
            hall_group.setPos(width - hall_group.getWidth(), height - hall_group.getHeight());
            forge_group.setPos(width - forge_group.getWidth(), height - forge_group.getHeight());
            totem_group.setPos(width - totem_group.getWidth(), height - totem_group.getHeight());
            dock_group.setPos(width - dock_group.getWidth(), height - dock_group.getHeight());
            market_group.setPos(width - market_group.getWidth(), height - market_group.getHeight());
            int left = width;
            foreach (com.oddlabs.tt.gui.Group g in new com.oddlabs.tt.gui.Group[] { tower_group, quarters_group, quarters_status_group, status_group, armory_group, shrine_group, palisade_group, hall_group, forge_group, totem_group, dock_group, market_group, harvest_group, build_group, army_group, transport_group, research_group })
            {
                if (((object)((com.oddlabs.tt.gui.GUIObject)g.getParent()) == (object)this))
                {
                    left = java.lang.Math.min(left, g.getX());
                }
            }
            tier_group.setPos(left - tier_group.getWidth() - 4, height - tier_group.getHeight());
        }
        // Java: 1664
        public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
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
                    else if (current_armory && extended && canSwitchSubmenu(@event) && @event.consumeAction(com.oddlabs.tt.input.GameAction.PROD_RESEARCH))
                    {
                        activate(@event, research_button);
                    }
                    else if (current_unit && current_peon && extended && @event.consumeAction(com.oddlabs.tt.input.GameAction.UNIT_BUILD_PALISADE))
                    {
                        activate(@event, palisade_button);
                    }
                    else if (current_unit && current_peon && extended && @event.consumeAction(com.oddlabs.tt.input.GameAction.UNIT_BUILD_SHRINE))
                    {
                        activate(@event, shrine_button);
                    }
                    else if (current_chieftain != null && extended && @event.consumeAction(com.oddlabs.tt.input.GameAction.MAGIC_4))
                    {
                        if (viewer.getLocalPlayer().canDoMagic(com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_RALLY))
                        {
                            activate(@event, magic4_button);
                        }
                    }
                    else if (current_chieftain != null && extended && @event.consumeAction(com.oddlabs.tt.input.GameAction.MAGIC_5))
                    {
                        if (viewer.getLocalPlayer().canDoMagic(com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_RALLY + 1))
                        {
                            activate(@event, magic5_button);
                        }
                    }
                    else if (current_chieftain != null && extended && @event.consumeAction(com.oddlabs.tt.input.GameAction.MAGIC_6))
                    {
                        if (viewer.getLocalPlayer().canDoMagic(com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_ELEMENT))
                        {
                            activate(@event, magic6_button);
                        }
                    }
                    else if (current_unit && current_peon && extended && @event.consumeAction(com.oddlabs.tt.input.GameAction.UNIT_BUILD_HALL))
                    {
                        activate(@event, hall_button);
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
                        if (com.oddlabs.tt.global.Globals.SHIPS_ENABLED)
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
                    else if (current_unit && @event.consumeAction(com.oddlabs.tt.input.GameAction.UNIT_GUARD))
                    {
                        activate(@event, guard_button);
                    }
                    else if (current_unit && @event.consumeAction(com.oddlabs.tt.input.GameAction.UNIT_PATROL))
                    {
                        activate(@event, patrol_button);
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
                    else if (current_chieftain != null && @event.consumeAction(com.oddlabs.tt.input.GameAction.MAGIC_2))
                    {
                        if (viewer.getLocalPlayer().canDoMagic(1))
                        {
                            activate(@event, magic2_button);
                        }
                    }
                    else if (current_chieftain != null && @event.consumeAction(com.oddlabs.tt.input.GameAction.MAGIC_3))
                    {
                        if (viewer.getLocalPlayer().canDoMagic(2))
                        {
                            activate(@event, magic3_button);
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
                        else if (((object)current_submenu == (object)transport_group))
                        {
                            activate(@event, transport_back_button);
                        }
                        else if (((object)current_submenu == (object)research_group))
                        {
                            activate(@event, research_back_button);
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
                        if (viewer.getLocalPlayer().canDoMagic(0))
                        {
                            activate(@event, magic1_button);
                        }
                    }
                    if (@event.isConsumed())
                    {
                        return;
                    }
                }
                if (extendedArmyShortcut(@event, true) || extendedHarvestShortcut(@event, true))
                {
                    return;
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
                        else if ((current_armory || current_ship) && ((object)current_submenu == (object)army_group))
                        {
                            army_peon_button.shortcutPressed(peon.decrement(), peon.batch());
                            @event.getActions().clear();
                        }
                    }
                    com.oddlabs.tt.gui.ActionButtonPanel.ResourceAction chicken = checkResourceAction(@event, com.oddlabs.tt.input.GameAction.RES_CHICKEN, com.oddlabs.tt.input.GameAction.RES_CHICKEN_DEC, com.oddlabs.tt.input.GameAction.RES_CHICKEN_BATCH, com.oddlabs.tt.input.GameAction.RES_CHICKEN_BATCH_DEC);
                    if (chicken.active())
                    {
                        handleArmoryShortcut(true, chicken, harvest_rubber_button, build_weapon_rubber_button, army_warrior_rubber_button, transport_rubber_button);
                        @event.getActions().clear();
                    }
                    com.oddlabs.tt.gui.ActionButtonPanel.ResourceAction iron = checkResourceAction(@event, com.oddlabs.tt.input.GameAction.RES_IRON, com.oddlabs.tt.input.GameAction.RES_IRON_DEC, com.oddlabs.tt.input.GameAction.RES_IRON_BATCH, com.oddlabs.tt.input.GameAction.RES_IRON_BATCH_DEC);
                    if (iron.active())
                    {
                        handleArmoryShortcut(true, iron, harvest_iron_button, build_weapon_iron_button, army_warrior_iron_button, transport_iron_button);
                        @event.getActions().clear();
                    }
                    com.oddlabs.tt.gui.ActionButtonPanel.ResourceAction tree = checkResourceAction(@event, com.oddlabs.tt.input.GameAction.RES_TREE, com.oddlabs.tt.input.GameAction.RES_TREE_DEC, com.oddlabs.tt.input.GameAction.RES_TREE_BATCH, com.oddlabs.tt.input.GameAction.RES_TREE_BATCH_DEC);
                    if (tree.active())
                    {
                        handleArmoryShortcut(true, tree, harvest_tree_button, null, null, transport_tree_button);
                        @event.getActions().clear();
                    }
                    com.oddlabs.tt.gui.ActionButtonPanel.ResourceAction rock = checkResourceAction(@event, com.oddlabs.tt.input.GameAction.RES_ROCK, com.oddlabs.tt.input.GameAction.RES_ROCK_DEC, com.oddlabs.tt.input.GameAction.RES_ROCK_BATCH, com.oddlabs.tt.input.GameAction.RES_ROCK_BATCH_DEC);
                    if (rock.active())
                    {
                        handleArmoryShortcut(true, rock, harvest_rock_button, build_weapon_rock_button, army_warrior_rock_button, transport_rock_button);
                        @event.getActions().clear();
                    }
                }
            }
            else if (released)
            {
                if (extendedArmyShortcut(@event, false) || extendedHarvestShortcut(@event, false))
                {
                    return;
                }
                com.oddlabs.tt.gui.ActionButtonPanel.ResourceAction chicken_2 = checkResourceAction(@event, com.oddlabs.tt.input.GameAction.RES_CHICKEN, com.oddlabs.tt.input.GameAction.RES_CHICKEN_DEC, com.oddlabs.tt.input.GameAction.RES_CHICKEN_BATCH, com.oddlabs.tt.input.GameAction.RES_CHICKEN_BATCH_DEC);
                if (chicken_2.active())
                {
                    handleArmoryShortcut(false, chicken_2, harvest_rubber_button, build_weapon_rubber_button, army_warrior_rubber_button, transport_rubber_button);
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
                        else if ((current_armory || current_ship) && ((object)current_submenu == (object)army_group))
                        {
                            army_peon_button.shortcutReleased(peon_2.decrement(), peon_2.batch());
                            @event.getActions().clear();
                        }
                    }
                    else
                    {
                        com.oddlabs.tt.gui.ActionButtonPanel.ResourceAction iron_2 = checkResourceAction(@event, com.oddlabs.tt.input.GameAction.RES_IRON, com.oddlabs.tt.input.GameAction.RES_IRON_DEC, com.oddlabs.tt.input.GameAction.RES_IRON_BATCH, com.oddlabs.tt.input.GameAction.RES_IRON_BATCH_DEC);
                        if (iron_2.active())
                        {
                            handleArmoryShortcut(false, iron_2, harvest_iron_button, build_weapon_iron_button, army_warrior_iron_button, transport_iron_button);
                        }
                        else
                        {
                            com.oddlabs.tt.gui.ActionButtonPanel.ResourceAction rock_2 = checkResourceAction(@event, com.oddlabs.tt.input.GameAction.RES_ROCK, com.oddlabs.tt.input.GameAction.RES_ROCK_DEC, com.oddlabs.tt.input.GameAction.RES_ROCK_BATCH, com.oddlabs.tt.input.GameAction.RES_ROCK_BATCH_DEC);
                            if (rock_2.active())
                            {
                                handleArmoryShortcut(false, rock_2, harvest_rock_button, build_weapon_rock_button, army_warrior_rock_button, transport_rock_button);
                            }
                            else
                            {
                                com.oddlabs.tt.gui.ActionButtonPanel.ResourceAction tree_2 = checkResourceAction(@event, com.oddlabs.tt.input.GameAction.RES_TREE, com.oddlabs.tt.input.GameAction.RES_TREE_DEC, com.oddlabs.tt.input.GameAction.RES_TREE_BATCH, com.oddlabs.tt.input.GameAction.RES_TREE_BATCH_DEC);
                                if (tree_2.active())
                                {
                                    handleArmoryShortcut(false, tree_2, harvest_tree_button, null, null, transport_tree_button);
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
        // Java: 1920
        public bool extendedHarvestShortcut(com.oddlabs.tt.input.InputEvent @event, bool pressed)
        {
            if (!extended || !current_armory || ((object)current_submenu != (object)harvest_group))
            {
                return false;
            }
            for (int i = 0; i < harvest_rare_buttons.Length; i++)
            {
                bool up = @event.consumeAction(com.oddlabs.tt.gui.ActionButtonPanel.RARE_INC[i]);
                bool down = !up && @event.consumeAction(com.oddlabs.tt.gui.ActionButtonPanel.RARE_DEC[i]);
                if (up || down)
                {
                    if (pressed)
                    {
                        harvest_rare_buttons[i].shortcutPressed(down, false);
                    }
                    else
                    {
                        harvest_rare_buttons[i].shortcutReleased(down, false);
                    }
                    @event.getActions().clear();
                    return true;
                }
            }
            return false;
        }
        // Java: 1939
        public bool extendedArmyShortcut(com.oddlabs.tt.input.InputEvent @event, bool pressed)
        {
            if (!extended || !current_armory || ((object)current_submenu != (object)army_group))
            {
                return false;
            }
            com.oddlabs.tt.input.GameAction[] inc = new com.oddlabs.tt.input.GameAction[] { com.oddlabs.tt.input.GameAction.TRAIN_CHAMPION, com.oddlabs.tt.input.GameAction.TRAIN_SCOUT, com.oddlabs.tt.input.GameAction.TRAIN_SHAMAN, com.oddlabs.tt.input.GameAction.TRAIN_SLINGER, com.oddlabs.tt.input.GameAction.TRAIN_MELEE_LIGHT, com.oddlabs.tt.input.GameAction.TRAIN_MELEE_HEAVY, com.oddlabs.tt.input.GameAction.TRAIN_UNIQUE_1, com.oddlabs.tt.input.GameAction.TRAIN_UNIQUE_2, com.oddlabs.tt.input.GameAction.TRAIN_SPINNER };
            com.oddlabs.tt.input.GameAction[] dec = new com.oddlabs.tt.input.GameAction[] { com.oddlabs.tt.input.GameAction.TRAIN_CHAMPION_DEC, com.oddlabs.tt.input.GameAction.TRAIN_SCOUT_DEC, com.oddlabs.tt.input.GameAction.TRAIN_SHAMAN_DEC, com.oddlabs.tt.input.GameAction.TRAIN_SLINGER_DEC, com.oddlabs.tt.input.GameAction.TRAIN_MELEE_LIGHT_DEC, com.oddlabs.tt.input.GameAction.TRAIN_MELEE_HEAVY_DEC, com.oddlabs.tt.input.GameAction.TRAIN_UNIQUE_1_DEC, com.oddlabs.tt.input.GameAction.TRAIN_UNIQUE_2_DEC, com.oddlabs.tt.input.GameAction.TRAIN_SPINNER_DEC };
            com.oddlabs.tt.gui.DeploySpinner[] spinners = new com.oddlabs.tt.gui.DeploySpinner[] { army_champion_button, army_scout_button, army_shaman_button, army_slinger_button, army_melee_light_button, army_melee_heavy_button, army_unique1_button, army_unique2_button, army_spinner_button };
            for (int i = 0; i < spinners.Length; i++)
            {
                bool up = @event.consumeAction(inc[i]);
                bool down = !up && @event.consumeAction(dec[i]);
                if (up || down)
                {
                    if (pressed)
                    {
                        spinners[i].shortcutPressed(down, false);
                    }
                    else
                    {
                        spinners[i].shortcutReleased(down, false);
                    }
                    @event.getActions().clear();
                    return true;
                }
            }
            return false;
        }
        // Java: 1967
        public void activate(com.oddlabs.tt.input.InputEvent @event, com.oddlabs.tt.gui.GUIObject button)
        {
            button.mouseClickedAll(com.oddlabs.tt.gui.MouseButton.LEFT, 0, 0, 1);
            @event.consume();
        }
        // Java: 1972
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
        // Java: 1975
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
        // Java: 1984
        public void handleArmoryShortcut(bool pressed, com.oddlabs.tt.gui.ActionButtonPanel.ResourceAction action, com.oddlabs.tt.gui.IconSpinner harvestBtn, com.oddlabs.tt.gui.IconSpinner buildBtn, com.oddlabs.tt.gui.IconSpinner armyBtn, com.oddlabs.tt.gui.IconSpinner transportBtn)
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
        // Java: 2003
        public override bool canHoverBehind()
        {
            return true;
        }
        // Java: 2008
        public bool inHarvestMenu()
        {
            return ((object)current_submenu == (object)harvest_group);
        }
        // Java: 2012
        public bool inBuildMenu()
        {
            return ((object)current_submenu == (object)build_group);
        }
        // Java: 2016
        public bool inArmyMenu()
        {
            return ((object)current_submenu == (object)army_group);
        }
        // Java: 2020
        public bool inTransportMenu()
        {
            return ((object)current_submenu == (object)transport_group);
        }
        // Java: 2025
        public override void mouseDragged(com.oddlabs.tt.gui.MouseButton button, int x, int y, int relative_x, int relative_y, int absolute_x, int absolute_y)
        {
            if (((com.oddlabs.tt.gui.GUIObject)getParent()) != null)
            {
                ((com.oddlabs.tt.gui.GUIObject)getParent()).mouseDragged(button, x, y, relative_x, relative_y, absolute_x, absolute_y);
            }
        }
        // Java: 2032
        public void setRallyPoint(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
        {
            if (current_building != null && !current_building.isDead())
            {
                pushDelegate(new com.oddlabs.tt.@delegate.RallyPointDelegate(viewer, camera, current_building));
            }
            removeGroups();
            update = true;
        }
        // Java: 2040
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
        // Java: 2050
        public void cancelSubMenu(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
        {
            removeGroups();
            update = true;
        }
        // Java: 2055
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
            updateGroups();
        }
        // Java: 2070
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
        // Java: 2085
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
        // Java: 2096
        public bool suppliesEmpty(params com.oddlabs.tt.model.SupplyCounter[] counters)
        {
            return java.util.Arrays.stream(counters).anyMatch(new java.util.function.Predicate_L((object c_l) =>
            {
                com.oddlabs.tt.model.SupplyCounter c = (com.oddlabs.tt.model.SupplyCounter)c_l;
                return c.getNumSupplies() == 0;
            }));
        }
        // Java: 2101
        public static int weaponsPer(com.oddlabs.tt.model.Building building, com.oddlabs.tt.model.DeployType type)
        {
            com.oddlabs.tt.model.DeployContainer c = building.getDeployContainer(type);
            if ((c is com.oddlabs.tt.model.MultiWeaponDeployContainer m))
            {
                return m.getWeaponsPerUnit();
            }
            if ((c is com.oddlabs.tt.model.RareDeployContainer r))
            {
                return r.getWeaponsPerUnit();
            }
            return 1;
        }
        // Java: 2111
        public static com.oddlabs.tt.gui.IconDisabler lacks(com.oddlabs.tt.model.SupplyCounter units, com.oddlabs.tt.model.SupplyCounter weapons, int per)
        {
            int n = java.lang.Math.max(1, per);
            return new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return units.getNumSupplies() == 0 || weapons.getNumSupplies() < n;
            });
        }
        static ActionButtonPanel()
        {
            ActionButtonPanel.ARMORY_SUBMENU_ACTIONS = java.util.EnumSet.of(com.oddlabs.tt.input.GameAction.RES_TREE, com.oddlabs.tt.input.GameAction.RES_TREE_DEC, com.oddlabs.tt.input.GameAction.RES_TREE_BATCH, com.oddlabs.tt.input.GameAction.RES_TREE_BATCH_DEC, com.oddlabs.tt.input.GameAction.RES_ROCK, com.oddlabs.tt.input.GameAction.RES_ROCK_DEC, com.oddlabs.tt.input.GameAction.RES_ROCK_BATCH, com.oddlabs.tt.input.GameAction.RES_ROCK_BATCH_DEC, com.oddlabs.tt.input.GameAction.RES_IRON, com.oddlabs.tt.input.GameAction.RES_IRON_DEC, com.oddlabs.tt.input.GameAction.RES_IRON_BATCH, com.oddlabs.tt.input.GameAction.RES_IRON_BATCH_DEC, com.oddlabs.tt.input.GameAction.RES_CHICKEN, com.oddlabs.tt.input.GameAction.RES_CHICKEN_DEC, com.oddlabs.tt.input.GameAction.RES_CHICKEN_BATCH, com.oddlabs.tt.input.GameAction.RES_CHICKEN_BATCH_DEC, com.oddlabs.tt.input.GameAction.TRAIN_PEON, com.oddlabs.tt.input.GameAction.TRAIN_PEON_DEC, com.oddlabs.tt.input.GameAction.TRAIN_PEON_BATCH, com.oddlabs.tt.input.GameAction.TRAIN_PEON_BATCH_DEC, com.oddlabs.tt.input.GameAction.GAMEPLAY_BACK, com.oddlabs.tt.input.GameAction.TRAIN_CHAMPION, com.oddlabs.tt.input.GameAction.TRAIN_CHAMPION_DEC, com.oddlabs.tt.input.GameAction.TRAIN_SCOUT, com.oddlabs.tt.input.GameAction.TRAIN_SCOUT_DEC, com.oddlabs.tt.input.GameAction.TRAIN_SHAMAN, com.oddlabs.tt.input.GameAction.TRAIN_SHAMAN_DEC, com.oddlabs.tt.input.GameAction.TRAIN_SLINGER, com.oddlabs.tt.input.GameAction.TRAIN_SLINGER_DEC, com.oddlabs.tt.input.GameAction.TRAIN_MELEE_LIGHT, com.oddlabs.tt.input.GameAction.TRAIN_MELEE_LIGHT_DEC, com.oddlabs.tt.input.GameAction.TRAIN_MELEE_HEAVY, com.oddlabs.tt.input.GameAction.TRAIN_MELEE_HEAVY_DEC, com.oddlabs.tt.input.GameAction.TRAIN_UNIQUE_1, com.oddlabs.tt.input.GameAction.TRAIN_UNIQUE_1_DEC, com.oddlabs.tt.input.GameAction.TRAIN_UNIQUE_2, com.oddlabs.tt.input.GameAction.TRAIN_UNIQUE_2_DEC, com.oddlabs.tt.input.GameAction.RES_GOLD, com.oddlabs.tt.input.GameAction.RES_GOLD_DEC, com.oddlabs.tt.input.GameAction.RES_CRYSTAL, com.oddlabs.tt.input.GameAction.RES_CRYSTAL_DEC, com.oddlabs.tt.input.GameAction.RES_HERBS, com.oddlabs.tt.input.GameAction.RES_HERBS_DEC, com.oddlabs.tt.input.GameAction.RES_COAL, com.oddlabs.tt.input.GameAction.RES_COAL_DEC, com.oddlabs.tt.input.GameAction.TRAIN_CRYSTAL, com.oddlabs.tt.input.GameAction.TRAIN_CRYSTAL_DEC, com.oddlabs.tt.input.GameAction.TRAIN_DIAMOND, com.oddlabs.tt.input.GameAction.TRAIN_DIAMOND_DEC, com.oddlabs.tt.input.GameAction.TRAIN_UNIQUE_3, com.oddlabs.tt.input.GameAction.TRAIN_UNIQUE_3_DEC, com.oddlabs.tt.input.GameAction.CRAFT_CRYSTAL, com.oddlabs.tt.input.GameAction.CRAFT_CRYSTAL_DEC, com.oddlabs.tt.input.GameAction.CRAFT_DIAMOND, com.oddlabs.tt.input.GameAction.CRAFT_DIAMOND_DEC, com.oddlabs.tt.input.GameAction.CRAFT_DARTS, com.oddlabs.tt.input.GameAction.CRAFT_DARTS_DEC, com.oddlabs.tt.input.GameAction.TRAIN_MINOR_GOD, com.oddlabs.tt.input.GameAction.TRAIN_MINOR_GOD_DEC, com.oddlabs.tt.input.GameAction.TRAIN_MAJOR_GOD, com.oddlabs.tt.input.GameAction.TRAIN_MAJOR_GOD_DEC, com.oddlabs.tt.input.GameAction.TRAIN_ELITE_1, com.oddlabs.tt.input.GameAction.TRAIN_ELITE_1_DEC, com.oddlabs.tt.input.GameAction.TRAIN_ELITE_2, com.oddlabs.tt.input.GameAction.TRAIN_ELITE_2_DEC, com.oddlabs.tt.input.GameAction.TRAIN_ELITE_3, com.oddlabs.tt.input.GameAction.TRAIN_ELITE_3_DEC);
            ActionButtonPanel.ARMORY_RESEARCH = new int[] { com.oddlabs.tt.model.ResearchContainer.ARMOR, com.oddlabs.tt.model.ResearchContainer.SPEED, com.oddlabs.tt.model.ResearchContainer.TOOLS, com.oddlabs.tt.model.ResearchContainer.RANGE, com.oddlabs.tt.model.ResearchContainer.SMITHING, com.oddlabs.tt.model.ResearchContainer.DRILL };
            ActionButtonPanel.CRAFTS = new com.oddlabs.tt.model.DeployType[] { com.oddlabs.tt.model.DeployType.CRAFT_CRYSTAL, com.oddlabs.tt.model.DeployType.CRAFT_DIAMOND, com.oddlabs.tt.model.DeployType.CRAFT_DARTS };
            ActionButtonPanel.ARSENAL = new System.Type[] { typeof(com.oddlabs.tt.model.weapon.CrystalWeapon), typeof(com.oddlabs.tt.model.weapon.DiamondWeapon), typeof(com.oddlabs.tt.model.weapon.DartWeapon) };
            ActionButtonPanel.RARE_INC = new com.oddlabs.tt.input.GameAction[] { com.oddlabs.tt.input.GameAction.RES_GOLD, com.oddlabs.tt.input.GameAction.RES_CRYSTAL, com.oddlabs.tt.input.GameAction.RES_HERBS, com.oddlabs.tt.input.GameAction.RES_COAL, com.oddlabs.tt.input.GameAction.RES_DIAMOND };
            ActionButtonPanel.RARE_DEC = new com.oddlabs.tt.input.GameAction[] { com.oddlabs.tt.input.GameAction.RES_GOLD_DEC, com.oddlabs.tt.input.GameAction.RES_CRYSTAL_DEC, com.oddlabs.tt.input.GameAction.RES_HERBS_DEC, com.oddlabs.tt.input.GameAction.RES_COAL_DEC, com.oddlabs.tt.input.GameAction.RES_DIAMOND_DEC };
            ActionButtonPanel.RARE_HARVEST = new com.oddlabs.tt.model.DeployType[] { com.oddlabs.tt.model.DeployType.PEON_HARVEST_GOLD, com.oddlabs.tt.model.DeployType.PEON_HARVEST_CRYSTAL, com.oddlabs.tt.model.DeployType.PEON_HARVEST_HERBS, com.oddlabs.tt.model.DeployType.PEON_HARVEST_COAL, com.oddlabs.tt.model.DeployType.PEON_HARVEST_DIAMOND };
            ActionButtonPanel.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.gui.ActionButtonPanel).getName());
            ActionButtonPanel.PANEL_BODY = new org.joml.Vector4f(0.05f, 0.07f, 0.1f, 0.6f);
            ActionButtonPanel.PANEL_EDGE = new org.joml.Vector4f(0.42f, 0.5f, 0.6f, 0.55f);
            ActionButtonPanel.PANEL_ACCENT = new org.joml.Vector4f(0.9f, 0.74f, 0.42f, 0.9f);
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
            this.transport_group = new com.oddlabs.tt.gui.NonFocusGroup();
            this.research_group = new com.oddlabs.tt.gui.NonFocusGroup();
            this.shrine_group = new com.oddlabs.tt.gui.NonFocusGroup();
            this.palisade_group = new com.oddlabs.tt.gui.NonFocusGroup();
            this.current_shrine = false;
            this.current_palisade = false;
            this.research_buttons = new com.oddlabs.tt.gui.ResearchButton[com.oddlabs.tt.model.ResearchContainer.NUM_UPGRADES];
            this.tier_group = new com.oddlabs.tt.gui.NonFocusGroup();
            this.hall_group = new com.oddlabs.tt.gui.NonFocusGroup();
            this.current_hall = false;
            this.forge_group = new com.oddlabs.tt.gui.NonFocusGroup();
            this.dock_group = new com.oddlabs.tt.gui.NonFocusGroup();
            this.market_group = new com.oddlabs.tt.gui.NonFocusGroup();
            this.totem_group = new com.oddlabs.tt.gui.NonFocusGroup();
            this.current_dock = false;
            this.current_market = false;
            this.market_buttons = new com.oddlabs.tt.gui.MarketSpinner[11];
            this.current_forge = false;
            this.current_totem = false;
            this.current_tiered = false;
            this.tier_buttons = new com.oddlabs.tt.gui.TierButton[4];
            this.god_buttons = new com.oddlabs.tt.gui.GodButton[2];
            this.army_elite_buttons = new com.oddlabs.tt.gui.DeploySpinner[3];
            this.craft_buttons = new com.oddlabs.tt.gui.CraftSpinner[3];
            this.arsenal_status = new com.oddlabs.tt.gui.StatusIcon[3];
            this.harvest_rare_buttons = new com.oddlabs.tt.gui.DeploySpinner[5];
            this.rare_status = new com.oddlabs.tt.gui.StatusIcon[5];
            this.current_submenu = null;
            this.update = false;
            this.current_quarters = false;
            this.current_armory = false;
            this.current_ship = false;
            this.current_unit = false;
            this.current_peon = false;
            this.current_tower = false;
        }
        public ActionButtonPanel(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.camera.GameCamera camera) : this(viewer, camera, viewer.getGUIRoot().getWidth(), viewer.getGUIRoot().getHeight())
        {
        }
        public ActionButtonPanel(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.camera.GameCamera camera, int width, int height) : base()
        {
            init_ActionButtonPanel();
            this.viewer = viewer;
            this.camera = camera;
            com.oddlabs.tt.gui.RaceIcons race_icons = viewer.getLocalPlayer().getRace().getIcons();
            this.extended = viewer.getWorld().isExtended();
            com.oddlabs.tt.gui.ExtendedIcons extended_icons = com.oddlabs.tt.gui.ExtendedIcons.getIcons();
            com.oddlabs.tt.model.Race race = viewer.getLocalPlayer().getRace();
            bool vikings = ((object)race.getIcons() == (object)com.oddlabs.tt.gui.GUIIcons.getIcons().getVikingIcons());
            com.oddlabs.tt.player.PlayerInterface player_interface = viewer.getPeerHub().getPlayerInterface();
            com.oddlabs.tt.gui.Skin skin = com.oddlabs.tt.gui.Skin.getSkin();
            com.oddlabs.tt.gui.GUIIcons icons = com.oddlabs.tt.gui.GUIIcons.getIcons();
            string widest_char = java.lang.JString.create(java.lang.Character.toChars(skin.getEditFont().getWidestCodepoint("0123456789")));
            int label_width = skin.getEditFont().getWidth(widest_char.repeat(5));
            move_button = new com.oddlabs.tt.gui.NonFocusIconButton(race_icons.moveIcon(), com.oddlabs.tt.input.GameAction.UNIT_MOVE, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("move_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.UNIT_MOVE));
            }));
            move_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return !viewer.getLocalPlayer().canMove();
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
                return !viewer.getLocalPlayer().canAttack();
            }));
            unit_group.addChild(attack_button);
            attack_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_5, int unused_6, int unused_7, int unused_8) =>
            {
                pushDelegate(new com.oddlabs.tt.@delegate.TargetDelegate(viewer, camera, com.oddlabs.tt.model.Action.ATTACK));
            }));
            guard_button = new com.oddlabs.tt.gui.NonFocusIconButton(extended_icons.get(race, "guard_icon"), com.oddlabs.tt.input.GameAction.UNIT_GUARD, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("guard_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.UNIT_GUARD));
            }));
            guard_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return !viewer.getLocalPlayer().canAttack();
            }));
            unit_group.addChild(guard_button);
            guard_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_9, int unused_10, int unused_11, int unused_12) =>
            {
                pushDelegate(new com.oddlabs.tt.@delegate.TargetDelegate(viewer, camera, com.oddlabs.tt.model.Action.GUARD));
            }));
            patrol_button = new com.oddlabs.tt.gui.NonFocusIconButton(extended_icons.get(race, "patrol_icon"), com.oddlabs.tt.input.GameAction.UNIT_PATROL, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("patrol_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.UNIT_PATROL));
            }));
            patrol_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return !viewer.getLocalPlayer().canAttack();
            }));
            unit_group.addChild(patrol_button);
            patrol_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_13, int unused_14, int unused_15, int unused_16) =>
            {
                pushDelegate(new com.oddlabs.tt.@delegate.TargetDelegate(viewer, camera, com.oddlabs.tt.model.Action.PATROL));
            }));
            stand_button = new com.oddlabs.tt.gui.NonFocusIconButton(extended_icons.get(race, "stand_icon"), com.oddlabs.tt.input.GameAction.STAND_GROUND, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("stand_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.STAND_GROUND));
            }));
            unit_group.addChild(stand_button);
            stand_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_17, int unused_18, int unused_19, int unused_20) =>
            {
                viewer.getPeerHub().getPlayerInterface().standGround(((com.oddlabs.tt.model.Selectable[])viewer.getSelection().getCurrentSelection().getSet().toArray(new com.oddlabs.tt.model.Selectable[0])));
            }));
            move_button.place();
            attack_button.place(move_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            guard_button.place(attack_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            patrol_button.place(guard_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            stand_button.place(patrol_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            unit_group.compileCanvas(com.oddlabs.tt.gui.ActionButtonPanel.GROUP_LEFT_OFFSET, 0, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_RIGHT_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_BOTTOM_OFFSET);
            gather_repair_button = new com.oddlabs.tt.gui.NonFocusIconButton(race_icons.gatherRepairIcon(), com.oddlabs.tt.input.GameAction.UNIT_GATHER, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("gather_repair_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.UNIT_GATHER));
            }));
            peon_group.addChild(gather_repair_button);
            gather_repair_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_21, int unused_22, int unused_23, int unused_24) =>
            {
                pushDelegate(new com.oddlabs.tt.@delegate.TargetDelegate(viewer, camera, com.oddlabs.tt.model.Action.GATHER_REPAIR));
            }));
            gather_repair_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return !viewer.getLocalPlayer().canRepair();
            }));
            quarters_button = new com.oddlabs.tt.gui.NonFocusIconButton(race_icons.quartersIcon(), com.oddlabs.tt.input.GameAction.UNIT_BUILD_QUARTERS, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("quarters_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.UNIT_BUILD_QUARTERS));
            }));
            peon_group.addChild(quarters_button);
            quarters_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_25, int unused_26, int unused_27, int unused_28) =>
            {
                pushDelegate(new com.oddlabs.tt.@delegate.PlacingDelegate(viewer, camera.getState(), com.oddlabs.tt.model.Race.BUILDING_QUARTERS));
            }));
            quarters_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return !viewer.getLocalPlayer().canBuild(com.oddlabs.tt.model.Race.BUILDING_QUARTERS);
            }));
            armory_button = new com.oddlabs.tt.gui.NonFocusIconButton(race_icons.armoryIcon(), com.oddlabs.tt.input.GameAction.UNIT_BUILD_ARMORY, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("armory_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.UNIT_BUILD_ARMORY));
            }));
            peon_group.addChild(armory_button);
            armory_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_29, int unused_30, int unused_31, int unused_32) =>
            {
                pushDelegate(new com.oddlabs.tt.@delegate.PlacingDelegate(viewer, camera.getState(), com.oddlabs.tt.model.Race.BUILDING_ARMORY));
            }));
            armory_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return !viewer.getLocalPlayer().canBuild(com.oddlabs.tt.model.Race.BUILDING_ARMORY);
            }));
            tower_button = new com.oddlabs.tt.gui.NonFocusIconButton(race_icons.towerIcon(), com.oddlabs.tt.input.GameAction.UNIT_BUILD_TOWER, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("tower_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.UNIT_BUILD_TOWER));
            }));
            peon_group.addChild(tower_button);
            tower_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_33, int unused_34, int unused_35, int unused_36) =>
            {
                pushDelegate(new com.oddlabs.tt.@delegate.PlacingDelegate(viewer, camera.getState(), com.oddlabs.tt.model.Race.BUILDING_TOWER));
            }));
            tower_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return !viewer.getLocalPlayer().canBuild(com.oddlabs.tt.model.Race.BUILDING_TOWER);
            }));
            ship_button = new com.oddlabs.tt.gui.NonFocusIconButton(race_icons.shipIcon(), com.oddlabs.tt.input.GameAction.UNIT_BUILD_SHIP, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("ship_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.UNIT_BUILD_SHIP));
            }));
            if (com.oddlabs.tt.global.Globals.SHIPS_ENABLED)
            {
                peon_group.addChild(ship_button);
                ship_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_37, int unused_38, int unused_39, int unused_40) =>
                {
                    pushDelegate(new com.oddlabs.tt.@delegate.PlacingDelegate(viewer, camera.getState(), com.oddlabs.tt.model.Race.BUILDING_SHIP));
                }));
            }
            palisade_button = new com.oddlabs.tt.gui.NonFocusIconButton(extended_icons.get(race, "palisade_icon"), com.oddlabs.tt.input.GameAction.UNIT_BUILD_PALISADE, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("palisade_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.UNIT_BUILD_PALISADE));
            }));
            palisade_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_41, int unused_42, int unused_43, int unused_44) =>
            {
                pushDelegate(new com.oddlabs.tt.@delegate.PlacingDelegate(viewer, camera.getState(), com.oddlabs.tt.model.Race.BUILDING_PALISADE));
            }));
            palisade_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return !viewer.getLocalPlayer().canBuild(com.oddlabs.tt.model.Race.BUILDING_PALISADE);
            }));
            palisade_button.setHider(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return viewer.getLocalPlayer().isLocked(com.oddlabs.tt.model.Unlocks.building(com.oddlabs.tt.model.Race.BUILDING_PALISADE));
            }));
            shrine_button = new com.oddlabs.tt.gui.NonFocusIconButton(extended_icons.get(race, "shrine_icon"), com.oddlabs.tt.input.GameAction.UNIT_BUILD_SHRINE, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("shrine_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.UNIT_BUILD_SHRINE));
            }));
            shrine_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_45, int unused_46, int unused_47, int unused_48) =>
            {
                pushDelegate(new com.oddlabs.tt.@delegate.PlacingDelegate(viewer, camera.getState(), com.oddlabs.tt.model.Race.BUILDING_SHRINE));
            }));
            shrine_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return !viewer.getLocalPlayer().canBuild(com.oddlabs.tt.model.Race.BUILDING_SHRINE);
            }));
            shrine_button.setHider(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return viewer.getLocalPlayer().isLocked(com.oddlabs.tt.model.Unlocks.building(com.oddlabs.tt.model.Race.BUILDING_SHRINE));
            }));
            hall_button = new com.oddlabs.tt.gui.NonFocusIconButton(extended_icons.get(race, "hall_icon"), com.oddlabs.tt.input.GameAction.UNIT_BUILD_HALL, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n((vikings ? "mead_hall_tip" : "spirit_lodge_tip"), com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.UNIT_BUILD_HALL));
            }));
            hall_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_49, int unused_50, int unused_51, int unused_52) =>
            {
                pushDelegate(new com.oddlabs.tt.@delegate.PlacingDelegate(viewer, camera.getState(), com.oddlabs.tt.model.Race.BUILDING_HALL));
            }));
            hall_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return !viewer.getLocalPlayer().canBuild(com.oddlabs.tt.model.Race.BUILDING_HALL);
            }));
            hall_button.setHider(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return viewer.getLocalPlayer().isLocked(com.oddlabs.tt.model.Unlocks.building(com.oddlabs.tt.model.Race.BUILDING_HALL));
            }));
            forge_button = new com.oddlabs.tt.gui.NonFocusIconButton(extended_icons.get(race, "forge_icon"), com.oddlabs.tt.input.GameAction.UNIT_BUILD_FORGE, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n((vikings ? "rune_forge_tip" : "crystal_forge_tip"), com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.UNIT_BUILD_FORGE));
            }));
            forge_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_53, int unused_54, int unused_55, int unused_56) =>
            {
                pushDelegate(new com.oddlabs.tt.@delegate.PlacingDelegate(viewer, camera.getState(), com.oddlabs.tt.model.Race.BUILDING_FORGE));
            }));
            forge_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return !viewer.getLocalPlayer().canBuild(com.oddlabs.tt.model.Race.BUILDING_FORGE);
            }));
            forge_button.setHider(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return viewer.getLocalPlayer().isLocked(com.oddlabs.tt.model.Unlocks.building(com.oddlabs.tt.model.Race.BUILDING_FORGE));
            }));
            totem_button = new com.oddlabs.tt.gui.NonFocusIconButton(extended_icons.get(race, "totem_icon"), com.oddlabs.tt.input.GameAction.UNIT_BUILD_TOTEM, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n((vikings ? "rune_stone_tip" : "war_totem_tip"), com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.UNIT_BUILD_TOTEM));
            }));
            totem_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_57, int unused_58, int unused_59, int unused_60) =>
            {
                pushDelegate(new com.oddlabs.tt.@delegate.PlacingDelegate(viewer, camera.getState(), com.oddlabs.tt.model.Race.BUILDING_TOTEM));
            }));
            totem_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return !viewer.getLocalPlayer().canBuild(com.oddlabs.tt.model.Race.BUILDING_TOTEM);
            }));
            totem_button.setHider(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return viewer.getLocalPlayer().isLocked(com.oddlabs.tt.model.Unlocks.building(com.oddlabs.tt.model.Race.BUILDING_TOTEM));
            }));
            dock_button = new com.oddlabs.tt.gui.NonFocusIconButton(extended_icons.get(race, "dock_icon"), com.oddlabs.tt.input.GameAction.UNIT_BUILD_DOCK, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("dock_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.UNIT_BUILD_DOCK));
            }));
            dock_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_61, int unused_62, int unused_63, int unused_64) =>
            {
                pushDelegate(new com.oddlabs.tt.@delegate.PlacingDelegate(viewer, camera.getState(), com.oddlabs.tt.model.Race.BUILDING_DOCK));
            }));
            dock_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return !viewer.getLocalPlayer().canBuild(com.oddlabs.tt.model.Race.BUILDING_DOCK);
            }));
            dock_button.setHider(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return viewer.getLocalPlayer().isLocked(com.oddlabs.tt.model.Unlocks.building(com.oddlabs.tt.model.Race.BUILDING_DOCK));
            }));
            market_button = new com.oddlabs.tt.gui.NonFocusIconButton(extended_icons.get(race, "market_icon"), com.oddlabs.tt.input.GameAction.UNIT_BUILD_MARKET, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("market_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.UNIT_BUILD_MARKET));
            }));
            market_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_65, int unused_66, int unused_67, int unused_68) =>
            {
                pushDelegate(new com.oddlabs.tt.@delegate.PlacingDelegate(viewer, camera.getState(), com.oddlabs.tt.model.Race.BUILDING_MARKET));
            }));
            market_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return !viewer.getLocalPlayer().canBuild(com.oddlabs.tt.model.Race.BUILDING_MARKET);
            }));
            market_button.setHider(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return viewer.getLocalPlayer().isLocked(com.oddlabs.tt.model.Unlocks.building(com.oddlabs.tt.model.Race.BUILDING_MARKET));
            }));
            bridge_button = new com.oddlabs.tt.gui.NonFocusIconButton(extended_icons.get(race, "bridge_icon"), com.oddlabs.tt.input.GameAction.UNIT_BUILD_BRIDGE, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("bridge_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.UNIT_BUILD_BRIDGE));
            }));
            bridge_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_69, int unused_70, int unused_71, int unused_72) =>
            {
                pushDelegate(new com.oddlabs.tt.@delegate.PlacingDelegate(viewer, camera.getState(), com.oddlabs.tt.model.Race.BUILDING_BRIDGE));
            }));
            bridge_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return !viewer.getLocalPlayer().canBuild(com.oddlabs.tt.model.Race.BUILDING_BRIDGE);
            }));
            bridge_button.setHider(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return viewer.getLocalPlayer().isLocked(com.oddlabs.tt.model.Unlocks.building(com.oddlabs.tt.model.Race.BUILDING_BRIDGE)) || viewer.getLocalPlayer().getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.BRIDGES) == 0;
            }));
            gather_repair_button.place();
            quarters_button.place(gather_repair_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            armory_button.place(quarters_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            tower_button.place(armory_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            if (com.oddlabs.tt.global.Globals.SHIPS_ENABLED)
            {
                ship_button.place(tower_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            }
            if (extended)
            {
                peon_group.addChild(palisade_button);
                peon_group.addChild(shrine_button);
                palisade_button.place((com.oddlabs.tt.global.Globals.SHIPS_ENABLED ? (com.oddlabs.tt.gui.GUIObject)(ship_button) : (com.oddlabs.tt.gui.GUIObject)(tower_button)), com.oddlabs.tt.gui.Placement.BOTTOM_MID);
                shrine_button.place(palisade_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
                peon_group.addChild(hall_button);
                hall_button.place(shrine_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
                peon_group.addChild(forge_button);
                peon_group.addChild(totem_button);
                forge_button.place(palisade_button, com.oddlabs.tt.gui.Placement.LEFT_MID, 5);
                totem_button.place(forge_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
                peon_group.addChild(dock_button);
                peon_group.addChild(market_button);
                dock_button.place(totem_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
                market_button.place(dock_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
                peon_group.addChild(bridge_button);
                bridge_button.place(market_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            }
            peon_group.compileCanvas(com.oddlabs.tt.gui.ActionButtonPanel.GROUP_LEFT_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_BOTTOM_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_RIGHT_OFFSET, 0);
            magic1_button = new com.oddlabs.tt.gui.RechargeButton(player_interface, race_icons.magic1Icon(), com.oddlabs.tt.input.GameAction.MAGIC_1, race_icons.magic1Desc(), 0);
            chieftain_group.addChild(magic1_button);
            magic2_button = new com.oddlabs.tt.gui.RechargeButton(player_interface, race_icons.magic2Icon(), com.oddlabs.tt.input.GameAction.MAGIC_2, race_icons.magic2Desc(), 1);
            chieftain_group.addChild(magic2_button);
            magic3_button = new com.oddlabs.tt.gui.RechargeButton(player_interface, race_icons.magic3Icon(), com.oddlabs.tt.input.GameAction.MAGIC_3, race_icons.magic3Desc(), 2);
            chieftain_group.addChild(magic3_button);
            magic4_button = new com.oddlabs.tt.gui.RechargeButton(player_interface, extended_icons.get(race, "magic4_icon"), com.oddlabs.tt.input.GameAction.MAGIC_4, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n((vikings ? "magic4_viking_tip" : "magic4_native_tip"), com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.MAGIC_4));
            }), com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_RALLY);
            magic5_button = new com.oddlabs.tt.gui.RechargeButton(player_interface, extended_icons.get(race, "magic5_icon"), com.oddlabs.tt.input.GameAction.MAGIC_5, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n((vikings ? "magic5_viking_tip" : "magic5_native_tip"), com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.MAGIC_5));
            }), com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_RALLY + 1);
            magic6_button = new com.oddlabs.tt.gui.RechargeButton(player_interface, extended_icons.get(race, "magic6_icon"), com.oddlabs.tt.input.GameAction.MAGIC_6, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n((vikings ? "magic6_viking_tip" : "magic6_native_tip"), com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.MAGIC_6));
            }), com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_ELEMENT);
            magic1_button.place();
            magic2_button.place(magic1_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            magic3_button.place(magic2_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            if (extended)
            {
                chieftain_group.addChild(magic4_button);
                chieftain_group.addChild(magic5_button);
                magic4_button.place(magic3_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
                magic5_button.place(magic4_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
                chieftain_group.addChild(magic6_button);
                magic6_button.place(magic5_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            }
            chieftain_group.compileCanvas(com.oddlabs.tt.gui.ActionButtonPanel.GROUP_LEFT_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_BOTTOM_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_RIGHT_OFFSET, 0);
            tower_attack_button = new com.oddlabs.tt.gui.NonFocusIconButton(race_icons.attackIcon(), com.oddlabs.tt.input.GameAction.UNIT_ATTACK, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("attack_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.UNIT_ATTACK));
            }));
            tower_group.addChild(tower_attack_button);
            tower_attack_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_73, int unused_74, int unused_75, int unused_76) =>
            {
                pushDelegate(new com.oddlabs.tt.@delegate.TargetDelegate(viewer, camera, com.oddlabs.tt.model.Action.ATTACK));
            }));
            tower_exit_button = new com.oddlabs.tt.gui.NonFocusIconButton(race_icons.towerExitIcon(), com.oddlabs.tt.input.GameAction.UNIT_EXIT_TOWER, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("exit_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.UNIT_EXIT_TOWER));
            }));
            tower_group.addChild(tower_exit_button);
            tower_exit_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_77, int unused_78, int unused_79, int unused_80) =>
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
            com.oddlabs.tt.model.DeployType[] research_types = new com.oddlabs.tt.model.DeployType[] { com.oddlabs.tt.model.DeployType.RESEARCH_ARMOR, com.oddlabs.tt.model.DeployType.RESEARCH_SPEED, com.oddlabs.tt.model.DeployType.RESEARCH_TOOLS, com.oddlabs.tt.model.DeployType.RESEARCH_RANGE, com.oddlabs.tt.model.DeployType.RESEARCH_GROWTH, com.oddlabs.tt.model.DeployType.RESEARCH_TOWER_RANGE, com.oddlabs.tt.model.DeployType.RESEARCH_TOWER_ARMOR, com.oddlabs.tt.model.DeployType.RESEARCH_BLESSING, com.oddlabs.tt.model.DeployType.RESEARCH_LORE, com.oddlabs.tt.model.DeployType.RESEARCH_WALLS, com.oddlabs.tt.model.DeployType.RESEARCH_RACE_1, com.oddlabs.tt.model.DeployType.RESEARCH_RACE_2, com.oddlabs.tt.model.DeployType.RESEARCH_SMITHING, com.oddlabs.tt.model.DeployType.RESEARCH_DRILL, com.oddlabs.tt.model.DeployType.RESEARCH_STURDY, com.oddlabs.tt.model.DeployType.RESEARCH_BASKETS, com.oddlabs.tt.model.DeployType.RESEARCH_VOLLEY, com.oddlabs.tt.model.DeployType.RESEARCH_STAKES, com.oddlabs.tt.model.DeployType.RESEARCH_FOCUS, com.oddlabs.tt.model.DeployType.RESEARCH_PROSPECTING, com.oddlabs.tt.model.DeployType.RESEARCH_VIGOR, com.oddlabs.tt.model.DeployType.RESEARCH_CRYSTAL_CRAFT, com.oddlabs.tt.model.DeployType.RESEARCH_DIAMOND_CRAFT, com.oddlabs.tt.model.DeployType.RESEARCH_ROCK_EDGE, com.oddlabs.tt.model.DeployType.RESEARCH_IRON_TEMPER, com.oddlabs.tt.model.DeployType.RESEARCH_CHICKEN_BOUNCE, com.oddlabs.tt.model.DeployType.RESEARCH_CRYSTAL_FACET, com.oddlabs.tt.model.DeployType.RESEARCH_DIAMOND_EDGE, com.oddlabs.tt.model.DeployType.RESEARCH_SHIP_OARS, com.oddlabs.tt.model.DeployType.RESEARCH_SHIP_HULL, com.oddlabs.tt.model.DeployType.RESEARCH_ARCANE_POWER, com.oddlabs.tt.model.DeployType.RESEARCH_COURAGE_MASTERY, com.oddlabs.tt.model.DeployType.RESEARCH_FLEET, com.oddlabs.tt.model.DeployType.RESEARCH_CARGO, com.oddlabs.tt.model.DeployType.RESEARCH_BUILDINGS, com.oddlabs.tt.model.DeployType.RESEARCH_ELITE_RANKS, com.oddlabs.tt.model.DeployType.RESEARCH_ELITE_TRAINING, com.oddlabs.tt.model.DeployType.RESEARCH_GREAT_TRIBE, com.oddlabs.tt.model.DeployType.RESEARCH_BRIDGES, com.oddlabs.tt.model.DeployType.RESEARCH_RUNE_REACH, com.oddlabs.tt.model.DeployType.RESEARCH_RUNE_MIGHT, com.oddlabs.tt.model.DeployType.RESEARCH_RUNE_WARD };
            string[] research_icons = new string[] { "research_armor_icon", "research_speed_icon", "research_tools_icon", "research_range_icon", "research_growth_icon", "research_tower_range_icon", "research_tower_armor_icon", "research_blessing_icon", "research_lore_icon", "research_walls_icon", "research_race1_icon", "research_race2_icon", "research_smithing_icon", "research_drill_icon", "research_sturdy_icon", "research_baskets_icon", "research_volley_icon", "research_stakes_icon", "research_focus_icon", "research_prospecting_icon", "research_vigor_icon", "research_crystal_craft_icon", "research_diamond_craft_icon", "research_rock_edge_icon", "research_iron_temper_icon", "research_chicken_bounce_icon", "research_crystal_facet_icon", "research_diamond_edge_icon", "research_ship_oars_icon", "research_ship_hull_icon", "research_arcane_icon", "research_courage_icon", "research_fleet_icon", "research_cargo_icon", "research_buildings_icon", "research_elite_ranks_icon", "research_elite_training_icon", "research_great_tribe_icon", "research_bridges_icon", "research_rune_reach_icon", "research_rune_might_icon", "research_rune_ward_icon" };
            for (int u = 0; u < com.oddlabs.tt.model.ResearchContainer.NUM_UPGRADES; u++)
            {
                research_buttons[u] = new com.oddlabs.tt.gui.ResearchButton(player_interface, viewer.getLocalPlayer(), extended_icons.get(race, research_icons[u]), research_types[u], u);
            }
            if (extended)
            {
                tower_group.addChild(research_buttons[com.oddlabs.tt.model.ResearchContainer.TOWER_RANGE]);
                tower_group.addChild(research_buttons[com.oddlabs.tt.model.ResearchContainer.TOWER_ARMOR]);
                research_buttons[com.oddlabs.tt.model.ResearchContainer.TOWER_RANGE].place(tower_exit_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
                research_buttons[com.oddlabs.tt.model.ResearchContainer.TOWER_ARMOR].place(research_buttons[com.oddlabs.tt.model.ResearchContainer.TOWER_RANGE], com.oddlabs.tt.gui.Placement.BOTTOM_MID);
                tower_group.addChild(research_buttons[com.oddlabs.tt.model.ResearchContainer.VOLLEY]);
                research_buttons[com.oddlabs.tt.model.ResearchContainer.VOLLEY].place(research_buttons[com.oddlabs.tt.model.ResearchContainer.TOWER_ARMOR], com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            }
            tower_group.compileCanvas();
            shrine_group.addChild(research_buttons[com.oddlabs.tt.model.ResearchContainer.BLESSING]);
            shrine_group.addChild(research_buttons[com.oddlabs.tt.model.ResearchContainer.LORE]);
            research_buttons[com.oddlabs.tt.model.ResearchContainer.BLESSING].place();
            research_buttons[com.oddlabs.tt.model.ResearchContainer.LORE].place(research_buttons[com.oddlabs.tt.model.ResearchContainer.BLESSING], com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            shrine_group.addChild(research_buttons[com.oddlabs.tt.model.ResearchContainer.ARCANE_POWER]);
            shrine_group.addChild(research_buttons[com.oddlabs.tt.model.ResearchContainer.COURAGE_MASTERY]);
            research_buttons[com.oddlabs.tt.model.ResearchContainer.ARCANE_POWER].place(research_buttons[com.oddlabs.tt.model.ResearchContainer.LORE], com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            research_buttons[com.oddlabs.tt.model.ResearchContainer.COURAGE_MASTERY].place(research_buttons[com.oddlabs.tt.model.ResearchContainer.ARCANE_POWER], com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            shrine_group.compileCanvas();
            palisade_group.addChild(research_buttons[com.oddlabs.tt.model.ResearchContainer.WALLS]);
            research_buttons[com.oddlabs.tt.model.ResearchContainer.WALLS].place();
            palisade_group.addChild(research_buttons[com.oddlabs.tt.model.ResearchContainer.STAKES]);
            research_buttons[com.oddlabs.tt.model.ResearchContainer.STAKES].place(research_buttons[com.oddlabs.tt.model.ResearchContainer.WALLS], com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            palisade_group.compileCanvas();
            hall_group.addChild(research_buttons[com.oddlabs.tt.model.ResearchContainer.RACE_1]);
            hall_group.addChild(research_buttons[com.oddlabs.tt.model.ResearchContainer.RACE_2]);
            research_buttons[com.oddlabs.tt.model.ResearchContainer.RACE_1].place();
            research_buttons[com.oddlabs.tt.model.ResearchContainer.RACE_2].place(research_buttons[com.oddlabs.tt.model.ResearchContainer.RACE_1], com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            hall_group.addChild(research_buttons[com.oddlabs.tt.model.ResearchContainer.FOCUS]);
            hall_group.addChild(research_buttons[com.oddlabs.tt.model.ResearchContainer.PROSPECTING]);
            research_buttons[com.oddlabs.tt.model.ResearchContainer.FOCUS].place(research_buttons[com.oddlabs.tt.model.ResearchContainer.RACE_2], com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            research_buttons[com.oddlabs.tt.model.ResearchContainer.PROSPECTING].place(research_buttons[com.oddlabs.tt.model.ResearchContainer.FOCUS], com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            hall_group.addChild(research_buttons[com.oddlabs.tt.model.ResearchContainer.ELITE_RANKS]);
            hall_group.addChild(research_buttons[com.oddlabs.tt.model.ResearchContainer.ELITE_TRAINING]);
            research_buttons[com.oddlabs.tt.model.ResearchContainer.ELITE_RANKS].place(research_buttons[com.oddlabs.tt.model.ResearchContainer.RACE_1], com.oddlabs.tt.gui.Placement.LEFT_MID, 5);
            research_buttons[com.oddlabs.tt.model.ResearchContainer.ELITE_TRAINING].place(research_buttons[com.oddlabs.tt.model.ResearchContainer.ELITE_RANKS], com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            god_buttons[0] = new com.oddlabs.tt.gui.GodButton(player_interface, extended_icons.get(race, "minor_god_icon"), com.oddlabs.tt.model.DeployType.MINOR_GOD, (vikings ? "deploy_valkyrie_tip" : "deploy_jaguar_spirit_tip"), com.oddlabs.tt.input.GameAction.TRAIN_MINOR_GOD);
            god_buttons[1] = new com.oddlabs.tt.gui.GodButton(player_interface, extended_icons.get(race, "major_god_icon"), com.oddlabs.tt.model.DeployType.MAJOR_GOD, (vikings ? "deploy_thunder_lord_tip" : "deploy_great_tiki_tip"), com.oddlabs.tt.input.GameAction.TRAIN_MAJOR_GOD);
            hall_group.addChild(god_buttons[0]);
            hall_group.addChild(god_buttons[1]);
            god_buttons[0].place(research_buttons[com.oddlabs.tt.model.ResearchContainer.ELITE_TRAINING], com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            god_buttons[1].place(god_buttons[0], com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            hall_group.compileCanvas();
            for (int u_2 = com.oddlabs.tt.model.ResearchContainer.CRYSTAL_CRAFT; u_2 <= com.oddlabs.tt.model.ResearchContainer.DIAMOND_EDGE; u_2++)
            {
                forge_group.addChild(research_buttons[u_2]);
                if (u_2 == com.oddlabs.tt.model.ResearchContainer.CRYSTAL_CRAFT)
                {
                    research_buttons[u_2].place();
                }
                else if (u_2 == com.oddlabs.tt.model.ResearchContainer.IRON_TEMPER)
                {
                    research_buttons[u_2].place(research_buttons[com.oddlabs.tt.model.ResearchContainer.CRYSTAL_CRAFT], com.oddlabs.tt.gui.Placement.LEFT_MID, 5);
                }
                else
                {
                    research_buttons[u_2].place(research_buttons[u_2 - 1], com.oddlabs.tt.gui.Placement.BOTTOM_MID);
                }
            }
            forge_group.compileCanvas();
            for (int u_3 = com.oddlabs.tt.model.ResearchContainer.RUNE_REACH; u_3 <= com.oddlabs.tt.model.ResearchContainer.RUNE_WARD; u_3++)
            {
                totem_group.addChild(research_buttons[u_3]);
                if (u_3 == com.oddlabs.tt.model.ResearchContainer.RUNE_REACH)
                {
                    research_buttons[u_3].place();
                }
                else
                {
                    research_buttons[u_3].place(research_buttons[u_3 - 1], com.oddlabs.tt.gui.Placement.BOTTOM_MID);
                }
            }
            totem_group.compileCanvas();
            dock_group.addChild(research_buttons[com.oddlabs.tt.model.ResearchContainer.SHIP_OARS]);
            dock_group.addChild(research_buttons[com.oddlabs.tt.model.ResearchContainer.SHIP_HULL]);
            research_buttons[com.oddlabs.tt.model.ResearchContainer.SHIP_OARS].place();
            research_buttons[com.oddlabs.tt.model.ResearchContainer.SHIP_HULL].place(research_buttons[com.oddlabs.tt.model.ResearchContainer.SHIP_OARS], com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            dock_group.addChild(research_buttons[com.oddlabs.tt.model.ResearchContainer.FLEET]);
            dock_group.addChild(research_buttons[com.oddlabs.tt.model.ResearchContainer.CARGO]);
            dock_group.addChild(research_buttons[com.oddlabs.tt.model.ResearchContainer.BRIDGES]);
            research_buttons[com.oddlabs.tt.model.ResearchContainer.BRIDGES].place(research_buttons[com.oddlabs.tt.model.ResearchContainer.SHIP_OARS], com.oddlabs.tt.gui.Placement.LEFT_MID, 5);
            research_buttons[com.oddlabs.tt.model.ResearchContainer.FLEET].place(research_buttons[com.oddlabs.tt.model.ResearchContainer.SHIP_HULL], com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            research_buttons[com.oddlabs.tt.model.ResearchContainer.CARGO].place(research_buttons[com.oddlabs.tt.model.ResearchContainer.FLEET], com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            dock_group.compileCanvas();
            {
                com.oddlabs.tt.model.DeployType[] mt = new com.oddlabs.tt.model.DeployType[] { com.oddlabs.tt.model.DeployType.MARKET_WOOD, com.oddlabs.tt.model.DeployType.MARKET_ROCK, com.oddlabs.tt.model.DeployType.MARKET_IRON, com.oddlabs.tt.model.DeployType.MARKET_CHICKEN, com.oddlabs.tt.model.DeployType.MARKET_CRYSTAL, com.oddlabs.tt.model.DeployType.MARKET_HERBS, com.oddlabs.tt.model.DeployType.MARKET_COAL, com.oddlabs.tt.model.DeployType.GIFT_WOOD, com.oddlabs.tt.model.DeployType.GIFT_ROCK, com.oddlabs.tt.model.DeployType.GIFT_IRON, com.oddlabs.tt.model.DeployType.GIFT_GOLD };
                com.oddlabs.tt.gui.ModeIconQuads[] mi = new com.oddlabs.tt.gui.ModeIconQuads[] { icons.getTreeIcon(), icons.getRockIcon(), icons.getIronIcon(), icons.getRubberIcon(), extended_icons.get(race, "harvest_crystal_icon"), extended_icons.get(race, "harvest_herbs_icon"), extended_icons.get(race, "harvest_coal_icon"), icons.getTreeIcon(), icons.getRockIcon(), icons.getIronIcon(), extended_icons.get(race, "harvest_gold_icon") };
                string[] names = new string[] { "wood", "rock", "iron", "chicken", "crystal", "herbs", "coal", "wood", "rock", "iron", "gold" };
                com.oddlabs.tt.input.GameAction[][] keys = new com.oddlabs.tt.input.GameAction[][] { new com.oddlabs.tt.input.GameAction[] { com.oddlabs.tt.input.GameAction.RES_TREE, com.oddlabs.tt.input.GameAction.RES_TREE_DEC }, new com.oddlabs.tt.input.GameAction[] { com.oddlabs.tt.input.GameAction.RES_ROCK, com.oddlabs.tt.input.GameAction.RES_ROCK_DEC }, new com.oddlabs.tt.input.GameAction[] { com.oddlabs.tt.input.GameAction.RES_IRON, com.oddlabs.tt.input.GameAction.RES_IRON_DEC }, new com.oddlabs.tt.input.GameAction[] { com.oddlabs.tt.input.GameAction.RES_CHICKEN, com.oddlabs.tt.input.GameAction.RES_CHICKEN_DEC }, new com.oddlabs.tt.input.GameAction[] { com.oddlabs.tt.input.GameAction.RES_CRYSTAL, com.oddlabs.tt.input.GameAction.RES_CRYSTAL_DEC }, new com.oddlabs.tt.input.GameAction[] { com.oddlabs.tt.input.GameAction.RES_HERBS, com.oddlabs.tt.input.GameAction.RES_HERBS_DEC }, new com.oddlabs.tt.input.GameAction[] { com.oddlabs.tt.input.GameAction.RES_COAL, com.oddlabs.tt.input.GameAction.RES_COAL_DEC }, new com.oddlabs.tt.input.GameAction[] { com.oddlabs.tt.input.GameAction.GIFT_WOOD, com.oddlabs.tt.input.GameAction.GIFT_WOOD_DEC }, new com.oddlabs.tt.input.GameAction[] { com.oddlabs.tt.input.GameAction.GIFT_ROCK, com.oddlabs.tt.input.GameAction.GIFT_ROCK_DEC }, new com.oddlabs.tt.input.GameAction[] { com.oddlabs.tt.input.GameAction.GIFT_IRON, com.oddlabs.tt.input.GameAction.GIFT_IRON_DEC }, new com.oddlabs.tt.input.GameAction[] { com.oddlabs.tt.input.GameAction.GIFT_GOLD, com.oddlabs.tt.input.GameAction.GIFT_GOLD_DEC } };
                for (int k = 0; k < mt.Length; k++)
                {
                    bool gift = k >= 7;
                    market_buttons[k] = new com.oddlabs.tt.gui.MarketSpinner(viewer, player_interface, mi[k], com.oddlabs.tt.gui.ActionButtonPanel.i18n((gift ? "market_gift_tip" : "market_trade_tip"), com.oddlabs.tt.gui.ActionButtonPanel.i18n("market_" + java.lang.JStr.valueOf(names[k]))), null, keys[k][0], keys[k][1], mt[k]);
                    market_group.addChild(market_buttons[k]);
                    if (k == 0)
                    {
                        market_buttons[k].place();
                    }
                    else if (k == 7)
                    {
                        market_buttons[k].place(market_buttons[0], com.oddlabs.tt.gui.Placement.LEFT_MID, 5);
                    }
                    else
                    {
                        market_buttons[k].place(market_buttons[k - 1], com.oddlabs.tt.gui.Placement.BOTTOM_MID);
                    }
                }
                market_group.compileCanvas();
            }
            com.oddlabs.tt.model.DeployType[] tier_types = new com.oddlabs.tt.model.DeployType[] { com.oddlabs.tt.model.DeployType.UPGRADE_STONE, com.oddlabs.tt.model.DeployType.UPGRADE_IRON, com.oddlabs.tt.model.DeployType.UPGRADE_CHICKEN, com.oddlabs.tt.model.DeployType.UPGRADE_RUNE };
            string[] tier_icons = new string[] { "upgrade_stone_icon", "upgrade_iron_icon", "upgrade_chicken_icon", "upgrade_rune_icon" };
            for (int k_2 = 0; k_2 < tier_buttons.Length; k_2++)
            {
                tier_buttons[k_2] = new com.oddlabs.tt.gui.TierButton(player_interface, extended_icons.get(race, tier_icons[k_2]), tier_types[k_2], k_2);
                tier_group.addChild(tier_buttons[k_2]);
                if (k_2 == 0)
                {
                    tier_buttons[k_2].place();
                }
                else
                {
                    tier_buttons[k_2].place(tier_buttons[k_2 - 1], com.oddlabs.tt.gui.Placement.BOTTOM_MID);
                }
            }
            tier_group.compileCanvas();
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
            for (int i = 0; i < rare_status.Length; i++)
            {
                rare_status[i] = new com.oddlabs.tt.gui.StatusIcon(label_width, extended_icons.getRareStatus(i), com.oddlabs.tt.gui.ActionButtonPanel.i18n(java.lang.JStr.valueOf(com.oddlabs.tt.gui.ExtendedIcons.RARE_NAMES[i]) + "_resources_tip"));
                if (extended)
                {
                    status_group.addChild(rare_status[i]);
                    if (i == 0)
                    {
                        rare_status[i].place(tree_status, com.oddlabs.tt.gui.Placement.LEFT_MID, 5);
                    }
                    else
                    {
                        rare_status[i].place(rare_status[i - 1], com.oddlabs.tt.gui.Placement.BOTTOM_MID);
                    }
                }
            }
            string[] arsenal_tips = new string[] { "crystal_weapons_tip", "diamond_weapons_tip", (vikings ? "firepots_weapons_tip" : "darts_weapons_tip") };
            string[] arsenal_icons = new string[] { "crystal_weapon_status_icon", "diamond_weapon_status_icon", (vikings ? "firepots_status_icon" : "darts_status_icon") };
            for (int i_2 = 0; i_2 < arsenal_status.Length; i_2++)
            {
                arsenal_status[i_2] = new com.oddlabs.tt.gui.StatusIcon(label_width, extended_icons.getStatus(vikings, arsenal_icons[i_2]), com.oddlabs.tt.gui.ActionButtonPanel.i18n(arsenal_tips[i_2]));
                if (extended)
                {
                    status_group.addChild(arsenal_status[i_2]);
                    if (i_2 == 0)
                    {
                        arsenal_status[i_2].place(weapon_rubber_status, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
                    }
                    else
                    {
                        arsenal_status[i_2].place(arsenal_status[i_2 - 1], com.oddlabs.tt.gui.Placement.BOTTOM_MID);
                    }
                }
            }
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
            if (extended)
            {
                quarters_group.addChild(research_buttons[com.oddlabs.tt.model.ResearchContainer.GROWTH]);
                research_buttons[com.oddlabs.tt.model.ResearchContainer.GROWTH].place(quarters_rally_point_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
                int prev = com.oddlabs.tt.model.ResearchContainer.GROWTH;
                foreach (int u_4 in new int[] { com.oddlabs.tt.model.ResearchContainer.STURDY, com.oddlabs.tt.model.ResearchContainer.BASKETS, com.oddlabs.tt.model.ResearchContainer.VIGOR, com.oddlabs.tt.model.ResearchContainer.BUILDINGS })
                {
                    quarters_group.addChild(research_buttons[u_4]);
                    research_buttons[u_4].place(research_buttons[prev], com.oddlabs.tt.gui.Placement.BOTTOM_MID);
                    prev = u_4;
                }
                quarters_group.addChild(research_buttons[com.oddlabs.tt.model.ResearchContainer.GREAT_TRIBE]);
                research_buttons[com.oddlabs.tt.model.ResearchContainer.GREAT_TRIBE].place(research_buttons[com.oddlabs.tt.model.ResearchContainer.GROWTH], com.oddlabs.tt.gui.Placement.LEFT_MID, 5);
            }
            quarters_group.compileCanvas(com.oddlabs.tt.gui.ActionButtonPanel.GROUP_LEFT_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_BOTTOM_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_RIGHT_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_TOP_OFFSET);
            harvest_button = new com.oddlabs.tt.gui.NonFocusIconButton(icons.getHarvestIcon(), com.oddlabs.tt.input.GameAction.PROD_HARVEST, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("gather_resources_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.PROD_HARVEST));
            }));
            harvest_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return !viewer.getLocalPlayer().canHarvest();
            }));
            armory_group.addChild(harvest_button);
            harvest_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_81, int unused_82, int unused_83, int unused_84) =>
            {
                openSubmenu(harvest_group);
            }));
            build_button = new com.oddlabs.tt.gui.NonFocusIconButton(race_icons.buildWeaponsIcon(), com.oddlabs.tt.input.GameAction.PROD_WEAPONS, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("produce_weapons_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.PROD_WEAPONS));
            }));
            build_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return !viewer.getLocalPlayer().canBuildWeapons();
            }));
            armory_group.addChild(build_button);
            build_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_85, int unused_86, int unused_87, int unused_88) =>
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
                return !viewer.getLocalPlayer().canBuildArmies();
            }));
            armory_group.addChild(army_button);
            army_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_89, int unused_90, int unused_91, int unused_92) =>
            {
                openSubmenu(army_group);
            }));
            transport_button = new com.oddlabs.tt.gui.NonFocusIconButton(race_icons.transportIcon(), com.oddlabs.tt.input.GameAction.PROD_TRANSPORT, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("transport_resources_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.PROD_TRANSPORT));
            }));
            armory_group.addChild(transport_button);
            transport_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_93, int unused_94, int unused_95, int unused_96) =>
            {
                openSubmenu(transport_group);
            }));
            rally_point_button = new com.oddlabs.tt.gui.NonFocusIconButton(race_icons.rallyPointIcon(), com.oddlabs.tt.input.GameAction.UNIT_SET_RALLY, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("rally_point_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.UNIT_SET_RALLY));
            }));
            rally_point_button.setIconDisabler(new com.oddlabs.tt.gui.IconDisabler_L(() =>
            {
                return !viewer.getLocalPlayer().canSetRallyPoints();
            }));
            armory_group.addChild(rally_point_button);
            rally_point_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton a0, int a1, int a2, int a3) => { this.setRallyPoint(a0, a1, a2, a3); }));
            research_button = new com.oddlabs.tt.gui.NonFocusIconButton(extended_icons.get(race, "research_icon"), com.oddlabs.tt.input.GameAction.PROD_RESEARCH, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("research_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.PROD_RESEARCH));
            }));
            research_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_97, int unused_98, int unused_99, int unused_100) =>
            {
                openSubmenu(research_group);
            }));
            harvest_button.place();
            build_button.place(harvest_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            army_button.place(build_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            transport_button.place(army_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            rally_point_button.place(transport_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            if (extended)
            {
                armory_group.addChild(research_button);
                research_button.place(rally_point_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            }
            foreach (int u_5 in com.oddlabs.tt.gui.ActionButtonPanel.ARMORY_RESEARCH)
            {
                research_group.addChild(research_buttons[u_5]);
            }
            research_back_button = new com.oddlabs.tt.gui.NonFocusIconButton(skin.getBackButton(), com.oddlabs.tt.input.GameAction.GAMEPLAY_BACK, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("back_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.GAMEPLAY_BACK));
            }));
            research_back_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton a0, int a1, int a2, int a3) => { this.cancelSubMenu(a0, a1, a2, a3); }));
            research_group.addChild(research_back_button);
            research_buttons[com.oddlabs.tt.gui.ActionButtonPanel.ARMORY_RESEARCH[0]].place();
            for (int i_3 = 1; i_3 < com.oddlabs.tt.gui.ActionButtonPanel.ARMORY_RESEARCH.Length; i_3++)
            {
                research_buttons[com.oddlabs.tt.gui.ActionButtonPanel.ARMORY_RESEARCH[i_3]].place(research_buttons[com.oddlabs.tt.gui.ActionButtonPanel.ARMORY_RESEARCH[i_3 - 1]], com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            }
            research_back_button.place(research_buttons[com.oddlabs.tt.gui.ActionButtonPanel.ARMORY_RESEARCH[com.oddlabs.tt.gui.ActionButtonPanel.ARMORY_RESEARCH.Length - 1]], com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            research_group.compileCanvas(com.oddlabs.tt.gui.ActionButtonPanel.GROUP_LEFT_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_BOTTOM_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_RIGHT_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_TOP_OFFSET);
            armory_group.compileCanvas(com.oddlabs.tt.gui.ActionButtonPanel.GROUP_LEFT_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_BOTTOM_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_RIGHT_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_TOP_OFFSET);
            ship_harvest_button = new com.oddlabs.tt.gui.NonFocusIconButton(icons.getHarvestIcon(), com.oddlabs.tt.input.GameAction.PROD_HARVEST, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("gather_resources_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.PROD_HARVEST));
            }));
            ship_group.addChild(ship_harvest_button);
            ship_harvest_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_101, int unused_102, int unused_103, int unused_104) =>
            {
                openSubmenu(harvest_group);
            }));
            ship_army_button = new com.oddlabs.tt.gui.NonFocusIconButton(race_icons.armyIcon(), com.oddlabs.tt.input.GameAction.PROD_ARMY, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("deploy_army_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.PROD_ARMY));
            }));
            ship_group.addChild(ship_army_button);
            ship_army_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_105, int unused_106, int unused_107, int unused_108) =>
            {
                openSubmenu(army_group);
            }));
            ship_transport_button = new com.oddlabs.tt.gui.NonFocusIconButton(race_icons.transportIcon(), com.oddlabs.tt.input.GameAction.PROD_TRANSPORT, new java.util.function.Supplier_L(() =>
            {
                return com.oddlabs.tt.gui.ActionButtonPanel.i18n("transport_resources_tip", com.oddlabs.tt.gui.ActionButtonPanel.getBinding(com.oddlabs.tt.input.GameAction.PROD_TRANSPORT));
            }));
            ship_group.addChild(ship_transport_button);
            ship_transport_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_109, int unused_110, int unused_111, int unused_112) =>
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
            ship_sail_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_113, int unused_114, int unused_115, int unused_116) =>
            {
                pushDelegate(new com.oddlabs.tt.@delegate.TargetDelegate(viewer, camera, com.oddlabs.tt.model.Action.MOVE, true));
            }));
            ship_sail_button.place(ship_rally_point_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            ship_group.compileCanvas(com.oddlabs.tt.gui.ActionButtonPanel.GROUP_LEFT_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_BOTTOM_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_RIGHT_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_TOP_OFFSET);
            com.oddlabs.tt.player.Player local_player = viewer.getLocalPlayer();
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
            for (int i_4 = 0; i_4 < harvest_rare_buttons.Length; i_4++)
            {
                string n = com.oddlabs.tt.gui.ExtendedIcons.RARE_NAMES[i_4];
                harvest_rare_buttons[i_4] = new com.oddlabs.tt.gui.DeploySpinner(viewer, player_interface, extended_icons.get(race, "harvest_" + java.lang.JStr.valueOf(n) + "_icon"), com.oddlabs.tt.gui.ActionButtonPanel.i18n("harvest_" + java.lang.JStr.valueOf(n) + "_tip"), java.util.List_.of(race_icons.unitStatusIcon()), com.oddlabs.tt.gui.ActionButtonPanel.RARE_INC[i_4], com.oddlabs.tt.gui.ActionButtonPanel.RARE_DEC[i_4], local_player, com.oddlabs.tt.model.ExtendedResources.RARE[i_4]);
                if (extended)
                {
                    harvest_group.addChild(harvest_rare_buttons[i_4]);
                    if (i_4 == 0)
                    {
                        harvest_rare_buttons[i_4].place(harvest_tree_button, com.oddlabs.tt.gui.Placement.LEFT_MID, 5);
                    }
                    else
                    {
                        harvest_rare_buttons[i_4].place(harvest_rare_buttons[i_4 - 1], com.oddlabs.tt.gui.Placement.BOTTOM_MID);
                    }
                }
            }
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
            if (extended)
            {
                com.oddlabs.tt.gui.IconQuad crystal_res = extended_icons.getRareStatus(1);
                com.oddlabs.tt.gui.IconQuad coal_res = extended_icons.getRareStatus(3);
                com.oddlabs.tt.gui.IconQuad herbs_res = extended_icons.getRareStatus(2);
                craft_buttons[0] = new com.oddlabs.tt.gui.CraftSpinner(viewer, player_interface, extended_icons.get(race, "craft_crystal_icon"), com.oddlabs.tt.gui.ActionButtonPanel.i18n("craft_crystal_tip"), java.util.List_.of(icons.getTreeStatusIcon(), icons.getTreeStatusIcon(), icons.getRockStatusIcon(), crystal_res, crystal_res), com.oddlabs.tt.input.GameAction.CRAFT_CRYSTAL, com.oddlabs.tt.input.GameAction.CRAFT_CRYSTAL_DEC, com.oddlabs.tt.model.DeployType.CRAFT_CRYSTAL);
                craft_buttons[1] = new com.oddlabs.tt.gui.CraftSpinner(viewer, player_interface, extended_icons.get(race, "craft_diamond_icon"), com.oddlabs.tt.gui.ActionButtonPanel.i18n("craft_diamond_tip"), java.util.List_.of(icons.getTreeStatusIcon(), icons.getTreeStatusIcon(), icons.getIronStatusIcon(), crystal_res, crystal_res, crystal_res, coal_res, coal_res), com.oddlabs.tt.input.GameAction.CRAFT_DIAMOND, com.oddlabs.tt.input.GameAction.CRAFT_DIAMOND_DEC, com.oddlabs.tt.model.DeployType.CRAFT_DIAMOND);
                craft_buttons[2] = new com.oddlabs.tt.gui.CraftSpinner(viewer, player_interface, extended_icons.get(race, "craft_darts_icon"), com.oddlabs.tt.gui.ActionButtonPanel.i18n((vikings ? "craft_firepots_tip" : "craft_darts_tip")), java.util.List_.of(icons.getTreeStatusIcon(), (vikings ? coal_res : herbs_res)), com.oddlabs.tt.input.GameAction.CRAFT_DARTS, com.oddlabs.tt.input.GameAction.CRAFT_DARTS_DEC, com.oddlabs.tt.model.DeployType.CRAFT_DARTS);
                for (int i_5 = 0; i_5 < craft_buttons.Length; i_5++)
                {
                    build_group.addChild(craft_buttons[i_5]);
                    if (i_5 == 0)
                    {
                        craft_buttons[i_5].place(build_weapon_rock_button, com.oddlabs.tt.gui.Placement.LEFT_MID, 5);
                    }
                    else
                    {
                        craft_buttons[i_5].place(craft_buttons[i_5 - 1], com.oddlabs.tt.gui.Placement.BOTTOM_MID);
                    }
                }
            }
            build_group.compileCanvas(com.oddlabs.tt.gui.ActionButtonPanel.GROUP_LEFT_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_BOTTOM_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_RIGHT_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_TOP_OFFSET);
            army_peon_button = new com.oddlabs.tt.gui.DeploySpinner(viewer, player_interface, race_icons.peonIcon(), com.oddlabs.tt.gui.ActionButtonPanel.i18n("deploy_peon_tip"), java.util.List_.of(race_icons.unitStatusIcon()), com.oddlabs.tt.input.GameAction.TRAIN_PEON, com.oddlabs.tt.input.GameAction.TRAIN_PEON_DEC, null, null);
            army_group.addChild(army_peon_button);
            army_warrior_rock_button = new com.oddlabs.tt.gui.DeploySpinner(viewer, player_interface, race_icons.warriorRockIcon(), com.oddlabs.tt.gui.ActionButtonPanel.i18n("deploy_rock_tip"), java.util.List_.of(race_icons.unitStatusIcon(), race_icons.weaponRockStatusIcon()), com.oddlabs.tt.input.GameAction.RES_ROCK, com.oddlabs.tt.input.GameAction.RES_ROCK_DEC, null, null);
            army_group.addChild(army_warrior_rock_button);
            army_warrior_iron_button = new com.oddlabs.tt.gui.DeploySpinner(viewer, player_interface, race_icons.warriorIronIcon(), com.oddlabs.tt.gui.ActionButtonPanel.i18n("deploy_iron_tip"), java.util.List_.of(race_icons.unitStatusIcon(), race_icons.weaponIronStatusIcon()), com.oddlabs.tt.input.GameAction.RES_IRON, com.oddlabs.tt.input.GameAction.RES_IRON_DEC, null, null);
            army_group.addChild(army_warrior_iron_button);
            army_warrior_rubber_button = new com.oddlabs.tt.gui.DeploySpinner(viewer, player_interface, race_icons.warriorRubberIcon(), com.oddlabs.tt.gui.ActionButtonPanel.i18n("deploy_chicken_tip"), java.util.List_.of(race_icons.unitStatusIcon(), race_icons.weaponRubberStatusIcon()), com.oddlabs.tt.input.GameAction.RES_CHICKEN, com.oddlabs.tt.input.GameAction.RES_CHICKEN_DEC, null, null);
            army_group.addChild(army_warrior_rubber_button);
            army_champion_button = new com.oddlabs.tt.gui.DeploySpinner(viewer, player_interface, extended_icons.get(race, "champion_icon"), com.oddlabs.tt.gui.ActionButtonPanel.i18n("deploy_champion_tip"), java.util.List_.of(race_icons.unitStatusIcon(), race_icons.weaponIronStatusIcon(), race_icons.weaponIronStatusIcon()), com.oddlabs.tt.input.GameAction.TRAIN_CHAMPION, com.oddlabs.tt.input.GameAction.TRAIN_CHAMPION_DEC, null, null);
            army_scout_button = new com.oddlabs.tt.gui.DeploySpinner(viewer, player_interface, extended_icons.get(race, "scout_icon"), com.oddlabs.tt.gui.ActionButtonPanel.i18n("deploy_scout_tip"), java.util.List_.of(race_icons.unitStatusIcon(), race_icons.weaponRockStatusIcon()), com.oddlabs.tt.input.GameAction.TRAIN_SCOUT, com.oddlabs.tt.input.GameAction.TRAIN_SCOUT_DEC, null, null);
            army_shaman_button = new com.oddlabs.tt.gui.DeploySpinner(viewer, player_interface, extended_icons.get(race, "shaman_icon"), com.oddlabs.tt.gui.ActionButtonPanel.i18n((vikings ? "deploy_skald_tip" : "deploy_shaman_tip")), java.util.List_.of(race_icons.unitStatusIcon(), race_icons.weaponRockStatusIcon(), race_icons.weaponRockStatusIcon()), com.oddlabs.tt.input.GameAction.TRAIN_SHAMAN, com.oddlabs.tt.input.GameAction.TRAIN_SHAMAN_DEC, null, null);
            army_slinger_button = new com.oddlabs.tt.gui.DeploySpinner(viewer, player_interface, extended_icons.get(race, "slinger_icon"), com.oddlabs.tt.gui.ActionButtonPanel.i18n("deploy_slinger_tip"), java.util.List_.of(race_icons.unitStatusIcon(), race_icons.weaponRockStatusIcon()), com.oddlabs.tt.input.GameAction.TRAIN_SLINGER, com.oddlabs.tt.input.GameAction.TRAIN_SLINGER_DEC, null, null);
            army_melee_light_button = new com.oddlabs.tt.gui.DeploySpinner(viewer, player_interface, extended_icons.get(race, "melee_light_icon"), com.oddlabs.tt.gui.ActionButtonPanel.i18n((vikings ? "deploy_axeman_tip" : "deploy_club_warrior_tip")), java.util.List_.of(race_icons.unitStatusIcon(), race_icons.weaponRockStatusIcon()), com.oddlabs.tt.input.GameAction.TRAIN_MELEE_LIGHT, com.oddlabs.tt.input.GameAction.TRAIN_MELEE_LIGHT_DEC, null, null);
            army_melee_heavy_button = new com.oddlabs.tt.gui.DeploySpinner(viewer, player_interface, extended_icons.get(race, "melee_heavy_icon"), com.oddlabs.tt.gui.ActionButtonPanel.i18n((vikings ? "deploy_huscarl_tip" : "deploy_spear_guard_tip")), java.util.List_.of(race_icons.unitStatusIcon(), race_icons.weaponIronStatusIcon()), com.oddlabs.tt.input.GameAction.TRAIN_MELEE_HEAVY, com.oddlabs.tt.input.GameAction.TRAIN_MELEE_HEAVY_DEC, null, null);
            com.oddlabs.tt.gui.IconQuad herbs_or_coal = extended_icons.getRareStatus((vikings ? 3 : 2));
            com.oddlabs.tt.gui.IconQuad gold_icon = extended_icons.getRareStatus(0);
            army_unique1_button = new com.oddlabs.tt.gui.DeploySpinner(viewer, player_interface, extended_icons.get(race, "unique1_icon"), com.oddlabs.tt.gui.ActionButtonPanel.i18n((vikings ? "deploy_fire_thrower_tip" : "deploy_blowgunner_tip")), java.util.List_.of(race_icons.unitStatusIcon(), extended_icons.getStatus((vikings ? "firepots_status_icon" : "darts_status_icon"))), com.oddlabs.tt.input.GameAction.TRAIN_UNIQUE_1, com.oddlabs.tt.input.GameAction.TRAIN_UNIQUE_1_DEC, null, null);
            army_unique2_button = new com.oddlabs.tt.gui.DeploySpinner(viewer, player_interface, extended_icons.get(race, "unique2_icon"), com.oddlabs.tt.gui.ActionButtonPanel.i18n((vikings ? "deploy_berserker_tip" : "deploy_jaguar_tip")), (vikings ? java.util.List_.of(race_icons.unitStatusIcon(), race_icons.weaponIronStatusIcon(), gold_icon, gold_icon, gold_icon) : java.util.List_.of(race_icons.unitStatusIcon(), race_icons.weaponIronStatusIcon(), gold_icon, gold_icon, extended_icons.getRareStatus(1))), com.oddlabs.tt.input.GameAction.TRAIN_UNIQUE_2, com.oddlabs.tt.input.GameAction.TRAIN_UNIQUE_2_DEC, null, null);
            com.oddlabs.tt.gui.IconQuad crystal_weapon = extended_icons.getStatus(vikings, "crystal_weapon_status_icon");
            com.oddlabs.tt.gui.IconQuad diamond_weapon = extended_icons.getStatus(vikings, "diamond_weapon_status_icon");
            com.oddlabs.tt.gui.IconQuad dart_weapon = extended_icons.getStatus((vikings ? "firepots_status_icon" : "darts_status_icon"));
            army_crystal_button = new com.oddlabs.tt.gui.DeploySpinner(viewer, player_interface, extended_icons.get(race, "crystal_icon"), com.oddlabs.tt.gui.ActionButtonPanel.i18n((vikings ? "deploy_crystal_raider_tip" : "deploy_crystal_thrower_tip")), java.util.List_.of(race_icons.unitStatusIcon(), crystal_weapon), com.oddlabs.tt.input.GameAction.TRAIN_CRYSTAL, com.oddlabs.tt.input.GameAction.TRAIN_CRYSTAL_DEC, null, null);
            army_diamond_button = new com.oddlabs.tt.gui.DeploySpinner(viewer, player_interface, extended_icons.get(race, "diamond_icon"), com.oddlabs.tt.gui.ActionButtonPanel.i18n("deploy_diamond_guard_tip"), java.util.List_.of(race_icons.unitStatusIcon(), diamond_weapon), com.oddlabs.tt.input.GameAction.TRAIN_DIAMOND, com.oddlabs.tt.input.GameAction.TRAIN_DIAMOND_DEC, null, null);
            army_unique3_button = new com.oddlabs.tt.gui.DeploySpinner(viewer, player_interface, extended_icons.get(race, "unique3_icon"), com.oddlabs.tt.gui.ActionButtonPanel.i18n((vikings ? "deploy_shield_maiden_tip" : "deploy_dart_master_tip")), (vikings ? java.util.List_.of(race_icons.unitStatusIcon(), race_icons.weaponIronStatusIcon(), gold_icon, gold_icon) : java.util.List_.of(race_icons.unitStatusIcon(), dart_weapon, dart_weapon, extended_icons.getRareStatus(1))), com.oddlabs.tt.input.GameAction.TRAIN_UNIQUE_3, com.oddlabs.tt.input.GameAction.TRAIN_UNIQUE_3_DEC, null, null);
            com.oddlabs.tt.gui.IconQuad herb_or_coal_icon = extended_icons.getRareStatus((vikings ? 3 : 2));
            army_minor_god_button = new com.oddlabs.tt.gui.DeploySpinner(viewer, player_interface, extended_icons.get(race, "minor_god_icon"), com.oddlabs.tt.gui.ActionButtonPanel.i18n((vikings ? "deploy_valkyrie_tip" : "deploy_jaguar_spirit_tip")), java.util.List_.of(race_icons.unitStatusIcon(), diamond_weapon, gold_icon, extended_icons.getRareStatus(1)), com.oddlabs.tt.input.GameAction.TRAIN_MINOR_GOD, com.oddlabs.tt.input.GameAction.TRAIN_MINOR_GOD_DEC, null, null);
            army_major_god_button = new com.oddlabs.tt.gui.DeploySpinner(viewer, player_interface, extended_icons.get(race, "major_god_icon"), com.oddlabs.tt.gui.ActionButtonPanel.i18n((vikings ? "deploy_thunder_lord_tip" : "deploy_great_tiki_tip")), java.util.List_.of(race_icons.unitStatusIcon(), diamond_weapon, diamond_weapon, gold_icon, extended_icons.getRareStatus(1), herb_or_coal_icon), com.oddlabs.tt.input.GameAction.TRAIN_MAJOR_GOD, com.oddlabs.tt.input.GameAction.TRAIN_MAJOR_GOD_DEC, null, null);
            com.oddlabs.tt.gui.IconQuad crystal_icon = extended_icons.getRareStatus(1);
            army_elite_buttons[0] = new com.oddlabs.tt.gui.DeploySpinner(viewer, player_interface, extended_icons.get(race, "elite1_icon"), com.oddlabs.tt.gui.ActionButtonPanel.i18n((vikings ? "deploy_jarl_tip" : "deploy_eagle_knight_tip")), java.util.List_.of(race_icons.unitStatusIcon(), race_icons.weaponIronStatusIcon(), gold_icon, crystal_icon, herb_or_coal_icon), com.oddlabs.tt.input.GameAction.TRAIN_ELITE_1, com.oddlabs.tt.input.GameAction.TRAIN_ELITE_1_DEC, null, null);
            army_elite_buttons[1] = new com.oddlabs.tt.gui.DeploySpinner(viewer, player_interface, extended_icons.get(race, "elite2_icon"), com.oddlabs.tt.gui.ActionButtonPanel.i18n((vikings ? "deploy_huntress_tip" : "deploy_hawk_eye_tip")), java.util.List_.of(race_icons.unitStatusIcon(), race_icons.weaponIronStatusIcon(), gold_icon, crystal_icon), com.oddlabs.tt.input.GameAction.TRAIN_ELITE_2, com.oddlabs.tt.input.GameAction.TRAIN_ELITE_2_DEC, null, null);
            army_elite_buttons[2] = new com.oddlabs.tt.gui.DeploySpinner(viewer, player_interface, extended_icons.get(race, "elite3_icon"), com.oddlabs.tt.gui.ActionButtonPanel.i18n((vikings ? "deploy_horn_bearer_tip" : "deploy_war_drummer_tip")), java.util.List_.of(race_icons.unitStatusIcon(), race_icons.weaponRockStatusIcon(), gold_icon, crystal_icon, herb_or_coal_icon), com.oddlabs.tt.input.GameAction.TRAIN_ELITE_3, com.oddlabs.tt.input.GameAction.TRAIN_ELITE_3_DEC, null, null);
            army_spinner_button = new com.oddlabs.tt.gui.DeploySpinner(viewer, player_interface, extended_icons.get(race, "spinner_icon"), com.oddlabs.tt.gui.ActionButtonPanel.i18n((vikings ? "deploy_whirlwind_tip" : "deploy_feather_whirler_tip")), java.util.List_.of(race_icons.unitStatusIcon(), race_icons.weaponRubberStatusIcon(), race_icons.weaponRubberStatusIcon()), com.oddlabs.tt.input.GameAction.TRAIN_SPINNER, com.oddlabs.tt.input.GameAction.TRAIN_SPINNER_DEC, null, null);
            if (extended)
            {
                army_group.addChild(army_spinner_button);
                foreach (com.oddlabs.tt.gui.DeploySpinner e in army_elite_buttons)
                {
                    army_group.addChild(e);
                }
                army_group.addChild(army_crystal_button);
                army_group.addChild(army_diamond_button);
                army_group.addChild(army_unique3_button);
                army_group.addChild(army_unique1_button);
                army_group.addChild(army_unique2_button);
                army_group.addChild(army_champion_button);
                army_group.addChild(army_scout_button);
                army_group.addChild(army_shaman_button);
                army_group.addChild(army_slinger_button);
                army_group.addChild(army_melee_light_button);
                army_group.addChild(army_melee_heavy_button);
            }
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
            if (extended)
            {
                army_champion_button.place(army_peon_button, com.oddlabs.tt.gui.Placement.LEFT_MID, 5);
                army_scout_button.place(army_champion_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
                army_shaman_button.place(army_scout_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
                army_slinger_button.place(army_shaman_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
                army_melee_light_button.place(army_slinger_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
                army_melee_heavy_button.place(army_melee_light_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
                army_unique1_button.place(army_champion_button, com.oddlabs.tt.gui.Placement.LEFT_MID, 5);
                army_unique2_button.place(army_unique1_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
                army_unique3_button.place(army_unique2_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
                army_crystal_button.place(army_unique3_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
                army_diamond_button.place(army_crystal_button, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
                army_elite_buttons[0].place(army_unique1_button, com.oddlabs.tt.gui.Placement.LEFT_MID, 5);
                army_elite_buttons[1].place(army_elite_buttons[0], com.oddlabs.tt.gui.Placement.BOTTOM_MID);
                army_elite_buttons[2].place(army_elite_buttons[1], com.oddlabs.tt.gui.Placement.BOTTOM_MID);
                army_spinner_button.place(army_elite_buttons[2], com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            }
            army_group.compileCanvas(com.oddlabs.tt.gui.ActionButtonPanel.GROUP_LEFT_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_BOTTOM_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_RIGHT_OFFSET, com.oddlabs.tt.gui.ActionButtonPanel.GROUP_TOP_OFFSET);
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
            setCanFocus(true);
            displayChangedNotify(width, height);
        }
        // mixin of default method Animated.updateChecksum
        public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
        }
    }
}
