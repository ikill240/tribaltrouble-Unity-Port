// Translated from GUIRoot.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 35
    public class GUIRoot : com.oddlabs.tt.gui.GUIObject
    {
        public static java.util.logging.Logger logger;
        public static java.util.ResourceBundle bundle;
        // Java: 39
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.gui.GUIRoot.bundle, key, args);
        }
        public const int CURSOR_OFFSET_Y = 27;
        public const float MIN_PROJECTED_W = 0.1f;
        public java.util.Deque delegate_stack;
        public java.util.Deque modal_delegate_stack;
        public java.util.Deque focus_backup_stack;
        public com.oddlabs.tt.animation.TimerAnimation tool_tip_timer;
        public com.oddlabs.tt.gui.GUI gui;
        public com.oddlabs.tt.gui.ToolTipBox tool_tip;
        public com.oddlabs.tt.gui.InfoPrinter info_printer;
        public com.oddlabs.tt.form.Status status;
        public com.oddlabs.tt.gui.InputState input_state;
        public bool render_tool_tip;
        public com.oddlabs.tt.gui.GUIObject current_gui_object;
        public com.oddlabs.tt.gui.GUIObject global_focus;
        public float effective_scale;
        // Java: 80
        public override com.oddlabs.util.ListElementImpl self()
        {
            return this;
        }
        // Java: 84
        public override float getGlobalScale()
        {
            return effective_scale;
        }
        // Java: 88
        public override com.oddlabs.tt.gui.GUIRoot getParentGUIRoot()
        {
            return ((com.oddlabs.tt.gui.GUIRoot)self());
        }
        // Java: 92
        public com.oddlabs.tt.gui.GUI getGUI()
        {
            return gui;
        }
        // Java: 96
        public com.oddlabs.tt.gui.InputState getInputState()
        {
            return input_state;
        }
        // Java: 103
        public com.oddlabs.tt.gui.GUIObject getGlobalFocus()
        {
            return global_focus;
        }
        // Java: 111
        public void setGlobalFocus(com.oddlabs.tt.gui.GUIObject @object)
        {
            global_focus = @object;
        }
        // Java: 115
        public void setToolTipTimer()
        {
            tool_tip_timer.setTimerInterval((float)(com.oddlabs.tt.global.Settings.getSettings().tooltip_delay * com.oddlabs.tt.gui.ToolTipBox.MAX_DELAY_SECONDS));
        }
        // Java: 119
        public void timerUpdate(com.oddlabs.tt.animation.TimerAnimation anim)
        {
            render_tool_tip = true;
        }
        // Java: 123
        public void stopToolTip()
        {
            render_tool_tip = false;
            tool_tip_timer.stop();
        }
        // Java: 128
        public com.oddlabs.tt.gui.InfoPrinter getInfoPrinter()
        {
            return info_printer;
        }
        // Java: 132
        public void pushDelegate(com.oddlabs.tt.@delegate.CameraDelegate @delegate)
        {
            if (!delegate_stack.isEmpty())
            {
                getDelegate().remove();
            }
            delegate_stack.push(@delegate);
            addChild(@delegate);
            mousePick();
        }
        // Java: 142
        public void removeDelegate(com.oddlabs.tt.@delegate.CameraDelegate @delegate)
        {
            bool top_most = ((object)getDelegate() == (object)@delegate);
            @delegate.remove();
            delegate_stack.remove(@delegate);
            if (delegate_stack.isEmpty())
            {
                pushDelegate(new com.oddlabs.tt.@delegate.NullDelegate(this, false));
            }
            else if (top_most)
            {
                addChild(getDelegate());
            }
            mousePick();
        }
        // Java: 156
        public com.oddlabs.tt.@delegate.CameraDelegate getDelegate()
        {
            return ((com.oddlabs.tt.@delegate.CameraDelegate)delegate_stack.element());
        }
        // Java: 160
        public void pushModalDelegate(com.oddlabs.tt.@delegate.ModalDelegate @delegate)
        {
            com.oddlabs.tt.@delegate.ModalDelegate old = getModalDelegate();
            modal_delegate_stack.push(@delegate);
            if (old != null)
            {
                old.remove();
            }
            base.addChild(@delegate);
            mousePick();
        }
        // Java: 170
        public void popModalDelegate(com.oddlabs.tt.@delegate.ModalDelegate @delegate)
        {
            if (!modal_delegate_stack.contains(@delegate))
            {
                return;
            }
            bool top_most = ((object)getModalDelegate() == (object)@delegate);
            modal_delegate_stack.remove(@delegate);
            @delegate.remove();
            com.oddlabs.tt.@delegate.ModalDelegate modal_delegate = getModalDelegate();
            if (top_most && modal_delegate != null)
            {
                base.addChild(modal_delegate);
            }
            com.oddlabs.tt.gui.GUIObject @object = ((com.oddlabs.tt.gui.GUIObject)focus_backup_stack.pop());
            if (top_most)
            {
                if (!delegate_stack.isEmpty())
                {
                    getDelegate().setFocus();
                }
                if (@object != null)
                {
                    @object.setFocus();
                }
            }
            else
            {
                if (modal_delegate != null)
                {
                    com.oddlabs.tt.gui.GUIObject focused = modal_delegate.getFocusedChild();
                    if (focused != null)
                    {
                        focused.restoreFocus();
                    }
                    else
                    {
                        com.oddlabs.tt.gui.GUIObject child = ((com.oddlabs.tt.gui.GUIObject)modal_delegate.getFirstChild());
                        ((com.oddlabs.tt.gui.GUIObject)java.util.Objects.requireNonNullElse(child, modal_delegate)).setFocus();
                    }
                }
            }
            mousePick();
        }
        // Java: 202
        public com.oddlabs.tt.@delegate.ModalDelegate getModalDelegate()
        {
            return ((com.oddlabs.tt.@delegate.ModalDelegate)modal_delegate_stack.peek());
        }
        // Java: 206
        public bool isShowingQuitForm()
        {
            com.oddlabs.tt.@delegate.ModalDelegate modal = getModalDelegate();
            if (modal == null)
            {
                return false;
            }
            com.oddlabs.tt.gui.GUIObject child = ((com.oddlabs.tt.gui.GUIObject)modal.getFirstChild());
            while (child != null)
            {
                if ((child is com.oddlabs.tt.form.QuitForm))
                {
                    return true;
                }
                child = ((com.oddlabs.tt.gui.GUIObject)child.getNext());
            }
            return false;
        }
        // Java: 219
        public bool isCapturingKeyBinding()
        {
            com.oddlabs.tt.@delegate.ModalDelegate modal = getModalDelegate();
            if (modal == null)
            {
                return false;
            }
            com.oddlabs.tt.gui.GUIObject child = ((com.oddlabs.tt.gui.GUIObject)modal.getFirstChild());
            while (child != null)
            {
                if ((child is com.oddlabs.tt.form.KeyBindingDialog))
                {
                    return true;
                }
                child = ((com.oddlabs.tt.gui.GUIObject)child.getNext());
            }
            return false;
        }
        // Java: 230
        public void addModalForm(com.oddlabs.tt.gui.Form form)
        {
            focus_backup_stack.push(global_focus);
            com.oddlabs.tt.@delegate.ModalDelegate @delegate = new com.oddlabs.tt.@delegate.ModalDelegate();
            @delegate.addChild(form);
            form.addCloseListener(new com.oddlabs.tt.guievent.CloseListener_L(() =>
            {
                popModalDelegate(@delegate);
            }));
            pushModalDelegate(@delegate);
            form.setFocus();
        }
        // Java: 239
        public void swapFocusBackup(com.oddlabs.tt.gui.GUIObject o)
        {
            focus_backup_stack.pop();
            focus_backup_stack.push(o);
        }
        // Java: 244
        public static float calculateMaxScale(int width, int height)
        {
            if (width <= 0 || height <= 0)
            {
                return 1.0f;
            }
            float maxScaleX = (float)(width / 800.0f);
            float maxScaleY = (float)(height / 600.0f);
            return java.lang.Math.max(1.0f, java.lang.Math.min(maxScaleX, maxScaleY));
        }
        // Java: 251
        public static float calculateMinScale(int width, int height)
        {
            if (width <= 0 || height <= 0)
            {
                return 1.0f;
            }
            float autoScale = java.lang.Math.min((float)(width / 1280.0f), (float)(height / 1024.0f));
            return java.lang.Math.max(1.0f, autoScale);
        }
        // Java: 257
        public static float calculateEffectiveScale(int width, int height)
        {
            if (width <= 0 || height <= 0)
            {
                return 1.0f;
            }
            float minScale = com.oddlabs.tt.gui.GUIRoot.calculateMinScale(width, height);
            float maxAllowedScale = java.lang.Math.max(minScale, com.oddlabs.tt.gui.GUIRoot.calculateMaxScale(width, height));
            float current = java.lang.Math.clamp(com.oddlabs.tt.global.Settings.getSettings().ui_scale, 0.0f, 1.0f);
            float rawTarget = (float)(minScale + ((float)(current * ((float)(maxAllowedScale - minScale)))));
            float snappedTarget = (float)(java.lang.Math.round((float)(rawTarget * 4.0f)) / 4.0f);
            return java.lang.Math.clamp(snappedTarget, minScale, maxAllowedScale);
        }
        // Java: 274
        public override void displayChangedNotify(int width, int height)
        {
            if (width <= 0 || height <= 0)
            {
                return;
            }
            effective_scale = com.oddlabs.tt.gui.GUIRoot.calculateEffectiveScale(width, height);
            int virtualWidth = java.lang.JC.f2i(((float)(width / effective_scale)));
            int virtualHeight = java.lang.JC.f2i(((float)(height / effective_scale)));
            setDim(virtualWidth, virtualHeight);
        }
        // Java: 286
        public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
            if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.PRESSED))
            {
                bool consumed = false;
                bool textFieldFocused = (current_gui_object is com.oddlabs.tt.gui.TextField) || (global_focus is com.oddlabs.tt.gui.TextField);
                if (@event.hasActions() && !textFieldFocused)
                {
                    if (@event.consumeAction(com.oddlabs.tt.input.GameAction.GLOBAL_SCREENSHOT))
                    {
                        string filename = com.oddlabs.tt.util.GLUtils.takeScreenshot("");
                        info_printer.print(i18n("screenshot_message", filename));
                        consumed = true;
                    }
                    if (@event.consumeAction(com.oddlabs.tt.input.GameAction.GLOBAL_AGGRESSIVE_UNITS))
                    {
                        com.oddlabs.tt.global.Settings.getSettings().aggressive_units = !com.oddlabs.tt.global.Settings.getSettings().aggressive_units;
                        info_printer.print(i18n((com.oddlabs.tt.global.Settings.getSettings().aggressive_units ? "aggressive_unites_on" : "aggressive_unites_off")));
                        consumed = true;
                    }
                    if (@event.consumeAction(com.oddlabs.tt.input.GameAction.GLOBAL_TOGGLE_STATUS))
                    {
                        com.oddlabs.tt.global.Globals.draw_status = !com.oddlabs.tt.global.Globals.draw_status;
                        consumed = true;
                    }
                    if (@event.consumeAction(com.oddlabs.tt.input.GameAction.GLOBAL_TOGGLE_FULLSCREEN))
                    {
                        com.oddlabs.tt.render.Renderer.getRenderer().toggleFullscreen();
                        consumed = true;
                    }
                    if (com.oddlabs.tt.global.Settings.getSettings().inDeveloperMode())
                    {
                        if (@event.consumeAction(com.oddlabs.tt.input.GameAction.DEBUG_TOGGLE_LIGHT))
                        {
                            com.oddlabs.tt.global.Globals.draw_light = !com.oddlabs.tt.global.Globals.draw_light;
                            consumed = true;
                        }
                        if (@event.consumeAction(com.oddlabs.tt.input.GameAction.DEBUG_TOGGLE_PLANTS))
                        {
                            com.oddlabs.tt.global.Globals.draw_plants = !com.oddlabs.tt.global.Globals.draw_plants;
                            consumed = true;
                        }
                        if (@event.consumeAction(com.oddlabs.tt.input.GameAction.DEBUG_TOGGLE_PARTICLES))
                        {
                            com.oddlabs.tt.global.Globals.draw_particles = !com.oddlabs.tt.global.Globals.draw_particles;
                            consumed = true;
                        }
                        if (@event.consumeAction(com.oddlabs.tt.input.GameAction.DEBUG_TOGGLE_AXES))
                        {
                            com.oddlabs.tt.global.Globals.draw_axes = !com.oddlabs.tt.global.Globals.draw_axes;
                            consumed = true;
                        }
                        if (@event.consumeAction(com.oddlabs.tt.input.GameAction.DEBUG_TOGGLE_MISC))
                        {
                            com.oddlabs.tt.global.Globals.draw_misc = !com.oddlabs.tt.global.Globals.draw_misc;
                            consumed = true;
                        }
                        if (@event.consumeAction(com.oddlabs.tt.input.GameAction.DEBUG_PROCESS_MISC))
                        {
                            com.oddlabs.tt.gui.GUIRoot.logger.info("WARNING: DEBUG_PROCESS_MISC triggered!");
                            com.oddlabs.tt.global.Globals.process_misc = !com.oddlabs.tt.global.Globals.process_misc;
                            consumed = true;
                        }
                        if (@event.consumeAction(com.oddlabs.tt.input.GameAction.DEBUG_RESET_CURSOR))
                        {
                            com.oddlabs.tt.render.Renderer.getLocalInput().getInputProvider().setCursorPosition(10, 10);
                            consumed = true;
                        }
                        if (@event.consumeAction(com.oddlabs.tt.input.GameAction.DEBUG_TOGGLE_DETAIL))
                        {
                            com.oddlabs.tt.global.Globals.draw_detail = !com.oddlabs.tt.global.Globals.draw_detail;
                            consumed = true;
                        }
                        if (@event.consumeAction(com.oddlabs.tt.input.GameAction.DEBUG_CRASH))
                        {
                            com.oddlabs.tt.gui.GUIRoot.logger.info("crash!");
                            throw new System.Exception("Debug crash action triggered.");
                        }
                        if (@event.consumeAction(com.oddlabs.tt.input.GameAction.DEBUG_TOGGLE_FRAME_BUFFER))
                        {
                            com.oddlabs.tt.global.Globals.clear_frame_buffer = !com.oddlabs.tt.global.Globals.clear_frame_buffer;
                            consumed = true;
                        }
                        if (@event.consumeAction(com.oddlabs.tt.input.GameAction.DEBUG_TOGGLE_BOUNDING))
                        {
                            com.oddlabs.tt.global.Globals.switchBoundingMode();
                            consumed = true;
                        }
                        if (@event.consumeAction(com.oddlabs.tt.input.GameAction.DEBUG_TOGGLE_FRUSTUM_FREEZE))
                        {
                            com.oddlabs.tt.global.Globals.frustum_freeze = !com.oddlabs.tt.global.Globals.frustum_freeze;
                            com.oddlabs.tt.gui.GUIRoot.logger.info("Globals.frustum_freeze = " + java.lang.JStr.valueOf(com.oddlabs.tt.global.Globals.frustum_freeze));
                            consumed = true;
                        }
                        if (@event.consumeAction(com.oddlabs.tt.input.GameAction.DEBUG_FORCE_GC))
                        {
                            com.oddlabs.tt.gui.GUIRoot.logger.info("GC Forced");
                            java.lang.JSystem.gc();
                            consumed = true;
                        }
                        if (@event.consumeAction(com.oddlabs.tt.input.GameAction.DEBUG_START_RECORDING))
                        {
                            com.oddlabs.tt.render.Renderer.getRenderer().startMovieRecording();
                            consumed = true;
                        }
                        if (@event.consumeAction(com.oddlabs.tt.input.GameAction.DEBUG_TOGGLE_WATER))
                        {
                            com.oddlabs.tt.global.Globals.draw_water = !com.oddlabs.tt.global.Globals.draw_water;
                            consumed = true;
                        }
                        if (@event.consumeAction(com.oddlabs.tt.input.GameAction.DEBUG_TOGGLE_AI))
                        {
                            com.oddlabs.tt.global.Globals.run_ai = !com.oddlabs.tt.global.Globals.run_ai;
                            com.oddlabs.tt.gui.GUIRoot.logger.info("Globals.run_ai = " + java.lang.JStr.valueOf(com.oddlabs.tt.global.Globals.run_ai));
                            consumed = true;
                        }
                        if (@event.consumeAction(com.oddlabs.tt.input.GameAction.DEBUG_DUMP_ANIMATIONS))
                        {
                            com.oddlabs.tt.gui.GUIRoot.logger.info("*********************************************************");
                            com.oddlabs.tt.@event.LocalEventQueue.getQueue().debugPrintAnimations();
                            com.oddlabs.tt.gui.GUIRoot.logger.info("Texture.globalSize() = " + (com.oddlabs.tt.render.Texture.globalSize()));
                            consumed = true;
                        }
                    }
                }
                if (consumed)
                {
                    @event.consume();
                    return;
                }
            }
            base.handleInput(@event);
        }
        // Java: 406
        public void mousePick()
        {
            mousePick(java.lang.Math.round((float)(com.oddlabs.tt.render.Renderer.getLocalInput().getMouseX() / effective_scale)), java.lang.Math.round((float)(com.oddlabs.tt.render.Renderer.getLocalInput().getMouseY() / effective_scale)));
        }
        // Java: 411
        public void mousePick(int x, int y)
        {
            com.oddlabs.tt.gui.GUIObject target = ((com.oddlabs.tt.gui.GUIObject)pick(x, y));
            if (target != null && ((object)target != (object)current_gui_object))
            {
                current_gui_object.mouseExitedAll();
                tool_tip_timer.resetTime();
                bool old_tip = current_gui_object.hasToolTip();
                bool new_tip = target.hasToolTip();
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
                current_gui_object = target;
                current_gui_object.mouseEnteredAll();
                if (!current_gui_object.isDisabled())
                {
                    com.oddlabs.tt.render.Renderer.getLocalInput().getPointerInput().setActiveCursor(current_gui_object.getCursorType());
                }
            }
        }
        // Java: 436
        public org.joml.Vector4f projectToScreen(float x, float y, float z, org.joml.Vector4f dest)
        {
            dest.set(x, y, z, 1.0f);
            getDelegate().getCamera().getState().getProjectionModelView().transform(dest, dest);
            float w = dest.w_f;
            float inv_w = (float)(1.0f / java.lang.Math.max(w, com.oddlabs.tt.gui.GUIRoot.MIN_PROJECTED_W));
            dest.set((float)((float)(((float)((float)(dest.x_f * inv_w) + 1.0f)) * 0.5f) * getWidth()), (float)((float)(((float)((float)(dest.y_f * inv_w) + 1.0f)) * 0.5f) * getHeight()), 0.0f, w);
            return dest;
        }
        // Java: 445
        public com.oddlabs.tt.gui.GUIObject getCurrentGUIObject()
        {
            return current_gui_object;
        }
        // Java: 450
        public override void addChild(com.oddlabs.tt.gui.Renderable child_p)
        {
            com.oddlabs.tt.gui.GUIObject child = (com.oddlabs.tt.gui.GUIObject)child_p;
            base.addChild(child);
            com.oddlabs.tt.@delegate.ModalDelegate modal_delegate = getModalDelegate();
            putFirst(info_printer);
            if (modal_delegate != null)
            {
                base.addChild(modal_delegate);
            }
        }
        // Java: 460
        public bool showToolTip()
        {
            return getModalDelegate() != null || getDelegate().renderCursor();
        }
        // Java: 464
        public void renderTopmost(com.oddlabs.tt.render.GUIRenderer renderer, com.oddlabs.tt.gui.ToolTip hovered, bool cheater)
        {
            if (cheater)
            {
                renderer.drawIcon(com.oddlabs.tt.gui.GUIIcons.getIcons().getCheatIcon(), getWidth() - com.oddlabs.tt.gui.GUIIcons.getIcons().getCheatIcon().getWidth() - 10, 5);
            }
            getDelegate().render2D(renderer);
            bool initial = true;
            foreach (com.oddlabs.tt.@delegate.CameraDelegate @delegate in java.lang.JIter.of(delegate_stack))
            {
                if (initial)
                {
                    initial = false;
                }
                else if (@delegate.forceRender())
                {
                    @delegate.render(renderer);
                }
            }
            if (com.oddlabs.tt.global.Globals.draw_status)
            {
                status.render(renderer);
            }
            if (gui.getFade() != null)
            {
                gui.getFade().render(renderer);
            }
            if (showToolTip())
            {
                com.oddlabs.tt.gui.ToolTip tooltip = getToolTip();
                if (tooltip == null)
                {
                    tooltip = hovered;
                }
                if (tooltip != null)
                {
                    renderToolTip(renderer, tooltip);
                }
            }
        }
        // Java: 500
        public org.joml.Matrix4f multProjection(org.joml.Matrix4f matrix)
        {
            float fovy = com.oddlabs.tt.global.Globals.FOV;
            float zNear = com.oddlabs.tt.global.Globals.VIEW_MIN;
            float zFar = com.oddlabs.tt.global.Globals.VIEW_MAX;
            org.joml.Matrix4f perspectiveMatrix = new org.joml.Matrix4f().perspective(((float)(java.lang.Math.toRadians(fovy))), (float)(((float)(getWidth())) / getHeight()), zNear, zFar);
            return matrix.mul(perspectiveMatrix);
        }
        // Java: 510
        public com.oddlabs.tt.gui.ToolTip getToolTip()
        {
            com.oddlabs.tt.gui.GUIObject obj = getCurrentGUIObject();
            return (render_tool_tip && obj.hasToolTip() ? (com.oddlabs.tt.gui.ToolTip)(obj) : (com.oddlabs.tt.gui.ToolTip)(null));
        }
        // Java: 515
        public void renderToolTip(com.oddlabs.tt.render.GUIRenderer renderer, com.oddlabs.tt.gui.ToolTip hovered)
        {
            tool_tip.clear();
            hovered.appendToolTip(tool_tip);
            tool_tip.render(renderer, java.lang.Math.round((float)(com.oddlabs.tt.render.Renderer.getLocalInput().getMouseX() / effective_scale)), java.lang.Math.round((float)(com.oddlabs.tt.render.Renderer.getLocalInput().getMouseY() / effective_scale)) - com.oddlabs.tt.gui.GUIRoot.CURSOR_OFFSET_Y, getWidth(), getHeight());
        }
        static GUIRoot()
        {
            GUIRoot.logger = java.util.logging.Logger.getLogger(typeof(com.oddlabs.tt.gui.GUIRoot).getName());
            GUIRoot.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.gui.GUIRoot).getName());
        }
        private void init_GUIRoot()
        {
            this.delegate_stack = new java.util.ArrayDeque();
            this.modal_delegate_stack = new java.util.ArrayDeque();
            this.focus_backup_stack = new java.util.ArrayDeque();
            this.tool_tip_timer = new com.oddlabs.tt.animation.TimerAnimation(new com.oddlabs.tt.animation.Updatable_L((object a0) => { this.timerUpdate(((com.oddlabs.tt.animation.TimerAnimation)a0)); }), 0);
            this.tool_tip = new com.oddlabs.tt.gui.ToolTipBox();
            this.info_printer = new com.oddlabs.tt.gui.InfoPrinter(this, 4, com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            this.status = new com.oddlabs.tt.form.Status();
            this.input_state = new com.oddlabs.tt.gui.InputState(this);
            this.render_tool_tip = false;
            this.current_gui_object = this;
            this.global_focus = this;
            this.effective_scale = 1.0f;
        }
        public GUIRoot(com.oddlabs.tt.gui.GUI gui) : base()
        {
            init_GUIRoot();
            this.gui = gui;
            setPos(0, 0);
            setCanFocus(true);
            setFocusCycle(true);
            setToolTipTimer();
            addChild(info_printer);
            info_printer.setPos(0, 0);
            pushDelegate(new com.oddlabs.tt.@delegate.NullDelegate(this, false));
            com.oddlabs.tt.render.Renderer.getLocalInput().getPointerInput().setActiveCursor(current_gui_object.getCursorType());
        }
    }
}
