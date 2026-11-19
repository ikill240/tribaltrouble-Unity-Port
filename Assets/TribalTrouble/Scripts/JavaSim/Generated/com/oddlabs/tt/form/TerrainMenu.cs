// Translated from TerrainMenu.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 81
    public class TerrainMenu : com.oddlabs.tt.gui.Group
    {
        public static int[] SIZES;
        public static bool[] ARCHIPELAGO;
        public com.oddlabs.tt.mapeditor.CustomMap selected_editor_map;
        public const int SLIDER_LENGTH = 250;
        public const int BUTTON_WIDTH = 100;
        public const int ROSTER_CONTENT_HEIGHT = 200;
        public const int MIN_ROSTER_CONTENT_HEIGHT = 80;
        public const int SLIDER_MAX_VALUE = 10;
        public const int MAP_SETTING_SLIDER_MAX = 20;
        public const int SETTING_SLIDER_LENGTH = 200;
        public const int VALUE_LABEL_WIDTH = 50;
        public const int ADVANCED_ROW_SPACING = 20;
        public const int UNIT_COUNT_STEP = 5;
        public const int MIN_INITIAL_UNITS = 5;
        public const int MAX_INITIAL_UNITS = 5000;
        public const int DEFAULT_INITIAL_UNITS = 20;
        public const int MIN_STARTING_WARRIORS = 0;
        public const int MAX_STARTING_WARRIORS = 300;
        public const int DEFAULT_STARTING_WARRIORS = 0;
        public const int MIN_MAGIC_COST = 5;
        public const int MAX_MAGIC_COST = 500;
        public const int MIN_HEALTH_MULT_PERCENT = 5;
        public const int MAX_HEALTH_MULT_PERCENT = 500;
        public const int DEFAULT_HEALTH_MULT_PERCENT = 100;
        public const int MIN_RESOURCE_CAP = 50;
        public const int MAX_RESOURCE_CAP = 100000;
        public const int DEFAULT_RESOURCE_CAP = 200;
        public const int MIN_MAX_UNITS = 0;
        public const int MAX_MAX_UNITS = 25000;
        // Java: 148
        public static int defaultMaxUnits()
        {
            return (com.oddlabs.tt.global.Edition.new_tides ? com.oddlabs.tt.player.Player.NEW_TIDES_MAX_UNIT_COUNT : com.oddlabs.tt.player.Player.DEFAULT_MAX_UNIT_COUNT);
        }
        public const string SEED_CARDINALITY = "40000";
        public const int SLIDER_CARDINALITY = 21;
        public const int TERRAIN_TYPE_CARDINALITY = 4;
        public const int TERRAIN_TYPE_CARDINALITY_LEGACY = 2;
        public const int SIZE_CARDINALITY = 7;
        public const int SIZE_CARDINALITY_LEGACY = 4;
        public const int DIFFICULTY_CARDINALITY = 8;
        public const int RACE_CARDINALITY = 2;
        public const int TEAM_CARDINALITY = 6;
        public static java.math.BigInteger MAX_VALUE;
        public static java.math.BigInteger MAX_VALUE_LEGACY;
        public static java.util.ResourceBundle bundle;
        // Java: 182
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.form.TerrainMenu.bundle, key, args);
        }
        public com.oddlabs.tt.@delegate.Menu main_menu;
        public com.oddlabs.tt.form.TerrainMenuListener owner;
        public com.oddlabs.tt.gui.PulldownMenu pulldown_size;
        public com.oddlabs.tt.gui.EditLine editline_name;
        public com.oddlabs.tt.gui.PulldownMenu pm_terrain_type;
        public com.oddlabs.tt.gui.Slider slider_hills;
        public com.oddlabs.tt.gui.Slider slider_vegetation;
        public com.oddlabs.tt.gui.Slider slider_supplies;
        public com.oddlabs.tt.gui.Label label_mapcode;
        public com.oddlabs.tt.gui.HorizButton button_ok;
        public string selected_custom_map_path;
        public com.oddlabs.tt.gui.PulldownMenu[] difficulty_pulldown_menus;
        public com.oddlabs.tt.gui.PulldownMenu[] race_pulldown_menus;
        public com.oddlabs.tt.gui.PulldownMenu[] team_pulldown_menus;
        public com.oddlabs.tt.gui.PulldownButton[] difficulty_pulldown_buttons;
        public com.oddlabs.tt.gui.PulldownButton[] race_pulldown_buttons;
        public com.oddlabs.tt.gui.PulldownButton[] team_pulldown_buttons;
        public com.oddlabs.tt.gui.Label[] labels_players;
        public com.oddlabs.tt.gui.CheckBox cb_rated;
        public com.oddlabs.tt.gui.CheckBox cb_free_for_all;
        public com.oddlabs.tt.gui.CheckBox cb_magic1_enabled;
        public com.oddlabs.tt.gui.CheckBox cb_magic2_enabled;
        public com.oddlabs.tt.gui.CheckBox cb_magic3_enabled;
        public com.oddlabs.tt.gui.CheckBox cb_chiefs_courage_enabled;
        public com.oddlabs.tt.gui.CheckBox cb_adaptive_ai_enabled;
        public com.oddlabs.tt.gui.CheckBox cb_team_together;
        public com.oddlabs.tt.gui.CheckBox cb_free_form;
        public com.oddlabs.tt.gui.CheckBox cb_extended;
        public com.oddlabs.tt.gui.CheckBox cb_weather;
        public com.oddlabs.tt.gui.Group group_set_all_difficulty;
        public com.oddlabs.tt.gui.PulldownMenu pm_set_all_difficulty;
        public com.oddlabs.tt.gui.Slider slider_magic1_cost;
        public com.oddlabs.tt.gui.Label label_magic1_cost_value;
        public com.oddlabs.tt.gui.Slider slider_magic2_cost;
        public com.oddlabs.tt.gui.Label label_magic2_cost_value;
        public com.oddlabs.tt.gui.Slider slider_magic3_cost;
        public com.oddlabs.tt.gui.Label label_magic3_cost_value;
        public com.oddlabs.tt.gui.Slider slider_building_health_mult;
        public com.oddlabs.tt.gui.Label label_building_health_mult_value;
        public com.oddlabs.tt.gui.Slider slider_viking_chief_health_mult;
        public com.oddlabs.tt.gui.Label label_viking_chief_health_mult_value;
        public com.oddlabs.tt.gui.Slider slider_native_chief_health_mult;
        public com.oddlabs.tt.gui.Label label_native_chief_health_mult_value;
        public com.oddlabs.tt.gui.Slider slider_unit_range_mult;
        public com.oddlabs.tt.gui.Label label_unit_range_mult_value;
        public com.oddlabs.tt.gui.Slider slider_armory_resource_cap;
        public com.oddlabs.tt.gui.Label label_armory_resource_cap_value;
        public com.oddlabs.tt.gui.Slider slider_rock_resource_cap;
        public com.oddlabs.tt.gui.Label label_rock_resource_cap_value;
        public com.oddlabs.tt.gui.Slider slider_iron_resource_cap;
        public com.oddlabs.tt.gui.Label label_iron_resource_cap_value;
        public com.oddlabs.tt.gui.Slider slider_rubber_resource_cap;
        public com.oddlabs.tt.gui.Label label_rubber_resource_cap_value;
        public com.oddlabs.tt.gui.PulldownMenu pm_max_chiefs_per_q;
        public com.oddlabs.tt.gui.PulldownMenu pm_max_chiefs_total;
        public com.oddlabs.tt.gui.Slider slider_initial_units;
        public com.oddlabs.tt.gui.Label label_initial_units_value;
        public com.oddlabs.tt.gui.Slider slider_starting_rock_warriors;
        public com.oddlabs.tt.gui.Label label_starting_rock_warriors_value;
        public com.oddlabs.tt.gui.Slider slider_starting_iron_warriors;
        public com.oddlabs.tt.gui.Label label_starting_iron_warriors_value;
        public com.oddlabs.tt.gui.Slider slider_starting_rubber_warriors;
        public com.oddlabs.tt.gui.Label label_starting_rubber_warriors_value;
        public com.oddlabs.tt.gui.Slider slider_max_units;
        public com.oddlabs.tt.gui.Label label_max_units_value;
        public com.oddlabs.tt.gui.PulldownMenu pm_max_buildings;
        public com.oddlabs.tt.gui.PulldownMenu pm_chief_heal_idle;
        public com.oddlabs.tt.gui.PulldownMenu pm_chief_heal_amount;
        public com.oddlabs.tt.gui.PulldownMenu pm_target_quarters;
        public com.oddlabs.tt.gui.PulldownMenu pm_target_armories;
        public com.oddlabs.tt.gui.PulldownMenu pm_max_concur_quarters;
        public com.oddlabs.tt.gui.PulldownMenu pm_max_concur_armories;
        public com.oddlabs.tt.gui.PulldownMenu pm_num_res_towers;
        public com.oddlabs.tt.gui.PulldownMenu pm_max_concur_towers;
        public com.oddlabs.tt.gui.PulldownMenu pm_koth_statue_count;
        // the extended chieftain magics (RacesResources 3..5): on / off and their cost
        public com.oddlabs.tt.gui.CheckBox[] cb_extra_magic_enabled = new com.oddlabs.tt.gui.CheckBox[com.oddlabs.tt.model.RacesResources.NUM_MAGIC];
        public com.oddlabs.tt.gui.Slider[] slider_extra_magic_cost = new com.oddlabs.tt.gui.Slider[com.oddlabs.tt.model.RacesResources.NUM_MAGIC];
        public float snapshotExtraMagicCost(int index)
        {
            float v = slider_extra_magic_cost[index].getValue();
            // the game's own cost is stored as 0, so presets keep following it
            return (java.lang.Math.round(v) == java.lang.Math.round(com.oddlabs.tt.model.Unit.MAX_MAGIC_ENERGY[index]) ? 0.0f : v);
        }
        public bool snapshotExtraMagicEnabled(int index)
        {
            return cb_extra_magic_enabled[index].isMarked();
        }
        // a column of right-aligned rows (label + pulldown), and a second column beside it, never overlapping
        /// <summary>Lays the check boxes out in rows no wider than max_width (the map options above them): the six of
        /// New Tides in one row ran past the panel. Returns the first box of the last row (to place what follows).</summary>
        public static com.oddlabs.tt.gui.GUIObject flowRows(com.oddlabs.tt.gui.GUIObject[] items, com.oddlabs.tt.gui.GUIObject above, int max_width, int gap, int row_gap, int per_row = 3)
        {
            com.oddlabs.tt.gui.GUIObject row_start = null, prev = null;
            int row_width = 0, in_row = 0;
            // a common column width, so the boxes of the rows line up
            int cell = 0;
            foreach (com.oddlabs.tt.gui.GUIObject item in items) if (item != null) cell = java.lang.Math.max(cell, item.getWidth());
            foreach (com.oddlabs.tt.gui.GUIObject item in items)
            {
                if (item == null) continue;
                int w = item.getWidth();
                if (prev == null) { if (above != null) item.place(above, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT, row_gap); else item.place(); row_start = item; row_width = cell; in_row = 1; }
                else if (in_row >= per_row || row_width + gap + cell > max_width) { item.place(row_start, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT, gap / 2); row_start = item; row_width = cell; in_row = 1; }
                else { item.setPos(prev.getX() + cell + gap, prev.getY() + (prev.getHeight() - item.getHeight()) / 2); row_width += gap + cell; in_row++; }
                prev = item;
            }
            return row_start;
        }

        /// <summary>Rows of (label, slider, value) in two columns: each column's labels right-aligned to one edge so
        /// its sliders start together, the right column past the widest left row. The rows keep their heights.</summary>
        public static void alignColumns(com.oddlabs.tt.gui.GUIObject[][] left, com.oddlabs.tt.gui.GUIObject[][] right, int gap, int column_gap)
        {
            int x0 = int.MaxValue;
            foreach (var r in left) x0 = java.lang.Math.min(x0, r[0].getX());
            int end = alignColumn(left, x0, gap);
            alignColumn(right, end + column_gap, gap);
        }

        static int alignColumn(com.oddlabs.tt.gui.GUIObject[][] rows, int x0, int gap)
        {
            int lw = 0, sw = 0, vw = 0;
            foreach (var r in rows) { lw = java.lang.Math.max(lw, r[0].getWidth()); sw = java.lang.Math.max(sw, r[1].getWidth()); vw = java.lang.Math.max(vw, r[2].getWidth()); }
            foreach (var r in rows)
            {
                int y = r[0].getY(), h = r[0].getHeight();
                r[0].setPos(x0 + lw - r[0].getWidth(), y);
                r[1].setPos(x0 + lw + gap, y + (h - r[1].getHeight()) / 2);
                r[2].setPos(x0 + lw + gap + sw + gap, y + (h - r[2].getHeight()) / 2);
            }
            return x0 + lw + gap + sw + gap + vw;
        }

        /// <summary>A team colour as text on the dark forms: the dark ones (Team 2's blue) were unreadable, so New Tides
        /// lifts them towards white until they read; the colour stays recognisable.</summary>
        public static org.joml.Vector4fc readable(org.joml.Vector4fc c)
        {
            if (!com.oddlabs.tt.global.Edition.new_tides || c == null) return c;
            // (the forms are dark blue: blue and violet text vanished into them, the warm colours read fine)
            if (c.z() < c.x() || c.z() < c.y()) return c;
            float lum = 0.2126f * c.x() + 0.7152f * c.y() + 0.0722f * c.z();
            if (lum >= 0.6f) return c;
            float t = java.lang.Math.min(1f, (0.6f - lum) / (1f - lum) * 1.15f);
            return new org.joml.Vector4f(c.x() + (1f - c.x()) * t, c.y() + (1f - c.y()) * t, c.z() + (1f - c.z()) * t, c.w());
        }

        public static void layoutColumns(com.oddlabs.tt.gui.Renderable[] left, com.oddlabs.tt.gui.Renderable[] right, int row_gap, int column_gap)
        {
            int lw = 0, rw = 0, rh = 0;
            foreach (com.oddlabs.tt.gui.Renderable r in left) { lw = java.lang.Math.max(lw, r.getWidth()); rh = java.lang.Math.max(rh, r.getHeight()); }
            foreach (com.oddlabs.tt.gui.Renderable r in right) { rw = java.lang.Math.max(rw, r.getWidth()); rh = java.lang.Math.max(rh, r.getHeight()); }
            int step = rh + row_gap;
            int rows = java.lang.Math.max(left.Length, right.Length);
            int top = rows * step - row_gap;
            for (int i = 0; i < left.Length; i++)
            {
                left[i].setPos(lw - left[i].getWidth(), top - rh - i * step + (rh - left[i].getHeight()) / 2);
            }
            for (int i = 0; i < right.Length; i++)
            {
                right[i].setPos(lw + column_gap + rw - right[i].getWidth(), top - rh - i * step + (rh - right[i].getHeight()) / 2);
            }
        }
        public bool multiplayer;
        public com.oddlabs.tt.gui.PulldownMenu pm_gamespeed;
        public com.oddlabs.tt.gui.GUIRoot gui_root;
        public com.oddlabs.net.NetworkSelector network;
        public com.oddlabs.tt.gamemode.PresetLibrary preset_library;
        public com.oddlabs.tt.form.RosterPanel roster_panel;
        public com.oddlabs.tt.form.ModeAndPresetsPanel mode_and_presets;
        public com.oddlabs.tt.gui.ScrollablePulldownMenu pulldown_menu_slots;
        public const int DEFAULT_PLAYER_COUNT = 6;
        public int player_count;
        public int seed;
        public com.oddlabs.matchmaking.Preset current_preset;
        public com.oddlabs.matchmaking.GameMode selected_mode;
        public bool modified;
        public bool apply_in_progress;
        public bool initialized;
        // Java: 1504
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
        // Java: 1514
        public void updateBanner()
        {
            if (mode_and_presets != null)
            {
                mode_and_presets.setPresetState(current_preset, modified);
            }
        }
        // Java: 1520
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
        // Java: 1546
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
            int terrain_type = java.lang.Math.min(pm_terrain_type.getChosenItemIndex(), com.oddlabs.tt.form.TerrainMenu.TERRAIN_TYPE_CARDINALITY_LEGACY - 1);
            result = result.add((new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(terrain_type)) })).multiply(max_val));
            max_val = max_val.multiply(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.TERRAIN_TYPE_CARDINALITY_LEGACY)) }));
            int size = pulldown_size.getChosenItemIndex();
            result = result.add((new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(size)) })).multiply(max_val));
            max_val = max_val.multiply(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.SIZE_CARDINALITY_LEGACY)) }));
            int player_race = race_pulldown_menus[0].getChosenItemIndex() % com.oddlabs.tt.form.TerrainMenu.RACE_CARDINALITY;
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
                int race = race_pulldown_menus[i].getChosenItemIndex() % com.oddlabs.tt.form.TerrainMenu.RACE_CARDINALITY;
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
        // Java: 1590
        // (from the Steam release, 2.2.207: a letter code that isn't one no longer crashes, and any text gives an island)
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
                    label_mapcode.clear();
                    label_mapcode.append(code);
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
        /// <summary>Text that is not a map code: the same island every time for the same text, shown as its word code.</summary>
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
        // Java: 1611
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
        // Java: 1642
        public void parseBigIntegerLegacy(java.math.BigInteger result)
        {
            java.math.BigInteger max_val = com.oddlabs.tt.form.TerrainMenu.MAX_VALUE_LEGACY;
            for (int i = com.oddlabs.tt.form.TerrainMenu.DEFAULT_PLAYER_COUNT - 1; i >= 1; i--)
            {
                result = result.mod(max_val);
                max_val = max_val.divide(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.TEAM_CARDINALITY)) }));
                int team = result.divide(max_val).intValue();
                team_pulldown_menus[i].chooseItem(java.lang.Math.max(0, java.lang.Math.min(team, team_pulldown_menus[i].getSize() - 1)));   // (a code can hold more than the menu has)
                result = result.mod(max_val);
                max_val = max_val.divide(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.RACE_CARDINALITY)) }));
                int race = result.divide(max_val).intValue();
                race_pulldown_menus[i].chooseItem(java.lang.Math.max(0, java.lang.Math.min(race, race_pulldown_menus[i].getSize() - 1)));   // (a code can hold more than the menu has)
                result = result.mod(max_val);
                max_val = max_val.divide(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.DIFFICULTY_CARDINALITY)) }));
                int difficulty = result.divide(max_val).intValue();
                difficulty = java.lang.Math.min(difficulty, difficulty_pulldown_menus[i].getSize() - 1);
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
            team_pulldown_menus[0].chooseItem(java.lang.Math.max(0, java.lang.Math.min(player_team, team_pulldown_menus[0].getSize() - 1)));   // (a code can hold more than the menu has)
            result = result.mod(max_val);
            max_val = max_val.divide(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.RACE_CARDINALITY)) }));
            int player_race = result.divide(max_val).intValue();
            race_pulldown_menus[0].chooseItem(java.lang.Math.max(0, java.lang.Math.min(player_race, race_pulldown_menus[0].getSize() - 1)));   // (a code can hold more than the menu has)
            result = result.mod(max_val);
            max_val = max_val.divide(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.SIZE_CARDINALITY_LEGACY)) }));
            int size = result.divide(max_val).intValue();
            pulldown_size.chooseItem(java.lang.Math.max(0, java.lang.Math.min(size, pulldown_size.getSize() - 1)));   // (a code can hold more than the menu has)
            result = result.mod(max_val);
            max_val = max_val.divide(new java.math.BigInteger(new sbyte[] { unchecked((sbyte)(com.oddlabs.tt.form.TerrainMenu.TERRAIN_TYPE_CARDINALITY_LEGACY)) }));
            int terrain_type = result.divide(max_val).intValue();
            pm_terrain_type.chooseItem(java.lang.Math.max(0, java.lang.Math.min(terrain_type, pm_terrain_type.getSize() - 1)));   // (a code can hold more than the menu has)
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
        // Java: 1705
        public void setSeed(int seed)
        {
            this.seed = seed;
        }
        // Java: 1709
        public com.oddlabs.tt.gui.GUIObject getButtonOK()
        {
            return button_ok;
        }
        // Java: 1713
        public class CancelButtonListener : java.lang.Object, com.oddlabs.tt.guievent.MouseClickListener
        {
            public com.oddlabs.tt.form.TerrainMenu this_0;
            // Java: 1714
            public void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
            {
                this_0.owner.terrainMenuCancel();
            }
            public CancelButtonListener(com.oddlabs.tt.form.TerrainMenu this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        // Java: 1720
        public com.oddlabs.tt.gui.ScrollableGroup buildPlayerSlots(int count)
        {
            int roster_height = com.oddlabs.tt.form.TerrainMenu.ROSTER_CONTENT_HEIGHT;
            if (multiplayer)
            {
                int spacing = com.oddlabs.tt.gui.Skin.getSkin().getFormData().sectionSpacing();
                roster_height -= cb_adaptive_ai_enabled.getHeight() + spacing + group_set_all_difficulty.getHeight() + spacing;
                roster_height = java.lang.Math.max(roster_height, com.oddlabs.tt.form.TerrainMenu.MIN_ROSTER_CONTENT_HEIGHT);
            }
            com.oddlabs.tt.gui.ScrollableGroup inner = new com.oddlabs.tt.gui.ScrollableGroup(roster_height, 64);
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
                    difficulty_pulldown_menus[i].addItem(new com.oddlabs.tt.gui.PulldownItem(i18n("expert_ai")));
                    difficulty_pulldown_menus[i].addItem(new com.oddlabs.tt.gui.PulldownItem(i18n("gauntlet_ai")));
                }
                difficulty_pulldown_buttons[i] = new com.oddlabs.tt.gui.PulldownButton(gui_root, difficulty_pulldown_menus[i], 0, 115);
                inner.addChild(difficulty_pulldown_buttons[i]);
                for (int j = 0; j < com.oddlabs.tt.model.RacesResources.getNumRaceChoices(); j++)
                {
                    race_pulldown_menus[i].addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.model.RacesResources.getRaceChoiceName(j)));
                }
                race_pulldown_buttons[i] = new com.oddlabs.tt.gui.PulldownButton(gui_root, race_pulldown_menus[i], 0, 115);
                inner.addChild(race_pulldown_buttons[i]);
                for (int j_2 = 0; j_2 < count; j_2++)
                {
                    com.oddlabs.tt.gui.PulldownItem team_item = new com.oddlabs.tt.gui.PulldownItem(i18n("team", java.lang.Integer.toString(j_2 + 1)));
                    team_item.setLabelColor(readable(com.oddlabs.tt.global.Settings.getSettings().team_colours[j_2]));
                    team_pulldown_menus[i].addItem(team_item);
                }
                team_pulldown_buttons[i] = new com.oddlabs.tt.gui.PulldownButton(gui_root, team_pulldown_menus[i], java.lang.Math.min(i, count - 1), 115);
                inner.addChild(team_pulldown_buttons[i]);
                if (i == 0)
                {
                    labels_players[0] = new com.oddlabs.tt.gui.Label(i18n("player", java.lang.Integer.toString(1)), com.oddlabs.tt.gui.Skin.getSkin().getEditFont()).setColor(readable(com.oddlabs.tt.global.Settings.getSettings().team_colours[0]));
                    inner.addChild(labels_players[0]);
                    labels_players[0].place();
                    difficulty_pulldown_buttons[0].place(labels_players[0], com.oddlabs.tt.gui.Placement.RIGHT_MID);
                    race_pulldown_buttons[0].place(difficulty_pulldown_buttons[0], com.oddlabs.tt.gui.Placement.RIGHT_MID);
                    team_pulldown_buttons[0].place(race_pulldown_buttons[0], com.oddlabs.tt.gui.Placement.RIGHT_MID);
                }
                else
                {
                    labels_players[i] = new com.oddlabs.tt.gui.Label(i18n("player", java.lang.Integer.toString(i + 1)), com.oddlabs.tt.gui.Skin.getSkin().getEditFont()).setColor(readable(com.oddlabs.tt.global.Settings.getSettings().team_colours[i]));
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
        // Java: 1800
        public void randomize()
        {
            java.util.Random random = new java.util.Random(com.oddlabs.tt.@event.LocalEventQueue.getQueue().getHighPrecisionManager().getTick() * ((long)(com.oddlabs.tt.@event.LocalEventQueue.getQueue().getHighPrecisionManager().getTick())));
            random.nextInt();
            java.math.BigInteger rand_int = new java.math.BigInteger(100, random);
            parseBigIntegerLegacy(rand_int);
            setMapcode();
        }
        // Java: 1809
        public void doCancel()
        {
            if (multiplayer)
            {
                new com.oddlabs.tt.form.SelectGameMenu(network, gui_root, main_menu);
            }
        }
        // Java: 1818
        public int defaultTeam(int i)
        {
            if (multiplayer)
            {
                return i;
            }
            return (i == 0 ? 0 : 1);
        }
        // Java: 1825
        public bool isChosen(com.oddlabs.tt.gui.PulldownMenu menu)
        {
            return menu.getChosenItemIndex() != 0;
        }
        // Java: 1829
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
                game = com.oddlabs.matchmaking.Game.builder().name(game_name).size(unchecked((sbyte)(pulldown_size.getChosenItemIndex()))).terrain(unchecked((sbyte)(terrain_type.ordinal()))).hills(unchecked((sbyte)(hills))).trees(unchecked((sbyte)(vegetation_amount))).supplies(unchecked((sbyte)(supplies_amount))).rated(rated).gamespeed(unchecked((sbyte)((pm_gamespeed.getChosenItemIndex() + 1)))).mapcode(label_mapcode.getContents()).randomStartPos(random_start_pos).maxUnitCount(snapshotMaxUnits()).build();
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
            com.oddlabs.tt.viewer.InGameInfo ingame_info = (multiplayer ? (com.oddlabs.tt.viewer.InGameInfo)(new com.oddlabs.tt.viewer.MultiplayerInGameInfo(game.getRandomStartPos(), game.isRated())) : (com.oddlabs.tt.viewer.InGameInfo)(new com.oddlabs.tt.viewer.DefaultInGameInfo()));
            com.oddlabs.tt.net.GameNetwork game_network = com.oddlabs.tt.@delegate.Menu.startNewGame(network, gui_root, menu, com.oddlabs.tt.landscape.WorldParameters.builder().initialGameSpeed((multiplayer ? game.getGamespeed() : com.oddlabs.tt.global.Globals.gamespeed)).mapcode((selected_editor_map != null ? selected_editor_map.getMapcode() : label_mapcode.getContents())).initialUnitCount(snapshotInitialUnits()).maxUnitCount(snapshotMaxUnits()).mapSize(editorMapSizeIndex()).mode(selected_mode).maxChieftains(snapshotMaxChiefsTotal()).maxChieftainsPerQuarters(snapshotMaxChiefsPerQ()).maxBuildingCount(snapshotMaxBuildings()).chieftainHealIdleSeconds(snapshotChiefHealIdleSeconds()).chieftainHealAmount(snapshotChiefHealAmount()).targetNumQuarters(snapshotTargetQuarters()).targetNumArmories(snapshotTargetArmories()).maxConcurrentQuarters(snapshotMaxConcurrentQuarters()).maxConcurrentArmories(snapshotMaxConcurrentArmories()).numResourceTowers(snapshotNumResourceTowers()).maxConcurrentTowers(snapshotMaxConcurrentTowers()).kothStatueCount(snapshotKothStatueCount()).startingRockWarriors(snapshotStartingRockWarriors()).startingIronWarriors(snapshotStartingIronWarriors()).startingRubberWarriors(snapshotStartingRubberWarriors()).magic1Enabled(cb_magic1_enabled.isMarked()).magic2Enabled(cb_magic2_enabled.isMarked()).magic3Enabled(cb_magic3_enabled.isMarked()).chiefsCourageEnabled(cb_chiefs_courage_enabled.isMarked()).adaptiveAiEnabled(cb_adaptive_ai_enabled.isMarked()).adaptiveAiSeedDifficulty((cb_adaptive_ai_enabled.isMarked() ? com.oddlabs.tt.player.AdaptiveAIProfile.get().packedOpeningTiers() : (-1))).teamTogether(cb_team_together.isMarked()).freeForm(cb_free_form.isMarked()).extended(cb_extended.isMarked()).weather(com.oddlabs.tt.global.Edition.new_tides && cb_weather.isMarked()).magic1Cost(snapshotMagic1Cost()).magic2Cost(snapshotMagic2Cost()).magic3Cost(snapshotMagic3Cost()).buildingHealthMultiplier(snapshotBuildingHealthMultiplier()).vikingChiefHealthMultiplier(snapshotVikingChiefHealthMultiplier()).nativeChiefHealthMultiplier(snapshotNativeChiefHealthMultiplier()).unitRangeMultiplier(snapshotUnitRangeMultiplier()).armoryResourceCap(snapshotArmoryResourceCap()).rockResourceCap(snapshotRockResourceCap()).ironResourceCap(snapshotIronResourceCap()).rubberResourceCap(snapshotRubberResourceCap()).extraMagicCost(3, snapshotExtraMagicCost(3)).extraMagicEnabled(3, snapshotExtraMagicEnabled(3)).extraMagicCost(4, snapshotExtraMagicCost(4)).extraMagicEnabled(4, snapshotExtraMagicEnabled(4)).extraMagicCost(5, snapshotExtraMagicCost(5)).extraMagicEnabled(5, snapshotExtraMagicEnabled(5)).build(), ingame_info, new com.oddlabs.tt.@delegate.Menu.DefaultWorldInitAction(), game, com.oddlabs.tt.form.TerrainMenu.SIZES[editorMapSizeIndex()], terrain_type, (float)(hills / ((float)(com.oddlabs.tt.form.TerrainMenu.SLIDER_MAX_VALUE))), (float)(vegetation_amount / ((float)(com.oddlabs.tt.form.TerrainMenu.SLIDER_MAX_VALUE))), (float)(supplies_amount / ((float)(com.oddlabs.tt.form.TerrainMenu.SLIDER_MAX_VALUE))), seed * seed, com.oddlabs.tt.form.TerrainMenu.ARCHIPELAGO[pulldown_size.getChosenItemIndex()] && com.oddlabs.tt.global.Globals.SHIPS_ENABLED, (selected_editor_map != null ? (string)(null) : selected_custom_map_path), ai_names, player_count, (selected_editor_map != null ? selected_editor_map.createGenerator() : (com.oddlabs.tt.resource.WorldGenerator)(null)));
            game_network.getClient().getServerInterface().setPlayerSlot(0, com.oddlabs.tt.net.PlayerSlot.HUMAN, race_pulldown_menus[0].getChosenItemIndex(), team_pulldown_menus[0].getChosenItemIndex(), !multiplayer, com.oddlabs.tt.net.PlayerSlot.AI_NONE);
            if (com.oddlabs.tt.global.Edition.new_tides)
            {
                game_network.getClient().getServerInterface().setPlayerNames(com.oddlabs.tt.global.Settings.getSettings().chief_name, com.oddlabs.tt.global.Settings.getSettings().tribe_name);
            }
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
                        game_network.getClient().getServerInterface().setPlayerSlot(i_3, com.oddlabs.tt.net.PlayerSlot.AI, race_pulldown_menus[i_3].getChosenItemIndex(), team_pulldown_menus[i_3].getChosenItemIndex(), true, com.oddlabs.tt.form.TerrainMenu.fillToPlayerSlotAI(difficultyIndexToFill(i_3, difficulty_pulldown_menus[i_3].getChosenItemIndex())));
                    }
                }
                game_network.getClient().getServerInterface().startServer();
                java.lang.IO.println("Start server");
            }
            java.lang.IO.println("Map code: " + java.lang.JStr.valueOf(label_mapcode.getContents()));
            return true;
        }
        // Java: 1982
        public class MapcodeListener : java.lang.Object, com.oddlabs.tt.guievent.MouseClickListener
        {
            public com.oddlabs.tt.form.TerrainMenu this_0;
            // Java: 1983
            public void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
            {
                this_0.gui_root.addModalForm(new com.oddlabs.tt.form.MapcodeForm(this.this_0));
            }
            public MapcodeListener(com.oddlabs.tt.form.TerrainMenu this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        // Java: 1997
        public class CustomMapListener : java.lang.Object, com.oddlabs.tt.guievent.MouseClickListener
        {
            public com.oddlabs.tt.form.TerrainMenu this_0;
            // Java: 1998
            public void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
            {
                bool wasFullscreen = com.oddlabs.tt.global.Settings.getSettings().fullscreen;
                if (wasFullscreen)
                {
                    com.oddlabs.tt.render.Renderer.getRenderer().toggleFullscreen();
                }
                java.io.File maps_dir = com.oddlabs.tt.resource.CustomMapGenerator.sharedMapsDir().toFile();
                maps_dir.mkdirs();
                string path = org.lwjgl.util.tinyfd.TinyFileDialogs.tinyfd_openFileDialog(this_0.i18n("dialog_load_custom_map"), java.lang.JStr.valueOf(maps_dir.getAbsolutePath()) + java.lang.JStr.valueOf(java.io.File.separator), null, this_0.i18n("ttmap_files"), false);
                if (path != null)
                {
                    try
                    {
                        if (com.oddlabs.tt.mapeditor.MapEditor.isEditorMap(java.nio.file.Path_.of(path)))
                        {
                            this_0.selected_editor_map = com.oddlabs.tt.mapeditor.MapEditor.customMapFromFile(java.nio.file.Path_.of(path));
                            this_0.selected_custom_map_path = null;
                            this_0.label_mapcode.setText(new java.io.File(path).getName());
                            this_0.slider_hills.setDisabled(true);
                            this_0.slider_vegetation.setDisabled(true);
                            this_0.slider_supplies.setDisabled(true);
                        }
                        else
                        {
                            com.oddlabs.procedural.AuthoredTerrain.load(new java.io.File(path));
                            this_0.selected_editor_map = null;
                            this_0.selected_custom_map_path = path;
                            this_0.label_mapcode.setText(new java.io.File(path).getName());
                            this_0.slider_hills.setDisabled(true);
                            this_0.slider_vegetation.setDisabled(true);
                            this_0.slider_supplies.setDisabled(true);
                        }
                    }
                    catch (System.IO.IOException e)
                    {
                        this_0.gui_root.addModalForm(new com.oddlabs.tt.form.MessageForm(this_0.i18n("error_load_custom_map_failed", e.getMessage())));
                    }
                }
                if (wasFullscreen)
                {
                    com.oddlabs.tt.render.Renderer.getRenderer().toggleFullscreen();
                }
            }
            public CustomMapListener(com.oddlabs.tt.form.TerrainMenu this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        // Java: 2046
        public class OKListener : java.lang.Object, com.oddlabs.tt.guievent.MouseClickListener
        {
            public com.oddlabs.tt.form.TerrainMenu this_0;
            // Java: 2047
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
        // Java: 2055
        public class DisableListener : java.lang.Object, com.oddlabs.tt.guievent.ItemChosenListener
        {
            public com.oddlabs.tt.form.TerrainMenu this_0;
            public int i;
            // Java: 2062
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
        // Java: 2074
        public class PulldownUpdateMapcodeListener : java.lang.Object, com.oddlabs.tt.guievent.ItemChosenListener
        {
            public com.oddlabs.tt.form.TerrainMenu this_0;
            // Java: 2075
            public void itemChosen(com.oddlabs.tt.gui.PulldownMenu menu, int item_index)
            {
                this_0.setMapcode();
            }
            public PulldownUpdateMapcodeListener(com.oddlabs.tt.form.TerrainMenu this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        // Java: 2081
        public class PulldownUpdateSizeListener : java.lang.Object, com.oddlabs.tt.guievent.ItemChosenListener
        {
            // Java: 2082
            public void itemChosen(com.oddlabs.tt.gui.PulldownMenu menu, int item_index)
            {
            }
            public PulldownUpdateSizeListener() : base()
            {
            }
        }
        // Java: 2087
        public class PulldownUpdateHardListener : java.lang.Object, com.oddlabs.tt.guievent.ItemChosenListener
        {
            // Java: 2088
            public void itemChosen(com.oddlabs.tt.gui.PulldownMenu menu, int item_index)
            {
            }
            public PulldownUpdateHardListener() : base()
            {
            }
        }
        // Java: 2093
        public class PulldownUpdateTerrainListener : java.lang.Object, com.oddlabs.tt.guievent.ItemChosenListener
        {
            // Java: 2094
            public void itemChosen(com.oddlabs.tt.gui.PulldownMenu menu, int item_index)
            {
            }
            public PulldownUpdateTerrainListener() : base()
            {
            }
        }
        // Java: 2099
        public class SliderUpdateMapcodeListener : java.lang.Object, com.oddlabs.tt.guievent.ValueListener
        {
            public com.oddlabs.tt.form.TerrainMenu this_0;
            // Java: 2100
            public void valueSet(long @value)
            {
                this_0.setMapcode();
            }
            public SliderUpdateMapcodeListener(com.oddlabs.tt.form.TerrainMenu this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        // Java: 2106
        public class PresetsHandler : java.lang.Object, com.oddlabs.tt.form.ModeAndPresetsHandler
        {
            public com.oddlabs.tt.form.TerrainMenu this_0;
            // Java: 2107
            public void modeChosen(com.oddlabs.matchmaking.GameMode mode)
            {
                this.this_0.selected_mode = mode;
                if (this_0.current_preset != null && ((object)this_0.current_preset.getMode() != (object)mode))
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
            // Java: 2120
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
            // Java: 2132
            public void applySelectedPreset(com.oddlabs.matchmaking.Preset preset)
            {
                this_0.current_preset = preset;
                this_0.apply_in_progress = true;
                this_0.applyPreset(preset);
                this_0.apply_in_progress = false;
                this_0.modified = false;
                this_0.updateBanner();
            }
            // Java: 2141
            public void revertSelection()
            {
                if (this_0.mode_and_presets != null)
                {
                    this_0.mode_and_presets.refreshPresets();
                }
            }
            // Java: 2147
            public void presetDeleted(com.oddlabs.matchmaking.Preset preset)
            {
                this_0.gui_root.addModalForm(new com.oddlabs.tt.form.QuestionForm(this_0.i18n("confirm_delete_preset", preset.getName()), new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
                {
                    confirmDeletePreset(preset);
                })));
            }
            // Java: 2153
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
            // Java: 2166
            public void saveClicked()
            {
                this_0.gui_root.addModalForm(new com.oddlabs.tt.form.SavePresetDialog(new java.util.function.Predicate_L((object a0) => { return this_0.preset_library.hasName(((string)a0)); }), new com.oddlabs.tt.form.SavePresetDialog.SaveListener_L((string a0) => { this.saveNewPreset(a0); })));
            }
            // Java: 2171
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
            // Java: 2183
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
            // Java: 2201
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
        // Java: 2216
        public com.oddlabs.matchmaking.StandardOptions snapshotModeOptions()
        {
            return com.oddlabs.matchmaking.StandardOptions.builder().rated(cb_rated.isMarked()).build();
        }
        // Java: 2222
        public int snapshotMaxChiefsPerQ()
        {
            return ((pm_max_chiefs_per_q.getChosenItemIndex()) switch { 0 => (int)(1), 1 => (int)(3), 2 => (int)(5), 3 => (int)(10), 4 => (int)(15), 5 => (int)(20), 6 => (int)(9999), _ => (int)(1) });
        }
        // Java: 2237
        public int snapshotMaxChiefsTotal()
        {
            return ((pm_max_chiefs_total.getChosenItemIndex()) switch { 0 => (int)(1), 1 => (int)(3), 2 => (int)(5), 3 => (int)(10), 4 => (int)(15), 5 => (int)(20), 6 => (int)(9999), _ => (int)(5) });
        }
        // Java: 2251
        public int snapshotInitialUnits()
        {
            return (java.lang.Math.clamp(slider_initial_units.getValue() * ((long)(com.oddlabs.tt.form.TerrainMenu.UNIT_COUNT_STEP)), com.oddlabs.tt.form.TerrainMenu.MIN_INITIAL_UNITS, com.oddlabs.tt.form.TerrainMenu.MAX_INITIAL_UNITS));
        }
        // Java: 2257
        public int snapshotMaxUnits()
        {
            return (java.lang.Math.clamp(slider_max_units.getValue() * ((long)(com.oddlabs.tt.form.TerrainMenu.UNIT_COUNT_STEP)), com.oddlabs.tt.form.TerrainMenu.MIN_MAX_UNITS, com.oddlabs.tt.form.TerrainMenu.MAX_MAX_UNITS));
        }
        // Java: 2261
        public int snapshotStartingRockWarriors()
        {
            return java.lang.Math.clamp(slider_starting_rock_warriors.getValue(), com.oddlabs.tt.form.TerrainMenu.MIN_STARTING_WARRIORS, com.oddlabs.tt.form.TerrainMenu.MAX_STARTING_WARRIORS);
        }
        // Java: 2265
        public int snapshotStartingIronWarriors()
        {
            return java.lang.Math.clamp(slider_starting_iron_warriors.getValue(), com.oddlabs.tt.form.TerrainMenu.MIN_STARTING_WARRIORS, com.oddlabs.tt.form.TerrainMenu.MAX_STARTING_WARRIORS);
        }
        // Java: 2269
        public int snapshotStartingRubberWarriors()
        {
            return java.lang.Math.clamp(slider_starting_rubber_warriors.getValue(), com.oddlabs.tt.form.TerrainMenu.MIN_STARTING_WARRIORS, com.oddlabs.tt.form.TerrainMenu.MAX_STARTING_WARRIORS);
        }
        // Java: 2273
        public float snapshotMagic1Cost()
        {
            return java.lang.Math.clamp(slider_magic1_cost.getValue(), com.oddlabs.tt.form.TerrainMenu.MIN_MAGIC_COST, com.oddlabs.tt.form.TerrainMenu.MAX_MAGIC_COST);
        }
        // Java: 2277
        public float snapshotMagic2Cost()
        {
            return java.lang.Math.clamp(slider_magic2_cost.getValue(), com.oddlabs.tt.form.TerrainMenu.MIN_MAGIC_COST, com.oddlabs.tt.form.TerrainMenu.MAX_MAGIC_COST);
        }
        // Java: 2281
        public float snapshotMagic3Cost()
        {
            return java.lang.Math.clamp(slider_magic3_cost.getValue(), com.oddlabs.tt.form.TerrainMenu.MIN_MAGIC_COST, com.oddlabs.tt.form.TerrainMenu.MAX_MAGIC_COST);
        }
        // Java: 2285
        public float snapshotBuildingHealthMultiplier()
        {
            return (float)(java.lang.Math.clamp(slider_building_health_mult.getValue(), com.oddlabs.tt.form.TerrainMenu.MIN_HEALTH_MULT_PERCENT, com.oddlabs.tt.form.TerrainMenu.MAX_HEALTH_MULT_PERCENT) / 100.0f);
        }
        // Java: 2290
        public float snapshotVikingChiefHealthMultiplier()
        {
            return (float)(java.lang.Math.clamp(slider_viking_chief_health_mult.getValue(), com.oddlabs.tt.form.TerrainMenu.MIN_HEALTH_MULT_PERCENT, com.oddlabs.tt.form.TerrainMenu.MAX_HEALTH_MULT_PERCENT) / 100.0f);
        }
        // Java: 2295
        public float snapshotNativeChiefHealthMultiplier()
        {
            return (float)(java.lang.Math.clamp(slider_native_chief_health_mult.getValue(), com.oddlabs.tt.form.TerrainMenu.MIN_HEALTH_MULT_PERCENT, com.oddlabs.tt.form.TerrainMenu.MAX_HEALTH_MULT_PERCENT) / 100.0f);
        }
        // Java: 2300
        public float snapshotUnitRangeMultiplier()
        {
            return (float)(java.lang.Math.clamp(slider_unit_range_mult.getValue(), com.oddlabs.tt.form.TerrainMenu.MIN_HEALTH_MULT_PERCENT, com.oddlabs.tt.form.TerrainMenu.MAX_HEALTH_MULT_PERCENT) / 100.0f);
        }
        // Java: 2305
        public int snapshotArmoryResourceCap()
        {
            return java.lang.Math.clamp(slider_armory_resource_cap.getValue(), com.oddlabs.tt.form.TerrainMenu.MIN_RESOURCE_CAP, com.oddlabs.tt.form.TerrainMenu.MAX_RESOURCE_CAP);
        }
        // Java: 2309
        public int snapshotRockResourceCap()
        {
            return java.lang.Math.clamp(slider_rock_resource_cap.getValue(), com.oddlabs.tt.form.TerrainMenu.MIN_RESOURCE_CAP, com.oddlabs.tt.form.TerrainMenu.MAX_RESOURCE_CAP);
        }
        // Java: 2313
        public int snapshotIronResourceCap()
        {
            return java.lang.Math.clamp(slider_iron_resource_cap.getValue(), com.oddlabs.tt.form.TerrainMenu.MIN_RESOURCE_CAP, com.oddlabs.tt.form.TerrainMenu.MAX_RESOURCE_CAP);
        }
        // Java: 2317
        public int snapshotRubberResourceCap()
        {
            return java.lang.Math.clamp(slider_rubber_resource_cap.getValue(), com.oddlabs.tt.form.TerrainMenu.MIN_RESOURCE_CAP, com.oddlabs.tt.form.TerrainMenu.MAX_RESOURCE_CAP);
        }
        // Java: 2322
        public int snapshotMaxBuildings()
        {
            return ((pm_max_buildings.getChosenItemIndex()) switch { 0 => (int)(20), 1 => (int)(50), 2 => (int)(100), 3 => (int)(500), 4 => (int)(1200), 5 => (int)(9999), _ => (int)(1200) });
        }
        // Java: 2335
        public float snapshotChiefHealIdleSeconds()
        {
            return ((pm_chief_heal_idle.getChosenItemIndex()) switch { 0 => (float)(1.0f), 1 => (float)(3.0f), 2 => (float)(5.0f), 3 => (float)(10.0f), 4 => (float)(20.0f), _ => (float)(3.0f) });
        }
        // Java: 2347
        public int snapshotChiefHealAmount()
        {
            return ((pm_chief_heal_amount.getChosenItemIndex()) switch { 0 => (int)(10), 1 => (int)(25), 2 => (int)(50), 3 => (int)(100), 4 => (int)(150), 5 => (int)(250), _ => (int)(50) });
        }
        // Java: 2360
        public int snapshotTargetQuarters()
        {
            return ((pm_target_quarters.getChosenItemIndex()) switch { 0 => (int)(1), 1 => (int)(2), 2 => (int)(3), 3 => (int)(5), 4 => (int)(7), 5 => (int)(10), 6 => (int)(12), _ => (int)(3) });
        }
        // Java: 2374
        public int snapshotTargetArmories()
        {
            return ((pm_target_armories.getChosenItemIndex()) switch { 0 => (int)(1), 1 => (int)(2), 2 => (int)(3), 3 => (int)(4), 4 => (int)(5), 5 => (int)(6), _ => (int)(1) });
        }
        // Java: 2391
        public int snapshotKothStatueCount()
        {
            return com.oddlabs.tt.landscape.WorldParameters.MIN_KOTH_STATUE_COUNT + pm_koth_statue_count.getChosenItemIndex();
        }
        // Java: 2395
        public int snapshotMaxConcurrentQuarters()
        {
            return ((pm_max_concur_quarters.getChosenItemIndex()) switch { 0 => (int)(1), 1 => (int)(2), 2 => (int)(3), 3 => (int)(4), 4 => (int)(5), _ => (int)(2) });
        }
        // Java: 2407
        public int snapshotMaxConcurrentArmories()
        {
            return ((pm_max_concur_armories.getChosenItemIndex()) switch { 0 => (int)(1), 1 => (int)(2), 2 => (int)(3), _ => (int)(1) });
        }
        // Java: 2417
        public int snapshotNumResourceTowers()
        {
            return ((pm_num_res_towers.getChosenItemIndex()) switch { 0 => (int)(0), 1 => (int)(1), 2 => (int)(3), 3 => (int)(5), 4 => (int)(7), 5 => (int)(10), 6 => (int)(15), 7 => (int)(20), 8 => (int)(30), _ => (int)(6) });
        }
        // Java: 2433
        public int snapshotMaxConcurrentTowers()
        {
            return ((pm_max_concur_towers.getChosenItemIndex()) switch { 0 => (int)(1), 1 => (int)(3), 2 => (int)(5), 3 => (int)(7), 4 => (int)(10), 5 => (int)(15), 6 => (int)(9999), _ => (int)(6) });
        }
        // Java: 2446
        public com.oddlabs.matchmaking.WorldConfig snapshotWorldConfig()
        {
            return com.oddlabs.matchmaking.WorldConfig.builder().gamespeed(pm_gamespeed.getChosenItemIndex()).islandSize(pulldown_size.getChosenItemIndex()).terrainType(pm_terrain_type.getChosenItemIndex()).hills(slider_hills.getValue()).vegetation(slider_vegetation.getValue()).supplies(slider_supplies.getValue()).initialUnitCount(snapshotInitialUnits()).maxUnitCount(snapshotMaxUnits()).maxChieftains(snapshotMaxChiefsTotal()).maxChieftainsPerQuarters(snapshotMaxChiefsPerQ()).maxBuildingCount(snapshotMaxBuildings()).chieftainHealIdleSeconds(snapshotChiefHealIdleSeconds()).chieftainHealAmount(snapshotChiefHealAmount()).targetNumQuarters(snapshotTargetQuarters()).targetNumArmories(snapshotTargetArmories()).maxConcurrentQuarters(snapshotMaxConcurrentQuarters()).maxConcurrentArmories(snapshotMaxConcurrentArmories()).numResourceTowers(snapshotNumResourceTowers()).maxConcurrentTowers(snapshotMaxConcurrentTowers()).kothStatueCount(snapshotKothStatueCount()).startingRockWarriors(snapshotStartingRockWarriors()).startingIronWarriors(snapshotStartingIronWarriors()).startingRubberWarriors(snapshotStartingRubberWarriors()).magic1Enabled(cb_magic1_enabled.isMarked()).magic2Enabled(cb_magic2_enabled.isMarked()).magic3Enabled(cb_magic3_enabled.isMarked()).chiefsCourageEnabled(cb_chiefs_courage_enabled.isMarked()).adaptiveAiEnabled(cb_adaptive_ai_enabled.isMarked()).teamTogether(cb_team_together.isMarked()).freeForm(cb_free_form.isMarked()).extended(cb_extended.isMarked()).weather(com.oddlabs.tt.global.Edition.new_tides && cb_weather.isMarked()).magic1Cost(snapshotMagic1Cost()).magic2Cost(snapshotMagic2Cost()).magic3Cost(snapshotMagic3Cost()).buildingHealthMultiplier(snapshotBuildingHealthMultiplier()).vikingChiefHealthMultiplier(snapshotVikingChiefHealthMultiplier()).nativeChiefHealthMultiplier(snapshotNativeChiefHealthMultiplier()).unitRangeMultiplier(snapshotUnitRangeMultiplier()).armoryResourceCap(snapshotArmoryResourceCap()).rockResourceCap(snapshotRockResourceCap()).ironResourceCap(snapshotIronResourceCap()).rubberResourceCap(snapshotRubberResourceCap()).extraMagicCost(3, snapshotExtraMagicCost(3)).extraMagicEnabled(3, snapshotExtraMagicEnabled(3)).extraMagicCost(4, snapshotExtraMagicCost(4)).extraMagicEnabled(4, snapshotExtraMagicEnabled(4)).extraMagicCost(5, snapshotExtraMagicCost(5)).extraMagicEnabled(5, snapshotExtraMagicEnabled(5)).build();
        }
        // Java: 2496
        public void applyWorldConfig(com.oddlabs.matchmaking.WorldConfig world)
        {
            pm_gamespeed.chooseItem(com.oddlabs.tt.form.TerrainMenu.clampIndex(world.getGamespeed(), pm_gamespeed.getSize()));
            pulldown_size.chooseItem(com.oddlabs.tt.form.TerrainMenu.clampIndex(world.getIslandSize(), pulldown_size.getSize()));
            pm_terrain_type.chooseItem(com.oddlabs.tt.form.TerrainMenu.clampIndex(world.getTerrainType(), pm_terrain_type.getSize()));
            slider_hills.setValue(world.getHills());
            slider_vegetation.setValue(world.getVegetation());
            slider_supplies.setValue(world.getSupplies());
            slider_initial_units.setValue(java.lang.Math.round((float)(java.lang.Math.clamp(world.getInitialUnitCount(), com.oddlabs.tt.form.TerrainMenu.MIN_INITIAL_UNITS, com.oddlabs.tt.form.TerrainMenu.MAX_INITIAL_UNITS) / ((float)(com.oddlabs.tt.form.TerrainMenu.UNIT_COUNT_STEP)))));
            slider_max_units.setValue(java.lang.Math.round((float)(java.lang.Math.clamp(world.getMaxUnitCount(), com.oddlabs.tt.form.TerrainMenu.MIN_MAX_UNITS, com.oddlabs.tt.form.TerrainMenu.MAX_MAX_UNITS) / ((float)(com.oddlabs.tt.form.TerrainMenu.UNIT_COUNT_STEP)))));
            pm_max_chiefs_total.chooseItem(indexForMaxChiefsTotal(world.getMaxChieftains()));
            pm_max_chiefs_per_q.chooseItem(indexForMaxChiefsPerQ(world.getMaxChieftainsPerQuarters()));
            pm_max_buildings.chooseItem(indexForMaxBuildings(world.getMaxBuildingCount()));
            pm_chief_heal_idle.chooseItem(indexForChiefHealIdle(world.getChieftainHealIdleSeconds()));
            pm_chief_heal_amount.chooseItem(indexForChiefHealAmount(world.getChieftainHealAmount()));
            pm_target_quarters.chooseItem(indexForTargetQuarters(world.getTargetNumQuarters()));
            pm_target_armories.chooseItem(indexForTargetArmories(world.getTargetNumArmories()));
            pm_max_concur_quarters.chooseItem(indexForMaxConcurQuarters(world.getMaxConcurrentQuarters()));
            pm_max_concur_armories.chooseItem(indexForMaxConcurArmories(world.getMaxConcurrentArmories()));
            pm_num_res_towers.chooseItem(indexForNumResTowers(world.getNumResourceTowers()));
            pm_max_concur_towers.chooseItem(indexForMaxConcurTowers(world.getMaxConcurrentTowers()));
            pm_koth_statue_count.chooseItem(java.lang.Math.clamp(world.getKothStatueCount(), com.oddlabs.tt.landscape.WorldParameters.MIN_KOTH_STATUE_COUNT, com.oddlabs.tt.landscape.WorldParameters.MAX_KOTH_STATUE_COUNT) - com.oddlabs.tt.landscape.WorldParameters.MIN_KOTH_STATUE_COUNT);
            slider_starting_rock_warriors.setValue(java.lang.Math.clamp(world.getStartingRockWarriors(), com.oddlabs.tt.form.TerrainMenu.MIN_STARTING_WARRIORS, com.oddlabs.tt.form.TerrainMenu.MAX_STARTING_WARRIORS));
            slider_starting_iron_warriors.setValue(java.lang.Math.clamp(world.getStartingIronWarriors(), com.oddlabs.tt.form.TerrainMenu.MIN_STARTING_WARRIORS, com.oddlabs.tt.form.TerrainMenu.MAX_STARTING_WARRIORS));
            slider_starting_rubber_warriors.setValue(java.lang.Math.clamp(world.getStartingRubberWarriors(), com.oddlabs.tt.form.TerrainMenu.MIN_STARTING_WARRIORS, com.oddlabs.tt.form.TerrainMenu.MAX_STARTING_WARRIORS));
            cb_magic1_enabled.setMarked(world.isMagic1Enabled());
            cb_magic2_enabled.setMarked(world.isMagic2Enabled());
            cb_magic3_enabled.setMarked(world.isMagic3Enabled());
            cb_chiefs_courage_enabled.setMarked(world.isChiefsCourageEnabled());
            cb_adaptive_ai_enabled.setMarked(world.isAdaptiveAiEnabled());
            cb_team_together.setMarked(world.isTeamTogether());
            cb_free_form.setMarked(world.isFreeForm());
            cb_extended.setMarked(world.isExtended());
            cb_weather.setMarked(world.isWeather());
            slider_magic1_cost.setValue(java.lang.Math.round(java.lang.Math.clamp(world.getMagic1Cost(), com.oddlabs.tt.form.TerrainMenu.MIN_MAGIC_COST, com.oddlabs.tt.form.TerrainMenu.MAX_MAGIC_COST)));
            slider_magic2_cost.setValue(java.lang.Math.round(java.lang.Math.clamp(world.getMagic2Cost(), com.oddlabs.tt.form.TerrainMenu.MIN_MAGIC_COST, com.oddlabs.tt.form.TerrainMenu.MAX_MAGIC_COST)));
            slider_magic3_cost.setValue(java.lang.Math.round(java.lang.Math.clamp(world.getMagic3Cost(), com.oddlabs.tt.form.TerrainMenu.MIN_MAGIC_COST, com.oddlabs.tt.form.TerrainMenu.MAX_MAGIC_COST)));
            for (int m_5 = com.oddlabs.tt.model.RacesResources.NUM_CLASSIC_MAGIC; m_5 < com.oddlabs.tt.model.RacesResources.NUM_MAGIC; m_5++)
            {
                float c_5 = world.getExtraMagicCost(m_5);
                slider_extra_magic_cost[m_5].setValue(java.lang.Math.round(java.lang.Math.clamp((c_5 > 0.0f ? c_5 : com.oddlabs.tt.model.Unit.MAX_MAGIC_ENERGY[m_5]), com.oddlabs.tt.form.TerrainMenu.MIN_MAGIC_COST, com.oddlabs.tt.form.TerrainMenu.MAX_MAGIC_COST)));
                cb_extra_magic_enabled[m_5].setMarked(world.isExtraMagicEnabled(m_5));
            }
            slider_building_health_mult.setValue(java.lang.Math.round(java.lang.Math.clamp((float)(world.getBuildingHealthMultiplier() * 100.0f), com.oddlabs.tt.form.TerrainMenu.MIN_HEALTH_MULT_PERCENT, com.oddlabs.tt.form.TerrainMenu.MAX_HEALTH_MULT_PERCENT)));
            slider_viking_chief_health_mult.setValue(java.lang.Math.round(java.lang.Math.clamp((float)(world.getVikingChiefHealthMultiplier() * 100.0f), com.oddlabs.tt.form.TerrainMenu.MIN_HEALTH_MULT_PERCENT, com.oddlabs.tt.form.TerrainMenu.MAX_HEALTH_MULT_PERCENT)));
            slider_native_chief_health_mult.setValue(java.lang.Math.round(java.lang.Math.clamp((float)(world.getNativeChiefHealthMultiplier() * 100.0f), com.oddlabs.tt.form.TerrainMenu.MIN_HEALTH_MULT_PERCENT, com.oddlabs.tt.form.TerrainMenu.MAX_HEALTH_MULT_PERCENT)));
            slider_unit_range_mult.setValue(java.lang.Math.round(java.lang.Math.clamp((float)(world.getUnitRangeMultiplier() * 100.0f), com.oddlabs.tt.form.TerrainMenu.MIN_HEALTH_MULT_PERCENT, com.oddlabs.tt.form.TerrainMenu.MAX_HEALTH_MULT_PERCENT)));
            slider_armory_resource_cap.setValue(java.lang.Math.clamp(world.getArmoryResourceCap(), com.oddlabs.tt.form.TerrainMenu.MIN_RESOURCE_CAP, com.oddlabs.tt.form.TerrainMenu.MAX_RESOURCE_CAP));
            slider_rock_resource_cap.setValue(java.lang.Math.clamp(world.getRockResourceCap(), com.oddlabs.tt.form.TerrainMenu.MIN_RESOURCE_CAP, com.oddlabs.tt.form.TerrainMenu.MAX_RESOURCE_CAP));
            slider_iron_resource_cap.setValue(java.lang.Math.clamp(world.getIronResourceCap(), com.oddlabs.tt.form.TerrainMenu.MIN_RESOURCE_CAP, com.oddlabs.tt.form.TerrainMenu.MAX_RESOURCE_CAP));
            slider_rubber_resource_cap.setValue(java.lang.Math.clamp(world.getRubberResourceCap(), com.oddlabs.tt.form.TerrainMenu.MIN_RESOURCE_CAP, com.oddlabs.tt.form.TerrainMenu.MAX_RESOURCE_CAP));
            setMapcode();
        }
        // Java: 2570
        public int indexForMaxChiefsTotal(int v)
        {
            return ((v) switch { 1 => (int)(0), 3 => (int)(1), 5 => (int)(2), 10 => (int)(3), 15 => (int)(4), 20 => (int)(5), 9999 => (int)(6), _ => (int)(2) });
        }
        // Java: 2583
        public int indexForMaxChiefsPerQ(int v)
        {
            return ((v) switch { 1 => (int)(0), 3 => (int)(1), 5 => (int)(2), 10 => (int)(3), 15 => (int)(4), 20 => (int)(5), 9999 => (int)(6), _ => (int)(0) });
        }
        // Java: 2596
        public int indexForMaxBuildings(int v)
        {
            return ((v) switch { 20 => (int)(0), 50 => (int)(1), 100 => (int)(2), 500 => (int)(3), 1200 => (int)(4), 9999 => (int)(5), _ => (int)(4) });
        }
        // Java: 2608
        public int indexForChiefHealIdle(float v)
        {
            if (v == 1.0f)
            {
                return 0;
            }
            if (v == 5.0f)
            {
                return 2;
            }
            if (v == 10.0f)
            {
                return 3;
            }
            if (v == 20.0f)
            {
                return 4;
            }
            return 1;
        }
        // Java: 2616
        public int indexForChiefHealAmount(int v)
        {
            return ((v) switch { 10 => (int)(0), 25 => (int)(1), 50 => (int)(2), 100 => (int)(3), 150 => (int)(4), 250 => (int)(5), _ => (int)(2) });
        }
        // Java: 2628
        public int indexForTargetQuarters(int v)
        {
            return ((v) switch { 1 => (int)(0), 2 => (int)(1), 3 => (int)(2), 5 => (int)(3), 7 => (int)(4), 10 => (int)(5), 12 => (int)(6), _ => (int)(2) });
        }
        // Java: 2641
        public int indexForTargetArmories(int v)
        {
            return ((v) switch { 1 => (int)(0), 2 => (int)(1), 3 => (int)(2), 4 => (int)(3), 5 => (int)(4), 6 => (int)(5), _ => (int)(0) });
        }
        // Java: 2653
        public int indexForMaxConcurQuarters(int v)
        {
            return ((v) switch { 1 => (int)(0), 2 => (int)(1), 3 => (int)(2), 4 => (int)(3), 5 => (int)(4), _ => (int)(1) });
        }
        // Java: 2664
        public int indexForMaxConcurArmories(int v)
        {
            return ((v) switch { 2 => (int)(1), 3 => (int)(2), _ => (int)(0) });
        }
        // Java: 2672
        public int indexForNumResTowers(int v)
        {
            return ((v) switch { 0 => (int)(0), 1 => (int)(1), 3 => (int)(2), 5 => (int)(3), 7 => (int)(4), 10 => (int)(5), 15 => (int)(6), 20 => (int)(7), 30 => (int)(8), _ => (int)(2) });
        }
        // Java: 2687
        public int indexForMaxConcurTowers(int v)
        {
            return ((v) switch { 1 => (int)(0), 3 => (int)(1), 5 => (int)(2), 7 => (int)(3), 10 => (int)(4), 15 => (int)(5), 9999 => (int)(6), _ => (int)(2) });
        }
        // Java: 2700
        public static int clampIndex(int index, int size)
        {
            if (index < 0)
            {
                return 0;
            }
            return java.lang.Math.min(index, size - 1);
        }
        // Java: 2707
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
        // Java: 2739
        public com.oddlabs.matchmaking.RosterTemplate.Fill difficultyIndexToFill(int slot_index, int difficulty_index)
        {
            if (slot_index == 0)
            {
                return com.oddlabs.matchmaking.RosterTemplate.Fill.HOST;
            }
            if (multiplayer)
            {
                return ((difficulty_index) switch { 1 => (com.oddlabs.matchmaking.RosterTemplate.Fill)(com.oddlabs.matchmaking.RosterTemplate.Fill.CLOSED), 2 => (com.oddlabs.matchmaking.RosterTemplate.Fill)(com.oddlabs.matchmaking.RosterTemplate.Fill.EASY_AI), 3 => (com.oddlabs.matchmaking.RosterTemplate.Fill)(com.oddlabs.matchmaking.RosterTemplate.Fill.NORMAL_AI), 4 => (com.oddlabs.matchmaking.RosterTemplate.Fill)(com.oddlabs.matchmaking.RosterTemplate.Fill.HARD_AI), 5 => (com.oddlabs.matchmaking.RosterTemplate.Fill)(com.oddlabs.matchmaking.RosterTemplate.Fill.EXPERT_AI), 6 => (com.oddlabs.matchmaking.RosterTemplate.Fill)(com.oddlabs.matchmaking.RosterTemplate.Fill.GAUNTLET_AI), _ => (com.oddlabs.matchmaking.RosterTemplate.Fill)(com.oddlabs.matchmaking.RosterTemplate.Fill.OPEN) });
            }
            return ((difficulty_index) switch { 1 => (com.oddlabs.matchmaking.RosterTemplate.Fill)(com.oddlabs.matchmaking.RosterTemplate.Fill.EASY_AI), 2 => (com.oddlabs.matchmaking.RosterTemplate.Fill)(com.oddlabs.matchmaking.RosterTemplate.Fill.NORMAL_AI), 3 => (com.oddlabs.matchmaking.RosterTemplate.Fill)(com.oddlabs.matchmaking.RosterTemplate.Fill.HARD_AI), 4 => (com.oddlabs.matchmaking.RosterTemplate.Fill)(com.oddlabs.matchmaking.RosterTemplate.Fill.EXPERT_AI), 5 => (com.oddlabs.matchmaking.RosterTemplate.Fill)(com.oddlabs.matchmaking.RosterTemplate.Fill.GAUNTLET_AI), _ => (com.oddlabs.matchmaking.RosterTemplate.Fill)(com.oddlabs.matchmaking.RosterTemplate.Fill.CLOSED) });
        }
        // Java: 2771
        public static int fillToPlayerSlotAI(com.oddlabs.matchmaking.RosterTemplate.Fill fill)
        {
            return ((fill).ordinal() switch { 3 /*EASY_AI*/ => (int)(com.oddlabs.tt.net.PlayerSlot.AI_EASY), 4 /*NORMAL_AI*/ => (int)(com.oddlabs.tt.net.PlayerSlot.AI_NORMAL), 5 /*HARD_AI*/ => (int)(com.oddlabs.tt.net.PlayerSlot.AI_HARD), 6 /*EXPERT_AI*/ => (int)(com.oddlabs.tt.net.PlayerSlot.AI_EXPERT), 7 /*ULTRA_AI*/ or 8 /*FABLE_AI*/ or 9 /*GAUNTLET_AI*/ => (int)(com.oddlabs.tt.net.PlayerSlot.AI_GAUNTLET), 0 /*HOST*/ or 1 /*OPEN*/ or 2 /*CLOSED*/ => (int)(com.oddlabs.tt.net.PlayerSlot.AI_NONE), _ => throw new System.InvalidOperationException("MatchException") });
        }
        // Java: 2787
        public void applyPreset(com.oddlabs.matchmaking.Preset preset)
        {
            applyWorldConfig(preset.getWorld());
            com.oddlabs.matchmaking.RosterTemplate.Slot[] slots = preset.getRoster().getSlots();
            int target_count = java.lang.Math.min(slots.Length, com.oddlabs.matchmaking.MatchmakingServerInterface.MAX_PLAYERS);
            if (target_count >= com.oddlabs.tt.form.TerrainMenu.DEFAULT_PLAYER_COUNT && target_count != player_count)
            {
                pulldown_menu_slots.chooseItem(target_count - com.oddlabs.tt.form.TerrainMenu.DEFAULT_PLAYER_COUNT);
            }
            if ((preset.getModeOptions() is com.oddlabs.matchmaking.StandardOptions opts))
            {
                cb_rated.setMarked(opts.isRated());
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
        // Java: 2819
        public int fillToDifficultyIndex(com.oddlabs.matchmaking.RosterTemplate.Fill fill, int slot_index)
        {
            if (slot_index == 0)
            {
                return 0;
            }
            if (multiplayer)
            {
                return ((fill).ordinal() switch { 0 /*HOST*/ or 1 /*OPEN*/ => (int)(0), 2 /*CLOSED*/ => (int)(1), 3 /*EASY_AI*/ => (int)(2), 4 /*NORMAL_AI*/ => (int)(3), 5 /*HARD_AI*/ => (int)(4), 6 /*EXPERT_AI*/ => (int)(5), 7 /*ULTRA_AI*/ or 8 /*FABLE_AI*/ or 9 /*GAUNTLET_AI*/ => (int)(6), _ => throw new System.InvalidOperationException("MatchException") });
            }
            return ((fill).ordinal() switch { 0 /*HOST*/ or 1 /*OPEN*/ or 2 /*CLOSED*/ => (int)(0), 3 /*EASY_AI*/ => (int)(1), 4 /*NORMAL_AI*/ => (int)(2), 5 /*HARD_AI*/ => (int)(3), 6 /*EXPERT_AI*/ => (int)(4), 7 /*ULTRA_AI*/ or 8 /*FABLE_AI*/ or 9 /*GAUNTLET_AI*/ => (int)(5), _ => throw new System.InvalidOperationException("MatchException") });
        }
        // Java: 2854
        public class ExtraSettingsForm : com.oddlabs.tt.gui.Form
        {
            public ExtraSettingsForm(string caption, params com.oddlabs.tt.gui.Panel[] panels) : base(caption)
            {
                com.oddlabs.tt.gui.PanelGroup group = new com.oddlabs.tt.gui.PanelGroup(panels);
                addChild(group);
                com.oddlabs.tt.gui.HorizButton button_close = new com.oddlabs.tt.gui.OKButton(com.oddlabs.tt.form.TerrainMenu.BUTTON_WIDTH);
                button_close.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
                {
                    cancel();
                }));
                addChild(button_close);
                group.place();
                button_close.place(group, com.oddlabs.tt.gui.Placement.BOTTOM_RIGHT);
                compileCanvas();
                centerPos();
            }
        }
        // Java: 2871
        public int editorMapSizeIndex()
        {
            if (selected_editor_map != null)
            {
                for (int i = 0; i < com.oddlabs.tt.form.TerrainMenu.SIZES.Length; i++)
                {
                    if (com.oddlabs.tt.form.TerrainMenu.SIZES[i] == selected_editor_map.getMetersPerWorld() && com.oddlabs.tt.form.TerrainMenu.ARCHIPELAGO[i] == selected_editor_map.isArchipelago())
                    {
                        return i;
                    }
                }
            }
            return pulldown_size.getChosenItemIndex();
        }
        // Java: 2880
        public class PulldownUpdatePlayersChangedListener : java.lang.Object, com.oddlabs.tt.guievent.ItemChosenListener
        {
            public com.oddlabs.tt.form.TerrainMenu this_0;
            public com.oddlabs.tt.gui.Panel standard;
            public com.oddlabs.tt.gui.ScrollableGroup current_race_team;
            // Java: 2888
            public void setCurrentGroup(com.oddlabs.tt.gui.ScrollableGroup group)
            {
                this.current_race_team = group;
            }
            // Java: 2892
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
                    new_group.place(this_0.group_set_all_difficulty, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT, com.oddlabs.tt.gui.Skin.getSkin().getFormData().sectionSpacing());
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
                        if (!this_0.multiplayer)
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
            TerrainMenu.SIZES = new int[] { 256, 512, 1024, 2048, 4096, 2048 };
            TerrainMenu.ARCHIPELAGO = new bool[] { false, false, false, false, false, true };
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
            this.selected_mode = com.oddlabs.matchmaking.GameMode.STANDARD;
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
            preset_library.load(com.oddlabs.tt.render.Renderer.getLocalInput().getGameDir().resolve(com.oddlabs.tt.global.Globals.getPresetsFileName()));
            mode_and_presets = new com.oddlabs.tt.form.ModeAndPresetsPanel(gui_root, preset_library, new com.oddlabs.tt.form.TerrainMenu.PresetsHandler(this));
            com.oddlabs.tt.gui.Panel standard = new com.oddlabs.tt.gui.Panel(i18n("standard_options"));
            com.oddlabs.tt.gui.Panel advanced = new com.oddlabs.tt.gui.Panel(i18n("advanced_options"));
            com.oddlabs.tt.gui.Panel custom_options = new com.oddlabs.tt.gui.Panel(i18n("custom_options"));
            com.oddlabs.tt.gui.Panel economy_options = new com.oddlabs.tt.gui.Panel(i18n("economy_options"));
            com.oddlabs.tt.gui.Panel units_options = new com.oddlabs.tt.gui.Panel(i18n("units_options"));
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
            com.oddlabs.tt.gui.Group group_advanced_settings = new com.oddlabs.tt.gui.Group();
            com.oddlabs.tt.gui.Group group_max_chiefs_per_q = new com.oddlabs.tt.gui.Group();
            com.oddlabs.tt.gui.Label label_max_chiefs_per_q = new com.oddlabs.tt.gui.Label(i18n("max_chiefs_per_q"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_max_chiefs_per_q.addChild(label_max_chiefs_per_q);
            pm_max_chiefs_per_q = new com.oddlabs.tt.gui.PulldownMenu();
            pm_max_chiefs_per_q.addItem(new com.oddlabs.tt.gui.PulldownItem("1"));
            pm_max_chiefs_per_q.addItem(new com.oddlabs.tt.gui.PulldownItem("3"));
            pm_max_chiefs_per_q.addItem(new com.oddlabs.tt.gui.PulldownItem("5"));
            pm_max_chiefs_per_q.addItem(new com.oddlabs.tt.gui.PulldownItem("10"));
            pm_max_chiefs_per_q.addItem(new com.oddlabs.tt.gui.PulldownItem("15"));
            pm_max_chiefs_per_q.addItem(new com.oddlabs.tt.gui.PulldownItem("30"));
            pm_max_chiefs_per_q.addItem(new com.oddlabs.tt.gui.PulldownItem(i18n("unlimited")));
            com.oddlabs.tt.gui.PulldownButton pb_max_chiefs_per_q = new com.oddlabs.tt.gui.PulldownButton(gui_root, pm_max_chiefs_per_q, 6, 150);
            pm_max_chiefs_per_q.addItemChosenListener(new com.oddlabs.tt.guievent.ItemChosenListener_L((com.oddlabs.tt.gui.PulldownMenu unused, int unused_2) =>
            {
                markModified();
            }));
            group_max_chiefs_per_q.addChild(pb_max_chiefs_per_q);
            label_max_chiefs_per_q.place();
            pb_max_chiefs_per_q.place(label_max_chiefs_per_q, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            group_max_chiefs_per_q.compileCanvas();
            group_advanced_settings.addChild(group_max_chiefs_per_q);
            com.oddlabs.tt.gui.Group group_max_chiefs_total = new com.oddlabs.tt.gui.Group();
            com.oddlabs.tt.gui.Label label_max_chiefs_total = new com.oddlabs.tt.gui.Label(i18n("max_chiefs_total"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_max_chiefs_total.addChild(label_max_chiefs_total);
            pm_max_chiefs_total = new com.oddlabs.tt.gui.PulldownMenu();
            pm_max_chiefs_total.addItem(new com.oddlabs.tt.gui.PulldownItem("1"));
            pm_max_chiefs_total.addItem(new com.oddlabs.tt.gui.PulldownItem("3"));
            pm_max_chiefs_total.addItem(new com.oddlabs.tt.gui.PulldownItem("5"));
            pm_max_chiefs_total.addItem(new com.oddlabs.tt.gui.PulldownItem("10"));
            pm_max_chiefs_total.addItem(new com.oddlabs.tt.gui.PulldownItem("15"));
            pm_max_chiefs_total.addItem(new com.oddlabs.tt.gui.PulldownItem("30"));
            pm_max_chiefs_total.addItem(new com.oddlabs.tt.gui.PulldownItem(i18n("unlimited")));
            com.oddlabs.tt.gui.PulldownButton pb_max_chiefs_total = new com.oddlabs.tt.gui.PulldownButton(gui_root, pm_max_chiefs_total, 0, 150);
            pm_max_chiefs_total.addItemChosenListener(new com.oddlabs.tt.guievent.ItemChosenListener_L((com.oddlabs.tt.gui.PulldownMenu unused_3, int unused_4) =>
            {
                markModified();
            }));
            group_max_chiefs_total.addChild(pb_max_chiefs_total);
            label_max_chiefs_total.place();
            pb_max_chiefs_total.place(label_max_chiefs_total, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            group_max_chiefs_total.compileCanvas();
            group_advanced_settings.addChild(group_max_chiefs_total);
            com.oddlabs.tt.gui.Group group_starting_warriors = new com.oddlabs.tt.gui.Group();
            com.oddlabs.tt.gui.Label label_starting_warriors_header = new com.oddlabs.tt.gui.Label(i18n("starting_units_header"), com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont());
            group_starting_warriors.addChild(label_starting_warriors_header);
            label_starting_warriors_header.place();
            com.oddlabs.tt.gui.Label label_initial_units = new com.oddlabs.tt.gui.Label(i18n("initial_units"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_starting_warriors.addChild(label_initial_units);
            slider_initial_units = new com.oddlabs.tt.gui.Slider(com.oddlabs.tt.form.TerrainMenu.SETTING_SLIDER_LENGTH, com.oddlabs.tt.form.TerrainMenu.MIN_INITIAL_UNITS / com.oddlabs.tt.form.TerrainMenu.UNIT_COUNT_STEP, com.oddlabs.tt.form.TerrainMenu.MAX_INITIAL_UNITS / com.oddlabs.tt.form.TerrainMenu.UNIT_COUNT_STEP, com.oddlabs.tt.form.TerrainMenu.DEFAULT_INITIAL_UNITS / com.oddlabs.tt.form.TerrainMenu.UNIT_COUNT_STEP);
            label_initial_units_value = new com.oddlabs.tt.gui.Label(java.lang.Integer.toString(com.oddlabs.tt.form.TerrainMenu.DEFAULT_INITIAL_UNITS), com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), com.oddlabs.tt.form.TerrainMenu.VALUE_LABEL_WIDTH);
            slider_initial_units.addValueListener(new com.oddlabs.tt.guievent.ValueListener_L((long @value) =>
            {
                label_initial_units_value.setText(java.lang.Long.toString(@value * com.oddlabs.tt.form.TerrainMenu.UNIT_COUNT_STEP));
                markModified();
            }));
            group_starting_warriors.addChild(slider_initial_units);
            group_starting_warriors.addChild(label_initial_units_value);
            label_initial_units.place(label_starting_warriors_header, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            slider_initial_units.place(label_initial_units, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            label_initial_units_value.place(slider_initial_units, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            com.oddlabs.tt.gui.Label label_max_units = new com.oddlabs.tt.gui.Label(i18n("max_units"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_starting_warriors.addChild(label_max_units);
            slider_max_units = new com.oddlabs.tt.gui.Slider(com.oddlabs.tt.form.TerrainMenu.SETTING_SLIDER_LENGTH, com.oddlabs.tt.form.TerrainMenu.MIN_MAX_UNITS / com.oddlabs.tt.form.TerrainMenu.UNIT_COUNT_STEP, com.oddlabs.tt.form.TerrainMenu.MAX_MAX_UNITS / com.oddlabs.tt.form.TerrainMenu.UNIT_COUNT_STEP, com.oddlabs.tt.form.TerrainMenu.defaultMaxUnits() / com.oddlabs.tt.form.TerrainMenu.UNIT_COUNT_STEP);
            label_max_units_value = new com.oddlabs.tt.gui.Label(java.lang.Integer.toString(com.oddlabs.tt.form.TerrainMenu.defaultMaxUnits()), com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), com.oddlabs.tt.form.TerrainMenu.VALUE_LABEL_WIDTH);
            slider_max_units.addValueListener(new com.oddlabs.tt.guievent.ValueListener_L((long value_2) =>
            {
                label_max_units_value.setText(java.lang.Long.toString(value_2 * com.oddlabs.tt.form.TerrainMenu.UNIT_COUNT_STEP));
                markModified();
            }));
            group_starting_warriors.addChild(slider_max_units);
            group_starting_warriors.addChild(label_max_units_value);
            label_max_units.place(label_initial_units, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            slider_max_units.place(label_max_units, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            label_max_units_value.place(slider_max_units, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            com.oddlabs.tt.gui.Label label_starting_rock_warriors = new com.oddlabs.tt.gui.Label(i18n("starting_rock_warriors"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_starting_warriors.addChild(label_starting_rock_warriors);
            slider_starting_rock_warriors = new com.oddlabs.tt.gui.Slider(com.oddlabs.tt.form.TerrainMenu.SETTING_SLIDER_LENGTH, com.oddlabs.tt.form.TerrainMenu.MIN_STARTING_WARRIORS, com.oddlabs.tt.form.TerrainMenu.MAX_STARTING_WARRIORS, com.oddlabs.tt.form.TerrainMenu.DEFAULT_STARTING_WARRIORS);
            label_starting_rock_warriors_value = new com.oddlabs.tt.gui.Label(java.lang.Integer.toString(com.oddlabs.tt.form.TerrainMenu.DEFAULT_STARTING_WARRIORS), com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), com.oddlabs.tt.form.TerrainMenu.VALUE_LABEL_WIDTH);
            slider_starting_rock_warriors.addValueListener(new com.oddlabs.tt.guievent.ValueListener_L((long value_3) =>
            {
                label_starting_rock_warriors_value.setText(java.lang.Long.toString(value_3));
                markModified();
            }));
            group_starting_warriors.addChild(slider_starting_rock_warriors);
            group_starting_warriors.addChild(label_starting_rock_warriors_value);
            label_starting_rock_warriors.place(label_max_units, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            slider_starting_rock_warriors.place(label_starting_rock_warriors, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            label_starting_rock_warriors_value.place(slider_starting_rock_warriors, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            com.oddlabs.tt.gui.Label label_starting_iron_warriors = new com.oddlabs.tt.gui.Label(i18n("starting_iron_warriors"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_starting_warriors.addChild(label_starting_iron_warriors);
            slider_starting_iron_warriors = new com.oddlabs.tt.gui.Slider(com.oddlabs.tt.form.TerrainMenu.SETTING_SLIDER_LENGTH, com.oddlabs.tt.form.TerrainMenu.MIN_STARTING_WARRIORS, com.oddlabs.tt.form.TerrainMenu.MAX_STARTING_WARRIORS, com.oddlabs.tt.form.TerrainMenu.DEFAULT_STARTING_WARRIORS);
            label_starting_iron_warriors_value = new com.oddlabs.tt.gui.Label(java.lang.Integer.toString(com.oddlabs.tt.form.TerrainMenu.DEFAULT_STARTING_WARRIORS), com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), com.oddlabs.tt.form.TerrainMenu.VALUE_LABEL_WIDTH);
            slider_starting_iron_warriors.addValueListener(new com.oddlabs.tt.guievent.ValueListener_L((long value_4) =>
            {
                label_starting_iron_warriors_value.setText(java.lang.Long.toString(value_4));
                markModified();
            }));
            group_starting_warriors.addChild(slider_starting_iron_warriors);
            group_starting_warriors.addChild(label_starting_iron_warriors_value);
            label_starting_iron_warriors.place(label_starting_rock_warriors, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            slider_starting_iron_warriors.place(label_starting_iron_warriors, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            label_starting_iron_warriors_value.place(slider_starting_iron_warriors, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            com.oddlabs.tt.gui.Label label_starting_rubber_warriors = new com.oddlabs.tt.gui.Label(i18n("starting_rubber_warriors"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_starting_warriors.addChild(label_starting_rubber_warriors);
            slider_starting_rubber_warriors = new com.oddlabs.tt.gui.Slider(com.oddlabs.tt.form.TerrainMenu.SETTING_SLIDER_LENGTH, com.oddlabs.tt.form.TerrainMenu.MIN_STARTING_WARRIORS, com.oddlabs.tt.form.TerrainMenu.MAX_STARTING_WARRIORS, com.oddlabs.tt.form.TerrainMenu.DEFAULT_STARTING_WARRIORS);
            label_starting_rubber_warriors_value = new com.oddlabs.tt.gui.Label(java.lang.Integer.toString(com.oddlabs.tt.form.TerrainMenu.DEFAULT_STARTING_WARRIORS), com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), com.oddlabs.tt.form.TerrainMenu.VALUE_LABEL_WIDTH);
            slider_starting_rubber_warriors.addValueListener(new com.oddlabs.tt.guievent.ValueListener_L((long value_5) =>
            {
                label_starting_rubber_warriors_value.setText(java.lang.Long.toString(value_5));
                markModified();
            }));
            group_starting_warriors.addChild(slider_starting_rubber_warriors);
            group_starting_warriors.addChild(label_starting_rubber_warriors_value);
            label_starting_rubber_warriors.place(label_starting_iron_warriors, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            slider_starting_rubber_warriors.place(label_starting_rubber_warriors, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            label_starting_rubber_warriors_value.place(slider_starting_rubber_warriors, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            group_starting_warriors.compileCanvas();
            if (multiplayer)
            {
                units_options.addChild(group_starting_warriors);
            }
            else
            {
                group_map_options.addChild(group_starting_warriors);
            }
            com.oddlabs.tt.gui.Group group_max_buildings = new com.oddlabs.tt.gui.Group();
            com.oddlabs.tt.gui.Label label_max_buildings = new com.oddlabs.tt.gui.Label(i18n("max_buildings"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_max_buildings.addChild(label_max_buildings);
            pm_max_buildings = new com.oddlabs.tt.gui.PulldownMenu();
            pm_max_buildings.addItem(new com.oddlabs.tt.gui.PulldownItem("20"));
            pm_max_buildings.addItem(new com.oddlabs.tt.gui.PulldownItem("40"));
            pm_max_buildings.addItem(new com.oddlabs.tt.gui.PulldownItem("60"));
            pm_max_buildings.addItem(new com.oddlabs.tt.gui.PulldownItem("100"));
            pm_max_buildings.addItem(new com.oddlabs.tt.gui.PulldownItem("200"));
            pm_max_buildings.addItem(new com.oddlabs.tt.gui.PulldownItem(i18n("unlimited")));
            com.oddlabs.tt.gui.PulldownButton pb_max_buildings = new com.oddlabs.tt.gui.PulldownButton(gui_root, pm_max_buildings, 0, 150);
            pm_max_buildings.addItemChosenListener(new com.oddlabs.tt.guievent.ItemChosenListener_L((com.oddlabs.tt.gui.PulldownMenu unused_5, int unused_6) =>
            {
                markModified();
            }));
            group_max_buildings.addChild(pb_max_buildings);
            label_max_buildings.place();
            pb_max_buildings.place(label_max_buildings, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            group_max_buildings.compileCanvas();
            if (multiplayer)
            {
                units_options.addChild(group_max_buildings);
            }
            else
            {
                group_map_options.addChild(group_max_buildings);
            }
            com.oddlabs.tt.gui.Group group_chief_heal_idle = new com.oddlabs.tt.gui.Group();
            com.oddlabs.tt.gui.Label label_chief_heal_idle = new com.oddlabs.tt.gui.Label(i18n("chief_heal_idle_seconds"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_chief_heal_idle.addChild(label_chief_heal_idle);
            pm_chief_heal_idle = new com.oddlabs.tt.gui.PulldownMenu();
            pm_chief_heal_idle.addItem(new com.oddlabs.tt.gui.PulldownItem("10"));
            pm_chief_heal_idle.addItem(new com.oddlabs.tt.gui.PulldownItem("20"));
            pm_chief_heal_idle.addItem(new com.oddlabs.tt.gui.PulldownItem("30"));
            pm_chief_heal_idle.addItem(new com.oddlabs.tt.gui.PulldownItem("60"));
            pm_chief_heal_idle.addItem(new com.oddlabs.tt.gui.PulldownItem("120"));
            com.oddlabs.tt.gui.PulldownButton pb_chief_heal_idle = new com.oddlabs.tt.gui.PulldownButton(gui_root, pm_chief_heal_idle, 2, 150);
            pm_chief_heal_idle.addItemChosenListener(new com.oddlabs.tt.guievent.ItemChosenListener_L((com.oddlabs.tt.gui.PulldownMenu unused_7, int unused_8) =>
            {
                markModified();
            }));
            group_chief_heal_idle.addChild(pb_chief_heal_idle);
            label_chief_heal_idle.place();
            pb_chief_heal_idle.place(label_chief_heal_idle, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            group_chief_heal_idle.compileCanvas();
            group_advanced_settings.addChild(group_chief_heal_idle);
            com.oddlabs.tt.gui.Group group_chief_heal_amount = new com.oddlabs.tt.gui.Group();
            com.oddlabs.tt.gui.Label label_chief_heal_amount = new com.oddlabs.tt.gui.Label(i18n("chief_heal_amount"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_chief_heal_amount.addChild(label_chief_heal_amount);
            pm_chief_heal_amount = new com.oddlabs.tt.gui.PulldownMenu();
            pm_chief_heal_amount.addItem(new com.oddlabs.tt.gui.PulldownItem("1"));
            pm_chief_heal_amount.addItem(new com.oddlabs.tt.gui.PulldownItem("3"));
            pm_chief_heal_amount.addItem(new com.oddlabs.tt.gui.PulldownItem("5"));
            pm_chief_heal_amount.addItem(new com.oddlabs.tt.gui.PulldownItem("10"));
            pm_chief_heal_amount.addItem(new com.oddlabs.tt.gui.PulldownItem("15"));
            pm_chief_heal_amount.addItem(new com.oddlabs.tt.gui.PulldownItem("30"));
            com.oddlabs.tt.gui.PulldownButton pb_chief_heal_amount = new com.oddlabs.tt.gui.PulldownButton(gui_root, pm_chief_heal_amount, 2, 150);
            pm_chief_heal_amount.addItemChosenListener(new com.oddlabs.tt.guievent.ItemChosenListener_L((com.oddlabs.tt.gui.PulldownMenu unused_9, int unused_10) =>
            {
                markModified();
            }));
            group_chief_heal_amount.addChild(pb_chief_heal_amount);
            label_chief_heal_amount.place();
            pb_chief_heal_amount.place(label_chief_heal_amount, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            group_chief_heal_amount.compileCanvas();
            group_advanced_settings.addChild(group_chief_heal_amount);
            com.oddlabs.tt.gui.Group group_target_quarters = new com.oddlabs.tt.gui.Group();
            com.oddlabs.tt.gui.Label label_target_quarters = new com.oddlabs.tt.gui.Label(i18n("target_quarters"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_target_quarters.addChild(label_target_quarters);
            pm_target_quarters = new com.oddlabs.tt.gui.PulldownMenu();
            pm_target_quarters.addItem(new com.oddlabs.tt.gui.PulldownItem("1"));
            pm_target_quarters.addItem(new com.oddlabs.tt.gui.PulldownItem("2"));
            pm_target_quarters.addItem(new com.oddlabs.tt.gui.PulldownItem("3"));
            pm_target_quarters.addItem(new com.oddlabs.tt.gui.PulldownItem("4"));
            pm_target_quarters.addItem(new com.oddlabs.tt.gui.PulldownItem("5"));
            pm_target_quarters.addItem(new com.oddlabs.tt.gui.PulldownItem("7"));
            pm_target_quarters.addItem(new com.oddlabs.tt.gui.PulldownItem("12"));
            com.oddlabs.tt.gui.PulldownButton pb_target_quarters = new com.oddlabs.tt.gui.PulldownButton(gui_root, pm_target_quarters, 1, 150);
            pm_target_quarters.addItemChosenListener(new com.oddlabs.tt.guievent.ItemChosenListener_L((com.oddlabs.tt.gui.PulldownMenu unused_11, int unused_12) =>
            {
                markModified();
            }));
            group_target_quarters.addChild(pb_target_quarters);
            label_target_quarters.place();
            pb_target_quarters.place(label_target_quarters, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            group_target_quarters.compileCanvas();
            group_advanced_settings.addChild(group_target_quarters);
            com.oddlabs.tt.gui.Group group_target_armories = new com.oddlabs.tt.gui.Group();
            com.oddlabs.tt.gui.Label label_target_armories = new com.oddlabs.tt.gui.Label(i18n("target_armories"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_target_armories.addChild(label_target_armories);
            pm_target_armories = new com.oddlabs.tt.gui.PulldownMenu();
            pm_target_armories.addItem(new com.oddlabs.tt.gui.PulldownItem("1"));
            pm_target_armories.addItem(new com.oddlabs.tt.gui.PulldownItem("2"));
            pm_target_armories.addItem(new com.oddlabs.tt.gui.PulldownItem("3"));
            pm_target_armories.addItem(new com.oddlabs.tt.gui.PulldownItem("4"));
            pm_target_armories.addItem(new com.oddlabs.tt.gui.PulldownItem("5"));
            pm_target_armories.addItem(new com.oddlabs.tt.gui.PulldownItem("6"));
            com.oddlabs.tt.gui.PulldownButton pb_target_armories = new com.oddlabs.tt.gui.PulldownButton(gui_root, pm_target_armories, 0, 150);
            pm_target_armories.addItemChosenListener(new com.oddlabs.tt.guievent.ItemChosenListener_L((com.oddlabs.tt.gui.PulldownMenu unused_13, int unused_14) =>
            {
                markModified();
            }));
            group_target_armories.addChild(pb_target_armories);
            label_target_armories.place();
            pb_target_armories.place(label_target_armories, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            group_target_armories.compileCanvas();
            group_advanced_settings.addChild(group_target_armories);
            com.oddlabs.tt.gui.Group group_max_concur_quarters = new com.oddlabs.tt.gui.Group();
            com.oddlabs.tt.gui.Label label_max_concur_quarters = new com.oddlabs.tt.gui.Label(i18n("max_concurrent_quarters"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_max_concur_quarters.addChild(label_max_concur_quarters);
            pm_max_concur_quarters = new com.oddlabs.tt.gui.PulldownMenu();
            pm_max_concur_quarters.addItem(new com.oddlabs.tt.gui.PulldownItem("1"));
            pm_max_concur_quarters.addItem(new com.oddlabs.tt.gui.PulldownItem("2"));
            pm_max_concur_quarters.addItem(new com.oddlabs.tt.gui.PulldownItem("3"));
            pm_max_concur_quarters.addItem(new com.oddlabs.tt.gui.PulldownItem("4"));
            pm_max_concur_quarters.addItem(new com.oddlabs.tt.gui.PulldownItem("5"));
            com.oddlabs.tt.gui.PulldownButton pb_max_concur_quarters = new com.oddlabs.tt.gui.PulldownButton(gui_root, pm_max_concur_quarters, 1, 150);
            pm_max_concur_quarters.addItemChosenListener(new com.oddlabs.tt.guievent.ItemChosenListener_L((com.oddlabs.tt.gui.PulldownMenu unused_15, int unused_16) =>
            {
                markModified();
            }));
            group_max_concur_quarters.addChild(pb_max_concur_quarters);
            label_max_concur_quarters.place();
            pb_max_concur_quarters.place(label_max_concur_quarters, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            group_max_concur_quarters.compileCanvas();
            group_advanced_settings.addChild(group_max_concur_quarters);
            com.oddlabs.tt.gui.Group group_max_concur_armories = new com.oddlabs.tt.gui.Group();
            com.oddlabs.tt.gui.Label label_max_concur_armories = new com.oddlabs.tt.gui.Label(i18n("max_concurrent_armories"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_max_concur_armories.addChild(label_max_concur_armories);
            pm_max_concur_armories = new com.oddlabs.tt.gui.PulldownMenu();
            pm_max_concur_armories.addItem(new com.oddlabs.tt.gui.PulldownItem("1"));
            pm_max_concur_armories.addItem(new com.oddlabs.tt.gui.PulldownItem("2"));
            pm_max_concur_armories.addItem(new com.oddlabs.tt.gui.PulldownItem("3"));
            com.oddlabs.tt.gui.PulldownButton pb_max_concur_armories = new com.oddlabs.tt.gui.PulldownButton(gui_root, pm_max_concur_armories, 0, 150);
            pm_max_concur_armories.addItemChosenListener(new com.oddlabs.tt.guievent.ItemChosenListener_L((com.oddlabs.tt.gui.PulldownMenu unused_17, int unused_18) =>
            {
                markModified();
            }));
            group_max_concur_armories.addChild(pb_max_concur_armories);
            label_max_concur_armories.place();
            pb_max_concur_armories.place(label_max_concur_armories, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            group_max_concur_armories.compileCanvas();
            group_advanced_settings.addChild(group_max_concur_armories);
            com.oddlabs.tt.gui.Group group_num_res_towers = new com.oddlabs.tt.gui.Group();
            com.oddlabs.tt.gui.Label label_num_res_towers = new com.oddlabs.tt.gui.Label(i18n("num_resource_towers"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_num_res_towers.addChild(label_num_res_towers);
            pm_num_res_towers = new com.oddlabs.tt.gui.PulldownMenu();
            pm_num_res_towers.addItem(new com.oddlabs.tt.gui.PulldownItem("0"));
            pm_num_res_towers.addItem(new com.oddlabs.tt.gui.PulldownItem("3"));
            pm_num_res_towers.addItem(new com.oddlabs.tt.gui.PulldownItem("5"));
            pm_num_res_towers.addItem(new com.oddlabs.tt.gui.PulldownItem("7"));
            pm_num_res_towers.addItem(new com.oddlabs.tt.gui.PulldownItem("10"));
            pm_num_res_towers.addItem(new com.oddlabs.tt.gui.PulldownItem("15"));
            pm_num_res_towers.addItem(new com.oddlabs.tt.gui.PulldownItem("30"));
            pm_num_res_towers.addItem(new com.oddlabs.tt.gui.PulldownItem("60"));
            pm_num_res_towers.addItem(new com.oddlabs.tt.gui.PulldownItem("90"));
            com.oddlabs.tt.gui.PulldownButton pb_num_res_towers = new com.oddlabs.tt.gui.PulldownButton(gui_root, pm_num_res_towers, 3, 150);
            pm_num_res_towers.addItemChosenListener(new com.oddlabs.tt.guievent.ItemChosenListener_L((com.oddlabs.tt.gui.PulldownMenu unused_19, int unused_20) =>
            {
                markModified();
            }));
            group_num_res_towers.addChild(pb_num_res_towers);
            label_num_res_towers.place();
            pb_num_res_towers.place(label_num_res_towers, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            group_num_res_towers.compileCanvas();
            group_advanced_settings.addChild(group_num_res_towers);
            com.oddlabs.tt.gui.Group group_max_concur_towers = new com.oddlabs.tt.gui.Group();
            com.oddlabs.tt.gui.Label label_max_concur_towers = new com.oddlabs.tt.gui.Label(i18n("max_concurrent_towers"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_max_concur_towers.addChild(label_max_concur_towers);
            pm_max_concur_towers = new com.oddlabs.tt.gui.PulldownMenu();
            pm_max_concur_towers.addItem(new com.oddlabs.tt.gui.PulldownItem("1"));
            pm_max_concur_towers.addItem(new com.oddlabs.tt.gui.PulldownItem("3"));
            pm_max_concur_towers.addItem(new com.oddlabs.tt.gui.PulldownItem("5"));
            pm_max_concur_towers.addItem(new com.oddlabs.tt.gui.PulldownItem("7"));
            pm_max_concur_towers.addItem(new com.oddlabs.tt.gui.PulldownItem("10"));
            pm_max_concur_towers.addItem(new com.oddlabs.tt.gui.PulldownItem("15"));
            pm_max_concur_towers.addItem(new com.oddlabs.tt.gui.PulldownItem(i18n("unlimited")));
            com.oddlabs.tt.gui.PulldownButton pb_max_concur_towers = new com.oddlabs.tt.gui.PulldownButton(gui_root, pm_max_concur_towers, 2, 150);
            pm_max_concur_towers.addItemChosenListener(new com.oddlabs.tt.guievent.ItemChosenListener_L((com.oddlabs.tt.gui.PulldownMenu unused_21, int unused_22) =>
            {
                markModified();
            }));
            group_max_concur_towers.addChild(pb_max_concur_towers);
            label_max_concur_towers.place();
            pb_max_concur_towers.place(label_max_concur_towers, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            group_max_concur_towers.compileCanvas();
            group_advanced_settings.addChild(group_max_concur_towers);
            com.oddlabs.tt.gui.Group group_koth_statue_count = new com.oddlabs.tt.gui.Group();
            com.oddlabs.tt.gui.Label label_koth_statue_count = new com.oddlabs.tt.gui.Label(i18n("koth_statue_count"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_koth_statue_count.addChild(label_koth_statue_count);
            pm_koth_statue_count = new com.oddlabs.tt.gui.PulldownMenu();
            for (int i = com.oddlabs.tt.landscape.WorldParameters.MIN_KOTH_STATUE_COUNT; i <= com.oddlabs.tt.landscape.WorldParameters.MAX_KOTH_STATUE_COUNT; i++)
            {
                pm_koth_statue_count.addItem(new com.oddlabs.tt.gui.PulldownItem(java.lang.Integer.toString(i)));
            }
            com.oddlabs.tt.gui.PulldownButton pb_koth_statue_count = new com.oddlabs.tt.gui.PulldownButton(gui_root, pm_koth_statue_count, com.oddlabs.tt.landscape.WorldParameters.DEFAULT_KOTH_STATUE_COUNT - com.oddlabs.tt.landscape.WorldParameters.MIN_KOTH_STATUE_COUNT, 150);
            pm_koth_statue_count.addItemChosenListener(new com.oddlabs.tt.guievent.ItemChosenListener_L((com.oddlabs.tt.gui.PulldownMenu unused_23, int unused_24) =>
            {
                markModified();
            }));
            group_koth_statue_count.addChild(pb_koth_statue_count);
            label_koth_statue_count.place();
            pb_koth_statue_count.place(label_koth_statue_count, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            group_koth_statue_count.compileCanvas();
            // shown on the Mode & presets tab, beside the mode, while King of the Island is the mode
            mode_and_presets.setModeOption(com.oddlabs.matchmaking.GameMode.KING_OF_THE_ISLAND, group_koth_statue_count);
            com.oddlabs.tt.gui.Group group_magic_toggles = new com.oddlabs.tt.gui.Group();
            com.oddlabs.tt.gui.Label label_magic_toggles_header = new com.oddlabs.tt.gui.Label(i18n("magic_toggles_header"), com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont());
            group_magic_toggles.addChild(label_magic_toggles_header);
            label_magic_toggles_header.place();
            cb_magic1_enabled = new com.oddlabs.tt.gui.CheckBox(true, i18n("magic1_enabled"), i18n("magic1_enabled_tip"));
            group_magic_toggles.addChild(cb_magic1_enabled);
            cb_magic1_enabled.place(label_magic_toggles_header, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            cb_magic1_enabled.addCheckBoxListener(new com.oddlabs.tt.guievent.CheckBoxListener_L((bool unused_25) =>
            {
                markModified();
            }));
            cb_magic2_enabled = new com.oddlabs.tt.gui.CheckBox(true, i18n("magic2_enabled"), i18n("magic2_enabled_tip"));
            group_magic_toggles.addChild(cb_magic2_enabled);
            cb_magic2_enabled.place(cb_magic1_enabled, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            cb_magic2_enabled.addCheckBoxListener(new com.oddlabs.tt.guievent.CheckBoxListener_L((bool unused_26) =>
            {
                markModified();
            }));
            cb_magic3_enabled = new com.oddlabs.tt.gui.CheckBox(true, i18n("magic3_enabled"), i18n("magic3_enabled_tip"));
            group_magic_toggles.addChild(cb_magic3_enabled);
            cb_magic3_enabled.place(cb_magic2_enabled, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            cb_magic3_enabled.addCheckBoxListener(new com.oddlabs.tt.guievent.CheckBoxListener_L((bool unused_27) =>
            {
                markModified();
            }));
            cb_chiefs_courage_enabled = new com.oddlabs.tt.gui.CheckBox(true, i18n("chiefs_courage_enabled"), i18n("chiefs_courage_enabled_tip"));
            group_magic_toggles.addChild(cb_chiefs_courage_enabled);
            cb_chiefs_courage_enabled.place(cb_magic3_enabled, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            cb_chiefs_courage_enabled.addCheckBoxListener(new com.oddlabs.tt.guievent.CheckBoxListener_L((bool unused_28) =>
            {
                markModified();
            }));
            com.oddlabs.tt.gui.GUIObject previous_toggle = cb_chiefs_courage_enabled;
            for (int m_3 = com.oddlabs.tt.model.RacesResources.NUM_CLASSIC_MAGIC; m_3 < com.oddlabs.tt.model.RacesResources.NUM_MAGIC; m_3++)
            {
                int n_3 = m_3 + 1;
                cb_extra_magic_enabled[m_3] = new com.oddlabs.tt.gui.CheckBox(true, i18n("magic" + n_3 + "_enabled"), i18n("magic" + n_3 + "_enabled_tip"));
                if (!com.oddlabs.tt.global.Edition.new_tides)
                {
                    continue;
                }
                group_magic_toggles.addChild(cb_extra_magic_enabled[m_3]);
                cb_extra_magic_enabled[m_3].place(previous_toggle, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
                cb_extra_magic_enabled[m_3].addCheckBoxListener(new com.oddlabs.tt.guievent.CheckBoxListener_L((bool unused_x) =>
                {
                    markModified();
                }));
                previous_toggle = cb_extra_magic_enabled[m_3];
            }
            group_magic_toggles.compileCanvas();
            custom_options.addChild(group_magic_toggles);
            cb_adaptive_ai_enabled = new com.oddlabs.tt.gui.CheckBox(false, i18n("adaptive_ai_enabled"), i18n("adaptive_ai_enabled_tip"));
            cb_adaptive_ai_enabled.addCheckBoxListener(new com.oddlabs.tt.guievent.CheckBoxListener_L((bool unused_29) =>
            {
                markModified();
            }));
            pm_set_all_difficulty = new com.oddlabs.tt.gui.PulldownMenu();
            pm_set_all_difficulty.addItem(new com.oddlabs.tt.gui.PulldownItem(i18n("easy_ai"), com.oddlabs.matchmaking.RosterTemplate.Fill.EASY_AI));
            pm_set_all_difficulty.addItem(new com.oddlabs.tt.gui.PulldownItem(i18n("normal_ai"), com.oddlabs.matchmaking.RosterTemplate.Fill.NORMAL_AI));
            pm_set_all_difficulty.addItem(new com.oddlabs.tt.gui.PulldownItem(i18n("hard_ai"), com.oddlabs.matchmaking.RosterTemplate.Fill.HARD_AI));
            pm_set_all_difficulty.addItem(new com.oddlabs.tt.gui.PulldownItem(i18n("expert_ai"), com.oddlabs.matchmaking.RosterTemplate.Fill.EXPERT_AI));
            pm_set_all_difficulty.addItem(new com.oddlabs.tt.gui.PulldownItem(i18n("gauntlet_ai"), com.oddlabs.matchmaking.RosterTemplate.Fill.GAUNTLET_AI));
            group_set_all_difficulty = new com.oddlabs.tt.gui.Group();
            com.oddlabs.tt.gui.Label label_set_all_difficulty = new com.oddlabs.tt.gui.Label(i18n("set_all_ai_difficulty"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_set_all_difficulty.addChild(label_set_all_difficulty);
            com.oddlabs.tt.gui.PulldownButton pb_set_all_difficulty = new com.oddlabs.tt.gui.PulldownButton(gui_root, pm_set_all_difficulty, 0, 115);
            group_set_all_difficulty.addChild(pb_set_all_difficulty);
            label_set_all_difficulty.place();
            pb_set_all_difficulty.place(label_set_all_difficulty, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            group_set_all_difficulty.compileCanvas();
            cb_free_for_all = new com.oddlabs.tt.gui.CheckBox(false, i18n("free_for_all"), i18n("free_for_all_tip"));
            cb_team_together = new com.oddlabs.tt.gui.CheckBox(true, i18n("team_together"), i18n("team_together_tip"));
            cb_team_together.addCheckBoxListener(new com.oddlabs.tt.guievent.CheckBoxListener_L((bool unused_30) =>
            {
                markModified();
            }));
            cb_free_form = new com.oddlabs.tt.gui.CheckBox(false, i18n("free_form"), i18n("free_form_tip"));
            cb_extended = new com.oddlabs.tt.gui.CheckBox(com.oddlabs.tt.global.Edition.new_tides && com.oddlabs.tt.global.Edition.extended_default, i18n("extended"), i18n("extended_tip"));
            cb_free_form.addCheckBoxListener(new com.oddlabs.tt.guievent.CheckBoxListener_L((bool unused_31) =>
            {
                markModified();
            }));
            cb_extended.addCheckBoxListener(new com.oddlabs.tt.guievent.CheckBoxListener_L((bool unused_32) =>
            {
                markModified();
            }));
            cb_weather = new com.oddlabs.tt.gui.CheckBox(false, i18n("weather"), i18n("weather_tip"));
            cb_weather.addCheckBoxListener(new com.oddlabs.tt.guievent.CheckBoxListener_L((bool unused_33) =>
            {
                markModified();
            }));
            com.oddlabs.tt.gui.Group group_costs_health = new com.oddlabs.tt.gui.Group();
            com.oddlabs.tt.gui.Label label_costs_health_header = new com.oddlabs.tt.gui.Label(i18n("costs_health_header"), com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont());
            group_costs_health.addChild(label_costs_health_header);
            label_costs_health_header.place();
            com.oddlabs.tt.gui.Label label_magic1_cost = new com.oddlabs.tt.gui.Label(i18n("magic1_cost"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_costs_health.addChild(label_magic1_cost);
            slider_magic1_cost = new com.oddlabs.tt.gui.Slider(com.oddlabs.tt.form.TerrainMenu.SETTING_SLIDER_LENGTH, com.oddlabs.tt.form.TerrainMenu.MIN_MAGIC_COST, com.oddlabs.tt.form.TerrainMenu.MAX_MAGIC_COST, 40);
            label_magic1_cost_value = new com.oddlabs.tt.gui.Label("40", com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), com.oddlabs.tt.form.TerrainMenu.VALUE_LABEL_WIDTH);
            slider_magic1_cost.addValueListener(new com.oddlabs.tt.guievent.ValueListener_L((long value_6) =>
            {
                label_magic1_cost_value.setText(java.lang.Long.toString(value_6));
                markModified();
            }));
            group_costs_health.addChild(slider_magic1_cost);
            group_costs_health.addChild(label_magic1_cost_value);
            label_magic1_cost.place(label_costs_health_header, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            slider_magic1_cost.place(label_magic1_cost, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            label_magic1_cost_value.place(slider_magic1_cost, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            com.oddlabs.tt.gui.Label label_magic2_cost = new com.oddlabs.tt.gui.Label(i18n("magic2_cost"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_costs_health.addChild(label_magic2_cost);
            slider_magic2_cost = new com.oddlabs.tt.gui.Slider(com.oddlabs.tt.form.TerrainMenu.SETTING_SLIDER_LENGTH, com.oddlabs.tt.form.TerrainMenu.MIN_MAGIC_COST, com.oddlabs.tt.form.TerrainMenu.MAX_MAGIC_COST, 70);
            label_magic2_cost_value = new com.oddlabs.tt.gui.Label("70", com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), com.oddlabs.tt.form.TerrainMenu.VALUE_LABEL_WIDTH);
            slider_magic2_cost.addValueListener(new com.oddlabs.tt.guievent.ValueListener_L((long value_7) =>
            {
                label_magic2_cost_value.setText(java.lang.Long.toString(value_7));
                markModified();
            }));
            group_costs_health.addChild(slider_magic2_cost);
            group_costs_health.addChild(label_magic2_cost_value);
            label_magic2_cost.place(label_magic1_cost, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            slider_magic2_cost.place(label_magic2_cost, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            label_magic2_cost_value.place(slider_magic2_cost, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            com.oddlabs.tt.gui.Label label_magic3_cost = new com.oddlabs.tt.gui.Label(i18n("magic3_cost"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_costs_health.addChild(label_magic3_cost);
            slider_magic3_cost = new com.oddlabs.tt.gui.Slider(com.oddlabs.tt.form.TerrainMenu.SETTING_SLIDER_LENGTH, com.oddlabs.tt.form.TerrainMenu.MIN_MAGIC_COST, com.oddlabs.tt.form.TerrainMenu.MAX_MAGIC_COST, 120);
            label_magic3_cost_value = new com.oddlabs.tt.gui.Label("90", com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), com.oddlabs.tt.form.TerrainMenu.VALUE_LABEL_WIDTH);
            slider_magic3_cost.addValueListener(new com.oddlabs.tt.guievent.ValueListener_L((long value_8) =>
            {
                label_magic3_cost_value.setText(java.lang.Long.toString(value_8));
                markModified();
            }));
            group_costs_health.addChild(slider_magic3_cost);
            group_costs_health.addChild(label_magic3_cost_value);
            label_magic3_cost.place(label_magic2_cost, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            slider_magic3_cost.place(label_magic3_cost, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            label_magic3_cost_value.place(slider_magic3_cost, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            com.oddlabs.tt.gui.GUIObject previous_cost = label_magic3_cost;
            var cost_rows = new System.Collections.Generic.List<com.oddlabs.tt.gui.GUIObject[]> {
                new com.oddlabs.tt.gui.GUIObject[] { label_magic1_cost, slider_magic1_cost, label_magic1_cost_value },
                new com.oddlabs.tt.gui.GUIObject[] { label_magic2_cost, slider_magic2_cost, label_magic2_cost_value },
                new com.oddlabs.tt.gui.GUIObject[] { label_magic3_cost, slider_magic3_cost, label_magic3_cost_value } };
            for (int m_4 = com.oddlabs.tt.model.RacesResources.NUM_CLASSIC_MAGIC; m_4 < com.oddlabs.tt.model.RacesResources.NUM_MAGIC; m_4++)
            {
                int def_4 = java.lang.Math.round(com.oddlabs.tt.model.Unit.MAX_MAGIC_ENERGY[m_4]);
                com.oddlabs.tt.gui.Label label_cost_4 = new com.oddlabs.tt.gui.Label(i18n("magic" + (m_4 + 1) + "_cost"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
                com.oddlabs.tt.gui.Slider slider_4 = new com.oddlabs.tt.gui.Slider(com.oddlabs.tt.form.TerrainMenu.SETTING_SLIDER_LENGTH, com.oddlabs.tt.form.TerrainMenu.MIN_MAGIC_COST, com.oddlabs.tt.form.TerrainMenu.MAX_MAGIC_COST, def_4);
                com.oddlabs.tt.gui.Label value_4 = new com.oddlabs.tt.gui.Label(java.lang.Integer.toString(def_4), com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), com.oddlabs.tt.form.TerrainMenu.VALUE_LABEL_WIDTH);
                slider_4.addValueListener(new com.oddlabs.tt.guievent.ValueListener_L((long v_4) =>
                {
                    value_4.setText(java.lang.Long.toString(v_4));
                    markModified();
                }));
                slider_extra_magic_cost[m_4] = slider_4;
                if (!com.oddlabs.tt.global.Edition.new_tides)
                {
                    continue;
                }
                group_costs_health.addChild(label_cost_4);
                group_costs_health.addChild(slider_4);
                group_costs_health.addChild(value_4);
                label_cost_4.place(previous_cost, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
                slider_4.place(label_cost_4, com.oddlabs.tt.gui.Placement.RIGHT_MID);
                value_4.place(slider_4, com.oddlabs.tt.gui.Placement.RIGHT_MID);
                previous_cost = label_cost_4;
                cost_rows.Add(new com.oddlabs.tt.gui.GUIObject[] { label_cost_4, slider_4, value_4 });
            }
            com.oddlabs.tt.gui.Label label_building_health_mult = new com.oddlabs.tt.gui.Label(i18n("building_health_mult"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_costs_health.addChild(label_building_health_mult);
            slider_building_health_mult = new com.oddlabs.tt.gui.Slider(com.oddlabs.tt.form.TerrainMenu.SETTING_SLIDER_LENGTH, com.oddlabs.tt.form.TerrainMenu.MIN_HEALTH_MULT_PERCENT, com.oddlabs.tt.form.TerrainMenu.MAX_HEALTH_MULT_PERCENT, com.oddlabs.tt.form.TerrainMenu.DEFAULT_HEALTH_MULT_PERCENT);
            label_building_health_mult_value = new com.oddlabs.tt.gui.Label((com.oddlabs.tt.form.TerrainMenu.DEFAULT_HEALTH_MULT_PERCENT) + "%", com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), com.oddlabs.tt.form.TerrainMenu.VALUE_LABEL_WIDTH);
            slider_building_health_mult.addValueListener(new com.oddlabs.tt.guievent.ValueListener_L((long value_9) =>
            {
                label_building_health_mult_value.setText((value_9) + "%");
                markModified();
            }));
            group_costs_health.addChild(slider_building_health_mult);
            group_costs_health.addChild(label_building_health_mult_value);
            slider_building_health_mult.place(label_building_health_mult, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            label_building_health_mult_value.place(slider_building_health_mult, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            com.oddlabs.tt.gui.Label label_viking_chief_health_mult = new com.oddlabs.tt.gui.Label(i18n("viking_chief_health_mult"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_costs_health.addChild(label_viking_chief_health_mult);
            slider_viking_chief_health_mult = new com.oddlabs.tt.gui.Slider(com.oddlabs.tt.form.TerrainMenu.SETTING_SLIDER_LENGTH, com.oddlabs.tt.form.TerrainMenu.MIN_HEALTH_MULT_PERCENT, com.oddlabs.tt.form.TerrainMenu.MAX_HEALTH_MULT_PERCENT, com.oddlabs.tt.form.TerrainMenu.DEFAULT_HEALTH_MULT_PERCENT);
            label_viking_chief_health_mult_value = new com.oddlabs.tt.gui.Label((com.oddlabs.tt.form.TerrainMenu.DEFAULT_HEALTH_MULT_PERCENT) + "%", com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), com.oddlabs.tt.form.TerrainMenu.VALUE_LABEL_WIDTH);
            slider_viking_chief_health_mult.addValueListener(new com.oddlabs.tt.guievent.ValueListener_L((long value_10) =>
            {
                label_viking_chief_health_mult_value.setText((value_10) + "%");
                markModified();
            }));
            group_costs_health.addChild(slider_viking_chief_health_mult);
            group_costs_health.addChild(label_viking_chief_health_mult_value);
            label_viking_chief_health_mult.place(label_building_health_mult, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            slider_viking_chief_health_mult.place(label_viking_chief_health_mult, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            label_viking_chief_health_mult_value.place(slider_viking_chief_health_mult, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            com.oddlabs.tt.gui.Label label_native_chief_health_mult = new com.oddlabs.tt.gui.Label(i18n("native_chief_health_mult"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_costs_health.addChild(label_native_chief_health_mult);
            slider_native_chief_health_mult = new com.oddlabs.tt.gui.Slider(com.oddlabs.tt.form.TerrainMenu.SETTING_SLIDER_LENGTH, com.oddlabs.tt.form.TerrainMenu.MIN_HEALTH_MULT_PERCENT, com.oddlabs.tt.form.TerrainMenu.MAX_HEALTH_MULT_PERCENT, com.oddlabs.tt.form.TerrainMenu.DEFAULT_HEALTH_MULT_PERCENT);
            label_native_chief_health_mult_value = new com.oddlabs.tt.gui.Label((com.oddlabs.tt.form.TerrainMenu.DEFAULT_HEALTH_MULT_PERCENT) + "%", com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), com.oddlabs.tt.form.TerrainMenu.VALUE_LABEL_WIDTH);
            slider_native_chief_health_mult.addValueListener(new com.oddlabs.tt.guievent.ValueListener_L((long value_11) =>
            {
                label_native_chief_health_mult_value.setText((value_11) + "%");
                markModified();
            }));
            group_costs_health.addChild(slider_native_chief_health_mult);
            group_costs_health.addChild(label_native_chief_health_mult_value);
            label_native_chief_health_mult.place(label_viking_chief_health_mult, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            slider_native_chief_health_mult.place(label_native_chief_health_mult, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            label_native_chief_health_mult_value.place(slider_native_chief_health_mult, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            com.oddlabs.tt.gui.Label label_unit_range_mult = new com.oddlabs.tt.gui.Label(i18n("unit_range_mult"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_costs_health.addChild(label_unit_range_mult);
            slider_unit_range_mult = new com.oddlabs.tt.gui.Slider(com.oddlabs.tt.form.TerrainMenu.SETTING_SLIDER_LENGTH, com.oddlabs.tt.form.TerrainMenu.MIN_HEALTH_MULT_PERCENT, com.oddlabs.tt.form.TerrainMenu.MAX_HEALTH_MULT_PERCENT, com.oddlabs.tt.form.TerrainMenu.DEFAULT_HEALTH_MULT_PERCENT);
            label_unit_range_mult_value = new com.oddlabs.tt.gui.Label((com.oddlabs.tt.form.TerrainMenu.DEFAULT_HEALTH_MULT_PERCENT) + "%", com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), com.oddlabs.tt.form.TerrainMenu.VALUE_LABEL_WIDTH);
            slider_unit_range_mult.addValueListener(new com.oddlabs.tt.guievent.ValueListener_L((long value_12) =>
            {
                label_unit_range_mult_value.setText((value_12) + "%");
                markModified();
            }));
            group_costs_health.addChild(slider_unit_range_mult);
            group_costs_health.addChild(label_unit_range_mult_value);
            label_unit_range_mult.place(label_magic1_cost_value, com.oddlabs.tt.gui.Placement.RIGHT_TOP);
            slider_unit_range_mult.place(label_unit_range_mult, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            label_unit_range_mult_value.place(slider_unit_range_mult, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            label_building_health_mult.place(label_unit_range_mult, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            slider_building_health_mult.place(label_building_health_mult, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            label_building_health_mult_value.place(slider_building_health_mult, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            label_viking_chief_health_mult.place(label_building_health_mult, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            slider_viking_chief_health_mult.place(label_viking_chief_health_mult, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            label_viking_chief_health_mult_value.place(slider_viking_chief_health_mult, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            label_native_chief_health_mult.place(label_viking_chief_health_mult, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            slider_native_chief_health_mult.place(label_native_chief_health_mult, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            label_native_chief_health_mult_value.place(slider_native_chief_health_mult, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            com.oddlabs.tt.gui.Label label_armory_resource_cap = new com.oddlabs.tt.gui.Label(i18n("armory_resource_cap"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            slider_armory_resource_cap = new com.oddlabs.tt.gui.Slider(com.oddlabs.tt.form.TerrainMenu.SETTING_SLIDER_LENGTH, com.oddlabs.tt.form.TerrainMenu.MIN_RESOURCE_CAP, com.oddlabs.tt.form.TerrainMenu.MAX_RESOURCE_CAP, com.oddlabs.tt.form.TerrainMenu.DEFAULT_RESOURCE_CAP);
            label_armory_resource_cap_value = new com.oddlabs.tt.gui.Label(java.lang.Integer.toString(com.oddlabs.tt.form.TerrainMenu.DEFAULT_RESOURCE_CAP), com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), com.oddlabs.tt.form.TerrainMenu.VALUE_LABEL_WIDTH);
            slider_armory_resource_cap.addValueListener(new com.oddlabs.tt.guievent.ValueListener_L((long value_13) =>
            {
                label_armory_resource_cap_value.setText(java.lang.Long.toString(value_13));
                markModified();
            }));
            label_armory_resource_cap.place(label_unit_range_mult, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            slider_armory_resource_cap.place(label_armory_resource_cap, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            label_armory_resource_cap_value.place(slider_armory_resource_cap, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            com.oddlabs.tt.gui.Label label_rock_resource_cap = new com.oddlabs.tt.gui.Label(i18n("rock_resource_cap"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            slider_rock_resource_cap = new com.oddlabs.tt.gui.Slider(com.oddlabs.tt.form.TerrainMenu.SETTING_SLIDER_LENGTH, com.oddlabs.tt.form.TerrainMenu.MIN_RESOURCE_CAP, com.oddlabs.tt.form.TerrainMenu.MAX_RESOURCE_CAP, com.oddlabs.tt.form.TerrainMenu.DEFAULT_RESOURCE_CAP);
            label_rock_resource_cap_value = new com.oddlabs.tt.gui.Label(java.lang.Integer.toString(com.oddlabs.tt.form.TerrainMenu.DEFAULT_RESOURCE_CAP), com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), com.oddlabs.tt.form.TerrainMenu.VALUE_LABEL_WIDTH);
            slider_rock_resource_cap.addValueListener(new com.oddlabs.tt.guievent.ValueListener_L((long value_14) =>
            {
                label_rock_resource_cap_value.setText(java.lang.Long.toString(value_14));
                markModified();
            }));
            label_rock_resource_cap.place(label_armory_resource_cap, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            slider_rock_resource_cap.place(label_rock_resource_cap, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            label_rock_resource_cap_value.place(slider_rock_resource_cap, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            com.oddlabs.tt.gui.Label label_iron_resource_cap = new com.oddlabs.tt.gui.Label(i18n("iron_resource_cap"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            slider_iron_resource_cap = new com.oddlabs.tt.gui.Slider(com.oddlabs.tt.form.TerrainMenu.SETTING_SLIDER_LENGTH, com.oddlabs.tt.form.TerrainMenu.MIN_RESOURCE_CAP, com.oddlabs.tt.form.TerrainMenu.MAX_RESOURCE_CAP, com.oddlabs.tt.form.TerrainMenu.DEFAULT_RESOURCE_CAP);
            label_iron_resource_cap_value = new com.oddlabs.tt.gui.Label(java.lang.Integer.toString(com.oddlabs.tt.form.TerrainMenu.DEFAULT_RESOURCE_CAP), com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), com.oddlabs.tt.form.TerrainMenu.VALUE_LABEL_WIDTH);
            slider_iron_resource_cap.addValueListener(new com.oddlabs.tt.guievent.ValueListener_L((long value_15) =>
            {
                label_iron_resource_cap_value.setText(java.lang.Long.toString(value_15));
                markModified();
            }));
            label_iron_resource_cap.place(label_rock_resource_cap, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            slider_iron_resource_cap.place(label_iron_resource_cap, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            label_iron_resource_cap_value.place(slider_iron_resource_cap, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            com.oddlabs.tt.gui.Label label_rubber_resource_cap = new com.oddlabs.tt.gui.Label(i18n("rubber_resource_cap"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            slider_rubber_resource_cap = new com.oddlabs.tt.gui.Slider(com.oddlabs.tt.form.TerrainMenu.SETTING_SLIDER_LENGTH, com.oddlabs.tt.form.TerrainMenu.MIN_RESOURCE_CAP, com.oddlabs.tt.form.TerrainMenu.MAX_RESOURCE_CAP, com.oddlabs.tt.form.TerrainMenu.DEFAULT_RESOURCE_CAP);
            label_rubber_resource_cap_value = new com.oddlabs.tt.gui.Label(java.lang.Integer.toString(com.oddlabs.tt.form.TerrainMenu.DEFAULT_RESOURCE_CAP), com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), com.oddlabs.tt.form.TerrainMenu.VALUE_LABEL_WIDTH);
            slider_rubber_resource_cap.addValueListener(new com.oddlabs.tt.guievent.ValueListener_L((long value_16) =>
            {
                label_rubber_resource_cap_value.setText(java.lang.Long.toString(value_16));
                markModified();
            }));
            label_rubber_resource_cap.place(label_iron_resource_cap, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            slider_rubber_resource_cap.place(label_rubber_resource_cap, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            label_rubber_resource_cap_value.place(slider_rubber_resource_cap, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            // (cleanup) the two columns as a grid: the sliders of a column start together, and the combat column
            // starts past the longest magic row (the long New Tides names ran into it)
            alignColumns(cost_rows.ToArray(), new com.oddlabs.tt.gui.GUIObject[][] {
                new com.oddlabs.tt.gui.GUIObject[] { label_unit_range_mult, slider_unit_range_mult, label_unit_range_mult_value },
                new com.oddlabs.tt.gui.GUIObject[] { label_building_health_mult, slider_building_health_mult, label_building_health_mult_value },
                new com.oddlabs.tt.gui.GUIObject[] { label_viking_chief_health_mult, slider_viking_chief_health_mult, label_viking_chief_health_mult_value },
                new com.oddlabs.tt.gui.GUIObject[] { label_native_chief_health_mult, slider_native_chief_health_mult, label_native_chief_health_mult_value } },
                com.oddlabs.tt.gui.Skin.getSkin().getFormData().objectSpacing(), 4 * com.oddlabs.tt.gui.Skin.getSkin().getFormData().sectionSpacing());
            group_costs_health.compileCanvas();
            custom_options.addChild(group_costs_health);
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
            pm_gamespeed.addItemChosenListener(new com.oddlabs.tt.guievent.ItemChosenListener_L((com.oddlabs.tt.gui.PulldownMenu unused_34, int unused_35) =>
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
            pulldown_size.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.util.ServerMessageBundler.getSizeString(com.oddlabs.matchmaking.Game.SIZE_UNREAL)));
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
            if (com.oddlabs.tt.global.Edition.new_tides)
            {
                pm_terrain_type.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.util.ServerMessageBundler.getTerrainTypeString(com.oddlabs.matchmaking.Game.TERRAIN_TYPE_ELEMENTAL)));
            }
            com.oddlabs.tt.gui.PulldownButton pb_terrain_type = new com.oddlabs.tt.gui.PulldownButton(gui_root, pm_terrain_type, 0, 150);
            group_terrain_type.addChild(pb_terrain_type);
            label_terrain_type.place();
            pb_terrain_type.place(label_terrain_type, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            group_terrain_type.compileCanvas();
            pm_terrain_type.addItemChosenListener(new com.oddlabs.tt.form.TerrainMenu.PulldownUpdateMapcodeListener(this));
            group_map_options.addChild(group_terrain_type);
            com.oddlabs.tt.gui.Group group_sliders = new com.oddlabs.tt.gui.Group();
            com.oddlabs.tt.gui.Label label_hills_low = new com.oddlabs.tt.gui.Label(i18n("min"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_sliders.addChild(label_hills_low);
            com.oddlabs.tt.gui.Label label_hills_high = new com.oddlabs.tt.gui.Label(i18n("max"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_sliders.addChild(label_hills_high);
            com.oddlabs.tt.gui.Label label_hills = new com.oddlabs.tt.gui.Label(i18n("hills"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_sliders.addChild(label_hills);
            slider_hills = new com.oddlabs.tt.gui.Slider(com.oddlabs.tt.form.TerrainMenu.SLIDER_LENGTH, 0, com.oddlabs.tt.form.TerrainMenu.MAP_SETTING_SLIDER_MAX, com.oddlabs.tt.form.TerrainMenu.SLIDER_MAX_VALUE / 2);
            slider_hills.addValueListener(new com.oddlabs.tt.form.TerrainMenu.SliderUpdateMapcodeListener(this));
            group_sliders.addChild(slider_hills);
            com.oddlabs.tt.gui.Label label_vegetation_low = new com.oddlabs.tt.gui.Label(i18n("min"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_sliders.addChild(label_vegetation_low);
            com.oddlabs.tt.gui.Label label_vegetation_high = new com.oddlabs.tt.gui.Label(i18n("max"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_sliders.addChild(label_vegetation_high);
            com.oddlabs.tt.gui.Label label_vegetation = new com.oddlabs.tt.gui.Label(i18n("trees"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_sliders.addChild(label_vegetation);
            slider_vegetation = new com.oddlabs.tt.gui.Slider(com.oddlabs.tt.form.TerrainMenu.SLIDER_LENGTH, 0, com.oddlabs.tt.form.TerrainMenu.MAP_SETTING_SLIDER_MAX, com.oddlabs.tt.form.TerrainMenu.SLIDER_MAX_VALUE / 2);
            slider_vegetation.addValueListener(new com.oddlabs.tt.form.TerrainMenu.SliderUpdateMapcodeListener(this));
            group_sliders.addChild(slider_vegetation);
            com.oddlabs.tt.gui.Label label_supplies_low = new com.oddlabs.tt.gui.Label(i18n("min"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_sliders.addChild(label_supplies_low);
            com.oddlabs.tt.gui.Label label_supplies_high = new com.oddlabs.tt.gui.Label(i18n("max"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_sliders.addChild(label_supplies_high);
            com.oddlabs.tt.gui.Label label_supplies = new com.oddlabs.tt.gui.Label(i18n("resources"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group_sliders.addChild(label_supplies);
            slider_supplies = new com.oddlabs.tt.gui.Slider(com.oddlabs.tt.form.TerrainMenu.SLIDER_LENGTH, 0, com.oddlabs.tt.form.TerrainMenu.MAP_SETTING_SLIDER_MAX, com.oddlabs.tt.form.TerrainMenu.SLIDER_MAX_VALUE / 2);
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
            for (int i_2 = com.oddlabs.tt.form.TerrainMenu.DEFAULT_PLAYER_COUNT; i_2 <= com.oddlabs.matchmaking.MatchmakingServerInterface.MAX_PLAYERS; i_2++)
            {
                pulldown_menu_slots.addItem(new com.oddlabs.tt.gui.PulldownItem(java.lang.Integer.toString(i_2)));
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
            if (multiplayer)
            {
                // (cleanup: the check boxes in rows of at most 560 pixels instead of one long row)
                com.oddlabs.tt.gui.GUIObject[] mp_boxes = com.oddlabs.tt.global.Edition.new_tides
                    ? new com.oddlabs.tt.gui.GUIObject[] { cb_adaptive_ai_enabled, cb_free_for_all, cb_team_together, cb_free_form, cb_extended, cb_weather }
                    : new com.oddlabs.tt.gui.GUIObject[] { cb_adaptive_ai_enabled, cb_free_for_all, cb_team_together };
                com.oddlabs.tt.gui.GUIObject mp_last_row = flowRows(mp_boxes, null, 10000, 2 * com.oddlabs.tt.gui.Skin.getSkin().getFormData().objectSpacing(), 0);
                group_set_all_difficulty.place(mp_last_row, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT, com.oddlabs.tt.gui.Skin.getSkin().getFormData().sectionSpacing());
                if (com.oddlabs.tt.global.Edition.new_tides)
                {
                    roster_panel.addChild(cb_free_form);
                    roster_panel.addChild(cb_extended);
                    roster_panel.addChild(cb_weather);
                }
                roster_panel.addChild(cb_adaptive_ai_enabled);
                roster_panel.addChild(cb_free_for_all);
                roster_panel.addChild(cb_team_together);
                roster_panel.addChild(group_set_all_difficulty);
                roster_panel.setHeader(group_set_all_difficulty);
            }
            com.oddlabs.tt.gui.ScrollableGroup group_race_team = buildPlayerSlots(player_count);
            if (multiplayer)
            {
                roster_panel.setRoster(group_race_team);
            }
            else
            {
                standard.addChild(group_race_team);
            }
            pm_set_all_difficulty.addItemChosenListener(new com.oddlabs.tt.guievent.ItemChosenListener_L((com.oddlabs.tt.gui.PulldownMenu menu, int index) =>
            {
                com.oddlabs.matchmaking.RosterTemplate.Fill chosen = ((com.oddlabs.matchmaking.RosterTemplate.Fill)menu.getItem(index).getAttachment());
                if (chosen == null)
                {
                    return;
                }
                for (int i_3 = 1; i_3 < player_count; i_3++)
                {
                    difficulty_pulldown_menus[i_3].chooseItem(fillToDifficultyIndex(chosen, i_3));
                }
                markModified();
            }));
            cb_free_for_all.addCheckBoxListener(new com.oddlabs.tt.guievent.CheckBoxListener_L((bool unused_36) =>
            {
                if (cb_free_for_all.isMarked())
                {
                    for (int i_4 = 0; i_4 < player_count; i_4++)
                    {
                        team_pulldown_menus[i_4].chooseItem(i_4);
                    }
                    markModified();
                }
            }));
            com.oddlabs.tt.gui.Group group_buttons = new com.oddlabs.tt.gui.Group();
            button_ok = new com.oddlabs.tt.gui.OKButton(com.oddlabs.tt.form.TerrainMenu.BUTTON_WIDTH);
            button_ok.addMouseClickListener(new com.oddlabs.tt.form.TerrainMenu.OKListener(this));
            com.oddlabs.tt.gui.HorizButton button_cancel = new com.oddlabs.tt.gui.CancelButton(com.oddlabs.tt.form.TerrainMenu.BUTTON_WIDTH);
            button_cancel.addMouseClickListener(new com.oddlabs.tt.form.TerrainMenu.CancelButtonListener(this));
            com.oddlabs.tt.gui.HorizButton button_mapcode = new com.oddlabs.tt.gui.HorizButton(i18n("enter_map_code"), 170);
            button_mapcode.addMouseClickListener(new com.oddlabs.tt.form.TerrainMenu.MapcodeListener(this));
            com.oddlabs.tt.gui.HorizButton button_custom_map = new com.oddlabs.tt.gui.HorizButton(i18n("load_custom_map"), 170);
            button_custom_map.addMouseClickListener(new com.oddlabs.tt.form.TerrainMenu.CustomMapListener(this));
            com.oddlabs.tt.gui.HorizButton button_advanced = new com.oddlabs.tt.gui.HorizButton(i18n("advanced"), 130);
            if (button_advanced != null)
            {
                button_advanced.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_37, int unused_38, int unused_39, int unused_40) =>
                {
                    if (multiplayer)
                    {
                        gui_root.addModalForm(new com.oddlabs.tt.form.TerrainMenu.ExtraSettingsForm(i18n("advanced_settings_caption"), units_options, custom_options, economy_options));
                    }
                    else
                    {
                        gui_root.addModalForm(new com.oddlabs.tt.form.TerrainMenu.ExtraSettingsForm(i18n("advanced_settings_caption"), custom_options, economy_options));
                    }
                }));
                addChild(button_advanced);
            }
            group_buttons.addChild(button_ok);
            group_buttons.addChild(button_cancel);
            group_buttons.addChild(button_mapcode);
            group_buttons.addChild(button_custom_map);
            com.oddlabs.tt.gui.HorizButton button_editor_map = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.mapeditor.MapEditor.i18n("play_editor_map"), 170);
            button_editor_map.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_41, int unused_42, int unused_43, int unused_44) =>
            {
                com.oddlabs.tt.mapeditor.MapEditor.chooseMapToPlay(gui_root, new java.util.function.Consumer_L((object custom_l) =>
                {
                    com.oddlabs.tt.mapeditor.CustomMap custom = (com.oddlabs.tt.mapeditor.CustomMap)custom_l;
                    selected_editor_map = custom;
                    if (!startGame())
                    {
                        selected_editor_map = null;
                    }
                }));
            }));
            if (!multiplayer)
            {
                group_buttons.addChild(button_editor_map);
            }
            button_cancel.place();
            button_ok.place(button_cancel, com.oddlabs.tt.gui.Placement.LEFT_MID);
            if (multiplayer)
            {
                button_mapcode.place(button_ok, com.oddlabs.tt.gui.Placement.LEFT_MID);
                button_custom_map.place(button_mapcode, com.oddlabs.tt.gui.Placement.LEFT_MID);
            }
            else
            {
                button_mapcode.place(button_cancel, com.oddlabs.tt.gui.Placement.TOP_RIGHT);
                button_custom_map.place(button_mapcode, com.oddlabs.tt.gui.Placement.LEFT_MID);
                button_editor_map.place(button_custom_map, com.oddlabs.tt.gui.Placement.LEFT_MID);
            }
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
            if (multiplayer)
            {
                group_max_buildings.place();
                group_starting_warriors.place(group_max_buildings, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT, com.oddlabs.tt.gui.Skin.getSkin().getFormData().sectionSpacing());
                units_options.compileCanvas();
            }
            else
            {
                group_max_buildings.place(group_terrain_type, com.oddlabs.tt.gui.Placement.BOTTOM_RIGHT);
                group_starting_warriors.place(group_max_buildings, com.oddlabs.tt.gui.Placement.BOTTOM_RIGHT);
            }
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
                // the check boxes in rows that fit under the map options (cleanup: six in one row ran past the panel)
                com.oddlabs.tt.gui.GUIObject[] boxes = com.oddlabs.tt.global.Edition.new_tides
                    ? new com.oddlabs.tt.gui.GUIObject[] { cb_adaptive_ai_enabled, cb_free_for_all, cb_team_together, cb_free_form, cb_extended, cb_weather }
                    : new com.oddlabs.tt.gui.GUIObject[] { cb_adaptive_ai_enabled, cb_free_for_all, cb_team_together };
                com.oddlabs.tt.gui.GUIObject last_row = flowRows(boxes, group_map_options, 10000,
                    2 * com.oddlabs.tt.gui.Skin.getSkin().getFormData().objectSpacing(), com.oddlabs.tt.gui.Skin.getSkin().getFormData().sectionSpacing());
                if (com.oddlabs.tt.global.Edition.new_tides)
                {
                    standard.addChild(cb_free_form);
                    standard.addChild(cb_extended);
                    standard.addChild(cb_weather);
                }
                group_set_all_difficulty.place(last_row, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT, com.oddlabs.tt.gui.Skin.getSkin().getFormData().sectionSpacing());
                group_race_team.place(group_set_all_difficulty, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT, com.oddlabs.tt.gui.Skin.getSkin().getFormData().sectionSpacing());
                standard.addChild(cb_adaptive_ai_enabled);
                standard.addChild(group_set_all_difficulty);
                standard.addChild(cb_free_for_all);
                standard.addChild(cb_team_together);
            }
            standard.compileCanvas();
            group_sliders.place();
            group_num_players.place(group_sliders, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT, com.oddlabs.tt.gui.Skin.getSkin().getFormData().sectionSpacing());
            group_seed.place(group_num_players, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT, com.oddlabs.tt.gui.Skin.getSkin().getFormData().sectionSpacing());
            com.oddlabs.tt.form.TerrainMenu.layoutColumns(new com.oddlabs.tt.gui.Renderable[] { group_max_chiefs_per_q, group_max_chiefs_total, group_chief_heal_idle, group_chief_heal_amount, group_target_quarters }, new com.oddlabs.tt.gui.Renderable[] { group_target_armories, group_max_concur_quarters, group_max_concur_armories, group_num_res_towers, group_max_concur_towers }, com.oddlabs.tt.form.TerrainMenu.ADVANCED_ROW_SPACING, 3 * com.oddlabs.tt.gui.Skin.getSkin().getFormData().sectionSpacing());
            group_advanced_settings.compileCanvas();
            economy_options.addChild(group_advanced_settings);
            group_advanced_settings.place();
            economy_options.compileCanvas();
            advanced.compileCanvas();
            group_costs_health.place();
            group_magic_toggles.place(group_costs_health, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT, com.oddlabs.tt.gui.Skin.getSkin().getFormData().sectionSpacing());
            custom_options.compileCanvas();
            com.oddlabs.tt.gui.PanelGroup panel_group = (multiplayer ? new com.oddlabs.tt.gui.PanelGroup(1, mode_and_presets, standard, advanced, roster_panel) : new com.oddlabs.tt.gui.PanelGroup(1, mode_and_presets, standard, advanced));
            addChild(panel_group);
            com.oddlabs.tt.form.TerrainMenu.PulldownUpdatePlayersChangedListener playersChangedListener = new com.oddlabs.tt.form.TerrainMenu.PulldownUpdatePlayersChangedListener(this, standard);
            playersChangedListener.setCurrentGroup(group_race_team);
            pulldown_menu_slots.addItemChosenListener(playersChangedListener);
            label_headline.place();
            panel_group.place(label_headline, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            if (button_advanced != null)
            {
                button_advanced.place(panel_group, com.oddlabs.tt.gui.Placement.TOP_RIGHT);
            }
            group_buttons.place(com.oddlabs.tt.gui.Origin.AT_END);
            compileCanvas();
            randomize();
            pulldown_size.addItemChosenListener(new com.oddlabs.tt.form.TerrainMenu.PulldownUpdateSizeListener());
            pm_terrain_type.addItemChosenListener(new com.oddlabs.tt.form.TerrainMenu.PulldownUpdateTerrainListener());
            for (int i_5 = 0; i_5 < player_count; i_5++)
            {
                difficulty_pulldown_menus[i_5].addItemChosenListener(new com.oddlabs.tt.form.TerrainMenu.PulldownUpdateHardListener());
                team_pulldown_menus[i_5].chooseItem(defaultTeam(i_5));
                if (!multiplayer)
                {
                    if (i_5 == 0)
                    {
                        difficulty_pulldown_menus[i_5].chooseItem(0);
                    }
                    else
                    {
                        difficulty_pulldown_menus[i_5].chooseItem(com.oddlabs.tt.net.PlayerSlot.AI_EASY);
                        race_pulldown_menus[i_5].chooseItem((race_pulldown_menus[0].getChosenItemIndex() + 1) % 2);
                    }
                }
                else
                {
                    difficulty_pulldown_menus[i_5].chooseItem(0);
                }
            }
            pulldown_size.chooseItem(1);
            if (!com.oddlabs.tt.render.Renderer.isRegistered())
            {
                pm_terrain_type.chooseItem(0);
            }
            cb_rated.addCheckBoxListener(new com.oddlabs.tt.guievent.CheckBoxListener_L((bool unused_45) =>
            {
                markModified();
            }));
            initialized = true;
        }
    }
}
