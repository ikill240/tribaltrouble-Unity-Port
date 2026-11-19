// Translated from KeyBindingDialog.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 33
    public class KeyBindingDialog : com.oddlabs.tt.gui.Form
    {
        public static java.util.Map SPINNER_FAMILIES;
        public com.oddlabs.tt.input.GameAction action;
        public java.util.function.Consumer onBindingChosen;
        public com.oddlabs.tt.gui.GUIRoot guiRoot;
        // Java: 107
        public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
            if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.PRESSED))
            {
                com.oddlabs.tt.input.Key key = @event.getKeyCode();
                bool isModifierKey = (((object)key == (object)com.oddlabs.tt.input.Key.LSHIFT) || ((object)key == (object)com.oddlabs.tt.input.Key.RSHIFT) || ((object)key == (object)com.oddlabs.tt.input.Key.LCONTROL) || ((object)key == (object)com.oddlabs.tt.input.Key.RCONTROL) || ((object)key == (object)com.oddlabs.tt.input.Key.LALT) || ((object)key == (object)com.oddlabs.tt.input.Key.RALT) || ((object)key == (object)com.oddlabs.tt.input.Key.LSUPER) || ((object)key == (object)com.oddlabs.tt.input.Key.RSUPER));
                if (!isModifierKey && key != null && ((object)key != (object)com.oddlabs.tt.input.Key.KEY_UNKNOWN))
                {
                    java.util.EnumSet modifiers = java.util.EnumSet.noneOf(typeof(com.oddlabs.tt.input.Modifier));
                    if (@event.isShiftDown())
                    {
                        modifiers.add(com.oddlabs.tt.input.Modifier.SHIFT);
                    }
                    if (@event.isAltDown())
                    {
                        modifiers.add(com.oddlabs.tt.input.Modifier.ALT);
                    }
                    if (@event.isControlDown())
                    {
                        modifiers.add(com.oddlabs.tt.input.Modifier.CONTROL);
                    }
                    if (@event.isMetaDown())
                    {
                        modifiers.add(com.oddlabs.tt.input.Modifier.META);
                    }
                    com.oddlabs.tt.input.InputBinding binding = new com.oddlabs.tt.input.InputBinding(key, modifiers, action);
                    com.oddlabs.tt.input.InputManager manager = com.oddlabs.tt.render.Renderer.getLocalInput().getInputManager();
                    com.oddlabs.tt.input.GameAction conflict = com.oddlabs.tt.input.KeyBindingConflicts.findConflict(action, binding, manager);
                    if (conflict != null)
                    {
                        string otherName = default;
                        try
                        {
                            otherName = com.oddlabs.tt.form.AbstractOptionsMenu.i18n("action." + java.lang.JStr.valueOf(conflict.name()));
                        }
                        catch (System.Exception e)
                        {
                            otherName = conflict.name();
                        }
                        guiRoot.addModalForm(new com.oddlabs.tt.form.KeyBindingDialog.SwapConflictForm(this, otherName, conflict, binding));
                        @event.consume();
                        return;
                    }
                    com.oddlabs.tt.form.KeyBindingDialog.propagateKeyChange(manager, action, manager.getBindings(action), java.util.Set_.of(binding));
                    onBindingChosen.accept(java.util.Set_.of(binding));
                    remove();
                    @event.consume();
                    return;
                }
                @event.consume();
                return;
            }
            base.handleInput(@event);
        }
        // Java: 154
        public void swapBindings(com.oddlabs.tt.input.GameAction other, com.oddlabs.tt.input.InputBinding binding)
        {
            com.oddlabs.tt.input.InputManager manager = com.oddlabs.tt.render.Renderer.getLocalInput().getInputManager();
            java.util.Set mine_old = manager.getBindings(action);
            java.util.Set their_old = manager.getBindings(other);
            java.util.Set theirs = new java.util.HashSet();
            foreach (com.oddlabs.tt.input.InputBinding b in java.lang.JIter.of(their_old))
            {
                if (((object)b.key() != (object)binding.key()) || !java.lang.JO.equals(b.modifiers(), binding.modifiers()))
                {
                    theirs.add(b);
                }
            }
            foreach (com.oddlabs.tt.input.InputBinding b_2 in java.lang.JIter.of(mine_old))
            {
                theirs.add(new com.oddlabs.tt.input.InputBinding(b_2.key(), b_2.modifiers(), other));
            }
            manager.setBindings(other, theirs);
            if (((object)((com.oddlabs.tt.input.GameAction[])com.oddlabs.tt.form.KeyBindingDialog.SPINNER_FAMILIES.get(action)) != (object)((com.oddlabs.tt.input.GameAction[])com.oddlabs.tt.form.KeyBindingDialog.SPINNER_FAMILIES.get(other))))
            {
                com.oddlabs.tt.form.KeyBindingDialog.propagateKeyChange(manager, other, their_old, theirs);
                com.oddlabs.tt.form.KeyBindingDialog.propagateKeyChange(manager, action, mine_old, java.util.Set_.of(binding));
            }
            onBindingChosen.accept(java.util.Set_.of(binding));
            remove();
        }
        // Java: 181
        public static void propagateKeyChange(com.oddlabs.tt.input.InputManager manager, com.oddlabs.tt.input.GameAction changed, java.util.Set old_bindings, java.util.Set new_bindings)
        {
            com.oddlabs.tt.input.GameAction[] family = ((com.oddlabs.tt.input.GameAction[])com.oddlabs.tt.form.KeyBindingDialog.SPINNER_FAMILIES.get(changed));
            if (family == null || ((object)family[0] != (object)changed))
            {
                return;
            }
            java.util.Set old_keys = new java.util.HashSet();
            foreach (com.oddlabs.tt.input.InputBinding b in java.lang.JIter.of(old_bindings))
            {
                old_keys.add(b.key());
            }
            java.util.Set new_keys = new java.util.HashSet();
            foreach (com.oddlabs.tt.input.InputBinding b_2 in java.lang.JIter.of(new_bindings))
            {
                new_keys.add(b_2.key());
            }
            java.util.Set off_keys = new java.util.HashSet(old_keys);
            off_keys.removeAll(new_keys);
            new_keys.removeAll(old_keys);
            if (off_keys.isEmpty() || new_keys.size() != 1)
            {
                return;
            }
            com.oddlabs.tt.input.Key new_key = ((com.oddlabs.tt.input.Key)new_keys.iterator().next());
            foreach (com.oddlabs.tt.input.GameAction member in family)
            {
                if (((object)member == (object)changed))
                {
                    continue;
                }
                java.util.Set updated = new java.util.HashSet();
                bool touched = false;
                foreach (com.oddlabs.tt.input.InputBinding b_3 in java.lang.JIter.of(manager.getBindings(member)))
                {
                    if (off_keys.contains(b_3.key()))
                    {
                        updated.add(new com.oddlabs.tt.input.InputBinding(new_key, b_3.modifiers(), member));
                        touched = true;
                    }
                    else
                    {
                        updated.add(b_3);
                    }
                }
                if (touched)
                {
                    manager.setBindings(member, updated);
                }
            }
        }
        // Java: 222
        public class SwapConflictForm : com.oddlabs.tt.gui.Form
        {
            public com.oddlabs.tt.form.KeyBindingDialog this_0;
            public SwapConflictForm(com.oddlabs.tt.form.KeyBindingDialog this_0_, string otherName, com.oddlabs.tt.input.GameAction other, com.oddlabs.tt.input.InputBinding binding) : base()
            {
                this.this_0 = this_0_;
                com.oddlabs.tt.gui.LabelBox info_label = new com.oddlabs.tt.gui.LabelBox(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("conflict_swap_message", otherName), com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), 300);
                addChild(info_label);
                com.oddlabs.tt.gui.Group button_group = new com.oddlabs.tt.gui.Group();
                com.oddlabs.tt.gui.HorizButton swap_button = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("btn_swap"), 80);
                swap_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
                {
                    this_0.swapBindings(other, binding);
                    remove();
                }));
                button_group.addChild(swap_button);
                com.oddlabs.tt.gui.HorizButton cancel_button = new com.oddlabs.tt.gui.CancelButton(80);
                cancel_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_5, int unused_6, int unused_7, int unused_8) =>
                {
                    cancel();
                }));
                button_group.addChild(cancel_button);
                swap_button.place();
                cancel_button.place(swap_button, com.oddlabs.tt.gui.Placement.RIGHT_MID);
                button_group.compileCanvas();
                addChild(button_group);
                info_label.place();
                button_group.place(info_label, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
                compileCanvas();
                centerPos();
            }
        }
        static KeyBindingDialog()
        {
            {
                com.oddlabs.tt.input.GameAction[][] families = new com.oddlabs.tt.input.GameAction[][] { new com.oddlabs.tt.input.GameAction[] { com.oddlabs.tt.input.GameAction.RES_TREE, com.oddlabs.tt.input.GameAction.RES_TREE_DEC, com.oddlabs.tt.input.GameAction.RES_TREE_BATCH, com.oddlabs.tt.input.GameAction.RES_TREE_BATCH_DEC }, new com.oddlabs.tt.input.GameAction[] { com.oddlabs.tt.input.GameAction.RES_ROCK, com.oddlabs.tt.input.GameAction.RES_ROCK_DEC, com.oddlabs.tt.input.GameAction.RES_ROCK_BATCH, com.oddlabs.tt.input.GameAction.RES_ROCK_BATCH_DEC }, new com.oddlabs.tt.input.GameAction[] { com.oddlabs.tt.input.GameAction.RES_IRON, com.oddlabs.tt.input.GameAction.RES_IRON_DEC, com.oddlabs.tt.input.GameAction.RES_IRON_BATCH, com.oddlabs.tt.input.GameAction.RES_IRON_BATCH_DEC }, new com.oddlabs.tt.input.GameAction[] { com.oddlabs.tt.input.GameAction.RES_CHICKEN, com.oddlabs.tt.input.GameAction.RES_CHICKEN_DEC, com.oddlabs.tt.input.GameAction.RES_CHICKEN_BATCH, com.oddlabs.tt.input.GameAction.RES_CHICKEN_BATCH_DEC }, new com.oddlabs.tt.input.GameAction[] { com.oddlabs.tt.input.GameAction.TRAIN_PEON, com.oddlabs.tt.input.GameAction.TRAIN_PEON_DEC, com.oddlabs.tt.input.GameAction.TRAIN_PEON_BATCH, com.oddlabs.tt.input.GameAction.TRAIN_PEON_BATCH_DEC } };
                java.util.Map members = new java.util.EnumMap(typeof(com.oddlabs.tt.input.GameAction));
                foreach (com.oddlabs.tt.input.GameAction[] family in families)
                {
                    foreach (com.oddlabs.tt.input.GameAction member in family)
                    {
                        members.put(member, family);
                    }
                }
                com.oddlabs.tt.form.KeyBindingDialog.SPINNER_FAMILIES = members;
            }
        }
        public KeyBindingDialog(com.oddlabs.tt.gui.GUIRoot guiRoot, com.oddlabs.tt.input.GameAction action, java.util.function.Consumer onBindingChosen) : base()
        {
            this.guiRoot = guiRoot;
            this.action = action;
            this.onBindingChosen = onBindingChosen;
            string actionName = default;
            try
            {
                actionName = com.oddlabs.tt.form.AbstractOptionsMenu.i18n("action." + java.lang.JStr.valueOf(action.name()));
            }
            catch (System.Exception e)
            {
                actionName = action.name();
            }
            com.oddlabs.tt.gui.LabelBox info_label = new com.oddlabs.tt.gui.LabelBox("Press key for: " + java.lang.JStr.valueOf(actionName), com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), 300);
            addChild(info_label);
            com.oddlabs.tt.gui.Group button_group = new com.oddlabs.tt.gui.Group();
            addChild(button_group);
            com.oddlabs.tt.gui.HorizButton clear_button = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("btn_clear"), 80);
            clear_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                onBindingChosen.accept(java.util.Set_.of());
                remove();
            }));
            button_group.addChild(clear_button);
            com.oddlabs.tt.gui.HorizButton reset_button = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("btn_reset"), 80);
            reset_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_5, int unused_6, int unused_7, int unused_8) =>
            {
                com.oddlabs.tt.input.InputManager manager = com.oddlabs.tt.render.Renderer.getLocalInput().getInputManager();
                java.util.NavigableSet defaults = manager.getDefaultBindings(action);
                com.oddlabs.tt.form.KeyBindingDialog.propagateKeyChange(manager, action, manager.getBindings(action), defaults);
                onBindingChosen.accept(defaults);
                remove();
            }));
            button_group.addChild(reset_button);
            com.oddlabs.tt.gui.HorizButton cancel_button = new com.oddlabs.tt.gui.CancelButton(80);
            cancel_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_9, int unused_10, int unused_11, int unused_12) =>
            {
                cancel();
            }));
            button_group.addChild(cancel_button);
            info_label.place();
            clear_button.place();
            reset_button.place(clear_button, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            cancel_button.place(reset_button, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            button_group.compileCanvas();
            button_group.place(info_label, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            compileCanvas();
            centerPos();
            setCanFocus(true);
        }
    }
}
