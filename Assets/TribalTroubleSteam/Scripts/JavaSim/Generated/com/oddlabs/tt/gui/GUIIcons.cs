// Translated from GUIIcons.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 21
    public class GUIIcons : java.lang.Object
    {
        public static java.util.ResourceBundle bundle;
        // Java: 24
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.gui.GUIIcons.bundle, key, args);
        }
        public static com.oddlabs.tt.gui.GUIIcons ICONS;
        public com.oddlabs.tt.gui.ModeIconQuads harvest_icon;
        public com.oddlabs.tt.gui.ModeIconQuads tree_icon;
        public com.oddlabs.tt.gui.ModeIconQuads rock_icon;
        public com.oddlabs.tt.gui.ModeIconQuads iron_icon;
        public com.oddlabs.tt.gui.ModeIconQuads rubber_icon;
        public com.oddlabs.tt.gui.IconQuad tree_status_icon;
        public com.oddlabs.tt.gui.IconQuad rock_status_icon;
        public com.oddlabs.tt.gui.IconQuad iron_status_icon;
        public com.oddlabs.tt.gui.IconQuad rubber_status_icon;
        public com.oddlabs.tt.gui.IconQuad cheat_icon;
        public com.oddlabs.tt.gui.RaceIcons native_icons;
        public com.oddlabs.tt.gui.RaceIcons viking_icons;
        public com.oddlabs.tt.gui.IconQuad[] watch;
        public com.oddlabs.tt.gui.IconQuad infinite;
        public com.oddlabs.tt.gui.NotifyArrowData notify_arrow_data;
        public java.util.Map tool_tip_icons;
        // Java: 48
        public static com.oddlabs.tt.gui.GUIIcons getIcons()
        {
            return com.oddlabs.tt.gui.GUIIcons.ICONS;
        }
        // Java: 88
        public static com.oddlabs.tt.gui.RaceIcons parseRaceIcons(org.w3c.dom.Node n, string head, java.util.function.Supplier magic1_desc, java.util.function.Supplier magic2_desc, com.oddlabs.tt.render.Texture texture)
        {
            return new com.oddlabs.tt.gui.RaceIcons(com.oddlabs.tt.gui.Icons.getNamedIconQuad(n, java.lang.JStr.valueOf(head) + "_unit_status_icon", texture), com.oddlabs.tt.gui.Icons.getNamedIconQuad(n, java.lang.JStr.valueOf(head) + "_weapon_rock_status_icon", texture), com.oddlabs.tt.gui.Icons.getNamedIconQuad(n, java.lang.JStr.valueOf(head) + "_weapon_iron_status_icon", texture), com.oddlabs.tt.gui.Icons.getNamedIconQuad(n, java.lang.JStr.valueOf(head) + "_weapon_rubber_status_icon", texture), com.oddlabs.tt.gui.Icons.getNamedIconQuads(n, java.lang.JStr.valueOf(head) + "_build_weapons_icon", texture), com.oddlabs.tt.gui.Icons.getNamedIconQuads(n, java.lang.JStr.valueOf(head) + "_build_weapon_rock_icon", texture), com.oddlabs.tt.gui.Icons.getNamedIconQuads(n, java.lang.JStr.valueOf(head) + "_build_weapon_iron_icon", texture), com.oddlabs.tt.gui.Icons.getNamedIconQuads(n, java.lang.JStr.valueOf(head) + "_build_weapon_rubber_icon", texture), com.oddlabs.tt.gui.Icons.getNamedIconQuads(n, java.lang.JStr.valueOf(head) + "_army_icon", texture), com.oddlabs.tt.gui.Icons.getNamedIconQuads(n, java.lang.JStr.valueOf(head) + "_warrior_rock_icon", texture), com.oddlabs.tt.gui.Icons.getNamedIconQuads(n, java.lang.JStr.valueOf(head) + "_warrior_iron_icon", texture), com.oddlabs.tt.gui.Icons.getNamedIconQuads(n, java.lang.JStr.valueOf(head) + "_warrior_rubber_icon", texture), com.oddlabs.tt.gui.Icons.getNamedIconQuads(n, java.lang.JStr.valueOf(head) + "_peon_icon", texture), com.oddlabs.tt.gui.Icons.getNamedIconQuads(n, java.lang.JStr.valueOf(head) + "_chieftain_icon", texture), com.oddlabs.tt.gui.Icons.getNamedIconQuads(n, java.lang.JStr.valueOf(head) + "_transport_icon", texture), com.oddlabs.tt.gui.Icons.getNamedIconQuads(n, java.lang.JStr.valueOf(head) + "_attack_icon", texture), com.oddlabs.tt.gui.Icons.getNamedIconQuads(n, java.lang.JStr.valueOf(head) + "_move_icon", texture), com.oddlabs.tt.gui.Icons.getNamedIconQuads(n, java.lang.JStr.valueOf(head) + "_gather_repair_icon", texture), com.oddlabs.tt.gui.Icons.getNamedIconQuads(n, java.lang.JStr.valueOf(head) + "_quarters_icon", texture), com.oddlabs.tt.gui.Icons.getNamedIconQuads(n, java.lang.JStr.valueOf(head) + "_armory_icon", texture), com.oddlabs.tt.gui.Icons.getNamedIconQuads(n, java.lang.JStr.valueOf(head) + "_tower_icon", texture), com.oddlabs.tt.gui.Icons.getNamedIconQuads(n, java.lang.JStr.valueOf(head) + "_tower_exit_icon", texture), com.oddlabs.tt.gui.Icons.getNamedIconQuads(n, java.lang.JStr.valueOf(head) + "_rally_point_icon", texture), com.oddlabs.tt.gui.Icons.getNamedIconQuads(n, java.lang.JStr.valueOf(head) + "_magic1_icon", texture), magic1_desc, com.oddlabs.tt.gui.Icons.getNamedIconQuads(n, java.lang.JStr.valueOf(head) + "_magic2_icon", texture), magic2_desc, com.oddlabs.tt.gui.Icons.getNamedIconQuads(n, java.lang.JStr.valueOf(head) + "_ship_icon", texture));
        }
        // Java: 121
        public static com.oddlabs.tt.gui.IconQuad[] generateWatchIcons()
        {
            int numIcons = 25;
            int iconSize = 64;
            int textureSize = 512;
            int perRow = textureSize / iconSize;
            com.oddlabs.tt.resource.GLIntImage image = new com.oddlabs.tt.resource.GLIntImage(textureSize, textureSize, org.lwjgl.opengl.GL11.GL_RGBA);
            image.clearAll(0);
            int radius = 24;
            int rimWidth = 2;
            int outerRadius = radius + rimWidth;
            int shadowOffset = 2;
            for (int i = 0; i < numIcons; i++)
            {
                float progress = (float)(i / ((float)((numIcons - 1))));
                int r = default;
                int g = default;
                int b = default;
                if (progress < 0.5f)
                {
                    r = 255;
                    g = java.lang.JC.f2i(((float)(255 * ((float)(progress * 2)))));
                    b = 0;
                }
                else
                {
                    r = java.lang.JC.f2i(((float)(255 * ((float)(1.0f - (float)(((float)(progress - 0.5f)) * 2))))));
                    g = 255;
                    b = 0;
                }
                int fillColor = (255 << 24) | (b << 16) | (g << 8) | r;
                int whiteColor = (-1);
                int rimColor = (-4144960);
                int col = i % perRow;
                int row = i / perRow;
                int startX = col * iconSize;
                int startY = row * iconSize;
                for (int y = 0; y < iconSize; y++)
                {
                    for (int x = 0; x < iconSize; x++)
                    {
                        int px = startX + x;
                        int py = textureSize - 1 - (startY + y);
                        float dx = (float)((float)(x - (float)(iconSize / 2.0f)) + 0.5f);
                        float dy = (float)((float)((float)(iconSize / 2.0f) - y) - 0.5f);
                        float dist = ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy)))));
                        float shadowDx = (float)(dx - shadowOffset);
                        float shadowDy = (float)(dy + shadowOffset);
                        float shadowDist = ((float)(java.lang.Math.sqrt((float)((float)(shadowDx * shadowDx) + (float)(shadowDy * shadowDy)))));
                        int finalColor = 0;
                        if (shadowDist < outerRadius + 2)
                        {
                            float alpha = 0.5f;
                            if (shadowDist > outerRadius)
                            {
                                alpha = (float)(alpha * ((float)(1.0f - (float)(((float)(shadowDist - outerRadius)) / 2.0f))));
                            }
                            finalColor = (java.lang.JC.f2i(((float)(alpha * 255))) << 24);
                        }
                        if (dist < outerRadius + 1)
                        {
                            float alpha_2 = (dist > outerRadius ? (float)(1.0f - ((float)(dist - outerRadius))) : 1.0f);
                            int pixelColor = default;
                            if (dist > radius)
                            {
                                pixelColor = rimColor;
                            }
                            else
                            {
                                double angle = java.lang.Math.atan2(dy, dx);
                                double normalizedAngle = java.lang.Math.PI / 2 - angle;
                                if (normalizedAngle < 0)
                                {
                                    normalizedAngle += 2 * java.lang.Math.PI;
                                }
                                float angleFraction = ((float)((normalizedAngle / (2 * java.lang.Math.PI))));
                                pixelColor = (angleFraction <= progress ? fillColor : whiteColor);
                            }
                            int destA = (java.lang.JC.ushr(finalColor, 24));
                            int srcA = java.lang.JC.f2i(((float)((java.lang.JC.ushr(pixelColor, 24)) * alpha_2)));
                            float srcAf = (float)(srcA / 255.0f);
                            float destAf = (float)(destA / 255.0f);
                            float outAf = (float)(srcAf + (float)(destAf * ((float)(1.0f - srcAf))));
                            if (outAf > 0)
                            {
                                int srcR = (java.lang.JC.ushr(pixelColor, 16)) & 255;
                                int srcG = (java.lang.JC.ushr(pixelColor, 8)) & 255;
                                int srcB = pixelColor & 255;
                                int outR = java.lang.JC.f2i(((float)(((float)(srcR * srcAf)) / outAf)));
                                int outG = java.lang.JC.f2i(((float)(((float)(srcG * srcAf)) / outAf)));
                                int outB = java.lang.JC.f2i(((float)(((float)(srcB * srcAf)) / outAf)));
                                int outA = java.lang.JC.f2i(((float)(outAf * 255)));
                                finalColor = (outA << 24) | (outR << 16) | (outG << 8) | outB;
                            }
                        }
                        image.putPixel(px, py, finalColor);
                    }
                }
            }
            com.oddlabs.tt.render.Texture texture = new com.oddlabs.tt.render.Texture(new com.oddlabs.tt.resource.GLImage[] { image }, org.lwjgl.opengl.GL11.GL_RGBA, org.lwjgl.opengl.GL11.GL_LINEAR, org.lwjgl.opengl.GL11.GL_LINEAR, org.lwjgl.opengl.GL12.GL_CLAMP_TO_EDGE, org.lwjgl.opengl.GL12.GL_CLAMP_TO_EDGE);
            return ((com.oddlabs.tt.gui.IconQuad[])java.util.stream.IntStream_.range(0, numIcons).mapToObj(new java.util.function.IntFunction_L((int i_2) =>
            {
                int col_2 = i_2 % perRow;
                int row_2 = i_2 / perRow;
                int startX_2 = col_2 * iconSize;
                int startY_2 = row_2 * iconSize;
                float u1 = (float)(startX_2 / ((float)(textureSize)));
                float v1 = (float)(1.0f - (float)((startY_2 + iconSize) / ((float)(textureSize))));
                float u2 = (float)((startX_2 + iconSize) / ((float)(textureSize)));
                float v2 = (float)(1.0f - (float)(startY_2 / ((float)(textureSize))));
                return new com.oddlabs.tt.gui.IconQuad(u1, v1, u2, v2, 22, 22, texture);
            })).toArray(new java.util.function.IntFunction_L((int a0) => { return new com.oddlabs.tt.gui.IconQuad[a0]; })));
        }
        // Java: 245
        public static com.oddlabs.tt.gui.NotifyArrowData parseNotifyArrowData(org.w3c.dom.Node n, com.oddlabs.tt.render.Texture texture)
        {
            org.w3c.dom.Node node = com.oddlabs.tt.gui.Icons.getNodeByName("notify_arrow", n);
            return new com.oddlabs.tt.gui.NotifyArrowData(com.oddlabs.tt.gui.Icons.getIconQuad(node, texture), com.oddlabs.tt.gui.Icons.getInt(node, "head_x"), com.oddlabs.tt.gui.Icons.getInt(node, "head_y"), com.oddlabs.tt.gui.Icons.getInt(node, "end_x"), com.oddlabs.tt.gui.Icons.getInt(node, "end_y"));
        }
        // Java: 254
        public virtual java.util.List getToolTipIcon(System.Type key)
        {
            return ((java.util.List)tool_tip_icons.get(key));
        }
        // Java: 258
        public com.oddlabs.tt.gui.RaceIcons getVikingIcons()
        {
            return viking_icons;
        }
        // Java: 262
        public com.oddlabs.tt.gui.RaceIcons getNativeIcons()
        {
            return native_icons;
        }
        // Java: 266
        public com.oddlabs.tt.gui.ModeIconQuads getHarvestIcon()
        {
            return harvest_icon;
        }
        // Java: 270
        public com.oddlabs.tt.gui.IconQuad getTreeStatusIcon()
        {
            return tree_status_icon;
        }
        // Java: 274
        public com.oddlabs.tt.gui.IconQuad getRockStatusIcon()
        {
            return rock_status_icon;
        }
        // Java: 278
        public com.oddlabs.tt.gui.IconQuad getIronStatusIcon()
        {
            return iron_status_icon;
        }
        // Java: 282
        public com.oddlabs.tt.gui.IconQuad getRubberStatusIcon()
        {
            return rubber_status_icon;
        }
        // Java: 286
        public com.oddlabs.tt.gui.IconQuad getCheatIcon()
        {
            return cheat_icon;
        }
        // Java: 290
        public com.oddlabs.tt.gui.ModeIconQuads getTreeIcon()
        {
            return tree_icon;
        }
        // Java: 294
        public com.oddlabs.tt.gui.ModeIconQuads getRockIcon()
        {
            return rock_icon;
        }
        // Java: 298
        public com.oddlabs.tt.gui.ModeIconQuads getIronIcon()
        {
            return iron_icon;
        }
        // Java: 302
        public com.oddlabs.tt.gui.ModeIconQuads getRubberIcon()
        {
            return rubber_icon;
        }
        // Java: 306
        public com.oddlabs.tt.gui.IconQuad getWatch(float progress)
        {
            return watch[java.lang.JC.f2i(((float)(progress * (watch.Length - 1))))];
        }
        // Java: 310
        public com.oddlabs.tt.gui.IconQuad getInfinite()
        {
            return infinite;
        }
        // Java: 314
        public com.oddlabs.tt.gui.NotifyArrowData getNotifyArrowData()
        {
            return notify_arrow_data;
        }
        static GUIIcons()
        {
            GUIIcons.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.gui.Icons).getName());
            GUIIcons.ICONS = new com.oddlabs.tt.gui.GUIIcons("/gui/icons.xml");
        }
        public GUIIcons(string xml_file) : base()
        {
            org.w3c.dom.Node root = com.oddlabs.tt.gui.Icons.loadFile(xml_file, new com.oddlabs.tt.gui.GUIErrorHandler());
            com.oddlabs.tt.render.Texture texture = com.oddlabs.tt.gui.Icons.loadTexture(root);
            harvest_icon = com.oddlabs.tt.gui.Icons.getNamedIconQuads(root, "harvest_icon", texture);
            tree_icon = com.oddlabs.tt.gui.Icons.getNamedIconQuads(root, "tree_icon", texture);
            rock_icon = com.oddlabs.tt.gui.Icons.getNamedIconQuads(root, "rock_icon", texture);
            iron_icon = com.oddlabs.tt.gui.Icons.getNamedIconQuads(root, "iron_icon", texture);
            rubber_icon = com.oddlabs.tt.gui.Icons.getNamedIconQuads(root, "rubber_icon", texture);
            tree_status_icon = com.oddlabs.tt.gui.Icons.getNamedIconQuad(root, "tree_status_icon", texture);
            rock_status_icon = com.oddlabs.tt.gui.Icons.getNamedIconQuad(root, "rock_status_icon", texture);
            iron_status_icon = com.oddlabs.tt.gui.Icons.getNamedIconQuad(root, "iron_status_icon", texture);
            rubber_status_icon = com.oddlabs.tt.gui.Icons.getNamedIconQuad(root, "rubber_status_icon", texture);
            cheat_icon = com.oddlabs.tt.gui.Icons.getNamedIconQuad(root, "cheat_icon", texture);
            java.util.function.Supplier tt_caption = new java.util.function.Supplier_L(() =>
            {
                return i18n("terrifying_toot", com.oddlabs.tt.render.Renderer.getLocalInput().getInputManager().getBindingString(com.oddlabs.tt.input.GameAction.MAGIC_1));
            });
            java.util.function.Supplier rr_caption = new java.util.function.Supplier_L(() =>
            {
                return i18n("ravaging_roar", com.oddlabs.tt.render.Renderer.getLocalInput().getInputManager().getBindingString(com.oddlabs.tt.input.GameAction.MAGIC_2));
            });
            java.util.function.Supplier ss_caption = new java.util.function.Supplier_L(() =>
            {
                return i18n("stinking_stew", com.oddlabs.tt.render.Renderer.getLocalInput().getInputManager().getBindingString(com.oddlabs.tt.input.GameAction.MAGIC_1));
            });
            java.util.function.Supplier cc_caption = new java.util.function.Supplier_L(() =>
            {
                return i18n("crackling_cloud", com.oddlabs.tt.render.Renderer.getLocalInput().getInputManager().getBindingString(com.oddlabs.tt.input.GameAction.MAGIC_2));
            });
            viking_icons = com.oddlabs.tt.gui.GUIIcons.parseRaceIcons(root, "vikings", tt_caption, rr_caption, texture);
            native_icons = com.oddlabs.tt.gui.GUIIcons.parseRaceIcons(root, "natives", ss_caption, cc_caption, texture);
            watch = com.oddlabs.tt.gui.GUIIcons.generateWatchIcons();
            infinite = com.oddlabs.tt.gui.Icons.getNamedIconQuad(root, "infinite", texture);
            notify_arrow_data = com.oddlabs.tt.gui.GUIIcons.parseNotifyArrowData(root, texture);
            tool_tip_icons = java.util.Map_.of(typeof(com.oddlabs.tt.landscape.TreeSupply), java.util.List_.of(tree_status_icon), typeof(com.oddlabs.tt.model.RockSupply), java.util.List_.of(rock_status_icon), typeof(com.oddlabs.tt.model.IronSupply), java.util.List_.of(iron_status_icon), typeof(com.oddlabs.tt.model.RubberSupply), java.util.List_.of(rubber_status_icon));
        }
    }
}
