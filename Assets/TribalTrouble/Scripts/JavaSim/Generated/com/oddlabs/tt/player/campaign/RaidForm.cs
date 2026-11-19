// Translated from RaidForm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.campaign
{
    // Java: 19
    public class RaidForm : com.oddlabs.tt.gui.Form
    {
        public const int BUTTON_WIDTH = 360;
        // Java: 88
        public com.oddlabs.tt.gui.HorizButton button(string caption, java.lang.Runnable action)
        {
            com.oddlabs.tt.gui.HorizButton b = new com.oddlabs.tt.gui.HorizButton(caption, com.oddlabs.tt.player.campaign.RaidForm.BUTTON_WIDTH);
            b.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                action.run();
            }));
            addChild(b);
            return b;
        }
        // Java: 95
        public override void doCancel()
        {
        }
        public RaidForm(com.oddlabs.tt.player.campaign.SagaCampaign campaign, com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root, int island, int strength, bool ally_isle) : base()
        {
            if (ally_isle)
            {
                com.oddlabs.tt.gui.Label headline = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.player.campaign.SagaIsland.i18n("raid_header"), com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont());
                addChild(headline);
                com.oddlabs.tt.gui.LabelBox text = new com.oddlabs.tt.gui.LabelBox(com.oddlabs.tt.player.campaign.SagaIsland.i18n("ally_raid_text", campaign.islandName(island), strength), com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), com.oddlabs.tt.player.campaign.RaidForm.BUTTON_WIDTH + 40);
                addChild(text);
                headline.place();
                text.place(headline, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
                com.oddlabs.tt.gui.HorizButton help = button(com.oddlabs.tt.player.campaign.SagaIsland.i18n("ally_raid_help"), new java.lang.Runnable_L(() =>
                {
                    remove();
                    campaign.defend(network, gui_root, island, com.oddlabs.tt.player.campaign.SagaCampaign.DEFEND_HELP);
                }));
                help.place(text, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
                com.oddlabs.tt.gui.HorizButton leave = button(com.oddlabs.tt.player.campaign.SagaIsland.i18n("ally_raid_leave"), new java.lang.Runnable_L(() =>
                {
                    remove();
                    campaign.resolve(gui_root, island, strength, false);
                }));
                leave.place(help, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
                compileCanvas();
                centerPos();
                return;
            }
            int allies = campaign.getState().getSagaAllies();
            com.oddlabs.tt.gui.Label headline_2 = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.player.campaign.SagaIsland.i18n("raid_header"), com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont());
            addChild(headline_2);
            com.oddlabs.tt.gui.LabelBox text_2 = new com.oddlabs.tt.gui.LabelBox(com.oddlabs.tt.player.campaign.SagaIsland.i18n("raid_text", campaign.islandName(island), strength, com.oddlabs.tt.player.campaign.SagaIsland.i18n("raid_allies", allies)), com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), com.oddlabs.tt.player.campaign.RaidForm.BUTTON_WIDTH + 40);
            addChild(text_2);
            headline_2.place();
            text_2.place(headline_2, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            com.oddlabs.tt.gui.HorizButton defend = button(com.oddlabs.tt.player.campaign.SagaIsland.i18n("raid_defend"), new java.lang.Runnable_L(() =>
            {
                remove();
                campaign.defend(network, gui_root, island, com.oddlabs.tt.player.campaign.SagaCampaign.DEFEND_SELF);
            }));
            defend.place(text_2, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            com.oddlabs.tt.gui.HorizButton chief = button(com.oddlabs.tt.player.campaign.SagaIsland.i18n("raid_chief_watch"), new java.lang.Runnable_L(() =>
            {
                remove();
                campaign.defend(network, gui_root, island, com.oddlabs.tt.player.campaign.SagaCampaign.DEFEND_AUTO);
            }));
            chief.place(defend, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            com.oddlabs.tt.gui.HorizButton last = chief;
            if (allies > 0)
            {
                com.oddlabs.tt.gui.HorizButton watch = button(com.oddlabs.tt.player.campaign.SagaIsland.i18n("raid_ally_watch"), new java.lang.Runnable_L(() =>
                {
                    remove();
                    campaign.defend(network, gui_root, island, com.oddlabs.tt.player.campaign.SagaCampaign.DEFEND_ALLY);
                }));
                watch.place(last, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
                com.oddlabs.tt.gui.HorizButton auto = button(com.oddlabs.tt.player.campaign.SagaIsland.i18n("raid_ally_auto"), new java.lang.Runnable_L(() =>
                {
                    remove();
                    campaign.resolve(gui_root, island, strength, true);
                }));
                auto.place(watch, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
                last = auto;
            }
            com.oddlabs.tt.gui.HorizButton leave_2 = button(com.oddlabs.tt.player.campaign.SagaIsland.i18n("raid_leave"), new java.lang.Runnable_L(() =>
            {
                remove();
                campaign.resolve(gui_root, island, strength, false);
            }));
            leave_2.place(last, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            compileCanvas();
            centerPos();
        }
    }
}
