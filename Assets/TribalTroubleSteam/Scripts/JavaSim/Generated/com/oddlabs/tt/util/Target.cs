// Translated from Target.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.util
{
    // Java: 5
    public interface Target : com.oddlabs.tt.net.Distributable
    {
        // Java: 6
        int getGridX();
        // Java: 8
        int getGridY();
        // Java: 10
        float getPositionX();
        // Java: 12
        float getPositionY();
        // Java: 14
        float getSize();
        // Java: 16
        bool isDead();
        // Java: 18
        bool isAlive();
    }
}
