using UnityEngine;

namespace TribalTrouble.Game
{
    /// <summary>
    /// Multiplayer options: the server domain (Java GeneralPanel "Multiplayer domain" with Update / Reset), the
    /// Steam and Discord application ids used for invites, and the ports of a server hosted on this PC.
    /// Saved in PlayerPrefs; the domain is pushed into the Java Settings that MatchmakingClient / PeerHub read.
    /// </summary>
    public static class OnlineSettings
    {
        public const string OfficialDomain = "tribaltrouble.org";
        /// <summary>Tribal Trouble on Steam (the Steam release's app): signing in with Steam on the official server only
        /// works with its tickets - Steam's public test app (480, Spacewar) gave "user does not exist" there. A Steam
        /// account that doesn't own Tribal Trouble falls back to 480 (TTSteamService).</summary>
        public const uint DefaultSteamAppId = 3945720;
        public const uint TestSteamAppId = 480;

        public static string domain = OfficialDomain;
        public static uint steamAppId = DefaultSteamAppId;
        public static bool steamEnabled = true;
        public static string discordAppId = "";
        public static bool discordEnabled = true;
        /// <summary>Player name used on servers hosted by players (direct / Steam / Relay games).</summary>
        public static string playerName = "";

        static bool s_loaded;
        static string s_domainEdit, s_domainStatus, s_steamEdit, s_discordEdit;

        public static void Load()
        {
            if (s_loaded) return;
            s_loaded = true;
            domain = PlayerPrefs.GetString("tt.domain", OfficialDomain).Trim();
            if (domain.Length == 0) domain = OfficialDomain;
            steamAppId = (uint)Mathf.Max(1, PlayerPrefs.GetInt("tt.steamAppId", (int)DefaultSteamAppId));
            // the old default (480) was saved with every options change: move it to Tribal Trouble's own app once
            if (steamAppId == TestSteamAppId && PlayerPrefs.GetInt("tt.steamAppIdV2", 0) == 0) steamAppId = DefaultSteamAppId;
            PlayerPrefs.SetInt("tt.steamAppIdV2", 1);
            steamEnabled = PlayerPrefs.GetInt("tt.steam", 1) != 0;
            discordAppId = PlayerPrefs.GetString("tt.discordAppId", "");
            discordEnabled = PlayerPrefs.GetInt("tt.discord", 1) != 0;
            playerName = PlayerPrefs.GetString("tt.playerName", "");
            if (playerName.Length == 0) playerName = System.Environment.UserName ?? "Player";
            ApplyDomain();
        }

        public static void Save()
        {
            PlayerPrefs.SetString("tt.domain", domain);
            PlayerPrefs.SetInt("tt.steamAppId", (int)steamAppId);
            PlayerPrefs.SetInt("tt.steam", steamEnabled ? 1 : 0);
            PlayerPrefs.SetString("tt.discordAppId", discordAppId);
            PlayerPrefs.SetInt("tt.discord", discordEnabled ? 1 : 0);
            PlayerPrefs.SetString("tt.playerName", playerName);
            PlayerPrefs.Save();
        }

        /// <summary>Sets the server domain (as GeneralPanel's Update button) and saves it.</summary>
        public static void SetDomain(string d)
        {
            Load();
            d = (d ?? "").Trim();
            if (d.Length == 0) return;
            domain = d;
            ApplyDomain();
            Save();
        }

        static void ApplyDomain()
        {
            try { com.oddlabs.tt.global.Settings.getSettings().setDomain(domain); } catch (System.Exception) { }
        }

        /// <summary>Where the game will connect: "matchmaking.&lt;domain&gt;", or the address itself for an IP / localhost.</summary>
        public static string MatchmakingHost => com.oddlabs.tt.global.Settings.isDirectAddress(domain) ? domain : "matchmaking." + domain;

        static void Row(string label, System.Action control)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(150));
            control();
            GUILayout.EndHorizontal();
        }

        /// <summary>The Multiplayer part of the options page.</summary>
        public static void OnGUI()
        {
            Load();
            s_domainEdit ??= domain;
            s_steamEdit ??= steamAppId.ToString();
            s_discordEdit ??= discordAppId;
            GUILayout.Label("Multiplayer");
            Row("Server domain", () =>
            {
                s_domainEdit = GUILayout.TextField(s_domainEdit, 64, GUILayout.MinWidth(160));
                if (GUILayout.Button("Update", GUILayout.Width(70)) && s_domainEdit.Trim().Length > 0) { SetDomain(s_domainEdit); s_domainStatus = "Domain updated"; }
                if (GUILayout.Button("Reset", GUILayout.Width(60))) { s_domainEdit = OfficialDomain; SetDomain(OfficialDomain); s_domainStatus = "Domain reset"; }
            });
            GUILayout.Label("<size=11><color=#aaaaaa>" + (s_domainStatus != null ? "<color=#9fe39f>" + s_domainStatus + ".</color> " : "") +
                            "Connects to " + MatchmakingHost + ":33214 (router " + (com.oddlabs.tt.global.Settings.isDirectAddress(domain) ? domain : "router." + domain) + ":11221). " +
                            "Use an IP address or localhost for a server without DNS names.</color></size>");
            Row("Your name", () => { var n = GUILayout.TextField(playerName, 20); if (n != playerName) { playerName = n; Save(); } });
            Row("Steam", () =>
            {
                bool e = GUILayout.Toggle(steamEnabled, steamEnabled ? " On" : " Off", GUILayout.Width(60));
                if (e != steamEnabled) { steamEnabled = e; Save(); }
                GUILayout.Label("App ID", GUILayout.Width(50));
                s_steamEdit = GUILayout.TextField(s_steamEdit, 10, GUILayout.Width(90));
                if (GUILayout.Button("Set", GUILayout.Width(50)) && uint.TryParse(s_steamEdit.Trim(), out var id) && id > 0) { steamAppId = id; Save(); }
            });
            GUILayout.Label("<size=11><color=#aaaaaa>Steam invites, joining friends and Steam login. 480 is Steam's free test app; " +
                            "logging in to the official server with Steam needs the game's own app id. Applies after a restart.</color></size>");
            Row("Discord", () =>
            {
                bool e = GUILayout.Toggle(discordEnabled, discordEnabled ? " On" : " Off", GUILayout.Width(60));
                if (e != discordEnabled) { discordEnabled = e; Save(); TribalTrouble.FaithfulGame.TTDiscord.Configure(); }
                GUILayout.Label("App ID", GUILayout.Width(50));
                s_discordEdit = GUILayout.TextField(s_discordEdit, 24, GUILayout.Width(170));
                if (GUILayout.Button("Set", GUILayout.Width(50))) { discordAppId = s_discordEdit.Trim(); Save(); TribalTrouble.FaithfulGame.TTDiscord.Configure(); }
            });
            GUILayout.Label("<size=11><color=#aaaaaa>Discord shows what you play and lets friends join or be invited from Discord. " +
                            "Needs an application id from discord.com/developers (free). Status: " + (TribalTrouble.FaithfulGame.TTDiscord.I?.Status ?? "off") + "</color></size>");
        }
    }
}
