// Translated from StartLayer.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.mapeditor
{
    // Java: 16
    public class StartLayer : java.lang.Object
    {
        // Java: 18
        public class Start : java.lang.Object
        {
            public int player() { return player_f; }
            public int x() { return x_f; }
            public int y() { return y_f; }
            public int team() { return team_f; }
            public Start(int player, int x, int y, int team)
            {
                this.player_f = player;
                this.x_f = x;
                this.y_f = y;
                this.team_f = team;
            }
            public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is Start r)) return false; return true && java.lang.JO.equals(player_f, r.player_f) && java.lang.JO.equals(x_f, r.x_f) && java.lang.JO.equals(y_f, r.y_f) && java.lang.JO.equals(team_f, r.team_f); }
            public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(player_f); h = 31 * h + java.lang.JO.hashCode(x_f); h = 31 * h + java.lang.JO.hashCode(y_f); h = 31 * h + java.lang.JO.hashCode(team_f); } return h; }
            public override string toString() { return "Start[" + "player=" + java.lang.JO.toString(player_f) + ", x=" + java.lang.JO.toString(x_f) + ", y=" + java.lang.JO.toString(y_f) + ", team=" + java.lang.JO.toString(team_f) + "]"; }
            public int player_f;
            public int x_f;
            public int y_f;
            public int team_f;
        }
        public java.util.List starts;
        public int grid_size;
        // Java: 33
        public void place(int player, int x, int y, int team)
        {
            starts.removeIf(new java.util.function.Predicate_L((object s_l) =>
            {
                com.oddlabs.tt.mapeditor.StartLayer.Start s = (com.oddlabs.tt.mapeditor.StartLayer.Start)s_l;
                return s.player() == player;
            }));
            starts.add(new com.oddlabs.tt.mapeditor.StartLayer.Start(player, java.lang.Math.clamp(x, 0, grid_size - 1), java.lang.Math.clamp(y, 0, grid_size - 1), team));
        }
        // Java: 39
        public bool removeNearest(float gx, float gy, float max_distance)
        {
            com.oddlabs.tt.mapeditor.StartLayer.Start nearest = null;
            float best = (float)(max_distance * max_distance);
            foreach (com.oddlabs.tt.mapeditor.StartLayer.Start s in java.lang.JIter.of(starts))
            {
                float dx = (float)(s.x() - gx);
                float dy = (float)(s.y() - gy);
                float d = (float)((float)(dx * dx) + (float)(dy * dy));
                if (d <= best)
                {
                    best = d;
                    nearest = s;
                }
            }
            return nearest != null && starts.remove(nearest);
        }
        // Java: 58
        public void autoPlace(int count)
        {
            starts.clear();
            int center = grid_size / 2;
            float radius = (float)(0.35f * grid_size);
            float angle = (float)(0.5f * ((float)(java.lang.Math.PI)));
            float step = (float)((float)(2.0f * ((float)(java.lang.Math.PI))) / count);
            for (int i = 0; i < count; i++)
            {
                int x = java.lang.JC.f2i(((float)((float)((float)(radius * ((float)(java.lang.StrictMath.cos(angle)))) + center) + 0.5f)));
                int y = java.lang.JC.f2i(((float)((float)((float)(radius * ((float)(java.lang.StrictMath.sin(angle)))) + center) + 0.5f)));
                place(i, x, y, (-1));
                angle = (float)(angle + step);
            }
        }
        // Java: 72
        public java.util.List all()
        {
            return starts;
        }
        // Java: 77
        public java.util.List toSaved()
        {
            java.util.List saved = new java.util.ArrayList(starts.size());
            foreach (com.oddlabs.tt.mapeditor.StartLayer.Start s in java.lang.JIter.of(starts))
            {
                saved.add(new int[] { s.player(), s.x(), s.y(), s.team() });
            }
            return saved;
        }
        private void init_StartLayer()
        {
            this.starts = new java.util.ArrayList();
        }
        public StartLayer(int grid_size, java.util.List saved) : base()
        {
            init_StartLayer();
            this.grid_size = grid_size;
            if (saved != null)
            {
                foreach (int[] s in java.lang.JIter.of(saved))
                {
                    place(s[0], s[1], s[2], s[3]);
                }
            }
        }
    }
}
