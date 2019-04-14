using Terraria.ID;
using System.Collections.Generic;
using Terraria.ModLoader;
using TUA.API;

namespace TUA.Items.Wasteland.Accessories
{
    public abstract class LeadenSoles : TUAModItem
    {
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

        protected override bool CraftingMaterials(out int[] items)
        {
            items = new List<int>
            {

            }.ToArray();
            return true;
        }

        protected override void CraftingConditions(ModRecipe recipe)
        {
            recipe.AddRecipeGroup("IronOre", 15);
            // TODO: Wasteland furnace
            recipe.AddTile(TileID.Anvils);
        }
    }
}
