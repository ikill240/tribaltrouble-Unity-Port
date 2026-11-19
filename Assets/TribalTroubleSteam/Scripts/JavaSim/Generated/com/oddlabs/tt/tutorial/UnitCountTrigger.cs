// Translated from UnitCountTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.tutorial
{
    // Java: 7
    public class UnitCountTrigger : com.oddlabs.tt.tutorial.TutorialTrigger
    {
        public int target_count;
        // Java: 15
        public override void run(com.oddlabs.tt.tutorial.Tutorial tutorial)
        {
            java.util.Set set = tutorial.getViewer().getLocalPlayer().getUnits().getSet();
            java.util.Iterator it = set.iterator();
            int count = 0;
            while (it.hasNext())
            {
                com.oddlabs.tt.model.Selectable s = ((com.oddlabs.tt.model.Selectable)it.next());
                if ((s is com.oddlabs.tt.model.Unit))
                {
                    count++;
                }
            }
            if (count >= target_count)
            {
                tutorial.done(com.oddlabs.tt.form.TutorialForm.TUTORIAL_QUARTERS);
            }
        }
        public UnitCountTrigger(int target_count) : base(1.0f, 0.0f, "unit_count", new object[] { target_count })
        {
            this.target_count = target_count;
        }
    }
}
