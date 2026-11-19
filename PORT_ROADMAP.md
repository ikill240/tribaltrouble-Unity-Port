# Tribal Trouble → Unity 6.1 port roadmap

Source of truth: **`tribaltrouble-main(Working)1.4.1`** (Omar's fork + Bondolo base + the `ikill240c` / `ikill240p` features).
Target: Unity 6.1, URP. Single-player first, then multiplayer that talks to the existing Java matchmaker/router servers.

## Ground rules

1. **The base game stays intact.** The simulation is ported faithfully: same rules, same numbers (`RacesResources`), same timing.
   New ideas and extra RTS mechanics go in later as *rule options* (like `GameModeOptions`). With the options off, the game is
   byte-for-byte the original.
2. **Bit-exact deterministic simulation.** The C# sim is written to reproduce the Java sim exactly (the same `java.util.Random`,
   StrictMath/fdlibm where Java uses it, Java float→int cast rules, the same iteration orders). This costs little if we plan it from
   the start, and it is the only way Unity players can share a lockstep game with Java players.
3. **Three layers.**
   - `TribalTrouble.Sim` is pure C# with no UnityEngine. It covers world, entities, pathfinding, AI and rules, and runs headless.
   - `TribalTrouble.Runtime` is the presentation layer: rendering, audio, input→commands and UI. It reads sim state and never changes it directly.
   - `TribalTrouble.Net` (later) holds the ARMI protocol, router/lockstep and matchmaking.
4. **Conformance testing against Java.** A headless Java runner records the world checksum every tick for a seed and a command
   log. The C# sim replays the same log and must match tick for tick. The fork already contains `player/ai/sim` (a match runner)
   and `MapDump`, which we can extend.

## Stages

| # | Stage | Scope | Done when |
|---|-------|-------|-----------|
| 0 | **Assets & project** ✅ | Unity project, URP, model converter (XML → glTF, verified to ~2 µm against the Java skinning), textures, audio, translations, team-colour shader, model viewer | All 76 models browse and animate in the viewer |
| 1 | Deterministic foundations ✅ (all 10 golden cases pass) | `JavaRandom` ✅, Java numeric semantics (`J`) ✅, Java collections/deque ✅, CRC32 and DataInput ✅, `Channel`/`Layer`/`Voronoi`/`Midpoint`/`Mountain`/`Hill`/`Tools` ✅, `Landscape` island generator incl. terrain textures ✅, `AuthoredTerrain` (.ttmap v2) ✅, 10 Java golden maps + conformance runner ✅. Still to do: map codes (`WordsEncoding`) and an fdlibm port if conformance needs it | All 10 golden cases bit-identical (Tools → Tribal Trouble → Conformance) |
| 2 | Simulation core ✅ (translated, conformance passes) | `World`, `AnimationManager` (50 Hz, 20 ms ticks, game speeds), `UnitGrid`, hierarchical pathfinder, `Model/Unit/Building/Ship/Supply`, controllers and behaviours, weapons, magic, `RacesResources`, global checksum | Java vs C# checksums match on recorded AI-vs-AI games |
| 3 | Presentation | Terrain and landscape shading, water, sky, instanced trees, units (`TTSpriteView` → GPU-baked animation for big armies), particles (fog, lightning, sonic blast), audio, RTS camera, selection and orders, HUD, map mode, compass | A playable skirmish on a generated island |
| 4 | Single-player game | AIs (`AdvancedAI` first, then Expert/Ultra/Fable/Gauntlet ≈ 57k lines), modes (standard, KOTH, KOTI, survival), skirmish menu (`TerrainMenu`), tutorial, campaign (23 islands + triggers), saves, settings, localization, adaptive-AI profiles | The full offline game |
| 5 | Multiplayer ✅ (Unity↔Unity verified through your servers) | ARMI codec (method-index table generated from the Java interfaces), Java-serialization subset for the lobby objects, router/PeerHub lockstep, matchmaker login, lobby and chat, tunnelling, custom-map transfer, spectator | Unity↔Unity games through your servers, then Unity↔Java cross-play once stage 2 conformance passes |
| 6 | Your ideas + extra RTS mechanics | Added behind rule toggles, one at a time, each with its own conformance baseline | — |
| 7 | Map editor | Port of the 3D map editor (`mapeditor/*`, `.ttmap` v4) | — |

## Known compatibility hazards (from the source survey)

- **Wire format.** ARMI method ids are positions in `Method.toString()`-sorted order, so package, class and parameter type names all
  matter. The fork's `PlayerInterface` changes (queue, stand ground, formations) mean it is already not wire-compatible with stock TT.
  The C# side must use *this* fork's table.
- **Java serialization on the wire.** Enums (`Action`, `DeployType`, `Formation`), `Class<? extends Supply>`, and lobby objects (`Game`,
  `WorldConfig`, `PlayerSlot[]`, the world generator) are sent with `ObjectOutputStream`. We need a C# codec for that subset.
- **Float truncation.** `DefaultARMIArgumentWriter` sends floats as `intValue()`, so `beacon(float, float)` is truncated. The C# side reproduces this.
- **Non-determinism already present in Java (worth fixing on the Java side too):**
  - `AiBehaviorBandit` uses an unseeded `new Random()`.
  - Adaptive AI reads local profile files mid-match, which desyncs in multiplayer.
  - `LandBuilding.occupy()` iterates a `HashSet<Unit>` in identity-hash order.
- **Math.** `Math.sin/cos/pow/atan2` are used in weapons, formations, AI and procedural code. Java's results equal StrictMath (fdlibm) on
  HotSpot only for some functions, so the conformance suite decides function by function. `sqrt` is safe.

## Stage 0 contents (this delivery)

- `PortTools/tt_geometry_to_gltf.py` converts `geometry.xml` sprites to `.glb` + `catalog.json`.
  - TT is Z-up and is mapped to glTF Y-up.
  - Absolute bone matrices are converted to a local TRS hierarchy, with inverse bind = init⁻¹.
  - Each clip is 1 s = 1 cycle, so clip time = `anim_ticks / wpc`. LOOP clips wrap and PLAIN clips clamp, exactly as in `Sprite.getAnimationState`.
- `PortTools/check_skinning.py` compares the glTF pose against the Java formula `Σ w·(F·I⁻¹)·v`. `render_glb.py` is the preview renderer.
- `PortTools/import_tt_assets.py` re-imports everything from the Java repo.
- `Assets/TribalTrouble/`
  - `Shaders/TTSprite.shader`: the URP port of SpriteShader's team-colour blend.
  - `Scripts/Runtime/Presentation`: sprite definitions, catalog and view.
  - `Scripts/Runtime/Viewer/ModelViewer`.
  - `Scripts/Sim/Determinism/JavaRandom` (checked against real Java output).
  - `Scripts/Editor`: project setup, texture rules, catalog builder and viewer scene.

