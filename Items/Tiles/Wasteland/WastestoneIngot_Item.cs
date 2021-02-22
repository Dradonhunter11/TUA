using Terraria;
using Terraria.ModLoader;
using TUA.API.TerraEnergy.MachineRecipe.Furnace;
using TUA.Tiles.Furniture.Ingots;

namespace TUA.Items.Tiles.Wasteland
{
    class WastestoneIngot_Item : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 24;
            item.useStyle = 1;
            item.value = Item.sellPrice(0, 0, 99, 0);
            item.maxStack = 999;
            item.createTile = ModContent.TileType<WastestoneIngot>();
            item.consumable = true;
        }

        public override void AddRecipes()
        {
            FurnaceRecipe furnace = new FurnaceRecipe(mod);
            furnace.AddIngredient(ModContent.ItemType<WastelandOre_Item>(), 3);
            furnace.SetCostAndCookTime(500);
            furnace.SetResult(ModContent.ItemType<WastestoneIngot_Item>());
            furnace.AddRecipe();
        }
    }
}
