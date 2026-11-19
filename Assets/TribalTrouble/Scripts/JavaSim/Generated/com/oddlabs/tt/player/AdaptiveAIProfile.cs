// Translated from AdaptiveAIProfile.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player
{
    // Java: 38
    public class AdaptiveAIProfile : java.lang.Object
    {
        public static java.util.logging.Logger logger;
        public const float DEFAULT_SKILL_RATING = 0.5f;
        public const float LEARNING_RATE = 0.15f;
        public const float EASY_SEED_CEILING = 0.35f;
        public const float HARD_SEED_FLOOR = 0.65f;
        public static com.oddlabs.tt.player.AdaptiveAIProfile instance;
        public float skill_rating;
        // Java: 60
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public static com.oddlabs.tt.player.AdaptiveAIProfile get()
        {
            if (com.oddlabs.tt.player.AdaptiveAIProfile.instance == null)
            {
                com.oddlabs.tt.player.AdaptiveAIProfile.instance = new com.oddlabs.tt.player.AdaptiveAIProfile();
                com.oddlabs.tt.player.AdaptiveAIProfile.instance.load();
            }
            return com.oddlabs.tt.player.AdaptiveAIProfile.instance;
        }
        // Java: 72
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public void reset()
        {
            skill_rating = com.oddlabs.tt.player.AdaptiveAIProfile.DEFAULT_SKILL_RATING;
            for (int d = 0; d < 4; d++)
            {
                tier_memory[d] = d;
            }
            save();
        }
        // Java: 79
        public float getSkillRating()
        {
            return skill_rating;
        }
        // Java: 88
        public int getSeedDifficulty()
        {
            if (skill_rating < com.oddlabs.tt.player.AdaptiveAIProfile.EASY_SEED_CEILING)
            {
                return 0;
            }
            if (skill_rating > com.oddlabs.tt.player.AdaptiveAIProfile.HARD_SEED_FLOOR)
            {
                return 2;
            }
            return 1;
        }
        public static float[] TIER_LEARNING;
        public const int TOP_TIER = 5;
        public const int PACKED = 1048576;
        public float[] tier_memory;
        // Java: 109
        public int openingTier(int start)
        {
            int d = java.lang.Math.clamp(start, 0, 3);
            return java.lang.Math.clamp(java.lang.Math.round(tier_memory[d]), d, com.oddlabs.tt.player.AdaptiveAIProfile.TOP_TIER);
        }
        // Java: 115
        public int packedOpeningTiers()
        {
            int packed = com.oddlabs.tt.player.AdaptiveAIProfile.PACKED;
            for (int d = 0; d < 4; d++)
            {
                packed |= openingTier(d) << (3 * d);
            }
            return packed;
        }
        // Java: 123
        public static int unpackOpeningTier(int packed, int start)
        {
            if ((packed & com.oddlabs.tt.player.AdaptiveAIProfile.PACKED) == 0)
            {
                return (-1);
            }
            int d = java.lang.Math.clamp(start, 0, 3);
            return java.lang.Math.clamp((packed >> (3 * d)) & 7, d, com.oddlabs.tt.player.AdaptiveAIProfile.TOP_TIER);
        }
        // Java: 131
        public void recordTierNeeded(int start, int top_tier, bool ai_won)
        {
            int d = java.lang.Math.clamp(start, 0, 3);
            float needed = java.lang.Math.clamp((ai_won ? (float)(top_tier - 0.5f) : (float)(top_tier + 1.0f)), d, com.oddlabs.tt.player.AdaptiveAIProfile.TOP_TIER);
            tier_memory[d] += (float)(com.oddlabs.tt.player.AdaptiveAIProfile.TIER_LEARNING[d] * ((float)(needed - tier_memory[d])));
            save();
        }
        // Java: 138
        public void recordMatchResult(bool player_won)
        {
            if (player_won)
            {
                skill_rating += (float)(com.oddlabs.tt.player.AdaptiveAIProfile.LEARNING_RATE * ((float)(1.0f - skill_rating)));
            }
            else
            {
                skill_rating -= (float)(com.oddlabs.tt.player.AdaptiveAIProfile.LEARNING_RATE * skill_rating);
            }
            skill_rating = java.lang.Math.clamp(skill_rating, 0.0f, 1.0f);
            save();
        }
        // Java: 148
        public java.nio.file.Path file()
        {
            return ((com.oddlabs.tt.render.Renderer.getLocalInput() == null || com.oddlabs.tt.render.Renderer.getLocalInput().getGameDir() == null ? (java.nio.file.Path)(null) : com.oddlabs.tt.render.Renderer.getLocalInput().getGameDir().resolve(com.oddlabs.tt.global.Globals.getAdaptiveAiProfileFileName())));
        }
        // Java: 152
        public void load()
        {
            java.nio.file.Path file_v = file();
            if (file_v == null)
            {
                return;
            }
            if (!java.nio.file.Files.exists(file_v))
            {
                return;
            }
            java.util.Properties props = new java.util.Properties();
            {
                java.io.InputStream @in = java.nio.file.Files.newInputStream(file_v);
                try
                {
                    props.load(@in);
                }
                catch (System.IO.IOException e)
                {
                    com.oddlabs.tt.player.AdaptiveAIProfile.logger.log(java.util.logging.Level.WARNING, "Failed to read adaptive AI profile from " + java.lang.JStr.valueOf(file_v) + "; using default.", e);
                    return;
                }
                finally
                {
                    if (@in != null) @in.close();
                }
            }
            try
            {
                skill_rating = java.lang.Math.clamp(java.lang.Float.parseFloat(props.getProperty("skill_rating", java.lang.Float.toString(com.oddlabs.tt.player.AdaptiveAIProfile.DEFAULT_SKILL_RATING))), 0.0f, 1.0f);
            }
            catch (System.FormatException e_2)
            {
                com.oddlabs.tt.player.AdaptiveAIProfile.logger.log(java.util.logging.Level.WARNING, "Invalid skill_rating in " + java.lang.JStr.valueOf(file_v) + "; using default.", e_2);
                skill_rating = com.oddlabs.tt.player.AdaptiveAIProfile.DEFAULT_SKILL_RATING;
            }
            for (int d = 0; d < 4; d++)
            {
                try
                {
                    tier_memory[d] = java.lang.Math.clamp(java.lang.Float.parseFloat(props.getProperty("tier_memory." + (d), java.lang.Integer.toString(d))), d, com.oddlabs.tt.player.AdaptiveAIProfile.TOP_TIER);
                }
                catch (System.FormatException e_3)
                {
                    tier_memory[d] = d;
                }
            }
        }
        // Java: 182
        public void save()
        {
            java.util.Properties props = new java.util.Properties();
            props.setProperty("skill_rating", java.lang.Float.toString(skill_rating));
            for (int d = 0; d < 4; d++)
            {
                props.setProperty("tier_memory." + (d), java.lang.Float.toString(tier_memory[d]));
            }
            java.nio.file.Path file_v = file();
            if (file_v == null)
            {
                return;
            }
            {
                java.io.OutputStream @out = java.nio.file.Files.newOutputStream(file_v);
                try
                {
                    props.store(@out, java.lang.JO.toString(java.time.Instant.now()));
                }
                catch (System.IO.IOException e)
                {
                    com.oddlabs.tt.player.AdaptiveAIProfile.logger.log(java.util.logging.Level.WARNING, "Failed to write adaptive AI profile to " + java.lang.JStr.valueOf(file_v), e);
                }
                finally
                {
                    if (@out != null) @out.close();
                }
            }
        }
        static AdaptiveAIProfile()
        {
            AdaptiveAIProfile.logger = java.util.logging.Logger.getLogger(typeof(com.oddlabs.tt.player.AdaptiveAIProfile).getName());
            AdaptiveAIProfile.TIER_LEARNING = new float[] { 0.4f, 0.5f, 0.65f, 0.8f };
        }
        private void init_AdaptiveAIProfile()
        {
            this.skill_rating = com.oddlabs.tt.player.AdaptiveAIProfile.DEFAULT_SKILL_RATING;
            this.tier_memory = new float[] { 0.0f, 1.0f, 2.0f, 3.0f };
        }
        public AdaptiveAIProfile() : base()
        {
            init_AdaptiveAIProfile();
        }
    }
}
