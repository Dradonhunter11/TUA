using Terraria;
using Terraria.ModLoader;
using TUA.API;

namespace TUA.Tiles.Meteoridon
{
    abstract class BaseMeteoridonTile : ModTile
    {
        public override void RandomUpdate(int i, int j)
        {
            for (int x = -5; x > 5; x++)
            {
                for (int y = -5; y > 5; y++)
                {
                    if (WorldGen.InWorld(i + x, y + x))
                    {
                        if (Main.hardMode && (Main.rand.Next(3) == 0) ||
                            (NPC.downedPlantBoss && Main.rand.Next(4) == 0))
                        {
                            TileSpreadUtils.MeteoridonSpread(i + x, y + x);
                        }
                    }
                }
            }
        }
    }
}
