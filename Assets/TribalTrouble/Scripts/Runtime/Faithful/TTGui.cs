using System.Collections.Generic;
using UnityEngine;

namespace TribalTrouble.FaithfulGame
{
    /// <summary>
    /// The game's look for menus and the HUD: the original icon atlas (Resources/TTGui/icons + icons.xml, the same
    /// regions as your Java gui/icons.xml) and a modern IMGUI skin — rounded translucent panels, warm wood and gold
    /// accents, readable system font, buttons with hover and pressed states, visible slider tracks.
    /// </summary>
    public static class TTGui
    {
        public static readonly Color Gold = new Color(1f, 0.82f, 0.42f);
        public static readonly Color Text = new Color(0.96f, 0.94f, 0.9f);
        public static readonly Color Muted = new Color(0.72f, 0.68f, 0.62f);

        static GUISkin s_skin;
        static Font s_font;
        static Texture2D s_icons;
        static readonly Dictionary<string, Rect[]> s_regions = new Dictionary<string, Rect[]>();
        static readonly Dictionary<string, Texture2D> s_regionTex = new Dictionary<string, Texture2D>();
        static Texture2D s_panel, s_panelStrong, s_form, s_glow;
        // palette sampled from the original gui.dds (gui_skin.xml): clay bevel buttons, dark text
        static readonly Color ClayA = Hex(0x83685A), ClayB = Hex(0xB89585);          // top / bottom of the fill
        static readonly Color ClayHA = Hex(0xA88877), ClayHB = Hex(0xD6B4A6);        // hover ("active" in the skin)
        static readonly Color ClayOnA = Hex(0xC9A28C), ClayOnB = Hex(0xE6C8B4);      // selected / toggled on
        public static readonly Color Dark = Hex(0x24170F), Darker = Hex(0x120A05);
        const int M = 6;   // panel shadow margin (texture pixels outside the frame)
        const int BM = 3;  // button shadow margin
        static readonly RectOffset PanelBorder = new RectOffset(M + 12, M + 12, M + 12, M + 12);
        public static Texture2D Logo { get; private set; }
        public static Texture2D MenuArt { get; private set; }
        const float IconsW = 1131f, IconsH = 1024f;

        // ================================================================== skin

        /// <summary>The skin; call from OnGUI (builds on first use).</summary>
        public static GUISkin Skin
        {
            get
            {
                if (s_skin == null) Build();
                return s_skin;
            }
        }

        public static Font Font => s_font;
        /// <summary>Compact one-line button for tab rows.</summary>
        public static GUIStyle Tab { get; private set; }

