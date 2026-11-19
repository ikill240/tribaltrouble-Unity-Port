// Translated from Icons.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 27
    public class Icons : java.lang.Object
    {
        // Java: 32
        public static org.w3c.dom.Node loadFile(string xml_file, org.xml.sax.ErrorHandler error_handler)
        {
            java.net.URL url = com.oddlabs.util.Utils.makeURL(xml_file);
            try
            {
                javax.xml.parsers.DocumentBuilderFactory factory = javax.xml.parsers.DocumentBuilderFactory.newInstance();
                factory.setValidating(true);
                javax.xml.parsers.DocumentBuilder builder = factory.newDocumentBuilder();
                builder.setErrorHandler(error_handler);
                org.w3c.dom.Document document = builder.parse(url.openStream());
                return document.getDocumentElement();
            }
            catch (System.Exception e) when (e is System.Exception || e is org.xml.sax.SAXException || e is System.IO.IOException)
            {
                throw new System.Exception(java.lang.JStr.valueOf(e), (System.Exception)e);
            }
        }
        // Java: 47
        public static com.oddlabs.tt.render.Texture loadTexture(org.w3c.dom.Node n)
        {
            return com.oddlabs.tt.gui.Icons.loadTexture(n.getAttributes().getNamedItem("texture").getNodeValue());
        }
        // Java: 51
        public static com.oddlabs.tt.render.Texture loadTexture(string tex_file)
        {
            com.oddlabs.tt.resource.TextureFile file = new com.oddlabs.tt.resource.TextureFile(tex_file, org.lwjgl.opengl.GL11.GL_RGBA, org.lwjgl.opengl.GL11.GL_LINEAR, org.lwjgl.opengl.GL11.GL_LINEAR, org.lwjgl.opengl.GL12.GL_CLAMP_TO_EDGE, org.lwjgl.opengl.GL12.GL_CLAMP_TO_EDGE);
            return ((com.oddlabs.tt.render.Texture)com.oddlabs.tt.resource.Resources.findResource(file));
        }
        // Java: 61
        public static org.w3c.dom.Node getNodeByName(string name, org.w3c.dom.Node n)
        {
            org.w3c.dom.NodeList nl = n.getChildNodes();
            for (int i = 0; i < nl.getLength(); i++)
            {
                if (java.lang.JO.equals(nl.item(i).getNodeName(), name))
                {
                    return nl.item(i);
                }
            }
            return null;
        }
        // Java: 71
        public static int getInt(org.w3c.dom.Node n, string key)
        {
            string @string = n.getAttributes().getNamedItem(key).getNodeValue();
            return java.lang.Integer.parseInt(@string);
        }
        // Java: 76
        public static float getFloat(org.w3c.dom.Node n, string key)
        {
            string @string = n.getAttributes().getNamedItem(key).getNodeValue();
            return java.lang.Float.parseFloat(@string);
        }
        // Java: 81
        public static com.oddlabs.tt.gui.ModeIconQuads getNamedIconQuads(org.w3c.dom.Node n, string name, com.oddlabs.tt.render.Texture texture)
        {
            return com.oddlabs.tt.gui.Icons.getIconQuads(com.oddlabs.tt.gui.Icons.getNodeByName(name, n), texture);
        }
        // Java: 85
        public static com.oddlabs.tt.gui.IconQuad getNamedIconQuad(org.w3c.dom.Node n, string name, com.oddlabs.tt.render.Texture texture)
        {
            return com.oddlabs.tt.gui.Icons.getIconQuad(com.oddlabs.tt.gui.Icons.getNodeByName(name, n), texture);
        }
        // Java: 89
        public static com.oddlabs.tt.gui.ModeIconQuads getIconQuads(org.w3c.dom.Node n, com.oddlabs.tt.render.Texture texture)
        {
            return new com.oddlabs.tt.gui.ModeIconQuads(com.oddlabs.tt.gui.Icons.getIconQuad(com.oddlabs.tt.gui.Icons.getNodeByName("normal", n), texture), com.oddlabs.tt.gui.Icons.getIconQuad(com.oddlabs.tt.gui.Icons.getNodeByName("active", n), texture), com.oddlabs.tt.gui.Icons.getIconQuad(com.oddlabs.tt.gui.Icons.getNodeByName("disabled", n), texture));
        }
        // Java: 97
        public static com.oddlabs.tt.gui.IconQuad getIconQuad(org.w3c.dom.Node n, com.oddlabs.tt.render.Texture texture)
        {
            org.w3c.dom.Node q = com.oddlabs.tt.gui.Icons.getNodeByName("quad", n);
            return com.oddlabs.tt.gui.Icons.parseIconQuad(q, texture);
        }
        // Java: 102
        public static com.oddlabs.tt.gui.IconQuad parseIconQuad(org.w3c.dom.Node q, com.oddlabs.tt.render.Texture texture)
        {
            int left = com.oddlabs.tt.gui.Icons.getInt(q, "left");
            int top = com.oddlabs.tt.gui.Icons.getInt(q, "top");
            int right = com.oddlabs.tt.gui.Icons.getInt(q, "right");
            int bottom = com.oddlabs.tt.gui.Icons.getInt(q, "bottom");
            return new com.oddlabs.tt.gui.IconQuad((float)(left / ((float)(texture.getWidth()))), (float)(1.0f - (float)(bottom / ((float)(texture.getHeight())))), (float)(right / ((float)(texture.getWidth()))), (float)(1.0f - (float)(top / ((float)(texture.getHeight())))), right - left, bottom - top, texture);
        }
        // Java: 116
        public static org.joml.Vector4f getNamedColor(org.w3c.dom.Node n, string name)
        {
            return com.oddlabs.tt.gui.Icons.getColor(com.oddlabs.tt.gui.Icons.getNodeByName(name, n));
        }
        // Java: 120
        public static org.joml.Vector4f getColor(org.w3c.dom.Node n)
        {
            org.w3c.dom.Node q = com.oddlabs.tt.gui.Icons.getNodeByName("color", n);
            float r = com.oddlabs.tt.gui.Icons.getFloat(q, "r");
            float g = com.oddlabs.tt.gui.Icons.getFloat(q, "g");
            float b = com.oddlabs.tt.gui.Icons.getFloat(q, "b");
            float a = com.oddlabs.tt.gui.Icons.getFloat(q, "a");
            return new org.joml.Vector4f(r, g, b, a);
        }
        public Icons() : base()
        {
        }
    }
}
