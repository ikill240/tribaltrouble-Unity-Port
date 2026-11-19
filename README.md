# Tribal Trouble — Unity 6.6 port

A staged port of `tribaltrouble-main`. See `PORT_ROADMAP.md` for the plan.

## Open it (Stage 0)

1. **Unity Hub → Add → Add project from disk** → choose this folder → open it with a **Unity 6.1 (6000.1.x)** editor.
   The first import takes a few minutes: Unity fetches URP and glTFast and imports 76 `.glb` models.
2. On first load, **Tools → Tribal Trouble → Setup Project** runs by itself. It creates and assigns a URP asset and switches to Linear
   colour space. Check the Console for `[Tribal Trouble] Project setup done`. You can run it again from the menu at any time.
3. **Tools → Tribal Trouble → Build Sprite Catalog**. This makes one definition per model and `Resources/SpriteCatalog.asset`.
4. **Tools → Tribal Trouble → Create Model Viewer Scene**, then press **Play**.
   - Use Up/Down or the left list to pick a model, and the buttons on the right to play an animation.
   - Space pauses; while paused, a slider lets you scrub through the animation.
   - You can also switch LOD and texture (rock / iron / rubber) and pick a team colour.
   - Right-drag orbits, the mouse wheel zooms and middle-drag pans.

The viewer uses the classic Input Manager. If the Console shows errors about `UnityEngine.Input` every frame, open
**Project Settings → Player → Active Input Handling** and set it to **Input Manager (Old)** or **Both**.

If the URP auto-setup fails, use **Assets → Create → Rendering → URP Asset (with Universal Renderer)**. Then assign the new asset in
**Project Settings → Graphics** and **Quality**.

## Re-importing assets from the Java repo

```
python PortTools/import_tt_assets.py "C:\Users\tre24\Desktop\ttremastered\tribaltrouble-main(Working)1.4.1"
```

The script needs Python 3 and numpy. It overwrites `Assets/TribalTrouble/Art`, `Audio` and `Localization/Source`.

## Layout

```
Assets/TribalTrouble/
  Art/Models/<group>/<sprite>.glb   converted geometry + skin + animations, plus catalog.json
  Art/Textures/...                  original PNGs (team decals imported as linear masks)
  Audio/Sfx, Audio/Music            original OGGs
  Localization/Source               original .properties translations (as .txt)
  Shaders/TTSprite.shader           URP port of TT's sprite shader (team colour blend)
  Scripts/Sim                       deterministic simulation (no UnityEngine), e.g. JavaRandom
  Scripts/Runtime                   presentation: sprite views, viewer
  Scripts/Editor                    setup, import rules, catalog builder
PortTools/                          converter + verification scripts (Python)
```
