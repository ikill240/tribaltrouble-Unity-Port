// Translated from ExtendedIcons.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 18
    public class ExtendedIcons : java.lang.Object
    {
        public static com.oddlabs.tt.gui.ExtendedIcons icons;
        public java.util.Map quads;
        public java.util.Map single;
        // Java: 39
        public static com.oddlabs.tt.gui.ExtendedIcons getIcons()
        {
            if (com.oddlabs.tt.gui.ExtendedIcons.icons == null)
            {
                com.oddlabs.tt.gui.ExtendedIcons.icons = new com.oddlabs.tt.gui.ExtendedIcons();
            }
            return com.oddlabs.tt.gui.ExtendedIcons.icons;
        }
        // Java: 46
        public com.oddlabs.tt.gui.IconQuad getStatus(string name)
        {
            com.oddlabs.tt.gui.IconQuad q = ((com.oddlabs.tt.gui.IconQuad)single.get(name));
            if (q == null)
            {
                throw new System.ArgumentException("No extended status icon " + java.lang.JStr.valueOf(name));
            }
            return q;
        }
        // Java: 54
        public com.oddlabs.tt.gui.IconQuad getStatus(bool vikings, string name)
        {
            com.oddlabs.tt.gui.IconQuad q = ((com.oddlabs.tt.gui.IconQuad)single.get(java.lang.JStr.valueOf(((vikings ? "vikings_" : "natives_"))) + java.lang.JStr.valueOf(name)));
            return (q != null ? q : getStatus(name));
        }
        // Java: 60
        public com.oddlabs.tt.gui.IconQuad getRareStatus(int rare_index)
        {
            return getStatus(java.lang.JStr.valueOf(com.oddlabs.tt.gui.ExtendedIcons.RARE_NAMES[rare_index]) + "_status_icon");
        }
        public static string[] RARE_NAMES;
        // Java: 67
        public com.oddlabs.tt.gui.ModeIconQuads find(string full_name)
        {
            return ((com.oddlabs.tt.gui.ModeIconQuads)quads.get(full_name));
        }
        // Java: 72
        public com.oddlabs.tt.gui.ModeIconQuads get(com.oddlabs.tt.model.Race race, string name)
        {
            bool vikings = ((object)race.getIcons() == (object)com.oddlabs.tt.gui.GUIIcons.getIcons().getVikingIcons());
            com.oddlabs.tt.gui.ModeIconQuads q = ((com.oddlabs.tt.gui.ModeIconQuads)quads.get(java.lang.JStr.valueOf(((vikings ? "vikings_" : "natives_"))) + java.lang.JStr.valueOf(name)));
            if (q == null)
            {
                q = ((com.oddlabs.tt.gui.ModeIconQuads)quads.get("natives_" + java.lang.JStr.valueOf(name)));
            }
            if (q == null)
            {
                throw new System.ArgumentException("No extended icon " + java.lang.JStr.valueOf(name));
            }
            return q;
        }
        static ExtendedIcons()
        {
            ExtendedIcons.RARE_NAMES = new string[] { "gold", "crystal", "herbs", "coal", "diamond" };
        }
        private void init_ExtendedIcons()
        {
            this.quads = new java.util.HashMap();
            this.single = new java.util.HashMap();
        }
        public ExtendedIcons() : base()
        {
            init_ExtendedIcons();
            org.w3c.dom.Node root = com.oddlabs.tt.gui.Icons.loadFile("/gui/icons_extended.xml", new com.oddlabs.tt.gui.GUIErrorHandler());
            com.oddlabs.tt.render.Texture texture = com.oddlabs.tt.gui.Icons.loadTexture(root);
            org.w3c.dom.NodeList nl = root.getChildNodes();
            for (int i = 0; i < nl.getLength(); i++)
            {
                org.w3c.dom.Node n = nl.item(i);
                if ((n is org.w3c.dom.Element))
                {
                    if (n.getNodeName().endsWith("_status_icon"))
                    {
                        single.put(n.getNodeName(), com.oddlabs.tt.gui.Icons.getIconQuad(n, texture));
                    }
                    else
                    {
                        quads.put(n.getNodeName(), com.oddlabs.tt.gui.Icons.getIconQuads(n, texture));
                    }
                }
            }
        }
    }
}
