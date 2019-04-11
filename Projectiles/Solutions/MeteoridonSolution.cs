using System;
using Terraria;
using Terraria.ID;
using TUA.API.Default;
using TUA.Tiles.NewBiome.Meteoridon;

namespace TUA.Projectiles.Solutions
{
    public class MeteoridonSolution : TUASolution
    {
        public override string name => "Meteoridon Solution";
        public override int dust => DustID.Sandstorm;
        public override int size => 2;

        public override void Convert(int i, int j)
        {
            for (int k = i - size; k <= i + size; k++)
            {
                for (int l = j - size; l <= j + size; l++)
                {
                    if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < Math.Sqrt(size * size + size * size))
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
