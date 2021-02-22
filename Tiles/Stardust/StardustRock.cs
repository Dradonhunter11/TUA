using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TUA.Tiles.Stardust
{
    class StardustRock : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileMerge[this.Type][ModContent.TileType<StardustIce>()] = true;
            drop = ItemID.DirtBlock;
            AddMapEntry(new Microsoft.Xna.Framework.Color(65, 105, 225));
        }
    }
}