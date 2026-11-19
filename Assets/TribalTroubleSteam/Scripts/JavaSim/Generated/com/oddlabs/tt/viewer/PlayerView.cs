// Translated from PlayerView.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.viewer
{
    // Java: 10
    public class PlayerView : java.lang.Object
    {
        public java.util.Set selection;
        public int selection_version;
        public int panel_submenu;
        public bool map_mode;
        public bool targeting;
        public bool placing;
        public int placing_building_index;
        public int placing_grid_x;
        public int placing_grid_y;
        public bool box_active;
        public float box_x1;
        public float box_y1;
        public float box_x2;
        public float box_y2;
        public bool cursor_on_map;
        public float cursor_x;
        public float cursor_y;
        public bool has_camera;
        public float camera_x;
        public float camera_y;
        public float camera_z;
        public float camera_horiz_angle;
        public float camera_vert_angle;
        // Java: 35
        public void setCamera(float x, float y, float z, float horiz_angle, float vert_angle)
        {
            has_camera = true;
            camera_x = x;
            camera_y = y;
            camera_z = z;
            camera_horiz_angle = horiz_angle;
            camera_vert_angle = vert_angle;
        }
        // Java: 44
        public bool hasCamera()
        {
            return has_camera;
        }
        // Java: 48
        public float getCameraX()
        {
            return camera_x;
        }
        // Java: 52
        public float getCameraY()
        {
            return camera_y;
        }
        // Java: 56
        public float getCameraZ()
        {
            return camera_z;
        }
        // Java: 60
        public float getCameraHorizAngle()
        {
            return camera_horiz_angle;
        }
        // Java: 64
        public float getCameraVertAngle()
        {
            return camera_vert_angle;
        }
        // Java: 68
        public void setCursor(float x, float y, bool on_map)
        {
            cursor_on_map = on_map;
            cursor_x = x;
            cursor_y = y;
        }
        // Java: 74
        public void setMapMode(bool on)
        {
            map_mode = on;
        }
        // Java: 78
        public bool isMapMode()
        {
            return map_mode;
        }
        // Java: 82
        public void setSelectionBox(float x1, float y1, float x2, float y2, bool active)
        {
            box_active = active;
            box_x1 = x1;
            box_y1 = y1;
            box_x2 = x2;
            box_y2 = y2;
        }
        // Java: 90
        public bool isSelectionBoxActive()
        {
            return box_active;
        }
        // Java: 95
        public float getSelectionBoxX1()
        {
            return box_x1;
        }
        // Java: 99
        public float getSelectionBoxY1()
        {
            return box_y1;
        }
        // Java: 103
        public float getSelectionBoxX2()
        {
            return box_x2;
        }
        // Java: 107
        public float getSelectionBoxY2()
        {
            return box_y2;
        }
        // Java: 111
        public void setPlacing(int building_index, int grid_x, int grid_y, bool on)
        {
            placing = on;
            placing_building_index = building_index;
            placing_grid_x = grid_x;
            placing_grid_y = grid_y;
        }
        // Java: 118
        public bool isPlacing()
        {
            return placing;
        }
        // Java: 122
        public int getPlacingBuildingIndex()
        {
            return placing_building_index;
        }
        // Java: 126
        public int getPlacingGridX()
        {
            return placing_grid_x;
        }
        // Java: 130
        public int getPlacingGridY()
        {
            return placing_grid_y;
        }
        // Java: 134
        public void setTargeting(bool on)
        {
            targeting = on;
        }
        // Java: 139
        public bool isTargeting()
        {
            return targeting;
        }
        // Java: 143
        public bool isCursorOnMap()
        {
            return cursor_on_map;
        }
        // Java: 147
        public float getCursorX()
        {
            return cursor_x;
        }
        // Java: 151
        public float getCursorY()
        {
            return cursor_y;
        }
        // Java: 155
        public void setSelection(com.oddlabs.tt.model.Selectable[] selected)
        {
            selection.clear();
            selection_version++;
            foreach (com.oddlabs.tt.model.Selectable s in selected)
            {
                if (s != null)
                {
                    selection.add(s);
                }
            }
        }
        // Java: 165
        public int getSelectionVersion()
        {
            return selection_version;
        }
        // Java: 169
        public java.util.Set getSelection()
        {
            return java.util.Collections.unmodifiableSet(selection);
        }
        // Java: 173
        public void setPanelSubmenu(int submenu)
        {
            panel_submenu = submenu;
        }
        // Java: 177
        public int getPanelSubmenu()
        {
            return panel_submenu;
        }
        // Java: 181
        public bool isSelected(com.oddlabs.tt.model.Selectable selectable)
        {
            return !selectable.isDead() && selection.contains(selectable);
        }
        private void init_PlayerView()
        {
            this.selection = new java.util.HashSet();
        }
        public PlayerView() : base()
        {
            init_PlayerView();
        }
    }
}
