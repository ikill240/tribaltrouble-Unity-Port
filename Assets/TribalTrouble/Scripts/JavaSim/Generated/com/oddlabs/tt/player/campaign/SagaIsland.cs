// Translated from SagaIsland.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.campaign
{
    // Java: 63
    public class SagaIsland : com.oddlabs.tt.player.campaign.Island
    {
        public static java.util.ResourceBundle bundle;
        public static int[] AI_TIERS;
        // Java: 68
        public static string i18n(string key_v, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.player.campaign.SagaIsland.bundle, key_v, args);
        }
        // Java: 73
        public static bool isWarrior(int unit)
        {
            return unit != com.oddlabs.tt.model.Race.UNIT_PEON && unit != com.oddlabs.tt.model.Race.UNIT_CHIEFTAIN && unit != com.oddlabs.tt.model.Race.UNIT_ZOMBIE && unit != com.oddlabs.tt.model.Race.UNIT_ZOMBIE_BRUTE && unit != com.oddlabs.tt.model.Race.UNIT_ZOMBIE_BOSS && unit < com.oddlabs.tt.model.Race.NUM_UNITS;
        }
        // Java: 78
        public static string holdingHeader(com.oddlabs.tt.player.campaign.SagaCampaign campaign, int island)
        {
            return (island == com.oddlabs.tt.player.campaign.SagaCampaign.HOME ? com.oddlabs.tt.player.campaign.SagaIsland.i18n("home_header") : com.oddlabs.tt.player.campaign.SagaIsland.i18n("isle_header", campaign.islandName(island)));
        }
        // Java: 83
        public static string holdingDescription(com.oddlabs.tt.player.campaign.SagaCampaign campaign, int island)
        {
            com.oddlabs.tt.player.campaign.CampaignState state = campaign.getState();
            int[] u = state.getIsleUnits(island);
            int[] st = state.getIsleStock(island);
            int warriors_v = 0;
            for (int i = 0; i < u.Length; i++)
            {
                if (com.oddlabs.tt.player.campaign.SagaIsland.isWarrior(i))
                {
                    warriors_v += u[i];
                }
            }
            int cap = campaign.isleCap(island);
            string raid = (campaign.claims() >= com.oddlabs.tt.player.campaign.SagaCampaign.RAIDS_AFTER ? " " + java.lang.JStr.valueOf(com.oddlabs.tt.player.campaign.SagaIsland.i18n("raids_warning")) : "");
            return java.lang.JStr.valueOf(com.oddlabs.tt.player.campaign.SagaIsland.i18n((island == com.oddlabs.tt.player.campaign.SagaCampaign.HOME ? "home_description" : "isle_description"), u[com.oddlabs.tt.model.Race.UNIT_PEON], warriors_v, cap, st[0], st[1], st[2], st[3], st[4], st[5], st[6], st[7], st[8])) + java.lang.JStr.valueOf(raid);
        }
        public com.oddlabs.tt.player.campaign.SagaMission mission_f;
        public com.oddlabs.tt.player.Player defended;
        // Java: 99
        public com.oddlabs.tt.player.campaign.SagaMission mission()
        {
            return mission_f;
        }
        public bool[] done;
        public bool finished;
        public float elapsed;
        public const int MISSION = 0;
        public const int HOLD = 1;
        public const int DEFEND = 2;
        public const int HELP = 3;
        public const int VISIT = 4;
        public const int VISIT_GARRISON = 120;
        public int mode;
        public bool home;
        public int defense;
        public bool raids;
        public int raid_strength;
        public int raids_sent;
        public float next_raid;
        public float next_camp_work;
        public int camp_rounds;
        public bool settled;
        public int trees_at_start;
        public float next_autosave;
        public int[] base_home_units;
        public int[] base_home_stock;
        public bool supplies_told;
        public int[] shipment;
        public float shipment_at;
        public float next_shipment;
        public int[] expedition;
        public int[] pending;
        public java.util.List fleet;
        public java.util.List fleet_from;
        public java.util.List raid_fleet;
        public java.util.List raid_from;
        public java.util.List reinf_fleet;
        public java.util.List reinf_from;
        public java.util.List reinf_owner;
        public float next_reinforcement;
        public int reinforcements_sent;
        public java.util.Map ally_call;
        public float next_ally_check;
        public const int ALLY_BUILDINGS = 1000;
        public float next_guard;
        public float next_establish_note;
        public const float SHIPMENT_TRIP = 45.0f;
        public const float SHIPMENT_WAIT = 180.0f;
        // Java: 155
        public bool canSendForSupplies()
        {
            if (mode != com.oddlabs.tt.player.campaign.SagaIsland.MISSION || settled || shipment != null || elapsed < next_shipment || getViewer() == null)
            {
                return false;
            }
            if (!saga().hasHome())
            {
                return false;
            }
            int any = 0;
            foreach (int v in getCampaign().getState().getSagaStock())
            {
                any += v;
            }
            if (any == 0)
            {
                return false;
            }
            com.oddlabs.tt.player.Player me = getViewer().getLocalPlayer();
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(me.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.LandBuilding b) && !b.isDead() && b.isComplete() && ((object)((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()) == (object)me.getRace().getBuildingTemplate(com.oddlabs.tt.model.Race.BUILDING_MARKET)))
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 173
        public void sendForSupplies()
        {
            if (!canSendForSupplies())
            {
                return;
            }
            com.oddlabs.tt.player.campaign.CampaignState state = getCampaign().getState();
            int cargo = java.lang.Math.min(3, state.getSagaResearch()[com.oddlabs.tt.model.ResearchContainer.CARGO]);
            int[] home_stock = state.getSagaStock();
            shipment = new int[home_stock.Length];
            for (int i = 0; i < home_stock.Length; i++)
            {
                shipment[i] = home_stock[i] * com.oddlabs.tt.player.campaign.ExpeditionForm.STOCK_PERCENT[java.lang.Math.clamp(mission_f.chapter_f, 0, 2)][cargo] / 100;
                home_stock[i] -= shipment[i];
            }
            shipment_at = (float)(elapsed + com.oddlabs.tt.player.campaign.SagaIsland.SHIPMENT_TRIP);
            next_shipment = (float)(elapsed + com.oddlabs.tt.player.campaign.SagaIsland.SHIPMENT_WAIT);
        }
        // Java: 189
        public bool isHolding()
        {
            return (mode == com.oddlabs.tt.player.campaign.SagaIsland.HOLD || mode == com.oddlabs.tt.player.campaign.SagaIsland.VISIT) && !settled;
        }
        // Java: 193
        public com.oddlabs.tt.player.campaign.SagaCampaign saga()
        {
            return ((com.oddlabs.tt.player.campaign.SagaCampaign)getCampaign());
        }
        // Java: 203
        public string key(string what)
        {
            return "m" + (mission_f.index) + "." + java.lang.JStr.valueOf(what);
        }
        // Java: 207
        public bool nativeTribe()
        {
            return getCampaign().getState().getRace() == com.oddlabs.tt.player.campaign.CampaignState.RACE_NATIVES;
        }
        // Java: 212
        public int tier(int @base)
        {
            int t = @base + ((getCampaign().getState().getDifficulty()) switch { 1 => (int)((-1)), 2 => (int)(1), _ => (int)(0) });
            return com.oddlabs.tt.player.campaign.SagaIsland.AI_TIERS[java.lang.Math.clamp(t, 1, com.oddlabs.tt.player.campaign.SagaIsland.AI_TIERS.Length) - 1];
        }
        // Java: 221
        public override void init(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root)
        {
            java.util.Arrays.fill(done, false);
            finished = false;
            elapsed = 0.0f;
            pending = null;
            fleet.clear();
            fleet_from.clear();
            settled = false;
            supplies_told = false;
            shipment = null;
            next_shipment = 0.0f;
            raids_sent = 0;
            com.oddlabs.tt.player.campaign.CampaignState state = getCampaign().getState();
            defense = saga().takeDefense(mission_f.index);
            bool claimed = saga().isClaimed(mission_f.index);
            mode = (saga().takeVisit(mission_f.index) ? com.oddlabs.tt.player.campaign.SagaIsland.VISIT : (defense == com.oddlabs.tt.player.campaign.SagaCampaign.DEFEND_HELP ? com.oddlabs.tt.player.campaign.SagaIsland.HELP : (defense != com.oddlabs.tt.player.campaign.SagaCampaign.DEFEND_NONE && claimed ? com.oddlabs.tt.player.campaign.SagaIsland.DEFEND : (claimed ? com.oddlabs.tt.player.campaign.SagaIsland.HOLD : com.oddlabs.tt.player.campaign.SagaIsland.MISSION))));
            home = mode == com.oddlabs.tt.player.campaign.SagaIsland.HOLD || mode == com.oddlabs.tt.player.campaign.SagaIsland.DEFEND;
            int[] raid = state.getSagaRaid();
            raid_strength = ((mode == com.oddlabs.tt.player.campaign.SagaIsland.DEFEND || mode == com.oddlabs.tt.player.campaign.SagaIsland.HELP) && raid != null ? raid[1] : 1 + saga().claims() / 3);
            raids = mode == com.oddlabs.tt.player.campaign.SagaIsland.DEFEND || mode == com.oddlabs.tt.player.campaign.SagaIsland.HELP || mode == com.oddlabs.tt.player.campaign.SagaIsland.HOLD && saga().claims() >= com.oddlabs.tt.player.campaign.SagaCampaign.RAIDS_AFTER;
            base_home_units = ((int[])state.getSagaUnits().clone());
            base_home_stock = ((int[])state.getSagaStock().clone());
            bool at_home_island = mission_f.index == com.oddlabs.tt.player.campaign.SagaCampaign.HOME;
            expedition = (home && at_home_island ? new int[com.oddlabs.tt.player.campaign.CampaignState.SAGA_UNITS + com.oddlabs.tt.player.campaign.CampaignState.SAGA_STOCKS] : ((int[])state.getSagaExpedition().clone()));
            int[] at_home = state.getSagaUnits();
            int[] stock = state.getSagaStock();
            for (int i = 0; i < expedition.Length; i++)
            {
                expedition[i] = java.lang.Math.max(0, java.lang.Math.min(expedition[i], (i < com.oddlabs.tt.player.campaign.CampaignState.SAGA_UNITS ? at_home[i] : stock[i - com.oddlabs.tt.player.campaign.CampaignState.SAGA_UNITS])));
            }
            int cap = (home ? population() + 10 : com.oddlabs.tt.player.Player.NEW_TIDES_MAX_UNIT_COUNT);
            float left = (home ? java.lang.Math.max(0.25f, (float)(1.0f - (float)(state.getIsleDepletion(mission_f.index) / 100.0f))) : 1.0f);
            string[] ai_names = clanNames(state);
            com.oddlabs.tt.landscape.WorldParameters @params = com.oddlabs.tt.landscape.WorldParameters.builder().initialGameSpeed(com.oddlabs.matchmaking.Game.GAMESPEED_NORMAL).mapcode("Saga" + (mission_f.index)).initialUnitCount((home ? cap : com.oddlabs.tt.player.campaign.SagaCampaign.MAX_UNITS)).maxUnitCount(cap).mode((home ? com.oddlabs.matchmaking.GameMode.STANDARD : mission_f.mode)).extended(true).weather(!home && mission_f.weather).build();
            com.oddlabs.tt.procedural.Landscape.TerrainType terrain = com.oddlabs.tt.player.campaign.SagaMissions.terrain(mission_f.chapter_f, nativeTribe());
            com.oddlabs.tt.net.GameNetwork game_network = startNewGame(network, gui_root, mission_f.size, terrain, mission_f.hills, (float)(mission_f.trees * left), (float)(mission_f.supplies * left), mission_f.seed, ai_names, @params, mission_f.archipelago_f);
            int own_race = (nativeTribe() ? com.oddlabs.tt.model.RacesResources.RACE_NATIVES : com.oddlabs.tt.model.RacesResources.RACE_VIKINGS);
            int other_race = (nativeTribe() ? com.oddlabs.tt.model.RacesResources.RACE_VIKINGS : com.oddlabs.tt.model.RacesResources.RACE_NATIVES);
            game_network.getClient().getServerInterface().setPlayerSlot(0, com.oddlabs.tt.net.PlayerSlot.HUMAN, own_race, 0, true, com.oddlabs.tt.net.PlayerSlot.AI_NONE);
            if (home)
            {
                game_network.getClient().setUnitInfo(0, new com.oddlabs.tt.player.UnitInfo(false, false, 0, 0, true, 0, 0, 0, 0));
                int slot = 1;
                if (raids)
                {
                    game_network.getClient().getServerInterface().setPlayerSlot(slot, com.oddlabs.tt.net.PlayerSlot.AI, other_race, 1, true, tier(java.lang.Math.min(3, 1 + raid_strength / 2)));
                    game_network.getClient().setUnitInfo(slot++, new com.oddlabs.tt.player.UnitInfo(false, false, 0, 0, false, 0, 0, 0, 0));
                }
                if (defense == com.oddlabs.tt.player.campaign.SagaCampaign.DEFEND_ALLY)
                {
                    int allies = state.getSagaAllies();
                    game_network.getClient().getServerInterface().setPlayerSlot(slot, com.oddlabs.tt.net.PlayerSlot.AI, own_race, 0, true, com.oddlabs.tt.net.PlayerSlot.AI_HARD);
                    game_network.getClient().setUnitInfo(slot, new com.oddlabs.tt.player.UnitInfo(false, false, 0, 0, false, 4, 4 + allies, 6 + 2 * raid_strength, 2 + allies / 2));
                }
                else if (state.getIsleBuildings(mission_f.index + com.oddlabs.tt.player.campaign.SagaIsland.ALLY_BUILDINGS).Length > 0)
                {
                    game_network.getClient().getServerInterface().setPlayerSlot(slot, com.oddlabs.tt.net.PlayerSlot.AI, own_race, 0, true, com.oddlabs.tt.net.PlayerSlot.AI_HARD);
                    game_network.getClient().setUnitInfo(slot, new com.oddlabs.tt.player.UnitInfo(false, false, 0, 0, false, 4, 2, 2, 0));
                }
                game_network.getClient().getServerInterface().startServer();
                return;
            }
            if (mode == com.oddlabs.tt.player.campaign.SagaIsland.VISIT)
            {
                game_network.getClient().setUnitInfo(0, new com.oddlabs.tt.player.UnitInfo(false, false, 0, 0, true, 4 + expedition[com.oddlabs.tt.model.Race.UNIT_PEON], expedition[com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK], expedition[com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON], expedition[com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER]));
                bool other = false;
                foreach (com.oddlabs.tt.player.campaign.SagaMission.Rival r in java.lang.JIter.of(mission_f.rivals))
                {
                    if (r.team() == 0)
                    {
                        other = r.other_race();
                        break;
                    }
                }
                game_network.getClient().getServerInterface().setPlayerSlot(1, com.oddlabs.tt.net.PlayerSlot.AI, (other ? other_race : own_race), 0, true, com.oddlabs.tt.net.PlayerSlot.AI_HARD);
                game_network.getClient().setUnitInfo(1, new com.oddlabs.tt.player.UnitInfo(true, true, 0, 2, true, 16, 4, 4, 2));
                game_network.getClient().getServerInterface().startServer();
                return;
            }
            if (mode == com.oddlabs.tt.player.campaign.SagaIsland.HELP)
            {
                game_network.getClient().setUnitInfo(0, new com.oddlabs.tt.player.UnitInfo(false, false, 0, 0, true, 6 + expedition[com.oddlabs.tt.model.Race.UNIT_PEON], 4 + expedition[com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK], 2 + expedition[com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON], expedition[com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER]));
                game_network.getClient().getServerInterface().setPlayerSlot(1, com.oddlabs.tt.net.PlayerSlot.AI, other_race, 1, true, tier(java.lang.Math.min(3, 1 + raid_strength / 2)));
                game_network.getClient().setUnitInfo(1, new com.oddlabs.tt.player.UnitInfo(false, false, 0, 0, false, 0, 0, 0, 0));
                int allies_2 = state.getSagaAllies();
                game_network.getClient().getServerInterface().setPlayerSlot(2, com.oddlabs.tt.net.PlayerSlot.AI, own_race, 0, true, com.oddlabs.tt.net.PlayerSlot.AI_HARD);
                game_network.getClient().setUnitInfo(2, new com.oddlabs.tt.player.UnitInfo(true, true, 0, 2, true, 10, 4 + allies_2, 6, 2));
                game_network.getClient().getServerInterface().startServer();
                return;
            }
            game_network.getClient().setUnitInfo(0, new com.oddlabs.tt.player.UnitInfo(false, false, 0, 0, true, mission_f.peons + expedition[com.oddlabs.tt.model.Race.UNIT_PEON], mission_f.rock + expedition[com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK], mission_f.iron + expedition[com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON], mission_f.rubber + expedition[com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER]));
            int slot_2 = 1;
            int step = difficultyStep();
            int ch = java.lang.Math.clamp(mission_f.chapter_f, 0, 2);
            foreach (com.oddlabs.tt.player.campaign.SagaMission.Rival r_2 in java.lang.JIter.of(mission_f.rivals))
            {
                int peons = r_2.peons();
                int rock = r_2.rock();
                int iron = r_2.iron();
                int rubber = r_2.rubber();
                int towers = r_2.towers();
                if (r_2.team() != 0)
                {
                    peons += ((getCampaign().getState().getDifficulty()) switch { 1 => (int)(-peons / 3), 2 => (int)(peons / 2), _ => (int)(0) });
                    rock += java.lang.Math.max(0, 2 + ch + step);
                    iron += java.lang.Math.max(0, 1 + 2 * ch + step);
                    rubber += (ch >= 1 ? ch + java.lang.Math.max(0, step) : 0);
                    towers += ch + java.lang.Math.max(0, step);
                }
                else
                {
                    rock += 4 + ch;
                    iron += 3 + 2 * ch;
                    rubber += 1 + ch;
                }
                game_network.getClient().getServerInterface().setPlayerSlot(slot_2, com.oddlabs.tt.net.PlayerSlot.AI, (r_2.other_race() ? other_race : own_race), r_2.team(), true, tier(r_2.tier()));
                game_network.getClient().setUnitInfo(slot_2, new com.oddlabs.tt.player.UnitInfo(r_2.@base(), r_2.@base(), 0, towers, r_2.chieftain(), peons, rock, iron, rubber));
                slot_2++;
            }
            if (state.isSagaHired())
            {
                game_network.getClient().getServerInterface().setPlayerSlot(slot_2, com.oddlabs.tt.net.PlayerSlot.AI, own_race, 0, true, tier(4));
                game_network.getClient().setUnitInfo(slot_2, new com.oddlabs.tt.player.UnitInfo(true, true, 0, 1, true, 14, 7 + ch, 6 + 2 * ch, 3 + ch));
            }
            game_network.getClient().getServerInterface().startServer();
        }
        // Java: 362
        public override void start()
        {
            com.oddlabs.tt.player.Player me = getViewer().getLocalPlayer();
            com.oddlabs.tt.player.campaign.CampaignState state = getCampaign().getState();
            com.oddlabs.tt.model.Unlocks.lockNewTides(me);
            bool at_home_island = home && mission_f.index == com.oddlabs.tt.player.campaign.SagaCampaign.HOME;
            foreach (int k in state.getSagaUnlocked())
            {
                if (at_home_island || state.isSagaEstablished(k))
                {
                    me.unlock(k);
                }
            }
            if (mode == com.oddlabs.tt.player.campaign.SagaIsland.MISSION)
            {
                foreach (int k_2 in com.oddlabs.tt.player.campaign.SagaIsland.storyKeys(mission_f))
                {
                    me.unlock(k_2);
                }
            }
            com.oddlabs.tt.model.Unlocks.apply(me);
            int[] research = state.getSagaResearch();
            if (at_home_island)
            {
                me.setResearchCostPercent(com.oddlabs.tt.player.campaign.SagaIsland.HOME_RESEARCH_COST_PERCENT);
                me.setCampaignTribe(true);
                int[] cap = new int[com.oddlabs.tt.model.ResearchContainer.NUM_UPGRADES];
                for (int u = 0; u < cap.Length; u++)
                {
                    cap[u] = com.oddlabs.tt.model.ResearchContainer.getMaxLevel(u);
                }
                cap[com.oddlabs.tt.model.ResearchContainer.GREAT_TRIBE] = saga().greatTribeLimit();
                me.setResearchCap(cap);
            }
            else if (mode == com.oddlabs.tt.player.campaign.SagaIsland.MISSION)
            {
                int[] cap_2 = new int[com.oddlabs.tt.model.ResearchContainer.NUM_UPGRADES];
                for (int u_2 = 0; u_2 < cap_2.Length; u_2++)
                {
                    cap_2[u_2] = java.lang.Math.max(research[u_2], storyLevel(u_2));
                }
                me.setResearchCap(cap_2);
                research = new int[research.Length];
            }
            for (int u_3 = 0; u_3 < com.oddlabs.tt.model.ResearchContainer.NUM_UPGRADES; u_3++)
            {
                for (int l = 0; l < com.oddlabs.tt.model.ResearchContainer.MAX_LEVEL && me.getUpgradeLevel(u_3) < research[u_3]; l++)
                {
                    me.upgrade(u_3);
                }
            }
            int gx = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(me.getStartX());
            int gy = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(me.getStartY());
            int[] stock = new int[com.oddlabs.tt.player.campaign.CampaignState.SAGA_STOCKS];
            if (home)
            {
                rebuildHolding(me, state, gx, gy);
            }
            else
            {
                if (mode == com.oddlabs.tt.player.campaign.SagaIsland.VISIT && state.getIsleBuildings(mission_f.index).Length > 0)
                {
                    rebuildHolding(me, state, gx, gy);
                }
                for (int u_4 = 0; u_4 < com.oddlabs.tt.player.campaign.CampaignState.SAGA_UNITS; u_4++)
                {
                    if (u_4 != com.oddlabs.tt.model.Race.UNIT_PEON && u_4 != com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK && u_4 != com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON && u_4 != com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER)
                    {
                        for (int n = 0; n < expedition[u_4]; n++)
                        {
                            com.oddlabs.tt.player.campaign.SagaIsland.spawn(me, u_4, n);
                        }
                    }
                }
                for (int i = 0; i < stock.Length; i++)
                {
                    stock[i] = ((i <= com.oddlabs.tt.model.ExtendedResources.CHICKEN ? mission_f.stock : 0)) + expedition[com.oddlabs.tt.player.campaign.CampaignState.SAGA_UNITS + i];
                }
            }
            if (mode == com.oddlabs.tt.player.campaign.SagaIsland.MISSION && mission_f.@base)
            {
                me.buildBuilding(com.oddlabs.tt.model.Race.BUILDING_QUARTERS, gx, gy);
                me.buildBuilding(com.oddlabs.tt.model.Race.BUILDING_ARMORY, gx, gy);
                com.oddlabs.tt.player.campaign.SagaIsland.deposit(me, stock);
            }
            else if (!home)
            {
                pending = stock;
            }
            trees_at_start = com.oddlabs.tt.player.campaign.SagaIsland.countTrees(getViewer().getWorld());
            int claims_now = saga().claims();
            next_raid = (mode == com.oddlabs.tt.player.campaign.SagaIsland.DEFEND || mode == com.oddlabs.tt.player.campaign.SagaIsland.HELP ? 60.0f : (float)(java.lang.Math.max(60.0f, (float)(150.0f - (float)(8.0f * claims_now))) + getViewer().getWorld().getRandom().nextInt(60)));
            next_camp_work = (float)(next_raid + 90.0f);
            camp_rounds = 0;
            next_autosave = 120.0f;
            if (!home && mission_f.by_sea)
            {
                embark(me);
            }
            if (mode == com.oddlabs.tt.player.campaign.SagaIsland.MISSION && mission_f.archipelago_f)
            {
                foreach (com.oddlabs.tt.player.Player p in getViewer().getWorld().getPlayers())
                {
                    if (((object)p != (object)me) && p.getPlayerInfo().getTeam() != com.oddlabs.tt.landscape.World.ZOMBIE_TEAM)
                    {
                        com.oddlabs.tt.player.campaign.SagaIsland.harbour(p, (p.getPlayerInfo().getTeam() == me.getPlayerInfo().getTeam() ? 1 : 2));
                    }
                }
            }
            if (mode == com.oddlabs.tt.player.campaign.SagaIsland.MISSION)
            {
                for (int k_3 = 0; k_3 < mission_f.rivals.size(); k_3++)
                {
                    if (((com.oddlabs.tt.player.campaign.SagaMission.Rival)mission_f.rivals.get(k_3)).elemental() && k_3 + 1 < getViewer().getWorld().getPlayers().Length)
                    {
                        com.oddlabs.tt.player.campaign.SagaIsland.makeElemental(getViewer().getWorld().getPlayers()[k_3 + 1]);
                    }
                }
            }
            if (home)
            {
                rebuildAllies(me, state, gx, gy);
            }
            if (mode == com.oddlabs.tt.player.campaign.SagaIsland.MISSION)
            {
                foreach (com.oddlabs.tt.player.Player p_2 in getViewer().getWorld().getPlayers())
                {
                    if (((object)p_2 != (object)me) && me.isEnemy(p_2) && p_2.getPlayerInfo().getTeam() != com.oddlabs.tt.landscape.World.ZOMBIE_TEAM && com.oddlabs.tt.player.campaign.SagaIsland.hasBuildings(p_2))
                    {
                        village(p_2);
                    }
                }
                reinforcements_sent = 0;
                next_reinforcement = (float)(reinforcementInterval() + 60.0f);
            }
            if (mission_f.rival_god >= 0 && mode == com.oddlabs.tt.player.campaign.SagaIsland.MISSION)
            {
                foreach (com.oddlabs.tt.player.Player p_3 in getViewer().getWorld().getPlayers())
                {
                    if (me.isEnemy(p_3) && p_3.getPlayerInfo().getTeam() != com.oddlabs.tt.landscape.World.ZOMBIE_TEAM)
                    {
                        new com.oddlabs.tt.model.Unit(p_3, (float)(p_3.getStartX() + 4.0f), (float)(p_3.getStartY() + 4.0f), null, p_3.getRace().getUnitTemplate(mission_f.rival_god));
                    }
                }
            }
            foreach (int u_5 in mission_f.known)
            {
                if (me.getUpgradeLevel(u_5) == 0)
                {
                    me.upgrade(u_5);
                }
            }
            if (me.getChieftain() != null)
            {
                me.getChieftain().increaseMagicEnergy(0, 1000);
                me.getChieftain().increaseMagicEnergy(1, 1000);
            }
            com.oddlabs.tt.gui.ObjectiveBanner status = new com.oddlabs.tt.gui.ObjectiveBanner(getViewer().getGUIRoot(), (mode == com.oddlabs.tt.player.campaign.SagaIsland.MISSION ? missionHeader() : ""));
            getViewer().getGUIRoot().addChild(status);
            defended = (mode == com.oddlabs.tt.player.campaign.SagaIsland.HELP && getViewer().getWorld().getPlayers().Length > 2 ? getViewer().getWorld().getPlayers()[2] : me);
            string intro = (mode == com.oddlabs.tt.player.campaign.SagaIsland.VISIT ? com.oddlabs.tt.player.campaign.SagaIsland.i18n("visit_intro", saga().islandName(mission_f.index), com.oddlabs.tt.player.campaign.SagaIsland.VISIT_GARRISON) : (mode == com.oddlabs.tt.player.campaign.SagaIsland.HELP ? com.oddlabs.tt.player.campaign.SagaIsland.i18n("help_intro", saga().islandName(mission_f.index)) : (mode == com.oddlabs.tt.player.campaign.SagaIsland.DEFEND ? com.oddlabs.tt.player.campaign.SagaIsland.i18n((defense == com.oddlabs.tt.player.campaign.SagaCampaign.DEFEND_ALLY ? "defend_intro_allies" : "defend_intro"), saga().islandName(mission_f.index)) : (home ? java.lang.JStr.valueOf(((mission_f.index == com.oddlabs.tt.player.campaign.SagaCampaign.HOME ? java.lang.JStr.valueOf(com.oddlabs.tt.player.campaign.SagaIsland.i18n("home_intro", population())) + java.lang.JStr.valueOf(((com.oddlabs.tt.player.campaign.SagaIsland.unestablished(state) > 0 ? " " + java.lang.JStr.valueOf(com.oddlabs.tt.player.campaign.SagaIsland.i18n("learn_at_home", com.oddlabs.tt.player.campaign.SagaIsland.unestablished(state))) : ""))) : com.oddlabs.tt.player.campaign.SagaIsland.i18n("isle_intro", saga().islandName(mission_f.index), population())))) + java.lang.JStr.valueOf(((raids ? " " + java.lang.JStr.valueOf(com.oddlabs.tt.player.campaign.SagaIsland.i18n("raids_warning")) : ""))) : (mission_f.by_sea ? java.lang.JStr.valueOf(com.oddlabs.tt.player.campaign.SagaIsland.i18n(key("intro"))) + " " + java.lang.JStr.valueOf(com.oddlabs.tt.player.campaign.SagaIsland.i18n("by_sea")) : com.oddlabs.tt.player.campaign.SagaIsland.i18n(key("intro")))))));
            new com.oddlabs.tt.trigger.campaign.GameStartedTrigger(getViewer().getWorld(), new java.lang.Runnable_L(() =>
            {
                addModalForm(new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), getHeader(), intro, face(mission_f.speaker), com.oddlabs.tt.gui.Origin.AT_START));
            }));
            getViewer().getWorld().getAnimationManagerGameTime().registerAnimation(new com.oddlabs.tt.player.campaign.SagaIsland.Watch(this, status));
        }
        // Java: 480
        public static int[] storyKeys(com.oddlabs.tt.player.campaign.SagaMission mission_v)
        {
            java.util.LinkedHashSet keys = new java.util.LinkedHashSet();
            foreach (int k in mission_v.uses_f)
            {
                keys.add(k);
            }
            foreach (com.oddlabs.tt.player.campaign.SagaMission.Task t in java.lang.JIter.of(mission_v.tasks))
            {
                switch ((t.kind()).ordinal())
                {
                    case 0 /*BUILD*/:
                    {
                        keys.add(com.oddlabs.tt.model.Unlocks.building(t.what()));
                        break;
                    }
                    case 1 /*RESEARCH*/:
                    {
                        keys.add(com.oddlabs.tt.model.Unlocks.upgrade(t.what()));
                        if (t.what() == com.oddlabs.tt.model.ResearchContainer.CRYSTAL_CRAFT || t.what() == com.oddlabs.tt.model.ResearchContainer.DIAMOND_CRAFT || t.what() == com.oddlabs.tt.model.ResearchContainer.CRYSTAL_FACET || t.what() == com.oddlabs.tt.model.ResearchContainer.DIAMOND_EDGE)
                        {
                            keys.add(com.oddlabs.tt.model.Unlocks.building(com.oddlabs.tt.model.Race.BUILDING_FORGE));
                            keys.add(com.oddlabs.tt.model.Unlocks.RARE);
                        }
                        break;
                    }
                    case 2 /*TRAIN*/:
                    {
                        keys.add(com.oddlabs.tt.model.Unlocks.unit(t.what()));
                        switch ((t.what()))
                        {
                            case 13:
                            {
                                keys.add(com.oddlabs.tt.model.Unlocks.building(com.oddlabs.tt.model.Race.BUILDING_FORGE));
                                keys.add(com.oddlabs.tt.model.Unlocks.upgrade(com.oddlabs.tt.model.ResearchContainer.CRYSTAL_CRAFT));
                                keys.add(com.oddlabs.tt.model.Unlocks.RARE);
                                break;
                            }
                            case 14:
                            case 16:
                            case 17:
                            {
                                if (t.what() != com.oddlabs.tt.model.Race.UNIT_DIAMOND)
                                {
                                    keys.add(com.oddlabs.tt.model.Unlocks.building(com.oddlabs.tt.model.Race.BUILDING_HALL));
                                }
                                keys.add(com.oddlabs.tt.model.Unlocks.building(com.oddlabs.tt.model.Race.BUILDING_FORGE));
                                keys.add(com.oddlabs.tt.model.Unlocks.upgrade(com.oddlabs.tt.model.ResearchContainer.DIAMOND_CRAFT));
                                keys.add(com.oddlabs.tt.model.Unlocks.unit(com.oddlabs.tt.model.Race.UNIT_DIAMOND));
                                keys.add(com.oddlabs.tt.model.Unlocks.RARE);
                                if (t.what() != com.oddlabs.tt.model.Race.UNIT_DIAMOND)
                                {
                                    keys.add(com.oddlabs.tt.model.Unlocks.building(com.oddlabs.tt.model.Race.BUILDING_HALL));
                                }
                                break;
                            }
                            case 20:
                            case 21:
                            case 22:
                            {
                                keys.add(com.oddlabs.tt.model.Unlocks.building(com.oddlabs.tt.model.Race.BUILDING_TOTEM));
                                keys.add(com.oddlabs.tt.model.Unlocks.RARE);
                                break;
                            }
                            case 11:
                            case 12:
                            case 15:
                            case 7:
                            {
                                keys.add(com.oddlabs.tt.model.Unlocks.building(com.oddlabs.tt.model.Race.BUILDING_HALL));
                                keys.add(com.oddlabs.tt.model.Unlocks.RARE);
                                break;
                            }
                            default:
                            {
                                break;
                            }
                        }
                        break;
                    }
                    case 4 /*GOLD*/:
                    case 5 /*CRYSTAL*/:
                    {
                        keys.add(com.oddlabs.tt.model.Unlocks.RARE);
                        break;
                    }
                    default:
                    {
                        break;
                    }
                }
            }
            int[] a = new int[keys.size()];
            int i = 0;
            foreach (int k_2 in java.lang.JIter.of(keys))
            {
                a[i++] = k_2;
            }
            return a;
        }
        public const int HOME_RESEARCH_COST_PERCENT = 300;
        // Java: 540
        public int storyLevel(int upgrade)
        {
            int n = 0;
            foreach (com.oddlabs.tt.player.campaign.SagaMission.Task t in java.lang.JIter.of(mission_f.tasks))
            {
                if (((object)t.kind() == (object)com.oddlabs.tt.player.campaign.SagaMission.TaskKind.RESEARCH) && t.what() == upgrade)
                {
                    n = java.lang.Math.max(n, t.count());
                }
            }
            foreach (int k in com.oddlabs.tt.player.campaign.SagaIsland.storyKeys(mission_f))
            {
                if (k == com.oddlabs.tt.model.Unlocks.upgrade(upgrade))
                {
                    n = java.lang.Math.max(n, 1);
                }
            }
            return n;
        }
        // Java: 552
        public static int unestablished(com.oddlabs.tt.player.campaign.CampaignState state)
        {
            int n = 0;
            foreach (int k in state.getSagaUnlocked())
            {
                if (!state.isSagaEstablished(k))
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 565
        public java.util.List establish(com.oddlabs.tt.player.Player me)
        {
            com.oddlabs.tt.player.campaign.CampaignState state = getCampaign().getState();
            java.util.List now = new java.util.ArrayList();
            if (state.getSagaEstablished() == null)
            {
                return now;
            }
            com.oddlabs.tt.model.Race race = me.getRace();
            foreach (int k in state.getSagaUnlocked())
            {
                if (state.isSagaEstablished(k))
                {
                    continue;
                }
                bool ok = false;
                string name = null;
                if (k >= 0 && k < com.oddlabs.tt.model.Race.NUM_BUILDINGS)
                {
                    foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(me.getUnits().getSet()))
                    {
                        if ((s is com.oddlabs.tt.model.LandBuilding b) && !b.isDead() && b.isComplete() && ((object)((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()) == (object)race.getBuildingTemplate(k)))
                        {
                            ok = true;
                        }
                    }
                    name = race.getBuildingTemplate(k).getName();
                }
                else if (k >= com.oddlabs.tt.model.Unlocks.unit(0) && k < com.oddlabs.tt.model.Unlocks.unit(com.oddlabs.tt.model.Race.NUM_UNITS))
                {
                    int u = k - com.oddlabs.tt.model.Unlocks.unit(0);
                    ok = com.oddlabs.tt.player.campaign.SagaIsland.census(me)[u] > 0;
                    if (u == com.oddlabs.tt.model.Race.UNIT_DIAMOND && !ok)
                    {
                        ok = me.getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.DIAMOND_CRAFT) > 0 && me.getRareHarvested(4) > 0;
                    }
                    name = race.getUnitTemplate(u).getName();
                }
                else if (com.oddlabs.tt.model.Unlocks.upgradeOf(k) >= 0)
                {
                    ok = me.getUpgradeLevel(com.oddlabs.tt.model.Unlocks.upgradeOf(k)) > 0;
                    name = com.oddlabs.tt.player.campaign.SagaIsland.i18n("upgrade_word");
                }
                else if (k >= com.oddlabs.tt.model.Unlocks.spell(0) && k < com.oddlabs.tt.model.Unlocks.spell(10))
                {
                    ok = true;
                    name = com.oddlabs.tt.player.campaign.SagaIsland.i18n("spell_word");
                }
                else if (k == com.oddlabs.tt.model.Unlocks.TIERS)
                {
                    foreach (com.oddlabs.tt.model.Selectable s_2 in java.lang.JIter.of(me.getUnits().getSet()))
                    {
                        if ((s_2 is com.oddlabs.tt.model.LandBuilding b_2) && !b_2.isDead() && b_2.getTier() > 0)
                        {
                            ok = true;
                        }
                    }
                    name = com.oddlabs.tt.player.campaign.SagaIsland.i18n("tiers_word");
                }
                else if (k == com.oddlabs.tt.model.Unlocks.RUNE_TIER)
                {
                    foreach (com.oddlabs.tt.model.Selectable s_3 in java.lang.JIter.of(me.getUnits().getSet()))
                    {
                        if ((s_3 is com.oddlabs.tt.model.LandBuilding b_3) && !b_3.isDead() && b_3.getTier() >= 3)
                        {
                            ok = true;
                        }
                    }
                    name = com.oddlabs.tt.player.campaign.SagaIsland.i18n("rune_tier_word");
                }
                else if (k == com.oddlabs.tt.model.Unlocks.RARE)
                {
                    for (int r = 0; r < 4; r++)
                    {
                        ok |= me.getRareHarvested(r) > 0;
                    }
                    name = com.oddlabs.tt.player.campaign.SagaIsland.i18n("rare_word");
                }
                else
                {
                    ok = true;
                }
                if (ok)
                {
                    state.sagaEstablish(k);
                    if (name != null && !now.contains(name))
                    {
                        now.add(name);
                    }
                }
            }
            return now;
        }
        // Java: 620
        public static void makeElemental(com.oddlabs.tt.player.Player p)
        {
            p.setElemental(true);
            foreach (int u in new int[] { com.oddlabs.tt.model.ResearchContainer.RACE_1, com.oddlabs.tt.model.ResearchContainer.ARMOR, com.oddlabs.tt.model.ResearchContainer.RANGE })
            {
                for (int l = 0; l < 2 && p.getUpgradeLevel(u) < com.oddlabs.tt.model.ResearchContainer.MAX_LEVEL; l++)
                {
                    p.upgrade(u);
                }
            }
        }
        // Java: 631
        public static int[] deposit(com.oddlabs.tt.player.Player me, int[] stock)
        {
            int[] left = ((int[])stock.clone());
            com.oddlabs.tt.model.Race race = me.getRace();
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(me.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Building b) && !b.isDead() && b.isComplete() && ((object)((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()) == (object)race.getBuildingTemplate(com.oddlabs.tt.model.Race.BUILDING_ARMORY)))
                {
                    for (int i = 0; i < left.Length && i < com.oddlabs.tt.model.ExtendedResources.NUM_STORES; i++)
                    {
                        com.oddlabs.tt.model.SupplyContainer c = b.getSupplyContainer(com.oddlabs.tt.model.ExtendedResources.STORES[i]);
                        if (c != null && left[i] > 0)
                        {
                            left[i] -= c.increaseSupply(left[i]);
                        }
                    }
                }
            }
            foreach (int v in left)
            {
                if (v > 0)
                {
                    return left;
                }
            }
            return null;
        }
        // Java: 649
        public static int countTrees(com.oddlabs.tt.landscape.World world)
        {
            int[] n = new int[] { 0 };
            world.getTreeRoot().visit(new SagaIsland.Anon1_TreeNodeVisitor(n));
            return n[0];
        }
        // Java: 672
        public void keepDepletion(com.oddlabs.tt.player.campaign.CampaignState state)
        {
            if (trees_at_start <= 0)
            {
                return;
            }
            float now = (float)(com.oddlabs.tt.player.campaign.SagaIsland.countTrees(getViewer().getWorld()) / ((float)(trees_at_start)));
            float before = (float)(1.0f - (float)(state.getIsleDepletion(mission_f.index) / 100.0f));
            state.setIsleDepletion(mission_f.index, java.lang.Math.round((float)(100.0f * ((float)(1.0f - (float)(java.lang.Math.max(0.25f, before) * java.lang.Math.min(1.0f, now)))))));
        }
        // Java: 680
        public static void spawn(com.oddlabs.tt.player.Player me, int unit, int n)
        {
            float a = (float)(n * 2.4f);
            float r = (float)(6.0f + (float)(n * 0.6f));
            new com.oddlabs.tt.model.Unit(me, (float)(me.getStartX() + (float)(r * ((float)(java.lang.StrictMath.cos(a))))), (float)(me.getStartY() + (float)(r * ((float)(java.lang.StrictMath.sin(a))))), null, me.getRace().getUnitTemplate(unit));
        }
        public const int CLANS = 16;
        public const int FRIEND_CLANS = 4;
        public const int ELEMENTAL_CLANS = 8;
        // Java: 692
        public static string clan(bool vikings, int k)
        {
            return com.oddlabs.tt.player.campaign.SagaIsland.i18n(java.lang.JStr.valueOf(((vikings ? "clan_v" : "clan_n"))) + (k));
        }
        // Java: 702
        public string[] clanNames(com.oddlabs.tt.player.campaign.CampaignState state)
        {
            string[] names = new string[6];
            java.util.Arrays.fill(names, "");
            bool own_vik = !nativeTribe();
            java.util.Random rnd = new java.util.Random(mission_f.index * 7919L + 17 + ((home ? state.getSagaVoyages() * 31L : 0)));
            java.util.List enemy_v = new java.util.ArrayList();
            java.util.List enemy_n = new java.util.ArrayList();
            java.util.List elemental = new java.util.ArrayList();
            for (int k = com.oddlabs.tt.player.campaign.SagaIsland.FRIEND_CLANS; k < com.oddlabs.tt.player.campaign.SagaIsland.CLANS; k++)
            {
                enemy_v.add(k);
                enemy_n.add(k);
            }
            for (int k_2 = 0; k_2 < com.oddlabs.tt.player.campaign.SagaIsland.ELEMENTAL_CLANS; k_2++)
            {
                elemental.add(k_2);
            }
            java.util.Collections.shuffle(enemy_v, rnd);
            java.util.Collections.shuffle(enemy_n, rnd);
            java.util.Collections.shuffle(elemental, rnd);
            int isle_friend = 1 + mission_f.index % (com.oddlabs.tt.player.campaign.SagaIsland.FRIEND_CLANS - 1);
            int slot = 1;
            if (mode == com.oddlabs.tt.player.campaign.SagaIsland.HOLD || mode == com.oddlabs.tt.player.campaign.SagaIsland.DEFEND || mode == com.oddlabs.tt.player.campaign.SagaIsland.HELP)
            {
                if (raids || mode == com.oddlabs.tt.player.campaign.SagaIsland.HELP)
                {
                    names[slot++] = com.oddlabs.tt.player.campaign.SagaIsland.clan(!own_vik, ((int)((int?)((own_vik ? enemy_n : enemy_v)).remove(0))));
                }
                if (mode == com.oddlabs.tt.player.campaign.SagaIsland.HELP || defense == com.oddlabs.tt.player.campaign.SagaCampaign.DEFEND_ALLY)
                {
                    names[slot] = com.oddlabs.tt.player.campaign.SagaIsland.clan(own_vik, (mode == com.oddlabs.tt.player.campaign.SagaIsland.HELP ? isle_friend : 0));
                }
                return names;
            }
            if (mode == com.oddlabs.tt.player.campaign.SagaIsland.VISIT)
            {
                names[1] = com.oddlabs.tt.player.campaign.SagaIsland.clan(own_vik, isle_friend);
                return names;
            }
            foreach (com.oddlabs.tt.player.campaign.SagaMission.Rival r in java.lang.JIter.of(mission_f.rivals))
            {
                if (slot >= names.Length)
                {
                    break;
                }
                bool vik = r.other_race() != own_vik;
                if (r.elemental())
                {
                    names[slot] = com.oddlabs.tt.player.campaign.SagaIsland.i18n("clan_e" + java.lang.JStr.valueOf(((int?)elemental.remove(0))));
                }
                else if (r.team() == 0)
                {
                    names[slot] = com.oddlabs.tt.player.campaign.SagaIsland.clan(vik, (mission_f.befriends_f ? isle_friend : (r.other_race() ? 0 : (slot == 1 ? 0 : isle_friend))));
                }
                else
                {
                    names[slot] = com.oddlabs.tt.player.campaign.SagaIsland.clan(vik, ((int)((int?)((vik ? enemy_v : enemy_n)).remove(0))));
                }
                slot++;
            }
            if (slot < names.Length)
            {
                names[slot] = com.oddlabs.tt.player.campaign.SagaIsland.clan(own_vik, 0);
            }
            return names;
        }
        // Java: 749
        public int population()
        {
            return (mission_f.index == com.oddlabs.tt.player.campaign.SagaCampaign.HOME ? saga().homeCap() : ((mode == com.oddlabs.tt.player.campaign.SagaIsland.VISIT ? com.oddlabs.tt.player.campaign.SagaIsland.VISIT_GARRISON : saga().isleCap(mission_f.index))) + expeditionSize());
        }
        // Java: 754
        public int expeditionSize()
        {
            int n = 0;
            for (int u = 0; u < com.oddlabs.tt.player.campaign.CampaignState.SAGA_UNITS && u < expedition.Length; u++)
            {
                n += expedition[u];
            }
            return n;
        }
        // Java: 765
        public void rebuildHolding(com.oddlabs.tt.player.Player me, com.oddlabs.tt.player.campaign.CampaignState state, int gx, int gy)
        {
            int[] b = state.getIsleBuildings(mission_f.index);
            bool quarters = false;
            bool armory_built = false;
            for (int i = 0; i + 2 < b.Length; i += 3)
            {
                quarters |= b[i] == com.oddlabs.tt.model.Race.BUILDING_QUARTERS;
                armory_built |= b[i] == com.oddlabs.tt.model.Race.BUILDING_ARMORY;
            }
            if (!quarters)
            {
                me.buildBuilding(com.oddlabs.tt.model.Race.BUILDING_QUARTERS, gx, gy);
            }
            if (!armory_built)
            {
                me.buildBuilding(com.oddlabs.tt.model.Race.BUILDING_ARMORY, gx, gy);
            }
            com.oddlabs.tt.model.Building[] built = new com.oddlabs.tt.model.Building[b.Length / 3];
            for (int i_2 = 0; i_2 + 2 < b.Length; i_2 += 3)
            {
                if (b[i_2] != com.oddlabs.tt.model.Race.BUILDING_SHIP && b[i_2] >= 0 && b[i_2] < com.oddlabs.tt.model.Race.NUM_BUILDINGS && !me.isLocked(com.oddlabs.tt.model.Unlocks.building(b[i_2])))
                {
                    built[i_2 / 3] = me.buildBuilding(b[i_2], gx + b[i_2 + 1], gy + b[i_2 + 2]);
                }
            }
            int[] units = ((int[])state.getIsleUnits(mission_f.index).clone());
            int cap = population();
            int n = 0;
            int[] people_v = state.getIslePeople(mission_f.index);
            int grid = me.getWorld().getUnitGrid().getGridSize();
            for (int i_3 = 0; i_3 + 3 < people_v.Length && n < cap; i_3 += 4)
            {
                int u = people_v[i_3];
                if (u < 0 || u >= units.Length || units[u] <= 0)
                {
                    continue;
                }
                units[u]--;
                com.oddlabs.tt.model.Building @in = (people_v[i_3 + 3] >= 0 && people_v[i_3 + 3] < built.Length ? built[people_v[i_3 + 3]] : (com.oddlabs.tt.model.Building)(null));
                int x = java.lang.Math.clamp(gx + people_v[i_3 + 1], 1, grid - 2);
                int y = java.lang.Math.clamp(gy + people_v[i_3 + 2], 1, grid - 2);
                float wx = (@in != null ? @in.getPositionX() : (people_v[i_3 + 3] == (-2) ? me.getStartX() : com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(x)));
                float wy = (@in != null ? @in.getPositionY() : (people_v[i_3 + 3] == (-2) ? me.getStartY() : com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(y)));
                com.oddlabs.tt.model.Unit unit = new com.oddlabs.tt.model.Unit(me, wx, wy, null, me.getRace().getUnitTemplate(u));
                n++;
                if (@in != null && !@in.isDead() && @in.isComplete() && @in.getUnitContainer() != null && @in.getUnitContainer().canEnter(unit))
                {
                    @in.getUnitContainer().enter(unit);
                }
            }
            for (int u_2 = 0; u_2 < units.Length; u_2++)
            {
                for (int k = 0; k < units[u_2] + ((u_2 < expedition.Length && mission_f.index != com.oddlabs.tt.player.campaign.SagaCampaign.HOME ? expedition[u_2] : 0)) && n < cap; k++)
                {
                    com.oddlabs.tt.player.campaign.SagaIsland.spawn(me, u_2, n++);
                }
            }
            int[] stock = ((int[])state.getIsleStock(mission_f.index).clone());
            if (mission_f.index != com.oddlabs.tt.player.campaign.SagaCampaign.HOME)
            {
                for (int i_4 = 0; i_4 < stock.Length; i_4++)
                {
                    stock[i_4] += expedition[com.oddlabs.tt.player.campaign.CampaignState.SAGA_UNITS + i_4];
                }
            }
            com.oddlabs.tt.player.campaign.SagaIsland.deposit(me, stock);
            com.oddlabs.tt.player.campaign.SagaIsland.depositWeapons(me, state.getIsleWeapons(mission_f.index));
        }
        // Java: 819
        public void embark(com.oddlabs.tt.player.Player me)
        {
            com.oddlabs.tt.pathfinder.UnitGrid grid = me.getWorld().getUnitGrid();
            int[] sea = com.oddlabs.tt.player.campaign.SagaIsland.openSea(grid, com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(me.getStartX()), com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(me.getStartY()));
            if (sea == null)
            {
                return;
            }
            java.util.List crew = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(me.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Unit u) && !u.isDead() && !u.isMounted())
                {
                    crew.add(u);
                }
            }
            com.oddlabs.tt.model.UnitTemplate peon = me.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON);
            crew.sort(new java.util.Comparator_L((object a_l, object b_l) =>
            {
                com.oddlabs.tt.model.Unit a = (com.oddlabs.tt.model.Unit)a_l;
                com.oddlabs.tt.model.Unit b = (com.oddlabs.tt.model.Unit)b_l;
                return java.lang.Integer.compare((((object)((com.oddlabs.tt.model.UnitTemplate)a.getTemplate()) == (object)peon) ? 0 : (a.isWarrior() ? 1 : 2)), (((object)((com.oddlabs.tt.model.UnitTemplate)b.getTemplate()) == (object)peon) ? 0 : (b.isWarrior() ? 1 : 2)));
            }));
            java.util.List ships = new java.util.ArrayList();
            com.oddlabs.tt.model.Ship ship = null;
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(crew))
            {
                if (ship == null || !(((com.oddlabs.tt.model.ShipUnitContainer)ship.getUnitContainer())).canEnter(u_2))
                {
                    if (ships.size() >= java.lang.Math.max((mission_f.archipelago_f ? 2 : 1), java.lang.Math.min(4, me.getFleetCap())))
                    {
                        break;
                    }
                    int k = ships.size();
                    int sx = sea[0] + ((k % 2 == 0 ? 1 : (-1))) * 7 * ((k + 1) / 2);
                    int sy = sea[1] + ((k % 2 == 0 ? (-1) : 1)) * 3 * ((k + 1) / 2);
                    if (!com.oddlabs.tt.player.campaign.SagaIsland.clearWater(grid, sx, sy, 4))
                    {
                        sx = sea[0];
                        sy = sea[1];
                        if (k > 0)
                        {
                            break;
                        }
                    }
                    com.oddlabs.tt.model.Building b_2 = me.getRace().getBuildingTemplate(com.oddlabs.tt.model.Race.BUILDING_SHIP).create(me, sx, sy);
                    if (!((b_2 is com.oddlabs.tt.model.Ship s_2)))
                    {
                        break;
                    }
                    s_2.instantBuild();
                    ships.add(s_2);
                    ship = s_2;
                    if (!(((com.oddlabs.tt.model.ShipUnitContainer)ship.getUnitContainer())).canEnter(u_2))
                    {
                        continue;
                    }
                }
                (((com.oddlabs.tt.model.ShipUnitContainer)ship.getUnitContainer())).enter(u_2);
            }
            fleet.addAll(ships);
            foreach (com.oddlabs.tt.model.Ship s_3 in java.lang.JIter.of(ships))
            {
                fleet_from.add(new float[] { s_3.getPositionX(), s_3.getPositionY() });
            }
            if (!ships.isEmpty())
            {
                getViewer().getCamera().reset(((com.oddlabs.tt.model.Ship)ships.getFirst()).getPositionX(), ((com.oddlabs.tt.model.Ship)ships.getFirst()).getPositionY());
            }
        }
        // Java: 867
        public void land(com.oddlabs.tt.player.Player me)
        {
            if (fleet.isEmpty())
            {
                return;
            }
            for (int i = fleet.size() - 1; i >= 0; i--)
            {
                com.oddlabs.tt.model.Ship ship = ((com.oddlabs.tt.model.Ship)fleet.get(i));
                if (ship.isDead() || ship.getShipHR() == null)
                {
                    fleet.remove(i);
                    fleet_from.remove(i);
                    continue;
                }
                float dx = (float)(ship.getPositionX() - ((float[])fleet_from.get(i))[0]);
                float dy = (float)(ship.getPositionY() - ((float[])fleet_from.get(i))[1]);
                if (ship.isMoving() || (float)((float)(dx * dx) + (float)(dy * dy)) < 100.0f || ((object)ship.getEntrance() == (object)ship))
                {
                    continue;
                }
                foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(ship.getShipHR().crew()))
                {
                    if (!u.isDead())
                    {
                        ship.getShipHR().disembark(u);
                    }
                }
                fleet.remove(i);
                fleet_from.remove(i);
                if (fleet.isEmpty())
                {
                    addModalForm(new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), getHeader(), com.oddlabs.tt.player.campaign.SagaIsland.i18n("landed"), face(com.oddlabs.tt.player.campaign.SagaCampaign.FACE_ADVISOR), com.oddlabs.tt.gui.Origin.AT_START));
                }
            }
        }
        // Java: 893
        public static void harbour(com.oddlabs.tt.player.Player p, int n)
        {
            com.oddlabs.tt.pathfinder.UnitGrid grid = p.getWorld().getUnitGrid();
            int[] sea = com.oddlabs.tt.player.campaign.SagaIsland.openSea(grid, com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(p.getStartX()), com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(p.getStartY()));
            if (sea == null)
            {
                return;
            }
            for (int k = 0; k < n; k++)
            {
                int sx = sea[0] + ((k % 2 == 0 ? 1 : (-1))) * 6 * ((k + 1) / 2);
                int sy = sea[1];
                if (!com.oddlabs.tt.player.campaign.SagaIsland.clearWater(grid, sx, sy, 4))
                {
                    if (k > 0)
                    {
                        return;
                    }
                    sx = sea[0];
                    sy = sea[1];
                }
                if ((p.getRace().getBuildingTemplate(com.oddlabs.tt.model.Race.BUILDING_SHIP).create(p, sx, sy) is com.oddlabs.tt.model.Ship s))
                {
                    s.instantBuild();
                }
            }
        }
        // Java: 912
        public static int[] openSea(com.oddlabs.tt.pathfinder.UnitGrid grid, int gx, int gy)
        {
            int size = grid.getGridSize();
            for (int r = 18; r < size / 2; r += 2)
            {
                for (int i = -r; i <= r; i += 2)
                {
                    foreach (int[] c in new int[][] { new int[] { gx + i, gy - r }, new int[] { gx + i, gy + r }, new int[] { gx - r, gy + i }, new int[] { gx + r, gy + i } })
                    {
                        if (c[0] > 12 && c[1] > 12 && c[0] < size - 12 && c[1] < size - 12 && com.oddlabs.tt.player.campaign.SagaIsland.clearWater(grid, c[0], c[1], 6))
                        {
                            return c;
                        }
                    }
                }
            }
            return null;
        }
        // Java: 922
        public static bool clearWater(com.oddlabs.tt.pathfinder.UnitGrid grid, int x, int y, int radius)
        {
            int size = grid.getGridSize();
            for (int dy = -radius; dy <= radius; dy++)
            {
                for (int dx = -radius; dx <= radius; dx++)
                {
                    int cx = x + dx;
                    int cy = y + dy;
                    if (cx < 0 || cy < 0 || cx >= size || cy >= size || !grid.isWater(cx, cy) || grid.isGridOccupied(cx, cy, com.oddlabs.tt.pathfinder.UnitGrid.SEA))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        // Java: 940
        public void keepHolding()
        {
            if (getViewer() == null || mode == com.oddlabs.tt.player.campaign.SagaIsland.MISSION || mode == com.oddlabs.tt.player.campaign.SagaIsland.HELP || settled)
            {
                return;
            }
            com.oddlabs.tt.player.Player me = getViewer().getLocalPlayer();
            com.oddlabs.tt.player.campaign.CampaignState state = getCampaign().getState();
            int island = mission_f.index;
            java.util.List kept = com.oddlabs.tt.player.campaign.SagaIsland.keptBuildings(me);
            state.setIsleBuildings(island, com.oddlabs.tt.player.campaign.SagaIsland.buildings(me, kept));
            state.setIsleBuildings(island + com.oddlabs.tt.player.campaign.SagaIsland.ALLY_BUILDINGS, com.oddlabs.tt.player.campaign.SagaIsland.allyBuildings(me));
            state.setIsleWeapons(island, com.oddlabs.tt.player.campaign.SagaIsland.armoryWeapons(me));
            int[] people_v = com.oddlabs.tt.player.campaign.SagaIsland.census(me);
            int[] stock = com.oddlabs.tt.player.campaign.SagaIsland.armoryStock(me);
            for (int i = 0; i < stock.Length; i++)
            {
                stock[i] += ((pending != null ? pending[i] : 0));
            }
            if (island == com.oddlabs.tt.player.campaign.SagaCampaign.HOME)
            {
                int[] staying = com.oddlabs.tt.player.campaign.SagaIsland.capped(people_v, saga().homeCap());
                state.setIsleUnits(island, staying);
                state.setIslePeople(island, com.oddlabs.tt.player.campaign.SagaIsland.places(me, kept, staying));
                state.setIsleStock(island, stock);
            }
            else
            {
                int[] home_units = ((int[])base_home_units.clone());
                int[] home_stock = ((int[])base_home_stock.clone());
                for (int u = 0; u < home_units.Length && u < expedition.Length; u++)
                {
                    home_units[u] = java.lang.Math.max(0, home_units[u] - expedition[u]);
                }
                for (int i_2 = 0; i_2 < home_stock.Length; i_2++)
                {
                    home_stock[i_2] = java.lang.Math.max(0, home_stock[i_2] - expedition[com.oddlabs.tt.player.campaign.CampaignState.SAGA_UNITS + i_2]);
                }
                int[] garrison = com.oddlabs.tt.player.campaign.SagaIsland.capped(people_v, (mode == com.oddlabs.tt.player.campaign.SagaIsland.VISIT ? com.oddlabs.tt.player.campaign.SagaIsland.VISIT_GARRISON : saga().isleCap(island)));
                int room = saga().homeCap() - com.oddlabs.tt.player.campaign.SagaIsland.sum(home_units);
                for (int u_2 = 0; u_2 < people_v.Length; u_2++)
                {
                    int k = java.lang.Math.max(0, java.lang.Math.min(room, people_v[u_2] - garrison[u_2]));
                    home_units[u_2] += k;
                    room -= k;
                }
                state.setIsleUnits(island, garrison);
                state.setIslePeople(island, com.oddlabs.tt.player.campaign.SagaIsland.places(me, kept, garrison));
                state.setIsleStock(island, stock);
                state.setIsleUnits(com.oddlabs.tt.player.campaign.SagaCampaign.HOME, home_units);
                state.setIsleStock(com.oddlabs.tt.player.campaign.SagaCampaign.HOME, home_stock);
            }
            keepResearch(me, state);
            keepDepletion(state);
        }
        // Java: 982
        public static int sum(int[] a)
        {
            int n = 0;
            foreach (int v in a)
            {
                n += v;
            }
            return n;
        }
        // Java: 990
        public static int[] capped(int[] people_v, int cap)
        {
            int[] a = new int[people_v.Length];
            int left = cap;
            bool any = true;
            while (left > 0 && any)
            {
                any = false;
                for (int u = 0; u < people_v.Length && left > 0; u++)
                {
                    if (a[u] < people_v[u])
                    {
                        a[u]++;
                        left--;
                        any = true;
                    }
                }
            }
            return a;
        }
        // Java: 1010
        public static int[] census(com.oddlabs.tt.player.Player me)
        {
            int[] counts = new int[com.oddlabs.tt.player.campaign.CampaignState.SAGA_UNITS];
            foreach (com.oddlabs.tt.model.Unit unit in java.lang.JIter.of(com.oddlabs.tt.player.campaign.SagaIsland.people(me)))
            {
                int u = com.oddlabs.tt.player.campaign.SagaIsland.unitIndex(me, unit);
                if (u >= 0)
                {
                    counts[u]++;
                }
            }
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(me.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.LandBuilding b) && !b.isDead())
                {
                    counts[com.oddlabs.tt.model.Race.UNIT_PEON] += com.oddlabs.tt.player.campaign.SagaIsland.insidePeons(b);
                }
            }
            return counts;
        }
        // Java: 1023
        public static int unitIndex(com.oddlabs.tt.player.Player me, com.oddlabs.tt.model.Unit unit)
        {
            com.oddlabs.tt.model.Race race = me.getRace();
            for (int u = 0; u < com.oddlabs.tt.player.campaign.CampaignState.SAGA_UNITS; u++)
            {
                if ((u == com.oddlabs.tt.model.Race.UNIT_PEON || com.oddlabs.tt.player.campaign.SagaIsland.isWarrior(u)) && ((object)((com.oddlabs.tt.model.UnitTemplate)unit.getTemplate()) == (object)race.getUnitTemplate(u)))
                {
                    return u;
                }
            }
            return (-1);
        }
        // Java: 1032
        public static int insidePeons(com.oddlabs.tt.model.LandBuilding b)
        {
            com.oddlabs.tt.model.UnitContainer c = b.getUnitContainer();
            if (c == null || (c is com.oddlabs.tt.model.MountUnitContainer))
            {
                return 0;
            }
            return java.lang.Math.max(0, c.getNumSupplies());
        }
        // Java: 1040
        public static java.util.List keptBuildings(com.oddlabs.tt.player.Player me)
        {
            java.util.List l = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(me.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.LandBuilding b) && !b.isDead() && b.isComplete() && ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID() != com.oddlabs.tt.model.Race.BUILDING_BRIDGE)
                {
                    l.add(b);
                }
            }
            return l;
        }
        // Java: 1053
        public static int[] places(com.oddlabs.tt.player.Player me, java.util.List kept, int[] staying)
        {
            int gx = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(me.getStartX());
            int gy = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(me.getStartY());
            int[] left = ((int[])staying.clone());
            java.util.List l = new java.util.ArrayList();
            for (int i = 0; i < kept.size(); i++)
            {
                com.oddlabs.tt.model.LandBuilding b = ((com.oddlabs.tt.model.LandBuilding)kept.get(i));
                if ((b.getUnitContainer() is com.oddlabs.tt.model.MountUnitContainer m) && m.getUnit() != null)
                {
                    int u = com.oddlabs.tt.player.campaign.SagaIsland.unitIndex(me, m.getUnit());
                    if (u >= 0 && left[u] > 0)
                    {
                        left[u]--;
                        l.add(u);
                        l.add(b.getGridX() - gx);
                        l.add(b.getGridY() - gy);
                        l.add(i);
                    }
                }
                for (int k = com.oddlabs.tt.player.campaign.SagaIsland.insidePeons(b); k > 0 && left[com.oddlabs.tt.model.Race.UNIT_PEON] > 0; k--)
                {
                    left[com.oddlabs.tt.model.Race.UNIT_PEON]--;
                    l.add(com.oddlabs.tt.model.Race.UNIT_PEON);
                    l.add(b.getGridX() - gx);
                    l.add(b.getGridY() - gy);
                    l.add(i);
                }
            }
            java.util.Set aboard = new java.util.HashSet();
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(me.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Ship ship) && !ship.isDead() && ship.getShipHR() != null)
                {
                    aboard.addAll(ship.getShipHR().crew());
                }
            }
            foreach (com.oddlabs.tt.model.Unit unit in java.lang.JIter.of(com.oddlabs.tt.player.campaign.SagaIsland.people(me)))
            {
                int u_2 = com.oddlabs.tt.player.campaign.SagaIsland.unitIndex(me, unit);
                if (u_2 < 0 || left[u_2] <= 0)
                {
                    continue;
                }
                left[u_2]--;
                l.add(u_2);
                l.add(unit.getGridX() - gx);
                l.add(unit.getGridY() - gy);
                l.add((aboard.contains(unit) ? (-2) : (-1)));
            }
            int[] a = new int[l.size()];
            for (int i_2 = 0; i_2 < a.Length; i_2++)
            {
                a[i_2] = ((int)((int?)l.get(i_2)));
            }
            return a;
        }
        public static System.Type[] WEAPONS;
        // Java: 1096
        public static int[] armoryWeapons(com.oddlabs.tt.player.Player me)
        {
            int[] w = new int[com.oddlabs.tt.player.campaign.CampaignState.SAGA_WEAPONS];
            com.oddlabs.tt.model.Race race = me.getRace();
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(me.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Building b) && !b.isDead() && ((object)((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()) == (object)race.getBuildingTemplate(com.oddlabs.tt.model.Race.BUILDING_ARMORY)))
                {
                    for (int i = 0; i < w.Length; i++)
                    {
                        com.oddlabs.tt.model.SupplyContainer c = b.getSupplyContainer(com.oddlabs.tt.player.campaign.SagaIsland.WEAPONS[i]);
                        if (c != null)
                        {
                            w[i] += c.getNumSupplies();
                        }
                    }
                }
            }
            return w;
        }
        // Java: 1110
        public static void depositWeapons(com.oddlabs.tt.player.Player me, int[] weapons)
        {
            int[] left = ((int[])weapons.clone());
            com.oddlabs.tt.model.Race race = me.getRace();
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(me.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Building b) && !b.isDead() && b.isComplete() && ((object)((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()) == (object)race.getBuildingTemplate(com.oddlabs.tt.model.Race.BUILDING_ARMORY)))
                {
                    for (int i = 0; i < left.Length && i < com.oddlabs.tt.player.campaign.SagaIsland.WEAPONS.Length; i++)
                    {
                        com.oddlabs.tt.model.SupplyContainer c = b.getSupplyContainer(com.oddlabs.tt.player.campaign.SagaIsland.WEAPONS[i]);
                        if (c != null && left[i] > 0)
                        {
                            left[i] -= c.increaseSupply(left[i]);
                        }
                    }
                }
            }
        }
        // Java: 1123
        public static int[] armoryStock(com.oddlabs.tt.player.Player me)
        {
            int[] stock = new int[com.oddlabs.tt.player.campaign.CampaignState.SAGA_STOCKS];
            com.oddlabs.tt.model.Race race = me.getRace();
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(me.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Building b) && !b.isDead() && ((object)((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()) == (object)race.getBuildingTemplate(com.oddlabs.tt.model.Race.BUILDING_ARMORY)))
                {
                    for (int i = 0; i < stock.Length && i < com.oddlabs.tt.model.ExtendedResources.NUM_STORES; i++)
                    {
                        com.oddlabs.tt.model.SupplyContainer c = b.getSupplyContainer(com.oddlabs.tt.model.ExtendedResources.STORES[i]);
                        if (c != null)
                        {
                            stock[i] += c.getNumSupplies();
                        }
                    }
                }
            }
            return stock;
        }
        // Java: 1140
        public void lost()
        {
            if (settled)
            {
                return;
            }
            settled = true;
            com.oddlabs.tt.player.campaign.CampaignState state = getCampaign().getState();
            if ((mode == com.oddlabs.tt.player.campaign.SagaIsland.HOLD || mode == com.oddlabs.tt.player.campaign.SagaIsland.VISIT) && getViewer() != null)
            {
                mode_keep();
                return;
            }
            int[] home_units = state.getSagaUnits();
            int[] home_stock = state.getSagaStock();
            for (int u = 0; u < home_units.Length && u < expedition.Length; u++)
            {
                home_units[u] = java.lang.Math.max(0, home_units[u] - expedition[u]);
            }
            for (int i = 0; i < home_stock.Length; i++)
            {
                home_stock[i] = java.lang.Math.max(0, home_stock[i] - expedition[com.oddlabs.tt.player.campaign.CampaignState.SAGA_UNITS + i]);
            }
            state.clearSagaExpedition();
            if (mode == com.oddlabs.tt.player.campaign.SagaIsland.DEFEND)
            {
                saga().loseIsland(mission_f.index, 0.3f);
                state.setSagaRaid(null);
            }
            if (mode == com.oddlabs.tt.player.campaign.SagaIsland.HELP)
            {
                state.setAllyIsle(mission_f.index, false);
                state.forceIslandState(mission_f.index, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE);
                state.loseSagaAlly();
                state.setSagaRaid(null);
            }
        }
        // Java: 1167
        public void mode_keep()
        {
            settled = false;
            keepHolding();
            settled = true;
        }
        // Java: 1173
        public static int[] buildings(com.oddlabs.tt.player.Player me, java.util.List kept)
        {
            int gx = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(me.getStartX());
            int gy = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(me.getStartY());
            java.util.List l = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.LandBuilding b in java.lang.JIter.of(kept))
            {
                l.add(((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID());
                l.add(b.getGridX() - gx);
                l.add(b.getGridY() - gy);
            }
            int[] a = new int[l.size()];
            for (int i = 0; i < a.Length; i++)
            {
                a[i] = ((int)((int?)l.get(i)));
            }
            return a;
        }
        // Java: 1188
        public static java.util.List people(com.oddlabs.tt.player.Player me)
        {
            java.util.Set seen = new java.util.HashSet();
            java.util.List l = new java.util.ArrayList();
            com.oddlabs.tt.model.UnitTemplate chief = me.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_CHIEFTAIN);
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(me.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Unit u) && !u.isDead() && ((object)((com.oddlabs.tt.model.UnitTemplate)u.getTemplate()) != (object)chief) && seen.add(u))
                {
                    l.add(u);
                }
                if ((s is com.oddlabs.tt.model.Ship ship) && !ship.isDead() && ship.getShipHR() != null)
                {
                    foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(ship.getShipHR().crew()))
                    {
                        if (!u_2.isDead() && ((object)((com.oddlabs.tt.model.UnitTemplate)u_2.getTemplate()) != (object)chief) && seen.add(u_2))
                        {
                            l.add(u_2);
                        }
                    }
                }
                if ((s is com.oddlabs.tt.model.LandBuilding b) && !b.isDead() && (b.getUnitContainer() is com.oddlabs.tt.model.MountUnitContainer m) && m.getUnit() != null && !m.getUnit().isDead() && seen.add(m.getUnit()))
                {
                    l.add(m.getUnit());
                }
            }
            return l;
        }
        // Java: 1208
        public void keepResearch(com.oddlabs.tt.player.Player me, com.oddlabs.tt.player.campaign.CampaignState state)
        {
            if (mission_f.index != com.oddlabs.tt.player.campaign.SagaCampaign.HOME)
            {
                return;
            }
            int[] research = state.getSagaResearch();
            for (int u = 0; u < com.oddlabs.tt.model.ResearchContainer.NUM_UPGRADES; u++)
            {
                research[u] = java.lang.Math.max(research[u], me.getUpgradeLevel(u));
            }
        }
        // Java: 1217
        public static com.oddlabs.tt.player.Player raider(com.oddlabs.tt.landscape.World world, com.oddlabs.tt.player.Player me)
        {
            foreach (com.oddlabs.tt.player.Player p in world.getPlayers())
            {
                if (((object)p != (object)me) && me.isEnemy(p) && p.getPlayerInfo().getTeam() != com.oddlabs.tt.landscape.World.ZOMBIE_TEAM)
                {
                    return p;
                }
            }
            return null;
        }
        // Java: 1225
        public static bool standing(com.oddlabs.tt.player.Player p)
        {
            return p.getUnitCountContainer().getNumSupplies() > 0 || p.hasActiveChieftain() || com.oddlabs.tt.player.campaign.SagaIsland.hasBuildings(p);
        }
        // Java: 1230
        public static int buildingsOf(com.oddlabs.tt.player.Player p)
        {
            int n = 0;
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(p.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.LandBuilding b) && !b.isDead())
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 1238
        public static bool hasBuildings(com.oddlabs.tt.player.Player me)
        {
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(me.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.LandBuilding b) && !b.isDead())
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 1246
        public int difficultyStep()
        {
            return ((getCampaign().getState().getDifficulty()) switch { 1 => (int)((-1)), 2 => (int)(1), _ => (int)(0) });
        }
        // Java: 1255
        public float reinforcementInterval()
        {
            return java.lang.Math.max(130.0f, (float)((float)(300.0f - (float)(45.0f * java.lang.Math.clamp(mission_f.chapter_f, 0, 2))) - (float)(50.0f * difficultyStep())));
        }
        // Java: 1263
        public void village(com.oddlabs.tt.player.Player p)
        {
            int ch = java.lang.Math.clamp(mission_f.chapter_f, 0, 2);
            int gx = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(p.getStartX());
            int gy = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(p.getStartY());
            int center = p.getWorld().getHeightMap().getGridUnitsPerWorld() / 2;
            float dx = center - gx;
            float dy = center - gy;
            float l = ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy)))));
            if (l < 1.0f)
            {
                dx = 1.0f;
                l = 1.0f;
            }
            dx = (float)(dx / l);
            dy = (float)(dy / l);
            if (ch >= 1)
            {
                p.buildBuilding(com.oddlabs.tt.model.Race.BUILDING_QUARTERS, java.lang.JC.f2i(((float)(gx - (float)(dy * 12.0f)))), java.lang.JC.f2i(((float)(gy + (float)(dx * 12.0f)))));
            }
            if (ch >= 2 || difficultyStep() > 0 && ch >= 1)
            {
                p.buildBuilding(com.oddlabs.tt.model.Race.BUILDING_HALL, java.lang.JC.f2i(((float)(gx + (float)(dy * 12.0f)))), java.lang.JC.f2i(((float)(gy - (float)(dx * 12.0f)))));
            }
            if (ch >= 1)
            {
                p.buildBuilding(com.oddlabs.tt.model.Race.BUILDING_PALISADE, java.lang.JC.f2i(((float)(gx + (float)(dx * 9.0f)))), java.lang.JC.f2i(((float)(gy + (float)(dy * 9.0f)))));
            }
        }
        // Java: 1287
        public void reinforce(com.oddlabs.tt.player.Player me)
        {
            if (elapsed < next_reinforcement)
            {
                return;
            }
            next_reinforcement = (float)(elapsed + reinforcementInterval());
            int ch = java.lang.Math.clamp(mission_f.chapter_f, 0, 2);
            int step = difficultyStep();
            foreach (com.oddlabs.tt.player.Player p in getViewer().getWorld().getPlayers())
            {
                if (((object)p == (object)me) || !me.isEnemy(p) || p.getPlayerInfo().getTeam() == com.oddlabs.tt.landscape.World.ZOMBIE_TEAM || !com.oddlabs.tt.player.campaign.SagaIsland.hasBuildings(p))
                {
                    continue;
                }
                int size = java.lang.Math.min(24, java.lang.Math.max(2, 3 + 2 * ch + 2 * step + reinforcements_sent));
                com.oddlabs.tt.model.Race race = p.getRace();
                java.util.List band = new java.util.ArrayList();
                for (int k = 0; k < size; k++)
                {
                    float a = (float)(k * 2.4f);
                    float r = (float)(3.0f + (float)(k * 0.5f));
                    band.add(new com.oddlabs.tt.model.Unit(p, (float)(p.getStartX() + (float)(r * ((float)(java.lang.StrictMath.cos(a))))), (float)(p.getStartY() + (float)(r * ((float)(java.lang.StrictMath.sin(a))))), null, race.getUnitTemplate(com.oddlabs.tt.player.campaign.SagaIsland.raidWarrior(k, 1 + ch * 2 + step + reinforcements_sent / 2))));
                }
                com.oddlabs.tt.pathfinder.UnitGrid grid = p.getWorld().getUnitGrid();
                int lx = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(p.getStartX());
                int ly = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(p.getStartY());
                int[] sea = com.oddlabs.tt.player.campaign.SagaIsland.openSea(grid, lx, ly);
                if (sea == null)
                {
                    com.oddlabs.tt.player.campaign.SagaIsland.sendHalf(me, p, band);
                    continue;
                }
                com.oddlabs.tt.model.Ship ship = null;
                foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(band))
                {
                    if (ship == null || !(((com.oddlabs.tt.model.ShipUnitContainer)ship.getUnitContainer())).canEnter(u))
                    {
                        com.oddlabs.tt.model.Building s = race.getBuildingTemplate(com.oddlabs.tt.model.Race.BUILDING_SHIP).create(p, sea[0], sea[1]);
                        if (!((s is com.oddlabs.tt.model.Ship sh)))
                        {
                            continue;
                        }
                        sh.instantBuild();
                        ship = sh;
                        reinf_fleet.add(sh);
                        reinf_from.add(new float[] { sh.getPositionX(), sh.getPositionY() });
                        reinf_owner.add(p);
                        p.setLandscapeTarget(com.oddlabs.tt.model.Selectable.newArray(sh), lx, ly, com.oddlabs.tt.model.Action.MOVE, false);
                    }
                    if ((((com.oddlabs.tt.model.ShipUnitContainer)ship.getUnitContainer())).canEnter(u))
                    {
                        (((com.oddlabs.tt.model.ShipUnitContainer)ship.getUnitContainer())).enter(u);
                    }
                }
            }
            reinforcements_sent++;
        }
        // Java: 1337
        public void allyReinforce(com.oddlabs.tt.player.Player me)
        {
            if (elapsed < next_ally_check)
            {
                return;
            }
            next_ally_check = (float)(elapsed + 10.0f);
            com.oddlabs.tt.player.campaign.CampaignState state = getCampaign().getState();
            int isles = state.getAllyIsleCount();
            foreach (com.oddlabs.tt.player.Player p in getViewer().getWorld().getPlayers())
            {
                if (((object)p == (object)me) || me.isEnemy(p) || p.getAI() == null || p.getPlayerInfo().getTeam() == com.oddlabs.tt.landscape.World.ZOMBIE_TEAM || !com.oddlabs.tt.player.campaign.SagaIsland.hasBuildings(p))
                {
                    continue;
                }
                bool own_isle = mode == com.oddlabs.tt.player.campaign.SagaIsland.HELP && ((object)p == (object)defended);
                int warriors_v = com.oddlabs.tt.player.campaign.SagaIsland.warriors(p);
                int[] call = ((int[])ally_call.get(p));
                if (call == null)
                {
                    if (elapsed < 8.0f)
                    {
                        continue;
                    }
                    int reserve = (own_isle || isles > 0 ? 6 + 6 * isles + 2 * state.getSagaFriendship() + ((own_isle ? 8 : 0)) : 0);
                    ally_call.put(p, call = new int[] { java.lang.Math.max(6, warriors_v), java.lang.Math.min(60, reserve), 0 });
                }
                if (call[1] <= 0 || elapsed < call[2] || warriors_v * 5 > call[0] * 3)
                {
                    continue;
                }
                int size = java.lang.Math.min(call[1], java.lang.Math.min(16, java.lang.Math.max(4, call[0] - warriors_v)));
                if (sendBand(p, size, 1 + java.lang.Math.clamp(mission_f.chapter_f, 0, 2) * 2 + isles))
                {
                    call[1] -= size;
                }
                call[2] = java.lang.JC.f2i(elapsed) + 120;
            }
        }
        // Java: 1365
        public static int warriors(com.oddlabs.tt.player.Player p)
        {
            int n = 0;
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(p.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Unit u) && !u.isDead() && u.isWarrior())
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 1374
        public bool sendBand(com.oddlabs.tt.player.Player p, int size, int threat)
        {
            com.oddlabs.tt.model.Race race = p.getRace();
            com.oddlabs.tt.pathfinder.UnitGrid grid = p.getWorld().getUnitGrid();
            int lx = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(p.getStartX());
            int ly = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(p.getStartY());
            int[] sea = com.oddlabs.tt.player.campaign.SagaIsland.openSea(grid, lx, ly);
            java.util.List band = new java.util.ArrayList();
            for (int k = 0; k < size; k++)
            {
                float a = (float)(k * 2.4f);
                float r = (float)(3.0f + (float)(k * 0.5f));
                band.add(new com.oddlabs.tt.model.Unit(p, (float)(p.getStartX() + (float)(r * ((float)(java.lang.StrictMath.cos(a))))), (float)(p.getStartY() + (float)(r * ((float)(java.lang.StrictMath.sin(a))))), null, race.getUnitTemplate(com.oddlabs.tt.player.campaign.SagaIsland.raidWarrior(k, threat))));
            }
            if (sea == null)
            {
                return true;
            }
            com.oddlabs.tt.model.Ship ship = null;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(band))
            {
                if (ship == null || !(((com.oddlabs.tt.model.ShipUnitContainer)ship.getUnitContainer())).canEnter(u))
                {
                    com.oddlabs.tt.model.Building s = race.getBuildingTemplate(com.oddlabs.tt.model.Race.BUILDING_SHIP).create(p, sea[0], sea[1]);
                    if (!((s is com.oddlabs.tt.model.Ship sh)))
                    {
                        continue;
                    }
                    sh.instantBuild();
                    ship = sh;
                    reinf_fleet.add(sh);
                    reinf_from.add(new float[] { sh.getPositionX(), sh.getPositionY() });
                    reinf_owner.add(p);
                    p.setLandscapeTarget(com.oddlabs.tt.model.Selectable.newArray(sh), lx, ly, com.oddlabs.tt.model.Action.MOVE, false);
                }
                if ((((com.oddlabs.tt.model.ShipUnitContainer)ship.getUnitContainer())).canEnter(u))
                {
                    (((com.oddlabs.tt.model.ShipUnitContainer)ship.getUnitContainer())).enter(u);
                }
            }
            return true;
        }
        // Java: 1407
        public static void sendHalf(com.oddlabs.tt.player.Player me, com.oddlabs.tt.player.Player p, java.util.List band)
        {
            java.util.List go = new java.util.ArrayList();
            for (int i = 0; i < band.size(); i += 2)
            {
                if (!((com.oddlabs.tt.model.Unit)band.get(i)).isDead() && !((com.oddlabs.tt.model.Unit)band.get(i)).isMounted())
                {
                    go.add(((com.oddlabs.tt.model.Unit)band.get(i)));
                }
            }
            if (!go.isEmpty())
            {
                com.oddlabs.tt.player.campaign.SagaIsland.march(me, p, go);
            }
        }
        // Java: 1417
        public void landReinforcements(com.oddlabs.tt.player.Player me)
        {
            for (int i = reinf_fleet.size() - 1; i >= 0; i--)
            {
                com.oddlabs.tt.model.Ship ship = ((com.oddlabs.tt.model.Ship)reinf_fleet.get(i));
                if (ship.isDead() || ship.getShipHR() == null)
                {
                    reinf_fleet.remove(i);
                    reinf_from.remove(i);
                    reinf_owner.remove(i);
                    continue;
                }
                float dx = (float)(ship.getPositionX() - ((float[])reinf_from.get(i))[0]);
                float dy = (float)(ship.getPositionY() - ((float[])reinf_from.get(i))[1]);
                if (ship.isMoving() || (float)((float)(dx * dx) + (float)(dy * dy)) < 100.0f || ((object)ship.getEntrance() == (object)ship))
                {
                    continue;
                }
                java.util.List crew = new java.util.ArrayList(ship.getShipHR().crew());
                foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(crew))
                {
                    if (!u.isDead())
                    {
                        ship.getShipHR().disembark(u);
                    }
                }
                com.oddlabs.tt.player.Player p = ((com.oddlabs.tt.player.Player)reinf_owner.get(i));
                reinf_fleet.remove(i);
                reinf_from.remove(i);
                reinf_owner.remove(i);
                if (me.isEnemy(p))
                {
                    com.oddlabs.tt.player.campaign.SagaIsland.sendHalf(me, p, crew);
                }
            }
        }
        // Java: 1443
        public static int[] allyBuildings(com.oddlabs.tt.player.Player me)
        {
            int gx = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(me.getStartX());
            int gy = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(me.getStartY());
            java.util.List l = new java.util.ArrayList();
            foreach (com.oddlabs.tt.player.Player p in me.getWorld().getPlayers())
            {
                if (((object)p == (object)me) || me.isEnemy(p) || p.getAI() == null || p.getPlayerInfo().getTeam() == com.oddlabs.tt.landscape.World.ZOMBIE_TEAM)
                {
                    continue;
                }
                foreach (com.oddlabs.tt.model.LandBuilding b in java.lang.JIter.of(com.oddlabs.tt.player.campaign.SagaIsland.keptBuildings(p)))
                {
                    if (((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID() == com.oddlabs.tt.model.Race.BUILDING_SHIP)
                    {
                        continue;
                    }
                    l.add(((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID());
                    l.add(b.getGridX() - gx);
                    l.add(b.getGridY() - gy);
                }
            }
            int[] a = new int[l.size()];
            for (int i = 0; i < a.Length; i++)
            {
                a[i] = ((int)((int?)l.get(i)));
            }
            return a;
        }
        // Java: 1464
        public void rebuildAllies(com.oddlabs.tt.player.Player me, com.oddlabs.tt.player.campaign.CampaignState state, int gx, int gy)
        {
            int[] b = state.getIsleBuildings(mission_f.index + com.oddlabs.tt.player.campaign.SagaIsland.ALLY_BUILDINGS);
            if (b.Length == 0)
            {
                return;
            }
            com.oddlabs.tt.player.Player ally = null;
            foreach (com.oddlabs.tt.player.Player p in me.getWorld().getPlayers())
            {
                if (((object)p != (object)me) && !me.isEnemy(p) && p.getAI() != null && p.getPlayerInfo().getTeam() != com.oddlabs.tt.landscape.World.ZOMBIE_TEAM)
                {
                    ally = p;
                    break;
                }
            }
            if (ally == null)
            {
                return;
            }
            for (int i = 0; i + 2 < b.Length; i += 3)
            {
                if (b[i] >= 0 && b[i] < com.oddlabs.tt.model.Race.NUM_BUILDINGS && b[i] != com.oddlabs.tt.model.Race.BUILDING_SHIP && b[i] != com.oddlabs.tt.model.Race.BUILDING_BRIDGE)
                {
                    ally.buildBuilding(b[i], gx + b[i + 1], gy + b[i + 2]);
                }
            }
        }
        // Java: 1486
        public void sendRaid(com.oddlabs.tt.player.Player me, com.oddlabs.tt.player.Player raider_v)
        {
            int claims = saga().claims();
            int size = (mode == com.oddlabs.tt.player.campaign.SagaIsland.DEFEND || mode == com.oddlabs.tt.player.campaign.SagaIsland.HELP ? 5 + 3 * raid_strength + 3 * raids_sent : (raids_sent == 0 ? 4 + claims / 2 : (raids_sent == 1 ? 6 + claims / 2 : 8 + 3 * (raids_sent - 1) + 2 * raid_strength + claims)));
            size = java.lang.Math.min(size, 60);
            int peons = java.lang.Math.min(12, 3 + raids_sent + raid_strength);
            com.oddlabs.tt.model.Race race = raider_v.getRace();
            java.util.List band = new java.util.ArrayList();
            for (int k = 0; k < size + peons; k++)
            {
                int type = (k >= size ? com.oddlabs.tt.model.Race.UNIT_PEON : com.oddlabs.tt.player.campaign.SagaIsland.raidWarrior(k, raids_sent + raid_strength + claims / 3));
                float a = (float)(k * 2.4f);
                float r = (float)(3.0f + (float)(k * 0.5f));
                band.add(new com.oddlabs.tt.model.Unit(raider_v, (float)(raider_v.getStartX() + (float)(r * ((float)(java.lang.StrictMath.cos(a))))), (float)(raider_v.getStartY() + (float)(r * ((float)(java.lang.StrictMath.sin(a))))), null, race.getUnitTemplate(type)));
            }
            if (raids_sent >= 2 && (raids_sent % 3 == 2 || mode == com.oddlabs.tt.player.campaign.SagaIsland.DEFEND || mode == com.oddlabs.tt.player.campaign.SagaIsland.HELP) && !raider_v.hasActiveChieftain())
            {
                com.oddlabs.tt.model.Unit chief = new com.oddlabs.tt.model.Unit(raider_v, (float)(raider_v.getStartX() + 2.0f), (float)(raider_v.getStartY() + 2.0f), null, race.getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_CHIEFTAIN), com.oddlabs.tt.player.campaign.SagaIsland.i18n("raid_chief"), false);
                chief.increaseMagicEnergy(0, 1000);
                chief.increaseMagicEnergy(1, 1000);
                raider_v.setActiveChieftain(chief);
                band.add(0, chief);
            }
            com.oddlabs.tt.pathfinder.UnitGrid grid = raider_v.getWorld().getUnitGrid();
            int lx = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(raider_v.getStartX());
            int ly = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(raider_v.getStartY());
            int[] sea = com.oddlabs.tt.player.campaign.SagaIsland.openSea(grid, lx, ly);
            if (sea != null)
            {
                com.oddlabs.tt.model.Ship ship = null;
                java.util.List ashore = new java.util.ArrayList();
                foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(band))
                {
                    if (ship == null || !(((com.oddlabs.tt.model.ShipUnitContainer)ship.getUnitContainer())).canEnter(u))
                    {
                        int k_2 = raid_fleet.size();
                        int sx = sea[0] + ((k_2 % 2 == 0 ? 1 : (-1))) * 6 * ((k_2 + 1) / 2);
                        int sy = sea[1];
                        if (!com.oddlabs.tt.player.campaign.SagaIsland.clearWater(grid, sx, sy, 4))
                        {
                            sx = sea[0];
                            sy = sea[1];
                        }
                        com.oddlabs.tt.model.Building s = race.getBuildingTemplate(com.oddlabs.tt.model.Race.BUILDING_SHIP).create(raider_v, sx, sy);
                        if (!((s is com.oddlabs.tt.model.Ship sh)))
                        {
                            ashore.add(u);
                            continue;
                        }
                        sh.instantBuild();
                        ship = sh;
                        raid_fleet.add(sh);
                        raid_from.add(new float[] { sh.getPositionX(), sh.getPositionY() });
                        raider_v.setLandscapeTarget(com.oddlabs.tt.model.Selectable.newArray(sh), lx, ly, com.oddlabs.tt.model.Action.MOVE, false);
                        if (!(((com.oddlabs.tt.model.ShipUnitContainer)ship.getUnitContainer())).canEnter(u))
                        {
                            ashore.add(u);
                            continue;
                        }
                    }
                    (((com.oddlabs.tt.model.ShipUnitContainer)ship.getUnitContainer())).enter(u);
                }
                com.oddlabs.tt.player.campaign.SagaIsland.march(me, raider_v, ashore);
                return;
            }
            com.oddlabs.tt.player.campaign.SagaIsland.march(me, raider_v, band);
            com.oddlabs.tt.player.campaign.SagaIsland.camp(raider_v, band);
        }
        // Java: 1550
        public static int raidWarrior(int k, int threat)
        {
            if (threat <= 1)
            {
                return com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK;
            }
            if (threat >= 6 && k % 7 == 3)
            {
                return com.oddlabs.tt.model.Race.UNIT_CHAMPION;
            }
            if (threat >= 4 && k % 5 == 2)
            {
                return com.oddlabs.tt.model.Race.UNIT_MELEE_HEAVY;
            }
            if (threat >= 3 && k % 4 == 1)
            {
                return com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER;
            }
            if (threat >= 3 && k % 6 == 5)
            {
                return com.oddlabs.tt.model.Race.UNIT_SLINGER;
            }
            return (k % 3 == 0 || threat >= 5 && k % 2 == 0 ? com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON : com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK);
        }
        // Java: 1569
        public void campWork(com.oddlabs.tt.player.Player target, com.oddlabs.tt.player.Player raider_v)
        {
            com.oddlabs.tt.model.LandBuilding armory = null;
            com.oddlabs.tt.model.LandBuilding quarters = null;
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(raider_v.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.LandBuilding b) && !b.isDead() && b.isComplete())
                {
                    int id = ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID();
                    if (id == com.oddlabs.tt.model.Race.BUILDING_ARMORY && armory == null)
                    {
                        armory = b;
                    }
                    else if (id == com.oddlabs.tt.model.Race.BUILDING_QUARTERS && quarters == null)
                    {
                        quarters = b;
                    }
                }
            }
            if (armory == null && quarters == null)
            {
                return;
            }
            com.oddlabs.tt.model.Race race = raider_v.getRace();
            int threat = raids_sent + raid_strength + saga().claims() / 3;
            java.util.List @out = new java.util.ArrayList();
            if (armory != null)
            {
                int n = java.lang.Math.min(10, 2 + raids_sent / 2 + raid_strength / 2 + saga().claims() / 4);
                for (int k = 0; k < n; k++)
                {
                    float a = (float)(k * 2.1f);
                    @out.add(new com.oddlabs.tt.model.Unit(raider_v, (float)(armory.getPositionX() + (float)(4.0f * ((float)(java.lang.StrictMath.cos(a))))), (float)(armory.getPositionY() + (float)(4.0f * ((float)(java.lang.StrictMath.sin(a))))), null, race.getUnitTemplate(com.oddlabs.tt.player.campaign.SagaIsland.raidWarrior(k + camp_rounds, threat))));
                }
            }
            int peons = 0;
            com.oddlabs.tt.model.Unit any_peon = null;
            foreach (com.oddlabs.tt.model.Selectable s_2 in java.lang.JIter.of(raider_v.getUnits().getSet()))
            {
                if ((s_2 is com.oddlabs.tt.model.Unit u) && !u.isDead())
                {
                    if (((object)((com.oddlabs.tt.model.UnitTemplate)u.getTemplate()) == (object)race.getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON)))
                    {
                        peons++;
                        any_peon = u;
                    }
                    else if (u.isWarrior() && !u.isMounted() && (u.getCurrentController() is com.oddlabs.tt.model.behaviour.IdleController))
                    {
                        @out.add(u);
                    }
                }
            }
            if (quarters != null && peons < 6)
            {
                com.oddlabs.tt.model.Unit p = new com.oddlabs.tt.model.Unit(raider_v, (float)(quarters.getPositionX() + 3.0f), (float)(quarters.getPositionY() + 3.0f), null, race.getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
                java.util.List crew = new java.util.ArrayList();
                crew.add(p);
                if (camp_rounds % 2 == 1)
                {
                    com.oddlabs.tt.player.campaign.SagaIsland.tower(raider_v, p);
                }
                else
                {
                    com.oddlabs.tt.player.campaign.SagaIsland.camp(raider_v, crew);
                }
            }
            else if (any_peon != null && camp_rounds % 3 == 2)
            {
                com.oddlabs.tt.player.campaign.SagaIsland.tower(raider_v, any_peon);
            }
            camp_rounds++;
            com.oddlabs.tt.player.campaign.SagaIsland.march(target, raider_v, @out);
        }
        // Java: 1619
        public static void tower(com.oddlabs.tt.player.Player raider_v, com.oddlabs.tt.model.Unit peon)
        {
            com.oddlabs.tt.player.BuildingSiteScanFilter filter = new com.oddlabs.tt.player.BuildingSiteScanFilter(raider_v.getWorld().getUnitGrid(), raider_v.getRace().getBuildingTemplate(com.oddlabs.tt.model.Race.BUILDING_TOWER), 20, true);
            raider_v.getWorld().getUnitGrid().scan(filter, peon.getGridX(), peon.getGridY());
            java.util.List sites = filter.getResult();
            if (!sites.isEmpty())
            {
                raider_v.placeBuilding(com.oddlabs.tt.model.Selectable.newArray(peon), com.oddlabs.tt.model.Race.BUILDING_TOWER, ((com.oddlabs.tt.landscape.LandscapeTarget)sites.getFirst()).getGridX(), ((com.oddlabs.tt.landscape.LandscapeTarget)sites.getFirst()).getGridY(), false);
            }
        }
        // Java: 1630
        public static void march(com.oddlabs.tt.player.Player me, com.oddlabs.tt.player.Player raider_v, java.util.List units)
        {
            java.util.List warriors_v = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(units))
            {
                if (!u.isDead() && ((object)((com.oddlabs.tt.model.UnitTemplate)u.getTemplate()) != (object)raider_v.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON)))
                {
                    warriors_v.add(u);
                }
            }
            if (warriors_v.isEmpty())
            {
                return;
            }
            com.oddlabs.tt.model.Unit lead = ((com.oddlabs.tt.model.Unit)((com.oddlabs.tt.model.Selectable)warriors_v.getFirst()));
            com.oddlabs.tt.model.LandBuilding target = null;
            float best = java.lang.Float.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(me.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.LandBuilding b) && !b.isDead())
                {
                    float dx = (float)(b.getPositionX() - lead.getPositionX());
                    float dy = (float)(b.getPositionY() - lead.getPositionY());
                    if ((float)((float)(dx * dx) + (float)(dy * dy)) < best)
                    {
                        best = (float)((float)(dx * dx) + (float)(dy * dy));
                        target = b;
                    }
                }
            }
            int tx = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate((target != null ? target.getPositionX() : me.getStartX()));
            int ty = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate((target != null ? target.getPositionY() : me.getStartY()));
            raider_v.setLandscapeTarget(((com.oddlabs.tt.model.Selectable[])warriors_v.toArray(new com.oddlabs.tt.model.Selectable[0])), tx, ty, com.oddlabs.tt.model.Action.ATTACK, true);
        }
        // Java: 1654
        public static void camp(com.oddlabs.tt.player.Player raider_v, java.util.List units)
        {
            java.util.List peons = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(units))
            {
                if (!u.isDead() && ((object)((com.oddlabs.tt.model.UnitTemplate)u.getTemplate()) == (object)raider_v.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON)))
                {
                    peons.add(u);
                }
            }
            if (peons.isEmpty())
            {
                return;
            }
            com.oddlabs.tt.model.Unit p = ((com.oddlabs.tt.model.Unit)((com.oddlabs.tt.model.Selectable)peons.getFirst()));
            com.oddlabs.tt.model.Selectable[] sel = ((com.oddlabs.tt.model.Selectable[])peons.toArray(new com.oddlabs.tt.model.Selectable[0]));
            bool first = true;
            foreach (int type in new int[] { com.oddlabs.tt.model.Race.BUILDING_QUARTERS, com.oddlabs.tt.model.Race.BUILDING_ARMORY })
            {
                com.oddlabs.tt.player.BuildingSiteScanFilter filter = new com.oddlabs.tt.player.BuildingSiteScanFilter(raider_v.getWorld().getUnitGrid(), raider_v.getRace().getBuildingTemplate(type), 30, true);
                raider_v.getWorld().getUnitGrid().scan(filter, p.getGridX(), p.getGridY());
                java.util.List sites = filter.getResult();
                if (sites.isEmpty())
                {
                    continue;
                }
                raider_v.placeBuilding(sel, type, ((com.oddlabs.tt.landscape.LandscapeTarget)sites.getFirst()).getGridX(), ((com.oddlabs.tt.landscape.LandscapeTarget)sites.getFirst()).getGridY(), !first);
                first = false;
            }
        }
        // Java: 1677
        public void landRaiders(com.oddlabs.tt.player.Player me, com.oddlabs.tt.player.Player raider_v)
        {
            if (raider_v == null)
            {
                return;
            }
            for (int i = raid_fleet.size() - 1; i >= 0; i--)
            {
                com.oddlabs.tt.model.Ship ship = ((com.oddlabs.tt.model.Ship)raid_fleet.get(i));
                if (ship.isDead() || ship.getShipHR() == null)
                {
                    raid_fleet.remove(i);
                    raid_from.remove(i);
                    continue;
                }
                float dx = (float)(ship.getPositionX() - ((float[])raid_from.get(i))[0]);
                float dy = (float)(ship.getPositionY() - ((float[])raid_from.get(i))[1]);
                if (ship.isMoving() || (float)((float)(dx * dx) + (float)(dy * dy)) < 100.0f || ((object)ship.getEntrance() == (object)ship))
                {
                    continue;
                }
                java.util.List crew = new java.util.ArrayList(ship.getShipHR().crew());
                foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(crew))
                {
                    if (!u.isDead())
                    {
                        ship.getShipHR().disembark(u);
                    }
                }
                raid_fleet.remove(i);
                raid_from.remove(i);
                com.oddlabs.tt.player.campaign.SagaIsland.march(me, raider_v, crew);
                com.oddlabs.tt.player.campaign.SagaIsland.camp(raider_v, crew);
                if (mode == com.oddlabs.tt.player.campaign.SagaIsland.HOLD)
                {
                    addModalForm(new com.oddlabs.tt.form.InGameCampaignDialogForm(getViewer(), getHeader(), com.oddlabs.tt.player.campaign.SagaIsland.i18n("raid_landed"), face(com.oddlabs.tt.player.campaign.SagaCampaign.FACE_WARRIOR), com.oddlabs.tt.gui.Origin.AT_START));
                }
            }
        }
        // Java: 1708
        public void guard(com.oddlabs.tt.player.Player me, com.oddlabs.tt.player.Player raider_v)
        {
            if (raider_v == null || elapsed < next_guard)
            {
                return;
            }
            next_guard = (float)(elapsed + 3.0f);
            java.util.List idle = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(me.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Unit u) && !u.isDead() && u.isWarrior() && !u.isMounted() && (u.getCurrentController() is com.oddlabs.tt.model.behaviour.IdleController))
                {
                    idle.add(u);
                }
            }
            if (idle.isEmpty())
            {
                return;
            }
            foreach (com.oddlabs.tt.model.Selectable s_2 in java.lang.JIter.of(me.getUnits().getSet()))
            {
                if ((s_2 is com.oddlabs.tt.model.LandBuilding b) && !b.isDead() && b.isComplete() && !idle.isEmpty() && (b.getUnitContainer() is com.oddlabs.tt.model.MountUnitContainer m) && m.getUnit() == null)
                {
                    me.setTarget(com.oddlabs.tt.model.Selectable.newArray(((com.oddlabs.tt.model.Selectable)idle.removeLast())), b, com.oddlabs.tt.model.Action.DEFAULT, false);
                }
            }
            com.oddlabs.tt.model.Selectable foe = null;
            float best = (float)(45.0f * 45.0f);
            foreach (com.oddlabs.tt.model.Selectable s_3 in java.lang.JIter.of(raider_v.getUnits().getSet()))
            {
                if ((s_3 is com.oddlabs.tt.model.Unit u_2) && !u_2.isDead())
                {
                    foreach (com.oddlabs.tt.model.Selectable o in java.lang.JIter.of(me.getUnits().getSet()))
                    {
                        if ((o is com.oddlabs.tt.model.LandBuilding b_2) && !b_2.isDead())
                        {
                            float dx = (float)(u_2.getPositionX() - b_2.getPositionX());
                            float dy = (float)(u_2.getPositionY() - b_2.getPositionY());
                            if ((float)((float)(dx * dx) + (float)(dy * dy)) < best)
                            {
                                best = (float)((float)(dx * dx) + (float)(dy * dy));
                                foe = u_2;
                            }
                        }
                    }
                }
            }
            if (foe != null && !idle.isEmpty())
            {
                me.setLandscapeTarget(((com.oddlabs.tt.model.Selectable[])idle.toArray(new com.oddlabs.tt.model.Selectable[0])), foe.getGridX(), foe.getGridY(), com.oddlabs.tt.model.Action.ATTACK, true);
            }
        }
        // Java: 1742
        public void returnHome(com.oddlabs.tt.player.Player me)
        {
            com.oddlabs.tt.player.campaign.CampaignState state = getCampaign().getState();
            int[] home_units = state.getSagaUnits();
            for (int u = 0; u < home_units.Length && u < expedition.Length; u++)
            {
                home_units[u] = java.lang.Math.max(0, home_units[u] - expedition[u]);
            }
            int[] back = com.oddlabs.tt.player.campaign.SagaIsland.census(me);
            int room = saga().homeCap() - com.oddlabs.tt.player.campaign.SagaIsland.sum(home_units);
            for (int u_2 = 0; u_2 < back.Length && room > 0; u_2++)
            {
                int k = java.lang.Math.min(room, java.lang.Math.min(back[u_2], (u_2 < expedition.Length ? expedition[u_2] + 50 : back[u_2])));
                home_units[u_2] += k;
                room -= k;
            }
            int[] home_stock = state.getSagaStock();
            for (int i = 0; i < home_stock.Length; i++)
            {
                home_stock[i] = java.lang.Math.max(0, home_stock[i] - expedition[com.oddlabs.tt.player.campaign.CampaignState.SAGA_UNITS + i]);
            }
            state.clearSagaExpedition();
        }
        // Java: 1764
        public void islandLost(com.oddlabs.tt.player.Player me)
        {
            settled = true;
            finished = true;
            com.oddlabs.tt.player.campaign.CampaignState state = getCampaign().getState();
            bool ship = false;
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(me.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Ship sh) && !sh.isDead())
                {
                    ship = true;
                }
            }
            int[] survivors = (ship ? com.oddlabs.tt.player.campaign.SagaIsland.capped(com.oddlabs.tt.player.campaign.SagaIsland.census(me), 16) : new int[com.oddlabs.tt.player.campaign.CampaignState.SAGA_UNITS]);
            int[] home_units = ((int[])base_home_units.clone());
            for (int u = 0; u < home_units.Length && u < expedition.Length; u++)
            {
                home_units[u] = java.lang.Math.max(0, home_units[u] - expedition[u]);
            }
            state.setIsleUnits(com.oddlabs.tt.player.campaign.SagaCampaign.HOME, home_units);
            saga().loseIsland(mission_f.index, 0.0f);
            int[] home_now = state.getSagaUnits();
            int room = saga().homeCap() - com.oddlabs.tt.player.campaign.SagaIsland.sum(home_now);
            int fled = 0;
            for (int u_2 = 0; u_2 < survivors.Length && room > 0; u_2++)
            {
                int k = java.lang.Math.min(room, survivors[u_2]);
                home_now[u_2] += k;
                room -= k;
                fled += k;
            }
            state.clearSagaExpedition();
            state.setSagaRaid(null);
            getCampaign().defeated(getViewer(), com.oddlabs.tt.player.campaign.SagaIsland.i18n((ship && fled > 0 ? "isle_lost_fled" : "isle_lost"), saga().islandName(mission_f.index), fled));
        }
        // Java: 1793
        public com.oddlabs.tt.gui.IconQuad face(int which)
        {
            return getCampaign().getIcons().getFaces()[which];
        }
        // Java: 1797
        public class Watch : java.lang.Object, com.oddlabs.tt.animation.Animated
        {
            public com.oddlabs.tt.player.campaign.SagaIsland this_0;
            public com.oddlabs.tt.gui.ObjectiveBanner status;
            public float next;
            public string shown;
            // Java: 1806
            public void animate(float t)
            {
                this_0.elapsed += t;
                if (this_0.finished || this_0.elapsed < next)
                {
                    return;
                }
                next = (float)(this_0.elapsed + 0.5f);
                com.oddlabs.tt.landscape.World world = this_0.getViewer().getWorld();
                com.oddlabs.tt.player.Player me = this_0.getViewer().getLocalPlayer();
                this_0.land(me);
                if (this_0.shipment != null && this_0.elapsed >= this_0.shipment_at && me.getArmory() != null)
                {
                    com.oddlabs.tt.player.campaign.SagaIsland.deposit(me, this_0.shipment);
                    this_0.shipment = null;
                    this_0.addModalForm(new com.oddlabs.tt.form.InGameCampaignDialogForm(this_0.getViewer(), this_0.getHeader(), com.oddlabs.tt.player.campaign.SagaIsland.i18n("shipment_arrived"), this_0.face(com.oddlabs.tt.player.campaign.SagaCampaign.FACE_ADVISOR), com.oddlabs.tt.gui.Origin.AT_START));
                }
                if (this_0.pending != null && me.getArmory() != null && me.getArmory().isComplete())
                {
                    com.oddlabs.tt.player.campaign.SagaIsland.deposit(me, this_0.pending);
                    this_0.pending = null;
                    if (!this_0.supplies_told)
                    {
                        this_0.supplies_told = true;
                        bool any = false;
                        for (int i = 0; i < com.oddlabs.tt.player.campaign.CampaignState.SAGA_STOCKS; i++)
                        {
                            any |= this_0.expedition.Length > com.oddlabs.tt.player.campaign.CampaignState.SAGA_UNITS + i && this_0.expedition[com.oddlabs.tt.player.campaign.CampaignState.SAGA_UNITS + i] > 0;
                        }
                        if (any || this_0.mission_f.stock > 0)
                        {
                            this_0.addModalForm(new com.oddlabs.tt.form.InGameCampaignDialogForm(this_0.getViewer(), this_0.getHeader(), com.oddlabs.tt.player.campaign.SagaIsland.i18n("supplies_landed"), this_0.face(com.oddlabs.tt.player.campaign.SagaCampaign.FACE_ADVISOR), com.oddlabs.tt.gui.Origin.AT_START));
                        }
                    }
                }
                if (!this_0.settled && this_0.mode != com.oddlabs.tt.player.campaign.SagaIsland.VISIT)
                {
                    this_0.landReinforcements(me);
                    this_0.allyReinforce(me);
                }
                if (this_0.home || this_0.mode == com.oddlabs.tt.player.campaign.SagaIsland.HELP || this_0.mode == com.oddlabs.tt.player.campaign.SagaIsland.VISIT)
                {
                    holding(world, me);
                    return;
                }
                if (this_0.mode == com.oddlabs.tt.player.campaign.SagaIsland.MISSION && !this_0.settled)
                {
                    this_0.reinforce(me);
                }
                for (int i_2 = 0; i_2 < this_0.done.Length; i_2++)
                {
                    if (!this_0.done[i_2] && this_0.taskDone(me, ((com.oddlabs.tt.player.campaign.SagaMission.Task)this_0.mission_f.tasks.get(i_2))))
                    {
                        this_0.done[i_2] = true;
                        bool all = this_0.allTasksDone();
                        if (all && ((object)this_0.mission_f.goal_f != (object)com.oddlabs.tt.player.campaign.SagaMission.Goal.TASKS))
                        {
                            this_0.addModalForm(new com.oddlabs.tt.form.InGameCampaignDialogForm(this_0.getViewer(), this_0.getHeader(), com.oddlabs.tt.player.campaign.SagaIsland.i18n(this_0.key("ready")), this_0.face(this_0.mission_f.speaker), com.oddlabs.tt.gui.Origin.AT_START));
                        }
                    }
                }
                int verdict_v = this_0.verdict(world, me);
                string text = this_0.statusText(world, me);
                if (!java.lang.JO.equals(text, shown))
                {
                    shown = text;
                    status.setText(text);
                }
                if (verdict_v > 0 && this_0.allTasksDone())
                {
                    this_0.finished = true;
                    this_0.addModalForm(new com.oddlabs.tt.form.InGameCampaignDialogForm(this_0.getViewer(), this_0.getHeader(), com.oddlabs.tt.player.campaign.SagaIsland.i18n(this_0.key("outro")), this_0.face(winFace()), com.oddlabs.tt.gui.Origin.AT_END, new java.lang.Runnable_L(() => { this.prize(); })));
                }
                else if (verdict_v < 0)
                {
                    this_0.finished = true;
                    this_0.getCampaign().defeated(this_0.getViewer(), com.oddlabs.tt.player.campaign.SagaIsland.i18n("lost_" + java.lang.JStr.valueOf(this_0.mission_f.goal_f.name().toLowerCase())));
                }
            }
            // Java: 1870
            public void holding(com.oddlabs.tt.landscape.World world, com.oddlabs.tt.player.Player me)
            {
                if (this_0.settled)
                {
                    return;
                }
                com.oddlabs.tt.player.Player raider_v = com.oddlabs.tt.player.campaign.SagaIsland.raider(world, me);
                com.oddlabs.tt.player.Player target = (this_0.defended != null ? this_0.defended : me);
                this_0.landRaiders(target, raider_v);
                if (this_0.raids && raider_v != null && this_0.raids_sent > 0 && this_0.elapsed >= this_0.next_camp_work)
                {
                    this_0.next_camp_work = (float)(this_0.elapsed + java.lang.Math.max(35.0f, (float)(75.0f - (float)(4.0f * this_0.saga().claims()))));
                    this_0.campWork(target, raider_v);
                }
                if (this_0.defense == com.oddlabs.tt.player.campaign.SagaCampaign.DEFEND_AUTO || this_0.defense == com.oddlabs.tt.player.campaign.SagaCampaign.DEFEND_ALLY)
                {
                    this_0.guard(me, raider_v);
                }
                if (this_0.mission_f.index == com.oddlabs.tt.player.campaign.SagaCampaign.HOME && this_0.mode == com.oddlabs.tt.player.campaign.SagaIsland.HOLD)
                {
                    java.util.List ready = this_0.establish(me);
                    if (!ready.isEmpty() && this_0.elapsed >= this_0.next_establish_note)
                    {
                        this_0.next_establish_note = (float)(this_0.elapsed + 20.0f);
                        this_0.addModalForm(new com.oddlabs.tt.form.InGameCampaignDialogForm(this_0.getViewer(), this_0.getHeader(), com.oddlabs.tt.player.campaign.SagaIsland.i18n("established", java.lang.JString.join(", ", ready), com.oddlabs.tt.player.campaign.SagaIsland.unestablished(this_0.getCampaign().getState())), this_0.face(com.oddlabs.tt.player.campaign.SagaCampaign.FACE_ADVISOR), com.oddlabs.tt.gui.Origin.AT_START));
                    }
                }
                int alive = (raider_v == null ? 0 : raider_v.getUnitCountContainer().getNumSupplies());
                int camp_v = (raider_v == null ? 0 : com.oddlabs.tt.player.campaign.SagaIsland.buildingsOf(raider_v));
                bool beaten = raider_v == null || alive == 0 && camp_v == 0 && this_0.raid_fleet.isEmpty() && !raider_v.hasActiveChieftain();
                int waves = (this_0.mode == com.oddlabs.tt.player.campaign.SagaIsland.DEFEND || this_0.mode == com.oddlabs.tt.player.campaign.SagaIsland.HELP ? 3 : java.lang.Integer.MAX_VALUE);
                if (this_0.raids && raider_v != null && this_0.elapsed >= this_0.next_raid && this_0.raids_sent < waves)
                {
                    if (this_0.raids_sent == 0 && this_0.mode == com.oddlabs.tt.player.campaign.SagaIsland.HOLD)
                    {
                        this_0.addModalForm(new com.oddlabs.tt.form.InGameCampaignDialogForm(this_0.getViewer(), this_0.getHeader(), com.oddlabs.tt.player.campaign.SagaIsland.i18n("raid_coming"), this_0.face(com.oddlabs.tt.player.campaign.SagaCampaign.FACE_WARRIOR), com.oddlabs.tt.gui.Origin.AT_START));
                    }
                    this_0.sendRaid(target, raider_v);
                    this_0.raids_sent++;
                    this_0.next_raid = (float)(this_0.elapsed + ((this_0.mode == com.oddlabs.tt.player.campaign.SagaIsland.DEFEND || this_0.mode == com.oddlabs.tt.player.campaign.SagaIsland.HELP ? 70.0f : (float)(java.lang.Math.max(90.0f, (float)(240.0f - (float)(15.0f * this_0.saga().claims()))) + world.getRandom().nextInt(60)))));
                }
                string text = (this_0.mode == com.oddlabs.tt.player.campaign.SagaIsland.DEFEND || this_0.mode == com.oddlabs.tt.player.campaign.SagaIsland.HELP ? java.lang.JStr.valueOf(com.oddlabs.tt.player.campaign.SagaIsland.i18n("defend_status", java.lang.Math.max(0, waves - this_0.raids_sent), alive)) + java.lang.JStr.valueOf(((camp_v > 0 ? " " + java.lang.JStr.valueOf(com.oddlabs.tt.player.campaign.SagaIsland.i18n("raid_camp", camp_v)) : ""))) : java.lang.JStr.valueOf(com.oddlabs.tt.player.campaign.SagaIsland.i18n("home_status", com.oddlabs.tt.player.campaign.SagaIsland.sum(com.oddlabs.tt.player.campaign.SagaIsland.census(me)), this_0.population())) + java.lang.JStr.valueOf(((this_0.raids && this_0.raids_sent > 0 && alive > 0 ? " " + java.lang.JStr.valueOf(com.oddlabs.tt.player.campaign.SagaIsland.i18n("raiders_here", alive)) : ""))) + java.lang.JStr.valueOf(((this_0.raids && this_0.raids_sent > 0 && camp_v > 0 ? " " + java.lang.JStr.valueOf(com.oddlabs.tt.player.campaign.SagaIsland.i18n("raid_camp", camp_v)) : ""))));
                if (!java.lang.JO.equals(text, shown))
                {
                    shown = text;
                    status.setText(text);
                }
                if (this_0.mode == com.oddlabs.tt.player.campaign.SagaIsland.HELP)
                {
                    if (this_0.raids_sent > 0 && !com.oddlabs.tt.player.campaign.SagaIsland.hasBuildings(target))
                    {
                        this_0.finished = true;
                        this_0.getCampaign().defeated(this_0.getViewer(), com.oddlabs.tt.player.campaign.SagaIsland.i18n("help_lost", this_0.saga().islandName(this_0.mission_f.index)));
                        return;
                    }
                    if (this_0.raids_sent >= waves && beaten)
                    {
                        this_0.finished = true;
                        this_0.settled = true;
                        this_0.returnHome(me);
                        com.oddlabs.tt.player.campaign.CampaignState st = this_0.getCampaign().getState();
                        st.setSagaRaid(null);
                        st.addSagaFriendship(2);
                        st.addSagaVoyage();
                        this_0.getCampaign().leave(this_0.getViewer(), com.oddlabs.tt.player.campaign.SagaIsland.i18n("help_won", this_0.saga().islandName(this_0.mission_f.index)));
                    }
                    return;
                }
                if (this_0.raids_sent > 0 && !com.oddlabs.tt.player.campaign.SagaIsland.hasBuildings(me))
                {
                    this_0.islandLost(me);
                    return;
                }
                if (this_0.mode == com.oddlabs.tt.player.campaign.SagaIsland.DEFEND && this_0.raids_sent >= waves && beaten)
                {
                    this_0.settled = false;
                    this_0.keepHolding();
                    this_0.settled = true;
                    this_0.finished = true;
                    this_0.getCampaign().getState().setSagaRaid(null);
                    this_0.getCampaign().getState().addSagaVoyage();
                    this_0.getCampaign().leave(this_0.getViewer(), com.oddlabs.tt.player.campaign.SagaIsland.i18n("raid_repelled", this_0.saga().islandName(this_0.mission_f.index)));
                    return;
                }
                if ((this_0.mode == com.oddlabs.tt.player.campaign.SagaIsland.HOLD || this_0.mode == com.oddlabs.tt.player.campaign.SagaIsland.VISIT) && this_0.elapsed >= this_0.next_autosave)
                {
                    this_0.next_autosave = (float)(this_0.elapsed + 120.0f);
                    this_0.keepHolding();
                    this_0.getCampaign().save(this_0.getViewer().getGUIRoot());
                }
            }
            // Java: 1953
            public int winFace()
            {
                return (this_0.mission_f.index == com.oddlabs.tt.player.campaign.SagaCampaign.FINAL_ISLAND ? com.oddlabs.tt.player.campaign.SagaCampaign.FACE_ORACLE : com.oddlabs.tt.player.campaign.SagaCampaign.FACE_CHIEF);
            }
            // Java: 1957
            public void prize()
            {
                com.oddlabs.tt.player.campaign.CampaignState state = this_0.getCampaign().getState();
                com.oddlabs.tt.player.Player me = this_0.getViewer().getLocalPlayer();
                int island = this_0.mission_f.index;
                bool founding = island == com.oddlabs.tt.player.campaign.SagaCampaign.HOME;
                this_0.settled = true;
                foreach (int k in this_0.mission_f.unlocks_f)
                {
                    state.sagaUnlock(k);
                }
                this_0.keepResearch(me, state);
                int[] home_units = state.getSagaUnits();
                int[] home_stock = state.getSagaStock();
                if (founding && state.getSagaVoyages() == 0)
                {
                    java.util.Arrays.fill(home_units, 0);
                }
                for (int u = 0; u < home_units.Length && u < this_0.expedition.Length; u++)
                {
                    home_units[u] = java.lang.Math.max(0, home_units[u] - this_0.expedition[u]);
                }
                for (int i = 0; i < home_stock.Length; i++)
                {
                    home_stock[i] = java.lang.Math.max(0, home_stock[i] - this_0.expedition[com.oddlabs.tt.player.campaign.CampaignState.SAGA_UNITS + i]);
                }
                state.setIslandState(island, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_COMPLETED);
                int[] people_v = com.oddlabs.tt.player.campaign.SagaIsland.census(me);
                int[] stock = com.oddlabs.tt.player.campaign.SagaIsland.armoryStock(me);
                for (int i_2 = 0; i_2 < stock.Length; i_2++)
                {
                    stock[i_2] += ((this_0.pending != null ? this_0.pending[i_2] : 0));
                }
                state.setIsleWeapons(island, com.oddlabs.tt.player.campaign.SagaIsland.armoryWeapons(me));
                if (founding)
                {
                    state.setSagaBuildings(com.oddlabs.tt.player.campaign.SagaIsland.buildings(me, com.oddlabs.tt.player.campaign.SagaIsland.keptBuildings(me)));
                    for (int i_3 = 0; i_3 < home_stock.Length; i_3++)
                    {
                        home_stock[i_3] += stock[i_3];
                    }
                }
                else if (this_0.mission_f.befriends_f)
                {
                    state.setAllyIsle(island, true);
                    for (int i_4 = 0; i_4 < stock.Length; i_4++)
                    {
                        home_stock[i_4] += stock[i_4] / 2;
                    }
                }
                else
                {
                    state.setIsleCap(island, 15 + new java.util.Random(island * 131L + state.getSagaVoyages()).nextInt(16));
                    int[] garrison = com.oddlabs.tt.player.campaign.SagaIsland.capped(people_v, java.lang.Math.min(this_0.saga().isleCap(island) / 2, com.oddlabs.tt.player.campaign.SagaIsland.sum(people_v) / 3));
                    for (int u_2 = 0; u_2 < people_v.Length; u_2++)
                    {
                        people_v[u_2] -= garrison[u_2];
                    }
                    java.util.List kept_buildings = com.oddlabs.tt.player.campaign.SagaIsland.keptBuildings(me);
                    state.setIsleUnits(island, garrison);
                    state.setIslePeople(island, com.oddlabs.tt.player.campaign.SagaIsland.places(me, kept_buildings, garrison));
                    state.setIsleBuildings(island, com.oddlabs.tt.player.campaign.SagaIsland.buildings(me, kept_buildings));
                    state.setIsleBuildings(island + com.oddlabs.tt.player.campaign.SagaIsland.ALLY_BUILDINGS, com.oddlabs.tt.player.campaign.SagaIsland.allyBuildings(me));
                    int[] kept = new int[stock.Length];
                    for (int i_5 = 0; i_5 < stock.Length; i_5++)
                    {
                        kept[i_5] = stock[i_5] - stock[i_5] / 2;
                        home_stock[i_5] += stock[i_5] / 2;
                    }
                    state.setIsleStock(island, kept);
                }
                int room = this_0.saga().homeCap() - com.oddlabs.tt.player.campaign.SagaIsland.sum(home_units);
                for (int u_3 = 0; u_3 < people_v.Length && room > 0; u_3++)
                {
                    int k_2 = java.lang.Math.min(room, people_v[u_3]);
                    home_units[u_3] += k_2;
                    room -= k_2;
                }
                for (int i_6 = 0; i_6 < home_stock.Length && i_6 < this_0.mission_f.reward_f.Length; i_6++)
                {
                    home_stock[i_6] += this_0.mission_f.reward_f[i_6];
                }
                this_0.keepDepletion(state);
                if (this_0.mission_f.befriends_f)
                {
                    state.addSagaAlly();
                }
                state.clearSagaExpedition();
                foreach (int i_7 in this_0.mission_f.opens_f)
                {
                    state.setIslandState(i_7, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE);
                }
                foreach (int i_8 in this_0.mission_f.half_opens)
                {
                    if (state.getIslandState(i_8) != com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE)
                    {
                        state.setIslandState(i_8, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_SEMI_AVAILABLE);
                    }
                }
                foreach (int i_9 in this_0.mission_f.reveals_f)
                {
                    if (state.getIslandState(i_9) == com.oddlabs.tt.player.campaign.CampaignState.ISLAND_HIDDEN)
                    {
                        state.setIslandState(i_9, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE);
                    }
                }
                this_0.saga().afterVoyage(island);
                this_0.getCampaign().victory(this_0.getViewer());
            }
            private void init_Watch()
            {
                this.shown = "";
            }
            public Watch(com.oddlabs.tt.player.campaign.SagaIsland this_0_, com.oddlabs.tt.gui.ObjectiveBanner status) : base()
            {
                this.this_0 = this_0_;
                init_Watch();
                this.status = status;
            }
            // mixin of default method Animated.updateChecksum
            public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
            {
            }
        }
        // Java: 2031
        public bool allTasksDone()
        {
            foreach (bool d in done)
            {
                if (!d)
                {
                    return false;
                }
            }
            return true;
        }
        // Java: 2039
        public int verdict(com.oddlabs.tt.landscape.World world, com.oddlabs.tt.player.Player me)
        {
            com.oddlabs.tt.gamemode.GameModeRules rules = com.oddlabs.tt.gamemode.GameModeRegistry.get(world.getGameMode());
            switch ((mission_f.goal_f).ordinal())
            {
                case 0 /*CONQUER*/:
                {
                    return (com.oddlabs.tt.player.campaign.SagaIsland.conquered(world, me) ? 1 : 0);
                }
                case 1 /*HOLD*/:
                {
                    int winner = ((rules is com.oddlabs.tt.gamemode.koth.KingOfTheHillModeRules k) ? k.getWinningTeam() : ((rules is com.oddlabs.tt.gamemode.koth.KingOfTheIslandModeRules k_2) ? k_2.getWinningTeam() : com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL));
                    if (winner == com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL)
                    {
                        return (com.oddlabs.tt.player.campaign.SagaIsland.conquered(world, me) ? 1 : 0);
                    }
                    return (winner == me.getPlayerInfo().getTeam() ? 1 : (-1));
                }
                case 2 /*SURVIVE_WAVES*/:
                {
                    int wave = ((rules is com.oddlabs.tt.gamemode.survival.SurvivalModeRules s) ? s.getWaveNumber() : 0);
                    return (wave >= mission_f.goal_amount ? 1 : 0);
                }
                case 5 /*FLAGS*/:
                {
                    int winner_2 = ((rules is com.oddlabs.tt.gamemode.ctf.CaptureTheFlagModeRules c) ? c.getWinningTeam() : com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL);
                    if (winner_2 == com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL)
                    {
                        return (com.oddlabs.tt.player.campaign.SagaIsland.conquered(world, me) ? 1 : 0);
                    }
                    return (winner_2 == me.getPlayerInfo().getTeam() ? 1 : (-1));
                }
                case 3 /*SURVIVE_TIME*/:
                {
                    return (elapsed >= mission_f.goal_amount ? 1 : 0);
                }
                case 4 /*REGICIDE*/:
                {
                    if (!me.hasActiveChieftain())
                    {
                        return (-1);
                    }
                    foreach (com.oddlabs.tt.player.Player p in world.getPlayers())
                    {
                        if (com.oddlabs.tt.player.campaign.SagaIsland.rival(world, me, p) && p.hasActiveChieftain())
                        {
                            return 0;
                        }
                    }
                    return 1;
                }
                default:
                {
                    return 1;
                }
            }
        }
        // Java: 2080
        public static bool conquered(com.oddlabs.tt.landscape.World world, com.oddlabs.tt.player.Player me)
        {
            foreach (com.oddlabs.tt.player.Player p in world.getPlayers())
            {
                if (com.oddlabs.tt.player.campaign.SagaIsland.rival(world, me, p) && com.oddlabs.tt.player.campaign.SagaIsland.standing(p))
                {
                    return false;
                }
            }
            return true;
        }
        // Java: 2088
        public static bool rival(com.oddlabs.tt.landscape.World world, com.oddlabs.tt.player.Player me, com.oddlabs.tt.player.Player p)
        {
            return me.isEnemy(p) && p.getPlayerInfo().getTeam() != com.oddlabs.tt.landscape.World.ZOMBIE_TEAM;
        }
        // Java: 2092
        public bool taskDone(com.oddlabs.tt.player.Player me, com.oddlabs.tt.player.campaign.SagaMission.Task task)
        {
            com.oddlabs.tt.model.Race race = me.getRace();
            switch ((task.kind()).ordinal())
            {
                case 0 /*BUILD*/:
                {
                    foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(me.getUnits().getSet()))
                    {
                        if ((s is com.oddlabs.tt.model.LandBuilding b) && !b.isDead() && b.isComplete() && ((object)((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()) == (object)race.getBuildingTemplate(task.what())))
                        {
                            return true;
                        }
                    }
                    return false;
                }
                case 1 /*RESEARCH*/:
                {
                    return me.getUpgradeLevel(task.what()) >= task.count();
                }
                case 2 /*TRAIN*/:
                {
                    int n = 0;
                    com.oddlabs.tt.model.UnitTemplate t = race.getUnitTemplate(task.what());
                    foreach (com.oddlabs.tt.model.Selectable s_2 in java.lang.JIter.of(me.getUnits().getSet()))
                    {
                        if ((s_2 is com.oddlabs.tt.model.Unit u) && !u.isDead() && ((object)((com.oddlabs.tt.model.UnitTemplate)u.getTemplate()) == (object)t))
                        {
                            n++;
                        }
                    }
                    return n >= task.count();
                }
                case 3 /*GOD_KILLS*/:
                {
                    foreach (com.oddlabs.tt.model.Selectable s_3 in java.lang.JIter.of(me.getUnits().getSet()))
                    {
                        if ((s_3 is com.oddlabs.tt.model.Unit u_2) && !u_2.isDead() && u_2.getGodKind() != com.oddlabs.tt.model.GodPowers.NONE && u_2.getGodKills() >= task.count())
                        {
                            return true;
                        }
                    }
                    return false;
                }
                case 4 /*GOLD*/:
                case 5 /*CRYSTAL*/:
                {
                    System.Type type = com.oddlabs.tt.model.ExtendedResources.STORES[(((object)task.kind() == (object)com.oddlabs.tt.player.campaign.SagaMission.TaskKind.GOLD) ? com.oddlabs.tt.model.ExtendedResources.GOLD : com.oddlabs.tt.model.ExtendedResources.CRYSTAL)];
                    int n_2 = 0;
                    foreach (com.oddlabs.tt.model.Selectable s_4 in java.lang.JIter.of(me.getUnits().getSet()))
                    {
                        if ((s_4 is com.oddlabs.tt.model.Building b_2) && !b_2.isDead() && b_2.getSupplyContainer(type) != null && ((object)((com.oddlabs.tt.model.BuildingTemplate)b_2.getTemplate()) == (object)race.getBuildingTemplate(com.oddlabs.tt.model.Race.BUILDING_ARMORY)))
                        {
                            n_2 += b_2.getSupplyContainer(type).getNumSupplies();
                        }
                    }
                    return n_2 >= task.count();
                }
                default:
                {
                    return false;
                }
            }
        }
        // Java: 2136
        public string statusText(com.oddlabs.tt.landscape.World world, com.oddlabs.tt.player.Player me)
        {
            java.lang.StringBuilder sb = new java.lang.StringBuilder();
            for (int i = 0; i < done.Length; i++)
            {
                if (!done[i])
                {
                    sb.append(com.oddlabs.tt.player.campaign.SagaIsland.i18n(key("task" + (i))));
                    break;
                }
            }
            if (sb.isEmpty())
            {
                switch ((mission_f.goal_f).ordinal())
                {
                    case 3 /*SURVIVE_TIME*/:
                    {
                        int left = java.lang.Math.max(0, java.lang.JC.f2i(((float)(mission_f.goal_amount - elapsed))));
                        sb.append(com.oddlabs.tt.player.campaign.SagaIsland.i18n("hold_out", left / 60, java.lang.JStr.valueOf(((left % 60 < 10 ? "0" : ""))) + (left % 60)));
                        break;
                    }
                    case 2 /*SURVIVE_WAVES*/:
                    {
                        com.oddlabs.tt.gamemode.GameModeRules rules = com.oddlabs.tt.gamemode.GameModeRegistry.get(world.getGameMode());
                        int wave = ((rules is com.oddlabs.tt.gamemode.survival.SurvivalModeRules s) ? s.getWaveNumber() : 0);
                        sb.append(com.oddlabs.tt.player.campaign.SagaIsland.i18n("waves", java.lang.Math.min(wave, mission_f.goal_amount), mission_f.goal_amount));
                        break;
                    }
                    case 0 /*CONQUER*/:
                    {
                        int left_2 = 0;
                        foreach (com.oddlabs.tt.player.Player p in world.getPlayers())
                        {
                            if (com.oddlabs.tt.player.campaign.SagaIsland.rival(world, me, p) && com.oddlabs.tt.player.campaign.SagaIsland.standing(p))
                            {
                                left_2++;
                            }
                        }
                        sb.append(com.oddlabs.tt.player.campaign.SagaIsland.i18n("tribes_left", left_2));
                        break;
                    }
                    default:
                    {
                        sb.append(com.oddlabs.tt.player.campaign.SagaIsland.i18n(key("goal")));
                        break;
                    }
                }
            }
            return java.lang.JO.toString(sb);
        }
        // Java: 2168
        public string missionHeader()
        {
            return com.oddlabs.tt.player.campaign.SagaIsland.i18n(key("header"));
        }
        // Java: 2172
        public override string getHeader()
        {
            if (home)
            {
                return com.oddlabs.tt.player.campaign.SagaIsland.holdingHeader(saga(), mission_f.index);
            }
            return com.oddlabs.tt.player.campaign.SagaIsland.i18n(key("header"));
        }
        // Java: 2179
        public override string getDescription()
        {
            return com.oddlabs.tt.player.campaign.SagaIsland.i18n(key("description"));
        }
        // Java: 2184
        public override string getCurrentObjective()
        {
            if (mode == com.oddlabs.tt.player.campaign.SagaIsland.DEFEND)
            {
                return com.oddlabs.tt.player.campaign.SagaIsland.i18n("defend_objective", saga().islandName(mission_f.index));
            }
            if (mode == com.oddlabs.tt.player.campaign.SagaIsland.HELP)
            {
                return com.oddlabs.tt.player.campaign.SagaIsland.i18n("help_objective", saga().islandName(mission_f.index));
            }
            if (mode == com.oddlabs.tt.player.campaign.SagaIsland.VISIT)
            {
                return com.oddlabs.tt.player.campaign.SagaIsland.i18n("visit_objective", saga().islandName(mission_f.index), com.oddlabs.tt.player.campaign.SagaIsland.VISIT_GARRISON);
            }
            if (home)
            {
                return com.oddlabs.tt.player.campaign.SagaIsland.i18n("home_objective", population());
            }
            java.lang.StringBuilder sb = new java.lang.StringBuilder(com.oddlabs.tt.player.campaign.SagaIsland.i18n(key("objective")));
            for (int i = 0; i < done.Length; i++)
            {
                sb.append('\n').append((done[i] ? "[x] " : "[ ] ")).append(com.oddlabs.tt.player.campaign.SagaIsland.i18n(key("task" + (i))));
            }
            return java.lang.JO.toString(sb);
        }
        static SagaIsland()
        {
            SagaIsland.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.player.campaign.SagaIsland).getName());
            SagaIsland.AI_TIERS = new int[] { com.oddlabs.tt.net.PlayerSlot.AI_EASY, com.oddlabs.tt.net.PlayerSlot.AI_NORMAL, com.oddlabs.tt.net.PlayerSlot.AI_HARD, com.oddlabs.tt.net.PlayerSlot.AI_EXPERT, com.oddlabs.tt.net.PlayerSlot.AI_GAUNTLET };
            SagaIsland.WEAPONS = new System.Type[] { typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon), typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon), typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon), typeof(com.oddlabs.tt.model.weapon.CrystalWeapon), typeof(com.oddlabs.tt.model.weapon.DiamondWeapon), typeof(com.oddlabs.tt.model.weapon.DartWeapon) };
        }
        private void init_SagaIsland()
        {
            this.mode = com.oddlabs.tt.player.campaign.SagaIsland.MISSION;
            this.base_home_units = new int[0];
            this.base_home_stock = new int[0];
            this.expedition = new int[0];
            this.fleet = new java.util.ArrayList();
            this.fleet_from = new java.util.ArrayList();
            this.raid_fleet = new java.util.ArrayList();
            this.raid_from = new java.util.ArrayList();
            this.reinf_fleet = new java.util.ArrayList();
            this.reinf_from = new java.util.ArrayList();
            this.reinf_owner = new java.util.ArrayList();
            this.ally_call = new java.util.HashMap();
        }
        public SagaIsland(com.oddlabs.tt.player.campaign.SagaCampaign campaign, com.oddlabs.tt.player.campaign.SagaMission mission_v) : base(campaign)
        {
            init_SagaIsland();
            this.mission_f = mission_v;
            this.done = new bool[mission_v.tasks.size()];
        }
        // Java: anonymous class at line 651
        public sealed class Anon1_TreeNodeVisitor : java.lang.Object, com.oddlabs.tt.landscape.TreeNodeVisitor
        {
            public int[] n;
            public Anon1_TreeNodeVisitor(int[] n_c)
            {
                this.n = n_c;
            }
            // Java: 652
            public void visitLeaf(com.oddlabs.tt.landscape.TreeLeaf tree_leaf)
            {
                tree_leaf.visitTrees(this);
            }
            // Java: 657
            public void visitNode(com.oddlabs.tt.landscape.TreeGroup tree_group)
            {
                tree_group.visitChildren(this);
            }
            // Java: 662
            public void visitTree(com.oddlabs.tt.landscape.TreeSupply tree_supply)
            {
                if (!tree_supply.isEmpty() && !tree_supply.isDead())
                {
                    n[0]++;
                }
            }
        }
    }
}
