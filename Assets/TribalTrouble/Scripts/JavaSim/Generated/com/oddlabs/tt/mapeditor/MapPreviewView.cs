// Translated from MapPreviewView.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.mapeditor
{
    // Java: 23
    public class MapPreviewView : com.oddlabs.tt.gui.GUIObject
    {
        public static org.joml.Vector4fc BACKGROUND;
        public com.oddlabs.tt.gui.Label label_note;
        public com.oddlabs.tt.mapeditor.MapPreview preview;
        public com.oddlabs.tt.render.Texture texture;
        // Java: 39
        public void show(com.oddlabs.tt.mapeditor.MapPreview new_preview, string note)
        {
            preview = new_preview;
            closeTexture();
            label_note.set((new_preview == null ? note : ""));
            if (getParentGUIRoot() != null)
            {
                upload();
            }
        }
        // Java: 48
        public void upload()
        {
            com.oddlabs.tt.mapeditor.MapPreview shown = preview;
            if (shown == null || texture != null)
            {
                return;
            }
            texture = com.oddlabs.tt.util.PortHost.textureFromRGB(shown.size(), shown.rgb());
        }
        // Java: 55
        public void closeTexture()
        {
            if (texture != null)
            {
                texture.close();
                texture = null;
            }
        }
        // Java: 62
        public override void doAdd()
        {
            base.doAdd();
            upload();
        }
        // Java: 68
        public override void doRemove()
        {
            base.doRemove();
            closeTexture();
        }
        // Java: 74
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            renderer.drawColoredQuad(0, 0, getWidth(), getHeight(), com.oddlabs.tt.mapeditor.MapPreviewView.BACKGROUND);
            if (texture != null)
            {
                renderer.drawTexture(texture, 0, 0, getWidth(), getHeight(), 0.0f, 0.0f, 1.0f, 1.0f, com.oddlabs.util.Color.WHITE);
            }
        }
        static MapPreviewView()
        {
            MapPreviewView.BACKGROUND = new org.joml.Vector4f(0.0f, 0.0f, 0.0f, 0.45f);
        }
        public MapPreviewView(int size) : base()
        {
            label_note = new com.oddlabs.tt.gui.Label("", com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), size, com.oddlabs.tt.gui.Origin.AT_MIDDLE);
            label_note.setPos(0, (size - label_note.getHeight()) / 2);
            addChild(label_note);
            setDim(size, size);
        }
    }
}
