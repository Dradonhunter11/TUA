using Dimlibs;
using DiscordRPC;
using log4net;
using System;
using System.Linq;
using Terraria;
using TUA.API.Dev;
using TUA.API.EventManager;

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
                    LargeImageText = "Loading mods"
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
            client.Initialize();
            client.SetPresence(presence);
        }

        public static void Update()
        {
            // Runs through all of discord-rpc's logging stuff, basically
            client?.Invoke();

            if (Main.rand.NextBool(100))
            {
                presence.Assets.LargeImageKey = "logo";
                if (!Main.gameMenu)
                {
                    presence.Details = "Playing Terraria"; 
                    /*if (Main.LocalPlayer.GetModPlayer<DimPlayer>().getCurrentDimension() == "Solar")
                    {
                        presence.Assets.LargeImageText = Main.rand.NextBool() ? "Playing TUA" : "Exploring solar";
                        presence.Details = Main.LocalPlayer.name + " is exploring the solar dimension";
                    }*/

                    if (!Main.npc.Any(i => i.boss) && !MoonEventManagerWorld.moonEventList.Any(i => i.Value.IsActive))
                    {
                        if (!NPC.downedBoss1)
                        {
                            presence.Assets.LargeImageText = Main.rand.NextBool() ? "Playing TUA" : "The start of a new day";
                            presence.Details = Main.LocalPlayer.name + " is before EoC";
                        }
                        else if (!NPC.downedBoss2)
                        {
                            presence.Assets.LargeImageText = Main.rand.NextBool() ? "Playing TUA" : "Preparing to fight the evil";
                            presence.Details = Main.LocalPlayer.name + " is before the evil boss";
                        }
                        else if (!NPC.downedBoss3)
                        {
                            presence.Assets.LargeImageText = Main.rand.NextBool() ? "Playing TUA" : "Trying to help the old man";
                            presence.Details = Main.LocalPlayer.name + " is before skeletron";
                        }
                        else if (!Main.hardMode)
                        {
                            presence.Assets.LargeImageText = Main.rand.NextBool() ? "Playing TUA" : "Preparing for the initial raids";
                            presence.Details = Main.LocalPlayer.name + " is before hardmode";
                        }
                        else if (Main.hardMode)
                        {
                            presence.Assets.LargeImageText = Main.rand.NextBool() ? "Playing TUA" : "A new era of challenge";
                            presence.Details = Main.LocalPlayer.name + " is in hardmode";
                        }
                        else if (NPC.downedMechBossAny && !(NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3))
                        {
                            presence.Assets.LargeImageText = Main.rand.NextBool() ? "Playing TUA" : "The mechanical arise";
                            presence.Details = Main.LocalPlayer.name + " is after the death of a mechanical boss";
                        }
                        else if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
                        {
                            presence.Assets.LargeImageText = Main.rand.NextBool() ? "Playing TUA" : "The jungle arise";
                            presence.Details = Main.LocalPlayer.name + " is before plantera";
                        }
                        else if (NPC.downedPlantBoss)
                        {
                            presence.Assets.LargeImageText = Main.rand.NextBool() ? "Playing TUA" : "The legends of a lizhard divinity";
                            presence.Details = Main.LocalPlayer.name + " is before the golem";
                        }
                        else if (NPC.downedGolemBoss)
                        {
                            presence.Assets.LargeImageText = Main.rand.NextBool() ? "Playing TUA" : "The ancient cult of the moon";
                            presence.Details = Main.LocalPlayer.name + " is before the cultist";
                        }
                        else if (NPC.downedMoonlord)
                        {
                            presence.Assets.LargeImageText = Main.rand.NextBool() ? "Playing TUA" : "Before the lord, there were the god";
                            presence.Details = Main.LocalPlayer.name + " has beaten the moon lord";
                        }
                        else if (TUAWorld.EoCPostMLDowned)
                        {
                            presence.Assets.LargeImageText = Main.rand.NextBool() ? "Playing TUA" : "The revenge of the eye";
                            presence.Details = Main.LocalPlayer.name + " has beaten the ultra eye of cthulhu";
                        }
                        else if (TUAWorld.ApoMoonDowned)
                        {
                            presence.Assets.LargeImageText = Main.rand.NextBool() ? "Playing TUA" : "The destruction of the moon";
                            presence.Details = Main.LocalPlayer.name + " has beaten the apocalypse moon";
                        }
                        else if (TUAWorld.EoADowned)
                        {
                            presence.Assets.LargeImageText = Main.rand.NextBool() ? "Playing TUA" : "The death of a god";
                            presence.Details = Main.LocalPlayer.name + " has beaten the ultra eye of cthulhu";
                        }
                    }
                    else
                    {
                        if (NPC.LunarApocalypseIsUp)
                        {
                            presence.Assets.LargeImageText = Main.rand.NextBool() ? "Playing TUA" : "The dimension started to clash";
                            presence.Details = Main.LocalPlayer.name + " is fighting the lunar apocalypse";
                        }
                    }


                    presence.Assets.LargeImageText = Main.rand.NextBool() ? "Playing TUA" : "exploring the wasteland";
                    presence.Details = Main.LocalPlayer.name + " is currently in the wasteland";
                }
                else
                {
                    presence.Details = $"In Main Menu ({(Environment.Is64BitProcess ? "64" : "32")}bit)";
                    presence.State = (SteamID64Checker.Instance.VerifyDevID() && TUA.devMode)
                        ? "Debugging/Developing" :
                            (Main.netMode == 0 ? Main.rand.Next(new string[] { "Playing Alone", "Lone Samurai", "Singleplayer" })
                            : "In A Game Of " + Main.ActivePlayersCount);

                    presence.Assets.LargeImageText = Main.rand.NextBool() ? "Playing TUA" : "Doing Nothing";
                }
                client.SetPresence(presence);
            }
        }

        public static void Kill() => client.Dispose();

        public static void ReloadLogger() => Logger = TUA.instance.Logger;
    }
}
