// Translated from MapEditorForm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.mapeditor
{
    // Java: 41
    public class MapEditorForm : com.oddlabs.tt.gui.Form
    {
        public const int SLIDER_LENGTH = 250;
        public const int BUTTON_WIDTH = 100;
        public const int PULLDOWN_WIDTH = 150;
        public com.oddlabs.net.NetworkSelector network;
        public com.oddlabs.tt.gui.GUIRoot gui_root;
        public com.oddlabs.tt.gui.PulldownMenu pulldown_size;
        public com.oddlabs.tt.gui.PulldownMenu pulldown_terrain;
        public com.oddlabs.tt.gui.Slider slider_hills;
        public com.oddlabs.tt.gui.Slider slider_trees;
        public com.oddlabs.tt.gui.Slider slider_supplies;
        public com.oddlabs.tt.gui.Label label_mapcode;
        public com.oddlabs.tt.gui.Label label_map;
        public com.oddlabs.tt.gui.HorizButton button_start;
        public com.oddlabs.tt.mapeditor.MapSettings settings;
        public string map_name;
        public float[][] heights;
        public com.oddlabs.tt.mapeditor.MapFile.Resources resources;
        public java.util.List starts;
        public bool applying;
        // Java: 165
        public static com.oddlabs.tt.gui.Group labelled(string caption_key, com.oddlabs.tt.gui.GUIObject control)
        {
            com.oddlabs.tt.gui.Group group = new com.oddlabs.tt.gui.Group();
            com.oddlabs.tt.gui.Label label = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.mapeditor.MapEditor.terrainI18n(caption_key), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            group.addChild(label);
            group.addChild(control);
            label.place();
            control.place(label, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            group.compileCanvas();
            return group;
        }
        // Java: 177
        public class SliderRow : java.lang.Object
        {
            public com.oddlabs.tt.gui.Label caption;
            public com.oddlabs.tt.gui.Label low;
            public com.oddlabs.tt.gui.Label high;
            public com.oddlabs.tt.gui.Slider slider;
            // Java: 194
            public void place(com.oddlabs.tt.mapeditor.MapEditorForm.SliderRow below)
            {
                int section = com.oddlabs.tt.gui.Skin.getSkin().getFormData().sectionSpacing();
                if (below == null)
                {
                    caption.place();
                    low.place(caption, com.oddlabs.tt.gui.Placement.RIGHT_MID);
                    slider.place(low, com.oddlabs.tt.gui.Placement.RIGHT_MID);
                }
                else
                {
                    caption.place(below.caption, com.oddlabs.tt.gui.Placement.TOP_LEFT, section);
                    slider.place(below.slider, com.oddlabs.tt.gui.Placement.TOP_MID, section);
                    low.place(slider, com.oddlabs.tt.gui.Placement.LEFT_MID);
                }
                high.place(slider, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            }
            public SliderRow(com.oddlabs.tt.gui.Group group, string caption_key, com.oddlabs.tt.gui.Slider slider) : base()
            {
                this.caption = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.mapeditor.MapEditor.terrainI18n(caption_key), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
                this.low = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.mapeditor.MapEditor.terrainI18n("min"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
                this.high = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.mapeditor.MapEditor.terrainI18n("max"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
                this.slider = slider;
                group.addChild(caption);
                group.addChild(low);
                group.addChild(slider);
                group.addChild(high);
            }
        }
        // Java: 209
        public override void setFocus(com.oddlabs.tt.gui.FocusDirection direction)
        {
            if (((object)direction == (object)com.oddlabs.tt.gui.FocusDirection.BACKWARD))
            {
                base.setFocus(direction);
            }
            else
            {
                button_start.setFocus(direction);
            }
        }
        // Java: 218
        public void settingsChanged(com.oddlabs.tt.mapeditor.MapSettings new_settings)
        {
            if (applying || java.lang.JO.equals(new_settings, settings))
            {
                return;
            }
            settings = new_settings;
            if (heights != null || resources != null)
            {
                heights = null;
                resources = null;
                starts = null;
                map_name = null;
            }
            refresh();
        }
        // Java: 232
        public bool applyMapcode(string code)
        {
            com.oddlabs.tt.mapeditor.MapSettings parsed = settings.parseMapcode(code);
            if (parsed == null)
            {
                return false;
            }
            heights = null;
            resources = null;
            starts = null;
            map_name = null;
            showSettings(parsed);
            return true;
        }
        // Java: 245
        public void showSettings(com.oddlabs.tt.mapeditor.MapSettings new_settings)
        {
            settings = new_settings;
            applying = true;
            try
            {
                pulldown_size.chooseItem(settings.size());
                pulldown_terrain.chooseItem(settings.terrain());
                slider_hills.setValue(settings.hills());
                slider_trees.setValue(settings.trees());
                slider_supplies.setValue(settings.supplies());
            }
            finally
            {
                applying = false;
            }
            refresh();
        }
        // Java: 260
        public void refresh()
        {
            label_mapcode.clear();
            label_mapcode.append(settings.toMapcode());
            label_map.clear();
            label_map.append((map_name == null ? com.oddlabs.tt.mapeditor.MapEditor.i18n("new_map") : com.oddlabs.tt.mapeditor.MapEditor.i18n("current_map", map_name)));
        }
        // Java: 267
        public java.nio.file.Path mapsDir()
        {
            java.nio.file.Path dir = com.oddlabs.tt.mapeditor.MapEditor.getMapsDir();
            if (dir == null)
            {
                gui_root.addModalForm(new com.oddlabs.tt.form.MessageForm(com.oddlabs.tt.mapeditor.MapEditor.i18n("no_maps_dir")));
            }
            return dir;
        }
        // Java: 274
        public void save()
        {
            java.nio.file.Path dir = mapsDir();
            if (dir == null)
            {
                return;
            }
            gui_root.addModalForm(new com.oddlabs.tt.mapeditor.SaveMapDialog(gui_root, dir, (map_name != null ? map_name : ""), new java.util.function.Consumer_L((object name_l) =>
            {
                string name = (string)name_l;
                try
                {
                    com.oddlabs.tt.mapeditor.MapPreview preview = (heights != null ? com.oddlabs.tt.mapeditor.MapPreview.render(heights, settings, resources) : (com.oddlabs.tt.mapeditor.MapPreview)(null));
                    new com.oddlabs.tt.mapeditor.MapFile(name, settings, heights, resources, preview, starts).save(dir);
                    map_name = name;
                    refresh();
                    gui_root.getInfoPrinter().print(com.oddlabs.tt.mapeditor.MapEditor.i18n("saved", name));
                }
                catch (System.IO.IOException e)
                {
                    gui_root.addModalForm(new com.oddlabs.tt.form.MessageForm(com.oddlabs.tt.mapeditor.MapEditor.i18n("save_failed", e.getMessage())));
                }
            })));
        }
        // Java: 291
        public void load()
        {
            java.nio.file.Path dir = mapsDir();
            if (dir == null)
            {
                return;
            }
            gui_root.addModalForm(new com.oddlabs.tt.mapeditor.LoadMapDialog(gui_root, dir, com.oddlabs.tt.mapeditor.MapEditor.i18n("load_caption"), com.oddlabs.tt.mapeditor.MapEditor.i18n("load_button"), new java.util.function.Consumer_L((object entry_l) =>
            {
                com.oddlabs.tt.mapeditor.MapFile.Entry entry = (com.oddlabs.tt.mapeditor.MapFile.Entry)entry_l;
                com.oddlabs.tt.mapeditor.MapFile map = default;
                try
                {
                    map = com.oddlabs.tt.mapeditor.MapFile.load(entry.path());
                }
                catch (System.IO.IOException e)
                {
                    gui_root.addModalForm(new com.oddlabs.tt.form.MessageForm(com.oddlabs.tt.mapeditor.MapEditor.i18n("load_failed", e.getMessage())));
                    return;
                }
                heights = map.heights();
                resources = map.resources();
                starts = map.starts();
                map_name = map.name();
                showSettings(map.settings());
            })));
        }
        // Java: 312
        public void start()
        {
            button_start.setDisabled(true);
            com.oddlabs.tt.form.ProgressForm.setProgressForm(network, gui_root.getGUI(), new com.oddlabs.tt.mapeditor.MapEditorLoader(network, settings, map_name, heights, resources, starts));
        }
        public MapEditorForm(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root) : base()
        {
            this.network = network;
            this.gui_root = gui_root;
            long tick = com.oddlabs.tt.@event.LocalEventQueue.getQueue().getHighPrecisionManager().getTick();
            this.settings = com.oddlabs.tt.mapeditor.MapSettings.random(new java.util.Random(tick * tick));
            com.oddlabs.tt.gui.Label label_headline = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.mapeditor.MapEditor.i18n("headline"), com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont());
            addChild(label_headline);
            pulldown_size = new com.oddlabs.tt.gui.PulldownMenu();
            for (int i = 0; i < com.oddlabs.tt.mapeditor.MapSettings.NUM_SIZES; i++)
            {
                if (i != com.oddlabs.matchmaking.Game.SIZE_ARCHIPELAGO || com.oddlabs.tt.global.Globals.SHIPS_ENABLED)
                {
                    pulldown_size.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.util.ServerMessageBundler.getSizeString(i)));
                }
            }
            pulldown_terrain = new com.oddlabs.tt.gui.PulldownMenu();
            pulldown_terrain.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.util.ServerMessageBundler.getTerrainTypeString(com.oddlabs.matchmaking.Game.TERRAIN_TYPE_NATIVE)));
            pulldown_terrain.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.util.ServerMessageBundler.getTerrainTypeString(com.oddlabs.matchmaking.Game.TERRAIN_TYPE_VIKING)));
            com.oddlabs.tt.gui.Group group_size = com.oddlabs.tt.mapeditor.MapEditorForm.labelled("island_size", new com.oddlabs.tt.gui.PulldownButton(gui_root, pulldown_size, settings.size(), com.oddlabs.tt.mapeditor.MapEditorForm.PULLDOWN_WIDTH));
            com.oddlabs.tt.gui.Group group_terrain = com.oddlabs.tt.mapeditor.MapEditorForm.labelled("terrain_type", new com.oddlabs.tt.gui.PulldownButton(gui_root, pulldown_terrain, settings.terrain(), com.oddlabs.tt.mapeditor.MapEditorForm.PULLDOWN_WIDTH));
            addChild(group_size);
            addChild(group_terrain);
            pulldown_size.addItemChosenListener(new com.oddlabs.tt.guievent.ItemChosenListener_L((com.oddlabs.tt.gui.PulldownMenu unused, int index) =>
            {
                settingsChanged(settings.withSize(index));
            }));
            pulldown_terrain.addItemChosenListener(new com.oddlabs.tt.guievent.ItemChosenListener_L((com.oddlabs.tt.gui.PulldownMenu unused_2, int index_2) =>
            {
                settingsChanged(settings.withTerrain(index_2));
            }));
            com.oddlabs.tt.gui.Group group_sliders = new com.oddlabs.tt.gui.Group();
            slider_hills = new com.oddlabs.tt.gui.Slider(com.oddlabs.tt.mapeditor.MapEditorForm.SLIDER_LENGTH, 0, com.oddlabs.tt.mapeditor.MapSettings.SLIDER_MAX, settings.hills());
            slider_trees = new com.oddlabs.tt.gui.Slider(com.oddlabs.tt.mapeditor.MapEditorForm.SLIDER_LENGTH, 0, com.oddlabs.tt.mapeditor.MapSettings.SLIDER_MAX, settings.trees());
            slider_supplies = new com.oddlabs.tt.gui.Slider(com.oddlabs.tt.mapeditor.MapEditorForm.SLIDER_LENGTH, 0, com.oddlabs.tt.mapeditor.MapSettings.SLIDER_MAX, settings.supplies());
            com.oddlabs.tt.mapeditor.MapEditorForm.SliderRow row_supplies = new com.oddlabs.tt.mapeditor.MapEditorForm.SliderRow(group_sliders, "resources", slider_supplies);
            com.oddlabs.tt.mapeditor.MapEditorForm.SliderRow row_trees = new com.oddlabs.tt.mapeditor.MapEditorForm.SliderRow(group_sliders, "trees", slider_trees);
            com.oddlabs.tt.mapeditor.MapEditorForm.SliderRow row_hills = new com.oddlabs.tt.mapeditor.MapEditorForm.SliderRow(group_sliders, "hills", slider_hills);
            row_supplies.place(null);
            row_trees.place(row_supplies);
            row_hills.place(row_trees);
            group_sliders.compileCanvas();
            addChild(group_sliders);
            slider_hills.addValueListener(new com.oddlabs.tt.guievent.ValueListener_L((long @value) =>
            {
                settingsChanged(settings.withHills(unchecked((int)(@value))));
            }));
            slider_trees.addValueListener(new com.oddlabs.tt.guievent.ValueListener_L((long value_2) =>
            {
                settingsChanged(settings.withTrees(unchecked((int)(value_2))));
            }));
            slider_supplies.addValueListener(new com.oddlabs.tt.guievent.ValueListener_L((long value_3) =>
            {
                settingsChanged(settings.withSupplies(unchecked((int)(value_3))));
            }));
            com.oddlabs.tt.gui.Group group_code = new com.oddlabs.tt.gui.Group();
            com.oddlabs.tt.gui.Label label_code = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.mapeditor.MapEditor.terrainI18n("map_code"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            label_mapcode = new com.oddlabs.tt.gui.Label("", com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont(), 500);
            group_code.addChild(label_code);
            group_code.addChild(label_mapcode);
            label_code.place();
            label_mapcode.place(label_code, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            group_code.compileCanvas();
            addChild(group_code);
            label_map = new com.oddlabs.tt.gui.Label("", com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), 500);
            addChild(label_map);
            com.oddlabs.tt.gui.Group group_buttons = new com.oddlabs.tt.gui.Group();
            com.oddlabs.tt.gui.HorizButton button_mapcode = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.mapeditor.MapEditor.terrainI18n("enter_map_code"), 170);
            button_mapcode.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_3, int unused_4, int unused_5, int unused_6) =>
            {
                gui_root.addModalForm(new com.oddlabs.tt.mapeditor.MapcodeDialog(settings.toMapcode(), new java.util.function.Predicate_L((object a0) => { return this.applyMapcode(((string)a0)); })));
            }));
            com.oddlabs.tt.gui.HorizButton button_save = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.mapeditor.MapEditor.i18n("save"), com.oddlabs.tt.mapeditor.MapEditorForm.BUTTON_WIDTH);
            button_save.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_7, int unused_8, int unused_9, int unused_10) =>
            {
                save();
            }));
            com.oddlabs.tt.gui.HorizButton button_load = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.mapeditor.MapEditor.i18n("load"), com.oddlabs.tt.mapeditor.MapEditorForm.BUTTON_WIDTH);
            button_load.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_11, int unused_12, int unused_13, int unused_14) =>
            {
                load();
            }));
            button_start = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.mapeditor.MapEditor.i18n("start"), com.oddlabs.tt.mapeditor.MapEditorForm.BUTTON_WIDTH);
            button_start.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_15, int unused_16, int unused_17, int unused_18) =>
            {
                start();
            }));
            com.oddlabs.tt.gui.HorizButton button_cancel = new com.oddlabs.tt.gui.CancelButton(com.oddlabs.tt.mapeditor.MapEditorForm.BUTTON_WIDTH);
            button_cancel.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_19, int unused_20, int unused_21, int unused_22) =>
            {
                cancel();
            }));
            group_buttons.addChild(button_mapcode);
            group_buttons.addChild(button_save);
            group_buttons.addChild(button_load);
            group_buttons.addChild(button_start);
            group_buttons.addChild(button_cancel);
            button_cancel.place();
            button_start.place(button_cancel, com.oddlabs.tt.gui.Placement.LEFT_MID);
            button_load.place(button_start, com.oddlabs.tt.gui.Placement.LEFT_MID);
            button_save.place(button_load, com.oddlabs.tt.gui.Placement.LEFT_MID);
            button_mapcode.place(button_save, com.oddlabs.tt.gui.Placement.LEFT_MID);
            group_buttons.compileCanvas();
            addChild(group_buttons);
            int section = com.oddlabs.tt.gui.Skin.getSkin().getFormData().sectionSpacing();
            label_headline.place();
            group_size.place(label_headline, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT, section);
            group_terrain.place(group_size, com.oddlabs.tt.gui.Placement.BOTTOM_RIGHT);
            group_sliders.place(group_terrain, com.oddlabs.tt.gui.Placement.BOTTOM_RIGHT, section);
            group_code.place(group_sliders, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT, section);
            label_map.place(group_code, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            group_buttons.place(com.oddlabs.tt.gui.Origin.AT_END);
            compileCanvas();
            refresh();
        }
    }
}
