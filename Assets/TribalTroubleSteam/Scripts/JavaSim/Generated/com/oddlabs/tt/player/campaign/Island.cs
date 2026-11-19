// Translated from Island.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.campaign
{
    // Java: 30
    public abstract class Island : java.lang.Object
    {
        public const float CAMPAIGN_DIFFICULTY_BONUS = 0.75f;
        public com.oddlabs.tt.player.campaign.Campaign campaign;
        public com.oddlabs.tt.viewer.WorldViewer world_viewer;
        // Java: 41
        public com.oddlabs.tt.player.campaign.Campaign getCampaign()
        {
            return campaign;
        }
        // Java: 45
        public void chosen(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root)
        {
            init(network, gui_root);
        }
        // Java: 49
        public void addModalForm(com.oddlabs.tt.gui.Form form)
        {
            world_viewer.getGUIRoot().addModalForm(form);
        }
        // Java: 53
        public com.oddlabs.tt.net.GameNetwork startNewGame(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root, int meters_per_world, com.oddlabs.tt.procedural.Landscape.TerrainType terrain, float hills, float vegetation_amount, float supplies_amount, int seed, int campaign_num, int initial_units, string[] ai_names)
        {
            com.oddlabs.tt.viewer.InGameInfo ingame_info = new com.oddlabs.tt.player.campaign.CampaignInGameInfo(campaign);
            com.oddlabs.tt.net.WorldInitAction init_action = new com.oddlabs.tt.net.WorldInitAction_L((com.oddlabs.tt.viewer.WorldViewer viewer) =>
            {
                world_viewer = viewer;
                com.oddlabs.tt.@delegate.Menu.completeGameSetupHack(world_viewer);
                if (!campaign.getState().hasRubberWeapons())
                {
                    viewer.getLocalPlayer().enableRubber(false);
                }
                if (!campaign.getState().hasMagic0())
                {
                    viewer.getLocalPlayer().enableMagic(0, false);
                }
                if (!campaign.getState().hasMagic1())
                {
                    viewer.getLocalPlayer().enableMagic(1, false);
                }
                com.oddlabs.tt.player.Player[] players = viewer.getWorld().getPlayers();
                switch ((campaign.getState().getDifficulty()))
                {
                    case 1:
                    {
                        foreach (com.oddlabs.tt.player.Player player in players)
                        {
                            if (player.isEnemy(viewer.getLocalPlayer()))
                            {
                                viewer.getLocalPlayer().setHitBonus(com.oddlabs.tt.player.campaign.Island.CAMPAIGN_DIFFICULTY_BONUS);
                            }
                        }
                        break;
                    }
                    case 0:
                    {
                        break;
                    }
                    case 2:
                    {
                        foreach (com.oddlabs.tt.player.Player player_2 in players)
                        {
                            if (player_2.isEnemy(viewer.getLocalPlayer()))
                            {
                                player_2.setHitBonus(com.oddlabs.tt.player.campaign.Island.CAMPAIGN_DIFFICULTY_BONUS);
                            }
                        }
                        break;
                    }
                    default:
                    {
                        throw new System.ArgumentException("unexpected difficulty: " + (campaign.getState().getDifficulty()));
                    }
                }
                start();
                new com.oddlabs.tt.trigger.campaign.DefeatTrigger(world_viewer, campaign, viewer.getLocalPlayer().getChieftain());
            });
            return com.oddlabs.tt.@delegate.Menu.startNewGame(network, gui_root, null, new com.oddlabs.tt.landscape.WorldParameters(com.oddlabs.matchmaking.Game.GAMESPEED_NORMAL, "Campaign" + (campaign_num), initial_units, com.oddlabs.tt.player.Player.DEFAULT_MAX_UNIT_COUNT), ingame_info, init_action, null, meters_per_world, terrain, hills, vegetation_amount, supplies_amount, seed, false, ai_names);
        }
        // Java: 101
        public com.oddlabs.tt.viewer.WorldViewer getViewer()
        {
            return world_viewer;
        }
        // Java: 105
        public abstract void init(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root);
        // Java: 107
        public abstract void start();
        // Java: 109
        public abstract string getHeader();
        // Java: 111
        public abstract string getDescription();
        // Java: 113
        public abstract string getCurrentObjective();
        // Java: 115
        public com.oddlabs.tt.model.Unit changeOwner(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.player.Player owner)
        {
            float x = unit.getPositionX();
            float y = unit.getPositionY();
            com.oddlabs.tt.model.UnitTemplate template = ((com.oddlabs.tt.model.UnitTemplate)unit.getTemplate());
            unit.removeNow();
            if (!owner.getUnitCountContainer().isSupplyFull())
            {
                com.oddlabs.tt.model.Unit new_unit = new com.oddlabs.tt.model.Unit(owner, x, y, null, template);
                world_viewer.getPicker().getRespondManager().addResponder(new_unit);
                return new_unit;
            }
            else
            {
                return null;
            }
        }
        // Java: 128
        public void insertGuardTower(com.oddlabs.tt.player.Player owner, int warrior_type, int grid_x, int grid_y)
        {
            com.oddlabs.tt.model.Building tower = owner.buildBuilding(com.oddlabs.tt.model.Race.BUILDING_TOWER, grid_x, grid_y);
            com.oddlabs.tt.model.Unit unit = new com.oddlabs.tt.model.Unit(owner, com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(grid_x), com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(grid_y), null, owner.getRace().getUnitTemplate(warrior_type));
            unit.setTarget(tower, com.oddlabs.tt.model.Action.DEFAULT, false);
        }
        // Java: 138
        public void placePrisoners(com.oddlabs.tt.player.Player captive, com.oddlabs.tt.player.Player enemy, int peons, int rock_warriors, int iron_warriors, int rubber_warriors, bool chieftain)
        {
            int ox = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(enemy.getStartX());
            int oy = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(enemy.getStartY());
            int center = captive.getWorld().getHeightMap().getGridUnitsPerWorld() / 2;
            int dx = center - ox;
            int dy = center - oy;
            float inv_dist = (float)(1.0f / ((float)(java.lang.Math.sqrt(dx * dx + dy * dy))));
            int tx = java.lang.JC.f2i(((float)(ox - (float)((float)(5.0f * dx) * inv_dist))));
            int ty = java.lang.JC.f2i(((float)(oy - (float)((float)(5.0f * dy) * inv_dist))));
            for (int i = 0; i < peons; i++)
            {
                new com.oddlabs.tt.model.Unit(captive, com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(tx), com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(ty), null, captive.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
            }
            for (int i_2 = 0; i_2 < rock_warriors; i_2++)
            {
                new com.oddlabs.tt.model.Unit(captive, com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(tx), com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(ty), null, captive.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
            }
            for (int i_3 = 0; i_3 < iron_warriors; i_3++)
            {
                new com.oddlabs.tt.model.Unit(captive, com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(tx), com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(ty), null, captive.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
            }
            for (int i_4 = 0; i_4 < rubber_warriors; i_4++)
            {
                new com.oddlabs.tt.model.Unit(captive, com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(tx), com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(ty), null, captive.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
            }
            if (chieftain)
            {
                captive.setActiveChieftain(new com.oddlabs.tt.model.Unit(captive, com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(tx), com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(ty), null, captive.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_CHIEFTAIN)));
            }
        }
        // Java: 171
        public void deploy(com.oddlabs.tt.player.Player enemy, int num_units)
        {
            if (enemy.getArmory() != null && !enemy.getArmory().isDead())
            {
                enemy.deployUnits(enemy.getArmory(), com.oddlabs.tt.model.DeployType.IRON_WARRIOR, num_units);
            }
        }
        // Java: 177
        public void attack(com.oddlabs.tt.player.Player enemy, com.oddlabs.tt.util.Target target, int num_units)
        {
            com.oddlabs.tt.player.AI.attackLandscape(enemy, target, num_units);
        }
        // Java: 182
        public com.oddlabs.tt.model.Unit getWarrior(com.oddlabs.tt.player.Player player)
        {
            return com.oddlabs.tt.player.AI.getWarrior(player);
        }
        // Java: 186
        public void refillArmory(com.oddlabs.tt.player.Player enemy)
        {
            if (enemy.getQuarters() == null || enemy.getArmory() == null)
            {
                return;
            }
            enemy.getQuarters().removeSupplies(typeof(com.oddlabs.tt.model.Unit));
            enemy.getArmory().fillSupplies(typeof(com.oddlabs.tt.model.Unit), enemy.getWorld().getMaxUnitCount() - enemy.getUnitCountContainer().getNumSupplies());
            enemy.getArmory().fillSupplies(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon), java.lang.Integer.MAX_VALUE);
        }
        // Java: 196
        public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
        }
        public Island(com.oddlabs.tt.player.campaign.Campaign campaign) : base()
        {
            this.campaign = campaign;
        }
    }
}
