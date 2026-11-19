// Translated from AiBehaviorBandit.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player
{
    // Java: 34
    public class AiBehaviorBandit : java.lang.Object
    {
        public static java.util.logging.Logger logger;
        public const float EPSILON = 0.2f;
        public const float OPTIMISTIC_INITIAL_VALUE = 1.0f;
        public static com.oddlabs.tt.player.AiBehaviorBandit instance;
        public java.util.Random random;
        public java.util.Map counts;
        public java.util.Map averages;
        // Java: 59
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public static com.oddlabs.tt.player.AiBehaviorBandit get()
        {
            if (com.oddlabs.tt.player.AiBehaviorBandit.instance == null)
            {
                com.oddlabs.tt.player.AiBehaviorBandit.instance = new com.oddlabs.tt.player.AiBehaviorBandit();
                com.oddlabs.tt.player.AiBehaviorBandit.instance.load();
            }
            return com.oddlabs.tt.player.AiBehaviorBandit.instance;
        }
        // Java: 71
        public com.oddlabs.tt.player.AiBehaviorProfile selectArm(com.oddlabs.tt.player.PlayerStyleContext context)
        {
            return selectArm(context, (-1));
        }
        // Java: 80
        public com.oddlabs.tt.player.AiBehaviorProfile selectArm(com.oddlabs.tt.player.PlayerStyleContext context, int difficulty)
        {
            com.oddlabs.tt.player.AiBehaviorProfile[] arms = com.oddlabs.tt.player.AiBehaviorProfile.values();
            if (com.oddlabs.tt.player.AiBehaviorBandit.forced_arm != null)
            {
                return com.oddlabs.tt.player.AiBehaviorBandit.forced_arm;
            }
            if (random.nextFloat() < com.oddlabs.tt.player.AiBehaviorBandit.EPSILON)
            {
                return arms[random.nextInt(arms.Length)];
            }
            bool tiered = false;
            foreach (com.oddlabs.tt.player.AiBehaviorProfile a in arms)
            {
                tiered |= counts.containsKey(com.oddlabs.tt.player.AiBehaviorBandit.key(context, difficulty, a));
            }
            int d = (tiered ? difficulty : (-1));
            com.oddlabs.tt.player.AiBehaviorProfile best = arms[0];
            float best_value = averageFor(context, d, best);
            for (int i = 1; i < arms.Length; i++)
            {
                float @value = averageFor(context, d, arms[i]);
                if (@value > best_value)
                {
                    best = arms[i];
                    best_value = @value;
                }
            }
            return best;
        }
        // Java: 105
        public void recordOutcome(com.oddlabs.tt.player.PlayerStyleContext context, com.oddlabs.tt.player.AiBehaviorProfile arm, float reward)
        {
            recordOutcome(context, (-1), arm, reward);
        }
        // Java: 110
        public void recordOutcome(com.oddlabs.tt.player.PlayerStyleContext context, int difficulty, com.oddlabs.tt.player.AiBehaviorProfile arm, float reward)
        {
            @record(com.oddlabs.tt.player.AiBehaviorBandit.key(context, (-1), arm), reward, 0.0f);
            if (difficulty >= 0)
            {
                @record(com.oddlabs.tt.player.AiBehaviorBandit.key(context, difficulty, arm), reward, com.oddlabs.tt.player.AiBehaviorBandit.MIN_STEP[java.lang.Math.clamp(difficulty, 0, com.oddlabs.tt.player.AiBehaviorBandit.MIN_STEP.Length - 1)]);
            }
            save();
        }
        public static float[] MIN_STEP;
        // Java: 123
        public void @record(string key_v, float reward, float min_step)
        {
            int count = ((int)((int?)counts.getOrDefault(key_v, 0))) + 1;
            float previous_average = ((float)((float?)averages.getOrDefault(key_v, com.oddlabs.tt.player.AiBehaviorBandit.OPTIMISTIC_INITIAL_VALUE)));
            float new_average = (float)(previous_average + (float)(((float)(reward - previous_average)) * java.lang.Math.max((float)(1.0f / count), min_step)));
            counts.put(key_v, count);
            averages.put(key_v, new_average);
        }
        // Java: 131
        public float averageFor(com.oddlabs.tt.player.PlayerStyleContext context, int difficulty, com.oddlabs.tt.player.AiBehaviorProfile arm)
        {
            return ((float)((float?)averages.getOrDefault(com.oddlabs.tt.player.AiBehaviorBandit.key(context, difficulty, arm), com.oddlabs.tt.player.AiBehaviorBandit.OPTIMISTIC_INITIAL_VALUE)));
        }
        // Java: 135
        public static string key(com.oddlabs.tt.player.PlayerStyleContext context, int difficulty, com.oddlabs.tt.player.AiBehaviorProfile arm)
        {
            return java.lang.JStr.valueOf(context.name()) + java.lang.JStr.valueOf(((difficulty >= 0 ? ".d" + (difficulty) : ""))) + "." + java.lang.JStr.valueOf(arm.name());
        }
        public static com.oddlabs.tt.player.AiBehaviorProfile forced_arm;
        public const int PRIOR_WEIGHT = 6;
        public const string PRIOR = "/com/oddlabs/tt/player/adaptive_prior.properties";
        // Java: 145
        public static void forceArm(com.oddlabs.tt.player.AiBehaviorProfile arm)
        {
            com.oddlabs.tt.player.AiBehaviorBandit.forced_arm = arm;
        }
        // Java: 150
        public void clear()
        {
            counts.clear();
            averages.clear();
        }
        // Java: 155
        public void exportTo(java.nio.file.Path path)
        {
            java.util.Properties props = new java.util.Properties();
            foreach (java.util.Map.Entry e in java.lang.JIter.of(counts.entrySet()))
            {
                props.setProperty(java.lang.JStr.valueOf(((string)e.getKey())) + ".count", java.lang.Integer.toString(((int)((int?)e.getValue()))));
            }
            foreach (java.util.Map.Entry e_2 in java.lang.JIter.of(averages.entrySet()))
            {
                props.setProperty(java.lang.JStr.valueOf(((string)e_2.getKey())) + ".average", java.lang.Float.toString(((float)((float?)e_2.getValue()))));
            }
            {
                java.io.OutputStream @out = java.nio.file.Files.newOutputStream(path);
                try
                {
                    props.store(@out, "trained headless (AIMatchRunner --train-adaptive)");
                }
                finally
                {
                    if (@out != null) @out.close();
                }
            }
        }
        // Java: 167
        public void loadPrior()
        {
            java.util.Properties props = new java.util.Properties();
            {
                java.io.InputStream @in = typeof(com.oddlabs.tt.player.AiBehaviorBandit).getResourceAsStream(com.oddlabs.tt.player.AiBehaviorBandit.PRIOR);
                try
                {
                    if (@in == null)
                    {
                        return;
                    }
                    props.load(@in);
                }
                catch (System.Exception e) when (e is System.IO.IOException || e is System.Exception)
                {
                    return;
                }
                finally
                {
                    if (@in != null) @in.close();
                }
            }
            foreach (string name in java.lang.JIter.of(props.stringPropertyNames()))
            {
                if (!name.endsWith(".count"))
                {
                    continue;
                }
                string key_v = name.substring(0, name.length() - ".count".length());
                string avg = props.getProperty(java.lang.JStr.valueOf(key_v) + ".average");
                if (avg == null)
                {
                    continue;
                }
                try
                {
                    counts.put(key_v, java.lang.Math.min(com.oddlabs.tt.player.AiBehaviorBandit.PRIOR_WEIGHT, java.lang.Integer.parseInt(props.getProperty(name).trim())));
                    averages.put(key_v, java.lang.Float.parseFloat(avg.trim()));
                }
                catch (System.FormatException e_2)
                {
                }
            }
        }
        // Java: 192
        public java.nio.file.Path file()
        {
            return ((com.oddlabs.tt.render.Renderer.getLocalInput() == null || com.oddlabs.tt.render.Renderer.getLocalInput().getGameDir() == null ? (java.nio.file.Path)(null) : com.oddlabs.tt.render.Renderer.getLocalInput().getGameDir().resolve(com.oddlabs.tt.global.Globals.getAiBehaviorBanditFileName())));
        }
        // Java: 196
        public void load()
        {
            loadPrior();
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
                    com.oddlabs.tt.player.AiBehaviorBandit.logger.log(java.util.logging.Level.WARNING, "Failed to read AI behavior bandit stats from " + java.lang.JStr.valueOf(file_v) + "; starting fresh.", e);
                    return;
                }
                finally
                {
                    if (@in != null) @in.close();
                }
            }
            foreach (com.oddlabs.tt.player.PlayerStyleContext context in com.oddlabs.tt.player.PlayerStyleContext.values())
            {
                for (int d = (-1); d <= com.oddlabs.tt.player.AdvancedAI.DIFFICULTY_INSANE; d++)
                {
                    foreach (com.oddlabs.tt.player.AiBehaviorProfile arm in com.oddlabs.tt.player.AiBehaviorProfile.values())
                    {
                        string key_v = com.oddlabs.tt.player.AiBehaviorBandit.key(context, d, arm);
                        string count_str = props.getProperty(java.lang.JStr.valueOf(key_v) + ".count");
                        string average_str = props.getProperty(java.lang.JStr.valueOf(key_v) + ".average");
                        if (count_str == null || average_str == null)
                        {
                            continue;
                        }
                        try
                        {
                            counts.put(key_v, java.lang.Integer.parseInt(count_str));
                            averages.put(key_v, java.lang.Float.parseFloat(average_str));
                        }
                        catch (System.FormatException e_2)
                        {
                            com.oddlabs.tt.player.AiBehaviorBandit.logger.log(java.util.logging.Level.WARNING, "Invalid bandit stats for " + java.lang.JStr.valueOf(key_v) + " in " + java.lang.JStr.valueOf(file_v) + "; skipping.", e_2);
                        }
                    }
                }
            }
        }
        // Java: 229
        public void save()
        {
            java.util.Properties props = new java.util.Properties();
            foreach (java.util.Map.Entry entry in java.lang.JIter.of(counts.entrySet()))
            {
                props.setProperty(java.lang.JStr.valueOf(((string)entry.getKey())) + ".count", java.lang.Integer.toString(((int)((int?)entry.getValue()))));
            }
            foreach (java.util.Map.Entry entry_2 in java.lang.JIter.of(averages.entrySet()))
            {
                props.setProperty(java.lang.JStr.valueOf(((string)entry_2.getKey())) + ".average", java.lang.Float.toString(((float)((float?)entry_2.getValue()))));
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
                    com.oddlabs.tt.player.AiBehaviorBandit.logger.log(java.util.logging.Level.WARNING, "Failed to write AI behavior bandit stats to " + java.lang.JStr.valueOf(file_v), e);
                }
                finally
                {
                    if (@out != null) @out.close();
                }
            }
        }
        static AiBehaviorBandit()
        {
            AiBehaviorBandit.logger = java.util.logging.Logger.getLogger(typeof(com.oddlabs.tt.player.AiBehaviorBandit).getName());
            AiBehaviorBandit.MIN_STEP = new float[] { 0.05f, 0.1f, 0.15f, 0.25f };
        }
        private void init_AiBehaviorBandit()
        {
            this.random = new java.util.Random();
            this.counts = new java.util.HashMap();
            this.averages = new java.util.HashMap();
        }
        public AiBehaviorBandit() : base()
        {
            init_AiBehaviorBandit();
        }
    }
}
