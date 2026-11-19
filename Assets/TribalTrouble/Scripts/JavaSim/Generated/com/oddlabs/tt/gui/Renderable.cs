// Translated from Renderable.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 14
    public abstract class Renderable : com.oddlabs.util.ListElementImpl
    {
        public int x;
        public int y;
        public int width;
        public int height;
        public com.oddlabs.util.LinkedList children;
        public com.oddlabs.tt.gui.Renderable parent;
        // Java: 24
        public virtual com.oddlabs.tt.gui.Renderable setDim(int w, int h)
        {
            width = w;
            height = h;
            return this;
        }
        // Java: 30
        public int getWidth()
        {
            return width;
        }
        // Java: 34
        public int getHeight()
        {
            return height;
        }
        // Java: 38
        public virtual void setPos(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        // Java: 43
        public int getX()
        {
            return x;
        }
        // Java: 47
        public int getY()
        {
            return y;
        }
        // Java: 51
        public int getNumChildren()
        {
            return children.size();
        }
        // Java: 55
        public virtual com.oddlabs.tt.gui.Renderable getParent()
        {
            return parent;
        }
        // Java: 59
        public void putLast(com.oddlabs.tt.gui.Renderable child)
        {
            children.putLast(child);
        }
        // Java: 63
        public void putFirst(com.oddlabs.tt.gui.Renderable child)
        {
            children.putFirst(child);
        }
        // Java: 67
        public virtual void removeChild(com.oddlabs.tt.gui.Renderable child)
        {
            child.parent = null;
            children.remove(child);
        }
        // Java: 72
        public void clearChildren()
        {
            while (!children.isEmpty())
            {
                getLastChild().remove();
            }
        }
        // Java: 78
        public virtual void doAdd()
        {
        }
        // Java: 81
        public virtual void addChild(com.oddlabs.tt.gui.Renderable child)
        {
            child.remove();
            children.addFirst(child);
            child.parent = ((com.oddlabs.tt.gui.Renderable)self());
            child.addTree();
        }
        // Java: 88
        public com.oddlabs.tt.gui.Renderable getLastChild()
        {
            return ((com.oddlabs.tt.gui.Renderable)children.getLast());
        }
        // Java: 92
        public com.oddlabs.tt.gui.Renderable getFirstChild()
        {
            return ((com.oddlabs.tt.gui.Renderable)children.getFirst());
        }
        // Java: 96
        public void displayChanged(int width, int height)
        {
            displayChangedNotify(width, height);
            com.oddlabs.tt.gui.Renderable current = ((com.oddlabs.tt.gui.Renderable)children.getFirst());
            while (current != null)
            {
                current.displayChanged(getWidth(), getHeight());
                current = ((com.oddlabs.tt.gui.Renderable)current.getNext());
            }
        }
        // Java: 111
        public virtual void displayChangedNotify(int width, int height)
        {
        }
        // Java: 114
        public void render(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            render(renderer, java.lang.Float.NEGATIVE_INFINITY, java.lang.Float.POSITIVE_INFINITY, java.lang.Float.NEGATIVE_INFINITY, java.lang.Float.POSITIVE_INFINITY);
        }
        // Java: 123
        public virtual void render(com.oddlabs.tt.render.GUIRenderer renderer, float clip_left, float clip_right, float clip_bottom, float clip_top)
        {
            if (isHidden())
            {
                return;
            }
            if ((this is com.oddlabs.tt.gui.Clipped))
            {
                java.nio.IntBuffer scissor_box = default;
                if (org.lwjgl.opengl.GL11.glIsEnabled(org.lwjgl.opengl.GL11.GL_SCISSOR_TEST))
                {
                    scissor_box = ((java.nio.IntBuffer)java.util.Objects.requireNonNull(org.lwjgl.BufferUtils.createIntBuffer(4)));
                    org.lwjgl.opengl.GL11.glGetIntegerv(org.lwjgl.opengl.GL11.GL_SCISSOR_BOX, scissor_box);
                }
                else
                {
                    scissor_box = null;
                }
                renderer.flush();
                org.lwjgl.opengl.GL11.glEnable(org.lwjgl.opengl.GL11.GL_SCISSOR_TEST);
                float scale = getGlobalScale();
                org.lwjgl.opengl.GL11.glScissor(java.lang.JC.f2i(((float)(getRootX() * scale))), java.lang.JC.f2i(((float)(getRootY() * scale))), java.lang.JC.f2i(((float)(getWidth() * scale))), java.lang.JC.f2i(((float)(getHeight() * scale))));
                renderClipped(renderer, clip_left, clip_right, clip_bottom, clip_top);
                renderer.flush();
                if (null != scissor_box)
                {
                    org.lwjgl.opengl.GL11.glScissor(scissor_box.get(0), scissor_box.get(1), scissor_box.get(2), scissor_box.get(3));
                }
                else
                {
                    org.lwjgl.opengl.GL11.glDisable(org.lwjgl.opengl.GL11.GL_SCISSOR_TEST);
                }
            }
            else
            {
                renderClipped(renderer, clip_left, clip_right, clip_bottom, clip_top);
            }
        }
        // Java: 155
        public void renderClipped(com.oddlabs.tt.render.GUIRenderer renderer, float clip_left, float clip_right, float clip_bottom, float clip_top)
        {
            clip_left = java.lang.Math.max(transformX(clip_left), 0);
            clip_right = java.lang.Math.min(transformX(clip_right), width);
            clip_bottom = java.lang.Math.max(transformY(clip_bottom), 0);
            clip_top = java.lang.Math.min(transformY(clip_top), height);
            if (clip_left >= width || clip_right <= 0 || clip_bottom >= height || clip_top <= 0)
            {
                return;
            }
            if (!((this is com.oddlabs.tt.gui.GUIRoot)))
            {
                renderer.getMatrixStack().push();
                renderer.getMatrixStack().translate(getX(), getY(), 0);
            }
            renderGeometry(renderer);
            com.oddlabs.tt.gui.Renderable current = ((com.oddlabs.tt.gui.Renderable)children.getLast());
            while (current != null)
            {
                current.render(renderer, clip_left, clip_right, clip_bottom, clip_top);
                current = ((com.oddlabs.tt.gui.Renderable)current.getPrior());
            }
            postRender(renderer);
            if (!((this is com.oddlabs.tt.gui.GUIRoot)))
            {
                renderer.getMatrixStack().pop();
            }
        }
        // Java: 193
        public virtual void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
        }
        // Java: 200
        public virtual void postRender(com.oddlabs.tt.render.GUIRenderer renderer)
        {
        }
        // Java: 204
        public abstract bool isFocusable();
        // Java: 206
        public float getRootX()
        {
            return (parent == null ? 0 : ((float)(parent.getRootX() + getX())));
        }
        // Java: 210
        public float getRootY()
        {
            return (parent == null ? 0 : ((float)(parent.getRootY() + getY())));
        }
        // Java: 214
        public float transformX(float x)
        {
            return (float)(x - getX());
        }
        // Java: 218
        public float transformY(float y)
        {
            return (float)(y - getY());
        }
        // Java: 223
        public virtual bool isHidden()
        {
            return false;
        }
        // Java: 227
        public virtual com.oddlabs.tt.gui.Renderable pick(float x, float y)
        {
            if (isHidden())
            {
                return null;
            }
            float trans_x = transformX(x);
            float trans_y = transformY(y);
            if (isFocusable() && trans_x >= 0 && trans_y >= 0 && trans_x < getWidth() && trans_y < getHeight())
            {
                com.oddlabs.tt.gui.Renderable current = ((com.oddlabs.tt.gui.Renderable)children.getFirst());
                while (current != null)
                {
                    com.oddlabs.tt.gui.Renderable picked = current.pick(trans_x, trans_y);
                    if (picked != null)
                    {
                        return picked;
                    }
                    current = ((com.oddlabs.tt.gui.Renderable)current.getNext());
                }
                return ((com.oddlabs.tt.gui.Renderable)self());
            }
            return null;
        }
        // Java: 245
        public virtual void addTree()
        {
            doAdd();
            com.oddlabs.tt.gui.Renderable current = ((com.oddlabs.tt.gui.Renderable)children.getFirst());
            while (current != null)
            {
                current.addTree();
                current = ((com.oddlabs.tt.gui.Renderable)current.getNext());
            }
        }
        // Java: 254
        public void removeTree()
        {
            doRemove();
            com.oddlabs.tt.gui.Renderable current = ((com.oddlabs.tt.gui.Renderable)children.getFirst());
            while (current != null)
            {
                current.removeTree();
                current = ((com.oddlabs.tt.gui.Renderable)current.getNext());
            }
        }
        // Java: 263
        public virtual void doRemove()
        {
        }
        // Java: 266
        public virtual void remove()
        {
            if (parent != null)
            {
                parent.removeChild(((com.oddlabs.tt.gui.Renderable)self()));
            }
        }
        // Java: 272
        public virtual float getGlobalScale()
        {
            return (parent != null ? parent.getGlobalScale() : 1.0f);
        }
        private void init_Renderable()
        {
            this.x = 0;
            this.y = 0;
            this.width = 0;
            this.height = 0;
            this.children = new com.oddlabs.util.LinkedList();
            this.parent = null;
        }
        public Renderable() : base()
        {
            init_Renderable();
        }
    }
}
