﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TUA.API.Dev;

namespace TUA
{
    public class TUAPlayer : ModPlayer
    {
        public static bool augmentVortex = false;
        public static bool arenaActive = false;
        public bool noImmunityDebuff;
        public bool isScreenLocked = false;
        public Vector2 positionLock = Vector2.Zero;
        // private static int splashTimer = 0;
        public static Vector2 initialPoint = Vector2.Zero;
        public static Vector2 endPoint = Vector2.Zero;

        #region LoreBook Trigger
        public bool UnlockedHell;
        public bool UnlockedWasteland;
        public bool UnlockedUltraBoss;
        public bool UnlockedTraveller;
        public bool UnlockedWoF;
        public bool UnlockedHotW;
        #endregion

        public override void Initialize()
        {
            augmentVortex = false;
            arenaActive = false;
            noImmunityDebuff = false;
            isScreenLocked = false;
            positionLock = Vector2.Zero;
            // splashTimer = 0;
            initialPoint = Vector2.Zero;
            endPoint = Vector2.Zero;
            ID = new Guid();
        }

        public override void Load(TagCompound tag)
        {
            if (tag.GetString("GUID") is string str && !string.IsNullOrWhiteSpace(str))
            {
                ID = Guid.Parse(str);
            }
        }

        public override TagCompound Save()
        {
            return new TagCompound
            {
                ["GUID"] = ID.ToString()
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
                        damageSource = PlayerDeathReason.ByCustomReason($"Hey, {player.name}. You really thought you were more powerful than the God of Destruction... really?");
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

            return true;
        }

        public override void UpdateBiomeVisuals()
        {
            bool inSolar = Dimension.DimensionUtil.InSolar;
            player.ManageSpecialBiomeVisuals("TUA:TUAPlayer", inSolar, player.Center);

            bool inStardust = Dimension.DimensionUtil.InStardust;
            player.ManageSpecialBiomeVisuals("TUA:StardustPillar", inStardust, player.Center);
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
                player.respawnTimer = 1; //for faster respawn while debugging
        }

        public override void ModifyScreenPosition()
        {
            if (isScreenLocked)
                Main.screenPosition = positionLock;
        }

        public static void LockPlayerCamera(Vector2? position, bool lockCamera)
        {
            TUAPlayer player = Main.LocalPlayer.GetModPlayer<TUAPlayer>();

            if (!lockCamera || !position.HasValue)
            {
                player.isScreenLocked = false;
                return;
            }

            if (Main.netMode == NetmodeID.Server)
            {
                foreach (Player p in Main.player)
                {
                    if (p == null) break;

                    player = p.GetModPlayer<TUAPlayer>();
                    player.positionLock = position.Value;
                    player.isScreenLocked = lockCamera;

                    NetMessage.SendData(MessageID.SyncPlayer, -1, 1);
                }

                return;
            }

            player = Main.LocalPlayer.GetModPlayer<TUAPlayer>();
            player.positionLock = position.Value;
            player.isScreenLocked = true;
        }

        public Guid ID { get; private set; }
    }
}
