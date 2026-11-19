// Translated from DirectionNode.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.pathfinder
{
    // Java: 3
    public class DirectionNode : java.lang.Object
    {
        public float inv_length;
        public int direction_x;
        public int direction_y;
        // Java: 14
        public float getInvLength()
        {
            return inv_length;
        }
        // Java: 18
        public int getDirectionX()
        {
            return direction_x;
        }
        // Java: 22
        public int getDirectionY()
        {
            return direction_y;
        }
        public DirectionNode(float inv_length, int direction_x, int direction_y) : base()
        {
            this.inv_length = inv_length;
            this.direction_x = direction_x;
            this.direction_y = direction_y;
        }
    }
}
