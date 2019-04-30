using Terraria;
using Terraria.ID;
using TUA.API;

namespace TUA.Items.Spells.BiomeSpell
{
    public static class Terraformer
    {
        public static void Terraform(int x, int y, byte wall, ushort dirt, ushort stone)
        {
            Tile tile = Main.tile[x, y];
            if (tile.wall == WallID.Dirt || WallID.Sets.Corrupt[tile.wall] || WallID.Sets.Crimson[tile.wall])
            {
                TileSpreadUtils.ChangeWall(x, y, wall);
            }

            if (tile.type == TileID.Dirt || TileID.Sets.Conversion.Grass[tile.type] 
                || tile.type == TileID.SnowBlock || TileID.Sets.Conversion.Sand[tile.type])
            {
                TileSpreadUtils.ChangeTile(x, y, dirt);
            }

            if (TileID.Sets.Conversion.Stone[tile.type] 
                || TileID.Sets.Conversion.Sandstone[tile.type] || TileID.Sets.Conversion.HardenedSand[tile.type])
            {
                TileSpreadUtils.ChangeTile(x, y, stone);
            }
        }
    }
}
