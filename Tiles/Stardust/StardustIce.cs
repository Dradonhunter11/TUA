using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TUA.Tiles.Stardust
{
    class StardustIce : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileMerge[this.Type][ModContent.TileType<StardustRock>()] = true;
            drop = ItemID.DirtBlock;
            AddMapEntry(new Microsoft.Xna.Framework.Color(65, 105, 225));
        }
    }
}