// Translated from Reflexes.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.gauntlet
{
    // Java: 57
    public class Reflexes : java.lang.Object
    {
        public const float TICK_SECONDS = 0.02f;
        public com.oddlabs.tt.player.gauntlet.GauntletAI ai;
        public bool swing_restart;
        public bool stun_cancel;
        public bool tower_unstun;
        public java.util.Map tower_last;
        public java.util.Map swings;
        public java.util.Map release_ticks;
        public java.util.List due;
        public java.util.List stunned;
        public int tick_f;
        // Java: 75
        public class Swing : java.lang.Object
        {
            public com.oddlabs.tt.model.behaviour.Behaviour behaviour;
            public int seen;
            public Swing(com.oddlabs.tt.model.behaviour.Behaviour behaviour, int seen) : base()
            {
                this.behaviour = behaviour;
                this.seen = seen;
            }
        }
        // Java: 92
        public void tick()
        {
            if (!swing_restart && !stun_cancel && !tower_unstun)
            {
                return;
            }
            tick_f++;
            if (tower_unstun)
            {
                towerUnstun();
            }
            com.oddlabs.tt.player.Player me = ai.owner();
            due.clear();
            stunned.clear();
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(me.getUnits().getSet()))
            {
                if (!((s is com.oddlabs.tt.model.Unit u)) || u.isDead() || u.isMounted())
                {
                    continue;
                }
                if (stun_cancel && (u.getCurrentController() is com.oddlabs.tt.model.behaviour.StunController))
                {
                    stunned.add(u);
                    continue;
                }
                if (!swing_restart)
                {
                    continue;
                }
                com.oddlabs.tt.model.behaviour.Behaviour b = u.getCurrentBehaviour();
                if (!((b is com.oddlabs.tt.model.behaviour.HarvestBehaviour)))
                {
                    continue;
                }
                com.oddlabs.tt.player.gauntlet.Reflexes.Swing sw = ((com.oddlabs.tt.player.gauntlet.Reflexes.Swing)swings.get(u));
                if (sw == null || ((object)sw.behaviour != (object)b))
                {
                    swings.put(u, new com.oddlabs.tt.player.gauntlet.Reflexes.Swing(b, tick_f));
                    continue;
                }
                if (tick_f - sw.seen >= releaseTicks(u))
                {
                    due.add(u);
                }
            }
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(stunned))
            {
                unstun(u_2);
            }
            foreach (com.oddlabs.tt.model.Unit u_3 in java.lang.JIter.of(due))
            {
                restart(u_3);
            }
            if (tick_f % 250 == 0)
            {
                swings.keySet().removeIf(new java.util.function.Predicate_L((object a0) => { return ((com.oddlabs.tt.model.Unit)a0).isDead(); }));
            }
        }
        // Java: 129
        public int releaseTicks(com.oddlabs.tt.model.Unit u)
        {
            float release = u.getWeaponFactory().getSecondsPerRelease(1.0f);
            int? n = ((int?)release_ticks.get(release));
            if (n == null)
            {
                float sum = 0.0f;
                int ticks = 0;
                while (sum <= release)
                {
                    sum = (float)(sum + com.oddlabs.tt.player.gauntlet.Reflexes.TICK_SECONDS);
                    ticks++;
                }
                n = ticks;
                release_ticks.put(release, n);
            }
            return ((int)n);
        }
        // Java: 146
        public void restart(com.oddlabs.tt.model.Unit u)
        {
            swings.remove(u);
            if (u.isDead() || u.isMounted() || !((u.getCurrentBehaviour() is com.oddlabs.tt.model.behaviour.HarvestBehaviour)))
            {
                return;
            }
            com.oddlabs.tt.model.behaviour.Controller primary = u.getPrimaryController();
            if ((primary is com.oddlabs.tt.model.behaviour.GatherController gather))
            {
                com.oddlabs.tt.model.Supply supply = gather.getSupply();
                if (supply == null || supply.isDead())
                {
                    return;
                }
                ai.owner().setTarget(com.oddlabs.tt.model.Selectable.newArray(u), supply, com.oddlabs.tt.model.Action.GATHER_REPAIR, false);
                ai.aiLog().count("swing_restart");
            }
            else if ((primary is com.oddlabs.tt.model.behaviour.RepairController repair))
            {
                com.oddlabs.tt.model.Building b = repair.getBuilding();
                if (b == null || b.isDead())
                {
                    return;
                }
                ai.owner().setTarget(com.oddlabs.tt.model.Selectable.newArray(u), b, com.oddlabs.tt.model.Action.GATHER_REPAIR, false);
                ai.aiLog().count("swing_restart_build");
            }
        }
        // Java: 167
        public void unstun(com.oddlabs.tt.model.Unit u)
        {
            com.oddlabs.tt.model.behaviour.Controller primary = u.getPrimaryController();
            if ((primary is com.oddlabs.tt.model.behaviour.MagicController))
            {
                return;
            }
            com.oddlabs.tt.util.Target target = null;
            com.oddlabs.tt.model.Action action = com.oddlabs.tt.model.Action.DEFAULT;
            bool aggressive = false;
            if ((primary is com.oddlabs.tt.model.behaviour.GatherController gather) && gather.getSupply() != null)
            {
                target = gather.getSupply();
                action = com.oddlabs.tt.model.Action.GATHER_REPAIR;
            }
            else if ((primary is com.oddlabs.tt.model.behaviour.RepairController repair) && repair.getBuilding() != null)
            {
                target = repair.getBuilding();
                action = com.oddlabs.tt.model.Action.GATHER_REPAIR;
            }
            else if ((primary is com.oddlabs.tt.model.behaviour.PlaceBuildingController place) && place.getBuilding() != null)
            {
                target = place.getBuilding();
            }
            else if ((primary is com.oddlabs.tt.model.behaviour.WalkController walk))
            {
                target = walk.getTarget();
                aggressive = walk.isAgressive();
                action = (aggressive ? com.oddlabs.tt.model.Action.DEFAULT : com.oddlabs.tt.model.Action.MOVE);
            }
            else if ((primary is com.oddlabs.tt.model.behaviour.HuntController hunt))
            {
                target = hunt.getTarget();
                action = com.oddlabs.tt.model.Action.ATTACK;
            }
            else if ((primary is com.oddlabs.tt.model.behaviour.AttackController attack))
            {
                target = attack.getTarget();
                action = com.oddlabs.tt.model.Action.ATTACK;
            }
            if (target == null || target.isDead() || ((object)target == (object)u))
            {
                bool fighter = !u.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD);
                target = new com.oddlabs.tt.landscape.LandscapeTarget(u.getGridX(), u.getGridY());
                action = (fighter ? com.oddlabs.tt.model.Action.ATTACK : com.oddlabs.tt.model.Action.MOVE);
                aggressive = fighter;
            }
            ai.owner().setTarget(com.oddlabs.tt.model.Selectable.newArray(u), target, action, aggressive);
            ai.aiLog().count("stun_cancel");
        }
        // Java: 205
        public void towerUnstun()
        {
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(ai.intel().towers))
            {
                if (t.isDead() || !t.isComplete() || t.getUnitContainer() == null || t.getUnitContainer().getNumSupplies() == 0)
                {
                    continue;
                }
                com.oddlabs.tt.model.Unit gunner = com.oddlabs.tt.player.gauntlet.Intel.gunner(t);
                if (gunner == null || gunner.isDead())
                {
                    continue;
                }
                com.oddlabs.tt.model.behaviour.Controller current = gunner.getCurrentController();
                com.oddlabs.tt.model.behaviour.Controller previous = ((com.oddlabs.tt.model.behaviour.Controller)tower_last.put(t, current));
                if (!((current is com.oddlabs.tt.model.behaviour.StunController)) || ((object)current == (object)previous))
                {
                    continue;
                }
                com.oddlabs.tt.model.Selectable target = ai.military().towerTargetFor(t, gunner);
                if (target == null)
                {
                    continue;
                }
                ai.owner().setTarget(com.oddlabs.tt.model.Selectable.newArray(t), target, com.oddlabs.tt.model.Action.ATTACK, false);
                ai.aiLog().count("tower_unstun");
            }
            if (tick_f % 250 == 0)
            {
                tower_last.keySet().removeIf(new java.util.function.Predicate_L((object a0) => { return ((com.oddlabs.tt.model.Building)a0).isDead(); }));
            }
        }
        private void init_Reflexes()
        {
            this.tower_last = new java.util.LinkedHashMap();
            this.swings = new java.util.LinkedHashMap();
            this.release_ticks = new java.util.LinkedHashMap();
            this.due = new java.util.ArrayList();
            this.stunned = new java.util.ArrayList();
        }
        public Reflexes(com.oddlabs.tt.player.gauntlet.GauntletAI ai, bool swing_restart, bool stun_cancel, bool tower_unstun) : base()
        {
            init_Reflexes();
            this.ai = ai;
            this.swing_restart = swing_restart;
            this.stun_cancel = stun_cancel;
            this.tower_unstun = tower_unstun;
        }
    }
}
