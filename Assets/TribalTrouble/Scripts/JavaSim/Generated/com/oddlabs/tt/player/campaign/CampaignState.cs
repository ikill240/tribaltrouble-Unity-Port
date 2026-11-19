// Translated from CampaignState.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.campaign
{
    // Java: 8
    public class CampaignState : java.lang.Object
    {
        public const long serialVersionUID = 1L;
        public const int RACE_VIKINGS = 0;
        public const int RACE_NATIVES = 1;
        public const int DIFFICULTY_EASY = 1;
        public const int DIFFICULTY_NORMAL = 0;
        public const int DIFFICULTY_HARD = 2;
        public const int ENEMY_AI_ADAPTIVE = (-1);
        public const int ISLAND_AVAILABLE = 1;
        public const int ISLAND_UNAVAILABLE = 2;
        public const int ISLAND_COMPLETED = 3;
        public const int ISLAND_SEMI_AVAILABLE = 4;
        public const int ISLAND_HIDDEN = 5;
        public int[] island_states;
        public int prev_island;
        public int current_island;
        public int num_peons;
        public int num_rock_warriors;
        public int num_iron_warriors;
        public int num_rubber_warriors;
        public bool has_rubber_weapons;
        public bool has_magic0;
        public bool has_magic1;
        public string name;
        public long date;
        public int race;
        public int difficulty;
        public int enemy_ai;
        public bool saga;
        public int[] saga_unlocked;
        public int[] saga_research;
        public int[] saga_stock;
        public int[] saga_units;
        public int[] saga_buildings;
        public int[] saga_expedition;
        public int[] saga_isle_units;
        public int[] saga_isle_stock;
        public int[] saga_isle_buildings;
        public int[] saga_isle_cap;
        public int[] saga_isle_depletion;
        public int[] saga_isle_regrow;
        public int saga_allies;
        public int saga_voyages;
        public int[] saga_raid;
        public int[] saga_established;
        public int[] saga_isle_people;
        public int[] saga_isle_weapons;
        public const int SAGA_WEAPONS = 6;
        public int[] saga_ally_isles;
        public int saga_friendship;
        public int saga_hired;
        // Java: 93
        public void setIslandState(int index, int state)
        {
            if (island_states[index] == com.oddlabs.tt.player.campaign.CampaignState.ISLAND_SEMI_AVAILABLE && state == com.oddlabs.tt.player.campaign.CampaignState.ISLAND_SEMI_AVAILABLE)
            {
                island_states[index] = com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE;
            }
            else if (island_states[index] != com.oddlabs.tt.player.campaign.CampaignState.ISLAND_COMPLETED)
            {
                island_states[index] = state;
            }
        }
        // Java: 100
        public int getIslandState(int index)
        {
            return island_states[index];
        }
        // Java: 107
        public void setCurrentIsland(int current_island)
        {
            setPrevIsland(this.current_island);
            this.current_island = current_island;
        }
        // Java: 112
        public int getCurrentIsland()
        {
            return current_island;
        }
        // Java: 116
        public void setPrevIsland(int prev_island)
        {
            this.prev_island = prev_island;
        }
        // Java: 120
        public int getPrevIsland()
        {
            return prev_island;
        }
        // Java: 124
        public void setNumPeons(int num_peons)
        {
            this.num_peons = num_peons;
        }
        // Java: 128
        public int getNumPeons()
        {
            return num_peons;
        }
        // Java: 132
        public void setNumRockWarriors(int num_rock_warriors)
        {
            this.num_rock_warriors = num_rock_warriors;
        }
        // Java: 136
        public int getNumRockWarriors()
        {
            return num_rock_warriors;
        }
        // Java: 140
        public void setNumIronWarriors(int num_iron_warriors)
        {
            this.num_iron_warriors = num_iron_warriors;
        }
        // Java: 144
        public int getNumIronWarriors()
        {
            return num_iron_warriors;
        }
        // Java: 148
        public void setNumRubberWarriors(int num_rubber_warriors)
        {
            this.num_rubber_warriors = num_rubber_warriors;
        }
        // Java: 152
        public int getNumRubberWarriors()
        {
            return num_rubber_warriors;
        }
        // Java: 156
        public void setHasRubberWeapons(bool has_rubber_weapons)
        {
            this.has_rubber_weapons = has_rubber_weapons;
        }
        // Java: 160
        public bool hasRubberWeapons()
        {
            return has_rubber_weapons;
        }
        // Java: 164
        public void setHasMagic0(bool has_magic0)
        {
            this.has_magic0 = has_magic0;
        }
        // Java: 168
        public bool hasMagic0()
        {
            return has_magic0;
        }
        // Java: 172
        public void setHasMagic1(bool has_magic1)
        {
            this.has_magic1 = has_magic1;
        }
        // Java: 176
        public bool hasMagic1()
        {
            return has_magic1;
        }
        // Java: 180
        public string getName()
        {
            return name;
        }
        // Java: 184
        public void setName(string name)
        {
            this.name = name;
        }
        // Java: 188
        public long getDate()
        {
            return date;
        }
        // Java: 192
        public void setDate(long date)
        {
            this.date = date;
        }
        // Java: 196
        public void setRace(int race)
        {
            this.race = race;
        }
        // Java: 200
        public int getRace()
        {
            return race;
        }
        // Java: 204
        public void setDifficulty(int difficulty)
        {
            this.difficulty = difficulty;
        }
        // Java: 208
        public int getDifficulty()
        {
            return difficulty;
        }
        // Java: 212
        public int getEnemyAI()
        {
            return enemy_ai;
        }
        // Java: 216
        public void setEnemyAI(int enemy_ai)
        {
            this.enemy_ai = enemy_ai;
        }
        // Java: 220
        public bool isSaga()
        {
            return saga;
        }
        // Java: 224
        public void setSaga(bool saga)
        {
            this.saga = saga;
        }
        // Java: 230
        public int getNumIslands()
        {
            return island_states.Length;
        }
        // Java: 235
        public void growIslands(int[] initial_states)
        {
            if (initial_states.Length <= island_states.Length)
            {
                return;
            }
            int[] a = ((int[])initial_states.clone());
            java.lang.JSystem.arraycopy(island_states, 0, a, 0, island_states.Length);
            island_states = a;
        }
        // Java: 244
        public void forceIslandState(int index, int state)
        {
            island_states[index] = state;
        }
        // Java: 249
        public void restartIslands(int[] initial_states)
        {
            island_states = ((int[])initial_states.clone());
            setCurrentIsland((-1));
        }
        public const int SAGA_STOCKS = 9;
        public const int SAGA_UNITS = 24;
        // Java: 257
        public static int[] sized(int[] a, int n_v)
        {
            if (a != null && a.Length == n_v)
            {
                return a;
            }
            int[] b = new int[n_v];
            if (a != null)
            {
                java.lang.JSystem.arraycopy(a, 0, b, 0, java.lang.Math.min(n_v, a.Length));
            }
            return b;
        }
        // Java: 266
        public int[] getSagaUnlocked()
        {
            return (saga_unlocked == null ? new int[0] : saga_unlocked);
        }
        // Java: 270
        public bool isSagaUnlocked(int key)
        {
            foreach (int k in getSagaUnlocked())
            {
                if (k == key)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 277
        public void sagaUnlock(int key)
        {
            if (isSagaUnlocked(key))
            {
                return;
            }
            int[] a = getSagaUnlocked();
            int[] b = java.util.Arrays.copyOf(a, a.Length + 1);
            b[a.Length] = key;
            saga_unlocked = b;
        }
        // Java: 287
        public int[] getSagaEstablished()
        {
            return saga_established;
        }
        // Java: 291
        public void setSagaEstablished(int[] keys)
        {
            saga_established = keys;
        }
        // Java: 295
        public bool isSagaEstablished(int key)
        {
            if (saga_established == null)
            {
                return true;
            }
            foreach (int k in saga_established)
            {
                if (k == key)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 304
        public void sagaEstablish(int key)
        {
            if (isSagaEstablished(key))
            {
                return;
            }
            int[] b = java.util.Arrays.copyOf(saga_established, saga_established.Length + 1);
            b[saga_established.Length] = key;
            saga_established = b;
        }
        // Java: 313
        public int[] getIsleWeapons(int island)
        {
            saga_isle_weapons = com.oddlabs.tt.player.campaign.CampaignState.sized(saga_isle_weapons, n() * com.oddlabs.tt.player.campaign.CampaignState.SAGA_WEAPONS);
            return java.util.Arrays.copyOfRange(saga_isle_weapons, island * com.oddlabs.tt.player.campaign.CampaignState.SAGA_WEAPONS, (island + 1) * com.oddlabs.tt.player.campaign.CampaignState.SAGA_WEAPONS);
        }
        // Java: 318
        public void setIsleWeapons(int island, int[] weapons)
        {
            saga_isle_weapons = com.oddlabs.tt.player.campaign.CampaignState.sized(saga_isle_weapons, n() * com.oddlabs.tt.player.campaign.CampaignState.SAGA_WEAPONS);
            for (int i = 0; i < com.oddlabs.tt.player.campaign.CampaignState.SAGA_WEAPONS && i < weapons.Length; i++)
            {
                saga_isle_weapons[island * com.oddlabs.tt.player.campaign.CampaignState.SAGA_WEAPONS + i] = weapons[i];
            }
        }
        // Java: 325
        public int[] getIslePeople(int island)
        {
            int[] all = (saga_isle_people == null ? new int[0] : saga_isle_people);
            java.util.List l = new java.util.ArrayList();
            for (int i = 0; i + 5 <= all.Length; i += 5)
            {
                if (all[i] == island)
                {
                    for (int k = 1; k < 5; k++)
                    {
                        l.add(all[i + k]);
                    }
                }
            }
            int[] a = new int[l.size()];
            for (int i_2 = 0; i_2 < a.Length; i_2++)
            {
                a[i_2] = ((int)((int?)l.get(i_2)));
            }
            return a;
        }
        // Java: 338
        public void setIslePeople(int island, int[] quads)
        {
            int[] all = (saga_isle_people == null ? new int[0] : saga_isle_people);
            java.util.List l = new java.util.ArrayList();
            for (int i = 0; i + 5 <= all.Length; i += 5)
            {
                if (all[i] != island)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        l.add(all[i + k]);
                    }
                }
            }
            for (int i_2 = 0; i_2 + 4 <= quads.Length; i_2 += 4)
            {
                l.add(island);
                for (int k_2 = 0; k_2 < 4; k_2++)
                {
                    l.add(quads[i_2 + k_2]);
                }
            }
            int[] a = new int[l.size()];
            for (int i_3 = 0; i_3 < a.Length; i_3++)
            {
                a[i_3] = ((int)((int?)l.get(i_3)));
            }
            saga_isle_people = a;
        }
        // Java: 356
        public int[] getSagaResearch()
        {
            return saga_research = com.oddlabs.tt.player.campaign.CampaignState.sized(saga_research, 64);
        }
        // Java: 360
        public int[] getSagaStock()
        {
            return saga_stock = com.oddlabs.tt.player.campaign.CampaignState.sized(saga_stock, com.oddlabs.tt.player.campaign.CampaignState.SAGA_STOCKS);
        }
        // Java: 364
        public int[] getSagaUnits()
        {
            return saga_units = com.oddlabs.tt.player.campaign.CampaignState.sized(saga_units, com.oddlabs.tt.player.campaign.CampaignState.SAGA_UNITS);
        }
        // Java: 368
        public int[] getSagaBuildings()
        {
            return (saga_buildings == null ? new int[0] : saga_buildings);
        }
        // Java: 372
        public void setSagaBuildings(int[] buildings)
        {
            saga_buildings = buildings;
        }
        // Java: 377
        public int[] getSagaExpedition()
        {
            return saga_expedition = com.oddlabs.tt.player.campaign.CampaignState.sized(saga_expedition, com.oddlabs.tt.player.campaign.CampaignState.SAGA_UNITS + com.oddlabs.tt.player.campaign.CampaignState.SAGA_STOCKS);
        }
        // Java: 381
        public void clearSagaExpedition()
        {
            saga_expedition = null;
        }
        // Java: 385
        public int n()
        {
            return island_states.Length;
        }
        // Java: 390
        public int[] getIsleUnits(int island)
        {
            if (island == 0)
            {
                return getSagaUnits();
            }
            saga_isle_units = com.oddlabs.tt.player.campaign.CampaignState.sized(saga_isle_units, n() * com.oddlabs.tt.player.campaign.CampaignState.SAGA_UNITS);
            int[] a = new int[com.oddlabs.tt.player.campaign.CampaignState.SAGA_UNITS];
            java.lang.JSystem.arraycopy(saga_isle_units, island * com.oddlabs.tt.player.campaign.CampaignState.SAGA_UNITS, a, 0, com.oddlabs.tt.player.campaign.CampaignState.SAGA_UNITS);
            return a;
        }
        // Java: 399
        public void setIsleUnits(int island, int[] units)
        {
            if (island == 0)
            {
                java.lang.JSystem.arraycopy(units, 0, getSagaUnits(), 0, com.oddlabs.tt.player.campaign.CampaignState.SAGA_UNITS);
                return;
            }
            saga_isle_units = com.oddlabs.tt.player.campaign.CampaignState.sized(saga_isle_units, n() * com.oddlabs.tt.player.campaign.CampaignState.SAGA_UNITS);
            java.lang.JSystem.arraycopy(units, 0, saga_isle_units, island * com.oddlabs.tt.player.campaign.CampaignState.SAGA_UNITS, com.oddlabs.tt.player.campaign.CampaignState.SAGA_UNITS);
        }
        // Java: 409
        public void restrideUnits(int old)
        {
            if (old <= 0 || old == com.oddlabs.tt.player.campaign.CampaignState.SAGA_UNITS)
            {
                return;
            }
            if (saga_isle_units != null && saga_isle_units.Length % old == 0)
            {
                int k = saga_isle_units.Length / old;
                int[] a = new int[k * com.oddlabs.tt.player.campaign.CampaignState.SAGA_UNITS];
                for (int i = 0; i < k; i++)
                {
                    java.lang.JSystem.arraycopy(saga_isle_units, i * old, a, i * com.oddlabs.tt.player.campaign.CampaignState.SAGA_UNITS, java.lang.Math.min(old, com.oddlabs.tt.player.campaign.CampaignState.SAGA_UNITS));
                }
                saga_isle_units = a;
            }
            if (saga_expedition != null && saga_expedition.Length >= old)
            {
                int[] a_2 = new int[com.oddlabs.tt.player.campaign.CampaignState.SAGA_UNITS + com.oddlabs.tt.player.campaign.CampaignState.SAGA_STOCKS];
                java.lang.JSystem.arraycopy(saga_expedition, 0, a_2, 0, java.lang.Math.min(old, com.oddlabs.tt.player.campaign.CampaignState.SAGA_UNITS));
                java.lang.JSystem.arraycopy(saga_expedition, old, a_2, com.oddlabs.tt.player.campaign.CampaignState.SAGA_UNITS, java.lang.Math.min(com.oddlabs.tt.player.campaign.CampaignState.SAGA_STOCKS, saga_expedition.Length - old));
                saga_expedition = a_2;
            }
        }
        // Java: 428
        public void restrideStock()
        {
            if (saga_isle_stock != null && saga_isle_stock.Length == n() * 8 && com.oddlabs.tt.player.campaign.CampaignState.SAGA_STOCKS != 8)
            {
                int[] a = new int[n() * com.oddlabs.tt.player.campaign.CampaignState.SAGA_STOCKS];
                for (int i = 0; i < n(); i++)
                {
                    java.lang.JSystem.arraycopy(saga_isle_stock, i * 8, a, i * com.oddlabs.tt.player.campaign.CampaignState.SAGA_STOCKS, 8);
                }
                saga_isle_stock = a;
            }
        }
        // Java: 437
        public int[] getIsleStock(int island)
        {
            if (island == 0)
            {
                return getSagaStock();
            }
            restrideStock();
            saga_isle_stock = com.oddlabs.tt.player.campaign.CampaignState.sized(saga_isle_stock, n() * com.oddlabs.tt.player.campaign.CampaignState.SAGA_STOCKS);
            int[] a = new int[com.oddlabs.tt.player.campaign.CampaignState.SAGA_STOCKS];
            java.lang.JSystem.arraycopy(saga_isle_stock, island * com.oddlabs.tt.player.campaign.CampaignState.SAGA_STOCKS, a, 0, com.oddlabs.tt.player.campaign.CampaignState.SAGA_STOCKS);
            return a;
        }
        // Java: 447
        public void setIsleStock(int island, int[] stock)
        {
            if (island == 0)
            {
                java.lang.JSystem.arraycopy(stock, 0, getSagaStock(), 0, com.oddlabs.tt.player.campaign.CampaignState.SAGA_STOCKS);
                return;
            }
            restrideStock();
            saga_isle_stock = com.oddlabs.tt.player.campaign.CampaignState.sized(saga_isle_stock, n() * com.oddlabs.tt.player.campaign.CampaignState.SAGA_STOCKS);
            java.lang.JSystem.arraycopy(stock, 0, saga_isle_stock, island * com.oddlabs.tt.player.campaign.CampaignState.SAGA_STOCKS, java.lang.Math.min(stock.Length, com.oddlabs.tt.player.campaign.CampaignState.SAGA_STOCKS));
        }
        // Java: 458
        public int[] getIsleBuildings(int island)
        {
            if (island == 0)
            {
                return getSagaBuildings();
            }
            int[] all = (saga_isle_buildings == null ? new int[0] : saga_isle_buildings);
            int count = 0;
            for (int i = 0; i + 4 <= all.Length; i += 4)
            {
                if (all[i] == island)
                {
                    count++;
                }
            }
            int[] a = new int[count * 3];
            int k = 0;
            for (int i_2 = 0; i_2 + 4 <= all.Length; i_2 += 4)
            {
                if (all[i_2] == island)
                {
                    a[k++] = all[i_2 + 1];
                    a[k++] = all[i_2 + 2];
                    a[k++] = all[i_2 + 3];
                }
            }
            return a;
        }
        // Java: 477
        public void setIsleBuildings(int island, int[] triples)
        {
            if (island == 0)
            {
                setSagaBuildings(triples);
                return;
            }
            int[] all = (saga_isle_buildings == null ? new int[0] : saga_isle_buildings);
            java.util.List l = new java.util.ArrayList();
            for (int i = 0; i + 4 <= all.Length; i += 4)
            {
                if (all[i] != island)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        l.add(all[i + j]);
                    }
                }
            }
            for (int i_2 = 0; i_2 + 3 <= triples.Length; i_2 += 3)
            {
                l.add(island);
                l.add(triples[i_2]);
                l.add(triples[i_2 + 1]);
                l.add(triples[i_2 + 2]);
            }
            int[] a = new int[l.size()];
            for (int i_3 = 0; i_3 < a.Length; i_3++)
            {
                a[i_3] = ((int)((int?)l.get(i_3)));
            }
            saga_isle_buildings = a;
        }
        // Java: 501
        public int getIsleCap(int island)
        {
            saga_isle_cap = com.oddlabs.tt.player.campaign.CampaignState.sized(saga_isle_cap, n());
            return saga_isle_cap[island];
        }
        // Java: 506
        public void setIsleCap(int island, int cap)
        {
            saga_isle_cap = com.oddlabs.tt.player.campaign.CampaignState.sized(saga_isle_cap, n());
            saga_isle_cap[island] = cap;
        }
        // Java: 512
        public const int REGROW_HOME_MINUTES = 5 * 60;
        public const int REGROW_ISLE_MINUTES = 3 * 60;
        public static int nowMinutes()
        {
            return (int)(java.lang.JSystem.currentTimeMillis() / 60000L);
        }
        public int getIsleDepletion(int island)
        {
            saga_isle_depletion = com.oddlabs.tt.player.campaign.CampaignState.sized(saga_isle_depletion, n());
            saga_isle_regrow = com.oddlabs.tt.player.campaign.CampaignState.sized(saga_isle_regrow, n());
            if (saga_isle_depletion[island] > 0)
            {
                int now = com.oddlabs.tt.player.campaign.CampaignState.nowMinutes();
                int since = saga_isle_regrow[island];
                int wait = (island == com.oddlabs.tt.player.campaign.SagaCampaign.HOME ? com.oddlabs.tt.player.campaign.CampaignState.REGROW_HOME_MINUTES : com.oddlabs.tt.player.campaign.CampaignState.REGROW_ISLE_MINUTES);
                if (since <= 0 || since > now)
                {
                    saga_isle_regrow[island] = now;
                }
                else if (now - since >= wait)
                {
                    saga_isle_depletion[island] = 0;
                    saga_isle_regrow[island] = 0;
                }
            }
            return saga_isle_depletion[island];
        }
        // Java: 517
        public void setIsleDepletion(int island, int percent)
        {
            saga_isle_depletion = com.oddlabs.tt.player.campaign.CampaignState.sized(saga_isle_depletion, n());
            saga_isle_regrow = com.oddlabs.tt.player.campaign.CampaignState.sized(saga_isle_regrow, n());
            int p = java.lang.Math.clamp(percent, 0, 90);
            if (p <= 0)
            {
                saga_isle_regrow[island] = 0;
            }
            else if (saga_isle_depletion[island] <= 0 || saga_isle_regrow[island] <= 0)
            {
                saga_isle_regrow[island] = com.oddlabs.tt.player.campaign.CampaignState.nowMinutes();
            }
            saga_isle_depletion[island] = p;
        }
        // Java: 523
        public void clearIsle(int island)
        {
            setIsleUnits(island, new int[com.oddlabs.tt.player.campaign.CampaignState.SAGA_UNITS]);
            setIsleStock(island, new int[com.oddlabs.tt.player.campaign.CampaignState.SAGA_STOCKS]);
            setIsleBuildings(island, new int[0]);
            setIsleCap(island, 0);
            setIslePeople(island, new int[0]);
            setIsleWeapons(island, new int[com.oddlabs.tt.player.campaign.CampaignState.SAGA_WEAPONS]);
        }
        // Java: 532
        public int getSagaAllies()
        {
            return saga_allies;
        }
        // Java: 536
        public void addSagaAlly()
        {
            saga_allies++;
        }
        // Java: 540
        public void loseSagaAlly()
        {
            saga_allies = java.lang.Math.max(0, saga_allies - 1);
        }
        // Java: 545
        public int getAllyIsleCount()
        {
            return (saga_ally_isles == null ? 0 : saga_ally_isles.Length);
        }
        // Java: 549
        public bool isAllyIsle(int island)
        {
            if (saga_ally_isles != null)
            {
                foreach (int i in saga_ally_isles)
                {
                    if (i == island)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        // Java: 557
        public void setAllyIsle(int island, bool ally)
        {
            java.util.List l = new java.util.ArrayList();
            if (saga_ally_isles != null)
            {
                foreach (int i in saga_ally_isles)
                {
                    if (i != island)
                    {
                        l.add(i);
                    }
                }
            }
            if (ally)
            {
                l.add(island);
            }
            saga_ally_isles = l.stream().mapToInt(new java.util.function.ToIntFunction_L((object a0) => { return ((int?)a0).intValue(); })).toArray();
        }
        // Java: 568
        public int getSagaFriendship()
        {
            return saga_friendship;
        }
        // Java: 572
        public void addSagaFriendship(int n_v)
        {
            saga_friendship = java.lang.Math.max(0, saga_friendship + n_v);
        }
        // Java: 576
        public bool isSagaHired()
        {
            return saga_hired != 0;
        }
        // Java: 580
        public void setSagaHired(bool hired)
        {
            saga_hired = (hired ? 1 : 0);
        }
        // Java: 584
        public int getSagaVoyages()
        {
            return saga_voyages;
        }
        // Java: 588
        public void addSagaVoyage()
        {
            saga_voyages++;
        }
        // Java: 593
        public int[] getSagaRaid()
        {
            return saga_raid;
        }
        // Java: 597
        public void setSagaRaid(int[] raid)
        {
            saga_raid = raid;
        }
        // Java: 601
        public int getSagaHomePopulation()
        {
            int n_v = 0;
            foreach (int c in getSagaUnits())
            {
                n_v += c;
            }
            return n_v;
        }
        // Java: 611
        public static string ints(int[] a)
        {
            if (a == null)
            {
                return "";
            }
            java.lang.StringBuilder sb = new java.lang.StringBuilder();
            for (int i = 0; i < a.Length; i++)
            {
                sb.append((i == 0 ? "" : ",")).append(a[i]);
            }
            return java.lang.JO.toString(sb);
        }
        // Java: 620
        public static int[] ints(string s)
        {
            if (s == null || s.isBlank())
            {
                return null;
            }
            string[] p = s.trim().split(",");
            int[] a = new int[p.Length];
            for (int i = 0; i < p.Length; i++)
            {
                a[i] = java.lang.Integer.parseInt(p[i].trim());
            }
            return a;
        }
        // Java: 630
        public void write(java.lang.StringBuilder sb)
        {
            sb.append("name=").append(name.replace('\n', ' ').replace('\r', ' ')).append('\n');
            sb.append("date=").append(date).append('\n');
            sb.append("race=").append(race).append('\n');
            sb.append("difficulty=").append(difficulty).append('\n');
            sb.append("enemy_ai=").append(enemy_ai).append('\n');
            sb.append("islands=").append(com.oddlabs.tt.player.campaign.CampaignState.ints(island_states)).append('\n');
            sb.append("current=").append(current_island).append('\n');
            sb.append("prev=").append(prev_island).append('\n');
            sb.append("peons=").append(num_peons).append('\n');
            sb.append("rock=").append(num_rock_warriors).append('\n');
            sb.append("iron=").append(num_iron_warriors).append('\n');
            sb.append("rubber=").append(num_rubber_warriors).append('\n');
            sb.append("rubber_weapons=").append((has_rubber_weapons ? 1 : 0)).append('\n');
            sb.append("magic0=").append((has_magic0 ? 1 : 0)).append('\n');
            sb.append("magic1=").append((has_magic1 ? 1 : 0)).append('\n');
            sb.append("saga=").append((saga ? 1 : 0)).append('\n');
            sb.append("saga_unlocked=").append(com.oddlabs.tt.player.campaign.CampaignState.ints(saga_unlocked)).append('\n');
            sb.append("saga_research=").append(com.oddlabs.tt.player.campaign.CampaignState.ints(saga_research)).append('\n');
            sb.append("saga_stock=").append(com.oddlabs.tt.player.campaign.CampaignState.ints(saga_stock)).append('\n');
            sb.append("saga_units=").append(com.oddlabs.tt.player.campaign.CampaignState.ints(saga_units)).append('\n');
            sb.append("saga_buildings=").append(com.oddlabs.tt.player.campaign.CampaignState.ints(saga_buildings)).append('\n');
            sb.append("saga_expedition=").append(com.oddlabs.tt.player.campaign.CampaignState.ints(saga_expedition)).append('\n');
            sb.append("saga_isle_units=").append(com.oddlabs.tt.player.campaign.CampaignState.ints(saga_isle_units)).append('\n');
            sb.append("saga_unit_stride=").append(com.oddlabs.tt.player.campaign.CampaignState.SAGA_UNITS).append('\n');
            sb.append("saga_isle_stock=").append(com.oddlabs.tt.player.campaign.CampaignState.ints(saga_isle_stock)).append('\n');
            sb.append("saga_isle_buildings=").append(com.oddlabs.tt.player.campaign.CampaignState.ints(saga_isle_buildings)).append('\n');
            sb.append("saga_isle_cap=").append(com.oddlabs.tt.player.campaign.CampaignState.ints(saga_isle_cap)).append('\n');
            sb.append("saga_isle_depletion=").append(com.oddlabs.tt.player.campaign.CampaignState.ints(saga_isle_depletion)).append('\n');
            sb.append("saga_isle_regrow=").append(com.oddlabs.tt.player.campaign.CampaignState.ints(saga_isle_regrow)).append('\n');
            sb.append("saga_allies=").append(saga_allies).append('\n');
            sb.append("saga_voyages=").append(saga_voyages).append('\n');
            sb.append("saga_raid=").append(com.oddlabs.tt.player.campaign.CampaignState.ints(saga_raid)).append('\n');
            sb.append("saga_established=").append((saga_established == null ? "-" : com.oddlabs.tt.player.campaign.CampaignState.ints(saga_established))).append('\n');
            sb.append("saga_isle_people=").append(com.oddlabs.tt.player.campaign.CampaignState.ints(saga_isle_people)).append('\n');
            sb.append("saga_isle_weapons=").append(com.oddlabs.tt.player.campaign.CampaignState.ints(saga_isle_weapons)).append('\n');
            sb.append("saga_ally_isles=").append(com.oddlabs.tt.player.campaign.CampaignState.ints(saga_ally_isles)).append('\n');
            sb.append("saga_friendship=").append(saga_friendship).append('\n');
            sb.append("saga_hired=").append(saga_hired).append('\n');
        }
        // Java: 670
        public static com.oddlabs.tt.player.campaign.CampaignState read(java.util.Map m)
        {
            int[] islands = com.oddlabs.tt.player.campaign.CampaignState.ints(((string)m.get("islands")));
            com.oddlabs.tt.player.campaign.CampaignState c = new com.oddlabs.tt.player.campaign.CampaignState((islands != null ? islands : new int[0]));
            c.name = ((string)m.getOrDefault("name", ""));
            c.date = java.lang.Long.parseLong(((string)m.getOrDefault("date", "0")).trim());
            c.race = java.lang.Integer.parseInt(((string)m.getOrDefault("race", "0")).trim());
            c.difficulty = java.lang.Integer.parseInt(((string)m.getOrDefault("difficulty", "0")).trim());
            c.enemy_ai = java.lang.Integer.parseInt(((string)m.getOrDefault("enemy_ai", "0")).trim());
            c.current_island = java.lang.Integer.parseInt(((string)m.getOrDefault("current", "-1")).trim());
            c.prev_island = java.lang.Integer.parseInt(((string)m.getOrDefault("prev", "-1")).trim());
            c.num_peons = java.lang.Integer.parseInt(((string)m.getOrDefault("peons", "10")).trim());
            c.num_rock_warriors = java.lang.Integer.parseInt(((string)m.getOrDefault("rock", "0")).trim());
            c.num_iron_warriors = java.lang.Integer.parseInt(((string)m.getOrDefault("iron", "0")).trim());
            c.num_rubber_warriors = java.lang.Integer.parseInt(((string)m.getOrDefault("rubber", "0")).trim());
            c.has_rubber_weapons = java.lang.JO.equals("1", ((string)m.getOrDefault("rubber_weapons", "0")).trim());
            c.has_magic0 = java.lang.JO.equals("1", ((string)m.getOrDefault("magic0", "0")).trim());
            c.has_magic1 = java.lang.JO.equals("1", ((string)m.getOrDefault("magic1", "0")).trim());
            c.saga = java.lang.JO.equals("1", ((string)m.getOrDefault("saga", "0")).trim());
            c.saga_unlocked = com.oddlabs.tt.player.campaign.CampaignState.ints(((string)m.get("saga_unlocked")));
            c.saga_research = com.oddlabs.tt.player.campaign.CampaignState.ints(((string)m.get("saga_research")));
            c.saga_stock = com.oddlabs.tt.player.campaign.CampaignState.ints(((string)m.get("saga_stock")));
            c.saga_units = com.oddlabs.tt.player.campaign.CampaignState.ints(((string)m.get("saga_units")));
            c.saga_buildings = com.oddlabs.tt.player.campaign.CampaignState.ints(((string)m.get("saga_buildings")));
            c.saga_expedition = com.oddlabs.tt.player.campaign.CampaignState.ints(((string)m.get("saga_expedition")));
            c.saga_isle_units = com.oddlabs.tt.player.campaign.CampaignState.ints(((string)m.get("saga_isle_units")));
            c.saga_isle_stock = com.oddlabs.tt.player.campaign.CampaignState.ints(((string)m.get("saga_isle_stock")));
            c.saga_isle_buildings = com.oddlabs.tt.player.campaign.CampaignState.ints(((string)m.get("saga_isle_buildings")));
            c.saga_isle_cap = com.oddlabs.tt.player.campaign.CampaignState.ints(((string)m.get("saga_isle_cap")));
            c.saga_isle_depletion = com.oddlabs.tt.player.campaign.CampaignState.ints(((string)m.get("saga_isle_depletion")));
            c.saga_isle_regrow = com.oddlabs.tt.player.campaign.CampaignState.ints(((string)m.get("saga_isle_regrow")));
            c.saga_allies = java.lang.Integer.parseInt(((string)m.getOrDefault("saga_allies", "0")).trim());
            c.saga_voyages = java.lang.Integer.parseInt(((string)m.getOrDefault("saga_voyages", "0")).trim());
            c.saga_raid = com.oddlabs.tt.player.campaign.CampaignState.ints(((string)m.get("saga_raid")));
            string est = ((string)m.get("saga_established"));
            c.saga_established = (est == null || java.lang.JO.equals(est.trim(), "-") ? (int[])(null) : com.oddlabs.tt.player.campaign.CampaignState.ints(est));
            if (c.saga_established == null && est != null && !java.lang.JO.equals(est.trim(), "-"))
            {
                c.saga_established = new int[0];
            }
            c.saga_isle_people = com.oddlabs.tt.player.campaign.CampaignState.ints(((string)m.get("saga_isle_people")));
            c.saga_isle_weapons = com.oddlabs.tt.player.campaign.CampaignState.ints(((string)m.get("saga_isle_weapons")));
            c.saga_ally_isles = com.oddlabs.tt.player.campaign.CampaignState.ints(((string)m.get("saga_ally_isles")));
            c.saga_friendship = java.lang.Integer.parseInt(((string)m.getOrDefault("saga_friendship", "0")).trim());
            c.saga_hired = java.lang.Integer.parseInt(((string)m.getOrDefault("saga_hired", "0")).trim());
            c.restrideUnits(java.lang.Integer.parseInt(((string)m.getOrDefault("saga_unit_stride", "20")).trim()));
            return c;
        }
        // Java: 716
        public static string encodeAll(com.oddlabs.tt.player.campaign.CampaignState[] states)
        {
            java.lang.StringBuilder sb = new java.lang.StringBuilder("# Tribal Trouble campaigns\n");
            foreach (com.oddlabs.tt.player.campaign.CampaignState c in states)
            {
                sb.append('\n');
                c.write(sb);
            }
            return java.lang.JO.toString(sb);
        }
        // Java: 725
        public static com.oddlabs.tt.player.campaign.CampaignState[] decodeAll(string text)
        {
            java.util.List list = new java.util.ArrayList();
            java.util.Map m = new java.util.HashMap();
            foreach (string raw in (java.lang.JStr.valueOf(text) + "\n\n").split("\n"))
            {
                string line = raw.trim();
                if (line.isEmpty())
                {
                    if (m.containsKey("islands"))
                    {
                        list.add(com.oddlabs.tt.player.campaign.CampaignState.read(m));
                    }
                    m = new java.util.HashMap();
                    continue;
                }
                if (line.startsWith("#"))
                {
                    continue;
                }
                int eq = line.indexOf('=');
                if (eq > 0)
                {
                    m.put(line.substring(0, eq).trim(), line.substring(eq + 1));
                }
            }
            if (m.containsKey("islands"))
            {
                list.add(com.oddlabs.tt.player.campaign.CampaignState.read(m));
            }
            return ((com.oddlabs.tt.player.campaign.CampaignState[])list.toArray(new com.oddlabs.tt.player.campaign.CampaignState[0]));
        }
        private void init_CampaignState()
        {
            this.current_island = (-1);
            this.num_peons = 10;
            this.num_rock_warriors = 0;
            this.num_iron_warriors = 0;
            this.num_rubber_warriors = 0;
            this.has_rubber_weapons = false;
            this.has_magic0 = false;
            this.has_magic1 = false;
        }
        public CampaignState(int[] initial_states) : base()
        {
            init_CampaignState();
            island_states = new int[initial_states.Length];
            java.lang.JSystem.arraycopy(initial_states, 0, island_states, 0, island_states.Length);
            name = "";
        }
    }
}
