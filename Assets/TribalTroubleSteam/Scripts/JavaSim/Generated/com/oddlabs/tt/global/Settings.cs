// Translated from Settings.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.global
{
    // Java: 28
    public class Settings : java.lang.Object
    {
        public const long serialVersionUID = 1L;
        public static com.oddlabs.tt.global.Settings settings;
        public static java.util.logging.Logger logger;
        public java.nio.file.Path last_event_log_dir;
        public int last_revision;
        public bool crashed;
        public string username;
        public string pw_digest;
        public bool remember_login;
        public int graphic_detail;
        public int shadow_mode;
        public bool play_music;
        public bool play_sfx;
        public bool headphone_mode;
        public bool classic_audio;
        public float music_gain;
        public float sound_gain;
        public string language;
        public int view_width;
        public int view_height;
        public int view_freq;
        public int new_view_width;
        public int new_view_height;
        public int new_view_freq;
        public int new_view_samples;
        public bool fullscreen;
        public bool vsync;
        public int view_samples;
        public bool invert_camera_pitch;
        public bool invert_camera_yaw;
        public bool aggressive_units;
        public bool show_compass;
        public float camera_pan_speed;
        public bool camera_pan_acceleration;
        public float camera_rotate_speed;
        public float camera_zoom_speed;
        public float cinematic_camera_speed;
        public bool cinematic_unlock_limits;
        public bool confine_cursor;
        public int gamespeed;
        public float mapmode_delay;
        public float tooltip_delay;
        public float ui_scale;
        public bool developer_mode;
        public bool has_native_campaign;
        public bool save_event_log;
        public bool generate_dummy_worlds;
        public bool first_run;
        public bool warning_no_sound;
        public string patch_notes_seen_version;
        public bool show_patch_notes_on_update;
        public bool hide_multiplayer;
        public const string OFFICIAL_DOMAIN = "tribaltrouble.org";
        public string domain_name;
        public const string DISCORD_URL = "https://discord.gg/TribalTrouble";
        public const string GITHUB_URL = "https://github.com/OmarAMokhtar/tribaltrouble";
        public int frame_grab_milliseconds_per_frame;
        public int cvd_mode;
        public float cvd_intensity;
        public bool high_contrast;
        public float contrast_intensity;
        public bool team_stencil;
        public static org.joml.Vector4f[] DEFAULT_TEAM_COLOURS;
        // Java: 125
        public static org.joml.Vector4f[] generateDefaultColours()
        {
            org.joml.Vector4f[] handPicked = new org.joml.Vector4f[] { com.oddlabs.util.Color.argb4v((-16640)), com.oddlabs.util.Color.argb4v((-16744449)), com.oddlabs.util.Color.argb4v((-65472)), com.oddlabs.util.Color.argb4v((-16711745)), com.oddlabs.util.Color.argb4v((-4259585)), com.oddlabs.util.Color.argb4v((-4194560)), com.oddlabs.util.Color.argb4v((-256)), com.oddlabs.util.Color.argb4v((-38476)), com.oddlabs.util.Color.argb4v((-16751616)), com.oddlabs.util.Color.argb4v((-7667712)), com.oddlabs.util.Color.argb4v((-16728065)), com.oddlabs.util.Color.argb4v((-1)), com.oddlabs.util.Color.argb4v((-16777088)), com.oddlabs.util.Color.argb4v((-32944)), com.oddlabs.util.Color.argb4v((-10496)), com.oddlabs.util.Color.argb4v((-65281)), com.oddlabs.util.Color.argb4v((-14513374)), com.oddlabs.util.Color.argb4v((-9404272)) };
            org.joml.Vector4f[] all = new org.joml.Vector4f[com.oddlabs.matchmaking.MatchmakingServerInterface.MAX_PLAYERS];
            int copyCount = java.lang.Math.min(handPicked.Length, all.Length);
            java.lang.JSystem.arraycopy(handPicked, 0, all, 0, copyCount);
            for (int i = copyCount; i < all.Length; i++)
            {
                float hue = (float)((i - handPicked.Length) / ((float)((all.Length - handPicked.Length))));
                int rgb = java.awt.Color.HSBtoRGB(hue, 0.8f, 0.9f);
                all[i] = com.oddlabs.util.Color.argb4v((-16777216) | (rgb & 16777215));
            }
            return all;
        }
        public org.joml.Vector4f[] team_colours;
        // Java: 167
        public static void setSettings(com.oddlabs.tt.global.Settings new_settings)
        {
            com.oddlabs.tt.global.Settings.settings = new_settings;
        }
        // Java: 171
        public static com.oddlabs.tt.global.Settings getSettings()
        {
            return com.oddlabs.tt.global.Settings.settings;
        }
        // Java: 175
        public bool inDeveloperMode()
        {
            return developer_mode;
        }
        // Java: 179
        public void setDomain(string new_domain)
        {
            domain_name = new_domain;
        }
        // Java: 183
        public string getDomainName()
        {
            return domain_name;
        }
        // Java: 187
        public bool isOfficialServer()
        {
            if (java.lang.JO.equals(com.oddlabs.tt.global.Settings.OFFICIAL_DOMAIN, domain_name) || domain_name.endsWith("." + java.lang.JStr.valueOf(com.oddlabs.tt.global.Settings.OFFICIAL_DOMAIN)))
            {
                return true;
            }
            if (com.oddlabs.tt.global.Globals.debug_steam_auth_localhost && (java.lang.JO.equals("localhost", domain_name) || java.lang.JO.equals("local", domain_name)))
            {
                return true;
            }
            return false;
        }
        // Java: 199
        public string getMatchmakingAddress()
        {
            return "matchmaking." + java.lang.JStr.valueOf(domain_name);
        }
        // Java: 203
        public string getRouterAddress()
        {
            return "router." + java.lang.JStr.valueOf(domain_name);
        }
        // Java: 207
        public void save()
        {
            if (com.oddlabs.tt.@event.LocalEventQueue.getQueue().getDeterministic().isPlayback())
            {
                return;
            }
            com.oddlabs.tt.global.Settings defaults = new com.oddlabs.tt.global.Settings();
            java.util.Properties props = new java.util.Properties();
            setProperty(props, "last_event_log_dir", last_event_log_dir, defaults.last_event_log_dir);
            setProperty(props, "last_revision", last_revision, defaults.last_revision);
            setProperty(props, "crashed", crashed, defaults.crashed);
            setProperty(props, "username", username, defaults.username);
            setProperty(props, "pw_digest", pw_digest, defaults.pw_digest);
            setProperty(props, "remember_login", remember_login, defaults.remember_login);
            setProperty(props, "graphic_detail", graphic_detail, defaults.graphic_detail);
            setProperty(props, "shadow_mode", shadow_mode, defaults.shadow_mode);
            setProperty(props, "play_music", play_music, defaults.play_music);
            setProperty(props, "play_sfx", play_sfx, defaults.play_sfx);
            setProperty(props, "headphone_mode", headphone_mode, defaults.headphone_mode);
            setProperty(props, "classic_audio", classic_audio, defaults.classic_audio);
            setProperty(props, "music_gain", music_gain, defaults.music_gain);
            setProperty(props, "sound_gain", sound_gain, defaults.sound_gain);
            setProperty(props, "language", language, defaults.language);
            setProperty(props, "view_width", view_width, defaults.view_width);
            setProperty(props, "view_height", view_height, defaults.view_height);
            setProperty(props, "view_freq", view_freq, defaults.view_freq);
            setProperty(props, "new_view_width", new_view_width, defaults.new_view_width);
            setProperty(props, "new_view_height", new_view_height, defaults.new_view_height);
            setProperty(props, "new_view_freq", new_view_freq, defaults.new_view_freq);
            setProperty(props, "new_view_samples", new_view_samples, defaults.new_view_samples);
            setProperty(props, "fullscreen", fullscreen, defaults.fullscreen);
            setProperty(props, "view_samples", view_samples, defaults.view_samples);
            setProperty(props, "invert_camera_pitch", invert_camera_pitch, defaults.invert_camera_pitch);
            setProperty(props, "invert_camera_yaw", invert_camera_yaw, defaults.invert_camera_yaw);
            setProperty(props, "aggressive_units", aggressive_units, defaults.aggressive_units);
            setProperty(props, "show_compass", show_compass, defaults.show_compass);
            setProperty(props, "camera_pan_speed", camera_pan_speed, defaults.camera_pan_speed);
            setProperty(props, "camera_pan_acceleration", camera_pan_acceleration, defaults.camera_pan_acceleration);
            setProperty(props, "camera_rotate_speed", camera_rotate_speed, defaults.camera_rotate_speed);
            setProperty(props, "camera_zoom_speed", camera_zoom_speed, defaults.camera_zoom_speed);
            setProperty(props, "cinematic_camera_speed", cinematic_camera_speed, defaults.cinematic_camera_speed);
            setProperty(props, "cinematic_unlock_limits", cinematic_unlock_limits, defaults.cinematic_unlock_limits);
            setProperty(props, "confine_cursor", confine_cursor, defaults.confine_cursor);
            setProperty(props, "gamespeed", gamespeed, defaults.gamespeed);
            setProperty(props, "mapmode_delay", mapmode_delay, defaults.mapmode_delay);
            setProperty(props, "tooltip_delay", tooltip_delay, defaults.tooltip_delay);
            setProperty(props, "ui_scale", ui_scale, defaults.ui_scale);
            setProperty(props, "first_run", first_run, defaults.first_run);
            setProperty(props, "warning_no_sound", warning_no_sound, defaults.warning_no_sound);
            setProperty(props, "patch_notes_seen_version", patch_notes_seen_version, defaults.patch_notes_seen_version);
            setProperty(props, "show_patch_notes_on_update", show_patch_notes_on_update, defaults.show_patch_notes_on_update);
            setProperty(props, "domain_name", domain_name, defaults.domain_name);
            setProperty(props, "cvd_mode", cvd_mode, defaults.cvd_mode);
            setProperty(props, "cvd_intensity", cvd_intensity, defaults.cvd_intensity);
            setProperty(props, "high_contrast", high_contrast, defaults.high_contrast);
            setProperty(props, "contrast_intensity", contrast_intensity, defaults.contrast_intensity);
            setProperty(props, "team_stencil", team_stencil, defaults.team_stencil);
            setProperty(props, "team_colours", team_colours, defaults.team_colours);
            com.oddlabs.tt.render.Renderer.getLocalInput().getInputManager().saveBindings(props);
            java.nio.file.Path settings_file = com.oddlabs.tt.render.Renderer.getLocalInput().getGameDir().resolve(com.oddlabs.tt.global.Globals.getSettingsFileName());
            {
                java.io.OutputStream @out = java.nio.file.Files.newOutputStream(settings_file);
                try
                {
                    props.store(@out, java.lang.JO.toString(java.time.Instant.now()));
                }
                catch (System.IO.IOException e)
                {
                    com.oddlabs.tt.global.Settings.logger.warning("Failed to write settings to " + java.lang.JStr.valueOf(settings_file) + " exception: " + java.lang.JStr.valueOf(e));
                }
                finally
                {
                    if (@out != null) @out.close();
                }
            }
        }
        // Java: 276
        public void load(java.nio.file.Path game_dir)
        {
            java.util.Properties props = new java.util.Properties();
            java.nio.file.Path settings_file = game_dir.resolve(com.oddlabs.tt.global.Globals.getSettingsFileName());
            if (!java.nio.file.Files.exists(settings_file))
            {
                return;
            }
            {
                java.io.InputStream @in = java.nio.file.Files.newInputStream(settings_file);
                try
                {
                    props.load(@in);
                }
                catch (System.IO.IOException unused)
                {
                    com.oddlabs.tt.global.Settings.logger.warning("WARNING: Could not read settings from " + java.lang.JStr.valueOf(settings_file) + ". Using defaults.");
                    return;
                }
                finally
                {
                    if (@in != null) @in.close();
                }
            }
            last_event_log_dir = com.oddlabs.tt.global.Settings.getPath(props, "last_event_log_dir", last_event_log_dir);
            last_revision = getInt(props, "last_revision", last_revision);
            crashed = getBoolean(props, "crashed", crashed);
            username = props.getProperty("username", username);
            pw_digest = props.getProperty("pw_digest", pw_digest);
            remember_login = getBoolean(props, "remember_login", remember_login);
            graphic_detail = getInt(props, "graphic_detail", graphic_detail);
            shadow_mode = getInt(props, "shadow_mode", shadow_mode);
            play_music = getBoolean(props, "play_music", play_music);
            play_sfx = getBoolean(props, "play_sfx", play_sfx);
            headphone_mode = getBoolean(props, "headphone_mode", headphone_mode);
            classic_audio = getBoolean(props, "classic_audio", classic_audio);
            music_gain = getFloat(props, "music_gain", music_gain);
            sound_gain = getFloat(props, "sound_gain", sound_gain);
            language = props.getProperty("language", language);
            view_width = getInt(props, "view_width", view_width);
            view_height = getInt(props, "view_height", view_height);
            view_freq = getInt(props, "view_freq", view_freq);
            new_view_width = getInt(props, "new_view_width", new_view_width);
            new_view_height = getInt(props, "new_view_height", new_view_height);
            new_view_freq = getInt(props, "new_view_freq", new_view_freq);
            new_view_samples = getInt(props, "new_view_samples", new_view_samples);
            fullscreen = getBoolean(props, "fullscreen", fullscreen);
            view_samples = getInt(props, "view_samples", view_samples);
            invert_camera_pitch = getBoolean(props, "invert_camera_pitch", invert_camera_pitch);
            invert_camera_yaw = getBoolean(props, "invert_camera_yaw", invert_camera_yaw);
            aggressive_units = getBoolean(props, "aggressive_units", aggressive_units);
            show_compass = getBoolean(props, "show_compass", show_compass);
            camera_pan_speed = getFloat(props, "camera_pan_speed", camera_pan_speed);
            camera_pan_acceleration = getBoolean(props, "camera_pan_acceleration", camera_pan_acceleration);
            camera_rotate_speed = getFloat(props, "camera_rotate_speed", camera_rotate_speed);
            camera_zoom_speed = getFloat(props, "camera_zoom_speed", camera_zoom_speed);
            cinematic_camera_speed = getFloat(props, "cinematic_camera_speed", cinematic_camera_speed);
            cinematic_unlock_limits = getBoolean(props, "cinematic_unlock_limits", cinematic_unlock_limits);
            confine_cursor = getBoolean(props, "confine_cursor", confine_cursor);
            gamespeed = getInt(props, "gamespeed", gamespeed);
            mapmode_delay = getFloat(props, "mapmode_delay", mapmode_delay);
            tooltip_delay = getFloat(props, "tooltip_delay", tooltip_delay);
            ui_scale = getFloat(props, "ui_scale", ui_scale);
            first_run = getBoolean(props, "first_run", first_run);
            warning_no_sound = getBoolean(props, "warning_no_sound", warning_no_sound);
            patch_notes_seen_version = props.getProperty("patch_notes_seen_version", patch_notes_seen_version);
            show_patch_notes_on_update = getBoolean(props, "show_patch_notes_on_update", show_patch_notes_on_update);
            domain_name = props.getProperty("domain_name", domain_name);
            cvd_mode = getInt(props, "cvd_mode", cvd_mode);
            cvd_intensity = getFloat(props, "cvd_intensity", cvd_intensity);
            high_contrast = getBoolean(props, "high_contrast", high_contrast);
            contrast_intensity = getFloat(props, "contrast_intensity", contrast_intensity);
            team_stencil = getBoolean(props, "team_stencil", team_stencil);
            team_colours = com.oddlabs.tt.global.Settings.getColours(props, "team_colours", team_colours);
            com.oddlabs.tt.render.Renderer.getLocalInput().getInputManager().loadBindings(props);
        }
        // Java: 345
        public void setProperty(java.util.Properties props, string key, java.nio.file.Path @value, java.nio.file.Path defaultValue)
        {
            if (!java.lang.JO.equals(@value, defaultValue))
            {
                props.setProperty(key, java.lang.JO.toString(@value));
            }
        }
        // Java: 351
        public void setProperty(java.util.Properties props, string key, string @value, string defaultValue)
        {
            if (!java.lang.JO.equals(@value, defaultValue))
            {
                props.setProperty(key, @value);
            }
        }
        // Java: 358
        public void setProperty(java.util.Properties props, string key, int @value, int defaultValue)
        {
            if (@value != defaultValue)
            {
                props.setProperty(key, java.lang.JString.valueOf(@value));
            }
        }
        // Java: 364
        public void setProperty(java.util.Properties props, string key, float @value, float defaultValue)
        {
            if (@value != defaultValue)
            {
                props.setProperty(key, java.lang.JString.valueOf(@value));
            }
        }
        // Java: 370
        public void setProperty(java.util.Properties props, string key, bool @value, bool defaultValue)
        {
            if (@value != defaultValue)
            {
                props.setProperty(key, java.lang.JString.valueOf(@value));
            }
        }
        // Java: 376
        public void setProperty(java.util.Properties props, string key, org.joml.Vector4fc[] @value, org.joml.Vector4fc[] defaultValue)
        {
            if (!java.util.Arrays.equals(@value, defaultValue))
            {
                string colors = ((string)java.util.Arrays.stream(@value).mapToInt(new java.util.function.ToIntFunction_L((object a0) => { return com.oddlabs.util.Color.argbi(((org.joml.Vector4fc)a0)); })).mapToObj(new java.util.function.IntFunction_L((int a0) => { return java.lang.Integer.toHexString(a0); })).collect(java.util.stream.Collectors.joining(",")));
                props.setProperty(key, colors);
            }
        }
        // Java: 387
        public bool getBoolean(java.util.Properties props, string key, bool defaultValue)
        {
            string @value = props.getProperty(key);
            if (@value == null)
            {
                return defaultValue;
            }
            return java.lang.Boolean.parseBoolean(@value);
        }
        // Java: 396
        public int getInt(java.util.Properties props, string key, int defaultValue)
        {
            string @value = props.getProperty(key);
            if (@value == null)
            {
                return defaultValue;
            }
            try
            {
                return java.lang.Integer.parseInt(@value);
            }
            catch (System.FormatException unused)
            {
                com.oddlabs.tt.global.Settings.logger.warning("WARNING: Invalid value for setting '" + java.lang.JStr.valueOf(key) + "': '" + java.lang.JStr.valueOf(@value) + "'. Using default value '" + (defaultValue) + "'.");
                return defaultValue;
            }
        }
        // Java: 410
        public float getFloat(java.util.Properties props, string key, float defaultValue)
        {
            string @value = props.getProperty(key);
            if (@value == null)
            {
                return defaultValue;
            }
            try
            {
                return java.lang.Float.parseFloat(@value);
            }
            catch (System.FormatException unused)
            {
                com.oddlabs.tt.global.Settings.logger.warning("WARNING: Invalid value for setting '" + java.lang.JStr.valueOf(key) + "': '" + java.lang.JStr.valueOf(@value) + "'. Using default value '" + java.lang.JStr.valueOf(defaultValue) + "'.");
                return defaultValue;
            }
        }
        // Java: 424
        public static java.nio.file.Path getPath(java.util.Properties props, string key, java.nio.file.Path defaultValue)
        {
            string @value = props.getProperty(key);
            if (@value == null || @value.isEmpty())
            {
                return defaultValue;
            }
            try
            {
                return java.nio.file.Path_.of(@value);
            }
            catch (System.Exception unused)
            {
                com.oddlabs.tt.global.Settings.logger.warning("Invalid path for setting '" + java.lang.JStr.valueOf(key) + "': '" + java.lang.JStr.valueOf(@value) + "'. Using default value '" + java.lang.JStr.valueOf(defaultValue) + "'.");
                return defaultValue;
            }
        }
        // Java: 438
        public static org.joml.Vector4f[] getColours(java.util.Properties props, string key, org.joml.Vector4f[] defaultValue)
        {
            string @value = props.getProperty(key);
            if (@value == null)
            {
                return defaultValue;
            }
            try
            {
                string[] hexStrings = @value.split(",");
                org.joml.Vector4f[] result = new org.joml.Vector4f[com.oddlabs.tt.global.Settings.DEFAULT_TEAM_COLOURS.Length];
                for (int i = 0; i < com.oddlabs.tt.global.Settings.DEFAULT_TEAM_COLOURS.Length; i++)
                {
                    result[i] = new org.joml.Vector4f(com.oddlabs.tt.global.Settings.DEFAULT_TEAM_COLOURS[i]);
                }
                for (int i_2 = 0; i_2 < java.lang.Math.min(com.oddlabs.tt.global.Settings.DEFAULT_TEAM_COLOURS.Length, hexStrings.Length); i_2++)
                {
                    int argb = unchecked((int)(java.lang.Long.parseLong(hexStrings[i_2], 16)));
                    result[i_2] = com.oddlabs.util.Color.argb4v(argb);
                }
                return result;
            }
            catch (System.Exception e)
            {
                com.oddlabs.tt.global.Settings.logger.warning("WARNING: Invalid value for setting '" + java.lang.JStr.valueOf(key) + "': '" + java.lang.JStr.valueOf(@value) + "'. Using default value. Error: " + java.lang.JStr.valueOf(e));
                return defaultValue;
            }
        }
        // Java: 462
        public void writeObject(java.io.ObjectOutputStream @out)
        {
            @out.defaultWriteObject();
            @out.writeObject(java.lang.JO.toString(last_event_log_dir));
        }
        // Java: 468
        public void readObject(java.io.ObjectInputStream @in)
        {
            @in.defaultReadObject();
            last_event_log_dir = java.nio.file.Path_.of(((string)@in.readObject()));
        }
        static Settings()
        {
            Settings.logger = java.util.logging.Logger.getLogger(typeof(com.oddlabs.tt.global.Settings).getName());
            Settings.DEFAULT_TEAM_COLOURS = com.oddlabs.tt.global.Settings.generateDefaultColours();
        }
        private void init_Settings()
        {
            this.last_event_log_dir = java.nio.file.Path_.of("");
            this.last_revision = (-1);
            this.crashed = false;
            this.username = "";
            this.pw_digest = "";
            this.remember_login = false;
            this.graphic_detail = com.oddlabs.tt.global.Globals.DETAIL_NORMAL;
            this.shadow_mode = com.oddlabs.tt.global.Globals.SHADOWS_STATIC;
            this.play_music = true;
            this.play_sfx = true;
            this.headphone_mode = false;
            this.classic_audio = true;
            this.music_gain = 0.5f;
            this.sound_gain = 1.0f;
            this.language = "default";
            this.view_width = (-1);
            this.view_height = (-1);
            this.view_freq = (-1);
            this.new_view_width = view_width;
            this.new_view_height = view_height;
            this.new_view_freq = view_freq;
            this.new_view_samples = 4;
            this.fullscreen = true;
            this.vsync = true;
            this.view_samples = 4;
            this.invert_camera_pitch = false;
            this.invert_camera_yaw = false;
            this.aggressive_units = false;
            this.show_compass = true;
            this.camera_pan_speed = 1.0f;
            this.camera_pan_acceleration = true;
            this.camera_rotate_speed = 1.0f;
            this.camera_zoom_speed = 1.0f;
            this.cinematic_camera_speed = 0.4f;
            this.cinematic_unlock_limits = true;
            this.confine_cursor = true;
            this.gamespeed = com.oddlabs.matchmaking.Game.GAMESPEED_NORMAL;
            this.mapmode_delay = 0.5f;
            this.tooltip_delay = 0.5f;
            this.ui_scale = 0.0f;
            this.developer_mode = java.lang.Boolean.getBoolean("com.oddlabs.tt.developer");
            this.has_native_campaign = developer_mode;
            this.save_event_log = true;
            this.generate_dummy_worlds = false;
            this.first_run = true;
            this.warning_no_sound = true;
            this.patch_notes_seen_version = "";
            this.show_patch_notes_on_update = true;
            this.hide_multiplayer = false;
            this.domain_name = com.oddlabs.tt.global.Settings.OFFICIAL_DOMAIN;
            this.frame_grab_milliseconds_per_frame = 40;
            this.cvd_mode = 0;
            this.cvd_intensity = 1.0f;
            this.high_contrast = false;
            this.contrast_intensity = 0.5f;
            this.team_stencil = false;
            this.team_colours = new org.joml.Vector4f[com.oddlabs.tt.global.Settings.DEFAULT_TEAM_COLOURS.Length];
        }
        public Settings() : base()
        {
            init_Settings();
            for (int i = 0; i < com.oddlabs.tt.global.Settings.DEFAULT_TEAM_COLOURS.Length; i++)
            {
                team_colours[i] = new org.joml.Vector4f(com.oddlabs.tt.global.Settings.DEFAULT_TEAM_COLOURS[i]);
            }
        }
    }
}
