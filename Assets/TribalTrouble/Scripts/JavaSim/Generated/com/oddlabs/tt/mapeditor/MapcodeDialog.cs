// Translated from MapcodeDialog.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.mapeditor
{
    // Java: 24
    public class MapcodeDialog : com.oddlabs.tt.gui.Form
    {
        public const int BUTTON_WIDTH = 100;
        public java.util.function.Predicate apply;
        public com.oddlabs.tt.gui.EditLine editline_code;
        // Java: 61
        public override void setFocus(com.oddlabs.tt.gui.FocusDirection direction)
        {
            if (((object)direction == (object)com.oddlabs.tt.gui.FocusDirection.BACKWARD))
            {
                base.setFocus(direction);
            }
            else
            {
                editline_code.setFocus(direction);
            }
        }
        // Java: 70
        public void randomize()
        {
            long tick = com.oddlabs.tt.@event.LocalEventQueue.getQueue().getHighPrecisionManager().getTick();
            java.util.Random random = new java.util.Random(tick * tick);
            random.nextInt();
            editline_code.clear();
            editline_code.append(com.oddlabs.tt.util.WordsEncoding.encode(new java.math.BigInteger(60, random)));
        }
        // Java: 78
        public void done()
        {
            if (apply.test(editline_code.getContents()))
            {
                remove();
            }
            else
            {
                editline_code.triggerError();
            }
        }
        public MapcodeDialog(string current_code, java.util.function.Predicate apply) : base()
        {
            this.apply = apply;
            com.oddlabs.tt.gui.Label label_code = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.mapeditor.MapEditor.terrainI18n("map_code"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            editline_code = new com.oddlabs.tt.gui.EditLine(400, 100);
            editline_code.append(current_code);
            editline_code.addEnterListener(new com.oddlabs.tt.guievent.EnterListener_L((string unused) =>
            {
                done();
            }));
            com.oddlabs.tt.gui.HorizButton button_ok = new com.oddlabs.tt.gui.OKButton(com.oddlabs.tt.mapeditor.MapcodeDialog.BUTTON_WIDTH);
            button_ok.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_2, int unused_3, int unused_4, int unused_5) =>
            {
                done();
            }));
            com.oddlabs.tt.gui.HorizButton button_cancel = new com.oddlabs.tt.gui.CancelButton(com.oddlabs.tt.mapeditor.MapcodeDialog.BUTTON_WIDTH);
            button_cancel.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_6, int unused_7, int unused_8, int unused_9) =>
            {
                cancel();
            }));
            com.oddlabs.tt.gui.HorizButton button_random = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.mapeditor.MapEditor.mapcodeI18n("randomize"), com.oddlabs.tt.mapeditor.MapcodeDialog.BUTTON_WIDTH);
            button_random.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_10, int unused_11, int unused_12, int unused_13) =>
            {
                randomize();
            }));
            addChild(label_code);
            addChild(editline_code);
            addChild(button_ok);
            addChild(button_cancel);
            addChild(button_random);
            label_code.place();
            editline_code.place(label_code, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            button_cancel.place(editline_code, com.oddlabs.tt.gui.Placement.BOTTOM_RIGHT);
            button_ok.place(button_cancel, com.oddlabs.tt.gui.Placement.LEFT_MID);
            button_random.place(button_ok, com.oddlabs.tt.gui.Placement.LEFT_MID);
            compileCanvas();
            centerPos();
        }
    }
}
