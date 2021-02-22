using Terraria;
using Terraria.ID;

namespace TUA.Tiles.Meteoridon
{
    class MeteoridonStone : BaseMeteoridonTile
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
