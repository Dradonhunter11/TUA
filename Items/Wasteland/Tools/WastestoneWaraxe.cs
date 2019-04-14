using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TUA.API.TerraEnergy.MachineRecipe.Furnace;

namespace TUA.Items.Wasteland.Tools
{
    class WastestoneWaraxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eradiated Waraxe");
            Tooltip.SetDefault("Holds the power of 100 fragmented souls");
        }

        public override void SetDefaults()
        {
            item.width = 44;
            item.height = 38;
            item.axe = 12;
            item.hammer = 125;
            item.melee = true;
            item.value = Item.sellPrice(0, 0, 50, 0);
            item.damage = 35;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.crit = 10;
            item.useTime = 30;
            item.useAnimation = 15;
            item.knockBack = 0.7f;
            item.autoReuse = true;
        }

        public override void AddRecipes()
        {
            FurnaceRecipe furnace = new FurnaceRecipe(mod);
            furnace.AddIngredient(mod.ItemType<Materials.Wasteland.WastestoneIngot>(), 15);
            furnace.SetCostAndCookTime(500);
            furnace.SetResult(mod.ItemType<WastestoneWaraxe>());
            furnace.AddRecipe();
        }

        // TODO: debuff
    }
}