        static void Build()
        {
            LoadIcons();
            try { s_font = Font.CreateDynamicFontFromOSFont(new[] { "Segoe UI", "Segoe UI Variable Text", "Helvetica Neue", "Arial" }, 14); } catch { s_font = null; }
            var src = GUI.skin;
            var k = ScriptableObject.CreateInstance<GUISkin>();
            k.hideFlags = HideFlags.DontSave;
            k.font = s_font != null ? s_font : src.font;

            // ---- the original gui_skin.xml look (clay-brown bevelled buttons and frames around dark translucent
            // fields), redrawn anti-aliased with soft drop shadows, rounder corners and clear hover / pressed states.
            s_panel = Frame(64, 64, M, 7, 4, 4, ClayA, ClayB, new Color(0.035f, 0.03f, 0.025f, 0.80f), new Color(0.02f, 0.017f, 0.014f, 0.86f));
            s_panelStrong = Frame(64, 64, M, 8, 6, 6, ClayA, ClayB, new Color(0.04f, 0.033f, 0.027f, 0.93f), new Color(0.02f, 0.017f, 0.014f, 0.95f));
            s_form = Frame(80, 96, M, 9, 6, 30, ClayA, ClayB, new Color(0.04f, 0.033f, 0.027f, 0.93f), new Color(0.02f, 0.017f, 0.014f, 0.95f));
            s_glow = Ring(48, 48, 10, 2.2f, new Color(1f, 0.86f, 0.55f, 0.95f));

            var btnN = Clay(40, 40, BM, 7, ClayA, ClayB, false);
            var btnH = Clay(40, 40, BM, 7, ClayHA, ClayHB, false);
            var btnA = Clay(40, 40, BM, 7, ClayA * 0.92f, ClayB * 0.95f, true);
            var btnOn = Clay(40, 40, BM, 7, ClayOnA, ClayOnB, true, Gold);
            var btnOnH = Clay(40, 40, BM, 7, ClayOnA * 1.04f, ClayOnB * 1.04f, true, new Color(1f, 0.92f, 0.65f));
            var btnD = Clay(40, 40, BM, 7, Gray(ClayA, 0.55f), Gray(ClayB, 0.55f), false);
            var field = Frame(32, 32, 1, 5, 3, 3, ClayA, ClayB, new Color(0, 0, 0, 0.62f), new Color(0, 0, 0, 0.7f), 0f);
            var fieldF = Frame(32, 32, 1, 5, 3, 3, ClayHA, ClayHB, new Color(0.02f, 0.015f, 0.01f, 0.78f), new Color(0, 0, 0, 0.82f), 0f, Gold);
            var track = Groove(24, 8);
            var thumb = Clay(16, 26, 1, 4, ClayA, ClayB, false);
            var thumbH = Clay(16, 26, 1, 4, ClayHA, ClayHB, false);
            // the box sits in the left 22 px; the rest of the texture is transparent and stretches under the label
            var check = Pad(Clay(22, 22, 1, 5, ClayA, ClayB, false), 26);
            var checkH = Pad(Clay(22, 22, 1, 5, ClayHA, ClayHB, false), 26);
            var checkOn = Pad(Tick(Clay(22, 22, 1, 5, ClayA, ClayB, true)), 26);
            var checkOnH = Pad(Tick(Clay(22, 22, 1, 5, ClayHA, ClayHB, true)), 26);
            var scrollTrack = Frame(14, 24, 0, 5, 2, 2, ClayA, ClayB, new Color(0, 0, 0, 0.45f), new Color(0, 0, 0, 0.55f), 0f);
            var scrollThumb = Clay(14, 24, 1, 5, ClayA, ClayB, false);
            var scrollThumbH = Clay(14, 24, 1, 5, ClayHA, ClayHB, false);

            k.label = new GUIStyle(src.label) { font = k.font, fontSize = 14, richText = true, wordWrap = true, normal = { textColor = Text } };
            k.box = new GUIStyle(src.box) { normal = { background = s_panel, textColor = Text }, border = PanelBorder, overflow = new RectOffset(M, M, M, M), padding = new RectOffset(10, 10, 8, 8), font = k.font, fontSize = 14, richText = true };
            k.window = new GUIStyle(k.box) { padding = new RectOffset(16, 16, 34, 14), normal = { background = s_form, textColor = Dark }, onNormal = { background = s_form, textColor = Dark }, border = new RectOffset(M + 14, M + 14, M + 34, M + 14), contentOffset = new Vector2(0, -27), fontStyle = FontStyle.Bold, fontSize = 15, alignment = TextAnchor.UpperCenter };
            k.button = Btn(src.button, k.font, btnN, btnH, btnA, btnOn, btnOnH, btnD);
            k.toggle = new GUIStyle(src.toggle)
            {
                font = k.font, fontSize = 14, richText = true,
                normal = { background = check, textColor = Text }, hover = { background = checkH, textColor = Gold }, active = { background = checkOnH, textColor = Gold },
                onNormal = { background = checkOn, textColor = Text }, onHover = { background = checkOnH, textColor = Gold }, onActive = { background = checkH, textColor = Gold },
                border = new RectOffset(22, 3, 0, 0), padding = new RectOffset(28, 4, 2, 2), overflow = new RectOffset(0, 0, 0, 0), fixedHeight = 22,
                imagePosition = ImagePosition.ImageLeft,
            };
            k.toggle.contentOffset = new Vector2(0, -1);
            k.textField = new GUIStyle(src.textField)
            {
                font = k.font, fontSize = 14, normal = { background = field, textColor = Text }, hover = { background = field, textColor = Text },
                focused = { background = fieldF, textColor = Color.white }, onNormal = { background = field, textColor = Text }, onFocused = { background = fieldF, textColor = Color.white },
                border = new RectOffset(7, 7, 7, 7), padding = new RectOffset(9, 9, 6, 6), overflow = new RectOffset(1, 1, 1, 1),
            };
            k.textArea = new GUIStyle(k.textField) { wordWrap = true };
            k.horizontalSlider = new GUIStyle(src.horizontalSlider) { normal = { background = track }, border = new RectOffset(4, 4, 3, 3), fixedHeight = 8, margin = new RectOffset(4, 4, 11, 8), padding = new RectOffset(-1, -1, 0, 0) };
            k.horizontalSliderThumb = new GUIStyle(src.horizontalSliderThumb) { normal = { background = thumb }, hover = { background = thumbH }, active = { background = thumbH }, fixedWidth = 14, fixedHeight = 22, border = new RectOffset(6, 6, 6, 6), overflow = new RectOffset(1, 1, 1, 1) };
            k.verticalSlider = src.verticalSlider; k.verticalSliderThumb = src.verticalSliderThumb;
            k.verticalScrollbar = new GUIStyle(src.verticalScrollbar) { normal = { background = scrollTrack }, border = new RectOffset(6, 6, 6, 6), fixedWidth = 14, margin = new RectOffset(3, 1, 0, 0), padding = new RectOffset(0, 0, 0, 0) };
            k.verticalScrollbarThumb = new GUIStyle(src.verticalScrollbarThumb) { normal = { background = scrollThumb }, hover = { background = scrollThumbH }, active = { background = scrollThumbH }, border = new RectOffset(6, 6, 7, 7), overflow = new RectOffset(1, 1, 1, 1), fixedWidth = 14 };
            k.verticalScrollbarUpButton = new GUIStyle { fixedHeight = 0 };
            k.verticalScrollbarDownButton = new GUIStyle { fixedHeight = 0 };
            k.horizontalScrollbar = new GUIStyle(src.horizontalScrollbar) { normal = { background = scrollTrack }, border = new RectOffset(6, 6, 6, 6), fixedHeight = 14 };
            k.horizontalScrollbarThumb = new GUIStyle(src.horizontalScrollbarThumb) { normal = { background = scrollThumb }, hover = { background = scrollThumbH }, active = { background = scrollThumbH }, border = new RectOffset(6, 6, 6, 6), fixedHeight = 14 };
            k.horizontalScrollbarLeftButton = new GUIStyle { fixedWidth = 0 };
            k.horizontalScrollbarRightButton = new GUIStyle { fixedWidth = 0 };
            k.scrollView = new GUIStyle(src.scrollView);
            k.settings.cursorColor = Gold;
            k.settings.selectionColor = new Color(1f, 0.8f, 0.4f, 0.35f);
            k.settings.doubleClickSelectsWord = true;
            k.customStyles = src.customStyles;
            Tab = new GUIStyle(k.button) { fontSize = 13, wordWrap = false, clipping = TextClipping.Clip, padding = new RectOffset(4, 4, 4, 5) };
            s_skin = k;
        }

