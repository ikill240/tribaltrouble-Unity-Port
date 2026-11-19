// Translated from SagaCampaign.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.campaign
{
    // Java: 33
    public class SagaCampaign : com.oddlabs.tt.player.campaign.Campaign
    {
        public const int MAX_UNITS = 60;
        public const int FACE_CHIEF = 0;
        public const int FACE_ADVISOR = 1;
        public const int FACE_WARRIOR = 2;
        public const int FACE_KIN = 3;
        public const int FACE_RIVAL = 4;
        public const int FACE_ORACLE = 5;
        public const int FINAL_ISLAND = 20;
        public const int HOME = 0;
        public static int[] TRIBE_POPULATION;
        public const int HOME_SHARE_PERCENT = 40;
        public const int RAIDS_AFTER = 3;
        public const int DEFEND_NONE = 0;
        public const int DEFEND_SELF = 1;
        public const int DEFEND_ALLY = 2;
        public const int DEFEND_AUTO = 3;
        public const int DEFEND_HELP = 4;
        public const int NUM_PAGES = 3;
        public static int[] INITIAL_STATES;
        public com.oddlabs.tt.player.campaign.SagaIsland[] islands;
        public int defense;
        // Java: 61
        public static int[] initialStates()
        {
            int[] s = new int[com.oddlabs.tt.player.campaign.SagaMissions.COUNT];
            java.util.Arrays.fill(s, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_UNAVAILABLE);
            foreach (int i in com.oddlabs.tt.player.campaign.SagaMissions.HIDDEN)
            {
                s[i] = com.oddlabs.tt.player.campaign.CampaignState.ISLAND_HIDDEN;
            }
            s[com.oddlabs.tt.player.campaign.SagaCampaign.HOME] = com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE;
            return s;
        }
        public int[] chapter_of;
        public int page;
        // Java: 77
        public static com.oddlabs.tt.player.campaign.CampaignState newState(int race)
        {
            com.oddlabs.tt.player.campaign.CampaignState state = new com.oddlabs.tt.player.campaign.CampaignState(com.oddlabs.tt.player.campaign.SagaCampaign.INITIAL_STATES);
            state.setRace(race);
            state.setSaga(true);
            state.setHasRubberWeapons(true);
            state.setHasMagic0(true);
            state.setHasMagic1(true);
            return state;
        }
        // Java: 120
        public int chapterOf(int island_v)
        {
            return (island_v >= 0 && island_v < chapter_of.Length ? chapter_of[island_v] : 0);
        }
        // Java: 125
        public bool chapterDone(int chapter)
        {
            return isClaimed(com.oddlabs.tt.player.campaign.SagaMissions.CHAPTER_END[chapter]);
        }
        // Java: 130
        public bool pageOpen(int p)
        {
            for (int i = 0; i < chapter_of.Length; i++)
            {
                if (chapter_of[i] == p)
                {
                    int st = getState().getIslandState(i);
                    if (st != com.oddlabs.tt.player.campaign.CampaignState.ISLAND_UNAVAILABLE && st != com.oddlabs.tt.player.campaign.CampaignState.ISLAND_HIDDEN)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        // Java: 141
        public int getPage()
        {
            if (page < 0 || !pageOpen(page))
            {
                int cur = getState().getCurrentIsland();
                page = (cur >= 0 ? chapterOf(cur) : 0);
                if (!pageOpen(page))
                {
                    page = 0;
                }
            }
            return page;
        }
        // Java: 151
        public void setPage(int p)
        {
            page = p;
        }
        // Java: 156
        public string pageName(int p)
        {
            if (p >= 2)
            {
                return com.oddlabs.tt.player.campaign.SagaIsland.i18n("page_rift");
            }
            bool tropical = (p == 0) != isNativeTribe();
            return com.oddlabs.tt.player.campaign.SagaIsland.i18n((tropical ? "page_tropical" : "page_northern"));
        }
        // Java: 164
        public int pageStyle(int p)
        {
            if (p >= 2)
            {
                return 2;
            }
            return ((p == 0) != isNativeTribe() ? 0 : 1);
        }
        // Java: 175
        public bool openChapters()
        {
            com.oddlabs.tt.player.campaign.CampaignState state = getState();
            bool opened = false;
            if (chapterDone(0) && state.getIslandState(com.oddlabs.tt.player.campaign.SagaMissions.CHAPTER_START[1]) == com.oddlabs.tt.player.campaign.CampaignState.ISLAND_UNAVAILABLE)
            {
                state.setIslandState(com.oddlabs.tt.player.campaign.SagaMissions.CHAPTER_START[1], com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE);
                opened = true;
            }
            if (chapterDone(0) && chapterDone(1) && isNativeTribe() && !com.oddlabs.tt.global.Settings.getSettings().saga_rift_open)
            {
                com.oddlabs.tt.global.Settings.getSettings().saga_rift_open = true;
                com.oddlabs.tt.global.Settings.getSettings().save();
            }
            if (chapterDone(1) && com.oddlabs.tt.global.Settings.getSettings().saga_rift_open && state.getIslandState(com.oddlabs.tt.player.campaign.SagaMissions.CHAPTER_START[2]) == com.oddlabs.tt.player.campaign.CampaignState.ISLAND_UNAVAILABLE)
            {
                state.setIslandState(com.oddlabs.tt.player.campaign.SagaMissions.CHAPTER_START[2], com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE);
                opened = true;
            }
            return opened;
        }
        // Java: 199
        public void repairRoutes()
        {
            com.oddlabs.tt.player.campaign.CampaignState state = getState();
            int[] halves = new int[islands.Length];
            for (int i = 0; i < islands.Length; i++)
            {
                if (state.getIslandState(i) != com.oddlabs.tt.player.campaign.CampaignState.ISLAND_COMPLETED)
                {
                    continue;
                }
                com.oddlabs.tt.player.campaign.SagaMission m = islands[i].mission();
                foreach (int k in m.opens_f)
                {
                    if (k < islands.Length && state.getIslandState(k) == com.oddlabs.tt.player.campaign.CampaignState.ISLAND_UNAVAILABLE || k < islands.Length && state.getIslandState(k) == com.oddlabs.tt.player.campaign.CampaignState.ISLAND_SEMI_AVAILABLE)
                    {
                        state.forceIslandState(k, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE);
                    }
                }
                foreach (int k_2 in m.half_opens)
                {
                    if (k_2 < islands.Length)
                    {
                        halves[k_2]++;
                    }
                }
                foreach (int k_3 in m.reveals_f)
                {
                    if (k_3 < islands.Length && state.getIslandState(k_3) == com.oddlabs.tt.player.campaign.CampaignState.ISLAND_HIDDEN)
                    {
                        state.forceIslandState(k_3, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE);
                    }
                }
            }
            for (int k_4 = 0; k_4 < islands.Length; k_4++)
            {
                int st = state.getIslandState(k_4);
                if (halves[k_4] >= 2 && (st == com.oddlabs.tt.player.campaign.CampaignState.ISLAND_UNAVAILABLE || st == com.oddlabs.tt.player.campaign.CampaignState.ISLAND_SEMI_AVAILABLE))
                {
                    state.forceIslandState(k_4, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE);
                }
                else if (halves[k_4] == 1 && st == com.oddlabs.tt.player.campaign.CampaignState.ISLAND_UNAVAILABLE)
                {
                    state.forceIslandState(k_4, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_SEMI_AVAILABLE);
                }
            }
            if (state.getIslandState(com.oddlabs.tt.player.campaign.SagaCampaign.HOME) == com.oddlabs.tt.player.campaign.CampaignState.ISLAND_UNAVAILABLE || state.getIslandState(com.oddlabs.tt.player.campaign.SagaCampaign.HOME) == com.oddlabs.tt.player.campaign.CampaignState.ISLAND_HIDDEN)
            {
                state.forceIslandState(com.oddlabs.tt.player.campaign.SagaCampaign.HOME, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE);
            }
        }
        // Java: 229
        public bool isNativeTribe()
        {
            return getState().getRace() == com.oddlabs.tt.player.campaign.CampaignState.RACE_NATIVES;
        }
        // Java: 233
        public override com.oddlabs.tt.gui.CampaignIcons getIcons()
        {
            return com.oddlabs.tt.gui.SagaCampaignIcons.get(isNativeTribe(), getPage(), pageStyle(getPage()));
        }
        // Java: 239
        public bool hasHome()
        {
            return isClaimed(com.oddlabs.tt.player.campaign.SagaCampaign.HOME);
        }
        // Java: 243
        public bool isClaimed(int island_v)
        {
            return getState().getIslandState(island_v) == com.oddlabs.tt.player.campaign.CampaignState.ISLAND_COMPLETED && !getState().isAllyIsle(island_v);
        }
        // Java: 248
        public bool isAllyIsle(int island_v)
        {
            return getState().getIslandState(island_v) == com.oddlabs.tt.player.campaign.CampaignState.ISLAND_COMPLETED && getState().isAllyIsle(island_v);
        }
        // Java: 253
        public int hireFee()
        {
            return java.lang.Math.max(15, 100 - 15 * getState().getSagaFriendship());
        }
        // Java: 258
        public bool canHire(int island_v)
        {
            if (getState().getSagaAllies() <= 0 || island_v < 0 || island_v >= islands.Length || isClaimed(island_v) || getState().isAllyIsle(island_v))
            {
                return false;
            }
            int enemies = 0;
            foreach (com.oddlabs.tt.player.campaign.SagaMission.Rival r in java.lang.JIter.of(islands[island_v].mission().rivals))
            {
                if (r.team() != 0)
                {
                    enemies++;
                }
            }
            return enemies >= 2;
        }
        // Java: 270
        public int claims()
        {
            int n = 0;
            for (int i = 1; i < islands.Length; i++)
            {
                if (isClaimed(i))
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 279
        public int greatTribeLimit()
        {
            return (pageOpen(2) ? 4 : (pageOpen(1) ? 2 : 0));
        }
        // Java: 284
        public int tribeMax()
        {
            int l = java.lang.Math.min(getState().getSagaResearch()[com.oddlabs.tt.model.ResearchContainer.GREAT_TRIBE], greatTribeLimit());
            return com.oddlabs.tt.player.campaign.SagaCampaign.TRIBE_POPULATION[java.lang.Math.clamp(l, 0, com.oddlabs.tt.player.campaign.SagaCampaign.TRIBE_POPULATION.Length - 1)];
        }
        // Java: 290
        public int homeCap()
        {
            return tribeMax() * com.oddlabs.tt.player.campaign.SagaCampaign.HOME_SHARE_PERCENT / 100;
        }
        // Java: 295
        public int weight(int island_v)
        {
            return java.lang.Math.max(1, islands[island_v].mission().size / 256);
        }
        // Java: 303
        public int isleCap(int island_v)
        {
            if (island_v == com.oddlabs.tt.player.campaign.SagaCampaign.HOME)
            {
                return homeCap();
            }
            int total = weight(island_v);
            for (int i = 1; i < islands.Length; i++)
            {
                if (i != island_v && isClaimed(i))
                {
                    total += weight(i);
                }
            }
            return tribeMax() * (100 - com.oddlabs.tt.player.campaign.SagaCampaign.HOME_SHARE_PERCENT) / 100 * weight(island_v) / total;
        }
        public int visiting;
        // Java: 316
        public bool takeVisit(int island_v)
        {
            bool v = visiting == island_v;
            visiting = (-1);
            return v;
        }
        // Java: 323
        public int takeDefense(int island_v)
        {
            int[] raid = getState().getSagaRaid();
            int d = (raid != null && raid[0] == island_v ? defense : com.oddlabs.tt.player.campaign.SagaCampaign.DEFEND_NONE);
            defense = com.oddlabs.tt.player.campaign.SagaCampaign.DEFEND_NONE;
            return d;
        }
        // Java: 331
        public bool canSetSail()
        {
            int current = getState().getCurrentIsland();
            return current >= 0 && current < islands.Length && islands[current].isHolding();
        }
        // Java: 336
        public override void islandChosen(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root, int number)
        {
            if (getState().isAllyIsle(number))
            {
                java.lang.Runnable sail = new java.lang.Runnable_L(() =>
                {
                    visiting = number;
                    startIsland(network, gui_root, number);
                });
                bool ask_v = hasHome() && (getState().getSagaHomePopulation() > 0 || hasStock());
                java.lang.Runnable go_v = (ask_v ? new java.lang.Runnable_L(() =>
                {
                    gui_root.addModalForm(new com.oddlabs.tt.player.campaign.ExpeditionForm(gui_root, getState(), chapterOf(number), sail));
                }) : sail);
                gui_root.addModalForm(new com.oddlabs.tt.form.CampaignDialogForm(com.oddlabs.tt.player.campaign.SagaIsland.i18n("ally_isle_header", islands[number].missionHeader()), java.lang.JStr.valueOf(com.oddlabs.tt.player.campaign.SagaIsland.i18n("ally_isle_description", getState().getSagaFriendship(), hireFee())) + " " + java.lang.JStr.valueOf(com.oddlabs.tt.player.campaign.SagaIsland.i18n("ally_isle_visit", com.oddlabs.tt.player.campaign.SagaIsland.VISIT_GARRISON)), null, com.oddlabs.tt.gui.Origin.AT_START, go_v, true));
                return;
            }
            bool claimed = isClaimed(number);
            getState().setSagaHired(false);
            java.lang.Runnable go = new java.lang.Runnable_L(() =>
            {
                startIsland(network, gui_root, number);
            });
            bool ask = number != com.oddlabs.tt.player.campaign.SagaCampaign.HOME && hasHome() && (getState().getSagaHomePopulation() > 0 || hasStock());
            java.lang.Runnable expedition = (ask ? new java.lang.Runnable_L(() =>
            {
                gui_root.addModalForm(new com.oddlabs.tt.player.campaign.ExpeditionForm(gui_root, getState(), chapterOf(number), go));
            }) : go);
            java.lang.Runnable next = (!claimed && canHire(number) ? new java.lang.Runnable_L(() =>
            {
                gui_root.addModalForm(new com.oddlabs.tt.player.campaign.HireForm(this, gui_root, expedition));
            }) : expedition);
            com.oddlabs.tt.gui.Form dialog = new com.oddlabs.tt.form.CampaignDialogForm((claimed ? com.oddlabs.tt.player.campaign.SagaIsland.holdingHeader(this, number) : islands[number].getHeader()), (claimed ? com.oddlabs.tt.player.campaign.SagaIsland.holdingDescription(this, number) : islands[number].getDescription()), null, com.oddlabs.tt.gui.Origin.AT_START, next, true);
            gui_root.addModalForm(dialog);
        }
        // Java: 367
        public bool hasStock()
        {
            foreach (int s in getState().getSagaStock())
            {
                if (s > 0)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 375
        public void setSail(com.oddlabs.tt.viewer.WorldViewer viewer)
        {
            int current = getState().getCurrentIsland();
            islands[current].keepHolding();
            afterVoyage(current);
            leave(viewer, com.oddlabs.tt.player.campaign.SagaIsland.i18n("set_sail_done"));
        }
        // Java: 383
        public void setSail()
        {
            int current = getState().getCurrentIsland();
            com.oddlabs.tt.viewer.WorldViewer viewer = (current >= 0 ? islands[current].getViewer() : (com.oddlabs.tt.viewer.WorldViewer)(null));
            if (viewer != null)
            {
                setSail(viewer);
            }
        }
        // Java: 391
        public bool canSendForSupplies()
        {
            int current = getState().getCurrentIsland();
            return current >= 0 && current < islands.Length && islands[current].canSendForSupplies();
        }
        // Java: 396
        public void sendForSupplies()
        {
            int current = getState().getCurrentIsland();
            if (current >= 0 && current < islands.Length)
            {
                islands[current].sendForSupplies();
            }
        }
        // Java: 402
        public string setSailText()
        {
            return com.oddlabs.tt.player.campaign.SagaIsland.i18n("set_sail");
        }
        // Java: 406
        public override void onDefeated()
        {
            int current = getState().getCurrentIsland();
            if (current >= 0 && current < islands.Length)
            {
                islands[current].lost();
                afterVoyage(current);
            }
        }
        // Java: 419
        public void afterVoyage(int left)
        {
            com.oddlabs.tt.player.campaign.CampaignState state = getState();
            state.addSagaVoyage();
            openChapters();
            repairRoutes();
            for (int i = 0; i < islands.Length; i++)
            {
                if (i != left && isClaimed(i))
                {
                    state.setIsleDepletion(i, state.getIsleDepletion(i) - 3);
                    if (i != com.oddlabs.tt.player.campaign.SagaCampaign.HOME)
                    {
                        tend(i);
                    }
                }
            }
            int claims_v = claims();
            if (claims_v < com.oddlabs.tt.player.campaign.SagaCampaign.RAIDS_AFTER || state.getSagaRaid() != null)
            {
                return;
            }
            java.util.Random r = new java.util.Random(state.getSagaVoyages() * 7919L + claims_v * 131L + state.getDifficulty());
            int chance = java.lang.Math.min(60, 30 + 6 * (claims_v - com.oddlabs.tt.player.campaign.SagaCampaign.RAIDS_AFTER));
            if (r.nextInt(100) >= chance)
            {
                return;
            }
            int[] held = new int[islands.Length];
            int n = 0;
            for (int i_2 = 0; i_2 < islands.Length; i_2++)
            {
                if (i_2 != left && (isClaimed(i_2) || isAllyIsle(i_2)))
                {
                    held[n++] = i_2;
                }
            }
            if (n == 0)
            {
                return;
            }
            int strength = java.lang.Math.min(6, 1 + (claims_v - com.oddlabs.tt.player.campaign.SagaCampaign.RAIDS_AFTER) / 2 + state.getSagaVoyages() / 10);
            state.setSagaRaid(new int[] { held[r.nextInt(n)], strength });
        }
        // Java: 452
        public void tend(int island_v)
        {
            com.oddlabs.tt.player.campaign.CampaignState state = getState();
            int[] g = state.getIsleUnits(island_v);
            int n = 0;
            foreach (int v in g)
            {
                n += v;
            }
            int cap = isleCap(island_v);
            if (n > 0 && n < cap)
            {
                g[com.oddlabs.tt.model.Race.UNIT_PEON] += java.lang.Math.min(java.lang.Math.max(2, cap / 40), cap - n);
            }
            state.setIsleUnits(island_v, g);
            int[] st = state.getIsleStock(island_v);
            int workers = java.lang.Math.min(10, g[com.oddlabs.tt.model.Race.UNIT_PEON]);
            st[0] += 2 * workers;
            st[1] += workers;
            st[3] += workers / 2;
            state.setIsleStock(island_v, st);
        }
        public bool finale_shown;
        // Java: 473
        public bool firstTimeFinale()
        {
            if (finale_shown || getState().getPrevIsland() == com.oddlabs.tt.player.campaign.SagaCampaign.FINAL_ISLAND)
            {
                return false;
            }
            finale_shown = true;
            return true;
        }
        // Java: 485
        public string chapterNews()
        {
            int cur = getState().getCurrentIsland();
            for (int c = 0; c < com.oddlabs.tt.player.campaign.SagaMissions.CHAPTER_END.Length; c++)
            {
                int end = com.oddlabs.tt.player.campaign.SagaMissions.CHAPTER_END[c];
                if (cur != end || !isClaimed(end) || finale_shown || getState().getPrevIsland() == end)
                {
                    continue;
                }
                finale_shown = true;
                openChapters();
                string tribe = (isNativeTribe() ? "_natives" : "_vikings");
                if (c == 0)
                {
                    page = 1;
                    return com.oddlabs.tt.player.campaign.SagaIsland.i18n("chapter1_done" + java.lang.JStr.valueOf(tribe));
                }
                if (c == 1)
                {
                    if (getState().getIslandState(com.oddlabs.tt.player.campaign.SagaMissions.CHAPTER_START[2]) != com.oddlabs.tt.player.campaign.CampaignState.ISLAND_UNAVAILABLE)
                    {
                        page = 2;
                        return com.oddlabs.tt.player.campaign.SagaIsland.i18n("chapter2_done_rift");
                    }
                    return com.oddlabs.tt.player.campaign.SagaIsland.i18n("chapter2_done_wait");
                }
                return com.oddlabs.tt.player.campaign.SagaIsland.i18n("chapter3_done");
            }
            return null;
        }
        // Java: 511
        public com.oddlabs.tt.player.campaign.SagaIsland island(int i)
        {
            return islands[i];
        }
        // Java: 515
        public string islandName(int island_v)
        {
            return (island_v == com.oddlabs.tt.player.campaign.SagaCampaign.HOME ? com.oddlabs.tt.player.campaign.SagaIsland.i18n("home_header") : islands[island_v].missionHeader());
        }
        // Java: 520
        public void onChart(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root)
        {
            int[] raid = getState().getSagaRaid();
            if (raid == null || !isClaimed(raid[0]) && !isAllyIsle(raid[0]))
            {
                getState().setSagaRaid(null);
                return;
            }
            gui_root.addModalForm(new com.oddlabs.tt.player.campaign.RaidForm(this, network, gui_root, raid[0], raid[1], isAllyIsle(raid[0])));
        }
        // Java: 530
        public void defend(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root, int island_v, int how)
        {
            java.lang.Runnable go = new java.lang.Runnable_L(() =>
            {
                defense = how;
                if (how == com.oddlabs.tt.player.campaign.SagaCampaign.DEFEND_ALLY)
                {
                    getState().addSagaFriendship(1);
                }
                startIsland(network, gui_root, island_v);
            });
            bool ask = (how == com.oddlabs.tt.player.campaign.SagaCampaign.DEFEND_SELF || how == com.oddlabs.tt.player.campaign.SagaCampaign.DEFEND_HELP) && island_v != com.oddlabs.tt.player.campaign.SagaCampaign.HOME && (getState().getSagaHomePopulation() > 0 || hasStock());
            if (ask)
            {
                gui_root.addModalForm(new com.oddlabs.tt.player.campaign.ExpeditionForm(gui_root, getState(), chapterOf(island_v), go));
            }
            else
            {
                go.run();
            }
        }
        // Java: 548
        public void resolve(com.oddlabs.tt.gui.GUIRoot gui_root, int island_v, int strength, bool allies)
        {
            com.oddlabs.tt.player.campaign.CampaignState state = getState();
            if (isAllyIsle(island_v))
            {
                float defence = (float)((float)(8.0f + (float)(3.0f * state.getSagaAllies())) + state.getSagaFriendship());
                bool held = new java.util.Random(state.getSagaVoyages() * 31L + island_v * 977L + strength).nextFloat() < (float)(defence / ((float)((float)(defence + 5.0f) + (float)(5.0f * strength))));
                if (!held)
                {
                    state.setAllyIsle(island_v, false);
                    state.forceIslandState(island_v, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE);
                    state.loseSagaAlly();
                }
                state.setSagaRaid(null);
                save(gui_root);
                gui_root.addModalForm(new com.oddlabs.tt.form.CampaignDialogForm(com.oddlabs.tt.player.campaign.SagaIsland.i18n("raid_header"), com.oddlabs.tt.player.campaign.SagaIsland.i18n((held ? "ally_raid_held" : "ally_raid_lost"), islandName(island_v)), getIcons().getFaces()[(held ? com.oddlabs.tt.player.campaign.SagaCampaign.FACE_CHIEF : com.oddlabs.tt.player.campaign.SagaCampaign.FACE_ADVISOR)], com.oddlabs.tt.gui.Origin.AT_START, new java.lang.Runnable_L(() =>
                {
                }), true));
                return;
            }
            int[] g = state.getIsleUnits(island_v);
            int warriors = 0;
            for (int u = 0; u < g.Length; u++)
            {
                if (com.oddlabs.tt.player.campaign.SagaIsland.isWarrior(u))
                {
                    warriors += g[u];
                }
            }
            int towers = 0;
            int buildings = 0;
            int[] b = state.getIsleBuildings(island_v);
            for (int i = 0; i + 2 < b.Length; i += 3)
            {
                buildings++;
                if (b[i] == com.oddlabs.tt.model.Race.BUILDING_TOWER || b[i] == com.oddlabs.tt.model.Race.BUILDING_PALISADE)
                {
                    towers++;
                }
            }
            float defence_2 = (float)((float)((float)((float)((float)(warriors * 1.3f) + (float)(g[com.oddlabs.tt.model.Race.UNIT_PEON] * 0.3f)) + (float)(towers * 3.0f)) + (float)(buildings * 0.4f)) + ((allies ? (float)(6.0f + (float)(3.0f * state.getSagaAllies())) : 0.0f)));
            float attack = (float)(5.0f + (float)(5.0f * strength));
            java.util.Random r = new java.util.Random(state.getSagaVoyages() * 31L + island_v * 977L + strength);
            bool held_2 = r.nextFloat() < (float)(defence_2 / ((float)(defence_2 + attack)));
            string text = default;
            if (held_2)
            {
                int lost = 0;
                for (int u_2 = 0; u_2 < g.Length; u_2++)
                {
                    if (com.oddlabs.tt.player.campaign.SagaIsland.isWarrior(u_2) && g[u_2] > 0)
                    {
                        int k = java.lang.JC.f2i(((float)(g[u_2] * ((float)(0.15f + (float)(0.2f * r.nextFloat()))))));
                        g[u_2] -= k;
                        lost += k;
                    }
                }
                state.setIsleUnits(island_v, g);
                if (allies)
                {
                    state.addSagaFriendship(1);
                }
                text = com.oddlabs.tt.player.campaign.SagaIsland.i18n((allies ? "raid_held_allies" : "raid_held"), islandName(island_v), lost);
            }
            else
            {
                int fled = loseIsland(island_v, 0.3f);
                text = com.oddlabs.tt.player.campaign.SagaIsland.i18n("raid_lost", islandName(island_v), fled);
            }
            state.setSagaRaid(null);
            save(gui_root);
            gui_root.addModalForm(new com.oddlabs.tt.form.CampaignDialogForm(com.oddlabs.tt.player.campaign.SagaIsland.i18n("raid_header"), text, getIcons().getFaces()[(held_2 ? com.oddlabs.tt.player.campaign.SagaCampaign.FACE_CHIEF : com.oddlabs.tt.player.campaign.SagaCampaign.FACE_ADVISOR)], com.oddlabs.tt.gui.Origin.AT_START, new java.lang.Runnable_L(() =>
            {
            }), true));
        }
        // Java: 612
        public int loseIsland(int island_v, float escape)
        {
            com.oddlabs.tt.player.campaign.CampaignState state = getState();
            int[] g = state.getIsleUnits(island_v);
            int[] home = state.getSagaUnits();
            int fled = 0;
            if (island_v != com.oddlabs.tt.player.campaign.SagaCampaign.HOME)
            {
                int room = homeCap() - state.getSagaHomePopulation();
                for (int u = 0; u < g.Length; u++)
                {
                    int k = java.lang.Math.min(java.lang.Math.max(0, room), java.lang.JC.f2i(((float)(g[u] * escape))));
                    home[u] += k;
                    room -= k;
                    fled += k;
                }
            }
            else
            {
                for (int u_2 = 0; u_2 < home.Length; u_2++)
                {
                    int k_2 = java.lang.JC.f2i(((float)(home[u_2] * escape)));
                    fled += k_2;
                    home[u_2] = k_2;
                }
            }
            if (island_v != com.oddlabs.tt.player.campaign.SagaCampaign.HOME)
            {
                state.clearIsle(island_v);
            }
            else
            {
                state.setSagaBuildings(new int[0]);
                java.util.Arrays.fill(state.getSagaStock(), 0);
            }
            state.forceIslandState(island_v, com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE);
            return fled;
        }
        // Java: 642
        public override string getCurrentObjective()
        {
            if (getState().getCurrentIsland() != (-1))
            {
                return islands[getState().getCurrentIsland()].getCurrentObjective();
            }
            throw new System.InvalidOperationException("No current island");
        }
        // Java: 650
        public override void startIsland(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root, int number)
        {
            if (!hasHome() || number == com.oddlabs.tt.player.campaign.SagaCampaign.HOME && isClaimed(com.oddlabs.tt.player.campaign.SagaCampaign.HOME))
            {
                getState().clearSagaExpedition();
            }
            getState().setCurrentIsland(number);
            com.oddlabs.tt.steam.SteamManager.setCampaignRichPresence("New Tides");
            com.oddlabs.tt.steam.SteamManager.setInActiveWorld(true);
            islands[number].chosen(network, gui_root);
        }
        static SagaCampaign()
        {
            SagaCampaign.TRIBE_POPULATION = new int[] { 500, 750, 1000, 1500, 2000 };
            SagaCampaign.INITIAL_STATES = com.oddlabs.tt.player.campaign.SagaCampaign.initialStates();
        }
        private void init_SagaCampaign()
        {
            this.defense = com.oddlabs.tt.player.campaign.SagaCampaign.DEFEND_NONE;
            this.chapter_of = new int[com.oddlabs.tt.player.campaign.SagaMissions.COUNT];
            this.page = (-1);
            this.visiting = (-1);
        }
        public SagaCampaign(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root, int race) : this(network, gui_root, com.oddlabs.tt.player.campaign.SagaCampaign.newState(race))
        {
        }
        public SagaCampaign(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.player.campaign.CampaignState campaign_state) : base(campaign_state)
        {
            init_SagaCampaign();
            campaign_state.setHasRubberWeapons(true);
            campaign_state.setHasMagic0(true);
            campaign_state.setHasMagic1(true);
            if (campaign_state.getNumIslands() >= 21 && campaign_state.getNumIslands() < com.oddlabs.tt.player.campaign.SagaMissions.COUNT)
            {
                campaign_state.growIslands(com.oddlabs.tt.player.campaign.SagaCampaign.INITIAL_STATES);
            }
            else if (campaign_state.getNumIslands() != com.oddlabs.tt.player.campaign.SagaMissions.COUNT)
            {
                campaign_state.restartIslands(com.oddlabs.tt.player.campaign.SagaCampaign.INITIAL_STATES);
            }
            com.oddlabs.tt.player.campaign.SagaMission[] missions = com.oddlabs.tt.player.campaign.SagaMissions.all();
            islands = new com.oddlabs.tt.player.campaign.SagaIsland[missions.Length];
            for (int i = 0; i < missions.Length; i++)
            {
                islands[i] = new com.oddlabs.tt.player.campaign.SagaIsland(this, missions[i]);
                chapter_of[i] = missions[i].chapter_f;
            }
            for (int i_2 = 0; i_2 < missions.Length && i_2 < campaign_state.getNumIslands(); i_2++)
            {
                if (campaign_state.getIslandState(i_2) == com.oddlabs.tt.player.campaign.CampaignState.ISLAND_COMPLETED)
                {
                    foreach (int k in missions[i_2].unlocks_f)
                    {
                        campaign_state.sagaUnlock(k);
                    }
                }
            }
            if (campaign_state.getSagaEstablished() == null)
            {
                campaign_state.setSagaEstablished(((int[])campaign_state.getSagaUnlocked().clone()));
            }
            openChapters();
            repairRoutes();
            if (getState().getCurrentIsland() == (-1))
            {
                startIsland(network, gui_root, 0);
            }
        }
    }
}
