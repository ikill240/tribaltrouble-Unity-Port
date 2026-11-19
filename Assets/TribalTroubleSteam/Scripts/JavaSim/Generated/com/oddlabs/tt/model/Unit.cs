// Translated from Unit.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 52
    public class Unit : com.oddlabs.tt.model.Selectable, com.oddlabs.tt.pathfinder.Occupant, com.oddlabs.tt.pathfinder.Movable
    {
        public const float IDLE_SPEED = 0.4f;
        public const float TRANSPORT_SPEED_SCALE = 0.8f;
        public const int PENALTY_INCREMENT = 3;
        public const int INITIAL_PATH_PENALTY = 5;
        public static float[] MAX_MAGIC_ENERGY;
        // Java: 61
        public class Animation : java.lang.Object
        {
            public com.oddlabs.tt.model.Unit this_0;
            public const int IDLING = 0;
            public const int MOVING = 1;
            public const int THROWING = 2;
            public const int DYING = 3;
            public const int MAGIC = 4;
            public const int THOR = 5;
            public const int SITTING = 4;
            public const int STEERING = 5;
            public const int ROWING_RIGHT = 6;
            public const int ROWING_LEFT = 7;
            public Animation(com.oddlabs.tt.model.Unit this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        public const int SPEAR_RELEASE_FRAME = 29;
        public const int NO_ATTACHMENT = (-1);
        public com.oddlabs.tt.model.UnitSupplyContainer supply_container;
        public java.util.List attachments;
        public string name;
        public com.oddlabs.tt.pathfinder.PathTracker path_tracker;
        public float[] magic_energy;
        public int last_magic_index;
        public com.oddlabs.tt.particle.BalancedParametricEmitter stun_marker;
        public int hit_points;
        public int animation;
        public float anim_speed;
        public float anim_time;
        public int path_penalty;
        public bool imaginary;
        public bool mounted;
        public bool on_ship;
        public float mount_offset;
        public com.oddlabs.tt.model.Building mounted_building;
        public float range_bonus;
        // Java: 174
        public override com.oddlabs.tt.model.Element self()
        {
            return this;
        }
        // Java: 179
        public override float getZError()
        {
            if (on_ship)
            {
                return 0.0f;
            }
            else
            {
                return getLandscapeError();
            }
        }
        // Java: 188
        public override void visit(com.oddlabs.tt.model.ElementVisitor visitor)
        {
            visitor.visitUnit(this);
        }
        // Java: 193
        public com.oddlabs.tt.model.UnitSupplyContainer getSupplyContainer()
        {
            return supply_container;
        }
        // Java: 197
        public java.util.List getAttachments()
        {
            return attachments;
        }
        // Java: 204
        public int getAttachment(string slot)
        {
            int index = 0;
            foreach (com.oddlabs.tt.render.SpriteKey key in java.lang.JIter.of(((java.util.Map)((com.oddlabs.tt.model.UnitTemplate)getTemplate()).getAttachments().getOrDefault(slot, java.util.Map_.of())).values()))
            {
                if (attachments.contains(key))
                {
                    return index;
                }
                index++;
            }
            return com.oddlabs.tt.model.Unit.NO_ATTACHMENT;
        }
        // Java: 217
        public void setAttachment(string slot, int index)
        {
            int i = 0;
            foreach (com.oddlabs.tt.render.SpriteKey key in java.lang.JIter.of(((java.util.Map)((com.oddlabs.tt.model.UnitTemplate)getTemplate()).getAttachments().getOrDefault(slot, java.util.Map_.of())).values()))
            {
                attachments.remove(key);
                if (i++ == index)
                {
                    attachments.add(key);
                }
            }
        }
        // Java: 226
        public override string toString()
        {
            if (!isDead())
            {
                return "Unit: " + (java.lang.JO.hashCode(this)) + " | getOwner() = " + java.lang.JStr.valueOf(getOwner()) + " | mounted = " + java.lang.JStr.valueOf(mounted) + " | getGridX() = " + (getGridX()) + " | getGridY() = " + (getGridY());
            }
            else
            {
                return base.toString();
            }
        }
        // Java: 234
        public virtual void reposition(com.oddlabs.tt.model.Building building)
        {
            findInitialPosition(getPositionX(), getPositionY(), true, building.getIslandId());
        }
        // Java: 238
        public void findInitialPosition(float x, float y, bool grid_targets_only, int island)
        {
            com.oddlabs.tt.pathfinder.UnitGrid unit_grid = getUnitGrid();
            com.oddlabs.tt.util.Target reserved_target = unit_grid.findGridTargets(com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(x), com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(y), 1, grid_targets_only, island)[0];
            setGridPosition(reserved_target.getGridX(), reserved_target.getGridY());
            setPosition(reserved_target.getPositionX(), reserved_target.getPositionY());
            float center = (float)(getOwner().getWorld().getHeightMap().getMetersPerWorld() / 2.0f);
            float dx = (float)(center - reserved_target.getPositionX());
            float dy = (float)(center - reserved_target.getPositionY());
            float len = ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy)))));
            if (len > 0)
            {
                setDirection((float)(dx / len), (float)(dy / len));
            }
            occupy();
            reinsert();
        }
        // Java: 258
        public override int getStatusValue()
        {
            int tower_factor = 1;
            if (mounted)
            {
                tower_factor = 3;
            }
            return ((com.oddlabs.tt.model.UnitTemplate)getTemplate()).getStatusValue() * tower_factor;
        }
        // Java: 266
        public void increaseRange(float amount)
        {
            range_bonus += amount;
        }
        // Java: 271
        public override com.oddlabs.tt.model.AttackScanFilter.Priority getAttackPriority()
        {
            return (getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD) ? com.oddlabs.tt.model.AttackScanFilter.Priority.PEON : com.oddlabs.tt.model.AttackScanFilter.Priority.WARRIOR);
        }
        // Java: 278
        public override void visit(com.oddlabs.tt.model.ToolTipVisitor visitor)
        {
            visitor.visitUnit(this);
        }
        // Java: 283
        public string getName()
        {
            return name;
        }
        // Java: 287
        public int getHitPoints()
        {
            return hit_points;
        }
        // Java: 291
        public void drown()
        {
            if (isDead())
            {
                return;
            }
            clearControllerStack();
            setReference(null);
            mounted = false;
            on_ship = false;
            mount_offset = 0;
            if (!imaginary)
            {
                enable();
            }
            if (supply_container != null)
            {
                supply_container.resetSupply(typeof(com.oddlabs.tt.model.LeftPaddle));
                supply_container.resetSupply(typeof(com.oddlabs.tt.model.RightPaddle));
            }
            mounted_building = null;
            startDying();
        }
        // Java: 311
        public void unmount()
        {
            clearControllerStack();
            swapController(new com.oddlabs.tt.model.behaviour.IdleController(this, new com.oddlabs.tt.model.AttackScanFilter(getOwner(), com.oddlabs.tt.model.AttackScanFilter.UNIT_RANGE), true));
            mounted = false;
            on_ship = false;
            mount_offset = 0;
            enable();
            com.oddlabs.tt.model.Building entrance = mounted_building.getEntrance();
            findInitialPosition(entrance.getPositionX(), entrance.getPositionY(), true, entrance.getIslandId());
            if (supply_container != null)
            {
                supply_container.resetSupply(typeof(com.oddlabs.tt.model.LeftPaddle));
                supply_container.resetSupply(typeof(com.oddlabs.tt.model.RightPaddle));
            }
            mounted_building = null;
        }
        // Java: 328
        public void mount(com.oddlabs.tt.model.Building building)
        {
            mounted_building = building;
            mount_offset = ((com.oddlabs.tt.model.BuildingTemplate)building.getTemplate()).getMountOffset();
            if (!imaginary)
            {
                disable();
                free();
            }
            setPosition(building.getPositionX(), building.getPositionY());
            mounted = true;
            clearControllerStack();
            swapController(new com.oddlabs.tt.model.behaviour.IdleController(this, new com.oddlabs.tt.model.AttackScanFilter(getOwner(), com.oddlabs.tt.model.AttackScanFilter.TOWER_RANGE), false));
        }
        // Java: 342
        public void mount(com.oddlabs.tt.model.Ship ship, com.oddlabs.tt.model.ShipAllocation ship_allocation)
        {
            mounted_building = ship;
            if (!imaginary)
            {
                disable();
                free();
            }
            mounted = true;
            on_ship = true;
            setReference(ship);
            clearControllerStack();
            switch ((ship_allocation.getRole()))
            {
                case 3:
                {
                    swapController(new com.oddlabs.tt.model.behaviour.ShipAttackController(this, ship, new com.oddlabs.tt.model.AttackScanFilter(getOwner(), com.oddlabs.tt.model.AttackScanFilter.TOWER_RANGE + 10), ship_allocation));
                    break;
                }
                default:
                {
                    swapController(new com.oddlabs.tt.model.behaviour.SittingController(this, ship, ship_allocation));
                    break;
                }
            }
        }
        // Java: 369
        public bool isMounted()
        {
            return mounted;
        }
        // Java: 373
        public override bool isEnabled()
        {
            return !isDead() && !mounted;
        }
        // Java: 378
        public float getMetersPerSecond()
        {
            if (getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.HARVEST) && supply_container.getNumSupplies() > 0)
            {
                return (float)(com.oddlabs.tt.model.Unit.TRANSPORT_SPEED_SCALE * ((com.oddlabs.tt.model.UnitTemplate)getTemplate()).getMetersPerSecond());
            }
            else
            {
                return ((com.oddlabs.tt.model.UnitTemplate)getTemplate()).getMetersPerSecond();
            }
        }
        // Java: 386
        public void aimAtTarget(com.oddlabs.tt.util.Target target)
        {
            float dx = (float)(target.getPositionX() - getPositionX());
            float dy = (float)(target.getPositionY() - getPositionY());
            float dir_len_inv = (float)(1.0f / ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy))))));
            dx = (float)(dx * dir_len_inv);
            dy = (float)(dy * dir_len_inv);
            setDirection(dx, dy);
        }
        // Java: 396
        public void switchToIdleAnimation()
        {
            switchAnimation(com.oddlabs.tt.model.Unit.IDLE_SPEED, com.oddlabs.tt.model.Unit.Animation.IDLING);
        }
        // Java: 401
        public void switchToSittingAnimation()
        {
            switchAnimation(com.oddlabs.tt.model.Unit.IDLE_SPEED, com.oddlabs.tt.model.Unit.Animation.SITTING);
        }
        // Java: 406
        public void switchToSteeringAnimation()
        {
            switchAnimation(com.oddlabs.tt.model.Unit.IDLE_SPEED, com.oddlabs.tt.model.Unit.Animation.STEERING);
        }
        // Java: 411
        public void switchToRowingRightAnimation()
        {
            switchAnimation(com.oddlabs.tt.model.Unit.IDLE_SPEED, com.oddlabs.tt.model.Unit.Animation.ROWING_RIGHT);
            supply_container.increaseSupply(1, typeof(com.oddlabs.tt.model.RightPaddle));
        }
        // Java: 418
        public void switchToRowingLeftAnimation()
        {
            switchAnimation(com.oddlabs.tt.model.Unit.IDLE_SPEED, com.oddlabs.tt.model.Unit.Animation.ROWING_LEFT);
            supply_container.increaseSupply(1, typeof(com.oddlabs.tt.model.LeftPaddle));
        }
        // Java: 425
        public com.oddlabs.tt.model.weapon.WeaponFactory getWeaponFactory()
        {
            return ((com.oddlabs.tt.model.UnitTemplate)getTemplate()).getWeaponFactory();
        }
        // Java: 430
        public float getRange(com.oddlabs.tt.util.Target target)
        {
            return (float)((float)(getWeaponFactory().getRange() + range_bonus) + target.getSize());
        }
        // Java: 435
        public override float getSize()
        {
            return 1.9f;
        }
        // Java: 440
        public override com.oddlabs.tt.render.SpriteKey getSpriteRenderer()
        {
            return getOwner().getSkins().rendererFor(((com.oddlabs.tt.model.UnitTemplate)getTemplate()));
        }
        // Java: 445
        public override void doAnimate(float t)
        {
            anim_time += (float)(anim_speed * t);
            if (isDead() || mounted)
            {
                reinsert();
            }
            getOwner().getWorld().updateGlobalChecksum(animation);
            if (getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC))
            {
                for (int i = 0; i < magic_energy.Length; i++)
                {
                    increaseMagicEnergy(i, t);
                }
            }
        }
        // Java: 459
        public void increaseMagicEnergy(int index, float amount)
        {
            magic_energy[index] += amount;
            if (magic_energy[index] > com.oddlabs.tt.model.Unit.MAX_MAGIC_ENERGY[index])
            {
                magic_energy[index] = com.oddlabs.tt.model.Unit.MAX_MAGIC_ENERGY[index];
            }
        }
        // Java: 466
        public com.oddlabs.tt.pathfinder.PathTracker getTracker()
        {
            return path_tracker;
        }
        // Java: 472
        public void markBlocking()
        {
            path_penalty = java.lang.Math.min(path_penalty + com.oddlabs.tt.model.Unit.PENALTY_INCREMENT, com.oddlabs.tt.pathfinder.Occupant.STATIC - 1);
        }
        // Java: 478
        public int getPenalty()
        {
            return (isBlocking() ? com.oddlabs.tt.pathfinder.Occupant.STATIC : path_penalty);
        }
        // Java: 484
        public override void removeDying()
        {
            if (getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC))
            {
                getOwner().setActiveChieftain(null);
            }
            if (!imaginary)
            {
                free();
                if (!getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC))
                {
                    int result = getOwner().getUnitCountContainer().increaseSupply((-1));
                }
            }
            if (stun_marker != null)
            {
                stun_marker.done();
                stun_marker = null;
            }
            base.removeDying();
        }
        // Java: 503
        public void removeNow()
        {
            removeDying();
            remove();
        }
        // Java: 509
        public void free()
        {
            com.oddlabs.tt.pathfinder.UnitGrid unit_grid = getUnitGrid();
            if (((object)unit_grid.getOccupant(getGridX(), getGridY()) == (object)this))
            {
                unit_grid.freeGrid(getGridX(), getGridY(), this);
            }
            path_penalty = com.oddlabs.tt.model.Unit.INITIAL_PATH_PENALTY;
        }
        // Java: 519
        public void occupy()
        {
            com.oddlabs.tt.pathfinder.UnitGrid unit_grid = getUnitGrid();
            unit_grid.occupyGrid(getGridX(), getGridY(), this);
            getOwner().unitMoved();
        }
        // Java: 529
        public bool isMoving()
        {
            return (getCurrentBehaviour() is com.oddlabs.tt.model.behaviour.WalkBehaviour);
        }
        // Java: 539
        public override void hit(int damage, float direction_x, float direction_y, com.oddlabs.tt.player.Player owner)
        {
            base.hit(damage, direction_x, direction_y, owner);
            if (mounted && !on_ship)
            {
                mounted_building.hit(damage, direction_x, direction_y, owner);
            }
            else if (!isDead())
            {
                hit_points = java.lang.Math.clamp(hit_points - damage, 0, ((com.oddlabs.tt.model.UnitTemplate)getTemplate()).getMaxHitPoints());
                if (hit_points == 0)
                {
                    owner.unitKilled();
                    if ((mounted_building is com.oddlabs.tt.model.Ship ship))
                    {
                        ship.getShipHR().removeUnit(this);
                        drown();
                    }
                    else
                    {
                        startDying();
                        setDirection((float)(-direction_x), (float)(-direction_y));
                    }
                }
            }
        }
        // Java: 559
        public void startDying()
        {
            getOwner().unitLost();
            mounted = false;
            on_ship = false;
            mount_offset = 0;
            pushController(new com.oddlabs.tt.model.behaviour.DieController(this));
            forceDecide();
            getOwner().getWorld().getAudio().newAudio(new com.oddlabs.tt.audio.AudioParameters(((com.oddlabs.tt.model.UnitTemplate)getTemplate()).getDeathSound(), getPositionX(), getPositionY(), getPositionZ(), com.oddlabs.tt.audio.AudioPlayer.AUDIO_RANK_DEATH, com.oddlabs.tt.audio.AudioPlayer.AUDIO_DISTANCE_DEATH, com.oddlabs.tt.audio.AudioPlayer.AUDIO_GAIN_DEATH, com.oddlabs.tt.audio.AudioPlayer.AUDIO_RADIUS_DEATH, (float)(1.0f + (float)(((float)(getOwner().getWorld().getRandom().nextFloat() - 0.5f)) * ((com.oddlabs.tt.model.UnitTemplate)getTemplate()).getDeathPitch()))));
            removeDying();
        }
        // Java: 582
        public void stun(float time)
        {
            float x = (float)((float)(getPositionX() + (float)(((com.oddlabs.tt.model.UnitTemplate)getTemplate()).getStunX() * getDirectionX())) + (float)(((com.oddlabs.tt.model.UnitTemplate)getTemplate()).getStunY() * ((float)(-getDirectionY()))));
            float y = (float)((float)(getPositionY() + (float)(((com.oddlabs.tt.model.UnitTemplate)getTemplate()).getStunX() * getDirectionY())) + (float)(((com.oddlabs.tt.model.UnitTemplate)getTemplate()).getStunY() * getDirectionX()));
            float z = (float)((float)(getOwner().getWorld().getHeightMap().getNearestHeight(x, y) + ((com.oddlabs.tt.model.UnitTemplate)getTemplate()).getStunZ()) + mount_offset);
            if (stun_marker != null)
            {
                stun_marker.done();
            }
            stun_marker = createStunStar(x, y, z, time, (float)(((float)(java.lang.Math.PI)) / 2));
            pushController(new com.oddlabs.tt.model.behaviour.StunController(this, time));
            forceDecide();
        }
        // Java: 595
        public bool isWarrior()
        {
            System.Type type = getWeaponFactory().getType();
            if (((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)) || ((object)type == (object)typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)) || ((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)) || ((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RockSpearWeapon)) || ((object)type == (object)typeof(com.oddlabs.tt.model.weapon.IronSpearWeapon)) || ((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RubberSpearWeapon)))
            {
                return true;
            }
            return false;
        }
        // Java: 608
        public bool isChieftain()
        {
            return getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC);
        }
        // Java: 612
        public com.oddlabs.tt.particle.BalancedParametricEmitter createStunStar(float x, float y, float z, float time, float velocity)
        {
            int num_particles = 5;
            return new com.oddlabs.tt.particle.BalancedParametricEmitter(getOwner().getWorld(), new com.oddlabs.tt.particle.StunFunction(0.4f, 0.15f), new org.joml.Vector3f(x, y, z), velocity, 5.0f, (float)(((float)(java.lang.Math.PI)) * 2), (float)(((float)(java.lang.Math.PI)) * 2), num_particles, 0.0f, 2.0f, new org.joml.Vector4f(1.0f, 1.0f, 1.0f, 1.0f), new org.joml.Vector4f(0.0f, 0.0f, 0.0f, 0.0f), new org.joml.Vector3f(0.1f, 0.1f, 0.1f), new org.joml.Vector3f(0.0f, 0.0f, 0.0f), time, org.lwjgl.opengl.GL11.GL_SRC_ALPHA, org.lwjgl.opengl.GL11.GL_ONE_MINUS_SRC_ALPHA, getOwner().getWorld().getRacesResources().getStarTextures(), getOwner().getWorld().getAnimationManagerGameTime());
        }
        // Java: 625
        public bool canAttack(com.oddlabs.tt.util.Target target, bool kill_friendly)
        {
            if (!((target is com.oddlabs.tt.model.Selectable selectable)) || !getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.ATTACK))
            {
                return false;
            }
            com.oddlabs.tt.player.Player target_player = selectable.getOwner();
            return kill_friendly || getOwner().isEnemy(target_player);
        }
        // Java: 633
        public bool canBuild(com.oddlabs.tt.util.Target target)
        {
            return (target is com.oddlabs.tt.model.Building building) && getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD) && !building.isPlaced();
        }
        // Java: 639
        public bool canGather(com.oddlabs.tt.util.Target target)
        {
            return (target is com.oddlabs.tt.model.Supply) && getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD);
        }
        // Java: 643
        public com.oddlabs.tt.model.Building nearestSupplyBuilding(com.oddlabs.tt.model.Supply supply)
        {
            com.oddlabs.tt.pathfinder.UnitGrid grid = getUnitGrid();
            com.oddlabs.tt.model.BuildingFinder finder = new com.oddlabs.tt.model.BuildingFinder(getOwner(), com.oddlabs.tt.model.Abilities.SUPPLY_CONTAINER);
            com.oddlabs.tt.pathfinder.Region region = com.oddlabs.tt.pathfinder.PathFinder.findPathRegion(grid, new com.oddlabs.tt.pathfinder.TargetRegionFinder(grid, finder), grid.getRegion(supply.getGridX(), supply.getGridY()));
            com.oddlabs.tt.model.Building building = (region != null ? ((com.oddlabs.tt.model.Building)finder.getOccupantFromRegion(region, true)) : (com.oddlabs.tt.model.Building)(null));
            return (building != null ? building.getBase() : (com.oddlabs.tt.model.Building)(null));
        }
        // Java: 652
        public bool canRepair(com.oddlabs.tt.util.Target target, bool action_repair)
        {
            return (target is com.oddlabs.tt.model.Building building) && getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD) && (action_repair || !building.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.SUPPLY_CONTAINER) || !building.isComplete()) && !getOwner().isEnemy(building.getOwner()) && building.isPlaced() && building.isDamaged();
        }
        // Java: 661
        public virtual bool canEnter(com.oddlabs.tt.util.Target target)
        {
            if (!((target is com.oddlabs.tt.model.Building building)) || building.getUnitContainer() == null || ((object)getOwner() != (object)building.getOwner()) || !building.getUnitContainer().canEnter(this))
            {
                return false;
            }
            if ((target is com.oddlabs.tt.model.LandBuilding))
            {
                return !isChieftain();
            }
            else
            {
                return true;
            }
        }
        // Java: 674
        public override float getDefenseChance()
        {
            return ((getCurrentController() is com.oddlabs.tt.model.behaviour.StunController) ? 0 : base.getDefenseChance());
        }
        // Java: 679
        public void walkToTarget(com.oddlabs.tt.util.Target target, bool scan_attack)
        {
            com.oddlabs.tt.util.Target walkable_target = getUnitGrid().findGridTargets(target.getGridX(), target.getGridY(), 1, false)[0];
            pushController(new com.oddlabs.tt.model.behaviour.WalkController(this, walkable_target, scan_attack));
        }
        // Java: 684
        public override void setTarget(com.oddlabs.tt.util.Target target, com.oddlabs.tt.model.Action action, bool aggressive)
        {
            if (((object)target == (object)this))
            {
                return;
            }
            if ((target is com.oddlabs.tt.model.Building))
            {
                target = (((com.oddlabs.tt.model.Building)target)).getEntrance();
            }
            switch ((action).ordinal())
            {
                case 0 /*DEFAULT*/:
                {
                    if (canBuild(target))
                    {
                        pushController(new com.oddlabs.tt.model.behaviour.PlaceBuildingController(this, ((com.oddlabs.tt.model.Building)target)));
                    }
                    else if (canGather(target))
                    {
                        pushController(new com.oddlabs.tt.model.behaviour.GatherController(this, ((com.oddlabs.tt.model.Supply)target), (((com.oddlabs.tt.model.Supply)target)).GetType(), nearestSupplyBuilding(((com.oddlabs.tt.model.Supply)target))));
                    }
                    else if (canRepair(target, false))
                    {
                        pushController(new com.oddlabs.tt.model.behaviour.RepairController(this, ((com.oddlabs.tt.model.Building)target)));
                    }
                    else if (canEnter(target))
                    {
                        pushController(new com.oddlabs.tt.model.behaviour.EnterController(this, ((com.oddlabs.tt.model.Building)target)));
                    }
                    else if (canAttack(target, false))
                    {
                        pushController(new com.oddlabs.tt.model.behaviour.HuntController(this, ((com.oddlabs.tt.model.Selectable)target)));
                    }
                    else
                    {
                        walkToTarget(target, aggressive);
                    }
                    break;
                }
                case 1 /*MOVE*/:
                {
                    if (canEnter(target))
                    {
                        pushController(new com.oddlabs.tt.model.behaviour.EnterController(this, ((com.oddlabs.tt.model.Building)target)));
                    }
                    else
                    {
                        walkToTarget(target, false);
                    }
                    break;
                }
                case 2 /*ATTACK*/:
                {
                    if (canAttack(target, true))
                    {
                        pushController(new com.oddlabs.tt.model.behaviour.HuntController(this, ((com.oddlabs.tt.model.Selectable)target)));
                    }
                    else
                    {
                        walkToTarget(target, true);
                    }
                    break;
                }
                case 3 /*GATHER_REPAIR*/:
                {
                    if (canGather(target))
                    {
                        pushController(new com.oddlabs.tt.model.behaviour.GatherController(this, ((com.oddlabs.tt.model.Supply)target), (((com.oddlabs.tt.model.Supply)target)).GetType(), nearestSupplyBuilding(((com.oddlabs.tt.model.Supply)target))));
                    }
                    else if (canRepair(target, true))
                    {
                        pushController(new com.oddlabs.tt.model.behaviour.RepairController(this, ((com.oddlabs.tt.model.Building)target)));
                    }
                    break;
                }
                case 4 /*DEFEND*/:
                {
                    pushController(new com.oddlabs.tt.model.behaviour.DefendController(this, target));
                    break;
                }
                default:
                {
                    java.lang.IO.println("Invalid action: " + java.lang.JStr.valueOf(action));
                    break;
                }
            }
        }
        // Java: 741
        public void printDebugInfo()
        {
            java.lang.IO.println("-----------------------------------");
            java.lang.IO.println("Primary Controller = " + java.lang.JStr.valueOf(getPrimaryController()));
            if (getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC))
            {
                java.lang.IO.println("Hit Points = " + (hit_points));
                java.lang.IO.println("Magic Energy 0 = " + java.lang.JStr.valueOf(magic_energy[0]));
                java.lang.IO.println("Magic Energy 1 = " + java.lang.JStr.valueOf(magic_energy[1]));
                java.lang.IO.println("Controller = " + java.lang.JStr.valueOf(getPrimaryController()));
            }
        }
        // Java: 752
        public bool canDoMagic(int magic_index)
        {
            return !isDead() && magic_index >= 0 && magic_index < com.oddlabs.tt.model.RacesResources.NUM_MAGIC && getOwner().canDoMagic(magic_index) && magic_energy[magic_index] == com.oddlabs.tt.model.Unit.MAX_MAGIC_ENERGY[magic_index];
        }
        // Java: 757
        public void doMagic(int magic_index, bool clear_stack)
        {
            if (canDoMagic(magic_index))
            {
                if (clear_stack)
                {
                    clearControllerStack();
                }
                pushController(new com.oddlabs.tt.model.behaviour.MagicController(this, getOwner().getRace().getMagicFactory(magic_index)));
                java.util.Arrays.fill(magic_energy, 0.0f);
                last_magic_index = magic_index;
                getOwner().magicCast();
            }
        }
        // Java: 770
        public int getLastMagicIndex()
        {
            return last_magic_index;
        }
        // Java: 774
        public float getMagicProgress(int magic_index)
        {
            return (float)(magic_energy[magic_index] / com.oddlabs.tt.model.Unit.MAX_MAGIC_ENERGY[magic_index]);
        }
        // Java: 778
        public void switchAnimation(float anim_speed, int animation)
        {
            if (supply_container != null)
            {
                supply_container.resetSupply(typeof(com.oddlabs.tt.model.LeftPaddle));
                supply_container.resetSupply(typeof(com.oddlabs.tt.model.RightPaddle));
            }
            this.anim_speed = anim_speed;
            if (this.animation != animation)
            {
                this.animation = animation;
                this.anim_time = 0.0f;
            }
            else if (((com.oddlabs.tt.model.UnitTemplate)getTemplate()).getSpriteRenderer().getAnimationType(animation) == com.oddlabs.geometry.AnimationInfo.AnimationType.PLAIN.ordinal())
            {
                this.anim_time = 0.0f;
            }
        }
        // Java: 795
        public override int getAnimation()
        {
            return animation;
        }
        // Java: 800
        public override float getAnimationTicks()
        {
            return anim_time;
        }
        // Java: 805
        public void setMountOffset(float offset)
        {
            mount_offset = offset;
        }
        // Java: 809
        public float getMountOffset()
        {
            return mount_offset;
        }
        // Java: 814
        public override float getOffsetZ()
        {
            if (mounted)
            {
                return (float)(mounted_building.getOffsetZ() + mount_offset);
            }
            else
            {
                if (isDead())
                {
                    com.oddlabs.tt.model.behaviour.DieBehaviour die_behaviour = ((com.oddlabs.tt.model.behaviour.DieBehaviour)getCurrentBehaviour());
                    return die_behaviour.getOffsetZ();
                }
                else
                {
                    return calculateSlopeOffset();
                }
            }
        }
        // Java: 827
        public float calculateSlopeOffset()
        {
            float r = (float)(getSize() * 0.2f);
            float x = getPositionX();
            float y = getPositionY();
            com.oddlabs.tt.landscape.HeightMap hm = getOwner().getWorld().getHeightMap();
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
        // Java: 853
        public float getHitError()
        {
            if (on_ship && (mounted_building is com.oddlabs.tt.model.Ship ship))
            {
                if (ship.getShipHR().hasChieftain())
                {
                    return 0.2f;
                }
                else
                {
                    return 2.2f;
                }
            }
            else
            {
                return 0.0f;
            }
        }
        // Java: 865
        public void debugRender()
        {
            path_tracker.debugRender();
        }
        static Unit()
        {
            Unit.MAX_MAGIC_ENERGY = new float[] { 40.0f, 70.0f };
        }
        private void init_Unit()
        {
            this.attachments = new java.util.ArrayList(1);
            this.magic_energy = new float[2];
            this.last_magic_index = (-1);
            this.animation = com.oddlabs.tt.model.Unit.Animation.IDLING;
            this.on_ship = false;
            this.mount_offset = 0;
        }
        public Unit(com.oddlabs.tt.player.Player owner, float x, float y, com.oddlabs.tt.util.Target rally_point, com.oddlabs.tt.model.UnitTemplate unit_template) : this(owner, x, y, rally_point, unit_template, null)
        {
        }
        public Unit(com.oddlabs.tt.player.Player owner, float x, float y, com.oddlabs.tt.util.Target rally_point, com.oddlabs.tt.model.UnitTemplate unit_template, string name) : this(owner, x, y, rally_point, unit_template, name, true)
        {
        }
        public Unit(com.oddlabs.tt.player.Player owner, float x, float y, com.oddlabs.tt.util.Target rally_point, com.oddlabs.tt.model.UnitTemplate unit_template, string name, bool notify_by_chieftain) : this(owner, x, y, rally_point, unit_template, name, notify_by_chieftain, false)
        {
        }
        public Unit(com.oddlabs.tt.player.Player owner, float x, float y, com.oddlabs.tt.util.Target rally_point, com.oddlabs.tt.model.UnitTemplate unit_template, string name, bool notify_by_chieftain, bool grid_targets_only) : this(owner, x, y, rally_point, unit_template, name, notify_by_chieftain, grid_targets_only, false)
        {
        }
        public Unit(com.oddlabs.tt.player.Player owner, float x, float y, com.oddlabs.tt.util.Target rally_point, com.oddlabs.tt.model.UnitTemplate unit_template, string name, bool notify_by_chieftain, bool grid_targets_only, bool imaginary) : base(owner, unit_template)
        {
            init_Unit();
            this.name = name;
            this.imaginary = imaginary;
            getAbilities().addAbilities(unit_template.getAbilities());
            register();
            hit_points = unit_template.getMaxHitPoints();
            this.path_tracker = new com.oddlabs.tt.pathfinder.PathTracker(getUnitGrid(), this);
            com.oddlabs.tt.model.UnitSupplyContainerFactory factory = unit_template.getUnitSupplyContainerFactory();
            supply_container = (factory != null ? ((com.oddlabs.tt.model.UnitSupplyContainer)factory.createContainer(this)) : (com.oddlabs.tt.model.UnitSupplyContainer)(null));
            foreach (string slot in java.lang.JIter.of(unit_template.getDefaultAttachments()))
            {
                setAttachment(slot, 0);
            }
            if (!imaginary)
            {
                findInitialPosition(x, y, grid_targets_only, (-1));
            }
            pushController(new com.oddlabs.tt.model.behaviour.IdleController(this, new com.oddlabs.tt.model.AttackScanFilter(getOwner(), com.oddlabs.tt.model.AttackScanFilter.UNIT_RANGE), true));
            if (!getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC) && !imaginary)
            {
                int result = getOwner().getUnitCountContainer().increaseSupply(1);
            }
            else if (notify_by_chieftain)
            {
                owner.getWorld().getNotificationListener().newSelectableNotification(this);
            }
            if (rally_point != null)
            {
                com.oddlabs.tt.util.Target unit_target = default;
                if ((rally_point is com.oddlabs.tt.landscape.LandscapeTarget))
                {
                    com.oddlabs.tt.pathfinder.UnitGrid grid = getUnitGrid();
                    java.util.List temp_occupants = new java.util.ArrayList();
                    foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(getOwner().getUnits().getSet()))
                    {
                        if ((s.getCurrentController() is com.oddlabs.tt.model.behaviour.WalkController))
                        {
                            com.oddlabs.tt.util.Target target = (((com.oddlabs.tt.model.behaviour.WalkController)s.getCurrentController())).getTarget();
                            if (!grid.isGridOccupied(target.getGridX(), target.getGridY()))
                            {
                                grid.occupyGrid(target.getGridX(), target.getGridY(), this);
                                temp_occupants.add(target);
                            }
                        }
                    }
                    unit_target = grid.findGridTargets(rally_point.getGridX(), rally_point.getGridY(), 1, true)[0];
                    foreach (com.oddlabs.tt.util.Target target_2 in java.lang.JIter.of(temp_occupants))
                    {
                        grid.freeGrid(target_2.getGridX(), target_2.getGridY(), this);
                    }
                }
                else
                {
                    unit_target = rally_point;
                }
                bool aggressive = unit_template.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.THROW);
                setTarget(unit_target, com.oddlabs.tt.model.Action.DEFAULT, aggressive);
            }
        }
    }
}
