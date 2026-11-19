// Translated from IllegalARMIEventException.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.net
{
    // Java: 5
    public class IllegalARMIEventException : System.Exception
    {
        public const long serialVersionUID = 6874182030169648695L;
        public IllegalARMIEventException(string message) : base(message)
        {
        }
        public IllegalARMIEventException(System.Exception e) : base(J2CS.Ext.JExc.CauseMessage(e), e)
        {
        }
    }
}
