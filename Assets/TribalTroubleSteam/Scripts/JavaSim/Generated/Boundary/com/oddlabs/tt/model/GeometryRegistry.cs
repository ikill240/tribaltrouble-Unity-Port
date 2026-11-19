// Steam port: GeometryRegistry by hand (the Java one reads registry.json with Jackson). The registry the geometry
// converter writes next to the binary sprites - the items units wear in their slots, skins, the scenery of events and
// the glow lights - with the entries of events that are off left out. Everything here is render-only: which event is
// on comes from the local date (Autumn through October, Halloween from the 16th), so players in one game may differ.
#pragma warning disable
using System;
using System.Collections.Generic;
using J2CS.Ext;

namespace com.oddlabs.geometry.registry
{
    /// <summary>An item in a slot of its base sprite.</summary>
    public sealed class AttachmentEntry : java.lang.Object
    {
        readonly string _group, _base, _slot, _name, _event; readonly bool _defaultOn; readonly int _textures;
        public AttachmentEntry(string group, string @base, string slot, bool defaultOn, string name, int textures, string @event)
        { _group = group; _base = @base; _slot = slot; _defaultOn = defaultOn; _name = name; _textures = textures; _event = @event; }
        public string group() => _group;
        public string @base() => _base;
        public string slot() => _slot;
        public bool defaultOn() => _defaultOn;
        public string name() => _name;
        public int textures() => _textures;
        public string @event() => _event;
        /// <summary>Items of a slot show default-on ones first, then by name (Java's ORDER).</summary>
        public static int Order(AttachmentEntry a, AttachmentEntry b)
        {
            int c = string.CompareOrdinal(a._group, b._group); if (c != 0) return c;
            c = string.CompareOrdinal(a._base, b._base); if (c != 0) return c;
            c = string.CompareOrdinal(a._slot, b._slot); if (c != 0) return c;
            c = (!a._defaultOn).CompareTo(!b._defaultOn); if (c != 0) return c;
            return string.CompareOrdinal(a._name, b._name);
        }
    }

    /// <summary>A sprite that stands in for another sprite of its group while the skin applies.</summary>
    public sealed class SkinEntry : java.lang.Object
    {
        readonly string _group, _skin, _replaces, _name, _event; readonly int _textures;
        public SkinEntry(string group, string skin, string replaces, string name, int textures, string @event)
        { _group = group; _skin = skin; _replaces = replaces; _name = name; _textures = textures; _event = @event; }
        public string group() => _group;
        public string skin() => _skin;
        public string replaces() => _replaces;
        public string name() => _name;
        public int textures() => _textures;
        public string @event() => _event;
    }

    /// <summary>Scenery the game scatters on the named grounds, count per map.</summary>
    public sealed class DecorationEntry : java.lang.Object
    {
        readonly string _group, _name, _grounds, _event; readonly int _count;
        public DecorationEntry(string group, string name, string grounds, int count, string @event)
        { _group = group; _name = name; _grounds = grounds; _count = count; _event = @event; }
        public string group() => _group;
        public string name() => _name;
        public string grounds() => _grounds;
        public int count() => _count;
        public string @event() => _event;
    }

    /// <summary>A spot in a sprite's space where the glow of one of its textures lights the surroundings at night.</summary>
    public sealed class LightEntry : java.lang.Object
    {
        readonly string _group, _name, _color; readonly int _texture; readonly float _reach, _strength, _x, _y, _z;
        public LightEntry(string group, string name, int texture, string color, float reach, float strength, float x, float y, float z)
        { _group = group; _name = name; _texture = texture; _color = color; _reach = reach; _strength = strength; _x = x; _y = y; _z = z; }
        public string group() => _group;
        public string name() => _name;
        public int texture() => _texture;
        public string color() => _color;
        public float reach() => _reach;
        public float strength() => _strength;
        public float x() => _x;
        public float y() => _y;
        public float z() => _z;
    }

