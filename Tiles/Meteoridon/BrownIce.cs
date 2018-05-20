using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrariaUltraApocalypse.Tiles.Meteoridon
{
    class BrownIce : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            drop = ItemID.DirtBlock;
            AddMapEntry(new Microsoft.Xna.Framework.Color(255, 100, 35));
            base.SetDefaults();
        }
    }
}
