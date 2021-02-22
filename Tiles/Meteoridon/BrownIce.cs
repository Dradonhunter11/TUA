using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TUA.Tiles.Meteoridon
{
    class BrownIce : BaseMeteoridonTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            TileID.Sets.Conversion.Ice[Type] = true;
            drop = ModContent.TileType<BrownIce>();
            AddMapEntry(new Microsoft.Xna.Framework.Color(255, 100, 35));
            base.SetDefaults();
        }
    }
}
