// Translated from TerrainMenu.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 73
    public class TerrainMenu : com.oddlabs.tt.gui.Group
    {
        public static int[] SIZES;
        public static bool[] ARCHIPELAGO;
        public const int SLIDER_LENGTH = 250;
        public const int BUTTON_WIDTH = 100;
        public const int SLIDER_MAX_VALUE = 10;
        public const string SEED_CARDINALITY = "40000";
        public const int SLIDER_CARDINALITY = 11;
        public const int TERRAIN_TYPE_CARDINALITY = 4;
        public const int TERRAIN_TYPE_CARDINALITY_LEGACY = 2;
        public const int SIZE_CARDINALITY = 7;
        public const int SIZE_CARDINALITY_LEGACY = 4;
        public const int DIFFICULTY_CARDINALITY = 4;
        public const int RACE_CARDINALITY = 2;
        public const int TEAM_CARDINALITY = 6;
        public static java.math.BigInteger MAX_VALUE;
        public static java.math.BigInteger MAX_VALUE_LEGACY;
        public static java.util.ResourceBundle bundle;
        // Java: 95
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.form.TerrainMenu.bundle, key, args);
        }
        public com.oddlabs.tt.@delegate.Menu main_menu;
        public com.oddlabs.tt.form.TerrainMenuListener owner;
        public com.oddlabs.tt.gui.PulldownMenu pulldown_size;
        public com.oddlabs.tt.gui.EditLine editline_name;
        public com.oddlabs.tt.gui.PulldownMenu pm_terrain_type;
        public com.oddlabs.tt.gui.PulldownMenu pm_time_of_day;
        public com.oddlabs.tt.gui.Slider slider_hills;
        public com.oddlabs.tt.gui.Slider slider_vegetation;
        public com.oddlabs.tt.gui.Slider slider_supplies;
        public com.oddlabs.tt.gui.Label label_mapcode;
        public com.oddlabs.tt.gui.HorizButton button_ok;
        public com.oddlabs.tt.gui.HorizButton button_advanced;
        public com.oddlabs.tt.gui.PulldownMenu[] difficulty_pulldown_menus;
        public com.oddlabs.tt.gui.PulldownMenu[] race_pulldown_menus;
        public com.oddlabs.tt.gui.PulldownMenu[] team_pulldown_menus;
        public com.oddlabs.tt.gui.PulldownButton[] difficulty_pulldown_buttons;
        public com.oddlabs.tt.gui.PulldownButton[] race_pulldown_buttons;
        public com.oddlabs.tt.gui.PulldownButton[] team_pulldown_buttons;
        public com.oddlabs.tt.gui.Label[] labels_players;
        public com.oddlabs.tt.gui.CheckBox cb_rated;
        public bool multiplayer;
        public com.oddlabs.tt.gui.PulldownMenu pm_gamespeed;
        public com.oddlabs.tt.gui.GUIRoot gui_root;
        public com.oddlabs.net.NetworkSelector network;
        public com.oddlabs.tt.gamemode.PresetLibrary preset_library;
        public com.oddlabs.tt.form.RosterPanel roster_panel;
        public com.oddlabs.tt.form.ModeAndPresetsPanel mode_and_presets;
        public com.oddlabs.tt.gui.ScrollablePulldownMenu pulldown_menu_slots;
        public const int DEFAULT_PLAYER_COUNT = 6;
        public const int DAY = 0;
        public const int NIGHT = 1;
        public int player_count;
        public int seed;
        public com.oddlabs.tt.form.AdvancedSettingsForm.Values advanced_settings;
        public com.oddlabs.matchmaking.Preset current_preset;
        public bool modified;
        public bool apply_in_progress;
        public bool initialized;
        // Java: 490
        public void applyAdvancedSettings(com.oddlabs.tt.form.AdvancedSettingsForm.Values values)
        {
            if (!java.lang.JO.equals(values, advanced_settings))
            {
                advanced_settings = values;
                markModified();
            }
        }
        // Java: 497
        public void markModified()
        {
            if (!initialized || apply_in_progress)
            {
                return;
            }
            if (!modified)
            {
                modified = true;
                updateBanner();
            }
        }
        // Java: 507
        public void updateBanner()
        {
            if (mode_and_presets != null)
            {
                mode_and_presets.setPresetState(current_preset, modified);
            }
        }
        // Java: 513
        public void setMapcode()
        {
            java.math.BigInteger max_val = java.math.BigInteger.ONE;
            java.math.BigInteger result = java.math.BigInteger.ZERO;
            result = result.add((new java.math.BigInteger("" + (seed))).multiply(max_val));
            max_val = max_val.multiply(new java.math.BigInteger(com.oddlabs.tt.form.TerrainMenu.SEED_CARDINALITY));
            int hills = slider_hills.getValue();
            result = result.add((new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(hills)) })).multiply(max_val));
            max_val = max_val.multiply(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.SLIDER_CARDINALITY)) }));
            int vegetation_amount = slider_vegetation.getValue();
            result = result.add((new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(vegetation_amount)) })).multiply(max_val));
            max_val = max_val.multiply(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.SLIDER_CARDINALITY)) }));
            int supplies_amount = slider_supplies.getValue();
            result = result.add((new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(supplies_amount)) })).multiply(max_val));
            max_val = max_val.multiply(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.SLIDER_CARDINALITY)) }));
            int terrain_type = pm_terrain_type.getChosenItemIndex();
            result = result.add((new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(terrain_type)) })).multiply(max_val));
            max_val = max_val.multiply(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.TERRAIN_TYPE_CARDINALITY)) }));
            int size = pulldown_size.getChosenItemIndex();
            result = result.add((new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(size)) })).multiply(max_val));
            string code = com.oddlabs.tt.util.WordsEncoding.encode(result);
            label_mapcode.clear();
            label_mapcode.append(code);
            markModified();
        }
        // Java: 539
        public void setMapcodeLegacy()
        {
            java.math.BigInteger max_val = java.math.BigInteger.ONE;
            java.math.BigInteger result = java.math.BigInteger.ZERO;
            result = result.add((new java.math.BigInteger("" + (seed))).multiply(max_val));
            max_val = max_val.multiply(new java.math.BigInteger(com.oddlabs.tt.form.TerrainMenu.SEED_CARDINALITY));
            int hills = slider_hills.getValue();
            result = result.add((new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(hills)) })).multiply(max_val));
            max_val = max_val.multiply(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.SLIDER_CARDINALITY)) }));
            int vegetation_amount = slider_vegetation.getValue();
            result = result.add((new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(vegetation_amount)) })).multiply(max_val));
            max_val = max_val.multiply(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.SLIDER_CARDINALITY)) }));
            int supplies_amount = slider_supplies.getValue();
            result = result.add((new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(supplies_amount)) })).multiply(max_val));
            max_val = max_val.multiply(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.SLIDER_CARDINALITY)) }));
            int terrain_type = pm_terrain_type.getChosenItemIndex();
            result = result.add((new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(terrain_type)) })).multiply(max_val));
            max_val = max_val.multiply(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.TERRAIN_TYPE_CARDINALITY_LEGACY)) }));
            int size = pulldown_size.getChosenItemIndex();
            result = result.add((new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(size)) })).multiply(max_val));
            max_val = max_val.multiply(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.SIZE_CARDINALITY_LEGACY)) }));
            int player_race = race_pulldown_menus[0].getChosenItemIndex();
            result = result.add((new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(player_race)) })).multiply(max_val));
            max_val = max_val.multiply(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.RACE_CARDINALITY)) }));
            int player_team = team_pulldown_menus[0].getChosenItemIndex();
            result = result.add((new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(player_team)) })).multiply(max_val));
            max_val = max_val.multiply(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.TEAM_CARDINALITY)) }));
            for (int i = 1; i < com.oddlabs.tt.form.TerrainMenu.DEFAULT_PLAYER_COUNT; i++)
            {
                int difficulty = difficulty_pulldown_menus[i].getChosenItemIndex();
                result = result.add((new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(difficulty)) })).multiply(max_val));
                max_val = max_val.multiply(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.DIFFICULTY_CARDINALITY)) }));
                int race = race_pulldown_menus[i].getChosenItemIndex();
                result = result.add((new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(race)) })).multiply(max_val));
                max_val = max_val.multiply(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.RACE_CARDINALITY)) }));
                int team = team_pulldown_menus[i].getChosenItemIndex();
                result = result.add((new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(team)) })).multiply(max_val));
                max_val = max_val.multiply(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.TEAM_CARDINALITY)) }));
            }
            string code = com.oddlabs.registration.RegistrationKey.createString(result);
            label_mapcode.clear();
            label_mapcode.append(code);
            markModified();
        }
        // Java: 583
        public void parseMapcode(string text)
        {
            string trimmed = text.trim();
            if (trimmed.indexOf(' ') == (-1))
            {
                try
                {
                    string code = trimmed.toUpperCase();
                    java.math.BigInteger result = com.oddlabs.registration.RegistrationKey.parseBits(code);
                    parseBigIntegerLegacy(result);
                    setMapcode();
                }
                catch (com.oddlabs.registration.RegistrationKeyFormatException unused)
                {
                    parseText(trimmed);
                }
            }
            else
            {
                try
                {
                    java.math.BigInteger result_2 = com.oddlabs.tt.util.WordsEncoding.decode(trimmed);
                    parseBigInteger(result_2);
                    label_mapcode.clear();
                    label_mapcode.append(trimmed);
                }
                catch (System.ArgumentException unused_2)
                {
                    parseText(trimmed);
                }
            }
        }
        // Java: 613
        public void parseText(string text)
        {
            string normalized = text.replaceAll("\\s+", " ").toLowerCase(java.util.Locale.ROOT);
            java.util.Random random = new java.util.Random(java.lang.JO.hashCode(normalized));
            pulldown_size.chooseItem(random.nextInt(pulldown_size.getSize()));
            pm_terrain_type.chooseItem(random.nextInt(pm_terrain_type.getSize()));
            slider_supplies.setValue(random.nextInt(com.oddlabs.tt.form.TerrainMenu.SLIDER_CARDINALITY));
            slider_vegetation.setValue(random.nextInt(com.oddlabs.tt.form.TerrainMenu.SLIDER_CARDINALITY));
            slider_hills.setValue(random.nextInt(com.oddlabs.tt.form.TerrainMenu.SLIDER_CARDINALITY));
            seed = random.nextInt(java.lang.Integer.parseInt(com.oddlabs.tt.form.TerrainMenu.SEED_CARDINALITY));
            setMapcode();
        }
        // Java: 625
        public void parseBigInteger(java.math.BigInteger result)
        {
            java.math.BigInteger max_val = com.oddlabs.tt.form.TerrainMenu.MAX_VALUE;
            result = result.mod(max_val);
            max_val = max_val.divide(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.SIZE_CARDINALITY)) }));
            int size = result.divide(max_val).intValue();
            if (pulldown_size.getSize() > size)
            {
                pulldown_size.chooseItem(size);
            }
            result = result.mod(max_val);
            max_val = max_val.divide(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.TERRAIN_TYPE_CARDINALITY)) }));
            int terrain_type = result.divide(max_val).intValue();
            if (pm_terrain_type.getSize() > terrain_type)
            {
                pm_terrain_type.chooseItem(terrain_type);
            }
            result = result.mod(max_val);
            max_val = max_val.divide(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.SLIDER_CARDINALITY)) }));
            int supplies_amount = result.divide(max_val).intValue();
            slider_supplies.setValue(supplies_amount);
            result = result.mod(max_val);
            max_val = max_val.divide(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.SLIDER_CARDINALITY)) }));
            int vegetation_amount = result.divide(max_val).intValue();
            slider_vegetation.setValue(vegetation_amount);
            result = result.mod(max_val);
            max_val = max_val.divide(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.SLIDER_CARDINALITY)) }));
            int hills = result.divide(max_val).intValue();
            slider_hills.setValue(hills);
            result = result.mod(max_val);
            max_val = max_val.divide(new java.math.BigInteger(com.oddlabs.tt.form.TerrainMenu.SEED_CARDINALITY));
            seed = result.divide(max_val).intValue();
        }
        // Java: 656
        public void parseBigIntegerLegacy(java.math.BigInteger result)
        {
            java.math.BigInteger max_val = com.oddlabs.tt.form.TerrainMenu.MAX_VALUE_LEGACY;
            for (int i = com.oddlabs.tt.form.TerrainMenu.DEFAULT_PLAYER_COUNT - 1; i >= 1; i--)
            {
                result = result.mod(max_val);
                max_val = max_val.divide(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.TEAM_CARDINALITY)) }));
                int team = result.divide(max_val).intValue();
                team_pulldown_menus[i].chooseItem(team);
                result = result.mod(max_val);
                max_val = max_val.divide(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.RACE_CARDINALITY)) }));
                int race = result.divide(max_val).intValue();
                race_pulldown_menus[i].chooseItem(race);
                result = result.mod(max_val);
                max_val = max_val.divide(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.DIFFICULTY_CARDINALITY)) }));
                int difficulty = result.divide(max_val).intValue();
                difficulty_pulldown_menus[i].chooseItem(difficulty);
                if (difficulty == 0)
                {
                    labels_players[i].setDisabled(true);
                    race_pulldown_buttons[i].setDisabled(true);
                    team_pulldown_buttons[i].setDisabled(true);
                }
                else
                {
                    labels_players[i].setDisabled(false);
                    race_pulldown_buttons[i].setDisabled(false);
                    team_pulldown_buttons[i].setDisabled(false);
                }
            }
            result = result.mod(max_val);
            max_val = max_val.divide(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.TEAM_CARDINALITY)) }));
            int player_team = result.divide(max_val).intValue();
            team_pulldown_menus[0].chooseItem(player_team);
            result = result.mod(max_val);
            max_val = max_val.divide(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.RACE_CARDINALITY)) }));
            int player_race = result.divide(max_val).intValue();
            race_pulldown_menus[0].chooseItem(player_race);
            result = result.mod(max_val);
            max_val = max_val.divide(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.SIZE_CARDINALITY_LEGACY)) }));
            int size = result.divide(max_val).intValue();
            pulldown_size.chooseItem(size);
            result = result.mod(max_val);
            max_val = max_val.divide(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.TERRAIN_TYPE_CARDINALITY_LEGACY)) }));
            int terrain_type = result.divide(max_val).intValue();
            pm_terrain_type.chooseItem(terrain_type);
            result = result.mod(max_val);
            max_val = max_val.divide(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.SLIDER_CARDINALITY)) }));
            int supplies_amount = result.divide(max_val).intValue();
            slider_supplies.setValue(supplies_amount);
            result = result.mod(max_val);
            max_val = max_val.divide(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.SLIDER_CARDINALITY)) }));
            int vegetation_amount = result.divide(max_val).intValue();
            slider_vegetation.setValue(vegetation_amount);
            result = result.mod(max_val);
            max_val = max_val.divide(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.SLIDER_CARDINALITY)) }));
            int hills = result.divide(max_val).intValue();
            slider_hills.setValue(hills);
            result = result.mod(max_val);
            max_val = max_val.divide(new java.math.BigInteger(com.oddlabs.tt.form.TerrainMenu.SEED_CARDINALITY));
            seed = result.divide(max_val).intValue();
        }
        // Java: 716
        public void setSeed(int seed)
        {
            this.seed = seed;
        }
        // Java: 720
        public com.oddlabs.tt.gui.GUIObject getButtonOK()
        {
            return button_ok;
        }
        // Java: 724
        public class CancelButtonListener : java.lang.Object, com.oddlabs.tt.guievent.MouseClickListener
        {
            public com.oddlabs.tt.form.TerrainMenu this_0;
            // Java: 725
            public void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
            {
                this_0.owner.terrainMenuCancel();
            }
            public CancelButtonListener(com.oddlabs.tt.form.TerrainMenu this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        // Java: 731
        public com.oddlabs.tt.gui.ScrollableGroup buildPlayerSlots(int count)
        {
            com.oddlabs.tt.gui.ScrollableGroup inner = new com.oddlabs.tt.gui.ScrollableGroup(200, 64);
            java.util.Random random = new java.util.Random(com.oddlabs.tt.@event.LocalEventQueue.getQueue().getHighPrecisionManager().getTick() * ((long)(com.oddlabs.tt.@event.LocalEventQueue.getQueue().getHighPrecisionManager().getTick())));
            random.nextFloat();
            for (int i = 0; i < count; i++)
            {
                difficulty_pulldown_menus[i] = new com.oddlabs.tt.gui.PulldownMenu();
                race_pulldown_menus[i] = new com.oddlabs.tt.gui.PulldownMenu();
                team_pulldown_menus[i] = new com.oddlabs.tt.gui.ScrollablePulldownMenu(com.oddlabs.tt.form.TerrainMenu.DEFAULT_PLAYER_COUNT);
                if (i == 0)
                {
                    difficulty_pulldown_menus[i].addItem(new com.oddlabs.tt.gui.PulldownItem(i18n("human")));
                }
                else
                {
                    if (multiplayer)
                    {
                        difficulty_pulldown_menus[i].addItem(new com.oddlabs.tt.gui.PulldownItem(i18n("open")));
                    }
                    difficulty_pulldown_menus[i].addItem(new com.oddlabs.tt.gui.PulldownItem(i18n("closed")));
                    difficulty_pulldown_menus[i].addItem(new com.oddlabs.tt.gui.PulldownItem(i18n("easy_ai")));
                    difficulty_pulldown_menus[i].addItem(new com.oddlabs.tt.gui.PulldownItem(i18n("normal_ai")));
                    difficulty_pulldown_menus[i].addItem(new com.oddlabs.tt.gui.PulldownItem(i18n("hard_ai")));
                }
                difficulty_pulldown_buttons[i] = new com.oddlabs.tt.gui.PulldownButton(gui_root, difficulty_pulldown_menus[i], 0, 115);
                inner.addChild(difficulty_pulldown_buttons[i]);
                for (int j = 0; j < com.oddlabs.tt.model.RacesResources.getNumRaces(); j++)
                {
                    race_pulldown_menus[i].addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.model.RacesResources.getRaceName(j)));
                }
                race_pulldown_buttons[i] = new com.oddlabs.tt.gui.PulldownButton(gui_root, race_pulldown_menus[i], 0, 115);
                inner.addChild(race_pulldown_buttons[i]);
                for (int j_2 = 0; j_2 < count; j_2++)
                {
                    team_pulldown_menus[i].addItem(new com.oddlabs.tt.gui.PulldownItem(i18n("team", java.lang.Integer.toString(j_2 + 1))));
                }
                team_pulldown_buttons[i] = new com.oddlabs.tt.gui.PulldownButton(gui_root, team_pulldown_menus[i], java.lang.Math.min(i, count - 1), 115);
                inner.addChild(team_pulldown_buttons[i]);
                if (i == 0)
                {
                    labels_players[0] = new com.oddlabs.tt.gui.Label(i18n("player", java.lang.Integer.toString(1)), com.oddlabs.tt.gui.Skin.getSkin().getEditFont()).setColor(com.oddlabs.tt.global.Settings.getSettings().team_colours[0]);
                    inner.addChild(labels_players[0]);
                    labels_players[0].place();
                    difficulty_pulldown_buttons[0].place(labels_players[0], com.oddlabs.tt.gui.Placement.RIGHT_MID);
                    race_pulldown_buttons[0].place(difficulty_pulldown_buttons[0], com.oddlabs.tt.gui.Placement.RIGHT_MID);
                    team_pulldown_buttons[0].place(race_pulldown_buttons[0], com.oddlabs.tt.gui.Placement.RIGHT_MID);
                }
                else
                {
                    labels_players[i] = new com.oddlabs.tt.gui.Label(i18n("player", java.lang.Integer.toString(i + 1)), com.oddlabs.tt.gui.Skin.getSkin().getEditFont()).setColor(com.oddlabs.tt.global.Settings.getSettings().team_colours[i]);
                    inner.addChild(labels_players[i]);
                    labels_players[i].place(labels_players[i - 1], com.oddlabs.tt.gui.Placement.BOTTOM_RIGHT);
                    difficulty_pulldown_buttons[i].place(labels_players[i], com.oddlabs.tt.gui.Placement.RIGHT_MID);
                    race_pulldown_buttons[i].place(difficulty_pulldown_buttons[i], com.oddlabs.tt.gui.Placement.RIGHT_MID);
                    team_pulldown_buttons[i].place(race_pulldown_buttons[i], com.oddlabs.tt.gui.Placement.RIGHT_MID);
                    difficulty_pulldown_menus[i].addItemChosenListener(new com.oddlabs.tt.form.TerrainMenu.DisableListener(this, i));
                }
                difficulty_pulldown_menus[i].addItemChosenListener(new com.oddlabs.tt.form.TerrainMenu.PulldownUpdateMapcodeListener(this));
                race_pulldown_menus[i].addItemChosenListener(new com.oddlabs.tt.form.TerrainMenu.PulldownUpdateMapcodeListener(this));
                team_pulldown_menus[i].addItemChosenListener(new com.oddlabs.tt.form.TerrainMenu.PulldownUpdateMapcodeListener(this));
            }
            inner.compileCanvas();
            return inner;
        }
        // Java: 797
        public void randomize()
        {
            java.util.Random random = new java.util.Random(com.oddlabs.tt.@event.LocalEventQueue.getQueue().getHighPrecisionManager().getTick() * ((long)(com.oddlabs.tt.@event.LocalEventQueue.getQueue().getHighPrecisionManager().getTick())));
            random.nextInt();
            java.math.BigInteger rand_int = new java.math.BigInteger(100, random);
            parseBigIntegerLegacy(rand_int);
            setMapcode();
        }
        // Java: 806
        public void doCancel()
        {
            if (multiplayer)
            {
                new com.oddlabs.tt.form.SelectGameMenu(network, gui_root, main_menu);
            }
        }
        // Java: 815
        public int defaultTeam(int i)
        {
            if (multiplayer)
            {
                return i;
            }
            return (i == 0 ? 0 : 1);
        }
        // Java: 822
        public bool isNight()
        {
            return pm_time_of_day.getChosenItemIndex() == com.oddlabs.tt.form.TerrainMenu.NIGHT;
        }
        // Java: 826
        public bool isChosen(com.oddlabs.tt.gui.PulldownMenu menu)
        {
            return menu.getChosenItemIndex() != 0;
        }
        // Java: 830
        public bool startGame()
        {
            int hills = slider_hills.getValue();
            int vegetation_amount = slider_vegetation.getValue();
            int supplies_amount = slider_supplies.getValue();
            com.oddlabs.tt.procedural.Landscape.TerrainType terrain_type = com.oddlabs.tt.procedural.Landscape.TerrainType.values()[pm_terrain_type.getChosenItemIndex()];
            com.oddlabs.matchmaking.Game game = default;
            bool rated = cb_rated.isMarked();
            if (rated)
            {
                team_pulldown_menus[0].chooseItem(team_pulldown_menus[0].getChosenItemIndex() % 2);
            }
            com.oddlabs.tt.form.AdvancedSettingsForm.Values settings = (rated ? com.oddlabs.tt.form.AdvancedSettingsForm.Values.defaults() : advanced_settings);
            int size = pulldown_size.getChosenItemIndex();
            bool ships = com.oddlabs.tt.global.Globals.SHIPS_ENABLED && (settings.ships() || com.oddlabs.tt.form.TerrainMenu.ARCHIPELAGO[size]);
            bool night = isNight();
            if (multiplayer)
            {
                string game_name = editline_name.getContents();
                if (game_name.length() < com.oddlabs.matchmaking.Game.MIN_LENGTH)
                {
                    string min_name = i18n("min_name_length", com.oddlabs.matchmaking.Game.MIN_LENGTH);
                    gui_root.addModalForm(new com.oddlabs.tt.form.MessageForm(min_name));
                    return false;
                }
                float random_start_pos = (float)(com.oddlabs.tt.@event.LocalEventQueue.getQueue().getTime() % 1.0f);
                game = com.oddlabs.matchmaking.Game.builder().name(game_name).size(unchecked((sbyte)(size))).terrain(unchecked((sbyte)(terrain_type.ordinal()))).hills(unchecked((sbyte)(hills))).trees(unchecked((sbyte)(vegetation_amount))).supplies(unchecked((sbyte)(supplies_amount))).rated(rated).gamespeed(unchecked((sbyte)((pm_gamespeed.getChosenItemIndex() + 1)))).mapcode(label_mapcode.getContents()).randomStartPos(random_start_pos).maxUnitCount(settings.maxUnits()).initialUnitCount(settings.startingUnits()).maxBuildingCount(settings.maxBuildings()).ships(ships).night(night).build();
            }
            else
            {
                bool has_enemy = false;
                for (int i = 1; i < player_count; i++)
                {
                    if (isChosen(difficulty_pulldown_menus[i]) && team_pulldown_menus[i].getChosenItemIndex() != team_pulldown_menus[0].getChosenItemIndex())
                    {
                        has_enemy = true;
                        break;
                    }
                }
                if (!has_enemy)
                {
                    string min_name_2 = i18n("min_num_teams", 2);
                    gui_root.addModalForm(new com.oddlabs.tt.form.MessageForm(min_name_2));
                    return false;
                }
                game = null;
            }
            if (owner != null)
            {
                owner.terrainMenuOK();
            }
            com.oddlabs.tt.form.SelectGameMenu menu = null;
            if (multiplayer)
            {
                menu = ((com.oddlabs.tt.form.SelectGameMenu)owner);
            }
            int gametype = default;
            java.lang.IO.println("hills = " + java.lang.JStr.valueOf((float)(hills / ((float)(com.oddlabs.tt.form.TerrainMenu.SLIDER_MAX_VALUE)))) + " | vegetation_amount = " + java.lang.JStr.valueOf((float)(vegetation_amount / ((float)(com.oddlabs.tt.form.TerrainMenu.SLIDER_MAX_VALUE)))) + " | supplies_amount = " + java.lang.JStr.valueOf((float)(supplies_amount / ((float)(com.oddlabs.tt.form.TerrainMenu.SLIDER_MAX_VALUE)))) + " | seed = " + (seed * seed));
            string ai_string = i18n("ai");
            string[] ai_names = new string[com.oddlabs.matchmaking.MatchmakingServerInterface.MAX_PLAYERS];
            for (int i_2 = 0; i_2 < ai_names.Length; i_2++)
            {
                ai_names[i_2] = java.lang.JStr.valueOf(ai_string) + (i_2);
            }
            com.oddlabs.tt.viewer.InGameInfo ingame_info = (multiplayer ? (com.oddlabs.tt.viewer.InGameInfo)(new com.oddlabs.tt.viewer.MultiplayerInGameInfo(game.getRandomStartPos(), game.isRated())) : (com.oddlabs.tt.viewer.InGameInfo)(new com.oddlabs.tt.viewer.DefaultInGameInfo(snapshotRoster())));
            com.oddlabs.tt.landscape.WorldParameters world_params = com.oddlabs.tt.landscape.WorldParameters.builder().initialGameSpeed((multiplayer ? game.getGamespeed() : com.oddlabs.tt.global.Globals.gamespeed)).mapcode(label_mapcode.getContents()).initialUnitCount(settings.startingUnits()).maxUnitCount(settings.maxUnits()).mapSize(size).maxBuildingCount(settings.maxBuildings()).ships(ships).night(night).build();
            com.oddlabs.tt.net.GameNetwork game_network = com.oddlabs.tt.@delegate.Menu.startNewGame(network, gui_root, menu, world_params, ingame_info, new com.oddlabs.tt.@delegate.Menu.DefaultWorldInitAction(), game, com.oddlabs.tt.form.TerrainMenu.SIZES[size], terrain_type, (float)(hills / ((float)(com.oddlabs.tt.form.TerrainMenu.SLIDER_MAX_VALUE))), (float)(vegetation_amount / ((float)(com.oddlabs.tt.form.TerrainMenu.SLIDER_MAX_VALUE))), (float)(supplies_amount / ((float)(com.oddlabs.tt.form.TerrainMenu.SLIDER_MAX_VALUE))), seed * seed, com.oddlabs.tt.form.TerrainMenu.ARCHIPELAGO[size] && com.oddlabs.tt.global.Globals.SHIPS_ENABLED, ai_names, player_count);
            game_network.getClient().getServerInterface().setPlayerSlot(0, com.oddlabs.tt.net.PlayerSlot.HUMAN, race_pulldown_menus[0].getChosenItemIndex(), team_pulldown_menus[0].getChosenItemIndex(), !multiplayer, com.oddlabs.tt.net.PlayerSlot.AI_NONE);
            if (multiplayer)
            {
                game_network.setInitialRoster(snapshotRoster());
            }
            if (!multiplayer)
            {
                for (int i_3 = 1; i_3 < player_count; i_3++)
                {
                    if (isChosen(difficulty_pulldown_menus[i_3]))
                    {
                        game_network.getClient().getServerInterface().setPlayerSlot(i_3, com.oddlabs.tt.net.PlayerSlot.AI, race_pulldown_menus[i_3].getChosenItemIndex(), team_pulldown_menus[i_3].getChosenItemIndex(), true, difficulty_pulldown_menus[i_3].getChosenItemIndex());
                    }
                }
                game_network.getClient().getServerInterface().startServer();
                java.lang.IO.println("Start server");
            }
            java.lang.IO.println("Map code: " + java.lang.JStr.valueOf(label_mapcode.getContents()));
            return true;
        }
        // Java: 950
        public class MapcodeListener : java.lang.Object, com.oddlabs.tt.guievent.MouseClickListener
        {
            public com.oddlabs.tt.form.TerrainMenu this_0;
            // Java: 951
            public void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
            {
                this_0.gui_root.addModalForm(new com.oddlabs.tt.form.MapcodeForm(this.this_0));
            }
            public MapcodeListener(com.oddlabs.tt.form.TerrainMenu this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        // Java: 957
        public class OKListener : java.lang.Object, com.oddlabs.tt.guievent.MouseClickListener
        {
            public com.oddlabs.tt.form.TerrainMenu this_0;
            // Java: 958
            public void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
            {
                bool started = this_0.startGame();
                if (started)
                {
                    this_0.button_ok.setDisabled(true);
                }
            }
            public OKListener(com.oddlabs.tt.form.TerrainMenu this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        // Java: 966
        public class DisableListener : java.lang.Object, com.oddlabs.tt.guievent.ItemChosenListener
        {
            public com.oddlabs.tt.form.TerrainMenu this_0;
            public int i;
            // Java: 973
            public void itemChosen(com.oddlabs.tt.gui.PulldownMenu menu, int item_index)
            {
                int closed_index = (this_0.multiplayer ? 1 : 0);
                bool closed = item_index == closed_index;
                this_0.labels_players[i].setDisabled(closed);
                this_0.race_pulldown_buttons[i].setDisabled(closed);
                this_0.team_pulldown_buttons[i].setDisabled(closed);
            }
            public DisableListener(com.oddlabs.tt.form.TerrainMenu this_0_, int i) : base()
            {
                this.this_0 = this_0_;
                this.i = i;
            }
        }
        // Java: 985
        public class PulldownUpdateMapcodeListener : java.lang.Object, com.oddlabs.tt.guievent.ItemChosenListener
        {
            public com.oddlabs.tt.form.TerrainMenu this_0;
            // Java: 986
            public void itemChosen(com.oddlabs.tt.gui.PulldownMenu menu, int item_index)
            {
                this_0.setMapcode();
            }
            public PulldownUpdateMapcodeListener(com.oddlabs.tt.form.TerrainMenu this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        // Java: 992
        public class PulldownUpdateSizeListener : java.lang.Object, com.oddlabs.tt.guievent.ItemChosenListener
        {
            // Java: 993
            public void itemChosen(com.oddlabs.tt.gui.PulldownMenu menu, int item_index)
            {
            }
            public PulldownUpdateSizeListener() : base()
            {
            }
        }
        // Java: 998
        public class PulldownUpdateHardListener : java.lang.Object, com.oddlabs.tt.guievent.ItemChosenListener
        {
            // Java: 999
            public void itemChosen(com.oddlabs.tt.gui.PulldownMenu menu, int item_index)
            {
            }
            public PulldownUpdateHardListener() : base()
            {
            }
        }
        // Java: 1004
        public class PulldownUpdateTerrainListener : java.lang.Object, com.oddlabs.tt.guievent.ItemChosenListener
        {
            // Java: 1005
            public void itemChosen(com.oddlabs.tt.gui.PulldownMenu menu, int item_index)
            {
            }
            public PulldownUpdateTerrainListener() : base()
            {
            }
        }
        // Java: 1010
        public class SliderUpdateMapcodeListener : java.lang.Object, com.oddlabs.tt.guievent.ValueListener
        {
            public com.oddlabs.tt.form.TerrainMenu this_0;
            // Java: 1011
            public void valueSet(long @value)
            {
                this_0.setMapcode();
            }
            public SliderUpdateMapcodeListener(com.oddlabs.tt.form.TerrainMenu this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        // Java: 1017
        public class PresetsHandler : java.lang.Object, com.oddlabs.tt.form.ModeAndPresetsHandler
        {
            public com.oddlabs.tt.form.TerrainMenu this_0;
            // Java: 1018
            public void modeChosen(com.oddlabs.matchmaking.GameMode mode)
            {
            }
            // Java: 1022
            public void presetChosen(com.oddlabs.matchmaking.Preset preset)
            {
                if (this_0.modified)
                {
                    this_0.gui_root.addModalForm(new com.oddlabs.tt.form.QuestionForm(this_0.i18n("confirm_discard_changes"), new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
                    {
                        applySelectedPreset(preset);
                    }), new java.lang.Runnable_L(() => { this.revertSelection(); })));
                }
                else
                {
                    applySelectedPreset(preset);
                }
            }
            // Java: 1034
            public void applySelectedPreset(com.oddlabs.matchmaking.Preset preset)
            {
                this_0.current_preset = preset;
                this_0.apply_in_progress = true;
                this_0.applyPreset(preset);
                this_0.apply_in_progress = false;
                this_0.modified = false;
                this_0.updateBanner();
            }
            // Java: 1043
            public void revertSelection()
            {
                if (this_0.mode_and_presets != null)
                {
                    this_0.mode_and_presets.refreshPresets();
                }
            }
            // Java: 1049
            public void presetDeleted(com.oddlabs.matchmaking.Preset preset)
            {
                this_0.gui_root.addModalForm(new com.oddlabs.tt.form.QuestionForm(this_0.i18n("confirm_delete_preset", preset.getName()), new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
                {
                    confirmDeletePreset(preset);
                })));
            }
            // Java: 1055
            public void confirmDeletePreset(com.oddlabs.matchmaking.Preset preset)
            {
                this_0.preset_library.remove(preset);
                this_0.preset_library.save(com.oddlabs.tt.render.Renderer.getLocalInput().getGameDir().resolve(com.oddlabs.tt.global.Globals.getPresetsFileName()));
                if (this_0.current_preset != null && java.lang.JO.equals(this_0.current_preset.getId(), preset.getId()))
                {
                    this_0.current_preset = null;
                    this_0.modified = false;
                    this_0.updateBanner();
                }
                if (this_0.mode_and_presets != null)
                {
                    this_0.mode_and_presets.refreshPresets();
                }
            }
            // Java: 1068
            public void saveClicked()
            {
                this_0.gui_root.addModalForm(new com.oddlabs.tt.form.SavePresetDialog(new java.util.function.Predicate_L((object a0) => { return this_0.preset_library.hasName(((string)a0)); }), new com.oddlabs.tt.form.SavePresetDialog.SaveListener_L((string a0) => { this.saveNewPreset(a0); })));
            }
            // Java: 1073
            public void resetClicked()
            {
                if (this_0.current_preset == null)
                {
                    return;
                }
                this_0.apply_in_progress = true;
                this_0.applyPreset(this_0.current_preset);
                this_0.apply_in_progress = false;
                this_0.modified = false;
                this_0.updateBanner();
            }
            // Java: 1085
            public void updateClicked()
            {
                if (this_0.current_preset == null)
                {
                    return;
                }
                com.oddlabs.matchmaking.Preset updated = new com.oddlabs.matchmaking.Preset(this_0.current_preset.getId(), this_0.current_preset.getName(), this_0.snapshotWorldConfig(), this_0.snapshotModeOptions(), this_0.snapshotRoster(), false);
                this_0.preset_library.remove(this_0.current_preset);
                this_0.preset_library.add(updated);
                this_0.preset_library.save(com.oddlabs.tt.render.Renderer.getLocalInput().getGameDir().resolve(com.oddlabs.tt.global.Globals.getPresetsFileName()));
                this_0.current_preset = updated;
                this_0.modified = false;
                this_0.updateBanner();
                if (this_0.mode_and_presets != null)
                {
                    this_0.mode_and_presets.refreshPresets();
                }
            }
            // Java: 1103
            public void saveNewPreset(string name)
            {
                com.oddlabs.matchmaking.Preset preset = new com.oddlabs.matchmaking.Preset(java.lang.JO.toString(java.util.UUID.randomUUID()), name, this_0.snapshotWorldConfig(), this_0.snapshotModeOptions(), this_0.snapshotRoster(), false);
                this_0.preset_library.add(preset);
                this_0.preset_library.save(com.oddlabs.tt.render.Renderer.getLocalInput().getGameDir().resolve(com.oddlabs.tt.global.Globals.getPresetsFileName()));
                this_0.current_preset = preset;
                this_0.modified = false;
                this_0.updateBanner();
                if (this_0.mode_and_presets != null)
                {
                    this_0.mode_and_presets.refreshPresets();
                }
            }
            public PresetsHandler(com.oddlabs.tt.form.TerrainMenu this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        // Java: 1118
        public com.oddlabs.matchmaking.StandardOptions snapshotModeOptions()
        {
            return com.oddlabs.matchmaking.StandardOptions.builder().rated(cb_rated.isMarked()).build();
        }
        // Java: 1122
        public com.oddlabs.matchmaking.WorldConfig snapshotWorldConfig()
        {
            return com.oddlabs.matchmaking.WorldConfig.builder().gamespeed(pm_gamespeed.getChosenItemIndex()).islandSize(pulldown_size.getChosenItemIndex()).terrainType(pm_terrain_type.getChosenItemIndex()).hills(slider_hills.getValue()).vegetation(slider_vegetation.getValue()).supplies(slider_supplies.getValue()).maxUnits(advanced_settings.maxUnits()).startingUnits(advanced_settings.startingUnits()).maxBuildings(advanced_settings.maxBuildings()).ships(advanced_settings.ships()).night(isNight()).build();
        }
        // Java: 1140
        public void applyWorldConfig(com.oddlabs.matchmaking.WorldConfig world)
        {
            pm_gamespeed.chooseItem(com.oddlabs.tt.form.TerrainMenu.clampIndex(world.getGamespeed(), pm_gamespeed.getSize()));
            pulldown_size.chooseItem(com.oddlabs.tt.form.TerrainMenu.clampIndex(world.getIslandSize(), pulldown_size.getSize()));
            pm_terrain_type.chooseItem(com.oddlabs.tt.form.TerrainMenu.clampIndex(world.getTerrainType(), pm_terrain_type.getSize()));
            pm_time_of_day.chooseItem((world.isNight() ? 1 : 0));
            slider_hills.setValue(world.getHills());
            slider_vegetation.setValue(world.getVegetation());
            slider_supplies.setValue(world.getSupplies());
            advanced_settings = new com.oddlabs.tt.form.AdvancedSettingsForm.Values(world.getMaxUnits(), world.getStartingUnits(), world.getMaxBuildings(), world.isShips());
            setMapcode();
        }
        // Java: 1156
        public static int clampIndex(int index, int size)
        {
            if (index < 0)
            {
                return 0;
            }
            return java.lang.Math.min(index, size - 1);
        }
        // Java: 1163
        public com.oddlabs.matchmaking.RosterTemplate snapshotRoster()
        {
            com.oddlabs.matchmaking.RosterTemplate.Slot[] slots = new com.oddlabs.matchmaking.RosterTemplate.Slot[player_count];
            for (int i = 0; i < player_count; i++)
            {
                int race = race_pulldown_menus[i].getChosenItemIndex();
                int team = team_pulldown_menus[i].getChosenItemIndex();
                slots[i] = new com.oddlabs.matchmaking.RosterTemplate.Slot(difficultyIndexToFill(i, difficulty_pulldown_menus[i].getChosenItemIndex()), race, team);
            }
            return new com.oddlabs.matchmaking.RosterTemplate(slots);
        }
        // Java: 1176
        public com.oddlabs.matchmaking.RosterTemplate.Fill difficultyIndexToFill(int slot_index, int difficulty_index)
        {
            if (slot_index == 0)
            {
                return com.oddlabs.matchmaking.RosterTemplate.Fill.HOST;
            }
            return (((multiplayer ? difficulty_index : difficulty_index + 1)) switch { 1 => (com.oddlabs.matchmaking.RosterTemplate.Fill)(com.oddlabs.matchmaking.RosterTemplate.Fill.CLOSED), 2 => (com.oddlabs.matchmaking.RosterTemplate.Fill)(com.oddlabs.matchmaking.RosterTemplate.Fill.EASY_AI), 3 => (com.oddlabs.matchmaking.RosterTemplate.Fill)(com.oddlabs.matchmaking.RosterTemplate.Fill.NORMAL_AI), 4 => (com.oddlabs.matchmaking.RosterTemplate.Fill)(com.oddlabs.matchmaking.RosterTemplate.Fill.HARD_AI), _ => (com.oddlabs.matchmaking.RosterTemplate.Fill)(com.oddlabs.matchmaking.RosterTemplate.Fill.OPEN) });
        }
        // Java: 1189
        public void applyPreset(com.oddlabs.matchmaking.Preset preset)
        {
            applyWorldConfig(preset.getWorld());
            applyRoster(preset.getRoster());
            if ((preset.getModeOptions() is com.oddlabs.matchmaking.StandardOptions opts))
            {
                cb_rated.setMarked(opts.isRated());
            }
        }
        // Java: 1197
        public void applyRoster(com.oddlabs.matchmaking.RosterTemplate roster)
        {
            com.oddlabs.matchmaking.RosterTemplate.Slot[] slots = roster.getSlots();
            int target_count = java.lang.Math.min(slots.Length, com.oddlabs.matchmaking.MatchmakingServerInterface.MAX_PLAYERS);
            if (target_count >= com.oddlabs.tt.form.TerrainMenu.DEFAULT_PLAYER_COUNT && target_count != player_count)
            {
                pulldown_menu_slots.chooseItem(target_count - com.oddlabs.tt.form.TerrainMenu.DEFAULT_PLAYER_COUNT);
            }
            for (int i = 0; i < java.lang.Math.min(slots.Length, player_count); i++)
            {
                com.oddlabs.matchmaking.RosterTemplate.Slot slot = slots[i];
                difficulty_pulldown_menus[i].chooseItem(fillToDifficultyIndex(slot.getFill(), i));
                if (slot.getRace() != null && ((int)slot.getRace()) < race_pulldown_menus[i].getSize())
                {
                    race_pulldown_menus[i].chooseItem(((int)slot.getRace()));
                }
                if (slot.getTeam() != null && ((int)slot.getTeam()) < team_pulldown_menus[i].getSize())
                {
                    team_pulldown_menus[i].chooseItem(((int)slot.getTeam()));
                }
            }
        }
        // Java: 1217
        public void applyWorldParameters(com.oddlabs.tt.landscape.WorldParameters @params)
        {
            pm_time_of_day.chooseItem((@params.isNight() ? com.oddlabs.tt.form.TerrainMenu.NIGHT : com.oddlabs.tt.form.TerrainMenu.DAY));
            applyAdvancedSettings(new com.oddlabs.tt.form.AdvancedSettingsForm.Values(@params.getMaxUnitCount(), @params.getInitialUnitCount(), @params.getMaxBuildingCount(), @params.isShipsEnabled()));
        }
        // Java: 1223
        public int fillToDifficultyIndex(com.oddlabs.matchmaking.RosterTemplate.Fill fill, int slot_index)
        {
            if (slot_index == 0)
            {
                return 0;
            }
            int index = ((fill).ordinal() switch { 0 /*HOST*/ or 1 /*OPEN*/ => (int)(0), 2 /*CLOSED*/ => (int)(1), 3 /*EASY_AI*/ => (int)(2), 4 /*NORMAL_AI*/ => (int)(3), 5 /*HARD_AI*/ => (int)(4), _ => throw new System.InvalidOperationException("MatchException") });
            return (multiplayer ? index : java.lang.Math.max(0, index - 1));
        }
        // Java: 1237
        public class PulldownUpdatePlayersChangedListener : java.lang.Object, com.oddlabs.tt.guievent.ItemChosenListener
        {
            public com.oddlabs.tt.form.TerrainMenu this_0;
            public com.oddlabs.tt.gui.Panel standard;
            public com.oddlabs.tt.gui.ScrollableGroup current_race_team;
            // Java: 1245
            public void setCurrentGroup(com.oddlabs.tt.gui.ScrollableGroup group)
            {
                this.current_race_team = group;
            }
            // Java: 1249
            public void itemChosen(com.oddlabs.tt.gui.PulldownMenu menu, int item_index)
            {
                int previous_count = this_0.player_count;
                this_0.player_count = item_index + com.oddlabs.tt.form.TerrainMenu.DEFAULT_PLAYER_COUNT;
                int[] prev_difficulty = new int[previous_count];
                int[] prev_race = new int[previous_count];
                int[] prev_team = new int[previous_count];
                for (int i = 0; i < previous_count; i++)
                {
                    prev_difficulty[i] = this_0.difficulty_pulldown_menus[i].getChosenItemIndex();
                    prev_race[i] = this_0.race_pulldown_menus[i].getChosenItemIndex();
                    prev_team[i] = this_0.team_pulldown_menus[i].getChosenItemIndex();
                }
                com.oddlabs.tt.gui.ScrollableGroup new_group = this_0.buildPlayerSlots(this_0.player_count);
                if (this_0.multiplayer)
                {
                    this_0.roster_panel.setRoster(new_group);
                }
                else
                {
                    if (current_race_team != null)
                    {
                        standard.removeChild(current_race_team);
                    }
                    new_group.place();
                    standard.addChild(new_group);
                }
                current_race_team = new_group;
                for (int i_2 = 0; i_2 < this_0.player_count; i_2++)
                {
                    if (i_2 < previous_count)
                    {
                        this_0.difficulty_pulldown_menus[i_2].chooseItem(prev_difficulty[i_2]);
                        this_0.race_pulldown_menus[i_2].chooseItem(prev_race[i_2]);
                        this_0.team_pulldown_menus[i_2].chooseItem(java.lang.Math.min(prev_team[i_2], this_0.player_count - 1));
                    }
                    else
                    {
                        this_0.team_pulldown_menus[i_2].chooseItem(this_0.defaultTeam(i_2));
                        if (!this_0.multiplayer && i_2 == 1)
                        {
                            this_0.difficulty_pulldown_menus[i_2].chooseItem(com.oddlabs.tt.net.PlayerSlot.AI_EASY);
                            this_0.race_pulldown_menus[i_2].chooseItem((this_0.race_pulldown_menus[0].getChosenItemIndex() + 1) % 2);
                        }
                        else if (i_2 != 0)
                        {
                            this_0.difficulty_pulldown_menus[i_2].chooseItem(0);
                        }
                    }
                }
                this_0.markModified();
            }
            public PulldownUpdatePlayersChangedListener(com.oddlabs.tt.form.TerrainMenu this_0_, com.oddlabs.tt.gui.Panel standard) : base()
            {
                this.this_0 = this_0_;
                this.standard = standard;
            }
        }
        static TerrainMenu()
        {
            TerrainMenu.SIZES = new int[] { 256, 512, 1024, 2048, 2048 };
            TerrainMenu.ARCHIPELAGO = new bool[] { false, false, false, false, true };
            TerrainMenu.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.form.TerrainMenu).getName());
            {
                java.math.BigInteger max = java.math.BigInteger.ONE;
                max = max.multiply(new java.math.BigInteger(com.oddlabs.tt.form.TerrainMenu.SEED_CARDINALITY));
                max = max.multiply(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.SLIDER_CARDINALITY)) }));
                max = max.multiply(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.SLIDER_CARDINALITY)) }));
                max = max.multiply(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.SLIDER_CARDINALITY)) }));
                max = max.multiply(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.TERRAIN_TYPE_CARDINALITY_LEGACY)) }));
                max = max.multiply(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.SIZE_CARDINALITY_LEGACY)) }));
                max = max.multiply(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.RACE_CARDINALITY)) }));
                max = max.multiply(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.TEAM_CARDINALITY)) }));
                for (int i = 1; i < com.oddlabs.tt.form.TerrainMenu.DEFAULT_PLAYER_COUNT; i++)
                {
                    max = max.multiply(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.DIFFICULTY_CARDINALITY)) }));
                    max = max.multiply(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.RACE_CARDINALITY)) }));
                    max = max.multiply(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.TEAM_CARDINALITY)) }));
                }
                com.oddlabs.tt.form.TerrainMenu.MAX_VALUE_LEGACY = max;
                max = java.math.BigInteger.ONE;
                max = max.multiply(new java.math.BigInteger(com.oddlabs.tt.form.TerrainMenu.SEED_CARDINALITY));
                max = max.multiply(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.SLIDER_CARDINALITY)) }));
                max = max.multiply(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.SLIDER_CARDINALITY)) }));
                max = max.multiply(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.SLIDER_CARDINALITY)) }));
                max = max.multiply(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.TERRAIN_TYPE_CARDINALITY)) }));
                max = max.multiply(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.SIZE_CARDINALITY)) }));
                com.oddlabs.tt.form.TerrainMenu.MAX_VALUE = max;
            }
        }
        private void init_TerrainMenu()
        {
            this.preset_library = new com.oddlabs.tt.gamemode.PresetLibrary();
            this.player_count = com.oddlabs.tt.form.TerrainMenu.DEFAULT_PLAYER_COUNT;
            this.advanced_settings = com.oddlabs.tt.form.AdvancedSettingsForm.Values.defaults();
        }
        public TerrainMenu(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.@delegate.Menu main_menu, bool multiplayer, com.oddlabs.tt.form.TerrainMenuListener owner) : base()
        {
            init_TerrainMenu();
            this.network = network;
            this.main_menu = main_menu;
            this.multiplayer = multiplayer;
            this.owner = owner;
            this.gui_root = gui_root;
            com.oddlabs.tt.gui.Label label_headline = new com.oddlabs.tt.gui.Label(i18n((multiplayer ? "new_game" : "skirmish")), com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont());
            addChild(label_headline);
            if (multiplayer)
            {
                preset_library.load(com.oddlabs.tt.render.Renderer.getLocalInput().getGameDir().resolve(com.oddlabs.tt.global.Globals.getPresetsFileName()));
            }
            mode_and_presets = (multiplayer ? new com.oddlabs.tt.form.ModeAndPresetsPanel(gui_root, preset_library, new com.oddlabs.tt.form.TerrainMenu.PresetsHandler(this)) : (com.oddlabs.tt.form.ModeAndPresetsPanel)(null));
            com.oddlabs.tt.gui.Panel standard = new com.oddlabs.tt.gui.Panel(i18n("standard_options"));
            com.oddlabs.tt.gui.Panel advanced = new com.oddlabs.tt.gui.Panel(i18n("advanced_options"));
            roster_panel = (multiplayer ? new com.oddlabs.tt.form.RosterPanel() : (com.oddlabs.tt.form.RosterPanel)(null));
            com.oddlabs.tt.gui.Group group_map_options = new com.oddlabs.tt.gui.Group();
            com.oddlabs.tt.gui.Label label_name = new com.oddlabs.tt.gui.Label(i18n("game_name"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            com.oddlabs.tt.gui.Label label_default_name = null;
            editline_name = new com.oddlabs.tt.gui.EditLine(180, com.oddlabs.matchmaking.Game.MAX_LENGTH);
            if (multiplayer)
            {
                standard.addChild(label_name);
                string default_name = i18n("default_name", com.oddlabs.tt.net.Network.getMatchmakingClient().getProfile().getNick());
                label_default_name = new com.oddlabs.tt.gui.Label(default_name, com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
                editline_name.append(default_name);
                if (com.oddlabs.tt.render.Renderer.isRegistered())
                {
                    standard.addChild(editline_name);
                }
                else
                {
                    standard.addChild(label_default_name);
                }
            }
            string rated_tip = i18n("rated_game_tip", com.oddlabs.matchmaking.GameSession.MIN_WINS_FOR_RANKING);
            cb_rated = new com.oddlabs.tt.gui.CheckBox(false, i18n("rated_game"), rated_tip);
            if (multiplayer)
            {
                standard.addChild(cb_rated);
                cb_rated.setDisabled(com.oddlabs.tt.net.Network.getMatchmakingClient().getProfile() == null || com.oddlabs.tt.net.Network.getMatchmakingClient().getProfile().getWins() < com.oddlabs.matchmaking.GameSession.MIN_WINS_FOR_RANKING);
            }
            com.oddlabs.tt.gui.Group group_gamespeed = new com.oddlabs.tt.gui.Group();
            com.oddlabs.tt.gui.Label label_gamespeed = new com.oddlabs.tt.gui.Label(i18n("gamespeed"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_gamespeed.addChild(label_gamespeed);
            pm_gamespeed = new com.oddlabs.tt.gui.PulldownMenu();
            pm_gamespeed.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.util.ServerMessageBundler.getGamespeedString(com.oddlabs.matchmaking.Game.GAMESPEED_SLOW)));
            pm_gamespeed.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.util.ServerMessageBundler.getGamespeedString(com.oddlabs.matchmaking.Game.GAMESPEED_NORMAL)));
            pm_gamespeed.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.util.ServerMessageBundler.getGamespeedString(com.oddlabs.matchmaking.Game.GAMESPEED_FAST)));
            pm_gamespeed.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.util.ServerMessageBundler.getGamespeedString(com.oddlabs.matchmaking.Game.GAMESPEED_LUDICROUS)));
            int gamespeed_index = java.lang.Math.clamp(com.oddlabs.tt.global.Globals.gamespeed - com.oddlabs.matchmaking.Game.GAMESPEED_SLOW, 0, com.oddlabs.matchmaking.Game.GAMESPEED_LUDICROUS - com.oddlabs.matchmaking.Game.GAMESPEED_SLOW);
            com.oddlabs.tt.gui.PulldownButton pb_gamespeed = new com.oddlabs.tt.gui.PulldownButton(gui_root, pm_gamespeed, gamespeed_index, 150);
            pm_gamespeed.addItemChosenListener(new com.oddlabs.tt.guievent.ItemChosenListener_L((com.oddlabs.tt.gui.PulldownMenu unused, int unused_2) =>
            {
                markModified();
            }));
            group_gamespeed.addChild(pb_gamespeed);
            label_gamespeed.place();
            pb_gamespeed.place(label_gamespeed, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            group_gamespeed.compileCanvas();
            if (multiplayer)
            {
                group_map_options.addChild(group_gamespeed);
            }
            com.oddlabs.tt.gui.Group group_size = new com.oddlabs.tt.gui.Group();
            com.oddlabs.tt.gui.Label label_size = new com.oddlabs.tt.gui.Label(i18n("island_size"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_size.addChild(label_size);
            pulldown_size = new com.oddlabs.tt.gui.PulldownMenu();
            pulldown_size.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.util.ServerMessageBundler.getSizeString(com.oddlabs.matchmaking.Game.SIZE_SMALL)));
            pulldown_size.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.util.ServerMessageBundler.getSizeString(com.oddlabs.matchmaking.Game.SIZE_MEDIUM)));
            pulldown_size.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.util.ServerMessageBundler.getSizeString(com.oddlabs.matchmaking.Game.SIZE_LARGE)));
            pulldown_size.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.util.ServerMessageBundler.getSizeString(com.oddlabs.matchmaking.Game.SIZE_ENORMOUS)));
            if (com.oddlabs.tt.global.Globals.SHIPS_ENABLED)
            {
                pulldown_size.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.util.ServerMessageBundler.getSizeString(com.oddlabs.matchmaking.Game.SIZE_ARCHIPELAGO)));
            }
            com.oddlabs.tt.gui.PulldownButton pb_size = new com.oddlabs.tt.gui.PulldownButton(gui_root, pulldown_size, 1, 150);
            group_size.addChild(pb_size);
            label_size.place();
            pb_size.place(label_size, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            group_size.compileCanvas();
            group_map_options.addChild(group_size);
            pulldown_size.addItemChosenListener(new com.oddlabs.tt.form.TerrainMenu.PulldownUpdateMapcodeListener(this));
            com.oddlabs.tt.gui.Label label_seed = new com.oddlabs.tt.gui.Label(i18n("map_code"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            label_mapcode = new com.oddlabs.tt.gui.Label("", com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont(), 500);
            com.oddlabs.tt.gui.Group group_seed = new com.oddlabs.tt.gui.Group();
            group_seed.addChild(label_seed);
            group_seed.addChild(label_mapcode);
            label_seed.place();
            label_mapcode.place(label_seed, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            group_seed.compileCanvas();
            advanced.addChild(group_seed);
            com.oddlabs.tt.gui.Group group_terrain_type = new com.oddlabs.tt.gui.Group();
            com.oddlabs.tt.gui.Label label_terrain_type = new com.oddlabs.tt.gui.Label(i18n("terrain_type"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_terrain_type.addChild(label_terrain_type);
            pm_terrain_type = new com.oddlabs.tt.gui.PulldownMenu();
            pm_terrain_type.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.util.ServerMessageBundler.getTerrainTypeString(com.oddlabs.matchmaking.Game.TERRAIN_TYPE_NATIVE)));
            pm_terrain_type.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.util.ServerMessageBundler.getTerrainTypeString(com.oddlabs.matchmaking.Game.TERRAIN_TYPE_VIKING)));
            com.oddlabs.tt.gui.PulldownButton pb_terrain_type = new com.oddlabs.tt.gui.PulldownButton(gui_root, pm_terrain_type, 0, 150);
            group_terrain_type.addChild(pb_terrain_type);
            label_terrain_type.place();
            pb_terrain_type.place(label_terrain_type, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            group_terrain_type.compileCanvas();
            pm_terrain_type.addItemChosenListener(new com.oddlabs.tt.form.TerrainMenu.PulldownUpdateMapcodeListener(this));
            group_map_options.addChild(group_terrain_type);
            com.oddlabs.tt.gui.Group group_time_of_day = new com.oddlabs.tt.gui.Group();
            com.oddlabs.tt.gui.Label label_time_of_day = new com.oddlabs.tt.gui.Label(i18n("time_of_day"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_time_of_day.addChild(label_time_of_day);
            pm_time_of_day = new com.oddlabs.tt.gui.PulldownMenu();
            pm_time_of_day.addItem(new com.oddlabs.tt.gui.PulldownItem(i18n("day")));
            pm_time_of_day.addItem(new com.oddlabs.tt.gui.PulldownItem(i18n("night")));
            com.oddlabs.tt.gui.PulldownButton pb_time_of_day = new com.oddlabs.tt.gui.PulldownButton(gui_root, pm_time_of_day, 0, 150);
            group_time_of_day.addChild(pb_time_of_day);
            label_time_of_day.place();
            pb_time_of_day.place(label_time_of_day, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            group_time_of_day.compileCanvas();
            pm_time_of_day.addItemChosenListener(new com.oddlabs.tt.guievent.ItemChosenListener_L((com.oddlabs.tt.gui.PulldownMenu unused_3, int unused_4) =>
            {
                markModified();
            }));
            group_map_options.addChild(group_time_of_day);
            com.oddlabs.tt.gui.Group group_sliders = new com.oddlabs.tt.gui.Group();
            com.oddlabs.tt.gui.Label label_hills_low = new com.oddlabs.tt.gui.Label(i18n("min"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_sliders.addChild(label_hills_low);
            com.oddlabs.tt.gui.Label label_hills_high = new com.oddlabs.tt.gui.Label(i18n("max"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_sliders.addChild(label_hills_high);
            com.oddlabs.tt.gui.Label label_hills = new com.oddlabs.tt.gui.Label(i18n("hills"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_sliders.addChild(label_hills);
            slider_hills = new com.oddlabs.tt.gui.Slider(com.oddlabs.tt.form.TerrainMenu.SLIDER_LENGTH, 0, com.oddlabs.tt.form.TerrainMenu.SLIDER_MAX_VALUE, com.oddlabs.tt.form.TerrainMenu.SLIDER_MAX_VALUE / 2);
            slider_hills.addValueListener(new com.oddlabs.tt.form.TerrainMenu.SliderUpdateMapcodeListener(this));
            group_sliders.addChild(slider_hills);
            com.oddlabs.tt.gui.Label label_vegetation_low = new com.oddlabs.tt.gui.Label(i18n("min"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_sliders.addChild(label_vegetation_low);
            com.oddlabs.tt.gui.Label label_vegetation_high = new com.oddlabs.tt.gui.Label(i18n("max"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_sliders.addChild(label_vegetation_high);
            com.oddlabs.tt.gui.Label label_vegetation = new com.oddlabs.tt.gui.Label(i18n("trees"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_sliders.addChild(label_vegetation);
            slider_vegetation = new com.oddlabs.tt.gui.Slider(com.oddlabs.tt.form.TerrainMenu.SLIDER_LENGTH, 0, com.oddlabs.tt.form.TerrainMenu.SLIDER_MAX_VALUE, com.oddlabs.tt.form.TerrainMenu.SLIDER_MAX_VALUE / 2);
            slider_vegetation.addValueListener(new com.oddlabs.tt.form.TerrainMenu.SliderUpdateMapcodeListener(this));
            group_sliders.addChild(slider_vegetation);
            com.oddlabs.tt.gui.Label label_supplies_low = new com.oddlabs.tt.gui.Label(i18n("min"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_sliders.addChild(label_supplies_low);
            com.oddlabs.tt.gui.Label label_supplies_high = new com.oddlabs.tt.gui.Label(i18n("max"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_sliders.addChild(label_supplies_high);
            com.oddlabs.tt.gui.Label label_supplies = new com.oddlabs.tt.gui.Label(i18n("resources"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_sliders.addChild(label_supplies);
            slider_supplies = new com.oddlabs.tt.gui.Slider(com.oddlabs.tt.form.TerrainMenu.SLIDER_LENGTH, 0, com.oddlabs.tt.form.TerrainMenu.SLIDER_MAX_VALUE, com.oddlabs.tt.form.TerrainMenu.SLIDER_MAX_VALUE / 2);
            slider_supplies.addValueListener(new com.oddlabs.tt.form.TerrainMenu.SliderUpdateMapcodeListener(this));
            group_sliders.addChild(slider_supplies);
            label_supplies.place();
            label_supplies_low.place(label_supplies, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            slider_supplies.place(label_supplies_low, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            label_supplies_high.place(slider_supplies, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            label_vegetation.place(label_supplies, com.oddlabs.tt.gui.Placement.TOP_LEFT, com.oddlabs.tt.gui.Skin.getSkin().getFormData().sectionSpacing());
            slider_vegetation.place(slider_supplies, com.oddlabs.tt.gui.Placement.TOP_MID, com.oddlabs.tt.gui.Skin.getSkin().getFormData().sectionSpacing());
            label_vegetation_low.place(slider_vegetation, com.oddlabs.tt.gui.Placement.LEFT_MID);
            label_vegetation_high.place(slider_vegetation, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            label_hills.place(label_vegetation, com.oddlabs.tt.gui.Placement.TOP_LEFT, com.oddlabs.tt.gui.Skin.getSkin().getFormData().sectionSpacing());
            slider_hills.place(slider_vegetation, com.oddlabs.tt.gui.Placement.TOP_MID, com.oddlabs.tt.gui.Skin.getSkin().getFormData().sectionSpacing());
            label_hills_low.place(slider_hills, com.oddlabs.tt.gui.Placement.LEFT_MID);
            label_hills_high.place(slider_hills, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            group_sliders.compileCanvas();
            advanced.addChild(group_sliders);
            com.oddlabs.tt.gui.Group group_num_players = new com.oddlabs.tt.gui.Group();
            com.oddlabs.tt.gui.Label label_player_slots = new com.oddlabs.tt.gui.Label(i18n("players"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_num_players.addChild(label_player_slots);
            pulldown_menu_slots = new com.oddlabs.tt.gui.ScrollablePulldownMenu(com.oddlabs.tt.form.TerrainMenu.DEFAULT_PLAYER_COUNT);
            for (int i = com.oddlabs.tt.form.TerrainMenu.DEFAULT_PLAYER_COUNT; i <= com.oddlabs.matchmaking.MatchmakingServerInterface.MAX_PLAYERS; i++)
            {
                pulldown_menu_slots.addItem(new com.oddlabs.tt.gui.PulldownItem(java.lang.Integer.toString(i)));
            }
            com.oddlabs.tt.gui.PulldownButton pulldown_player_slots = new com.oddlabs.tt.gui.PulldownButton(gui_root, pulldown_menu_slots, 0, 150);
            group_num_players.addChild(pulldown_player_slots);
            label_player_slots.place();
            pulldown_player_slots.place(label_player_slots, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            group_num_players.compileCanvas();
            advanced.addChild(group_num_players);
            labels_players = new com.oddlabs.tt.gui.Label[com.oddlabs.matchmaking.MatchmakingServerInterface.MAX_PLAYERS];
            difficulty_pulldown_menus = new com.oddlabs.tt.gui.PulldownMenu[com.oddlabs.matchmaking.MatchmakingServerInterface.MAX_PLAYERS];
            race_pulldown_menus = new com.oddlabs.tt.gui.PulldownMenu[com.oddlabs.matchmaking.MatchmakingServerInterface.MAX_PLAYERS];
            team_pulldown_menus = new com.oddlabs.tt.gui.PulldownMenu[com.oddlabs.matchmaking.MatchmakingServerInterface.MAX_PLAYERS];
            difficulty_pulldown_buttons = new com.oddlabs.tt.gui.PulldownButton[com.oddlabs.matchmaking.MatchmakingServerInterface.MAX_PLAYERS];
            race_pulldown_buttons = new com.oddlabs.tt.gui.PulldownButton[com.oddlabs.matchmaking.MatchmakingServerInterface.MAX_PLAYERS];
            team_pulldown_buttons = new com.oddlabs.tt.gui.PulldownButton[com.oddlabs.matchmaking.MatchmakingServerInterface.MAX_PLAYERS];
            com.oddlabs.tt.gui.ScrollableGroup group_race_team = buildPlayerSlots(player_count);
            if (multiplayer)
            {
                roster_panel.setRoster(group_race_team);
            }
            else
            {
                standard.addChild(group_race_team);
            }
            com.oddlabs.tt.gui.Group group_buttons = new com.oddlabs.tt.gui.Group();
            button_ok = new com.oddlabs.tt.gui.OKButton(com.oddlabs.tt.form.TerrainMenu.BUTTON_WIDTH);
            button_ok.addMouseClickListener(new com.oddlabs.tt.form.TerrainMenu.OKListener(this));
            com.oddlabs.tt.gui.HorizButton button_cancel = new com.oddlabs.tt.gui.CancelButton(com.oddlabs.tt.form.TerrainMenu.BUTTON_WIDTH);
            button_cancel.addMouseClickListener(new com.oddlabs.tt.form.TerrainMenu.CancelButtonListener(this));
            com.oddlabs.tt.gui.HorizButton button_mapcode = new com.oddlabs.tt.gui.HorizButton(i18n("enter_map_code"), 170);
            button_mapcode.addMouseClickListener(new com.oddlabs.tt.form.TerrainMenu.MapcodeListener(this));
            button_advanced = new com.oddlabs.tt.gui.HorizButton(i18n("advanced"), 130);
            button_advanced.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_5, int unused_6, int unused_7, int unused_8) =>
            {
                gui_root.addModalForm(new com.oddlabs.tt.form.AdvancedSettingsForm(advanced_settings, com.oddlabs.tt.global.Globals.SHIPS_ENABLED, com.oddlabs.tt.global.Globals.SHIPS_ENABLED && com.oddlabs.tt.form.TerrainMenu.ARCHIPELAGO[pulldown_size.getChosenItemIndex()], new com.oddlabs.tt.form.AdvancedSettingsForm.Listener_L((com.oddlabs.tt.form.AdvancedSettingsForm.Values a0) => { this.applyAdvancedSettings(a0); })));
            }));
            addChild(button_advanced);
            group_buttons.addChild(button_mapcode);
            group_buttons.addChild(button_ok);
            group_buttons.addChild(button_cancel);
            button_cancel.place();
            button_ok.place(button_cancel, com.oddlabs.tt.gui.Placement.LEFT_MID);
            button_mapcode.place(button_ok, com.oddlabs.tt.gui.Placement.LEFT_MID);
            group_buttons.compileCanvas();
            addChild(group_buttons);
            if (multiplayer)
            {
                group_gamespeed.place();
                group_size.place(group_gamespeed, com.oddlabs.tt.gui.Placement.BOTTOM_RIGHT);
            }
            else
            {
                group_size.place();
            }
            group_terrain_type.place(group_size, com.oddlabs.tt.gui.Placement.BOTTOM_RIGHT);
            group_time_of_day.place(group_terrain_type, com.oddlabs.tt.gui.Placement.BOTTOM_RIGHT);
            group_map_options.compileCanvas();
            standard.addChild(group_map_options);
            if (multiplayer)
            {
                label_name.place();
                if (com.oddlabs.tt.render.Renderer.isRegistered())
                {
                    editline_name.place(label_name, com.oddlabs.tt.gui.Placement.RIGHT_MID);
                }
                else
                {
                    label_default_name.place(label_name, com.oddlabs.tt.gui.Placement.RIGHT_MID);
                }
                cb_rated.place(label_name, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT, com.oddlabs.tt.gui.Skin.getSkin().getFormData().sectionSpacing());
                group_map_options.place(cb_rated, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            }
            else
            {
                group_map_options.place();
                group_race_team.place(group_map_options, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT, com.oddlabs.tt.gui.Skin.getSkin().getFormData().sectionSpacing());
            }
            standard.compileCanvas();
            group_sliders.place();
            group_num_players.place(group_sliders, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT, com.oddlabs.tt.gui.Skin.getSkin().getFormData().sectionSpacing());
            group_seed.place(group_num_players, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT, com.oddlabs.tt.gui.Skin.getSkin().getFormData().sectionSpacing());
            advanced.compileCanvas();
            com.oddlabs.tt.gui.PanelGroup panel_group = (multiplayer ? new com.oddlabs.tt.gui.PanelGroup(1, mode_and_presets, standard, advanced, roster_panel) : new com.oddlabs.tt.gui.PanelGroup(standard, advanced));
            addChild(panel_group);
            com.oddlabs.tt.form.TerrainMenu.PulldownUpdatePlayersChangedListener playersChangedListener = new com.oddlabs.tt.form.TerrainMenu.PulldownUpdatePlayersChangedListener(this, standard);
            playersChangedListener.setCurrentGroup(group_race_team);
            pulldown_menu_slots.addItemChosenListener(playersChangedListener);
            label_headline.place();
            panel_group.place(label_headline, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            button_advanced.place(panel_group, com.oddlabs.tt.gui.Placement.TOP_RIGHT);
            group_buttons.place(com.oddlabs.tt.gui.Origin.AT_END);
            compileCanvas();
            randomize();
            pulldown_size.addItemChosenListener(new com.oddlabs.tt.form.TerrainMenu.PulldownUpdateSizeListener());
            pm_terrain_type.addItemChosenListener(new com.oddlabs.tt.form.TerrainMenu.PulldownUpdateTerrainListener());
            for (int i_2 = 0; i_2 < player_count; i_2++)
            {
                difficulty_pulldown_menus[i_2].addItemChosenListener(new com.oddlabs.tt.form.TerrainMenu.PulldownUpdateHardListener());
                team_pulldown_menus[i_2].chooseItem(defaultTeam(i_2));
                if (!multiplayer && i_2 == 1)
                {
                    difficulty_pulldown_menus[i_2].chooseItem(com.oddlabs.tt.net.PlayerSlot.AI_EASY);
                    race_pulldown_menus[i_2].chooseItem((race_pulldown_menus[0].getChosenItemIndex() + 1) % 2);
                }
                else
                {
                    difficulty_pulldown_menus[i_2].chooseItem(0);
                }
            }
            pulldown_size.chooseItem(1);
            if (!com.oddlabs.tt.render.Renderer.isRegistered())
            {
                pm_terrain_type.chooseItem(0);
            }
            if (com.oddlabs.tt.model.GeometryRegistry.isEventActive(com.oddlabs.tt.model.GeometryRegistry.HALLOWEEN))
            {
                pm_time_of_day.chooseItem(com.oddlabs.tt.form.TerrainMenu.NIGHT);
            }
            cb_rated.addCheckBoxListener(new com.oddlabs.tt.guievent.CheckBoxListener_L((bool marked) =>
            {
                button_advanced.setDisabled(marked);
                markModified();
            }));
            initialized = true;
        }
    }
}
