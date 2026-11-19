// Translated from SecureConnectionInterface.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.net
{
    // Java: 5
    public interface SecureConnectionInterface
    {
        // Java: 6
        void initAgreement(sbyte[] encoded_public_key);
        // Java: 8
        void tunnelEvent(javax.crypto.SealedObject @event);
    }
}
