// Hand-written replacement of the generated stub: sounds the game starts go to the Unity presentation (SimHooks.Sound).
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.audio
{
    public class AudioManager : java.lang.Object, com.oddlabs.tt.landscape.AudioImplementation
    {
        static readonly AudioManager instance = new AudioManager();
        public static com.oddlabs.tt.audio.AudioManager getManager() { return instance; }
        public virtual com.oddlabs.tt.audio.AbstractAudioPlayer newAudio(com.oddlabs.tt.camera.CameraState p0, com.oddlabs.tt.audio.AudioParameters p1) => newAudio(p1);
        public virtual com.oddlabs.tt.audio.AbstractAudioPlayer newAudio(com.oddlabs.tt.audio.AudioParameters p0)
        {
            if (p0 != null) TribalTrouble.Bridge.SimHooks.Sound?.Invoke(p0);
            return new com.oddlabs.tt.audio.AbstractAudioPlayer();
        }
        public AudioManager() { }
        public virtual void close() { }
        // ---- merged from the generated stub (members the translated code now uses)
        public virtual void startSources() { }
        public virtual void stopSources() { }
        public virtual bool isHRTFSupported() { return default; }
        public AudioManager(com.oddlabs.tt.audio.AudioSource[] p0) { }
        public virtual com.oddlabs.tt.audio.AudioManager masterGain(float p0) { return new com.oddlabs.tt.audio.AudioManager(); }
        public virtual com.oddlabs.tt.audio.AudioManager updateOrientation(java.nio.FloatBuffer p0) { return new com.oddlabs.tt.audio.AudioManager(); }
        public virtual com.oddlabs.tt.audio.AudioManager updatePosition(float p0, float p1, float p2) { return new com.oddlabs.tt.audio.AudioManager(); }
        public virtual float[] getListenerPosition() { return new float[0]; }
        public virtual com.oddlabs.tt.audio.Audio createAudio(java.net.URL p0) { return default; }
    }
}
