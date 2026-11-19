// Translated from LoadMapDialog.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.mapeditor
{
    // Java: 48
    public class LoadMapDialog : com.oddlabs.tt.gui.Form
    {
        public const int BUTTON_WIDTH = 100;
        public const int NAME_WIDTH = 230;
        public const int LIST_HEIGHT = 300;
        public const int PREVIEW_SIZE = 256;
        public const string PARENT = "..";
        public static java.nio.file.Path last_dir;
        // Java: 59
        public class Item : java.lang.Object
        {
            public java.nio.file.Path path() { return path_f; }
            public com.oddlabs.tt.mapeditor.MapFile.Entry entry() { return entry_f; }
            public Item(java.nio.file.Path path, com.oddlabs.tt.mapeditor.MapFile.Entry entry)
            {
                this.path_f = path;
                this.entry_f = entry;
            }
            public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is Item r)) return false; return true && java.lang.JO.equals(path_f, r.path_f) && java.lang.JO.equals(entry_f, r.entry_f); }
            public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(path_f); h = 31 * h + java.lang.JO.hashCode(entry_f); } return h; }
            public override string toString() { return "Item[" + "path=" + java.lang.JO.toString(path_f) + ", entry=" + java.lang.JO.toString(entry_f) + "]"; }
            public java.nio.file.Path path_f;
            public com.oddlabs.tt.mapeditor.MapFile.Entry entry_f;
        }
        public com.oddlabs.tt.gui.GUIRoot gui_root;
        public java.util.function.Consumer load;
        public com.oddlabs.tt.gui.EditLine editline_dir;
        public com.oddlabs.tt.gui.MultiColumnComboBox list;
        public com.oddlabs.tt.mapeditor.MapPreviewView preview;
        public com.oddlabs.tt.gui.Label label_info;
        public java.util.Map previews;
        public java.nio.file.Path dir;
        // Java: 155
        public override void setFocus(com.oddlabs.tt.gui.FocusDirection direction)
        {
            if (((object)direction == (object)com.oddlabs.tt.gui.FocusDirection.BACKWARD))
            {
                base.setFocus(direction);
            }
            else
            {
                list.setFocus(direction);
            }
        }
        // Java: 165
        public void browseTyped()
        {
            java.nio.file.Path typed = default;
            try
            {
                typed = java.nio.file.Path_.of(editline_dir.getContents().trim());
            }
            catch (System.Exception unused)
            {
                editline_dir.triggerError();
                return;
            }
            if (!browse(typed))
            {
                editline_dir.triggerError();
            }
            else
            {
                list.setFocus(com.oddlabs.tt.gui.FocusDirection.FORWARD);
            }
        }
        // Java: 180
        public bool browse(java.nio.file.Path path)
        {
            java.nio.file.Path target = path.toAbsolutePath().normalize();
            if (!java.nio.file.Files.isDirectory(target))
            {
                return false;
            }
            dir = target;
            com.oddlabs.tt.mapeditor.LoadMapDialog.last_dir = target;
            editline_dir.set(java.lang.JO.toString(target));
            refresh();
            return true;
        }
        // Java: 191
        public void refresh()
        {
            list.clear();
            com.oddlabs.tt.font.Font font = com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font();
            int index = 0;
            java.nio.file.Path parent = dir.getParent();
            if (parent != null)
            {
                list.addRow(com.oddlabs.tt.mapeditor.LoadMapDialog.folderRow(com.oddlabs.tt.mapeditor.LoadMapDialog.PARENT, parent, index++, font));
            }
            foreach (java.nio.file.Path sub in java.lang.JIter.of(com.oddlabs.tt.mapeditor.LoadMapDialog.folders(dir)))
            {
                list.addRow(com.oddlabs.tt.mapeditor.LoadMapDialog.folderRow(java.lang.JO.toString(sub.getFileName()), sub, index++, font));
            }
            foreach (com.oddlabs.tt.mapeditor.MapFile.Entry entry in java.lang.JIter.of(com.oddlabs.tt.mapeditor.MapFile.list(dir)))
            {
                string edited = (entry.edited() ? com.oddlabs.tt.mapeditor.MapEditor.i18n("edited_yes") : "");
                list.addRow(new com.oddlabs.tt.gui.Row(java.util.List_.of(new com.oddlabs.tt.gui.SortedLabel(entry.name(), index++, font), new com.oddlabs.tt.gui.Label(com.oddlabs.tt.util.ServerMessageBundler.getSizeString(entry.settings().size()), font), new com.oddlabs.tt.gui.Label(edited, font), new com.oddlabs.tt.gui.DateLabel(entry.modified().toMillis(), font)), new com.oddlabs.tt.mapeditor.LoadMapDialog.Item(entry.path(), entry)));
            }
            if (list.getSize() > 0)
            {
                list.selectFirst();
            }
            else
            {
                showPreview(null);
            }
        }
        // Java: 216
        public static com.oddlabs.tt.gui.Row folderRow(string name, java.nio.file.Path path, int index, com.oddlabs.tt.font.Font font)
        {
            long modified = default;
            try
            {
                modified = java.nio.file.Files.getLastModifiedTime(path).toMillis();
            }
            catch (System.IO.IOException unused)
            {
                modified = 0;
            }
            return new com.oddlabs.tt.gui.Row(java.util.List_.of(new com.oddlabs.tt.gui.SortedLabel(java.lang.JStr.valueOf(name) + "/", index, font), new com.oddlabs.tt.gui.Label("", font), new com.oddlabs.tt.gui.Label("", font), new com.oddlabs.tt.gui.DateLabel(modified, font)), new com.oddlabs.tt.mapeditor.LoadMapDialog.Item(path, null));
        }
        // Java: 233
        public static java.util.List folders(java.nio.file.Path dir)
        {
            java.util.List folders_v = new java.util.ArrayList();
            {
                java.util.stream.Stream paths = java.nio.file.Files.list(dir);
                try
                {
                    foreach (java.nio.file.Path path in java.lang.JIter.of(((java.lang.Iterable)new java.lang.Iterable_L(() => { return paths.iterator(); }))))
                    {
                        string name = java.lang.JO.toString(path.getFileName());
                        if (!name.startsWith(".") && java.nio.file.Files.isDirectory(path) && java.nio.file.Files.isReadable(path))
                        {
                            folders_v.add(path);
                        }
                    }
                }
                catch (System.IO.IOException e)
                {
                    java.lang.IO.println("Could not list folders in " + java.lang.JStr.valueOf(dir) + ": " + java.lang.JStr.valueOf(e));
                }
                finally
                {
                    if (paths != null) paths.close();
                }
            }
            folders_v.sort(new java.util.Comparator_L((object a_l, object b_l) =>
            {
                java.nio.file.Path a = (java.nio.file.Path)a_l;
                java.nio.file.Path b = (java.nio.file.Path)b_l;
                return java.lang.JO.toString(a.getFileName()).compareToIgnoreCase(java.lang.JO.toString(b.getFileName()));
            }));
            return folders_v;
        }
        // Java: 248
        public void showPreview(com.oddlabs.tt.mapeditor.LoadMapDialog.Item item)
        {
            com.oddlabs.tt.mapeditor.MapFile.Entry entry = (item != null ? item.entry() : (com.oddlabs.tt.mapeditor.MapFile.Entry)(null));
            if (entry == null)
            {
                preview.show(null, (item != null ? com.oddlabs.tt.mapeditor.MapEditor.i18n("folder_preview") : ""));
                label_info.set("");
                return;
            }
            java.util.Optional found = ((java.util.Optional)previews.computeIfAbsent(entry.path(), new java.util.function.Function_L((object path_l) =>
            {
                java.nio.file.Path path = (java.nio.file.Path)path_l;
                try
                {
                    return java.util.Optional.ofNullable(com.oddlabs.tt.mapeditor.MapFile.loadPreview(path));
                }
                catch (System.IO.IOException e)
                {
                    java.lang.IO.println("Could not read the preview of " + java.lang.JStr.valueOf(path) + ": " + java.lang.JStr.valueOf(e));
                    return java.util.Optional.empty();
                }
            })));
            preview.show(((com.oddlabs.tt.mapeditor.MapPreview)found.orElse(null)), com.oddlabs.tt.mapeditor.MapEditor.i18n("no_preview"));
            label_info.set(com.oddlabs.tt.mapeditor.MapEditor.i18n("map_info", com.oddlabs.tt.util.ServerMessageBundler.getSizeString(entry.settings().size()), com.oddlabs.tt.util.ServerMessageBundler.getTerrainTypeString(entry.settings().terrain())));
        }
        // Java: 269
        public void open(com.oddlabs.tt.mapeditor.LoadMapDialog.Item item)
        {
            com.oddlabs.tt.mapeditor.MapFile.Entry entry = item.entry();
            if (entry == null)
            {
                browse(item.path());
            }
            else
            {
                remove();
                load.accept(entry);
            }
        }
        // Java: 279
        public void delete(com.oddlabs.tt.mapeditor.MapFile.Entry entry)
        {
            try
            {
                java.nio.file.Files.deleteIfExists(entry.path());
            }
            catch (System.IO.IOException e)
            {
                gui_root.addModalForm(new com.oddlabs.tt.form.MessageForm(com.oddlabs.tt.mapeditor.MapEditor.i18n("delete_failed", e.getMessage())));
            }
            previews.remove(entry.path());
            refresh();
        }
        private void init_LoadMapDialog()
        {
            this.previews = new java.util.HashMap();
        }
        public LoadMapDialog(com.oddlabs.tt.gui.GUIRoot gui_root, java.nio.file.Path start_dir, string caption, string action, java.util.function.Consumer load) : base(caption)
        {
            init_LoadMapDialog();
            this.gui_root = gui_root;
            this.load = load;
            this.dir = (com.oddlabs.tt.mapeditor.LoadMapDialog.last_dir != null && java.nio.file.Files.isDirectory(com.oddlabs.tt.mapeditor.LoadMapDialog.last_dir) ? com.oddlabs.tt.mapeditor.LoadMapDialog.last_dir : start_dir);
            com.oddlabs.tt.gui.Label label_dir = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.mapeditor.MapEditor.i18n("folder"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            com.oddlabs.tt.gui.HorizButton button_go = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.mapeditor.MapEditor.i18n("go_button"), com.oddlabs.tt.mapeditor.LoadMapDialog.BUTTON_WIDTH);
            button_go.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                browseTyped();
            }));
            com.oddlabs.tt.gui.ColumnInfo[] columns = new com.oddlabs.tt.gui.ColumnInfo[] { new com.oddlabs.tt.gui.ColumnInfo(com.oddlabs.tt.mapeditor.MapEditor.i18n("column_name"), com.oddlabs.tt.mapeditor.LoadMapDialog.NAME_WIDTH), new com.oddlabs.tt.gui.ColumnInfo(com.oddlabs.tt.mapeditor.MapEditor.i18n("column_size"), 110), new com.oddlabs.tt.gui.ColumnInfo(com.oddlabs.tt.mapeditor.MapEditor.i18n("column_edited"), 70), new com.oddlabs.tt.gui.ColumnInfo(com.oddlabs.tt.mapeditor.MapEditor.i18n("column_modified"), 150) };
            list = new com.oddlabs.tt.gui.MultiColumnComboBox(gui_root, columns, com.oddlabs.tt.mapeditor.LoadMapDialog.LIST_HEIGHT);
            list.addRowListener(new LoadMapDialog.Anon1_RowListener(this));
            preview = new com.oddlabs.tt.mapeditor.MapPreviewView(com.oddlabs.tt.mapeditor.LoadMapDialog.PREVIEW_SIZE);
            label_info = new com.oddlabs.tt.gui.Label("", com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), com.oddlabs.tt.mapeditor.LoadMapDialog.PREVIEW_SIZE);
            int dir_width = list.getWidth() + com.oddlabs.tt.gui.Skin.getSkin().getFormData().objectSpacing() + com.oddlabs.tt.mapeditor.LoadMapDialog.PREVIEW_SIZE - label_dir.getWidth() - button_go.getWidth() - 2 * com.oddlabs.tt.gui.Skin.getSkin().getFormData().objectSpacing();
            editline_dir = new com.oddlabs.tt.gui.EditLine(dir_width, 1024);
            editline_dir.addEnterListener(new com.oddlabs.tt.guievent.EnterListener_L((string unused_5) =>
            {
                browseTyped();
            }));
            com.oddlabs.tt.gui.HorizButton button_load = new com.oddlabs.tt.gui.HorizButton(action, com.oddlabs.tt.mapeditor.LoadMapDialog.BUTTON_WIDTH);
            button_load.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_6, int unused_7, int unused_8, int unused_9) =>
            {
                com.oddlabs.tt.mapeditor.LoadMapDialog.Item selected = ((com.oddlabs.tt.mapeditor.LoadMapDialog.Item)list.getSelected());
                if (selected != null)
                {
                    open(selected);
                }
            }));
            com.oddlabs.tt.gui.HorizButton button_delete = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.mapeditor.MapEditor.i18n("delete_button"), com.oddlabs.tt.mapeditor.LoadMapDialog.BUTTON_WIDTH);
            button_delete.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_10, int unused_11, int unused_12, int unused_13) =>
            {
                com.oddlabs.tt.mapeditor.LoadMapDialog.Item selected_2 = ((com.oddlabs.tt.mapeditor.LoadMapDialog.Item)list.getSelected());
                if (selected_2 != null && selected_2.entry() != null)
                {
                    com.oddlabs.tt.mapeditor.MapFile.Entry entry = selected_2.entry();
                    gui_root.addModalForm(new com.oddlabs.tt.form.QuestionForm(com.oddlabs.tt.mapeditor.MapEditor.i18n("delete_confirm", entry.name()), new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_14, int unused_15, int unused_16, int unused_17) =>
                    {
                        delete(entry);
                    })));
                }
            }));
            com.oddlabs.tt.gui.HorizButton button_cancel = new com.oddlabs.tt.gui.CancelButton(com.oddlabs.tt.mapeditor.LoadMapDialog.BUTTON_WIDTH);
            button_cancel.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_18, int unused_19, int unused_20, int unused_21) =>
            {
                cancel();
            }));
            addChild(label_dir);
            addChild(editline_dir);
            addChild(button_go);
            addChild(list);
            addChild(preview);
            addChild(label_info);
            addChild(button_load);
            addChild(button_delete);
            addChild(button_cancel);
            label_dir.place();
            editline_dir.place(label_dir, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            button_go.place(editline_dir, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            list.place(label_dir, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            preview.place(list, com.oddlabs.tt.gui.Placement.RIGHT_TOP);
            label_info.place(preview, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            button_cancel.place(preview, com.oddlabs.tt.gui.Placement.BOTTOM_RIGHT, list.getHeight() - com.oddlabs.tt.mapeditor.LoadMapDialog.PREVIEW_SIZE + com.oddlabs.tt.gui.Skin.getSkin().getFormData().objectSpacing());
            button_load.place(button_cancel, com.oddlabs.tt.gui.Placement.LEFT_MID);
            button_delete.place(button_load, com.oddlabs.tt.gui.Placement.LEFT_MID);
            compileCanvas();
            centerPos();
            browse(dir);
        }
        // Java: anonymous class at line 92
        public sealed class Anon1_RowListener : java.lang.Object, com.oddlabs.tt.guievent.RowListener
        {
            public com.oddlabs.tt.mapeditor.LoadMapDialog this_0;
            public Anon1_RowListener(com.oddlabs.tt.mapeditor.LoadMapDialog this_0_)
            {
                this.this_0 = this_0_;
            }
            // Java: 93
            public void rowChosen(object item_p)
            {
                com.oddlabs.tt.mapeditor.LoadMapDialog.Item item = (com.oddlabs.tt.mapeditor.LoadMapDialog.Item)item_p;
                this_0.showPreview(item);
            }
            // Java: 98
            public void rowDoubleClicked(object item_p)
            {
                com.oddlabs.tt.mapeditor.LoadMapDialog.Item item = (com.oddlabs.tt.mapeditor.LoadMapDialog.Item)item_p;
                this_0.open(item);
            }
        }
    }
}
