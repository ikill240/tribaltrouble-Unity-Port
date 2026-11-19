// Translated from MatrixStack.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.render
{
    // Java: 14
    public class MatrixStack : java.lang.Object
    {
        public java.util.Deque stack;
        // Java: 17
        public interface TopListener
        {
            // Java: 18
            void topChanging(org.joml.Matrix4fc matrix);
        }
        public sealed class TopListener_L : java.lang.Object, TopListener
        {
            readonly System.Action<org.joml.Matrix4fc> f;
            public TopListener_L(System.Action<org.joml.Matrix4fc> f) { this.f = f; }
            public void topChanging(org.joml.Matrix4fc p0) { f(p0); }
        }
        public com.oddlabs.tt.render.MatrixStack.TopListener topListener;
        // Java: 32
        public org.joml.Matrix4f push()
        {
            org.joml.Matrix4f copy = new org.joml.Matrix4f(current());
            stack.push(copy);
            if (null != topListener)
            {
                topListener.topChanging(current());
            }
            return current();
        }
        // Java: 41
        public org.joml.Matrix4f pop()
        {
            if (null != topListener)
            {
                topListener.topChanging(current());
            }
            if (stack.size() > 1)
            {
                stack.pop();
            }
            else
            {
                clear();
            }
            return current();
        }
        // Java: 53
        public org.joml.Matrix4f current()
        {
            return ((org.joml.Matrix4f)stack.element());
        }
        // Java: 57
        public org.joml.Matrix4f clear()
        {
            stack.clear();
            stack.push(new org.joml.Matrix4f());
            return current();
        }
        // Java: 65
        public com.oddlabs.tt.render.MatrixStack translate(float x, float y, float z)
        {
            current().translate(x, y, z);
            return this;
        }
        // Java: 79
        public com.oddlabs.tt.render.MatrixStack rotate(float angle, float x, float y, float z)
        {
            current().rotate(((float)(java.lang.Math.toRadians(angle))), x, y, z);
            return this;
        }
        // Java: 84
        public com.oddlabs.tt.render.MatrixStack scale(float x, float y, float z)
        {
            current().scale(x, y, z);
            return this;
        }
        // Java: 89
        public com.oddlabs.tt.render.MatrixStack multiply(org.joml.Matrix4f matrix)
        {
            current().mul(matrix);
            return this;
        }
        private void init_MatrixStack()
        {
            this.stack = new java.util.ArrayDeque();
        }
        public MatrixStack() : this(null)
        {
        }
        public MatrixStack(com.oddlabs.tt.render.MatrixStack.TopListener topListener) : base()
        {
            init_MatrixStack();
            clear();
            this.topListener = topListener;
        }
    }
}
