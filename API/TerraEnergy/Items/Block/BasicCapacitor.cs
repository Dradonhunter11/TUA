using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace TUA.API.TerraEnergy.Items.Block
{
    class BasicCapacitor : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Basic TE storage");
            Tooltip.SetDefault("A basic terra energy capacitor");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 48;
            item.maxStack = 999;
            item.consumable = true;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.autoReuse = true;
            item.createTile = mod.TileType("BasicTECapacitor");
        }
    }
}
