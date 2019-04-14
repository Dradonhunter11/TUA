using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TUA.API.TerraEnergy.MachineRecipe.Furnace;

namespace TUA.Items.Wasteland.Tools
{
    class ToxicPick : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Toxic Pick");
            Tooltip.SetDefault("A pickaxe created from the most malignant of materials " +
                "\nto mine the most malignant of metals");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.pick = 100;
            item.melee = true;
            item.value = Item.sellPrice(0, 0, 50, 0);
            item.damage = 35;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.crit = 10;
            item.useTime = 30;
            item.useAnimation = 15;
            item.knockBack = 0.7f;
            item.autoReuse = true;
            item.useTurn = true;
        }

        public override void AddRecipes()
        {
            FurnaceRecipe furnace = new FurnaceRecipe(mod);
            furnace.AddIngredient(mod.ItemType<Materials.Wasteland.WastestoneIngot>(), 20);
            furnace.SetCostAndCookTime(500);
            furnace.SetResult(mod.ItemType<ToxicPick>());
            furnace.AddRecipe();
        }

        // TODO: debuff
    }
}
