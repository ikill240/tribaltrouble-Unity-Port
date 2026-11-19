// Translated from AbstractElementNode.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 14
    public abstract class AbstractElementNode : com.oddlabs.tt.util.BoundingBox
    {
        public com.oddlabs.util.LinkedList models;
        public int child_count;
        public com.oddlabs.tt.model.AbstractElementNode owner;
        // Java: 25
        public int getChildCount()
        {
            return child_count - models.size();
        }
        // Java: 29
        public com.oddlabs.tt.model.AbstractElementNode insertElement(com.oddlabs.tt.model.Element model)
        {
            checkBoundsZ(model.bmin_z);
            checkBoundsZ(model.bmax_z);
            return doInsertElement(model);
        }
        // Java: 35
        public abstract com.oddlabs.tt.model.AbstractElementNode doInsertElement(com.oddlabs.tt.model.Element model);
        // Java: 37
        public void removeElement(com.oddlabs.tt.model.Element model)
        {
            models.remove(model);
        }
        // Java: 41
        public void incElementCount()
        {
            child_count++;
        }
        // Java: 45
        public com.oddlabs.tt.model.AbstractElementNode reinsertElement(com.oddlabs.tt.model.Element model)
        {
            child_count--;
            if (contains(model) || owner == null)
            {
                return insertElement(model);
            }
            else
            {
                return owner.reinsertElement(model);
            }
        }
        // Java: 55
        public int getDepth()
        {
            if (owner != null)
            {
                return 1 + owner.getDepth();
            }
            else
            {
                return 0;
            }
        }
        // Java: 62
        public com.oddlabs.tt.model.AbstractElementNode addElement(com.oddlabs.tt.model.Element model)
        {
            models.addLast(model);
            return this;
        }
        // Java: 67
        public static com.oddlabs.tt.model.AbstractElementNode newRoot(com.oddlabs.tt.landscape.HeightMap heightmap)
        {
            com.oddlabs.tt.model.AbstractElementNode root = new com.oddlabs.tt.model.ElementNode(null, heightmap.getGridUnitsPerWorld(), 0, 0);
            return root;
        }
        // Java: 72
        public static void buildSupplies(com.oddlabs.tt.landscape.World world, java.util.List iron_positions, java.util.List rock_positions, float[][] plants, com.oddlabs.tt.procedural.Landscape.TerrainType terrain)
        {
            com.oddlabs.tt.model.AbstractElementNode.buildRockSupplies(world, rock_positions);
            com.oddlabs.tt.model.AbstractElementNode.buildIronSupplies(world, iron_positions);
            com.oddlabs.tt.model.AbstractElementNode.addPlants(world, plants, terrain);
        }
        // Java: 80
        public static void buildRockSupplies(com.oddlabs.tt.landscape.World world, java.util.List positions)
        {
            com.oddlabs.tt.render.SpriteKey[] sprite_renderers = world.getLandscapeResources().getRockFragments();
            int num_supplies = positions.size();
            java.lang.IO.println("num_rocks = " + (num_supplies));
            for (int i = 0; i < num_supplies; i++)
            {
                int[] coords = ((int[])positions.get(i));
                int grid_x = coords[0];
                int grid_y = coords[1];
                float x = (float)(com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(grid_x) + ((float)(world.getRandom().nextFloat() - 0.5f)));
                float y = (float)(com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(grid_y) + ((float)(world.getRandom().nextFloat() - 0.5f)));
                float rotation = (float)(world.getRandom().nextFloat() * 360.0f);
                new com.oddlabs.tt.model.RockSupply(world, sprite_renderers[i % sprite_renderers.Length], 2.0f, grid_x, grid_y, x, y, rotation, true);
            }
        }
        // Java: 96
        public static void buildIronSupplies(com.oddlabs.tt.landscape.World world, java.util.List positions)
        {
            com.oddlabs.tt.render.SpriteKey[] sprite_renderers = world.getLandscapeResources().getIronFragments();
            int num_supplies = positions.size();
            java.lang.IO.println("num_iron = " + (num_supplies));
            for (int i = 0; i < num_supplies; i++)
            {
                int[] coords = ((int[])positions.get(i));
                int grid_x = coords[0];
                int grid_y = coords[1];
                float x = (float)(com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(grid_x) + ((float)(world.getRandom().nextFloat() - 0.5f)));
                float y = (float)(com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(grid_y) + ((float)(world.getRandom().nextFloat() - 0.5f)));
                float rotation = (float)(world.getRandom().nextFloat() * 360.0f);
                new com.oddlabs.tt.model.IronSupply(world, sprite_renderers[i % sprite_renderers.Length], 2.0f, grid_x, grid_y, x, y, rotation, true);
            }
        }
        // Java: 112
        public static void addPlants(com.oddlabs.tt.landscape.World world, float[][] plants, com.oddlabs.tt.procedural.Landscape.TerrainType terrain)
        {
            int num_plants = 0;
            for (int t = 0; t < plants.Length; t++)
            {
                num_plants += plants[t].Length / 2;
                for (int p = 0; p < plants[t].Length >> 1; p++)
                {
                    float dir_x = world.getRandom().nextFloat();
                    float dir_y = world.getRandom().nextFloat();
                    float len_sqr = (float)((float)(dir_x * dir_x) + (float)(dir_y * dir_y));
                    if (len_sqr < 0.001d)
                    {
                        dir_x = 1.0f;
                        dir_y = 0.0f;
                    }
                    else
                    {
                        float inv_len = (float)(1.0f / ((float)(java.lang.Math.sqrt(len_sqr))));
                        dir_x = (float)(dir_x * inv_len);
                        dir_y = (float)(dir_y * inv_len);
                    }
                    new com.oddlabs.tt.model.Plants(world, plants[t][2 * p], plants[t][2 * p + 1], dir_x, dir_y, world.getLandscapeResources().getPlants()[terrain.@base().ordinal()][t]);
                }
            }
            java.lang.IO.println("num_plants = " + (num_plants));
        }
        // Java: 136
        public abstract void visit(com.oddlabs.tt.model.ElementNodeVisitor visitor);
        // Java: 138
        public void visitElements(com.oddlabs.tt.model.ElementNodeVisitor visitor)
        {
            com.oddlabs.tt.model.Element model = ((com.oddlabs.tt.model.Element)models.getFirst());
            while (model != null)
            {
                visitor.visit(model);
                model = ((com.oddlabs.tt.model.Element)model.getNext());
            }
        }
        private void init_AbstractElementNode()
        {
            this.models = new com.oddlabs.util.LinkedList();
            this.child_count = 0;
        }
        public AbstractElementNode(com.oddlabs.tt.model.AbstractElementNode owner) : base()
        {
            init_AbstractElementNode();
            this.owner = owner;
        }
    }
}
