using UnityEngine;

namespace TribalTrouble.FaithfulGame
{
    /// <summary>
    /// Time of day (GameSettings.timeOfDay): Day keeps the original fixed afternoon sun, Night is a moonlit island,
    /// Cycle runs a 24 hour day where one real minute is one game hour. Purely visual: the simulation never sees it,
    /// so it can't affect multiplayer or recordings.
    /// The sun rises in the east at 6:00, peaks at 12:00 and sets at 18:00; at about 15:30 it stands exactly where the
    /// original game's sun is. At night a moon on the other side of the sky gives a dim blue light.
    /// </summary>
    public static class TTDayNight
    {
        public const int Day = 0, Night = 1, Cycle = 2;
        public const float NightHour = 23f;
        const float MaxElevation = 62f;            // degrees, at noon
        const float MoonElevation = 38f;
        public const float SecondsPerHour = 60f;    // one real minute per game hour

        static readonly Vector3 OriginalToSun = new Vector3(-0.9f, 0.7f, 0.7f).normalized;   // TTEnvironment.JavaSun in Unity axes
        static readonly float OriginalElevation = Mathf.Asin(OriginalToSun.y) * Mathf.Rad2Deg;
        static readonly float OriginalAzimuth = Mathf.Atan2(OriginalToSun.z, OriginalToSun.x) * Mathf.Rad2Deg;
        /// <summary>The afternoon hour at which the cycle's sun matches the original one.</summary>
        public static readonly float OriginalHour = 18f - Mathf.Asin(OriginalElevation / MaxElevation) * Mathf.Rad2Deg / 15f;

        public static float SunElevation(float hour) => MaxElevation * Mathf.Sin((hour - 6f) / 24f * 2f * Mathf.PI);

        static Vector3 Dir(float azimuthDeg, float elevationDeg)
        {
            float a = azimuthDeg * Mathf.Deg2Rad, e = elevationDeg * Mathf.Deg2Rad;
            return new Vector3(Mathf.Cos(e) * Mathf.Cos(a), Mathf.Sin(e), Mathf.Cos(e) * Mathf.Sin(a)).normalized;
        }

        /// <summary>Direction to the sun at this hour (its elevation is real, so it's below the horizon at night).</summary>
        public static Vector3 SunDir(float hour) => Dir(OriginalAzimuth + (hour - OriginalHour) * 15f, SunElevation(hour));

        /// <summary>Where the light comes from at this hour: the sun while it's up (kept a few degrees up), the moon after
        /// dusk - cheap, for moving the shadows every frame (Evaluate's lightDir).</summary>
        public static Vector3 LightDir(float hour)
        {
            hour = Mathf.Repeat(hour, 24f);
            float e = SunElevation(hour);
            float az = OriginalAzimuth + (hour - OriginalHour) * 15f;
            if (e > -2f) return Dir(az, Mathf.Max(e, 3f));
            return Dir(az + 180f, Mathf.Max(MoonElevation * Mathf.Clamp01(-e / 20f), 12f));
        }

        /// <summary>0 at night, 1 by day, a smooth dawn / dusk around the sun crossing the horizon.</summary>
        public static float DayWeight(float hour) => Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(-7f, 5f, SunElevation(hour)));

        public struct State
        {
            public Vector3 lightDir;             // the sun by day, the moon by night
            public TTAtmosphere.Lighting atm;    // modern lighting (sun colour is the moon's at night)
            public Color classicTint;            // classic look: colour filter applied to the whole picture
            public float classicExposure;        // classic look: post exposure (stops)
            public float modernExposure;         // modern look: extra exposure so the night stays readable
            public float dayWeight;
            public float fogScale;               // the original fog colour is scaled by this
        }

        static readonly Color MoonLight = new Color(0.30f, 0.38f, 0.62f);
        static readonly Color NightZenith = new Color(0.010f, 0.016f, 0.045f);
        static readonly Color NightHorizon = new Color(0.035f, 0.050f, 0.095f);
        static readonly Color NightAmbientSky = new Color(0.12f, 0.15f, 0.26f);
        static readonly Color NightAmbientGround = new Color(0.04f, 0.04f, 0.06f);

        /// <summary>The lighting at this hour. Compute() is a few hundred atmosphere samples: call it a few times a second at most.</summary>
        public static State Evaluate(float hour)
        {
            hour = Mathf.Repeat(hour, 24f);
            var s = new State();
            float e = SunElevation(hour), w = DayWeight(hour);
            s.dayWeight = w;
            float az = OriginalAzimuth + (hour - OriginalHour) * 15f;
            // the sun lights the scene while it's up (kept a few degrees up so the sky model stays lit), the moon after
            // dusk; both fade out around the moment the light swaps over, so shadows never jump
            Vector3 sunDir = Dir(az, Mathf.Max(e, 3f));
            var day = TTAtmosphere.Compute(sunDir);
            float sunFade = Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(-2f, 4f, e));
            float moonFade = Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(-2f, -9f, e));
            bool sunUp = e > -2f;
            float moonEl = Mathf.Max(MoonElevation * Mathf.Clamp01(-e / 20f), 12f);
            s.lightDir = sunUp ? sunDir : Dir(az + 180f, moonEl);
            var atm = new TTAtmosphere.Lighting
            {
                sun = sunUp ? day.sun * sunFade : MoonLight * moonFade,
                zenith = Color.Lerp(NightZenith, day.zenith, w),
                horizon = Color.Lerp(NightHorizon, day.horizon, w),
                ambientSky = Color.Lerp(NightAmbientSky, day.ambientSky, w),
                ambientGround = Color.Lerp(NightAmbientGround, day.ambientGround, w),
            };
            atm.fog = atm.horizon;
            s.atm = atm;
            // classic: the original picture graded towards a warm dawn / dusk and a blue night
            float warm = Mathf.Clamp01(1f - Mathf.Abs(e - 2f) / 12f);   // strongest just above the horizon
            var tint = Color.Lerp(new Color(0.58f, 0.68f, 1.0f), Color.white, w);
            tint = Color.Lerp(tint, new Color(1.0f, 0.80f, 0.64f), warm * 0.8f);
            s.classicTint = tint;
            s.classicExposure = Mathf.Lerp(-1.15f, 0f, w) - warm * 0.15f;
            s.modernExposure = Mathf.Lerp(0.55f, 0f, w);
            s.fogScale = Mathf.Lerp(0.18f, 1f, w);
            return s;
        }

        /// <summary>"14:00" style clock text.</summary>
        static int s_clockKey = -1;
        static string s_clock;

        public static string Clock(float hour)
        {
            hour = Mathf.Repeat(hour, 24f);
            int h = (int)hour, m = (int)((hour - h) * 60f);
            int key = h * 60 + m;
            if (key != s_clockKey) { s_clockKey = key; s_clock = $"{h:00}:{m:00}"; } // once a game minute, not every frame
            return s_clock;
        }
    }
}
