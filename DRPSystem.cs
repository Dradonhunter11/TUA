using DiscordRPC;
using Terraria;
using System;
using TUA.API.Dev;
using log4net;

namespace TUA
{
    public static class DRPSystem
    {
        private static ILog Logger;

        private const string ClientID = "528086919670792233";

        private static RichPresence presence;

        private static DiscordRpcClient client;

        public static void Boot()
        {
            presence = new RichPresence()
            {
                Details = $"In Main Menu ({(Environment.Is64BitProcess ? "64" : "32")}bit)",
                State = (SteamID64Checker.Instance.VerifyDevID() && TUA.devMode)
                    ? "Debugging/Developing" :
                        (Main.netMode == 0 ? Main.rand.Next(new string[] { "Playing Alone", "Lone Samurai", "Singleplayer" })
                        : "In A Game Of " + Main.ActivePlayersCount),
                Assets = new Assets()
                {
                    LargeImageKey = "logo",
                    LargeImageText = Main.rand.NextBool() ? "Playing TUA" : "Doing Nothing"
                }
            };
            client = new DiscordRpcClient(ClientID, SteamID64Checker.CurrentSteamID64, true, -1);
            client.OnReady += (sender, args) => { Logger.Info("Rich Presence is ready for connection!"); };
            client.OnClose += (sender, args) => { Logger.Info("Rich Presense closed."); };
            client.OnError += (sender, args) => { Logger.ErrorFormat("Rich Presence failed. Code {1}, {0}", args.Message, args.Code); };
            presence.Timestamps = new Timestamps()
            {
                Start = DateTime.UtcNow,
                End = DateTime.UtcNow + TimeSpan.FromSeconds(15)
            };
            client.SetPresence(presence);
            client.Initialize();
        }

        public static void Update()
        {
            // Runs through all of discord-rpc's logging stuff, basically
            client?.Invoke();

            if (Main.rand.NextBool(100))
            {
                if (!Main.gameMenu)
                {
                    presence.Details = "Playing Terraria";
                }
                else if (Main.rand.NextBool(10000))
                {
                    presence.Details = $"In Main Menu ({(Environment.Is64BitProcess ? "64" : "32")}bit)";
                    presence.State = (SteamID64Checker.Instance.VerifyDevID() && TUA.devMode)
                        ? "Debugging/Developing" :
                            (Main.netMode == 0 ? Main.rand.Next(new string[] { "Playing Alone", "Lone Samurai", "Singleplayer" })
                            : "In A Game Of " + Main.ActivePlayersCount);
                    presence.Assets.LargeImageKey = "logo";
                    presence.Assets.LargeImageText = Main.rand.NextBool() ? "Playing TUA" : "Doing Nothing";
                }
                client.SetPresence(presence);
            }
        }

        public static void Kill() => client.Dispose();

        public static void ReloadLogger() => Logger = TUA.instance.Logger;
    }
}
