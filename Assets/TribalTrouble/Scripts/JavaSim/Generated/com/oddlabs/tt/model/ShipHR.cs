// Translated from ShipHR.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 20
    public class ShipHR : java.lang.Object
    {
        public bool vikings;
        // Java: 24
        public float unitSize(com.oddlabs.tt.model.Unit unit)
        {
            if (unit.isWarrior())
            {
                return 1.2f;
            }
            else
            {
                return 0.7f;
            }
        }
        // Java: 32
        public interface Row
        {
            // Java: 33
            bool canFit(com.oddlabs.tt.model.Unit unit);
            // Java: 35
            void seat(com.oddlabs.tt.model.Unit unit);
            // Java: 37
            void exit(com.oddlabs.tt.model.Unit unit);
            // Java: 39
            com.oddlabs.tt.model.ShipAllocation getAllocation(com.oddlabs.tt.model.Unit unit);
            // Java: 41
            void killAll();
            // Java: 43
            java.util.List allUnits();
            // Java: 45
            com.oddlabs.tt.model.Unit findUnit(com.oddlabs.tt.model.UnitTemplate template);
            // Java: 47
            bool needRowers();
            // Java: 49
            int countRowers();
            // Java: 56
            bool canSparePeons();
        }
        // Java: 59
        public class Rudder : java.lang.Object, com.oddlabs.tt.model.ShipHR.Row
        {
            public com.oddlabs.tt.model.Unit unit;
            public com.oddlabs.tt.model.ShipAllocation alloc;
            // Java: 71
            public virtual bool canFit(com.oddlabs.tt.model.Unit unit)
            {
                return false;
            }
            // Java: 75
            public virtual void seat(com.oddlabs.tt.model.Unit unit)
            {
            }
            // Java: 79
            public virtual void exit(com.oddlabs.tt.model.Unit unit)
            {
            }
            // Java: 83
            public virtual com.oddlabs.tt.model.ShipAllocation getAllocation(com.oddlabs.tt.model.Unit unit)
            {
                if (((object)this.unit == (object)unit))
                {
                    return alloc;
                }
                else
                {
                    return null;
                }
            }
            // Java: 91
            public virtual void killAll()
            {
                if (unit != null && !unit.isDead())
                {
                    unit.drown();
                }
                unit = null;
            }
            // Java: 103
            public virtual java.util.List allUnits()
            {
                return new java.util.ArrayList();
            }
            // Java: 107
            public virtual com.oddlabs.tt.model.Unit findUnit(com.oddlabs.tt.model.UnitTemplate template)
            {
                return null;
            }
            // Java: 111
            public virtual bool needRowers()
            {
                return false;
            }
            // Java: 115
            public virtual int countRowers()
            {
                return 0;
            }
            // Java: 119
            public virtual bool canSparePeons()
            {
                return false;
            }
            private void init_Rudder()
            {
                this.unit = null;
            }
            public Rudder(com.oddlabs.tt.model.Ship ship, float x, float y, float z) : base()
            {
                init_Rudder();
                alloc = new com.oddlabs.tt.model.ShipAllocation(new org.joml.Vector3f(x, y, z), new org.joml.Vector2f(0.0f, 1.0f), com.oddlabs.tt.model.ShipAllocation.STEERING);
                com.oddlabs.tt.player.Player owner = ship.getOwner();
                unit = new com.oddlabs.tt.model.Unit(owner, x, y, null, owner.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON), null, false, false, true);
                unit.mount(ship, alloc);
            }
        }
        // Java: 124
        public class LowerDeckRow : java.lang.Object, com.oddlabs.tt.model.ShipHR.Row
        {
            public com.oddlabs.tt.model.ShipHR this_0;
            public float x;
            public float y0;
            public float y1;
            public float z;
            public float total;
            public bool left_rower;
            public bool right_rower;
            public float used;
            public java.util.Map allocs;
            public java.util.List all_units;
            public java.util.List peons;
            public java.util.List warriors;
            // Java: 149
            public virtual bool canFit(com.oddlabs.tt.model.Unit unit)
            {
                float size = this_0.unitSize(unit);
                return (float)(total - used) > size;
            }
            // Java: 154
            public virtual void seat(com.oddlabs.tt.model.Unit unit)
            {
                if (!allocs.containsKey(unit))
                {
                    float size = this_0.unitSize(unit);
                    allocs.put(unit, new com.oddlabs.tt.model.ShipAllocation());
                    all_units.add(unit);
                    if (unit.isWarrior())
                    {
                        warriors.add(unit);
                    }
                    else
                    {
                        peons.add(unit);
                    }
                    reassign();
                }
            }
            // Java: 168
            public virtual void exit(com.oddlabs.tt.model.Unit unit)
            {
                if (allocs.containsKey(unit))
                {
                    allocs.remove(unit);
                    all_units.remove(unit);
                    peons.remove(unit);
                    warriors.remove(unit);
                    reassign();
                }
            }
            // Java: 179
            public virtual void killAll()
            {
                for (int i = 0; i < all_units.size(); i++)
                {
                    com.oddlabs.tt.model.Unit unit = ((com.oddlabs.tt.model.Unit)all_units.get(i));
                    if (!unit.isDead())
                    {
                        unit.drown();
                    }
                }
                all_units.clear();
                peons.clear();
                warriors.clear();
                allocs.clear();
            }
            // Java: 193
            public virtual com.oddlabs.tt.model.ShipAllocation getAllocation(com.oddlabs.tt.model.Unit unit)
            {
                if (allocs.containsKey(unit))
                {
                    return ((com.oddlabs.tt.model.ShipAllocation)allocs.get(unit));
                }
                else
                {
                    return null;
                }
            }
            // Java: 201
            public virtual java.util.List allUnits()
            {
                return all_units;
            }
            // Java: 205
            public virtual com.oddlabs.tt.model.Unit findUnit(com.oddlabs.tt.model.UnitTemplate template)
            {
                for (int i = 0; i < all_units.size(); i++)
                {
                    com.oddlabs.tt.model.Unit unit = ((com.oddlabs.tt.model.Unit)all_units.get(i));
                    if (((object)((com.oddlabs.tt.model.UnitTemplate)unit.getTemplate()) == (object)template))
                    {
                        return unit;
                    }
                }
                return null;
            }
            // Java: 215
            public virtual bool needRowers()
            {
                int required = ((left_rower ? 1 : 0)) + ((right_rower ? 1 : 0));
                return peons.size() < required;
            }
            // Java: 220
            public virtual int countRowers()
            {
                int required = ((left_rower ? 1 : 0)) + ((right_rower ? 1 : 0));
                return java.lang.Math.min(peons.size(), required);
            }
            // Java: 225
            public virtual bool canSparePeons()
            {
                int required = ((left_rower ? 1 : 0)) + ((right_rower ? 1 : 0));
                return peons.size() > required;
            }
            // Java: 234
            public void reassign()
            {
                used = 0.0f;
                for (int i = 0; i < all_units.size(); i++)
                {
                    used += this_0.unitSize(((com.oddlabs.tt.model.Unit)all_units.get(i)));
                }
                if (all_units.size() == 1)
                {
                    com.oddlabs.tt.model.Unit unit = ((com.oddlabs.tt.model.Unit)all_units.get(0));
                    float size = this_0.unitSize(unit);
                    com.oddlabs.tt.model.ShipAllocation alloc = ((com.oddlabs.tt.model.ShipAllocation)allocs.get(unit));
                    alloc.setRotation(1.0f, 0.0f);
                    if (unit.isWarrior())
                    {
                        alloc.setOffset(x, (float)(((float)(y0 + y1)) * 0.5f), z);
                        alloc.setRole(com.oddlabs.tt.model.ShipAllocation.SITTING);
                    }
                    else
                    {
                        if (right_rower)
                        {
                            alloc.setOffset(x, (float)(y0 + (float)(size * 0.5f)), z);
                            alloc.setRole(com.oddlabs.tt.model.ShipAllocation.ROWING_RIGHT);
                        }
                        else if (left_rower)
                        {
                            alloc.setOffset(x, (float)(y1 - (float)(size * 0.5f)), z);
                            alloc.setRole(com.oddlabs.tt.model.ShipAllocation.ROWING_LEFT);
                        }
                        else
                        {
                            alloc.setOffset(x, (float)(((float)(y0 + y1)) * 0.5f), z);
                            alloc.setRole(com.oddlabs.tt.model.ShipAllocation.SITTING);
                        }
                    }
                }
                else if (all_units.size() > 1)
                {
                    float gap = (float)(((float)(total - used)) / (all_units.size() - 1));
                    float yOffset = java.lang.Math.min(y0, y1);
                    int tmpCounter = 0;
                    com.oddlabs.tt.model.Unit leftRower = null;
                    com.oddlabs.tt.model.Unit rightRower = null;
                    if (left_rower && peons.size() > tmpCounter)
                    {
                        leftRower = ((com.oddlabs.tt.model.Unit)peons.get(tmpCounter));
                        tmpCounter++;
                    }
                    if (right_rower && peons.size() > tmpCounter)
                    {
                        rightRower = ((com.oddlabs.tt.model.Unit)peons.get(tmpCounter));
                        tmpCounter++;
                    }
                    if (rightRower != null)
                    {
                        com.oddlabs.tt.model.ShipAllocation alloc_2 = ((com.oddlabs.tt.model.ShipAllocation)allocs.get(rightRower));
                        float s = this_0.unitSize(rightRower);
                        alloc_2.setOffset(x, (float)(yOffset + (float)(s * 0.5f)), z);
                        alloc_2.setRole(com.oddlabs.tt.model.ShipAllocation.ROWING_RIGHT);
                        yOffset = (float)(yOffset + (float)(gap + s));
                    }
                    for (int i_2 = 0; i_2 < all_units.size(); i_2++)
                    {
                        com.oddlabs.tt.model.Unit unit_2 = ((com.oddlabs.tt.model.Unit)all_units.get(i_2));
                        if (((object)unit_2 == (object)leftRower) || ((object)unit_2 == (object)rightRower))
                        {
                            continue;
                        }
                        com.oddlabs.tt.model.ShipAllocation alloc_3 = ((com.oddlabs.tt.model.ShipAllocation)allocs.get(unit_2));
                        float s_2 = this_0.unitSize(unit_2);
                        alloc_3.setOffset(x, (float)(yOffset + (float)(s_2 * 0.5f)), z);
                        yOffset = (float)(yOffset + (float)(gap + s_2));
                    }
                    if (leftRower != null)
                    {
                        com.oddlabs.tt.model.ShipAllocation alloc_4 = ((com.oddlabs.tt.model.ShipAllocation)allocs.get(leftRower));
                        float s_3 = this_0.unitSize(leftRower);
                        alloc_4.setOffset(x, (float)(yOffset + (float)(s_3 * 0.5f)), z);
                        alloc_4.setRole(com.oddlabs.tt.model.ShipAllocation.ROWING_LEFT);
                        yOffset = (float)(yOffset + (float)(gap + s_3));
                    }
                }
            }
            private void init_LowerDeckRow()
            {
                this.allocs = new java.util.LinkedHashMap();
                this.all_units = new java.util.ArrayList();
                this.peons = new java.util.ArrayList();
                this.warriors = new java.util.ArrayList();
            }
            public LowerDeckRow(com.oddlabs.tt.model.ShipHR this_0_, float x, float y0, float y1, float z, bool left_rower, bool right_rower) : base()
            {
                this.this_0 = this_0_;
                init_LowerDeckRow();
                this.x = x;
                this.y0 = y0;
                this.y1 = y1;
                this.z = z;
                this.total = java.lang.Math.abs((float)(y1 - y0));
                this.used = 0.0f;
                this.left_rower = left_rower;
                this.right_rower = right_rower;
            }
        }
        // Java: 301
        public class UpperDeckRow : java.lang.Object, com.oddlabs.tt.model.ShipHR.Row
        {
            public com.oddlabs.tt.model.Unit left;
            public com.oddlabs.tt.model.Unit right;
            public com.oddlabs.tt.model.ShipAllocation leftAlloc;
            public com.oddlabs.tt.model.ShipAllocation rightAlloc;
            // Java: 312
            public virtual bool canFit(com.oddlabs.tt.model.Unit unit)
            {
                return unit.isWarrior() && (left == null || right == null);
            }
            // Java: 316
            public virtual void seat(com.oddlabs.tt.model.Unit unit)
            {
                if (unit.isWarrior())
                {
                    if (left == null)
                    {
                        left = unit;
                    }
                    else if (right == null)
                    {
                        right = unit;
                    }
                }
            }
            // Java: 326
            public virtual void exit(com.oddlabs.tt.model.Unit unit)
            {
                if (((object)left == (object)unit))
                {
                    left = null;
                }
                else if (((object)right == (object)unit))
                {
                    right = null;
                }
            }
            // Java: 334
            public virtual void killAll()
            {
                if (left != null)
                {
                    if (!left.isDead())
                    {
                        left.drown();
                    }
                    left = null;
                }
                if (right != null)
                {
                    if (!right.isDead())
                    {
                        right.drown();
                    }
                    right = null;
                }
            }
            // Java: 353
            public virtual com.oddlabs.tt.model.ShipAllocation getAllocation(com.oddlabs.tt.model.Unit unit)
            {
                if (((object)left == (object)unit))
                {
                    return leftAlloc;
                }
                else if (((object)right == (object)unit))
                {
                    return rightAlloc;
                }
                return null;
            }
            // Java: 362
            public virtual java.util.List allUnits()
            {
                java.util.List ret = new java.util.ArrayList();
                if (left != null)
                {
                    ret.add(left);
                }
                if (right != null)
                {
                    ret.add(right);
                }
                return ret;
            }
            // Java: 373
            public virtual com.oddlabs.tt.model.Unit findUnit(com.oddlabs.tt.model.UnitTemplate template)
            {
                if (left != null && ((object)((com.oddlabs.tt.model.UnitTemplate)left.getTemplate()) == (object)template))
                {
                    return left;
                }
                if (right != null && ((object)((com.oddlabs.tt.model.UnitTemplate)right.getTemplate()) == (object)template))
                {
                    return right;
                }
                return null;
            }
            // Java: 383
            public virtual bool needRowers()
            {
                return false;
            }
            // Java: 387
            public virtual int countRowers()
            {
                return 0;
            }
            // Java: 391
            public virtual bool canSparePeons()
            {
                return false;
            }
            private void init_UpperDeckRow()
            {
                this.left = null;
                this.right = null;
            }
            public UpperDeckRow(float x, float y, float z) : base()
            {
                init_UpperDeckRow();
                leftAlloc = new com.oddlabs.tt.model.ShipAllocation(new org.joml.Vector3f(x, y, z), new org.joml.Vector2f(0.0f, 1.0f), com.oddlabs.tt.model.ShipAllocation.FIGHTING);
                rightAlloc = new com.oddlabs.tt.model.ShipAllocation(new org.joml.Vector3f(x, (float)(-y), z), new org.joml.Vector2f(0.0f, (float)(-1.0f)), com.oddlabs.tt.model.ShipAllocation.FIGHTING);
            }
        }
        public java.util.LinkedHashMap unit2row;
        public java.util.ArrayList rows;
        // Java: 464
        public bool canAllocate(com.oddlabs.tt.model.Unit unit)
        {
            for (int i = 0; i < rows.size(); i++)
            {
                if (((com.oddlabs.tt.model.ShipHR.Row)rows.get(i)).canFit(unit))
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 473
        public com.oddlabs.tt.model.ShipAllocation tryAllocate(com.oddlabs.tt.model.Unit unit)
        {
            if (!unit.isWarrior())
            {
                for (int i = 0; i < rows.size(); i++)
                {
                    com.oddlabs.tt.model.ShipHR.Row row = ((com.oddlabs.tt.model.ShipHR.Row)rows.get(i));
                    if (row.needRowers())
                    {
                        row.seat(unit);
                        unit2row.put(unit, row);
                        return row.getAllocation(unit);
                    }
                }
                for (int i_2 = 0; i_2 < rows.size(); i_2++)
                {
                    com.oddlabs.tt.model.ShipHR.Row row_2 = ((com.oddlabs.tt.model.ShipHR.Row)rows.get(i_2));
                    if (row_2.canFit(unit))
                    {
                        row_2.seat(unit);
                        unit2row.put(unit, row_2);
                        return row_2.getAllocation(unit);
                    }
                }
            }
            else
            {
                for (int i_3 = rows.size() - 1; i_3 >= 0; i_3--)
                {
                    com.oddlabs.tt.model.ShipHR.Row row_3 = ((com.oddlabs.tt.model.ShipHR.Row)rows.get(i_3));
                    if (row_3.canFit(unit))
                    {
                        row_3.seat(unit);
                        unit2row.put(unit, row_3);
                        com.oddlabs.tt.model.ShipAllocation alloc = row_3.getAllocation(unit);
                        if (alloc.getRole() == com.oddlabs.tt.model.ShipAllocation.FIGHTING)
                        {
                            unit.increaseRange(10.0f);
                        }
                        return alloc;
                    }
                }
            }
            return null;
        }
        // Java: 509
        public java.util.List crew()
        {
            java.util.List l = new java.util.ArrayList();
            for (int i = 0; i < rows.size(); i++)
            {
                l.addAll(((com.oddlabs.tt.model.ShipHR.Row)rows.get(i)).allUnits());
            }
            return l;
        }
        // Java: 516
        public void killCrew()
        {
            for (int i = 0; i < rows.size(); i++)
            {
                ((com.oddlabs.tt.model.ShipHR.Row)rows.get(i)).killAll();
                unit2row.clear();
            }
        }
        // Java: 524
        public void disembark(com.oddlabs.tt.model.Unit unit)
        {
            com.oddlabs.tt.model.ShipHR.Row row = ((com.oddlabs.tt.model.ShipHR.Row)unit2row.get(unit));
            if (row == null)
            {
                return;
            }
            com.oddlabs.tt.model.ShipAllocation alloc = row.getAllocation(unit);
            row.exit(unit);
            unit2row.remove(unit);
            unit.setReference(null);
            unit.unmount();
            if (alloc != null && alloc.getRole() == com.oddlabs.tt.model.ShipAllocation.FIGHTING)
            {
                unit.increaseRange((float)(-10.0f));   // (Steam 2.2.207 fix: was -8, each trip on a fighting seat left +2 range)
            }
        }
        // Java: 537
        public void removeUnit(com.oddlabs.tt.model.Unit unit)
        {
            if (unit2row.containsKey(unit))
            {
                com.oddlabs.tt.model.ShipHR.Row row = ((com.oddlabs.tt.model.ShipHR.Row)unit2row.get(unit));
                row.exit(unit);
                unit.setReference(null);
                unit2row.remove(unit);
            }
        }
        // Java: 546
        public com.oddlabs.tt.model.Unit exitUnit(com.oddlabs.tt.model.UnitTemplate template)
        {
            bool warrior = (template.getWeaponFactory().getType() != null);
            if (warrior)
            {
                for (int i = 0; i < rows.size(); i++)
                {
                    com.oddlabs.tt.model.ShipHR.Row row = ((com.oddlabs.tt.model.ShipHR.Row)rows.get(i));
                    com.oddlabs.tt.model.Unit unit = row.findUnit(template);
                    if (unit != null)
                    {
                        com.oddlabs.tt.model.ShipAllocation alloc = row.getAllocation(unit);
                        row.exit(unit);
                        unit2row.remove(unit);
                        unit.setReference(null);
                        unit.unmount();
                        if (alloc.getRole() == com.oddlabs.tt.model.ShipAllocation.FIGHTING)
                        {
                            unit.increaseRange((float)(-10.0f));   // (Steam 2.2.207 fix: was -8, each trip on a fighting seat left +2 range)
                        }
                        return unit;
                    }
                }
            }
            else
            {
                for (int i_2 = rows.size() - 1; i_2 >= 0; i_2--)
                {
                    com.oddlabs.tt.model.ShipHR.Row row_2 = ((com.oddlabs.tt.model.ShipHR.Row)rows.get(i_2));
                    com.oddlabs.tt.model.Unit unit_2 = row_2.findUnit(template);
                    if (unit_2 != null && row_2.canSparePeons())
                    {
                        row_2.exit(unit_2);
                        unit2row.remove(unit_2);
                        unit_2.setReference(null);
                        unit_2.unmount();
                        return unit_2;
                    }
                }
                for (int i_3 = rows.size() - 1; i_3 >= 0; i_3--)
                {
                    com.oddlabs.tt.model.ShipHR.Row row_3 = ((com.oddlabs.tt.model.ShipHR.Row)rows.get(i_3));
                    com.oddlabs.tt.model.Unit unit_3 = row_3.findUnit(template);
                    if (unit_3 != null)
                    {
                        com.oddlabs.tt.model.ShipAllocation alloc_2 = row_3.getAllocation(unit_3);
                        row_3.exit(unit_3);
                        unit2row.remove(unit_3);
                        unit_3.setReference(null);
                        unit_3.unmount();
                        return unit_3;
                    }
                }
            }
            return null;
        }
        // Java: 605
        public bool isRock(System.Type type)
        {
            return ((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)) || ((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RockSpearWeapon));
        }
        // Java: 609
        public bool isIron(System.Type type)
        {
            return ((object)type == (object)typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)) || ((object)type == (object)typeof(com.oddlabs.tt.model.weapon.IronSpearWeapon));
        }
        // Java: 613
        public bool isRubber(System.Type type)
        {
            return ((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)) || ((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RubberSpearWeapon));
        }
        // Java: 617
        public bool isSameType(System.Type type, com.oddlabs.tt.model.Unit unit)
        {
            if (!unit.isWarrior())
            {
                return ((object)type == (object)typeof(com.oddlabs.tt.model.Unit));
            }
            else
            {
                System.Type unitType = unit.getWeaponFactory().getType();
                if (isRock(unitType) && isRock(type))
                {
                    return true;
                }
                if (isIron(unitType) && isIron(type))
                {
                    return true;
                }
                if (isRubber(unitType) && isRubber(type))
                {
                    return true;
                }
                return false;
            }
        }
        // Java: 629
        public int countUnitsOfType(System.Type type)
        {
            int result = 0;
            for (int i = 0; i < rows.size(); i++)
            {
                com.oddlabs.tt.model.ShipHR.Row row = ((com.oddlabs.tt.model.ShipHR.Row)rows.get(i));
                java.util.List units = row.allUnits();
                for (int j = 0; j < units.size(); j++)
                {
                    com.oddlabs.tt.model.Unit unit = ((com.oddlabs.tt.model.Unit)units.get(j));
                    if (isSameType(type, unit))
                    {
                        result++;
                    }
                }
            }
            return result;
        }
        // Java: 644
        public int countUnits()
        {
            return unit2row.size();
        }
        // Java: 648
        public int countPeons()
        {
            int result = 0;
            com.oddlabs.tt.model.Unit[] units = ((com.oddlabs.tt.model.Unit[])unit2row.keySet().toArray(new com.oddlabs.tt.model.Unit[0]));
            for (int j = 0; j < units.Length; j++)
            {
                com.oddlabs.tt.model.Unit unit = units[j];
                if (!unit.isWarrior())
                {
                    result++;
                }
            }
            return result;
        }
        // Java: 660
        public com.oddlabs.tt.model.Unit pickVictim(float random)
        {
            int index = java.lang.StrictMath.round((float)(random * 120));
            if (index >= unit2row.size())
            {
                return null;
            }
            java.util.Iterator it = unit2row.entrySet().iterator();
            java.util.Map.Entry victim = ((java.util.Map.Entry)it.next());
            for (int i = 0; i < index; i++)
            {
                victim = ((java.util.Map.Entry)it.next());
            }
            com.oddlabs.tt.model.Unit unit = ((com.oddlabs.tt.model.Unit)victim.getKey());
            com.oddlabs.tt.model.ShipHR.Row row = ((com.oddlabs.tt.model.ShipHR.Row)victim.getValue());
            return unit;
        }
        // Java: 675
        public int countRowers()
        {
            int result = 0;
            for (int i = 0; i < rows.size(); i++)
            {
                com.oddlabs.tt.model.ShipHR.Row row = ((com.oddlabs.tt.model.ShipHR.Row)rows.get(i));
                result += row.countRowers();
            }
            return result;
        }
        private void init_ShipHR()
        {
            this.unit2row = new java.util.LinkedHashMap();
            this.rows = new java.util.ArrayList();
        }
        public ShipHR(com.oddlabs.tt.model.Ship ship, bool vikings) : base()
        {
            init_ShipHR();
            this.vikings = vikings;
            if (vikings)
            {
                rows.add(new com.oddlabs.tt.model.ShipHR.Rudder(ship, (float)(-11.14f), (float)(-0.43f), +0.54f));
                rows.add(new com.oddlabs.tt.model.ShipHR.LowerDeckRow(this, (float)(-9.62f), (float)(-1.98f), +1.98f, 0.42f, true, true));
                rows.add(new com.oddlabs.tt.model.ShipHR.LowerDeckRow(this, (float)(-8.18f), (float)(-2.31f), +2.31f, 0.42f, true, true));
                rows.add(new com.oddlabs.tt.model.ShipHR.LowerDeckRow(this, (float)(-6.71f), (float)(-2.54f), +2.54f, 0.43f, true, true));
                rows.add(new com.oddlabs.tt.model.ShipHR.LowerDeckRow(this, (float)(-5.32f), +0.25f, +2.75f, 0.44f, true, false));
                rows.add(new com.oddlabs.tt.model.ShipHR.LowerDeckRow(this, (float)(-5.32f), (float)(-2.75f), (float)(-0.25f), 0.44f, false, true));
                rows.add(new com.oddlabs.tt.model.ShipHR.LowerDeckRow(this, (float)(-3.42f), +0.39f, +2.9f, 0.45f, true, false));
                rows.add(new com.oddlabs.tt.model.ShipHR.LowerDeckRow(this, (float)(-3.42f), (float)(-2.9f), (float)(-0.39f), 0.45f, false, true));
                rows.add(new com.oddlabs.tt.model.ShipHR.LowerDeckRow(this, (float)(-1.75f), (float)(-2.99f), +2.99f, 0.51f, true, true));
                rows.add(new com.oddlabs.tt.model.ShipHR.LowerDeckRow(this, (float)(-0.24f), (float)(-3.07f), +3.07f, 0.55f, true, true));
                rows.add(new com.oddlabs.tt.model.ShipHR.LowerDeckRow(this, +1.3f, (float)(-3.0f), +3.0f, 0.58f, true, true));
                rows.add(new com.oddlabs.tt.model.ShipHR.LowerDeckRow(this, +2.8f, (float)(-2.9f), +2.9f, 0.59f, true, true));
                rows.add(new com.oddlabs.tt.model.ShipHR.LowerDeckRow(this, +4.32f, (float)(-2.8f), +2.8f, 0.62f, true, true));
                rows.add(new com.oddlabs.tt.model.ShipHR.LowerDeckRow(this, +5.81f, (float)(-2.64f), +2.64f, 0.65f, true, true));
                rows.add(new com.oddlabs.tt.model.ShipHR.LowerDeckRow(this, +7.31f, +0.18f, +2.45f, 0.66f, true, false));
                rows.add(new com.oddlabs.tt.model.ShipHR.LowerDeckRow(this, +7.31f, (float)(-2.45f), (float)(-0.18f), 0.66f, false, true));
                rows.add(new com.oddlabs.tt.model.ShipHR.LowerDeckRow(this, +8.8f, (float)(-2.19f), +2.19f, 0.68f, true, true));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow(+9.64f, +1.17f, +3.29f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow(+7.89f, +1.36f, +3.29f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow(+6.53f, +1.6f, +3.29f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow(+5.12f, +1.74f, +3.29f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow(+3.78f, +1.8f, +3.29f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow(+2.1f, +2.07f, +3.11f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow(+0.56f, +2.07f, +3.11f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow((float)(-0.82f), +2.07f, +3.11f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow((float)(-2.6f), +2.07f, +3.11f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow((float)(-5.24f), +1.82f, +3.13f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow((float)(-7.17f), +1.86f, +2.99f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow((float)(-8.99f), +1.31f, +2.99f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow((float)(-10.49f), +1.0f, +2.99f));
            }
            else
            {
                rows.add(new com.oddlabs.tt.model.ShipHR.Rudder(ship, (float)(-11.4f), +0.87f, +2.455f));
                rows.add(new com.oddlabs.tt.model.ShipHR.LowerDeckRow(this, (float)(-9.39f), (float)(-2.88f), +2.88f, +0.37f, true, true));
                rows.add(new com.oddlabs.tt.model.ShipHR.LowerDeckRow(this, (float)(-7.92f), (float)(-2.88f), +2.88f, +0.37f, true, true));
                rows.add(new com.oddlabs.tt.model.ShipHR.LowerDeckRow(this, (float)(-6.47f), (float)(-2.88f), +2.88f, +0.37f, true, true));
                rows.add(new com.oddlabs.tt.model.ShipHR.LowerDeckRow(this, (float)(-4.84f), (float)(-2.88f), +2.88f, +0.37f, true, true));
                rows.add(new com.oddlabs.tt.model.ShipHR.LowerDeckRow(this, (float)(-3.44f), (float)(-2.88f), +2.88f, +0.37f, true, true));
                rows.add(new com.oddlabs.tt.model.ShipHR.LowerDeckRow(this, (float)(-1.96f), (float)(-2.88f), +2.88f, +0.37f, true, true));
                rows.add(new com.oddlabs.tt.model.ShipHR.LowerDeckRow(this, (float)(-0.42f), (float)(-2.88f), +2.88f, +0.37f, true, true));
                rows.add(new com.oddlabs.tt.model.ShipHR.LowerDeckRow(this, +1.1f, (float)(-2.88f), +2.88f, +0.37f, true, true));
                rows.add(new com.oddlabs.tt.model.ShipHR.LowerDeckRow(this, +2.46f, (float)(-2.88f), +2.88f, +0.37f, true, true));
                rows.add(new com.oddlabs.tt.model.ShipHR.LowerDeckRow(this, +4.17f, (float)(-2.84f), +2.84f, +0.37f, true, true));
                rows.add(new com.oddlabs.tt.model.ShipHR.LowerDeckRow(this, +5.57f, (float)(-2.77f), +2.77f, +0.37f, true, true));
                rows.add(new com.oddlabs.tt.model.ShipHR.LowerDeckRow(this, +7.01f, (float)(-2.42f), +2.42f, +0.37f, true, true));
                rows.add(new com.oddlabs.tt.model.ShipHR.LowerDeckRow(this, +8.62f, (float)(-2.42f), +2.42f, +0.37f, true, true));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow((float)(-9.39f), +1.01f, +3.41f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow((float)(-7.75f), +1.22f, +3.41f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow((float)(-6.02f), +1.35f, +3.41f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow((float)(-3.93f), +1.22f, +3.24f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow((float)(-2.5f), +1.22f, +3.24f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow((float)(-1.0f), +1.22f, +3.24f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow(+0.5f, +1.22f, +3.24f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow(+2.0f, +1.22f, +3.24f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow(+3.5f, +1.22f, +3.24f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow(+5.0f, +1.22f, +3.24f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow(+6.5f, +1.22f, +3.24f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow(+8.0f, +1.09f, +3.24f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow(+9.5f, +0.83f, +3.24f));
            }
        }
    }
}
