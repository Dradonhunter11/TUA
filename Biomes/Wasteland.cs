using BiomeLibrary.API;
using Microsoft.Xna.Framework;
using Terraria;

namespace TUA.Biomes
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
