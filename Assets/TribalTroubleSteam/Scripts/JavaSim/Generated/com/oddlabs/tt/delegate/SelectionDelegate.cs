// Translated from SelectionDelegate.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.@delegate
{
    // Java: 49
    public class SelectionDelegate : com.oddlabs.tt.@delegate.ControllableCameraDelegate
    {
        public static java.util.ResourceBundle bundle;
        public static org.joml.Vector4fc SELECTION_COLOR;
        public static com.oddlabs.tt.input.GameAction[] ARMY_CREATES;
        public static com.oddlabs.tt.input.GameAction[] ARMY_SELECTS;
        public com.oddlabs.tt.form.InGameChatForm chat_form;
        public const int SPECTATOR_MARGIN = 10;
        public java.util.List spectator_labels;
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
        // Java: 89
        public com.oddlabs.tt.form.InGameChatForm getChatForm()
        {
            return chat_form;
        }
        // Java: 93
        public com.oddlabs.tt.gui.ActionButtonPanel getActionButtonPanel()
        {
            return getViewer().getPanel();
        }
        // Java: 97
        public void setObserverMode()
        {
            if (observer)
            {
                return;
            }
            observer = true;
            getViewer().getSelection().clearSelection();
            com.oddlabs.tt.viewer.SpectatorView view = getViewer().getSpectatorView();
            if (view != null)
            {
                getActionButtonPanel().remove();
                getViewer().getRenderer().setSelectedBuilding(null);
                view.setListener(new java.lang.Runnable_L(() => { this.refreshSpectator(); }));
                refreshSpectator();
                java.util.function.BooleanSupplier hud_shown = new java.util.function.BooleanSupplier_L(() =>
                {
                    return !map_mode && com.oddlabs.tt.global.Globals.draw_hud;
                });
                addChild(new com.oddlabs.tt.gui.SpectatorCursor(getViewer(), getGUIRoot(), hud_shown));
                addChild(new com.oddlabs.tt.gui.SpectatorSelectionBox(getViewer(), getGUIRoot(), hud_shown));
                addChild(new com.oddlabs.tt.gui.SpectatorPanel(getViewer(), getGUIRoot(), hud_shown));
            }
        }
        // Java: 115
        public static string spectatorText(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.@delegate.SelectionDelegate).getName()), key, args);
        }
        // Java: 121
        public void refreshSpectator()
        {
            com.oddlabs.tt.viewer.SpectatorView view = getViewer().getSpectatorView();
            if (view == null)
            {
                return;
            }
            foreach (com.oddlabs.tt.gui.Label label in java.lang.JIter.of(spectator_labels))
            {
                label.remove();
            }
            spectator_labels.clear();
            if (map_mode || !com.oddlabs.tt.global.Globals.draw_hud)
            {
                return;
            }
            com.oddlabs.tt.player.Player followed = view.getFollowedPlayer();
            com.oddlabs.tt.font.Font headline = com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont();
            int width = getGUIRoot().getWidth();
            int top = getGUIRoot().getHeight() - com.oddlabs.tt.@delegate.SelectionDelegate.SPECTATOR_MARGIN;
            com.oddlabs.tt.gui.Label title = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.@delegate.SelectionDelegate.spectatorText((followed == null ? "spectator_free" : "spectator_following")), headline);
            com.oddlabs.tt.gui.Label name = (followed == null ? (com.oddlabs.tt.gui.Label)(null) : new com.oddlabs.tt.gui.Label(followed.getPlayerInfo().getName(), headline).setColor(followed.getColor()));
            int x = (width - title.getWidth() - ((name == null ? 0 : name.getWidth()))) / 2;
            int y = top - title.getHeight();
            showSpectatorLabel(title, x, y);
            if (name != null)
            {
                showSpectatorLabel(name, x + title.getWidth(), y);
            }
            com.oddlabs.tt.input.InputManager input = com.oddlabs.tt.render.Renderer.getLocalInput().getInputManager();
            string[] lines = new string[] { com.oddlabs.tt.@delegate.SelectionDelegate.spectatorText("spectator_next", input.getBindingString(com.oddlabs.tt.input.GameAction.SPECTATOR_NEXT_PLAYER)), com.oddlabs.tt.@delegate.SelectionDelegate.spectatorText("spectator_previous", input.getBindingString(com.oddlabs.tt.input.GameAction.SPECTATOR_PREV_PLAYER)), com.oddlabs.tt.@delegate.SelectionDelegate.spectatorText("spectator_free_cam", input.getBindingString(com.oddlabs.tt.input.GameAction.SPECTATOR_FREE_CAMERA)), com.oddlabs.tt.@delegate.SelectionDelegate.spectatorText("spectator_exit") };
            y = top;
            foreach (string text in lines)
            {
                com.oddlabs.tt.gui.Label line = new com.oddlabs.tt.gui.Label(text, com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
                y -= line.getHeight();
                showSpectatorLabel(line, com.oddlabs.tt.@delegate.SelectionDelegate.SPECTATOR_MARGIN, y);
            }
        }
        // Java: 155
        public void showSpectatorLabel(com.oddlabs.tt.gui.Label label, int x, int y)
        {
            label.setPos(x, y);
            addChild(label);
            spectator_labels.add(label);
        }
        // Java: 161
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
                com.oddlabs.tt.viewer.SpectatorView view = (observer ? getViewer().getSpectatorView() : (com.oddlabs.tt.viewer.SpectatorView)(null));
                if (view != null)
                {
                    if (@event.consumeAction(com.oddlabs.tt.input.GameAction.SPECTATOR_NEXT_PLAYER))
                    {
                        view.next();
                        @event.consume();
                        return;
                    }
                    if (@event.consumeAction(com.oddlabs.tt.input.GameAction.SPECTATOR_PREV_PLAYER))
                    {
                        view.previous();
                        @event.consume();
                        return;
                    }
                    if (@event.consumeAction(com.oddlabs.tt.input.GameAction.SPECTATOR_FREE_CAMERA))
                    {
                        view.toggleFreeCamera();
                        @event.consume();
                        return;
                    }
                }
                if (@event.hasActions())
                {
                    if (@event.consumeAction(com.oddlabs.tt.input.GameAction.CAMERA_MAP_MODE))
                    {
                        enterMapMode();
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
                    if (@event.consumeAction(com.oddlabs.tt.input.GameAction.GLOBAL_TOGGLE_HUD))
                    {
                        setHUDVisible(!com.oddlabs.tt.global.Globals.draw_hud);
                        @event.consume();
                        return;
                    }
                    if (@event.consumeAction(com.oddlabs.tt.input.GameAction.CAMERA_CINEMATIC))
                    {
                        com.oddlabs.tt.global.Globals.cinematic_camera = !com.oddlabs.tt.global.Globals.cinematic_camera;
                        getGUIRoot().getInfoPrinter().print(com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.@delegate.SelectionDelegate.bundle, (com.oddlabs.tt.global.Globals.cinematic_camera ? "cinematic_on" : "cinematic_off")));
                        @event.consume();
                        return;
                    }
                    if (@event.consumeAction(com.oddlabs.tt.input.GameAction.CAMERA_ORBIT_LEFT))
                    {
                        if (!map_mode)
                        {
                            game_camera.toggleOrbit(1);
                        }
                        @event.consume();
                        return;
                    }
                    if (@event.consumeAction(com.oddlabs.tt.input.GameAction.CAMERA_ORBIT_RIGHT))
                    {
                        if (!map_mode)
                        {
                            game_camera.toggleOrbit((-1));
                        }
                        @event.consume();
                        return;
                    }
                    if (@event.consumeAction(com.oddlabs.tt.input.GameAction.CAMERA_AUTO_PAN_FORWARD))
                    {
                        if (!map_mode)
                        {
                            game_camera.toggleAutoPan(1);
                        }
                        @event.consume();
                        return;
                    }
                    if (@event.consumeAction(com.oddlabs.tt.input.GameAction.CAMERA_AUTO_PAN_BACKWARD))
                    {
                        if (!map_mode)
                        {
                            game_camera.toggleAutoPan((-1));
                        }
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
        // Java: 389
        public void changeGamespeed(int delta)
        {
            getViewer().getPeerHub().getPlayerInterface().changePreferredGamespeed(delta);
        }
        // Java: 393
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
        // Java: 439
        public override com.oddlabs.tt.gui.CursorType getCursorType()
        {
            if (!com.oddlabs.tt.global.Globals.draw_hud)
            {
                return com.oddlabs.tt.gui.CursorType.HIDDEN;
            }
            return (map_mode ? com.oddlabs.tt.gui.CursorType.TARGET : com.oddlabs.tt.gui.CursorType.NORMAL);
        }
        // Java: 446
        public override bool renderCursor()
        {
            return com.oddlabs.tt.global.Globals.draw_hud;
        }
        // Java: 451
        public void enterMapMode()
        {
            if (map_mode)
            {
                return;
            }
            selection = false;
            getViewer().getPicker().pickRotate(((com.oddlabs.tt.camera.GameCamera)getCamera()));
            map_mode = true;
            if (observer)
            {
                refreshSpectator();
            }
            else
            {
                getActionButtonPanel().remove();
            }
            getCamera().disable();
            setCamera(new com.oddlabs.tt.camera.MapCamera(this, game_camera));
            getCamera().enable();
        }
        // Java: 466
        public void exitMapMode()
        {
            map_mode = false;
            getCamera().disable();
            game_camera.getState().snapToTarget();
            setCamera(game_camera);
            getCamera().enable();
            if (observer)
            {
                refreshSpectator();
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
        // Java: 486
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
        // Java: 504
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
        // Java: 523
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
        // Java: 543
        public override void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
        {
            if (((object)button == (object)com.oddlabs.tt.gui.MouseButton.LEFT) && !map_mode && !observer)
            {
                if (selection)
                {
                    selection = false;
                    com.oddlabs.tt.model.Selectable[] picked = getViewer().getPicker().pickBoxed(getViewer().getGUIRoot().getDelegate().getCamera().getState(), selection_x1, selection_y1, selection_x2, selection_y2, clicks);
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
                            else
                            {
                                enemy = selectable;
                            }
                        }
                    }
                    if (com.oddlabs.tt.render.Renderer.getLocalInput().isShiftDownCurrently() && getViewer().getSelection().getCurrentSelection().size() > 0)
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
        // Java: 582
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
        // Java: 598
        public override bool canHoverBehind()
        {
            return true;
        }
        // Java: 603
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
        // Java: 620
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
                                if (selection.size() > 0)
                                {
                                    if (selection.containsAbility(com.oddlabs.tt.model.Abilities.SAIL))
                                    {
                                        getViewer().getPicker().pickSailingTarget(selection, getViewer().getGUIRoot().getDelegate().getCamera().getState(), getViewer().getPeerHub().getPlayerInterface(), x, y);
                                    }
                                    else if (selection.containsAbility(com.oddlabs.tt.model.Abilities.TARGET))
                                    {
                                        getViewer().getPicker().pickTarget(selection, getViewer().getGUIRoot().getDelegate().getCamera().getState(), getViewer().getPeerHub().getPlayerInterface(), x, y, com.oddlabs.tt.model.Action.DEFAULT);
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
        // Java: 661
        public bool isInMapMode()
        {
            return map_mode;
        }
        // Java: 666
        public bool isOnMap()
        {
            return map_mode && (getCamera() is com.oddlabs.tt.camera.MapCamera map_camera) && !map_camera.isLeaving();
        }
        // Java: 671
        public bool getSelectionBox(int[] dest)
        {
            dest[0] = selection_x1;
            dest[1] = selection_y1;
            dest[2] = selection_x2;
            dest[3] = selection_y2;
            return selection;
        }
        // Java: 679
        public bool isSelecting()
        {
            return selection;
        }
        // Java: 683
        public void setHUDVisible(bool visible)
        {
            com.oddlabs.tt.global.Globals.draw_hud = visible;
            com.oddlabs.tt.render.Renderer.getLocalInput().getPointerInput().setActiveCursor(getCursorType());
            if (map_mode)
            {
                return;
            }
            if (observer)
            {
                refreshSpectator();
                return;
            }
            if (visible)
            {
                addChild(getActionButtonPanel());
            }
            else
            {
                getActionButtonPanel().remove();
            }
        }
        // Java: 698
        public override bool keyboardBlocked()
        {
            return chat_visible && chat_form.isActive();
        }
        // Java: 703
        public override void render2D(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            if (!com.oddlabs.tt.global.Globals.draw_hud)
            {
                return;
            }
            if (com.oddlabs.tt.global.Settings.getSettings().show_compass && getCamera() != null)
            {
                float horizAngle = getCamera().getState().getHorizAngle();
                com.oddlabs.tt.render.CompassRenderer.render(renderer, com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), horizAngle, getGUIRoot().getWidth(), getGUIRoot().getHeight());
            }
            if (selection)
            {
                com.oddlabs.tt.@delegate.SelectionDelegate.drawSelectionBox(renderer, selection_x1, selection_y1, selection_x2, selection_y2);
            }
        }
        // Java: 717
        public static void drawSelectionBox(com.oddlabs.tt.render.GUIRenderer renderer, float x1, float y1, float x2, float y2)
        {
            float minX = java.lang.Math.min(x1, x2);
            float minY = java.lang.Math.min(y1, y2);
            float maxX = java.lang.Math.max(x1, x2);
            float maxY = java.lang.Math.max(y1, y2);
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
        // Java: 737
        public override void displayChangedNotify(int width, int height)
        {
            base.displayChangedNotify(width, height);
            if (observer)
            {
                refreshSpectator();
            }
        }
        static SelectionDelegate()
        {
            SelectionDelegate.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.@delegate.SelectionDelegate).getName());
            SelectionDelegate.SELECTION_COLOR = com.oddlabs.util.Color.argb4v((-11731200));
            SelectionDelegate.ARMY_CREATES = new com.oddlabs.tt.input.GameAction[] { com.oddlabs.tt.input.GameAction.ARMY_CREATE_0, com.oddlabs.tt.input.GameAction.ARMY_CREATE_1, com.oddlabs.tt.input.GameAction.ARMY_CREATE_2, com.oddlabs.tt.input.GameAction.ARMY_CREATE_3, com.oddlabs.tt.input.GameAction.ARMY_CREATE_4, com.oddlabs.tt.input.GameAction.ARMY_CREATE_5, com.oddlabs.tt.input.GameAction.ARMY_CREATE_6, com.oddlabs.tt.input.GameAction.ARMY_CREATE_7, com.oddlabs.tt.input.GameAction.ARMY_CREATE_8, com.oddlabs.tt.input.GameAction.ARMY_CREATE_9 };
            SelectionDelegate.ARMY_SELECTS = new com.oddlabs.tt.input.GameAction[] { com.oddlabs.tt.input.GameAction.ARMY_SELECT_0, com.oddlabs.tt.input.GameAction.ARMY_SELECT_1, com.oddlabs.tt.input.GameAction.ARMY_SELECT_2, com.oddlabs.tt.input.GameAction.ARMY_SELECT_3, com.oddlabs.tt.input.GameAction.ARMY_SELECT_4, com.oddlabs.tt.input.GameAction.ARMY_SELECT_5, com.oddlabs.tt.input.GameAction.ARMY_SELECT_6, com.oddlabs.tt.input.GameAction.ARMY_SELECT_7, com.oddlabs.tt.input.GameAction.ARMY_SELECT_8, com.oddlabs.tt.input.GameAction.ARMY_SELECT_9 };
        }
        private void init_SelectionDelegate()
        {
            this.spectator_labels = new java.util.ArrayList();
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
    }
}
