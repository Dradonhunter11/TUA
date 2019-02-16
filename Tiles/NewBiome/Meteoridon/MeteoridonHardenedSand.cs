using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TUA.Tiles.NewBiome.Meteoridon
{
    class MeteoridonHardenedSand : ModTile
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
