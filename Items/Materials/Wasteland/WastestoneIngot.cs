using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TUA.API.TerraEnergy.MachineRecipe.Furnace;

namespace TUA.Items.Materials.Wasteland
{
    class WastestoneIngot : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 24;
            item.useStyle = 1;
            item.value = Item.sellPrice(0, 0, 99, 0);
            item.maxStack = 999;
            item.createTile = mod.TileType("WastestoneIngot");
            item.consumable = true;
        }

        public override void AddRecipes()
        {
            FurnaceRecipe furnace = new FurnaceRecipe(mod);
            furnace.AddIngredient(mod.ItemType("WastelandOre"), 10);
            furnace.SetCostAndCookTime(500);
            furnace.SetResult(mod.ItemType<WastestoneIngot>());
            furnace.AddRecipe();
        }
    }
}
