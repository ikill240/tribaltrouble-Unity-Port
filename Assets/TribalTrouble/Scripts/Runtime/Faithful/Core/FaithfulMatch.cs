// Engine-free view of a running game for the Unity presentation. The game itself is the Java one, unchanged:
// Menu.startNewGame -> Server + Client (loopback for single player, the matchmaking tunnel for multiplayer) ->
// WorldStarter -> WorldViewer -> PeerHub ticking the World. This class reads that WorldViewer (world, players,
// island data) and sends the player's orders through PeerHub's PlayerInterface, like the Java GUI does.
// No UnityEngine here, so it also runs in the headless test harness.
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using com.oddlabs.tt.model;
using com.oddlabs.tt.player;
using JWorld = com.oddlabs.tt.landscape.World;

namespace TribalTrouble.Faithful
{
    /// <summary>Something the simulation reported (sound, notification); queued so any thread can produce them.</summary>
    public struct SimEvent
    {
        public enum Kind { Sound, Attacked, NewSelectable, TargetGone, GameSpeed }
        public Kind kind;
        public string sound;          // resource location, e.g. "/sfx/hit3.ogg"
        public float x, y, z, gain;
        public bool relative;
        public object target;         // Selectable / Target for notifications
        public int value;
    }

    public sealed class FaithfulMatch
    {
        static readonly object s_initLock = new object();
        static bool s_inited;

        public readonly com.oddlabs.tt.viewer.WorldViewer viewer;
        public JWorld world { get; }
        public com.oddlabs.tt.resource.WorldInfo info { get; }
        public com.oddlabs.tt.resource.WorldGenerator generator { get; }
        public Player[] players { get; }
        public Player local { get; }
        public int localIndex { get; } = -1;
        /// <summary>Where the player's orders go: PeerHub's PlayerInterface (every peer executes them on the same tick).</summary>
        public PlayerInterface cmd => spectator ? NoOrders.Instance : viewer.getPeerHub().getPlayerInterface();
        /// <summary>Watching someone else's game (SpectatorWorldInitAction -> observer mode): orders go nowhere.</summary>
        public bool spectator => TribalTrouble.Bridge.UiState.observer;

        sealed class NoOrders : java.lang.Object, java.lang.reflect.InvocationHandler
        {
            static PlayerInterface s_instance;
            public static PlayerInterface Instance => s_instance ??= (PlayerInterface)J2CS.Net.JReflect.NewProxy(typeof(PlayerInterface), new NoOrders());
            public object invoke(object proxy, java.lang.reflect.Method method, object[] args) => null;
        }
        public bool multiplayer => viewer.isMultiplayer();
        public readonly ConcurrentQueue<SimEvent> events = new ConcurrentQueue<SimEvent>();

        public enum Outcome { Playing, Won, Lost, Over }
        public Outcome outcome { get; private set; } = Outcome.Playing;
        public readonly List<string> messages = new List<string>();
        /// <summary>Skirmish victory display (GameOverTrigger's rules, read-only). Campaign islands decide the end themselves.</summary>
        public bool skirmishRules = true;
        /// <summary>Text for the end screen when the game's own code ended it (GameStatsDelegate label).</summary>
        public string overText;

        public void EndWith(Outcome o, string text) { outcome = o; overText = text; }
        readonly bool[] _dead;

        public const float TickSeconds = com.oddlabs.tt.animation.AnimationManager.ANIMATION_SECONDS_PER_TICK;

        /// <summary>One-time global init (settings, static tables, the network method tables).</summary>
        public static void InitGlobals()
        {
            lock (s_initLock)
            {
                if (s_inited) return;
                java.net.URL.ResourceRoot = n => "res:" + n;   // the simulation only needs names; models / sounds live in Unity
                com.oddlabs.tt.global.Settings.setSettings(new com.oddlabs.tt.global.Settings());
                com.oddlabs.tt.global.GlobalsInit.init();
                J2CS.Generated.NetTables.Register();
                s_inited = true;
            }
        }

        public static org.joml.Vector4fc TeamColour(int index)
        {
            var c = com.oddlabs.tt.global.Settings.getSettings().team_colours;
            return (org.joml.Vector4fc)c[((index % c.Length) + c.Length) % c.Length];
        }

        public static int TeamColourCount => com.oddlabs.tt.global.Settings.getSettings().team_colours.Length;

        public FaithfulMatch(com.oddlabs.tt.viewer.WorldViewer viewer)
        {
            this.viewer = viewer;
            world = viewer.getWorld();
            var r = (com.oddlabs.tt.render.DefaultRenderer)viewer.getRenderer();
            info = r.WorldInfo;
            generator = r.Generator;
            players = world.getPlayers();
            local = viewer.getLocalPlayer();
            for (int i = 0; i < players.Length; i++) if (players[i] == local) localIndex = i;
            _dead = new bool[players.Length];
            TribalTrouble.Bridge.SimHooks.Sound = p =>
            {
                if (p.sound is com.oddlabs.tt.audio.SoundRef sr)
                    events.Enqueue(new SimEvent { kind = SimEvent.Kind.Sound, sound = sr.location, x = p.x, y = p.y, z = p.z, gain = p.gain, relative = p.relative });
            };
            TribalTrouble.Bridge.SimHooks.Attacked = t => events.Enqueue(new SimEvent { kind = SimEvent.Kind.Attacked, target = t });
            TribalTrouble.Bridge.SimHooks.NewSelectable = t => events.Enqueue(new SimEvent { kind = SimEvent.Kind.NewSelectable, target = t });
        }

        public void Detach()
        {
            TribalTrouble.Bridge.SimHooks.Sound = null;
            TribalTrouble.Bridge.SimHooks.Attacked = null;
            TribalTrouble.Bridge.SimHooks.NewSelectable = null;
        }

        public int Tick => world.getTick();

        /// <summary>Called every frame after the game loop ran: GameOverTrigger's rules from the local player's point of view, read-only.</summary>
        public void Poll()
        {
            if (!skirmishRules || outcome != Outcome.Playing || local == null) return;
            bool enemyAlive = false, teamAlive = false;
            for (int i = 0; i < players.Length; i++)
            {
                var p = players[i];
                if (_dead[i]) continue;
                if (!p.isAlive())
                {
                    _dead[i] = true;
                    if (p != local) messages.Add(p.getPlayerInfo().getName() + " has been defeated");
                }
                else if (local.isEnemy(p)) enemyAlive = true;
                else teamAlive = true;
            }
            if (!teamAlive) outcome = Outcome.Lost;
            else if (!enemyAlive) outcome = Outcome.Won;
        }

        public float HeightAt(float x, float y) => world.getHeightMap().computeInterpolatedHeight(0, x, y);
        public float MetersPerWorld => world.getHeightMap().getMetersPerWorld();
        public float SeaLevel => info.sea_level_meters();
    }
}
