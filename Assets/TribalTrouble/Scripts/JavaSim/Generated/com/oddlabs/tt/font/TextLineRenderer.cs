// Translated from TextLineRenderer.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.font
{
    // Java: 8
    public class TextLineRenderer : java.lang.Object
    {
        // Java: 14
        public static void render(com.oddlabs.tt.render.GUIRenderer renderer, com.oddlabs.tt.font.TextLayout layout, float x, float y, org.joml.Vector4fc color)
        {
            com.oddlabs.tt.font.TextLineRenderer.render(renderer, layout, x, y, java.lang.Float.NEGATIVE_INFINITY, java.lang.Float.POSITIVE_INFINITY, color);
        }
        // Java: 19
        public static void render(com.oddlabs.tt.render.GUIRenderer renderer, com.oddlabs.tt.font.TextLayout layout, float x, float y, float clipLeft, float clipRight, org.joml.Vector4fc color)
        {
            float currentY = y;
            foreach (com.oddlabs.tt.font.TextLayout.Line line in java.lang.JIter.of(layout.getLines()))
            {
                com.oddlabs.tt.font.TextLineRenderer.render(renderer, layout.getFont(), line.content(), x, currentY, clipLeft, clipRight, color);
                currentY = (float)(currentY - layout.getFont().getHeight());
            }
        }
        // Java: 32
        public static float render(com.oddlabs.tt.render.GUIRenderer renderer, com.oddlabs.tt.font.Font font, string text, float x, float y, float clipLeft, float clipRight, org.joml.Vector4fc color)
        {
            return ((float)(text.codePoints().asDoubleStream().reduce(x, new java.util.function.DoubleBinaryOperator_L((double currentX, double codePointAsDouble) =>
            {
                int codePoint = java.lang.JC.d2i(codePointAsDouble);
                if (codePoint == '\n')
                {
                    return currentX;
                }
                com.oddlabs.util.Quad quad = font.getQuad(codePoint);
                if (quad != null)
                {
                    float quadWidth = quad.getWidth();
                    float charAdvance = (float)(quadWidth - font.getXBorder());
                    if (currentX + quadWidth < clipLeft || currentX > clipRight)
                    {
                        return currentX + charAdvance;
                    }
                    float renderX = ((float)(currentX));
                    float renderWidth = quadWidth;
                    float u1 = quad.getU1();
                    float u2 = quad.getU2();
                    float textureUWidth = (float)(u2 - u1);
                    if (renderX < clipLeft)
                    {
                        float leftClipPixels = (float)(clipLeft - renderX);
                        float leftClipRatio = (float)(leftClipPixels / quadWidth);
                        u1 = (float)(u1 + (float)(textureUWidth * leftClipRatio));
                        renderWidth = (float)(renderWidth - leftClipPixels);
                        renderX = clipLeft;
                    }
                    if ((float)(renderX + renderWidth) > clipRight)
                    {
                        float rightClipPixels = (float)(((float)(renderX + renderWidth)) - clipRight);
                        float rightClipRatio = (float)(rightClipPixels / quadWidth);
                        u2 = (float)(u2 - (float)(textureUWidth * rightClipRatio));
                        renderWidth = (float)(renderWidth - rightClipPixels);
                    }
                    if (renderWidth > 0)
                    {
                        renderer.drawTexture(font.getTexture(), renderX, y, renderWidth, quad.getHeight(), u1, quad.getV1(), u2, quad.getV2(), color);
                    }
                    return currentX + charAdvance;
                }
                return currentX;
            }))));
        }
        public TextLineRenderer() : base()
        {
        }
    }
}
