// Translated from KeyBindingConflicts.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.input
{
    // Java: 24
    public class KeyBindingConflicts : java.lang.Object
    {
        public static com.oddlabs.tt.input.GameAction[] ALWAYS_ACTIVE;
        public static com.oddlabs.tt.input.GameAction[] UNIT_CONTEXT;
        public static com.oddlabs.tt.input.GameAction[] BUILDING_CONTEXT;
        public static com.oddlabs.tt.input.GameAction[] RESOURCE_CONTEXT;
        public static com.oddlabs.tt.input.GameAction[] UI_NAV;
        public static com.oddlabs.tt.input.GameAction[] PEON_CONTEXT;
        public static com.oddlabs.tt.input.GameAction[] QUARTERS_CONTEXT;
        public static com.oddlabs.tt.input.GameAction[] EXTENDED_ARMY_CONTEXT;
        public static com.oddlabs.tt.input.GameAction[] EXTENDED_HARVEST_CONTEXT;
        public static com.oddlabs.tt.input.GameAction[][] IN_GAME_CONTEXTS;
        public static com.oddlabs.tt.input.GameAction[][] CONFLICT_GROUPS;
        public static java.util.Map CONFLICT_MAP;
        // Java: 83
        public static java.util.Map buildConflictMap()
        {
            java.util.EnumMap map = new java.util.EnumMap(typeof(com.oddlabs.tt.input.GameAction));
            foreach (com.oddlabs.tt.input.GameAction[] group in com.oddlabs.tt.input.KeyBindingConflicts.CONFLICT_GROUPS)
            {
                foreach (com.oddlabs.tt.input.GameAction a in group)
                {
                    java.util.Set peers = ((java.util.Set)map.computeIfAbsent(a, new java.util.function.Function_L((object k_l) =>
                    {
                        com.oddlabs.tt.input.GameAction k = (com.oddlabs.tt.input.GameAction)k_l;
                        return java.util.EnumSet.noneOf(typeof(com.oddlabs.tt.input.GameAction));
                    })));
                    foreach (com.oddlabs.tt.input.GameAction other in group)
                    {
                        if (((object)other != (object)a))
                        {
                            peers.add(other);
                        }
                    }
                }
            }
            foreach (com.oddlabs.tt.input.GameAction[] context in com.oddlabs.tt.input.KeyBindingConflicts.IN_GAME_CONTEXTS)
            {
                foreach (com.oddlabs.tt.input.GameAction a_2 in com.oddlabs.tt.input.KeyBindingConflicts.ALWAYS_ACTIVE)
                {
                    java.util.Set peers_2 = ((java.util.Set)map.computeIfAbsent(a_2, new java.util.function.Function_L((object k_2_l) =>
                    {
                        com.oddlabs.tt.input.GameAction k_2 = (com.oddlabs.tt.input.GameAction)k_2_l;
                        return java.util.EnumSet.noneOf(typeof(com.oddlabs.tt.input.GameAction));
                    })));
                    foreach (com.oddlabs.tt.input.GameAction other_2 in context)
                    {
                        peers_2.add(other_2);
                        ((java.util.Set)map.computeIfAbsent(other_2, new java.util.function.Function_L((object k_3_l) =>
                        {
                            com.oddlabs.tt.input.GameAction k_3 = (com.oddlabs.tt.input.GameAction)k_3_l;
                            return java.util.EnumSet.noneOf(typeof(com.oddlabs.tt.input.GameAction));
                        }))).add(a_2);
                    }
                }
            }
            return map;
        }
        // Java: 117
        public static com.oddlabs.tt.input.GameAction findConflict(com.oddlabs.tt.input.GameAction action, com.oddlabs.tt.input.InputBinding proposed, com.oddlabs.tt.input.InputManager manager)
        {
            java.util.Set peers = ((java.util.Set)com.oddlabs.tt.input.KeyBindingConflicts.CONFLICT_MAP.get(action));
            if (peers == null)
            {
                return null;
            }
            foreach (com.oddlabs.tt.input.GameAction other in java.lang.JIter.of(peers))
            {
                foreach (com.oddlabs.tt.input.InputBinding existing in java.lang.JIter.of(manager.getBindings(other)))
                {
                    if (com.oddlabs.tt.input.KeyBindingConflicts.bindingsCollide(existing, proposed))
                    {
                        return other;
                    }
                }
            }
            return null;
        }
        // Java: 136
        public static java.util.List findExistingConflicts(com.oddlabs.tt.input.GameAction action, com.oddlabs.tt.input.InputManager manager)
        {
            java.util.Set peers = ((java.util.Set)com.oddlabs.tt.input.KeyBindingConflicts.CONFLICT_MAP.get(action));
            if (peers == null)
            {
                return java.util.List_.of();
            }
            java.util.Set mine = manager.getBindings(action);
            if (mine.isEmpty())
            {
                return java.util.List_.of();
            }
            java.util.List conflicts = new java.util.ArrayList();
            foreach (com.oddlabs.tt.input.GameAction other in java.lang.JIter.of(peers))
            {
                foreach (com.oddlabs.tt.input.InputBinding existing in java.lang.JIter.of(manager.getBindings(other)))
                {
                    foreach (com.oddlabs.tt.input.InputBinding ours in java.lang.JIter.of(mine))
                    {
                        if (com.oddlabs.tt.input.KeyBindingConflicts.bindingsCollide(existing, ours))
                        {
                            conflicts.add(other);
                            goto cnt_outer_10;
                        }
                    }
                }
                cnt_outer_10: ;
            }
            return conflicts;
        }
        // Java: 158
        public static bool bindingsCollide(com.oddlabs.tt.input.InputBinding a, com.oddlabs.tt.input.InputBinding b)
        {
            return ((object)a.key() == (object)b.key()) && java.lang.JO.equals(a.modifiers(), b.modifiers());
        }
        static KeyBindingConflicts()
        {
            KeyBindingConflicts.ALWAYS_ACTIVE = new com.oddlabs.tt.input.GameAction[] { com.oddlabs.tt.input.GameAction.GLOBAL_QUIT, com.oddlabs.tt.input.GameAction.GLOBAL_TOGGLE_FULLSCREEN, com.oddlabs.tt.input.GameAction.GLOBAL_CHAT, com.oddlabs.tt.input.GameAction.GLOBAL_CHAT_TEAM, com.oddlabs.tt.input.GameAction.GLOBAL_MENU, com.oddlabs.tt.input.GameAction.GLOBAL_AGGRESSIVE_UNITS, com.oddlabs.tt.input.GameAction.CAMERA_PAN_LEFT, com.oddlabs.tt.input.GameAction.CAMERA_PAN_RIGHT, com.oddlabs.tt.input.GameAction.CAMERA_PAN_UP, com.oddlabs.tt.input.GameAction.CAMERA_PAN_DOWN, com.oddlabs.tt.input.GameAction.CAMERA_PITCH_UP, com.oddlabs.tt.input.GameAction.CAMERA_PITCH_DOWN, com.oddlabs.tt.input.GameAction.CAMERA_ROTATE_LEFT, com.oddlabs.tt.input.GameAction.CAMERA_ROTATE_RIGHT, com.oddlabs.tt.input.GameAction.CAMERA_ZOOM_IN, com.oddlabs.tt.input.GameAction.CAMERA_ZOOM_OUT, com.oddlabs.tt.input.GameAction.CAMERA_MAP_MODE, com.oddlabs.tt.input.GameAction.CAMERA_FIRST_PERSON, com.oddlabs.tt.input.GameAction.CAMERA_ZOOM_MODE, com.oddlabs.tt.input.GameAction.UNIT_BEACON, com.oddlabs.tt.input.GameAction.UNIT_NEXT_IDLE, com.oddlabs.tt.input.GameAction.NOTIFICATION_JUMP, com.oddlabs.tt.input.GameAction.GAME_SPEED_UP, com.oddlabs.tt.input.GameAction.GAME_SPEED_DOWN, com.oddlabs.tt.input.GameAction.ARMY_SELECT_0, com.oddlabs.tt.input.GameAction.ARMY_SELECT_1, com.oddlabs.tt.input.GameAction.ARMY_SELECT_2, com.oddlabs.tt.input.GameAction.ARMY_SELECT_3, com.oddlabs.tt.input.GameAction.ARMY_SELECT_4, com.oddlabs.tt.input.GameAction.ARMY_SELECT_5, com.oddlabs.tt.input.GameAction.ARMY_SELECT_6, com.oddlabs.tt.input.GameAction.ARMY_SELECT_7, com.oddlabs.tt.input.GameAction.ARMY_SELECT_8, com.oddlabs.tt.input.GameAction.ARMY_SELECT_9, com.oddlabs.tt.input.GameAction.ARMY_CREATE_0, com.oddlabs.tt.input.GameAction.ARMY_CREATE_1, com.oddlabs.tt.input.GameAction.ARMY_CREATE_2, com.oddlabs.tt.input.GameAction.ARMY_CREATE_3, com.oddlabs.tt.input.GameAction.ARMY_CREATE_4, com.oddlabs.tt.input.GameAction.ARMY_CREATE_5, com.oddlabs.tt.input.GameAction.ARMY_CREATE_6, com.oddlabs.tt.input.GameAction.ARMY_CREATE_7, com.oddlabs.tt.input.GameAction.ARMY_CREATE_8, com.oddlabs.tt.input.GameAction.ARMY_CREATE_9 };
            KeyBindingConflicts.UNIT_CONTEXT = new com.oddlabs.tt.input.GameAction[] { com.oddlabs.tt.input.GameAction.UNIT_MOVE, com.oddlabs.tt.input.GameAction.UNIT_ATTACK, com.oddlabs.tt.input.GameAction.UNIT_GATHER, com.oddlabs.tt.input.GameAction.UNIT_BUILD_QUARTERS, com.oddlabs.tt.input.GameAction.UNIT_BUILD_ARMORY, com.oddlabs.tt.input.GameAction.UNIT_BUILD_TOWER, com.oddlabs.tt.input.GameAction.UNIT_BUILD_SHIP, com.oddlabs.tt.input.GameAction.UNIT_EXIT_TOWER, com.oddlabs.tt.input.GameAction.MAGIC_1, com.oddlabs.tt.input.GameAction.MAGIC_2, com.oddlabs.tt.input.GameAction.MAGIC_3, com.oddlabs.tt.input.GameAction.MAGIC_4, com.oddlabs.tt.input.GameAction.MAGIC_5, com.oddlabs.tt.input.GameAction.MAGIC_6, com.oddlabs.tt.input.GameAction.UNIT_BUILD_PALISADE, com.oddlabs.tt.input.GameAction.UNIT_BUILD_SHRINE, com.oddlabs.tt.input.GameAction.UNIT_BUILD_HALL, com.oddlabs.tt.input.GameAction.FORMATION_SQUARE, com.oddlabs.tt.input.GameAction.FORMATION_DIAMOND, com.oddlabs.tt.input.GameAction.FORMATION_CIRCLE, com.oddlabs.tt.input.GameAction.FORMATION_TIGHT, com.oddlabs.tt.input.GameAction.FORMATION_LOOSE, com.oddlabs.tt.input.GameAction.FORMATION_DEFAULT, com.oddlabs.tt.input.GameAction.FORMATION_STAR, com.oddlabs.tt.input.GameAction.STAND_GROUND, com.oddlabs.tt.input.GameAction.UNIT_GUARD, com.oddlabs.tt.input.GameAction.UNIT_PATROL, com.oddlabs.tt.input.GameAction.GAMEPLAY_BACK };
            KeyBindingConflicts.BUILDING_CONTEXT = new com.oddlabs.tt.input.GameAction[] { com.oddlabs.tt.input.GameAction.PROD_WEAPONS, com.oddlabs.tt.input.GameAction.PROD_HARVEST, com.oddlabs.tt.input.GameAction.PROD_ARMY, com.oddlabs.tt.input.GameAction.PROD_TRANSPORT, com.oddlabs.tt.input.GameAction.UNIT_SET_RALLY, com.oddlabs.tt.input.GameAction.TRAIN_CHIEFTAIN, com.oddlabs.tt.input.GameAction.PROD_RESEARCH };
            KeyBindingConflicts.RESOURCE_CONTEXT = new com.oddlabs.tt.input.GameAction[] { com.oddlabs.tt.input.GameAction.RES_TREE, com.oddlabs.tt.input.GameAction.RES_TREE_DEC, com.oddlabs.tt.input.GameAction.RES_TREE_BATCH, com.oddlabs.tt.input.GameAction.RES_TREE_BATCH_DEC, com.oddlabs.tt.input.GameAction.RES_ROCK, com.oddlabs.tt.input.GameAction.RES_ROCK_DEC, com.oddlabs.tt.input.GameAction.RES_ROCK_BATCH, com.oddlabs.tt.input.GameAction.RES_ROCK_BATCH_DEC, com.oddlabs.tt.input.GameAction.RES_IRON, com.oddlabs.tt.input.GameAction.RES_IRON_DEC, com.oddlabs.tt.input.GameAction.RES_IRON_BATCH, com.oddlabs.tt.input.GameAction.RES_IRON_BATCH_DEC, com.oddlabs.tt.input.GameAction.RES_CHICKEN, com.oddlabs.tt.input.GameAction.RES_CHICKEN_DEC, com.oddlabs.tt.input.GameAction.RES_CHICKEN_BATCH, com.oddlabs.tt.input.GameAction.RES_CHICKEN_BATCH_DEC };
            KeyBindingConflicts.UI_NAV = new com.oddlabs.tt.input.GameAction[] { com.oddlabs.tt.input.GameAction.UI_ACTIVATE, com.oddlabs.tt.input.GameAction.UI_CANCEL, com.oddlabs.tt.input.GameAction.UI_FOCUS_NEXT, com.oddlabs.tt.input.GameAction.UI_FOCUS_PREV, com.oddlabs.tt.input.GameAction.UI_NEXT_PANEL, com.oddlabs.tt.input.GameAction.UI_PREV_PANEL, com.oddlabs.tt.input.GameAction.UI_NAV_UP, com.oddlabs.tt.input.GameAction.UI_NAV_DOWN, com.oddlabs.tt.input.GameAction.UI_NAV_LEFT, com.oddlabs.tt.input.GameAction.UI_NAV_RIGHT, com.oddlabs.tt.input.GameAction.UI_NAV_HOME, com.oddlabs.tt.input.GameAction.UI_NAV_END, com.oddlabs.tt.input.GameAction.UI_NAV_PAGE_UP, com.oddlabs.tt.input.GameAction.UI_NAV_PAGE_DOWN, com.oddlabs.tt.input.GameAction.UI_BACKSPACE, com.oddlabs.tt.input.GameAction.UI_DELETE };
            KeyBindingConflicts.PEON_CONTEXT = new com.oddlabs.tt.input.GameAction[] { com.oddlabs.tt.input.GameAction.TRAIN_PEON, com.oddlabs.tt.input.GameAction.TRAIN_PEON_DEC, com.oddlabs.tt.input.GameAction.TRAIN_PEON_BATCH, com.oddlabs.tt.input.GameAction.TRAIN_PEON_BATCH_DEC };
            KeyBindingConflicts.QUARTERS_CONTEXT = new com.oddlabs.tt.input.GameAction[] { com.oddlabs.tt.input.GameAction.TRAIN_PEON, com.oddlabs.tt.input.GameAction.TRAIN_PEON_DEC, com.oddlabs.tt.input.GameAction.TRAIN_PEON_BATCH, com.oddlabs.tt.input.GameAction.TRAIN_PEON_BATCH_DEC, com.oddlabs.tt.input.GameAction.TRAIN_CHIEFTAIN, com.oddlabs.tt.input.GameAction.UNIT_SET_RALLY };
            KeyBindingConflicts.EXTENDED_ARMY_CONTEXT = new com.oddlabs.tt.input.GameAction[] { com.oddlabs.tt.input.GameAction.TRAIN_CHAMPION, com.oddlabs.tt.input.GameAction.TRAIN_CHAMPION_DEC, com.oddlabs.tt.input.GameAction.TRAIN_SCOUT, com.oddlabs.tt.input.GameAction.TRAIN_SCOUT_DEC, com.oddlabs.tt.input.GameAction.TRAIN_SHAMAN, com.oddlabs.tt.input.GameAction.TRAIN_SHAMAN_DEC, com.oddlabs.tt.input.GameAction.TRAIN_SLINGER, com.oddlabs.tt.input.GameAction.TRAIN_SLINGER_DEC, com.oddlabs.tt.input.GameAction.TRAIN_MELEE_LIGHT, com.oddlabs.tt.input.GameAction.TRAIN_MELEE_LIGHT_DEC, com.oddlabs.tt.input.GameAction.TRAIN_MELEE_HEAVY, com.oddlabs.tt.input.GameAction.TRAIN_MELEE_HEAVY_DEC, com.oddlabs.tt.input.GameAction.TRAIN_UNIQUE_1, com.oddlabs.tt.input.GameAction.TRAIN_UNIQUE_1_DEC, com.oddlabs.tt.input.GameAction.TRAIN_UNIQUE_2, com.oddlabs.tt.input.GameAction.TRAIN_UNIQUE_2_DEC, com.oddlabs.tt.input.GameAction.TRAIN_SPINNER, com.oddlabs.tt.input.GameAction.TRAIN_SPINNER_DEC };
            KeyBindingConflicts.EXTENDED_HARVEST_CONTEXT = new com.oddlabs.tt.input.GameAction[] { com.oddlabs.tt.input.GameAction.RES_TREE, com.oddlabs.tt.input.GameAction.RES_TREE_DEC, com.oddlabs.tt.input.GameAction.RES_ROCK, com.oddlabs.tt.input.GameAction.RES_ROCK_DEC, com.oddlabs.tt.input.GameAction.RES_IRON, com.oddlabs.tt.input.GameAction.RES_IRON_DEC, com.oddlabs.tt.input.GameAction.RES_CHICKEN, com.oddlabs.tt.input.GameAction.RES_CHICKEN_DEC, com.oddlabs.tt.input.GameAction.RES_GOLD, com.oddlabs.tt.input.GameAction.RES_GOLD_DEC, com.oddlabs.tt.input.GameAction.RES_CRYSTAL, com.oddlabs.tt.input.GameAction.RES_CRYSTAL_DEC, com.oddlabs.tt.input.GameAction.RES_HERBS, com.oddlabs.tt.input.GameAction.RES_HERBS_DEC, com.oddlabs.tt.input.GameAction.RES_COAL, com.oddlabs.tt.input.GameAction.RES_COAL_DEC, com.oddlabs.tt.input.GameAction.RES_DIAMOND, com.oddlabs.tt.input.GameAction.RES_DIAMOND_DEC };
            KeyBindingConflicts.IN_GAME_CONTEXTS = new com.oddlabs.tt.input.GameAction[][] { com.oddlabs.tt.input.KeyBindingConflicts.UNIT_CONTEXT, com.oddlabs.tt.input.KeyBindingConflicts.BUILDING_CONTEXT, com.oddlabs.tt.input.KeyBindingConflicts.PEON_CONTEXT, com.oddlabs.tt.input.KeyBindingConflicts.RESOURCE_CONTEXT, com.oddlabs.tt.input.KeyBindingConflicts.EXTENDED_ARMY_CONTEXT, com.oddlabs.tt.input.KeyBindingConflicts.EXTENDED_HARVEST_CONTEXT };
            KeyBindingConflicts.CONFLICT_GROUPS = new com.oddlabs.tt.input.GameAction[][] { com.oddlabs.tt.input.KeyBindingConflicts.ALWAYS_ACTIVE, com.oddlabs.tt.input.KeyBindingConflicts.UNIT_CONTEXT, com.oddlabs.tt.input.KeyBindingConflicts.PEON_CONTEXT, com.oddlabs.tt.input.KeyBindingConflicts.QUARTERS_CONTEXT, com.oddlabs.tt.input.KeyBindingConflicts.BUILDING_CONTEXT, com.oddlabs.tt.input.KeyBindingConflicts.RESOURCE_CONTEXT, com.oddlabs.tt.input.KeyBindingConflicts.EXTENDED_ARMY_CONTEXT, com.oddlabs.tt.input.KeyBindingConflicts.EXTENDED_HARVEST_CONTEXT, com.oddlabs.tt.input.KeyBindingConflicts.UI_NAV };
            KeyBindingConflicts.CONFLICT_MAP = com.oddlabs.tt.input.KeyBindingConflicts.buildConflictMap();
        }
        public KeyBindingConflicts() : base()
        {
        }
    }
}
