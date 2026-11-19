// Translated from TargetDelegate.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.@delegate
{
    // Java: 13
    public class TargetDelegate : com.oddlabs.tt.@delegate.ControllableCameraDelegate
    {
        public com.oddlabs.tt.model.Action action;
        public bool sailing;
        // Java: 30
        public override bool canHoverBehind()
        {
            return true;
        }
        // Java: 35
        public override com.oddlabs.tt.gui.CursorType getCursorType()
        {
            return com.oddlabs.tt.gui.CursorType.TARGET;
        }
        // Java: 40
        public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
            @event.consumeAction(com.oddlabs.tt.input.GameAction.UI_ACTIVATE);
            if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.PRESSED) || ((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.REPEAT))
            {
                if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_CANCEL))
                {
                    pop();
                    @event.consume();
                    return;
                }
            }
            if (handleMapModeWhileTransient(@event))
            {
                return;
            }
            base.handleInput(@event);
            if (@event.isConsumed())
            {
                return;
            }
        }
        // Java: 63
        public override void mousePressed(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
            if (((object)button == (object)com.oddlabs.tt.gui.MouseButton.LEFT))
            {
                if (sailing)
                {
                    getViewer().getPicker().pickSailingTarget(getViewer().getSelection().getCurrentSelection(), getViewer().getGUIRoot().getDelegate().getCamera().getState(), getViewer().getPeerHub().getPlayerInterface(), x, y);
                }
                else
                {
                    getViewer().getPicker().pickTarget(getViewer().getSelection().getCurrentSelection(), getViewer().getGUIRoot().getDelegate().getCamera().getState(), getViewer().getPeerHub().getPlayerInterface(), x, y, action);
                }
                pop();
            }
            else
            {
                base.mousePressed(button, x, y);
            }
        }
        public TargetDelegate(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.camera.GameCamera camera, com.oddlabs.tt.model.Action action) : base(viewer, camera)
        {
            this.action = action;
            this.sailing = false;
        }
        public TargetDelegate(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.camera.GameCamera camera, com.oddlabs.tt.model.Action action, bool sailing) : base(viewer, camera)
        {
            this.action = action;
            this.sailing = sailing;
        }
    }
}
