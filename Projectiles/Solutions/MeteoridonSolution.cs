using System;
using Terraria;
using Terraria.ID;
using TUA.API.Default;
using TUA.Tiles.NewBiome.Meteoridon;

namespace TUA.Projectiles.Solutions
{
    public class MeteoridonSolution : TUASolution
    {
        public override string SolutionName => "Meteoridon Solution";
        public override int Dust => DustID.Sandstorm;
        public override int Size => 2;

        public override void Convert(int i, int j)
        {
            for (int k = i - Size; k <= i + Size; k++)
            {
                for (int l = j - Size; l <= j + Size; l++)
                {
                    if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < Math.Sqrt(Size * Size + Size * Size))
                    {
                        int type = (int)Main.tile[k, l].type;
                        if (TileID.Sets.Conversion.Stone[type])
                        {
                            Main.tile[k, l].type = (ushort)mod.TileType<MeteoridonStone>();
                            WorldGen.SquareTileFrame(k, l, true);
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        }
                        else if (TileID.Sets.Conversion.Grass[type])
                        {
                            Main.tile[k, l].type = (ushort)mod.TileType<MeteoridonGrass>();
                            WorldGen.SquareTileFrame(k, l, true);
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        }
                        else if (TileID.Sets.Conversion.Ice[type])
                        {
                            Main.tile[k, l].type = (ushort)mod.TileType<BrownIce>();
                            WorldGen.SquareTileFrame(k, l, true);
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        }
                        else if (TileID.Sets.Conversion.Sand[type])
                        {
                            Main.tile[k, l].type = (ushort)mod.TileType<MeteoridonSand>();
                            WorldGen.SquareTileFrame(k, l);
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        }
                        else if (TileID.Sets.Conversion.HardenedSand[type])
                        {
                            Main.tile[k, l].type = (ushort)mod.TileType<MeteoridonHardenedSand>();
                            WorldGen.SquareTileFrame(k, l);
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        }
                        else if (TileID.Sets.Conversion.Sandstone[type])
                        {
                            Main.tile[k, l].type = (ushort)mod.TileType<MeteoridonSandstone>();
                            WorldGen.SquareTileFrame(k, l);
                            NetMessage.SendTileSquare(-1, k, l, 1);
                        }
                    }
                }
            }
        }
    }
}
