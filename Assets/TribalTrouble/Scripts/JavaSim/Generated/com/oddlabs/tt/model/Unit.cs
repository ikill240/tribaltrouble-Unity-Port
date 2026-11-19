// Translated from Unit.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 54
    public class Unit : com.oddlabs.tt.model.Selectable, com.oddlabs.tt.pathfinder.Occupant, com.oddlabs.tt.pathfinder.Movable
    {
        public const float IDLE_SPEED = 0.4f;
        public const float TRANSPORT_SPEED_SCALE = 0.8f;
        public const int PENALTY_INCREMENT = 3;
        public const int INITIAL_PATH_PENALTY = 5;
        public static float[] MAX_MAGIC_ENERGY;
        // Java: 66
        public float getMagicCost(int index)
        {
            return ((index) switch { 0 => (float)(getOwner().getWorld().getMagic1Cost()), 1 => (float)(getOwner().getWorld().getMagic2Cost()), 2 => (float)(getOwner().getWorld().getMagic3Cost()), _ => (float)(getOwner().getWorld().getExtraMagicCost(index)) });
        }
        // Java: 82
        public class CourageStack : java.lang.Object
        {
            public float time_remaining;
            public float speed_mult;
            public float range_bonus;
            public float attack_speed_mult;
            public float combat_mult;
            public float cooldown_reduction;
            public CourageStack(float duration, float speed_mult, float range_bonus, float attack_speed_mult, float combat_mult, float cooldown_reduction) : base()
            {
                this.time_remaining = duration;
                this.speed_mult = speed_mult;
                this.range_bonus = range_bonus;
                this.attack_speed_mult = attack_speed_mult;
                this.combat_mult = combat_mult;
                this.cooldown_reduction = cooldown_reduction;
            }
        }
        public java.util.List courage_stacks;
        public float courage_time_remaining;
        public float courage_speed_mult;
        public float courage_range_bonus;
        public float courage_attack_speed_mult;
        public float courage_combat_mult;
        public float courage_cooldown_reduction;
        public static float[] COURAGE_DURATION;
        public const float COURAGE_SPEED_MULT = 2.5f;
        public const float COURAGE_RANGE_BONUS = 5.0f;
        public const float COURAGE_ATTACK_SPEED_MULT = 2.5f;
        public const float COURAGE_COMBAT_MULT = 1.75f;
        public const float COOLDOWN_REDUCTION = 0.03f;
        public const float COURAGE_RADIUS = 80.0f;
        public float stuck_check_x;
        public float stuck_check_y;
        public float stuck_time;
        public com.oddlabs.tt.model.Supply avoided_supply;
        // Java: 144
        public virtual com.oddlabs.tt.model.Supply getAvoidedSupply()
        {
            return avoided_supply;
        }
        // Java: 148
        public virtual void setAvoidedSupply(com.oddlabs.tt.model.Supply supply)
        {
            this.avoided_supply = supply;
        }
        public const float GATHER_STUCK_DIST_SQ = 4.0f;
        public const float GATHER_STUCK_SECONDS = 3.0f;
        public const float GENERAL_STUCK_SECONDS = 5.0f;
        // Java: 161
        public class Animation : java.lang.Object
        {
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
            public Animation() : base()
            {
            }
        }
        public const int SPEAR_RELEASE_FRAME = 29;
        public com.oddlabs.tt.model.UnitSupplyContainer supply_container;
        public string name;
        public com.oddlabs.tt.pathfinder.PathTracker path_tracker;
        public float[] magic_energy;
        public int magic_race_override_index;
        public int last_magic_index;
        public com.oddlabs.tt.particle.BalancedParametricEmitter stun_marker;
        public int hit_points;
        public float[] nt_row;
        public bool nt_known;
        public int shaman_kind;
        public com.oddlabs.tt.model.ShamanAura shaman_aura;
        public com.oddlabs.tt.model.Afflictions afflictions_f;
        public float remedy_timer;
        public float time_since_damage;
        public float chieftain_heal_timer;
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
        // Java: 253
        public float[] newTidesRow()
        {
            if (!nt_known)
            {
                nt_known = true;
                if (getOwner().getWorld().isExtended() && getOwner().getWorld().getRacesResources() != null)
                {
                    nt_row = com.oddlabs.tt.model.NewTidesStats.lookup(getOwner().getWorld().getRacesResources(), ((com.oddlabs.tt.model.UnitTemplate)getTemplate()));
                }
            }
            return nt_row;
        }
        public com.oddlabs.tt.player.Player killed_by;
        // Java: 265
        public com.oddlabs.tt.player.Player getKilledBy()
        {
            return killed_by;
        }
        // Java: 269
        public int getEffectiveMaxHitPoints()
        {
            int @base = ((com.oddlabs.tt.model.UnitTemplate)getTemplate()).getMaxHitPoints();
            if (!getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC))
            {
                bool ext = getOwner().getWorld().isExtended();
                float[] row = (ext ? newTidesRow() : (float[])(null));
                if (row != null && row[com.oddlabs.tt.model.NewTidesStats.HEALTH] > 0.0f)
                {
                    int health = java.lang.Math.round(row[com.oddlabs.tt.model.NewTidesStats.HEALTH]);
                    if (god_kind > 0)
                    {
                        health += (com.oddlabs.tt.model.GodPowers.bonusHitPoints(this, god_kind) / 4) * 4;
                    }
                    if (getEliteKind() == com.oddlabs.tt.model.ElitePowers.KNIGHT)
                    {
                        return com.oddlabs.tt.model.ElitePowers.hitPoints(this, health + getOwner().getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.ARMOR));
                    }
                    return health + ((getAbilities().isWarrior() ? getOwner().getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.ARMOR) : (getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.HARVEST) ? getOwner().getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.STURDY) : 0)));
                }
                if (ext && god_kind > 0)
                {
                    @base = @base + com.oddlabs.tt.model.GodPowers.bonusHitPoints(this, god_kind) / 4;
                }
                if (ext && getEliteKind() == com.oddlabs.tt.model.ElitePowers.KNIGHT)
                {
                    return com.oddlabs.tt.model.ElitePowers.hitPoints(this, @base * 4 + getOwner().getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.ARMOR));
                }
                return (getAbilities().isWarrior() ? @base * ((ext ? 4 : 1)) + getOwner().getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.ARMOR) : @base * ((ext ? 2 : 1)) + ((getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.HARVEST) ? getOwner().getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.STURDY) : 0)));
            }
            int race_index = (magic_race_override_index >= 0 ? magic_race_override_index : getOwner().getPlayerInfo().getRace());
            int vigor = getOwner().getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.VIGOR);
            if (vigor > 0)
            {
                return java.lang.Math.round((float)((float)(@base * getOwner().getWorld().getChiefHealthMultiplier(race_index)) * ((float)(1.0f + (float)(0.25f * vigor)))));
            }
            return java.lang.Math.round((float)(@base * getOwner().getWorld().getChiefHealthMultiplier(race_index)));
        }
        // Java: 361
        public override com.oddlabs.tt.model.Element self()
        {
            return this;
        }
        // Java: 366
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
        // Java: 375
        public override void visit(com.oddlabs.tt.model.ElementVisitor visitor)
        {
            visitor.visitUnit(this);
        }
        // Java: 380
        public com.oddlabs.tt.model.UnitSupplyContainer getSupplyContainer()
        {
            return supply_container;
        }
        // Java: 384
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
        // Java: 406
        public void updateStuckCheck(float t)
        {
            if (isDead() || !isMoving())
            {
                stuck_time = 0.0f;
                stuck_check_x = java.lang.Float.NaN;
                return;
            }
            if (getOwner().usesReferenceCommandRules() && !((getPrimaryController() is com.oddlabs.tt.model.behaviour.GatherController)))
            {
                if (getOwner().getWorld().isExtended() && arrivedInCrowd(t))
                {
                    return;
                }
                stuck_time = 0.0f;
                stuck_check_x = java.lang.Float.NaN;
                return;
            }
            if (!(isMoving() && getPrimaryController() != null))
            {
                stuck_time = 0.0f;
                stuck_check_x = java.lang.Float.NaN;
                return;
            }
            float x = getPositionX();
            float y = getPositionY();
            if (java.lang.Float.isNaN(stuck_check_x))
            {
                stuck_check_x = x;
                stuck_check_y = y;
                return;
            }
            float dx = (float)(x - stuck_check_x);
            float dy = (float)(y - stuck_check_y);
            if ((float)((float)(dx * dx) + (float)(dy * dy)) < com.oddlabs.tt.model.Unit.GATHER_STUCK_DIST_SQ)
            {
                stuck_time += t;
                bool is_gathering = (getPrimaryController() is com.oddlabs.tt.model.behaviour.GatherController);
                float threshold = (is_gathering ? com.oddlabs.tt.model.Unit.GATHER_STUCK_SECONDS : com.oddlabs.tt.model.Unit.GENERAL_STUCK_SECONDS);
                if (stuck_time > threshold)
                {
                    if (!getPrimaryController().onStuck())
                    {
                        popController();
                    }
                    stuck_time = 0.0f;
                    stuck_check_x = x;
                    stuck_check_y = y;
                }
            }
            else
            {
                stuck_check_x = x;
                stuck_check_y = y;
                stuck_time = 0.0f;
            }
        }
        // Java: 472
        public bool arrivedInCrowd(float t)
        {
            if (!((getPrimaryController() is com.oddlabs.tt.model.behaviour.WalkController walk)))
            {
                return false;
            }
            float dx = (float)(walk.getTarget().getPositionX() - getPositionX());
            float dy = (float)(walk.getTarget().getPositionY() - getPositionY());
            if ((float)((float)(dx * dx) + (float)(dy * dy)) > (float)(com.oddlabs.tt.model.Unit.CROWD_ARRIVAL_DIST * com.oddlabs.tt.model.Unit.CROWD_ARRIVAL_DIST))
            {
                return false;
            }
            float x = getPositionX();
            float y = getPositionY();
            if (java.lang.Float.isNaN(stuck_check_x))
            {
                stuck_check_x = x;
                stuck_check_y = y;
                stuck_time = 0.0f;
                return true;
            }
            float mx = (float)(x - stuck_check_x);
            float my = (float)(y - stuck_check_y);
            if ((float)((float)(mx * mx) + (float)(my * my)) >= com.oddlabs.tt.model.Unit.GATHER_STUCK_DIST_SQ)
            {
                stuck_check_x = x;
                stuck_check_y = y;
                stuck_time = 0.0f;
                return true;
            }
            stuck_time += t;
            if (stuck_time > com.oddlabs.tt.model.Unit.CROWD_ARRIVAL_SECONDS)
            {
                stuck_time = 0.0f;
                stuck_check_x = java.lang.Float.NaN;
                popController();
            }
            return true;
        }
        public const float CROWD_ARRIVAL_DIST = 10.0f;
        public const float CROWD_ARRIVAL_SECONDS = 3.0f;
        // Java: 504
        public virtual void reposition()
        {
            findInitialPosition(getPositionX(), getPositionY(), true);
        }
        // Java: 508
        public void findInitialPosition(float x, float y, bool grid_targets_only)
        {
            com.oddlabs.tt.pathfinder.UnitGrid unit_grid = getUnitGrid();
            com.oddlabs.tt.util.Target reserved_target = unit_grid.findGridTargets(com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(x), com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(y), 1, grid_targets_only)[0];
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
        // Java: 528
        public override int getStatusValue()
        {
            int tower_factor = 1;
            if (mounted)
            {
                tower_factor = 3;
            }
            return ((com.oddlabs.tt.model.UnitTemplate)getTemplate()).getStatusValue() * tower_factor;
        }
        // Java: 536
        public void increaseRange(float amount)
        {
            range_bonus += amount;
        }
        // Java: 541
        public override com.oddlabs.tt.model.AttackScanFilter.Priority getAttackPriority()
        {
            return (getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD) ? com.oddlabs.tt.model.AttackScanFilter.Priority.PEON : com.oddlabs.tt.model.AttackScanFilter.Priority.WARRIOR);
        }
        // Java: 548
        public override void visit(com.oddlabs.tt.model.ToolTipVisitor visitor)
        {
            visitor.visitUnit(this);
        }
        public int chief_serial;
        // Java: 555
        public string getName()
        {
            if (name == null && chief_serial >= 0 && getOwner().getWorld().isExtended())
            {
                return com.oddlabs.tt.player.ChiefNames.of(getOwner(), chief_serial);
            }
            return name;
        }
        // Java: 561
        public int getHitPoints()
        {
            return hit_points;
        }
        // Java: 565
        public float getCourageAttackSpeedMultiplier()
        {
            float volley = (mounted && mounted_building != null && ((com.oddlabs.tt.model.BuildingTemplate)mounted_building.getTemplate()).getTemplateID() == com.oddlabs.tt.model.Race.BUILDING_TOWER ? (float)(1.0f + (float)(0.2f * getOwner().getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.VOLLEY))) : 1.0f);
            return (float)((float)((float)(courage_attack_speed_mult * getOwner().getUpgradeAttackSpeedMultiplier()) * volley) * ((drum_left > 0.0f ? com.oddlabs.tt.model.ElitePowers.attackSpeed(this) : 1.0f)));
        }
        // Java: 573
        public com.oddlabs.tt.model.Afflictions afflictions()
        {
            if (afflictions_f == null)
            {
                afflictions_f = new com.oddlabs.tt.model.Afflictions();
            }
            return afflictions_f;
        }
        // Java: 579
        public bool hasAfflictions()
        {
            return afflictions_f != null;
        }
        // Java: 583
        public bool isPoisoned()
        {
            return afflictions_f != null && afflictions_f.isPoisoned();
        }
        // Java: 587
        public bool isBurning()
        {
            return afflictions_f != null && afflictions_f.isBurning();
        }
        // Java: 591
        public bool isSlowed()
        {
            return afflictions_f != null && afflictions_f.isSlowed();
        }
        // Java: 595
        public float getCourageCombatMultiplier()
        {
            return courage_combat_mult;
        }
        // Java: 599
        public void applyCourageBuff(float duration, float speed_mult, float extra_range, float attack_speed_mult, float combat_mult, float cooldown_reduction)
        {
            if (isDead())
            {
                return;
            }
            if (getOwner().getWorld().isExtended())
            {
                int cap = com.oddlabs.tt.model.Unit.courageCap(getOwner());
                while (courage_stacks.size() >= cap)
                {
                    com.oddlabs.tt.model.Unit.CourageStack oldest = ((com.oddlabs.tt.model.Unit.CourageStack)courage_stacks.getFirst());
                    foreach (com.oddlabs.tt.model.Unit.CourageStack c in java.lang.JIter.of(courage_stacks))
                    {
                        if (c.time_remaining < oldest.time_remaining)
                        {
                            oldest = c;
                        }
                    }
                    courage_stacks.remove(oldest);
                }
            }
            courage_stacks.add(new com.oddlabs.tt.model.Unit.CourageStack(duration, speed_mult, extra_range, attack_speed_mult, combat_mult, cooldown_reduction));
            recomputeCourageTotals();
        }
        // Java: 629
        public static int courageCap(com.oddlabs.tt.player.Player p)
        {
            if (((object)p.getWorld().getGameMode() == (object)com.oddlabs.matchmaking.GameMode.MAGIC_RAMPAGE))
            {
                return 15;
            }
            return ((p.getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.COURAGE_MASTERY)) switch { 0 => (int)(10), 1 => (int)(12), _ => (int)(15) });
        }
        // Java: 639
        public int getCourageStacks()
        {
            return courage_stacks.size();
        }
        // Java: 643
        public void recomputeCourageTotals()
        {
            float longest_remaining = 0.0f;
            float speed_bonus_sum = 0.0f;
            float range_bonus_sum = 0.0f;
            float attack_speed_bonus_sum = 0.0f;
            float combat_bonus_sum = 0.0f;
            float cooldown_reduction_sum = 0.0f;
            foreach (com.oddlabs.tt.model.Unit.CourageStack stack in java.lang.JIter.of(courage_stacks))
            {
                if (stack.time_remaining > longest_remaining)
                {
                    longest_remaining = stack.time_remaining;
                }
                speed_bonus_sum = (float)(speed_bonus_sum + (float)(stack.speed_mult - 1.0f));
                range_bonus_sum = (float)(range_bonus_sum + stack.range_bonus);
                attack_speed_bonus_sum = (float)(attack_speed_bonus_sum + (float)(stack.attack_speed_mult - 1.0f));
                combat_bonus_sum = (float)(combat_bonus_sum + (float)(stack.combat_mult - 1.0f));
                cooldown_reduction_sum = (float)(cooldown_reduction_sum + stack.cooldown_reduction);
            }
            courage_time_remaining = longest_remaining;
            range_bonus -= courage_range_bonus;
            courage_range_bonus = range_bonus_sum;
            range_bonus += courage_range_bonus;
            courage_speed_mult = (float)(1.0f + speed_bonus_sum);
            courage_attack_speed_mult = (float)(1.0f + attack_speed_bonus_sum);
            courage_combat_mult = (float)(1.0f + combat_bonus_sum);
            courage_cooldown_reduction = cooldown_reduction_sum;
        }
        // Java: 669
        public float getTimeSinceDamage()
        {
            return time_since_damage;
        }
        // Java: 673
        public void heal(int amount)
        {
            if (isDead())
            {
                return;
            }
            hit_points = java.lang.Math.min(hit_points + amount, getEffectiveMaxHitPoints());
        }
        // Java: 679
        public void drown()
        {
            clearOrderQueue();
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
        // Java: 697
        public void unmount()
        {
            clearControllerStack();
            swapController(new com.oddlabs.tt.model.behaviour.IdleController(this, new com.oddlabs.tt.model.AttackScanFilter(getOwner(), com.oddlabs.tt.model.AttackScanFilter.UNIT_RANGE), true));
            mounted = false;
            on_ship = false;
            mount_offset = 0;
            enable();
            com.oddlabs.tt.model.Building entrance = mounted_building.getEntrance();
            findInitialPosition(entrance.getPositionX(), entrance.getPositionY(), true);
            if (supply_container != null)
            {
                supply_container.resetSupply(typeof(com.oddlabs.tt.model.LeftPaddle));
                supply_container.resetSupply(typeof(com.oddlabs.tt.model.RightPaddle));
            }
            mounted_building = null;
        }
        // Java: 714
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
            clearOrderQueue();
            clearControllerStack();
            swapController(new com.oddlabs.tt.model.behaviour.IdleController(this, new com.oddlabs.tt.model.TowerAttackScanFilter(getOwner(), com.oddlabs.tt.model.AttackScanFilter.TOWER_RANGE), false));
        }
        // Java: 740
        public void standGround()
        {
            if (mounted || !getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.ATTACK))
            {
                return;
            }
            clearOrderQueue();
            clearControllerStack();
            swapController(new com.oddlabs.tt.model.behaviour.IdleController(this, new com.oddlabs.tt.model.AttackScanFilter(getOwner(), com.oddlabs.tt.model.AttackScanFilter.UNIT_RANGE), false));
        }
        // Java: 749
        public void mount(com.oddlabs.tt.model.Ship ship, com.oddlabs.tt.model.ShipAllocation ship_allocation)
        {
            mounted_building = ship;
            mount_offset = ship_allocation.getOffset().z_f;
            if (!imaginary)
            {
                disable();
                free();
            }
            mounted = true;
            on_ship = true;
            setReference(ship);
            clearOrderQueue();
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
        public int elite_kind;
        public int elite_xp;
        public float elite_timer;
        public int elite_beats;
        public float drum_left;
        public float tower_check;
        public float target_check;
        public float retarget_hold;
        // Java: 788
        public float retargetHold()
        {
            return retarget_hold;
        }
        // Java: 792
        public void setRetargetHold(float seconds)
        {
            retarget_hold = seconds;
        }
        public int drum_rank;
        public int fury_hits;
        // Java: 799
        public int getEliteKind()
        {
            if (elite_kind < 0)
            {
                elite_kind = (getOwner().getWorld().isExtended() ? com.oddlabs.tt.model.ElitePowers.kind(this) : com.oddlabs.tt.model.ElitePowers.NONE);
            }
            return elite_kind;
        }
        // Java: 805
        public int getEliteXp()
        {
            return elite_xp;
        }
        // Java: 810
        public void setEliteXp(int xp)
        {
            elite_xp = java.lang.Math.max(0, xp);
        }
        // Java: 814
        public void addEliteXp(int xp)
        {
            elite_xp += xp;
        }
        // Java: 819
        public bool tickEliteTimer(float t, float every)
        {
            elite_timer += t;
            if (elite_timer < every)
            {
                return false;
            }
            elite_timer -= every;
            return true;
        }
        // Java: 827
        public int nextBeat()
        {
            return elite_beats++;
        }
        // Java: 832
        public void drum(float seconds, int rank)
        {
            drum_rank = (drum_left > 0.0f ? java.lang.Math.max(drum_rank, rank) : rank);
            drum_left = java.lang.Math.max(drum_left, seconds);
        }
        // Java: 837
        public bool isDrummed()
        {
            return drum_left > 0.0f;
        }
        // Java: 841
        public int getDrumRank()
        {
            return drum_rank;
        }
        // Java: 845
        public int nextFuryHit()
        {
            return fury_hits++;
        }
        public int god_kind;
        public int god_kills;
        public float god_power_timer;
        public float god_weather_timer;
        // Java: 856
        public int getGodKind()
        {
            if (god_kind < 0)
            {
                god_kind = (getOwner().getWorld().isExtended() ? com.oddlabs.tt.model.GodPowers.kind(this) : com.oddlabs.tt.model.GodPowers.NONE);
            }
            return god_kind;
        }
        // Java: 862
        public int getGodKills()
        {
            return god_kills;
        }
        // Java: 866
        public void addGodKill()
        {
            god_kills++;
        }
        // Java: 870
        public float tickGodPower(float t)
        {
            return god_power_timer += t;
        }
        // Java: 874
        public void resetGodPower()
        {
            god_power_timer = 0.0f;
        }
        // Java: 878
        public float tickGodWeather(float t)
        {
            return god_weather_timer += t;
        }
        // Java: 882
        public void resetGodWeather()
        {
            god_weather_timer = 0.0f;
        }
        public int harvest_hits;
        // Java: 889
        public int nextHarvestHit()
        {
            return ++harvest_hits;
        }
        // Java: 893
        public void resetHarvestHits()
        {
            harvest_hits = 0;
        }
        public bool objective_hold;
        // Java: 900
        public bool isObjectiveHold()
        {
            return objective_hold;
        }
        // Java: 904
        public void setObjectiveHold(bool hold)
        {
            objective_hold = hold;
        }
        // Java: 908
        public bool isMounted()
        {
            return mounted;
        }
        // Java: 918
        public void initGather(System.Type supply_type, com.oddlabs.tt.model.Building drop_off)
        {
            if (mounted)
            {
                return;
            }
            clearOrderQueue();
            clearControllerStack();
            if (drop_off != null && !drop_off.isDead())
            {
                pushController(new com.oddlabs.tt.model.behaviour.GatherController(this, null, supply_type, drop_off));
            }
            else
            {
                pushController(new com.oddlabs.tt.model.behaviour.GatherController(this, null, supply_type));
            }
        }
        // Java: 930
        public override bool isEnabled()
        {
            return !isDead() && !mounted;
        }
        // Java: 935
        public float getMetersPerSecond()
        {
            float @base = default;
            float[] row = newTidesRow();
            float own = (row != null && row[com.oddlabs.tt.model.NewTidesStats.SPEED] > 0.0f ? row[com.oddlabs.tt.model.NewTidesStats.SPEED] : ((com.oddlabs.tt.model.UnitTemplate)getTemplate()).getMetersPerSecond());
            if (getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.HARVEST) && supply_container.getNumSupplies() > 0)
            {
                @base = (float)(com.oddlabs.tt.model.Unit.TRANSPORT_SPEED_SCALE * own);
            }
            else
            {
                @base = own;
            }
            float slowed = (afflictions_f != null ? afflictions_f.speedMultiplier() : 1.0f);
            float baskets = (getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.HARVEST) ? (float)(1.0f + (float)(0.1f * getOwner().getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.BASKETS))) : 1.0f);
            com.oddlabs.tt.landscape.Weather weather = getOwner().getWorld().getWeather();
            float climate = (weather != null ? weather.getSpeedFactor() : 1.0f);
            return (float)((float)((float)((float)((float)(@base * courage_speed_mult) * getOwner().getUpgradeSpeedMultiplier()) * slowed) * baskets) * climate);
        }
        // Java: 951
        public void aimAtTarget(com.oddlabs.tt.util.Target target)
        {
            float dx = (float)(target.getPositionX() - getPositionX());
            float dy = (float)(target.getPositionY() - getPositionY());
            float dir_len_inv = (float)(1.0f / ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy))))));
            dx = (float)(dx * dir_len_inv);
            dy = (float)(dy * dir_len_inv);
            setDirection(dx, dy);
        }
        // Java: 961
        public void switchToIdleAnimation()
        {
            switchAnimation(com.oddlabs.tt.model.Unit.IDLE_SPEED, com.oddlabs.tt.model.Unit.Animation.IDLING);
        }
        // Java: 966
        public void switchToSittingAnimation()
        {
            switchAnimation(com.oddlabs.tt.model.Unit.IDLE_SPEED, com.oddlabs.tt.model.Unit.Animation.SITTING);
        }
        // Java: 971
        public void switchToSteeringAnimation()
        {
            switchAnimation(com.oddlabs.tt.model.Unit.IDLE_SPEED, com.oddlabs.tt.model.Unit.Animation.STEERING);
        }
        // Java: 976
        public void switchToRowingRightAnimation()
        {
            switchAnimation(com.oddlabs.tt.model.Unit.IDLE_SPEED, com.oddlabs.tt.model.Unit.Animation.ROWING_RIGHT);
            if (supply_container != null)
            {
                supply_container.increaseSupply(1, typeof(com.oddlabs.tt.model.RightPaddle));
            }
        }
        // Java: 984
        public void switchToRowingLeftAnimation()
        {
            switchAnimation(com.oddlabs.tt.model.Unit.IDLE_SPEED, com.oddlabs.tt.model.Unit.Animation.ROWING_LEFT);
            if (supply_container != null)
            {
                supply_container.increaseSupply(1, typeof(com.oddlabs.tt.model.LeftPaddle));
            }
        }
        // Java: 992
        public com.oddlabs.tt.model.weapon.WeaponFactory getWeaponFactory()
        {
            return ((com.oddlabs.tt.model.UnitTemplate)getTemplate()).getWeaponFactory();
        }
        // Java: 997
        public float getRange(com.oddlabs.tt.util.Target target)
        {
            float[] row = newTidesRow();
            float reach = (row != null && row[com.oddlabs.tt.model.NewTidesStats.REACH] > 0.0f && getWeaponFactory().getRange() > 1.0f ? row[com.oddlabs.tt.model.NewTidesStats.REACH] : getWeaponFactory().getRange());
            return (float)((float)(((float)((float)((float)((float)((float)(reach + range_bonus) + ((getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.THROW) ? getOwner().getUpgradeRangeBonus() : 0.0f))) + ((elite_kind > 0 ? com.oddlabs.tt.model.ElitePowers.rangeBonus(this) : 0.0f))) + ((mounted && mounted_building != null && ((com.oddlabs.tt.model.BuildingTemplate)mounted_building.getTemplate()).getTemplateID() == com.oddlabs.tt.model.Race.BUILDING_TOWER ? getOwner().getUpgradeTowerRangeBonus() : 0.0f))) + ((mounted && (mounted_building is com.oddlabs.tt.model.LandBuilding tier_building) ? tier_building.getTierRangeBonus() : 0.0f)))) * getOwner().getWorld().getUnitRangeMultiplier()) + target.getSize());
        }
        // Java: 1012
        public override float getSize()
        {
            return 1.9f;
        }
        // Java: 1017
        public override com.oddlabs.tt.render.SpriteKey getSpriteRenderer()
        {
            return ((com.oddlabs.tt.model.UnitTemplate)getTemplate()).getSpriteRenderer();
        }
        // Java: 1022
        public override void doAnimate(float t)
        {
            time_since_damage += t;
            updateStuckCheck(t);
            anim_time += (float)(anim_speed * t);
            if (isDead() || mounted)
            {
                reinsert();
            }
            getOwner().getWorld().updateGlobalChecksum(animation);
            if (shaman_kind >= 0 && !isDead() && getOwner().getWorld().isExtended())
            {
                if (shaman_kind == 0)
                {
                    shaman_kind = (-1);
                    com.oddlabs.tt.model.RacesResources rr = getOwner().getWorld().getRacesResources();
                    for (int r = 0; r < com.oddlabs.tt.model.RacesResources.getNumRaces(); r++)
                    {
                        if (((object)rr.getRace(r).getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_SHAMAN) == (object)((com.oddlabs.tt.model.UnitTemplate)getTemplate())))
                        {
                            shaman_kind = (r == com.oddlabs.tt.model.RacesResources.RACE_VIKINGS ? 2 : 1);
                        }
                    }
                    if (shaman_kind > 0)
                    {
                        shaman_aura = new com.oddlabs.tt.model.ShamanAura(this, shaman_kind == 2);
                    }
                }
                if (shaman_aura != null)
                {
                    shaman_aura.animate(t);
                }
            }
            if (afflictions_f != null && !isDead())
            {
                afflictions_f.animate(t, this);
            }
            if (!isDead() && getOwner().getWorld().isExtended() && getGodKind() != com.oddlabs.tt.model.GodPowers.NONE)
            {
                com.oddlabs.tt.model.GodPowers.animate(this, getGodKind(), t);
            }
            if (drum_left > 0.0f)
            {
                drum_left -= t;
            }
            if (retarget_hold > 0.0f)
            {
                retarget_hold -= t;
            }
            if (!isDead() && getOwner().getWorld().isExtended() && isWarrior() && (target_check += t) >= 1.0f)
            {
                target_check = 0.0f;
                com.oddlabs.tt.model.CounterTargeting.look(this);
            }
            if (!isDead() && (getCurrentController() is com.oddlabs.tt.model.behaviour.EnterController enter) && getOwner().getWorld().isExtended() && (tower_check += t) >= 0.5f)
            {
                tower_check = 0.0f;
                enter.towerTaken();
            }
            if (!isDead() && getOwner().getWorld().isExtended() && getEliteKind() != com.oddlabs.tt.model.ElitePowers.NONE)
            {
                com.oddlabs.tt.model.ElitePowers.animate(this, t);
            }
            if (!isDead() && getOwner().getWorld().isExtended() && getOwner().getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.RACE_2) > 0 && !com.oddlabs.tt.model.ExtendedCombat.isVikings(getOwner()))
            {
                remedy_timer += t;
                float every = (getOwner().getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.RACE_2) >= 2 ? 5.0f : 8.0f);
                if (remedy_timer >= every)
                {
                    remedy_timer -= every;
                    heal(1);
                }
            }
            if (getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC))
            {
                float regen_amount = (float)((float)(t * ((float)(1.0f + courage_cooldown_reduction))) * getOwner().getUpgradeLoreMultiplier());
                for (int i = 0; i < magic_energy.Length; i++)
                {
                    increaseMagicEnergy(i, regen_amount);
                }
                if (getTimeSinceDamage() < getOwner().getWorld().getChieftainHealIdleSeconds() || getHitPoints() >= getEffectiveMaxHitPoints())
                {
                    chieftain_heal_timer = (float)(-1.0f);
                }
                else if (chieftain_heal_timer < 0.0f)
                {
                    heal(getOwner().getWorld().getChieftainHealAmount());
                    chieftain_heal_timer = 0.0f;
                }
                else
                {
                    chieftain_heal_timer += t;
                    if (chieftain_heal_timer >= getOwner().getWorld().getChieftainHealIdleSeconds())
                    {
                        heal(getOwner().getWorld().getChieftainHealAmount());
                        chieftain_heal_timer = 0.0f;
                    }
                }
            }
            if (!courage_stacks.isEmpty())
            {
                bool any_expired = false;
                java.util.Iterator iterator = courage_stacks.iterator();
                while (iterator.hasNext())
                {
                    com.oddlabs.tt.model.Unit.CourageStack stack = ((com.oddlabs.tt.model.Unit.CourageStack)iterator.next());
                    stack.time_remaining -= t;
                    if (stack.time_remaining <= 0.0f)
                    {
                        iterator.remove();
                        any_expired = true;
                    }
                }
                if (any_expired)
                {
                    recomputeCourageTotals();
                }
            }
        }
        // Java: 1127
        public void increaseMagicEnergy(int index, float amount)
        {
            magic_energy[index] += amount;
            if (magic_energy[index] > getMagicCost(index))
            {
                magic_energy[index] = getMagicCost(index);
            }
        }
        // Java: 1134
        public com.oddlabs.tt.pathfinder.PathTracker getTracker()
        {
            return path_tracker;
        }
        // Java: 1140
        public void markBlocking()
        {
            path_penalty = java.lang.Math.min(path_penalty + com.oddlabs.tt.model.Unit.PENALTY_INCREMENT, com.oddlabs.tt.pathfinder.Occupant.STATIC - 1);
        }
        // Java: 1146
        public int getPenalty()
        {
            return (isBlocking() ? com.oddlabs.tt.pathfinder.Occupant.STATIC : path_penalty);
        }
        // Java: 1152
        public override void removeDying()
        {
            if (getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC) && ((object)getOwner().getChieftain() == (object)this))
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
        // Java: 1179
        public void removeNow()
        {
            removeDying();
            remove();
        }
        // Java: 1185
        public void free()
        {
            com.oddlabs.tt.pathfinder.UnitGrid unit_grid = getUnitGrid();
            if (((object)unit_grid.getOccupant(getGridX(), getGridY()) == (object)this))
            {
                unit_grid.freeGrid(getGridX(), getGridY(), this);
            }
            path_penalty = com.oddlabs.tt.model.Unit.INITIAL_PATH_PENALTY;
        }
        // Java: 1195
        public void occupy()
        {
            com.oddlabs.tt.pathfinder.UnitGrid unit_grid = getUnitGrid();
            unit_grid.occupyGrid(getGridX(), getGridY(), this);
            getOwner().unitMoved();
        }
        // Java: 1205
        public bool isMoving()
        {
            return (getCurrentBehaviour() is com.oddlabs.tt.model.behaviour.WalkBehaviour);
        }
        // Java: 1215
        public override void hit(int damage, float direction_x, float direction_y, com.oddlabs.tt.player.Player owner)
        {
            if (damage > 0)
            {
                time_since_damage = 0.0f;
            }
            base.hit(damage, direction_x, direction_y, owner);
            if (mounted && !on_ship)
            {
                mounted_building.hit(damage, direction_x, direction_y, owner);
            }
            else if (!isDead())
            {
                hit_points = java.lang.Math.clamp(hit_points - damage, 0, getEffectiveMaxHitPoints());
                if (hit_points == 0)
                {
                    killed_by = owner;
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
                        getOwner().getWorld().unitFell(this);
                    }
                }
            }
        }
        // Java: 1238
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
        // Java: 1261
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
        // Java: 1274
        public bool isWarrior()
        {
            System.Type type = getWeaponFactory().getType();
            if (((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)) || ((object)type == (object)typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)) || ((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)) || ((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RockSpearWeapon)) || ((object)type == (object)typeof(com.oddlabs.tt.model.weapon.IronSpearWeapon)) || ((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RubberSpearWeapon)))
            {
                return true;
            }
            return false;
        }
        // Java: 1287
        public com.oddlabs.tt.particle.BalancedParametricEmitter createStunStar(float x, float y, float z, float time, float velocity)
        {
            int num_particles = 5;
            return new com.oddlabs.tt.particle.BalancedParametricEmitter(getOwner().getWorld(), new com.oddlabs.tt.particle.StunFunction(0.4f, 0.15f), new org.joml.Vector3f(x, y, z), velocity, 5.0f, (float)(((float)(java.lang.Math.PI)) * 2), (float)(((float)(java.lang.Math.PI)) * 2), num_particles, 0.0f, 2.0f, new org.joml.Vector4f(1.0f, 1.0f, 1.0f, 1.0f), new org.joml.Vector4f(0.0f, 0.0f, 0.0f, 0.0f), new org.joml.Vector3f(0.1f, 0.1f, 0.1f), new org.joml.Vector3f(0.0f, 0.0f, 0.0f), time, org.lwjgl.opengl.GL11.GL_SRC_ALPHA, org.lwjgl.opengl.GL11.GL_ONE_MINUS_SRC_ALPHA, getOwner().getWorld().getRacesResources().getStarTextures(), getOwner().getWorld().getAnimationManagerGameTime());
        }
        // Java: 1300
        public bool canAttack(com.oddlabs.tt.util.Target target, bool kill_friendly)
        {
            if (!((target is com.oddlabs.tt.model.Selectable selectable)) || !getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.ATTACK))
            {
                return false;
            }
            com.oddlabs.tt.player.Player target_player = selectable.getOwner();
            return kill_friendly || getOwner().isEnemy(target_player);
        }
        // Java: 1308
        public bool canBuild(com.oddlabs.tt.util.Target target)
        {
            return (target is com.oddlabs.tt.model.Building building) && getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD) && !building.isPlaced();
        }
        // Java: 1314
        public bool canGather(com.oddlabs.tt.util.Target target)
        {
            return (target is com.oddlabs.tt.model.Supply) && getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD);
        }
        // Java: 1318
        public com.oddlabs.tt.model.Building nearestSupplyBuilding(com.oddlabs.tt.model.Supply supply)
        {
            com.oddlabs.tt.pathfinder.UnitGrid grid = getUnitGrid();
            com.oddlabs.tt.model.BuildingFinder finder = new com.oddlabs.tt.model.BuildingFinder(getOwner(), com.oddlabs.tt.model.Abilities.SUPPLY_CONTAINER);
            com.oddlabs.tt.pathfinder.Region region = com.oddlabs.tt.pathfinder.PathFinder.findPathRegion(grid, new com.oddlabs.tt.pathfinder.TargetRegionFinder(grid, finder), grid.getRegion(supply.getGridX(), supply.getGridY()));
            com.oddlabs.tt.model.Building building = (region != null ? ((com.oddlabs.tt.model.Building)finder.getOccupantFromRegion(region, true)) : (com.oddlabs.tt.model.Building)(null));
            return (building != null ? building.getBase() : (com.oddlabs.tt.model.Building)(null));
        }
        // Java: 1327
        public bool canRepair(com.oddlabs.tt.util.Target target, bool action_repair)
        {
            return (target is com.oddlabs.tt.model.Building building) && getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD) && (action_repair || !building.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.SUPPLY_CONTAINER) || !building.isComplete()) && !getOwner().isEnemy(building.getOwner()) && building.isPlaced() && building.isDamaged();
        }
        // Java: 1336
        public virtual bool canEnter(com.oddlabs.tt.util.Target target)
        {
            return (target is com.oddlabs.tt.model.Building building) && (!getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC) || ((building is com.oddlabs.tt.model.Ship) || (building is com.oddlabs.tt.model.ShipProxy)) && getOwner().getWorld().isExtended()) && building.getUnitContainer() != null && (((object)getOwner() == (object)building.getOwner()) || (!getOwner().isEnemy(building.getOwner()) && (((com.oddlabs.tt.model.BuildingTemplate)building.getTemplate()).getTemplateID() == com.oddlabs.tt.model.Race.BUILDING_TOWER || ((com.oddlabs.tt.model.BuildingTemplate)building.getTemplate()).getTemplateID() == com.oddlabs.tt.model.Race.BUILDING_QUARTERS || ((com.oddlabs.tt.model.BuildingTemplate)building.getTemplate()).getTemplateID() == com.oddlabs.tt.model.Race.BUILDING_ARMORY))) && building.getUnitContainer().canEnter(this);
        }
        // Java: 1368
        public override float getDefenseChance()
        {
            if ((getCurrentController() is com.oddlabs.tt.model.behaviour.StunController))
            {
                return 0;
            }
            float[] row = newTidesRow();
            return (row != null ? row[com.oddlabs.tt.model.NewTidesStats.DODGE] : base.getDefenseChance());
        }
        // Java: 1376
        public void walkToTarget(com.oddlabs.tt.util.Target target, bool scan_attack)
        {
            com.oddlabs.tt.util.Target walkable_target = getUnitGrid().findGridTargets(target.getGridX(), target.getGridY(), 1, false)[0];
            pushController(new com.oddlabs.tt.model.behaviour.WalkController(this, (walkable_target != null ? walkable_target : target), scan_attack));
        }
        // Java: 1390
        public com.oddlabs.tt.model.behaviour.HuntController focus(com.oddlabs.tt.model.behaviour.HuntController h)
        {
            com.oddlabs.tt.player.Player owner = getOwner();
            if (owner.getWorld().isExtended() && owner.getAI() == null && !owner.usesReferenceCommandRules() && (h.getTarget() is com.oddlabs.tt.model.Building))
            {
                h.ordered();
            }
            return h;
        }
        // Java: 1398
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
                    else if ((target is com.oddlabs.tt.model.Unit friendly_unit) && !getOwner().isEnemy(friendly_unit.getOwner()))
                    {
                        pushController(new com.oddlabs.tt.model.behaviour.FollowController(this, target, aggressive));
                    }
                    else if (canAttack(target, false))
                    {
                        pushController(focus(new com.oddlabs.tt.model.behaviour.HuntController(this, ((com.oddlabs.tt.model.Selectable)target))));
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
                        pushController(focus(new com.oddlabs.tt.model.behaviour.HuntController(this, ((com.oddlabs.tt.model.Selectable)target))));
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
                case 5 /*GUARD*/:
                {
                    pushController(new com.oddlabs.tt.model.behaviour.GuardController(this, target));
                    break;
                }
                case 6 /*PATROL*/:
                {
                    pushController(new com.oddlabs.tt.model.behaviour.PatrolController(this, new com.oddlabs.tt.landscape.LandscapeTarget(getGridX(), getGridY()), target));
                    break;
                }
                default:
                {
                    java.lang.IO.println("Invalid action: " + java.lang.JStr.valueOf(action));
                    break;
                }
            }
        }
        // Java: 1474
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
        // Java: 1487
        public bool canDoMagic(int magic_index)
        {
            return !isDead() && magic_index >= 0 && magic_index < com.oddlabs.tt.model.RacesResources.NUM_MAGIC && getOwner().canDoMagic(magic_index) && magic_energy[magic_index] >= getMagicCost(magic_index) && (magic_index != com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_ELEMENT || elementCost()[5] == 0 || com.oddlabs.tt.model.ExtendedResources.findPayer(getOwner(), null, elementCost()) != null);
        }
        // Java: 1495
        public int[] elementCost()
        {
            int crystals = java.lang.Math.max(0, com.oddlabs.tt.model.weapon.ElementalMagicFactory.CRYSTAL_COST - getOwner().getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.FOCUS));
            return new int[] { 0, 0, 0, 0, 0, crystals };
        }
        public int smith_hits;
        // Java: 1503
        public int nextSmithHit()
        {
            return ++smith_hits;
        }
        // Java: 1507
        public void doMagic(int magic_index, bool clear_stack)
        {
            if (canDoMagic(magic_index))
            {
                if (clear_stack)
                {
                    clearControllerStack();
                }
                com.oddlabs.tt.model.Race casting_race = (magic_race_override_index >= 0 ? getOwner().getWorld().getRacesResources().getRace(magic_race_override_index) : getOwner().getRace());
                if (magic_index == com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_ELEMENT)
                {
                    int[] element_cost = elementCost();
                    com.oddlabs.tt.model.Building payer = (element_cost[5] > 0 ? com.oddlabs.tt.model.ExtendedResources.findPayer(getOwner(), null, element_cost) : (com.oddlabs.tt.model.Building)(null));
                    if (payer != null)
                    {
                        com.oddlabs.tt.model.ExtendedResources.pay(payer, element_cost, 1);
                    }
                }
                pushController(new com.oddlabs.tt.model.behaviour.MagicController(this, casting_race.getMagicFactory(magic_index)));
                magic_energy[magic_index] = 0.0f;
                last_magic_index = magic_index;
                getOwner().magicCast();
                triggerChiefsCourage(magic_index);
            }
        }
        // Java: 1543
        public void setMagicRaceOverride(int race_index)
        {
            this.magic_race_override_index = race_index;
        }
        // Java: 1555
        public int getMagicRaceOverride()
        {
            return magic_race_override_index;
        }
        // Java: 1559
        public void triggerChiefsCourage(int magic_index)
        {
            if (!getOwner().canUseChiefsCourage())
            {
                return;
            }
            if (!getOwner().isChiefsCourageReady())
            {
                return;
            }
            float duration = (magic_index >= 0 && magic_index < com.oddlabs.tt.model.Unit.COURAGE_DURATION.Length ? com.oddlabs.tt.model.Unit.COURAGE_DURATION[magic_index] : 6.0f);
            int previous_uses = getOwner().incrementAndGetChiefsCourageUses() - 1;
            float escalation = ((float)(java.lang.StrictMath.pow(1.015d, previous_uses)));
            float speed_mult = (float)(1.0f + (float)(((float)(com.oddlabs.tt.model.Unit.COURAGE_SPEED_MULT - 1.0f)) * escalation));
            float attack_speed_mult = (float)(1.0f + (float)(((float)(com.oddlabs.tt.model.Unit.COURAGE_ATTACK_SPEED_MULT - 1.0f)) * escalation));
            float combat_mult = (float)(1.0f + (float)(((float)(com.oddlabs.tt.model.Unit.COURAGE_COMBAT_MULT - 1.0f)) * escalation));
            float range_bonus_value = (float)(com.oddlabs.tt.model.Unit.COURAGE_RANGE_BONUS * escalation);
            float escalated_duration = (float)(duration * escalation);
            if (getOwner().getWorld().isExtended())
            {
                escalated_duration = (float)(escalated_duration * (float)(1.0f + (float)(0.25f * getOwner().getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.COURAGE_MASTERY))));
            }
            float caster_x = getPositionX();
            float caster_y = getPositionY();
            foreach (com.oddlabs.tt.player.Player player in getOwner().getWorld().getPlayers())
            {
                if (!getOwner().isAlly(player))
                {
                    continue;
                }
                foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(player.getUnits().getSet()))
                {
                    if ((s is com.oddlabs.tt.model.Unit unit) && !unit.isDead())
                    {
                        float dx = (float)(unit.getPositionX() - caster_x);
                        float dy = (float)(unit.getPositionY() - caster_y);
                        if ((float)((float)(dx * dx) + (float)(dy * dy)) > (float)(com.oddlabs.tt.model.Unit.COURAGE_RADIUS * com.oddlabs.tt.model.Unit.COURAGE_RADIUS))
                        {
                            continue;
                        }
                        unit.applyCourageBuff(escalated_duration, speed_mult, range_bonus_value, attack_speed_mult, combat_mult, com.oddlabs.tt.model.Unit.COOLDOWN_REDUCTION);
                    }
                }
            }
            getOwner().chiefsCourageTriggered();
            float z = (float)(getPositionZ() + getHitOffsetZ());
            getOwner().getWorld().getAudio().newAudio(new com.oddlabs.tt.audio.AudioParameters(getOwner().getWorld().getRacesResources().getStunSound(getOwner().getWorld().getRandom()), getPositionX(), getPositionY(), z, com.oddlabs.tt.audio.AudioPlayer.AUDIO_RANK_MAGIC, com.oddlabs.tt.audio.AudioPlayer.AUDIO_DISTANCE_MAGIC, com.oddlabs.tt.audio.AudioPlayer.AUDIO_GAIN_STUN_LUR, com.oddlabs.tt.audio.AudioPlayer.AUDIO_RADIUS_STUN_LUR, 1.0f));
        }
        // Java: 1628
        public int getLastMagicIndex()
        {
            return last_magic_index;
        }
        // Java: 1632
        public float getMagicProgress(int magic_index)
        {
            return (float)(magic_energy[magic_index] / getMagicCost(magic_index));
        }
        // Java: 1636
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
        // Java: 1652
        public override int getAnimation()
        {
            return animation;
        }
        // Java: 1657
        public override float getAnimationTicks()
        {
            return anim_time;
        }
        // Java: 1662
        public float getMountOffset()
        {
            return mount_offset;
        }
        // Java: 1672
        public void setMountOffset(float offset)
        {
            mount_offset = offset;
        }
        // Java: 1676
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
        // Java: 1689
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
        // Java: 1715
        public float getHitError()
        {
            return (on_ship ? 2.2f : 0.0f);
        }
        // Java: 1719
        public void debugRender()
        {
            path_tracker.debugRender();
        }
        static Unit()
        {
            Unit.MAX_MAGIC_ENERGY = new float[] { 40.0f, 70.0f, 90.0f, 60.0f, 75.0f, 80.0f };
            Unit.COURAGE_DURATION = new float[] { 6.0f, 9.0f, 16.0f };
        }
        private void init_Unit()
        {
            this.courage_stacks = new java.util.ArrayList();
            this.courage_time_remaining = 0.0f;
            this.courage_speed_mult = 1.0f;
            this.courage_range_bonus = 0.0f;
            this.courage_attack_speed_mult = 1.0f;
            this.courage_combat_mult = 1.0f;
            this.courage_cooldown_reduction = 0.0f;
            this.stuck_check_x = java.lang.Float.NaN;
            this.stuck_check_y = java.lang.Float.NaN;
            this.stuck_time = 0.0f;
            this.magic_energy = new float[com.oddlabs.tt.model.Unit.MAX_MAGIC_ENERGY.Length];
            this.magic_race_override_index = (-1);
            this.last_magic_index = (-1);
            this.time_since_damage = 0.0f;
            this.chieftain_heal_timer = (float)(-1.0f);
            this.animation = com.oddlabs.tt.model.Unit.Animation.IDLING;
            this.on_ship = false;
            this.mount_offset = 0;
            this.chief_serial = (-1);
            this.elite_kind = (-1);
            this.god_kind = (-1);
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
            if (((object)unit_template == (object)owner.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_CHIEFTAIN)))
            {
                chief_serial = owner.nextChiefSerial();
            }
            getAbilities().addAbilities(unit_template.getAbilities());
            register();
            hit_points = getEffectiveMaxHitPoints();
            this.path_tracker = new com.oddlabs.tt.pathfinder.PathTracker(getUnitGrid(), this);
            com.oddlabs.tt.model.UnitSupplyContainerFactory factory = unit_template.getUnitSupplyContainerFactory();
            supply_container = (factory != null ? ((com.oddlabs.tt.model.UnitSupplyContainer)factory.createContainer(this)) : (com.oddlabs.tt.model.UnitSupplyContainer)(null));
            if (!imaginary)
            {
                findInitialPosition(x, y, grid_targets_only);
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
                    bool spread = getOwner().getWorld().isExtended();
                    foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(getOwner().getUnits().getSet()))
                    {
                        if ((s.getCurrentController() is com.oddlabs.tt.model.behaviour.WalkController))
                        {
                            com.oddlabs.tt.util.Target target = (((com.oddlabs.tt.model.behaviour.WalkController)s.getCurrentController())).getTarget();
                            for (int oy = (spread ? (-1) : 0); oy <= ((spread ? 1 : 0)); oy++)
                            {
                                for (int ox = (spread ? (-1) : 0); ox <= ((spread ? 1 : 0)); ox++)
                                {
                                    int tx = target.getGridX() + ox;
                                    int ty = target.getGridY() + oy;
                                    if (tx < 0 || ty < 0 || tx >= grid.getGridSize() || ty >= grid.getGridSize())
                                    {
                                        continue;
                                    }
                                    if (!grid.isGridOccupied(tx, ty))
                                    {
                                        grid.occupyGrid(tx, ty, this);
                                        temp_occupants.add(new com.oddlabs.tt.landscape.LandscapeTarget(tx, ty));
                                    }
                                }
                            }
                        }
                    }
                    com.oddlabs.tt.util.Target found_target = grid.findGridTargets(rally_point.getGridX(), rally_point.getGridY(), 1, true)[0];
                    unit_target = (found_target != null ? found_target : rally_point);
                    foreach (com.oddlabs.tt.util.Target target_2 in java.lang.JIter.of(temp_occupants))
                    {
                        grid.freeGrid(target_2.getGridX(), target_2.getGridY(), this);
                    }
                }
                else
                {
                    unit_target = rally_point;
                }
                bool aggressive = unit_template.getAbilities().isWarrior();
                setTarget(unit_target, com.oddlabs.tt.model.Action.DEFAULT, aggressive);
            }
        }
    }
}
