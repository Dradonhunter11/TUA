using BiomeLibrary;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrariaUltraApocalypse.Dimension.World
{
    class SolarWorld : ModWorld
    {
        int solarRainCooldown = 300;
        public override void PostUpdate()
        {
            TUAPlayer plr = Main.LocalPlayer.GetModPlayer<TUAPlayer>();
            if (plr.currentDimension == "solar")
            {
                Player p = Main.LocalPlayer;

                if (solarRainCooldown == 0)
                {
                    Vector2 spawnPosition = new Vector2(p.Center.X + Main.rand.NextFloat(-300f, 300f), p.Center.Y - 400f);
                    NPC.NewNPC((int)spawnPosition.X, (int)spawnPosition.Y, NPCID.SolarFlare);
                    spawnPosition = new Vector2(p.Center.X + Main.rand.NextFloat(-300f, 300f), p.Center.Y - 400f);
                    NPC.NewNPC((int)spawnPosition.X, (int)spawnPosition.Y, NPCID.SolarFlare);
                    spawnPosition = new Vector2(p.Center.X + Main.rand.NextFloat(-300f, 300f), p.Center.Y - 400f);
                    NPC.NewNPC((int)spawnPosition.X, (int)spawnPosition.Y, NPCID.SolarFlare);
                    solarRainCooldown = 150;
                }
                solarRainCooldown--;
            }
        }
    }
}