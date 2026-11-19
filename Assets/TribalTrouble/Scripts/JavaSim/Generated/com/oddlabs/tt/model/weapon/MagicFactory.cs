// Translated from MagicFactory.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.weapon
{
    // Java: 5
    public interface MagicFactory
    {
        // Java: 6
        float getHitRadius();
        // Java: 8
        float getSecondsPerAnim();
        // Java: 10
        float getSecondsPerInit();
        // Java: 12
        float getSecondsPerRelease();
        // Java: 14
        com.oddlabs.tt.model.weapon.Magic execute(com.oddlabs.tt.model.Unit src);
    }
}
