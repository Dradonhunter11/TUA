using Terraria.ModLoader;
using TUA.Tiles.Plagues;

namespace TUA.Items.Tiles.Plagues
{
    class ApocalypseDirt_Item : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Apocalypse Dirt");
            Tooltip.SetDefault("You cheated to obtain this :P");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 999;
            item.consumable = true;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.autoReuse = true;
            //item.createTile = mod.TileType("ApocalypseDirt_Item");
            item.createTile = ModContent.TileType<ApocalypseDirt>();
        }
    }
}
