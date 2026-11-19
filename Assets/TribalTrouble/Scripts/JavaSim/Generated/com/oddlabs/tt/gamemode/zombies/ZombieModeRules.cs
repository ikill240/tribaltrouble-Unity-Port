// Translated from ZombieModeRules.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gamemode.zombies
{
    // Java: 38
    public class ZombieModeRules : java.lang.Object, com.oddlabs.tt.gamemode.GameModeRules
    {
        public const float RISE_SECONDS = 3.0f;
        public const float WAVE_SECONDS = 90.0f;
        public const float DRIVE_SECONDS = 1.5f;
        public const float HUNT_RADIUS = 60.0f;
        public const float BOSS_HUNT_RADIUS = 90.0f;
        public const float BOSS_RISE_SECONDS = 4.0f;
        public string last_status;
        // Java: 48
        public java.util.List getOptions()
        {
            return java.util.List_.of();
        }
        // Java: 53
        public bool isPlayerAlive(com.oddlabs.tt.player.Player player)
        {
            if (player.getPlayerInfo().getTeam() == com.oddlabs.tt.landscape.World.ZOMBIE_TEAM)
            {
                return false;
            }
            int units = player.getUnitCountContainer().getNumSupplies();
            return units > 0 || player.hasActiveChieftain() || player.getQuarters() != null;
        }
        // Java: 61
        public void onGameStart(com.oddlabs.tt.viewer.WorldViewer viewer)
        {
            com.oddlabs.tt.gui.Label label = new com.oddlabs.tt.gui.Label("", com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont(), 500, com.oddlabs.tt.gui.Origin.AT_MIDDLE);
            label.setPos((viewer.getGUIRoot().getWidth() - label.getWidth()) / 2, 10);
            viewer.getGUIRoot().addChild(label);
            campaign_start = viewer.getInGameInfo().isCampaign();
            onWorldStart(viewer.getWorld(), new java.util.function.Consumer_L((object a0) => { label.setText(((string)a0)); }));
            campaign_start = false;
        }
        // Java: 69
        public string describe()
        {
            return last_status;
        }
        // Java: 74
        // skirmish and multiplayer start with a single zombie (patient zero); a campaign island keeps its horde
        public bool campaign_start;
        public void onWorldStart(com.oddlabs.tt.landscape.World world, java.util.function.Consumer status)
        {
            last_status = "";
            com.oddlabs.tt.player.Player dead = world.getZombiePlayer();
            if (dead == null)
            {
                return;
            }
            com.oddlabs.tt.gamemode.zombies.ZombieModeRules.Horde horde = new com.oddlabs.tt.gamemode.zombies.ZombieModeRules.Horde(this, world, dead, status);
            horde.campaign = campaign_start;
            world.setFallListener(new com.oddlabs.tt.landscape.World.FallListener_L((com.oddlabs.tt.model.Unit a0) => { horde.fell(a0); }));
            world.getAnimationManagerGameTime().registerAnimation(horde);
        }
        // Java: 85
        public class Horde : java.lang.Object, com.oddlabs.tt.animation.Animated
        {
            public com.oddlabs.tt.gamemode.zombies.ZombieModeRules this_0;
            public com.oddlabs.tt.landscape.World world;
            public com.oddlabs.tt.player.Player dead;
            public java.util.function.Consumer status;
            public java.util.List rising;
            public float time;
            public float drive;
            public float wave_timer;
            public int wave;
            public bool seeded;
            public bool campaign;
            public float boss_note;
            // Java: 103
            public void fell(com.oddlabs.tt.model.Unit u)
            {
                if (((object)u.getOwner() == (object)dead) || u.isMounted())
                {
                    return;
                }
                com.oddlabs.tt.model.Race race = u.getOwner().getRace();
                com.oddlabs.tt.model.UnitTemplate t = ((com.oddlabs.tt.model.UnitTemplate)u.getTemplate());
                bool plain = u.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.HARVEST) || ((object)t == (object)race.getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK)) || ((object)t == (object)race.getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON)) || ((object)t == (object)race.getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
                bool boss = ((object)t == (object)race.getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_CHIEFTAIN)) && ((object)u.getKilledBy() == (object)dead);
                rising.add(new float[] { (boss ? com.oddlabs.tt.gamemode.zombies.ZombieModeRules.BOSS_RISE_SECONDS : com.oddlabs.tt.gamemode.zombies.ZombieModeRules.RISE_SECONDS), u.getPositionX(), u.getPositionY(), (boss ? 2.0f : (plain ? 0.0f : 1.0f)), u.getOwner().getPlayerInfo().getRace() });
            }
            // Java: 116
            public void raise(float x, float y, int kind, int race_index)
            {
                com.oddlabs.tt.model.Race race = world.getRacesResources().getRace(race_index);
                new com.oddlabs.tt.model.Unit(dead, x, y, null, race.getUnitTemplate((kind == 2 ? com.oddlabs.tt.model.Race.UNIT_ZOMBIE_BOSS : (kind == 1 ? com.oddlabs.tt.model.Race.UNIT_ZOMBIE_BRUTE : com.oddlabs.tt.model.Race.UNIT_ZOMBIE))));
                if (kind == 2)
                {
                    boss_note = 8.0f;
                }
            }
            // Java: 124
            public void animate(float t)
            {
                time += t;
                if (!seeded)
                {
                    seeded = true;
                    if (campaign)
                    {
                        spawnWave(2);
                    }
                    else
                    {
                        spawnDead(1);
                    }
                }
                for (int i = 0; i < rising.size(); i++)
                {
                    float[] r = ((float[])rising.get(i));
                    r[0] -= t;
                    if (r[0] > 0.0f)
                    {
                        continue;
                    }
                    rising.remove(i--);
                    raise(r[1], r[2], java.lang.JC.f2i(r[3]), java.lang.JC.f2i(r[4]));
                }
                wave_timer -= t;
                if (wave_timer <= 0.0f)
                {
                    wave_timer = com.oddlabs.tt.gamemode.zombies.ZombieModeRules.WAVE_SECONDS;
                    wave++;
                    spawnWave(1 + wave / 2);
                }
                drive -= t;
                if (drive <= 0.0f)
                {
                    drive = com.oddlabs.tt.gamemode.zombies.ZombieModeRules.DRIVE_SECONDS;
                    hunt();
                }
                int count = 0;
                int bosses = 0;
                foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(dead.getUnits().getSet()))
                {
                    if ((s is com.oddlabs.tt.model.Unit u) && !u.isDead())
                    {
                        count++;
                        if (isBoss(u))
                        {
                            bosses++;
                        }
                    }
                }
                boss_note -= t;
                int m = java.lang.JC.f2i(((float)(time / 60.0f)));
                int sec = java.lang.JC.f2i(((float)(time % 60.0f)));
                string text = "Wave " + (wave) + " - the dead: " + (count) + " - survived " + (m) + ":" + java.lang.JStr.valueOf(((sec < 10 ? "0" : ""))) + (sec);
                if (boss_note > 0.0f)
                {
                    text = "A chieftain has risen from the dead! - " + java.lang.JStr.valueOf(text);
                }
                else if (bosses > 0)
                {
                    text += " - risen chieftains: " + (bosses);
                }
                if (!java.lang.JO.equals(text, this_0.last_status))
                {
                    this_0.last_status = text;
                    if (status != null)
                    {
                        status.accept(text);
                    }
                }
            }
            // Java: 172
            public void spawnWave(int per_player)
            {
                int living = 0;
                foreach (com.oddlabs.tt.player.Player p in world.getPlayers())
                {
                    if (((object)p != (object)dead) && p.isAlive())
                    {
                        living++;
                    }
                }
                spawnDead(per_player * living);
            }
            public void spawnDead(int count)
            {
                float size = world.getHeightMap().getMetersPerWorld();
                float sea = world.getHeightMap().getSeaLevelMeters();
                for (int n = 0; n < count; n++)
                {
                    for (int tries = 0; tries < 20; tries++)
                    {
                        float x = (float)(12.0f + (float)(world.getRandom().nextFloat() * ((float)(size - 24.0f))));
                        float y = (float)(12.0f + (float)(world.getRandom().nextFloat() * ((float)(size - 24.0f))));
                        if (world.getHeightMap().getNearestHeight(x, y) <= (float)(sea + 0.5f) || nearBuilding(x, y, 30.0f))
                        {
                            continue;
                        }
                        raise(x, y, (world.getRandom().nextFloat() < (float)(0.2f + (float)(0.05f * wave)) ? 1 : 0), world.getRandom().nextInt(2));
                        break;
                    }
                }
            }
            // Java: 192
            public bool nearBuilding(float x, float y, float r)
            {
                foreach (com.oddlabs.tt.player.Player p in world.getPlayers())
                {
                    if (((object)p == (object)dead))
                    {
                        continue;
                    }
                    foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(p.getUnits().getSet()))
                    {
                        if ((s is com.oddlabs.tt.model.Building b) && !b.isDead() && (float)(java.lang.Math.abs((float)(b.getPositionX() - x)) + java.lang.Math.abs((float)(b.getPositionY() - y))) < r)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            // Java: 208
            public void hunt()
            {
                java.util.List zombies = new java.util.ArrayList();
                foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(dead.getUnits().getSet()))
                {
                    if ((s is com.oddlabs.tt.model.Unit u) && !u.isDead() && !u.isMoving() && (u.getCurrentController() is com.oddlabs.tt.model.behaviour.IdleController))
                    {
                        zombies.add(u);
                    }
                }
                foreach (com.oddlabs.tt.model.Unit z in java.lang.JIter.of(zombies))
                {
                    bool brute = isBrute(z) || isBoss(z);
                    com.oddlabs.tt.model.Selectable prey = nearestLiving(z, brute, (isBoss(z) ? com.oddlabs.tt.gamemode.zombies.ZombieModeRules.BOSS_HUNT_RADIUS : com.oddlabs.tt.gamemode.zombies.ZombieModeRules.HUNT_RADIUS));
                    if (prey == null)
                    {
                        continue;
                    }
                    float dx = (float)(prey.getPositionX() - z.getPositionX());
                    float dy = (float)(prey.getPositionY() - z.getPositionY());
                    if ((float)((float)(dx * dx) + (float)(dy * dy)) < 4.0f)
                    {
                        continue;
                    }
                    if ((prey is com.oddlabs.tt.model.Building))
                    {
                        dead.setTarget(new com.oddlabs.tt.model.Selectable[] { z }, prey, com.oddlabs.tt.model.Action.ATTACK, false);
                    }
                    else
                    {
                        dead.setLandscapeTarget(new com.oddlabs.tt.model.Selectable[] { z }, com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(prey.getPositionX()), com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(prey.getPositionY()), com.oddlabs.tt.model.Action.ATTACK, true);
                    }
                }
            }
            // Java: 230
            public bool isBrute(com.oddlabs.tt.model.Unit z)
            {
                for (int r = 0; r < 2; r++)
                {
                    if (((object)((com.oddlabs.tt.model.UnitTemplate)z.getTemplate()) == (object)world.getRacesResources().getRace(r).getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_ZOMBIE_BRUTE)))
                    {
                        return true;
                    }
                }
                return false;
            }
            // Java: 237
            public bool isBoss(com.oddlabs.tt.model.Unit z)
            {
                for (int r = 0; r < 2; r++)
                {
                    if (((object)((com.oddlabs.tt.model.UnitTemplate)z.getTemplate()) == (object)world.getRacesResources().getRace(r).getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_ZOMBIE_BOSS)))
                    {
                        return true;
                    }
                }
                return false;
            }
            // Java: 244
            public com.oddlabs.tt.model.Selectable nearestLiving(com.oddlabs.tt.model.Unit z, bool brute, float radius)
            {
                com.oddlabs.tt.pathfinder.FindOccupantFilter units = new com.oddlabs.tt.pathfinder.FindOccupantFilter(z.getPositionX(), z.getPositionY(), radius, z, typeof(com.oddlabs.tt.model.Unit));
                world.getUnitGrid().scan(units, z.getGridX(), z.getGridY());
                com.oddlabs.tt.model.Selectable best = null;
                float best_d = java.lang.Float.MAX_VALUE;
                foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(units.getResult()))
                {
                    if (u.isDead() || u.isMounted() || ((object)u.getOwner() == (object)dead))
                    {
                        continue;
                    }
                    float d = d2(u, z);
                    if (d < best_d)
                    {
                        best_d = d;
                        best = u;
                    }
                }
                if (brute || best == null)
                {
                    foreach (com.oddlabs.tt.player.Player p in world.getPlayers())
                    {
                        if (((object)p == (object)dead))
                        {
                            continue;
                        }
                        foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(p.getUnits().getSet()))
                        {
                            if ((s is com.oddlabs.tt.model.Building b) && !b.isDead() && !((b is com.oddlabs.tt.model.Ship)) && d2(b, z) < best_d && d2(b, z) < (float)(radius * radius))
                            {
                                best_d = d2(b, z);
                                best = b;
                            }
                        }
                    }
                }
                return best;
            }
            // Java: 274
            public float d2(com.oddlabs.tt.model.Selectable a, com.oddlabs.tt.model.Selectable b)
            {
                float dx = (float)(a.getPositionX() - b.getPositionX());
                float dy = (float)(a.getPositionY() - b.getPositionY());
                return (float)((float)(dx * dx) + (float)(dy * dy));
            }
            private void init_Horde()
            {
                this.rising = new java.util.ArrayList();
                this.wave_timer = com.oddlabs.tt.gamemode.zombies.ZombieModeRules.WAVE_SECONDS;
            }
            public Horde(com.oddlabs.tt.gamemode.zombies.ZombieModeRules this_0_, com.oddlabs.tt.landscape.World world, com.oddlabs.tt.player.Player dead, java.util.function.Consumer status) : base()
            {
                this.this_0 = this_0_;
                init_Horde();
                this.world = world;
                this.dead = dead;
                this.status = status;
            }
            // mixin of default method Animated.updateChecksum
            public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
            {
            }
        }
        private void init_ZombieModeRules()
        {
            this.last_status = "";
        }
        public ZombieModeRules() : base()
        {
            init_ZombieModeRules();
        }
    }
}
