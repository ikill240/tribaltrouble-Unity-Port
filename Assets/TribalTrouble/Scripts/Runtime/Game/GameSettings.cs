using UnityEngine;

namespace TribalTrouble.Game
{
    /// <summary>
    /// Player options (the Java OptionsMenu equivalents plus a few Unity display options), saved in PlayerPrefs.
    /// Readers pick the values up every frame, so changes apply immediately.
    /// </summary>
    public static class GameSettings
    {
        public static float master = 1f, music = 1f, sfx = 1f;
        public static bool edgePan = true;
        public static float scrollSpeed = 1f;
        public static bool invertMouseLook;
        /// <summary>"" = system language, "en", "de", "es", "it", "da", "pt".</summary>
        public static string language = "";
        public static bool fullscreen = true, vsync = true;
        /// <summary>0 = Classic (the original look), 1 = Modern (real-time shadows, SSAO, bloom, grading, sun glints).</summary>
        public static int graphicsStyle = 1;
        public static bool shadows = true, postEffects = true, reflections = true, antiAliasing = true, ambientOcclusion = true;
        public static float shadowDistance = 220f;
        /// <summary>Modern: photo-textured tree models from your other projects instead of the original trees.</summary>
        public static bool realTrees = true;
        public static bool realWeapons = true;
        public static bool combatEffects = true, combatBlood = true;
        /// <summary>The game's simulation on a thread of its own, while the graphics card is fed (a few ms a frame).</summary>
        public static bool threadedSim = true;
        /// <summary>Internal render resolution (1 = native, 1.5 / 2 = supersampled, e.g. 4K on a 1080p screen).</summary>
        public static float renderScale = 1f;
        /// <summary>0 = Day (the original sun), 1 = Night, 2 = day / night cycle (one real minute per game hour).</summary>
        public static int timeOfDay;
        /// <summary>The modern interface (slate glass menus, panels behind the in-game buttons); read at start.</summary>
        public static bool modernUi = true;
        /// <summary>The time of day in effect: the classic game always has the original day.</summary>
        public static int TimeOfDayNow => Edition.NewTides ? timeOfDay : 0;
        /// <summary>The hour a cycle starts at.</summary>
        public static float cycleStartHour = 8f;
        // ---- the Java OptionsMenu's General / Accessibility panels
        /// <summary>Settings.aggressive_units: moved units attack what they meet (Ctrl+A).</summary>
        public static bool aggressiveUnits;
        /// <summary>Settings.health_halos_enabled: selection rings turn red as health drops.</summary>
        public static bool healthHalos;
        /// <summary>Health bars over damaged units and buildings.</summary>
        public static bool healthBars = true;
        public static bool showCompass = true;
        public static bool invertYaw;
        public static bool confineCursor = true;
        public static float tooltipDelay = 0.5f;
        /// <summary>Settings.high_contrast / contrast_intensity.</summary>
        public static bool highContrast;
        public static float contrastIntensity = 0.5f;
        /// <summary>Settings.cvd_mode (0 none, 1 protanopia, 2 deuteranopia, 3 tritanopia) / cvd_intensity.</summary>
        public static int cvdMode;
        public static float cvdIntensity = 1f;
        /// <summary>Raised whenever a setting changes (the environment re-applies itself).</summary>
        public static event System.Action Changed;
        public static int quality = -1, resolution = -1;

        static readonly string[] LangCodes = { "", "en", "de", "es", "it", "da", "pt" };
        static readonly string[] LangNames = { "System", "English", "Deutsch", "Español", "Italiano", "Dansk", "Português" };
        static bool s_loaded;