## Stage 1 notes (island generator)

- **Golden data.** `PortTools/golden/GoldenLandscape.java` runs the real 1.4.1 `Landscape` headless and records every gameplay output.
  - It compiles against your source unchanged; only GL, GUI and `java.lang.IO` shims are added.
  - Java output was identical in four JVM modes: JIT, interpreter, C1 only, and math intrinsics off. So the generator does not depend on
    HotSpot math intrinsics, and correctly rounded C# math reproduces it.
- **Translation.** `PortTools/j2cs.py` transliterates Java to C# with Java numeric semantics; every file was then reviewed by hand.
  Java identifiers are kept, so each `.cs` file diffs against its `.java` source.
- **Java behaviours the port reproduces deliberately:**
  - `Arrays.asList` is a view, so player start positions are shuffled in place.
  - `HashMap<Integer, …>` iterates small integer keys in ascending order (team grouping).
  - `Float.MIN_VALUE` is `float.Epsilon`, which matters in `findMax`.
  - Signed `byte` becomes `sbyte`.
  - Java float→int casts follow Java rules (NaN gives 0, out-of-range values saturate).
  - `Math.round` rounds halves up.
  - Labelled `break` becomes `goto`.
- **Running the checks:**
  - In Unity: Tools → Tribal Trouble → Conformance. The report is written to `PortTools/conformance_report.txt`.
  - Outside Unity: `cd PortTools/SimTests && dotnet run -c Release` (needs the .NET 8 SDK).

## Playable skirmish (interim layer, stage 3 preview)

The island generator is bit-exact (all 10 golden cases pass). On top of it there is a **playable skirmish** in
`Scripts/Runtime/Game/`. It uses 1.4.1 numbers (speeds, HP, sizes, weapon costs, reproduction formula, supplies)
but its rules code is new and interim. The faithful Java sim (stage 2) replaces `GameSim`/`SkirmishAI` later;
the camera, HUD, view, terrain and audio stay.

- `GameWorld` (grid, walkability, supplies from the generator), `PathFinder` (8-dir A*), `GameSim` (20 ms ticks:
  gather → deliver to armory, build, quarters breeding, armory weapons, deploy warriors, towers, combat, victory),
  `SkirmishAI` (Easy/Normal/Hard), `TerrainBuilder` (generator's own blend textures), `GameView`,
  `RtsCamera`, `PlayerController` (selection, orders, placement, panels, minimap, groups), `SfxPlayer`, `SkirmishGame` (menu + bootstrap).
- Run it: **Tools → Tribal Trouble → Create Skirmish Scene (playable)**, then Play.
- Known gaps: terrain casts no shadows; minimap doesn't update when supplies run out; no chieftain, magic, ships or
  rally points yet; AI is a simple wave attacker, not the original AdvancedAI.

## Stage 2 — faithful simulation (translated Java) ✅ first playable

The whole Java simulation (model, controllers/behaviours, pathfinder, weapons, magic, `AdvancedAI`, island generator;
282 files) is translated to C# by the J2CS translator and lives in `Scripts/JavaSim/` (asmdef `TribalTrouble.JavaSim`,
generated — regenerate instead of editing). `JavaSim/Runtime/` is the small Java library shim (java.lang/util, StrictMath via
translated fdlibm, JOML subset).

- **Conformance:** replays of real Java games match tick for tick — 2 Hard AIs 30 000 ticks; natives vs vikings, northern
  512 map 18 000 ticks; 4-player FFA (Hard/Hard/Normal/Easy) 18 000 ticks.
- **Determinism fixes needed in Java too** (applied to both sides for the recordings): identity-hash `HashSet`/`HashMap`
  iteration in `AdvancedAI`, `LandBuilding.trappedUnits`, `ShipTrajectory`, `ShipHR` (→ Linked collections);
  `Controller.getKey()` / `RepairController.getKey()` used identity hashes; `Math.sin/cos/atan2/pow/...` differ from fdlibm on
  HotSpot for 3–10 % of inputs (→ `StrictMath`). Without these, Java vs Java runs can also desync in multiplayer.
- **Game layer** (`Scripts/Runtime/Faithful/`): `FaithfulMatch` (world setup exactly like `WorldViewer`, 50 Hz ticks,
  `GameOverTrigger` rules), `FaithfulScene` (the Java `RenderState`/`TreeRenderer` visitors → per-model sprite, animation,
  TT transform), `FaithfulView` (GameObjects for units/buildings/chickens/weapons, GPU instancing for trees/plants/rocks),
  `FaithfulTerrain`, `FaithfulController` (Java selection/target/placing delegates, `ActionButtonPanel` panels and the
  original spinners and key bindings), `FaithfulGame` (menu + loop). Orders are the same `PlayerInterface` calls the Java GUI makes.
- Run it: **Tools → Tribal Trouble → Create Faithful Skirmish Scene (original rules)**, then Play.
- AI profiles (`player_style_profile`, adaptive) are saved to `Application.persistentDataPath`.
- **All skirmish AIs:** Expert, Ultra, Fable and Gauntlet are translated too (73 more files). Each was recorded
  against Hard AdvancedAI in Java and replays tick for tick in C# (12 000 ticks each). Their packages iterate several
  `HashSet`/`HashMap`s keyed on units: the determinism overlay makes those insertion-ordered on both sides.
- **Particles:** building smoke, damage smoke, poison fog, stun stars, lightning and the sonic blast are drawn from the
  simulation's own emitters (`FaithfulParticles`, shader `TribalTrouble/Particle`), with textures from the game's own
  procedural generators (`GeneratorSmoke`, `GeneratorLightning`, ...).
- **Campaign:** both campaigns (8 native + 15 viking islands) run on the translated island scripts and triggers.
  The Java GUI calls they make (dialogs, camera jumps, countdowns, game-over, campaign map, save list) go through a
  small bridge (`JavaSim/Generated/Boundary/TribalTrouble/Bridge.cs` + overridden boundary classes) to
  `Runtime/Faithful/CampaignHost.cs`. Texts come from the game's own .properties files (all languages) in
  `Resources/TTStrings.txt`. Campaigns are saved to `campaigns.txt` in the persistent data folder. Every island was
  started and run 3 000 ticks headless without errors.