        static GUIStyle Btn(GUIStyle src, Font f, Texture2D n, Texture2D h, Texture2D a, Texture2D on, Texture2D onH, Texture2D d)
        {
            var s = new GUIStyle(src)
            {
                font = f, fontSize = 14, richText = true, wordWrap = true, alignment = TextAnchor.MiddleCenter,
                normal = { background = n, textColor = Dark }, hover = { background = h, textColor = Darker }, active = { background = a, textColor = Darker },
                focused = { background = n, textColor = Dark },
                onNormal = { background = on, textColor = Darker }, onHover = { background = onH, textColor = Darker },
                onActive = { background = a, textColor = Darker }, onFocused = { background = on, textColor = Darker },
                border = new RectOffset(BM + 8, BM + 8, BM + 8, BM + 8), overflow = new RectOffset(BM, BM, BM, BM),
                padding = new RectOffset(10, 10, 5, 6), margin = new RectOffset(3, 3, 3, 3), fontStyle = FontStyle.Bold,
            };
            s_disabledButton = d;
            return s;
        }
        static Texture2D s_disabledButton;

        // ---- procedural textures (signed-distance painting, so edges are anti-aliased at any size)

        static Color Hex(int rgb) => new Color(((rgb >> 16) & 255) / 255f, ((rgb >> 8) & 255) / 255f, (rgb & 255) / 255f, 1f);
        static Color Gray(Color c, float k) { float l = c.r * 0.3f + c.g * 0.59f + c.b * 0.11f; var g = Color.Lerp(new Color(l, l, l, 1), c, 0.35f); g.a = k; return g; }
        static Texture2D NewTex(int w, int h) => new Texture2D(w, h, TextureFormat.RGBA32, false) { wrapMode = TextureWrapMode.Clamp, filterMode = FilterMode.Bilinear, hideFlags = HideFlags.DontSave };

