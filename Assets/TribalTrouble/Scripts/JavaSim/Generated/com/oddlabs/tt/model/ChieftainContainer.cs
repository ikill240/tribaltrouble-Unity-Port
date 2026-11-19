// Translated from ChieftainContainer.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 3
    public class ChieftainContainer : java.lang.Object
    {
        public const float PROGRESS_AMOUNT = 0.025f;
        public com.oddlabs.tt.model.Building building;
        public bool training;
        public float progress_f;
        // Java: 15
        public float getBuildProgress()
        {
            return progress_f;
        }
        // Java: 19
        public void startTraining()
        {
            training = true;
            (((com.oddlabs.tt.model.ReproduceUnitContainer)building.getUnitContainer())).resetProgress();
        }
        // Java: 24
        public void stopTraining()
        {
            training = false;
            progress_f = 0.0f;
            (((com.oddlabs.tt.model.ReproduceUnitContainer)building.getUnitContainer())).resetProgress();
        }
        // Java: 30
        public bool isTraining()
        {
            return training;
        }
        // Java: 34
        public void progress()
        {
            progress_f += com.oddlabs.tt.model.ChieftainContainer.PROGRESS_AMOUNT;
            if (progress_f >= 1.0f)
            {
                building.deployChieftain();
            }
        }
        private void init_ChieftainContainer()
        {
            this.training = false;
            this.progress_f = 0.0f;
        }
        public ChieftainContainer(com.oddlabs.tt.model.Building building) : base()
        {
            init_ChieftainContainer();
            this.building = building;
        }
    }
}
