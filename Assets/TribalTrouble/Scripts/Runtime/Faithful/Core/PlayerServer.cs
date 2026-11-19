// A Tribal Trouble server hosted by a player: the translated matchmaking server and router
// (com.oddlabs.matchserver.EmbeddedServer) on a background thread. Friends connect with this machine's address as
// their server domain (directly, or through the Steam / Relay tunnels, which forward to these ports). The game run
// with -server (or in batch mode) runs only this: a dedicated server. Engine-free.
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;

namespace TribalTrouble.Faithful
{
    public static class PlayerServer
    {
        public const int MatchmakingPort = 33214;   // MatchmakingServerInterface.MATCHMAKING_SERVER_PORT
        public const int RouterPort = 11221;        // RouterInterface.PORT

        static Thread s_thread;
        static volatile bool s_stop, s_running, s_starting;
        static volatile int s_online;
        public static volatile string Error;
        public static DateTime StartedAt { get; private set; }

        public static bool Running => s_running;
        public static bool Starting => s_starting;
        public static int Online => s_online;

        /// <summary>Starts the server (no-op when it already runs). Errors (e.g. the ports are in use) end up in Error.</summary>
        public static void Start()
        {
            if (s_thread != null && s_thread.IsAlive) return;
            Error = null;
            s_stop = false;
            s_starting = true;
            s_thread = new Thread(Run) { IsBackground = true, Name = "TT server" };
            s_thread.Start();
        }

        public static void Stop()
        {
            s_stop = true;
            var t = s_thread;
            if (t != null && t != Thread.CurrentThread) t.Join(3000);
            s_thread = null;
        }

        static void Run()
        {
            com.oddlabs.matchserver.EmbeddedServer srv = null;
            try
            {
                srv = new com.oddlabs.matchserver.EmbeddedServer();
                StartedAt = DateTime.UtcNow;
                s_running = true;
                s_starting = false;
                java.lang.IO.println("Server running: matchmaking port " + MatchmakingPort + ", router port " + RouterPort);
                while (!s_stop)
                {
                    srv.tick(100);
                    s_online = srv.getOnlineCount();
                }
            }
            catch (Exception e)
            {
                string m = e.InnerException?.Message ?? e.Message;
                if (m.Contains("Bind") || m.Contains("in use") || m.Contains("Only one usage"))
                    m = "Ports " + MatchmakingPort + " / " + RouterPort + " are in use (is another server running on this PC?)";
                Error = "The server couldn't start: " + m;
                java.lang.IO.println(Error + "\n" + e);
            }
            finally
            {
                try { srv?.close(); } catch (Exception) { }
                s_running = false;
                s_starting = false;
                s_online = 0;
            }
        }

        /// <summary>This machine's LAN (IPv4) addresses, for friends on the same network.</summary>
        public static List<string> LanAddresses()
        {
            var r = new List<string>();
            try
            {
                foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (ni.OperationalStatus != OperationalStatus.Up || ni.NetworkInterfaceType == NetworkInterfaceType.Loopback) continue;
                    foreach (var a in ni.GetIPProperties().UnicastAddresses)
                        if (a.Address.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(a.Address))
                        {
                            string s = a.Address.ToString();
                            if (!s.StartsWith("169.254.") && !r.Contains(s)) r.Add(s);
                        }
                }
            }
            catch (Exception) { }
            return r;
        }
    }
}
