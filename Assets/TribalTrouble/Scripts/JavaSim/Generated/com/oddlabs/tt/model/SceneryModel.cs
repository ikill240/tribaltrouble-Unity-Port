// Translated from SceneryModel.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 12
    public class SceneryModel : com.oddlabs.tt.model.Model, com.oddlabs.tt.pathfinder.Occupant, com.oddlabs.tt.model.ModelToolTip, com.oddlabs.tt.animation.Animated
    {
        public com.oddlabs.tt.render.SpriteKey sprite_renderer;
        public float shadow_diameter;
        public bool occupy;
        public bool did_occupy;
        public string name;
        public int animation;
        public float seconds_per_animation_cycle;
        public float anim_time;
        // Java: 69
        public string getName()
        {
            return name;
        }
        // Java: 73
        public override float getShadowDiameter()
        {
            return shadow_diameter;
        }
        // Java: 78
        public virtual void doRegister()
        {
            register();
            reinsert();
            getWorld().getNotificationListener().registerTarget(this);
            if (animation > (-1))
            {
                getWorld().getAnimationManagerGameTime().registerAnimation(this);
            }
        }
        // Java: 86
        public override void remove()
        {
            if (did_occupy)
            {
                getWorld().getUnitGrid().freeGrid(getGridX(), getGridY(), this);
            }
            base.remove();
            getWorld().getNotificationListener().unregisterTarget(this);
            if (animation > (-1))
            {
                getWorld().getAnimationManagerGameTime().removeAnimation(this);
            }
        }
        // Java: 97
        public void visit(com.oddlabs.tt.model.ToolTipVisitor visitor)
        {
            visitor.visitSceneryModel(this);
        }
        // Java: 102
        public void animate(float t)
        {
            anim_time += (float)(t / 2.5f);
            if (seconds_per_animation_cycle > (-1) && anim_time > seconds_per_animation_cycle)
            {
                anim_time = 0;
            }
            reinsert();
        }
        // Java: 110
        public override int getAnimation()
        {
            return (animation > (-1) ? animation : 0);
        }
        // Java: 115
        public override float getAnimationTicks()
        {
            return (animation > (-1) ? anim_time : 0);
        }
        // Java: 120
        public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
        }
        // Java: 124
        public virtual int getPenalty()
        {
            return com.oddlabs.tt.pathfinder.Occupant.STATIC;
        }
        // Java: 129
        public int getGridX()
        {
            return com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(getPositionX());
        }
        // Java: 134
        public int getGridY()
        {
            return com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(getPositionY());
        }
        // Java: 139
        public float getSize()
        {
            throw new System.Exception();
        }
        // Java: 144
        public bool isDead()
        {
            return false;
        }
        // Java: 149
        public bool isOccupying()
        {
            return occupy;
        }
        // Java: 153
        public override com.oddlabs.tt.render.SpriteKey getSpriteRenderer()
        {
            return sprite_renderer;
        }
        // Java: 158
        public override void visit(com.oddlabs.tt.model.ElementVisitor visitor)
        {
            visitor.visitSceneryModel(this);
        }
        private void init_SceneryModel()
        {
            this.did_occupy = false;
            this.anim_time = 0;
        }
        public SceneryModel(com.oddlabs.tt.landscape.World world, float x, float y, float dir_x, float dir_y, com.oddlabs.tt.render.SpriteKey sprite_renderer) : this(world, x, y, dir_x, dir_y, sprite_renderer, 0.0f, false, null)
        {
        }
        public SceneryModel(com.oddlabs.tt.landscape.World world, float x, float y, float dir_x, float dir_y, com.oddlabs.tt.render.SpriteKey sprite_renderer, float shadow_diameter, bool occupy, string name) : this(world, x, y, dir_x, dir_y, sprite_renderer, shadow_diameter, occupy, name, (-1), (-1), 0)
        {
        }
        public SceneryModel(com.oddlabs.tt.landscape.World world, float x, float y, float dir_x, float dir_y, com.oddlabs.tt.render.SpriteKey sprite_renderer, float shadow_diameter, bool occupy, string name, int animation, float seconds_per_animation_cycle, float anim_offset) : base(world)
        {
            init_SceneryModel();
            this.sprite_renderer = sprite_renderer;
            this.shadow_diameter = shadow_diameter;
            this.occupy = occupy;
            this.name = name;
            this.animation = animation;
            this.seconds_per_animation_cycle = seconds_per_animation_cycle;
            anim_time = anim_offset;
            setPosition(x, y);
            setDirection(dir_x, dir_y);
            doRegister();
            if (occupy)
            {
                if (!world.getUnitGrid().isGridOccupied(getGridX(), getGridY(), com.oddlabs.tt.pathfinder.UnitGrid.LAND))
                {
                    world.getUnitGrid().occupyGrid(getGridX(), getGridY(), this);
                    did_occupy = true;
                }
            }
        }
        // mixin of default method Target.isAlive
        public virtual bool isAlive()
        {
            return !isDead();
        }
    }
}
