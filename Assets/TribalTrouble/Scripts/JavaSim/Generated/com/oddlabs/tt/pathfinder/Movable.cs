// Translated from Movable.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.pathfinder
{
    // Java: 3
    public interface Movable
    {
        // Java: 4
        bool isMoving();
        // Java: 6
        int getGridX();
        // Java: 8
        int getGridY();
        // Java: 10
        void setPosition(float x, float y);
        // Java: 12
        void setDirection(float dir_x, float dir_y);
        // Java: 14
        float getPositionX();
        // Java: 16
        float getPositionY();
        // Java: 18
        com.oddlabs.tt.pathfinder.PathTracker getTracker();
        // Java: 20
        void free();
        // Java: 22
        void setGridPosition(int grid_x, int grid_y);
        // Java: 24
        void occupy();
        // Java: 26
        void markBlocking();
        // Java: 28
        bool isDead();
    }
}
