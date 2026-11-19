// Hand-written replacement for the generated stub: PlacingDelegate.render3D asks for the placing ghost here (the
// building and the legal sites around the cursor); the Unity view (SteamWorldOverlay) calls render3D while it
// draws the frame and draws what was asked for, as the Java BuildingGhostRenderer did.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.render
{
    public class BuildingGhostRenderer : java.lang.Object
    {
        public sealed class Request
        {
            public com.oddlabs.tt.landscape.World world;
            public com.oddlabs.tt.model.BuildingTemplate template;
            public int gridX, gridY;   // the placing centre (UnitGrid coordinates)
        }

        /// <summary>The ghost asked for since the view last took one, or null.</summary>
        public static volatile Request Pending;

        public virtual void render(com.oddlabs.tt.landscape.World p0, com.oddlabs.tt.model.BuildingTemplate p1, int p2, int p3, com.oddlabs.tt.render.LandscapeRenderer p4, com.oddlabs.tt.render.RenderQueues p5, com.oddlabs.tt.render.MatrixStack p6, com.oddlabs.tt.render.MatrixStack p7)
        {
            if (p0 == null || p1 == null) return;
            Pending = new Request { world = p0, template = p1, gridX = p2, gridY = p3 };
        }
        public BuildingGhostRenderer() { }
    }
}
