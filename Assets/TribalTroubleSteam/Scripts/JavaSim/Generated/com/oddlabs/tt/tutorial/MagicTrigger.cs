// Translated from MagicTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.tutorial
{
    // Java: 8
    public class MagicTrigger : com.oddlabs.tt.tutorial.TutorialTrigger
    {
        public bool[] magic_used;
        public com.oddlabs.tt.model.Unit chieftain;
        // Java: 18
        public override void run(com.oddlabs.tt.tutorial.Tutorial tutorial)
        {
            int last = chieftain.getLastMagicIndex();
            if (last != (-1))
            {
                magic_used[last] = true;
            }
            foreach (bool b in magic_used)
            {
                if (!b)
                {
                    return;
                }
            }
            tutorial.done(com.oddlabs.tt.form.TutorialForm.TUTORIAL_CHIEFTAIN);
        }
        private void init_MagicTrigger()
        {
            this.magic_used = new bool[com.oddlabs.tt.model.RacesResources.NUM_MAGIC];
        }
        public MagicTrigger(com.oddlabs.tt.model.Unit chieftain) : base(0.1f, 20.0f, "magic")
        {
            init_MagicTrigger();
            this.chieftain = chieftain;
        }
    }
}
