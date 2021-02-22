using Terraria.ModLoader;
using TUA.Dimension.Block;

namespace TUA.Items.Tiles
{
    class MobSpawner_Item : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.maxStack = 999;
            item.consumable = true;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.autoReuse = true;
            item.createTile = ModContent.TileType<MobSpawner>();
        }
    }
}
