// Translated from DefeatTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.trigger.campaign
{
    // Java: 13
    public class DefeatTrigger : com.oddlabs.tt.trigger.IntervalTrigger
    {
        public static java.util.ResourceBundle bundle;
        // Java: 16
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.trigger.campaign.DefeatTrigger.bundle, key, args);
        }
        public com.oddlabs.tt.player.campaign.Campaign campaign;
        public com.oddlabs.tt.model.Unit chieftain;
        public java.lang.Runnable runnable;
        public com.oddlabs.tt.viewer.WorldViewer viewer;
        public bool triggered_by_chieftain_death;
        // Java: 39
        public override void check()
        {
            com.oddlabs.tt.player.Player current = viewer.getLocalPlayer();
            if (((object)chieftain != (object)current.getChieftain()))
            {
                triggered_by_chieftain_death = true;
                triggered();
            }
            int units = current.getUnitCountContainer().getNumSupplies();
            if (units == 0 && !current.hasActiveChieftain())
            {
                triggered();
            }
        }
        // Java: 53
        public override void done()
        {
            if (runnable == null)
            {
                string game_over_message = i18n((triggered_by_chieftain_death ? "defeat_by_chieftain" : "defeat"));
                campaign.defeated(viewer, game_over_message);
            }
            else
            {
                runnable.run();
            }
        }
        static DefeatTrigger()
        {
            DefeatTrigger.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.trigger.campaign.DefeatTrigger).getName());
        }
        private void init_DefeatTrigger()
        {
            this.triggered_by_chieftain_death = false;
        }
        public DefeatTrigger(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.player.campaign.Campaign campaign, com.oddlabs.tt.model.Unit chieftain) : this(viewer, campaign, chieftain, null)
        {
        }
        public DefeatTrigger(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.player.campaign.Campaign campaign, com.oddlabs.tt.model.Unit chieftain, java.lang.Runnable runnable) : base(viewer.getWorld(), 0.5f, 0.0f)
        {
            init_DefeatTrigger();
            this.viewer = viewer;
            this.campaign = campaign;
            this.chieftain = chieftain;
            this.runnable = runnable;
        }
    }
}
