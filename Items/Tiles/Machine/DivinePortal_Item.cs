using Terraria.ModLoader;
using TUA.API;
using TUA.Tiles.Machine;

namespace TUA.Items.Tiles.Machine
{
    class DivinePortal_Item : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The god summoner");
            Tooltip.SetDefault("Access god from another realm...");
        }

        public override void SetDefaults()
        {
            item.width = 128;
            item.height = 128;
            item.maxStack = 999;
            item.consumable = true;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.autoReuse = true;
            item.createTile = ModContent.TileType<DivinePortal>();
        }
    }
}
