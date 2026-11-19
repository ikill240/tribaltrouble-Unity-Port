// Translated from Selectable.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 19
    public abstract class Selectable : com.oddlabs.tt.model.Model, com.oddlabs.tt.util.Target, com.oddlabs.tt.animation.Animated, com.oddlabs.tt.model.ModelToolTip
    {
        public const int MAX_QUEUED_ORDERS = 20;
        // Java: 24
        public class QueuedOrder : java.lang.Object
        {
            public com.oddlabs.tt.util.Target target() { return target_f; }
            public com.oddlabs.tt.model.Action action() { return action_f; }
            public bool aggressive() { return aggressive_f; }
            public QueuedOrder(com.oddlabs.tt.util.Target target, com.oddlabs.tt.model.Action action, bool aggressive)
            {
                this.target_f = target;
                this.action_f = action;
                this.aggressive_f = aggressive;
            }
            public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is QueuedOrder r)) return false; return true && java.lang.JO.equals(target_f, r.target_f) && java.lang.JO.equals(action_f, r.action_f) && java.lang.JO.equals(aggressive_f, r.aggressive_f); }
            public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(target_f); h = 31 * h + java.lang.JO.hashCode(action_f); h = 31 * h + java.lang.JO.hashCode(aggressive_f); } return h; }
            public override string toString() { return "QueuedOrder[" + "target=" + java.lang.JO.toString(target_f) + ", action=" + java.lang.JO.toString(action_f) + ", aggressive=" + java.lang.JO.toString(aggressive_f) + "]"; }
            public com.oddlabs.tt.util.Target target_f;
            public com.oddlabs.tt.model.Action action_f;
            public bool aggressive_f;
        }
        public com.oddlabs.tt.player.Player owner;
        public com.oddlabs.tt.model.behaviour.Behaviour current_behaviour;
        public com.oddlabs.tt.model.Abilities abilities;
        public com.oddlabs.tt.model.Template template;
        public java.util.List controller_stack;
        public java.util.List order_queue;
        public bool dead;
        public bool should_decide;
        public com.oddlabs.tt.model.behaviour.Behaviour.State last;
        public int grid_x;
        public int grid_y;
        public int layer;
        public int human_override_until_tick;
        // Java: 52
        public virtual int getHumanOverrideUntilTick()
        {
            return human_override_until_tick;
        }
        // Java: 56
        public virtual void setHumanOverrideUntilTick(int tick)
        {
            human_override_until_tick = tick;
        }
        public com.oddlabs.tt.player.Formation formation;
        // Java: 73
        public virtual com.oddlabs.tt.player.Formation getFormation()
        {
            return formation;
        }
        // Java: 77
        public virtual void setFormation(com.oddlabs.tt.player.Formation formation)
        {
            this.formation = formation;
        }
        // Java: 88
        public virtual void setLayer(int layer)
        {
            this.layer = layer;
        }
        // Java: 92
        public virtual int getLayer()
        {
            return layer;
        }
        // Java: 96
        public override float getShadowDiameter()
        {
            return template.getShadowDiameter();
        }
        // Java: 101
        public com.oddlabs.tt.model.Template getTemplate()
        {
            return template;
        }
        // Java: 105
        public virtual float getDefenseChance()
        {
            return template.getDefenseChance();
        }
        // Java: 109
        public override float getNoDetailSize()
        {
            return template.getNoDetailSize();
        }
        // Java: 114
        public abstract bool isEnabled();
        // Java: 116
        public virtual float getHitOffsetZ()
        {
            return template.getHitOffsetZ(0);
        }
        // Java: 120
        public com.oddlabs.tt.model.behaviour.Controller getCurrentController()
        {
            return ((com.oddlabs.tt.model.behaviour.Controller)controller_stack.getLast());
        }
        // Java: 124
        public void animate(float t)
        {
            if (!order_queue.isEmpty() && !isDead() && controller_stack.size() == 1 && ((object)last != (object)com.oddlabs.tt.model.behaviour.Behaviour.State.UNINTERRUPTIBLE) && !((this is com.oddlabs.tt.model.Unit unit) && unit.isMounted()))
            {
                startNextQueuedOrder();
                if (isDead())
                {
                    return;
                }
            }
            if (current_behaviour == null && !recoverFromError())
            {
                return;
            }
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
        // Java: 155
        public virtual void doAnimate(float t)
        {
        }
        // Java: 162
        public bool recoverFromError()
        {
            if (isDead() || controller_stack.isEmpty())
            {
                return false;
            }
            order_queue.clear();
            clearControllerStack();
            last = com.oddlabs.tt.model.behaviour.Behaviour.State.INTERRUPTIBLE;
            doDecide();
            return current_behaviour != null;
        }
        // Java: 172
        public bool isBlocking()
        {
            return current_behaviour.isBlocking();
        }
        // Java: 176
        public com.oddlabs.tt.model.behaviour.Behaviour getCurrentBehaviour()
        {
            return current_behaviour;
        }
        // Java: 180
        public com.oddlabs.tt.pathfinder.UnitGrid getUnitGrid()
        {
            return owner.getWorld().getUnitGrid();
        }
        // Java: 184
        public void scanVicinity(com.oddlabs.tt.pathfinder.ScanFilter filter)
        {
            getUnitGrid().scan(filter, getGridX(), getGridY(), com.oddlabs.tt.pathfinder.UnitGrid.LAND);
            getUnitGrid().scan(filter, getGridX(), getGridY(), com.oddlabs.tt.pathfinder.UnitGrid.SEA);
        }
        // Java: 190
        public static bool isAdjacent(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, int grid_x, int grid_y, com.oddlabs.tt.pathfinder.Occupant occ)
        {
            return com.oddlabs.tt.model.Selectable.isAdjacent(unit_grid, grid_x, grid_y, occ, com.oddlabs.tt.pathfinder.UnitGrid.LAND);
        }
        // Java: 194
        public static bool isAdjacent(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, int grid_x, int grid_y, com.oddlabs.tt.pathfinder.Occupant occ, int layer)
        {
            // (Steam 2.2.207 fix: any of the 8 cells around, not only the one towards the target's middle - builders of
            // a ship, or of a building whose cells aren't a plain rectangle, stood next to it and never started)
            int size = unit_grid.getGridSize();
            for (int x = java.lang.StrictMath.max(0, grid_x - 1); x <= grid_x + 1 && x < size; x++)
            {
                for (int y = java.lang.StrictMath.max(0, grid_y - 1); y <= grid_y + 1 && y < size; y++)
                {
                    if (((object)unit_grid.getOccupant(x, y, layer) == (object)occ))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        // Java: 212
        public bool isCloseEnough(float max_dist, com.oddlabs.tt.util.Target target)
        {
            return com.oddlabs.tt.model.Selectable.isCloseEnough(getUnitGrid(), max_dist, getGridX(), getGridY(), target, com.oddlabs.tt.pathfinder.UnitGrid.LAND);
        }
        // Java: 217
        public bool isCloseEnough(float max_dist, com.oddlabs.tt.util.Target target, int layer)
        {
            return com.oddlabs.tt.model.Selectable.isCloseEnough(getUnitGrid(), max_dist, getGridX(), getGridY(), target, layer);
        }
        // Java: 222
        public static bool isCloseEnough(com.oddlabs.tt.pathfinder.UnitGrid unit_grid, float max_dist, int grid_x, int grid_y, com.oddlabs.tt.util.Target target)
        {
            return com.oddlabs.tt.model.Selectable.isCloseEnough(unit_grid, max_dist, grid_x, grid_y, target, com.oddlabs.tt.pathfinder.UnitGrid.LAND);
        }
        // Java: 228
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
        // Java: 241
        public override void register()
        {
            base.register();
            owner.getWorld().getAnimationManagerGameTime().registerAnimation(this);
            enable();
        }
        // Java: 248
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
        // Java: 256
        public void doDecide()
        {
            should_decide = false;
            current_behaviour = null;
            getCurrentController().decide();
        }
        // Java: 262
        public void forceDecide()
        {
            current_behaviour.forceInterrupted();
            last = com.oddlabs.tt.model.behaviour.Behaviour.State.INTERRUPTIBLE;
            doDecide();
        }
        // Java: 268
        public void pushController(com.oddlabs.tt.model.behaviour.Controller controller)
        {
            controller_stack.add(controller);
            decide();
        }
        // Java: 274
        public void pushControllers(params com.oddlabs.tt.model.behaviour.Controller[] controllers)
        {
            controller_stack.addAll(java.util.Arrays.asList(controllers));
            decide();
        }
        // Java: 280
        public void swapController(com.oddlabs.tt.model.behaviour.Controller controller)
        {
            controller_stack.removeLast();
            pushController(controller);
        }
        // Java: 286
        public void popController()
        {
            if (controller_stack.size() > 1)
            {
                controller_stack.removeLast();
            }
            decide();
        }
        // Java: 304
        public void setBehaviour(com.oddlabs.tt.model.behaviour.Behaviour behaviour)
        {
            current_behaviour = behaviour;
        }
        // Java: 310
        public com.oddlabs.tt.model.behaviour.Controller getPrimaryController()
        {
            return (controller_stack.size() > 1 ? ((com.oddlabs.tt.model.behaviour.Controller)controller_stack.get(1)) : ((com.oddlabs.tt.model.behaviour.Controller)controller_stack.getFirst()));
        }
        // Java: 315
        public void clearControllerStack()
        {
            com.oddlabs.tt.model.behaviour.Controller default_controller = ((com.oddlabs.tt.model.behaviour.Controller)controller_stack.getFirst());
            controller_stack.clear();
            controller_stack.add(default_controller);
        }
        // Java: 321
        public void initTarget(com.oddlabs.tt.util.Target target, com.oddlabs.tt.model.Action action, bool aggressive)
        {
            if (target == null)
            {
                return;
            }
            order_queue.clear();
            clearControllerStack();
            setTarget(target, action, aggressive);
        }
        // Java: 331
        public void enqueueTarget(com.oddlabs.tt.util.Target target, com.oddlabs.tt.model.Action action, bool aggressive)
        {
            if (isDead() || target == null || ((object)target == (object)this) || target.isDead())
            {
                return;
            }
            if (order_queue.size() >= com.oddlabs.tt.model.Selectable.MAX_QUEUED_ORDERS)
            {
                return;
            }
            order_queue.add(new com.oddlabs.tt.model.Selectable.QueuedOrder(target, action, aggressive));
        }
        // Java: 340
        public int getQueuedOrderCount()
        {
            return order_queue.size();
        }
        // Java: 344
        public void clearOrderQueue()
        {
            order_queue.clear();
        }
        // Java: 348
        public void startNextQueuedOrder()
        {
            while (!order_queue.isEmpty())
            {
                com.oddlabs.tt.model.Selectable.QueuedOrder order = ((com.oddlabs.tt.model.Selectable.QueuedOrder)order_queue.removeFirst());
                com.oddlabs.tt.util.Target target = order.target();
                if (((object)target == (object)this) || target.isDead())
                {
                    continue;
                }
                clearControllerStack();
                setTarget(target, order.action(), order.aggressive());
                return;
            }
        }
        // Java: 360
        public abstract void setTarget(com.oddlabs.tt.util.Target target, com.oddlabs.tt.model.Action action, bool aggressive);
        // Java: 362
        public abstract com.oddlabs.tt.model.AttackScanFilter.Priority getAttackPriority();
        // Java: 364
        public abstract int getStatusValue();
        // Java: 366
        public com.oddlabs.tt.model.Abilities getAbilities()
        {
            return abilities;
        }
        // Java: 370
        public int getGridX()
        {
            return grid_x;
        }
        // Java: 375
        public int getGridY()
        {
            return grid_y;
        }
        // Java: 380
        public int getIslandId()
        {
            return getUnitGrid().getIslandId(grid_x, grid_y);
        }
        // Java: 384
        public void setGridPosition(int grid_x, int grid_y)
        {
            this.grid_x = grid_x;
            this.grid_y = grid_y;
        }
        // Java: 392
        public com.oddlabs.tt.player.Player getOwnerNoCheck()
        {
            return owner;
        }
        // Java: 396
        public com.oddlabs.tt.player.Player getOwner()
        {
            return getOwnerNoCheck();
        }
        // Java: 400
        public override void remove()
        {
            base.remove();
            owner.getWorld().getAnimationManagerGameTime().removeAnimation(this);
        }
        // Java: 406
        public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
        }
        // Java: 414
        public void disable()
        {
            owner.getWorld().getNotificationListener().unregisterTarget(this);
            owner.getUnits().remove(this);
        }
        // Java: 419
        public void enable()
        {
            owner.getWorld().getNotificationListener().registerTarget(this);
            owner.getUnits().add(this);
        }
        // Java: 425
        public virtual void removeDying()
        {
            disable();
            dead = true;
        }
        // Java: 430
        public bool isDead()
        {
            return dead;
        }
        // Java: 435
        public virtual void hit(int damage, float direction_x, float direction_y, com.oddlabs.tt.player.Player attacker)
        {
            if (owner.isEnemy(attacker))
            {
                owner.getWorld().getNotificationListener().newAttackNotification(this);
            }
        }
        // Java: 441
        public static System.Type genericClass()
        {
            return ((System.Type)((System.Type)typeof(com.oddlabs.tt.model.Selectable)));
        }
        // Java: 446
        public static com.oddlabs.tt.model.Selectable[] newArray(int length)
        {
            return new com.oddlabs.tt.model.Selectable[length];
        }
        // Java: 451
        public static com.oddlabs.tt.model.Selectable[] newArray(params com.oddlabs.tt.model.Selectable[] selectables)
        {
            return selectables;
        }
        private void init_Selectable()
        {
            this.abilities = new com.oddlabs.tt.model.Abilities(com.oddlabs.tt.model.Abilities.NONE);
            this.controller_stack = new java.util.ArrayList();
            this.order_queue = new java.util.ArrayList();
            this.last = com.oddlabs.tt.model.behaviour.Behaviour.State.DONE;
            this.human_override_until_tick = (-1);
            this.formation = null;
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
