// Translated from PlacingDelegate.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.@delegate
{
    // Java: 36
    public class PlacingDelegate : com.oddlabs.tt.@delegate.ControllableCameraDelegate
    {
        public static java.util.logging.Logger logger;
        public const int GRID_RADIUS = 20;
        public static com.oddlabs.tt.render.LandscapeLocation landscape_hit;
        public com.oddlabs.tt.render.BuildingSiteRenderer site_renderer;
        public int building_index;
        public com.oddlabs.tt.render.shader.SpriteShader spriteShader;
        public bool has_placed_once;
        public const int MAX_WALL_POSTS = 60;
        public const int MAX_SHAPE_POSTS = 160;
        public bool wall_dragging;
        public int wall_x0;
        public int wall_y0;
        // Java: 65
        public com.oddlabs.tt.model.BuildingTemplate getTemplate()
        {
            return getViewer().getLocalPlayer().getRace().getBuildingTemplate(building_index);
        }
        // Java: 70
        public bool isWall()
        {
            return building_index == com.oddlabs.tt.model.Race.BUILDING_PALISADE || isBridge();
        }
        // Java: 75
        public bool isBridge()
        {
            return building_index == com.oddlabs.tt.model.Race.BUILDING_BRIDGE;
        }
        // Java: 83
        public int[][] getWallPosts()
        {
            if (!getViewer().getPicker().pickLocation(getCamera().getState(), com.oddlabs.tt.@delegate.PlacingDelegate.landscape_hit))
            {
                return new int[0][];
            }
            int gx = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(com.oddlabs.tt.@delegate.PlacingDelegate.landscape_hit.x);
            int gy = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(com.oddlabs.tt.@delegate.PlacingDelegate.landscape_hit.y);
            if (isBridge())
            {
                return bridgeSpans((wall_dragging ? wall_x0 : gx), (wall_dragging ? wall_y0 : gy), gx, gy);
            }
            if (!wall_dragging)
            {
                return wallPosts(gx, gy, gx, gy);
            }
            java.util.List posts = new java.util.ArrayList();
            if (com.oddlabs.tt.render.Renderer.getLocalInput().isControlDownCurrently())
            {
                int step = wallStep();
                float r = ((float)(java.lang.Math.sqrt((gx - wall_x0) * (gx - wall_x0) + (gy - wall_y0) * (gy - wall_y0))));
                int n = java.lang.Math.max(6, java.lang.Math.min(com.oddlabs.tt.@delegate.PlacingDelegate.MAX_SHAPE_POSTS, java.lang.Math.round(((float)((2 * java.lang.Math.PI * r / step))))));
                for (int i = 0; i < n; i++)
                {
                    double a = 2 * java.lang.Math.PI * i / n;
                    addPost(posts, java.lang.Math.round((float)(wall_x0 + (float)(r * ((float)(java.lang.Math.cos(a)))))), java.lang.Math.round((float)(wall_y0 + (float)(r * ((float)(java.lang.Math.sin(a)))))));
                }
            }
            else if (com.oddlabs.tt.render.Renderer.getLocalInput().isShiftDownCurrently())
            {
                line(posts, wall_x0, wall_y0, gx, wall_y0);
                line(posts, gx, wall_y0, gx, gy);
                line(posts, gx, gy, wall_x0, gy);
                line(posts, wall_x0, gy, wall_x0, wall_y0);
            }
            else
            {
                line(posts, wall_x0, wall_y0, gx, gy);
            }
            return ((int[][])posts.toArray(new int[0][]));
        }
        // Java: 116
        public int[][] bridgeSpans(int x0, int y0, int x1, int y1)
        {
            return com.oddlabs.tt.model.BridgeSpan.layout(getViewer().getWorld().getUnitGrid(), x0, y0, x1, y1);
        }
        // Java: 120
        public int wallStep()
        {
            return java.lang.Math.max(1, 2 * getTemplate().getPlacingSize() - 1);
        }
        // Java: 125
        public void addPost(java.util.List posts, int gx, int gy)
        {
            int step = wallStep();
            foreach (int[] p in java.lang.JIter.of(posts))
            {
                if (java.lang.Math.abs(p[0] - gx) < step && java.lang.Math.abs(p[1] - gy) < step)
                {
                    return;
                }
            }
            if (posts.size() >= com.oddlabs.tt.@delegate.PlacingDelegate.MAX_SHAPE_POSTS)
            {
                return;
            }
            posts.add(new int[] { gx, gy, (getTemplate().isPlacingLegal(getViewer().getWorld().getUnitGrid(), gx, gy) ? 1 : 0) });
        }
        // Java: 135
        public void line(java.util.List posts, int x0, int y0, int x1, int y1)
        {
            int step = wallStep();
            int dx = x1 - x0;
            int dy = y1 - y0;
            float len = ((float)(java.lang.Math.sqrt(dx * dx + dy * dy)));
            int n = java.lang.Math.min(com.oddlabs.tt.@delegate.PlacingDelegate.MAX_WALL_POSTS, java.lang.JC.d2i(java.lang.Math.floor((float)(len / step))) + 1);
            for (int i = 0; i < n; i++)
            {
                float t = (n == 1 ? 0.0f : (float)(i * step / len));
                addPost(posts, java.lang.Math.round((float)(x0 + (float)(dx * t))), java.lang.Math.round((float)(y0 + (float)(dy * t))));
            }
            if (n > 1)
            {
                addPost(posts, x1, y1);
            }
        }
        // Java: 149
        public int[][] wallPosts(int x0, int y0, int x1, int y1)
        {
            java.util.List posts = new java.util.ArrayList();
            line(posts, x0, y0, x1, y1);
            return ((int[][])posts.toArray(new int[0][]));
        }
        // Java: 155
        public void placeWall()
        {
            int[][] posts = getWallPosts();
            wall_dragging = false;
            com.oddlabs.tt.model.Selectable[] peons = getViewer().getSelection().getCurrentSelection().filter(com.oddlabs.tt.model.Abilities.BUILD);
            if (peons.Length == 0)
            {
                com.oddlabs.tt.@delegate.PlacingDelegate.logger.info("placeWall: No peons selected");
                return;
            }
            bool placed = false;
            foreach (int[] p in posts)
            {
                if (p[2] == 0)
                {
                    continue;
                }
                getViewer().getPeerHub().getPlayerInterface().placeBuilding(peons, building_index, p[0], p[1], has_placed_once);
                has_placed_once = true;
                placed = true;
            }
            if (placed && !com.oddlabs.tt.render.Renderer.getLocalInput().isShiftDownCurrently())
            {
                pop();
            }
        }
        // Java: 176
        public void placeObject()
        {
            if (!getViewer().getPicker().pickLocation(getCamera().getState(), com.oddlabs.tt.@delegate.PlacingDelegate.landscape_hit))
            {
                com.oddlabs.tt.@delegate.PlacingDelegate.logger.info("placeObject: Pick failed (off map?)");
                return;
            }
            com.oddlabs.tt.pathfinder.UnitGrid unit_grid = getViewer().getWorld().getUnitGrid();
            int placing_grid_x = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(com.oddlabs.tt.@delegate.PlacingDelegate.landscape_hit.x);
            int placing_grid_y = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(com.oddlabs.tt.@delegate.PlacingDelegate.landscape_hit.y);
            if (getTemplate().isPlacingLegal(getViewer().getWorld().getUnitGrid(), placing_grid_x, placing_grid_y))
            {
                com.oddlabs.tt.model.Selectable[] peons = getViewer().getSelection().getCurrentSelection().filter(com.oddlabs.tt.model.Abilities.BUILD);
                if (peons.Length > 0)
                {
                    com.oddlabs.tt.@delegate.PlacingDelegate.logger.info("placeObject: Placing building at " + (placing_grid_x) + "," + (placing_grid_y));
                    if (getViewer().getWorld().isFreeForm())
                    {
                        int offset_x = java.lang.Math.round((float)(((float)(com.oddlabs.tt.@delegate.PlacingDelegate.landscape_hit.x - com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(placing_grid_x))) * 100.0f));
                        int offset_y = java.lang.Math.round((float)(((float)(com.oddlabs.tt.@delegate.PlacingDelegate.landscape_hit.y - com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(placing_grid_y))) * 100.0f));
                        getViewer().getPeerHub().getPlayerInterface().placeBuildingFree(peons, building_index, placing_grid_x, placing_grid_y, offset_x, offset_y, has_placed_once);
                    }
                    else
                    {
                        getViewer().getPeerHub().getPlayerInterface().placeBuilding(peons, building_index, placing_grid_x, placing_grid_y, has_placed_once);
                    }
                    has_placed_once = true;
                }
                else
                {
                    com.oddlabs.tt.@delegate.PlacingDelegate.logger.info("placeObject: No peons selected");
                }
                if (com.oddlabs.tt.render.Renderer.getLocalInput().isShiftDownCurrently())
                {
                    com.oddlabs.tt.@delegate.PlacingDelegate.logger.info("placeObject: Shift held, staying in placement mode");
                }
                else
                {
                    com.oddlabs.tt.@delegate.PlacingDelegate.logger.info("placeObject: Popping delegate");
                    pop();
                }
            }
            else
            {
                com.oddlabs.tt.@delegate.PlacingDelegate.logger.info("placeObject: Placement illegal");
            }
        }
        // Java: 221
        public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
            if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_ACTIVATE))
            {
                if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.RELEASED))
                {
                    if (isWall())
                    {
                        placeWall();
                    }
                    else
                    {
                        placeObject();
                    }
                }
                @event.consume();
                return;
            }
            if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.PRESSED) || ((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.REPEAT))
            {
                if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_CANCEL))
                {
                    pop();
                    @event.consume();
                    return;
                }
            }
            base.handleInput(@event);
        }
        // Java: 245
        public override void mousePressed(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
            if (isWall() && ((object)button == (object)com.oddlabs.tt.gui.MouseButton.LEFT))
            {
                if (getViewer().getPicker().pickLocation(getCamera().getState(), com.oddlabs.tt.@delegate.PlacingDelegate.landscape_hit))
                {
                    wall_x0 = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(com.oddlabs.tt.@delegate.PlacingDelegate.landscape_hit.x);
                    wall_y0 = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(com.oddlabs.tt.@delegate.PlacingDelegate.landscape_hit.y);
                    wall_dragging = true;
                }
                return;
            }
            if (isWall() && ((object)button == (object)com.oddlabs.tt.gui.MouseButton.RIGHT) && wall_dragging)
            {
                wall_dragging = false;
                return;
            }
            switch ((button).ordinal())
            {
                case 0 /*LEFT*/:
                {
                    placeObject();
                    break;
                }
                case 1 /*RIGHT*/:
                {
                    pop();
                    break;
                }
                default:
                {
                    base.mousePressed(button, x, y);
                    break;
                }
            }
        }
        // Java: 266
        public override void mouseReleased(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
            if (isWall() && ((object)button == (object)com.oddlabs.tt.gui.MouseButton.LEFT) && wall_dragging)
            {
                placeWall();
                return;
            }
            base.mouseReleased(button, x, y);
        }
        // Java: 275
        public override void render3D(com.oddlabs.tt.render.LandscapeRenderer renderer, com.oddlabs.tt.render.RenderQueues queues, com.oddlabs.tt.camera.CameraState state, com.oddlabs.tt.render.MatrixStack modelViewStack, com.oddlabs.tt.render.MatrixStack projectionStack)
        {
            if (!getViewer().getPicker().pickLocation(getCamera().getState(), com.oddlabs.tt.@delegate.PlacingDelegate.landscape_hit))
            {
                return;
            }
            com.oddlabs.tt.pathfinder.UnitGrid unit_grid = getViewer().getWorld().getUnitGrid();
            int placing_grid_x = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(com.oddlabs.tt.@delegate.PlacingDelegate.landscape_hit.x) - (getTemplate().getPlacingSize() - 1);
            int placing_grid_y = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(com.oddlabs.tt.@delegate.PlacingDelegate.landscape_hit.y) - (getTemplate().getPlacingSize() - 1);
            int placing_center_grid_x = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(com.oddlabs.tt.@delegate.PlacingDelegate.landscape_hit.x);
            int placing_center_grid_y = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(com.oddlabs.tt.@delegate.PlacingDelegate.landscape_hit.y);
            float center_x = (float)(com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID * ((float)(placing_grid_x + ((float)(getTemplate().getPlacingSize() - 0.5f)))));
            float center_y = (float)(com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID * ((float)(placing_grid_y + ((float)(getTemplate().getPlacingSize() - 0.5f)))));
            com.oddlabs.tt.player.BuildingSiteScanFilter filter = new com.oddlabs.tt.player.BuildingSiteScanFilter(unit_grid, getTemplate(), com.oddlabs.tt.@delegate.PlacingDelegate.GRID_RADIUS, false);
            unit_grid.scan(filter, placing_center_grid_x, placing_center_grid_y);
            java.util.List target_list = filter.getResult();
            com.oddlabs.tt.render.state.RenderContext context = com.oddlabs.tt.render.Renderer.getRenderer().getRenderContext();
            if (!getViewer().getWorld().isFreeForm())
            {
                site_renderer.renderSites(context, renderer, modelViewStack, projectionStack, target_list, center_x, center_y, 2 * com.oddlabs.tt.@delegate.PlacingDelegate.GRID_RADIUS);
            }
            com.oddlabs.tt.util.GLUtils.checkGLError("Placing: After renderSites");
            com.oddlabs.tt.render.SpriteRenderer built_renderer = queues.getRenderer(getTemplate().getBuiltRenderer());
            com.oddlabs.tt.render.Sprite sprite = built_renderer.getSpriteList().getSprite(0);
            {
                com.oddlabs.tt.render.state.ScopedState unused = spriteShader.use();
                try
                {
                    spriteShader.setUniform(com.oddlabs.tt.render.shader.SpriteShader.Uniforms.DESATURATE, 0.5f);
                    sprite.setupShaderUniforms(context, spriteShader, 0, false);
                    spriteShader.setUniform(com.oddlabs.tt.render.shader.SpriteShader.Uniforms.MODULATE_COLOR, true);
                    spriteShader.setUniform(com.oddlabs.tt.render.shader.SpriteShader.Uniforms.ALPHA_TEST_VALUE, 0.5f);
                    float[][] ghosts = default;
                    if (isWall())
                    {
                        int[][] posts = getWallPosts();
                        ghosts = new float[posts.Length][];
                        for (int i = 0; i < posts.Length; i++)
                        {
                            ghosts[i] = new float[] { (float)(com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID * ((float)(posts[i][0] + 0.5f))), (float)(com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID * ((float)(posts[i][1] + 0.5f))), posts[i][2] };
                        }
                    }
                    else
                    {
                        ghosts = new float[][] { new float[] { center_x, center_y, (getTemplate().isPlacingLegal(unit_grid, placing_center_grid_x, placing_center_grid_y) ? 1 : 0) } };
                    }
                    foreach (float[] ghost in ghosts)
                    {
                        if (ghost[2] != 0)
                        {
                            spriteShader.setUniform(com.oddlabs.tt.render.shader.SpriteShader.Uniforms.COLOR, 1.0f, 1.0f, 1.0f, 0.8f);
                        }
                        else
                        {
                            spriteShader.setUniform(com.oddlabs.tt.render.shader.SpriteShader.Uniforms.COLOR, 1.0f, 0.0f, 0.0f, 0.8f);
                        }
                        float z = getViewer().getWorld().getHeightMap().getNearestHeight(ghost[0], ghost[1]);
                        modelViewStack.push();
                        modelViewStack.translate(ghost[0], ghost[1], z);
                        spriteShader.setUniformMatrix4(com.oddlabs.tt.render.shader.SpriteShader.Uniforms.MODEL_VIEW_MATRIX, false, modelViewStack.current());
                        {
                            com.oddlabs.tt.render.state.ScopedState unused_2 = context.withCullMode(com.oddlabs.tt.render.state.CullMode.BACK);
                            try
                            {
                                {
                                    com.oddlabs.tt.render.state.ScopedState unused_3 = context.withDepthMode(com.oddlabs.tt.render.state.DepthMode.READ_WRITE);
                                    com.oddlabs.tt.render.state.ScopedState unused_4 = context.withColorMask(false, false, false, false);
                                    com.oddlabs.tt.render.state.ScopedState unused_5 = context.withBlendMode(com.oddlabs.tt.render.state.BlendMode.NONE);
                                    try
                                    {
                                        sprite.renderShader(spriteShader, 0, 0.0f, built_renderer.getSpriteList());
                                    }
                                    finally
                                    {
                                        if (unused_5 != null) unused_5.close();
                                        if (unused_4 != null) unused_4.close();
                                        if (unused_3 != null) unused_3.close();
                                    }
                                }
                                {
                                    com.oddlabs.tt.render.state.ScopedState unused_6 = context.withDepthMode(com.oddlabs.tt.render.state.DepthMode.READ_ONLY);
                                    com.oddlabs.tt.render.state.ScopedState unused_7 = context.withColorMask(true, true, true, true);
                                    com.oddlabs.tt.render.state.ScopedState unused_8 = context.withBlendMode(com.oddlabs.tt.render.state.BlendMode.ALPHA);
                                    try
                                    {
                                        sprite.renderShader(spriteShader, 0, 0.0f, built_renderer.getSpriteList());
                                    }
                                    finally
                                    {
                                        if (unused_8 != null) unused_8.close();
                                        if (unused_7 != null) unused_7.close();
                                        if (unused_6 != null) unused_6.close();
                                    }
                                }
                            }
                            finally
                            {
                                if (unused_2 != null) unused_2.close();
                                spriteShader.setUniform(com.oddlabs.tt.render.shader.SpriteShader.Uniforms.DESATURATE, 0.0f);
                                spriteShader.setUniform(com.oddlabs.tt.render.shader.SpriteShader.Uniforms.MODULATE_COLOR, false);
                                spriteShader.setUniform(com.oddlabs.tt.render.shader.SpriteShader.Uniforms.ALPHA_TEST_VALUE, 0.3f);
                            }
                        }
                        modelViewStack.pop();
                    }
                }
                finally
                {
                    if (unused != null) unused.close();
                }
            }
        }
        static PlacingDelegate()
        {
            PlacingDelegate.logger = java.util.logging.Logger.getLogger(typeof(com.oddlabs.tt.@delegate.PlacingDelegate).getName());
            PlacingDelegate.landscape_hit = new com.oddlabs.tt.render.LandscapeLocation();
        }
        private void init_PlacingDelegate()
        {
            this.site_renderer = new com.oddlabs.tt.render.BuildingSiteRenderer();
            this.spriteShader = new com.oddlabs.tt.render.shader.SpriteShader();
            this.has_placed_once = false;
        }
        public PlacingDelegate(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.camera.CameraState old_camera, int building_index) : base(viewer, new com.oddlabs.tt.camera.GameCamera(viewer, old_camera))
        {
            init_PlacingDelegate();
            this.building_index = building_index;
        }
    }
}
