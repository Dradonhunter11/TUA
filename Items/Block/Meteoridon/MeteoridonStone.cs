using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace TUA.Items.Block.Meteoridon
{
    class MeteoridonStone : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Strange Rock");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 24;
            item.maxStack = 999;
            item.consumable = true;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.autoReuse = true;
            item.createTile = mod.TileType("MeteoridonStone");
        }
    }
}
