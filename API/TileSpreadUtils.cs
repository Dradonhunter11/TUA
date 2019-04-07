using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TUA.API
{
    internal static class TileSpreadUtils
    {
        public static void MeteoridonSpread(Mod mod, int x, int y)
        {
            Tile tile = Main.tile[x, y];
            if (tile.type == TileID.Stone || tile.type == TileID.Crimstone || tile.type == TileID.Ebonstone)
            {
                TileSpreadUtils.ChangeTile(x, y, mod.TileType("MeteoridonStone"));
            }
            else if (tile.type == TileID.Grass || tile.type == TileID.CorruptGrass || tile.type == TileID.FleshGrass)
            {
                TileSpreadUtils.ChangeTile(x, y, mod.TileType("MeteoridonGrass"));
            }
            else if (tile.type == TileID.Sand || tile.type == TileID.Ebonsand || tile.type == TileID.Crimsand)
            {
                TileSpreadUtils.ChangeTile(x, y, mod.TileType("MeteoridonSand"));
            }
            else if (tile.type == TileID.Sandstone || tile.type == TileID.CorruptSandstone || tile.type == TileID.CrimsonSandstone)
            {
                TileSpreadUtils.ChangeTile(x, y, mod.TileType("MeteoridonSandstone"));
            }
            else if (tile.type == TileID.HardenedSand || tile.type == TileID.CorruptHardenedSand || tile.type == TileID.CrimsonHardenedSand)
            {
                TileSpreadUtils.ChangeTile(x, y, mod.TileType("MeteoridonHardenedSand"));
            }
            else if (tile.type == TileID.IceBlock || tile.type == TileID.CorruptIce || tile.type == TileID.FleshIce)
            {
                TileSpreadUtils.ChangeTile(x, y, mod.TileType("BrownIce"));
            }
        }

        public static void ChangeTile(int x, int y, int tileType)
        {
            Main.tile[x, y].type = (ushort)tileType;
            WorldGen.SquareTileFrame(x, y, true);
            NetMessage.SendTileSquare(-1, x, y, 1);
        }

        public static void ChangeWall(int x, int y, int wallType)
        {
            Main.tile[x, y].type = (ushort) wallType;
            //Add other net sync stuff related to wall if necessary
        }
    }
}
