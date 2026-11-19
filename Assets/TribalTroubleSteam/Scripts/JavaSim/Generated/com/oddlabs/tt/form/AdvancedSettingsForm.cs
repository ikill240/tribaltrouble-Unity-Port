// Translated from AdvancedSettingsForm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 29
    public class AdvancedSettingsForm : com.oddlabs.tt.gui.Form
    {
        public const int MIN_MAX_UNITS = 20;
        public const int MAX_MAX_UNITS = 1000;
        public const int MAX_UNITS_STEP = 10;
        public const int MIN_STARTING_UNITS = 1;
        public const int MIN_MAX_BUILDINGS = 1;
        public const int MAX_MAX_BUILDINGS = 100;
        public const int BUTTON_WIDTH = 100;
        public const int RESET_BUTTON_WIDTH = 160;
        public const int LABEL_WIDTH = 160;
        public const int SLIDER_LENGTH = 250;
        public const int VALUE_WIDTH = 60;
        public const int WARNING_WIDTH = 470;
        public static java.util.ResourceBundle bundle;
        // Java: 45
        public class Values : java.lang.Object
        {
            public int maxUnits() { return maxUnits_f; }
            public int startingUnits() { return startingUnits_f; }
            public int maxBuildings() { return maxBuildings_f; }
            public bool ships() { return ships_f; }
            public Values(int maxUnits, int startingUnits, int maxBuildings, bool ships)
            {
                this.maxUnits_f = maxUnits;
                this.startingUnits_f = startingUnits;
                this.maxBuildings_f = maxBuildings;
                this.ships_f = ships;
            }
            public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is Values r)) return false; return true && java.lang.JO.equals(maxUnits_f, r.maxUnits_f) && java.lang.JO.equals(startingUnits_f, r.startingUnits_f) && java.lang.JO.equals(maxBuildings_f, r.maxBuildings_f) && java.lang.JO.equals(ships_f, r.ships_f); }
            public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(maxUnits_f); h = 31 * h + java.lang.JO.hashCode(startingUnits_f); h = 31 * h + java.lang.JO.hashCode(maxBuildings_f); h = 31 * h + java.lang.JO.hashCode(ships_f); } return h; }
            public override string toString() { return "Values[" + "maxUnits=" + java.lang.JO.toString(maxUnits_f) + ", startingUnits=" + java.lang.JO.toString(startingUnits_f) + ", maxBuildings=" + java.lang.JO.toString(maxBuildings_f) + ", ships=" + java.lang.JO.toString(ships_f) + "]"; }
            public int maxUnits_f;
            public int startingUnits_f;
            public int maxBuildings_f;
            public bool ships_f;
            // Java: 46
            public static com.oddlabs.tt.form.AdvancedSettingsForm.Values defaults()
            {
                return new com.oddlabs.tt.form.AdvancedSettingsForm.Values(com.oddlabs.matchmaking.Game.DEFAULT_MAX_UNIT_COUNT, com.oddlabs.matchmaking.Game.DEFAULT_INITIAL_UNIT_COUNT, com.oddlabs.matchmaking.Game.DEFAULT_MAX_BUILDING_COUNT, false);
            }
            // Java: 51
            public bool isOverDefaults()
            {
                return maxUnits_f > com.oddlabs.matchmaking.Game.DEFAULT_MAX_UNIT_COUNT || startingUnits_f > com.oddlabs.matchmaking.Game.DEFAULT_INITIAL_UNIT_COUNT || maxBuildings_f > com.oddlabs.matchmaking.Game.DEFAULT_MAX_BUILDING_COUNT;
            }
        }
        // Java: 57
        public interface Listener
        {
            // Java: 59
            void applied(com.oddlabs.tt.form.AdvancedSettingsForm.Values values);
        }
        public sealed class Listener_L : java.lang.Object, Listener
        {
            readonly System.Action<com.oddlabs.tt.form.AdvancedSettingsForm.Values> f;
            public Listener_L(System.Action<com.oddlabs.tt.form.AdvancedSettingsForm.Values> f) { this.f = f; }
            public void applied(com.oddlabs.tt.form.AdvancedSettingsForm.Values p0) { f(p0); }
        }
        public com.oddlabs.tt.form.AdvancedSettingsForm.Listener listener;
        public com.oddlabs.tt.gui.Slider slider_max_units;
        public com.oddlabs.tt.gui.Slider slider_starting_units;
        public com.oddlabs.tt.gui.Slider slider_max_buildings;
        public com.oddlabs.tt.gui.Label value_max_units;
        public com.oddlabs.tt.gui.Label value_starting_units;
        public com.oddlabs.tt.gui.Label value_max_buildings;
        public com.oddlabs.tt.gui.CheckBox cb_ships;
        public com.oddlabs.tt.gui.Label label_warning;
        public bool force_ships;
        public bool ships_choice;
        // Java: 150
        public static string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.form.AdvancedSettingsForm.bundle, key, args);
        }
        // Java: 154
        public override void setFocus(com.oddlabs.tt.gui.FocusDirection direction)
        {
            if (((object)direction == (object)com.oddlabs.tt.gui.FocusDirection.BACKWARD))
            {
                base.setFocus(direction);
            }
            else
            {
                slider_max_units.setFocus(direction);
            }
        }
        // Java: 163
        public com.oddlabs.tt.form.AdvancedSettingsForm.Values currentValues()
        {
            bool ships = (force_ships ? ships_choice : cb_ships != null && cb_ships.isMarked());
            return new com.oddlabs.tt.form.AdvancedSettingsForm.Values(slider_max_units.getValue() * com.oddlabs.tt.form.AdvancedSettingsForm.MAX_UNITS_STEP, slider_starting_units.getValue(), slider_max_buildings.getValue(), ships);
        }
        // Java: 170
        public void setValues(com.oddlabs.tt.form.AdvancedSettingsForm.Values values)
        {
            slider_max_units.setValue(values.maxUnits() / com.oddlabs.tt.form.AdvancedSettingsForm.MAX_UNITS_STEP);
            slider_starting_units.setValue(values.startingUnits());
            slider_max_buildings.setValue(values.maxBuildings());
            ships_choice = values.ships();
            if (cb_ships != null)
            {
                cb_ships.setMarked(force_ships || values.ships());
            }
            update();
        }
        // Java: 181
        public void update()
        {
            slider_starting_units.setMax(slider_max_units.getValue() * com.oddlabs.tt.form.AdvancedSettingsForm.MAX_UNITS_STEP);
            com.oddlabs.tt.form.AdvancedSettingsForm.Values values = currentValues();
            value_max_units.set(java.lang.Integer.toString(values.maxUnits()));
            value_starting_units.set(java.lang.Integer.toString(values.startingUnits()));
            value_max_buildings.set(java.lang.Integer.toString(values.maxBuildings()));
            label_warning.set((values.isOverDefaults() ? com.oddlabs.tt.form.AdvancedSettingsForm.i18n("performance_warning") : ""));
        }
        // Java: 191
        public void submit()
        {
            remove();
            listener.applied(currentValues());
        }
        static AdvancedSettingsForm()
        {
            AdvancedSettingsForm.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.form.AdvancedSettingsForm).getName());
        }
        public AdvancedSettingsForm(com.oddlabs.tt.form.AdvancedSettingsForm.Values current, bool show_ships, bool force_ships, com.oddlabs.tt.form.AdvancedSettingsForm.Listener listener) : base(com.oddlabs.tt.form.AdvancedSettingsForm.i18n("caption"))
        {
            this.listener = listener;
            this.force_ships = force_ships;
            com.oddlabs.tt.gui.Label label_max_units = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.AdvancedSettingsForm.i18n("max_units"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), com.oddlabs.tt.form.AdvancedSettingsForm.LABEL_WIDTH);
            slider_max_units = new com.oddlabs.tt.gui.Slider(com.oddlabs.tt.form.AdvancedSettingsForm.SLIDER_LENGTH, com.oddlabs.tt.form.AdvancedSettingsForm.MIN_MAX_UNITS / com.oddlabs.tt.form.AdvancedSettingsForm.MAX_UNITS_STEP, com.oddlabs.tt.form.AdvancedSettingsForm.MAX_MAX_UNITS / com.oddlabs.tt.form.AdvancedSettingsForm.MAX_UNITS_STEP, com.oddlabs.matchmaking.Game.DEFAULT_MAX_UNIT_COUNT / com.oddlabs.tt.form.AdvancedSettingsForm.MAX_UNITS_STEP);
            value_max_units = new com.oddlabs.tt.gui.Label("", com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), com.oddlabs.tt.form.AdvancedSettingsForm.VALUE_WIDTH);
            com.oddlabs.tt.gui.Label label_starting_units = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.AdvancedSettingsForm.i18n("starting_units"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), com.oddlabs.tt.form.AdvancedSettingsForm.LABEL_WIDTH);
            slider_starting_units = new com.oddlabs.tt.gui.Slider(com.oddlabs.tt.form.AdvancedSettingsForm.SLIDER_LENGTH, com.oddlabs.tt.form.AdvancedSettingsForm.MIN_STARTING_UNITS, com.oddlabs.tt.form.AdvancedSettingsForm.MAX_MAX_UNITS, com.oddlabs.matchmaking.Game.DEFAULT_INITIAL_UNIT_COUNT);
            value_starting_units = new com.oddlabs.tt.gui.Label("", com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), com.oddlabs.tt.form.AdvancedSettingsForm.VALUE_WIDTH);
            com.oddlabs.tt.gui.Label label_max_buildings = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.AdvancedSettingsForm.i18n("max_buildings"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), com.oddlabs.tt.form.AdvancedSettingsForm.LABEL_WIDTH);
            slider_max_buildings = new com.oddlabs.tt.gui.Slider(com.oddlabs.tt.form.AdvancedSettingsForm.SLIDER_LENGTH, com.oddlabs.tt.form.AdvancedSettingsForm.MIN_MAX_BUILDINGS, com.oddlabs.tt.form.AdvancedSettingsForm.MAX_MAX_BUILDINGS, com.oddlabs.matchmaking.Game.DEFAULT_MAX_BUILDING_COUNT);
            value_max_buildings = new com.oddlabs.tt.gui.Label("", com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), com.oddlabs.tt.form.AdvancedSettingsForm.VALUE_WIDTH);
            cb_ships = (show_ships ? new com.oddlabs.tt.gui.CheckBox(current.ships(), com.oddlabs.tt.form.AdvancedSettingsForm.i18n("ships"), com.oddlabs.tt.form.AdvancedSettingsForm.i18n("ships_tip")) : (com.oddlabs.tt.gui.CheckBox)(null));
            if (cb_ships != null && force_ships)
            {
                cb_ships.setDisabled(true);
            }
            label_warning = new com.oddlabs.tt.gui.Label("", com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), com.oddlabs.tt.form.AdvancedSettingsForm.WARNING_WIDTH);
            slider_max_units.addValueListener(new com.oddlabs.tt.guievent.ValueListener_L((long unused) =>
            {
                update();
            }));
            slider_starting_units.addValueListener(new com.oddlabs.tt.guievent.ValueListener_L((long unused_2) =>
            {
                update();
            }));
            slider_max_buildings.addValueListener(new com.oddlabs.tt.guievent.ValueListener_L((long unused_3) =>
            {
                update();
            }));
            com.oddlabs.tt.gui.HorizButton button_reset = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.form.AdvancedSettingsForm.i18n("reset_defaults"), com.oddlabs.tt.form.AdvancedSettingsForm.RESET_BUTTON_WIDTH);
            button_reset.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_4, int unused_5, int unused_6, int unused_7) =>
            {
                setValues(com.oddlabs.tt.form.AdvancedSettingsForm.Values.defaults());
            }));
            com.oddlabs.tt.gui.HorizButton button_ok = new com.oddlabs.tt.gui.OKButton(com.oddlabs.tt.form.AdvancedSettingsForm.BUTTON_WIDTH);
            button_ok.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_8, int unused_9, int unused_10, int unused_11) =>
            {
                submit();
            }));
            com.oddlabs.tt.gui.HorizButton button_cancel = new com.oddlabs.tt.gui.CancelButton(com.oddlabs.tt.form.AdvancedSettingsForm.BUTTON_WIDTH);
            button_cancel.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_12, int unused_13, int unused_14, int unused_15) =>
            {
                this.cancel();
            }));
            addChild(label_max_units);
            addChild(slider_max_units);
            addChild(value_max_units);
            addChild(label_starting_units);
            addChild(slider_starting_units);
            addChild(value_starting_units);
            addChild(label_max_buildings);
            addChild(slider_max_buildings);
            addChild(value_max_buildings);
            if (cb_ships != null)
            {
                addChild(cb_ships);
            }
            addChild(label_warning);
            addChild(button_reset);
            addChild(button_ok);
            addChild(button_cancel);
            label_max_units.place();
            slider_max_units.place(label_max_units, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            value_max_units.place(slider_max_units, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            label_starting_units.place(label_max_units, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            slider_starting_units.place(label_starting_units, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            value_starting_units.place(slider_starting_units, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            label_max_buildings.place(label_starting_units, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            slider_max_buildings.place(label_max_buildings, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            value_max_buildings.place(slider_max_buildings, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            com.oddlabs.tt.gui.GUIObject last = label_max_buildings;
            if (cb_ships != null)
            {
                cb_ships.place(label_max_buildings, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
                last = cb_ships;
            }
            label_warning.place(last, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT, com.oddlabs.tt.gui.Skin.getSkin().getFormData().sectionSpacing());
            button_cancel.place(label_warning, com.oddlabs.tt.gui.Placement.BOTTOM_RIGHT);
            button_ok.place(button_cancel, com.oddlabs.tt.gui.Placement.LEFT_MID);
            button_reset.place(button_ok, com.oddlabs.tt.gui.Placement.LEFT_MID);
            setValues(current);
            compileCanvas();
            centerPos();
        }
    }
}
