// Translated from Weather.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.landscape
{
    // Java: 39
    public class Weather : java.lang.Object
    {
        public const int CALM = 0;
        public const int STORM = 1;
        public const int QUAKE = 2;
        public const int WILDFIRE = 3;
        public const int FLOOD = 4;
        public const int TORNADO = 5;
        public const int HAIL = 6;
        public const int BLIZZARD = 7;
        public const int HEATWAVE = 8;
        public const int FOG = 9;
        public const int METEORS = 10;
        public const int LAST_EVENT = 10;
        public const float EVENT_MIN = 30.0f;
        public const float EVENT_MAX = 180.0f;
        public const float FIRST_EVENT_MIN = 150.0f;
        public const float FIRST_EVENT_MAX = 240.0f;
        public const float CALM_MIN = 110.0f;
        public const float CALM_MAX = 230.0f;
        public const float STORM_STRIKE_MIN = 2.2f;
        public const float STORM_STRIKE_MAX = 5.5f;
        public const float BOLT_HEIGHT = 38.0f;
        public const float BOLT_UNIT_RADIUS = 2.2f;
        public const int BOLT_UNIT_DAMAGE = 3;
        public const float BOLT_BUILDING_RADIUS = 3.5f;
        public const int BOLT_BUILDING_DAMAGE = 14;
        public const float BOLT_SHELTER_CHANCE = 0.3f;
        public const float BOLT_IGNITE_CHANCE = 0.45f;
        public const float LIGHTNING_TIME = 0.1f;
        public static org.joml.Vector4fc DELTA_COLOR;
        public const float QUAKE_RADIUS = 48.0f;
        public const float QUAKE_PERIOD = 0.8f;
        public const int QUAKE_BUILDING_DAMAGE = 7;
        public const int MAX_FIRES = 48;
        public const float FIRE_SPREAD_PERIOD = 2.0f;
        public const float FIRE_SPREAD_RADIUS = 9.0f;
        public const float FIRE_SPREAD_CHANCE = 0.3f;
        public const float FIRE_HEAT_RADIUS = 4.5f;
        public const float FLOOD_RISE = 7.0f;
        public const float FLOOD_HOLD = 9.0f;
        public const float FLOOD_FALL = 9.0f;
        public const float TORNADO_SPEED = 3.6f;
        public const float TORNADO_UNIT_RADIUS = 4.5f;
        public const float TORNADO_BUILDING_RADIUS = 6.5f;
        public const float TORNADO_PERIOD = 0.5f;
        public com.oddlabs.tt.landscape.World world;
        public java.util.Random random;
        public int @event;
        public float event_time;
        public float event_length;
        public float calm_left;
        public int events;
        public float rain;
        public float darkness;
        public float wind;
        public float wind_x;
        public float wind_y;
        public float strike_timer;
        public java.util.List pending_bolts;
        public float quake_x;
        public float quake_y;
        public float quake_strength;
        public float quake_timer;
        public java.util.List fires;
        public java.util.List fire_state;
        public float blaze;
        public float flood_peak;
        public float flood_level;
        public float flood_dir_x;
        public float flood_dir_y;
        public float flood_tick;
        public float tornado_x;
        public float tornado_y;
        public float tornado_dx;
        public float tornado_dy;
        public float tornado_timer;
        public bool tornado_f;
        public float flood_rise;
        public float flood_hold;
        public float flood_fall;
        public float shock_time;
        public float shock_length;
        public float next_shock;
        public float shock_strength;
        public float hazard_timer;
        public java.util.List meteors_f;
        public float meteor_timer;
        public int units_hit;
        public int buildings_hit;
        public int trees_lost;
        public int sheltered_killed;
        public int meteors_fallen;
        // Java: 149
        public int getUnitsHit()
        {
            return units_hit;
        }
        // Java: 150
        public int getBuildingsHit()
        {
            return buildings_hit;
        }
        // Java: 151
        public int getTreesLost()
        {
            return trees_lost;
        }
        // Java: 152
        public int getShelteredKilled()
        {
            return sheltered_killed;
        }
        // Java: 153
        public int getMeteorsFallen()
        {
            return meteors_fallen;
        }
        public bool natural;
        public float focus_x;
        public float focus_y;
        public const float FOCUS_SPREAD = 18.0f;
        public const float FOCUS_TARGETS = 25.0f;
        // Java: 164
        public static com.oddlabs.tt.landscape.Weather summonedOnly(com.oddlabs.tt.landscape.World world)
        {
            com.oddlabs.tt.landscape.Weather w = new com.oddlabs.tt.landscape.Weather(world);
            w.natural = false;
            w.calm_left = java.lang.Float.MAX_VALUE;
            return w;
        }
        // Java: 172
        public void summon(int kind, float x, float y)
        {
            forceEvent(kind);
            focus_x = x;
            focus_y = y;
            if (kind == com.oddlabs.tt.landscape.Weather.QUAKE)
            {
                quake_x = x;
                quake_y = y;
            }
        }
        // Java: 182
        public bool isFocused()
        {
            return !java.lang.Float.isNaN(focus_x);
        }
        // Java: 183
        public float getFocusX()
        {
            return focus_x;
        }
        // Java: 184
        public float getFocusY()
        {
            return focus_y;
        }
        // Java: 187
        public static void bolt(com.oddlabs.tt.landscape.World world, float x, float y)
        {
            com.oddlabs.tt.model.RacesResources rr = world.getRacesResources();
            if (rr == null)
            {
                return;
            }
            float z = world.getHeightMap().getNearestHeight(x, y);
            new com.oddlabs.tt.particle.Lightning(world, new org.joml.Vector3f((float)(x + 3.0f), (float)(y - 2.0f), (float)(z + com.oddlabs.tt.landscape.Weather.BOLT_HEIGHT)), new org.joml.Vector3f(x, y, z), 0.5f, 15, com.oddlabs.util.Color.WHITE, com.oddlabs.tt.landscape.Weather.DELTA_COLOR, rr.getLightningTexture(), com.oddlabs.tt.landscape.Weather.LIGHTNING_TIME, world.getAnimationManagerGameTime());
            world.getAudio().newAudio(new com.oddlabs.tt.audio.AudioParameters(rr.getLightningSound(), x, y, z, com.oddlabs.tt.audio.AudioPlayer.AUDIO_RANK_MAGIC, com.oddlabs.tt.audio.AudioPlayer.AUDIO_DISTANCE_MAGIC, com.oddlabs.tt.audio.AudioPlayer.AUDIO_GAIN_LIGHTNING, com.oddlabs.tt.audio.AudioPlayer.AUDIO_RADIUS_LIGHTNING));
        }
        // Java: 209
        public int getEvent()
        {
            return @event;
        }
        // Java: 210
        public float getEventTime()
        {
            return event_time;
        }
        // Java: 211
        public float getEventLength()
        {
            return event_length;
        }
        // Java: 212
        public float getCalmLeft()
        {
            return (@event == com.oddlabs.tt.landscape.Weather.CALM ? calm_left : 0.0f);
        }
        // Java: 213
        public int getEventCount()
        {
            return events;
        }
        // Java: 214
        public float getRain()
        {
            return rain;
        }
        // Java: 215
        public float getDarkness()
        {
            return darkness;
        }
        // Java: 216
        public float getWind()
        {
            return wind;
        }
        // Java: 217
        public float getWindX()
        {
            return wind_x;
        }
        // Java: 218
        public float getWindY()
        {
            return wind_y;
        }
        // Java: 219
        public float getQuakeX()
        {
            return quake_x;
        }
        // Java: 220
        public float getQuakeY()
        {
            return quake_y;
        }
        // Java: 221
        public float getQuakeStrength()
        {
            return (@event == com.oddlabs.tt.landscape.Weather.QUAKE ? (float)(shock_strength * quakeEnvelope()) : 0.0f);
        }
        // Java: 222
        public java.util.List getFires()
        {
            return fires;
        }
        // Java: 223
        public float getFloodLevel()
        {
            return flood_level;
        }
        // Java: 224
        public float getFloodDirX()
        {
            return flood_dir_x;
        }
        // Java: 225
        public float getFloodDirY()
        {
            return flood_dir_y;
        }
        // Java: 226
        public bool hasTornado()
        {
            return tornado_f;
        }
        // Java: 227
        public float getTornadoX()
        {
            return tornado_x;
        }
        // Java: 228
        public float getTornadoY()
        {
            return tornado_y;
        }
        // Java: 229
        public float getTornadoStrength()
        {
            return (tornado_f ? java.lang.Math.min(1.0f, java.lang.Math.min((float)(event_time / 4.0f), (float)(((float)(event_length - event_time)) / 4.0f))) : 0.0f);
        }
        // Java: 232
        public float getIntensity()
        {
            return (@event >= com.oddlabs.tt.landscape.Weather.HAIL ? envelope() : (@event == com.oddlabs.tt.landscape.Weather.STORM ? stormEnvelope() : 0.0f));
        }
        // Java: 237
        public java.util.List getMeteors()
        {
            return meteors_f;
        }
        // Java: 240
        public float getAccuracyFactor()
        {
            float k = (@event >= com.oddlabs.tt.landscape.Weather.HAIL ? envelope() : 0.0f);
            float f = (float)((float)((float)((float)(1.0f - (float)(0.22f * wind)) - ((@event == com.oddlabs.tt.landscape.Weather.FOG ? (float)(0.35f * k) : 0.0f))) - ((@event == com.oddlabs.tt.landscape.Weather.HAIL ? (float)(0.12f * k) : 0.0f))) - ((@event == com.oddlabs.tt.landscape.Weather.BLIZZARD ? (float)(0.15f * k) : 0.0f)));
            return java.lang.Math.max(0.4f, f);
        }
        // Java: 248
        public float getSpeedFactor()
        {
            if (@event == com.oddlabs.tt.landscape.Weather.BLIZZARD)
            {
                return (float)(1.0f - (float)(0.3f * envelope()));
            }
            if (@event == com.oddlabs.tt.landscape.Weather.HEATWAVE)
            {
                return (float)(1.0f - (float)(0.15f * envelope()));
            }
            return 1.0f;
        }
        // Java: 257
        public float getHarvestFactor()
        {
            if (@event == com.oddlabs.tt.landscape.Weather.BLIZZARD)
            {
                return (float)(1.0f - (float)(0.25f * envelope()));
            }
            if (@event == com.oddlabs.tt.landscape.Weather.HEATWAVE)
            {
                return (float)(1.0f - (float)(0.15f * envelope()));
            }
            return 1.0f;
        }
        // Java: 267
        public void animate(float t)
        {
            if (t <= 0.0f)
            {
                return;
            }
            float k = (@event >= com.oddlabs.tt.landscape.Weather.HAIL ? envelope() : 0.0f);
            float want_rain = (@event == com.oddlabs.tt.landscape.Weather.STORM ? stormEnvelope() : (@event == com.oddlabs.tt.landscape.Weather.HAIL ? (float)(0.6f * k) : 0.0f));
            float want_dark = (@event == com.oddlabs.tt.landscape.Weather.STORM ? (float)(0.85f * stormEnvelope()) : (@event == com.oddlabs.tt.landscape.Weather.TORNADO ? (float)(0.55f * getTornadoStrength()) : (@event == com.oddlabs.tt.landscape.Weather.HAIL ? (float)(0.6f * k) : (@event == com.oddlabs.tt.landscape.Weather.BLIZZARD ? (float)(0.5f * k) : (@event == com.oddlabs.tt.landscape.Weather.FOG ? (float)(0.3f * k) : (@event == com.oddlabs.tt.landscape.Weather.METEORS ? (float)(0.35f * k) : 0.0f))))));
            float want_wind = (@event == com.oddlabs.tt.landscape.Weather.STORM ? (float)(0.45f * stormEnvelope()) : (@event == com.oddlabs.tt.landscape.Weather.TORNADO ? getTornadoStrength() : (@event == com.oddlabs.tt.landscape.Weather.WILDFIRE ? 0.3f : (@event == com.oddlabs.tt.landscape.Weather.BLIZZARD ? (float)(0.8f * k) : (@event == com.oddlabs.tt.landscape.Weather.HAIL ? (float)(0.4f * k) : (@event == com.oddlabs.tt.landscape.Weather.HEATWAVE ? (float)(0.15f * k) : 0.0f))))));
            rain = com.oddlabs.tt.landscape.Weather.ease(rain, want_rain, (float)(t * 0.25f));
            darkness = com.oddlabs.tt.landscape.Weather.ease(darkness, want_dark, (float)(t * 0.2f));
            wind = com.oddlabs.tt.landscape.Weather.ease(wind, want_wind, (float)(t * 0.3f));
            if (@event == com.oddlabs.tt.landscape.Weather.CALM)
            {
                calm_left -= t;
                if (calm_left <= 0.0f)
                {
                    startEvent();
                }
            }
            else
            {
                event_time += t;
                switch ((@event))
                {
                    case 1:
                    {
                        storm(t);
                        break;
                    }
                    case 2:
                    {
                        quake(t);
                        break;
                    }
                    case 4:
                    {
                        flood(t);
                        break;
                    }
                    case 5:
                    {
                        tornado(t);
                        break;
                    }
                    case 6:
                    {
                        hail(t);
                        break;
                    }
                    case 7:
                    {
                        blizzard(t);
                        break;
                    }
                    case 8:
                    {
                        heatwave(t);
                        break;
                    }
                    case 10:
                    {
                        meteorShower(t);
                        break;
                    }
                    default:
                    {
                        break;
                    }
                }
                if ((@event == com.oddlabs.tt.landscape.Weather.WILDFIRE ? (fires.isEmpty() && event_time >= com.oddlabs.tt.landscape.Weather.EVENT_MIN) || event_time >= (float)(event_length * 1.5f) : event_time >= event_length))
                {
                    endEvent();
                }
            }
            bolts(t);
            burn(t);
            meteors(t);
        }
        // Java: 308
        public static float ease(float v, float target, float step)
        {
            return (v < target ? java.lang.Math.min(target, (float)(v + step)) : java.lang.Math.max(target, (float)(v - step)));
        }
        // Java: 312
        public float stormEnvelope()
        {
            float @in = java.lang.Math.min(1.0f, (float)(event_time / 10.0f));
            float @out = java.lang.Math.min(1.0f, java.lang.Math.max(0.0f, (float)(((float)(event_length - event_time)) / 10.0f)));
            return java.lang.Math.min(@in, @out);
        }
        // Java: 319
        public float quakeEnvelope()
        {
            float x = (shock_length > 0.0f ? (float)(shock_time / shock_length) : 1.0f);
            return (x < 0.15f ? (float)(x / 0.15f) : (x > 0.7f ? java.lang.Math.max(0.0f, (float)(((float)(1.0f - x)) / 0.3f)) : 1.0f));
        }
        // Java: 325
        public float envelope()
        {
            float @in = java.lang.Math.min(1.0f, (float)(event_time / 12.0f));
            float @out = java.lang.Math.min(1.0f, java.lang.Math.max(0.0f, (float)(((float)(event_length - event_time)) / 12.0f)));
            return java.lang.Math.min(@in, @out);
        }
        // Java: 332
        public float randomLength()
        {
            return (float)(com.oddlabs.tt.landscape.Weather.EVENT_MIN + (float)(random.nextFloat() * ((float)(com.oddlabs.tt.landscape.Weather.EVENT_MAX - com.oddlabs.tt.landscape.Weather.EVENT_MIN))));
        }
        // Java: 336
        public void startEvent()
        {
            bool north = ((object)world.getTerrainType() == (object)com.oddlabs.tt.procedural.Landscape.TerrainType.VIKING);
            int[] weights = new int[] { 22, 12, 12, (hasSea() ? 12 : 0), 14, 12, (north ? 16 : 4), (north ? 4 : 14), 12, 8 };
            int sum = 0;
            foreach (int w in weights)
            {
                sum += w;
            }
            int roll = random.nextInt(sum);
            int pick = 0;
            while (roll >= weights[pick])
            {
                roll -= weights[pick];
                pick++;
            }
            startEvent(pick + 1);
        }
        // Java: 353
        public void forceEvent(int kind)
        {
            if (kind < com.oddlabs.tt.landscape.Weather.STORM || kind > com.oddlabs.tt.landscape.Weather.LAST_EVENT)
            {
                return;
            }
            if (@event != com.oddlabs.tt.landscape.Weather.CALM)
            {
                endEvent();
            }
            startEvent(kind);
        }
        // Java: 361
        public void startEvent(int kind)
        {
            events++;
            event_time = 0.0f;
            @event = kind;
            switch ((@event))
            {
                case 1:
                {
                    event_length = randomLength();
                    strike_timer = 6.0f;
                    float a = (float)(random.nextFloat() * 6.2831855f);
                    wind_x = ((float)(java.lang.StrictMath.cos(a)));
                    wind_y = ((float)(java.lang.StrictMath.sin(a)));
                    break;
                }
                case 2:
                {
                    event_length = randomLength();
                    float[] p = randomLand();
                    quake_x = p[0];
                    quake_y = p[1];
                    quake_strength = (float)(0.55f + (float)(random.nextFloat() * 0.45f));
                    quake_timer = 0.0f;
                    shock_time = 0.0f;
                    shock_length = (float)(8.0f + (float)(random.nextFloat() * 5.0f));
                    shock_strength = quake_strength;
                    next_shock = (float)((float)(shock_length + 6.0f) + (float)(random.nextFloat() * 14.0f));
                    break;
                }
                case 3:
                {
                    event_length = randomLength();
                    int sparks = 1 + random.nextInt(3);
                    for (int i = 0; i < sparks; i++)
                    {
                        float[] p_2 = randomLand();
                        com.oddlabs.tt.landscape.TreeSupply tree = nearestTree(p_2[0], p_2[1], 40.0f);
                        if (tree != null)
                        {
                            ignite(tree);
                        }
                    }
                    if (fires.isEmpty())
                    {
                        @event = com.oddlabs.tt.landscape.Weather.CALM;
                    }
                    break;
                }
                case 4:
                {
                    event_length = randomLength();
                    flood_rise = (float)(event_length * 0.2f);
                    flood_hold = (float)(event_length * 0.55f);
                    flood_fall = (float)((float)(event_length - flood_rise) - flood_hold);
                    flood_peak = (float)(2.2f + (float)(random.nextFloat() * 1.4f));
                    float a_2 = (float)(random.nextFloat() * 6.2831855f);
                    flood_dir_x = ((float)(java.lang.StrictMath.cos(a_2)));
                    flood_dir_y = ((float)(java.lang.StrictMath.sin(a_2)));
                    flood_tick = 0.0f;
                    break;
                }
                case 5:
                {
                    event_length = randomLength();
                    float[] p_3 = randomLand();
                    tornado_x = p_3[0];
                    tornado_y = p_3[1];
                    float a_3 = (float)(random.nextFloat() * 6.2831855f);
                    tornado_dx = ((float)(java.lang.StrictMath.cos(a_3)));
                    tornado_dy = ((float)(java.lang.StrictMath.sin(a_3)));
                    wind_x = tornado_dx;
                    wind_y = tornado_dy;
                    tornado_timer = 0.0f;
                    tornado_f = true;
                    break;
                }
                case 6:
                case 7:
                case 8:
                case 9:
                {
                    event_length = randomLength();
                    hazard_timer = 0.0f;
                    float a_4 = (float)(random.nextFloat() * 6.2831855f);
                    wind_x = ((float)(java.lang.StrictMath.cos(a_4)));
                    wind_y = ((float)(java.lang.StrictMath.sin(a_4)));
                    break;
                }
                case 10:
                {
                    event_length = randomLength();
                    meteor_timer = 4.0f;
                    break;
                }
                default:
                {
                    break;
                }
            }
            if (@event == com.oddlabs.tt.landscape.Weather.CALM)
            {
                calm_left = 30.0f;
            }
        }
        // Java: 439
        public void endEvent()
        {
            @event = com.oddlabs.tt.landscape.Weather.CALM;
            tornado_f = false;
            flood_level = 0.0f;
            quake_strength = 0.0f;
            shock_strength = 0.0f;
            focus_x = java.lang.Float.NaN;
            focus_y = java.lang.Float.NaN;
            calm_left = (natural ? (float)(com.oddlabs.tt.landscape.Weather.CALM_MIN + (float)(random.nextFloat() * ((float)(com.oddlabs.tt.landscape.Weather.CALM_MAX - com.oddlabs.tt.landscape.Weather.CALM_MIN)))) : java.lang.Float.MAX_VALUE);
        }
        // Java: 452
        public void storm(float t)
        {
            strike_timer -= t;
            if (strike_timer > 0.0f)
            {
                return;
            }
            strike_timer = (float)(com.oddlabs.tt.landscape.Weather.STORM_STRIKE_MIN + (float)(random.nextFloat() * ((float)(com.oddlabs.tt.landscape.Weather.STORM_STRIKE_MAX - com.oddlabs.tt.landscape.Weather.STORM_STRIKE_MIN))));
            float roll = random.nextFloat();
            float x = default;
            float y = default;
            if (roll < 0.5f)
            {
                com.oddlabs.tt.model.Selectable s = randomTarget();
                if (s == null)
                {
                    return;
                }
                x = s.getPositionX();
                y = s.getPositionY();
            }
            else if (roll < 0.8f)
            {
                float[] p = randomLand();
                com.oddlabs.tt.landscape.TreeSupply tree = nearestTree(p[0], p[1], 20.0f);
                if (tree != null)
                {
                    x = tree.getPositionX();
                    y = tree.getPositionY();
                }
                else
                {
                    x = p[0];
                    y = p[1];
                }
            }
            else
            {
                float[] p_2 = randomLand();
                x = p_2[0];
                y = p_2[1];
            }
            strike(x, y);
        }
        // Java: 485
        public void strike(float x, float y)
        {
            float z = world.getHeightMap().getNearestHeight(x, y);
            float sx = (float)(x + (float)(((float)(random.nextFloat() - 0.5f)) * 14.0f));
            float sy = (float)(y + (float)(((float)(random.nextFloat() - 0.5f)) * 14.0f));
            pending_bolts.add(new float[] { 0.0f, x, y, z, sx, sy });
            pending_bolts.add(new float[] { 0.09f, x, y, z, sx, sy });
            pending_bolts.add(new float[] { 0.21f, x, y, z, sx, sy });
            com.oddlabs.tt.model.RacesResources rr = world.getRacesResources();
            if (rr != null)
            {
                world.getAudio().newAudio(new com.oddlabs.tt.audio.AudioParameters(rr.getLightningSound(), x, y, z, com.oddlabs.tt.audio.AudioPlayer.AUDIO_RANK_MAGIC, (float)(com.oddlabs.tt.audio.AudioPlayer.AUDIO_DISTANCE_MAGIC * 2.0f), com.oddlabs.tt.audio.AudioPlayer.AUDIO_GAIN_LIGHTNING, (float)(com.oddlabs.tt.audio.AudioPlayer.AUDIO_RADIUS_LIGHTNING * 2.0f)));
            }
            com.oddlabs.tt.model.Unit arc_from = null;
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(near(x, y, java.lang.Math.max(com.oddlabs.tt.landscape.Weather.BOLT_UNIT_RADIUS, com.oddlabs.tt.landscape.Weather.BOLT_BUILDING_RADIUS))))
            {
                float dx = (float)(s.getPositionX() - x);
                float dy = (float)(s.getPositionY() - y);
                float d2 = (float)((float)(dx * dx) + (float)(dy * dy));
                if ((s is com.oddlabs.tt.model.LandBuilding b))
                {
                    if (d2 < (float)(com.oddlabs.tt.landscape.Weather.BOLT_BUILDING_RADIUS * com.oddlabs.tt.landscape.Weather.BOLT_BUILDING_RADIUS) && !b.isDead())
                    {
                        sheltered_killed += com.oddlabs.tt.model.ExtendedCombat.killOccupants(b, null, com.oddlabs.tt.landscape.Weather.BOLT_SHELTER_CHANCE, 1);
                        if (!b.isDead())
                        {
                            hurt(b, com.oddlabs.tt.landscape.Weather.BOLT_BUILDING_DAMAGE, 0.0f, 1.0f);
                        }
                    }
                }
                else if ((s is com.oddlabs.tt.model.Unit u) && d2 < (float)(com.oddlabs.tt.landscape.Weather.BOLT_UNIT_RADIUS * com.oddlabs.tt.landscape.Weather.BOLT_UNIT_RADIUS) && !u.isDead())
                {
                    hurt(u, com.oddlabs.tt.landscape.Weather.BOLT_UNIT_DAMAGE, 0.0f, 1.0f);
                    arc_from = (arc_from == null ? u : arc_from);
                }
            }
            if (arc_from != null)
            {
                com.oddlabs.tt.landscape.HeightMap hm = world.getHeightMap();
                com.oddlabs.tt.model.RacesResources res = world.getRacesResources();
                foreach (com.oddlabs.tt.model.Unit[] arc in java.lang.JIter.of(com.oddlabs.tt.model.ExtendedCombat.chainLightning(world, null, arc_from, com.oddlabs.tt.landscape.Weather.BOLT_UNIT_DAMAGE, 3)))
                {
                    units_hit++;
                    if (res != null)
                    {
                        new com.oddlabs.tt.particle.Lightning(world, new org.joml.Vector3f(arc[0].getPositionX(), arc[0].getPositionY(), (float)(hm.getNearestHeight(arc[0].getPositionX(), arc[0].getPositionY()) + 1.2f)), new org.joml.Vector3f(arc[1].getPositionX(), arc[1].getPositionY(), (float)(hm.getNearestHeight(arc[1].getPositionX(), arc[1].getPositionY()) + 0.8f)), 0.2f, 6, com.oddlabs.util.Color.WHITE, com.oddlabs.tt.landscape.Weather.DELTA_COLOR, res.getLightningTexture(), com.oddlabs.tt.landscape.Weather.LIGHTNING_TIME, world.getAnimationManagerGameTime());
                    }
                }
            }
            com.oddlabs.tt.landscape.TreeSupply tree = nearestTree(x, y, 2.5f);
            if (tree != null && random.nextFloat() < (float)(com.oddlabs.tt.landscape.Weather.BOLT_IGNITE_CHANCE * ((float)(1.0f - (float)(0.6f * rain)))))
            {
                ignite(tree);
            }
        }
        // Java: 532
        public void bolts(float t)
        {
            if (pending_bolts.isEmpty())
            {
                return;
            }
            com.oddlabs.tt.model.RacesResources rr = world.getRacesResources();
            for (int i = 0; i < pending_bolts.size(); i++)
            {
                float[] b = ((float[])pending_bolts.get(i));
                b[0] -= t;
                if (b[0] > 0.0f)
                {
                    continue;
                }
                pending_bolts.remove(i--);
                if (rr == null)
                {
                    continue;
                }
                new com.oddlabs.tt.particle.Lightning(world, new org.joml.Vector3f(b[4], b[5], (float)(b[3] + com.oddlabs.tt.landscape.Weather.BOLT_HEIGHT)), new org.joml.Vector3f(b[1], b[2], b[3]), 0.5f, 15, com.oddlabs.util.Color.WHITE, com.oddlabs.tt.landscape.Weather.DELTA_COLOR, rr.getLightningTexture(), com.oddlabs.tt.landscape.Weather.LIGHTNING_TIME, world.getAnimationManagerGameTime());
            }
        }
        // Java: 552
        public void quake(float t)
        {
            shock_time += t;
            if (event_time >= next_shock && event_time < (float)(event_length - 6.0f))
            {
                shock_time = 0.0f;
                shock_length = (float)(5.0f + (float)(random.nextFloat() * 4.0f));
                shock_strength = (float)(quake_strength * ((float)(0.45f + (float)(random.nextFloat() * 0.35f))));
                quake_x += (float)(((float)(random.nextFloat() - 0.5f)) * 50.0f);
                quake_y += (float)(((float)(random.nextFloat() - 0.5f)) * 50.0f);
                float max = (float)(world.getHeightMap().getMetersPerWorld() - 10.0f);
                quake_x = java.lang.Math.max(10.0f, java.lang.Math.min(max, quake_x));
                quake_y = java.lang.Math.max(10.0f, java.lang.Math.min(max, quake_y));
                next_shock = (float)((float)((float)(event_time + shock_length) + 8.0f) + (float)(random.nextFloat() * 16.0f));
            }
            quake_timer += t;
            if (quake_timer < com.oddlabs.tt.landscape.Weather.QUAKE_PERIOD)
            {
                return;
            }
            quake_timer -= com.oddlabs.tt.landscape.Weather.QUAKE_PERIOD;
            float strength = (float)(shock_strength * quakeEnvelope());
            if (strength <= 0.05f)
            {
                return;
            }
            float r = (float)(com.oddlabs.tt.landscape.Weather.QUAKE_RADIUS * ((float)(0.6f + (float)(0.4f * strength))));
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(near(quake_x, quake_y, r)))
            {
                float dx = (float)(s.getPositionX() - quake_x);
                float dy = (float)(s.getPositionY() - quake_y);
                float k = (float)(1.0f - (float)(((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy))))) / r));
                if (k <= 0.0f || s.isDead())
                {
                    continue;
                }
                if ((s is com.oddlabs.tt.model.LandBuilding b))
                {
                    int damage = java.lang.Math.round((float)((float)(com.oddlabs.tt.landscape.Weather.QUAKE_BUILDING_DAMAGE * strength) * k));
                    if (damage > 0)
                    {
                        hurt(b, damage, 0.0f, 1.0f);
                    }
                    if (!b.isDead() && random.nextFloat() < (float)((float)(0.05f * strength) * k))
                    {
                        sheltered_killed += com.oddlabs.tt.model.ExtendedCombat.killOccupants(b, null, 1.0f, 1);
                    }
                }
                else if ((s is com.oddlabs.tt.model.Unit u) && !u.isMounted())
                {
                    float roll = random.nextFloat();
                    if (roll < (float)((float)(0.12f * strength) * k))
                    {
                        u.stun(1.2f);
                    }
                    else if (roll < (float)((float)(0.17f * strength) * k))
                    {
                        hurt(u, 1, 0.0f, 1.0f);
                    }
                }
            }
            if (random.nextFloat() < (float)(0.35f * strength))
            {
                com.oddlabs.tt.landscape.TreeSupply tree = nearestTree((float)(quake_x + (float)(((float)(random.nextFloat() - 0.5f)) * r)), (float)(quake_y + (float)(((float)(random.nextFloat() - 0.5f)) * r)), 6.0f);
                if (tree != null)
                {
                    tree.destroyByNature();
                    trees_lost++;
                }
            }
        }
        // Java: 603
        public void ignite(com.oddlabs.tt.landscape.TreeSupply tree)
        {
            if (tree.isDead() || fires.contains(tree) || fires.size() >= com.oddlabs.tt.landscape.Weather.MAX_FIRES)
            {
                return;
            }
            if (fires.isEmpty())
            {
                blaze = 0.0f;
            }
            fires.add(tree);
            fire_state.add(new float[] { (float)(11.0f + (float)(random.nextFloat() * 9.0f)), (float)(com.oddlabs.tt.landscape.Weather.FIRE_SPREAD_PERIOD * random.nextFloat()), 0.0f });
        }
        // Java: 612
        public bool isBurning(com.oddlabs.tt.landscape.TreeSupply tree)
        {
            return fires.contains(tree);
        }
        // Java: 616
        public void burn(float t)
        {
            if (fires.isEmpty())
            {
                return;
            }
            blaze += t;
            int n = fires.size();
            for (int i = 0; i < n && i < fires.size(); i++)
            {
                com.oddlabs.tt.landscape.TreeSupply tree = ((com.oddlabs.tt.landscape.TreeSupply)fires.get(i));
                float[] st = ((float[])fire_state.get(i));
                st[0] -= (float)(t * ((float)(1.0f + (float)(2.0f * rain))));
                if (st[0] <= 0.0f || tree.isDead())
                {
                    if (!tree.isDead())
                    {
                        tree.destroyByNature();
                        trees_lost++;
                    }
                    fires.remove(i);
                    fire_state.remove(i);
                    i--;
                    n--;
                    continue;
                }
                st[1] -= t;
                bool spreading = (@event == com.oddlabs.tt.landscape.Weather.WILDFIRE ? event_time < java.lang.Math.max(20.0f, (float)(event_length - 15.0f)) : blaze < 60.0f);
                if (st[1] <= 0.0f && spreading)
                {
                    st[1] += com.oddlabs.tt.landscape.Weather.FIRE_SPREAD_PERIOD;
                    foreach (com.oddlabs.tt.landscape.TreeSupply other in java.lang.JIter.of(trees(tree.getPositionX(), tree.getPositionY(), com.oddlabs.tt.landscape.Weather.FIRE_SPREAD_RADIUS)))
                    {
                        if (((object)other == (object)tree) || other.isDead() || fires.contains(other))
                        {
                            continue;
                        }
                        float dx = (float)(other.getPositionX() - tree.getPositionX());
                        float dy = (float)(other.getPositionY() - tree.getPositionY());
                        float len = ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy)))));
                        float downwind = (len > 0.0f ? (float)(((float)((float)(dx * wind_x) + (float)(dy * wind_y))) / len) : 0.0f);
                        float chance = (float)((float)(com.oddlabs.tt.landscape.Weather.FIRE_SPREAD_CHANCE * ((float)(1.0f + (float)((float)(wind * 1.5f) * downwind)))) * ((float)(1.0f - (float)(0.8f * rain))));
                        if (random.nextFloat() < chance)
                        {
                            ignite(other);
                        }
                    }
                }
                st[2] -= t;
                if (st[2] <= 0.0f)
                {
                    st[2] += 1.5f;
                    foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(near(tree.getPositionX(), tree.getPositionY(), com.oddlabs.tt.landscape.Weather.FIRE_HEAT_RADIUS)))
                    {
                        if (s.isDead())
                        {
                            continue;
                        }
                        if ((s is com.oddlabs.tt.model.LandBuilding b))
                        {
                            b.afflictions().burn(4.0f, 2, b.getOwner());
                        }
                        else if ((s is com.oddlabs.tt.model.Unit u) && !u.isMounted())
                        {
                            u.afflictions().burn(3.0f, 1, u.getOwner());
                        }
                    }
                }
            }
        }
        // Java: 668
        public bool hasSea()
        {
            return world.getHeightMap().getSeaLevelMeters() > 0.0f;
        }
        // Java: 672
        public void flood(float t)
        {
            float x = event_time;
            float level = default;
            if (x < flood_rise)
            {
                level = ((float)(java.lang.StrictMath.sin((float)(x / flood_rise) * java.lang.Math.PI / 2)));
            }
            else if (x < (float)(flood_rise + flood_hold))
            {
                level = 1.0f;
            }
            else
            {
                level = java.lang.Math.max(0.0f, (float)(1.0f - (float)(((float)((float)(x - flood_rise) - flood_hold)) / flood_fall)));
            }
            flood_level = (float)(flood_peak * level);
            flood_tick += t;
            if (flood_tick < 1.0f)
            {
                return;
            }
            flood_tick -= 1.0f;
            float sea = world.getHeightMap().getSeaLevelMeters();
            float water = (float)(sea + flood_level);
            foreach (com.oddlabs.tt.player.Player p in world.getPlayers())
            {
                foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(new java.util.ArrayList(p.getUnits().getSet())))
                {
                    if (s.isDead())
                    {
                        continue;
                    }
                    float ground = world.getHeightMap().getNearestHeight(s.getPositionX(), s.getPositionY());
                    float depth = (float)(water - ground);
                    if (depth <= 0.25f || ground < (float)(sea - 0.5f))
                    {
                        continue;
                    }
                    if ((s is com.oddlabs.tt.model.Unit u) && !u.isMounted())
                    {
                        hurt(u, (depth > 1.0f ? 2 : 1), flood_dir_x, flood_dir_y);
                        if (!u.isDead() && depth > 0.8f && random.nextFloat() < 0.25f)
                        {
                            u.stun(1.0f);
                        }
                    }
                    else if ((s is com.oddlabs.tt.model.LandBuilding b))
                    {
                        hurt(b, (depth > 1.0f ? 5 : 2), 0.0f, 1.0f);
                    }
                }
            }
        }
        // Java: 709
        public void tornado(float t)
        {
            float turn = (float)((float)(((float)(random.nextFloat() - 0.5f)) * 1.2f) * t);
            float c = ((float)(java.lang.StrictMath.cos(turn)));
            float s = ((float)(java.lang.StrictMath.sin(turn)));
            float ndx = (float)((float)(tornado_dx * c) - (float)(tornado_dy * s));
            float ndy = (float)((float)(tornado_dx * s) + (float)(tornado_dy * c));
            tornado_dx = ndx;
            tornado_dy = ndy;
            float nx = (float)(tornado_x + (float)((float)(tornado_dx * com.oddlabs.tt.landscape.Weather.TORNADO_SPEED) * t));
            float ny = (float)(tornado_y + (float)((float)(tornado_dy * com.oddlabs.tt.landscape.Weather.TORNADO_SPEED) * t));
            com.oddlabs.tt.landscape.HeightMap hm = world.getHeightMap();
            float max = (float)(hm.getMetersPerWorld() - 8.0f);
            if (nx < 8.0f || ny < 8.0f || nx > max || ny > max || hm.getNearestHeight(nx, ny) < hm.getSeaLevelMeters())
            {
                tornado_dx = (float)(-tornado_dx);
                tornado_dy = (float)(-tornado_dy);
            }
            else
            {
                tornado_x = nx;
                tornado_y = ny;
            }
            wind_x = tornado_dx;
            wind_y = tornado_dy;
            tornado_timer += t;
            if (tornado_timer < com.oddlabs.tt.landscape.Weather.TORNADO_PERIOD)
            {
                return;
            }
            tornado_timer -= com.oddlabs.tt.landscape.Weather.TORNADO_PERIOD;
            float strength = getTornadoStrength();
            if (strength <= 0.1f)
            {
                return;
            }
            foreach (com.oddlabs.tt.model.Selectable sel in java.lang.JIter.of(near(tornado_x, tornado_y, com.oddlabs.tt.landscape.Weather.TORNADO_BUILDING_RADIUS)))
            {
                if (sel.isDead())
                {
                    continue;
                }
                float dx = (float)(sel.getPositionX() - tornado_x);
                float dy = (float)(sel.getPositionY() - tornado_y);
                float d2 = (float)((float)(dx * dx) + (float)(dy * dy));
                if ((sel is com.oddlabs.tt.model.LandBuilding b))
                {
                    sheltered_killed += com.oddlabs.tt.model.ExtendedCombat.killOccupants(b, null, (float)(0.08f * strength), 2);
                    if (!b.isDead())
                    {
                        hurt(b, java.lang.Math.max(1, java.lang.Math.round((float)(9 * strength))), 0.0f, 1.0f);
                    }
                }
                else if ((sel is com.oddlabs.tt.model.Unit u) && !u.isMounted() && d2 < (float)(com.oddlabs.tt.landscape.Weather.TORNADO_UNIT_RADIUS * com.oddlabs.tt.landscape.Weather.TORNADO_UNIT_RADIUS))
                {
                    hurt(u, 2, tornado_dx, tornado_dy);
                    if (!u.isDead() && random.nextFloat() < 0.5f)
                    {
                        u.stun(1.5f);
                    }
                }
            }
            com.oddlabs.tt.landscape.TreeSupply tree = nearestTree(tornado_x, tornado_y, 4.0f);
            if (tree != null && random.nextFloat() < (float)(0.6f * strength))
            {
                tree.destroyByNature();
                trees_lost++;
            }
        }
        // Java: 762
        public java.util.List outdoors()
        {
            java.util.List list = new java.util.ArrayList();
            foreach (com.oddlabs.tt.player.Player p in world.getPlayers())
            {
                foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(p.getUnits().getSet()))
                {
                    if ((s is com.oddlabs.tt.model.Unit u) && !u.isDead() && !u.isMounted())
                    {
                        list.add(u);
                    }
                }
            }
            return list;
        }
        // Java: 771
        public void hail(float t)
        {
            hazard_timer += t;
            if (hazard_timer < 1.0f)
            {
                return;
            }
            hazard_timer -= 1.0f;
            float k = envelope();
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(outdoors()))
            {
                if (random.nextFloat() < (float)(0.004f * k))
                {
                    hurt(u, 1, 0.0f, 1.0f);
                }
            }
            foreach (com.oddlabs.tt.player.Player p in world.getPlayers())
            {
                foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(new java.util.ArrayList(p.getUnits().getSet())))
                {
                    if ((s is com.oddlabs.tt.model.LandBuilding b) && !b.isDead() && random.nextFloat() < (float)(0.1f * k))
                    {
                        hurt(b, 2, 0.0f, 1.0f);
                    }
                }
            }
        }
        // Java: 787
        public void blizzard(float t)
        {
            hazard_timer += t;
            if (hazard_timer < 3.0f)
            {
                return;
            }
            hazard_timer -= 3.0f;
            float k = envelope();
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(outdoors()))
            {
                if (random.nextFloat() < (float)(0.008f * k))
                {
                    hurt(u, 1, 0.0f, 1.0f);
                }
            }
        }
        // Java: 798
        public void heatwave(float t)
        {
            hazard_timer += t;
            if (hazard_timer < 6.0f)
            {
                return;
            }
            hazard_timer -= 6.0f;
            if (random.nextFloat() < (float)(0.35f * envelope()))
            {
                float[] p = randomLand();
                com.oddlabs.tt.landscape.TreeSupply tree = nearestTree(p[0], p[1], 30.0f);
                if (tree != null)
                {
                    ignite(tree);
                }
            }
        }
        // Java: 811
        public void meteorShower(float t)
        {
            meteor_timer -= t;
            if (meteor_timer > 0.0f || event_time > (float)(event_length - 4.0f))
            {
                return;
            }
            meteor_timer = (float)(2.5f + (float)(random.nextFloat() * 4.5f));
            float x = default;
            float y = default;
            com.oddlabs.tt.model.Selectable s = (random.nextFloat() < 0.4f ? randomTarget() : (com.oddlabs.tt.model.Selectable)(null));
            if (s != null)
            {
                x = (float)(s.getPositionX() + (float)(((float)(random.nextFloat() - 0.5f)) * 10.0f));
                y = (float)(s.getPositionY() + (float)(((float)(random.nextFloat() - 0.5f)) * 10.0f));
            }
            else
            {
                float[] p = randomLand();
                x = p[0];
                y = p[1];
            }
            float max = (float)(world.getHeightMap().getMetersPerWorld() - 2.0f);
            x = java.lang.Math.max(2.0f, java.lang.Math.min(max, x));
            y = java.lang.Math.max(2.0f, java.lang.Math.min(max, y));
            float z = world.getHeightMap().getNearestHeight(x, y);
            float a = (float)(random.nextFloat() * 6.2831855f);
            meteors_f.add(new float[] { 2.2f, x, y, z, (float)(x + (float)(((float)(java.lang.StrictMath.cos(a))) * 60.0f)), (float)(y + (float)(((float)(java.lang.StrictMath.sin(a))) * 60.0f)) });
        }
        // Java: 835
        public void meteors(float t)
        {
            for (int i = 0; i < meteors_f.size(); i++)
            {
                float[] m = ((float[])meteors_f.get(i));
                m[0] -= t;
                if (m[0] > 0.0f)
                {
                    continue;
                }
                meteors_f.remove(i--);
                impact(m[1], m[2], m[3]);
            }
        }
        // Java: 846
        public void impact(float x, float y, float z)
        {
            meteors_fallen++;
            com.oddlabs.tt.model.RacesResources rr = world.getRacesResources();
            if (rr != null)
            {
                world.getAudio().newAudio(new com.oddlabs.tt.audio.AudioParameters(rr.getLightningSound(), x, y, z, com.oddlabs.tt.audio.AudioPlayer.AUDIO_RANK_MAGIC, (float)(com.oddlabs.tt.audio.AudioPlayer.AUDIO_DISTANCE_MAGIC * 2.0f), com.oddlabs.tt.audio.AudioPlayer.AUDIO_GAIN_LIGHTNING, (float)(com.oddlabs.tt.audio.AudioPlayer.AUDIO_RADIUS_LIGHTNING * 2.0f)));
            }
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(near(x, y, 5.0f)))
            {
                if (s.isDead())
                {
                    continue;
                }
                float dx = (float)(s.getPositionX() - x);
                float dy = (float)(s.getPositionY() - y);
                float d = ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy)))));
                if ((s is com.oddlabs.tt.model.LandBuilding b))
                {
                    sheltered_killed += com.oddlabs.tt.model.ExtendedCombat.killOccupants(b, null, 0.25f, 1);
                    if (!b.isDead())
                    {
                        hurt(b, 22, 0.0f, 1.0f);
                    }
                }
                else if ((s is com.oddlabs.tt.model.Unit u) && !u.isMounted() && d < 4.0f)
                {
                    hurt(u, (d < 2.0f ? 4 : 2), (d > 0.0f ? (float)(dx / d) : 0.0f), (d > 0.0f ? (float)(dy / d) : 1.0f));
                    if (!u.isDead())
                    {
                        u.stun(1.5f);
                    }
                }
            }
            foreach (com.oddlabs.tt.landscape.TreeSupply tree in java.lang.JIter.of(trees(x, y, 3.5f)))
            {
                if (tree.isDead())
                {
                    continue;
                }
                if (random.nextFloat() < 0.5f)
                {
                    ignite(tree);
                }
                else
                {
                    tree.destroyByNature();
                    trees_lost++;
                }
            }
        }
        // Java: 883
        public void hurt(com.oddlabs.tt.model.Selectable s, int damage, float dx, float dy)
        {
            if ((s is com.oddlabs.tt.model.Unit))
            {
                units_hit++;
            }
            else
            {
                buildings_hit++;
            }
            s.hit(damage, dx, dy, s.getOwner());
        }
        // Java: 891
        public java.util.List near(float x, float y, float r)
        {
            com.oddlabs.tt.pathfinder.FindOccupantFilter filter = new com.oddlabs.tt.pathfinder.FindOccupantFilter(x, y, r, null, com.oddlabs.tt.model.Selectable.genericClass());
            world.getUnitGrid().scan(filter, com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(x), com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(y));
            java.util.List result = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(filter.getResult()))
            {
                result.add(s);
            }
            return result;
        }
        // Java: 900
        public java.util.List trees(float x, float y, float r)
        {
            com.oddlabs.tt.landscape.TreeScanFilter filter = new com.oddlabs.tt.landscape.TreeScanFilter(x, y, r);
            world.getUnitGrid().scan(filter, com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(x), com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(y));
            return filter.getResult();
        }
        // Java: 906
        public com.oddlabs.tt.landscape.TreeSupply nearestTree(float x, float y, float r)
        {
            com.oddlabs.tt.landscape.TreeSupply best = null;
            float best_d = java.lang.Float.MAX_VALUE;
            foreach (com.oddlabs.tt.landscape.TreeSupply tree in java.lang.JIter.of(trees(x, y, r)))
            {
                if (tree.isDead())
                {
                    continue;
                }
                float dx = (float)(tree.getPositionX() - x);
                float dy = (float)(tree.getPositionY() - y);
                float d = (float)((float)(dx * dx) + (float)(dy * dy));
                if (d < best_d)
                {
                    best_d = d;
                    best = tree;
                }
            }
            return best;
        }
        // Java: 923
        public float[] randomLand()
        {
            com.oddlabs.tt.landscape.HeightMap hm = world.getHeightMap();
            float size = hm.getMetersPerWorld();
            float sea = hm.getSeaLevelMeters();
            if (!java.lang.Float.isNaN(focus_x))
            {
                for (int i = 0; i < 12; i++)
                {
                    float tx = java.lang.Math.max(10.0f, java.lang.Math.min((float)(size - 10.0f), (float)(focus_x + (float)((float)(((float)(random.nextFloat() - 0.5f)) * 2.0f) * com.oddlabs.tt.landscape.Weather.FOCUS_SPREAD))));
                    float ty = java.lang.Math.max(10.0f, java.lang.Math.min((float)(size - 10.0f), (float)(focus_y + (float)((float)(((float)(random.nextFloat() - 0.5f)) * 2.0f) * com.oddlabs.tt.landscape.Weather.FOCUS_SPREAD))));
                    if (hm.getNearestHeight(tx, ty) > (float)(sea + 0.5f))
                    {
                        return new float[] { tx, ty };
                    }
                }
                return new float[] { focus_x, focus_y };
            }
            float x = (float)(size * 0.5f);
            float y = (float)(size * 0.5f);
            for (int i_2 = 0; i_2 < 24; i_2++)
            {
                float tx_2 = (float)(10.0f + (float)(random.nextFloat() * ((float)(size - 20.0f))));
                float ty_2 = (float)(10.0f + (float)(random.nextFloat() * ((float)(size - 20.0f))));
                x = tx_2;
                y = ty_2;
                if (hm.getNearestHeight(tx_2, ty_2) > (float)(sea + 0.5f))
                {
                    break;
                }
            }
            return new float[] { x, y };
        }
        // Java: 949
        public com.oddlabs.tt.model.Selectable randomTarget()
        {
            java.util.List all = new java.util.ArrayList();
            foreach (com.oddlabs.tt.player.Player p in world.getPlayers())
            {
                foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(p.getUnits().getSet()))
                {
                    if (!s.isDead() && !((s is com.oddlabs.tt.model.Unit u) && u.isMounted()) && (java.lang.Float.isNaN(focus_x) || (float)(java.lang.Math.abs((float)(s.getPositionX() - focus_x)) + java.lang.Math.abs((float)(s.getPositionY() - focus_y))) < com.oddlabs.tt.landscape.Weather.FOCUS_TARGETS))
                    {
                        all.add(s);
                    }
                }
            }
            if (all.isEmpty())
            {
                return null;
            }
            int pick = random.nextInt(all.size());
            com.oddlabs.tt.model.Selectable s_2 = ((com.oddlabs.tt.model.Selectable)all.get(pick));
            if (!((s_2 is com.oddlabs.tt.model.LandBuilding)) && random.nextFloat() < 0.4f)
            {
                for (int i = 0; i < 3; i++)
                {
                    com.oddlabs.tt.model.Selectable o = ((com.oddlabs.tt.model.Selectable)all.get(random.nextInt(all.size())));
                    if ((o is com.oddlabs.tt.model.LandBuilding))
                    {
                        return o;
                    }
                }
            }
            return s_2;
        }
        static Weather()
        {
            Weather.DELTA_COLOR = new org.joml.Vector4f(0.0f, 0.0f, 0.0f, (float)((float)(-1.0f) / com.oddlabs.tt.landscape.Weather.LIGHTNING_TIME));
        }
        private void init_Weather()
        {
            this.@event = com.oddlabs.tt.landscape.Weather.CALM;
            this.events = 0;
            this.wind_x = 1.0f;
            this.wind_y = 0.0f;
            this.pending_bolts = new java.util.ArrayList();
            this.fires = new java.util.ArrayList();
            this.fire_state = new java.util.ArrayList();
            this.flood_rise = com.oddlabs.tt.landscape.Weather.FLOOD_RISE;
            this.flood_hold = com.oddlabs.tt.landscape.Weather.FLOOD_HOLD;
            this.flood_fall = com.oddlabs.tt.landscape.Weather.FLOOD_FALL;
            this.meteors_f = new java.util.ArrayList();
            this.natural = true;
            this.focus_x = java.lang.Float.NaN;
            this.focus_y = java.lang.Float.NaN;
        }
        public Weather(com.oddlabs.tt.landscape.World world) : base()
        {
            init_Weather();
            this.world = world;
            this.random = world.getRandom();
            calm_left = (float)(com.oddlabs.tt.landscape.Weather.FIRST_EVENT_MIN + (float)(random.nextFloat() * ((float)(com.oddlabs.tt.landscape.Weather.FIRST_EVENT_MAX - com.oddlabs.tt.landscape.Weather.FIRST_EVENT_MIN))));
            float a = (float)(random.nextFloat() * 6.2831855f);
            wind_x = ((float)(java.lang.StrictMath.cos(a)));
            wind_y = ((float)(java.lang.StrictMath.sin(a)));
        }
    }
}
