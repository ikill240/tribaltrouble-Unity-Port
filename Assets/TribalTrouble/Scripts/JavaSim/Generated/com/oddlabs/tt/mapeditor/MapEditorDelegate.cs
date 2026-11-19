// Translated from MapEditorDelegate.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.mapeditor
{
    // Java: 78
    public class MapEditorDelegate : com.oddlabs.tt.@delegate.CameraDelegate, com.oddlabs.tt.camera.CameraHost, com.oddlabs.tt.camera.MapCameraOwner
    {
        void com.oddlabs.tt.camera.MapCameraOwner.addChild(com.oddlabs.tt.gui.GUIObject child) => addChild((com.oddlabs.tt.gui.Renderable)child);
        public const float MIN_RADIUS = 4.0f;
        public const float MAX_RADIUS = 96.0f;
        public const float RADIUS_STEP = 1.15f;
        public const int MIN_INTENSITY = 0;
        public const float RESOURCE_DAB_INTERVAL = 0.05f;
        public const int MAX_INTENSITY = 100;
        public const int INTENSITY_STEP = 5;
        public const int LABEL_WIDTH = 150;
        public const int PULLDOWN_WIDTH = 150;
        public const int HINT_WIDTH = 760;
        public com.oddlabs.net.NetworkSelector network;
        public com.oddlabs.tt.landscape.World world;
        public com.oddlabs.tt.animation.AnimationManager manager;
        public com.oddlabs.tt.render.Picker picker;
        public const float GROUND_UPDATE_INTERVAL = 0.1f;
        public com.oddlabs.tt.mapeditor.TerrainEditor editor;
        public com.oddlabs.tt.mapeditor.GroundTextures ground;
        public const float ACCESS_UPDATE_INTERVAL = 0.25f;
        public com.oddlabs.tt.mapeditor.AccessMap access_map;
        public com.oddlabs.tt.mapeditor.AccessOverlay access;
        public com.oddlabs.tt.gui.Label label_access_legend;
        public float access_timer;
        public float ground_timer;
        public bool ground_settle;
        public com.oddlabs.tt.mapeditor.MapSettings settings;
        public com.oddlabs.tt.mapeditor.ResourceLayer layer;
        public java.util.Deque history;
        public java.util.Deque future;
        public com.oddlabs.tt.mapeditor.ResourceLayer.Stroke resource_stroke;
        public com.oddlabs.tt.mapeditor.ResourceLayer.Stroke pruned;
        public float resource_timer;
        public bool resources_edited;
        public bool resources_modified;
        public com.oddlabs.tt.mapeditor.StartLayer starts;
        public bool starts_modified;
        public bool two_d;
        public bool top_down_dirty;
        public float top_down_timer;
        public com.oddlabs.tt.mapeditor.MapEditorDelegate.TopDownView top_down;
        public const int TOP_DOWN_MAX_PIXELS = 1024;
        public const float TOP_DOWN_REFRESH = 0.2f;
        public int start_player;
        public int start_team;
        public const int MAX_PLAYERS = 80;
        public const float START_MARKER_RADIUS = 6.0f;
        public const int SMALL_PULLDOWN_WIDTH = 120;
        public com.oddlabs.tt.mapeditor.BrushRenderer brush_renderer;
        public com.oddlabs.tt.animation.Animated ticker;
        public com.oddlabs.tt.render.LandscapeLocation location;
        public java.util.Random random;
        public com.oddlabs.tt.mapeditor.MapEditorDelegate.EditorCamera game_camera;
        public com.oddlabs.tt.mapeditor.MapEditorDelegate.Toolbar toolbar;
        public com.oddlabs.tt.gui.Label label_radius;
        public com.oddlabs.tt.gui.Label label_intensity;
        public com.oddlabs.tt.gui.Label label_hint;
        public string map_name;
        public bool edited;
        public com.oddlabs.tt.mapeditor.Brush brush;
        public float radius;
        public int intensity;
        public bool has_cursor;
        public float cursor_x;
        public float cursor_y;
        public int stroke_sign;
        public float stroke_z;
        public int random_seed;
        public float ramp_x;
        public float ramp_y;
        public bool map_mode;
        public com.oddlabs.tt.mapeditor.MapEditorDelegate.LookDelegate look;
        // Java: 371
        public static com.oddlabs.tt.gui.Group tool(com.oddlabs.tt.gui.RadioButton radio, com.oddlabs.tt.gui.PulldownButton pulldown)
        {
            com.oddlabs.tt.gui.Group group = new com.oddlabs.tt.gui.Group();
            group.addChild(radio);
            group.addChild(pulldown);
            radio.place();
            pulldown.place(radio, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            group.compileCanvas();
            return group;
        }
        // Java: 385
        public class EditorCamera : com.oddlabs.tt.camera.GameCamera
        {
            public const float EDITOR_MAX_Z = 3000.0f;
            // Java: 392
            public override float getMaxZ()
            {
                return java.lang.Math.max(com.oddlabs.tt.mapeditor.MapEditorDelegate.EditorCamera.EDITOR_MAX_Z, base.getMaxZ());
            }
            // Java: 397
            public float maxZ()
            {
                return getMaxZ();
            }
            public EditorCamera(com.oddlabs.tt.camera.CameraHost host, com.oddlabs.tt.camera.CameraState state) : base(host, state)
            {
            }
        }
        // Java: 406
        public class TerrainStep : java.lang.Object
        {
            public com.oddlabs.tt.mapeditor.ResourceLayer.Stroke pruned() { return pruned_f; }
            public TerrainStep(com.oddlabs.tt.mapeditor.ResourceLayer.Stroke pruned)
            {
                this.pruned_f = pruned;
            }
            public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is TerrainStep r)) return false; return true && java.lang.JO.equals(pruned_f, r.pruned_f); }
            public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(pruned_f); } return h; }
            public override string toString() { return "TerrainStep[" + "pruned=" + java.lang.JO.toString(pruned_f) + "]"; }
            public com.oddlabs.tt.mapeditor.ResourceLayer.Stroke pruned_f;
        }
        // Java: 410
        public class Toolbar : com.oddlabs.tt.gui.Form
        {
            public com.oddlabs.tt.mapeditor.MapEditorDelegate this_0;
            // Java: 411
            public override void cancel()
            {
                this_0.openMenu();
            }
            public Toolbar(com.oddlabs.tt.mapeditor.MapEditorDelegate this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        // Java: 419
        public com.oddlabs.tt.landscape.World getWorld()
        {
            return world;
        }
        // Java: 424
        public com.oddlabs.tt.render.Picker getPicker()
        {
            return picker;
        }
        // Java: 430
        public float getMaxCameraZ()
        {
            return game_camera.maxZ();
        }
        // Java: 437
        public override void doAdd()
        {
            base.doAdd();
            com.oddlabs.tt.@event.LocalEventQueue.getQueue().getHighPrecisionManager().registerAnimation(ticker);
        }
        // Java: 443
        public override void doRemove()
        {
            base.doRemove();
            com.oddlabs.tt.@event.LocalEventQueue.getQueue().getHighPrecisionManager().removeAnimation(ticker);
            cancelStroke();
        }
        // Java: 450
        public override void displayChangedNotify(int width, int height)
        {
            base.displayChangedNotify(width, height);
            toolbar.setPos((width - toolbar.getWidth()) / 2, height - toolbar.getHeight());
            placeAccessLegend();
        }
        // Java: 458
        public void placeAccessLegend()
        {
            label_access_legend.setPos((getWidth() - label_access_legend.getWidth()) / 2, toolbar.getY() - label_access_legend.getHeight() - 4);
        }
        // Java: 463
        public override bool canScroll()
        {
            com.oddlabs.tt.gui.LocalInput input = com.oddlabs.tt.render.Renderer.getLocalInput();
            float scale = getGUIRoot().getGlobalScale();
            game_camera.mouseMoved(java.lang.Math.round((float)(input.getMouseX() / scale)), java.lang.Math.round((float)(input.getMouseY() / scale)));
            return !map_mode && getGUIRoot().getModalDelegate() == null;
        }
        // Java: 473
        public void tick(float t)
        {
            world.tick(t);
            manager.runAnimations(t);
            if (two_d)
            {
                if (top_down.getWidth() != getWidth() || top_down.getHeight() != getHeight())
                {
                    top_down.setDim(getWidth(), getHeight());
                }
                has_cursor = this.getGUIRoot().getModalDelegate() == null && top_down.updateCursor();
                top_down_timer += t;
                if ((top_down_dirty || stroke_sign != 0) && top_down_timer >= com.oddlabs.tt.mapeditor.MapEditorDelegate.TOP_DOWN_REFRESH)
                {
                    top_down.refresh();
                }
            }
            else
            {
                has_cursor = !map_mode && pickCursor();
            }
            if (stroke_sign != 0 && has_cursor && !brush.isDragShape())
            {
                paint(t);
            }
            editor.flush();
            updateGround(t);
            updateAccess(t);
        }
        // Java: 499
        public bool pickCursor()
        {
            if (!picker.pickLocation(game_camera.getState(), location))
            {
                return false;
            }
            cursor_x = location.x;
            cursor_y = location.y;
            if (stroke_sign == 0 || brush.isDragShape())
            {
                return true;
            }
            com.oddlabs.tt.camera.CameraState state = game_camera.getState();
            float eye_x = state.getCurrentX();
            float eye_y = state.getCurrentY();
            float eye_z = state.getCurrentZ();
            float dz = (float)(editor.getHeight(com.oddlabs.tt.mapeditor.MapEditorDelegate.toGrid(location.x), com.oddlabs.tt.mapeditor.MapEditorDelegate.toGrid(location.y)) - eye_z);
            float t = (float)(((float)(stroke_z - eye_z)) / dz);
            if (dz < 0.0f && t > 0.0f)
            {
                cursor_x = (float)(eye_x + (float)(((float)(location.x - eye_x)) * t));
                cursor_y = (float)(eye_y + (float)(((float)(location.y - eye_y)) * t));
            }
            return true;
        }
        // Java: 522
        public static float toGrid(float meters)
        {
            return (float)(meters / com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID);
        }
        // Java: 526
        public static com.oddlabs.tt.mapeditor.Brush chosenBrush(com.oddlabs.tt.gui.PulldownMenu menu)
        {
            return ((com.oddlabs.tt.mapeditor.Brush)java.util.Objects.requireNonNull(((com.oddlabs.tt.mapeditor.Brush)menu.getItem(menu.getChosenItemIndex()).getAttachment())));
        }
        // Java: 530
        public void selectBrush(com.oddlabs.tt.mapeditor.Brush new_brush)
        {
            cancelStroke();
            brush = new_brush;
            refreshLabels();
        }
        // Java: 536
        public void refreshLabels()
        {
            label_radius.clear();
            label_radius.append(com.oddlabs.tt.mapeditor.MapEditor.i18n("radius", java.lang.Math.round(radius)));
            label_intensity.clear();
            label_intensity.append(com.oddlabs.tt.mapeditor.MapEditor.i18n("intensity", intensity));
            label_hint.clear();
            label_hint.append(brush.getHint());
        }
        // Java: 545
        public void beginStroke(int sign)
        {
            if (stroke_sign != 0 || !has_cursor)
            {
                return;
            }
            if (brush.isPlacementBrush())
            {
                float gx = com.oddlabs.tt.mapeditor.MapEditorDelegate.toGrid(cursor_x);
                float gy = com.oddlabs.tt.mapeditor.MapEditorDelegate.toGrid(cursor_y);
                if (sign > 0)
                {
                    starts.place(start_player, java.lang.Math.round(gx), java.lang.Math.round(gy), start_team);
                }
                else if (!starts.removeNearest(gx, gy, java.lang.Math.max(4.0f, com.oddlabs.tt.mapeditor.MapEditorDelegate.toGrid(radius))))
                {
                    getGUIRoot().getInfoPrinter().print(com.oddlabs.tt.mapeditor.MapEditor.i18n("no_start_here"));
                    return;
                }
                starts_modified = true;
                return;
            }
            stroke_sign = sign;
            if (brush.isDragShape())
            {
                ramp_x = cursor_x;
                ramp_y = cursor_y;
                return;
            }
            stroke_z = editor.getHeight(com.oddlabs.tt.mapeditor.MapEditorDelegate.toGrid(cursor_x), com.oddlabs.tt.mapeditor.MapEditorDelegate.toGrid(cursor_y));
            if (brush.isResourceBrush())
            {
                resource_stroke = new com.oddlabs.tt.mapeditor.ResourceLayer.Stroke();
                resource_timer = com.oddlabs.tt.mapeditor.MapEditorDelegate.RESOURCE_DAB_INTERVAL;
                return;
            }
            random_seed = random.nextInt();
            beginTerrainStroke();
        }
        // Java: 577
        public void remember(object step)
        {
            future.clear();
            editor.clearRedo();
            history.push(step);
            while (history.size() > com.oddlabs.tt.mapeditor.TerrainEditor.MAX_UNDO_STEPS)
            {
                history.removeLast();
            }
        }
        // Java: 585
        public void beginTerrainStroke()
        {
            pruned = new com.oddlabs.tt.mapeditor.ResourceLayer.Stroke();
            editor.beginStroke();
        }
        // Java: 591
        public void finishTerrainStroke()
        {
            com.oddlabs.tt.mapeditor.ResourceLayer.Stroke stroke = pruned;
            if (editor.endStroke() && stroke != null)
            {
                remember(new com.oddlabs.tt.mapeditor.MapEditorDelegate.TerrainStep(stroke));
            }
            else
            {
                pruned = null;
            }
        }
        // Java: 599
        public void finishResourceStroke()
        {
            com.oddlabs.tt.mapeditor.ResourceLayer.Stroke stroke = resource_stroke;
            resource_stroke = null;
            if (stroke != null && !stroke.isEmpty())
            {
                remember(stroke);
                resources_edited = true;
                resources_modified = true;
            }
        }
        // Java: 612
        public void updateGround(float t)
        {
            if (ground == null)
            {
                return;
            }
            ground_timer += t;
            if (stroke_sign == 0 && ground_settle)
            {
                ground_settle = false;
                ground.settle();
            }
            else if (ground.hasChanges() && ground_timer >= com.oddlabs.tt.mapeditor.MapEditorDelegate.GROUND_UPDATE_INTERVAL)
            {
                ground_timer = 0.0f;
                ground.update();
            }
        }
        // Java: 629
        public void updateAccess(float t)
        {
            access_timer += t;
            if (access_map.isStale() && (stroke_sign == 0 || access_timer >= com.oddlabs.tt.mapeditor.MapEditorDelegate.ACCESS_UPDATE_INTERVAL))
            {
                access_timer = 0.0f;
                access_map.compute();
                access.mapChanged();
                if (layer.prune((pruned != null ? pruned : new com.oddlabs.tt.mapeditor.ResourceLayer.Stroke())))
                {
                    resources_edited = true;
                    resources_modified = true;
                    ground_settle = true;
                    top_down_dirty = true;
                }
            }
            access.update(com.oddlabs.tt.render.Renderer.getRenderer().getRenderContext());
        }
        // Java: 646
        public void endStroke()
        {
            if (stroke_sign == 0)
            {
                return;
            }
            top_down_dirty = true;
            ground_settle = true;
            if (brush.isResourceBrush())
            {
                finishResourceStroke();
            }
            else if (brush.isDragShape())
            {
                if (has_cursor)
                {
                    float ax = com.oddlabs.tt.mapeditor.MapEditorDelegate.toGrid(ramp_x);
                    float ay = com.oddlabs.tt.mapeditor.MapEditorDelegate.toGrid(ramp_y);
                    float bx = com.oddlabs.tt.mapeditor.MapEditorDelegate.toGrid(cursor_x);
                    float by = com.oddlabs.tt.mapeditor.MapEditorDelegate.toGrid(cursor_y);
                    beginTerrainStroke();
                    editor.applyRamp(ax, ay, editor.getHeight(ax, ay), bx, by, editor.getHeight(bx, by), com.oddlabs.tt.mapeditor.MapEditorDelegate.toGrid(radius), (float)(intensity / 100.0f), stroke_sign);
                    finishTerrainStroke();
                }
            }
            else
            {
                finishTerrainStroke();
            }
            stroke_sign = 0;
        }
        // Java: 671
        public void cancelStroke()
        {
            if (stroke_sign != 0 && brush.isResourceBrush())
            {
                finishResourceStroke();
                ground_settle = true;
            }
            else if (stroke_sign != 0 && !brush.isDragShape())
            {
                finishTerrainStroke();
                ground_settle = true;
            }
            stroke_sign = 0;
        }
        // Java: 682
        public void paint(float t)
        {
            if (brush.isResourceBrush())
            {
                com.oddlabs.tt.mapeditor.Resource resource = brush.getResource();
                resource_timer += t;
                if (resource_timer < com.oddlabs.tt.mapeditor.MapEditorDelegate.RESOURCE_DAB_INTERVAL || resource_stroke == null)
                {
                    return;
                }
                resource_timer = 0.0f;
                if (resource != null && stroke_sign > 0)
                {
                    layer.paint(resource, cursor_x, cursor_y, radius, (float)(intensity / 100.0f), resource_stroke);
                }
                else
                {
                    layer.erase(resource, cursor_x, cursor_y, radius, resource_stroke);
                }
                return;
            }
            float gx = com.oddlabs.tt.mapeditor.MapEditorDelegate.toGrid(cursor_x);
            float gy = com.oddlabs.tt.mapeditor.MapEditorDelegate.toGrid(cursor_y);
            float r = com.oddlabs.tt.mapeditor.MapEditorDelegate.toGrid(radius);
            float strength = (float)(intensity / 100.0f);
            switch ((brush).ordinal())
            {
                case 0 /*HEIGHT*/:
                {
                    editor.applyHeight(gx, gy, r, strength, stroke_sign, t);
                    break;
                }
                case 1 /*FLATTEN*/:
                {
                    editor.applyFlatten(gx, gy, r, strength, stroke_sign, t, stroke_z);
                    break;
                }
                case 2 /*SMOOTH*/:
                {
                    editor.applySmooth(gx, gy, r, strength, stroke_sign, t);
                    break;
                }
                case 3 /*RANDOM*/:
                {
                    editor.applyRandom(gx, gy, r, strength, stroke_sign, t, random_seed);
                    break;
                }
                case 5 /*CLIFF*/:
                {
                    editor.applyCliff(gx, gy, r, strength, stroke_sign, t);
                    break;
                }
                case 6 /*PLATEAU*/:
                {
                    editor.applyPlateau(gx, gy, r, stroke_z);
                    break;
                }
                default:
                {
                    break;
                }
            }
        }
        // Java: 713
        public void undo()
        {
            cancelStroke();
            top_down_dirty = true;
            ground_settle = true;
            object step = history.poll();
            pruned = null;
            if ((step is com.oddlabs.tt.mapeditor.ResourceLayer.Stroke stroke))
            {
                layer.undo(stroke);
                resources_modified = true;
                future.push(step);
            }
            else if ((step is com.oddlabs.tt.mapeditor.MapEditorDelegate.TerrainStep terrain) && editor.undo())
            {
                future.push(step);
                if (!terrain.pruned().isEmpty())
                {
                    layer.undo(terrain.pruned());
                    resources_modified = true;
                }
            }
            else
            {
                getGUIRoot().getInfoPrinter().print(com.oddlabs.tt.mapeditor.MapEditor.i18n("nothing_to_undo"));
            }
        }
        // Java: 736
        public void redo()
        {
            cancelStroke();
            top_down_dirty = true;
            ground_settle = true;
            object step = future.poll();
            pruned = null;
            if ((step is com.oddlabs.tt.mapeditor.ResourceLayer.Stroke stroke))
            {
                layer.redo(stroke);
                resources_modified = true;
                history.push(step);
            }
            else if ((step is com.oddlabs.tt.mapeditor.MapEditorDelegate.TerrainStep terrain) && editor.redo())
            {
                if (!terrain.pruned().isEmpty())
                {
                    layer.redo(terrain.pruned());
                    resources_modified = true;
                }
                history.push(step);
            }
            else
            {
                getGUIRoot().getInfoPrinter().print(com.oddlabs.tt.mapeditor.MapEditor.i18n("nothing_to_redo"));
            }
        }
        // Java: 759
        public bool isOverToolbar(int x, int y)
        {
            return x >= toolbar.getX() && x < toolbar.getX() + toolbar.getWidth() && y >= toolbar.getY() && y < toolbar.getY() + toolbar.getHeight();
        }
        // Java: 764
        public override void mousePressed(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
            if (map_mode || isOverToolbar(x, y))
            {
                return;
            }
            switch ((button).ordinal())
            {
                case 0 /*LEFT*/:
                {
                    beginStroke(1);
                    break;
                }
                case 1 /*RIGHT*/:
                {
                    beginStroke((-1));
                    break;
                }
                case 2 /*MIDDLE*/:
                {
                    cancelStroke();
                    look = new com.oddlabs.tt.mapeditor.MapEditorDelegate.LookDelegate(this);
                    getGUIRoot().pushDelegate(look);
                    break;
                }
            }
        }
        // Java: 779
        public override void mouseReleased(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
            if (map_mode)
            {
                if (((object)button == (object)com.oddlabs.tt.gui.MouseButton.LEFT) && (getCamera() is com.oddlabs.tt.camera.MapCamera map_camera))
                {
                    picker.pickMapGoto(x, y, map_camera);
                }
                return;
            }
            if (((object)button == (object)com.oddlabs.tt.gui.MouseButton.MIDDLE) && look != null)
            {
                look.pop();
                look = null;
            }
            if ((((object)button == (object)com.oddlabs.tt.gui.MouseButton.LEFT) && stroke_sign > 0) || (((object)button == (object)com.oddlabs.tt.gui.MouseButton.RIGHT) && stroke_sign < 0))
            {
                endStroke();
            }
        }
        // Java: 795
        public override void mouseMoved(int x, int y)
        {
            game_camera.mouseMoved(x, y);
        }
        // Java: 800
        public override void mouseDragged(com.oddlabs.tt.gui.MouseButton button, int x, int y, int relative_x, int relative_y, int absolute_x, int absolute_y)
        {
            if (((object)button == (object)com.oddlabs.tt.gui.MouseButton.MIDDLE) && look != null)
            {
                ((com.oddlabs.tt.camera.FirstPersonCamera)look.getCamera()).mouseMoved(x, y);
                return;
            }
            game_camera.mouseMoved(x, y);
        }
        // Java: 810
        public override void mouseScrolled(int amount)
        {
            if (map_mode)
            {
                return;
            }
            com.oddlabs.tt.gui.LocalInput input = com.oddlabs.tt.render.Renderer.getLocalInput();
            int steps = java.lang.Integer.signum(amount);
            if (input.isKeyDown(com.oddlabs.tt.input.Key.LCONTROL) || input.isKeyDown(com.oddlabs.tt.input.Key.RCONTROL))
            {
                radius = java.lang.Math.clamp((steps > 0 ? (float)(radius * com.oddlabs.tt.mapeditor.MapEditorDelegate.RADIUS_STEP) : (float)(radius / com.oddlabs.tt.mapeditor.MapEditorDelegate.RADIUS_STEP)), com.oddlabs.tt.mapeditor.MapEditorDelegate.MIN_RADIUS, com.oddlabs.tt.mapeditor.MapEditorDelegate.MAX_RADIUS);
                refreshLabels();
            }
            else if (input.isKeyDown(com.oddlabs.tt.input.Key.LSHIFT) || input.isKeyDown(com.oddlabs.tt.input.Key.RSHIFT))
            {
                intensity = java.lang.Math.clamp(intensity + steps * com.oddlabs.tt.mapeditor.MapEditorDelegate.INTENSITY_STEP, com.oddlabs.tt.mapeditor.MapEditorDelegate.MIN_INTENSITY, com.oddlabs.tt.mapeditor.MapEditorDelegate.MAX_INTENSITY);
                refreshLabels();
            }
            else
            {
                game_camera.mouseScrolled(amount);
            }
        }
        // Java: 828
        public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
            @event.consumeAction(com.oddlabs.tt.input.GameAction.UI_ACTIVATE);
            if (@event.hasAction(com.oddlabs.tt.input.GameAction.CAMERA_MAP_MODE))
            {
                if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.PRESSED) && !map_mode)
                {
                    @event.consumeAction(com.oddlabs.tt.input.GameAction.CAMERA_MAP_MODE);
                    if (!two_d)
                    {
                        enterMapMode();
                    }
                    @event.consume();
                    return;
                }
                if (((object)@event.getPhase() != (object)com.oddlabs.tt.input.InputPhase.PRESSED))
                {
                    @event.consumeAction(com.oddlabs.tt.input.GameAction.CAMERA_MAP_MODE);
                    @event.consume();
                    return;
                }
            }
            if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.PRESSED) && !map_mode)
            {
                if (@event.isControlDown() && (((object)@event.getKeyCode() == (object)com.oddlabs.tt.input.Key.Y) || (@event.isShiftDown() && ((object)@event.getKeyCode() == (object)com.oddlabs.tt.input.Key.Z))))
                {
                    redo();
                    @event.consume();
                    return;
                }
                if (@event.isControlDown() && ((object)@event.getKeyCode() == (object)com.oddlabs.tt.input.Key.Z))
                {
                    undo();
                    @event.consume();
                    return;
                }
                if (@event.consumeAction(com.oddlabs.tt.input.GameAction.GLOBAL_MENU) || @event.consumeAction(com.oddlabs.tt.input.GameAction.UI_CANCEL))
                {
                    openMenu();
                    @event.consume();
                    return;
                }
            }
            base.handleInput(@event);
        }
        // Java: 871
        public override void render3D(com.oddlabs.tt.render.LandscapeRenderer renderer, com.oddlabs.tt.render.RenderQueues render_queues, com.oddlabs.tt.camera.CameraState state, com.oddlabs.tt.render.MatrixStack model_view, com.oddlabs.tt.render.MatrixStack projection)
        {
            access.render(com.oddlabs.tt.render.Renderer.getRenderer().getRenderContext(), renderer, state);
            if (map_mode || getGUIRoot().getModalDelegate() != null)
            {
                return;
            }
            if (state.getCurrentZ() < world.getHeightMap().getSeaLevelMeters())
            {
                return;
            }
            {
                com.oddlabs.tt.mapeditor.BrushRenderer.Batch batch = brush_renderer.begin(renderer, model_view, projection);
                try
                {
                    drawStarts(batch);
                    if (!has_cursor)
                    {
                        return;
                    }
                    float r = (stroke_sign < 0 ? 1.0f : 0.4f);
                    float g = (stroke_sign < 0 ? 0.4f : 1.0f);
                    float b = (stroke_sign == 0 ? 1.0f : 0.4f);
                    if (brush.isPlacementBrush())
                    {
                        batch.circle(cursor_x, cursor_y, com.oddlabs.tt.mapeditor.MapEditorDelegate.START_MARKER_RADIUS, r, g, b, 0.9f);
                    }
                    else if (brush.isResourceBrush() || brush.isDragShape())
                    {
                        batch.circle(cursor_x, cursor_y, radius, r, g, b, 0.9f);
                    }
                    else
                    {
                        outline(batch, cursor_x, cursor_y, radius, r, g, b);
                    }
                    batch.dot(cursor_x, cursor_y, r, g, b, 0.9f);
                    if (brush.isDragShape() && stroke_sign != 0)
                    {
                        batch.circle(ramp_x, ramp_y, radius, r, g, b, 0.9f);
                        batch.line(ramp_x, ramp_y, cursor_x, cursor_y, r, g, b, 0.9f);
                    }
                }
                finally
                {
                    if (batch != null) batch.close();
                }
            }
        }
        // Java: 902
        public void outline(com.oddlabs.tt.mapeditor.BrushRenderer.Batch batch, float cx, float cy, float radius, float r, float g, float b)
        {
            switch ((editor.getShape()).ordinal())
            {
                case 0 /*CIRCLE*/:
                {
                    batch.circle(cx, cy, radius, r, g, b, 0.9f);
                    break;
                }
                case 1 /*SQUARE*/:
                {
                    batch.line((float)(cx - radius), (float)(cy - radius), (float)(cx + radius), (float)(cy - radius), r, g, b, 0.9f);
                    batch.line((float)(cx + radius), (float)(cy - radius), (float)(cx + radius), (float)(cy + radius), r, g, b, 0.9f);
                    batch.line((float)(cx + radius), (float)(cy + radius), (float)(cx - radius), (float)(cy + radius), r, g, b, 0.9f);
                    batch.line((float)(cx - radius), (float)(cy + radius), (float)(cx - radius), (float)(cy - radius), r, g, b, 0.9f);
                    break;
                }
                case 2 /*DIAMOND*/:
                {
                    batch.line(cx, (float)(cy - radius), (float)(cx + radius), cy, r, g, b, 0.9f);
                    batch.line((float)(cx + radius), cy, cx, (float)(cy + radius), r, g, b, 0.9f);
                    batch.line(cx, (float)(cy + radius), (float)(cx - radius), cy, r, g, b, 0.9f);
                    batch.line((float)(cx - radius), cy, cx, (float)(cy - radius), r, g, b, 0.9f);
                    break;
                }
            }
        }
        // Java: 922
        public void drawStarts(com.oddlabs.tt.mapeditor.BrushRenderer.Batch batch)
        {
            org.joml.Vector4f[] colours = com.oddlabs.tt.global.Settings.getSettings().team_colours;
            foreach (com.oddlabs.tt.mapeditor.StartLayer.Start start in java.lang.JIter.of(starts.all()))
            {
                org.joml.Vector4f c = colours[start.player() % colours.Length];
                float x = start.x() * com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID;
                float y = start.y() * com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID;
                batch.circle(x, y, com.oddlabs.tt.mapeditor.MapEditorDelegate.START_MARKER_RADIUS, c.x_f, c.y_f, c.z_f, 1.0f);
                batch.circle(x, y, (float)(com.oddlabs.tt.mapeditor.MapEditorDelegate.START_MARKER_RADIUS * 0.5f), c.x_f, c.y_f, c.z_f, 1.0f);
                batch.dot(x, y, c.x_f, c.y_f, c.z_f, 1.0f);
            }
        }
        // Java: 938
        public void setTwoD(bool on)
        {
            if (on == two_d)
            {
                return;
            }
            cancelStroke();
            two_d = on;
            has_cursor = false;
            if (on)
            {
                toolbar.remove();
                top_down.setDim(getWidth(), getHeight());
                addChild(top_down);
                addChild(toolbar);
                top_down.refresh();
            }
            else
            {
                top_down.remove();
            }
            setFocus();
        }
        // Java: 962
        public class TopDownView : com.oddlabs.tt.gui.GUIObject
        {
            public com.oddlabs.tt.mapeditor.MapEditorDelegate this_0;
            public const float MARGIN = 10.0f;
            public org.joml.Vector4fc background;
            public com.oddlabs.tt.mapeditor.MapPreview image;
            public com.oddlabs.tt.render.Texture texture;
            public int mouse_x;
            public int mouse_y;
            public bool mouse_inside;
            // Java: 972
            public void refresh()
            {
                java.util.List[] positions = new java.util.List[com.oddlabs.tt.mapeditor.Resource.values().Length];
                foreach (com.oddlabs.tt.mapeditor.Resource kind in com.oddlabs.tt.mapeditor.Resource.values())
                {
                    positions[kind.ordinal()] = this_0.layer.positions(kind);
                }
                image = com.oddlabs.tt.mapeditor.MapPreview.render(this_0.editor.peekHeights(), this_0.settings, new com.oddlabs.tt.mapeditor.MapFile.Resources(positions), com.oddlabs.tt.mapeditor.MapEditorDelegate.TOP_DOWN_MAX_PIXELS);
                closeTexture();
                if (getParentGUIRoot() != null)
                {
                    upload();
                }
                this_0.top_down_dirty = false;
                this_0.top_down_timer = 0.0f;
            }
            // Java: 987
            public float side()
            {
                return java.lang.Math.max(1.0f, (float)(java.lang.Math.min(getWidth(), this_0.toolbar.getY()) - (float)(2 * com.oddlabs.tt.mapeditor.MapEditorDelegate.TopDownView.MARGIN)));
            }
            // Java: 991
            public float left()
            {
                return (float)(((float)(getWidth() - side())) / 2.0f);
            }
            // Java: 995
            public float bottom()
            {
                return (float)(((float)(this_0.toolbar.getY() - side())) / 2.0f);
            }
            // Java: 1000
            public bool updateCursor()
            {
                float side_v = side();
                float fx = (float)(((float)(mouse_x - left())) / side_v);
                float fy = (float)(((float)(mouse_y - bottom())) / side_v);
                if (!mouse_inside || fx < 0.0f || fy < 0.0f || fx >= 1.0f || fy >= 1.0f)
                {
                    return false;
                }
                float meters = this_0.world.getHeightMap().getMetersPerWorld();
                this_0.cursor_x = (float)(fx * meters);
                this_0.cursor_y = (float)(fy * meters);
                return true;
            }
            // Java: 1012
            public override void mouseMoved(int x, int y)
            {
                mouse_x = x;
                mouse_y = y;
                mouse_inside = true;
            }
            // Java: 1019
            public override void mouseDragged(com.oddlabs.tt.gui.MouseButton button, int x, int y, int relative_x, int relative_y, int absolute_x, int absolute_y)
            {
                mouseMoved(x, y);
            }
            // Java: 1025
            public override void mouseExited()
            {
                mouse_inside = false;
            }
            // Java: 1030
            public override void mousePressed(com.oddlabs.tt.gui.MouseButton button, int x, int y)
            {
                mouseMoved(x, y);
                this_0.has_cursor = updateCursor();
                if (((object)button == (object)com.oddlabs.tt.gui.MouseButton.LEFT))
                {
                    this_0.beginStroke(1);
                }
                else if (((object)button == (object)com.oddlabs.tt.gui.MouseButton.RIGHT))
                {
                    this_0.beginStroke((-1));
                }
                this.this_0.setFocus();
            }
            // Java: 1041
            public override void mouseReleased(com.oddlabs.tt.gui.MouseButton button, int x, int y)
            {
                if ((((object)button == (object)com.oddlabs.tt.gui.MouseButton.LEFT) && this_0.stroke_sign > 0) || (((object)button == (object)com.oddlabs.tt.gui.MouseButton.RIGHT) && this_0.stroke_sign < 0))
                {
                    this_0.endStroke();
                }
            }
            // Java: 1047
            public override void mouseScrolled(int amount)
            {
                com.oddlabs.tt.gui.LocalInput input = com.oddlabs.tt.render.Renderer.getLocalInput();
                int steps = java.lang.Integer.signum(amount);
                if (input.isKeyDown(com.oddlabs.tt.input.Key.LCONTROL) || input.isKeyDown(com.oddlabs.tt.input.Key.RCONTROL))
                {
                    this_0.radius = java.lang.Math.clamp((steps > 0 ? (float)(this_0.radius * com.oddlabs.tt.mapeditor.MapEditorDelegate.RADIUS_STEP) : (float)(this_0.radius / com.oddlabs.tt.mapeditor.MapEditorDelegate.RADIUS_STEP)), com.oddlabs.tt.mapeditor.MapEditorDelegate.MIN_RADIUS, com.oddlabs.tt.mapeditor.MapEditorDelegate.MAX_RADIUS);
                    this_0.refreshLabels();
                }
                else if (input.isKeyDown(com.oddlabs.tt.input.Key.LSHIFT) || input.isKeyDown(com.oddlabs.tt.input.Key.RSHIFT))
                {
                    this_0.intensity = java.lang.Math.clamp(this_0.intensity + steps * com.oddlabs.tt.mapeditor.MapEditorDelegate.INTENSITY_STEP, com.oddlabs.tt.mapeditor.MapEditorDelegate.MIN_INTENSITY, com.oddlabs.tt.mapeditor.MapEditorDelegate.MAX_INTENSITY);
                    this_0.refreshLabels();
                }
            }
            // Java: 1061
            public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
            {
                renderer.drawColoredQuad(0, 0, getWidth(), getHeight(), background);
                float side_v = side();
                float x0 = left();
                float y0 = bottom();
                if (texture != null)
                {
                    renderer.drawTexture(texture, x0, y0, side_v, side_v, 0.0f, 0.0f, 1.0f, 1.0f, com.oddlabs.util.Color.WHITE);
                }
                float pixels_per_meter = (float)(side_v / this_0.world.getHeightMap().getMetersPerWorld());
                org.joml.Vector4f[] colours = com.oddlabs.tt.global.Settings.getSettings().team_colours;
                foreach (com.oddlabs.tt.mapeditor.StartLayer.Start start in java.lang.JIter.of(this_0.starts.all()))
                {
                    org.joml.Vector4f c = colours[start.player() % colours.Length];
                    org.joml.Vector4f colour = new org.joml.Vector4f(c.x_f, c.y_f, c.z_f, 1.0f);
                    float sx = (float)(x0 + (float)(start.x() * com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID * pixels_per_meter));
                    float sy = (float)(y0 + (float)(start.y() * com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID * pixels_per_meter));
                    com.oddlabs.tt.mapeditor.MapEditorDelegate.TopDownView.ring(renderer, sx, sy, 8.0f, colour);
                    com.oddlabs.tt.mapeditor.MapEditorDelegate.TopDownView.ring(renderer, sx, sy, 4.0f, colour);
                    com.oddlabs.tt.mapeditor.MapEditorDelegate.TopDownView.dot(renderer, sx, sy, 3.0f, colour);
                }
                if (!this_0.has_cursor)
                {
                    return;
                }
                org.joml.Vector4f colour_2 = new org.joml.Vector4f((this_0.stroke_sign < 0 ? 1.0f : 0.4f), (this_0.stroke_sign < 0 ? 0.4f : 1.0f), (this_0.stroke_sign == 0 ? 1.0f : 0.4f), 0.9f);
                float cx = (float)(x0 + (float)(this_0.cursor_x * pixels_per_meter));
                float cy = (float)(y0 + (float)(this_0.cursor_y * pixels_per_meter));
                float r = (float)(this_0.radius * pixels_per_meter);
                if (this_0.brush.isPlacementBrush())
                {
                    com.oddlabs.tt.mapeditor.MapEditorDelegate.TopDownView.ring(renderer, cx, cy, 8.0f, colour_2);
                }
                else if (this_0.brush.isResourceBrush() || this_0.brush.isDragShape() || ((object)this_0.editor.getShape() == (object)com.oddlabs.tt.mapeditor.TerrainEditor.Shape.CIRCLE))
                {
                    com.oddlabs.tt.mapeditor.MapEditorDelegate.TopDownView.ring(renderer, cx, cy, r, colour_2);
                }
                else if (((object)this_0.editor.getShape() == (object)com.oddlabs.tt.mapeditor.TerrainEditor.Shape.SQUARE))
                {
                    com.oddlabs.tt.mapeditor.MapEditorDelegate.TopDownView.line(renderer, (float)(cx - r), (float)(cy - r), (float)(cx + r), (float)(cy - r), colour_2);
                    com.oddlabs.tt.mapeditor.MapEditorDelegate.TopDownView.line(renderer, (float)(cx + r), (float)(cy - r), (float)(cx + r), (float)(cy + r), colour_2);
                    com.oddlabs.tt.mapeditor.MapEditorDelegate.TopDownView.line(renderer, (float)(cx + r), (float)(cy + r), (float)(cx - r), (float)(cy + r), colour_2);
                    com.oddlabs.tt.mapeditor.MapEditorDelegate.TopDownView.line(renderer, (float)(cx - r), (float)(cy + r), (float)(cx - r), (float)(cy - r), colour_2);
                }
                else
                {
                    com.oddlabs.tt.mapeditor.MapEditorDelegate.TopDownView.line(renderer, cx, (float)(cy - r), (float)(cx + r), cy, colour_2);
                    com.oddlabs.tt.mapeditor.MapEditorDelegate.TopDownView.line(renderer, (float)(cx + r), cy, cx, (float)(cy + r), colour_2);
                    com.oddlabs.tt.mapeditor.MapEditorDelegate.TopDownView.line(renderer, cx, (float)(cy + r), (float)(cx - r), cy, colour_2);
                    com.oddlabs.tt.mapeditor.MapEditorDelegate.TopDownView.line(renderer, (float)(cx - r), cy, cx, (float)(cy - r), colour_2);
                }
                com.oddlabs.tt.mapeditor.MapEditorDelegate.TopDownView.dot(renderer, cx, cy, 3.0f, colour_2);
                if (this_0.brush.isDragShape() && this_0.stroke_sign != 0)
                {
                    float rx = (float)(x0 + (float)(this_0.ramp_x * pixels_per_meter));
                    float ry = (float)(y0 + (float)(this_0.ramp_y * pixels_per_meter));
                    com.oddlabs.tt.mapeditor.MapEditorDelegate.TopDownView.ring(renderer, rx, ry, r, colour_2);
                    com.oddlabs.tt.mapeditor.MapEditorDelegate.TopDownView.line(renderer, rx, ry, cx, cy, colour_2);
                }
            }
            // Java: 1111
            public static void dot(com.oddlabs.tt.render.GUIRenderer renderer, float x, float y, float size, org.joml.Vector4f colour)
            {
                renderer.drawColoredQuad((float)(x - (float)(size / 2.0f)), (float)(y - (float)(size / 2.0f)), size, size, colour);
            }
            // Java: 1115
            public static void ring(com.oddlabs.tt.render.GUIRenderer renderer, float cx, float cy, float r, org.joml.Vector4f colour)
            {
                int count = java.lang.Math.clamp(java.lang.JC.d2i((2 * java.lang.Math.PI * r / 4.0f)), 12, 360);
                for (int i = 0; i < count; i++)
                {
                    double angle = 2 * java.lang.Math.PI * i / count;
                    com.oddlabs.tt.mapeditor.MapEditorDelegate.TopDownView.dot(renderer, (float)(cx + (float)(r * ((float)(java.lang.Math.cos(angle))))), (float)(cy + (float)(r * ((float)(java.lang.Math.sin(angle))))), 2.0f, colour);
                }
            }
            // Java: 1123
            public static void line(com.oddlabs.tt.render.GUIRenderer renderer, float x0, float y0, float x1, float y1, org.joml.Vector4f colour)
            {
                float length = ((float)(java.lang.Math.hypot((float)(x1 - x0), (float)(y1 - y0))));
                int count = java.lang.Math.clamp(java.lang.JC.f2i(((float)(length / 4.0f))), 1, 720);
                for (int i = 0; i <= count; i++)
                {
                    float t = (float)(i / ((float)(count)));
                    com.oddlabs.tt.mapeditor.MapEditorDelegate.TopDownView.dot(renderer, (float)(x0 + (float)(((float)(x1 - x0)) * t)), (float)(y0 + (float)(((float)(y1 - y0)) * t)), 2.0f, colour);
                }
            }
            // Java: 1134
            public void upload()
            {
                com.oddlabs.tt.mapeditor.MapPreview shown = image;
                if (shown == null || texture != null)
                {
                    return;
                }
                int size = shown.size();
                texture = com.oddlabs.tt.util.PortHost.textureFromRGB(size, shown.rgb());
            }
            // Java: 1142
            public void closeTexture()
            {
                if (texture != null)
                {
                    texture.close();
                    texture = null;
                }
            }
            // Java: 1149
            public override void doAdd()
            {
                base.doAdd();
                upload();
            }
            // Java: 1155
            public override void doRemove()
            {
                base.doRemove();
                closeTexture();
            }
            private void init_TopDownView()
            {
                this.background = new org.joml.Vector4f(0.05f, 0.05f, 0.08f, 1.0f);
            }
            public TopDownView(com.oddlabs.tt.mapeditor.MapEditorDelegate this_0_) : base()
            {
                this.this_0 = this_0_;
                init_TopDownView();
            }
        }
        // Java: 1164
        public void enterMapMode()
        {
            cancelStroke();
            map_mode = true;
            has_cursor = false;
            toolbar.remove();
            setFocus();
            game_camera.disable();
            setCamera(new com.oddlabs.tt.camera.MapCamera(this, game_camera));
            getCamera().enable();
        }
        // Java: 1175
        public void exitMapMode()
        {
            map_mode = false;
            getCamera().disable();
            game_camera.getState().snapToTarget();
            setCamera(game_camera);
            game_camera.enable();
            addChild(toolbar);
        }
        // Java: 1186
        public void openMenu()
        {
            cancelStroke();
            setFocus();
            if (getGUIRoot().getModalDelegate() == null)
            {
                getGUIRoot().addModalForm(new com.oddlabs.tt.mapeditor.EditorMenu(new java.lang.Runnable_L(() => { this.save(); }), new java.lang.Runnable_L(() => { this.exit(); })));
            }
        }
        // Java: 1194
        public void save()
        {
            java.nio.file.Path dir = com.oddlabs.tt.mapeditor.MapEditor.getMapsDir();
            if (dir == null)
            {
                getGUIRoot().addModalForm(new com.oddlabs.tt.form.MessageForm(com.oddlabs.tt.mapeditor.MapEditor.i18n("no_maps_dir")));
                return;
            }
            getGUIRoot().addModalForm(new com.oddlabs.tt.mapeditor.SaveMapDialog(getGUIRoot(), dir, (map_name != null ? map_name : ""), new java.util.function.Consumer_L((object name_l) =>
            {
                string name = (string)name_l;
                bool keep_heights = edited || editor.isModified();
                java.util.List[] positions = new java.util.List[com.oddlabs.tt.mapeditor.Resource.values().Length];
                foreach (com.oddlabs.tt.mapeditor.Resource kind in com.oddlabs.tt.mapeditor.Resource.values())
                {
                    positions[kind.ordinal()] = layer.positions(kind);
                }
                com.oddlabs.tt.mapeditor.MapFile.Resources current = new com.oddlabs.tt.mapeditor.MapFile.Resources(positions);
                float[][] heights = editor.copyHeights();
                com.oddlabs.tt.mapeditor.MapPreview preview = com.oddlabs.tt.mapeditor.MapPreview.render(heights, settings, current);
                try
                {
                    new com.oddlabs.tt.mapeditor.MapFile(name, settings, (keep_heights ? heights : (float[][])(null)), (resources_edited ? current : (com.oddlabs.tt.mapeditor.MapFile.Resources)(null)), preview, starts.toSaved()).save(dir);
                }
                catch (System.IO.IOException e)
                {
                    getGUIRoot().addModalForm(new com.oddlabs.tt.form.MessageForm(com.oddlabs.tt.mapeditor.MapEditor.i18n("save_failed", e.getMessage())));
                    return;
                }
                map_name = name;
                edited = keep_heights;
                editor.markSaved();
                resources_modified = false;
                starts_modified = false;
                getGUIRoot().getInfoPrinter().print(com.oddlabs.tt.mapeditor.MapEditor.i18n("saved", name));
            })));
        }
        // Java: 1226
        public void exit()
        {
            if (editor.isModified() || resources_modified || starts_modified)
            {
                getGUIRoot().addModalForm(new com.oddlabs.tt.form.QuestionForm(com.oddlabs.tt.mapeditor.MapEditor.i18n("exit_confirm"), new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
                {
                    leave();
                })));
            }
            else
            {
                leave();
            }
        }
        // Java: 1234
        public void leave()
        {
            if (ground != null)
            {
                ground.close();
            }
            access.close();
            com.oddlabs.tt.render.Renderer.startMenu(network, getGUIRoot().getGUI());
        }
        // Java: 1242
        public class LookDelegate : com.oddlabs.tt.@delegate.CameraDelegate
        {
            // Java: 1248
            public override com.oddlabs.tt.gui.CursorType getCursorType()
            {
                return com.oddlabs.tt.gui.CursorType.NULL;
            }
            public LookDelegate(com.oddlabs.tt.mapeditor.MapEditorDelegate owner) : base(owner.getGUIRoot(), new com.oddlabs.tt.camera.FirstPersonCamera(owner, owner.world.getHeightMap(), owner.game_camera.getState()))
            {
            }
        }
        private void init_MapEditorDelegate()
        {
            this.history = new java.util.ArrayDeque();
            this.future = new java.util.ArrayDeque();
            this.top_down = new com.oddlabs.tt.mapeditor.MapEditorDelegate.TopDownView(this);
            this.start_team = (-1);
            this.brush_renderer = new com.oddlabs.tt.mapeditor.BrushRenderer();
            this.ticker = new com.oddlabs.tt.animation.Animated_L((float a0) => { this.tick(a0); });
            this.location = new com.oddlabs.tt.render.LandscapeLocation();
            this.random = new java.util.Random(com.oddlabs.tt.@event.LocalEventQueue.getQueue().getHighPrecisionManager().getTick());
            this.brush = com.oddlabs.tt.mapeditor.Brush.HEIGHT;
            this.radius = 16.0f;
            this.intensity = 50;
        }
        public MapEditorDelegate(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.landscape.World world, com.oddlabs.tt.animation.AnimationManager manager, com.oddlabs.tt.render.Picker picker, com.oddlabs.tt.viewer.Cheat view, com.oddlabs.tt.camera.CameraState camera_state, com.oddlabs.tt.mapeditor.TerrainEditor editor, com.oddlabs.tt.mapeditor.GroundTextures ground, com.oddlabs.tt.mapeditor.AccessMap access_map, com.oddlabs.tt.mapeditor.AccessOverlay access, com.oddlabs.tt.mapeditor.ResourceLayer layer, com.oddlabs.tt.mapeditor.MapSettings settings, string map_name, bool edited, bool resources_edited, java.util.List saved_starts) : base(gui_root, null)
        {
            init_MapEditorDelegate();
            this.network = network;
            this.world = world;
            this.manager = manager;
            this.picker = picker;
            this.editor = editor;
            this.ground = ground;
            this.access_map = access_map;
            this.access = access;
            this.settings = settings;
            this.map_name = map_name;
            this.edited = edited;
            this.layer = layer;
            this.resources_edited = resources_edited;
            this.starts = new com.oddlabs.tt.mapeditor.StartLayer(editor.getSize(), saved_starts);
            game_camera = new com.oddlabs.tt.mapeditor.MapEditorDelegate.EditorCamera(this, camera_state);
            setCamera(game_camera);
            float center = (float)(world.getHeightMap().getMetersPerWorld() / 2.0f);
            game_camera.reset(center, (float)(center * 0.75f));
            toolbar = new com.oddlabs.tt.mapeditor.MapEditorDelegate.Toolbar(this);
            com.oddlabs.tt.procedural.Landscape.TerrainType terrain = com.oddlabs.tt.procedural.Landscape.TerrainType.values()[settings.terrain()];
            com.oddlabs.tt.gui.PulldownMenu menu_terrain = new com.oddlabs.tt.gui.PulldownMenu();
            com.oddlabs.tt.gui.PulldownMenu menu_resource = new com.oddlabs.tt.gui.PulldownMenu();
            foreach (com.oddlabs.tt.mapeditor.Brush b in com.oddlabs.tt.mapeditor.Brush.values())
            {
                ((b.isResourceBrush() || b.isPlacementBrush() ? menu_resource : menu_terrain)).addItem(new com.oddlabs.tt.gui.PulldownItem(b.getName(terrain), b));
            }
            com.oddlabs.tt.gui.RadioButtonGroup tools = new com.oddlabs.tt.gui.RadioButtonGroup();
            com.oddlabs.tt.gui.RadioButton radio_terrain = new com.oddlabs.tt.gui.RadioButton(true, tools, com.oddlabs.tt.mapeditor.MapEditor.i18n("tool_terrain"));
            com.oddlabs.tt.gui.RadioButton radio_resource = new com.oddlabs.tt.gui.RadioButton(false, tools, com.oddlabs.tt.mapeditor.MapEditor.i18n("tool_resources"));
            com.oddlabs.tt.gui.PulldownButton pulldown_terrain = new com.oddlabs.tt.gui.PulldownButton(gui_root, menu_terrain, 0, com.oddlabs.tt.mapeditor.MapEditorDelegate.PULLDOWN_WIDTH);
            com.oddlabs.tt.gui.PulldownButton pulldown_resource = new com.oddlabs.tt.gui.PulldownButton(gui_root, menu_resource, 0, com.oddlabs.tt.mapeditor.MapEditorDelegate.PULLDOWN_WIDTH);
            radio_terrain.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                selectBrush(com.oddlabs.tt.mapeditor.MapEditorDelegate.chosenBrush(menu_terrain));
                setFocus();
            }));
            radio_resource.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_5, int unused_6, int unused_7, int unused_8) =>
            {
                selectBrush(com.oddlabs.tt.mapeditor.MapEditorDelegate.chosenBrush(menu_resource));
                setFocus();
            }));
            menu_terrain.addItemChosenListener(new com.oddlabs.tt.guievent.ItemChosenListener_L((com.oddlabs.tt.gui.PulldownMenu menu, int unused_9) =>
            {
                tools.mark(radio_terrain);
                selectBrush(com.oddlabs.tt.mapeditor.MapEditorDelegate.chosenBrush(menu));
                setFocus();
            }));
            menu_resource.addItemChosenListener(new com.oddlabs.tt.guievent.ItemChosenListener_L((com.oddlabs.tt.gui.PulldownMenu menu_2, int unused_10) =>
            {
                tools.mark(radio_resource);
                selectBrush(com.oddlabs.tt.mapeditor.MapEditorDelegate.chosenBrush(menu_2));
                setFocus();
            }));
            com.oddlabs.tt.gui.Group group_terrain = com.oddlabs.tt.mapeditor.MapEditorDelegate.tool(radio_terrain, pulldown_terrain);
            com.oddlabs.tt.gui.Group group_resource = com.oddlabs.tt.mapeditor.MapEditorDelegate.tool(radio_resource, pulldown_resource);
            toolbar.addChild(group_terrain);
            toolbar.addChild(group_resource);
            group_terrain.place();
            group_resource.place(group_terrain, com.oddlabs.tt.gui.Placement.RIGHT_MID, 20);
            com.oddlabs.tt.gui.Group row_extra = new com.oddlabs.tt.gui.Group();
            com.oddlabs.tt.gui.PulldownMenu menu_shape = new com.oddlabs.tt.gui.PulldownMenu();
            foreach (com.oddlabs.tt.mapeditor.TerrainEditor.Shape shape in com.oddlabs.tt.mapeditor.TerrainEditor.Shape.values())
            {
                menu_shape.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.mapeditor.MapEditor.i18n("shape_" + java.lang.JStr.valueOf(shape.name().toLowerCase(java.util.Locale.ROOT))), shape));
            }
            menu_shape.addItemChosenListener(new com.oddlabs.tt.guievent.ItemChosenListener_L((com.oddlabs.tt.gui.PulldownMenu menu_3, int index) =>
            {
                editor.setShape(((com.oddlabs.tt.mapeditor.TerrainEditor.Shape)java.util.Objects.requireNonNull(((com.oddlabs.tt.mapeditor.TerrainEditor.Shape)menu_3.getItem(index).getAttachment()))));
                setFocus();
            }));
            com.oddlabs.tt.gui.ScrollablePulldownMenu menu_player = new com.oddlabs.tt.gui.ScrollablePulldownMenu(10);
            for (int i = 1; i <= com.oddlabs.tt.mapeditor.MapEditorDelegate.MAX_PLAYERS; i++)
            {
                menu_player.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.mapeditor.MapEditor.i18n("start_player", i)));
            }
            menu_player.addItemChosenListener(new com.oddlabs.tt.guievent.ItemChosenListener_L((com.oddlabs.tt.gui.PulldownMenu unused_11, int index_2) =>
            {
                start_player = index_2;
                setFocus();
            }));
            com.oddlabs.tt.gui.ScrollablePulldownMenu menu_team = new com.oddlabs.tt.gui.ScrollablePulldownMenu(10);
            menu_team.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.mapeditor.MapEditor.i18n("no_team")));
            for (int t = 1; t <= com.oddlabs.tt.mapeditor.MapEditorDelegate.MAX_PLAYERS; t++)
            {
                menu_team.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.mapeditor.MapEditor.i18n("team_n", t)));
            }
            menu_team.addItemChosenListener(new com.oddlabs.tt.guievent.ItemChosenListener_L((com.oddlabs.tt.gui.PulldownMenu unused_12, int index_3) =>
            {
                start_team = index_3 - 1;
                setFocus();
            }));
            com.oddlabs.tt.gui.ScrollablePulldownMenu menu_count = new com.oddlabs.tt.gui.ScrollablePulldownMenu(10);
            for (int n = 2; n <= com.oddlabs.tt.mapeditor.MapEditorDelegate.MAX_PLAYERS; n++)
            {
                menu_count.addItem(new com.oddlabs.tt.gui.PulldownItem(com.oddlabs.tt.mapeditor.MapEditor.i18n("players_n", n)));
            }
            com.oddlabs.tt.gui.HorizButton button_auto = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.mapeditor.MapEditor.i18n("auto_place"), 150);
            button_auto.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_13, int unused_14, int unused_15, int unused_16) =>
            {
                starts.autoPlace(menu_count.getChosenItemIndex() + 2);
                starts_modified = true;
                setFocus();
            }));
            com.oddlabs.tt.gui.Label label_shape = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.mapeditor.MapEditor.i18n("shape"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            com.oddlabs.tt.gui.Label label_start = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.mapeditor.MapEditor.i18n("start_label"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            com.oddlabs.tt.gui.PulldownButton pulldown_shape = new com.oddlabs.tt.gui.PulldownButton(gui_root, menu_shape, 0, com.oddlabs.tt.mapeditor.MapEditorDelegate.SMALL_PULLDOWN_WIDTH);
            com.oddlabs.tt.gui.PulldownButton pulldown_player = new com.oddlabs.tt.gui.PulldownButton(gui_root, menu_player, 0, com.oddlabs.tt.mapeditor.MapEditorDelegate.SMALL_PULLDOWN_WIDTH);
            com.oddlabs.tt.gui.PulldownButton pulldown_team = new com.oddlabs.tt.gui.PulldownButton(gui_root, menu_team, 0, com.oddlabs.tt.mapeditor.MapEditorDelegate.SMALL_PULLDOWN_WIDTH);
            com.oddlabs.tt.gui.PulldownButton pulldown_count = new com.oddlabs.tt.gui.PulldownButton(gui_root, menu_count, 2, com.oddlabs.tt.mapeditor.MapEditorDelegate.SMALL_PULLDOWN_WIDTH);
            row_extra.addChild(label_shape);
            row_extra.addChild(pulldown_shape);
            row_extra.addChild(label_start);
            row_extra.addChild(pulldown_player);
            row_extra.addChild(pulldown_team);
            row_extra.addChild(pulldown_count);
            row_extra.addChild(button_auto);
            label_shape.place();
            pulldown_shape.place(label_shape, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            label_start.place(pulldown_shape, com.oddlabs.tt.gui.Placement.RIGHT_MID, 20);
            pulldown_player.place(label_start, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            pulldown_team.place(pulldown_player, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            pulldown_count.place(pulldown_team, com.oddlabs.tt.gui.Placement.RIGHT_MID, 20);
            button_auto.place(pulldown_count, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            row_extra.compileCanvas();
            toolbar.addChild(row_extra);
            row_extra.place(group_terrain, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            label_radius = new com.oddlabs.tt.gui.Label("", com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), com.oddlabs.tt.mapeditor.MapEditorDelegate.LABEL_WIDTH);
            label_intensity = new com.oddlabs.tt.gui.Label("", com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), com.oddlabs.tt.mapeditor.MapEditorDelegate.LABEL_WIDTH);
            com.oddlabs.tt.gui.CheckBox check_trees = new com.oddlabs.tt.gui.CheckBox(view.draw_trees, com.oddlabs.tt.mapeditor.MapEditor.i18n("show_trees"));
            check_trees.addCheckBoxListener(new com.oddlabs.tt.guievent.CheckBoxListener_L((bool marked) =>
            {
                view.draw_trees = marked;
                setFocus();
            }));
            label_access_legend = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.mapeditor.MapEditor.i18n("access_legend"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            com.oddlabs.tt.gui.CheckBox check_access = new com.oddlabs.tt.gui.CheckBox(false, com.oddlabs.tt.mapeditor.MapEditor.i18n("show_access"));
            check_access.addCheckBoxListener(new com.oddlabs.tt.guievent.CheckBoxListener_L((bool marked_2) =>
            {
                access.setVisible(marked_2);
                if (marked_2)
                {
                    addChild(label_access_legend);
                }
                else
                {
                    label_access_legend.remove();
                }
                placeAccessLegend();
                setFocus();
            }));
            com.oddlabs.tt.gui.CheckBox check_2d = new com.oddlabs.tt.gui.CheckBox(false, com.oddlabs.tt.mapeditor.MapEditor.i18n("view_2d"));
            check_2d.addCheckBoxListener(new com.oddlabs.tt.guievent.CheckBoxListener_L((bool a0) => { this.setTwoD(a0); }));
            com.oddlabs.tt.gui.CheckBox check_wireframe = new com.oddlabs.tt.gui.CheckBox(view.line_mode, com.oddlabs.tt.mapeditor.MapEditor.i18n("wireframe"));
            check_wireframe.addCheckBoxListener(new com.oddlabs.tt.guievent.CheckBoxListener_L((bool marked_3) =>
            {
                view.line_mode = marked_3;
                setFocus();
            }));
            com.oddlabs.tt.gui.HorizButton button_undo = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.mapeditor.MapEditor.i18n("undo"), 80);
            button_undo.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_17, int unused_18, int unused_19, int unused_20) =>
            {
                undo();
                setFocus();
            }));
            com.oddlabs.tt.gui.HorizButton button_redo = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.mapeditor.MapEditor.i18n("redo"), 80);
            button_redo.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_21, int unused_22, int unused_23, int unused_24) =>
            {
                redo();
                setFocus();
            }));
            com.oddlabs.tt.gui.HorizButton button_menu = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.mapeditor.MapEditor.i18n("menu"), 80);
            button_menu.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_25, int unused_26, int unused_27, int unused_28) =>
            {
                openMenu();
            }));
            label_hint = new com.oddlabs.tt.gui.Label("", com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), com.oddlabs.tt.mapeditor.MapEditorDelegate.HINT_WIDTH);
            com.oddlabs.tt.gui.Label label_controls = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.mapeditor.MapEditor.i18n("hint_controls"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), com.oddlabs.tt.mapeditor.MapEditorDelegate.HINT_WIDTH);
            toolbar.addChild(label_radius);
            toolbar.addChild(label_intensity);
            toolbar.addChild(check_trees);
            toolbar.addChild(check_wireframe);
            toolbar.addChild(check_access);
            toolbar.addChild(check_2d);
            toolbar.addChild(button_undo);
            toolbar.addChild(button_redo);
            toolbar.addChild(button_menu);
            toolbar.addChild(label_hint);
            toolbar.addChild(label_controls);
            label_radius.place(group_resource, com.oddlabs.tt.gui.Placement.RIGHT_MID, 20);
            label_intensity.place(label_radius, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            check_trees.place(label_intensity, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            check_wireframe.place(check_trees, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            check_access.place(check_wireframe, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            check_2d.place(check_access, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            button_undo.place(check_2d, com.oddlabs.tt.gui.Placement.RIGHT_MID, 20);
            button_redo.place(button_undo, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            button_menu.place(button_redo, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            label_hint.place(row_extra, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            label_controls.place(label_hint, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            toolbar.compileCanvas();
            addChild(toolbar);
            refreshLabels();
        }
    }
}
