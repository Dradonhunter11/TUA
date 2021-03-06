﻿using Terraria.ModLoader;
using TUA.Tiles.Meteoridon;

namespace TUA.Items.Tiles.Meteoridon
{
    class MeteoridonSandstone_Item : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Strange sandstone");
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
            item.createTile = ModContent.TileType<MeteoridonSandstone>();
        }
    }
}
