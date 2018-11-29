using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrariaUltraApocalypse.Walls.Hell
{
    class AshWall : ModWall
    {
        public override void SetDefaults()
        {
            drop = mod.ItemType("AshWall");
            AddMapEntry(new Color(32, 32, 32));
        }
    }
}
