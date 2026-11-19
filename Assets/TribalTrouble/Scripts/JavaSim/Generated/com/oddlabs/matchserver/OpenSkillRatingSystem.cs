// Translated from OpenSkillRatingSystem.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchserver
{
    // Java: 16
    public class OpenSkillRatingSystem : java.lang.Object
    {
        public const double SCALING_FACTOR = 60.0d;
        public const double INITIAL_MU = 25.0d;
        public const double INITIAL_SIGMA = 8.333333333333334d;
        public static int INITIAL_DISPLAY_RATING;
        public const double PROVISIONAL_SIGMA_THRESHOLD = 2.777777777777778d;
        public static java.util.logging.Logger LOGGER;
        // Java: 25
        public static int displayRating(double mu, double sigma)
        {
            return unchecked((int)(java.lang.Math.round(com.oddlabs.matchserver.OpenSkillRatingSystem.SCALING_FACTOR * (mu - sigma))));
        }
        // Java: 29
        public static bool isProvisional(double sigma)
        {
            return sigma > com.oddlabs.matchserver.OpenSkillRatingSystem.PROVISIONAL_SIGMA_THRESHOLD;
        }
        // Java: 33
        public static void rateGame(java.util.List teams, int[] ranks)
        {
            if (teams.size() < 2)
            {
                throw new System.ArgumentException("Need at least two teams to rate a game");
            }
            if (teams.size() != ranks.Length)
            {
                throw new System.ArgumentException("Need exactly one rank per team");
            }
            double meanRank = 0;
            int n = 0;
            for (int i = 0; i < teams.size(); i++)
            {
                if (!((java.util.List)teams.get(i)).isEmpty())
                {
                    meanRank += ranks[i];
                    n++;
                }
            }
            if (n < 2)
            {
                return;
            }
            meanRank /= n;
            java.util.Map acc = new java.util.LinkedHashMap();
            for (int i_2 = 0; i_2 < teams.size(); i_2++)
            {
                foreach (string nick in java.lang.JIter.of(((java.util.List)teams.get(i_2))))
                {
                    com.oddlabs.matchmaking.OpenSkillRating r = com.oddlabs.matchserver.OpenSkillRatingSystem.loadRating(nick);
                    double mu = r.mu() + (meanRank - ranks[i_2]) * 1.0d;
                    double sigma = java.lang.Math.max(1.0d, r.sigma() * 0.95d);
                    double[] a = ((double[])acc.computeIfAbsent(nick, new java.util.function.Function_L((object k_l) =>
                    {
                        string k = (string)k_l;
                        return new double[3];
                    })));
                    a[0] += mu;
                    a[1] += sigma;
                    a[2]++;
                }
            }
            foreach (java.util.Map.Entry e in java.lang.JIter.of(acc.entrySet()))
            {
                double[] a_2 = ((double[])e.getValue());
                com.oddlabs.matchmaking.OpenSkillRating r_2 = new com.oddlabs.matchmaking.OpenSkillRating(((string)e.getKey()), a_2[0] / a_2[2], a_2[1] / a_2[2]);
                try
                {
                    com.oddlabs.matchserver.DBInterface.upsertOpenSkillRating(r_2);
                }
                catch (System.Exception ex)
                {
                    com.oddlabs.matchserver.OpenSkillRatingSystem.LOGGER.warning("OpenSkill stub: save failed " + java.lang.JStr.valueOf(ex));
                }
                com.oddlabs.matchserver.OpenSkillRatingSystem.LOGGER.info("OpenSkillRatingSystem(stub): " + java.lang.JStr.valueOf(r_2.nick()) + " -> mu=" + java.lang.JStr.valueOf(r_2.mu()) + " sigma=" + java.lang.JStr.valueOf(r_2.sigma()));
            }
        }
        // Java: 63
        public static com.oddlabs.matchmaking.OpenSkillRating loadRating(string nick)
        {
            try
            {
                com.oddlabs.matchmaking.OpenSkillRating rating = com.oddlabs.matchserver.DBInterface.getOpenSkillRating(nick);
                if (rating != null)
                {
                    return rating;
                }
            }
            catch (System.Exception e)
            {
            }
            return new com.oddlabs.matchmaking.OpenSkillRating(nick, com.oddlabs.matchserver.OpenSkillRatingSystem.INITIAL_MU, com.oddlabs.matchserver.OpenSkillRatingSystem.INITIAL_SIGMA);
        }
        static OpenSkillRatingSystem()
        {
            OpenSkillRatingSystem.INITIAL_DISPLAY_RATING = com.oddlabs.matchserver.OpenSkillRatingSystem.displayRating(com.oddlabs.matchserver.OpenSkillRatingSystem.INITIAL_MU, com.oddlabs.matchserver.OpenSkillRatingSystem.INITIAL_SIGMA);
            OpenSkillRatingSystem.LOGGER = com.oddlabs.matchserver.MatchmakingServer.getLogger();
        }
        public OpenSkillRatingSystem() : base()
        {
        }
    }
}
