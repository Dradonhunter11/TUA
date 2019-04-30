using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

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