        public static void Load()
        {
            if (s_loaded) return;
            s_loaded = true;
            OnlineSettings.Load();
            master = PlayerPrefs.GetFloat("tt.master", 1f);
            music = PlayerPrefs.GetFloat("tt.music", 1f);
            sfx = PlayerPrefs.GetFloat("tt.sfx", 1f);
            edgePan = PlayerPrefs.GetInt("tt.edgePan", 1) != 0;
            scrollSpeed = PlayerPrefs.GetFloat("tt.scrollSpeed", 1f);
            invertMouseLook = PlayerPrefs.GetInt("tt.invertLook", 0) != 0;
            language = PlayerPrefs.GetString("tt.language", "");
            fullscreen = PlayerPrefs.GetInt("tt.fullscreen", Screen.fullScreen ? 1 : 0) != 0;
            vsync = PlayerPrefs.GetInt("tt.vsync", 1) != 0;
            quality = PlayerPrefs.GetInt("tt.quality", QualitySettings.GetQualityLevel());
            resolution = PlayerPrefs.GetInt("tt.resolution", -1);
            graphicsStyle = PlayerPrefs.GetInt("tt.gfxStyle", 1);
            shadows = PlayerPrefs.GetInt("tt.shadows", 1) != 0;
            postEffects = PlayerPrefs.GetInt("tt.post", 1) != 0;
            reflections = PlayerPrefs.GetInt("tt.reflections", 1) != 0;
            antiAliasing = PlayerPrefs.GetInt("tt.aa", 1) != 0;
            ambientOcclusion = PlayerPrefs.GetInt("tt.ssao", 1) != 0;
            shadowDistance = PlayerPrefs.GetFloat("tt.shadowDist", 220f);
            realTrees = PlayerPrefs.GetInt("tt.realTrees", 1) != 0;
            realWeapons = PlayerPrefs.GetInt("tt.realWeapons", 1) != 0;
            combatEffects = PlayerPrefs.GetInt("tt.combatFx", 1) != 0;
            threadedSim = PlayerPrefs.GetInt("tt.threadedSim", 1) != 0;
            combatBlood = PlayerPrefs.GetInt("tt.combatBlood", 1) != 0;
            renderScale = PlayerPrefs.GetFloat("tt.renderScale", 1f);
            timeOfDay = Mathf.Clamp(PlayerPrefs.GetInt("tt.timeOfDay", 0), 0, 2);
            modernUi = PlayerPrefs.GetInt("tt.modernUi", 1) != 0;
            cycleStartHour = Mathf.Repeat(PlayerPrefs.GetFloat("tt.cycleStart", 8f), 24f);
            aggressiveUnits = PlayerPrefs.GetInt("tt.aggressive", 0) != 0;
            healthHalos = PlayerPrefs.GetInt("tt.healthHalos", 0) != 0;
            healthBars = PlayerPrefs.GetInt("tt.healthBars", 1) != 0;
            showCompass = PlayerPrefs.GetInt("tt.compass", 1) != 0;
            invertYaw = PlayerPrefs.GetInt("tt.invertYaw", 0) != 0;
            confineCursor = PlayerPrefs.GetInt("tt.confine", 1) != 0;
            tooltipDelay = PlayerPrefs.GetFloat("tt.tooltipDelay", 0.5f);
            highContrast = PlayerPrefs.GetInt("tt.highContrast", 0) != 0;
            contrastIntensity = PlayerPrefs.GetFloat("tt.contrast", 0.5f);
            cvdMode = PlayerPrefs.GetInt("tt.cvd", 0);
            cvdIntensity = PlayerPrefs.GetFloat("tt.cvdIntensity", 1f);
            Apply(true);
        }