    public sealed class Registry : java.lang.Object
    {
        public const string FILE = "registry.json";
        /// <summary>The event of an entry that is loaded all year.</summary>
        public const string NO_EVENT = "-";
        public readonly List<AttachmentEntry> Attachments = new List<AttachmentEntry>();
        public readonly List<SkinEntry> Skins = new List<SkinEntry>();
        public readonly List<DecorationEntry> Decorations = new List<DecorationEntry>();
        public readonly List<LightEntry> Lights = new List<LightEntry>();

        public static Registry Parse(string json)
        {
            var r = new Registry();
            if (string.IsNullOrEmpty(json)) return r;
            var root = MiniJson.Parse(json) as Dictionary<string, object>;
            if (root == null) return r;
            foreach (var o in Arr(root, "attachments"))
                r.Attachments.Add(new AttachmentEntry(S(o, "group"), S(o, "base"), S(o, "slot"), B(o, "defaultOn"), S(o, "name"), I(o, "textures"), S(o, "event")));
            foreach (var o in Arr(root, "skins"))
                r.Skins.Add(new SkinEntry(S(o, "group"), S(o, "skin"), S(o, "replaces"), S(o, "name"), I(o, "textures"), S(o, "event")));
            foreach (var o in Arr(root, "decorations"))
                r.Decorations.Add(new DecorationEntry(S(o, "group"), S(o, "name"), S(o, "grounds"), I(o, "count"), S(o, "event")));
            foreach (var o in Arr(root, "lights"))
                r.Lights.Add(new LightEntry(S(o, "group"), S(o, "name"), I(o, "texture"), S(o, "color"), F(o, "reach"), F(o, "strength"), F(o, "x"), F(o, "y"), F(o, "z")));
            return r;
        }

        static IEnumerable<Dictionary<string, object>> Arr(Dictionary<string, object> o, string k)
        {
            if (o.TryGetValue(k, out var v) && v is List<object> l)
                foreach (var e in l) if (e is Dictionary<string, object> d) yield return d;
        }
        static string S(Dictionary<string, object> o, string k) => o.TryGetValue(k, out var v) && v is string s ? s : (k == "event" ? NO_EVENT : "");
        static bool B(Dictionary<string, object> o, string k) => o.TryGetValue(k, out var v) && v is bool b && b;
        static int I(Dictionary<string, object> o, string k) => o.TryGetValue(k, out var v) && v is double d ? (int)d : 0;
        static float F(Dictionary<string, object> o, string k) => o.TryGetValue(k, out var v) && v is double d ? (float)d : 0f;
    }

    /// <summary>Just enough JSON for registry.json: objects, arrays, strings, numbers, booleans and null.</summary>
    static class MiniJson
    {
        public static object Parse(string s) { int i = 0; return Value(s, ref i); }
        static void Ws(string s, ref int i) { while (i < s.Length && char.IsWhiteSpace(s[i])) i++; }
        static object Value(string s, ref int i)
        {
            Ws(s, ref i);
            if (i >= s.Length) return null;
            char c = s[i];
            if (c == '{')
            {
                var d = new Dictionary<string, object>(); i++;
                for (;;)
                {
                    Ws(s, ref i); if (i < s.Length && s[i] == '}') { i++; return d; }
                    string k = Str(s, ref i); Ws(s, ref i); i++; // ':'
                    d[k] = Value(s, ref i); Ws(s, ref i);
                    if (i < s.Length && s[i] == ',') { i++; continue; }
                    if (i < s.Length && s[i] == '}') { i++; return d; }
                    return d;
                }
            }
            if (c == '[')
            {
                var l = new List<object>(); i++;
                for (;;)
                {
                    Ws(s, ref i); if (i < s.Length && s[i] == ']') { i++; return l; }
                    l.Add(Value(s, ref i)); Ws(s, ref i);
                    if (i < s.Length && s[i] == ',') { i++; continue; }
                    if (i < s.Length && s[i] == ']') { i++; return l; }
                    return l;
                }
            }
            if (c == '"') return Str(s, ref i);
            if (s.Length - i >= 4 && string.CompareOrdinal(s, i, "true", 0, 4) == 0) { i += 4; return true; }
            if (s.Length - i >= 5 && string.CompareOrdinal(s, i, "false", 0, 5) == 0) { i += 5; return false; }
            if (s.Length - i >= 4 && string.CompareOrdinal(s, i, "null", 0, 4) == 0) { i += 4; return null; }
            int st = i;
            while (i < s.Length && "+-0123456789.eE".IndexOf(s[i]) >= 0) i++;
            return double.Parse(s.Substring(st, i - st), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);
        }
        static string Str(string s, ref int i)
        {
            var sb = new System.Text.StringBuilder(); i++; // opening quote
            while (i < s.Length && s[i] != '"')
            {
                char c = s[i++];
                if (c == '\\' && i < s.Length)
                {
                    char e = s[i++];
                    switch (e)
                    {
                        case 'n': sb.Append('\n'); break;
                        case 't': sb.Append('\t'); break;
                        case 'r': sb.Append('\r'); break;
                        case 'b': sb.Append('\b'); break;
                        case 'f': sb.Append('\f'); break;
                        case 'u': sb.Append((char)Convert.ToInt32(s.Substring(i, 4), 16)); i += 4; break;
                        default: sb.Append(e); break;
                    }
                }
                else sb.Append(c);
            }
            i++; // closing quote
            return sb.ToString();
        }
    }
}

