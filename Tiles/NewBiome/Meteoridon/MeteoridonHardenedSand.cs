using Microsoft.Xna.Framework;
using Terraria;

namespace TUA.Tiles.NewBiome.Meteoridon
{
    class MeteoridonHardenedSand : BaseMeteoridonTile
    {
        public override void SetDefaults()
        {
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            drop = mod.ItemType("MeteoridonHardenedSand");
            AddMapEntry(new Color(186, 127, 217));
        }
    }
}
