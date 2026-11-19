// Translated from AbstractOptionsMenu.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 20
    public abstract class AbstractOptionsMenu : com.oddlabs.tt.gui.Form
    {
        public const int BUTTON_WIDTH = 100;
        public static java.util.ResourceBundle bundle;
        // Java: 24
        public static string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.form.AbstractOptionsMenu.bundle, key, args);
        }
        public com.oddlabs.tt.form.GeneralPanel generalPanel;
        public com.oddlabs.tt.form.GraphicsPanel graphicsPanel;
        // Java: 71
        public override void displayChangedNotify(int width, int height)
        {
            base.displayChangedNotify(width, height);
            centerPos();
            graphicsPanel.updateScaleLabel();
        }
        // Java: 78
        public void chooseGamespeed(int speed)
        {
            generalPanel.chooseGamespeed(speed);
        }
        // Java: 82
        public virtual void changeGamespeed(int index)
        {
            com.oddlabs.tt.global.Globals.gamespeed = index;
            com.oddlabs.tt.global.Settings.getSettings().gamespeed = index;
        }
        static AbstractOptionsMenu()
        {
            AbstractOptionsMenu.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.form.OptionsMenu).getName());
        }
        public AbstractOptionsMenu(com.oddlabs.tt.gui.GUIRoot gui_root) : base()
        {
            com.oddlabs.tt.gui.Label label_headline = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("options_caption"), com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont());
            addChild(label_headline);
            generalPanel = new com.oddlabs.tt.form.GeneralPanel(gui_root, new java.util.function.IntConsumer_L((int a0) => { this.changeGamespeed(a0); }));
            graphicsPanel = new com.oddlabs.tt.form.GraphicsPanel(gui_root, this);
            com.oddlabs.tt.gui.PanelGroup panel_group = new com.oddlabs.tt.gui.PanelGroup(generalPanel, graphicsPanel, new com.oddlabs.tt.form.KeyBindingPanel(gui_root), new com.oddlabs.tt.form.AccessibilityPanel(gui_root), new com.oddlabs.tt.form.SoundPanel(gui_root), new com.oddlabs.tt.form.LanguagePanel(gui_root));
            addChild(panel_group);
            com.oddlabs.tt.gui.HorizButton button_close = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("close"), com.oddlabs.tt.form.AbstractOptionsMenu.BUTTON_WIDTH);
            button_close.addMouseClickListener(new com.oddlabs.tt.gui.CancelListener(this));
            addChild(button_close);
            com.oddlabs.tt.gui.HorizButton button_about = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.form.AbstractOptionsMenu.i18n("about"), com.oddlabs.tt.form.AbstractOptionsMenu.BUTTON_WIDTH);
            button_about.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                gui_root.addModalForm(new com.oddlabs.tt.form.CreditsForm());
            }));
            addChild(button_about);
            addCloseListener(new com.oddlabs.tt.guievent.CloseListener_L(() =>
            {
                com.oddlabs.tt.global.Settings.getSettings().save();
            }));
            label_headline.place();
            panel_group.place(label_headline, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            button_close.place(com.oddlabs.tt.gui.Origin.AT_END);
            button_about.place(button_close, com.oddlabs.tt.gui.Placement.LEFT_MID);
            com.oddlabs.tt.gui.HorizButton button_engine = new com.oddlabs.tt.gui.HorizButton("Modern graphics...", com.oddlabs.tt.form.AbstractOptionsMenu.BUTTON_WIDTH * 2);
            button_engine.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_5, int unused_6, int unused_7, int unused_8) =>
            {
                com.oddlabs.tt.util.PortHost.openEngineSettings();
            }));
            addChild(button_engine);
            button_engine.place(button_about, com.oddlabs.tt.gui.Placement.LEFT_MID);
            compileCanvas();
        }
    }
}
