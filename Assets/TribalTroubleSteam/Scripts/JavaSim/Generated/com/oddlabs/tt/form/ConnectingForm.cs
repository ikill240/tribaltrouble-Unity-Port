// Translated from ConnectingForm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 25
    public class ConnectingForm : com.oddlabs.tt.gui.Form, com.oddlabs.tt.net.ConfigurationListener
    {
        public static java.util.ResourceBundle bundle;
        // Java: 28
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.form.ConnectingForm.bundle, key, args);
        }
        public com.oddlabs.tt.form.SelectGameMenu owner;
        public bool multiplayer;
        public com.oddlabs.tt.gui.GUIRoot gui_root;
        public com.oddlabs.tt.net.GameNetwork game_network;
        // Java: 58
        public void connected(com.oddlabs.tt.net.Client client, com.oddlabs.matchmaking.Game game, com.oddlabs.tt.resource.WorldGenerator generator, int player_slot, int player_count)
        {
            if (multiplayer)
            {
                java.util.Random random = new java.util.Random(com.oddlabs.tt.@event.LocalEventQueue.getQueue().getHighPrecisionManager().getTick());
                random.nextFloat();
                int race = java.lang.JC.f2i(((float)((float)(random.nextFloat() * (com.oddlabs.tt.model.RacesResources.getNumRaces() - 1)) + 0.5f)));
                int team = player_slot;
                if (game.isRated())
                {
                    team = player_slot % 2;
                }
                client.getServerInterface().setPlayerSlot(player_slot, com.oddlabs.tt.net.PlayerSlot.HUMAN, race, team, false, com.oddlabs.tt.net.PlayerSlot.AI_NONE);
                remove();
                owner.createGameMenu(game_network, game, generator, player_slot, player_count);
            }
            else
            {
            }
        }
        // Java: 80
        public void chat(int player_slot, string chat_v)
        {
        }
        // Java: 83
        public void setPlayers(com.oddlabs.tt.net.PlayerSlot[] players)
        {
        }
        // Java: 88
        public void connectionLost()
        {
            remove();
            gui_root.addModalForm(new com.oddlabs.tt.form.MessageForm(i18n("connection_lost")));
        }
        // Java: 94
        public void gameStarted()
        {
            remove();
        }
        // Java: 101
        public override void doCancel()
        {
            game_network.close();
        }
        static ConnectingForm()
        {
            ConnectingForm.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.form.ConnectingForm).getName());
        }
        public ConnectingForm(com.oddlabs.tt.net.GameNetwork game_network, com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.form.SelectGameMenu owner, bool multiplayer) : base()
        {
            this.game_network = game_network;
            this.gui_root = gui_root;
            this.owner = owner;
            this.multiplayer = multiplayer;
            com.oddlabs.tt.gui.Label info_label = new com.oddlabs.tt.gui.Label(i18n((multiplayer ? "connecting" : "starting")), com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont());
            addChild(info_label);
            com.oddlabs.tt.gui.HorizButton cancel_button = new com.oddlabs.tt.gui.CancelButton(120);
            addChild(cancel_button);
            cancel_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                this.cancel();
            }));
            info_label.place();
            cancel_button.place(info_label, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            compileCanvas();
            centerPos();
        }
    }
}
