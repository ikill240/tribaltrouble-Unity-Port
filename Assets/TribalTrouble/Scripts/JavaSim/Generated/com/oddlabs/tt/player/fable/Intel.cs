// Translated from Intel.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.fable
{
    // Java: 24
    public class Intel : java.lang.Object
    {
        public const float VALUE_PEON = 0.25f;
        public const float VALUE_ROCK = 0.6f;
        public const float VALUE_IRON = 1.0f;
        public const float VALUE_RUBBER = 1.6f;
        public const float VALUE_CHIEFTAIN = 6.0f;
        public const float VALUE_MANNED_TOWER = 4.0f;
        public com.oddlabs.tt.player.Player me;
        public java.util.List warriors;
        public java.util.List peons;
        public java.util.List chieftains;
        public java.util.List armories;
        public java.util.List quarters;
        public java.util.List towers;
        public java.util.List manned_towers;
        public java.util.List construction_sites;
        public float warrior_value;
        public int unit_count;
        // Java: 53
        public void refresh()
        {
            warriors.clear();
            peons.clear();
            chieftains.clear();
            armories.clear();
            quarters.clear();
            towers.clear();
            manned_towers.clear();
            construction_sites.clear();
            warrior_value = 0.0f;
            unit_count = 0;
            foreach (com.oddlabs.tt.player.Player p in me.getWorld().getPlayers())
            {
                if (!me.isEnemy(p))
                {
                    continue;
                }
                foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(p.getUnits().getSet()))
                {
                    if (s.isDead())
                    {
                        continue;
                    }
                    if ((s is com.oddlabs.tt.model.Unit u))
                    {
                        unit_count++;
                        if (u.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC))
                        {
                            chieftains.add(u);
                        }
                        else if (u.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD))
                        {
                            peons.add(u);
                        }
                        else if (u.getAbilities().isWarrior())
                        {
                            warriors.add(u);
                            warrior_value += com.oddlabs.tt.player.fable.Intel.unitValue(u);
                        }
                    }
                    else if ((s is com.oddlabs.tt.model.LandBuilding b))
                    {
                        if (!b.isComplete())
                        {
                            construction_sites.add(b);
                        }
                        else if (b.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD_ARMIES))
                        {
                            armories.add(b);
                        }
                        else if (b.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.REPRODUCE))
                        {
                            quarters.add(b);
                        }
                        else if (b.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.ATTACK))
                        {
                            towers.add(b);
                            if (b.getUnitContainer() != null && b.getUnitContainer().getNumSupplies() > 0)
                            {
                                manned_towers.add(b);
                            }
                        }
                    }
                }
            }
        }
        // Java: 98
        public static float unitValue(com.oddlabs.tt.model.Unit u)
        {
            if (u.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC))
            {
                return com.oddlabs.tt.player.fable.Intel.VALUE_CHIEFTAIN;
            }
            if (u.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD))
            {
                return com.oddlabs.tt.player.fable.Intel.VALUE_PEON;
            }
            System.Type type = u.getWeaponFactory().getType();
            if (((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)) || ((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RockSpearWeapon)))
            {
                return com.oddlabs.tt.player.fable.Intel.VALUE_ROCK;
            }
            if (((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)) || ((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RubberSpearWeapon)))
            {
                return com.oddlabs.tt.player.fable.Intel.VALUE_RUBBER;
            }
            return com.oddlabs.tt.player.fable.Intel.VALUE_IRON;
        }
        // Java: 111
        public bool hasBuildings()
        {
            return !armories.isEmpty() || !quarters.isEmpty() || !towers.isEmpty() || !construction_sites.isEmpty();
        }
        // Java: 116
        public float strengthNear(int gx, int gy, int radius)
        {
            int r2 = radius * radius;
            float sum = 0.0f;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(warriors))
            {
                if (com.oddlabs.tt.player.fable.Intel.dist2(u, gx, gy) <= r2)
                {
                    sum = (float)(sum + com.oddlabs.tt.player.fable.Intel.unitValue(u));
                }
            }
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(chieftains))
            {
                if (com.oddlabs.tt.player.fable.Intel.dist2(u_2, gx, gy) <= r2)
                {
                    sum = (float)(sum + com.oddlabs.tt.player.fable.Intel.VALUE_CHIEFTAIN);
                }
            }
            foreach (com.oddlabs.tt.model.LandBuilding t in java.lang.JIter.of(manned_towers))
            {
                if (com.oddlabs.tt.player.fable.Intel.dist2(t, gx, gy) <= r2)
                {
                    sum = (float)(sum + com.oddlabs.tt.player.fable.Intel.VALUE_MANNED_TOWER);
                }
            }
            return sum;
        }
        // Java: 131
        public int warriorsNear(int gx, int gy, int radius)
        {
            int r2 = radius * radius;
            int n = 0;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(warriors))
            {
                if (com.oddlabs.tt.player.fable.Intel.dist2(u, gx, gy) <= r2)
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 140
        public int unitsNear(int gx, int gy, int radius)
        {
            int r2 = radius * radius;
            int n = 0;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(warriors))
            {
                if (com.oddlabs.tt.player.fable.Intel.dist2(u, gx, gy) <= r2)
                {
                    n++;
                }
            }
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(peons))
            {
                if (com.oddlabs.tt.player.fable.Intel.dist2(u_2, gx, gy) <= r2)
                {
                    n++;
                }
            }
            foreach (com.oddlabs.tt.model.Unit u_3 in java.lang.JIter.of(chieftains))
            {
                if (com.oddlabs.tt.player.fable.Intel.dist2(u_3, gx, gy) <= r2)
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 155
        public com.oddlabs.tt.model.Unit nearestWarrior(int gx, int gy)
        {
            return ((com.oddlabs.tt.model.Unit)com.oddlabs.tt.player.fable.Intel.nearest(warriors, gx, gy));
        }
        // Java: 159
        public com.oddlabs.tt.model.LandBuilding nearestBuilding(int gx, int gy)
        {
            com.oddlabs.tt.model.LandBuilding best = null;
            int best_d2 = java.lang.Integer.MAX_VALUE;
            foreach (java.util.List list in java.lang.JIter.of(java.util.List_.of(armories, quarters, towers, construction_sites)))
            {
                foreach (com.oddlabs.tt.model.LandBuilding b in java.lang.JIter.of(list))
                {
                    int d2 = com.oddlabs.tt.player.fable.Intel.dist2(b, gx, gy);
                    if (d2 < best_d2)
                    {
                        best_d2 = d2;
                        best = b;
                    }
                }
            }
            return best;
        }
        // Java: 174
        public static com.oddlabs.tt.model.Selectable nearest(java.util.List list, int gx, int gy)
        {
            com.oddlabs.tt.model.Selectable best = null;
            int best_d2 = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(list))
            {
                int d2 = com.oddlabs.tt.player.fable.Intel.dist2(s, gx, gy);
                if (d2 < best_d2)
                {
                    best_d2 = d2;
                    best = s;
                }
            }
            return best;
        }
        // Java: 187
        public static int dist2(com.oddlabs.tt.model.Selectable s, int gx, int gy)
        {
            int dx = s.getGridX() - gx;
            int dy = s.getGridY() - gy;
            return dx * dx + dy * dy;
        }
        // Java: 194
        public int[] armyCentroid(int gx, int gy, int radius)
        {
            int r2 = radius * radius;
            long sx = 0;
            long sy = 0;
            int n = 0;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(warriors))
            {
                if (com.oddlabs.tt.player.fable.Intel.dist2(u, gx, gy) <= r2)
                {
                    sx += u.getGridX();
                    sy += u.getGridY();
                    n++;
                }
            }
            if (n == 0)
            {
                return null;
            }
            return new int[] { unchecked((int)((sx / n))), unchecked((int)((sy / n))) };
        }
        // Java: 210
        public com.oddlabs.tt.model.Building enemyBuildingAt(int gx, int gy)
        {
            foreach (java.util.List list in java.lang.JIter.of(java.util.List_.of(armories, quarters, towers, construction_sites)))
            {
                foreach (com.oddlabs.tt.model.LandBuilding b in java.lang.JIter.of(list))
                {
                    if (b.getGridX() == gx && b.getGridY() == gy)
                    {
                        return b;
                    }
                }
            }
            return null;
        }
        private void init_Intel()
        {
            this.warriors = new java.util.ArrayList();
            this.peons = new java.util.ArrayList();
            this.chieftains = new java.util.ArrayList();
            this.armories = new java.util.ArrayList();
            this.quarters = new java.util.ArrayList();
            this.towers = new java.util.ArrayList();
            this.manned_towers = new java.util.ArrayList();
            this.construction_sites = new java.util.ArrayList();
        }
        public Intel(com.oddlabs.tt.player.Player me) : base()
        {
            init_Intel();
            this.me = me;
        }
    }
}
