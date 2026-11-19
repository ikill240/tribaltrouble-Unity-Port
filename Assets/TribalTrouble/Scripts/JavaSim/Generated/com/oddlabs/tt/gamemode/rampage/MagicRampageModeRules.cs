// Translated from MagicRampageModeRules.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gamemode.rampage
{
    // Java: 32
    public class MagicRampageModeRules : java.lang.Object, com.oddlabs.tt.gamemode.GameModeRules
    {
        public const int CHIEFTAINS = 10;
        public const int START_CHIEFTAINS = 3;
        public const float RECHARGE_BOOST = 2.0f;
        public const float HUNT_RADIUS = 45.0f;
        public string last_status;
        // Java: 40
        public java.util.List getOptions()
        {
            return java.util.List_.of();
        }
        // Java: 45
        public bool isPlayerAlive(com.oddlabs.tt.player.Player player)
        {
            int units = player.getUnitCountContainer().getNumSupplies();
            return units > 0 || player.hasActiveChieftain() || player.getQuarters() != null;
        }
        // Java: 51
        public void onGameStart(com.oddlabs.tt.viewer.WorldViewer viewer)
        {
            com.oddlabs.tt.gui.Label label = new com.oddlabs.tt.gui.Label("", com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont(), 500, com.oddlabs.tt.gui.Origin.AT_MIDDLE);
            label.setPos((viewer.getGUIRoot().getWidth() - label.getWidth()) / 2, 10);
            viewer.getGUIRoot().addChild(label);
            onWorldStart(viewer.getWorld(), new java.util.function.Consumer_L((object a0) => { label.setText(((string)a0)); }));
        }
        // Java: 59
        public string describe()
        {
            return last_status;
        }
        // Java: 64
        public void onWorldStart(com.oddlabs.tt.landscape.World world, java.util.function.Consumer status)
        {
            last_status = "";
            foreach (com.oddlabs.tt.player.Player p in world.getPlayers())
            {
                int team = p.getPlayerInfo().getTeam();
                if (team == com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL || team == com.oddlabs.tt.landscape.World.ZOMBIE_TEAM)
                {
                    continue;
                }
                int have = p.getTotalChieftainCount();
                for (int i = have; i < com.oddlabs.tt.gamemode.rampage.MagicRampageModeRules.START_CHIEFTAINS; i++)
                {
                    com.oddlabs.tt.model.Unit chief = new com.oddlabs.tt.model.Unit(p, (float)((float)(p.getStartX() + 3.0f) + (float)(2.0f * i)), (float)(p.getStartY() + 3.0f), null, p.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_CHIEFTAIN), "Chieftain", false);
                    for (int m = 0; m < com.oddlabs.tt.model.RacesResources.NUM_MAGIC; m++)
                    {
                        chief.increaseMagicEnergy(m, 1000);
                    }
                    if (!p.hasActiveChieftain() || p.getChieftain() == null)
                    {
                        p.setActiveChieftain(chief);
                    }
                    else
                    {
                        p.addExtraChieftain(chief);
                    }
                }
            }
            world.getAnimationManagerGameTime().registerAnimation(new com.oddlabs.tt.gamemode.rampage.MagicRampageModeRules.Surge(this, world, status));
        }
        // Java: 86
        public class Surge : java.lang.Object, com.oddlabs.tt.animation.Animated
        {
            public com.oddlabs.tt.gamemode.rampage.MagicRampageModeRules this_0;
            public com.oddlabs.tt.landscape.World world;
            public java.util.function.Consumer status;
            public float time;
            public float next_hunt;
            // Java: 97
            public void animate(float t)
            {
                time += t;
                int chiefs = 0;
                foreach (com.oddlabs.tt.player.Player p in world.getPlayers())
                {
                    foreach (com.oddlabs.tt.model.Unit c in java.lang.JIter.of(chieftains(p)))
                    {
                        chiefs++;
                        for (int m = 0; m < com.oddlabs.tt.model.RacesResources.NUM_MAGIC; m++)
                        {
                            c.increaseMagicEnergy(m, (float)(com.oddlabs.tt.gamemode.rampage.MagicRampageModeRules.RECHARGE_BOOST * t));
                        }
                    }
                }
                if (time >= next_hunt)
                {
                    next_hunt = (float)(time + 6.0f);
                    foreach (com.oddlabs.tt.player.Player p_2 in world.getPlayers())
                    {
                        hunt(p_2);
                    }
                }
                string text = "Magic Rampage - chieftains on the field: " + (chiefs);
                if (!java.lang.JO.equals(text, this_0.last_status))
                {
                    this_0.last_status = text;
                    if (status != null)
                    {
                        status.accept(text);
                    }
                }
            }
            // Java: 121
            public java.util.List chieftains(com.oddlabs.tt.player.Player p)
            {
                java.util.List list = new java.util.ArrayList();
                com.oddlabs.tt.model.Unit c = p.getChieftain();
                if (c != null && !c.isDead())
                {
                    list.add(c);
                }
                list.addAll(p.getExtraChieftains());
                return list;
            }
            // Java: 131
            public void hunt(com.oddlabs.tt.player.Player p)
            {
                if (p.getAI() == null)
                {
                    return;
                }
                foreach (com.oddlabs.tt.model.Unit c in java.lang.JIter.of(p.getExtraChieftains()))
                {
                    if (c.isMoving() || c.isMounted())
                    {
                        continue;
                    }
                    com.oddlabs.tt.model.Unit prey = null;
                    float best = (float)(com.oddlabs.tt.gamemode.rampage.MagicRampageModeRules.HUNT_RADIUS * com.oddlabs.tt.gamemode.rampage.MagicRampageModeRules.HUNT_RADIUS);
                    foreach (com.oddlabs.tt.player.Player e in world.getPlayers())
                    {
                        if (!p.isEnemy(e))
                        {
                            continue;
                        }
                        foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(e.getUnits().getSet()))
                        {
                            if ((s is com.oddlabs.tt.model.Unit u) && !u.isDead() && !u.isMounted())
                            {
                                float dx = (float)(u.getPositionX() - c.getPositionX());
                                float dy = (float)(u.getPositionY() - c.getPositionY());
                                float d = (float)((float)(dx * dx) + (float)(dy * dy));
                                if (d < best)
                                {
                                    best = d;
                                    prey = u;
                                }
                            }
                        }
                    }
                    if (prey != null)
                    {
                        p.setLandscapeTarget(new com.oddlabs.tt.model.Selectable[] { c }, com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(prey.getPositionX()), com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(prey.getPositionY()), com.oddlabs.tt.model.Action.ATTACK, true);
                    }
                }
            }
            private void init_Surge()
            {
                this.next_hunt = 6.0f;
            }
            public Surge(com.oddlabs.tt.gamemode.rampage.MagicRampageModeRules this_0_, com.oddlabs.tt.landscape.World world, java.util.function.Consumer status) : base()
            {
                this.this_0 = this_0_;
                init_Surge();
                this.world = world;
                this.status = status;
            }
            // mixin of default method Animated.updateChecksum
            public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
            {
            }
        }
        private void init_MagicRampageModeRules()
        {
            this.last_status = "";
        }
        public MagicRampageModeRules() : base()
        {
            init_MagicRampageModeRules();
        }
    }
}