        /// <summary>Signed distance to a rounded rectangle (top-down pixel coordinates, inside negative).</summary>
        static float SdRound(float px, float py, float x0, float y0, float x1, float y1, float r)
        {
            float cx = (x0 + x1) * 0.5f, cy = (y0 + y1) * 0.5f;
            float qx = Mathf.Abs(px - cx) - ((x1 - x0) * 0.5f - r), qy = Mathf.Abs(py - cy) - ((y1 - y0) * 0.5f - r);
            return new Vector2(Mathf.Max(qx, 0), Mathf.Max(qy, 0)).magnitude + Mathf.Min(Mathf.Max(qx, qy), 0) - r;
        }
        static float Cov(float d) => Mathf.Clamp01(0.5f - d);
        static Color Over(Color dst, Color src)
        {
            float a = src.a + dst.a * (1 - src.a);
            if (a <= 1e-5f) return new Color(0, 0, 0, 0);
            var c = (src * src.a + dst * dst.a * (1 - src.a)) / a;
            c.a = a;
            return c;
        }
        static Color Shadow(float d, float blur, float strength) => new Color(0, 0, 0, strength * (1 - Mathf.SmoothStep(0, 1, Mathf.Clamp01((d + 1f) / blur))));

        /// <summary>
        /// A clay button like gui_skin.xml's horiz_button: black outline, light top bevel, dark bottom bevel (swapped
        /// when pressed), vertical gradient fill, soft drop shadow in the <paramref name="m"/> pixel margin.
        /// </summary>
        static Texture2D Clay(int w, int h, int m, float r, Color top, Color bottom, bool pressed, Color? inner = null)
        {
            var t = NewTex(w, h);
            var px = new Color[w * h];
            float x0 = m, y0 = m * 0.5f, x1 = w - m, y1 = h - m * 1.5f;
            var outline = new Color(0.03f, 0.02f, 0.01f, 1);
            var hi = Color.Lerp(bottom, Color.white, 0.35f);
            var lo = Color.Lerp(top, Color.black, 0.45f);
            for (int iy = 0; iy < h; iy++)
                for (int ix = 0; ix < w; ix++)
                {
                    float x = ix + 0.5f, y = h - iy - 0.5f;   // top-down
                    float d0 = SdRound(x, y, x0, y0, x1, y1, r);
                    var c = m > 0 ? Shadow(SdRound(x, y - m * 0.6f, x0, y0, x1, y1, r), m + 1.5f, 0.55f) : new Color(0, 0, 0, 0);
                    float body = Cov(d0 + 1.1f);
                    float topBand = body * (1 - Cov(SdRound(x, y - 2.0f, x0, y0, x1, y1, r) + 1.1f));
                    float botBand = body * (1 - Cov(SdRound(x, y + 2.0f, x0, y0, x1, y1, r) + 1.1f));
                    float k = Mathf.Clamp01((y - y0) / Mathf.Max(1, y1 - y0));
                    var fill = Color.Lerp(top, bottom, pressed ? 1 - k * 0.6f : k);
                    // a little sheen in the upper half
                    if (!pressed) fill = Color.Lerp(fill, Color.white, 0.07f * Mathf.Clamp01(1 - k * 2));
                    fill = Color.Lerp(fill, pressed ? lo : hi, topBand * 0.85f);
                    fill = Color.Lerp(fill, pressed ? hi : lo, botBand * (pressed ? 0.35f : 0.8f));
                    if (inner.HasValue)
                    {
                        float ring = body * (1 - Cov(d0 + 3.2f));
                        fill = Color.Lerp(fill, inner.Value, Mathf.Clamp01(ring - topBand) * 0.75f);
                    }
                    var shape = Color.Lerp(outline, fill, body);
                    shape.a = Cov(d0);
                    px[iy * w + ix] = Over(c, shape);
                }
            t.SetPixels(px);
            t.Apply(false, false);
            return t;
        }

