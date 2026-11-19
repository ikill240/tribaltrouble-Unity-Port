// Steam port: RegistryItems by hand (it reads the geometry registry, which the port parses itself). What the geometry
// registry adds to the race templates: the things peons carry, the items a unit can wear in its slots, and the skins
// players may own. All of it is render-only.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    public class RegistryItems : java.lang.Object
    {
        public const int DEFAULT_TEXTURE = 0;
        const int IRON_TEXTURE = 1;
        const string CARRIED_SLOT = "carried";
        const string CARRIED_UNIT = "peon";

        public RegistryItems() { }

        static System.Collections.Generic.IEnumerable<com.oddlabs.geometry.registry.AttachmentEntry> Attachments()
        {
            var l = com.oddlabs.tt.model.GeometryRegistry.attachments();
            for (int i = 0; i < l.size(); i++) yield return (com.oddlabs.geometry.registry.AttachmentEntry)l.get(i);
        }

        /// <summary>The sprite a peon of this tribe shows for each kind of thing it can carry; the simulation decides which.</summary>
        public static java.util.Map carried(com.oddlabs.tt.render.RenderQueues queues, string group)
        {
            var rock = CarriedEntry(group, "rock_resource");
            var m = new java.util.LinkedHashMap();
            m.put(typeof(com.oddlabs.tt.landscape.TreeSupply), RegisterItem(queues, CarriedEntry(group, "wood_resource"), DEFAULT_TEXTURE));
            m.put(typeof(com.oddlabs.tt.model.RockSupply), RegisterItem(queues, rock, DEFAULT_TEXTURE));
            m.put(typeof(com.oddlabs.tt.model.IronSupply), RegisterItem(queues, rock, IRON_TEXTURE));
            m.put(typeof(com.oddlabs.tt.model.RubberSupply), RegisterItem(queues, CarriedEntry(group, "rubber_resource"), DEFAULT_TEXTURE));
            m.put(typeof(com.oddlabs.tt.model.LeftPaddle), RegisterItem(queues, CarriedEntry(group, "left_paddle"), DEFAULT_TEXTURE));
            m.put(typeof(com.oddlabs.tt.model.RightPaddle), RegisterItem(queues, CarriedEntry(group, "right_paddle"), DEFAULT_TEXTURE));
            return m;
        }

        // During an event, <name>_<event> stands in for <name> when the registry has one. Without a registry (headless
        // tests with no assets), the stock sprite path stands in.
        static com.oddlabs.geometry.registry.AttachmentEntry CarriedEntry(string group, string name)
        {
            com.oddlabs.geometry.registry.AttachmentEntry found = null;
            foreach (var entry in Attachments())
            {
                if (entry.group() != group || entry.@base() != CARRIED_UNIT || entry.slot() != CARRIED_SLOT) continue;
                bool evented = false;
                foreach (var ev in com.oddlabs.tt.model.GeometryRegistry.ActiveEventNames) if (entry.name() == name + "_" + ev) evented = true;
                if (evented || (entry.name() == name && found == null)) found = entry;
            }
            return found ?? new com.oddlabs.geometry.registry.AttachmentEntry(group, CARRIED_UNIT, CARRIED_SLOT, false, name, 2, com.oddlabs.geometry.registry.Registry.NO_EVENT);
        }

        /// <summary>The items a unit of this tribe can show, by slot and then by item name, drawn with the unit's texture tier.</summary>
        public static java.util.Map slots(com.oddlabs.tt.render.RenderQueues queues, string group, string unit, int tex_index)
        {
            var slots = new java.util.LinkedHashMap();
            foreach (var entry in Attachments())
            {
                if (entry.group() != group || entry.@base() != unit || entry.slot() == CARRIED_SLOT) continue;
                var items = (java.util.Map)slots.get(entry.slot());
                if (items == null) { items = new java.util.LinkedHashMap(); slots.put(entry.slot(), items); }
                items.put(entry.name(), RegisterItem(queues, entry, tex_index));
            }
            return slots;
        }

        /// <summary>The slots a new unit of this tribe starts with an item in.</summary>
        public static java.util.Set defaultSlots(string group, string unit)
        {
            var s = new java.util.LinkedHashSet();
            foreach (var entry in Attachments())
                if (entry.defaultOn() && entry.group() == group && entry.@base() == unit) s.add(entry.slot());
            return s;
        }

        static com.oddlabs.tt.render.SpriteKey RegisterItem(com.oddlabs.tt.render.RenderQueues queues, com.oddlabs.geometry.registry.AttachmentEntry entry, int tex_index)
        {
            string location = com.oddlabs.tt.model.GeometryRegistry.spritePath(entry.group(), entry.name());
            return queues.register(new com.oddlabs.tt.resource.SpriteFile(location, com.oddlabs.tt.global.Globals.NO_MIPMAP_CUTOFF, true, true, true, false),
                TextureIndex(entry.textures(), tex_index));
        }

        static int TextureIndex(int textures, int tex_index) => tex_index < textures ? tex_index : DEFAULT_TEXTURE;

        /// <summary>Every owned skin by name: for each, the sprite that stands in for a unit, a building stage or an item,
        /// plus the props the skin brings to a building stage.</summary>
        public static java.util.Map skins(com.oddlabs.tt.render.RenderQueues queues, com.oddlabs.tt.model.Race[] races, java.util.Set carried)
        {
            var units = new System.Collections.Generic.Dictionary<string, java.util.Map>();
            var buildings = new System.Collections.Generic.Dictionary<string, java.util.Map>();
            var props = new System.Collections.Generic.Dictionary<string, java.util.Map>();
            var items = new System.Collections.Generic.Dictionary<string, java.util.Map>();
            var names = new System.Collections.Generic.List<string>();
            void Name(string n) { if (!names.Contains(n)) names.Add(n); }
            var item_keys = new System.Collections.Generic.List<com.oddlabs.tt.render.SpriteKey>();
            void AddKey(object k) { if (k is com.oddlabs.tt.render.SpriteKey sk && !item_keys.Contains(sk)) item_keys.Add(sk); }
            foreach (var k in java.lang.JIter.of(carried)) AddKey(k);
            foreach (var race in races)
                for (int i = 0; i < com.oddlabs.tt.model.Race.NUM_UNITS; i++)
                {
                    var att = race.getUnitTemplate(i).getAttachments();
                    if (att == null) continue;
                    foreach (var slot in java.lang.JIter.of(att.values()))
                        foreach (var k in java.lang.JIter.of(((java.util.Map)slot).values())) AddKey(k);
                }
            var skinList = com.oddlabs.tt.model.GeometryRegistry.skins();
            for (int e = 0; e < skinList.size(); e++)
            {
                var entry = (com.oddlabs.geometry.registry.SkinEntry)skinList.get(e);
                if (entry.@event() != com.oddlabs.geometry.registry.Registry.NO_EVENT) continue;
                foreach (var item in item_keys)
                {
                    var skin = Reskin(queues, item, entry);
                    if (skin == null) continue;
                    if (!items.TryGetValue(entry.skin(), out var m)) items[entry.skin()] = m = new java.util.HashMap();
                    m.put(item, skin); Name(entry.skin());
                }
                foreach (var race in races)
                {
                    for (int i = 0; i < com.oddlabs.tt.model.Race.NUM_UNITS; i++)
                    {
                        var unit = race.getUnitTemplate(i);
                        var skin = Reskin(queues, unit.getSpriteRenderer(), entry);
                        if (skin == null) continue;
                        if (!units.TryGetValue(entry.skin(), out var m)) units[entry.skin()] = m = new java.util.HashMap();
                        m.put(unit, skin); Name(entry.skin());
                    }
                    for (int i = 0; i < com.oddlabs.tt.model.Race.NUM_BUILDINGS; i++)
                    {
                        var building = race.getBuildingTemplate(i);
                        foreach (var stage in com.oddlabs.tt.model.Building.BuildState.values())
                        {
                            var skin = Reskin(queues, building.getRenderer(stage), entry);
                            if (skin == null) continue;
                            if (!buildings.TryGetValue(entry.skin(), out var bm)) buildings[entry.skin()] = bm = new java.util.HashMap();
                            var stages = (java.util.Map)bm.get(building);
                            if (stages == null) { stages = new java.util.EnumMap(typeof(com.oddlabs.tt.model.Building.BuildState)); bm.put(building, stages); }
                            stages.put(stage, skin);
                            if (!props.TryGetValue(entry.skin(), out var pm)) props[entry.skin()] = pm = new java.util.HashMap();
                            var pstages = (java.util.Map)pm.get(building);
                            if (pstages == null) { pstages = new java.util.EnumMap(typeof(com.oddlabs.tt.model.Building.BuildState)); pm.put(building, pstages); }
                            var list = new java.util.ArrayList();
                            var stock = building.getProps(stage);
                            if (stock != null) foreach (var p in java.lang.JIter.of(stock)) list.add(p);
                            var extra = queues.getProps(skin);
                            if (extra != null) foreach (var p in java.lang.JIter.of(extra)) list.add(p);
                            pstages.put(stage, list);
                            Name(entry.skin());
                        }
                    }
                }
            }
            var skins = new java.util.HashMap();
            foreach (var name in names)
                skins.put(name, new com.oddlabs.tt.player.PlayerSkins(
                    units.TryGetValue(name, out var u) ? u : new java.util.HashMap(),
                    buildings.TryGetValue(name, out var b) ? b : new java.util.HashMap(),
                    props.TryGetValue(name, out var p2) ? p2 : new java.util.HashMap(),
                    items.TryGetValue(name, out var it) ? it : new java.util.HashMap()));
            return skins;
        }

        // Drawn with the replaced sprite's flags and texture, or the first texture when the skin has fewer.
        static com.oddlabs.tt.render.SpriteKey Reskin(com.oddlabs.tt.render.RenderQueues queues, com.oddlabs.tt.render.SpriteKey stock, com.oddlabs.geometry.registry.SkinEntry entry)
        {
            if (stock == null) return null;
            var stock_file = queues.getSpriteFile(stock);
            if (stock_file == null) return null;
            string replaced = com.oddlabs.tt.model.GeometryRegistry.spritePath(entry.group(), entry.replaces());
            if (!java.lang.JO.equals(stock_file, stock_file.withLocation(replaced))) return null;
            string location = com.oddlabs.tt.model.GeometryRegistry.spritePath(entry.group(), entry.name());
            return queues.register(stock_file.withLocation(location), TextureIndex(entry.textures(), queues.getTexIndex(stock)));
        }
    }
}
