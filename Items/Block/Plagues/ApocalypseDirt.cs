using TUA.API;
using TUA.Tiles.Furniture.Coins;


namespace TUA.Items.Block.Plagues
{
    class ApocalypseDirt : TUAModLegacyItem
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
            //item.createTile = mod.TileType("ApocalypseDirt");
            item.createTile = mod.TileType<DiamondCoins>();
        }
    }
}
