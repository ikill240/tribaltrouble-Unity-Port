// Translated from Picker.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.render
{
    // Java: 47
    public class Picker : java.lang.Object, com.oddlabs.tt.animation.Updatable
    {
        public const int PICK_SIZE = 5;
        public const int SELECTION_THRESHOLD = 5;
        public const float PATCH_PICK_PRECISION = 0.1f;
        public const float PATCH_PICK_STEP = 2.0f;
        public const float TOOL_TIP_DELAY = 0.1f;
        public org.joml.Matrix4f proj;
        public java.nio.IntBuffer viewport;
        public float[] hit_result_array;
        public float[] dir_vector;
        public int[] viewportArray;
        public java.util.List element_pick_list;
        public java.util.List tree_pick_list;
        public com.oddlabs.tt.camera.CameraState tmp_camera;
        public com.oddlabs.tt.render.PortPicker port_picker;
        public com.oddlabs.tt.util.BoundingBox world_box;
        public com.oddlabs.tt.render.RespondManager respond_manager;
        public com.oddlabs.tt.player.Player local_player;
        public com.oddlabs.tt.gui.GUIRoot gui_root;
        public com.oddlabs.tt.util.Target current_hovered;
        public com.oddlabs.tt.gui.ToolTip current_tooltip;
        public com.oddlabs.tt.animation.TimerAnimation tool_tip_timer;
        public bool render_tool_tip;
        public float patch_hit_x;
        public float patch_hit_y;
        public float patch_hit_z;
        public com.oddlabs.tt.model.Selectable[] old_target_selection;
        public com.oddlabs.tt.model.Action old_target_action;
        public bool old_target_aggressive;
        public int old_landscape_target_grid_x;
        public int old_landscape_target_grid_y;
        public com.oddlabs.tt.util.Target old_set_target_target;
        // Java: 97
        public com.oddlabs.tt.render.RespondManager getRespondManager()
        {
            return respond_manager;
        }
        public bool prefer_units;
        // Java: 103
        public com.oddlabs.tt.util.Target getNearestUnitFirst(java.util.List pick_list, System.Type filter)
        {
            prefer_units = true;
            try
            {
                return getNearestPick(pick_list, filter);
            }
            finally
            {
                prefer_units = false;
            }
        }
        // Java: 113
        public com.oddlabs.tt.util.Target getNearestPick(java.util.List pick_list, System.Type filter)
        {
            com.oddlabs.tt.util.Target nearest_pickable = null;
            float nearest_squared_distance = java.lang.Float.POSITIVE_INFINITY;
            com.oddlabs.tt.util.Target nearest_unit = null;
            float nearest_unit_distance = java.lang.Float.POSITIVE_INFINITY;
            for (int i = 0; i < pick_list.size(); i++)
            {
                com.oddlabs.tt.util.Target pickable = ((com.oddlabs.tt.util.Target)pick_list.get(i));
                pick_list.set(i, null);
                float squared_distance = com.oddlabs.tt.render.PortPicker.cameraDistanceSquared(pickable, tmp_camera.getCurrentX(), tmp_camera.getCurrentY(), tmp_camera.getCurrentZ());
                if (filter.isInstance(pickable) && squared_distance < nearest_squared_distance)
                {
                    nearest_squared_distance = squared_distance;
                    nearest_pickable = pickable;
                }
                if (filter.isInstance(pickable) && (pickable is com.oddlabs.tt.model.Unit) && squared_distance < nearest_unit_distance)
                {
                    nearest_unit_distance = squared_distance;
                    nearest_unit = pickable;
                }
            }
            if (nearest_unit != null && prefer_units && local_player.getWorld().isExtended())
            {
                return nearest_unit;
            }
            return nearest_pickable;
        }
        // Java: 141
        public float getScale()
        {
            return gui_root.getGlobalScale();
        }
        // Java: 145
        public void pickSailingTarget(com.oddlabs.tt.model.Army selected_army, com.oddlabs.tt.camera.CameraState camera, com.oddlabs.tt.player.PlayerInterface player_interface, int x, int y)
        {
            float scale = getScale();
            setupPicking(camera, (float)(x * scale), (float)(y * scale), com.oddlabs.tt.render.Picker.PICK_SIZE, com.oddlabs.tt.render.Picker.PICK_SIZE);
            pickObjects();
            com.oddlabs.tt.util.Target target = getNearestPick(element_pick_list, typeof(com.oddlabs.tt.util.Target));
            com.oddlabs.tt.model.Selectable[] selection = selected_army.filter(com.oddlabs.tt.model.Abilities.SAIL);
            if ((target is com.oddlabs.tt.model.Unit) || (target is com.oddlabs.tt.model.Building))
            {
                player_interface.setSailingTarget(selection, target);
            }
            else if (nearestLandscape(java.lang.Math.round((float)(x * scale)), java.lang.Math.round((float)(y * scale))))
            {
                com.oddlabs.tt.pathfinder.UnitGrid grid = local_player.getWorld().getUnitGrid();
                int grid_x = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(patch_hit_x);
                int grid_y = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(patch_hit_y);
                com.oddlabs.tt.render.PortPicker.addRespond(new com.oddlabs.tt.landscape.LandscapeTargetRespond(local_player.getWorld(), patch_hit_x, patch_hit_y));
                player_interface.setSailingTarget(selection, grid_x, grid_y);
            }
        }
        // Java: 175
        public void pickTarget(com.oddlabs.tt.model.Army selected_army, com.oddlabs.tt.camera.CameraState camera, com.oddlabs.tt.player.PlayerInterface player_interface, int x, int y, com.oddlabs.tt.model.Action action)
        {
            pickTarget(selected_army, camera, player_interface, x, y, action, false);
        }
        // Java: 181
        public void pickTarget(com.oddlabs.tt.model.Army selected_army, com.oddlabs.tt.camera.CameraState camera, com.oddlabs.tt.player.PlayerInterface player_interface, int x, int y, com.oddlabs.tt.model.Action action, bool queue)
        {
            float scale = getScale();
            setupPicking(camera, (float)(x * scale), (float)(y * scale), com.oddlabs.tt.render.Picker.PICK_SIZE, com.oddlabs.tt.render.Picker.PICK_SIZE);
            pickObjects();
            com.oddlabs.tt.util.Target nearest_pickable = getNearestPick(element_pick_list, typeof(com.oddlabs.tt.util.Target));
            com.oddlabs.tt.model.Selectable[] selection = selected_army.filter(com.oddlabs.tt.model.Abilities.TARGET);
            bool aggressive = com.oddlabs.tt.global.Settings.getSettings().aggressive_units;
            if (nearest_pickable != null)
            {
                if (!((nearest_pickable is com.oddlabs.tt.model.SceneryModel)) || (((com.oddlabs.tt.model.SceneryModel)nearest_pickable)).isOccupying())
                {
                    respond_manager.addResponder(nearest_pickable);
                }
                if (queue)
                {
                    player_interface.queueTarget(selection, nearest_pickable, action, aggressive);
                }
                else if (isNewSetTarget(selection, nearest_pickable, action, aggressive))
                {
                    player_interface.setTarget(selection, nearest_pickable, action, aggressive);
                }
            }
            else
            {
                pickResources();
                com.oddlabs.tt.landscape.TreeSupply supply = ((com.oddlabs.tt.landscape.TreeSupply)getNearestPick(tree_pick_list, typeof(com.oddlabs.tt.util.Target)));
                if (supply != null)
                {
                    respond_manager.addResponder(supply, new java.lang.Runnable_L(() =>
                    {
                        supply.changeRespondingTrees((-1));
                    }));
                    supply.changeRespondingTrees(1);
                    if (queue)
                    {
                        player_interface.queueTarget(selection, supply, action, aggressive);
                    }
                    else if (isNewSetTarget(selection, supply, action, aggressive))
                    {
                        player_interface.setTarget(selection, supply, action, aggressive);
                    }
                }
                else if (nearestLandscape(java.lang.Math.round((float)(x * scale)), java.lang.Math.round((float)(y * scale))))
                {
                    com.oddlabs.tt.render.PortPicker.addRespond(new com.oddlabs.tt.landscape.LandscapeTargetRespond(local_player.getWorld(), patch_hit_x, patch_hit_y));
                    int grid_x = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(patch_hit_x);
                    int grid_y = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(patch_hit_y);
                    if (queue)
                    {
                        player_interface.queueLandscapeTarget(selection, grid_x, grid_y, action, aggressive);
                    }
                    else if (isNewLandscapeTarget(selection, grid_x, grid_y, action, aggressive))
                    {
                        player_interface.setLandscapeTarget(selection, grid_x, grid_y, action, aggressive);
                    }
                }
            }
        }
        // Java: 219
        public bool isNewSetTarget(com.oddlabs.tt.model.Selectable[] selection, com.oddlabs.tt.util.Target target, com.oddlabs.tt.model.Action action, bool aggressive)
        {
            old_landscape_target_grid_x = (-1);
            old_landscape_target_grid_y = (-1);
            bool new_target = isNewOrder(selection, action, aggressive);
            new_target |= ((object)target != (object)old_set_target_target);
            old_set_target_target = target;
            return new_target;
        }
        // Java: 232
        public bool isNewLandscapeTarget(com.oddlabs.tt.model.Selectable[] selection, int grid_x, int grid_y, com.oddlabs.tt.model.Action action, bool aggressive)
        {
            old_set_target_target = null;
            bool new_target = isNewOrder(selection, action, aggressive);
            new_target |= grid_x != old_landscape_target_grid_x;
            new_target |= grid_y != old_landscape_target_grid_y;
            old_landscape_target_grid_x = grid_x;
            old_landscape_target_grid_y = grid_y;
            return new_target;
        }
        // Java: 246
        public bool isNewOrder(com.oddlabs.tt.model.Selectable[] selection, com.oddlabs.tt.model.Action action, bool aggressive)
        {
            bool new_order = false;
            if (selection.Length == old_target_selection.Length)
            {
                for (int i = 0; i < selection.Length; i++)
                {
                    new_order |= ((object)selection[i] != (object)old_target_selection[i]);
                }
            }
            else
            {
                new_order = true;
            }
            new_order |= ((object)action != (object)old_target_action);
            new_order |= aggressive != old_target_aggressive;
            old_target_selection = selection;
            old_target_action = action;
            old_target_aggressive = aggressive;
            return new_order;
        }
        // Java: 266
        public com.oddlabs.tt.model.Selectable[] pickBoxed(com.oddlabs.tt.camera.CameraState camera, int x1, int y1, int x2, int y2, int clicks)
        {
            return pickBoxed(camera, x1, y1, x2, y2, clicks, false);
        }
        // Java: 272
        public com.oddlabs.tt.model.Selectable[] pickBoxed(com.oddlabs.tt.camera.CameraState camera, int x1, int y1, int x2, int y2, int clicks, bool by_type)
        {
            float scale = gui_root.getGlobalScale();
            float sx1 = (float)(x1 * scale);
            float sy1 = (float)(y1 * scale);
            float sx2 = (float)(x2 * scale);
            float sy2 = (float)(y2 * scale);
            float cx = (float)(((float)(sx1 + sx2)) * 0.5f);
            float cy = (float)(((float)(sy1 + sy2)) * 0.5f);
            int width = java.lang.JC.f2i(java.lang.Math.abs((float)(sx1 - sx2))) + 1;
            int height = java.lang.JC.f2i(java.lang.Math.abs((float)(sy1 - sy2))) + 1;
            width = java.lang.Math.max(width, com.oddlabs.tt.render.Picker.PICK_SIZE);
            height = java.lang.Math.max(height, com.oddlabs.tt.render.Picker.PICK_SIZE);
            setupPicking(camera, cx, cy, width, height);
            pickObjects();
            return (java.lang.Math.abs(x1 - x2) < com.oddlabs.tt.render.Picker.SELECTION_THRESHOLD && java.lang.Math.abs(y1 - y2) < com.oddlabs.tt.render.Picker.SELECTION_THRESHOLD ? createSinglePick(camera, clicks, by_type) : createBoxedPick());
        }
        public com.oddlabs.tt.model.Selectable last_single_pick;
        // Java: 295
        public com.oddlabs.tt.model.Selectable getLastSinglePick()
        {
            return last_single_pick;
        }
        // Java: 299
        public com.oddlabs.tt.model.Selectable[] createSinglePick(com.oddlabs.tt.camera.CameraState camera, int clicks, bool by_type)
        {
            com.oddlabs.tt.model.Selectable nearest = ((com.oddlabs.tt.model.Selectable)getNearestUnitFirst(element_pick_list, typeof(com.oddlabs.tt.model.Selectable)));
            last_single_pick = nearest;
            if (nearest != null)
            {
                if (by_type && clicks > 1 && (nearest is com.oddlabs.tt.model.Unit) && !nearest.isDead())
                {
                    return pickAllOfSameType(camera, nearest);
                }
                if (clicks > 1 && (nearest is com.oddlabs.tt.model.Unit) && !nearest.isDead() && nearest.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC))
                {
                    return pickAllOfSameType(camera, nearest);
                }
                if (clicks > 1 && (nearest is com.oddlabs.tt.model.Building tower) && !tower.isDead() && (tower.getUnitContainer() is com.oddlabs.tt.model.MountUnitContainer mount_container) && mount_container.getUnit() != null)
                {
                    return new com.oddlabs.tt.model.Selectable[] { tower };
                }
                if (clicks > 1 && (nearest is com.oddlabs.tt.model.Building) && !nearest.isDead())
                {
                    return pickAllOfSameBuildingType(camera, nearest);
                }
                if (clicks > 1)
                {
                    if (nearest.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MELEE))
                    {
                        return pickAll(camera, com.oddlabs.tt.model.Abilities.MELEE, nearest.getIslandId(), nearest);
                    }
                    if (nearest.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.THROW))
                    {
                        return pickAll(camera, com.oddlabs.tt.model.Abilities.THROW, nearest.getIslandId(), nearest);
                    }
                    else if (nearest.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.HARVEST))
                    {
                        return pickAll(camera, com.oddlabs.tt.model.Abilities.HARVEST, nearest.getIslandId(), nearest);
                    }
                    else
                    {
                        return com.oddlabs.tt.model.Selectable.newArray(nearest);
                    }
                }
                else
                {
                    return com.oddlabs.tt.model.Selectable.newArray(nearest);
                }
            }
            else
            {
                return com.oddlabs.tt.model.Selectable.newArray(0);
            }
        }
        // Java: 358
        public com.oddlabs.tt.model.Selectable[] pickAllOfSameBuildingType(com.oddlabs.tt.camera.CameraState camera, com.oddlabs.tt.model.Selectable reference)
        {
            com.oddlabs.tt.model.Selectable[] complete_list = pickBoxed(camera, 0, 0, gui_root.getWidth() - 1, gui_root.getHeight() - 1, 1, false);
            int island = reference.getIslandId();
            java.util.List matches = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Selectable s in complete_list)
            {
                if (s != null && !s.isDead() && (s is com.oddlabs.tt.model.Building) && s.getIslandId() == island && ((object)s.getOwnerNoCheck() == (object)reference.getOwnerNoCheck()) && ((object)s.getTemplate() == (object)reference.getTemplate()))
                {
                    matches.add(s);
                }
            }
            if (matches.isEmpty())
            {
                return com.oddlabs.tt.model.Selectable.newArray(reference);
            }
            return ((com.oddlabs.tt.model.Selectable[])matches.toArray(new java.util.function.IntFunction_L((int a0) => { return com.oddlabs.tt.model.Selectable.newArray(a0); })));
        }
        // Java: 374
        public com.oddlabs.tt.model.Selectable[] createBoxedPick()
        {
            com.oddlabs.tt.model.Selectable[] array = ((com.oddlabs.tt.model.Selectable[])element_pick_list.stream().filter(new java.util.function.Predicate_L((object element_l) =>
            {
                com.oddlabs.tt.util.Target element = (com.oddlabs.tt.util.Target)element_l;
                return (element is com.oddlabs.tt.model.Selectable);
            })).toArray(new java.util.function.IntFunction_L((int a0) => { return com.oddlabs.tt.model.Selectable.newArray(a0); })));
            element_pick_list.clear();
            return array;
        }
        // Java: 386
        public com.oddlabs.tt.model.Selectable[] pickAllOfSameType(com.oddlabs.tt.camera.CameraState camera, com.oddlabs.tt.model.Selectable reference)
        {
            com.oddlabs.tt.model.Selectable[] complete_list = pickBoxed(camera, 0, 0, gui_root.getWidth() - 1, gui_root.getHeight() - 1, 1, false);
            int island = reference.getIslandId();
            java.util.List matches = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Selectable s in complete_list)
            {
                if (s != null && !s.isDead() && s.getIslandId() == island && com.oddlabs.tt.render.Picker.isSameType(reference, s))
                {
                    matches.add(s);
                }
            }
            if (matches.isEmpty())
            {
                return com.oddlabs.tt.model.Selectable.newArray(reference);
            }
            return ((com.oddlabs.tt.model.Selectable[])matches.toArray(new java.util.function.IntFunction_L((int a0) => { return com.oddlabs.tt.model.Selectable.newArray(a0); })));
        }
        // Java: 401
        public static bool isSameType(com.oddlabs.tt.model.Selectable reference, com.oddlabs.tt.model.Selectable other)
        {
            if (!((other is com.oddlabs.tt.model.Unit)))
            {
                return false;
            }
            if (((object)other.getOwnerNoCheck() != (object)reference.getOwnerNoCheck()))
            {
                return false;
            }
            if (((object)other.getTemplate() != (object)reference.getTemplate()))
            {
                return false;
            }
            if (!reference.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD))
            {
                return true;
            }
            return java.lang.JO.equals(other.getPrimaryController().getKey(), reference.getPrimaryController().getKey());
        }
        // Java: 425
        public com.oddlabs.tt.model.Selectable[] pickAll(com.oddlabs.tt.camera.CameraState camera, int ability_filter, int island, com.oddlabs.tt.model.Selectable reference)
        {
            com.oddlabs.tt.model.Selectable[] complete_list = pickBoxed(camera, 0, 0, gui_root.getWidth() - 1, gui_root.getHeight() - 1, 2, false);
            return ((com.oddlabs.tt.model.Selectable[])java.util.Arrays.stream(complete_list).filter(new java.util.function.Predicate_L((object s_l) =>
            {
                com.oddlabs.tt.model.Selectable s = (com.oddlabs.tt.model.Selectable)s_l;
                return s.getAbilities().hasAbilities(ability_filter) && s.getIslandId() == island && ((object)s.getOwnerNoCheck() == (object)reference.getOwnerNoCheck());
            })).toArray(new java.util.function.IntFunction_L((int a0) => { return com.oddlabs.tt.model.Selectable.newArray(a0); })));
        }
        // Java: 435
        public void pickRotate(com.oddlabs.tt.camera.GameCamera camera)
        {
            int x = gui_root.getWidth() / 2;
            int y = camera.getRotateY();
            float scale = getScale();
            setupPicking(camera.getState(), (float)(x * scale), (float)(y * scale), com.oddlabs.tt.render.Picker.PICK_SIZE, com.oddlabs.tt.render.Picker.PICK_SIZE);
            if (!nearestLandscape(java.lang.Math.round((float)(x * scale)), java.lang.Math.round((float)(y * scale))) || patch_hit_z < local_player.getWorld().getHeightMap().getSeaLevelMeters())
            {
                float dz = (float)(tmp_camera.getCurrentZ() - local_player.getWorld().getHeightMap().getSeaLevelMeters());
                float factor = (float)(dz / dir_vector[2]);
                patch_hit_x = (float)(tmp_camera.getCurrentX() - (float)(factor * dir_vector[0]));
                patch_hit_y = (float)(tmp_camera.getCurrentY() - (float)(factor * dir_vector[1]));
                patch_hit_z = local_player.getWorld().getHeightMap().getSeaLevelMeters();
            }
            int grid_x = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(patch_hit_x);
            int grid_y = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(patch_hit_y);
            camera.setRotationPoint(new com.oddlabs.tt.landscape.LandscapeTarget(grid_x, grid_y));
        }
        // Java: 453
        public void calcPosAndDir(int pixel_x, int pixel_y)
        {
            float pixel_z = 0.5f;
            unproject(pixel_x, pixel_y, pixel_z, tmp_camera.getProjectionModelView());
            float hit_x = hit_result_array[0];
            float hit_y = hit_result_array[1];
            float hit_z = hit_result_array[2];
            pixel_z = 0.1f;
            unproject(pixel_x, pixel_y, pixel_z, tmp_camera.getProjectionModelView());
            float dx = (float)(hit_x - hit_result_array[0]);
            float dy = (float)(hit_y - hit_result_array[1]);
            float dz = (float)(hit_z - hit_result_array[2]);
            float vec_len_inv = (float)(1.0f / ((float)(java.lang.Math.sqrt((float)((float)((float)(dx * dx) + (float)(dy * dy)) + (float)(dz * dz))))));
            dir_vector[0] = (float)(dx * vec_len_inv);
            dir_vector[1] = (float)(dy * vec_len_inv);
            dir_vector[2] = (float)(dz * vec_len_inv);
        }
        // Java: 472
        public bool nearestLandscape(int pixel_x, int pixel_y)
        {
            pickLandscape();
            calcPosAndDir(pixel_x, pixel_y);
            return doNearestLandscape(hit_result_array[0], hit_result_array[1], hit_result_array[2], dir_vector[0], dir_vector[1], dir_vector[2]);
        }
        // Java: 487
        public void unproject(float winx, float winy, float winz, org.joml.Matrix4f proj)
        {
            viewport.get(0, viewportArray, 0, 4);
            org.joml.Vector3f tempVector = new org.joml.Vector3f();
            proj.unproject(winx, winy, winz, viewportArray, tempVector);
            hit_result_array[0] = tempVector.x_f;
            hit_result_array[1] = tempVector.y_f;
            hit_result_array[2] = tempVector.z_f;
        }
        // Java: 500
        public static float computeTMax(float bmin, float bmax, float c, float d)
        {
            if (d == 0)
            {
                return java.lang.Float.POSITIVE_INFINITY;
            }
            float t1 = (float)(((float)(bmin - c)) / d);
            float t2 = (float)(((float)(bmax - c)) / d);
            return java.lang.Math.max(t1, t2);
        }
        // Java: 509
        public static float computeTMin(float bmin, float bmax, float c, float d)
        {
            if (d == 0)
            {
                return java.lang.Float.NEGATIVE_INFINITY;
            }
            float t1 = (float)(((float)(bmin - c)) / d);
            float t2 = (float)(((float)(bmax - c)) / d);
            return java.lang.Math.min(t1, t2);
        }
        // Java: 518
        public bool doNearestLandscape(float x, float y, float z, float dx, float dy, float dz)
        {
            if (getHeight(x, y) > z)
            {
                return false;
            }
            foreach (com.oddlabs.tt.util.BoundingBox bb in new com.oddlabs.tt.util.BoundingBox[] { world_box })
            {
                float tx_min = com.oddlabs.tt.render.Picker.computeTMin(bb.bmin_x, bb.bmax_x, x, dx);
                float ty_min = com.oddlabs.tt.render.Picker.computeTMin(bb.bmin_y, bb.bmax_y, y, dy);
                float tz_min = com.oddlabs.tt.render.Picker.computeTMin(bb.bmin_z, bb.bmax_z, z, dz);
                float tx_max = com.oddlabs.tt.render.Picker.computeTMax(bb.bmin_x, bb.bmax_x, x, dx);
                float ty_max = com.oddlabs.tt.render.Picker.computeTMax(bb.bmin_y, bb.bmax_y, y, dy);
                float tz_max = com.oddlabs.tt.render.Picker.computeTMax(bb.bmin_z, bb.bmax_z, z, dz);
                float t_min = java.lang.Math.max(tx_min, java.lang.Math.max(ty_min, tz_min));
                float t_max = java.lang.Math.min(tx_max, java.lang.Math.min(ty_max, tz_max));
                if (t_min < 0)
                {
                    t_min = 0;
                }
                if (t_min >= t_max)
                {
                    continue;
                }
                float t_min_x = (float)(x + (float)(t_min * dx));
                float t_min_y = (float)(y + (float)(t_min * dy));
                float t_min_z = (float)(z + (float)(t_min * dz));
                float t_min_height = getHeight(t_min_x, t_min_y);
                if (t_min_height >= (float)(0.001f + t_min_z))
                {
                    return false;
                }
                bool found_t_range = false;
                for (float t_scan = t_min; t_scan <= t_max; t_scan = (float)(t_scan + com.oddlabs.tt.render.Picker.PATCH_PICK_STEP))
                {
                    float t_scan_next = java.lang.Math.min((float)(t_scan + com.oddlabs.tt.render.Picker.PATCH_PICK_STEP), t_max);
                    float t_scan_x = (float)(x + (float)(t_scan_next * dx));
                    float t_scan_y = (float)(y + (float)(t_scan_next * dy));
                    float t_scan_z = (float)(z + (float)(t_scan_next * dz));
                    float t_scan_height = getHeight(t_scan_x, t_scan_y);
                    if (t_scan_height >= (float)(t_scan_z - 0.001f))
                    {
                        t_min = t_scan;
                        t_max = t_scan_next;
                        found_t_range = true;
                        break;
                    }
                }
                if (!found_t_range)
                {
                    continue;
                }
                float t_mid_x = default;
                float t_mid_y = default;
                float t_mid_z = default;
                float t_mid_height = default;
                float height_diff = default;
                float old_t_mid = default;
                float t_mid = java.lang.Float.NaN;
                do
                {
                    old_t_mid = t_mid;
                    t_mid = (float)(((float)(t_max + t_min)) * 0.5f);
                    t_mid_x = (float)(x + (float)(t_mid * dx));
                    t_mid_y = (float)(y + (float)(t_mid * dy));
                    t_mid_z = (float)(z + (float)(t_mid * dz));
                    t_mid_height = getHeight(t_mid_x, t_mid_y);
                    height_diff = (float)(t_mid_height - t_mid_z);
                    if (height_diff >= 0)
                    {
                        t_max = t_mid;
                    }
                    else
                    {
                        t_min = t_mid;
                    }
                }
                while (java.lang.Math.abs(height_diff) > com.oddlabs.tt.render.Picker.PATCH_PICK_PRECISION && t_mid != old_t_mid);
                patch_hit_x = t_mid_x;
                patch_hit_y = t_mid_y;
                patch_hit_z = t_mid_height;
                return true;
            }
            return false;
        }
        // Java: 590
        public float getHeight(float x, float y)
        {
            return java.lang.Math.max(local_player.getWorld().getHeightMap().getNearestHeight(x, y), local_player.getWorld().getHeightMap().getSeaLevelMeters());
        }
        // Java: 595
        public void pickMapGoto(int x, int y, com.oddlabs.tt.camera.MapCamera camera)
        {
            float scale = getScale();
            setupPicking(camera.getState(), (float)(x * scale), (float)(y * scale), com.oddlabs.tt.render.Picker.PICK_SIZE, com.oddlabs.tt.render.Picker.PICK_SIZE);
            if (nearestLandscape(java.lang.Math.round((float)(x * scale)), java.lang.Math.round((float)(y * scale))))
            {
                camera.mapGoto(patch_hit_x, patch_hit_y);
            }
        }
        // Java: 602
        public com.oddlabs.tt.util.Target pickRallyPoint(com.oddlabs.tt.camera.CameraState camera, int x, int y, com.oddlabs.tt.model.Building building)
        {
            float scale = getScale();
            setupPicking(camera, (float)(x * scale), (float)(y * scale), com.oddlabs.tt.render.Picker.PICK_SIZE, com.oddlabs.tt.render.Picker.PICK_SIZE);
            pickObjects();
            com.oddlabs.tt.util.Target nearest = getNearestPick(element_pick_list, typeof(com.oddlabs.tt.util.Target));
            if ((nearest is com.oddlabs.tt.model.Building))
            {
                return nearest;
            }
            else if (nearestLandscape(java.lang.Math.round((float)(x * scale)), java.lang.Math.round((float)(y * scale))))
            {
                int grid_x = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(patch_hit_x);
                int grid_y = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(patch_hit_y);
                return building.getUnitGrid().findGridTargets(grid_x, grid_y, 1, false)[0];
            }
            else
            {
                return null;
            }
        }
        // Java: 618
        public void pickHoverPhysical(com.oddlabs.tt.camera.CameraState camera, int physical_x, int physical_y)
        {
            setupPicking(camera, physical_x, physical_y, com.oddlabs.tt.render.Picker.PICK_SIZE, com.oddlabs.tt.render.Picker.PICK_SIZE);
            pickObjects();
            com.oddlabs.tt.util.Target nearest = getNearestUnitFirst(element_pick_list, typeof(com.oddlabs.tt.util.Target));
            com.oddlabs.tt.util.Target new_current_hovered = default;
            if (nearest != null)
            {
                new_current_hovered = nearest;
            }
            else
            {
                pickResources();
                new_current_hovered = ((com.oddlabs.tt.landscape.TreeSupply)getNearestPick(tree_pick_list, typeof(com.oddlabs.tt.util.Target)));
            }
            if (((object)current_hovered != (object)new_current_hovered))
            {
                tool_tip_timer.resetTime();
                bool old_tip = (current_hovered is com.oddlabs.tt.model.ModelToolTip);
                bool new_tip = (new_current_hovered is com.oddlabs.tt.model.ModelToolTip);
                if (!old_tip && new_tip)
                {
                    tool_tip_timer.start();
                    render_tool_tip = false;
                }
                if (old_tip && !new_tip)
                {
                    if (!render_tool_tip)
                    {
                        tool_tip_timer.stop();
                    }
                    else
                    {
                        render_tool_tip = false;
                    }
                }
                current_hovered = new_current_hovered;
                current_tooltip = (new_tip ? (com.oddlabs.tt.gui.ToolTip)(new com.oddlabs.tt.render.ToolTipAdapter(((com.oddlabs.tt.model.ModelToolTip)current_hovered), local_player)) : (com.oddlabs.tt.gui.ToolTip)(null));
            }
        }
        // Java: 648
        public void pickHover(com.oddlabs.tt.camera.CameraState camera, int x, int y)
        {
            float scale = gui_root.getGlobalScale();
            setupPicking(camera, (float)(x * scale), (float)(y * scale), com.oddlabs.tt.render.Picker.PICK_SIZE, com.oddlabs.tt.render.Picker.PICK_SIZE);
            pickObjects();
            com.oddlabs.tt.util.Target nearest = getNearestUnitFirst(element_pick_list, typeof(com.oddlabs.tt.util.Target));
            com.oddlabs.tt.util.Target new_current_hovered = default;
            if (nearest != null)
            {
                new_current_hovered = nearest;
            }
            else
            {
                pickResources();
                new_current_hovered = ((com.oddlabs.tt.landscape.TreeSupply)getNearestPick(tree_pick_list, typeof(com.oddlabs.tt.util.Target)));
            }
            if (((object)current_hovered != (object)new_current_hovered))
            {
                tool_tip_timer.resetTime();
                bool old_tip = (current_hovered is com.oddlabs.tt.model.ModelToolTip);
                bool new_tip = (new_current_hovered is com.oddlabs.tt.model.ModelToolTip);
                if (!old_tip && new_tip)
                {
                    tool_tip_timer.start();
                    render_tool_tip = false;
                }
                if (old_tip && !new_tip)
                {
                    if (!render_tool_tip)
                    {
                        tool_tip_timer.stop();
                    }
                    else
                    {
                        render_tool_tip = false;
                    }
                }
                current_hovered = new_current_hovered;
                current_tooltip = (new_tip ? (com.oddlabs.tt.gui.ToolTip)(new com.oddlabs.tt.render.ToolTipAdapter(((com.oddlabs.tt.model.ModelToolTip)current_hovered), local_player)) : (com.oddlabs.tt.gui.ToolTip)(null));
            }
        }
        // Java: 679
        public void update(object anim_p)
        {
            com.oddlabs.tt.animation.TimerAnimation anim = (com.oddlabs.tt.animation.TimerAnimation)anim_p;
            render_tool_tip = true;
            tool_tip_timer.stop();
        }
        // Java: 685
        public com.oddlabs.tt.gui.ToolTip getCurrentToolTip()
        {
            return (canRenderToolTip() ? current_tooltip : (com.oddlabs.tt.gui.ToolTip)(null));
        }
        // Java: 689
        public com.oddlabs.tt.util.Target getCurrentHovered()
        {
            return current_hovered;
        }
        // Java: 693
        public bool canRenderToolTip()
        {
            return render_tool_tip;
        }
        // Java: 697
        public void resetCurrentHovered()
        {
            current_hovered = null;
            current_tooltip = null;
        }
        // Java: 702
        public bool pickLocation(com.oddlabs.tt.camera.CameraState camera, com.oddlabs.tt.render.LandscapeLocation landscape_location)
        {
            int x = com.oddlabs.tt.render.Renderer.getLocalInput().getMouseX();
            int y = com.oddlabs.tt.render.Renderer.getLocalInput().getMouseY();
            setupPicking(camera, x, y, com.oddlabs.tt.render.Picker.PICK_SIZE, com.oddlabs.tt.render.Picker.PICK_SIZE);
            if (!nearestLandscape(x, y))
            {
                return false;
            }
            landscape_location.x = patch_hit_x;
            landscape_location.y = patch_hit_y;
            return true;
        }
        // Java: 713
        public void setupPicking(com.oddlabs.tt.camera.CameraState camera, float x_center, float y_center, int width, int height)
        {
            proj.identity();
            viewport.clear();
            com.oddlabs.tt.window.Window window = com.oddlabs.tt.render.Renderer.getRenderer().getWindow();
            viewport.put(0).put(0).put(window.getWidth()).put(window.getHeight());
            viewport.flip();
            if (width > 0 && height > 0)
            {
                org.joml.Vector3f temp_vector = new org.joml.Vector3f((float)(((float)(viewport.get(2) - (float)(2 * ((float)(x_center - viewport.get(0)))))) / width), (float)(((float)(viewport.get(3) - (float)(2 * ((float)(y_center - viewport.get(1)))))) / height), 0);
                proj.translate(temp_vector.x_f, temp_vector.y_f, temp_vector.z_f);
                temp_vector.set((float)(((float)(viewport.get(2))) / width), (float)(((float)(viewport.get(3))) / height), 1.0f);
                proj.scale(temp_vector.x_f, temp_vector.y_f, temp_vector.z_f);
            }
            gui_root.multProjection(proj);
            tmp_camera.set(camera);
            tmp_camera.setView(proj, width, height);
        }
        // Java: 734
        public void pickLandscape()
        {
            com.oddlabs.tt.landscape.HeightMap hm = local_player.getWorld().getHeightMap();
            float size = hm.getMetersPerWorld();
            world_box.setBounds(0.0f, size, 0.0f, size, (float)(-10.0f), 1000.0f);
        }
        // Java: 740
        public void pickObjects()
        {
            element_pick_list.clear();
            port_picker.pickObjects(local_player.getWorld(), tmp_camera, element_pick_list);
        }
        // Java: 745
        public void pickResources()
        {
            tree_pick_list.clear();
            port_picker.pickTrees(local_player.getWorld(), tmp_camera, tree_pick_list);
        }
        // Java: 750
        public void debugRender()
        {
            com.oddlabs.tt.util.DebugRender.drawPoint(patch_hit_x, patch_hit_y, patch_hit_z, 10.0f, 1.0f, 1.0f, 1.0f);
        }
        // Java: 754
        public class LandscapeLeafComparator : java.lang.Object, java.util.Comparator
        {
            public com.oddlabs.tt.render.Picker this_0;
            // Java: 755
            public int compare(com.oddlabs.tt.camera.CameraState camera_state, com.oddlabs.tt.landscape.LandscapeLeaf l1, com.oddlabs.tt.landscape.LandscapeLeaf l2)
            {
                float l1_dist = com.oddlabs.tt.render.RenderTools.getCameraDistanceXYSquared(l1, camera_state.getCurrentX(), camera_state.getCurrentY());
                float l2_dist = com.oddlabs.tt.render.RenderTools.getCameraDistanceXYSquared(l2, camera_state.getCurrentX(), camera_state.getCurrentY());
                if (l1_dist < l2_dist)
                {
                    return (-1);
                }
                else if (l1_dist > l2_dist)
                {
                    return 1;
                }
                else if (l1.bmin_x < l2.bmin_x)
                {
                    return (-1);
                }
                else if (l1.bmin_x > l2.bmin_x)
                {
                    return 1;
                }
                else if (l1.bmin_y < l2.bmin_y)
                {
                    return (-1);
                }
                else if (l1.bmin_y > l2.bmin_y)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            // Java: 778
            public int compare(object l1_p, object l2_p)
            {
                com.oddlabs.tt.landscape.LandscapeLeaf l1 = (com.oddlabs.tt.landscape.LandscapeLeaf)l1_p;
                com.oddlabs.tt.landscape.LandscapeLeaf l2 = (com.oddlabs.tt.landscape.LandscapeLeaf)l2_p;
                return compare(this_0.tmp_camera, l1, l2);
            }
            public LandscapeLeafComparator(com.oddlabs.tt.render.Picker this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        private void init_Picker()
        {
            this.proj = new org.joml.Matrix4f();
            this.viewport = ((java.nio.IntBuffer)java.util.Objects.requireNonNull(org.lwjgl.BufferUtils.createIntBuffer(16)));
            this.hit_result_array = new float[3];
            this.dir_vector = new float[3];
            this.viewportArray = new int[4];
            this.element_pick_list = new java.util.ArrayList();
            this.tree_pick_list = new java.util.ArrayList();
            this.tmp_camera = new com.oddlabs.tt.camera.CameraState();
            this.port_picker = new com.oddlabs.tt.render.PortPicker();
            this.world_box = new com.oddlabs.tt.util.BoundingBox();
            this.tool_tip_timer = new com.oddlabs.tt.animation.TimerAnimation(this, com.oddlabs.tt.render.Picker.TOOL_TIP_DELAY);
            this.render_tool_tip = false;
            this.old_target_selection = com.oddlabs.tt.model.Selectable.newArray(0);
        }
        public Picker(com.oddlabs.tt.animation.AnimationManager manager, com.oddlabs.tt.player.Player local_player, com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.render.RenderQueues render_queues, com.oddlabs.tt.render.LandscapeRenderer landscape_renderer, com.oddlabs.tt.viewer.Selection selection) : base()
        {
            init_Picker();
            this.local_player = local_player;
            this.gui_root = gui_root;
            this.respond_manager = new com.oddlabs.tt.render.RespondManager(manager);
        }
    }
}
