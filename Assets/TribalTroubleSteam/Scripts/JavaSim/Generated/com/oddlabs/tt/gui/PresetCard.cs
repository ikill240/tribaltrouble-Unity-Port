// Translated from PresetCard.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 10
    public class PresetCard : com.oddlabs.tt.gui.RadioButtonGroupElement
    {
        public const int PADDING_X = 12;
        public const int PADDING_Y = 10;
        public const int DELETE_INSET = 4;
        public com.oddlabs.matchmaking.Preset preset;
        public com.oddlabs.tt.guievent.PresetChosenListener choose_listener;
        public com.oddlabs.tt.gui.PresetCard.DeleteButton delete_button;
        public bool pressed;
        // Java: 46
        public com.oddlabs.matchmaking.Preset getPreset()
        {
            return preset;
        }
        // Java: 50
        public override void mousePressed(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
            pressed = true;
        }
        // Java: 55
        public override void mouseReleased(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
            pressed = false;
        }
        // Java: 60
        public override void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
        {
            base.mouseClicked(button, x, y, clicks);
            choose_listener.presetChosen(preset);
        }
        // Java: 66
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            com.oddlabs.tt.gui.PresetCard.renderCardBackground(renderer, this, isMarked() || isActive() || (isHovered() && pressed));
        }
        // Java: 76
        public static void renderCardBackground(com.oddlabs.tt.render.GUIRenderer renderer, com.oddlabs.tt.gui.GUIObject card, bool active)
        {
            com.oddlabs.tt.gui.ModeIconQuads.Mode skinMode = (card.isDisabled() ? com.oddlabs.tt.gui.ModeIconQuads.Mode.DISABLED : (active ? com.oddlabs.tt.gui.ModeIconQuads.Mode.ACTIVE : com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL));
            com.oddlabs.tt.gui.Skin.getSkin().getGroupData().group().render(renderer, 0.0f, 0.0f, card.getWidth(), card.getHeight(), skinMode);
        }
        // Java: 81
        public class DeleteButton : com.oddlabs.tt.gui.IconButton
        {
            public com.oddlabs.tt.gui.PresetCard card;
            // Java: 89
            public override void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
            {
            }
            // Java: 94
            public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
            {
                if (!card.isHovered() && !isHovered() && !isActive())
                {
                    return;
                }
                base.renderGeometry(renderer);
            }
            public DeleteButton(com.oddlabs.tt.gui.PresetCard card) : base(com.oddlabs.tt.gui.Skin.getSkin().getFormData().formClose(), null)
            {
                this.card = card;
            }
        }
        public PresetCard(com.oddlabs.matchmaking.Preset preset, int width, bool marked, com.oddlabs.tt.gui.RadioButtonGroup group, com.oddlabs.tt.guievent.PresetChosenListener choose_listener, com.oddlabs.tt.guievent.PresetDeleteListener delete_listener) : base(marked, group)
        {
            this.preset = preset;
            this.choose_listener = choose_listener;
            com.oddlabs.tt.font.Font title_font = com.oddlabs.tt.gui.Skin.getSkin().getButtonFont();
            int content_width = width - 2 * com.oddlabs.tt.gui.PresetCard.PADDING_X;
            com.oddlabs.tt.gui.Label title_label = new com.oddlabs.tt.gui.Label(preset.getName(), title_font, content_width);
            int height = com.oddlabs.tt.gui.PresetCard.PADDING_Y + title_label.getHeight() + com.oddlabs.tt.gui.PresetCard.PADDING_Y;
            setDim(width, height);
            title_label.setPos(com.oddlabs.tt.gui.PresetCard.PADDING_X, com.oddlabs.tt.gui.PresetCard.PADDING_Y);
            addChild(title_label);
            delete_button = new com.oddlabs.tt.gui.PresetCard.DeleteButton(this);
            delete_button.setPos(width - delete_button.getWidth() - com.oddlabs.tt.gui.PresetCard.DELETE_INSET, height - delete_button.getHeight() - com.oddlabs.tt.gui.PresetCard.DELETE_INSET);
            addChild(delete_button);
            delete_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                delete_listener.presetDeleted(preset);
            }));
            setCanFocus(true);
        }
    }
}