        /// <summary>
        /// A frame like gui_skin.xml's form / slim_form / editbox: a clay rim (<paramref name="rim"/> px, <paramref name="header"/>
        /// px on top) around a dark translucent field with an inset edge, plus a drop shadow.
        /// </summary>
        static Texture2D Frame(int w, int h, int m, float r, int rim, int header, Color top, Color bottom, Color fieldTop, Color fieldBottom, float shadow = 0.6f, Color? glow = null)
        {
            var t = NewTex(w, h);
            var px = new Color[w * h];
            float x0 = m, y0 = m * 0.6f, x1 = w - m, y1 = h - m * 1.4f;
            float ix0 = x0 + rim, iy0 = y0 + header, ix1 = x1 - rim, iy1 = y1 - rim;
            float ir = Mathf.Max(2f, r - rim * 0.6f);
            var outline = new Color(0.03f, 0.02f, 0.01f, 1);
            var hi = Color.Lerp(bottom, Color.white, 0.35f);
            var lo = Color.Lerp(top, Color.black, 0.5f);
            for (int iy = 0; iy < h; iy++)
                for (int ix = 0; ix < w; ix++)
                {
                    float x = ix + 0.5f, y = h - iy - 0.5f;
                    float d0 = SdRound(x, y, x0, y0, x1, y1, r);
                    var c = m > 0 && shadow > 0 ? Shadow(SdRound(x, y - m * 0.5f, x0, y0, x1, y1, r), m + 1f, shadow) : new Color(0, 0, 0, 0);
                    float body = Cov(d0 + 1f);
                    float topBand = body * (1 - Cov(SdRound(x, y - 1.6f, x0, y0, x1, y1, r) + 1f));
                    float botBand = body * (1 - Cov(SdRound(x, y + 1.6f, x0, y0, x1, y1, r) + 1f));
                    // rim gradient follows the whole height (the header gets the warm top of it)
                    float k = Mathf.Clamp01((y - y0) / Mathf.Max(1, Mathf.Min(y1 - y0, header + rim + 6f)));
                    var rimCol = Color.Lerp(top, bottom, k);
                    rimCol = Color.Lerp(rimCol, hi, topBand * 0.85f);
                    rimCol = Color.Lerp(rimCol, lo, botBand * 0.8f);
                    // inner edge of the rim: highlight under the field, shade above it
                    float di = SdRound(x, y, ix0, iy0, ix1, iy1, ir);
                    float field = Cov(di);
                    float underField = (1 - field) * Cov(SdRound(x, y - 1.6f, ix0, iy0, ix1, iy1, ir));
                    float overField = (1 - field) * Cov(SdRound(x, y + 1.6f, ix0, iy0, ix1, iy1, ir));
                    rimCol = Color.Lerp(rimCol, hi, underField * 0.8f);
                    rimCol = Color.Lerp(rimCol, lo, overField * 0.7f);
                    if (glow.HasValue) rimCol = Color.Lerp(rimCol, glow.Value, 0.35f * body * (1 - field));
                    var shape = Color.Lerp(outline, rimCol, body);
                    shape.a = 1;
                    // the field: dark, translucent, with a 1 px black inset line and a faint inner shadow at the top
                    float fk = Mathf.Clamp01((y - iy0) / Mathf.Max(1, iy1 - iy0));
                    var f = Color.Lerp(fieldTop, fieldBottom, fk);
                    float edge = field * (1 - Cov(di + 1.2f));
                    float innerShade = field * (1 - Mathf.Clamp01((y - iy0) / 5f)) * 0.35f;
                    f = Color.Lerp(f, new Color(0, 0, 0, Mathf.Max(f.a, 0.9f)), Mathf.Max(edge, innerShade));
                    var all = Color.Lerp(shape, f, field);
                    all.a = Mathf.Lerp(1, f.a, field) * Cov(d0);
                    px[iy * w + ix] = Over(c, all);
                }
            t.SetPixels(px);
            t.Apply(false, false);
            return t;
        }

