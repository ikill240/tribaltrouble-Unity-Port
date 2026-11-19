// Translated from FormationLayout.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player
{
    // Java: 15
    public class FormationLayout : java.lang.Object
    {
        // Java: 25
        public static java.util.List computeOffsets(com.oddlabs.tt.player.Formation formation, int count, float facing_dx, float facing_dy)
        {
            java.util.List offsets = new java.util.ArrayList(java.lang.Math.max(count, 0));
            if (count <= 0)
            {
                return offsets;
            }
            float spacing = ((formation).ordinal() switch { 5 /*TIGHT*/ => (float)(1.2f), 6 /*LOOSE*/ => (float)(3.0f), _ => (float)(1.8f) });
            if (((object)formation == (object)com.oddlabs.tt.player.Formation.CIRCLE))
            {
                if (count == 1)
                {
                    offsets.add(new float[] { 0.0f, 0.0f });
                }
                else
                {
                    float radius = (float)((float)(spacing * count) / ((float)(2.0f * ((float)(java.lang.Math.PI)))));
                    for (int i = 0; i < count; i++)
                    {
                        float angle = ((float)((2 * java.lang.Math.PI * i / count)));
                        offsets.add(new float[] { (float)(((float)(java.lang.StrictMath.cos(angle))) * radius), (float)(((float)(java.lang.StrictMath.sin(angle))) * radius) });
                    }
                }
            }
            else if (((object)formation == (object)com.oddlabs.tt.player.Formation.STAR))
            {
                if (count == 1)
                {
                    offsets.add(new float[] { 0.0f, 0.0f });
                }
                else
                {
                    int STAR_POINTS = 6;
                    float outer_radius = (float)((float)(spacing * count) / ((float)(2.0f * ((float)(java.lang.Math.PI)))));
                    float inner_radius = (float)(outer_radius * 0.5f);
                    for (int i_2 = 0; i_2 < count; i_2++)
                    {
                        float angle_2 = ((float)((2 * java.lang.Math.PI * i_2 / count)));
                        float phase = ((float)((java.lang.StrictMath.cos((float)(angle_2 * STAR_POINTS)) * 0.5d + 0.5d)));
                        float radius_2 = (float)(inner_radius + (float)(((float)(outer_radius - inner_radius)) * phase));
                        offsets.add(new float[] { (float)(((float)(java.lang.StrictMath.cos(angle_2))) * radius_2), (float)(((float)(java.lang.StrictMath.sin(angle_2))) * radius_2) });
                    }
                }
            }
            else if (((object)formation == (object)com.oddlabs.tt.player.Formation.DIAMOND))
            {
                int placed = 0;
                offsets.add(new float[] { 0.0f, 0.0f });
                placed++;
                for (int d = 1; placed < count; d++)
                {
                    int x = d;
                    int y = 0;
                    int[][] steps = new int[][] { new int[] { (-1), 1 }, new int[] { (-1), (-1) }, new int[] { 1, (-1) }, new int[] { 1, 1 } };
                    foreach (int[] step in steps)
                    {
                        for (int k = 0; k < d && placed < count; k++)
                        {
                            offsets.add(new float[] { (float)(x * spacing), (float)(y * spacing) });
                            placed++;
                            x += step[0];
                            y += step[1];
                        }
                    }
                }
            }
            else
            {
                int cols = java.lang.JC.d2i(java.lang.Math.ceil(java.lang.Math.sqrt(count)));
                int rows = java.lang.JC.d2i(java.lang.Math.ceil((float)(((float)(count)) / cols)));
                int placed_2 = 0;
                for (int row = 0; row < rows && placed_2 < count; row++)
                {
                    for (int col = 0; col < cols && placed_2 < count; col++)
                    {
                        float x_2 = (float)(((float)(col - (float)((cols - 1) / 2.0f))) * spacing);
                        float y_2 = (float)(((float)(row - (float)((rows - 1) / 2.0f))) * spacing);
                        offsets.add(new float[] { x_2, y_2 });
                        placed_2++;
                    }
                }
            }
            float len = ((float)(java.lang.Math.sqrt((float)((float)(facing_dx * facing_dx) + (float)(facing_dy * facing_dy)))));
            if (len > 1.0E-4f)
            {
                float dir_x = (float)(facing_dx / len);
                float dir_y = (float)(facing_dy / len);
                foreach (float[] o in java.lang.JIter.of(offsets))
                {
                    float x_3 = o[0];
                    float y_3 = o[1];
                    o[0] = (float)((float)(x_3 * dir_x) - (float)(y_3 * dir_y));
                    o[1] = (float)((float)(x_3 * dir_y) + (float)(y_3 * dir_x));
                }
            }
            return offsets;
        }
        public FormationLayout() : base()
        {
        }
    }
}
