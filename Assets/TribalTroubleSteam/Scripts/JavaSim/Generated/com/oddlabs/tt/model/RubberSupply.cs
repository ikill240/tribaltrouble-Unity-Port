// Translated from RubberSupply.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 20
    public class RubberSupply : com.oddlabs.tt.model.SupplyModel, com.oddlabs.tt.animation.Animated, com.oddlabs.tt.pathfinder.Movable
    {
        public const float MIN_TREE_FALL_HEIGHT = 4.0f;
        public const float MAX_TREE_FALL_HEIGHT = 8.0f;
        public const float METERS_PER_SECOND = 8.0f;
        public const int INITIAL_SUPPLIES = 1;
        public const int MAX_MOVE_GRIDS = 5;
        // Java: 28
        public sealed class Animation : java.lang.Enum
        {
            public static Animation[] values() { return (Animation[])VALUES_.Clone(); }
            public static Animation valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly Animation IDLING = new Animation("IDLING", 0, (float)(1.0f / ((float)(50.0f / 25.0f))));
            public static readonly Animation PECKING = new Animation("PECKING", 1, (float)(1.0f / ((float)(120.0f / 50.0f))));
            public static readonly Animation DYING = new Animation("DYING", 2, (float)(1.0f / ((float)(150.0f / 50.0f))));
            public static readonly Animation RUNNING = new Animation("RUNNING", 3, com.oddlabs.tt.model.RubberSupply.METERS_PER_SECOND);
            public static readonly Animation FLYING = new Animation("FLYING", 4, com.oddlabs.tt.model.RubberSupply.METERS_PER_SECOND);
            public float speed;
            // Java: 41
            public float getSpeed()
            {
                return speed;
            }
            public static readonly Animation[] VALUES_ = new Animation[] { IDLING, PECKING, DYING, RUNNING, FLYING };
            public Animation(string name_, int ordinal_, float speed) : base(name_, ordinal_)
            {
                this.speed = speed;
            }
        }
        public com.oddlabs.tt.pathfinder.PathTracker path_tracker;
        public int start_grid_x;
        public int start_grid_y;
        public float spawn_x;
        public float spawn_y;
        public float spawn_z;
        public com.oddlabs.tt.model.RubberGroup group;
        public float anim_time;
        public com.oddlabs.tt.model.RubberSupply.Animation animation;
        public bool is_hit;
        public bool spawning;
        public float offset_z;
        // Java: 79
        public override float getZError()
        {
            return getLandscapeError();
        }
        // Java: 84
        public override float getShadowDiameter()
        {
            return 1.2f;
        }
        // Java: 89
        public override void animateSpawn(float t, float progress)
        {
            anim_time += (float)(animation.getSpeed() * t);
            float x = (float)(spawn_x + (float)(((float)(com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(getGridX()) - spawn_x)) * progress));
            float y = (float)(spawn_y + (float)(((float)(com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(getGridY()) - spawn_y)) * progress));
            setPosition(x, y);
            offset_z = (float)(spawn_z - (float)((float)(spawn_z * progress) * progress));
            reinsert();
        }
        // Java: 99
        public override void spawnComplete()
        {
            offset_z = 0;
            spawning = false;
            setNewAnimation(com.oddlabs.tt.model.RubberSupply.Animation.IDLING);
        }
        // Java: 106
        public override com.oddlabs.tt.model.Supply respawn()
        {
            throw new System.Exception();
        }
        // Java: 111
        public com.oddlabs.tt.pathfinder.PathTracker getTracker()
        {
            return path_tracker;
        }
        // Java: 116
        public bool isMoving()
        {
            return false;
        }
        // Java: 121
        public void free()
        {
            getWorld().getUnitGrid().freeGrid(getGridX(), getGridY(), this);
        }
        // Java: 126
        public void occupy()
        {
            getWorld().getUnitGrid().occupyGrid(getGridX(), getGridY(), this);
        }
        // Java: 131
        public override void setGridPosition(int grid_x, int grid_y)
        {
            com.oddlabs.tt.pathfinder.Region current_region = getWorld().getUnitGrid().getRegion(getGridX(), getGridY());
            com.oddlabs.tt.pathfinder.Region new_region = getWorld().getUnitGrid().getRegion(grid_x, grid_y);
            if (((object)current_region != (object)new_region))
            {
                current_region.unregisterObject(((System.Type)this.GetType()), this);
                new_region.registerObject(((System.Type)this.GetType()), this);
            }
            base.setGridPosition(grid_x, grid_y);
        }
        // Java: 143
        public void markBlocking()
        {
        }
        // Java: 147
        public bool isHit()
        {
            return is_hit;
        }
        // Java: 151
        public void animate(float t)
        {
            if (spawning || path_tracker == null)
            {
                return;
            }
            anim_time += (float)(animation.getSpeed() * t);
            if (((object)animation == (object)com.oddlabs.tt.model.RubberSupply.Animation.FLYING) || ((object)animation == (object)com.oddlabs.tt.model.RubberSupply.Animation.RUNNING))
            {
                fly(t);
            }
            else if (!is_hit && anim_time >= 1.0f)
            {
                float random = getWorld().getRandom().nextFloat();
                if (random < 0.75d)
                {
                    setNewAnimation(com.oddlabs.tt.model.RubberSupply.Animation.IDLING);
                    if (random < 0.05d)
                    {
                        getWorld().getAudio().newAudio(new com.oddlabs.tt.audio.AudioParameters(getWorld().getLandscapeResources().getBirdIdleSound(getWorld().getRandom()), getPositionX(), getPositionY(), getPositionZ(), com.oddlabs.tt.audio.AudioPlayer.AUDIO_RANK_CHICKEN, com.oddlabs.tt.audio.AudioPlayer.AUDIO_DISTANCE_CHICKEN, com.oddlabs.tt.audio.AudioPlayer.AUDIO_GAIN_CHICKEN_IDLE, com.oddlabs.tt.audio.AudioPlayer.AUDIO_RADIUS_CHICKEN_IDLE));
                    }
                }
                else if (random < 0.85d)
                {
                    int new_grid_x = start_grid_x + java.lang.JC.f2i(((float)(((float)((float)(getWorld().getRandom().nextFloat() * 2) - 1)) * com.oddlabs.tt.model.RubberSupply.MAX_MOVE_GRIDS)));
                    int new_grid_y = start_grid_y + java.lang.JC.f2i(((float)(((float)((float)(getWorld().getRandom().nextFloat() * 2) - 1)) * com.oddlabs.tt.model.RubberSupply.MAX_MOVE_GRIDS)));
                    com.oddlabs.tt.util.Target target = getWorld().getUnitGrid().findGridTargets(new_grid_x, new_grid_y, 1, false)[0];
                    path_tracker.setTarget(new com.oddlabs.tt.pathfinder.TargetTrackerAlgorithm(getWorld().getUnitGrid(), 0.0f, target));
                    float move_random = getWorld().getRandom().nextFloat();
                    if (move_random < 0.25f)
                    {
                        setNewAnimation(com.oddlabs.tt.model.RubberSupply.Animation.FLYING);
                        getWorld().getAudio().newAudio(new com.oddlabs.tt.audio.AudioParameters(getWorld().getLandscapeResources().getBirdPeckSound(), getPositionX(), getPositionY(), getPositionZ(), com.oddlabs.tt.audio.AudioPlayer.AUDIO_RANK_CHICKEN, com.oddlabs.tt.audio.AudioPlayer.AUDIO_DISTANCE_CHICKEN, com.oddlabs.tt.audio.AudioPlayer.AUDIO_GAIN_CHICKEN_PECK, com.oddlabs.tt.audio.AudioPlayer.AUDIO_RADIUS_CHICKEN_PECK));
                    }
                    else
                    {
                        setNewAnimation(com.oddlabs.tt.model.RubberSupply.Animation.RUNNING);
                    }
                }
                else
                {
                    setNewAnimation(com.oddlabs.tt.model.RubberSupply.Animation.PECKING);
                    if (random > 0.98f)
                    {
                        getWorld().getAudio().newAudio(new com.oddlabs.tt.audio.AudioParameters(getWorld().getLandscapeResources().getBirdPeckSound(), getPositionX(), getPositionY(), getPositionZ(), com.oddlabs.tt.audio.AudioPlayer.AUDIO_RANK_CHICKEN, com.oddlabs.tt.audio.AudioPlayer.AUDIO_DISTANCE_CHICKEN, com.oddlabs.tt.audio.AudioPlayer.AUDIO_GAIN_CHICKEN_PECK, com.oddlabs.tt.audio.AudioPlayer.AUDIO_RADIUS_CHICKEN_PECK));
                    }
                }
            }
        }
        // Java: 204
        public void fly(float t)
        {
            com.oddlabs.tt.pathfinder.PathTracker.State state = path_tracker.animate((float)(com.oddlabs.tt.model.RubberSupply.METERS_PER_SECOND * t));
            switch ((state).ordinal())
            {
                case 0 /*OK*/:
                case 1 /*OK_INTERRUPTIBLE*/:
                {
                    return;
                }
                case 2 /*DONE*/:
                case 4 /*BLOCKED*/:
                case 3 /*SOFTBLOCKED*/:
                {
                    setNewAnimation(com.oddlabs.tt.model.RubberSupply.Animation.IDLING);
                    return;
                }
                default:
                {
                    throw new System.Exception("Invalid tracker state: " + java.lang.JStr.valueOf(state));
                }
            }
        }
        // Java: 220
        public override float getOffsetZ()
        {
            return offset_z;
        }
        // Java: 225
        public void setNewAnimation(com.oddlabs.tt.model.RubberSupply.Animation animation)
        {
            anim_time = 0;
            this.animation = animation;
        }
        // Java: 230
        public override int getAnimation()
        {
            return (null != animation ? animation.ordinal() : 0);
        }
        // Java: 236
        public override float getAnimationTicks()
        {
            return anim_time;
        }
        // Java: 241
        public override bool hit()
        {
            if (!is_hit)
            {
                is_hit = true;
                setNewAnimation(com.oddlabs.tt.model.RubberSupply.Animation.DYING);
                getWorld().getAudio().newAudio(new com.oddlabs.tt.audio.AudioParameters(getWorld().getLandscapeResources().getBirdDeathSound(), getPositionX(), getPositionY(), getPositionZ(), com.oddlabs.tt.audio.AudioPlayer.AUDIO_RANK_DEATH, com.oddlabs.tt.audio.AudioPlayer.AUDIO_DISTANCE_DEATH, com.oddlabs.tt.audio.AudioPlayer.AUDIO_GAIN_CHICKEN_DEATH, com.oddlabs.tt.audio.AudioPlayer.AUDIO_RADIUS_CHICKEN_DEATH));
                group.remove(this);
            }
            return base.hit();
        }
        // Java: 257
        public override void register()
        {
            base.register();
            getWorld().getAnimationManagerGameTime().registerAnimation(this);
        }
        // Java: 263
        public override void remove()
        {
            getWorld().getAnimationManagerGameTime().removeAnimation(this);
            base.remove();
        }
        // Java: 269
        public override void visit(com.oddlabs.tt.model.ElementVisitor visitor)
        {
            visitor.visitRubberSupply(this);
        }
        private void init_RubberSupply()
        {
            this.anim_time = 0;
            this.animation = com.oddlabs.tt.model.RubberSupply.Animation.IDLING;
            this.is_hit = false;
        }
        public RubberSupply(com.oddlabs.tt.landscape.World world, com.oddlabs.tt.render.SpriteKey sprite_renderer, float size, int grid_x, int grid_y, float x, float y, float rotation, com.oddlabs.tt.model.RubberGroup group, float spawn_x, float spawn_y) : base(world, sprite_renderer, size, grid_x, grid_y, x, y, rotation, com.oddlabs.tt.model.RubberSupply.INITIAL_SUPPLIES, false)
        {
            init_RubberSupply();
            this.path_tracker = new com.oddlabs.tt.pathfinder.PathTracker(world.getUnitGrid(), this);
            this.group = group;
            start_grid_x = grid_x;
            start_grid_y = grid_y;
            this.spawn_x = spawn_x;
            this.spawn_y = spawn_y;
            this.spawn_z = offset_z = (float)((float)(world.getRandom().nextFloat() * ((float)(com.oddlabs.tt.model.RubberSupply.MAX_TREE_FALL_HEIGHT - com.oddlabs.tt.model.RubberSupply.MIN_TREE_FALL_HEIGHT))) + com.oddlabs.tt.model.RubberSupply.MIN_TREE_FALL_HEIGHT);
            spawning = true;
            float dx = (float)(x - spawn_x);
            float dy = (float)(y - spawn_y);
            float inv_len = (float)(1.0f / ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy))))));
            setDirection((float)(dx * inv_len), (float)(dy * inv_len));
            setNewAnimation(com.oddlabs.tt.model.RubberSupply.Animation.FLYING);
        }
        // mixin of default method Animated.updateChecksum
        public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
        }
    }
}
