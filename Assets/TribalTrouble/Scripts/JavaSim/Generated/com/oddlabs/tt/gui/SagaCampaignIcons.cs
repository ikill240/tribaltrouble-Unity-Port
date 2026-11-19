// Translated from SagaCampaignIcons.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 15
    public class SagaCampaignIcons : java.lang.Object, com.oddlabs.tt.gui.CampaignIcons
    {
        public const int NUM_ISLANDS = 56;
        public static int[][][] HIDDEN_ROUTES;
        public static com.oddlabs.tt.gui.SagaCampaignIcons.Chart[][] charts;
        public static com.oddlabs.tt.gui.SagaCampaignIcons[][][] icons;
        public com.oddlabs.tt.gui.IconQuad[] faces;
        public int page;
        public int style;
        // Java: 30
        public class Chart : java.lang.Object
        {
            public com.oddlabs.tt.gui.IconQuad map;
            public com.oddlabs.tt.gui.MapIslandData[] islands;
            public com.oddlabs.tt.gui.IconQuad[] flags;
            public com.oddlabs.tt.gui.IconQuad[] boats;
            public com.oddlabs.tt.gui.GUIIcon[] routes;
            private void init_Chart()
            {
                this.islands = new com.oddlabs.tt.gui.MapIslandData[com.oddlabs.tt.gui.SagaCampaignIcons.NUM_ISLANDS];
                this.flags = new com.oddlabs.tt.gui.IconQuad[3];
                this.boats = new com.oddlabs.tt.gui.IconQuad[3];
            }
            public Chart(int page, int style) : base()
            {
                init_Chart();
                org.w3c.dom.Node root = com.oddlabs.tt.gui.Icons.loadFile("/gui/saga_chart_p" + (page) + "_s" + (style) + ".xml", new com.oddlabs.tt.gui.GUIErrorHandler());
                com.oddlabs.tt.render.Texture atlas = com.oddlabs.tt.gui.Icons.loadTexture(root);
                for (int i = 0; i < 3; i++)
                {
                    flags[i] = com.oddlabs.tt.gui.Icons.getNamedIconQuad(root, "flag" + (i), atlas);
                    boats[i] = com.oddlabs.tt.gui.Icons.getNamedIconQuad(root, "boat" + (i), atlas);
                }
                map = com.oddlabs.tt.gui.Icons.getNamedIconQuad(root, "map", atlas);
                int[][] hidden = com.oddlabs.tt.gui.SagaCampaignIcons.HIDDEN_ROUTES[page];
                routes = new com.oddlabs.tt.gui.GUIIcon[hidden.Length];
                for (int i_2 = 0; i_2 < hidden.Length; i_2++)
                {
                    string name = "route" + (hidden[i_2][0]) + "_" + (hidden[i_2][1]);
                    com.oddlabs.tt.gui.IconQuad quad = com.oddlabs.tt.gui.Icons.getNamedIconQuad(root, name, atlas);
                    org.w3c.dom.Node n = com.oddlabs.tt.gui.Icons.getNodeByName(name, root);
                    routes[i_2] = new com.oddlabs.tt.gui.GUIIcon(quad);
                    routes[i_2].setPos(com.oddlabs.tt.gui.Icons.getInt(n, "x"), atlas.getHeight() - com.oddlabs.tt.gui.Icons.getInt(n, "y"));
                }
                for (int i_3 = 0; i_3 < com.oddlabs.tt.gui.SagaCampaignIcons.NUM_ISLANDS; i_3++)
                {
                    org.w3c.dom.Node node = com.oddlabs.tt.gui.SagaCampaignIcons.find("island" + (i_3), root);
                    if (node == null)
                    {
                        continue;
                    }
                    com.oddlabs.tt.gui.ModeIconQuads quads = com.oddlabs.tt.gui.Icons.getNamedIconQuads(node, "island", atlas);
                    org.w3c.dom.Node n_2 = com.oddlabs.tt.gui.Icons.getNodeByName("island", node);
                    int x = com.oddlabs.tt.gui.Icons.getInt(n_2, "x");
                    int y = atlas.getHeight() - com.oddlabs.tt.gui.Icons.getInt(n_2, "y");
                    int pin_index = com.oddlabs.tt.gui.Icons.getInt(n_2, "pin_index");
                    int pin_x = com.oddlabs.tt.gui.Icons.getInt(n_2, "pin_x");
                    int pin_y = atlas.getHeight() - com.oddlabs.tt.gui.Icons.getInt(n_2, "pin_y");
                    islands[i_3] = new com.oddlabs.tt.gui.MapIslandData(quads, x, y, flags[pin_index], boats[pin_index], pin_x, pin_y);
                }
            }
        }
        // Java: 70
        public static org.w3c.dom.Node find(string name, org.w3c.dom.Node n)
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
        // Java: 78
        public com.oddlabs.tt.gui.SagaCampaignIcons.Chart chart()
        {
            if (com.oddlabs.tt.gui.SagaCampaignIcons.charts[page][style] == null)
            {
                com.oddlabs.tt.gui.SagaCampaignIcons.charts[page][style] = new com.oddlabs.tt.gui.SagaCampaignIcons.Chart(page, style);
            }
            return com.oddlabs.tt.gui.SagaCampaignIcons.charts[page][style];
        }
        // Java: 85
        public static com.oddlabs.tt.gui.SagaCampaignIcons get(bool native_tribe, int page, int style)
        {
            int t = (native_tribe ? 1 : 0);
            if (com.oddlabs.tt.gui.SagaCampaignIcons.icons[t][page][style] == null)
            {
                com.oddlabs.tt.gui.IconQuad[] faces = default;
                if (native_tribe)
                {
                    com.oddlabs.tt.gui.IconQuad[] n = com.oddlabs.tt.gui.NativeCampaignIcons.getIcons().getFaces();
                    com.oddlabs.tt.gui.IconQuad[] v = com.oddlabs.tt.gui.VikingCampaignIcons.getIcons().getFaces();
                    faces = new com.oddlabs.tt.gui.IconQuad[] { n[0], n[2], n[4], n[1], n[7], v[8] };
                }
                else
                {
                    com.oddlabs.tt.gui.IconQuad[] v_2 = com.oddlabs.tt.gui.VikingCampaignIcons.getIcons().getFaces();
                    faces = new com.oddlabs.tt.gui.IconQuad[] { v_2[0], v_2[1], v_2[4], v_2[6], v_2[7], v_2[8] };
                }
                com.oddlabs.tt.gui.SagaCampaignIcons.icons[t][page][style] = new com.oddlabs.tt.gui.SagaCampaignIcons(faces, page, style);
            }
            return com.oddlabs.tt.gui.SagaCampaignIcons.icons[t][page][style];
        }
        // Java: 103
        public static com.oddlabs.tt.gui.SagaCampaignIcons get(bool native_tribe)
        {
            return com.oddlabs.tt.gui.SagaCampaignIcons.get(native_tribe, 0, (native_tribe ? 1 : 0));
        }
        // Java: 113
        public int getNumRoutes()
        {
            return com.oddlabs.tt.gui.SagaCampaignIcons.HIDDEN_ROUTES[page].Length;
        }
        // Java: 118
        public com.oddlabs.tt.gui.IconQuad getPin(int k)
        {
            return chart().flags[k];
        }
        // Java: 123
        public int[] getRouteEnds(int i)
        {
            return com.oddlabs.tt.gui.SagaCampaignIcons.HIDDEN_ROUTES[page][i];
        }
        // Java: 128
        public com.oddlabs.tt.gui.GUIIcon getRoute(int i)
        {
            return chart().routes[i];
        }
        // Java: 132
        public com.oddlabs.tt.gui.GUIIcon[] getHiddenRoutes()
        {
            return new com.oddlabs.tt.gui.GUIIcon[0];
        }
        // Java: 137
        public com.oddlabs.tt.gui.IconQuad[] getFaces()
        {
            return faces;
        }
        // Java: 142
        public com.oddlabs.tt.gui.IconQuad getMap()
        {
            return chart().map;
        }
        // Java: 147
        public int getNumIslands()
        {
            return com.oddlabs.tt.gui.SagaCampaignIcons.NUM_ISLANDS;
        }
        // Java: 153
        public com.oddlabs.tt.gui.MapIslandData getMapIslandData(int i)
        {
            return chart().islands[i];
        }
        static SagaCampaignIcons()
        {
            SagaCampaignIcons.HIDDEN_ROUTES = new int[][][] { new int[][] { new int[] { 8, 21 }, new int[] { 7, 21 }, new int[] { 3, 22 }, new int[] { 1, 22 }, new int[] { 4, 23 }, new int[] { 5, 23 }, new int[] { 10, 24 }, new int[] { 12, 24 }, new int[] { 12, 25 }, new int[] { 14, 25 }, new int[] { 20, 26 }, new int[] { 17, 26 }, new int[] { 19, 27 }, new int[] { 6, 27 }, new int[] { 14, 28 }, new int[] { 12, 28 } }, new int[][] { new int[] { 29, 38 }, new int[] { 31, 38 }, new int[] { 34, 39 }, new int[] { 36, 39 } }, new int[][] { new int[] { 42, 47 }, new int[] { 44, 47 } } };
            SagaCampaignIcons.charts = java.lang.JA.newArray2<com.oddlabs.tt.gui.SagaCampaignIcons.Chart>(3, 3);
            SagaCampaignIcons.icons = java.lang.JA.newArray3<com.oddlabs.tt.gui.SagaCampaignIcons>(2, 3, 3);
        }
        public SagaCampaignIcons(com.oddlabs.tt.gui.IconQuad[] faces, int page, int style) : base()
        {
            this.faces = faces;
            this.page = page;
            this.style = style;
        }
    }
}
