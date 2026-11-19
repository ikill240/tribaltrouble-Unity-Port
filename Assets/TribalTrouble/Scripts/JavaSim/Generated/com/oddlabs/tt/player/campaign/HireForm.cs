// Translated from HireForm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.campaign
{
    // Java: 19
    public class HireForm : com.oddlabs.tt.gui.Form
    {
        public const int BUTTON_WIDTH = 360;
        public HireForm(com.oddlabs.tt.player.campaign.SagaCampaign campaign, com.oddlabs.tt.gui.GUIRoot gui_root, java.lang.Runnable next) : base()
        {
            com.oddlabs.tt.player.campaign.CampaignState state = campaign.getState();
            int fee = campaign.hireFee();
            int gold = state.getSagaStock()[com.oddlabs.tt.model.ExtendedResources.GOLD];
            com.oddlabs.tt.gui.Label headline = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.player.campaign.SagaIsland.i18n("hire_header"), com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont());
            addChild(headline);
            com.oddlabs.tt.gui.LabelBox text = new com.oddlabs.tt.gui.LabelBox(com.oddlabs.tt.player.campaign.SagaIsland.i18n("hire_text", fee, gold, state.getSagaFriendship()), com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), com.oddlabs.tt.player.campaign.HireForm.BUTTON_WIDTH + 40);
            addChild(text);
            headline.place();
            text.place(headline, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            com.oddlabs.tt.gui.HorizButton hire = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.player.campaign.SagaIsland.i18n("hire_yes", fee), com.oddlabs.tt.player.campaign.HireForm.BUTTON_WIDTH);
            hire.setDisabled(gold < fee);
            hire.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                if (state.getSagaStock()[com.oddlabs.tt.model.ExtendedResources.GOLD] < fee)
                {
                    return;
                }
                state.getSagaStock()[com.oddlabs.tt.model.ExtendedResources.GOLD] -= fee;
                state.setSagaHired(true);
                state.addSagaFriendship(1);
                remove();
                next.run();
            }));
            addChild(hire);
            hire.place(text, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            com.oddlabs.tt.gui.HorizButton no = new com.oddlabs.tt.gui.HorizButton(com.oddlabs.tt.player.campaign.SagaIsland.i18n("hire_no"), com.oddlabs.tt.player.campaign.HireForm.BUTTON_WIDTH);
            no.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_5, int unused_6, int unused_7, int unused_8) =>
            {
                state.setSagaHired(false);
                remove();
                next.run();
            }));
            addChild(no);
            no.place(hire, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            compileCanvas();
            centerPos();
        }
    }
}
