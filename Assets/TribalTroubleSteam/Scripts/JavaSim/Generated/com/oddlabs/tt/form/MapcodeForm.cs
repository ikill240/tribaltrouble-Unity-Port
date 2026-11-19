// Translated from MapcodeForm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 26
    public class MapcodeForm : com.oddlabs.tt.gui.Form
    {
        public const int BUTTON_WIDTH = 100;
        public static java.util.ResourceBundle bundle;
        // Java: 30
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.form.MapcodeForm.bundle, key, args);
        }
        public com.oddlabs.tt.form.TerrainMenu menu;
        public com.oddlabs.tt.gui.EditLine editline_seed;
        // Java: 65
        public override void setFocus(com.oddlabs.tt.gui.FocusDirection direction)
        {
            if (((object)direction == (object)com.oddlabs.tt.gui.FocusDirection.BACKWARD))
            {
                base.setFocus(direction);
            }
            else
            {
                editline_seed.setFocus(direction);
            }
        }
        // Java: 74
        public void done()
        {
            remove();
            menu.parseMapcode(editline_seed.getContents());
            menu.setFocus();
        }
        // Java: 80
        public class RandButtonListener : java.lang.Object, com.oddlabs.tt.guievent.MouseClickListener
        {
            public com.oddlabs.tt.form.MapcodeForm this_0;
            // Java: 81
            public void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
            {
                java.util.Random random = new java.util.Random(com.oddlabs.tt.@event.LocalEventQueue.getQueue().getHighPrecisionManager().getTick() * ((long)(com.oddlabs.tt.@event.LocalEventQueue.getQueue().getHighPrecisionManager().getTick())));
                random.nextInt();
                java.math.BigInteger rand_int = new java.math.BigInteger(60, random);
                string rand_string = com.oddlabs.tt.util.WordsEncoding.encode(rand_int);
                this_0.editline_seed.clear();
                this_0.editline_seed.append(rand_string);
            }
            public RandButtonListener(com.oddlabs.tt.form.MapcodeForm this_0_) : base()
            {
                this.this_0 = this_0_;
            }
        }
        static MapcodeForm()
        {
            MapcodeForm.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.form.MapcodeForm).getName());
        }
        public MapcodeForm(com.oddlabs.tt.form.TerrainMenu menu) : base()
        {
            this.menu = menu;
            com.oddlabs.tt.gui.Label label_seed = new com.oddlabs.tt.gui.Label(i18n("map_code"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            editline_seed = new com.oddlabs.tt.gui.EditLine(400, 100);
            editline_seed.addEnterListener(new com.oddlabs.tt.guievent.EnterListener_L((string unused) =>
            {
                done();
            }));
            com.oddlabs.tt.gui.HorizButton button_ok = new com.oddlabs.tt.gui.OKButton(com.oddlabs.tt.form.MapcodeForm.BUTTON_WIDTH);
            button_ok.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_2, int unused_3, int unused_4, int unused_5) =>
            {
                done();
            }));
            com.oddlabs.tt.gui.HorizButton button_cancel = new com.oddlabs.tt.gui.CancelButton(com.oddlabs.tt.form.MapcodeForm.BUTTON_WIDTH);
            button_cancel.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_6, int unused_7, int unused_8, int unused_9) =>
            {
                this.cancel();
            }));
            com.oddlabs.tt.gui.HorizButton button_rand = new com.oddlabs.tt.gui.HorizButton(i18n("randomize"), com.oddlabs.tt.form.MapcodeForm.BUTTON_WIDTH);
            button_rand.addMouseClickListener(new com.oddlabs.tt.form.MapcodeForm.RandButtonListener(this));
            addChild(label_seed);
            addChild(editline_seed);
            addChild(button_ok);
            addChild(button_cancel);
            addChild(button_rand);
            label_seed.place();
            editline_seed.place(label_seed, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            button_cancel.place(editline_seed, com.oddlabs.tt.gui.Placement.BOTTOM_RIGHT);
            button_ok.place(button_cancel, com.oddlabs.tt.gui.Placement.LEFT_MID);
            button_rand.place(button_ok, com.oddlabs.tt.gui.Placement.LEFT_MID);
            compileCanvas();
            centerPos();
        }
    }
}
