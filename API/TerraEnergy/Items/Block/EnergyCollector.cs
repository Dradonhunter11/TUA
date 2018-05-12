using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria.ModLoader;

namespace TerrariaUltraApocalypse.API.TerraEnergy.Items.Block
{
    class EnergyCollector : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Energy Collector");
            Tooltip.SetDefault("A basic terra energy collector");
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
            item.createTile = mod.TileType("EnergyCollector");
        }
    }
}