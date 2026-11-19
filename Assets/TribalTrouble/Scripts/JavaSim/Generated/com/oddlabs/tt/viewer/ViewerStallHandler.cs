// Translated from ViewerStallHandler.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.viewer
{
    // Java: 9
    public class ViewerStallHandler : java.lang.Object, com.oddlabs.tt.net.StallHandler
    {
        public const float SHOW_WAITING_DELAY_SECONDS = 3.0f;
        public com.oddlabs.tt.viewer.WorldViewer viewer;
        public float local_stall_time;
        public int stall_tick;
        public com.oddlabs.tt.form.WaitingForPlayersForm waiting_for_players_form;
        // Java: 23
        public void resetStallTime()
        {
            local_stall_time = com.oddlabs.tt.@event.LocalEventQueue.getQueue().getTime();
        }
        // Java: 27
        public void stopStall()
        {
            if (waiting_for_players_form != null)
            {
                waiting_for_players_form.remove();
                waiting_for_players_form = null;
            }
        }
        // Java: 35
        public void peerhubFailed()
        {
            viewer.close();
        }
        // Java: 40
        public void processStall(int tick)
        {
            if (stall_tick != tick)
            {
                java.lang.IO.println("Stalled on tick " + (tick));
                stall_tick = tick;
                resetStallTime();
            }
            float elapsed_time = (float)(com.oddlabs.tt.@event.LocalEventQueue.getQueue().getTime() - local_stall_time);
            if (tick == 0 || elapsed_time > com.oddlabs.tt.viewer.ViewerStallHandler.SHOW_WAITING_DELAY_SECONDS)
            {
                if (waiting_for_players_form == null)
                {
                    waiting_for_players_form = new com.oddlabs.tt.form.WaitingForPlayersForm(viewer);
                    viewer.getGUIRoot().addModalForm(waiting_for_players_form);
                }
            }
        }
        public ViewerStallHandler(com.oddlabs.tt.viewer.WorldViewer viewer) : base()
        {
            this.viewer = viewer;
        }
    }
}
