using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrariaUltraApocalypse.Tiles.Meteoridon
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
