using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TUA.API.TerraEnergy.Block
{
    class TerraWaste : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            drop = mod.ItemType("TerraWaste");
            AddMapEntry(Color.SandyBrown);
        }
    }
}
