using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TUA.API.Dev;

namespace TUA
{
    class TUAPlayer : ModPlayer
    {

        public static bool AugmendVortex = false;

        public static bool arenaActive = false;

        public bool noImmunityDebuff;

        public string ID;

        public bool IsScreenLocked = false;
        public Vector2 PositionLock = Vector2.Zero;

        private static int splashTimer = 0;

        public override void Initialize()
        {
            ID = new Guid().ToString();
        }

        public override void Load(TagCompound tag)
        {
            if (tag.GetString("GUID") is string str && !string.IsNullOrEmpty(str))
            {
                ID = str;
            }
        }

        public override TagCompound Save()
        {
            return new TagCompound
            {
                ["GUID"] = ID
            };
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (arenaActive)
            {
                arenaActive = false;
                switch (Main.rand.Next(3))
                {
                    case 0:
                        damageSource = PlayerDeathReason.ByCustomReason($"Hey, {player.name}. You think he was more powerful than the God of Destruction... really?");
                        break;
                    case 1:
                        damageSource = PlayerDeathReason.ByCustomReason($"Ouch, looks like {player.name} thought {(player.Male ? "he" : "she")} mistakenly thought they could take on the enemy.");
                        break;
                    case 2:
                        damageSource = Main.player.Length > 1 ? PlayerDeathReason.ByCustomReason($"HAHAHA, can we get a RIP in the chat for {player.name}!?")
                            : PlayerDeathReason.ByCustomReason($"Damn {player.name}, you really are a fool!");
                        break;
                }
            }
            if (Dimlibs.Dimlibs.getPlayerDim() == "solar")
            {
                Main.WorldPath = Main.SavePath + "/World/solar";
            }
            else if (Dimlibs.Dimlibs.getPlayerDim() == "overworld")
            {
                Main.WorldPath = Main.SavePath + "/World";
            }
            return true;
        }

        public override void UpdateBiomeVisuals()
        {
            if (Dimlibs.Dimlibs.getPlayerDim() != null) {
                bool inSolar = Dimlibs.Dimlibs.getPlayerDim() == "Solar";
                player.ManageSpecialBiomeVisuals("TUA:TUAPlayer", false, player.Center);
                bool inStardust = Dimlibs.Dimlibs.getPlayerDim() == "Stardust";
                player.ManageSpecialBiomeVisuals("TUA:StardustPillar", inStardust, player.Center);
            }
        }

        public override void PreUpdate()
        {
            if (noImmunityDebuff)
            {
                player.immune = false;
                player.immuneTime = -1;
            }
        }

        public override void UpdateDead()
        {
            if (SteamID64Checker.Instance.VerifyDevID() && TUA.devMode)
            {
                player.respawnTimer = 1; //for faster respawn while debugging
            }
        }

        public override void ModifyScreenPosition()
        {
            if (IsScreenLocked)
            {
                Main.screenPosition = PositionLock;
            }
        }

        internal static void LockPlayerCamera(Vector2? position, bool lockCamera)
        {
            TUAPlayer player = Main.LocalPlayer.GetModPlayer<TUAPlayer>();

            if (!lockCamera || !position.HasValue)
            {
                player.IsScreenLocked = false;
                return;
            }

            
            if (Main.netMode == 2)
            {
                foreach (Player p in Main.player)
                {
                    if (p == null)
                    {
                        break;
                    }
                    player = p.GetModPlayer<TUAPlayer>();
                    player.PositionLock = position.Value;
                    player.IsScreenLocked = lockCamera;
                    NetMessage.SendData(MessageID.SyncPlayer, -1, 1);
                }
                return;
            }
            player = Main.LocalPlayer.GetModPlayer<TUAPlayer>();
            player.PositionLock = position.Value;
            player.IsScreenLocked = true;
        }
    }
}
