using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrariaUltraApocalypse
{
    class RecipeManager
    {
        public static void removeRecipe(int itemID) {
            RecipeFinder rf = new RecipeFinder();
            rf.SetResult(itemID);

            foreach (Recipe r in rf.SearchRecipes()) {
                RecipeEditor re = new RecipeEditor(r);
                re.DeleteRecipe();
            }
            
        }

        public static ModRecipe createModRecipe(Mod mod) {
            return new ModRecipe(mod);
        }

        public static void addRecipe(Mod mod, String result, int resultAmount, RecipeForma recipe)
        {
            ModRecipe r = new ModRecipe(mod);
            for (int i = 0; i < recipe.ingredient.Length; i++) {
                r.AddIngredient(mod, recipe.ingredient[i], recipe.number[i]);
            }
            r.SetResult(mod, result, resultAmount);
            r.AddRecipe();
        }
    }
}
