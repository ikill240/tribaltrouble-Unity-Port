// Translated from NativeCampaignIcons.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 7
    public class NativeCampaignIcons : java.lang.Object, com.oddlabs.tt.gui.CampaignIcons
    {
        public const int NUM_ISLANDS = 8;
        public static com.oddlabs.tt.gui.NativeCampaignIcons ICONS;
        public com.oddlabs.tt.gui.IconQuad map;
        public com.oddlabs.tt.gui.MapIslandData[] islands;
        public com.oddlabs.tt.gui.IconQuad[] flags;
        public com.oddlabs.tt.gui.IconQuad[] boats;
        public com.oddlabs.tt.gui.GUIIcon[] hidden;
        public com.oddlabs.tt.gui.IconQuad[] faces;
        public int offset_x;
        public int offset_y;
        public int width;
        public int height;
        // Java: 23
        public static com.oddlabs.tt.gui.NativeCampaignIcons getIcons()
        {
            return com.oddlabs.tt.gui.NativeCampaignIcons.ICONS;
        }
        // Java: 60
        public com.oddlabs.tt.gui.MapIslandData loadMapIslandData(org.w3c.dom.Node root, string name, com.oddlabs.tt.render.Texture texture)
        {
            org.w3c.dom.Node node = com.oddlabs.tt.gui.Icons.getNodeByName(name, root);
            com.oddlabs.tt.gui.ModeIconQuads quads = com.oddlabs.tt.gui.Icons.getNamedIconQuads(node, "island", texture);
            org.w3c.dom.Node n = com.oddlabs.tt.gui.Icons.getNodeByName("island", node);
            int x = com.oddlabs.tt.gui.Icons.getInt(n, "x");
            int y = texture.getHeight() - com.oddlabs.tt.gui.Icons.getInt(n, "y");
            int pin_index = com.oddlabs.tt.gui.Icons.getInt(n, "pin_index");
            int pin_x = com.oddlabs.tt.gui.Icons.getInt(n, "pin_x");
            int pin_y = texture.getHeight() - com.oddlabs.tt.gui.Icons.getInt(n, "pin_y");
            return new com.oddlabs.tt.gui.MapIslandData(quads, x, y, flags[pin_index], boats[pin_index], pin_x, pin_y);
        }
        // Java: 73
        public com.oddlabs.tt.gui.GUIIcon getNamedGUIIcon(org.w3c.dom.Node root, string name, com.oddlabs.tt.render.Texture texture)
        {
            com.oddlabs.tt.gui.IconQuad temp = com.oddlabs.tt.gui.Icons.getNamedIconQuad(root, name, texture);
            org.w3c.dom.Node n = com.oddlabs.tt.gui.Icons.getNodeByName(name, root);
            int x = com.oddlabs.tt.gui.Icons.getInt(n, "x");
            int y = texture.getHeight() - com.oddlabs.tt.gui.Icons.getInt(n, "y");
            com.oddlabs.tt.gui.GUIIcon gui_icon = new com.oddlabs.tt.gui.GUIIcon(temp);
            gui_icon.setPos(x, y);
            return gui_icon;
        }
        // Java: 83
        public com.oddlabs.tt.gui.GUIIcon[] getHiddenRoutes()
        {
            return hidden;
        }
        // Java: 88
        public com.oddlabs.tt.gui.IconQuad[] getFaces()
        {
            return faces;
        }
        // Java: 93
        public com.oddlabs.tt.gui.IconQuad getMap()
        {
            return map;
        }
        // Java: 98
        public int getNumIslands()
        {
            return islands.Length;
        }
        // Java: 103
        public int getOffsetX()
        {
            return offset_x;
        }
        // Java: 107
        public int getOffsetY()
        {
            return offset_y;
        }
        // Java: 111
        public int getInternalWidth()
        {
            return width;
        }
        // Java: 115
        public int getInternalHeight()
        {
            return height;
        }
        // Java: 119
        public com.oddlabs.tt.gui.MapIslandData getMapIslandData(int i)
        {
            return islands[i];
        }
        static NativeCampaignIcons()
        {
            NativeCampaignIcons.ICONS = new com.oddlabs.tt.gui.NativeCampaignIcons("/gui/native_campaign.xml");
        }
        private void init_NativeCampaignIcons()
        {
            this.islands = new com.oddlabs.tt.gui.MapIslandData[com.oddlabs.tt.gui.NativeCampaignIcons.NUM_ISLANDS];
            this.flags = new com.oddlabs.tt.gui.IconQuad[3];
            this.boats = new com.oddlabs.tt.gui.IconQuad[3];
            this.hidden = new com.oddlabs.tt.gui.GUIIcon[1];
            this.faces = new com.oddlabs.tt.gui.IconQuad[9];
        }
        public NativeCampaignIcons(string xml_file) : base()
        {
            init_NativeCampaignIcons();
            org.w3c.dom.Node root = com.oddlabs.tt.gui.Icons.loadFile(xml_file, new com.oddlabs.tt.gui.GUIErrorHandler());
            com.oddlabs.tt.render.Texture atlas = com.oddlabs.tt.gui.Icons.loadTexture(root);
            flags[0] = com.oddlabs.tt.gui.Icons.getNamedIconQuad(root, "flag0", atlas);
            flags[1] = com.oddlabs.tt.gui.Icons.getNamedIconQuad(root, "flag1", atlas);
            flags[2] = com.oddlabs.tt.gui.Icons.getNamedIconQuad(root, "flag2", atlas);
            boats[0] = com.oddlabs.tt.gui.Icons.getNamedIconQuad(root, "boat0", atlas);
            boats[1] = com.oddlabs.tt.gui.Icons.getNamedIconQuad(root, "boat1", atlas);
            boats[2] = com.oddlabs.tt.gui.Icons.getNamedIconQuad(root, "boat2", atlas);
            hidden[0] = getNamedGUIIcon(root, "hidden0", atlas);
            faces[0] = com.oddlabs.tt.gui.Icons.getNamedIconQuad(root, "face0", atlas);
            faces[1] = com.oddlabs.tt.gui.Icons.getNamedIconQuad(root, "face1", atlas);
            faces[2] = com.oddlabs.tt.gui.Icons.getNamedIconQuad(root, "face2", atlas);
            faces[3] = com.oddlabs.tt.gui.Icons.getNamedIconQuad(root, "face3", atlas);
            faces[4] = com.oddlabs.tt.gui.Icons.getNamedIconQuad(root, "face4", atlas);
            faces[5] = com.oddlabs.tt.gui.Icons.getNamedIconQuad(root, "face5", atlas);
            faces[6] = com.oddlabs.tt.gui.Icons.getNamedIconQuad(root, "face6", atlas);
            faces[7] = com.oddlabs.tt.gui.Icons.getNamedIconQuad(root, "face7", atlas);
            faces[8] = com.oddlabs.tt.gui.Icons.getNamedIconQuad(root, "face8", atlas);
            map = com.oddlabs.tt.gui.Icons.getNamedIconQuad(root, "map", atlas);
            for (int i = 0; i < com.oddlabs.tt.gui.NativeCampaignIcons.NUM_ISLANDS; i++)
            {
                islands[i] = loadMapIslandData(root, "island" + (i), atlas);
            }
            org.w3c.dom.Node map_node = com.oddlabs.tt.gui.Icons.getNodeByName("map", root);
            offset_x = com.oddlabs.tt.gui.Icons.getInt(map_node, "offset_x");
            offset_y = com.oddlabs.tt.gui.Icons.getInt(map_node, "offset_y");
            width = com.oddlabs.tt.gui.Icons.getInt(map_node, "width");
            height = com.oddlabs.tt.gui.Icons.getInt(map_node, "height");
        }
    }
}
