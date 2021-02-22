using Microsoft.Xna.Framework;
using Terraria;
using TUA.API;
using TUA.API.Default;

namespace TUA.Tiles.Meteoridon
{
    class MeteoridonSand : TUAFallingBlock
    {
        public override int ItemDropID => mod.ItemType("MeteoridonSand");
        public override int ItemProjectileID => mod.ProjectileType("MeteoridonSandProjectile");
        public override bool SandTile => true;
        public override Color MapColor => Color.DarkOrange;

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
                            TileSpreadUtils.MeteoridonSpread( i + x, y + x);
                        }
                    }
                }
            }
        }
    }
}
