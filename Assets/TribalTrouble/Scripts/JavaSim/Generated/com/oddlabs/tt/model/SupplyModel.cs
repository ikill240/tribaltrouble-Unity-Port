// Translated from SupplyModel.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 11
    public abstract class SupplyModel : com.oddlabs.tt.model.Model, com.oddlabs.tt.model.Supply, com.oddlabs.tt.util.Target, com.oddlabs.tt.model.ModelToolTip
    {
        public const float SPAWN_OFFSET_Z = (-2.0f);
        public com.oddlabs.tt.render.SpriteKey sprite_renderer;
        public float size;
        public float rotation;
        public float offset_z;
        public int grid_x;
        public int grid_y;
        public int num_supplies;
        public int hit_counter;
        // Java: 49
        public void visit(com.oddlabs.tt.model.ToolTipVisitor visitor)
        {
            visitor.visitSupply(this);
        }
        // Java: 54
        public float getRotation()
        {
            return rotation;
        }
        // Java: 58
        public virtual void animateSpawn(float t, float progress)
        {
            offset_z = (float)(com.oddlabs.tt.model.SupplyModel.SPAWN_OFFSET_Z * ((float)(1 - progress)));
            reinsert();
        }
        // Java: 64
        public virtual void spawnComplete()
        {
        }
        // Java: 68
        public bool isEmpty()
        {
            return num_supplies == 0;
        }
        // Java: 73
        public virtual bool hit()
        {
            hit_counter++;
            if (hit_counter == com.oddlabs.tt.model.Supply.HITS_PER_HARVEST)
            {
                hit_counter = 0;
                decreaseSupply();
                return true;
            }
            else
            {
                return false;
            }
        }
        // Java: 84
        public virtual bool take()
        {
            if (isEmpty())
            {
                return false;
            }
            decreaseSupply();
            return true;
        }
        // Java: 92
        public bool isDead()
        {
            return isEmpty();
        }
        // Java: 97
        public void decreaseSupply()
        {
            num_supplies--;
            if (isEmpty())
            {
                com.oddlabs.tt.pathfinder.UnitGrid unit_grid = getWorld().getUnitGrid();
                unit_grid.freeGrid(grid_x, grid_y, this);
                getWorld().getNotificationListener().unregisterTarget(this);
                com.oddlabs.tt.pathfinder.Region region = unit_grid.getRegion(grid_x, grid_y);
                region.unregisterObject(((System.Type)this.GetType()), this);
                remove();
                getWorld().getSupplyManager(this.GetType()).emptySupply(this);
            }
        }
        // Java: 111
        public override float getNoDetailSize()
        {
            throw new System.InvalidOperationException();
        }
        // Java: 116
        public float getSize()
        {
            return size;
        }
        // Java: 121
        public int getGridX()
        {
            return grid_x;
        }
        // Java: 126
        public int getGridY()
        {
            return grid_y;
        }
        // Java: 131
        public virtual void setGridPosition(int grid_x, int grid_y)
        {
            this.grid_x = grid_x;
            this.grid_y = grid_y;
        }
        // Java: 137
        public override float getOffsetZ()
        {
            return (float)(offset_z + calculateSlopeOffset());
        }
        // Java: 142
        public float calculateSlopeOffset()
        {
            float r = (float)(getSize() * 0.2f);
            float x = getPositionX();
            float y = getPositionY();
            com.oddlabs.tt.landscape.HeightMap hm = getWorld().getHeightMap();
            float h_center = hm.getNearestHeight(x, y);
            float h_max = h_center;
            h_max = java.lang.Math.max(h_max, hm.getNearestHeight((float)(x + r), y));
            h_max = java.lang.Math.max(h_max, hm.getNearestHeight((float)(x - r), y));
            h_max = java.lang.Math.max(h_max, hm.getNearestHeight(x, (float)(y + r)));
            h_max = java.lang.Math.max(h_max, hm.getNearestHeight(x, (float)(y - r)));
            float d = (float)(r * 0.707f);
            h_max = java.lang.Math.max(h_max, hm.getNearestHeight((float)(x + d), (float)(y + d)));
            h_max = java.lang.Math.max(h_max, hm.getNearestHeight((float)(x - d), (float)(y + d)));
            h_max = java.lang.Math.max(h_max, hm.getNearestHeight((float)(x + d), (float)(y - d)));
            h_max = java.lang.Math.max(h_max, hm.getNearestHeight((float)(x - d), (float)(y - d)));
            return java.lang.Math.max(0.0f, (float)(h_max - h_center));
        }
        // Java: 168
        public override com.oddlabs.tt.render.SpriteKey getSpriteRenderer()
        {
            return sprite_renderer;
        }
        // Java: 173
        public override void visit(com.oddlabs.tt.model.ElementVisitor visitor)
        {
            visitor.visitSupplyModel(this);
        }
        // Java: 178
        public virtual int getPenalty()
        {
            return com.oddlabs.tt.pathfinder.Occupant.STATIC;
        }
        // Java: 185
        public void removeFromWorld()
        {
            if (!isEmpty())
            {
                num_supplies = 0;
                leaveWorld();
            }
        }
        // Java: 192
        public void leaveWorld()
        {
            com.oddlabs.tt.pathfinder.UnitGrid unit_grid = getWorld().getUnitGrid();
            unit_grid.freeGrid(grid_x, grid_y, this);
            getWorld().getNotificationListener().unregisterTarget(this);
            com.oddlabs.tt.pathfinder.Region region = unit_grid.getRegion(grid_x, grid_y);
            region.unregisterObject(((System.Type)this.GetType()), this);
            remove();
        }
        private void init_SupplyModel()
        {
            this.offset_z = 0;
            this.hit_counter = 0;
        }
        public SupplyModel(com.oddlabs.tt.landscape.World world, com.oddlabs.tt.render.SpriteKey sprite_renderer, float size, int grid_x, int grid_y, float x, float y, float rotation, int num_supplies, bool increase_count) : base(world)
        {
            init_SupplyModel();
            this.sprite_renderer = sprite_renderer;
            this.size = size;
            this.grid_x = grid_x;
            this.grid_y = grid_y;
            this.rotation = rotation;
            this.num_supplies = num_supplies;
            setPosition(x, y);
            world.getNotificationListener().registerTarget(this);
            com.oddlabs.tt.pathfinder.UnitGrid unit_grid = world.getUnitGrid();
            unit_grid.occupyGrid(grid_x, grid_y, this);
            com.oddlabs.tt.pathfinder.Region region = unit_grid.getRegion(grid_x, grid_y);
            region.registerObject(((System.Type)this.GetType()), this);
            register();
            reinsert();
            if (increase_count)
            {
                world.getSupplyManager(this.GetType()).newSupply();
            }
        }
        // mixin of default method Supply.getName
        public virtual string getName()
        {
            return com.oddlabs.tt.util.Utils.getBundleString(java.util.ResourceBundle.getBundle(this.GetType().getName()), "name");
        }
        public abstract com.oddlabs.tt.model.Supply respawn();
        // mixin of default method Target.isAlive
        public virtual bool isAlive()
        {
            return !isDead();
        }
    }
}
