using System;
using System.Linq;
using DiscordRPC;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;
using TUA.API.Dev;
using TUA.API.EventManager;

namespace TUA.Discord
{
    public static class DRPSystem
    {
        private static RichPresence _presence;

        private static DiscordRpcClient _client;

        // public static DiscordRpcClient_client => _client;

        private static string _currentState;

        public static void Boot()
        {
	        UnifiedRandom rand = new UnifiedRandom();
            _currentState = Main.netMode == 0
                ? rand.Next(new string[] { "Playing Alone", "Lone Samurai", "Singleplayer" })
                : rand.Next(new string[] { "Playing With Friends", "Multiplayer" });

            _presence = new RichPresence()
            {
                Details = $"In Main Menu ({(Environment.Is64BitProcess ? "64" : "32")}bit)",
                State = (SteamID64Checker.Instance.VerifyDevID() && TUA.devMode)
                    ? "Debugging/Developing" : _currentState,
                Assets = new Assets()
                {
                    LargeImageKey = "logo",
                    LargeImageText = "Loading mods"
                }
            };
            if (Main.netMode != 0)
            {
                _presence.Party = new Party()
                {
                    Size = Main.ActivePlayersCount,
                    Max = Main.maxNetPlayers
                };
            }
            _client = new DiscordRpcClient("528086919670792233", SteamID64Checker.currentSteamID64, true, -1);
            // _client.OnError += (sender, args) => { TUA.instance.Logger.ErrorFormat("Rich Presence failed. Code {1}, {0}", args.Message, args.Code); };
            _presence.Timestamps = new Timestamps()
            {
                Start = DateTime.UtcNow,
            };
            _client.Initialize();
            _client.SetPresence(_presence);
        }

