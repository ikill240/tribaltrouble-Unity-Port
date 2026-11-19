// Translated from IslandWarsRules.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.islandwars
{
    // Java: 33
    public class IslandWarsRules : java.lang.Object, com.oddlabs.tt.gamemode.GameModeRules
    {
        public static com.oddlabs.tt.islandwars.IslandWarsSetup setup;
        public static com.oddlabs.tt.islandwars.IslandWarsRules.Raid raid;
        // Java: 38
        public static com.oddlabs.tt.islandwars.IslandWarsSetup currentSetup()
        {
            return com.oddlabs.tt.islandwars.IslandWarsRules.setup;
        }
        // Java: 42
        public static com.oddlabs.tt.islandwars.IslandWarsRules.Raid currentRaid()
        {
            return com.oddlabs.tt.islandwars.IslandWarsRules.raid;
        }
        // Java: 46
        public java.util.List getOptions()
        {
            return java.util.List_.of();
        }
        // Java: 51
        public bool isPlayerAlive(com.oddlabs.tt.player.Player player)
        {
            com.oddlabs.tt.islandwars.IslandWarsSetup s = com.oddlabs.tt.islandwars.IslandWarsRules.setup;
            if (s != null && s.kind == com.oddlabs.tt.islandwars.IslandWarsSetup.HOME)
            {
                return true;
            }
            com.oddlabs.tt.islandwars.IslandWarsRules.Raid r = com.oddlabs.tt.islandwars.IslandWarsRules.raid;
            if (r != null && ((object)r.world == (object)player.getWorld()))
            {
                int slot = r.slotOf(player);
                if (slot == r.defender)
                {
                    return !r.over || !r.attackerWon();
                }
                if (slot == r.attacker)
                {
                    return (r.over ? r.attackerWon() : true);
                }
            }
            int units = player.getUnitCountContainer().getNumSupplies();
            return units > 0 || player.hasActiveChieftain() || player.getQuarters() != null;
        }
        // Java: 68
        public void onGameStart(com.oddlabs.tt.viewer.WorldViewer viewer)
        {
            onWorldStart(viewer.getWorld(), null);
            com.oddlabs.tt.islandwars.IslandWarsClient.gameStarted(viewer);
        }
        // Java: 74
        public string describe()
        {
            com.oddlabs.tt.islandwars.IslandWarsRules.Raid r = com.oddlabs.tt.islandwars.IslandWarsRules.raid;
            return (r != null ? r.status : "");
        }
        // Java: 80
        public void onWorldStart(com.oddlabs.tt.landscape.World world, java.util.function.Consumer status)
        {
            com.oddlabs.tt.islandwars.IslandWarsRules.raid = null;
            com.oddlabs.tt.islandwars.IslandWarsRules.setup = com.oddlabs.tt.islandwars.IslandWarsSetup.decode(world.getIslandWars());
            com.oddlabs.tt.islandwars.IslandWarsSetup s = com.oddlabs.tt.islandwars.IslandWarsRules.setup;
            if (s == null)
            {
                return;
            }
            com.oddlabs.tt.player.Player[] players = world.getPlayers();
            for (int i = 0; i < players.Length; i++)
            {
                if (s.hasSlot(i) && players[i].getPlayerInfo().getTeam() != com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL)
                {
                    com.oddlabs.tt.islandwars.IslandWarsRules.applyUpgrades(players[i], s.upgrades(i), s, i);
                }
            }
            if ((s.kind == com.oddlabs.tt.islandwars.IslandWarsSetup.RAID || s.kind == com.oddlabs.tt.islandwars.IslandWarsSetup.HOME) && s.defender >= 0 && s.defender < players.Length)
            {
                com.oddlabs.tt.player.Player islander = players[s.defender];
                com.oddlabs.tt.islandwars.IslandWarsRules.raise(islander, s.buildings, s.upgrades(s.defender)[com.oddlabs.tt.islandwars.IslandUpgrades.MASONS]);
                if (s.buildings.Length == 0 && islander.getAI() == null)
                {
                    int gx = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(islander.getStartX());
                    int gy = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(islander.getStartY());
                    islander.buildBuilding(com.oddlabs.tt.model.Race.BUILDING_QUARTERS, gx, gy);
                    islander.buildBuilding(com.oddlabs.tt.model.Race.BUILDING_ARMORY, gx, gy);
                }
                if (s.kind == com.oddlabs.tt.islandwars.IslandWarsSetup.RAID && islander.getStandbyAI() == null)
                {
                    islander.setAI(new com.oddlabs.tt.player.AdvancedAI(islander, null, com.oddlabs.tt.player.AdvancedAI.DIFFICULTY_HARD, false, (-1)));
                }
            }
            if (s.kind == com.oddlabs.tt.islandwars.IslandWarsSetup.RAID && s.attacker >= 0 && s.attacker < players.Length && s.defender < players.Length)
            {
                com.oddlabs.tt.islandwars.IslandWarsRules.raid = new com.oddlabs.tt.islandwars.IslandWarsRules.Raid(world, s, status);
                world.getAnimationManagerRealTime().registerAnimation(com.oddlabs.tt.islandwars.IslandWarsRules.raid);
            }
        }
        // Java: 109
        public static void applyUpgrades(com.oddlabs.tt.player.Player p, int[] up, com.oddlabs.tt.islandwars.IslandWarsSetup s, int slot)
        {
            for (int l = 0; l < up[com.oddlabs.tt.islandwars.IslandUpgrades.VETERANS]; l++)
            {
                p.upgrade(com.oddlabs.tt.model.ResearchContainer.ARMOR);
                p.upgrade(com.oddlabs.tt.model.ResearchContainer.RANGE);
            }
            if (up[com.oddlabs.tt.islandwars.IslandUpgrades.CHIEF_HALL] >= 2)
            {
                p.upgrade(com.oddlabs.tt.model.ResearchContainer.VIGOR);
            }
            int gx = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(p.getStartX());
            int gy = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(p.getStartY());
            if (s.kind == com.oddlabs.tt.islandwars.IslandWarsSetup.MATCH && p.getAI() == null)
            {
                for (int t = 0; t < up[com.oddlabs.tt.islandwars.IslandUpgrades.WATCHTOWERS]; t++)
                {
                    p.buildBuilding(com.oddlabs.tt.model.Race.BUILDING_TOWER, gx + 6 * ((t & 1) * 2 - 1), gy + 6 * ((t & 2) - 1));
                }
            }
            if (up[com.oddlabs.tt.islandwars.IslandUpgrades.STOREHOUSE] > 0 && slot != s.attacker)
            {
                int lv = up[com.oddlabs.tt.islandwars.IslandUpgrades.STOREHOUSE];
                int[] stock = new int[] { 20 * lv, 10 * lv, 5 * lv };
                foreach (com.oddlabs.tt.model.Selectable sel in java.lang.JIter.of(p.getUnits().getSet()))
                {
                    if ((sel is com.oddlabs.tt.model.Building b) && !b.isDead() && ((object)((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()) == (object)p.getRace().getBuildingTemplate(com.oddlabs.tt.model.Race.BUILDING_ARMORY)))
                    {
                        for (int i = 0; i < stock.Length; i++)
                        {
                            com.oddlabs.tt.model.SupplyContainer c = b.getSupplyContainer(com.oddlabs.tt.model.ExtendedResources.STORES[i]);
                            if (c != null && stock[i] > 0)
                            {
                                stock[i] -= c.increaseSupply(stock[i]);
                            }
                        }
                    }
                }
            }
        }
        // Java: 134
        public static void raise(com.oddlabs.tt.player.Player p, int[] b, int masons)
        {
            int gx = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(p.getStartX());
            int gy = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(p.getStartY());
            for (int i = 0; i + 3 < b.Length; i += 4)
            {
                int type = b[i];
                if (type < 0 || type >= com.oddlabs.tt.model.Race.NUM_BUILDINGS || type == com.oddlabs.tt.model.Race.BUILDING_SHIP || type == com.oddlabs.tt.model.Race.BUILDING_BRIDGE)
                {
                    continue;
                }
                com.oddlabs.tt.model.Building built = p.buildBuilding(type, gx + java.lang.Math.clamp(b[i + 1], (-200), 200), gy + java.lang.Math.clamp(b[i + 2], (-200), 200));
                int tier = java.lang.Math.clamp(java.lang.Math.max(b[i + 3], masons), 0, 3);
                if ((built is com.oddlabs.tt.model.LandBuilding lb) && tier > 0 && p.getWorld().isExtended())
                {
                    lb.setTier(tier);
                }
            }
        }
        // Java: 148
        public static int worth(com.oddlabs.tt.model.Building b)
        {
            int id = ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID();
            return (id == com.oddlabs.tt.model.Race.BUILDING_QUARTERS || id == com.oddlabs.tt.model.Race.BUILDING_ARMORY || id == com.oddlabs.tt.model.Race.BUILDING_HALL ? 3 : (id == com.oddlabs.tt.model.Race.BUILDING_TOWER || id == com.oddlabs.tt.model.Race.BUILDING_FORGE ? 2 : 1));
        }
        // Java: 155
        public static int[] layout(com.oddlabs.tt.player.Player p)
        {
            int gx = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(p.getStartX());
            int gy = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(p.getStartY());
            java.util.List l = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(p.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.LandBuilding b) && !b.isDead() && b.isComplete() && ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID() != com.oddlabs.tt.model.Race.BUILDING_BRIDGE && ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID() != com.oddlabs.tt.model.Race.BUILDING_SHIP)
                {
                    l.add(((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID());
                    l.add(b.getGridX() - gx);
                    l.add(b.getGridY() - gy);
                    l.add(b.getTier());
                }
            }
            int[] a = new int[l.size()];
            for (int i = 0; i < a.Length; i++)
            {
                a[i] = ((int)((int?)l.get(i)));
            }
            return a;
        }
        // Java: 173
        public class Raid : java.lang.Object, com.oddlabs.tt.animation.Animated
        {
            public com.oddlabs.tt.landscape.World world;
            public int attacker;
            public int defender;
            public com.oddlabs.tt.islandwars.IslandWarsSetup setup;
            public java.util.function.Consumer status_out;
            public java.util.List island;
            public int total_worth;
            public float time;
            public float next_check;
            public bool over;
            public float damage;
            public string status;
            // Java: 200
            public int slotOf(com.oddlabs.tt.player.Player p)
            {
                com.oddlabs.tt.player.Player[] players = world.getPlayers();
                for (int i = 0; i < players.Length; i++)
                {
                    if (((object)players[i] == (object)p))
                    {
                        return i;
                    }
                }
                return (-1);
            }
            // Java: 208
            public bool isOver()
            {
                return over;
            }
            // Java: 213
            public float getDamage()
            {
                return damage;
            }
            // Java: 218
            public bool attackerWon()
            {
                return damage >= 0.5f;
            }
            // Java: 222
            public float getTimeLeft()
            {
                return java.lang.Math.max(0.0f, (float)(com.oddlabs.tt.islandwars.IslandWarsSetup.RAID_SECONDS - time));
            }
            // Java: 227
            public int[] loot()
            {
                int[] l = new int[3];
                for (int i = 0; i < 3; i++)
                {
                    l[i] = java.lang.JC.f2i(((float)(setup.loot[i] * java.lang.Math.min(1.0f, damage))));
                }
                return l;
            }
            // Java: 234
            public string getStatus()
            {
                return status;
            }
            // Java: 238
            public com.oddlabs.tt.islandwars.IslandWarsSetup getSetup()
            {
                return setup;
            }
            // Java: 242
            public void animate(float t)
            {
                if (over)
                {
                    return;
                }
                time += t;
                if (time < next_check)
                {
                    return;
                }
                next_check = (float)(time + 1.0f);
                int lost = 0;
                foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(island))
                {
                    if (b.isDead())
                    {
                        lost += com.oddlabs.tt.islandwars.IslandWarsRules.worth(b);
                    }
                }
                damage = (total_worth == 0 ? 0.0f : (float)(lost / ((float)(total_worth))));
                com.oddlabs.tt.player.Player raider = world.getPlayers()[attacker];
                bool raiders_left = false;
                foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(raider.getUnits().getSet()))
                {
                    if ((s is com.oddlabs.tt.model.Unit u) && !u.isDead())
                    {
                        raiders_left = true;
                        break;
                    }
                }
                if (time >= com.oddlabs.tt.islandwars.IslandWarsSetup.RAID_SECONDS || !raiders_left || damage >= 0.999f)
                {
                    over = true;
                }
                int secs = java.lang.JC.f2i(getTimeLeft());
                string text = java.lang.JStr.valueOf(((over ? "Raid over: " : "Raid: "))) + (java.lang.Math.round((float)(damage * 100.0f))) + "% of " + java.lang.JStr.valueOf(setup.name(defender)) + "'s island destroyed" + java.lang.JStr.valueOf(((over ? "" : " - " + (secs / 60) + ":" + java.lang.JStr.valueOf(((secs % 60 < 10 ? "0" : ""))) + (secs % 60) + " left")));
                if (!java.lang.JO.equals(text, status))
                {
                    status = text;
                    if (status_out != null)
                    {
                        status_out.accept(text);
                    }
                }
            }
            private void init_Raid()
            {
                this.island = new java.util.ArrayList();
                this.status = "";
            }
            public Raid(com.oddlabs.tt.landscape.World world, com.oddlabs.tt.islandwars.IslandWarsSetup setup, java.util.function.Consumer status_out) : base()
            {
                init_Raid();
                this.world = world;
                this.setup = setup;
                this.attacker = setup.attacker;
                this.defender = setup.defender;
                this.status_out = status_out;
                com.oddlabs.tt.player.Player islander = world.getPlayers()[defender];
                foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(islander.getUnits().getSet()))
                {
                    if ((s is com.oddlabs.tt.model.Building b) && !b.isDead() && ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID() != com.oddlabs.tt.model.Race.BUILDING_SHIP)
                    {
                        island.add(b);
                        total_worth += com.oddlabs.tt.islandwars.IslandWarsRules.worth(b);
                    }
                }
            }
            // mixin of default method Animated.updateChecksum
            public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
            {
            }
        }
        public IslandWarsRules() : base()
        {
        }
    }
}
