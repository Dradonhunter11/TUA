using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TUA
{
    class RecipeUtils
    {
        private static List<Recipe> removedRecipes = new List<Recipe>();

        public static void removeRecipe(int itemID)
        {
            RecipeFinder rf = new RecipeFinder();
            rf.SetResult(itemID);

            foreach (Recipe r in rf.SearchRecipes())
            {
                RecipeEditor re = new RecipeEditor(r);
                re.DeleteRecipe();
            }

        }
        public static void AddRecipe(Mod mod, List<Tuple<int, int>> ingredient, Tuple<int, int> result)
        {
            ModRecipe r = new ModRecipe(mod);
            foreach (var tuple in ingredient)
            {
                r.AddIngredient(tuple.Item1, tuple.Item2);
            }
            r.SetResult(result.Item1, result.Item2);
            r.AddRecipe();
        }

        public static void GetAllRecipeByIngredientAndReplace(int ingredientToReplace, int replacingIngredient)
        {
            RecipeFinder rf = new RecipeFinder();
            rf.AddIngredient(ingredientToReplace);

            foreach (Recipe r in rf.SearchRecipes())
            {
                Recipe recipe = r;
                RecipeEditor re = new RecipeEditor(recipe);
                
                if (re.DeleteIngredient(ingredientToReplace))
                {
                    re.AddIngredient(replacingIngredient);
                    Main.recipe[Recipe.numRecipes] = r;
                    Recipe.numRecipes++;
                }
            }
        }


        public static void setAllFurnaceRecipeSystem()
        {
            RecipeFinder rf = new RecipeFinder();
            rf.AddTile(TileID.Furnaces);

            foreach (Recipe r in rf.SearchRecipes())
            {
                Recipe recipe = r;
                if (recipe.requiredItem.Length == 1)
                {
                    TerrariaUltraApocalypse.instance.addFurnaceRecipe(recipe.requiredItem[0].type, recipe.createItem.type, 20);
                    removedRecipes.Add(r);
                    RecipeEditor re = new RecipeEditor(r);
                    re.DeleteRecipe();
                }
            }
        }

        public static void setBackAllRecipe()
        {
            foreach (var recipe in removedRecipes)
            {
                
            }
        }

    }
}
