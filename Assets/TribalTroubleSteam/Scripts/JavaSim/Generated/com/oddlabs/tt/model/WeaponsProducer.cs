// Translated from WeaponsProducer.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 13
    public class WeaponsProducer : java.lang.Object
    {
        public const float MAX_BREAK_TIME = 0.25f;
        public const float BREAK_PROBABILITY = 0.2f;
        public static java.util.Deque build_list;
        public com.oddlabs.tt.model.Building building;
        public com.oddlabs.tt.model.WorkerUnitContainer unit_container;
        public com.oddlabs.tt.model.BuildProductionContainer[] production_containers;
        public com.oddlabs.tt.particle.LinearEmitter emitter;
        public float break_time;
        public com.oddlabs.tt.audio.AbstractAudioPlayer production_player;
        // Java: 35
        public void animate(float t)
        {
            foreach (com.oddlabs.tt.model.BuildProductionContainer production_container in production_containers)
            {
                if (production_container.getNumSupplies() > 0 && production_container.hasEnoughSupplies())
                {
                    com.oddlabs.tt.model.WeaponsProducer.build_list.add(production_container);
                }
            }
            if (!com.oddlabs.tt.model.WeaponsProducer.build_list.isEmpty())
            {
                if (break_time <= 0)
                {
                    if (building.getOwner().getWorld().getRandom().nextFloat() < com.oddlabs.tt.model.WeaponsProducer.BREAK_PROBABILITY)
                    {
                        break_time = (float)(building.getOwner().getWorld().getRandom().nextFloat() * com.oddlabs.tt.model.WeaponsProducer.MAX_BREAK_TIME);
                        emitter.stop();
                    }
                    else
                    {
                        emitter.start();
                    }
                }
                startSound();
                float man_seconds_per_container = (float)((float)(unit_container.getNumSupplies() * t) / com.oddlabs.tt.model.WeaponsProducer.build_list.size());
                while (!com.oddlabs.tt.model.WeaponsProducer.build_list.isEmpty())
                {
                    ((com.oddlabs.tt.model.BuildProductionContainer)com.oddlabs.tt.model.WeaponsProducer.build_list.pop()).build(man_seconds_per_container);
                }
            }
            else
            {
                emitter.stop();
                stopSound();
            }
            break_time -= t;
        }
        // Java: 63
        public void startSound()
        {
            if (production_player == null)
            {
                production_player = building.getOwner().getWorld().getAudio().newAudio(new com.oddlabs.tt.audio.AudioParameters(building.getOwner().getWorld().getRacesResources().getArmorySound(), building.getPositionX(), building.getPositionY(), building.getPositionZ(), com.oddlabs.tt.audio.AudioPlayer.AUDIO_RANK_ARMORY, com.oddlabs.tt.audio.AudioPlayer.AUDIO_DISTANCE_ARMORY, com.oddlabs.tt.audio.AudioPlayer.AUDIO_GAIN_ARMORY, com.oddlabs.tt.audio.AudioPlayer.AUDIO_RADIUS_ARMORY, 1.0f, true, false));
            }
        }
        // Java: 76
        public void stopSound()
        {
            if (production_player != null)
            {
                production_player.stop();
                production_player = null;
            }
        }
        static WeaponsProducer()
        {
            WeaponsProducer.build_list = new java.util.ArrayDeque();
        }
        private void init_WeaponsProducer()
        {
            this.break_time = 0.0f;
        }
        public WeaponsProducer(com.oddlabs.tt.model.Building building, com.oddlabs.tt.model.WorkerUnitContainer unit_container, com.oddlabs.tt.model.BuildProductionContainer[] production_containers, com.oddlabs.tt.particle.LinearEmitter emitter) : base()
        {
            init_WeaponsProducer();
            this.building = building;
            this.unit_container = unit_container;
            this.production_containers = production_containers;
            this.emitter = emitter;
        }
    }
}
