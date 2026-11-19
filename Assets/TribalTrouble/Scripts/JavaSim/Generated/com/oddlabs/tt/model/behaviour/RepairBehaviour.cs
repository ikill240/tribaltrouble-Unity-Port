// Translated from RepairBehaviour.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.behaviour
{
    // Java: 10
    public class RepairBehaviour : java.lang.Object, com.oddlabs.tt.model.behaviour.Behaviour
    {
        public const int REPAIRS_PER_SUPPLY = 5;
        public const float SECONDS_PER_ANIMATION_CYCLE = 1.0f;
        public com.oddlabs.tt.model.Building building;
        public com.oddlabs.tt.model.Unit unit;
        public float anim_time;
        public int repairs;
        public bool sound;
        // Java: 29
        public bool isBlocking()
        {
            return true;
        }
        // Java: 34
        public com.oddlabs.tt.model.behaviour.Behaviour.State animate(float t)
        {
            anim_time += t;
            if (anim_time > unit.getWeaponFactory().getSecondsPerRelease((float)(1.0f / com.oddlabs.tt.model.behaviour.RepairBehaviour.SECONDS_PER_ANIMATION_CYCLE)) && !sound)
            {
                sound = true;
                unit.getOwner().getWorld().getAudio().newAudio(new com.oddlabs.tt.audio.AudioParameters(unit.getOwner().getWorld().getRacesResources().getHarvestSound(typeof(com.oddlabs.tt.landscape.TreeSupply), unit.getOwner().getWorld().getRandom()), unit.getPositionX(), unit.getPositionY(), unit.getPositionZ(), com.oddlabs.tt.audio.AudioPlayer.AUDIO_RANK_HARVEST, com.oddlabs.tt.audio.AudioPlayer.AUDIO_DISTANCE_HARVEST, com.oddlabs.tt.audio.AudioPlayer.AUDIO_GAIN_HARVEST, com.oddlabs.tt.audio.AudioPlayer.AUDIO_RADIUS_HARVEST));
            }
            if (anim_time > com.oddlabs.tt.model.behaviour.RepairBehaviour.SECONDS_PER_ANIMATION_CYCLE)
            {
                restartAnimation();
                repairs++;
                if (building.isDead() || !building.isDamaged())
                {
                    return com.oddlabs.tt.model.behaviour.Behaviour.State.DONE;
                }
                else
                {
                    building.repair(1);
                }
            }
            return (repairs == com.oddlabs.tt.model.behaviour.RepairBehaviour.REPAIRS_PER_SUPPLY ? com.oddlabs.tt.model.behaviour.Behaviour.State.DONE : com.oddlabs.tt.model.behaviour.Behaviour.State.INTERRUPTIBLE);
        }
        // Java: 61
        public void restartAnimation()
        {
            anim_time = 0;
            sound = false;
            unit.switchAnimation((float)(1.0f / com.oddlabs.tt.model.behaviour.RepairBehaviour.SECONDS_PER_ANIMATION_CYCLE), com.oddlabs.tt.model.Unit.Animation.THROWING);
        }
        // Java: 67
        public void forceInterrupted()
        {
        }
        public RepairBehaviour(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.model.Building building) : base()
        {
            this.unit = unit;
            this.building = building;
            unit.aimAtTarget(building);
            restartAnimation();
            unit.getSupplyContainer().increaseSupply((-1), typeof(com.oddlabs.tt.landscape.TreeSupply));
            repairs = 0;
        }
    }
}
