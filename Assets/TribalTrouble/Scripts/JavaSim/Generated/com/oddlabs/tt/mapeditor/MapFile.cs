// Translated from MapFile.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.mapeditor
{
    // Java: 31
    public class MapFile : java.lang.Object
    {
        public string name() { return name_f; }
        public com.oddlabs.tt.mapeditor.MapSettings settings() { return settings_f; }
        public float[][] heights() { return heights_f; }
        public com.oddlabs.tt.mapeditor.MapFile.Resources resources() { return resources_f; }
        public com.oddlabs.tt.mapeditor.MapPreview preview() { return preview_f; }
        public java.util.List starts() { return starts_f; }
        public MapFile(string name, com.oddlabs.tt.mapeditor.MapSettings settings, float[][] heights, com.oddlabs.tt.mapeditor.MapFile.Resources resources, com.oddlabs.tt.mapeditor.MapPreview preview, java.util.List starts)
        {
            this.name_f = name;
            this.settings_f = settings;
            this.heights_f = heights;
            this.resources_f = resources;
            this.preview_f = preview;
            this.starts_f = starts;
        }
        public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is MapFile r)) return false; return true && java.lang.JO.equals(name_f, r.name_f) && java.lang.JO.equals(settings_f, r.settings_f) && java.lang.JO.equals(heights_f, r.heights_f) && java.lang.JO.equals(resources_f, r.resources_f) && java.lang.JO.equals(preview_f, r.preview_f) && java.lang.JO.equals(starts_f, r.starts_f); }
        public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(name_f); h = 31 * h + java.lang.JO.hashCode(settings_f); h = 31 * h + java.lang.JO.hashCode(heights_f); h = 31 * h + java.lang.JO.hashCode(resources_f); h = 31 * h + java.lang.JO.hashCode(preview_f); h = 31 * h + java.lang.JO.hashCode(starts_f); } return h; }
        public override string toString() { return "MapFile[" + "name=" + java.lang.JO.toString(name_f) + ", settings=" + java.lang.JO.toString(settings_f) + ", heights=" + java.lang.JO.toString(heights_f) + ", resources=" + java.lang.JO.toString(resources_f) + ", preview=" + java.lang.JO.toString(preview_f) + ", starts=" + java.lang.JO.toString(starts_f) + "]"; }
        public string name_f;
        public com.oddlabs.tt.mapeditor.MapSettings settings_f;
        public float[][] heights_f;
        public com.oddlabs.tt.mapeditor.MapFile.Resources resources_f;
        public com.oddlabs.tt.mapeditor.MapPreview preview_f;
        public java.util.List starts_f;
        public const string EXTENSION = ".ttmap";
        public const int MAGIC = 1414810960;
        public const int VERSION = 4;
        // Java: 41
        public class Resources : java.lang.Object
        {
            public java.util.List[] positions() { return positions_f; }
            public Resources(java.util.List[] positions)
            {
                this.positions_f = positions;
            }
            public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is Resources r)) return false; return true && java.lang.JO.equals(positions_f, r.positions_f); }
            public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(positions_f); } return h; }
            public override string toString() { return "Resources[" + "positions=" + java.lang.JO.toString(positions_f) + "]"; }
            public java.util.List[] positions_f;
            // Java: 42
            public java.util.List of(com.oddlabs.tt.mapeditor.Resource kind)
            {
                return positions_f[kind.ordinal()];
            }
        }
        public const int MAX_NAME_LENGTH = 48;
        // Java: 49
        public class Entry : java.lang.Object
        {
            public string name() { return name_f; }
            public java.nio.file.Path path() { return path_f; }
            public com.oddlabs.tt.mapeditor.MapSettings settings() { return settings_f; }
            public bool edited() { return edited_f; }
            public java.nio.file.attribute.FileTime modified() { return modified_f; }
            public Entry(string name, java.nio.file.Path path, com.oddlabs.tt.mapeditor.MapSettings settings, bool edited, java.nio.file.attribute.FileTime modified)
            {
                this.name_f = name;
                this.path_f = path;
                this.settings_f = settings;
                this.edited_f = edited;
                this.modified_f = modified;
            }
            public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is Entry r)) return false; return true && java.lang.JO.equals(name_f, r.name_f) && java.lang.JO.equals(path_f, r.path_f) && java.lang.JO.equals(settings_f, r.settings_f) && java.lang.JO.equals(edited_f, r.edited_f) && java.lang.JO.equals(modified_f, r.modified_f); }
            public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(name_f); h = 31 * h + java.lang.JO.hashCode(path_f); h = 31 * h + java.lang.JO.hashCode(settings_f); h = 31 * h + java.lang.JO.hashCode(edited_f); h = 31 * h + java.lang.JO.hashCode(modified_f); } return h; }
            public override string toString() { return "Entry[" + "name=" + java.lang.JO.toString(name_f) + ", path=" + java.lang.JO.toString(path_f) + ", settings=" + java.lang.JO.toString(settings_f) + ", edited=" + java.lang.JO.toString(edited_f) + ", modified=" + java.lang.JO.toString(modified_f) + "]"; }
            public string name_f;
            public java.nio.file.Path path_f;
            public com.oddlabs.tt.mapeditor.MapSettings settings_f;
            public bool edited_f;
            public java.nio.file.attribute.FileTime modified_f;
        }
        // Java: 53
        public static int getMaxNameLength()
        {
            return com.oddlabs.tt.mapeditor.MapFile.MAX_NAME_LENGTH;
        }
        // Java: 58
        public static bool isValidName(string name_v)
        {
            if (name_v.isEmpty() || name_v.length() > com.oddlabs.tt.mapeditor.MapFile.MAX_NAME_LENGTH || name_v.startsWith(".") || name_v.endsWith(".") || name_v.endsWith(" "))
            {
                return false;
            }
            for (int i = 0; i < name_v.length(); i++)
            {
                char c = name_v.charAt(i);
                if (c < ' ' || "/\\:*?\"<>|".indexOf(c) != (-1))
                {
                    return false;
                }
            }
            return true;
        }
        // Java: 71
        public static int gridSize(com.oddlabs.tt.mapeditor.MapSettings settings_v)
        {
            return settings_v.getMetersPerWorld() / com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID;
        }
        // Java: 75
        public static java.nio.file.Path pathFor(java.nio.file.Path dir, string name_v)
        {
            return dir.resolve(java.lang.JStr.valueOf(name_v) + java.lang.JStr.valueOf(com.oddlabs.tt.mapeditor.MapFile.EXTENSION));
        }
        // Java: 79
        public void save(java.nio.file.Path dir)
        {
            java.nio.file.Files.createDirectories(dir);
            java.nio.file.Path target = com.oddlabs.tt.mapeditor.MapFile.pathFor(dir, name_f);
            java.nio.file.Path temp = java.nio.file.Files.createTempFile(dir, name_f, ".tmp");
            try
            {
                {
                    java.io.DataOutputStream @out = new java.io.DataOutputStream(new java.io.BufferedOutputStream(new java.util.zip.GZIPOutputStream(java.nio.file.Files.newOutputStream(temp))));
                    try
                    {
                        @out.writeInt(com.oddlabs.tt.mapeditor.MapFile.MAGIC);
                        @out.writeInt(com.oddlabs.tt.mapeditor.MapFile.VERSION);
                        com.oddlabs.tt.mapeditor.MapFile.writeSettings(@out, settings_f);
                        @out.writeBoolean(heights_f != null);
                        @out.writeBoolean(resources_f != null);
                        @out.writeBoolean(preview_f != null);
                        if (preview_f != null)
                        {
                            @out.writeShort(preview_f.size());
                            @out.write(preview_f.rgb());
                        }
                        if (heights_f != null)
                        {
                            @out.writeInt(heights_f.Length);
                            foreach (float[] row in heights_f)
                            {
                                foreach (float height in row)
                                {
                                    @out.writeFloat(height);
                                }
                            }
                        }
                        if (resources_f != null)
                        {
                            foreach (com.oddlabs.tt.mapeditor.Resource kind in com.oddlabs.tt.mapeditor.Resource.values())
                            {
                                java.util.List positions = resources_f.of(kind);
                                @out.writeInt(positions.size());
                                foreach (int[] position in java.lang.JIter.of(positions))
                                {
                                    @out.writeShort(position[0]);
                                    @out.writeShort(position[1]);
                                }
                            }
                        }
                        java.util.List start_list = (starts_f != null ? starts_f : java.util.List_.of());
                        @out.writeInt(start_list.size());
                        foreach (int[] start in java.lang.JIter.of(start_list))
                        {
                            @out.writeShort(start[0]);
                            @out.writeShort(start[1]);
                            @out.writeShort(start[2]);
                            @out.writeShort(start[3]);
                        }
                    }
                    finally
                    {
                        if (@out != null) @out.close();
                    }
                }
                java.nio.file.Files.move(temp, target, java.nio.file.StandardCopyOption.REPLACE_EXISTING);
            }
            finally
            {
                java.nio.file.Files.deleteIfExists(temp);
            }
        }
        // Java: 131
        public static com.oddlabs.tt.mapeditor.MapFile load(java.nio.file.Path path)
        {
            {
                java.io.DataInputStream @in = com.oddlabs.tt.mapeditor.MapFile.open(path);
                try
                {
                    int version = com.oddlabs.tt.mapeditor.MapFile.readVersion(@in);
                    com.oddlabs.tt.mapeditor.MapSettings settings_v = com.oddlabs.tt.mapeditor.MapFile.readSettings(@in, version);
                    bool has_heights = @in.readBoolean();
                    bool has_resources = version >= 2 && @in.readBoolean();
                    bool has_preview = version >= 3 && @in.readBoolean();
                    com.oddlabs.tt.mapeditor.MapPreview preview_v = (has_preview ? com.oddlabs.tt.mapeditor.MapFile.readPreview(@in) : (com.oddlabs.tt.mapeditor.MapPreview)(null));
                    float[][] heights_v = null;
                    if (has_heights)
                    {
                        int grid_size = @in.readInt();
                        if (grid_size != com.oddlabs.tt.mapeditor.MapFile.gridSize(settings_v))
                        {
                            throw new System.IO.IOException("Height map does not fit the island size");
                        }
                        heights_v = java.lang.JA.newArray2<float>(grid_size, grid_size);
                        foreach (float[] row in heights_v)
                        {
                            for (int x = 0; x < row.Length; x++)
                            {
                                row[x] = @in.readFloat();
                            }
                        }
                    }
                    com.oddlabs.tt.mapeditor.MapFile.Resources resources_v = null;
                    if (has_resources)
                    {
                        int grid_size_2 = com.oddlabs.tt.mapeditor.MapFile.gridSize(settings_v);
                        java.util.List[] positions = new java.util.List[com.oddlabs.tt.mapeditor.Resource.values().Length];
                        for (int k = 0; k < positions.Length; k++)
                        {
                            int count = @in.readInt();
                            if (count < 0 || count > grid_size_2 * grid_size_2)
                            {
                                throw new System.IO.IOException("Bad resource count " + (count));
                            }
                            positions[k] = new java.util.ArrayList(count);
                            for (int i = 0; i < count; i++)
                            {
                                int x_2 = @in.readShort();
                                int y = @in.readShort();
                                if (x_2 < 0 || y < 0 || x_2 >= grid_size_2 || y >= grid_size_2)
                                {
                                    throw new System.IO.IOException("Resource outside the island");
                                }
                                positions[k].add(new int[] { x_2, y });
                            }
                        }
                        resources_v = new com.oddlabs.tt.mapeditor.MapFile.Resources(positions);
                    }
                    java.util.List starts_v = new java.util.ArrayList();
                    if (version >= 4)
                    {
                        int grid_size_3 = com.oddlabs.tt.mapeditor.MapFile.gridSize(settings_v);
                        int count_2 = @in.readInt();
                        if (count_2 < 0 || count_2 > 10000)
                        {
                            throw new System.IO.IOException("Bad start position count " + (count_2));
                        }
                        for (int i_2 = 0; i_2 < count_2; i_2++)
                        {
                            int player = @in.readShort();
                            int x_3 = @in.readShort();
                            int y_2 = @in.readShort();
                            int team = @in.readShort();
                            if (player < 0 || x_3 < 0 || y_2 < 0 || x_3 >= grid_size_3 || y_2 >= grid_size_3)
                            {
                                throw new System.IO.IOException("Start position outside the island");
                            }
                            starts_v.add(new int[] { player, x_3, y_2, team });
                        }
                    }
                    if (preview_v == null && heights_v != null)
                    {
                        preview_v = com.oddlabs.tt.mapeditor.MapPreview.render(heights_v, settings_v, resources_v);
                    }
                    return new com.oddlabs.tt.mapeditor.MapFile(com.oddlabs.tt.mapeditor.MapFile.nameOf(path), settings_v, heights_v, resources_v, preview_v, starts_v);
                }
                finally
                {
                    if (@in != null) @in.close();
                }
            }
        }
        // Java: 198
        public static com.oddlabs.tt.mapeditor.MapPreview loadPreview(java.nio.file.Path path)
        {
            bool has_heights = default;
            {
                java.io.DataInputStream @in = com.oddlabs.tt.mapeditor.MapFile.open(path);
                try
                {
                    int version = com.oddlabs.tt.mapeditor.MapFile.readVersion(@in);
                    com.oddlabs.tt.mapeditor.MapFile.readSettings(@in, version);
                    has_heights = @in.readBoolean();
                    if (version >= 2)
                    {
                        @in.readBoolean();
                    }
                    bool has_preview = version >= 3 && @in.readBoolean();
                    if (has_preview)
                    {
                        return com.oddlabs.tt.mapeditor.MapFile.readPreview(@in);
                    }
                }
                finally
                {
                    if (@in != null) @in.close();
                }
            }
            return (has_heights ? com.oddlabs.tt.mapeditor.MapFile.load(path).preview() : (com.oddlabs.tt.mapeditor.MapPreview)(null));
        }
        // Java: 214
        public static com.oddlabs.tt.mapeditor.MapPreview readPreview(java.io.DataInputStream @in)
        {
            int size = @in.readShort();
            if (size <= 0 || size > com.oddlabs.tt.mapeditor.MapPreview.MAX_SIZE)
            {
                throw new System.IO.IOException("Bad preview size " + (size));
            }
            sbyte[] rgb = new sbyte[size * size * 3];
            @in.readFully(rgb);
            return new com.oddlabs.tt.mapeditor.MapPreview(size, rgb);
        }
        // Java: 224
        public static java.util.List list(java.nio.file.Path dir)
        {
            java.util.List entries = new java.util.ArrayList();
            if (!java.nio.file.Files.isDirectory(dir))
            {
                return entries;
            }
            {
                java.util.stream.Stream files = java.nio.file.Files.list(dir);
                try
                {
                    foreach (java.nio.file.Path path in java.lang.JIter.of(((java.lang.Iterable)new java.lang.Iterable_L(() => { return files.iterator(); }))))
                    {
                        if (!java.lang.JO.toString(path.getFileName()).endsWith(com.oddlabs.tt.mapeditor.MapFile.EXTENSION) || !java.nio.file.Files.isRegularFile(path))
                        {
                            continue;
                        }
                        {
                            java.io.DataInputStream @in = com.oddlabs.tt.mapeditor.MapFile.open(path);
                            try
                            {
                                int version = com.oddlabs.tt.mapeditor.MapFile.readVersion(@in);
                                com.oddlabs.tt.mapeditor.MapSettings settings_v = com.oddlabs.tt.mapeditor.MapFile.readSettings(@in, version);
                                bool edited = @in.readBoolean();
                                if (version >= 2 && @in.readBoolean())
                                {
                                    edited = true;
                                }
                                entries.add(new com.oddlabs.tt.mapeditor.MapFile.Entry(com.oddlabs.tt.mapeditor.MapFile.nameOf(path), path, settings_v, edited, java.nio.file.Files.getLastModifiedTime(path)));
                            }
                            catch (System.IO.IOException e)
                            {
                                java.lang.IO.println("Skipping unreadable map " + java.lang.JStr.valueOf(path) + ": " + java.lang.JStr.valueOf(e));
                            }
                            finally
                            {
                                if (@in != null) @in.close();
                            }
                        }
                    }
                }
                catch (System.IO.IOException e_2)
                {
                    java.lang.IO.println("Could not list maps in " + java.lang.JStr.valueOf(dir) + ": " + java.lang.JStr.valueOf(e_2));
                }
                finally
                {
                    if (files != null) files.close();
                }
            }
            entries.sort(new java.util.Comparator_L((object a_l, object b_l) =>
            {
                com.oddlabs.tt.mapeditor.MapFile.Entry a = (com.oddlabs.tt.mapeditor.MapFile.Entry)a_l;
                com.oddlabs.tt.mapeditor.MapFile.Entry b = (com.oddlabs.tt.mapeditor.MapFile.Entry)b_l;
                return b.modified().compareTo(a.modified());
            }));
            return entries;
        }
        // Java: 251
        public static int readVersion(java.io.DataInputStream @in)
        {
            int version = @in.readInt();
            if (version < 1 || version > com.oddlabs.tt.mapeditor.MapFile.VERSION)
            {
                throw new System.IO.IOException("Unsupported map version " + (version));
            }
            return version;
        }
        // Java: 258
        public static java.io.DataInputStream open(java.nio.file.Path path)
        {
            java.io.DataInputStream @in = new java.io.DataInputStream(new java.io.BufferedInputStream(new java.util.zip.GZIPInputStream(java.nio.file.Files.newInputStream(path))));
            try
            {
                if (@in.readInt() != com.oddlabs.tt.mapeditor.MapFile.MAGIC)
                {
                    throw new System.IO.IOException("Not a map file");
                }
                return @in;
            }
            catch (System.IO.IOException e)
            {
                @in.close();
                throw e;
            }
        }
        // Java: 270
        public static void writeSettings(java.io.DataOutputStream @out, com.oddlabs.tt.mapeditor.MapSettings settings_v)
        {
            @out.writeInt(settings_v.size());
            @out.writeInt(settings_v.terrain());
            @out.writeInt(settings_v.hills());
            @out.writeInt(settings_v.trees());
            @out.writeInt(settings_v.supplies());
            @out.writeInt(settings_v.seed());
        }
        // Java: 280
        public static com.oddlabs.tt.mapeditor.MapSettings readSettings(java.io.DataInputStream @in, int version)
        {
            int size = @in.readInt();
            if (version <= 3 && size == 4)
            {
                size = com.oddlabs.matchmaking.Game.SIZE_ARCHIPELAGO;
            }
            return new com.oddlabs.tt.mapeditor.MapSettings(size, @in.readInt(), @in.readInt(), @in.readInt(), @in.readInt(), @in.readInt());
        }
        // Java: 288
        public static string nameOf(java.nio.file.Path path)
        {
            string file_name = java.lang.JO.toString(path.getFileName());
            return file_name.substring(0, file_name.length() - com.oddlabs.tt.mapeditor.MapFile.EXTENSION.length());
        }
    }
}
