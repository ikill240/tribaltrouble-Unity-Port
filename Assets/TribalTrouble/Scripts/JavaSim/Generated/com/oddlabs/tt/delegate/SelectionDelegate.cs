// Translated from SelectionDelegate.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.@delegate
{
    // Java: 42
    public class SelectionDelegate : com.oddlabs.tt.@delegate.ControllableCameraDelegate, com.oddlabs.tt.camera.MapCameraOwner
    {
        void com.oddlabs.tt.camera.MapCameraOwner.addChild(com.oddlabs.tt.gui.GUIObject child) => addChild((com.oddlabs.tt.gui.Renderable)child);
        public static org.joml.Vector4fc SELECTION_COLOR;
        public static com.oddlabs.tt.input.GameAction[] ARMY_CREATES;
        public static com.oddlabs.tt.input.GameAction[] ARMY_SELECTS;
        public static com.oddlabs.tt.input.GameAction[] FORMATION_ACTIONS;
        public static com.oddlabs.tt.player.Formation[] FORMATIONS;
        public com.oddlabs.tt.form.InGameChatForm chat_form;
        public com.oddlabs.tt.gui.Label observer_label;
        public com.oddlabs.tt.camera.GameCamera game_camera;
        public bool close_chat_override;
        public bool chat_visible;
        public bool selection;
        public int selection_x1;
        public int selection_y1;
        public int selection_x2;
        public int selection_y2;
        public bool pick_done;
        public bool map_mode;
        public bool observer;
        public int last_idle_peon_name;
        // Java: 91
        public com.oddlabs.tt.form.InGameChatForm getChatForm()
        {
            return chat_form;
        }
        // Java: 95
        public com.oddlabs.tt.gui.ActionButtonPanel getActionButtonPanel()
        {
            return getViewer().getPanel();
        }
        // Java: 99
        public void setObserverMode()
        {
            observer = true;
            getViewer().getSelection().clearSelection();
            if (!map_mode)
            {
                addChild(observer_label);
            }
        }
        // Java: 106
        public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
            @event.consumeAction(com.oddlabs.tt.input.GameAction.UI_ACTIVATE);
            if ((((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.PRESSED) || ((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.REPEAT)) && !map_mode && !observer && (@event.hasAction(com.oddlabs.tt.input.GameAction.GLOBAL_MENU) || @event.hasAction(com.oddlabs.tt.input.GameAction.UI_CANCEL)))
            {
                if (getActionButtonPanel().tryCloseSubmenu(@event))
                {
                    return;
                }
            }
            base.handleInput(@event);
            if (@event.isConsumed())
            {
                return;
            }
            if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.PRESSED))
            {
                if (@event.hasActions())
                {
                    if (@event.consumeAction(com.oddlabs.tt.input.GameAction.CAMERA_MAP_MODE))
                    {
                        if (!map_mode)
                        {
                            this.selection = false;
                            getViewer().getPicker().pickRotate(((com.oddlabs.tt.camera.GameCamera)getCamera()));
                            map_mode = true;
                            if (observer)
                            {
                                observer_label.remove();
                            }
                            else
                            {
                                getActionButtonPanel().remove();
                            }
                            getCamera().disable();
                            setCamera(new com.oddlabs.tt.camera.MapCamera(this, game_camera));
                            getCamera().enable();
                        }
                        @event.consume();
                        return;
                    }
                    if (@event.consumeAction(com.oddlabs.tt.input.GameAction.NOTIFICATION_JUMP))
                    {
                        if (!observer)
                        {
                            com.oddlabs.tt.viewer.Notification n = getViewer().getNotificationManager().getLatestNotification();
                            if (n != null)
                            {
                                if ((getCamera() is com.oddlabs.tt.camera.GameCamera))
                                {
                                    getGUIRoot().pushDelegate(new com.oddlabs.tt.@delegate.JumpDelegate(getViewer(), ((com.oddlabs.tt.camera.GameCamera)getCamera()), n.getX(), n.getY()));
                                }
                                else if ((getCamera() is com.oddlabs.tt.camera.MapCamera))
                                {
                                    (((com.oddlabs.tt.camera.MapCamera)getCamera())).mapGoto(n.getX(), n.getY(), true);
                                }
                            }
                        }
                        @event.consume();
                        return;
                    }
                    for (int i = 0; i <= 9; i++)
                    {
                        if (@event.consumeAction(com.oddlabs.tt.@delegate.SelectionDelegate.ARMY_SELECTS[i]))
                        {
                            if (!map_mode && !observer)
                            {
                                bool selected = getViewer().getSelection().enableShortcutArmy(i);
                                if (selected && @event.getClicks() > 1)
                                {
                                    java.util.Set set = getViewer().getSelection().getCurrentSelection().getSet();
                                    if (!set.isEmpty())
                                    {
                                        com.oddlabs.tt.model.Selectable s = ((com.oddlabs.tt.model.Selectable)set.iterator().next());
                                        getGUIRoot().pushDelegate(new com.oddlabs.tt.@delegate.JumpDelegate(getViewer(), ((com.oddlabs.tt.camera.GameCamera)getCamera()), s.getPositionX(), s.getPositionY()));
                                    }
                                }
                            }
                            @event.consume();
                            return;
                        }
                        if (@event.consumeAction(com.oddlabs.tt.@delegate.SelectionDelegate.ARMY_CREATES[i]))
                        {
                            if (!map_mode && !observer)
                            {
                                getViewer().getSelection().setShortcutArmy(i);
                            }
                            @event.consume();
                            return;
                        }
                    }
                    for (int i_2 = 0; i_2 < com.oddlabs.tt.@delegate.SelectionDelegate.FORMATIONS.Length; i_2++)
                    {
                        if (@event.consumeAction(com.oddlabs.tt.@delegate.SelectionDelegate.FORMATION_ACTIONS[i_2]))
                        {
                            if (!map_mode && !observer)
                            {
                                com.oddlabs.tt.model.Selectable[] selection = getViewer().getSelection().getCurrentSelection().filter(com.oddlabs.tt.model.Abilities.TARGET);
                                getViewer().getLocalPlayer().setFormation(selection, com.oddlabs.tt.@delegate.SelectionDelegate.FORMATIONS[i_2]);
                                getGUIRoot().getInfoPrinter().print(com.oddlabs.tt.util.Utils.getBundleString(java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.@delegate.SelectionDelegate).getName()), "formation_" + java.lang.JStr.valueOf(com.oddlabs.tt.@delegate.SelectionDelegate.FORMATIONS[i_2].name().toLowerCase())));
                            }
                            @event.consume();
                            return;
                        }
                    }
                    if (@event.consumeAction(com.oddlabs.tt.input.GameAction.STAND_GROUND))
                    {
                        if (!map_mode && !observer)
                        {
                            java.util.Set set_2 = getViewer().getSelection().getCurrentSelection().getSet();
                            getViewer().getPeerHub().getPlayerInterface().standGround(((com.oddlabs.tt.model.Selectable[])set_2.toArray(new com.oddlabs.tt.model.Selectable[0])));
                        }
                        @event.consume();
                        return;
                    }
                    if (@event.consumeAction(com.oddlabs.tt.input.GameAction.GLOBAL_CHAT))
                    {
                        if (!chat_visible)
                        {
                            chat_form.setReceivers(true);
                        }
                        @event.consume();
                        return;
                    }
                    if (@event.consumeAction(com.oddlabs.tt.input.GameAction.GLOBAL_CHAT_TEAM))
                    {
                        if (!chat_visible)
                        {
                            chat_form.setReceivers(false);
                        }
                        @event.consume();
                        return;
                    }
                    if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UNIT_BEACON))
                    {
                        if (!map_mode && !observer)
                        {
                            getGUIRoot().pushDelegate(new com.oddlabs.tt.@delegate.BeaconDelegate(getViewer(), ((com.oddlabs.tt.camera.GameCamera)getCamera())));
                        }
                        @event.consume();
                        return;
                    }
                    if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UNIT_NEXT_IDLE))
                    {
                        nextIdlePeon();
                        @event.consume();
                        return;
                    }
                    if (@event.consumeAction(com.oddlabs.tt.input.GameAction.GAME_SPEED_UP))
                    {
                        changeGamespeed(1);
                        @event.consume();
                        return;
                    }
                    if (@event.consumeAction(com.oddlabs.tt.input.GameAction.GAME_SPEED_DOWN))
                    {
                        changeGamespeed((-1));
                        @event.consume();
                        return;
                    }
                    if (@event.hasAction(com.oddlabs.tt.input.GameAction.CAMERA_FIRST_PERSON) || @event.hasAction(com.oddlabs.tt.input.GameAction.CAMERA_ZOOM_MODE))
                    {
                        if (map_mode)
                        {
                            @event.consume();
                            return;
                        }
                    }
                }
                if (map_mode || observer)
                {
                }
                else
                {
                    getActionButtonPanel().handleInput(@event);
                    if (@event.isConsumed())
                    {
                        return;
                    }
                }
            }
            else if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.REPEAT))
            {
                if (@event.hasActions())
                {
                    if (@event.consumeAction(com.oddlabs.tt.input.GameAction.GAME_SPEED_UP))
                    {
                        changeGamespeed(1);
                        @event.consume();
                        return;
                    }
                    if (@event.consumeAction(com.oddlabs.tt.input.GameAction.GAME_SPEED_DOWN))
                    {
                        changeGamespeed((-1));
                        @event.consume();
                        return;
                    }
                }
                if (!map_mode && !observer)
                {
                    getActionButtonPanel().handleInput(@event);
                    if (@event.isConsumed())
                    {
                        return;
                    }
                }
            }
            else if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.RELEASED))
            {
                if (@event.consumeAction(com.oddlabs.tt.input.GameAction.GLOBAL_CHAT) || @event.consumeAction(com.oddlabs.tt.input.GameAction.GLOBAL_CHAT_TEAM))
                {
                    if (!close_chat_override)
                    {
                        if (!chat_visible)
                        {
                            addChild(chat_form);
                            chat_form.setPos(com.oddlabs.tt.camera.GameCamera.SCROLL_BUFFER, com.oddlabs.tt.camera.GameCamera.SCROLL_BUFFER);
                            chat_form.setFocus();
                            chat_visible = true;
                        }
                    }
                    else
                    {
                        close_chat_override = false;
                    }
                    @event.consume();
                    return;
                }
                if (!map_mode && !observer)
                {
                    getActionButtonPanel().handleInput(@event);
                    if (@event.isConsumed())
                    {
                        return;
                    }
                }
            }
            base.handleInput(@event);
        }
        // Java: 324
        public void changeGamespeed(int delta)
        {
            getViewer().getPeerHub().getPlayerInterface().changePreferredGamespeed(delta);
        }
        // Java: 328
        public void nextIdlePeon()
        {
            java.util.Set set = getViewer().getLocalPlayer().getUnits().getSet();
            bool has_idle_peon = false;
            int lowest_name = java.lang.Integer.MAX_VALUE;
            com.oddlabs.tt.model.Selectable lowest_peon = null;
            bool has_greater_name = false;
            int lowest_greater_name = java.lang.Integer.MAX_VALUE;
            com.oddlabs.tt.model.Selectable lowest_greater_peon = null;
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(set))
            {
                if (((object)s.getOwner() != (object)getViewer().getLocalPlayer()))
                {
                    continue;
                }
                com.oddlabs.tt.model.Abilities abilities = s.getAbilities();
                if ((abilities.hasAbilities(com.oddlabs.tt.model.Abilities.BUILD)) && ((s.getPrimaryController() is com.oddlabs.tt.model.behaviour.IdleController)))
                {
                    int name = getViewer().getDistributableTable().getName(s);
                    if (name < lowest_name)
                    {
                        has_idle_peon = true;
                        lowest_name = name;
                        lowest_peon = s;
                    }
                    if (name > last_idle_peon_name && name < lowest_greater_name)
                    {
                        has_greater_name = true;
                        lowest_greater_name = name;
                        lowest_greater_peon = s;
                    }
                }
            }
            com.oddlabs.tt.model.Selectable target = null;
            if (has_greater_name)
            {
                last_idle_peon_name = lowest_greater_name;
                target = lowest_greater_peon;
            }
            else if (has_idle_peon)
            {
                last_idle_peon_name = lowest_name;
                target = lowest_peon;
            }
            if (target != null && (getCamera() is com.oddlabs.tt.camera.GameCamera))
            {
                getViewer().getSelection().clearSelection();
                getViewer().getSelection().getCurrentSelection().add(target);
                getGUIRoot().pushDelegate(new com.oddlabs.tt.@delegate.JumpDelegate(getViewer(), ((com.oddlabs.tt.camera.GameCamera)getCamera()), target.getPositionX(), target.getPositionY()));
            }
        }
        // Java: 374
        public override com.oddlabs.tt.gui.CursorType getCursorType()
        {
            return (map_mode ? com.oddlabs.tt.gui.CursorType.TARGET : com.oddlabs.tt.gui.CursorType.NORMAL);
        }
        // Java: 379
        public void exitMapMode()
        {
            map_mode = false;
            getCamera().disable();
            game_camera.getState().snapToTarget();
            setCamera(game_camera);
            getCamera().enable();
            if (observer)
            {
                addChild(observer_label);
            }
            else
            {
                addChild(getActionButtonPanel());
            }
            if (chat_visible)
            {
                chat_form.remove();
                addChild(chat_form);
                chat_visible = true;
            }
        }
        // Java: 399
        public void updateSelection(java.util.List friendly_units, java.util.List friendly_ships, com.oddlabs.tt.model.Selectable friendly_building, com.oddlabs.tt.model.Selectable enemy)
        {
            com.oddlabs.tt.model.Army current_selection = getViewer().getSelection().getCurrentSelection();
            com.oddlabs.tt.model.Selectable first = ((com.oddlabs.tt.model.Selectable)current_selection.getSet().iterator().next());
            if ((first is com.oddlabs.tt.model.Building) || ((object)first.getOwner() != (object)getViewer().getLocalPlayer()))
            {
                if (((object)first == (object)friendly_building) || ((object)first == (object)enemy))
                {
                    current_selection.clear();
                }
                return;
            }
            if ((first is com.oddlabs.tt.model.Ship))
            {
                toggleSelection(current_selection, friendly_ships);
                return;
            }
            toggleSelection(current_selection, friendly_units);
        }
        // Java: 417
        public void toggleSelection(com.oddlabs.tt.model.Army current_selection, java.util.List picked)
        {
            bool add = false;
            foreach (com.oddlabs.tt.model.Selectable selectable in java.lang.JIter.of(picked))
            {
                if (!current_selection.contains(selectable))
                {
                    add = true;
                    break;
                }
            }
            foreach (com.oddlabs.tt.model.Selectable selectable_2 in java.lang.JIter.of(picked))
            {
                if (add)
                {
                    if (!current_selection.contains(selectable_2))
                    {
                        current_selection.add(selectable_2);
                    }
                }
                else
                {
                    current_selection.remove(selectable_2);
                }
            }
        }
        // Java: 436
        public void replaceSelection(java.util.List friendly_units, java.util.List friendly_ships, com.oddlabs.tt.model.Selectable friendly_building, com.oddlabs.tt.model.Selectable enemy)
        {
            com.oddlabs.tt.model.Army current_selection = getViewer().getSelection().getCurrentSelection();
            current_selection.clear();
            if (!friendly_units.isEmpty())
            {
                foreach (com.oddlabs.tt.model.Selectable friendlyUnit in java.lang.JIter.of(friendly_units))
                {
                    current_selection.add(friendlyUnit);
                }
            }
            else if (!friendly_ships.isEmpty())
            {
                foreach (com.oddlabs.tt.model.Ship ship in java.lang.JIter.of(friendly_ships))
                {
                    current_selection.add(ship);
                }
            }
            else if (friendly_building != null)
            {
                current_selection.add(friendly_building);
            }
            else if (enemy != null)
            {
                current_selection.add(enemy);
            }
        }
        // Java: 456
        public override void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
        {
            if (((object)button == (object)com.oddlabs.tt.gui.MouseButton.LEFT) && !map_mode && !observer)
            {
                if (selection)
                {
                    selection = false;
                    bool shift_down = com.oddlabs.tt.render.Renderer.getLocalInput().isShiftDownCurrently();
                    bool select_by_type = com.oddlabs.tt.render.Renderer.getLocalInput().isControlDownCurrently() && clicks > 1;
                    com.oddlabs.tt.model.Selectable[] picked = getViewer().getPicker().pickBoxed(getViewer().getGUIRoot().getDelegate().getCamera().getState(), selection_x1, selection_y1, selection_x2, selection_y2, clicks, select_by_type);
                    com.oddlabs.tt.model.Selectable clicked = getViewer().getPicker().getLastSinglePick();
                    if (clicks > 1 && (clicked is com.oddlabs.tt.model.Building empty_tower) && !empty_tower.isDead() && ((object)empty_tower.getOwner() == (object)getViewer().getLocalPlayer()) && (empty_tower.getUnitContainer() is com.oddlabs.tt.model.MountUnitContainer empty_mount) && empty_mount.getUnit() == null)
                    {
                        bool any = false;
                        foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(getViewer().getLocalPlayer().getUnits().getSet()))
                        {
                            if ((s is com.oddlabs.tt.model.Building other) && !other.isDead() && other.getIslandId() == empty_tower.getIslandId() && (other.getUnitContainer() is com.oddlabs.tt.model.MountUnitContainer m) && m.getUnit() != null)
                            {
                                getViewer().getPeerHub().getPlayerInterface().exitTower(other);
                                any = true;
                            }
                        }
                        if (any)
                        {
                            pick_done = true;
                            return;
                        }
                    }
                    if (clicks > 1 && picked.Length == 1 && (picked[0] is com.oddlabs.tt.model.Building tower) && ((object)tower.getOwner() == (object)getViewer().getLocalPlayer()) && (tower.getUnitContainer() is com.oddlabs.tt.model.MountUnitContainer mount_container) && mount_container.getUnit() != null)
                    {
                        getViewer().getPeerHub().getPlayerInterface().exitTower(tower);
                        return;
                    }
                    java.util.List friendly_units = new java.util.ArrayList();
                    java.util.List friendly_ships = new java.util.ArrayList();
                    com.oddlabs.tt.model.Selectable friendly_building = null;
                    com.oddlabs.tt.model.Selectable enemy = null;
                    foreach (com.oddlabs.tt.model.Selectable selectable in picked)
                    {
                        if (selectable != null)
                        {
                            if (((object)selectable.getOwner() == (object)getViewer().getLocalPlayer()))
                            {
                                if ((selectable is com.oddlabs.tt.model.Ship ship))
                                {
                                    friendly_ships.add(ship);
                                }
                                else if ((selectable is com.oddlabs.tt.model.LandBuilding building))
                                {
                                    friendly_building = building;
                                }
                                else if ((selectable is com.oddlabs.tt.model.Unit unit))
                                {
                                    friendly_units.add(unit);
                                }
                                else
                                {
                                    throw new System.Exception();
                                }
                            }
                            else if ((selectable is com.oddlabs.tt.model.LandBuilding ally_building) && !getViewer().getLocalPlayer().isEnemy(selectable.getOwner()))
                            {
                                friendly_building = ally_building;
                            }
                            else if ((selectable is com.oddlabs.tt.model.Ship ally_ship) && !getViewer().getLocalPlayer().isEnemy(selectable.getOwner()))
                            {
                                friendly_ships.add(ally_ship);
                            }
                            else if ((selectable is com.oddlabs.tt.model.Unit ally_unit) && !getViewer().getLocalPlayer().isEnemy(selectable.getOwner()))
                            {
                                friendly_units.add(ally_unit);
                            }
                            else
                            {
                                enemy = selectable;
                            }
                        }
                    }
                    if (shift_down && !select_by_type && getViewer().getSelection().getCurrentSelection().size() > 0)
                    {
                        updateSelection(friendly_units, friendly_ships, friendly_building, enemy);
                    }
                    else
                    {
                        replaceSelection(friendly_units, friendly_ships, friendly_building, enemy);
                    }
                    pick_done = true;
                }
            }
        }
        // Java: 562
        public override void mouseReleased(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
            if (map_mode)
            {
                if (((object)button == (object)com.oddlabs.tt.gui.MouseButton.LEFT))
                {
                    getViewer().getPicker().pickMapGoto(x, y, ((com.oddlabs.tt.camera.MapCamera)getCamera()));
                }
            }
            else if (!observer)
            {
                if (!pick_done)
                {
                    mouseClicked(button, x, y, 1);
                }
                pick_done = false;
                base.mouseReleased(button, x, y);
            }
            else
            {
                base.mouseReleased(button, x, y);
            }
        }
        // Java: 578
        public override bool canHoverBehind()
        {
            return true;
        }
        // Java: 583
        public override void mouseDragged(com.oddlabs.tt.gui.MouseButton button, int x, int y, int relative_x, int relative_y, int absolute_x, int absolute_y)
        {
            if (!map_mode)
            {
                if (!observer)
                {
                    if (((object)button == (object)com.oddlabs.tt.gui.MouseButton.LEFT))
                    {
                        selection_x2 += relative_x;
                        selection_y2 += relative_y;
                    }
                    else
                    {
                        base.mouseDragged(button, x, y, relative_x, relative_y, absolute_x, absolute_y);
                    }
                }
                else
                {
                    base.mouseDragged(button, x, y, relative_x, relative_y, absolute_x, absolute_y);
                }
            }
        }
        // Java: 600
        public override void mousePressed(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
            if (!map_mode)
            {
                if (!observer)
                {
                    com.oddlabs.tt.input.InputManager inputManager = com.oddlabs.tt.render.Renderer.getLocalInput().getInputManager();
                    switch ((button).ordinal())
                    {
                        case 0 /*LEFT*/:
                        {
                            if (!inputManager.isActive(com.oddlabs.tt.input.GameAction.CAMERA_MAP_MODE))
                            {
                                this.selection = true;
                            }
                            selection_x1 = x;
                            selection_y1 = y;
                            selection_x2 = x;
                            selection_y2 = y;
                            break;
                        }
                        case 1 /*RIGHT*/:
                        {
                            {
                                com.oddlabs.tt.model.Army selection = getViewer().getSelection().getCurrentSelection();
                                bool queue_order = com.oddlabs.tt.render.Renderer.getLocalInput().isShiftDownCurrently();
                                if (selection.size() > 0)
                                {
                                    if (selection.containsAbility(com.oddlabs.tt.model.Abilities.SAIL))
                                    {
                                        getViewer().getPicker().pickSailingTarget(selection, getViewer().getGUIRoot().getDelegate().getCamera().getState(), getViewer().getPeerHub().getPlayerInterface(), x, y);
                                    }
                                    else if (selection.containsAbility(com.oddlabs.tt.model.Abilities.TARGET))
                                    {
                                        getViewer().getPicker().pickTarget(selection, getViewer().getGUIRoot().getDelegate().getCamera().getState(), getViewer().getPeerHub().getPlayerInterface(), x, y, com.oddlabs.tt.model.Action.DEFAULT, queue_order);
                                    }
                                }
                                break;
                            }
                        }
                        default:
                        {
                            base.mousePressed(button, x, y);
                            break;
                        }
                    }
                }
                else
                {
                    base.mousePressed(button, x, y);
                }
            }
        }
        // Java: 644
        public bool isSelecting()
        {
            return selection;
        }
        // Java: 648
        public override bool keyboardBlocked()
        {
            return chat_visible && chat_form.isActive();
        }
        // Java: 653
        public override void render2D(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            if (com.oddlabs.tt.global.Settings.getSettings().show_compass && getCamera() != null)
            {
                float horizAngle = getCamera().getState().getHorizAngle();
                com.oddlabs.tt.render.CompassRenderer.render(renderer, com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), horizAngle, getGUIRoot().getWidth(), getGUIRoot().getHeight());
            }
            if (selection)
            {
                float minX = java.lang.Math.min(selection_x1, selection_x2);
                float minY = java.lang.Math.min(selection_y1, selection_y2);
                float maxX = java.lang.Math.max(selection_x1, selection_x2);
                float maxY = java.lang.Math.max(selection_y1, selection_y2);
                float w = (float)(maxX - minX);
                float h = (float)(maxY - minY);
                float thickness = (com.oddlabs.tt.global.Settings.getSettings().high_contrast ? 3.0f : 1.0f);
                if (thickness > (float)(w / 2))
                {
                    thickness = (float)(w / 2);
                }
                if (thickness > (float)(h / 2))
                {
                    thickness = (float)(h / 2);
                }
                renderer.drawColoredQuad(minX, minY, w, thickness, com.oddlabs.tt.@delegate.SelectionDelegate.SELECTION_COLOR);
                renderer.drawColoredQuad(minX, (float)(maxY - thickness), w, thickness, com.oddlabs.tt.@delegate.SelectionDelegate.SELECTION_COLOR);
                renderer.drawColoredQuad(minX, (float)(minY + thickness), thickness, (float)(h - (float)(2 * thickness)), com.oddlabs.tt.@delegate.SelectionDelegate.SELECTION_COLOR);
                renderer.drawColoredQuad((float)(maxX - thickness), (float)(minY + thickness), thickness, (float)(h - (float)(2 * thickness)), com.oddlabs.tt.@delegate.SelectionDelegate.SELECTION_COLOR);
            }
        }
        // Java: 682
        public override void displayChangedNotify(int width, int height)
        {
            base.displayChangedNotify(width, height);
            observer_label.setPos((width - observer_label.getWidth()) / 2, height - observer_label.getHeight());
        }
        static SelectionDelegate()
        {
            SelectionDelegate.SELECTION_COLOR = com.oddlabs.util.Color.argb4v((-11731200));
            SelectionDelegate.ARMY_CREATES = new com.oddlabs.tt.input.GameAction[] { com.oddlabs.tt.input.GameAction.ARMY_CREATE_0, com.oddlabs.tt.input.GameAction.ARMY_CREATE_1, com.oddlabs.tt.input.GameAction.ARMY_CREATE_2, com.oddlabs.tt.input.GameAction.ARMY_CREATE_3, com.oddlabs.tt.input.GameAction.ARMY_CREATE_4, com.oddlabs.tt.input.GameAction.ARMY_CREATE_5, com.oddlabs.tt.input.GameAction.ARMY_CREATE_6, com.oddlabs.tt.input.GameAction.ARMY_CREATE_7, com.oddlabs.tt.input.GameAction.ARMY_CREATE_8, com.oddlabs.tt.input.GameAction.ARMY_CREATE_9 };
            SelectionDelegate.ARMY_SELECTS = new com.oddlabs.tt.input.GameAction[] { com.oddlabs.tt.input.GameAction.ARMY_SELECT_0, com.oddlabs.tt.input.GameAction.ARMY_SELECT_1, com.oddlabs.tt.input.GameAction.ARMY_SELECT_2, com.oddlabs.tt.input.GameAction.ARMY_SELECT_3, com.oddlabs.tt.input.GameAction.ARMY_SELECT_4, com.oddlabs.tt.input.GameAction.ARMY_SELECT_5, com.oddlabs.tt.input.GameAction.ARMY_SELECT_6, com.oddlabs.tt.input.GameAction.ARMY_SELECT_7, com.oddlabs.tt.input.GameAction.ARMY_SELECT_8, com.oddlabs.tt.input.GameAction.ARMY_SELECT_9 };
            SelectionDelegate.FORMATION_ACTIONS = new com.oddlabs.tt.input.GameAction[] { com.oddlabs.tt.input.GameAction.FORMATION_SQUARE, com.oddlabs.tt.input.GameAction.FORMATION_DIAMOND, com.oddlabs.tt.input.GameAction.FORMATION_CIRCLE, com.oddlabs.tt.input.GameAction.FORMATION_TIGHT, com.oddlabs.tt.input.GameAction.FORMATION_LOOSE, com.oddlabs.tt.input.GameAction.FORMATION_BY_TYPE, com.oddlabs.tt.input.GameAction.FORMATION_DEFAULT, com.oddlabs.tt.input.GameAction.FORMATION_STAR };
            SelectionDelegate.FORMATIONS = new com.oddlabs.tt.player.Formation[] { com.oddlabs.tt.player.Formation.SQUARE, com.oddlabs.tt.player.Formation.DIAMOND, com.oddlabs.tt.player.Formation.CIRCLE, com.oddlabs.tt.player.Formation.TIGHT, com.oddlabs.tt.player.Formation.LOOSE, com.oddlabs.tt.player.Formation.BY_TYPE, com.oddlabs.tt.player.Formation.DEFAULT, com.oddlabs.tt.player.Formation.STAR };
        }
        private void init_SelectionDelegate()
        {
            this.close_chat_override = false;
            this.selection = false;
            this.pick_done = false;
            this.map_mode = false;
            this.observer = false;
            this.last_idle_peon_name = (-1);
        }
        public SelectionDelegate(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.camera.GameCamera camera) : base(viewer, camera)
        {
            init_SelectionDelegate();
            string observer_mode = com.oddlabs.tt.util.Utils.getBundleString(java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.@delegate.SelectionDelegate).getName()), "observer_mode");
            this.observer_label = new com.oddlabs.tt.gui.Label(observer_mode, com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont());
            this.game_camera = ((com.oddlabs.tt.camera.GameCamera)getCamera());
            displayChangedNotify(getGUIRoot().getWidth(), getGUIRoot().getHeight());
            addChild(getViewer().getPanel());
            chat_form = new com.oddlabs.tt.form.InGameChatForm(getViewer().getGUIRoot().getInfoPrinter(), getViewer());
            chat_form.addCloseListener(new com.oddlabs.tt.guievent.CloseListener_L(() =>
            {
                if (com.oddlabs.tt.render.Renderer.getLocalInput().getInputManager().isActive(com.oddlabs.tt.input.GameAction.GLOBAL_CHAT))
                {
                    close_chat_override = true;
                }
                chat_visible = false;
            }));
            chat_visible = false;
            (((com.oddlabs.tt.camera.GameCamera)getCamera())).setOwner(this);
        }
        // mixin of default method MapCameraOwner.getMaxCameraZ
        public float getMaxCameraZ()
        {
            return com.oddlabs.tt.camera.GameCamera.MAX_Z;
        }
    }
}
