// Translated from Selectable.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 19
    public abstract class Selectable : com.oddlabs.tt.model.Model, com.oddlabs.tt.util.Target, com.oddlabs.tt.animation.Animated, com.oddlabs.tt.model.ModelToolTip
    {
        public com.oddlabs.tt.player.Player owner;
        public com.oddlabs.tt.model.behaviour.Behaviour current_behaviour;
        public com.oddlabs.tt.model.Abilities abilities;
        public com.oddlabs.tt.model.Template template;
        public java.util.List controller_stack;
        public bool dead;
        public bool should_decide;
        public com.oddlabs.tt.model.behaviour.Behaviour.State last;
        public int grid_x;
        public int grid_y;
        public int layer;
        // Java: 41
        public virtual void setLayer(int layer)
        {
            this.layer = layer;
        }
        // Java: 45
        public virtual int getLayer()
        {
            return layer;
        }
        // Java: 49
        public override float getShadowDiameter()
        {
            return template.getShadowDiameter();
        }
        // Java: 54
        public com.oddlabs.tt.model.Template getTemplate()
        {
            return template;
        }
        // Java: 58
        public virtual float getDefenseChance()
        {
            return template.getDefenseChance();
        }
        // Java: 62
        public override float getNoDetailSize()
        {
            return template.getNoDetailSize();
        }
        // Java: 67
        public abstract bool isEnabled();
        // Java: 69
        public virtual float getHitOffsetZ()
        {
            return template.getHitOffsetZ(0);
        }
        // Java: 73
        public com.oddlabs.tt.model.behaviour.Controller getCurrentController()
        {
            return ((com.oddlabs.tt.model.behaviour.Controller)controller_stack.getLast());
        }
        // Java: 77
        public void animate(float t)
        {
            last = current_behaviour.animate(t);
            switch ((last).ordinal())
            {
                case 0 /*UNINTERRUPTIBLE*/:
                {
                    break;
                }
                case 1 /*INTERRUPTIBLE*/:
                {
                    if (should_decide)
                    {
                        decide();
                    }
                    break;
                }
                case 2 /*DONE*/:
                {
                    decide();
                    break;
                }
            }
            doAnimate(t);
            owner.getWorld().updateGlobalChecksum(grid_x + grid_y);
        }
        // Java: 93
        public virtual void doAnimate(float t)
        {
        }
        // Java: 96
        public bool isBlocking()
        {
            return current_behaviour.isBlocking();
        }
        // Java: 100
        public com.oddlabs.tt.model.behaviour.Behaviour getCurrentBehaviour()
        {
            return current_behaviour;
        }
        // Java: 104
        public com.oddlabs.tt.pathfinder.UnitGrid getUnitGrid()
        {
            return owner.getWorld().getUnitGrid();
        }
        // Java: 108
        public void scanVicinity(com.oddlabs.tt.pathfinder.ScanFilter filter)
        {
            getUnitGrid().scan(filter, getGridX(), getGridY(), com.oddlabs.tt.pathfinder.UnitGrid.LAND);
            getUnitGrid().scan(filter, getGridX(), getGridY(), com.oddlabs.tt.pathfinder.UnitGrid.SEA);
        }
        // Java: 114
        public static bool isAdjacent(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, int grid_x, int grid_y, com.oddlabs.tt.pathfinder.Occupant occ)
        {
            return com.oddlabs.tt.model.Selectable.isAdjacent(unit_grid, grid_x, grid_y, occ, com.oddlabs.tt.pathfinder.UnitGrid.LAND);
        }
        // Java: 118
        public static bool isAdjacent(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, int grid_x, int grid_y, com.oddlabs.tt.pathfinder.Occupant occ, int layer)
        {
            for (int x = grid_x - 1; x <= grid_x + 1; x++)
            {
                for (int y = grid_y - 1; y <= grid_y + 1; y++)
                {
                    if (((object)unit_grid.getOccupant(x, y, layer) == (object)occ))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        // Java: 130
        public bool isCloseEnough(float max_dist, com.oddlabs.tt.util.Target target)
        {
            return com.oddlabs.tt.model.Selectable.isCloseEnough(getUnitGrid(), max_dist, getGridX(), getGridY(), target, com.oddlabs.tt.pathfinder.UnitGrid.LAND);
        }
        // Java: 135
        public bool isCloseEnough(float max_dist, com.oddlabs.tt.util.Target target, int layer)
        {
            return com.oddlabs.tt.model.Selectable.isCloseEnough(getUnitGrid(), max_dist, getGridX(), getGridY(), target, layer);
        }
        // Java: 140
        public static bool isCloseEnough(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, float max_dist, int grid_x, int grid_y, com.oddlabs.tt.util.Target target)
        {
            return com.oddlabs.tt.model.Selectable.isCloseEnough(unit_grid, max_dist, grid_x, grid_y, target, com.oddlabs.tt.pathfinder.UnitGrid.LAND);
        }
        // Java: 146
        public static bool isCloseEnough(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, float max_dist, int grid_x, int grid_y, com.oddlabs.tt.util.Target target, int layer)
        {
            if (max_dist == 0.0f && (target is com.oddlabs.tt.pathfinder.Occupant occupant))
            {
                return com.oddlabs.tt.model.Selectable.isAdjacent(unit_grid, grid_x, grid_y, occupant, layer);
            }
            else
            {
                int dx = grid_x - target.getGridX();
                int dy = grid_y - target.getGridY();
                int dist_squared = dx * dx + dy * dy;
                float max_dist_squared = (float)(max_dist * max_dist);
                return dist_squared <= max_dist_squared;
            }
        }
        // Java: 159
        public override void register()
        {
            base.register();
            owner.getWorld().getAnimationManagerGameTime().registerAnimation(this);
            enable();
        }
        // Java: 166
        public void decide()
        {
            if (((object)last != (object)com.oddlabs.tt.model.behaviour.Behaviour.State.UNINTERRUPTIBLE))
            {
                doDecide();
            }
            else
            {
                should_decide = true;
            }
        }
        // Java: 174
        public void doDecide()
        {
            should_decide = false;
            current_behaviour = null;
            getCurrentController().decide();
        }
        // Java: 180
        public void forceDecide()
        {
            current_behaviour.forceInterrupted();
            last = com.oddlabs.tt.model.behaviour.Behaviour.State.INTERRUPTIBLE;
            doDecide();
        }
        // Java: 186
        public void pushController(com.oddlabs.tt.model.behaviour.Controller controller)
        {
            controller_stack.add(controller);
            decide();
        }
        // Java: 192
        public void pushControllers(params com.oddlabs.tt.model.behaviour.Controller[] controllers)
        {
            controller_stack.addAll(java.util.Arrays.asList(controllers));
            decide();
        }
        // Java: 198
        public void swapController(com.oddlabs.tt.model.behaviour.Controller controller)
        {
            controller_stack.removeLast();
            pushController(controller);
        }
        // Java: 204
        public void popController()
        {
            controller_stack.removeLast();
            decide();
        }
        // Java: 210
        public void setBehaviour(com.oddlabs.tt.model.behaviour.Behaviour behaviour)
        {
            current_behaviour = behaviour;
        }
        // Java: 216
        public com.oddlabs.tt.model.behaviour.Controller getPrimaryController()
        {
            return (controller_stack.size() > 1 ? ((com.oddlabs.tt.model.behaviour.Controller)controller_stack.get(1)) : ((com.oddlabs.tt.model.behaviour.Controller)controller_stack.getFirst()));
        }
        // Java: 221
        public void clearControllerStack()
        {
            com.oddlabs.tt.model.behaviour.Controller default_controller = ((com.oddlabs.tt.model.behaviour.Controller)controller_stack.getFirst());
            controller_stack.clear();
            controller_stack.add(default_controller);
        }
        // Java: 227
        public void initTarget(com.oddlabs.tt.util.Target target, com.oddlabs.tt.model.Action action, bool aggressive)
        {
            if (target == null)
            {
                return;
            }
            clearControllerStack();
            setTarget(target, action, aggressive);
        }
        // Java: 235
        public abstract void setTarget(com.oddlabs.tt.util.Target target, com.oddlabs.tt.model.Action action, bool aggressive);
        // Java: 237
        public abstract com.oddlabs.tt.model.AttackScanFilter.Priority getAttackPriority();
        // Java: 239
        public abstract int getStatusValue();
        // Java: 241
        public com.oddlabs.tt.model.Abilities getAbilities()
        {
            return abilities;
        }
        // Java: 245
        public int getGridX()
        {
            return grid_x;
        }
        // Java: 250
        public int getGridY()
        {
            return grid_y;
        }
        // Java: 255
        public int getIslandId()
        {
            return getUnitGrid().getIslandId(grid_x, grid_y);
        }
        // Java: 259
        public void setGridPosition(int grid_x, int grid_y)
        {
            this.grid_x = grid_x;
            this.grid_y = grid_y;
        }
        // Java: 267
        public com.oddlabs.tt.player.Player getOwnerNoCheck()
        {
            return owner;
        }
        // Java: 271
        public com.oddlabs.tt.player.Player getOwner()
        {
            return getOwnerNoCheck();
        }
        // Java: 275
        public override void remove()
        {
            base.remove();
            owner.getWorld().getAnimationManagerGameTime().removeAnimation(this);
        }
        // Java: 281
        public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
        }
        // Java: 289
        public void disable()
        {
            owner.getWorld().getNotificationListener().unregisterTarget(this);
            owner.getUnits().remove(this);
        }
        // Java: 294
        public void enable()
        {
            owner.getWorld().getNotificationListener().registerTarget(this);
            owner.getUnits().add(this);
        }
        // Java: 300
        public virtual void removeDying()
        {
            disable();
            dead = true;
        }
        // Java: 305
        public bool isDead()
        {
            return dead;
        }
        // Java: 310
        public virtual void hit(int damage, float direction_x, float direction_y, com.oddlabs.tt.player.Player attacker)
        {
            if (owner.isEnemy(attacker))
            {
                owner.getWorld().getNotificationListener().newAttackNotification(this);
            }
        }
        // Java: 316
        public static System.Type genericClass()
        {
            return ((System.Type)((System.Type)typeof(com.oddlabs.tt.model.Selectable)));
        }
        // Java: 321
        public static com.oddlabs.tt.model.Selectable[] newArray(int length)
        {
            return new com.oddlabs.tt.model.Selectable[length];
        }
        // Java: 326
        public static com.oddlabs.tt.model.Selectable[] newArray(params com.oddlabs.tt.model.Selectable[] selectables)
        {
            return selectables;
        }
        private void init_Selectable()
        {
            this.abilities = new com.oddlabs.tt.model.Abilities(com.oddlabs.tt.model.Abilities.NONE);
            this.controller_stack = new java.util.ArrayList();
            this.last = com.oddlabs.tt.model.behaviour.Behaviour.State.DONE;
        }
        public Selectable(com.oddlabs.tt.player.Player owner, com.oddlabs.tt.model.Template template) : base(owner.getWorld())
        {
            init_Selectable();
            this.owner = owner;
            this.template = template;
            this.layer = com.oddlabs.tt.pathfinder.UnitGrid.LAND;
        }
        public abstract float getSize();
        // mixin of default method Target.isAlive
        public virtual bool isAlive()
        {
            return !isDead();
        }
        public abstract void visit(com.oddlabs.tt.model.ToolTipVisitor p0);
    }
}
