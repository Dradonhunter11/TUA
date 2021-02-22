using Terraria.ModLoader;
using TUA.API;
using TUA.API.TerraEnergy.Block.FunctionnalBlock;

namespace TUA.Items.Tiles.Machine
{
    class TerraForge_Item : TUAModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Terra Forge");
            Tooltip.SetDefault("A machine that can forge metal");
        }

        public override void SetDefaults()
        {

            item.width = 96;
            item.height = 152;
            item.maxStack = 999;
            item.consumable = true;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.autoReuse = true;
            item.createTile = ModContent.TileType<TerraForge>();
        }
    }
}

