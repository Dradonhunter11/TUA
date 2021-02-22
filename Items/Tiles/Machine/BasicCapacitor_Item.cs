using Terraria.ModLoader;
using TUA.API.TerraEnergy.Block.FunctionnalBlock;

namespace TUA.Items.Tiles.Machine
{
    class BasicCapacitor_Item : ModItem
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
            item.createTile = ModContent.TileType<BasicTECapacitor>();
        }
    }
}
