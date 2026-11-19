// Hand-written replacement: keeps the sound request so the Unity audio layer can play it.
#pragma warning disable
namespace com.oddlabs.tt.audio
{
    public class AudioParameters : java.lang.Object
    {
        public readonly object sound;
        public readonly float x, y, z, distance, gain = 1f, radius, pitch = 1f;
        public readonly int rank;
        public readonly bool looping, relative, music;
        public AudioParameters(object sound, float x, float y, float z, int rank, float distance, float gain, float radius)
        { this.sound = sound; this.x = x; this.y = y; this.z = z; this.rank = rank; this.distance = distance; this.gain = gain; this.radius = radius; }
        public AudioParameters(object sound, float x, float y, float z, int rank, float distance, float gain, float radius, float pitch)
            : this(sound, x, y, z, rank, distance, gain, radius) { this.pitch = pitch; }
        public AudioParameters(object sound, float x, float y, float z, int rank, float distance, float gain, float radius, float pitch, bool looping, bool relative)
            : this(sound, x, y, z, rank, distance, gain, radius, pitch) { this.looping = looping; this.relative = relative; }
        public AudioParameters() { }
    }
}
