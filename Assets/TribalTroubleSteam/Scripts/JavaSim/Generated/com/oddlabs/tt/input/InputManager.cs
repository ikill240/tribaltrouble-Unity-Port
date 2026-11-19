// Translated from InputManager.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.input
{
    // Java: 27
    public class InputManager : java.lang.Object
    {
        public static java.util.logging.Logger logger;
        public static java.util.Map DEFAULT_BINDINGS;
        // Java: 243
        public static void def(com.oddlabs.tt.input.GameAction action, com.oddlabs.tt.input.Key key, params com.oddlabs.tt.input.Modifier[] modifiers)
        {
            java.util.Set modSet = java.util.EnumSet.noneOf(typeof(com.oddlabs.tt.input.Modifier));
            java.util.Collections.addAll(modSet, modifiers);
            ((java.util.NavigableSet)com.oddlabs.tt.input.InputManager.DEFAULT_BINDINGS.computeIfAbsent(action, new java.util.function.Function_L((object k_l) =>
            {
                com.oddlabs.tt.input.GameAction k = (com.oddlabs.tt.input.GameAction)k_l;
                return new java.util.TreeSet();
            }))).add(new com.oddlabs.tt.input.InputBinding(key, modSet, action));
        }
        public java.util.NavigableSet bindings;
        public java.util.Set activeActions;
        public java.util.Map keyState;
        // Java: 257
        public void loadDefaultBindings()
        {
            bindings.clear();
            com.oddlabs.tt.input.InputManager.DEFAULT_BINDINGS.values().forEach(new java.util.function.Consumer_L((object a0) => { bindings.addAll(((java.util.Collection)a0)); }));
        }
        // Java: 263
        public void loadBindings(java.util.Properties props)
        {
            bindings.clear();
            foreach (com.oddlabs.tt.input.GameAction action in com.oddlabs.tt.input.GameAction.values())
            {
                string key = "key_binding." + java.lang.JStr.valueOf(action.name());
                string @value = props.getProperty(key);
                if (@value != null)
                {
                    try
                    {
                        java.util.NavigableSet loaded = parseBindings(@value, action);
                        if (!loaded.isEmpty() || com.oddlabs.tt.input.InputManager.isEmptyList(@value))
                        {
                            bindings.addAll(loaded);
                            continue;
                        }
                    }
                    catch (System.Exception e)
                    {
                    }
                    com.oddlabs.tt.input.InputManager.logger.warning("Failed to parse binding for " + java.lang.JStr.valueOf(action) + ": " + java.lang.JStr.valueOf(@value) + ". Using defaults.");
                }
                java.util.Set defaults = ((java.util.NavigableSet)com.oddlabs.tt.input.InputManager.DEFAULT_BINDINGS.get(action));
                if (defaults != null)
                {
                    bindings.addAll(defaults);
                }
            }
        }
        // Java: 296
        public void saveBindings(java.util.Properties props)
        {
            java.util.Map currentMap = new java.util.EnumMap(typeof(com.oddlabs.tt.input.GameAction));
            foreach (com.oddlabs.tt.input.InputBinding b in java.lang.JIter.of(bindings))
            {
                ((java.util.NavigableSet)currentMap.computeIfAbsent(b.action(), new java.util.function.Function_L((object k_l) =>
                {
                    com.oddlabs.tt.input.GameAction k = (com.oddlabs.tt.input.GameAction)k_l;
                    return new java.util.TreeSet();
                }))).add(b);
            }
            foreach (com.oddlabs.tt.input.GameAction action in com.oddlabs.tt.input.GameAction.values())
            {
                java.util.NavigableSet current = ((java.util.NavigableSet)currentMap.get(action));
                java.util.NavigableSet defaults = ((java.util.NavigableSet)com.oddlabs.tt.input.InputManager.DEFAULT_BINDINGS.get(action));
                if (current != null)
                {
                    if (current.isEmpty())
                    {
                        props.setProperty("key_binding." + java.lang.JStr.valueOf(action.name()), "[]");
                    }
                    else if (!java.util.Objects.equals(current, defaults))
                    {
                        props.setProperty("key_binding." + java.lang.JStr.valueOf(action.name()), serializeBindings(current));
                    }
                }
                else if (defaults != null)
                {
                    props.setProperty("key_binding." + java.lang.JStr.valueOf(action.name()), "[]");
                }
            }
        }
        // Java: 320
        public java.util.NavigableSet getBindings(com.oddlabs.tt.input.GameAction action)
        {
            return ((java.util.TreeSet)bindings.stream().filter(new java.util.function.Predicate_L((object b_l) =>
            {
                com.oddlabs.tt.input.InputBinding b = (com.oddlabs.tt.input.InputBinding)b_l;
                return ((object)b.action() == (object)action);
            })).collect(java.util.stream.Collectors.toCollection(new java.util.function.Supplier_L(() => { return new java.util.TreeSet(); }))));
        }
        // Java: 324
        public string getBindingString(com.oddlabs.tt.input.GameAction action)
        {
            java.util.NavigableSet set = getBindings(action);
            return (set.isEmpty() ? "" : java.lang.JO.toString(((com.oddlabs.tt.input.InputBinding)set.getFirst())));
        }
        // Java: 329
        public java.util.NavigableSet getDefaultBindings(com.oddlabs.tt.input.GameAction action)
        {
            java.util.NavigableSet defaults = ((java.util.NavigableSet)com.oddlabs.tt.input.InputManager.DEFAULT_BINDINGS.get(action));
            return (defaults == null ? java.util.Collections.emptyNavigableSet() : (java.util.NavigableSet)(new java.util.TreeSet(defaults)));
        }
        // Java: 334
        public void setBindings(com.oddlabs.tt.input.GameAction action, java.util.Collection newBindings)
        {
            bindings.removeIf(new java.util.function.Predicate_L((object b_l) =>
            {
                com.oddlabs.tt.input.InputBinding b = (com.oddlabs.tt.input.InputBinding)b_l;
                return ((object)b.action() == (object)action);
            }));
            bindings.addAll(newBindings);
        }
        // Java: 339
        public void resetToDefaults()
        {
            bindings.clear();
            com.oddlabs.tt.input.InputManager.DEFAULT_BINDINGS.values().forEach(new java.util.function.Consumer_L((object a0) => { bindings.addAll(((java.util.Collection)a0)); }));
        }
        // Java: 344
        public string exportBindings()
        {
            java.util.Map currentMap = new java.util.EnumMap(typeof(com.oddlabs.tt.input.GameAction));
            foreach (com.oddlabs.tt.input.InputBinding b in java.lang.JIter.of(bindings))
            {
                ((java.util.Set)currentMap.computeIfAbsent(b.action(), new java.util.function.Function_L((object k_l) =>
                {
                    com.oddlabs.tt.input.GameAction k = (com.oddlabs.tt.input.GameAction)k_l;
                    return new java.util.concurrent.CopyOnWriteArraySet();
                }))).add(b);
            }
            java.lang.StringBuilder sb = new java.lang.StringBuilder();
            sb.append("{\n");
            bool first = true;
            foreach (com.oddlabs.tt.input.GameAction action in com.oddlabs.tt.input.GameAction.values())
            {
                java.util.Set current = ((java.util.Set)currentMap.get(action));
                if (current == null && ((java.util.NavigableSet)com.oddlabs.tt.input.InputManager.DEFAULT_BINDINGS.get(action)) == null)
                {
                    continue;
                }
                if (!first)
                {
                    sb.append(",\n");
                }
                first = false;
                sb.append("  \"").append(action.name()).append("\": ");
                sb.append(serializeBindings((current == null ? (java.util.Collection)(java.util.Set_.of()) : (java.util.Collection)(current))));
            }
            sb.append("\n}");
            return java.lang.JO.toString(sb);
        }
        // Java: 367
        public void importBindings(string json)
        {
            string patternStr = "\\\"(\\w+)\\\"\\s*:\\s*(\\[[^\\]]*\\])";
            java.util.regex.Pattern p = java.util.regex.Pattern.compile(patternStr);
            java.util.regex.Matcher m = p.matcher(json);
            java.util.Set newBindings = new java.util.HashSet();
            java.util.Set mentioned = java.util.EnumSet.noneOf(typeof(com.oddlabs.tt.input.GameAction));
            bool foundAny = false;
            while (m.find())
            {
                string actionName = m.group(1);
                string arrayContent = m.group(2);
                try
                {
                    com.oddlabs.tt.input.GameAction action = com.oddlabs.tt.input.GameAction.valueOf(actionName);
                    java.util.Set parsed = parseBindings(arrayContent, action);
                    if (!parsed.isEmpty() || com.oddlabs.tt.input.InputManager.isEmptyList(arrayContent))
                    {
                        newBindings.addAll(parsed);
                        mentioned.add(action);
                        foundAny = true;
                    }
                }
                catch (System.Exception e)
                {
                    com.oddlabs.tt.input.InputManager.logger.warning("Failed to import bindings for " + java.lang.JStr.valueOf(actionName));
                }
            }
            if (foundAny)
            {
                bindings.clear();
                bindings.addAll(newBindings);
                foreach (com.oddlabs.tt.input.GameAction action_2 in com.oddlabs.tt.input.GameAction.values())
                {
                    if (!mentioned.contains(action_2))
                    {
                        java.util.Set defaults = ((java.util.NavigableSet)com.oddlabs.tt.input.InputManager.DEFAULT_BINDINGS.get(action_2));
                        if (defaults != null)
                        {
                            bindings.addAll(defaults);
                        }
                    }
                }
            }
            else
            {
                com.oddlabs.tt.input.InputManager.logger.warning("No valid bindings found in JSON import.");
            }
        }
        // Java: 415
        public static bool isEmptyList(string @value)
        {
            return java.lang.JO.equals(@value.replaceAll("\\s", ""), "[]");
        }
        // Java: 419
        public string serializeBindings(java.util.Collection set)
        {
            return ((string)set.stream().map(new java.util.function.Function_L((object b_l) =>
            {
                com.oddlabs.tt.input.InputBinding b = (com.oddlabs.tt.input.InputBinding)b_l;
                return "{\"key\":\"" + java.lang.JStr.valueOf(b.key().name()) + "\"" + java.lang.JStr.valueOf(((b.shift() ? ", \"shift\":true" : ""))) + java.lang.JStr.valueOf(((b.control() ? ", \"control\":true" : ""))) + java.lang.JStr.valueOf(((b.alt() ? ", \"alt\":true" : ""))) + java.lang.JStr.valueOf(((b.meta() ? ", \"meta\":true" : ""))) + "}";
            })).collect(java.util.stream.Collectors.joining(", ", "[", "]")));
        }
        // Java: 425
        public java.util.NavigableSet parseBindings(string json, com.oddlabs.tt.input.GameAction action)
        {
            java.util.NavigableSet set = new java.util.TreeSet();
            string trimmed = json.trim();
            if (trimmed.length() < 2 || !trimmed.startsWith("[") || !trimmed.endsWith("]"))
            {
                return set;
            }
            string content = trimmed.substring(1, trimmed.length() - 1);
            if (content.isBlank())
            {
                return set;
            }
            string[] parts = content.split("(?<=})\\s*,\\s*(?=\\{)");
            foreach (string part in parts)
            {
                com.oddlabs.tt.input.InputBinding b = parseBindingObject(part, action);
                if (b != null)
                {
                    set.add(b);
                }
            }
            return set;
        }
        // Java: 442
        public com.oddlabs.tt.input.InputBinding parseBindingObject(string content, com.oddlabs.tt.input.GameAction action)
        {
            string inner = content.trim();
            if (inner.startsWith("{"))
            {
                inner = inner.substring(1);
            }
            if (inner.endsWith("}"))
            {
                inner = inner.substring(0, inner.length() - 1);
            }
            string[] pairs = inner.split(",");
            com.oddlabs.tt.input.Key key = com.oddlabs.tt.input.Key.KEY_UNKNOWN;
            java.util.Set modifiers = java.util.EnumSet.noneOf(typeof(com.oddlabs.tt.input.Modifier));
            foreach (string pair in pairs)
            {
                string[] kv = pair.split("[:=]");
                if (kv.Length != 2)
                {
                    continue;
                }
                string k = unquote(kv[0].trim()).toLowerCase(java.util.Locale.ROOT);
                string v = unquote(kv[1].trim());
                try
                {
                    switch ((k))
                    {
                        case "key":
                        {
                            key = com.oddlabs.tt.input.Key.valueOf(v);
                            break;
                        }
                        case "shift":
                        {
                            if (java.lang.Boolean.parseBoolean(v))
                            {
                                modifiers.add(com.oddlabs.tt.input.Modifier.SHIFT);
                            }
                            break;
                        }
                        case "control":
                        {
                            if (java.lang.Boolean.parseBoolean(v))
                            {
                                modifiers.add(com.oddlabs.tt.input.Modifier.CONTROL);
                            }
                            break;
                        }
                        case "alt":
                        {
                            if (java.lang.Boolean.parseBoolean(v))
                            {
                                modifiers.add(com.oddlabs.tt.input.Modifier.ALT);
                            }
                            break;
                        }
                        case "meta":
                        {
                            if (java.lang.Boolean.parseBoolean(v))
                            {
                                modifiers.add(com.oddlabs.tt.input.Modifier.META);
                            }
                            break;
                        }
                    }
                }
                catch (System.Exception e)
                {
                    com.oddlabs.tt.input.InputManager.logger.warning("ignoring " + java.lang.JStr.valueOf(k) + ": " + java.lang.JStr.valueOf(v));
                }
            }
            if (((object)key != (object)com.oddlabs.tt.input.Key.KEY_UNKNOWN))
            {
                return new com.oddlabs.tt.input.InputBinding(key, modifiers, action);
            }
            return null;
        }
        // Java: 487
        public string unquote(string s)
        {
            if (s.startsWith("\"") && s.endsWith("\"") && s.length() >= 2)
            {
                return s.substring(1, s.length() - 1);
            }
            return s;
        }
        // Java: 494
        public java.util.Set getActions(com.oddlabs.tt.input.KeyboardEvent @event)
        {
            java.util.Set actions = java.util.EnumSet.noneOf(typeof(com.oddlabs.tt.input.GameAction));
            foreach (com.oddlabs.tt.input.InputBinding binding in java.lang.JIter.of(bindings))
            {
                if (binding.matches(@event))
                {
                    actions.add(binding.action());
                }
            }
            return actions;
        }
        // Java: 505
        public void updateState(com.oddlabs.tt.input.KeyboardEvent @event, bool pressed)
        {
            if (pressed)
            {
                java.util.Set actions = getActions(@event);
                if (!actions.isEmpty())
                {
                    keyState.put(@event.keyCode(), actions);
                    activeActions.addAll(actions);
                }
            }
            else
            {
                java.util.Set actions_2 = ((java.util.Set)keyState.remove(@event.keyCode()));
                if (actions_2 != null)
                {
                    activeActions.removeAll(actions_2);
                }
            }
        }
        // Java: 520
        public bool isActive(com.oddlabs.tt.input.GameAction action)
        {
            return activeActions.contains(action);
        }
        // Java: 524
        public void reset()
        {
            activeActions.clear();
            keyState.clear();
        }
        static InputManager()
        {
            InputManager.logger = java.util.logging.Logger.getLogger(typeof(com.oddlabs.tt.input.InputManager).getName());
            InputManager.DEFAULT_BINDINGS = new java.util.EnumMap(typeof(com.oddlabs.tt.input.GameAction));
            {
                bool isMac = java.lang.JSystem.getProperty("os.name", "").toLowerCase().contains("mac");
                if (isMac)
                {
                    com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.GLOBAL_QUIT, com.oddlabs.tt.input.Key.Q, com.oddlabs.tt.input.Modifier.META);
                }
                else
                {
                    com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.GLOBAL_QUIT, com.oddlabs.tt.input.Key.Q, com.oddlabs.tt.input.Modifier.CONTROL);
                }
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.GLOBAL_TOGGLE_FULLSCREEN, com.oddlabs.tt.input.Key.F11);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.GLOBAL_SCREENSHOT, com.oddlabs.tt.input.Key.S, com.oddlabs.tt.input.Modifier.CONTROL);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.GLOBAL_SCREENSHOT, com.oddlabs.tt.input.Key.P, com.oddlabs.tt.input.Modifier.CONTROL);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.GLOBAL_CHAT, com.oddlabs.tt.input.Key.RETURN);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.GLOBAL_CHAT_TEAM, com.oddlabs.tt.input.Key.RETURN, com.oddlabs.tt.input.Modifier.SHIFT);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.GLOBAL_MENU, com.oddlabs.tt.input.Key.ESCAPE);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.GLOBAL_TOGGLE_STATUS, com.oddlabs.tt.input.Key.I, com.oddlabs.tt.input.Modifier.CONTROL);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.GLOBAL_TOGGLE_HUD, com.oddlabs.tt.input.Key.H, com.oddlabs.tt.input.Modifier.CONTROL);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.DEBUG_PRINT_INFO, com.oddlabs.tt.input.Key.I, com.oddlabs.tt.input.Modifier.CONTROL);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.GLOBAL_AGGRESSIVE_UNITS, com.oddlabs.tt.input.Key.A, com.oddlabs.tt.input.Modifier.CONTROL);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.CAMERA_PAN_LEFT, com.oddlabs.tt.input.Key.LEFT);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.CAMERA_PAN_RIGHT, com.oddlabs.tt.input.Key.RIGHT);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.CAMERA_PAN_UP, com.oddlabs.tt.input.Key.UP);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.CAMERA_PAN_DOWN, com.oddlabs.tt.input.Key.DOWN);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.CAMERA_PITCH_UP, com.oddlabs.tt.input.Key.HOME);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.CAMERA_PITCH_UP, com.oddlabs.tt.input.Key.NUMPAD8);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.CAMERA_PITCH_UP, com.oddlabs.tt.input.Key.UP, com.oddlabs.tt.input.Modifier.ALT);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.CAMERA_PITCH_DOWN, com.oddlabs.tt.input.Key.END);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.CAMERA_PITCH_DOWN, com.oddlabs.tt.input.Key.NUMPAD2);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.CAMERA_PITCH_DOWN, com.oddlabs.tt.input.Key.DOWN, com.oddlabs.tt.input.Modifier.ALT);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.CAMERA_ROTATE_RIGHT, com.oddlabs.tt.input.Key.INSERT);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.CAMERA_ROTATE_RIGHT, com.oddlabs.tt.input.Key.NUMPAD6);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.CAMERA_ROTATE_RIGHT, com.oddlabs.tt.input.Key.RIGHT, com.oddlabs.tt.input.Modifier.ALT);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.CAMERA_ROTATE_LEFT, com.oddlabs.tt.input.Key.DELETE);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.CAMERA_ROTATE_LEFT, com.oddlabs.tt.input.Key.NUMPAD4);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.CAMERA_ROTATE_LEFT, com.oddlabs.tt.input.Key.LEFT, com.oddlabs.tt.input.Modifier.ALT);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.CAMERA_ZOOM_IN, com.oddlabs.tt.input.Key.PAGE_UP);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.CAMERA_ZOOM_IN, com.oddlabs.tt.input.Key.NUMPAD9);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.CAMERA_ZOOM_OUT, com.oddlabs.tt.input.Key.PAGE_DOWN);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.CAMERA_ZOOM_OUT, com.oddlabs.tt.input.Key.NUMPAD3);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.CAMERA_MAP_MODE, com.oddlabs.tt.input.Key.SPACE);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.CAMERA_MAP_MODE, com.oddlabs.tt.input.Key.NUMPAD5);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.CAMERA_FIRST_PERSON, com.oddlabs.tt.input.Key.F);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.CAMERA_ZOOM_MODE, com.oddlabs.tt.input.Key.Z);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.CAMERA_CINEMATIC, com.oddlabs.tt.input.Key.F, com.oddlabs.tt.input.Modifier.CONTROL);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.CAMERA_ORBIT_LEFT, com.oddlabs.tt.input.Key.LEFT, com.oddlabs.tt.input.Modifier.CONTROL);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.CAMERA_ORBIT_RIGHT, com.oddlabs.tt.input.Key.RIGHT, com.oddlabs.tt.input.Modifier.CONTROL);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.CAMERA_AUTO_PAN_FORWARD, com.oddlabs.tt.input.Key.UP, com.oddlabs.tt.input.Modifier.CONTROL);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.CAMERA_AUTO_PAN_BACKWARD, com.oddlabs.tt.input.Key.DOWN, com.oddlabs.tt.input.Modifier.CONTROL);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.UI_ACTIVATE, com.oddlabs.tt.input.Key.SPACE);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.UI_ACTIVATE, com.oddlabs.tt.input.Key.RETURN);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.UI_CANCEL, com.oddlabs.tt.input.Key.ESCAPE);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.UI_FOCUS_NEXT, com.oddlabs.tt.input.Key.TAB);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.UI_FOCUS_PREV, com.oddlabs.tt.input.Key.TAB, com.oddlabs.tt.input.Modifier.SHIFT);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.UI_NEXT_PANEL, com.oddlabs.tt.input.Key.TAB, com.oddlabs.tt.input.Modifier.CONTROL);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.UI_PREV_PANEL, com.oddlabs.tt.input.Key.TAB, com.oddlabs.tt.input.Modifier.SHIFT, com.oddlabs.tt.input.Modifier.CONTROL);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.UI_NAV_UP, com.oddlabs.tt.input.Key.UP);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.UI_NAV_DOWN, com.oddlabs.tt.input.Key.DOWN);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.UI_NAV_LEFT, com.oddlabs.tt.input.Key.LEFT);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.UI_NAV_RIGHT, com.oddlabs.tt.input.Key.RIGHT);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.UI_NAV_HOME, com.oddlabs.tt.input.Key.HOME);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.UI_NAV_END, com.oddlabs.tt.input.Key.END);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.UI_NAV_UP, com.oddlabs.tt.input.Key.UP, com.oddlabs.tt.input.Modifier.SHIFT);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.UI_NAV_DOWN, com.oddlabs.tt.input.Key.DOWN, com.oddlabs.tt.input.Modifier.SHIFT);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.UI_NAV_LEFT, com.oddlabs.tt.input.Key.LEFT, com.oddlabs.tt.input.Modifier.SHIFT);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.UI_NAV_RIGHT, com.oddlabs.tt.input.Key.RIGHT, com.oddlabs.tt.input.Modifier.SHIFT);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.UI_NAV_HOME, com.oddlabs.tt.input.Key.HOME, com.oddlabs.tt.input.Modifier.SHIFT);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.UI_NAV_END, com.oddlabs.tt.input.Key.END, com.oddlabs.tt.input.Modifier.SHIFT);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.UI_NAV_PAGE_UP, com.oddlabs.tt.input.Key.PAGE_UP);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.UI_NAV_PAGE_DOWN, com.oddlabs.tt.input.Key.PAGE_DOWN);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.UI_BACKSPACE, com.oddlabs.tt.input.Key.BACK);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.UI_DELETE, com.oddlabs.tt.input.Key.DELETE);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.UNIT_MOVE, com.oddlabs.tt.input.Key.M);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.UNIT_ATTACK, com.oddlabs.tt.input.Key.A);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.UNIT_GATHER, com.oddlabs.tt.input.Key.G);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.UNIT_BUILD_QUARTERS, com.oddlabs.tt.input.Key.Q);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.UNIT_BUILD_ARMORY, com.oddlabs.tt.input.Key.R);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.UNIT_BUILD_TOWER, com.oddlabs.tt.input.Key.T);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.UNIT_BUILD_SHIP, com.oddlabs.tt.input.Key.S);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.UNIT_EXIT_TOWER, com.oddlabs.tt.input.Key.X);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.UNIT_BEACON, com.oddlabs.tt.input.Key.B, com.oddlabs.tt.input.Modifier.CONTROL);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.UNIT_NEXT_IDLE, com.oddlabs.tt.input.Key.N);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.UNIT_SET_RALLY, com.oddlabs.tt.input.Key.R);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.GAMEPLAY_BACK, com.oddlabs.tt.input.Key.BACK);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.ARMY_SELECT_0, com.oddlabs.tt.input.Key.KEY_0);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.ARMY_SELECT_1, com.oddlabs.tt.input.Key.KEY_1);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.ARMY_SELECT_2, com.oddlabs.tt.input.Key.KEY_2);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.ARMY_SELECT_3, com.oddlabs.tt.input.Key.KEY_3);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.ARMY_SELECT_4, com.oddlabs.tt.input.Key.KEY_4);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.ARMY_SELECT_5, com.oddlabs.tt.input.Key.KEY_5);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.ARMY_SELECT_6, com.oddlabs.tt.input.Key.KEY_6);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.ARMY_SELECT_7, com.oddlabs.tt.input.Key.KEY_7);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.ARMY_SELECT_8, com.oddlabs.tt.input.Key.KEY_8);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.ARMY_SELECT_9, com.oddlabs.tt.input.Key.KEY_9);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.ARMY_CREATE_0, com.oddlabs.tt.input.Key.KEY_0, com.oddlabs.tt.input.Modifier.CONTROL);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.ARMY_CREATE_1, com.oddlabs.tt.input.Key.KEY_1, com.oddlabs.tt.input.Modifier.CONTROL);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.ARMY_CREATE_2, com.oddlabs.tt.input.Key.KEY_2, com.oddlabs.tt.input.Modifier.CONTROL);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.ARMY_CREATE_3, com.oddlabs.tt.input.Key.KEY_3, com.oddlabs.tt.input.Modifier.CONTROL);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.ARMY_CREATE_4, com.oddlabs.tt.input.Key.KEY_4, com.oddlabs.tt.input.Modifier.CONTROL);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.ARMY_CREATE_5, com.oddlabs.tt.input.Key.KEY_5, com.oddlabs.tt.input.Modifier.CONTROL);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.ARMY_CREATE_6, com.oddlabs.tt.input.Key.KEY_6, com.oddlabs.tt.input.Modifier.CONTROL);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.ARMY_CREATE_7, com.oddlabs.tt.input.Key.KEY_7, com.oddlabs.tt.input.Modifier.CONTROL);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.ARMY_CREATE_8, com.oddlabs.tt.input.Key.KEY_8, com.oddlabs.tt.input.Modifier.CONTROL);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.ARMY_CREATE_9, com.oddlabs.tt.input.Key.KEY_9, com.oddlabs.tt.input.Modifier.CONTROL);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.PROD_WEAPONS, com.oddlabs.tt.input.Key.W);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.PROD_HARVEST, com.oddlabs.tt.input.Key.G);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.PROD_ARMY, com.oddlabs.tt.input.Key.A);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.PROD_TRANSPORT, com.oddlabs.tt.input.Key.T);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.RES_TREE, com.oddlabs.tt.input.Key.W);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.RES_TREE_DEC, com.oddlabs.tt.input.Key.W, com.oddlabs.tt.input.Modifier.SHIFT);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.RES_TREE_BATCH, com.oddlabs.tt.input.Key.W, com.oddlabs.tt.input.Modifier.CONTROL);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.RES_TREE_BATCH_DEC, com.oddlabs.tt.input.Key.W, com.oddlabs.tt.input.Modifier.SHIFT, com.oddlabs.tt.input.Modifier.CONTROL);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.RES_ROCK, com.oddlabs.tt.input.Key.R);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.RES_ROCK_DEC, com.oddlabs.tt.input.Key.R, com.oddlabs.tt.input.Modifier.SHIFT);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.RES_ROCK_BATCH, com.oddlabs.tt.input.Key.R, com.oddlabs.tt.input.Modifier.CONTROL);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.RES_ROCK_BATCH_DEC, com.oddlabs.tt.input.Key.R, com.oddlabs.tt.input.Modifier.SHIFT, com.oddlabs.tt.input.Modifier.CONTROL);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.RES_IRON, com.oddlabs.tt.input.Key.I);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.RES_IRON_DEC, com.oddlabs.tt.input.Key.I, com.oddlabs.tt.input.Modifier.SHIFT);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.RES_IRON_BATCH, com.oddlabs.tt.input.Key.I, com.oddlabs.tt.input.Modifier.CONTROL);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.RES_IRON_BATCH_DEC, com.oddlabs.tt.input.Key.I, com.oddlabs.tt.input.Modifier.SHIFT, com.oddlabs.tt.input.Modifier.CONTROL);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.RES_CHICKEN, com.oddlabs.tt.input.Key.C);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.RES_CHICKEN_DEC, com.oddlabs.tt.input.Key.C, com.oddlabs.tt.input.Modifier.SHIFT);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.RES_CHICKEN_BATCH, com.oddlabs.tt.input.Key.C, com.oddlabs.tt.input.Modifier.CONTROL);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.RES_CHICKEN_BATCH_DEC, com.oddlabs.tt.input.Key.C, com.oddlabs.tt.input.Modifier.SHIFT, com.oddlabs.tt.input.Modifier.CONTROL);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.DEPLOY_CHIEFTAIN, com.oddlabs.tt.input.Key.H);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.TRAIN_PEON, com.oddlabs.tt.input.Key.P);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.TRAIN_PEON_DEC, com.oddlabs.tt.input.Key.P, com.oddlabs.tt.input.Modifier.SHIFT);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.TRAIN_PEON_BATCH, com.oddlabs.tt.input.Key.P, com.oddlabs.tt.input.Modifier.CONTROL);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.TRAIN_PEON_BATCH_DEC, com.oddlabs.tt.input.Key.P, com.oddlabs.tt.input.Modifier.SHIFT, com.oddlabs.tt.input.Modifier.CONTROL);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.TRAIN_CHIEFTAIN, com.oddlabs.tt.input.Key.C);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.MAGIC_1, com.oddlabs.tt.input.Key.S);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.MAGIC_2, com.oddlabs.tt.input.Key.C);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.GAME_SPEED_UP, com.oddlabs.tt.input.Key.EQUALS, com.oddlabs.tt.input.Modifier.SHIFT);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.GAME_SPEED_UP, com.oddlabs.tt.input.Key.ADD);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.GAME_SPEED_DOWN, com.oddlabs.tt.input.Key.MINUS);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.GAME_SPEED_DOWN, com.oddlabs.tt.input.Key.SUBTRACT);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.NOTIFICATION_JUMP, com.oddlabs.tt.input.Key.TAB);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.CHEAT_1, com.oddlabs.tt.input.Key.F1);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.CHEAT_2, com.oddlabs.tt.input.Key.F2);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.CHEAT_3, com.oddlabs.tt.input.Key.F3);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.CHEAT_4, com.oddlabs.tt.input.Key.F4);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.CHEAT_5, com.oddlabs.tt.input.Key.F5);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.CHEAT_6, com.oddlabs.tt.input.Key.F6);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.CHEAT_7, com.oddlabs.tt.input.Key.F7);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.CHEAT_8, com.oddlabs.tt.input.Key.F8);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.CHEAT_9, com.oddlabs.tt.input.Key.F9);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.CHEAT_10, com.oddlabs.tt.input.Key.F10);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.SPECTATOR_NEXT_PLAYER, com.oddlabs.tt.input.Key.RBRACKET);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.SPECTATOR_PREV_PLAYER, com.oddlabs.tt.input.Key.LBRACKET);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.SPECTATOR_FREE_CAMERA, com.oddlabs.tt.input.Key.SLASH);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.DEBUG_PRINT_INFO, com.oddlabs.tt.input.Key.I, com.oddlabs.tt.input.Modifier.CONTROL);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.DEBUG_KILL_SELECTED, com.oddlabs.tt.input.Key.K, com.oddlabs.tt.input.Modifier.CONTROL);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.DEBUG_TOGGLE_LIGHT, com.oddlabs.tt.input.Key.L);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.DEBUG_TOGGLE_LIGHT, com.oddlabs.tt.input.Key.O);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.DEBUG_TOGGLE_PLANTS, com.oddlabs.tt.input.Key.P);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.DEBUG_TOGGLE_PARTICLES, com.oddlabs.tt.input.Key.E);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.DEBUG_TOGGLE_AXES, com.oddlabs.tt.input.Key.A);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.DEBUG_TOGGLE_MISC, com.oddlabs.tt.input.Key.M, com.oddlabs.tt.input.Modifier.CONTROL);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.DEBUG_RESET_CURSOR, com.oddlabs.tt.input.Key.J);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.DEBUG_TOGGLE_DETAIL, com.oddlabs.tt.input.Key.S);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.DEBUG_CRASH, com.oddlabs.tt.input.Key.C, com.oddlabs.tt.input.Modifier.CONTROL);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.DEBUG_TOGGLE_FRAME_BUFFER, com.oddlabs.tt.input.Key.C);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.DEBUG_TOGGLE_BOUNDING, com.oddlabs.tt.input.Key.D);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.DEBUG_TOGGLE_FRUSTUM_FREEZE, com.oddlabs.tt.input.Key.V);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.DEBUG_FORCE_GC, com.oddlabs.tt.input.Key.F12);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.DEBUG_START_RECORDING, com.oddlabs.tt.input.Key.U);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.DEBUG_TOGGLE_WATER, com.oddlabs.tt.input.Key.W, com.oddlabs.tt.input.Modifier.CONTROL);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.DEBUG_TOGGLE_AI, com.oddlabs.tt.input.Key.R, com.oddlabs.tt.input.Modifier.CONTROL);
                com.oddlabs.tt.input.InputManager.def(com.oddlabs.tt.input.GameAction.DEBUG_DUMP_ANIMATIONS, com.oddlabs.tt.input.Key.F1);
            }
        }
        private void init_InputManager()
        {
            this.bindings = new java.util.TreeSet();
            this.activeActions = java.util.EnumSet.noneOf(typeof(com.oddlabs.tt.input.GameAction));
            this.keyState = new java.util.EnumMap(typeof(com.oddlabs.tt.input.Key));
        }
        public InputManager() : base()
        {
            init_InputManager();
            loadDefaultBindings();
        }
    }
}
