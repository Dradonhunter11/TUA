using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TUA.Tiles.NewBiome.Meteoridon
{
    class MeteoridonStone : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            TileID.Sets.Conversion.Stone[Type] = true; 
            drop = ItemID.DirtBlock;
            AddMapEntry(new Microsoft.Xna.Framework.Color(255, 120, 55));
        }
    }
}
