using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TUA.Items.Meteoridon.Weapons
{
    class MeteoridonSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Space Force");
            Tooltip.SetDefault("The strength of a thousand light year of travelling");
            Tooltip.SetDefault("The true melee power");
        }

        public override void SetDefaults()
        {
            item.width = 52;
            item.height = 56;
            item.damage = 70;
            item.melee = true;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.useStyle = 1;
            item.useTime = 14;
            item.useAnimation = 20;
            item.maxStack = 1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("MeteoridonBar"), 14);
            recipe.AddIngredient(mod.GetItem("MeteorideScale"), 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
