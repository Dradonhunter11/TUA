using Terraria.ModLoader;
using TUA.API.TerraEnergy.Block.FunctionnalBlock;

namespace TUA.Items.Tiles.Machine
{
    class TerraFurnace_Item : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Terra Furnace");
            Tooltip.SetDefault("The furnace you want to use!");
        }

        public override void SetDefaults()
        {
            item.width = 60;
            item.height = 42;
            item.maxStack = 999;
            item.consumable = true;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.autoReuse = true;
            item.createTile = ModContent.TileType<TerraFurnace>();
        }

    }
}
