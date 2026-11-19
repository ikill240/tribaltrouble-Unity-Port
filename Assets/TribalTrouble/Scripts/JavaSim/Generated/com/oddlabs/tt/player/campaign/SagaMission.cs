// Translated from SagaMission.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.campaign
{
    // Java: 14
    public class SagaMission : java.lang.Object
    {
        // Java: 16
        public sealed class Goal : java.lang.Enum
        {
            public static Goal[] values() { return (Goal[])VALUES_.Clone(); }
            public static Goal valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly Goal CONQUER = new Goal("CONQUER", 0);
            public static readonly Goal HOLD = new Goal("HOLD", 1);
            public static readonly Goal SURVIVE_WAVES = new Goal("SURVIVE_WAVES", 2);
            public static readonly Goal SURVIVE_TIME = new Goal("SURVIVE_TIME", 3);
            public static readonly Goal REGICIDE = new Goal("REGICIDE", 4);
            public static readonly Goal FLAGS = new Goal("FLAGS", 5);
            public static readonly Goal TASKS = new Goal("TASKS", 6);
            public static readonly Goal[] VALUES_ = new Goal[] { CONQUER, HOLD, SURVIVE_WAVES, SURVIVE_TIME, REGICIDE, FLAGS, TASKS };
            public Goal(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        // Java: 34
        public sealed class TaskKind : java.lang.Enum
        {
            public static TaskKind[] values() { return (TaskKind[])VALUES_.Clone(); }
            public static TaskKind valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly TaskKind BUILD = new TaskKind("BUILD", 0);
            public static readonly TaskKind RESEARCH = new TaskKind("RESEARCH", 1);
            public static readonly TaskKind TRAIN = new TaskKind("TRAIN", 2);
            public static readonly TaskKind GOD_KILLS = new TaskKind("GOD_KILLS", 3);
            public static readonly TaskKind GOLD = new TaskKind("GOLD", 4);
            public static readonly TaskKind CRYSTAL = new TaskKind("CRYSTAL", 5);
            public static readonly TaskKind[] VALUES_ = new TaskKind[] { BUILD, RESEARCH, TRAIN, GOD_KILLS, GOLD, CRYSTAL };
            public TaskKind(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        // Java: 36
        public class Task : java.lang.Object
        {
            public com.oddlabs.tt.player.campaign.SagaMission.TaskKind kind() { return kind_f; }
            public int what() { return what_f; }
            public int count() { return count_f; }
            public Task(com.oddlabs.tt.player.campaign.SagaMission.TaskKind kind, int what, int count)
            {
                this.kind_f = kind;
                this.what_f = what;
                this.count_f = count;
            }
            public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is Task r)) return false; return true && java.lang.JO.equals(kind_f, r.kind_f) && java.lang.JO.equals(what_f, r.what_f) && java.lang.JO.equals(count_f, r.count_f); }
            public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(kind_f); h = 31 * h + java.lang.JO.hashCode(what_f); h = 31 * h + java.lang.JO.hashCode(count_f); } return h; }
            public override string toString() { return "Task[" + "kind=" + java.lang.JO.toString(kind_f) + ", what=" + java.lang.JO.toString(what_f) + ", count=" + java.lang.JO.toString(count_f) + "]"; }
            public com.oddlabs.tt.player.campaign.SagaMission.TaskKind kind_f;
            public int what_f;
            public int count_f;
        }
        // Java: 43
        public class Rival : java.lang.Object
        {
            public bool other_race() { return other_race_f; }
            public int team() { return team_f; }
            public int tier() { return tier_f; }
            public bool @base() { return base_f; }
            public int towers() { return towers_f; }
            public int peons() { return peons_f; }
            public int rock() { return rock_f; }
            public int iron() { return iron_f; }
            public int rubber() { return rubber_f; }
            public bool chieftain() { return chieftain_f; }
            public bool elemental() { return elemental_f; }
            public Rival(bool other_race, int team, int tier, bool @base, int towers, int peons, int rock, int iron, int rubber, bool chieftain, bool elemental)
            {
                this.other_race_f = other_race;
                this.team_f = team;
                this.tier_f = tier;
                this.base_f = @base;
                this.towers_f = towers;
                this.peons_f = peons;
                this.rock_f = rock;
                this.iron_f = iron;
                this.rubber_f = rubber;
                this.chieftain_f = chieftain;
                this.elemental_f = elemental;
            }
            public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is Rival r)) return false; return true && java.lang.JO.equals(other_race_f, r.other_race_f) && java.lang.JO.equals(team_f, r.team_f) && java.lang.JO.equals(tier_f, r.tier_f) && java.lang.JO.equals(base_f, r.base_f) && java.lang.JO.equals(towers_f, r.towers_f) && java.lang.JO.equals(peons_f, r.peons_f) && java.lang.JO.equals(rock_f, r.rock_f) && java.lang.JO.equals(iron_f, r.iron_f) && java.lang.JO.equals(rubber_f, r.rubber_f) && java.lang.JO.equals(chieftain_f, r.chieftain_f) && java.lang.JO.equals(elemental_f, r.elemental_f); }
            public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(other_race_f); h = 31 * h + java.lang.JO.hashCode(team_f); h = 31 * h + java.lang.JO.hashCode(tier_f); h = 31 * h + java.lang.JO.hashCode(base_f); h = 31 * h + java.lang.JO.hashCode(towers_f); h = 31 * h + java.lang.JO.hashCode(peons_f); h = 31 * h + java.lang.JO.hashCode(rock_f); h = 31 * h + java.lang.JO.hashCode(iron_f); h = 31 * h + java.lang.JO.hashCode(rubber_f); h = 31 * h + java.lang.JO.hashCode(chieftain_f); h = 31 * h + java.lang.JO.hashCode(elemental_f); } return h; }
            public override string toString() { return "Rival[" + "other_race=" + java.lang.JO.toString(other_race_f) + ", team=" + java.lang.JO.toString(team_f) + ", tier=" + java.lang.JO.toString(tier_f) + ", base=" + java.lang.JO.toString(base_f) + ", towers=" + java.lang.JO.toString(towers_f) + ", peons=" + java.lang.JO.toString(peons_f) + ", rock=" + java.lang.JO.toString(rock_f) + ", iron=" + java.lang.JO.toString(iron_f) + ", rubber=" + java.lang.JO.toString(rubber_f) + ", chieftain=" + java.lang.JO.toString(chieftain_f) + ", elemental=" + java.lang.JO.toString(elemental_f) + "]"; }
            public bool other_race_f;
            public int team_f;
            public int tier_f;
            public bool base_f;
            public int towers_f;
            public int peons_f;
            public int rock_f;
            public int iron_f;
            public int rubber_f;
            public bool chieftain_f;
            public bool elemental_f;
        }
        public int index;
        public int size;
        public com.oddlabs.tt.procedural.Landscape.TerrainType terrain;
        public float hills;
        public float trees;
        public float supplies;
        public int seed;
        public com.oddlabs.matchmaking.GameMode mode;
        public bool weather;
        public int peons;
        public int rock;
        public int iron;
        public int rubber;
        public bool @base;
        public int stock;
        public com.oddlabs.tt.player.campaign.SagaMission.Goal goal_f;
        public int goal_amount;
        public java.util.List tasks;
        public java.util.List rivals;
        public int[] opens_f;
        public int[] half_opens;
        public bool by_sea;
        public int[] unlocks_f;
        public int[] uses_f;
        public int chapter_f;
        public int[] reward_f;
        public int[] reveals_f;
        public bool befriends_f;
        public bool archipelago_f;
        public int rival_god;
        public int[] known;
        public int speaker;
        public int foe;
        // Java: 87
        public com.oddlabs.tt.player.campaign.SagaMission land(int size, com.oddlabs.tt.procedural.Landscape.TerrainType terrain, float hills, float trees, float supplies)
        {
            this.size = size;
            this.terrain = terrain;
            this.hills = hills;
            this.trees = trees;
            this.supplies = supplies;
            return this;
        }
        // Java: 96
        public com.oddlabs.tt.player.campaign.SagaMission rules(com.oddlabs.matchmaking.GameMode mode, bool weather)
        {
            this.mode = mode;
            this.weather = weather;
            return this;
        }
        // Java: 102
        public com.oddlabs.tt.player.campaign.SagaMission start(int peons, int rock, int iron, int rubber, bool @base, int stock)
        {
            this.peons = peons;
            this.rock = rock;
            this.iron = iron;
            this.rubber = rubber;
            this.@base = @base;
            this.stock = stock;
            return this;
        }
        // Java: 112
        public com.oddlabs.tt.player.campaign.SagaMission goal(com.oddlabs.tt.player.campaign.SagaMission.Goal goal_v, int amount)
        {
            this.goal_f = goal_v;
            this.goal_amount = amount;
            return this;
        }
        // Java: 118
        public com.oddlabs.tt.player.campaign.SagaMission task(com.oddlabs.tt.player.campaign.SagaMission.TaskKind kind, int what, int count)
        {
            tasks.add(new com.oddlabs.tt.player.campaign.SagaMission.Task(kind, what, count));
            return this;
        }
        // Java: 123
        public com.oddlabs.tt.player.campaign.SagaMission rival(bool other_race, int team, int tier, bool @base, int towers, int peons, int rock, int iron, int rubber)
        {
            rivals.add(new com.oddlabs.tt.player.campaign.SagaMission.Rival(other_race, team, tier, @base, towers, peons, rock, iron, rubber, true, false));
            return this;
        }
        // Java: 130
        public com.oddlabs.tt.player.campaign.SagaMission elemental(bool other_race, int team, int tier, bool @base, int towers, int peons, int rock, int iron, int rubber)
        {
            rivals.add(new com.oddlabs.tt.player.campaign.SagaMission.Rival(other_race, team, tier, @base, towers, peons, rock, iron, rubber, true, true));
            return this;
        }
        // Java: 136
        public com.oddlabs.tt.player.campaign.SagaMission ally(int tier, int peons)
        {
            rivals.add(new com.oddlabs.tt.player.campaign.SagaMission.Rival(false, 0, tier, true, 0, peons, 0, 0, 0, true, false));
            return this;
        }
        // Java: 142
        public com.oddlabs.tt.player.campaign.SagaMission allyOther(int tier, int peons)
        {
            rivals.add(new com.oddlabs.tt.player.campaign.SagaMission.Rival(true, 0, tier, true, 0, peons, 2, 2, 0, true, false));
            return this;
        }
        // Java: 147
        public com.oddlabs.tt.player.campaign.SagaMission uses(params int[] keys)
        {
            this.uses_f = keys;
            return this;
        }
        // Java: 152
        public com.oddlabs.tt.player.campaign.SagaMission chapter(int chapter_v)
        {
            this.chapter_f = chapter_v;
            return this;
        }
        // Java: 157
        public com.oddlabs.tt.player.campaign.SagaMission opens(params int[] islands)
        {
            this.opens_f = islands;
            return this;
        }
        // Java: 162
        public com.oddlabs.tt.player.campaign.SagaMission halfOpens(params int[] islands)
        {
            this.half_opens = islands;
            return this;
        }
        // Java: 167
        public com.oddlabs.tt.player.campaign.SagaMission onLand()
        {
            this.by_sea = false;
            return this;
        }
        // Java: 172
        public com.oddlabs.tt.player.campaign.SagaMission unlocks(params int[] keys)
        {
            this.unlocks_f = keys;
            return this;
        }
        // Java: 177
        public com.oddlabs.tt.player.campaign.SagaMission reward(params int[] stock)
        {
            this.reward_f = stock;
            return this;
        }
        // Java: 182
        public com.oddlabs.tt.player.campaign.SagaMission reveals(params int[] islands)
        {
            this.reveals_f = islands;
            return this;
        }
        // Java: 187
        public com.oddlabs.tt.player.campaign.SagaMission archipelago()
        {
            this.archipelago_f = true;
            return this;
        }
        // Java: 192
        public com.oddlabs.tt.player.campaign.SagaMission befriends()
        {
            this.befriends_f = true;
            return this;
        }
        // Java: 197
        public com.oddlabs.tt.player.campaign.SagaMission rivalGod(int unit)
        {
            this.rival_god = unit;
            return this;
        }
        // Java: 202
        public com.oddlabs.tt.player.campaign.SagaMission knows(params int[] upgrades)
        {
            this.known = upgrades;
            return this;
        }
        // Java: 207
        public com.oddlabs.tt.player.campaign.SagaMission faces(int speaker, int foe)
        {
            this.speaker = speaker;
            this.foe = foe;
            return this;
        }
        private void init_SagaMission()
        {
            this.size = 512;
            this.terrain = com.oddlabs.tt.procedural.Landscape.TerrainType.NATIVE;
            this.hills = 1.0f;
            this.trees = 1.0f;
            this.supplies = 1.0f;
            this.mode = com.oddlabs.matchmaking.GameMode.STANDARD;
            this.peons = 10;
            this.goal_f = com.oddlabs.tt.player.campaign.SagaMission.Goal.CONQUER;
            this.tasks = new java.util.ArrayList();
            this.rivals = new java.util.ArrayList();
            this.opens_f = new int[0];
            this.half_opens = new int[0];
            this.by_sea = true;
            this.unlocks_f = new int[0];
            this.uses_f = new int[0];
            this.reward_f = new int[0];
            this.reveals_f = new int[0];
            this.rival_god = (-1);
            this.known = new int[0];
            this.speaker = com.oddlabs.tt.player.campaign.SagaCampaign.FACE_ADVISOR;
            this.foe = com.oddlabs.tt.player.campaign.SagaCampaign.FACE_RIVAL;
        }
        public SagaMission(int index) : base()
        {
            init_SagaMission();
            this.index = index;
            this.seed = 7001 + 977 * index;
        }
    }
}
