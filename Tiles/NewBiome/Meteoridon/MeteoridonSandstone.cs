using Microsoft.Xna.Framework;
using Terraria;

namespace TUA.Tiles.NewBiome.Meteoridon
{
    class MeteoridonSandstone : BaseMeteoridonTile
    {
        public override void SetDefaults()
        {
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            drop = mod.ItemType("MeteoridonSandstone");
            AddMapEntry(new Color(87, 53, 113));
        }
    }
}