        // We should get some images pertaining to each boss
        //_client.UpdateLargeAsset("EoC logo", Main.rand.NextBool() ? "Playing TUA" : "The start of a new day");
        public static void Update()
        {
	        if (_client == null)
		        return;

            // Runs through all of discord-rpc's logging stuff, basically
            _client.Invoke();


            _presence.Assets.LargeImageKey = "logo";
            if (!Main.gameMenu)
            {
                _presence.Details = "Playing Terraria";
				
				if (!Main.npc.Any(i => i.boss) && !MoonEventManagerWorld.moonEventList.Any(i => i.Value.IsActive))
                {
                    if (TUAWorld.EoADowned)
                    {
                       _client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The death of a god");
                       _client.UpdateDetails(Main.LocalPlayer.name + " has beaten the eye of apocalypse");
                    }
                    else if (TUAWorld.ApoMoonDowned)
                    {
                       _client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The destruction of the moon");
                       _client.UpdateDetails(Main.LocalPlayer.name + " has beaten the apocalypse moon");
                    }
                    else if (TUAWorld.EoCPostMLDowned)
                    {
                       _client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The revenge of the eye");
                       _client.UpdateDetails(Main.LocalPlayer.name + " has beaten the ultra eye of cthulhu");
                    }
                    else if (NPC.downedMoonlord)
                    {
                       _client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "Before the lord, there were the god");
                       _client.UpdateDetails(Main.LocalPlayer.name + " has beaten the moon lord");
                    }
                    else if (NPC.downedGolemBoss)
                    {
                       _client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The ancient cult of the moon");
                       _client.UpdateDetails(Main.LocalPlayer.name + " is before the cultist");
                    }
                    else if (NPC.downedPlantBoss)
                    {
                       _client.UpdateLargeAsset(Main.rand.NextBool() ? "Playing TUA" : "The legends of a lizhard divinity");
                       _client.UpdateDetails(Main.LocalPlayer.name + " is before the golem");
                    }
                    else if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
                    {
                       _client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The jungle arise");
                       _client.UpdateDetails(Main.LocalPlayer.name + " is before plantera");
                    }
                    else if (NPC.downedMechBossAny && !(NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3))
                    {
                       _client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The mechanical arise");
                       _client.UpdateDetails(Main.LocalPlayer.name + " is after the death of a mechanical boss");
                    }
                    else if (Main.hardMode)
                    {
                       _client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "A new era of challenge");
                       _client.UpdateDetails(Main.LocalPlayer.name + " is in hardmode");
                    }
                    else if (!Main.hardMode)
                    {
                       _client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "Preparing for the initial raids");
                       _client.UpdateDetails(Main.LocalPlayer.name + " is close to hardmode");
                    }
                    else if (!NPC.downedBoss3)
                    {
                       _client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "Trying to help the old man");
                       _client.UpdateDetails(Main.LocalPlayer.name + " hasn't entered the Dungeon");
                    }
                    else if (!NPC.downedBoss2)
                    {
                       _client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "Preparing to fight the evil");
                       _client.UpdateDetails(Main.LocalPlayer.name + " hasn't purged their world");
                    }
                    else if (!NPC.downedBoss1)
                    {
                       _client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The start of a new day");
                       _client.UpdateDetails(Main.LocalPlayer.name + " is preparing for EoC");
                    }
                }
                else
                {
                    if (Main.npc.Any(i => i.type == TUA.instance.NPCType("Eye_of_Apocalypse")))
                    {
                       _client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The incarnated destruction");
                       _client.UpdateDetails(Main.LocalPlayer.name + " is fighting the eye of azathoth - god of destruction");
                    }
                    else if (Main.npc.Any(i => i.type == TUA.instance.NPCType("UEoC")))
                    {
                       _client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The fallen eyes");
                       _client.UpdateDetails(Main.LocalPlayer.name + " is fighting the Ultra Eye of Cthulhu");
                    }
                    else if (NPC.LunarApocalypseIsUp)
                    {
                       _client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The dimension started to clash");
                       _client.UpdateDetails(Main.LocalPlayer.name + " is fighting the lunar apocalypse");
                    }
                    else if (Main.npc.Any(i => i.type == NPCID.MoonLordCore))
                    {
                       _client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The lord of the final frontier");
                       _client.UpdateDetails(Main.LocalPlayer.name + " is fighting the Moon Lord");
                    }
                    else if (Main.npc.Any(i => i.type == NPCID.CultistBoss))
                    {
                       _client.UpdateLargeAsset(null, "The psychotic ritual");
                       _client.UpdateDetails(Main.LocalPlayer.name + " is fighting the Lunatic Cultist");
                    }
                    else if (Main.npc.Any(i => i.type == NPCID.Golem))
                    {
                       _client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The lizhard divinity");
                       _client.UpdateDetails(Main.LocalPlayer.name + " is fighting the Golem");
                    }
                    else if (Main.npc.Any(i => i.type == NPCID.Plantera))
                    {
                       _client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The jungle terror");
                       _client.UpdateDetails(Main.LocalPlayer.name + " is fighting Plantera");
                    }
                    else if (Main.npc.Any(i => i.type == NPCID.TheDestroyer))
                    {
                       _client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The mechanical worm");
                       _client.UpdateDetails(Main.LocalPlayer.name + " is fighting the destroyer");
                    }
                    else if (Main.npc.Any(i => i.type == NPCID.Retinazer) || (Main.npc.Any(i => i.type == NPCID.Spazmatism)))
                    {
                       _client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The mechanical eyes");
                       _client.UpdateDetails(Main.LocalPlayer.name + " is fighting the Twins");
                    }
                    else if (Main.npc.Any(i => i.type == NPCID.SkeletronPrime))
                    {
                       _client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The mechanical skeleton");
                       _client.UpdateDetails(Main.LocalPlayer.name + " is fighting Skeletron Prime");
                    }
                    else if (Main.npc.Any(i => i.type == NPCID.WallofFlesh))
                    {
                       _client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The great wall made of flesh!");
                       _client.UpdateDetails(Main.LocalPlayer.name + " is fighting the Wall of Flesh");
                    }
                    else if (Main.npc.Any(i => i.type == TUA.instance.NPCType("HeartOfTheWasteland") && i.boss)
                    ) //There is a chance the the heart will be asleep, so making sure
                    {
                       _client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The ruined amalgamate");
                       _client.UpdateDetails(Main.LocalPlayer.name + " is fighting the Heart of the Wasteland");
                    }
                    else if (Main.npc.Any(i => i.type == NPCID.SkeletronHead))
                    {
                       _client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The cursed man");
                       _client.UpdateDetails(Main.LocalPlayer.name + " is fighting Skeletron");
                    }
                    else if (Main.npc.Any(i => i.type == NPCID.QueenBee))
                    {
                       _client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "NOT THE BEES");
                       _client.UpdateDetails(Main.LocalPlayer.name + " is fighting the Queen Bee");
                    }
                    else if (Main.npc.Any(i => i.type == NPCID.EaterofWorldsHead))
                    {
                       _client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The corrupted abomination");
                       _client.UpdateDetails(Main.LocalPlayer.name + " is fighting the Eater of the World");
                    }
                    else if (Main.npc.Any(i => i.type == NPCID.BrainofCthulhu))
                    {
                       _client.UpdateLargeAsset(null, Main.rand.NextBool() ? "Playing TUA" : "The bloody brain");
                       _client.UpdateDetails(Main.LocalPlayer.name + " is fighting the Brain of Cthulhu");
                    }
                }
                


                // presence.Assets.LargeImageText = Main.rand.NextBool() ? "Playing TUA" : "exploring the wasteland";
                // presence.Details = Main.LocalPlayer.name + " is currently in the wasteland";
                //_client.SetPresence(presence);
            }
            else
            {
                _presence.Details = $"In Main Menu ({(Environment.Is64BitProcess ? "64" : "32")}bit)";
                _presence.State = (SteamID64Checker.Instance.VerifyDevID() && TUA.devMode)
                    ? "Debugging/Developing" : "Doing nothing";

                _presence.Assets.LargeImageText = "Doing nothing";
                _client.SetPresence(_presence);
            }

        }

        public static void Kill()
        {
            _client.UpdateEndTime(DateTime.UtcNow);
            _client.Dispose();
        }
    }
}