namespace com.oddlabs.tt.model
{
    public class GeometryRegistry : java.lang.Object
    {
        public const string AUTUMN = "autumn";
        public const string HALLOWEEN = "halloween";
        const int LAST_AUTUMN_ONLY_DAY = 15;
        const string PROP_SLOT = "prop";
        const string GEOMETRY_DIR = "/geometry/";
        static readonly System.Text.RegularExpressions.Regex SPRITE_PATH = new System.Text.RegularExpressions.Regex(@"/geometry/\w+/\w+\.binsprite$");

        /// <summary>Overrides the date (like -Dcom.oddlabs.tt.event=a,b); null = by the local date.</summary>
        public static string ForcedEvents;
        /// <summary>The registry's text (the Unity host hands it over from its Resources; null = JResources).</summary>
        public static Func<string> ReadRegistry;

        static HashSet<string> s_events;
        static com.oddlabs.geometry.registry.Registry s_registry;
        static java.util.List s_attachments, s_skins;
        static Dictionary<string, string> s_eventSkins;
        static Dictionary<string, List<Light>> s_lights;

        /// <summary>A glow light of a sprite (Java: GlowLights.Light): where, how far, and its colour times strength.</summary>
        public struct Light { public float x, y, z, reach, r, g, b; }

        public GeometryRegistry() { }