- **Java patch:** `determinism-fixes.patch` in the Java repo root has the determinism fixes (applies cleanly; not applied).
- **Ships:** peons build ships (S); ship panel with Harvest / Army / Transport / Rally / Sail (M); right-click
  with ships selected sails (Picker.pickSailingTarget). Headless test: ship built, peons boarded, sailed, deployed.
- **Map mode:** hold Space (or keypad 5) for the whole island from above; click to go there.
- **Tutorial:** all six tutorials run on the translated `tutorial/*` triggers and `TutorialForm`. What the triggers watch (camera scroll/zoom/rotate/pitch, mouse look, map mode, placing, the panel's sub-menus, the selection) comes from the Unity camera and controller through `UiState`; the instruction box and the "Tutorial completed" dialog are drawn by Unity. New camera controls for it: Home/End or numpad 8/2 pitch, hold F or the middle mouse button to look around.

## Stage 5 — multiplayer ✅ (Unity↔Unity through the Java servers; Java↔Unity cross-play still to test on your PC)

The whole network layer is translated too (534 Java files in all): ARMI, `Connection`/`SecureConnection`,
`NetworkSelector`, `MatchmakingClient`, the game `Server`/`Client`, `WorldStarter`, `WorldViewer`, `PeerHub`,
`RouterClient`. **Every game now starts the Java way**, single player included: `Menu.startNewGame` runs a local
`Server` on the loopback address plus a `Client`; `WorldStarter` builds the `WorldViewer`; `PeerHub` ticks the world.
Campaign islands start through the same path.

- **Runtime (JavaSim/Runtime):** `NetSerial.cs` — Java Object Serialization Stream Protocol (byte-compatible; class
  descriptors with serialVersionUIDs generated from the JDK's `ObjectStreamClass`); `NetReflect.cs` — `Method`/`Proxy`
  over generated tables (IL2CPP-safe, no runtime codegen), threads, wait/notify; `NetSockets.cs` — non-blocking
  `SocketChannel`/`ServerSocketChannel`/`Selector` over System.Net.Sockets, `InetAddress` (`*.localhost` = loopback);
  `NetCrypto.cs` — Diffie-Hellman with X.509/PKCS#8 encodings, SunJCE secret derivation (128-byte padded secret, first
  24 bytes with odd parity → DESede), `DESede/ECB/PKCS5Padding`, `SealedObject`, `MessageDigest`.
- **Generated:** `JavaSim/Generated/NetTables.cs` (by `j2cs/armi/GenNet.java`): the 12 ARMI interfaces' method tables
  (sorted by `Method.toString()` exactly like `ARMIInterfaceMethods`) + proxies, and 169 serialization descriptors.
- **Game layer:** `Core/GameSession.cs` (the Java main loop `AnimationManager.runGameLoop`, game start for skirmish /
  host / join, lobby state; world loading on a worker thread while the screen shows "Loading"), `Core/MatchmakingSession.cs`
  (login, accounts, profiles, chat rooms, game list), `MultiplayerMenu.cs` (Multiplayer tab + lobby), in-game chat
  (Enter = all, Shift+Enter = team). Orders go through `PeerHub.getPlayerInterface()` like the Java GUI.
- **Verified here** against your real `MatchmakingServer` and `RouterServer` (built with an in-memory DB stub instead of
  MySQL, in `/home/claude/srvtest`): encrypted login, accounts, profiles, chat, game registration and list, tunnels,
  routed events, and full 2-player games between two C# clients — lobby, start, orders from both sides, identical world
  hashes on both machines every 250 ticks. Goldens still match after the switch.
- **Server note:** on JDK 25 the servers need `-Djdk.crypto.KeyAgreement.legacyKDF=true` (the game client's gradle file
  already sets it), otherwise the encrypted login fails with "Unsupported secret key algorithm: DESede".
- **Build fingerprint:** the port reports "unknown", which the host never counts as a mismatch.
- **Since then:** spectating (Players tab → Spectate, in sync with the players), whispers, Rankings tab (ladder and OpenSkill lists).

## Game modes and rule options ✅

All four modes of your fork run on the translated rules (`gamemode/*`, King of the Island now translated too):
Standard, King of the Island, King of the Hill, Survival. Their on-screen status labels (`Label` added to the GUI root)
are drawn top-centre by Unity (`UiState.hudLabels`). The skirmish menu has a **Game mode** choice and **More rules**:
starting peons / max units / max buildings, starting warriors, magic on/off and costs, chief's courage, adaptive AI,
teams start together, health and range multipliers, armory cap, and the AdvancedAI tuning values — the same
`WorldParameters` options as your `TerrainMenu`, same defaults (`Core/RuleSet.cs`). Headless check: each mode runs 3000
ticks with its HUD working.

**Multiplayer rules fix ✅ (both the Java game and the port):** joiners used to build their `WorldParameters` only from
the `Game` fields, so mode and rule options never reached them. Now the `Server` sends the host's parameters
(`GameClientInterface.setWorldParameters`) before `setWorldGeneratorAndPlayerSlot`; `Client` accepts them while
connecting/negotiating. Patch for the Java repo: `multiplayer-world-params-fix.patch`. Checked: a King of the Hill game with
3 starting rock warriors and 12 peons gives both players 15 units and identical hashes. The host screen now offers every
mode and the More rules options. Old clients can't join new hosts (new ARMI method), so update both sides together.

## Custom maps ✅

Skirmish and host-game screens have a **Map** picker: "Random island" or any `.ttmap` in
`<persistentDataPath>/maps` (the **Folder** button opens it; copy the maps from your Java game's maps folder there). Maps
made in your Java map editor load through `MapEditor.customMapFromFile`; others go through `CustomMapGenerator`, like
the Java menu. Hosted games send the map to joiners who don't have it (your Server's chunked map transfer). Checked:
`sillymap` and `test3d` skirmishes, plus a 2-player hosted sillymap game where the joiner had no copy — both loaded
the same map and stayed in sync.

Java class-path resources (the map-code dictionary) load from `Resources/TTResources` through `java.lang.JResources.Open`.

## Settings, island options and map codes ✅

- **Settings** (main menu tab, and **Settings** in the in-game menu): master / music / effects volume, screen-edge
  scrolling, scroll speed, invert mouse look, language (system, English, Deutsch, Español, Italiano, Dansk, Português),
  and in builds fullscreen, v-sync, resolution and quality. Saved in PlayerPrefs (`Game/GameSettings.cs`).
- **Island options** now match your TerrainMenu exactly (`Core/MapCode.cs`, `IslandOptionsGUI.cs`, shared by skirmish
  and host game): all six sizes (Small … Unreal, Archipelago with ships), sliders in 0–200 % steps of 10 %, seed
  0–39999 with world seed = seed², and the **word map code** — shown for the island, and **Enter code** reads one.
  Checked: the C# code for an island is the same words your Java `WordsEncoding` gives, and it reads back.
- `Resources/TTStrings.txt` was regenerated from all 559 `.properties` files of your repo (adds the map editor's
  texts, and fixes the Portuguese campaign texts that were double-encoded).

**Found in the Java code:** the map editor's `MapSettings` encodes map codes with 11 slider steps
(`SLIDER_MAX + 1`), but `TerrainMenu` now uses 21 (`SLIDER_CARDINALITY = 21`, sliders 0–20). So a map code copied
from the skirmish menu opens a different island in the editor, and the other way round. **Fixed** (both your Java
fork and the port): `MapSettings` now uses the 0–20 skirmish scale (`SLIDER_MAX = 20`, `SLIDER_UNIT = 10`), so codes
are the same in both menus.

## Stage 7: map editor ✅ (first version)

**Map editor** tab: new map from size / terrain / sliders / seed or a map code, and your saved maps with their previews
(Edit, Play, Delete). The editor screen is the translated Java editor (`TerrainEditor`, `ResourceLayer`, `StartLayer`,
`AccessMap`, `GroundAlphas`, `MapFile`) driven by `Core/MapEditorSession.cs` (the non-GUI half of `MapEditorDelegate`,
`MapEditorLoader`, `ResourceSnapper` and `GroundTextures`' bookkeeping) and drawn by `MapEditorHost.cs`:

- Brushes: Height, Flatten, Smooth, Random, Ramp (drag), Cliff, Plateau; Trees, Palms/Pines, Rock, Iron, Erase;
  Player start (with team) and Auto-place spawns. Shapes circle / square / diamond. Left = apply, right = reverse.
- Ctrl+wheel radius, Shift+wheel intensity, Ctrl+Z / Ctrl+Y undo/redo, Esc menu, Space map view, WASD/arrows/Q/E and
  middle mouse for the camera. Brush outline on the ground, start markers, a playable-area map (click to jump).
- The ground texture follows edits (GroundAlphas → the terrain shader's alpha maps), meshes and the sea colour too.
- Save / Save & play: same gzip `.ttmap` format (version 4) in `<persistentDataPath>/maps`, which the skirmish Map
  picker lists. Checked headless: raise, plant trees, undo/redo, starts, save, reload, then play the map; and your Java
  `MapFile.load` reads the saved file (settings, heights, trees, 4 starts, preview).
- Not ported: the Java editor's flat 2D painting view and wireframe switch.

## Graphics and interface ✅ (Settings → Graphics: Classic / Modern)

**Classic** reproduces the Java renderer: the baked ground (LandscapeBaker in gamma space, detail texture by its
alpha), `SpriteShader` lighting `clamp(0.65 + N·L)`, the sky dome with its two cloud layers, and the sea from
`Water.java` (three Gerstner waves on open sea only, GeneratorOcean textures, depth-faded alpha, Fresnel, reflection).
Rocks and iron use the generated textures (`GeneratorRock` / `GeneratorIron` / `Perlin` now translated).

**Modern** (default) is a physically based look over the same islands, units and rules:
- **Sky** (`Shaders/TTAtmosphere.hlsl`, `Faithful/TTAtmosphere.cs`): Rayleigh + Mie single-scattering sky with a sun
  disc and a lit, self-shadowing cloud layer drifting with the wind; the same model gives the sun colour, sky and
  ground ambient and the haze colour, so everything agrees. Height fog with sun in-scattering; clouds cast shadows.
- **Terrain**: the Java structure colours and alphas, lit in real time (GGX sun, soft + cloud shadows, sky / ground
  ambient, SSAO) instead of the baked light. Each ground type (sand, dirt, gravel, rock, grass, snow) gets a
  1024² detail set from `Resources/TTReal` (albedo variation, normal + roughness; made by
  `PortTools/gen_real_textures.py`), two scales against tiling, height-aware layer transitions, triplanar rock on
  cliffs, large-scale colour variation, wet sand at the waterline, caustics on the shallow sea floor. A sea-floor skirt
  continues the island's floor beyond the map so the sea darkens gradually (no square edge).
- **Water**: six Gerstner waves damped in the shallows, scrolling detail normals, refraction of the scene below with
  Beer-Lambert absorption (turquoise shallows, deep blue / northern green depths), Fresnel sky / planar reflection,
  GGX sun glint, light through wave crests, foam on shores and crests.
- **Buildings, units, trees**: GGX shading with roughness / reflectance read from the painted textures (metal,
  stone, bone shine; cloth, thatch matte), relief from the textures, sky reflections, SSAO, light through leaves,
  wind sway (shadows follow), 8× anisotropic filtering.
- **Post**: ACES tone mapping, bloom on the sun and glints, light grading, SMAA. Shadow distance follows the camera.
- Runs at ~110 fps in the editor at 2400×750 on your PC. The URP asset is upgraded once (rendering v2: full-resolution
  opaque texture for the refraction).

**Interface**: the menus, panels, buttons, check boxes, sliders, fields and scroll bars are redrawn from your
`gui_skin.xml` / `gui.dds` (clay-brown bevelled buttons, clay-framed dark forms with a title bar, the original
palette and dark button text), anti-aliased with drop shadows and hover / pressed / selected states, plus the original
command icons (`icons.xml`) on the HUD with hotkeys, counts and tooltips. The main menu is a framed form over a
slowly orbiting 3D island, with the original logo.

## HD assets, free-form rule, settings ✅

- **HD ground**: the Modern terrain uses 2048² photo sets (Realistic Terrain Textures: grass, soil, dry grass, cracked
  ground) plus generated sand / gravel / rock / snow sets in `Resources/TTReal`.
- **Real trees** (Settings → Graphics → Realistic trees): jungle, oak and pine trees are drawn with the photo-textured
  Mobile_FV models from Stick City (`Resources/TTRealTrees.asset`, built by Tools → Tribal Trouble → Build Real Trees).
  Leaf cards use alpha cutout, light through leaves and fill light. Palms keep the original model (no palm asset found).
- **Real weapons** (Settings → Graphics → Realistic weapons): the thrown Viking axe and Native spear are drawn with the
  MeleeWeaponsPack axe / spear from Stick City (`Assets/ThirdParty/MeleeWeapons`, `Resources/TTRealWeapons.asset`). The
  closest-proportioned model is fitted to the original: same length, head at the same end, blade on the same side.
  Tier atlases (`Resources/TTRealWeapons/*.png`, 2048²): a stone head for rock, steel for iron, the original toy
  colours for rubber. In a game, F8 lays out all six at the cursor (4× size) for checking. Weapons held in the
  warriors' hands are part of the skinned warrior models and stay original.
- **No realistic tribal building models** exist in your projects, so buildings keep the enhanced originals.
- **Round placement**: the placement marker is a disc + rim draped over the ground (green = legal, red = not).
- **Free-form rule** (skirmish menu "Movement / building", Java TerrainMenu "Free-form" check box): round building
  footprints with per-cell ground checks, and string-pulled unit paths (`pathfinder/PathSmoother`). Integer maths only,
  so it stays deterministic. The classic grid stays the default for cross-play with stock Java clients. Java patch:
  `free-form-rule.patch` (applied to the PC fork, with the TerrainMenu strings in all six languages).
- **Settings added**: aggressive units (Ctrl+A in game too), health halos, health bars, compass, invert rotation,
  confine cursor, tooltip delay, render scale, high contrast, colour-blind modes.
- Fixed: the terrain shader failed to compile (duplicate variable names), which showed the ground magenta.
- The ScriptableObject classes now live in their own files (`TTRealTreeSet.cs`, `TTRealWeaponSet.cs`). Unity needs
  this to keep the assets' script link; the builders rebuild an asset whose link was lost.

## Extended rules ✅ (skirmish "Units & buildings: Extended", Java TerrainMenu "Extended")

Off = the original game, bit for bit (all golden replays still match). On adds, in the simulation (Java + C#, so
Java players running the patched game can join):

| | What | Cost / how | Hotkey |
|---|---|---|---|
| **Champion** (unit) | heavy warrior: 3 hit points, 80 % defense, slower (3.4 m/s) | armory, 2 iron weapons, 3 s | Army → H |
| **Scout** (unit) | fast skirmisher: 6 m/s, 75 % range, 35 % defense | armory, 1 rock weapon, 0.75 s | Army → S |
| **Palisade** (building) | ring of sharpened logs that blocks the way; 60 hit points | peons | L |
| **Shrine** (building) | every 2 s heals own units (+1 hp) and repairs own buildings (+3) within 14 m | peons | J |
| **Research** (armory) | Armor (+1 hp for warriors), Swift feet (+10 % speed), Better tools (+25 % gathering), Long throw (+1 m range); 2 levels each, paid from the armory's wood / rock / iron / chicken | armory → Research (E), 1–4 | E |
| **4th spell** | Vikings: War Cry (nearby units +35 % speed, +40 % throw rate, harder to hit, 8 s). Natives: Spirit Dance (heals, +2 m range, harder to hit, 10 s) | chieftain, 60 energy | B |

- No new network messages: research orders reuse `deployUnits` with new `DeployType` values, the spell is magic
  index 3. Java patch: `extended-rules.patch` (applied to the PC fork, strings in all six languages). The Java HUD has
  no buttons for the new things yet; Java players can still play Extended games.
- Looks (Unity): palisade and shrine have their own models (logs from the weapon-pack wood, standing stones from the
  TTReal rock, team-coloured bands, a glowing orb and a healing pulse ring on the shrine); champions are 1.2x with a
  round shield on the back (wood + iron for Vikings, painted hide for Natives); scouts 0.88x.
- **Held weapons**: warriors now hold the real axe / spear too (the painted weapon is cut out of the warrior mesh, the
  real one rides the weapon bone; Tools → Tribal Trouble → Build Real Weapons).
- **Combat effects** (Settings → Graphics): dust where weapons land, sparks from iron, dust and blood when units fall,
  flames, embers and crackle on buildings below half health, a dust cloud when a building collapses.
- Builds: `Editor/BuildSafety.cs` keeps the instancing / alpha-test shader variants and the runtime shaders in player
  builds (Tools → Tribal Trouble → Prepare For Build runs it by hand).

## Original main menu, spinner controls, new icons, big-island performance ✅

- **Main menu** (`FaithfulGame.MainMenuGUI`, from `delegate/Menu.java` + `MainMenu.java`): the original overlay (the
  letterbox bars with the tiki hanging over the top bar, kept 4:3 with the bars carried across wide screens) over the
  orbiting island, the logo at 347x206 x height/600, and the Impact text buttons Tutorial / Campaign / Skirmish /
  Multiplayer / Map editor / Options / Quit from y = 190/600, spaced 0.875 lines. White text, #FFCC9F with a
  1.5 s ±6 % pulse when hovered or focused (`MenuButton.java`), greyed while a form is open. Forms open at
  MENU_X, two thirds down the free space, with Back / OK; Quit asks first (QuitForm). Up/Down/Tab + Enter navigate,
  Esc closes the open form. Discord and GitHub buttons bottom right (your fork's icons and links).
- **Spinners** (`gui/IconSpinner.java`): the icon is "+" (left +1, right +10, Shift = everything) and the small corner
  "−" removes (left −1, right −10, Shift −100). Holding a mouse button or a hotkey repeats. Works on the harvest,
  weapons, army and transport rows.
- **Icons** (`Resources/TTGui/icons_extra.png` + `icons_extra_layout.xml`, 29 icons, each with normal / active /
  disabled like the original atlas; generated by `/home/claude/icons/gen.py` from the original icons and the extended
  textures): back, and per tribe guard, patrol, hold, palisade, shrine, champion, scout, research, the four upgrades,
  Convert (magic 3, which had none) and War Cry / Spirit Dance (magic 4). `TTGui` now reads several atlases. Spell
  buttons have real names (Stun, Sonic blast, Poison fog, Lightning, Convert).
- **Unreal islands**: erosion (most of the generation time) runs as a bit-exact pipelined parallel loop
  (`ChannelErodeFast`, busy-wait handoff — Mono's SpinWait sleeps and stalled it); trees are culled by frustum and
  distance with original models beyond 320 m; terrain chunks drop to coarse meshes beyond 380 m and big maps pick
  from the height map instead of colliders; the editor compiles in Release. First run on your PC: the landscape took
  72 s and the game was playable (logs now print "Island ready after … s" and "View built in … ms").
- **Palms**: six procedural palm shapes (bent ringed trunk, drooping V-folded fronds, HD frond/trunk textures) in
  place of the original palm model for Modern trees; trees vary 0.86–1.12 in size.

## Online: server domain, hosting, invites ✅

- **Server domain** (Options → Multiplayer, like the Java GeneralPanel): Update / Reset; an IP address or `localhost` is used
  as-is (no `matchmaking.` / `router.` prefix). The same change is in the Java fork (`Settings.isDirectAddress`).
- **Host a server from the game** (Multiplayer → Friends): your matchmaking server and router, translated to C#, run inside the
  game (`Core/PlayerServer.cs`, `EmbeddedServer`, in-memory accounts). Friends join by IP, through **Steam** (P2P sockets, no
  ports to open) or through **Unity Relay** (a short join code). Both tunnel the game's two TCP ports (`Core/Tunnel.cs`).
- **Dedicated server**: run the build with `-server` (or `-batchmode`); it hosts the same server headless.
- **Invites**: Steam rich presence + invite overlay; **Discord** (Rich Presence over the local Discord pipe, Join / Ask to join);
  the relay code can be copied and pasted. Log in as guest, or with Steam.
- **Needs**: Steam uses app id 480 (Valve's test app) until you have your own; Discord needs an application id from
  discord.com/developers; Unity Relay needs the project linked to a Unity Cloud project with Relay turned on.
- Checked here: two C# clients played a full game on the embedded server with matching hashes; a joiner on an isolated network
  played through the tunnel in sync.

## Extended rules, pass 2 ✅

- **Units**: **Shaman** (Natives) / **Skald** (Vikings) — every 3 s within 5 cells the Shaman heals own units 1 hp, the Skald's
  song gives the Courage buff (faster, quicker throws, harder to hit); orb above the head. **Slinger** — long range (1.4x),
  fast, fragile. Army hotkeys M and L.
- **5th spell**: Natives' *Ancestor Shield*, Vikings' *Berserk* (key X). Fixed: the 4th spell was never enabled.
- **Upgrades on every building**: Quarters → Growth (+30 % breeding per level); Tower → Range (+2 m) and Armor (ignores every
  5th/4th hit); Shrine → Blessing (more healing, bigger radius); Chieftain's lore (+25 % magic regen); Walls (palisade/shrine
  armor). Paid from the building, or from your first armory with enough materials. Hotkeys U and O.
- **Hit points x2** for every unit except peons and chieftains (Extended only).
- **Walls, point to point** (like Age of Empires): with Palisade selected, press on the start, drag, release on the end; posts
  are placed along the line (up to 60) as queued build orders.
- Original rules unchanged (all golden replays match).

## Big-island performance, pass 2 ✅

- **Impostors** for the realistic trees and palms: each variant is photographed once (front + side) and far trees are two
  crossed cards with that picture — the same trees all the way to the horizon, no switch back to the original models.
  Fixed: the leaves were lost in the capture (tiny mips; now sampled at full detail with coverage-preserving mips) and the
  picture came back upside down on DirectX (an orientation marker now detects and corrects it).
- **Culling**: static props (plants, rocks, scenery) in element groups outside the view are skipped.
- **Tree cache**: standing trees' matrices and models are worked out once instead of every frame.
- Unreal map on your PC: 31–36 fps → about 66 fps at the normal view (build 12 ms → 3 ms), ~43 fps zoomed out with
  3,400 trees, ~27 fps with 11,000 trees in view. F9 shows the counters.

## The original interface, translated ✅ + melee units ✅

**The Java GUI now runs in Unity.** All menus and panels are your fork's own Java code, translated by J2CS the same
way as the simulation. That covers `gui`, `form`, `delegate`, `guievent`, `font`, `input`, `camera` and `mapeditor`,
built into `TTGui.dll` by `guiport/build.sh`. Unity hosts these screens and draws their quads, and no longer redraws
look-alikes:

- **Main menu, Tutorial and Campaign forms, Options and the skirmish `TerrainMenu`** are exactly the Java screens. The
  skirmish screen has every check box (Free-form, Extended), slider, AI and team choice and the map picker. The
  Loading screen, the in-game `ActionButtonPanel`, the spinners and the tooltips are the Java ones too.
- **In game:** selection rings, responding targets, click rings, building ghosts and the rally flag are drawn by
  `Faithful/TTWorldOverlay.cs` with the `TribalTrouble/WorldMarker` shader. Wall ghosts are drawn per post.
- **Map editor:** the original editor form and screen (`MapEditorDelegate`, brushes, undo, the playable-area tint
  and the menu). `BrushRenderer`, `AccessOverlay` and `GroundTextures` are GL-free overrides. They hand dots, heights
  and ground alphas to Unity through `GuiPort.EditorView`.
- **Options → "Modern graphics..."** (a new button left of About) opens the Unity graphics settings in a window
  over the Java form.
- **Threading:** worlds load on a worker thread, so texture loads, `Resources.Load` and the `Screen` reads are
  marshalled to the main thread (`FaithfulGame.OnMain`). Textures are made the first time something draws them.
- **Extended panel (Java fork and port):** buttons for Palisade (L), Shrine (J), Research (E) at the armory,
  quarters, tower, shrine and palisade, and spells 4 and 5. The army page has a second column for Champion (H),
  Scout (S), Shaman (M), Slinger (L) and the melee units. New icons live in `gui/icons_extended.xml` and `.png`, and
  `ExtendedIcons` / `ResearchButton` are new. Walls are dragged point to point in `PlacingDelegate`. So Java players
  now have every Extended button as well.

**Melee units** (Extended rules only; armory → Army):

| | Natives | Vikings | Cost | Stats | Key |
|---|---|---|---|---|---|
| Light | Club Warrior | Axeman | 1 rock weapon, 0.75 s | speed 5, 2 hp, 45 % defense, 1 damage (2 vs buildings) | K |
| Heavy | Spear Guard | Huscarl | 1 iron weapon, 1.5 s | speed 3.8, 4 hp, 65 % defense, 2 damage (4 vs buildings), front shield | D |

- They strike adjacent targets (`weapon/MeleeFactory`). A dodged blow still lands 35 % of the time, and buildings
  take every blow. They count as warriors of their weapon's type, so armor research, healing, entering towers and
  the AIs all treat them as warriors (`Abilities.MELEE`, `isWarrior()`).
- Headless balance check (`guiport/tests/MeleeSmoke.cs`): about even against throwers in a close fight. Throwers win
  at range.
- Original rules unchanged: all 8 golden replays still match.
- In the Unity editor, **F8** sets up a test skirmish (armories for both sides, then melee vs throwers).

**Still open:** the Friends-hosting entry in the Java multiplayer flow, the editor's wireframe switch, and moving the
old headless tests over to the Java-GUI path.

## Big games: 80 players on an Unreal island ✅ (crash fix)

The 80-player Unreal match (1 human + 79 Expert AIs) froze, then died. Two causes, both fixed in the Java fork and the port:

- **Memory.** Every Expert AI kept its own copy of the map analysis: the supply scan with a 2049² tree table, and
  whole-map distance fields allocated even for 220 m searches (16 MB each on a 2048 map). 79 copies came to
  5–8 GB, with gigabytes of new garbage every 30 s, which is enough to bring down the game's Mono heap.
  - The AIs now share one supply scan per tick. A scan from earlier in the same tick is reused, minus any supplies
    emptied since, which gives exactly what a fresh walk of the grid would.
  - A distance field stores only the square its search can reach.
  - Whole-map fields are kept in 16 bits.
  - Supplies are bucketed by 8×8 (trees) or 32×32 (iron, rock) cells, and nearest-supply searches go outwards ring
    by ring with an exact cutoff.
  - The AI makes exactly the same decisions as before. The Expert golden replays still match the old Java code,
    including a new 6-Expert-AI game (`gout/expert6.gz`, 15 000 ticks). `guiport/tests/ScanCheck.cs` compares the
    shortcuts against brute force.
- **Frames that never ended.** When the simulation fell behind, `AnimationManager.runGameLoop` tried to catch up on
  up to 30 s of backlog in one frame. Each frame took longer than the last until the window stopped responding.
  A frame now simulates for at most 80 ms of real time and drops the rest of the backlog: the game runs slower but
  keeps drawing. Replays and goldens are unaffected.
- **Results** (headless, this machine, 80 players):

  | Island | Memory at start | Game speed | Frames |
  |---|---|---|---|
  | Enormous, before | 1.9 GB | ~0.8x | 4 per 10 s (2–5 s each) |
  | Enormous, after | 0.5 GB | ~0.7–1x | 30–80 per 10 s |
  | Unreal, before | 4.7 GB and climbing during load; 1 game minute in 9 minutes | | |
  | Unreal, after | 2.4 GB managed, flat | ~0.7x | responsive (most frames < 1 s) |

- **Also fixed in the Unity host:** the game now runs while its window is in the background
  (`Application.runInBackground`). World loads no longer give up after 60 s waiting for the main thread.
- **Test harness:** `guiport/tests/ThreadedBoot.cs` runs the GUI port the way Unity does: loads on a worker thread,
  textures and resources marshalled to the main thread, and a deadlock watchdog. `Big(players, size, seconds)`
  starts a skirmish through the real `TerrainMenu` and reports speed, frame times and memory.
  `tools/prof_patch.py` adds per-section timing to a test build.

## Hotkeys, screenshots, free-form ✅

- **Hotkeys:** no key binding ever matched in the port. `InputBinding.matches` compares the binding's modifier
  `EnumSet` with the key event's, and the runtime shim's `TreeSet`/`EnumSet` (and some other `Set`s) compared by
  reference instead of by contents. Every `Set` in `jrt` now has Java set equality and hash codes.
  - `guiport/tests/KeyAudit.cs` presses every bound key in every context: nothing selected, peons, warriors, the
    chieftain, each building and each page of its panel. All in-game actions are taken.
  - `MenuKeyAudit` covers menu and form navigation.
  - The goldens still match.
- **Screenshots** (Ctrl+S, and Ctrl+P, which the default bindings also give to batch peon training) threw from a stub.
  They now save `<persistentDataPath>/screenshots/screenshotNNNNNN.png` (`PortHooks.ScreenshotImpl`).
- **Free-form:**
  - The rule already reached the simulation. Units walk straight: a 26 m walk at an awkward angle strays 0.6 m from
    the line, against 5 m with the classic grid.
  - The rule didn't show, though. Placing now draws the building's round footprint (green / red) in free-form
    games instead of the grid of site tiles.
  - The ground a building flattens (a round pad in free-form) now re-meshes the Unity terrain in every game, not
    just the map editor (`WorldViewer.patchesEdited` → `EditorView.GroundEdited`, 32-cell tiles, 8 per frame).
- **Erosion:** this now uses half the cores at most, and one thread below 1024². Slow runs (> 3 s) are logged, to
  track down the ~40 s main menu island seen in your build. All 10 island conformance cases still pass.

## Right-click on buildings, map-mode team squares, shields ✅

- **Picking:** models have no sprite bounding boxes in the port (the GL sprite renderer filled them in Java). So
  `Picker.getNearestPick` (distance to the box centre) never chose a unit or building. Hover, single-click select and
  right-click orders on units or buildings all fell through to trees or the ground.
  - `PortPicker.cameraDistanceSquared` measures to the middle of the selection box (or the position) when there are
    no bounds.
  - Rocks, iron, chickens and scenery get a box of their usual size.
  - Checked (`tests/RightClickProbe.cs`): peons right-clicked onto an unfinished quarters build it, and a warrior
    right-clicked onto a tower goes in. In Unity, hovering an armory shows its ring and name.
- **Map mode:** like Java's no-detail sprites, every living unit and building is drawn as a square in its team colour
  (template no-detail size, no depth test) while the camera is in no-detail mode (`TTWorldOverlay.MapMarkers`).
- **Shields** (champions on the back, heavy melee on the left forearm) now ride a bone: `Spine1` / `L Forearm`,
  placed with the bone's bind pose. Before, they stood still on the unit's root while the body animated through them.
- Still open: the main-menu island takes ~40 s to generate in Unity (seen in the editor too), and it isn't the
  erosion.

## Extended rules, second batch: new resources, building tiers, race content ✅
Everything here is behind the **Extended** check box; all nine golden recordings still match tick for tick, so the
original rules and multiplayer compatibility with them are unchanged.
- **Free-form**: buildings now land exactly under the cursor (off-grid offset sent as `placeBuildingFree`), with smaller
  round footprints (tower 1 cell, armory/quarters 5–7) and builders reaching the round edge.
- **Stuck units**: a walk that hasn't moved for 5 s sidesteps to a free cell and carries on (up to 4 times) instead of
  dropping the order — group moves through a packed base went from 514/592 to 558/592 arriving.
- **New resources**: gold, crystal, herbs, coal. Deposits are laid next to the map's rock fields (coal, herbs) and iron
  fields (gold, crystal), kept clear of start spots; mined like iron, stored at the armory, grow back. Armory: a second
  harvest column (G / Y / H / K) and four more stock counters.
- **Building tiers** (every building): Stone (10 wood, 20 rock, 30 s: +50 % HP, ignores some hits, towers +1.5 m) →
  Iron (10 wood, 10 rock, 15 iron, 5 gold, 5 coal, 45 s: double HP, more armour, towers +3 m). **Chicken barbs** (5 wood,
  4 chicken, 20 s): every other hit taken stings the attacker back. Buttons left of the building's panel; Unity draws a
  stone footing, an iron band with spikes and bone barbs with red feathers.
- **Race halls**: Natives' *Spirit Lodge* (slows enemies near it, cures your units) and Vikings' *Mead Hall* (your units
  near it heal and fight 15 % faster). Build key O. Each researches the race upgrades.
- **Race units** (armory, third column, need a finished hall): Natives — *Blowgunner* (poison darts, rock weapon + 2
  herbs, U) and *Jaguar Warrior* (very fast melee, iron weapon + 2 gold + 1 crystal, J); Vikings — *Fire Thrower*
  (burning axes, rock weapon + 2 coal) and *Berserker* (hits harder below half health, iron weapon + 3 gold).
- **Race upgrades**: Natives — Poison Tips (throwers poison), Herbal Remedies (units regenerate); Vikings — Fire Axes
  (throwers set targets alight), Battle Fury (+15 % attack speed per level).
- **Sixth spell** (I, costs 2 crystal): Natives' *Healing Rain*, Vikings' *Fire Storm*.
- **Poison / fire** wear units and buildings down 1 (buildings: 2–3) every 1.5 s; Unity shows green bubbles / flames.
- **Expert AI**: sends peons to the new resources near its base, rebuilds towers/armories in stone then iron, barbs its
  towers, builds its race hall, researches the race upgrades and trains race units.
- Icons: `forkpush/gen_rare_icons.py` adds 19 buttons and 4 status icons to `icons_extended.png/.xml`.
- Test: `[J2CSTests.RareProbe]::Run(1)` (headless: deposits, gathering, tiers, barbs, hall, race units, afflictions,
  spell, HUD rows) and `::Ai(minutes)` (an Extended game left to the AIs).

## Crash on the biggest islands, middle-mouse look ✅ (pending a build to confirm)
- **Crash**: on an Enormous island, zooming out or panning over a dense forest made the AMD driver reset ("Device
  removed 887a0006") — reproduced with and without Extended, so it wasn't the new content. One frame was getting tens of
  thousands of full tree models plus their shadows, and the shadow distance followed the camera up to 1500 m. Now: at
  most 2500 full tree models per frame (the full-model radius shrinks while over budget and grows back), at most 2000
  trees cast shadows (within 160 m), and the shadow distance is capped at 600 m.
- **Middle mouse (first-person look)**: the Windows build now does what the original does — the hidden cursor is put
  back where you pressed after every move and reappears there — instead of locking it to the screen centre (which made
  the view jump on press and the cursor jump to the middle on release). The editor keeps the locked-cursor fallback.

## Two games in one, day / night, new upgrades, unique weapons, modern interface ✅ (pending a build to confirm)
- **Launcher**: at start you pick *Tribal Trouble* (the classic game: original rules and look; the skirmish form has no
  Free-form / Extended boxes; no day / night) or *Tribal Trouble: New Tides* (Extended ticked by default, modern
  interface, day / night). "Remember my choice" and Settings → *At start* (Ask / classic / New Tides); `-classic` /
  `-newtides` on the command line. Java side: `global/Edition.java`; Unity: `Game/Edition.cs`, `FaithfulGame.BootGui`.
- **Resources invisible up close**: every static prop (rocks, iron, the new deposits, plants) was culled whenever the
  map's top-right corner was off screen — the boundary `SpriteKey.getBounds` returned empty bounds, so they all sat in
  one corner leaf of the element tree. Fixed with real default bounds.
- **Day / night** (Settings → Time of day, New Tides only): Day (the original sun), Night (moonlight), or a 24-hour
  cycle at one real minute per game hour with a clock at the top. The sun rises in the east and stands where the
  original sun is at about 16:00; at night a dim blue moon lights the island. Modern look: the sky model, sun and
  ambient follow the hour; classic look: the picture is graded (warm dusk, blue night). `TTDayNight.cs`.
- **Old spells look like the original again**: additive particles were drawn with their alpha applied twice (too faint),
  particle quads ignored the particles' per-axis size, lightning was one camera-facing strip instead of the original
  two crossed quads, and the Sonic Blast was a plain ring — now a port of the original shock-ring shader
  (`TTSonicBlast.shader`). **New spells**: Fire Storm gets a burning rune ring, flames bursting from the ground, embers
  and a dark cloud; Healing Rain a glowing green ring and rising motes. **Convert** has its own icon.
- **Every new warrior has his own weapon and look** (`TTUniqueArt.cs`): Blowgunner — blowgun; Jaguar Warrior —
  obsidian-edged macuahuitl and a jaguar pelt; Club Warrior — spiked club; Spear Guard — long obsidian spear; Shaman —
  feathered staff and feather cloak; Scout — javelin / hatchet; Slinger — sling; Fire Thrower — glowing ember axe;
  Berserker — two bearded axes and a wolf pelt; Axeman — bearded axe; Huscarl — Dane axe and a red cloak; Skald — rune
  staff with a horn and a blue cloak; champions — a cape in the team colour. Projectiles too: darts, sling stones,
  spirit bolts, burning axes with flames. The weapons are fitted onto the painted one's frame, which *Build Real
  Weapons* now stores (version 5 — it rebuilds by itself when the editor opens).
- **Nine more upgrades** (21 in all; armor, speed, tools, range and growth now have a third level): Weapon Smithing,
  Drill Yard (armory); Sturdy Peons, Woven Baskets, Chieftain's Vigor (quarters); Volley (tower); Sharpened Stakes
  (palisade); Crystal Focus, Prospecting (race hall). The AI researches them too. A building researches one thing at a
  time.
- **Pathfinding** (Extended / Free-form): blocked walkers look again after 0.25 s (max 1.5 s, was 0.5–5 s), an idle unit
  of yours in the way steps aside, and a group's formation places are handed out closest-first so the group doesn't
  cross through itself. Stuck probe: group moves 282/282 arrived, single moves 120/120, nobody stuck.
- **Modern interface** (New Tides): slate-glass forms and buttons (`gui_modern.png`, same layout as the original skin)
  and dark glass panels behind the in-game button groups. Also fixed: the building-tier buttons could sit on top of the
  armory's status icons (the panel wasn't re-laid out after a selection).
- Tools: `guiport/tests/GuiSnap.cs` + `guiport/tools/gui_render.py` draw the Java GUI headless to PNG
  (`[J2CSTests.HudShots]::Run(dir, 1920, 1080, $true)`); `RareProbe.Upgrades(1)`; icons `forkpush/gen_icons_v3.py`.