        public static void Save()
        {
            PlayerPrefs.SetFloat("tt.master", master);
            PlayerPrefs.SetFloat("tt.music", music);
            PlayerPrefs.SetFloat("tt.sfx", sfx);
            PlayerPrefs.SetInt("tt.edgePan", edgePan ? 1 : 0);
            PlayerPrefs.SetFloat("tt.scrollSpeed", scrollSpeed);
            PlayerPrefs.SetInt("tt.invertLook", invertMouseLook ? 1 : 0);
            PlayerPrefs.SetString("tt.language", language);
            PlayerPrefs.SetInt("tt.fullscreen", fullscreen ? 1 : 0);
            PlayerPrefs.SetInt("tt.vsync", vsync ? 1 : 0);
            PlayerPrefs.SetInt("tt.quality", quality);
            PlayerPrefs.SetInt("tt.resolution", resolution);
            PlayerPrefs.SetInt("tt.gfxStyle", graphicsStyle);
            PlayerPrefs.SetInt("tt.shadows", shadows ? 1 : 0);
            PlayerPrefs.SetInt("tt.post", postEffects ? 1 : 0);
            PlayerPrefs.SetInt("tt.reflections", reflections ? 1 : 0);
            PlayerPrefs.SetInt("tt.aa", antiAliasing ? 1 : 0);
            PlayerPrefs.SetInt("tt.ssao", ambientOcclusion ? 1 : 0);
            PlayerPrefs.SetFloat("tt.shadowDist", shadowDistance);
            PlayerPrefs.SetInt("tt.realTrees", realTrees ? 1 : 0);
            PlayerPrefs.SetInt("tt.realWeapons", realWeapons ? 1 : 0);
            PlayerPrefs.SetInt("tt.combatFx", combatEffects ? 1 : 0);
            PlayerPrefs.SetInt("tt.threadedSim", threadedSim ? 1 : 0);
            PlayerPrefs.SetInt("tt.combatBlood", combatBlood ? 1 : 0);
            PlayerPrefs.SetFloat("tt.renderScale", renderScale);
            PlayerPrefs.SetInt("tt.timeOfDay", timeOfDay);
            PlayerPrefs.SetInt("tt.modernUi", modernUi ? 1 : 0);
            PlayerPrefs.SetFloat("tt.cycleStart", cycleStartHour);
            PlayerPrefs.SetInt("tt.aggressive", aggressiveUnits ? 1 : 0);
            PlayerPrefs.SetInt("tt.healthHalos", healthHalos ? 1 : 0);
            PlayerPrefs.SetInt("tt.healthBars", healthBars ? 1 : 0);
            PlayerPrefs.SetInt("tt.compass", showCompass ? 1 : 0);
            PlayerPrefs.SetInt("tt.invertYaw", invertYaw ? 1 : 0);
            PlayerPrefs.SetInt("tt.confine", confineCursor ? 1 : 0);
            PlayerPrefs.SetFloat("tt.tooltipDelay", tooltipDelay);
            PlayerPrefs.SetInt("tt.highContrast", highContrast ? 1 : 0);
            PlayerPrefs.SetFloat("tt.contrast", contrastIntensity);
            PlayerPrefs.SetInt("tt.cvd", cvdMode);
            PlayerPrefs.SetFloat("tt.cvdIntensity", cvdIntensity);
            PlayerPrefs.Save();
        }

        /// <summary>Applies the settings that aren't read every frame (volume, language, display).</summary>
        public static void Apply(bool display)
        {
            AudioListener.volume = Mathf.Clamp01(master);
            if (UnityEngine.Rendering.GraphicsSettings.currentRenderPipeline is UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset urp)
                urp.renderScale = Mathf.Clamp(renderScale, 0.5f, 2f);
            // the simulation-side option lives in the Java Settings (FaithfulController passes it with every order)
            try { com.oddlabs.tt.global.Settings.getSettings().aggressive_units = aggressiveUnits; com.oddlabs.tt.global.Settings.getSettings().health_halos_enabled = healthHalos; } catch (System.Exception) { }
            // the Expert AI writes a log of each game; in a build the working folder may not be writable, so keep the
            // logs in the game's data folder (<persistentDataPath>/ai-logs)
            try
            {
                var js = com.oddlabs.tt.global.Settings.getSettings();
                if (js.last_event_log_dir == null || string.IsNullOrWhiteSpace(js.last_event_log_dir.toString()))
                    js.last_event_log_dir = java.nio.file.Paths.get(System.IO.Path.Combine(Application.persistentDataPath, "ai-logs"));
            }
            catch (System.Exception) { }
            if (!Application.isEditor) Cursor.lockState = confineCursor ? CursorLockMode.Confined : CursorLockMode.None;
            TribalTrouble.FaithfulGame.TTRenderFeatures.SetAmbientOcclusion(graphicsStyle == 1 && postEffects && ambientOcclusion);
            Changed?.Invoke();
            if (language.Length > 0) TribalTrouble.FaithfulGame.TTStrings.Language = language == "en" ? "" : language;
            if (!display || Application.isEditor) return;
            // (the quality level first: it brings its own vsync, which would undo the player's choice)
            if (quality >= 0 && quality < QualitySettings.names.Length) QualitySettings.SetQualityLevel(quality, true);
            QualitySettings.vSyncCount = vsync ? 1 : 0;
            var res = Screen.resolutions;
            if (resolution >= 0 && resolution < res.Length)
                Screen.SetResolution(res[resolution].width, res[resolution].height, fullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed);
            else Screen.fullScreenMode = fullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        }

