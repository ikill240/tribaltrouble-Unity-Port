// Translated from GlobalsInit.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.global
{
    // Java: 8
    public class GlobalsInit : java.lang.Object
    {
        // Java: 9
        public static void init()
        {
            com.oddlabs.tt.global.GlobalsInit.initTextureSize();
        }
        // Java: 13
        public static int bestTextureSize(int size)
        {
            int texture_max = com.oddlabs.tt.global.Globals.MIN_TEXTURE_SIZE;
            int texture_min = texture_max;
            for (; texture_max < com.oddlabs.tt.global.Globals.MAX_TEXTURE_SIZE; texture_min = texture_max, texture_max <<= 1)
            {
                if (texture_max >= size)
                {
                    break;
                }
            }
            return ((float)((float)(size + (float)(texture_min * ((float)(com.oddlabs.tt.global.Globals.TEXTURE_WEIGHT - 1.0f)))) - (float)(texture_max * com.oddlabs.tt.global.Globals.TEXTURE_WEIGHT)) > 0 ? texture_max : texture_min);
        }
        // Java: 22
        public static void initTextureSize()
        {
            int tex_pow = default;
            int max_size = org.lwjgl.opengl.GL11.glGetInteger(org.lwjgl.opengl.GL11.GL_MAX_TEXTURE_SIZE);
            java.lang.IO.println("Maximum texture size " + (max_size));
            com.oddlabs.tt.global.Globals.MAX_TEXTURE_SIZE = max_size;
            for (tex_pow = 0; max_size != 1; max_size >>= 1, tex_pow++)
            {
                ;
            }
            com.oddlabs.tt.global.Globals.MAX_TEXTURE_POWER = tex_pow;
            com.oddlabs.tt.global.Globals.TEXTURE_SIZES = new int[com.oddlabs.tt.global.Globals.MAX_TEXTURE_SIZE];
            com.oddlabs.tt.global.Globals.TEXTURE_SPLITS = new sbyte[com.oddlabs.tt.global.Globals.MAX_TEXTURE_SIZE];
            com.oddlabs.tt.global.Globals.BEST_SIZES = new int[com.oddlabs.tt.global.Globals.MAX_TEXTURE_SIZE];
            for (int i = 0; i < com.oddlabs.tt.global.Globals.MAX_TEXTURE_SIZE; i++)
            {
                com.oddlabs.tt.global.Globals.BEST_SIZES[i] = com.oddlabs.tt.global.GlobalsInit.bestTextureSize(i);
            }
            int result = default;
            sbyte num_splits = default;
            for (int i_2 = 0; i_2 < com.oddlabs.tt.global.Globals.MAX_TEXTURE_SIZE; i_2++)
            {
                result = 0;
                num_splits = unchecked((sbyte)(0));
                while (result < i_2)
                {
                    result += com.oddlabs.tt.global.Globals.BEST_SIZES[i_2 - result];
                    num_splits++;
                }
                com.oddlabs.tt.global.Globals.TEXTURE_SIZES[i_2] = result;
                com.oddlabs.tt.global.Globals.TEXTURE_SPLITS[i_2] = num_splits;
            }
        }
        public GlobalsInit() : base()
        {
        }
    }
}
