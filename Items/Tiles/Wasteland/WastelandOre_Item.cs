using Terraria.ModLoader;
using TUA.Tiles.Wasteland;

namespace TUA.Items.Tiles.Wasteland
{
    class WastelandOre_Item : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wastestone");
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
            item.createTile = ModContent.TileType<WastelandOre>();
        }
    }
}
