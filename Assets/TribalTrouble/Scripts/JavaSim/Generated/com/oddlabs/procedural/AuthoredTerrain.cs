// Translated from AuthoredTerrain.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.procedural
{
    // Java: 36
    public class AuthoredTerrain : java.lang.Object
    {
        public const long serialVersionUID = 1L;
        public const int MAGIC = 1414810960;
        public const int VERSION = 2;
        // Java: 43
        public sealed class ResourceType : java.lang.Enum
        {
            public static ResourceType[] values() { return (ResourceType[])VALUES_.Clone(); }
            public static ResourceType valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly ResourceType TREE = new ResourceType("TREE", 0);
            public static readonly ResourceType PALM_TREE = new ResourceType("PALM_TREE", 1);
            public static readonly ResourceType ROCK = new ResourceType("ROCK", 2);
            public static readonly ResourceType IRON = new ResourceType("IRON", 3);
            public static readonly ResourceType[] VALUES_ = new ResourceType[] { TREE, PALM_TREE, ROCK, IRON };
            public ResourceType(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        // Java: 50
        public class ResourceNode : java.lang.Object
        {
            public com.oddlabs.procedural.AuthoredTerrain.ResourceType type() { return type_f; }
            public int x() { return x_f; }
            public int y() { return y_f; }
            public ResourceNode(com.oddlabs.procedural.AuthoredTerrain.ResourceType type, int x, int y)
            {
                this.type_f = type;
                this.x_f = x;
                this.y_f = y;
            }
            public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is ResourceNode r)) return false; return true && java.lang.JO.equals(type_f, r.type_f) && java.lang.JO.equals(x_f, r.x_f) && java.lang.JO.equals(y_f, r.y_f); }
            public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(type_f); h = 31 * h + java.lang.JO.hashCode(x_f); h = 31 * h + java.lang.JO.hashCode(y_f); } return h; }
            public override string toString() { return "ResourceNode[" + "type=" + java.lang.JO.toString(type_f) + ", x=" + java.lang.JO.toString(x_f) + ", y=" + java.lang.JO.toString(y_f) + "]"; }
            public com.oddlabs.procedural.AuthoredTerrain.ResourceType type_f;
            public int x_f;
            public int y_f;
        }
        // Java: 53
        public class StartPosition : java.lang.Object
        {
            public int player_index() { return player_index_f; }
            public int x() { return x_f; }
            public int y() { return y_f; }
            public int team() { return team_f; }
            public StartPosition(int player_index, int x, int y, int team)
            {
                this.player_index_f = player_index;
                this.x_f = x;
                this.y_f = y;
                this.team_f = team;
            }
            public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is StartPosition r)) return false; return true && java.lang.JO.equals(player_index_f, r.player_index_f) && java.lang.JO.equals(x_f, r.x_f) && java.lang.JO.equals(y_f, r.y_f) && java.lang.JO.equals(team_f, r.team_f); }
            public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(player_index_f); h = 31 * h + java.lang.JO.hashCode(x_f); h = 31 * h + java.lang.JO.hashCode(y_f); h = 31 * h + java.lang.JO.hashCode(team_f); } return h; }
            public override string toString() { return "StartPosition[" + "player_index=" + java.lang.JO.toString(player_index_f) + ", x=" + java.lang.JO.toString(x_f) + ", y=" + java.lang.JO.toString(y_f) + ", team=" + java.lang.JO.toString(team_f) + "]"; }
            public int player_index_f;
            public int x_f;
            public int y_f;
            public int team_f;
        }
        public com.oddlabs.procedural.Channel height;
        public bool authored_resources;
        public java.util.List resources;
        public java.util.List starts;
        // Java: 75
        public bool hasHeights()
        {
            return height != null;
        }
        // Java: 79
        public bool hasAuthoredResources()
        {
            return authored_resources;
        }
        // Java: 83
        public com.oddlabs.procedural.Channel getHeightChannel()
        {
            if (height == null)
            {
                throw new System.InvalidOperationException("this map keeps the generated heights");
            }
            return height;
        }
        // Java: 89
        public void addResource(com.oddlabs.procedural.AuthoredTerrain.ResourceType type, int x, int y)
        {
            resources.add(new com.oddlabs.procedural.AuthoredTerrain.ResourceNode(type, x, y));
        }
        // Java: 98
        public bool removeNearestResource(int x, int y, double max_distance)
        {
            com.oddlabs.procedural.AuthoredTerrain.ResourceNode nearest = null;
            double nearest_dist = java.lang.Double.MAX_VALUE;
            foreach (com.oddlabs.procedural.AuthoredTerrain.ResourceNode node in java.lang.JIter.of(resources))
            {
                double dx = node.x() - x;
                double dy = node.y() - y;
                double dist = java.lang.Math.sqrt(dx * dx + dy * dy);
                if (dist < nearest_dist)
                {
                    nearest_dist = dist;
                    nearest = node;
                }
            }
            if (nearest != null && nearest_dist <= max_distance)
            {
                resources.remove(nearest);
                return true;
            }
            return false;
        }
        // Java: 120
        public void addStart(int player_index, int x, int y)
        {
            addStart(player_index, x, y, (-1));
        }
        // Java: 124
        public void addStart(int player_index, int x, int y, int team)
        {
            starts.removeIf(new java.util.function.Predicate_L((object s_l) =>
            {
                com.oddlabs.procedural.AuthoredTerrain.StartPosition s = (com.oddlabs.procedural.AuthoredTerrain.StartPosition)s_l;
                return s.player_index() == player_index;
            }));
            starts.add(new com.oddlabs.procedural.AuthoredTerrain.StartPosition(player_index, x, y, team));
        }
        // Java: 134
        public bool removeNearestStart(int x, int y, double max_distance)
        {
            com.oddlabs.procedural.AuthoredTerrain.StartPosition nearest = null;
            double nearest_dist = java.lang.Double.MAX_VALUE;
            foreach (com.oddlabs.procedural.AuthoredTerrain.StartPosition start in java.lang.JIter.of(starts))
            {
                double dx = start.x() - x;
                double dy = start.y() - y;
                double dist = java.lang.Math.sqrt(dx * dx + dy * dy);
                if (dist < nearest_dist)
                {
                    nearest_dist = dist;
                    nearest = start;
                }
            }
            if (nearest != null && nearest_dist <= max_distance)
            {
                starts.remove(nearest);
                return true;
            }
            return false;
        }
        // Java: 153
        public java.util.List getResources()
        {
            return resources;
        }
        // Java: 157
        public java.util.List getStarts()
        {
            return starts;
        }
        // Java: 162
        public void save(java.io.File file)
        {
            if (height == null)
            {
                throw new System.IO.IOException("this map keeps the generated heights; nothing to save as a .ttmap height field");
            }
            {
                java.io.DataOutputStream @out = new java.io.DataOutputStream(new java.io.BufferedOutputStream(new java.io.FileOutputStream(file)));
                try
                {
                    @out.writeInt(com.oddlabs.procedural.AuthoredTerrain.MAGIC);
                    @out.writeInt(com.oddlabs.procedural.AuthoredTerrain.VERSION);
                    @out.writeInt(height.getWidth());
                    @out.writeInt(height.getHeight());
                    float[][] pixels = height.getPixels();
                    for (int y = 0; y < height.getHeight(); y++)
                    {
                        for (int x = 0; x < height.getWidth(); x++)
                        {
                            @out.writeFloat(pixels[y][x]);
                        }
                    }
                    @out.writeInt(resources.size());
                    foreach (com.oddlabs.procedural.AuthoredTerrain.ResourceNode r in java.lang.JIter.of(resources))
                    {
                        @out.writeInt(r.type().ordinal());
                        @out.writeInt(r.x());
                        @out.writeInt(r.y());
                    }
                    @out.writeInt(starts.size());
                    foreach (com.oddlabs.procedural.AuthoredTerrain.StartPosition s in java.lang.JIter.of(starts))
                    {
                        @out.writeInt(s.player_index());
                        @out.writeInt(s.x());
                        @out.writeInt(s.y());
                        @out.writeInt(s.team());
                    }
                }
                finally
                {
                    if (@out != null) @out.close();
                }
            }
        }
        // Java: 195
        public static com.oddlabs.procedural.AuthoredTerrain load(java.io.File file)
        {
            {
                java.io.DataInputStream @in = new java.io.DataInputStream(new java.io.BufferedInputStream(new java.io.FileInputStream(file)));
                try
                {
                    if (@in.readInt() != com.oddlabs.procedural.AuthoredTerrain.MAGIC)
                    {
                        throw new System.IO.IOException("Not a Tribal Trouble map file: " + java.lang.JStr.valueOf(file));
                    }
                    int version = @in.readInt();
                    if (version != 1 && version != com.oddlabs.procedural.AuthoredTerrain.VERSION)
                    {
                        throw new System.IO.IOException("Unsupported map file version: " + (version));
                    }
                    int width = @in.readInt();
                    int height_size = @in.readInt();
                    com.oddlabs.procedural.Channel height = new com.oddlabs.procedural.Channel(width, height_size);
                    float[][] pixels = height.getPixels();
                    for (int y = 0; y < height_size; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            pixels[y][x] = @in.readFloat();
                        }
                    }
                    com.oddlabs.procedural.AuthoredTerrain terrain = new com.oddlabs.procedural.AuthoredTerrain(height);
                    int num_resources = @in.readInt();
                    com.oddlabs.procedural.AuthoredTerrain.ResourceType[] types = com.oddlabs.procedural.AuthoredTerrain.ResourceType.values();
                    for (int i = 0; i < num_resources; i++)
                    {
                        int type_ordinal = @in.readInt();
                        int x_2 = @in.readInt();
                        int y_2 = @in.readInt();
                        terrain.addResource(types[type_ordinal], x_2, y_2);
                    }
                    int num_starts = @in.readInt();
                    for (int i_2 = 0; i_2 < num_starts; i_2++)
                    {
                        int player_index = @in.readInt();
                        int x_3 = @in.readInt();
                        int y_3 = @in.readInt();
                        if (version >= 2)
                        {
                            int team = @in.readInt();
                            terrain.addStart(player_index, x_3, y_3, team);
                        }
                        else
                        {
                            terrain.addStart(player_index, x_3, y_3);
                        }
                    }
                    return terrain;
                }
                finally
                {
                    if (@in != null) @in.close();
                }
            }
        }
        private void init_AuthoredTerrain()
        {
            this.resources = new java.util.ArrayList();
            this.starts = new java.util.ArrayList();
        }
        public AuthoredTerrain(com.oddlabs.procedural.Channel height) : this(height, true)
        {
        }
        public AuthoredTerrain(com.oddlabs.procedural.Channel height, bool authored_resources) : base()
        {
            init_AuthoredTerrain();
            this.height = height;
            this.authored_resources = authored_resources;
        }
    }
}