        static HashSet<string> ActiveEvents()
        {
            if (ForcedEvents != null) return new HashSet<string>(ForcedEvents.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            var today = DateTime.Now;
            if (today.Month != 10) return new HashSet<string>();
            return today.Day <= LAST_AUTUMN_ONLY_DAY ? new HashSet<string> { AUTUMN } : new HashSet<string> { AUTUMN, HALLOWEEN };
        }

        static HashSet<string> Events => s_events ??= ActiveEvents();

        /// <summary>Starts over (after ForcedEvents or the registry changed).</summary>
        public static void Reset() { s_events = null; s_registry = null; s_attachments = null; s_skins = null; s_eventSkins = null; s_lights = null; }

        public static java.util.Set events()
        {
            var s = new java.util.LinkedHashSet();
            foreach (var e in Events) s.add(e);
            return s;
        }

        public static IEnumerable<string> ActiveEventNames => Events;

        public static bool isEventActive(string @event) => @event == com.oddlabs.geometry.registry.Registry.NO_EVENT || Events.Contains(@event);

        public static java.util.List attachments()
        {
            if (s_attachments == null)
            {
                var l = new List<com.oddlabs.geometry.registry.AttachmentEntry>();
                foreach (var e in Reg().Attachments) if (isEventActive(e.@event())) l.Add(e);
                l.Sort(com.oddlabs.geometry.registry.AttachmentEntry.Order);
                var jl = new java.util.ArrayList();
                foreach (var e in l) jl.add(e);
                s_attachments = jl;
            }
            return s_attachments;
        }

        public static java.util.List skins()
        {
            if (s_skins == null)
            {
                var jl = new java.util.ArrayList();
                foreach (var e in Reg().Skins) if (isEventActive(e.@event())) jl.add(e);
                s_skins = jl;
            }
            return s_skins;
        }

        public static java.util.List decorations()
        {
            var jl = new java.util.ArrayList();
            foreach (var e in Reg().Decorations) if (isEventActive(e.@event())) jl.add(e);
            return jl;
        }

        /// <summary>The registry path of every prop drawn with the sprite at location.</summary>
        public static java.util.List getProps(string location)
        {
            string @base = registryPath(location);
            var jl = new java.util.ArrayList();
            var all = attachments();
            for (int i = 0; i < all.size(); i++)
            {
                var e = (com.oddlabs.geometry.registry.AttachmentEntry)all.get(i);
                if (e.slot() == PROP_SLOT && spritePath(e.group(), e.@base()) == @base) jl.add(spritePath(e.group(), e.name()));
            }
            return jl;
        }

        /// <summary>Where the glow of the sprite at location drawn with texture tex_index lights from.</summary>
        public static List<Light> getLights(string location, int tex_index)
        {
            if (s_lights == null)
            {
                s_lights = new Dictionary<string, List<Light>>();
                foreach (var e in Reg().Lights)
                {
                    int rgb = Convert.ToInt32(e.color().Substring(1), 16);
                    float strength = e.strength() / 255f;
                    string key = spritePath(e.group(), e.name()) + " " + e.texture();
                    if (!s_lights.TryGetValue(key, out var l)) s_lights[key] = l = new List<Light>();
                    l.Add(new Light { x = e.x(), y = e.y(), z = e.z(), reach = e.reach(), r = (rgb >> 16 & 0xff) * strength, g = (rgb >> 8 & 0xff) * strength, b = (rgb & 0xff) * strength });
                }
            }
            return s_lights.TryGetValue(registryPath(location) + " " + tex_index, out var r) ? r : new List<Light>();
        }

        /// <summary>The sprite location the active event draws for everyone in place of this one (itself when none).</summary>
        public static string EventSkinLocation(string location)
        {
            if (s_eventSkins == null)
            {
                s_eventSkins = new Dictionary<string, string>();
                var all = skins();
                for (int i = 0; i < all.size(); i++)
                {
                    var e = (com.oddlabs.geometry.registry.SkinEntry)all.get(i);
                    if (e.@event() != com.oddlabs.geometry.registry.Registry.NO_EVENT)
                        s_eventSkins[spritePath(e.group(), e.replaces())] = spritePath(e.group(), e.name());
                }
            }
            string path = registryPath(location);
            return path != null && s_eventSkins.TryGetValue(path, out var skin) ? skin : location;
        }

        public static com.oddlabs.tt.resource.SpriteFile eventSkin(com.oddlabs.tt.resource.SpriteFile sprite_file)
        {
            string location = EventSkinLocation(sprite_file.getLocation());
            return location != sprite_file.getLocation() ? sprite_file.withLocation(location) : sprite_file;
        }

        public static string spritePath(string group, string name) => GEOMETRY_DIR + group + "/" + name + ".binsprite";

        // Resource locations are URLs, while the registry names a sprite by its path under /geometry.
        static string registryPath(string location)
        {
            if (location == null) return null;
            var m = SPRITE_PATH.Match(location);
            return m.Success ? m.Value : null;
        }

        static com.oddlabs.geometry.registry.Registry Reg()
        {
            if (s_registry == null)
            {
                string text = null;
                try
                {
                    if (ReadRegistry != null) text = ReadRegistry();
                    else
                    {
                        var bytes = java.lang.JResources.Open?.Invoke(GEOMETRY_DIR + com.oddlabs.geometry.registry.Registry.FILE);
                        if (bytes != null) text = System.Text.Encoding.UTF8.GetString(bytes);
                    }
                }
                catch (Exception e) { java.lang.IO.println("GeometryRegistry: " + e.Message); }
                s_registry = com.oddlabs.geometry.registry.Registry.Parse(text);
            }
            return s_registry;
        }
    }
}
