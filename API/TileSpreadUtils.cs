using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TUA.API
{
    internal class TileSpreadUtils
    {
        public static void MeteoridonSpread(Mod mod, int x, int y)
        {
            Tile tile = Main.tile[x, y];
            if (tile.type == TileID.Stone || tile.type == TileID.Crimstone || tile.type == TileID.Ebonstone)
            {
                Main.tile[x, y].type = (ushort)mod.TileType("MeteoridonStone");
            }
            else if (tile.type == TileID.Grass || tile.type == TileID.CorruptGrass || tile.type == TileID.FleshGrass)
            {
                Main.tile[x, y].type = (ushort)mod.TileType("MeteoridonGrass");
            }
            else if (tile.type == TileID.Sand || tile.type == TileID.Ebonsand || tile.type == TileID.Crimsand)
            {
                Main.tile[x, y].type = (ushort)mod.TileType("MeteoridonSand");
            }
            else if (tile.type == TileID.Sandstone || tile.type == TileID.CorruptSandstone || tile.type == TileID.CrimsonSandstone)
            {
                Main.tile[x, y].type = (ushort)mod.TileType("MeteoridonSandstone");
            }
            else if (tile.type == TileID.HardenedSand || tile.type == TileID.CorruptHardenedSand || tile.type == TileID.CrimsonHardenedSand)
            {
                Main.tile[x, y].type = (ushort)mod.TileType("MeteoridonHardenedSand");
            }
            else if (tile.type == TileID.IceBlock || tile.type == TileID.CorruptIce || tile.type == TileID.FleshIce)
            {
                Main.tile[x, y].type = (ushort)mod.TileType("BrownIce");
            }
        }
    }
}
