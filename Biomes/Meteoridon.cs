using Terraria;
using Terraria.ModLoader;

namespace TerrariaUltraApocalypse.Biomes
{
    class Meteoridon : ModWorld
    {
        public static int meteoridonTile = 0;


        public override void ResetNearbyTileEffects()
        {
            meteoridonTile = 0;
        }

        public override void TileCountsAvailable(int[] tileCounts)
        {
            int tempCount = tileCounts[mod.TileType("BrownIce")];
            tempCount += tileCounts[mod.TileType("MeteoridonGrass")];
            tempCount += tileCounts[mod.TileType("MeteoridonStone")];
            meteoridonTile = tempCount;
        }
    }
}