        static void Row(string label, System.Action control)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(150));
            control();
            GUILayout.EndHorizontal();
        }

        static float Slider(float v, float max = 1f)
        {
            float r = GUILayout.HorizontalSlider(v, 0f, max);
            GUILayout.Label(Mathf.RoundToInt(r * 100) + "%", GUILayout.Width(44));
            return r;
        }

        /// <summary>The options page (main menu Settings tab and the in-game menu). Saves when something changes.</summary>
        static Vector2 s_scroll;

        public static void OnGUI()
        {
            s_scroll = GUILayout.BeginScrollView(s_scroll);
            Body();
            GUILayout.EndScrollView();
        }

        static void Body()
        {
            Load();
            bool changed = false, displayChanged = false;
            GUI.changed = false;
            GUILayout.Label("Sound");
            Row("Master volume", () => master = Slider(master));
            Row("Music", () => music = Slider(music));
            Row("Effects", () => sfx = Slider(sfx));
            GUILayout.Space(6);
            GUILayout.Label("Camera");
            Row("Screen-edge scrolling", () => edgePan = GUILayout.Toggle(edgePan, edgePan ? " On" : " Off"));
            Row("Scroll speed", () => scrollSpeed = Slider(scrollSpeed, 2f));
            Row("Invert mouse look", () => invertMouseLook = GUILayout.Toggle(invertMouseLook, invertMouseLook ? " On" : " Off"));
            Row("Invert rotation", () => invertYaw = GUILayout.Toggle(invertYaw, invertYaw ? " On" : " Off"));
            Row("Confine cursor", () => confineCursor = GUILayout.Toggle(confineCursor, confineCursor ? " On (builds)" : " Off"));
            GUILayout.Space(6);
            GUILayout.Label("Gameplay");
            Row("Aggressive units", () => aggressiveUnits = GUILayout.Toggle(aggressiveUnits, aggressiveUnits ? " On  (Ctrl+A)" : " Off  (Ctrl+A)"));
            Row("Health halos", () => healthHalos = GUILayout.Toggle(healthHalos, healthHalos ? " On (rings redden with damage)" : " Off"));
            Row("Health bars", () => healthBars = GUILayout.Toggle(healthBars, healthBars ? " On (damaged units / buildings)" : " Off"));
            Row("Show compass", () => showCompass = GUILayout.Toggle(showCompass, showCompass ? " On" : " Off"));
            Row("Tooltip delay", () => { tooltipDelay = Mathf.Round(GUILayout.HorizontalSlider(tooltipDelay, 0f, 2f) * 10f) / 10f; GUILayout.Label(tooltipDelay.ToString("0.0") + " s", GUILayout.Width(60)); });
            GUILayout.Space(6);
            GUILayout.Label("Graphics");
            Row("Style", () => graphicsStyle = GUILayout.Toolbar(graphicsStyle, new[] { "Classic", "Modern" }));
            if (Edition.NewTides)
                Row("Interface", () => modernUi = GUILayout.Toolbar(modernUi ? 1 : 0, new[] { "Original", "Modern (after a restart)" }) == 1);
            if (Edition.Locked == null)   // (a copy of the project made for one game has nothing to pick)
            Row("At start", () =>
            {
                int cur = Edition.StartWith + 1;
                int next = GUILayout.Toolbar(cur, new[] { "Ask", Edition.ClassicTitle, "New Tides", "Tiki Madness" });
                if (next != cur) Edition.StartWith = next - 1;
            });
            if (Edition.NewTides)
                Row("Time of day", () => timeOfDay = GUILayout.Toolbar(timeOfDay, new[] { "Day (original)", "Night", "Day / night cycle" }));
            if (Edition.NewTides && timeOfDay == 2)
                Row("   Cycle starts at", () => { cycleStartHour = Mathf.Round(GUILayout.HorizontalSlider(cycleStartHour, 0f, 23f)); GUILayout.Label(((int)cycleStartHour).ToString("00") + ":00  (1 min = 1 h)", GUILayout.Width(150)); });
            GUI.enabled = graphicsStyle == 1;
            Row("Real-time shadows", () => shadows = GUILayout.Toggle(shadows, shadows ? " On" : " Off"));
            Row("Shadow distance", () => { shadowDistance = Mathf.Round(GUILayout.HorizontalSlider(shadowDistance, 80f, 500f) / 10f) * 10f; GUILayout.Label(shadowDistance + " m", GUILayout.Width(60)); });
            Row("Post effects", () => postEffects = GUILayout.Toggle(postEffects, postEffects ? " On (bloom, grading)" : " Off"));
            Row("Ambient occlusion", () => ambientOcclusion = GUILayout.Toggle(ambientOcclusion, ambientOcclusion ? " On" : " Off"));
            Row("Realistic trees", () => realTrees = GUILayout.Toggle(realTrees, realTrees ? " On" : " Off (original trees)"));
            Row("Realistic weapons", () => realWeapons = GUILayout.Toggle(realWeapons, realWeapons ? " On" : " Off (original axes and spears)"));
            Row("Combat effects", () => combatEffects = GUILayout.Toggle(combatEffects, combatEffects ? " On (impact dust, sparks, burning buildings)" : " Off"));
            Row("Simulation thread", () => threadedSim = GUILayout.Toggle(threadedSim, threadedSim ? " On (the game ticks while the frame is drawn)" : " Off (one thread)"));
            Row("Blood", () => combatBlood = GUILayout.Toggle(combatBlood, combatBlood ? " On" : " Off"));
            GUI.enabled = true;
            Row("Render resolution", () =>
            {
                int ri = renderScale >= 1.99f ? 3 : renderScale >= 1.49f ? 2 : renderScale >= 1.2f ? 1 : 0;
                int ni = GUILayout.Toolbar(ri, new[] { "100%", "125%", "150%", "200% (4K)" });
                if (ni != ri) renderScale = ni == 3 ? 2f : ni == 2 ? 1.5f : ni == 1 ? 1.25f : 1f;
            });
            Row("Water reflections", () => reflections = GUILayout.Toggle(reflections, reflections ? " On" : " Off"));
            Row("Anti-aliasing", () => antiAliasing = GUILayout.Toggle(antiAliasing, antiAliasing ? " On (SMAA)" : " Off"));
            GUILayout.Space(6);
            GUILayout.Label("Accessibility");
            Row("High contrast", () => highContrast = GUILayout.Toggle(highContrast, highContrast ? " On" : " Off"));
            if (highContrast) Row("   Contrast", () => contrastIntensity = Slider(contrastIntensity));
            Row("Colour vision", () => cvdMode = GUILayout.Toolbar(cvdMode, new[] { "Normal", "Protan.", "Deutan.", "Tritan." }));
            if (cvdMode > 0) Row("   Correction", () => cvdIntensity = Slider(cvdIntensity));
            GUILayout.Space(6);
            OnlineSettings.OnGUI();
            GUILayout.Space(6);
            GUILayout.Label("Language (in-game texts; some apply from the next game)");
            int li = System.Math.Max(0, System.Array.IndexOf(LangCodes, language));
            int nli = GUILayout.SelectionGrid(li, LangNames, 4);
            if (nli != li) { language = LangCodes[nli]; if (language.Length == 0) ResetLanguage(); }
            changed |= GUI.changed;
            if (!Application.isEditor)
            {
                GUI.changed = false;
                GUILayout.Space(6);
                GUILayout.Label("Display");
                Row("Fullscreen", () => fullscreen = GUILayout.Toggle(fullscreen, fullscreen ? " On" : " Off"));
                Row("V-sync", () => vsync = GUILayout.Toggle(vsync, vsync ? " On" : " Off"));
                var res = Screen.resolutions;
                if (res.Length > 0)
                    Row("Resolution", () =>
                    {
                        int cur = resolution >= 0 && resolution < res.Length ? resolution : res.Length - 1;
                        if (GUILayout.Button("<", GUILayout.Width(30))) resolution = (cur + res.Length - 1) % res.Length;
                        GUILayout.Box(res[cur].width + " x " + res[cur].height, GUILayout.Width(140));
                        if (GUILayout.Button(">", GUILayout.Width(30))) resolution = (cur + 1) % res.Length;
                    });
                var names = QualitySettings.names;
                if (names.Length > 1)
                    Row("Quality", () => quality = GUILayout.SelectionGrid(Mathf.Clamp(quality, 0, names.Length - 1), names, 3));
                displayChanged = GUI.changed;
            }
            if (changed || displayChanged) { Apply(displayChanged); Save(); }
            GUILayout.Space(6);
            GUILayout.Label("Keys: arrows / screen edge scroll, Insert/Delete rotate, Home/End tilt, PageUp/PageDown zoom, " +
                            "F or middle mouse = mouse look, Space = map, Enter = chat (Shift+Enter team), F3 / Pause = pause.");
        }

        static void ResetLanguage()
        {
            TribalTrouble.FaithfulGame.TTStrings.Language = Application.systemLanguage switch
            {
                SystemLanguage.German => "de", SystemLanguage.Spanish => "es", SystemLanguage.Italian => "it",
                SystemLanguage.Danish => "da", SystemLanguage.Portuguese => "pt", _ => "",
            };
        }
    }
}
