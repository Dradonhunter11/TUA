using Terraria;
using Terraria.ModLoader;

namespace TUA.Items.Walls.Hell
{
    class AshWall : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ash wall");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.placeStyle = 1;
            item.value = Item.sellPrice(0, 0, 0, 0);
            item.useTime = 7;
            item.useStyle = 1;
            item.consumable = true;
            item.useTurn = true;
            item.autoReuse = true;
            item.maxStack = 999;
            item.createWall = mod.WallType("AshWall");  
        }
    }
}