        /// <summary>The slider groove (gui_skin.xml slider): a dark slot with a clay lip, shaded above and lit below.</summary>
        static Texture2D Groove(int w, int h)
        {
            var t = NewTex(w, h);
            var px = new Color[w * h];
            var lipTop = Hex(0x5E4A3E); var lipBottom = Hex(0xC4A696);
            for (int iy = 0; iy < h; iy++)
                for (int ix = 0; ix < w; ix++)
                {
                    float x = ix + 0.5f, y = h - iy - 0.5f;
                    float d = SdRound(x, y, 0.5f, 0.5f, w - 0.5f, h - 0.5f, 3.5f);
                    var lip = Color.Lerp(lipTop, lipBottom, y / h);
                    float ring = Cov(d) * (1 - Cov(d + 1.3f));
                    var c = Color.Lerp(new Color(0.03f, 0.02f, 0.015f, 1f), lip, ring);
                    c.a = Cov(d) * Mathf.Lerp(0.9f, 1f, ring);
                    px[iy * w + ix] = c;
                }
            t.SetPixels(px);
            t.Apply(false, false);
            return t;
        }

        /// <summary>Paints a dark check mark into a checkbox texture.</summary>
        static Texture2D Tick(Texture2D t)
        {
            int w = t.width, h = t.height;
            var px = t.GetPixels();
            var a = new Vector2(w * 0.27f, h * 0.52f); var b = new Vector2(w * 0.44f, h * 0.70f); var c = new Vector2(w * 0.75f, h * 0.30f);
            for (int iy = 0; iy < h; iy++)
                for (int ix = 0; ix < w; ix++)
                {
                    var p = new Vector2(ix + 0.5f, h - iy - 0.5f);
                    float d = Mathf.Min(SegDist(p, a, b), SegDist(p, b, c)) - 1.6f;
                    float k = Cov(d);
                    if (k <= 0) continue;
                    var o = px[iy * w + ix];
                    var ink = Color.Lerp(o, Dark, k); ink.a = o.a;
                    px[iy * w + ix] = ink;
                }
            t.SetPixels(px);
            t.Apply(false, false);
            return t;
        }
        static float SegDist(Vector2 p, Vector2 a, Vector2 b)
        {
            var ab = b - a; float k = Mathf.Clamp01(Vector2.Dot(p - a, ab) / ab.sqrMagnitude);
            return Vector2.Distance(p, a + ab * k);
        }

        /// <summary>A highlight ring (hover / selection outline).</summary>
        static Texture2D Ring(int w, int h, float r, float width, Color col)
        {
            var t = NewTex(w, h);
            var px = new Color[w * h];
            for (int iy = 0; iy < h; iy++)
                for (int ix = 0; ix < w; ix++)
                {
                    float d = SdRound(ix + 0.5f, iy + 0.5f, 1, 1, w - 1, h - 1, r);
                    float ring = Cov(Mathf.Abs(d + width * 0.5f) - width * 0.5f);
                    float halo = Mathf.Exp(-Mathf.Max(0, -d - width) * 0.6f) * 0.25f * Cov(d);
                    px[iy * w + ix] = new Color(col.r, col.g, col.b, col.a * Mathf.Max(ring, halo));
                }
            t.SetPixels(px);
            t.Apply(false, false);
            return t;
        }

        /// <summary>Puts a square box on the left of a wider texture with a transparent right part (toggle style).</summary>
        static Texture2D Pad(Texture2D box, int width)
        {
            var t = NewTex(width, box.height);
            t.SetPixels(new Color[width * box.height]);
            t.SetPixels(0, 0, box.width, box.height, box.GetPixels());
            t.Apply(false, false);
            return t;
        }

        // ================================================================== drawing helpers

        /// <summary>A rounded translucent panel.</summary>
        public static void Panel(Rect r, bool strong = false)
        {
            if (Event.current.type != EventType.Repaint) return;
            if (s_skin == null) Build();
            var st = strong ? (s_panelStyleStrong ??= new GUIStyle { normal = { background = s_panelStrong }, border = PanelBorder, overflow = new RectOffset(M, M, M, M) })
                            : (s_panelStyle ??= new GUIStyle { normal = { background = s_panel }, border = PanelBorder, overflow = new RectOffset(M, M, M, M) });
            st.Draw(r, false, false, false, false);
        }

        static GUIStyle s_panelStyle, s_panelStyleStrong, s_formStyle, s_glowStyle;

