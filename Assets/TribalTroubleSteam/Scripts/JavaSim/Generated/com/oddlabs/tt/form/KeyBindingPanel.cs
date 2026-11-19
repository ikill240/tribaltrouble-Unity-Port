// Translated from KeyBindingPanel.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 42
    public class KeyBindingPanel : com.oddlabs.tt.gui.Panel
    {
        public const int COL_ACTION_WIDTH = 300;
        public const int COL_BINDINGS_WIDTH = 300;
        public static org.joml.Vector4f CONFLICT_COLOR;
        public static org.joml.Vector4f HEADER_COLOR;
        public const int CATEGORY_STRIDE = 10000;
        // Java: 49
        public sealed class Category : java.lang.Enum
        {
            public static Category[] values() { return (Category[])VALUES_.Clone(); }
            public static Category valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly Category GENERAL = new Category("GENERAL", 0, "category.general");
            public static readonly Category CAMERA = new Category("CAMERA", 1, "category.camera");
            public static readonly Category UI = new Category("UI", 2, "category.ui");
            public static readonly Category UNIT = new Category("UNIT", 3, "category.unit");
            public static readonly Category ARMY_GROUPS = new Category("ARMY_GROUPS", 4, "category.army_groups");
            public static readonly Category PRODUCTION = new Category("PRODUCTION", 5, "category.production");
            public static readonly Category TRAINING = new Category("TRAINING", 6, "category.training");
            public static readonly Category RESOURCES = new Category("RESOURCES", 7, "category.resources");
            public static readonly Category MAGIC = new Category("MAGIC", 8, "category.magic");
            public static readonly Category GAME = new Category("GAME", 9, "category.game");
            public static readonly Category CHEATS = new Category("CHEATS", 10, "category.cheats");
            public static readonly Category DEBUG = new Category("DEBUG", 11, "category.debug");
            public string i18nKey;
            public static readonly Category[] VALUES_ = new Category[] { GENERAL, CAMERA, UI, UNIT, ARMY_GROUPS, PRODUCTION, TRAINING, RESOURCES, MAGIC, GAME, CHEATS, DEBUG };
            public Category(string name_, int ordinal_, string i18nKey) : base(name_, ordinal_)
            {
                this.i18nKey = i18nKey;
            }
        }
        // Java: 70
        public static com.oddlabs.tt.form.KeyBindingPanel.Category categorize(com.oddlabs.tt.input.GameAction action)
        {
            string name = action.name();
            if (name.startsWith("CAMERA_"))
            {
                return com.oddlabs.tt.form.KeyBindingPanel.Category.CAMERA;
            }
            if (name.startsWith("UI_"))
            {
                return com.oddlabs.tt.form.KeyBindingPanel.Category.UI;
            }
            if (name.startsWith("ARMY_"))
            {
                return com.oddlabs.tt.form.KeyBindingPanel.Category.ARMY_GROUPS;
            }
            if (name.startsWith("PROD_"))
            {
                return com.oddlabs.tt.form.KeyBindingPanel.Category.PRODUCTION;
            }
            if (name.startsWith("TRAIN_"))
            {
                return com.oddlabs.tt.form.KeyBindingPanel.Category.TRAINING;
            }
            if (name.startsWith("RES_"))
            {
                return com.oddlabs.tt.form.KeyBindingPanel.Category.RESOURCES;
            }
            if (name.startsWith("MAGIC_"))
            {
                return com.oddlabs.tt.form.KeyBindingPanel.Category.MAGIC;
            }
            if (name.startsWith("CHEAT_"))
            {
                return com.oddlabs.tt.form.KeyBindingPanel.Category.CHEATS;
            }
            if (name.startsWith("DEBUG_"))
            {
                return com.oddlabs.tt.form.KeyBindingPanel.Category.DEBUG;
            }
            if (name.startsWith("UNIT_") || java.lang.JO.equals(name, "GAMEPLAY_BACK"))
            {
                return com.oddlabs.tt.form.KeyBindingPanel.Category.UNIT;
            }
            if (name.startsWith("GAME_SPEED_") || java.lang.JO.equals(name, "NOTIFICATION_JUMP"))
            {
                return com.oddlabs.tt.form.KeyBindingPanel.Category.GAME;
            }
            return com.oddlabs.tt.form.KeyBindingPanel.Category.GENERAL;
        }
        public com.oddlabs.tt.gui.MultiColumnComboBox list_box;
        public com.oddlabs.tt.gui.GUIRoot gui_root;
        public com.oddlabs.tt.input.GameAction last_selected_action;
        public string filter;
        // Java: 157
        public void updateList()
        {
            int savedOffset = list_box.getOffsetY();
            list_box.clear();
            com.oddlabs.tt.gui.Row rowToReselect = null;
            java.util.EnumMap byCategory = new java.util.EnumMap(typeof(com.oddlabs.tt.form.KeyBindingPanel.Category));
            foreach (com.oddlabs.tt.input.GameAction action in com.oddlabs.tt.input.GameAction.values())
            {
                if (action.name().startsWith("DEBUG_") && !com.oddlabs.tt.global.Settings.getSettings().inDeveloperMode())
                {
                    continue;
                }
                if (action.name().startsWith("CHEAT_") && !com.oddlabs.tt.render.Renderer.getRenderer().isCheater())
                {
                    continue;
                }
                com.oddlabs.tt.form.KeyBindingPanel.Category category = com.oddlabs.tt.form.KeyBindingPanel.categorize(action);
                if (!filter.isEmpty())
                {
                    string name = com.oddlabs.tt.form.AbstractOptionsMenu.i18n("action." + java.lang.JStr.valueOf(action.name()));
                    string category_name = com.oddlabs.tt.form.AbstractOptionsMenu.i18n(category.i18nKey);
                    string bindings = com.oddlabs.tt.form.KeyBindingPanel.formatBindings(com.oddlabs.tt.render.Renderer.getLocalInput().getInputManager().getBindings(action));
                    if (!name.toLowerCase().contains(filter) && !category_name.toLowerCase().contains(filter) && !bindings.toLowerCase().contains(filter))
                    {
                        continue;
                    }
                }
                ((java.util.List)byCategory.computeIfAbsent(category, new java.util.function.Function_L((object k_l) =>
                {
                    com.oddlabs.tt.form.KeyBindingPanel.Category k = (com.oddlabs.tt.form.KeyBindingPanel.Category)k_l;
                    return new java.util.ArrayList();
                }))).add(action);
            }
            foreach (com.oddlabs.tt.form.KeyBindingPanel.Category category_2 in com.oddlabs.tt.form.KeyBindingPanel.Category.values())
            {
                java.util.List actions = ((java.util.List)byCategory.get(category_2));
                if (actions == null || actions.isEmpty())
                {
                    continue;
                }
                actions.sort(new java.util.Comparator_L((object a_l, object b_l) =>
                {
                    com.oddlabs.tt.input.GameAction a = (com.oddlabs.tt.input.GameAction)a_l;
                    com.oddlabs.tt.input.GameAction b = (com.oddlabs.tt.input.GameAction)b_l;
                    return com.oddlabs.tt.form.AbstractOptionsMenu.i18n("action." + java.lang.JStr.valueOf(a.name())).compareToIgnoreCase(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("action." + java.lang.JStr.valueOf(b.name())));
                }));
                int categoryBase = category_2.ordinal() * com.oddlabs.tt.form.KeyBindingPanel.CATEGORY_STRIDE;
                string headerText = "-- " + java.lang.JStr.valueOf(com.oddlabs.tt.form.AbstractOptionsMenu.i18n(category_2.i18nKey)) + " --";
                com.oddlabs.tt.gui.Label headerLeft = new com.oddlabs.tt.gui.SortedLabel(headerText, categoryBase, com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font());
                headerLeft.setColor(com.oddlabs.tt.form.KeyBindingPanel.HEADER_COLOR);
                com.oddlabs.tt.gui.Label headerRight = new com.oddlabs.tt.gui.Label("", com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font());
                list_box.addRow(new com.oddlabs.tt.gui.Row(new com.oddlabs.tt.gui.Label[] { headerLeft, headerRight }, null));
                int withinCategory = 1;
                foreach (com.oddlabs.tt.input.GameAction action_2 in java.lang.JIter.of(actions))
                {
                    string name_2 = com.oddlabs.tt.form.AbstractOptionsMenu.i18n("action." + java.lang.JStr.valueOf(action_2.name()));
                    java.util.NavigableSet bindings_2 = com.oddlabs.tt.render.Renderer.getLocalInput().getInputManager().getBindings(action_2);
                    com.oddlabs.tt.gui.Label l2 = default;
                    if (bindings_2.isEmpty())
                    {
                        l2 = new com.oddlabs.tt.form.KeyBindingPanel.InvertedLabel(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("unassigned"), com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font(), com.oddlabs.tt.form.KeyBindingPanel.COL_BINDINGS_WIDTH);
                    }
                    else
                    {
                        l2 = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.KeyBindingPanel.formatBindings(bindings_2), com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font());
                    }
                    com.oddlabs.tt.gui.Label l1 = new com.oddlabs.tt.gui.SortedLabel(name_2, categoryBase + withinCategory, com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font());
                    if (!com.oddlabs.tt.input.KeyBindingConflicts.findExistingConflicts(action_2, com.oddlabs.tt.render.Renderer.getLocalInput().getInputManager()).isEmpty())
                    {
                        l1.setColor(com.oddlabs.tt.form.KeyBindingPanel.CONFLICT_COLOR);
                        l2.setColor(com.oddlabs.tt.form.KeyBindingPanel.CONFLICT_COLOR);
                    }
                    com.oddlabs.tt.gui.Row row = new com.oddlabs.tt.gui.Row(new com.oddlabs.tt.gui.Label[] { l1, l2 }, action_2);
                    list_box.addRow(row);
                    if (((object)action_2 == (object)last_selected_action))
                    {
                        rowToReselect = row;
                    }
                    withinCategory++;
                }
            }
            list_box.setOffsetY(savedOffset);
            if (rowToReselect != null)
            {
                list_box.selectRow(rowToReselect);
            }
        }
        // Java: 234
        public static string formatBindings(java.util.Set bindings)
        {
            bool isMac = java.lang.JSystem.getProperty("os.name", "").toLowerCase().contains("mac");
            return ((string)bindings.stream().map(new java.util.function.Function_L((object b_l) =>
            {
                com.oddlabs.tt.input.InputBinding b = (com.oddlabs.tt.input.InputBinding)b_l;
                string s = "";
                if (isMac)
                {
                    if (b.control())
                    {
                        s = java.lang.JStr.valueOf(s) + "\u2303";
                    }
                    if (b.alt())
                    {
                        s = java.lang.JStr.valueOf(s) + "\u2325";
                    }
                    if (b.shift())
                    {
                        s = java.lang.JStr.valueOf(s) + "\u21e7";
                    }
                    if (b.meta())
                    {
                        s = java.lang.JStr.valueOf(s) + "\u2318";
                    }
                }
                else
                {
                    if (b.control())
                    {
                        s = java.lang.JStr.valueOf(s) + "Ctrl+";
                    }
                    if (b.alt())
                    {
                        s = java.lang.JStr.valueOf(s) + "Alt+";
                    }
                    if (b.shift())
                    {
                        s = java.lang.JStr.valueOf(s) + "Shift+";
                    }
                    if (b.meta())
                    {
                        s = java.lang.JStr.valueOf(s) + "Meta+";
                    }
                }
                return java.lang.JStr.valueOf(s) + java.lang.JStr.valueOf(b.key().getDisplayName());
            })).collect(java.util.stream.Collectors.joining(", ")));
        }
        // Java: 255
        public class FilterLine : com.oddlabs.tt.gui.EditLine
        {
            public com.oddlabs.tt.form.KeyBindingPanel this_0;
            // Java: 260
            public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
            {
                base.handleInput(@event);
                string text = getContents().trim().toLowerCase();
                if (!java.lang.JO.equals(text, this_0.filter))
                {
                    this_0.filter = text;
                    this_0.updateList();
                    this_0.list_box.setOffsetY(0);
                }
            }
            public FilterLine(com.oddlabs.tt.form.KeyBindingPanel this_0_) : base(250, 40)
            {
                this.this_0 = this_0_;
            }
        }
        // Java: 272
        public void saveMappings()
        {
            bool wasFullscreen = com.oddlabs.tt.global.Settings.getSettings().fullscreen;
            if (wasFullscreen)
            {
                com.oddlabs.tt.render.Renderer.getRenderer().toggleFullscreen();
            }
            string path = org.lwjgl.util.tinyfd.TinyFileDialogs.tinyfd_saveFileDialog(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("dialog_save_bindings"), "", null, com.oddlabs.tt.form.AbstractOptionsMenu.i18n("json_files"));
            if (path != null)
            {
                string json = com.oddlabs.tt.render.Renderer.getLocalInput().getInputManager().exportBindings();
                try
                {
                    java.nio.file.Files.writeString(java.nio.file.Path_.of(path), json);
                }
                catch (System.IO.IOException e)
                {
                    gui_root.addModalForm(new com.oddlabs.tt.form.MessageForm(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("error_save_failed", e.getMessage())));
                }
            }
            if (wasFullscreen)
            {
                com.oddlabs.tt.render.Renderer.getRenderer().toggleFullscreen();
            }
        }
        // Java: 294
        public void loadMappings()
        {
            bool wasFullscreen = com.oddlabs.tt.global.Settings.getSettings().fullscreen;
            if (wasFullscreen)
            {
                com.oddlabs.tt.render.Renderer.getRenderer().toggleFullscreen();
            }
            string path = org.lwjgl.util.tinyfd.TinyFileDialogs.tinyfd_openFileDialog(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("dialog_load_bindings"), "", null, com.oddlabs.tt.form.AbstractOptionsMenu.i18n("json_files"), false);
            if (path != null)
            {
                try
                {
                    string json = java.nio.file.Files.readString(java.nio.file.Path_.of(path));
                    com.oddlabs.tt.render.Renderer.getLocalInput().getInputManager().importBindings(json);
                    updateList();
                }
                catch (System.IO.IOException e)
                {
                    gui_root.addModalForm(new com.oddlabs.tt.form.MessageForm(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("error_load_failed", e.getMessage())));
                }
            }
            if (wasFullscreen)
            {
                com.oddlabs.tt.render.Renderer.getRenderer().toggleFullscreen();
            }
        }
        // Java: 317
        public class InvertedLabel : com.oddlabs.tt.gui.Label
        {
            // Java: 323
            public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
            {
                renderer.drawColoredQuad(0, 0, getWidth(), getHeight(), com.oddlabs.tt.gui.Label.DEFAULT_COLOR);
                base.renderGeometry(renderer);
            }
            public InvertedLabel(string text, com.oddlabs.tt.font.Font font, int width) : base(text, font, width, com.oddlabs.tt.gui.Origin.AT_MIDDLE)
            {
                setColor(com.oddlabs.util.Color.BLACK);
            }
        }
        static KeyBindingPanel()
        {
            KeyBindingPanel.CONFLICT_COLOR = new org.joml.Vector4f(1.0f, 0.3f, 0.3f, 1.0f);
            KeyBindingPanel.HEADER_COLOR = new org.joml.Vector4f(0.9f, 0.75f, 0.4f, 1.0f);
        }
        private void init_KeyBindingPanel()
        {
            this.filter = "";
        }
        public KeyBindingPanel(com.oddlabs.tt.gui.GUIRoot gui_root) : base(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("key_bindings_title"))
        {
            init_KeyBindingPanel();
            this.gui_root = gui_root;
            com.oddlabs.tt.gui.ColumnInfo[] infos = new com.oddlabs.tt.gui.ColumnInfo[] { new com.oddlabs.tt.gui.ColumnInfo(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("column_action"), com.oddlabs.tt.form.KeyBindingPanel.COL_ACTION_WIDTH), new com.oddlabs.tt.gui.ColumnInfo(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("column_bindings"), com.oddlabs.tt.form.KeyBindingPanel.COL_BINDINGS_WIDTH) };
            list_box = new com.oddlabs.tt.gui.MultiColumnComboBox(gui_root, infos, 300, false);
            addChild(list_box);
            com.oddlabs.tt.gui.Group filter_group = new com.oddlabs.tt.gui.Group();
            com.oddlabs.tt.gui.Label filter_label = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("filter_bindings"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            filter_group.addChild(filter_label);
            com.oddlabs.tt.gui.EditLine filter_line = new com.oddlabs.tt.form.KeyBindingPanel.FilterLine(this);
            filter_group.addChild(filter_line);
            filter_label.place();
            filter_line.place(filter_label, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            filter_group.compileCanvas();
            addChild(filter_group);
            updateList();
            list_box.addRowListener(new KeyBindingPanel.Anon1_RowListener(this, gui_root));
            com.oddlabs.tt.gui.Group button_group = new com.oddlabs.tt.gui.Group();
            addChild(button_group);
            com.oddlabs.tt.gui.HorizButton btn_reset = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("btn_reset_all"), 100);
            btn_reset.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                gui_root.addModalForm(new com.oddlabs.tt.form.QuestionForm(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("confirm_reset_all"), new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_5, int unused_6, int unused_7, int unused_8) =>
                {
                    com.oddlabs.tt.render.Renderer.getLocalInput().getInputManager().resetToDefaults();
                    updateList();
                })));
            }));
            button_group.addChild(btn_reset);
            com.oddlabs.tt.gui.HorizButton btn_save = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("btn_save_bindings"), 100);
            btn_save.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_9, int unused_10, int unused_11, int unused_12) =>
            {
                saveMappings();
            }));
            button_group.addChild(btn_save);
            com.oddlabs.tt.gui.HorizButton btn_load = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("btn_load_bindings"), 100);
            btn_load.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_13, int unused_14, int unused_15, int unused_16) =>
            {
                loadMappings();
            }));
            button_group.addChild(btn_load);
            btn_reset.place();
            btn_save.place(btn_reset, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            btn_load.place(btn_save, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            button_group.compileCanvas();
            filter_group.place();
            list_box.place(filter_group, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            button_group.place(list_box, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            compileCanvas();
        }
        // Java: anonymous class at line 114
        public sealed class Anon1_RowListener : java.lang.Object, com.oddlabs.tt.guievent.RowListener
        {
            public com.oddlabs.tt.form.KeyBindingPanel this_0;
            public com.oddlabs.tt.gui.GUIRoot gui_root;
            public Anon1_RowListener(com.oddlabs.tt.form.KeyBindingPanel this_0_, com.oddlabs.tt.gui.GUIRoot gui_root_c)
            {
                this.this_0 = this_0_;
                this.gui_root = gui_root_c;
            }
            // Java: 115
            public void rowDoubleClicked(object action_p)
            {
                com.oddlabs.tt.input.GameAction action = (com.oddlabs.tt.input.GameAction)action_p;
                this_0.last_selected_action = action;
                gui_root.addModalForm(new com.oddlabs.tt.form.KeyBindingDialog(gui_root, action, new java.util.function.Consumer_L((object bindings_l) =>
                {
                    java.util.Set bindings = (java.util.Set)bindings_l;
                    com.oddlabs.tt.render.Renderer.getLocalInput().getInputManager().setBindings(action, bindings);
                    this_0.updateList();
                })));
            }
            // mixin of default method RowListener.rowChosen
            public void rowChosen(object row_context)
            {
            }
        }
    }
}
