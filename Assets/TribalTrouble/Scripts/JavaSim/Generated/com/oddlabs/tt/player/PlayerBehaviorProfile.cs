// Translated from PlayerBehaviorProfile.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player
{
    // Java: 35
    public class PlayerBehaviorProfile : java.lang.Object
    {
        public static java.util.logging.Logger logger;
        public static com.fasterxml.jackson.databind.ObjectMapper MAPPER;
        public static com.fasterxml.jackson.core.type.TypeReference SEGMENT_MAP_TYPE;
        public const float NO_FIRST_ATTACK_DATA = 99999.0f;
        public const float EARLY_RUSH_THRESHOLD_SECONDS = 240.0f;
        public const float FIRST_ATTACK_LEARNING_RATE = 0.3f;
        public const float FORCE_SIZE_LEARNING_RATE = 0.25f;
        // Java: 54
        public class Segment : java.lang.Object
        {
            public float first_attack_seconds;
            public float quarters_hits;
            public float armory_hits;
            public float outpost_hits;
            public float avg_force_size;
            public float avg_force_score;
            public int wave_count;
            public float total_minutes_observed;
            public int matches_played;
            private void init_Segment()
            {
                this.first_attack_seconds = com.oddlabs.tt.player.PlayerBehaviorProfile.NO_FIRST_ATTACK_DATA;
                this.quarters_hits = 0.0f;
                this.armory_hits = 0.0f;
                this.outpost_hits = 0.0f;
                this.avg_force_size = 0.0f;
                this.avg_force_score = 0.0f;
                this.wave_count = 0;
                this.total_minutes_observed = 0.0f;
                this.matches_played = 0;
            }
            public Segment() : base()
            {
                init_Segment();
            }
        }
        public static com.oddlabs.tt.player.PlayerBehaviorProfile instance;
        public java.util.Map segments;
        // Java: 87
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public static com.oddlabs.tt.player.PlayerBehaviorProfile get()
        {
            if (com.oddlabs.tt.player.PlayerBehaviorProfile.instance == null)
            {
                com.oddlabs.tt.player.PlayerBehaviorProfile.instance = new com.oddlabs.tt.player.PlayerBehaviorProfile(com.oddlabs.tt.player.PlayerBehaviorProfile.load());
            }
            return com.oddlabs.tt.player.PlayerBehaviorProfile.instance;
        }
        // Java: 99
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public void reset()
        {
            segments.clear();
            save();
        }
        // Java: 109
        public static string segmentKey(string race, string map_size, string difficulty)
        {
            return java.lang.JStr.valueOf(race) + "|" + java.lang.JStr.valueOf(map_size) + "|" + java.lang.JStr.valueOf(difficulty);
        }
        // Java: 113
        public com.oddlabs.tt.player.PlayerBehaviorProfile.Segment segment(string key)
        {
            return ((com.oddlabs.tt.player.PlayerBehaviorProfile.Segment)segments.computeIfAbsent(key, new java.util.function.Function_L((object k_l) =>
            {
                string k = (string)k_l;
                return new com.oddlabs.tt.player.PlayerBehaviorProfile.Segment();
            })));
        }
        // Java: 121
        public void recordFirstAttackTime(string key, float seconds)
        {
            com.oddlabs.tt.player.PlayerBehaviorProfile.Segment s = segment(key);
            if (s.first_attack_seconds >= com.oddlabs.tt.player.PlayerBehaviorProfile.NO_FIRST_ATTACK_DATA)
            {
                s.first_attack_seconds = seconds;
            }
            else
            {
                s.first_attack_seconds += (float)(com.oddlabs.tt.player.PlayerBehaviorProfile.FIRST_ATTACK_LEARNING_RATE * ((float)(seconds - s.first_attack_seconds)));
            }
            save();
        }
        // Java: 131
        public void recordAttackWave(string key, string kind, int force_size, int force_score)
        {
            com.oddlabs.tt.player.PlayerBehaviorProfile.Segment s = segment(key);
            switch ((kind))
            {
                case "quarters":
                {
                    s.quarters_hits++;
                    break;
                }
                case "armory":
                {
                    s.armory_hits++;
                    break;
                }
                case "outpost":
                {
                    s.outpost_hits++;
                    break;
                }
                default:
                {
                    com.oddlabs.tt.player.PlayerBehaviorProfile.logger.warning("Unknown attack-target kind: " + java.lang.JStr.valueOf(kind));
                    break;
                }
            }
            if (s.wave_count == 0)
            {
                s.avg_force_size = force_size;
                s.avg_force_score = force_score;
            }
            else
            {
                s.avg_force_size += (float)(com.oddlabs.tt.player.PlayerBehaviorProfile.FORCE_SIZE_LEARNING_RATE * ((float)(force_size - s.avg_force_size)));
                s.avg_force_score += (float)(com.oddlabs.tt.player.PlayerBehaviorProfile.FORCE_SIZE_LEARNING_RATE * ((float)(force_score - s.avg_force_score)));
            }
            s.wave_count++;
            save();
        }
        // Java: 154
        public void recordMatchExposure(string key, float minutes_played)
        {
            com.oddlabs.tt.player.PlayerBehaviorProfile.Segment s = segment(key);
            s.total_minutes_observed += minutes_played;
            s.matches_played++;
            save();
        }
        // Java: 165
        public bool isEarlyRusher(string key)
        {
            return segment(key).first_attack_seconds < com.oddlabs.tt.player.PlayerBehaviorProfile.EARLY_RUSH_THRESHOLD_SECONDS;
        }
        // Java: 169
        public float totalHits(com.oddlabs.tt.player.PlayerBehaviorProfile.Segment s)
        {
            return (float)((float)(s.quarters_hits + s.armory_hits) + s.outpost_hits);
        }
        // Java: 173
        public float getQuartersHitShare(string key)
        {
            com.oddlabs.tt.player.PlayerBehaviorProfile.Segment s = segment(key);
            return (totalHits(s) <= 0.0f ? ((float)(1.0f / 3.0f)) : (float)(s.quarters_hits / totalHits(s)));
        }
        // Java: 178
        public float getArmoryHitShare(string key)
        {
            com.oddlabs.tt.player.PlayerBehaviorProfile.Segment s = segment(key);
            return (totalHits(s) <= 0.0f ? ((float)(1.0f / 3.0f)) : (float)(s.armory_hits / totalHits(s)));
        }
        // Java: 183
        public float getOutpostHitShare(string key)
        {
            com.oddlabs.tt.player.PlayerBehaviorProfile.Segment s = segment(key);
            return (totalHits(s) <= 0.0f ? ((float)(1.0f / 3.0f)) : (float)(s.outpost_hits / totalHits(s)));
        }
        // Java: 192
        public float getForceSizeMultiplier(string key, float reference_force_size)
        {
            com.oddlabs.tt.player.PlayerBehaviorProfile.Segment s = segment(key);
            if (s.wave_count == 0 || reference_force_size <= 0.0f)
            {
                return 1.0f;
            }
            return java.lang.Math.clamp((float)(s.avg_force_size / reference_force_size), 1.0f, 2.0f);
        }
        // Java: 199
        public float getAttacksPerMinute(string key)
        {
            com.oddlabs.tt.player.PlayerBehaviorProfile.Segment s = segment(key);
            if (s.total_minutes_observed <= 0.0f)
            {
                return 0.0f;
            }
            return (float)(s.wave_count / s.total_minutes_observed);
        }
        // Java: 206
        public java.nio.file.Path file()
        {
            return ((com.oddlabs.tt.render.Renderer.getLocalInput() == null || com.oddlabs.tt.render.Renderer.getLocalInput().getGameDir() == null ? (java.nio.file.Path)(null) : com.oddlabs.tt.render.Renderer.getLocalInput().getGameDir().resolve(com.oddlabs.tt.global.Globals.getPlayerBehaviorProfileFileName())));
        }
        // Java: 210
        public static java.util.Map load()
        {
            java.nio.file.Path file_v = ((com.oddlabs.tt.render.Renderer.getLocalInput() == null || com.oddlabs.tt.render.Renderer.getLocalInput().getGameDir() == null ? (java.nio.file.Path)(null) : com.oddlabs.tt.render.Renderer.getLocalInput().getGameDir().resolve(com.oddlabs.tt.global.Globals.getPlayerBehaviorProfileFileName())));
            if (file_v == null)
            {
                return new java.util.HashMap();
            }
            if (!java.nio.file.Files.exists(file_v))
            {
                return new java.util.HashMap();
            }
            try
            {
                java.util.Map loaded = ((java.util.Map)com.oddlabs.tt.player.PlayerBehaviorProfile.MAPPER.readValue(file_v.toFile(), com.oddlabs.tt.player.PlayerBehaviorProfile.SEGMENT_MAP_TYPE));
                return (loaded != null ? loaded : (java.util.Map)(new java.util.HashMap()));
            }
            catch (System.IO.IOException e)
            {
                com.oddlabs.tt.player.PlayerBehaviorProfile.logger.log(java.util.logging.Level.WARNING, "Failed to read player behavior profile from " + java.lang.JStr.valueOf(file_v) + "; starting fresh.", e);
                return new java.util.HashMap();
            }
        }
        // Java: 225
        public void save()
        {
            java.nio.file.Path file_v = file();
            if (file_v == null)
            {
                return;
            }
            try
            {
                com.oddlabs.tt.player.PlayerBehaviorProfile.MAPPER.writeValue(file_v.toFile(), segments);
            }
            catch (System.IO.IOException e)
            {
                com.oddlabs.tt.player.PlayerBehaviorProfile.logger.log(java.util.logging.Level.WARNING, "Failed to write player behavior profile to " + java.lang.JStr.valueOf(file_v), e);
            }
        }
        static PlayerBehaviorProfile()
        {
            PlayerBehaviorProfile.logger = java.util.logging.Logger.getLogger(typeof(com.oddlabs.tt.player.PlayerBehaviorProfile).getName());
            PlayerBehaviorProfile.MAPPER = new com.fasterxml.jackson.databind.ObjectMapper().enable(com.fasterxml.jackson.databind.SerializationFeature.INDENT_OUTPUT);
            PlayerBehaviorProfile.SEGMENT_MAP_TYPE = new PlayerBehaviorProfile.Anon1_TypeReference();
        }
        public PlayerBehaviorProfile(java.util.Map segments) : base()
        {
            this.segments = segments;
        }
        // Java: anonymous class at line 38
        public sealed class Anon1_TypeReference : com.fasterxml.jackson.core.type.TypeReference
        {
            public Anon1_TypeReference()
            {
            }
        }
    }
}
