using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TerrariaUltraApocalypse.Tiles.EoA
{
    class Arena : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            this.minPick = 99999;
            AddMapEntry(new Microsoft.Xna.Framework.Color(0, 0, 0));
        }
    }
}
