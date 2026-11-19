// Translated from ExpeditionForm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.campaign
{
    // Java: 25
    public class ExpeditionForm : com.oddlabs.tt.gui.Form
    {
        public static int[][] WARRIOR_PERCENT;
        public static int[][] PEON_PERCENT;
        public static int[][] STOCK_PERCENT;
        public const int SLIDER_WIDTH = 220;
        public com.oddlabs.tt.player.campaign.CampaignState state;
        public com.oddlabs.tt.gui.Slider peons;
        public com.oddlabs.tt.gui.Slider warriors;
        public com.oddlabs.tt.gui.Slider supplies;
        // Java: 114
        public static int share(int n, int percent)
        {
            return (n <= 0 ? 0 : (n * percent + 99) / 100);
        }
        // Java: 118
        public void choose(int peon_count, int warrior_count, int pct)
        {
            int[] home = state.getSagaUnits();
            int[] stock = state.getSagaStock();
            state.clearSagaExpedition();
            int[] exp = state.getSagaExpedition();
            exp[com.oddlabs.tt.model.Race.UNIT_PEON] = java.lang.Math.min(peon_count, home[com.oddlabs.tt.model.Race.UNIT_PEON]);
            int left = warrior_count;
            for (int u = home.Length - 1; u >= 0 && left > 0; u--)
            {
                if (com.oddlabs.tt.player.campaign.SagaIsland.isWarrior(u))
                {
                    int n = java.lang.Math.min(left, home[u]);
                    exp[u] = n;
                    left -= n;
                }
            }
            for (int i = 0; i < com.oddlabs.tt.player.campaign.CampaignState.SAGA_STOCKS; i++)
            {
                exp[com.oddlabs.tt.player.campaign.CampaignState.SAGA_UNITS + i] = stock[i] * pct / 100;
            }
        }
        static ExpeditionForm()
        {
            ExpeditionForm.WARRIOR_PERCENT = new int[][] { new int[] { 6, 9, 12, 15 }, new int[] { 8, 12, 16, 20 }, new int[] { 8, 12, 16, 20 } };
            ExpeditionForm.PEON_PERCENT = new int[][] { new int[] { 10, 15, 20, 25 }, new int[] { 15, 20, 25, 30 }, new int[] { 15, 20, 25, 30 } };
            ExpeditionForm.STOCK_PERCENT = new int[][] { new int[] { 12, 18, 24, 30 }, new int[] { 16, 24, 32, 40 }, new int[] { 20, 30, 40, 50 } };
        }
        public ExpeditionForm(com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.player.campaign.CampaignState state, int page, java.lang.Runnable sail) : base()
        {
            int pg = java.lang.Math.clamp(page, 0, 2);
            this.state = state;
            int[] home = state.getSagaUnits();
            int home_warriors = 0;
            for (int u = 0; u < home.Length; u++)
            {
                if (com.oddlabs.tt.player.campaign.SagaIsland.isWarrior(u))
                {
                    home_warriors += home[u];
                }
            }
            int cargo = java.lang.Math.min(3, state.getSagaResearch()[com.oddlabs.tt.model.ResearchContainer.CARGO]);
            int max_peons = com.oddlabs.tt.player.campaign.ExpeditionForm.share(home[com.oddlabs.tt.model.Race.UNIT_PEON], com.oddlabs.tt.player.campaign.ExpeditionForm.PEON_PERCENT[pg][cargo]);
            int max_warriors = com.oddlabs.tt.player.campaign.ExpeditionForm.share(home_warriors, com.oddlabs.tt.player.campaign.ExpeditionForm.WARRIOR_PERCENT[pg][cargo]);
            int stock_cap = com.oddlabs.tt.player.campaign.ExpeditionForm.STOCK_PERCENT[pg][cargo];
            com.oddlabs.tt.gui.Label headline = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.player.campaign.SagaIsland.i18n("exp_caption"), com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont());
            addChild(headline);
            com.oddlabs.tt.gui.Label info = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.player.campaign.SagaIsland.i18n("exp_info", home[com.oddlabs.tt.model.Race.UNIT_PEON], home_warriors, max_peons, max_warriors, stock_cap), com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            addChild(info);
            com.oddlabs.tt.gui.Group group = new com.oddlabs.tt.gui.Group();
            peons = new com.oddlabs.tt.gui.Slider(com.oddlabs.tt.player.campaign.ExpeditionForm.SLIDER_WIDTH, 0, java.lang.Math.max(1, max_peons), max_peons);
            warriors = new com.oddlabs.tt.gui.Slider(com.oddlabs.tt.player.campaign.ExpeditionForm.SLIDER_WIDTH, 0, java.lang.Math.max(1, max_warriors), max_warriors);
            supplies = new com.oddlabs.tt.gui.Slider(com.oddlabs.tt.player.campaign.ExpeditionForm.SLIDER_WIDTH, 0, java.lang.Math.max(1, stock_cap), stock_cap);
            peons.setDisabled(max_peons == 0);
            warriors.setDisabled(max_warriors == 0);
            com.oddlabs.tt.gui.Label peons_label = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.player.campaign.SagaIsland.i18n("exp_peons"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), 100);
            com.oddlabs.tt.gui.Label warriors_label = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.player.campaign.SagaIsland.i18n("exp_warriors"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), 100);
            com.oddlabs.tt.gui.Label supplies_label = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.player.campaign.SagaIsland.i18n("exp_supplies"), com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), 100);
            com.oddlabs.tt.gui.Label peons_value = new com.oddlabs.tt.gui.Label(java.lang.JString.valueOf(max_peons), com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), 60);
            com.oddlabs.tt.gui.Label warriors_value = new com.oddlabs.tt.gui.Label(java.lang.JString.valueOf(max_warriors), com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), 60);
            com.oddlabs.tt.gui.Label supplies_value = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.player.campaign.SagaIsland.i18n("exp_percent", stock_cap), com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), 60);
            peons.addValueListener(new com.oddlabs.tt.guievent.ValueListener_L((long v) =>
            {
                peons_value.setText(java.lang.JString.valueOf(java.lang.Math.min(v, max_peons)));
            }));
            warriors.addValueListener(new com.oddlabs.tt.guievent.ValueListener_L((long v_2) =>
            {
                warriors_value.setText(java.lang.JString.valueOf(java.lang.Math.min(v_2, max_warriors)));
            }));
            supplies.addValueListener(new com.oddlabs.tt.guievent.ValueListener_L((long v_3) =>
            {
                supplies_value.setText(com.oddlabs.tt.player.campaign.SagaIsland.i18n("exp_percent", java.lang.Math.min(v_3, stock_cap)));
            }));
            foreach (com.oddlabs.tt.gui.GUIObject o in new com.oddlabs.tt.gui.GUIObject[] { peons_label, peons, peons_value, warriors_label, warriors, warriors_value, supplies_label, supplies, supplies_value })
            {
                group.addChild(o);
            }
            peons_label.place();
            peons.place(peons_label, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            peons_value.place(peons, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            warriors_label.place(peons_label, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            warriors.place(warriors_label, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            warriors_value.place(warriors, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            supplies_label.place(warriors_label, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            supplies.place(supplies_label, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            supplies_value.place(supplies, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            group.compileCanvas();
            addChild(group);
            int peon_cap = max_peons;
            int warrior_cap = max_warriors;
            int stock_max = stock_cap;
            com.oddlabs.tt.gui.ButtonObject ok = new com.oddlabs.tt.gui.OKButton(100);
            ok.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                choose(java.lang.Math.min(peons.getValue(), peon_cap), java.lang.Math.min(warriors.getValue(), warrior_cap), java.lang.Math.min(supplies.getValue(), stock_max));
                remove();
                sail.run();
            }));
            addChild(ok);
            com.oddlabs.tt.gui.ButtonObject cancel_v = new com.oddlabs.tt.gui.CancelButton(100);
            cancel_v.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused_5, int unused_6, int unused_7, int unused_8) =>
            {
                cancel();
            }));
            addChild(cancel_v);
            headline.place();
            info.place(headline, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            group.place(info, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            cancel_v.place(com.oddlabs.tt.gui.Origin.AT_END);
            ok.place(cancel_v, com.oddlabs.tt.gui.Placement.LEFT_MID);
            compileCanvas();
            centerPos();
        }
    }
}
