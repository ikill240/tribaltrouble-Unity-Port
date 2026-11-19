// Translated from TextureGenerator.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.procedural
{
    // Java: 9
    public abstract class TextureGenerator : java.lang.Object, java.util.function.Supplier
    {
        // Java: 10
        public abstract com.oddlabs.tt.render.Texture[] generate();
        // Java: 12
        public object get()
        {
            return generate();
        }
        // Java: 17
        public override int hashCode()
        {
            return java.lang.JO.hashCode(this.GetType().getSimpleName());
        }
        // Java: 22
        public override bool equals(object o)
        {
            return this.GetType().isInstance(o);
        }
        public TextureGenerator() : base()
        {
        }
    }
}
