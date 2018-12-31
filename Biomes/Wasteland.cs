using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiomeLibrary.API;
using Microsoft.Xna.Framework;
using Terraria;

namespace TerrariaUltraApocalypse.Biomes
{
    class Wasteland : ModBiome
    {
        public override bool Condition()
        {
            Vector2 playerPos = Main.LocalPlayer.Center / 16;
            return Main.ActiveWorldFileData.HasCrimson && playerPos.Y < Main.maxTilesY - 200;
        }
    }
}
