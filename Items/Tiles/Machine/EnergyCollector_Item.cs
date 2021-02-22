using Terraria.ModLoader;
using TUA.API.TerraEnergy.Block.FunctionnalBlock;

namespace TUA.Items.Tiles.Machine
{
    class EnergyCollector_Item : ModItem
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
            item.createTile = ModContent.TileType<EnergyCollector>();
        }
    }
}