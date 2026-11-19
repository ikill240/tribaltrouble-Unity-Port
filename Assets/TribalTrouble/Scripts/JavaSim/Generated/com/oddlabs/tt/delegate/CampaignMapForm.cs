// Translated from CampaignMapForm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.@delegate
{
    // Java: 40
    public class CampaignMapForm : com.oddlabs.tt.@delegate.CameraDelegate, com.oddlabs.tt.animation.Animated
    {
        public const float BASE_WIDTH = 800.0f;
        public const float BASE_HEIGHT = 600.0f;
        public static java.util.ResourceBundle bundle;
        // Java: 45
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.@delegate.CampaignMapForm.bundle, key, args);
        }
        public float scale_x;
        public float scale_y;
        public com.oddlabs.tt.player.campaign.Campaign campaign;
        public com.oddlabs.net.NetworkSelector network;
        public java.util.List islandButtons;
        public bool initialFocusSet;
        public org.joml.Vector4f mapColor;
        // Java: 259
        public override void setFocus(com.oddlabs.tt.gui.FocusDirection direction)
        {
            if (((object)direction == (object)com.oddlabs.tt.gui.FocusDirection.BACKWARD))
            {
                base.setFocus(direction);
            }
            else
            {
                setFocus();
            }
        }
        // Java: 268
        public override void setFocus()
        {
            if (islandButtons.isEmpty())
            {
                base.setFocus(com.oddlabs.tt.gui.FocusDirection.FORWARD);
                return;
            }
            if ((getFocusedChild() is com.oddlabs.tt.gui.MapIslandButton))
            {
                return;
            }
            com.oddlabs.tt.gui.MapIslandButton toFocus = null;
            if (!initialFocusSet)
            {
                int currentIsland = campaign.getState().getCurrentIsland();
                foreach (com.oddlabs.tt.gui.MapIslandButton button in java.lang.JIter.of(islandButtons))
                {
                    if (button.getIslandIndex() == currentIsland)
                    {
                        toFocus = button;
                        break;
                    }
                }
                initialFocusSet = true;
            }
            if (toFocus == null)
            {
                toFocus = ((com.oddlabs.tt.gui.MapIslandButton)islandButtons.getLast());
            }
            toFocus.setFocus();
        }
        // Java: 299
        public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
            if (!@event.isConsumed() && ((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.PRESSED))
            {
                int dx = 0;
                int dy = 0;
                if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_NAV_UP))
                {
                    dy = (-1);
                }
                else if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_NAV_DOWN))
                {
                    dy = 1;
                }
                else if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_NAV_LEFT))
                {
                    dx = (-1);
                }
                else if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_NAV_RIGHT))
                {
                    dx = 1;
                }
                else if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_FOCUS_NEXT))
                {
                    focusNext();
                    @event.consume();
                    return;
                }
                else if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_FOCUS_PREV))
                {
                    focusPrior();
                    @event.consume();
                    return;
                }
                else if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_ACTIVATE))
                {
                    if ((getFocusedChild() is com.oddlabs.tt.gui.MapIslandButton button))
                    {
                        campaign.islandChosen(network, getGUIRoot(), button.getIslandIndex());
                        @event.consume();
                        return;
                    }
                }
                if (dx != 0 || dy != 0)
                {
                    navigate(dx, dy);
                    @event.consume();
                    return;
                }
            }
            if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.PRESSED))
            {
                if (@event.consumeAction(com.oddlabs.tt.input.GameAction.GLOBAL_MENU) || @event.consumeAction(com.oddlabs.tt.input.GameAction.UI_CANCEL))
                {
                    getGUIRoot().addModalForm(new com.oddlabs.tt.@delegate.CampaignMapMenu(network, getGUIRoot()));
                    @event.consume();
                    return;
                }
            }
            base.handleInput(@event);
        }
        // Java: 341
        public void navigate(int dx, int dy)
        {
            if (islandButtons.isEmpty())
            {
                return;
            }
            com.oddlabs.tt.gui.MapIslandButton current = null;
            if ((getFocusedChild() is com.oddlabs.tt.gui.MapIslandButton b) && islandButtons.contains(b))
            {
                current = b;
            }
            if (current == null)
            {
                ((com.oddlabs.tt.gui.MapIslandButton)islandButtons.getLast()).setFocus();
                return;
            }
            com.oddlabs.tt.gui.MapIslandButton best = null;
            float bestScore = java.lang.Float.MAX_VALUE;
            foreach (com.oddlabs.tt.gui.MapIslandButton candidate in java.lang.JIter.of(islandButtons))
            {
                if (((object)candidate == (object)current))
                {
                    continue;
                }
                float cdx = candidate.getX() - current.getX();
                float cdy = candidate.getY() - current.getY();
                bool inDir = false;
                if (dy < 0)
                {
                    inDir = cdy < (-1) && java.lang.Math.abs(cdx) < (float)(java.lang.Math.abs(cdy) * 2);
                }
                else if (dy > 0)
                {
                    inDir = cdy > 1 && java.lang.Math.abs(cdx) < (float)(java.lang.Math.abs(cdy) * 2);
                }
                else if (dx < 0)
                {
                    inDir = cdx < (-1) && java.lang.Math.abs(cdy) < (float)(java.lang.Math.abs(cdx) * 2);
                }
                else if (dx > 0)
                {
                    inDir = cdx > 1 && java.lang.Math.abs(cdy) < (float)(java.lang.Math.abs(cdx) * 2);
                }
                if (inDir)
                {
                    float distSq = (float)((float)(cdx * cdx) + (float)(cdy * cdy));
                    if (distSq < bestScore)
                    {
                        bestScore = distSq;
                        best = candidate;
                    }
                }
            }
            if (best != null)
            {
                best.setFocus();
            }
        }
        // Java: 383
        public static void closeCampaign(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUI gui)
        {
            com.oddlabs.tt.render.Renderer.startMenu(network, gui);
        }
        // Java: 387
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            renderer.drawIcon(campaign.getIcons().getMap(), 0.0f, 0.0f, mapColor);
        }
        // Java: 392
        public override void doAdd()
        {
            base.doAdd();
            com.oddlabs.tt.@event.LocalEventQueue.getQueue().getManager().registerAnimation(this);
        }
        // Java: 398
        public override void doRemove()
        {
            base.doRemove();
            com.oddlabs.tt.@event.LocalEventQueue.getQueue().getManager().removeAnimation(this);
        }
        // Java: 404
        public void animate(float t)
        {
            float flicker = 0.9f;
            mapColor.set(flicker, flicker, flicker, 1.0f);
        }
        // Java: 410
        public override void render(com.oddlabs.tt.render.GUIRenderer renderer, float clip_left, float clip_right, float clip_bottom, float clip_top)
        {
            renderer.getMatrixStack().push();
            renderer.getMatrixStack().scale(scale_x, scale_y, 1.0f);
            base.render(renderer, clip_left, clip_right, clip_bottom, clip_top);
            renderer.getMatrixStack().pop();
        }
        // Java: 419
        public override com.oddlabs.tt.gui.Renderable pick(float x, float y)
        {
            return ((com.oddlabs.tt.gui.GUIObject)base.pick((float)(x / scale_x), (float)(y / scale_y)));
        }
        static CampaignMapForm()
        {
            CampaignMapForm.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.@delegate.CampaignMapForm).getName());
        }
        private void init_CampaignMapForm()
        {
            this.islandButtons = new java.util.ArrayList();
            this.initialFocusSet = false;
            this.mapColor = new org.joml.Vector4f(1.0f, 1.0f, 1.0f, 1.0f);
        }
        public CampaignMapForm(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.player.campaign.Campaign campaign) : base(gui_root, new com.oddlabs.tt.camera.StaticCamera(new com.oddlabs.tt.camera.CameraState()))
        {
            init_CampaignMapForm();
            this.campaign = campaign;
            this.network = network;
            this.scale_x = (float)(gui_root.getWidth() / com.oddlabs.tt.@delegate.CampaignMapForm.BASE_WIDTH);
            this.scale_y = (float)(gui_root.getHeight() / com.oddlabs.tt.@delegate.CampaignMapForm.BASE_HEIGHT);
            if (campaign.getState().isSaga() && (campaign is com.oddlabs.tt.player.campaign.SagaCampaign saga))
            {
                string news = saga.chapterNews();
                int shown = saga.getPage();
                if ((campaign.getIcons() is com.oddlabs.tt.gui.SagaCampaignIcons icons))
                {
                    for (int i = 0; i < icons.getNumRoutes(); i++)
                    {
                        int[] r = icons.getRouteEnds(i);
                        if (campaign.getState().getIslandState(r[0]) != com.oddlabs.tt.player.campaign.CampaignState.ISLAND_HIDDEN && campaign.getState().getIslandState(r[1]) != com.oddlabs.tt.player.campaign.CampaignState.ISLAND_HIDDEN)
                        {
                            addChild(icons.getRoute(i));
                        }
                    }
                }
                int tab_x = 330;
                for (int p = 0; p < com.oddlabs.tt.player.campaign.SagaCampaign.NUM_PAGES; p++)
                {
                    if (!saga.pageOpen(p) && p != shown)
                    {
                        continue;
                    }
                    int page_index = p;
                    com.oddlabs.tt.gui.HorizButton tab = new com.oddlabs.tt.gui.HorizButton(saga.pageName(p), 150);
                    tab.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
                    {
                        if (page_index == saga.getPage())
                        {
                            return;
                        }
                        saga.setPage(page_index);
                        gui_root.removeDelegate(this);
                        gui_root.pushDelegate(new com.oddlabs.tt.@delegate.CampaignMapForm(network, gui_root, campaign));
                    }));
                    addChild(tab);
                    tab.setPos(tab_x, 560);
                    if (p == shown)
                    {
                        tab.setDisabled(true);
                    }
                    tab_x += 154;
                }
                if (news != null)
                {
                    gui_root.addModalForm(new com.oddlabs.tt.form.CampaignDialogForm(i18n("saga_header"), news, campaign.getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START, new java.lang.Runnable_L(() =>
                    {
                    })));
                }
            }
            else
            {
                switch ((campaign.getState().getRace()))
                {
                    case 0:
                    {
                        if (campaign.getState().getIslandState(10) != com.oddlabs.tt.player.campaign.CampaignState.ISLAND_HIDDEN)
                        {
                            addChild(campaign.getIcons().getHiddenRoutes()[0]);
                            addChild(campaign.getIcons().getHiddenRoutes()[1]);
                        }
                        if (campaign.getState().getCurrentIsland() == 14)
                        {
                            java.lang.Runnable runnable_menu = new java.lang.Runnable_L(() =>
                            {
                                com.oddlabs.tt.@delegate.CampaignMapForm.closeCampaign(network, gui_root.getGUI());
                            });
                            java.lang.Runnable runnable_next = new java.lang.Runnable_L(() =>
                            {
                                com.oddlabs.tt.form.CampaignDialogForm dialog = new com.oddlabs.tt.form.CampaignDialogForm(i18n("native_campaign_opened_header"), i18n("native_campaign_opened"), null, com.oddlabs.tt.gui.Origin.AT_START, runnable_menu);
                                gui_root.addModalForm(dialog);
                            });
                            com.oddlabs.tt.form.CampaignDialogForm dialog_2 = new com.oddlabs.tt.form.CampaignDialogForm(i18n("viking_header"), i18n("viking_campaign_completed"), campaign.getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START, runnable_next);
                            if (campaign.getState().getDifficulty() != com.oddlabs.tt.player.campaign.CampaignState.DIFFICULTY_EASY)
                            {
                                com.oddlabs.tt.steam.SteamManager.unlockAchievement(com.oddlabs.tt.steam.SteamAchievementNames.COMPLETE_VIKING_CAMPAIGN_NORMAL);
                            }
                            if (campaign.getState().getDifficulty() == com.oddlabs.tt.player.campaign.CampaignState.DIFFICULTY_HARD)
                            {
                                com.oddlabs.tt.steam.SteamManager.unlockAchievement(com.oddlabs.tt.steam.SteamAchievementNames.COMPLETE_VIKING_CAMPAIGN_HARD);
                            }
                            gui_root.addModalForm(dialog_2);
                            com.oddlabs.tt.global.Settings.getSettings().has_native_campaign = true;
                        }
                        break;
                    }
                    case 1:
                    {
                        if (campaign.getState().getIslandState(7) != com.oddlabs.tt.player.campaign.CampaignState.ISLAND_HIDDEN)
                        {
                            addChild(campaign.getIcons().getHiddenRoutes()[0]);
                        }
                        if (campaign.getState().getCurrentIsland() == 7)
                        {
                            java.lang.Runnable runnable = new java.lang.Runnable_L(() =>
                            {
                                com.oddlabs.tt.@delegate.CampaignMapForm.closeCampaign(network, gui_root.getGUI());
                            });
                            com.oddlabs.tt.form.CampaignDialogForm dialog_3 = new com.oddlabs.tt.form.CampaignDialogForm(i18n("native_header"), i18n("native_campaign_completed"), campaign.getIcons().getFaces()[0], com.oddlabs.tt.gui.Origin.AT_START, runnable);
                            if (campaign.getState().getDifficulty() != com.oddlabs.tt.player.campaign.CampaignState.DIFFICULTY_EASY)
                            {
                                com.oddlabs.tt.steam.SteamManager.unlockAchievement(com.oddlabs.tt.steam.SteamAchievementNames.COMPLETE_NATIVE_CAMPAIGN_NORMAL);
                            }
                            if (campaign.getState().getDifficulty() == com.oddlabs.tt.player.campaign.CampaignState.DIFFICULTY_HARD)
                            {
                                com.oddlabs.tt.steam.SteamManager.unlockAchievement(com.oddlabs.tt.steam.SteamAchievementNames.COMPLETE_NATIVE_CAMPAIGN_HARD);
                            }
                            gui_root.addModalForm(dialog_3);
                        }
                        break;
                    }
                }
            }
            for (int i_2 = 0; i_2 < campaign.getIcons().getNumIslands(); i_2++)
            {
                com.oddlabs.tt.gui.MapIslandData data = campaign.getIcons().getMapIslandData(i_2);
                if (data == null)
                {
                    continue;
                }
                int state = campaign.getState().getIslandState(i_2);
                if (campaign.getState().isSaga() && (campaign.getIcons() is com.oddlabs.tt.gui.SagaCampaignIcons saga_icons) && (state == com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE || state == com.oddlabs.tt.player.campaign.CampaignState.ISLAND_SEMI_AVAILABLE || state == com.oddlabs.tt.player.campaign.CampaignState.ISLAND_COMPLETED && campaign.getState().isAllyIsle(i_2)))
                {
                    bool ally = state == com.oddlabs.tt.player.campaign.CampaignState.ISLAND_COMPLETED;
                    com.oddlabs.tt.gui.GUIIcon pin = new com.oddlabs.tt.gui.GUIIcon(saga_icons.getPin((ally ? 2 : 1)));
                    pin.setPos(data.pinX(), data.pinY());
                    if (ally)
                    {
                        int index = i_2;
                        com.oddlabs.tt.gui.MapIslandButton button = new com.oddlabs.tt.gui.MapIslandButton(data.button(), index);
                        button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_5, int unused_6, int unused_7, int unused_8) =>
                        {
                            campaign.islandChosen(network, getGUIRoot(), index);
                        }));
                        addChild(button);
                        islandButtons.add(button);
                        button.setPos(data.x(), data.y());
                        addChild(pin);
                        continue;
                    }
                    if (state == com.oddlabs.tt.player.campaign.CampaignState.ISLAND_AVAILABLE)
                    {
                        int index_2 = i_2;
                        com.oddlabs.tt.gui.MapIslandButton button_2 = new com.oddlabs.tt.gui.MapIslandButton(data.button(), index_2);
                        button_2.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_9, int unused_10, int unused_11, int unused_12) =>
                        {
                            campaign.islandChosen(network, getGUIRoot(), index_2);
                        }));
                        addChild(button_2);
                        islandButtons.add(button_2);
                        if (campaign.getState().getCurrentIsland() == i_2)
                        {
                            button_2.setFocus();
                        }
                        button_2.setPos(data.x(), data.y());
                    }
                    else
                    {
                        com.oddlabs.tt.gui.GUIObject icon = new com.oddlabs.tt.gui.GUIIcon(data.button().quad(com.oddlabs.tt.gui.ModeIconQuads.Mode.DISABLED));
                        addChild(icon);
                        icon.setPos(data.x(), data.y());
                    }
                    addChild(pin);
                    continue;
                }
                if (campaign.getState().isSaga() && state == com.oddlabs.tt.player.campaign.CampaignState.ISLAND_COMPLETED)
                {
                    int index_3 = i_2;
                    com.oddlabs.tt.gui.MapIslandButton button_3 = new com.oddlabs.tt.gui.MapIslandButton(data.button(), index_3);
                    button_3.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_13, int unused_14, int unused_15, int unused_16) =>
                    {
                        campaign.islandChosen(network, getGUIRoot(), index_3);
                    }));
                    addChild(button_3);
                    islandButtons.add(button_3);
                    button_3.setPos(data.x(), data.y());
                    com.oddlabs.tt.gui.GUIIcon pin_2 = new com.oddlabs.tt.gui.GUIIcon((campaign.getState().getCurrentIsland() == i_2 ? data.boat() : data.flag()));
                    pin_2.setPos(data.pinX(), data.pinY());
                    addChild(pin_2);
                    if (campaign.getState().getCurrentIsland() == i_2)
                    {
                        button_3.setFocus();
                    }
                    continue;
                }
                com.oddlabs.tt.gui.GUIObject island = ((System.Func<com.oddlabs.tt.gui.GUIObject>)(() =>
                {
                    switch ((state))
                    {
                        case 1:
                        {
                            int index_4 = i_2;
                            com.oddlabs.tt.gui.MapIslandButton button_4 = new com.oddlabs.tt.gui.MapIslandButton(data.button(), index_4);
                            button_4.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_17, int unused_18, int unused_19, int unused_20) =>
                            {
                                campaign.islandChosen(network, getGUIRoot(), index_4);
                            }));
                            addChild(button_4);
                            islandButtons.add(button_4);
                            if (campaign.getState().getCurrentIsland() == i_2)
                            {
                                button_4.setFocus();
                            }
                            return button_4;
                        }
                        case 4:
                        case 2:
                        {
                            com.oddlabs.tt.gui.GUIObject icon_2 = new com.oddlabs.tt.gui.GUIIcon(data.button().quad(com.oddlabs.tt.gui.ModeIconQuads.Mode.DISABLED));
                            addChild(icon_2);
                            return icon_2;
                        }
                        case 3:
                        {
                            com.oddlabs.tt.gui.GUIObject icon_3 = new com.oddlabs.tt.gui.GUIIcon(data.button().quad(com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL));
                            addChild(icon_3);
                            if (campaign.getState().getCurrentIsland() != i_2)
                            {
                                com.oddlabs.tt.gui.GUIIcon flag = new com.oddlabs.tt.gui.GUIIcon(data.flag());
                                flag.setPos(data.pinX(), data.pinY());
                                addChild(flag);
                            }
                            else
                            {
                                com.oddlabs.tt.gui.GUIIcon boat = new com.oddlabs.tt.gui.GUIIcon(data.boat());
                                boat.setPos(data.pinX(), data.pinY());
                                addChild(boat);
                            }
                            return icon_3;
                        }
                        case 5:
                        {
                            return null;
                        }
                        default:
                        {
                            throw new System.ArgumentException("Unexpected island state: " + (state));
                        }
                    }
                    throw new System.InvalidOperationException("MatchException");
                }))();
                if (island != null)
                {
                    island.setPos(data.x(), data.y());
                }
            }
            setFocus();
            if ((campaign is com.oddlabs.tt.player.campaign.SagaCampaign saga_2))
            {
                saga_2.onChart(network, gui_root);
            }
        }
        // mixin of default method Animated.updateChecksum
        public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
        }
    }
}
