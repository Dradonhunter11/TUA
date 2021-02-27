using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using TUA.API.TerraEnergy.Block.FunctionnalBlock;

namespace TUA.Items.Tiles.Machine
{
    class AdvancedCapacitor_Item : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Advanced TE capacitor");
            Tooltip.SetDefault("An advanced terra energy capacitor");
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
            item.createTile = ModContent.TileType<AdvancedTECapacitor>();
        }
    }
}
