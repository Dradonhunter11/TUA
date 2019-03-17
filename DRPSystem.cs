using DiscordRPC;
using System;
using System.Linq;
using Terraria;
using Terraria.Utilities;
using TUA.API.Dev;
using TUA.API.EventManager;

namespace TUA
{
    public static class DRPSystem
    {
        private const string ClientID = "528086919670792233";

        private static RichPresence presence;

        private static DiscordRpcClient client;
        
        private static string currentState;

        public static void Boot()
        {
	        UnifiedRandom rand = new UnifiedRandom();
            currentState = Main.netMode == 0
                ? rand.Next(new string[] { "Playing Alone", "Lone Samurai", "Singleplayer" })
                : rand.Next(new string[] { "Playing With Friends", "Multiplayer" });

            presence = new RichPresence()
            {
                Details = $"In Main Menu ({(Environment.Is64BitProcess ? "64" : "32")}bit)",
                State = (SteamID64Checker.Instance.VerifyDevID() && TUA.devMode)
                    ? "Debugging/Developing" : currentState,
                Assets = new Assets()
                {
                    LargeImageKey = "logo",
                    LargeImageText = "Loading mods"
                }
            };
            if (Main.netMode != 0)
            {
                presence.Party = new Party()
                {
                    Size = Main.ActivePlayersCount,
                    Max = Main.maxNetPlayers
                };
            }
            client = new DiscordRpcClient(ClientID, SteamID64Checker.CurrentSteamID64, true, -1);
            // client.OnReady += (sender, args) => { TUA.instance.Logger.Info("Rich Presence is ready for connection!"); };
            // client.OnClose += (sender, args) => { TUA.instance.Logger.Info("Rich Presense closed."); };
            // client.OnError += (sender, args) => { TUA.instance.Logger.ErrorFormat("Rich Presence failed. Code {1}, {0}", args.Message, args.Code); };
            presence.Timestamps = new Timestamps()
            {
                Start = DateTime.UtcNow,
            };
            client.Initialize();
            client.SetPresence(presence);
        }

        // We should get some images pertaining to each boss
        // client.UpdateLargeAsset("EoC logo", Main.rand.NextBool() ? "Playing TUA" : "The start of a new day");
        public static void Update()
        {
	        if (client == null)
		        return;

            // Runs through all of discord-rpc's logging stuff, basically
            client.Invoke();

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
                        if (TUAWorld.EoADowned)
                        {
                            client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The death of a god");
                            client.UpdateDetails(Main.LocalPlayer.name + " has beaten the eye of apocalypse");
                        }
                        else if (TUAWorld.ApoMoonDowned)
                        {
                            client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The destruction of the moon");
                            client.UpdateDetails(Main.LocalPlayer.name + " has beaten the apocalypse moon");
                        }
                        else if (TUAWorld.EoCPostMLDowned)
                        {
                            client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The revenge of the eye");
                            client.UpdateDetails(Main.LocalPlayer.name + " has beaten the ultra eye of cthulhu");
                        }
                        else if (NPC.downedMoonlord)
                        {
                            client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "Before the lord, there were the god");
                            client.UpdateDetails(Main.LocalPlayer.name + " has beaten the moon lord");
                        }
                        else if (NPC.downedGolemBoss)
                        {
                            client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The ancient cult of the moon");
                            client.UpdateDetails(Main.LocalPlayer.name + " is before the cultist");
                        }
                        else if (NPC.downedPlantBoss)
                        {
                            client.UpdateLargeAsset(Main.rand.NextBool() ? "Playing TUA" : "The legends of a lizhard divinity");
                            client.UpdateDetails(Main.LocalPlayer.name + " is before the golem");
                        }
                        else if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
                        {
                            client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The jungle arise");
                            client.UpdateDetails(Main.LocalPlayer.name + " is before plantera");
                        }
                        else if (NPC.downedMechBossAny && !(NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3))
                        {
                            client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The mechanical arise");
                            client.UpdateDetails(Main.LocalPlayer.name + " is after the death of a mechanical boss");
                        }
                        else if (Main.hardMode)
                        {
                            client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "A new era of challenge");
                            client.UpdateDetails(Main.LocalPlayer.name + " is in hardmode");
                        }
                        else if (!Main.hardMode)
                        {
                            client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "Preparing for the initial raids");
                            client.UpdateDetails(Main.LocalPlayer.name + " is close to hardmode");
                        }
                        else if (!NPC.downedBoss3)
                        {
                            client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "Trying to help the old man");
                            client.UpdateDetails(Main.LocalPlayer.name + " hasn't entered the Dungeon");
                        }
                        else if (!NPC.downedBoss2)
                        {
                            client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "Preparing to fight the evil");
                            client.UpdateDetails(Main.LocalPlayer.name + " hasn't purged their world");
                        }
                        else if (!NPC.downedBoss1)
                        {
                            client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The start of a new day");
                            client.UpdateDetails(Main.LocalPlayer.name + " is preparing for EoC");
                        }
                    }
                    else
                    {
                        if (NPC.LunarApocalypseIsUp)
                        {
                            client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The dimension started to clash");
                            client.UpdateDetails(Main.LocalPlayer.name + " is fighting the lunar apocalypse");
                        }
                    }


                    // presence.Assets.LargeImageText = Main.rand.NextBool() ? "Playing TUA" : "exploring the wasteland";
                    // presence.Details = Main.LocalPlayer.name + " is currently in the wasteland";
                    // client.SetPresence(presence);
                }
                else
                {
                    presence.Details = $"In Main Menu ({(Environment.Is64BitProcess ? "64" : "32")}bit)";
                    presence.State = (SteamID64Checker.Instance.VerifyDevID() && TUA.devMode)
                        ? "Debugging/Developing" : "Doing nothing";

                    presence.Assets.LargeImageText = "Doing nothing";
                    client.SetPresence(presence);
                }
            }
        }

        public static void Kill()
        {
            client.UpdateEndTime(DateTime.UtcNow);
            client.Dispose();
        }
    }
}
