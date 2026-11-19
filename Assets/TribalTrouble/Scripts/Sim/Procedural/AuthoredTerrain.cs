// Ported from com.oddlabs.procedural.AuthoredTerrain (Tribal Trouble 1.4.1, ikill240c).
// A hand-authored map: a painted height Channel plus placed resource nodes and player start
// positions. Binary format: big-endian ints/floats (java.io.DataInputStream), magic "TTMP".
using System;
using System.Collections.Generic;
using System.IO;
using TribalTrouble.Sim.Determinism;

namespace TribalTrouble.Sim.Procedural
{
    public sealed class AuthoredTerrain
    {
        const int MAGIC = 0x54544D50; // "TTMP"
        const int VERSION = 2;

        public enum ResourceType { TREE, PALM_TREE, ROCK, IRON }

        /// <summary>Java record ResourceNode(type, x, y).</summary>
        public sealed class ResourceNode
        {
            readonly ResourceType _type;
            readonly int _x, _y;
            public ResourceNode(ResourceType type, int x, int y) { _type = type; _x = x; _y = y; }
            public ResourceType type() => _type;
            public int x() => _x;
            public int y() => _y;
        }

        /// <summary>Java record StartPosition(player_index, x, y, team).</summary>
        public sealed class StartPosition
        {
            readonly int _player_index, _x, _y, _team;
            public StartPosition(int player_index, int x, int y, int team) { _player_index = player_index; _x = x; _y = y; _team = team; }
            public int player_index() => _player_index;
            public int x() => _x;
            public int y() => _y;
            public int team() => _team;
        }

        // Null = keep the generated heights (a 3D map editor map whose terrain was never edited).
        readonly Channel height;
        // False = generate resources as usual (an editor map whose resources were never edited).
        readonly bool authored_resources;
        readonly List<ResourceNode> resources = new List<ResourceNode>();
        readonly List<StartPosition> starts = new List<StartPosition>();

        public AuthoredTerrain(Channel height) : this(height, true) { }

        public AuthoredTerrain(Channel height, bool authored_resources)
        {
            this.height = height;
            this.authored_resources = authored_resources;
        }

        public bool hasHeights() => height != null;
        public bool hasAuthoredResources() => authored_resources;

        public Channel getHeightChannel()
        {
            if (height == null)
                throw new InvalidOperationException("this map keeps the generated heights");
            return height;
        }

        public void addResource(ResourceType type, int x, int y) => resources.Add(new ResourceNode(type, x, y));

        public void addStart(int player_index, int x, int y) => addStart(player_index, x, y, -1);

        public void addStart(int player_index, int x, int y, int team)
        {
            starts.RemoveAll(s => s.player_index() == player_index); // one start per player
            starts.Add(new StartPosition(player_index, x, y, team));
        }

        public List<ResourceNode> getResources() => resources;
        public List<StartPosition> getStarts() => starts;

        public static AuthoredTerrain load(string file)
        {
            using (var stream = new BufferedStream(File.OpenRead(file)))
            {
                var @in = new JDataInput(stream);
                if (@in.readInt() != MAGIC)
                    throw new IOException("Not a Tribal Trouble map file: " + file);
                int version = @in.readInt();
                if (version != 1 && version != VERSION)
                    throw new IOException("Unsupported map file version: " + version);
                int width = @in.readInt();
                int height_size = @in.readInt();
                Channel height = new Channel(width, height_size);
                float[][] pixels = height.getPixels();
                for (int y = 0; y < height_size; y++)
                    for (int x = 0; x < width; x++)
                        pixels[y][x] = @in.readFloat();
                AuthoredTerrain terrain = new AuthoredTerrain(height);
                int num_resources = @in.readInt();
                for (int i = 0; i < num_resources; i++)
                {
                    int type_ordinal = @in.readInt();
                    int x = @in.readInt();
                    int y = @in.readInt();
                    terrain.addResource((ResourceType)type_ordinal, x, y);
                }
                int num_starts = @in.readInt();
                for (int i = 0; i < num_starts; i++)
                {
                    int player_index = @in.readInt();
                    int x = @in.readInt();
                    int y = @in.readInt();
                    if (version >= 2)
                        terrain.addStart(player_index, x, y, @in.readInt());
                    else
                        terrain.addStart(player_index, x, y);
                }
                return terrain;
            }
        }
    }
}