        /// <summary>A framed window with a clay title bar (gui_skin.xml form); returns the content rect.</summary>
        public static Rect Form(Rect r, string title)
        {
            if (s_skin == null) Build();
            if (Event.current.type == EventType.Repaint)
            {
                s_formStyle ??= new GUIStyle { normal = { background = s_form }, border = new RectOffset(M + 14, M + 14, M + 34, M + 14), overflow = new RectOffset(M, M, M, M) };
                s_formStyle.Draw(r, false, false, false, false);
            }
            if (!string.IsNullOrEmpty(title))
                GUI.Label(new Rect(r.x, r.y - 2, r.width, 30), title, s_formTitle ??= new GUIStyle(Skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 15, wordWrap = false, normal = { textColor = Darker } });
            return new Rect(r.x + 12, r.y + 34, r.width - 24, r.height - 46);
        }
        static GUIStyle s_formTitle;

        /// <summary>A gold outline (selection / hover highlight).</summary>
        public static void Glow(Rect r)
        {
            if (Event.current.type != EventType.Repaint || s_glow == null) return;
            (s_glowStyle ??= new GUIStyle { normal = { background = s_glow }, border = new RectOffset(12, 12, 12, 12), overflow = new RectOffset(2, 2, 2, 2) }).Draw(r, false, false, false, false);
        }

        // ================================================================== icons

        static void LoadIcons()
        {
            if (s_icons != null) return;
            s_icons = Resources.Load<Texture2D>("TTGui/icons");
            Logo = Resources.Load<Texture2D>("TTGui/tt_mainform_logo");
            MenuArt = Resources.Load<Texture2D>("TTGui/mainmenu");
            ReadLayout(s_icons, Resources.Load<TextAsset>("TTGui/icons_layout"), IconsW, IconsH);
            // icons drawn for the commands, buildings, units, upgrades and spells the original atlas has none for
            // (guard, patrol, hold, back, palisade, shrine, champion, scout, research, convert, war cry, spirit dance)
            var extra = Resources.Load<Texture2D>("TTGui/icons_extra");
            if (extra != null) ReadLayout(extra, Resources.Load<TextAsset>("TTGui/icons_extra_layout"), extra.width, extra.height);
        }

        static void ReadLayout(Texture2D tex, TextAsset xml, float texW, float texH)
        {
            if (xml == null || tex == null) return;
            try
            {
                var doc = new System.Xml.XmlDocument { XmlResolver = null };
                var settings = new System.Xml.XmlReaderSettings { DtdProcessing = System.Xml.DtdProcessing.Ignore };
                using (var reader = System.Xml.XmlReader.Create(new System.IO.StringReader(xml.text), settings)) doc.Load(reader);
                foreach (System.Xml.XmlNode n in doc.DocumentElement.ChildNodes)
                {
                    if (n.NodeType != System.Xml.XmlNodeType.Element) continue;
                    var states = new Rect[3];
                    var direct = n.SelectSingleNode("quad");
                    if (direct != null) states[0] = states[1] = states[2] = Quad(direct, texW, texH);
                    else
                    {
                        string[] names = { "normal", "active", "disabled" };
                        for (int i = 0; i < 3; i++) { var q = n.SelectSingleNode(names[i] + "/quad"); if (q != null) states[i] = Quad(q, texW, texH); }
                        if (states[1].width == 0) states[1] = states[0];
                        if (states[2].width == 0) states[2] = states[0];
                    }
                    s_regions[n.Name] = states;
                    s_regionTex[n.Name] = tex;
                }
            }
            catch (System.Exception e) { Debug.LogWarning("[TT] " + xml.name + ": " + e.Message); }
        }

        static Rect Quad(System.Xml.XmlNode q, float texW, float texH)
        {
            var inv = System.Globalization.CultureInfo.InvariantCulture;
            float l = float.Parse(q.Attributes["left"].Value, inv), t = float.Parse(q.Attributes["top"].Value, inv);
            float r = float.Parse(q.Attributes["right"].Value, inv), b = float.Parse(q.Attributes["bottom"].Value, inv);
            return new Rect(l / texW, 1f - b / texH, (r - l) / texW, (b - t) / texH);
        }

        public static bool HasIcon(string name) { LoadIcons(); return s_regionTex.TryGetValue(name, out var t) && t != null; }

