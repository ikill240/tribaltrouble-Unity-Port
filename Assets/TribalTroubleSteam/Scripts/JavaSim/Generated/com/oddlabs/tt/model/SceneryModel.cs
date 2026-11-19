// Translated from SceneryModel.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 14
    public class SceneryModel : com.oddlabs.tt.model.Model, com.oddlabs.tt.pathfinder.Occupant, com.oddlabs.tt.model.ModelToolTip, com.oddlabs.tt.animation.Animated
    {
        public com.oddlabs.tt.render.SpriteKey sprite_renderer;
        public float shadow_diameter;
        public float shadow_fade;
        public bool occupy;
        public string name;
        public int animation;
        public java.util.List attachments;
        public float seconds_per_animation_cycle;
        public float anim_time;
        // Java: 83
        public string getName()
        {
            return name;
        }
        // Java: 87
        public override float getShadowDiameter()
        {
            return shadow_diameter;
        }
        // Java: 92
        public override float getShadowFade()
        {
            return shadow_fade;
        }
        // Java: 97
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
        // Java: 105
        public override void remove()
        {
            if (occupy)
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
        // Java: 116
        public void visit(com.oddlabs.tt.model.ToolTipVisitor visitor)
        {
            visitor.visitSceneryModel(this);
        }
        // Java: 121
        public void animate(float t)
        {
            anim_time += (float)(t / 2.5f);
            if (seconds_per_animation_cycle > (-1) && anim_time > seconds_per_animation_cycle)
            {
                anim_time = 0;
            }
            reinsert();
        }
        // Java: 129
        public override int getAnimation()
        {
            return (animation > (-1) ? animation : 0);
        }
        // Java: 134
        public override float getAnimationTicks()
        {
            return (animation > (-1) ? anim_time : 0);
        }
        // Java: 139
        public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
        }
        // Java: 143
        public virtual int getPenalty()
        {
            return com.oddlabs.tt.pathfinder.Occupant.STATIC;
        }
        // Java: 148
        public int getGridX()
        {
            return com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(getPositionX());
        }
        // Java: 153
        public int getGridY()
        {
            return com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(getPositionY());
        }
        // Java: 158
        public float getSize()
        {
            throw new System.Exception();
        }
        // Java: 163
        public bool isDead()
        {
            return false;
        }
        // Java: 168
        public bool isOccupying()
        {
            return occupy;
        }
        // Java: 173
        public java.util.List getAttachments()
        {
            return attachments;
        }
        // Java: 177
        public override com.oddlabs.tt.render.SpriteKey getSpriteRenderer()
        {
            return sprite_renderer;
        }
        // Java: 182
        public override void visit(com.oddlabs.tt.model.ElementVisitor visitor)
        {
            visitor.visitSceneryModel(this);
        }
        private void init_SceneryModel()
        {
            this.anim_time = 0;
        }
        public SceneryModel(com.oddlabs.tt.landscape.World world, float x, float y, float dir_x, float dir_y, com.oddlabs.tt.render.SpriteKey sprite_renderer) : this(world, x, y, dir_x, dir_y, sprite_renderer, 0.0f, false, null)
        {
        }
        public SceneryModel(com.oddlabs.tt.landscape.World world, float x, float y, float dir_x, float dir_y, com.oddlabs.tt.render.SpriteKey sprite_renderer, float shadow_diameter, float shadow_fade) : this(world, x, y, dir_x, dir_y, sprite_renderer, shadow_diameter, shadow_fade, false, null, (-1), (-1), 0)
        {
        }
        public SceneryModel(com.oddlabs.tt.landscape.World world, float x, float y, float dir_x, float dir_y, com.oddlabs.tt.render.SpriteKey sprite_renderer, float shadow_diameter, bool occupy, string name) : this(world, x, y, dir_x, dir_y, sprite_renderer, shadow_diameter, 0, occupy, name, (-1), (-1), 0)
        {
        }
        public SceneryModel(com.oddlabs.tt.landscape.World world, float x, float y, float dir_x, float dir_y, com.oddlabs.tt.render.SpriteKey sprite_renderer, float shadow_diameter, bool occupy, string name, int animation, float seconds_per_animation_cycle, float anim_offset) : this(world, x, y, dir_x, dir_y, sprite_renderer, shadow_diameter, 0, occupy, name, animation, seconds_per_animation_cycle, anim_offset)
        {
        }
        public SceneryModel(com.oddlabs.tt.landscape.World world, float x, float y, float dir_x, float dir_y, com.oddlabs.tt.render.SpriteKey sprite_renderer, float shadow_diameter, float shadow_fade, bool occupy, string name, int animation, float seconds_per_animation_cycle, float anim_offset) : this(world, x, y, dir_x, dir_y, sprite_renderer, shadow_diameter, shadow_fade, occupy, name, animation, seconds_per_animation_cycle, anim_offset, java.util.List_.of())
        {
        }
        public SceneryModel(com.oddlabs.tt.landscape.World world, float x, float y, float dir_x, float dir_y, com.oddlabs.tt.render.SpriteKey sprite_renderer, float shadow_diameter, bool occupy, string name, int animation, float seconds_per_animation_cycle, float anim_offset, java.util.List attachments) : this(world, x, y, dir_x, dir_y, sprite_renderer, shadow_diameter, 0, occupy, name, animation, seconds_per_animation_cycle, anim_offset, attachments)
        {
        }
        public SceneryModel(com.oddlabs.tt.landscape.World world, float x, float y, float dir_x, float dir_y, com.oddlabs.tt.render.SpriteKey sprite_renderer, float shadow_diameter, float shadow_fade, bool occupy, string name, int animation, float seconds_per_animation_cycle, float anim_offset, java.util.List attachments) : base(world)
        {
            init_SceneryModel();
            this.sprite_renderer = sprite_renderer;
            this.shadow_diameter = shadow_diameter;
            this.shadow_fade = shadow_fade;
            this.occupy = occupy;
            this.name = name;
            this.animation = animation;
            this.attachments = attachments;
            this.seconds_per_animation_cycle = seconds_per_animation_cycle;
            anim_time = anim_offset;
            setPosition(x, y);
            setDirection(dir_x, dir_y);
            doRegister();
            if (occupy)
            {
                world.getUnitGrid().occupyGrid(getGridX(), getGridY(), this);
            }
        }
        // mixin of default method Target.isAlive
        public virtual bool isAlive()
        {
            return !isDead();
        }
    }
}
