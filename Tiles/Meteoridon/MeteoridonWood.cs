using Terraria;
using Terraria.ModLoader;

namespace TUA.Tiles.Meteoridon
{
    class MeteoridonWood : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            drop = mod.ItemType("MeteoridonWoodPlank");
            AddMapEntry(new Microsoft.Xna.Framework.Color(255, 120, 55));
        }
    }
}
