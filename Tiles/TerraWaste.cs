using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TUA.Items.Tiles.Machine;

namespace TUA.Tiles
{
    class TerraWaste : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            drop = ModContent.ItemType<TerraWaste_Item>();
            AddMapEntry(Color.SandyBrown);
        }
    }
}
