// Translated from GUIObject.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 24
    public abstract class GUIObject : com.oddlabs.tt.gui.Renderable, com.oddlabs.tt.gui.ToolTip
    {
        public static java.util.logging.Logger logger;
        public const int ROW_TOLERANCE = 5;
        public bool disabled;
        public bool hovered;
        public bool active;
        public bool focus_cycle;
        public bool can_focus;
        public bool tab_stop;
        public com.oddlabs.tt.gui.GUIObject focused_child;
        // Java: 56
        public void setTabStop(bool tab_stop)
        {
            this.tab_stop = tab_stop;
        }
        // Java: 60
        public bool isTabStop()
        {
            return tab_stop;
        }
        public com.oddlabs.tt.gui.GUIObject next_hover;
        public java.util.Set listeners;
        public bool placed;
        public com.oddlabs.tt.gui.Origin origin;
        public java.util.function.Supplier tool_tip;
        public string cached_tool_tip;
        // Java: 88
        public override com.oddlabs.util.ListElementImpl self()
        {
            return this;
        }
        // Java: 93
        public virtual com.oddlabs.tt.gui.GUIRoot getParentGUIRoot()
        {
            return (null != ((com.oddlabs.tt.gui.GUIObject)parent) ? ((com.oddlabs.tt.gui.GUIObject)parent).getParentGUIRoot() : (com.oddlabs.tt.gui.GUIRoot)(null));
        }
        // Java: 97
        public virtual bool hasToolTip()
        {
            return getToolTipText().isPresent();
        }
        // Java: 102
        public virtual void appendToolTip(com.oddlabs.tt.gui.ToolTipBox tool_tip_box)
        {
            getToolTipText().ifPresent(new java.util.function.Consumer_L((object a0) => { tool_tip_box.append(((string)a0)); }));
        }
        // Java: 107
        public java.util.Optional getToolTipText()
        {
            if (cached_tool_tip == null && tool_tip != null)
            {
                cached_tool_tip = ((string)tool_tip.get());
            }
            return (cached_tool_tip != null && !cached_tool_tip.isEmpty() ? java.util.Optional.of(cached_tool_tip) : java.util.Optional.empty());
        }
        // Java: 114
        public override void addTree()
        {
            if (getParentGUIRoot() != null)
            {
                base.addTree();
            }
        }
        // Java: 121
        public override void remove()
        {
            bool notify_remove = getParentGUIRoot() != null && ((com.oddlabs.tt.gui.GUIObject)parent) != null;
            base.remove();
            if (notify_remove)
            {
                removeTree();
            }
        }
        // Java: 129
        public virtual int translateXToLocal(int x)
        {
            com.oddlabs.tt.gui.GUIObject parent = ((com.oddlabs.tt.gui.GUIObject)getParent());
            return (parent == null ? x - getX() : parent.translateXToLocal(x) - getX());
        }
        // Java: 134
        public virtual int translateYToLocal(int y)
        {
            com.oddlabs.tt.gui.GUIObject parent = ((com.oddlabs.tt.gui.GUIObject)getParent());
            return (parent == null ? y - getY() : parent.translateYToLocal(y) - getY());
        }
        // Java: 139
        public virtual void correctPos(int dx, int dy)
        {
            setPos(getX() + dx, getY() + dy);
        }
        // Java: 143
        public void place()
        {
            place(origin);
        }
        // Java: 147
        public void place(com.oddlabs.tt.gui.Origin origin)
        {
            this.origin = origin;
            setPos(0, 0);
            placed = true;
        }
        // Java: 154
        public virtual void place(com.oddlabs.tt.gui.GUIObject neighbor, com.oddlabs.tt.gui.Placement direction)
        {
            place(neighbor, direction, com.oddlabs.tt.gui.Skin.getSkin().getFormData().objectSpacing());
        }
        // Java: 158
        public void place(com.oddlabs.tt.gui.GUIObject neighbor, com.oddlabs.tt.gui.Placement direction, int spacing)
        {
            int new_x = getXFromDirection(direction, spacing, neighbor.getX(), neighbor.getWidth());
            int new_y = getYFromDirection(direction, spacing, neighbor.getY(), neighbor.getHeight());
            origin = neighbor.origin;
            setPos(new_x, new_y);
            placed = true;
        }
        // Java: 177
        public void replace(com.oddlabs.tt.gui.GUIObject neighbor, com.oddlabs.tt.gui.Placement direction, int spacing)
        {
            int new_x = getXFromDirection(direction, spacing, neighbor.getX(), neighbor.getWidth());
            int new_y = getYFromDirection(direction, spacing, neighbor.getY(), neighbor.getHeight());
            origin = neighbor.origin;
            setPos(new_x, new_y);
            placed = true;
        }
        // Java: 186
        public int getXFromDirection(com.oddlabs.tt.gui.Placement direction, int spacing, int neighbour_x, int neighbour_width)
        {
            return ((direction).ordinal() switch { 3 /*BOTTOM_LEFT*/ or 0 /*TOP_LEFT*/ => (int)(neighbour_x), 4 /*BOTTOM_MID*/ or 1 /*TOP_MID*/ => (int)(neighbour_x + (neighbour_width - getWidth()) / 2), 5 /*BOTTOM_RIGHT*/ or 2 /*TOP_RIGHT*/ => (int)(neighbour_x + neighbour_width - getWidth()), 11 /*RIGHT_BOTTOM*/ or 10 /*RIGHT_MID*/ or 9 /*RIGHT_TOP*/ => (int)(neighbour_x + neighbour_width + spacing), 8 /*LEFT_BOTTOM*/ or 7 /*LEFT_MID*/ or 6 /*LEFT_TOP*/ => (int)(neighbour_x - getWidth() - spacing), _ => throw new System.InvalidOperationException("MatchException") });
        }
        // Java: 196
        public int getYFromDirection(com.oddlabs.tt.gui.Placement direction, int spacing, int neighbour_y, int neighbour_height)
        {
            return ((direction).ordinal() switch { 3 /*BOTTOM_LEFT*/ or 4 /*BOTTOM_MID*/ or 5 /*BOTTOM_RIGHT*/ => (int)(neighbour_y - getHeight() - spacing), 0 /*TOP_LEFT*/ or 2 /*TOP_RIGHT*/ or 1 /*TOP_MID*/ => (int)(neighbour_y + neighbour_height + spacing), 11 /*RIGHT_BOTTOM*/ or 8 /*LEFT_BOTTOM*/ => (int)(neighbour_y), 9 /*RIGHT_TOP*/ or 6 /*LEFT_TOP*/ => (int)(neighbour_y + neighbour_height - getHeight()), 7 /*LEFT_MID*/ or 10 /*RIGHT_MID*/ => (int)(neighbour_y + (neighbour_height - getHeight()) / 2), _ => throw new System.InvalidOperationException("MatchException") });
        }
        // Java: 206
        public com.oddlabs.tt.gui.Origin getOrigin()
        {
            return origin;
        }
        // Java: 211
        public void setFocusCycle(bool cycle)
        {
            focus_cycle = cycle;
        }
        // Java: 215
        public void setCanFocus(bool can_focus)
        {
            this.can_focus = can_focus;
        }
        // Java: 219
        public virtual void setDisabled(bool disabled)
        {
            if (this.disabled != disabled)
            {
                this.disabled = disabled;
                if (disabled && isActive())
                {
                    com.oddlabs.tt.gui.GUIObject parent = ((com.oddlabs.tt.gui.GUIObject)getParent());
                    if (null != parent)
                    {
                        parent.focusNext();
                    }
                }
            }
        }
        // Java: 232
        public bool isActive()
        {
            return active;
        }
        // Java: 239
        public void setGlobalFocus()
        {
            com.oddlabs.tt.gui.GUIRoot gui_root = getParentGUIRoot();
            if (gui_root != null)
            {
                gui_root.setGlobalFocus(this);
            }
        }
        // Java: 248
        public com.oddlabs.tt.gui.GUIObject getGlobalFocus()
        {
            com.oddlabs.tt.gui.GUIRoot gui_root = getParentGUIRoot();
            return (gui_root != null ? gui_root.getGlobalFocus() : (com.oddlabs.tt.gui.GUIObject)(null));
        }
        // Java: 253
        public bool isFocused()
        {
            return ((object)this == (object)getGlobalFocus());
        }
        // Java: 260
        public override bool isFocusable()
        {
            return can_focus;
        }
        // Java: 268
        public bool canFocus()
        {
            return can_focus && !isDisabled();
        }
        // Java: 272
        public bool isDisabled()
        {
            com.oddlabs.tt.gui.GUIObject parent = ((com.oddlabs.tt.gui.GUIObject)getParent());
            return disabled || (parent != null && parent.isDisabled());
        }
        // Java: 277
        public bool isHovered()
        {
            return hovered;
        }
        // Java: 281
        public com.oddlabs.tt.gui.GUIObject getNextHover()
        {
            return next_hover;
        }
        // Java: 285
        public void setNextHover(com.oddlabs.tt.gui.GUIObject next_hover)
        {
            this.next_hover = next_hover;
        }
        // Java: 289
        public com.oddlabs.tt.gui.GUIObject getFocusedChild()
        {
            return focused_child;
        }
        // Java: 293
        public void setGlobalFocused(com.oddlabs.tt.gui.GUIObject gui_object)
        {
            if (gui_object != null && ((object)gui_object != (object)focused_child))
            {
                putFirst(gui_object);
            }
            focused_child = gui_object;
        }
        // Java: 300
        public override void addChild(com.oddlabs.tt.gui.Renderable child_p)
        {
            com.oddlabs.tt.gui.GUIObject child = (com.oddlabs.tt.gui.GUIObject)child_p;
            base.addChild(child);
        }
        // Java: 305
        public override void removeChild(com.oddlabs.tt.gui.Renderable child_p)
        {
            com.oddlabs.tt.gui.GUIObject child = (com.oddlabs.tt.gui.GUIObject)child_p;
            com.oddlabs.tt.gui.GUIObject current = default;
            if (((object)child == (object)focused_child))
            {
                focused_child = null;
                current = child;
                while (current.getFocusedChild() != null)
                {
                    current = current.getFocusedChild();
                }
                if (current.isFocused())
                {
                    setGlobalFocus();
                }
            }
            base.removeChild(child);
        }
        // Java: 321
        public void switchFocusToFirstChild(com.oddlabs.tt.gui.FocusDirection dirEnum, bool bubble)
        {
            com.oddlabs.tt.gui.GUIObject bestCandidate = findNextFocusable(null, dirEnum);
            if (bestCandidate != null)
            {
                com.oddlabs.tt.gui.GUIObject.switchFocusToObject(bestCandidate, dirEnum);
            }
            else if (bubble && !focus_cycle)
            {
                com.oddlabs.tt.gui.GUIObject parent = ((com.oddlabs.tt.gui.GUIObject)getParent());
                if (parent != null)
                {
                    parent.switchFocusToNextChild(dirEnum, true);
                }
            }
        }
        // Java: 333 (Steam 2.2.207 fix: 'bubble' - a group focused under a modal dialog no longer hands the focus up to
        // the screen behind it, which crashed clicking the multiplayer screen's background in a game lobby)
        public void switchFocusToNextChild(com.oddlabs.tt.gui.FocusDirection dirEnum, bool bubble)
        {
            com.oddlabs.tt.gui.GUIObject bestCandidate = findNextFocusable(focused_child, dirEnum);
            if (bestCandidate != null)
            {
                com.oddlabs.tt.gui.GUIObject.switchFocusToObject(bestCandidate, dirEnum);
            }
            else if (focus_cycle)
            {
                com.oddlabs.tt.gui.GUIObject first = findNextFocusable(null, dirEnum);
                if (first != null)
                {
                    com.oddlabs.tt.gui.GUIObject.switchFocusToObject(first, dirEnum);
                }
                else
                {
                }
            }
            else if (bubble)
            {
                com.oddlabs.tt.gui.GUIObject parent = ((com.oddlabs.tt.gui.GUIObject)getParent());
                if (parent != null)
                {
                    parent.switchFocusToNextChild(dirEnum, true);
                }
                else
                {
                    com.oddlabs.tt.gui.GUIObject first_2 = findNextFocusable(null, dirEnum);
                    if (first_2 != null)
                    {
                        com.oddlabs.tt.gui.GUIObject.switchFocusToObject(first_2, dirEnum);
                    }
                }
            }
        }
        // Java: 360
        public com.oddlabs.tt.gui.GUIObject findNextFocusable(com.oddlabs.tt.gui.GUIObject current, com.oddlabs.tt.gui.FocusDirection dir)
        {
            com.oddlabs.tt.gui.GUIObject bestCandidate = null;
            com.oddlabs.tt.gui.GUIObject child = ((com.oddlabs.tt.gui.GUIObject)getFirstChild());
            while (child != null)
            {
                if (child.canFocus() && child.isTabStop())
                {
                    if (isBetterCandidate(current, child, bestCandidate, dir))
                    {
                        bestCandidate = child;
                    }
                }
                else
                {
                }
                child = ((com.oddlabs.tt.gui.GUIObject)child.getNext());
            }
            return bestCandidate;
        }
        // Java: 376
        public bool isBetterCandidate(com.oddlabs.tt.gui.GUIObject current, com.oddlabs.tt.gui.GUIObject candidate, com.oddlabs.tt.gui.GUIObject bestSoFar, com.oddlabs.tt.gui.FocusDirection dir)
        {
            if (((object)current == (object)candidate))
            {
                return false;
            }
            {
                switch ((dir).ordinal())
                {
                    case 0 /*FORWARD*/:
                    {
                        if (current != null && compareSpatial(candidate, current) <= 0)
                        {
                            return false;
                        }
                        return bestSoFar == null || compareSpatial(candidate, bestSoFar) < 0;
                    }
                    case 1 /*BACKWARD*/:
                    {
                        if (current != null && compareSpatial(candidate, current) >= 0)
                        {
                            return false;
                        }
                        return bestSoFar == null || compareSpatial(candidate, bestSoFar) > 0;
                    }
                    default:
                    {
                        return isBetterDirectionalCandidate(current, candidate, bestSoFar, dir);
                    }
                }
                throw new System.InvalidOperationException("MatchException");
            }
        }
        // Java: 401
        public bool isBetterDirectionalCandidate(com.oddlabs.tt.gui.GUIObject current, com.oddlabs.tt.gui.GUIObject candidate, com.oddlabs.tt.gui.GUIObject bestSoFar, com.oddlabs.tt.gui.FocusDirection dir)
        {
            if (current == null)
            {
                return isBetterCandidate(null, candidate, bestSoFar, com.oddlabs.tt.gui.FocusDirection.FORWARD);
            }
            int dx = candidate.getX() - current.getX();
            int dy = candidate.getY() - current.getY();
            bool validDirection = ((dir).ordinal() switch { 5 /*RIGHT*/ => (bool)(dx > 0), 4 /*LEFT*/ => (bool)(dx < 0), 2 /*UP*/ => (bool)(dy > 0), 3 /*DOWN*/ => (bool)(dy < 0), _ => (bool)(false) });
            if (!validDirection)
            {
                return false;
            }
            int distSq = dx * dx + dy * dy;
            int bestDistSq = java.lang.Integer.MAX_VALUE;
            if (bestSoFar != null)
            {
                int bdx = bestSoFar.getX() - current.getX();
                int bdy = bestSoFar.getY() - current.getY();
                bestDistSq = bdx * bdx + bdy * bdy;
            }
            return distSq < bestDistSq;
        }
        // Java: 438
        public int compareSpatial(com.oddlabs.tt.gui.GUIObject a, com.oddlabs.tt.gui.GUIObject b)
        {
            int yDiff = b.getY() - a.getY();
            if (java.lang.Math.abs(yDiff) > com.oddlabs.tt.gui.GUIObject.ROW_TOLERANCE)
            {
                return yDiff;
            }
            return a.getX() - b.getX();
        }
        // Java: 451
        public static void switchFocusToObject(com.oddlabs.tt.gui.GUIObject obj, com.oddlabs.tt.gui.FocusDirection dir)
        {
            obj.setFocus(dir);
        }
        // Java: 455
        public void switchFocus(com.oddlabs.tt.gui.FocusDirection direction)
        {
            switchFocus(direction, true);
        }
        // Java: 459
        public void switchFocus(com.oddlabs.tt.gui.FocusDirection direction, bool bubble)
        {
            if (focused_child == null)
            {
                switchFocusToFirstChild(direction, bubble);
                return;
            }
            switchFocusToNextChild(direction, bubble);
        }
        // Java: 469
        public void focusNext()
        {
            switchFocus(com.oddlabs.tt.gui.FocusDirection.FORWARD);
        }
        // Java: 473
        public void focusPrior()
        {
            switchFocus(com.oddlabs.tt.gui.FocusDirection.BACKWARD);
        }
        // Java: 477
        public void defocusBranch()
        {
            if (getFocusedChild() != null)
            {
                getFocusedChild().defocusBranch();
            }
            focusNotifyAll(false);
            focused_child = null;
        }
        // Java: 484
        public void refocusTree(com.oddlabs.tt.gui.GUIObject caller_child)
        {
            com.oddlabs.tt.gui.GUIObject parent = ((com.oddlabs.tt.gui.GUIObject)getParent());
            if (parent != null)
            {
                parent.refocusTree(this);
            }
            if (getFocusedChild() != null && ((object)getFocusedChild() != (object)caller_child))
            {
                getFocusedChild().defocusBranch();
            }
            if (!active)
            {
                focusNotifyAll(true);
            }
            focused_child = caller_child;
        }
        // Java: 498
        public virtual void restoreFocus()
        {
            if (focused_child != null)
            {
                focused_child.restoreFocus();
            }
            else
            {
                setFocus();
            }
        }
        // Java: 506
        public virtual void setFocus()
        {
            setFocus(com.oddlabs.tt.gui.FocusDirection.FORWARD);
        }
        // Java: 510
        public virtual void setFocus(com.oddlabs.tt.gui.FocusDirection direction)
        {
            com.oddlabs.tt.gui.GUIRoot gui_root = getParentGUIRoot();
            if (gui_root == null)
            {
                return;
            }
            if (isFocused() || !canFocus() || modalBlocked(gui_root))
            {
                if (modalBlocked(gui_root))
                {
                    gui_root.swapFocusBackup(this);
                }
                return;
            }
            refocusTree(null);
            setGlobalFocus();
        }
        // Java: 525
        public void focusNotifyAll(bool focus)
        {
            active = focus;
            focusNotify(focus);
            foreach (com.oddlabs.tt.guievent.EventListener listener in java.lang.JIter.of(listeners))
            {
                if ((listener is com.oddlabs.tt.guievent.FocusListener l))
                {
                    l.activated(focus);
                }
            }
        }
        // Java: 535
        public virtual void focusNotify(bool focus)
        {
        }
        // Java: 538
        public bool modalBlocked(com.oddlabs.tt.gui.GUIRoot gui_root)
        {
            com.oddlabs.tt.@delegate.ModalDelegate modal_delegate = gui_root.getModalDelegate();
            return modal_delegate != null && !modalRelative(modal_delegate);
        }
        // Java: 543
        public bool modalRelative(com.oddlabs.tt.@delegate.ModalDelegate modal_delegate)
        {
            return ((object)this == (object)modal_delegate) || (((com.oddlabs.tt.gui.GUIObject)getParent()) != null && (((object)((com.oddlabs.tt.gui.GUIObject)getParent()) == (object)modal_delegate) || ((com.oddlabs.tt.gui.GUIObject)getParent()).modalRelative(modal_delegate)));
        }
        // Java: 548
        public virtual com.oddlabs.tt.gui.CursorType getCursorType()
        {
            return com.oddlabs.tt.gui.CursorType.NORMAL;
        }
        // Java: 552
        public virtual bool canHoverBehind()
        {
            return false;
        }
        // Java: 556
        public void mouseScrolledAll(int amount)
        {
            if (disabled)
            {
                com.oddlabs.tt.gui.GUIObject parent = ((com.oddlabs.tt.gui.GUIObject)getParent());
                if (parent != null)
                {
                    parent.mouseScrolledAll(amount);
                }
                return;
            }
            mouseScrolled(amount);
            foreach (com.oddlabs.tt.guievent.EventListener listener in java.lang.JIter.of(listeners))
            {
                if ((listener is com.oddlabs.tt.guievent.MouseWheelListener l))
                {
                    l.mouseScrolled(amount);
                }
            }
        }
        // Java: 572
        public virtual void mouseScrolled(int amount)
        {
            com.oddlabs.tt.gui.GUIObject parent = ((com.oddlabs.tt.gui.GUIObject)getParent());
            if (parent != null)
            {
                parent.mouseScrolledAll(amount);
            }
        }
        // Java: 578
        public void mouseDraggedAll(com.oddlabs.tt.gui.MouseButton button, int x, int y, int relative_x, int relative_y, int absolute_x, int absolute_y)
        {
            if (disabled)
            {
                return;
            }
            mouseDragged(button, x, y, relative_x, relative_y, absolute_x, absolute_y);
            foreach (com.oddlabs.tt.guievent.EventListener listener in java.lang.JIter.of(listeners))
            {
                if ((listener is com.oddlabs.tt.guievent.MouseMotionListener l))
                {
                    l.mouseDragged(button, x, y, relative_x, relative_y, absolute_x, absolute_y);
                }
            }
        }
        // Java: 590
        public virtual void mouseDragged(com.oddlabs.tt.gui.MouseButton button, int x, int y, int relative_x, int relative_y, int absolute_x, int absolute_y)
        {
        }
        // Java: 595
        public void mouseMovedAll(int x, int y)
        {
            if (disabled)
            {
                return;
            }
            mouseMoved(x, y);
            foreach (com.oddlabs.tt.guievent.EventListener listener in java.lang.JIter.of(listeners))
            {
                if ((listener is com.oddlabs.tt.guievent.MouseMotionListener l))
                {
                    l.mouseMoved(x, y);
                }
            }
        }
        // Java: 606
        public virtual void mouseMoved(int x, int y)
        {
            com.oddlabs.tt.gui.GUIObject parent = ((com.oddlabs.tt.gui.GUIObject)getParent());
            if (parent != null)
            {
                parent.mouseMovedAll(x, y);
            }
        }
        // Java: 612
        public void mouseExitedAll()
        {
            hovered = false;
            mouseExited();
            foreach (com.oddlabs.tt.guievent.EventListener listener in java.lang.JIter.of(listeners))
            {
                if ((listener is com.oddlabs.tt.guievent.MouseMotionListener l))
                {
                    l.mouseExited();
                }
            }
        }
        // Java: 622
        public virtual void mouseExited()
        {
            cached_tool_tip = null;
            com.oddlabs.tt.gui.GUIObject parent = ((com.oddlabs.tt.gui.GUIObject)getParent());
            if (parent != null)
            {
                parent.mouseExitedAll();
            }
        }
        // Java: 629
        public void mouseEnteredAll()
        {
            hovered = true;
            mouseEntered();
            foreach (com.oddlabs.tt.guievent.EventListener listener in java.lang.JIter.of(listeners))
            {
                if ((listener is com.oddlabs.tt.guievent.MouseMotionListener l))
                {
                    l.mouseEntered();
                }
            }
        }
        // Java: 639
        public virtual void mouseEntered()
        {
            com.oddlabs.tt.gui.GUIObject parent = ((com.oddlabs.tt.gui.GUIObject)getParent());
            if (parent != null)
            {
                parent.mouseEnteredAll();
            }
        }
        // Java: 645
        public void mouseClickedAll(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
        {
            if (disabled)
            {
                return;
            }
            mouseClicked(button, x, y, clicks);
            foreach (com.oddlabs.tt.guievent.EventListener listener in java.lang.JIter.of(listeners))
            {
                if ((listener is com.oddlabs.tt.guievent.MouseClickListener l))
                {
                    l.mouseClicked(button, x, y, clicks);
                }
            }
        }
        // Java: 656
        public virtual void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
        {
            com.oddlabs.tt.gui.GUIObject parent = ((com.oddlabs.tt.gui.GUIObject)getParent());
            if (parent != null)
            {
                parent.mouseClickedAll(button, x, y, clicks);
            }
        }
        // Java: 662
        public void mouseReleasedAll(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
            if (disabled)
            {
                return;
            }
            mouseReleased(button, x, y);
            foreach (com.oddlabs.tt.guievent.EventListener listener in java.lang.JIter.of(listeners))
            {
                if ((listener is com.oddlabs.tt.guievent.MouseButtonListener l))
                {
                    l.mouseReleased(button, x, y);
                }
            }
        }
        // Java: 673
        public virtual void mouseReleased(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
            com.oddlabs.tt.gui.GUIObject parent = ((com.oddlabs.tt.gui.GUIObject)getParent());
            if (parent != null)
            {
                parent.mouseReleasedAll(button, x, y);
            }
        }
        // Java: 679
        public void mousePressedAll(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
            if (disabled)
            {
                return;
            }
            mousePressed(button, x, y);
            foreach (com.oddlabs.tt.guievent.EventListener listener in java.lang.JIter.of(listeners))
            {
                if ((listener is com.oddlabs.tt.guievent.MouseButtonListener l))
                {
                    l.mousePressed(button, x, y);
                }
            }
        }
        // Java: 690
        public virtual void mousePressed(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
            com.oddlabs.tt.gui.GUIObject parent = ((com.oddlabs.tt.gui.GUIObject)getParent());
            if (parent != null)
            {
                parent.mousePressedAll(button, x, y);
            }
        }
        // Java: 696
        public void mouseHeldAll(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
            if (disabled)
            {
                return;
            }
            mouseHeld(button, x, y);
            foreach (com.oddlabs.tt.guievent.EventListener listener in java.lang.JIter.of(listeners))
            {
                if ((listener is com.oddlabs.tt.guievent.MouseButtonListener l))
                {
                    l.mouseHeld(button, x, y);
                }
            }
        }
        // Java: 707
        public virtual void mouseHeld(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
            com.oddlabs.tt.gui.GUIObject parent = ((com.oddlabs.tt.gui.GUIObject)getParent());
            if (parent != null)
            {
                parent.mouseHeldAll(button, x, y);
            }
        }
        // Java: 713
        public void handleInputAll(com.oddlabs.tt.input.InputEvent @event)
        {
            if (@event.isConsumed())
            {
                return;
            }
            handleInput(@event);
            if (@event.isConsumed())
            {
                return;
            }
            foreach (com.oddlabs.tt.guievent.EventListener listener in java.lang.JIter.of(listeners))
            {
                if ((listener is com.oddlabs.tt.guievent.InputListener l))
                {
                    l.handleInput(@event);
                    if (@event.isConsumed())
                    {
                        return;
                    }
                }
            }
            com.oddlabs.tt.gui.GUIObject parent = ((com.oddlabs.tt.gui.GUIObject)getParent());
            if (parent != null)
            {
                parent.handleInputAll(@event);
            }
        }
        // Java: 731
        public virtual void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
            if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_ACTIVATE))
            {
                if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.PRESSED))
                {
                    mousePressedAll(com.oddlabs.tt.gui.MouseButton.LEFT, 0, 0);
                }
                else if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.RELEASED))
                {
                    mouseReleasedAll(com.oddlabs.tt.gui.MouseButton.LEFT, 0, 0);
                    mouseClickedAll(com.oddlabs.tt.gui.MouseButton.LEFT, 0, 0, 1);
                }
            }
        }
        // Java: 742
        public void addMouseClickListener(com.oddlabs.tt.guievent.MouseClickListener listener)
        {
            listeners.add(((com.oddlabs.tt.guievent.MouseClickListener)java.util.Objects.requireNonNull(listener, "listener")));
        }
        // Java: 749
        public void addMouseButtonListener(com.oddlabs.tt.guievent.MouseButtonListener listener)
        {
            listeners.add(((com.oddlabs.tt.guievent.MouseButtonListener)java.util.Objects.requireNonNull(listener, "listener")));
        }
        // Java: 753
        public void addMouseMotionListener(com.oddlabs.tt.guievent.MouseMotionListener listener)
        {
            listeners.add(((com.oddlabs.tt.guievent.MouseMotionListener)java.util.Objects.requireNonNull(listener, "listener")));
        }
        // Java: 757
        public void addMouseWheelListener(com.oddlabs.tt.guievent.MouseWheelListener listener)
        {
            listeners.add(((com.oddlabs.tt.guievent.MouseWheelListener)java.util.Objects.requireNonNull(listener, "listener")));
        }
        // Java: 761
        public void addInputListener(com.oddlabs.tt.guievent.InputListener listener)
        {
            listeners.add(((com.oddlabs.tt.guievent.InputListener)java.util.Objects.requireNonNull(listener, "listener")));
        }
        // Java: 765
        public void addFocusListener(com.oddlabs.tt.guievent.FocusListener listener)
        {
            listeners.add(((com.oddlabs.tt.guievent.FocusListener)java.util.Objects.requireNonNull(listener, "listener")));
        }
        static GUIObject()
        {
            GUIObject.logger = java.util.logging.Logger.getLogger(typeof(com.oddlabs.tt.gui.GUIObject).getName());
        }
        private void init_GUIObject()
        {
            this.tab_stop = true;
            this.focused_child = null;
            this.next_hover = null;
            this.listeners = new java.util.concurrent.CopyOnWriteArraySet();
            this.placed = false;
            this.origin = com.oddlabs.tt.gui.Origin.AT_START;
            this.cached_tool_tip = null;
        }
        public GUIObject() : this(null)
        {
        }
        public GUIObject(java.util.function.Supplier tool_tip) : base()
        {
            init_GUIObject();
            this.tool_tip = tool_tip;
        }
    }
}