        /// <summary>Draws an icon (state: 0 normal, 1 active/hover, 2 disabled).</summary>
        public static void Icon(Rect r, string name, int state = 0)
        {
            LoadIcons();
            if (Event.current.type != EventType.Repaint || !s_regions.TryGetValue(name, out var st) || !s_regionTex.TryGetValue(name, out var tex) || tex == null) return;
            GUI.DrawTextureWithTexCoords(r, tex, st[Mathf.Clamp(state, 0, 2)], true);
        }

        /// <summary>
        /// A command button with the original icon, its hotkey in the corner and an optional badge (count, percent).
        /// Returns true when clicked. Falls back to a text button when the icon is missing.
        /// </summary>
        public static bool IconButton(Rect r, string icon, string label, string key, bool enabled, string badge = null, bool selected = false)
        {
            var e = Event.current;
            bool hover = enabled && r.Contains(e.mousePosition);
            if (!HasIcon(icon))
            {
                var old = GUI.enabled; GUI.enabled = enabled;
                s_textCmd ??= new GUIStyle(Skin.button) { fontSize = 12, wordWrap = false, padding = new RectOffset(2, 2, 2, 2) };
                bool c = GUI.Button(r, label + (key != null ? "\n<size=11><color=#5a2a0a>" + key + "</color></size>" : ""), s_textCmd);
                GUI.enabled = old;
                return c;
            }
            if (e.type == EventType.Repaint)
            {
                var bg = new GUIStyle(Skin.button);
                bg.Draw(r, GUIContent.none, hover, hover && Input.GetMouseButton(0), selected, false);
                var ir = new Rect(r.x + 4, r.y + 4, r.width - 8, r.height - 8);
                Icon(ir, icon, !enabled ? 2 : hover ? 1 : 0);
                if (!enabled) { var o = GUI.color; GUI.color = new Color(0, 0, 0, 0.35f); GUI.DrawTexture(r, Texture2D.whiteTexture); GUI.color = o; }
                if (key != null) GUI.Label(new Rect(r.xMax - 22, r.y + 1, 20, 16), "<size=11><b><color=#ffd27f>" + key + "</color></b></size>", s_keyStyle ??= new GUIStyle(Skin.label) { alignment = TextAnchor.UpperRight, wordWrap = false });
                if (badge != null) GUI.Label(new Rect(r.x + 3, r.yMax - 19, r.width - 6, 18), "<b>" + badge + "</b>", s_badgeStyle ??= new GUIStyle(Skin.label) { alignment = TextAnchor.LowerLeft, fontSize = 13, wordWrap = false });
                if (hover) Glow(r);
            }
            if (hover && !string.IsNullOrEmpty(label)) s_tooltip = label + (key != null ? "  <color=#ffd27f>[" + key + "]</color>" : "");
            if (enabled && e.type == EventType.MouseDown && e.button == 0 && r.Contains(e.mousePosition)) { e.Use(); return true; }
            return false;
        }
        static GUIStyle s_keyStyle, s_badgeStyle, s_tipStyle, s_textCmd;
        static string s_tooltip, s_tipLast;
        static float s_tipSince = -1f;

        /// <summary>Draws the hovered button's tooltip (call once at the end of OnGUI).</summary>
        public static void Tooltip()
        {
            if (Event.current.type == EventType.Layout) { return; }
            if (Event.current.type != EventType.Repaint) return;
            if (string.IsNullOrEmpty(s_tooltip)) { s_tipSince = -1f; s_tipLast = null; return; }
            // Settings.tooltip_delay
            if (s_tooltip != s_tipLast) { s_tipLast = s_tooltip; s_tipSince = Time.unscaledTime; }
            if (Time.unscaledTime - s_tipSince < TribalTrouble.Game.GameSettings.tooltipDelay) { s_tooltip = null; return; }
            s_tipStyle ??= new GUIStyle(Skin.label) { wordWrap = false, fontSize = 13 };
            var size = s_tipStyle.CalcSize(new GUIContent(s_tooltip));
            var m = Event.current.mousePosition;
            var r = new Rect(m.x + 14, m.y - size.y - 14, size.x + 16, size.y + 8);
            if (r.xMax > Screen.width) r.x = Screen.width - r.width - 4;
            if (r.y < 0) r.y = m.y + 20;
            Panel(r, true);
            GUI.Label(new Rect(r.x + 8, r.y + 4, size.x, size.y), s_tooltip, s_tipStyle);
            s_tooltip = null;
        }
    }
}
