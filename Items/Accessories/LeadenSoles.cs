using Terraria.ID;
using Terraria.ModLoader;
using TUA.API.TerraEnergy.MachineRecipe.Furnace;

namespace TUA.Items.Accessories
{
    public class LeadenSoles : ModItem
    {
        public override bool Autoload(ref string name)
        {
            return false;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Leaden Soles");
            Tooltip.SetDefault("These will enabled the wearer to tread " +
                "\non the most dangerous metals.");
        }

        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.autoReuse = true;
            item.maxStack = 999;
            item.consumable = true;
            item.createTile = 22;
            item.width = 12;
            item.height = 12;
            item.rare = 1;
            item.value = 4000;
        }

        public override void AddRecipes()
        {
            FurnaceRecipe furnace = new FurnaceRecipe(mod);
            furnace.AddIngredient(ItemID.LeadOre, 25);
            furnace.SetCostAndCookTime(500);
            furnace.SetResult(ModContent.ItemType<LeadenSoles>());
            furnace.AddRecipe();

            furnace = new FurnaceRecipe(mod);
            furnace.AddIngredient(ItemID.IronOre, 26);
            furnace.SetCostAndCookTime(500);
            furnace.SetResult(ModContent.ItemType<LeadenSoles>());
            furnace.AddRecipe();
        }
    }
}
