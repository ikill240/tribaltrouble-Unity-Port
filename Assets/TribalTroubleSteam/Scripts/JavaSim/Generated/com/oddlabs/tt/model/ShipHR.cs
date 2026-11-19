// Translated from ShipHR.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 20
    public class ShipHR : java.lang.Object
    {
        public const float WARRIOR_INCREASE = 10.0f;
        public bool vikings;
        // Java: 25
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
        // Java: 33
        public interface Row
        {
            // Java: 34
            bool canFit(com.oddlabs.tt.model.Unit unit);
            // Java: 36
            void seat(com.oddlabs.tt.model.Unit unit);
            // Java: 38
            void exit(com.oddlabs.tt.model.Unit unit);
            // Java: 40
            com.oddlabs.tt.model.ShipAllocation getAllocation(com.oddlabs.tt.model.Unit unit);
            // Java: 42
            void killAll();
            // Java: 44
            java.util.List allUnits();
            // Java: 46
            com.oddlabs.tt.model.Unit findUnit(com.oddlabs.tt.model.UnitTemplate template);
            // Java: 48
            bool needRowers();
            // Java: 50
            bool canSparePeons();
            // Java: 52
            int countRowers();
            // Java: 54
            bool empty();
        }
        // Java: 57
        public class Rudder : java.lang.Object, com.oddlabs.tt.model.ShipHR.Row
        {
            public com.oddlabs.tt.model.ShipHR this_0;
            public com.oddlabs.tt.model.Unit unit;
            public com.oddlabs.tt.model.ShipAllocation alloc;
            // Java: 69
            public virtual bool canFit(com.oddlabs.tt.model.Unit unit)
            {
                return false;
            }
            // Java: 73
            public virtual void seat(com.oddlabs.tt.model.Unit unit)
            {
            }
            // Java: 77
            public virtual void exit(com.oddlabs.tt.model.Unit unit)
            {
            }
            // Java: 81
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
            // Java: 89
            public virtual void killAll()
            {
                unit.drown();
                unit = null;
            }
            // Java: 94
            public virtual java.util.List allUnits()
            {
                return new java.util.ArrayList();
            }
            // Java: 98
            public virtual com.oddlabs.tt.model.Unit findUnit(com.oddlabs.tt.model.UnitTemplate template)
            {
                return null;
            }
            // Java: 102
            public virtual bool needRowers()
            {
                return false;
            }
            // Java: 106
            public virtual bool canSparePeons()
            {
                return false;
            }
            // Java: 110
            public virtual int countRowers()
            {
                return 0;
            }
            // Java: 114
            public virtual bool empty()
            {
                return unit == null;
            }
            private void init_Rudder()
            {
                this.unit = null;
            }
            public Rudder(com.oddlabs.tt.model.ShipHR this_0_, com.oddlabs.tt.model.Ship ship, float x, float y, float z) : base()
            {
                this.this_0 = this_0_;
                init_Rudder();
                alloc = new com.oddlabs.tt.model.ShipAllocation(new org.joml.Vector3f(x, y, z), new org.joml.Vector2f(0.0f, 1.0f), com.oddlabs.tt.model.ShipAllocation.STEERING);
                com.oddlabs.tt.player.Player owner = ship.getOwner();
                unit = new com.oddlabs.tt.model.Unit(owner, x, y, null, owner.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON), null, false, false, true);
                unit.mount(ship, alloc);
            }
        }
        // Java: 119
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
            // Java: 144
            public virtual bool canFit(com.oddlabs.tt.model.Unit unit)
            {
                if (unit.isChieftain())
                {
                    return false;
                }
                float size = this_0.unitSize(unit);
                return (float)(total - used) > size;
            }
            // Java: 152
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
                    else if (!unit.isChieftain())
                    {
                        peons.add(unit);
                    }
                    reassign();
                }
            }
            // Java: 166
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
            // Java: 177
            public virtual void killAll()
            {
                for (int i = 0; i < all_units.size(); i++)
                {
                    com.oddlabs.tt.model.Unit unit = ((com.oddlabs.tt.model.Unit)all_units.get(i));
                    unit.drown();
                }
                all_units.clear();
                peons.clear();
                warriors.clear();
                allocs.clear();
            }
            // Java: 188
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
            // Java: 196
            public virtual java.util.List allUnits()
            {
                return all_units;
            }
            // Java: 200
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
            // Java: 210
            public virtual bool needRowers()
            {
                int required = ((left_rower ? 1 : 0)) + ((right_rower ? 1 : 0));
                return peons.size() < required;
            }
            // Java: 215
            public virtual bool canSparePeons()
            {
                int required = ((left_rower ? 1 : 0)) + ((right_rower ? 1 : 0));
                return peons.size() > required;
            }
            // Java: 220
            public virtual int countRowers()
            {
                int required = ((left_rower ? 1 : 0)) + ((right_rower ? 1 : 0));
                return java.lang.Math.min(peons.size(), required);
            }
            // Java: 225
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
            // Java: 291
            public virtual bool empty()
            {
                return all_units.size() == 0;
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
        // Java: 296
        public class UpperDeckRow : java.lang.Object, com.oddlabs.tt.model.ShipHR.Row
        {
            public com.oddlabs.tt.model.ShipHR this_0;
            public com.oddlabs.tt.model.Unit left;
            public com.oddlabs.tt.model.Unit right;
            public com.oddlabs.tt.model.ShipAllocation leftAlloc;
            public com.oddlabs.tt.model.ShipAllocation rightAlloc;
            public com.oddlabs.tt.model.ShipHR.Row alt;
            // Java: 303
            public virtual void setAlt(com.oddlabs.tt.model.ShipHR.Row row)
            {
                alt = row;
            }
            // Java: 312
            public virtual bool canFit(com.oddlabs.tt.model.Unit unit)
            {
                return unit.isWarrior() && !unit.isChieftain() && (left == null || right == null) && (alt == null || alt.empty());
            }
            // Java: 317
            public virtual void seat(com.oddlabs.tt.model.Unit unit)
            {
                if (canFit(unit))
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
            // Java: 327
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
            // Java: 335
            public virtual void killAll()
            {
                if (left != null)
                {
                    left.drown();
                    left = null;
                }
                if (right != null)
                {
                    right.drown();
                    right = null;
                }
            }
            // Java: 346
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
            // Java: 355
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
            // Java: 366
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
            // Java: 376
            public virtual bool needRowers()
            {
                return false;
            }
            // Java: 380
            public virtual bool canSparePeons()
            {
                return false;
            }
            // Java: 384
            public virtual int countRowers()
            {
                return 0;
            }
            // Java: 388
            public virtual bool empty()
            {
                return left == null && right == null;
            }
            private void init_UpperDeckRow()
            {
                this.left = null;
                this.right = null;
            }
            public UpperDeckRow(com.oddlabs.tt.model.ShipHR this_0_, float x, float y, float z) : base()
            {
                this.this_0 = this_0_;
                init_UpperDeckRow();
                leftAlloc = new com.oddlabs.tt.model.ShipAllocation(new org.joml.Vector3f(x, y, z), new org.joml.Vector2f(0.0f, 1.0f), com.oddlabs.tt.model.ShipAllocation.FIGHTING);
                rightAlloc = new com.oddlabs.tt.model.ShipAllocation(new org.joml.Vector3f(x, (float)(-y), z), new org.joml.Vector2f(0.0f, (float)(-1.0f)), com.oddlabs.tt.model.ShipAllocation.FIGHTING);
            }
        }
        // Java: 393
        public class ChieftainRow : java.lang.Object, com.oddlabs.tt.model.ShipHR.Row
        {
            public com.oddlabs.tt.model.ShipHR this_0;
            public com.oddlabs.tt.model.Unit unit;
            public com.oddlabs.tt.model.ShipAllocation alloc;
            public com.oddlabs.tt.model.ShipHR.Row alt1;
            public com.oddlabs.tt.model.ShipHR.Row alt2;
            // Java: 403
            public virtual void setAlts(com.oddlabs.tt.model.ShipHR.Row r1, com.oddlabs.tt.model.ShipHR.Row r2)
            {
                alt1 = r1;
                alt2 = r2;
            }
            // Java: 408
            public virtual bool canFit(com.oddlabs.tt.model.Unit unit)
            {
                return unit.isChieftain() && alt1.empty() && alt2.empty();
            }
            // Java: 412
            public virtual void seat(com.oddlabs.tt.model.Unit unit)
            {
                if (unit.isChieftain())
                {
                    this.unit = unit;
                }
            }
            // Java: 418
            public virtual void exit(com.oddlabs.tt.model.Unit unit)
            {
                if (((object)this.unit == (object)unit))
                {
                    this.unit = null;
                }
            }
            // Java: 424
            public virtual void killAll()
            {
                if (unit != null)
                {
                    unit.drown();
                }
                unit = null;
            }
            // Java: 431
            public virtual com.oddlabs.tt.model.ShipAllocation getAllocation(com.oddlabs.tt.model.Unit unit)
            {
                if (((object)this.unit == (object)unit))
                {
                    return alloc;
                }
                return null;
            }
            // Java: 438
            public virtual java.util.List allUnits()
            {
                java.util.List ret = new java.util.ArrayList();
                if (unit != null)
                {
                    ret.add(unit);
                }
                return ret;
            }
            // Java: 446
            public virtual com.oddlabs.tt.model.Unit findUnit(com.oddlabs.tt.model.UnitTemplate template)
            {
                return null;
            }
            // Java: 450
            public virtual bool needRowers()
            {
                return false;
            }
            // Java: 454
            public virtual bool canSparePeons()
            {
                return false;
            }
            // Java: 458
            public virtual int countRowers()
            {
                return 0;
            }
            // Java: 462
            public virtual bool empty()
            {
                return unit == null;
            }
            private void init_ChieftainRow()
            {
                this.unit = null;
                this.alt1 = null;
                this.alt2 = null;
            }
            public ChieftainRow(com.oddlabs.tt.model.ShipHR this_0_, float x, float y, float z) : base()
            {
                this.this_0 = this_0_;
                init_ChieftainRow();
                alloc = new com.oddlabs.tt.model.ShipAllocation(new org.joml.Vector3f(x, y, z), new org.joml.Vector2f(1.0f, 0.0f), com.oddlabs.tt.model.ShipAllocation.FIGHTING);
            }
        }
        public java.util.LinkedHashMap unit2row;
        public com.oddlabs.tt.model.ShipHR.ChieftainRow chieftainRow;
        public java.util.ArrayList rows;
        // Java: 550
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
        // Java: 559
        public com.oddlabs.tt.model.ShipAllocation tryAllocate(com.oddlabs.tt.model.Unit unit)
        {
            if (!unit.isWarrior() && !unit.isChieftain())
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
                        if (alloc.getRole() == com.oddlabs.tt.model.ShipAllocation.FIGHTING && !unit.isChieftain())
                        {
                            unit.increaseRange(com.oddlabs.tt.model.ShipHR.WARRIOR_INCREASE);
                        }
                        return alloc;
                    }
                }
            }
            return null;
        }
        // Java: 594
        public void killCrew()
        {
            for (int i = 0; i < rows.size(); i++)
            {
                ((com.oddlabs.tt.model.ShipHR.Row)rows.get(i)).killAll();
                unit2row.clear();
            }
        }
        // Java: 601
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
        // Java: 610
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
                            unit.increaseRange((float)(-com.oddlabs.tt.model.ShipHR.WARRIOR_INCREASE));
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
        // Java: 655
        public bool isRock(System.Type type)
        {
            return ((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)) || ((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RockSpearWeapon));
        }
        // Java: 659
        public bool isIron(System.Type type)
        {
            return ((object)type == (object)typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)) || ((object)type == (object)typeof(com.oddlabs.tt.model.weapon.IronSpearWeapon));
        }
        // Java: 663
        public bool isRubber(System.Type type)
        {
            return ((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)) || ((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RubberSpearWeapon));
        }
        // Java: 667
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
        // Java: 679
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
        // Java: 694
        public int countUnits()
        {
            return unit2row.size();
        }
        // Java: 698
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
        // Java: 710
        public com.oddlabs.tt.model.Unit pickVictim(float random)
        {
            int index = java.lang.StrictMath.round((float)(random * 200));
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
        // Java: 725
        public com.oddlabs.tt.model.Unit exitChieftain()
        {
            if (hasChieftain())
            {
                com.oddlabs.tt.model.Unit unit = ((com.oddlabs.tt.model.Unit)chieftainRow.allUnits().get(0));
                chieftainRow.exit(unit);
                unit2row.remove(unit);
                unit.setReference(null);
                unit.unmount();
                return unit;
            }
            return null;
        }
        // Java: 737
        public bool hasChieftain()
        {
            return !chieftainRow.empty();
        }
        // Java: 741
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
                rows.add(new com.oddlabs.tt.model.ShipHR.Rudder(this, ship, (float)(-11.14f), (float)(-0.43f), +0.54f));
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
                com.oddlabs.tt.model.ShipHR.UpperDeckRow r1 = new com.oddlabs.tt.model.ShipHR.UpperDeckRow(this, +9.64f, +1.17f, +3.29f);
                com.oddlabs.tt.model.ShipHR.UpperDeckRow r2 = new com.oddlabs.tt.model.ShipHR.UpperDeckRow(this, +7.89f, +1.36f, +3.29f);
                rows.add(r1);
                rows.add(r2);
                chieftainRow = new com.oddlabs.tt.model.ShipHR.ChieftainRow(this, +8.76f, +0.0f, +3.29f);
                rows.add(chieftainRow);
                chieftainRow.setAlts(r1, r2);
                r1.setAlt(chieftainRow);
                r2.setAlt(chieftainRow);
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow(this, +6.53f, +1.6f, +3.29f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow(this, +5.12f, +1.74f, +3.29f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow(this, +3.78f, +1.8f, +3.29f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow(this, +2.1f, +2.07f, +3.11f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow(this, +0.56f, +2.07f, +3.11f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow(this, (float)(-0.82f), +2.07f, +3.11f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow(this, (float)(-2.6f), +2.07f, +3.11f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow(this, (float)(-5.24f), +1.82f, +3.13f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow(this, (float)(-7.17f), +1.86f, +2.99f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow(this, (float)(-8.99f), +1.31f, +2.99f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow(this, (float)(-10.49f), +1.0f, +2.99f));
            }
            else
            {
                rows.add(new com.oddlabs.tt.model.ShipHR.Rudder(this, ship, (float)(-11.4f), +0.87f, +2.455f));
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
                com.oddlabs.tt.model.ShipHR.UpperDeckRow r1_2 = new com.oddlabs.tt.model.ShipHR.UpperDeckRow(this, +8.0f, +1.09f, +3.24f);
                com.oddlabs.tt.model.ShipHR.UpperDeckRow r2_2 = new com.oddlabs.tt.model.ShipHR.UpperDeckRow(this, +9.5f, +0.83f, +3.24f);
                rows.add(r1_2);
                rows.add(r2_2);
                chieftainRow = new com.oddlabs.tt.model.ShipHR.ChieftainRow(this, +8.75f, +0.0f, +3.24f);
                rows.add(chieftainRow);
                r1_2.setAlt(chieftainRow);
                r2_2.setAlt(chieftainRow);
                chieftainRow.setAlts(r1_2, r2_2);
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow(this, (float)(-9.39f), +1.01f, +3.41f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow(this, (float)(-7.75f), +1.22f, +3.41f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow(this, (float)(-6.02f), +1.35f, +3.41f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow(this, (float)(-3.93f), +1.22f, +3.24f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow(this, (float)(-2.5f), +1.22f, +3.24f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow(this, (float)(-1.0f), +1.22f, +3.24f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow(this, +0.5f, +1.22f, +3.24f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow(this, +2.0f, +1.22f, +3.24f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow(this, +3.5f, +1.22f, +3.24f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow(this, +5.0f, +1.22f, +3.24f));
                rows.add(new com.oddlabs.tt.model.ShipHR.UpperDeckRow(this, +6.5f, +1.22f, +3.24f));
            }
        }
    }
}
