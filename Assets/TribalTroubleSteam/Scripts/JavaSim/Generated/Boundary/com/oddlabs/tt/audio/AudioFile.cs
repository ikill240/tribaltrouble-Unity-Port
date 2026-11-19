// Hand-written replacement: an AudioFile resolves to a SoundRef carrying its location, so the Unity
// audio layer can play the matching clip.
#pragma warning disable
namespace com.oddlabs.tt.audio
{
    public sealed class SoundRef : java.lang.Object, com.oddlabs.tt.audio.Audio
    {
        public readonly string location;
        public SoundRef(string location) { this.location = location; }
        public override string toString() => location;
    }

    public class AudioFile : com.oddlabs.tt.resource.File
    {
        readonly string location;
        public AudioFile(string p0) : base(p0) { location = p0; }
        public AudioFile() : base(default(java.net.URI)) { }
        public override object get() { return new SoundRef(location); }
    }
}
