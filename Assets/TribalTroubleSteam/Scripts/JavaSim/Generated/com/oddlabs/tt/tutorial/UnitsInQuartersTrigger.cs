// Translated from UnitsInQuartersTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.tutorial
{
    // Java: 6
    public class UnitsInQuartersTrigger : com.oddlabs.tt.tutorial.TutorialTrigger
    {
        // Java: 11
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
            if (count == 0)
            {
                tutorial.next(new com.oddlabs.tt.tutorial.RallyPointTrigger());
            }
        }
        public UnitsInQuartersTrigger() : base(1.0f, 0.0f, "units_in_quarters")
        {
        }
    }
}
