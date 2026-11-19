using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;

namespace TribalTrouble.FaithfulGame
{
    /// <summary>
    /// Discord Rich Presence over Discord's local IPC pipe (no SDK needed): shows what you play, and while you host
    /// a party with a join secret, so friends can "Ask to Join" or be invited from a Discord chat (+ → Invite to
    /// play). A friend's game receives ACTIVITY_JOIN with the secret and joins through TTOnline.
    /// Needs an application id from discord.com/developers (Options → Multiplayer → Discord).
    /// </summary>
    public sealed class TTDiscord
    {
        public static TTDiscord I { get; private set; }
        public string Status { get; private set; } = "off";

        readonly string _appId;
        NamedPipeClientStream _pipe;
        Thread _reader;
        volatile bool _stop;
        readonly object _write = new object();
        volatile string _joinSecret;
        string _lastActivity;
        readonly long _start = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        float _retryAt;

        TTDiscord(string appId) { _appId = appId; }

        /// <summary>Starts (or restarts with a new id) when Discord is enabled and an application id is set.</summary>
        public static void Configure()
        {
            Game.OnlineSettings.Load();
            string id = Game.OnlineSettings.discordEnabled ? Game.OnlineSettings.discordAppId?.Trim() : null;
            if (I != null && (string.IsNullOrEmpty(id) || I._appId != id)) { I.Close(); I = null; }
            bool windows = Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor;
            if (I == null && !string.IsNullOrEmpty(id) && windows) I = new TTDiscord(id);
        }

        public string TakeJoinSecret()
        {
            if (_pipe == null && Time.unscaledTime > _retryAt) { _retryAt = Time.unscaledTime + 30f; Connect(); }
            var s = _joinSecret;
            _joinSecret = null;
            return s;
        }

        void Connect()
        {
            for (int i = 0; i < 10 && _pipe == null; i++)
            {
                try
                {
                    var p = new NamedPipeClientStream(".", "discord-ipc-" + i, PipeDirection.InOut);
                    p.Connect(150);
                    _pipe = p;
                }
                catch (Exception) { }
            }
            if (_pipe == null) { Status = "Discord isn't running"; return; }
            try
            {
                Send(0, "{\"v\":1,\"client_id\":\"" + _appId + "\"}");
                _stop = false;
                _reader = new Thread(ReadLoop) { IsBackground = true, Name = "Discord IPC" };
                _reader.Start();
                Status = "connecting";
            }
            catch (Exception e) { Drop("Discord: " + e.Message); }
        }

        void ReadLoop()
        {
            var hdr = new byte[8];
            try
            {
                while (!_stop)
                {
                    Fill(hdr, 8);
                    int op = BitConverter.ToInt32(hdr, 0), len = BitConverter.ToInt32(hdr, 4);
                    if (len < 0 || len > 1 << 20) throw new IOException("bad frame");
                    var body = new byte[len];
                    Fill(body, len);
                    string json = Encoding.UTF8.GetString(body);
                    if (op == 3) { Send(4, json); continue; }   // ping -> pong
                    if (op == 2) throw new IOException("closed by Discord: " + json);
                    Handle(json);
                }
            }
            catch (Exception e) { if (!_stop) Drop("Discord: " + e.Message); }
        }

        void Fill(byte[] b, int n)
        {
            int o = 0;
            while (o < n) { int r = _pipe.Read(b, o, n - o); if (r <= 0) throw new IOException("pipe closed"); o += r; }
        }

        static readonly Regex Evt = new Regex("\"evt\"\\s*:\\s*\"([A-Z_]+)\"");
        static readonly Regex Secret = new Regex("\"secret\"\\s*:\\s*\"([^\"]+)\"");
        static readonly Regex UserId = new Regex("\"user\"\\s*:\\s*\\{[^}]*\"id\"\\s*:\\s*\"(\\d+)\"");

        void Handle(string json)
        {
            var m = Evt.Match(json);
            string evt = m.Success ? m.Groups[1].Value : null;
            if (evt == "READY")
            {
                Status = "connected";
                Send(1, "{\"cmd\":\"SUBSCRIBE\",\"evt\":\"ACTIVITY_JOIN\",\"nonce\":\"" + Guid.NewGuid() + "\"}");
                Send(1, "{\"cmd\":\"SUBSCRIBE\",\"evt\":\"ACTIVITY_JOIN_REQUEST\",\"nonce\":\"" + Guid.NewGuid() + "\"}");
                if (_lastActivity != null) Send(1, _lastActivity);
            }
            else if (evt == "ACTIVITY_JOIN")
            {
                var s = Secret.Match(json);
                if (s.Success) _joinSecret = s.Groups[1].Value;
            }
            else if (evt == "ACTIVITY_JOIN_REQUEST")
            {
                // a friend asked to join: let them (they still need to reach our server)
                var u = UserId.Match(json);
                if (u.Success) Send(1, "{\"cmd\":\"SEND_ACTIVITY_JOIN_INVITE\",\"args\":{\"user_id\":\"" + u.Groups[1].Value + "\"},\"nonce\":\"" + Guid.NewGuid() + "\"}");
            }
            else if (evt == "ERROR") Status = "error: " + json;
        }

        /// <summary>Sets the activity. joinSecret / party null = not joinable.</summary>
        public void SetActivity(string state, string joinSecret, string partyId, int size, int max)
        {
            var sb = new StringBuilder();
            sb.Append("{\"cmd\":\"SET_ACTIVITY\",\"args\":{\"pid\":").Append(System.Diagnostics.Process.GetCurrentProcess().Id).Append(",\"activity\":{");
            sb.Append("\"details\":\"Tribal Trouble\",\"state\":").Append(Q(state ?? ""));
            sb.Append(",\"timestamps\":{\"start\":").Append(_start).Append('}');
            sb.Append(",\"assets\":{\"large_text\":\"Tribal Trouble\"}");
            if (joinSecret != null && partyId != null)
            {
                sb.Append(",\"party\":{\"id\":").Append(Q(partyId)).Append(",\"size\":[").Append(Math.Max(1, size)).Append(',').Append(Math.Max(size, max)).Append("]}");
                sb.Append(",\"secrets\":{\"join\":").Append(Q(joinSecret)).Append('}');
            }
            sb.Append(",\"instance\":false}},\"nonce\":\"").Append(Guid.NewGuid()).Append("\"}");
            string json = sb.ToString();
            if (json == _lastActivity) return;
            _lastActivity = json;
            if (_pipe != null && Status == "connected") Send(1, json);
        }

        static string Q(string s) => "\"" + s.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"";

        void Send(int op, string json)
        {
            var p = _pipe;
            if (p == null) return;
            var body = Encoding.UTF8.GetBytes(json);
            var frame = new byte[8 + body.Length];
            BitConverter.GetBytes(op).CopyTo(frame, 0);
            BitConverter.GetBytes(body.Length).CopyTo(frame, 4);
            body.CopyTo(frame, 8);
            try { lock (_write) { p.Write(frame, 0, frame.Length); p.Flush(); } }
            catch (Exception e) { Drop("Discord: " + e.Message); }
        }

        void Drop(string why)
        {
            Status = why;
            try { _pipe?.Dispose(); } catch (Exception) { }
            _pipe = null;
        }

        public void Close()
        {
            _stop = true;
            Drop("off");
        }
    }
}
