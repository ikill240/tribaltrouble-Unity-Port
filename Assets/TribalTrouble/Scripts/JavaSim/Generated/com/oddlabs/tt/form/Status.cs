// Translated from Status.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 12
    public class Status : java.lang.Object
    {
        public java.lang.StringBuilder buf;
        // Java: 15
        public void render(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            long free_mem = java.lang.Runtime.getRuntime().freeMemory();
            buf.delete(0, buf.length());
            if (com.oddlabs.tt.global.Settings.getSettings().inDeveloperMode())
            {
                buf.append("TPF ").append(com.oddlabs.tt.render.Renderer.getTrianglesRendered()).append(" JHeap ").append(free_mem).append("(");
                int total_jheap = unchecked((int)((java.lang.Runtime.getRuntime().totalMemory() / (1024 * 1024))));
                buf.append(total_jheap).append("M) globj ").append(com.oddlabs.tt.resource.NativeResource.getCount());
            }
            buf.append(" FPS ").append(java.lang.Math.round((float)(1000.0f / com.oddlabs.tt.render.Renderer.getFPS()))).append(" (").append(java.lang.Math.round(com.oddlabs.tt.render.Renderer.getFPS())).append(" ms/frame)");
            com.oddlabs.tt.font.TextLineRenderer.render(renderer, com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), buf, 0, 0, java.lang.Float.NEGATIVE_INFINITY, java.lang.Float.POSITIVE_INFINITY, com.oddlabs.util.Color.WHITE);
        }
        private void init_Status()
        {
            this.buf = new java.lang.StringBuilder();
        }
        public Status() : base()
        {
            init_Status();
        }
    }
}
