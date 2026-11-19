// Translated from GUIErrorHandler.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 10
    public class GUIErrorHandler : java.lang.Object, org.xml.sax.ErrorHandler
    {
        public static java.util.logging.Logger logger;
        // Java: 13
        public void fatalError(org.xml.sax.SAXParseException e)
        {
            com.oddlabs.tt.gui.GUIErrorHandler.logger.log(java.util.logging.Level.SEVERE, "fatal line " + (e.getLineNumber()) + ", uri " + java.lang.JStr.valueOf(e.getSystemId()), e);
        }
        // Java: 20
        public void error(org.xml.sax.SAXParseException e)
        {
            com.oddlabs.tt.gui.GUIErrorHandler.logger.log(java.util.logging.Level.SEVERE, "error line " + (e.getLineNumber()) + ", uri " + java.lang.JStr.valueOf(e.getSystemId()), e);
            throw e;
        }
        // Java: 27
        public void warning(org.xml.sax.SAXParseException err)
        {
            com.oddlabs.tt.gui.GUIErrorHandler.logger.log(java.util.logging.Level.WARNING, "line " + (err.getLineNumber()) + ", uri " + java.lang.JStr.valueOf(err.getSystemId()), err);
        }
        static GUIErrorHandler()
        {
            GUIErrorHandler.logger = java.util.logging.Logger.getLogger("SAXParseError");
        }
        public GUIErrorHandler() : base()
        {
        }
    }
}
