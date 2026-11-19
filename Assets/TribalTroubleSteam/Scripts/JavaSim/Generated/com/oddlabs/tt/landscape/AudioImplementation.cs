// Translated from AudioImplementation.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.landscape
{
    // Java: 6
    public interface AudioImplementation
    {
        // Java: 8
        com.oddlabs.tt.audio.AbstractAudioPlayer newAudio(com.oddlabs.tt.audio.AudioParameters @params);
    }
    public sealed class AudioImplementation_L : java.lang.Object, AudioImplementation
    {
        readonly System.Func<com.oddlabs.tt.audio.AudioParameters, com.oddlabs.tt.audio.AbstractAudioPlayer> f;
        public AudioImplementation_L(System.Func<com.oddlabs.tt.audio.AudioParameters, com.oddlabs.tt.audio.AbstractAudioPlayer> f) { this.f = f; }
        public com.oddlabs.tt.audio.AbstractAudioPlayer newAudio(com.oddlabs.tt.audio.AudioParameters p0) { return f(p0); }
    }
}
