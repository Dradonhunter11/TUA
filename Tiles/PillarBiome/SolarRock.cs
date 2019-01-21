using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TUA.Tiles.PillarBiome
{
    class SolarRock : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            
            drop = ItemID.DirtBlock;
            AddMapEntry(new Microsoft.Xna.Framework.Color(178, 98, 0));
        }
    }
}
