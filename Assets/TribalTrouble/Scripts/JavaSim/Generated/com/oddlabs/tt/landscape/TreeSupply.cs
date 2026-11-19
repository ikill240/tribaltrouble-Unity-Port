// Translated from TreeSupply.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.landscape
{
    // Java: 17
    public class TreeSupply : com.oddlabs.tt.landscape.AbstractTreeGroup, com.oddlabs.tt.model.Supply, com.oddlabs.tt.util.Target, com.oddlabs.tt.animation.Animated, com.oddlabs.tt.model.ModelToolTip
    {
        public const int INITIAL_SUPPLIES = 100;
        public const float SECOND_PER_TREEFALL = 3.0f;
        public org.joml.Matrix4f matrix;
        public com.oddlabs.tt.landscape.AbstractTreeGroup.TreeType tree_type;
        public float x;
        public float y;
        public int grid_x;
        public int grid_y;
        public int grid_size;
        public float size;
        public com.oddlabs.tt.landscape.World world;
        public int num_supplies;
        public float animation_time;
        public bool hide;
        public float scale;
        public int hit_counter;
        // Java: 64
        public com.oddlabs.tt.landscape.World getWorld()
        {
            return world;
        }
        // Java: 69
        public void visit(com.oddlabs.tt.model.ToolTipVisitor visitor)
        {
            visitor.visitSupply(this);
        }
        // Java: 74
        public float getScale()
        {
            return scale;
        }
        // Java: 78
        public float getTreeFallProgress()
        {
            return (float)(animation_time / com.oddlabs.tt.landscape.TreeSupply.SECOND_PER_TREEFALL);
        }
        // Java: 82
        public com.oddlabs.tt.model.Supply respawn()
        {
            occupyTree();
            hide = false;
            num_supplies = com.oddlabs.tt.landscape.TreeSupply.INITIAL_SUPPLIES;
            stateChanged();
            return this;
        }
        // Java: 91
        public void animateSpawn(float t, float progress)
        {
            float inv = (float)(1 - progress);
            scale = (float)(1 - (float)((float)((float)((float)((float)(inv * inv) * inv) * inv) * inv) * inv));
        }
        // Java: 97
        public void spawnComplete()
        {
            stateChanged();
        }
        // Java: 102
        public override string toString()
        {
            return "Tree at " + (grid_x) + " " + (grid_y) + " isEmpty() " + java.lang.JStr.valueOf(isEmpty());
        }
        // Java: 107
        public void occupyTree()
        {
            com.oddlabs.tt.pathfinder.UnitGrid grid = world.getUnitGrid();
            world.getNotificationListener().registerTarget(this);
            com.oddlabs.tt.pathfinder.Region region = grid.getRegion(getGridX(), getGridY());
            region.registerObject(((System.Type)this.GetType()), this);
            for (int y = 0; y < grid_size; y++)
            {
                int occ_y = grid_y + y - (grid_size - 1) / 2;
                for (int x = 0; x < grid_size; x++)
                {
                    int occ_x = grid_x + x - (grid_size - 1) / 2;
                    if (!grid.isGridOccupied(occ_x, occ_y))
                    {
                    }
                }
            }
            grid.occupyGrid(grid_x, grid_y, this);
        }
        // Java: 125
        public void unoccupyTree()
        {
            com.oddlabs.tt.pathfinder.UnitGrid grid = world.getUnitGrid();
            world.getNotificationListener().unregisterTarget(this);
            com.oddlabs.tt.pathfinder.Region region = grid.getRegion(grid_x, grid_y);
            region.unregisterObject(((System.Type)this.GetType()), this);
            grid.freeGrid(grid_x, grid_y, this);
        }
        // Java: 134
        public float getSize()
        {
            return size;
        }
        // Java: 139
        public float getPositionX()
        {
            return x;
        }
        // Java: 144
        public float getPositionY()
        {
            return y;
        }
        // Java: 149
        public int getGridX()
        {
            return grid_x;
        }
        // Java: 154
        public int getGridY()
        {
            return grid_y;
        }
        // Java: 159
        public int getPenalty()
        {
            return com.oddlabs.tt.pathfinder.Occupant.STATIC;
        }
        // Java: 164
        public bool isEmpty()
        {
            return num_supplies == 0;
        }
        // Java: 169
        public bool hit()
        {
            hit_counter++;
            if (hit_counter == com.oddlabs.tt.model.Supply.HITS_PER_HARVEST)
            {
                hit_counter = 0;
                decreaseSupply();
                return true;
            }
            return false;
        }
        // Java: 180
        public bool take()
        {
            if (isEmpty())
            {
                return false;
            }
            decreaseSupply();
            return true;
        }
        // Java: 188
        public bool isDead()
        {
            return isEmpty();
        }
        // Java: 193
        public void decreaseSupply()
        {
            num_supplies--;
            if (isEmpty())
            {
                unoccupyTree();
                world.getSupplyManager(this.GetType()).emptySupply(this);
                world.getAudio().newAudio(new com.oddlabs.tt.audio.AudioParameters(world.getRacesResources().getTreeFallSound()[tree_type.ordinal() % 2], getCX(), getCY(), getCZ(), com.oddlabs.tt.audio.AudioPlayer.AUDIO_RANK_TREE_FALL, com.oddlabs.tt.audio.AudioPlayer.AUDIO_DISTANCE_TREE_FALL, com.oddlabs.tt.audio.AudioPlayer.AUDIO_GAIN_TREE_FALL, com.oddlabs.tt.audio.AudioPlayer.AUDIO_RADIUS_TREE_FALL));
                world.getAnimationManagerRealTime().registerAnimation(this);
                animation_time = 0.0f;
                stateChanged();
            }
        }
        // Java: 209
        public void stateChanged()
        {
            if ((getParent() is com.oddlabs.tt.landscape.TreeLeaf leaf))
            {
                leaf.treeStateChanged();
            }
        }
        // Java: 215
        public void destroyByNature()
        {
            if (num_supplies > 0)
            {
                num_supplies = 1;
                decreaseSupply();
            }
        }
        // Java: 222
        public com.oddlabs.tt.landscape.AbstractTreeGroup.TreeType getTreeType()
        {
            return tree_type;
        }
        // Java: 226
        public override bool initBounds()
        {
            base.initBounds();
            return true;
        }
        // Java: 232
        public void animate(float t)
        {
            animation_time += t;
            if (animation_time >= com.oddlabs.tt.landscape.TreeSupply.SECOND_PER_TREEFALL)
            {
                world.getAnimationManagerRealTime().removeAnimation(this);
                hide = true;
                stateChanged();
            }
        }
        // Java: 242
        public org.joml.Matrix4f getMatrix()
        {
            return matrix;
        }
        // Java: 246
        public bool isHidden()
        {
            return hide;
        }
        // Java: 250
        public override void visit(com.oddlabs.tt.landscape.TreeNodeVisitor visitor)
        {
            visitor.visitTree(this);
        }
        // Java: 256
        public void leaveWorld()
        {
            if (!isEmpty())
            {
                unoccupyTree();
            }
        }
        private void init_TreeSupply()
        {
            this.num_supplies = com.oddlabs.tt.landscape.TreeSupply.INITIAL_SUPPLIES;
            this.hide = false;
            this.scale = 1.0f;
            this.hit_counter = 0;
        }
        public TreeSupply(com.oddlabs.tt.landscape.World world, com.oddlabs.tt.landscape.AbstractTreeGroup parent, float x, float y, int grid_x, int grid_y, int grid_size, float size, org.joml.Matrix4f matrix, com.oddlabs.tt.landscape.AbstractTreeGroup.TreeType tree_type, float[] vertices) : base(parent)
        {
            init_TreeSupply();
            this.world = world;
            this.x = x;
            this.y = y;
            this.grid_x = grid_x;
            this.grid_y = grid_y;
            this.grid_size = grid_size;
            this.size = size;
            this.tree_type = tree_type;
            this.matrix = matrix;
            org.joml.Vector4f src = new org.joml.Vector4f();
            org.joml.Vector4f dest = new org.joml.Vector4f();
            for (int i = 0; i < vertices.Length; i += 3)
            {
                src.set(vertices[i], vertices[i + 1], vertices[i + 2], 1.0f);
                matrix.transform(src, dest);
                checkBoundsX(dest.x_f);
                checkBoundsY(dest.y_f);
                checkBoundsZ(dest.z_f);
            }
            if (world.getUnitGrid().getOccupant(grid_x, grid_y) == null)
            {
                occupyTree();
            }
            world.getSupplyManager(this.GetType()).newSupply();
        }
        // mixin of default method Supply.getName
        public string getName()
        {
            return com.oddlabs.tt.util.Utils.getBundleString(java.util.ResourceBundle.getBundle(this.GetType().getName()), "name");
        }
        // mixin of default method Target.isAlive
        public bool isAlive()
        {
            return !isDead();
        }
        // mixin of default method Animated.updateChecksum
        public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
        }
    }
}
