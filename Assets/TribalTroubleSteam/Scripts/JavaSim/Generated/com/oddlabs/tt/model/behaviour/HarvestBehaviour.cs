// Translated from HarvestBehaviour.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.behaviour
{
    // Java: 9
    public class HarvestBehaviour : java.lang.Object, com.oddlabs.tt.model.behaviour.Behaviour
    {
        public const float SECONDS_PER_ANIMATION_CYCLE = 1.0f;
        public com.oddlabs.tt.model.Supply supply;
        public com.oddlabs.tt.model.Unit unit;
        public float anim_time;
        public bool sound;
        // Java: 23
        public bool isBlocking()
        {
            return true;
        }
        // Java: 28
        public com.oddlabs.tt.model.behaviour.Behaviour.State animate(float t)
        {
            anim_time += t;
            if (anim_time > unit.getWeaponFactory().getSecondsPerRelease((float)(1.0f / com.oddlabs.tt.model.behaviour.HarvestBehaviour.SECONDS_PER_ANIMATION_CYCLE)) && !sound)
            {
                sound = true;
                unit.getOwner().getWorld().getAudio().newAudio(new com.oddlabs.tt.audio.AudioParameters(unit.getOwner().getWorld().getRacesResources().getHarvestSound(supply.GetType(), unit.getOwner().getWorld().getRandom()), unit.getPositionX(), unit.getPositionY(), unit.getPositionZ(), com.oddlabs.tt.audio.AudioPlayer.AUDIO_RANK_HARVEST, com.oddlabs.tt.audio.AudioPlayer.AUDIO_DISTANCE_HARVEST, com.oddlabs.tt.audio.AudioPlayer.AUDIO_GAIN_HARVEST, com.oddlabs.tt.audio.AudioPlayer.AUDIO_RADIUS_HARVEST));
                if (supply.hit())
                {
                    unit.getSupplyContainer().increaseSupply(1, supply.GetType());
                    unit.getOwner().harvested(supply.GetType());
                }
            }
            if (anim_time > com.oddlabs.tt.model.behaviour.HarvestBehaviour.SECONDS_PER_ANIMATION_CYCLE)
            {
                restartAnimation();
                if (unit.getSupplyContainer().isSupplyFull() || supply.isEmpty())
                {
                    return com.oddlabs.tt.model.behaviour.Behaviour.State.DONE;
                }
            }
            return com.oddlabs.tt.model.behaviour.Behaviour.State.INTERRUPTIBLE;
        }
        // Java: 56
        public void restartAnimation()
        {
            unit.switchAnimation((float)(1.0f / com.oddlabs.tt.model.behaviour.HarvestBehaviour.SECONDS_PER_ANIMATION_CYCLE), com.oddlabs.tt.model.Unit.Animation.THROWING);
            anim_time = 0;
            sound = false;
        }
        // Java: 62
        public void forceInterrupted()
        {
        }
        public HarvestBehaviour(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.model.Supply supply) : base()
        {
            this.unit = unit;
            this.supply = supply;
            unit.aimAtTarget(supply);
            restartAnimation();
        }
    }
}
